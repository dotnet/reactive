// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the Apache 2.0 License.
// See the LICENSE file in the project root for more information. 

using System.Reactive.Concurrency;
using System.Reactive.Disposables;
using System.Threading;

namespace System.Reactive
{
    using Collections.Concurrent;
    using Diagnostics;

    internal class ScheduledObserver<T> : ObserverBase<T>, IScheduledObserver<T>
    {
        private int _state;
        private const int Stopped = 0;
        private const int Running = 1;
        private const int Pending = 2;
        private const int Faulted = 9;
        private readonly ConcurrentQueue<T> _queue = new ConcurrentQueue<T>();
        private bool _failed;
        private Exception _error;
        private bool _completed;
        private readonly IObserver<T> _observer;
        private readonly IScheduler _scheduler;
        private readonly ISchedulerLongRunning _longRunning;
        private IDisposable _disposable;

        public ScheduledObserver(IScheduler scheduler, IObserver<T> observer)
        {
            _scheduler = scheduler;
            _observer = observer;
            _longRunning = _scheduler.AsLongRunning();

            if (_longRunning != null)
            {
                _dispatcherEvent = new SemaphoreSlim(0);
                _dispatcherEventRelease = new SemaphoreSlimRelease(_dispatcherEvent);
            }
        }

        private sealed class SemaphoreSlimRelease : IDisposable
        {
            private SemaphoreSlim _dispatcherEvent;

            public SemaphoreSlimRelease(SemaphoreSlim dispatcherEvent)
            {
                Volatile.Write(ref _dispatcherEvent, dispatcherEvent);
            }

            public void Dispose()
            {
                Interlocked.Exchange(ref _dispatcherEvent, null)?.Release();
            }
        }

        private readonly object _dispatcherInitGate = new object();
        private readonly SemaphoreSlim _dispatcherEvent;
        private readonly IDisposable _dispatcherEventRelease;
        private IDisposable _dispatcherJob;

        private void EnsureDispatcher()
        {
            if (_dispatcherJob == null)
            {
                lock (_dispatcherInitGate)
                {
                    if (_dispatcherJob == null)
                    {
                        _dispatcherJob = _longRunning.ScheduleLongRunning(Dispatch);

                        Disposable.TrySetSerial(ref _disposable, StableCompositeDisposable.Create
                        (
                            _dispatcherJob,
                            _dispatcherEventRelease
                        ));
                    }
                }
            }
        }

        private void Dispatch(ICancelable cancel)
        {
            while (true)
            {
                _dispatcherEvent.Wait();

                if (cancel.IsDisposed)
                {
                    return;
                }

                var next = default(T);
                while (_queue.TryDequeue(out next))
                {
                    try
                    {
                        _observer.OnNext(next);
                    }
                    catch
                    {
                        while (_queue.TryDequeue(out _))
                        {
                        }

                        throw;
                    }

                    _dispatcherEvent.Wait();

                    if (cancel.IsDisposed)
                    {
                        return;
                    }
                }

                if (_failed)
                {
                    _observer.OnError(_error);
                    Dispose();
                    return;
                }

                if (_completed)
                {
                    _observer.OnCompleted();
                    Dispose();
                    return;
                }
            }
        }

        public void EnsureActive() => EnsureActive(1);

        public void EnsureActive(int n)
        {
            if (_longRunning != null)
            {
                if (n > 0)
                {
                    _dispatcherEvent.Release(n);
                }

                EnsureDispatcher();
            }
            else
            {
                EnsureActiveSlow();
            }
        }

        private void EnsureActiveSlow()
        {
            var isOwner = false;

            while (true)
            {
                var old = Interlocked.CompareExchange(ref _state, Running, Stopped);
                if (old == Stopped)
                {
                    isOwner = true; // RUNNING
                    break;
                }

                if (old == Faulted)
                {
                    return;
                }

                // If we find the consumer loop running, we transition to PENDING to handle
                // the case where the queue is seen empty by the consumer, making it transition
                // to the STOPPED state, but we inserted an item into the queue.
                //
                // C: _queue.TryDequeue == false                  (RUNNING)
                // ----------------------------------------------
                // P: _queue.Enqueue(...)
                //    EnsureActive
                //      Exchange(ref _state, RUNNING) == RUNNING
                // ----------------------------------------------
                // C: transition to STOPPED                       (STOPPED)
                //
                // In this case, P would believe C is running and not invoke the scheduler
                // using the isOwner flag.
                //
                // By introducing an intermediate PENDING state and using CAS in the consumer
                // to only transition to STOPPED in case we were still RUNNING, we can force
                // the consumer to reconsider the decision to transition to STOPPED. In that
                // case, the consumer loops again and re-reads from the queue and other state
                // fields. At least one bit of state will have changed because EnsureActive
                // should only be called after invocation of IObserver<T> methods that touch
                // this state.
                //
                if (old == Pending || old == Running && Interlocked.CompareExchange(ref _state, Pending, Running) == Running)
                {
                    break;
                }
            }

            if (isOwner)
            {
                Disposable.TrySetSerial(ref _disposable, _scheduler.Schedule<object>(null, Run));
            }
        }

        private void Run(object state, Action<object> recurse)
        {
            var next = default(T);
            while (!_queue.TryDequeue(out next))
            {
                if (_failed)
                {
                    // Between transitioning to _failed and the queue check in the loop,
                    // items could have been queued, so we can't stop yet. We don't spin
                    // and immediately re-check the queue.
                    //
                    // C: _queue.TryDequeue == false
                    // ----------------------------------------------
                    // P: OnNext(...)
                    //      _queue.Enqueue(...)                        // Will get lost
                    // P: OnError(...)
                    //      _failed = true
                    // ----------------------------------------------
                    // C: if (_failed)
                    //      _observer.OnError(...)                     // Lost an OnNext
                    //
                    if (!_queue.IsEmpty)
                    {
                        continue;
                    }

                    Interlocked.Exchange(ref _state, Stopped);
                    _observer.OnError(_error);
                    Dispose();
                    return;
                }

                if (_completed)
                {
                    // Between transitioning to _completed and the queue check in the loop,
                    // items could have been queued, so we can't stop yet. We don't spin
                    // and immediately re-check the queue.
                    //
                    // C: _queue.TryDequeue == false
                    // ----------------------------------------------
                    // P: OnNext(...)
                    //      _queue.Enqueue(...)                        // Will get lost
                    // P: OnCompleted(...)
                    //      _completed = true
                    // ----------------------------------------------
                    // C: if (_completed)
                    //      _observer.OnCompleted()                    // Lost an OnNext
                    //
                    if (!_queue.IsEmpty)
                    {
                        continue;
                    }

                    Interlocked.Exchange(ref _state, Stopped);
                    _observer.OnCompleted();
                    Dispose();
                    return;
                }

                var old = Interlocked.CompareExchange(ref _state, Stopped, Running);
                if (old == Running || old == Faulted)
                {
                    return;
                }

                Debug.Assert(old == Pending);

                // The producer has put us in the PENDING state to prevent us from
                // transitioning to STOPPED, so we go RUNNING again and re-check our state.
                _state = Running;
            }

            Interlocked.Exchange(ref _state, Running);

            try
            {
                _observer.OnNext(next);
            }
            catch
            {
                Interlocked.Exchange(ref _state, Faulted);

                while (_queue.TryDequeue(out _))
                {
                }

                throw;
            }

            recurse(state);
        }

        protected override void OnNextCore(T value)
        {
            _queue.Enqueue(value);
        }

        protected override void OnErrorCore(Exception exception)
        {
            _error = exception;
            _failed = true;
        }

        protected override void OnCompletedCore()
        {
            _completed = true;
        }

        protected override void Dispose(bool disposing)
        {
            base.Dispose(disposing);

            if (disposing)
            {
                Disposable.TryDispose(ref _disposable);
            }
        }
    }

    internal sealed class ObserveOnObserver<T> : ScheduledObserver<T>
    {
        private IDisposable _run;

        public ObserveOnObserver(IScheduler scheduler, IObserver<T> observer)
            : base(scheduler, observer)
        {

        }

        public void Run(IObservable<T> source)
        {
            Disposable.SetSingle(ref _run, source.SubscribeSafe(this));
        }

        protected override void OnNextCore(T value)
        {
            base.OnNextCore(value);
            EnsureActive();
        }

        protected override void OnErrorCore(Exception exception)
        {
            base.OnErrorCore(exception);
            EnsureActive();
        }

        protected override void OnCompletedCore()
        {
            base.OnCompletedCore();
            EnsureActive();
        }

        protected override void Dispose(bool disposing)
        {
            base.Dispose(disposing);

            if (disposing)
            {
                Disposable.TryDispose(ref _run);
            }
        }
    }

    internal interface IScheduledObserver<T> : IObserver<T>, IDisposable
    {
        void EnsureActive();
        void EnsureActive(int count);
    }

    /// <summary>
    /// An ObserveOn operator implementation that uses lock-free
    /// techniques to signal events to the downstream.
    /// </summary>
    /// <typeparam name="T">The element type of the sequence.</typeparam>
    internal sealed class ObserveOnObserverNew<T> : IdentitySink<T>
    {
        private readonly IScheduler _scheduler;

        private readonly ConcurrentQueue<T> _queue;

        /// <summary>
        /// The current task representing a running drain operation.
        /// </summary>
        private IDisposable _task;

        /// <summary>
        /// Indicates the work-in-progress state of this operator,
        /// zero means no work is currently being done.
        /// </summary>
        private int _wip;

        /// <summary>
        /// If true, the upstream has issued OnCompleted.
        /// </summary>
        private bool _done;

        /// <summary>
        /// If <see cref="_done"/> is true and this is non-null, the upstream
        /// failed with an OnError.
        /// </summary>
        private Exception _error;

        /// <summary>
        /// Indicates a dispose has been requested.
        /// </summary>
        private bool _disposed;

        public ObserveOnObserverNew(IScheduler scheduler, IObserver<T> downstream) : base(downstream)
        {
            _scheduler = scheduler;
            _queue = new ConcurrentQueue<T>();
        }

        protected override void Dispose(bool disposing)
        {
            Volatile.Write(ref _disposed, true);

            base.Dispose(disposing);
            if (disposing)
            {
                Disposable.TryDispose(ref _task);
                Clear(_queue);
            }
        }

        /// <summary>
        /// Remove remaining elements from the queue upon
        /// cancellation or failure.
        /// </summary>
        /// <param name="q">The queue to use. The argument ensures that the
        /// _queue field is not re-read from memory unnecessarily
        /// due to the memory barriers inside TryDequeue mandating it
        /// despite the field is read-only.</param>
        private void Clear(ConcurrentQueue<T> q)
        {
            while (q.TryDequeue(out var _))
            {
            }
        }

        public override void OnCompleted()
        {
            Volatile.Write(ref _done, true);
            Schedule();
        }

        public override void OnError(Exception error)
        {
            _error = error;
            Volatile.Write(ref _done, true);
            Schedule();
        }

        public override void OnNext(T value)
        {
            _queue.Enqueue(value);
            Schedule();
        }

        /// <summary>
        /// Submit the drain task via the appropriate scheduler if
        /// there is no drain currently running (wip > 0).
        /// </summary>
        private void Schedule()
        {
            if (Interlocked.Increment(ref _wip) == 1)
            {
                var newTask = new SingleAssignmentDisposable();

                if (Disposable.TrySetMultiple(ref _task, newTask))
                {
                    newTask.Disposable = _scheduler.Schedule(this, DrainShortRunningFunc);
                }

                // If there was a cancellation, clear the queue
                // of items. This doesn't have to be inside the
                // wip != 0 (exclusive) mode as the queue
                // is of a multi-consumer type.
                if (Volatile.Read(ref _disposed))
                {
                    Clear(_queue);
                }
            }
        }

        /// <summary>
        /// The static action to be scheduled on a simple scheduler.
        /// Avoids creating a delegate that captures <code>this</code>
        /// whenever the signals have to be drained.
        /// </summary>
        private static readonly Func<IScheduler, ObserveOnObserverNew<T>, IDisposable> DrainShortRunningFunc =
            (scheduler, self) => self.DrainShortRunning(scheduler);

        /// <summary>
        /// Emits at most one signal per run on a scheduler that doesn't like
        /// long running tasks.
        /// </summary>
        /// <param name="recursiveScheduler">The scheduler to use for scheduling the next signal emission if necessary.</param>
        /// <returns>The IDisposable of the recursively scheduled task or an empty disposable.</returns>
        private IDisposable DrainShortRunning(IScheduler recursiveScheduler)
        {
            DrainStep(_queue);

            if (Interlocked.Decrement(ref _wip) != 0)
            {
                // Don't return the disposable of Schedule() because that may chain together
                // a long string of ScheduledItems causing StackOverflowException upon Dispose()
                var d = recursiveScheduler.Schedule(this, DrainShortRunningFunc);
                Disposable.TrySetMultiple(ref _task, d);
            }
            return Disposable.Empty;
        }

        /// <summary>
        /// Executes a drain step by checking the disposed state,
        /// checking for the terminated state and for an
        /// empty queue, issuing the appropriate signals to the
        /// given downstream.
        /// </summary>
        /// <param name="q">The queue to use. The argument ensures that the
        /// _queue field is not re-read from memory due to the memory barriers
        /// inside TryDequeue mandating it despite the field is read-only.
        /// In addition, the DrainStep is invoked from the DrainLongRunning's loop
        /// so reading _queue inside this method would still incur the same barrier
        /// overhead otherwise.</param>
        private void DrainStep(ConcurrentQueue<T> q)
        {
            // Check if the operator has been disposed
            if (Volatile.Read(ref _disposed))
            {
                // cleanup residue items in the queue
                Clear(q);
                return;
            }

            // Has the upstream call OnCompleted?
            var d = Volatile.Read(ref _done);

            if (d)
            {
                // done = true happens before setting error
                // this is safe to be a plain read
                var ex = _error;
                // if not null, there was an OnError call
                if (ex != null)
                {
                    Volatile.Write(ref _disposed, true);
                    ForwardOnError(ex);
                    return;
                }
            }

            // get the next item from the queue if any
            var empty = !q.TryDequeue(out var v);

            // the upstream called OnComplete and the queue is empty
            // that means we are done, no further signals can happen
            if (d && empty)
            {
                Volatile.Write(ref _disposed, true);
                // otherwise, complete normally
                ForwardOnCompleted();
                return;
            }
            
            // the queue is empty and the upstream hasn't completed yet
            if (empty)
            {
                return;
            }
            // emit the item
            ForwardOnNext(v);
        }
    }

    /// <summary>
    /// Signals events on a ISchedulerLongRunning by blocking the emission thread while waiting
    /// for them from the upstream.
    /// </summary>
    /// <typeparam name="TSource">The element type of the sequence.</typeparam>
    internal sealed class ObserveOnObserverLongRunning<TSource> : IdentitySink<TSource>
    {
        /// <summary>
        /// This will run a suspending drain task, hogging the backing thread
        /// until the sequence terminates or gets disposed.
        /// </summary>
        private readonly ISchedulerLongRunning _scheduler;

        /// <summary>
        /// The queue for holding the OnNext items, terminal signals have their own fields.
        /// </summary>
        private readonly ConcurrentQueue<TSource> _queue;

        /// <summary>
        /// Protects the suspension and resumption of the long running drain task.
        /// </summary>
        private readonly object _suspendGuard;

        /// <summary>
        /// The work-in-progress counter. If it jumps from 0 to 1, the drain task is resumed,
        /// if it reaches 0 again, the drain task is suspended.
        /// </summary>
        private long _wip;

        /// <summary>
        /// Set to true if the upstream terminated.
        /// </summary>
        private bool _done;

        /// <summary>
        /// Set to a non-null Exception if the upstream terminated with OnError.
        /// </summary>
        private Exception _error;

        /// <summary>
        /// Indicates the sequence has been disposed and the drain task should quit.
        /// </summary>
        private bool _disposed;

        /// <summary>
        /// Makes sure the drain task is scheduled only once, when the first signal
        /// from upstream arrives.
        /// </summary>
        private int _runDrainOnce;

        /// <summary>
        /// The disposable tracking the drain task.
        /// </summary>
        private IDisposable _drainTask;

        public ObserveOnObserverLongRunning(ISchedulerLongRunning scheduler, IObserver<TSource> observer) : base(observer)
        {
            _scheduler = scheduler;
            _queue = new ConcurrentQueue<TSource>();
            _suspendGuard = new object();
        }

        public override void OnCompleted()
        {
            Volatile.Write(ref _done, true);
            Schedule();
        }

        public override void OnError(Exception error)
        {
            _error = error;
            Volatile.Write(ref _done, true);
            Schedule();
        }

        public override void OnNext(TSource value)
        {
            _queue.Enqueue(value);
            Schedule();
        }

        private void Schedule()
        {
            // Schedule the suspending drain once
            if (Volatile.Read(ref _runDrainOnce) == 0
                && Interlocked.CompareExchange(ref _runDrainOnce, 1, 0) == 0)
            {
                Disposable.SetSingle(ref _drainTask, _scheduler.ScheduleLongRunning(this, DrainLongRunning));
            }

            // Indicate more work is to be done by the drain loop
            if (Interlocked.Increment(ref _wip) == 1L)
            {
                // resume the drain loop waiting on the guard
                lock (_suspendGuard)
                {
                    Monitor.Pulse(_suspendGuard);
                }
            }
        }

        /// <summary>
        /// Static reference to the Drain method, saves allocation.
        /// </summary>
        private static readonly Action<ObserveOnObserverLongRunning<TSource>, ICancelable> DrainLongRunning = (self, cancelable) => self.Drain();

        protected override void Dispose(bool disposing)
        {
            // Indicate the drain task should quit
            Volatile.Write(ref _disposed, true);
            // Resume the drain task in case it sleeps
            lock (_suspendGuard)
            {
                Monitor.Pulse(_suspendGuard);
            }
            // Cancel the drain task handle.
            Disposable.TryDispose(ref _drainTask);
            base.Dispose(disposing);
        }

        private void Drain()
        {
            var q = _queue;
            for (; ; )
            {
                // If the sequence was disposed, clear the queue and quit
                if (Volatile.Read(ref _disposed))
                {
                    while (q.TryDequeue(out var _)) ;
                    break;
                }

                // Has the upstream terminated?
                var isDone = Volatile.Read(ref _done);
                // Do we have an item in the queue
                var hasValue = q.TryDequeue(out var item);

                // If the upstream has terminated and no further items are in the queue
                if (isDone && !hasValue)
                {
                    // Find out if the upstream terminated with an error and signal accordingly.
                    var e = _error;
                    if (e != null)
                    {
                        ForwardOnError(e);
                    }
                    else
                    {
                        ForwardOnCompleted();
                    }
                    break;
                }

                // There was an item, signal it.
                if (hasValue)
                {
                    ForwardOnNext(item);
                    // Consume the item and try the next item if the work-in-progress
                    // indicator is still not zero
                    if (Interlocked.Decrement(ref _wip) != 0L)
                    {
                        continue;
                    }
                }

                // If we run out of work and the sequence is not disposed
                if (Volatile.Read(ref _wip) == 0L && !Volatile.Read(ref _disposed))
                {
                    var g = _suspendGuard;
                    // try sleeping, if we can't even enter the lock, the producer
                    // side is currently trying to resume us
                    if (Monitor.TryEnter(g))
                    {
                        // Make sure again there is still no work and the sequence is not disposed
                        if (Volatile.Read(ref _wip) == 0L && !Volatile.Read(ref _disposed))
                        { 
                            // wait for a Pulse(g)
                            Monitor.Wait(g);
                        }
                        // Unlock
                        Monitor.Exit(g);
                    }
                }
            }
        }
    }
}

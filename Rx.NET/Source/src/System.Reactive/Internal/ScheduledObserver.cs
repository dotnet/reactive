// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the Apache 2.0 License.
// See the LICENSE file in the project root for more information. 

using System.Collections.Generic;
using System.Reactive.Concurrency;
using System.Reactive.Disposables;
using System.Threading;

namespace System.Reactive
{
    using System.Collections.Concurrent;
    using System.Diagnostics;

    internal class ScheduledObserver<T> : ObserverBase<T>, IScheduledObserver<T>
    {
        private volatile int _state = 0;
        private const int STOPPED = 0;
        private const int RUNNING = 1;
        private const int PENDING = 2;
        private const int FAULTED = 9;

        private readonly ConcurrentQueue<T> _queue = new ConcurrentQueue<T>();
        private volatile bool _failed;
        private volatile Exception _error;
        private volatile bool _completed;

        private readonly IObserver<T> _observer;
        private readonly IScheduler _scheduler;
        private readonly ISchedulerLongRunning _longRunning;
        private readonly SerialDisposable _disposable = new SerialDisposable();

        public ScheduledObserver(IScheduler scheduler, IObserver<T> observer)
        {
            _scheduler = scheduler;
            _observer = observer;
            _longRunning = _scheduler.AsLongRunning();

            if (_longRunning != null)
            {
                _dispatcherEvent = new SemaphoreSlim(0);
                _dispatcherEventRelease = Disposable.Create(() => _dispatcherEvent.Release());
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

                        _disposable.Disposable = StableCompositeDisposable.Create
                        (
                            _dispatcherJob,
                            _dispatcherEventRelease
                        );
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
                    return;

                var next = default(T);
                while (_queue.TryDequeue(out next))
                {
                    try
                    {
                        _observer.OnNext(next);
                    }
                    catch
                    {
                        var nop = default(T);
                        while (_queue.TryDequeue(out nop))
                            ;

                        throw;
                    }

                    _dispatcherEvent.Wait();

                    if (cancel.IsDisposed)
                        return;
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
                EnsureActiveSlow();
        }

        private void EnsureActiveSlow()
        {
            var isOwner = false;

            while (true)
            {
                var old = Interlocked.CompareExchange(ref _state, RUNNING, STOPPED);
                if (old == STOPPED)
                {
                    isOwner = true; // RUNNING
                    break;
                }

                if (old == FAULTED)
                    return;

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
                if (old == PENDING || old == RUNNING && Interlocked.CompareExchange(ref _state, PENDING, RUNNING) == RUNNING)
                    break;
            }

            if (isOwner)
            {
                _disposable.Disposable = _scheduler.Schedule<object>(null, Run);
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
                        continue;

                    Interlocked.Exchange(ref _state, STOPPED);
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
                        continue;

                    Interlocked.Exchange(ref _state, STOPPED);
                    _observer.OnCompleted();
                    Dispose();
                    return;
                }

                var old = Interlocked.CompareExchange(ref _state, STOPPED, RUNNING);
                if (old == RUNNING || old == FAULTED)
                    return;

                Debug.Assert(old == PENDING);

                // The producer has put us in the PENDING state to prevent us from
                // transitioning to STOPPED, so we go RUNNING again and re-check our state.
                _state = RUNNING;
            }

            Interlocked.Exchange(ref _state, RUNNING);

            try
            {
                _observer.OnNext(next);
            }
            catch
            {
                Interlocked.Exchange(ref _state, FAULTED);

                var nop = default(T);
                while (_queue.TryDequeue(out nop))
                    ;

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
                _disposable.Dispose();
            }
        }
    }

    internal sealed class ObserveOnObserver<T> : ScheduledObserver<T>
    {
        private IDisposable _cancel;

        public ObserveOnObserver(IScheduler scheduler, IObserver<T> observer, IDisposable cancel)
            : base(scheduler, observer)
        {
            _cancel = cancel;
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
                Interlocked.Exchange(ref _cancel, null)?.Dispose();
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
    internal sealed class ObserveOnObserverNew<T> : IObserver<T>, IDisposable
    {
        readonly IObserver<T> downstream;

        readonly IScheduler scheduler;

        /// <summary>
        /// If not null, the <see cref="scheduler"/> supports
        /// long running tasks.
        /// </summary>
        readonly ISchedulerLongRunning longRunning;

        readonly ConcurrentQueue<T> queue;

        /// <summary>
        /// The disposable of the upstream source.
        /// </summary>
        IDisposable upstream;

        /// <summary>
        /// The current task representing a running drain operation.
        /// </summary>
        IDisposable task;

        /// <summary>
        /// Indicates the work-in-progress state of this operator,
        /// zero means no work is currently being done.
        /// </summary>
        int wip;

        /// <summary>
        /// If true, the upstream has issued OnCompleted.
        /// </summary>
        bool done;
        /// <summary>
        /// If <see cref="done"/> is true and this is non-null, the upstream
        /// failed with an OnError.
        /// </summary>
        Exception error;

        /// <summary>
        /// Indicates a dispose has been requested.
        /// </summary>
        bool disposed;

        public ObserveOnObserverNew(IScheduler scheduler, IObserver<T> downstream, IDisposable upstream)
        {
            this.downstream = downstream;
            this.scheduler = scheduler;
            this.longRunning = scheduler.AsLongRunning();
            this.queue = new ConcurrentQueue<T>();
            Volatile.Write(ref this.upstream, upstream);
        }

        public void Dispose()
        {
            Volatile.Write(ref disposed, true);
            Interlocked.Exchange(ref upstream, BooleanDisposable.True)?.Dispose();
            Interlocked.Exchange(ref task, BooleanDisposable.True)?.Dispose();
            Clear();
        }

        /// <summary>
        /// Remove remaining elements from the queue upon
        /// cancellation or failure.
        /// </summary>
        void Clear()
        {
            var q = queue;
            while (q.TryDequeue(out var _)) ;
        }

        public void OnCompleted()
        {
            Volatile.Write(ref done, true);
            Schedule();
        }

        public void OnError(Exception error)
        {
            this.error = error;
            Volatile.Write(ref done, true);
            Schedule();
        }

        public void OnNext(T value)
        {
            queue.Enqueue(value);
            Schedule();
        }

        /// <summary>
        /// Submit the drain task via the appropriate scheduler if
        /// there is no drain currently running (wip > 0).
        /// </summary>
        void Schedule()
        {
            if (Interlocked.Increment(ref wip) == 1)
            {
                var oldTask = Volatile.Read(ref task);

                var newTask = new SingleAssignmentDisposable();

                if (oldTask != BooleanDisposable.True
                    && Interlocked.CompareExchange(ref task, newTask, oldTask) == oldTask)
                {

                    var longRunning = this.longRunning;
                    if (longRunning != null)
                    {
                        newTask.Disposable = longRunning.ScheduleLongRunning(this, DRAIN_LONG_RUNNING);
                    }
                    else
                    {
                        newTask.Disposable = scheduler.Schedule(this, DRAIN_SHORT_RUNNING);
                    }
                }

                // If there was a cancellation, clear the queue
                // of items. This doesn't have to be inside the
                // wip != 0 (exclusive) mode as the queue
                // is of a multi-consumer type.
                if (Volatile.Read(ref disposed))
                {
                    Clear();
                }
            }
        }

        /// <summary>
        /// The static action to be scheduled on a long running scheduler.
        /// Avoids creating a delegate that captures <code>this</code>
        /// whenever the signals have to be drained.
        /// </summary>
        static readonly Action<ObserveOnObserverNew<T>, ICancelable> DRAIN_LONG_RUNNING =
            (self, cancel) => self.DrainLongRunning();

        /// <summary>
        /// The static action to be scheduled on a simple scheduler.
        /// Avoids creating a delegate that captures <code>this</code>
        /// whenever the signals have to be drained.
        /// </summary>
        static readonly Func<IScheduler, ObserveOnObserverNew<T>, IDisposable> DRAIN_SHORT_RUNNING =
            (scheduler, self) => self.DrainShortRunning(scheduler);

        /// <summary>
        /// Emits at most one signal per run on a scheduler that doesn't like
        /// long running tasks.
        /// </summary>
        /// <param name="recursiveScheduler">The scheduler to use for scheduling the next signal emission if necessary.</param>
        /// <returns>The IDisposable of the recursively scheduled task or an empty disposable.</returns>
        IDisposable DrainShortRunning(IScheduler recursiveScheduler)
        {
            DrainStep(queue, downstream, false);

            if (Interlocked.Decrement(ref wip) != 0)
            {
                return recursiveScheduler.Schedule(this, DRAIN_SHORT_RUNNING);
            }
            return Disposable.Empty;
        }

        /// <summary>
        /// Executes a drain step by checking the disposed state,
        /// checking for the terminated state and for an
        /// empty queue, issuing the approrpiate signals to the
        /// given downstream.
        /// </summary>
        /// <param name="q">The queue to use.</param>
        /// <param name="downstream">The intended consumer of the events.</param>
        /// <param name="delayError">Should the errors be delayed until all
        /// queued items have been emitted to the downstream?</param>
        /// <returns>True if the drain loop should stop.</returns>
        bool DrainStep(ConcurrentQueue<T> q, IObserver<T> downstream, bool delayError)
        {
            // Check if the operator has been disposed
            if (Volatile.Read(ref disposed))
            {
                // cleanup residue items in the queue
                Clear();
                return true;
            }

            // Has the upstream call OnCompleted?
            var d = Volatile.Read(ref done);

            if (d && !delayError)
            {
                // done = true happens before setting error
                // this is safe to be a plain read
                var ex = error;
                // if not null, there was an OnError call
                if (ex != null)
                {
                    Volatile.Write(ref disposed, true);
                    downstream.OnError(ex);
                    return true;
                }
            }

            // get the next item from the queue if any
            var empty = !queue.TryDequeue(out var v);

            // the upstream called OnComplete and the queue is empty
            // that means we are done, no further signals can happen
            if (d && empty)
            {
                Volatile.Write(ref disposed, true);
                // done = true happens before setting error
                // this is safe to be a plain read
                var ex = error;
                // if not null, there was an OnError call
                if (ex != null)
                {
                    downstream.OnError(ex);
                }
                else
                {
                    // otherwise, complete normally
                    downstream.OnCompleted();
                }
                return true;
            }
            else
            // the queue is empty and the upstream hasn't completed yet
            if (empty)
            {
                return true;
            }
            // emit the item
            downstream.OnNext(v);

            // keep looping
            return false;
        }

        /// <summary>
        /// Emits as many signals as possible to the downstream observer
        /// as this is executing a long-running scheduler so
        /// it can occupy that thread as long as it needs to.
        /// </summary>
        void DrainLongRunning()
        {
            var missed = 1;

            // read out fields upfront as the DrainStep uses atomics
            // that would force the re-read of these constant values
            // from memory, regardless of readonly, afaik
            var q = queue;
            var downstream = this.downstream;

            for (; ; )
            {
                for (; ; )
                {
                    // delayError: true - because of 
                    //      ObserveOn_LongRunning_HoldUpDuringDispatchAndFail
                    // expects something that almost looks like full delayError
                    if (DrainStep(q, downstream, true))
                    {
                        break;
                    }
                }

                missed = Interlocked.Add(ref wip, -missed);
                if (missed == 0)
                {
                    break;
                }
            }
        }
    }
}

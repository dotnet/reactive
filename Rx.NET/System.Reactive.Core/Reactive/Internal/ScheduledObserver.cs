// Copyright (c) Microsoft Open Technologies, Inc. All rights reserved. See License.txt in the project root for license information.

using System.Collections.Generic;
using System.Reactive.Concurrency;
using System.Reactive.Disposables;
using System.Threading;

namespace System.Reactive
{
#if !NO_PERF && !NO_CDS
    using System.Collections.Concurrent;
    using System.Diagnostics;

    internal class ScheduledObserver<T> : ObserverBase<T>, IDisposable
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
                _dispatcherEvent = new SemaphoreSlim(0);
        }

        private readonly object _dispatcherInitGate = new object();
        private SemaphoreSlim _dispatcherEvent;
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

                        _disposable.Disposable = new CompositeDisposable(2)
                        {
                            _dispatcherJob,
                            Disposable.Create(() => _dispatcherEvent.Release())
                        };
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

        public void EnsureActive()
        {
            EnsureActive(1);
        }

        public void EnsureActive(int n)
        {
            if (_longRunning != null)
            {
                if (n > 0)
                    _dispatcherEvent.Release(n);

                EnsureDispatcher();
            }
            else
                EnsureActiveSlow();
        }

        private void EnsureActiveSlow()
        {
            var isOwner = false;

#pragma warning disable 0420
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
#pragma warning restore 0420

            if (isOwner)
            {
                _disposable.Disposable = _scheduler.Schedule<object>(null, Run);
            }
        }

        private void Run(object state, Action<object> recurse)
        {
#pragma warning disable 0420
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

#pragma warning restore 0420

            try
            {
                _observer.OnNext(next);
            }
            catch
            {
#pragma warning disable 0420
                Interlocked.Exchange(ref _state, FAULTED);
#pragma warning restore 0420

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
#else
    class ScheduledObserver<T> : ObserverBase<T>, IDisposable
    {
        private bool _isAcquired = false;
        private bool _hasFaulted = false;
        private readonly Queue<Action> _queue = new Queue<Action>();
        private readonly IObserver<T> _observer;
        private readonly IScheduler _scheduler;
        private readonly SerialDisposable _disposable = new SerialDisposable();

        public ScheduledObserver(IScheduler scheduler, IObserver<T> observer)
        {
            _scheduler = scheduler;
            _observer = observer;
        }

        public void EnsureActive(int n)
        {
            EnsureActive();
        }

        public void EnsureActive()
        {
            var isOwner = false;

            lock (_queue)
            {
                if (!_hasFaulted && _queue.Count > 0)
                {
                    isOwner = !_isAcquired;
                    _isAcquired = true;
                }
            }

            if (isOwner)
            {
                _disposable.Disposable = _scheduler.Schedule<object>(null, Run);
            }
        }

        private void Run(object state, Action<object> recurse)
        {
            var work = default(Action);
            lock (_queue)
            {
                if (_queue.Count > 0)
                    work = _queue.Dequeue();
                else
                {
                    _isAcquired = false;
                    return;
                }
            }

            try
            {
                work();
            }
            catch
            {
                lock (_queue)
                {
                    _queue.Clear();
                    _hasFaulted = true;
                }
                throw;
            }

            recurse(state);
        }

        protected override void OnNextCore(T value)
        {
            lock (_queue)
                _queue.Enqueue(() => _observer.OnNext(value));
        }

        protected override void OnErrorCore(Exception exception)
        {
            lock (_queue)
                _queue.Enqueue(() => _observer.OnError(exception));
        }

        protected override void OnCompletedCore()
        {
            lock (_queue)
                _queue.Enqueue(() => _observer.OnCompleted());
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
#endif

    class ObserveOnObserver<T> : ScheduledObserver<T>
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
                var cancel = Interlocked.Exchange(ref _cancel, null);
                if (cancel != null)
                {
                    cancel.Dispose();
                }
            }
        }
    }
}

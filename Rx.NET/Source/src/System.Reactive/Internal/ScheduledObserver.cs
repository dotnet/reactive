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
}

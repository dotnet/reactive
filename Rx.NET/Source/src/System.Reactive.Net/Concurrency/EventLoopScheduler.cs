﻿// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information. 

using System.Collections.Generic;
using System.Reactive.Disposables;
using System.Threading;

namespace System.Reactive.Concurrency
{
    /// <summary>
    /// Represents an object that schedules units of work on a designated thread.
    /// </summary>
    public sealed class EventLoopScheduler : LocalScheduler, ISchedulerPeriodic, IDisposable
    {
        #region Fields

        /// <summary>
        /// Counter for diagnostic purposes, to name the threads.
        /// </summary>
        private static int _counter;

        /// <summary>
        /// Thread factory function.
        /// </summary>
        private readonly Func<ThreadStart, Thread> _threadFactory;

        /// <summary>
        /// Stopwatch for timing free of absolute time dependencies.
        /// </summary>
        private readonly IStopwatch _stopwatch;

        /// <summary>
        /// Thread used by the event loop to run work items on. No work should be run on any other thread.
        /// If ExitIfEmpty is set, the thread can quit and a new thread will be created when new work is scheduled.
        /// </summary>
        private Thread? _thread;

        /// <summary>
        /// Gate to protect data structures, including the work queue and the ready list.
        /// </summary>
        private readonly object _gate;

        /// <summary>
        /// Semaphore to count requests to re-evaluate the queue, from either Schedule requests or when a timer
        /// expires and moves on to the next item in the queue.
        /// </summary>
        private readonly SemaphoreSlim _evt;

        /// <summary>
        /// Queue holding work items. Protected by the gate.
        /// </summary>
        private readonly SchedulerQueue<TimeSpan> _queue;

        /// <summary>
        /// Queue holding items that are ready to be run as soon as possible. Protected by the gate.
        /// </summary>
        private readonly Queue<ScheduledItem<TimeSpan>> _readyList;

        /// <summary>
        /// Work item that will be scheduled next. Used upon reevaluation of the queue to check whether the next
        /// item is still the same. If not, a new timer needs to be started (see below).
        /// </summary>
        private ScheduledItem<TimeSpan>? _nextItem;

        /// <summary>
        /// Disposable that always holds the timer to dispatch the first element in the queue.
        /// </summary>
        private SerialDisposableValue _nextTimer;

        /// <summary>
        /// Flag indicating whether the event loop should quit. When set, the event should be signaled as well to
        /// wake up the event loop thread, which will subsequently abandon all work.
        /// </summary>
        private bool _disposed;

        #endregion

        #region Constructors

        /// <summary>
        /// Creates an object that schedules units of work on a designated thread.
        /// </summary>
        public EventLoopScheduler()
            : this(static a => new Thread(a) { Name = "Event Loop " + Interlocked.Increment(ref _counter), IsBackground = true })
        {
        }

        /// <summary>
        /// Creates an object that schedules units of work on a designated thread, using the specified factory to control thread creation options.
        /// </summary>
        /// <param name="threadFactory">Factory function for thread creation.</param>
        /// <exception cref="ArgumentNullException"><paramref name="threadFactory"/> is <c>null</c>.</exception>
        public EventLoopScheduler(Func<ThreadStart, Thread> threadFactory)
        {
            _threadFactory = threadFactory ?? throw new ArgumentNullException(nameof(threadFactory));
            _stopwatch = ConcurrencyAbstractionLayer.Current.StartStopwatch();

            _gate = new object();

            _evt = new SemaphoreSlim(0);
            _queue = new SchedulerQueue<TimeSpan>();
            _readyList = new Queue<ScheduledItem<TimeSpan>>();

            ExitIfEmpty = false;
        }

        #endregion

        #region Properties

        /// <summary>
        /// Indicates whether the event loop thread is allowed to quit when no work is left. If new work
        /// is scheduled afterwards, a new event loop thread is created. This property is used by the
        /// NewThreadScheduler which uses an event loop for its recursive invocations.
        /// </summary>
        internal bool ExitIfEmpty
        {
            get;
            set;
        }

        #endregion

        #region Public methods

        /// <summary>
        /// Schedules an action to be executed after dueTime.
        /// </summary>
        /// <typeparam name="TState">The type of the state passed to the scheduled action.</typeparam>
        /// <param name="state">State passed to the action to be executed.</param>
        /// <param name="action">Action to be executed.</param>
        /// <param name="dueTime">Relative time after which to execute the action.</param>
        /// <returns>The disposable object used to cancel the scheduled action (best effort).</returns>
        /// <exception cref="ArgumentNullException"><paramref name="action"/> is <c>null</c>.</exception>
        /// <exception cref="ObjectDisposedException">The scheduler has been disposed and doesn't accept new work.</exception>
        public override IDisposable Schedule<TState>(TState state, TimeSpan dueTime, Func<IScheduler, TState, IDisposable> action)
        {
            if (action == null)
            {
                throw new ArgumentNullException(nameof(action));
            }

            var due = _stopwatch.Elapsed + dueTime;
            var si = new ScheduledItem<TimeSpan, TState>(this, state, action, due);

            lock (_gate)
            {
                if (_disposed)
                {
                    throw new ObjectDisposedException(nameof(EventLoopScheduler));
                }

                if (dueTime <= TimeSpan.Zero)
                {
                    _readyList.Enqueue(si);
                    _evt.Release();
                }
                else
                {
                    _queue.Enqueue(si);
                    _evt.Release();
                }

                EnsureThread();
            }

            return si;
        }

        /// <summary>
        /// Schedules a periodic piece of work on the designated thread.
        /// </summary>
        /// <typeparam name="TState">The type of the state passed to the scheduled action.</typeparam>
        /// <param name="state">Initial state passed to the action upon the first iteration.</param>
        /// <param name="period">Period for running the work periodically.</param>
        /// <param name="action">Action to be executed, potentially updating the state.</param>
        /// <returns>The disposable object used to cancel the scheduled recurring action (best effort).</returns>
        /// <exception cref="ArgumentNullException"><paramref name="action"/> is <c>null</c>.</exception>
        /// <exception cref="ArgumentOutOfRangeException"><paramref name="period"/> is less than <see cref="TimeSpan.Zero"/>.</exception>
        /// <exception cref="ObjectDisposedException">The scheduler has been disposed and doesn't accept new work.</exception>
        public IDisposable SchedulePeriodic<TState>(TState state, TimeSpan period, Func<TState, TState> action)
        {
            if (period < TimeSpan.Zero)
            {
                throw new ArgumentOutOfRangeException(nameof(period));
            }

            if (action == null)
            {
                throw new ArgumentNullException(nameof(action));
            }

            return new PeriodicallyScheduledWorkItem<TState>(this, state, period, action);
        }

        private sealed class PeriodicallyScheduledWorkItem<TState> : IDisposable
        {
            private readonly TimeSpan _period;
            private readonly Func<TState, TState> _action;
            private readonly EventLoopScheduler _scheduler;
            private readonly AsyncLock _gate = new();

            private TState _state;
            private TimeSpan _next;
            private MultipleAssignmentDisposableValue _task;

            public PeriodicallyScheduledWorkItem(EventLoopScheduler scheduler, TState state, TimeSpan period, Func<TState, TState> action)
            {
                _state = state;
                _period = period;
                _action = action;
                _scheduler = scheduler;
                _next = scheduler._stopwatch.Elapsed + period;

                _task.TrySetFirst(scheduler.Schedule(this, _next - scheduler._stopwatch.Elapsed, static (_, s) => s.Tick(_)));
            }

            private IDisposable Tick(IScheduler self)
            {
                _next += _period;

                _task.Disposable = self.Schedule(this, _next - _scheduler._stopwatch.Elapsed, static (_, s) => s.Tick(_));

                _gate.Wait(
                    this,
                    static closureWorkItem => closureWorkItem._state = closureWorkItem._action(closureWorkItem._state));

                return Disposable.Empty;
            }

            public void Dispose()
            {
                _task.Dispose();
                _gate.Dispose();
            }
        }

        /// <summary>
        /// Starts a new stopwatch object.
        /// </summary>
        /// <returns>New stopwatch object; started at the time of the request.</returns>
        public override IStopwatch StartStopwatch()
        {
            //
            // Strictly speaking, this explicit override is not necessary because the base implementation calls into
            // the enlightenment module to obtain the CAL, which would circle back to System.Reactive.PlatformServices
            // where we're currently running. This is merely a short-circuit to avoid the additional roundtrip.
            //
            return new StopwatchImpl();
        }

        /// <summary>
        /// Ends the thread associated with this scheduler. All remaining work in the scheduler queue is abandoned.
        /// </summary>
        public void Dispose()
        {
            lock (_gate)
            {
                if (!_disposed)
                {
                    _disposed = true;
                    _nextTimer.Dispose();
                    _evt.Release();
                }
            }
        }

        #endregion

        #region Private implementation

        /// <summary>
        /// Ensures there is an event loop thread running. Should be called under the gate.
        /// </summary>
        private void EnsureThread()
        {
            if (_thread == null)
            {
                _thread = _threadFactory(Run);
                _thread.Start();
            }
        }

        /// <summary>
        /// Event loop scheduled on the designated event loop thread. The loop is suspended/resumed using the event
        /// which gets set by calls to Schedule, the next item timer, or calls to Dispose.
        /// </summary>
        private void Run()
        {
            while (true)
            {
                _evt.Wait();

                ScheduledItem<TimeSpan>[]? ready = null;

                lock (_gate)
                {
                    //
                    // Bug fix that ensures the number of calls to Release never greatly exceeds the number of calls to Wait.
                    // See work item #37: https://rx.codeplex.com/workitem/37
                    //
                    while (_evt.CurrentCount > 0)
                    {
                        _evt.Wait();
                    }

                    //
                    // The event could have been set by a call to Dispose. This takes priority over anything else. We quit the
                    // loop immediately. Subsequent calls to Schedule won't ever create a new thread.
                    //
                    if (_disposed)
                    {
                        _evt.Dispose();
                        return;
                    }

                    while (_queue.Count > 0 && _queue.Peek().DueTime <= _stopwatch.Elapsed)
                    {
                        var item = _queue.Dequeue();
                        _readyList.Enqueue(item);
                    }

                    if (_queue.Count > 0)
                    {
                        var next = _queue.Peek();
                        if (next != _nextItem)
                        {
                            _nextItem = next;

                            var due = next.DueTime - _stopwatch.Elapsed;
                            _nextTimer.Disposable = ConcurrencyAbstractionLayer.Current.StartTimer(Tick, next, due);
                        }
                    }

                    if (_readyList.Count > 0)
                    {
                        ready = _readyList.ToArray();
                        _readyList.Clear();
                    }
                }

                if (ready != null)
                {
                    foreach (var item in ready)
                    {
                        if (!item.IsCanceled)
                        {
                            try
                            {
                                item.Invoke();
                            }
                            catch (ObjectDisposedException ex) when (ex.ObjectName == nameof(EventLoopScheduler))
                            {
                                // Since we are not inside the lock at this point
                                // the scheduler can be disposed before the item had a chance to run
                            }
                        }
                    }
                }

                if (ExitIfEmpty)
                {
                    lock (_gate)
                    {
                        if (_readyList.Count == 0 && _queue.Count == 0)
                        {
                            _thread = null;
                            return;
                        }
                    }
                }
            }
        }

        private void Tick(object? state)
        {
            lock (_gate)
            {
                if (!_disposed)
                {
                    var item = (ScheduledItem<TimeSpan>)state!;
                    if (item == _nextItem)
                    {
                        _nextItem = null;
                    }
                    if (_queue.Remove(item))
                    {
                        _readyList.Enqueue(item);
                    }

                    _evt.Release();
                }
            }
        }

        #endregion
    }
}

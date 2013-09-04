// Copyright (c) Microsoft Open Technologies, Inc. All rights reserved. See License.txt in the project root for license information.

#if !WINDOWS && !NO_THREAD
using System.Collections.Generic;
using System.Reactive.Disposables;
using System.Threading;

namespace System.Reactive.Concurrency
{
    /// <summary>
    /// Represents an object that schedules units of work on the CLR thread pool.
    /// </summary>
    /// <seealso cref="ThreadPoolScheduler.Instance">Singleton instance of this type exposed through this static property.</seealso>
    public sealed class ThreadPoolScheduler : LocalScheduler, ISchedulerLongRunning, ISchedulerPeriodic
    {
        private static readonly ThreadPoolScheduler s_instance = new ThreadPoolScheduler();

        /// <summary>
        /// Gets the singleton instance of the CLR thread pool scheduler.
        /// </summary>
        public static ThreadPoolScheduler Instance
        {
            get
            {
                return s_instance;
            }
        }

        ThreadPoolScheduler()
        {
        }

        /// <summary>
        /// Schedules an action to be executed.
        /// </summary>
        /// <typeparam name="TState">The type of the state passed to the scheduled action.</typeparam>
        /// <param name="state">State passed to the action to be executed.</param>
        /// <param name="action">Action to be executed.</param>
        /// <returns>The disposable object used to cancel the scheduled action (best effort).</returns>
        /// <exception cref="ArgumentNullException"><paramref name="action"/> is null.</exception>
        public override IDisposable Schedule<TState>(TState state, Func<IScheduler, TState, IDisposable> action)
        {
            if (action == null)
                throw new ArgumentNullException("action");

            var d = new SingleAssignmentDisposable();

            ThreadPool.QueueUserWorkItem(_ =>
            {
                if (!d.IsDisposed)
                    d.Disposable = action(this, state);
            }, null);

            return d;
        }

        /// <summary>
        /// Schedules an action to be executed after dueTime, using a System.Threading.Timer object.
        /// </summary>
        /// <typeparam name="TState">The type of the state passed to the scheduled action.</typeparam>
        /// <param name="state">State passed to the action to be executed.</param>
        /// <param name="action">Action to be executed.</param>
        /// <param name="dueTime">Relative time after which to execute the action.</param>
        /// <returns>The disposable object used to cancel the scheduled action (best effort).</returns>
        /// <exception cref="ArgumentNullException"><paramref name="action"/> is null.</exception>
        public override IDisposable Schedule<TState>(TState state, TimeSpan dueTime, Func<IScheduler, TState, IDisposable> action)
        {
            if (action == null)
                throw new ArgumentNullException("action");

            var dt = Scheduler.Normalize(dueTime);
            if (dt.Ticks == 0)
                return Schedule(state, action);

            return new Timer<TState>(this, state, dt, action);
        }

        /// <summary>
        /// Schedules a long-running task by creating a new thread. Cancellation happens through polling.
        /// </summary>
        /// <typeparam name="TState">The type of the state passed to the scheduled action.</typeparam>
        /// <param name="state">State passed to the action to be executed.</param>
        /// <param name="action">Action to be executed.</param>
        /// <returns>The disposable object used to cancel the scheduled action (best effort).</returns>
        /// <exception cref="ArgumentNullException"><paramref name="action"/> is null.</exception>
        public IDisposable ScheduleLongRunning<TState>(TState state, Action<TState, ICancelable> action)
        {
            if (action == null)
                throw new ArgumentNullException("action");

            return NewThreadScheduler.Default.ScheduleLongRunning(state, action);
        }

#if !NO_STOPWATCH
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
#endif

        /// <summary>
        /// Schedules a periodic piece of work, using a System.Threading.Timer object.
        /// </summary>
        /// <typeparam name="TState">The type of the state passed to the scheduled action.</typeparam>
        /// <param name="state">Initial state passed to the action upon the first iteration.</param>
        /// <param name="period">Period for running the work periodically.</param>
        /// <param name="action">Action to be executed, potentially updating the state.</param>
        /// <returns>The disposable object used to cancel the scheduled recurring action (best effort).</returns>
        /// <exception cref="ArgumentNullException"><paramref name="action"/> is null.</exception>
        /// <exception cref="ArgumentOutOfRangeException"><paramref name="period"/> is less than or equal to zero.</exception>
        public IDisposable SchedulePeriodic<TState>(TState state, TimeSpan period, Func<TState, TState> action)
        {
            //
            // MSDN documentation states the following:
            //
            //    "If period is zero (0) or negative one (-1) milliseconds and dueTime is positive, callback is invoked once;
            //     the periodic behavior of the timer is disabled, but can be re-enabled using the Change method."
            //
            if (period <= TimeSpan.Zero)
                throw new ArgumentOutOfRangeException("period");
            if (action == null)
                throw new ArgumentNullException("action");

            return new PeriodicTimer<TState>(state, period, action);
        }

#if USE_TIMER_SELF_ROOT

        //
        // See ConcurrencyAbstractionLayerImpl.cs for more information about the code
        // below and its timer rooting behavior.
        //

        sealed class Timer<TState> : IDisposable
        {
            private readonly MultipleAssignmentDisposable _disposable;

            private readonly IScheduler _parent;
            private readonly TState _state;
            private Func<IScheduler, TState, IDisposable> _action;

            private volatile System.Threading.Timer _timer;

            public Timer(IScheduler parent, TState state, TimeSpan dueTime, Func<IScheduler, TState, IDisposable> action)
            {
                _parent = parent;
                _state = state;
                _action = action;

                _disposable = new MultipleAssignmentDisposable();
                _disposable.Disposable = Disposable.Create(Stop);

                // Don't want the spin wait in Tick to get stuck if this thread gets aborted.
                try { }
                finally
                {
                    //
                    // Rooting of the timer happens through the this.Tick delegate's target object,
                    // which is the current instance and has a field to store the Timer instance.
                    //
                    _timer = new System.Threading.Timer(this.Tick, null, dueTime, TimeSpan.FromMilliseconds(System.Threading.Timeout.Infinite));
                }
            }

            private void Tick(object state)
            {
                try
                {
                    _disposable.Disposable = _action(_parent, _state);
                }
                finally
                {
                    SpinWait.SpinUntil(IsTimerAssigned);
                    Stop();
                }
            }

            private bool IsTimerAssigned()
            {
                return _timer != null;
            }

            public void Dispose()
            {
                _disposable.Dispose();
            }

            private void Stop()
            {
                var timer = _timer;
                if (timer != TimerStubs.Never)
                {
                    _action = Nop;
                    _timer = TimerStubs.Never;

                    timer.Dispose();
                }
            }

            private IDisposable Nop(IScheduler scheduler, TState state)
            {
                return Disposable.Empty;
            }
        }

        sealed class PeriodicTimer<TState> : IDisposable
        {
            private TState _state;
            private Func<TState, TState> _action;

            private readonly AsyncLock _gate;
            private volatile System.Threading.Timer _timer;

            public PeriodicTimer(TState state, TimeSpan period, Func<TState, TState> action)
            {
                _state = state;
                _action = action;

                _gate = new AsyncLock();

                //
                // Rooting of the timer happens through the this.Tick delegate's target object,
                // which is the current instance and has a field to store the Timer instance.
                //
                _timer = new System.Threading.Timer(this.Tick, null, period, period);
            }

            private void Tick(object state)
            {
                _gate.Wait(() =>
                {
                    _state = _action(_state);
                });
            }

            public void Dispose()
            {
                var timer = _timer;
                if (timer != null)
                {
                    _action = Stubs<TState>.I;
                    _timer = null;

                    timer.Dispose();
                    _gate.Dispose();
                }
            }
        }
#else
        abstract class Timer
        {
            //
            // Note: the dictionary exists to "root" the timers so that they are not garbage collected and finalized while they are running.
            //
#if !NO_HASHSET
            protected static readonly HashSet<System.Threading.Timer> s_timers = new HashSet<System.Threading.Timer>();
#else
            protected static readonly Dictionary<System.Threading.Timer, object> s_timers = new Dictionary<System.Threading.Timer, object>();
#endif
        }

        sealed class Timer<TState> : Timer, IDisposable
        {
            private readonly MultipleAssignmentDisposable _disposable;

            private readonly IScheduler _parent;
            private readonly TState _state;

            private Func<IScheduler, TState, IDisposable> _action;
            private System.Threading.Timer _timer;

            private bool _hasAdded;
            private bool _hasRemoved;

            public Timer(IScheduler parent, TState state, TimeSpan dueTime, Func<IScheduler, TState, IDisposable> action)
            {
                _disposable = new MultipleAssignmentDisposable();
                _disposable.Disposable = Disposable.Create(Unroot);

                _parent = parent;
                _state = state;

                _action = action;
                _timer = new System.Threading.Timer(Tick, null, dueTime, TimeSpan.FromMilliseconds(System.Threading.Timeout.Infinite));

                lock (s_timers)
                {
                    if (!_hasRemoved)
                    {
#if !NO_HASHSET
                        s_timers.Add(_timer);
#else
                        s_timers.Add(_timer, null);
#endif

                        _hasAdded = true;
                    }
                }
            }

            private void Tick(object state)
            {
                try
                {
                    _disposable.Disposable = _action(_parent, _state);
                }
                finally
                {
                    Unroot();
                }
            }

            private void Unroot()
            {
                _action = Nop;

                var timer = default(System.Threading.Timer);

                lock (s_timers)
                {
                    if (!_hasRemoved)
                    {
                        timer = _timer;
                        _timer = null;

                        if (_hasAdded && timer != null)
                            s_timers.Remove(timer);

                        _hasRemoved = true;
                    }
                }

                if (timer != null)
                    timer.Dispose();
            }

            private IDisposable Nop(IScheduler scheduler, TState state)
            {
                return Disposable.Empty;
            }

            public void Dispose()
            {
                _disposable.Dispose();
            }
        }

        abstract class PeriodicTimer
        {
            //
            // Note: the dictionary exists to "root" the timers so that they are not garbage collected and finalized while they are running.
            //
#if !NO_HASHSET
            protected static readonly HashSet<System.Threading.Timer> s_timers = new HashSet<System.Threading.Timer>();
#else
            protected static readonly Dictionary<System.Threading.Timer, object> s_timers = new Dictionary<System.Threading.Timer, object>();
#endif
        }

        sealed class PeriodicTimer<TState> : PeriodicTimer, IDisposable
        {
            private readonly AsyncLock _gate;

            private TState _state;
            private Func<TState, TState> _action;
            private System.Threading.Timer _timer;

            public PeriodicTimer(TState state, TimeSpan period, Func<TState, TState> action)
            {
                _gate = new AsyncLock();

                _state = state;
                _action = action;
                _timer = new System.Threading.Timer(Tick, null, period, period);

                lock (s_timers)
                {
#if !NO_HASHSET
                    s_timers.Add(_timer);
#else
                    s_timers.Add(_timer, null);
#endif
                }
            }

            private void Tick(object state)
            {
                _gate.Wait(() =>
                {
                    _state = _action(_state);
                });
            }

            public void Dispose()
            {
                var timer = default(System.Threading.Timer);

                lock (s_timers)
                {
                    timer = _timer;
                    _timer = null;

                    if (timer != null)
                        s_timers.Remove(timer);
                }

                if (timer != null)
                {
                    timer.Dispose();
                    _gate.Dispose();
                    _action = Stubs<TState>.I;
                }
            }
        }
#endif
    }
}
#endif
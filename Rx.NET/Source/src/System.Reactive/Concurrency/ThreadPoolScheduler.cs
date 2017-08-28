// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the Apache 2.0 License.
// See the LICENSE file in the project root for more information. 

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
        private static readonly Lazy<ThreadPoolScheduler> s_instance = new Lazy<ThreadPoolScheduler>(() => new ThreadPoolScheduler());
        private static readonly Lazy<NewThreadScheduler> s_newBackgroundThread = new Lazy<NewThreadScheduler>(() => new NewThreadScheduler(action => new Thread(action) { IsBackground = true }));

        /// <summary>
        /// Gets the singleton instance of the CLR thread pool scheduler.
        /// </summary>
        public static ThreadPoolScheduler Instance => s_instance.Value;

        private ThreadPoolScheduler()
        {
        }

        /// <summary>
        /// Schedules an action to be executed.
        /// </summary>
        /// <typeparam name="TState">The type of the state passed to the scheduled action.</typeparam>
        /// <param name="state">State passed to the action to be executed.</param>
        /// <param name="action">Action to be executed.</param>
        /// <returns>The disposable object used to cancel the scheduled action (best effort).</returns>
        /// <exception cref="ArgumentNullException"><paramref name="action"/> is <c>null</c>.</exception>
        public override IDisposable Schedule<TState>(TState state, Func<IScheduler, TState, IDisposable> action)
        {
            if (action == null)
                throw new ArgumentNullException(nameof(action));

            var d = new SingleAssignmentDisposable();

            ThreadPool.QueueUserWorkItem(_ =>
            {
                if (!d.IsDisposed)
                {
                    d.Disposable = action(this, state);
                }
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
        /// <exception cref="ArgumentNullException"><paramref name="action"/> is <c>null</c>.</exception>
        public override IDisposable Schedule<TState>(TState state, TimeSpan dueTime, Func<IScheduler, TState, IDisposable> action)
        {
            if (action == null)
                throw new ArgumentNullException(nameof(action));

            var dt = Scheduler.Normalize(dueTime);
            if (dt.Ticks == 0)
            {
                return Schedule(state, action);
            }

            return new Timer<TState>(this, state, dt, action);
        }

        /// <summary>
        /// Schedules a long-running task by creating a new thread. Cancellation happens through polling.
        /// </summary>
        /// <typeparam name="TState">The type of the state passed to the scheduled action.</typeparam>
        /// <param name="state">State passed to the action to be executed.</param>
        /// <param name="action">Action to be executed.</param>
        /// <returns>The disposable object used to cancel the scheduled action (best effort).</returns>
        /// <exception cref="ArgumentNullException"><paramref name="action"/> is <c>null</c>.</exception>
        public IDisposable ScheduleLongRunning<TState>(TState state, Action<TState, ICancelable> action)
        {
            if (action == null)
                throw new ArgumentNullException(nameof(action));

            return s_newBackgroundThread.Value.ScheduleLongRunning(state, action);
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
        /// Schedules a periodic piece of work, using a System.Threading.Timer object.
        /// </summary>
        /// <typeparam name="TState">The type of the state passed to the scheduled action.</typeparam>
        /// <param name="state">Initial state passed to the action upon the first iteration.</param>
        /// <param name="period">Period for running the work periodically.</param>
        /// <param name="action">Action to be executed, potentially updating the state.</param>
        /// <returns>The disposable object used to cancel the scheduled recurring action (best effort).</returns>
        /// <exception cref="ArgumentNullException"><paramref name="action"/> is <c>null</c>.</exception>
        /// <exception cref="ArgumentOutOfRangeException"><paramref name="period"/> is less than zero.</exception>
        public IDisposable SchedulePeriodic<TState>(TState state, TimeSpan period, Func<TState, TState> action)
        {
            if (period < TimeSpan.Zero)
                throw new ArgumentOutOfRangeException(nameof(period));
            if (action == null)
                throw new ArgumentNullException(nameof(action));

            if (period == TimeSpan.Zero)
            {
                return new FastPeriodicTimer<TState>(state, action);
            }
            else
            {
                return new PeriodicTimer<TState>(state, period, action);
            }
        }

        private sealed class FastPeriodicTimer<TState> : IDisposable
        {
            private TState _state;
            private Func<TState, TState> _action;
            private volatile bool _disposed;

            public FastPeriodicTimer(TState state, Func<TState, TState> action)
            {
                _state = state;
                _action = action;

                ThreadPool.QueueUserWorkItem(Tick, null);
            }

            private void Tick(object state)
            {
                if (!_disposed)
                {
                    _state = _action(_state);
                    ThreadPool.QueueUserWorkItem(Tick, null);
                }
            }

            public void Dispose()
            {
                _disposed = true;
                _action = Stubs<TState>.I;
            }
        }

        //
        // See ConcurrencyAbstractionLayerImpl.cs for more information about the code
        // below and its timer rooting behavior.
        //

        private sealed class Timer<TState> : IDisposable
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

            private bool IsTimerAssigned() => _timer != null;

            public void Dispose() => _disposable.Dispose();

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

            private IDisposable Nop(IScheduler scheduler, TState state) => Disposable.Empty;
        }

        private sealed class PeriodicTimer<TState> : IDisposable
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
    }
}
#endif

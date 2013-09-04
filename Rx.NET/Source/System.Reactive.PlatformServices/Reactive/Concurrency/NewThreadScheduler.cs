// Copyright (c) Microsoft Open Technologies, Inc. All rights reserved. See License.txt in the project root for license information.

using System.Reactive.Disposables;
using System.Threading;

namespace System.Reactive.Concurrency
{
    /// <summary>
    /// Represents an object that schedules each unit of work on a separate thread.
    /// </summary>
    public sealed class NewThreadScheduler : LocalScheduler, ISchedulerLongRunning, ISchedulerPeriodic
    {
        internal static readonly NewThreadScheduler s_instance = new NewThreadScheduler();

        private readonly Func<ThreadStart, Thread> _threadFactory;

        /// <summary>
        /// Creates an object that schedules each unit of work on a separate thread.
        /// </summary>
        public NewThreadScheduler()
            : this(action => new Thread(action))
        {
        }

        /// <summary>
        /// Gets an instance of this scheduler that uses the default Thread constructor.
        /// </summary>
        public static NewThreadScheduler Default
        {
            get
            {
                return s_instance;
            }
        }

#if !NO_THREAD
        /// <summary>
        /// Creates an object that schedules each unit of work on a separate thread.
        /// </summary>
        /// <param name="threadFactory">Factory function for thread creation.</param>
        /// <exception cref="ArgumentNullException"><paramref name="threadFactory"/> is null.</exception>
        public NewThreadScheduler(Func<ThreadStart, Thread> threadFactory)
        {
            if (threadFactory == null)
                throw new ArgumentNullException("threadFactory");
#else
        private NewThreadScheduler(Func<ThreadStart, Thread> threadFactory)
        {
#endif
            _threadFactory = threadFactory;
        }

        /// <summary>
        /// Schedules an action to be executed after dueTime.
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

            var scheduler = new EventLoopScheduler(_threadFactory);
            scheduler.ExitIfEmpty = true;
            return scheduler.Schedule(state, dueTime, action);
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

            var d = new BooleanDisposable();

            var thread = _threadFactory(() =>
            {
                //
                // Notice we don't check d.IsDisposed. The contract for ISchedulerLongRunning
                // requires us to ensure the scheduled work gets an opportunity to observe
                // the cancellation request.
                //
                action(state, d);
            });

            thread.Start();

            return d;
        }

        /// <summary>
        /// Schedules a periodic piece of work by creating a new thread that goes to sleep when work has been dispatched and wakes up again at the next periodic due time.
        /// </summary>
        /// <typeparam name="TState">The type of the state passed to the scheduled action.</typeparam>
        /// <param name="state">Initial state passed to the action upon the first iteration.</param>
        /// <param name="period">Period for running the work periodically.</param>
        /// <param name="action">Action to be executed, potentially updating the state.</param>
        /// <returns>The disposable object used to cancel the scheduled recurring action (best effort).</returns>
        /// <exception cref="ArgumentNullException"><paramref name="action"/> is null.</exception>
        /// <exception cref="ArgumentOutOfRangeException"><paramref name="period"/> is less than TimeSpan.Zero.</exception>
        public IDisposable SchedulePeriodic<TState>(TState state, TimeSpan period, Func<TState, TState> action)
        {
            if (period < TimeSpan.Zero)
                throw new ArgumentOutOfRangeException("period");
            if (action == null)
                throw new ArgumentNullException("action");

            var periodic = new Periodic<TState>(state, period, action);

            var thread = _threadFactory(periodic.Run);
            thread.Start();

            return periodic;
        }

        class Periodic<TState> : IDisposable
        {
            private readonly IStopwatch _stopwatch;
            private readonly TimeSpan _period;
            private readonly Func<TState, TState> _action;

            private readonly object _cancel = new object();
            private volatile bool _done;

            private TState _state;
            private TimeSpan _next;

            public Periodic(TState state, TimeSpan period, Func<TState, TState> action)
            {
                _stopwatch = ConcurrencyAbstractionLayer.Current.StartStopwatch();

                _period = period;
                _action = action;

                _state = state;
                _next = period;
            }

            public void Run()
            {
                while (!_done)
                {
                    var timeout = Scheduler.Normalize(_next - _stopwatch.Elapsed);

                    lock (_cancel)
                    {
                        if (Monitor.Wait(_cancel, timeout))
                            return;
                    }

                    _state = _action(_state);
                    _next += _period;
                }
            }

            public void Dispose()
            {
                _done = true;

                lock (_cancel)
                {
                    Monitor.Pulse(_cancel);
                }
            }
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
    }
}
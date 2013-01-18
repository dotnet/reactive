// Copyright (c) Microsoft Open Technologies, Inc. All rights reserved. See License.txt in the project root for license information.

#if WINDOWS
using System.Reactive.Concurrency;
using System.Reactive.Disposables;
using Windows.System.Threading;

namespace System.Reactive.Concurrency
{
    /// <summary>
    /// Represents an object that schedules units of work on the Windows Runtime thread pool.
    /// </summary>
    /// <seealso cref="ThreadPoolScheduler.Default">Singleton instance of this type exposed through this static property.</seealso>
    [CLSCompliant(false)]
    public sealed class ThreadPoolScheduler : LocalScheduler, ISchedulerPeriodic
    {
        private readonly WorkItemPriority _priority;
        private readonly WorkItemOptions _options;
        private static Lazy<ThreadPoolScheduler> s_default = new Lazy<ThreadPoolScheduler>(() => new ThreadPoolScheduler());

        /// <summary>
        /// Constructs a ThreadPoolScheduler that schedules units of work on the Windows ThreadPool.
        /// </summary>
        public ThreadPoolScheduler()
        {
        }

        /// <summary>
        /// Constructs a ThreadPoolScheduler that schedules units of work on the Windows ThreadPool with the given priority.
        /// </summary>
        /// <param name="priority">Priority for scheduled units of work.</param>
        public ThreadPoolScheduler(WorkItemPriority priority)
        {
            _priority = priority;
            _options = WorkItemOptions.None;
        }

        /// <summary>
        /// Constructs a ThreadPoolScheduler that schedules units of work on the Windows ThreadPool with the given priority.
        /// </summary>
        /// <param name="priority">Priority for scheduled units of work.</param>
        /// <param name="options">Options that configure how work is scheduled.</param>
        public ThreadPoolScheduler(WorkItemPriority priority, WorkItemOptions options)
        {
            _priority = priority;
            _options = options;
        }

        /// <summary>
        /// Gets the singleton instance of the Windows Runtime thread pool scheduler.
        /// </summary>
        public static ThreadPoolScheduler Default
        {
            get
            {
                return s_default.Value;
            }
        }

        /// <summary>
        /// Gets the priority at which work is scheduled.
        /// </summary>
        public WorkItemPriority Priority
        {
            get { return _priority; }
        }

        /// <summary>
        /// Gets the options that configure how work is scheduled.
        /// </summary>
        public WorkItemOptions Options
        {
            get { return _options; }
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

            var res = global::Windows.System.Threading.ThreadPool.RunAsync(iaa =>
            {
                if (!d.IsDisposed)
                    d.Disposable = action(this, state);
            }, _priority, _options);

            return new CompositeDisposable(
                d,
                Disposable.Create(res.Cancel)
            );
        }

        /// <summary>
        /// Schedules an action to be executed after dueTime, using a Windows.System.Threading.ThreadPoolTimer object.
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

            var d = new SingleAssignmentDisposable();

            var res = global::Windows.System.Threading.ThreadPoolTimer.CreateTimer(
                tpt =>
                {
                    if (!d.IsDisposed)
                        d.Disposable = action(this, state);
                },
                dt
            );

            return new CompositeDisposable(
                d,
                Disposable.Create(res.Cancel)
            );
        }

        /// <summary>
        /// Schedules a periodic piece of work, using a Windows.System.Threading.ThreadPoolTimer object.
        /// </summary>
        /// <typeparam name="TState">The type of the state passed to the scheduled action.</typeparam>
        /// <param name="state">Initial state passed to the action upon the first iteration.</param>
        /// <param name="period">Period for running the work periodically.</param>
        /// <param name="action">Action to be executed, potentially updating the state.</param>
        /// <returns>The disposable object used to cancel the scheduled recurring action (best effort).</returns>
        /// <exception cref="ArgumentNullException"><paramref name="action"/> is null.</exception>
        /// <exception cref="ArgumentOutOfRangeException"><paramref name="period"/> is less than one millisecond.</exception>
        public IDisposable SchedulePeriodic<TState>(TState state, TimeSpan period, Func<TState, TState> action)
        {
            //
            // The WinRT thread pool is based on the Win32 thread pool and cannot handle
            // sub-1ms resolution. When passing a lower period, we get single-shot
            // timer behavior instead. See MSDN documentation for CreatePeriodicTimer
            // for more information.
            //
            if (period < TimeSpan.FromMilliseconds(1))
                throw new ArgumentOutOfRangeException("period", Strings_PlatformServices.WINRT_NO_SUB1MS_TIMERS);
            if (action == null)
                throw new ArgumentNullException("action");

            var state1 = state;
            var gate = new AsyncLock();

            var res = global::Windows.System.Threading.ThreadPoolTimer.CreatePeriodicTimer(
                tpt =>
                {
                    gate.Wait(() =>
                    {
                        state1 = action(state1);
                    });
                },
                period
            );

            return Disposable.Create(() =>
            {
                res.Cancel();
                gate.Dispose();
                action = Stubs<TState>.I;
            });
        }
    }
}
#endif
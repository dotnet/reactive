// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information. 

#if LEGACY_WINRT
using System.ComponentModel;
using Windows.System.Threading;

namespace System.Reactive.Concurrency
{
    /// <summary>
    /// Represents an object that schedules units of work on the Windows Runtime thread pool.
    /// </summary>
    /// <seealso cref="Default">Singleton instance of this type exposed through this static property.</seealso>
    [CLSCompliant(false)]
    public sealed class ThreadPoolScheduler : LocalScheduler, ISchedulerPeriodic
    {
#pragma warning disable CS0618 // Type or member is obsolete. The non-UWP ThreadPoolScheduler (which will eventually supersede this) defines the zero-args constructor as private, so it's only the accessibility of "public" that is obsolete, not the presence of the constructor. So this warning is spurious in this particular case.
        private static readonly Lazy<ThreadPoolScheduler> LazyDefault = new(static () => new ThreadPoolScheduler());
#pragma warning restore CS0618

        /// <summary>
        /// Constructs a ThreadPoolScheduler that schedules units of work on the Windows ThreadPool.
        /// </summary>
        [Obsolete("If you require the UWP-specific features of ThreadPoolScheduler use the UwpThreadPoolScheduler in the System.Reactive.For.Uwp package. Otherwise, use the Instance property, because this constructor will be removed in a future version (because UWP applications will end up with the same ThreadPoolScheduler as all other application types).")]
        public ThreadPoolScheduler()
        {
        }

        /// <summary>
        /// Constructs a ThreadPoolScheduler that schedules units of work on the Windows ThreadPool with the given priority.
        /// </summary>
        /// <param name="priority">Priority for scheduled units of work.</param>
        [Obsolete("If you require the UWP-specific features of ThreadPoolScheduler use the UwpThreadPoolScheduler in the System.Reactive.For.Uwp package. Otherwise, use the Instance property, because this constructor will be removed in a future version (because UWP applications will end up with the same ThreadPoolScheduler as all other application types).")]
        public ThreadPoolScheduler(WorkItemPriority priority)
        {
            Priority = priority;
            Options = WorkItemOptions.None;
        }

        /// <summary>
        /// Constructs a ThreadPoolScheduler that schedules units of work on the Windows ThreadPool with the given priority.
        /// </summary>
        /// <param name="priority">Priority for scheduled units of work.</param>
        /// <param name="options">Options that configure how work is scheduled.</param>
        [Obsolete("If you require the UWP-specific features of ThreadPoolScheduler use the UwpThreadPoolScheduler in the System.Reactive.For.Uwp package. Otherwise, use the Instance property, because this constructor will be removed in a future version (because UWP applications will end up with the same ThreadPoolScheduler as all other application types).")]
        public ThreadPoolScheduler(WorkItemPriority priority, WorkItemOptions options)
        {
            Priority = priority;
            Options = options;
        }

        /// <summary>
        /// Gets the singleton instance of the Windows Runtime thread pool scheduler.
        /// </summary>
        [Obsolete("Use the Instance property", false)]
        [EditorBrowsable(EditorBrowsableState.Never)]
        public static ThreadPoolScheduler Default => LazyDefault.Value;

        /// <summary>
        /// Gets the singleton instance of the Windows Runtime thread pool scheduler.
        /// </summary>
        public static ThreadPoolScheduler Instance => LazyDefault.Value;

        /// <summary>
        /// Gets the priority at which work is scheduled.
        /// </summary>
        [Obsolete("If you require the UWP-specific features of ThreadPoolScheduler use the UwpThreadPoolScheduler in the System.Reactive.For.Uwp package. This property will be removed in a future version (because UWP applications will end up with the same ThreadPoolScheduler as all other application types).")]
        public WorkItemPriority Priority { get; }

        /// <summary>
        /// Gets the options that configure how work is scheduled.
        /// </summary>
        [Obsolete("If you require the UWP-specific features of ThreadPoolScheduler use the UwpThreadPoolScheduler in the System.Reactive.For.Uwp package. This property will be removed in a future version (because UWP applications will end up with the same ThreadPoolScheduler as all other application types).")]
        public WorkItemOptions Options { get; }

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
                throw new ArgumentNullException(nameof(action));

            var userWorkItem = new UserWorkItem<TState>(this, state, action);

#pragma warning disable CS0618 // Type or member is obsolete.
            // A note on obsolescence:
            //  The compiler complains because this uses Priority and Options. We could mark the
            //  whole method as obsolete, but this would be slightly misleading because when we
            // eventually remove the obsoleted UWP support, this whole ThreadPoolScheduler will
            // be replaced by the non-UWP implementation, and that continues to support this
            // Schedule overload. So the method isn't really obsolete - it will continue to be
            // available to UWP apps even after we've removed all UWP-specific code from
            // System.Reactive.
            // An argument in favour of marking the method as Obsolete anyway is that the
            // behaviour will change once we remove UWP code from System.Reactive. However,
            // the change in behaviour is interesting only if you've specified either
            // priority or options for the work items, and all the public methods we supply
            // for that *are* obsolete. So anyone relying on that behaviour will already have
            // received an obsolescence warning, and should move to UwpThreadPoolScheduler.
            // Code that left these with the default values should not be affected by the
            // change to the non-UWP ThreadPoolScheduler, so it would be irksome for them
            // to get an obsolescence warning, particularly since there isn't actually
            // anything they can do about it. If they want to continue using this type in
            // the full knowledge that in a future version that means they'll get the
            // non-UWP version, we want to let them.
            var res = ThreadPool.RunAsync(
                iaa => userWorkItem.Run(),
                Priority,
                Options);
#pragma warning restore CS0618 // Type or member is obsolete

            userWorkItem.CancelQueueDisposable = res.AsDisposable();

            return userWorkItem;
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
                throw new ArgumentNullException(nameof(action));

            var dt = Scheduler.Normalize(dueTime);

            if (dt.Ticks == 0)
            {
                return Schedule(state, action);
            }

            return ScheduleSlow(state, dt, action);
        }

        private IDisposable ScheduleSlow<TState>(TState state, TimeSpan dueTime, Func<IScheduler, TState, IDisposable> action)
        {
            var userWorkItem = new UserWorkItem<TState>(this, state, action);

            var res = ThreadPoolTimer.CreateTimer(
                tpt => userWorkItem.Run(),
                dueTime);

            userWorkItem.CancelQueueDisposable = res.AsDisposable();

            return userWorkItem;
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
                throw new ArgumentOutOfRangeException(nameof(period), Strings_PlatformServices.WINRT_NO_SUB1MS_TIMERS);
            if (action == null)
                throw new ArgumentNullException(nameof(action));

            return new PeriodicallyScheduledWorkItem<TState>(state, period, action);
        }

        private sealed class PeriodicallyScheduledWorkItem<TState> : IDisposable
        {
            private TState _state;
            private Func<TState, TState> _action;

            private readonly ThreadPoolTimer _timer;
            private readonly AsyncLock _gate = new();

            public PeriodicallyScheduledWorkItem(TState state, TimeSpan period, Func<TState, TState> action)
            {
                _state = state;
                _action = action;

                _timer = ThreadPoolTimer.CreatePeriodicTimer(
                    Tick,
                    period);
            }

            private void Tick(ThreadPoolTimer timer)
            {
                _gate.Wait(
                    this,
                    static @this => @this._state = @this._action(@this._state));
            }

            public void Dispose()
            {
                _timer.Cancel();
                _gate.Dispose();
                _action = Stubs<TState>.I;
            }
        }
    }
}
#endif

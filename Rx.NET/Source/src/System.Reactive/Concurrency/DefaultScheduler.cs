// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the Apache 2.0 License.
// See the LICENSE file in the project root for more information. 

using System.Reactive.Disposables;

namespace System.Reactive.Concurrency
{
    /// <summary>
    /// Represents an object that schedules units of work on the platform's default scheduler.
    /// </summary>
    /// <seealso cref="Scheduler.Default">Singleton instance of this type exposed through this static property.</seealso>
    public sealed class DefaultScheduler : LocalScheduler, ISchedulerPeriodic
    {
        private static readonly Lazy<DefaultScheduler> s_instance = new Lazy<DefaultScheduler>(() => new DefaultScheduler());
        private static IConcurrencyAbstractionLayer s_cal = ConcurrencyAbstractionLayer.Current;

        /// <summary>
        /// Gets the singleton instance of the default scheduler.
        /// </summary>
        public static DefaultScheduler Instance => s_instance.Value;

        private DefaultScheduler()
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

            var cancel = s_cal.QueueUserWorkItem(_ =>
            {
                if (!d.IsDisposed)
                {
                    d.Disposable = action(this, state);
                }
            }, null);

            return StableCompositeDisposable.Create(
                d,
                cancel
            );
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
                return Schedule(state, action);

            var d = new SingleAssignmentDisposable();

            var cancel = s_cal.StartTimer(_ =>
            {
                if (!d.IsDisposed)
                {
                    d.Disposable = action(this, state);
                }
            }, null, dt);

            return StableCompositeDisposable.Create(
                d,
                cancel
            );
        }

        /// <summary>
        /// Schedules a periodic piece of work, using a System.Threading.Timer object.
        /// </summary>
        /// <typeparam name="TState">The type of the state passed to the scheduled action.</typeparam>
        /// <param name="state">Initial state passed to the action upon the first iteration.</param>
        /// <param name="period">Period for running the work periodically.</param>
        /// <param name="action">Action to be executed, potentially updating the state.</param>
        /// <returns>The disposable object used to cancel the scheduled recurring action (best effort).</returns>
        /// <exception cref="ArgumentOutOfRangeException"><paramref name="period"/> is less than <see cref="TimeSpan.Zero"/>.</exception>
        /// <exception cref="ArgumentNullException"><paramref name="action"/> is <c>null</c>.</exception>
        public IDisposable SchedulePeriodic<TState>(TState state, TimeSpan period, Func<TState, TState> action)
        {
            if (period < TimeSpan.Zero)
                throw new ArgumentOutOfRangeException(nameof(period));
            if (action == null)
                throw new ArgumentNullException(nameof(action));

            var state1 = state;
            var gate = new AsyncLock();

            var cancel = s_cal.StartPeriodicTimer(() =>
            {
                gate.Wait(() =>
                {
                    state1 = action(state1);
                });
            }, period);

            return Disposable.Create(() =>
            {
                cancel.Dispose();
                gate.Dispose();
                action = Stubs<TState>.I;
            });
        }

        /// <summary>
        /// Discovers scheduler services by interface type.
        /// </summary>
        /// <param name="serviceType">Scheduler service interface type to discover.</param>
        /// <returns>Object implementing the requested service, if available; null otherwise.</returns>
        protected override object GetService(Type serviceType)
        {
            if (serviceType == typeof(ISchedulerLongRunning))
            {
                if (s_cal.SupportsLongRunning)
                {
                    return LongRunning.Instance;
                }
            }

            return base.GetService(serviceType);
        }

        private sealed class LongRunning : ISchedulerLongRunning
        {
            public static ISchedulerLongRunning Instance = new LongRunning();

            public IDisposable ScheduleLongRunning<TState>(TState state, Action<TState, ICancelable> action)
            {
                if (action == null)
                    throw new ArgumentNullException(nameof(action));

                var cancel = new BooleanDisposable();

                DefaultScheduler.s_cal.StartThread(
                    arg =>
                    {
                        var d = (ICancelable)arg;

                        //
                        // Notice we don't check d.IsDisposed. The contract for ISchedulerLongRunning
                        // requires us to ensure the scheduled work gets an opportunity to observe
                        // the cancellation request.
                        //
                        action(state, d);
                    },
                    cancel
                );

                return cancel;
            }
        }
    }
}

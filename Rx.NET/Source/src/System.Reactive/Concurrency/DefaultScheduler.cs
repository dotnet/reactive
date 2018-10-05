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
        private static readonly Lazy<DefaultScheduler> _instance = new Lazy<DefaultScheduler>(() => new DefaultScheduler());
        private static readonly IConcurrencyAbstractionLayer Cal = ConcurrencyAbstractionLayer.Current;

        /// <summary>
        /// Gets the singleton instance of the default scheduler.
        /// </summary>
        public static DefaultScheduler Instance => _instance.Value;

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
            {
                throw new ArgumentNullException(nameof(action));
            }

            var workItem = new UserWorkItem<TState>(this, state, action);

            workItem.CancelQueueDisposable = Cal.QueueUserWorkItem(
                closureWorkItem => ((UserWorkItem<TState>)closureWorkItem).Run(),
                workItem);

            return workItem;
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
            {
                throw new ArgumentNullException(nameof(action));
            }

            var dt = Scheduler.Normalize(dueTime);
            if (dt.Ticks == 0)
            {
                return Schedule(state, action);
            }

            var workItem = new UserWorkItem<TState>(this, state, action);

            workItem.CancelQueueDisposable = Cal.StartTimer(
                closureWorkItem => ((UserWorkItem<TState>)closureWorkItem).Run(),
                workItem,
                dt);

            return workItem;
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
            {
                throw new ArgumentOutOfRangeException(nameof(period));
            }

            if (action == null)
            {
                throw new ArgumentNullException(nameof(action));
            }

            return new PeriodicallyScheduledWorkItem<TState>(state, period, action);
        }

        private sealed class PeriodicallyScheduledWorkItem<TState> : IDisposable
        {
            private TState _state;
            private Func<TState, TState> _action;
            private readonly IDisposable _cancel;
            private readonly AsyncLock _gate = new AsyncLock();

            public PeriodicallyScheduledWorkItem(TState state, TimeSpan period, Func<TState, TState> action)
            {
                _state = state;
                _action = action;

                _cancel = Cal.StartPeriodicTimer(Tick, period);
            }

            private void Tick()
            {
                _gate.Wait(
                    this,
                    closureWorkItem => closureWorkItem._state = closureWorkItem._action(closureWorkItem._state));
            }

            public void Dispose()
            {
                _cancel.Dispose();
                _gate.Dispose();
                _action = Stubs<TState>.I;
            }
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
                if (Cal.SupportsLongRunning)
                {
                    return LongRunning.Instance;
                }
            }

            return base.GetService(serviceType);
        }

        private sealed class LongRunning : ISchedulerLongRunning
        {
            private sealed class LongScheduledWorkItem<TState> : ICancelable
            {
                private readonly TState _state;
                private readonly Action<TState, ICancelable> _action;

                private IDisposable _cancel;

                public LongScheduledWorkItem(TState state, Action<TState, ICancelable> action)
                {
                    _state = state;
                    _action = action;

                    Cal.StartThread(
                        thisObject =>
                        {
                            var @this = (LongScheduledWorkItem<TState>)thisObject;

                            //
                            // Notice we don't check d.IsDisposed. The contract for ISchedulerLongRunning
                            // requires us to ensure the scheduled work gets an opportunity to observe
                            // the cancellation request.
                            //
                            @this._action(@this._state, @this);
                        },
                        this
                    );
                }

                public void Dispose()
                {
                    Disposable.TryDispose(ref _cancel);
                }

                public bool IsDisposed => Disposable.GetIsDisposed(ref _cancel);
            }

            public static readonly ISchedulerLongRunning Instance = new LongRunning();

            public IDisposable ScheduleLongRunning<TState>(TState state, Action<TState, ICancelable> action)
            {
                if (action == null)
                {
                    throw new ArgumentNullException(nameof(action));
                }

                return new LongScheduledWorkItem<TState>(state, action);
            }
        }
    }
}

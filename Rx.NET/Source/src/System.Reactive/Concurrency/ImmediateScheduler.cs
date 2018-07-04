// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the Apache 2.0 License.
// See the LICENSE file in the project root for more information. 

using System.Reactive.Disposables;

namespace System.Reactive.Concurrency
{
    /// <summary>
    /// Represents an object that schedules units of work to run immediately on the current thread.
    /// </summary>
    /// <seealso cref="Scheduler.Immediate">Singleton instance of this type exposed through this static property.</seealso>
    public sealed class ImmediateScheduler : LocalScheduler, IOneTimeScheduler
    {
        private static readonly Lazy<ImmediateScheduler> _instance = new Lazy<ImmediateScheduler>(() => new ImmediateScheduler());

        private ImmediateScheduler()
        {
        }

        /// <summary>
        /// Gets the singleton instance of the immediate scheduler.
        /// </summary>
        public static ImmediateScheduler Instance => _instance.Value;

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
            {
                throw new ArgumentNullException(nameof(action));
            }

            return action(new AsyncLockScheduler(), state);
        }

        /// <summary>
        /// Schedule the immediate execution of an action with the
        /// given associated state.
        /// </summary>
        /// <typeparam name="TState">The type of the state instance.</typeparam>
        /// <param name="state">The state to be handed to the action when run
        /// by this scheduler.</param>
        /// <param name="action">The action to execute on this scheduler.</param>
        /// <returns>The disposable instance that allows canceling the action before it runs.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="action"/> is null.</exception>
        IDisposable IOneTimeScheduler.ScheduleAction<TState>(TState state, Action<TState> action)
        {
            if (action == null)
                throw new ArgumentNullException(nameof(action));
            action(state);
            return Disposable.Empty;
        }

        /// <summary>
        /// Schedule the delayed execution of an action
        /// with the given associated state.
        /// </summary>
        /// <typeparam name="TState">The type of the state instance.</typeparam>
        /// <param name="state">The state to be handed to the action when run
        /// by this scheduler.</param>
        /// <param name="dueTime">The time delay before the action executes.</param>
        /// <param name="action">The action to execute on this scheduler.</param>
        /// <returns>The disposable instance that allows canceling the action before it runs.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="action"/> is null.</exception>
        IDisposable IOneTimeScheduler.ScheduleAction<TState>(TState state, TimeSpan dueTime, Action<TState> action)
        {
            if (action == null)
                throw new ArgumentNullException(nameof(action));

            var dt = Scheduler.Normalize(dueTime);
            if (dt.Ticks > 0)
            {
                ConcurrencyAbstractionLayer.Current.Sleep(dt);
            }
            action(state);
            return Disposable.Empty;
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
            {
                throw new ArgumentNullException(nameof(action));
            }

            var dt = Scheduler.Normalize(dueTime);
            if (dt.Ticks > 0)
            {
                ConcurrencyAbstractionLayer.Current.Sleep(dt);
            }

            return action(new AsyncLockScheduler(), state);
        }

        private sealed class AsyncLockScheduler : LocalScheduler
        {
            private AsyncLock _asyncLock;

            public override IDisposable Schedule<TState>(TState state, Func<IScheduler, TState, IDisposable> action)
            {
                if (action == null)
                {
                    throw new ArgumentNullException(nameof(action));
                }

                var m = new SingleAssignmentDisposable();

                if (_asyncLock == null)
                {
                    _asyncLock = new AsyncLock();
                }

                _asyncLock.Wait(
                    (@this: this, m, action, state),
                    tuple =>
                    {
                        if (!tuple.m.IsDisposed)
                        {
                            tuple.m.Disposable = tuple.action(tuple.@this, tuple.state);
                        }
                    });

                return m;
            }

            public override IDisposable Schedule<TState>(TState state, TimeSpan dueTime, Func<IScheduler, TState, IDisposable> action)
            {
                if (action == null)
                {
                    throw new ArgumentNullException(nameof(action));
                }

                if (dueTime.Ticks <= 0)
                {
                    return Schedule(state, action);
                }

                return ScheduleSlow(state, dueTime, action);
            }

            private IDisposable ScheduleSlow<TState>(TState state, TimeSpan dueTime, Func<IScheduler, TState, IDisposable> action)
            {
                var timer = ConcurrencyAbstractionLayer.Current.StartStopwatch();

                var m = new SingleAssignmentDisposable();

                if (_asyncLock == null)
                {
                    _asyncLock = new AsyncLock();
                }

                _asyncLock.Wait(
                    (@this: this, m, state, action, timer, dueTime),
                    tuple =>
                    {
                        if (!tuple.m.IsDisposed)
                        {
                            var sleep = tuple.dueTime - tuple.timer.Elapsed;
                            if (sleep.Ticks > 0)
                            {
                                ConcurrencyAbstractionLayer.Current.Sleep(sleep);
                            }

                            if (!tuple.m.IsDisposed)
                            {
                                tuple.m.Disposable = tuple.action(tuple.@this, tuple.state);
                            }
                        }
                    });

                return m;
            }
        }
    }
}

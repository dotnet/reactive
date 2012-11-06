// Copyright (c) Microsoft Open Technologies, Inc. All rights reserved. See License.txt in the project root for license information.

using System.Threading;
using System.Reactive.Disposables;

namespace System.Reactive.Concurrency
{
    /// <summary>
    /// Represents an object that schedules units of work to run immediately on the current thread.
    /// </summary>
    /// <seealso cref="Scheduler.Immediate">Singleton instance of this type exposed through this static property.</seealso>
    public sealed class ImmediateScheduler : LocalScheduler
    {
        private static readonly ImmediateScheduler s_instance = new ImmediateScheduler();

        ImmediateScheduler()
        {
        }

        /// <summary>
        /// Gets the singleton instance of the immediate scheduler.
        /// </summary>
        public static ImmediateScheduler Instance
        {
            get { return s_instance; }
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

            return action(new AsyncLockScheduler(), state);
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

            var dt = Scheduler.Normalize(dueTime);
            if (dt.Ticks > 0)
            {
                ConcurrencyAbstractionLayer.Current.Sleep(dt);
            }

            return action(new AsyncLockScheduler(), state);
        }

        class AsyncLockScheduler : LocalScheduler
        {
            AsyncLock asyncLock;

            public override IDisposable Schedule<TState>(TState state, Func<IScheduler, TState, IDisposable> action)
            {
                if (action == null)
                    throw new ArgumentNullException("action");

                var m = new SingleAssignmentDisposable();

                if (asyncLock == null)
                    asyncLock = new AsyncLock();

                asyncLock.Wait(() =>
                {
                    if (!m.IsDisposed)
                        m.Disposable = action(this, state);
                });

                return m;
            }

            public override IDisposable Schedule<TState>(TState state, TimeSpan dueTime, Func<IScheduler, TState, IDisposable> action)
            {
                if (action == null)
                    throw new ArgumentNullException("action");

                if (dueTime.Ticks <= 0)
                    return Schedule<TState>(state, action);

                var timer = ConcurrencyAbstractionLayer.Current.StartStopwatch();

                var m = new SingleAssignmentDisposable();

                if (asyncLock == null)
                    asyncLock = new AsyncLock();

                asyncLock.Wait(() =>
                {
                    if (!m.IsDisposed)
                    {
                        var sleep = dueTime - timer.Elapsed;
                        if (sleep.Ticks > 0)
                        {
                            ConcurrencyAbstractionLayer.Current.Sleep(sleep);
                        }
                        if (!m.IsDisposed)
                            m.Disposable = action(this, state);
                    }
                });

                return m;
            }
        }
    }
}

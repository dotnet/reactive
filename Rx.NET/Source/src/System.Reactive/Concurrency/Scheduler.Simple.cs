// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the Apache 2.0 License.
// See the LICENSE file in the project root for more information. 

using System.Reactive.Disposables;

namespace System.Reactive.Concurrency
{
    public static partial class Scheduler
    {
        /// <summary>
        /// Schedules an action to be executed.
        /// </summary>
        /// <param name="scheduler">Scheduler to execute the action on.</param>
        /// <param name="action">Action to execute.</param>
        /// <returns>The disposable object used to cancel the scheduled action (best effort).</returns>
        /// <exception cref="ArgumentNullException"><paramref name="scheduler"/> or <paramref name="action"/> is <c>null</c>.</exception>
        public static IDisposable Schedule(this IScheduler scheduler, Action action)
        {
            if (scheduler == null)
            {
                throw new ArgumentNullException(nameof(scheduler));
            }

            if (action == null)
            {
                throw new ArgumentNullException(nameof(action));
            }

            // Surprisingly, passing the method group of Invoke will create a fresh
            // delegate each time although it's static, while an anonymous
            // lambda without the need of a closure will be cached.
            // Once Roslyn supports caching delegates for method groups,
            // the anonymous lambda can be replaced by the method group again. Until then,
            // to avoid the repetition of code, the call to Invoke is left intact.
            // Watch https://github.com/dotnet/roslyn/issues/5835
            return scheduler.Schedule(action, (s, a) => Invoke(s, a));
        }

        /// <summary>
        /// Schedules an action to be executed.
        /// </summary>
        /// <param name="scheduler">Scheduler to execute the action on.</param>
        /// <param name="state">A state object to be passed to <paramref name="action"/>.</param>
        /// <param name="action">Action to execute.</param>
        /// <returns>The disposable object used to cancel the scheduled action (best effort).</returns>
        /// <exception cref="ArgumentNullException"><paramref name="scheduler"/> or <paramref name="action"/> is <c>null</c>.</exception>
        // Note: The naming of that method differs because otherwise, the signature would cause ambiguities.
        internal static IDisposable ScheduleAction<TState>(this IScheduler scheduler, TState state, Action<TState> action)
        {
            if (scheduler == null)
            {
                throw new ArgumentNullException(nameof(scheduler));
            }

            if (action == null)
            {
                throw new ArgumentNullException(nameof(action));
            }

            return scheduler.Schedule(
                (action, state),
                (_, tuple) =>
                {
                    tuple.action(tuple.state);
                    return Disposable.Empty;
                });
        }

        /// <summary>
        /// Schedules an action to be executed.
        /// </summary>
        /// <param name="scheduler">Scheduler to execute the action on.</param>
        /// <param name="state">A state object to be passed to <paramref name="action"/>.</param>
        /// <param name="action">Action to execute.</param>
        /// <returns>The disposable object used to cancel the scheduled action (best effort).</returns>
        /// <exception cref="ArgumentNullException"><paramref name="scheduler"/> or <paramref name="action"/> is <c>null</c>.</exception>
        // Note: The naming of that method differs because otherwise, the signature would cause ambiguities.
        internal static IDisposable ScheduleAction<TState>(this IScheduler scheduler, TState state, Func<TState, IDisposable> action)
        {
            if (scheduler == null)
                throw new ArgumentNullException(nameof(scheduler));
            if (action == null)
                throw new ArgumentNullException(nameof(action));

            return scheduler.Schedule(
                (action, state),
                (_, tuple) => tuple.action(tuple.state));
        }

        /// <summary>
        /// Schedules an action to be executed after the specified relative due time.
        /// </summary>
        /// <param name="scheduler">Scheduler to execute the action on.</param>
        /// <param name="action">Action to execute.</param>
        /// <param name="dueTime">Relative time after which to execute the action.</param>
        /// <returns>The disposable object used to cancel the scheduled action (best effort).</returns>
        /// <exception cref="ArgumentNullException"><paramref name="scheduler"/> or <paramref name="action"/> is <c>null</c>.</exception>
        public static IDisposable Schedule(this IScheduler scheduler, TimeSpan dueTime, Action action)
        {
            if (scheduler == null)
            {
                throw new ArgumentNullException(nameof(scheduler));
            }

            if (action == null)
            {
                throw new ArgumentNullException(nameof(action));
            }

            // See note above.
            return scheduler.Schedule(action, dueTime, (s, a) => Invoke(s, a));
        }

        /// <summary>
        /// Schedules an action to be executed after the specified relative due time.
        /// </summary>
        /// <param name="scheduler">Scheduler to execute the action on.</param>
        /// <param name="action">Action to execute.</param>
        /// <param name="state">A state object to be passed to <paramref name="action"/>.</param>
        /// <param name="dueTime">Relative time after which to execute the action.</param>
        /// <returns>The disposable object used to cancel the scheduled action (best effort).</returns>
        /// <exception cref="ArgumentNullException"><paramref name="scheduler"/> or <paramref name="action"/> is <c>null</c>.</exception>
        internal static IDisposable ScheduleAction<TState>(this IScheduler scheduler, TState state, TimeSpan dueTime, Action<TState> action)
        {
            if (scheduler == null)
            {
                throw new ArgumentNullException(nameof(scheduler));
            }

            if (action == null)
            {
                throw new ArgumentNullException(nameof(action));
            }

            // See note above.
            return scheduler.Schedule((state, action), dueTime, (s, tuple) => Invoke(s, tuple));
        }

        /// <summary>
        /// Schedules an action to be executed after the specified relative due time.
        /// </summary>
        /// <param name="scheduler">Scheduler to execute the action on.</param>
        /// <param name="action">Action to execute.</param>
        /// <param name="state">A state object to be passed to <paramref name="action"/>.</param>
        /// <param name="dueTime">Relative time after which to execute the action.</param>
        /// <returns>The disposable object used to cancel the scheduled action (best effort).</returns>
        /// <exception cref="ArgumentNullException"><paramref name="scheduler"/> or <paramref name="action"/> is <c>null</c>.</exception>
        internal static IDisposable ScheduleAction<TState>(this IScheduler scheduler, TState state, TimeSpan dueTime, Func<TState, IDisposable> action)
        {
            if (scheduler == null)
                throw new ArgumentNullException(nameof(scheduler));
            if (action == null)
                throw new ArgumentNullException(nameof(action));

            // See note above.
            return scheduler.Schedule((state, action), dueTime, (s, tuple) => Invoke(s, tuple));
        }

        /// <summary>
        /// Schedules an action to be executed at the specified absolute due time.
        /// </summary>
        /// <param name="scheduler">Scheduler to execute the action on.</param>
        /// <param name="action">Action to execute.</param>
        /// <param name="dueTime">Absolute time at which to execute the action.</param>
        /// <returns>The disposable object used to cancel the scheduled action (best effort).</returns>
        /// <exception cref="ArgumentNullException"><paramref name="scheduler"/> or <paramref name="action"/> is <c>null</c>.</exception>
        public static IDisposable Schedule(this IScheduler scheduler, DateTimeOffset dueTime, Action action)
        {
            if (scheduler == null)
            {
                throw new ArgumentNullException(nameof(scheduler));
            }

            if (action == null)
            {
                throw new ArgumentNullException(nameof(action));
            }

            // See note above.
            return scheduler.Schedule(action, dueTime, (s, a) => Invoke(s, a));
        }

        /// <summary>
        /// Schedules an action to be executed after the specified relative due time.
        /// </summary>
        /// <param name="scheduler">Scheduler to execute the action on.</param>
        /// <param name="action">Action to execute.</param>
        /// <param name="state">A state object to be passed to <paramref name="action"/>.</param>
        /// <param name="dueTime">Relative time after which to execute the action.</param>
        /// <returns>The disposable object used to cancel the scheduled action (best effort).</returns>
        /// <exception cref="ArgumentNullException"><paramref name="scheduler"/> or <paramref name="action"/> is <c>null</c>.</exception>
        internal static IDisposable ScheduleAction<TState>(this IScheduler scheduler, TState state, DateTimeOffset dueTime, Action<TState> action)
        {
            if (scheduler == null)
            {
                throw new ArgumentNullException(nameof(scheduler));
            }

            if (action == null)
            {
                throw new ArgumentNullException(nameof(action));
            }

            // See note above.
            return scheduler.Schedule((state, action), dueTime, (s, tuple) => Invoke(s, tuple));
        }

        /// <summary>
        /// Schedules an action to be executed after the specified relative due time.
        /// </summary>
        /// <param name="scheduler">Scheduler to execute the action on.</param>
        /// <param name="action">Action to execute.</param>
        /// <param name="state">A state object to be passed to <paramref name="action"/>.</param>
        /// <param name="dueTime">Relative time after which to execute the action.</param>
        /// <returns>The disposable object used to cancel the scheduled action (best effort).</returns>
        /// <exception cref="ArgumentNullException"><paramref name="scheduler"/> or <paramref name="action"/> is <c>null</c>.</exception>
        internal static IDisposable ScheduleAction<TState>(this IScheduler scheduler, TState state, DateTimeOffset dueTime, Func<TState, IDisposable> action)
        {
            if (scheduler == null)
                throw new ArgumentNullException(nameof(scheduler));
            if (action == null)
                throw new ArgumentNullException(nameof(action));

            // See note above.
            return scheduler.Schedule((state, action), dueTime, (s, tuple) => Invoke(s, tuple));
        }

        /// <summary>
        /// Schedules an action to be executed.
        /// </summary>
        /// <param name="scheduler">Scheduler to execute the action on.</param>
        /// <param name="action">Action to execute.</param>
        /// <returns>The disposable object used to cancel the scheduled action (best effort).</returns>
        /// <exception cref="ArgumentNullException"><paramref name="scheduler"/> or <paramref name="action"/> is <c>null</c>.</exception>
        public static IDisposable ScheduleLongRunning(this ISchedulerLongRunning scheduler, Action<ICancelable> action)
        {
            if (scheduler == null)
            {
                throw new ArgumentNullException(nameof(scheduler));
            }

            if (action == null)
            {
                throw new ArgumentNullException(nameof(action));
            }

            return scheduler.ScheduleLongRunning(action, (a, c) => a(c));
        }

        private static IDisposable Invoke(IScheduler scheduler, Action action)
        {
            action();
            return Disposable.Empty;
        }

        private static IDisposable Invoke<TState>(IScheduler scheduler, (TState state, Action<TState> action) tuple)
        {
            tuple.action(tuple.state);
            return Disposable.Empty;
        }

        private static IDisposable Invoke<TState>(IScheduler scheduler, (TState state, Func<TState, IDisposable> action) tuple)
        {
            return tuple.action(tuple.state);
        }
    }
}

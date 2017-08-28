// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the Apache 2.0 License.
// See the LICENSE file in the project root for more information. 

using System.Reactive.Disposables;

namespace System.Reactive.Concurrency
{
    public static partial class Scheduler
    {
        /// <summary>
        /// Schedules an action to be executed recursively.
        /// </summary>
        /// <param name="scheduler">Scheduler to execute the recursive action on.</param>
        /// <param name="action">Action to execute recursively. The parameter passed to the action is used to trigger recursive scheduling of the action.</param>
        /// <returns>The disposable object used to cancel the scheduled action (best effort).</returns>
        /// <exception cref="ArgumentNullException"><paramref name="scheduler"/> or <paramref name="action"/> is <c>null</c>.</exception>
        public static IDisposable Schedule(this IScheduler scheduler, Action<Action> action)
        {
            if (scheduler == null)
                throw new ArgumentNullException(nameof(scheduler));
            if (action == null)
                throw new ArgumentNullException(nameof(action));

            return scheduler.Schedule(action, (_action, self) => _action(() => self(_action)));
        }

        /// <summary>
        /// Schedules an action to be executed recursively.
        /// </summary>
        /// <typeparam name="TState">The type of the state passed to the scheduled action.</typeparam>
        /// <param name="scheduler">Scheduler to execute the recursive action on.</param>
        /// <param name="state">State passed to the action to be executed.</param>
        /// <param name="action">Action to execute recursively. The last parameter passed to the action is used to trigger recursive scheduling of the action, passing in recursive invocation state.</param>
        /// <returns>The disposable object used to cancel the scheduled action (best effort).</returns>
        /// <exception cref="ArgumentNullException"><paramref name="scheduler"/> or <paramref name="action"/> is <c>null</c>.</exception>
        public static IDisposable Schedule<TState>(this IScheduler scheduler, TState state, Action<TState, Action<TState>> action)
        {
            if (scheduler == null)
                throw new ArgumentNullException(nameof(scheduler));
            if (action == null)
                throw new ArgumentNullException(nameof(action));

            return scheduler.Schedule(new Pair<TState, Action<TState, Action<TState>>> { First = state, Second = action }, InvokeRec1);
        }

        private static IDisposable InvokeRec1<TState>(IScheduler scheduler, Pair<TState, Action<TState, Action<TState>>> pair)
        {
            var group = new CompositeDisposable(1);
            var gate = new object();
            var state = pair.First;
            var action = pair.Second;

            Action<TState> recursiveAction = null;
            recursiveAction = state1 => action(state1, state2 =>
            {
                var isAdded = false;
                var isDone = false;
                var d = default(IDisposable);
                d = scheduler.Schedule(state2, (scheduler1, state3) =>
                {
                    lock (gate)
                    {
                        if (isAdded)
                        {
                            group.Remove(d);
                        }
                        else
                        {
                            isDone = true;
                        }
                    }
                    recursiveAction(state3);
                    return Disposable.Empty;
                });

                lock (gate)
                {
                    if (!isDone)
                    {
                        group.Add(d);
                        isAdded = true;
                    }
                }
            });

            recursiveAction(state);

            return group;
        }

        /// <summary>
        /// Schedules an action to be executed recursively after a specified relative due time.
        /// </summary>
        /// <param name="scheduler">Scheduler to execute the recursive action on.</param>
        /// <param name="action">Action to execute recursively. The parameter passed to the action is used to trigger recursive scheduling of the action at the specified relative time.</param>
        /// <param name="dueTime">Relative time after which to execute the action for the first time.</param>
        /// <returns>The disposable object used to cancel the scheduled action (best effort).</returns>
        /// <exception cref="ArgumentNullException"><paramref name="scheduler"/> or <paramref name="action"/> is <c>null</c>.</exception>
        public static IDisposable Schedule(this IScheduler scheduler, TimeSpan dueTime, Action<Action<TimeSpan>> action)
        {
            if (scheduler == null)
                throw new ArgumentNullException(nameof(scheduler));
            if (action == null)
                throw new ArgumentNullException(nameof(action));

            return scheduler.Schedule(action, dueTime, (_action, self) => _action(dt => self(_action, dt)));
        }

        /// <summary>
        /// Schedules an action to be executed recursively after a specified relative due time.
        /// </summary>
        /// <typeparam name="TState">The type of the state passed to the scheduled action.</typeparam>
        /// <param name="scheduler">Scheduler to execute the recursive action on.</param>
        /// <param name="state">State passed to the action to be executed.</param>
        /// <param name="action">Action to execute recursively. The last parameter passed to the action is used to trigger recursive scheduling of the action, passing in the recursive due time and invocation state.</param>
        /// <param name="dueTime">Relative time after which to execute the action for the first time.</param>
        /// <returns>The disposable object used to cancel the scheduled action (best effort).</returns>
        /// <exception cref="ArgumentNullException"><paramref name="scheduler"/> or <paramref name="action"/> is <c>null</c>.</exception>
        public static IDisposable Schedule<TState>(this IScheduler scheduler, TState state, TimeSpan dueTime, Action<TState, Action<TState, TimeSpan>> action)
        {
            if (scheduler == null)
                throw new ArgumentNullException(nameof(scheduler));
            if (action == null)
                throw new ArgumentNullException(nameof(action));

            return scheduler.Schedule(new Pair<TState, Action<TState, Action<TState, TimeSpan>>> { First = state, Second = action }, dueTime, InvokeRec2);
        }

        private static IDisposable InvokeRec2<TState>(IScheduler scheduler, Pair<TState, Action<TState, Action<TState, TimeSpan>>> pair)
        {
            var group = new CompositeDisposable(1);
            var gate = new object();
            var state = pair.First;
            var action = pair.Second;

            Action<TState> recursiveAction = null;
            recursiveAction = state1 => action(state1, (state2, dueTime1) =>
            {
                var isAdded = false;
                var isDone = false;
                var d = default(IDisposable);
                d = scheduler.Schedule(state2, dueTime1, (scheduler1, state3) =>
                {
                    lock (gate)
                    {
                        if (isAdded)
                        {
                            group.Remove(d);
                        }
                        else
                        {
                            isDone = true;
                        }
                    }
                    recursiveAction(state3);
                    return Disposable.Empty;
                });
                
                lock (gate)
                {
                    if (!isDone)
                    {
                        group.Add(d);
                        isAdded = true;
                    }
                }
            });

            recursiveAction(state);

            return group;
        }

        /// <summary>
        /// Schedules an action to be executed recursively at a specified absolute due time.
        /// </summary>
        /// <param name="scheduler">Scheduler to execute the recursive action on.</param>
        /// <param name="action">Action to execute recursively. The parameter passed to the action is used to trigger recursive scheduling of the action at the specified absolute time.</param>
        /// <param name="dueTime">Absolute time at which to execute the action for the first time.</param>
        /// <returns>The disposable object used to cancel the scheduled action (best effort).</returns>
        /// <exception cref="ArgumentNullException"><paramref name="scheduler"/> or <paramref name="action"/> is <c>null</c>.</exception>
        public static IDisposable Schedule(this IScheduler scheduler, DateTimeOffset dueTime, Action<Action<DateTimeOffset>> action)
        {
            if (scheduler == null)
                throw new ArgumentNullException(nameof(scheduler));
            if (action == null)
                throw new ArgumentNullException(nameof(action));

            return scheduler.Schedule(action, dueTime, (_action, self) => _action(dt => self(_action, dt)));
        }

        /// <summary>
        /// Schedules an action to be executed recursively at a specified absolute due time.
        /// </summary>
        /// <typeparam name="TState">The type of the state passed to the scheduled action.</typeparam>
        /// <param name="scheduler">Scheduler to execute the recursive action on.</param>
        /// <param name="state">State passed to the action to be executed.</param>
        /// <param name="action">Action to execute recursively. The last parameter passed to the action is used to trigger recursive scheduling of the action, passing in the recursive due time and invocation state.</param>
        /// <param name="dueTime">Absolute time at which to execute the action for the first time.</param>
        /// <returns>The disposable object used to cancel the scheduled action (best effort).</returns>
        /// <exception cref="ArgumentNullException"><paramref name="scheduler"/> or <paramref name="action"/> is <c>null</c>.</exception>
        public static IDisposable Schedule<TState>(this IScheduler scheduler, TState state, DateTimeOffset dueTime, Action<TState, Action<TState, DateTimeOffset>> action)
        {
            if (scheduler == null)
                throw new ArgumentNullException(nameof(scheduler));
            if (action == null)
                throw new ArgumentNullException(nameof(action));

            return scheduler.Schedule(new Pair<TState, Action<TState, Action<TState, DateTimeOffset>>> { First = state, Second = action }, dueTime, InvokeRec3);
        }

        private static IDisposable InvokeRec3<TState>(IScheduler scheduler, Pair<TState, Action<TState, Action<TState, DateTimeOffset>>> pair)
        {
            var group = new CompositeDisposable(1);
            var gate = new object();
            var state = pair.First;
            var action = pair.Second;

            Action<TState> recursiveAction = null;
            recursiveAction = state1 => action(state1, (state2, dueTime1) =>
            {
                var isAdded = false;
                var isDone = false;
                var d = default(IDisposable);
                d = scheduler.Schedule(state2, dueTime1, (scheduler1, state3) =>
                {
                    lock (gate)
                    {
                        if (isAdded)
                        {
                            group.Remove(d);
                        }
                        else
                        {
                            isDone = true;
                        }
                    }
                    recursiveAction(state3);
                    return Disposable.Empty;
                });

                lock (gate)
                {
                    if (!isDone)
                    {
                        group.Add(d);
                        isAdded = true;
                    }
                }
            });

            recursiveAction(state);

            return group;
        }

#if !NO_SERIALIZABLE
        [Serializable]
#endif
        private struct Pair<T1, T2>
        {
            public T1 First;
            public T2 Second;
        }
    }
}

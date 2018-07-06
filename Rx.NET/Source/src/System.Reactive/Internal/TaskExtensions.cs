// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the Apache 2.0 License.
// See the LICENSE file in the project root for more information.

namespace System.Threading.Tasks
{
    internal static class TaskExtensions
    {
        public static Task ContinueWithState<TState>(this Task task, Action<Task, TState> continuationAction, TState state)
        {
            return task.ContinueWith(
                (t, tupleObject) =>
                {
                    var (closureAction, closureState) = ((Action<Task, TState>, TState))tupleObject;

                    closureAction(t, closureState);
                },
                (continuationAction, state));
        }

        public static Task ContinueWithState<TState>(this Task task, Action<Task, TState> continuationAction, TState state, CancellationToken cancellationToken)
        {
            return task.ContinueWith(
                (t, tupleObject) =>
                {
                    var (closureAction, closureState) = ((Action<Task, TState>, TState))tupleObject;

                    closureAction(t, closureState);
                },
                (continuationAction, state),
                cancellationToken);
        }

        public static Task ContinueWithState<TState>(this Task task, Action<Task, TState> continuationAction, TState state, TaskContinuationOptions continuationOptions)
        {
            return task.ContinueWith(
                (t, tupleObject) =>
                {
                    var (closureAction, closureState) = ((Action<Task, TState>, TState))tupleObject;

                    closureAction(t, closureState);
                },
                (continuationAction, state),
                continuationOptions);
        }

        public static Task ContinueWithState<TState>(this Task task, Action<Task, TState> continuationAction, TState state, CancellationToken cancellationToken, TaskContinuationOptions continuationOptions)
        {
            return task.ContinueWith(
                (t, tupleObject) =>
                {
                    var (closureAction, closureState) = ((Action<Task, TState>, TState))tupleObject;

                    closureAction(t, closureState);
                },
                (continuationAction, state),
                cancellationToken,
                continuationOptions,
                TaskScheduler.Default);
        }

        public static Task ContinueWithState<TResult, TState>(this Task<TResult> task, Action<Task<TResult>, TState> continuationAction, TState state)
        {
            return task.ContinueWith(
                (t, tupleObject) =>
                {
                    var (closureAction, closureState) = ((Action<Task<TResult>, TState>, TState))tupleObject;

                    closureAction(t, closureState);
                },
                (continuationAction, state));
        }

        public static Task ContinueWithState<TResult, TState>(this Task<TResult> task, Action<Task<TResult>, TState> continuationAction, TState state, CancellationToken cancellationToken)
        {
            return task.ContinueWith(
                (t, tupleObject) =>
                {
                    var (closureAction, closureState) = ((Action<Task<TResult>, TState>, TState))tupleObject;

                    closureAction(t, closureState);
                },
                (continuationAction, state),
                cancellationToken);
        }

        public static Task ContinueWithState<TResult, TState>(this Task<TResult> task, Action<Task<TResult>, TState> continuationAction, TState state, TaskContinuationOptions continuationOptions)
        {
            return task.ContinueWith(
                (t, tupleObject) =>
                {
                    var (closureAction, closureState) = ((Action<Task<TResult>, TState>, TState))tupleObject;

                    closureAction(t, closureState);
                },
                (continuationAction, state),
                continuationOptions);
        }

        public static Task ContinueWithState<TResult, TState>(this Task<TResult> task, Action<Task<TResult>, TState> continuationAction, TState state, CancellationToken cancellationToken, TaskContinuationOptions continuationOptions)
        {
            return task.ContinueWith(
                (t, tupleObject) =>
                {
                    var (closureAction, closureState) = ((Action<Task<TResult>, TState>, TState))tupleObject;

                    closureAction(t, closureState);
                },
                (continuationAction, state),
                cancellationToken,
                continuationOptions,
                TaskScheduler.Default);
        }
    }
}

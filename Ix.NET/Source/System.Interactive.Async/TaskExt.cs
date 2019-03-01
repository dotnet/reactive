// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the Apache 2.0 License.
// See the LICENSE file in the project root for more information. 

using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.CompilerServices;

namespace System.Threading.Tasks
{
    internal static class TaskExt
    {
        public static readonly Task<bool> Never = new TaskCompletionSource<bool>().Task;

#if USE_FAIR_AND_CHEAPER_MERGE
        public static WhenAnyValueTask<T> WhenAny<T>(ValueTask<T>[] tasks)
        {
            var whenAny = new WhenAnyValueTask<T>(tasks);

            whenAny.Start();

            return whenAny;
        }

        // REVIEW: Evaluate options to reduce locking and test performance. Right now, there's one lock
        //         protecting the queue and the completion delegate field. Care has been taken to limit
        //         the time under the lock, and the (sequential single) reader path has limited locking.
        //         Contention due to concurrent completion of tasks could be a concern.

        internal sealed class WhenAnyValueTask<T>
        {
            /// <summary>
            /// The tasks to await. Entries in this array may be replaced using <see cref="Replace"/>.
            /// </summary>
            private readonly ValueTask<T>[] _tasks;

            /// <summary>
            /// Array of cached delegates passed to awaiters on tasks. These delegates have a closure containing the task index.
            /// </summary>
            private readonly Action[] _onReady;

            /// <summary>
            /// Queue of indexes of ready tasks. Awaiting the <see cref="WhenAnyValueTask{T}"/> object will consume this queue in order.
            /// </summary>
            /// <remarks>
            /// A lock on this field is taken when updating the queue or <see cref="_onCompleted"/>.
            /// </remarks>
            private readonly Queue<int> _ready;

            /// <summary>
            /// Callback of the current awaiter, if any.
            /// </summary>
            /// <remarks>
            /// Protected for reads and writes by a lock on <see cref="_ready"/>.
            /// </remarks>
            private Action _onCompleted;

            /// <summary>
            /// Creates a when any task around the specified tasks.
            /// </summary>
            /// <param name="tasks">Initial set of tasks to await.</param>
            public WhenAnyValueTask(ValueTask<T>[] tasks)
            {
                _tasks = tasks;

                var n = tasks.Length;

                _ready = new Queue<int>(n); // NB: Should never exceed this length, so we won't see dynamic realloc.
                _onReady = new Action[n];

                for (var i = 0; i < n; i++)
                {
                    //
                    // Cache these delegates, for they have closures (over `this` and `index`), and we need them
                    // for each replacement of a task, to hook up the continuation.
                    //

                    int index = i;
                    _onReady[index] = () => OnReady(index);
                }
            }

            /// <summary>
            /// Start awaiting the tasks. This is done separately from the constructor to avoid complexity dealing
            /// with handling concurrent callbacks to the current instance while the constructor is running.
            /// </summary>
            public void Start()
            {
                for (var i = 0; i < _tasks.Length; i++)
                {
                    //
                    // Register a callback with the task, which will enqueue the index of the completed task
                    // for consumption by awaiters.
                    //

                    _tasks[i].ConfigureAwait(false).GetAwaiter().OnCompleted(_onReady[i]);
                }
            }

            /// <summary>
            /// Gets an awaiter to await completion of any of the awaited tasks, returning the index of the completed
            /// task. When sequentially awaiting the current instance, task indices are yielded in the order that of
            /// completion. If all tasks have completed and been observed by awaiting the current instance, the awaiter
            /// never returns on a subsequent attempt to await the completion of any task. The caller is responsible
            /// for bookkeeping that avoids awaiting this instance more often than the number of pending tasks.
            /// </summary>
            /// <returns>Awaiter to await completion of any of the awaited task.</returns>
            /// <remarks>This class only supports a single active awaiter at any point in time.</remarks>
            public Awaiter GetAwaiter() => new Awaiter(this);

            /// <summary>
            /// Replaces the (completed) task at the specified <paramref name="index"/> and starts awaiting it.
            /// </summary>
            /// <param name="index">The index of the parameter to replace.</param>
            /// <param name="task">The new task to store and await at the specified index.</param>
            public void Replace(int index, in ValueTask<T> task)
            {
                Debug.Assert(_tasks[index].IsCompleted, "A task shouldn't be replaced before it has completed.");

                _tasks[index] = task;

                task.ConfigureAwait(false).GetAwaiter().OnCompleted(_onReady[index]);
            }

            /// <summary>
            /// Called when any task has completed (thus may run concurrently).
            /// </summary>
            /// <param name="index">The index of the completed task in <see cref="_tasks"/>.</param>
            private void OnReady(int index)
            {
                Action onCompleted = null;

                lock (_ready)
                {
                    //
                    // Store the index of the task that has completed. This will be picked up from GetResult.
                    //

                    _ready.Enqueue(index);

                    //
                    // If there's a current awaiter, we'll steal its continuation action and invoke it. By setting
                    // the continuation action to null, we avoid waking up the same awaiter more than once. Any
                    // task completions that occur while no awaiter is active will end up being enqueued in _ready.
                    //

                    if (_onCompleted != null)
                    {
                        onCompleted = _onCompleted;
                        _onCompleted = null;
                    }
                }

                onCompleted?.Invoke();
            }

            /// <summary>
            /// Invoked by awaiters to check if any task has completed, in order to short-circuit the await operation.
            /// </summary>
            /// <returns><c>true</c> if any task has completed; otherwise, <c>false</c>.</returns>
            private bool IsCompleted()
            {
                // REVIEW: Evaluate options to reduce locking, so the single consuming awaiter has limited contention
                //         with the multiple concurrent completing enumerator tasks, e.g. using ConcurrentQueue<T>.

                lock (_ready)
                {
                    return _ready.Count > 0;
                }
            }

            /// <summary>
            /// Gets the index of the earliest task that has completed, used by the awaiter. After stealing an index from
            /// the ready queue (by means of awaiting the current instance), the user may chose to replace the task at the
            /// returned index by a new task, using the <see cref="Replace"/> method.
            /// </summary>
            /// <returns>Index of the earliest task that has completed.</returns>
            private int GetResult()
            {
                lock (_ready)
                {
                    return _ready.Dequeue();
                }
            }

            /// <summary>
            /// Register a continuation passed by an awaiter.
            /// </summary>
            /// <param name="action">The continuation action delegate to call when any task is ready.</param>
            private void OnCompleted(Action action)
            {
                bool shouldInvoke = false;

                lock (_ready)
                {
                    //
                    // Check if we have anything ready (which could happen in the short window between checking
                    // for IsCompleted and calling OnCompleted). If so, we should invoke the action directly. Not
                    // doing so would be a correctness issue where a task has completed, its index was enqueued,
                    // but the continuation was never called (unless another task completes and calls the action
                    // delegate, whose subsequent call to GetResult would pick up the lost index).
                    //

                    if (_ready.Count > 0)
                    {
                        shouldInvoke = true;
                    }
                    else
                    {
                        Debug.Assert(_onCompleted == null, "Only a single awaiter is allowed.");

                        _onCompleted = action;
                    }
                }

                //
                // NB: We assume this case is rare enough (IsCompleted and OnCompleted happen right after one
                //     another, and an enqueue should have happened right in between to go from an empty to a
                //     non-empty queue), so we don't run the risk of triggering a stack overflow due to
                //     synchronous completion of the await operation (which may be in a loop that awaits the
                //     current instance again).
                //

                if (shouldInvoke)
                {
                    action();
                }
            }

            /// <summary>
            /// Awaiter type used to await completion of any task.
            /// </summary>
            public struct Awaiter : INotifyCompletion
            {
                private readonly WhenAnyValueTask<T> _parent;

                public Awaiter(WhenAnyValueTask<T> parent) => _parent = parent;

                public bool IsCompleted => _parent.IsCompleted();
                public int GetResult() => _parent.GetResult();
                public void OnCompleted(Action action) => _parent.OnCompleted(action);
            }
        }
#endif
    }
}

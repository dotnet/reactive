// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information. 

using System.Collections.Generic;
using System.Threading.Tasks;

namespace System.Linq
{
#if INCLUDE_RELOCATED_TO_INTERACTIVE_ASYNC
    public static partial class AsyncEnumerable
    {
        // Moved to AsyncEnumerableEx in System.Interactive.Async.
        // System.Linq.AsyncEnumerable has chosen not to implement this. We continue to implement this because
        // we believe it is a useful feature, but since it's now in the category of LINQ-adjacent functionality
        // not built into the .NET runtime libraries, it now lives in System.Interactive.Async.

        /// <summary>
        /// Converts a task to an async-enumerable sequence.
        /// </summary>
        /// <typeparam name="TSource">The type of the elements in the source task.</typeparam>
        /// <param name="task">Task to convert to an async-enumerable sequence.</param>
        /// <returns>The async-enumerable sequence whose element is pulled from the given task.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="task"/> is null.</exception>
        public static IAsyncEnumerable<TSource> ToAsyncEnumerable<TSource>(this Task<TSource> task)
        {
            if (task == null)
                throw Error.ArgumentNull(nameof(task));

            return new TaskToAsyncEnumerable<TSource>(task);
        }

        private sealed class TaskToAsyncEnumerable<T> : AsyncIterator<T>
        {
            private readonly Task<T> _task;

            public TaskToAsyncEnumerable(Task<T> task) => _task = task;

            public override AsyncIteratorBase<T> Clone() => new TaskToAsyncEnumerable<T>(_task);

            protected override async ValueTask<bool> MoveNextCore()
            {
                if (_state == AsyncIteratorState.Allocated)
                {
                    _state = AsyncIteratorState.Iterating;
                    _current = await _task.ConfigureAwait(false);
                    return true;
                }

                return false;
            }
        }
    }
#endif // INCLUDE_RELOCATED_TO_INTERACTIVE_ASYNC
}

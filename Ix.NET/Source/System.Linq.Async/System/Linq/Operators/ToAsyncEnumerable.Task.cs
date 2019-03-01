// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the Apache 2.0 License.
// See the LICENSE file in the project root for more information. 

using System.Collections.Generic;
using System.Threading.Tasks;

namespace System.Linq
{
    public static partial class AsyncEnumerable
    {
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
}

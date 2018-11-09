// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the Apache 2.0 License.
// See the LICENSE file in the project root for more information. 

using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace System.Linq
{
    public static partial class AsyncEnumerableEx
    {
        public static IAsyncEnumerable<TValue> Throw<TValue>(Exception exception)
        {
            if (exception == null)
                throw new ArgumentNullException(nameof(exception));

#if NO_TASK_FROMEXCEPTION
            var tcs = new TaskCompletionSource<bool>();
            tcs.TrySetException(exception);
            var moveNextThrows = new ValueTask<bool>(tcs.Task);
#else
            var moveNextThrows = new ValueTask<bool>(Task.FromException<bool>(exception));
#endif

            return AsyncEnumerable.CreateEnumerable(
                () => AsyncEnumerable.CreateEnumerator<TValue>(
                    () => moveNextThrows,
                    current: null,
                    dispose: null)
            );
        }

        private sealed class ThrowEnumerable<TValue> : IAsyncEnumerable<TValue>, IAsyncEnumerator<TValue>
        {
            private readonly ValueTask<bool> _moveNextThrows;

            public TValue Current => default;

            public ThrowEnumerable(ValueTask<bool> moveNextThrows)
            {
                _moveNextThrows = moveNextThrows;
            }

            public IAsyncEnumerator<TValue> GetAsyncEnumerator(CancellationToken cancellationToken = default)
            {
                return this;
            }

            public ValueTask DisposeAsync()
            {
                return TaskExt.CompletedTask;
            }

            public ValueTask<bool> MoveNextAsync()
            {
                // May we let this fail over and over?
                // If so, the class doesn't need extra state
                // and thus can be reused without creating an
                // async enumerator
                return _moveNextThrows;
            }
        }
    }
}

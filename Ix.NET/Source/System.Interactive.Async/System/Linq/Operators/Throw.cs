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
                throw Error.ArgumentNull(nameof(exception));

#if NO_TASK_FROMEXCEPTION
            var tcs = new TaskCompletionSource<bool>();
            tcs.TrySetException(exception);
            var moveNextThrows = new ValueTask<bool>(tcs.Task);
#else
            var moveNextThrows = new ValueTask<bool>(Task.FromException<bool>(exception));
#endif

            return new ThrowEnumerable<TValue>(moveNextThrows);
        }

        private sealed class ThrowEnumerable<TValue> : IAsyncEnumerable<TValue>
        {
            private readonly ValueTask<bool> _moveNextThrows;

            public ThrowEnumerable(ValueTask<bool> moveNextThrows)
            {
                _moveNextThrows = moveNextThrows;
            }

            public IAsyncEnumerator<TValue> GetAsyncEnumerator(CancellationToken cancellationToken)
            {
                return new ThrowEnumerator(_moveNextThrows);
            }

            private sealed class ThrowEnumerator : IAsyncEnumerator<TValue>
            {
                private ValueTask<bool> _moveNextThrows;

                public ThrowEnumerator(ValueTask<bool> moveNextThrows)
                {
                    _moveNextThrows = moveNextThrows;
                }

                public TValue Current => default;

                public ValueTask DisposeAsync()
                {
                    _moveNextThrows = new ValueTask<bool>(false);
                    return default;
                }

                public ValueTask<bool> MoveNextAsync()
                {
                    var result = _moveNextThrows;
                    _moveNextThrows = new ValueTask<bool>(false);
                    return result;
                }

            }
        }
    }
}

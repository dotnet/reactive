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
        /// <summary>
        /// Returns an async-enumerable sequence that terminates with an exception.
        /// </summary>
        /// <typeparam name="TValue">The type used for the <see cref="IAsyncEnumerable{T}"/> type parameter of the resulting sequence.</typeparam>
        /// <param name="exception">Exception object used for the sequence's termination.</param>
        /// <returns>The async-enumerable sequence that terminates exceptionally with the specified exception object.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="exception"/> is null.</exception>
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
                cancellationToken.ThrowIfCancellationRequested(); // NB: [LDM-2018-11-28] Equivalent to async iterator behavior.

                return new ThrowEnumerator(_moveNextThrows);
            }

            private sealed class ThrowEnumerator : IAsyncEnumerator<TValue>
            {
                private ValueTask<bool> _moveNextThrows;

                public ThrowEnumerator(ValueTask<bool> moveNextThrows)
                {
                    _moveNextThrows = moveNextThrows;
                }

                public TValue Current => default!;

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

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
        /// Returns a non-terminating async-enumerable sequence, which can be used to denote an infinite duration (e.g. when using reactive joins).
        /// </summary>
        /// <typeparam name="TValue">The type used for the <see cref="IAsyncEnumerable{T}"/> type parameter of the resulting sequence.</typeparam>
        /// <returns>An async-enumerable sequence whose consumers will never resume after awaiting <see cref="IAsyncEnumerator{T}.MoveNextAsync"/>.</returns>
        public static IAsyncEnumerable<TValue> Never<TValue>() => NeverAsyncEnumerable<TValue>.Instance;

        private sealed class NeverAsyncEnumerable<TValue> : IAsyncEnumerable<TValue>
        {
            internal static readonly NeverAsyncEnumerable<TValue> Instance = new NeverAsyncEnumerable<TValue>();

            public IAsyncEnumerator<TValue> GetAsyncEnumerator(CancellationToken cancellationToken)
            {
                cancellationToken.ThrowIfCancellationRequested(); // NB: [LDM-2018-11-28] Equivalent to async iterator behavior.

                return new NeverAsyncEnumerator(cancellationToken);
            }

            private sealed class NeverAsyncEnumerator : IAsyncEnumerator<TValue>
            {
                private readonly CancellationToken _token;

                private CancellationTokenRegistration _registration;
                private bool _once;

                public NeverAsyncEnumerator(CancellationToken token) => _token = token;

                public TValue Current => throw new InvalidOperationException();

                public ValueTask DisposeAsync()
                {
                    _registration.Dispose();
                    return default;
                }

                public ValueTask<bool> MoveNextAsync()
                {
                    if (_once)
                    {
                        return new ValueTask<bool>(false);
                    }

                    _once = true;
                    var task = new TaskCompletionSource<bool>();
                    _registration = _token.Register(state => ((TaskCompletionSource<bool>)state).TrySetCanceled(_token), task);
                    return new ValueTask<bool>(task.Task);
                }
            }
        }
    }
}

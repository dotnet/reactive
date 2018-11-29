// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the Apache 2.0 License.
// See the LICENSE file in the project root for more information. 

using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace System.Linq
{
    public static partial class AsyncEnumerable
    {
        public static IAsyncEnumerable<TValue> Empty<TValue>() => EmptyAsyncIterator<TValue>.Instance;

        internal sealed class EmptyAsyncIterator<TValue> : IAsyncPartition<TValue>, IAsyncEnumerator<TValue>
        {
            public static readonly EmptyAsyncIterator<TValue> Instance = new EmptyAsyncIterator<TValue>();

            public TValue Current => default;

            public Task<int> GetCountAsync(bool onlyIfCheap, CancellationToken cancellationToken) => Task.FromResult(0);

            public IAsyncPartition<TValue> Skip(int count) => this;

            public IAsyncPartition<TValue> Take(int count) => this;

            public Task<TValue[]> ToArrayAsync(CancellationToken cancellationToken) => Task.FromResult(
#if NO_ARRAY_EMPTY
                EmptyArray<TValue>.Value
#else
                Array.Empty<TValue>()
#endif
                );

            public Task<List<TValue>> ToListAsync(CancellationToken cancellationToken) => Task.FromResult(new List<TValue>());

            public ValueTask<Maybe<TValue>> TryGetElementAsync(int index, CancellationToken cancellationToken) => new ValueTask<Maybe<TValue>>(new Maybe<TValue>());

            public ValueTask<Maybe<TValue>> TryGetFirstAsync(CancellationToken cancellationToken) => new ValueTask<Maybe<TValue>>(new Maybe<TValue>());

            public ValueTask<Maybe<TValue>> TryGetLastAsync(CancellationToken cancellationToken) => new ValueTask<Maybe<TValue>>(new Maybe<TValue>());

            public ValueTask<bool> MoveNextAsync() => new ValueTask<bool>(false);

            public IAsyncEnumerator<TValue> GetAsyncEnumerator(CancellationToken cancellationToken)
            {
                cancellationToken.ThrowIfCancellationRequested(); // NB: [LDM-2018-11-28] Equivalent to async iterator behavior.

                return this;
            }

            public ValueTask DisposeAsync() => default;
        }
    }
}

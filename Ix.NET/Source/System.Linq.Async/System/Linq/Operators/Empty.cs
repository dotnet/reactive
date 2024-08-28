// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information. 

using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace System.Linq
{
    public static partial class AsyncEnumerable
    {
        /// <summary>
        /// Returns an empty async-enumerable sequence.
        /// </summary>
        /// <typeparam name="TValue">The type used for the <see cref="IAsyncEnumerable{T}"/> type parameter of the resulting sequence.</typeparam>
        /// <returns>An async-enumerable sequence with no elements.</returns>
        public static IAsyncEnumerable<TValue> Empty<TValue>() => EmptyAsyncIterator<TValue>.Instance;

        internal sealed class EmptyAsyncIterator<TValue> : IAsyncPartition<TValue>, IAsyncEnumerator<TValue>
        {
            public static readonly EmptyAsyncIterator<TValue> Instance = new();

            public TValue Current => default!;

            public ValueTask<int> GetCountAsync(bool onlyIfCheap, CancellationToken cancellationToken) => new(0);

            public IAsyncPartition<TValue> Skip(int count) => this;

            public IAsyncPartition<TValue> Take(int count) => this;

            public ValueTask<TValue[]> ToArrayAsync(CancellationToken cancellationToken) => new([]);

            public ValueTask<List<TValue>> ToListAsync(CancellationToken cancellationToken) => new([]);

            public ValueTask<Maybe<TValue>> TryGetElementAtAsync(int index, CancellationToken cancellationToken) => new(new Maybe<TValue>());

            public ValueTask<Maybe<TValue>> TryGetFirstAsync(CancellationToken cancellationToken) => new(new Maybe<TValue>());

            public ValueTask<Maybe<TValue>> TryGetLastAsync(CancellationToken cancellationToken) => new(new Maybe<TValue>());

            public ValueTask<bool> MoveNextAsync() => new(false);

            public IAsyncEnumerator<TValue> GetAsyncEnumerator(CancellationToken cancellationToken)
            {
                cancellationToken.ThrowIfCancellationRequested(); // NB: [LDM-2018-11-28] Equivalent to async iterator behavior.

                return this;
            }

            public ValueTask DisposeAsync() => default;
        }
    }
}

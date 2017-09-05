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

            public TValue Current => default(TValue);

            public Task<int> GetCountAsync(bool onlyIfCheap, CancellationToken cancellationToken) => Task.FromResult(0);

            public IAsyncPartition<TValue> Skip(int count) => this;

            public IAsyncPartition<TValue> Take(int count) => this;

            public Task<TValue[]> ToArrayAsync(CancellationToken cancellationToken) => Task.FromResult(Array.Empty<TValue>());

            public Task<List<TValue>> ToListAsync(CancellationToken cancellationToken) => Task.FromResult(new List<TValue>());

            public Task<Maybe<TValue>> TryGetElementAsync(int index, CancellationToken cancellationToken) => Task.FromResult(new Maybe<TValue>());

            public Task<Maybe<TValue>> TryGetFirstAsync(CancellationToken cancellationToken) => Task.FromResult(new Maybe<TValue>());

            public Task<Maybe<TValue>> TryGetLastAsync(CancellationToken cancellationToken) => Task.FromResult(new Maybe<TValue>());

            public Task<bool> MoveNextAsync() => TaskExt.False;

            public IAsyncEnumerator<TValue> GetAsyncEnumerator() => this;

            public Task DisposeAsync() => TaskExt.CompletedTask;
        }
    }
}

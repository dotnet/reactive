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
        public static IAsyncEnumerable<TValue> Return<TValue>(TValue value)
        {
            return new ReturnEnumerable<TValue>(value);
        }

        // REVIEW: Add support for IAsyncPartition<T>.

        private sealed class ReturnEnumerable<TValue> : IAsyncEnumerable<TValue>, IAsyncIListProvider<TValue>
        {
            private readonly TValue _value;

            public ReturnEnumerable(TValue value)
            {
                _value = value;
            }

            public IAsyncEnumerator<TValue> GetAsyncEnumerator(CancellationToken cancellationToken)
            {
                cancellationToken.ThrowIfCancellationRequested(); // NB: [LDM-2018-11-28] Equivalent to async iterator behavior.

                return new ReturnEnumerator(_value);
            }

            public ValueTask<int> GetCountAsync(bool onlyIfCheap, CancellationToken cancellationToken) => new ValueTask<int>(1);

            public ValueTask<TValue[]> ToArrayAsync(CancellationToken cancellationToken) => new ValueTask<TValue[]>(new[] { _value });

            public ValueTask<List<TValue>> ToListAsync(CancellationToken cancellationToken) => new ValueTask<List<TValue>>(new List<TValue>(1) { _value });

            private sealed class ReturnEnumerator : IAsyncEnumerator<TValue>
            {
                private bool _once;

                public ReturnEnumerator(TValue current)
                {
                    Current = current;
                }

                public TValue Current { get; private set; }

                public ValueTask DisposeAsync()
                {
                    Current = default;
                    return default;
                }

                public ValueTask<bool> MoveNextAsync()
                {
                    if (_once)
                    {
                        return new ValueTask<bool>(false);
                    }

                    _once = true;
                    return new ValueTask<bool>(true);
                }
            }
        }
    }
}

// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the Apache 2.0 License.
// See the LICENSE file in the project root for more information. 

using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading;
using System.Threading.Tasks;

namespace System.Linq
{
    public static partial class AsyncEnumerable
    {
        public static IAsyncEnumerable<TSource> Reverse<TSource>(this IAsyncEnumerable<TSource> source)
        {
            if (source == null)
            {
                throw Error.ArgumentNull(nameof(source));
            }

            return new ReverseAsyncIterator<TSource>(source);
        }

        private sealed class ReverseAsyncIterator<TSource> : AsyncIterator<TSource>, IAsyncIListProvider<TSource>
        {
            private readonly IAsyncEnumerable<TSource> _source;

            private int _index;
            private TSource[] _items;

            public ReverseAsyncIterator(IAsyncEnumerable<TSource> source)
            {
                Debug.Assert(source != null);

                _source = source;
            }

            public async Task<TSource[]> ToArrayAsync(CancellationToken cancellationToken)
            {
                var array = await _source.ToArray(cancellationToken).ConfigureAwait(false);

                // Array.Reverse() involves boxing for non-primitive value types, but
                // checking that has its own cost, so just use this approach for all types.
                for (int i = 0, j = array.Length - 1; i < j; ++i, --j)
                {
                    var temp = array[i];
                    array[i] = array[j];
                    array[j] = temp;
                }

                return array;
            }

            public async Task<List<TSource>> ToListAsync(CancellationToken cancellationToken)
            {
                var list = await _source.ToList(cancellationToken).ConfigureAwait(false);

                list.Reverse();
                return list;
            }

            public Task<int> GetCountAsync(bool onlyIfCheap, CancellationToken cancellationToken)
            {
                if (onlyIfCheap)
                {
                    if (_source is IAsyncIListProvider<TSource> listProv)
                    {
                        return listProv.GetCountAsync(true, cancellationToken);
                    }

                    if (!(_source is ICollection<TSource>) && !(_source is ICollection))
                    {
                        return Task.FromResult(-1);
                    }
                }

                return _source.Count(cancellationToken);
            }

            public override AsyncIterator<TSource> Clone()
            {
                return new ReverseAsyncIterator<TSource>(_source);
            }

            public override async ValueTask DisposeAsync()
            {
                _items = null; // Just in case this ends up being long-lived, allow the memory to be reclaimed.
                await base.DisposeAsync().ConfigureAwait(false);
            }

            protected override async ValueTask<bool> MoveNextCore(CancellationToken cancellationToken)
            {
                switch (state)
                {
                    case AsyncIteratorState.Allocated:
                        _items = await _source.ToArray().ConfigureAwait(false);
                        _index = _items.Length - 1;

                        state = AsyncIteratorState.Iterating;
                        goto case AsyncIteratorState.Iterating;

                    case AsyncIteratorState.Iterating:
                        if (_index != -1)
                        {
                            current = _items[_index];
                            --_index;
                            return true;
                        }

                        break;
                }

                await DisposeAsync().ConfigureAwait(false);
                return false;
            }
        }
    }
}

// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the Apache 2.0 License.
// See the LICENSE file in the project root for more information. 

using System.Collections;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace System.Linq
{
    public static partial class AsyncEnumerable
    {
        /// <summary>
        /// Inverts the order of the elements in a sequence.
        /// </summary>
        /// <typeparam name="TSource">The type of the elements of source.</typeparam>
        /// <param name="source">An async-enumerable sequence of values to reverse.</param>
        /// <returns>An async-enumerable sequence whose elements correspond to those of the input sequence in reverse order.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="source"/> is null.</exception>
        public static IAsyncEnumerable<TSource> Reverse<TSource>(this IAsyncEnumerable<TSource> source)
        {
            if (source == null)
                throw Error.ArgumentNull(nameof(source));

            return new ReverseAsyncIterator<TSource>(source);
        }

        private sealed class ReverseAsyncIterator<TSource> : AsyncIterator<TSource>, IAsyncIListProvider<TSource>
        {
            private readonly IAsyncEnumerable<TSource> _source;

            private int _index;
            private TSource[]? _items;

            public ReverseAsyncIterator(IAsyncEnumerable<TSource> source)
            {
                _source = source;
            }

            public async ValueTask<TSource[]> ToArrayAsync(CancellationToken cancellationToken)
            {
                var array = await _source.ToArrayAsync(cancellationToken).ConfigureAwait(false);

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

            public async ValueTask<List<TSource>> ToListAsync(CancellationToken cancellationToken)
            {
                var list = await _source.ToListAsync(cancellationToken).ConfigureAwait(false);

                list.Reverse();
                return list;
            }

            public ValueTask<int> GetCountAsync(bool onlyIfCheap, CancellationToken cancellationToken)
            {
                if (onlyIfCheap)
                {
                    if (_source is IAsyncIListProvider<TSource> listProv)
                    {
                        return listProv.GetCountAsync(true, cancellationToken);
                    }

                    if (!(_source is ICollection<TSource>) && !(_source is ICollection))
                    {
                        return new ValueTask<int>(-1);
                    }
                }

                return _source.CountAsync(cancellationToken);
            }

            public override AsyncIteratorBase<TSource> Clone()
            {
                return new ReverseAsyncIterator<TSource>(_source);
            }

            public override async ValueTask DisposeAsync()
            {
                _items = null; // Just in case this ends up being long-lived, allow the memory to be reclaimed.
                await base.DisposeAsync().ConfigureAwait(false);
            }

            protected override async ValueTask<bool> MoveNextCore()
            {
                switch (_state)
                {
                    case AsyncIteratorState.Allocated:
                        _items = await _source.ToArrayAsync(_cancellationToken).ConfigureAwait(false);
                        _index = _items.Length - 1;

                        _state = AsyncIteratorState.Iterating;
                        goto case AsyncIteratorState.Iterating;

                    case AsyncIteratorState.Iterating:
                        if (_index != -1)
                        {
                            _current = _items![_index];
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

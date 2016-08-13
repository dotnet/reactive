// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the Apache 2.0 License.
// See the LICENSE file in the project root for more information. 

using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace System.Linq
{
    public static partial class AsyncEnumerable
    {
        public static IAsyncEnumerable<TSource> Reverse<TSource>(this IAsyncEnumerable<TSource> source)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));

            return new ReverseAsyncIterator<TSource>(source);
        }

        private sealed class ReverseAsyncIterator<TSource> : AsyncIterator<TSource>, IIListProvider<TSource>
        {
            private readonly IAsyncEnumerable<TSource> source;
            private int index;
            private TSource[] items;

            public ReverseAsyncIterator(IAsyncEnumerable<TSource> source)
            {
                Debug.Assert(source != null);
                this.source = source;
            }

            public async Task<TSource[]> ToArrayAsync(CancellationToken cancellationToken)
            {
                var array = await source.ToArray(cancellationToken)
                                        .ConfigureAwait(false);

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
                var list = await source.ToList(cancellationToken)
                                       .ConfigureAwait(false);

                list.Reverse();
                return list;
            }

            public Task<int> GetCountAsync(bool onlyIfCheap, CancellationToken cancellationToken)
            {
                if (onlyIfCheap)
                {
                    var listProv = source as IIListProvider<TSource>;
                    if (listProv != null)
                    {
                        return listProv.GetCountAsync(onlyIfCheap: true, cancellationToken: cancellationToken);
                    }

                    if (!(source is ICollection<TSource>) && !(source is ICollection))
                    {
                        return Task.FromResult(-1);
                    }
                }

                return source.Count(cancellationToken);
            }

            public override AsyncIterator<TSource> Clone()
            {
                return new ReverseAsyncIterator<TSource>(source);
            }

            public override void Dispose()
            {
                items = null; // Just in case this ends up being long-lived, allow the memory to be reclaimed.
                base.Dispose();
            }


            protected override async Task<bool> MoveNextCore(CancellationToken cancellationToken)
            {
                switch (state)
                {
                    case AsyncIteratorState.Allocated:
                        items = await source.ToArray(cancellationToken)
                                            .ConfigureAwait(false);
                        index = items.Length - 1;

                        state = AsyncIteratorState.Iterating;
                        goto case AsyncIteratorState.Iterating;

                    case AsyncIteratorState.Iterating:
                        if (index != -1)
                        {
                            current = items[index];
                            --index;
                            return true;
                        }

                        break;
                }

                Dispose();
                return false;
            }
        }
    }
}
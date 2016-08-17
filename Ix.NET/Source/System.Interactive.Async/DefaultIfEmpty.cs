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
        public static IAsyncEnumerable<TSource> DefaultIfEmpty<TSource>(this IAsyncEnumerable<TSource> source, TSource defaultValue)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));

            return new DefaultIfEmptyAsyncIterator<TSource>(source, defaultValue);
        }

        public static IAsyncEnumerable<TSource> DefaultIfEmpty<TSource>(this IAsyncEnumerable<TSource> source)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));

            return DefaultIfEmpty(source, default(TSource));
        }

        private sealed class DefaultIfEmptyAsyncIterator<TSource> : AsyncIterator<TSource>, IIListProvider<TSource>
        {
            private readonly IAsyncEnumerable<TSource> source;
            private readonly TSource defaultValue;
            private bool hasValue = false;
            private IAsyncEnumerator<TSource> enumerator;

            public DefaultIfEmptyAsyncIterator(IAsyncEnumerable<TSource> source, TSource defaultValue)
            {
                this.source = source;
                this.defaultValue = defaultValue;
                Debug.Assert(source != null);
            }

            public override AsyncIterator<TSource> Clone()
            {
                return new DefaultIfEmptyAsyncIterator<TSource>(source, defaultValue);
            }

            public override void Dispose()
            {
                if (enumerator != null)
                {
                    enumerator.Dispose();
                    enumerator = null;
                }

                base.Dispose();
            }

            protected override async Task<bool> MoveNextCore(CancellationToken cancellationToken)
            {
                if (await enumerator.MoveNext(cancellationToken)
                                    .ConfigureAwait(false))
                {
                    hasValue = true;
                    current = enumerator.Current;

                    return true;
                }

                if (!hasValue)
                {
                    current = defaultValue;
                    enumerator.Dispose();
                    enumerator = null;

                    state = AsyncIteratorState.Disposed;

                    return true;
                }

                return false;
            }

            protected override Task Initialize(CancellationToken cancellationToken)
            {
                enumerator = source.GetEnumerator();
                return TaskExt.True;
            }

            public async Task<TSource[]> ToArrayAsync(CancellationToken cancellationToken)
            {
                var array = await source.ToArray(cancellationToken).ConfigureAwait(false);
                return array.Length == 0 ? new[] { defaultValue } : array;
            }

            public async Task<List<TSource>> ToListAsync(CancellationToken cancellationToken)
            {
                var list = await source.ToList(cancellationToken).ConfigureAwait(false);
                if (list.Count == 0)
                {
                    list.Add(defaultValue);
                }

                return list;
            }

            public async Task<int> GetCountAsync(bool onlyIfCheap, CancellationToken cancellationToken)
            {
                int count;
                if (!onlyIfCheap || source is ICollection<TSource> || source is ICollection)
                {
                    count = await source.Count(cancellationToken).ConfigureAwait(false);
                }
                else
                {
                    var listProv = source as IIListProvider<TSource>;
                    count = listProv == null ? -1 : await listProv.GetCountAsync(onlyIfCheap: true, cancellationToken: cancellationToken).ConfigureAwait(false);
                }

                return count == 0 ? 1 : count;
            }
        }
    }
}
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
        public static Task<HashSet<TSource>> ToHashSetAsync<TSource>(this IAsyncEnumerable<TSource> source, CancellationToken cancellationToken = default) =>
            ToHashSetAsync(source, comparer: null, cancellationToken);

        public static Task<HashSet<TSource>> ToHashSetAsync<TSource>(this IAsyncEnumerable<TSource> source, IEqualityComparer<TSource> comparer, CancellationToken cancellationToken = default)
        {
            if (source == null)
                throw Error.ArgumentNull(nameof(source));

            return Core(source, comparer, cancellationToken);

            static async Task<HashSet<TSource>> Core(IAsyncEnumerable<TSource> _source, IEqualityComparer<TSource> _comparer, CancellationToken _cancellationToken)
            {
                var set = new HashSet<TSource>(_comparer);

#if USE_AWAIT_FOREACH
                await foreach (TSource item in _source.WithCancellation(_cancellationToken).ConfigureAwait(false))
                {
                    set.Add(item);
                }
#else
                var e = _source.GetAsyncEnumerator(_cancellationToken);

                try
                {
                    while (await e.MoveNextAsync().ConfigureAwait(false))
                    {
                        set.Add(e.Current);
                    }
                }
                finally
                {
                    await e.DisposeAsync().ConfigureAwait(false);
                }
#endif

                return set;
            }
        }
    }
}

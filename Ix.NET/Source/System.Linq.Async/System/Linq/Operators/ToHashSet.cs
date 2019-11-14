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
        public static ValueTask<HashSet<TSource>> ToHashSetAsync<TSource>(this IAsyncEnumerable<TSource> source, CancellationToken cancellationToken = default) =>
            ToHashSetAsync(source, comparer: null, cancellationToken);

        public static ValueTask<HashSet<TSource>> ToHashSetAsync<TSource>(this IAsyncEnumerable<TSource> source, IEqualityComparer<TSource>? comparer, CancellationToken cancellationToken = default)
        {
            if (source == null)
                throw Error.ArgumentNull(nameof(source));

            return Core(source, comparer, cancellationToken);

            static async ValueTask<HashSet<TSource>> Core(IAsyncEnumerable<TSource> source, IEqualityComparer<TSource>? comparer, CancellationToken cancellationToken)
            {
                var set = new HashSet<TSource>(comparer);

                await foreach (var item in source.WithCancellation(cancellationToken).ConfigureAwait(false))
                {
                    set.Add(item);
                }

                return set;
            }
        }
    }
}

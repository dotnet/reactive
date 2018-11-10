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
        public static Task<HashSet<TSource>> ToHashSet<TSource>(this IAsyncEnumerable<TSource> source)
        {
            if (source == null)
                throw Error.ArgumentNull(nameof(source));

            return ToHashSetCore(source, EqualityComparer<TSource>.Default, CancellationToken.None);
        }

        public static Task<HashSet<TSource>> ToHashSet<TSource>(this IAsyncEnumerable<TSource> source, CancellationToken cancellationToken)
        {
            if (source == null)
                throw Error.ArgumentNull(nameof(source));

            return ToHashSetCore(source, EqualityComparer<TSource>.Default, cancellationToken);
        }

        public static Task<HashSet<TSource>> ToHashSet<TSource>(this IAsyncEnumerable<TSource> source, IEqualityComparer<TSource> comparer)
        {
            if (source == null)
                throw Error.ArgumentNull(nameof(source));
            if (comparer == null)
                throw Error.ArgumentNull(nameof(comparer));

            return ToHashSetCore(source, comparer, CancellationToken.None);
        }

        public static Task<HashSet<TSource>> ToHashSet<TSource>(this IAsyncEnumerable<TSource> source, IEqualityComparer<TSource> comparer, CancellationToken cancellationToken)
        {
            if (source == null)
                throw Error.ArgumentNull(nameof(source));
            if (comparer == null)
                throw Error.ArgumentNull(nameof(comparer));

            return ToHashSetCore(source, comparer, cancellationToken);
        }

        private static async Task<HashSet<TSource>> ToHashSetCore<TSource>(IAsyncEnumerable<TSource> source, IEqualityComparer<TSource> comparer, CancellationToken cancellationToken)
        {
            var e = source.GetAsyncEnumerator(cancellationToken);

            try
            {
                var set = new HashSet<TSource>(comparer);

                while (await e.MoveNextAsync().ConfigureAwait(false))
                {
                    set.Add(e.Current);
                }

                return set;
            }
            finally
            {
                await e.DisposeAsync().ConfigureAwait(false);
            }
        }
    }
}

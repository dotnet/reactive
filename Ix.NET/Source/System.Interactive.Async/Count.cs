// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the Apache 2.0 License.
// See the LICENSE file in the project root for more information. 

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace System.Linq
{
    public static partial class AsyncEnumerable
    {
        public static Task<int> Count<TSource>(this IAsyncEnumerable<TSource> source, CancellationToken cancellationToken)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));

            var collectionoft = source as ICollection<TSource>;
            if (collectionoft != null)
            {
                return Task.FromResult(collectionoft.Count);
            }

            var listProv = source as IIListProvider<TSource>;
            if (listProv != null)
            {
                return listProv.GetCountAsync(onlyIfCheap: false, cancellationToken: cancellationToken);
            }

            return source.Aggregate(0, (c, _) => checked(c + 1), cancellationToken);
        }

        public static Task<int> Count<TSource>(this IAsyncEnumerable<TSource> source, Func<TSource, bool> predicate, CancellationToken cancellationToken)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));
            if (predicate == null)
                throw new ArgumentNullException(nameof(predicate));

            return source.Where(predicate)
                         .Aggregate(0, (c, _) => checked(c + 1), cancellationToken);
        }

        public static Task<int> Count<TSource>(this IAsyncEnumerable<TSource> source)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));

            return Count(source, CancellationToken.None);
        }

        public static Task<int> Count<TSource>(this IAsyncEnumerable<TSource> source, Func<TSource, bool> predicate)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));
            if (predicate == null)
                throw new ArgumentNullException(nameof(predicate));

            return Count(source, predicate, CancellationToken.None);
        }

        public static Task<long> LongCount<TSource>(this IAsyncEnumerable<TSource> source, CancellationToken cancellationToken)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));

            return source.Aggregate(0L, (c, _) => checked(c + 1), cancellationToken);
        }

        public static Task<long> LongCount<TSource>(this IAsyncEnumerable<TSource> source, Func<TSource, bool> predicate, CancellationToken cancellationToken)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));
            if (predicate == null)
                throw new ArgumentNullException(nameof(predicate));

            return source.Where(predicate)
                         .Aggregate(0L, (c, _) => checked(c + 1), cancellationToken);
        }

        public static Task<long> LongCount<TSource>(this IAsyncEnumerable<TSource> source)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));

            return LongCount(source, CancellationToken.None);
        }

        public static Task<long> LongCount<TSource>(this IAsyncEnumerable<TSource> source, Func<TSource, bool> predicate)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));
            if (predicate == null)
                throw new ArgumentNullException(nameof(predicate));

            return LongCount(source, predicate, CancellationToken.None);
        }
    }
}
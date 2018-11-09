﻿// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the Apache 2.0 License.
// See the LICENSE file in the project root for more information. 

using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace System.Linq
{
    public static partial class AsyncEnumerable
    {
        public static Task<int> Count<TSource>(this IAsyncEnumerable<TSource> source)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));

            return CountCore(source, CancellationToken.None);
        }

        public static Task<int> Count<TSource>(this IAsyncEnumerable<TSource> source, CancellationToken cancellationToken)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));

            return CountCore(source, cancellationToken);
        }

        public static Task<int> Count<TSource>(this IAsyncEnumerable<TSource> source, Func<TSource, bool> predicate)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));
            if (predicate == null)
                throw new ArgumentNullException(nameof(predicate));

            return CountCore(source, predicate, CancellationToken.None);
        }

        public static Task<int> Count<TSource>(this IAsyncEnumerable<TSource> source, Func<TSource, bool> predicate, CancellationToken cancellationToken)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));
            if (predicate == null)
                throw new ArgumentNullException(nameof(predicate));

            return CountCore(source, predicate, cancellationToken);
        }

        public static Task<int> Count<TSource>(this IAsyncEnumerable<TSource> source, Func<TSource, Task<bool>> predicate)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));
            if (predicate == null)
                throw new ArgumentNullException(nameof(predicate));

            return CountCore(source, predicate, CancellationToken.None);
        }

        public static Task<int> Count<TSource>(this IAsyncEnumerable<TSource> source, Func<TSource, Task<bool>> predicate, CancellationToken cancellationToken)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));
            if (predicate == null)
                throw new ArgumentNullException(nameof(predicate));

            return CountCore(source, predicate, cancellationToken);
        }

        private static Task<int> CountCore<TSource>(IAsyncEnumerable<TSource> source, CancellationToken cancellationToken)
        {
            if (source is ICollection<TSource> collection)
            {
                return Task.FromResult(collection.Count);
            }

            if (source is IAsyncIListProvider<TSource> listProv)
            {
                return listProv.GetCountAsync(onlyIfCheap: false, cancellationToken);
            }

            return source.Aggregate(0, (c, _) => checked(c + 1), cancellationToken);
        }

        private static Task<int> CountCore<TSource>(IAsyncEnumerable<TSource> source, Func<TSource, bool> predicate, CancellationToken cancellationToken)
        {
            return source.Where(predicate).Aggregate(0, (c, _) => checked(c + 1), cancellationToken);
        }

        private static Task<int> CountCore<TSource>(this IAsyncEnumerable<TSource> source, Func<TSource, Task<bool>> predicate, CancellationToken cancellationToken)
        {
            return source.Where(predicate).Aggregate(0, (c, _) => checked(c + 1), cancellationToken);
        }
    }
}
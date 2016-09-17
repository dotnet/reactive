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
        public static Task<TSource> Last<TSource>(this IAsyncEnumerable<TSource> source)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));

            return Last(source, CancellationToken.None);
        }

        public static Task<TSource> Last<TSource>(this IAsyncEnumerable<TSource> source, Func<TSource, bool> predicate)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));
            if (predicate == null)
                throw new ArgumentNullException(nameof(predicate));

            return Last(source, predicate, CancellationToken.None);
        }

        public static Task<TSource> Last<TSource>(this IAsyncEnumerable<TSource> source, CancellationToken cancellationToken)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));

            return Last_(source, cancellationToken);
        }

        public static Task<TSource> Last<TSource>(this IAsyncEnumerable<TSource> source, Func<TSource, bool> predicate, CancellationToken cancellationToken)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));
            if (predicate == null)
                throw new ArgumentNullException(nameof(predicate));

            return source.Where(predicate)
                         .Last(cancellationToken);
        }

        public static Task<TSource> LastOrDefault<TSource>(this IAsyncEnumerable<TSource> source)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));

            return LastOrDefault(source, CancellationToken.None);
        }

        public static Task<TSource> LastOrDefault<TSource>(this IAsyncEnumerable<TSource> source, Func<TSource, bool> predicate)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));
            if (predicate == null)
                throw new ArgumentNullException(nameof(predicate));

            return LastOrDefault(source, predicate, CancellationToken.None);
        }

        public static Task<TSource> LastOrDefault<TSource>(this IAsyncEnumerable<TSource> source, CancellationToken cancellationToken)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));

            return LastOrDefault_(source, cancellationToken);
        }

        public static Task<TSource> LastOrDefault<TSource>(this IAsyncEnumerable<TSource> source, Func<TSource, bool> predicate, CancellationToken cancellationToken)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));
            if (predicate == null)
                throw new ArgumentNullException(nameof(predicate));

            return source.Where(predicate)
                         .LastOrDefault(cancellationToken);
        }

        private static async Task<TSource> Last_<TSource>(IAsyncEnumerable<TSource> source, CancellationToken cancellationToken)
        {
            var last = default(TSource);
            var hasLast = false;

            var list = source as IList<TSource>;
            if (list != null)
            {
                var count = list.Count;
                if (count > 0)
                {
                    return list[count - 1];
                }
            }

            using (var e = source.GetEnumerator())
            {
                while (await e.MoveNext(cancellationToken)
                              .ConfigureAwait(false))
                {
                    hasLast = true;
                    last = e.Current;
                }
            }
            if (!hasLast)
                throw new InvalidOperationException(Strings.NO_ELEMENTS);
            return last;
        }

        private static async Task<TSource> LastOrDefault_<TSource>(IAsyncEnumerable<TSource> source, CancellationToken cancellationToken)
        {
            var last = default(TSource);
            var hasLast = false;

            var list = source as IList<TSource>;
            if (list != null)
            {
                var count = list.Count;
                if (count > 0)
                {
                    return list[count - 1];
                }
            }

            using (var e = source.GetEnumerator())
            {
                while (await e.MoveNext(cancellationToken)
                              .ConfigureAwait(false))
                {
                    hasLast = true;
                    last = e.Current;
                }
            }
            return !hasLast ? default(TSource) : last;
        }
    }
}
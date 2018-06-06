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
        public static Task<TSource> First<TSource>(this IAsyncEnumerable<TSource> source)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));

            return First(source, CancellationToken.None);
        }

        public static Task<TSource> First<TSource>(this IAsyncEnumerable<TSource> source, Func<TSource, bool> predicate)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));
            if (predicate == null)
                throw new ArgumentNullException(nameof(predicate));

            return First(source, predicate, CancellationToken.None);
        }

        public static Task<TSource> First<TSource>(this IAsyncEnumerable<TSource> source, CancellationToken cancellationToken)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));

            return First_(source, cancellationToken);
        }

        public static Task<TSource> First<TSource>(this IAsyncEnumerable<TSource> source, Func<TSource, bool> predicate, CancellationToken cancellationToken)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));
            if (predicate == null)
                throw new ArgumentNullException(nameof(predicate));

            return source.Where(predicate)
                         .First(cancellationToken);
        }

        public static Task<TSource> FirstOrDefault<TSource>(this IAsyncEnumerable<TSource> source)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));

            return FirstOrDefault(source, CancellationToken.None);
        }

        public static Task<TSource> FirstOrDefault<TSource>(this IAsyncEnumerable<TSource> source, Func<TSource, bool> predicate)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));
            if (predicate == null)
                throw new ArgumentNullException(nameof(predicate));

            return FirstOrDefault(source, predicate, CancellationToken.None);
        }

        public static Task<TSource> FirstOrDefault<TSource>(this IAsyncEnumerable<TSource> source, CancellationToken cancellationToken)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));

            return FirstOrDefault_(source, cancellationToken);
        }

        public static Task<TSource> FirstOrDefault<TSource>(this IAsyncEnumerable<TSource> source, Func<TSource, bool> predicate, CancellationToken cancellationToken)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));
            if (predicate == null)
                throw new ArgumentNullException(nameof(predicate));

            return source.Where(predicate)
                         .FirstOrDefault(cancellationToken);
        }

        private static async Task<TSource> First_<TSource>(IAsyncEnumerable<TSource> source, CancellationToken cancellationToken)
        {
            var list = source as IList<TSource>;
            if (list?.Count > 0)
            {
                return list[0];
            }

            using (var e = source.GetEnumerator())
            {
                if (await e.MoveNext(cancellationToken)
                           .ConfigureAwait(false))
                {
                    return e.Current;
                }
            }
            throw new InvalidOperationException(Strings.NO_ELEMENTS);
        }

        private static async Task<TSource> FirstOrDefault_<TSource>(IAsyncEnumerable<TSource> source, CancellationToken cancellationToken)
        {
            var list = source as IList<TSource>;
            if (list?.Count > 0)
            {
                return list[0];
            }

            using (var e = source.GetEnumerator())
            {
                if (await e.MoveNext(cancellationToken)
                           .ConfigureAwait(false))
                {
                    return e.Current;
                }
            }
            return default(TSource);
        }
    }
}
// // Licensed to the .NET Foundation under one or more agreements.
// // The .NET Foundation licenses this file to you under the Apache 2.0 License.
// // See the LICENSE file in the project root for more information. 

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace System.Linq
{
    public static partial class AsyncEnumerable
    {
        public static Task<bool> All<TSource>(this IAsyncEnumerable<TSource> source, Func<TSource, bool> predicate)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));
            if (predicate == null)
                throw new ArgumentNullException(nameof(predicate));

            return All(source, predicate, CancellationToken.None);
        }

        public static Task<bool> All<TSource>(this IAsyncEnumerable<TSource> source, Func<TSource, bool> predicate, CancellationToken cancellationToken)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));
            if (predicate == null)
                throw new ArgumentNullException(nameof(predicate));

            return All_(source, predicate, cancellationToken);
        }

        public static Task<bool> Any<TSource>(this IAsyncEnumerable<TSource> source, Func<TSource, bool> predicate)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));
            if (predicate == null)
                throw new ArgumentNullException(nameof(predicate));

            return Any(source, predicate, CancellationToken.None);
        }

        public static Task<bool> Any<TSource>(this IAsyncEnumerable<TSource> source)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));

            return Any(source, CancellationToken.None);
        }

        public static Task<bool> Any<TSource>(this IAsyncEnumerable<TSource> source, Func<TSource, bool> predicate, CancellationToken cancellationToken)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));
            if (predicate == null)
                throw new ArgumentNullException(nameof(predicate));

            return Any_(source, predicate, cancellationToken);
        }

        public static async Task<bool> Any<TSource>(this IAsyncEnumerable<TSource> source, CancellationToken cancellationToken)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));

            using (var e = source.GetEnumerator())
            {
                return await e.MoveNext(cancellationToken).ConfigureAwait(false);
            }
        }

        private static async Task<bool> All_<TSource>(IAsyncEnumerable<TSource> source, Func<TSource, bool> predicate, CancellationToken cancellationToken)
        {
            using (var e = source.GetEnumerator())
            {
                while (await e.MoveNext(cancellationToken)
                              .ConfigureAwait(false))
                {
                    if (!predicate(e.Current))
                        return false;
                }
            }
            return true;
        }

        private static async Task<bool> Any_<TSource>(IAsyncEnumerable<TSource> source, Func<TSource, bool> predicate, CancellationToken cancellationToken)
        {
            using (var e = source.GetEnumerator())
            {
                while (await e.MoveNext(cancellationToken)
                              .ConfigureAwait(false))
                {
                    if (predicate(e.Current))
                        return true;
                }
            }
            return false;
        }
    }
}
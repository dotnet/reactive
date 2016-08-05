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
        public static Task<bool> SequenceEqual<TSource>(this IAsyncEnumerable<TSource> first, IAsyncEnumerable<TSource> second, IEqualityComparer<TSource> comparer)
        {
            if (first == null)
                throw new ArgumentNullException(nameof(first));
            if (second == null)
                throw new ArgumentNullException(nameof(second));
            if (comparer == null)
                throw new ArgumentNullException(nameof(comparer));

            return SequenceEqual(first, second, comparer, CancellationToken.None);
        }

        public static Task<bool> SequenceEqual<TSource>(this IAsyncEnumerable<TSource> first, IAsyncEnumerable<TSource> second)
        {
            if (first == null)
                throw new ArgumentNullException(nameof(first));
            if (second == null)
                throw new ArgumentNullException(nameof(second));

            return SequenceEqual(first, second, CancellationToken.None);
        }


        public static Task<bool> SequenceEqual<TSource>(this IAsyncEnumerable<TSource> first, IAsyncEnumerable<TSource> second, IEqualityComparer<TSource> comparer, CancellationToken cancellationToken)
        {
            if (first == null)
                throw new ArgumentNullException(nameof(first));
            if (second == null)
                throw new ArgumentNullException(nameof(second));
            if (comparer == null)
                throw new ArgumentNullException(nameof(comparer));

            return SequenceEqual_(first, second, comparer, cancellationToken);
        }

        public static Task<bool> SequenceEqual<TSource>(this IAsyncEnumerable<TSource> first, IAsyncEnumerable<TSource> second, CancellationToken cancellationToken)
        {
            if (first == null)
                throw new ArgumentNullException(nameof(first));
            if (second == null)
                throw new ArgumentNullException(nameof(second));

            return first.SequenceEqual(second, EqualityComparer<TSource>.Default, cancellationToken);
        }

        private static async Task<bool> SequenceEqual_<TSource>(IAsyncEnumerable<TSource> first, IAsyncEnumerable<TSource> second, IEqualityComparer<TSource> comparer,
                                                                CancellationToken cancellationToken)
        {
            var firstCol = first as ICollection<TSource>;
            if (firstCol != null)
            {
                var secondCol = second as ICollection<TSource>;
                if (secondCol != null && firstCol.Count != secondCol.Count)
                {
                    return false;
                }
            }

            using (var e1 = first.GetEnumerator())
            using (var e2 = second.GetEnumerator())
            {
                while (await e1.MoveNext(cancellationToken)
                               .ConfigureAwait(false))
                {
                    if (!(await e2.MoveNext(cancellationToken)
                                  .ConfigureAwait(false) && comparer.Equals(e1.Current, e2.Current)))
                    {
                        return false;
                    }
                }

                return !await e2.MoveNext(cancellationToken)
                                .ConfigureAwait(false);
            }
        }
    }
}
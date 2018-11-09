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
        public static Task<bool> Contains<TSource>(this IAsyncEnumerable<TSource> source, TSource value)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));

            return ContainsCore(source, value, EqualityComparer<TSource>.Default, CancellationToken.None);
        }

        public static Task<bool> Contains<TSource>(this IAsyncEnumerable<TSource> source, TSource value, IEqualityComparer<TSource> comparer)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));
            if (comparer == null)
                throw new ArgumentNullException(nameof(comparer));

            return ContainsCore(source, value, comparer, CancellationToken.None);
        }

        public static Task<bool> Contains<TSource>(this IAsyncEnumerable<TSource> source, TSource value, CancellationToken cancellationToken)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));

            return ContainsCore(source, value, EqualityComparer<TSource>.Default, cancellationToken);
        }

        public static Task<bool> Contains<TSource>(this IAsyncEnumerable<TSource> source, TSource value, IEqualityComparer<TSource> comparer, CancellationToken cancellationToken)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));
            if (comparer == null)
                throw new ArgumentNullException(nameof(comparer));

            return ContainsCore(source, value, comparer, cancellationToken);
        }

        private static Task<bool> ContainsCore<TSource>(IAsyncEnumerable<TSource> source, TSource value, IEqualityComparer<TSource> comparer, CancellationToken cancellationToken)
        {
            return source.Any(x => comparer.Equals(x, value), cancellationToken);
        }
    }
}

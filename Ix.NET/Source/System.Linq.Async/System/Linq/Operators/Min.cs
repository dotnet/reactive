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
        public static Task<TSource> Min<TSource>(this IAsyncEnumerable<TSource> source)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));

            return Min(source, CancellationToken.None);
        }

        public static Task<TSource> Min<TSource>(this IAsyncEnumerable<TSource> source, CancellationToken cancellationToken)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));

            var comparer = Comparer<TSource>.Default;
            return source.Aggregate((x, y) => comparer.Compare(x, y) <= 0 ? x : y, cancellationToken);
        }

        public static Task<TSource> Min<TSource>(this IAsyncEnumerable<TSource> source, IComparer<TSource> comparer)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));
            if (comparer == null)
                throw new ArgumentNullException(nameof(comparer));

            return source.Min(comparer, CancellationToken.None);
        }

        public static Task<TSource> Min<TSource>(this IAsyncEnumerable<TSource> source, IComparer<TSource> comparer, CancellationToken cancellationToken)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));
            if (comparer == null)
                throw new ArgumentNullException(nameof(comparer));

            return Min_(source, comparer, cancellationToken);
        }

        public static Task<TResult> Min<TSource, TResult>(this IAsyncEnumerable<TSource> source, Func<TSource, TResult> selector)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));
            if (selector == null)
                throw new ArgumentNullException(nameof(selector));

            return Min(source, selector, CancellationToken.None);
        }

        public static Task<TResult> Min<TSource, TResult>(this IAsyncEnumerable<TSource> source, Func<TSource, TResult> selector, CancellationToken cancellationToken)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));
            if (selector == null)
                throw new ArgumentNullException(nameof(selector));

            return source.Select(selector).Min(cancellationToken);
        }

        public static Task<TResult> Min<TSource, TResult>(this IAsyncEnumerable<TSource> source, Func<TSource, Task<TResult>> selector)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));
            if (selector == null)
                throw new ArgumentNullException(nameof(selector));

            return Min(source, selector, CancellationToken.None);
        }

        public static Task<TResult> Min<TSource, TResult>(this IAsyncEnumerable<TSource> source, Func<TSource, Task<TResult>> selector, CancellationToken cancellationToken)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));
            if (selector == null)
                throw new ArgumentNullException(nameof(selector));

            return source.Select(selector).Min(cancellationToken);
        }

        private static async Task<TSource> Min_<TSource>(IAsyncEnumerable<TSource> source, IComparer<TSource> comparer, CancellationToken cancellationToken)
        {
            var e = source.GetAsyncEnumerator();

            try
            {
                if (!await e.MoveNextAsync(cancellationToken).ConfigureAwait(false))
                    throw new InvalidOperationException(Strings.NO_ELEMENTS);

                var min = e.Current;

                while (await e.MoveNextAsync(cancellationToken).ConfigureAwait(false))
                {
                    var cur = e.Current;

                    if (comparer.Compare(cur, min) < 0)
                    {
                        min = cur;
                    }
                }

                return min;
            }
            finally
            {
                await e.DisposeAsync().ConfigureAwait(false);
            }
        }
    }
}

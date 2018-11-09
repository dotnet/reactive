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
        public static Task<TSource> Max<TSource>(this IAsyncEnumerable<TSource> source)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));

            return MaxCore(source, Comparer<TSource>.Default, CancellationToken.None);
        }

        public static Task<TSource> Max<TSource>(this IAsyncEnumerable<TSource> source, CancellationToken cancellationToken)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));

            return MaxCore(source, Comparer<TSource>.Default, cancellationToken);
        }

        public static Task<TResult> Max<TSource, TResult>(this IAsyncEnumerable<TSource> source, Func<TSource, TResult> selector)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));
            if (selector == null)
                throw new ArgumentNullException(nameof(selector));

            return MaxCore(source, selector, CancellationToken.None);
        }

        public static Task<TResult> Max<TSource, TResult>(this IAsyncEnumerable<TSource> source, Func<TSource, TResult> selector, CancellationToken cancellationToken)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));
            if (selector == null)
                throw new ArgumentNullException(nameof(selector));

            return MaxCore(source, selector, cancellationToken);
        }

        public static Task<TResult> Max<TSource, TResult>(this IAsyncEnumerable<TSource> source, Func<TSource, Task<TResult>> selector)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));
            if (selector == null)
                throw new ArgumentNullException(nameof(selector));

            return MaxCore(source, selector, CancellationToken.None);
        }

        public static Task<TResult> Max<TSource, TResult>(this IAsyncEnumerable<TSource> source, Func<TSource, Task<TResult>> selector, CancellationToken cancellationToken)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));
            if (selector == null)
                throw new ArgumentNullException(nameof(selector));

            return MaxCore(source, selector, cancellationToken);
        }

        private static async Task<TSource> MaxCore<TSource>(IAsyncEnumerable<TSource> source, IComparer<TSource> comparer, CancellationToken cancellationToken)
        {
            var e = source.GetAsyncEnumerator(cancellationToken);

            try
            {
                if (!await e.MoveNextAsync().ConfigureAwait(false))
                    throw new InvalidOperationException(Strings.NO_ELEMENTS);

                var max = e.Current;

                while (await e.MoveNextAsync().ConfigureAwait(false))
                {
                    var cur = e.Current;

                    if (comparer.Compare(cur, max) > 0)
                    {
                        max = cur;
                    }
                }

                return max;
            }
            finally
            {
                await e.DisposeAsync().ConfigureAwait(false);
            }
        }

        private static Task<TResult> MaxCore<TSource, TResult>(IAsyncEnumerable<TSource> source, Func<TSource, TResult> selector, CancellationToken cancellationToken)
        {
            return MaxCore(source.Select(selector), Comparer<TResult>.Default, cancellationToken);
        }

        private static Task<TResult> MaxCore<TSource, TResult>(IAsyncEnumerable<TSource> source, Func<TSource, Task<TResult>> selector, CancellationToken cancellationToken)
        {
            return MaxCore(source.Select(selector), Comparer<TResult>.Default, cancellationToken);
        }
    }
}

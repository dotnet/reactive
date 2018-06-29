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
        public static void ForEach<TSource>(this IAsyncEnumerable<TSource> source, Action<TSource> action)
        {
            if (source == null)
            {
                throw new ArgumentNullException(nameof(source));
            }

            if (action == null)
            {
                throw new ArgumentNullException(nameof(action));
            }

            source.ForEach(action, CancellationToken.None);
        }

        public static void ForEach<TSource>(this IAsyncEnumerable<TSource> source, Action<TSource, int> action)
        {
            if (source == null)
            {
                throw new ArgumentNullException(nameof(source));
            }

            if (action == null)
            {
                throw new ArgumentNullException(nameof(action));
            }

            source.ForEach(action, CancellationToken.None);
        }


        public static void ForEach<TSource>(this IAsyncEnumerable<TSource> source, Action<TSource> action, CancellationToken cancellationToken)
        {
            if (source == null)
            {
                throw new ArgumentNullException(nameof(source));
            }

            if (action == null)
            {
                throw new ArgumentNullException(nameof(action));
            }

            source.ForEachAsync(action, cancellationToken)
                  .Wait(cancellationToken);
        }

        public static void ForEach<TSource>(this IAsyncEnumerable<TSource> source, Action<TSource, int> action, CancellationToken cancellationToken)
        {
            if (source == null)
            {
                throw new ArgumentNullException(nameof(source));
            }

            if (action == null)
            {
                throw new ArgumentNullException(nameof(action));
            }

            source.ForEachAsync(action, cancellationToken)
                  .Wait(cancellationToken);
        }

        public static Task ForEachAsync<TSource>(this IAsyncEnumerable<TSource> source, Action<TSource> action)
        {
            if (source == null)
            {
                throw new ArgumentNullException(nameof(source));
            }

            if (action == null)
            {
                throw new ArgumentNullException(nameof(action));
            }

            return ForEachAsync(source, action, CancellationToken.None);
        }

        public static Task ForEachAsync<TSource>(this IAsyncEnumerable<TSource> source, Action<TSource, int> action)
        {
            if (source == null)
            {
                throw new ArgumentNullException(nameof(source));
            }

            if (action == null)
            {
                throw new ArgumentNullException(nameof(action));
            }

            return ForEachAsync(source, action, CancellationToken.None);
        }

        public static Task ForEachAsync<TSource>(this IAsyncEnumerable<TSource> source, Action<TSource> action, CancellationToken cancellationToken)
        {
            if (source == null)
            {
                throw new ArgumentNullException(nameof(source));
            }

            if (action == null)
            {
                throw new ArgumentNullException(nameof(action));
            }

            return source.ForEachAsync((x, i) => action(x), cancellationToken);
        }

        public static Task ForEachAsync<TSource>(this IAsyncEnumerable<TSource> source, Action<TSource, int> action, CancellationToken cancellationToken)
        {
            if (source == null)
            {
                throw new ArgumentNullException(nameof(source));
            }

            if (action == null)
            {
                throw new ArgumentNullException(nameof(action));
            }

            return ForEachAsync_(source, action, cancellationToken);
        }

        private static async Task ForEachAsync_<TSource>(IAsyncEnumerable<TSource> source, Action<TSource, int> action, CancellationToken cancellationToken)
        {
            var index = 0;
            using (var e = source.GetEnumerator())
            {
                while (await e.MoveNext(cancellationToken)
                              .ConfigureAwait(false))
                {
                    action(e.Current, checked(index++));
                }
            }
        }
    }
}
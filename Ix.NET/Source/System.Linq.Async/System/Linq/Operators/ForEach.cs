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
        public static Task ForEachAsync<TSource>(this IAsyncEnumerable<TSource> source, Action<TSource> action)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));
            if (action == null)
                throw new ArgumentNullException(nameof(action));

            return ForEachAsyncCore(source, action, CancellationToken.None);
        }

        public static Task ForEachAsync<TSource>(this IAsyncEnumerable<TSource> source, Action<TSource> action, CancellationToken cancellationToken)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));
            if (action == null)
                throw new ArgumentNullException(nameof(action));

            return ForEachAsyncCore(source, action, cancellationToken);
        }

        public static Task ForEachAsync<TSource>(this IAsyncEnumerable<TSource> source, Action<TSource, int> action)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));
            if (action == null)
                throw new ArgumentNullException(nameof(action));

            return ForEachAsyncCore(source, action, CancellationToken.None);
        }

        public static Task ForEachAsync<TSource>(this IAsyncEnumerable<TSource> source, Action<TSource, int> action, CancellationToken cancellationToken)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));
            if (action == null)
                throw new ArgumentNullException(nameof(action));

            return ForEachAsyncCore(source, action, cancellationToken);
        }

        public static Task ForEachAsync<TSource>(this IAsyncEnumerable<TSource> source, Func<TSource, Task> action)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));
            if (action == null)
                throw new ArgumentNullException(nameof(action));

            return ForEachAsyncCore(source, (x, ct) => action(x), CancellationToken.None);
        }

        public static Task ForEachAsync<TSource>(this IAsyncEnumerable<TSource> source, Func<TSource, Task> action, CancellationToken cancellationToken)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));
            if (action == null)
                throw new ArgumentNullException(nameof(action));

            return ForEachAsyncCore(source, (x, ct) => action(x), cancellationToken);
        }

        public static Task ForEachAsync<TSource>(this IAsyncEnumerable<TSource> source, Func<TSource, CancellationToken, Task> action, CancellationToken cancellationToken)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));
            if (action == null)
                throw new ArgumentNullException(nameof(action));

            return ForEachAsyncCore(source, action, cancellationToken);
        }

        public static Task ForEachAsync<TSource>(this IAsyncEnumerable<TSource> source, Func<TSource, int, Task> action)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));
            if (action == null)
                throw new ArgumentNullException(nameof(action));

            return ForEachAsyncCore(source, (x, i, ct) => action(x, i), CancellationToken.None);
        }

        public static Task ForEachAsync<TSource>(this IAsyncEnumerable<TSource> source, Func<TSource, int, Task> action, CancellationToken cancellationToken)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));
            if (action == null)
                throw new ArgumentNullException(nameof(action));

            return ForEachAsyncCore(source, (x, i, ct) => action(x, i), cancellationToken);
        }

        public static Task ForEachAsync<TSource>(this IAsyncEnumerable<TSource> source, Func<TSource, int, CancellationToken, Task> action, CancellationToken cancellationToken)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));
            if (action == null)
                throw new ArgumentNullException(nameof(action));

            return ForEachAsyncCore(source, action, cancellationToken);
        }

        private static async Task ForEachAsyncCore<TSource>(IAsyncEnumerable<TSource> source, Action<TSource> action, CancellationToken cancellationToken)
        {
            var e = source.GetAsyncEnumerator(cancellationToken);

            try
            {
                while (await e.MoveNextAsync().ConfigureAwait(false))
                {
                    action(e.Current);
                }
            }
            finally
            {
                await e.DisposeAsync().ConfigureAwait(false);
            }
        }

        private static async Task ForEachAsyncCore<TSource>(IAsyncEnumerable<TSource> source, Func<TSource, CancellationToken, Task> action, CancellationToken cancellationToken)
        {
            var e = source.GetAsyncEnumerator(cancellationToken);

            try
            {
                while (await e.MoveNextAsync().ConfigureAwait(false))
                {
                    await action(e.Current, cancellationToken).ConfigureAwait(false);
                }
            }
            finally
            {
                await e.DisposeAsync().ConfigureAwait(false);
            }
        }

        private static async Task ForEachAsyncCore<TSource>(IAsyncEnumerable<TSource> source, Action<TSource, int> action, CancellationToken cancellationToken)
        {
            var index = 0;

            var e = source.GetAsyncEnumerator(cancellationToken);

            try
            {
                while (await e.MoveNextAsync().ConfigureAwait(false))
                {
                    action(e.Current, checked(index++));
                }
            }
            finally
            {
                await e.DisposeAsync().ConfigureAwait(false);
            }
        }

        private static async Task ForEachAsyncCore<TSource>(IAsyncEnumerable<TSource> source, Func<TSource, int, CancellationToken, Task> action, CancellationToken cancellationToken)
        {
            var index = 0;

            var e = source.GetAsyncEnumerator(cancellationToken);

            try
            {
                while (await e.MoveNextAsync().ConfigureAwait(false))
                {
                    await action(e.Current, checked(index++), cancellationToken).ConfigureAwait(false);
                }
            }
            finally
            {
                await e.DisposeAsync().ConfigureAwait(false);
            }
        }
    }
}

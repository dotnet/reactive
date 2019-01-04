// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the Apache 2.0 License.
// See the LICENSE file in the project root for more information. 

#if !HAS_AWAIT_FOREACH

using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace System.Linq
{
    public static partial class AsyncEnumerable
    {
        // REVIEW: Once we have C# 8.0 language support, we may want to do away with these methods. An open question is how to
        //         provide support for cancellation, which could be offered through WithCancellation on the source. If we still
        //         want to keep these methods, they may be a candidate for System.Interactive.Async if we consider them to be
        //         non-standard (i.e. IEnumerable<T> doesn't have a ForEach extension method either).

        public static Task ForEachAsync<TSource>(this IAsyncEnumerable<TSource> source, Action<TSource> action, CancellationToken cancellationToken = default)
        {
            if (source == null)
                throw Error.ArgumentNull(nameof(source));
            if (action == null)
                throw Error.ArgumentNull(nameof(action));

            return ForEachAsyncCore(source, action, cancellationToken);
        }

        public static Task ForEachAsync<TSource>(this IAsyncEnumerable<TSource> source, Action<TSource, int> action, CancellationToken cancellationToken = default)
        {
            if (source == null)
                throw Error.ArgumentNull(nameof(source));
            if (action == null)
                throw Error.ArgumentNull(nameof(action));

            return ForEachAsyncCore(source, action, cancellationToken);
        }

        public static Task ForEachAsync<TSource>(this IAsyncEnumerable<TSource> source, Func<TSource, Task> action, CancellationToken cancellationToken = default)
        {
            if (source == null)
                throw Error.ArgumentNull(nameof(source));
            if (action == null)
                throw Error.ArgumentNull(nameof(action));

            return ForEachAsyncCore(source, (x, ct) => action(x), cancellationToken);
        }

        public static Task ForEachAsync<TSource>(this IAsyncEnumerable<TSource> source, Func<TSource, CancellationToken, Task> action, CancellationToken cancellationToken)
        {
            if (source == null)
                throw Error.ArgumentNull(nameof(source));
            if (action == null)
                throw Error.ArgumentNull(nameof(action));

            return ForEachAsyncCore(source, action, cancellationToken);
        }

        public static Task ForEachAsync<TSource>(this IAsyncEnumerable<TSource> source, Func<TSource, int, Task> action, CancellationToken cancellationToken = default)
        {
            if (source == null)
                throw Error.ArgumentNull(nameof(source));
            if (action == null)
                throw Error.ArgumentNull(nameof(action));

            return ForEachAsyncCore(source, (x, i, ct) => action(x, i), cancellationToken);
        }

        public static Task ForEachAsync<TSource>(this IAsyncEnumerable<TSource> source, Func<TSource, int, CancellationToken, Task> action, CancellationToken cancellationToken)
        {
            if (source == null)
                throw Error.ArgumentNull(nameof(source));
            if (action == null)
                throw Error.ArgumentNull(nameof(action));

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

#endif

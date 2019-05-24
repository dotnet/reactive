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
        public static ValueTask<TSource> AggregateAsync<TSource>(this IAsyncEnumerable<TSource> source, Func<TSource, TSource, TSource> accumulator, CancellationToken cancellationToken = default)
        {
            if (source == null)
                throw Error.ArgumentNull(nameof(source));
            if (accumulator == null)
                throw Error.ArgumentNull(nameof(accumulator));

            return Core(source, accumulator, cancellationToken);

            static async ValueTask<TSource> Core(IAsyncEnumerable<TSource> source, Func<TSource, TSource, TSource> accumulator, CancellationToken cancellationToken)
            {
                await using (var e = source.GetConfiguredAsyncEnumerator(cancellationToken, false))
                {
                    if (!await e.MoveNextAsync())
                    {
                        throw Error.NoElements();
                    }

                    var acc = e.Current;

                    while (await e.MoveNextAsync())
                    {
                        acc = accumulator(acc, e.Current);
                    }

                    return acc;
                }
            }
        }

        internal static ValueTask<TSource> AggregateAwaitAsyncCore<TSource>(this IAsyncEnumerable<TSource> source, Func<TSource, TSource, ValueTask<TSource>> accumulator, CancellationToken cancellationToken = default)
        {
            if (source == null)
                throw Error.ArgumentNull(nameof(source));
            if (accumulator == null)
                throw Error.ArgumentNull(nameof(accumulator));

            return Core(source, accumulator, cancellationToken);

            static async ValueTask<TSource> Core(IAsyncEnumerable<TSource> source, Func<TSource, TSource, ValueTask<TSource>> accumulator, CancellationToken cancellationToken)
            {
                await using (var e = source.GetConfiguredAsyncEnumerator(cancellationToken, false))
                {
                    if (!await e.MoveNextAsync())
                    {
                        throw Error.NoElements();
                    }

                    var acc = e.Current;

                    while (await e.MoveNextAsync())
                    {
                        acc = await accumulator(acc, e.Current).ConfigureAwait(false);
                    }

                    return acc;
                }
            }
        }

#if !NO_DEEP_CANCELLATION
        internal static ValueTask<TSource> AggregateAwaitWithCancellationAsyncCore<TSource>(this IAsyncEnumerable<TSource> source, Func<TSource, TSource, CancellationToken, ValueTask<TSource>> accumulator, CancellationToken cancellationToken = default)
        {
            if (source == null)
                throw Error.ArgumentNull(nameof(source));
            if (accumulator == null)
                throw Error.ArgumentNull(nameof(accumulator));

            return Core(source, accumulator, cancellationToken);

            static async ValueTask<TSource> Core(IAsyncEnumerable<TSource> source, Func<TSource, TSource, CancellationToken, ValueTask<TSource>> accumulator, CancellationToken cancellationToken)
            {
                await using (var e = source.GetConfiguredAsyncEnumerator(cancellationToken, false))
                {
                    if (!await e.MoveNextAsync())
                    {
                        throw Error.NoElements();
                    }

                    var acc = e.Current;

                    while (await e.MoveNextAsync())
                    {
                        acc = await accumulator(acc, e.Current, cancellationToken).ConfigureAwait(false);
                    }

                    return acc;
                }
            }
        }
#endif

        public static ValueTask<TAccumulate> AggregateAsync<TSource, TAccumulate>(this IAsyncEnumerable<TSource> source, TAccumulate seed, Func<TAccumulate, TSource, TAccumulate> accumulator, CancellationToken cancellationToken = default)
        {
            if (source == null)
                throw Error.ArgumentNull(nameof(source));
            if (accumulator == null)
                throw Error.ArgumentNull(nameof(accumulator));

            return Core(source, seed, accumulator, cancellationToken);

            static async ValueTask<TAccumulate> Core(IAsyncEnumerable<TSource> source, TAccumulate seed, Func<TAccumulate, TSource, TAccumulate> accumulator, CancellationToken cancellationToken)
            {
                var acc = seed;

                await foreach (var item in source.WithCancellation(cancellationToken).ConfigureAwait(false))
                {
                    acc = accumulator(acc, item);
                }

                return acc;
            }
        }

        internal static ValueTask<TAccumulate> AggregateAwaitAsyncCore<TSource, TAccumulate>(this IAsyncEnumerable<TSource> source, TAccumulate seed, Func<TAccumulate, TSource, ValueTask<TAccumulate>> accumulator, CancellationToken cancellationToken = default)
        {
            if (source == null)
                throw Error.ArgumentNull(nameof(source));
            if (accumulator == null)
                throw Error.ArgumentNull(nameof(accumulator));

            return Core(source, seed, accumulator, cancellationToken);

            static async ValueTask<TAccumulate> Core(IAsyncEnumerable<TSource> source, TAccumulate seed, Func<TAccumulate, TSource, ValueTask<TAccumulate>> accumulator, CancellationToken cancellationToken)
            {
                var acc = seed;

                await foreach (var item in source.WithCancellation(cancellationToken).ConfigureAwait(false))
                {
                    acc = await accumulator(acc, item).ConfigureAwait(false);
                }

                return acc;
            }
        }

#if !NO_DEEP_CANCELLATION
        internal static ValueTask<TAccumulate> AggregateAwaitWithCancellationAsyncCore<TSource, TAccumulate>(this IAsyncEnumerable<TSource> source, TAccumulate seed, Func<TAccumulate, TSource, CancellationToken, ValueTask<TAccumulate>> accumulator, CancellationToken cancellationToken = default)
        {
            if (source == null)
                throw Error.ArgumentNull(nameof(source));
            if (accumulator == null)
                throw Error.ArgumentNull(nameof(accumulator));

            return Core(source, seed, accumulator, cancellationToken);

            static async ValueTask<TAccumulate> Core(IAsyncEnumerable<TSource> source, TAccumulate seed, Func<TAccumulate, TSource, CancellationToken, ValueTask<TAccumulate>> accumulator, CancellationToken cancellationToken)
            {
                var acc = seed;

                await foreach (var item in source.WithCancellation(cancellationToken).ConfigureAwait(false))
                {
                    acc = await accumulator(acc, item, cancellationToken).ConfigureAwait(false);
                }

                return acc;
            }
        }
#endif

        public static ValueTask<TResult> AggregateAsync<TSource, TAccumulate, TResult>(this IAsyncEnumerable<TSource> source, TAccumulate seed, Func<TAccumulate, TSource, TAccumulate> accumulator, Func<TAccumulate, TResult> resultSelector, CancellationToken cancellationToken = default)
        {
            if (source == null)
                throw Error.ArgumentNull(nameof(source));
            if (accumulator == null)
                throw Error.ArgumentNull(nameof(accumulator));
            if (resultSelector == null)
                throw Error.ArgumentNull(nameof(resultSelector));

            return Core(source, seed, accumulator, resultSelector, cancellationToken);

            static async ValueTask<TResult> Core(IAsyncEnumerable<TSource> source, TAccumulate seed, Func<TAccumulate, TSource, TAccumulate> accumulator, Func<TAccumulate, TResult> resultSelector, CancellationToken cancellationToken)
            {
                var acc = seed;

                await foreach (var item in source.WithCancellation(cancellationToken).ConfigureAwait(false))
                {
                    acc = accumulator(acc, item);
                }

                return resultSelector(acc);
            }
        }

        internal static ValueTask<TResult> AggregateAwaitAsyncCore<TSource, TAccumulate, TResult>(this IAsyncEnumerable<TSource> source, TAccumulate seed, Func<TAccumulate, TSource, ValueTask<TAccumulate>> accumulator, Func<TAccumulate, ValueTask<TResult>> resultSelector, CancellationToken cancellationToken = default)
        {
            if (source == null)
                throw Error.ArgumentNull(nameof(source));
            if (accumulator == null)
                throw Error.ArgumentNull(nameof(accumulator));
            if (resultSelector == null)
                throw Error.ArgumentNull(nameof(resultSelector));

            return Core(source, seed, accumulator, resultSelector, cancellationToken);

            static async ValueTask<TResult> Core(IAsyncEnumerable<TSource> source, TAccumulate seed, Func<TAccumulate, TSource, ValueTask<TAccumulate>> accumulator, Func<TAccumulate, ValueTask<TResult>> resultSelector, CancellationToken cancellationToken)
            {
                var acc = seed;

                await foreach (var item in source.WithCancellation(cancellationToken).ConfigureAwait(false))
                {
                    acc = await accumulator(acc, item).ConfigureAwait(false);
                }

                return await resultSelector(acc).ConfigureAwait(false);
            }
        }

#if !NO_DEEP_CANCELLATION
        internal static ValueTask<TResult> AggregateAwaitWithCancellationAsyncCore<TSource, TAccumulate, TResult>(this IAsyncEnumerable<TSource> source, TAccumulate seed, Func<TAccumulate, TSource, CancellationToken, ValueTask<TAccumulate>> accumulator, Func<TAccumulate, CancellationToken, ValueTask<TResult>> resultSelector, CancellationToken cancellationToken = default)
        {
            if (source == null)
                throw Error.ArgumentNull(nameof(source));
            if (accumulator == null)
                throw Error.ArgumentNull(nameof(accumulator));
            if (resultSelector == null)
                throw Error.ArgumentNull(nameof(resultSelector));

            return Core(source, seed, accumulator, resultSelector, cancellationToken);

            static async ValueTask<TResult> Core(IAsyncEnumerable<TSource> source, TAccumulate seed, Func<TAccumulate, TSource, CancellationToken, ValueTask<TAccumulate>> accumulator, Func<TAccumulate, CancellationToken, ValueTask<TResult>> resultSelector, CancellationToken cancellationToken)
            {
                var acc = seed;

                await foreach (var item in source.WithCancellation(cancellationToken).ConfigureAwait(false))
                {
                    acc = await accumulator(acc, item, cancellationToken).ConfigureAwait(false);
                }

                return await resultSelector(acc, cancellationToken).ConfigureAwait(false);
            }
        }
#endif
    }
}

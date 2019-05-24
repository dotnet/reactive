// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the Apache 2.0 License.
// See the LICENSE file in the project root for more information. 

using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace System.Linq
{
    public static partial class AsyncEnumerableEx
    {
        // NB: Implementations of Scan never yield the first element, unlike the behavior of Aggregate on a sequence with one
        //     element, which returns the first element (or the seed if given an empty sequence). This is compatible with Rx
        //     but one could argue whether it was the right default.

        public static IAsyncEnumerable<TSource> Scan<TSource>(this IAsyncEnumerable<TSource> source, Func<TSource, TSource, TSource> accumulator)
        {
            if (source == null)
                throw Error.ArgumentNull(nameof(source));
            if (accumulator == null)
                throw Error.ArgumentNull(nameof(accumulator));

            return AsyncEnumerable.Create(Core);

            async IAsyncEnumerator<TSource> Core(CancellationToken cancellationToken)
            {
                await using (var e = source.GetConfiguredAsyncEnumerator(cancellationToken, false))
                {
                    if (!await e.MoveNextAsync())
                    {
                        yield break;
                    }

                    var res = e.Current;

                    while (await e.MoveNextAsync())
                    {
                        res = accumulator(res, e.Current);

                        yield return res;
                    }
                }
            }
        }

        public static IAsyncEnumerable<TAccumulate> Scan<TSource, TAccumulate>(this IAsyncEnumerable<TSource> source, TAccumulate seed, Func<TAccumulate, TSource, TAccumulate> accumulator)
        {
            if (source == null)
                throw Error.ArgumentNull(nameof(source));
            if (accumulator == null)
                throw Error.ArgumentNull(nameof(accumulator));

            return AsyncEnumerable.Create(Core);

            async IAsyncEnumerator<TAccumulate> Core(CancellationToken cancellationToken)
            {
                var res = seed;

                await foreach (var item in source.WithCancellation(cancellationToken).ConfigureAwait(false))
                {
                    res = accumulator(res, item);

                    yield return res;
                }
            }
        }

        public static IAsyncEnumerable<TSource> Scan<TSource>(this IAsyncEnumerable<TSource> source, Func<TSource, TSource, ValueTask<TSource>> accumulator)
        {
            if (source == null)
                throw Error.ArgumentNull(nameof(source));
            if (accumulator == null)
                throw Error.ArgumentNull(nameof(accumulator));

            return AsyncEnumerable.Create(Core);

            async IAsyncEnumerator<TSource> Core(CancellationToken cancellationToken)
            {
                await using (var e = source.GetConfiguredAsyncEnumerator(cancellationToken, false))
                {
                    if (!await e.MoveNextAsync())
                    {
                        yield break;
                    }

                    var res = e.Current;

                    while (await e.MoveNextAsync())
                    {
                        res = await accumulator(res, e.Current).ConfigureAwait(false);

                        yield return res;
                    }
                }
            }
        }

#if !NO_DEEP_CANCELLATION
        public static IAsyncEnumerable<TSource> Scan<TSource>(this IAsyncEnumerable<TSource> source, Func<TSource, TSource, CancellationToken, ValueTask<TSource>> accumulator)
        {
            if (source == null)
                throw Error.ArgumentNull(nameof(source));
            if (accumulator == null)
                throw Error.ArgumentNull(nameof(accumulator));

            return AsyncEnumerable.Create(Core);

            async IAsyncEnumerator<TSource> Core(CancellationToken cancellationToken)
            {
                await using (var e = source.GetConfiguredAsyncEnumerator(cancellationToken, false))
                {
                    if (!await e.MoveNextAsync())
                    {
                        yield break;
                    }

                    var res = e.Current;

                    while (await e.MoveNextAsync())
                    {
                        res = await accumulator(res, e.Current, cancellationToken).ConfigureAwait(false);

                        yield return res;
                    }
                }
            }
        }
#endif

        public static IAsyncEnumerable<TAccumulate> Scan<TSource, TAccumulate>(this IAsyncEnumerable<TSource> source, TAccumulate seed, Func<TAccumulate, TSource, ValueTask<TAccumulate>> accumulator)
        {
            if (source == null)
                throw Error.ArgumentNull(nameof(source));
            if (accumulator == null)
                throw Error.ArgumentNull(nameof(accumulator));

            return AsyncEnumerable.Create(Core);

            async IAsyncEnumerator<TAccumulate> Core(CancellationToken cancellationToken)
            {
                var res = seed;

                await foreach (var item in source.WithCancellation(cancellationToken).ConfigureAwait(false))
                {
                    res = await accumulator(res, item).ConfigureAwait(false);

                    yield return res;
                }
            }
        }

#if !NO_DEEP_CANCELLATION
        public static IAsyncEnumerable<TAccumulate> Scan<TSource, TAccumulate>(this IAsyncEnumerable<TSource> source, TAccumulate seed, Func<TAccumulate, TSource, CancellationToken, ValueTask<TAccumulate>> accumulator)
        {
            if (source == null)
                throw Error.ArgumentNull(nameof(source));
            if (accumulator == null)
                throw Error.ArgumentNull(nameof(accumulator));

            return AsyncEnumerable.Create(Core);

            async IAsyncEnumerator<TAccumulate> Core(CancellationToken cancellationToken)
            {
                var res = seed;

                await foreach (var item in source.WithCancellation(cancellationToken).ConfigureAwait(false))
                {
                    res = await accumulator(res, item, cancellationToken).ConfigureAwait(false);

                    yield return res;
                }
            }
        }
#endif
    }
}

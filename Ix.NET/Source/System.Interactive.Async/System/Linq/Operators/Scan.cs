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

        /// <summary>
        /// Applies an accumulator function over an async-enumerable sequence and returns each intermediate result.
        /// For aggregation behavior with no intermediate results, see <see cref="AsyncEnumerable.AggregateAsync{TSource}"/>.
        /// </summary>
        /// <typeparam name="TSource">The type of the elements in the source sequence and the result of the aggregation.</typeparam>
        /// <param name="source">An async-enumerable sequence to accumulate over.</param>
        /// <param name="accumulator">An accumulator function to be invoked on each element.</param>
        /// <returns>An async-enumerable sequence containing the accumulated values.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="source"/> or <paramref name="accumulator"/> is null.</exception>
        public static IAsyncEnumerable<TSource> Scan<TSource>(this IAsyncEnumerable<TSource> source, Func<TSource, TSource, TSource> accumulator)
        {
            if (source == null)
                throw Error.ArgumentNull(nameof(source));
            if (accumulator == null)
                throw Error.ArgumentNull(nameof(accumulator));

            return AsyncEnumerable.Create(Core);

            async IAsyncEnumerator<TSource> Core(CancellationToken cancellationToken)
            {
                await using var e = source.GetConfiguredAsyncEnumerator(cancellationToken, false);

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

        /// <summary>
        /// Applies an accumulator function over an async-enumerable sequence and returns each intermediate result. The specified seed value is used as the initial accumulator value.
        /// For aggregation behavior with no intermediate results, see <see cref="AsyncEnumerable.AggregateAsync{TSource, Accumulate}"/>.
        /// </summary>
        /// <typeparam name="TSource">The type of the elements in the source sequence.</typeparam>
        /// <typeparam name="TAccumulate">The type of the result of the aggregation.</typeparam>
        /// <param name="source">An async-enumerable sequence to accumulate over.</param>
        /// <param name="seed">The initial accumulator value.</param>
        /// <param name="accumulator">An accumulator function to be invoked on each element.</param>
        /// <returns>An async-enumerable sequence containing the accumulated values.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="source"/> or <paramref name="accumulator"/> is null.</exception>
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

        /// <summary>
        /// Applies an asynchronous accumulator function over an async-enumerable sequence and returns each intermediate result.
        /// For aggregation behavior with no intermediate results, see <see cref="AsyncEnumerable.AggregateAsync{TSource}"/>.
        /// </summary>
        /// <typeparam name="TSource">The type of the elements in the source sequence and the result of the aggregation.</typeparam>
        /// <param name="source">An async-enumerable sequence to accumulate over.</param>
        /// <param name="accumulator">An asynchronous accumulator function to be invoked and awaited on each element.</param>
        /// <returns>An async-enumerable sequence containing the accumulated values.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="source"/> or <paramref name="accumulator"/> is null.</exception>
        public static IAsyncEnumerable<TSource> Scan<TSource>(this IAsyncEnumerable<TSource> source, Func<TSource, TSource, ValueTask<TSource>> accumulator)
        {
            if (source == null)
                throw Error.ArgumentNull(nameof(source));
            if (accumulator == null)
                throw Error.ArgumentNull(nameof(accumulator));

            return AsyncEnumerable.Create(Core);

            async IAsyncEnumerator<TSource> Core(CancellationToken cancellationToken)
            {
                await using var e = source.GetConfiguredAsyncEnumerator(cancellationToken, false);

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

#if !NO_DEEP_CANCELLATION
        /// <summary>
        /// Applies an asynchronous (cancellable) accumulator function over an async-enumerable sequence and returns each intermediate result.
        /// For aggregation behavior with no intermediate results, see <see cref="AsyncEnumerable.AggregateAsync{TSource}"/>.
        /// </summary>
        /// <typeparam name="TSource">The type of the elements in the source sequence and the result of the aggregation.</typeparam>
        /// <param name="source">An async-enumerable sequence to accumulate over.</param>
        /// <param name="accumulator">An asynchronous (cancellable) accumulator function to be invoked and awaited on each element.</param>
        /// <returns>An async-enumerable sequence containing the accumulated values.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="source"/> or <paramref name="accumulator"/> is null.</exception>
        public static IAsyncEnumerable<TSource> Scan<TSource>(this IAsyncEnumerable<TSource> source, Func<TSource, TSource, CancellationToken, ValueTask<TSource>> accumulator)
        {
            if (source == null)
                throw Error.ArgumentNull(nameof(source));
            if (accumulator == null)
                throw Error.ArgumentNull(nameof(accumulator));

            return AsyncEnumerable.Create(Core);

            async IAsyncEnumerator<TSource> Core(CancellationToken cancellationToken)
            {
                await using var e = source.GetConfiguredAsyncEnumerator(cancellationToken, false);

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
#endif

        /// <summary>
        /// Applies an asynchronous accumulator function over an async-enumerable sequence and returns each intermediate result. The specified seed value is used as the initial accumulator value.
        /// For aggregation behavior with no intermediate results, see <see cref="AsyncEnumerable.AggregateAsync{TSource, Accumulate}"/>.
        /// </summary>
        /// <typeparam name="TSource">The type of the elements in the source sequence.</typeparam>
        /// <typeparam name="TAccumulate">The type of the result of the aggregation.</typeparam>
        /// <param name="source">An async-enumerable sequence to accumulate over.</param>
        /// <param name="seed">The initial accumulator value.</param>
        /// <param name="accumulator">An asynchronous accumulator function to be invoked on each element.</param>
        /// <returns>An async-enumerable sequence containing the accumulated values.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="source"/> or <paramref name="accumulator"/> is null.</exception>
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
        /// <summary>
        /// Applies an asynchronous (cancellable) accumulator function over an async-enumerable sequence and returns each intermediate result. The specified seed value is used as the initial accumulator value.
        /// For aggregation behavior with no intermediate results, see <see cref="AsyncEnumerable.AggregateAsync{TSource, Accumulate}"/>.
        /// </summary>
        /// <typeparam name="TSource">The type of the elements in the source sequence.</typeparam>
        /// <typeparam name="TAccumulate">The type of the result of the aggregation.</typeparam>
        /// <param name="source">An async-enumerable sequence to accumulate over.</param>
        /// <param name="seed">The initial accumulator value.</param>
        /// <param name="accumulator">An asynchronous (cancellable) accumulator function to be invoked on each element.</param>
        /// <returns>An async-enumerable sequence containing the accumulated values.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="source"/> or <paramref name="accumulator"/> is null.</exception>
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

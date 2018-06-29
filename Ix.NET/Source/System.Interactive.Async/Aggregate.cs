// // Licensed to the .NET Foundation under one or more agreements.
// // The .NET Foundation licenses this file to you under the Apache 2.0 License.
// // See the LICENSE file in the project root for more information. 

using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace System.Linq
{
    public static partial class AsyncEnumerable
    {
        public static Task<TResult> Aggregate<TSource, TAccumulate, TResult>(this IAsyncEnumerable<TSource> source, TAccumulate seed, Func<TAccumulate, TSource, TAccumulate> accumulator, Func<TAccumulate, TResult> resultSelector)
        {
            if (source == null)
            {
                throw new ArgumentNullException(nameof(source));
            }

            if (accumulator == null)
            {
                throw new ArgumentNullException(nameof(accumulator));
            }

            if (resultSelector == null)
            {
                throw new ArgumentNullException(nameof(resultSelector));
            }

            return Aggregate(source, seed, accumulator, resultSelector, CancellationToken.None);
        }

        public static Task<TAccumulate> Aggregate<TSource, TAccumulate>(this IAsyncEnumerable<TSource> source, TAccumulate seed, Func<TAccumulate, TSource, TAccumulate> accumulator)
        {
            if (source == null)
            {
                throw new ArgumentNullException(nameof(source));
            }

            if (accumulator == null)
            {
                throw new ArgumentNullException(nameof(accumulator));
            }

            return Aggregate(source, seed, accumulator, CancellationToken.None);
        }

        public static Task<TSource> Aggregate<TSource>(this IAsyncEnumerable<TSource> source, Func<TSource, TSource, TSource> accumulator)
        {
            if (source == null)
            {
                throw new ArgumentNullException(nameof(source));
            }

            if (accumulator == null)
            {
                throw new ArgumentNullException(nameof(accumulator));
            }

            return Aggregate(source, accumulator, CancellationToken.None);
        }

        public static Task<TResult> Aggregate<TSource, TAccumulate, TResult>(this IAsyncEnumerable<TSource> source, TAccumulate seed, Func<TAccumulate, TSource, TAccumulate> accumulator, Func<TAccumulate, TResult> resultSelector, CancellationToken cancellationToken)
        {
            if (source == null)
            {
                throw new ArgumentNullException(nameof(source));
            }

            if (accumulator == null)
            {
                throw new ArgumentNullException(nameof(accumulator));
            }

            if (resultSelector == null)
            {
                throw new ArgumentNullException(nameof(resultSelector));
            }

            return Aggregate_(source, seed, accumulator, resultSelector, cancellationToken);
        }

        public static Task<TAccumulate> Aggregate<TSource, TAccumulate>(this IAsyncEnumerable<TSource> source, TAccumulate seed, Func<TAccumulate, TSource, TAccumulate> accumulator, CancellationToken cancellationToken)
        {
            if (source == null)
            {
                throw new ArgumentNullException(nameof(source));
            }

            if (accumulator == null)
            {
                throw new ArgumentNullException(nameof(accumulator));
            }

            return source.Aggregate(seed, accumulator, x => x, cancellationToken);
        }

        public static Task<TSource> Aggregate<TSource>(this IAsyncEnumerable<TSource> source, Func<TSource, TSource, TSource> accumulator, CancellationToken cancellationToken)
        {
            if (source == null)
            {
                throw new ArgumentNullException(nameof(source));
            }

            if (accumulator == null)
            {
                throw new ArgumentNullException(nameof(accumulator));
            }

            return Aggregate_(source, accumulator, cancellationToken);
        }

        private static async Task<TResult> Aggregate_<TSource, TAccumulate, TResult>(IAsyncEnumerable<TSource> source, TAccumulate seed,
                                                                                     Func<TAccumulate, TSource, TAccumulate> accumulator, Func<TAccumulate, TResult> resultSelector, CancellationToken cancellationToken)
        {
            var acc = seed;

            using (var e = source.GetEnumerator())
            {
                while (await e.MoveNext(cancellationToken)
                              .ConfigureAwait(false))
                {
                    acc = accumulator(acc, e.Current);
                }
            }

            return resultSelector(acc);
        }

        private static async Task<TSource> Aggregate_<TSource>(IAsyncEnumerable<TSource> source, Func<TSource, TSource, TSource> accumulator, CancellationToken cancellationToken)
        {
            var first = true;
            var acc = default(TSource);

            using (var e = source.GetEnumerator())
            {
                while (await e.MoveNext(cancellationToken)
                              .ConfigureAwait(false))
                {
                    acc = first ? e.Current : accumulator(acc, e.Current);
                    first = false;
                }
            }
            if (first)
            {
                throw new InvalidOperationException(Strings.NO_ELEMENTS);
            }

            return acc;
        }
    }
}
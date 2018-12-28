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
        public static Task<TSource> AggregateAsync<TSource>(this IAsyncEnumerable<TSource> source, Func<TSource, TSource, TSource> accumulator)
        {
            if (source == null)
                throw Error.ArgumentNull(nameof(source));
            if (accumulator == null)
                throw Error.ArgumentNull(nameof(accumulator));

            return AggregateCore(source, accumulator, CancellationToken.None);
        }

        public static Task<TSource> AggregateAsync<TSource>(this IAsyncEnumerable<TSource> source, Func<TSource, TSource, TSource> accumulator, CancellationToken cancellationToken)
        {
            if (source == null)
                throw Error.ArgumentNull(nameof(source));
            if (accumulator == null)
                throw Error.ArgumentNull(nameof(accumulator));

            return AggregateCore(source, accumulator, cancellationToken);
        }

        public static Task<TSource> AggregateAsync<TSource>(this IAsyncEnumerable<TSource> source, Func<TSource, TSource, ValueTask<TSource>> accumulator)
        {
            if (source == null)
                throw Error.ArgumentNull(nameof(source));
            if (accumulator == null)
                throw Error.ArgumentNull(nameof(accumulator));

            return AggregateCore(source, accumulator, CancellationToken.None);
        }

        public static Task<TSource> AggregateAsync<TSource>(this IAsyncEnumerable<TSource> source, Func<TSource, TSource, ValueTask<TSource>> accumulator, CancellationToken cancellationToken)
        {
            if (source == null)
                throw Error.ArgumentNull(nameof(source));
            if (accumulator == null)
                throw Error.ArgumentNull(nameof(accumulator));

            return AggregateCore(source, accumulator, cancellationToken);
        }

#if !NO_DEEP_CANCELLATION
        public static Task<TSource> AggregateAsync<TSource>(this IAsyncEnumerable<TSource> source, Func<TSource, TSource, CancellationToken, ValueTask<TSource>> accumulator, CancellationToken cancellationToken)
        {
            if (source == null)
                throw Error.ArgumentNull(nameof(source));
            if (accumulator == null)
                throw Error.ArgumentNull(nameof(accumulator));

            return AggregateCore(source, accumulator, cancellationToken);
        }
#endif

        public static Task<TAccumulate> AggregateAsync<TSource, TAccumulate>(this IAsyncEnumerable<TSource> source, TAccumulate seed, Func<TAccumulate, TSource, TAccumulate> accumulator)
        {
            if (source == null)
                throw Error.ArgumentNull(nameof(source));
            if (accumulator == null)
                throw Error.ArgumentNull(nameof(accumulator));

            return AggregateCore(source, seed, accumulator, x => x, CancellationToken.None);
        }

        public static Task<TAccumulate> AggregateAsync<TSource, TAccumulate>(this IAsyncEnumerable<TSource> source, TAccumulate seed, Func<TAccumulate, TSource, TAccumulate> accumulator, CancellationToken cancellationToken)
        {
            if (source == null)
                throw Error.ArgumentNull(nameof(source));
            if (accumulator == null)
                throw Error.ArgumentNull(nameof(accumulator));

            return AggregateCore(source, seed, accumulator, x => x, cancellationToken);
        }

        public static Task<TAccumulate> AggregateAsync<TSource, TAccumulate>(this IAsyncEnumerable<TSource> source, TAccumulate seed, Func<TAccumulate, TSource, ValueTask<TAccumulate>> accumulator)
        {
            if (source == null)
                throw Error.ArgumentNull(nameof(source));
            if (accumulator == null)
                throw Error.ArgumentNull(nameof(accumulator));

            return AggregateCore(source, seed, accumulator, CancellationToken.None);
        }

        public static Task<TAccumulate> AggregateAsync<TSource, TAccumulate>(this IAsyncEnumerable<TSource> source, TAccumulate seed, Func<TAccumulate, TSource, ValueTask<TAccumulate>> accumulator, CancellationToken cancellationToken)
        {
            if (source == null)
                throw Error.ArgumentNull(nameof(source));
            if (accumulator == null)
                throw Error.ArgumentNull(nameof(accumulator));

            return AggregateCore(source, seed, accumulator, cancellationToken);
        }

#if !NO_DEEP_CANCELLATION
        public static Task<TAccumulate> AggregateAsync<TSource, TAccumulate>(this IAsyncEnumerable<TSource> source, TAccumulate seed, Func<TAccumulate, TSource, CancellationToken, ValueTask<TAccumulate>> accumulator, CancellationToken cancellationToken)
        {
            if (source == null)
                throw Error.ArgumentNull(nameof(source));
            if (accumulator == null)
                throw Error.ArgumentNull(nameof(accumulator));

            return AggregateCore(source, seed, accumulator, cancellationToken);
        }
#endif

        public static Task<TResult> AggregateAsync<TSource, TAccumulate, TResult>(this IAsyncEnumerable<TSource> source, TAccumulate seed, Func<TAccumulate, TSource, TAccumulate> accumulator, Func<TAccumulate, TResult> resultSelector)
        {
            if (source == null)
                throw Error.ArgumentNull(nameof(source));
            if (accumulator == null)
                throw Error.ArgumentNull(nameof(accumulator));
            if (resultSelector == null)
                throw Error.ArgumentNull(nameof(resultSelector));

            return AggregateCore(source, seed, accumulator, resultSelector, CancellationToken.None);
        }

        public static Task<TResult> AggregateAsync<TSource, TAccumulate, TResult>(this IAsyncEnumerable<TSource> source, TAccumulate seed, Func<TAccumulate, TSource, TAccumulate> accumulator, Func<TAccumulate, TResult> resultSelector, CancellationToken cancellationToken)
        {
            if (source == null)
                throw Error.ArgumentNull(nameof(source));
            if (accumulator == null)
                throw Error.ArgumentNull(nameof(accumulator));
            if (resultSelector == null)
                throw Error.ArgumentNull(nameof(resultSelector));

            return AggregateCore(source, seed, accumulator, resultSelector, cancellationToken);
        }

        public static Task<TResult> AggregateAsync<TSource, TAccumulate, TResult>(this IAsyncEnumerable<TSource> source, TAccumulate seed, Func<TAccumulate, TSource, ValueTask<TAccumulate>> accumulator, Func<TAccumulate, ValueTask<TResult>> resultSelector)
        {
            if (source == null)
                throw Error.ArgumentNull(nameof(source));
            if (accumulator == null)
                throw Error.ArgumentNull(nameof(accumulator));
            if (resultSelector == null)
                throw Error.ArgumentNull(nameof(resultSelector));

            return AggregateCore(source, seed, accumulator, resultSelector, CancellationToken.None);
        }

        public static Task<TResult> AggregateAsync<TSource, TAccumulate, TResult>(this IAsyncEnumerable<TSource> source, TAccumulate seed, Func<TAccumulate, TSource, ValueTask<TAccumulate>> accumulator, Func<TAccumulate, ValueTask<TResult>> resultSelector, CancellationToken cancellationToken)
        {
            if (source == null)
                throw Error.ArgumentNull(nameof(source));
            if (accumulator == null)
                throw Error.ArgumentNull(nameof(accumulator));
            if (resultSelector == null)
                throw Error.ArgumentNull(nameof(resultSelector));

            return AggregateCore(source, seed, accumulator, resultSelector, cancellationToken);
        }

#if !NO_DEEP_CANCELLATION
        public static Task<TResult> AggregateAsync<TSource, TAccumulate, TResult>(this IAsyncEnumerable<TSource> source, TAccumulate seed, Func<TAccumulate, TSource, CancellationToken, ValueTask<TAccumulate>> accumulator, Func<TAccumulate, CancellationToken, ValueTask<TResult>> resultSelector, CancellationToken cancellationToken)
        {
            if (source == null)
                throw Error.ArgumentNull(nameof(source));
            if (accumulator == null)
                throw Error.ArgumentNull(nameof(accumulator));
            if (resultSelector == null)
                throw Error.ArgumentNull(nameof(resultSelector));

            return AggregateCore(source, seed, accumulator, resultSelector, cancellationToken);
        }
#endif

        private static async Task<TResult> AggregateCore<TSource, TAccumulate, TResult>(IAsyncEnumerable<TSource> source, TAccumulate seed, Func<TAccumulate, TSource, TAccumulate> accumulator, Func<TAccumulate, TResult> resultSelector, CancellationToken cancellationToken)
        {
            TAccumulate acc = seed;

            IAsyncEnumerator<TSource> e = source.GetAsyncEnumerator(cancellationToken);

            try
            {
                while (await e.MoveNextAsync().ConfigureAwait(false))
                {
                    acc = accumulator(acc, e.Current);
                }
            }
            finally
            {
                await e.DisposeAsync().ConfigureAwait(false);
            }

            return resultSelector(acc);
        }

        private static async Task<TSource> AggregateCore<TSource>(IAsyncEnumerable<TSource> source, Func<TSource, TSource, TSource> accumulator, CancellationToken cancellationToken)
        {
            IAsyncEnumerator<TSource> e = source.GetAsyncEnumerator(cancellationToken);

            try
            {
                if (!await e.MoveNextAsync().ConfigureAwait(false))
                {
                    throw Error.NoElements();
                }

                TSource acc = e.Current;

                while (await e.MoveNextAsync().ConfigureAwait(false))
                {
                    acc = accumulator(acc, e.Current);
                }

                return acc;
            }
            finally
            {
                await e.DisposeAsync().ConfigureAwait(false);
            }
        }

        private static async Task<TResult> AggregateCore<TSource, TResult>(IAsyncEnumerable<TSource> source, TResult seed, Func<TResult, TSource, ValueTask<TResult>> accumulator, CancellationToken cancellationToken)
        {
            TResult acc = seed;

            IAsyncEnumerator<TSource> e = source.GetAsyncEnumerator(cancellationToken);

            try
            {
                while (await e.MoveNextAsync().ConfigureAwait(false))
                {
                    acc = await accumulator(acc, e.Current).ConfigureAwait(false);
                }
            }
            finally
            {
                await e.DisposeAsync().ConfigureAwait(false);
            }

            return acc;
        }

#if !NO_DEEP_CANCELLATION
        private static async Task<TResult> AggregateCore<TSource, TResult>(IAsyncEnumerable<TSource> source, TResult seed, Func<TResult, TSource, CancellationToken, ValueTask<TResult>> accumulator, CancellationToken cancellationToken)
        {
            TResult acc = seed;

            IAsyncEnumerator<TSource> e = source.GetAsyncEnumerator(cancellationToken);

            try
            {
                while (await e.MoveNextAsync().ConfigureAwait(false))
                {
                    acc = await accumulator(acc, e.Current, cancellationToken).ConfigureAwait(false);
                }
            }
            finally
            {
                await e.DisposeAsync().ConfigureAwait(false);
            }

            return acc;
        }
#endif

        private static async Task<TResult> AggregateCore<TSource, TAccumulate, TResult>(IAsyncEnumerable<TSource> source, TAccumulate seed, Func<TAccumulate, TSource, ValueTask<TAccumulate>> accumulator, Func<TAccumulate, ValueTask<TResult>> resultSelector, CancellationToken cancellationToken)
        {
            TAccumulate acc = seed;

            IAsyncEnumerator<TSource> e = source.GetAsyncEnumerator(cancellationToken);

            try
            {
                while (await e.MoveNextAsync().ConfigureAwait(false))
                {
                    acc = await accumulator(acc, e.Current).ConfigureAwait(false);
                }
            }
            finally
            {
                await e.DisposeAsync().ConfigureAwait(false);
            }

            return await resultSelector(acc).ConfigureAwait(false);
        }

#if !NO_DEEP_CANCELLATION
        private static async Task<TResult> AggregateCore<TSource, TAccumulate, TResult>(IAsyncEnumerable<TSource> source, TAccumulate seed, Func<TAccumulate, TSource, CancellationToken, ValueTask<TAccumulate>> accumulator, Func<TAccumulate, CancellationToken, ValueTask<TResult>> resultSelector, CancellationToken cancellationToken)
        {
            TAccumulate acc = seed;

            IAsyncEnumerator<TSource> e = source.GetAsyncEnumerator(cancellationToken);

            try
            {
                while (await e.MoveNextAsync().ConfigureAwait(false))
                {
                    acc = await accumulator(acc, e.Current, cancellationToken).ConfigureAwait(false);
                }
            }
            finally
            {
                await e.DisposeAsync().ConfigureAwait(false);
            }

            return await resultSelector(acc, cancellationToken).ConfigureAwait(false);
        }
#endif

        private static async Task<TSource> AggregateCore<TSource>(IAsyncEnumerable<TSource> source, Func<TSource, TSource, ValueTask<TSource>> accumulator, CancellationToken cancellationToken)
        {
            IAsyncEnumerator<TSource> e = source.GetAsyncEnumerator(cancellationToken);

            try
            {
                if (!await e.MoveNextAsync().ConfigureAwait(false))
                {
                    throw Error.NoElements();
                }

                TSource acc = e.Current;

                while (await e.MoveNextAsync().ConfigureAwait(false))
                {
                    acc = await accumulator(acc, e.Current).ConfigureAwait(false);
                }

                return acc;
            }
            finally
            {
                await e.DisposeAsync().ConfigureAwait(false);
            }
        }

#if !NO_DEEP_CANCELLATION
        private static async Task<TSource> AggregateCore<TSource>(IAsyncEnumerable<TSource> source, Func<TSource, TSource, CancellationToken, ValueTask<TSource>> accumulator, CancellationToken cancellationToken)
        {
            IAsyncEnumerator<TSource> e = source.GetAsyncEnumerator(cancellationToken);

            try
            {
                if (!await e.MoveNextAsync().ConfigureAwait(false))
                {
                    throw Error.NoElements();
                }

                TSource acc = e.Current;

                while (await e.MoveNextAsync().ConfigureAwait(false))
                {
                    acc = await accumulator(acc, e.Current, cancellationToken).ConfigureAwait(false);
                }

                return acc;
            }
            finally
            {
                await e.DisposeAsync().ConfigureAwait(false);
            }
        }
#endif
    }
}

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
        public static Task<TSource> AggregateAsync<TSource>(this IAsyncEnumerable<TSource> source, Func<TSource, TSource, TSource> accumulator, CancellationToken cancellationToken = default)
        {
            if (source == null)
                throw Error.ArgumentNull(nameof(source));
            if (accumulator == null)
                throw Error.ArgumentNull(nameof(accumulator));

            return Core(source, accumulator, cancellationToken);

            static async Task<TSource> Core(IAsyncEnumerable<TSource> _source, Func<TSource, TSource, TSource> _accumulator, CancellationToken _cancellationToken)
            {
#if USE_AWAIT_USING
                await using (var e = _source.GetAsyncEnumerator(_cancellationToken).ConfigureAwait(false))
                {
                    if (!await e.MoveNextAsync())
                    {
                        throw Error.NoElements();
                    }

                    var acc = e.Current;

                    while (await e.MoveNextAsync())
                    {
                        acc = _accumulator(acc, e.Current);
                    }

                    return acc;
                }
#else
                var e = _source.GetAsyncEnumerator(_cancellationToken);

                try
                {
                    if (!await e.MoveNextAsync().ConfigureAwait(false))
                    {
                        throw Error.NoElements();
                    }

                    var acc = e.Current;

                    while (await e.MoveNextAsync().ConfigureAwait(false))
                    {
                        acc = _accumulator(acc, e.Current);
                    }

                    return acc;
                }
                finally
                {
                    await e.DisposeAsync().ConfigureAwait(false);
                }
#endif
            }
        }

        public static Task<TSource> AggregateAsync<TSource>(this IAsyncEnumerable<TSource> source, Func<TSource, TSource, ValueTask<TSource>> accumulator, CancellationToken cancellationToken = default)
        {
            if (source == null)
                throw Error.ArgumentNull(nameof(source));
            if (accumulator == null)
                throw Error.ArgumentNull(nameof(accumulator));

            return Core(source, accumulator, cancellationToken);

            static async Task<TSource> Core(IAsyncEnumerable<TSource> _source, Func<TSource, TSource, ValueTask<TSource>> _accumulator, CancellationToken _cancellationToken)
            {
#if USE_AWAIT_USING
                await using (var e = _source.GetAsyncEnumerator(_cancellationToken).ConfigureAwait(false))
                {
                    if (!await e.MoveNextAsync())
                    {
                        throw Error.NoElements();
                    }

                    var acc = e.Current;

                    while (await e.MoveNextAsync())
                    {
                        acc = await _accumulator(acc, e.Current).ConfigureAwait(false);
                    }

                    return acc;
                }
#else
                var e = _source.GetAsyncEnumerator(_cancellationToken);

                try
                {
                    if (!await e.MoveNextAsync().ConfigureAwait(false))
                    {
                        throw Error.NoElements();
                    }

                    var acc = e.Current;

                    while (await e.MoveNextAsync().ConfigureAwait(false))
                    {
                        acc = await _accumulator(acc, e.Current).ConfigureAwait(false);
                    }

                    return acc;
                }
                finally
                {
                    await e.DisposeAsync().ConfigureAwait(false);
                }
#endif
            }
        }

#if !NO_DEEP_CANCELLATION
        public static Task<TSource> AggregateAsync<TSource>(this IAsyncEnumerable<TSource> source, Func<TSource, TSource, CancellationToken, ValueTask<TSource>> accumulator, CancellationToken cancellationToken = default)
        {
            if (source == null)
                throw Error.ArgumentNull(nameof(source));
            if (accumulator == null)
                throw Error.ArgumentNull(nameof(accumulator));

            return Core(source, accumulator, cancellationToken);

            static async Task<TSource> Core(IAsyncEnumerable<TSource> _source, Func<TSource, TSource, CancellationToken, ValueTask<TSource>> _accumulator, CancellationToken _cancellationToken)
            {
#if USE_AWAIT_USING
                await using (var e = _source.GetAsyncEnumerator(_cancellationToken).ConfigureAwait(false))
                {
                    if (!await e.MoveNextAsync())
                    {
                        throw Error.NoElements();
                    }

                    var acc = e.Current;

                    while (await e.MoveNextAsync())
                    {
                        acc = await _accumulator(acc, e.Current, _cancellationToken).ConfigureAwait(false);
                    }

                    return acc;
                }
#else
                var e = _source.GetAsyncEnumerator(_cancellationToken);

                try
                {
                    if (!await e.MoveNextAsync().ConfigureAwait(false))
                    {
                        throw Error.NoElements();
                    }

                    var acc = e.Current;

                    while (await e.MoveNextAsync().ConfigureAwait(false))
                    {
                        acc = await _accumulator(acc, e.Current, _cancellationToken).ConfigureAwait(false);
                    }

                    return acc;
                }
                finally
                {
                    await e.DisposeAsync().ConfigureAwait(false);
                }
#endif
            }
        }
#endif

        public static Task<TAccumulate> AggregateAsync<TSource, TAccumulate>(this IAsyncEnumerable<TSource> source, TAccumulate seed, Func<TAccumulate, TSource, TAccumulate> accumulator, CancellationToken cancellationToken = default)
        {
            if (source == null)
                throw Error.ArgumentNull(nameof(source));
            if (accumulator == null)
                throw Error.ArgumentNull(nameof(accumulator));

            return Core(source, seed, accumulator, cancellationToken);

            static async Task<TAccumulate> Core(IAsyncEnumerable<TSource> _source, TAccumulate _seed, Func<TAccumulate, TSource, TAccumulate> _accumulator, CancellationToken _cancellationToken)
            {
                var acc = _seed;

#if USE_AWAIT_FOREACH
                await foreach (TSource item in _source.WithCancellation(_cancellationToken).ConfigureAwait(false))
                {
                    acc = _accumulator(acc, item);
                }
#else
                var e = _source.GetAsyncEnumerator(_cancellationToken);

                try
                {
                    while (await e.MoveNextAsync().ConfigureAwait(false))
                    {
                        acc = _accumulator(acc, e.Current);
                    }
                }
                finally
                {
                    await e.DisposeAsync().ConfigureAwait(false);
                }
#endif

                return acc;
            }
        }

        public static Task<TAccumulate> AggregateAsync<TSource, TAccumulate>(this IAsyncEnumerable<TSource> source, TAccumulate seed, Func<TAccumulate, TSource, ValueTask<TAccumulate>> accumulator, CancellationToken cancellationToken = default)
        {
            if (source == null)
                throw Error.ArgumentNull(nameof(source));
            if (accumulator == null)
                throw Error.ArgumentNull(nameof(accumulator));

            return Core(source, seed, accumulator, cancellationToken);

            static async Task<TAccumulate> Core(IAsyncEnumerable<TSource> _source, TAccumulate _seed, Func<TAccumulate, TSource, ValueTask<TAccumulate>> _accumulator, CancellationToken _cancellationToken)
            {
                var acc = _seed;

#if USE_AWAIT_FOREACH
                await foreach (TSource item in _source.WithCancellation(_cancellationToken).ConfigureAwait(false))
                {
                    acc = await _accumulator(acc, item).ConfigureAwait(false);
                }
#else
                var e = _source.GetAsyncEnumerator(_cancellationToken);

                try
                {
                    while (await e.MoveNextAsync().ConfigureAwait(false))
                    {
                        acc = await _accumulator(acc, e.Current).ConfigureAwait(false);
                    }
                }
                finally
                {
                    await e.DisposeAsync().ConfigureAwait(false);
                }
#endif

                return acc;
            }
        }

#if !NO_DEEP_CANCELLATION
        public static Task<TAccumulate> AggregateAsync<TSource, TAccumulate>(this IAsyncEnumerable<TSource> source, TAccumulate seed, Func<TAccumulate, TSource, CancellationToken, ValueTask<TAccumulate>> accumulator, CancellationToken cancellationToken = default)
        {
            if (source == null)
                throw Error.ArgumentNull(nameof(source));
            if (accumulator == null)
                throw Error.ArgumentNull(nameof(accumulator));

            return Core(source, seed, accumulator, cancellationToken);

            static async Task<TAccumulate> Core(IAsyncEnumerable<TSource> _source, TAccumulate _seed, Func<TAccumulate, TSource, CancellationToken, ValueTask<TAccumulate>> _accumulator, CancellationToken _cancellationToken)
            {
                var acc = _seed;

#if USE_AWAIT_FOREACH
                await foreach (TSource item in _source.WithCancellation(_cancellationToken).ConfigureAwait(false))
                {
                    acc = await _accumulator(acc, item, _cancellationToken).ConfigureAwait(false);
                }
#else
                var e = _source.GetAsyncEnumerator(_cancellationToken);

                try
                {
                    while (await e.MoveNextAsync().ConfigureAwait(false))
                    {
                        acc = await _accumulator(acc, e.Current, _cancellationToken).ConfigureAwait(false);
                    }
                }
                finally
                {
                    await e.DisposeAsync().ConfigureAwait(false);
                }
#endif

                return acc;
            }
        }
#endif

        public static Task<TResult> AggregateAsync<TSource, TAccumulate, TResult>(this IAsyncEnumerable<TSource> source, TAccumulate seed, Func<TAccumulate, TSource, TAccumulate> accumulator, Func<TAccumulate, TResult> resultSelector, CancellationToken cancellationToken = default)
        {
            if (source == null)
                throw Error.ArgumentNull(nameof(source));
            if (accumulator == null)
                throw Error.ArgumentNull(nameof(accumulator));
            if (resultSelector == null)
                throw Error.ArgumentNull(nameof(resultSelector));

            return Core(source, seed, accumulator, resultSelector, cancellationToken);

            static async Task<TResult> Core(IAsyncEnumerable<TSource> _source, TAccumulate _seed, Func<TAccumulate, TSource, TAccumulate> _accumulator, Func<TAccumulate, TResult> _resultSelector, CancellationToken _cancellationToken)
            {
                var acc = _seed;

#if USE_AWAIT_FOREACH
                await foreach (TSource item in _source.WithCancellation(_cancellationToken).ConfigureAwait(false))
                {
                    acc = _accumulator(acc, item);
                }
#else
                var e = _source.GetAsyncEnumerator(_cancellationToken);

                try
                {
                    while (await e.MoveNextAsync().ConfigureAwait(false))
                    {
                        acc = _accumulator(acc, e.Current);
                    }
                }
                finally
                {
                    await e.DisposeAsync().ConfigureAwait(false);
                }
#endif

                return _resultSelector(acc);
            }
        }

        public static Task<TResult> AggregateAsync<TSource, TAccumulate, TResult>(this IAsyncEnumerable<TSource> source, TAccumulate seed, Func<TAccumulate, TSource, ValueTask<TAccumulate>> accumulator, Func<TAccumulate, ValueTask<TResult>> resultSelector, CancellationToken cancellationToken = default)
        {
            if (source == null)
                throw Error.ArgumentNull(nameof(source));
            if (accumulator == null)
                throw Error.ArgumentNull(nameof(accumulator));
            if (resultSelector == null)
                throw Error.ArgumentNull(nameof(resultSelector));

            return Core(source, seed, accumulator, resultSelector, cancellationToken);

            static async Task<TResult> Core(IAsyncEnumerable<TSource> _source, TAccumulate _seed, Func<TAccumulate, TSource, ValueTask<TAccumulate>> _accumulator, Func<TAccumulate, ValueTask<TResult>> _resultSelector, CancellationToken _cancellationToken)
            {
                var acc = _seed;

#if USE_AWAIT_FOREACH
                await foreach (TSource item in _source.WithCancellation(_cancellationToken).ConfigureAwait(false))
                {
                    acc = await _accumulator(acc, item).ConfigureAwait(false);
                }
#else
                var e = _source.GetAsyncEnumerator(_cancellationToken);

                try
                {
                    while (await e.MoveNextAsync().ConfigureAwait(false))
                    {
                        acc = await _accumulator(acc, e.Current).ConfigureAwait(false);
                    }
                }
                finally
                {
                    await e.DisposeAsync().ConfigureAwait(false);
                }
#endif

                return await _resultSelector(acc).ConfigureAwait(false);
            }
        }

#if !NO_DEEP_CANCELLATION
        public static Task<TResult> AggregateAsync<TSource, TAccumulate, TResult>(this IAsyncEnumerable<TSource> source, TAccumulate seed, Func<TAccumulate, TSource, CancellationToken, ValueTask<TAccumulate>> accumulator, Func<TAccumulate, CancellationToken, ValueTask<TResult>> resultSelector, CancellationToken cancellationToken = default)
        {
            if (source == null)
                throw Error.ArgumentNull(nameof(source));
            if (accumulator == null)
                throw Error.ArgumentNull(nameof(accumulator));
            if (resultSelector == null)
                throw Error.ArgumentNull(nameof(resultSelector));

            return Core(source, seed, accumulator, resultSelector, cancellationToken);

            static async Task<TResult> Core(IAsyncEnumerable<TSource> _source, TAccumulate _seed, Func<TAccumulate, TSource, CancellationToken, ValueTask<TAccumulate>> _accumulator, Func<TAccumulate, CancellationToken, ValueTask<TResult>> _resultSelector, CancellationToken _cancellationToken)
            {
                var acc = _seed;

#if USE_AWAIT_FOREACH
                await foreach (TSource item in _source.WithCancellation(_cancellationToken).ConfigureAwait(false))
                {
                    acc = await _accumulator(acc, item, _cancellationToken).ConfigureAwait(false);
                }
#else
                var e = _source.GetAsyncEnumerator(_cancellationToken);

                try
                {
                    while (await e.MoveNextAsync().ConfigureAwait(false))
                    {
                        acc = await _accumulator(acc, e.Current, _cancellationToken).ConfigureAwait(false);
                    }
                }
                finally
                {
                    await e.DisposeAsync().ConfigureAwait(false);
                }
#endif

                return await _resultSelector(acc, _cancellationToken).ConfigureAwait(false);
            }
        }
#endif
    }
}

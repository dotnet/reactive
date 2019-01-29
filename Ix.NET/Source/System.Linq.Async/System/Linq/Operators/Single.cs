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
        public static Task<TSource> SingleAsync<TSource>(this IAsyncEnumerable<TSource> source, CancellationToken cancellationToken = default)
        {
            if (source == null)
                throw Error.ArgumentNull(nameof(source));

            return Core(source, cancellationToken);

            static async Task<TSource> Core(IAsyncEnumerable<TSource> _source, CancellationToken _cancellationToken)
            {
                if (_source is IList<TSource> list)
                {
                    switch (list.Count)
                    {
                        case 0: throw Error.NoElements();
                        case 1: return list[0];
                    }

                    throw Error.MoreThanOneElement();
                }

#if USE_AWAIT_USING
                await using (var e = _source.GetAsyncEnumerator(_cancellationToken).ConfigureAwait(false))
                {
                    if (!await e.MoveNextAsync())
                    {
                        throw Error.NoElements();
                    }

                    var result = e.Current;

                    if (await e.MoveNextAsync())
                    {
                        throw Error.MoreThanOneElement();
                    }

                    return result;
                }
#else
                var e = _source.GetAsyncEnumerator(_cancellationToken);

                try
                {
                    if (!await e.MoveNextAsync().ConfigureAwait(false))
                    {
                        throw Error.NoElements();
                    }

                    var result = e.Current;
                    if (await e.MoveNextAsync().ConfigureAwait(false))
                    {
                        throw Error.MoreThanOneElement();
                    }

                    return result;
                }
                finally
                {
                    await e.DisposeAsync().ConfigureAwait(false);
                }
#endif
            }
        }

        public static Task<TSource> SingleAsync<TSource>(this IAsyncEnumerable<TSource> source, Func<TSource, bool> predicate, CancellationToken cancellationToken = default)
        {
            if (source == null)
                throw Error.ArgumentNull(nameof(source));
            if (predicate == null)
                throw Error.ArgumentNull(nameof(predicate));

            return Core(source, predicate, cancellationToken);

            static async Task<TSource> Core(IAsyncEnumerable<TSource> _source, Func<TSource, bool> _predicate, CancellationToken _cancellationToken)
            {
#if USE_AWAIT_USING
                await using (var e = _source.GetAsyncEnumerator(_cancellationToken).ConfigureAwait(false))
                {
                    while (await e.MoveNextAsync())
                    {
                        var result = e.Current;

                        if (_predicate(result))
                        {
                            while (await e.MoveNextAsync())
                            {
                                if (_predicate(e.Current))
                                {
                                    throw Error.MoreThanOneElement();
                                }
                            }

                            return result;
                        }
                    }
                }
#else
                var e = _source.GetAsyncEnumerator(_cancellationToken);

                try
                {
                    while (await e.MoveNextAsync().ConfigureAwait(false))
                    {
                        var result = e.Current;

                        if (_predicate(result))
                        {
                            while (await e.MoveNextAsync().ConfigureAwait(false))
                            {
                                if (_predicate(e.Current))
                                {
                                    throw Error.MoreThanOneElement();
                                }
                            }

                            return result;
                        }
                    }
                }
                finally
                {
                    await e.DisposeAsync().ConfigureAwait(false);
                }
#endif

                throw Error.NoElements();
            }
        }

        public static Task<TSource> SingleAsync<TSource>(this IAsyncEnumerable<TSource> source, Func<TSource, ValueTask<bool>> predicate, CancellationToken cancellationToken = default)
        {
            if (source == null)
                throw Error.ArgumentNull(nameof(source));
            if (predicate == null)
                throw Error.ArgumentNull(nameof(predicate));

            return Core(source, predicate, cancellationToken);

            static async Task<TSource> Core(IAsyncEnumerable<TSource> _source, Func<TSource, ValueTask<bool>> _predicate, CancellationToken _cancellationToken)
            {
#if USE_AWAIT_USING
                await using (var e = _source.GetAsyncEnumerator(_cancellationToken).ConfigureAwait(false))
                {
                    while (await e.MoveNextAsync())
                    {
                        var result = e.Current;

                        if (await _predicate(result).ConfigureAwait(false))
                        {
                            while (await e.MoveNextAsync())
                            {
                                if (await _predicate(e.Current).ConfigureAwait(false))
                                {
                                    throw Error.MoreThanOneElement();
                                }
                            }

                            return result;
                        }
                    }
                }
#else
                var e = _source.GetAsyncEnumerator(_cancellationToken);

                try
                {
                    while (await e.MoveNextAsync().ConfigureAwait(false))
                    {
                        var result = e.Current;

                        if (await _predicate(result).ConfigureAwait(false))
                        {
                            while (await e.MoveNextAsync().ConfigureAwait(false))
                            {
                                if (await _predicate(e.Current).ConfigureAwait(false))
                                {
                                    throw Error.MoreThanOneElement();
                                }
                            }

                            return result;
                        }
                    }
                }
                finally
                {
                    await e.DisposeAsync().ConfigureAwait(false);
                }
#endif

                throw Error.NoElements();
            }
        }

#if !NO_DEEP_CANCELLATION
        public static Task<TSource> SingleAsync<TSource>(this IAsyncEnumerable<TSource> source, Func<TSource, CancellationToken, ValueTask<bool>> predicate, CancellationToken cancellationToken = default)
        {
            if (source == null)
                throw Error.ArgumentNull(nameof(source));
            if (predicate == null)
                throw Error.ArgumentNull(nameof(predicate));

            return Core(source, predicate, cancellationToken);

            static async Task<TSource> Core(IAsyncEnumerable<TSource> _source, Func<TSource, CancellationToken, ValueTask<bool>> _predicate, CancellationToken _cancellationToken)
            {
#if USE_AWAIT_USING
                await using (var e = _source.GetAsyncEnumerator(_cancellationToken).ConfigureAwait(false))
                {
                    while (await e.MoveNextAsync())
                    {
                        var result = e.Current;

                        if (await _predicate(result, _cancellationToken).ConfigureAwait(false))
                        {
                            while (await e.MoveNextAsync())
                            {
                                if (await _predicate(e.Current, _cancellationToken).ConfigureAwait(false))
                                {
                                    throw Error.MoreThanOneElement();
                                }
                            }

                            return result;
                        }
                    }
                }
#else
                var e = _source.GetAsyncEnumerator(_cancellationToken);

                try
                {
                    while (await e.MoveNextAsync().ConfigureAwait(false))
                    {
                        var result = e.Current;

                        if (await _predicate(result, _cancellationToken).ConfigureAwait(false))
                        {
                            while (await e.MoveNextAsync().ConfigureAwait(false))
                            {
                                if (await _predicate(e.Current, _cancellationToken).ConfigureAwait(false))
                                {
                                    throw Error.MoreThanOneElement();
                                }
                            }

                            return result;
                        }
                    }
                }
                finally
                {
                    await e.DisposeAsync().ConfigureAwait(false);
                }
#endif

                throw Error.NoElements();
            }
        }
#endif
    }
}

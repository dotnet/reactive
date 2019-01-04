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
        public static Task<TSource> MinAsync<TSource>(this IAsyncEnumerable<TSource> source, CancellationToken cancellationToken = default)
        {
            if (source == null)
                throw Error.ArgumentNull(nameof(source));

            var comparer = Comparer<TSource>.Default;
            if (default(TSource) == null)
            {
                return Core();

                async Task<TSource> Core()
                {
                    var value = default(TSource);

                    var e = source.GetAsyncEnumerator(cancellationToken);

                    try
                    {
                        do
                        {
                            if (!await e.MoveNextAsync().ConfigureAwait(false))
                            {
                                return value;
                            }

                            value = e.Current;
                        }
                        while (value == null);

                        while (await e.MoveNextAsync().ConfigureAwait(false))
                        {
                            var x = e.Current;
                            if (x != null && comparer.Compare(x, value) < 0)
                            {
                                value = x;
                            }
                        }
                    }
                    finally
                    {
                        await e.DisposeAsync().ConfigureAwait(false);
                    }

                    return value;
                }
            }
            else
            {
                return Core();

                async Task<TSource> Core()
                {
                    var value = default(TSource);

                    var e = source.GetAsyncEnumerator(cancellationToken);

                    try
                    {
                        if (!await e.MoveNextAsync().ConfigureAwait(false))
                        {
                            throw Error.NoElements();
                        }

                        value = e.Current;
                        while (await e.MoveNextAsync().ConfigureAwait(false))
                        {
                            var x = e.Current;
                            if (comparer.Compare(x, value) < 0)
                            {
                                value = x;
                            }
                        }
                    }
                    finally
                    {
                        await e.DisposeAsync().ConfigureAwait(false);
                    }

                    return value;
                }
            }
        }

        public static Task<TResult> MinAsync<TSource, TResult>(this IAsyncEnumerable<TSource> source, Func<TSource, TResult> selector, CancellationToken cancellationToken = default)
        {
            if (source == null)
                throw Error.ArgumentNull(nameof(source));
            if (selector == null)
                throw Error.ArgumentNull(nameof(selector));

            var comparer = Comparer<TResult>.Default;
            if (default(TResult) == null)
            {
                return Core();

                async Task<TResult> Core()
                {
                    var value = default(TResult);

                    var e = source.GetAsyncEnumerator(cancellationToken);

                    try
                    {
                        do
                        {
                            if (!await e.MoveNextAsync().ConfigureAwait(false))
                            {
                                return value;
                            }

                            value = selector(e.Current);
                        }
                        while (value == null);

                        while (await e.MoveNextAsync().ConfigureAwait(false))
                        {
                            var x = selector(e.Current);
                            if (x != null && comparer.Compare(x, value) < 0)
                            {
                                value = x;
                            }
                        }
                    }
                    finally
                    {
                        await e.DisposeAsync().ConfigureAwait(false);
                    }

                    return value;
                }
            }
            else
            {
                return Core();

                async Task<TResult> Core()
                {
                    var value = default(TResult);

                    var e = source.GetAsyncEnumerator(cancellationToken);

                    try
                    {
                        if (!await e.MoveNextAsync().ConfigureAwait(false))
                        {
                            throw Error.NoElements();
                        }

                        value = selector(e.Current);
                        while (await e.MoveNextAsync().ConfigureAwait(false))
                        {
                            var x = selector(e.Current);
                            if (comparer.Compare(x, value) < 0)
                            {
                                value = x;
                            }
                        }
                    }
                    finally
                    {
                        await e.DisposeAsync().ConfigureAwait(false);
                    }

                    return value;
                }
            }
        }

        public static Task<TResult> MinAsync<TSource, TResult>(this IAsyncEnumerable<TSource> source, Func<TSource, ValueTask<TResult>> selector, CancellationToken cancellationToken = default)
        {
            if (source == null)
                throw Error.ArgumentNull(nameof(source));
            if (selector == null)
                throw Error.ArgumentNull(nameof(selector));

            var comparer = Comparer<TResult>.Default;
            if (default(TResult) == null)
            {
                return Core();

                async Task<TResult> Core()
                {
                    var value = default(TResult);

                    var e = source.GetAsyncEnumerator(cancellationToken);

                    try
                    {
                        do
                        {
                            if (!await e.MoveNextAsync().ConfigureAwait(false))
                            {
                                return value;
                            }

                            value = await selector(e.Current).ConfigureAwait(false);
                        }
                        while (value == null);

                        while (await e.MoveNextAsync().ConfigureAwait(false))
                        {
                            var x = await selector(e.Current).ConfigureAwait(false);
                            if (x != null && comparer.Compare(x, value) < 0)
                            {
                                value = x;
                            }
                        }
                    }
                    finally
                    {
                        await e.DisposeAsync().ConfigureAwait(false);
                    }

                    return value;
                }
            }
            else
            {
                return Core();

                async Task<TResult> Core()
                {
                    var value = default(TResult);

                    var e = source.GetAsyncEnumerator(cancellationToken);

                    try
                    {
                        if (!await e.MoveNextAsync().ConfigureAwait(false))
                        {
                            throw Error.NoElements();
                        }

                        value = await selector(e.Current).ConfigureAwait(false);
                        while (await e.MoveNextAsync().ConfigureAwait(false))
                        {
                            var x = await selector(e.Current).ConfigureAwait(false);
                            if (comparer.Compare(x, value) < 0)
                            {
                                value = x;
                            }
                        }
                    }
                    finally
                    {
                        await e.DisposeAsync().ConfigureAwait(false);
                    }

                    return value;
                }
            }
        }

#if !NO_DEEP_CANCELLATION
        public static Task<TResult> MinAsync<TSource, TResult>(this IAsyncEnumerable<TSource> source, Func<TSource, CancellationToken, ValueTask<TResult>> selector, CancellationToken cancellationToken = default)
        {
            if (source == null)
                throw Error.ArgumentNull(nameof(source));
            if (selector == null)
                throw Error.ArgumentNull(nameof(selector));

            var comparer = Comparer<TResult>.Default;
            if (default(TResult) == null)
            {
                return Core();

                async Task<TResult> Core()
                {
                    var value = default(TResult);

                    var e = source.GetAsyncEnumerator(cancellationToken);

                    try
                    {
                        do
                        {
                            if (!await e.MoveNextAsync().ConfigureAwait(false))
                            {
                                return value;
                            }

                            value = await selector(e.Current, cancellationToken).ConfigureAwait(false);
                        }
                        while (value == null);

                        while (await e.MoveNextAsync().ConfigureAwait(false))
                        {
                            var x = await selector(e.Current, cancellationToken).ConfigureAwait(false);
                            if (x != null && comparer.Compare(x, value) < 0)
                            {
                                value = x;
                            }
                        }
                    }
                    finally
                    {
                        await e.DisposeAsync().ConfigureAwait(false);
                    }

                    return value;
                }
            }
            else
            {
                return Core();

                async Task<TResult> Core()
                {
                    var value = default(TResult);

                    var e = source.GetAsyncEnumerator(cancellationToken);

                    try
                    {
                        if (!await e.MoveNextAsync().ConfigureAwait(false))
                        {
                            throw Error.NoElements();
                        }

                        value = await selector(e.Current, cancellationToken).ConfigureAwait(false);
                        while (await e.MoveNextAsync().ConfigureAwait(false))
                        {
                            var x = await selector(e.Current, cancellationToken).ConfigureAwait(false);
                            if (comparer.Compare(x, value) < 0)
                            {
                                value = x;
                            }
                        }
                    }
                    finally
                    {
                        await e.DisposeAsync().ConfigureAwait(false);
                    }

                    return value;
                }
            }
        }
#endif
    }
}

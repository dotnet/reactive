// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace System.Linq
{
    public static partial class AsyncEnumerable
    {
        private static async Task<TSource> MinCore<TSource>(IAsyncEnumerable<TSource> source, CancellationToken cancellationToken)
        {
            var comparer = Comparer<TSource>.Default;
            var value = default(TSource);
            if (value == null)
            {
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
            }
            else
            {
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
            }

            return value;
        }

        private static async Task<TResult> MinCore<TSource, TResult>(IAsyncEnumerable<TSource> source, Func<TSource, TResult> selector, CancellationToken cancellationToken)
        {
            var comparer = Comparer<TResult>.Default;
            var value = default(TResult);
            if (value == null)
            {
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
            }
            else
            {
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
            }

            return value;
        }

        private static async Task<TResult> MinCore<TSource, TResult>(IAsyncEnumerable<TSource> source, Func<TSource, Task<TResult>> selector, CancellationToken cancellationToken)
        {
            var comparer = Comparer<TResult>.Default;
            var value = default(TResult);
            if (value == null)
            {
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
            }
            else
            {
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
            }

            return value;
        }
    }
}

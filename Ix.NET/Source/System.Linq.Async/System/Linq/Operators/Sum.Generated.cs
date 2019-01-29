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
        public static Task<int> SumAsync(this IAsyncEnumerable<int> source, CancellationToken cancellationToken = default)
        {
            if (source == null)
                throw Error.ArgumentNull(nameof(source));

            return Core(source, cancellationToken);

            async Task<int> Core(IAsyncEnumerable<int> _source, CancellationToken _cancellationToken)
            {
                var sum = 0;

#if CSHARP8
                await foreach (int value in _source.WithCancellation(_cancellationToken).ConfigureAwait(false))
                {
                    checked
                    {
                        sum += value;
                    }
                }
#else
                var e = _source.GetAsyncEnumerator(_cancellationToken);

                try
                {
                    while (await e.MoveNextAsync().ConfigureAwait(false))
                    {
                        checked
                        {
                            sum += e.Current;
                        }
                    }
                }
                finally
                {
                    await e.DisposeAsync().ConfigureAwait(false);
                }
#endif

                return sum;
            }
        }

        public static Task<int> SumAsync<TSource>(this IAsyncEnumerable<TSource> source, Func<TSource, int> selector, CancellationToken cancellationToken = default)
        {
            if (source == null)
                throw Error.ArgumentNull(nameof(source));
            if (selector == null)
                throw Error.ArgumentNull(nameof(selector));

            return Core(source, selector, cancellationToken);

            async Task<int> Core(IAsyncEnumerable<TSource> _source, Func<TSource, int> _selector, CancellationToken _cancellationToken)
            {
                var sum = 0;

#if CSHARP8
                await foreach (TSource item in _source.WithCancellation(_cancellationToken).ConfigureAwait(false))
                {
                    var value = _selector(item);

                    checked
                    {
                        sum += value;
                    }
                }
#else
                var e = _source.GetAsyncEnumerator(_cancellationToken);

                try
                {
                    while (await e.MoveNextAsync().ConfigureAwait(false))
                    {
                        var value = _selector(e.Current);

                        checked
                        {
                            sum += value;
                        }
                    }
                }
                finally
                {
                    await e.DisposeAsync().ConfigureAwait(false);
                }
#endif

                return sum;
            }
        }

        public static Task<int> SumAsync<TSource>(this IAsyncEnumerable<TSource> source, Func<TSource, ValueTask<int>> selector, CancellationToken cancellationToken = default)
        {
            if (source == null)
                throw Error.ArgumentNull(nameof(source));
            if (selector == null)
                throw Error.ArgumentNull(nameof(selector));

            return Core(source, selector, cancellationToken);

            async Task<int> Core(IAsyncEnumerable<TSource> _source, Func<TSource, ValueTask<int>> _selector, CancellationToken _cancellationToken)
            {
                var sum = 0;

#if CSHARP8
                await foreach (TSource item in _source.WithCancellation(_cancellationToken).ConfigureAwait(false))
                {
                    var value = await _selector(item).ConfigureAwait(false);

                    checked
                    {
                        sum += value;
                    }
                }
#else
                var e = _source.GetAsyncEnumerator(_cancellationToken);

                try
                {
                    while (await e.MoveNextAsync().ConfigureAwait(false))
                    {
                        var value = await _selector(e.Current).ConfigureAwait(false);

                        checked
                        {
                            sum += value;
                        }
                    }
                }
                finally
                {
                    await e.DisposeAsync().ConfigureAwait(false);
                }
#endif

                return sum;
            }
        }

#if !NO_DEEP_CANCELLATION
        public static Task<int> SumAsync<TSource>(this IAsyncEnumerable<TSource> source, Func<TSource, CancellationToken, ValueTask<int>> selector, CancellationToken cancellationToken = default)
        {
            if (source == null)
                throw Error.ArgumentNull(nameof(source));
            if (selector == null)
                throw Error.ArgumentNull(nameof(selector));

            return Core(source, selector, cancellationToken);

            async Task<int> Core(IAsyncEnumerable<TSource> _source, Func<TSource, CancellationToken, ValueTask<int>> _selector, CancellationToken _cancellationToken)
            {
                var sum = 0;

#if CSHARP8
                await foreach (TSource item in _source.WithCancellation(_cancellationToken).ConfigureAwait(false))
                {
                    var value = await _selector(item, _cancellationToken).ConfigureAwait(false);

                    checked
                    {
                        sum += value;
                    }
                }
#else
                var e = _source.GetAsyncEnumerator(_cancellationToken);

                try
                {
                    while (await e.MoveNextAsync().ConfigureAwait(false))
                    {
                        var value = await _selector(e.Current, _cancellationToken).ConfigureAwait(false);

                        checked
                        {
                            sum += value;
                        }
                    }
                }
                finally
                {
                    await e.DisposeAsync().ConfigureAwait(false);
                }
#endif

                return sum;
            }
        }
#endif

        public static Task<long> SumAsync(this IAsyncEnumerable<long> source, CancellationToken cancellationToken = default)
        {
            if (source == null)
                throw Error.ArgumentNull(nameof(source));

            return Core(source, cancellationToken);

            async Task<long> Core(IAsyncEnumerable<long> _source, CancellationToken _cancellationToken)
            {
                var sum = 0L;

#if CSHARP8
                await foreach (long value in _source.WithCancellation(_cancellationToken).ConfigureAwait(false))
                {
                    checked
                    {
                        sum += value;
                    }
                }
#else
                var e = _source.GetAsyncEnumerator(_cancellationToken);

                try
                {
                    while (await e.MoveNextAsync().ConfigureAwait(false))
                    {
                        checked
                        {
                            sum += e.Current;
                        }
                    }
                }
                finally
                {
                    await e.DisposeAsync().ConfigureAwait(false);
                }
#endif

                return sum;
            }
        }

        public static Task<long> SumAsync<TSource>(this IAsyncEnumerable<TSource> source, Func<TSource, long> selector, CancellationToken cancellationToken = default)
        {
            if (source == null)
                throw Error.ArgumentNull(nameof(source));
            if (selector == null)
                throw Error.ArgumentNull(nameof(selector));

            return Core(source, selector, cancellationToken);

            async Task<long> Core(IAsyncEnumerable<TSource> _source, Func<TSource, long> _selector, CancellationToken _cancellationToken)
            {
                var sum = 0L;

#if CSHARP8
                await foreach (TSource item in _source.WithCancellation(_cancellationToken).ConfigureAwait(false))
                {
                    var value = _selector(item);

                    checked
                    {
                        sum += value;
                    }
                }
#else
                var e = _source.GetAsyncEnumerator(_cancellationToken);

                try
                {
                    while (await e.MoveNextAsync().ConfigureAwait(false))
                    {
                        var value = _selector(e.Current);

                        checked
                        {
                            sum += value;
                        }
                    }
                }
                finally
                {
                    await e.DisposeAsync().ConfigureAwait(false);
                }
#endif

                return sum;
            }
        }

        public static Task<long> SumAsync<TSource>(this IAsyncEnumerable<TSource> source, Func<TSource, ValueTask<long>> selector, CancellationToken cancellationToken = default)
        {
            if (source == null)
                throw Error.ArgumentNull(nameof(source));
            if (selector == null)
                throw Error.ArgumentNull(nameof(selector));

            return Core(source, selector, cancellationToken);

            async Task<long> Core(IAsyncEnumerable<TSource> _source, Func<TSource, ValueTask<long>> _selector, CancellationToken _cancellationToken)
            {
                var sum = 0L;

#if CSHARP8
                await foreach (TSource item in _source.WithCancellation(_cancellationToken).ConfigureAwait(false))
                {
                    var value = await _selector(item).ConfigureAwait(false);

                    checked
                    {
                        sum += value;
                    }
                }
#else
                var e = _source.GetAsyncEnumerator(_cancellationToken);

                try
                {
                    while (await e.MoveNextAsync().ConfigureAwait(false))
                    {
                        var value = await _selector(e.Current).ConfigureAwait(false);

                        checked
                        {
                            sum += value;
                        }
                    }
                }
                finally
                {
                    await e.DisposeAsync().ConfigureAwait(false);
                }
#endif

                return sum;
            }
        }

#if !NO_DEEP_CANCELLATION
        public static Task<long> SumAsync<TSource>(this IAsyncEnumerable<TSource> source, Func<TSource, CancellationToken, ValueTask<long>> selector, CancellationToken cancellationToken = default)
        {
            if (source == null)
                throw Error.ArgumentNull(nameof(source));
            if (selector == null)
                throw Error.ArgumentNull(nameof(selector));

            return Core(source, selector, cancellationToken);

            async Task<long> Core(IAsyncEnumerable<TSource> _source, Func<TSource, CancellationToken, ValueTask<long>> _selector, CancellationToken _cancellationToken)
            {
                var sum = 0L;

#if CSHARP8
                await foreach (TSource item in _source.WithCancellation(_cancellationToken).ConfigureAwait(false))
                {
                    var value = await _selector(item, _cancellationToken).ConfigureAwait(false);

                    checked
                    {
                        sum += value;
                    }
                }
#else
                var e = _source.GetAsyncEnumerator(_cancellationToken);

                try
                {
                    while (await e.MoveNextAsync().ConfigureAwait(false))
                    {
                        var value = await _selector(e.Current, _cancellationToken).ConfigureAwait(false);

                        checked
                        {
                            sum += value;
                        }
                    }
                }
                finally
                {
                    await e.DisposeAsync().ConfigureAwait(false);
                }
#endif

                return sum;
            }
        }
#endif

        public static Task<float> SumAsync(this IAsyncEnumerable<float> source, CancellationToken cancellationToken = default)
        {
            if (source == null)
                throw Error.ArgumentNull(nameof(source));

            return Core(source, cancellationToken);

            async Task<float> Core(IAsyncEnumerable<float> _source, CancellationToken _cancellationToken)
            {
                var sum = 0.0f;

#if CSHARP8
                await foreach (float value in _source.WithCancellation(_cancellationToken).ConfigureAwait(false))
                {
                    sum += value;
                }
#else
                var e = _source.GetAsyncEnumerator(_cancellationToken);

                try
                {
                    while (await e.MoveNextAsync().ConfigureAwait(false))
                    {
                        sum += e.Current;
                    }
                }
                finally
                {
                    await e.DisposeAsync().ConfigureAwait(false);
                }
#endif

                return sum;
            }
        }

        public static Task<float> SumAsync<TSource>(this IAsyncEnumerable<TSource> source, Func<TSource, float> selector, CancellationToken cancellationToken = default)
        {
            if (source == null)
                throw Error.ArgumentNull(nameof(source));
            if (selector == null)
                throw Error.ArgumentNull(nameof(selector));

            return Core(source, selector, cancellationToken);

            async Task<float> Core(IAsyncEnumerable<TSource> _source, Func<TSource, float> _selector, CancellationToken _cancellationToken)
            {
                var sum = 0.0f;

#if CSHARP8
                await foreach (TSource item in _source.WithCancellation(_cancellationToken).ConfigureAwait(false))
                {
                    var value = _selector(item);

                    sum += value;
                }
#else
                var e = _source.GetAsyncEnumerator(_cancellationToken);

                try
                {
                    while (await e.MoveNextAsync().ConfigureAwait(false))
                    {
                        var value = _selector(e.Current);

                        sum += value;
                    }
                }
                finally
                {
                    await e.DisposeAsync().ConfigureAwait(false);
                }
#endif

                return sum;
            }
        }

        public static Task<float> SumAsync<TSource>(this IAsyncEnumerable<TSource> source, Func<TSource, ValueTask<float>> selector, CancellationToken cancellationToken = default)
        {
            if (source == null)
                throw Error.ArgumentNull(nameof(source));
            if (selector == null)
                throw Error.ArgumentNull(nameof(selector));

            return Core(source, selector, cancellationToken);

            async Task<float> Core(IAsyncEnumerable<TSource> _source, Func<TSource, ValueTask<float>> _selector, CancellationToken _cancellationToken)
            {
                var sum = 0.0f;

#if CSHARP8
                await foreach (TSource item in _source.WithCancellation(_cancellationToken).ConfigureAwait(false))
                {
                    var value = await _selector(item).ConfigureAwait(false);

                    sum += value;
                }
#else
                var e = _source.GetAsyncEnumerator(_cancellationToken);

                try
                {
                    while (await e.MoveNextAsync().ConfigureAwait(false))
                    {
                        var value = await _selector(e.Current).ConfigureAwait(false);

                        sum += value;
                    }
                }
                finally
                {
                    await e.DisposeAsync().ConfigureAwait(false);
                }
#endif

                return sum;
            }
        }

#if !NO_DEEP_CANCELLATION
        public static Task<float> SumAsync<TSource>(this IAsyncEnumerable<TSource> source, Func<TSource, CancellationToken, ValueTask<float>> selector, CancellationToken cancellationToken = default)
        {
            if (source == null)
                throw Error.ArgumentNull(nameof(source));
            if (selector == null)
                throw Error.ArgumentNull(nameof(selector));

            return Core(source, selector, cancellationToken);

            async Task<float> Core(IAsyncEnumerable<TSource> _source, Func<TSource, CancellationToken, ValueTask<float>> _selector, CancellationToken _cancellationToken)
            {
                var sum = 0.0f;

#if CSHARP8
                await foreach (TSource item in _source.WithCancellation(_cancellationToken).ConfigureAwait(false))
                {
                    var value = await _selector(item, _cancellationToken).ConfigureAwait(false);

                    sum += value;
                }
#else
                var e = _source.GetAsyncEnumerator(_cancellationToken);

                try
                {
                    while (await e.MoveNextAsync().ConfigureAwait(false))
                    {
                        var value = await _selector(e.Current, _cancellationToken).ConfigureAwait(false);

                        sum += value;
                    }
                }
                finally
                {
                    await e.DisposeAsync().ConfigureAwait(false);
                }
#endif

                return sum;
            }
        }
#endif

        public static Task<double> SumAsync(this IAsyncEnumerable<double> source, CancellationToken cancellationToken = default)
        {
            if (source == null)
                throw Error.ArgumentNull(nameof(source));

            return Core(source, cancellationToken);

            async Task<double> Core(IAsyncEnumerable<double> _source, CancellationToken _cancellationToken)
            {
                var sum = 0.0;

#if CSHARP8
                await foreach (double value in _source.WithCancellation(_cancellationToken).ConfigureAwait(false))
                {
                    sum += value;
                }
#else
                var e = _source.GetAsyncEnumerator(_cancellationToken);

                try
                {
                    while (await e.MoveNextAsync().ConfigureAwait(false))
                    {
                        sum += e.Current;
                    }
                }
                finally
                {
                    await e.DisposeAsync().ConfigureAwait(false);
                }
#endif

                return sum;
            }
        }

        public static Task<double> SumAsync<TSource>(this IAsyncEnumerable<TSource> source, Func<TSource, double> selector, CancellationToken cancellationToken = default)
        {
            if (source == null)
                throw Error.ArgumentNull(nameof(source));
            if (selector == null)
                throw Error.ArgumentNull(nameof(selector));

            return Core(source, selector, cancellationToken);

            async Task<double> Core(IAsyncEnumerable<TSource> _source, Func<TSource, double> _selector, CancellationToken _cancellationToken)
            {
                var sum = 0.0;

#if CSHARP8
                await foreach (TSource item in _source.WithCancellation(_cancellationToken).ConfigureAwait(false))
                {
                    var value = _selector(item);

                    sum += value;
                }
#else
                var e = _source.GetAsyncEnumerator(_cancellationToken);

                try
                {
                    while (await e.MoveNextAsync().ConfigureAwait(false))
                    {
                        var value = _selector(e.Current);

                        sum += value;
                    }
                }
                finally
                {
                    await e.DisposeAsync().ConfigureAwait(false);
                }
#endif

                return sum;
            }
        }

        public static Task<double> SumAsync<TSource>(this IAsyncEnumerable<TSource> source, Func<TSource, ValueTask<double>> selector, CancellationToken cancellationToken = default)
        {
            if (source == null)
                throw Error.ArgumentNull(nameof(source));
            if (selector == null)
                throw Error.ArgumentNull(nameof(selector));

            return Core(source, selector, cancellationToken);

            async Task<double> Core(IAsyncEnumerable<TSource> _source, Func<TSource, ValueTask<double>> _selector, CancellationToken _cancellationToken)
            {
                var sum = 0.0;

#if CSHARP8
                await foreach (TSource item in _source.WithCancellation(_cancellationToken).ConfigureAwait(false))
                {
                    var value = await _selector(item).ConfigureAwait(false);

                    sum += value;
                }
#else
                var e = _source.GetAsyncEnumerator(_cancellationToken);

                try
                {
                    while (await e.MoveNextAsync().ConfigureAwait(false))
                    {
                        var value = await _selector(e.Current).ConfigureAwait(false);

                        sum += value;
                    }
                }
                finally
                {
                    await e.DisposeAsync().ConfigureAwait(false);
                }
#endif

                return sum;
            }
        }

#if !NO_DEEP_CANCELLATION
        public static Task<double> SumAsync<TSource>(this IAsyncEnumerable<TSource> source, Func<TSource, CancellationToken, ValueTask<double>> selector, CancellationToken cancellationToken = default)
        {
            if (source == null)
                throw Error.ArgumentNull(nameof(source));
            if (selector == null)
                throw Error.ArgumentNull(nameof(selector));

            return Core(source, selector, cancellationToken);

            async Task<double> Core(IAsyncEnumerable<TSource> _source, Func<TSource, CancellationToken, ValueTask<double>> _selector, CancellationToken _cancellationToken)
            {
                var sum = 0.0;

#if CSHARP8
                await foreach (TSource item in _source.WithCancellation(_cancellationToken).ConfigureAwait(false))
                {
                    var value = await _selector(item, _cancellationToken).ConfigureAwait(false);

                    sum += value;
                }
#else
                var e = _source.GetAsyncEnumerator(_cancellationToken);

                try
                {
                    while (await e.MoveNextAsync().ConfigureAwait(false))
                    {
                        var value = await _selector(e.Current, _cancellationToken).ConfigureAwait(false);

                        sum += value;
                    }
                }
                finally
                {
                    await e.DisposeAsync().ConfigureAwait(false);
                }
#endif

                return sum;
            }
        }
#endif

        public static Task<decimal> SumAsync(this IAsyncEnumerable<decimal> source, CancellationToken cancellationToken = default)
        {
            if (source == null)
                throw Error.ArgumentNull(nameof(source));

            return Core(source, cancellationToken);

            async Task<decimal> Core(IAsyncEnumerable<decimal> _source, CancellationToken _cancellationToken)
            {
                var sum = 0m;

#if CSHARP8
                await foreach (decimal value in _source.WithCancellation(_cancellationToken).ConfigureAwait(false))
                {
                    sum += value;
                }
#else
                var e = _source.GetAsyncEnumerator(_cancellationToken);

                try
                {
                    while (await e.MoveNextAsync().ConfigureAwait(false))
                    {
                        sum += e.Current;
                    }
                }
                finally
                {
                    await e.DisposeAsync().ConfigureAwait(false);
                }
#endif

                return sum;
            }
        }

        public static Task<decimal> SumAsync<TSource>(this IAsyncEnumerable<TSource> source, Func<TSource, decimal> selector, CancellationToken cancellationToken = default)
        {
            if (source == null)
                throw Error.ArgumentNull(nameof(source));
            if (selector == null)
                throw Error.ArgumentNull(nameof(selector));

            return Core(source, selector, cancellationToken);

            async Task<decimal> Core(IAsyncEnumerable<TSource> _source, Func<TSource, decimal> _selector, CancellationToken _cancellationToken)
            {
                var sum = 0m;

#if CSHARP8
                await foreach (TSource item in _source.WithCancellation(_cancellationToken).ConfigureAwait(false))
                {
                    var value = _selector(item);

                    sum += value;
                }
#else
                var e = _source.GetAsyncEnumerator(_cancellationToken);

                try
                {
                    while (await e.MoveNextAsync().ConfigureAwait(false))
                    {
                        var value = _selector(e.Current);

                        sum += value;
                    }
                }
                finally
                {
                    await e.DisposeAsync().ConfigureAwait(false);
                }
#endif

                return sum;
            }
        }

        public static Task<decimal> SumAsync<TSource>(this IAsyncEnumerable<TSource> source, Func<TSource, ValueTask<decimal>> selector, CancellationToken cancellationToken = default)
        {
            if (source == null)
                throw Error.ArgumentNull(nameof(source));
            if (selector == null)
                throw Error.ArgumentNull(nameof(selector));

            return Core(source, selector, cancellationToken);

            async Task<decimal> Core(IAsyncEnumerable<TSource> _source, Func<TSource, ValueTask<decimal>> _selector, CancellationToken _cancellationToken)
            {
                var sum = 0m;

#if CSHARP8
                await foreach (TSource item in _source.WithCancellation(_cancellationToken).ConfigureAwait(false))
                {
                    var value = await _selector(item).ConfigureAwait(false);

                    sum += value;
                }
#else
                var e = _source.GetAsyncEnumerator(_cancellationToken);

                try
                {
                    while (await e.MoveNextAsync().ConfigureAwait(false))
                    {
                        var value = await _selector(e.Current).ConfigureAwait(false);

                        sum += value;
                    }
                }
                finally
                {
                    await e.DisposeAsync().ConfigureAwait(false);
                }
#endif

                return sum;
            }
        }

#if !NO_DEEP_CANCELLATION
        public static Task<decimal> SumAsync<TSource>(this IAsyncEnumerable<TSource> source, Func<TSource, CancellationToken, ValueTask<decimal>> selector, CancellationToken cancellationToken = default)
        {
            if (source == null)
                throw Error.ArgumentNull(nameof(source));
            if (selector == null)
                throw Error.ArgumentNull(nameof(selector));

            return Core(source, selector, cancellationToken);

            async Task<decimal> Core(IAsyncEnumerable<TSource> _source, Func<TSource, CancellationToken, ValueTask<decimal>> _selector, CancellationToken _cancellationToken)
            {
                var sum = 0m;

#if CSHARP8
                await foreach (TSource item in _source.WithCancellation(_cancellationToken).ConfigureAwait(false))
                {
                    var value = await _selector(item, _cancellationToken).ConfigureAwait(false);

                    sum += value;
                }
#else
                var e = _source.GetAsyncEnumerator(_cancellationToken);

                try
                {
                    while (await e.MoveNextAsync().ConfigureAwait(false))
                    {
                        var value = await _selector(e.Current, _cancellationToken).ConfigureAwait(false);

                        sum += value;
                    }
                }
                finally
                {
                    await e.DisposeAsync().ConfigureAwait(false);
                }
#endif

                return sum;
            }
        }
#endif

        public static Task<int?> SumAsync(this IAsyncEnumerable<int?> source, CancellationToken cancellationToken = default)
        {
            if (source == null)
                throw Error.ArgumentNull(nameof(source));

            return Core(source, cancellationToken);

            async Task<int?> Core(IAsyncEnumerable<int?> _source, CancellationToken _cancellationToken)
            {
                var sum = 0;

#if CSHARP8
                await foreach (int? value in _source.WithCancellation(_cancellationToken).ConfigureAwait(false))
                {
                    checked
                    {
                        sum += value.GetValueOrDefault();
                    }
                }
#else
                var e = _source.GetAsyncEnumerator(_cancellationToken);

                try
                {
                    while (await e.MoveNextAsync().ConfigureAwait(false))
                    {
                        checked
                        {
                            sum += e.Current.GetValueOrDefault();
                        }
                    }
                }
                finally
                {
                    await e.DisposeAsync().ConfigureAwait(false);
                }
#endif

                return sum;
            }
        }

        public static Task<int?> SumAsync<TSource>(this IAsyncEnumerable<TSource> source, Func<TSource, int?> selector, CancellationToken cancellationToken = default)
        {
            if (source == null)
                throw Error.ArgumentNull(nameof(source));
            if (selector == null)
                throw Error.ArgumentNull(nameof(selector));

            return Core(source, selector, cancellationToken);

            async Task<int?> Core(IAsyncEnumerable<TSource> _source, Func<TSource, int?> _selector, CancellationToken _cancellationToken)
            {
                var sum = 0;

#if CSHARP8
                await foreach (TSource item in _source.WithCancellation(_cancellationToken).ConfigureAwait(false))
                {
                    var value = _selector(item);

                    checked
                    {
                        sum += value.GetValueOrDefault();
                    }
                }
#else
                var e = _source.GetAsyncEnumerator(_cancellationToken);

                try
                {
                    while (await e.MoveNextAsync().ConfigureAwait(false))
                    {
                        var value = _selector(e.Current);

                        checked
                        {
                            sum += value.GetValueOrDefault();
                        }
                    }
                }
                finally
                {
                    await e.DisposeAsync().ConfigureAwait(false);
                }
#endif

                return sum;
            }
        }

        public static Task<int?> SumAsync<TSource>(this IAsyncEnumerable<TSource> source, Func<TSource, ValueTask<int?>> selector, CancellationToken cancellationToken = default)
        {
            if (source == null)
                throw Error.ArgumentNull(nameof(source));
            if (selector == null)
                throw Error.ArgumentNull(nameof(selector));

            return Core(source, selector, cancellationToken);

            async Task<int?> Core(IAsyncEnumerable<TSource> _source, Func<TSource, ValueTask<int?>> _selector, CancellationToken _cancellationToken)
            {
                var sum = 0;

#if CSHARP8
                await foreach (TSource item in _source.WithCancellation(_cancellationToken).ConfigureAwait(false))
                {
                    var value = await _selector(item).ConfigureAwait(false);

                    checked
                    {
                        sum += value.GetValueOrDefault();
                    }
                }
#else
                var e = _source.GetAsyncEnumerator(_cancellationToken);

                try
                {
                    while (await e.MoveNextAsync().ConfigureAwait(false))
                    {
                        var value = await _selector(e.Current).ConfigureAwait(false);

                        checked
                        {
                            sum += value.GetValueOrDefault();
                        }
                    }
                }
                finally
                {
                    await e.DisposeAsync().ConfigureAwait(false);
                }
#endif

                return sum;
            }
        }

#if !NO_DEEP_CANCELLATION
        public static Task<int?> SumAsync<TSource>(this IAsyncEnumerable<TSource> source, Func<TSource, CancellationToken, ValueTask<int?>> selector, CancellationToken cancellationToken = default)
        {
            if (source == null)
                throw Error.ArgumentNull(nameof(source));
            if (selector == null)
                throw Error.ArgumentNull(nameof(selector));

            return Core(source, selector, cancellationToken);

            async Task<int?> Core(IAsyncEnumerable<TSource> _source, Func<TSource, CancellationToken, ValueTask<int?>> _selector, CancellationToken _cancellationToken)
            {
                var sum = 0;

#if CSHARP8
                await foreach (TSource item in _source.WithCancellation(_cancellationToken).ConfigureAwait(false))
                {
                    var value = await _selector(item, _cancellationToken).ConfigureAwait(false);

                    checked
                    {
                        sum += value.GetValueOrDefault();
                    }
                }
#else
                var e = _source.GetAsyncEnumerator(_cancellationToken);

                try
                {
                    while (await e.MoveNextAsync().ConfigureAwait(false))
                    {
                        var value = await _selector(e.Current, _cancellationToken).ConfigureAwait(false);

                        checked
                        {
                            sum += value.GetValueOrDefault();
                        }
                    }
                }
                finally
                {
                    await e.DisposeAsync().ConfigureAwait(false);
                }
#endif

                return sum;
            }
        }
#endif

        public static Task<long?> SumAsync(this IAsyncEnumerable<long?> source, CancellationToken cancellationToken = default)
        {
            if (source == null)
                throw Error.ArgumentNull(nameof(source));

            return Core(source, cancellationToken);

            async Task<long?> Core(IAsyncEnumerable<long?> _source, CancellationToken _cancellationToken)
            {
                var sum = 0L;

#if CSHARP8
                await foreach (long? value in _source.WithCancellation(_cancellationToken).ConfigureAwait(false))
                {
                    checked
                    {
                        sum += value.GetValueOrDefault();
                    }
                }
#else
                var e = _source.GetAsyncEnumerator(_cancellationToken);

                try
                {
                    while (await e.MoveNextAsync().ConfigureAwait(false))
                    {
                        checked
                        {
                            sum += e.Current.GetValueOrDefault();
                        }
                    }
                }
                finally
                {
                    await e.DisposeAsync().ConfigureAwait(false);
                }
#endif

                return sum;
            }
        }

        public static Task<long?> SumAsync<TSource>(this IAsyncEnumerable<TSource> source, Func<TSource, long?> selector, CancellationToken cancellationToken = default)
        {
            if (source == null)
                throw Error.ArgumentNull(nameof(source));
            if (selector == null)
                throw Error.ArgumentNull(nameof(selector));

            return Core(source, selector, cancellationToken);

            async Task<long?> Core(IAsyncEnumerable<TSource> _source, Func<TSource, long?> _selector, CancellationToken _cancellationToken)
            {
                var sum = 0L;

#if CSHARP8
                await foreach (TSource item in _source.WithCancellation(_cancellationToken).ConfigureAwait(false))
                {
                    var value = _selector(item);

                    checked
                    {
                        sum += value.GetValueOrDefault();
                    }
                }
#else
                var e = _source.GetAsyncEnumerator(_cancellationToken);

                try
                {
                    while (await e.MoveNextAsync().ConfigureAwait(false))
                    {
                        var value = _selector(e.Current);

                        checked
                        {
                            sum += value.GetValueOrDefault();
                        }
                    }
                }
                finally
                {
                    await e.DisposeAsync().ConfigureAwait(false);
                }
#endif

                return sum;
            }
        }

        public static Task<long?> SumAsync<TSource>(this IAsyncEnumerable<TSource> source, Func<TSource, ValueTask<long?>> selector, CancellationToken cancellationToken = default)
        {
            if (source == null)
                throw Error.ArgumentNull(nameof(source));
            if (selector == null)
                throw Error.ArgumentNull(nameof(selector));

            return Core(source, selector, cancellationToken);

            async Task<long?> Core(IAsyncEnumerable<TSource> _source, Func<TSource, ValueTask<long?>> _selector, CancellationToken _cancellationToken)
            {
                var sum = 0L;

#if CSHARP8
                await foreach (TSource item in _source.WithCancellation(_cancellationToken).ConfigureAwait(false))
                {
                    var value = await _selector(item).ConfigureAwait(false);

                    checked
                    {
                        sum += value.GetValueOrDefault();
                    }
                }
#else
                var e = _source.GetAsyncEnumerator(_cancellationToken);

                try
                {
                    while (await e.MoveNextAsync().ConfigureAwait(false))
                    {
                        var value = await _selector(e.Current).ConfigureAwait(false);

                        checked
                        {
                            sum += value.GetValueOrDefault();
                        }
                    }
                }
                finally
                {
                    await e.DisposeAsync().ConfigureAwait(false);
                }
#endif

                return sum;
            }
        }

#if !NO_DEEP_CANCELLATION
        public static Task<long?> SumAsync<TSource>(this IAsyncEnumerable<TSource> source, Func<TSource, CancellationToken, ValueTask<long?>> selector, CancellationToken cancellationToken = default)
        {
            if (source == null)
                throw Error.ArgumentNull(nameof(source));
            if (selector == null)
                throw Error.ArgumentNull(nameof(selector));

            return Core(source, selector, cancellationToken);

            async Task<long?> Core(IAsyncEnumerable<TSource> _source, Func<TSource, CancellationToken, ValueTask<long?>> _selector, CancellationToken _cancellationToken)
            {
                var sum = 0L;

#if CSHARP8
                await foreach (TSource item in _source.WithCancellation(_cancellationToken).ConfigureAwait(false))
                {
                    var value = await _selector(item, _cancellationToken).ConfigureAwait(false);

                    checked
                    {
                        sum += value.GetValueOrDefault();
                    }
                }
#else
                var e = _source.GetAsyncEnumerator(_cancellationToken);

                try
                {
                    while (await e.MoveNextAsync().ConfigureAwait(false))
                    {
                        var value = await _selector(e.Current, _cancellationToken).ConfigureAwait(false);

                        checked
                        {
                            sum += value.GetValueOrDefault();
                        }
                    }
                }
                finally
                {
                    await e.DisposeAsync().ConfigureAwait(false);
                }
#endif

                return sum;
            }
        }
#endif

        public static Task<float?> SumAsync(this IAsyncEnumerable<float?> source, CancellationToken cancellationToken = default)
        {
            if (source == null)
                throw Error.ArgumentNull(nameof(source));

            return Core(source, cancellationToken);

            async Task<float?> Core(IAsyncEnumerable<float?> _source, CancellationToken _cancellationToken)
            {
                var sum = 0.0f;

#if CSHARP8
                await foreach (float? value in _source.WithCancellation(_cancellationToken).ConfigureAwait(false))
                {
                    sum += value.GetValueOrDefault();
                }
#else
                var e = _source.GetAsyncEnumerator(_cancellationToken);

                try
                {
                    while (await e.MoveNextAsync().ConfigureAwait(false))
                    {
                        sum += e.Current.GetValueOrDefault();
                    }
                }
                finally
                {
                    await e.DisposeAsync().ConfigureAwait(false);
                }
#endif

                return sum;
            }
        }

        public static Task<float?> SumAsync<TSource>(this IAsyncEnumerable<TSource> source, Func<TSource, float?> selector, CancellationToken cancellationToken = default)
        {
            if (source == null)
                throw Error.ArgumentNull(nameof(source));
            if (selector == null)
                throw Error.ArgumentNull(nameof(selector));

            return Core(source, selector, cancellationToken);

            async Task<float?> Core(IAsyncEnumerable<TSource> _source, Func<TSource, float?> _selector, CancellationToken _cancellationToken)
            {
                var sum = 0.0f;

#if CSHARP8
                await foreach (TSource item in _source.WithCancellation(_cancellationToken).ConfigureAwait(false))
                {
                    var value = _selector(item);

                    sum += value.GetValueOrDefault();
                }
#else
                var e = _source.GetAsyncEnumerator(_cancellationToken);

                try
                {
                    while (await e.MoveNextAsync().ConfigureAwait(false))
                    {
                        var value = _selector(e.Current);

                        sum += value.GetValueOrDefault();
                    }
                }
                finally
                {
                    await e.DisposeAsync().ConfigureAwait(false);
                }
#endif

                return sum;
            }
        }

        public static Task<float?> SumAsync<TSource>(this IAsyncEnumerable<TSource> source, Func<TSource, ValueTask<float?>> selector, CancellationToken cancellationToken = default)
        {
            if (source == null)
                throw Error.ArgumentNull(nameof(source));
            if (selector == null)
                throw Error.ArgumentNull(nameof(selector));

            return Core(source, selector, cancellationToken);

            async Task<float?> Core(IAsyncEnumerable<TSource> _source, Func<TSource, ValueTask<float?>> _selector, CancellationToken _cancellationToken)
            {
                var sum = 0.0f;

#if CSHARP8
                await foreach (TSource item in _source.WithCancellation(_cancellationToken).ConfigureAwait(false))
                {
                    var value = await _selector(item).ConfigureAwait(false);

                    sum += value.GetValueOrDefault();
                }
#else
                var e = _source.GetAsyncEnumerator(_cancellationToken);

                try
                {
                    while (await e.MoveNextAsync().ConfigureAwait(false))
                    {
                        var value = await _selector(e.Current).ConfigureAwait(false);

                        sum += value.GetValueOrDefault();
                    }
                }
                finally
                {
                    await e.DisposeAsync().ConfigureAwait(false);
                }
#endif

                return sum;
            }
        }

#if !NO_DEEP_CANCELLATION
        public static Task<float?> SumAsync<TSource>(this IAsyncEnumerable<TSource> source, Func<TSource, CancellationToken, ValueTask<float?>> selector, CancellationToken cancellationToken = default)
        {
            if (source == null)
                throw Error.ArgumentNull(nameof(source));
            if (selector == null)
                throw Error.ArgumentNull(nameof(selector));

            return Core(source, selector, cancellationToken);

            async Task<float?> Core(IAsyncEnumerable<TSource> _source, Func<TSource, CancellationToken, ValueTask<float?>> _selector, CancellationToken _cancellationToken)
            {
                var sum = 0.0f;

#if CSHARP8
                await foreach (TSource item in _source.WithCancellation(_cancellationToken).ConfigureAwait(false))
                {
                    var value = await _selector(item, _cancellationToken).ConfigureAwait(false);

                    sum += value.GetValueOrDefault();
                }
#else
                var e = _source.GetAsyncEnumerator(_cancellationToken);

                try
                {
                    while (await e.MoveNextAsync().ConfigureAwait(false))
                    {
                        var value = await _selector(e.Current, _cancellationToken).ConfigureAwait(false);

                        sum += value.GetValueOrDefault();
                    }
                }
                finally
                {
                    await e.DisposeAsync().ConfigureAwait(false);
                }
#endif

                return sum;
            }
        }
#endif

        public static Task<double?> SumAsync(this IAsyncEnumerable<double?> source, CancellationToken cancellationToken = default)
        {
            if (source == null)
                throw Error.ArgumentNull(nameof(source));

            return Core(source, cancellationToken);

            async Task<double?> Core(IAsyncEnumerable<double?> _source, CancellationToken _cancellationToken)
            {
                var sum = 0.0;

#if CSHARP8
                await foreach (double? value in _source.WithCancellation(_cancellationToken).ConfigureAwait(false))
                {
                    sum += value.GetValueOrDefault();
                }
#else
                var e = _source.GetAsyncEnumerator(_cancellationToken);

                try
                {
                    while (await e.MoveNextAsync().ConfigureAwait(false))
                    {
                        sum += e.Current.GetValueOrDefault();
                    }
                }
                finally
                {
                    await e.DisposeAsync().ConfigureAwait(false);
                }
#endif

                return sum;
            }
        }

        public static Task<double?> SumAsync<TSource>(this IAsyncEnumerable<TSource> source, Func<TSource, double?> selector, CancellationToken cancellationToken = default)
        {
            if (source == null)
                throw Error.ArgumentNull(nameof(source));
            if (selector == null)
                throw Error.ArgumentNull(nameof(selector));

            return Core(source, selector, cancellationToken);

            async Task<double?> Core(IAsyncEnumerable<TSource> _source, Func<TSource, double?> _selector, CancellationToken _cancellationToken)
            {
                var sum = 0.0;

#if CSHARP8
                await foreach (TSource item in _source.WithCancellation(_cancellationToken).ConfigureAwait(false))
                {
                    var value = _selector(item);

                    sum += value.GetValueOrDefault();
                }
#else
                var e = _source.GetAsyncEnumerator(_cancellationToken);

                try
                {
                    while (await e.MoveNextAsync().ConfigureAwait(false))
                    {
                        var value = _selector(e.Current);

                        sum += value.GetValueOrDefault();
                    }
                }
                finally
                {
                    await e.DisposeAsync().ConfigureAwait(false);
                }
#endif

                return sum;
            }
        }

        public static Task<double?> SumAsync<TSource>(this IAsyncEnumerable<TSource> source, Func<TSource, ValueTask<double?>> selector, CancellationToken cancellationToken = default)
        {
            if (source == null)
                throw Error.ArgumentNull(nameof(source));
            if (selector == null)
                throw Error.ArgumentNull(nameof(selector));

            return Core(source, selector, cancellationToken);

            async Task<double?> Core(IAsyncEnumerable<TSource> _source, Func<TSource, ValueTask<double?>> _selector, CancellationToken _cancellationToken)
            {
                var sum = 0.0;

#if CSHARP8
                await foreach (TSource item in _source.WithCancellation(_cancellationToken).ConfigureAwait(false))
                {
                    var value = await _selector(item).ConfigureAwait(false);

                    sum += value.GetValueOrDefault();
                }
#else
                var e = _source.GetAsyncEnumerator(_cancellationToken);

                try
                {
                    while (await e.MoveNextAsync().ConfigureAwait(false))
                    {
                        var value = await _selector(e.Current).ConfigureAwait(false);

                        sum += value.GetValueOrDefault();
                    }
                }
                finally
                {
                    await e.DisposeAsync().ConfigureAwait(false);
                }
#endif

                return sum;
            }
        }

#if !NO_DEEP_CANCELLATION
        public static Task<double?> SumAsync<TSource>(this IAsyncEnumerable<TSource> source, Func<TSource, CancellationToken, ValueTask<double?>> selector, CancellationToken cancellationToken = default)
        {
            if (source == null)
                throw Error.ArgumentNull(nameof(source));
            if (selector == null)
                throw Error.ArgumentNull(nameof(selector));

            return Core(source, selector, cancellationToken);

            async Task<double?> Core(IAsyncEnumerable<TSource> _source, Func<TSource, CancellationToken, ValueTask<double?>> _selector, CancellationToken _cancellationToken)
            {
                var sum = 0.0;

#if CSHARP8
                await foreach (TSource item in _source.WithCancellation(_cancellationToken).ConfigureAwait(false))
                {
                    var value = await _selector(item, _cancellationToken).ConfigureAwait(false);

                    sum += value.GetValueOrDefault();
                }
#else
                var e = _source.GetAsyncEnumerator(_cancellationToken);

                try
                {
                    while (await e.MoveNextAsync().ConfigureAwait(false))
                    {
                        var value = await _selector(e.Current, _cancellationToken).ConfigureAwait(false);

                        sum += value.GetValueOrDefault();
                    }
                }
                finally
                {
                    await e.DisposeAsync().ConfigureAwait(false);
                }
#endif

                return sum;
            }
        }
#endif

        public static Task<decimal?> SumAsync(this IAsyncEnumerable<decimal?> source, CancellationToken cancellationToken = default)
        {
            if (source == null)
                throw Error.ArgumentNull(nameof(source));

            return Core(source, cancellationToken);

            async Task<decimal?> Core(IAsyncEnumerable<decimal?> _source, CancellationToken _cancellationToken)
            {
                var sum = 0m;

#if CSHARP8
                await foreach (decimal? value in _source.WithCancellation(_cancellationToken).ConfigureAwait(false))
                {
                    sum += value.GetValueOrDefault();
                }
#else
                var e = _source.GetAsyncEnumerator(_cancellationToken);

                try
                {
                    while (await e.MoveNextAsync().ConfigureAwait(false))
                    {
                        sum += e.Current.GetValueOrDefault();
                    }
                }
                finally
                {
                    await e.DisposeAsync().ConfigureAwait(false);
                }
#endif

                return sum;
            }
        }

        public static Task<decimal?> SumAsync<TSource>(this IAsyncEnumerable<TSource> source, Func<TSource, decimal?> selector, CancellationToken cancellationToken = default)
        {
            if (source == null)
                throw Error.ArgumentNull(nameof(source));
            if (selector == null)
                throw Error.ArgumentNull(nameof(selector));

            return Core(source, selector, cancellationToken);

            async Task<decimal?> Core(IAsyncEnumerable<TSource> _source, Func<TSource, decimal?> _selector, CancellationToken _cancellationToken)
            {
                var sum = 0m;

#if CSHARP8
                await foreach (TSource item in _source.WithCancellation(_cancellationToken).ConfigureAwait(false))
                {
                    var value = _selector(item);

                    sum += value.GetValueOrDefault();
                }
#else
                var e = _source.GetAsyncEnumerator(_cancellationToken);

                try
                {
                    while (await e.MoveNextAsync().ConfigureAwait(false))
                    {
                        var value = _selector(e.Current);

                        sum += value.GetValueOrDefault();
                    }
                }
                finally
                {
                    await e.DisposeAsync().ConfigureAwait(false);
                }
#endif

                return sum;
            }
        }

        public static Task<decimal?> SumAsync<TSource>(this IAsyncEnumerable<TSource> source, Func<TSource, ValueTask<decimal?>> selector, CancellationToken cancellationToken = default)
        {
            if (source == null)
                throw Error.ArgumentNull(nameof(source));
            if (selector == null)
                throw Error.ArgumentNull(nameof(selector));

            return Core(source, selector, cancellationToken);

            async Task<decimal?> Core(IAsyncEnumerable<TSource> _source, Func<TSource, ValueTask<decimal?>> _selector, CancellationToken _cancellationToken)
            {
                var sum = 0m;

#if CSHARP8
                await foreach (TSource item in _source.WithCancellation(_cancellationToken).ConfigureAwait(false))
                {
                    var value = await _selector(item).ConfigureAwait(false);

                    sum += value.GetValueOrDefault();
                }
#else
                var e = _source.GetAsyncEnumerator(_cancellationToken);

                try
                {
                    while (await e.MoveNextAsync().ConfigureAwait(false))
                    {
                        var value = await _selector(e.Current).ConfigureAwait(false);

                        sum += value.GetValueOrDefault();
                    }
                }
                finally
                {
                    await e.DisposeAsync().ConfigureAwait(false);
                }
#endif

                return sum;
            }
        }

#if !NO_DEEP_CANCELLATION
        public static Task<decimal?> SumAsync<TSource>(this IAsyncEnumerable<TSource> source, Func<TSource, CancellationToken, ValueTask<decimal?>> selector, CancellationToken cancellationToken = default)
        {
            if (source == null)
                throw Error.ArgumentNull(nameof(source));
            if (selector == null)
                throw Error.ArgumentNull(nameof(selector));

            return Core(source, selector, cancellationToken);

            async Task<decimal?> Core(IAsyncEnumerable<TSource> _source, Func<TSource, CancellationToken, ValueTask<decimal?>> _selector, CancellationToken _cancellationToken)
            {
                var sum = 0m;

#if CSHARP8
                await foreach (TSource item in _source.WithCancellation(_cancellationToken).ConfigureAwait(false))
                {
                    var value = await _selector(item, _cancellationToken).ConfigureAwait(false);

                    sum += value.GetValueOrDefault();
                }
#else
                var e = _source.GetAsyncEnumerator(_cancellationToken);

                try
                {
                    while (await e.MoveNextAsync().ConfigureAwait(false))
                    {
                        var value = await _selector(e.Current, _cancellationToken).ConfigureAwait(false);

                        sum += value.GetValueOrDefault();
                    }
                }
                finally
                {
                    await e.DisposeAsync().ConfigureAwait(false);
                }
#endif

                return sum;
            }
        }
#endif

    }
}

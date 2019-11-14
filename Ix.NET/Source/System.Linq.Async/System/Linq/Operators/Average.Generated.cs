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
        public static ValueTask<double> AverageAsync(this IAsyncEnumerable<int> source, CancellationToken cancellationToken = default)
        {
            if (source == null)
                throw Error.ArgumentNull(nameof(source));

            return Core(source, cancellationToken);

            static async ValueTask<double> Core(IAsyncEnumerable<int> source, CancellationToken cancellationToken)
            {
                await using (var e = source.GetConfiguredAsyncEnumerator(cancellationToken, false))
                {
                    if (!await e.MoveNextAsync())
                    {
                        throw Error.NoElements();
                    }

                    long sum = e.Current;
                    long count = 1;
                    checked
                    {
                        while (await e.MoveNextAsync())
                        {
                            sum += e.Current;
                            ++count;
                        }
                    }

                    return (double)sum / count;
                }
            }
        }

        public static ValueTask<double> AverageAsync<TSource>(this IAsyncEnumerable<TSource> source, Func<TSource, int> selector, CancellationToken cancellationToken = default)
        {
            if (source == null)
                throw Error.ArgumentNull(nameof(source));
            if (selector == null)
                throw Error.ArgumentNull(nameof(selector));

            return Core(source, selector, cancellationToken);

            static async ValueTask<double> Core(IAsyncEnumerable<TSource> source, Func<TSource, int> selector, CancellationToken cancellationToken)
            {
                await using (var e = source.GetConfiguredAsyncEnumerator(cancellationToken, false))
                {
                    if (!await e.MoveNextAsync())
                    {
                        throw Error.NoElements();
                    }

                    long sum = selector(e.Current);
                    long count = 1;
                    checked
                    {
                        while (await e.MoveNextAsync())
                        {
                            sum += selector(e.Current);
                            ++count;
                        }
                    }

                    return (double)sum / count;
                }
            }
        }

        internal static ValueTask<double> AverageAwaitAsyncCore<TSource>(this IAsyncEnumerable<TSource> source, Func<TSource, ValueTask<int>> selector, CancellationToken cancellationToken = default)
        {
            if (source == null)
                throw Error.ArgumentNull(nameof(source));
            if (selector == null)
                throw Error.ArgumentNull(nameof(selector));

            return Core(source, selector, cancellationToken);

            static async ValueTask<double> Core(IAsyncEnumerable<TSource> source, Func<TSource, ValueTask<int>> selector, CancellationToken cancellationToken)
            {
                await using (var e = source.GetConfiguredAsyncEnumerator(cancellationToken, false))
                {
                    if (!await e.MoveNextAsync())
                    {
                        throw Error.NoElements();
                    }

                    long sum = await selector(e.Current).ConfigureAwait(false);
                    long count = 1;
                    checked
                    {
                        while (await e.MoveNextAsync())
                        {
                            sum += await selector(e.Current).ConfigureAwait(false);
                            ++count;
                        }
                    }

                    return (double)sum / count;
                }
            }
        }

#if !NO_DEEP_CANCELLATION
        internal static ValueTask<double> AverageAwaitWithCancellationAsyncCore<TSource>(this IAsyncEnumerable<TSource> source, Func<TSource, CancellationToken, ValueTask<int>> selector, CancellationToken cancellationToken = default)
        {
            if (source == null)
                throw Error.ArgumentNull(nameof(source));
            if (selector == null)
                throw Error.ArgumentNull(nameof(selector));

            return Core(source, selector, cancellationToken);

            static async ValueTask<double> Core(IAsyncEnumerable<TSource> source, Func<TSource, CancellationToken, ValueTask<int>> selector, CancellationToken cancellationToken)
            {
                await using (var e = source.GetConfiguredAsyncEnumerator(cancellationToken, false))
                {
                    if (!await e.MoveNextAsync())
                    {
                        throw Error.NoElements();
                    }

                    long sum = await selector(e.Current, cancellationToken).ConfigureAwait(false);
                    long count = 1;
                    checked
                    {
                        while (await e.MoveNextAsync())
                        {
                            sum += await selector(e.Current, cancellationToken).ConfigureAwait(false);
                            ++count;
                        }
                    }

                    return (double)sum / count;
                }
            }
        }
#endif

        public static ValueTask<double> AverageAsync(this IAsyncEnumerable<long> source, CancellationToken cancellationToken = default)
        {
            if (source == null)
                throw Error.ArgumentNull(nameof(source));

            return Core(source, cancellationToken);

            static async ValueTask<double> Core(IAsyncEnumerable<long> source, CancellationToken cancellationToken)
            {
                await using (var e = source.GetConfiguredAsyncEnumerator(cancellationToken, false))
                {
                    if (!await e.MoveNextAsync())
                    {
                        throw Error.NoElements();
                    }

                    long sum = e.Current;
                    long count = 1;
                    checked
                    {
                        while (await e.MoveNextAsync())
                        {
                            sum += e.Current;
                            ++count;
                        }
                    }

                    return (double)sum / count;
                }
            }
        }

        public static ValueTask<double> AverageAsync<TSource>(this IAsyncEnumerable<TSource> source, Func<TSource, long> selector, CancellationToken cancellationToken = default)
        {
            if (source == null)
                throw Error.ArgumentNull(nameof(source));
            if (selector == null)
                throw Error.ArgumentNull(nameof(selector));

            return Core(source, selector, cancellationToken);

            static async ValueTask<double> Core(IAsyncEnumerable<TSource> source, Func<TSource, long> selector, CancellationToken cancellationToken)
            {
                await using (var e = source.GetConfiguredAsyncEnumerator(cancellationToken, false))
                {
                    if (!await e.MoveNextAsync())
                    {
                        throw Error.NoElements();
                    }

                    long sum = selector(e.Current);
                    long count = 1;
                    checked
                    {
                        while (await e.MoveNextAsync())
                        {
                            sum += selector(e.Current);
                            ++count;
                        }
                    }

                    return (double)sum / count;
                }
            }
        }

        internal static ValueTask<double> AverageAwaitAsyncCore<TSource>(this IAsyncEnumerable<TSource> source, Func<TSource, ValueTask<long>> selector, CancellationToken cancellationToken = default)
        {
            if (source == null)
                throw Error.ArgumentNull(nameof(source));
            if (selector == null)
                throw Error.ArgumentNull(nameof(selector));

            return Core(source, selector, cancellationToken);

            static async ValueTask<double> Core(IAsyncEnumerable<TSource> source, Func<TSource, ValueTask<long>> selector, CancellationToken cancellationToken)
            {
                await using (var e = source.GetConfiguredAsyncEnumerator(cancellationToken, false))
                {
                    if (!await e.MoveNextAsync())
                    {
                        throw Error.NoElements();
                    }

                    long sum = await selector(e.Current).ConfigureAwait(false);
                    long count = 1;
                    checked
                    {
                        while (await e.MoveNextAsync())
                        {
                            sum += await selector(e.Current).ConfigureAwait(false);
                            ++count;
                        }
                    }

                    return (double)sum / count;
                }
            }
        }

#if !NO_DEEP_CANCELLATION
        internal static ValueTask<double> AverageAwaitWithCancellationAsyncCore<TSource>(this IAsyncEnumerable<TSource> source, Func<TSource, CancellationToken, ValueTask<long>> selector, CancellationToken cancellationToken = default)
        {
            if (source == null)
                throw Error.ArgumentNull(nameof(source));
            if (selector == null)
                throw Error.ArgumentNull(nameof(selector));

            return Core(source, selector, cancellationToken);

            static async ValueTask<double> Core(IAsyncEnumerable<TSource> source, Func<TSource, CancellationToken, ValueTask<long>> selector, CancellationToken cancellationToken)
            {
                await using (var e = source.GetConfiguredAsyncEnumerator(cancellationToken, false))
                {
                    if (!await e.MoveNextAsync())
                    {
                        throw Error.NoElements();
                    }

                    long sum = await selector(e.Current, cancellationToken).ConfigureAwait(false);
                    long count = 1;
                    checked
                    {
                        while (await e.MoveNextAsync())
                        {
                            sum += await selector(e.Current, cancellationToken).ConfigureAwait(false);
                            ++count;
                        }
                    }

                    return (double)sum / count;
                }
            }
        }
#endif

        public static ValueTask<float> AverageAsync(this IAsyncEnumerable<float> source, CancellationToken cancellationToken = default)
        {
            if (source == null)
                throw Error.ArgumentNull(nameof(source));

            return Core(source, cancellationToken);

            static async ValueTask<float> Core(IAsyncEnumerable<float> source, CancellationToken cancellationToken)
            {
                await using (var e = source.GetConfiguredAsyncEnumerator(cancellationToken, false))
                {
                    if (!await e.MoveNextAsync())
                    {
                        throw Error.NoElements();
                    }

                    double sum = e.Current;
                    long count = 1;
                    checked
                    {
                        while (await e.MoveNextAsync())
                        {
                            sum += e.Current;
                            ++count;
                        }
                    }

                    return (float)(sum / count);
                }
            }
        }

        public static ValueTask<float> AverageAsync<TSource>(this IAsyncEnumerable<TSource> source, Func<TSource, float> selector, CancellationToken cancellationToken = default)
        {
            if (source == null)
                throw Error.ArgumentNull(nameof(source));
            if (selector == null)
                throw Error.ArgumentNull(nameof(selector));

            return Core(source, selector, cancellationToken);

            static async ValueTask<float> Core(IAsyncEnumerable<TSource> source, Func<TSource, float> selector, CancellationToken cancellationToken)
            {
                await using (var e = source.GetConfiguredAsyncEnumerator(cancellationToken, false))
                {
                    if (!await e.MoveNextAsync())
                    {
                        throw Error.NoElements();
                    }

                    double sum = selector(e.Current);
                    long count = 1;
                    checked
                    {
                        while (await e.MoveNextAsync())
                        {
                            sum += selector(e.Current);
                            ++count;
                        }
                    }

                    return (float)(sum / count);
                }
            }
        }

        internal static ValueTask<float> AverageAwaitAsyncCore<TSource>(this IAsyncEnumerable<TSource> source, Func<TSource, ValueTask<float>> selector, CancellationToken cancellationToken = default)
        {
            if (source == null)
                throw Error.ArgumentNull(nameof(source));
            if (selector == null)
                throw Error.ArgumentNull(nameof(selector));

            return Core(source, selector, cancellationToken);

            static async ValueTask<float> Core(IAsyncEnumerable<TSource> source, Func<TSource, ValueTask<float>> selector, CancellationToken cancellationToken)
            {
                await using (var e = source.GetConfiguredAsyncEnumerator(cancellationToken, false))
                {
                    if (!await e.MoveNextAsync())
                    {
                        throw Error.NoElements();
                    }

                    double sum = await selector(e.Current).ConfigureAwait(false);
                    long count = 1;
                    checked
                    {
                        while (await e.MoveNextAsync())
                        {
                            sum += await selector(e.Current).ConfigureAwait(false);
                            ++count;
                        }
                    }

                    return (float)(sum / count);
                }
            }
        }

#if !NO_DEEP_CANCELLATION
        internal static ValueTask<float> AverageAwaitWithCancellationAsyncCore<TSource>(this IAsyncEnumerable<TSource> source, Func<TSource, CancellationToken, ValueTask<float>> selector, CancellationToken cancellationToken = default)
        {
            if (source == null)
                throw Error.ArgumentNull(nameof(source));
            if (selector == null)
                throw Error.ArgumentNull(nameof(selector));

            return Core(source, selector, cancellationToken);

            static async ValueTask<float> Core(IAsyncEnumerable<TSource> source, Func<TSource, CancellationToken, ValueTask<float>> selector, CancellationToken cancellationToken)
            {
                await using (var e = source.GetConfiguredAsyncEnumerator(cancellationToken, false))
                {
                    if (!await e.MoveNextAsync())
                    {
                        throw Error.NoElements();
                    }

                    double sum = await selector(e.Current, cancellationToken).ConfigureAwait(false);
                    long count = 1;
                    checked
                    {
                        while (await e.MoveNextAsync())
                        {
                            sum += await selector(e.Current, cancellationToken).ConfigureAwait(false);
                            ++count;
                        }
                    }

                    return (float)(sum / count);
                }
            }
        }
#endif

        public static ValueTask<double> AverageAsync(this IAsyncEnumerable<double> source, CancellationToken cancellationToken = default)
        {
            if (source == null)
                throw Error.ArgumentNull(nameof(source));

            return Core(source, cancellationToken);

            static async ValueTask<double> Core(IAsyncEnumerable<double> source, CancellationToken cancellationToken)
            {
                await using (var e = source.GetConfiguredAsyncEnumerator(cancellationToken, false))
                {
                    if (!await e.MoveNextAsync())
                    {
                        throw Error.NoElements();
                    }

                    double sum = e.Current;
                    long count = 1;
                    checked
                    {
                        while (await e.MoveNextAsync())
                        {
                            sum += e.Current;
                            ++count;
                        }
                    }

                    return sum / count;
                }
            }
        }

        public static ValueTask<double> AverageAsync<TSource>(this IAsyncEnumerable<TSource> source, Func<TSource, double> selector, CancellationToken cancellationToken = default)
        {
            if (source == null)
                throw Error.ArgumentNull(nameof(source));
            if (selector == null)
                throw Error.ArgumentNull(nameof(selector));

            return Core(source, selector, cancellationToken);

            static async ValueTask<double> Core(IAsyncEnumerable<TSource> source, Func<TSource, double> selector, CancellationToken cancellationToken)
            {
                await using (var e = source.GetConfiguredAsyncEnumerator(cancellationToken, false))
                {
                    if (!await e.MoveNextAsync())
                    {
                        throw Error.NoElements();
                    }

                    double sum = selector(e.Current);
                    long count = 1;
                    checked
                    {
                        while (await e.MoveNextAsync())
                        {
                            sum += selector(e.Current);
                            ++count;
                        }
                    }

                    return sum / count;
                }
            }
        }

        internal static ValueTask<double> AverageAwaitAsyncCore<TSource>(this IAsyncEnumerable<TSource> source, Func<TSource, ValueTask<double>> selector, CancellationToken cancellationToken = default)
        {
            if (source == null)
                throw Error.ArgumentNull(nameof(source));
            if (selector == null)
                throw Error.ArgumentNull(nameof(selector));

            return Core(source, selector, cancellationToken);

            static async ValueTask<double> Core(IAsyncEnumerable<TSource> source, Func<TSource, ValueTask<double>> selector, CancellationToken cancellationToken)
            {
                await using (var e = source.GetConfiguredAsyncEnumerator(cancellationToken, false))
                {
                    if (!await e.MoveNextAsync())
                    {
                        throw Error.NoElements();
                    }

                    double sum = await selector(e.Current).ConfigureAwait(false);
                    long count = 1;
                    checked
                    {
                        while (await e.MoveNextAsync())
                        {
                            sum += await selector(e.Current).ConfigureAwait(false);
                            ++count;
                        }
                    }

                    return sum / count;
                }
            }
        }

#if !NO_DEEP_CANCELLATION
        internal static ValueTask<double> AverageAwaitWithCancellationAsyncCore<TSource>(this IAsyncEnumerable<TSource> source, Func<TSource, CancellationToken, ValueTask<double>> selector, CancellationToken cancellationToken = default)
        {
            if (source == null)
                throw Error.ArgumentNull(nameof(source));
            if (selector == null)
                throw Error.ArgumentNull(nameof(selector));

            return Core(source, selector, cancellationToken);

            static async ValueTask<double> Core(IAsyncEnumerable<TSource> source, Func<TSource, CancellationToken, ValueTask<double>> selector, CancellationToken cancellationToken)
            {
                await using (var e = source.GetConfiguredAsyncEnumerator(cancellationToken, false))
                {
                    if (!await e.MoveNextAsync())
                    {
                        throw Error.NoElements();
                    }

                    double sum = await selector(e.Current, cancellationToken).ConfigureAwait(false);
                    long count = 1;
                    checked
                    {
                        while (await e.MoveNextAsync())
                        {
                            sum += await selector(e.Current, cancellationToken).ConfigureAwait(false);
                            ++count;
                        }
                    }

                    return sum / count;
                }
            }
        }
#endif

        public static ValueTask<decimal> AverageAsync(this IAsyncEnumerable<decimal> source, CancellationToken cancellationToken = default)
        {
            if (source == null)
                throw Error.ArgumentNull(nameof(source));

            return Core(source, cancellationToken);

            static async ValueTask<decimal> Core(IAsyncEnumerable<decimal> source, CancellationToken cancellationToken)
            {
                await using (var e = source.GetConfiguredAsyncEnumerator(cancellationToken, false))
                {
                    if (!await e.MoveNextAsync())
                    {
                        throw Error.NoElements();
                    }

                    decimal sum = e.Current;
                    long count = 1;
                    checked
                    {
                        while (await e.MoveNextAsync())
                        {
                            sum += e.Current;
                            ++count;
                        }
                    }

                    return sum / count;
                }
            }
        }

        public static ValueTask<decimal> AverageAsync<TSource>(this IAsyncEnumerable<TSource> source, Func<TSource, decimal> selector, CancellationToken cancellationToken = default)
        {
            if (source == null)
                throw Error.ArgumentNull(nameof(source));
            if (selector == null)
                throw Error.ArgumentNull(nameof(selector));

            return Core(source, selector, cancellationToken);

            static async ValueTask<decimal> Core(IAsyncEnumerable<TSource> source, Func<TSource, decimal> selector, CancellationToken cancellationToken)
            {
                await using (var e = source.GetConfiguredAsyncEnumerator(cancellationToken, false))
                {
                    if (!await e.MoveNextAsync())
                    {
                        throw Error.NoElements();
                    }

                    decimal sum = selector(e.Current);
                    long count = 1;
                    checked
                    {
                        while (await e.MoveNextAsync())
                        {
                            sum += selector(e.Current);
                            ++count;
                        }
                    }

                    return sum / count;
                }
            }
        }

        internal static ValueTask<decimal> AverageAwaitAsyncCore<TSource>(this IAsyncEnumerable<TSource> source, Func<TSource, ValueTask<decimal>> selector, CancellationToken cancellationToken = default)
        {
            if (source == null)
                throw Error.ArgumentNull(nameof(source));
            if (selector == null)
                throw Error.ArgumentNull(nameof(selector));

            return Core(source, selector, cancellationToken);

            static async ValueTask<decimal> Core(IAsyncEnumerable<TSource> source, Func<TSource, ValueTask<decimal>> selector, CancellationToken cancellationToken)
            {
                await using (var e = source.GetConfiguredAsyncEnumerator(cancellationToken, false))
                {
                    if (!await e.MoveNextAsync())
                    {
                        throw Error.NoElements();
                    }

                    decimal sum = await selector(e.Current).ConfigureAwait(false);
                    long count = 1;
                    checked
                    {
                        while (await e.MoveNextAsync())
                        {
                            sum += await selector(e.Current).ConfigureAwait(false);
                            ++count;
                        }
                    }

                    return sum / count;
                }
            }
        }

#if !NO_DEEP_CANCELLATION
        internal static ValueTask<decimal> AverageAwaitWithCancellationAsyncCore<TSource>(this IAsyncEnumerable<TSource> source, Func<TSource, CancellationToken, ValueTask<decimal>> selector, CancellationToken cancellationToken = default)
        {
            if (source == null)
                throw Error.ArgumentNull(nameof(source));
            if (selector == null)
                throw Error.ArgumentNull(nameof(selector));

            return Core(source, selector, cancellationToken);

            static async ValueTask<decimal> Core(IAsyncEnumerable<TSource> source, Func<TSource, CancellationToken, ValueTask<decimal>> selector, CancellationToken cancellationToken)
            {
                await using (var e = source.GetConfiguredAsyncEnumerator(cancellationToken, false))
                {
                    if (!await e.MoveNextAsync())
                    {
                        throw Error.NoElements();
                    }

                    decimal sum = await selector(e.Current, cancellationToken).ConfigureAwait(false);
                    long count = 1;
                    checked
                    {
                        while (await e.MoveNextAsync())
                        {
                            sum += await selector(e.Current, cancellationToken).ConfigureAwait(false);
                            ++count;
                        }
                    }

                    return sum / count;
                }
            }
        }
#endif

        public static ValueTask<double?> AverageAsync(this IAsyncEnumerable<int?> source, CancellationToken cancellationToken = default)
        {
            if (source == null)
                throw Error.ArgumentNull(nameof(source));

            return Core(source, cancellationToken);

            static async ValueTask<double?> Core(IAsyncEnumerable<int?> source, CancellationToken cancellationToken)
            {
                await using (var e = source.GetConfiguredAsyncEnumerator(cancellationToken, false))
                {
                    while (await e.MoveNextAsync())
                    {
                        var v = e.Current;
                        if (v.HasValue)
                        {
                            long sum = v.GetValueOrDefault();
                            long count = 1;
                            checked
                            {
                                while (await e.MoveNextAsync())
                                {
                                    v = e.Current;
                                    if (v.HasValue)
                                    {
                                        sum += v.GetValueOrDefault();
                                        ++count;
                                    }
                                }
                            }

                            return (double)sum / count;
                        }
                    }
                }

                return null;
            }
        }

        public static ValueTask<double?> AverageAsync<TSource>(this IAsyncEnumerable<TSource> source, Func<TSource, int?> selector, CancellationToken cancellationToken = default)
        {
            if (source == null)
                throw Error.ArgumentNull(nameof(source));
            if (selector == null)
                throw Error.ArgumentNull(nameof(selector));

            return Core(source, selector, cancellationToken);

            static async ValueTask<double?> Core(IAsyncEnumerable<TSource> source, Func<TSource, int?> selector, CancellationToken cancellationToken)
            {
                await using (var e = source.GetConfiguredAsyncEnumerator(cancellationToken, false))
                {
                    while (await e.MoveNextAsync())
                    {
                        var v = selector(e.Current);
                        if (v.HasValue)
                        {
                            long sum = v.GetValueOrDefault();
                            long count = 1;
                            checked
                            {
                                while (await e.MoveNextAsync())
                                {
                                    v = selector(e.Current);
                                    if (v.HasValue)
                                    {
                                        sum += v.GetValueOrDefault();
                                        ++count;
                                    }
                                }
                            }

                            return (double)sum / count;
                        }
                    }
                }

                return null;
            }
        }

        internal static ValueTask<double?> AverageAwaitAsyncCore<TSource>(this IAsyncEnumerable<TSource> source, Func<TSource, ValueTask<int?>> selector, CancellationToken cancellationToken = default)
        {
            if (source == null)
                throw Error.ArgumentNull(nameof(source));
            if (selector == null)
                throw Error.ArgumentNull(nameof(selector));

            return Core(source, selector, cancellationToken);

            static async ValueTask<double?> Core(IAsyncEnumerable<TSource> source, Func<TSource, ValueTask<int?>> selector, CancellationToken cancellationToken)
            {
                await using (var e = source.GetConfiguredAsyncEnumerator(cancellationToken, false))
                {
                    while (await e.MoveNextAsync())
                    {
                        var v = await selector(e.Current).ConfigureAwait(false);
                        if (v.HasValue)
                        {
                            long sum = v.GetValueOrDefault();
                            long count = 1;
                            checked
                            {
                                while (await e.MoveNextAsync())
                                {
                                    v = await selector(e.Current).ConfigureAwait(false);
                                    if (v.HasValue)
                                    {
                                        sum += v.GetValueOrDefault();
                                        ++count;
                                    }
                                }
                            }

                            return (double)sum / count;
                        }
                    }
                }

                return null;
            }
        }

#if !NO_DEEP_CANCELLATION
        internal static ValueTask<double?> AverageAwaitWithCancellationAsyncCore<TSource>(this IAsyncEnumerable<TSource> source, Func<TSource, CancellationToken, ValueTask<int?>> selector, CancellationToken cancellationToken = default)
        {
            if (source == null)
                throw Error.ArgumentNull(nameof(source));
            if (selector == null)
                throw Error.ArgumentNull(nameof(selector));

            return Core(source, selector, cancellationToken);

            static async ValueTask<double?> Core(IAsyncEnumerable<TSource> source, Func<TSource, CancellationToken, ValueTask<int?>> selector, CancellationToken cancellationToken)
            {
                await using (var e = source.GetConfiguredAsyncEnumerator(cancellationToken, false))
                {
                    while (await e.MoveNextAsync())
                    {
                        var v = await selector(e.Current, cancellationToken).ConfigureAwait(false);
                        if (v.HasValue)
                        {
                            long sum = v.GetValueOrDefault();
                            long count = 1;
                            checked
                            {
                                while (await e.MoveNextAsync())
                                {
                                    v = await selector(e.Current, cancellationToken).ConfigureAwait(false);
                                    if (v.HasValue)
                                    {
                                        sum += v.GetValueOrDefault();
                                        ++count;
                                    }
                                }
                            }

                            return (double)sum / count;
                        }
                    }
                }

                return null;
            }
        }
#endif

        public static ValueTask<double?> AverageAsync(this IAsyncEnumerable<long?> source, CancellationToken cancellationToken = default)
        {
            if (source == null)
                throw Error.ArgumentNull(nameof(source));

            return Core(source, cancellationToken);

            static async ValueTask<double?> Core(IAsyncEnumerable<long?> source, CancellationToken cancellationToken)
            {
                await using (var e = source.GetConfiguredAsyncEnumerator(cancellationToken, false))
                {
                    while (await e.MoveNextAsync())
                    {
                        var v = e.Current;
                        if (v.HasValue)
                        {
                            long sum = v.GetValueOrDefault();
                            long count = 1;
                            checked
                            {
                                while (await e.MoveNextAsync())
                                {
                                    v = e.Current;
                                    if (v.HasValue)
                                    {
                                        sum += v.GetValueOrDefault();
                                        ++count;
                                    }
                                }
                            }

                            return (double)sum / count;
                        }
                    }
                }

                return null;
            }
        }

        public static ValueTask<double?> AverageAsync<TSource>(this IAsyncEnumerable<TSource> source, Func<TSource, long?> selector, CancellationToken cancellationToken = default)
        {
            if (source == null)
                throw Error.ArgumentNull(nameof(source));
            if (selector == null)
                throw Error.ArgumentNull(nameof(selector));

            return Core(source, selector, cancellationToken);

            static async ValueTask<double?> Core(IAsyncEnumerable<TSource> source, Func<TSource, long?> selector, CancellationToken cancellationToken)
            {
                await using (var e = source.GetConfiguredAsyncEnumerator(cancellationToken, false))
                {
                    while (await e.MoveNextAsync())
                    {
                        var v = selector(e.Current);
                        if (v.HasValue)
                        {
                            long sum = v.GetValueOrDefault();
                            long count = 1;
                            checked
                            {
                                while (await e.MoveNextAsync())
                                {
                                    v = selector(e.Current);
                                    if (v.HasValue)
                                    {
                                        sum += v.GetValueOrDefault();
                                        ++count;
                                    }
                                }
                            }

                            return (double)sum / count;
                        }
                    }
                }

                return null;
            }
        }

        internal static ValueTask<double?> AverageAwaitAsyncCore<TSource>(this IAsyncEnumerable<TSource> source, Func<TSource, ValueTask<long?>> selector, CancellationToken cancellationToken = default)
        {
            if (source == null)
                throw Error.ArgumentNull(nameof(source));
            if (selector == null)
                throw Error.ArgumentNull(nameof(selector));

            return Core(source, selector, cancellationToken);

            static async ValueTask<double?> Core(IAsyncEnumerable<TSource> source, Func<TSource, ValueTask<long?>> selector, CancellationToken cancellationToken)
            {
                await using (var e = source.GetConfiguredAsyncEnumerator(cancellationToken, false))
                {
                    while (await e.MoveNextAsync())
                    {
                        var v = await selector(e.Current).ConfigureAwait(false);
                        if (v.HasValue)
                        {
                            long sum = v.GetValueOrDefault();
                            long count = 1;
                            checked
                            {
                                while (await e.MoveNextAsync())
                                {
                                    v = await selector(e.Current).ConfigureAwait(false);
                                    if (v.HasValue)
                                    {
                                        sum += v.GetValueOrDefault();
                                        ++count;
                                    }
                                }
                            }

                            return (double)sum / count;
                        }
                    }
                }

                return null;
            }
        }

#if !NO_DEEP_CANCELLATION
        internal static ValueTask<double?> AverageAwaitWithCancellationAsyncCore<TSource>(this IAsyncEnumerable<TSource> source, Func<TSource, CancellationToken, ValueTask<long?>> selector, CancellationToken cancellationToken = default)
        {
            if (source == null)
                throw Error.ArgumentNull(nameof(source));
            if (selector == null)
                throw Error.ArgumentNull(nameof(selector));

            return Core(source, selector, cancellationToken);

            static async ValueTask<double?> Core(IAsyncEnumerable<TSource> source, Func<TSource, CancellationToken, ValueTask<long?>> selector, CancellationToken cancellationToken)
            {
                await using (var e = source.GetConfiguredAsyncEnumerator(cancellationToken, false))
                {
                    while (await e.MoveNextAsync())
                    {
                        var v = await selector(e.Current, cancellationToken).ConfigureAwait(false);
                        if (v.HasValue)
                        {
                            long sum = v.GetValueOrDefault();
                            long count = 1;
                            checked
                            {
                                while (await e.MoveNextAsync())
                                {
                                    v = await selector(e.Current, cancellationToken).ConfigureAwait(false);
                                    if (v.HasValue)
                                    {
                                        sum += v.GetValueOrDefault();
                                        ++count;
                                    }
                                }
                            }

                            return (double)sum / count;
                        }
                    }
                }

                return null;
            }
        }
#endif

        public static ValueTask<float?> AverageAsync(this IAsyncEnumerable<float?> source, CancellationToken cancellationToken = default)
        {
            if (source == null)
                throw Error.ArgumentNull(nameof(source));

            return Core(source, cancellationToken);

            static async ValueTask<float?> Core(IAsyncEnumerable<float?> source, CancellationToken cancellationToken)
            {
                await using (var e = source.GetConfiguredAsyncEnumerator(cancellationToken, false))
                {
                    while (await e.MoveNextAsync())
                    {
                        var v = e.Current;
                        if (v.HasValue)
                        {
                            double sum = v.GetValueOrDefault();
                            long count = 1;
                            checked
                            {
                                while (await e.MoveNextAsync())
                                {
                                    v = e.Current;
                                    if (v.HasValue)
                                    {
                                        sum += v.GetValueOrDefault();
                                        ++count;
                                    }
                                }
                            }

                            return (float)(sum / count);
                        }
                    }
                }

                return null;
            }
        }

        public static ValueTask<float?> AverageAsync<TSource>(this IAsyncEnumerable<TSource> source, Func<TSource, float?> selector, CancellationToken cancellationToken = default)
        {
            if (source == null)
                throw Error.ArgumentNull(nameof(source));
            if (selector == null)
                throw Error.ArgumentNull(nameof(selector));

            return Core(source, selector, cancellationToken);

            static async ValueTask<float?> Core(IAsyncEnumerable<TSource> source, Func<TSource, float?> selector, CancellationToken cancellationToken)
            {
                await using (var e = source.GetConfiguredAsyncEnumerator(cancellationToken, false))
                {
                    while (await e.MoveNextAsync())
                    {
                        var v = selector(e.Current);
                        if (v.HasValue)
                        {
                            double sum = v.GetValueOrDefault();
                            long count = 1;
                            checked
                            {
                                while (await e.MoveNextAsync())
                                {
                                    v = selector(e.Current);
                                    if (v.HasValue)
                                    {
                                        sum += v.GetValueOrDefault();
                                        ++count;
                                    }
                                }
                            }

                            return (float)(sum / count);
                        }
                    }
                }

                return null;
            }
        }

        internal static ValueTask<float?> AverageAwaitAsyncCore<TSource>(this IAsyncEnumerable<TSource> source, Func<TSource, ValueTask<float?>> selector, CancellationToken cancellationToken = default)
        {
            if (source == null)
                throw Error.ArgumentNull(nameof(source));
            if (selector == null)
                throw Error.ArgumentNull(nameof(selector));

            return Core(source, selector, cancellationToken);

            static async ValueTask<float?> Core(IAsyncEnumerable<TSource> source, Func<TSource, ValueTask<float?>> selector, CancellationToken cancellationToken)
            {
                await using (var e = source.GetConfiguredAsyncEnumerator(cancellationToken, false))
                {
                    while (await e.MoveNextAsync())
                    {
                        var v = await selector(e.Current).ConfigureAwait(false);
                        if (v.HasValue)
                        {
                            double sum = v.GetValueOrDefault();
                            long count = 1;
                            checked
                            {
                                while (await e.MoveNextAsync())
                                {
                                    v = await selector(e.Current).ConfigureAwait(false);
                                    if (v.HasValue)
                                    {
                                        sum += v.GetValueOrDefault();
                                        ++count;
                                    }
                                }
                            }

                            return (float)(sum / count);
                        }
                    }
                }

                return null;
            }
        }

#if !NO_DEEP_CANCELLATION
        internal static ValueTask<float?> AverageAwaitWithCancellationAsyncCore<TSource>(this IAsyncEnumerable<TSource> source, Func<TSource, CancellationToken, ValueTask<float?>> selector, CancellationToken cancellationToken = default)
        {
            if (source == null)
                throw Error.ArgumentNull(nameof(source));
            if (selector == null)
                throw Error.ArgumentNull(nameof(selector));

            return Core(source, selector, cancellationToken);

            static async ValueTask<float?> Core(IAsyncEnumerable<TSource> source, Func<TSource, CancellationToken, ValueTask<float?>> selector, CancellationToken cancellationToken)
            {
                await using (var e = source.GetConfiguredAsyncEnumerator(cancellationToken, false))
                {
                    while (await e.MoveNextAsync())
                    {
                        var v = await selector(e.Current, cancellationToken).ConfigureAwait(false);
                        if (v.HasValue)
                        {
                            double sum = v.GetValueOrDefault();
                            long count = 1;
                            checked
                            {
                                while (await e.MoveNextAsync())
                                {
                                    v = await selector(e.Current, cancellationToken).ConfigureAwait(false);
                                    if (v.HasValue)
                                    {
                                        sum += v.GetValueOrDefault();
                                        ++count;
                                    }
                                }
                            }

                            return (float)(sum / count);
                        }
                    }
                }

                return null;
            }
        }
#endif

        public static ValueTask<double?> AverageAsync(this IAsyncEnumerable<double?> source, CancellationToken cancellationToken = default)
        {
            if (source == null)
                throw Error.ArgumentNull(nameof(source));

            return Core(source, cancellationToken);

            static async ValueTask<double?> Core(IAsyncEnumerable<double?> source, CancellationToken cancellationToken)
            {
                await using (var e = source.GetConfiguredAsyncEnumerator(cancellationToken, false))
                {
                    while (await e.MoveNextAsync())
                    {
                        var v = e.Current;
                        if (v.HasValue)
                        {
                            double sum = v.GetValueOrDefault();
                            long count = 1;
                            checked
                            {
                                while (await e.MoveNextAsync())
                                {
                                    v = e.Current;
                                    if (v.HasValue)
                                    {
                                        sum += v.GetValueOrDefault();
                                        ++count;
                                    }
                                }
                            }

                            return sum / count;
                        }
                    }
                }

                return null;
            }
        }

        public static ValueTask<double?> AverageAsync<TSource>(this IAsyncEnumerable<TSource> source, Func<TSource, double?> selector, CancellationToken cancellationToken = default)
        {
            if (source == null)
                throw Error.ArgumentNull(nameof(source));
            if (selector == null)
                throw Error.ArgumentNull(nameof(selector));

            return Core(source, selector, cancellationToken);

            static async ValueTask<double?> Core(IAsyncEnumerable<TSource> source, Func<TSource, double?> selector, CancellationToken cancellationToken)
            {
                await using (var e = source.GetConfiguredAsyncEnumerator(cancellationToken, false))
                {
                    while (await e.MoveNextAsync())
                    {
                        var v = selector(e.Current);
                        if (v.HasValue)
                        {
                            double sum = v.GetValueOrDefault();
                            long count = 1;
                            checked
                            {
                                while (await e.MoveNextAsync())
                                {
                                    v = selector(e.Current);
                                    if (v.HasValue)
                                    {
                                        sum += v.GetValueOrDefault();
                                        ++count;
                                    }
                                }
                            }

                            return sum / count;
                        }
                    }
                }

                return null;
            }
        }

        internal static ValueTask<double?> AverageAwaitAsyncCore<TSource>(this IAsyncEnumerable<TSource> source, Func<TSource, ValueTask<double?>> selector, CancellationToken cancellationToken = default)
        {
            if (source == null)
                throw Error.ArgumentNull(nameof(source));
            if (selector == null)
                throw Error.ArgumentNull(nameof(selector));

            return Core(source, selector, cancellationToken);

            static async ValueTask<double?> Core(IAsyncEnumerable<TSource> source, Func<TSource, ValueTask<double?>> selector, CancellationToken cancellationToken)
            {
                await using (var e = source.GetConfiguredAsyncEnumerator(cancellationToken, false))
                {
                    while (await e.MoveNextAsync())
                    {
                        var v = await selector(e.Current).ConfigureAwait(false);
                        if (v.HasValue)
                        {
                            double sum = v.GetValueOrDefault();
                            long count = 1;
                            checked
                            {
                                while (await e.MoveNextAsync())
                                {
                                    v = await selector(e.Current).ConfigureAwait(false);
                                    if (v.HasValue)
                                    {
                                        sum += v.GetValueOrDefault();
                                        ++count;
                                    }
                                }
                            }

                            return sum / count;
                        }
                    }
                }

                return null;
            }
        }

#if !NO_DEEP_CANCELLATION
        internal static ValueTask<double?> AverageAwaitWithCancellationAsyncCore<TSource>(this IAsyncEnumerable<TSource> source, Func<TSource, CancellationToken, ValueTask<double?>> selector, CancellationToken cancellationToken = default)
        {
            if (source == null)
                throw Error.ArgumentNull(nameof(source));
            if (selector == null)
                throw Error.ArgumentNull(nameof(selector));

            return Core(source, selector, cancellationToken);

            static async ValueTask<double?> Core(IAsyncEnumerable<TSource> source, Func<TSource, CancellationToken, ValueTask<double?>> selector, CancellationToken cancellationToken)
            {
                await using (var e = source.GetConfiguredAsyncEnumerator(cancellationToken, false))
                {
                    while (await e.MoveNextAsync())
                    {
                        var v = await selector(e.Current, cancellationToken).ConfigureAwait(false);
                        if (v.HasValue)
                        {
                            double sum = v.GetValueOrDefault();
                            long count = 1;
                            checked
                            {
                                while (await e.MoveNextAsync())
                                {
                                    v = await selector(e.Current, cancellationToken).ConfigureAwait(false);
                                    if (v.HasValue)
                                    {
                                        sum += v.GetValueOrDefault();
                                        ++count;
                                    }
                                }
                            }

                            return sum / count;
                        }
                    }
                }

                return null;
            }
        }
#endif

        public static ValueTask<decimal?> AverageAsync(this IAsyncEnumerable<decimal?> source, CancellationToken cancellationToken = default)
        {
            if (source == null)
                throw Error.ArgumentNull(nameof(source));

            return Core(source, cancellationToken);

            static async ValueTask<decimal?> Core(IAsyncEnumerable<decimal?> source, CancellationToken cancellationToken)
            {
                await using (var e = source.GetConfiguredAsyncEnumerator(cancellationToken, false))
                {
                    while (await e.MoveNextAsync())
                    {
                        var v = e.Current;
                        if (v.HasValue)
                        {
                            decimal sum = v.GetValueOrDefault();
                            long count = 1;
                            checked
                            {
                                while (await e.MoveNextAsync())
                                {
                                    v = e.Current;
                                    if (v.HasValue)
                                    {
                                        sum += v.GetValueOrDefault();
                                        ++count;
                                    }
                                }
                            }

                            return sum / count;
                        }
                    }
                }

                return null;
            }
        }

        public static ValueTask<decimal?> AverageAsync<TSource>(this IAsyncEnumerable<TSource> source, Func<TSource, decimal?> selector, CancellationToken cancellationToken = default)
        {
            if (source == null)
                throw Error.ArgumentNull(nameof(source));
            if (selector == null)
                throw Error.ArgumentNull(nameof(selector));

            return Core(source, selector, cancellationToken);

            static async ValueTask<decimal?> Core(IAsyncEnumerable<TSource> source, Func<TSource, decimal?> selector, CancellationToken cancellationToken)
            {
                await using (var e = source.GetConfiguredAsyncEnumerator(cancellationToken, false))
                {
                    while (await e.MoveNextAsync())
                    {
                        var v = selector(e.Current);
                        if (v.HasValue)
                        {
                            decimal sum = v.GetValueOrDefault();
                            long count = 1;
                            checked
                            {
                                while (await e.MoveNextAsync())
                                {
                                    v = selector(e.Current);
                                    if (v.HasValue)
                                    {
                                        sum += v.GetValueOrDefault();
                                        ++count;
                                    }
                                }
                            }

                            return sum / count;
                        }
                    }
                }

                return null;
            }
        }

        internal static ValueTask<decimal?> AverageAwaitAsyncCore<TSource>(this IAsyncEnumerable<TSource> source, Func<TSource, ValueTask<decimal?>> selector, CancellationToken cancellationToken = default)
        {
            if (source == null)
                throw Error.ArgumentNull(nameof(source));
            if (selector == null)
                throw Error.ArgumentNull(nameof(selector));

            return Core(source, selector, cancellationToken);

            static async ValueTask<decimal?> Core(IAsyncEnumerable<TSource> source, Func<TSource, ValueTask<decimal?>> selector, CancellationToken cancellationToken)
            {
                await using (var e = source.GetConfiguredAsyncEnumerator(cancellationToken, false))
                {
                    while (await e.MoveNextAsync())
                    {
                        var v = await selector(e.Current).ConfigureAwait(false);
                        if (v.HasValue)
                        {
                            decimal sum = v.GetValueOrDefault();
                            long count = 1;
                            checked
                            {
                                while (await e.MoveNextAsync())
                                {
                                    v = await selector(e.Current).ConfigureAwait(false);
                                    if (v.HasValue)
                                    {
                                        sum += v.GetValueOrDefault();
                                        ++count;
                                    }
                                }
                            }

                            return sum / count;
                        }
                    }
                }

                return null;
            }
        }

#if !NO_DEEP_CANCELLATION
        internal static ValueTask<decimal?> AverageAwaitWithCancellationAsyncCore<TSource>(this IAsyncEnumerable<TSource> source, Func<TSource, CancellationToken, ValueTask<decimal?>> selector, CancellationToken cancellationToken = default)
        {
            if (source == null)
                throw Error.ArgumentNull(nameof(source));
            if (selector == null)
                throw Error.ArgumentNull(nameof(selector));

            return Core(source, selector, cancellationToken);

            static async ValueTask<decimal?> Core(IAsyncEnumerable<TSource> source, Func<TSource, CancellationToken, ValueTask<decimal?>> selector, CancellationToken cancellationToken)
            {
                await using (var e = source.GetConfiguredAsyncEnumerator(cancellationToken, false))
                {
                    while (await e.MoveNextAsync())
                    {
                        var v = await selector(e.Current, cancellationToken).ConfigureAwait(false);
                        if (v.HasValue)
                        {
                            decimal sum = v.GetValueOrDefault();
                            long count = 1;
                            checked
                            {
                                while (await e.MoveNextAsync())
                                {
                                    v = await selector(e.Current, cancellationToken).ConfigureAwait(false);
                                    if (v.HasValue)
                                    {
                                        sum += v.GetValueOrDefault();
                                        ++count;
                                    }
                                }
                            }

                            return sum / count;
                        }
                    }
                }

                return null;
            }
        }
#endif

    }
}

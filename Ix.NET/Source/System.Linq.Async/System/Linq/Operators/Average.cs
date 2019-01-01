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
        private static async Task<double> AverageCore(this IAsyncEnumerable<int> source, CancellationToken cancellationToken)
        {
            var e = source.GetAsyncEnumerator(cancellationToken);

            try
            {
                if (!await e.MoveNextAsync().ConfigureAwait(false))
                {
                    throw Error.NoElements();
                }

                long sum = e.Current;
                long count = 1;
                checked
                {
                    while (await e.MoveNextAsync().ConfigureAwait(false))
                    {
                        sum += e.Current;
                        ++count;
                    }
                }

                return (double)sum / count;
            }
            finally
            {
                await e.DisposeAsync().ConfigureAwait(false);
            }
        }

        private static async Task<double> AverageCore<TSource>(this IAsyncEnumerable<TSource> source, Func<TSource, int> selector, CancellationToken cancellationToken)
        {
            var e = source.GetAsyncEnumerator(cancellationToken);

            try
            {
                if (!await e.MoveNextAsync().ConfigureAwait(false))
                {
                    throw Error.NoElements();
                }

                long sum = selector(e.Current);
                long count = 1;
                checked
                {
                    while (await e.MoveNextAsync().ConfigureAwait(false))
                    {
                        sum += selector(e.Current);
                        ++count;
                    }
                }

                return (double)sum / count;
            }
            finally
            {
                await e.DisposeAsync().ConfigureAwait(false);
            }
        }

        private static async Task<double> AverageCore<TSource>(this IAsyncEnumerable<TSource> source, Func<TSource, ValueTask<int>> selector, CancellationToken cancellationToken)
        {
            var e = source.GetAsyncEnumerator(cancellationToken);

            try
            {
                if (!await e.MoveNextAsync().ConfigureAwait(false))
                {
                    throw Error.NoElements();
                }

                long sum = await selector(e.Current).ConfigureAwait(false);
                long count = 1;
                checked
                {
                    while (await e.MoveNextAsync().ConfigureAwait(false))
                    {
                        sum += await selector(e.Current).ConfigureAwait(false);
                        ++count;
                    }
                }

                return (double)sum / count;
            }
            finally
            {
                await e.DisposeAsync().ConfigureAwait(false);
            }
        }

#if !NO_DEEP_CANCELLATION
        private static async Task<double> AverageCore<TSource>(this IAsyncEnumerable<TSource> source, Func<TSource, CancellationToken, ValueTask<int>> selector, CancellationToken cancellationToken)
        {
            var e = source.GetAsyncEnumerator(cancellationToken);

            try
            {
                if (!await e.MoveNextAsync().ConfigureAwait(false))
                {
                    throw Error.NoElements();
                }

                long sum = await selector(e.Current, cancellationToken).ConfigureAwait(false);
                long count = 1;
                checked
                {
                    while (await e.MoveNextAsync().ConfigureAwait(false))
                    {
                        sum += await selector(e.Current, cancellationToken).ConfigureAwait(false);
                        ++count;
                    }
                }

                return (double)sum / count;
            }
            finally
            {
                await e.DisposeAsync().ConfigureAwait(false);
            }
        }
#endif

        private static async Task<double?> AverageCore(IAsyncEnumerable<int?> source, CancellationToken cancellationToken)
        {
            var e = source.GetAsyncEnumerator(cancellationToken);

            try
            {
                while (await e.MoveNextAsync().ConfigureAwait(false))
                {
                    var v = e.Current;
                    if (v.HasValue)
                    {
                        long sum = v.GetValueOrDefault();
                        long count = 1;
                        checked
                        {
                            while (await e.MoveNextAsync().ConfigureAwait(false))
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
            finally
            {
                await e.DisposeAsync().ConfigureAwait(false);
            }

            return null;
        }

        private static async Task<double?> AverageCore<TSource>(IAsyncEnumerable<TSource> source, Func<TSource, int?> selector, CancellationToken cancellationToken)
        {
            var e = source.GetAsyncEnumerator(cancellationToken);

            try
            {
                while (await e.MoveNextAsync().ConfigureAwait(false))
                {
                    var v = selector(e.Current);
                    if (v.HasValue)
                    {
                        long sum = v.GetValueOrDefault();
                        long count = 1;
                        checked
                        {
                            while (await e.MoveNextAsync().ConfigureAwait(false))
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
            finally
            {
                await e.DisposeAsync().ConfigureAwait(false);
            }

            return null;
        }

        private static async Task<double?> AverageCore<TSource>(IAsyncEnumerable<TSource> source, Func<TSource, ValueTask<int?>> selector, CancellationToken cancellationToken)
        {
            var e = source.GetAsyncEnumerator(cancellationToken);

            try
            {
                while (await e.MoveNextAsync().ConfigureAwait(false))
                {
                    var v = await selector(e.Current).ConfigureAwait(false);
                    if (v.HasValue)
                    {
                        long sum = v.GetValueOrDefault();
                        long count = 1;
                        checked
                        {
                            while (await e.MoveNextAsync().ConfigureAwait(false))
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
            finally
            {
                await e.DisposeAsync().ConfigureAwait(false);
            }

            return null;
        }

#if !NO_DEEP_CANCELLATION
        private static async Task<double?> AverageCore<TSource>(IAsyncEnumerable<TSource> source, Func<TSource, CancellationToken, ValueTask<int?>> selector, CancellationToken cancellationToken)
        {
            var e = source.GetAsyncEnumerator(cancellationToken);

            try
            {
                while (await e.MoveNextAsync().ConfigureAwait(false))
                {
                    var v = await selector(e.Current, cancellationToken).ConfigureAwait(false);
                    if (v.HasValue)
                    {
                        long sum = v.GetValueOrDefault();
                        long count = 1;
                        checked
                        {
                            while (await e.MoveNextAsync().ConfigureAwait(false))
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
            finally
            {
                await e.DisposeAsync().ConfigureAwait(false);
            }

            return null;
        }
#endif

        private static async Task<double> AverageCore(IAsyncEnumerable<long> source, CancellationToken cancellationToken)
        {
            var e = source.GetAsyncEnumerator(cancellationToken);

            try
            {
                if (!await e.MoveNextAsync().ConfigureAwait(false))
                {
                    throw Error.NoElements();
                }

                var sum = e.Current;
                long count = 1;
                checked
                {
                    while (await e.MoveNextAsync().ConfigureAwait(false))
                    {
                        sum += e.Current;
                        ++count;
                    }
                }

                return (double)sum / count;
            }
            finally
            {
                await e.DisposeAsync().ConfigureAwait(false);
            }
        }

        private static async Task<double> AverageCore<TSource>(IAsyncEnumerable<TSource> source, Func<TSource, long> selector, CancellationToken cancellationToken)
        {
            var e = source.GetAsyncEnumerator(cancellationToken);

            try
            {
                if (!await e.MoveNextAsync().ConfigureAwait(false))
                {
                    throw Error.NoElements();
                }

                var sum = selector(e.Current);
                long count = 1;
                checked
                {
                    while (await e.MoveNextAsync().ConfigureAwait(false))
                    {
                        sum += selector(e.Current);
                        ++count;
                    }
                }

                return (double)sum / count;
            }
            finally
            {
                await e.DisposeAsync().ConfigureAwait(false);
            }
        }

        private static async Task<double> AverageCore<TSource>(IAsyncEnumerable<TSource> source, Func<TSource, ValueTask<long>> selector, CancellationToken cancellationToken)
        {
            var e = source.GetAsyncEnumerator(cancellationToken);

            try
            {
                if (!await e.MoveNextAsync().ConfigureAwait(false))
                {
                    throw Error.NoElements();
                }

                var sum = await selector(e.Current).ConfigureAwait(false);
                long count = 1;
                checked
                {
                    while (await e.MoveNextAsync().ConfigureAwait(false))
                    {
                        sum += await selector(e.Current).ConfigureAwait(false);
                        ++count;
                    }
                }

                return (double)sum / count;
            }
            finally
            {
                await e.DisposeAsync().ConfigureAwait(false);
            }
        }

#if !NO_DEEP_CANCELLATION
        private static async Task<double> AverageCore<TSource>(IAsyncEnumerable<TSource> source, Func<TSource, CancellationToken, ValueTask<long>> selector, CancellationToken cancellationToken)
        {
            var e = source.GetAsyncEnumerator(cancellationToken);

            try
            {
                if (!await e.MoveNextAsync().ConfigureAwait(false))
                {
                    throw Error.NoElements();
                }

                var sum = await selector(e.Current, cancellationToken).ConfigureAwait(false);
                long count = 1;
                checked
                {
                    while (await e.MoveNextAsync().ConfigureAwait(false))
                    {
                        sum += await selector(e.Current, cancellationToken).ConfigureAwait(false);
                        ++count;
                    }
                }

                return (double)sum / count;
            }
            finally
            {
                await e.DisposeAsync().ConfigureAwait(false);
            }
        }
#endif

        private static async Task<double?> AverageCore(IAsyncEnumerable<long?> source, CancellationToken cancellationToken)
        {
            var e = source.GetAsyncEnumerator(cancellationToken);

            try
            {
                while (await e.MoveNextAsync().ConfigureAwait(false))
                {
                    var v = e.Current;
                    if (v.HasValue)
                    {
                        var sum = v.GetValueOrDefault();
                        long count = 1;
                        checked
                        {
                            while (await e.MoveNextAsync().ConfigureAwait(false))
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
            finally
            {
                await e.DisposeAsync().ConfigureAwait(false);
            }

            return null;
        }

        private static async Task<double?> AverageCore<TSource>(IAsyncEnumerable<TSource> source, Func<TSource, long?> selector, CancellationToken cancellationToken)
        {
            var e = source.GetAsyncEnumerator(cancellationToken);

            try
            {
                while (await e.MoveNextAsync().ConfigureAwait(false))
                {
                    var v = selector(e.Current);
                    if (v.HasValue)
                    {
                        var sum = v.GetValueOrDefault();
                        long count = 1;
                        checked
                        {
                            while (await e.MoveNextAsync().ConfigureAwait(false))
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
            finally
            {
                await e.DisposeAsync().ConfigureAwait(false);
            }

            return null;
        }

        private static async Task<double?> AverageCore<TSource>(IAsyncEnumerable<TSource> source, Func<TSource, ValueTask<long?>> selector, CancellationToken cancellationToken)
        {
            var e = source.GetAsyncEnumerator(cancellationToken);

            try
            {
                while (await e.MoveNextAsync().ConfigureAwait(false))
                {
                    var v = await selector(e.Current).ConfigureAwait(false);
                    if (v.HasValue)
                    {
                        var sum = v.GetValueOrDefault();
                        long count = 1;
                        checked
                        {
                            while (await e.MoveNextAsync().ConfigureAwait(false))
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
            finally
            {
                await e.DisposeAsync().ConfigureAwait(false);
            }

            return null;
        }

#if !NO_DEEP_CANCELLATION
        private static async Task<double?> AverageCore<TSource>(IAsyncEnumerable<TSource> source, Func<TSource, CancellationToken, ValueTask<long?>> selector, CancellationToken cancellationToken)
        {
            var e = source.GetAsyncEnumerator(cancellationToken);

            try
            {
                while (await e.MoveNextAsync().ConfigureAwait(false))
                {
                    var v = await selector(e.Current, cancellationToken).ConfigureAwait(false);
                    if (v.HasValue)
                    {
                        var sum = v.GetValueOrDefault();
                        long count = 1;
                        checked
                        {
                            while (await e.MoveNextAsync().ConfigureAwait(false))
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
            finally
            {
                await e.DisposeAsync().ConfigureAwait(false);
            }

            return null;
        }
#endif

        private static async Task<double> AverageCore(IAsyncEnumerable<double> source, CancellationToken cancellationToken)
        {
            var e = source.GetAsyncEnumerator(cancellationToken);

            try
            {
                if (!await e.MoveNextAsync().ConfigureAwait(false))
                {
                    throw Error.NoElements();
                }

                var sum = e.Current;
                long count = 1;
                while (await e.MoveNextAsync().ConfigureAwait(false))
                {
                    // There is an opportunity to short-circuit here, in that if e.Current is
                    // ever NaN then the result will always be NaN. Assuming that this case is
                    // rare enough that not checking is the better approach generally.
                    sum += e.Current;
                    ++count;
                }

                return sum / count;
            }
            finally
            {
                await e.DisposeAsync().ConfigureAwait(false);
            }
        }

        private static async Task<double> AverageCore<TSource>(IAsyncEnumerable<TSource> source, Func<TSource, double> selector, CancellationToken cancellationToken)
        {
            var e = source.GetAsyncEnumerator(cancellationToken);

            try
            {
                if (!await e.MoveNextAsync().ConfigureAwait(false))
                {
                    throw Error.NoElements();
                }

                var sum = selector(e.Current);
                long count = 1;
                while (await e.MoveNextAsync().ConfigureAwait(false))
                {
                    // There is an opportunity to short-circuit here, in that if e.Current is
                    // ever NaN then the result will always be NaN. Assuming that this case is
                    // rare enough that not checking is the better approach generally.
                    sum += selector(e.Current);
                    ++count;
                }

                return sum / count;
            }
            finally
            {
                await e.DisposeAsync().ConfigureAwait(false);
            }
        }

        private static async Task<double> AverageCore<TSource>(IAsyncEnumerable<TSource> source, Func<TSource, ValueTask<double>> selector, CancellationToken cancellationToken)
        {
            var e = source.GetAsyncEnumerator(cancellationToken);

            try
            {
                if (!await e.MoveNextAsync().ConfigureAwait(false))
                {
                    throw Error.NoElements();
                }

                var sum = await selector(e.Current).ConfigureAwait(false);
                long count = 1;
                while (await e.MoveNextAsync().ConfigureAwait(false))
                {
                    // There is an opportunity to short-circuit here, in that if e.Current is
                    // ever NaN then the result will always be NaN. Assuming that this case is
                    // rare enough that not checking is the better approach generally.
                    sum += await selector(e.Current).ConfigureAwait(false);
                    ++count;
                }

                return sum / count;
            }
            finally
            {
                await e.DisposeAsync().ConfigureAwait(false);
            }
        }

#if !NO_DEEP_CANCELLATION
        private static async Task<double> AverageCore<TSource>(IAsyncEnumerable<TSource> source, Func<TSource, CancellationToken, ValueTask<double>> selector, CancellationToken cancellationToken)
        {
            var e = source.GetAsyncEnumerator(cancellationToken);

            try
            {
                if (!await e.MoveNextAsync().ConfigureAwait(false))
                {
                    throw Error.NoElements();
                }

                var sum = await selector(e.Current, cancellationToken).ConfigureAwait(false);
                long count = 1;
                while (await e.MoveNextAsync().ConfigureAwait(false))
                {
                    // There is an opportunity to short-circuit here, in that if e.Current is
                    // ever NaN then the result will always be NaN. Assuming that this case is
                    // rare enough that not checking is the better approach generally.
                    sum += await selector(e.Current, cancellationToken).ConfigureAwait(false);
                    ++count;
                }

                return sum / count;
            }
            finally
            {
                await e.DisposeAsync().ConfigureAwait(false);
            }
        }
#endif

        private static async Task<double?> AverageCore(IAsyncEnumerable<double?> source, CancellationToken cancellationToken)
        {
            var e = source.GetAsyncEnumerator(cancellationToken);

            try
            {
                while (await e.MoveNextAsync().ConfigureAwait(false))
                {
                    var v = e.Current;
                    if (v.HasValue)
                    {
                        var sum = v.GetValueOrDefault();
                        long count = 1;
                        checked
                        {
                            while (await e.MoveNextAsync().ConfigureAwait(false))
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
            finally
            {
                await e.DisposeAsync().ConfigureAwait(false);
            }

            return null;
        }

        private static async Task<double?> AverageCore<TSource>(IAsyncEnumerable<TSource> source, Func<TSource, double?> selector, CancellationToken cancellationToken)
        {
            var e = source.GetAsyncEnumerator(cancellationToken);

            try
            {
                while (await e.MoveNextAsync().ConfigureAwait(false))
                {
                    var v = selector(e.Current);
                    if (v.HasValue)
                    {
                        var sum = v.GetValueOrDefault();
                        long count = 1;
                        checked
                        {
                            while (await e.MoveNextAsync().ConfigureAwait(false))
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
            finally
            {
                await e.DisposeAsync().ConfigureAwait(false);
            }

            return null;
        }

        private static async Task<double?> AverageCore<TSource>(IAsyncEnumerable<TSource> source, Func<TSource, ValueTask<double?>> selector, CancellationToken cancellationToken)
        {
            var e = source.GetAsyncEnumerator(cancellationToken);

            try
            {
                while (await e.MoveNextAsync().ConfigureAwait(false))
                {
                    var v = await selector(e.Current).ConfigureAwait(false);
                    if (v.HasValue)
                    {
                        var sum = v.GetValueOrDefault();
                        long count = 1;
                        checked
                        {
                            while (await e.MoveNextAsync().ConfigureAwait(false))
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
            finally
            {
                await e.DisposeAsync().ConfigureAwait(false);
            }

            return null;
        }

#if !NO_DEEP_CANCELLATION
        private static async Task<double?> AverageCore<TSource>(IAsyncEnumerable<TSource> source, Func<TSource, CancellationToken, ValueTask<double?>> selector, CancellationToken cancellationToken)
        {
            var e = source.GetAsyncEnumerator(cancellationToken);

            try
            {
                while (await e.MoveNextAsync().ConfigureAwait(false))
                {
                    var v = await selector(e.Current, cancellationToken).ConfigureAwait(false);
                    if (v.HasValue)
                    {
                        var sum = v.GetValueOrDefault();
                        long count = 1;
                        checked
                        {
                            while (await e.MoveNextAsync().ConfigureAwait(false))
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
            finally
            {
                await e.DisposeAsync().ConfigureAwait(false);
            }

            return null;
        }
#endif

        private static async Task<float> AverageCore(IAsyncEnumerable<float> source, CancellationToken cancellationToken)
        {
            var e = source.GetAsyncEnumerator(cancellationToken);

            try
            {
                if (!await e.MoveNextAsync().ConfigureAwait(false))
                {
                    throw Error.NoElements();
                }

                double sum = e.Current;
                long count = 1;
                while (await e.MoveNextAsync().ConfigureAwait(false))
                {
                    sum += e.Current;
                    ++count;
                }

                return (float)(sum / count);
            }
            finally
            {
                await e.DisposeAsync().ConfigureAwait(false);
            }
        }

        private static async Task<float> AverageCore<TSource>(IAsyncEnumerable<TSource> source, Func<TSource, float> selector, CancellationToken cancellationToken)
        {
            var e = source.GetAsyncEnumerator(cancellationToken);

            try
            {
                if (!await e.MoveNextAsync().ConfigureAwait(false))
                {
                    throw Error.NoElements();
                }

                double sum = selector(e.Current);
                long count = 1;
                while (await e.MoveNextAsync().ConfigureAwait(false))
                {
                    sum += selector(e.Current);
                    ++count;
                }

                return (float)(sum / count);
            }
            finally
            {
                await e.DisposeAsync().ConfigureAwait(false);
            }
        }

        private static async Task<float> AverageCore<TSource>(IAsyncEnumerable<TSource> source, Func<TSource, ValueTask<float>> selector, CancellationToken cancellationToken)
        {
            var e = source.GetAsyncEnumerator(cancellationToken);

            try
            {
                if (!await e.MoveNextAsync().ConfigureAwait(false))
                {
                    throw Error.NoElements();
                }

                double sum = await selector(e.Current).ConfigureAwait(false);
                long count = 1;
                while (await e.MoveNextAsync().ConfigureAwait(false))
                {
                    sum += await selector(e.Current).ConfigureAwait(false);
                    ++count;
                }

                return (float)(sum / count);
            }
            finally
            {
                await e.DisposeAsync().ConfigureAwait(false);
            }
        }

#if !NO_DEEP_CANCELLATION
        private static async Task<float> AverageCore<TSource>(IAsyncEnumerable<TSource> source, Func<TSource, CancellationToken, ValueTask<float>> selector, CancellationToken cancellationToken)
        {
            var e = source.GetAsyncEnumerator(cancellationToken);

            try
            {
                if (!await e.MoveNextAsync().ConfigureAwait(false))
                {
                    throw Error.NoElements();
                }

                double sum = await selector(e.Current, cancellationToken).ConfigureAwait(false);
                long count = 1;
                while (await e.MoveNextAsync().ConfigureAwait(false))
                {
                    sum += await selector(e.Current, cancellationToken).ConfigureAwait(false);
                    ++count;
                }

                return (float)(sum / count);
            }
            finally
            {
                await e.DisposeAsync().ConfigureAwait(false);
            }
        }
#endif

        private static async Task<float?> AverageCore(IAsyncEnumerable<float?> source, CancellationToken cancellationToken)
        {
            var e = source.GetAsyncEnumerator(cancellationToken);

            try
            {
                while (await e.MoveNextAsync().ConfigureAwait(false))
                {
                    var v = e.Current;
                    if (v.HasValue)
                    {
                        double sum = v.GetValueOrDefault();
                        long count = 1;
                        checked
                        {
                            while (await e.MoveNextAsync().ConfigureAwait(false))
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
            finally
            {
                await e.DisposeAsync().ConfigureAwait(false);
            }

            return null;
        }

        private static async Task<float?> AverageCore<TSource>(IAsyncEnumerable<TSource> source, Func<TSource, float?> selector, CancellationToken cancellationToken)
        {
            var e = source.GetAsyncEnumerator(cancellationToken);

            try
            {
                while (await e.MoveNextAsync().ConfigureAwait(false))
                {
                    var v = selector(e.Current);
                    if (v.HasValue)
                    {
                        double sum = v.GetValueOrDefault();
                        long count = 1;
                        checked
                        {
                            while (await e.MoveNextAsync().ConfigureAwait(false))
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
            finally
            {
                await e.DisposeAsync().ConfigureAwait(false);
            }

            return null;
        }

        private static async Task<float?> AverageCore<TSource>(IAsyncEnumerable<TSource> source, Func<TSource, ValueTask<float?>> selector, CancellationToken cancellationToken)
        {
            var e = source.GetAsyncEnumerator(cancellationToken);

            try
            {
                while (await e.MoveNextAsync().ConfigureAwait(false))
                {
                    var v = await selector(e.Current).ConfigureAwait(false);
                    if (v.HasValue)
                    {
                        double sum = v.GetValueOrDefault();
                        long count = 1;
                        checked
                        {
                            while (await e.MoveNextAsync().ConfigureAwait(false))
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
            finally
            {
                await e.DisposeAsync().ConfigureAwait(false);
            }

            return null;
        }

#if !NO_DEEP_CANCELLATION
        private static async Task<float?> AverageCore<TSource>(IAsyncEnumerable<TSource> source, Func<TSource, CancellationToken, ValueTask<float?>> selector, CancellationToken cancellationToken)
        {
            var e = source.GetAsyncEnumerator(cancellationToken);

            try
            {
                while (await e.MoveNextAsync().ConfigureAwait(false))
                {
                    var v = await selector(e.Current, cancellationToken).ConfigureAwait(false);
                    if (v.HasValue)
                    {
                        double sum = v.GetValueOrDefault();
                        long count = 1;
                        checked
                        {
                            while (await e.MoveNextAsync().ConfigureAwait(false))
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
            finally
            {
                await e.DisposeAsync().ConfigureAwait(false);
            }

            return null;
        }
#endif

        private static async Task<decimal> AverageCore(IAsyncEnumerable<decimal> source, CancellationToken cancellationToken)
        {
            var e = source.GetAsyncEnumerator(cancellationToken);

            try
            {
                if (!await e.MoveNextAsync().ConfigureAwait(false))
                {
                    throw Error.NoElements();
                }

                var sum = e.Current;
                long count = 1;
                while (await e.MoveNextAsync().ConfigureAwait(false))
                {
                    sum += e.Current;
                    ++count;
                }

                return sum / count;
            }
            finally
            {
                await e.DisposeAsync().ConfigureAwait(false);
            }
        }

        private static async Task<decimal> AverageCore<TSource>(IAsyncEnumerable<TSource> source, Func<TSource, decimal> selector, CancellationToken cancellationToken)
        {
            var e = source.GetAsyncEnumerator(cancellationToken);

            try
            {
                if (!await e.MoveNextAsync().ConfigureAwait(false))
                {
                    throw Error.NoElements();
                }

                var sum = selector(e.Current);
                long count = 1;
                while (await e.MoveNextAsync().ConfigureAwait(false))
                {
                    sum += selector(e.Current);
                    ++count;
                }

                return sum / count;
            }
            finally
            {
                await e.DisposeAsync().ConfigureAwait(false);
            }
        }

        private static async Task<decimal> AverageCore<TSource>(IAsyncEnumerable<TSource> source, Func<TSource, ValueTask<decimal>> selector, CancellationToken cancellationToken)
        {
            var e = source.GetAsyncEnumerator(cancellationToken);

            try
            {
                if (!await e.MoveNextAsync().ConfigureAwait(false))
                {
                    throw Error.NoElements();
                }

                var sum = await selector(e.Current).ConfigureAwait(false);
                long count = 1;
                while (await e.MoveNextAsync().ConfigureAwait(false))
                {
                    sum += await selector(e.Current).ConfigureAwait(false);
                    ++count;
                }

                return sum / count;
            }
            finally
            {
                await e.DisposeAsync().ConfigureAwait(false);
            }
        }

#if !NO_DEEP_CANCELLATION
        private static async Task<decimal> AverageCore<TSource>(IAsyncEnumerable<TSource> source, Func<TSource, CancellationToken, ValueTask<decimal>> selector, CancellationToken cancellationToken)
        {
            var e = source.GetAsyncEnumerator(cancellationToken);

            try
            {
                if (!await e.MoveNextAsync().ConfigureAwait(false))
                {
                    throw Error.NoElements();
                }

                var sum = await selector(e.Current, cancellationToken).ConfigureAwait(false);
                long count = 1;
                while (await e.MoveNextAsync().ConfigureAwait(false))
                {
                    sum += await selector(e.Current, cancellationToken).ConfigureAwait(false);
                    ++count;
                }

                return sum / count;
            }
            finally
            {
                await e.DisposeAsync().ConfigureAwait(false);
            }
        }
#endif

        private static async Task<decimal?> AverageCore(IAsyncEnumerable<decimal?> source, CancellationToken cancellationToken)
        {
            var e = source.GetAsyncEnumerator(cancellationToken);

            try
            {
                while (await e.MoveNextAsync().ConfigureAwait(false))
                {
                    var v = e.Current;
                    if (v.HasValue)
                    {
                        var sum = v.GetValueOrDefault();
                        long count = 1;
                        while (await e.MoveNextAsync().ConfigureAwait(false))
                        {
                            v = e.Current;
                            if (v.HasValue)
                            {
                                sum += v.GetValueOrDefault();
                                ++count;
                            }
                        }

                        return sum / count;
                    }
                }
            }
            finally
            {
                await e.DisposeAsync().ConfigureAwait(false);
            }

            return null;
        }

        private static async Task<decimal?> AverageCore<TSource>(IAsyncEnumerable<TSource> source, Func<TSource, decimal?> selector, CancellationToken cancellationToken)
        {
            var e = source.GetAsyncEnumerator(cancellationToken);

            try
            {
                while (await e.MoveNextAsync().ConfigureAwait(false))
                {
                    var v = selector(e.Current);
                    if (v.HasValue)
                    {
                        var sum = v.GetValueOrDefault();
                        long count = 1;
                        while (await e.MoveNextAsync().ConfigureAwait(false))
                        {
                            v = selector(e.Current);
                            if (v.HasValue)
                            {
                                sum += v.GetValueOrDefault();
                                ++count;
                            }
                        }

                        return sum / count;
                    }
                }
            }
            finally
            {
                await e.DisposeAsync().ConfigureAwait(false);
            }

            return null;
        }

        private static async Task<decimal?> AverageCore<TSource>(IAsyncEnumerable<TSource> source, Func<TSource, ValueTask<decimal?>> selector, CancellationToken cancellationToken)
        {
            var e = source.GetAsyncEnumerator(cancellationToken);

            try
            {
                while (await e.MoveNextAsync().ConfigureAwait(false))
                {
                    var v = await selector(e.Current).ConfigureAwait(false);
                    if (v.HasValue)
                    {
                        var sum = v.GetValueOrDefault();
                        long count = 1;
                        while (await e.MoveNextAsync().ConfigureAwait(false))
                        {
                            v = await selector(e.Current).ConfigureAwait(false);
                            if (v.HasValue)
                            {
                                sum += v.GetValueOrDefault();
                                ++count;
                            }
                        }

                        return sum / count;
                    }
                }
            }
            finally
            {
                await e.DisposeAsync().ConfigureAwait(false);
            }

            return null;
        }

#if !NO_DEEP_CANCELLATION
        private static async Task<decimal?> AverageCore<TSource>(IAsyncEnumerable<TSource> source, Func<TSource, CancellationToken, ValueTask<decimal?>> selector, CancellationToken cancellationToken)
        {
            var e = source.GetAsyncEnumerator(cancellationToken);

            try
            {
                while (await e.MoveNextAsync().ConfigureAwait(false))
                {
                    var v = await selector(e.Current, cancellationToken).ConfigureAwait(false);
                    if (v.HasValue)
                    {
                        var sum = v.GetValueOrDefault();
                        long count = 1;
                        while (await e.MoveNextAsync().ConfigureAwait(false))
                        {
                            v = await selector(e.Current, cancellationToken).ConfigureAwait(false);
                            if (v.HasValue)
                            {
                                sum += v.GetValueOrDefault();
                                ++count;
                            }
                        }

                        return sum / count;
                    }
                }
            }
            finally
            {
                await e.DisposeAsync().ConfigureAwait(false);
            }

            return null;
        }
#endif
    }
}

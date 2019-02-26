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
        public static ValueTask<int> SumAsync(this IAsyncEnumerable<int> source, CancellationToken cancellationToken = default)
        {
            if (source == null)
                throw Error.ArgumentNull(nameof(source));

            return Core(source, cancellationToken);

            static async ValueTask<int> Core(IAsyncEnumerable<int> _source, CancellationToken _cancellationToken)
            {
                var sum = 0;

                await foreach (int value in AsyncEnumerableExtensions.WithCancellation(_source, _cancellationToken).ConfigureAwait(false))
                {
                    checked
                    {
                        sum += value;
                    }
                }

                return sum;
            }
        }

        public static ValueTask<int> SumAsync<TSource>(this IAsyncEnumerable<TSource> source, Func<TSource, int> selector, CancellationToken cancellationToken = default)
        {
            if (source == null)
                throw Error.ArgumentNull(nameof(source));
            if (selector == null)
                throw Error.ArgumentNull(nameof(selector));

            return Core(source, selector, cancellationToken);

            static async ValueTask<int> Core(IAsyncEnumerable<TSource> _source, Func<TSource, int> _selector, CancellationToken _cancellationToken)
            {
                var sum = 0;

                await foreach (TSource item in AsyncEnumerableExtensions.WithCancellation(_source, _cancellationToken).ConfigureAwait(false))
                {
                    var value = _selector(item);

                    checked
                    {
                        sum += value;
                    }
                }

                return sum;
            }
        }

        public static ValueTask<int> SumAwaitAsync<TSource>(this IAsyncEnumerable<TSource> source, Func<TSource, ValueTask<int>> selector, CancellationToken cancellationToken = default)
        {
            if (source == null)
                throw Error.ArgumentNull(nameof(source));
            if (selector == null)
                throw Error.ArgumentNull(nameof(selector));

            return Core(source, selector, cancellationToken);

            static async ValueTask<int> Core(IAsyncEnumerable<TSource> _source, Func<TSource, ValueTask<int>> _selector, CancellationToken _cancellationToken)
            {
                var sum = 0;

                await foreach (TSource item in AsyncEnumerableExtensions.WithCancellation(_source, _cancellationToken).ConfigureAwait(false))
                {
                    var value = await _selector(item).ConfigureAwait(false);

                    checked
                    {
                        sum += value;
                    }
                }

                return sum;
            }
        }

#if !NO_DEEP_CANCELLATION
        public static ValueTask<int> SumAwaitWithCancellationAsync<TSource>(this IAsyncEnumerable<TSource> source, Func<TSource, CancellationToken, ValueTask<int>> selector, CancellationToken cancellationToken = default)
        {
            if (source == null)
                throw Error.ArgumentNull(nameof(source));
            if (selector == null)
                throw Error.ArgumentNull(nameof(selector));

            return Core(source, selector, cancellationToken);

            static async ValueTask<int> Core(IAsyncEnumerable<TSource> _source, Func<TSource, CancellationToken, ValueTask<int>> _selector, CancellationToken _cancellationToken)
            {
                var sum = 0;

                await foreach (TSource item in AsyncEnumerableExtensions.WithCancellation(_source, _cancellationToken).ConfigureAwait(false))
                {
                    var value = await _selector(item, _cancellationToken).ConfigureAwait(false);

                    checked
                    {
                        sum += value;
                    }
                }

                return sum;
            }
        }
#endif

        public static ValueTask<long> SumAsync(this IAsyncEnumerable<long> source, CancellationToken cancellationToken = default)
        {
            if (source == null)
                throw Error.ArgumentNull(nameof(source));

            return Core(source, cancellationToken);

            static async ValueTask<long> Core(IAsyncEnumerable<long> _source, CancellationToken _cancellationToken)
            {
                var sum = 0L;

                await foreach (long value in AsyncEnumerableExtensions.WithCancellation(_source, _cancellationToken).ConfigureAwait(false))
                {
                    checked
                    {
                        sum += value;
                    }
                }

                return sum;
            }
        }

        public static ValueTask<long> SumAsync<TSource>(this IAsyncEnumerable<TSource> source, Func<TSource, long> selector, CancellationToken cancellationToken = default)
        {
            if (source == null)
                throw Error.ArgumentNull(nameof(source));
            if (selector == null)
                throw Error.ArgumentNull(nameof(selector));

            return Core(source, selector, cancellationToken);

            static async ValueTask<long> Core(IAsyncEnumerable<TSource> _source, Func<TSource, long> _selector, CancellationToken _cancellationToken)
            {
                var sum = 0L;

                await foreach (TSource item in AsyncEnumerableExtensions.WithCancellation(_source, _cancellationToken).ConfigureAwait(false))
                {
                    var value = _selector(item);

                    checked
                    {
                        sum += value;
                    }
                }

                return sum;
            }
        }

        public static ValueTask<long> SumAwaitAsync<TSource>(this IAsyncEnumerable<TSource> source, Func<TSource, ValueTask<long>> selector, CancellationToken cancellationToken = default)
        {
            if (source == null)
                throw Error.ArgumentNull(nameof(source));
            if (selector == null)
                throw Error.ArgumentNull(nameof(selector));

            return Core(source, selector, cancellationToken);

            static async ValueTask<long> Core(IAsyncEnumerable<TSource> _source, Func<TSource, ValueTask<long>> _selector, CancellationToken _cancellationToken)
            {
                var sum = 0L;

                await foreach (TSource item in AsyncEnumerableExtensions.WithCancellation(_source, _cancellationToken).ConfigureAwait(false))
                {
                    var value = await _selector(item).ConfigureAwait(false);

                    checked
                    {
                        sum += value;
                    }
                }

                return sum;
            }
        }

#if !NO_DEEP_CANCELLATION
        public static ValueTask<long> SumAwaitWithCancellationAsync<TSource>(this IAsyncEnumerable<TSource> source, Func<TSource, CancellationToken, ValueTask<long>> selector, CancellationToken cancellationToken = default)
        {
            if (source == null)
                throw Error.ArgumentNull(nameof(source));
            if (selector == null)
                throw Error.ArgumentNull(nameof(selector));

            return Core(source, selector, cancellationToken);

            static async ValueTask<long> Core(IAsyncEnumerable<TSource> _source, Func<TSource, CancellationToken, ValueTask<long>> _selector, CancellationToken _cancellationToken)
            {
                var sum = 0L;

                await foreach (TSource item in AsyncEnumerableExtensions.WithCancellation(_source, _cancellationToken).ConfigureAwait(false))
                {
                    var value = await _selector(item, _cancellationToken).ConfigureAwait(false);

                    checked
                    {
                        sum += value;
                    }
                }

                return sum;
            }
        }
#endif

        public static ValueTask<float> SumAsync(this IAsyncEnumerable<float> source, CancellationToken cancellationToken = default)
        {
            if (source == null)
                throw Error.ArgumentNull(nameof(source));

            return Core(source, cancellationToken);

            static async ValueTask<float> Core(IAsyncEnumerable<float> _source, CancellationToken _cancellationToken)
            {
                var sum = 0.0f;

                await foreach (float value in AsyncEnumerableExtensions.WithCancellation(_source, _cancellationToken).ConfigureAwait(false))
                {
                    sum += value;
                }

                return sum;
            }
        }

        public static ValueTask<float> SumAsync<TSource>(this IAsyncEnumerable<TSource> source, Func<TSource, float> selector, CancellationToken cancellationToken = default)
        {
            if (source == null)
                throw Error.ArgumentNull(nameof(source));
            if (selector == null)
                throw Error.ArgumentNull(nameof(selector));

            return Core(source, selector, cancellationToken);

            static async ValueTask<float> Core(IAsyncEnumerable<TSource> _source, Func<TSource, float> _selector, CancellationToken _cancellationToken)
            {
                var sum = 0.0f;

                await foreach (TSource item in AsyncEnumerableExtensions.WithCancellation(_source, _cancellationToken).ConfigureAwait(false))
                {
                    var value = _selector(item);

                    sum += value;
                }

                return sum;
            }
        }

        public static ValueTask<float> SumAwaitAsync<TSource>(this IAsyncEnumerable<TSource> source, Func<TSource, ValueTask<float>> selector, CancellationToken cancellationToken = default)
        {
            if (source == null)
                throw Error.ArgumentNull(nameof(source));
            if (selector == null)
                throw Error.ArgumentNull(nameof(selector));

            return Core(source, selector, cancellationToken);

            static async ValueTask<float> Core(IAsyncEnumerable<TSource> _source, Func<TSource, ValueTask<float>> _selector, CancellationToken _cancellationToken)
            {
                var sum = 0.0f;

                await foreach (TSource item in AsyncEnumerableExtensions.WithCancellation(_source, _cancellationToken).ConfigureAwait(false))
                {
                    var value = await _selector(item).ConfigureAwait(false);

                    sum += value;
                }

                return sum;
            }
        }

#if !NO_DEEP_CANCELLATION
        public static ValueTask<float> SumAwaitWithCancellationAsync<TSource>(this IAsyncEnumerable<TSource> source, Func<TSource, CancellationToken, ValueTask<float>> selector, CancellationToken cancellationToken = default)
        {
            if (source == null)
                throw Error.ArgumentNull(nameof(source));
            if (selector == null)
                throw Error.ArgumentNull(nameof(selector));

            return Core(source, selector, cancellationToken);

            static async ValueTask<float> Core(IAsyncEnumerable<TSource> _source, Func<TSource, CancellationToken, ValueTask<float>> _selector, CancellationToken _cancellationToken)
            {
                var sum = 0.0f;

                await foreach (TSource item in AsyncEnumerableExtensions.WithCancellation(_source, _cancellationToken).ConfigureAwait(false))
                {
                    var value = await _selector(item, _cancellationToken).ConfigureAwait(false);

                    sum += value;
                }

                return sum;
            }
        }
#endif

        public static ValueTask<double> SumAsync(this IAsyncEnumerable<double> source, CancellationToken cancellationToken = default)
        {
            if (source == null)
                throw Error.ArgumentNull(nameof(source));

            return Core(source, cancellationToken);

            static async ValueTask<double> Core(IAsyncEnumerable<double> _source, CancellationToken _cancellationToken)
            {
                var sum = 0.0;

                await foreach (double value in AsyncEnumerableExtensions.WithCancellation(_source, _cancellationToken).ConfigureAwait(false))
                {
                    sum += value;
                }

                return sum;
            }
        }

        public static ValueTask<double> SumAsync<TSource>(this IAsyncEnumerable<TSource> source, Func<TSource, double> selector, CancellationToken cancellationToken = default)
        {
            if (source == null)
                throw Error.ArgumentNull(nameof(source));
            if (selector == null)
                throw Error.ArgumentNull(nameof(selector));

            return Core(source, selector, cancellationToken);

            static async ValueTask<double> Core(IAsyncEnumerable<TSource> _source, Func<TSource, double> _selector, CancellationToken _cancellationToken)
            {
                var sum = 0.0;

                await foreach (TSource item in AsyncEnumerableExtensions.WithCancellation(_source, _cancellationToken).ConfigureAwait(false))
                {
                    var value = _selector(item);

                    sum += value;
                }

                return sum;
            }
        }

        public static ValueTask<double> SumAwaitAsync<TSource>(this IAsyncEnumerable<TSource> source, Func<TSource, ValueTask<double>> selector, CancellationToken cancellationToken = default)
        {
            if (source == null)
                throw Error.ArgumentNull(nameof(source));
            if (selector == null)
                throw Error.ArgumentNull(nameof(selector));

            return Core(source, selector, cancellationToken);

            static async ValueTask<double> Core(IAsyncEnumerable<TSource> _source, Func<TSource, ValueTask<double>> _selector, CancellationToken _cancellationToken)
            {
                var sum = 0.0;

                await foreach (TSource item in AsyncEnumerableExtensions.WithCancellation(_source, _cancellationToken).ConfigureAwait(false))
                {
                    var value = await _selector(item).ConfigureAwait(false);

                    sum += value;
                }

                return sum;
            }
        }

#if !NO_DEEP_CANCELLATION
        public static ValueTask<double> SumAwaitWithCancellationAsync<TSource>(this IAsyncEnumerable<TSource> source, Func<TSource, CancellationToken, ValueTask<double>> selector, CancellationToken cancellationToken = default)
        {
            if (source == null)
                throw Error.ArgumentNull(nameof(source));
            if (selector == null)
                throw Error.ArgumentNull(nameof(selector));

            return Core(source, selector, cancellationToken);

            static async ValueTask<double> Core(IAsyncEnumerable<TSource> _source, Func<TSource, CancellationToken, ValueTask<double>> _selector, CancellationToken _cancellationToken)
            {
                var sum = 0.0;

                await foreach (TSource item in AsyncEnumerableExtensions.WithCancellation(_source, _cancellationToken).ConfigureAwait(false))
                {
                    var value = await _selector(item, _cancellationToken).ConfigureAwait(false);

                    sum += value;
                }

                return sum;
            }
        }
#endif

        public static ValueTask<decimal> SumAsync(this IAsyncEnumerable<decimal> source, CancellationToken cancellationToken = default)
        {
            if (source == null)
                throw Error.ArgumentNull(nameof(source));

            return Core(source, cancellationToken);

            static async ValueTask<decimal> Core(IAsyncEnumerable<decimal> _source, CancellationToken _cancellationToken)
            {
                var sum = 0m;

                await foreach (decimal value in AsyncEnumerableExtensions.WithCancellation(_source, _cancellationToken).ConfigureAwait(false))
                {
                    sum += value;
                }

                return sum;
            }
        }

        public static ValueTask<decimal> SumAsync<TSource>(this IAsyncEnumerable<TSource> source, Func<TSource, decimal> selector, CancellationToken cancellationToken = default)
        {
            if (source == null)
                throw Error.ArgumentNull(nameof(source));
            if (selector == null)
                throw Error.ArgumentNull(nameof(selector));

            return Core(source, selector, cancellationToken);

            static async ValueTask<decimal> Core(IAsyncEnumerable<TSource> _source, Func<TSource, decimal> _selector, CancellationToken _cancellationToken)
            {
                var sum = 0m;

                await foreach (TSource item in AsyncEnumerableExtensions.WithCancellation(_source, _cancellationToken).ConfigureAwait(false))
                {
                    var value = _selector(item);

                    sum += value;
                }

                return sum;
            }
        }

        public static ValueTask<decimal> SumAwaitAsync<TSource>(this IAsyncEnumerable<TSource> source, Func<TSource, ValueTask<decimal>> selector, CancellationToken cancellationToken = default)
        {
            if (source == null)
                throw Error.ArgumentNull(nameof(source));
            if (selector == null)
                throw Error.ArgumentNull(nameof(selector));

            return Core(source, selector, cancellationToken);

            static async ValueTask<decimal> Core(IAsyncEnumerable<TSource> _source, Func<TSource, ValueTask<decimal>> _selector, CancellationToken _cancellationToken)
            {
                var sum = 0m;

                await foreach (TSource item in AsyncEnumerableExtensions.WithCancellation(_source, _cancellationToken).ConfigureAwait(false))
                {
                    var value = await _selector(item).ConfigureAwait(false);

                    sum += value;
                }

                return sum;
            }
        }

#if !NO_DEEP_CANCELLATION
        public static ValueTask<decimal> SumAwaitWithCancellationAsync<TSource>(this IAsyncEnumerable<TSource> source, Func<TSource, CancellationToken, ValueTask<decimal>> selector, CancellationToken cancellationToken = default)
        {
            if (source == null)
                throw Error.ArgumentNull(nameof(source));
            if (selector == null)
                throw Error.ArgumentNull(nameof(selector));

            return Core(source, selector, cancellationToken);

            static async ValueTask<decimal> Core(IAsyncEnumerable<TSource> _source, Func<TSource, CancellationToken, ValueTask<decimal>> _selector, CancellationToken _cancellationToken)
            {
                var sum = 0m;

                await foreach (TSource item in AsyncEnumerableExtensions.WithCancellation(_source, _cancellationToken).ConfigureAwait(false))
                {
                    var value = await _selector(item, _cancellationToken).ConfigureAwait(false);

                    sum += value;
                }

                return sum;
            }
        }
#endif

        public static ValueTask<int?> SumAsync(this IAsyncEnumerable<int?> source, CancellationToken cancellationToken = default)
        {
            if (source == null)
                throw Error.ArgumentNull(nameof(source));

            return Core(source, cancellationToken);

            static async ValueTask<int?> Core(IAsyncEnumerable<int?> _source, CancellationToken _cancellationToken)
            {
                var sum = 0;

                await foreach (int? value in AsyncEnumerableExtensions.WithCancellation(_source, _cancellationToken).ConfigureAwait(false))
                {
                    checked
                    {
                        sum += value.GetValueOrDefault();
                    }
                }

                return sum;
            }
        }

        public static ValueTask<int?> SumAsync<TSource>(this IAsyncEnumerable<TSource> source, Func<TSource, int?> selector, CancellationToken cancellationToken = default)
        {
            if (source == null)
                throw Error.ArgumentNull(nameof(source));
            if (selector == null)
                throw Error.ArgumentNull(nameof(selector));

            return Core(source, selector, cancellationToken);

            static async ValueTask<int?> Core(IAsyncEnumerable<TSource> _source, Func<TSource, int?> _selector, CancellationToken _cancellationToken)
            {
                var sum = 0;

                await foreach (TSource item in AsyncEnumerableExtensions.WithCancellation(_source, _cancellationToken).ConfigureAwait(false))
                {
                    var value = _selector(item);

                    checked
                    {
                        sum += value.GetValueOrDefault();
                    }
                }

                return sum;
            }
        }

        public static ValueTask<int?> SumAwaitAsync<TSource>(this IAsyncEnumerable<TSource> source, Func<TSource, ValueTask<int?>> selector, CancellationToken cancellationToken = default)
        {
            if (source == null)
                throw Error.ArgumentNull(nameof(source));
            if (selector == null)
                throw Error.ArgumentNull(nameof(selector));

            return Core(source, selector, cancellationToken);

            static async ValueTask<int?> Core(IAsyncEnumerable<TSource> _source, Func<TSource, ValueTask<int?>> _selector, CancellationToken _cancellationToken)
            {
                var sum = 0;

                await foreach (TSource item in AsyncEnumerableExtensions.WithCancellation(_source, _cancellationToken).ConfigureAwait(false))
                {
                    var value = await _selector(item).ConfigureAwait(false);

                    checked
                    {
                        sum += value.GetValueOrDefault();
                    }
                }

                return sum;
            }
        }

#if !NO_DEEP_CANCELLATION
        public static ValueTask<int?> SumAwaitWithCancellationAsync<TSource>(this IAsyncEnumerable<TSource> source, Func<TSource, CancellationToken, ValueTask<int?>> selector, CancellationToken cancellationToken = default)
        {
            if (source == null)
                throw Error.ArgumentNull(nameof(source));
            if (selector == null)
                throw Error.ArgumentNull(nameof(selector));

            return Core(source, selector, cancellationToken);

            static async ValueTask<int?> Core(IAsyncEnumerable<TSource> _source, Func<TSource, CancellationToken, ValueTask<int?>> _selector, CancellationToken _cancellationToken)
            {
                var sum = 0;

                await foreach (TSource item in AsyncEnumerableExtensions.WithCancellation(_source, _cancellationToken).ConfigureAwait(false))
                {
                    var value = await _selector(item, _cancellationToken).ConfigureAwait(false);

                    checked
                    {
                        sum += value.GetValueOrDefault();
                    }
                }

                return sum;
            }
        }
#endif

        public static ValueTask<long?> SumAsync(this IAsyncEnumerable<long?> source, CancellationToken cancellationToken = default)
        {
            if (source == null)
                throw Error.ArgumentNull(nameof(source));

            return Core(source, cancellationToken);

            static async ValueTask<long?> Core(IAsyncEnumerable<long?> _source, CancellationToken _cancellationToken)
            {
                var sum = 0L;

                await foreach (long? value in AsyncEnumerableExtensions.WithCancellation(_source, _cancellationToken).ConfigureAwait(false))
                {
                    checked
                    {
                        sum += value.GetValueOrDefault();
                    }
                }

                return sum;
            }
        }

        public static ValueTask<long?> SumAsync<TSource>(this IAsyncEnumerable<TSource> source, Func<TSource, long?> selector, CancellationToken cancellationToken = default)
        {
            if (source == null)
                throw Error.ArgumentNull(nameof(source));
            if (selector == null)
                throw Error.ArgumentNull(nameof(selector));

            return Core(source, selector, cancellationToken);

            static async ValueTask<long?> Core(IAsyncEnumerable<TSource> _source, Func<TSource, long?> _selector, CancellationToken _cancellationToken)
            {
                var sum = 0L;

                await foreach (TSource item in AsyncEnumerableExtensions.WithCancellation(_source, _cancellationToken).ConfigureAwait(false))
                {
                    var value = _selector(item);

                    checked
                    {
                        sum += value.GetValueOrDefault();
                    }
                }

                return sum;
            }
        }

        public static ValueTask<long?> SumAwaitAsync<TSource>(this IAsyncEnumerable<TSource> source, Func<TSource, ValueTask<long?>> selector, CancellationToken cancellationToken = default)
        {
            if (source == null)
                throw Error.ArgumentNull(nameof(source));
            if (selector == null)
                throw Error.ArgumentNull(nameof(selector));

            return Core(source, selector, cancellationToken);

            static async ValueTask<long?> Core(IAsyncEnumerable<TSource> _source, Func<TSource, ValueTask<long?>> _selector, CancellationToken _cancellationToken)
            {
                var sum = 0L;

                await foreach (TSource item in AsyncEnumerableExtensions.WithCancellation(_source, _cancellationToken).ConfigureAwait(false))
                {
                    var value = await _selector(item).ConfigureAwait(false);

                    checked
                    {
                        sum += value.GetValueOrDefault();
                    }
                }

                return sum;
            }
        }

#if !NO_DEEP_CANCELLATION
        public static ValueTask<long?> SumAwaitWithCancellationAsync<TSource>(this IAsyncEnumerable<TSource> source, Func<TSource, CancellationToken, ValueTask<long?>> selector, CancellationToken cancellationToken = default)
        {
            if (source == null)
                throw Error.ArgumentNull(nameof(source));
            if (selector == null)
                throw Error.ArgumentNull(nameof(selector));

            return Core(source, selector, cancellationToken);

            static async ValueTask<long?> Core(IAsyncEnumerable<TSource> _source, Func<TSource, CancellationToken, ValueTask<long?>> _selector, CancellationToken _cancellationToken)
            {
                var sum = 0L;

                await foreach (TSource item in AsyncEnumerableExtensions.WithCancellation(_source, _cancellationToken).ConfigureAwait(false))
                {
                    var value = await _selector(item, _cancellationToken).ConfigureAwait(false);

                    checked
                    {
                        sum += value.GetValueOrDefault();
                    }
                }

                return sum;
            }
        }
#endif

        public static ValueTask<float?> SumAsync(this IAsyncEnumerable<float?> source, CancellationToken cancellationToken = default)
        {
            if (source == null)
                throw Error.ArgumentNull(nameof(source));

            return Core(source, cancellationToken);

            static async ValueTask<float?> Core(IAsyncEnumerable<float?> _source, CancellationToken _cancellationToken)
            {
                var sum = 0.0f;

                await foreach (float? value in AsyncEnumerableExtensions.WithCancellation(_source, _cancellationToken).ConfigureAwait(false))
                {
                    sum += value.GetValueOrDefault();
                }

                return sum;
            }
        }

        public static ValueTask<float?> SumAsync<TSource>(this IAsyncEnumerable<TSource> source, Func<TSource, float?> selector, CancellationToken cancellationToken = default)
        {
            if (source == null)
                throw Error.ArgumentNull(nameof(source));
            if (selector == null)
                throw Error.ArgumentNull(nameof(selector));

            return Core(source, selector, cancellationToken);

            static async ValueTask<float?> Core(IAsyncEnumerable<TSource> _source, Func<TSource, float?> _selector, CancellationToken _cancellationToken)
            {
                var sum = 0.0f;

                await foreach (TSource item in AsyncEnumerableExtensions.WithCancellation(_source, _cancellationToken).ConfigureAwait(false))
                {
                    var value = _selector(item);

                    sum += value.GetValueOrDefault();
                }

                return sum;
            }
        }

        public static ValueTask<float?> SumAwaitAsync<TSource>(this IAsyncEnumerable<TSource> source, Func<TSource, ValueTask<float?>> selector, CancellationToken cancellationToken = default)
        {
            if (source == null)
                throw Error.ArgumentNull(nameof(source));
            if (selector == null)
                throw Error.ArgumentNull(nameof(selector));

            return Core(source, selector, cancellationToken);

            static async ValueTask<float?> Core(IAsyncEnumerable<TSource> _source, Func<TSource, ValueTask<float?>> _selector, CancellationToken _cancellationToken)
            {
                var sum = 0.0f;

                await foreach (TSource item in AsyncEnumerableExtensions.WithCancellation(_source, _cancellationToken).ConfigureAwait(false))
                {
                    var value = await _selector(item).ConfigureAwait(false);

                    sum += value.GetValueOrDefault();
                }

                return sum;
            }
        }

#if !NO_DEEP_CANCELLATION
        public static ValueTask<float?> SumAwaitWithCancellationAsync<TSource>(this IAsyncEnumerable<TSource> source, Func<TSource, CancellationToken, ValueTask<float?>> selector, CancellationToken cancellationToken = default)
        {
            if (source == null)
                throw Error.ArgumentNull(nameof(source));
            if (selector == null)
                throw Error.ArgumentNull(nameof(selector));

            return Core(source, selector, cancellationToken);

            static async ValueTask<float?> Core(IAsyncEnumerable<TSource> _source, Func<TSource, CancellationToken, ValueTask<float?>> _selector, CancellationToken _cancellationToken)
            {
                var sum = 0.0f;

                await foreach (TSource item in AsyncEnumerableExtensions.WithCancellation(_source, _cancellationToken).ConfigureAwait(false))
                {
                    var value = await _selector(item, _cancellationToken).ConfigureAwait(false);

                    sum += value.GetValueOrDefault();
                }

                return sum;
            }
        }
#endif

        public static ValueTask<double?> SumAsync(this IAsyncEnumerable<double?> source, CancellationToken cancellationToken = default)
        {
            if (source == null)
                throw Error.ArgumentNull(nameof(source));

            return Core(source, cancellationToken);

            static async ValueTask<double?> Core(IAsyncEnumerable<double?> _source, CancellationToken _cancellationToken)
            {
                var sum = 0.0;

                await foreach (double? value in AsyncEnumerableExtensions.WithCancellation(_source, _cancellationToken).ConfigureAwait(false))
                {
                    sum += value.GetValueOrDefault();
                }

                return sum;
            }
        }

        public static ValueTask<double?> SumAsync<TSource>(this IAsyncEnumerable<TSource> source, Func<TSource, double?> selector, CancellationToken cancellationToken = default)
        {
            if (source == null)
                throw Error.ArgumentNull(nameof(source));
            if (selector == null)
                throw Error.ArgumentNull(nameof(selector));

            return Core(source, selector, cancellationToken);

            static async ValueTask<double?> Core(IAsyncEnumerable<TSource> _source, Func<TSource, double?> _selector, CancellationToken _cancellationToken)
            {
                var sum = 0.0;

                await foreach (TSource item in AsyncEnumerableExtensions.WithCancellation(_source, _cancellationToken).ConfigureAwait(false))
                {
                    var value = _selector(item);

                    sum += value.GetValueOrDefault();
                }

                return sum;
            }
        }

        public static ValueTask<double?> SumAwaitAsync<TSource>(this IAsyncEnumerable<TSource> source, Func<TSource, ValueTask<double?>> selector, CancellationToken cancellationToken = default)
        {
            if (source == null)
                throw Error.ArgumentNull(nameof(source));
            if (selector == null)
                throw Error.ArgumentNull(nameof(selector));

            return Core(source, selector, cancellationToken);

            static async ValueTask<double?> Core(IAsyncEnumerable<TSource> _source, Func<TSource, ValueTask<double?>> _selector, CancellationToken _cancellationToken)
            {
                var sum = 0.0;

                await foreach (TSource item in AsyncEnumerableExtensions.WithCancellation(_source, _cancellationToken).ConfigureAwait(false))
                {
                    var value = await _selector(item).ConfigureAwait(false);

                    sum += value.GetValueOrDefault();
                }

                return sum;
            }
        }

#if !NO_DEEP_CANCELLATION
        public static ValueTask<double?> SumAwaitWithCancellationAsync<TSource>(this IAsyncEnumerable<TSource> source, Func<TSource, CancellationToken, ValueTask<double?>> selector, CancellationToken cancellationToken = default)
        {
            if (source == null)
                throw Error.ArgumentNull(nameof(source));
            if (selector == null)
                throw Error.ArgumentNull(nameof(selector));

            return Core(source, selector, cancellationToken);

            static async ValueTask<double?> Core(IAsyncEnumerable<TSource> _source, Func<TSource, CancellationToken, ValueTask<double?>> _selector, CancellationToken _cancellationToken)
            {
                var sum = 0.0;

                await foreach (TSource item in AsyncEnumerableExtensions.WithCancellation(_source, _cancellationToken).ConfigureAwait(false))
                {
                    var value = await _selector(item, _cancellationToken).ConfigureAwait(false);

                    sum += value.GetValueOrDefault();
                }

                return sum;
            }
        }
#endif

        public static ValueTask<decimal?> SumAsync(this IAsyncEnumerable<decimal?> source, CancellationToken cancellationToken = default)
        {
            if (source == null)
                throw Error.ArgumentNull(nameof(source));

            return Core(source, cancellationToken);

            static async ValueTask<decimal?> Core(IAsyncEnumerable<decimal?> _source, CancellationToken _cancellationToken)
            {
                var sum = 0m;

                await foreach (decimal? value in AsyncEnumerableExtensions.WithCancellation(_source, _cancellationToken).ConfigureAwait(false))
                {
                    sum += value.GetValueOrDefault();
                }

                return sum;
            }
        }

        public static ValueTask<decimal?> SumAsync<TSource>(this IAsyncEnumerable<TSource> source, Func<TSource, decimal?> selector, CancellationToken cancellationToken = default)
        {
            if (source == null)
                throw Error.ArgumentNull(nameof(source));
            if (selector == null)
                throw Error.ArgumentNull(nameof(selector));

            return Core(source, selector, cancellationToken);

            static async ValueTask<decimal?> Core(IAsyncEnumerable<TSource> _source, Func<TSource, decimal?> _selector, CancellationToken _cancellationToken)
            {
                var sum = 0m;

                await foreach (TSource item in AsyncEnumerableExtensions.WithCancellation(_source, _cancellationToken).ConfigureAwait(false))
                {
                    var value = _selector(item);

                    sum += value.GetValueOrDefault();
                }

                return sum;
            }
        }

        public static ValueTask<decimal?> SumAwaitAsync<TSource>(this IAsyncEnumerable<TSource> source, Func<TSource, ValueTask<decimal?>> selector, CancellationToken cancellationToken = default)
        {
            if (source == null)
                throw Error.ArgumentNull(nameof(source));
            if (selector == null)
                throw Error.ArgumentNull(nameof(selector));

            return Core(source, selector, cancellationToken);

            static async ValueTask<decimal?> Core(IAsyncEnumerable<TSource> _source, Func<TSource, ValueTask<decimal?>> _selector, CancellationToken _cancellationToken)
            {
                var sum = 0m;

                await foreach (TSource item in AsyncEnumerableExtensions.WithCancellation(_source, _cancellationToken).ConfigureAwait(false))
                {
                    var value = await _selector(item).ConfigureAwait(false);

                    sum += value.GetValueOrDefault();
                }

                return sum;
            }
        }

#if !NO_DEEP_CANCELLATION
        public static ValueTask<decimal?> SumAwaitWithCancellationAsync<TSource>(this IAsyncEnumerable<TSource> source, Func<TSource, CancellationToken, ValueTask<decimal?>> selector, CancellationToken cancellationToken = default)
        {
            if (source == null)
                throw Error.ArgumentNull(nameof(source));
            if (selector == null)
                throw Error.ArgumentNull(nameof(selector));

            return Core(source, selector, cancellationToken);

            static async ValueTask<decimal?> Core(IAsyncEnumerable<TSource> _source, Func<TSource, CancellationToken, ValueTask<decimal?>> _selector, CancellationToken _cancellationToken)
            {
                var sum = 0m;

                await foreach (TSource item in AsyncEnumerableExtensions.WithCancellation(_source, _cancellationToken).ConfigureAwait(false))
                {
                    var value = await _selector(item, _cancellationToken).ConfigureAwait(false);

                    sum += value.GetValueOrDefault();
                }

                return sum;
            }
        }
#endif

    }
}

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
        public static Task<double> Average(this IAsyncEnumerable<int> source)
        {
            if (source == null)
            {
                throw new ArgumentNullException(nameof(source));
            }

            return Average(source, CancellationToken.None);
        }

        public static Task<double?> Average(this IAsyncEnumerable<int?> source)
        {
            if (source == null)
            {
                throw new ArgumentNullException(nameof(source));
            }

            return Average(source, CancellationToken.None);
        }

        public static Task<double> Average(this IAsyncEnumerable<long> source)
        {
            if (source == null)
            {
                throw new ArgumentNullException(nameof(source));
            }

            return Average(source, CancellationToken.None);
        }

        public static Task<double?> Average(this IAsyncEnumerable<long?> source)
        {
            if (source == null)
            {
                throw new ArgumentNullException(nameof(source));
            }

            return Average(source, CancellationToken.None);
        }

        public static Task<double> Average(this IAsyncEnumerable<double> source)
        {
            if (source == null)
            {
                throw new ArgumentNullException(nameof(source));
            }

            return Average(source, CancellationToken.None);
        }

        public static Task<double?> Average(this IAsyncEnumerable<double?> source)
        {
            if (source == null)
            {
                throw new ArgumentNullException(nameof(source));
            }

            return Average(source, CancellationToken.None);
        }

        public static Task<float> Average(this IAsyncEnumerable<float> source)
        {
            if (source == null)
            {
                throw new ArgumentNullException(nameof(source));
            }

            return Average(source, CancellationToken.None);
        }

        public static Task<float?> Average(this IAsyncEnumerable<float?> source)
        {
            if (source == null)
            {
                throw new ArgumentNullException(nameof(source));
            }

            return Average(source, CancellationToken.None);
        }

        public static Task<decimal> Average(this IAsyncEnumerable<decimal> source)
        {
            if (source == null)
            {
                throw new ArgumentNullException(nameof(source));
            }

            return Average(source, CancellationToken.None);
        }

        public static Task<decimal?> Average(this IAsyncEnumerable<decimal?> source)
        {
            if (source == null)
            {
                throw new ArgumentNullException(nameof(source));
            }

            return Average(source, CancellationToken.None);
        }

        public static Task<double?> Average<TSource>(this IAsyncEnumerable<TSource> source, Func<TSource, int?> selector)
        {
            if (source == null)
            {
                throw new ArgumentNullException(nameof(source));
            }

            if (selector == null)
            {
                throw new ArgumentNullException(nameof(selector));
            }

            return Average(source, selector, CancellationToken.None);
        }

        public static Task<double> Average<TSource>(this IAsyncEnumerable<TSource> source, Func<TSource, int> selector)
        {
            if (source == null)
            {
                throw new ArgumentNullException(nameof(source));
            }

            if (selector == null)
            {
                throw new ArgumentNullException(nameof(selector));
            }

            return Average(source, selector, CancellationToken.None);
        }

        public static Task<double> Average<TSource>(this IAsyncEnumerable<TSource> source, Func<TSource, long> selector)
        {
            if (source == null)
            {
                throw new ArgumentNullException(nameof(source));
            }

            if (selector == null)
            {
                throw new ArgumentNullException(nameof(selector));
            }

            return Average(source, selector, CancellationToken.None);
        }

        public static Task<double?> Average<TSource>(this IAsyncEnumerable<TSource> source, Func<TSource, long?> selector)
        {
            if (source == null)
            {
                throw new ArgumentNullException(nameof(source));
            }

            if (selector == null)
            {
                throw new ArgumentNullException(nameof(selector));
            }

            return Average(source, selector, CancellationToken.None);
        }

        public static Task<double> Average<TSource>(this IAsyncEnumerable<TSource> source, Func<TSource, double> selector)
        {
            if (source == null)
            {
                throw new ArgumentNullException(nameof(source));
            }

            if (selector == null)
            {
                throw new ArgumentNullException(nameof(selector));
            }

            return Average(source, selector, CancellationToken.None);
        }

        public static Task<double?> Average<TSource>(this IAsyncEnumerable<TSource> source, Func<TSource, double?> selector)
        {
            if (source == null)
            {
                throw new ArgumentNullException(nameof(source));
            }

            if (selector == null)
            {
                throw new ArgumentNullException(nameof(selector));
            }

            return Average(source, selector, CancellationToken.None);
        }

        public static Task<float> Average<TSource>(this IAsyncEnumerable<TSource> source, Func<TSource, float> selector)
        {
            if (source == null)
            {
                throw new ArgumentNullException(nameof(source));
            }

            if (selector == null)
            {
                throw new ArgumentNullException(nameof(selector));
            }

            return Average(source, selector, CancellationToken.None);
        }

        public static Task<float?> Average<TSource>(this IAsyncEnumerable<TSource> source, Func<TSource, float?> selector)
        {
            if (source == null)
            {
                throw new ArgumentNullException(nameof(source));
            }

            if (selector == null)
            {
                throw new ArgumentNullException(nameof(selector));
            }

            return Average(source, selector, CancellationToken.None);
        }

        public static Task<decimal> Average<TSource>(this IAsyncEnumerable<TSource> source, Func<TSource, decimal> selector)
        {
            if (source == null)
            {
                throw new ArgumentNullException(nameof(source));
            }

            if (selector == null)
            {
                throw new ArgumentNullException(nameof(selector));
            }

            return Average(source, selector, CancellationToken.None);
        }

        public static Task<decimal?> Average<TSource>(this IAsyncEnumerable<TSource> source, Func<TSource, decimal?> selector)
        {
            if (source == null)
            {
                throw new ArgumentNullException(nameof(source));
            }

            if (selector == null)
            {
                throw new ArgumentNullException(nameof(selector));
            }

            return Average(source, selector, CancellationToken.None);
        }


        public static Task<double> Average(this IAsyncEnumerable<int> source, CancellationToken cancellationToken)
        {
            if (source == null)
            {
                throw new ArgumentNullException(nameof(source));
            }

            return Average_(source, cancellationToken);
        }

        public static Task<double?> Average(this IAsyncEnumerable<int?> source, CancellationToken cancellationToken)
        {
            if (source == null)
            {
                throw new ArgumentNullException(nameof(source));
            }

            return Average_(source, cancellationToken);
        }

        public static Task<double> Average(this IAsyncEnumerable<long> source, CancellationToken cancellationToken)
        {
            if (source == null)
            {
                throw new ArgumentNullException(nameof(source));
            }

            return Average_(source, cancellationToken);
        }

        public static Task<double?> Average(this IAsyncEnumerable<long?> source, CancellationToken cancellationToken)
        {
            if (source == null)
            {
                throw new ArgumentNullException(nameof(source));
            }

            return Average_(source, cancellationToken);
        }

        public static Task<double> Average(this IAsyncEnumerable<double> source, CancellationToken cancellationToken)
        {
            if (source == null)
            {
                throw new ArgumentNullException(nameof(source));
            }

            return Average_(source, cancellationToken);
        }

        public static Task<double?> Average(this IAsyncEnumerable<double?> source, CancellationToken cancellationToken)
        {
            if (source == null)
            {
                throw new ArgumentNullException(nameof(source));
            }

            return Average_(source, cancellationToken);
        }

        public static Task<float> Average(this IAsyncEnumerable<float> source, CancellationToken cancellationToken)
        {
            if (source == null)
            {
                throw new ArgumentNullException(nameof(source));
            }

            return Average_(source, cancellationToken);
        }

        public static Task<float?> Average(this IAsyncEnumerable<float?> source, CancellationToken cancellationToken)
        {
            if (source == null)
            {
                throw new ArgumentNullException(nameof(source));
            }

            return Average_(source, cancellationToken);
        }

        public static Task<decimal> Average(this IAsyncEnumerable<decimal> source, CancellationToken cancellationToken)
        {
            if (source == null)
            {
                throw new ArgumentNullException(nameof(source));
            }

            return Average_(source, cancellationToken);
        }

        public static Task<decimal?> Average(this IAsyncEnumerable<decimal?> source, CancellationToken cancellationToken)
        {
            if (source == null)
            {
                throw new ArgumentNullException(nameof(source));
            }

            return Average_(source, cancellationToken);
        }

        public static Task<double?> Average<TSource>(this IAsyncEnumerable<TSource> source, Func<TSource, int?> selector, CancellationToken cancellationToken)
        {
            if (source == null)
            {
                throw new ArgumentNullException(nameof(source));
            }

            if (selector == null)
            {
                throw new ArgumentNullException(nameof(selector));
            }

            return source.Select(selector)
                         .Average(cancellationToken);
        }

        public static Task<double> Average<TSource>(this IAsyncEnumerable<TSource> source, Func<TSource, int> selector, CancellationToken cancellationToken)
        {
            if (source == null)
            {
                throw new ArgumentNullException(nameof(source));
            }

            if (selector == null)
            {
                throw new ArgumentNullException(nameof(selector));
            }

            return source.Select(selector)
                         .Average(cancellationToken);
        }

        public static Task<double> Average<TSource>(this IAsyncEnumerable<TSource> source, Func<TSource, long> selector, CancellationToken cancellationToken)
        {
            if (source == null)
            {
                throw new ArgumentNullException(nameof(source));
            }

            if (selector == null)
            {
                throw new ArgumentNullException(nameof(selector));
            }

            return source.Select(selector)
                         .Average(cancellationToken);
        }

        public static Task<double?> Average<TSource>(this IAsyncEnumerable<TSource> source, Func<TSource, long?> selector, CancellationToken cancellationToken)
        {
            if (source == null)
            {
                throw new ArgumentNullException(nameof(source));
            }

            if (selector == null)
            {
                throw new ArgumentNullException(nameof(selector));
            }

            return source.Select(selector)
                         .Average(cancellationToken);
        }

        public static Task<double> Average<TSource>(this IAsyncEnumerable<TSource> source, Func<TSource, double> selector, CancellationToken cancellationToken)
        {
            if (source == null)
            {
                throw new ArgumentNullException(nameof(source));
            }

            if (selector == null)
            {
                throw new ArgumentNullException(nameof(selector));
            }

            return source.Select(selector)
                         .Average(cancellationToken);
        }

        public static Task<double?> Average<TSource>(this IAsyncEnumerable<TSource> source, Func<TSource, double?> selector, CancellationToken cancellationToken)
        {
            if (source == null)
            {
                throw new ArgumentNullException(nameof(source));
            }

            if (selector == null)
            {
                throw new ArgumentNullException(nameof(selector));
            }

            return source.Select(selector)
                         .Average(cancellationToken);
        }

        public static Task<float> Average<TSource>(this IAsyncEnumerable<TSource> source, Func<TSource, float> selector, CancellationToken cancellationToken)
        {
            if (source == null)
            {
                throw new ArgumentNullException(nameof(source));
            }

            if (selector == null)
            {
                throw new ArgumentNullException(nameof(selector));
            }

            return source.Select(selector)
                         .Average(cancellationToken);
        }

        public static Task<float?> Average<TSource>(this IAsyncEnumerable<TSource> source, Func<TSource, float?> selector, CancellationToken cancellationToken)
        {
            if (source == null)
            {
                throw new ArgumentNullException(nameof(source));
            }

            if (selector == null)
            {
                throw new ArgumentNullException(nameof(selector));
            }

            return source.Select(selector)
                         .Average(cancellationToken);
        }

        public static Task<decimal> Average<TSource>(this IAsyncEnumerable<TSource> source, Func<TSource, decimal> selector, CancellationToken cancellationToken)
        {
            if (source == null)
            {
                throw new ArgumentNullException(nameof(source));
            }

            if (selector == null)
            {
                throw new ArgumentNullException(nameof(selector));
            }

            return source.Select(selector)
                         .Average(cancellationToken);
        }

        public static Task<decimal?> Average<TSource>(this IAsyncEnumerable<TSource> source, Func<TSource, decimal?> selector, CancellationToken cancellationToken)
        {
            if (source == null)
            {
                throw new ArgumentNullException(nameof(source));
            }

            if (selector == null)
            {
                throw new ArgumentNullException(nameof(selector));
            }

            return source.Select(selector)
                         .Average(cancellationToken);
        }

        private static async Task<double> Average_(this IAsyncEnumerable<int> source, CancellationToken cancellationToken)
        {
            using (var e = source.GetEnumerator())
            {
                if (!await e.MoveNext(cancellationToken)
                            .ConfigureAwait(false))
                {
                    throw new InvalidOperationException(Strings.NO_ELEMENTS);
                }

                long sum = e.Current;
                long count = 1;
                checked
                {
                    while (await e.MoveNext(cancellationToken)
                                  .ConfigureAwait(false))
                    {
                        sum += e.Current;
                        ++count;
                    }
                }

                return (double)sum / count;
            }
        }

        private static async Task<double?> Average_(IAsyncEnumerable<int?> source, CancellationToken cancellationToken)
        {
            using (var e = source.GetEnumerator())
            {
                while (await e.MoveNext(cancellationToken)
                              .ConfigureAwait(false))
                {
                    var v = e.Current;
                    if (v.HasValue)
                    {
                        long sum = v.GetValueOrDefault();
                        long count = 1;
                        checked
                        {
                            while (await e.MoveNext(cancellationToken)
                                          .ConfigureAwait(false))
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

        private static async Task<double> Average_(IAsyncEnumerable<long> source, CancellationToken cancellationToken)
        {
            using (var e = source.GetEnumerator())
            {
                if (!await e.MoveNext(cancellationToken)
                            .ConfigureAwait(false))
                {
                    throw new InvalidOperationException(Strings.NO_ELEMENTS);
                }

                var sum = e.Current;
                long count = 1;
                checked
                {
                    while (await e.MoveNext(cancellationToken)
                                  .ConfigureAwait(false))
                    {
                        sum += e.Current;
                        ++count;
                    }
                }

                return (double)sum / count;
            }
        }

        private static async Task<double?> Average_(IAsyncEnumerable<long?> source, CancellationToken cancellationToken)
        {
            using (var e = source.GetEnumerator())
            {
                while (await e.MoveNext(cancellationToken)
                              .ConfigureAwait(false))
                {
                    var v = e.Current;
                    if (v.HasValue)
                    {
                        var sum = v.GetValueOrDefault();
                        long count = 1;
                        checked
                        {
                            while (await e.MoveNext(cancellationToken)
                                          .ConfigureAwait(false))
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

        private static async Task<double> Average_(IAsyncEnumerable<double> source, CancellationToken cancellationToken)
        {
            using (var e = source.GetEnumerator())
            {
                if (!await e.MoveNext(cancellationToken)
                            .ConfigureAwait(false))
                {
                    throw new InvalidOperationException(Strings.NO_ELEMENTS);
                }

                var sum = e.Current;
                long count = 1;
                while (await e.MoveNext(cancellationToken)
                              .ConfigureAwait(false))
                {
                    // There is an opportunity to short-circuit here, in that if e.Current is
                    // ever NaN then the result will always be NaN. Assuming that this case is
                    // rare enough that not checking is the better approach generally.
                    sum += e.Current;
                    ++count;
                }

                return sum / count;
            }
        }

        private static async Task<double?> Average_(IAsyncEnumerable<double?> source, CancellationToken cancellationToken)
        {
            using (var e = source.GetEnumerator())
            {
                while (await e.MoveNext(cancellationToken)
                              .ConfigureAwait(false))
                {
                    var v = e.Current;
                    if (v.HasValue)
                    {
                        var sum = v.GetValueOrDefault();
                        long count = 1;
                        checked
                        {
                            while (await e.MoveNext(cancellationToken)
                                          .ConfigureAwait(false))
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

        private static async Task<float> Average_(IAsyncEnumerable<float> source, CancellationToken cancellationToken)
        {
            using (var e = source.GetEnumerator())
            {
                if (!await e.MoveNext(cancellationToken)
                            .ConfigureAwait(false))
                {
                    throw new InvalidOperationException(Strings.NO_ELEMENTS);
                }

                double sum = e.Current;
                long count = 1;
                while (await e.MoveNext(cancellationToken)
                              .ConfigureAwait(false))
                {
                    sum += e.Current;
                    ++count;
                }

                return (float)(sum / count);
            }
        }

        private static async Task<float?> Average_(IAsyncEnumerable<float?> source, CancellationToken cancellationToken)
        {
            using (var e = source.GetEnumerator())
            {
                while (await e.MoveNext(cancellationToken)
                              .ConfigureAwait(false))
                {
                    var v = e.Current;
                    if (v.HasValue)
                    {
                        double sum = v.GetValueOrDefault();
                        long count = 1;
                        checked
                        {
                            while (await e.MoveNext(cancellationToken)
                                          .ConfigureAwait(false))
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

        private static async Task<decimal> Average_(IAsyncEnumerable<decimal> source, CancellationToken cancellationToken)
        {
            using (var e = source.GetEnumerator())
            {
                if (!await e.MoveNext(cancellationToken)
                            .ConfigureAwait(false))
                {
                    throw new InvalidOperationException(Strings.NO_ELEMENTS);
                }

                var sum = e.Current;
                long count = 1;
                while (await e.MoveNext(cancellationToken)
                              .ConfigureAwait(false))
                {
                    sum += e.Current;
                    ++count;
                }

                return sum / count;
            }
        }

        private static async Task<decimal?> Average_(IAsyncEnumerable<decimal?> source, CancellationToken cancellationToken)
        {
            using (var e = source.GetEnumerator())
            {
                while (await e.MoveNext(cancellationToken)
                              .ConfigureAwait(false))
                {
                    var v = e.Current;
                    if (v.HasValue)
                    {
                        var sum = v.GetValueOrDefault();
                        long count = 1;
                        while (await e.MoveNext(cancellationToken)
                                      .ConfigureAwait(false))
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

            return null;
        }
    }
}
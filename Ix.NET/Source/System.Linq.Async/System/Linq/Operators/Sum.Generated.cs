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
        public static Task<int> Sum(this IAsyncEnumerable<int> source)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));

            return SumCore(source, CancellationToken.None);
        }

        public static Task<int> Sum(this IAsyncEnumerable<int> source, CancellationToken cancellationToken)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));

            return SumCore(source, cancellationToken);
        }

        public static Task<int> Sum<TSource>(this IAsyncEnumerable<TSource> source, Func<TSource, int> selector)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));
            if (selector == null)
                throw new ArgumentNullException(nameof(selector));

            return SumCore(source, selector, CancellationToken.None);
        }

        public static Task<int> Sum<TSource>(this IAsyncEnumerable<TSource> source, Func<TSource, int> selector, CancellationToken cancellationToken)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));
            if (selector == null)
                throw new ArgumentNullException(nameof(selector));

            return SumCore(source, selector, cancellationToken);
        }

        public static Task<int> Sum<TSource>(this IAsyncEnumerable<TSource> source, Func<TSource, Task<int>> selector)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));
            if (selector == null)
                throw new ArgumentNullException(nameof(selector));

            return SumCore(source, selector, CancellationToken.None);
        }

        public static Task<int> Sum<TSource>(this IAsyncEnumerable<TSource> source, Func<TSource, Task<int>> selector, CancellationToken cancellationToken)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));
            if (selector == null)
                throw new ArgumentNullException(nameof(selector));

            return SumCore(source, selector, cancellationToken);
        }

        private static Task<int> SumCore(IAsyncEnumerable<int> source, CancellationToken cancellationToken)
        {
            return source.Aggregate(0, (x, y) => x + y, cancellationToken);
        }

        private static Task<int> SumCore<TSource>(IAsyncEnumerable<TSource> source, Func<TSource, int> selector, CancellationToken cancellationToken)
        {
            return source.Select(selector).Sum(cancellationToken);
        }

        private static Task<int> SumCore<TSource>(IAsyncEnumerable<TSource> source, Func<TSource, Task<int>> selector, CancellationToken cancellationToken)
        {
            return source.Select(selector).Sum(cancellationToken);
        }
        public static Task<long> Sum(this IAsyncEnumerable<long> source)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));

            return SumCore(source, CancellationToken.None);
        }

        public static Task<long> Sum(this IAsyncEnumerable<long> source, CancellationToken cancellationToken)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));

            return SumCore(source, cancellationToken);
        }

        public static Task<long> Sum<TSource>(this IAsyncEnumerable<TSource> source, Func<TSource, long> selector)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));
            if (selector == null)
                throw new ArgumentNullException(nameof(selector));

            return SumCore(source, selector, CancellationToken.None);
        }

        public static Task<long> Sum<TSource>(this IAsyncEnumerable<TSource> source, Func<TSource, long> selector, CancellationToken cancellationToken)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));
            if (selector == null)
                throw new ArgumentNullException(nameof(selector));

            return SumCore(source, selector, cancellationToken);
        }

        public static Task<long> Sum<TSource>(this IAsyncEnumerable<TSource> source, Func<TSource, Task<long>> selector)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));
            if (selector == null)
                throw new ArgumentNullException(nameof(selector));

            return SumCore(source, selector, CancellationToken.None);
        }

        public static Task<long> Sum<TSource>(this IAsyncEnumerable<TSource> source, Func<TSource, Task<long>> selector, CancellationToken cancellationToken)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));
            if (selector == null)
                throw new ArgumentNullException(nameof(selector));

            return SumCore(source, selector, cancellationToken);
        }

        private static Task<long> SumCore(IAsyncEnumerable<long> source, CancellationToken cancellationToken)
        {
            return source.Aggregate(0L, (x, y) => x + y, cancellationToken);
        }

        private static Task<long> SumCore<TSource>(IAsyncEnumerable<TSource> source, Func<TSource, long> selector, CancellationToken cancellationToken)
        {
            return source.Select(selector).Sum(cancellationToken);
        }

        private static Task<long> SumCore<TSource>(IAsyncEnumerable<TSource> source, Func<TSource, Task<long>> selector, CancellationToken cancellationToken)
        {
            return source.Select(selector).Sum(cancellationToken);
        }
        public static Task<float> Sum(this IAsyncEnumerable<float> source)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));

            return SumCore(source, CancellationToken.None);
        }

        public static Task<float> Sum(this IAsyncEnumerable<float> source, CancellationToken cancellationToken)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));

            return SumCore(source, cancellationToken);
        }

        public static Task<float> Sum<TSource>(this IAsyncEnumerable<TSource> source, Func<TSource, float> selector)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));
            if (selector == null)
                throw new ArgumentNullException(nameof(selector));

            return SumCore(source, selector, CancellationToken.None);
        }

        public static Task<float> Sum<TSource>(this IAsyncEnumerable<TSource> source, Func<TSource, float> selector, CancellationToken cancellationToken)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));
            if (selector == null)
                throw new ArgumentNullException(nameof(selector));

            return SumCore(source, selector, cancellationToken);
        }

        public static Task<float> Sum<TSource>(this IAsyncEnumerable<TSource> source, Func<TSource, Task<float>> selector)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));
            if (selector == null)
                throw new ArgumentNullException(nameof(selector));

            return SumCore(source, selector, CancellationToken.None);
        }

        public static Task<float> Sum<TSource>(this IAsyncEnumerable<TSource> source, Func<TSource, Task<float>> selector, CancellationToken cancellationToken)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));
            if (selector == null)
                throw new ArgumentNullException(nameof(selector));

            return SumCore(source, selector, cancellationToken);
        }

        private static Task<float> SumCore(IAsyncEnumerable<float> source, CancellationToken cancellationToken)
        {
            return source.Aggregate(0.0f, (x, y) => x + y, cancellationToken);
        }

        private static Task<float> SumCore<TSource>(IAsyncEnumerable<TSource> source, Func<TSource, float> selector, CancellationToken cancellationToken)
        {
            return source.Select(selector).Sum(cancellationToken);
        }

        private static Task<float> SumCore<TSource>(IAsyncEnumerable<TSource> source, Func<TSource, Task<float>> selector, CancellationToken cancellationToken)
        {
            return source.Select(selector).Sum(cancellationToken);
        }
        public static Task<double> Sum(this IAsyncEnumerable<double> source)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));

            return SumCore(source, CancellationToken.None);
        }

        public static Task<double> Sum(this IAsyncEnumerable<double> source, CancellationToken cancellationToken)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));

            return SumCore(source, cancellationToken);
        }

        public static Task<double> Sum<TSource>(this IAsyncEnumerable<TSource> source, Func<TSource, double> selector)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));
            if (selector == null)
                throw new ArgumentNullException(nameof(selector));

            return SumCore(source, selector, CancellationToken.None);
        }

        public static Task<double> Sum<TSource>(this IAsyncEnumerable<TSource> source, Func<TSource, double> selector, CancellationToken cancellationToken)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));
            if (selector == null)
                throw new ArgumentNullException(nameof(selector));

            return SumCore(source, selector, cancellationToken);
        }

        public static Task<double> Sum<TSource>(this IAsyncEnumerable<TSource> source, Func<TSource, Task<double>> selector)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));
            if (selector == null)
                throw new ArgumentNullException(nameof(selector));

            return SumCore(source, selector, CancellationToken.None);
        }

        public static Task<double> Sum<TSource>(this IAsyncEnumerable<TSource> source, Func<TSource, Task<double>> selector, CancellationToken cancellationToken)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));
            if (selector == null)
                throw new ArgumentNullException(nameof(selector));

            return SumCore(source, selector, cancellationToken);
        }

        private static Task<double> SumCore(IAsyncEnumerable<double> source, CancellationToken cancellationToken)
        {
            return source.Aggregate(0.0, (x, y) => x + y, cancellationToken);
        }

        private static Task<double> SumCore<TSource>(IAsyncEnumerable<TSource> source, Func<TSource, double> selector, CancellationToken cancellationToken)
        {
            return source.Select(selector).Sum(cancellationToken);
        }

        private static Task<double> SumCore<TSource>(IAsyncEnumerable<TSource> source, Func<TSource, Task<double>> selector, CancellationToken cancellationToken)
        {
            return source.Select(selector).Sum(cancellationToken);
        }
        public static Task<decimal> Sum(this IAsyncEnumerable<decimal> source)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));

            return SumCore(source, CancellationToken.None);
        }

        public static Task<decimal> Sum(this IAsyncEnumerable<decimal> source, CancellationToken cancellationToken)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));

            return SumCore(source, cancellationToken);
        }

        public static Task<decimal> Sum<TSource>(this IAsyncEnumerable<TSource> source, Func<TSource, decimal> selector)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));
            if (selector == null)
                throw new ArgumentNullException(nameof(selector));

            return SumCore(source, selector, CancellationToken.None);
        }

        public static Task<decimal> Sum<TSource>(this IAsyncEnumerable<TSource> source, Func<TSource, decimal> selector, CancellationToken cancellationToken)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));
            if (selector == null)
                throw new ArgumentNullException(nameof(selector));

            return SumCore(source, selector, cancellationToken);
        }

        public static Task<decimal> Sum<TSource>(this IAsyncEnumerable<TSource> source, Func<TSource, Task<decimal>> selector)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));
            if (selector == null)
                throw new ArgumentNullException(nameof(selector));

            return SumCore(source, selector, CancellationToken.None);
        }

        public static Task<decimal> Sum<TSource>(this IAsyncEnumerable<TSource> source, Func<TSource, Task<decimal>> selector, CancellationToken cancellationToken)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));
            if (selector == null)
                throw new ArgumentNullException(nameof(selector));

            return SumCore(source, selector, cancellationToken);
        }

        private static Task<decimal> SumCore(IAsyncEnumerable<decimal> source, CancellationToken cancellationToken)
        {
            return source.Aggregate(0m, (x, y) => x + y, cancellationToken);
        }

        private static Task<decimal> SumCore<TSource>(IAsyncEnumerable<TSource> source, Func<TSource, decimal> selector, CancellationToken cancellationToken)
        {
            return source.Select(selector).Sum(cancellationToken);
        }

        private static Task<decimal> SumCore<TSource>(IAsyncEnumerable<TSource> source, Func<TSource, Task<decimal>> selector, CancellationToken cancellationToken)
        {
            return source.Select(selector).Sum(cancellationToken);
        }
        public static Task<int?> Sum(this IAsyncEnumerable<int?> source)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));

            return SumCore(source, CancellationToken.None);
        }

        public static Task<int?> Sum(this IAsyncEnumerable<int?> source, CancellationToken cancellationToken)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));

            return SumCore(source, cancellationToken);
        }

        public static Task<int?> Sum<TSource>(this IAsyncEnumerable<TSource> source, Func<TSource, int?> selector)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));
            if (selector == null)
                throw new ArgumentNullException(nameof(selector));

            return SumCore(source, selector, CancellationToken.None);
        }

        public static Task<int?> Sum<TSource>(this IAsyncEnumerable<TSource> source, Func<TSource, int?> selector, CancellationToken cancellationToken)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));
            if (selector == null)
                throw new ArgumentNullException(nameof(selector));

            return SumCore(source, selector, cancellationToken);
        }

        public static Task<int?> Sum<TSource>(this IAsyncEnumerable<TSource> source, Func<TSource, Task<int?>> selector)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));
            if (selector == null)
                throw new ArgumentNullException(nameof(selector));

            return SumCore(source, selector, CancellationToken.None);
        }

        public static Task<int?> Sum<TSource>(this IAsyncEnumerable<TSource> source, Func<TSource, Task<int?>> selector, CancellationToken cancellationToken)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));
            if (selector == null)
                throw new ArgumentNullException(nameof(selector));

            return SumCore(source, selector, cancellationToken);
        }

        private static Task<int?> SumCore(IAsyncEnumerable<int?> source, CancellationToken cancellationToken)
        {
            return source.Aggregate((int?)0, (x, y) => x + y.GetValueOrDefault(), cancellationToken);
        }

        private static Task<int?> SumCore<TSource>(IAsyncEnumerable<TSource> source, Func<TSource, int?> selector, CancellationToken cancellationToken)
        {
            return source.Select(selector).Sum(cancellationToken);
        }

        private static Task<int?> SumCore<TSource>(IAsyncEnumerable<TSource> source, Func<TSource, Task<int?>> selector, CancellationToken cancellationToken)
        {
            return source.Select(selector).Sum(cancellationToken);
        }
        public static Task<long?> Sum(this IAsyncEnumerable<long?> source)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));

            return SumCore(source, CancellationToken.None);
        }

        public static Task<long?> Sum(this IAsyncEnumerable<long?> source, CancellationToken cancellationToken)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));

            return SumCore(source, cancellationToken);
        }

        public static Task<long?> Sum<TSource>(this IAsyncEnumerable<TSource> source, Func<TSource, long?> selector)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));
            if (selector == null)
                throw new ArgumentNullException(nameof(selector));

            return SumCore(source, selector, CancellationToken.None);
        }

        public static Task<long?> Sum<TSource>(this IAsyncEnumerable<TSource> source, Func<TSource, long?> selector, CancellationToken cancellationToken)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));
            if (selector == null)
                throw new ArgumentNullException(nameof(selector));

            return SumCore(source, selector, cancellationToken);
        }

        public static Task<long?> Sum<TSource>(this IAsyncEnumerable<TSource> source, Func<TSource, Task<long?>> selector)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));
            if (selector == null)
                throw new ArgumentNullException(nameof(selector));

            return SumCore(source, selector, CancellationToken.None);
        }

        public static Task<long?> Sum<TSource>(this IAsyncEnumerable<TSource> source, Func<TSource, Task<long?>> selector, CancellationToken cancellationToken)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));
            if (selector == null)
                throw new ArgumentNullException(nameof(selector));

            return SumCore(source, selector, cancellationToken);
        }

        private static Task<long?> SumCore(IAsyncEnumerable<long?> source, CancellationToken cancellationToken)
        {
            return source.Aggregate((long?)0L, (x, y) => x + y.GetValueOrDefault(), cancellationToken);
        }

        private static Task<long?> SumCore<TSource>(IAsyncEnumerable<TSource> source, Func<TSource, long?> selector, CancellationToken cancellationToken)
        {
            return source.Select(selector).Sum(cancellationToken);
        }

        private static Task<long?> SumCore<TSource>(IAsyncEnumerable<TSource> source, Func<TSource, Task<long?>> selector, CancellationToken cancellationToken)
        {
            return source.Select(selector).Sum(cancellationToken);
        }
        public static Task<float?> Sum(this IAsyncEnumerable<float?> source)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));

            return SumCore(source, CancellationToken.None);
        }

        public static Task<float?> Sum(this IAsyncEnumerable<float?> source, CancellationToken cancellationToken)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));

            return SumCore(source, cancellationToken);
        }

        public static Task<float?> Sum<TSource>(this IAsyncEnumerable<TSource> source, Func<TSource, float?> selector)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));
            if (selector == null)
                throw new ArgumentNullException(nameof(selector));

            return SumCore(source, selector, CancellationToken.None);
        }

        public static Task<float?> Sum<TSource>(this IAsyncEnumerable<TSource> source, Func<TSource, float?> selector, CancellationToken cancellationToken)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));
            if (selector == null)
                throw new ArgumentNullException(nameof(selector));

            return SumCore(source, selector, cancellationToken);
        }

        public static Task<float?> Sum<TSource>(this IAsyncEnumerable<TSource> source, Func<TSource, Task<float?>> selector)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));
            if (selector == null)
                throw new ArgumentNullException(nameof(selector));

            return SumCore(source, selector, CancellationToken.None);
        }

        public static Task<float?> Sum<TSource>(this IAsyncEnumerable<TSource> source, Func<TSource, Task<float?>> selector, CancellationToken cancellationToken)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));
            if (selector == null)
                throw new ArgumentNullException(nameof(selector));

            return SumCore(source, selector, cancellationToken);
        }

        private static Task<float?> SumCore(IAsyncEnumerable<float?> source, CancellationToken cancellationToken)
        {
            return source.Aggregate((float?)0.0f, (x, y) => x + y.GetValueOrDefault(), cancellationToken);
        }

        private static Task<float?> SumCore<TSource>(IAsyncEnumerable<TSource> source, Func<TSource, float?> selector, CancellationToken cancellationToken)
        {
            return source.Select(selector).Sum(cancellationToken);
        }

        private static Task<float?> SumCore<TSource>(IAsyncEnumerable<TSource> source, Func<TSource, Task<float?>> selector, CancellationToken cancellationToken)
        {
            return source.Select(selector).Sum(cancellationToken);
        }
        public static Task<double?> Sum(this IAsyncEnumerable<double?> source)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));

            return SumCore(source, CancellationToken.None);
        }

        public static Task<double?> Sum(this IAsyncEnumerable<double?> source, CancellationToken cancellationToken)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));

            return SumCore(source, cancellationToken);
        }

        public static Task<double?> Sum<TSource>(this IAsyncEnumerable<TSource> source, Func<TSource, double?> selector)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));
            if (selector == null)
                throw new ArgumentNullException(nameof(selector));

            return SumCore(source, selector, CancellationToken.None);
        }

        public static Task<double?> Sum<TSource>(this IAsyncEnumerable<TSource> source, Func<TSource, double?> selector, CancellationToken cancellationToken)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));
            if (selector == null)
                throw new ArgumentNullException(nameof(selector));

            return SumCore(source, selector, cancellationToken);
        }

        public static Task<double?> Sum<TSource>(this IAsyncEnumerable<TSource> source, Func<TSource, Task<double?>> selector)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));
            if (selector == null)
                throw new ArgumentNullException(nameof(selector));

            return SumCore(source, selector, CancellationToken.None);
        }

        public static Task<double?> Sum<TSource>(this IAsyncEnumerable<TSource> source, Func<TSource, Task<double?>> selector, CancellationToken cancellationToken)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));
            if (selector == null)
                throw new ArgumentNullException(nameof(selector));

            return SumCore(source, selector, cancellationToken);
        }

        private static Task<double?> SumCore(IAsyncEnumerable<double?> source, CancellationToken cancellationToken)
        {
            return source.Aggregate((double?)0.0, (x, y) => x + y.GetValueOrDefault(), cancellationToken);
        }

        private static Task<double?> SumCore<TSource>(IAsyncEnumerable<TSource> source, Func<TSource, double?> selector, CancellationToken cancellationToken)
        {
            return source.Select(selector).Sum(cancellationToken);
        }

        private static Task<double?> SumCore<TSource>(IAsyncEnumerable<TSource> source, Func<TSource, Task<double?>> selector, CancellationToken cancellationToken)
        {
            return source.Select(selector).Sum(cancellationToken);
        }
        public static Task<decimal?> Sum(this IAsyncEnumerable<decimal?> source)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));

            return SumCore(source, CancellationToken.None);
        }

        public static Task<decimal?> Sum(this IAsyncEnumerable<decimal?> source, CancellationToken cancellationToken)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));

            return SumCore(source, cancellationToken);
        }

        public static Task<decimal?> Sum<TSource>(this IAsyncEnumerable<TSource> source, Func<TSource, decimal?> selector)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));
            if (selector == null)
                throw new ArgumentNullException(nameof(selector));

            return SumCore(source, selector, CancellationToken.None);
        }

        public static Task<decimal?> Sum<TSource>(this IAsyncEnumerable<TSource> source, Func<TSource, decimal?> selector, CancellationToken cancellationToken)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));
            if (selector == null)
                throw new ArgumentNullException(nameof(selector));

            return SumCore(source, selector, cancellationToken);
        }

        public static Task<decimal?> Sum<TSource>(this IAsyncEnumerable<TSource> source, Func<TSource, Task<decimal?>> selector)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));
            if (selector == null)
                throw new ArgumentNullException(nameof(selector));

            return SumCore(source, selector, CancellationToken.None);
        }

        public static Task<decimal?> Sum<TSource>(this IAsyncEnumerable<TSource> source, Func<TSource, Task<decimal?>> selector, CancellationToken cancellationToken)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));
            if (selector == null)
                throw new ArgumentNullException(nameof(selector));

            return SumCore(source, selector, cancellationToken);
        }

        private static Task<decimal?> SumCore(IAsyncEnumerable<decimal?> source, CancellationToken cancellationToken)
        {
            return source.Aggregate((decimal?)0m, (x, y) => x + y.GetValueOrDefault(), cancellationToken);
        }

        private static Task<decimal?> SumCore<TSource>(IAsyncEnumerable<TSource> source, Func<TSource, decimal?> selector, CancellationToken cancellationToken)
        {
            return source.Select(selector).Sum(cancellationToken);
        }

        private static Task<decimal?> SumCore<TSource>(IAsyncEnumerable<TSource> source, Func<TSource, Task<decimal?>> selector, CancellationToken cancellationToken)
        {
            return source.Select(selector).Sum(cancellationToken);
        }
    }
}

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
                throw Error.ArgumentNull(nameof(source));

            return AverageCore(source, CancellationToken.None);
        }

        public static Task<double> Average(this IAsyncEnumerable<int> source, CancellationToken cancellationToken)
        {
            if (source == null)
                throw Error.ArgumentNull(nameof(source));

            return AverageCore(source, cancellationToken);
        }

        public static Task<double> Average<TSource>(this IAsyncEnumerable<TSource> source, Func<TSource, int> selector)
        {
            if (source == null)
                throw Error.ArgumentNull(nameof(source));
            if (selector == null)
                throw Error.ArgumentNull(nameof(selector));

            return source.Select(selector).Average(CancellationToken.None);
        }

        public static Task<double> Average<TSource>(this IAsyncEnumerable<TSource> source, Func<TSource, int> selector, CancellationToken cancellationToken)
        {
            if (source == null)
                throw Error.ArgumentNull(nameof(source));
            if (selector == null)
                throw Error.ArgumentNull(nameof(selector));

            return source.Select(selector).Average(cancellationToken);
        }

        public static Task<double> Average<TSource>(this IAsyncEnumerable<TSource> source, Func<TSource, ValueTask<int>> selector)
        {
            if (source == null)
                throw Error.ArgumentNull(nameof(source));
            if (selector == null)
                throw Error.ArgumentNull(nameof(selector));

            return source.Select(selector).Average(CancellationToken.None);
        }

        public static Task<double> Average<TSource>(this IAsyncEnumerable<TSource> source, Func<TSource, ValueTask<int>> selector, CancellationToken cancellationToken)
        {
            if (source == null)
                throw Error.ArgumentNull(nameof(source));
            if (selector == null)
                throw Error.ArgumentNull(nameof(selector));

            return source.Select(selector).Average(cancellationToken);
        }

        public static Task<double> Average(this IAsyncEnumerable<long> source)
        {
            if (source == null)
                throw Error.ArgumentNull(nameof(source));

            return AverageCore(source, CancellationToken.None);
        }

        public static Task<double> Average(this IAsyncEnumerable<long> source, CancellationToken cancellationToken)
        {
            if (source == null)
                throw Error.ArgumentNull(nameof(source));

            return AverageCore(source, cancellationToken);
        }

        public static Task<double> Average<TSource>(this IAsyncEnumerable<TSource> source, Func<TSource, long> selector)
        {
            if (source == null)
                throw Error.ArgumentNull(nameof(source));
            if (selector == null)
                throw Error.ArgumentNull(nameof(selector));

            return source.Select(selector).Average(CancellationToken.None);
        }

        public static Task<double> Average<TSource>(this IAsyncEnumerable<TSource> source, Func<TSource, long> selector, CancellationToken cancellationToken)
        {
            if (source == null)
                throw Error.ArgumentNull(nameof(source));
            if (selector == null)
                throw Error.ArgumentNull(nameof(selector));

            return source.Select(selector).Average(cancellationToken);
        }

        public static Task<double> Average<TSource>(this IAsyncEnumerable<TSource> source, Func<TSource, ValueTask<long>> selector)
        {
            if (source == null)
                throw Error.ArgumentNull(nameof(source));
            if (selector == null)
                throw Error.ArgumentNull(nameof(selector));

            return source.Select(selector).Average(CancellationToken.None);
        }

        public static Task<double> Average<TSource>(this IAsyncEnumerable<TSource> source, Func<TSource, ValueTask<long>> selector, CancellationToken cancellationToken)
        {
            if (source == null)
                throw Error.ArgumentNull(nameof(source));
            if (selector == null)
                throw Error.ArgumentNull(nameof(selector));

            return source.Select(selector).Average(cancellationToken);
        }

        public static Task<float> Average(this IAsyncEnumerable<float> source)
        {
            if (source == null)
                throw Error.ArgumentNull(nameof(source));

            return AverageCore(source, CancellationToken.None);
        }

        public static Task<float> Average(this IAsyncEnumerable<float> source, CancellationToken cancellationToken)
        {
            if (source == null)
                throw Error.ArgumentNull(nameof(source));

            return AverageCore(source, cancellationToken);
        }

        public static Task<float> Average<TSource>(this IAsyncEnumerable<TSource> source, Func<TSource, float> selector)
        {
            if (source == null)
                throw Error.ArgumentNull(nameof(source));
            if (selector == null)
                throw Error.ArgumentNull(nameof(selector));

            return source.Select(selector).Average(CancellationToken.None);
        }

        public static Task<float> Average<TSource>(this IAsyncEnumerable<TSource> source, Func<TSource, float> selector, CancellationToken cancellationToken)
        {
            if (source == null)
                throw Error.ArgumentNull(nameof(source));
            if (selector == null)
                throw Error.ArgumentNull(nameof(selector));

            return source.Select(selector).Average(cancellationToken);
        }

        public static Task<float> Average<TSource>(this IAsyncEnumerable<TSource> source, Func<TSource, ValueTask<float>> selector)
        {
            if (source == null)
                throw Error.ArgumentNull(nameof(source));
            if (selector == null)
                throw Error.ArgumentNull(nameof(selector));

            return source.Select(selector).Average(CancellationToken.None);
        }

        public static Task<float> Average<TSource>(this IAsyncEnumerable<TSource> source, Func<TSource, ValueTask<float>> selector, CancellationToken cancellationToken)
        {
            if (source == null)
                throw Error.ArgumentNull(nameof(source));
            if (selector == null)
                throw Error.ArgumentNull(nameof(selector));

            return source.Select(selector).Average(cancellationToken);
        }

        public static Task<double> Average(this IAsyncEnumerable<double> source)
        {
            if (source == null)
                throw Error.ArgumentNull(nameof(source));

            return AverageCore(source, CancellationToken.None);
        }

        public static Task<double> Average(this IAsyncEnumerable<double> source, CancellationToken cancellationToken)
        {
            if (source == null)
                throw Error.ArgumentNull(nameof(source));

            return AverageCore(source, cancellationToken);
        }

        public static Task<double> Average<TSource>(this IAsyncEnumerable<TSource> source, Func<TSource, double> selector)
        {
            if (source == null)
                throw Error.ArgumentNull(nameof(source));
            if (selector == null)
                throw Error.ArgumentNull(nameof(selector));

            return source.Select(selector).Average(CancellationToken.None);
        }

        public static Task<double> Average<TSource>(this IAsyncEnumerable<TSource> source, Func<TSource, double> selector, CancellationToken cancellationToken)
        {
            if (source == null)
                throw Error.ArgumentNull(nameof(source));
            if (selector == null)
                throw Error.ArgumentNull(nameof(selector));

            return source.Select(selector).Average(cancellationToken);
        }

        public static Task<double> Average<TSource>(this IAsyncEnumerable<TSource> source, Func<TSource, ValueTask<double>> selector)
        {
            if (source == null)
                throw Error.ArgumentNull(nameof(source));
            if (selector == null)
                throw Error.ArgumentNull(nameof(selector));

            return source.Select(selector).Average(CancellationToken.None);
        }

        public static Task<double> Average<TSource>(this IAsyncEnumerable<TSource> source, Func<TSource, ValueTask<double>> selector, CancellationToken cancellationToken)
        {
            if (source == null)
                throw Error.ArgumentNull(nameof(source));
            if (selector == null)
                throw Error.ArgumentNull(nameof(selector));

            return source.Select(selector).Average(cancellationToken);
        }

        public static Task<decimal> Average(this IAsyncEnumerable<decimal> source)
        {
            if (source == null)
                throw Error.ArgumentNull(nameof(source));

            return AverageCore(source, CancellationToken.None);
        }

        public static Task<decimal> Average(this IAsyncEnumerable<decimal> source, CancellationToken cancellationToken)
        {
            if (source == null)
                throw Error.ArgumentNull(nameof(source));

            return AverageCore(source, cancellationToken);
        }

        public static Task<decimal> Average<TSource>(this IAsyncEnumerable<TSource> source, Func<TSource, decimal> selector)
        {
            if (source == null)
                throw Error.ArgumentNull(nameof(source));
            if (selector == null)
                throw Error.ArgumentNull(nameof(selector));

            return source.Select(selector).Average(CancellationToken.None);
        }

        public static Task<decimal> Average<TSource>(this IAsyncEnumerable<TSource> source, Func<TSource, decimal> selector, CancellationToken cancellationToken)
        {
            if (source == null)
                throw Error.ArgumentNull(nameof(source));
            if (selector == null)
                throw Error.ArgumentNull(nameof(selector));

            return source.Select(selector).Average(cancellationToken);
        }

        public static Task<decimal> Average<TSource>(this IAsyncEnumerable<TSource> source, Func<TSource, ValueTask<decimal>> selector)
        {
            if (source == null)
                throw Error.ArgumentNull(nameof(source));
            if (selector == null)
                throw Error.ArgumentNull(nameof(selector));

            return source.Select(selector).Average(CancellationToken.None);
        }

        public static Task<decimal> Average<TSource>(this IAsyncEnumerable<TSource> source, Func<TSource, ValueTask<decimal>> selector, CancellationToken cancellationToken)
        {
            if (source == null)
                throw Error.ArgumentNull(nameof(source));
            if (selector == null)
                throw Error.ArgumentNull(nameof(selector));

            return source.Select(selector).Average(cancellationToken);
        }

        public static Task<double?> Average(this IAsyncEnumerable<int?> source)
        {
            if (source == null)
                throw Error.ArgumentNull(nameof(source));

            return AverageCore(source, CancellationToken.None);
        }

        public static Task<double?> Average(this IAsyncEnumerable<int?> source, CancellationToken cancellationToken)
        {
            if (source == null)
                throw Error.ArgumentNull(nameof(source));

            return AverageCore(source, cancellationToken);
        }

        public static Task<double?> Average<TSource>(this IAsyncEnumerable<TSource> source, Func<TSource, int?> selector)
        {
            if (source == null)
                throw Error.ArgumentNull(nameof(source));
            if (selector == null)
                throw Error.ArgumentNull(nameof(selector));

            return source.Select(selector).Average(CancellationToken.None);
        }

        public static Task<double?> Average<TSource>(this IAsyncEnumerable<TSource> source, Func<TSource, int?> selector, CancellationToken cancellationToken)
        {
            if (source == null)
                throw Error.ArgumentNull(nameof(source));
            if (selector == null)
                throw Error.ArgumentNull(nameof(selector));

            return source.Select(selector).Average(cancellationToken);
        }

        public static Task<double?> Average<TSource>(this IAsyncEnumerable<TSource> source, Func<TSource, ValueTask<int?>> selector)
        {
            if (source == null)
                throw Error.ArgumentNull(nameof(source));
            if (selector == null)
                throw Error.ArgumentNull(nameof(selector));

            return source.Select(selector).Average(CancellationToken.None);
        }

        public static Task<double?> Average<TSource>(this IAsyncEnumerable<TSource> source, Func<TSource, ValueTask<int?>> selector, CancellationToken cancellationToken)
        {
            if (source == null)
                throw Error.ArgumentNull(nameof(source));
            if (selector == null)
                throw Error.ArgumentNull(nameof(selector));

            return source.Select(selector).Average(cancellationToken);
        }

        public static Task<double?> Average(this IAsyncEnumerable<long?> source)
        {
            if (source == null)
                throw Error.ArgumentNull(nameof(source));

            return AverageCore(source, CancellationToken.None);
        }

        public static Task<double?> Average(this IAsyncEnumerable<long?> source, CancellationToken cancellationToken)
        {
            if (source == null)
                throw Error.ArgumentNull(nameof(source));

            return AverageCore(source, cancellationToken);
        }

        public static Task<double?> Average<TSource>(this IAsyncEnumerable<TSource> source, Func<TSource, long?> selector)
        {
            if (source == null)
                throw Error.ArgumentNull(nameof(source));
            if (selector == null)
                throw Error.ArgumentNull(nameof(selector));

            return source.Select(selector).Average(CancellationToken.None);
        }

        public static Task<double?> Average<TSource>(this IAsyncEnumerable<TSource> source, Func<TSource, long?> selector, CancellationToken cancellationToken)
        {
            if (source == null)
                throw Error.ArgumentNull(nameof(source));
            if (selector == null)
                throw Error.ArgumentNull(nameof(selector));

            return source.Select(selector).Average(cancellationToken);
        }

        public static Task<double?> Average<TSource>(this IAsyncEnumerable<TSource> source, Func<TSource, ValueTask<long?>> selector)
        {
            if (source == null)
                throw Error.ArgumentNull(nameof(source));
            if (selector == null)
                throw Error.ArgumentNull(nameof(selector));

            return source.Select(selector).Average(CancellationToken.None);
        }

        public static Task<double?> Average<TSource>(this IAsyncEnumerable<TSource> source, Func<TSource, ValueTask<long?>> selector, CancellationToken cancellationToken)
        {
            if (source == null)
                throw Error.ArgumentNull(nameof(source));
            if (selector == null)
                throw Error.ArgumentNull(nameof(selector));

            return source.Select(selector).Average(cancellationToken);
        }

        public static Task<float?> Average(this IAsyncEnumerable<float?> source)
        {
            if (source == null)
                throw Error.ArgumentNull(nameof(source));

            return AverageCore(source, CancellationToken.None);
        }

        public static Task<float?> Average(this IAsyncEnumerable<float?> source, CancellationToken cancellationToken)
        {
            if (source == null)
                throw Error.ArgumentNull(nameof(source));

            return AverageCore(source, cancellationToken);
        }

        public static Task<float?> Average<TSource>(this IAsyncEnumerable<TSource> source, Func<TSource, float?> selector)
        {
            if (source == null)
                throw Error.ArgumentNull(nameof(source));
            if (selector == null)
                throw Error.ArgumentNull(nameof(selector));

            return source.Select(selector).Average(CancellationToken.None);
        }

        public static Task<float?> Average<TSource>(this IAsyncEnumerable<TSource> source, Func<TSource, float?> selector, CancellationToken cancellationToken)
        {
            if (source == null)
                throw Error.ArgumentNull(nameof(source));
            if (selector == null)
                throw Error.ArgumentNull(nameof(selector));

            return source.Select(selector).Average(cancellationToken);
        }

        public static Task<float?> Average<TSource>(this IAsyncEnumerable<TSource> source, Func<TSource, ValueTask<float?>> selector)
        {
            if (source == null)
                throw Error.ArgumentNull(nameof(source));
            if (selector == null)
                throw Error.ArgumentNull(nameof(selector));

            return source.Select(selector).Average(CancellationToken.None);
        }

        public static Task<float?> Average<TSource>(this IAsyncEnumerable<TSource> source, Func<TSource, ValueTask<float?>> selector, CancellationToken cancellationToken)
        {
            if (source == null)
                throw Error.ArgumentNull(nameof(source));
            if (selector == null)
                throw Error.ArgumentNull(nameof(selector));

            return source.Select(selector).Average(cancellationToken);
        }

        public static Task<double?> Average(this IAsyncEnumerable<double?> source)
        {
            if (source == null)
                throw Error.ArgumentNull(nameof(source));

            return AverageCore(source, CancellationToken.None);
        }

        public static Task<double?> Average(this IAsyncEnumerable<double?> source, CancellationToken cancellationToken)
        {
            if (source == null)
                throw Error.ArgumentNull(nameof(source));

            return AverageCore(source, cancellationToken);
        }

        public static Task<double?> Average<TSource>(this IAsyncEnumerable<TSource> source, Func<TSource, double?> selector)
        {
            if (source == null)
                throw Error.ArgumentNull(nameof(source));
            if (selector == null)
                throw Error.ArgumentNull(nameof(selector));

            return source.Select(selector).Average(CancellationToken.None);
        }

        public static Task<double?> Average<TSource>(this IAsyncEnumerable<TSource> source, Func<TSource, double?> selector, CancellationToken cancellationToken)
        {
            if (source == null)
                throw Error.ArgumentNull(nameof(source));
            if (selector == null)
                throw Error.ArgumentNull(nameof(selector));

            return source.Select(selector).Average(cancellationToken);
        }

        public static Task<double?> Average<TSource>(this IAsyncEnumerable<TSource> source, Func<TSource, ValueTask<double?>> selector)
        {
            if (source == null)
                throw Error.ArgumentNull(nameof(source));
            if (selector == null)
                throw Error.ArgumentNull(nameof(selector));

            return source.Select(selector).Average(CancellationToken.None);
        }

        public static Task<double?> Average<TSource>(this IAsyncEnumerable<TSource> source, Func<TSource, ValueTask<double?>> selector, CancellationToken cancellationToken)
        {
            if (source == null)
                throw Error.ArgumentNull(nameof(source));
            if (selector == null)
                throw Error.ArgumentNull(nameof(selector));

            return source.Select(selector).Average(cancellationToken);
        }

        public static Task<decimal?> Average(this IAsyncEnumerable<decimal?> source)
        {
            if (source == null)
                throw Error.ArgumentNull(nameof(source));

            return AverageCore(source, CancellationToken.None);
        }

        public static Task<decimal?> Average(this IAsyncEnumerable<decimal?> source, CancellationToken cancellationToken)
        {
            if (source == null)
                throw Error.ArgumentNull(nameof(source));

            return AverageCore(source, cancellationToken);
        }

        public static Task<decimal?> Average<TSource>(this IAsyncEnumerable<TSource> source, Func<TSource, decimal?> selector)
        {
            if (source == null)
                throw Error.ArgumentNull(nameof(source));
            if (selector == null)
                throw Error.ArgumentNull(nameof(selector));

            return source.Select(selector).Average(CancellationToken.None);
        }

        public static Task<decimal?> Average<TSource>(this IAsyncEnumerable<TSource> source, Func<TSource, decimal?> selector, CancellationToken cancellationToken)
        {
            if (source == null)
                throw Error.ArgumentNull(nameof(source));
            if (selector == null)
                throw Error.ArgumentNull(nameof(selector));

            return source.Select(selector).Average(cancellationToken);
        }

        public static Task<decimal?> Average<TSource>(this IAsyncEnumerable<TSource> source, Func<TSource, ValueTask<decimal?>> selector)
        {
            if (source == null)
                throw Error.ArgumentNull(nameof(source));
            if (selector == null)
                throw Error.ArgumentNull(nameof(selector));

            return source.Select(selector).Average(CancellationToken.None);
        }

        public static Task<decimal?> Average<TSource>(this IAsyncEnumerable<TSource> source, Func<TSource, ValueTask<decimal?>> selector, CancellationToken cancellationToken)
        {
            if (source == null)
                throw Error.ArgumentNull(nameof(source));
            if (selector == null)
                throw Error.ArgumentNull(nameof(selector));

            return source.Select(selector).Average(cancellationToken);
        }

    }
}

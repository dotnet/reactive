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
        public static Task<int> Max(this IAsyncEnumerable<int> source)
        {
            if (source == null)
                throw Error.ArgumentNull(nameof(source));

            return MaxCore(source, CancellationToken.None);
        }

        public static Task<int> Max(this IAsyncEnumerable<int> source, CancellationToken cancellationToken)
        {
            if (source == null)
                throw Error.ArgumentNull(nameof(source));

            return MaxCore(source, cancellationToken);
        }

        public static Task<int> Max<TSource>(this IAsyncEnumerable<TSource> source, Func<TSource, int> selector)
        {
            if (source == null)
                throw Error.ArgumentNull(nameof(source));
            if (selector == null)
                throw Error.ArgumentNull(nameof(selector));

            return MaxCore(source, selector, CancellationToken.None);
        }

        public static Task<int> Max<TSource>(this IAsyncEnumerable<TSource> source, Func<TSource, int> selector, CancellationToken cancellationToken)
        {
            if (source == null)
                throw Error.ArgumentNull(nameof(source));
            if (selector == null)
                throw Error.ArgumentNull(nameof(selector));

            return MaxCore(source, selector, cancellationToken);
        }

        public static Task<int> Max<TSource>(this IAsyncEnumerable<TSource> source, Func<TSource, Task<int>> selector)
        {
            if (source == null)
                throw Error.ArgumentNull(nameof(source));
            if (selector == null)
                throw Error.ArgumentNull(nameof(selector));

            return MaxCore(source, selector, CancellationToken.None);
        }

        public static Task<int> Max<TSource>(this IAsyncEnumerable<TSource> source, Func<TSource, Task<int>> selector, CancellationToken cancellationToken)
        {
            if (source == null)
                throw Error.ArgumentNull(nameof(source));
            if (selector == null)
                throw Error.ArgumentNull(nameof(selector));

            return MaxCore(source, selector, cancellationToken);
        }

        public static Task<int?> Max(this IAsyncEnumerable<int?> source)
        {
            if (source == null)
                throw Error.ArgumentNull(nameof(source));

            return MaxCore(source, CancellationToken.None);
        }

        public static Task<int?> Max(this IAsyncEnumerable<int?> source, CancellationToken cancellationToken)
        {
            if (source == null)
                throw Error.ArgumentNull(nameof(source));

            return MaxCore(source, cancellationToken);
        }

        public static Task<int?> Max<TSource>(this IAsyncEnumerable<TSource> source, Func<TSource, int?> selector)
        {
            if (source == null)
                throw Error.ArgumentNull(nameof(source));
            if (selector == null)
                throw Error.ArgumentNull(nameof(selector));

            return MaxCore(source, selector, CancellationToken.None);
        }

        public static Task<int?> Max<TSource>(this IAsyncEnumerable<TSource> source, Func<TSource, int?> selector, CancellationToken cancellationToken)
        {
            if (source == null)
                throw Error.ArgumentNull(nameof(source));
            if (selector == null)
                throw Error.ArgumentNull(nameof(selector));

            return MaxCore(source, selector, cancellationToken);
        }

        public static Task<int?> Max<TSource>(this IAsyncEnumerable<TSource> source, Func<TSource, Task<int?>> selector)
        {
            if (source == null)
                throw Error.ArgumentNull(nameof(source));
            if (selector == null)
                throw Error.ArgumentNull(nameof(selector));

            return MaxCore(source, selector, CancellationToken.None);
        }

        public static Task<int?> Max<TSource>(this IAsyncEnumerable<TSource> source, Func<TSource, Task<int?>> selector, CancellationToken cancellationToken)
        {
            if (source == null)
                throw Error.ArgumentNull(nameof(source));
            if (selector == null)
                throw Error.ArgumentNull(nameof(selector));

            return MaxCore(source, selector, cancellationToken);
        }

        public static Task<long> Max(this IAsyncEnumerable<long> source)
        {
            if (source == null)
                throw Error.ArgumentNull(nameof(source));

            return MaxCore(source, CancellationToken.None);
        }

        public static Task<long> Max(this IAsyncEnumerable<long> source, CancellationToken cancellationToken)
        {
            if (source == null)
                throw Error.ArgumentNull(nameof(source));

            return MaxCore(source, cancellationToken);
        }

        public static Task<long> Max<TSource>(this IAsyncEnumerable<TSource> source, Func<TSource, long> selector)
        {
            if (source == null)
                throw Error.ArgumentNull(nameof(source));
            if (selector == null)
                throw Error.ArgumentNull(nameof(selector));

            return MaxCore(source, selector, CancellationToken.None);
        }

        public static Task<long> Max<TSource>(this IAsyncEnumerable<TSource> source, Func<TSource, long> selector, CancellationToken cancellationToken)
        {
            if (source == null)
                throw Error.ArgumentNull(nameof(source));
            if (selector == null)
                throw Error.ArgumentNull(nameof(selector));

            return MaxCore(source, selector, cancellationToken);
        }

        public static Task<long> Max<TSource>(this IAsyncEnumerable<TSource> source, Func<TSource, Task<long>> selector)
        {
            if (source == null)
                throw Error.ArgumentNull(nameof(source));
            if (selector == null)
                throw Error.ArgumentNull(nameof(selector));

            return MaxCore(source, selector, CancellationToken.None);
        }

        public static Task<long> Max<TSource>(this IAsyncEnumerable<TSource> source, Func<TSource, Task<long>> selector, CancellationToken cancellationToken)
        {
            if (source == null)
                throw Error.ArgumentNull(nameof(source));
            if (selector == null)
                throw Error.ArgumentNull(nameof(selector));

            return MaxCore(source, selector, cancellationToken);
        }

        public static Task<long?> Max(this IAsyncEnumerable<long?> source)
        {
            if (source == null)
                throw Error.ArgumentNull(nameof(source));

            return MaxCore(source, CancellationToken.None);
        }

        public static Task<long?> Max(this IAsyncEnumerable<long?> source, CancellationToken cancellationToken)
        {
            if (source == null)
                throw Error.ArgumentNull(nameof(source));

            return MaxCore(source, cancellationToken);
        }

        public static Task<long?> Max<TSource>(this IAsyncEnumerable<TSource> source, Func<TSource, long?> selector)
        {
            if (source == null)
                throw Error.ArgumentNull(nameof(source));
            if (selector == null)
                throw Error.ArgumentNull(nameof(selector));

            return MaxCore(source, selector, CancellationToken.None);
        }

        public static Task<long?> Max<TSource>(this IAsyncEnumerable<TSource> source, Func<TSource, long?> selector, CancellationToken cancellationToken)
        {
            if (source == null)
                throw Error.ArgumentNull(nameof(source));
            if (selector == null)
                throw Error.ArgumentNull(nameof(selector));

            return MaxCore(source, selector, cancellationToken);
        }

        public static Task<long?> Max<TSource>(this IAsyncEnumerable<TSource> source, Func<TSource, Task<long?>> selector)
        {
            if (source == null)
                throw Error.ArgumentNull(nameof(source));
            if (selector == null)
                throw Error.ArgumentNull(nameof(selector));

            return MaxCore(source, selector, CancellationToken.None);
        }

        public static Task<long?> Max<TSource>(this IAsyncEnumerable<TSource> source, Func<TSource, Task<long?>> selector, CancellationToken cancellationToken)
        {
            if (source == null)
                throw Error.ArgumentNull(nameof(source));
            if (selector == null)
                throw Error.ArgumentNull(nameof(selector));

            return MaxCore(source, selector, cancellationToken);
        }

        public static Task<float> Max(this IAsyncEnumerable<float> source)
        {
            if (source == null)
                throw Error.ArgumentNull(nameof(source));

            return MaxCore(source, CancellationToken.None);
        }

        public static Task<float> Max(this IAsyncEnumerable<float> source, CancellationToken cancellationToken)
        {
            if (source == null)
                throw Error.ArgumentNull(nameof(source));

            return MaxCore(source, cancellationToken);
        }

        public static Task<float> Max<TSource>(this IAsyncEnumerable<TSource> source, Func<TSource, float> selector)
        {
            if (source == null)
                throw Error.ArgumentNull(nameof(source));
            if (selector == null)
                throw Error.ArgumentNull(nameof(selector));

            return MaxCore(source, selector, CancellationToken.None);
        }

        public static Task<float> Max<TSource>(this IAsyncEnumerable<TSource> source, Func<TSource, float> selector, CancellationToken cancellationToken)
        {
            if (source == null)
                throw Error.ArgumentNull(nameof(source));
            if (selector == null)
                throw Error.ArgumentNull(nameof(selector));

            return MaxCore(source, selector, cancellationToken);
        }

        public static Task<float> Max<TSource>(this IAsyncEnumerable<TSource> source, Func<TSource, Task<float>> selector)
        {
            if (source == null)
                throw Error.ArgumentNull(nameof(source));
            if (selector == null)
                throw Error.ArgumentNull(nameof(selector));

            return MaxCore(source, selector, CancellationToken.None);
        }

        public static Task<float> Max<TSource>(this IAsyncEnumerable<TSource> source, Func<TSource, Task<float>> selector, CancellationToken cancellationToken)
        {
            if (source == null)
                throw Error.ArgumentNull(nameof(source));
            if (selector == null)
                throw Error.ArgumentNull(nameof(selector));

            return MaxCore(source, selector, cancellationToken);
        }

        public static Task<float?> Max(this IAsyncEnumerable<float?> source)
        {
            if (source == null)
                throw Error.ArgumentNull(nameof(source));

            return MaxCore(source, CancellationToken.None);
        }

        public static Task<float?> Max(this IAsyncEnumerable<float?> source, CancellationToken cancellationToken)
        {
            if (source == null)
                throw Error.ArgumentNull(nameof(source));

            return MaxCore(source, cancellationToken);
        }

        public static Task<float?> Max<TSource>(this IAsyncEnumerable<TSource> source, Func<TSource, float?> selector)
        {
            if (source == null)
                throw Error.ArgumentNull(nameof(source));
            if (selector == null)
                throw Error.ArgumentNull(nameof(selector));

            return MaxCore(source, selector, CancellationToken.None);
        }

        public static Task<float?> Max<TSource>(this IAsyncEnumerable<TSource> source, Func<TSource, float?> selector, CancellationToken cancellationToken)
        {
            if (source == null)
                throw Error.ArgumentNull(nameof(source));
            if (selector == null)
                throw Error.ArgumentNull(nameof(selector));

            return MaxCore(source, selector, cancellationToken);
        }

        public static Task<float?> Max<TSource>(this IAsyncEnumerable<TSource> source, Func<TSource, Task<float?>> selector)
        {
            if (source == null)
                throw Error.ArgumentNull(nameof(source));
            if (selector == null)
                throw Error.ArgumentNull(nameof(selector));

            return MaxCore(source, selector, CancellationToken.None);
        }

        public static Task<float?> Max<TSource>(this IAsyncEnumerable<TSource> source, Func<TSource, Task<float?>> selector, CancellationToken cancellationToken)
        {
            if (source == null)
                throw Error.ArgumentNull(nameof(source));
            if (selector == null)
                throw Error.ArgumentNull(nameof(selector));

            return MaxCore(source, selector, cancellationToken);
        }

        public static Task<double> Max(this IAsyncEnumerable<double> source)
        {
            if (source == null)
                throw Error.ArgumentNull(nameof(source));

            return MaxCore(source, CancellationToken.None);
        }

        public static Task<double> Max(this IAsyncEnumerable<double> source, CancellationToken cancellationToken)
        {
            if (source == null)
                throw Error.ArgumentNull(nameof(source));

            return MaxCore(source, cancellationToken);
        }

        public static Task<double> Max<TSource>(this IAsyncEnumerable<TSource> source, Func<TSource, double> selector)
        {
            if (source == null)
                throw Error.ArgumentNull(nameof(source));
            if (selector == null)
                throw Error.ArgumentNull(nameof(selector));

            return MaxCore(source, selector, CancellationToken.None);
        }

        public static Task<double> Max<TSource>(this IAsyncEnumerable<TSource> source, Func<TSource, double> selector, CancellationToken cancellationToken)
        {
            if (source == null)
                throw Error.ArgumentNull(nameof(source));
            if (selector == null)
                throw Error.ArgumentNull(nameof(selector));

            return MaxCore(source, selector, cancellationToken);
        }

        public static Task<double> Max<TSource>(this IAsyncEnumerable<TSource> source, Func<TSource, Task<double>> selector)
        {
            if (source == null)
                throw Error.ArgumentNull(nameof(source));
            if (selector == null)
                throw Error.ArgumentNull(nameof(selector));

            return MaxCore(source, selector, CancellationToken.None);
        }

        public static Task<double> Max<TSource>(this IAsyncEnumerable<TSource> source, Func<TSource, Task<double>> selector, CancellationToken cancellationToken)
        {
            if (source == null)
                throw Error.ArgumentNull(nameof(source));
            if (selector == null)
                throw Error.ArgumentNull(nameof(selector));

            return MaxCore(source, selector, cancellationToken);
        }

        public static Task<double?> Max(this IAsyncEnumerable<double?> source)
        {
            if (source == null)
                throw Error.ArgumentNull(nameof(source));

            return MaxCore(source, CancellationToken.None);
        }

        public static Task<double?> Max(this IAsyncEnumerable<double?> source, CancellationToken cancellationToken)
        {
            if (source == null)
                throw Error.ArgumentNull(nameof(source));

            return MaxCore(source, cancellationToken);
        }

        public static Task<double?> Max<TSource>(this IAsyncEnumerable<TSource> source, Func<TSource, double?> selector)
        {
            if (source == null)
                throw Error.ArgumentNull(nameof(source));
            if (selector == null)
                throw Error.ArgumentNull(nameof(selector));

            return MaxCore(source, selector, CancellationToken.None);
        }

        public static Task<double?> Max<TSource>(this IAsyncEnumerable<TSource> source, Func<TSource, double?> selector, CancellationToken cancellationToken)
        {
            if (source == null)
                throw Error.ArgumentNull(nameof(source));
            if (selector == null)
                throw Error.ArgumentNull(nameof(selector));

            return MaxCore(source, selector, cancellationToken);
        }

        public static Task<double?> Max<TSource>(this IAsyncEnumerable<TSource> source, Func<TSource, Task<double?>> selector)
        {
            if (source == null)
                throw Error.ArgumentNull(nameof(source));
            if (selector == null)
                throw Error.ArgumentNull(nameof(selector));

            return MaxCore(source, selector, CancellationToken.None);
        }

        public static Task<double?> Max<TSource>(this IAsyncEnumerable<TSource> source, Func<TSource, Task<double?>> selector, CancellationToken cancellationToken)
        {
            if (source == null)
                throw Error.ArgumentNull(nameof(source));
            if (selector == null)
                throw Error.ArgumentNull(nameof(selector));

            return MaxCore(source, selector, cancellationToken);
        }

        public static Task<decimal> Max(this IAsyncEnumerable<decimal> source)
        {
            if (source == null)
                throw Error.ArgumentNull(nameof(source));

            return MaxCore(source, CancellationToken.None);
        }

        public static Task<decimal> Max(this IAsyncEnumerable<decimal> source, CancellationToken cancellationToken)
        {
            if (source == null)
                throw Error.ArgumentNull(nameof(source));

            return MaxCore(source, cancellationToken);
        }

        public static Task<decimal> Max<TSource>(this IAsyncEnumerable<TSource> source, Func<TSource, decimal> selector)
        {
            if (source == null)
                throw Error.ArgumentNull(nameof(source));
            if (selector == null)
                throw Error.ArgumentNull(nameof(selector));

            return MaxCore(source, selector, CancellationToken.None);
        }

        public static Task<decimal> Max<TSource>(this IAsyncEnumerable<TSource> source, Func<TSource, decimal> selector, CancellationToken cancellationToken)
        {
            if (source == null)
                throw Error.ArgumentNull(nameof(source));
            if (selector == null)
                throw Error.ArgumentNull(nameof(selector));

            return MaxCore(source, selector, cancellationToken);
        }

        public static Task<decimal> Max<TSource>(this IAsyncEnumerable<TSource> source, Func<TSource, Task<decimal>> selector)
        {
            if (source == null)
                throw Error.ArgumentNull(nameof(source));
            if (selector == null)
                throw Error.ArgumentNull(nameof(selector));

            return MaxCore(source, selector, CancellationToken.None);
        }

        public static Task<decimal> Max<TSource>(this IAsyncEnumerable<TSource> source, Func<TSource, Task<decimal>> selector, CancellationToken cancellationToken)
        {
            if (source == null)
                throw Error.ArgumentNull(nameof(source));
            if (selector == null)
                throw Error.ArgumentNull(nameof(selector));

            return MaxCore(source, selector, cancellationToken);
        }

        public static Task<decimal?> Max(this IAsyncEnumerable<decimal?> source)
        {
            if (source == null)
                throw Error.ArgumentNull(nameof(source));

            return MaxCore(source, CancellationToken.None);
        }

        public static Task<decimal?> Max(this IAsyncEnumerable<decimal?> source, CancellationToken cancellationToken)
        {
            if (source == null)
                throw Error.ArgumentNull(nameof(source));

            return MaxCore(source, cancellationToken);
        }

        public static Task<decimal?> Max<TSource>(this IAsyncEnumerable<TSource> source, Func<TSource, decimal?> selector)
        {
            if (source == null)
                throw Error.ArgumentNull(nameof(source));
            if (selector == null)
                throw Error.ArgumentNull(nameof(selector));

            return MaxCore(source, selector, CancellationToken.None);
        }

        public static Task<decimal?> Max<TSource>(this IAsyncEnumerable<TSource> source, Func<TSource, decimal?> selector, CancellationToken cancellationToken)
        {
            if (source == null)
                throw Error.ArgumentNull(nameof(source));
            if (selector == null)
                throw Error.ArgumentNull(nameof(selector));

            return MaxCore(source, selector, cancellationToken);
        }

        public static Task<decimal?> Max<TSource>(this IAsyncEnumerable<TSource> source, Func<TSource, Task<decimal?>> selector)
        {
            if (source == null)
                throw Error.ArgumentNull(nameof(source));
            if (selector == null)
                throw Error.ArgumentNull(nameof(selector));

            return MaxCore(source, selector, CancellationToken.None);
        }

        public static Task<decimal?> Max<TSource>(this IAsyncEnumerable<TSource> source, Func<TSource, Task<decimal?>> selector, CancellationToken cancellationToken)
        {
            if (source == null)
                throw Error.ArgumentNull(nameof(source));
            if (selector == null)
                throw Error.ArgumentNull(nameof(selector));

            return MaxCore(source, selector, cancellationToken);
        }

        public static Task<int> Min(this IAsyncEnumerable<int> source)
        {
            if (source == null)
                throw Error.ArgumentNull(nameof(source));

            return MinCore(source, CancellationToken.None);
        }

        public static Task<int> Min(this IAsyncEnumerable<int> source, CancellationToken cancellationToken)
        {
            if (source == null)
                throw Error.ArgumentNull(nameof(source));

            return MinCore(source, cancellationToken);
        }

        public static Task<int> Min<TSource>(this IAsyncEnumerable<TSource> source, Func<TSource, int> selector)
        {
            if (source == null)
                throw Error.ArgumentNull(nameof(source));
            if (selector == null)
                throw Error.ArgumentNull(nameof(selector));

            return MinCore(source, selector, CancellationToken.None);
        }

        public static Task<int> Min<TSource>(this IAsyncEnumerable<TSource> source, Func<TSource, int> selector, CancellationToken cancellationToken)
        {
            if (source == null)
                throw Error.ArgumentNull(nameof(source));
            if (selector == null)
                throw Error.ArgumentNull(nameof(selector));

            return MinCore(source, selector, cancellationToken);
        }

        public static Task<int> Min<TSource>(this IAsyncEnumerable<TSource> source, Func<TSource, Task<int>> selector)
        {
            if (source == null)
                throw Error.ArgumentNull(nameof(source));
            if (selector == null)
                throw Error.ArgumentNull(nameof(selector));

            return MinCore(source, selector, CancellationToken.None);
        }

        public static Task<int> Min<TSource>(this IAsyncEnumerable<TSource> source, Func<TSource, Task<int>> selector, CancellationToken cancellationToken)
        {
            if (source == null)
                throw Error.ArgumentNull(nameof(source));
            if (selector == null)
                throw Error.ArgumentNull(nameof(selector));

            return MinCore(source, selector, cancellationToken);
        }

        public static Task<int?> Min(this IAsyncEnumerable<int?> source)
        {
            if (source == null)
                throw Error.ArgumentNull(nameof(source));

            return MinCore(source, CancellationToken.None);
        }

        public static Task<int?> Min(this IAsyncEnumerable<int?> source, CancellationToken cancellationToken)
        {
            if (source == null)
                throw Error.ArgumentNull(nameof(source));

            return MinCore(source, cancellationToken);
        }

        public static Task<int?> Min<TSource>(this IAsyncEnumerable<TSource> source, Func<TSource, int?> selector)
        {
            if (source == null)
                throw Error.ArgumentNull(nameof(source));
            if (selector == null)
                throw Error.ArgumentNull(nameof(selector));

            return MinCore(source, selector, CancellationToken.None);
        }

        public static Task<int?> Min<TSource>(this IAsyncEnumerable<TSource> source, Func<TSource, int?> selector, CancellationToken cancellationToken)
        {
            if (source == null)
                throw Error.ArgumentNull(nameof(source));
            if (selector == null)
                throw Error.ArgumentNull(nameof(selector));

            return MinCore(source, selector, cancellationToken);
        }

        public static Task<int?> Min<TSource>(this IAsyncEnumerable<TSource> source, Func<TSource, Task<int?>> selector)
        {
            if (source == null)
                throw Error.ArgumentNull(nameof(source));
            if (selector == null)
                throw Error.ArgumentNull(nameof(selector));

            return MinCore(source, selector, CancellationToken.None);
        }

        public static Task<int?> Min<TSource>(this IAsyncEnumerable<TSource> source, Func<TSource, Task<int?>> selector, CancellationToken cancellationToken)
        {
            if (source == null)
                throw Error.ArgumentNull(nameof(source));
            if (selector == null)
                throw Error.ArgumentNull(nameof(selector));

            return MinCore(source, selector, cancellationToken);
        }

        public static Task<long> Min(this IAsyncEnumerable<long> source)
        {
            if (source == null)
                throw Error.ArgumentNull(nameof(source));

            return MinCore(source, CancellationToken.None);
        }

        public static Task<long> Min(this IAsyncEnumerable<long> source, CancellationToken cancellationToken)
        {
            if (source == null)
                throw Error.ArgumentNull(nameof(source));

            return MinCore(source, cancellationToken);
        }

        public static Task<long> Min<TSource>(this IAsyncEnumerable<TSource> source, Func<TSource, long> selector)
        {
            if (source == null)
                throw Error.ArgumentNull(nameof(source));
            if (selector == null)
                throw Error.ArgumentNull(nameof(selector));

            return MinCore(source, selector, CancellationToken.None);
        }

        public static Task<long> Min<TSource>(this IAsyncEnumerable<TSource> source, Func<TSource, long> selector, CancellationToken cancellationToken)
        {
            if (source == null)
                throw Error.ArgumentNull(nameof(source));
            if (selector == null)
                throw Error.ArgumentNull(nameof(selector));

            return MinCore(source, selector, cancellationToken);
        }

        public static Task<long> Min<TSource>(this IAsyncEnumerable<TSource> source, Func<TSource, Task<long>> selector)
        {
            if (source == null)
                throw Error.ArgumentNull(nameof(source));
            if (selector == null)
                throw Error.ArgumentNull(nameof(selector));

            return MinCore(source, selector, CancellationToken.None);
        }

        public static Task<long> Min<TSource>(this IAsyncEnumerable<TSource> source, Func<TSource, Task<long>> selector, CancellationToken cancellationToken)
        {
            if (source == null)
                throw Error.ArgumentNull(nameof(source));
            if (selector == null)
                throw Error.ArgumentNull(nameof(selector));

            return MinCore(source, selector, cancellationToken);
        }

        public static Task<long?> Min(this IAsyncEnumerable<long?> source)
        {
            if (source == null)
                throw Error.ArgumentNull(nameof(source));

            return MinCore(source, CancellationToken.None);
        }

        public static Task<long?> Min(this IAsyncEnumerable<long?> source, CancellationToken cancellationToken)
        {
            if (source == null)
                throw Error.ArgumentNull(nameof(source));

            return MinCore(source, cancellationToken);
        }

        public static Task<long?> Min<TSource>(this IAsyncEnumerable<TSource> source, Func<TSource, long?> selector)
        {
            if (source == null)
                throw Error.ArgumentNull(nameof(source));
            if (selector == null)
                throw Error.ArgumentNull(nameof(selector));

            return MinCore(source, selector, CancellationToken.None);
        }

        public static Task<long?> Min<TSource>(this IAsyncEnumerable<TSource> source, Func<TSource, long?> selector, CancellationToken cancellationToken)
        {
            if (source == null)
                throw Error.ArgumentNull(nameof(source));
            if (selector == null)
                throw Error.ArgumentNull(nameof(selector));

            return MinCore(source, selector, cancellationToken);
        }

        public static Task<long?> Min<TSource>(this IAsyncEnumerable<TSource> source, Func<TSource, Task<long?>> selector)
        {
            if (source == null)
                throw Error.ArgumentNull(nameof(source));
            if (selector == null)
                throw Error.ArgumentNull(nameof(selector));

            return MinCore(source, selector, CancellationToken.None);
        }

        public static Task<long?> Min<TSource>(this IAsyncEnumerable<TSource> source, Func<TSource, Task<long?>> selector, CancellationToken cancellationToken)
        {
            if (source == null)
                throw Error.ArgumentNull(nameof(source));
            if (selector == null)
                throw Error.ArgumentNull(nameof(selector));

            return MinCore(source, selector, cancellationToken);
        }

        public static Task<float> Min(this IAsyncEnumerable<float> source)
        {
            if (source == null)
                throw Error.ArgumentNull(nameof(source));

            return MinCore(source, CancellationToken.None);
        }

        public static Task<float> Min(this IAsyncEnumerable<float> source, CancellationToken cancellationToken)
        {
            if (source == null)
                throw Error.ArgumentNull(nameof(source));

            return MinCore(source, cancellationToken);
        }

        public static Task<float> Min<TSource>(this IAsyncEnumerable<TSource> source, Func<TSource, float> selector)
        {
            if (source == null)
                throw Error.ArgumentNull(nameof(source));
            if (selector == null)
                throw Error.ArgumentNull(nameof(selector));

            return MinCore(source, selector, CancellationToken.None);
        }

        public static Task<float> Min<TSource>(this IAsyncEnumerable<TSource> source, Func<TSource, float> selector, CancellationToken cancellationToken)
        {
            if (source == null)
                throw Error.ArgumentNull(nameof(source));
            if (selector == null)
                throw Error.ArgumentNull(nameof(selector));

            return MinCore(source, selector, cancellationToken);
        }

        public static Task<float> Min<TSource>(this IAsyncEnumerable<TSource> source, Func<TSource, Task<float>> selector)
        {
            if (source == null)
                throw Error.ArgumentNull(nameof(source));
            if (selector == null)
                throw Error.ArgumentNull(nameof(selector));

            return MinCore(source, selector, CancellationToken.None);
        }

        public static Task<float> Min<TSource>(this IAsyncEnumerable<TSource> source, Func<TSource, Task<float>> selector, CancellationToken cancellationToken)
        {
            if (source == null)
                throw Error.ArgumentNull(nameof(source));
            if (selector == null)
                throw Error.ArgumentNull(nameof(selector));

            return MinCore(source, selector, cancellationToken);
        }

        public static Task<float?> Min(this IAsyncEnumerable<float?> source)
        {
            if (source == null)
                throw Error.ArgumentNull(nameof(source));

            return MinCore(source, CancellationToken.None);
        }

        public static Task<float?> Min(this IAsyncEnumerable<float?> source, CancellationToken cancellationToken)
        {
            if (source == null)
                throw Error.ArgumentNull(nameof(source));

            return MinCore(source, cancellationToken);
        }

        public static Task<float?> Min<TSource>(this IAsyncEnumerable<TSource> source, Func<TSource, float?> selector)
        {
            if (source == null)
                throw Error.ArgumentNull(nameof(source));
            if (selector == null)
                throw Error.ArgumentNull(nameof(selector));

            return MinCore(source, selector, CancellationToken.None);
        }

        public static Task<float?> Min<TSource>(this IAsyncEnumerable<TSource> source, Func<TSource, float?> selector, CancellationToken cancellationToken)
        {
            if (source == null)
                throw Error.ArgumentNull(nameof(source));
            if (selector == null)
                throw Error.ArgumentNull(nameof(selector));

            return MinCore(source, selector, cancellationToken);
        }

        public static Task<float?> Min<TSource>(this IAsyncEnumerable<TSource> source, Func<TSource, Task<float?>> selector)
        {
            if (source == null)
                throw Error.ArgumentNull(nameof(source));
            if (selector == null)
                throw Error.ArgumentNull(nameof(selector));

            return MinCore(source, selector, CancellationToken.None);
        }

        public static Task<float?> Min<TSource>(this IAsyncEnumerable<TSource> source, Func<TSource, Task<float?>> selector, CancellationToken cancellationToken)
        {
            if (source == null)
                throw Error.ArgumentNull(nameof(source));
            if (selector == null)
                throw Error.ArgumentNull(nameof(selector));

            return MinCore(source, selector, cancellationToken);
        }

        public static Task<double> Min(this IAsyncEnumerable<double> source)
        {
            if (source == null)
                throw Error.ArgumentNull(nameof(source));

            return MinCore(source, CancellationToken.None);
        }

        public static Task<double> Min(this IAsyncEnumerable<double> source, CancellationToken cancellationToken)
        {
            if (source == null)
                throw Error.ArgumentNull(nameof(source));

            return MinCore(source, cancellationToken);
        }

        public static Task<double> Min<TSource>(this IAsyncEnumerable<TSource> source, Func<TSource, double> selector)
        {
            if (source == null)
                throw Error.ArgumentNull(nameof(source));
            if (selector == null)
                throw Error.ArgumentNull(nameof(selector));

            return MinCore(source, selector, CancellationToken.None);
        }

        public static Task<double> Min<TSource>(this IAsyncEnumerable<TSource> source, Func<TSource, double> selector, CancellationToken cancellationToken)
        {
            if (source == null)
                throw Error.ArgumentNull(nameof(source));
            if (selector == null)
                throw Error.ArgumentNull(nameof(selector));

            return MinCore(source, selector, cancellationToken);
        }

        public static Task<double> Min<TSource>(this IAsyncEnumerable<TSource> source, Func<TSource, Task<double>> selector)
        {
            if (source == null)
                throw Error.ArgumentNull(nameof(source));
            if (selector == null)
                throw Error.ArgumentNull(nameof(selector));

            return MinCore(source, selector, CancellationToken.None);
        }

        public static Task<double> Min<TSource>(this IAsyncEnumerable<TSource> source, Func<TSource, Task<double>> selector, CancellationToken cancellationToken)
        {
            if (source == null)
                throw Error.ArgumentNull(nameof(source));
            if (selector == null)
                throw Error.ArgumentNull(nameof(selector));

            return MinCore(source, selector, cancellationToken);
        }

        public static Task<double?> Min(this IAsyncEnumerable<double?> source)
        {
            if (source == null)
                throw Error.ArgumentNull(nameof(source));

            return MinCore(source, CancellationToken.None);
        }

        public static Task<double?> Min(this IAsyncEnumerable<double?> source, CancellationToken cancellationToken)
        {
            if (source == null)
                throw Error.ArgumentNull(nameof(source));

            return MinCore(source, cancellationToken);
        }

        public static Task<double?> Min<TSource>(this IAsyncEnumerable<TSource> source, Func<TSource, double?> selector)
        {
            if (source == null)
                throw Error.ArgumentNull(nameof(source));
            if (selector == null)
                throw Error.ArgumentNull(nameof(selector));

            return MinCore(source, selector, CancellationToken.None);
        }

        public static Task<double?> Min<TSource>(this IAsyncEnumerable<TSource> source, Func<TSource, double?> selector, CancellationToken cancellationToken)
        {
            if (source == null)
                throw Error.ArgumentNull(nameof(source));
            if (selector == null)
                throw Error.ArgumentNull(nameof(selector));

            return MinCore(source, selector, cancellationToken);
        }

        public static Task<double?> Min<TSource>(this IAsyncEnumerable<TSource> source, Func<TSource, Task<double?>> selector)
        {
            if (source == null)
                throw Error.ArgumentNull(nameof(source));
            if (selector == null)
                throw Error.ArgumentNull(nameof(selector));

            return MinCore(source, selector, CancellationToken.None);
        }

        public static Task<double?> Min<TSource>(this IAsyncEnumerable<TSource> source, Func<TSource, Task<double?>> selector, CancellationToken cancellationToken)
        {
            if (source == null)
                throw Error.ArgumentNull(nameof(source));
            if (selector == null)
                throw Error.ArgumentNull(nameof(selector));

            return MinCore(source, selector, cancellationToken);
        }

        public static Task<decimal> Min(this IAsyncEnumerable<decimal> source)
        {
            if (source == null)
                throw Error.ArgumentNull(nameof(source));

            return MinCore(source, CancellationToken.None);
        }

        public static Task<decimal> Min(this IAsyncEnumerable<decimal> source, CancellationToken cancellationToken)
        {
            if (source == null)
                throw Error.ArgumentNull(nameof(source));

            return MinCore(source, cancellationToken);
        }

        public static Task<decimal> Min<TSource>(this IAsyncEnumerable<TSource> source, Func<TSource, decimal> selector)
        {
            if (source == null)
                throw Error.ArgumentNull(nameof(source));
            if (selector == null)
                throw Error.ArgumentNull(nameof(selector));

            return MinCore(source, selector, CancellationToken.None);
        }

        public static Task<decimal> Min<TSource>(this IAsyncEnumerable<TSource> source, Func<TSource, decimal> selector, CancellationToken cancellationToken)
        {
            if (source == null)
                throw Error.ArgumentNull(nameof(source));
            if (selector == null)
                throw Error.ArgumentNull(nameof(selector));

            return MinCore(source, selector, cancellationToken);
        }

        public static Task<decimal> Min<TSource>(this IAsyncEnumerable<TSource> source, Func<TSource, Task<decimal>> selector)
        {
            if (source == null)
                throw Error.ArgumentNull(nameof(source));
            if (selector == null)
                throw Error.ArgumentNull(nameof(selector));

            return MinCore(source, selector, CancellationToken.None);
        }

        public static Task<decimal> Min<TSource>(this IAsyncEnumerable<TSource> source, Func<TSource, Task<decimal>> selector, CancellationToken cancellationToken)
        {
            if (source == null)
                throw Error.ArgumentNull(nameof(source));
            if (selector == null)
                throw Error.ArgumentNull(nameof(selector));

            return MinCore(source, selector, cancellationToken);
        }

        public static Task<decimal?> Min(this IAsyncEnumerable<decimal?> source)
        {
            if (source == null)
                throw Error.ArgumentNull(nameof(source));

            return MinCore(source, CancellationToken.None);
        }

        public static Task<decimal?> Min(this IAsyncEnumerable<decimal?> source, CancellationToken cancellationToken)
        {
            if (source == null)
                throw Error.ArgumentNull(nameof(source));

            return MinCore(source, cancellationToken);
        }

        public static Task<decimal?> Min<TSource>(this IAsyncEnumerable<TSource> source, Func<TSource, decimal?> selector)
        {
            if (source == null)
                throw Error.ArgumentNull(nameof(source));
            if (selector == null)
                throw Error.ArgumentNull(nameof(selector));

            return MinCore(source, selector, CancellationToken.None);
        }

        public static Task<decimal?> Min<TSource>(this IAsyncEnumerable<TSource> source, Func<TSource, decimal?> selector, CancellationToken cancellationToken)
        {
            if (source == null)
                throw Error.ArgumentNull(nameof(source));
            if (selector == null)
                throw Error.ArgumentNull(nameof(selector));

            return MinCore(source, selector, cancellationToken);
        }

        public static Task<decimal?> Min<TSource>(this IAsyncEnumerable<TSource> source, Func<TSource, Task<decimal?>> selector)
        {
            if (source == null)
                throw Error.ArgumentNull(nameof(source));
            if (selector == null)
                throw Error.ArgumentNull(nameof(selector));

            return MinCore(source, selector, CancellationToken.None);
        }

        public static Task<decimal?> Min<TSource>(this IAsyncEnumerable<TSource> source, Func<TSource, Task<decimal?>> selector, CancellationToken cancellationToken)
        {
            if (source == null)
                throw Error.ArgumentNull(nameof(source));
            if (selector == null)
                throw Error.ArgumentNull(nameof(selector));

            return MinCore(source, selector, cancellationToken);
        }

    }
}

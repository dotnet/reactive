// Copyright (c) Microsoft Open Technologies, Inc. All rights reserved. See License.txt in the project root for license information.
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace System.Linq
{
    public static partial class AsyncEnumerable
    {
        public static Task<decimal?> Min<TSource>(this IAsyncEnumerable<TSource> source, Func<TSource, decimal?> selector)
        {
            if (source == null)
                throw new ArgumentNullException("source");
            if (selector == null)
                throw new ArgumentNullException("selector");

            return Min(source, selector, CancellationToken.None);
        }

        public static Task<TResult> Min<TSource, TResult>(this IAsyncEnumerable<TSource> source, Func<TSource, TResult> selector)
        {
            if (source == null)
                throw new ArgumentNullException("source");
            if (selector == null)
                throw new ArgumentNullException("selector");

            return Min(source, selector, CancellationToken.None);
        }

        public static Task<int> Sum(this IAsyncEnumerable<int> source)
        {
            if (source == null)
                throw new ArgumentNullException("source");

            return Sum(source, CancellationToken.None);
        }

        public static Task<long> Sum(this IAsyncEnumerable<long> source)
        {
            if (source == null)
                throw new ArgumentNullException("source");

            return Sum(source, CancellationToken.None);
        }

        public static Task<double> Sum(this IAsyncEnumerable<double> source)
        {
            if (source == null)
                throw new ArgumentNullException("source");

            return Sum(source, CancellationToken.None);
        }

        public static Task<float> Sum(this IAsyncEnumerable<float> source)
        {
            if (source == null)
                throw new ArgumentNullException("source");

            return Sum(source, CancellationToken.None);
        }

        public static Task<decimal> Sum(this IAsyncEnumerable<decimal> source)
        {
            if (source == null)
                throw new ArgumentNullException("source");

            return Sum(source, CancellationToken.None);
        }

        public static Task<int?> Sum(this IAsyncEnumerable<int?> source)
        {
            if (source == null)
                throw new ArgumentNullException("source");

            return Sum(source, CancellationToken.None);
        }

        public static Task<long?> Sum(this IAsyncEnumerable<long?> source)
        {
            if (source == null)
                throw new ArgumentNullException("source");

            return Sum(source, CancellationToken.None);
        }

        public static Task<double?> Sum(this IAsyncEnumerable<double?> source)
        {
            if (source == null)
                throw new ArgumentNullException("source");

            return Sum(source, CancellationToken.None);
        }

        public static Task<float?> Sum(this IAsyncEnumerable<float?> source)
        {
            if (source == null)
                throw new ArgumentNullException("source");

            return Sum(source, CancellationToken.None);
        }

        public static Task<decimal?> Sum(this IAsyncEnumerable<decimal?> source)
        {
            if (source == null)
                throw new ArgumentNullException("source");

            return Sum(source, CancellationToken.None);
        }

        public static Task<int> Sum<TSource>(this IAsyncEnumerable<TSource> source, Func<TSource, int> selector)
        {
            if (source == null)
                throw new ArgumentNullException("source");
            if (selector == null)
                throw new ArgumentNullException("selector");

            return Sum(source, selector, CancellationToken.None);
        }

        public static Task<long> Sum<TSource>(this IAsyncEnumerable<TSource> source, Func<TSource, long> selector)
        {
            if (source == null)
                throw new ArgumentNullException("source");
            if (selector == null)
                throw new ArgumentNullException("selector");

            return Sum(source, selector, CancellationToken.None);
        }

        public static Task<double> Sum<TSource>(this IAsyncEnumerable<TSource> source, Func<TSource, double> selector)
        {
            if (source == null)
                throw new ArgumentNullException("source");
            if (selector == null)
                throw new ArgumentNullException("selector");

            return Sum(source, selector, CancellationToken.None);
        }

        public static Task<float> Sum<TSource>(this IAsyncEnumerable<TSource> source, Func<TSource, float> selector)
        {
            if (source == null)
                throw new ArgumentNullException("source");
            if (selector == null)
                throw new ArgumentNullException("selector");

            return Sum(source, selector, CancellationToken.None);
        }

        public static Task<decimal> Sum<TSource>(this IAsyncEnumerable<TSource> source, Func<TSource, decimal> selector)
        {
            if (source == null)
                throw new ArgumentNullException("source");
            if (selector == null)
                throw new ArgumentNullException("selector");

            return Sum(source, selector, CancellationToken.None);
        }

        public static Task<int?> Sum<TSource>(this IAsyncEnumerable<TSource> source, Func<TSource, int?> selector)
        {
            if (source == null)
                throw new ArgumentNullException("source");
            if (selector == null)
                throw new ArgumentNullException("selector");

            return Sum(source, selector, CancellationToken.None);
        }

        public static Task<long?> Sum<TSource>(this IAsyncEnumerable<TSource> source, Func<TSource, long?> selector)
        {
            if (source == null)
                throw new ArgumentNullException("source");
            if (selector == null)
                throw new ArgumentNullException("selector");

            return Sum(source, selector, CancellationToken.None);
        }

        public static Task<double?> Sum<TSource>(this IAsyncEnumerable<TSource> source, Func<TSource, double?> selector)
        {
            if (source == null)
                throw new ArgumentNullException("source");
            if (selector == null)
                throw new ArgumentNullException("selector");

            return Sum(source, selector, CancellationToken.None);
        }

        public static Task<float?> Sum<TSource>(this IAsyncEnumerable<TSource> source, Func<TSource, float?> selector)
        {
            if (source == null)
                throw new ArgumentNullException("source");
            if (selector == null)
                throw new ArgumentNullException("selector");

            return Sum(source, selector, CancellationToken.None);
        }

        public static Task<decimal?> Sum<TSource>(this IAsyncEnumerable<TSource> source, Func<TSource, decimal?> selector)
        {
            if (source == null)
                throw new ArgumentNullException("source");
            if (selector == null)
                throw new ArgumentNullException("selector");

            return Sum(source, selector, CancellationToken.None);
        }

        public static void ForEach<TSource>(this IAsyncEnumerable<TSource> source, Action<TSource> action)
        {
            if (source == null)
                throw new ArgumentNullException("source");
            if (action == null)
                throw new ArgumentNullException("action");

            source.ForEachAsync(action).Wait();
        }

        public static Task ForEachAsync<TSource>(this IAsyncEnumerable<TSource> source, Action<TSource> action)
        {
            if (source == null)
                throw new ArgumentNullException("source");
            if (action == null)
                throw new ArgumentNullException("action");

            return ForEachAsync(source, action, CancellationToken.None);
        }

        public static void ForEach<TSource>(this IAsyncEnumerable<TSource> source, Action<TSource, int> action)
        {
            if (source == null)
                throw new ArgumentNullException("source");
            if (action == null)
                throw new ArgumentNullException("action");

            source.ForEachAsync(action).Wait();
        }

        public static Task ForEachAsync<TSource>(this IAsyncEnumerable<TSource> source, Action<TSource, int> action)
        {
            if (source == null)
                throw new ArgumentNullException("source");
            if (action == null)
                throw new ArgumentNullException("action");

            return ForEachAsync(source, action, CancellationToken.None);
        }

        public static Task<double> Average(this IAsyncEnumerable<long> source)
        {
            if (source == null)
                throw new ArgumentNullException("source");

            return Average(source, CancellationToken.None);
        }

        public static Task<double?> Average(this IAsyncEnumerable<long?> source)
        {
            if (source == null)
                throw new ArgumentNullException("source");

            return Average(source, CancellationToken.None);
        }

        public static Task<double> Average(this IAsyncEnumerable<double> source)
        {
            if (source == null)
                throw new ArgumentNullException("source");

            return Average(source, CancellationToken.None);
        }

        public static Task<double?> Average(this IAsyncEnumerable<double?> source)
        {
            if (source == null)
                throw new ArgumentNullException("source");

            return Average(source, CancellationToken.None);
        }

        public static Task<float> Average(this IAsyncEnumerable<float> source)
        {
            if (source == null)
                throw new ArgumentNullException("source");

            return Average(source, CancellationToken.None);
        }

        public static Task<float?> Average(this IAsyncEnumerable<float?> source)
        {
            if (source == null)
                throw new ArgumentNullException("source");

            return Average(source, CancellationToken.None);
        }

        public static Task<decimal> Average(this IAsyncEnumerable<decimal> source)
        {
            if (source == null)
                throw new ArgumentNullException("source");

            return Average(source, CancellationToken.None);
        }

        public static Task<decimal?> Average(this IAsyncEnumerable<decimal?> source)
        {
            if (source == null)
                throw new ArgumentNullException("source");

            return Average(source, CancellationToken.None);
        }

        public static Task<double?> Average<TSource>(this IAsyncEnumerable<TSource> source, Func<TSource, int?> selector)
        {
            if (source == null)
                throw new ArgumentNullException("source");
            if (selector == null)
                throw new ArgumentNullException("selector");

            return Average(source, selector, CancellationToken.None);
        }

        public static Task<double> Average<TSource>(this IAsyncEnumerable<TSource> source, Func<TSource, int> selector)
        {
            if (source == null)
                throw new ArgumentNullException("source");
            if (selector == null)
                throw new ArgumentNullException("selector");

            return Average(source, selector, CancellationToken.None);
        }

        public static Task<double> Average<TSource>(this IAsyncEnumerable<TSource> source, Func<TSource, long> selector)
        {
            if (source == null)
                throw new ArgumentNullException("source");
            if (selector == null)
                throw new ArgumentNullException("selector");

            return Average(source, selector, CancellationToken.None);
        }

        public static Task<double?> Average<TSource>(this IAsyncEnumerable<TSource> source, Func<TSource, long?> selector)
        {
            if (source == null)
                throw new ArgumentNullException("source");
            if (selector == null)
                throw new ArgumentNullException("selector");

            return Average(source, selector, CancellationToken.None);
        }

        public static Task<double> Average<TSource>(this IAsyncEnumerable<TSource> source, Func<TSource, double> selector)
        {
            if (source == null)
                throw new ArgumentNullException("source");
            if (selector == null)
                throw new ArgumentNullException("selector");

            return Average(source, selector, CancellationToken.None);
        }

        public static Task<double?> Average<TSource>(this IAsyncEnumerable<TSource> source, Func<TSource, double?> selector)
        {
            if (source == null)
                throw new ArgumentNullException("source");
            if (selector == null)
                throw new ArgumentNullException("selector");

            return Average(source, selector, CancellationToken.None);
        }

        public static Task<float> Average<TSource>(this IAsyncEnumerable<TSource> source, Func<TSource, float> selector)
        {
            if (source == null)
                throw new ArgumentNullException("source");
            if (selector == null)
                throw new ArgumentNullException("selector");

            return Average(source, selector, CancellationToken.None);
        }

        public static Task<float?> Average<TSource>(this IAsyncEnumerable<TSource> source, Func<TSource, float?> selector)
        {
            if (source == null)
                throw new ArgumentNullException("source");
            if (selector == null)
                throw new ArgumentNullException("selector");

            return Average(source, selector, CancellationToken.None);
        }

        public static Task<decimal> Average<TSource>(this IAsyncEnumerable<TSource> source, Func<TSource, decimal> selector)
        {
            if (source == null)
                throw new ArgumentNullException("source");
            if (selector == null)
                throw new ArgumentNullException("selector");

            return Average(source, selector, CancellationToken.None);
        }

        public static Task<decimal?> Average<TSource>(this IAsyncEnumerable<TSource> source, Func<TSource, decimal?> selector)
        {
            if (source == null)
                throw new ArgumentNullException("source");
            if (selector == null)
                throw new ArgumentNullException("selector");

            return Average(source, selector, CancellationToken.None);
        }

        public static Task<int> Max(this IAsyncEnumerable<int> source)
        {
            if (source == null)
                throw new ArgumentNullException("source");

            return Max(source, CancellationToken.None);
        }

        public static Task<long> Max(this IAsyncEnumerable<long> source)
        {
            if (source == null)
                throw new ArgumentNullException("source");

            return Max(source, CancellationToken.None);
        }

        public static Task<double> Max(this IAsyncEnumerable<double> source)
        {
            if (source == null)
                throw new ArgumentNullException("source");

            return Max(source, CancellationToken.None);
        }

        public static Task<float> Max(this IAsyncEnumerable<float> source)
        {
            if (source == null)
                throw new ArgumentNullException("source");

            return Max(source, CancellationToken.None);
        }

        public static Task<decimal> Max(this IAsyncEnumerable<decimal> source)
        {
            if (source == null)
                throw new ArgumentNullException("source");

            return Max(source, CancellationToken.None);
        }

        public static Task<int?> Max(this IAsyncEnumerable<int?> source)
        {
            if (source == null)
                throw new ArgumentNullException("source");

            return Max(source, CancellationToken.None);
        }

        public static Task<long?> Max(this IAsyncEnumerable<long?> source)
        {
            if (source == null)
                throw new ArgumentNullException("source");

            return Max(source, CancellationToken.None);
        }

        public static Task<double?> Max(this IAsyncEnumerable<double?> source)
        {
            if (source == null)
                throw new ArgumentNullException("source");

            return Max(source, CancellationToken.None);
        }

        public static Task<float?> Max(this IAsyncEnumerable<float?> source)
        {
            if (source == null)
                throw new ArgumentNullException("source");

            return Max(source, CancellationToken.None);
        }

        public static Task<decimal?> Max(this IAsyncEnumerable<decimal?> source)
        {
            if (source == null)
                throw new ArgumentNullException("source");

            return Max(source, CancellationToken.None);
        }

        public static Task<TSource> Max<TSource>(this IAsyncEnumerable<TSource> source)
        {
            if (source == null)
                throw new ArgumentNullException("source");

            return Max(source, CancellationToken.None);
        }

        public static Task<int> Max<TSource>(this IAsyncEnumerable<TSource> source, Func<TSource, int> selector)
        {
            if (source == null)
                throw new ArgumentNullException("source");
            if (selector == null)
                throw new ArgumentNullException("selector");

            return Max(source, selector, CancellationToken.None);
        }

        public static Task<long> Max<TSource>(this IAsyncEnumerable<TSource> source, Func<TSource, long> selector)
        {
            if (source == null)
                throw new ArgumentNullException("source");
            if (selector == null)
                throw new ArgumentNullException("selector");

            return Max(source, selector, CancellationToken.None);
        }

        public static Task<double> Max<TSource>(this IAsyncEnumerable<TSource> source, Func<TSource, double> selector)
        {
            if (source == null)
                throw new ArgumentNullException("source");
            if (selector == null)
                throw new ArgumentNullException("selector");

            return Max(source, selector, CancellationToken.None);
        }

        public static Task<float> Max<TSource>(this IAsyncEnumerable<TSource> source, Func<TSource, float> selector)
        {
            if (source == null)
                throw new ArgumentNullException("source");
            if (selector == null)
                throw new ArgumentNullException("selector");

            return Max(source, selector, CancellationToken.None);
        }

        public static Task<decimal> Max<TSource>(this IAsyncEnumerable<TSource> source, Func<TSource, decimal> selector)
        {
            if (source == null)
                throw new ArgumentNullException("source");
            if (selector == null)
                throw new ArgumentNullException("selector");

            return Max(source, selector, CancellationToken.None);
        }

        public static Task<int?> Max<TSource>(this IAsyncEnumerable<TSource> source, Func<TSource, int?> selector)
        {
            if (source == null)
                throw new ArgumentNullException("source");
            if (selector == null)
                throw new ArgumentNullException("selector");

            return Max(source, selector, CancellationToken.None);
        }

        public static Task<long?> Max<TSource>(this IAsyncEnumerable<TSource> source, Func<TSource, long?> selector)
        {
            if (source == null)
                throw new ArgumentNullException("source");
            if (selector == null)
                throw new ArgumentNullException("selector");

            return Max(source, selector, CancellationToken.None);
        }

        public static Task<double?> Max<TSource>(this IAsyncEnumerable<TSource> source, Func<TSource, double?> selector)
        {
            if (source == null)
                throw new ArgumentNullException("source");
            if (selector == null)
                throw new ArgumentNullException("selector");

            return Max(source, selector, CancellationToken.None);
        }

        public static Task<float?> Max<TSource>(this IAsyncEnumerable<TSource> source, Func<TSource, float?> selector)
        {
            if (source == null)
                throw new ArgumentNullException("source");
            if (selector == null)
                throw new ArgumentNullException("selector");

            return Max(source, selector, CancellationToken.None);
        }

        public static Task<decimal?> Max<TSource>(this IAsyncEnumerable<TSource> source, Func<TSource, decimal?> selector)
        {
            if (source == null)
                throw new ArgumentNullException("source");
            if (selector == null)
                throw new ArgumentNullException("selector");

            return Max(source, selector, CancellationToken.None);
        }

        public static Task<TResult> Max<TSource, TResult>(this IAsyncEnumerable<TSource> source, Func<TSource, TResult> selector)
        {
            if (source == null)
                throw new ArgumentNullException("source");
            if (selector == null)
                throw new ArgumentNullException("selector");

            return Max(source, selector, CancellationToken.None);
        }

        public static Task<int> Min(this IAsyncEnumerable<int> source)
        {
            if (source == null)
                throw new ArgumentNullException("source");

            return Min(source, CancellationToken.None);
        }

        public static Task<long> Min(this IAsyncEnumerable<long> source)
        {
            if (source == null)
                throw new ArgumentNullException("source");

            return Min(source, CancellationToken.None);
        }

        public static Task<double> Min(this IAsyncEnumerable<double> source)
        {
            if (source == null)
                throw new ArgumentNullException("source");

            return Min(source, CancellationToken.None);
        }

        public static Task<float> Min(this IAsyncEnumerable<float> source)
        {
            if (source == null)
                throw new ArgumentNullException("source");

            return Min(source, CancellationToken.None);
        }

        public static Task<decimal> Min(this IAsyncEnumerable<decimal> source)
        {
            if (source == null)
                throw new ArgumentNullException("source");

            return Min(source, CancellationToken.None);
        }

        public static Task<int?> Min(this IAsyncEnumerable<int?> source)
        {
            if (source == null)
                throw new ArgumentNullException("source");

            return Min(source, CancellationToken.None);
        }

        public static Task<long?> Min(this IAsyncEnumerable<long?> source)
        {
            if (source == null)
                throw new ArgumentNullException("source");

            return Min(source, CancellationToken.None);
        }

        public static Task<double?> Min(this IAsyncEnumerable<double?> source)
        {
            if (source == null)
                throw new ArgumentNullException("source");

            return Min(source, CancellationToken.None);
        }

        public static Task<float?> Min(this IAsyncEnumerable<float?> source)
        {
            if (source == null)
                throw new ArgumentNullException("source");

            return Min(source, CancellationToken.None);
        }

        public static Task<decimal?> Min(this IAsyncEnumerable<decimal?> source)
        {
            if (source == null)
                throw new ArgumentNullException("source");

            return Min(source, CancellationToken.None);
        }

        public static Task<TSource> Min<TSource>(this IAsyncEnumerable<TSource> source)
        {
            if (source == null)
                throw new ArgumentNullException("source");

            return Min(source, CancellationToken.None);
        }

        public static Task<int> Min<TSource>(this IAsyncEnumerable<TSource> source, Func<TSource, int> selector)
        {
            if (source == null)
                throw new ArgumentNullException("source");
            if (selector == null)
                throw new ArgumentNullException("selector");

            return Min(source, selector, CancellationToken.None);
        }

        public static Task<long> Min<TSource>(this IAsyncEnumerable<TSource> source, Func<TSource, long> selector)
        {
            if (source == null)
                throw new ArgumentNullException("source");
            if (selector == null)
                throw new ArgumentNullException("selector");

            return Min(source, selector, CancellationToken.None);
        }

        public static Task<double> Min<TSource>(this IAsyncEnumerable<TSource> source, Func<TSource, double> selector)
        {
            if (source == null)
                throw new ArgumentNullException("source");
            if (selector == null)
                throw new ArgumentNullException("selector");

            return Min(source, selector, CancellationToken.None);
        }

        public static Task<float> Min<TSource>(this IAsyncEnumerable<TSource> source, Func<TSource, float> selector)
        {
            if (source == null)
                throw new ArgumentNullException("source");
            if (selector == null)
                throw new ArgumentNullException("selector");

            return Min(source, selector, CancellationToken.None);
        }

        public static Task<decimal> Min<TSource>(this IAsyncEnumerable<TSource> source, Func<TSource, decimal> selector)
        {
            if (source == null)
                throw new ArgumentNullException("source");
            if (selector == null)
                throw new ArgumentNullException("selector");

            return Min(source, selector, CancellationToken.None);
        }

        public static Task<int?> Min<TSource>(this IAsyncEnumerable<TSource> source, Func<TSource, int?> selector)
        {
            if (source == null)
                throw new ArgumentNullException("source");
            if (selector == null)
                throw new ArgumentNullException("selector");

            return Min(source, selector, CancellationToken.None);
        }

        public static Task<long?> Min<TSource>(this IAsyncEnumerable<TSource> source, Func<TSource, long?> selector)
        {
            if (source == null)
                throw new ArgumentNullException("source");
            if (selector == null)
                throw new ArgumentNullException("selector");

            return Min(source, selector, CancellationToken.None);
        }

        public static Task<double?> Min<TSource>(this IAsyncEnumerable<TSource> source, Func<TSource, double?> selector)
        {
            if (source == null)
                throw new ArgumentNullException("source");
            if (selector == null)
                throw new ArgumentNullException("selector");

            return Min(source, selector, CancellationToken.None);
        }

        public static Task<float?> Min<TSource>(this IAsyncEnumerable<TSource> source, Func<TSource, float?> selector)
        {
            if (source == null)
                throw new ArgumentNullException("source");
            if (selector == null)
                throw new ArgumentNullException("selector");

            return Min(source, selector, CancellationToken.None);
        }

        public static Task<bool> SequenceEqual<TSource>(this IAsyncEnumerable<TSource> first, IAsyncEnumerable<TSource> second, IEqualityComparer<TSource> comparer)
        {
            if (first == null)
                throw new ArgumentNullException("first");
            if (second == null)
                throw new ArgumentNullException("second");
            if (comparer == null)
                throw new ArgumentNullException("comparer");

            return SequenceEqual(first, second, comparer, CancellationToken.None);
        }

        public static Task<bool> SequenceEqual<TSource>(this IAsyncEnumerable<TSource> first, IAsyncEnumerable<TSource> second)
        {
            if (first == null)
                throw new ArgumentNullException("first");
            if (second == null)
                throw new ArgumentNullException("second");

            return SequenceEqual(first, second, CancellationToken.None);
        }

        public static Task<TResult> Aggregate<TSource, TAccumulate, TResult>(this IAsyncEnumerable<TSource> source, TAccumulate seed, Func<TAccumulate, TSource, TAccumulate> accumulator, Func<TAccumulate, TResult> resultSelector)
        {
            if (source == null)
                throw new ArgumentNullException("source");
            if (accumulator == null)
                throw new ArgumentNullException("accumulator");
            if (resultSelector == null)
                throw new ArgumentNullException("resultSelector");

            return Aggregate(source, seed, accumulator, resultSelector, CancellationToken.None);
        }

        public static Task<TAccumulate> Aggregate<TSource, TAccumulate>(this IAsyncEnumerable<TSource> source, TAccumulate seed, Func<TAccumulate, TSource, TAccumulate> accumulator)
        {
            if (source == null)
                throw new ArgumentNullException("source");
            if (accumulator == null)
                throw new ArgumentNullException("accumulator");

            return Aggregate(source, seed, accumulator, CancellationToken.None);
        }

        public static Task<TSource> Aggregate<TSource>(this IAsyncEnumerable<TSource> source, Func<TSource, TSource, TSource> accumulator)
        {
            if (source == null)
                throw new ArgumentNullException("source");
            if (accumulator == null)
                throw new ArgumentNullException("accumulator");

            return Aggregate(source, accumulator, CancellationToken.None);
        }

        public static Task<int> Count<TSource>(this IAsyncEnumerable<TSource> source)
        {
            if (source == null)
                throw new ArgumentNullException("source");

            return Count(source, CancellationToken.None);
        }

        public static Task<int> Count<TSource>(this IAsyncEnumerable<TSource> source, Func<TSource, bool> predicate)
        {
            if (source == null)
                throw new ArgumentNullException("source");
            if (predicate == null)
                throw new ArgumentNullException("predicate");

            return Count(source, predicate, CancellationToken.None);
        }

        public static Task<long> LongCount<TSource>(this IAsyncEnumerable<TSource> source)
        {
            if (source == null)
                throw new ArgumentNullException("source");

            return LongCount(source, CancellationToken.None);
        }

        public static Task<long> LongCount<TSource>(this IAsyncEnumerable<TSource> source, Func<TSource, bool> predicate)
        {
            if (source == null)
                throw new ArgumentNullException("source");
            if (predicate == null)
                throw new ArgumentNullException("predicate");

            return LongCount(source, predicate, CancellationToken.None);
        }

        public static Task<bool> All<TSource>(this IAsyncEnumerable<TSource> source, Func<TSource, bool> predicate)
        {
            if (source == null)
                throw new ArgumentNullException("source");
            if (predicate == null)
                throw new ArgumentNullException("predicate");

            return All(source, predicate, CancellationToken.None);
        }

        public static Task<bool> Any<TSource>(this IAsyncEnumerable<TSource> source, Func<TSource, bool> predicate)
        {
            if (source == null)
                throw new ArgumentNullException("source");
            if (predicate == null)
                throw new ArgumentNullException("predicate");

            return Any(source, predicate, CancellationToken.None);
        }

        public static Task<bool> Any<TSource>(this IAsyncEnumerable<TSource> source)
        {
            if (source == null)
                throw new ArgumentNullException("source");

            return Any(source, CancellationToken.None);
        }

        public static Task<bool> Contains<TSource>(this IAsyncEnumerable<TSource> source, TSource value, IEqualityComparer<TSource> comparer)
        {
            if (source == null)
                throw new ArgumentNullException("source");
            if (comparer == null)
                throw new ArgumentNullException("comparer");

            return Contains(source, value, comparer, CancellationToken.None);
        }

        public static Task<bool> Contains<TSource>(this IAsyncEnumerable<TSource> source, TSource value)
        {
            if (source == null)
                throw new ArgumentNullException("source");

            return Contains(source, value, CancellationToken.None);
        }

        public static Task<TSource> First<TSource>(this IAsyncEnumerable<TSource> source)
        {
            if (source == null)
                throw new ArgumentNullException("source");

            return First(source, CancellationToken.None);
        }

        public static Task<TSource> First<TSource>(this IAsyncEnumerable<TSource> source, Func<TSource, bool> predicate)
        {
            if (source == null)
                throw new ArgumentNullException("source");
            if (predicate == null)
                throw new ArgumentNullException("predicate");

            return First(source, predicate, CancellationToken.None);
        }

        public static Task<TSource> FirstOrDefault<TSource>(this IAsyncEnumerable<TSource> source)
        {
            if (source == null)
                throw new ArgumentNullException("source");

            return FirstOrDefault(source, CancellationToken.None);
        }

        public static Task<TSource> FirstOrDefault<TSource>(this IAsyncEnumerable<TSource> source, Func<TSource, bool> predicate)
        {
            if (source == null)
                throw new ArgumentNullException("source");
            if (predicate == null)
                throw new ArgumentNullException("predicate");

            return FirstOrDefault(source, predicate, CancellationToken.None);
        }

        public static Task<TSource> Last<TSource>(this IAsyncEnumerable<TSource> source)
        {
            if (source == null)
                throw new ArgumentNullException("source");

            return Last(source, CancellationToken.None);
        }

        public static Task<TSource> Last<TSource>(this IAsyncEnumerable<TSource> source, Func<TSource, bool> predicate)
        {
            if (source == null)
                throw new ArgumentNullException("source");
            if (predicate == null)
                throw new ArgumentNullException("predicate");

            return Last(source, predicate, CancellationToken.None);
        }

        public static Task<TSource> LastOrDefault<TSource>(this IAsyncEnumerable<TSource> source)
        {
            if (source == null)
                throw new ArgumentNullException("source");

            return LastOrDefault(source, CancellationToken.None);
        }

        public static Task<TSource> LastOrDefault<TSource>(this IAsyncEnumerable<TSource> source, Func<TSource, bool> predicate)
        {
            if (source == null)
                throw new ArgumentNullException("source");
            if (predicate == null)
                throw new ArgumentNullException("predicate");

            return LastOrDefault(source, predicate, CancellationToken.None);
        }

        public static Task<TSource> Single<TSource>(this IAsyncEnumerable<TSource> source)
        {
            if (source == null)
                throw new ArgumentNullException("source");

            return Single(source, CancellationToken.None);
        }

        public static Task<TSource> Single<TSource>(this IAsyncEnumerable<TSource> source, Func<TSource, bool> predicate)
        {
            if (source == null)
                throw new ArgumentNullException("source");
            if (predicate == null)
                throw new ArgumentNullException("predicate");

            return Single(source, predicate, CancellationToken.None);
        }

        public static Task<TSource> SingleOrDefault<TSource>(this IAsyncEnumerable<TSource> source)
        {
            if (source == null)
                throw new ArgumentNullException("source");

            return SingleOrDefault(source, CancellationToken.None);
        }

        public static Task<TSource> SingleOrDefault<TSource>(this IAsyncEnumerable<TSource> source, Func<TSource, bool> predicate)
        {
            if (source == null)
                throw new ArgumentNullException("source");
            if (predicate == null)
                throw new ArgumentNullException("predicate");

            return SingleOrDefault(source, predicate, CancellationToken.None);
        }

        public static Task<TSource> ElementAt<TSource>(this IAsyncEnumerable<TSource> source, int index)
        {
            if (source == null)
                throw new ArgumentNullException("source");

            return ElementAt(source, index, CancellationToken.None);
        }

        public static Task<TSource> ElementAtOrDefault<TSource>(this IAsyncEnumerable<TSource> source, int index)
        {
            if (source == null)
                throw new ArgumentNullException("source");

            return ElementAtOrDefault(source, index, CancellationToken.None);
        }

        public static Task<TSource[]> ToArray<TSource>(this IAsyncEnumerable<TSource> source)
        {
            if (source == null)
                throw new ArgumentNullException("source");

            return ToArray(source, CancellationToken.None);
        }

        public static Task<List<TSource>> ToList<TSource>(this IAsyncEnumerable<TSource> source)
        {
            if (source == null)
                throw new ArgumentNullException("source");

            return ToList(source, CancellationToken.None);
        }

        public static Task<Dictionary<TKey, TElement>> ToDictionary<TSource, TKey, TElement>(this IAsyncEnumerable<TSource> source, Func<TSource, TKey> keySelector, Func<TSource, TElement> elementSelector, IEqualityComparer<TKey> comparer)
        {
            if (source == null)
                throw new ArgumentNullException("source");
            if (keySelector == null)
                throw new ArgumentNullException("keySelector");
            if (elementSelector == null)
                throw new ArgumentNullException("elementSelector");
            if (comparer == null)
                throw new ArgumentNullException("comparer");

            return ToDictionary(source, keySelector, elementSelector, comparer, CancellationToken.None);
        }

        public static Task<Dictionary<TKey, TElement>> ToDictionary<TSource, TKey, TElement>(this IAsyncEnumerable<TSource> source, Func<TSource, TKey> keySelector, Func<TSource, TElement> elementSelector)
        {
            if (source == null)
                throw new ArgumentNullException("source");
            if (keySelector == null)
                throw new ArgumentNullException("keySelector");
            if (elementSelector == null)
                throw new ArgumentNullException("elementSelector");

            return ToDictionary(source, keySelector, elementSelector, CancellationToken.None);
        }

        public static Task<Dictionary<TKey, TSource>> ToDictionary<TSource, TKey>(this IAsyncEnumerable<TSource> source, Func<TSource, TKey> keySelector, IEqualityComparer<TKey> comparer)
        {
            if (source == null)
                throw new ArgumentNullException("source");
            if (keySelector == null)
                throw new ArgumentNullException("keySelector");
            if (comparer == null)
                throw new ArgumentNullException("comparer");

            return ToDictionary(source, keySelector, comparer, CancellationToken.None);
        }

        public static Task<Dictionary<TKey, TSource>> ToDictionary<TSource, TKey>(this IAsyncEnumerable<TSource> source, Func<TSource, TKey> keySelector)
        {
            if (source == null)
                throw new ArgumentNullException("source");
            if (keySelector == null)
                throw new ArgumentNullException("keySelector");

            return ToDictionary(source, keySelector, CancellationToken.None);
        }

        public static Task<ILookup<TKey, TElement>> ToLookup<TSource, TKey, TElement>(this IAsyncEnumerable<TSource> source, Func<TSource, TKey> keySelector, Func<TSource, TElement> elementSelector, IEqualityComparer<TKey> comparer)
        {
            if (source == null)
                throw new ArgumentNullException("source");
            if (keySelector == null)
                throw new ArgumentNullException("keySelector");
            if (elementSelector == null)
                throw new ArgumentNullException("elementSelector");
            if (comparer == null)
                throw new ArgumentNullException("comparer");

            return ToLookup(source, keySelector, elementSelector, comparer, CancellationToken.None);
        }

        public static Task<ILookup<TKey, TElement>> ToLookup<TSource, TKey, TElement>(this IAsyncEnumerable<TSource> source, Func<TSource, TKey> keySelector, Func<TSource, TElement> elementSelector)
        {
            if (source == null)
                throw new ArgumentNullException("source");
            if (keySelector == null)
                throw new ArgumentNullException("keySelector");
            if (elementSelector == null)
                throw new ArgumentNullException("elementSelector");

            return ToLookup(source, keySelector, elementSelector, CancellationToken.None);
        }

        public static Task<ILookup<TKey, TSource>> ToLookup<TSource, TKey>(this IAsyncEnumerable<TSource> source, Func<TSource, TKey> keySelector, IEqualityComparer<TKey> comparer)
        {
            if (source == null)
                throw new ArgumentNullException("source");
            if (keySelector == null)
                throw new ArgumentNullException("keySelector");
            if (comparer == null)
                throw new ArgumentNullException("comparer");

            return ToLookup(source, keySelector, comparer, CancellationToken.None);
        }

        public static Task<ILookup<TKey, TSource>> ToLookup<TSource, TKey>(this IAsyncEnumerable<TSource> source, Func<TSource, TKey> keySelector)
        {
            if (source == null)
                throw new ArgumentNullException("source");
            if (keySelector == null)
                throw new ArgumentNullException("keySelector");

            return ToLookup(source, keySelector, CancellationToken.None);
        }

        public static Task<double> Average(this IAsyncEnumerable<int> source)
        {
            if (source == null)
                throw new ArgumentNullException("source");

            return Average(source, CancellationToken.None);
        }

        public static Task<double?> Average(this IAsyncEnumerable<int?> source)
        {
            if (source == null)
                throw new ArgumentNullException("source");

            return Average(source, CancellationToken.None);
        }

        public static Task<bool> IsEmpty<TSource>(this IAsyncEnumerable<TSource> source)
        {
            if (source == null)
                throw new ArgumentNullException("source");

            return source.IsEmpty(CancellationToken.None);
        }

        public static Task<TSource> Min<TSource>(this IAsyncEnumerable<TSource> source, IComparer<TSource> comparer)
        {
            if (source == null)
                throw new ArgumentNullException("source");
            if (comparer == null)
                throw new ArgumentNullException("comparer");

            return source.Min(comparer, CancellationToken.None);
        }

        public static Task<IList<TSource>> MinBy<TSource, TKey>(this IAsyncEnumerable<TSource> source, Func<TSource, TKey> keySelector)
        {
            if (source == null)
                throw new ArgumentNullException("source");
            if (keySelector == null)
                throw new ArgumentNullException("keySelector");

            return source.MinBy(keySelector, CancellationToken.None);
        }

        public static Task<IList<TSource>> MinBy<TSource, TKey>(this IAsyncEnumerable<TSource> source, Func<TSource, TKey> keySelector, IComparer<TKey> comparer)
        {
            if (source == null)
                throw new ArgumentNullException("source");
            if (keySelector == null)
                throw new ArgumentNullException("keySelector");
            if (comparer == null)
                throw new ArgumentNullException("comparer");

            return source.MinBy(keySelector, comparer, CancellationToken.None);
        }

        public static Task<TSource> Max<TSource>(this IAsyncEnumerable<TSource> source, IComparer<TSource> comparer)
        {
            if (source == null)
                throw new ArgumentNullException("source");
            if (comparer == null)
                throw new ArgumentNullException("comparer");

            return source.Max(comparer, CancellationToken.None);
        }

        public static Task<IList<TSource>> MaxBy<TSource, TKey>(this IAsyncEnumerable<TSource> source, Func<TSource, TKey> keySelector)
        {
            if (source == null)
                throw new ArgumentNullException("source");
            if (keySelector == null)
                throw new ArgumentNullException("keySelector");

            return source.MaxBy(keySelector, CancellationToken.None);
        }

        public static Task<IList<TSource>> MaxBy<TSource, TKey>(this IAsyncEnumerable<TSource> source, Func<TSource, TKey> keySelector, IComparer<TKey> comparer)
        {
            if (source == null)
                throw new ArgumentNullException("source");
            if (keySelector == null)
                throw new ArgumentNullException("keySelector");
            if (comparer == null)
                throw new ArgumentNullException("comparer");

            return source.MaxBy(keySelector, comparer, CancellationToken.None);
        }
    }
}

// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the Apache 2.0 License.
// See the LICENSE file in the project root for more information. 

using System.Collections.Generic;
using System.Linq;

namespace System.Reactive.Linq
{
    using ObservableImpl;

    internal partial class QueryLanguage
    {
        #region + Aggregate +

        public virtual IObservable<TAccumulate> Aggregate<TSource, TAccumulate>(IObservable<TSource> source, TAccumulate seed, Func<TAccumulate, TSource, TAccumulate> accumulator)
        {
            return new Aggregate<TSource, TAccumulate>(source, seed, accumulator);
        }

        public virtual IObservable<TResult> Aggregate<TSource, TAccumulate, TResult>(IObservable<TSource> source, TAccumulate seed, Func<TAccumulate, TSource, TAccumulate> accumulator, Func<TAccumulate, TResult> resultSelector)
        {
            return new Aggregate<TSource, TAccumulate, TResult>(source, seed, accumulator, resultSelector);
        }

        public virtual IObservable<TSource> Aggregate<TSource>(IObservable<TSource> source, Func<TSource, TSource, TSource> accumulator)
        {
            return new Aggregate<TSource>(source, accumulator);
        }

        public virtual IObservable<double> Average<TSource>(IObservable<TSource> source, Func<TSource, double> selector)
        {
            return Average(Select(source, selector));
        }

        public virtual IObservable<float> Average<TSource>(IObservable<TSource> source, Func<TSource, float> selector)
        {
            return Average(Select(source, selector));
        }

        public virtual IObservable<decimal> Average<TSource>(IObservable<TSource> source, Func<TSource, decimal> selector)
        {
            return Average(Select(source, selector));
        }

        public virtual IObservable<double> Average<TSource>(IObservable<TSource> source, Func<TSource, int> selector)
        {
            return Average(Select(source, selector));
        }

        public virtual IObservable<double> Average<TSource>(IObservable<TSource> source, Func<TSource, long> selector)
        {
            return Average(Select(source, selector));
        }

        public virtual IObservable<double?> Average<TSource>(IObservable<TSource> source, Func<TSource, double?> selector)
        {
            return Average(Select(source, selector));
        }

        public virtual IObservable<float?> Average<TSource>(IObservable<TSource> source, Func<TSource, float?> selector)
        {
            return Average(Select(source, selector));
        }

        public virtual IObservable<decimal?> Average<TSource>(IObservable<TSource> source, Func<TSource, decimal?> selector)
        {
            return Average(Select(source, selector));
        }

        public virtual IObservable<double?> Average<TSource>(IObservable<TSource> source, Func<TSource, int?> selector)
        {
            return Average(Select(source, selector));
        }

        public virtual IObservable<double?> Average<TSource>(IObservable<TSource> source, Func<TSource, long?> selector)
        {
            return Average(Select(source, selector));
        }

        #endregion

        #region + All +

        public virtual IObservable<bool> All<TSource>(IObservable<TSource> source, Func<TSource, bool> predicate)
        {
            return new All<TSource>(source, predicate);
        }

        #endregion

        #region + Any +

        public virtual IObservable<bool> Any<TSource>(IObservable<TSource> source)
        {
            return new Any<TSource>.Count(source);
        }

        public virtual IObservable<bool> Any<TSource>(IObservable<TSource> source, Func<TSource, bool> predicate)
        {
            return new Any<TSource>.Predicate(source, predicate);
        }

        #endregion

        #region + Average +

        public virtual IObservable<double> Average(IObservable<double> source)
        {
            return new AverageDouble(source);
        }

        public virtual IObservable<float> Average(IObservable<float> source)
        {
            return new AverageSingle(source);
        }

        public virtual IObservable<decimal> Average(IObservable<decimal> source)
        {
            return new AverageDecimal(source);
        }

        public virtual IObservable<double> Average(IObservable<int> source)
        {
            return new AverageInt32(source);
        }

        public virtual IObservable<double> Average(IObservable<long> source)
        {
            return new AverageInt64(source);
        }

        public virtual IObservable<double?> Average(IObservable<double?> source)
        {
            return new AverageDoubleNullable(source);
        }

        public virtual IObservable<float?> Average(IObservable<float?> source)
        {
            return new AverageSingleNullable(source);
        }

        public virtual IObservable<decimal?> Average(IObservable<decimal?> source)
        {
            return new AverageDecimalNullable(source);
        }

        public virtual IObservable<double?> Average(IObservable<int?> source)
        {
            return new AverageInt32Nullable(source);
        }

        public virtual IObservable<double?> Average(IObservable<long?> source)
        {
            return new AverageInt64Nullable(source);
        }

        #endregion

        #region + Contains +

        public virtual IObservable<bool> Contains<TSource>(IObservable<TSource> source, TSource value)
        {
            return new Contains<TSource>(source, value, EqualityComparer<TSource>.Default);
        }

        public virtual IObservable<bool> Contains<TSource>(IObservable<TSource> source, TSource value, IEqualityComparer<TSource> comparer)
        {
            return new Contains<TSource>(source, value, comparer);
        }

        #endregion

        #region + Count +

        public virtual IObservable<int> Count<TSource>(IObservable<TSource> source)
        {
            return new Count<TSource>.All(source);
        }

        public virtual IObservable<int> Count<TSource>(IObservable<TSource> source, Func<TSource, bool> predicate)
        {
            return new Count<TSource>.Predicate(source, predicate);
        }

        #endregion

        #region + ElementAt +

        public virtual IObservable<TSource> ElementAt<TSource>(IObservable<TSource> source, int index)
        {
            return new ElementAt<TSource>(source, index);
        }

        #endregion

        #region + ElementAtOrDefault +

        public virtual IObservable<TSource> ElementAtOrDefault<TSource>(IObservable<TSource> source, int index)
        {
            return new ElementAtOrDefault<TSource>(source, index);
        }

        #endregion

        #region + FirstAsync +

        public virtual IObservable<TSource> FirstAsync<TSource>(IObservable<TSource> source)
        {
            return new FirstAsync<TSource>.Sequence(source);
        }

        public virtual IObservable<TSource> FirstAsync<TSource>(IObservable<TSource> source, Func<TSource, bool> predicate)
        {
            return new FirstAsync<TSource>.Predicate(source, predicate);
        }

        #endregion

        #region + FirstAsyncOrDefaultAsync +

        public virtual IObservable<TSource> FirstOrDefaultAsync<TSource>(IObservable<TSource> source)
        {
            return new FirstOrDefaultAsync<TSource>.Sequence(source);
        }

        public virtual IObservable<TSource> FirstOrDefaultAsync<TSource>(IObservable<TSource> source, Func<TSource, bool> predicate)
        {
            return new FirstOrDefaultAsync<TSource>.Predicate(source, predicate);
        }

        #endregion

        #region + IsEmpty +

        public virtual IObservable<bool> IsEmpty<TSource>(IObservable<TSource> source)
        {
            return new IsEmpty<TSource>(source);
        }

        #endregion

        #region + LastAsync +

        public virtual IObservable<TSource> LastAsync<TSource>(IObservable<TSource> source)
        {
            return new LastAsync<TSource>.Sequence(source);
        }

        public virtual IObservable<TSource> LastAsync<TSource>(IObservable<TSource> source, Func<TSource, bool> predicate)
        {
            return new LastAsync<TSource>.Predicate(source, predicate);
        }

        #endregion

        #region + LastOrDefaultAsync +

        public virtual IObservable<TSource> LastOrDefaultAsync<TSource>(IObservable<TSource> source)
        {
            return new LastOrDefaultAsync<TSource>.Sequence(source);
        }

        public virtual IObservable<TSource> LastOrDefaultAsync<TSource>(IObservable<TSource> source, Func<TSource, bool> predicate)
        {
            return new LastOrDefaultAsync<TSource>.Predicate(source, predicate);
        }

        #endregion

        #region + LongCount +

        public virtual IObservable<long> LongCount<TSource>(IObservable<TSource> source)
        {
            return new LongCount<TSource>.All(source);
        }

        public virtual IObservable<long> LongCount<TSource>(IObservable<TSource> source, Func<TSource, bool> predicate)
        {
            return new LongCount<TSource>.Predicate(source, predicate);
        }

        #endregion

        #region + Max +

        public virtual IObservable<TSource> Max<TSource>(IObservable<TSource> source)
        {
            // BREAKING CHANGE v2 > v1.x - Behavior for reference types
            return new Max<TSource>(source, Comparer<TSource>.Default);
        }

        public virtual IObservable<TSource> Max<TSource>(IObservable<TSource> source, IComparer<TSource> comparer)
        {
            // BREAKING CHANGE v2 > v1.x - Behavior for reference types
            return new Max<TSource>(source, comparer);
        }

        public virtual IObservable<double> Max(IObservable<double> source)
        {
            return new MaxDouble(source);
        }

        public virtual IObservable<float> Max(IObservable<float> source)
        {
            return new MaxSingle(source);
        }

        public virtual IObservable<decimal> Max(IObservable<decimal> source)
        {
            return new MaxDecimal(source);
        }

        public virtual IObservable<int> Max(IObservable<int> source)
        {
            return new MaxInt32(source);
        }

        public virtual IObservable<long> Max(IObservable<long> source)
        {
            return new MaxInt64(source);
        }

        public virtual IObservable<double?> Max(IObservable<double?> source)
        {
            return new MaxDoubleNullable(source);
        }

        public virtual IObservable<float?> Max(IObservable<float?> source)
        {
            return new MaxSingleNullable(source);
        }

        public virtual IObservable<decimal?> Max(IObservable<decimal?> source)
        {
            return new MaxDecimalNullable(source);
        }

        public virtual IObservable<int?> Max(IObservable<int?> source)
        {
            return new MaxInt32Nullable(source);
        }

        public virtual IObservable<long?> Max(IObservable<long?> source)
        {
            return new MaxInt64Nullable(source);
        }

        public virtual IObservable<TResult> Max<TSource, TResult>(IObservable<TSource> source, Func<TSource, TResult> selector)
        {
            return Max(Select(source, selector));
        }

        public virtual IObservable<TResult> Max<TSource, TResult>(IObservable<TSource> source, Func<TSource, TResult> selector, IComparer<TResult> comparer)
        {
            return Max(Select(source, selector), comparer);
        }

        public virtual IObservable<double> Max<TSource>(IObservable<TSource> source, Func<TSource, double> selector)
        {
            return Max(Select(source, selector));
        }

        public virtual IObservable<float> Max<TSource>(IObservable<TSource> source, Func<TSource, float> selector)
        {
            return Max(Select(source, selector));
        }

        public virtual IObservable<decimal> Max<TSource>(IObservable<TSource> source, Func<TSource, decimal> selector)
        {
            return Max(Select(source, selector));
        }

        public virtual IObservable<int> Max<TSource>(IObservable<TSource> source, Func<TSource, int> selector)
        {
            return Max(Select(source, selector));
        }

        public virtual IObservable<long> Max<TSource>(IObservable<TSource> source, Func<TSource, long> selector)
        {
            return Max(Select(source, selector));
        }

        public virtual IObservable<double?> Max<TSource>(IObservable<TSource> source, Func<TSource, double?> selector)
        {
            return Max(Select(source, selector));
        }

        public virtual IObservable<float?> Max<TSource>(IObservable<TSource> source, Func<TSource, float?> selector)
        {
            return Max(Select(source, selector));
        }

        public virtual IObservable<decimal?> Max<TSource>(IObservable<TSource> source, Func<TSource, decimal?> selector)
        {
            return Max(Select(source, selector));
        }

        public virtual IObservable<int?> Max<TSource>(IObservable<TSource> source, Func<TSource, int?> selector)
        {
            return Max(Select(source, selector));
        }

        public virtual IObservable<long?> Max<TSource>(IObservable<TSource> source, Func<TSource, long?> selector)
        {
            return Max(Select(source, selector));
        }

        #endregion

        #region + MaxBy +

        public virtual IObservable<IList<TSource>> MaxBy<TSource, TKey>(IObservable<TSource> source, Func<TSource, TKey> keySelector)
        {
            return new MaxBy<TSource, TKey>(source, keySelector, Comparer<TKey>.Default);
        }

        public virtual IObservable<IList<TSource>> MaxBy<TSource, TKey>(IObservable<TSource> source, Func<TSource, TKey> keySelector, IComparer<TKey> comparer)
        {
            return new MaxBy<TSource, TKey>(source, keySelector, comparer);
        }

        #endregion

        #region + Min +

        public virtual IObservable<TSource> Min<TSource>(IObservable<TSource> source)
        {
            // BREAKING CHANGE v2 > v1.x - Behavior for reference types
            return new Min<TSource>(source, Comparer<TSource>.Default);
        }

        public virtual IObservable<TSource> Min<TSource>(IObservable<TSource> source, IComparer<TSource> comparer)
        {
            // BREAKING CHANGE v2 > v1.x - Behavior for reference types
            return new Min<TSource>(source, comparer);
        }

        public virtual IObservable<double> Min(IObservable<double> source)
        {
            return new MinDouble(source);
        }

        public virtual IObservable<float> Min(IObservable<float> source)
        {
            return new MinSingle(source);
        }

        public virtual IObservable<decimal> Min(IObservable<decimal> source)
        {
            return new MinDecimal(source);
        }

        public virtual IObservable<int> Min(IObservable<int> source)
        {
            return new MinInt32(source);
        }

        public virtual IObservable<long> Min(IObservable<long> source)
        {
            return new MinInt64(source);
        }

        public virtual IObservable<double?> Min(IObservable<double?> source)
        {
            return new MinDoubleNullable(source);
        }

        public virtual IObservable<float?> Min(IObservable<float?> source)
        {
            return new MinSingleNullable(source);
        }

        public virtual IObservable<decimal?> Min(IObservable<decimal?> source)
        {
            return new MinDecimalNullable(source);
        }

        public virtual IObservable<int?> Min(IObservable<int?> source)
        {
            return new MinInt32Nullable(source);
        }

        public virtual IObservable<long?> Min(IObservable<long?> source)
        {
            return new MinInt64Nullable(source);
        }

        public virtual IObservable<TResult> Min<TSource, TResult>(IObservable<TSource> source, Func<TSource, TResult> selector)
        {
            return Min(Select(source, selector));
        }

        public virtual IObservable<TResult> Min<TSource, TResult>(IObservable<TSource> source, Func<TSource, TResult> selector, IComparer<TResult> comparer)
        {
            return Min(Select(source, selector), comparer);
        }

        public virtual IObservable<double> Min<TSource>(IObservable<TSource> source, Func<TSource, double> selector)
        {
            return Min(Select(source, selector));
        }

        public virtual IObservable<float> Min<TSource>(IObservable<TSource> source, Func<TSource, float> selector)
        {
            return Min(Select(source, selector));
        }

        public virtual IObservable<decimal> Min<TSource>(IObservable<TSource> source, Func<TSource, decimal> selector)
        {
            return Min(Select(source, selector));
        }

        public virtual IObservable<int> Min<TSource>(IObservable<TSource> source, Func<TSource, int> selector)
        {
            return Min(Select(source, selector));
        }

        public virtual IObservable<long> Min<TSource>(IObservable<TSource> source, Func<TSource, long> selector)
        {
            return Min(Select(source, selector));
        }

        public virtual IObservable<double?> Min<TSource>(IObservable<TSource> source, Func<TSource, double?> selector)
        {
            return Min(Select(source, selector));
        }

        public virtual IObservable<float?> Min<TSource>(IObservable<TSource> source, Func<TSource, float?> selector)
        {
            return Min(Select(source, selector));
        }

        public virtual IObservable<decimal?> Min<TSource>(IObservable<TSource> source, Func<TSource, decimal?> selector)
        {
            return Min(Select(source, selector));
        }

        public virtual IObservable<int?> Min<TSource>(IObservable<TSource> source, Func<TSource, int?> selector)
        {
            return Min(Select(source, selector));
        }

        public virtual IObservable<long?> Min<TSource>(IObservable<TSource> source, Func<TSource, long?> selector)
        {
            return Min(Select(source, selector));
        }

        #endregion

        #region + MinBy +

        public virtual IObservable<IList<TSource>> MinBy<TSource, TKey>(IObservable<TSource> source, Func<TSource, TKey> keySelector)
        {
            return new MinBy<TSource, TKey>(source, keySelector, Comparer<TKey>.Default);
        }

        public virtual IObservable<IList<TSource>> MinBy<TSource, TKey>(IObservable<TSource> source, Func<TSource, TKey> keySelector, IComparer<TKey> comparer)
        {
            return new MinBy<TSource, TKey>(source, keySelector, comparer);
        }

        #endregion

        #region + SequenceEqual +

        public virtual IObservable<bool> SequenceEqual<TSource>(IObservable<TSource> first, IObservable<TSource> second)
        {
            return new SequenceEqual<TSource>.Observable(first, second, EqualityComparer<TSource>.Default);
        }

        public virtual IObservable<bool> SequenceEqual<TSource>(IObservable<TSource> first, IObservable<TSource> second, IEqualityComparer<TSource> comparer)
        {
            return new SequenceEqual<TSource>.Observable(first, second, comparer);
        }

        public virtual IObservable<bool> SequenceEqual<TSource>(IObservable<TSource> first, IEnumerable<TSource> second)
        {
            return new SequenceEqual<TSource>.Enumerable(first, second, EqualityComparer<TSource>.Default);
        }

        public virtual IObservable<bool> SequenceEqual<TSource>(IObservable<TSource> first, IEnumerable<TSource> second, IEqualityComparer<TSource> comparer)
        {
            return new SequenceEqual<TSource>.Enumerable(first, second, comparer);
        }

        #endregion

        #region + SingleAsync +

        public virtual IObservable<TSource> SingleAsync<TSource>(IObservable<TSource> source)
        {
            return new SingleAsync<TSource>.Sequence(source);
        }

        public virtual IObservable<TSource> SingleAsync<TSource>(IObservable<TSource> source, Func<TSource, bool> predicate)
        {
            return new SingleAsync<TSource>.Predicate(source, predicate);
        }

        #endregion

        #region + SingleOrDefaultAsync +

        public virtual IObservable<TSource> SingleOrDefaultAsync<TSource>(IObservable<TSource> source)
        {
            return new SingleOrDefaultAsync<TSource>.Sequence(source);
        }

        public virtual IObservable<TSource> SingleOrDefaultAsync<TSource>(IObservable<TSource> source, Func<TSource, bool> predicate)
        {
            return new SingleOrDefaultAsync<TSource>.Predicate(source, predicate);
        }

        #endregion

        #region + Sum +

        public virtual IObservable<double> Sum(IObservable<double> source)
        {
            return new SumDouble(source);
        }

        public virtual IObservable<float> Sum(IObservable<float> source)
        {
            return new SumSingle(source);
        }

        public virtual IObservable<decimal> Sum(IObservable<decimal> source)
        {
            return new SumDecimal(source);
        }

        public virtual IObservable<int> Sum(IObservable<int> source)
        {
            return new SumInt32(source);
        }

        public virtual IObservable<long> Sum(IObservable<long> source)
        {
            return new SumInt64(source);
        }

        public virtual IObservable<double?> Sum(IObservable<double?> source)
        {
            return new SumDoubleNullable(source);
        }

        public virtual IObservable<float?> Sum(IObservable<float?> source)
        {
            return new SumSingleNullable(source);
        }

        public virtual IObservable<decimal?> Sum(IObservable<decimal?> source)
        {
            return new SumDecimalNullable(source);
        }

        public virtual IObservable<int?> Sum(IObservable<int?> source)
        {
            return new SumInt32Nullable(source);
        }

        public virtual IObservable<long?> Sum(IObservable<long?> source)
        {
            return new SumInt64Nullable(source);
        }

        public virtual IObservable<double> Sum<TSource>(IObservable<TSource> source, Func<TSource, double> selector)
        {
            return Sum(Select(source, selector));
        }

        public virtual IObservable<float> Sum<TSource>(IObservable<TSource> source, Func<TSource, float> selector)
        {
            return Sum(Select(source, selector));
        }

        public virtual IObservable<decimal> Sum<TSource>(IObservable<TSource> source, Func<TSource, decimal> selector)
        {
            return Sum(Select(source, selector));
        }

        public virtual IObservable<int> Sum<TSource>(IObservable<TSource> source, Func<TSource, int> selector)
        {
            return Sum(Select(source, selector));
        }

        public virtual IObservable<long> Sum<TSource>(IObservable<TSource> source, Func<TSource, long> selector)
        {
            return Sum(Select(source, selector));
        }

        public virtual IObservable<double?> Sum<TSource>(IObservable<TSource> source, Func<TSource, double?> selector)
        {
            return Sum(Select(source, selector));
        }

        public virtual IObservable<float?> Sum<TSource>(IObservable<TSource> source, Func<TSource, float?> selector)
        {
            return Sum(Select(source, selector));
        }

        public virtual IObservable<decimal?> Sum<TSource>(IObservable<TSource> source, Func<TSource, decimal?> selector)
        {
            return Sum(Select(source, selector));
        }

        public virtual IObservable<int?> Sum<TSource>(IObservable<TSource> source, Func<TSource, int?> selector)
        {
            return Sum(Select(source, selector));
        }

        public virtual IObservable<long?> Sum<TSource>(IObservable<TSource> source, Func<TSource, long?> selector)
        {
            return Sum(Select(source, selector));
        }

        #endregion

        #region + ToArray +

        public virtual IObservable<TSource[]> ToArray<TSource>(IObservable<TSource> source)
        {
            return new ToArray<TSource>(source);
        }

        #endregion

        #region + ToDictionary +

        public virtual IObservable<IDictionary<TKey, TElement>> ToDictionary<TSource, TKey, TElement>(IObservable<TSource> source, Func<TSource, TKey> keySelector, Func<TSource, TElement> elementSelector, IEqualityComparer<TKey> comparer)
        {
            return new ToDictionary<TSource, TKey, TElement>(source, keySelector, elementSelector, comparer);
        }

        public virtual IObservable<IDictionary<TKey, TElement>> ToDictionary<TSource, TKey, TElement>(IObservable<TSource> source, Func<TSource, TKey> keySelector, Func<TSource, TElement> elementSelector)
        {
            return new ToDictionary<TSource, TKey, TElement>(source, keySelector, elementSelector, EqualityComparer<TKey>.Default);
        }

        public virtual IObservable<IDictionary<TKey, TSource>> ToDictionary<TSource, TKey>(IObservable<TSource> source, Func<TSource, TKey> keySelector, IEqualityComparer<TKey> comparer)
        {
            return new ToDictionary<TSource, TKey, TSource>(source, keySelector, x => x, comparer);
        }

        public virtual IObservable<IDictionary<TKey, TSource>> ToDictionary<TSource, TKey>(IObservable<TSource> source, Func<TSource, TKey> keySelector)
        {
            return new ToDictionary<TSource, TKey, TSource>(source, keySelector, x => x, EqualityComparer<TKey>.Default);
        }

        #endregion

        #region + ToList +

        public virtual IObservable<IList<TSource>> ToList<TSource>(IObservable<TSource> source)
        {
            return new ToList<TSource>(source);
        }

        #endregion

        #region + ToLookup +

        public virtual IObservable<ILookup<TKey, TElement>> ToLookup<TSource, TKey, TElement>(IObservable<TSource> source, Func<TSource, TKey> keySelector, Func<TSource, TElement> elementSelector, IEqualityComparer<TKey> comparer)
        {
            return new ToLookup<TSource, TKey, TElement>(source, keySelector, elementSelector, comparer);
        }

        public virtual IObservable<ILookup<TKey, TSource>> ToLookup<TSource, TKey>(IObservable<TSource> source, Func<TSource, TKey> keySelector, IEqualityComparer<TKey> comparer)
        {
            return new ToLookup<TSource, TKey, TSource>(source, keySelector, x => x, comparer);
        }

        public virtual IObservable<ILookup<TKey, TElement>> ToLookup<TSource, TKey, TElement>(IObservable<TSource> source, Func<TSource, TKey> keySelector, Func<TSource, TElement> elementSelector)
        {
            return new ToLookup<TSource, TKey, TElement>(source, keySelector, elementSelector, EqualityComparer<TKey>.Default);
        }

        public virtual IObservable<ILookup<TKey, TSource>> ToLookup<TSource, TKey>(IObservable<TSource> source, Func<TSource, TKey> keySelector)
        {
            return new ToLookup<TSource, TKey, TSource>(source, keySelector, x => x, EqualityComparer<TKey>.Default);
        }

        #endregion
    }
}

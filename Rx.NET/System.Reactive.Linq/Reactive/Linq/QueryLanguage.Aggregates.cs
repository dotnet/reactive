// Copyright (c) Microsoft Open Technologies, Inc. All rights reserved. See License.txt in the project root for license information.

using System.Collections.Generic;
using System.Linq;
using System.Reactive.Disposables;

namespace System.Reactive.Linq
{
#if !NO_PERF
    using Observαble;
#endif

    internal partial class QueryLanguage
    {
        #region + Aggregate +

        public virtual IObservable<TAccumulate> Aggregate<TSource, TAccumulate>(IObservable<TSource> source, TAccumulate seed, Func<TAccumulate, TSource, TAccumulate> accumulator)
        {
#if !NO_PERF
            return new Aggregate<TSource, TAccumulate, TAccumulate>(source, seed, accumulator, Stubs<TAccumulate>.I);
#else
            return source.Scan(seed, accumulator).StartWith(seed).Final();
#endif
        }

        public virtual IObservable<TResult> Aggregate<TSource, TAccumulate, TResult>(IObservable<TSource> source, TAccumulate seed, Func<TAccumulate, TSource, TAccumulate> accumulator, Func<TAccumulate, TResult> resultSelector)
        {
#if !NO_PERF
            return new Aggregate<TSource, TAccumulate, TResult>(source, seed, accumulator, resultSelector);
#else
            return Aggregate(source, seed, accumulator).Select(resultSelector);
#endif
        }

        public virtual IObservable<TSource> Aggregate<TSource>(IObservable<TSource> source, Func<TSource, TSource, TSource> accumulator)
        {
#if !NO_PERF
            return new Aggregate<TSource>(source, accumulator);
#else
            return source.Scan(accumulator).Final();
#endif
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
#if !NO_PERF
            return new All<TSource>(source, predicate);
#else
            return source.Where(v => !(predicate(v))).Any().Select(b => !b);
#endif
        }

        #endregion

        #region + Any +

        public virtual IObservable<bool> Any<TSource>(IObservable<TSource> source)
        {
#if !NO_PERF
            return new Any<TSource>(source);
#else
            return new AnonymousObservable<bool>(observer => source.Subscribe(
                _ =>
                {
                    observer.OnNext(true);
                    observer.OnCompleted();
                },
                observer.OnError,
                () =>
                {
                    observer.OnNext(false);
                    observer.OnCompleted();
                }));
#endif
        }

        public virtual IObservable<bool> Any<TSource>(IObservable<TSource> source, Func<TSource, bool> predicate)
        {
#if !NO_PERF
            return new Any<TSource>(source, predicate);
#else
            return source.Where(predicate).Any();
#endif
        }

        #endregion

        #region + Average +

        public virtual IObservable<double> Average(IObservable<double> source)
        {
#if !NO_PERF
            return new AverageDouble(source);
#else
            return source.Scan(new { sum = 0.0, count = 0L },
                               (prev, cur) => new { sum = prev.sum + cur, count = checked(prev.count + 1) })
                .Final()
                .Select(s => s.sum / (double)s.count);
#endif
        }

        public virtual IObservable<float> Average(IObservable<float> source)
        {
#if !NO_PERF
            return new AverageSingle(source);
#else
            return source.Scan(new { sum = 0F, count = 0L }, // NOTE: Uses a different accumulator type (float), *not* conform LINQ to Objects.
                               (prev, cur) => new { sum = prev.sum + cur, count = checked(prev.count + 1) })
                .Final()
                .Select(s => s.sum / (float)s.count);
#endif
        }

        public virtual IObservable<decimal> Average(IObservable<decimal> source)
        {
#if !NO_PERF
            return new AverageDecimal(source);
#else
            return source.Scan(new { sum = 0M, count = 0L },
                               (prev, cur) => new { sum = prev.sum + cur, count = checked(prev.count + 1) })
                .Final()
                .Select(s => s.sum / (decimal)s.count);
#endif
        }

        public virtual IObservable<double> Average(IObservable<int> source)
        {
#if !NO_PERF
            return new AverageInt32(source);
#else
            return source.Scan(new { sum = 0L, count = 0L },
                               (prev, cur) => new { sum = checked(prev.sum + cur), count = checked(prev.count + 1) })
                .Final()
                .Select(s => (double)s.sum / (double)s.count);
#endif
        }

        public virtual IObservable<double> Average(IObservable<long> source)
        {
#if !NO_PERF
            return new AverageInt64(source);
#else
            return source.Scan(new { sum = 0L, count = 0L },
                               (prev, cur) => new { sum = checked(prev.sum + cur), count = checked(prev.count + 1) })
                .Final()
                .Select(s => (double)s.sum / (double)s.count);
#endif
        }

        public virtual IObservable<double?> Average(IObservable<double?> source)
        {
#if !NO_PERF
            return new AverageDoubleNullable(source);
#else
            return source.Aggregate(new { sum = new double?(0.0), count = 0L },
                               (prev, cur) => cur != null ? new { sum = prev.sum + cur.GetValueOrDefault(), count = checked(prev.count + 1) } : prev)                
                               .Select(s => s.count == 0 ? default(double?) : (double?)s.sum / (double)s.count);
#endif
        }

        public virtual IObservable<float?> Average(IObservable<float?> source)
        {
#if !NO_PERF
            return new AverageSingleNullable(source);
#else
            return source.Aggregate(new { sum = new float?(0f), count = 0L }, // NOTE: Uses a different accumulator type (float), *not* conform LINQ to Objects.
                               (prev, cur) => cur != null ? new { sum = prev.sum + cur.GetValueOrDefault(), count = checked(prev.count + 1) } : prev)
                               .Select(s => s.count == 0 ? default(float?) : (float?)s.sum / (float)s.count);
#endif
        }

        public virtual IObservable<decimal?> Average(IObservable<decimal?> source)
        {
#if !NO_PERF
            return new AverageDecimalNullable(source);
#else
            return source.Aggregate(new { sum = new decimal?(0M), count = 0L },
                               (prev, cur) => cur != null ? new { sum = prev.sum + cur.GetValueOrDefault(), count = checked(prev.count + 1) } : prev)
                               .Select(s => s.count == 0 ? default(decimal?) : (decimal?)s.sum / (decimal)s.count);
#endif
        }

        public virtual IObservable<double?> Average(IObservable<int?> source)
        {
#if !NO_PERF
            return new AverageInt32Nullable(source);
#else
            return source.Aggregate(new { sum = new long?(0), count = 0L },
                               (prev, cur) => cur != null ? new { sum = checked(prev.sum + cur.GetValueOrDefault()), count = checked(prev.count + 1) } : prev)
                               .Select(s => s.count == 0 ? default(double?) : (double?)s.sum / s.count);
#endif
        }

        public virtual IObservable<double?> Average(IObservable<long?> source)
        {
#if !NO_PERF
            return new AverageInt64Nullable(source);
#else
            return source.Aggregate(new { sum = new long?(0), count = 0L },
                               (prev, cur) => cur != null ? new { sum = checked(prev.sum + cur.GetValueOrDefault()), count = checked(prev.count + 1) } : prev)
                               .Select(s => s.count == 0 ? default(double?): (double?)s.sum / s.count);
#endif
        }

        #endregion

        #region + Contains +

        public virtual IObservable<bool> Contains<TSource>(IObservable<TSource> source, TSource value)
        {
#if !NO_PERF
            return new Contains<TSource>(source, value, EqualityComparer<TSource>.Default);
#else
            return Contains_<TSource>(source, value, EqualityComparer<TSource>.Default);
#endif
        }

        public virtual IObservable<bool> Contains<TSource>(IObservable<TSource> source, TSource value, IEqualityComparer<TSource> comparer)
        {
#if !NO_PERF
            return new Contains<TSource>(source, value, comparer);
#else
            return Contains_<TSource>(source, value, comparer);
#endif
        }

#if NO_PERF
        private static IObservable<bool> Contains_<TSource>(IObservable<TSource> source, TSource value, IEqualityComparer<TSource> comparer)
        {
            return source.Where(v => comparer.Equals(v, value)).Any();
        }
#endif

        #endregion

        #region + Count +

        public virtual IObservable<int> Count<TSource>(IObservable<TSource> source)
        {
#if !NO_PERF
            return new Count<TSource>(source);
#else
            return source.Aggregate(0, (count, _) => checked(count + 1));
#endif
        }

        public virtual IObservable<int> Count<TSource>(IObservable<TSource> source, Func<TSource, bool> predicate)
        {
#if !NO_PERF
            return new Count<TSource>(source, predicate);
#else
            return source.Where(predicate).Aggregate(0, (count, _) => checked(count + 1));
#endif
        }

        #endregion

        #region + ElementAt +

        public virtual IObservable<TSource> ElementAt<TSource>(IObservable<TSource> source, int index)
        {
#if !NO_PERF
            return new ElementAt<TSource>(source, index, true);
#else
            return new AnonymousObservable<TSource>(observer =>
            {
                int i = index;
                return source.Subscribe(
                    x =>
                    {
                        if (i == 0)
                        {
                            observer.OnNext(x);
                            observer.OnCompleted();
                        }

                        i--;
                    },
                    observer.OnError,
                    () => observer.OnError(new ArgumentOutOfRangeException("index"))
                );
            });
#endif
        }

        #endregion

        #region + ElementAtOrDefault +

        public virtual IObservable<TSource> ElementAtOrDefault<TSource>(IObservable<TSource> source, int index)
        {
#if !NO_PERF
            return new ElementAt<TSource>(source, index, false);
#else
            return new AnonymousObservable<TSource>(observer =>
            {
                int i = index;
                return source.Subscribe(
                    x =>
                    {
                        if (i == 0)
                        {
                            observer.OnNext(x);
                            observer.OnCompleted();
                        }

                        i--;
                    },
                    observer.OnError,
                    () =>
                    {
                        observer.OnNext(default(TSource));
                        observer.OnCompleted();
                    }
                );
            });
#endif
        }

        #endregion

        #region + FirstAsync +

        public virtual IObservable<TSource> FirstAsync<TSource>(IObservable<TSource> source)
        {
#if !NO_PERF
            return new FirstAsync<TSource>(source, null, true);
#else
            return FirstOrDefaultAsync_(source, true);
#endif
        }

        public virtual IObservable<TSource> FirstAsync<TSource>(IObservable<TSource> source, Func<TSource, bool> predicate)
        {
#if !NO_PERF
            return new FirstAsync<TSource>(source, predicate, true);
#else
            return source.Where(predicate).FirstAsync();
#endif
        }

        #endregion

        #region + FirstAsyncOrDefaultAsync +

        public virtual IObservable<TSource> FirstOrDefaultAsync<TSource>(IObservable<TSource> source)
        {
#if !NO_PERF
            return new FirstAsync<TSource>(source, null, false);
#else
            return FirstOrDefaultAsync_(source, false);
#endif
        }

        public virtual IObservable<TSource> FirstOrDefaultAsync<TSource>(IObservable<TSource> source, Func<TSource, bool> predicate)
        {
#if !NO_PERF
            return new FirstAsync<TSource>(source, predicate, false);
#else
            return source.Where(predicate).FirstOrDefaultAsync();
#endif
        }

#if NO_PERF
        private static IObservable<TSource> FirstOrDefaultAsync_<TSource>(IObservable<TSource> source, bool throwOnEmpty)
        {
            return new AnonymousObservable<TSource>(observer =>
            {
                return source.Subscribe(
                    x =>
                    {
                        observer.OnNext(x);
                        observer.OnCompleted();
                    },
                    observer.OnError,
                    () =>
                    {
                        if (throwOnEmpty)
                        {
                            observer.OnError(new InvalidOperationException(Strings_Linq.NO_ELEMENTS));
                        }
                        else
                        {
                            observer.OnNext(default(TSource));
                            observer.OnCompleted();
                        }
                    }
                );
            });
        }
#endif

        #endregion

        #region + IsEmpty +

        public virtual IObservable<bool> IsEmpty<TSource>(IObservable<TSource> source)
        {
#if !NO_PERF
            return new IsEmpty<TSource>(source);
#else
            return source.Any().Select(b => !b);
#endif
        }

        #endregion

        #region + LastAsync +

        public virtual IObservable<TSource> LastAsync<TSource>(IObservable<TSource> source)
        {
#if !NO_PERF
            return new LastAsync<TSource>(source, null, true);
#else
            return LastOrDefaultAsync_(source, true);
#endif
        }

        public virtual IObservable<TSource> LastAsync<TSource>(IObservable<TSource> source, Func<TSource, bool> predicate)
        {
#if !NO_PERF
            return new LastAsync<TSource>(source, predicate, true);
#else
            return source.Where(predicate).LastAsync();
#endif
        }

        #endregion

        #region + LastOrDefaultAsync +

        public virtual IObservable<TSource> LastOrDefaultAsync<TSource>(IObservable<TSource> source)
        {
#if !NO_PERF
            return new LastAsync<TSource>(source, null, false);
#else
            return LastOrDefaultAsync_(source, false);
#endif
        }

        public virtual IObservable<TSource> LastOrDefaultAsync<TSource>(IObservable<TSource> source, Func<TSource, bool> predicate)
        {
#if !NO_PERF
            return new LastAsync<TSource>(source, predicate, false);
#else
            return source.Where(predicate).LastOrDefaultAsync();
#endif
        }

#if NO_PERF
        private static IObservable<TSource> LastOrDefaultAsync_<TSource>(IObservable<TSource> source, bool throwOnEmpty)
        {
            return new AnonymousObservable<TSource>(observer =>
            {
                var value = default(TSource);
                var seenValue = false;

                return source.Subscribe(
                    x =>
                    {
                        value = x;
                        seenValue = true;
                    },
                    observer.OnError,
                    () =>
                    {
                        if (throwOnEmpty && !seenValue)
                        {
                            observer.OnError(new InvalidOperationException(Strings_Linq.NO_ELEMENTS));
                        }
                        else
                        {
                            observer.OnNext(value);
                            observer.OnCompleted();
                        }
                    }
                );
            });
        }
#endif

        #endregion

        #region + LongCount +

        public virtual IObservable<long> LongCount<TSource>(IObservable<TSource> source)
        {
#if !NO_PERF
            return new LongCount<TSource>(source);
#else
            return source.Aggregate(0L, (count, _) => checked(count + 1));
#endif
        }

        public virtual IObservable<long> LongCount<TSource>(IObservable<TSource> source, Func<TSource, bool> predicate)
        {
#if !NO_PERF
            return new LongCount<TSource>(source, predicate);
#else
            return source.Where(predicate).Aggregate(0L, (count, _) => checked(count + 1));
#endif
        }

        #endregion

        #region + Max +

        public virtual IObservable<TSource> Max<TSource>(IObservable<TSource> source)
        {
#if !NO_PERF
            // BREAKING CHANGE v2 > v1.x - Behavior for reference types
            return new Max<TSource>(source, Comparer<TSource>.Default);
#else
            return MaxBy(source, x => x).Select(x => x.First());
#endif
        }

        public virtual IObservable<TSource> Max<TSource>(IObservable<TSource> source, IComparer<TSource> comparer)
        {
#if !NO_PERF
            // BREAKING CHANGE v2 > v1.x - Behavior for reference types
            return new Max<TSource>(source, comparer);
#else
            return MaxBy(source, x => x, comparer).Select(x => x.First());
#endif
        }

        public virtual IObservable<double> Max(IObservable<double> source)
        {
#if !NO_PERF
            return new MaxDouble(source);
#else
            return source.Scan(double.MinValue, Math.Max).Final();
#endif
        }

        public virtual IObservable<float> Max(IObservable<float> source)
        {
#if !NO_PERF
            return new MaxSingle(source);
#else
            return source.Scan(float.MinValue, Math.Max).Final();
#endif
        }

        public virtual IObservable<decimal> Max(IObservable<decimal> source)
        {
#if !NO_PERF
            return new MaxDecimal(source);
#else
            return source.Scan(decimal.MinValue, Math.Max).Final();
#endif
        }

        public virtual IObservable<int> Max(IObservable<int> source)
        {
#if !NO_PERF
            return new MaxInt32(source);
#else
            return source.Scan(int.MinValue, Math.Max).Final();
#endif
        }

        public virtual IObservable<long> Max(IObservable<long> source)
        {
#if !NO_PERF
            return new MaxInt64(source);
#else
            return source.Scan(long.MinValue, Math.Max).Final();
#endif
        }

        public virtual IObservable<double?> Max(IObservable<double?> source)
        {
#if !NO_PERF
            return new MaxDoubleNullable(source);
#else
            return source.Aggregate(new double?(), NullableMax);
#endif
        }

        public virtual IObservable<float?> Max(IObservable<float?> source)
        {
#if !NO_PERF
            return new MaxSingleNullable(source);
#else
            return source.Aggregate(new float?(), NullableMax);
#endif
        }

        public virtual IObservable<decimal?> Max(IObservable<decimal?> source)
        {
#if !NO_PERF
            return new MaxDecimalNullable(source);
#else
            return source.Aggregate(new decimal?(), NullableMax);
#endif
        }

        public virtual IObservable<int?> Max(IObservable<int?> source)
        {
#if !NO_PERF
            return new MaxInt32Nullable(source);
#else
            return source.Aggregate(new int?(), NullableMax);
#endif
        }

        public virtual IObservable<long?> Max(IObservable<long?> source)
        {
#if !NO_PERF
            return new MaxInt64Nullable(source);
#else
            return source.Aggregate(new long?(), NullableMax);
#endif
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
#if !NO_PERF
            return new MaxBy<TSource, TKey>(source, keySelector, Comparer<TKey>.Default);
#else
            return MaxBy(source, keySelector, Comparer<TKey>.Default);
#endif
        }

        public virtual IObservable<IList<TSource>> MaxBy<TSource, TKey>(IObservable<TSource> source, Func<TSource, TKey> keySelector, IComparer<TKey> comparer)
        {
#if !NO_PERF
            return new MaxBy<TSource, TKey>(source, keySelector, comparer);
#else
            return ExtremaBy(source, keySelector, comparer);
#endif
        }

        #endregion

        #region + Min +

        public virtual IObservable<TSource> Min<TSource>(IObservable<TSource> source)
        {
#if !NO_PERF
            // BREAKING CHANGE v2 > v1.x - Behavior for reference types
            return new Min<TSource>(source, Comparer<TSource>.Default);
#else
            return MinBy(source, x => x).Select(x => x.First());
#endif
        }

        public virtual IObservable<TSource> Min<TSource>(IObservable<TSource> source, IComparer<TSource> comparer)
        {
#if !NO_PERF
            // BREAKING CHANGE v2 > v1.x - Behavior for reference types
            return new Min<TSource>(source, comparer);
#else
            return MinBy(source, x => x, comparer).Select(x => x.First());
#endif
        }

        public virtual IObservable<double> Min(IObservable<double> source)
        {
#if !NO_PERF
            return new MinDouble(source);
#else
            return source.Scan(double.MaxValue, Math.Min).Final();
#endif
        }

        public virtual IObservable<float> Min(IObservable<float> source)
        {
#if !NO_PERF
            return new MinSingle(source);
#else
            return source.Scan(float.MaxValue, Math.Min).Final();
#endif
        }

        public virtual IObservable<decimal> Min(IObservable<decimal> source)
        {
#if !NO_PERF
            return new MinDecimal(source);
#else
            return source.Scan(decimal.MaxValue, Math.Min).Final();
#endif
        }

        public virtual IObservable<int> Min(IObservable<int> source)
        {
#if !NO_PERF
            return new MinInt32(source);
#else
            return source.Scan(int.MaxValue, Math.Min).Final();
#endif
        }

        public virtual IObservable<long> Min(IObservable<long> source)
        {
#if !NO_PERF
            return new MinInt64(source);
#else
            return source.Scan(long.MaxValue, Math.Min).Final();
#endif
        }

        public virtual IObservable<double?> Min(IObservable<double?> source)
        {
#if !NO_PERF
            return new MinDoubleNullable(source);
#else
            return source.Aggregate(new double?(), NullableMin);
#endif
        }

        public virtual IObservable<float?> Min(IObservable<float?> source)
        {
#if !NO_PERF
            return new MinSingleNullable(source);
#else
            return source.Aggregate(new float?(), NullableMin);
#endif
        }

        public virtual IObservable<decimal?> Min(IObservable<decimal?> source)
        {
#if !NO_PERF
            return new MinDecimalNullable(source);
#else
            return source.Aggregate(new decimal?(), NullableMin);
#endif
        }

        public virtual IObservable<int?> Min(IObservable<int?> source)
        {
#if !NO_PERF
            return new MinInt32Nullable(source);
#else
            return source.Aggregate(new int?(), NullableMin);
#endif
        }

        public virtual IObservable<long?> Min(IObservable<long?> source)
        {
#if !NO_PERF
            return new MinInt64Nullable(source);
#else
            return source.Aggregate(new long?(), NullableMin);
#endif
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
#if !NO_PERF
            return new MinBy<TSource, TKey>(source, keySelector, Comparer<TKey>.Default);
#else
            return MinBy(source, keySelector, Comparer<TKey>.Default);
#endif
        }

        public virtual IObservable<IList<TSource>> MinBy<TSource, TKey>(IObservable<TSource> source, Func<TSource, TKey> keySelector, IComparer<TKey> comparer)
        {
#if !NO_PERF
            return new MinBy<TSource, TKey>(source, keySelector, comparer);
#else
            return ExtremaBy(source, keySelector, new AnonymousComparer<TKey>((x, y) => comparer.Compare(x, y) * -1));
#endif
        }

        #endregion

        #region + SequenceEqual +

        public virtual IObservable<bool> SequenceEqual<TSource>(IObservable<TSource> first, IObservable<TSource> second)
        {
#if !NO_PERF
            return new SequenceEqual<TSource>(first, second, EqualityComparer<TSource>.Default);
#else
            return first.SequenceEqual(second, EqualityComparer<TSource>.Default);
#endif
        }

        public virtual IObservable<bool> SequenceEqual<TSource>(IObservable<TSource> first, IObservable<TSource> second, IEqualityComparer<TSource> comparer)
        {
#if !NO_PERF
            return new SequenceEqual<TSource>(first, second, comparer);
#else
            return new AnonymousObservable<bool>(observer =>
            {
                var gate = new object();
                var donel = false;
                var doner = false;
                var ql = new Queue<TSource>();
                var qr = new Queue<TSource>();

                var subscription1 = first.Subscribe(
                    x =>
                    {
                        lock (gate)
                        {
                            if (qr.Count > 0)
                            {
                                var equal = false;
                                var v = qr.Dequeue();
                                try
                                {
                                    equal = comparer.Equals(x, v);
                                }
                                catch (Exception exception)
                                {
                                    observer.OnError(exception);
                                    return;
                                }
                                if (!equal)
                                {
                                    observer.OnNext(false);
                                    observer.OnCompleted();
                                }
                            }
                            else if (doner)
                            {
                                observer.OnNext(false);
                                observer.OnCompleted();
                            }
                            else
                                ql.Enqueue(x);
                        }
                    },
                    observer.OnError,
                    () =>
                    {
                        lock (gate)
                        {
                            donel = true;
                            if (ql.Count == 0)
                            {
                                if (qr.Count > 0)
                                {
                                    observer.OnNext(false);
                                    observer.OnCompleted();
                                }
                                else if (doner)
                                {
                                    observer.OnNext(true);
                                    observer.OnCompleted();
                                }
                            }
                        }
                    });

                var subscription2 = second.Subscribe(
                    x =>
                    {
                        lock (gate)
                        {
                            if (ql.Count > 0)
                            {
                                var equal = false;
                                var v = ql.Dequeue();
                                try
                                {
                                    equal = comparer.Equals(v, x);
                                }
                                catch (Exception exception)
                                {
                                    observer.OnError(exception);
                                    return;
                                }
                                if (!equal)
                                {
                                    observer.OnNext(false);
                                    observer.OnCompleted();
                                }
                            }
                            else if (donel)
                            {
                                observer.OnNext(false);
                                observer.OnCompleted();
                            }
                            else
                                qr.Enqueue(x);
                        }
                    },
                    observer.OnError,
                    () =>
                    {
                        lock (gate)
                        {
                            doner = true;
                            if (qr.Count == 0)
                            {
                                if (ql.Count > 0)
                                {
                                    observer.OnNext(false);
                                    observer.OnCompleted();
                                }
                                else if (donel)
                                {
                                    observer.OnNext(true);
                                    observer.OnCompleted();
                                }
                            }
                        }
                    });

                return new CompositeDisposable(subscription1, subscription2);
            });
#endif
        }

        public virtual IObservable<bool> SequenceEqual<TSource>(IObservable<TSource> first, IEnumerable<TSource> second)
        {
#if !NO_PERF
            return new SequenceEqual<TSource>(first, second, EqualityComparer<TSource>.Default);
#else
            return SequenceEqual<TSource>(first, second, EqualityComparer<TSource>.Default);
#endif
        }

        public virtual IObservable<bool> SequenceEqual<TSource>(IObservable<TSource> first, IEnumerable<TSource> second, IEqualityComparer<TSource> comparer)
        {
#if !NO_PERF
            return new SequenceEqual<TSource>(first, second, comparer);
#else
            return new AnonymousObservable<bool>(observer =>
            {
                var e = default(IEnumerator<TSource>);
                try
                {
                    e = second.GetEnumerator();
                }
                catch (Exception ex)
                {
                    observer.OnError(ex);
                    return Disposable.Empty;
                }

                return new CompositeDisposable(
                    first.Subscribe(
                        value =>
                        {
                            var equal = false;
                            try
                            {
                                var hasNext = e.MoveNext();
                                if (hasNext)
                                {
                                    var current = e.Current;
                                    equal = comparer.Equals(value, current);
                                }
                            }
                            catch (Exception ex)
                            {
                                observer.OnError(ex);
                                return;
                            }

                            if (!equal)
                            {
                                observer.OnNext(false);
                                observer.OnCompleted();
                            }
                        },
                        observer.OnError,
                        () =>
                        {
                            var hasNext = false;

                            try
                            {
                                hasNext = e.MoveNext();
                            }
                            catch (Exception exception)
                            {
                                observer.OnError(exception);
                                return;
                            }

                            observer.OnNext(!hasNext);
                            observer.OnCompleted();
                        }
                    ),
                    e
                );
            });
#endif
        }

        #endregion

        #region + SingleAsync +

        public virtual IObservable<TSource> SingleAsync<TSource>(IObservable<TSource> source)
        {
#if !NO_PERF
            return new SingleAsync<TSource>(source, null, true);
#else
            return SingleOrDefaultAsync_(source, true);
#endif
        }

        public virtual IObservable<TSource> SingleAsync<TSource>(IObservable<TSource> source, Func<TSource, bool> predicate)
        {
#if !NO_PERF
            return new SingleAsync<TSource>(source, predicate, true);
#else
            return source.Where(predicate).SingleAsync();
#endif
        }

        #endregion

        #region + SingleOrDefaultAsync +

        public virtual IObservable<TSource> SingleOrDefaultAsync<TSource>(IObservable<TSource> source)
        {
#if !NO_PERF
            return new SingleAsync<TSource>(source, null, false);
#else
            return SingleOrDefaultAsync_(source, false);
#endif
        }

        public virtual IObservable<TSource> SingleOrDefaultAsync<TSource>(IObservable<TSource> source, Func<TSource, bool> predicate)
        {
#if !NO_PERF
            return new SingleAsync<TSource>(source, predicate, false);
#else
            return source.Where(predicate).SingleOrDefaultAsync();
#endif
        }

#if NO_PERF
        private static IObservable<TSource> SingleOrDefaultAsync_<TSource>(IObservable<TSource> source, bool throwOnEmpty)
        {
            return new AnonymousObservable<TSource>(observer =>
            {
                var value = default(TSource);
                var seenValue = false;

                return source.Subscribe(
                    x =>
                    {
                        if (seenValue)
                        {
                            observer.OnError(new InvalidOperationException(Strings_Linq.MORE_THAN_ONE_ELEMENT));
                        }
                        else
                        {
                            value = x;
                            seenValue = true;
                        }
                    },
                    observer.OnError,
                    () =>
                    {
                        if (throwOnEmpty && !seenValue)
                        {
                            observer.OnError(new InvalidOperationException(Strings_Linq.NO_ELEMENTS));
                        }
                        else
                        {
                            observer.OnNext(value);
                            observer.OnCompleted();
                        }
                    }
                );
            });
        }
#endif

        #endregion

        #region + Sum +

        public virtual IObservable<double> Sum(IObservable<double> source)
        {
#if !NO_PERF
            return new SumDouble(source);
#else
            return source.Aggregate(0.0, (prev, curr) => prev + curr);
#endif
        }

        public virtual IObservable<float> Sum(IObservable<float> source)
        {
#if !NO_PERF
            return new SumSingle(source);
#else
            return source.Aggregate(0f, (prev, curr) => prev + curr);
#endif
        }

        public virtual IObservable<decimal> Sum(IObservable<decimal> source)
        {
#if !NO_PERF
            return new SumDecimal(source);
#else
            return source.Aggregate(0M, (prev, curr) => prev + curr);
#endif
        }

        public virtual IObservable<int> Sum(IObservable<int> source)
        {
#if !NO_PERF
            return new SumInt32(source);
#else
            return source.Aggregate(0, (prev, curr) => checked(prev + curr));
#endif
        }

        public virtual IObservable<long> Sum(IObservable<long> source)
        {
#if !NO_PERF
            return new SumInt64(source);
#else
            return source.Aggregate(0L, (prev, curr) => checked(prev + curr));
#endif
        }

        public virtual IObservable<double?> Sum(IObservable<double?> source)
        {
#if !NO_PERF
            return new SumDoubleNullable(source);
#else
            return source.Aggregate(0.0, (prev, curr) => prev + curr.GetValueOrDefault()).Select(x => (double?)x);
#endif
        }

        public virtual IObservable<float?> Sum(IObservable<float?> source)
        {
#if !NO_PERF
            return new SumSingleNullable(source);
#else
            return source.Aggregate(0f, (prev, curr) => prev + curr.GetValueOrDefault()).Select(x => (float?)x);
#endif
        }

        public virtual IObservable<decimal?> Sum(IObservable<decimal?> source)
        {
#if !NO_PERF
            return new SumDecimalNullable(source);
#else
            return source.Aggregate(0M, (prev, curr) => prev + curr.GetValueOrDefault()).Select(x => (decimal?)x);
#endif
        }

        public virtual IObservable<int?> Sum(IObservable<int?> source)
        {
#if !NO_PERF
            return new SumInt32Nullable(source);
#else
            return source.Aggregate(0, (prev, curr) => checked(prev + curr.GetValueOrDefault())).Select(x => (int?)x);
#endif
        }

        public virtual IObservable<long?> Sum(IObservable<long?> source)
        {
#if !NO_PERF
            return new SumInt64Nullable(source);
#else
            return source.Aggregate(0L, (prev, curr) => checked(prev + curr.GetValueOrDefault())).Select(x => (long?)x);
#endif
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
#if !NO_PERF
            return new ToArray<TSource>(source);
#else
            return source.ToList().Select(xs => xs.ToArray());
#endif
        }

        #endregion

        #region + ToDictionary +

        public virtual IObservable<IDictionary<TKey, TElement>> ToDictionary<TSource, TKey, TElement>(IObservable<TSource> source, Func<TSource, TKey> keySelector, Func<TSource, TElement> elementSelector, IEqualityComparer<TKey> comparer)
        {
#if !NO_PERF
            return new ToDictionary<TSource, TKey, TElement>(source, keySelector, elementSelector, comparer);
#else
            return source.Aggregate((IDictionary<TKey, TElement>)new Dictionary<TKey, TElement>(comparer), (dict, x) =>
                {
                    dict.Add(keySelector(x), elementSelector(x));
                    return dict;
                });
#endif
        }

        public virtual IObservable<IDictionary<TKey, TElement>> ToDictionary<TSource, TKey, TElement>(IObservable<TSource> source, Func<TSource, TKey> keySelector, Func<TSource, TElement> elementSelector)
        {
#if !NO_PERF
            return new ToDictionary<TSource, TKey, TElement>(source, keySelector, elementSelector, EqualityComparer<TKey>.Default);
#else
            return source.ToDictionary(keySelector, elementSelector, EqualityComparer<TKey>.Default);
#endif
        }

        public virtual IObservable<IDictionary<TKey, TSource>> ToDictionary<TSource, TKey>(IObservable<TSource> source, Func<TSource, TKey> keySelector, IEqualityComparer<TKey> comparer)
        {
#if !NO_PERF
            return new ToDictionary<TSource, TKey, TSource>(source, keySelector, x => x, comparer);
#else
            return source.ToDictionary(keySelector, x => x, comparer);
#endif
        }

        public virtual IObservable<IDictionary<TKey, TSource>> ToDictionary<TSource, TKey>(IObservable<TSource> source, Func<TSource, TKey> keySelector)
        {
#if !NO_PERF
            return new ToDictionary<TSource, TKey, TSource>(source, keySelector, x => x, EqualityComparer<TKey>.Default);
#else
            return source.ToDictionary(keySelector, x => x, EqualityComparer<TKey>.Default);
#endif
        }

        #endregion

        #region + ToList +

        public virtual IObservable<IList<TSource>> ToList<TSource>(IObservable<TSource> source)
        {
#if !NO_PERF
            return new ToList<TSource>(source);
#else
            return source.Aggregate((IList<TSource>)new List<TSource>(), (list, x) =>
            {
                list.Add(x);
                return list;
            });
#endif
        }

        #endregion

        #region + ToLookup +

        public virtual IObservable<ILookup<TKey, TElement>> ToLookup<TSource, TKey, TElement>(IObservable<TSource> source, Func<TSource, TKey> keySelector, Func<TSource, TElement> elementSelector, IEqualityComparer<TKey> comparer)
        {
#if !NO_PERF
            return new ToLookup<TSource, TKey, TElement>(source, keySelector, elementSelector, comparer);
#else
            return source.Aggregate(new Lookup<TKey, TElement>(comparer), (lookup, x) =>
            {
                lookup.Add(keySelector(x), elementSelector(x));
                return lookup;
            }).Select(xs => (ILookup<TKey, TElement>)xs);
#endif
        }

        public virtual IObservable<ILookup<TKey, TSource>> ToLookup<TSource, TKey>(IObservable<TSource> source, Func<TSource, TKey> keySelector, IEqualityComparer<TKey> comparer)
        {
#if !NO_PERF
            return new ToLookup<TSource, TKey, TSource>(source, keySelector, x => x, comparer);
#else
            return source.ToLookup(keySelector, x => x, comparer);
#endif
        }

        public virtual IObservable<ILookup<TKey, TElement>> ToLookup<TSource, TKey, TElement>(IObservable<TSource> source, Func<TSource, TKey> keySelector, Func<TSource, TElement> elementSelector)
        {
#if !NO_PERF
            return new ToLookup<TSource, TKey, TElement>(source, keySelector, elementSelector, EqualityComparer<TKey>.Default);
#else
            return source.ToLookup(keySelector, elementSelector, EqualityComparer<TKey>.Default);
#endif
        }

        public virtual IObservable<ILookup<TKey, TSource>> ToLookup<TSource, TKey>(IObservable<TSource> source, Func<TSource, TKey> keySelector)
        {
#if !NO_PERF
            return new ToLookup<TSource, TKey, TSource>(source, keySelector, x => x, EqualityComparer<TKey>.Default);
#else
            return source.ToLookup(keySelector, x => x, EqualityComparer<TKey>.Default);
#endif
        }

        #endregion

        #region |> Helpers <|

#if NO_PERF
        private static T? NullableMin<T>(T? x, T? y)
            where T : struct, IComparable<T>
        {
            if (!x.HasValue)
                return y;
            if (!y.HasValue)
                return x;
            if (x.Value.CompareTo(y.Value) <= 0)
                return x;
            return y;
        }

        private static T? NullableMax<T>(T? x, T? y)
            where T : struct, IComparable<T>
        {
            if (!x.HasValue)
                return y;
            if (!y.HasValue)
                return x;
            if (x.Value.CompareTo(y.Value) >= 0)
                return x;
            return y;
        }

        private static IObservable<IList<TSource>> ExtremaBy<TSource, TKey>(IObservable<TSource> source, Func<TSource, TKey> keySelector, IComparer<TKey> comparer)
        {
            return new AnonymousObservable<IList<TSource>>(observer =>
            {
                var hasValue = false;
                var lastKey = default(TKey);
                var list = new List<TSource>();
                return source.Subscribe(
                    x =>
                    {
                        var key = default(TKey);
                        try
                        {
                            key = keySelector(x);
                        }
                        catch (Exception ex)
                        {
                            observer.OnError(ex);
                            return;
                        }
                        var comparison = 0;

                        if (!hasValue)
                        {
                            hasValue = true;
                            lastKey = key;
                        }
                        else
                        {
                            try
                            {
                                comparison = comparer.Compare(key, lastKey);
                            }
                            catch (Exception ex)
                            {
                                observer.OnError(ex);
                                return;
                            }
                        }
                        if (comparison > 0)
                        {
                            lastKey = key;
                            list.Clear();
                        }
                        if (comparison >= 0)
                        {
                            list.Add(x);
                        }
                    },
                    observer.OnError,
                    () =>
                    {
                        observer.OnNext(list);
                        observer.OnCompleted();
                    }
                );
            });
        }
#endif

        #endregion
    }

    #region |> Helper types <|

#if NO_PERF
    static class AggregateExtensions
    {
        public static IObservable<TSource> Final<TSource>(this IObservable<TSource> source)
        {
            return new AnonymousObservable<TSource>(observer =>
            {
                var value = default(TSource);
                var hasValue = false;

                return source.Subscribe(
                    x =>
                    {
                        hasValue = true;
                        value = x;
                    },
                    observer.OnError,
                    () =>
                    {
                        if (!hasValue)
                            observer.OnError(new InvalidOperationException(Strings_Linq.NO_ELEMENTS));
                        else
                        {
                            observer.OnNext(value);
                            observer.OnCompleted();
                        }
                    });
            });
        }
    }

    sealed class AnonymousComparer<T> : IComparer<T>
    {
        private readonly Func<T, T, int> comparer;

        /// <summary>
        /// Creates an instance of IComparer by providing a method that compares two objects.
        /// </summary>
        public AnonymousComparer(Func<T, T, int> comparer)
        {
            this.comparer = comparer;
        }

        /// <summary>
        /// Compares two objects and returns a value indicating whether one is less than, equal to, or greater than the other.
        /// </summary>
        public int Compare(T x, T y)
        {
            return comparer(x, y);
        }
    }
#endif

    #endregion
}

// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the Apache 2.0 License.
// See the LICENSE file in the project root for more information. 
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Threading;

namespace System.Linq
{
    public static partial class AsyncEnumerable
    {
        public static Task<TResult> Aggregate<TSource, TAccumulate, TResult>(this IAsyncEnumerable<TSource> source, TAccumulate seed, Func<TAccumulate, TSource, TAccumulate> accumulator, Func<TAccumulate, TResult> resultSelector, CancellationToken cancellationToken)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));
            if (accumulator == null)
                throw new ArgumentNullException(nameof(accumulator));
            if (resultSelector == null)
                throw new ArgumentNullException(nameof(resultSelector));

            return Aggregate_(source, seed, accumulator, resultSelector, cancellationToken);
        }

        private static async Task<TResult> Aggregate_<TSource, TAccumulate, TResult>(IAsyncEnumerable<TSource> source, TAccumulate seed,
            Func<TAccumulate, TSource, TAccumulate> accumulator, Func<TAccumulate, TResult> resultSelector, CancellationToken cancellationToken)
        {
            var acc = seed;

            using (var e = source.GetEnumerator())
            {
                while (await e.MoveNext(cancellationToken).ConfigureAwait(false))
                {
                    acc = accumulator(acc, e.Current);
                }
            }
            var result = resultSelector(acc);
            return result;
        }

        public static Task<TAccumulate> Aggregate<TSource, TAccumulate>(this IAsyncEnumerable<TSource> source, TAccumulate seed, Func<TAccumulate, TSource, TAccumulate> accumulator, CancellationToken cancellationToken)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));
            if (accumulator == null)
                throw new ArgumentNullException(nameof(accumulator));

            return source.Aggregate(seed, accumulator, x => x, cancellationToken);
        }

        public static Task<TSource> Aggregate<TSource>(this IAsyncEnumerable<TSource> source, Func<TSource, TSource, TSource> accumulator, CancellationToken cancellationToken)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));
            if (accumulator == null)
                throw new ArgumentNullException(nameof(accumulator));

            return Aggregate_(source, accumulator, cancellationToken);
        }

        private static async Task<TSource> Aggregate_<TSource>(IAsyncEnumerable<TSource> source, Func<TSource, TSource, TSource> accumulator, CancellationToken cancellationToken)
        {
            var first = true;
            var acc = default(TSource);

            using (var e = source.GetEnumerator())
            {
                while (await e.MoveNext(cancellationToken).ConfigureAwait(false))
                {
                    acc = first ? e.Current : accumulator(acc, e.Current);
                    first = false;
                }
            }
            if (first)
                throw new InvalidOperationException(Strings.NO_ELEMENTS);
            return acc;
        }

        public static Task<int> Count<TSource>(this IAsyncEnumerable<TSource> source, CancellationToken cancellationToken)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));

            return source.Aggregate(0, (c, _) => checked(c + 1), cancellationToken);
        }

        public static Task<int> Count<TSource>(this IAsyncEnumerable<TSource> source, Func<TSource, bool> predicate, CancellationToken cancellationToken)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));
            if (predicate == null)
                throw new ArgumentNullException(nameof(predicate));

            return source.Where(predicate).Aggregate(0, (c, _) => checked(c + 1), cancellationToken);
        }

        public static Task<long> LongCount<TSource>(this IAsyncEnumerable<TSource> source, CancellationToken cancellationToken)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));

            return source.Aggregate(0L, (c, _) => checked(c + 1), cancellationToken);
        }

        public static Task<long> LongCount<TSource>(this IAsyncEnumerable<TSource> source, Func<TSource, bool> predicate, CancellationToken cancellationToken)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));
            if (predicate == null)
                throw new ArgumentNullException(nameof(predicate));

            return source.Where(predicate).Aggregate(0L, (c, _) => checked(c + 1), cancellationToken);
        }

        public static Task<bool> All<TSource>(this IAsyncEnumerable<TSource> source, Func<TSource, bool> predicate, CancellationToken cancellationToken)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));
            if (predicate == null)
                throw new ArgumentNullException(nameof(predicate));

            return All_(source, predicate, cancellationToken);
        }

        private static async Task<bool> All_<TSource>(IAsyncEnumerable<TSource> source, Func<TSource, bool> predicate, CancellationToken cancellationToken)
        {
            using (var e = source.GetEnumerator())
            {
                while (await e.MoveNext(cancellationToken).ConfigureAwait(false))
                {
                    if (!predicate(e.Current))
                        return false;
                }
            }
            return true;
        }

        public static Task<bool> Any<TSource>(this IAsyncEnumerable<TSource> source, Func<TSource, bool> predicate, CancellationToken cancellationToken)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));
            if (predicate == null)
                throw new ArgumentNullException(nameof(predicate));

            return Any_(source, predicate, cancellationToken);
        }

        private static async Task<bool> Any_<TSource>(IAsyncEnumerable<TSource> source, Func<TSource, bool> predicate, CancellationToken cancellationToken)
        {
            using (var e = source.GetEnumerator())
            {
                while (await e.MoveNext(cancellationToken).ConfigureAwait(false))
                {
                    if (predicate(e.Current))
                        return true;
                }
            }
            return false;
        }

        public static Task<bool> Any<TSource>(this IAsyncEnumerable<TSource> source, CancellationToken cancellationToken)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));

            var e = source.GetEnumerator();
            return e.MoveNext(cancellationToken);
        }

        public static Task<bool> Contains<TSource>(this IAsyncEnumerable<TSource> source, TSource value, IEqualityComparer<TSource> comparer, CancellationToken cancellationToken)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));
            if (comparer == null)
                throw new ArgumentNullException(nameof(comparer));

            return source.Any(x => comparer.Equals(x, value), cancellationToken);
        }

        public static Task<bool> Contains<TSource>(this IAsyncEnumerable<TSource> source, TSource value, CancellationToken cancellationToken)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));

            return source.Contains(value, EqualityComparer<TSource>.Default, cancellationToken);
        }

        public static Task<TSource> First<TSource>(this IAsyncEnumerable<TSource> source, CancellationToken cancellationToken)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));

            return First_(source, cancellationToken);
        }

        private static async Task<TSource> First_<TSource>(IAsyncEnumerable<TSource> source, CancellationToken cancellationToken)
        {
            using (var e = source.GetEnumerator())
            {
                if (await e.MoveNext(cancellationToken).ConfigureAwait(false))
                {
                    return e.Current;
                }
            }
            throw new InvalidOperationException(Strings.NO_ELEMENTS);
        }

        public static Task<TSource> First<TSource>(this IAsyncEnumerable<TSource> source, Func<TSource, bool> predicate, CancellationToken cancellationToken)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));
            if (predicate == null)
                throw new ArgumentNullException(nameof(predicate));

            return source.Where(predicate).First(cancellationToken);
        }

        public static Task<TSource> FirstOrDefault<TSource>(this IAsyncEnumerable<TSource> source, CancellationToken cancellationToken)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));

            return FirstOrDefault_(source, cancellationToken);
        }

        private static async Task<TSource> FirstOrDefault_<TSource>(IAsyncEnumerable<TSource> source, CancellationToken cancellationToken)
        {
            using (var e = source.GetEnumerator())
            {
                if (await e.MoveNext(cancellationToken).ConfigureAwait(false))
                {
                    return e.Current;
                }
            }
            return default(TSource);
        }

        public static Task<TSource> FirstOrDefault<TSource>(this IAsyncEnumerable<TSource> source, Func<TSource, bool> predicate, CancellationToken cancellationToken)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));
            if (predicate == null)
                throw new ArgumentNullException(nameof(predicate));

            return source.Where(predicate).FirstOrDefault(cancellationToken);
        }

        public static Task<TSource> Last<TSource>(this IAsyncEnumerable<TSource> source, CancellationToken cancellationToken)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));

            return Last_(source, cancellationToken);
        }

        private static async Task<TSource> Last_<TSource>(IAsyncEnumerable<TSource> source, CancellationToken cancellationToken)
        {
            var last = default(TSource);
            var hasLast = false;

            using (var e = source.GetEnumerator())
            {
                while (await e.MoveNext(cancellationToken).ConfigureAwait(false))
                {
                    hasLast = true;
                    last = e.Current;
                }
            }
            if (!hasLast)
                throw new InvalidOperationException(Strings.NO_ELEMENTS);
            return last;
        }

        public static Task<TSource> Last<TSource>(this IAsyncEnumerable<TSource> source, Func<TSource, bool> predicate, CancellationToken cancellationToken)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));
            if (predicate == null)
                throw new ArgumentNullException(nameof(predicate));

            return source.Where(predicate).Last(cancellationToken);
        }

        public static Task<TSource> LastOrDefault<TSource>(this IAsyncEnumerable<TSource> source, CancellationToken cancellationToken)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));

            return LastOrDefault_(source, cancellationToken);
        }

        private static async Task<TSource> LastOrDefault_<TSource>(IAsyncEnumerable<TSource> source, CancellationToken cancellationToken)
        {
            var last = default(TSource);
            var hasLast = false;

            using (var e = source.GetEnumerator())
            {
                while (await e.MoveNext(cancellationToken).ConfigureAwait(false))
                {
                    hasLast = true;
                    last = e.Current;
                }
            }
            return !hasLast ? default(TSource) : last;
        }

        public static Task<TSource> LastOrDefault<TSource>(this IAsyncEnumerable<TSource> source, Func<TSource, bool> predicate, CancellationToken cancellationToken)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));
            if (predicate == null)
                throw new ArgumentNullException(nameof(predicate));

            return source.Where(predicate).LastOrDefault(cancellationToken);
        }

        public static Task<TSource> Single<TSource>(this IAsyncEnumerable<TSource> source, CancellationToken cancellationToken)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));

            return Single_(source, cancellationToken);
        }

        private static async Task<TSource> Single_<TSource>(IAsyncEnumerable<TSource> source, CancellationToken cancellationToken)
        {
            using (var e = source.GetEnumerator())
            {
                if (!await e.MoveNext(cancellationToken).ConfigureAwait(false))
                {
                    throw new InvalidOperationException(Strings.NO_ELEMENTS);
                }
                var result = e.Current;
                if (await e.MoveNext(cancellationToken).ConfigureAwait(false))
                {
                    throw new InvalidOperationException(Strings.MORE_THAN_ONE_ELEMENT);
                }
                return result;
            }
        }

        public static Task<TSource> Single<TSource>(this IAsyncEnumerable<TSource> source, Func<TSource, bool> predicate, CancellationToken cancellationToken)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));
            if (predicate == null)
                throw new ArgumentNullException(nameof(predicate));

            return source.Where(predicate).Single(cancellationToken);
        }

        public static Task<TSource> SingleOrDefault<TSource>(this IAsyncEnumerable<TSource> source, CancellationToken cancellationToken)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));

            return SingleOrDefault_(source, cancellationToken);
        }

        private static async Task<TSource> SingleOrDefault_<TSource>(IAsyncEnumerable<TSource> source, CancellationToken cancellationToken)
        {
            using (var e = source.GetEnumerator())
            {
                if (!await e.MoveNext(cancellationToken).ConfigureAwait(false))
                {
                    return default(TSource);
                }

                TSource result = e.Current;
                if (!await e.MoveNext(cancellationToken).ConfigureAwait(false))
                {
                    return result;
                }
            }
            throw new InvalidOperationException(Strings.MORE_THAN_ONE_ELEMENT);
        }

        public static Task<TSource> SingleOrDefault<TSource>(this IAsyncEnumerable<TSource> source, Func<TSource, bool> predicate, CancellationToken cancellationToken)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));
            if (predicate == null)
                throw new ArgumentNullException(nameof(predicate));

            return source.Where(predicate).SingleOrDefault(cancellationToken);
        }

        public static Task<TSource> ElementAt<TSource>(this IAsyncEnumerable<TSource> source, int index, CancellationToken cancellationToken)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));
            if (index < 0)
                throw new ArgumentOutOfRangeException(nameof(index));

            return ElementAt_(source, index, cancellationToken);
        }

        private static async Task<TSource> ElementAt_<TSource>(IAsyncEnumerable<TSource> source, int index, CancellationToken cancellationToken)
        {
            if (index >= 0)
            {
                using (var e = source.GetEnumerator())
                {
                    while (await e.MoveNext(cancellationToken).ConfigureAwait(false))
                    {
                        if (index == 0)
                        {
                            return e.Current;
                        }

                        index--;
                    }
                }
            }

            throw new ArgumentOutOfRangeException(nameof(index));
        }

        public static Task<TSource> ElementAtOrDefault<TSource>(this IAsyncEnumerable<TSource> source, int index, CancellationToken cancellationToken)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));
            if (index < 0)
                throw new ArgumentOutOfRangeException(nameof(index));

            return ElementAtOrDefault_(source, index, cancellationToken);
        }

        private static async Task<TSource> ElementAtOrDefault_<TSource>(IAsyncEnumerable<TSource> source, int index, CancellationToken cancellationToken)
        {
            if (index >= 0)
            {
                using (var e = source.GetEnumerator())
                {
                    while (await e.MoveNext(cancellationToken).ConfigureAwait(false))
                    {
                        if (index == 0)
                        {
                            return e.Current;
                        }

                        index--;
                    }
                }
            }

            return default(TSource);
        }

        public static Task<TSource[]> ToArray<TSource>(this IAsyncEnumerable<TSource> source, CancellationToken cancellationToken)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));

            return source.Aggregate(new List<TSource>(), (list, x) => { list.Add(x); return list; }, list => list.ToArray(), cancellationToken);
        }

        public static Task<List<TSource>> ToList<TSource>(this IAsyncEnumerable<TSource> source, CancellationToken cancellationToken)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));

            return source.Aggregate(new List<TSource>(), (list, x) => { list.Add(x); return list; }, cancellationToken);
        }

        public static Task<Dictionary<TKey, TElement>> ToDictionary<TSource, TKey, TElement>(this IAsyncEnumerable<TSource> source, Func<TSource, TKey> keySelector, Func<TSource, TElement> elementSelector, IEqualityComparer<TKey> comparer, CancellationToken cancellationToken)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));
            if (keySelector == null)
                throw new ArgumentNullException(nameof(keySelector));
            if (elementSelector == null)
                throw new ArgumentNullException(nameof(elementSelector));
            if (comparer == null)
                throw new ArgumentNullException(nameof(comparer));

            return source.Aggregate(new Dictionary<TKey, TElement>(comparer), (d, x) => { d.Add(keySelector(x), elementSelector(x)); return d; }, cancellationToken);
        }

        public static Task<Dictionary<TKey, TElement>> ToDictionary<TSource, TKey, TElement>(this IAsyncEnumerable<TSource> source, Func<TSource, TKey> keySelector, Func<TSource, TElement> elementSelector, CancellationToken cancellationToken)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));
            if (keySelector == null)
                throw new ArgumentNullException(nameof(keySelector));
            if (elementSelector == null)
                throw new ArgumentNullException(nameof(elementSelector));

            return source.ToDictionary(keySelector, elementSelector, EqualityComparer<TKey>.Default, cancellationToken);
        }

        public static Task<Dictionary<TKey, TSource>> ToDictionary<TSource, TKey>(this IAsyncEnumerable<TSource> source, Func<TSource, TKey> keySelector, IEqualityComparer<TKey> comparer, CancellationToken cancellationToken)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));
            if (keySelector == null)
                throw new ArgumentNullException(nameof(keySelector));
            if (comparer == null)
                throw new ArgumentNullException(nameof(comparer));

            return source.ToDictionary(keySelector, x => x, comparer, cancellationToken);
        }

        public static Task<Dictionary<TKey, TSource>> ToDictionary<TSource, TKey>(this IAsyncEnumerable<TSource> source, Func<TSource, TKey> keySelector, CancellationToken cancellationToken)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));
            if (keySelector == null)
                throw new ArgumentNullException(nameof(keySelector));

            return source.ToDictionary(keySelector, x => x, EqualityComparer<TKey>.Default, cancellationToken);
        }

        public static async Task<ILookup<TKey, TElement>> ToLookup<TSource, TKey, TElement>(this IAsyncEnumerable<TSource> source, Func<TSource, TKey> keySelector, Func<TSource, TElement> elementSelector, IEqualityComparer<TKey> comparer, CancellationToken cancellationToken)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));
            if (keySelector == null)
                throw new ArgumentNullException(nameof(keySelector));
            if (elementSelector == null)
                throw new ArgumentNullException(nameof(elementSelector));
            if (comparer == null)
                throw new ArgumentNullException(nameof(comparer));

            var lookup = await Internal.Lookup<TKey, TElement>.CreateAsync(source, keySelector, elementSelector, comparer, cancellationToken).ConfigureAwait(false);

            return lookup;
        }

        public static Task<ILookup<TKey, TElement>> ToLookup<TSource, TKey, TElement>(this IAsyncEnumerable<TSource> source, Func<TSource, TKey> keySelector, Func<TSource, TElement> elementSelector, CancellationToken cancellationToken)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));
            if (keySelector == null)
                throw new ArgumentNullException(nameof(keySelector));
            if (elementSelector == null)
                throw new ArgumentNullException(nameof(elementSelector));

            return source.ToLookup(keySelector, elementSelector, EqualityComparer<TKey>.Default, cancellationToken);
        }

        public static Task<ILookup<TKey, TSource>> ToLookup<TSource, TKey>(this IAsyncEnumerable<TSource> source, Func<TSource, TKey> keySelector, IEqualityComparer<TKey> comparer, CancellationToken cancellationToken)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));
            if (keySelector == null)
                throw new ArgumentNullException(nameof(keySelector));
            if (comparer == null)
                throw new ArgumentNullException(nameof(comparer));

            return source.ToLookup(keySelector, x => x, comparer, cancellationToken);
        }

        public static Task<ILookup<TKey, TSource>> ToLookup<TSource, TKey>(this IAsyncEnumerable<TSource> source, Func<TSource, TKey> keySelector, CancellationToken cancellationToken)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));
            if (keySelector == null)
                throw new ArgumentNullException(nameof(keySelector));

            return source.ToLookup(keySelector, x => x, EqualityComparer<TKey>.Default, cancellationToken);
        }

        public static Task<double> Average(this IAsyncEnumerable<int> source, CancellationToken cancellationToken)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));

            return Average_(source, cancellationToken);
        }

        private static async Task<double> Average_(this IAsyncEnumerable<int> source, CancellationToken cancellationToken)
        {
            using (var e = source.GetEnumerator())
            {
                if (!await e.MoveNext(cancellationToken).ConfigureAwait(false))
                {
                    throw new InvalidOperationException(Strings.NO_ELEMENTS);
                }

                long sum = e.Current;
                long count = 1;
                checked
                {
                    while (await e.MoveNext(cancellationToken).ConfigureAwait(false))
                    {
                        sum += e.Current;
                        ++count;
                    }
                }

                return (double)sum / count;
            }
        }

        public static Task<double?> Average(this IAsyncEnumerable<int?> source, CancellationToken cancellationToken)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));

            return Average_(source, cancellationToken);
        }

        private static async Task<double?> Average_(IAsyncEnumerable<int?> source, CancellationToken cancellationToken)
        {
            using (var e = source.GetEnumerator())
            {
                while (await e.MoveNext(cancellationToken).ConfigureAwait(false))
                {
                    int? v = e.Current;
                    if (v.HasValue)
                    {
                        long sum = v.GetValueOrDefault();
                        long count = 1;
                        checked
                        {
                            while (await e.MoveNext(cancellationToken).ConfigureAwait(false))
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

        public static Task<double> Average(this IAsyncEnumerable<long> source, CancellationToken cancellationToken)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));

            return Average_(source, cancellationToken);
        }

        private static async Task<double> Average_(IAsyncEnumerable<long> source, CancellationToken cancellationToken)
        {
            using (var e = source.GetEnumerator())
            {
                if (!await e.MoveNext(cancellationToken).ConfigureAwait(false))
                {
                    throw new InvalidOperationException(Strings.NO_ELEMENTS);
                }

                long sum = e.Current;
                long count = 1;
                checked
                {
                    while (await e.MoveNext(cancellationToken).ConfigureAwait(false))
                    {
                        sum += e.Current;
                        ++count;
                    }
                }

                return (double)sum / count;
            }
        }

        public static Task<double?> Average(this IAsyncEnumerable<long?> source, CancellationToken cancellationToken)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));

            return Average_(source, cancellationToken);
        }

        private static async Task<double?> Average_(IAsyncEnumerable<long?> source, CancellationToken cancellationToken)
        {
            using (var e = source.GetEnumerator())
            {
                while (await e.MoveNext(cancellationToken).ConfigureAwait(false))
                {
                    long? v = e.Current;
                    if (v.HasValue)
                    {
                        long sum = v.GetValueOrDefault();
                        long count = 1;
                        checked
                        {
                            while (await e.MoveNext(cancellationToken).ConfigureAwait(false))
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

        public static Task<double> Average(this IAsyncEnumerable<double> source, CancellationToken cancellationToken)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));

            return Average_(source, cancellationToken);
        }

        private static async Task<double> Average_(IAsyncEnumerable<double> source, CancellationToken cancellationToken)
        {
            using (var e = source.GetEnumerator())
            {
                if (!await e.MoveNext(cancellationToken).ConfigureAwait(false))
                {
                    throw new InvalidOperationException(Strings.NO_ELEMENTS);
                }

                double sum = e.Current;
                long count = 1;
                while (await e.MoveNext(cancellationToken).ConfigureAwait(false))
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

        public static Task<double?> Average(this IAsyncEnumerable<double?> source, CancellationToken cancellationToken)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));

            return Average_(source, cancellationToken);
        }

        private static async Task<double?> Average_(IAsyncEnumerable<double?> source, CancellationToken cancellationToken)
        {
            using (var e = source.GetEnumerator())
            {
                while (await e.MoveNext(cancellationToken).ConfigureAwait(false))
                {
                    double? v = e.Current;
                    if (v.HasValue)
                    {
                        double sum = v.GetValueOrDefault();
                        long count = 1;
                        checked
                        {
                            while (await e.MoveNext(cancellationToken).ConfigureAwait(false))
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

        public static Task<float> Average(this IAsyncEnumerable<float> source, CancellationToken cancellationToken)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));

            return Average_(source, cancellationToken);
        }

        private static async Task<float> Average_(IAsyncEnumerable<float> source, CancellationToken cancellationToken)
        {
            using (var e = source.GetEnumerator())
            {
                if (!await e.MoveNext(cancellationToken).ConfigureAwait(false))
                {
                    throw new InvalidOperationException(Strings.NO_ELEMENTS);
                }

                double sum = e.Current;
                long count = 1;
                while (await e.MoveNext(cancellationToken).ConfigureAwait(false))
                {
                    sum += e.Current;
                    ++count;
                }

                return (float)(sum / count);
            }
        }

        public static Task<float?> Average(this IAsyncEnumerable<float?> source, CancellationToken cancellationToken)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));

            return Average_(source, cancellationToken);
        }

        private static async Task<float?> Average_(IAsyncEnumerable<float?> source, CancellationToken cancellationToken)
        {
            using (var e = source.GetEnumerator())
            {
                while (await e.MoveNext(cancellationToken).ConfigureAwait(false))
                {
                    float? v = e.Current;
                    if (v.HasValue)
                    {
                        double sum = v.GetValueOrDefault();
                        long count = 1;
                        checked
                        {
                            while (await e.MoveNext(cancellationToken).ConfigureAwait(false))
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

        public static Task<decimal> Average(this IAsyncEnumerable<decimal> source, CancellationToken cancellationToken)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));

            return Average_(source, cancellationToken);
        }

        private static async Task<decimal> Average_(IAsyncEnumerable<decimal> source, CancellationToken cancellationToken)
        {
            using (var e = source.GetEnumerator())
            {
                if (!await e.MoveNext(cancellationToken).ConfigureAwait(false))
                {
                    throw new InvalidOperationException(Strings.NO_ELEMENTS);
                }

                decimal sum = e.Current;
                long count = 1;
                while (await e.MoveNext(cancellationToken).ConfigureAwait(false))
                {
                    sum += e.Current;
                    ++count;
                }

                return sum / count;
            }
        }

        public static Task<decimal?> Average(this IAsyncEnumerable<decimal?> source, CancellationToken cancellationToken)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));

            return Average_(source, cancellationToken);
        }

        private static async Task<decimal?> Average_(IAsyncEnumerable<decimal?> source, CancellationToken cancellationToken)
        {
            using (var e = source.GetEnumerator())
            {
                while (await e.MoveNext(cancellationToken).ConfigureAwait(false))
                {
                    decimal? v = e.Current;
                    if (v.HasValue)
                    {
                        decimal sum = v.GetValueOrDefault();
                        long count = 1;
                        while (await e.MoveNext(cancellationToken).ConfigureAwait(false))
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

        public static Task<double?> Average<TSource>(this IAsyncEnumerable<TSource> source, Func<TSource, int?> selector, CancellationToken cancellationToken)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));
            if (selector == null)
                throw new ArgumentNullException(nameof(selector));

            return source.Select(selector).Average(cancellationToken);
        }

        public static Task<double> Average<TSource>(this IAsyncEnumerable<TSource> source, Func<TSource, int> selector, CancellationToken cancellationToken)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));
            if (selector == null)
                throw new ArgumentNullException(nameof(selector));

            return source.Select(selector).Average(cancellationToken);
        }

        public static Task<double> Average<TSource>(this IAsyncEnumerable<TSource> source, Func<TSource, long> selector, CancellationToken cancellationToken)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));
            if (selector == null)
                throw new ArgumentNullException(nameof(selector));

            return source.Select(selector).Average(cancellationToken);
        }

        public static Task<double?> Average<TSource>(this IAsyncEnumerable<TSource> source, Func<TSource, long?> selector, CancellationToken cancellationToken)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));
            if (selector == null)
                throw new ArgumentNullException(nameof(selector));

            return source.Select(selector).Average(cancellationToken);
        }

        public static Task<double> Average<TSource>(this IAsyncEnumerable<TSource> source, Func<TSource, double> selector, CancellationToken cancellationToken)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));
            if (selector == null)
                throw new ArgumentNullException(nameof(selector));

            return source.Select(selector).Average(cancellationToken);
        }

        public static Task<double?> Average<TSource>(this IAsyncEnumerable<TSource> source, Func<TSource, double?> selector, CancellationToken cancellationToken)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));
            if (selector == null)
                throw new ArgumentNullException(nameof(selector));

            return source.Select(selector).Average(cancellationToken);
        }

        public static Task<float> Average<TSource>(this IAsyncEnumerable<TSource> source, Func<TSource, float> selector, CancellationToken cancellationToken)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));
            if (selector == null)
                throw new ArgumentNullException(nameof(selector));

            return source.Select(selector).Average(cancellationToken);
        }

        public static Task<float?> Average<TSource>(this IAsyncEnumerable<TSource> source, Func<TSource, float?> selector, CancellationToken cancellationToken)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));
            if (selector == null)
                throw new ArgumentNullException(nameof(selector));

            return source.Select(selector).Average(cancellationToken);
        }

        public static Task<decimal> Average<TSource>(this IAsyncEnumerable<TSource> source, Func<TSource, decimal> selector, CancellationToken cancellationToken)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));
            if (selector == null)
                throw new ArgumentNullException(nameof(selector));

            return source.Select(selector).Average(cancellationToken);
        }

        public static Task<decimal?> Average<TSource>(this IAsyncEnumerable<TSource> source, Func<TSource, decimal?> selector, CancellationToken cancellationToken)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));
            if (selector == null)
                throw new ArgumentNullException(nameof(selector));

            return source.Select(selector).Average(cancellationToken);
        }

        public static Task<int> Max(this IAsyncEnumerable<int> source, CancellationToken cancellationToken)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));

            return source.Aggregate(Math.Max, cancellationToken);
        }

        public static Task<long> Max(this IAsyncEnumerable<long> source, CancellationToken cancellationToken)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));

            return source.Aggregate(Math.Max, cancellationToken);
        }

        public static Task<double> Max(this IAsyncEnumerable<double> source, CancellationToken cancellationToken)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));

            return source.Aggregate(Math.Max, cancellationToken);
        }

        public static Task<float> Max(this IAsyncEnumerable<float> source, CancellationToken cancellationToken)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));

            return source.Aggregate(Math.Max, cancellationToken);
        }

        public static Task<decimal> Max(this IAsyncEnumerable<decimal> source, CancellationToken cancellationToken)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));

            return source.Aggregate(Math.Max, cancellationToken);
        }

        static T? NullableMax<T>(T? x, T? y)
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

        public static Task<int?> Max(this IAsyncEnumerable<int?> source, CancellationToken cancellationToken)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));

            return source.Aggregate(default(int?), NullableMax, cancellationToken);
        }

        public static Task<long?> Max(this IAsyncEnumerable<long?> source, CancellationToken cancellationToken)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));

            return source.Aggregate(default(long?), NullableMax, cancellationToken);
        }

        public static Task<double?> Max(this IAsyncEnumerable<double?> source, CancellationToken cancellationToken)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));

            return source.Aggregate(default(double?), NullableMax, cancellationToken);
        }

        public static Task<float?> Max(this IAsyncEnumerable<float?> source, CancellationToken cancellationToken)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));

            return source.Aggregate(default(float?), NullableMax, cancellationToken);
        }

        public static Task<decimal?> Max(this IAsyncEnumerable<decimal?> source, CancellationToken cancellationToken)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));

            return source.Aggregate(default(decimal?), NullableMax, cancellationToken);
        }

        public static Task<TSource> Max<TSource>(this IAsyncEnumerable<TSource> source, CancellationToken cancellationToken)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));

            var comparer = Comparer<TSource>.Default;
            return source.Aggregate((x, y) => comparer.Compare(x, y) >= 0 ? x : y, cancellationToken);
        }

        public static Task<int> Max<TSource>(this IAsyncEnumerable<TSource> source, Func<TSource, int> selector, CancellationToken cancellationToken)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));
            if (selector == null)
                throw new ArgumentNullException(nameof(selector));

            return source.Select(selector).Max(cancellationToken);
        }

        public static Task<long> Max<TSource>(this IAsyncEnumerable<TSource> source, Func<TSource, long> selector, CancellationToken cancellationToken)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));
            if (selector == null)
                throw new ArgumentNullException(nameof(selector));

            return source.Select(selector).Max(cancellationToken);
        }

        public static Task<double> Max<TSource>(this IAsyncEnumerable<TSource> source, Func<TSource, double> selector, CancellationToken cancellationToken)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));
            if (selector == null)
                throw new ArgumentNullException(nameof(selector));

            return source.Select(selector).Max(cancellationToken);
        }

        public static Task<float> Max<TSource>(this IAsyncEnumerable<TSource> source, Func<TSource, float> selector, CancellationToken cancellationToken)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));
            if (selector == null)
                throw new ArgumentNullException(nameof(selector));

            return source.Select(selector).Max(cancellationToken);
        }

        public static Task<decimal> Max<TSource>(this IAsyncEnumerable<TSource> source, Func<TSource, decimal> selector, CancellationToken cancellationToken)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));
            if (selector == null)
                throw new ArgumentNullException(nameof(selector));

            return source.Select(selector).Max(cancellationToken);
        }

        public static Task<int?> Max<TSource>(this IAsyncEnumerable<TSource> source, Func<TSource, int?> selector, CancellationToken cancellationToken)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));
            if (selector == null)
                throw new ArgumentNullException(nameof(selector));

            return source.Select(selector).Max(cancellationToken);
        }

        public static Task<long?> Max<TSource>(this IAsyncEnumerable<TSource> source, Func<TSource, long?> selector, CancellationToken cancellationToken)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));
            if (selector == null)
                throw new ArgumentNullException(nameof(selector));

            return source.Select(selector).Max(cancellationToken);
        }

        public static Task<double?> Max<TSource>(this IAsyncEnumerable<TSource> source, Func<TSource, double?> selector, CancellationToken cancellationToken)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));
            if (selector == null)
                throw new ArgumentNullException(nameof(selector));

            return source.Select(selector).Max(cancellationToken);
        }

        public static Task<float?> Max<TSource>(this IAsyncEnumerable<TSource> source, Func<TSource, float?> selector, CancellationToken cancellationToken)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));
            if (selector == null)
                throw new ArgumentNullException(nameof(selector));

            return source.Select(selector).Max(cancellationToken);
        }

        public static Task<decimal?> Max<TSource>(this IAsyncEnumerable<TSource> source, Func<TSource, decimal?> selector, CancellationToken cancellationToken)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));
            if (selector == null)
                throw new ArgumentNullException(nameof(selector));

            return source.Select(selector).Max(cancellationToken);
        }

        public static Task<TResult> Max<TSource, TResult>(this IAsyncEnumerable<TSource> source, Func<TSource, TResult> selector, CancellationToken cancellationToken)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));
            if (selector == null)
                throw new ArgumentNullException(nameof(selector));

            return source.Select(selector).Max(cancellationToken);
        }

        public static Task<int> Min(this IAsyncEnumerable<int> source, CancellationToken cancellationToken)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));

            return source.Aggregate(Math.Min, cancellationToken);
        }

        public static Task<long> Min(this IAsyncEnumerable<long> source, CancellationToken cancellationToken)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));

            return source.Aggregate(Math.Min, cancellationToken);
        }

        public static Task<double> Min(this IAsyncEnumerable<double> source, CancellationToken cancellationToken)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));

            return source.Aggregate(Math.Min, cancellationToken);
        }

        public static Task<float> Min(this IAsyncEnumerable<float> source, CancellationToken cancellationToken)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));

            return source.Aggregate(Math.Min, cancellationToken);
        }

        public static Task<decimal> Min(this IAsyncEnumerable<decimal> source, CancellationToken cancellationToken)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));

            return source.Aggregate(Math.Min, cancellationToken);
        }

        static T? NullableMin<T>(T? x, T? y)
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

        public static Task<int?> Min(this IAsyncEnumerable<int?> source, CancellationToken cancellationToken)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));

            return source.Aggregate(default(int?), NullableMin, cancellationToken);
        }

        public static Task<long?> Min(this IAsyncEnumerable<long?> source, CancellationToken cancellationToken)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));

            return source.Aggregate(default(long?), NullableMin, cancellationToken);
        }

        public static Task<double?> Min(this IAsyncEnumerable<double?> source, CancellationToken cancellationToken)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));

            return source.Aggregate(default(double?), NullableMin, cancellationToken);
        }

        public static Task<float?> Min(this IAsyncEnumerable<float?> source, CancellationToken cancellationToken)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));

            return source.Aggregate(default(float?), NullableMin, cancellationToken);
        }

        public static Task<decimal?> Min(this IAsyncEnumerable<decimal?> source, CancellationToken cancellationToken)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));

            return source.Aggregate(default(decimal?), NullableMin, cancellationToken);
        }

        public static Task<TSource> Min<TSource>(this IAsyncEnumerable<TSource> source, CancellationToken cancellationToken)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));

            var comparer = Comparer<TSource>.Default;
            return source.Aggregate((x, y) => comparer.Compare(x, y) <= 0 ? x : y, cancellationToken);
        }

        public static Task<int> Min<TSource>(this IAsyncEnumerable<TSource> source, Func<TSource, int> selector, CancellationToken cancellationToken)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));
            if (selector == null)
                throw new ArgumentNullException(nameof(selector));

            return source.Select(selector).Min(cancellationToken);
        }

        public static Task<long> Min<TSource>(this IAsyncEnumerable<TSource> source, Func<TSource, long> selector, CancellationToken cancellationToken)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));
            if (selector == null)
                throw new ArgumentNullException(nameof(selector));

            return source.Select(selector).Min(cancellationToken);
        }

        public static Task<double> Min<TSource>(this IAsyncEnumerable<TSource> source, Func<TSource, double> selector, CancellationToken cancellationToken)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));
            if (selector == null)
                throw new ArgumentNullException(nameof(selector));

            return source.Select(selector).Min(cancellationToken);
        }

        public static Task<float> Min<TSource>(this IAsyncEnumerable<TSource> source, Func<TSource, float> selector, CancellationToken cancellationToken)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));
            if (selector == null)
                throw new ArgumentNullException(nameof(selector));

            return source.Select(selector).Min(cancellationToken);
        }

        public static Task<decimal> Min<TSource>(this IAsyncEnumerable<TSource> source, Func<TSource, decimal> selector, CancellationToken cancellationToken)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));
            if (selector == null)
                throw new ArgumentNullException(nameof(selector));

            return source.Select(selector).Min(cancellationToken);
        }

        public static Task<int?> Min<TSource>(this IAsyncEnumerable<TSource> source, Func<TSource, int?> selector, CancellationToken cancellationToken)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));
            if (selector == null)
                throw new ArgumentNullException(nameof(selector));

            return source.Select(selector).Min(cancellationToken);
        }

        public static Task<long?> Min<TSource>(this IAsyncEnumerable<TSource> source, Func<TSource, long?> selector, CancellationToken cancellationToken)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));
            if (selector == null)
                throw new ArgumentNullException(nameof(selector));

            return source.Select(selector).Min(cancellationToken);
        }

        public static Task<double?> Min<TSource>(this IAsyncEnumerable<TSource> source, Func<TSource, double?> selector, CancellationToken cancellationToken)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));
            if (selector == null)
                throw new ArgumentNullException(nameof(selector));

            return source.Select(selector).Min(cancellationToken);
        }

        public static Task<float?> Min<TSource>(this IAsyncEnumerable<TSource> source, Func<TSource, float?> selector, CancellationToken cancellationToken)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));
            if (selector == null)
                throw new ArgumentNullException(nameof(selector));

            return source.Select(selector).Min(cancellationToken);
        }

        public static Task<decimal?> Min<TSource>(this IAsyncEnumerable<TSource> source, Func<TSource, decimal?> selector, CancellationToken cancellationToken)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));
            if (selector == null)
                throw new ArgumentNullException(nameof(selector));

            return source.Select(selector).Min(cancellationToken);
        }

        public static Task<TResult> Min<TSource, TResult>(this IAsyncEnumerable<TSource> source, Func<TSource, TResult> selector, CancellationToken cancellationToken)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));
            if (selector == null)
                throw new ArgumentNullException(nameof(selector));

            return source.Select(selector).Min(cancellationToken);
        }

        public static Task<int> Sum(this IAsyncEnumerable<int> source, CancellationToken cancellationToken)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));

            return source.Aggregate(0, (x, y) => x + y, cancellationToken);
        }

        public static Task<long> Sum(this IAsyncEnumerable<long> source, CancellationToken cancellationToken)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));

            return source.Aggregate(0L, (x, y) => x + y, cancellationToken);
        }

        public static Task<double> Sum(this IAsyncEnumerable<double> source, CancellationToken cancellationToken)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));

            return source.Aggregate(0.0, (x, y) => x + y, cancellationToken);
        }

        public static Task<float> Sum(this IAsyncEnumerable<float> source, CancellationToken cancellationToken)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));

            return source.Aggregate(0f, (x, y) => x + y, cancellationToken);
        }

        public static Task<decimal> Sum(this IAsyncEnumerable<decimal> source, CancellationToken cancellationToken)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));

            return source.Aggregate(0m, (x, y) => x + y, cancellationToken);
        }

        public static Task<int?> Sum(this IAsyncEnumerable<int?> source, CancellationToken cancellationToken)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));

            return source.Aggregate((int?)0, (x, y) => x + y.GetValueOrDefault(), cancellationToken);
        }

        public static Task<long?> Sum(this IAsyncEnumerable<long?> source, CancellationToken cancellationToken)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));

            return source.Aggregate((long?)0, (x, y) => x + y.GetValueOrDefault(), cancellationToken);
        }

        public static Task<double?> Sum(this IAsyncEnumerable<double?> source, CancellationToken cancellationToken)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));

            return source.Aggregate((double?)0, (x, y) => x + y.GetValueOrDefault(), cancellationToken);
        }

        public static Task<float?> Sum(this IAsyncEnumerable<float?> source, CancellationToken cancellationToken)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));

            return source.Aggregate((float?)0, (x, y) => x + y.GetValueOrDefault(), cancellationToken);
        }

        public static Task<decimal?> Sum(this IAsyncEnumerable<decimal?> source, CancellationToken cancellationToken)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));

            return source.Aggregate((decimal?)0, (x, y) => x + y.GetValueOrDefault(), cancellationToken);
        }

        public static Task<int> Sum<TSource>(this IAsyncEnumerable<TSource> source, Func<TSource, int> selector, CancellationToken cancellationToken)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));
            if (selector == null)
                throw new ArgumentNullException(nameof(selector));

            return source.Select(selector).Sum(cancellationToken);
        }

        public static Task<long> Sum<TSource>(this IAsyncEnumerable<TSource> source, Func<TSource, long> selector, CancellationToken cancellationToken)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));
            if (selector == null)
                throw new ArgumentNullException(nameof(selector));

            return source.Select(selector).Sum(cancellationToken);
        }

        public static Task<double> Sum<TSource>(this IAsyncEnumerable<TSource> source, Func<TSource, double> selector, CancellationToken cancellationToken)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));
            if (selector == null)
                throw new ArgumentNullException(nameof(selector));

            return source.Select(selector).Sum(cancellationToken);
        }

        public static Task<float> Sum<TSource>(this IAsyncEnumerable<TSource> source, Func<TSource, float> selector, CancellationToken cancellationToken)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));
            if (selector == null)
                throw new ArgumentNullException(nameof(selector));

            return source.Select(selector).Sum(cancellationToken);
        }

        public static Task<decimal> Sum<TSource>(this IAsyncEnumerable<TSource> source, Func<TSource, decimal> selector, CancellationToken cancellationToken)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));
            if (selector == null)
                throw new ArgumentNullException(nameof(selector));

            return source.Select(selector).Sum(cancellationToken);
        }

        public static Task<int?> Sum<TSource>(this IAsyncEnumerable<TSource> source, Func<TSource, int?> selector, CancellationToken cancellationToken)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));
            if (selector == null)
                throw new ArgumentNullException(nameof(selector));

            return source.Select(selector).Sum(cancellationToken);
        }

        public static Task<long?> Sum<TSource>(this IAsyncEnumerable<TSource> source, Func<TSource, long?> selector, CancellationToken cancellationToken)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));
            if (selector == null)
                throw new ArgumentNullException(nameof(selector));

            return source.Select(selector).Sum(cancellationToken);
        }

        public static Task<double?> Sum<TSource>(this IAsyncEnumerable<TSource> source, Func<TSource, double?> selector, CancellationToken cancellationToken)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));
            if (selector == null)
                throw new ArgumentNullException(nameof(selector));

            return source.Select(selector).Sum(cancellationToken);
        }

        public static Task<float?> Sum<TSource>(this IAsyncEnumerable<TSource> source, Func<TSource, float?> selector, CancellationToken cancellationToken)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));
            if (selector == null)
                throw new ArgumentNullException(nameof(selector));

            return source.Select(selector).Sum(cancellationToken);
        }

        public static Task<decimal?> Sum<TSource>(this IAsyncEnumerable<TSource> source, Func<TSource, decimal?> selector, CancellationToken cancellationToken)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));
            if (selector == null)
                throw new ArgumentNullException(nameof(selector));

            return source.Select(selector).Sum(cancellationToken);
        }

        public static Task<bool> IsEmpty<TSource>(this IAsyncEnumerable<TSource> source, CancellationToken cancellationToken)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));

            return IsEmpty_(source, cancellationToken);
        }

        private static async Task<bool> IsEmpty_<TSource>(IAsyncEnumerable<TSource> source, CancellationToken cancellationToken)
        {
            return !await source.Any(cancellationToken).ConfigureAwait(false);
        }

        public static Task<TSource> Min<TSource>(this IAsyncEnumerable<TSource> source, IComparer<TSource> comparer, CancellationToken cancellationToken)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));
            if (comparer == null)
                throw new ArgumentNullException(nameof(comparer));

            return Min_(source, comparer, cancellationToken);
        }

        private static async Task<TSource> Min_<TSource>(IAsyncEnumerable<TSource> source, IComparer<TSource> comparer, CancellationToken cancellationToken)
        {
            return (await MinBy(source, x => x, comparer, cancellationToken).ConfigureAwait(false)).First();
        }

        public static Task<IList<TSource>> MinBy<TSource, TKey>(this IAsyncEnumerable<TSource> source, Func<TSource, TKey> keySelector, CancellationToken cancellationToken)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));
            if (keySelector == null)
                throw new ArgumentNullException(nameof(keySelector));

            return MinBy(source, keySelector, Comparer<TKey>.Default, cancellationToken);
        }

        public static Task<IList<TSource>> MinBy<TSource, TKey>(this IAsyncEnumerable<TSource> source, Func<TSource, TKey> keySelector, IComparer<TKey> comparer, CancellationToken cancellationToken)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));
            if (keySelector == null)
                throw new ArgumentNullException(nameof(keySelector));
            if (comparer == null)
                throw new ArgumentNullException(nameof(comparer));

            return ExtremaBy(source, keySelector, (key, minValue) => -comparer.Compare(key, minValue), cancellationToken);
        }

        public static Task<TSource> Max<TSource>(this IAsyncEnumerable<TSource> source, IComparer<TSource> comparer, CancellationToken cancellationToken)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));
            if (comparer == null)
                throw new ArgumentNullException(nameof(comparer));

            return Max_(source, comparer, cancellationToken);
        }

        private static async Task<TSource> Max_<TSource>(IAsyncEnumerable<TSource> source, IComparer<TSource> comparer, CancellationToken cancellationToken)
        {
            return (await MaxBy(source, x => x, comparer, cancellationToken).ConfigureAwait(false)).First();
        }

        public static Task<IList<TSource>> MaxBy<TSource, TKey>(this IAsyncEnumerable<TSource> source, Func<TSource, TKey> keySelector, CancellationToken cancellationToken)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));
            if (keySelector == null)
                throw new ArgumentNullException(nameof(keySelector));

            return MaxBy(source, keySelector, Comparer<TKey>.Default, cancellationToken);
        }

        public static Task<IList<TSource>> MaxBy<TSource, TKey>(this IAsyncEnumerable<TSource> source, Func<TSource, TKey> keySelector, IComparer<TKey> comparer, CancellationToken cancellationToken)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));
            if (keySelector == null)
                throw new ArgumentNullException(nameof(keySelector));
            if (comparer == null)
                throw new ArgumentNullException(nameof(comparer));

            return ExtremaBy(source, keySelector, (key, minValue) => comparer.Compare(key, minValue), cancellationToken);
        }

        private static async Task<IList<TSource>> ExtremaBy<TSource, TKey>(IAsyncEnumerable<TSource> source, Func<TSource, TKey> keySelector, Func<TKey, TKey, int> compare, CancellationToken cancellationToken)
        {
            var result = new List<TSource>();

            using (var e = source.GetEnumerator())
            {
                if (!await e.MoveNext(cancellationToken).ConfigureAwait(false))
                    throw new InvalidOperationException("Source sequence doesn't contain any elements.");

                var current = e.Current;
                var resKey = keySelector(current);
                result.Add(current);

                while (await e.MoveNext(cancellationToken).ConfigureAwait(false))
                {
                    var cur = e.Current;
                    var key = keySelector(cur);

                    var cmp = compare(key, resKey);
                    if (cmp == 0)
                    {
                        result.Add(cur);
                    }
                    else if (cmp > 0)
                    {
                        result = new List<TSource> { cur };
                        resKey = key;
                    }
                }
            }

            return result;
        }
    }
}

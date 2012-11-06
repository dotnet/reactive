// Copyright (c) Microsoft Open Technologies, Inc. All rights reserved. See License.txt in the project root for license information.
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Threading;

namespace System.Linq
{
    public static partial class AsyncEnumerable
    {
        public static Task<TResult> Aggregate<TSource, TAccumulate, TResult>(this IAsyncEnumerable<TSource> source, TAccumulate seed, Func<TAccumulate, TSource, TAccumulate> accumulator, Func<TAccumulate, TResult> resultSelector, CancellationToken cancellationToken)
        {
            if (source == null)
                throw new ArgumentNullException("source");
            if (accumulator == null)
                throw new ArgumentNullException("accumulator");
            if (resultSelector == null)
                throw new ArgumentNullException("resultSelector");

            var tcs = new TaskCompletionSource<TResult>();

            var acc = seed;

            var e = source.GetEnumerator();

            var f = default(Action<CancellationToken>);
            f = ct => e.MoveNext(ct).ContinueWith(t =>
            {
                t.Handle(tcs, res =>
                {
                    if (res)
                    {
                        try
                        {
                            acc = accumulator(acc, e.Current);
                            f(ct);
                        }
                        catch (Exception exception)
                        {
                            tcs.TrySetException(exception);
                        }
                    }
                    else
                    {
                        var result = default(TResult);
                        try
                        {
                            result = resultSelector(acc);
                        }
                        catch (Exception exception)
                        {
                            tcs.TrySetException(exception);
                            return;
                        }

                        tcs.TrySetResult(result);
                    }
                });
            });

            f(cancellationToken);

            return tcs.Task.Finally(e.Dispose);
        }

        public static Task<TAccumulate> Aggregate<TSource, TAccumulate>(this IAsyncEnumerable<TSource> source, TAccumulate seed, Func<TAccumulate, TSource, TAccumulate> accumulator, CancellationToken cancellationToken)
        {
            if (source == null)
                throw new ArgumentNullException("source");
            if (accumulator == null)
                throw new ArgumentNullException("accumulator");

            return source.Aggregate(seed, accumulator, x => x, cancellationToken);
        }

        public static Task<TSource> Aggregate<TSource>(this IAsyncEnumerable<TSource> source, Func<TSource, TSource, TSource> accumulator, CancellationToken cancellationToken)
        {
            if (source == null)
                throw new ArgumentNullException("source");
            if (accumulator == null)
                throw new ArgumentNullException("accumulator");
            
            var tcs = new TaskCompletionSource<TSource>();

            var first = true;
            var acc = default(TSource);

            var e = source.GetEnumerator();

            var f = default(Action<CancellationToken>);
            f = ct => e.MoveNext(ct).ContinueWith(t =>
            {
                t.Handle(tcs, res =>
                {
                    if (res)
                    {
                        try
                        {
                            if (first)
                                acc = e.Current;
                            else
                                acc = accumulator(acc, e.Current);
                            f(ct);
                        }
                        catch (Exception ex)
                        {
                            tcs.TrySetException(ex);
                        }

                        first = false;
                    }
                    else
                    {
                        if (first)
                            tcs.TrySetException(new InvalidOperationException());
                        else
                            tcs.TrySetResult(acc);
                    }
                });
            });

            f(cancellationToken);

            return tcs.Task.Finally(e.Dispose);
        }

        public static Task<int> Count<TSource>(this IAsyncEnumerable<TSource> source, CancellationToken cancellationToken)
        {
            if (source == null)
                throw new ArgumentNullException("source");

            return source.Aggregate(0, (c, _) => c + 1, cancellationToken);
        }

        public static Task<int> Count<TSource>(this IAsyncEnumerable<TSource> source, Func<TSource, bool> predicate, CancellationToken cancellationToken)
        {
            if (source == null)
                throw new ArgumentNullException("source");
            if (predicate == null)
                throw new ArgumentNullException("predicate");

            return source.Where(predicate).Aggregate(0, (c, _) => c + 1, cancellationToken);
        }

        public static Task<long> LongCount<TSource>(this IAsyncEnumerable<TSource> source, CancellationToken cancellationToken)
        {
            if (source == null)
                throw new ArgumentNullException("source");

            return source.Aggregate(0L, (c, _) => c + 1, cancellationToken);
        }

        public static Task<long> LongCount<TSource>(this IAsyncEnumerable<TSource> source, Func<TSource, bool> predicate, CancellationToken cancellationToken)
        {
            if (source == null)
                throw new ArgumentNullException("source");
            if (predicate == null)
                throw new ArgumentNullException("predicate");

            return source.Where(predicate).Aggregate(0L, (c, _) => c + 1, cancellationToken);
        }

        public static Task<bool> All<TSource>(this IAsyncEnumerable<TSource> source, Func<TSource, bool> predicate, CancellationToken cancellationToken)
        {
            if (source == null)
                throw new ArgumentNullException("source");
            if (predicate == null)
                throw new ArgumentNullException("predicate");

            var tcs = new TaskCompletionSource<bool>();

            var e = source.GetEnumerator();

            var f = default(Action<CancellationToken>);
            f = ct => e.MoveNext(ct).ContinueWith(t =>
            {
                t.Handle(tcs, res =>
                {
                    if (res)
                    {
                        try
                        {
                            if (!predicate(e.Current))
                                tcs.TrySetResult(false);
                            else
                                f(ct);
                        }
                        catch (Exception ex)
                        {
                            tcs.TrySetException(ex);
                        }
                    }
                    else
                    {
                        tcs.TrySetResult(true);
                    }
                });
            });

            f(cancellationToken);

            return tcs.Task.Finally(e.Dispose);
        }

        public static Task<bool> Any<TSource>(this IAsyncEnumerable<TSource> source, Func<TSource, bool> predicate, CancellationToken cancellationToken)
        {
            if (source == null)
                throw new ArgumentNullException("source");
            if (predicate == null)
                throw new ArgumentNullException("predicate");

            var tcs = new TaskCompletionSource<bool>();

            var e = source.GetEnumerator();

            var f = default(Action<CancellationToken>);
            f = ct => e.MoveNext(ct).ContinueWith(t =>
            {
                t.Handle(tcs, res =>
                {
                    if (res)
                    {
                        try
                        {
                            if (predicate(e.Current))
                                tcs.TrySetResult(true);
                            else
                                f(ct);
                        }
                        catch (Exception ex)
                        {
                            tcs.TrySetException(ex);
                        }
                    }
                    else
                    {
                        tcs.TrySetResult(false);
                    }
                });
            });

            f(cancellationToken);

            return tcs.Task.Finally(e.Dispose);
        }

        public static Task<bool> Any<TSource>(this IAsyncEnumerable<TSource> source, CancellationToken cancellationToken)
        {
            if (source == null)
                throw new ArgumentNullException("source");

            var e = source.GetEnumerator();
            return e.MoveNext(cancellationToken);
        }

        public static Task<bool> Contains<TSource>(this IAsyncEnumerable<TSource> source, TSource value, IEqualityComparer<TSource> comparer, CancellationToken cancellationToken)
        {
            if (source == null)
                throw new ArgumentNullException("source");
            if (comparer == null)
                throw new ArgumentNullException("comparer");

            return source.Any(x => comparer.Equals(x, value), cancellationToken);
        }

        public static Task<bool> Contains<TSource>(this IAsyncEnumerable<TSource> source, TSource value, CancellationToken cancellationToken)
        {
            if (source == null)
                throw new ArgumentNullException("source");

            return source.Contains(value, EqualityComparer<TSource>.Default, cancellationToken);
        }

        public static Task<TSource> First<TSource>(this IAsyncEnumerable<TSource> source, CancellationToken cancellationToken)
        {
            if (source == null)
                throw new ArgumentNullException("source");

            var tcs = new TaskCompletionSource<TSource>();

            var e = source.GetEnumerator();

            e.MoveNext(cancellationToken).ContinueWith(t =>
            {
                t.Handle(tcs, res =>
                {
                    if (res)
                        tcs.TrySetResult(e.Current);
                    else
                        tcs.TrySetException(new InvalidOperationException());
                });
            });

            return tcs.Task.Finally(e.Dispose);
        }

        public static Task<TSource> First<TSource>(this IAsyncEnumerable<TSource> source, Func<TSource, bool> predicate, CancellationToken cancellationToken)
        {
            if (source == null)
                throw new ArgumentNullException("source");
            if (predicate == null)
                throw new ArgumentNullException("predicate");

            return source.Where(predicate).First(cancellationToken);
        }

        public static Task<TSource> FirstOrDefault<TSource>(this IAsyncEnumerable<TSource> source, CancellationToken cancellationToken)
        {
            if (source == null)
                throw new ArgumentNullException("source");

            var tcs = new TaskCompletionSource<TSource>();

            var e = source.GetEnumerator();

            e.MoveNext(cancellationToken).ContinueWith(t =>
            {
                t.Handle(tcs, res =>
                {
                    if (res)
                        tcs.TrySetResult(e.Current);
                    else
                        tcs.TrySetResult(default(TSource));
                });
            });

            return tcs.Task.Finally(e.Dispose);
        }

        public static Task<TSource> FirstOrDefault<TSource>(this IAsyncEnumerable<TSource> source, Func<TSource, bool> predicate, CancellationToken cancellationToken)
        {
            if (source == null)
                throw new ArgumentNullException("source");
            if (predicate == null)
                throw new ArgumentNullException("predicate");

            return source.Where(predicate).FirstOrDefault(cancellationToken);
        }

        public static Task<TSource> Last<TSource>(this IAsyncEnumerable<TSource> source, CancellationToken cancellationToken)
        {
            if (source == null)
                throw new ArgumentNullException("source");

            var tcs = new TaskCompletionSource<TSource>();

            var e = source.GetEnumerator();
            var last = default(TSource);
            var hasLast = false;

            var f = default(Action<CancellationToken>);
            f = ct => e.MoveNext(ct).ContinueWith(t =>
            {
                t.Handle(tcs, res =>
                {
                    if (res)
                    {
                        hasLast = true;
                        last = e.Current;
                        f(ct);
                    }
                    else
                    {
                        if (!hasLast)
                            tcs.TrySetException(new InvalidOperationException());
                        else
                            tcs.TrySetResult(last);
                    }
                });
            });

            f(cancellationToken);

            return tcs.Task.Finally(e.Dispose);
        }

        public static Task<TSource> Last<TSource>(this IAsyncEnumerable<TSource> source, Func<TSource, bool> predicate, CancellationToken cancellationToken)
        {
            if (source == null)
                throw new ArgumentNullException("source");
            if (predicate == null)
                throw new ArgumentNullException("predicate");

            return source.Where(predicate).Last(cancellationToken);
        }

        public static Task<TSource> LastOrDefault<TSource>(this IAsyncEnumerable<TSource> source, CancellationToken cancellationToken)
        {
            if (source == null)
                throw new ArgumentNullException("source");

            var tcs = new TaskCompletionSource<TSource>();

            var e = source.GetEnumerator();
            var last = default(TSource);
            var hasLast = false;

            var f = default(Action<CancellationToken>);
            f = ct => e.MoveNext(ct).ContinueWith(t =>
            {
                t.Handle(tcs, res =>
                {
                    if (res)
                    {
                        hasLast = true;
                        last = e.Current;
                        f(ct);
                    }
                    else
                    {
                        if (!hasLast)
                            tcs.TrySetResult(default(TSource));
                        else
                            tcs.TrySetResult(last);
                    }
                });
            });

            f(cancellationToken);

            return tcs.Task.Finally(e.Dispose);
        }

        public static Task<TSource> LastOrDefault<TSource>(this IAsyncEnumerable<TSource> source, Func<TSource, bool> predicate, CancellationToken cancellationToken)
        {
            if (source == null)
                throw new ArgumentNullException("source");
            if (predicate == null)
                throw new ArgumentNullException("predicate");

            return source.Where(predicate).LastOrDefault(cancellationToken);
        }

        public static Task<TSource> Single<TSource>(this IAsyncEnumerable<TSource> source, CancellationToken cancellationToken)
        {
            if (source == null)
                throw new ArgumentNullException("source");

            var tcs = new TaskCompletionSource<TSource>();

            var e = source.GetEnumerator();

            e.MoveNext(cancellationToken).ContinueWith(t =>
            {
                t.Handle(tcs, res =>
                {
                    if (res)
                    {
                        var result = e.Current;
                        e.MoveNext(cancellationToken).ContinueWith(t1 =>
                        {
                            t1.Handle(tcs, res1 =>
                            {
                                if (res1)
                                    tcs.TrySetException(new InvalidOperationException());
                                else
                                    tcs.TrySetResult(result);
                            });
                        });
                    }
                    else
                        tcs.TrySetException(new InvalidOperationException());
                });
            });

            return tcs.Task.Finally(e.Dispose);
        }

        public static Task<TSource> Single<TSource>(this IAsyncEnumerable<TSource> source, Func<TSource, bool> predicate, CancellationToken cancellationToken)
        {
            if (source == null)
                throw new ArgumentNullException("source");
            if (predicate == null)
                throw new ArgumentNullException("predicate");

            return source.Where(predicate).Single(cancellationToken);
        }

        public static Task<TSource> SingleOrDefault<TSource>(this IAsyncEnumerable<TSource> source, CancellationToken cancellationToken)
        {
            if (source == null)
                throw new ArgumentNullException("source");

            var tcs = new TaskCompletionSource<TSource>();

            var e = source.GetEnumerator();

            e.MoveNext(cancellationToken).ContinueWith(t =>
            {
                t.Handle(tcs, res =>
                {
                    if (res)
                    {
                        var result = e.Current;
                        e.MoveNext(cancellationToken).ContinueWith(t1 =>
                        {
                            t1.Handle(tcs, res1 =>
                            {
                                if (res1)
                                    tcs.TrySetException(new InvalidOperationException());
                                else
                                    tcs.TrySetResult(result);
                            });
                        });
                    }
                    else
                        tcs.TrySetResult(default(TSource));
                });
            });

            return tcs.Task.Finally(e.Dispose);
        }

        public static Task<TSource> SingleOrDefault<TSource>(this IAsyncEnumerable<TSource> source, Func<TSource, bool> predicate, CancellationToken cancellationToken)
        {
            if (source == null)
                throw new ArgumentNullException("source");
            if (predicate == null)
                throw new ArgumentNullException("predicate");

            return source.Where(predicate).SingleOrDefault(cancellationToken);
        }

        public static Task<TSource> ElementAt<TSource>(this IAsyncEnumerable<TSource> source, int index, CancellationToken cancellationToken)
        {
            if (source == null)
                throw new ArgumentNullException("source");
            if (index < 0)
                throw new ArgumentOutOfRangeException("index");

            var tcs = new TaskCompletionSource<TSource>();

            var e = source.GetEnumerator();

            var next = default(Action<CancellationToken>);
            next = ct => e.MoveNext(ct).ContinueWith(t =>
            {
                t.Handle(tcs, res =>
                {
                    if (res)
                    {
                        if (index == 0)
                        {
                            tcs.TrySetResult(e.Current);
                        }
                        else
                        {
                            index--;
                            next(ct);
                        }
                    }
                    else
                    {
                        tcs.TrySetException(new ArgumentOutOfRangeException());
                    }
                });
            });

            next(cancellationToken);

            return tcs.Task.Finally(e.Dispose);
        }

        public static Task<TSource> ElementAtOrDefault<TSource>(this IAsyncEnumerable<TSource> source, int index, CancellationToken cancellationToken)
        {
            if (source == null)
                throw new ArgumentNullException("source");
            if (index < 0)
                throw new ArgumentOutOfRangeException("index");

            var tcs = new TaskCompletionSource<TSource>();

            var e = source.GetEnumerator();

            var next = default(Action<CancellationToken>);
            next = ct => e.MoveNext(ct).ContinueWith(t =>
            {
                t.Handle(tcs, res =>
                {
                    if (res)
                    {
                        if (index == 0)
                        {
                            tcs.TrySetResult(e.Current);
                        }
                        else
                        {
                            index--;
                            next(ct);
                        }
                    }
                    else
                    {
                        tcs.TrySetResult(default(TSource));
                    }
                });
            });

            next(cancellationToken);

            return tcs.Task.Finally(e.Dispose);
        }

        public static Task<TSource[]> ToArray<TSource>(this IAsyncEnumerable<TSource> source, CancellationToken cancellationToken)
        {
            if (source == null)
                throw new ArgumentNullException("source");

            return source.Aggregate(new List<TSource>(), (list, x) => { list.Add(x); return list; }, list => list.ToArray(), cancellationToken);
        }

        public static Task<List<TSource>> ToList<TSource>(this IAsyncEnumerable<TSource> source, CancellationToken cancellationToken)
        {
            if (source == null)
                throw new ArgumentNullException("source");

            return source.Aggregate(new List<TSource>(), (list, x) => { list.Add(x); return list; }, cancellationToken);
        }

        public static Task<Dictionary<TKey, TElement>> ToDictionary<TSource, TKey, TElement>(this IAsyncEnumerable<TSource> source, Func<TSource, TKey> keySelector, Func<TSource, TElement> elementSelector, IEqualityComparer<TKey> comparer, CancellationToken cancellationToken)
        {
            if (source == null)
                throw new ArgumentNullException("source");
            if (keySelector == null)
                throw new ArgumentNullException("keySelector");
            if (elementSelector == null)
                throw new ArgumentNullException("elementSelector");
            if (comparer == null)
                throw new ArgumentNullException("comparer");

            return source.Aggregate(new Dictionary<TKey, TElement>(comparer), (d, x) => { d.Add(keySelector(x), elementSelector(x)); return d; }, cancellationToken);
        }

        public static Task<Dictionary<TKey, TElement>> ToDictionary<TSource, TKey, TElement>(this IAsyncEnumerable<TSource> source, Func<TSource, TKey> keySelector, Func<TSource, TElement> elementSelector, CancellationToken cancellationToken)
        {
            if (source == null)
                throw new ArgumentNullException("source");
            if (keySelector == null)
                throw new ArgumentNullException("keySelector");
            if (elementSelector == null)
                throw new ArgumentNullException("elementSelector");

            return source.ToDictionary(keySelector, elementSelector, EqualityComparer<TKey>.Default, cancellationToken);
        }

        public static Task<Dictionary<TKey, TSource>> ToDictionary<TSource, TKey>(this IAsyncEnumerable<TSource> source, Func<TSource, TKey> keySelector, IEqualityComparer<TKey> comparer, CancellationToken cancellationToken)
        {
            if (source == null)
                throw new ArgumentNullException("source");
            if (keySelector == null)
                throw new ArgumentNullException("keySelector");
            if (comparer == null)
                throw new ArgumentNullException("comparer");

            return source.ToDictionary(keySelector, x => x, comparer, cancellationToken);
        }

        public static Task<Dictionary<TKey, TSource>> ToDictionary<TSource, TKey>(this IAsyncEnumerable<TSource> source, Func<TSource, TKey> keySelector, CancellationToken cancellationToken)
        {
            if (source == null)
                throw new ArgumentNullException("source");
            if (keySelector == null)
                throw new ArgumentNullException("keySelector");

            return source.ToDictionary(keySelector, x => x, EqualityComparer<TKey>.Default, cancellationToken);
        }

        public static Task<ILookup<TKey, TElement>> ToLookup<TSource, TKey, TElement>(this IAsyncEnumerable<TSource> source, Func<TSource, TKey> keySelector, Func<TSource, TElement> elementSelector, IEqualityComparer<TKey> comparer, CancellationToken cancellationToken)
        {
            if (source == null)
                throw new ArgumentNullException("source");
            if (keySelector == null)
                throw new ArgumentNullException("keySelector");
            if (elementSelector == null)
                throw new ArgumentNullException("elementSelector");
            if (comparer == null)
                throw new ArgumentNullException("comparer");

            return source.Aggregate(new Lookup<TKey, TElement>(comparer), (lookup, x) => { lookup.Add(keySelector(x), elementSelector(x)); return lookup; }, lookup => (ILookup<TKey, TElement>)lookup, cancellationToken);
        }

        public static Task<ILookup<TKey, TElement>> ToLookup<TSource, TKey, TElement>(this IAsyncEnumerable<TSource> source, Func<TSource, TKey> keySelector, Func<TSource, TElement> elementSelector, CancellationToken cancellationToken)
        {
            if (source == null)
                throw new ArgumentNullException("source");
            if (keySelector == null)
                throw new ArgumentNullException("keySelector");
            if (elementSelector == null)
                throw new ArgumentNullException("elementSelector");

            return source.ToLookup(keySelector, elementSelector, EqualityComparer<TKey>.Default, cancellationToken);
        }

        public static Task<ILookup<TKey, TSource>> ToLookup<TSource, TKey>(this IAsyncEnumerable<TSource> source, Func<TSource, TKey> keySelector, IEqualityComparer<TKey> comparer, CancellationToken cancellationToken)
        {
            if (source == null)
                throw new ArgumentNullException("source");
            if (keySelector == null)
                throw new ArgumentNullException("keySelector");
            if (comparer == null)
                throw new ArgumentNullException("comparer");

            return source.ToLookup(keySelector, x => x, comparer, cancellationToken);
        }

        public static Task<ILookup<TKey, TSource>> ToLookup<TSource, TKey>(this IAsyncEnumerable<TSource> source, Func<TSource, TKey> keySelector, CancellationToken cancellationToken)
        {
            if (source == null)
                throw new ArgumentNullException("source");
            if (keySelector == null)
                throw new ArgumentNullException("keySelector");

            return source.ToLookup(keySelector, x => x, EqualityComparer<TKey>.Default, cancellationToken);
        }

        class Lookup<TKey, TElement> : ILookup<TKey, TElement>
        {
            private readonly Dictionary<TKey, EnumerableGrouping<TKey, TElement>> map;

            public Lookup(IEqualityComparer<TKey> comparer)
            {
                map = new Dictionary<TKey, EnumerableGrouping<TKey, TElement>>(comparer);
            }

            public void Add(TKey key, TElement element)
            {
                var g = default(EnumerableGrouping<TKey, TElement>);
                if (!map.TryGetValue(key, out g))
                {
                    g = new EnumerableGrouping<TKey, TElement>(key);
                    map.Add(key, g);
                }

                g.Add(element);
            }

            public bool Contains(TKey key)
            {
                return map.ContainsKey(key);
            }

            public int Count
            {
                get { return map.Keys.Count; }
            }

            public IEnumerable<TElement> this[TKey key]
            {
                get { return map[key]; }
            }

            public IEnumerator<IGrouping<TKey, TElement>> GetEnumerator()
            {
                return map.Values.Cast<IGrouping<TKey, TElement>>().GetEnumerator();
            }

            System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
            {
                return GetEnumerator();
            }
        }

        public static Task<double> Average(this IAsyncEnumerable<int> source, CancellationToken cancellationToken)
        {
            if (source == null)
                throw new ArgumentNullException("source");

            var tcs = new TaskCompletionSource<double>();

            var count = 0L;
            var sum = 0.0;

            var e = source.GetEnumerator();

            var f = default(Action<CancellationToken>);
            f = ct => e.MoveNext(ct).ContinueWith(t =>
            {
                t.Handle(tcs, res =>
                {
                    if (res)
                    {
                        count++;
                        sum += e.Current;
                        f(ct);
                    }
                    else
                    {
                        if (count == 0)
                            tcs.TrySetException(new InvalidOperationException());
                        else
                            tcs.TrySetResult(sum / count);
                    }
                });
            });

            f(cancellationToken);

            return tcs.Task.Finally(e.Dispose);
        }

        public static Task<double?> Average(this IAsyncEnumerable<int?> source, CancellationToken cancellationToken)
        {
            if (source == null)
                throw new ArgumentNullException("source");

            var tcs = new TaskCompletionSource<double?>();

            var count = 0L;
            var sum = 0.0;

            var e = source.GetEnumerator();

            var f = default(Action<CancellationToken>);
            f = ct => e.MoveNext(ct).ContinueWith(t =>
            {
                t.Handle(tcs, res =>
                {
                    if (res)
                    {
                        if (e.Current.HasValue)
                        {
                            count++;
                            sum += e.Current.Value;
                        }
                        f(ct);
                    }
                    else
                    {
                        if (count == 0)
                            tcs.TrySetResult(null);
                        else
                            tcs.TrySetResult(sum / count);
                    }
                });
            });

            f(cancellationToken);

            return tcs.Task.Finally(e.Dispose);
        }

        public static Task<double> Average(this IAsyncEnumerable<long> source, CancellationToken cancellationToken)
        {
            if (source == null)
                throw new ArgumentNullException("source");

            var tcs = new TaskCompletionSource<double>();

            var count = 0L;
            var sum = 0.0;

            var e = source.GetEnumerator();

            var f = default(Action<CancellationToken>);
            f = ct => e.MoveNext(ct).ContinueWith(t =>
            {
                t.Handle(tcs, res =>
                {
                    if (res)
                    {
                        count++;
                        sum += e.Current;
                        f(ct);
                    }
                    else
                    {
                        if (count == 0)
                            tcs.TrySetException(new InvalidOperationException());
                        else
                            tcs.TrySetResult(sum / count);
                    }
                });
            });

            f(cancellationToken);

            return tcs.Task.Finally(e.Dispose);
        }

        public static Task<double?> Average(this IAsyncEnumerable<long?> source, CancellationToken cancellationToken)
        {
            if (source == null)
                throw new ArgumentNullException("source");

            var tcs = new TaskCompletionSource<double?>();

            var count = 0L;
            var sum = 0.0;

            var e = source.GetEnumerator();

            var f = default(Action<CancellationToken>);
            f = ct => e.MoveNext(ct).ContinueWith(t =>
            {
                t.Handle(tcs, res =>
                {
                    if (res)
                    {
                        if (e.Current.HasValue)
                        {
                            count++;
                            sum += e.Current.Value;
                        }
                        f(ct);
                    }
                    else
                    {
                        if (count == 0)
                            tcs.TrySetResult(null);
                        else
                            tcs.TrySetResult(sum / count);
                    }
                });
            });

            f(cancellationToken);

            return tcs.Task.Finally(e.Dispose);
        }

        public static Task<double> Average(this IAsyncEnumerable<double> source, CancellationToken cancellationToken)
        {
            if (source == null)
                throw new ArgumentNullException("source");

            var tcs = new TaskCompletionSource<double>();

            var count = 0L;
            var sum = 0.0;

            var e = source.GetEnumerator();

            var f = default(Action<CancellationToken>);
            f = ct => e.MoveNext(ct).ContinueWith(t =>
            {
                t.Handle(tcs, res =>
                {
                    if (res)
                    {
                        count++;
                        sum += e.Current;
                        f(ct);
                    }
                    else
                    {
                        if (count == 0)
                            tcs.TrySetException(new InvalidOperationException());
                        else
                            tcs.TrySetResult(sum / count);
                    }
                });
            });

            f(cancellationToken);

            return tcs.Task.Finally(e.Dispose);
        }

        public static Task<double?> Average(this IAsyncEnumerable<double?> source, CancellationToken cancellationToken)
        {
            if (source == null)
                throw new ArgumentNullException("source");

            var tcs = new TaskCompletionSource<double?>();

            var count = 0L;
            var sum = 0.0;

            var e = source.GetEnumerator();

            var f = default(Action<CancellationToken>);
            f = ct => e.MoveNext(ct).ContinueWith(t =>
            {
                t.Handle(tcs, res =>
                {
                    if (res)
                    {
                        if (e.Current.HasValue)
                        {
                            count++;
                            sum += e.Current.Value;
                        }
                        f(ct);
                    }
                    else
                    {
                        if (count == 0)
                            tcs.TrySetResult(null);
                        else
                            tcs.TrySetResult(sum / count);
                    }
                });
            });

            f(cancellationToken);

            return tcs.Task.Finally(e.Dispose);
        }

        public static Task<float> Average(this IAsyncEnumerable<float> source, CancellationToken cancellationToken)
        {
            if (source == null)
                throw new ArgumentNullException("source");

            var tcs = new TaskCompletionSource<float>();

            var count = 0L;
            var sum = 0f;

            var e = source.GetEnumerator();

            var f = default(Action<CancellationToken>);
            f = ct => e.MoveNext(ct).ContinueWith(t =>
            {
                t.Handle(tcs, res =>
                {
                    if (res)
                    {
                        count++;
                        sum += e.Current;
                        f(ct);
                    }
                    else
                    {
                        if (count == 0)
                            tcs.TrySetException(new InvalidOperationException());
                        else
                            tcs.TrySetResult(sum / count);
                    }
                });
            });

            f(cancellationToken);

            return tcs.Task.Finally(e.Dispose);
        }

        public static Task<float?> Average(this IAsyncEnumerable<float?> source, CancellationToken cancellationToken)
        {
            if (source == null)
                throw new ArgumentNullException("source");

            var tcs = new TaskCompletionSource<float?>();

            var count = 0L;
            var sum = 0f;

            var e = source.GetEnumerator();

            var f = default(Action<CancellationToken>);
            f = ct => e.MoveNext(ct).ContinueWith(t =>
            {
                t.Handle(tcs, res =>
                {
                    if (res)
                    {
                        if (e.Current.HasValue)
                        {
                            count++;
                            sum += e.Current.Value;
                        }
                        f(ct);
                    }
                    else
                    {
                        if (count == 0)
                            tcs.TrySetResult(null);
                        else
                            tcs.TrySetResult(sum / count);
                    }
                });
            });

            f(cancellationToken);

            return tcs.Task.Finally(e.Dispose);
        }

        public static Task<decimal> Average(this IAsyncEnumerable<decimal> source, CancellationToken cancellationToken)
        {
            if (source == null)
                throw new ArgumentNullException("source");

            var tcs = new TaskCompletionSource<decimal>();

            var count = 0L;
            var sum = 0m;

            var e = source.GetEnumerator();

            var f = default(Action<CancellationToken>);
            f = ct => e.MoveNext(ct).ContinueWith(t =>
            {
                t.Handle(tcs, res =>
                {
                    if (res)
                    {
                        count++;
                        sum += e.Current;
                        f(ct);
                    }
                    else
                    {
                        if (count == 0)
                            tcs.TrySetException(new InvalidOperationException());
                        else
                            tcs.TrySetResult(sum / count);
                    }
                });
            });

            f(cancellationToken);

            return tcs.Task.Finally(e.Dispose);
        }

        public static Task<decimal?> Average(this IAsyncEnumerable<decimal?> source, CancellationToken cancellationToken)
        {
            if (source == null)
                throw new ArgumentNullException("source");

            var tcs = new TaskCompletionSource<decimal?>();

            var count = 0L;
            var sum = 0m;

            var e = source.GetEnumerator();

            var f = default(Action<CancellationToken>);
            f = ct => e.MoveNext(ct).ContinueWith(t =>
            {
                t.Handle(tcs, res =>
                {
                    if (res)
                    {
                        if (e.Current.HasValue)
                        {
                            count++;
                            sum += e.Current.Value;
                        }
                        f(ct);
                    }
                    else
                    {
                        if (count == 0)
                            tcs.TrySetResult(null);
                        else
                            tcs.TrySetResult(sum / count);
                    }
                });
            });

            f(cancellationToken);

            return tcs.Task.Finally(e.Dispose);
        }

        public static Task<double?> Average<TSource>(this IAsyncEnumerable<TSource> source, Func<TSource, int?> selector, CancellationToken cancellationToken)
        {
            if (source == null)
                throw new ArgumentNullException("source");
            if (selector == null)
                throw new ArgumentNullException("selector");

            return source.Select(selector).Average(cancellationToken);
        }

        public static Task<double> Average<TSource>(this IAsyncEnumerable<TSource> source, Func<TSource, int> selector, CancellationToken cancellationToken)
        {
            if (source == null)
                throw new ArgumentNullException("source");
            if (selector == null)
                throw new ArgumentNullException("selector");

            return source.Select(selector).Average(cancellationToken);
        }

        public static Task<double> Average<TSource>(this IAsyncEnumerable<TSource> source, Func<TSource, long> selector, CancellationToken cancellationToken)
        {
            if (source == null)
                throw new ArgumentNullException("source");
            if (selector == null)
                throw new ArgumentNullException("selector");

            return source.Select(selector).Average(cancellationToken);
        }

        public static Task<double?> Average<TSource>(this IAsyncEnumerable<TSource> source, Func<TSource, long?> selector, CancellationToken cancellationToken)
        {
            if (source == null)
                throw new ArgumentNullException("source");
            if (selector == null)
                throw new ArgumentNullException("selector");

            return source.Select(selector).Average(cancellationToken);
        }

        public static Task<double> Average<TSource>(this IAsyncEnumerable<TSource> source, Func<TSource, double> selector, CancellationToken cancellationToken)
        {
            if (source == null)
                throw new ArgumentNullException("source");
            if (selector == null)
                throw new ArgumentNullException("selector");

            return source.Select(selector).Average(cancellationToken);
        }

        public static Task<double?> Average<TSource>(this IAsyncEnumerable<TSource> source, Func<TSource, double?> selector, CancellationToken cancellationToken)
        {
            if (source == null)
                throw new ArgumentNullException("source");
            if (selector == null)
                throw new ArgumentNullException("selector");

            return source.Select(selector).Average(cancellationToken);
        }

        public static Task<float> Average<TSource>(this IAsyncEnumerable<TSource> source, Func<TSource, float> selector, CancellationToken cancellationToken)
        {
            if (source == null)
                throw new ArgumentNullException("source");
            if (selector == null)
                throw new ArgumentNullException("selector");

            return source.Select(selector).Average(cancellationToken);
        }

        public static Task<float?> Average<TSource>(this IAsyncEnumerable<TSource> source, Func<TSource, float?> selector, CancellationToken cancellationToken)
        {
            if (source == null)
                throw new ArgumentNullException("source");
            if (selector == null)
                throw new ArgumentNullException("selector");

            return source.Select(selector).Average(cancellationToken);
        }

        public static Task<decimal> Average<TSource>(this IAsyncEnumerable<TSource> source, Func<TSource, decimal> selector, CancellationToken cancellationToken)
        {
            if (source == null)
                throw new ArgumentNullException("source");
            if (selector == null)
                throw new ArgumentNullException("selector");

            return source.Select(selector).Average(cancellationToken);
        }

        public static Task<decimal?> Average<TSource>(this IAsyncEnumerable<TSource> source, Func<TSource, decimal?> selector, CancellationToken cancellationToken)
        {
            if (source == null)
                throw new ArgumentNullException("source");
            if (selector == null)
                throw new ArgumentNullException("selector");

            return source.Select(selector).Average(cancellationToken);
        }

        public static Task<int> Max(this IAsyncEnumerable<int> source, CancellationToken cancellationToken)
        {
            if (source == null)
                throw new ArgumentNullException("source");

            return source.Aggregate(Math.Max, cancellationToken);
        }

        public static Task<long> Max(this IAsyncEnumerable<long> source, CancellationToken cancellationToken)
        {
            if (source == null)
                throw new ArgumentNullException("source");

            return source.Aggregate(Math.Max, cancellationToken);
        }

        public static Task<double> Max(this IAsyncEnumerable<double> source, CancellationToken cancellationToken)
        {
            if (source == null)
                throw new ArgumentNullException("source");

            return source.Aggregate(Math.Max, cancellationToken);
        }

        public static Task<float> Max(this IAsyncEnumerable<float> source, CancellationToken cancellationToken)
        {
            if (source == null)
                throw new ArgumentNullException("source");

            return source.Aggregate(Math.Max, cancellationToken);
        }

        public static Task<decimal> Max(this IAsyncEnumerable<decimal> source, CancellationToken cancellationToken)
        {
            if (source == null)
                throw new ArgumentNullException("source");

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
                throw new ArgumentNullException("source");

            return source.Aggregate(default(int?), NullableMax, cancellationToken);
        }

        public static Task<long?> Max(this IAsyncEnumerable<long?> source, CancellationToken cancellationToken)
        {
            if (source == null)
                throw new ArgumentNullException("source");

            return source.Aggregate(default(long?), NullableMax, cancellationToken);
        }

        public static Task<double?> Max(this IAsyncEnumerable<double?> source, CancellationToken cancellationToken)
        {
            if (source == null)
                throw new ArgumentNullException("source");

            return source.Aggregate(default(double?), NullableMax, cancellationToken);
        }

        public static Task<float?> Max(this IAsyncEnumerable<float?> source, CancellationToken cancellationToken)
        {
            if (source == null)
                throw new ArgumentNullException("source");

            return source.Aggregate(default(float?), NullableMax, cancellationToken);
        }

        public static Task<decimal?> Max(this IAsyncEnumerable<decimal?> source, CancellationToken cancellationToken)
        {
            if (source == null)
                throw new ArgumentNullException("source");

            return source.Aggregate(default(decimal?), NullableMax, cancellationToken);
        }

        public static Task<TSource> Max<TSource>(this IAsyncEnumerable<TSource> source, CancellationToken cancellationToken)
        {
            if (source == null)
                throw new ArgumentNullException("source");

            var comparer = Comparer<TSource>.Default;
            return source.Aggregate((x, y) => comparer.Compare(x, y) >= 0 ? x : y, cancellationToken);
        }

        public static Task<int> Max<TSource>(this IAsyncEnumerable<TSource> source, Func<TSource, int> selector, CancellationToken cancellationToken)
        {
            if (source == null)
                throw new ArgumentNullException("source");
            if (selector == null)
                throw new ArgumentNullException("selector");

            return source.Select(selector).Max(cancellationToken);
        }

        public static Task<long> Max<TSource>(this IAsyncEnumerable<TSource> source, Func<TSource, long> selector, CancellationToken cancellationToken)
        {
            if (source == null)
                throw new ArgumentNullException("source");
            if (selector == null)
                throw new ArgumentNullException("selector");

            return source.Select(selector).Max(cancellationToken);
        }

        public static Task<double> Max<TSource>(this IAsyncEnumerable<TSource> source, Func<TSource, double> selector, CancellationToken cancellationToken)
        {
            if (source == null)
                throw new ArgumentNullException("source");
            if (selector == null)
                throw new ArgumentNullException("selector");

            return source.Select(selector).Max(cancellationToken);
        }

        public static Task<float> Max<TSource>(this IAsyncEnumerable<TSource> source, Func<TSource, float> selector, CancellationToken cancellationToken)
        {
            if (source == null)
                throw new ArgumentNullException("source");
            if (selector == null)
                throw new ArgumentNullException("selector");

            return source.Select(selector).Max(cancellationToken);
        }

        public static Task<decimal> Max<TSource>(this IAsyncEnumerable<TSource> source, Func<TSource, decimal> selector, CancellationToken cancellationToken)
        {
            if (source == null)
                throw new ArgumentNullException("source");
            if (selector == null)
                throw new ArgumentNullException("selector");

            return source.Select(selector).Max(cancellationToken);
        }

        public static Task<int?> Max<TSource>(this IAsyncEnumerable<TSource> source, Func<TSource, int?> selector, CancellationToken cancellationToken)
        {
            if (source == null)
                throw new ArgumentNullException("source");
            if (selector == null)
                throw new ArgumentNullException("selector");

            return source.Select(selector).Max(cancellationToken);
        }

        public static Task<long?> Max<TSource>(this IAsyncEnumerable<TSource> source, Func<TSource, long?> selector, CancellationToken cancellationToken)
        {
            if (source == null)
                throw new ArgumentNullException("source");
            if (selector == null)
                throw new ArgumentNullException("selector");

            return source.Select(selector).Max(cancellationToken);
        }

        public static Task<double?> Max<TSource>(this IAsyncEnumerable<TSource> source, Func<TSource, double?> selector, CancellationToken cancellationToken)
        {
            if (source == null)
                throw new ArgumentNullException("source");
            if (selector == null)
                throw new ArgumentNullException("selector");

            return source.Select(selector).Max(cancellationToken);
        }

        public static Task<float?> Max<TSource>(this IAsyncEnumerable<TSource> source, Func<TSource, float?> selector, CancellationToken cancellationToken)
        {
            if (source == null)
                throw new ArgumentNullException("source");
            if (selector == null)
                throw new ArgumentNullException("selector");

            return source.Select(selector).Max(cancellationToken);
        }

        public static Task<decimal?> Max<TSource>(this IAsyncEnumerable<TSource> source, Func<TSource, decimal?> selector, CancellationToken cancellationToken)
        {
            if (source == null)
                throw new ArgumentNullException("source");
            if (selector == null)
                throw new ArgumentNullException("selector");

            return source.Select(selector).Max(cancellationToken);
        }

        public static Task<TResult> Max<TSource, TResult>(this IAsyncEnumerable<TSource> source, Func<TSource, TResult> selector, CancellationToken cancellationToken)
        {
            if (source == null)
                throw new ArgumentNullException("source");
            if (selector == null)
                throw new ArgumentNullException("selector");

            return source.Select(selector).Max(cancellationToken);
        }

        public static Task<int> Min(this IAsyncEnumerable<int> source, CancellationToken cancellationToken)
        {
            if (source == null)
                throw new ArgumentNullException("source");

            return source.Aggregate(Math.Min, cancellationToken);
        }

        public static Task<long> Min(this IAsyncEnumerable<long> source, CancellationToken cancellationToken)
        {
            if (source == null)
                throw new ArgumentNullException("source");

            return source.Aggregate(Math.Min, cancellationToken);
        }

        public static Task<double> Min(this IAsyncEnumerable<double> source, CancellationToken cancellationToken)
        {
            if (source == null)
                throw new ArgumentNullException("source");

            return source.Aggregate(Math.Min, cancellationToken);
        }

        public static Task<float> Min(this IAsyncEnumerable<float> source, CancellationToken cancellationToken)
        {
            if (source == null)
                throw new ArgumentNullException("source");

            return source.Aggregate(Math.Min, cancellationToken);
        }

        public static Task<decimal> Min(this IAsyncEnumerable<decimal> source, CancellationToken cancellationToken)
        {
            if (source == null)
                throw new ArgumentNullException("source");

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
                throw new ArgumentNullException("source");

            return source.Aggregate(default(int?), NullableMin, cancellationToken);
        }

        public static Task<long?> Min(this IAsyncEnumerable<long?> source, CancellationToken cancellationToken)
        {
            if (source == null)
                throw new ArgumentNullException("source");

            return source.Aggregate(default(long?), NullableMin, cancellationToken);
        }

        public static Task<double?> Min(this IAsyncEnumerable<double?> source, CancellationToken cancellationToken)
        {
            if (source == null)
                throw new ArgumentNullException("source");

            return source.Aggregate(default(double?), NullableMin, cancellationToken);
        }

        public static Task<float?> Min(this IAsyncEnumerable<float?> source, CancellationToken cancellationToken)
        {
            if (source == null)
                throw new ArgumentNullException("source");

            return source.Aggregate(default(float?), NullableMin, cancellationToken);
        }

        public static Task<decimal?> Min(this IAsyncEnumerable<decimal?> source, CancellationToken cancellationToken)
        {
            if (source == null)
                throw new ArgumentNullException("source");

            return source.Aggregate(default(decimal?), NullableMin, cancellationToken);
        }

        public static Task<TSource> Min<TSource>(this IAsyncEnumerable<TSource> source, CancellationToken cancellationToken)
        {
            if (source == null)
                throw new ArgumentNullException("source");

            var comparer = Comparer<TSource>.Default;
            return source.Aggregate((x, y) => comparer.Compare(x, y) <= 0 ? x : y, cancellationToken);
        }

        public static Task<int> Min<TSource>(this IAsyncEnumerable<TSource> source, Func<TSource, int> selector, CancellationToken cancellationToken)
        {
            if (source == null)
                throw new ArgumentNullException("source");
            if (selector == null)
                throw new ArgumentNullException("selector");

            return source.Select(selector).Min(cancellationToken);
        }

        public static Task<long> Min<TSource>(this IAsyncEnumerable<TSource> source, Func<TSource, long> selector, CancellationToken cancellationToken)
        {
            if (source == null)
                throw new ArgumentNullException("source");
            if (selector == null)
                throw new ArgumentNullException("selector");

            return source.Select(selector).Min(cancellationToken);
        }

        public static Task<double> Min<TSource>(this IAsyncEnumerable<TSource> source, Func<TSource, double> selector, CancellationToken cancellationToken)
        {
            if (source == null)
                throw new ArgumentNullException("source");
            if (selector == null)
                throw new ArgumentNullException("selector");

            return source.Select(selector).Min(cancellationToken);
        }

        public static Task<float> Min<TSource>(this IAsyncEnumerable<TSource> source, Func<TSource, float> selector, CancellationToken cancellationToken)
        {
            if (source == null)
                throw new ArgumentNullException("source");
            if (selector == null)
                throw new ArgumentNullException("selector");

            return source.Select(selector).Min(cancellationToken);
        }

        public static Task<decimal> Min<TSource>(this IAsyncEnumerable<TSource> source, Func<TSource, decimal> selector, CancellationToken cancellationToken)
        {
            if (source == null)
                throw new ArgumentNullException("source");
            if (selector == null)
                throw new ArgumentNullException("selector");

            return source.Select(selector).Min(cancellationToken);
        }

        public static Task<int?> Min<TSource>(this IAsyncEnumerable<TSource> source, Func<TSource, int?> selector, CancellationToken cancellationToken)
        {
            if (source == null)
                throw new ArgumentNullException("source");
            if (selector == null)
                throw new ArgumentNullException("selector");

            return source.Select(selector).Min(cancellationToken);
        }

        public static Task<long?> Min<TSource>(this IAsyncEnumerable<TSource> source, Func<TSource, long?> selector, CancellationToken cancellationToken)
        {
            if (source == null)
                throw new ArgumentNullException("source");
            if (selector == null)
                throw new ArgumentNullException("selector");

            return source.Select(selector).Min(cancellationToken);
        }

        public static Task<double?> Min<TSource>(this IAsyncEnumerable<TSource> source, Func<TSource, double?> selector, CancellationToken cancellationToken)
        {
            if (source == null)
                throw new ArgumentNullException("source");
            if (selector == null)
                throw new ArgumentNullException("selector");

            return source.Select(selector).Min(cancellationToken);
        }

        public static Task<float?> Min<TSource>(this IAsyncEnumerable<TSource> source, Func<TSource, float?> selector, CancellationToken cancellationToken)
        {
            if (source == null)
                throw new ArgumentNullException("source");
            if (selector == null)
                throw new ArgumentNullException("selector");

            return source.Select(selector).Min(cancellationToken);
        }

        public static Task<decimal?> Min<TSource>(this IAsyncEnumerable<TSource> source, Func<TSource, decimal?> selector, CancellationToken cancellationToken)
        {
            if (source == null)
                throw new ArgumentNullException("source");
            if (selector == null)
                throw new ArgumentNullException("selector");

            return source.Select(selector).Min(cancellationToken);
        }

        public static Task<TResult> Min<TSource, TResult>(this IAsyncEnumerable<TSource> source, Func<TSource, TResult> selector, CancellationToken cancellationToken)
        {
            if (source == null)
                throw new ArgumentNullException("source");
            if (selector == null)
                throw new ArgumentNullException("selector");

            return source.Select(selector).Min(cancellationToken);
        }

        public static Task<int> Sum(this IAsyncEnumerable<int> source, CancellationToken cancellationToken)
        {
            if (source == null)
                throw new ArgumentNullException("source");

            return source.Aggregate(0, (x, y) => x + y, cancellationToken);
        }

        public static Task<long> Sum(this IAsyncEnumerable<long> source, CancellationToken cancellationToken)
        {
            if (source == null)
                throw new ArgumentNullException("source");

            return source.Aggregate(0L, (x, y) => x + y, cancellationToken);
        }

        public static Task<double> Sum(this IAsyncEnumerable<double> source, CancellationToken cancellationToken)
        {
            if (source == null)
                throw new ArgumentNullException("source");

            return source.Aggregate(0.0, (x, y) => x + y, cancellationToken);
        }

        public static Task<float> Sum(this IAsyncEnumerable<float> source, CancellationToken cancellationToken)
        {
            if (source == null)
                throw new ArgumentNullException("source");

            return source.Aggregate(0f, (x, y) => x + y, cancellationToken);
        }

        public static Task<decimal> Sum(this IAsyncEnumerable<decimal> source, CancellationToken cancellationToken)
        {
            if (source == null)
                throw new ArgumentNullException("source");

            return source.Aggregate(0m, (x, y) => x + y, cancellationToken);
        }

        public static Task<int?> Sum(this IAsyncEnumerable<int?> source, CancellationToken cancellationToken)
        {
            if (source == null)
                throw new ArgumentNullException("source");

            return source.Aggregate((int?)0, (x, y) => x + y.GetValueOrDefault(), cancellationToken);
        }

        public static Task<long?> Sum(this IAsyncEnumerable<long?> source, CancellationToken cancellationToken)
        {
            if (source == null)
                throw new ArgumentNullException("source");

            return source.Aggregate((long?)0, (x, y) => x + y.GetValueOrDefault(), cancellationToken);
        }

        public static Task<double?> Sum(this IAsyncEnumerable<double?> source, CancellationToken cancellationToken)
        {
            if (source == null)
                throw new ArgumentNullException("source");

            return source.Aggregate((double?)0, (x, y) => x + y.GetValueOrDefault(), cancellationToken);
        }

        public static Task<float?> Sum(this IAsyncEnumerable<float?> source, CancellationToken cancellationToken)
        {
            if (source == null)
                throw new ArgumentNullException("source");

            return source.Aggregate((float?)0, (x, y) => x + y.GetValueOrDefault(), cancellationToken);
        }

        public static Task<decimal?> Sum(this IAsyncEnumerable<decimal?> source, CancellationToken cancellationToken)
        {
            if (source == null)
                throw new ArgumentNullException("source");

            return source.Aggregate((decimal?)0, (x, y) => x + y.GetValueOrDefault(), cancellationToken);
        }

        public static Task<int> Sum<TSource>(this IAsyncEnumerable<TSource> source, Func<TSource, int> selector, CancellationToken cancellationToken)
        {
            if (source == null)
                throw new ArgumentNullException("source");
            if (selector == null)
                throw new ArgumentNullException("selector");

            return source.Select(selector).Sum(cancellationToken);
        }

        public static Task<long> Sum<TSource>(this IAsyncEnumerable<TSource> source, Func<TSource, long> selector, CancellationToken cancellationToken)
        {
            if (source == null)
                throw new ArgumentNullException("source");
            if (selector == null)
                throw new ArgumentNullException("selector");

            return source.Select(selector).Sum(cancellationToken);
        }

        public static Task<double> Sum<TSource>(this IAsyncEnumerable<TSource> source, Func<TSource, double> selector, CancellationToken cancellationToken)
        {
            if (source == null)
                throw new ArgumentNullException("source");
            if (selector == null)
                throw new ArgumentNullException("selector");

            return source.Select(selector).Sum(cancellationToken);
        }

        public static Task<float> Sum<TSource>(this IAsyncEnumerable<TSource> source, Func<TSource, float> selector, CancellationToken cancellationToken)
        {
            if (source == null)
                throw new ArgumentNullException("source");
            if (selector == null)
                throw new ArgumentNullException("selector");

            return source.Select(selector).Sum(cancellationToken);
        }

        public static Task<decimal> Sum<TSource>(this IAsyncEnumerable<TSource> source, Func<TSource, decimal> selector, CancellationToken cancellationToken)
        {
            if (source == null)
                throw new ArgumentNullException("source");
            if (selector == null)
                throw new ArgumentNullException("selector");

            return source.Select(selector).Sum(cancellationToken);
        }

        public static Task<int?> Sum<TSource>(this IAsyncEnumerable<TSource> source, Func<TSource, int?> selector, CancellationToken cancellationToken)
        {
            if (source == null)
                throw new ArgumentNullException("source");
            if (selector == null)
                throw new ArgumentNullException("selector");

            return source.Select(selector).Sum(cancellationToken);
        }

        public static Task<long?> Sum<TSource>(this IAsyncEnumerable<TSource> source, Func<TSource, long?> selector, CancellationToken cancellationToken)
        {
            if (source == null)
                throw new ArgumentNullException("source");
            if (selector == null)
                throw new ArgumentNullException("selector");

            return source.Select(selector).Sum(cancellationToken);
        }

        public static Task<double?> Sum<TSource>(this IAsyncEnumerable<TSource> source, Func<TSource, double?> selector, CancellationToken cancellationToken)
        {
            if (source == null)
                throw new ArgumentNullException("source");
            if (selector == null)
                throw new ArgumentNullException("selector");

            return source.Select(selector).Sum(cancellationToken);
        }

        public static Task<float?> Sum<TSource>(this IAsyncEnumerable<TSource> source, Func<TSource, float?> selector, CancellationToken cancellationToken)
        {
            if (source == null)
                throw new ArgumentNullException("source");
            if (selector == null)
                throw new ArgumentNullException("selector");

            return source.Select(selector).Sum(cancellationToken);
        }

        public static Task<decimal?> Sum<TSource>(this IAsyncEnumerable<TSource> source, Func<TSource, decimal?> selector, CancellationToken cancellationToken)
        {
            if (source == null)
                throw new ArgumentNullException("source");
            if (selector == null)
                throw new ArgumentNullException("selector");

            return source.Select(selector).Sum(cancellationToken);
        }

        public static Task<bool> IsEmpty<TSource>(this IAsyncEnumerable<TSource> source, CancellationToken cancellationToken)
        {
            if (source == null)
                throw new ArgumentNullException("source");

            return source.Any(cancellationToken).ContinueWith(t => !t.Result);
        }

        public static Task<TSource> Min<TSource>(this IAsyncEnumerable<TSource> source, IComparer<TSource> comparer, CancellationToken cancellationToken)
        {
            if (source == null)
                throw new ArgumentNullException("source");
            if (comparer == null)
                throw new ArgumentNullException("comparer");

            return MinBy(source, x => x, comparer, cancellationToken).ContinueWith(t => t.Result.First());
        }

        public static Task<IList<TSource>> MinBy<TSource, TKey>(this IAsyncEnumerable<TSource> source, Func<TSource, TKey> keySelector, CancellationToken cancellationToken)
        {
            if (source == null)
                throw new ArgumentNullException("source");
            if (keySelector == null)
                throw new ArgumentNullException("keySelector");

            return MinBy(source, keySelector, Comparer<TKey>.Default, cancellationToken);
        }

        public static Task<IList<TSource>> MinBy<TSource, TKey>(this IAsyncEnumerable<TSource> source, Func<TSource, TKey> keySelector, IComparer<TKey> comparer, CancellationToken cancellationToken)
        {
            if (source == null)
                throw new ArgumentNullException("source");
            if (keySelector == null)
                throw new ArgumentNullException("keySelector");
            if (comparer == null)
                throw new ArgumentNullException("comparer");

            return ExtremaBy(source, keySelector, (key, minValue) => -comparer.Compare(key, minValue), cancellationToken);
        }

        public static Task<TSource> Max<TSource>(this IAsyncEnumerable<TSource> source, IComparer<TSource> comparer, CancellationToken cancellationToken)
        {
            if (source == null)
                throw new ArgumentNullException("source");
            if (comparer == null)
                throw new ArgumentNullException("comparer");

            return MaxBy(source, x => x, comparer, cancellationToken).ContinueWith(t => t.Result.First());
        }

        public static Task<IList<TSource>> MaxBy<TSource, TKey>(this IAsyncEnumerable<TSource> source, Func<TSource, TKey> keySelector, CancellationToken cancellationToken)
        {
            if (source == null)
                throw new ArgumentNullException("source");
            if (keySelector == null)
                throw new ArgumentNullException("keySelector");

            return MaxBy(source, keySelector, Comparer<TKey>.Default, cancellationToken);
        }

        public static Task<IList<TSource>> MaxBy<TSource, TKey>(this IAsyncEnumerable<TSource> source, Func<TSource, TKey> keySelector, IComparer<TKey> comparer, CancellationToken cancellationToken)
        {
            if (source == null)
                throw new ArgumentNullException("source");
            if (keySelector == null)
                throw new ArgumentNullException("keySelector");
            if (comparer == null)
                throw new ArgumentNullException("comparer");

            return ExtremaBy(source, keySelector, (key, minValue) => comparer.Compare(key, minValue), cancellationToken);
        }

        private static Task<IList<TSource>> ExtremaBy<TSource, TKey>(IAsyncEnumerable<TSource> source, Func<TSource, TKey> keySelector, Func<TKey, TKey, int> compare, CancellationToken cancellationToken)
        {
            var tcs = new TaskCompletionSource<IList<TSource>>();

            var result = new List<TSource>(); 
            
            var hasFirst = false;
            var current = default(TSource);
            var resKey = default(TKey);

            var e = source.GetEnumerator();

            var f = default(Action<CancellationToken>);
            f = ct => e.MoveNext(ct).ContinueWith(t =>
            {
                t.Handle(tcs, res =>
                {
                    if (!hasFirst)
                    {
                        if (!res)
                        {
                            tcs.TrySetException(new InvalidOperationException("Source sequence doesn't contain any elements."));
                            return;
                        }

                        current = e.Current;

                        try
                        {
                            resKey = keySelector(current);
                        }
                        catch (Exception ex)
                        {
                            tcs.TrySetException(ex);
                            return;
                        }

                        result.Add(current);

                        hasFirst = true;
                        f(ct);
                    }
                    else
                    {
                        if (res)
                        {
                            var key = default(TKey);
                            var cmp = default(int);

                            try
                            {
                                current = e.Current;
                                key = keySelector(current);
                                cmp = compare(key, resKey);
                            }
                            catch (Exception ex)
                            {
                                tcs.TrySetException(ex);
                                return;
                            }

                            if (cmp == 0)
                            {
                                result.Add(current);
                            }
                            else if (cmp > 0)
                            {
                                result = new List<TSource> { current };
                                resKey = key;
                            }

                            f(ct);
                        }
                        else
                        {
                            tcs.TrySetResult(result);
                        }
                    }
                });
            });

            f(cancellationToken);

            return tcs.Task.Finally(e.Dispose);
        }
    }
}

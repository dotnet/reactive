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
        public static IAsyncEnumerable<TSource> Concat<TSource>(this IAsyncEnumerable<TSource> first, IAsyncEnumerable<TSource> second)
        {
            if (first == null)
                throw new ArgumentNullException("first");
            if (second == null)
                throw new ArgumentNullException("second");

            return Create(() =>
            {
                var switched = false;
                var e = first.GetEnumerator();

                var cts = new CancellationTokenDisposable();
                var a = new AssignableDisposable { Disposable = e };
                var d = new CompositeDisposable(cts, a);

                var f = default(Action<TaskCompletionSource<bool>, CancellationToken>);
                f = (tcs, ct) => e.MoveNext(ct).ContinueWith(t =>
                {
                    t.Handle(tcs, res =>
                    {
                        if (res)
                        {
                            tcs.TrySetResult(true);
                        }
                        else
                        {
                            if (switched)
                            {
                                tcs.TrySetResult(false);
                            }
                            else
                            {
                                switched = true;

                                e = second.GetEnumerator();
                                a.Disposable = e;

                                f(tcs, ct);
                            }
                        }
                    });
                });

                return Create(
                    (ct, tcs) =>
                    {
                        f(tcs, cts.Token);
                        return tcs.Task.UsingEnumerator(a);
                    },
                    () => e.Current,
                    d.Dispose
                );
            });
        }

        public static IAsyncEnumerable<TResult> Zip<TFirst, TSecond, TResult>(this IAsyncEnumerable<TFirst> first, IAsyncEnumerable<TSecond> second, Func<TFirst, TSecond, TResult> selector)
        {
            if (first == null)
                throw new ArgumentNullException("first");
            if (second == null)
                throw new ArgumentNullException("second");
            if (selector == null)
                throw new ArgumentNullException("selector");

            return Create(() =>
            {
                var e1 = first.GetEnumerator();
                var e2 = second.GetEnumerator();
                var current = default(TResult);

                var cts = new CancellationTokenDisposable();
                var d = new CompositeDisposable(cts, e1, e2);

                return Create(
                    (ct, tcs) =>
                    {
                        e1.MoveNext(cts.Token).Zip(e2.MoveNext(cts.Token), (f, s) =>
                        {
                            var result = f && s;
                            if (result)
                                current = selector(e1.Current, e2.Current);
                            return result;
                        }).ContinueWith(t =>
                        {
                            t.Handle(tcs, x => tcs.TrySetResult(x));
                        });

                        return tcs.Task.UsingEnumerator(e1).UsingEnumerator(e2);
                    },
                    () => current,
                    d.Dispose
                );
            });
        }

        public static IAsyncEnumerable<TSource> Except<TSource>(this IAsyncEnumerable<TSource> first, IAsyncEnumerable<TSource> second, IEqualityComparer<TSource> comparer)
        {
            if (first == null)
                throw new ArgumentNullException("first");
            if (second == null)
                throw new ArgumentNullException("second");
            if (comparer == null)
                throw new ArgumentNullException("comparer");

            return Create(() =>
            {
                var e = first.GetEnumerator();

                var cts = new CancellationTokenDisposable();
                var d = new CompositeDisposable(cts, e);

                var mapTask = default(Task<Dictionary<TSource, TSource>>);
                var getMapTask = new Func<CancellationToken, Task<Dictionary<TSource, TSource>>>(ct =>
                {
                    if (mapTask == null)
                        mapTask = second.ToDictionary(x => x, comparer, ct);
                    return mapTask;
                });

                var f = default(Action<TaskCompletionSource<bool>, CancellationToken>);
                f = (tcs, ct) =>
                {
                    e.MoveNext(ct).Zip(getMapTask(ct), (b, _) => b).ContinueWith(t =>
                    {
                        t.Handle(tcs, res =>
                        {
                            if (res)
                            {
                                if (!mapTask.Result.ContainsKey(e.Current))
                                    tcs.TrySetResult(true);
                                else
                                    f(tcs, ct);
                            }
                            else
                                tcs.TrySetResult(false);
                        });
                    });
                };

                return Create(
                    (ct, tcs) =>
                    {
                        f(tcs, cts.Token);
                        return tcs.Task.UsingEnumerator(e);
                    },
                    () => e.Current,
                    d.Dispose
                );
            });
        }

        public static IAsyncEnumerable<TSource> Except<TSource>(this IAsyncEnumerable<TSource> first, IAsyncEnumerable<TSource> second)
        {
            if (first == null)
                throw new ArgumentNullException("first");
            if (second == null)
                throw new ArgumentNullException("second");

            return first.Except(second, EqualityComparer<TSource>.Default);
        }

        public static IAsyncEnumerable<TSource> Intersect<TSource>(this IAsyncEnumerable<TSource> first, IAsyncEnumerable<TSource> second, IEqualityComparer<TSource> comparer)
        {
            if (first == null)
                throw new ArgumentNullException("first");
            if (second == null)
                throw new ArgumentNullException("second");
            if (comparer == null)
                throw new ArgumentNullException("comparer");

            return Create(() =>
            {
                var e = first.GetEnumerator();

                var cts = new CancellationTokenDisposable();
                var d = new CompositeDisposable(cts, e);

                var mapTask = default(Task<Dictionary<TSource, TSource>>);
                var getMapTask = new Func<CancellationToken, Task<Dictionary<TSource, TSource>>>(ct =>
                {
                    if (mapTask == null)
                        mapTask = second.ToDictionary(x => x, comparer, ct);
                    return mapTask;
                });

                var f = default(Action<TaskCompletionSource<bool>, CancellationToken>);
                f = (tcs, ct) =>
                {
                    e.MoveNext(ct).Zip(getMapTask(ct), (b, _) => b).ContinueWith(t =>
                    {
                        t.Handle(tcs, res =>
                        {
                            if (res)
                            {
                                if (mapTask.Result.ContainsKey(e.Current))
                                    tcs.TrySetResult(true);
                                else
                                    f(tcs, ct);
                            }
                            else
                                tcs.TrySetResult(false);
                        });
                    });
                };

                return Create(
                    (ct, tcs) =>
                    {
                        f(tcs, cts.Token);
                        return tcs.Task.UsingEnumerator(e);
                    },
                    () => e.Current,
                    d.Dispose
                );
            });
        }

        public static IAsyncEnumerable<TSource> Intersect<TSource>(this IAsyncEnumerable<TSource> first, IAsyncEnumerable<TSource> second)
        {
            if (first == null)
                throw new ArgumentNullException("first");
            if (second == null)
                throw new ArgumentNullException("second");

            return first.Intersect(second, EqualityComparer<TSource>.Default);
        }

        public static IAsyncEnumerable<TSource> Union<TSource>(this IAsyncEnumerable<TSource> first, IAsyncEnumerable<TSource> second, IEqualityComparer<TSource> comparer)
        {
            if (first == null)
                throw new ArgumentNullException("first");
            if (second == null)
                throw new ArgumentNullException("second");
            if (comparer == null)
                throw new ArgumentNullException("comparer");

            return first.Concat(second).Distinct(comparer);
        }

        public static IAsyncEnumerable<TSource> Union<TSource>(this IAsyncEnumerable<TSource> first, IAsyncEnumerable<TSource> second)
        {
            if (first == null)
                throw new ArgumentNullException("first");
            if (second == null)
                throw new ArgumentNullException("second");

            return first.Union(second, EqualityComparer<TSource>.Default);
        }

        public static Task<bool> SequenceEqual<TSource>(this IAsyncEnumerable<TSource> first, IAsyncEnumerable<TSource> second, IEqualityComparer<TSource> comparer, CancellationToken cancellationToken)
        {
            if (first == null)
                throw new ArgumentNullException("first");
            if (second == null)
                throw new ArgumentNullException("second");
            if (comparer == null)
                throw new ArgumentNullException("comparer");

            var tcs = new TaskCompletionSource<bool>();

            var e1 = first.GetEnumerator();
            var e2 = second.GetEnumerator();

            var run = default(Action<CancellationToken>);
            run = ct =>
            {
                e1.MoveNext(ct).Zip(e2.MoveNext(ct), (f, s) =>
                {
                    if (f ^ s)
                    {
                        tcs.TrySetResult(false);
                        return false;
                    }

                    if (f && s)
                    {
                        var eq = default(bool);
                        try
                        {
                            eq = comparer.Equals(e1.Current, e2.Current);
                        }
                        catch (Exception ex)
                        {
                            tcs.TrySetException(ex);
                            return false;
                        }

                        if (!eq)
                        {
                            tcs.TrySetResult(false);
                            return false;
                        }
                        else
                            return true;
                    }
                    else
                    {
                        tcs.TrySetResult(true);
                        return false;
                    }
                }).ContinueWith(t =>
                {
                    t.Handle(tcs, res =>
                    {
                        if (res)
                            run(ct);
                    });
                });
            };

            run(cancellationToken);

            return tcs.Task.Finally(() =>
            {
                e1.Dispose();
                e2.Dispose();
            });
        }

        public static Task<bool> SequenceEqual<TSource>(this IAsyncEnumerable<TSource> first, IAsyncEnumerable<TSource> second, CancellationToken cancellationToken)
        {
            if (first == null)
                throw new ArgumentNullException("first");
            if (second == null)
                throw new ArgumentNullException("second");

            return first.SequenceEqual(second, EqualityComparer<TSource>.Default, cancellationToken);
        }

        public static IAsyncEnumerable<TResult> GroupJoin<TOuter, TInner, TKey, TResult>(this IAsyncEnumerable<TOuter> outer, IAsyncEnumerable<TInner> inner, Func<TOuter, TKey> outerKeySelector, Func<TInner, TKey> innerKeySelector, Func<TOuter, IAsyncEnumerable<TInner>, TResult> resultSelector, IEqualityComparer<TKey> comparer)
        {
            if (outer == null)
                throw new ArgumentNullException("outer");
            if (inner == null)
                throw new ArgumentNullException("inner");
            if (outerKeySelector == null)
                throw new ArgumentNullException("outerKeySelector");
            if (innerKeySelector == null)
                throw new ArgumentNullException("innerKeySelector");
            if (resultSelector == null)
                throw new ArgumentNullException("resultSelector");
            if (comparer == null)
                throw new ArgumentNullException("comparer");

            return Create(() =>
            {
                var innerMap = default(Task<ILookup<TKey, TInner>>);
                var getInnerMap = new Func<CancellationToken, Task<ILookup<TKey, TInner>>>(ct =>
                {
                    if (innerMap == null)
                        innerMap = inner.ToLookup(innerKeySelector, comparer, ct);

                    return innerMap;
                });

                var outerE = outer.GetEnumerator();
                var current = default(TResult);

                var cts = new CancellationTokenDisposable();
                var d = new CompositeDisposable(cts, outerE);

                var f = default(Action<TaskCompletionSource<bool>, CancellationToken>);
                f = (tcs, ct) =>
                {
                    getInnerMap(ct).ContinueWith(ti =>
                    {
                        ti.Handle(tcs, map =>
                        {
                            outerE.MoveNext(ct).ContinueWith(to =>
                            {
                                to.Handle(tcs, res =>
                                {
                                    if (res)
                                    {
                                        var element = outerE.Current;
                                        var key = default(TKey);

                                        try
                                        {
                                            key = outerKeySelector(element);
                                        }
                                        catch (Exception ex)
                                        {
                                            tcs.TrySetException(ex);
                                            return;
                                        }

                                        var innerE = default(IAsyncEnumerable<TInner>);
                                        if (!map.Contains(key))
                                            innerE = AsyncEnumerable.Empty<TInner>();
                                        else
                                            innerE = map[key].ToAsyncEnumerable();

                                        try
                                        {
                                            current = resultSelector(element, innerE);
                                        }
                                        catch (Exception ex)
                                        {
                                            tcs.TrySetException(ex);
                                            return;
                                        }

                                        tcs.TrySetResult(true);
                                    }
                                    else
                                    {
                                        tcs.TrySetResult(false);
                                    }
                                });
                            });
                        });
                    });
                };

                return Create(
                    (ct, tcs) =>
                    {
                        f(tcs, cts.Token);
                        return tcs.Task.UsingEnumerator(outerE);
                    },
                    () => current,
                    d.Dispose
                );
            });
        }

        public static IAsyncEnumerable<TResult> GroupJoin<TOuter, TInner, TKey, TResult>(this IAsyncEnumerable<TOuter> outer, IAsyncEnumerable<TInner> inner, Func<TOuter, TKey> outerKeySelector, Func<TInner, TKey> innerKeySelector, Func<TOuter, IAsyncEnumerable<TInner>, TResult> resultSelector)
        {
            if (outer == null)
                throw new ArgumentNullException("outer");
            if (inner == null)
                throw new ArgumentNullException("inner");
            if (outerKeySelector == null)
                throw new ArgumentNullException("outerKeySelector");
            if (innerKeySelector == null)
                throw new ArgumentNullException("innerKeySelector");
            if (resultSelector == null)
                throw new ArgumentNullException("resultSelector");

            return outer.GroupJoin(inner, outerKeySelector, innerKeySelector, resultSelector, EqualityComparer<TKey>.Default);
        }

        public static IAsyncEnumerable<TResult> Join<TOuter, TInner, TKey, TResult>(this IAsyncEnumerable<TOuter> outer, IAsyncEnumerable<TInner> inner, Func<TOuter, TKey> outerKeySelector, Func<TInner, TKey> innerKeySelector, Func<TOuter, TInner, TResult> resultSelector, IEqualityComparer<TKey> comparer)
        {
            if (outer == null)
                throw new ArgumentNullException("outer");
            if (inner == null)
                throw new ArgumentNullException("inner");
            if (outerKeySelector == null)
                throw new ArgumentNullException("outerKeySelector");
            if (innerKeySelector == null)
                throw new ArgumentNullException("innerKeySelector");
            if (resultSelector == null)
                throw new ArgumentNullException("resultSelector");
            if (comparer == null)
                throw new ArgumentNullException("comparer");

            return Create(() =>
            {
                var oe = outer.GetEnumerator();
                var ie = inner.GetEnumerator();

                var cts = new CancellationTokenDisposable();
                var d = new CompositeDisposable(cts, oe, ie);

                var current = default(TResult);
                var useOuter = true;
                var outerMap = new Dictionary<TKey, List<TOuter>>(comparer);
                var innerMap = new Dictionary<TKey, List<TInner>>(comparer);
                var q = new Queue<TResult>();

                var gate = new object();

                var f = default(Action<TaskCompletionSource<bool>, CancellationToken>);
                f = (tcs, ct) =>
                {
                    if (q.Count > 0)
                    {
                        current = q.Dequeue();
                        tcs.TrySetResult(true);
                        return;
                    }

                    var b = useOuter;
                    if (ie == null && oe == null)
                    {
                        tcs.TrySetResult(false);
                        return;
                    }
                    else if (ie == null)
                        b = true;
                    else if (oe == null)
                        b = false;
                    useOuter = !useOuter;

                    var enqueue = new Func<TOuter, TInner, bool>((o, i) =>
                    {
                        var result = default(TResult);
                        try
                        {
                            result = resultSelector(o, i);
                        }
                        catch (Exception exception)
                        {
                            tcs.TrySetException(exception);
                            return false;
                        }

                        q.Enqueue(result);
                        return true;
                    });

                    if (b)
                        oe.MoveNext(ct).ContinueWith(t =>
                        {
                            t.Handle(tcs, res =>
                            {
                                if (res)
                                {
                                    var element = oe.Current;
                                    var key = default(TKey);

                                    try
                                    {
                                        key = outerKeySelector(element);
                                    }
                                    catch (Exception exception)
                                    {
                                        tcs.TrySetException(exception);
                                        return;
                                    }

                                    var outerList = default(List<TOuter>);
                                    if (!outerMap.TryGetValue(key, out outerList))
                                    {
                                        outerList = new List<TOuter>();
                                        outerMap.Add(key, outerList);
                                    }

                                    outerList.Add(element);

                                    var innerList = default(List<TInner>);
                                    if (!innerMap.TryGetValue(key, out innerList))
                                    {
                                        innerList = new List<TInner>();
                                        innerMap.Add(key, innerList);
                                    }

                                    foreach (var v in innerList)
                                    {
                                        if (!enqueue(element, v))
                                            return;
                                    }
                                    
                                    f(tcs, ct);
                                }
                                else
                                {
                                    oe.Dispose();
                                    oe = null;
                                    f(tcs, ct);
                                }
                            });
                        });
                    else
                        ie.MoveNext(ct).ContinueWith(t =>
                        {
                            t.Handle(tcs, res =>
                            {
                                if (res)
                                {
                                    var element = ie.Current;
                                    var key = default(TKey);

                                    try
                                    {
                                        key = innerKeySelector(element);
                                    }
                                    catch (Exception exception)
                                    {
                                        tcs.TrySetException(exception);
                                        return;
                                    }

                                    var innerList = default(List<TInner>);
                                    if (!innerMap.TryGetValue(key, out innerList))
                                    {
                                        innerList = new List<TInner>();
                                        innerMap.Add(key, innerList);
                                    }

                                    innerList.Add(element);

                                    var outerList = default(List<TOuter>);
                                    if (!outerMap.TryGetValue(key, out outerList))
                                    {
                                        outerList = new List<TOuter>();
                                        outerMap.Add(key, outerList);
                                    }

                                    foreach (var v in outerList)
                                    {
                                        if (!enqueue(v, element))
                                            return;
                                    }

                                    f(tcs, ct);
                                }
                                else
                                {
                                    ie.Dispose();
                                    ie = null;
                                    f(tcs, ct);
                                }
                            });
                        });
                };

                return Create(
                    (ct, tcs) =>
                    {
                        f(tcs, cts.Token);
                        return tcs.Task.UsingEnumerator(oe).UsingEnumerator(ie);
                    },
                    () => current,
                    d.Dispose
                );
            });
        }

        public static IAsyncEnumerable<TResult> Join<TOuter, TInner, TKey, TResult>(this IAsyncEnumerable<TOuter> outer, IAsyncEnumerable<TInner> inner, Func<TOuter, TKey> outerKeySelector, Func<TInner, TKey> innerKeySelector, Func<TOuter, TInner, TResult> resultSelector)
        {
            if (outer == null)
                throw new ArgumentNullException("outer");
            if (inner == null)
                throw new ArgumentNullException("inner");
            if (outerKeySelector == null)
                throw new ArgumentNullException("outerKeySelector");
            if (innerKeySelector == null)
                throw new ArgumentNullException("innerKeySelector");
            if (resultSelector == null)
                throw new ArgumentNullException("resultSelector");

            return outer.Join(inner, outerKeySelector, innerKeySelector, resultSelector, EqualityComparer<TKey>.Default);
        }

        public static IAsyncEnumerable<TSource> Concat<TSource>(this IEnumerable<IAsyncEnumerable<TSource>> sources)
        {
            if (sources == null)
                throw new ArgumentNullException("sources");

            return sources.Concat_();
        }

        public static IAsyncEnumerable<TSource> Concat<TSource>(params IAsyncEnumerable<TSource>[] sources)
        {
            if (sources == null)
                throw new ArgumentNullException("sources");

            return sources.Concat_();
        }

        private static IAsyncEnumerable<TSource> Concat_<TSource>(this IEnumerable<IAsyncEnumerable<TSource>> sources)
        {
            return Create(() =>
            {
                var se = sources.GetEnumerator();
                var e = default(IAsyncEnumerator<TSource>);

                var cts = new CancellationTokenDisposable();
                var a = new AssignableDisposable();
                var d = new CompositeDisposable(cts, se, a);

                var f = default(Action<TaskCompletionSource<bool>, CancellationToken>);
                f = (tcs, ct) =>
                {
                    if (e == null)
                    {
                        var b = false;
                        try
                        {
                            b = se.MoveNext();
                            if (b)
                                e = se.Current.GetEnumerator();
                        }
                        catch (Exception ex)
                        {
                            tcs.TrySetException(ex);
                            return;
                        }

                        if (!b)
                        {
                            tcs.TrySetResult(false);
                            return;
                        }

                        a.Disposable = e;
                    }

                    e.MoveNext(ct).ContinueWith(t =>
                    {
                        t.Handle(tcs, res =>
                        {
                            if (res)
                            {
                                tcs.TrySetResult(true);
                            }
                            else
                            {
                                e.Dispose();
                                e = null;

                                f(tcs, ct);
                            }
                        });
                    });
                };

                return Create(
                    (ct, tcs) =>
                    {
                        f(tcs, cts.Token);
                        return tcs.Task.UsingEnumerator(a);
                    },
                    () => e.Current,
                    d.Dispose
                );
            });
        }

        public static IAsyncEnumerable<TOther> SelectMany<TSource, TOther>(this IAsyncEnumerable<TSource> source, IAsyncEnumerable<TOther> other)
        {
            if (source == null)
                throw new ArgumentNullException("source");
            if (other == null)
                throw new ArgumentNullException("other");

            return source.SelectMany(_ => other);
        }
    }
}

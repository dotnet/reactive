// Copyright (c) Microsoft Open Technologies, Inc. All rights reserved. See License.txt in the project root for license information.
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace System.Linq
{
    public static partial class AsyncEnumerable
    {
        public static IAsyncEnumerable<TResult> Select<TSource, TResult>(this IAsyncEnumerable<TSource> source, Func<TSource, TResult> selector)
        {
            if (source == null)
                throw new ArgumentNullException("source");
            if (selector == null)
                throw new ArgumentNullException("selector");

            return Create(() =>
            {
                var e = source.GetEnumerator();
                var current = default(TResult);

                var cts = new CancellationTokenDisposable();
                var d = new CompositeDisposable(cts, e);

                return Create(
                    (ct, tcs) =>
                    {
                        e.MoveNext(cts.Token).ContinueWith(t =>
                        {
                            t.Handle(tcs, res =>
                            {
                                if (res)
                                {
                                    try
                                    {
                                        current = selector(e.Current);
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
                        
                        return tcs.Task.UsingEnumerator(e);
                    },
                    () => current,
                    d.Dispose
                );
            });
        }

        public static IAsyncEnumerable<TResult> Select<TSource, TResult>(this IAsyncEnumerable<TSource> source, Func<TSource, int, TResult> selector)
        {
            if (source == null)
                throw new ArgumentNullException("source");
            if (selector == null)
                throw new ArgumentNullException("selector");

            return Create(() =>
            {
                var e = source.GetEnumerator();
                var current = default(TResult);
                var index = 0;

                var cts = new CancellationTokenDisposable();
                var d = new CompositeDisposable(cts, e);

                return Create(
                    (ct, tcs) =>
                    {
                        e.MoveNext(cts.Token).ContinueWith(t =>
                        {
                            t.Handle(tcs, res =>
                            {
                                if (res)
                                {
                                    try
                                    {
                                        current = selector(e.Current, index++);
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

                        return tcs.Task.UsingEnumerator(e);
                    },
                    () => current,
                    d.Dispose
                );
            });
        }

        public static IAsyncEnumerable<TSource> AsAsyncEnumerable<TSource>(this IAsyncEnumerable<TSource> source)
        {
            if (source == null)
                throw new ArgumentNullException("source");

            return source.Select(x => x);
        }

        public static IAsyncEnumerable<TSource> Where<TSource>(this IAsyncEnumerable<TSource> source, Func<TSource, bool> predicate)
        {
            if (source == null)
                throw new ArgumentNullException("source");
            if (predicate == null)
                throw new ArgumentNullException("predicate");

            return Create(() =>
            {
                var e = source.GetEnumerator();

                var cts = new CancellationTokenDisposable();
                var d = new CompositeDisposable(cts, e);

                var f = default(Action<TaskCompletionSource<bool>, CancellationToken>);
                f = (tcs, ct) =>
                {
                    e.MoveNext(ct).ContinueWith(t =>
                    {
                        t.Handle(tcs, res =>
                        {
                            if (res)
                            {
                                var b = false;
                                try
                                {
                                    b = predicate(e.Current);
                                }
                                catch (Exception ex)
                                {
                                    tcs.TrySetException(ex);
                                    return;
                                }

                                if (b)
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

        public static IAsyncEnumerable<TSource> Where<TSource>(this IAsyncEnumerable<TSource> source, Func<TSource, int, bool> predicate)
        {
            if (source == null)
                throw new ArgumentNullException("source");
            if (predicate == null)
                throw new ArgumentNullException("predicate");

            return Create(() =>
            {
                var e = source.GetEnumerator();
                var index = 0;

                var cts = new CancellationTokenDisposable();
                var d = new CompositeDisposable(cts, e);

                var f = default(Action<TaskCompletionSource<bool>, CancellationToken>);
                f = (tcs, ct) =>
                {
                    e.MoveNext(ct).ContinueWith(t =>
                    {
                        t.Handle(tcs, res =>
                        {
                            if (res)
                            {
                                var b = false;
                                try
                                {
                                    b = predicate(e.Current, index++);
                                }
                                catch (Exception ex)
                                {
                                    tcs.TrySetException(ex);
                                    return;
                                }

                                if (b)
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

        public static IAsyncEnumerable<TResult> SelectMany<TSource, TResult>(this IAsyncEnumerable<TSource> source, Func<TSource, IAsyncEnumerable<TResult>> selector)
        {
            if (source == null)
                throw new ArgumentNullException("source");
            if (selector == null)
                throw new ArgumentNullException("selector");

            return Create(() =>
            {
                var e = source.GetEnumerator();
                var ie = default(IAsyncEnumerator<TResult>);

                var cts = new CancellationTokenDisposable();
                var d = new CompositeDisposable(cts, e);

                var outer = default(Action<TaskCompletionSource<bool>, CancellationToken>);
                var inner = default(Action<TaskCompletionSource<bool>, CancellationToken>);

                inner = (tcs, ct) =>
                {
                    ie.MoveNext(ct).ContinueWith(t =>
                    {
                        t.Handle(tcs, res =>
                        {
                            if (res)
                            {
                                tcs.TrySetResult(true);
                            }
                            else
                            {
                                ie = null;
                                outer(tcs, ct);
                            }
                        });
                    });
                };

                outer = (tcs, ct) =>
                {
                    e.MoveNext(ct).ContinueWith(t =>
                    {
                        t.Handle(tcs, res =>
                        {
                            if (res)
                            {
                                try
                                {
                                    ie = selector(e.Current).GetEnumerator();
                                    inner(tcs, ct);
                                }
                                catch (Exception ex)
                                {
                                    tcs.TrySetException(ex);
                                }
                            }
                            else
                                tcs.TrySetResult(false);
                        });
                    });
                };

                return Create(
                    (ct, tcs) =>
                    {
                        if (ie == null)
                            outer(tcs, cts.Token);
                        else
                            inner(tcs, cts.Token);

                        return tcs.Task.UsingEnumerator(e);
                    },
                    () => ie.Current,
                    d.Dispose
                );
            });
        }

        public static IAsyncEnumerable<TResult> SelectMany<TSource, TResult>(this IAsyncEnumerable<TSource> source, Func<TSource, int, IAsyncEnumerable<TResult>> selector)
        {
            if (source == null)
                throw new ArgumentNullException("source");
            if (selector == null)
                throw new ArgumentNullException("selector");

            return Create(() =>
            {
                var e = source.GetEnumerator();
                var ie = default(IAsyncEnumerator<TResult>);
                var index = 0;

                var cts = new CancellationTokenDisposable();
                var d = new CompositeDisposable(cts, e);

                var outer = default(Action<TaskCompletionSource<bool>, CancellationToken>);
                var inner = default(Action<TaskCompletionSource<bool>, CancellationToken>);

                inner = (tcs, ct) =>
                {
                    ie.MoveNext(ct).ContinueWith(t =>
                    {
                        t.Handle(tcs, res =>
                        {
                            if (res)
                            {
                                tcs.TrySetResult(true);
                            }
                            else
                            {
                                ie = null;
                                outer(tcs, ct);
                            }
                        });
                    });
                };

                outer = (tcs, ct) =>
                {
                    e.MoveNext(ct).ContinueWith(t =>
                    {
                        t.Handle(tcs, res =>
                        {
                            if (res)
                            {
                                try
                                {
                                    ie = selector(e.Current, index++).GetEnumerator();
                                    inner(tcs, ct);
                                }
                                catch (Exception ex)
                                {
                                    tcs.TrySetException(ex);
                                }
                            }
                            else
                                tcs.TrySetResult(false);
                        });
                    });
                };

                return Create(
                    (ct, tcs) =>
                    {
                        if (ie == null)
                            outer(tcs, cts.Token);
                        else
                            inner(tcs, cts.Token);

                        return tcs.Task.UsingEnumerator(e);
                    },
                    () => ie.Current,
                    d.Dispose
                );
            });
        }

        public static IAsyncEnumerable<TResult> SelectMany<TSource, TCollection, TResult>(this IAsyncEnumerable<TSource> source, Func<TSource, IAsyncEnumerable<TCollection>> selector, Func<TSource, TCollection, TResult> resultSelector)
        {
            if (source == null)
                throw new ArgumentNullException("source");
            if (selector == null)
                throw new ArgumentNullException("selector");
            if (resultSelector == null)
                throw new ArgumentNullException("resultSelector");

            return source.SelectMany(x => selector(x).Select(y => resultSelector(x, y)));
        }

        public static IAsyncEnumerable<TResult> SelectMany<TSource, TCollection, TResult>(this IAsyncEnumerable<TSource> source, Func<TSource, int, IAsyncEnumerable<TCollection>> selector, Func<TSource, TCollection, TResult> resultSelector)
        {
            if (source == null)
                throw new ArgumentNullException("source");
            if (selector == null)
                throw new ArgumentNullException("selector");
            if (resultSelector == null)
                throw new ArgumentNullException("resultSelector");

            return source.SelectMany((x, i) => selector(x, i).Select(y => resultSelector(x, y)));
        }

        public static IAsyncEnumerable<TType> OfType<TType>(this IAsyncEnumerable<object> source)
        {
            if (source == null)
                throw new ArgumentNullException("source");

            return source.Where(x => x is TType).Cast<TType>();
        }

        public static IAsyncEnumerable<TResult> Cast<TResult>(this IAsyncEnumerable<object> source)
        {
            if (source == null)
                throw new ArgumentNullException("source");

            return source.Select(x => (TResult)x);
        }

        public static IAsyncEnumerable<TSource> Take<TSource>(this IAsyncEnumerable<TSource> source, int count)
        {
            if (source == null)
                throw new ArgumentNullException("source");
            if (count < 0)
                throw new ArgumentOutOfRangeException("count");

            return Create(() =>
            {
                var e = source.GetEnumerator();
                var n = count;

                var cts = new CancellationTokenDisposable();
                var d = new CompositeDisposable(cts, e);

                return Create(
                    (ct, tcs) =>
                    {
                        if (n == 0)
                            return TaskExt.Return(false, cts.Token);

                        e.MoveNext(cts.Token).ContinueWith(t =>
                        {
                            t.Handle(tcs, res =>
                            {
                                --n;
                                tcs.TrySetResult(res);
                            });
                        });

                        return tcs.Task.UsingEnumerator(e);
                    },
                    () => e.Current,
                    d.Dispose
                );
            });
        }

        public static IAsyncEnumerable<TSource> TakeWhile<TSource>(this IAsyncEnumerable<TSource> source, Func<TSource, bool> predicate)
        {
            if (source == null)
                throw new ArgumentNullException("source");
            if (predicate == null)
                throw new ArgumentNullException("predicate");

            return Create(() =>
            {
                var e = source.GetEnumerator();

                var cts = new CancellationTokenDisposable();
                var d = new CompositeDisposable(cts, e);

                return Create(
                    (ct, tcs) =>
                    {
                        e.MoveNext(cts.Token).ContinueWith(t =>
                        {
                            t.Handle(tcs, res =>
                            {
                                if (res)
                                {
                                    var b = false;

                                    try
                                    {
                                        b = predicate(e.Current);
                                    }
                                    catch (Exception ex)
                                    {
                                        tcs.TrySetException(ex);
                                        return;
                                    }

                                    if (b)
                                    {
                                        tcs.TrySetResult(true);
                                        return;
                                    }
                                }
                                tcs.TrySetResult(false);
                            });
                        });

                        return tcs.Task.UsingEnumerator(e);
                    },
                    () => e.Current,
                    d.Dispose
                );
            });
        }

        public static IAsyncEnumerable<TSource> TakeWhile<TSource>(this IAsyncEnumerable<TSource> source, Func<TSource, int, bool> predicate)
        {
            if (source == null)
                throw new ArgumentNullException("source");
            if (predicate == null)
                throw new ArgumentNullException("predicate");

            return Create(() =>
            {
                var e = source.GetEnumerator();
                var index = 0;

                var cts = new CancellationTokenDisposable();
                var d = new CompositeDisposable(cts, e);

                return Create(
                    (ct, tcs) =>
                    {
                        e.MoveNext(cts.Token).ContinueWith(t =>
                        {
                            t.Handle(tcs, res =>
                            {
                                if (res)
                                {
                                    var b = false;

                                    try
                                    {
                                        b = predicate(e.Current, index++);
                                    }
                                    catch (Exception ex)
                                    {
                                        tcs.TrySetException(ex);
                                        return;
                                    }

                                    if (b)
                                    {
                                        tcs.TrySetResult(true);
                                        return;
                                    }
                                }
                                tcs.TrySetResult(false);
                            });
                        });

                        return tcs.Task.UsingEnumerator(e);
                    },
                    () => e.Current,
                    d.Dispose
                );
            });
        }

        public static IAsyncEnumerable<TSource> Skip<TSource>(this IAsyncEnumerable<TSource> source, int count)
        {
            if (source == null)
                throw new ArgumentNullException("source");
            if (count < 0)
                throw new ArgumentOutOfRangeException("count");

            return Create(() =>
            {
                var e = source.GetEnumerator();
                var n = count;

                var cts = new CancellationTokenDisposable();
                var d = new CompositeDisposable(cts, e);

                var f = default(Action<TaskCompletionSource<bool>, CancellationToken>);
                f = (tcs, ct) =>
                {
                    if (n == 0)
                        e.MoveNext(ct).ContinueWith(t =>
                        {
                            t.Handle(tcs, x => tcs.TrySetResult(x));
                        });
                    else
                    {
                        --n;
                        e.MoveNext(ct).ContinueWith(t =>
                        {
                            t.Handle(tcs, res =>
                            {
                                if (!res)
                                    tcs.TrySetResult(false);
                                else
                                    f(tcs, ct);
                            });
                        });
                    }
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

        public static IAsyncEnumerable<TSource> SkipWhile<TSource>(this IAsyncEnumerable<TSource> source, Func<TSource, bool> predicate)
        {
            if (source == null)
                throw new ArgumentNullException("source");
            if (predicate == null)
                throw new ArgumentNullException("predicate");

            return Create(() =>
            {
                var e = source.GetEnumerator();
                var skipping = true;

                var cts = new CancellationTokenDisposable();
                var d = new CompositeDisposable(cts, e);

                var f = default(Action<TaskCompletionSource<bool>, CancellationToken>);
                f = (tcs, ct) =>
                {
                    if (skipping)
                        e.MoveNext(ct).ContinueWith(t =>
                        {
                            t.Handle(tcs, res =>
                            {
                                if (res)
                                {
                                    var result = false;
                                    try
                                    {
                                        result = predicate(e.Current);
                                    }
                                    catch (Exception ex)
                                    {
                                        tcs.TrySetException(ex);
                                        return;
                                    }
                                    if (result)
                                        f(tcs, ct);
                                    else
                                    {
                                        skipping = false;
                                        tcs.TrySetResult(true);
                                    }
                                }
                                else
                                    tcs.TrySetResult(false);
                            });
                        });
                    else
                        e.MoveNext(ct).ContinueWith(t =>
                        {
                            t.Handle(tcs, x => tcs.TrySetResult(x));
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

        public static IAsyncEnumerable<TSource> SkipWhile<TSource>(this IAsyncEnumerable<TSource> source, Func<TSource, int, bool> predicate)
        {
            if (source == null)
                throw new ArgumentNullException("source");
            if (predicate == null)
                throw new ArgumentNullException("predicate");

            return Create(() =>
            {
                var e = source.GetEnumerator();
                var skipping = true;
                var index = 0;

                var cts = new CancellationTokenDisposable();
                var d = new CompositeDisposable(cts, e);

                var f = default(Action<TaskCompletionSource<bool>, CancellationToken>);
                f = (tcs, ct) =>
                {
                    if (skipping)
                        e.MoveNext(ct).ContinueWith(t =>
                        {
                            t.Handle(tcs, res =>
                            {
                                if (res)
                                {
                                    var result = false;
                                    try
                                    {
                                        result = predicate(e.Current, index++);
                                    }
                                    catch (Exception ex)
                                    {
                                        tcs.TrySetException(ex);
                                        return;
                                    }
                                    if (result)
                                        f(tcs, ct);
                                    else
                                    {
                                        skipping = false;
                                        tcs.TrySetResult(true);
                                    }
                                }
                                else
                                    tcs.TrySetResult(false);
                            });
                        });
                    else
                        e.MoveNext(ct).ContinueWith(t =>
                        {
                            t.Handle(tcs, x => tcs.TrySetResult(x));
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

        public static IAsyncEnumerable<TSource> DefaultIfEmpty<TSource>(this IAsyncEnumerable<TSource> source, TSource defaultValue)
        {
            if (source == null)
                throw new ArgumentNullException("source");

            return Create(() =>
            {
                var done = false;
                var hasElements = false;
                var e = source.GetEnumerator();
                var current = default(TSource);

                var cts = new CancellationTokenDisposable();
                var d = new CompositeDisposable(cts, e);

                var f = default(Action<TaskCompletionSource<bool>, CancellationToken>);
                f = (tcs, ct) =>
                {
                    if (done)
                        tcs.TrySetResult(false);
                    else
                        e.MoveNext(ct).ContinueWith(t =>
                        {
                            t.Handle(tcs, res =>
                            {
                                if (res)
                                {
                                    hasElements = true;
                                    current = e.Current;
                                    tcs.TrySetResult(true);
                                }
                                else
                                {
                                    done = true;

                                    if (!hasElements)
                                    {
                                        current = defaultValue;
                                        tcs.TrySetResult(true);
                                    }
                                    else
                                        tcs.TrySetResult(false);
                                }
                            });
                        });
                };

                return Create(
                    (ct, tcs) =>
                    {
                        f(tcs, cts.Token);
                        return tcs.Task.UsingEnumerator(e);
                    },
                    () => current,
                    d.Dispose
                );
            });
        }

        public static IAsyncEnumerable<TSource> DefaultIfEmpty<TSource>(this IAsyncEnumerable<TSource> source)
        {
            if (source == null)
                throw new ArgumentNullException("source");

            return source.DefaultIfEmpty(default(TSource));
        }

        public static IAsyncEnumerable<TSource> Distinct<TSource>(this IAsyncEnumerable<TSource> source, IEqualityComparer<TSource> comparer)
        {
            if (source == null)
                throw new ArgumentNullException("source");
            if (comparer == null)
                throw new ArgumentNullException("comparer");

            return Defer(() =>
            {
                var set = new HashSet<TSource>(comparer);
                return source.Where(set.Add);
            });
        }

        public static IAsyncEnumerable<TSource> Distinct<TSource>(this IAsyncEnumerable<TSource> source)
        {
            if (source == null)
                throw new ArgumentNullException("source");

            return source.Distinct(EqualityComparer<TSource>.Default);
        }

        public static IAsyncEnumerable<TSource> Reverse<TSource>(this IAsyncEnumerable<TSource> source)
        {
            if (source == null)
                throw new ArgumentNullException("source");

            return Create(() =>
            {
                var e = source.GetEnumerator();
                var stack = default(Stack<TSource>);

                var cts = new CancellationTokenDisposable();
                var d = new CompositeDisposable(cts, e);

                return Create(
                    (ct, tcs) =>
                    {
                        if (stack == null)
                        {
                            Create(() => e).Aggregate(new Stack<TSource>(), (s, x) => { s.Push(x); return s; }, cts.Token).ContinueWith(t =>
                            {
                                t.Handle(tcs, res =>
                                {
                                    stack = res;
                                    tcs.TrySetResult(stack.Count > 0);
                                });
                            }, cts.Token);
                        }
                        else
                        {
                            stack.Pop();
                            tcs.TrySetResult(stack.Count > 0);
                        }

                        return tcs.Task.UsingEnumerator(e);
                    },
                    () => stack.Peek(),
                    d.Dispose
                );
            });
        }

        public static IOrderedAsyncEnumerable<TSource> OrderBy<TSource, TKey>(this IAsyncEnumerable<TSource> source, Func<TSource, TKey> keySelector, IComparer<TKey> comparer)
        {
            if (source == null)
                throw new ArgumentNullException("source");
            if (keySelector == null)
                throw new ArgumentNullException("keySelector");
            if (comparer == null)
                throw new ArgumentNullException("comparer");

            return new OrderedAsyncEnumerable<TSource, TKey>(
                Create(() =>
                {
                    var current = default(IEnumerable<TSource>);

                    return Create(
                        ct =>
                        {
                            var tcs = new TaskCompletionSource<bool>();
                            if (current == null)
                            {
                                source.ToList(ct).ContinueWith(t =>
                                {
                                    t.Handle(tcs, res =>
                                    {
                                        current = res;
                                        tcs.TrySetResult(true);
                                    });
                                });
                            }
                            else
                                tcs.TrySetResult(false);
                            return tcs.Task;
                        },
                        () => current,
                        () => { }
                    );
                }),
                keySelector,
                comparer
            );
        }

        public static IOrderedAsyncEnumerable<TSource> OrderBy<TSource, TKey>(this IAsyncEnumerable<TSource> source, Func<TSource, TKey> keySelector)
        {
            if (source == null)
                throw new ArgumentNullException("source");
            if (keySelector == null)
                throw new ArgumentNullException("keySelector");

            return source.OrderBy(keySelector, Comparer<TKey>.Default);
        }

        public static IOrderedAsyncEnumerable<TSource> OrderByDescending<TSource, TKey>(this IAsyncEnumerable<TSource> source, Func<TSource, TKey> keySelector, IComparer<TKey> comparer)
        {
            if (source == null)
                throw new ArgumentNullException("source");
            if (keySelector == null)
                throw new ArgumentNullException("keySelector");
            if (comparer == null)
                throw new ArgumentNullException("comparer");

            return source.OrderBy(keySelector, new ReverseComparer<TKey>(comparer));
        }

        public static IOrderedAsyncEnumerable<TSource> OrderByDescending<TSource, TKey>(this IAsyncEnumerable<TSource> source, Func<TSource, TKey> keySelector)
        {
            if (source == null)
                throw new ArgumentNullException("source");
            if (keySelector == null)
                throw new ArgumentNullException("keySelector");

            return source.OrderByDescending(keySelector, Comparer<TKey>.Default);
        }

        public static IOrderedAsyncEnumerable<TSource> ThenBy<TSource, TKey>(this IOrderedAsyncEnumerable<TSource> source, Func<TSource, TKey> keySelector)
        {
            if (source == null)
                throw new ArgumentNullException("source");
            if (keySelector == null)
                throw new ArgumentNullException("keySelector");

            return source.ThenBy(keySelector, Comparer<TKey>.Default);
        }

        public static IOrderedAsyncEnumerable<TSource> ThenBy<TSource, TKey>(this IOrderedAsyncEnumerable<TSource> source, Func<TSource, TKey> keySelector, IComparer<TKey> comparer)
        {
            if (source == null)
                throw new ArgumentNullException("source");
            if (keySelector == null)
                throw new ArgumentNullException("keySelector");
            if (comparer == null)
                throw new ArgumentNullException("comparer");

            return source.CreateOrderedEnumerable(keySelector, comparer, false);
        }

        public static IOrderedAsyncEnumerable<TSource> ThenByDescending<TSource, TKey>(this IOrderedAsyncEnumerable<TSource> source, Func<TSource, TKey> keySelector)
        {
            if (source == null)
                throw new ArgumentNullException("source");
            if (keySelector == null)
                throw new ArgumentNullException("keySelector");

            return source.ThenByDescending(keySelector, Comparer<TKey>.Default);
        }

        public static IOrderedAsyncEnumerable<TSource> ThenByDescending<TSource, TKey>(this IOrderedAsyncEnumerable<TSource> source, Func<TSource, TKey> keySelector, IComparer<TKey> comparer)
        {
            if (source == null)
                throw new ArgumentNullException("source");
            if (keySelector == null)
                throw new ArgumentNullException("keySelector");
            if (comparer == null)
                throw new ArgumentNullException("comparer");

            return source.CreateOrderedEnumerable(keySelector, comparer, true);
        }

        static IEnumerable<IGrouping<TKey, TElement>> GroupUntil<TSource, TKey, TElement>(this IEnumerable<TSource> source, Func<TSource, TKey> keySelector, Func<TSource, TElement> elementSelector, IComparer<TKey> comparer)
        {
            var group = default(EnumerableGrouping<TKey, TElement>);
            foreach (var x in source)
            {
                var key = keySelector(x);
                if (group == null || comparer.Compare(group.Key, key) != 0)
                {
                    group = new EnumerableGrouping<TKey, TElement>(key);
                    yield return group;
                }
                group.Add(elementSelector(x));
            }
        }

        class OrderedAsyncEnumerable<T, K> : IOrderedAsyncEnumerable<T>
        {
            private readonly IAsyncEnumerable<IEnumerable<T>> equivalenceClasses;
            private readonly Func<T, K> keySelector;
            private readonly IComparer<K> comparer;

            public OrderedAsyncEnumerable(IAsyncEnumerable<IEnumerable<T>> equivalenceClasses, Func<T, K> keySelector, IComparer<K> comparer)
            {
                this.equivalenceClasses = equivalenceClasses;
                this.keySelector = keySelector;
                this.comparer = comparer;
            }

            public IOrderedAsyncEnumerable<T> CreateOrderedEnumerable<TKey>(Func<T, TKey> keySelector, IComparer<TKey> comparer, bool descending)
            {
                if (descending)
                    comparer = new ReverseComparer<TKey>(comparer);

                return new OrderedAsyncEnumerable<T, TKey>(Classes(), keySelector, comparer);
            }

            IAsyncEnumerable<IEnumerable<T>> Classes()
            {
                return Create(() =>
                {
                    var e = equivalenceClasses.GetEnumerator();
                    var list = new List<IEnumerable<T>>();
                    var e1 = default(IEnumerator<IEnumerable<T>>);

                    var cts = new CancellationTokenDisposable();
                    var d1 = new AssignableDisposable();
                    var d = new CompositeDisposable(cts, e, d1);

                    var f = default(Action<TaskCompletionSource<bool>, CancellationToken>);
                    var g = default(Action<TaskCompletionSource<bool>, CancellationToken>);

                    f = (tcs, ct) =>
                    {
                        e.MoveNext(ct).ContinueWith(t =>
                        {
                            t.Handle(tcs, res =>
                            {
                                if (res)
                                {
                                    try
                                    {
                                        foreach (var group in e.Current.OrderBy(keySelector, comparer).GroupUntil(keySelector, x => x, comparer))
                                            list.Add(group);
                                        f(tcs, ct);
                                    }
                                    catch (Exception exception)
                                    {
                                        tcs.TrySetException(exception);
                                        return;
                                    }
                                }
                                else
                                {
                                    e.Dispose();

                                    e1 = list.GetEnumerator();
                                    d1.Disposable = e1;

                                    g(tcs, ct);
                                }
                            });
                        });
                    };

                    g = (tcs, ct) =>
                    {
                        var res = false;
                        try
                        {
                            res = e1.MoveNext();
                        }
                        catch (Exception ex)
                        {
                            tcs.TrySetException(ex);
                            return;
                        }

                        tcs.TrySetResult(res);
                    };

                    return Create(
                        (ct, tcs) =>
                        {
                            if (e1 != null)
                            {
                                g(tcs, cts.Token);
                                return tcs.Task.UsingEnumerator(e1);
                            }
                            else
                            {
                                f(tcs, cts.Token);
                                return tcs.Task.UsingEnumerator(e);
                            }
                        },
                        () => e1.Current,
                        d.Dispose
                    );
                });
            }

            public IAsyncEnumerator<T> GetEnumerator()
            {
                return Classes().SelectMany(x => x.ToAsyncEnumerable()).GetEnumerator();
            }
        }

        class ReverseComparer<T> : IComparer<T>
        {
            IComparer<T> comparer;

            public ReverseComparer(IComparer<T> comparer)
            {
                this.comparer = comparer;
            }

            public int Compare(T x, T y)
            {
                return -comparer.Compare(x, y);
            }
        }

        public static IAsyncEnumerable<IAsyncGrouping<TKey, TElement>> GroupBy<TSource, TKey, TElement>(this IAsyncEnumerable<TSource> source, Func<TSource, TKey> keySelector, Func<TSource, TElement> elementSelector, IEqualityComparer<TKey> comparer)
        {
            if (source == null)
                throw new ArgumentNullException("source");
            if (keySelector == null)
                throw new ArgumentNullException("keySelector");
            if (elementSelector == null)
                throw new ArgumentNullException("elementSelector");
            if (comparer == null)
                throw new ArgumentNullException("comparer");

            return Create(() =>
            {
                var gate = new object();
                
                var e = source.GetEnumerator();
                var count = 1;

                var map = new Dictionary<TKey, Grouping<TKey, TElement>>(comparer);
                var list = new List<IAsyncGrouping<TKey, TElement>>();

                var index = 0;

                var current = default(IAsyncGrouping<TKey, TElement>);
                var faulted = default(Exception);

                var task = default(Task<bool>);

                var cts = new CancellationTokenDisposable();
                var refCount = new Disposable(
                    () =>
                    {
                        if (Interlocked.Decrement(ref count) == 0)
                            e.Dispose();
                    }
                );
                var d = new CompositeDisposable(cts, refCount);

                var iterateSource = default(Func<CancellationToken, Task<bool>>);
                iterateSource = ct =>
                {
                    var tcs = default(TaskCompletionSource<bool>);
                    lock (gate)
                    {
                        if (task != null)
                        {
                            return task;
                        }
                        else
                        {
                            tcs = new TaskCompletionSource<bool>();
                            task = tcs.Task.UsingEnumerator(e);
                        }
                    }

                    if (faulted != null)
                    {
                        tcs.TrySetException(faulted);
                        return task;
                    }

                    e.MoveNext(ct).ContinueWith(t =>
                    {
                        t.Handle(tcs,
                            res =>
                            {
                                if (res)
                                {
                                    var key = default(TKey);
                                    var element = default(TElement);

                                    var cur = e.Current;
                                    try
                                    {
                                        key = keySelector(cur);
                                        element = elementSelector(cur);
                                    }
                                    catch (Exception exception)
                                    {
                                        foreach (var v in map.Values)
                                            v.Error(exception);

                                        tcs.TrySetException(exception);
                                        return;
                                    }

                                    var group = default(Grouping<TKey, TElement>);
                                    if (!map.TryGetValue(key, out group))
                                    {
                                        group = new Grouping<TKey, TElement>(key, iterateSource, refCount);
                                        map.Add(key, group);
                                        lock (list)
                                            list.Add(group);

                                        Interlocked.Increment(ref count);
                                    }
                                    group.Add(element);
                                }

                                tcs.TrySetResult(res);
                            },
                            ex =>
                            {
                                foreach (var v in map.Values)
                                    v.Error(ex);

                                faulted = ex;
                                tcs.TrySetException(ex);
                            }
                        );

                        lock (gate)
                        {
                            task = null;
                        }
                    });

                    return tcs.Task.UsingEnumerator(e);
                };

                var f = default(Action<TaskCompletionSource<bool>, CancellationToken>);
                f = (tcs, ct) =>
                {
                    iterateSource(ct).ContinueWith(t =>
                    {
                        t.Handle(tcs,
                            res =>
                            {
                                current = null;
                                lock (list)
                                {
                                    if (index < list.Count)
                                        current = list[index++];
                                }

                                if (current != null)
                                {
                                    tcs.TrySetResult(true);
                                }
                                else
                                {
                                    if (res)
                                        f(tcs, ct);
                                    else
                                        tcs.TrySetResult(false);
                                }
                            }
                        );
                    });
                };

                return Create(
                    (ct, tcs) =>
                    {
                        f(tcs, cts.Token);
                        return tcs.Task;
                    },
                    () => current,
                    d.Dispose
                );
            });
        }

        public static IAsyncEnumerable<IAsyncGrouping<TKey, TElement>> GroupBy<TSource, TKey, TElement>(this IAsyncEnumerable<TSource> source, Func<TSource, TKey> keySelector, Func<TSource, TElement> elementSelector)
        {
            if (source == null)
                throw new ArgumentNullException("source");
            if (keySelector == null)
                throw new ArgumentNullException("keySelector");
            if (elementSelector == null)
                throw new ArgumentNullException("elementSelector");
            
            return source.GroupBy(keySelector, elementSelector, EqualityComparer<TKey>.Default);
        }

        public static IAsyncEnumerable<IAsyncGrouping<TKey, TSource>> GroupBy<TSource, TKey>(this IAsyncEnumerable<TSource> source, Func<TSource, TKey> keySelector, IEqualityComparer<TKey> comparer)
        {
            if (source == null)
                throw new ArgumentNullException("source");
            if (keySelector == null)
                throw new ArgumentNullException("keySelector");
            if (comparer == null)
                throw new ArgumentNullException("comparer");

            return source.GroupBy(keySelector, x => x, comparer);
        }

        public static IAsyncEnumerable<IAsyncGrouping<TKey, TSource>> GroupBy<TSource, TKey>(this IAsyncEnumerable<TSource> source, Func<TSource, TKey> keySelector)
        {
            if (source == null)
                throw new ArgumentNullException("source");
            if (keySelector == null)
                throw new ArgumentNullException("keySelector");
            
            return source.GroupBy(keySelector, x => x, EqualityComparer<TKey>.Default);
        }

        public static IAsyncEnumerable<TResult> GroupBy<TSource, TKey, TElement, TResult>(this IAsyncEnumerable<TSource> source, Func<TSource, TKey> keySelector, Func<TSource, TElement> elementSelector, Func<TKey, IAsyncEnumerable<TElement>, TResult> resultSelector, IEqualityComparer<TKey> comparer)
        {
            if (source == null)
                throw new ArgumentNullException("source");
            if (keySelector == null)
                throw new ArgumentNullException("keySelector");
            if (elementSelector == null)
                throw new ArgumentNullException("elementSelector");
            if (resultSelector == null)
                throw new ArgumentNullException("resultSelector");
            if (comparer == null)
                throw new ArgumentNullException("comparer");

            return source.GroupBy(keySelector, elementSelector, comparer).Select(g => resultSelector(g.Key, g));
        }

        public static IAsyncEnumerable<TResult> GroupBy<TSource, TKey, TElement, TResult>(this IAsyncEnumerable<TSource> source, Func<TSource, TKey> keySelector, Func<TSource, TElement> elementSelector, Func<TKey, IAsyncEnumerable<TElement>, TResult> resultSelector)
        {
            if (source == null)
                throw new ArgumentNullException("source");
            if (keySelector == null)
                throw new ArgumentNullException("keySelector");
            if (elementSelector == null)
                throw new ArgumentNullException("elementSelector");
            if (resultSelector == null)
                throw new ArgumentNullException("resultSelector");

            return source.GroupBy(keySelector, elementSelector, EqualityComparer<TKey>.Default).Select(g => resultSelector(g.Key, g));
        }

        public static IAsyncEnumerable<TResult> GroupBy<TSource, TKey, TResult>(this IAsyncEnumerable<TSource> source, Func<TSource, TKey> keySelector, Func<TKey, IAsyncEnumerable<TSource>, TResult> resultSelector, IEqualityComparer<TKey> comparer)
        {
            if (source == null)
                throw new ArgumentNullException("source");
            if (keySelector == null)
                throw new ArgumentNullException("keySelector");
            if (resultSelector == null)
                throw new ArgumentNullException("resultSelector");
            if (comparer == null)
                throw new ArgumentNullException("comparer");

            return source.GroupBy(keySelector, x => x, comparer).Select(g => resultSelector(g.Key, g));
        }

        public static IAsyncEnumerable<TResult> GroupBy<TSource, TKey, TResult>(this IAsyncEnumerable<TSource> source, Func<TSource, TKey> keySelector, Func<TKey, IAsyncEnumerable<TSource>, TResult> resultSelector)
        {
            if (source == null)
                throw new ArgumentNullException("source");
            if (keySelector == null)
                throw new ArgumentNullException("keySelector");
            if (resultSelector == null)
                throw new ArgumentNullException("resultSelector");
            
            return source.GroupBy(keySelector, x => x, EqualityComparer<TKey>.Default).Select(g => resultSelector(g.Key, g));
        }

        class Grouping<TKey, TElement> : IAsyncGrouping<TKey, TElement>
        {
            private readonly Func<CancellationToken, Task<bool>> iterateSource;
            private readonly IDisposable sourceDisposable;
            private readonly List<TElement> elements = new List<TElement>();
            private bool done = false;
            private Exception exception = null;

            public Grouping(TKey key, Func<CancellationToken, Task<bool>> iterateSource, IDisposable sourceDisposable)
            {
                this.iterateSource = iterateSource;
                this.sourceDisposable = sourceDisposable;
                Key = key;
            }

            public TKey Key
            {
                get;
                private set;
            }

            public void Add(TElement element)
            {
                lock (elements)
                    elements.Add(element);
            }

            public void Error(Exception exception)
            {
                done = true;
                this.exception = exception;
            }

            public IAsyncEnumerator<TElement> GetEnumerator()
            {
                var index = -1;

                var cts = new CancellationTokenDisposable();
                var d = new CompositeDisposable(cts, sourceDisposable);

                var f = default(Action<TaskCompletionSource<bool>, CancellationToken>);
                f = (tcs, ct) =>
                {
                    var size = 0;
                    lock (elements)
                        size = elements.Count;

                    if (index < size)
                    {
                        tcs.TrySetResult(true);
                    }
                    else if (done)
                    {
                        if (exception != null)
                            tcs.TrySetException(exception);
                        else
                            tcs.TrySetResult(false);
                    }
                    else
                    {
                        iterateSource(ct).ContinueWith(t =>
                        {
                            t.Handle(tcs, res =>
                            {
                                if (res)
                                    f(tcs, ct);
                                else
                                    tcs.TrySetResult(false);
                            });
                        });
                    }
                };

                return Create(
                    (ct, tcs) =>
                    {
                        ++index;
                        f(tcs, cts.Token);
                        return tcs.Task;
                    },
                    () => elements[index],
                    d.Dispose
                );
            }
        }

        #region Ix

        public static IAsyncEnumerable<TSource> Do<TSource>(this IAsyncEnumerable<TSource> source, Action<TSource> onNext)
        {
            if (source == null)
                throw new ArgumentNullException("source");
            if (onNext == null)
                throw new ArgumentNullException("onNext");

            return DoHelper(source, onNext, _ => { }, () => { });
        }

        public static IAsyncEnumerable<TSource> Do<TSource>(this IAsyncEnumerable<TSource> source, Action<TSource> onNext, Action onCompleted)
        {
            if (source == null)
                throw new ArgumentNullException("source");
            if (onNext == null)
                throw new ArgumentNullException("onNext");
            if (onCompleted == null)
                throw new ArgumentNullException("onCompleted");

            return DoHelper(source, onNext, _ => { }, onCompleted);
        }

        public static IAsyncEnumerable<TSource> Do<TSource>(this IAsyncEnumerable<TSource> source, Action<TSource> onNext, Action<Exception> onError)
        {
            if (source == null)
                throw new ArgumentNullException("source");
            if (onNext == null)
                throw new ArgumentNullException("onNext");
            if (onError == null)
                throw new ArgumentNullException("onError");

            return DoHelper(source, onNext, onError, () => { });
        }

        public static IAsyncEnumerable<TSource> Do<TSource>(this IAsyncEnumerable<TSource> source, Action<TSource> onNext, Action<Exception> onError, Action onCompleted)
        {
            if (source == null)
                throw new ArgumentNullException("source");
            if (onNext == null)
                throw new ArgumentNullException("onNext");
            if (onError == null)
                throw new ArgumentNullException("onError");
            if (onCompleted == null)
                throw new ArgumentNullException("onCompleted");

            return DoHelper(source, onNext, onError, onCompleted);
        }

#if !NO_RXINTERFACES
        public static IAsyncEnumerable<TSource> Do<TSource>(this IAsyncEnumerable<TSource> source, IObserver<TSource> observer)
        {
            if (source == null)
                throw new ArgumentNullException("source");
            if (observer == null)
                throw new ArgumentNullException("observer");

            return DoHelper(source, observer.OnNext, observer.OnError, observer.OnCompleted);
        }
#endif

        private static IAsyncEnumerable<TSource> DoHelper<TSource>(this IAsyncEnumerable<TSource> source, Action<TSource> onNext, Action<Exception> onError, Action onCompleted)
        {
            return Create(() =>
            {
                var e = source.GetEnumerator();

                var cts = new CancellationTokenDisposable();
                var d = new CompositeDisposable(cts, e);

                var current = default(TSource);

                var f = default(Action<TaskCompletionSource<bool>, CancellationToken>);
                f = (tcs, ct) =>
                {
                    e.MoveNext(ct).ContinueWith(t =>
                    {
                        if (!t.IsCanceled)
                        {
                            try
                            {
                                if (t.IsFaulted)
                                {
                                    onError(t.Exception);
                                }
                                else if (!t.Result)
                                {
                                    onCompleted();
                                }
                                else
                                {
                                    current = e.Current;
                                    onNext(current);
                                }
                            }
                            catch (Exception ex)
                            {
                                tcs.TrySetException(ex);
                                return;
                            }
                        }

                        t.Handle(tcs, res =>
                        {
                            tcs.TrySetResult(res);
                        });
                    });
                };

                return Create(
                    (ct, tcs) =>
                    {
                        f(tcs, cts.Token);
                        return tcs.Task.UsingEnumerator(e);
                    },
                    () => current,
                    d.Dispose
                );
            });
        }

        public static void ForEach<TSource>(this IAsyncEnumerable<TSource> source, Action<TSource> action, CancellationToken cancellationToken)
        {
            if (source == null)
                throw new ArgumentNullException("source");
            if (action == null)
                throw new ArgumentNullException("action");

            source.ForEachAsync(action, cancellationToken).Wait(cancellationToken);
        }

        public static Task ForEachAsync<TSource>(this IAsyncEnumerable<TSource> source, Action<TSource> action, CancellationToken cancellationToken)
        {
            if (source == null)
                throw new ArgumentNullException("source");
            if (action == null)
                throw new ArgumentNullException("action");

            return source.ForEachAsync((x, i) => action(x), cancellationToken);
        }

        public static void ForEach<TSource>(this IAsyncEnumerable<TSource> source, Action<TSource, int> action, CancellationToken cancellationToken)
        {
            if (source == null)
                throw new ArgumentNullException("source");
            if (action == null)
                throw new ArgumentNullException("action");

            source.ForEachAsync(action, cancellationToken).Wait(cancellationToken);
        }

        public static Task ForEachAsync<TSource>(this IAsyncEnumerable<TSource> source, Action<TSource, int> action, CancellationToken cancellationToken)
        {
            if (source == null)
                throw new ArgumentNullException("source");
            if (action == null)
                throw new ArgumentNullException("action");

            var tcs = new TaskCompletionSource<bool>();

            var e = source.GetEnumerator();

            var i = 0;

            var f = default(Action<CancellationToken>);
            f = ct =>
            {
                e.MoveNext(ct).ContinueWith(t =>
                {
                    t.Handle(tcs, res =>
                    {
                        if (res)
                        {
                            try
                            {
                                action(e.Current, i++);
                            }
                            catch (Exception ex)
                            {
                                tcs.TrySetException(ex);
                                return;
                            }

                            f(ct);
                        }
                        else
                            tcs.TrySetResult(true);
                    });
                });
            };

            f(cancellationToken);

            return tcs.Task.UsingEnumerator(e);
        }

        public static IAsyncEnumerable<TSource> Repeat<TSource>(this IAsyncEnumerable<TSource> source, int count)
        {
            if (source == null)
                throw new ArgumentNullException("source");
            if (count < 0)
                throw new ArgumentOutOfRangeException("count");

            return Create(() =>
            {
                var e = default(IAsyncEnumerator<TSource>);
                var a = new AssignableDisposable();
                var n = count;
                var current = default(TSource);

                var cts = new CancellationTokenDisposable();
                var d = new CompositeDisposable(cts, a);

                var f = default(Action<TaskCompletionSource<bool>, CancellationToken>);
                f = (tcs, ct) =>
                {
                    if (e == null)
                    {
                        if (n-- == 0)
                        {
                            tcs.TrySetResult(false);
                            return;
                        }

                        try
                        {
                            e = source.GetEnumerator();
                        }
                        catch (Exception ex)
                        {
                            tcs.TrySetException(ex);
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
                                current = e.Current;
                                tcs.TrySetResult(true);
                            }
                            else
                            {
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
                        return tcs.Task.UsingEnumerator(d);
                    },
                    () => current,
                    d.Dispose
                );
            });
        }

        public static IAsyncEnumerable<TSource> Repeat<TSource>(this IAsyncEnumerable<TSource> source)
        {
            if (source == null)
                throw new ArgumentNullException("source");

            return Create(() =>
            {
                var e = default(IAsyncEnumerator<TSource>);
                var a = new AssignableDisposable();
                var current = default(TSource);

                var cts = new CancellationTokenDisposable();
                var d = new CompositeDisposable(cts, a);

                var f = default(Action<TaskCompletionSource<bool>, CancellationToken>);
                f = (tcs, ct) =>
                {
                    if (e == null)
                    {
                        try
                        {
                            e = source.GetEnumerator();
                        }
                        catch (Exception ex)
                        {
                            tcs.TrySetException(ex);
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
                                current = e.Current;
                                tcs.TrySetResult(true);
                            }
                            else
                            {
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
                        return tcs.Task.UsingEnumerator(d);
                    },
                    () => current,
                    d.Dispose
                );
            });
        }

        public static IAsyncEnumerable<TSource> IgnoreElements<TSource>(this IAsyncEnumerable<TSource> source)
        {
            if (source == null)
                throw new ArgumentNullException("source");

            return Create(() =>
            {
                var e = source.GetEnumerator();

                var cts = new CancellationTokenDisposable();
                var d = new CompositeDisposable(cts, e);

                var f = default(Action<TaskCompletionSource<bool>, CancellationToken>);
                f = (tcs, ct) =>
                {
                    e.MoveNext(ct).ContinueWith(t =>
                    {
                        t.Handle(tcs, res =>
                        {
                            if (!res)
                            {
                                tcs.TrySetResult(false);
                                return;
                            }

                            f(tcs, ct);
                        });
                    });
                };

                return Create<TSource>(
                    (ct, tcs) =>
                    {
                        f(tcs, cts.Token);
                        return tcs.Task.UsingEnumerator(e);
                    },
                    () => { throw new InvalidOperationException(); },
                    d.Dispose
                );
            });
        }

        public static IAsyncEnumerable<TSource> StartWith<TSource>(this IAsyncEnumerable<TSource> source, params TSource[] values)
        {
            if (source == null)
                throw new ArgumentNullException("source");

            return values.ToAsyncEnumerable().Concat(source);
        }

        public static IAsyncEnumerable<IList<TSource>> Buffer<TSource>(this IAsyncEnumerable<TSource> source, int count)
        {
            if (source == null)
                throw new ArgumentNullException("source");
            if (count <= 0)
                throw new ArgumentOutOfRangeException("count");

            return source.Buffer_(count, count);
        }

        public static IAsyncEnumerable<IList<TSource>> Buffer<TSource>(this IAsyncEnumerable<TSource> source, int count, int skip)
        {
            if (source == null)
                throw new ArgumentNullException("source");
            if (count <= 0)
                throw new ArgumentOutOfRangeException("count");
            if (skip <= 0)
                throw new ArgumentOutOfRangeException("skip");

            return source.Buffer_(count, skip);
        }

        private static IAsyncEnumerable<IList<TSource>> Buffer_<TSource>(this IAsyncEnumerable<TSource> source, int count, int skip)
        {
            return Create(() =>
            {
                var e = source.GetEnumerator();

                var cts = new CancellationTokenDisposable();
                var d = new CompositeDisposable(cts, e);

                var buffers = new Queue<IList<TSource>>();

                var i = 0;

                var current = default(IList<TSource>);
                var stopped = false;

                var f = default(Action<TaskCompletionSource<bool>, CancellationToken>);
                f = (tcs, ct) =>
                {
                    if (!stopped)
                    {
                        e.MoveNext(ct).ContinueWith(t =>
                        {
                            t.Handle(tcs, res =>
                            {
                                if (res)
                                {
                                    var item = e.Current;

                                    if (i++ % skip == 0)
                                        buffers.Enqueue(new List<TSource>(count));

                                    foreach (var buffer in buffers)
                                        buffer.Add(item);

                                    if (buffers.Count > 0 && buffers.Peek().Count == count)
                                    {
                                        current = buffers.Dequeue();
                                        tcs.TrySetResult(true);
                                        return;
                                    }

                                    f(tcs, ct);
                                }
                                else
                                {
                                    stopped = true;
                                    e.Dispose();

                                    f(tcs, ct);
                                }
                            });
                        });
                    }
                    else
                    {
                        if (buffers.Count > 0)
                        {
                            current = buffers.Dequeue();
                            tcs.TrySetResult(true);
                        }
                        else
                        {
                            tcs.TrySetResult(false);
                        }
                    }
                };

                return Create(
                    (ct, tcs) =>
                    {
                        f(tcs, cts.Token);
                        return tcs.Task.UsingEnumerator(e);
                    },
                    () => current,
                    d.Dispose
                );
            });
        }

        public static IAsyncEnumerable<TSource> Distinct<TSource, TKey>(this IAsyncEnumerable<TSource> source, Func<TSource, TKey> keySelector, IEqualityComparer<TKey> comparer)
        {
            if (source == null)
                throw new ArgumentNullException("source");
            if (keySelector == null)
                throw new ArgumentNullException("keySelector");
            if (comparer == null)
                throw new ArgumentNullException("comparer");

            return Defer(() =>
            {
                var set = new HashSet<TKey>(comparer);
                return source.Where(item => set.Add(keySelector(item)));
            });
        }

        public static IAsyncEnumerable<TSource> Distinct<TSource, TKey>(this IAsyncEnumerable<TSource> source, Func<TSource, TKey> keySelector)
        {
            if (source == null)
                throw new ArgumentNullException("source");
            if (keySelector == null)
                throw new ArgumentNullException("keySelector");

            return source.Distinct(keySelector, EqualityComparer<TKey>.Default);
        }

        public static IAsyncEnumerable<TSource> DistinctUntilChanged<TSource>(this IAsyncEnumerable<TSource> source)
        {
            if (source == null)
                throw new ArgumentNullException("source");

            return source.DistinctUntilChanged_(x => x, EqualityComparer<TSource>.Default);
        }

        public static IAsyncEnumerable<TSource> DistinctUntilChanged<TSource>(this IAsyncEnumerable<TSource> source, IEqualityComparer<TSource> comparer)
        {
            if (source == null)
                throw new ArgumentNullException("source");
            if (comparer == null)
                throw new ArgumentNullException("comparer");

            return source.DistinctUntilChanged_(x => x, comparer);
        }

        public static IAsyncEnumerable<TSource> DistinctUntilChanged<TSource, TKey>(this IAsyncEnumerable<TSource> source, Func<TSource, TKey> keySelector)
        {
            if (source == null)
                throw new ArgumentNullException("source");
            if (keySelector == null)
                throw new ArgumentNullException("keySelector");

            return source.DistinctUntilChanged_(keySelector, EqualityComparer<TKey>.Default);
        }

        public static IAsyncEnumerable<TSource> DistinctUntilChanged<TSource, TKey>(this IAsyncEnumerable<TSource> source, Func<TSource, TKey> keySelector, IEqualityComparer<TKey> comparer)
        {
            if (source == null)
                throw new ArgumentNullException("source");
            if (keySelector == null)
                throw new ArgumentNullException("keySelector");
            if (comparer == null)
                throw new ArgumentNullException("comparer");

            return source.DistinctUntilChanged_(keySelector, comparer);
        }

        private static IAsyncEnumerable<TSource> DistinctUntilChanged_<TSource, TKey>(this IAsyncEnumerable<TSource> source, Func<TSource, TKey> keySelector, IEqualityComparer<TKey> comparer)
        {
            return Create(() =>
            {
                var e = source.GetEnumerator();

                var cts = new CancellationTokenDisposable();
                var d = new CompositeDisposable(cts, e);

                var currentKey = default(TKey);
                var hasCurrentKey = false;
                var current = default(TSource);

                var f = default(Action<TaskCompletionSource<bool>, CancellationToken>);
                f = (tcs, ct) =>
                {
                    e.MoveNext(ct).ContinueWith(t =>
                    {
                        t.Handle(tcs, res =>
                        {
                            if (res)
                            {
                                var item = e.Current;
                                var key = default(TKey);
                                var comparerEquals = false;

                                try
                                {
                                    key = keySelector(item);

                                    if (hasCurrentKey)
                                    {
                                        comparerEquals = comparer.Equals(currentKey, key);
                                    }
                                }
                                catch (Exception ex)
                                {
                                    tcs.TrySetException(ex);
                                    return;
                                }

                                if (!hasCurrentKey || !comparerEquals)
                                {
                                    hasCurrentKey = true;
                                    currentKey = key;

                                    current = item;
                                    tcs.TrySetResult(true);
                                }
                                else
                                {
                                    f(tcs, ct);
                                }
                            }
                            else
                            {
                                tcs.TrySetResult(false);
                            }
                        });
                    });
                };

                return Create(
                    (ct, tcs) =>
                    {
                        f(tcs, cts.Token);
                        return tcs.Task.UsingEnumerator(e);
                    },
                    () => current,
                    d.Dispose
                );
            });
        }

        public static IAsyncEnumerable<TSource> Expand<TSource>(this IAsyncEnumerable<TSource> source, Func<TSource, IAsyncEnumerable<TSource>> selector)
        {
            if (source == null)
                throw new ArgumentNullException("source");
            if (selector == null)
                throw new ArgumentNullException("selector");

            return Create(() =>
            {
                var e = default(IAsyncEnumerator<TSource>);

                var cts = new CancellationTokenDisposable();
                var a = new AssignableDisposable();
                var d = new CompositeDisposable(cts, a);

                var queue = new Queue<IAsyncEnumerable<TSource>>();
                queue.Enqueue(source);

                var current = default(TSource);

                var f = default(Action<TaskCompletionSource<bool>, CancellationToken>);
                f = (tcs, ct) =>
                {
                    if (e == null)
                    {
                        if (queue.Count > 0)
                        {
                            var src = queue.Dequeue();

                            try
                            {
                                e = src.GetEnumerator();
                            }
                            catch (Exception ex)
                            {
                                tcs.TrySetException(ex);
                                return;
                            }

                            a.Disposable = e;
                            f(tcs, ct);
                        }
                        else
                        {
                            tcs.TrySetResult(false);
                        }
                    }
                    else
                    {
                        e.MoveNext(ct).ContinueWith(t =>
                        {
                            t.Handle(tcs, res =>
                            {
                                if (res)
                                {
                                    var item = e.Current;

                                    var next = default(IAsyncEnumerable<TSource>);
                                    try
                                    {
                                        next = selector(item);
                                    }
                                    catch (Exception ex)
                                    {
                                        tcs.TrySetException(ex);
                                        return;
                                    }

                                    queue.Enqueue(next);
                                    current = item;
                                    tcs.TrySetResult(true);
                                }
                                else
                                {
                                    e = null;
                                    f(tcs, ct);
                                }
                            });
                        });
                    }
                };

                return Create(
                    (ct, tcs) =>
                    {
                        f(tcs, cts.Token);
                        return tcs.Task.UsingEnumerator(a);
                    },
                    () => current,
                    d.Dispose
                );
            });
        }

        public static IAsyncEnumerable<TAccumulate> Scan<TSource, TAccumulate>(this IAsyncEnumerable<TSource> source, TAccumulate seed, Func<TAccumulate, TSource, TAccumulate> accumulator)
        {
            if (source == null)
                throw new ArgumentNullException("source");
            if (accumulator == null)
                throw new ArgumentNullException("accumulator");

            return Create(() =>
            {
                var e = source.GetEnumerator();

                var cts = new CancellationTokenDisposable();
                var d = new CompositeDisposable(cts, e);

                var acc = seed;
                var current = default(TAccumulate);

                var f = default(Action<TaskCompletionSource<bool>, CancellationToken>);
                f = (tcs, ct) =>
                {
                    e.MoveNext(ct).ContinueWith(t =>
                    {
                        t.Handle(tcs, res =>
                        {
                            if (!res)
                            {
                                tcs.TrySetResult(false);
                                return;
                            }

                            var item = e.Current;
                            try
                            {
                                acc = accumulator(acc, item);
                            }
                            catch (Exception ex)
                            {
                                tcs.TrySetException(ex);
                                return;
                            }

                            current = acc;
                            tcs.TrySetResult(true);
                        });
                    });
                };

                return Create(
                    (ct, tcs) =>
                    {
                        f(tcs, cts.Token);
                        return tcs.Task.UsingEnumerator(e);
                    },
                    () => current,
                    d.Dispose
                );
            });
        }

        public static IAsyncEnumerable<TSource> Scan<TSource>(this IAsyncEnumerable<TSource> source, Func<TSource, TSource, TSource> accumulator)
        {
            if (source == null)
                throw new ArgumentNullException("source");
            if (accumulator == null)
                throw new ArgumentNullException("accumulator");

            return Create(() =>
            {
                var e = source.GetEnumerator();

                var cts = new CancellationTokenDisposable();
                var d = new CompositeDisposable(cts, e);

                var hasSeed = false;
                var acc = default(TSource);
                var current = default(TSource);

                var f = default(Action<TaskCompletionSource<bool>, CancellationToken>);
                f = (tcs, ct) =>
                {
                    e.MoveNext(ct).ContinueWith(t =>
                    {
                        t.Handle(tcs, res =>
                        {
                            if (!res)
                            {
                                tcs.TrySetResult(false);
                                return;
                            }

                            var item = e.Current;

                            if (!hasSeed)
                            {
                                hasSeed = true;
                                acc = item;
                                f(tcs, ct);
                                return;
                            }

                            try
                            {
                                acc = accumulator(acc, item);
                            }
                            catch (Exception ex)
                            {
                                tcs.TrySetException(ex);
                                return;
                            }

                            current = acc;
                            tcs.TrySetResult(true);
                        });
                    });
                };

                return Create(
                    (ct, tcs) =>
                    {
                        f(tcs, cts.Token);
                        return tcs.Task.UsingEnumerator(e);
                    },
                    () => current,
                    d.Dispose
                );
            });
        }

        public static IAsyncEnumerable<TSource> TakeLast<TSource>(this IAsyncEnumerable<TSource> source, int count)
        {
            if (source == null)
                throw new ArgumentNullException("source");
            if (count < 0)
                throw new ArgumentOutOfRangeException("count");

            return Create(() =>
            {
                var e = source.GetEnumerator();

                var cts = new CancellationTokenDisposable();
                var d = new CompositeDisposable(cts, e);

                var q = new Queue<TSource>(count);
                var done = false;
                var current = default(TSource);

                var f = default(Action<TaskCompletionSource<bool>, CancellationToken>);
                f = (tcs, ct) =>
                {
                    if (!done)
                    {
                        e.MoveNext(ct).ContinueWith(t =>
                        {
                            t.Handle(tcs, res =>
                            {
                                if (res)
                                {
                                    var item = e.Current;

                                    if (q.Count >= count)
                                        q.Dequeue();
                                    q.Enqueue(item);
                                }
                                else
                                {
                                    done = true;
                                    e.Dispose();
                                }

                                f(tcs, ct);
                            });
                        });
                    }
                    else
                    {
                        if (q.Count > 0)
                        {
                            current = q.Dequeue();
                            tcs.TrySetResult(true);
                        }
                        else
                        {
                            tcs.TrySetResult(false);
                        }
                    }
                };

                return Create(
                    (ct, tcs) =>
                    {
                        f(tcs, cts.Token);
                        return tcs.Task.UsingEnumerator(e);
                    },
                    () => current,
                    d.Dispose
                );
            });
        }

        public static IAsyncEnumerable<TSource> SkipLast<TSource>(this IAsyncEnumerable<TSource> source, int count)
        {
            if (source == null)
                throw new ArgumentNullException("source");
            if (count < 0)
                throw new ArgumentOutOfRangeException("count");

            return Create(() =>
            {
                var e = source.GetEnumerator();

                var cts = new CancellationTokenDisposable();
                var d = new CompositeDisposable(cts, e);

                var q = new Queue<TSource>();
                var current = default(TSource);

                var f = default(Action<TaskCompletionSource<bool>, CancellationToken>);
                f = (tcs, ct) =>
                {
                    e.MoveNext(ct).ContinueWith(t =>
                    {
                        t.Handle(tcs, res =>
                        {
                            if (res)
                            {
                                var item = e.Current;

                                q.Enqueue(item);
                                if (q.Count > count)
                                {
                                    current = q.Dequeue();
                                    tcs.TrySetResult(true);
                                }
                                else
                                {
                                    f(tcs, ct);
                                }
                            }
                            else
                            {
                                tcs.TrySetResult(false);
                            }
                        });
                    });
                };

                return Create(
                    (ct, tcs) =>
                    {
                        f(tcs, cts.Token);
                        return tcs.Task.UsingEnumerator(e);
                    },
                    () => current,
                    d.Dispose
                );
            });
        }

        #endregion
    }
}

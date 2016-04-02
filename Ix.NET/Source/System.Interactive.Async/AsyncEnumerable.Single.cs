// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the Apache 2.0 License.
// See the LICENSE file in the project root for more information. 
using System;
using System.Collections.Generic;
using System.Runtime.ExceptionServices;
using System.Threading;
using System.Threading.Tasks;

namespace System.Linq
{
    public static partial class AsyncEnumerable
    {
        public static IAsyncEnumerable<TResult> Select<TSource, TResult>(this IAsyncEnumerable<TSource> source, Func<TSource, TResult> selector)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));
            if (selector == null)
                throw new ArgumentNullException(nameof(selector));

            return Create(() =>
            {
                var e = source.GetEnumerator();
                var current = default(TResult);

                var cts = new CancellationTokenDisposable();
                var d = Disposable.Create(cts, e);

                return Create(
                    async ct =>
                    {
                        if (await e.MoveNext(cts.Token).ConfigureAwait(false))
                        {
                            current = selector(e.Current);
                            return true;
                        }
                        return false;
                    },
                    () => current,
                    d.Dispose,
                    e
                );
            });
        }

        public static IAsyncEnumerable<TResult> Select<TSource, TResult>(this IAsyncEnumerable<TSource> source, Func<TSource, int, TResult> selector)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));
            if (selector == null)
                throw new ArgumentNullException(nameof(selector));

            return Create(() =>
            {
                var e = source.GetEnumerator();
                var current = default(TResult);
                var index = 0;

                var cts = new CancellationTokenDisposable();
                var d = Disposable.Create(cts, e);

                return Create(
                    async ct =>
                    {
                        if (await e.MoveNext(cts.Token).ConfigureAwait(false))
                        {
                            current = selector(e.Current, checked(index++));
                            return true;
                        }
                        return false;
                    },
                    () => current,
                    d.Dispose,
                    e
                );
            });
        }

        public static IAsyncEnumerable<TSource> AsAsyncEnumerable<TSource>(this IAsyncEnumerable<TSource> source)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));

            return source.Select(x => x);
        }

        public static IAsyncEnumerable<TSource> Where<TSource>(this IAsyncEnumerable<TSource> source, Func<TSource, bool> predicate)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));
            if (predicate == null)
                throw new ArgumentNullException(nameof(predicate));

            return Create(() =>
            {
                var e = source.GetEnumerator();

                var cts = new CancellationTokenDisposable();
                var d = Disposable.Create(cts, e);

                var f = default(Func<CancellationToken, Task<bool>>);
                f = async ct =>
                {
                    if (await e.MoveNext(ct).ConfigureAwait(false))
                    {
                        if (predicate(e.Current))
                            return true;
                        return await f(ct).ConfigureAwait(false);
                    }
                    return false;
                };

                return Create(
                    ct => f(cts.Token),
                    () => e.Current,
                    d.Dispose,
                    e
                );
            });
        }

        public static IAsyncEnumerable<TSource> Where<TSource>(this IAsyncEnumerable<TSource> source, Func<TSource, int, bool> predicate)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));
            if (predicate == null)
                throw new ArgumentNullException(nameof(predicate));

            return Create(() =>
            {
                var e = source.GetEnumerator();
                var index = 0;

                var cts = new CancellationTokenDisposable();
                var d = Disposable.Create(cts, e);

                var f = default(Func<CancellationToken, Task<bool>>);
                f = async ct =>
                {
                    if (await e.MoveNext(ct).ConfigureAwait(false))
                    {
                        if (predicate(e.Current, checked(index++)))
                            return true;
                        return await f(ct).ConfigureAwait(false);
                    }
                    return false;
                };

                return Create(
                    ct => f(cts.Token),
                    () => e.Current,
                    d.Dispose,
                    e
                );
            });
        }

        public static IAsyncEnumerable<TResult> SelectMany<TSource, TResult>(this IAsyncEnumerable<TSource> source, Func<TSource, IAsyncEnumerable<TResult>> selector)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));
            if (selector == null)
                throw new ArgumentNullException(nameof(selector));

            return Create(() =>
            {
                var e = source.GetEnumerator();
                var ie = default(IAsyncEnumerator<TResult>);

                var innerDisposable = new AssignableDisposable();

                var cts = new CancellationTokenDisposable();
                var d = Disposable.Create(cts, innerDisposable, e);

                var inner = default(Func<CancellationToken, Task<bool>>);
                var outer = default(Func<CancellationToken, Task<bool>>);

                inner = async ct =>
                {
                    if (await ie.MoveNext(ct).ConfigureAwait(false))
                    {
                        return true;
                    }
                    innerDisposable.Disposable = null;
                    return await outer(ct).ConfigureAwait(false);
                };

                outer = async ct =>
                {
                    if (await e.MoveNext(ct).ConfigureAwait(false))
                    {
                        var enumerable = selector(e.Current);
                        ie = enumerable.GetEnumerator();
                        innerDisposable.Disposable = ie;

                        return await inner(ct).ConfigureAwait(false);
                    }
                    return false;
                };

                return Create(ct => ie == null ? outer(cts.Token) : inner(cts.Token),
                    () => ie.Current,
                    d.Dispose,
                    e
                );
            });
        }

        public static IAsyncEnumerable<TResult> SelectMany<TSource, TResult>(this IAsyncEnumerable<TSource> source, Func<TSource, int, IAsyncEnumerable<TResult>> selector)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));
            if (selector == null)
                throw new ArgumentNullException(nameof(selector));

            return Create(() =>
            {
                var e = source.GetEnumerator();
                var ie = default(IAsyncEnumerator<TResult>);

                var index = 0;

                var innerDisposable = new AssignableDisposable();

                var cts = new CancellationTokenDisposable();
                var d = Disposable.Create(cts, innerDisposable, e);

                var inner = default(Func<CancellationToken, Task<bool>>);
                var outer = default(Func<CancellationToken, Task<bool>>);

                inner = async ct =>
                {
                    if (await ie.MoveNext(ct).ConfigureAwait(false))
                    {
                        return true;
                    }
                    innerDisposable.Disposable = null;
                    return await outer(ct).ConfigureAwait(false);
                };

                outer = async ct =>
                {
                    if (await e.MoveNext(ct).ConfigureAwait(false))
                    {
                        var enumerable = selector(e.Current, checked(index++));
                        ie = enumerable.GetEnumerator();
                        innerDisposable.Disposable = ie;

                        return await inner(ct).ConfigureAwait(false);
                    }
                    return false;
                };

                return Create(ct => ie == null ? outer(cts.Token) : inner(cts.Token),
                    () => ie.Current,
                    d.Dispose,
                    e
                );
            });
        }

        public static IAsyncEnumerable<TResult> SelectMany<TSource, TCollection, TResult>(this IAsyncEnumerable<TSource> source, Func<TSource, IAsyncEnumerable<TCollection>> selector, Func<TSource, TCollection, TResult> resultSelector)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));
            if (selector == null)
                throw new ArgumentNullException(nameof(selector));
            if (resultSelector == null)
                throw new ArgumentNullException(nameof(resultSelector));

            return source.SelectMany(x => selector(x).Select(y => resultSelector(x, y)));
        }

        public static IAsyncEnumerable<TResult> SelectMany<TSource, TCollection, TResult>(this IAsyncEnumerable<TSource> source, Func<TSource, int, IAsyncEnumerable<TCollection>> selector, Func<TSource, TCollection, TResult> resultSelector)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));
            if (selector == null)
                throw new ArgumentNullException(nameof(selector));
            if (resultSelector == null)
                throw new ArgumentNullException(nameof(resultSelector));

            return source.SelectMany((x, i) => selector(x, i).Select(y => resultSelector(x, y)));
        }

        public static IAsyncEnumerable<TType> OfType<TType>(this IAsyncEnumerable<object> source)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));

            return source.Where(x => x is TType).Cast<TType>();
        }

        public static IAsyncEnumerable<TResult> Cast<TResult>(this IAsyncEnumerable<object> source)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));

            return source.Select(x => (TResult)x);
        }

        public static IAsyncEnumerable<TSource> Take<TSource>(this IAsyncEnumerable<TSource> source, int count)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));
            if (count < 0)
                throw new ArgumentOutOfRangeException(nameof(count));

            return Create(() =>
            {
                var e = source.GetEnumerator();
                var n = count;

                var cts = new CancellationTokenDisposable();
                var d = Disposable.Create(cts, e);

                return Create(
                    async ct =>
                    {
                        if (n == 0)
                            return false;

                        var result = await e.MoveNext(cts.Token).ConfigureAwait(false);

                        --n;

                        if (n == 0)
                            e.Dispose();

                        return result;
                    },
                    () => e.Current,
                    d.Dispose,
                    e
                );
            });
        }

        public static IAsyncEnumerable<TSource> TakeWhile<TSource>(this IAsyncEnumerable<TSource> source, Func<TSource, bool> predicate)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));
            if (predicate == null)
                throw new ArgumentNullException(nameof(predicate));

            return Create(() =>
            {
                var e = source.GetEnumerator();

                var cts = new CancellationTokenDisposable();
                var d = Disposable.Create(cts, e);

                return Create(
                    async ct =>
                    {
                        if (await e.MoveNext(cts.Token).ConfigureAwait(false))
                        {
                            if (predicate(e.Current))
                            {
                                return true;
                            }
                        }
                        return false;
                    },
                    () => e.Current,
                    d.Dispose,
                    e
                );
            });
        }

        public static IAsyncEnumerable<TSource> TakeWhile<TSource>(this IAsyncEnumerable<TSource> source, Func<TSource, int, bool> predicate)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));
            if (predicate == null)
                throw new ArgumentNullException(nameof(predicate));

            return Create(() =>
            {
                var e = source.GetEnumerator();
                var index = 0;

                var cts = new CancellationTokenDisposable();
                var d = Disposable.Create(cts, e);

                return Create(
                    async ct =>
                    {
                        if (await e.MoveNext(cts.Token).ConfigureAwait(false))
                        {
                            if (predicate(e.Current, checked(index++)))
                            {
                                return true;
                            }
                        }
                        return false;
                    },
                    () => e.Current,
                    d.Dispose,
                    e
                );
            });
        }

        public static IAsyncEnumerable<TSource> Skip<TSource>(this IAsyncEnumerable<TSource> source, int count)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));
            if (count < 0)
                throw new ArgumentOutOfRangeException(nameof(count));

            return Create(() =>
            {
                var e = source.GetEnumerator();
                var n = count;

                var cts = new CancellationTokenDisposable();
                var d = Disposable.Create(cts, e);

                var f = default(Func<CancellationToken, Task<bool>>);
                f = async ct =>
                {
                    var moveNext = await e.MoveNext(ct).ConfigureAwait(false);
                    if (n == 0)
                    {
                        return moveNext;
                    }
                    --n;
                    if (!moveNext)
                    {
                        return false;
                    }
                    return await f(ct).ConfigureAwait(false);
                };

                return Create(
                    ct => f(cts.Token),
                    () => e.Current,
                    d.Dispose,
                    e
                );
            });
        }

        public static IAsyncEnumerable<TSource> SkipWhile<TSource>(this IAsyncEnumerable<TSource> source, Func<TSource, bool> predicate)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));
            if (predicate == null)
                throw new ArgumentNullException(nameof(predicate));

            return Create(() =>
            {
                var e = source.GetEnumerator();
                var skipping = true;

                var cts = new CancellationTokenDisposable();
                var d = Disposable.Create(cts, e);

                var f = default(Func<CancellationToken, Task<bool>>);
                f = async ct =>
                {
                    if (skipping)
                    {
                        if (await e.MoveNext(ct).ConfigureAwait(false))
                        {
                            if (predicate(e.Current))
                                return await f(ct).ConfigureAwait(false);
                            skipping = false;
                            return true;
                        }
                        return false;
                    }
                    return await e.MoveNext(ct).ConfigureAwait(false);
                };

                return Create(
                    f,
                    () => e.Current,
                    d.Dispose,
                    e
                );
            });
        }

        public static IAsyncEnumerable<TSource> SkipWhile<TSource>(this IAsyncEnumerable<TSource> source, Func<TSource, int, bool> predicate)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));
            if (predicate == null)
                throw new ArgumentNullException(nameof(predicate));

            return Create(() =>
            {
                var e = source.GetEnumerator();
                var skipping = true;
                var index = 0;

                var cts = new CancellationTokenDisposable();
                var d = Disposable.Create(cts, e);

                var f = default(Func<CancellationToken, Task<bool>>);
                f = async ct =>
                {
                    if (skipping)
                    {
                        if (await e.MoveNext(ct).ConfigureAwait(false))
                        {
                            if (predicate(e.Current, checked(index++)))
                                return await f(ct).ConfigureAwait(false);
                            skipping = false;
                            return true;
                        }
                        return false;
                    }
                    return await e.MoveNext(ct).ConfigureAwait(false);
                };

                return Create(
                    f,
                    () => e.Current,
                    d.Dispose,
                    e
                );
            });
        }

        public static IAsyncEnumerable<TSource> DefaultIfEmpty<TSource>(this IAsyncEnumerable<TSource> source, TSource defaultValue)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));

            return Create(() =>
            {
                var done = false;
                var hasElements = false;
                var e = source.GetEnumerator();
                var current = default(TSource);

                var cts = new CancellationTokenDisposable();
                var d = Disposable.Create(cts, e);

                var f = default(Func<CancellationToken, Task<bool>>);
                f = async ct =>
                {
                    if (done)
                        return false;
                    if (await e.MoveNext(ct).ConfigureAwait(false))
                    {
                        hasElements = true;
                        current = e.Current;
                        return true;
                    }
                    done = true;
                    if (!hasElements)
                    {
                        current = defaultValue;
                        return true;
                    }
                    return false;
                };

                return Create(
                    f,
                    () => current,
                    d.Dispose,
                    e
                );
            });
        }

        public static IAsyncEnumerable<TSource> DefaultIfEmpty<TSource>(this IAsyncEnumerable<TSource> source)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));

            return source.DefaultIfEmpty(default(TSource));
        }

        public static IAsyncEnumerable<TSource> Distinct<TSource>(this IAsyncEnumerable<TSource> source, IEqualityComparer<TSource> comparer)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));
            if (comparer == null)
                throw new ArgumentNullException(nameof(comparer));

            return Defer(() =>
            {
                var set = new HashSet<TSource>(comparer);
                return source.Where(set.Add);
            });
        }

        public static IAsyncEnumerable<TSource> Distinct<TSource>(this IAsyncEnumerable<TSource> source)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));

            return source.Distinct(EqualityComparer<TSource>.Default);
        }

        public static IAsyncEnumerable<TSource> Reverse<TSource>(this IAsyncEnumerable<TSource> source)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));

            return Create(() =>
            {
                var e = source.GetEnumerator();
                var stack = default(Stack<TSource>);

                var cts = new CancellationTokenDisposable();
                var d = Disposable.Create(cts, e);

                return Create(
                    async ct =>
                    {
                        if (stack == null)
                        {
                            stack = await Create(() => e).Aggregate(new Stack<TSource>(), (s, x) => { s.Push(x); return s; }, cts.Token).ConfigureAwait(false);
                            return stack.Count > 0;
                        }
                        stack.Pop();
                        return stack.Count > 0;
                    },
                    () => stack.Peek(),
                    d.Dispose,
                    e
                );
            });
        }

        public static IOrderedAsyncEnumerable<TSource> OrderBy<TSource, TKey>(this IAsyncEnumerable<TSource> source, Func<TSource, TKey> keySelector, IComparer<TKey> comparer)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));
            if (keySelector == null)
                throw new ArgumentNullException(nameof(keySelector));
            if (comparer == null)
                throw new ArgumentNullException(nameof(comparer));

            return new OrderedAsyncEnumerable<TSource, TKey>(
                Create(() =>
                {
                    var current = default(IEnumerable<TSource>);

                    return Create(
                        async ct =>
                        {
                            if (current == null)
                            {
                                current = await source.ToList(ct).ConfigureAwait(false);
                                return true;
                            }
                            return false;
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
                throw new ArgumentNullException(nameof(source));
            if (keySelector == null)
                throw new ArgumentNullException(nameof(keySelector));

            return source.OrderBy(keySelector, Comparer<TKey>.Default);
        }

        public static IOrderedAsyncEnumerable<TSource> OrderByDescending<TSource, TKey>(this IAsyncEnumerable<TSource> source, Func<TSource, TKey> keySelector, IComparer<TKey> comparer)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));
            if (keySelector == null)
                throw new ArgumentNullException(nameof(keySelector));
            if (comparer == null)
                throw new ArgumentNullException(nameof(comparer));

            return source.OrderBy(keySelector, new ReverseComparer<TKey>(comparer));
        }

        public static IOrderedAsyncEnumerable<TSource> OrderByDescending<TSource, TKey>(this IAsyncEnumerable<TSource> source, Func<TSource, TKey> keySelector)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));
            if (keySelector == null)
                throw new ArgumentNullException(nameof(keySelector));

            return source.OrderByDescending(keySelector, Comparer<TKey>.Default);
        }

        public static IOrderedAsyncEnumerable<TSource> ThenBy<TSource, TKey>(this IOrderedAsyncEnumerable<TSource> source, Func<TSource, TKey> keySelector)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));
            if (keySelector == null)
                throw new ArgumentNullException(nameof(keySelector));

            return source.ThenBy(keySelector, Comparer<TKey>.Default);
        }

        public static IOrderedAsyncEnumerable<TSource> ThenBy<TSource, TKey>(this IOrderedAsyncEnumerable<TSource> source, Func<TSource, TKey> keySelector, IComparer<TKey> comparer)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));
            if (keySelector == null)
                throw new ArgumentNullException(nameof(keySelector));
            if (comparer == null)
                throw new ArgumentNullException(nameof(comparer));

            return source.CreateOrderedEnumerable(keySelector, comparer, false);
        }

        public static IOrderedAsyncEnumerable<TSource> ThenByDescending<TSource, TKey>(this IOrderedAsyncEnumerable<TSource> source, Func<TSource, TKey> keySelector)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));
            if (keySelector == null)
                throw new ArgumentNullException(nameof(keySelector));

            return source.ThenByDescending(keySelector, Comparer<TKey>.Default);
        }

        public static IOrderedAsyncEnumerable<TSource> ThenByDescending<TSource, TKey>(this IOrderedAsyncEnumerable<TSource> source, Func<TSource, TKey> keySelector, IComparer<TKey> comparer)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));
            if (keySelector == null)
                throw new ArgumentNullException(nameof(keySelector));
            if (comparer == null)
                throw new ArgumentNullException(nameof(comparer));

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
                    var d = Disposable.Create(cts, e, d1);

                    var f = default(Func<CancellationToken, Task<bool>>);

                    f = async ct =>
                    {
                        if (await e.MoveNext(ct).ConfigureAwait(false))
                        {
                            list.AddRange(e.Current.OrderBy(keySelector, comparer).GroupUntil(keySelector, x => x, comparer));
                            return await f(ct).ConfigureAwait(false);
                        }
                        e.Dispose();

                        e1 = list.GetEnumerator();
                        d1.Disposable = e1;

                        return e1.MoveNext();
                    };

                    return Create(
                        async ct =>
                        {
                            if (e1 != null)
                            {
                                return e1.MoveNext();
                            }
                            return await f(cts.Token).ConfigureAwait(false);
                        },
                        () => e1.Current,
                        d.Dispose,
                        e
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
                throw new ArgumentNullException(nameof(source));
            if (keySelector == null)
                throw new ArgumentNullException(nameof(keySelector));
            if (elementSelector == null)
                throw new ArgumentNullException(nameof(elementSelector));
            if (comparer == null)
                throw new ArgumentNullException(nameof(comparer));

            return Create(() =>
            {
                var gate = new object();

                var e = source.GetEnumerator();
                var count = 1;

                var map = new Dictionary<TKey, Grouping<TKey, TElement>>(comparer);
                var list = new List<IAsyncGrouping<TKey, TElement>>();

                var index = 0;

                var current = default(IAsyncGrouping<TKey, TElement>);
                var faulted = default(ExceptionDispatchInfo);

                var res = default(bool?);

                var cts = new CancellationTokenDisposable();
                var refCount = new Disposable(
                    () =>
                    {
                        if (Interlocked.Decrement(ref count) == 0)
                            e.Dispose();
                    }
                );
                var d = Disposable.Create(cts, refCount);

                var iterateSource = default(Func<CancellationToken, Task<bool>>);
                iterateSource = async ct =>
                {
                    lock (gate)
                    {
                        if (res != null)
                        {
                            return res.Value;
                        }
                        res = null;
                    }

                    faulted?.Throw();

                    try
                    {
                        res = await e.MoveNext(ct).ConfigureAwait(false);
                        if (res == true)
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

                                throw;
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

                        return res.Value;
                    }
                    catch (Exception ex)
                    {
                        foreach (var v in map.Values)
                            v.Error(ex);

                        faulted = ExceptionDispatchInfo.Capture(ex);
                        throw;
                    }
                    finally
                    {
                        res = null;
                    }
                };

                var f = default(Func<CancellationToken, Task<bool>>);
                f = async ct =>
                {
                    var result = await iterateSource(ct).ConfigureAwait(false);

                    current = null;
                    lock (list)
                    {
                        if (index < list.Count)
                            current = list[index++];
                    }

                    if (current != null)
                    {
                        return true;
                    }
                    return result && await f(ct).ConfigureAwait(false);
                };

                return Create(
                    f,
                    () => current,
                    d.Dispose,
                    e
                );
            });
        }

        public static IAsyncEnumerable<IAsyncGrouping<TKey, TElement>> GroupBy<TSource, TKey, TElement>(this IAsyncEnumerable<TSource> source, Func<TSource, TKey> keySelector, Func<TSource, TElement> elementSelector)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));
            if (keySelector == null)
                throw new ArgumentNullException(nameof(keySelector));
            if (elementSelector == null)
                throw new ArgumentNullException(nameof(elementSelector));

            return source.GroupBy(keySelector, elementSelector, EqualityComparer<TKey>.Default);
        }

        public static IAsyncEnumerable<IAsyncGrouping<TKey, TSource>> GroupBy<TSource, TKey>(this IAsyncEnumerable<TSource> source, Func<TSource, TKey> keySelector, IEqualityComparer<TKey> comparer)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));
            if (keySelector == null)
                throw new ArgumentNullException(nameof(keySelector));
            if (comparer == null)
                throw new ArgumentNullException(nameof(comparer));

            return source.GroupBy(keySelector, x => x, comparer);
        }

        public static IAsyncEnumerable<IAsyncGrouping<TKey, TSource>> GroupBy<TSource, TKey>(this IAsyncEnumerable<TSource> source, Func<TSource, TKey> keySelector)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));
            if (keySelector == null)
                throw new ArgumentNullException(nameof(keySelector));

            return source.GroupBy(keySelector, x => x, EqualityComparer<TKey>.Default);
        }

        public static IAsyncEnumerable<TResult> GroupBy<TSource, TKey, TElement, TResult>(this IAsyncEnumerable<TSource> source, Func<TSource, TKey> keySelector, Func<TSource, TElement> elementSelector, Func<TKey, IAsyncEnumerable<TElement>, TResult> resultSelector, IEqualityComparer<TKey> comparer)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));
            if (keySelector == null)
                throw new ArgumentNullException(nameof(keySelector));
            if (elementSelector == null)
                throw new ArgumentNullException(nameof(elementSelector));
            if (resultSelector == null)
                throw new ArgumentNullException(nameof(resultSelector));
            if (comparer == null)
                throw new ArgumentNullException(nameof(comparer));

            return source.GroupBy(keySelector, elementSelector, comparer).Select(g => resultSelector(g.Key, g));
        }

        public static IAsyncEnumerable<TResult> GroupBy<TSource, TKey, TElement, TResult>(this IAsyncEnumerable<TSource> source, Func<TSource, TKey> keySelector, Func<TSource, TElement> elementSelector, Func<TKey, IAsyncEnumerable<TElement>, TResult> resultSelector)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));
            if (keySelector == null)
                throw new ArgumentNullException(nameof(keySelector));
            if (elementSelector == null)
                throw new ArgumentNullException(nameof(elementSelector));
            if (resultSelector == null)
                throw new ArgumentNullException(nameof(resultSelector));

            return source.GroupBy(keySelector, elementSelector, EqualityComparer<TKey>.Default).Select(g => resultSelector(g.Key, g));
        }

        public static IAsyncEnumerable<TResult> GroupBy<TSource, TKey, TResult>(this IAsyncEnumerable<TSource> source, Func<TSource, TKey> keySelector, Func<TKey, IAsyncEnumerable<TSource>, TResult> resultSelector, IEqualityComparer<TKey> comparer)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));
            if (keySelector == null)
                throw new ArgumentNullException(nameof(keySelector));
            if (resultSelector == null)
                throw new ArgumentNullException(nameof(resultSelector));
            if (comparer == null)
                throw new ArgumentNullException(nameof(comparer));

            return source.GroupBy(keySelector, x => x, comparer).Select(g => resultSelector(g.Key, g));
        }

        public static IAsyncEnumerable<TResult> GroupBy<TSource, TKey, TResult>(this IAsyncEnumerable<TSource> source, Func<TSource, TKey> keySelector, Func<TKey, IAsyncEnumerable<TSource>, TResult> resultSelector)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));
            if (keySelector == null)
                throw new ArgumentNullException(nameof(keySelector));
            if (resultSelector == null)
                throw new ArgumentNullException(nameof(resultSelector));

            return source.GroupBy(keySelector, x => x, EqualityComparer<TKey>.Default).Select(g => resultSelector(g.Key, g));
        }

        class Grouping<TKey, TElement> : IAsyncGrouping<TKey, TElement>
        {
            private readonly Func<CancellationToken, Task<bool>> iterateSource;
            private readonly IDisposable sourceDisposable;
            private readonly List<TElement> elements = new List<TElement>();
            private bool done = false;
            private ExceptionDispatchInfo exception = null;

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
                this.exception = ExceptionDispatchInfo.Capture(exception);
            }

            public IAsyncEnumerator<TElement> GetEnumerator()
            {
                var index = -1;

                var cts = new CancellationTokenDisposable();
                var d = Disposable.Create(cts, sourceDisposable);

                var f = default(Func<CancellationToken, Task<bool>>);
                f = async ct =>
                {
                    var size = 0;
                    lock (elements)
                        size = elements.Count;

                    if (index < size)
                    {
                        return true;
                    }
                    if (done)
                    {
                        exception?.Throw();
                        return false;
                    }
                    if (await iterateSource(ct).ConfigureAwait(false))
                    {
                        return await f(ct).ConfigureAwait(false);
                    }
                    return false;
                };

                return Create(
                    ct =>
                    {
                        ++index;
                        return f(cts.Token);
                    },
                    () => elements[index],
                    d.Dispose,
                    null
                );
            }
        }

        #region Ix

        public static IAsyncEnumerable<TSource> Do<TSource>(this IAsyncEnumerable<TSource> source, Action<TSource> onNext)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));
            if (onNext == null)
                throw new ArgumentNullException(nameof(onNext));

            return DoHelper(source, onNext, _ => { }, () => { });
        }

        public static IAsyncEnumerable<TSource> Do<TSource>(this IAsyncEnumerable<TSource> source, Action<TSource> onNext, Action onCompleted)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));
            if (onNext == null)
                throw new ArgumentNullException(nameof(onNext));
            if (onCompleted == null)
                throw new ArgumentNullException(nameof(onCompleted));

            return DoHelper(source, onNext, _ => { }, onCompleted);
        }

        public static IAsyncEnumerable<TSource> Do<TSource>(this IAsyncEnumerable<TSource> source, Action<TSource> onNext, Action<Exception> onError)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));
            if (onNext == null)
                throw new ArgumentNullException(nameof(onNext));
            if (onError == null)
                throw new ArgumentNullException(nameof(onError));

            return DoHelper(source, onNext, onError, () => { });
        }

        public static IAsyncEnumerable<TSource> Do<TSource>(this IAsyncEnumerable<TSource> source, Action<TSource> onNext, Action<Exception> onError, Action onCompleted)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));
            if (onNext == null)
                throw new ArgumentNullException(nameof(onNext));
            if (onError == null)
                throw new ArgumentNullException(nameof(onError));
            if (onCompleted == null)
                throw new ArgumentNullException(nameof(onCompleted));

            return DoHelper(source, onNext, onError, onCompleted);
        }

#if !NO_RXINTERFACES
        public static IAsyncEnumerable<TSource> Do<TSource>(this IAsyncEnumerable<TSource> source, IObserver<TSource> observer)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));
            if (observer == null)
                throw new ArgumentNullException(nameof(observer));

            return DoHelper(source, observer.OnNext, observer.OnError, observer.OnCompleted);
        }
#endif

        private static IAsyncEnumerable<TSource> DoHelper<TSource>(this IAsyncEnumerable<TSource> source, Action<TSource> onNext, Action<Exception> onError, Action onCompleted)
        {
            return Create(() =>
            {
                var e = source.GetEnumerator();

                var cts = new CancellationTokenDisposable();
                var d = Disposable.Create(cts, e);

                var current = default(TSource);

                var f = default(Func<CancellationToken, Task<bool>>);
                f = async ct =>
                {
                    try
                    {
                        var result = await e.MoveNext(ct).ConfigureAwait(false);
                        if (!result)
                        {
                            onCompleted();
                        }
                        else
                        {
                            current = e.Current;
                            onNext(current);
                        }
                        return result;
                    }
                    catch (OperationCanceledException)
                    {
                        throw;
                    }
                    catch (Exception ex)
                    {
                        onError(ex);
                        throw;
                    }
                };

                return Create(
                    f,
                    () => current,
                    d.Dispose,
                    e
                );
            });
        }

        public static void ForEach<TSource>(this IAsyncEnumerable<TSource> source, Action<TSource> action, CancellationToken cancellationToken)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));
            if (action == null)
                throw new ArgumentNullException(nameof(action));

            source.ForEachAsync(action, cancellationToken).Wait(cancellationToken);
        }

        public static Task ForEachAsync<TSource>(this IAsyncEnumerable<TSource> source, Action<TSource> action, CancellationToken cancellationToken)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));
            if (action == null)
                throw new ArgumentNullException(nameof(action));

            return source.ForEachAsync((x, i) => action(x), cancellationToken);
        }

        public static void ForEach<TSource>(this IAsyncEnumerable<TSource> source, Action<TSource, int> action, CancellationToken cancellationToken)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));
            if (action == null)
                throw new ArgumentNullException(nameof(action));

            source.ForEachAsync(action, cancellationToken).Wait(cancellationToken);
        }

        public static async Task ForEachAsync<TSource>(this IAsyncEnumerable<TSource> source, Action<TSource, int> action, CancellationToken cancellationToken)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));
            if (action == null)
                throw new ArgumentNullException(nameof(action));

            return ForEachAsync_(source, action, cancellationToken);
        }

        private static async Task ForEachAsync_<TSource>(IAsyncEnumerable<TSource> source, Action<TSource, int> action, CancellationToken cancellationToken)
        {
            var index = 0;
            using (var e = source.GetEnumerator())
            {
                while (await e.MoveNext(cancellationToken).ConfigureAwait(false))
                {
                    action(e.Current, checked(index++));
                }
            }
        }

        public static IAsyncEnumerable<TSource> Repeat<TSource>(this IAsyncEnumerable<TSource> source, int count)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));
            if (count < 0)
                throw new ArgumentOutOfRangeException(nameof(count));

            return Create(() =>
            {
                var e = default(IAsyncEnumerator<TSource>);
                var a = new AssignableDisposable();
                var n = count;
                var current = default(TSource);

                var cts = new CancellationTokenDisposable();
                var d = Disposable.Create(cts, a);

                var f = default(Func<CancellationToken, Task<bool>>);
                f = async ct =>
                {
                    if (e == null)
                    {
                        if (n-- == 0)
                        {
                            return false;
                        }

                        e = source.GetEnumerator();

                        a.Disposable = e;
                    }

                    if (await e.MoveNext(ct).ConfigureAwait(false))
                    {
                        current = e.Current;
                        return true;
                    }
                    e = null;
                    return await f(ct).ConfigureAwait(false);
                };

                return Create(
                    f,
                    () => current,
                    d.Dispose,
                    e
                );
            });
        }

        public static IAsyncEnumerable<TSource> Repeat<TSource>(this IAsyncEnumerable<TSource> source)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));

            return Create(() =>
            {
                var e = default(IAsyncEnumerator<TSource>);
                var a = new AssignableDisposable();
                var current = default(TSource);

                var cts = new CancellationTokenDisposable();
                var d = Disposable.Create(cts, a);

                var f = default(Func<CancellationToken, Task<bool>>);
                f = async ct =>
                {
                    if (e == null)
                    {
                        e = source.GetEnumerator();

                        a.Disposable = e;
                    }

                    if (await e.MoveNext(ct).ConfigureAwait(false))
                    {
                        current = e.Current;
                        return true;
                    }
                    e = null;
                    return await f(ct).ConfigureAwait(false);
                };

                return Create(
                    f,
                    () => current,
                    d.Dispose,
                    e
                );
            });
        }

        public static IAsyncEnumerable<TSource> IgnoreElements<TSource>(this IAsyncEnumerable<TSource> source)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));

            return Create(() =>
            {
                var e = source.GetEnumerator();

                var cts = new CancellationTokenDisposable();
                var d = Disposable.Create(cts, e);

                var f = default(Func<CancellationToken, Task<bool>>);
                f = async ct =>
                {
                    if (!await e.MoveNext(ct).ConfigureAwait(false))
                    {
                        return false;
                    }

                    return await f(ct).ConfigureAwait(false);
                };

                return Create<TSource>(
                    f,
                    () => { throw new InvalidOperationException(); },
                    d.Dispose,
                    e
                );
            });
        }

        public static IAsyncEnumerable<TSource> StartWith<TSource>(this IAsyncEnumerable<TSource> source, params TSource[] values)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));

            return values.ToAsyncEnumerable().Concat(source);
        }

        public static IAsyncEnumerable<IList<TSource>> Buffer<TSource>(this IAsyncEnumerable<TSource> source, int count)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));
            if (count <= 0)
                throw new ArgumentOutOfRangeException(nameof(count));

            return source.Buffer_(count, count);
        }

        public static IAsyncEnumerable<IList<TSource>> Buffer<TSource>(this IAsyncEnumerable<TSource> source, int count, int skip)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));
            if (count <= 0)
                throw new ArgumentOutOfRangeException(nameof(count));
            if (skip <= 0)
                throw new ArgumentOutOfRangeException(nameof(skip));

            return source.Buffer_(count, skip);
        }

        private static IAsyncEnumerable<IList<TSource>> Buffer_<TSource>(this IAsyncEnumerable<TSource> source, int count, int skip)
        {
            return Create(() =>
            {
                var e = source.GetEnumerator();

                var cts = new CancellationTokenDisposable();
                var d = Disposable.Create(cts, e);

                var buffers = new Queue<IList<TSource>>();

                var i = 0;

                var current = default(IList<TSource>);
                var stopped = false;

                var f = default(Func<CancellationToken, Task<bool>>);
                f = async ct =>
                {
                    if (!stopped)
                    {
                        if (await e.MoveNext(ct).ConfigureAwait(false))
                        {
                            var item = e.Current;

                            if (i++ % skip == 0)
                                buffers.Enqueue(new List<TSource>(count));

                            foreach (var buffer in buffers)
                                buffer.Add(item);

                            if (buffers.Count > 0 && buffers.Peek().Count == count)
                            {
                                current = buffers.Dequeue();
                                return true;
                            }
                            return await f(ct).ConfigureAwait(false);
                        }
                        stopped = true;
                        e.Dispose();

                        return await f(ct).ConfigureAwait(false);
                    }
                    if (buffers.Count > 0)
                    {
                        current = buffers.Dequeue();
                        return true;
                    }
                    return false;
                };

                return Create(
                    f,
                    () => current,
                    d.Dispose,
                    e
                );
            });
        }

        public static IAsyncEnumerable<TSource> Distinct<TSource, TKey>(this IAsyncEnumerable<TSource> source, Func<TSource, TKey> keySelector, IEqualityComparer<TKey> comparer)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));
            if (keySelector == null)
                throw new ArgumentNullException(nameof(keySelector));
            if (comparer == null)
                throw new ArgumentNullException(nameof(comparer));

            return Defer(() =>
            {
                var set = new HashSet<TKey>(comparer);
                return source.Where(item => set.Add(keySelector(item)));
            });
        }

        public static IAsyncEnumerable<TSource> Distinct<TSource, TKey>(this IAsyncEnumerable<TSource> source, Func<TSource, TKey> keySelector)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));
            if (keySelector == null)
                throw new ArgumentNullException(nameof(keySelector));

            return source.Distinct(keySelector, EqualityComparer<TKey>.Default);
        }

        public static IAsyncEnumerable<TSource> DistinctUntilChanged<TSource>(this IAsyncEnumerable<TSource> source)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));

            return source.DistinctUntilChanged_(x => x, EqualityComparer<TSource>.Default);
        }

        public static IAsyncEnumerable<TSource> DistinctUntilChanged<TSource>(this IAsyncEnumerable<TSource> source, IEqualityComparer<TSource> comparer)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));
            if (comparer == null)
                throw new ArgumentNullException(nameof(comparer));

            return source.DistinctUntilChanged_(x => x, comparer);
        }

        public static IAsyncEnumerable<TSource> DistinctUntilChanged<TSource, TKey>(this IAsyncEnumerable<TSource> source, Func<TSource, TKey> keySelector)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));
            if (keySelector == null)
                throw new ArgumentNullException(nameof(keySelector));

            return source.DistinctUntilChanged_(keySelector, EqualityComparer<TKey>.Default);
        }

        public static IAsyncEnumerable<TSource> DistinctUntilChanged<TSource, TKey>(this IAsyncEnumerable<TSource> source, Func<TSource, TKey> keySelector, IEqualityComparer<TKey> comparer)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));
            if (keySelector == null)
                throw new ArgumentNullException(nameof(keySelector));
            if (comparer == null)
                throw new ArgumentNullException(nameof(comparer));

            return source.DistinctUntilChanged_(keySelector, comparer);
        }

        private static IAsyncEnumerable<TSource> DistinctUntilChanged_<TSource, TKey>(this IAsyncEnumerable<TSource> source, Func<TSource, TKey> keySelector, IEqualityComparer<TKey> comparer)
        {
            return Create(() =>
            {
                var e = source.GetEnumerator();

                var cts = new CancellationTokenDisposable();
                var d = Disposable.Create(cts, e);

                var currentKey = default(TKey);
                var hasCurrentKey = false;
                var current = default(TSource);

                var f = default(Func<CancellationToken, Task<bool>>);
                f = async ct =>
                {
                    if (await e.MoveNext(ct).ConfigureAwait(false))
                    {
                        var item = e.Current;
                        var key = default(TKey);
                        var comparerEquals = false;

                        key = keySelector(item);

                        if (hasCurrentKey)
                        {
                            comparerEquals = comparer.Equals(currentKey, key);
                        }

                        if (!hasCurrentKey || !comparerEquals)
                        {
                            hasCurrentKey = true;
                            currentKey = key;

                            current = item;
                            return true;
                        }
                        return await f(ct).ConfigureAwait(false);
                    }
                    return false;
                };

                return Create(
                    f,
                    () => current,
                    d.Dispose,
                    e
                );
            });
        }

        public static IAsyncEnumerable<TSource> Expand<TSource>(this IAsyncEnumerable<TSource> source, Func<TSource, IAsyncEnumerable<TSource>> selector)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));
            if (selector == null)
                throw new ArgumentNullException(nameof(selector));

            return Create(() =>
            {
                var e = default(IAsyncEnumerator<TSource>);

                var cts = new CancellationTokenDisposable();
                var a = new AssignableDisposable();
                var d = Disposable.Create(cts, a);

                var queue = new Queue<IAsyncEnumerable<TSource>>();
                queue.Enqueue(source);

                var current = default(TSource);

                var f = default(Func<CancellationToken, Task<bool>>);
                f = async ct =>
                {
                    if (e == null)
                    {
                        if (queue.Count > 0)
                        {
                            var src = queue.Dequeue();

                            e = src.GetEnumerator();

                            a.Disposable = e;
                            return await f(ct).ConfigureAwait(false);
                        }
                        return false;
                    }
                    if (await e.MoveNext(ct).ConfigureAwait(false))
                    {
                        var item = e.Current;
                        var next = selector(item);

                        queue.Enqueue(next);
                        current = item;
                        return true;
                    }
                    e = null;
                    return await f(ct).ConfigureAwait(false);
                };

                return Create(
                    f,
                    () => current,
                    d.Dispose,
                    e
                );
            });
        }

        public static IAsyncEnumerable<TAccumulate> Scan<TSource, TAccumulate>(this IAsyncEnumerable<TSource> source, TAccumulate seed, Func<TAccumulate, TSource, TAccumulate> accumulator)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));
            if (accumulator == null)
                throw new ArgumentNullException(nameof(accumulator));

            return Create(() =>
            {
                var e = source.GetEnumerator();

                var cts = new CancellationTokenDisposable();
                var d = Disposable.Create(cts, e);

                var acc = seed;
                var current = default(TAccumulate);

                var f = default(Func<CancellationToken, Task<bool>>);
                f = async ct =>
                {
                    if (!await e.MoveNext(ct).ConfigureAwait(false))
                    {
                        return false;
                    }

                    var item = e.Current;
                    acc = accumulator(acc, item);

                    current = acc;
                    return true;
                };

                return Create(
                    f,
                    () => current,
                    d.Dispose,
                    e
                );
            });
        }

        public static IAsyncEnumerable<TSource> Scan<TSource>(this IAsyncEnumerable<TSource> source, Func<TSource, TSource, TSource> accumulator)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));
            if (accumulator == null)
                throw new ArgumentNullException(nameof(accumulator));

            return Create(() =>
            {
                var e = source.GetEnumerator();

                var cts = new CancellationTokenDisposable();
                var d = Disposable.Create(cts, e);

                var hasSeed = false;
                var acc = default(TSource);
                var current = default(TSource);

                var f = default(Func<CancellationToken, Task<bool>>);
                f = async ct =>
                {
                    if (!await e.MoveNext(ct).ConfigureAwait(false))
                    {
                        return false;
                    }

                    var item = e.Current;

                    if (!hasSeed)
                    {
                        hasSeed = true;
                        acc = item;
                        return await f(ct).ConfigureAwait(false);
                    }

                    acc = accumulator(acc, item);

                    current = acc;
                    return true;
                };

                return Create(
                    f,
                    () => current,
                    d.Dispose,
                    e
                );
            });
        }

        public static IAsyncEnumerable<TSource> TakeLast<TSource>(this IAsyncEnumerable<TSource> source, int count)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));
            if (count < 0)
                throw new ArgumentOutOfRangeException(nameof(count));

            return Create(() =>
            {
                var e = source.GetEnumerator();

                var cts = new CancellationTokenDisposable();
                var d = Disposable.Create(cts, e);

                var q = new Queue<TSource>(count);
                var done = false;
                var current = default(TSource);

                var f = default(Func<CancellationToken, Task<bool>>);
                f = async ct =>
                {
                    if (!done)
                    {
                        if (await e.MoveNext(ct).ConfigureAwait(false))
                        {
                            if (count > 0)
                            {
                                var item = e.Current;
                                if (q.Count >= count)
                                    q.Dequeue();
                                q.Enqueue(item);
                            }
                        }
                        else
                        {
                            done = true;
                            e.Dispose();
                        }

                        return await f(ct).ConfigureAwait(false);
                    }
                    if (q.Count > 0)
                    {
                        current = q.Dequeue();
                        return true;
                    }
                    return false;
                };

                return Create(
                    f,
                    () => current,
                    d.Dispose,
                    e
                );
            });
        }

        public static IAsyncEnumerable<TSource> SkipLast<TSource>(this IAsyncEnumerable<TSource> source, int count)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));
            if (count < 0)
                throw new ArgumentOutOfRangeException(nameof(count));

            return Create(() =>
            {
                var e = source.GetEnumerator();

                var cts = new CancellationTokenDisposable();
                var d = Disposable.Create(cts, e);

                var q = new Queue<TSource>();
                var current = default(TSource);

                var f = default(Func<CancellationToken, Task<bool>>);
                f = async ct =>
                {
                    if (await e.MoveNext(ct).ConfigureAwait(false))
                    {
                        var item = e.Current;

                        q.Enqueue(item);
                        if (q.Count > count)
                        {
                            current = q.Dequeue();
                            return true;
                        }
                        return await f(ct).ConfigureAwait(false);
                    }
                    return false;
                };

                return Create(
                    f,
                    () => current,
                    d.Dispose,
                    e
                );
            });
        }

        #endregion
    }
}

// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the Apache 2.0 License.
// See the LICENSE file in the project root for more information. 
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
                throw new ArgumentNullException(nameof(first));
            if (second == null)
                throw new ArgumentNullException(nameof(second));

            return Create(() =>
            {
                var switched = false;
                var e = first.GetEnumerator();

                var cts = new CancellationTokenDisposable();
                var a = new AssignableDisposable { Disposable = e };
                var d = Disposable.Create(cts, a);

                var f = default(Func<CancellationToken, Task<bool>>);
                f = async ct =>
                {
                    if (await e.MoveNext(ct).ConfigureAwait(false))
                    {
                        return true;
                    }
                    if (switched)
                    {
                        return false;
                    }
                    switched = true;

                    e = second.GetEnumerator();
                    a.Disposable = e;

                    return await f(ct).ConfigureAwait(false);
                };

                return Create(
                    f,
                    () => e.Current,
                    d.Dispose,
                    e
                );
            });
        }

        public static IAsyncEnumerable<TResult> Zip<TFirst, TSecond, TResult>(this IAsyncEnumerable<TFirst> first, IAsyncEnumerable<TSecond> second, Func<TFirst, TSecond, TResult> selector)
        {
            if (first == null)
                throw new ArgumentNullException(nameof(first));
            if (second == null)
                throw new ArgumentNullException(nameof(second));
            if (selector == null)
                throw new ArgumentNullException(nameof(selector));

            return Create(() =>
            {
                var e1 = first.GetEnumerator();
                var e2 = second.GetEnumerator();
                var current = default(TResult);

                var cts = new CancellationTokenDisposable();
                var d = Disposable.Create(cts, e1, e2);

                return Create(
                    ct => e1.MoveNext(cts.Token).Zip(e2.MoveNext(cts.Token), (f, s) =>
                        {
                            var result = f && s;
                            if (result)
                                current = selector(e1.Current, e2.Current);
                            return result;
                        }),
                    () => current,
                    d.Dispose
                );
            });
        }

        public static IAsyncEnumerable<TSource> Except<TSource>(this IAsyncEnumerable<TSource> first, IAsyncEnumerable<TSource> second, IEqualityComparer<TSource> comparer)
        {
            if (first == null)
                throw new ArgumentNullException(nameof(first));
            if (second == null)
                throw new ArgumentNullException(nameof(second));
            if (comparer == null)
                throw new ArgumentNullException(nameof(comparer));

            return Create(() =>
            {
                var e = first.GetEnumerator();

                var cts = new CancellationTokenDisposable();
                var d = Disposable.Create(cts, e);

                var mapTask = default(Task<Dictionary<TSource, TSource>>);
                var getMapTask = new Func<CancellationToken, Task<Dictionary<TSource, TSource>>>(ct =>
                    mapTask ?? (mapTask = second.ToDictionary(x => x, comparer, ct)));

                var f = default(Func<CancellationToken, Task<bool>>);
                f = async ct =>
                {
                    if (await e.MoveNext(ct).Zip(getMapTask(ct), (b, _) => b).ConfigureAwait(false))
                    {
                        if (!mapTask.Result.ContainsKey(e.Current))
                            return true;
                        return await f(ct).ConfigureAwait(false);
                    }
                    return false;
                };

                return Create(
                    f,
                    () => e.Current,
                    d.Dispose,
                    e
                );
            });
        }

        public static IAsyncEnumerable<TSource> Except<TSource>(this IAsyncEnumerable<TSource> first, IAsyncEnumerable<TSource> second)
        {
            if (first == null)
                throw new ArgumentNullException(nameof(first));
            if (second == null)
                throw new ArgumentNullException(nameof(second));

            return first.Except(second, EqualityComparer<TSource>.Default);
        }

        public static IAsyncEnumerable<TSource> Intersect<TSource>(this IAsyncEnumerable<TSource> first, IAsyncEnumerable<TSource> second, IEqualityComparer<TSource> comparer)
        {
            if (first == null)
                throw new ArgumentNullException(nameof(first));
            if (second == null)
                throw new ArgumentNullException(nameof(second));
            if (comparer == null)
                throw new ArgumentNullException(nameof(comparer));

            return Create(() =>
            {
                var e = first.GetEnumerator();

                var cts = new CancellationTokenDisposable();
                var d = Disposable.Create(cts, e);

                var mapTask = default(Task<Dictionary<TSource, TSource>>);
                var getMapTask = new Func<CancellationToken, Task<Dictionary<TSource, TSource>>>(ct =>
                {
                    if (mapTask == null)
                        mapTask = second.ToDictionary(x => x, comparer, ct);
                    return mapTask;
                });

                var f = default(Func<CancellationToken, Task<bool>>);
                f = async ct =>
                {
                    if (await e.MoveNext(ct).Zip(getMapTask(ct), (b, _) => b).ConfigureAwait(false))
                    {
                        if (mapTask.Result.ContainsKey(e.Current))
                            return true;
                        return await f(ct).ConfigureAwait(false);
                    }
                    return false;
                };

                return Create(
                    f,
                    () => e.Current,
                    d.Dispose,
                    e
                );
            });
        }

        public static IAsyncEnumerable<TSource> Intersect<TSource>(this IAsyncEnumerable<TSource> first, IAsyncEnumerable<TSource> second)
        {
            if (first == null)
                throw new ArgumentNullException(nameof(first));
            if (second == null)
                throw new ArgumentNullException(nameof(second));

            return first.Intersect(second, EqualityComparer<TSource>.Default);
        }

        public static IAsyncEnumerable<TSource> Union<TSource>(this IAsyncEnumerable<TSource> first, IAsyncEnumerable<TSource> second, IEqualityComparer<TSource> comparer)
        {
            if (first == null)
                throw new ArgumentNullException(nameof(first));
            if (second == null)
                throw new ArgumentNullException(nameof(second));
            if (comparer == null)
                throw new ArgumentNullException(nameof(comparer));

            return first.Concat(second).Distinct(comparer);
        }

        public static IAsyncEnumerable<TSource> Union<TSource>(this IAsyncEnumerable<TSource> first, IAsyncEnumerable<TSource> second)
        {
            if (first == null)
                throw new ArgumentNullException(nameof(first));
            if (second == null)
                throw new ArgumentNullException(nameof(second));

            return first.Union(second, EqualityComparer<TSource>.Default);
        }

        public static Task<bool> SequenceEqual<TSource>(this IAsyncEnumerable<TSource> first, IAsyncEnumerable<TSource> second, IEqualityComparer<TSource> comparer, CancellationToken cancellationToken)
        {
            if (first == null)
                throw new ArgumentNullException(nameof(first));
            if (second == null)
                throw new ArgumentNullException(nameof(second));
            if (comparer == null)
                throw new ArgumentNullException(nameof(comparer));

            return SequenceEqual_(first, second, comparer, cancellationToken);
        }

        private static async Task<bool> SequenceEqual_<TSource>(IAsyncEnumerable<TSource> first, IAsyncEnumerable<TSource> second, IEqualityComparer<TSource> comparer,
            CancellationToken cancellationToken)
        {
            using (var e1 = first.GetEnumerator())
            using (var e2 = second.GetEnumerator())
            {
                while (await e1.MoveNext(cancellationToken).ConfigureAwait(false))
                {
                    if (!(await e2.MoveNext(cancellationToken).ConfigureAwait(false) && comparer.Equals(e1.Current, e2.Current)))
                    {
                        return false;
                    }
                }

                return !await e2.MoveNext(cancellationToken).ConfigureAwait(false);
            }
        }

        public static Task<bool> SequenceEqual<TSource>(this IAsyncEnumerable<TSource> first, IAsyncEnumerable<TSource> second, CancellationToken cancellationToken)
        {
            if (first == null)
                throw new ArgumentNullException(nameof(first));
            if (second == null)
                throw new ArgumentNullException(nameof(second));

            return first.SequenceEqual(second, EqualityComparer<TSource>.Default, cancellationToken);
        }

        public static IAsyncEnumerable<TResult> GroupJoin<TOuter, TInner, TKey, TResult>(this IAsyncEnumerable<TOuter> outer, IAsyncEnumerable<TInner> inner, Func<TOuter, TKey> outerKeySelector, Func<TInner, TKey> innerKeySelector, Func<TOuter, IAsyncEnumerable<TInner>, TResult> resultSelector, IEqualityComparer<TKey> comparer)
        {
            if (outer == null)
                throw new ArgumentNullException(nameof(outer));
            if (inner == null)
                throw new ArgumentNullException(nameof(inner));
            if (outerKeySelector == null)
                throw new ArgumentNullException(nameof(outerKeySelector));
            if (innerKeySelector == null)
                throw new ArgumentNullException(nameof(innerKeySelector));
            if (resultSelector == null)
                throw new ArgumentNullException(nameof(resultSelector));
            if (comparer == null)
                throw new ArgumentNullException(nameof(comparer));


            return new GroupJoinAsyncEnumerable<TOuter, TInner, TKey, TResult>(outer, inner, outerKeySelector, innerKeySelector, resultSelector, comparer);
        }

        public static IAsyncEnumerable<TResult> GroupJoin<TOuter, TInner, TKey, TResult>(this IAsyncEnumerable<TOuter> outer, IAsyncEnumerable<TInner> inner, Func<TOuter, TKey> outerKeySelector, Func<TInner, TKey> innerKeySelector, Func<TOuter, IAsyncEnumerable<TInner>, TResult> resultSelector)
        {
            if (outer == null)
                throw new ArgumentNullException(nameof(outer));
            if (inner == null)
                throw new ArgumentNullException(nameof(inner));
            if (outerKeySelector == null)
                throw new ArgumentNullException(nameof(outerKeySelector));
            if (innerKeySelector == null)
                throw new ArgumentNullException(nameof(innerKeySelector));
            if (resultSelector == null)
                throw new ArgumentNullException(nameof(resultSelector));

            return outer.GroupJoin(inner, outerKeySelector, innerKeySelector, resultSelector, EqualityComparer<TKey>.Default);
        }


        private sealed class GroupJoinAsyncEnumerable<TOuter, TInner, TKey, TResult> : IAsyncEnumerable<TResult>
        {
            private readonly IAsyncEnumerable<TOuter> _outer;
            private readonly IAsyncEnumerable<TInner> _inner;
            private readonly Func<TOuter, TKey> _outerKeySelector;
            private readonly Func<TInner, TKey> _innerKeySelector;
            private readonly Func<TOuter, IAsyncEnumerable<TInner>, TResult> _resultSelector;
            private readonly IEqualityComparer<TKey> _comparer;

            public GroupJoinAsyncEnumerable(
                IAsyncEnumerable<TOuter> outer,
                IAsyncEnumerable<TInner> inner,
                Func<TOuter, TKey> outerKeySelector,
                Func<TInner, TKey> innerKeySelector,
                Func<TOuter, IAsyncEnumerable<TInner>, TResult> resultSelector,
                IEqualityComparer<TKey> comparer)
            {
                _outer = outer;
                _inner = inner;
                _outerKeySelector = outerKeySelector;
                _innerKeySelector = innerKeySelector;
                _resultSelector = resultSelector;
                _comparer = comparer;
            }

            public IAsyncEnumerator<TResult> GetEnumerator()
                => new GroupJoinAsyncEnumerator(
                    _outer.GetEnumerator(),
                    _inner,
                    _outerKeySelector,
                    _innerKeySelector,
                    _resultSelector,
                    _comparer);

            private sealed class GroupJoinAsyncEnumerator : IAsyncEnumerator<TResult>
            {
                private readonly IAsyncEnumerator<TOuter> _outer;
                private readonly IAsyncEnumerable<TInner> _inner;
                private readonly Func<TOuter, TKey> _outerKeySelector;
                private readonly Func<TInner, TKey> _innerKeySelector;
                private readonly Func<TOuter, IAsyncEnumerable<TInner>, TResult> _resultSelector;
                private readonly IEqualityComparer<TKey> _comparer;

                private Internal.Lookup<TKey, TInner> _lookup;

                public GroupJoinAsyncEnumerator(
                    IAsyncEnumerator<TOuter> outer,
                    IAsyncEnumerable<TInner> inner,
                    Func<TOuter, TKey> outerKeySelector,
                    Func<TInner, TKey> innerKeySelector,
                    Func<TOuter, IAsyncEnumerable<TInner>, TResult> resultSelector,
                    IEqualityComparer<TKey> comparer)
                {
                    _outer = outer;
                    _inner = inner;
                    _outerKeySelector = outerKeySelector;
                    _innerKeySelector = innerKeySelector;
                    _resultSelector = resultSelector;
                    _comparer = comparer;
                }

                public async Task<bool> MoveNext(CancellationToken cancellationToken)
                {
                    // nothing to do 
                    if (!await _outer.MoveNext(cancellationToken).ConfigureAwait(false))
                    {
                        return false;
                    }

                    if (_lookup == null)
                    {
                        _lookup = await Internal.Lookup<TKey, TInner>.CreateForJoinAsync(_inner, _innerKeySelector, _comparer, cancellationToken).ConfigureAwait(false);
                    }
                    
                    var item = _outer.Current;
                    Current = _resultSelector(item, new AsyncEnumerableAdapter<TInner>(_lookup[_outerKeySelector(item)]));
                    return true;
                }

                public TResult Current { get; private set; }

                public void Dispose()
                {
                    _outer.Dispose();
                }

      
            }
        }

        private sealed class AsyncEnumerableAdapter<T> : IAsyncEnumerable<T>
        {
            private readonly IEnumerable<T> _source;

            public AsyncEnumerableAdapter(IEnumerable<T> source)
            {
                _source = source;
            }

            public IAsyncEnumerator<T> GetEnumerator()
                => new AsyncEnumeratorAdapter(_source.GetEnumerator());

            private sealed class AsyncEnumeratorAdapter : IAsyncEnumerator<T>
            {
                private readonly IEnumerator<T> _enumerator;

                public AsyncEnumeratorAdapter(IEnumerator<T> enumerator)
                {
                    _enumerator = enumerator;
                }

                public Task<bool> MoveNext(CancellationToken cancellationToken)
                {
                    cancellationToken.ThrowIfCancellationRequested();

#if HAS_AWAIT
                    return Task.FromResult(_enumerator.MoveNext());
#else
                    return TaskEx.FromResult(_enumerator.MoveNext());
#endif
                }

                public T Current => _enumerator.Current;

                public void Dispose() => _enumerator.Dispose();
            }
        }

        public static IAsyncEnumerable<TResult> Join<TOuter, TInner, TKey, TResult>(this IAsyncEnumerable<TOuter> outer, IAsyncEnumerable<TInner> inner, Func<TOuter, TKey> outerKeySelector, Func<TInner, TKey> innerKeySelector, Func<TOuter, TInner, TResult> resultSelector, IEqualityComparer<TKey> comparer)
        {
            if (outer == null)
                throw new ArgumentNullException(nameof(outer));
            if (inner == null)
                throw new ArgumentNullException(nameof(inner));
            if (outerKeySelector == null)
                throw new ArgumentNullException(nameof(outerKeySelector));
            if (innerKeySelector == null)
                throw new ArgumentNullException(nameof(innerKeySelector));
            if (resultSelector == null)
                throw new ArgumentNullException(nameof(resultSelector));
            if (comparer == null)
                throw new ArgumentNullException(nameof(comparer));

            return Create(() =>
            {
                var oe = outer.GetEnumerator();
                var ie = inner.GetEnumerator();

                var cts = new CancellationTokenDisposable();
                var d = Disposable.Create(cts, oe, ie);

                var current = default(TResult);
                var useOuter = true;
                var outerMap = new Dictionary<TKey, List<TOuter>>(comparer);
                var innerMap = new Dictionary<TKey, List<TInner>>(comparer);
                var q = new Queue<TResult>();

                var f = default(Func<CancellationToken, Task<bool>>);
                f = async ct =>
                {
                    if (q.Count > 0)
                    {
                        current = q.Dequeue();
                        return true;
                    }

                    var b = useOuter;
                    if (ie == null && oe == null)
                    {
                        return false;
                    }
                    if (ie == null)
                        b = true;
                    else if (oe == null)
                        b = false;
                    useOuter = !useOuter;

                    var enqueue = new Func<TOuter, TInner, bool>((o, i) =>
                    {
                        var result = resultSelector(o, i);
                        q.Enqueue(result);
                        return true;
                    });

                    if (b)
                    {
                        if (await oe.MoveNext(ct).ConfigureAwait(false))
                        {
                            var element = oe.Current;
                            var key = default(TKey);

                            key = outerKeySelector(element);

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
                                    return false;
                            }

                            return await f(ct).ConfigureAwait(false);
                        }
                        oe.Dispose();
                        oe = null;
                        return await f(ct).ConfigureAwait(false);
                    }
                    if (await ie.MoveNext(ct).ConfigureAwait(false))
                    {
                        var element = ie.Current;
                        var key = innerKeySelector(element);

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
                                return false;
                        }

                        return await f(ct).ConfigureAwait(false);
                    }
                    ie.Dispose();
                    ie = null;
                    return await f(ct).ConfigureAwait(false);
                };

                return Create(
                    f,
                    () => current,
                    d.Dispose,
                    ie
                );
            });
        }

        public static IAsyncEnumerable<TResult> Join<TOuter, TInner, TKey, TResult>(this IAsyncEnumerable<TOuter> outer, IAsyncEnumerable<TInner> inner, Func<TOuter, TKey> outerKeySelector, Func<TInner, TKey> innerKeySelector, Func<TOuter, TInner, TResult> resultSelector)
        {
            if (outer == null)
                throw new ArgumentNullException(nameof(outer));
            if (inner == null)
                throw new ArgumentNullException(nameof(inner));
            if (outerKeySelector == null)
                throw new ArgumentNullException(nameof(outerKeySelector));
            if (innerKeySelector == null)
                throw new ArgumentNullException(nameof(innerKeySelector));
            if (resultSelector == null)
                throw new ArgumentNullException(nameof(resultSelector));

            return outer.Join(inner, outerKeySelector, innerKeySelector, resultSelector, EqualityComparer<TKey>.Default);
        }

        public static IAsyncEnumerable<TSource> Concat<TSource>(this IEnumerable<IAsyncEnumerable<TSource>> sources)
        {
            if (sources == null)
                throw new ArgumentNullException(nameof(sources));

            return sources.Concat_();
        }

        public static IAsyncEnumerable<TSource> Concat<TSource>(params IAsyncEnumerable<TSource>[] sources)
        {
            if (sources == null)
                throw new ArgumentNullException(nameof(sources));

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
                var d = Disposable.Create(cts, se, a);

                var f = default(Func<CancellationToken, Task<bool>>);
                f = async ct =>
                {
                    if (e == null)
                    {
                        var b = false;
                        b = se.MoveNext();
                        if (b)
                            e = se.Current.GetEnumerator();

                        if (!b)
                        {
                            return false;
                        }

                        a.Disposable = e;
                    }

                    if (await e.MoveNext(ct).ConfigureAwait(false))
                    {
                        return true;
                    }
                    e.Dispose();
                    e = null;

                    return await f(ct).ConfigureAwait(false);
                };

                return Create(
                    f,
                    () => e.Current,
                    d.Dispose,
                    a
                );
            });
        }

        public static IAsyncEnumerable<TOther> SelectMany<TSource, TOther>(this IAsyncEnumerable<TSource> source, IAsyncEnumerable<TOther> other)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));
            if (other == null)
                throw new ArgumentNullException(nameof(other));

            return source.SelectMany(_ => other);
        }
    }
}

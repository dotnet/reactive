// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the Apache 2.0 License.
// See the LICENSE file in the project root for more information. 

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace System.Linq
{
    public static partial class AsyncEnumerable
    {
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

            return Create(
                () =>
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

                            var enqueue = new Func<TOuter, TInner, bool>(
                                (o, i) =>
                                {
                                    var result = resultSelector(o, i);
                                    q.Enqueue(result);
                                    return true;
                                });

                            if (b)
                            {
                                if (await oe.MoveNext(ct)
                                            .ConfigureAwait(false))
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

                                    return await f(ct)
                                               .ConfigureAwait(false);
                                }
                                oe.Dispose();
                                oe = null;
                                return await f(ct)
                                           .ConfigureAwait(false);
                            }
                            if (await ie.MoveNext(ct)
                                        .ConfigureAwait(false))
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

                                return await f(ct)
                                           .ConfigureAwait(false);
                            }
                            ie.Dispose();
                            ie = null;
                            return await f(ct)
                                       .ConfigureAwait(false);
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
    }
}
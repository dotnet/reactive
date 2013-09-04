// Copyright (c) Microsoft Open Technologies, Inc. All rights reserved. See License.txt in the project root for license information.

using System.Collections.Generic;
using System.Linq;
using System.Reactive.Concurrency;
using System.Reactive.Disposables;
using System.Reactive.Subjects;

#if !NO_TPL
using System.Reactive.Threading.Tasks;
using System.Threading;
using System.Threading.Tasks;
#endif

namespace System.Reactive.Linq
{
#if !NO_PERF
    using Observαble;
#endif

    internal partial class QueryLanguage
    {
        #region + Cast +

        public virtual IObservable<TResult> Cast<TResult>(IObservable<object> source)
        {
#if !NO_PERF
            return new Cast<object, TResult>(source);
#else
            return source.Select(x => (TResult)x);
#endif
        }

        #endregion

        #region + DefaultIfEmpty +

        public virtual IObservable<TSource> DefaultIfEmpty<TSource>(IObservable<TSource> source)
        {
#if !NO_PERF
            return new DefaultIfEmpty<TSource>(source, default(TSource));
#else
            return DefaultIfEmpty_(source, default(TSource));
#endif
        }

        public virtual IObservable<TSource> DefaultIfEmpty<TSource>(IObservable<TSource> source, TSource defaultValue)
        {
#if !NO_PERF
            return new DefaultIfEmpty<TSource>(source, defaultValue);
#else
            return DefaultIfEmpty_(source, defaultValue);
#endif
        }

#if NO_PERF
        private static IObservable<TSource> DefaultIfEmpty_<TSource>(IObservable<TSource> source, TSource defaultValue)
        {
            return new AnonymousObservable<TSource>(observer =>
            {
                var found = false;
                return source.Subscribe(
                    x =>
                    {
                        found = true;
                        observer.OnNext(x);
                    },
                    observer.OnError,
                    () =>
                    {
                        if (!found)
                            observer.OnNext(defaultValue);
                        observer.OnCompleted();
                    }
                );
            });
        }
#endif

        #endregion

        #region + Distinct +

        public virtual IObservable<TSource> Distinct<TSource>(IObservable<TSource> source)
        {
#if !NO_PERF
            return new Distinct<TSource, TSource>(source, x => x, EqualityComparer<TSource>.Default);
#else
            return Distinct_(source, x => x, EqualityComparer<TSource>.Default);
#endif
        }

        public virtual IObservable<TSource> Distinct<TSource>(IObservable<TSource> source, IEqualityComparer<TSource> comparer)
        {
#if !NO_PERF
            return new Distinct<TSource, TSource>(source, x => x, comparer);
#else
            return Distinct_(source, x => x, comparer);
#endif
        }

        public virtual IObservable<TSource> Distinct<TSource, TKey>(IObservable<TSource> source, Func<TSource, TKey> keySelector)
        {
#if !NO_PERF
            return new Distinct<TSource, TKey>(source, keySelector, EqualityComparer<TKey>.Default);
#else
            return Distinct_(source, keySelector, EqualityComparer<TKey>.Default);
#endif
        }

        public virtual IObservable<TSource> Distinct<TSource, TKey>(IObservable<TSource> source, Func<TSource, TKey> keySelector, IEqualityComparer<TKey> comparer)
        {
#if !NO_PERF
            return new Distinct<TSource, TKey>(source, keySelector, comparer);
#else
            return Distinct_(source, keySelector, comparer);
#endif
        }

#if NO_PERF
        private static IObservable<TSource> Distinct_<TSource, TKey>(IObservable<TSource> source, Func<TSource, TKey> keySelector, IEqualityComparer<TKey> comparer)
        {
            return new AnonymousObservable<TSource>(observer =>
            {
                var hashSet = new HashSet<TKey>(comparer);
                return source.Subscribe(
                    x =>
                    {
                        var key = default(TKey);
                        var hasAdded = false;

                        try
                        {
                            key = keySelector(x);
                            hasAdded = hashSet.Add(key);
                        }
                        catch (Exception exception)
                        {
                            observer.OnError(exception);
                            return;
                        }

                        if (hasAdded)
                            observer.OnNext(x);
                    },
                    observer.OnError,
                    observer.OnCompleted
                );
            });
        }
#endif

        #endregion

        #region + GroupBy +

        public virtual IObservable<IGroupedObservable<TKey, TElement>> GroupBy<TSource, TKey, TElement>(IObservable<TSource> source, Func<TSource, TKey> keySelector, Func<TSource, TElement> elementSelector)
        {
            return GroupBy_<TSource, TKey, TElement>(source, keySelector, elementSelector, EqualityComparer<TKey>.Default);
        }

        public virtual IObservable<IGroupedObservable<TKey, TSource>> GroupBy<TSource, TKey>(IObservable<TSource> source, Func<TSource, TKey> keySelector, IEqualityComparer<TKey> comparer)
        {
            return GroupBy_<TSource, TKey, TSource>(source, keySelector, x => x, comparer);
        }

        public virtual IObservable<IGroupedObservable<TKey, TSource>> GroupBy<TSource, TKey>(IObservable<TSource> source, Func<TSource, TKey> keySelector)
        {
            return GroupBy_<TSource, TKey, TSource>(source, keySelector, x => x, EqualityComparer<TKey>.Default);
        }

        public virtual IObservable<IGroupedObservable<TKey, TElement>> GroupBy<TSource, TKey, TElement>(IObservable<TSource> source, Func<TSource, TKey> keySelector, Func<TSource, TElement> elementSelector, IEqualityComparer<TKey> comparer)
        {
            return GroupBy_<TSource, TKey, TElement>(source, keySelector, elementSelector, comparer);
        }

        private static IObservable<IGroupedObservable<TKey, TElement>> GroupBy_<TSource, TKey, TElement>(IObservable<TSource> source, Func<TSource, TKey> keySelector, Func<TSource, TElement> elementSelector, IEqualityComparer<TKey> comparer)
        {
#if !NO_PERF
            return new GroupBy<TSource, TKey, TElement>(source, keySelector, elementSelector, comparer);
#else
            return GroupByUntil_<TSource, TKey, TElement, Unit>(source, keySelector, elementSelector, _ => Observable.Never<Unit>(), comparer);
#endif
        }

        #endregion

        #region + GroupByUntil +

        public virtual IObservable<IGroupedObservable<TKey, TElement>> GroupByUntil<TSource, TKey, TElement, TDuration>(IObservable<TSource> source, Func<TSource, TKey> keySelector, Func<TSource, TElement> elementSelector, Func<IGroupedObservable<TKey, TElement>, IObservable<TDuration>> durationSelector, IEqualityComparer<TKey> comparer)
        {
            return GroupByUntil_<TSource, TKey, TElement, TDuration>(source, keySelector, elementSelector, durationSelector, comparer);
        }

        public virtual IObservable<IGroupedObservable<TKey, TElement>> GroupByUntil<TSource, TKey, TElement, TDuration>(IObservable<TSource> source, Func<TSource, TKey> keySelector, Func<TSource, TElement> elementSelector, Func<IGroupedObservable<TKey, TElement>, IObservable<TDuration>> durationSelector)
        {
            return GroupByUntil_<TSource, TKey, TElement, TDuration>(source, keySelector, elementSelector, durationSelector, EqualityComparer<TKey>.Default);
        }

        public virtual IObservable<IGroupedObservable<TKey, TSource>> GroupByUntil<TSource, TKey, TDuration>(IObservable<TSource> source, Func<TSource, TKey> keySelector, Func<IGroupedObservable<TKey, TSource>, IObservable<TDuration>> durationSelector, IEqualityComparer<TKey> comparer)
        {
            return GroupByUntil_<TSource, TKey, TSource, TDuration>(source, keySelector, x => x, durationSelector, comparer);
        }

        public virtual IObservable<IGroupedObservable<TKey, TSource>> GroupByUntil<TSource, TKey, TDuration>(IObservable<TSource> source, Func<TSource, TKey> keySelector, Func<IGroupedObservable<TKey, TSource>, IObservable<TDuration>> durationSelector)
        {
            return GroupByUntil_<TSource, TKey, TSource, TDuration>(source, keySelector, x => x, durationSelector, EqualityComparer<TKey>.Default);
        }

        private static IObservable<IGroupedObservable<TKey, TElement>> GroupByUntil_<TSource, TKey, TElement, TDuration>(IObservable<TSource> source, Func<TSource, TKey> keySelector, Func<TSource, TElement> elementSelector, Func<IGroupedObservable<TKey, TElement>, IObservable<TDuration>> durationSelector, IEqualityComparer<TKey> comparer)
        {
#if !NO_PERF
            return new GroupByUntil<TSource, TKey, TElement, TDuration>(source, keySelector, elementSelector, durationSelector, comparer);
#else
            return new AnonymousObservable<IGroupedObservable<TKey, TElement>>(observer =>
            {
                var map = new Dictionary<TKey, ISubject<TElement>>(comparer);

                var groupDisposable = new CompositeDisposable();
                var refCountDisposable = new RefCountDisposable(groupDisposable);

                groupDisposable.Add(source.Subscribe(x =>
                {
                    var key = default(TKey);
                    try
                    {
                        key = keySelector(x);
                    }
                    catch (Exception exception)
                    {
                        lock (map)
                            foreach (var w in map.Values.ToArray())
                                w.OnError(exception);
                        observer.OnError(exception);
                        return;
                    }

                    var fireNewMapEntry = false;
                    var writer = default(ISubject<TElement>);
                    try
                    {
                        lock (map)
                        {
                            if (!map.TryGetValue(key, out writer))
                            {
                                writer = new Subject<TElement>();
                                map.Add(key, writer);
                                fireNewMapEntry = true;
                            }
                        }
                    }
                    catch (Exception exception)
                    {
                        lock (map)
                        {
                            foreach (var w in map.Values.ToArray())
                                w.OnError(exception);
                        }
                        observer.OnError(exception);
                        return;
                    }

                    if (fireNewMapEntry)
                    {
                        var group = new GroupedObservable<TKey, TElement>(key, writer, refCountDisposable);

                        var durationGroup = new GroupedObservable<TKey, TElement>(key, writer);
                        var duration = default(IObservable<TDuration>);
                        try
                        {
                            duration = durationSelector(durationGroup);
                        }
                        catch (Exception exception)
                        {
                            foreach (var w in map.Values.ToArray())
                                w.OnError(exception);
                            observer.OnError(exception);
                            return;
                        }

                        observer.OnNext(group);

                        var md = new SingleAssignmentDisposable();
                        groupDisposable.Add(md);

                        Action expire = () =>
                        {
                            lock (map)
                            {
                                if (map.Remove(key))
                                    writer.OnCompleted();
                            }

                            groupDisposable.Remove(md);
                        };

                        md.Disposable = duration.Take(1).Subscribe(
                                _ => { },
                                exception =>
                                {
                                    lock (map)
                                        foreach (var o in map.Values.ToArray())
                                            o.OnError(exception);
                                    observer.OnError(exception);
                                },
                                expire);
                    }

                    var element = default(TElement);
                    try
                    {
                        element = elementSelector(x);
                    }
                    catch (Exception exception)
                    {
                        lock (map)
                            foreach (var w in map.Values.ToArray())
                                w.OnError(exception);
                        observer.OnError(exception);
                        return;
                    }

                    writer.OnNext(element);
                },
                e =>
                {
                    lock (map)
                        foreach (var w in map.Values.ToArray())
                            w.OnError(e);
                    observer.OnError(e);
                },
                () =>
                {
                    lock (map)
                        foreach (var w in map.Values.ToArray())
                            w.OnCompleted();
                    observer.OnCompleted();
                }));

                return refCountDisposable;
            });
#endif
        }

        #endregion

        #region + GroupJoin +

        public virtual IObservable<TResult> GroupJoin<TLeft, TRight, TLeftDuration, TRightDuration, TResult>(IObservable<TLeft> left, IObservable<TRight> right, Func<TLeft, IObservable<TLeftDuration>> leftDurationSelector, Func<TRight, IObservable<TRightDuration>> rightDurationSelector, Func<TLeft, IObservable<TRight>, TResult> resultSelector)
        {
            return GroupJoin_<TLeft, TRight, TLeftDuration, TRightDuration, TResult>(left, right, leftDurationSelector, rightDurationSelector, resultSelector);
        }

        private static IObservable<TResult> GroupJoin_<TLeft, TRight, TLeftDuration, TRightDuration, TResult>(IObservable<TLeft> left, IObservable<TRight> right, Func<TLeft, IObservable<TLeftDuration>> leftDurationSelector, Func<TRight, IObservable<TRightDuration>> rightDurationSelector, Func<TLeft, IObservable<TRight>, TResult> resultSelector)
        {
#if !NO_PERF
            return new GroupJoin<TLeft, TRight, TLeftDuration, TRightDuration, TResult>(left, right, leftDurationSelector, rightDurationSelector, resultSelector);
#else
            return new AnonymousObservable<TResult>(observer =>
            {
                var gate = new object();
                var group = new CompositeDisposable();
                var r = new RefCountDisposable(group);
                var leftMap = new Dictionary<int, IObserver<TRight>>();
                var rightMap = new Dictionary<int, TRight>();
                var leftID = 0;
                var rightID = 0;

                group.Add(left.Subscribe(
                    value =>
                    {
                        var s = new Subject<TRight>();
                        var id = 0;
                        lock (gate)
                        {
                            id = leftID++;
                            leftMap.Add(id, s);
                        }

                        lock (gate)
                        {
                            var result = default(TResult);
                            try
                            {
                                result = resultSelector(value, s.AddRef(r));
                            }
                            catch (Exception exception)
                            {
                                foreach (var o in leftMap.Values.ToArray())
                                    o.OnError(exception);
                                observer.OnError(exception);
                                return;
                            }
                            observer.OnNext(result);

                            foreach (var rightValue in rightMap.Values.ToArray())
                            {
                                s.OnNext(rightValue);
                            }
                        }

                        var md = new SingleAssignmentDisposable();
                        group.Add(md);

                        Action expire = () =>
                        {
                            lock (gate)
                                if (leftMap.Remove(id))
                                    s.OnCompleted();

                            group.Remove(md);
                        };

                        var duration = default(IObservable<TLeftDuration>);
                        try
                        {
                            duration = leftDurationSelector(value);
                        }
                        catch (Exception exception)
                        {
                            lock (gate)
                            {
                                foreach (var o in leftMap.Values.ToArray())
                                    o.OnError(exception);
                                observer.OnError(exception);
                            }
                            return;
                        }

                        md.Disposable = duration.Take(1).Subscribe(
                                _ => { },
                                exception =>
                                {
                                    lock (gate)
                                    {
                                        foreach (var o in leftMap.Values.ToArray())
                                            o.OnError(exception);
                                        observer.OnError(exception);
                                    }
                                },
                                expire);
                    },
                    exception =>
                    {
                        lock (gate)
                        {
                            foreach (var o in leftMap.Values.ToArray())
                                o.OnError(exception);
                            observer.OnError(exception);
                        }
                    },
                    () =>
                    {
                        lock (gate)
                            observer.OnCompleted();
                    }));

                group.Add(right.Subscribe(
                    value =>
                    {
                        var id = 0;
                        lock (gate)
                        {
                            id = rightID++;
                            rightMap.Add(id, value);
                        }

                        var md = new SingleAssignmentDisposable();
                        group.Add(md);

                        Action expire = () =>
                        {
                            lock (gate)
                                rightMap.Remove(id);

                            group.Remove(md);
                        };

                        var duration = default(IObservable<TRightDuration>);
                        try
                        {
                            duration = rightDurationSelector(value);
                        }
                        catch (Exception exception)
                        {
                            lock (gate)
                            {
                                foreach (var o in leftMap.Values.ToArray())
                                    o.OnError(exception);
                                observer.OnError(exception);
                            }
                            return;
                        }
                        md.Disposable = duration.Take(1).Subscribe(
                                _ => { },
                                exception =>
                                {
                                    lock (gate)
                                    {
                                        foreach (var o in leftMap.Values.ToArray())
                                            o.OnError(exception);
                                        observer.OnError(exception);
                                    }
                                },
                                expire);

                        lock (gate)
                        {
                            foreach (var o in leftMap.Values.ToArray())
                                o.OnNext(value);
                        }
                    },
                    exception =>
                    {
                        lock (gate)
                        {
                            foreach (var o in leftMap.Values.ToArray())
                                o.OnError(exception);
                            observer.OnError(exception);
                        }
                    }));

                return r;
            });
#endif
        }

        #endregion

        #region + Join +

        public virtual IObservable<TResult> Join<TLeft, TRight, TLeftDuration, TRightDuration, TResult>(IObservable<TLeft> left, IObservable<TRight> right, Func<TLeft, IObservable<TLeftDuration>> leftDurationSelector, Func<TRight, IObservable<TRightDuration>> rightDurationSelector, Func<TLeft, TRight, TResult> resultSelector)
        {
            return Join_<TLeft, TRight, TLeftDuration, TRightDuration, TResult>(left, right, leftDurationSelector, rightDurationSelector, resultSelector);
        }

        private static IObservable<TResult> Join_<TLeft, TRight, TLeftDuration, TRightDuration, TResult>(IObservable<TLeft> left, IObservable<TRight> right, Func<TLeft, IObservable<TLeftDuration>> leftDurationSelector, Func<TRight, IObservable<TRightDuration>> rightDurationSelector, Func<TLeft, TRight, TResult> resultSelector)
        {
#if !NO_PERF
            return new Join<TLeft, TRight, TLeftDuration, TRightDuration, TResult>(left, right, leftDurationSelector, rightDurationSelector, resultSelector);
#else
            return new AnonymousObservable<TResult>(observer =>
            {
                var gate = new object();
                var leftDone = false;
                var rightDone = false;
                var group = new CompositeDisposable();
                var leftMap = new Dictionary<int, TLeft>();
                var rightMap = new Dictionary<int, TRight>();
                var leftID = 0;
                var rightID = 0;

                group.Add(left.Subscribe(
                    value =>
                    {
                        var id = 0;
                        lock (gate)
                        {
                            id = leftID++;
                            leftMap.Add(id, value);
                        }

                        var md = new SingleAssignmentDisposable();
                        group.Add(md);

                        Action expire = () =>
                        {
                            lock (gate)
                            {
                                if (leftMap.Remove(id) && leftMap.Count == 0 && leftDone)
                                    observer.OnCompleted();
                            }

                            group.Remove(md);
                        };

                        var duration = default(IObservable<TLeftDuration>);
                        try
                        {
                            duration = leftDurationSelector(value);
                        }
                        catch (Exception exception)
                        {
                            observer.OnError(exception);
                            return;
                        }

                        md.Disposable = duration.Take(1).Subscribe(
                                _ => { },
                                error =>
                                {
                                    lock (gate)
                                        observer.OnError(error);
                                },
                                expire);

                        lock (gate)
                        {
                            foreach (var rightValue in rightMap.Values.ToArray())
                            {
                                var result = default(TResult);
                                try
                                {
                                    result = resultSelector(value, rightValue);
                                }
                                catch (Exception exception)
                                {
                                    observer.OnError(exception);
                                    return;
                                }

                                observer.OnNext(result);
                            }
                        }
                    },
                    error =>
                    {
                        lock (gate)
                            observer.OnError(error);
                    },
                    () =>
                    {
                        lock (gate)
                        {
                            leftDone = true;
                            if (rightDone || leftMap.Count == 0)
                                observer.OnCompleted();
                        }
                    }));

                group.Add(right.Subscribe(
                    value =>
                    {
                        var id = 0;
                        lock (gate)
                        {
                            id = rightID++;
                            rightMap.Add(id, value);
                        }

                        var md = new SingleAssignmentDisposable();
                        group.Add(md);

                        Action expire = () =>
                        {
                            lock (gate)
                            {
                                if (rightMap.Remove(id) && rightMap.Count == 0 && rightDone)
                                    observer.OnCompleted();
                            }

                            group.Remove(md);
                        };

                        var duration = default(IObservable<TRightDuration>);
                        try
                        {
                            duration = rightDurationSelector(value);
                        }
                        catch (Exception exception)
                        {
                            observer.OnError(exception);
                            return;
                        }

                        md.Disposable = duration.Take(1).Subscribe(
                                _ => { },
                                error =>
                                {
                                    lock (gate)
                                        observer.OnError(error);
                                },
                                expire);

                        lock (gate)
                        {
                            foreach (var leftValue in leftMap.Values.ToArray())
                            {
                                var result = default(TResult);
                                try
                                {
                                    result = resultSelector(leftValue, value);
                                }
                                catch (Exception exception)
                                {
                                    observer.OnError(exception);
                                    return;
                                }

                                observer.OnNext(result);
                            }
                        }
                    },
                    error =>
                    {
                        lock (gate)
                            observer.OnError(error);
                    },
                    () =>
                    {
                        lock (gate)
                        {
                            rightDone = true;
                            if (leftDone || rightMap.Count == 0)
                                observer.OnCompleted();
                        }
                    }));

                return group;
            });
#endif
        }

        #endregion

        #region + OfType +

        public virtual IObservable<TResult> OfType<TResult>(IObservable<object> source)
        {
#if !NO_PERF
            return new OfType<object, TResult>(source);
#else
            return source.Where(x => x is TResult).Cast<TResult>();
#endif
        }

        #endregion

        #region + Select +

        public virtual IObservable<TResult> Select<TSource, TResult>(IObservable<TSource> source, Func<TSource, TResult> selector)
        {
#if !NO_PERF
            var select = source as Select<TSource>;
            if (select != null)
                return select.Ω(selector);

            return new Select<TSource, TResult>(source, selector);
#else
            var s = source as SelectObservable<TSource>;
            if (s != null)
                return s.Select(selector);

            return new SelectObservable<TSource, TResult>(source, selector);
#endif
        }

#if NO_PERF
        abstract class SelectObservable<TResult> : ObservableBase<TResult>
        {
            public abstract IObservable<TResult2> Select<TResult2>(Func<TResult, TResult2> selector);
        }

        class SelectObservable<TSource, TResult> : SelectObservable<TResult>
        {
            private readonly IObservable<TSource> _source;
            private readonly Func<TSource, TResult> _selector;

            public SelectObservable(IObservable<TSource> source, Func<TSource, TResult> selector)
            {
                _source = source;
                _selector = selector;
            }

            protected override IDisposable SubscribeCore(IObserver<TResult> observer)
            {
                return _source.Subscribe(new Observer(observer, _selector));
            }

            public override IObservable<TResult2> Select<TResult2>(Func<TResult, TResult2> selector)
            {
                return new SelectObservable<TSource, TResult2>(_source, x => selector(_selector(x)));
            }

            class Observer : ObserverBase<TSource>
            {
                private readonly IObserver<TResult> _observer;
                private readonly Func<TSource, TResult> _selector;

                public Observer(IObserver<TResult> observer, Func<TSource, TResult> selector)
                {
                    _observer = observer;
                    _selector = selector;
                }

                protected override void OnNextCore(TSource value)
                {
                    TResult result;
                    try
                    {
                        result = _selector(value);
                    }
                    catch (Exception exception)
                    {
                        _observer.OnError(exception);
                        return;
                    }
                    _observer.OnNext(result);
                }

                protected override void OnErrorCore(Exception error)
                {
                    _observer.OnError(error);
                }

                protected override void OnCompletedCore()
                {
                    _observer.OnCompleted();
                }
            }
        }
#endif

        public virtual IObservable<TResult> Select<TSource, TResult>(IObservable<TSource> source, Func<TSource, int, TResult> selector)
        {
#if !NO_PERF
            return new Select<TSource, TResult>(source, selector);
#else
            return Defer(() =>
            {
                var index = 0;
                return source.Select(x => selector(x, checked(index++)));
            });
#endif
        }

        #endregion

        #region + SelectMany +

        public virtual IObservable<TOther> SelectMany<TSource, TOther>(IObservable<TSource> source, IObservable<TOther> other)
        {
            return SelectMany_<TSource, TOther>(source, _ => other);
        }

        public virtual IObservable<TResult> SelectMany<TSource, TResult>(IObservable<TSource> source, Func<TSource, IObservable<TResult>> selector)
        {
            return SelectMany_<TSource, TResult>(source, selector);
        }

        public virtual IObservable<TResult> SelectMany<TSource, TResult>(IObservable<TSource> source, Func<TSource, int, IObservable<TResult>> selector)
        {
            return SelectMany_<TSource, TResult>(source, selector);
        }

#if !NO_TPL
        public virtual IObservable<TResult> SelectMany<TSource, TResult>(IObservable<TSource> source, Func<TSource, Task<TResult>> selector)
        {
#if !NO_PERF
            return new SelectMany<TSource, TResult>(source, (x, token) => selector(x));
#else
            return SelectMany_<TSource, TResult>(source, x => selector(x).ToObservable());
#endif
        }

        public virtual IObservable<TResult> SelectMany<TSource, TResult>(IObservable<TSource> source, Func<TSource, CancellationToken, Task<TResult>> selector)
        {
#if !NO_PERF
            return new SelectMany<TSource, TResult>(source, selector);
#else
            return SelectMany_<TSource, TResult>(source, x => FromAsync(ct => selector(x, ct)));
#endif
        }
#endif

        public virtual IObservable<TResult> SelectMany<TSource, TCollection, TResult>(IObservable<TSource> source, Func<TSource, IObservable<TCollection>> collectionSelector, Func<TSource, TCollection, TResult> resultSelector)
        {
            return SelectMany_<TSource, TCollection, TResult>(source, collectionSelector, resultSelector);
        }

        public virtual IObservable<TResult> SelectMany<TSource, TCollection, TResult>(IObservable<TSource> source, Func<TSource, int, IObservable<TCollection>> collectionSelector, Func<TSource, int, TCollection, int, TResult> resultSelector)
        {
            return SelectMany_<TSource, TCollection, TResult>(source, collectionSelector, resultSelector);
        }

#if !NO_TPL
        public virtual IObservable<TResult> SelectMany<TSource, TTaskResult, TResult>(IObservable<TSource> source, Func<TSource, Task<TTaskResult>> taskSelector, Func<TSource, TTaskResult, TResult> resultSelector)
        {
#if !NO_PERF
            return new SelectMany<TSource, TTaskResult, TResult>(source, (x, token) => taskSelector(x), resultSelector);
#else
            return SelectMany_<TSource, TTaskResult, TResult>(source, x => taskSelector(x).ToObservable(), resultSelector);
#endif
        }

        public virtual IObservable<TResult> SelectMany<TSource, TTaskResult, TResult>(IObservable<TSource> source, Func<TSource, CancellationToken, Task<TTaskResult>> taskSelector, Func<TSource, TTaskResult, TResult> resultSelector)
        {
#if !NO_PERF
            return new SelectMany<TSource, TTaskResult, TResult>(source, taskSelector, resultSelector);
#else
            return SelectMany_<TSource, TTaskResult, TResult>(source, x => FromAsync(ct => taskSelector(x, ct)), resultSelector);
#endif
        }
#endif

        private static IObservable<TResult> SelectMany_<TSource, TResult>(IObservable<TSource> source, Func<TSource, IObservable<TResult>> selector)
        {
#if !NO_PERF
            return new SelectMany<TSource, TResult>(source, selector);
#else
            return source.Select(selector).Merge();
#endif
        }

        private static IObservable<TResult> SelectMany_<TSource, TResult>(IObservable<TSource> source, Func<TSource, int, IObservable<TResult>> selector)
        {
#if !NO_PERF
            return new SelectMany<TSource, TResult>(source, selector);
#else
            return source.Select(selector).Merge();
#endif
        }

        private static IObservable<TResult> SelectMany_<TSource, TCollection, TResult>(IObservable<TSource> source, Func<TSource, IObservable<TCollection>> collectionSelector, Func<TSource, TCollection, TResult> resultSelector)
        {
#if !NO_PERF
            return new SelectMany<TSource, TCollection, TResult>(source, collectionSelector, resultSelector);
#else
            return SelectMany_<TSource, TResult>(source, x => collectionSelector(x).Select(y => resultSelector(x, y)));
#endif
        }

        private static IObservable<TResult> SelectMany_<TSource, TCollection, TResult>(IObservable<TSource> source, Func<TSource, int, IObservable<TCollection>> collectionSelector, Func<TSource, int, TCollection, int, TResult> resultSelector)
        {
#if !NO_PERF
            return new SelectMany<TSource, TCollection, TResult>(source, collectionSelector, resultSelector);
#else
            return SelectMany_<TSource, TResult>(source, x => collectionSelector(x).Select(y => resultSelector(x, y)));
#endif
        }

        public virtual IObservable<TResult> SelectMany<TSource, TResult>(IObservable<TSource> source, Func<TSource, IObservable<TResult>> onNext, Func<Exception, IObservable<TResult>> onError, Func<IObservable<TResult>> onCompleted)
        {
#if !NO_PERF
            return new SelectMany<TSource, TResult>(source, onNext, onError, onCompleted);
#else
            return source.Materialize().SelectMany(notification =>
            {
                if (notification.Kind == NotificationKind.OnNext)
                    return onNext(notification.Value);
                else if (notification.Kind == NotificationKind.OnError)
                    return onError(notification.Exception);
                else
                    return onCompleted();
            });
#endif
        }

        public virtual IObservable<TResult> SelectMany<TSource, TResult>(IObservable<TSource> source, Func<TSource, int, IObservable<TResult>> onNext, Func<Exception, int, IObservable<TResult>> onError, Func<int, IObservable<TResult>> onCompleted)
        {
#if !NO_PERF
            return new SelectMany<TSource, TResult>(source, onNext, onError, onCompleted);
#else
            return source.Materialize().SelectMany(notification =>
            {
                if (notification.Kind == NotificationKind.OnNext)
                    return onNext(notification.Value);
                else if (notification.Kind == NotificationKind.OnError)
                    return onError(notification.Exception);
                else
                    return onCompleted();
            });
#endif
        }

        public virtual IObservable<TResult> SelectMany<TSource, TResult>(IObservable<TSource> source, Func<TSource, IEnumerable<TResult>> selector)
        {
#if !NO_PERF
            return new SelectMany<TSource, TResult>(source, selector);
#else
            return SelectMany_<TSource, TResult, TResult>(source, selector, (_, x) => x);
#endif
        }

        public virtual IObservable<TResult> SelectMany<TSource, TResult>(IObservable<TSource> source, Func<TSource, int, IEnumerable<TResult>> selector)
        {
#if !NO_PERF
            return new SelectMany<TSource, TResult>(source, selector);
#else
            return SelectMany_<TSource, TResult, TResult>(source, selector, (_, x) => x);
#endif
        }

        public virtual IObservable<TResult> SelectMany<TSource, TCollection, TResult>(IObservable<TSource> source, Func<TSource, IEnumerable<TCollection>> collectionSelector, Func<TSource, TCollection, TResult> resultSelector)
        {
            return SelectMany_<TSource, TCollection, TResult>(source, collectionSelector, resultSelector);
        }

        private static IObservable<TResult> SelectMany_<TSource, TCollection, TResult>(IObservable<TSource> source, Func<TSource, IEnumerable<TCollection>> collectionSelector, Func<TSource, TCollection, TResult> resultSelector)
        {
#if !NO_PERF
            return new SelectMany<TSource, TCollection, TResult>(source, collectionSelector, resultSelector);
#else
            return new AnonymousObservable<TResult>(observer => 
                source.Subscribe(
                    x =>
                    {
                        var xs = default(IEnumerable<TCollection>);
                        try
                        {
                            xs = collectionSelector(x);
                        }
                        catch (Exception exception)
                        {
                            observer.OnError(exception);
                            return;
                        }

                        var e = xs.GetEnumerator();

                        try
                        {
                            var hasNext = true;
                            while (hasNext)
                            {
                                hasNext = false;
                                var current = default(TResult);

                                try
                                {
                                    hasNext = e.MoveNext();
                                    if (hasNext)
                                        current = resultSelector(x, e.Current);
                                }
                                catch (Exception exception)
                                {
                                    observer.OnError(exception);
                                    return;
                                }

                                if (hasNext)
                                    observer.OnNext(current);
                            }
                        }
                        finally
                        {
                            if (e != null)
                                e.Dispose();
                        }
                    },
                    observer.OnError,
                    observer.OnCompleted
                )
            );
#endif
        }

        public virtual IObservable<TResult> SelectMany<TSource, TCollection, TResult>(IObservable<TSource> source, Func<TSource, int, IEnumerable<TCollection>> collectionSelector, Func<TSource, int, TCollection, int, TResult> resultSelector)
        {
            return new SelectMany<TSource, TCollection, TResult>(source, collectionSelector, resultSelector);
        }

        #endregion

        #region + Skip +

        public virtual IObservable<TSource> Skip<TSource>(IObservable<TSource> source, int count)
        {
#if !NO_PERF
            var skip = source as Skip<TSource>;
            if (skip != null && skip._scheduler == null)
                return skip.Ω(count);

            return new Skip<TSource>(source, count);
#else
            return new AnonymousObservable<TSource>(observer =>
            {
                var remaining = count;
                return source.Subscribe(
                    x =>
                    {
                        if (remaining <= 0)
                            observer.OnNext(x);
                        else
                            remaining--;
                    },
                    observer.OnError,
                    observer.OnCompleted);
            });
#endif
        }

        #endregion

        #region + SkipWhile +

        public virtual IObservable<TSource> SkipWhile<TSource>(IObservable<TSource> source, Func<TSource, bool> predicate)
        {
#if !NO_PERF
            return new SkipWhile<TSource>(source, predicate);
#else
            return SkipWhile_(source, (x, i) => predicate(x));
#endif
        }

        public virtual IObservable<TSource> SkipWhile<TSource>(IObservable<TSource> source, Func<TSource, int, bool> predicate)
        {
#if !NO_PERF
            return new SkipWhile<TSource>(source, predicate);
#else
            return SkipWhile_(source, predicate);
#endif
        }

#if NO_PERF
        private static IObservable<TSource> SkipWhile_<TSource>(IObservable<TSource> source, Func<TSource, int, bool> predicate)
        {
            return new AnonymousObservable<TSource>(observer =>
            {
                var running = false;
                var i = 0;
                return source.Subscribe(
                    x =>
                    {
                        if (!running)
                            try
                            {
                                running = !predicate(x, checked(i++));
                            }
                            catch (Exception exception)
                            {
                                observer.OnError(exception);
                                return;
                            }
                        if (running)
                            observer.OnNext(x);
                    },
                    observer.OnError,
                    observer.OnCompleted);
            });
        }
#endif

        #endregion

        #region + Take +

        public virtual IObservable<TSource> Take<TSource>(IObservable<TSource> source, int count)
        {
            if (count == 0)
                return Empty<TSource>();

            return Take_(source, count);
        }

        public virtual IObservable<TSource> Take<TSource>(IObservable<TSource> source, int count, IScheduler scheduler)
        {
            if (count == 0)
                return Empty<TSource>(scheduler);

            return Take_(source, count);
        }

#if !NO_PERF
        private static IObservable<TSource> Take_<TSource>(IObservable<TSource> source, int count)
        {
            var take = source as Take<TSource>;
            if (take != null && take._scheduler == null)
                return take.Ω(count);

            return new Take<TSource>(source, count);
        }
#else
        private static IObservable<TSource> Take_<TSource>(IObservable<TSource> source, int count)
        {
            return new AnonymousObservable<TSource>(observer =>
            {
                var remaining = count;

                return source.Subscribe(
                    x =>
                    {
                        if (remaining > 0)
                        {
                            --remaining;
                            observer.OnNext(x);
                            if (remaining == 0)
                                observer.OnCompleted();
                        }
                    },
                    observer.OnError,
                    observer.OnCompleted);
            });
        }
#endif

        #endregion

        #region + TakeWhile +

        public virtual IObservable<TSource> TakeWhile<TSource>(IObservable<TSource> source, Func<TSource, bool> predicate)
        {
#if !NO_PERF
            return new TakeWhile<TSource>(source, predicate);
#else
            return TakeWhile_(source, (x, i) => predicate(x));
#endif
        }

        public virtual IObservable<TSource> TakeWhile<TSource>(IObservable<TSource> source, Func<TSource, int, bool> predicate)
        {
#if !NO_PERF
            return new TakeWhile<TSource>(source, predicate);
#else
            return TakeWhile_(source, predicate);
#endif
        }

#if NO_PERF
        private static IObservable<TSource> TakeWhile_<TSource>(IObservable<TSource> source, Func<TSource, int, bool> predicate)
        {
            return new AnonymousObservable<TSource>(observer =>
            {
                var running = true;
                var i = 0;
                return source.Subscribe(
                    x =>
                    {
                        if (running)
                        {
                            try
                            {
                                running = predicate(x, checked(i++));
                            }
                            catch (Exception exception)
                            {
                                observer.OnError(exception);
                                return;
                            }
                            if (running)
                                observer.OnNext(x);
                            else
                                observer.OnCompleted();
                        }
                    },
                    observer.OnError,
                    observer.OnCompleted);
            });
        }
#endif

        #endregion

        #region + Where +

        public virtual IObservable<TSource> Where<TSource>(IObservable<TSource> source, Func<TSource, bool> predicate)
        {
#if !NO_PERF
            var where = source as Where<TSource>;
            if (where != null)
                return where.Ω(predicate);

            return new Where<TSource>(source, predicate);
#else
            var w = source as WhereObservable<TSource>;
            if (w != null)
                return w.Where(predicate);

            return new WhereObservable<TSource>(source, predicate);
#endif
        }

#if NO_PERF
        class WhereObservable<TSource> : ObservableBase<TSource>
        {
            private readonly IObservable<TSource> _source;
            private readonly Func<TSource, bool> _predicate;

            public WhereObservable(IObservable<TSource> source, Func<TSource, bool> predicate)
            {
                _source = source;
                _predicate = predicate;
            }

            protected override IDisposable SubscribeCore(IObserver<TSource> observer)
            {
                return _source.Subscribe(new Observer(observer, _predicate));
            }

            public IObservable<TSource> Where(Func<TSource, bool> predicate)
            {
                return new WhereObservable<TSource>(_source, x => _predicate(x) && predicate(x));
            }

            class Observer : ObserverBase<TSource>
            {
                private readonly IObserver<TSource> _observer;
                private readonly Func<TSource, bool> _predicate;

                public Observer(IObserver<TSource> observer, Func<TSource, bool> predicate)
                {
                    _observer = observer;
                    _predicate = predicate;
                }

                protected override void OnNextCore(TSource value)
                {
                    bool shouldRun;
                    try
                    {
                        shouldRun = _predicate(value);
                    }
                    catch (Exception exception)
                    {
                        _observer.OnError(exception);
                        return;
                    }
                    if (shouldRun)
                        _observer.OnNext(value);
                }

                protected override void OnErrorCore(Exception error)
                {
                    _observer.OnError(error);
                }

                protected override void OnCompletedCore()
                {
                    _observer.OnCompleted();
                }
            }
        }
#endif

        public virtual IObservable<TSource> Where<TSource>(IObservable<TSource> source, Func<TSource, int, bool> predicate)
        {
#if !NO_PERF
            return new Where<TSource>(source, predicate);
#else
            return Defer(() =>
            {
                var index = 0;
                return source.Where(x => predicate(x, checked(index++)));
            });
#endif
        }

        #endregion
    }
}

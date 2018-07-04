// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the Apache 2.0 License.
// See the LICENSE file in the project root for more information. 

using System.Collections.Generic;
using System.Linq;
using System.Reactive.Concurrency;
using System.Reactive.Disposables;
using System.Reactive.Subjects;

namespace System.Reactive.Linq
{
    internal class QueryLanguageEx : IQueryLanguageEx
    {
        #region Create

        public virtual IObservable<TResult> Create<TResult>(Func<IObserver<TResult>, IEnumerable<IObservable<object>>> iteratorMethod)
        {
            return new CreateWithEnumerableObservable<TResult>(iteratorMethod);
        }

        private sealed class CreateWithEnumerableObservable<TResult> : ObservableBase<TResult>
        {
            private readonly Func<IObserver<TResult>, IEnumerable<IObservable<object>>> _iteratorMethod;

            public CreateWithEnumerableObservable(Func<IObserver<TResult>, IEnumerable<IObservable<object>>> iteratorMethod)
            {
                _iteratorMethod = iteratorMethod;
            }

            protected override IDisposable SubscribeCore(IObserver<TResult> observer)
            {
                return _iteratorMethod(observer)
                    .Concat()
                    .Subscribe(new TerminalOnlyObserver<TResult>(observer));
            }
        }

        private sealed class TerminalOnlyObserver<TResult> : IObserver<object>
        {
            private readonly IObserver<TResult> _observer;

            public TerminalOnlyObserver(IObserver<TResult> observer)
            {
                _observer = observer;
            }

            public void OnCompleted()
            {
                _observer.OnCompleted();
            }

            public void OnError(Exception error)
            {
                _observer.OnError(error);
            }

            public void OnNext(object value)
            {
                // deliberately ignored
            }
        }

        public virtual IObservable<Unit> Create(Func<IEnumerable<IObservable<object>>> iteratorMethod)
        {
            return new CreateWithOnlyEnumerableObservable<Unit>(iteratorMethod);
        }

        private sealed class CreateWithOnlyEnumerableObservable<TResult> : ObservableBase<TResult>
        {
            private readonly Func<IEnumerable<IObservable<object>>> _iteratorMethod;

            public CreateWithOnlyEnumerableObservable(Func<IEnumerable<IObservable<object>>> iteratorMethod)
            {
                _iteratorMethod = iteratorMethod;
            }

            protected override IDisposable SubscribeCore(IObserver<TResult> observer)
            {
                return _iteratorMethod()
                    .Concat()
                    .Subscribe(new TerminalOnlyObserver<TResult>(observer));
            }
        }

        #endregion

        #region Expand

        public virtual IObservable<TSource> Expand<TSource>(IObservable<TSource> source, Func<TSource, IObservable<TSource>> selector, IScheduler scheduler)
        {
            return new ExpandObservable<TSource>(source, selector, scheduler);
        }

        private sealed class ExpandObservable<TSource> : ObservableBase<TSource>
        {
            private readonly IObservable<TSource> _source;
            private readonly Func<TSource, IObservable<TSource>> _selector;
            private readonly IScheduler _scheduler;

            public ExpandObservable(IObservable<TSource> source, Func<TSource, IObservable<TSource>> selector, IScheduler scheduler)
            {
                _source = source;
                _selector = selector;
                _scheduler = scheduler;
            }

            protected override IDisposable SubscribeCore(IObserver<TSource> observer)
            {
                var outGate = new object();
                var q = new Queue<IObservable<TSource>>();
                var m = new SerialDisposable();
                var d = new CompositeDisposable { m };
                var activeCount = 0;
                var isAcquired = false;

                void ensureActive()
                {
                    var isOwner = false;

                    lock (q)
                    {
                        if (q.Count > 0)
                        {
                            isOwner = !isAcquired;
                            isAcquired = true;
                        }
                    }

                    if (isOwner)
                    {
                        m.Disposable = _scheduler.Schedule(self =>
                        {
                            var work = default(IObservable<TSource>);

                            lock (q)
                            {
                                if (q.Count > 0)
                                {
                                    work = q.Dequeue();
                                }
                                else
                                {
                                    isAcquired = false;
                                    return;
                                }
                            }

                            var m1 = new SingleAssignmentDisposable();
                            d.Add(m1);
                            m1.Disposable = work.Subscribe(
                                x =>
                                {
                                    lock (outGate)
                                    {
                                        observer.OnNext(x);
                                    }

                                    var result = default(IObservable<TSource>);
                                    try
                                    {
                                        result = _selector(x);
                                    }
                                    catch (Exception exception)
                                    {
                                        lock (outGate)
                                        {
                                            observer.OnError(exception);
                                        }
                                    }

                                    lock (q)
                                    {
                                        q.Enqueue(result);
                                        activeCount++;
                                    }

                                    ensureActive();
                                },
                                exception =>
                                {
                                    lock (outGate)
                                    {
                                        observer.OnError(exception);
                                    }
                                },
                                () =>
                                {
                                    d.Remove(m1);

                                    var done = false;
                                    lock (q)
                                    {
                                        activeCount--;
                                        if (activeCount == 0)
                                        {
                                            done = true;
                                        }
                                    }
                                    if (done)
                                    {
                                        lock (outGate)
                                        {
                                            observer.OnCompleted();
                                        }
                                    }
                                });
                            self();
                        });
                    }
                }

                lock (q)
                {
                    q.Enqueue(_source);
                    activeCount++;
                }
                ensureActive();

                return d;
            }
        }

        public virtual IObservable<TSource> Expand<TSource>(IObservable<TSource> source, Func<TSource, IObservable<TSource>> selector)
        {
            return source.Expand(selector, SchedulerDefaults.Iteration);
        }

        #endregion

        #region ForkJoin

        public virtual IObservable<TResult> ForkJoin<TFirst, TSecond, TResult>(IObservable<TFirst> first, IObservable<TSecond> second, Func<TFirst, TSecond, TResult> resultSelector)
        {
            return Combine<TFirst, TSecond, TResult>(first, second, (observer, leftSubscription, rightSubscription) =>
            {
                var leftStopped = false;
                var rightStopped = false;
                var hasLeft = false;
                var hasRight = false;
                var lastLeft = default(TFirst);
                var lastRight = default(TSecond);

                return new BinaryObserver<TFirst, TSecond>(
                    left =>
                    {
                        switch (left.Kind)
                        {
                            case NotificationKind.OnNext:
                                hasLeft = true;
                                lastLeft = left.Value;
                                break;
                            case NotificationKind.OnError:
                                rightSubscription.Dispose();
                                observer.OnError(left.Exception);
                                break;
                            case NotificationKind.OnCompleted:
                                leftStopped = true;
                                if (rightStopped)
                                {
                                    if (!hasLeft)
                                    {
                                        observer.OnCompleted();
                                    }
                                    else if (!hasRight)
                                    {
                                        observer.OnCompleted();
                                    }
                                    else
                                    {
                                        TResult result;
                                        try
                                        {
                                            result = resultSelector(lastLeft, lastRight);
                                        }
                                        catch (Exception exception)
                                        {
                                            observer.OnError(exception);
                                            return;
                                        }
                                        observer.OnNext(result);
                                        observer.OnCompleted();
                                    }
                                }
                                break;
                        }
                    },
                    right =>
                    {
                        switch (right.Kind)
                        {
                            case NotificationKind.OnNext:
                                hasRight = true;
                                lastRight = right.Value;
                                break;
                            case NotificationKind.OnError:
                                leftSubscription.Dispose();
                                observer.OnError(right.Exception);
                                break;
                            case NotificationKind.OnCompleted:
                                rightStopped = true;
                                if (leftStopped)
                                {
                                    if (!hasLeft)
                                    {
                                        observer.OnCompleted();
                                    }
                                    else if (!hasRight)
                                    {
                                        observer.OnCompleted();
                                    }
                                    else
                                    {
                                        TResult result;
                                        try
                                        {
                                            result = resultSelector(lastLeft, lastRight);
                                        }
                                        catch (Exception exception)
                                        {
                                            observer.OnError(exception);
                                            return;
                                        }
                                        observer.OnNext(result);
                                        observer.OnCompleted();
                                    }
                                }
                                break;
                        }
                    });
            });
        }

        public virtual IObservable<TSource[]> ForkJoin<TSource>(params IObservable<TSource>[] sources)
        {
            return sources.ForkJoin();
        }

        public virtual IObservable<TSource[]> ForkJoin<TSource>(IEnumerable<IObservable<TSource>> sources)
        {
            return new ForkJoinObservable<TSource>(sources);
        }

        private sealed class ForkJoinObservable<TSource> : ObservableBase<TSource[]>
        {
            private readonly IEnumerable<IObservable<TSource>> _sources;

            public ForkJoinObservable(IEnumerable<IObservable<TSource>> sources)
            {
                _sources = sources;
            }

            protected override IDisposable SubscribeCore(IObserver<TSource[]> observer)
            {
                var allSources = _sources.ToArray();
                var count = allSources.Length;

                if (count == 0)
                {
                    observer.OnCompleted();
                    return Disposable.Empty;
                }

                var group = new CompositeDisposable(allSources.Length);
                var gate = new object();

                var finished = false;
                var hasResults = new bool[count];
                var hasCompleted = new bool[count];
                var results = new List<TSource>(count);

                lock (gate)
                {
                    for (var index = 0; index < count; index++)
                    {
                        var currentIndex = index;
                        var source = allSources[index];
                        results.Add(default);
                        group.Add(source.Subscribe(
                            value =>
                            {
                                lock (gate)
                                {
                                    if (!finished)
                                    {
                                        hasResults[currentIndex] = true;
                                        results[currentIndex] = value;
                                    }
                                }
                            },
                            error =>
                            {
                                lock (gate)
                                {
                                    finished = true;
                                    observer.OnError(error);
                                    group.Dispose();
                                }
                            },
                            () =>
                            {
                                lock (gate)
                                {
                                    if (!finished)
                                    {
                                        if (!hasResults[currentIndex])
                                        {
                                            observer.OnCompleted();
                                            return;
                                        }
                                        hasCompleted[currentIndex] = true;
                                        foreach (var completed in hasCompleted)
                                        {
                                            if (!completed)
                                            {
                                                return;
                                            }
                                        }
                                        finished = true;
                                        observer.OnNext(results.ToArray());
                                        observer.OnCompleted();
                                    }
                                }
                            }));
                    }
                }
                return group;
            }
        }

        #endregion

        #region Let

        public virtual IObservable<TResult> Let<TSource, TResult>(IObservable<TSource> source, Func<IObservable<TSource>, IObservable<TResult>> function)
        {
            return function(source);
        }

        #endregion

        #region ManySelect

        public virtual IObservable<TResult> ManySelect<TSource, TResult>(IObservable<TSource> source, Func<IObservable<TSource>, TResult> selector)
        {
            return ManySelect(source, selector, DefaultScheduler.Instance);
        }

        public virtual IObservable<TResult> ManySelect<TSource, TResult>(IObservable<TSource> source, Func<IObservable<TSource>, TResult> selector, IScheduler scheduler)
        {
            return Observable.Defer(() =>
            {
                var chain = default(ChainObservable<TSource>);

                return source
                    .Select(
                        x =>
                        {
                            var curr = new ChainObservable<TSource>(x);

                            chain?.OnNext(curr);

                            chain = curr;

                            return (IObservable<TSource>)curr;
                        })
                    .Do(
                        _ => { },
                        exception =>
                        {
                            chain?.OnError(exception);
                        },
                        () =>
                        {
                            chain?.OnCompleted();
                        })
                    .ObserveOn(scheduler)
                    .Select(selector);
            });
        }

        private class ChainObservable<T> : ISubject<IObservable<T>, T>
        {
            private readonly T _head;
            private readonly AsyncSubject<IObservable<T>> _tail = new AsyncSubject<IObservable<T>>();

            public ChainObservable(T head)
            {
                _head = head;
            }

            public IDisposable Subscribe(IObserver<T> observer)
            {
                var g = new CompositeDisposable();
                g.Add(CurrentThreadScheduler.Instance.ScheduleAction((observer, g, @this: this),
                state =>
                {
                    state.observer.OnNext(state.@this._head);
                    state.g.Add(state.@this._tail.Merge().Subscribe(state.observer));
                }));
                return g;
            }

            public void OnCompleted()
            {
                OnNext(Observable.Empty<T>());
            }

            public void OnError(Exception error)
            {
                OnNext(Observable.Throw<T>(error));
            }

            public void OnNext(IObservable<T> value)
            {
                _tail.OnNext(value);
                _tail.OnCompleted();
            }
        }

        #endregion

        #region ToListObservable

        public virtual ListObservable<TSource> ToListObservable<TSource>(IObservable<TSource> source)
        {
            return new ListObservable<TSource>(source);
        }

        #endregion

        #region |> Helpers <|

        private static IObservable<TResult> Combine<TLeft, TRight, TResult>(IObservable<TLeft> leftSource, IObservable<TRight> rightSource, Func<IObserver<TResult>, IDisposable, IDisposable, IObserver<Either<Notification<TLeft>, Notification<TRight>>>> combinerSelector)
        {
            return new CombineObservable<TLeft, TRight, TResult>(leftSource, rightSource, combinerSelector);
        }

        private sealed class CombineObservable<TLeft, TRight, TResult> : ObservableBase<TResult>
        {
            private readonly IObservable<TLeft> _leftSource;
            private readonly IObservable<TRight> _rightSource;
            private readonly Func<IObserver<TResult>, IDisposable, IDisposable, IObserver<Either<Notification<TLeft>, Notification<TRight>>>> _combinerSelector;

            public CombineObservable(IObservable<TLeft> leftSource, IObservable<TRight> rightSource, Func<IObserver<TResult>, IDisposable, IDisposable, IObserver<Either<Notification<TLeft>, Notification<TRight>>>> combinerSelector)
            {
                _leftSource = leftSource;
                _rightSource = rightSource;
                _combinerSelector = combinerSelector;
            }

            protected override IDisposable SubscribeCore(IObserver<TResult> observer)
            {
                var leftSubscription = new SingleAssignmentDisposable();
                var rightSubscription = new SingleAssignmentDisposable();

                var combiner = _combinerSelector(observer, leftSubscription, rightSubscription);
                var gate = new object();

                leftSubscription.Disposable = _leftSource.Materialize().Select(x => Either<Notification<TLeft>, Notification<TRight>>.CreateLeft(x)).Synchronize(gate).Subscribe(combiner);
                rightSubscription.Disposable = _rightSource.Materialize().Select(x => Either<Notification<TLeft>, Notification<TRight>>.CreateRight(x)).Synchronize(gate).Subscribe(combiner);

                return StableCompositeDisposable.Create(leftSubscription, rightSubscription);

            }
        }

        #endregion
    }
}

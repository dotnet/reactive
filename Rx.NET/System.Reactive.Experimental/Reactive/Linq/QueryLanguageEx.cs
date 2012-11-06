// Copyright (c) Microsoft Open Technologies, Inc. All rights reserved. See License.txt in the project root for license information.

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
            return new AnonymousObservable<TResult>(observer =>
                iteratorMethod(observer).Concat().Subscribe(_ => { }, observer.OnError, observer.OnCompleted));
        }

        public virtual IObservable<Unit> Create(Func<IEnumerable<IObservable<object>>> iteratorMethod)
        {
            return new AnonymousObservable<Unit>(observer =>
                iteratorMethod().Concat().Subscribe(_ => { }, observer.OnError, observer.OnCompleted));
        }

        #endregion

        #region Expand

        public virtual IObservable<TSource> Expand<TSource>(IObservable<TSource> source, Func<TSource, IObservable<TSource>> selector, IScheduler scheduler)
        {
            return new AnonymousObservable<TSource>(observer =>
            {
                var outGate = new object();
                var q = new Queue<IObservable<TSource>>();
                var m = new SerialDisposable();
                var d = new CompositeDisposable { m };
                var activeCount = 0;
                var isAcquired = false;

                var ensureActive = default(Action);

                ensureActive = () =>
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
                        m.Disposable = scheduler.Schedule(self =>
                        {
                            var work = default(IObservable<TSource>);

                            lock (q)
                            {
                                if (q.Count > 0)
                                    work = q.Dequeue();
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
                                        observer.OnNext(x);

                                    var result = default(IObservable<TSource>);
                                    try
                                    {
                                        result = selector(x);
                                    }
                                    catch (Exception exception)
                                    {
                                        lock (outGate)
                                            observer.OnError(exception);
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
                                        observer.OnError(exception);
                                },
                                () =>
                                {
                                    d.Remove(m1);

                                    var done = false;
                                    lock (q)
                                    {
                                        activeCount--;
                                        if (activeCount == 0)
                                            done = true;
                                    }
                                    if (done)
                                        lock (outGate)
                                            observer.OnCompleted();
                                });
                            self();
                        });
                    }
                };

                lock (q)
                {
                    q.Enqueue(source);
                    activeCount++;
                }
                ensureActive();

                return d;
            });
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
                                        observer.OnCompleted();
                                    else if (!hasRight)
                                        observer.OnCompleted();
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
                                        observer.OnCompleted();
                                    else if (!hasRight)
                                        observer.OnCompleted();
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
            return new AnonymousObservable<TSource[]>(subscriber =>
            {
                var allSources = sources.ToArray();
                var count = allSources.Length;

                if (count == 0)
                {
                    subscriber.OnCompleted();
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
                        results.Add(default(TSource));
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
                                    subscriber.OnError(error);
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
                                            subscriber.OnCompleted();
                                            return;
                                        }
                                        hasCompleted[currentIndex] = true;
                                        foreach (var completed in hasCompleted)
                                        {
                                            if (!completed)
                                                return;
                                        }
                                        finished = true;
                                        subscriber.OnNext(results.ToArray());
                                        subscriber.OnCompleted();
                                    }
                                }
                            }));
                    }
                }
                return group;
            });
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
            return Observable.Defer<TResult>(() =>
            {
                var chain = default(ChainObservable<TSource>);

                return source
                    .Select(
                        x =>
                        {
                            var curr = new ChainObservable<TSource>(x);

                            if (chain != null)
                                chain.OnNext(curr);
                            chain = curr;

                            return (IObservable<TSource>)curr;
                        })
                    .Do(
                        _ => { },
                        exception =>
                        {
                            if (chain != null)
                                chain.OnError(exception);
                        },
                        () =>
                        {
                            if (chain != null)
                                chain.OnCompleted();
                        })
                    .ObserveOn(scheduler)
                    .Select(selector);
            });
        }

        class ChainObservable<T> : ISubject<IObservable<T>, T>
        {
            T head;
            AsyncSubject<IObservable<T>> tail = new AsyncSubject<IObservable<T>>();

            public ChainObservable(T head)
            {
                this.head = head;
            }

            public IDisposable Subscribe(IObserver<T> observer)
            {
                var g = new CompositeDisposable();
                g.Add(CurrentThreadScheduler.Instance.Schedule(() =>
                {
                    observer.OnNext(head);
                    g.Add(tail.Merge().Subscribe(observer));
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
                tail.OnNext(value);
                tail.OnCompleted();
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
            return new AnonymousObservable<TResult>(observer =>
            {
                var leftSubscription = new SingleAssignmentDisposable();
                var rightSubscription = new SingleAssignmentDisposable();

                var combiner = combinerSelector(observer, leftSubscription, rightSubscription);
                var gate = new object();

                leftSubscription.Disposable = leftSource.Materialize().Select(x => Either<Notification<TLeft>, Notification<TRight>>.CreateLeft(x)).Synchronize(gate).Subscribe(combiner);
                rightSubscription.Disposable = rightSource.Materialize().Select(x => Either<Notification<TLeft>, Notification<TRight>>.CreateRight(x)).Synchronize(gate).Subscribe(combiner);

                return new CompositeDisposable(leftSubscription, rightSubscription);
            });
        }

        #endregion
    }
}

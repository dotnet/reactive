// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the Apache 2.0 License.
// See the LICENSE file in the project root for more information. 

using System.Collections.Generic;
using System.Reactive.Concurrency;
using System.Reactive.Disposables;
using System.Reactive.Subjects;
using System.Linq;

namespace System.Reactive.Linq
{
    /// <summary>
    /// Provides a set of static methods for writing in-memory queries over observable sequences.
    /// </summary>
    public static class ObservableEx
    {

        #region Create

        /// <summary>
        /// Subscribes to each observable sequence returned by the iteratorMethod in sequence and returns the observable sequence of values sent to the observer given to the iteratorMethod.
        /// </summary>
        /// <typeparam name="TResult">The type of the elements in the produced sequence.</typeparam>
        /// <param name="iteratorMethod">Iterator method that produces elements in the resulting sequence by calling the given observer.</param>
        /// <returns>An observable sequence obtained by running the iterator and returning the elements that were sent to the observer.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="iteratorMethod"/> is null.</exception>
        [Experimental]
        public static IObservable<TResult> Create<TResult>(Func<IObserver<TResult>, IEnumerable<IObservable<object>>> iteratorMethod)
        {
            if (iteratorMethod == null)
                throw new ArgumentNullException(nameof(iteratorMethod));

            return new AnonymousObservable<TResult>(observer =>
                iteratorMethod(observer).Concat().Subscribe(_ => { }, observer.OnError, observer.OnCompleted));
        }

        /// <summary>
        /// Subscribes to each observable sequence returned by the iteratorMethod in sequence and produces a Unit value on the resulting sequence for each step of the iteration.
        /// </summary>
        /// <param name="iteratorMethod">Iterator method that drives the resulting observable sequence.</param>
        /// <returns>An observable sequence obtained by running the iterator and returning Unit values for each iteration step.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="iteratorMethod"/> is null.</exception>
        [Experimental]
        public static IObservable<Unit> Create(Func<IEnumerable<IObservable<object>>> iteratorMethod)
        {
            if (iteratorMethod == null)
                throw new ArgumentNullException(nameof(iteratorMethod));

            return new AnonymousObservable<Unit>(observer =>
                iteratorMethod().Concat().Subscribe(_ => { }, observer.OnError, observer.OnCompleted));
        }

        #endregion

        #region Expand

        /// <summary>
        /// Expands an observable sequence by recursively invoking selector, using the specified scheduler to enumerate the queue of obtained sequences.
        /// </summary>
        /// <typeparam name="TSource">The type of the elements in the source sequence and each of the recursively expanded sources obtained by running the selector function.</typeparam>
        /// <param name="source">Source sequence with the initial elements.</param>
        /// <param name="selector">Selector function to invoke for each produced element, resulting in another sequence to which the selector will be invoked recursively again.</param>
        /// <param name="scheduler">Scheduler on which to perform the expansion by enumerating the internal queue of obtained sequences.</param>
        /// <returns>An observable sequence containing all the elements produced by the recursive expansion.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="source"/> or <paramref name="selector"/> or <paramref name="scheduler"/> is null.</exception>
        [Experimental]
        public static IObservable<TSource> Expand<TSource>(this IObservable<TSource> source, Func<TSource, IObservable<TSource>> selector, IScheduler scheduler)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));
            if (selector == null)
                throw new ArgumentNullException(nameof(selector));
            if (scheduler == null)
                throw new ArgumentNullException(nameof(scheduler));

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

        /// <summary>
        /// Expands an observable sequence by recursively invoking selector.
        /// </summary>
        /// <typeparam name="TSource">The type of the elements in the source sequence and each of the recursively expanded sources obtained by running the selector function.</typeparam>
        /// <param name="source">Source sequence with the initial elements.</param>
        /// <param name="selector">Selector function to invoke for each produced element, resulting in another sequence to which the selector will be invoked recursively again.</param>
        /// <returns>An observable sequence containing all the elements produced by the recursive expansion.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="source"/> or <paramref name="selector"/> is null.</exception>
        [Experimental]
        public static IObservable<TSource> Expand<TSource>(this IObservable<TSource> source, Func<TSource, IObservable<TSource>> selector)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));
            if (selector == null)
                throw new ArgumentNullException(nameof(selector));

            return source.Expand(selector, SchedulerDefaults.Iteration);
        }

        #endregion

        #region ForkJoin

        /// <summary>
        /// Runs two observable sequences in parallel and combines their last elements.
        /// </summary>
        /// <typeparam name="TSource1">The type of the elements in the first source sequence.</typeparam>
        /// <typeparam name="TSource2">The type of the elements in the second source sequence.</typeparam>
        /// <typeparam name="TResult">The type of the elements in the result sequence, returned by the selector function.</typeparam>
        /// <param name="first">First observable sequence.</param>
        /// <param name="second">Second observable sequence.</param>
        /// <param name="resultSelector">Result selector function to invoke with the last elements of both sequences.</param>
        /// <returns>An observable sequence with the result of calling the selector function with the last elements of both input sequences.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="first"/> or <paramref name="second"/> or <paramref name="resultSelector"/> is null.</exception>
        [Experimental]
        public static IObservable<TResult> ForkJoin<TSource1, TSource2, TResult>(this IObservable<TSource1> first, IObservable<TSource2> second, Func<TSource1, TSource2, TResult> resultSelector)
        {
            if (first == null)
                throw new ArgumentNullException(nameof(first));
            if (second == null)
                throw new ArgumentNullException(nameof(second));
            if (resultSelector == null)
                throw new ArgumentNullException(nameof(resultSelector));

            return Combine<TSource1, TSource2, TResult>(first, second, (observer, leftSubscription, rightSubscription) =>
            {
                var leftStopped = false;
                var rightStopped = false;
                var hasLeft = false;
                var hasRight = false;
                var lastLeft = default(TSource1);
                var lastRight = default(TSource2);

                return new BinaryObserver<TSource1, TSource2>(
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

        /// <summary>
        /// Runs all specified observable sequences in parallel and collects their last elements.
        /// </summary>
        /// <typeparam name="TSource">The type of the elements in the source sequences.</typeparam>
        /// <param name="sources">Observable sequence to collect the last elements for.</param>
        /// <returns>An observable sequence with an array collecting the last elements of all the input sequences.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="sources"/> is null.</exception>
        [Experimental]
        public static IObservable<TSource[]> ForkJoin<TSource>(params IObservable<TSource>[] sources)
        {
            if (sources == null)
                throw new ArgumentNullException(nameof(sources));

            return sources.ForkJoin();
        }

        /// <summary>
        /// Runs all observable sequences in the enumerable sources sequence in parallel and collect their last elements.
        /// </summary>
        /// <typeparam name="TSource">The type of the elements in the source sequences.</typeparam>
        /// <param name="sources">Observable sequence to collect the last elements for.</param>
        /// <returns>An observable sequence with an array collecting the last elements of all the input sequences.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="sources"/> is null.</exception>
        [Experimental]
        public static IObservable<TSource[]> ForkJoin<TSource>(this IEnumerable<IObservable<TSource>> sources)
        {
            if (sources == null)
                throw new ArgumentNullException(nameof(sources));

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

        /// <summary>
        /// Returns an observable sequence that is the result of invoking the selector on the source sequence, without sharing subscriptions.
        /// This operator allows for a fluent style of writing queries that use the same sequence multiple times.
        /// </summary>
        /// <typeparam name="TSource">The type of the elements in the source sequence.</typeparam>
        /// <typeparam name="TResult">The type of the elements in the result sequence.</typeparam>
        /// <param name="source">Source sequence that will be shared in the selector function.</param>
        /// <param name="selector">Selector function which can use the source sequence as many times as needed, without sharing subscriptions to the source sequence.</param>
        /// <returns>An observable sequence that contains the elements of a sequence produced by multicasting the source sequence within a selector function.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="source"/> or <paramref name="selector"/> is null.</exception>
        [Experimental]
        public static IObservable<TResult> Let<TSource, TResult>(this IObservable<TSource> source, Func<IObservable<TSource>, IObservable<TResult>> selector)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));
            if (selector == null)
                throw new ArgumentNullException(nameof(selector));

            return selector(source);
        }

        #endregion

        #region ManySelect

        /// <summary>
        /// Comonadic bind operator.
        /// </summary>
        [Experimental]
        public static IObservable<TResult> ManySelect<TSource, TResult>(this IObservable<TSource> source, Func<IObservable<TSource>, TResult> selector, IScheduler scheduler)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));
            if (selector == null)
                throw new ArgumentNullException(nameof(selector));
            if (scheduler == null)
                throw new ArgumentNullException(nameof(scheduler));

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

        /// <summary>
        /// Comonadic bind operator.
        /// </summary>
        [Experimental]
        public static IObservable<TResult> ManySelect<TSource, TResult>(this IObservable<TSource> source, Func<IObservable<TSource>, TResult> selector)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));
            if (selector == null)
                throw new ArgumentNullException(nameof(selector));

            return ManySelect(source, selector, DefaultScheduler.Instance);
        }

        #endregion

        #region ToListObservable

        /// <summary>
        /// Immediately subscribes to source and retains the elements in the observable sequence.
        /// </summary>
        /// <typeparam name="TSource">The type of the elements in the source sequence.</typeparam>
        /// <param name="source">Source sequence.</param>
        /// <returns>Object that's both an observable sequence and a list which can be used to access the source sequence's elements.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="source"/> is null.</exception>
        [Experimental]
        public static ListObservable<TSource> ToListObservable<TSource>(this IObservable<TSource> source)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));

            return new ListObservable<TSource>(source);
        }

        #endregion

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

                return StableCompositeDisposable.Create(leftSubscription, rightSubscription);
            });
        }
    }
}

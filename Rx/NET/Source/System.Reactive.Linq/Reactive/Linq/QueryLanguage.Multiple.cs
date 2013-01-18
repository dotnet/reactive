// Copyright (c) Microsoft Open Technologies, Inc. All rights reserved. See License.txt in the project root for license information.

using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Reactive.Concurrency;
using System.Reactive.Disposables;
using System.Reactive.Subjects;

#if !NO_TPL
using System.Reactive.Threading.Tasks;
using System.Threading.Tasks;
#endif

namespace System.Reactive.Linq
{
#if !NO_PERF
    using Observαble;
#endif

    internal partial class QueryLanguage
    {
        #region + Amb +

        public virtual IObservable<TSource> Amb<TSource>(IObservable<TSource> first, IObservable<TSource> second)
        {
            return Amb_(first, second);
        }

        public virtual IObservable<TSource> Amb<TSource>(params IObservable<TSource>[] sources)
        {
            return Amb_(sources);
        }

        public virtual IObservable<TSource> Amb<TSource>(IEnumerable<IObservable<TSource>> sources)
        {
            return Amb_(sources);
        }

        private static IObservable<TSource> Amb_<TSource>(IEnumerable<IObservable<TSource>> sources)
        {
            return sources.Aggregate(Observable.Never<TSource>(), (previous, current) => previous.Amb(current));
        }

        private static IObservable<TSource> Amb_<TSource>(IObservable<TSource> leftSource, IObservable<TSource> rightSource)
        {
#if !NO_PERF
            return new Amb<TSource>(leftSource, rightSource);
#else
            return new AnonymousObservable<TSource>(observer =>
            {
                var leftSubscription = new SingleAssignmentDisposable();
                var rightSubscription = new SingleAssignmentDisposable();

                var choice = AmbState.Neither;

                var gate = new object();

                var left = new AmbObserver<TSource>();
                var right = new AmbObserver<TSource>();

                left.Observer = Observer.Synchronize(Observer.Create<TSource>(
                    x =>
                    {
                        if (choice == AmbState.Neither)
                        {
                            choice = AmbState.Left;
                            rightSubscription.Dispose();
                            left.Observer = observer;
                        }

                        if (choice == AmbState.Left)
                            observer.OnNext(x);
                    },
                    ex =>
                    {
                        if (choice == AmbState.Neither)
                        {
                            choice = AmbState.Left;
                            rightSubscription.Dispose();
                            left.Observer = observer;
                        }

                        if (choice == AmbState.Left)
                            observer.OnError(ex);
                    },
                    () =>
                    {
                        if (choice == AmbState.Neither)
                        {
                            choice = AmbState.Left;
                            rightSubscription.Dispose();
                            left.Observer = observer;
                        }

                        if (choice == AmbState.Left)
                            observer.OnCompleted();
                    }
                ), gate);

                right.Observer = Observer.Synchronize(Observer.Create<TSource>(
                    x =>
                    {
                        if (choice == AmbState.Neither)
                        {
                            choice = AmbState.Right;
                            leftSubscription.Dispose();
                            right.Observer = observer;
                        }

                        if (choice == AmbState.Right)
                            observer.OnNext(x);
                    },
                    ex =>
                    {
                        if (choice == AmbState.Neither)
                        {
                            choice = AmbState.Right;
                            leftSubscription.Dispose();
                            right.Observer = observer;
                        }

                        if (choice == AmbState.Right)
                            observer.OnError(ex);
                    },
                    () =>
                    {
                        if (choice == AmbState.Neither)
                        {
                            choice = AmbState.Right;
                            leftSubscription.Dispose();
                            right.Observer = observer;
                        }

                        if (choice == AmbState.Right)
                            observer.OnCompleted();
                    }
                ), gate);

                leftSubscription.Disposable = leftSource.Subscribe(left);
                rightSubscription.Disposable = rightSource.Subscribe(right);

                return new CompositeDisposable(leftSubscription, rightSubscription);
            });
#endif
        }

#if NO_PERF
        class AmbObserver<TSource> : IObserver<TSource>
        {
            public virtual IObserver<TSource> Observer { get; set; }

            public virtual void OnCompleted()
            {
                Observer.OnCompleted();
            }

            public virtual void OnError(Exception error)
            {
                Observer.OnError(error);
            }

            public virtual void OnNext(TSource value)
            {
                Observer.OnNext(value);
            }
        }

        enum AmbState
        {
            Left,
            Right,
            Neither
        }
#endif

        #endregion

        #region + Buffer +

        public virtual IObservable<IList<TSource>> Buffer<TSource, TBufferClosing>(IObservable<TSource> source, Func<IObservable<TBufferClosing>> bufferClosingSelector)
        {
#if !NO_PERF
            return new Buffer<TSource, TBufferClosing>(source, bufferClosingSelector);
#else
            return source.Window(bufferClosingSelector).SelectMany(ToList);
#endif
        }

        public virtual IObservable<IList<TSource>> Buffer<TSource, TBufferOpening, TBufferClosing>(IObservable<TSource> source, IObservable<TBufferOpening> bufferOpenings, Func<TBufferOpening, IObservable<TBufferClosing>> bufferClosingSelector)
        {
            return source.Window(bufferOpenings, bufferClosingSelector).SelectMany(ToList);
        }

        public virtual IObservable<IList<TSource>> Buffer<TSource, TBufferBoundary>(IObservable<TSource> source, IObservable<TBufferBoundary> bufferBoundaries)
        {
#if !NO_PERF
            return new Buffer<TSource, TBufferBoundary>(source, bufferBoundaries);
#else
            return source.Window(bufferBoundaries).SelectMany(ToList);
#endif
        }

        #endregion

        #region + Catch +

        public virtual IObservable<TSource> Catch<TSource, TException>(IObservable<TSource> source, Func<TException, IObservable<TSource>> handler) where TException : Exception
        {
#if !NO_PERF
            return new Catch<TSource, TException>(source, handler);
#else
            return new AnonymousObservable<TSource>(observer =>
            {
                var subscription = new SerialDisposable();

                var d1 = new SingleAssignmentDisposable();
                subscription.Disposable = d1;
                d1.Disposable = source.Subscribe(observer.OnNext,
                    exception =>
                    {
                        var e = exception as TException;
                        if (e != null)
                        {
                            IObservable<TSource> result;
                            try
                            {
                                result = handler(e);
                            }
                            catch (Exception ex)
                            {
                                observer.OnError(ex);
                                return;
                            }

                            var d = new SingleAssignmentDisposable();
                            subscription.Disposable = d;
                            d.Disposable = result.Subscribe(observer);
                        }
                        else
                            observer.OnError(exception);
                    }, observer.OnCompleted);

                return subscription;
            });
#endif
        }

        public virtual IObservable<TSource> Catch<TSource>(IObservable<TSource> first, IObservable<TSource> second)
        {
            return Catch_<TSource>(new[] { first, second });
        }

        public virtual IObservable<TSource> Catch<TSource>(params IObservable<TSource>[] sources)
        {
            return Catch_<TSource>(sources);
        }

        public virtual IObservable<TSource> Catch<TSource>(IEnumerable<IObservable<TSource>> sources)
        {
            return Catch_<TSource>(sources);
        }

        private static IObservable<TSource> Catch_<TSource>(IEnumerable<IObservable<TSource>> sources)
        {
#if !NO_PERF
            return new Catch<TSource>(sources);
#else
            return new AnonymousObservable<TSource>(observer =>
            {
                var gate = new AsyncLock();
                var isDisposed = false;
                var e = sources.GetEnumerator();
                var subscription = new SerialDisposable();
                var lastException = default(Exception);

                var cancelable = SchedulerDefaults.TailRecursion.Schedule(self => gate.Wait(() =>
                {
                    var current = default(IObservable<TSource>);
                    var hasNext = false;
                    var ex = default(Exception);

                    if (!isDisposed)
                    {
                        try
                        {
                            hasNext = e.MoveNext();
                            if (hasNext)
                                current = e.Current;
                            else
                                e.Dispose();
                        }
                        catch (Exception exception)
                        {
                            ex = exception;
                            e.Dispose();
                        }
                    }
                    else
                        return;

                    if (ex != null)
                    {
                        observer.OnError(ex);
                        return;
                    }

                    if (!hasNext)
                    {
                        if (lastException != null)
                            observer.OnError(lastException);
                        else
                            observer.OnCompleted();
                        return;
                    }

                    var d = new SingleAssignmentDisposable();
                    subscription.Disposable = d;
                    d.Disposable = current.Subscribe(observer.OnNext, exception =>
                    {
                        lastException = exception;
                        self();
                    }, observer.OnCompleted);
                }));

                return new CompositeDisposable(subscription, cancelable, Disposable.Create(() => gate.Wait(() =>
                {
                    e.Dispose();
                    isDisposed = true;
                })));
            });
#endif
        }

        #endregion

        #region + CombineLatest +

        public virtual IObservable<TResult> CombineLatest<TFirst, TSecond, TResult>(IObservable<TFirst> first, IObservable<TSecond> second, Func<TFirst, TSecond, TResult> resultSelector)
        {
#if !NO_PERF
            return new CombineLatest<TFirst, TSecond, TResult>(first, second, resultSelector);
#else
            return new AnonymousObservable<TResult>(observer =>
            {
                var hasLeft = false;
                var hasRight = false;

                var left = default(TFirst);
                var right = default(TSecond);

                var leftDone = false;
                var rightDone = false;

                var leftSubscription = new SingleAssignmentDisposable();
                var rightSubscription = new SingleAssignmentDisposable();

                var gate = new object();

                leftSubscription.Disposable = first.Synchronize(gate).Subscribe(
                    l =>
                    {
                        hasLeft = true;
                        left = l;

                        if (hasRight)
                        {
                            var res = default(TResult);
                            try
                            {
                                res = resultSelector(left, right);
                            }
                            catch (Exception ex)
                            {
                                observer.OnError(ex);
                                return;
                            }

                            observer.OnNext(res);
                        }
                        else if (rightDone)
                        {
                            observer.OnCompleted();
                            return;
                        }
                    },
                    observer.OnError,
                    () =>
                    {
                        leftDone = true;

                        if (rightDone)
                        {
                            observer.OnCompleted();
                            return;
                        }
                    }
                );

                rightSubscription.Disposable = second.Synchronize(gate).Subscribe(
                    r =>
                    {
                        hasRight = true;
                        right = r;

                        if (hasLeft)
                        {
                            var res = default(TResult);
                            try
                            {
                                res = resultSelector(left, right);
                            }
                            catch (Exception ex)
                            {
                                observer.OnError(ex);
                                return;
                            }

                            observer.OnNext(res);
                        }
                        else if (leftDone)
                        {
                            observer.OnCompleted();
                            return;
                        }
                    },
                    observer.OnError,
                    () =>
                    {
                        rightDone = true;

                        if (leftDone)
                        {
                            observer.OnCompleted();
                            return;
                        }
                    }
                );

                return new CompositeDisposable(leftSubscription, rightSubscription);
            });
#endif
        }

#if !NO_PERF

        /* The following code is generated by a tool checked in to $/.../Source/Tools/CodeGenerators. */

        #region CombineLatest auto-generated code (6/10/2012 7:25:03 PM)

        public virtual IObservable<TResult> CombineLatest<TSource1, TSource2, TSource3, TResult>(IObservable<TSource1> source1, IObservable<TSource2> source2, IObservable<TSource3> source3, Func<TSource1, TSource2, TSource3, TResult> resultSelector)
        {
            return new CombineLatest<TSource1, TSource2, TSource3, TResult>(source1, source2, source3, resultSelector);
        }

        public virtual IObservable<TResult> CombineLatest<TSource1, TSource2, TSource3, TSource4, TResult>(IObservable<TSource1> source1, IObservable<TSource2> source2, IObservable<TSource3> source3, IObservable<TSource4> source4, Func<TSource1, TSource2, TSource3, TSource4, TResult> resultSelector)
        {
            return new CombineLatest<TSource1, TSource2, TSource3, TSource4, TResult>(source1, source2, source3, source4, resultSelector);
        }

#if !NO_LARGEARITY
        public virtual IObservable<TResult> CombineLatest<TSource1, TSource2, TSource3, TSource4, TSource5, TResult>(IObservable<TSource1> source1, IObservable<TSource2> source2, IObservable<TSource3> source3, IObservable<TSource4> source4, IObservable<TSource5> source5, Func<TSource1, TSource2, TSource3, TSource4, TSource5, TResult> resultSelector)
        {
            return new CombineLatest<TSource1, TSource2, TSource3, TSource4, TSource5, TResult>(source1, source2, source3, source4, source5, resultSelector);
        }

        public virtual IObservable<TResult> CombineLatest<TSource1, TSource2, TSource3, TSource4, TSource5, TSource6, TResult>(IObservable<TSource1> source1, IObservable<TSource2> source2, IObservable<TSource3> source3, IObservable<TSource4> source4, IObservable<TSource5> source5, IObservable<TSource6> source6, Func<TSource1, TSource2, TSource3, TSource4, TSource5, TSource6, TResult> resultSelector)
        {
            return new CombineLatest<TSource1, TSource2, TSource3, TSource4, TSource5, TSource6, TResult>(source1, source2, source3, source4, source5, source6, resultSelector);
        }

        public virtual IObservable<TResult> CombineLatest<TSource1, TSource2, TSource3, TSource4, TSource5, TSource6, TSource7, TResult>(IObservable<TSource1> source1, IObservable<TSource2> source2, IObservable<TSource3> source3, IObservable<TSource4> source4, IObservable<TSource5> source5, IObservable<TSource6> source6, IObservable<TSource7> source7, Func<TSource1, TSource2, TSource3, TSource4, TSource5, TSource6, TSource7, TResult> resultSelector)
        {
            return new CombineLatest<TSource1, TSource2, TSource3, TSource4, TSource5, TSource6, TSource7, TResult>(source1, source2, source3, source4, source5, source6, source7, resultSelector);
        }

        public virtual IObservable<TResult> CombineLatest<TSource1, TSource2, TSource3, TSource4, TSource5, TSource6, TSource7, TSource8, TResult>(IObservable<TSource1> source1, IObservable<TSource2> source2, IObservable<TSource3> source3, IObservable<TSource4> source4, IObservable<TSource5> source5, IObservable<TSource6> source6, IObservable<TSource7> source7, IObservable<TSource8> source8, Func<TSource1, TSource2, TSource3, TSource4, TSource5, TSource6, TSource7, TSource8, TResult> resultSelector)
        {
            return new CombineLatest<TSource1, TSource2, TSource3, TSource4, TSource5, TSource6, TSource7, TSource8, TResult>(source1, source2, source3, source4, source5, source6, source7, source8, resultSelector);
        }

        public virtual IObservable<TResult> CombineLatest<TSource1, TSource2, TSource3, TSource4, TSource5, TSource6, TSource7, TSource8, TSource9, TResult>(IObservable<TSource1> source1, IObservable<TSource2> source2, IObservable<TSource3> source3, IObservable<TSource4> source4, IObservable<TSource5> source5, IObservable<TSource6> source6, IObservable<TSource7> source7, IObservable<TSource8> source8, IObservable<TSource9> source9, Func<TSource1, TSource2, TSource3, TSource4, TSource5, TSource6, TSource7, TSource8, TSource9, TResult> resultSelector)
        {
            return new CombineLatest<TSource1, TSource2, TSource3, TSource4, TSource5, TSource6, TSource7, TSource8, TSource9, TResult>(source1, source2, source3, source4, source5, source6, source7, source8, source9, resultSelector);
        }

        public virtual IObservable<TResult> CombineLatest<TSource1, TSource2, TSource3, TSource4, TSource5, TSource6, TSource7, TSource8, TSource9, TSource10, TResult>(IObservable<TSource1> source1, IObservable<TSource2> source2, IObservable<TSource3> source3, IObservable<TSource4> source4, IObservable<TSource5> source5, IObservable<TSource6> source6, IObservable<TSource7> source7, IObservable<TSource8> source8, IObservable<TSource9> source9, IObservable<TSource10> source10, Func<TSource1, TSource2, TSource3, TSource4, TSource5, TSource6, TSource7, TSource8, TSource9, TSource10, TResult> resultSelector)
        {
            return new CombineLatest<TSource1, TSource2, TSource3, TSource4, TSource5, TSource6, TSource7, TSource8, TSource9, TSource10, TResult>(source1, source2, source3, source4, source5, source6, source7, source8, source9, source10, resultSelector);
        }

        public virtual IObservable<TResult> CombineLatest<TSource1, TSource2, TSource3, TSource4, TSource5, TSource6, TSource7, TSource8, TSource9, TSource10, TSource11, TResult>(IObservable<TSource1> source1, IObservable<TSource2> source2, IObservable<TSource3> source3, IObservable<TSource4> source4, IObservable<TSource5> source5, IObservable<TSource6> source6, IObservable<TSource7> source7, IObservable<TSource8> source8, IObservable<TSource9> source9, IObservable<TSource10> source10, IObservable<TSource11> source11, Func<TSource1, TSource2, TSource3, TSource4, TSource5, TSource6, TSource7, TSource8, TSource9, TSource10, TSource11, TResult> resultSelector)
        {
            return new CombineLatest<TSource1, TSource2, TSource3, TSource4, TSource5, TSource6, TSource7, TSource8, TSource9, TSource10, TSource11, TResult>(source1, source2, source3, source4, source5, source6, source7, source8, source9, source10, source11, resultSelector);
        }

        public virtual IObservable<TResult> CombineLatest<TSource1, TSource2, TSource3, TSource4, TSource5, TSource6, TSource7, TSource8, TSource9, TSource10, TSource11, TSource12, TResult>(IObservable<TSource1> source1, IObservable<TSource2> source2, IObservable<TSource3> source3, IObservable<TSource4> source4, IObservable<TSource5> source5, IObservable<TSource6> source6, IObservable<TSource7> source7, IObservable<TSource8> source8, IObservable<TSource9> source9, IObservable<TSource10> source10, IObservable<TSource11> source11, IObservable<TSource12> source12, Func<TSource1, TSource2, TSource3, TSource4, TSource5, TSource6, TSource7, TSource8, TSource9, TSource10, TSource11, TSource12, TResult> resultSelector)
        {
            return new CombineLatest<TSource1, TSource2, TSource3, TSource4, TSource5, TSource6, TSource7, TSource8, TSource9, TSource10, TSource11, TSource12, TResult>(source1, source2, source3, source4, source5, source6, source7, source8, source9, source10, source11, source12, resultSelector);
        }

        public virtual IObservable<TResult> CombineLatest<TSource1, TSource2, TSource3, TSource4, TSource5, TSource6, TSource7, TSource8, TSource9, TSource10, TSource11, TSource12, TSource13, TResult>(IObservable<TSource1> source1, IObservable<TSource2> source2, IObservable<TSource3> source3, IObservable<TSource4> source4, IObservable<TSource5> source5, IObservable<TSource6> source6, IObservable<TSource7> source7, IObservable<TSource8> source8, IObservable<TSource9> source9, IObservable<TSource10> source10, IObservable<TSource11> source11, IObservable<TSource12> source12, IObservable<TSource13> source13, Func<TSource1, TSource2, TSource3, TSource4, TSource5, TSource6, TSource7, TSource8, TSource9, TSource10, TSource11, TSource12, TSource13, TResult> resultSelector)
        {
            return new CombineLatest<TSource1, TSource2, TSource3, TSource4, TSource5, TSource6, TSource7, TSource8, TSource9, TSource10, TSource11, TSource12, TSource13, TResult>(source1, source2, source3, source4, source5, source6, source7, source8, source9, source10, source11, source12, source13, resultSelector);
        }

        public virtual IObservable<TResult> CombineLatest<TSource1, TSource2, TSource3, TSource4, TSource5, TSource6, TSource7, TSource8, TSource9, TSource10, TSource11, TSource12, TSource13, TSource14, TResult>(IObservable<TSource1> source1, IObservable<TSource2> source2, IObservable<TSource3> source3, IObservable<TSource4> source4, IObservable<TSource5> source5, IObservable<TSource6> source6, IObservable<TSource7> source7, IObservable<TSource8> source8, IObservable<TSource9> source9, IObservable<TSource10> source10, IObservable<TSource11> source11, IObservable<TSource12> source12, IObservable<TSource13> source13, IObservable<TSource14> source14, Func<TSource1, TSource2, TSource3, TSource4, TSource5, TSource6, TSource7, TSource8, TSource9, TSource10, TSource11, TSource12, TSource13, TSource14, TResult> resultSelector)
        {
            return new CombineLatest<TSource1, TSource2, TSource3, TSource4, TSource5, TSource6, TSource7, TSource8, TSource9, TSource10, TSource11, TSource12, TSource13, TSource14, TResult>(source1, source2, source3, source4, source5, source6, source7, source8, source9, source10, source11, source12, source13, source14, resultSelector);
        }

        public virtual IObservable<TResult> CombineLatest<TSource1, TSource2, TSource3, TSource4, TSource5, TSource6, TSource7, TSource8, TSource9, TSource10, TSource11, TSource12, TSource13, TSource14, TSource15, TResult>(IObservable<TSource1> source1, IObservable<TSource2> source2, IObservable<TSource3> source3, IObservable<TSource4> source4, IObservable<TSource5> source5, IObservable<TSource6> source6, IObservable<TSource7> source7, IObservable<TSource8> source8, IObservable<TSource9> source9, IObservable<TSource10> source10, IObservable<TSource11> source11, IObservable<TSource12> source12, IObservable<TSource13> source13, IObservable<TSource14> source14, IObservable<TSource15> source15, Func<TSource1, TSource2, TSource3, TSource4, TSource5, TSource6, TSource7, TSource8, TSource9, TSource10, TSource11, TSource12, TSource13, TSource14, TSource15, TResult> resultSelector)
        {
            return new CombineLatest<TSource1, TSource2, TSource3, TSource4, TSource5, TSource6, TSource7, TSource8, TSource9, TSource10, TSource11, TSource12, TSource13, TSource14, TSource15, TResult>(source1, source2, source3, source4, source5, source6, source7, source8, source9, source10, source11, source12, source13, source14, source15, resultSelector);
        }

        public virtual IObservable<TResult> CombineLatest<TSource1, TSource2, TSource3, TSource4, TSource5, TSource6, TSource7, TSource8, TSource9, TSource10, TSource11, TSource12, TSource13, TSource14, TSource15, TSource16, TResult>(IObservable<TSource1> source1, IObservable<TSource2> source2, IObservable<TSource3> source3, IObservable<TSource4> source4, IObservable<TSource5> source5, IObservable<TSource6> source6, IObservable<TSource7> source7, IObservable<TSource8> source8, IObservable<TSource9> source9, IObservable<TSource10> source10, IObservable<TSource11> source11, IObservable<TSource12> source12, IObservable<TSource13> source13, IObservable<TSource14> source14, IObservable<TSource15> source15, IObservable<TSource16> source16, Func<TSource1, TSource2, TSource3, TSource4, TSource5, TSource6, TSource7, TSource8, TSource9, TSource10, TSource11, TSource12, TSource13, TSource14, TSource15, TSource16, TResult> resultSelector)
        {
            return new CombineLatest<TSource1, TSource2, TSource3, TSource4, TSource5, TSource6, TSource7, TSource8, TSource9, TSource10, TSource11, TSource12, TSource13, TSource14, TSource15, TSource16, TResult>(source1, source2, source3, source4, source5, source6, source7, source8, source9, source10, source11, source12, source13, source14, source15, source16, resultSelector);
        }

#endif

        #endregion

#endif

        public virtual IObservable<TResult> CombineLatest<TSource, TResult>(IEnumerable<IObservable<TSource>> sources, Func<IList<TSource>, TResult> resultSelector)
        {
            return CombineLatest_<TSource, TResult>(sources, resultSelector);
        }

        public virtual IObservable<IList<TSource>> CombineLatest<TSource>(IEnumerable<IObservable<TSource>> sources)
        {
            return CombineLatest_<TSource, IList<TSource>>(sources, res => res.ToList());
        }

        public virtual IObservable<IList<TSource>> CombineLatest<TSource>(params IObservable<TSource>[] sources)
        {
            return CombineLatest_<TSource, IList<TSource>>(sources, res => res.ToList());
        }

        private static IObservable<TResult> CombineLatest_<TSource, TResult>(IEnumerable<IObservable<TSource>> sources, Func<IList<TSource>, TResult> resultSelector)
        {
#if !NO_PERF
            return new CombineLatest<TSource, TResult>(sources, resultSelector);
#else
            return new AnonymousObservable<TResult>(observer =>
            {
                var srcs = sources.ToArray();

                var N = srcs.Length;

                var hasValue = new bool[N];
                var hasValueAll = false;

                var values = new List<TSource>(N);
                for (int i = 0; i < N; i++)
                    values.Add(default(TSource));

                var isDone = new bool[N];

                var next = new Action<int>(i =>
                {
                    hasValue[i] = true;

                    if (hasValueAll || (hasValueAll = hasValue.All(Stubs<bool>.I)))
                    {
                        var res = default(TResult);
                        try
                        {
                            res = resultSelector(new ReadOnlyCollection<TSource>(values));
                        }
                        catch (Exception ex)
                        {
                            observer.OnError(ex);
                            return;
                        }

                        observer.OnNext(res);
                    }
                    else if (isDone.Where((x, j) => j != i).All(Stubs<bool>.I))
                    {
                        observer.OnCompleted();
                        return;
                    }
                });

                var done = new Action<int>(i =>
                {
                    isDone[i] = true;

                    if (isDone.All(Stubs<bool>.I))
                    {
                        observer.OnCompleted();
                        return;
                    }
                });

                var subscriptions = new SingleAssignmentDisposable[N];

                var gate = new object();

                for (int i = 0; i < N; i++)
                {
                    var j = i;
                    subscriptions[j] = new SingleAssignmentDisposable
                    {
                        Disposable = srcs[j].Synchronize(gate).Subscribe(
                            x =>
                            {
                                values[j] = x;
                                next(j);
                            },
                            observer.OnError,
                            () =>
                            {
                                done(j);
                            }
                        )
                    };
                }

                return new CompositeDisposable(subscriptions);
            });
#endif
        }

        #endregion

        #region + Concat +

        public virtual IObservable<TSource> Concat<TSource>(IObservable<TSource> first, IObservable<TSource> second)
        {
            return Concat_<TSource>(new[] { first, second });
        }

        public virtual IObservable<TSource> Concat<TSource>(params IObservable<TSource>[] sources)
        {
            return Concat_<TSource>(sources);
        }

        public virtual IObservable<TSource> Concat<TSource>(IEnumerable<IObservable<TSource>> sources)
        {
            return Concat_<TSource>(sources);
        }

        private static IObservable<TSource> Concat_<TSource>(IEnumerable<IObservable<TSource>> sources)
        {
#if !NO_PERF
            return new Concat<TSource>(sources);
#else
            return new AnonymousObservable<TSource>(observer =>
            {
                var isDisposed = false;
                var e = sources.GetEnumerator();
                var subscription = new SerialDisposable();
                var gate = new AsyncLock();

                var cancelable = SchedulerDefaults.TailRecursion.Schedule(self => gate.Wait(() =>
                {
                    var current = default(IObservable<TSource>);
                    var hasNext = false;
                    var ex = default(Exception);

                    if (!isDisposed)
                    {
                        try
                        {
                            hasNext = e.MoveNext();
                            if (hasNext)
                                current = e.Current;
                            else
                                e.Dispose();
                        }
                        catch (Exception exception)
                        {
                            ex = exception;
                            e.Dispose();
                        }
                    }
                    else
                        return;

                    if (ex != null)
                    {
                        observer.OnError(ex);
                        return;
                    }

                    if (!hasNext)
                    {
                        observer.OnCompleted();
                        return;
                    }

                    var d = new SingleAssignmentDisposable();
                    subscription.Disposable = d;
                    d.Disposable = current.Subscribe(observer.OnNext, observer.OnError, self);
                }));

                return new CompositeDisposable(subscription, cancelable, Disposable.Create(() => gate.Wait(() =>
                            {
                                e.Dispose();
                                isDisposed = true;
                            })));
            });
#endif
        }

        public virtual IObservable<TSource> Concat<TSource>(IObservable<IObservable<TSource>> sources)
        {
            return Concat_<TSource>(sources);
        }

#if !NO_TPL
        public virtual IObservable<TSource> Concat<TSource>(IObservable<Task<TSource>> sources)
        {
            return Concat_<TSource>(Select(sources, TaskObservableExtensions.ToObservable));
        }
#endif

        private IObservable<TSource> Concat_<TSource>(IObservable<IObservable<TSource>> sources)
        {
            return Merge(sources, 1);
        }

        #endregion

        #region + Merge +

        public virtual IObservable<TSource> Merge<TSource>(IObservable<IObservable<TSource>> sources)
        {
            return Merge_<TSource>(sources);
        }

#if !NO_TPL
        public virtual IObservable<TSource> Merge<TSource>(IObservable<Task<TSource>> sources)
        {
#if !NO_PERF
            return new Merge<TSource>(sources);
#else
            return Merge_<TSource>(Select(sources, TaskObservableExtensions.ToObservable));
#endif
        }
#endif

        public virtual IObservable<TSource> Merge<TSource>(IObservable<IObservable<TSource>> sources, int maxConcurrent)
        {
            return Merge_<TSource>(sources, maxConcurrent);
        }

        public virtual IObservable<TSource> Merge<TSource>(IEnumerable<IObservable<TSource>> sources, int maxConcurrent)
        {
            return Merge_<TSource>(sources.ToObservable(SchedulerDefaults.ConstantTimeOperations), maxConcurrent);
        }

        public virtual IObservable<TSource> Merge<TSource>(IEnumerable<IObservable<TSource>> sources, int maxConcurrent, IScheduler scheduler)
        {
            return Merge_<TSource>(sources.ToObservable(scheduler), maxConcurrent);
        }

        public virtual IObservable<TSource> Merge<TSource>(IObservable<TSource> first, IObservable<TSource> second)
        {
            return Merge_<TSource>(new[] { first, second }.ToObservable(SchedulerDefaults.ConstantTimeOperations));
        }

        public virtual IObservable<TSource> Merge<TSource>(IObservable<TSource> first, IObservable<TSource> second, IScheduler scheduler)
        {
            return Merge_<TSource>(new[] { first, second }.ToObservable(scheduler));
        }

        public virtual IObservable<TSource> Merge<TSource>(params IObservable<TSource>[] sources)
        {
            return Merge_<TSource>(sources.ToObservable(SchedulerDefaults.ConstantTimeOperations));
        }

        public virtual IObservable<TSource> Merge<TSource>(IScheduler scheduler, params IObservable<TSource>[] sources)
        {
            return Merge_<TSource>(sources.ToObservable(scheduler));
        }

        public virtual IObservable<TSource> Merge<TSource>(IEnumerable<IObservable<TSource>> sources)
        {
            return Merge_<TSource>(sources.ToObservable(SchedulerDefaults.ConstantTimeOperations));
        }

        public virtual IObservable<TSource> Merge<TSource>(IEnumerable<IObservable<TSource>> sources, IScheduler scheduler)
        {
            return Merge_<TSource>(sources.ToObservable(scheduler));
        }

        private static IObservable<TSource> Merge_<TSource>(IObservable<IObservable<TSource>> sources)
        {
#if !NO_PERF
            return new Merge<TSource>(sources);
#else
            return new AnonymousObservable<TSource>(observer =>
            {
                var gate = new object();
                var isStopped = false;
                var m = new SingleAssignmentDisposable();
                var group = new CompositeDisposable() { m };

                m.Disposable = sources.Subscribe(
                    innerSource =>
                    {
                        var innerSubscription = new SingleAssignmentDisposable();
                        group.Add(innerSubscription);
                        innerSubscription.Disposable = innerSource.Subscribe(
                            x =>
                            {
                                lock (gate)
                                    observer.OnNext(x);
                            },
                            exception =>
                            {
                                lock (gate)
                                    observer.OnError(exception);
                            },
                            () =>
                            {
                                group.Remove(innerSubscription);   // modification MUST occur before subsequent check
                                if (isStopped && group.Count == 1) // isStopped must be checked before group Count to ensure outer is not creating more groups
                                    lock (gate)
                                        observer.OnCompleted();
                            });
                    },
                    exception =>
                    {
                        lock (gate)
                            observer.OnError(exception);
                    },
                    () =>
                    {
                        isStopped = true;     // modification MUST occur before subsequent check
                        if (group.Count == 1)
                            lock (gate)
                                observer.OnCompleted();
                    });

                return group;
            });
#endif
        }

        private static IObservable<TSource> Merge_<TSource>(IObservable<IObservable<TSource>> sources, int maxConcurrent)
        {
#if !NO_PERF
            return new Merge<TSource>(sources, maxConcurrent);
#else
            return new AnonymousObservable<TSource>(observer =>
            {
                var gate = new object();
                var q = new Queue<IObservable<TSource>>();
                var isStopped = false;
                var group = new CompositeDisposable();
                var activeCount = 0;

                var subscribe = default(Action<IObservable<TSource>>);
                subscribe = xs =>
                {
                    var subscription = new SingleAssignmentDisposable();
                    group.Add(subscription);
                    subscription.Disposable = xs.Subscribe(
                        x =>
                        {
                            lock (gate)
                                observer.OnNext(x);
                        },
                        exception =>
                        {
                            lock (gate)
                                observer.OnError(exception);
                        },
                        () =>
                        {
                            group.Remove(subscription);
                            lock (gate)
                            {
                                if (q.Count > 0)
                                {
                                    var s = q.Dequeue();
                                    subscribe(s);
                                }
                                else
                                {
                                    activeCount--;
                                    if (isStopped && activeCount == 0)
                                        observer.OnCompleted();
                                }
                            }
                        });
                };

                group.Add(sources.Subscribe(
                    innerSource =>
                    {
                        lock (gate)
                        {
                            if (activeCount < maxConcurrent)
                            {
                                activeCount++;
                                subscribe(innerSource);
                            }
                            else
                                q.Enqueue(innerSource);
                        }
                    },
                    exception =>
                    {
                        lock (gate)
                            observer.OnError(exception);
                    },
                    () =>
                    {
                        lock (gate)
                        {
                            isStopped = true;
                            if (activeCount == 0)
                                observer.OnCompleted();
                        }
                    }));

                return group;
            });
#endif
        }

        #endregion

        #region + OnErrorResumeNext +

        public virtual IObservable<TSource> OnErrorResumeNext<TSource>(IObservable<TSource> first, IObservable<TSource> second)
        {
            return OnErrorResumeNext_<TSource>(new[] { first, second });
        }

        public virtual IObservable<TSource> OnErrorResumeNext<TSource>(params IObservable<TSource>[] sources)
        {
            return OnErrorResumeNext_<TSource>(sources);
        }

        public virtual IObservable<TSource> OnErrorResumeNext<TSource>(IEnumerable<IObservable<TSource>> sources)
        {
            return OnErrorResumeNext_<TSource>(sources);
        }

        private static IObservable<TSource> OnErrorResumeNext_<TSource>(IEnumerable<IObservable<TSource>> sources)
        {
#if !NO_PERF
            return new OnErrorResumeNext<TSource>(sources);
#else
            return new AnonymousObservable<TSource>(observer =>
            {
                var gate = new AsyncLock();
                var isDisposed = false;
                var e = sources.GetEnumerator();
                var subscription = new SerialDisposable();

                var cancelable = SchedulerDefaults.TailRecursion.Schedule(self => gate.Wait(() =>
                {
                    var current = default(IObservable<TSource>);
                    var hasNext = false;
                    var ex = default(Exception);

                    if (!isDisposed)
                    {
                        try
                        {
                            hasNext = e.MoveNext();
                            if (hasNext)
                                current = e.Current;
                            else
                                e.Dispose();
                        }
                        catch (Exception exception)
                        {
                            ex = exception;
                            e.Dispose();
                        }
                    }
                    else
                        return;

                    if (ex != null)
                    {
                        observer.OnError(ex);
                        return;
                    }

                    if (!hasNext)
                    {
                        observer.OnCompleted();
                        return;
                    }

                    var d = new SingleAssignmentDisposable();
                    subscription.Disposable = d;
                    d.Disposable = current.Subscribe(observer.OnNext, exception => self(), self);
                }));

                return new CompositeDisposable(subscription, cancelable, Disposable.Create(() => gate.Wait(() =>
                {
                    e.Dispose();
                    isDisposed = true;
                })));
            });
#endif
        }

        #endregion

        #region + SkipUntil +

        public virtual IObservable<TSource> SkipUntil<TSource, TOther>(IObservable<TSource> source, IObservable<TOther> other)
        {
#if !NO_PERF
            return new SkipUntil<TSource, TOther>(source, other);
#else
            return new AnonymousObservable<TSource>(observer =>
            {
                var sourceSubscription = new SingleAssignmentDisposable();
                var otherSubscription = new SingleAssignmentDisposable();

                var open = false;

                var gate = new object();

                sourceSubscription.Disposable = source.Synchronize(gate).Subscribe(
                    x =>
                    {
                        if (open)
                            observer.OnNext(x);
                    },
                    observer.OnError, // BREAKING CHANGE - Error propagation was guarded by "other" source in v1.0.10621 (due to materialization).
                    () =>
                    {
                        if (open)
                            observer.OnCompleted();
                    }
                );

                otherSubscription.Disposable = other.Synchronize(gate).Subscribe(
                    x =>
                    {
                        open = true;
                        otherSubscription.Dispose();
                    },
                    observer.OnError
                );

                return new CompositeDisposable(sourceSubscription, otherSubscription);
            });
#endif
        }

        #endregion

        #region + Switch +

        public virtual IObservable<TSource> Switch<TSource>(IObservable<IObservable<TSource>> sources)
        {
            return Switch_<TSource>(sources);
        }

#if !NO_TPL
        public virtual IObservable<TSource> Switch<TSource>(IObservable<Task<TSource>> sources)
        {
            return Switch_<TSource>(Select(sources, TaskObservableExtensions.ToObservable));
        }
#endif

        private IObservable<TSource> Switch_<TSource>(IObservable<IObservable<TSource>> sources)
        {
#if !NO_PERF
            return new Switch<TSource>(sources);
#else
            return new AnonymousObservable<TSource>(observer =>
            {
                var gate = new object();
                var innerSubscription = new SerialDisposable();
                var isStopped = false;
                var latest = 0UL;
                var hasLatest = false;
                var subscription = sources.Subscribe(
                    innerSource =>
                    {
                        var id = default(ulong);
                        lock (gate)
                        {
                            id = unchecked(++latest);
                            hasLatest = true;
                        }

                        var d = new SingleAssignmentDisposable();
                        innerSubscription.Disposable = d;
                        d.Disposable = innerSource.Subscribe(
                        x =>
                        {
                            lock (gate)
                            {
                                if (latest == id)
                                    observer.OnNext(x);
                            }
                        },
                        exception =>
                        {
                            lock (gate)
                            {
                                if (latest == id)
                                    observer.OnError(exception);
                            }
                        },
                        () =>
                        {
                            lock (gate)
                            {
                                if (latest == id)
                                {
                                    hasLatest = false;

                                    if (isStopped)
                                        observer.OnCompleted();
                                }
                            }
                        });
                    },
                    exception =>
                    {
                        lock (gate)
                            observer.OnError(exception);
                    },
                    () =>
                    {
                        lock (gate)
                        {
                            isStopped = true;
                            if (!hasLatest)
                                observer.OnCompleted();
                        }
                    });

                return new CompositeDisposable(subscription, innerSubscription);
            });
#endif
        }

        #endregion

        #region + TakeUntil +

        public virtual IObservable<TSource> TakeUntil<TSource, TOther>(IObservable<TSource> source, IObservable<TOther> other)
        {
#if !NO_PERF
            return new TakeUntil<TSource, TOther>(source, other);
#else
            return new AnonymousObservable<TSource>(observer =>
            {
                var sourceSubscription = new SingleAssignmentDisposable();
                var otherSubscription = new SingleAssignmentDisposable();

                var gate = new object();

                // COMPAT - Order of Subscribe calls per v1.0.10621
                otherSubscription.Disposable = other.Synchronize(gate).Subscribe(
                    x =>
                    {
                        observer.OnCompleted();
                    },
                    observer.OnError
                );

                sourceSubscription.Disposable = source.Synchronize(gate).Finally(otherSubscription.Dispose).Subscribe(observer);

                return new CompositeDisposable(sourceSubscription, otherSubscription);
            });
#endif
        }

        #endregion

        #region + Window +

        public virtual IObservable<IObservable<TSource>> Window<TSource, TWindowClosing>(IObservable<TSource> source, Func<IObservable<TWindowClosing>> windowClosingSelector)
        {
#if !NO_PERF
            return new Window<TSource, TWindowClosing>(source, windowClosingSelector);
#else
            return new AnonymousObservable<IObservable<TSource>>(observer =>
            {
                var window = new Subject<TSource>();
                var gate = new object();

                var m = new SerialDisposable();
                var d = new CompositeDisposable(2) { m };
                var r = new RefCountDisposable(d);

                observer.OnNext(window.AddRef(r));
                d.Add(source.SubscribeSafe(new AnonymousObserver<TSource>(
                    x =>
                    {
                        lock (gate)
                        {
                            window.OnNext(x);
                        }
                    },
                    ex =>
                    {
                        lock (gate)
                        {
                            window.OnError(ex);
                            observer.OnError(ex);
                        }
                    },
                    () =>
                    {
                        lock (gate)
                        {
                            window.OnCompleted();
                            observer.OnCompleted();
                        }
                    })));

                var l = new AsyncLock();

                Action createWindowClose = null;
                createWindowClose = () =>
                {
                    var windowClose = default(IObservable<TWindowClosing>);
                    try
                    {
                        windowClose = windowClosingSelector();
                    }
                    catch (Exception exception)
                    {
                        lock (gate)
                        {
                            observer.OnError(exception);
                        }
                        return;
                    }

                    var m1 = new SingleAssignmentDisposable();
                    m.Disposable = m1;
                    m1.Disposable = windowClose.Take(1).SubscribeSafe(new AnonymousObserver<TWindowClosing>(
                        Stubs<TWindowClosing>.Ignore,
                        ex =>
                        {
                            lock (gate)
                            {
                                window.OnError(ex);
                                observer.OnError(ex);
                            }
                        },
                        () =>
                        {
                            lock (gate)
                            {
                                window.OnCompleted();
                                window = new Subject<TSource>();
                                observer.OnNext(window.AddRef(r));
                            }
                            l.Wait(createWindowClose);
                        }));
                };

                l.Wait(createWindowClose);

                return r;
            });
#endif
        }

        public virtual IObservable<IObservable<TSource>> Window<TSource, TWindowOpening, TWindowClosing>(IObservable<TSource> source, IObservable<TWindowOpening> windowOpenings, Func<TWindowOpening, IObservable<TWindowClosing>> windowClosingSelector)
        {
            return windowOpenings.GroupJoin(source, windowClosingSelector, _ => Observable.Empty<Unit>(), (_, window) => window);
        }

        public virtual IObservable<IObservable<TSource>> Window<TSource, TWindowBoundary>(IObservable<TSource> source, IObservable<TWindowBoundary> windowBoundaries)
        {
#if !NO_PERF
            return new Window<TSource, TWindowBoundary>(source, windowBoundaries);
#else
            return new AnonymousObservable<IObservable<TSource>>(observer =>
            {
                var window = new Subject<TSource>();
                var gate = new object();

                var d = new CompositeDisposable(2);
                var r = new RefCountDisposable(d);

                observer.OnNext(window.AddRef(r));

                d.Add(source.SubscribeSafe(new AnonymousObserver<TSource>(
                    x =>
                    {
                        lock (gate)
                        {
                            window.OnNext(x);
                        }
                    },
                    ex =>
                    {
                        lock (gate)
                        {
                            window.OnError(ex);
                            observer.OnError(ex);
                        }
                    },
                    () =>
                    {
                        lock (gate)
                        {
                            window.OnCompleted();
                            observer.OnCompleted();
                        }
                    }
                )));

                d.Add(windowBoundaries.SubscribeSafe(new AnonymousObserver<TWindowBoundary>(
                    w =>
                    {
                        lock (gate)
                        {
                            window.OnCompleted();
                            window = new Subject<TSource>();
                            observer.OnNext(window.AddRef(r));
                        }
                    },
                    ex =>
                    {
                        lock (gate)
                        {
                            window.OnError(ex);
                            observer.OnError(ex);
                        }
                    },
                    () =>
                    {
                        lock (gate)
                        {
                            window.OnCompleted();
                            observer.OnCompleted();
                        }
                    }
                )));

                return r;
            });
#endif
        }

        #endregion

        #region + Zip +

        public virtual IObservable<TResult> Zip<TFirst, TSecond, TResult>(IObservable<TFirst> first, IObservable<TSecond> second, Func<TFirst, TSecond, TResult> resultSelector)
        {
#if !NO_PERF
            return new Zip<TFirst, TSecond, TResult>(first, second, resultSelector);
#else
            return new AnonymousObservable<TResult>(observer =>
            {
                var queueLeft = new Queue<TFirst>();
                var queueRight = new Queue<TSecond>();

                var leftDone = false;
                var rightDone = false;

                var leftSubscription = new SingleAssignmentDisposable();
                var rightSubscription = new SingleAssignmentDisposable();

                var gate = new object();

                leftSubscription.Disposable = first.Synchronize(gate).Subscribe(
                    l =>
                    {
                        if (queueRight.Count > 0)
                        {
                            var r = queueRight.Dequeue();

                            var res = default(TResult);
                            try
                            {
                                res = resultSelector(l, r);
                            }
                            catch (Exception ex)
                            {
                                observer.OnError(ex);
                                return;
                            }

                            observer.OnNext(res);
                        }
                        else
                        {
                            if (rightDone)
                            {
                                observer.OnCompleted();
                                return;
                            }

                            queueLeft.Enqueue(l);
                        }
                    },
                    observer.OnError,
                    () =>
                    {
                        leftDone = true;

                        if (rightDone)
                        {
                            observer.OnCompleted();
                            return;
                        }
                    }
                );

                rightSubscription.Disposable = second.Synchronize(gate).Subscribe(
                    r =>
                    {
                        if (queueLeft.Count > 0)
                        {
                            var l = queueLeft.Dequeue();

                            var res = default(TResult);
                            try
                            {
                                res = resultSelector(l, r);
                            }
                            catch (Exception ex)
                            {
                                observer.OnError(ex);
                                return;
                            }

                            observer.OnNext(res);
                        }
                        else
                        {
                            if (leftDone)
                            {
                                observer.OnCompleted();
                                return;
                            }

                            queueRight.Enqueue(r);
                        }
                    },
                    observer.OnError,
                    () =>
                    {
                        rightDone = true;

                        if (leftDone)
                        {
                            observer.OnCompleted();
                            return;
                        }
                    }
                );

                return new CompositeDisposable(leftSubscription, rightSubscription, Disposable.Create(() => { queueLeft.Clear(); queueRight.Clear(); }));
            });
#endif
        }

        public virtual IObservable<TResult> Zip<TSource, TResult>(IEnumerable<IObservable<TSource>> sources, Func<IList<TSource>, TResult> resultSelector)
        {
            return Zip_<TSource>(sources).Select(resultSelector);
        }

        public virtual IObservable<IList<TSource>> Zip<TSource>(IEnumerable<IObservable<TSource>> sources)
        {
            return Zip_<TSource>(sources);
        }

        public virtual IObservable<IList<TSource>> Zip<TSource>(params IObservable<TSource>[] sources)
        {
            return Zip_<TSource>(sources);
        }

        private static IObservable<IList<TSource>> Zip_<TSource>(IEnumerable<IObservable<TSource>> sources)
        {
#if !NO_PERF
            return new Zip<TSource>(sources);
#else
            return new AnonymousObservable<IList<TSource>>(observer =>
            {
                var srcs = sources.ToArray();

                var N = srcs.Length;

                var queues = new Queue<TSource>[N];
                for (int i = 0; i < N; i++)
                    queues[i] = new Queue<TSource>();

                var isDone = new bool[N];

                var next = new Action<int>(i =>
                {
                    if (queues.All(q => q.Count > 0))
                    {
                        var res = queues.Select(q => q.Dequeue()).ToList();
                        observer.OnNext(res);
                    }
                    else if (isDone.Where((x, j) => j != i).All(Stubs<bool>.I))
                    {
                        observer.OnCompleted();
                        return;
                    }
                });

                var done = new Action<int>(i =>
                {
                    isDone[i] = true;

                    if (isDone.All(Stubs<bool>.I))
                    {
                        observer.OnCompleted();
                        return;
                    }
                });

                var subscriptions = new SingleAssignmentDisposable[N];

                var gate = new object();

                for (int i = 0; i < N; i++)
                {
                    var j = i;
                    subscriptions[j] = new SingleAssignmentDisposable
                    {
                        Disposable = srcs[j].Synchronize(gate).Subscribe(
                            x =>
                            {
                                queues[j].Enqueue(x);
                                next(j);
                            },
                            observer.OnError,
                            () =>
                            {
                                done(j);
                            }
                        )
                    };
                }

                return new CompositeDisposable(subscriptions) { Disposable.Create(() => { foreach (var q in queues) q.Clear(); }) };
            });
#endif
        }

#if !NO_PERF

        /* The following code is generated by a tool checked in to $/.../Source/Tools/CodeGenerators. */

        #region Zip auto-generated code (6/10/2012 8:15:28 PM)

        public virtual IObservable<TResult> Zip<TSource1, TSource2, TSource3, TResult>(IObservable<TSource1> source1, IObservable<TSource2> source2, IObservable<TSource3> source3, Func<TSource1, TSource2, TSource3, TResult> resultSelector)
        {
            return new Zip<TSource1, TSource2, TSource3, TResult>(source1, source2, source3, resultSelector);
        }

        public virtual IObservable<TResult> Zip<TSource1, TSource2, TSource3, TSource4, TResult>(IObservable<TSource1> source1, IObservable<TSource2> source2, IObservable<TSource3> source3, IObservable<TSource4> source4, Func<TSource1, TSource2, TSource3, TSource4, TResult> resultSelector)
        {
            return new Zip<TSource1, TSource2, TSource3, TSource4, TResult>(source1, source2, source3, source4, resultSelector);
        }

#if !NO_LARGEARITY

        public virtual IObservable<TResult> Zip<TSource1, TSource2, TSource3, TSource4, TSource5, TResult>(IObservable<TSource1> source1, IObservable<TSource2> source2, IObservable<TSource3> source3, IObservable<TSource4> source4, IObservable<TSource5> source5, Func<TSource1, TSource2, TSource3, TSource4, TSource5, TResult> resultSelector)
        {
            return new Zip<TSource1, TSource2, TSource3, TSource4, TSource5, TResult>(source1, source2, source3, source4, source5, resultSelector);
        }

        public virtual IObservable<TResult> Zip<TSource1, TSource2, TSource3, TSource4, TSource5, TSource6, TResult>(IObservable<TSource1> source1, IObservable<TSource2> source2, IObservable<TSource3> source3, IObservable<TSource4> source4, IObservable<TSource5> source5, IObservable<TSource6> source6, Func<TSource1, TSource2, TSource3, TSource4, TSource5, TSource6, TResult> resultSelector)
        {
            return new Zip<TSource1, TSource2, TSource3, TSource4, TSource5, TSource6, TResult>(source1, source2, source3, source4, source5, source6, resultSelector);
        }

        public virtual IObservable<TResult> Zip<TSource1, TSource2, TSource3, TSource4, TSource5, TSource6, TSource7, TResult>(IObservable<TSource1> source1, IObservable<TSource2> source2, IObservable<TSource3> source3, IObservable<TSource4> source4, IObservable<TSource5> source5, IObservable<TSource6> source6, IObservable<TSource7> source7, Func<TSource1, TSource2, TSource3, TSource4, TSource5, TSource6, TSource7, TResult> resultSelector)
        {
            return new Zip<TSource1, TSource2, TSource3, TSource4, TSource5, TSource6, TSource7, TResult>(source1, source2, source3, source4, source5, source6, source7, resultSelector);
        }

        public virtual IObservable<TResult> Zip<TSource1, TSource2, TSource3, TSource4, TSource5, TSource6, TSource7, TSource8, TResult>(IObservable<TSource1> source1, IObservable<TSource2> source2, IObservable<TSource3> source3, IObservable<TSource4> source4, IObservable<TSource5> source5, IObservable<TSource6> source6, IObservable<TSource7> source7, IObservable<TSource8> source8, Func<TSource1, TSource2, TSource3, TSource4, TSource5, TSource6, TSource7, TSource8, TResult> resultSelector)
        {
            return new Zip<TSource1, TSource2, TSource3, TSource4, TSource5, TSource6, TSource7, TSource8, TResult>(source1, source2, source3, source4, source5, source6, source7, source8, resultSelector);
        }

        public virtual IObservable<TResult> Zip<TSource1, TSource2, TSource3, TSource4, TSource5, TSource6, TSource7, TSource8, TSource9, TResult>(IObservable<TSource1> source1, IObservable<TSource2> source2, IObservable<TSource3> source3, IObservable<TSource4> source4, IObservable<TSource5> source5, IObservable<TSource6> source6, IObservable<TSource7> source7, IObservable<TSource8> source8, IObservable<TSource9> source9, Func<TSource1, TSource2, TSource3, TSource4, TSource5, TSource6, TSource7, TSource8, TSource9, TResult> resultSelector)
        {
            return new Zip<TSource1, TSource2, TSource3, TSource4, TSource5, TSource6, TSource7, TSource8, TSource9, TResult>(source1, source2, source3, source4, source5, source6, source7, source8, source9, resultSelector);
        }

        public virtual IObservable<TResult> Zip<TSource1, TSource2, TSource3, TSource4, TSource5, TSource6, TSource7, TSource8, TSource9, TSource10, TResult>(IObservable<TSource1> source1, IObservable<TSource2> source2, IObservable<TSource3> source3, IObservable<TSource4> source4, IObservable<TSource5> source5, IObservable<TSource6> source6, IObservable<TSource7> source7, IObservable<TSource8> source8, IObservable<TSource9> source9, IObservable<TSource10> source10, Func<TSource1, TSource2, TSource3, TSource4, TSource5, TSource6, TSource7, TSource8, TSource9, TSource10, TResult> resultSelector)
        {
            return new Zip<TSource1, TSource2, TSource3, TSource4, TSource5, TSource6, TSource7, TSource8, TSource9, TSource10, TResult>(source1, source2, source3, source4, source5, source6, source7, source8, source9, source10, resultSelector);
        }

        public virtual IObservable<TResult> Zip<TSource1, TSource2, TSource3, TSource4, TSource5, TSource6, TSource7, TSource8, TSource9, TSource10, TSource11, TResult>(IObservable<TSource1> source1, IObservable<TSource2> source2, IObservable<TSource3> source3, IObservable<TSource4> source4, IObservable<TSource5> source5, IObservable<TSource6> source6, IObservable<TSource7> source7, IObservable<TSource8> source8, IObservable<TSource9> source9, IObservable<TSource10> source10, IObservable<TSource11> source11, Func<TSource1, TSource2, TSource3, TSource4, TSource5, TSource6, TSource7, TSource8, TSource9, TSource10, TSource11, TResult> resultSelector)
        {
            return new Zip<TSource1, TSource2, TSource3, TSource4, TSource5, TSource6, TSource7, TSource8, TSource9, TSource10, TSource11, TResult>(source1, source2, source3, source4, source5, source6, source7, source8, source9, source10, source11, resultSelector);
        }

        public virtual IObservable<TResult> Zip<TSource1, TSource2, TSource3, TSource4, TSource5, TSource6, TSource7, TSource8, TSource9, TSource10, TSource11, TSource12, TResult>(IObservable<TSource1> source1, IObservable<TSource2> source2, IObservable<TSource3> source3, IObservable<TSource4> source4, IObservable<TSource5> source5, IObservable<TSource6> source6, IObservable<TSource7> source7, IObservable<TSource8> source8, IObservable<TSource9> source9, IObservable<TSource10> source10, IObservable<TSource11> source11, IObservable<TSource12> source12, Func<TSource1, TSource2, TSource3, TSource4, TSource5, TSource6, TSource7, TSource8, TSource9, TSource10, TSource11, TSource12, TResult> resultSelector)
        {
            return new Zip<TSource1, TSource2, TSource3, TSource4, TSource5, TSource6, TSource7, TSource8, TSource9, TSource10, TSource11, TSource12, TResult>(source1, source2, source3, source4, source5, source6, source7, source8, source9, source10, source11, source12, resultSelector);
        }

        public virtual IObservable<TResult> Zip<TSource1, TSource2, TSource3, TSource4, TSource5, TSource6, TSource7, TSource8, TSource9, TSource10, TSource11, TSource12, TSource13, TResult>(IObservable<TSource1> source1, IObservable<TSource2> source2, IObservable<TSource3> source3, IObservable<TSource4> source4, IObservable<TSource5> source5, IObservable<TSource6> source6, IObservable<TSource7> source7, IObservable<TSource8> source8, IObservable<TSource9> source9, IObservable<TSource10> source10, IObservable<TSource11> source11, IObservable<TSource12> source12, IObservable<TSource13> source13, Func<TSource1, TSource2, TSource3, TSource4, TSource5, TSource6, TSource7, TSource8, TSource9, TSource10, TSource11, TSource12, TSource13, TResult> resultSelector)
        {
            return new Zip<TSource1, TSource2, TSource3, TSource4, TSource5, TSource6, TSource7, TSource8, TSource9, TSource10, TSource11, TSource12, TSource13, TResult>(source1, source2, source3, source4, source5, source6, source7, source8, source9, source10, source11, source12, source13, resultSelector);
        }

        public virtual IObservable<TResult> Zip<TSource1, TSource2, TSource3, TSource4, TSource5, TSource6, TSource7, TSource8, TSource9, TSource10, TSource11, TSource12, TSource13, TSource14, TResult>(IObservable<TSource1> source1, IObservable<TSource2> source2, IObservable<TSource3> source3, IObservable<TSource4> source4, IObservable<TSource5> source5, IObservable<TSource6> source6, IObservable<TSource7> source7, IObservable<TSource8> source8, IObservable<TSource9> source9, IObservable<TSource10> source10, IObservable<TSource11> source11, IObservable<TSource12> source12, IObservable<TSource13> source13, IObservable<TSource14> source14, Func<TSource1, TSource2, TSource3, TSource4, TSource5, TSource6, TSource7, TSource8, TSource9, TSource10, TSource11, TSource12, TSource13, TSource14, TResult> resultSelector)
        {
            return new Zip<TSource1, TSource2, TSource3, TSource4, TSource5, TSource6, TSource7, TSource8, TSource9, TSource10, TSource11, TSource12, TSource13, TSource14, TResult>(source1, source2, source3, source4, source5, source6, source7, source8, source9, source10, source11, source12, source13, source14, resultSelector);
        }

        public virtual IObservable<TResult> Zip<TSource1, TSource2, TSource3, TSource4, TSource5, TSource6, TSource7, TSource8, TSource9, TSource10, TSource11, TSource12, TSource13, TSource14, TSource15, TResult>(IObservable<TSource1> source1, IObservable<TSource2> source2, IObservable<TSource3> source3, IObservable<TSource4> source4, IObservable<TSource5> source5, IObservable<TSource6> source6, IObservable<TSource7> source7, IObservable<TSource8> source8, IObservable<TSource9> source9, IObservable<TSource10> source10, IObservable<TSource11> source11, IObservable<TSource12> source12, IObservable<TSource13> source13, IObservable<TSource14> source14, IObservable<TSource15> source15, Func<TSource1, TSource2, TSource3, TSource4, TSource5, TSource6, TSource7, TSource8, TSource9, TSource10, TSource11, TSource12, TSource13, TSource14, TSource15, TResult> resultSelector)
        {
            return new Zip<TSource1, TSource2, TSource3, TSource4, TSource5, TSource6, TSource7, TSource8, TSource9, TSource10, TSource11, TSource12, TSource13, TSource14, TSource15, TResult>(source1, source2, source3, source4, source5, source6, source7, source8, source9, source10, source11, source12, source13, source14, source15, resultSelector);
        }

        public virtual IObservable<TResult> Zip<TSource1, TSource2, TSource3, TSource4, TSource5, TSource6, TSource7, TSource8, TSource9, TSource10, TSource11, TSource12, TSource13, TSource14, TSource15, TSource16, TResult>(IObservable<TSource1> source1, IObservable<TSource2> source2, IObservable<TSource3> source3, IObservable<TSource4> source4, IObservable<TSource5> source5, IObservable<TSource6> source6, IObservable<TSource7> source7, IObservable<TSource8> source8, IObservable<TSource9> source9, IObservable<TSource10> source10, IObservable<TSource11> source11, IObservable<TSource12> source12, IObservable<TSource13> source13, IObservable<TSource14> source14, IObservable<TSource15> source15, IObservable<TSource16> source16, Func<TSource1, TSource2, TSource3, TSource4, TSource5, TSource6, TSource7, TSource8, TSource9, TSource10, TSource11, TSource12, TSource13, TSource14, TSource15, TSource16, TResult> resultSelector)
        {
            return new Zip<TSource1, TSource2, TSource3, TSource4, TSource5, TSource6, TSource7, TSource8, TSource9, TSource10, TSource11, TSource12, TSource13, TSource14, TSource15, TSource16, TResult>(source1, source2, source3, source4, source5, source6, source7, source8, source9, source10, source11, source12, source13, source14, source15, source16, resultSelector);
        }

#endif

        #endregion

#endif

        public virtual IObservable<TResult> Zip<TFirst, TSecond, TResult>(IObservable<TFirst> first, IEnumerable<TSecond> second, Func<TFirst, TSecond, TResult> resultSelector)
        {
#if !NO_PERF
            return new Zip<TFirst, TSecond, TResult>(first, second, resultSelector);
#else
            return new AnonymousObservable<TResult>(observer =>
            {
                var rightEnumerator = second.GetEnumerator();
                var leftSubscription = first.Subscribe(left =>
                    {
                        var hasNext = false;
                        try
                        {
                            hasNext = rightEnumerator.MoveNext();
                        }
                        catch (Exception ex)
                        {
                            observer.OnError(ex);
                            return;
                        }

                        if (hasNext)
                        {
                            var right = default(TSecond);
                            try
                            {
                                right = rightEnumerator.Current;
                            }
                            catch (Exception ex)
                            {
                                observer.OnError(ex);
                                return;
                            }

                            TResult result;
                            try
                            {
                                result = resultSelector(left, right);
                            }
                            catch (Exception ex)
                            {
                                observer.OnError(ex);
                                return;
                            }
                            observer.OnNext(result);
                        }
                        else
                        {
                            observer.OnCompleted();
                        }
                    },
                    observer.OnError,
                    observer.OnCompleted
                );

                return new CompositeDisposable(leftSubscription, rightEnumerator);
            });
#endif
        }

        #endregion

        #region |> Helpers <|

#if NO_PERF

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

#endif

        #endregion
    }
}

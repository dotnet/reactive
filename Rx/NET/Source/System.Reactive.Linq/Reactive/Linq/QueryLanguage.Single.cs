// Copyright (c) Microsoft Open Technologies, Inc. All rights reserved. See License.txt in the project root for license information.

using System.Collections.Generic;
using System.Linq;
using System.Reactive.Concurrency;
using System.Reactive.Disposables;
using System.Reactive.Subjects;

namespace System.Reactive.Linq
{
#if !NO_PERF
    using Observαble;
#endif

    internal partial class QueryLanguage
    {
        #region + AsObservable +

        public virtual IObservable<TSource> AsObservable<TSource>(IObservable<TSource> source)
        {
#if !NO_PERF
            var asObservable = source as AsObservable<TSource>;
            if (asObservable != null)
                return asObservable.Ω();

            return new AsObservable<TSource>(source);
#else
            return new AnonymousObservable<TSource>(observer => source.Subscribe(observer));
#endif
        }

        #endregion

        #region + Buffer +

        public virtual IObservable<IList<TSource>> Buffer<TSource>(IObservable<TSource> source, int count)
        {
            return Buffer_<TSource>(source, count, count);
        }

        public virtual IObservable<IList<TSource>> Buffer<TSource>(IObservable<TSource> source, int count, int skip)
        {
            return Buffer_<TSource>(source, count, skip);
        }

        private static IObservable<IList<TSource>> Buffer_<TSource>(IObservable<TSource> source, int count, int skip)
        {
#if !NO_PERF
            return new Buffer<TSource>(source, count, skip);
#else
            return Window_<TSource>(source, count, skip).SelectMany(Observable.ToList).Where(list => list.Count > 0);
#endif
        }

        #endregion

        #region + Dematerialize +

        public virtual IObservable<TSource> Dematerialize<TSource>(IObservable<Notification<TSource>> source)
        {
#if !NO_PERF
            var materialize = source as Materialize<TSource>;
            if (materialize != null)
                return materialize.Dematerialize();

            return new Dematerialize<TSource>(source);
#else
            return new AnonymousObservable<TSource>(observer =>
                source.Subscribe(x => x.Accept(observer), observer.OnError, observer.OnCompleted));
#endif
        }

        #endregion

        #region + DistinctUntilChanged +

        public virtual IObservable<TSource> DistinctUntilChanged<TSource>(IObservable<TSource> source)
        {
            return DistinctUntilChanged_(source, x => x, EqualityComparer<TSource>.Default);
        }

        public virtual IObservable<TSource> DistinctUntilChanged<TSource>(IObservable<TSource> source, IEqualityComparer<TSource> comparer)
        {
            return DistinctUntilChanged_(source, x => x, comparer);
        }

        public virtual IObservable<TSource> DistinctUntilChanged<TSource, TKey>(IObservable<TSource> source, Func<TSource, TKey> keySelector)
        {
            return DistinctUntilChanged_(source, keySelector, EqualityComparer<TKey>.Default);
        }

        public virtual IObservable<TSource> DistinctUntilChanged<TSource, TKey>(IObservable<TSource> source, Func<TSource, TKey> keySelector, IEqualityComparer<TKey> comparer)
        {
            return DistinctUntilChanged_(source, keySelector, comparer);
        }

        private static IObservable<TSource> DistinctUntilChanged_<TSource, TKey>(IObservable<TSource> source, Func<TSource, TKey> keySelector, IEqualityComparer<TKey> comparer)
        {
#if !NO_PERF
            return new DistinctUntilChanged<TSource, TKey>(source, keySelector, comparer);
#else
            return new AnonymousObservable<TSource>(observer =>
            {
                var currentKey = default(TKey);
                var hasCurrentKey = false;
                return source.Subscribe(
                    value =>
                    {
                        var key = default(TKey);
                        try
                        {
                            key = keySelector(value);
                        }
                        catch (Exception exception)
                        {
                            observer.OnError(exception);
                            return;
                        }
                        var comparerEquals = false;
                        if (hasCurrentKey)
                        {
                            try
                            {
                                comparerEquals = comparer.Equals(currentKey, key);
                            }
                            catch (Exception exception)
                            {
                                observer.OnError(exception);
                                return;
                            }
                        }
                        if (!hasCurrentKey || !comparerEquals)
                        {
                            hasCurrentKey = true;
                            currentKey = key;
                            observer.OnNext(value);
                        }
                    },
                    observer.OnError,
                    observer.OnCompleted);
            });
#endif
        }

        #endregion

        #region + Do +

        public virtual IObservable<TSource> Do<TSource>(IObservable<TSource> source, Action<TSource> onNext)
        {
#if !NO_PERF
            return Do_<TSource>(source, onNext, Stubs<Exception>.Ignore, Stubs.Nop);
#else
            // PERFORMANCE - Use of Select allows for operator coalescing
            return source.Select(
                x =>
                {
                    onNext(x);
                    return x;
                }
            );
#endif
        }

        public virtual IObservable<TSource> Do<TSource>(IObservable<TSource> source, Action<TSource> onNext, Action onCompleted)
        {
#if !NO_PERF
            return Do_<TSource>(source, onNext, Stubs<Exception>.Ignore, onCompleted);
#else
            return new AnonymousObservable<TSource>(obs =>
            {
                return source.Subscribe(
                    x =>
                    {
                        try
                        {
                            onNext(x);
                        }
                        catch (Exception ex)
                        {
                            obs.OnError(ex);
                        }
                        obs.OnNext(x);
                    },
                    obs.OnError,
                    () =>
                    {
                        try
                        {
                            onCompleted();
                        }
                        catch (Exception ex)
                        {
                            obs.OnError(ex);
                        }
                        obs.OnCompleted();
                    });
            });
#endif
        }

        public virtual IObservable<TSource> Do<TSource>(IObservable<TSource> source, Action<TSource> onNext, Action<Exception> onError)
        {
#if !NO_PERF
            return Do_<TSource>(source, onNext, onError, Stubs.Nop);
#else
            return new AnonymousObservable<TSource>(obs =>
            {
                return source.Subscribe(
                    x =>
                    {
                        try
                        {
                            onNext(x);
                        }
                        catch (Exception ex)
                        {
                            obs.OnError(ex);
                        }
                        obs.OnNext(x);
                    },
                    ex =>
                    {
                        try
                        {
                            onError(ex);
                        }
                        catch (Exception ex2)
                        {
                            obs.OnError(ex2);
                        }
                        obs.OnError(ex);
                    },
                    obs.OnCompleted);
            });
#endif
        }

        public virtual IObservable<TSource> Do<TSource>(IObservable<TSource> source, Action<TSource> onNext, Action<Exception> onError, Action onCompleted)
        {
            return Do_(source, onNext, onError, onCompleted);
        }

        public virtual IObservable<TSource> Do<TSource>(IObservable<TSource> source, IObserver<TSource> observer)
        {
            return Do_(source, observer.OnNext, observer.OnError, observer.OnCompleted);
        }

        private static IObservable<TSource> Do_<TSource>(IObservable<TSource> source, Action<TSource> onNext, Action<Exception> onError, Action onCompleted)
        {
#if !NO_PERF
            return new Do<TSource>(source, onNext, onError, onCompleted);
#else
            return new AnonymousObservable<TSource>(obs =>
            {
                return source.Subscribe(
                    x =>
                    {
                        try
                        {
                            onNext(x);
                        }
                        catch (Exception ex)
                        {
                            obs.OnError(ex);
                        }
                        obs.OnNext(x);
                    },
                    ex =>
                    {
                        try
                        {
                            onError(ex);
                        }
                        catch (Exception ex2)
                        {
                            obs.OnError(ex2);
                        }
                        obs.OnError(ex);
                    },
                    () =>
                    {
                        try
                        {
                            onCompleted();
                        }
                        catch (Exception ex)
                        {
                            obs.OnError(ex);
                        }
                        obs.OnCompleted();
                    });
            });
#endif
        }

        #endregion

        #region + Finally +

        public virtual IObservable<TSource> Finally<TSource>(IObservable<TSource> source, Action finallyAction)
        {
#if !NO_PERF
            return new Finally<TSource>(source, finallyAction);
#else
            return new AnonymousObservable<TSource>(observer =>
            {
                var subscription = source.Subscribe(observer);

                return Disposable.Create(() =>
                    {
                        try
                        {
                            subscription.Dispose();
                        }
                        finally
                        {
                            finallyAction();
                        }
                    });
            });
#endif
        }

        #endregion

        #region + IgnoreElements +

        public virtual IObservable<TSource> IgnoreElements<TSource>(IObservable<TSource> source)
        {
#if !NO_PERF
            var ignoreElements = source as IgnoreElements<TSource>;
            if (ignoreElements != null)
                return ignoreElements.Ω();

            return new IgnoreElements<TSource>(source);
#else
            return new AnonymousObservable<TSource>(observer => source.Subscribe(_ => { }, observer.OnError, observer.OnCompleted));
#endif
        }

        #endregion

        #region + Materialize +

        public virtual IObservable<Notification<TSource>> Materialize<TSource>(IObservable<TSource> source)
        {
#if !NO_PERF
            //
            // NOTE: Peephole optimization of xs.Dematerialize().Materialize() should not be performed. It's possible for xs to
            //       contain multiple terminal notifications, which won't survive a Dematerialize().Materialize() chain. In case
            //       a reduction to xs.AsObservable() would be performed, those notification elements would survive.
            //

            return new Materialize<TSource>(source);
#else
            return new AnonymousObservable<Notification<TSource>>(observer =>
                source.Subscribe(
                    value => observer.OnNext(Notification.CreateOnNext<TSource>(value)),
                    exception =>
                    {
                        observer.OnNext(Notification.CreateOnError<TSource>(exception));
                        observer.OnCompleted();
                    },
                    () =>
                    {
                        observer.OnNext(Notification.CreateOnCompleted<TSource>());
                        observer.OnCompleted();
                    }));
#endif
        }

        #endregion

        #region - Repeat -

        public virtual IObservable<TSource> Repeat<TSource>(IObservable<TSource> source)
        {
            return RepeatInfinite(source).Concat();
        }

        private static IEnumerable<T> RepeatInfinite<T>(T value)
        {
            while (true)
                yield return value;
        }

        public virtual IObservable<TSource> Repeat<TSource>(IObservable<TSource> source, int repeatCount)
        {
            return Enumerable.Repeat(source, repeatCount).Concat();
        }

        #endregion

        #region - Retry -

        public virtual IObservable<TSource> Retry<TSource>(IObservable<TSource> source)
        {
            return RepeatInfinite(source).Catch();
        }

        public virtual IObservable<TSource> Retry<TSource>(IObservable<TSource> source, int retryCount)
        {
            return Enumerable.Repeat(source, retryCount).Catch();
        }

        #endregion

        #region + Scan +

        public virtual IObservable<TAccumulate> Scan<TSource, TAccumulate>(IObservable<TSource> source, TAccumulate seed, Func<TAccumulate, TSource, TAccumulate> accumulator)
        {
#if !NO_PERF
            return new Scan<TSource, TAccumulate>(source, seed, accumulator);
#else
            return Defer(() =>
            {
                var accumulation = default(TAccumulate);
                var hasAccumulation = false;
                return source.Select(x =>
                {
                    if (hasAccumulation)
                        accumulation = accumulator(accumulation, x);
                    else
                    {
                        accumulation = accumulator(seed, x);
                        hasAccumulation = true;
                    }
                    return accumulation;
                });
            });
#endif
        }

        public virtual IObservable<TSource> Scan<TSource>(IObservable<TSource> source, Func<TSource, TSource, TSource> accumulator)
        {
#if !NO_PERF
            return new Scan<TSource>(source, accumulator);
#else
            return Defer(() =>
            {
                var accumulation = default(TSource);
                var hasAccumulation = false;
                return source.Select(x =>
                {
                    if (hasAccumulation)
                        accumulation = accumulator(accumulation, x);
                    else
                    {
                        accumulation = x;
                        hasAccumulation = true;
                    }
                    return accumulation;
                });
            });
#endif
        }

        #endregion

        #region + SkipLast +

        public virtual IObservable<TSource> SkipLast<TSource>(IObservable<TSource> source, int count)
        {
#if !NO_PERF
            return new SkipLast<TSource>(source, count);
#else
            return new AnonymousObservable<TSource>(observer =>
            {
                var q = new Queue<TSource>();

                return source.Subscribe(
                    x =>
                    {
                        q.Enqueue(x);
                        if (q.Count > count)
                            observer.OnNext(q.Dequeue());
                    },
                    observer.OnError,
                    observer.OnCompleted);
            });
#endif
        }

        #endregion

        #region - StartWith -

        public virtual IObservable<TSource> StartWith<TSource>(IObservable<TSource> source, params TSource[] values)
        {
            return StartWith_<TSource>(source, SchedulerDefaults.ConstantTimeOperations, values);
        }

        public virtual IObservable<TSource> StartWith<TSource>(IObservable<TSource> source, IScheduler scheduler, params TSource[] values)
        {
            return StartWith_<TSource>(source, scheduler, values);
        }

        public virtual IObservable<TSource> StartWith<TSource>(IObservable<TSource> source, IEnumerable<TSource> values)
        {
            return StartWith(source, SchedulerDefaults.ConstantTimeOperations, values);
        }

        public virtual IObservable<TSource> StartWith<TSource>(IObservable<TSource> source, IScheduler scheduler, IEnumerable<TSource> values)
        {
            if (values == null)
                throw new ArgumentNullException("values");

            var valueArray = values as TSource[];
            if (valueArray == null)
            {
                List<TSource> valueList = new List<TSource>(values);
                valueArray = valueList.ToArray();
            }
            return StartWith_<TSource>(source, scheduler, valueArray);
        }

        private static IObservable<TSource> StartWith_<TSource>(IObservable<TSource> source, IScheduler scheduler, params TSource[] values)
        {
            return values.ToObservable(scheduler).Concat(source);
        }

        #endregion

        #region + TakeLast +

        public virtual IObservable<TSource> TakeLast<TSource>(IObservable<TSource> source, int count)
        {
            return TakeLast_(source, count, SchedulerDefaults.Iteration);
        }

        public virtual IObservable<TSource> TakeLast<TSource>(IObservable<TSource> source, int count, IScheduler scheduler)
        {
            return TakeLast_(source, count, scheduler);
        }

        private static IObservable<TSource> TakeLast_<TSource>(IObservable<TSource> source, int count, IScheduler scheduler)
        {
#if !NO_PERF
            return new TakeLast<TSource>(source, count, scheduler);
#else
            return new AnonymousObservable<TSource>(observer =>
            {
                var q = new Queue<TSource>();

                var g = new CompositeDisposable();

                g.Add(source.Subscribe(
                    x =>
                    {
                        q.Enqueue(x);
                        if (q.Count > count)
                            q.Dequeue();
                    },
                    observer.OnError,
                    () =>
                    {
                        g.Add(scheduler.Schedule(rec =>
                        {
                            if (q.Count > 0)
                            {
                                observer.OnNext(q.Dequeue());
                                rec();
                            }
                            else
                            {
                                observer.OnCompleted();
                            }
                        }));
                    }
                ));

                return g;
            });
#endif
        }

        public virtual IObservable<IList<TSource>> TakeLastBuffer<TSource>(IObservable<TSource> source, int count)
        {
#if !NO_PERF
            return new TakeLastBuffer<TSource>(source, count);
#else
            return new AnonymousObservable<IList<TSource>>(observer =>
            {
                var q = new Queue<TSource>();

                return source.Subscribe(
                    x =>
                    {
                        q.Enqueue(x);
                        if (q.Count > count)
                            q.Dequeue();
                    },
                    observer.OnError,
                    () =>
                    {
                        observer.OnNext(q.ToList());
                        observer.OnCompleted();
                    });
            });
#endif
        }

        #endregion

        #region + Window +

        public virtual IObservable<IObservable<TSource>> Window<TSource>(IObservable<TSource> source, int count, int skip)
        {
            return Window_<TSource>(source, count, skip);
        }

        public virtual IObservable<IObservable<TSource>> Window<TSource>(IObservable<TSource> source, int count)
        {
            return Window_<TSource>(source, count, count);
        }

        private static IObservable<IObservable<TSource>> Window_<TSource>(IObservable<TSource> source, int count, int skip)
        {
#if !NO_PERF
            return new Window<TSource>(source, count, skip);
#else
            return new AnonymousObservable<IObservable<TSource>>(observer =>
            {
                var q = new Queue<ISubject<TSource>>();
                var n = 0;

                var m = new SingleAssignmentDisposable();
                var refCountDisposable = new RefCountDisposable(m);

                Action createWindow = () =>
                {
                    var s = new Subject<TSource>();
                    q.Enqueue(s);
                    observer.OnNext(s.AddRef(refCountDisposable));
                };

                createWindow();

                m.Disposable = source.Subscribe(
                    x =>
                    {
                        foreach (var s in q)
                            s.OnNext(x);

                        var c = n - count + 1;
                        if (c >= 0 && c % skip == 0)
                        {
                            var s = q.Dequeue();
                            s.OnCompleted();
                        }

                        n++;
                        if (n % skip == 0)
                            createWindow();
                    },
                    exception =>
                    {
                        while (q.Count > 0)
                            q.Dequeue().OnError(exception);

                        observer.OnError(exception);
                    },
                    () =>
                    {
                        while (q.Count > 0)
                            q.Dequeue().OnCompleted();

                        observer.OnCompleted();
                    }
                );

                return refCountDisposable;
            });
#endif
        }

        #endregion
    }
}

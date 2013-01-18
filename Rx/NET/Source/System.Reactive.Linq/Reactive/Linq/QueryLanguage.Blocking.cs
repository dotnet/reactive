// Copyright (c) Microsoft Open Technologies, Inc. All rights reserved. See License.txt in the project root for license information.

using System.Collections;
using System.Collections.Generic;
using System.Threading;
using System.Reactive.Disposables;

#if NO_SEMAPHORE
using System.Reactive.Threading;
#endif

namespace System.Reactive.Linq
{
#if !NO_PERF
    using Observαble;
#endif

    internal partial class QueryLanguage
    {
        #region - Chunkify -

        public virtual IEnumerable<IList<TSource>> Chunkify<TSource>(IObservable<TSource> source)
        {
            return source.Collect<TSource, IList<TSource>>(() => new List<TSource>(), (lst, x) => { lst.Add(x); return lst; }, _ => new List<TSource>());
        }

        #endregion

        #region + Collect +

        public virtual IEnumerable<TResult> Collect<TSource, TResult>(IObservable<TSource> source, Func<TResult> newCollector, Func<TResult, TSource, TResult> merge)
        {
            return Collect_<TSource, TResult>(source, newCollector, merge, _ => newCollector());
        }

        public virtual IEnumerable<TResult> Collect<TSource, TResult>(IObservable<TSource> source, Func<TResult> getInitialCollector, Func<TResult, TSource, TResult> merge, Func<TResult, TResult> getNewCollector)
        {
            return Collect_<TSource, TResult>(source, getInitialCollector, merge, getNewCollector);
        }

        private static IEnumerable<TResult> Collect_<TSource, TResult>(IObservable<TSource> source, Func<TResult> getInitialCollector, Func<TResult, TSource, TResult> merge, Func<TResult, TResult> getNewCollector)
        {
#if !NO_PERF
            return new Collect<TSource, TResult>(source, getInitialCollector, merge, getNewCollector);
#else
            return new AnonymousEnumerable<TResult>(() =>
            {
                var c = getInitialCollector();
                var f = default(Notification<TSource>);
                var o = new object();
                var done = false;
                return PushToPull<TSource, TResult>(
                    source,
                    x =>
                    {
                        lock (o)
                        {
                            if (x.HasValue)
                            {
                                try
                                {
                                    c = merge(c, x.Value);
                                }
                                catch (Exception ex)
                                {
                                    f = Notification.CreateOnError<TSource>(ex);
                                }
                            }
                            else
                                f = x;
                        }
                    },
                    () =>
                    {
                        if (f != null)
                        {
                            if (f.Kind == NotificationKind.OnError)
                            {
                                return Notification.CreateOnError<TResult>(f.Exception);
                            }
                            else
                            {
                                if (done)
                                    return Notification.CreateOnCompleted<TResult>();
                                else
                                    done = true;
                            }
                        }

                        var l = default(TResult);
                        lock (o)
                        {
                            l = c;
                            c = getNewCollector(c);
                        }

                        return Notification.CreateOnNext(l);
                    }
                );
            });
#endif
        }

        #endregion

        #region First

        public virtual TSource First<TSource>(IObservable<TSource> source)
        {
            return FirstOrDefaultInternal(source, true);
        }

        public virtual TSource First<TSource>(IObservable<TSource> source, Func<TSource, bool> predicate)
        {
            return First(Where(source, predicate));
        }

        #endregion

        #region FirstOrDefault

        public virtual TSource FirstOrDefault<TSource>(IObservable<TSource> source)
        {
            return FirstOrDefaultInternal(source, false);
        }

        public virtual TSource FirstOrDefault<TSource>(IObservable<TSource> source, Func<TSource, bool> predicate)
        {
            return FirstOrDefault(Where(source, predicate));
        }

        private static TSource FirstOrDefaultInternal<TSource>(IObservable<TSource> source, bool throwOnEmpty)
        {
            var value = default(TSource);
            var seenValue = false;
            var ex = default(Exception);
            var evt = new ManualResetEvent(false);

            //
            // [OK] Use of unsafe Subscribe: fine to throw to our caller, behavior indistinguishable from going through the sink.
            //
            using (source.Subscribe/*Unsafe*/(new AnonymousObserver<TSource>(
                v =>
                {
                    if (!seenValue)
                    {
                        value = v;
                    }
                    seenValue = true;
                    evt.Set();
                },
                e =>
                {
                    ex = e;
                    evt.Set();
                },
                () =>
                {
                    evt.Set();
                })))
            {
                evt.WaitOne();
            }

            ex.ThrowIfNotNull();

            if (throwOnEmpty && !seenValue)
                throw new InvalidOperationException(Strings_Linq.NO_ELEMENTS);

            return value;
        }

        #endregion

        #region + ForEach +

        public virtual void ForEach<TSource>(IObservable<TSource> source, Action<TSource> onNext)
        {
#if !NO_PERF
            var evt = new ManualResetEvent(false);
            var sink = new ForEach<TSource>._(onNext, () => evt.Set());

            using (source.SubscribeSafe(sink))
            {
                evt.WaitOne();
            }

            sink.Error.ThrowIfNotNull();
#else
            ForEach_(source, onNext);
#endif
        }

        public virtual void ForEach<TSource>(IObservable<TSource> source, Action<TSource, int> onNext)
        {
#if !NO_PERF
            var evt = new ManualResetEvent(false);
            var sink = new ForEach<TSource>.τ(onNext, () => evt.Set());

            using (source.SubscribeSafe(sink))
            {
                evt.WaitOne();
            }

            sink.Error.ThrowIfNotNull();
#else
            var i = 0;
            ForEach_(source, x => onNext(x, checked(i++)));
#endif
        }

#if NO_PERF
        private static void ForEach_<TSource>(IObservable<TSource> source, Action<TSource> onNext)
        {
            var exception = default(Exception);

            var evt = new ManualResetEvent(false);
            using (source.Subscribe(
                x =>
                {
                    try
                    {
                        onNext(x);
                    }
                    catch (Exception ex)
                    {
                        exception = ex;
                        evt.Set();
                    }
                },
                ex =>
                {
                    exception = ex;
                    evt.Set();
                },
                () => evt.Set()
            ))
            {
                evt.WaitOne();
            }

            if (exception != null)
                exception.Throw();
        }
#endif

        #endregion

        #region + GetEnumerator +

        public virtual IEnumerator<TSource> GetEnumerator<TSource>(IObservable<TSource> source)
        {
#if !NO_PERF && !NO_CDS
            var e = new GetEnumerator<TSource>();
            return e.Run(source);
#else
            var q = new Queue<Notification<TSource>>();
            var s = new Semaphore(0, int.MaxValue);
            return PushToPull(
                source,
                x =>
                {
                    lock (q)
                        q.Enqueue(x);
                    s.Release();
                },
                () =>
                {
                    s.WaitOne();
                    lock (q)
                        return q.Dequeue();
                });
#endif
        }

        #endregion

        #region Last

        public virtual TSource Last<TSource>(IObservable<TSource> source)
        {
            return LastOrDefaultInternal(source, true);
        }

        public virtual TSource Last<TSource>(IObservable<TSource> source, Func<TSource, bool> predicate)
        {
            return Last(Where(source, predicate));
        }

        #endregion

        #region LastOrDefault

        public virtual TSource LastOrDefault<TSource>(IObservable<TSource> source)
        {
            return LastOrDefaultInternal(source, false);
        }

        public virtual TSource LastOrDefault<TSource>(IObservable<TSource> source, Func<TSource, bool> predicate)
        {
            return LastOrDefault(Where(source, predicate));
        }

        private static TSource LastOrDefaultInternal<TSource>(IObservable<TSource> source, bool throwOnEmpty)
        {
            var value = default(TSource);
            var seenValue = false;
            var ex = default(Exception);
            var evt = new ManualResetEvent(false);

            //
            // [OK] Use of unsafe Subscribe: fine to throw to our caller, behavior indistinguishable from going through the sink.
            //
            using (source.Subscribe/*Unsafe*/(new AnonymousObserver<TSource>(
                v =>
                {
                    seenValue = true;
                    value = v;
                },
                e =>
                {
                    ex = e;
                    evt.Set();
                },
                () =>
                {
                    evt.Set();
                })))
            {
                evt.WaitOne();
            }

            ex.ThrowIfNotNull();

            if (throwOnEmpty && !seenValue)
                throw new InvalidOperationException(Strings_Linq.NO_ELEMENTS);

            return value;
        }

        #endregion

        #region + Latest +

        public virtual IEnumerable<TSource> Latest<TSource>(IObservable<TSource> source)
        {
            return new Latest<TSource>(source);
        }

        #endregion

        #region + MostRecent +

        public virtual IEnumerable<TSource> MostRecent<TSource>(IObservable<TSource> source, TSource initialValue)
        {
            return new MostRecent<TSource>(source, initialValue);
        }

        #endregion

        #region + Next +

        public virtual IEnumerable<TSource> Next<TSource>(IObservable<TSource> source)
        {
            return new Next<TSource>(source);
        }

        #endregion

        #region Single

        public virtual TSource Single<TSource>(IObservable<TSource> source)
        {
            return SingleOrDefaultInternal(source, true);
        }

        public virtual TSource Single<TSource>(IObservable<TSource> source, Func<TSource, bool> predicate)
        {
            return Single(Where(source, predicate));
        }

        #endregion

        #region SingleOrDefault

        public virtual TSource SingleOrDefault<TSource>(IObservable<TSource> source)
        {
            return SingleOrDefaultInternal(source, false);
        }

        public virtual TSource SingleOrDefault<TSource>(IObservable<TSource> source, Func<TSource, bool> predicate)
        {
            return SingleOrDefault(Where(source, predicate));
        }

        private static TSource SingleOrDefaultInternal<TSource>(IObservable<TSource> source, bool throwOnEmpty)
        {
            var value = default(TSource);
            var seenValue = false;
            var ex = default(Exception);
            var evt = new ManualResetEvent(false);

            //
            // [OK] Use of unsafe Subscribe: fine to throw to our caller, behavior indistinguishable from going through the sink.
            //
            using (source.Subscribe/*Unsafe*/(new AnonymousObserver<TSource>(
                v =>
                {
                    if (seenValue)
                    {
                        ex = new InvalidOperationException(Strings_Linq.MORE_THAN_ONE_ELEMENT);
                        evt.Set();
                    }

                    value = v;
                    seenValue = true;
                },
                e =>
                {
                    ex = e;
                    evt.Set();
                },
                () =>
                {
                    evt.Set();
                })))
            {
                evt.WaitOne();
            }

            ex.ThrowIfNotNull();

            if (throwOnEmpty && !seenValue)
                throw new InvalidOperationException(Strings_Linq.NO_ELEMENTS);

            return value;
        }

        #endregion

        #region Wait

        public virtual TSource Wait<TSource>(IObservable<TSource> source)
        {
            return LastOrDefaultInternal(source, true);
        }

        #endregion

        #region |> Helpers <|

#if NO_CDS || NO_PERF
        private static IEnumerator<TResult> PushToPull<TSource, TResult>(IObservable<TSource> source, Action<Notification<TSource>> push, Func<Notification<TResult>> pull)
        {
            var subscription = new SingleAssignmentDisposable();
            var adapter = new PushPullAdapter<TSource, TResult>(push, pull, subscription.Dispose);
            subscription.Disposable = source.SubscribeSafe(adapter);
            return adapter;
        }
#endif

        #endregion
    }
}

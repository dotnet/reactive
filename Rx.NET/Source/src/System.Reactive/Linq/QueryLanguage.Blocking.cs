// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the Apache 2.0 License.
// See the LICENSE file in the project root for more information. 

using System.Collections.Generic;
using System.Threading;

namespace System.Reactive.Linq
{
    using ObservableImpl;

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
            return new Collect<TSource, TResult>(source, getInitialCollector, merge, getNewCollector);
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

            using (var evt = new WaitAndSetOnce())
            {
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
            using (var evt = new WaitAndSetOnce())
            {
                var sink = new ForEach<TSource>.Observer(onNext, () => evt.Set());

                using (source.SubscribeSafe(sink))
                {
                    evt.WaitOne();
                }

                sink.Error.ThrowIfNotNull();
            }
        }

        public virtual void ForEach<TSource>(IObservable<TSource> source, Action<TSource, int> onNext)
        {
            using (var evt = new WaitAndSetOnce())
            {
                var sink = new ForEach<TSource>.ObserverIndexed(onNext, () => evt.Set());

                using (source.SubscribeSafe(sink))
                {
                    evt.WaitOne();
                }

                sink.Error.ThrowIfNotNull();
            }
        }

        #endregion

        #region + GetEnumerator +

        public virtual IEnumerator<TSource> GetEnumerator<TSource>(IObservable<TSource> source)
        {
            var e = new GetEnumerator<TSource>();
            return e.Run(source);
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

            using (var evt = new WaitAndSetOnce())
            {
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

            using (var evt = new WaitAndSetOnce())
            {
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

        class WaitAndSetOnce : IDisposable
        {
            private readonly ManualResetEvent _evt;
            private int _hasSet;

            public WaitAndSetOnce()
            {
                _evt = new ManualResetEvent(false);
            }

            public void Set()
            {
                if (Interlocked.Exchange(ref _hasSet, 1) == 0)
                {
                    _evt.Set();
                }
            }

            public void WaitOne()
            {
                _evt.WaitOne();
            }

            public void Dispose()
            {
#if HAS_MREEXPLICITDISPOSABLE
                ((IDisposable)_evt).Dispose();
#else
                _evt.Dispose();
#endif
            }
        }

#endregion
    }
}

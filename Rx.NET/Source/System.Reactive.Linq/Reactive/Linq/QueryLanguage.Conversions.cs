// Copyright (c) Microsoft Open Technologies, Inc. All rights reserved. See License.txt in the project root for license information.

using System.Collections.Generic;
using System.Reactive.Concurrency;
using System.Reactive.Disposables;

namespace System.Reactive.Linq
{
#if !NO_PERF
    using Observαble;
#endif

    internal partial class QueryLanguage
    {
        #region + Subscribe +

        public virtual IDisposable Subscribe<TSource>(IEnumerable<TSource> source, IObserver<TSource> observer)
        {
            return Subscribe_<TSource>(source, observer, SchedulerDefaults.Iteration);
        }

        public virtual IDisposable Subscribe<TSource>(IEnumerable<TSource> source, IObserver<TSource> observer, IScheduler scheduler)
        {
            return Subscribe_<TSource>(source, observer, scheduler);
        }

        private static IDisposable Subscribe_<TSource>(IEnumerable<TSource> source, IObserver<TSource> observer, IScheduler scheduler)
        {
#if !NO_PERF
            //
            // [OK] Use of unsafe Subscribe: we're calling into a known producer implementation.
            //
            return new ToObservable<TSource>(source, scheduler).Subscribe/*Unsafe*/(observer);
#else
            var e = source.GetEnumerator();
            var flag = new BooleanDisposable();

            scheduler.Schedule(self =>
            {
                var hasNext = false;
                var ex = default(Exception);
                var current = default(TSource);

                if (flag.IsDisposed)
                {
                    e.Dispose();
                    return;
                }

                try
                {
                    hasNext = e.MoveNext();
                    if (hasNext)
                        current = e.Current;
                }
                catch (Exception exception)
                {
                    ex = exception;
                }

                if (!hasNext || ex != null)
                {
                    e.Dispose();
                }

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

                observer.OnNext(current);
                self();
            });

            return flag;
#endif
        }

        #endregion

        #region + ToEnumerable +

        public virtual IEnumerable<TSource> ToEnumerable<TSource>(IObservable<TSource> source)
        {
            return new AnonymousEnumerable<TSource>(() => source.GetEnumerator());
        }

        #endregion

        #region ToEvent

        public virtual IEventSource<Unit> ToEvent(IObservable<Unit> source)
        {
            return new EventSource<Unit>(source, (h, _) => h(Unit.Default));
        }

        public virtual IEventSource<TSource> ToEvent<TSource>(IObservable<TSource> source)
        {
            return new EventSource<TSource>(source, (h, value) => h(value));
        }

        #endregion

        #region ToEventPattern

        public virtual IEventPatternSource<TEventArgs> ToEventPattern<TEventArgs>(IObservable<EventPattern<TEventArgs>> source)
#if !NO_EVENTARGS_CONSTRAINT
            where TEventArgs : EventArgs
#endif
        {
            return new EventPatternSource<TEventArgs>(
#if !NO_VARIANCE
                source,
#else
                source.Select(x => (EventPattern<object, TEventArgs>)x),
#endif
                (h, evt) => h(evt.Sender, evt.EventArgs)
            );
        }

        #endregion

        #region + ToObservable +

        public virtual IObservable<TSource> ToObservable<TSource>(IEnumerable<TSource> source)
        {
#if !NO_PERF
            return new ToObservable<TSource>(source, SchedulerDefaults.Iteration);
#else
            return ToObservable_(source, SchedulerDefaults.Iteration);
#endif
        }

        public virtual IObservable<TSource> ToObservable<TSource>(IEnumerable<TSource> source, IScheduler scheduler)
        {
#if !NO_PERF
            return new ToObservable<TSource>(source, scheduler);
#else
            return ToObservable_(source, scheduler);
#endif
        }

#if NO_PERF
        private static IObservable<TSource> ToObservable_<TSource>(IEnumerable<TSource> source, IScheduler scheduler)
        {
            return new AnonymousObservable<TSource>(observer => source.Subscribe(observer, scheduler));
        }
#endif

        #endregion
    }
}

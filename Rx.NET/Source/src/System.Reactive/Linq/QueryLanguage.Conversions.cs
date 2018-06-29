// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the Apache 2.0 License.
// See the LICENSE file in the project root for more information. 

using System.Collections.Generic;
using System.Reactive.Concurrency;

namespace System.Reactive.Linq
{
    using ObservableImpl;

    internal partial class QueryLanguage
    {
        #region + Subscribe +

        public virtual IDisposable Subscribe<TSource>(IEnumerable<TSource> source, IObserver<TSource> observer)
        {
            return Subscribe_(source, observer, SchedulerDefaults.Iteration);
        }

        public virtual IDisposable Subscribe<TSource>(IEnumerable<TSource> source, IObserver<TSource> observer, IScheduler scheduler)
        {
            return Subscribe_(source, observer, scheduler);
        }

        private static IDisposable Subscribe_<TSource>(IEnumerable<TSource> source, IObserver<TSource> observer, IScheduler scheduler)
        {
            var longRunning = scheduler.AsLongRunning();
            if (longRunning != null)
            {
                //
                // [OK] Use of unsafe Subscribe: we're calling into a known producer implementation.
                //
                return new ToObservableLongRunning<TSource>(source, longRunning).Subscribe/*Unsafe*/(observer);
            }
            //
            // [OK] Use of unsafe Subscribe: we're calling into a known producer implementation.
            //
            return new ToObservableRecursive<TSource>(source, scheduler).Subscribe/*Unsafe*/(observer);
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
        {
            return new EventPatternSource<TEventArgs>(
                source,
                (h, evt) => h(evt.Sender, evt.EventArgs)
            );
        }

        #endregion

        #region + ToObservable +

        public virtual IObservable<TSource> ToObservable<TSource>(IEnumerable<TSource> source)
        {
            return ToObservable_(source, SchedulerDefaults.Iteration);
        }

        public virtual IObservable<TSource> ToObservable<TSource>(IEnumerable<TSource> source, IScheduler scheduler)
        {
            return ToObservable_(source, scheduler);
        }

        private static IObservable<TSource> ToObservable_<TSource>(IEnumerable<TSource> source, IScheduler scheduler)
        {
            var longRunning = scheduler.AsLongRunning();
            if (longRunning != null)
            {
                //
                // [OK] Use of unsafe Subscribe: we're calling into a known producer implementation.
                //
                return new ToObservableLongRunning<TSource>(source, longRunning);
            }
            //
            // [OK] Use of unsafe Subscribe: we're calling into a known producer implementation.
            //
            return new ToObservableRecursive<TSource>(source, scheduler);
        }

        #endregion
    }
}

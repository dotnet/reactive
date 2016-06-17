// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the Apache 2.0 License.
// See the LICENSE file in the project root for more information. 

using System.Reactive.Concurrency;
using System.Reactive.Disposables;
using System.Threading;

namespace System.Reactive.Linq
{
#if !NO_PERF
    using ObservableImpl;
#endif

    internal partial class QueryLanguage
    {
        #region + ObserveOn +

        public virtual IObservable<TSource> ObserveOn<TSource>(IObservable<TSource> source, IScheduler scheduler)
        {
            return Synchronization.ObserveOn<TSource>(source, scheduler);
        }

#if !NO_SYNCCTX
        public virtual IObservable<TSource> ObserveOn<TSource>(IObservable<TSource> source, SynchronizationContext context)
        {
            return Synchronization.ObserveOn<TSource>(source, context);
        }
#endif

        #endregion

        #region + SubscribeOn +

        public virtual IObservable<TSource> SubscribeOn<TSource>(IObservable<TSource> source, IScheduler scheduler)
        {
            return Synchronization.SubscribeOn<TSource>(source, scheduler);
        }

#if !NO_SYNCCTX
        public virtual IObservable<TSource> SubscribeOn<TSource>(IObservable<TSource> source, SynchronizationContext context)
        {
            return Synchronization.SubscribeOn<TSource>(source, context);
        }
#endif

        #endregion

        #region + Synchronize +

        public virtual IObservable<TSource> Synchronize<TSource>(IObservable<TSource> source)
        {
            return Synchronization.Synchronize(source);
        }

        public virtual IObservable<TSource> Synchronize<TSource>(IObservable<TSource> source, object gate)
        {
            return Synchronization.Synchronize(source, gate);
        }

        #endregion
    }
}

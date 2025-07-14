﻿// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information. 

using System.Reactive.Concurrency;
using System.Threading;

namespace System.Reactive.Linq
{
    internal partial class QueryLanguage
    {
        #region + ObserveOn +

        public virtual IObservable<TSource> ObserveOn<TSource>(IObservable<TSource> source, IScheduler scheduler)
        {
            return Synchronization.ObserveOn(source, scheduler);
        }

        public virtual IObservable<TSource> ObserveOn<TSource>(IObservable<TSource> source, SynchronizationContext context)
        {
            return Synchronization.ObserveOn(source, context);
        }

        #endregion

        #region + SubscribeOn +

        public virtual IObservable<TSource> SubscribeOn<TSource>(IObservable<TSource> source, IScheduler scheduler)
        {
            return Synchronization.SubscribeOn(source, scheduler);
        }

        public virtual IObservable<TSource> SubscribeOn<TSource>(IObservable<TSource> source, SynchronizationContext context)
        {
            return Synchronization.SubscribeOn(source, context);
        }

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

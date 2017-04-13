// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the Apache 2.0 License.
// See the LICENSE file in the project root for more information. 

using System.Reactive.Concurrency;
using System.Threading;

namespace System.Reactive.Linq
{
    public static partial class Observable
    {
        #region + ObserveOn +

        /// <summary>
        /// Wraps the source sequence in order to run its observer callbacks on the specified scheduler.
        /// </summary>
        /// <typeparam name="TSource">The type of the elements in the source sequence.</typeparam>
        /// <param name="source">Source sequence.</param>
        /// <param name="scheduler">Scheduler to notify observers on.</param>
        /// <returns>The source sequence whose observations happen on the specified scheduler.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="source"/> or <paramref name="scheduler"/> is null.</exception>
        /// <remarks>
        /// This only invokes observer callbacks on a scheduler. In case the subscription and/or unsubscription actions have side-effects
        /// that require to be run on a scheduler, use <see cref="Observable.SubscribeOn{TSource}(IObservable{TSource}, IScheduler)"/>.
        /// </remarks>
        public static IObservable<TSource> ObserveOn<TSource>(this IObservable<TSource> source, IScheduler scheduler)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));
            if (scheduler == null)
                throw new ArgumentNullException(nameof(scheduler));

            return s_impl.ObserveOn<TSource>(source, scheduler);
        }

        /// <summary>
        /// Wraps the source sequence in order to run its observer callbacks on the specified synchronization context.
        /// </summary>
        /// <typeparam name="TSource">The type of the elements in the source sequence.</typeparam>
        /// <param name="source">Source sequence.</param>
        /// <param name="context">Synchronization context to notify observers on.</param>
        /// <returns>The source sequence whose observations happen on the specified synchronization context.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="source"/> or <paramref name="context"/> is null.</exception>
        /// <remarks>
        /// This only invokes observer callbacks on a synchronization context. In case the subscription and/or unsubscription actions have side-effects
        /// that require to be run on a synchronization context, use <see cref="Observable.SubscribeOn{TSource}(IObservable{TSource}, SynchronizationContext)"/>.
        /// </remarks>
        public static IObservable<TSource> ObserveOn<TSource>(this IObservable<TSource> source, SynchronizationContext context)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));
            if (context == null)
                throw new ArgumentNullException(nameof(context));

            return s_impl.ObserveOn<TSource>(source, context);
        }

        #endregion

        #region + SubscribeOn +

        /// <summary>
        /// Wraps the source sequence in order to run its subscription and unsubscription logic on the specified scheduler. This operation is not commonly used;
        /// see the remarks section for more information on the distinction between SubscribeOn and ObserveOn.
        /// </summary>
        /// <typeparam name="TSource">The type of the elements in the source sequence.</typeparam>
        /// <param name="source">Source sequence.</param>
        /// <param name="scheduler">Scheduler to perform subscription and unsubscription actions on.</param>
        /// <returns>The source sequence whose subscriptions and unsubscriptions happen on the specified scheduler.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="source"/> or <paramref name="scheduler"/> is null.</exception>
        /// <remarks>
        /// This only performs the side-effects of subscription and unsubscription on the specified scheduler. In order to invoke observer
        /// callbacks on a scheduler, use <see cref="Observable.ObserveOn{TSource}(IObservable{TSource}, IScheduler)"/>.
        /// </remarks>
        public static IObservable<TSource> SubscribeOn<TSource>(this IObservable<TSource> source, IScheduler scheduler)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));
            if (scheduler == null)
                throw new ArgumentNullException(nameof(scheduler));

            return s_impl.SubscribeOn<TSource>(source, scheduler);
        }

        /// <summary>
        /// Wraps the source sequence in order to run its subscription and unsubscription logic on the specified synchronization context. This operation is not commonly used;
        /// see the remarks section for more information on the distinction between SubscribeOn and ObserveOn.
        /// </summary>
        /// <typeparam name="TSource">The type of the elements in the source sequence.</typeparam>
        /// <param name="source">Source sequence.</param>
        /// <param name="context">Synchronization context to perform subscription and unsubscription actions on.</param>
        /// <returns>The source sequence whose subscriptions and unsubscriptions happen on the specified synchronization context.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="source"/> or <paramref name="context"/> is null.</exception>
        /// <remarks>
        /// This only performs the side-effects of subscription and unsubscription on the specified synchronization context. In order to invoke observer
        /// callbacks on a synchronization context, use <see cref="Observable.ObserveOn{TSource}(IObservable{TSource}, SynchronizationContext)"/>.
        /// </remarks>
        public static IObservable<TSource> SubscribeOn<TSource>(this IObservable<TSource> source, SynchronizationContext context)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));
            if (context == null)
                throw new ArgumentNullException(nameof(context));

            return s_impl.SubscribeOn<TSource>(source, context);
        }

        #endregion

        #region + Synchronize +

        /// <summary>
        /// Synchronizes the observable sequence such that observer notifications cannot be delivered concurrently.
        /// This overload is useful to "fix" an observable sequence that exhibits concurrent callbacks on individual observers, which is invalid behavior for the query processor.
        /// </summary>
        /// <typeparam name="TSource">The type of the elements in the source sequence.</typeparam>
        /// <param name="source">Source sequence.</param>
        /// <returns>The source sequence whose outgoing calls to observers are synchronized.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="source"/> is null.</exception>
        /// <remarks>
        /// It's invalid behavior - according to the observer grammar - for a sequence to exhibit concurrent callbacks on a given observer.
        /// This operator can be used to "fix" a source that doesn't conform to this rule.
        /// </remarks>
        public static IObservable<TSource> Synchronize<TSource>(this IObservable<TSource> source)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));

            return s_impl.Synchronize<TSource>(source);
        }

        /// <summary>
        /// Synchronizes the observable sequence such that observer notifications cannot be delivered concurrently, using the specified gate object.
        /// This overload is useful when writing n-ary query operators, in order to prevent concurrent callbacks from different sources by synchronizing on a common gate object.
        /// </summary>
        /// <typeparam name="TSource">The type of the elements in the source sequence.</typeparam>
        /// <param name="source">Source sequence.</param>
        /// <param name="gate">Gate object to synchronize each observer call on.</param>
        /// <returns>The source sequence whose outgoing calls to observers are synchronized on the given gate object.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="source"/> or <paramref name="gate"/> is null.</exception>
        public static IObservable<TSource> Synchronize<TSource>(this IObservable<TSource> source, object gate)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));
            if (gate == null)
                throw new ArgumentNullException(nameof(gate));

            return s_impl.Synchronize<TSource>(source, gate);
        }

        #endregion
    }
}

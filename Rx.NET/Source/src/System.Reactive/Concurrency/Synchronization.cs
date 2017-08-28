// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the Apache 2.0 License.
// See the LICENSE file in the project root for more information. 

using System.ComponentModel;
using System.Reactive.Disposables;
using System.Threading;

namespace System.Reactive.Concurrency
{
    /// <summary>
    /// Provides basic synchronization and scheduling services for observable sequences.
    /// </summary>
    [EditorBrowsable(EditorBrowsableState.Advanced)]
    public static class Synchronization
    {
        #region SubscribeOn

        /// <summary>
        /// Wraps the source sequence in order to run its subscription and unsubscription logic on the specified scheduler.
        /// </summary>
        /// <typeparam name="TSource">The type of the elements in the source sequence.</typeparam>
        /// <param name="source">Source sequence.</param>
        /// <param name="scheduler">Scheduler to perform subscription and unsubscription actions on.</param>
        /// <returns>The source sequence whose subscriptions and unsubscriptions happen on the specified scheduler.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="source"/> or <paramref name="scheduler"/> is <c>null</c>.</exception>
        /// <remarks>
        /// Only the side-effects of subscribing to the source sequence and disposing subscriptions to the source sequence are run on the specified scheduler.
        /// In order to invoke observer callbacks on the specified scheduler, e.g. to offload callback processing to a dedicated thread, use <see cref="Synchronization.ObserveOn{TSource}(IObservable{TSource}, IScheduler)"/>.
        /// </remarks>
        public static IObservable<TSource> SubscribeOn<TSource>(IObservable<TSource> source, IScheduler scheduler)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));
            if (scheduler == null)
                throw new ArgumentNullException(nameof(scheduler));

            return new AnonymousObservable<TSource>(observer =>
            {
                var m = new SingleAssignmentDisposable();
                var d = new SerialDisposable();
                d.Disposable = m;

                m.Disposable = scheduler.Schedule(() =>
                {
                    d.Disposable = new ScheduledDisposable(scheduler, source.SubscribeSafe(observer));
                });

                return d;
            });
        }

        /// <summary>
        /// Wraps the source sequence in order to run its subscription and unsubscription logic on the specified synchronization context.
        /// </summary>
        /// <typeparam name="TSource">The type of the elements in the source sequence.</typeparam>
        /// <param name="source">Source sequence.</param>
        /// <param name="context">Synchronization context to perform subscription and unsubscription actions on.</param>
        /// <returns>The source sequence whose subscriptions and unsubscriptions happen on the specified synchronization context.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="source"/> or <paramref name="context"/> is <c>null</c>.</exception>
        /// <remarks>
        /// Only the side-effects of subscribing to the source sequence and disposing subscriptions to the source sequence are run on the specified synchronization context.
        /// In order to invoke observer callbacks on the specified synchronization context, e.g. to post callbacks to a UI thread represented by the synchronization context, use <see cref="Synchronization.ObserveOn{TSource}(IObservable{TSource}, SynchronizationContext)"/>.
        /// </remarks>
        public static IObservable<TSource> SubscribeOn<TSource>(IObservable<TSource> source, SynchronizationContext context)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));
            if (context == null)
                throw new ArgumentNullException(nameof(context));

            return new AnonymousObservable<TSource>(observer =>
            {
                var subscription = new SingleAssignmentDisposable();
                context.PostWithStartComplete(() =>
                {
                    if (!subscription.IsDisposed)
                    {
                        subscription.Disposable = new ContextDisposable(context, source.SubscribeSafe(observer));
                    }
                });
                return subscription;
            });
        }

        #endregion

        #region ObserveOn

        /// <summary>
        /// Wraps the source sequence in order to run its observer callbacks on the specified scheduler.
        /// </summary>
        /// <typeparam name="TSource">The type of the elements in the source sequence.</typeparam>
        /// <param name="source">Source sequence.</param>
        /// <param name="scheduler">Scheduler to notify observers on.</param>
        /// <returns>The source sequence whose observations happen on the specified scheduler.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="source"/> or <paramref name="scheduler"/> is <c>null</c>.</exception>
        public static IObservable<TSource> ObserveOn<TSource>(IObservable<TSource> source, IScheduler scheduler)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));
            if (scheduler == null)
                throw new ArgumentNullException(nameof(scheduler));

            return new ObserveOn<TSource>.Scheduler(source, scheduler);
        }

        /// <summary>
        /// Wraps the source sequence in order to run its observer callbacks on the specified synchronization context.
        /// </summary>
        /// <typeparam name="TSource">The type of the elements in the source sequence.</typeparam>
        /// <param name="source">Source sequence.</param>
        /// <param name="context">Synchronization context to notify observers on.</param>
        /// <returns>The source sequence whose observations happen on the specified synchronization context.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="source"/> or <paramref name="context"/> is <c>null</c>.</exception>
        public static IObservable<TSource> ObserveOn<TSource>(IObservable<TSource> source, SynchronizationContext context)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));
            if (context == null)
                throw new ArgumentNullException(nameof(context));

            return new ObserveOn<TSource>.Context(source, context);
        }

        #endregion

        #region Synchronize

        /// <summary>
        /// Wraps the source sequence in order to ensure observer callbacks are properly serialized.
        /// </summary>
        /// <typeparam name="TSource">The type of the elements in the source sequence.</typeparam>
        /// <param name="source">Source sequence.</param>
        /// <returns>The source sequence whose outgoing calls to observers are synchronized.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="source"/> is <c>null</c>.</exception>
        public static IObservable<TSource> Synchronize<TSource>(IObservable<TSource> source)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));

            return new Synchronize<TSource>(source);
        }

        /// <summary>
        /// Wraps the source sequence in order to ensure observer callbacks are synchronized using the specified gate object.
        /// </summary>
        /// <typeparam name="TSource">The type of the elements in the source sequence.</typeparam>
        /// <param name="source">Source sequence.</param>
        /// <param name="gate">Gate object to synchronize each observer call on.</param>
        /// <returns>The source sequence whose outgoing calls to observers are synchronized on the given gate object.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="source"/> or <paramref name="gate"/> is <c>null</c>.</exception>
        public static IObservable<TSource> Synchronize<TSource>(IObservable<TSource> source, object gate)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));
            if (gate == null)
                throw new ArgumentNullException(nameof(gate));

            return new Synchronize<TSource>(source, gate);
        }

        #endregion
    }
}

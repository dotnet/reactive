// Copyright (c) Microsoft Open Technologies, Inc. All rights reserved. See License.txt in the project root for license information.

using System;
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
        /// <exception cref="ArgumentNullException"><paramref name="source"/> or <paramref name="scheduler"/> is null.</exception>
        /// <remarks>
        /// Only the side-effects of subscribing to the source sequence and disposing subscriptions to the source sequence are run on the specified scheduler.
        /// In order to invoke observer callbacks on the specified scheduler, e.g. to offload callback processing to a dedicated thread, use <see cref="Synchronization.ObserveOn{TSource}(IObservable{TSource}, IScheduler)"/>.
        /// </remarks>
        public static IObservable<TSource> SubscribeOn<TSource>(IObservable<TSource> source, IScheduler scheduler)
        {
            if (source == null)
                throw new ArgumentNullException("source");
            if (scheduler == null)
                throw new ArgumentNullException("scheduler");

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

#if !NO_SYNCCTX
        /// <summary>
        /// Wraps the source sequence in order to run its subscription and unsubscription logic on the specified synchronization context.
        /// </summary>
        /// <typeparam name="TSource">The type of the elements in the source sequence.</typeparam>
        /// <param name="source">Source sequence.</param>
        /// <param name="context">Synchronization context to perform subscription and unsubscription actions on.</param>
        /// <returns>The source sequence whose subscriptions and unsubscriptions happen on the specified synchronization context.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="source"/> or <paramref name="context"/> is null.</exception>
        /// <remarks>
        /// Only the side-effects of subscribing to the source sequence and disposing subscriptions to the source sequence are run on the specified synchronization context.
        /// In order to invoke observer callbacks on the specified synchronization context, e.g. to post callbacks to a UI thread represented by the synchronization context, use <see cref="Synchronization.ObserveOn{TSource}(IObservable{TSource}, SynchronizationContext)"/>.
        /// </remarks>
        public static IObservable<TSource> SubscribeOn<TSource>(IObservable<TSource> source, SynchronizationContext context)
        {
            if (source == null)
                throw new ArgumentNullException("source");
            if (context == null)
                throw new ArgumentNullException("context");

            return new AnonymousObservable<TSource>(observer =>
            {
                var subscription = new SingleAssignmentDisposable();
                context.PostWithStartComplete(() =>
                {
                    if (!subscription.IsDisposed)
                        subscription.Disposable = new ContextDisposable(context, source.SubscribeSafe(observer));
                });
                return subscription;
            });
        }
#endif

        #endregion

        #region ObserveOn

        /// <summary>
        /// Wraps the source sequence in order to run its observer callbacks on the specified scheduler.
        /// </summary>
        /// <typeparam name="TSource">The type of the elements in the source sequence.</typeparam>
        /// <param name="source">Source sequence.</param>
        /// <param name="scheduler">Scheduler to notify observers on.</param>
        /// <returns>The source sequence whose observations happen on the specified scheduler.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="source"/> or <paramref name="scheduler"/> is null.</exception>
        public static IObservable<TSource> ObserveOn<TSource>(IObservable<TSource> source, IScheduler scheduler)
        {
            if (source == null)
                throw new ArgumentNullException("source");
            if (scheduler == null)
                throw new ArgumentNullException("scheduler");

#if !NO_PERF
            return new ObserveOn<TSource>(source, scheduler);
#else
            return new AnonymousObservable<TSource>(observer => source.Subscribe(new ObserveOnObserver<TSource>(scheduler, observer, null)));
#endif
        }

#if !NO_SYNCCTX
        /// <summary>
        /// Wraps the source sequence in order to run its observer callbacks on the specified synchronization context.
        /// </summary>
        /// <typeparam name="TSource">The type of the elements in the source sequence.</typeparam>
        /// <param name="source">Source sequence.</param>
        /// <param name="context">Synchronization context to notify observers on.</param>
        /// <returns>The source sequence whose observations happen on the specified synchronization context.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="source"/> or <paramref name="context"/> is null.</exception>
        public static IObservable<TSource> ObserveOn<TSource>(IObservable<TSource> source, SynchronizationContext context)
        {
            if (source == null)
                throw new ArgumentNullException("source");
            if (context == null)
                throw new ArgumentNullException("context");

#if !NO_PERF
            return new ObserveOn<TSource>(source, context);
#else
            return new AnonymousObservable<TSource>(observer =>
            {
                context.OperationStarted();

                return source.Subscribe(
                    x => context.Post(_ =>
                    {
                        observer.OnNext(x);
                    }, null),
                    exception => context.Post(_ =>
                    {
                        observer.OnError(exception);
                    }, null),
                    () => context.Post(_ =>
                    {
                        observer.OnCompleted();
                    }, null)
                ).Finally(() =>
                {
                    context.OperationCompleted();
                });
            });
#endif
        }
#endif

        #endregion

        #region Synchronize

        /// <summary>
        /// Wraps the source sequence in order to ensure observer callbacks are properly serialized.
        /// </summary>
        /// <typeparam name="TSource">The type of the elements in the source sequence.</typeparam>
        /// <param name="source">Source sequence.</param>
        /// <returns>The source sequence whose outgoing calls to observers are synchronized.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="source"/> is null.</exception>
        public static IObservable<TSource> Synchronize<TSource>(IObservable<TSource> source)
        {
            if (source == null)
                throw new ArgumentNullException("source");

#if !NO_PERF
            return new Synchronize<TSource>(source);
#else
            return new AnonymousObservable<TSource>(observer =>
            {
                var gate = new object();
                return source.Subscribe(Observer.Synchronize(observer, gate));
            });
#endif
        }

        /// <summary>
        /// Wraps the source sequence in order to ensure observer callbacks are synchronized using the specified gate object.
        /// </summary>
        /// <typeparam name="TSource">The type of the elements in the source sequence.</typeparam>
        /// <param name="source">Source sequence.</param>
        /// <param name="gate">Gate object to synchronize each observer call on.</param>
        /// <returns>The source sequence whose outgoing calls to observers are synchronized on the given gate object.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="source"/> or <paramref name="gate"/> is null.</exception>
        public static IObservable<TSource> Synchronize<TSource>(IObservable<TSource> source, object gate)
        {
            if (source == null)
                throw new ArgumentNullException("source");
            if (gate == null)
                throw new ArgumentNullException("gate");

#if !NO_PERF
            return new Synchronize<TSource>(source, gate);
#else
            return new AnonymousObservable<TSource>(observer =>
            {
                return source.Subscribe(Observer.Synchronize(observer, gate));
            });
#endif
        }

        #endregion
    }
}

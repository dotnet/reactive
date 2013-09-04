// Copyright (c) Microsoft Open Technologies, Inc. All rights reserved. See License.txt in the project root for license information.

#if !WINDOWS
using System.Reactive.Concurrency;
using System.Windows;
using System.Windows.Threading;

namespace System.Reactive.Linq
{
    /// <summary>
    /// Provides a set of extension methods for scheduling actions performed through observable sequences on UI dispatchers.
    /// </summary>
    public static class DispatcherObservable
    {
        #region ObserveOn[Dispatcher]

        /// <summary>
        /// Wraps the source sequence in order to run its observer callbacks on the specified dispatcher.
        /// </summary>
        /// <typeparam name="TSource">The type of the elements in the source sequence.</typeparam>
        /// <param name="source">Source sequence.</param>
        /// <param name="dispatcher">Dispatcher whose associated message loop is used to to notify observers on.</param>
        /// <returns>The source sequence whose observations happen on the specified dispatcher.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="source"/> or <paramref name="dispatcher"/> is null.</exception>
        public static IObservable<TSource> ObserveOn<TSource>(this IObservable<TSource> source, Dispatcher dispatcher)
        {
            if (source == null)
                throw new ArgumentNullException("source");
            if (dispatcher == null)
                throw new ArgumentNullException("dispatcher");

            return ObserveOn_<TSource>(source, dispatcher);
        }

#if HAS_DISPATCHER_PRIORITY
        /// <summary>
        /// Wraps the source sequence in order to run its observer callbacks on the specified dispatcher.
        /// </summary>
        /// <typeparam name="TSource">The type of the elements in the source sequence.</typeparam>
        /// <param name="source">Source sequence.</param>
        /// <param name="dispatcher">Dispatcher whose associated message loop is used to to notify observers on.</param>
        /// <param name="priority">Priority to schedule work items at.</param>
        /// <returns>The source sequence whose observations happen on the specified dispatcher.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="source"/> or <paramref name="dispatcher"/> is null.</exception>
        public static IObservable<TSource> ObserveOn<TSource>(this IObservable<TSource> source, Dispatcher dispatcher, DispatcherPriority priority)
        {
            if (source == null)
                throw new ArgumentNullException("source");
            if (dispatcher == null)
                throw new ArgumentNullException("dispatcher");

            return ObserveOn_<TSource>(source, dispatcher, priority);
        }
#endif

        /// <summary>
        /// Wraps the source sequence in order to run its observer callbacks on the specified dispatcher scheduler.
        /// </summary>
        /// <typeparam name="TSource">The type of the elements in the source sequence.</typeparam>
        /// <param name="source">Source sequence.</param>
        /// <param name="scheduler">Dispatcher scheduler to notify observers on.</param>
        /// <returns>The source sequence whose observations happen on the specified dispatcher scheduler.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="source"/> or <paramref name="scheduler"/> is null.</exception>
        public static IObservable<TSource> ObserveOn<TSource>(this IObservable<TSource> source, DispatcherScheduler scheduler)
        {
            if (source == null)
                throw new ArgumentNullException("source");
            if (scheduler == null)
                throw new ArgumentNullException("scheduler");

#if HAS_DISPATCHER_PRIORITY
            return ObserveOn_<TSource>(source, scheduler.Dispatcher, scheduler.Priority);
#else
            return ObserveOn_<TSource>(source, scheduler.Dispatcher);
#endif
        }

#if USE_SL_DISPATCHER
        /// <summary>
        /// Wraps the source sequence in order to run its observer callbacks on the dispatcher associated with the specified object.
        /// </summary>
        /// <typeparam name="TSource">The type of the elements in the source sequence.</typeparam>
        /// <param name="source">Source sequence.</param>
        /// <param name="dependencyObject">Object to get the dispatcher from.</param>
        /// <returns>The source sequence whose observations happen on the specified object's dispatcher.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="source"/> or <paramref name="dependencyObject"/> is null.</exception>
        public static IObservable<TSource> ObserveOn<TSource>(this IObservable<TSource> source, DependencyObject dependencyObject)
        {
            if (source == null)
                throw new ArgumentNullException("source");
            if (dependencyObject == null)
                throw new ArgumentNullException("dependencyObject");

            return ObserveOn_<TSource>(source, dependencyObject.Dispatcher);
        }
#else
        /// <summary>
        /// Wraps the source sequence in order to run its observer callbacks on the dispatcher associated with the specified object.
        /// </summary>
        /// <typeparam name="TSource">The type of the elements in the source sequence.</typeparam>
        /// <param name="source">Source sequence.</param>
        /// <param name="dispatcherObject">Object to get the dispatcher from.</param>
        /// <returns>The source sequence whose observations happen on the specified object's dispatcher.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="source"/> or <paramref name="dispatcherObject"/> is null.</exception>
        public static IObservable<TSource> ObserveOn<TSource>(this IObservable<TSource> source, DispatcherObject dispatcherObject)
        {
            if (source == null)
                throw new ArgumentNullException("source");
            if (dispatcherObject == null)
                throw new ArgumentNullException("dispatcherObject");

            return ObserveOn_<TSource>(source, dispatcherObject.Dispatcher);
        }
#endif

#if HAS_DISPATCHER_PRIORITY
        /// <summary>
        /// Wraps the source sequence in order to run its observer callbacks on the dispatcher associated with the specified object.
        /// </summary>
        /// <typeparam name="TSource">The type of the elements in the source sequence.</typeparam>
        /// <param name="source">Source sequence.</param>
        /// <param name="dispatcherObject">Object to get the dispatcher from.</param>
        /// <param name="priority">Priority to schedule work items at.</param>
        /// <returns>The source sequence whose observations happen on the specified object's dispatcher.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="source"/> or <paramref name="dispatcherObject"/> is null.</exception>
        public static IObservable<TSource> ObserveOn<TSource>(this IObservable<TSource> source, DispatcherObject dispatcherObject, DispatcherPriority priority)
        {
            if (source == null)
                throw new ArgumentNullException("source");
            if (dispatcherObject == null)
                throw new ArgumentNullException("dispatcherObject");

            return ObserveOn_<TSource>(source, dispatcherObject.Dispatcher, priority);
        }
#endif

        /// <summary>
        /// Wraps the source sequence in order to run its observer callbacks on the dispatcher associated with the current thread.
        /// </summary>
        /// <typeparam name="TSource">The type of the elements in the source sequence.</typeparam>
        /// <param name="source">Source sequence.</param>
        /// <returns>The source sequence whose observations happen on the current thread's dispatcher.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="source"/> is null.</exception>
        public static IObservable<TSource> ObserveOnDispatcher<TSource>(this IObservable<TSource> source)
        {
            if (source == null)
                throw new ArgumentNullException("source");

#if USE_SL_DISPATCHER
            return ObserveOn_<TSource>(source, System.Windows.Deployment.Current.Dispatcher);
#else
            return ObserveOn_<TSource>(source, DispatcherScheduler.Current.Dispatcher);
#endif
        }

#if HAS_DISPATCHER_PRIORITY
        /// <summary>
        /// Wraps the source sequence in order to run its observer callbacks on the dispatcher associated with the current thread.
        /// </summary>
        /// <typeparam name="TSource">The type of the elements in the source sequence.</typeparam>
        /// <param name="source">Source sequence.</param>
        /// <param name="priority">Priority to schedule work items at.</param>
        /// <returns>The source sequence whose observations happen on the current thread's dispatcher.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="source"/> is null.</exception>
        public static IObservable<TSource> ObserveOnDispatcher<TSource>(this IObservable<TSource> source, DispatcherPriority priority)
        {
            if (source == null)
                throw new ArgumentNullException("source");

            return ObserveOn_<TSource>(source, DispatcherScheduler.Current.Dispatcher, priority);
        }

        private static IObservable<TSource> ObserveOn_<TSource>(IObservable<TSource> source, Dispatcher dispatcher, DispatcherPriority priority)
        {
            return Synchronization.ObserveOn(source, new DispatcherSynchronizationContext(dispatcher, priority));
        }
#endif

        private static IObservable<TSource> ObserveOn_<TSource>(IObservable<TSource> source, Dispatcher dispatcher)
        {
            return Synchronization.ObserveOn(source, new DispatcherSynchronizationContext(dispatcher));
        }

        #endregion

        #region SubscribeOn[Dispatcher]

        /// <summary>
        /// Wraps the source sequence in order to run its subscription and unsubscription logic on the specified dispatcher.
        /// </summary>
        /// <typeparam name="TSource">The type of the elements in the source sequence.</typeparam>
        /// <param name="source">Source sequence.</param>
        /// <param name="dispatcher">Dispatcher whose associated message loop is used to to perform subscription and unsubscription actions on.</param>
        /// <returns>The source sequence whose subscriptions and unsubscriptions happen on the specified dispatcher.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="source"/> or <paramref name="dispatcher"/> is null.</exception>
        /// <remarks>
        /// Only the side-effects of subscribing to the source sequence and disposing subscriptions to the source sequence are run on the specified dispatcher.
        /// In order to invoke observer callbacks on the specified dispatcher, e.g. to render results in a control, use <see cref="DispatcherObservable.ObserveOn{TSource}(IObservable{TSource}, Dispatcher)"/>.
        /// </remarks>
        public static IObservable<TSource> SubscribeOn<TSource>(this IObservable<TSource> source, Dispatcher dispatcher)
        {
            if (source == null)
                throw new ArgumentNullException("source");
            if (dispatcher == null)
                throw new ArgumentNullException("dispatcher");

            return SubscribeOn_<TSource>(source, dispatcher);
        }

#if HAS_DISPATCHER_PRIORITY
        /// <summary>
        /// Wraps the source sequence in order to run its subscription and unsubscription logic on the specified dispatcher.
        /// </summary>
        /// <typeparam name="TSource">The type of the elements in the source sequence.</typeparam>
        /// <param name="source">Source sequence.</param>
        /// <param name="dispatcher">Dispatcher whose associated message loop is used to to perform subscription and unsubscription actions on.</param>
        /// <param name="priority">Priority to schedule work items at.</param>
        /// <returns>The source sequence whose subscriptions and unsubscriptions happen on the specified dispatcher.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="source"/> or <paramref name="dispatcher"/> is null.</exception>
        /// <remarks>
        /// Only the side-effects of subscribing to the source sequence and disposing subscriptions to the source sequence are run on the specified dispatcher.
        /// In order to invoke observer callbacks on the specified dispatcher, e.g. to render results in a control, use <see cref="DispatcherObservable.ObserveOn{TSource}(IObservable{TSource}, Dispatcher, DispatcherPriority)"/>.
        /// </remarks>
        public static IObservable<TSource> SubscribeOn<TSource>(this IObservable<TSource> source, Dispatcher dispatcher, DispatcherPriority priority)
        {
            if (source == null)
                throw new ArgumentNullException("source");
            if (dispatcher == null)
                throw new ArgumentNullException("dispatcher");

            return SubscribeOn_<TSource>(source, dispatcher, priority);
        }
#endif

        /// <summary>
        /// Wraps the source sequence in order to run its subscription and unsubscription logic on the specified dispatcher scheduler.
        /// </summary>
        /// <typeparam name="TSource">The type of the elements in the source sequence.</typeparam>
        /// <param name="source">Source sequence.</param>
        /// <param name="scheduler">Dispatcher scheduler to perform subscription and unsubscription actions on.</param>
        /// <returns>The source sequence whose subscriptions and unsubscriptions happen on the specified dispatcher scheduler.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="source"/> or <paramref name="scheduler"/> is null.</exception>
        /// <remarks>
        /// Only the side-effects of subscribing to the source sequence and disposing subscriptions to the source sequence are run on the specified scheduler.
        /// In order to invoke observer callbacks on the specified scheduler, e.g. to render results in a control, use <see cref="DispatcherObservable.ObserveOn{TSource}(IObservable{TSource}, DispatcherScheduler)"/>.
        /// </remarks>
        public static IObservable<TSource> SubscribeOn<TSource>(this IObservable<TSource> source, DispatcherScheduler scheduler)
        {
            if (source == null)
                throw new ArgumentNullException("source");
            if (scheduler == null)
                throw new ArgumentNullException("scheduler");

#if HAS_DISPATCHER_PRIORITY
            return SubscribeOn_<TSource>(source, scheduler.Dispatcher, scheduler.Priority);
#else
            return SubscribeOn_<TSource>(source, scheduler.Dispatcher);
#endif
        }

#if USE_SL_DISPATCHER
        /// <summary>
        /// Wraps the source sequence in order to run its subscription and unsubscription logic on the dispatcher associated with the specified object.
        /// </summary>
        /// <typeparam name="TSource">The type of the elements in the source sequence.</typeparam>
        /// <param name="source">Source sequence.</param>
        /// <param name="dependencyObject">Object to get the dispatcher from.</param>
        /// <returns>The source sequence whose subscriptions and unsubscriptions happen on the specified object's dispatcher.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="source"/> or <paramref name="dependencyObject"/> is null.</exception>
        /// <remarks>
        /// Only the side-effects of subscribing to the source sequence and disposing subscriptions to the source sequence are run on the dispatcher associated with the specified object.
        /// In order to invoke observer callbacks on the dispatcher associated with the specified object, e.g. to render results in a control, use <see cref="DispatcherObservable.ObserveOn{TSource}(IObservable{TSource}, DependencyObject)"/>.
        /// </remarks>
        public static IObservable<TSource> SubscribeOn<TSource>(this IObservable<TSource> source, DependencyObject dependencyObject)
        {
            if (source == null)
                throw new ArgumentNullException("source");
            if (dependencyObject == null)
                throw new ArgumentNullException("dependencyObject");

            return SubscribeOn_<TSource>(source, dependencyObject.Dispatcher);
        }
#else
        /// <summary>
        /// Wraps the source sequence in order to run its subscription and unsubscription logic on the dispatcher associated with the specified object.
        /// </summary>
        /// <typeparam name="TSource">The type of the elements in the source sequence.</typeparam>
        /// <param name="source">Source sequence.</param>
        /// <param name="dispatcherObject">Object to get the dispatcher from.</param>
        /// <returns>The source sequence whose subscriptions and unsubscriptions happen on the specified object's dispatcher.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="source"/> or <paramref name="dispatcherObject"/> is null.</exception>
        /// <remarks>
        /// Only the side-effects of subscribing to the source sequence and disposing subscriptions to the source sequence are run on the dispatcher associated with the specified object.
        /// In order to invoke observer callbacks on the dispatcher associated with the specified object, e.g. to render results in a control, use <see cref="DispatcherObservable.ObserveOn{TSource}(IObservable{TSource}, DispatcherObject)"/>.
        /// </remarks>
        public static IObservable<TSource> SubscribeOn<TSource>(this IObservable<TSource> source, DispatcherObject dispatcherObject)
        {
            if (source == null)
                throw new ArgumentNullException("source");
            if (dispatcherObject == null)
                throw new ArgumentNullException("dispatcherObject");

            return SubscribeOn_<TSource>(source, dispatcherObject.Dispatcher);
        }
#endif

#if HAS_DISPATCHER_PRIORITY
        /// <summary>
        /// Wraps the source sequence in order to run its subscription and unsubscription logic on the dispatcher associated with the specified object.
        /// </summary>
        /// <typeparam name="TSource">The type of the elements in the source sequence.</typeparam>
        /// <param name="source">Source sequence.</param>
        /// <param name="dispatcherObject">Object to get the dispatcher from.</param>
        /// <param name="priority">Priority to schedule work items at.</param>
        /// <returns>The source sequence whose subscriptions and unsubscriptions happen on the specified object's dispatcher.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="source"/> or <paramref name="dispatcherObject"/> is null.</exception>
        /// <remarks>
        /// Only the side-effects of subscribing to the source sequence and disposing subscriptions to the source sequence are run on the dispatcher associated with the specified object.
        /// In order to invoke observer callbacks on the dispatcher associated with the specified object, e.g. to render results in a control, use <see cref="DispatcherObservable.ObserveOn{TSource}(IObservable{TSource}, DispatcherObject, DispatcherPriority)"/>.
        /// </remarks>
        public static IObservable<TSource> SubscribeOn<TSource>(this IObservable<TSource> source, DispatcherObject dispatcherObject, DispatcherPriority priority)
        {
            if (source == null)
                throw new ArgumentNullException("source");
            if (dispatcherObject == null)
                throw new ArgumentNullException("dispatcherObject");

            return SubscribeOn_<TSource>(source, dispatcherObject.Dispatcher, priority);
        }
#endif

        /// <summary>
        /// Wraps the source sequence in order to run its subscription and unsubscription logic on the dispatcher associated with the current thread.
        /// </summary>
        /// <typeparam name="TSource">The type of the elements in the source sequence.</typeparam>
        /// <param name="source">Source sequence.</param>
        /// <returns>The source sequence whose subscriptions and unsubscriptions happen on the current thread's dispatcher.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="source"/> is null.</exception>
        /// <remarks>
        /// Only the side-effects of subscribing to the source sequence and disposing subscriptions to the source sequence are run on the dispatcher associated with the current thread.
        /// In order to invoke observer callbacks on the dispatcher associated with the current thread, e.g. to render results in a control, use <see cref="DispatcherObservable.ObserveOnDispatcher{TSource}(IObservable{TSource})"/>.
        /// </remarks>
        public static IObservable<TSource> SubscribeOnDispatcher<TSource>(this IObservable<TSource> source)
        {
            if (source == null)
                throw new ArgumentNullException("source");

#if USE_SL_DISPATCHER
            return SubscribeOn_<TSource>(source, System.Windows.Deployment.Current.Dispatcher);
#else
            return SubscribeOn_<TSource>(source, DispatcherScheduler.Current.Dispatcher);
#endif
        }

#if HAS_DISPATCHER_PRIORITY
        /// <summary>
        /// Wraps the source sequence in order to run its subscription and unsubscription logic on the dispatcher associated with the current thread.
        /// </summary>
        /// <typeparam name="TSource">The type of the elements in the source sequence.</typeparam>
        /// <param name="source">Source sequence.</param>
        /// <param name="priority">Priority to schedule work items at.</param>
        /// <returns>The source sequence whose observations happen on the current thread's dispatcher.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="source"/> is null.</exception>
        /// <remarks>
        /// Only the side-effects of subscribing to the source sequence and disposing subscriptions to the source sequence are run on the dispatcher associated with the current thread.
        /// In order to invoke observer callbacks on the dispatcher associated with the current thread, e.g. to render results in a control, use <see cref="DispatcherObservable.ObserveOnDispatcher{TSource}(IObservable{TSource}, DispatcherPriority)"/>.
        /// </remarks>
        public static IObservable<TSource> SubscribeOnDispatcher<TSource>(this IObservable<TSource> source, DispatcherPriority priority)
        {
            if (source == null)
                throw new ArgumentNullException("source");

            return SubscribeOn_<TSource>(source, DispatcherScheduler.Current.Dispatcher, priority);
        }

        private static IObservable<TSource> SubscribeOn_<TSource>(IObservable<TSource> source, Dispatcher dispatcher, DispatcherPriority priority)
        {
            return Synchronization.SubscribeOn(source, new DispatcherSynchronizationContext(dispatcher, priority));
        }
#endif

        private static IObservable<TSource> SubscribeOn_<TSource>(IObservable<TSource> source, Dispatcher dispatcher)
        {
            return Synchronization.SubscribeOn(source, new DispatcherSynchronizationContext(dispatcher));
        }

        #endregion
    }
}
#endif
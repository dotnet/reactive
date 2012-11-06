// Copyright (c) Microsoft Open Technologies, Inc. All rights reserved. See License.txt in the project root for license information.

#if WINDOWS
using System.Reactive.Concurrency;
using Windows.UI.Core;
using Windows.UI.Xaml;

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
        /// <param name="dispatcher">Dispatcher whose associated message loop is used to notify observers on.</param>
        /// <returns>The source sequence whose observations happen on the specified dispatcher.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="source"/> or <paramref name="dispatcher"/> is null.</exception>
        public static IObservable<TSource> ObserveOn<TSource>(this IObservable<TSource> source, CoreDispatcher dispatcher)
        {
            if (source == null)
                throw new ArgumentNullException("source");
            if (dispatcher == null)
                throw new ArgumentNullException("dispatcher");

            return Synchronization.ObserveOn(source, new CoreDispatcherScheduler(dispatcher));
        }

        /// <summary>
        /// Wraps the source sequence in order to run its observer callbacks on the specified dispatcher.
        /// </summary>
        /// <typeparam name="TSource">The type of the elements in the source sequence.</typeparam>
        /// <param name="source">Source sequence.</param>
        /// <param name="dispatcher">Dispatcher whose associated message loop is used to notify observers on.</param>
        /// <param name="priority">Priority to schedule work items at.</param>
        /// <returns>The source sequence whose observations happen on the specified dispatcher.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="source"/> or <paramref name="dispatcher"/> is null.</exception>
        public static IObservable<TSource> ObserveOn<TSource>(this IObservable<TSource> source, CoreDispatcher dispatcher, CoreDispatcherPriority priority)
        {
            if (source == null)
                throw new ArgumentNullException("source");
            if (dispatcher == null)
                throw new ArgumentNullException("dispatcher");

            return Synchronization.ObserveOn(source, new CoreDispatcherScheduler(dispatcher, priority));
        }

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

            return Synchronization.ObserveOn(source, new CoreDispatcherScheduler(dependencyObject.Dispatcher));
        }

        /// <summary>
        /// Wraps the source sequence in order to run its observer callbacks on the dispatcher associated with the specified object.
        /// </summary>
        /// <typeparam name="TSource">The type of the elements in the source sequence.</typeparam>
        /// <param name="source">Source sequence.</param>
        /// <param name="dependencyObject">Object to get the dispatcher from.</param>
        /// <param name="priority">Priority to schedule work items at.</param>
        /// <returns>The source sequence whose observations happen on the specified object's dispatcher.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="source"/> or <paramref name="dependencyObject"/> is null.</exception>
        public static IObservable<TSource> ObserveOn<TSource>(this IObservable<TSource> source, DependencyObject dependencyObject, CoreDispatcherPriority priority)
        {
            if (source == null)
                throw new ArgumentNullException("source");
            if (dependencyObject == null)
                throw new ArgumentNullException("dependencyObject");

            return Synchronization.ObserveOn(source, new CoreDispatcherScheduler(dependencyObject.Dispatcher, priority));
        }

        /// <summary>
        /// Wraps the source sequence in order to run its observer callbacks on the dispatcher associated with the current window.
        /// </summary>
        /// <typeparam name="TSource">The type of the elements in the source sequence.</typeparam>
        /// <param name="source">Source sequence.</param>
        /// <returns>The source sequence whose observations happen on the current window's dispatcher.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="source"/> is null.</exception>
        public static IObservable<TSource> ObserveOnDispatcher<TSource>(this IObservable<TSource> source)
        {
            if (source == null)
                throw new ArgumentNullException("source");

            return Synchronization.ObserveOn(source, CoreDispatcherScheduler.Current);
        }

        /// <summary>
        /// Wraps the source sequence in order to run its observer callbacks on the dispatcher associated with the current window.
        /// </summary>
        /// <typeparam name="TSource">The type of the elements in the source sequence.</typeparam>
        /// <param name="source">Source sequence.</param>
        /// <param name="priority">Priority to schedule work items at.</param>
        /// <returns>The source sequence whose observations happen on the current window's dispatcher.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="source"/> is null.</exception>
        public static IObservable<TSource> ObserveOnDispatcher<TSource>(this IObservable<TSource> source, CoreDispatcherPriority priority)
        {
            if (source == null)
                throw new ArgumentNullException("source");

            return Synchronization.ObserveOn(source, new CoreDispatcherScheduler(CoreDispatcherScheduler.Current.Dispatcher, priority));
        }

        #endregion

        #region SubscribeOn[Dispatcher]

        /// <summary>
        /// Wraps the source sequence in order to run its subscription and unsubscription logic on the specified dispatcher.
        /// </summary>
        /// <typeparam name="TSource">The type of the elements in the source sequence.</typeparam>
        /// <param name="source">Source sequence.</param>
        /// <param name="dispatcher">Dispatcher whose associated message loop is used to perform subscription and unsubscription actions on.</param>
        /// <returns>The source sequence whose subscriptions and unsubscriptions happen on the specified dispatcher.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="source"/> or <paramref name="dispatcher"/> is null.</exception>
        /// <remarks>
        /// Only the side-effects of subscribing to the source sequence and disposing subscriptions to the source sequence are run on the specified dispatcher.
        /// In order to invoke observer callbacks on the specified dispatcher, e.g. to render results in a control, use <see cref="DispatcherObservable.ObserveOn{TSource}(IObservable{TSource}, CoreDispatcher)"/>.
        /// </remarks>
        public static IObservable<TSource> SubscribeOn<TSource>(this IObservable<TSource> source, CoreDispatcher dispatcher)
        {
            if (source == null)
                throw new ArgumentNullException("source");
            if (dispatcher == null)
                throw new ArgumentNullException("dispatcher");

            return Synchronization.SubscribeOn(source, new CoreDispatcherScheduler(dispatcher));
        }

        /// <summary>
        /// Wraps the source sequence in order to run its subscription and unsubscription logic on the specified dispatcher.
        /// </summary>
        /// <typeparam name="TSource">The type of the elements in the source sequence.</typeparam>
        /// <param name="source">Source sequence.</param>
        /// <param name="dispatcher">Dispatcher whose associated message loop is used to perform subscription and unsubscription actions on.</param>
        /// <param name="priority">Priority to schedule work items at.</param>
        /// <returns>The source sequence whose subscriptions and unsubscriptions happen on the specified dispatcher.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="source"/> or <paramref name="dispatcher"/> is null.</exception>
        /// <remarks>
        /// Only the side-effects of subscribing to the source sequence and disposing subscriptions to the source sequence are run on the specified dispatcher.
        /// In order to invoke observer callbacks on the specified dispatcher, e.g. to render results in a control, use <see cref="DispatcherObservable.ObserveOn{TSource}(IObservable{TSource}, CoreDispatcher, CoreDispatcherPriority)"/>.
        /// </remarks>
        public static IObservable<TSource> SubscribeOn<TSource>(this IObservable<TSource> source, CoreDispatcher dispatcher, CoreDispatcherPriority priority)
        {
            if (source == null)
                throw new ArgumentNullException("source");
            if (dispatcher == null)
                throw new ArgumentNullException("dispatcher");

            return Synchronization.SubscribeOn(source, new CoreDispatcherScheduler(dispatcher, priority));
        }

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

            return Synchronization.SubscribeOn(source, new CoreDispatcherScheduler(dependencyObject.Dispatcher));
        }

        /// <summary>
        /// Wraps the source sequence in order to run its subscription and unsubscription logic on the dispatcher associated with the specified object.
        /// </summary>
        /// <typeparam name="TSource">The type of the elements in the source sequence.</typeparam>
        /// <param name="source">Source sequence.</param>
        /// <param name="dependencyObject">Object to get the dispatcher from.</param>
        /// <param name="priority">Priority to schedule work items at.</param>
        /// <returns>The source sequence whose subscriptions and unsubscriptions happen on the specified object's dispatcher.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="source"/> or <paramref name="dependencyObject"/> is null.</exception>
        /// <remarks>
        /// Only the side-effects of subscribing to the source sequence and disposing subscriptions to the source sequence are run on the dispatcher associated with the specified object.
        /// In order to invoke observer callbacks on the dispatcher associated with the specified object, e.g. to render results in a control, use <see cref="DispatcherObservable.ObserveOn{TSource}(IObservable{TSource}, DependencyObject, CoreDispatcherPriority)"/>.
        /// </remarks>
        public static IObservable<TSource> SubscribeOn<TSource>(this IObservable<TSource> source, DependencyObject dependencyObject, CoreDispatcherPriority priority)
        {
            if (source == null)
                throw new ArgumentNullException("source");
            if (dependencyObject == null)
                throw new ArgumentNullException("dependencyObject");

            return Synchronization.SubscribeOn(source, new CoreDispatcherScheduler(dependencyObject.Dispatcher, priority));
        }

        /// <summary>
        /// Wraps the source sequence in order to run its subscription and unsubscription logic on the dispatcher associated with the current window.
        /// </summary>
        /// <typeparam name="TSource">The type of the elements in the source sequence.</typeparam>
        /// <param name="source">Source sequence.</param>
        /// <returns>The source sequence whose subscriptions and unsubscriptions happen on the current window's dispatcher.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="source"/> is null.</exception>
        /// <remarks>
        /// Only the side-effects of subscribing to the source sequence and disposing subscriptions to the source sequence are run on the dispatcher associated with the current window.
        /// In order to invoke observer callbacks on the dispatcher associated with the current window, e.g. to render results in a control, use <see cref="DispatcherObservable.ObserveOnDispatcher{TSource}(IObservable{TSource})"/>.
        /// </remarks>
        public static IObservable<TSource> SubscribeOnDispatcher<TSource>(this IObservable<TSource> source)
        {
            if (source == null)
                throw new ArgumentNullException("source");

            return Synchronization.SubscribeOn(source, CoreDispatcherScheduler.Current);
        }

        /// <summary>
        /// Wraps the source sequence in order to run its subscription and unsubscription logic on the dispatcher associated with the current window.
        /// </summary>
        /// <typeparam name="TSource">The type of the elements in the source sequence.</typeparam>
        /// <param name="source">Source sequence.</param>
        /// <param name="priority">Priority to schedule work items at.</param>
        /// <returns>The source sequence whose subscriptions and unsubscriptions happen on the current window's dispatcher.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="source"/> is null.</exception>
        /// <remarks>
        /// Only the side-effects of subscribing to the source sequence and disposing subscriptions to the source sequence are run on the dispatcher associated with the current window.
        /// In order to invoke observer callbacks on the dispatcher associated with the current window, e.g. to render results in a control, use <see cref="DispatcherObservable.ObserveOnDispatcher{TSource}(IObservable{TSource}, CoreDispatcherPriority)"/>.
        /// </remarks>
        public static IObservable<TSource> SubscribeOnDispatcher<TSource>(this IObservable<TSource> source, CoreDispatcherPriority priority)
        {
            if (source == null)
                throw new ArgumentNullException("source");

            return Synchronization.SubscribeOn(source, new CoreDispatcherScheduler(CoreDispatcherScheduler.Current.Dispatcher, priority));
        }

        #endregion
    }
}
#endif
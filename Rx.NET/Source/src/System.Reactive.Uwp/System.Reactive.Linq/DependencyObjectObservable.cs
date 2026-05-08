// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information. 

extern alias SystemReactive;

using System.Reactive.Concurrency;

using Windows.UI.Core;
using Windows.UI.Xaml;

using Synchronization = SystemReactive::System.Reactive.Concurrency.Synchronization;

namespace System.Reactive.Uwp.System.Reactive.Linq
{
    /// <summary>
    /// Rx extension methods for UWP's (Windows.UI.Xaml) <see cref="DependencyObject"/>.
    /// </summary>
    public static class DependencyObjectObservable
    {
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
            {
                throw new ArgumentNullException(nameof(source));
            }

            if (dependencyObject == null)
            {
                throw new ArgumentNullException(nameof(dependencyObject));
            }

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
            {
                throw new ArgumentNullException(nameof(source));
            }

            if (dependencyObject == null)
            {
                throw new ArgumentNullException(nameof(dependencyObject));
            }

            return Synchronization.ObserveOn(source, new CoreDispatcherScheduler(dependencyObject.Dispatcher, priority));
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
        /// In order to invoke observer callbacks on the dispatcher associated with the specified object, e.g. to render results in a control, use <see cref="ObserveOn{TSource}(IObservable{TSource}, DependencyObject)"/>.
        /// </remarks>
        public static IObservable<TSource> SubscribeOn<TSource>(this IObservable<TSource> source, DependencyObject dependencyObject)
        {
            if (source == null)
            {
                throw new ArgumentNullException(nameof(source));
            }

            if (dependencyObject == null)
            {
                throw new ArgumentNullException(nameof(dependencyObject));
            }

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
        /// In order to invoke observer callbacks on the dispatcher associated with the specified object, e.g. to render results in a control, use <see cref="ObserveOn{TSource}(IObservable{TSource}, DependencyObject, CoreDispatcherPriority)"/>.
        /// </remarks>
        public static IObservable<TSource> SubscribeOn<TSource>(this IObservable<TSource> source, DependencyObject dependencyObject, CoreDispatcherPriority priority)
        {
            if (source == null)
            {
                throw new ArgumentNullException(nameof(source));
            }

            if (dependencyObject == null)
            {
                throw new ArgumentNullException(nameof(dependencyObject));
            }

            return Synchronization.SubscribeOn(source, new CoreDispatcherScheduler(dependencyObject.Dispatcher, priority));
        }
    }
}

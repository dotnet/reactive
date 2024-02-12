// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information. 

#if HAS_WPF
using System.Reactive.Concurrency;
using System.Windows.Threading;

namespace System.Reactive.Linq
{
    /// <summary>
    /// Obsolete. The <c>System.Reactive.Integration.Wpf</c> NuGet package defines a
    /// <c>WpfDispatcherObservable</c> class that defines new extension methods to be used in
    /// place of these (also in the <c>System.Reactive.Linq</c> namespace).
    /// </summary>
    /// <remarks>
    /// <para>
    /// The replacement <c>WpfDispatcherObservable</c> class uses different names for extension
    /// methods. When you migrate to that new class from this obsolete one, you will need to change
    /// your code to invoke different method names:
    /// </para>
    /// <list type="table">
    ///     <listheader><term>Rx &lt;= 6.0</term><term>Now</term></listheader>
    ///     <item>
    ///         <term><c>ObserveOn</c> (except for overload taking a <c>DispatcherScheduler</c></term>
    ///         <term><c>ObserveOnWpfDispatcher</c></term>
    ///     </item>
    ///     <item>
    ///         <term><c>ObserveOnDispatcher</c></term>
    ///         <term><c>ObserveOnCurrentWpfDispatcher</c></term>
    ///     </item>
    ///     <item>
    ///         <term><c>SubscribeOn</c> (except for overload taking a <c>DispatcherScheduler</c></term>
    ///         <term><c>SubscribeOnWpfDispatcher</c></term>
    ///     </item>
    ///     <item>
    ///         <term><c>SubscribeOnDispatcher</c></term>
    ///         <term><c>SubscribeOnCurrentWpfDispatcher</c></term>
    ///     </item>
    /// </list>
    /// <para>
    /// The overloads accepting a <see cref="DispatcherScheduler"/> don't need to be renamed because
    /// there is no scope for ambiguity: if you pass the non-obsolete <see cref="DispatcherScheduler"/>
    /// defined in this package, you will get the corresponding non-obsolete overload. (Code using
    /// the old obsolete <c>DispatcherScheduler</c> will get the old obsolete overload.)
    /// </para>
    /// <para>
    /// This will eventually be removed because all UI-framework-specific functionality is being
    /// removed from <c>System.Reactive</c>. This is necessary to fix problems in which
    /// <c>System.Reactive</c> causes applications to end up with dependencies on Windows Forms and
    /// WPF whether they want them or not.
    /// </para>
    /// <para>
    /// Rx defines extension methods for <see cref="IObservable{T}"/> in either the
    /// <c>System.Reactive.Linq</c> or <c>System</c> namespaces. This means the replacement for
    /// this obsolete class has to use different names. We need to retain these old obsolete
    /// extension methods for many years to provide code using Rx with time to adapt, which means
    /// the new extension methods must coexist with these old obsolete ones. Extension methods
    /// are awkward to fully qualify, making it easier for migrating code if the new methods have
    /// visibly different names (and not the same names on a new type).
    /// </para>
    /// </remarks>
    [Obsolete("Use the extension methods defined by the System.Reactive.Linq.WpfDispatcherObservable class in the System.Reactive.Integration.Wpf package instead", error: false)]
    public static class DispatcherObservable
    {
        #region ObserveOn[Dispatcher]

        /// <summary>
        /// Obsolete. Use the <c>ObserveOnWpfDispatcher</c> extension method defined by
        /// <c>System.Reactive.Linq.WpfDispatcherObservable</c> in the
        /// <c>System.Reactive.Integration.Wpf</c> package instead.
        /// </summary>
        /// <typeparam name="TSource">The type of the elements in the source sequence.</typeparam>
        /// <param name="source">Source sequence.</param>
        /// <param name="dispatcher">Dispatcher whose associated message loop is used to notify observers on.</param>
        /// <returns>The source sequence whose observations happen on the specified dispatcher.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="source"/> or <paramref name="dispatcher"/> is null.</exception>
        [Obsolete("Use the ObserveOnWpfDispatcher extension method defined by System.Reactive.Linq.WpfDispatcherObservable in the System.Reactive.Integration.Wpf package instead", error: false)]
        public static IObservable<TSource> ObserveOn<TSource>(this IObservable<TSource> source, Dispatcher dispatcher)
        {
            if (source == null)
            {
                throw new ArgumentNullException(nameof(source));
            }

            if (dispatcher == null)
            {
                throw new ArgumentNullException(nameof(dispatcher));
            }

            return ObserveOn_(source, dispatcher);
        }

        /// <summary>
        /// Obsolete. Use the <c>ObserveOnWpfDispatcher</c> extension method defined by
        /// <c>System.Reactive.Linq.WpfDispatcherObservable</c> in the
        /// <c>System.Reactive.Integration.Wpf</c> package instead.
        /// </summary>
        /// <typeparam name="TSource">The type of the elements in the source sequence.</typeparam>
        /// <param name="source">Source sequence.</param>
        /// <param name="dispatcher">Dispatcher whose associated message loop is used to notify observers on.</param>
        /// <param name="priority">Priority to schedule work items at.</param>
        /// <returns>The source sequence whose observations happen on the specified dispatcher.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="source"/> or <paramref name="dispatcher"/> is null.</exception>
        [Obsolete("Use the ObserveOnWpfDispatcher extension method defined by System.Reactive.Linq.WpfDispatcherObservable in the System.Reactive.Integration.Wpf package instead", error: false)]
        public static IObservable<TSource> ObserveOn<TSource>(this IObservable<TSource> source, Dispatcher dispatcher, DispatcherPriority priority)
        {
            if (source == null)
            {
                throw new ArgumentNullException(nameof(source));
            }

            if (dispatcher == null)
            {
                throw new ArgumentNullException(nameof(dispatcher));
            }

            return ObserveOn_(source, dispatcher, priority);
        }

        /// <summary>
        /// Obsolete. Use the <c>ObserveOnWpfDispatcher</c> extension method defined by
        /// <c>System.Reactive.Linq.WpfDispatcherObservable</c> in the
        /// <c>System.Reactive.Integration.Wpf</c> package instead.
        /// </summary>
        /// <typeparam name="TSource">The type of the elements in the source sequence.</typeparam>
        /// <param name="source">Source sequence.</param>
        /// <param name="scheduler">Dispatcher scheduler to notify observers on.</param>
        /// <returns>The source sequence whose observations happen on the specified dispatcher scheduler.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="source"/> or <paramref name="scheduler"/> is null.</exception>
        [Obsolete("Use the ObserveOnWpfDispatcher extension method defined by System.Reactive.Linq.WpfDispatcherObservable in the System.Reactive.Integration.Wpf package instead", error: false)]
        public static IObservable<TSource> ObserveOn<TSource>(this IObservable<TSource> source, DispatcherScheduler scheduler)
        {
            if (source == null)
            {
                throw new ArgumentNullException(nameof(source));
            }

            if (scheduler == null)
            {
                throw new ArgumentNullException(nameof(scheduler));
            }

            return ObserveOn_(source, scheduler.Dispatcher, scheduler.Priority);
        }

        /// <summary>
        /// Obsolete. Use the <c>ObserveOnWpfDispatcher</c> extension method defined by
        /// <c>System.Reactive.Linq.WpfDispatcherObservable</c> in the
        /// <c>System.Reactive.Integration.Wpf</c> package instead.
        /// </summary>
        /// <typeparam name="TSource">The type of the elements in the source sequence.</typeparam>
        /// <param name="source">Source sequence.</param>
        /// <param name="dispatcherObject">Object to get the dispatcher from.</param>
        /// <returns>The source sequence whose observations happen on the specified object's dispatcher.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="source"/> or <paramref name="dispatcherObject"/> is null.</exception>
        [Obsolete("Use the ObserveOnWpfDispatcher extension method defined by System.Reactive.Linq.WpfDispatcherObservable in the System.Reactive.Integration.Wpf package instead", error: false)]
        public static IObservable<TSource> ObserveOn<TSource>(this IObservable<TSource> source, DispatcherObject dispatcherObject)
        {
            if (source == null)
            {
                throw new ArgumentNullException(nameof(source));
            }

            if (dispatcherObject == null)
            {
                throw new ArgumentNullException(nameof(dispatcherObject));
            }

            return ObserveOn_(source, dispatcherObject.Dispatcher);
        }

        /// <summary>
        /// Obsolete. Use the <c>ObserveOnWpfDispatcher</c> extension method defined by
        /// <c>System.Reactive.Linq.WpfDispatcherObservable</c> in the
        /// <c>System.Reactive.Integration.Wpf</c> package instead.
        /// </summary>
        /// <typeparam name="TSource">The type of the elements in the source sequence.</typeparam>
        /// <param name="source">Source sequence.</param>
        /// <param name="dispatcherObject">Object to get the dispatcher from.</param>
        /// <param name="priority">Priority to schedule work items at.</param>
        /// <returns>The source sequence whose observations happen on the specified object's dispatcher.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="source"/> or <paramref name="dispatcherObject"/> is null.</exception>
        [Obsolete("Use the ObserveOnWpfDispatcher extension method defined by System.Reactive.Linq.WpfDispatcherObservable in the System.Reactive.Integration.Wpf package instead", error: false)]
        public static IObservable<TSource> ObserveOn<TSource>(this IObservable<TSource> source, DispatcherObject dispatcherObject, DispatcherPriority priority)
        {
            if (source == null)
            {
                throw new ArgumentNullException(nameof(source));
            }

            if (dispatcherObject == null)
            {
                throw new ArgumentNullException(nameof(dispatcherObject));
            }

            return ObserveOn_(source, dispatcherObject.Dispatcher, priority);
        }

        /// <summary>
        /// Obsolete. Use the <c>ObserveOnCurrentWpfDispatcher</c> extension method defined by
        /// <c>System.Reactive.Linq.WpfDispatcherObservable</c> in the
        /// <c>System.Reactive.Integration.Wpf</c> package instead.
        /// </summary>
        /// <typeparam name="TSource">The type of the elements in the source sequence.</typeparam>
        /// <param name="source">Source sequence.</param>
        /// <returns>The source sequence whose observations happen on the current thread's dispatcher.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="source"/> is null.</exception>
        [Obsolete("Use the ObserveOnCurrentWpfDispatcher extension method defined by System.Reactive.Linq.WpfDispatcherObservable in the System.Reactive.Integration.Wpf package instead", error: false)]
        public static IObservable<TSource> ObserveOnDispatcher<TSource>(this IObservable<TSource> source)
        {
            if (source == null)
            {
                throw new ArgumentNullException(nameof(source));
            }

            return ObserveOn_(source, DispatcherScheduler.Current.Dispatcher);
        }

        /// <summary>
        /// Obsolete. Use the <c>ObserveOnCurrentWpfDispatcher</c> extension method defined by
        /// <c>System.Reactive.Linq.WpfDispatcherObservable</c> in the
        /// <c>System.Reactive.Integration.Wpf</c> package instead.
        /// </summary>
        /// <typeparam name="TSource">The type of the elements in the source sequence.</typeparam>
        /// <param name="source">Source sequence.</param>
        /// <param name="priority">Priority to schedule work items at.</param>
        /// <returns>The source sequence whose observations happen on the current thread's dispatcher.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="source"/> is null.</exception>
        [Obsolete("Use the ObserveOnCurrentWpfDispatcher extension method defined by System.Reactive.Linq.WpfDispatcherObservable in the System.Reactive.Integration.Wpf package instead", error: false)]
        public static IObservable<TSource> ObserveOnDispatcher<TSource>(this IObservable<TSource> source, DispatcherPriority priority)
        {
            if (source == null)
            {
                throw new ArgumentNullException(nameof(source));
            }

            return ObserveOn_(source, DispatcherScheduler.Current.Dispatcher, priority);
        }

        private static IObservable<TSource> ObserveOn_<TSource>(IObservable<TSource> source, Dispatcher dispatcher, DispatcherPriority priority)
        {
            return Synchronization.ObserveOn(source, new DispatcherSynchronizationContext(dispatcher, priority));
        }


        private static IObservable<TSource> ObserveOn_<TSource>(IObservable<TSource> source, Dispatcher dispatcher)
        {
            return Synchronization.ObserveOn(source, new DispatcherSynchronizationContext(dispatcher));
        }

        #endregion

        #region SubscribeOn[Dispatcher]

        /// <summary>
        /// Obsolete. Use the <c>SubscribeOnWpfDispatcher</c> extension method defined by
        /// <c>System.Reactive.Linq.WpfDispatcherObservable</c> in the
        /// <c>System.Reactive.Integration.Wpf</c> package instead.
        /// </summary>
        /// <typeparam name="TSource">The type of the elements in the source sequence.</typeparam>
        /// <param name="source">Source sequence.</param>
        /// <param name="dispatcher">Dispatcher whose associated message loop is used to perform subscription and unsubscription actions on.</param>
        /// <returns>The source sequence whose subscriptions and unsubscriptions happen on the specified dispatcher.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="source"/> or <paramref name="dispatcher"/> is null.</exception>
        /// <remarks>
        /// Only the side-effects of subscribing to the source sequence and disposing subscriptions to the source sequence are run on the specified dispatcher.
        /// In order to invoke observer callbacks on the specified dispatcher, e.g. to render results in a control, use <see cref="DispatcherObservable.ObserveOn{TSource}(IObservable{TSource}, Dispatcher)"/>.
        /// </remarks>
        [Obsolete("Use the SubscribeOnWpfDispatcher extension method defined by System.Reactive.Linq.WpfDispatcherObservable in the System.Reactive.Integration.Wpf package instead", error: false)]
        public static IObservable<TSource> SubscribeOn<TSource>(this IObservable<TSource> source, Dispatcher dispatcher)
        {
            if (source == null)
            {
                throw new ArgumentNullException(nameof(source));
            }

            if (dispatcher == null)
            {
                throw new ArgumentNullException(nameof(dispatcher));
            }

            return SubscribeOn_(source, dispatcher);
        }

        /// <summary>
        /// Obsolete. Use the <c>SubscribeOnWpfDispatcher</c> extension method defined by
        /// <c>System.Reactive.Linq.WpfDispatcherObservable</c> in the
        /// <c>System.Reactive.Integration.Wpf</c> package instead.
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
        [Obsolete("Use the SubscribeOnWpfDispatcher extension method defined by System.Reactive.Linq.WpfDispatcherObservable in the System.Reactive.Integration.Wpf package instead", error: false)]
        public static IObservable<TSource> SubscribeOn<TSource>(this IObservable<TSource> source, Dispatcher dispatcher, DispatcherPriority priority)
        {
            if (source == null)
            {
                throw new ArgumentNullException(nameof(source));
            }

            if (dispatcher == null)
            {
                throw new ArgumentNullException(nameof(dispatcher));
            }

            return SubscribeOn_(source, dispatcher, priority);
        }

        /// <summary>
        /// Obsolete. Use the <c>SubscribeOnWpfDispatcher</c> extension method defined by
        /// <c>System.Reactive.Linq.WpfDispatcherObservable</c> in the
        /// <c>System.Reactive.Integration.Wpf</c> package instead.
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
        [Obsolete("Use the SubscribeOnWpfDispatcher extension method defined by System.Reactive.Linq.WpfDispatcherObservable in the System.Reactive.Integration.Wpf package instead", error: false)]
        public static IObservable<TSource> SubscribeOn<TSource>(this IObservable<TSource> source, DispatcherScheduler scheduler)
        {
            if (source == null)
            {
                throw new ArgumentNullException(nameof(source));
            }

            if (scheduler == null)
            {
                throw new ArgumentNullException(nameof(scheduler));
            }

            return SubscribeOn_(source, scheduler.Dispatcher, scheduler.Priority);
        }

        /// <summary>
        /// Obsolete. Use the <c>SubscribeOnWpfDispatcher</c> extension method defined by
        /// <c>System.Reactive.Linq.WpfDispatcherObservable</c> in the
        /// <c>System.Reactive.Integration.Wpf</c> package instead.
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
        [Obsolete("Use the SubscribeOnWpfDispatcher extension method defined by System.Reactive.Linq.WpfDispatcherObservable in the System.Reactive.Integration.Wpf package instead", error: false)]
        public static IObservable<TSource> SubscribeOn<TSource>(this IObservable<TSource> source, DispatcherObject dispatcherObject)
        {
            if (source == null)
            {
                throw new ArgumentNullException(nameof(source));
            }

            if (dispatcherObject == null)
            {
                throw new ArgumentNullException(nameof(dispatcherObject));
            }

            return SubscribeOn_(source, dispatcherObject.Dispatcher);
        }

        /// <summary>
        /// Obsolete. Use the <c>SubscribeOnWpfDispatcher</c> extension method defined by
        /// <c>System.Reactive.Linq.WpfDispatcherObservable</c> in the
        /// <c>System.Reactive.Integration.Wpf</c> package instead.
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
        [Obsolete("Use the SubscribeOnWpfDispatcher extension method defined by System.Reactive.Linq.WpfDispatcherObservable in the System.Reactive.Integration.Wpf package instead", error: false)]
        public static IObservable<TSource> SubscribeOn<TSource>(this IObservable<TSource> source, DispatcherObject dispatcherObject, DispatcherPriority priority)
        {
            if (source == null)
            {
                throw new ArgumentNullException(nameof(source));
            }

            if (dispatcherObject == null)
            {
                throw new ArgumentNullException(nameof(dispatcherObject));
            }

            return SubscribeOn_(source, dispatcherObject.Dispatcher, priority);
        }

        /// <summary>
        /// Obsolete. Use the <c>SubscribeOnCurrentWpfDispatcher</c> extension method defined by
        /// <c>System.Reactive.Linq.WpfDispatcherObservable</c> in the
        /// <c>System.Reactive.Integration.Wpf</c> package instead.
        /// </summary>
        /// <typeparam name="TSource">The type of the elements in the source sequence.</typeparam>
        /// <param name="source">Source sequence.</param>
        /// <returns>The source sequence whose subscriptions and unsubscriptions happen on the current thread's dispatcher.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="source"/> is null.</exception>
        /// <remarks>
        /// Only the side-effects of subscribing to the source sequence and disposing subscriptions to the source sequence are run on the dispatcher associated with the current thread.
        /// In order to invoke observer callbacks on the dispatcher associated with the current thread, e.g. to render results in a control, use <see cref="DispatcherObservable.ObserveOnDispatcher{TSource}(IObservable{TSource})"/>.
        /// </remarks>
        [Obsolete("Use the SubscribeOnCurrentWpfDispatcher extension method defined by System.Reactive.Linq.WpfDispatcherObservable in the System.Reactive.Integration.Wpf package instead", error: false)]
        public static IObservable<TSource> SubscribeOnDispatcher<TSource>(this IObservable<TSource> source)
        {
            if (source == null)
            {
                throw new ArgumentNullException(nameof(source));
            }

            return SubscribeOn_(source, DispatcherScheduler.Current.Dispatcher);
        }

        /// <summary>
        /// Obsolete. Use the <c>SubscribeOnCurrentWpfDispatcher</c> extension method defined by
        /// <c>System.Reactive.Linq.WpfDispatcherObservable</c> in the
        /// <c>System.Reactive.Integration.Wpf</c> package instead.
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
        [Obsolete("Use the SubscribeOnCurrentWpfDispatcher extension method defined by System.Reactive.Linq.WpfDispatcherObservable in the System.Reactive.Integration.Wpf package instead", error: false)]
        public static IObservable<TSource> SubscribeOnDispatcher<TSource>(this IObservable<TSource> source, DispatcherPriority priority)
        {
            if (source == null)
            {
                throw new ArgumentNullException(nameof(source));
            }

            return SubscribeOn_(source, DispatcherScheduler.Current.Dispatcher, priority);
        }

        private static IObservable<TSource> SubscribeOn_<TSource>(IObservable<TSource> source, Dispatcher dispatcher, DispatcherPriority priority)
        {
            return Synchronization.SubscribeOn(source, new DispatcherSynchronizationContext(dispatcher, priority));
        }


        private static IObservable<TSource> SubscribeOn_<TSource>(IObservable<TSource> source, Dispatcher dispatcher)
        {
            return Synchronization.SubscribeOn(source, new DispatcherSynchronizationContext(dispatcher));
        }

        #endregion
    }
}
#endif

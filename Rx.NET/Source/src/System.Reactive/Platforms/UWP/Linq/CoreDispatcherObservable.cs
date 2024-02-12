// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information. 

#if WINDOWS
using System.Reactive.Concurrency;
using Windows.UI.Core;

#if HAS_OS_XAML
using Windows.UI.Xaml;
#endif

namespace System.Reactive.Linq
{
    /// <summary>
    /// Obsolete. The <c>System.Reactive.Integration.WindowsRuntime</c> NuGet package defines a
    /// <c>WindowsRuntimeCoreDispatcherObservable</c> class that defines new extension methods to
    /// be used in place of these (also in the <c>System.Reactive.Linq</c> namespace).
    /// </summary>
    /// <remarks>
    /// <para>
    /// The replacement <c>WindowsRuntimeCoreDispatcherObservable</c> class uses different names for extension
    /// methods. When you migrate to that new class from this obsolete one, you will need to change
    /// your code to invoke different method names:
    /// </para>
    /// <list type="table">
    ///     <listheader><term>Rx &lt;= 6.0</term><term>Now</term></listheader>
    ///     <item>
    ///         <term><c>ObserveOn</c></term>
    ///         <term><c>ObserveOnWindowsRuntimeCoreDispatcher</c></term>
    ///     </item>
    ///     <item>
    ///         <term><c>ObserveOnCoreDispatcher</c></term>
    ///         <term><c>ObserveOnCurrentWindowsRuntimeCoreDispatcher</c></term>
    ///     </item>
    ///     <item>
    ///         <term><c>SubscribeOn</c></term>
    ///         <term><c>SubscribeOnWindowsRuntimeCoreDispatcher</c></term>
    ///     </item>
    ///     <item>
    ///         <term><c>SubscribeOnCoreDispatcher</c></term>
    ///         <term><c>SubscribeOnCurrentWindowsRuntimeCoreDispatcher</c></term>
    ///     </item>
    /// </list>
    /// <para>
    /// The name changes are necessary because of a limitation of the <c>Obsolete</c> attribute: if
    /// you want to move an existing extension method into a different package, and you leave the
    /// old one in place (and marked as <c>Obsolete</c>) for a few versions to enable a gradual
    /// transition to the new one, you will cause a problem if you keep the method name the same.
    /// The problem is that both the old and new extension methods will be in scope simultaneously,
    /// so the compiler will complain of ambiguity when you try to use them. In some cases you can
    /// mitigate this by defining the new type in a different namespace, but the problem is that
    /// these extension methods for <see cref="IObservable{T}"/> are defined in the
    /// <c>System.Reactive.Linq</c> namespace. Code often brings that namespace into scope for more
    /// than one reason, so we can't just tell developers to replace <c>using
    /// System.Reactive.Linq;</c> with some other namespace. While that might fix the ambiguity
    /// problem, it's likely to cause a load of new problems instead.
    /// </para>
    /// <para>
    /// The only practical solution for this is for the new methods to have different names than
    /// the old ones. (There is a proposal for being able to annotate a method as being for binary
    /// compatibility only, but it will be some time before that is available to all projects using
    /// Rx.NET.)
    /// </para>
    /// <para>
    /// This type will eventually be removed because all UI-framework-specific functionality is
    /// being removed from <c>System.Reactive</c>. This is necessary to fix problems in which
    /// <c>System.Reactive</c> causes applications to end up with dependencies on Windows Forms and
    /// WPF whether they want them or not.
    /// </para>
    /// </remarks>
    [CLSCompliant(false)]
    [Obsolete("Use the extension methods defined by the System.Reactive.Linq.WindowsRuntimeCoreDispatcherObservable class in the System.Reactive.Integration.WindowsRuntime package instead", error: false)]
    public static class CoreDispatcherObservable
    {
        #region ObserveOn[CoreDispatcher]

        /// <summary>
        /// Obsolete. Use the <c>ObserveOnWindowsRuntimeCoreDispatcher</c> extension method defined by
        /// <c>System.Reactive.Linq.WindowsRuntimeCoreDispatcherObservable</c> in the
        /// <c>System.Reactive.Integration.WindowsRuntime</c> package instead.
        /// </summary>
        /// <typeparam name="TSource">The type of the elements in the source sequence.</typeparam>
        /// <param name="source">Source sequence.</param>
        /// <param name="dispatcher">Dispatcher whose associated message loop is used to notify observers on.</param>
        /// <returns>The source sequence whose observations happen on the specified dispatcher.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="source"/> or <paramref name="dispatcher"/> is null.</exception>
        [Obsolete("Use the ObserveOnWindowsRuntimeCoreDispatcher extension method defined by System.Reactive.Linq.WindowsRuntimeCoreDispatcherObservable in the System.Reactive.Integration.WindowsRuntime package instead", error: false)]
        public static IObservable<TSource> ObserveOn<TSource>(this IObservable<TSource> source, CoreDispatcher dispatcher)
        {
            if (source == null)
            {
                throw new ArgumentNullException(nameof(source));
            }

            if (dispatcher == null)
            {
                throw new ArgumentNullException(nameof(dispatcher));
            }

            return Synchronization.ObserveOn(source, new CoreDispatcherScheduler(dispatcher));
        }

        /// <summary>
        /// Obsolete. Use the <c>ObserveOnWindowsRuntimeCoreDispatcher</c> extension method defined by
        /// <c>System.Reactive.Linq.WindowsRuntimeCoreDispatcherObservable</c> in the
        /// <c>System.Reactive.Integration.WindowsRuntime</c> package instead.
        /// </summary>
        /// <typeparam name="TSource">The type of the elements in the source sequence.</typeparam>
        /// <param name="source">Source sequence.</param>
        /// <param name="dispatcher">Dispatcher whose associated message loop is used to notify observers on.</param>
        /// <param name="priority">Priority to schedule work items at.</param>
        /// <returns>The source sequence whose observations happen on the specified dispatcher.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="source"/> or <paramref name="dispatcher"/> is null.</exception>
        [Obsolete("Use the ObserveOnWindowsRuntimeCoreDispatcher extension method defined by System.Reactive.Linq.WindowsRuntimeCoreDispatcherObservable in the System.Reactive.Integration.WindowsRuntime package instead", error: false)]
        public static IObservable<TSource> ObserveOn<TSource>(this IObservable<TSource> source, CoreDispatcher dispatcher, CoreDispatcherPriority priority)
        {
            if (source == null)
            {
                throw new ArgumentNullException(nameof(source));
            }

            if (dispatcher == null)
            {
                throw new ArgumentNullException(nameof(dispatcher));
            }

            return Synchronization.ObserveOn(source, new CoreDispatcherScheduler(dispatcher, priority));
        }

#if HAS_OS_XAML
        /// <summary>
        /// Obsolete. Use the <c>ObserveOnWindowsRuntimeCoreDispatcher</c> extension method defined by
        /// <c>System.Reactive.Linq.WindowsRuntimeCoreDispatcherObservable</c> in the
        /// <c>System.Reactive.Integration.WindowsRuntime</c> package instead.
        /// </summary>
        /// <typeparam name="TSource">The type of the elements in the source sequence.</typeparam>
        /// <param name="source">Source sequence.</param>
        /// <param name="dependencyObject">Object to get the dispatcher from.</param>
        /// <returns>The source sequence whose observations happen on the specified object's dispatcher.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="source"/> or <paramref name="dependencyObject"/> is null.</exception>
        [Obsolete("Use the ObserveOnWindowsRuntimeCoreDispatcher extension method defined by System.Reactive.Linq.WindowsRuntimeCoreDispatcherObservable in the System.Reactive.Integration.WindowsRuntime package instead", error: false)]
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
        /// Obsolete. Use the <c>ObserveOnWindowsRuntimeCoreDispatcher</c> extension method defined by
        /// <c>System.Reactive.Linq.WindowsRuntimeCoreDispatcherObservable</c> in the
        /// <c>System.Reactive.Integration.WindowsRuntime</c> package instead.
        /// </summary>
        /// <typeparam name="TSource">The type of the elements in the source sequence.</typeparam>
        /// <param name="source">Source sequence.</param>
        /// <param name="dependencyObject">Object to get the dispatcher from.</param>
        /// <param name="priority">Priority to schedule work items at.</param>
        /// <returns>The source sequence whose observations happen on the specified object's dispatcher.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="source"/> or <paramref name="dependencyObject"/> is null.</exception>
        [Obsolete("Use the ObserveOnWindowsRuntimeCoreDispatcher extension method defined by System.Reactive.Linq.WindowsRuntimeCoreDispatcherObservable in the System.Reactive.Integration.WindowsRuntime package instead", error: false)]
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
#endif
        /// <summary>
        /// Obsolete. Use the <c>ObserveOnCurrentWindowsRuntimeCoreDispatcher</c> extension method defined by
        /// <c>System.Reactive.Linq.WindowsRuntimeCoreDispatcherObservable</c> in the
        /// <c>System.Reactive.Integration.WindowsRuntime</c> package instead.
        /// </summary>
        /// <typeparam name="TSource">The type of the elements in the source sequence.</typeparam>
        /// <param name="source">Source sequence.</param>
        /// <returns>The source sequence whose observations happen on the current window's dispatcher.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="source"/> is null.</exception>
        [Obsolete("Use the ObserveOnCurrentWindowsRuntimeCoreDispatcher extension method defined by System.Reactive.Linq.WindowsRuntimeCoreDispatcherObservable in the System.Reactive.Integration.WindowsRuntime package instead", error: false)]
        public static IObservable<TSource> ObserveOnCoreDispatcher<TSource>(this IObservable<TSource> source)
        {
            if (source == null)
            {
                throw new ArgumentNullException(nameof(source));
            }

            return Synchronization.ObserveOn(source, CoreDispatcherScheduler.Current);
        }

        /// <summary>
        /// Obsolete. Use the <c>ObserveOnCurrentWindowsRuntimeCoreDispatcher</c> extension method defined by
        /// <c>System.Reactive.Linq.WindowsRuntimeCoreDispatcherObservable</c> in the
        /// <c>System.Reactive.Integration.WindowsRuntime</c> package instead.
        /// </summary>
        /// <typeparam name="TSource">The type of the elements in the source sequence.</typeparam>
        /// <param name="source">Source sequence.</param>
        /// <param name="priority">Priority to schedule work items at.</param>
        /// <returns>The source sequence whose observations happen on the current window's dispatcher.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="source"/> is null.</exception>
        [Obsolete("Use the ObserveOnCurrentWindowsRuntimeCoreDispatcher extension method defined by System.Reactive.Linq.WindowsRuntimeCoreDispatcherObservable in the System.Reactive.Integration.WindowsRuntime package instead", error: false)]
        public static IObservable<TSource> ObserveOnDispatcher<TSource>(this IObservable<TSource> source, CoreDispatcherPriority priority)
        {
            if (source == null)
            {
                throw new ArgumentNullException(nameof(source));
            }

            return Synchronization.ObserveOn(source, new CoreDispatcherScheduler(CoreDispatcherScheduler.Current.Dispatcher, priority));
        }

        #endregion

        #region SubscribeOn[CoreDispatcher]

        /// <summary>
        /// Obsolete. Use the <c>SubscribeOnWindowsRuntimeCoreDispatcher</c> extension method defined by
        /// <c>System.Reactive.Linq.WindowsRuntimeCoreDispatcherObservable</c> in the
        /// <c>System.Reactive.Integration.WindowsRuntime</c> package instead.
        /// </summary>
        /// <typeparam name="TSource">The type of the elements in the source sequence.</typeparam>
        /// <param name="source">Source sequence.</param>
        /// <param name="dispatcher">Dispatcher whose associated message loop is used to perform subscription and unsubscription actions on.</param>
        /// <returns>The source sequence whose subscriptions and unsubscriptions happen on the specified dispatcher.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="source"/> or <paramref name="dispatcher"/> is null.</exception>
        /// <remarks>
        /// Only the side-effects of subscribing to the source sequence and disposing subscriptions to the source sequence are run on the specified dispatcher.
        /// In order to invoke observer callbacks on the specified dispatcher, e.g. to render results in a control, use <see cref="CoreDispatcherObservable.ObserveOn{TSource}(IObservable{TSource}, CoreDispatcher)"/>.
        /// </remarks>
        [Obsolete("Use the SubscribeOnWindowsRuntimeCoreDispatcher extension method defined by System.Reactive.Linq.WindowsRuntimeCoreDispatcherObservable in the System.Reactive.Integration.WindowsRuntime package instead", error: false)]
        public static IObservable<TSource> SubscribeOn<TSource>(this IObservable<TSource> source, CoreDispatcher dispatcher)
        {
            if (source == null)
            {
                throw new ArgumentNullException(nameof(source));
            }

            if (dispatcher == null)
            {
                throw new ArgumentNullException(nameof(dispatcher));
            }

            return Synchronization.SubscribeOn(source, new CoreDispatcherScheduler(dispatcher));
        }

        /// <summary>
        /// Obsolete. Use the <c>SubscribeOnWindowsRuntimeCoreDispatcher</c> extension method defined by
        /// <c>System.Reactive.Linq.WindowsRuntimeCoreDispatcherObservable</c> in the
        /// <c>System.Reactive.Integration.WindowsRuntime</c> package instead.
        /// </summary>
        /// <typeparam name="TSource">The type of the elements in the source sequence.</typeparam>
        /// <param name="source">Source sequence.</param>
        /// <param name="dispatcher">Dispatcher whose associated message loop is used to perform subscription and unsubscription actions on.</param>
        /// <param name="priority">Priority to schedule work items at.</param>
        /// <returns>The source sequence whose subscriptions and unsubscriptions happen on the specified dispatcher.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="source"/> or <paramref name="dispatcher"/> is null.</exception>
        /// <remarks>
        /// Only the side-effects of subscribing to the source sequence and disposing subscriptions to the source sequence are run on the specified dispatcher.
        /// In order to invoke observer callbacks on the specified dispatcher, e.g. to render results in a control, use <see cref="CoreDispatcherObservable.ObserveOn{TSource}(IObservable{TSource}, CoreDispatcher, CoreDispatcherPriority)"/>.
        /// </remarks>
        [Obsolete("Use the SubscribeOnWindowsRuntimeCoreDispatcher extension method defined by System.Reactive.Linq.WindowsRuntimeCoreDispatcherObservable in the System.Reactive.Integration.WindowsRuntime package instead", error: false)]
        public static IObservable<TSource> SubscribeOn<TSource>(this IObservable<TSource> source, CoreDispatcher dispatcher, CoreDispatcherPriority priority)
        {
            if (source == null)
            {
                throw new ArgumentNullException(nameof(source));
            }

            if (dispatcher == null)
            {
                throw new ArgumentNullException(nameof(dispatcher));
            }

            return Synchronization.SubscribeOn(source, new CoreDispatcherScheduler(dispatcher, priority));
        }

#if HAS_OS_XAML
        /// <summary>
        /// Obsolete. Use the <c>SubscribeOnWindowsRuntimeCoreDispatcher</c> extension method defined by
        /// <c>System.Reactive.Linq.WindowsRuntimeCoreDispatcherObservable</c> in the
        /// <c>System.Reactive.Integration.WindowsRuntime</c> package instead.
        /// </summary>
        /// <typeparam name="TSource">The type of the elements in the source sequence.</typeparam>
        /// <param name="source">Source sequence.</param>
        /// <param name="dependencyObject">Object to get the dispatcher from.</param>
        /// <returns>The source sequence whose subscriptions and unsubscriptions happen on the specified object's dispatcher.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="source"/> or <paramref name="dependencyObject"/> is null.</exception>
        /// <remarks>
        /// Only the side-effects of subscribing to the source sequence and disposing subscriptions to the source sequence are run on the dispatcher associated with the specified object.
        /// In order to invoke observer callbacks on the dispatcher associated with the specified object, e.g. to render results in a control, use <see cref="CoreDispatcherObservable.ObserveOn{TSource}(IObservable{TSource}, DependencyObject)"/>.
        /// </remarks>
        [Obsolete("Use the SubscribeOnWindowsRuntimeCoreDispatcher extension method defined by System.Reactive.Linq.WindowsRuntimeCoreDispatcherObservable in the System.Reactive.Integration.WindowsRuntime package instead", error: false)]
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
        /// Obsolete. Use the <c>SubscribeOnWindowsRuntimeCoreDispatcher</c> extension method defined by
        /// <c>System.Reactive.Linq.WindowsRuntimeCoreDispatcherObservable</c> in the
        /// <c>System.Reactive.Integration.WindowsRuntime</c> package instead.
        /// </summary>
        /// <typeparam name="TSource">The type of the elements in the source sequence.</typeparam>
        /// <param name="source">Source sequence.</param>
        /// <param name="dependencyObject">Object to get the dispatcher from.</param>
        /// <param name="priority">Priority to schedule work items at.</param>
        /// <returns>The source sequence whose subscriptions and unsubscriptions happen on the specified object's dispatcher.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="source"/> or <paramref name="dependencyObject"/> is null.</exception>
        /// <remarks>
        /// Only the side-effects of subscribing to the source sequence and disposing subscriptions to the source sequence are run on the dispatcher associated with the specified object.
        /// In order to invoke observer callbacks on the dispatcher associated with the specified object, e.g. to render results in a control, use <see cref="CoreDispatcherObservable.ObserveOn{TSource}(IObservable{TSource}, DependencyObject, CoreDispatcherPriority)"/>.
        /// </remarks>
        [Obsolete("Use the SubscribeOnWindowsRuntimeCoreDispatcher extension method defined by System.Reactive.Linq.WindowsRuntimeCoreDispatcherObservable in the System.Reactive.Integration.WindowsRuntime package instead", error: false)]
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
#endif

        /// <summary>
        /// Obsolete. Use the <c>SubscribeOnCurrentWindowsRuntimeCoreDispatcher</c> extension method defined by
        /// <c>System.Reactive.Linq.WindowsRuntimeCoreDispatcherObservable</c> in the
        /// <c>System.Reactive.Integration.WindowsRuntime</c> package instead.
        /// </summary>
        /// <typeparam name="TSource">The type of the elements in the source sequence.</typeparam>
        /// <param name="source">Source sequence.</param>
        /// <returns>The source sequence whose subscriptions and unsubscriptions happen on the current window's dispatcher.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="source"/> is null.</exception>
        /// <remarks>
        /// Only the side-effects of subscribing to the source sequence and disposing subscriptions to the source sequence are run on the dispatcher associated with the current window.
        /// In order to invoke observer callbacks on the dispatcher associated with the current window, e.g. to render results in a control, use <see cref="CoreDispatcherObservable.ObserveOnCoreDispatcher{TSource}(IObservable{TSource})"/>.
        /// </remarks>
        [Obsolete("Use the SubscribeOnCurrentWindowsRuntimeCoreDispatcher extension method defined by System.Reactive.Linq.WindowsRuntimeCoreDispatcherObservable in the System.Reactive.Integration.WindowsRuntime package instead", error: false)]
        public static IObservable<TSource> SubscribeOnCoreDispatcher<TSource>(this IObservable<TSource> source)
        {
            if (source == null)
            {
                throw new ArgumentNullException(nameof(source));
            }

            return Synchronization.SubscribeOn(source, CoreDispatcherScheduler.Current);
        }

        /// <summary>
        /// Obsolete. Use the <c>SubscribeOnCurrentWindowsRuntimeCoreDispatcher</c> extension method defined by
        /// <c>System.Reactive.Linq.WindowsRuntimeCoreDispatcherObservable</c> in the
        /// <c>System.Reactive.Integration.WindowsRuntime</c> package instead.
        /// </summary>
        /// <typeparam name="TSource">The type of the elements in the source sequence.</typeparam>
        /// <param name="source">Source sequence.</param>
        /// <param name="priority">Priority to schedule work items at.</param>
        /// <returns>The source sequence whose subscriptions and unsubscriptions happen on the current window's dispatcher.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="source"/> is null.</exception>
        /// <remarks>
        /// Only the side-effects of subscribing to the source sequence and disposing subscriptions to the source sequence are run on the dispatcher associated with the current window.
        /// In order to invoke observer callbacks on the dispatcher associated with the current window, e.g. to render results in a control, use <see cref="CoreDispatcherObservable.ObserveOnDispatcher{TSource}(IObservable{TSource}, CoreDispatcherPriority)"/>.
        /// </remarks>
        [Obsolete("Use the SubscribeOnCurrentWindowsRuntimeCoreDispatcher extension method defined by System.Reactive.Linq.WindowsRuntimeCoreDispatcherObservable in the System.Reactive.Integration.WindowsRuntime package instead", error: false)]
        public static IObservable<TSource> SubscribeOnDispatcher<TSource>(this IObservable<TSource> source, CoreDispatcherPriority priority)
        {
            if (source == null)
            {
                throw new ArgumentNullException(nameof(source));
            }

            return Synchronization.SubscribeOn(source, new CoreDispatcherScheduler(CoreDispatcherScheduler.Current.Dispatcher, priority));
        }

        #endregion
    }
}
#endif

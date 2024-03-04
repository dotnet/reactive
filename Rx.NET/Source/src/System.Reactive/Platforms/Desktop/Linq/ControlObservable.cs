// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information. 

using System.Reactive.Concurrency;
using System.Windows.Forms;

namespace System.Reactive.Linq
{
    /// <summary>
    /// Obsolete. The <c>System.Reactive.For.WindowsForms</c> NuGet package defines a
    /// <c>WindowsFormsControlObservable</c> class that defines new extension methods to be used in
    /// place of these (also in the <c>System.Reactive.Linq</c> namespace).
    /// </summary>
    /// <remarks>
    /// <para>
    /// The replacement <c>WindowsFormsControlObservable</c> class uses different names for extension
    /// methods. When you migrate to that new class from this obsolete one, you will need to change
    /// your code to invoke different method names:
    /// </para>
    /// <list type="table">
    ///     <listheader><term>Rx &lt;= 6.0</term><term>Now</term></listheader>
    ///     <item>
    ///         <term><c>ObserveOn</c></term>
    ///         <term><c>ObserveOnWindowsFormsControl</c></term>
    ///     </item>
    ///     <item>
    ///         <term><c>SubscribeOn</c></term>
    ///         <term><c>SubscribeOnWindowFormsControl</c></term>
    ///     </item>
    /// </list>
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
    [Obsolete("Use the extension methods defined by the System.Reactive.Linq.WindowsFormsControlObservable class in the System.Reactive.For.WindowsForms package instead", error: false)]

    public static class ControlObservable
    {
        /// <summary>
        /// Obsolete. Use the <c>SubscribeOnWindowFormsControl</c> extension method defined by
        /// <c>System.Reactive.Linq.WindowsFormsControlObservable</c> in the
        /// <c>System.Reactive.For.WindowsForms</c> package instead.
        /// </summary>
        /// <typeparam name="TSource">The type of the elements in the source sequence.</typeparam>
        /// <param name="source">Source sequence.</param>
        /// <param name="control">Windows Forms control whose associated message loop is used to perform subscription and unsubscription actions on.</param>
        /// <returns>The source sequence whose subscriptions and unsubscriptions happen on the Windows Forms message loop associated with the specified control.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="source"/> or <paramref name="control"/> is null.</exception>
        /// <remarks>
        /// Only the side-effects of subscribing to the source sequence and disposing subscriptions to the source sequence are run on the specified control.
        /// In order to invoke observer callbacks on the specified control, e.g. to render results in a control, use <see cref="ObserveOn"/>.
        /// </remarks>
        [Obsolete("Use the SubscribeOnWindowFormsControl extension method defined by System.Reactive.Linq.WindowsFormsControlObservable in the System.Reactive.For.WindowsForms package instead", error: false)]
        public static IObservable<TSource> SubscribeOn<TSource>(this IObservable<TSource> source, Control control)
        {
            if (source == null)
            {
                throw new ArgumentNullException(nameof(source));
            }

            if (control == null)
            {
                throw new ArgumentNullException(nameof(control));
            }

            return Synchronization.SubscribeOn(source, new ControlScheduler(control));
        }

        /// <summary>
        /// Obsolete. Use the <c>ObserveOnWindowsFormsControl</c> extension method defined by
        /// <c>System.Reactive.Linq.WindowsFormsControlObservable</c> in the
        /// <c>System.Reactive.For.WindowsForms</c> package instead.
        /// </summary>
        /// <typeparam name="TSource">The type of the elements in the source sequence.</typeparam>
        /// <param name="source">Source sequence.</param>
        /// <param name="control">Windows Forms control whose associated message loop is used to notify observers on.</param>
        /// <returns>The source sequence whose observations happen on the Windows Forms message loop associated with the specified control.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="source"/> or <paramref name="control"/> is null.</exception>
        [Obsolete("Use the ObserveOnWindowsFormsControl extension method defined by System.Reactive.Linq.WindowsFormsControlObservable in the System.Reactive.For.WindowsForms package instead", error: false)]
        public static IObservable<TSource> ObserveOn<TSource>(this IObservable<TSource> source, Control control)
        {
            if (source == null)
            {
                throw new ArgumentNullException(nameof(source));
            }

            if (control == null)
            {
                throw new ArgumentNullException(nameof(control));
            }

            return Synchronization.ObserveOn(source, new ControlScheduler(control));
        }
    }
}

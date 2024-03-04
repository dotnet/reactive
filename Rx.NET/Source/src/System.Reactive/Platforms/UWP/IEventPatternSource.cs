// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information. 

#if HAS_WINRT
using Windows.Foundation;

namespace System.Reactive
{
    /// <summary>
    /// Obsolete. The <c>System.Reactive.For.WindowsRuntime</c> NuGet package defines an
    /// <c>ITypedEventPatternSource</c> interface that should be used instead.
    /// </summary>
    /// <typeparam name="TSender">Sender type.</typeparam>
    /// <typeparam name="TEventArgs">Event arguments type.</typeparam>
    /// <remarks>
    /// <para>
    /// This type is specific to Windows Runtime, because the <see cref="OnNext"/> member uses the
    /// <see cref="TypedEventHandler{TSender, TResult}"/> type. This interface has therefore been
    /// marked as Obsolete as part of a drive to remove all UI-framework-specific and platform-
    /// specific types and members from the main <c>System.Reactive</c> package.
    /// </para>
    /// <para>
    /// The replacement type is not just in a different package and namespace. Its name has been
    /// changed to <c>ITypedEventPatternSource</c>. There are two reasons.
    /// </para>
    /// <para>
    /// First, the choice of name and namespace implied, wrongly, that this was a general purpose
    /// type like <see cref="IEventPattern{TSender, TEventArgs}"/> or
    /// <see cref="IEventPatternSource{TEventArgs}"/>. It is not, because it can't exist in TFMs
    /// where Windows Runtime is unavailable, such as <c>net8.0</c> or <c>netstandard2.0</c>. It
    /// was especially confusing to give this the same name as the single-type-argument
    /// <see cref="IEventPatternSource{TEventArgs}"/>, which has no such restriction. By differing
    /// only in the number of type arguments, these types strongly implied that they were just
    /// different forms of the same thing, when one was in fact only possible to use on certain
    /// Windows targets.
    /// </para>
    /// <para>
    /// Second, the fact that this was placed in the <c>System.Reactive</c> namespace means that
    /// simply adding a replacement type with the same simple name in a different namespace is
    /// likely to cause problems, because code may well have <c>System.Reactive</c> in scope for
    /// other reasons. This could make it difficult for code to move cleanly onto the new
    /// definition for as long as the old one remains present in <c>Obsolete</c> form, because it
    /// might not always be possible to replace <c>using System.Reactive;</c> with, say,
    /// <c>using System.Reactive.WindowsRuntime;</c>
    /// </para>
    /// <para>
    /// The main reason for pushing such code out into separate packages is to avoid a problem in
    /// which applications with a Windows-specific TFM end up acquiring dependencies on WPF and
    /// Windows Forms when they take a dependency on <c>System.Reactive</c> regardless of whether
    /// they are actually using those UI frameworks. In principle, we don't need to remove this
    /// particular interface because <see cref="TypedEventHandler{TSender, TResult}"/> is not
    /// technically specific to one framework. (It gets used in both UWP and WinUI. And since you
    /// can use WinRT APIs in Windows Forms, WPF, and even Console applications, technically those
    /// can use it too, although in most cases they wouldn't.) However, leaving Windows Runtime
    /// APIs in the main <c>System.Reactive</c> component would mean we would still need to produce
    /// both <c>netX.0</c> and <c>netX.0-windows10.0.YYYYY</c> TFMs even after we finally remove
    /// all of the Obsolete types and members. People regularly get confused by the fact that in
    /// the Rx v >=6.0 design, to get WPF or Windows Forms support today you need to use a suitably
    /// recent Windows-specific TFM. If we retained that TFM but removed the UI framework support
    /// from it leaving just the non-UI-framework-specific WinRT support) it seems likely that we
    /// would cause even more confusion. So we want to get <c>System.Reactive</c> to a state where,
    /// once we've removed all Obsolete types andmembers, we will need only non-OS-specific TFMs.
    /// We are therefore moving all Windows Runtime/Foundation support out into a separate package
    /// as well 
    /// </para>
    /// </remarks>
    [CLSCompliant(false)]
    [Obsolete("Use the System.Reactive.WindowsRuntime.ITypedEventPatternSource interface in the System.Reactive.For.WindowsRuntime package instead", error: false)]
    public interface IEventPatternSource<TSender, TEventArgs>
    {
        /// <summary>
        /// Event signaling the next element in the data stream.
        /// </summary>
#pragma warning disable CA1003 // (Use generic EventHandler.) The use of the Windows.Foundation handler type is by design
        event TypedEventHandler<TSender, TEventArgs> OnNext;
#pragma warning restore CA1003
    }
}
#endif

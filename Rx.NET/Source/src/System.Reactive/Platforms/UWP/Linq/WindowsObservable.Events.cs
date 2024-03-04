// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information. 

#if HAS_WINRT
using Windows.Foundation;

namespace System.Reactive.Linq
{
    /// <summary>
    /// Obsolete. The <c>System.Reactive.For.WindowsRuntime</c> NuGet package defines a
    /// <c>WindowsRuntimeObservable</c> class (also in the <c>System.Reactive.Linq</c> namespace)
    /// to use instead.
    /// </summary>
    /// <remarks>
    /// <para>
    /// The replacement <c>WindowsRuntimeObservable</c> class uses different names for extension
    /// methods. When you migrate to that new class from this obsolete one, you will need to change
    /// your code to invoke different method names:
    /// </para>
    /// <list type="table">
    ///     <listheader><term>Rx &lt;= 6.0</term><term>Now</term></listheader>
    ///     <item>
    ///         <term><c>ToEventPattern</c></term>
    ///         <term><c>ToWindowsFoundationEventPattern</c></term>
    ///     </item>
    ///     <item>
    ///         <term><c>SelectMany</c></term>
    ///         <term><c>SelectManyIAsyncOperation</c> or <c>SelectManyIAsyncOperationWithProgress</c></term>
    ///     </item>
    /// </list>
    /// <para>
    /// (Note that the <c>FromEventPattern</c> method name has not changed.)
    /// </para>
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
    /// The <c>FromEventPattern</c> methods keep the same name because those are not extension
    /// methods, so they are always invoked through their class name. The replacement class has a
    /// different name, <c>WindowsRuntimeObservable</c>, so there is no ambiguity.
    /// </para>
    /// <para>
    /// This type will eventually be removed because all UI-framework-specific functionality is
    /// being removed from <c>System.Reactive</c>. This is necessary to fix problems in which
    /// <c>System.Reactive</c> causes applications to end up with dependencies on Windows Forms and
    /// WPF whether they want them or not. Strictly speaking, the <see cref="IAsyncAction"/>,
    /// <see cref="IAsyncActionWithProgress{TProgress}"/>, <see cref="IAsyncOperation{TResult}"/>
    /// and <see cref="IAsyncOperationWithProgress{TResult, TProgress}"/> types that this class
    /// supports are part of Windows Runtime, and aren't specific to a single UI framework. These
    /// types are used routinely in both UWP and WinUI applications, but it's also possible to use
    /// them from Windows Forms, WPF, and even console applications. These types are effectively
    /// WinRT native way of representing what the TPL's various Task types represent in a purely
    /// .NET world. Even so, once support for genuinely UI-framework-specific types (such as
    /// WPF's <c>Dispatcher</c>) has been removed from Rx.NET, support for these Windows Runtime
    /// types would require us to continue to offer <c>netX.0</c> and <c>netX.0-windows10.0.YYYYY</c>
    /// TFMs. The fact that we offer both has caused confusion because it's quite possible to get the
    /// former even when running on Windows.
    /// </para>
       /// </remarks>
    [Obsolete("Use the System.Reactive.Linq.WindowsRuntimeObservable class in the System.Reactive.For.WindowsRuntime instead", error: false)]
    [CLSCompliant(false)]
    public static partial class WindowsObservable
    {
        /// <summary>
        /// Converts a typed event, conforming to the standard event pattern, to an observable sequence.
        /// </summary>
        /// <typeparam name="TSender">The type of the sender that raises the event.</typeparam>
        /// <typeparam name="TResult">The type of the event data generated by the event.</typeparam>
        /// <param name="addHandler">Action that attaches the given event handler to the underlying .NET event.</param>
        /// <param name="removeHandler">Action that detaches the given event handler from the underlying .NET event.</param>
        /// <returns>The observable sequence that contains data representations of invocations of the underlying typed event.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="addHandler"/> or <paramref name="removeHandler"/> is null.</exception>
        /// <seealso cref="ToEventPattern"/>
        [Obsolete("Use the FromEventPattern method defined by System.Reactive.Linq.WindowsRuntimeObservable in the System.Reactive.For.WindowsRuntime package instead", error: false)]
        public static IObservable<EventPattern<TSender, TResult>> FromEventPattern<TSender, TResult>(Action<TypedEventHandler<TSender, TResult>> addHandler, Action<TypedEventHandler<TSender, TResult>> removeHandler)
        {
            if (addHandler == null)
            {
                throw new ArgumentNullException(nameof(addHandler));
            }

            if (removeHandler == null)
            {
                throw new ArgumentNullException(nameof(removeHandler));
            }

            return Observable.Create<EventPattern<TSender, TResult>>(observer =>
            {
                var h = new TypedEventHandler<TSender, TResult>((sender, args) =>
                {
                    observer.OnNext(new EventPattern<TSender, TResult>(sender, args));
                });

                addHandler(h);

                return () =>
                {
                    removeHandler(h);
                };
            });
        }

        /// <summary>
        /// Converts a typed event, conforming to the standard event pattern, to an observable sequence.
        /// </summary>
        /// <typeparam name="TDelegate">The delegate type of the event to be converted.</typeparam>
        /// <typeparam name="TSender">The type of the sender that raises the event.</typeparam>
        /// <typeparam name="TResult">The type of the event data generated by the event.</typeparam>
        /// <param name="conversion">A function used to convert the given event handler to a delegate compatible with the underlying typed event. The resulting delegate is used in calls to the addHandler and removeHandler action parameters.</param>
        /// <param name="addHandler">Action that attaches the given event handler to the underlying .NET event.</param>
        /// <param name="removeHandler">Action that detaches the given event handler from the underlying .NET event.</param>
        /// <returns>The observable sequence that contains data representations of invocations of the underlying typed event.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="conversion"/> or <paramref name="addHandler"/> or <paramref name="removeHandler"/> is null.</exception>
        /// <seealso cref="ToEventPattern"/>
        [Obsolete("Use the FromEventPattern method defined by System.Reactive.Linq.WindowsRuntimeObservable in the System.Reactive.For.WindowsRuntime package instead", error: false)]
        public static IObservable<EventPattern<TSender, TResult>> FromEventPattern<TDelegate, TSender, TResult>(Func<TypedEventHandler<TSender, TResult>, TDelegate> conversion, Action<TDelegate> addHandler, Action<TDelegate> removeHandler)
        {
            if (conversion == null)
            {
                throw new ArgumentNullException(nameof(conversion));
            }

            if (addHandler == null)
            {
                throw new ArgumentNullException(nameof(addHandler));
            }

            if (removeHandler == null)
            {
                throw new ArgumentNullException(nameof(removeHandler));
            }

            return Observable.Create<EventPattern<TSender, TResult>>(observer =>
            {
                var h = conversion(new TypedEventHandler<TSender, TResult>((sender, args) =>
                {
                    observer.OnNext(new EventPattern<TSender, TResult>(sender, args));
                }));

                addHandler(h);

                return () =>
                {
                    removeHandler(h);
                };
            });
        }

        /// <summary>
        /// Exposes an observable sequence as an object with a typed event.
        /// </summary>
        /// <typeparam name="TSender">The type of the sender that raises the event.</typeparam>
        /// <typeparam name="TEventArgs">The type of the event data generated by the event.</typeparam>
        /// <param name="source">Observable source sequence.</param>
        /// <returns>The event source object.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="source"/> is null.</exception>
        [Obsolete("Use the ToWindowsFoundationEventPattern extension method defined by System.Reactive.Linq.WindowsRuntimeObservable in the System.Reactive.For.WindowsRuntime package instead", error: false)]
        public static IEventPatternSource<TSender, TEventArgs> ToEventPattern<TSender, TEventArgs>(this IObservable<EventPattern<TSender, TEventArgs>> source)
        {
            if (source == null)
            {
                throw new ArgumentNullException(nameof(source));
            }

            return new EventPatternSource<TSender, TEventArgs>(source, static (h, evt) => h(evt.Sender!, evt.EventArgs));
        }
    }
}
#endif

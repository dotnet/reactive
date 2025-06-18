// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information. 

#if HAS_WINRT
extern alias SystemReactiveNet;

using SystemReactiveNet::System;
using SystemReactiveNet::System.Reactive;
using SystemReactiveNet::System.Reactive.Linq;
using SystemReactiveNet::System.Reactive.Threading.Tasks;

using System.Runtime.InteropServices.WindowsRuntime;
using System.Threading.Tasks;
using Windows.Foundation;

namespace System.Reactive.Linq
{
    /// <summary>
    /// Obsolete. The <c>System.Reactive.For.WindowsRuntime</c> NuGet package defines a
    /// <c>WindowsRuntimeAsyncInfoObservable</c> class that defines new extension methods to be used in
    /// place of these (also in the <c>System.Reactive.Linq</c> namespace).
    /// </summary>
    /// <remarks>
    /// <para>
    /// The replacement <c>WindowsRuntimeAsyncInfoObservable</c> class uses different method names.
    /// When you migrate to that new class from this obsolete one, you will need to change your code
    /// to invoke different method names:
    /// </para>
    /// <list type="table">
    ///     <listheader><term>Rx &lt;= 6.0</term><term>Now</term></listheader>
    ///     <item>
    ///         <term><c>ToAsyncAction</c></term>
    ///         <term><c>ToIAsyncAction</c></term>
    ///     </item>
    ///     <item>
    ///         <term><c>ToAsyncActionWithProgress</c></term>
    ///         <term><c>ToIAsyncActionWithProgress</c></term>
    ///     </item>
    ///     <item>
    ///         <term><c>ToAsyncOperation</c></term>
    ///         <term><c>ToIAsyncOperation</c></term>
    ///     </item>
    ///     <item>
    ///         <term><c>ToAsyncOperationWithProgress</c></term>
    ///         <term><c>ToIAsyncOperationWithProgress</c></term>
    ///     </item>
    /// </list>
    /// <para>
    /// This name change is necessary because of a limitation of the <c>Obsolete</c> attribute: if
    /// you want to move an existing method into a different package, and you leave the old one in
    /// place (and marked as <c>Obsolete</c>) for a few versions to enable a gradual transition to
    /// the new one, you will cause a problem if you keep the method name the same. The problem
    /// is that both the old and new extension methods will be in scope simultaneously, so the
    /// compiler will complain of ambiguity when you try to use them. In some cases you can
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
    [Obsolete("Use the extension methods defined by the System.Reactive.Linq.WindowsRuntimeAsyncInfoObservable class in the System.Reactive.For.WindowsRuntime package instead", error: false)]
    [CLSCompliant(false)]
    public static class AsyncInfoObservable
    {
        #region IAsyncAction

        /// <summary>
        /// Obsolete. Use the <c>WindowsRuntimeAsyncInfoObservable.ToIAsyncAction</c> method in the
        /// <c>System.Reactive.For.WindowsRuntime</c> package instead.
        /// </summary>
        /// <typeparam name="TSource">The type of the elements in the source sequence.</typeparam>
        /// <param name="source">Source sequence to expose as an asynchronous action.</param>
        /// <returns>Windows Runtime asynchronous action object representing the completion of the observable sequence.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="source"/> is null.</exception>
        [Obsolete("Use the ToIAsyncAction extension method defined by System.Reactive.Linq.WindowsRuntimeAsyncInfoObservable in the System.Reactive.For.WindowsRuntime package instead", error: false)]
        public static IAsyncAction ToAsyncAction<TSource>(this IObservable<TSource> source)
        {
            if (source == null)
            {
                throw new ArgumentNullException(nameof(source));
            }

            return AsyncInfo.Run(ct => (Task)source.DefaultIfEmpty().ToTask(ct));
        }

        #region Progress

        /// <summary>
        /// Obsolete. Use the <c>WindowsRuntimeAsyncInfoObservable.ToIAsyncActionWithProgress</c>
        /// method in the <c>System.Reactive.For.WindowsRuntime</c> package instead.
        /// </summary>
        /// <typeparam name="TSource">The type of the elements in the source sequence.</typeparam>
        /// <param name="source">Source sequence to expose as an asynchronous action.</param>
        /// <returns>Windows Runtime asynchronous action object representing the completion of the observable sequence, reporting incremental progress for each source sequence element.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="source"/> is null.</exception>
        [Obsolete("Use the ToIAsyncActionWithProgress extension method defined by System.Reactive.Linq.WindowsRuntimeAsyncInfoObservable in the System.Reactive.For.WindowsRuntime package instead", error: false)]
        public static IAsyncActionWithProgress<int> ToAsyncActionWithProgress<TSource>(this IObservable<TSource> source)
        {
            if (source == null)
            {
                throw new ArgumentNullException(nameof(source));
            }

            return AsyncInfo.Run<int>((ct, progress) =>
            {
                var i = 0;
                return source.Do(_ => progress.Report(i++)).DefaultIfEmpty().ToTask(ct);
            });
        }

        /// <summary>
        /// Obsolete. Use the <c>WindowsRuntimeAsyncInfoObservable.ToIAsyncActionWithProgress</c>
        /// method in the <c>System.Reactive.For.WindowsRuntime</c> package instead.
        /// </summary>
        /// <typeparam name="TSource">The type of the elements in the source sequence.</typeparam>
        /// <typeparam name="TProgress">The type of the elements in the progress sequence.</typeparam>
        /// <param name="source">Source sequence to expose as an asynchronous action and to compute a progress sequence that gets reported through the asynchronous action.</param>
        /// <param name="progressSelector">Selector function to map the source sequence on a progress reporting sequence.</param>
        /// <returns>Windows Runtime asynchronous action object representing the completion of the result sequence, reporting progress computed through the progress sequence.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="source"/> or <paramref name="progressSelector"/> is null.</exception>
        [Obsolete("Use the ToIAsyncActionWithProgress extension method defined by System.Reactive.Linq.WindowsRuntimeAsyncInfoObservable in the System.Reactive.For.WindowsRuntime package instead", error: false)]
        public static IAsyncActionWithProgress<TProgress> ToAsyncActionWithProgress<TSource, TProgress>(this IObservable<TSource> source, Func<IObservable<TSource>, IObservable<TProgress>> progressSelector)
        {
            if (source == null)
            {
                throw new ArgumentNullException(nameof(source));
            }

            if (progressSelector == null)
            {
                throw new ArgumentNullException(nameof(progressSelector));
            }

            return AsyncInfo.Run<TProgress>((ct, progress) =>
            {
                return Observable.Create<TSource?>(observer =>
                {
                    var obs = Observer.Synchronize(observer);

                    var data = source.Publish();

                    var progressSubscription = progressSelector(data).Subscribe(progress.Report, obs.OnError);
                    var dataSubscription = data.DefaultIfEmpty().Subscribe(obs);
                    var connection = data.Connect();

                    return StableCompositeDisposable.CreateTrusted(progressSubscription, dataSubscription, connection);
                }).ToTask(ct);
            });
        }

        #endregion

        #endregion

        #region IAsyncOperation<T>

        /// <summary>
        /// Obsolete. Use the <c>WindowsRuntimeAsyncInfoObservable.ToAsyncOperation</c> method in
        /// the <c>System.Reactive.For.WindowsRuntime</c> package instead.
        /// </summary>
        /// <typeparam name="TSource">The type of the elements in the source sequence.</typeparam>
        /// <param name="source">Source sequence to expose as an asynchronous operation.</param>
        /// <returns>Windows Runtime asynchronous operation object that returns the last element of the observable sequence.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="source"/> is null.</exception>
        [Obsolete("Use the ToIAsyncOperation extension method defined by System.Reactive.Linq.WindowsRuntimeAsyncInfoObservable in the System.Reactive.For.WindowsRuntime package instead", error: false)]
        public static IAsyncOperation<TSource> ToAsyncOperation<TSource>(this IObservable<TSource> source)
        {
            if (source == null)
            {
                throw new ArgumentNullException(nameof(source));
            }

            return AsyncInfo.Run(ct => source.ToTask(ct));
        }

        /// <summary>
        /// Obsolete. Use the <c>WindowsRuntimeAsyncInfoObservable.ToIAsyncOperationWithProgress</c>
        /// method in the <c>System.Reactive.For.WindowsRuntime</c> package instead.
        /// </summary>
        /// <typeparam name="TSource">The type of the elements in the source sequence.</typeparam>
        /// <param name="source">Source sequence to expose as an asynchronous operation.</param>
        /// <returns>Windows Runtime asynchronous operation object that returns the last element of the observable sequence, reporting incremental progress for each source sequence element.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="source"/> is null.</exception>
        [Obsolete("Use the ToIAsyncOperationWithProgress extension method defined by System.Reactive.Linq.WindowsRuntimeAsyncInfoObservable in the System.Reactive.For.WindowsRuntime package instead", error: false)]
        public static IAsyncOperationWithProgress<TSource, int> ToAsyncOperationWithProgress<TSource>(this IObservable<TSource> source)
        {
            if (source == null)
            {
                throw new ArgumentNullException(nameof(source));
            }

            return AsyncInfo.Run<TSource, int>((ct, progress) =>
            {
                var i = 0;
                return source.Do(_ => progress.Report(i++)).ToTask(ct);
            });
        }

        #region Progress

        /// <summary>
        /// Obsolete. Use the <c>WindowsRuntimeAsyncInfoObservable.ToIAsyncOperationWithProgress</c>
        /// method in the <c>System.Reactive.For.WindowsRuntime</c> package instead.
        /// </summary>
        /// <typeparam name="TSource">The type of the elements in the source sequence.</typeparam>
        /// <typeparam name="TResult">The type of the elements in the result sequence.</typeparam>
        /// <param name="source">Source sequence to compute a result sequence that gets exposed as an asynchronous operation.</param>
        /// <param name="resultSelector">Selector function to map the source sequence on a result sequence.</param>
        /// <returns>Windows Runtime asynchronous operation object that returns the last element of the result sequence, reporting incremental progress for each source sequence element.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="source"/> or <paramref name="resultSelector"/> is null.</exception>
        [Obsolete("Use the ToIAsyncOperationWithProgress extension method defined by System.Reactive.Linq.WindowsRuntimeAsyncInfoObservable in the System.Reactive.For.WindowsRuntime package instead", error: false)]
        public static IAsyncOperationWithProgress<TResult, int> ToAsyncOperationWithProgress<TSource, TResult>(this IObservable<TSource> source, Func<IObservable<TSource>, IObservable<TResult>> resultSelector)
        {
            if (source == null)
            {
                throw new ArgumentNullException(nameof(source));
            }

            if (resultSelector == null)
            {
                throw new ArgumentNullException(nameof(resultSelector));
            }

            return AsyncInfo.Run<TResult, int>((ct, progress) =>
            {
                var i = 0;
                return resultSelector(source.Do(_ => progress.Report(i++))).ToTask(ct);
            });
        }

        /// <summary>
        /// Obsolete. Use the <c>WindowsRuntimeAsyncInfoObservable.ToIAsyncOperationWithProgress</c>
        /// method in the <c>System.Reactive.For.WindowsRuntime</c> package instead.
        /// </summary>
        /// <typeparam name="TSource">The type of the elements in the source sequence.</typeparam>
        /// <typeparam name="TResult">The type of the elements in the result sequence.</typeparam>
        /// <typeparam name="TProgress">The type of the elements in the progress sequence.</typeparam>
        /// <param name="source">Source sequence to compute a result sequence that gets exposed as an asynchronous operation and a progress sequence that gets reported through the asynchronous operation.</param>
        /// <param name="resultSelector">Selector function to map the source sequence on a result sequence.</param>
        /// <param name="progressSelector">Selector function to map the source sequence on a progress reporting sequence.</param>
        /// <returns>Windows Runtime asynchronous operation object that returns the last element of the result sequence, reporting progress computed through the progress sequence.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="source"/> or <paramref name="resultSelector"/> or <paramref name="progressSelector"/> is null.</exception>
        [Obsolete("Use the ToIAsyncOperationWithProgress extension method defined by System.Reactive.Linq.WindowsRuntimeAsyncInfoObservable in the System.Reactive.For.WindowsRuntime package instead", error: false)]
        public static IAsyncOperationWithProgress<TResult, TProgress> ToAsyncOperationWithProgress<TSource, TResult, TProgress>(this IObservable<TSource> source, Func<IObservable<TSource>, IObservable<TResult>> resultSelector, Func<IObservable<TSource>, IObservable<TProgress>> progressSelector)
        {
            if (source == null)
            {
                throw new ArgumentNullException(nameof(source));
            }

            if (resultSelector == null)
            {
                throw new ArgumentNullException(nameof(resultSelector));
            }

            if (progressSelector == null)
            {
                throw new ArgumentNullException(nameof(progressSelector));
            }

            return AsyncInfo.Run<TResult, TProgress>((ct, progress) =>
            {
                return Observable.Create<TResult>(observer =>
                {
                    var obs = Observer.Synchronize(observer);

                    var data = source.Publish();

                    var progressSubscription = progressSelector(data).Subscribe(progress.Report, obs.OnError);
                    var dataSubscription = resultSelector(data).Subscribe(obs);
                    var connection = data.Connect();

                    return StableCompositeDisposable.CreateTrusted(progressSubscription, dataSubscription, connection);
                }).ToTask(ct);
            });
        }

        #endregion

        #endregion
    }
}
#endif

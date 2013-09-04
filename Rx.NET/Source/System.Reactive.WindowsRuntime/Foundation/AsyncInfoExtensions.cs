// Copyright (c) Microsoft Open Technologies, Inc. All rights reserved. See License.txt in the project root for license information.

#if HAS_WINRT
using System.Reactive.Disposables;
using System.Reactive.Linq;
using System.Reactive.Subjects;
using System.Threading;
using Windows.Foundation;

namespace System.Reactive.Windows.Foundation
{
    /// <summary>
    /// Provides conversions from Windows Runtime asynchronous actions and operations to observable sequences.
    /// </summary>
    public static class AsyncInfoObservableExtensions
    {
        #region IAsyncAction and IAsyncActionWithProgress

        /// <summary>
        /// Converts a Windows Runtime asynchronous action to an observable sequence.
        /// Each observer subscribed to the resulting observable sequence will be notified about the action's successful or exceptional completion.
        /// </summary>
        /// <param name="source">Asynchronous action to convert.</param>
        /// <returns>An observable sequence that produces a unit value when the asynchronous action completes, or propagates the exception produced by the asynchronous action.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="source"/> is null.</exception>
        public static IObservable<Unit> ToObservable(this IAsyncAction source)
        {
            if (source == null)
                throw new ArgumentNullException("source");

            return new AsyncInfoToObservableBridge<Unit, Unit>(
                source,
                (iai, a) => ((IAsyncAction)iai).Completed += new AsyncActionCompletedHandler((iaa, status) => a(iaa, status)),
                iai => Unit.Default,
                onProgress: null,
                progress: null,
                multiValue: false
            );
        }

        /// <summary>
        /// Converts a Windows Runtime asynchronous action to an observable sequence, ignoring its progress notifications.
        /// Each observer subscribed to the resulting observable sequence will be notified about the action's successful or exceptional completion.
        /// </summary>
        /// <typeparam name="TProgress">The type of the reported progress objects, which get ignored by this conversion.</typeparam>
        /// <param name="source">Asynchronous action to convert.</param>
        /// <returns>An observable sequence that produces a unit value when the asynchronous action completes, or propagates the exception produced by the asynchronous action.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="source"/> is null.</exception>
        public static IObservable<Unit> ToObservable<TProgress>(this IAsyncActionWithProgress<TProgress> source)
        {
            if (source == null)
                throw new ArgumentNullException("source");

            return source.ToObservable_(null);
        }

        /// <summary>
        /// Converts a Windows Runtime asynchronous action to an observable sequence, reporting its progress through the supplied progress object.
        /// Each observer subscribed to the resulting observable sequence will be notified about the action's successful or exceptional completion.
        /// </summary>
        /// <typeparam name="TProgress">The type of the reported progress objects.</typeparam>
        /// <param name="source">Asynchronous action to convert.</param>
        /// <param name="progress">Progress object to receive progress notifications on.</param>
        /// <returns>An observable sequence that produces a unit value when the asynchronous action completes, or propagates the exception produced by the asynchronous action.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="source"/> or <paramref name="progress"/> is null.</exception>
        public static IObservable<Unit> ToObservable<TProgress>(this IAsyncActionWithProgress<TProgress> source, IProgress<TProgress> progress)
        {
            if (source == null)
                throw new ArgumentNullException("source");
            if (progress == null)
                throw new ArgumentNullException("progress");

            return source.ToObservable_(progress);
        }

        /// <summary>
        /// Converts a Windows Runtime asynchronous action to an observable sequence reporting its progress.
        /// Each observer subscribed to the resulting observable sequence will be notified about the action's succesful or exceptional completion.
        /// </summary>
        /// <typeparam name="TProgress">The type of the reported progress objects.</typeparam>
        /// <param name="source">Asynchronous action to convert.</param>
        /// <returns>An observable sequence that produces progress values from the asynchronous action and notifies observers about the action's completion.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="source"/> is null.</exception>
        public static IObservable<TProgress> ToObservableProgress<TProgress>(this IAsyncActionWithProgress<TProgress> source)
        {
            if (source == null)
                throw new ArgumentNullException("source");

            return Observable.Create<TProgress>(observer =>
            {
                var progress = observer.ToProgress();
                var src = source.ToObservable_(progress);
                return src.Subscribe(_ => { }, observer.OnError, observer.OnCompleted);
            });
        }

        private static IObservable<Unit> ToObservable_<TProgress>(this IAsyncActionWithProgress<TProgress> source, IProgress<TProgress> progress)
        {
            return new AsyncInfoToObservableBridge<Unit, TProgress>(
                source,
                (iai, a) => ((IAsyncActionWithProgress<TProgress>)iai).Completed += new AsyncActionWithProgressCompletedHandler<TProgress>((iaa, status) => a(iaa, status)),
                iai => Unit.Default,
                (iai, a) => ((IAsyncActionWithProgress<TProgress>)iai).Progress += new AsyncActionProgressHandler<TProgress>((iap, p) => a(iap, p)),
                progress,
                multiValue: false
            );
        }

        #endregion

        #region IAsyncOperation and IAsyncOperationWithProgress

        /// <summary>
        /// Converts a Windows Runtime asynchronous operation to an observable sequence reporting its result.
        /// Each observer subscribed to the resulting observable sequence will be notified about the operation's single result and its successful exceptional completion.
        /// </summary>
        /// <typeparam name="TResult">The type of the asynchronous operation's result.</typeparam>
        /// <param name="source">Asynchronous operation to convert.</param>
        /// <returns>An observable sequence that notifies observers about the asynchronous operation's result value and completion.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="source"/> is null.</exception>
        public static IObservable<TResult> ToObservable<TResult>(this IAsyncOperation<TResult> source)
        {
            if (source == null)
                throw new ArgumentNullException("source");

            return new AsyncInfoToObservableBridge<TResult, Unit>(
                source,
                (iai, a) => ((IAsyncOperation<TResult>)iai).Completed += new AsyncOperationCompletedHandler<TResult>((iao, status) => a(iao, status)),
                iai => ((IAsyncOperation<TResult>)iai).GetResults(),
                onProgress: null,
                progress: null,
                multiValue: false
            );
        }

        /// <summary>
        /// Converts a Windows Runtime asynchronous operation to an observable sequence reporting its result but ignoring its progress notifications.
        /// Each observer subscribed to the resulting observable sequence will be notified about the operations's single result and its successful or exceptional completion.
        /// </summary>
        /// <typeparam name="TResult">The type of the asynchronous operation's result.</typeparam>
        /// <typeparam name="TProgress">The type of the reported progress objects, which get ignored by this conversion.</typeparam>
        /// <param name="source">Asynchronous action to convert.</param>
        /// <returns>An observable sequence that notifies observers about the asynchronous operation's result value and completion.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="source"/> is null.</exception>
        public static IObservable<TResult> ToObservable<TResult, TProgress>(this IAsyncOperationWithProgress<TResult, TProgress> source)
        {
            if (source == null)
                throw new ArgumentNullException("source");

            return source.ToObservable_(null, false);
        }

        /// <summary>
        /// Converts a Windows Runtime asynchronous operation to an observable sequence reporting its result and reporting its progress through the supplied progress object.
        /// Each observer subscribed to the resulting observable sequence will be notified about the operations's single result and its successful or exceptional completion.
        /// </summary>
        /// <typeparam name="TResult">The type of the asynchronous operation's result.</typeparam>
        /// <typeparam name="TProgress">The type of the reported progress objects.</typeparam>
        /// <param name="source">Asynchronous action to convert.</param>
        /// <param name="progress">Progress object to receive progress notifications on.</param>
        /// <returns>An observable sequence that notifies observers about the asynchronous operation's result value and completion.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="source"/> or <paramref name="progress"/> is null.</exception>
        public static IObservable<TResult> ToObservable<TResult, TProgress>(this IAsyncOperationWithProgress<TResult, TProgress> source, IProgress<TProgress> progress)
        {
            if (source == null)
                throw new ArgumentNullException("source");
            if (progress == null)
                throw new ArgumentNullException("progress");

            return source.ToObservable_(progress, false);
        }

        /// <summary>
        /// Converts a Windows Runtime asynchronous operation to an observable sequence reporting its progress but ignoring its result value.
        /// Each observer subscribed to the resulting observable sequence will be notified about the action's succesful or exceptional completion.
        /// </summary>
        /// <typeparam name="TResult">The type of the asynchronous operation's result, which gets ignored by this conversion.</typeparam>
        /// <typeparam name="TProgress">The type of the reported progress objects.</typeparam>
        /// <param name="source">Asynchronous action to convert.</param>
        /// <returns>An observable sequence that produces progress values from the asynchronous operatin and notifies observers about the operations's completion.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="source"/> is null.</exception>
        public static IObservable<TProgress> ToObservableProgress<TResult, TProgress>(this IAsyncOperationWithProgress<TResult, TProgress> source)
        {
            if (source == null)
                throw new ArgumentNullException("source");

            return Observable.Create<TProgress>(observer =>
            {
                var progress = observer.ToProgress();
                var src = source.ToObservable_(progress, false);
                return src.Subscribe(_ => { }, observer.OnError, observer.OnCompleted);
            });
        }

        /// <summary>
        /// Converts a Windows Runtime asynchronous operation to an observable sequence by retrieving the operation's results whenever progress is reported and when the operation completes.
        /// Each observer subscribed to the resulting observable sequence will be notified about the action's succesful or exceptional completion.
        /// </summary>
        /// <typeparam name="TResult">The type of the asynchronous operation's result.</typeparam>
        /// <typeparam name="TProgress">The type of the reported progress objects, which are used internally in the conversion but aren't exposed.</typeparam>
        /// <param name="source">Asynchronous operation to convert.</param>
        /// <returns>An observable sequence that notifies observers about the asynchronous operation's (incremental) result value(s) and completion.</returns>
        /// <remarks>This conversion can be used with Windows Runtime APIs that support incremental retrieval of results during an asynchronous operation's execution.</remarks>
        /// <exception cref="ArgumentNullException"><paramref name="source"/> is null.</exception>
        public static IObservable<TResult> ToObservableMultiple<TResult, TProgress>(this IAsyncOperationWithProgress<TResult, TProgress> source)
        {
            if (source == null)
                throw new ArgumentNullException("source");

            return source.ToObservable_(null, true);
        }

        /// <summary>
        /// Converts a Windows Runtime asynchronous operation to an observable sequence by retrieving the operation's results whenever progress is reported and when the operation completes. The operation's progress is reported through the supplied progress object.
        /// Each observer subscribed to the resulting observable sequence will be notified about the action's succesful or exceptional completion.
        /// </summary>
        /// <typeparam name="TResult">The type of the asynchronous operation's result.</typeparam>
        /// <typeparam name="TProgress">The type of the reported progress objects.</typeparam>
        /// <param name="source">Asynchronous operation to convert.</param>
        /// <param name="progress">Progress object to receive progress notifications on.</param>
        /// <returns>An observable sequence that notifies observers about the asynchronous operation's (incremental) result value(s) and completion.</returns>
        /// <remarks>This conversion can be used with Windows Runtime APIs that support incremental retrieval of results during an asynchronous operation's execution.</remarks>
        /// <exception cref="ArgumentNullException"><paramref name="source"/> or <paramref name="progress"/> is null.</exception>
        public static IObservable<TResult> ToObservableMultiple<TResult, TProgress>(this IAsyncOperationWithProgress<TResult, TProgress> source, IProgress<TProgress> progress)
        {
            if (source == null)
                throw new ArgumentNullException("source");
            if (progress == null)
                throw new ArgumentNullException("progress");

            return source.ToObservable_(progress, true);
        }

        private static IObservable<TResult> ToObservable_<TResult, TProgress>(this IAsyncOperationWithProgress<TResult, TProgress> source, IProgress<TProgress> progress, bool supportsMultiple)
        {
            return new AsyncInfoToObservableBridge<TResult, TProgress>(
                source,
                (iai, a) => ((IAsyncOperationWithProgress<TResult, TProgress>)iai).Completed += new AsyncOperationWithProgressCompletedHandler<TResult, TProgress>((iao, status) => a(iao, status)),
                iai => ((IAsyncOperationWithProgress<TResult, TProgress>)iai).GetResults(),
                (iai, a) => ((IAsyncOperationWithProgress<TResult, TProgress>)iai).Progress += new AsyncOperationProgressHandler<TResult, TProgress>((iap, p) => a(iap, p)),
                progress,
                supportsMultiple
            );
        }

        #endregion
    }
}
#endif
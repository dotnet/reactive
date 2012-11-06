// Copyright (c) Microsoft Open Technologies, Inc. All rights reserved. See License.txt in the project root for license information.

#if HAS_WINRT
using System.Reactive.Disposables;
using System.Reactive.Linq;
using System.Reactive.Subjects;
using System.Reactive.Threading.Tasks;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Threading.Tasks;
using Windows.Foundation;

namespace System.Reactive.Linq
{
    /// <summary>
    /// Provides a set of extension methods to expose observable sequences as Windows Runtime asynchronous actions and operations.
    /// </summary>
    public static class AsyncInfoObservable
    {
        #region IAsyncAction

        /// <summary>
        /// Creates a Windows Runtime asynchronous action that represents the completion of the observable sequence.
        /// Upon cancellation of the asynchronous action, the subscription to the source sequence will be disposed.
        /// </summary>
        /// <typeparam name="TSource">The type of the elements in the source sequence.</typeparam>
        /// <param name="source">Source sequence to expose as an asynchronous action.</param>
        /// <returns>Windows Runtime asynchronous action object representing the completion of the observable sequence.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="source"/> is null.</exception>
        public static IAsyncAction ToAsyncAction<TSource>(this IObservable<TSource> source)
        {
            if (source == null)
                throw new ArgumentNullException("source");

            return AsyncInfo.Run(ct => (Task)source.DefaultIfEmpty().ToTask(ct));
        }

        #region Progress

        /// <summary>
        /// Creates a Windows Runtime asynchronous action that represents the completion of the observable sequence, reporting incremental progress for each element produced by the sequence.
        /// Upon cancellation of the asynchronous action, the subscription to the source sequence will be disposed.
        /// </summary>
        /// <typeparam name="TSource">The type of the elements in the source sequence.</typeparam>
        /// <param name="source">Source sequence to expose as an asynchronous action.</param>
        /// <returns>Windows Runtime asynchronous action object representing the completion of the observable sequence, reporting incremental progress for each source sequence element.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="source"/> is null.</exception>
        public static IAsyncActionWithProgress<int> ToAsyncActionWithProgress<TSource>(this IObservable<TSource> source)
        {
            if (source == null)
                throw new ArgumentNullException("source");

            return AsyncInfo.Run<int>((ct, progress) =>
            {
                var i = 0;
                return (Task)source.Do(_ => progress.Report(i++)).DefaultIfEmpty().ToTask(ct);
            });
        }

        /// <summary>
        /// Creates a Windows Runtime asynchronous action that represents the completion of the observable sequence, using a selector function to map the source sequence on a progress reporting sequence.
        /// Upon cancellation of the asynchronous action, the subscription to the source sequence will be disposed.
        /// </summary>
        /// <typeparam name="TSource">The type of the elements in the source sequence.</typeparam>
        /// <typeparam name="TProgress">The type of the elements in the progress sequence.</typeparam>
        /// <param name="source">Source sequence to expose as an asynchronous action and to compute a progress sequence that gets reported through the asynchronous action.</param>
        /// <param name="progressSelector">Selector function to map the source sequence on a progress reporting sequence.</param>
        /// <returns>Windows Runtime asynchronous action object representing the completion of the result sequence, reporting progress computed through the progress sequence.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="source"/> or <paramref name="progressSelector"/> is null.</exception>
        public static IAsyncActionWithProgress<TProgress> ToAsyncActionWithProgress<TSource, TProgress>(this IObservable<TSource> source, Func<IObservable<TSource>, IObservable<TProgress>> progressSelector)
        {
            if (source == null)
                throw new ArgumentNullException("source");
            if (progressSelector == null)
                throw new ArgumentNullException("progressSelector");

            return AsyncInfo.Run<TProgress>((ct, progress) =>
            {
                return (Task)Observable.Create<TSource>(observer =>
                {
                    var obs = Observer.Synchronize(observer);

                    var data = source.Publish();

                    var progressSubscription = progressSelector(data).Subscribe(progress.Report, obs.OnError);
                    var dataSubscription = data.DefaultIfEmpty().Subscribe(obs);
                    var connection = data.Connect();

                    return new CompositeDisposable(progressSubscription, dataSubscription, connection);
                }).ToTask(ct);
            });
        }

        #endregion

        #endregion

        #region IAsyncOperation<T>

        /// <summary>
        /// Creates a Windows Runtime asynchronous operation that returns the last element of the observable sequence.
        /// Upon cancellation of the asynchronous operation, the subscription to the source sequence will be disposed.
        /// </summary>
        /// <typeparam name="TSource">The type of the elements in the source sequence.</typeparam>
        /// <param name="source">Source sequence to expose as an asynchronous operation.</param>
        /// <returns>Windows Runtime asynchronous operation object that returns the last element of the observable sequence.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="source"/> is null.</exception>
        public static IAsyncOperation<TSource> ToAsyncOperation<TSource>(this IObservable<TSource> source)
        {
            if (source == null)
                throw new ArgumentNullException("source");

            return AsyncInfo.Run(ct => source.ToTask(ct));
        }

        /// <summary>
        /// Creates a Windows Runtime asynchronous operation that returns the last element of the observable sequence, reporting incremental progress for each element produced by the sequence.
        /// Upon cancellation of the asynchronous operation, the subscription to the source sequence will be disposed.
        /// </summary>
        /// <typeparam name="TSource">The type of the elements in the source sequence.</typeparam>
        /// <param name="source">Source sequence to expose as an asynchronous operation.</param>
        /// <returns>Windows Runtime asynchronous operation object that returns the last element of the observable sequence, reporting incremental progress for each source sequence element.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="source"/> is null.</exception>
        public static IAsyncOperationWithProgress<TSource, int> ToAsyncOperationWithProgress<TSource>(this IObservable<TSource> source)
        {
            if (source == null)
                throw new ArgumentNullException("source");

            return AsyncInfo.Run<TSource, int>((ct, progress) =>
            {
                var i = 0;
                return source.Do(_ => progress.Report(i++)).ToTask(ct);
            });
        }

        #region Progress

        /// <summary>
        /// Creates a Windows Runtime asynchronous operation that returns the last element of the result sequence, reporting incremental progress for each element produced by the source sequence.
        /// Upon cancellation of the asynchronous operation, the subscription to the source sequence will be disposed.
        /// </summary>
        /// <typeparam name="TSource">The type of the elements in the source sequence.</typeparam>
        /// <typeparam name="TResult">The type of the elements in the result sequence.</typeparam>
        /// <param name="source">Source sequence to compute a result sequence that gets exposed as an asynchronous operation.</param>
        /// <param name="resultSelector">Selector function to map the source sequence on a result sequence.</param>
        /// <returns>Windows Runtime asynchronous operation object that returns the last element of the result sequence, reporting incremental progress for each source sequence element.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="source"/> or <paramref name="resultSelector"/> is null.</exception>
        public static IAsyncOperationWithProgress<TResult, int> ToAsyncOperationWithProgress<TSource, TResult>(this IObservable<TSource> source, Func<IObservable<TSource>, IObservable<TResult>> resultSelector)
        {
            if (source == null)
                throw new ArgumentNullException("source");
            if (resultSelector == null)
                throw new ArgumentNullException("resultSelector");

            return AsyncInfo.Run<TResult, int>((ct, progress) =>
            {
                var i = 0;
                return resultSelector(source.Do(_ => progress.Report(i++))).ToTask(ct);
            });
        }

        /// <summary>
        /// Creates a Windows Runtime asynchronous operation that returns the last element of the result sequence, using a selector function to map the source sequence on a progress reporting sequence.
        /// Upon cancellation of the asynchronous operation, the subscription to the source sequence will be disposed.
        /// </summary>
        /// <typeparam name="TSource">The type of the elements in the source sequence.</typeparam>
        /// <typeparam name="TResult">The type of the elements in the result sequence.</typeparam>
        /// <typeparam name="TProgress">The type of the elements in the progress sequence.</typeparam>
        /// <param name="source">Source sequence to compute a result sequence that gets exposed as an asynchronous operation and a progress sequence that gets reported through the asynchronous operation.</param>
        /// <param name="resultSelector">Selector function to map the source sequence on a result sequence.</param>
        /// <param name="progressSelector">Selector function to map the source sequence on a progress reporting sequence.</param>
        /// <returns>Windows Runtime asynchronous operation object that returns the last element of the result sequence, reporting progress computed through the progress sequence.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="source"/> or <paramref name="resultSelector"/> or <paramref name="progressSelector"/> is null.</exception>
        public static IAsyncOperationWithProgress<TResult, TProgress> ToAsyncOperationWithProgress<TSource, TResult, TProgress>(this IObservable<TSource> source, Func<IObservable<TSource>, IObservable<TResult>> resultSelector, Func<IObservable<TSource>, IObservable<TProgress>> progressSelector)
        {
            if (source == null)
                throw new ArgumentNullException("source");
            if (resultSelector == null)
                throw new ArgumentNullException("resultSelector");
            if (progressSelector == null)
                throw new ArgumentNullException("progressSelector");

            return AsyncInfo.Run<TResult, TProgress>((ct, progress) =>
            {
                return Observable.Create<TResult>(observer =>
                {
                    var obs = Observer.Synchronize(observer);

                    var data = source.Publish();

                    var progressSubscription = progressSelector(data).Subscribe(progress.Report, obs.OnError);
                    var dataSubscription = resultSelector(data).Subscribe(obs);
                    var connection = data.Connect();

                    return new CompositeDisposable(progressSubscription, dataSubscription, connection);
                }).ToTask(ct);
            });
        }

        #endregion

        #endregion
    }
}
#endif
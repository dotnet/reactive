// Copyright (c) Microsoft Open Technologies, Inc. All rights reserved. See License.txt in the project root for license information.

#if HAS_AWAIT
using System.Threading;
using System.Reactive.Disposables;
using System.Reactive.Subjects;

namespace System.Reactive.Linq
{
    public static partial class Observable
    {
        /// <summary>
        /// Gets an awaiter that returns the last value of the observable sequence or throws an exception if the sequence is empty.
        /// This operation subscribes to the observable sequence, making it hot.
        /// </summary>
        /// <typeparam name="TSource">The type of the elements in the source sequence.</typeparam>
        /// <param name="source">Source sequence to await.</param>
        /// <returns>Object that can be awaited.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="source"/> is null.</exception>
        public static AsyncSubject<TSource> GetAwaiter<TSource>(this IObservable<TSource> source)
        {
            if (source == null)
                throw new ArgumentNullException("source");

            return s_impl.GetAwaiter<TSource>(source);
        }

        /// <summary>
        /// Gets an awaiter that returns the last value of the observable sequence or throws an exception if the sequence is empty.
        /// This operation subscribes and connects to the observable sequence, making it hot.
        /// </summary>
        /// <typeparam name="TSource">The type of the elements in the source sequence.</typeparam>
        /// <param name="source">Source sequence to await.</param>
        /// <returns>Object that can be awaited.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="source"/> is null.</exception>
        public static AsyncSubject<TSource> GetAwaiter<TSource>(this IConnectableObservable<TSource> source)
        {
            if (source == null)
                throw new ArgumentNullException("source");

            return s_impl.GetAwaiter<TSource>(source);
        }

        /// <summary>
        /// Gets an awaiter that returns the last value of the observable sequence or throws an exception if the sequence is empty.
        /// This operation subscribes to the observable sequence, making it hot. The supplied CancellationToken can be used to cancel the subscription.
        /// </summary>
        /// <typeparam name="TSource">The type of the elements in the source sequence.</typeparam>
        /// <param name="source">Source sequence to await.</param>
        /// <param name="cancellationToken">Cancellation token.</param>
        /// <returns>Object that can be awaited.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="source"/> is null.</exception>
        public static AsyncSubject<TSource> RunAsync<TSource>(this IObservable<TSource> source, CancellationToken cancellationToken)
        {
            if (source == null)
                throw new ArgumentNullException("source");

            return s_impl.RunAsync<TSource>(source, cancellationToken);
        }

        /// <summary>
        /// Gets an awaiter that returns the last value of the observable sequence or throws an exception if the sequence is empty.
        /// This operation subscribes and connects to the observable sequence, making it hot. The supplied CancellationToken can be used to cancel the subscription and connection.
        /// </summary>
        /// <typeparam name="TSource">The type of the elements in the source sequence.</typeparam>
        /// <param name="source">Source sequence to await.</param>
        /// <param name="cancellationToken">Cancellation token.</param>
        /// <returns>Object that can be awaited.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="source"/> is null.</exception>
        public static AsyncSubject<TSource> RunAsync<TSource>(this IConnectableObservable<TSource> source, CancellationToken cancellationToken)
        {
            if (source == null)
                throw new ArgumentNullException("source");

            return s_impl.RunAsync<TSource>(source, cancellationToken);
        }
    }
}
#endif

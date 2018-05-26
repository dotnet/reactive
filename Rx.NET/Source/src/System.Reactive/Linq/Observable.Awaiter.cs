// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the Apache 2.0 License.
// See the LICENSE file in the project root for more information. 

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
                throw new ArgumentNullException(nameof(source));

            return RunAsync<TSource>(source, CancellationToken.None);
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
                throw new ArgumentNullException(nameof(source));

            return RunAsync<TSource>(source, CancellationToken.None);
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
                throw new ArgumentNullException(nameof(source));

            var s = new AsyncSubject<TSource>();

            if (cancellationToken.IsCancellationRequested)
            {
                return Cancel(s, cancellationToken);
            }

            var d = source.SubscribeSafe(s);

            if (cancellationToken.CanBeCanceled)
            {
                RegisterCancelation(s, d, cancellationToken);
            }

            return s;
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
                throw new ArgumentNullException(nameof(source));

            var s = new AsyncSubject<TSource>();

            if (cancellationToken.IsCancellationRequested)
            {
                return Cancel(s, cancellationToken);
            }

            var d = source.SubscribeSafe(s);
            var c = source.Connect();

            if (cancellationToken.CanBeCanceled)
            {
                RegisterCancelation(s, StableCompositeDisposable.Create(d, c), cancellationToken);
            }

            return s; ;
        }

        private static AsyncSubject<T> Cancel<T>(AsyncSubject<T> subject, CancellationToken cancellationToken)
        {
            subject.OnError(new OperationCanceledException(cancellationToken));
            return subject;
        }

        private static void RegisterCancelation<T>(AsyncSubject<T> subject, IDisposable subscription, CancellationToken token)
        {
            //
            // Separate method used to avoid heap allocation of closure when no cancellation is needed,
            // e.g. when CancellationToken.None is provided to the RunAsync overloads.
            //

            var ctr = token.Register(() =>
            {
                subscription.Dispose();
                Cancel(subject, token);
            });

            //
            // No null-check for ctr is needed:
            //
            // - CancellationTokenRegistration is a struct
            // - Registration will succeed 99% of the time, no warranting an attempt to avoid spurious Subscribe calls
            //
            subject.Subscribe(Stubs<T>.Ignore, _ => ctr.Dispose(), ctr.Dispose);
        }
    }
}

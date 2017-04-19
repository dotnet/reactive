// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the Apache 2.0 License.
// See the LICENSE file in the project root for more information. 

using System.ComponentModel;
using System.Reactive;
using System.Reactive.Disposables;
using System.Threading;

namespace System
{
    /// <summary>
    /// Provides a set of static methods for subscribing delegates to observables.
    /// </summary>
    public static class ObservableExtensions
    {
        #region Subscribe delegate-based overloads

        /// <summary>
        /// Subscribes to the observable sequence without specifying any handlers.
        /// This method can be used to evaluate the observable sequence for its side-effects only.
        /// </summary>
        /// <typeparam name="T">The type of the elements in the source sequence.</typeparam>
        /// <param name="source">Observable sequence to subscribe to.</param>
        /// <returns><see cref="IDisposable"/> object used to unsubscribe from the observable sequence.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="source"/> is <c>null</c>.</exception>
        public static IDisposable Subscribe<T>(this IObservable<T> source)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));

            //
            // [OK] Use of unsafe Subscribe: non-pretentious constructor for an observer; this overload is not to be used internally.
            //
            return source.Subscribe/*Unsafe*/(new AnonymousObserver<T>(Stubs<T>.Ignore, Stubs.Throw, Stubs.Nop));
        }

        /// <summary>
        /// Subscribes an element handler to an observable sequence.
        /// </summary>
        /// <typeparam name="T">The type of the elements in the source sequence.</typeparam>
        /// <param name="source">Observable sequence to subscribe to.</param>
        /// <param name="onNext">Action to invoke for each element in the observable sequence.</param>
        /// <returns><see cref="IDisposable"/> object used to unsubscribe from the observable sequence.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="source"/> or <paramref name="onNext"/> is <c>null</c>.</exception>
        public static IDisposable Subscribe<T>(this IObservable<T> source, Action<T> onNext)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));
            if (onNext == null)
                throw new ArgumentNullException(nameof(onNext));

            //
            // [OK] Use of unsafe Subscribe: non-pretentious constructor for an observer; this overload is not to be used internally.
            //
            return source.Subscribe/*Unsafe*/(new AnonymousObserver<T>(onNext, Stubs.Throw, Stubs.Nop));
        }

        /// <summary>
        /// Subscribes an element handler and an exception handler to an observable sequence.
        /// </summary>
        /// <typeparam name="T">The type of the elements in the source sequence.</typeparam>
        /// <param name="source">Observable sequence to subscribe to.</param>
        /// <param name="onNext">Action to invoke for each element in the observable sequence.</param>
        /// <param name="onError">Action to invoke upon exceptional termination of the observable sequence.</param>
        /// <returns><see cref="IDisposable"/> object used to unsubscribe from the observable sequence.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="source"/> or <paramref name="onNext"/> or <paramref name="onError"/> is <c>null</c>.</exception>
        public static IDisposable Subscribe<T>(this IObservable<T> source, Action<T> onNext, Action<Exception> onError)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));
            if (onNext == null)
                throw new ArgumentNullException(nameof(onNext));
            if (onError == null)
                throw new ArgumentNullException(nameof(onError));

            //
            // [OK] Use of unsafe Subscribe: non-pretentious constructor for an observer; this overload is not to be used internally.
            //
            return source.Subscribe/*Unsafe*/(new AnonymousObserver<T>(onNext, onError, Stubs.Nop));
        }

        /// <summary>
        /// Subscribes an element handler and a completion handler to an observable sequence.
        /// </summary>
        /// <typeparam name="T">The type of the elements in the source sequence.</typeparam>
        /// <param name="source">Observable sequence to subscribe to.</param>
        /// <param name="onNext">Action to invoke for each element in the observable sequence.</param>
        /// <param name="onCompleted">Action to invoke upon graceful termination of the observable sequence.</param>
        /// <returns><see cref="IDisposable"/> object used to unsubscribe from the observable sequence.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="source"/> or <paramref name="onNext"/> or <paramref name="onCompleted"/> is <c>null</c>.</exception>
        public static IDisposable Subscribe<T>(this IObservable<T> source, Action<T> onNext, Action onCompleted)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));
            if (onNext == null)
                throw new ArgumentNullException(nameof(onNext));
            if (onCompleted == null)
                throw new ArgumentNullException(nameof(onCompleted));

            //
            // [OK] Use of unsafe Subscribe: non-pretentious constructor for an observer; this overload is not to be used internally.
            //
            return source.Subscribe/*Unsafe*/(new AnonymousObserver<T>(onNext, Stubs.Throw, onCompleted));
        }

        /// <summary>
        /// Subscribes an element handler, an exception handler, and a completion handler to an observable sequence.
        /// </summary>
        /// <typeparam name="T">The type of the elements in the source sequence.</typeparam>
        /// <param name="source">Observable sequence to subscribe to.</param>
        /// <param name="onNext">Action to invoke for each element in the observable sequence.</param>
        /// <param name="onError">Action to invoke upon exceptional termination of the observable sequence.</param>
        /// <param name="onCompleted">Action to invoke upon graceful termination of the observable sequence.</param>
        /// <returns><see cref="IDisposable"/> object used to unsubscribe from the observable sequence.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="source"/> or <paramref name="onNext"/> or <paramref name="onError"/> or <paramref name="onCompleted"/> is <c>null</c>.</exception>
        public static IDisposable Subscribe<T>(this IObservable<T> source, Action<T> onNext, Action<Exception> onError, Action onCompleted)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));
            if (onNext == null)
                throw new ArgumentNullException(nameof(onNext));
            if (onError == null)
                throw new ArgumentNullException(nameof(onError));
            if (onCompleted == null)
                throw new ArgumentNullException(nameof(onCompleted));

            //
            // [OK] Use of unsafe Subscribe: non-pretentious constructor for an observer; this overload is not to be used internally.
            //
            return source.Subscribe/*Unsafe*/(new AnonymousObserver<T>(onNext, onError, onCompleted));
        }

        #endregion

        #region Subscribe overloads with CancellationToken

        /// <summary>
        /// Subscribes an observer to an observable sequence, using a <see cref="CancellationToken"/> to support unsubscription.
        /// </summary>
        /// <typeparam name="T">The type of the elements in the source sequence.</typeparam>
        /// <param name="source">Observable sequence to subscribe to.</param>
        /// <param name="observer">Observer to subscribe to the sequence.</param>
        /// <param name="token">CancellationToken that can be signaled to unsubscribe from the source sequence.</param>
        /// <exception cref="ArgumentNullException"><paramref name="source"/> or <paramref name="observer"/> is <c>null</c>.</exception>
        public static void Subscribe<T>(this IObservable<T> source, IObserver<T> observer, CancellationToken token)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));
            if (observer == null)
                throw new ArgumentNullException(nameof(observer));

            source.Subscribe_(observer, token);
        }

        /// <summary>
        /// Subscribes to the observable sequence without specifying any handlers, using a <see cref="CancellationToken"/> to support unsubscription.
        /// This method can be used to evaluate the observable sequence for its side-effects only.
        /// </summary>
        /// <typeparam name="T">The type of the elements in the source sequence.</typeparam>
        /// <param name="source">Observable sequence to subscribe to.</param>
        /// <param name="token">CancellationToken that can be signaled to unsubscribe from the source sequence.</param>
        /// <exception cref="ArgumentNullException"><paramref name="source"/> is <c>null</c>.</exception>
        public static void Subscribe<T>(this IObservable<T> source, CancellationToken token)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));

            source.Subscribe_(new AnonymousObserver<T>(Stubs<T>.Ignore, Stubs.Throw, Stubs.Nop), token);
        }

        /// <summary>
        /// Subscribes an element handler to an observable sequence, using a <see cref="CancellationToken"/> to support unsubscription.
        /// </summary>
        /// <typeparam name="T">The type of the elements in the source sequence.</typeparam>
        /// <param name="source">Observable sequence to subscribe to.</param>
        /// <param name="onNext">Action to invoke for each element in the observable sequence.</param>
        /// <param name="token">CancellationToken that can be signaled to unsubscribe from the source sequence.</param>
        /// <exception cref="ArgumentNullException"><paramref name="source"/> or <paramref name="onNext"/> is <c>null</c>.</exception>
        public static void Subscribe<T>(this IObservable<T> source, Action<T> onNext, CancellationToken token)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));
            if (onNext == null)
                throw new ArgumentNullException(nameof(onNext));

            source.Subscribe_(new AnonymousObserver<T>(onNext, Stubs.Throw, Stubs.Nop), token);
        }

        /// <summary>
        /// Subscribes an element handler and an exception handler to an observable sequence, using a <see cref="CancellationToken"/> to support unsubscription.
        /// </summary>
        /// <typeparam name="T">The type of the elements in the source sequence.</typeparam>
        /// <param name="source">Observable sequence to subscribe to.</param>
        /// <param name="onNext">Action to invoke for each element in the observable sequence.</param>
        /// <param name="onError">Action to invoke upon exceptional termination of the observable sequence.</param>
        /// <param name="token">CancellationToken that can be signaled to unsubscribe from the source sequence.</param>
        /// <exception cref="ArgumentNullException"><paramref name="source"/> or <paramref name="onNext"/> or <paramref name="onError"/> is <c>null</c>.</exception>
        public static void Subscribe<T>(this IObservable<T> source, Action<T> onNext, Action<Exception> onError, CancellationToken token)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));
            if (onNext == null)
                throw new ArgumentNullException(nameof(onNext));
            if (onError == null)
                throw new ArgumentNullException(nameof(onError));

            source.Subscribe_(new AnonymousObserver<T>(onNext, onError, Stubs.Nop), token);
        }

        /// <summary>
        /// Subscribes an element handler and a completion handler to an observable sequence, using a <see cref="CancellationToken"/> to support unsubscription.
        /// </summary>
        /// <typeparam name="T">The type of the elements in the source sequence.</typeparam>
        /// <param name="source">Observable sequence to subscribe to.</param>
        /// <param name="onNext">Action to invoke for each element in the observable sequence.</param>
        /// <param name="onCompleted">Action to invoke upon graceful termination of the observable sequence.</param>
        /// <param name="token">CancellationToken that can be signaled to unsubscribe from the source sequence.</param>
        /// <exception cref="ArgumentNullException"><paramref name="source"/> or <paramref name="onNext"/> or <paramref name="onCompleted"/> is <c>null</c>.</exception>
        public static void Subscribe<T>(this IObservable<T> source, Action<T> onNext, Action onCompleted, CancellationToken token)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));
            if (onNext == null)
                throw new ArgumentNullException(nameof(onNext));
            if (onCompleted == null)
                throw new ArgumentNullException(nameof(onCompleted));

            source.Subscribe_(new AnonymousObserver<T>(onNext, Stubs.Throw, onCompleted), token);
        }

        /// <summary>
        /// Subscribes an element handler, an exception handler, and a completion handler to an observable sequence, using a <see cref="CancellationToken"/> to support unsubscription.
        /// </summary>
        /// <typeparam name="T">The type of the elements in the source sequence.</typeparam>
        /// <param name="source">Observable sequence to subscribe to.</param>
        /// <param name="onNext">Action to invoke for each element in the observable sequence.</param>
        /// <param name="onError">Action to invoke upon exceptional termination of the observable sequence.</param>
        /// <param name="onCompleted">Action to invoke upon graceful termination of the observable sequence.</param>
        /// <param name="token">CancellationToken that can be signaled to unsubscribe from the source sequence.</param>
        /// <exception cref="ArgumentNullException"><paramref name="source"/> or <paramref name="onNext"/> or <paramref name="onError"/> or <paramref name="onCompleted"/> is <c>null</c>.</exception>
        public static void Subscribe<T>(this IObservable<T> source, Action<T> onNext, Action<Exception> onError, Action onCompleted, CancellationToken token)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));
            if (onNext == null)
                throw new ArgumentNullException(nameof(onNext));
            if (onError == null)
                throw new ArgumentNullException(nameof(onError));
            if (onCompleted == null)
                throw new ArgumentNullException(nameof(onCompleted));

            source.Subscribe_(new AnonymousObserver<T>(onNext, onError, onCompleted), token);
        }

        private static void Subscribe_<T>(this IObservable<T> source, IObserver<T> observer, CancellationToken token)
        {
            if (token.CanBeCanceled)
            {
                if (!token.IsCancellationRequested)
                {
                    var r = new SingleAssignmentDisposable();

                    //
                    // [OK] Use of unsafe Subscribe: exception during Subscribe doesn't orphan CancellationTokenRegistration.
                    //
                    var d = source.Subscribe/*Unsafe*/(
                        observer.OnNext,
                        ex =>
                        {
                            using (r)
                            {
                                observer.OnError(ex);
                            }
                        },
                        () =>
                        {
                            using (r)
                            {
                                observer.OnCompleted();
                            }
                        }
                    );

                    r.Disposable = token.Register(d.Dispose);
                }
            }
            else
            {
                source.Subscribe(observer);
            }
        }

        #endregion

        #region SubscribeSafe

        /// <summary>
        /// Subscribes to the specified source, re-routing synchronous exceptions during invocation of the <see cref="IObservable{T}.Subscribe(IObserver{T})"/> method to the observer's <see cref="IObserver{T}.OnError(Exception)"/> channel.
        /// This method is typically used when writing query operators.
        /// </summary>
        /// <typeparam name="T">The type of the elements in the source sequence.</typeparam>
        /// <param name="source">Observable sequence to subscribe to.</param>
        /// <param name="observer">Observer that will be passed to the observable sequence, and that will be used for exception propagation.</param>
        /// <returns><see cref="IDisposable"/> object used to unsubscribe from the observable sequence.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="source"/> or <paramref name="observer"/> is <c>null</c>.</exception>
        [EditorBrowsable(EditorBrowsableState.Advanced)]
        public static IDisposable SubscribeSafe<T>(this IObservable<T> source, IObserver<T> observer)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));
            if (observer == null)
                throw new ArgumentNullException(nameof(observer));

            //
            // The following types are white-listed and should not exhibit exceptional behavior
            // for regular operation circumstances.
            //
            if (source is ObservableBase<T>)
            {
                return source.Subscribe(observer);
            }

            if (source is IProducer<T> producer)
            {
                return producer.SubscribeRaw(observer, enableSafeguard: false);
            }

            var d = Disposable.Empty;

            try
            {
                d = source.Subscribe(observer);
            }
            catch (Exception exception)
            {
                //
                // The effect of redirecting the exception to the OnError channel is automatic
                // clean-up of query operator state for a large number of cases. For example,
                // consider a binary and temporal query operator with the following Subscribe
                // behavior (implemented using the Producer pattern with a Run method):
                //
                //   public IDisposable Run(...)
                //   {
                //       var tm = _scheduler.Schedule(_due, Tick);
                //
                //       var df = _fst.SubscribeSafe(new FstObserver(this, ...));
                //       var ds = _snd.SubscribeSafe(new SndObserver(this, ...)); // <-- fails
                //
                //       return new CompositeDisposable(tm, df, ds);
                //   }
                //
                // If the second subscription fails, we're not leaving the first subscription
                // or the scheduled job hanging around. Instead, the OnError propagation to
                // the SndObserver should take care of a Dispose call to the observer's parent
                // object. The handshake between Producer and Sink objects will ultimately
                // cause disposal of the CompositeDisposable that's returned from the method.
                //
                observer.OnError(exception);
            }

            return d;
        }

        #endregion
    }
}

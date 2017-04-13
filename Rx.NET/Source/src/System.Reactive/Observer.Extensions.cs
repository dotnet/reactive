// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the Apache 2.0 License.
// See the LICENSE file in the project root for more information. 

using System.Reactive.Concurrency;
using System.Threading;

namespace System.Reactive
{
    /// <summary>
    /// Provides a set of static methods for creating observers.
    /// </summary>
    public static class Observer
    {
        /// <summary>
        /// Creates an observer from a notification callback.
        /// </summary>
        /// <typeparam name="T">The type of the elements received by the observer.</typeparam>
        /// <param name="handler">Action that handles a notification.</param>
        /// <returns>The observer object that invokes the specified handler using a notification corresponding to each message it receives.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="handler"/> is null.</exception>
        public static IObserver<T> ToObserver<T>(this Action<Notification<T>> handler)
        {
            if (handler == null)
                throw new ArgumentNullException(nameof(handler));

            return new AnonymousObserver<T>(
                x => handler(Notification.CreateOnNext<T>(x)),
                exception => handler(Notification.CreateOnError<T>(exception)),
                () => handler(Notification.CreateOnCompleted<T>())
            );
        }

        /// <summary>
        /// Creates a notification callback from an observer.
        /// </summary>
        /// <typeparam name="T">The type of the elements received by the observer.</typeparam>
        /// <param name="observer">Observer object.</param>
        /// <returns>The action that forwards its input notification to the underlying observer.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="observer"/> is null.</exception>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly", MessageId = "Notifier", Justification = "Backward compat.")]
        public static Action<Notification<T>> ToNotifier<T>(this IObserver<T> observer)
        {
            if (observer == null)
                throw new ArgumentNullException(nameof(observer));

            return n => n.Accept(observer);
        }      

        /// <summary>
        /// Creates an observer from the specified OnNext action.
        /// </summary>
        /// <typeparam name="T">The type of the elements received by the observer.</typeparam>
        /// <param name="onNext">Observer's OnNext action implementation.</param>
        /// <returns>The observer object implemented using the given actions.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="onNext"/> is null.</exception>
        public static IObserver<T> Create<T>(Action<T> onNext)
        {
            if (onNext == null)
                throw new ArgumentNullException(nameof(onNext));

            return new AnonymousObserver<T>(onNext);
        }

        /// <summary>
        /// Creates an observer from the specified OnNext and OnError actions.
        /// </summary>
        /// <typeparam name="T">The type of the elements received by the observer.</typeparam>
        /// <param name="onNext">Observer's OnNext action implementation.</param>
        /// <param name="onError">Observer's OnError action implementation.</param>
        /// <returns>The observer object implemented using the given actions.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="onNext"/> or <paramref name="onError"/> is null.</exception>
        public static IObserver<T> Create<T>(Action<T> onNext, Action<Exception> onError)
        {
            if (onNext == null)
                throw new ArgumentNullException(nameof(onNext));
            if (onError == null)
                throw new ArgumentNullException(nameof(onError));

            return new AnonymousObserver<T>(onNext, onError);
        }

        /// <summary>
        /// Creates an observer from the specified OnNext and OnCompleted actions.
        /// </summary>
        /// <typeparam name="T">The type of the elements received by the observer.</typeparam>
        /// <param name="onNext">Observer's OnNext action implementation.</param>
        /// <param name="onCompleted">Observer's OnCompleted action implementation.</param>
        /// <returns>The observer object implemented using the given actions.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="onNext"/> or <paramref name="onCompleted"/> is null.</exception>
        public static IObserver<T> Create<T>(Action<T> onNext, Action onCompleted)
        {
            if (onNext == null)
                throw new ArgumentNullException(nameof(onNext));
            if (onCompleted == null)
                throw new ArgumentNullException(nameof(onCompleted));

            return new AnonymousObserver<T>(onNext, onCompleted);
        }

        /// <summary>
        /// Creates an observer from the specified OnNext, OnError, and OnCompleted actions.
        /// </summary>
        /// <typeparam name="T">The type of the elements received by the observer.</typeparam>
        /// <param name="onNext">Observer's OnNext action implementation.</param>
        /// <param name="onError">Observer's OnError action implementation.</param>
        /// <param name="onCompleted">Observer's OnCompleted action implementation.</param>
        /// <returns>The observer object implemented using the given actions.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="onNext"/> or <paramref name="onError"/> or <paramref name="onCompleted"/> is null.</exception>
        public static IObserver<T> Create<T>(Action<T> onNext, Action<Exception> onError, Action onCompleted)
        {
            if (onNext == null)
                throw new ArgumentNullException(nameof(onNext));
            if (onError == null)
                throw new ArgumentNullException(nameof(onError));
            if (onCompleted == null)
                throw new ArgumentNullException(nameof(onCompleted));

            return new AnonymousObserver<T>(onNext, onError, onCompleted);
        }

        /// <summary>
        /// Hides the identity of an observer.
        /// </summary>
        /// <typeparam name="T">The type of the elements received by the source observer.</typeparam>
        /// <param name="observer">An observer whose identity to hide.</param>
        /// <returns>An observer that hides the identity of the specified observer.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="observer"/> is null.</exception>
        public static IObserver<T> AsObserver<T>(this IObserver<T> observer)
        {
            if (observer == null)
                throw new ArgumentNullException(nameof(observer));

            return new AnonymousObserver<T>(observer.OnNext, observer.OnError, observer.OnCompleted);
        }

        /// <summary>
        /// Checks access to the observer for grammar violations. This includes checking for multiple OnError or OnCompleted calls, as well as reentrancy in any of the observer methods.
        /// If a violation is detected, an InvalidOperationException is thrown from the offending observer method call.
        /// </summary>
        /// <typeparam name="T">The type of the elements received by the source observer.</typeparam>
        /// <param name="observer">The observer whose callback invocations should be checked for grammar violations.</param>
        /// <returns>An observer that checks callbacks invocations against the observer grammar and, if the checks pass, forwards those to the specified observer.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="observer"/> is null.</exception>
        public static IObserver<T> Checked<T>(this IObserver<T> observer)
        {
            if (observer == null)
                throw new ArgumentNullException(nameof(observer));

            return new CheckedObserver<T>(observer);
        }

        /// <summary>
        /// Synchronizes access to the observer such that its callback methods cannot be called concurrently from multiple threads. This overload is useful when coordinating access to an observer.
        /// Notice reentrant observer callbacks on the same thread are still possible.
        /// </summary>
        /// <typeparam name="T">The type of the elements received by the source observer.</typeparam>
        /// <param name="observer">The observer whose callbacks should be synchronized.</param>
        /// <returns>An observer that delivers callbacks to the specified observer in a synchronized manner.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="observer"/> is null.</exception>
        /// <remarks>
        /// Because a <see cref="System.Threading.Monitor">Monitor</see> is used to perform the synchronization, there's no protection against reentrancy from the same thread.
        /// Hence, overlapped observer callbacks are still possible, which is invalid behavior according to the observer grammar. In order to protect against this behavior as
        /// well, use the <see cref="Synchronize{T}(IObserver{T}, bool)"/> overload, passing true for the second parameter.
        /// </remarks>
        public static IObserver<T> Synchronize<T>(IObserver<T> observer)
        {
            if (observer == null)
                throw new ArgumentNullException(nameof(observer));

            return new SynchronizedObserver<T>(observer, new object());
        }

        /// <summary>
        /// Synchronizes access to the observer such that its callback methods cannot be called concurrently. This overload is useful when coordinating access to an observer.
        /// The <paramref name="preventReentrancy"/> parameter configures the type of lock used for synchronization.
        /// </summary>
        /// <typeparam name="T">The type of the elements received by the source observer.</typeparam>
        /// <param name="observer">The observer whose callbacks should be synchronized.</param>
        /// <param name="preventReentrancy">If set to true, reentrant observer callbacks will be queued up and get delivered to the observer in a sequential manner.</param>
        /// <returns>An observer that delivers callbacks to the specified observer in a synchronized manner.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="observer"/> is null.</exception>
        /// <remarks>
        /// When the <paramref name="preventReentrancy"/> parameter is set to false, behavior is identical to the <see cref="Synchronize{T}(IObserver{T})"/> overload which uses
        /// a <see cref="System.Threading.Monitor">Monitor</see> for synchronization. When the <paramref name="preventReentrancy"/> parameter is set to true, an <see cref="AsyncLock"/>
        /// is used to queue up callbacks to the specified observer if a reentrant call is made.
        /// </remarks>
        public static IObserver<T> Synchronize<T>(IObserver<T> observer, bool preventReentrancy)
        {
            if (observer == null)
                throw new ArgumentNullException(nameof(observer));

            if (preventReentrancy)
                return new AsyncLockObserver<T>(observer, new AsyncLock());
            else
                return new SynchronizedObserver<T>(observer, new object());
        }

        /// <summary>
        /// Synchronizes access to the observer such that its callback methods cannot be called concurrently by multiple threads, using the specified gate object for use by a <see cref="System.Threading.Monitor">Monitor</see>-based lock.
        /// This overload is useful when coordinating multiple observers that access shared state by synchronizing on a common gate object.
        /// Notice reentrant observer callbacks on the same thread are still possible.
        /// </summary>
        /// <typeparam name="T">The type of the elements received by the source observer.</typeparam>
        /// <param name="observer">The observer whose callbacks should be synchronized.</param>
        /// <param name="gate">Gate object to synchronize each observer call on.</param>
        /// <returns>An observer that delivers callbacks to the specified observer in a synchronized manner.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="observer"/> or <paramref name="gate"/> is null.</exception>
        /// <remarks>
        /// Because a <see cref="System.Threading.Monitor">Monitor</see> is used to perform the synchronization, there's no protection against reentrancy from the same thread.
        /// Hence, overlapped observer callbacks are still possible, which is invalid behavior according to the observer grammar. In order to protect against this behavior as
        /// well, use the <see cref="Synchronize{T}(IObserver{T}, AsyncLock)"/> overload.
        /// </remarks>
        public static IObserver<T> Synchronize<T>(IObserver<T> observer, object gate)
        {
            if (observer == null)
                throw new ArgumentNullException(nameof(observer));
            if (gate == null)
                throw new ArgumentNullException(nameof(gate));

            return new SynchronizedObserver<T>(observer, gate);
        }

        /// <summary>
        /// Synchronizes access to the observer such that its callback methods cannot be called concurrently, using the specified asynchronous lock to protect against concurrent and reentrant access.
        /// This overload is useful when coordinating multiple observers that access shared state by synchronizing on a common asynchronous lock.
        /// </summary>
        /// <typeparam name="T">The type of the elements received by the source observer.</typeparam>
        /// <param name="observer">The observer whose callbacks should be synchronized.</param>
        /// <param name="asyncLock">Gate object to synchronize each observer call on.</param>
        /// <returns>An observer that delivers callbacks to the specified observer in a synchronized manner.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="observer"/> or <paramref name="asyncLock"/> is null.</exception>
        public static IObserver<T> Synchronize<T>(IObserver<T> observer, AsyncLock asyncLock)
        {
            if (observer == null)
                throw new ArgumentNullException(nameof(observer));
            if (asyncLock == null)
                throw new ArgumentNullException(nameof(asyncLock));

            return new AsyncLockObserver<T>(observer, asyncLock);
        }

        /// <summary>
        /// Schedules the invocation of observer methods on the given scheduler.
        /// </summary>
        /// <typeparam name="T">The type of the elements received by the source observer.</typeparam>
        /// <param name="observer">The observer to schedule messages for.</param>
        /// <param name="scheduler">Scheduler to schedule observer messages on.</param>
        /// <returns>Observer whose messages are scheduled on the given scheduler.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="observer"/> or <paramref name="scheduler"/> is null.</exception>
        public static IObserver<T> NotifyOn<T>(this IObserver<T> observer, IScheduler scheduler)
        {
            if (observer == null)
                throw new ArgumentNullException(nameof(observer));
            if (scheduler == null)
                throw new ArgumentNullException(nameof(scheduler));

            return new ObserveOnObserver<T>(scheduler, observer, null);
        }

        /// <summary>
        /// Schedules the invocation of observer methods on the given synchonization context.
        /// </summary>
        /// <typeparam name="T">The type of the elements received by the source observer.</typeparam>
        /// <param name="observer">The observer to schedule messages for.</param>
        /// <param name="context">Synchonization context to schedule observer messages on.</param>
        /// <returns>Observer whose messages are scheduled on the given synchonization context.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="observer"/> or <paramref name="context"/> is null.</exception>
        public static IObserver<T> NotifyOn<T>(this IObserver<T> observer, SynchronizationContext context)
        {
            if (observer == null)
                throw new ArgumentNullException(nameof(observer));
            if (context == null)
                throw new ArgumentNullException(nameof(context));

            return new ObserveOnObserver<T>(new SynchronizationContextScheduler(context), observer, null);
        }

        /// <summary>
        /// Converts an observer to a progress object.
        /// </summary>
        /// <typeparam name="T">The type of the progress objects received by the source observer.</typeparam>
        /// <param name="observer">The observer to convert.</param>
        /// <returns>Progress object whose Report messages correspond to the observer's OnNext messages.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="observer"/> is null.</exception>
        public static IProgress<T> ToProgress<T>(this IObserver<T> observer)
        {
            if (observer == null)
                throw new ArgumentNullException(nameof(observer));

            return new AnonymousProgress<T>(observer.OnNext);
        }

        /// <summary>
        /// Converts an observer to a progress object, using the specified scheduler to invoke the progress reporting method.
        /// </summary>
        /// <typeparam name="T">The type of the progress objects received by the source observer.</typeparam>
        /// <param name="observer">The observer to convert.</param>
        /// <param name="scheduler">Scheduler to report progress on.</param>
        /// <returns>Progress object whose Report messages correspond to the observer's OnNext messages.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="observer"/> or <paramref name="scheduler"/> is null.</exception>
        public static IProgress<T> ToProgress<T>(this IObserver<T> observer, IScheduler scheduler)
        {
            if (observer == null)
                throw new ArgumentNullException(nameof(observer));
            if (scheduler == null)
                throw new ArgumentNullException(nameof(scheduler));

            return new AnonymousProgress<T>(new ObserveOnObserver<T>(scheduler, observer, null).OnNext);
        }

        class AnonymousProgress<T> : IProgress<T>
        {
            private readonly Action<T> _progress;

            public AnonymousProgress(Action<T> progress)
            {
                _progress = progress;
            }

            public void Report(T value)
            {
                _progress(value);
            }
        }

        /// <summary>
        /// Converts a progress object to an observer.
        /// </summary>
        /// <typeparam name="T">The type of the progress objects received by the progress reporter.</typeparam>
        /// <param name="progress">The progress object to convert.</param>
        /// <returns>Observer whose OnNext messages correspond to the progress object's Report messages.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="progress"/> is null.</exception>
        public static IObserver<T> ToObserver<T>(this IProgress<T> progress)
        {
            if (progress == null)
                throw new ArgumentNullException(nameof(progress));

            return new AnonymousObserver<T>(progress.Report);
        }
    }
}

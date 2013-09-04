// Copyright (c) Microsoft Open Technologies, Inc. All rights reserved. See License.txt in the project root for license information.

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
        /// <param name="handler">Action that handles a notification.</param>
        /// <returns>The observer object that invokes the specified handler using a notification corresponding to each message it receives.</returns>
        public static IObserver<T> ToObserver<T>(this Action<Notification<T>> handler)
        {
            if (handler == null)
                throw new ArgumentNullException("handler");

            return new AnonymousObserver<T>(
                x => handler(Notification.CreateOnNext<T>(x)),
                exception => handler(Notification.CreateOnError<T>(exception)),
                () => handler(Notification.CreateOnCompleted<T>()));
        }

        /// <summary>
        /// Creates a notification callback from an observer.
        /// </summary>
        /// <param name="observer">Observer object.</param>
        /// <returns>The action that forwards its input notification to the underlying observer.</returns>
        public static Action<Notification<T>> ToNotifier<T>(this IObserver<T> observer)
        {
            if (observer == null)
                throw new ArgumentNullException("observer");

            return n => n.Accept(observer);
        }      

        /// <summary>
        /// Creates an observer from the specified OnNext action.
        /// </summary>
        /// <param name="onNext">Observer's OnNext action implementation.</param>
        /// <returns>The observer object implemented using the given actions.</returns>
        public static IObserver<T> Create<T>(Action<T> onNext)
        {
            if (onNext == null)
                throw new ArgumentNullException("onNext");

            return new AnonymousObserver<T>(onNext);
        }

        /// <summary>
        /// Creates an observer from the specified OnNext and OnError actions.
        /// </summary>
        /// <param name="onNext">Observer's OnNext action implementation.</param>
        /// <param name="onError">Observer's OnError action implementation.</param>
        /// <returns>The observer object implemented using the given actions.</returns>
        public static IObserver<T> Create<T>(Action<T> onNext, Action<Exception> onError)
        {
            if (onNext == null)
                throw new ArgumentNullException("onNext");
            if (onError == null)
                throw new ArgumentNullException("onError");

            return new AnonymousObserver<T>(onNext, onError);
        }

        /// <summary>
        /// Creates an observer from the specified OnNext and OnCompleted actions.
        /// </summary>
        /// <param name="onNext">Observer's OnNext action implementation.</param>
        /// <param name="onCompleted">Observer's OnCompleted action implementation.</param>
        /// <returns>The observer object implemented using the given actions.</returns>
        public static IObserver<T> Create<T>(Action<T> onNext, Action onCompleted)
        {
            if (onNext == null)
                throw new ArgumentNullException("onNext");
            if (onCompleted == null)
                throw new ArgumentNullException("onCompleted");

            return new AnonymousObserver<T>(onNext, onCompleted);
        }

        /// <summary>
        /// Creates an observer from the specified OnNext, OnError, and OnCompleted actions.
        /// </summary>
        /// <param name="onNext">Observer's OnNext action implementation.</param>
        /// <param name="onError">Observer's OnError action implementation.</param>
        /// <param name="onCompleted">Observer's OnCompleted action implementation.</param>
        /// <returns>The observer object implemented using the given actions.</returns>
        public static IObserver<T> Create<T>(Action<T> onNext, Action<Exception> onError, Action onCompleted)
        {
            if (onNext == null)
                throw new ArgumentNullException("onNext");
            if (onError == null)
                throw new ArgumentNullException("onError");
            if (onCompleted == null)
                throw new ArgumentNullException("onCompleted");

            return new AnonymousObserver<T>(onNext, onError, onCompleted);
        }

        /// <summary>
        /// Hides the identity of an observer.
        /// </summary>
        /// <param name="observer">An observer whose identity to hide.</param>
        /// <returns>An observer that hides the identity of the specified observer.</returns>
        public static IObserver<T> AsObserver<T>(this IObserver<T> observer)
        {
            if (observer == null)
                throw new ArgumentNullException("observer");

            return new AnonymousObserver<T>(observer.OnNext, observer.OnError, observer.OnCompleted);
        }

        /// <summary>
        /// Synchronizes the observer messages.
        /// </summary>
        /// <param name="observer">The observer to synchronize.</param>
        /// <param name="gate">Gate object to synchronize each observer call on.</param>
        /// <returns>The observer whose messages are synchronized on the given gate object.</returns>
        public static IObserver<T> Synchronize<T>(IObserver<T> observer, object gate)
        {
            if (observer == null)
                throw new ArgumentNullException("observer");
            if (gate == null)
                throw new ArgumentNullException("gate");

            return new SynchronizedObserver<T>(observer, gate);
        }

        /// <summary>
        /// Synchronizes the observer messages.
        /// </summary>
        /// <param name="observer">The observer to synchronize.</param>
        /// <returns>The observer whose messages are synchronized.</returns>
        public static IObserver<T> Synchronize<T>(IObserver<T> observer)
        {
            if (observer == null)
                throw new ArgumentNullException("observer");

            return Synchronize(observer, new object());
        }

        /// <summary>
        /// Schedules the observer messages on the given scheduler.
        /// </summary>
        /// <param name="observer">The observer to schedule messages for.</param>
        /// <param name="scheduler">Scheduler to schedule observer messages on.</param>
        /// <returns>Observer whose messages are scheduled on the given scheduler.</returns>
        public static IObserver<T> NotifyOn<T>(this IObserver<T> observer, IScheduler scheduler)
        {
            if (observer == null)
                throw new ArgumentNullException("observer");
            if (scheduler == null)
                throw new ArgumentNullException("scheduler");

            return new ObserveOnObserver<T>(scheduler, observer, null);
        }

#if !NO_SYNCCTX
        /// <summary>
        /// Schedules the observer messages on the given synchonization context.
        /// </summary>
        /// <param name="observer">The observer to schedule messages for.</param>
        /// <param name="context">Synchonization context to schedule observer messages on.</param>
        /// <returns>Observer whose messages are scheduled on the given synchonization context.</returns>
        public static IObserver<T> NotifyOn<T>(this IObserver<T> observer, SynchronizationContext context)
        {
            if (observer == null)
                throw new ArgumentNullException("observer");
            if (context == null)
                throw new ArgumentNullException("context");

            return new ObserveOnObserver<T>(new SynchronizationContextScheduler(context), observer, null);
        }
#endif

#if HAS_PROGRESS
        /// <summary>
        /// Converts an observer to a progress object.
        /// </summary>
        /// <param name="observer">The observer to convert.</param>
        /// <returns>Progress object whose Report messages correspond to the observer's OnNext messages.</returns>
        public static IProgress<T> ToProgress<T>(this IObserver<T> observer)
        {
            if (observer == null)
                throw new ArgumentNullException("observer");

            return new AnonymousProgress<T>(observer.OnNext);
        }

        /// <summary>
        /// Converts an observer to a progress object.
        /// </summary>
        /// <param name="observer">The observer to convert.</param>
        /// <param name="scheduler">Scheduler to report progress on.</param>
        /// <returns>Progress object whose Report messages correspond to the observer's OnNext messages.</returns>
        public static IProgress<T> ToProgress<T>(this IObserver<T> observer, IScheduler scheduler)
        {
            if (observer == null)
                throw new ArgumentNullException("observer");
            if (scheduler == null)
                throw new ArgumentNullException("scheduler");

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
        /// <param name="progress">The progress object to convert.</param>
        /// <returns>Observer whose OnNext messages correspond to the progress object's Report messages.</returns>
        public static IObserver<T> ToObserver<T>(this IProgress<T> progress)
        {
            if (progress == null)
                throw new ArgumentNullException("progress");

            return Create<T>(progress.Report);
        }
#endif
    }
}

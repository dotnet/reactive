// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the Apache 2.0 License.
// See the LICENSE file in the project root for more information. 

using System.Reactive.Concurrency;
using System.Reactive.Linq;

namespace System.Reactive.Subjects
{
    /// <summary>
    /// Provides a set of static methods for creating subjects.
    /// </summary>
    public static class Subject
    {
        /// <summary>
        /// Creates a subject from the specified observer and observable.
        /// </summary>
        /// <typeparam name="TSource">The type of the elements received by the observer.</typeparam>
        /// <typeparam name="TResult">The type of the elements produced by the observable sequence.</typeparam>
        /// <param name="observer">The observer used to send messages to the subject.</param>
        /// <param name="observable">The observable used to subscribe to messages sent from the subject.</param>
        /// <returns>Subject implemented using the given observer and observable.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="observer"/> or <paramref name="observable"/> is <c>null</c>.</exception>
        public static ISubject<TSource, TResult> Create<TSource, TResult>(IObserver<TSource> observer, IObservable<TResult> observable)
        {
            if (observer == null)
                throw new ArgumentNullException(nameof(observer));
            if (observable == null)
                throw new ArgumentNullException(nameof(observable));

            return new AnonymousSubject<TSource, TResult>(observer, observable);
        }

        /// <summary>
        /// Creates a subject from the specified observer and observable.
        /// </summary>
        /// <typeparam name="T">The type of the elements received by the observer and produced by the observable sequence.</typeparam>
        /// <param name="observer">The observer used to send messages to the subject.</param>
        /// <param name="observable">The observable used to subscribe to messages sent from the subject.</param>
        /// <returns>Subject implemented using the given observer and observable.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="observer"/> or <paramref name="observable"/> is <c>null</c>.</exception>
        public static ISubject<T> Create<T>(IObserver<T> observer, IObservable<T> observable)
        {
            if (observer == null)
                throw new ArgumentNullException(nameof(observer));
            if (observable == null)
                throw new ArgumentNullException(nameof(observable));

            return new AnonymousSubject<T>(observer, observable);
        }

        /// <summary>
        /// Synchronizes the messages sent to the subject.
        /// </summary>
        /// <typeparam name="TSource">The type of the elements received by the subject.</typeparam>
        /// <typeparam name="TResult">The type of the elements produced by the subject.</typeparam>
        /// <param name="subject">The subject to synchronize.</param>
        /// <returns>Subject whose messages are synchronized.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="subject"/> is <c>null</c>.</exception>
        public static ISubject<TSource, TResult> Synchronize<TSource, TResult>(ISubject<TSource, TResult> subject)
        {
            if (subject == null)
                throw new ArgumentNullException(nameof(subject));

            return new AnonymousSubject<TSource, TResult>(Observer.Synchronize(subject), subject);
        }

        /// <summary>
        /// Synchronizes the messages sent to the subject.
        /// </summary>
        /// <typeparam name="TSource">The type of the elements received and produced by the subject.</typeparam>
        /// <param name="subject">The subject to synchronize.</param>
        /// <returns>Subject whose messages are synchronized.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="subject"/> is <c>null</c>.</exception>
        public static ISubject<TSource> Synchronize<TSource>(ISubject<TSource> subject)
        {
            if (subject == null)
                throw new ArgumentNullException(nameof(subject));

            return new AnonymousSubject<TSource>(Observer.Synchronize(subject), subject);
        }

        /// <summary>
        /// Synchronizes the messages sent to the subject and notifies observers on the specified scheduler.
        /// </summary>
        /// <typeparam name="TSource">The type of the elements received by the subject.</typeparam>
        /// <typeparam name="TResult">The type of the elements produced by the subject.</typeparam>
        /// <param name="subject">The subject to synchronize.</param>
        /// <param name="scheduler">Scheduler to notify observers on.</param>
        /// <returns>Subject whose messages are synchronized and whose observers are notified on the given scheduler.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="subject"/> or <paramref name="scheduler"/> is <c>null</c>.</exception>
        public static ISubject<TSource, TResult> Synchronize<TSource, TResult>(ISubject<TSource, TResult> subject, IScheduler scheduler)
        {
            if (subject == null)
                throw new ArgumentNullException(nameof(subject));
            if (scheduler == null)
                throw new ArgumentNullException(nameof(scheduler));

            return new AnonymousSubject<TSource, TResult>(Observer.Synchronize(subject), subject.ObserveOn(scheduler));
        }

        /// <summary>
        /// Synchronizes the messages sent to the subject and notifies observers on the specified scheduler.
        /// </summary>
        /// <typeparam name="TSource">The type of the elements received and produced by the subject.</typeparam>
        /// <param name="subject">The subject to synchronize.</param>
        /// <param name="scheduler">Scheduler to notify observers on.</param>
        /// <returns>Subject whose messages are synchronized and whose observers are notified on the given scheduler.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="subject"/> or <paramref name="scheduler"/> is <c>null</c>.</exception>
        public static ISubject<TSource> Synchronize<TSource>(ISubject<TSource> subject, IScheduler scheduler)
        {
            if (subject == null)
                throw new ArgumentNullException(nameof(subject));
            if (scheduler == null)
                throw new ArgumentNullException(nameof(scheduler));

            return new AnonymousSubject<TSource>(Observer.Synchronize(subject), subject.ObserveOn(scheduler));
        }

        private class AnonymousSubject<T, U> : ISubject<T, U>
        {
            private readonly IObserver<T> _observer;
            private readonly IObservable<U> _observable;

            public AnonymousSubject(IObserver<T> observer, IObservable<U> observable)
            {
                _observer = observer;
                _observable = observable;
            }

            public void OnCompleted() => _observer.OnCompleted();

            public void OnError(Exception error)
            {
                if (error == null)
                    throw new ArgumentNullException(nameof(error));

                _observer.OnError(error);
            }

            public void OnNext(T value) => _observer.OnNext(value);

            public IDisposable Subscribe(IObserver<U> observer)
            {
                if (observer == null)
                    throw new ArgumentNullException(nameof(observer));

                //
                // [OK] Use of unsafe Subscribe: non-pretentious wrapping of an observable sequence.
                //
                return _observable.Subscribe/*Unsafe*/(observer);
            }
        }

        private sealed class AnonymousSubject<T> : AnonymousSubject<T, T>, ISubject<T>
        {
            public AnonymousSubject(IObserver<T> observer, IObservable<T> observable)
                : base(observer, observable)
            {
            }
        }
    }
}

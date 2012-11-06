// Copyright (c) Microsoft Open Technologies, Inc. All rights reserved. See License.txt in the project root for license information.

using System.Collections.Generic;
using System.Reactive.Concurrency;

namespace System.Reactive.Linq
{
    public static partial class Observable
    {
        #region + Subscribe +

        /// <summary>
        /// Subscribes an observer to an enumerable sequence.
        /// </summary>
        /// <typeparam name="TSource">The type of the elements in the source sequence.</typeparam>
        /// <param name="source">Enumerable sequence to subscribe to.</param>
        /// <param name="observer">Observer that will receive notifications from the enumerable sequence.</param>
        /// <returns>Disposable object that can be used to unsubscribe the observer from the enumerable</returns>
        /// <exception cref="ArgumentNullException"><paramref name="source"/> or <paramref name="observer"/> is null.</exception>
        public static IDisposable Subscribe<TSource>(this IEnumerable<TSource> source, IObserver<TSource> observer)
        {
            if (source == null)
                throw new ArgumentNullException("source");
            if (observer == null)
                throw new ArgumentNullException("observer");

            return s_impl.Subscribe<TSource>(source, observer);
        }

        /// <summary>
        /// Subscribes an observer to an enumerable sequence, using the specified scheduler to run the enumeration loop.
        /// </summary>
        /// <typeparam name="TSource">The type of the elements in the source sequence.</typeparam>
        /// <param name="source">Enumerable sequence to subscribe to.</param>
        /// <param name="observer">Observer that will receive notifications from the enumerable sequence.</param>
        /// <param name="scheduler">Scheduler to perform the enumeration on.</param>
        /// <returns>Disposable object that can be used to unsubscribe the observer from the enumerable</returns>
        /// <exception cref="ArgumentNullException"><paramref name="source"/> or <paramref name="observer"/> or <paramref name="scheduler"/> is null.</exception>
        public static IDisposable Subscribe<TSource>(this IEnumerable<TSource> source, IObserver<TSource> observer, IScheduler scheduler)
        {
            if (source == null)
                throw new ArgumentNullException("source");
            if (observer == null)
                throw new ArgumentNullException("observer");
            if (scheduler == null)
                throw new ArgumentNullException("scheduler");

            return s_impl.Subscribe<TSource>(source, observer, scheduler);
        }

        #endregion

        #region + ToEnumerable +

        /// <summary>
        /// Converts an observable sequence to an enumerable sequence.
        /// </summary>
        /// <typeparam name="TSource">The type of the elements in the source sequence.</typeparam>
        /// <param name="source">An observable sequence to convert to an enumerable sequence.</param>
        /// <returns>The enumerable sequence containing the elements in the observable sequence.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="source"/> is null.</exception>
        public static IEnumerable<TSource> ToEnumerable<TSource>(this IObservable<TSource> source)
        {
            if (source == null)
                throw new ArgumentNullException("source");

            return s_impl.ToEnumerable<TSource>(source);
        }

        #endregion

        #region ToEvent

        /// <summary>
        /// Exposes an observable sequence as an object with an Action-based .NET event.
        /// </summary>
        /// <param name="source">Observable source sequence.</param>
        /// <returns>The event source object.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="source"/> is null.</exception>
        public static IEventSource<Unit> ToEvent(this IObservable<Unit> source)
        {
            if (source == null)
                throw new ArgumentNullException("source");

            return s_impl.ToEvent(source);
        }

        /// <summary>
        /// Exposes an observable sequence as an object with an Action&lt;TSource&gt;-based .NET event.
        /// </summary>
        /// <typeparam name="TSource">The type of the elements in the source sequence.</typeparam>
        /// <param name="source">Observable source sequence.</param>
        /// <returns>The event source object.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="source"/> is null.</exception>
        public static IEventSource<TSource> ToEvent<TSource>(this IObservable<TSource> source)
        {
            if (source == null)
                throw new ArgumentNullException("source");

            return s_impl.ToEvent<TSource>(source);
        }

        #endregion

        #region ToEventPattern

        /// <summary>
        /// Exposes an observable sequence as an object with a .NET event, conforming to the standard .NET event pattern.
        /// </summary>
        /// <typeparam name="TEventArgs">The type of the event data generated by the event.</typeparam>
        /// <param name="source">Observable source sequence.</param>
        /// <returns>The event source object.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="source"/> is null.</exception>
        public static IEventPatternSource<TEventArgs> ToEventPattern<TEventArgs>(this IObservable<EventPattern<TEventArgs>> source)
#if !NO_EVENTARGS_CONSTRAINT
            where TEventArgs : EventArgs
#endif
        {
            if (source == null)
                throw new ArgumentNullException("source");

            return s_impl.ToEventPattern<TEventArgs>(source);
        }

        #endregion

        #region + ToObservable +

        /// <summary>
        /// Converts an enumerable sequence to an observable sequence.
        /// </summary>
        /// <typeparam name="TSource">The type of the elements in the source sequence.</typeparam>
        /// <param name="source">Enumerable sequence to convert to an observable sequence.</param>
        /// <returns>The observable sequence whose elements are pulled from the given enumerable sequence.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="source"/> is null.</exception>
        public static IObservable<TSource> ToObservable<TSource>(this IEnumerable<TSource> source)
        {
            if (source == null)
                throw new ArgumentNullException("source");

            return s_impl.ToObservable<TSource>(source);
        }

        /// <summary>
        /// Converts an enumerable sequence to an observable sequence, using the specified scheduler to run the enumeration loop.
        /// </summary>
        /// <typeparam name="TSource">The type of the elements in the source sequence.</typeparam>
        /// <param name="source">Enumerable sequence to convert to an observable sequence.</param>
        /// <param name="scheduler">Scheduler to run the enumeration of the input sequence on.</param>
        /// <returns>The observable sequence whose elements are pulled from the given enumerable sequence.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="source"/> or <paramref name="scheduler"/> is null.</exception>
        public static IObservable<TSource> ToObservable<TSource>(this IEnumerable<TSource> source, IScheduler scheduler)
        {
            if (source == null)
                throw new ArgumentNullException("source");
            if (scheduler == null)
                throw new ArgumentNullException("scheduler");

            return s_impl.ToObservable<TSource>(source, scheduler);
        }

        #endregion
    }
}

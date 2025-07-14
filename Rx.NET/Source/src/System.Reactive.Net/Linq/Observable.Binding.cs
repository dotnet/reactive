﻿// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information. 

using System.Reactive.Concurrency;
using System.Reactive.Subjects;

namespace System.Reactive.Linq
{
    public static partial class Observable
    {
        #region + Multicast +

        /// <summary>
        /// Multicasts the source sequence notifications through the specified subject to the resulting connectable observable. Upon connection of the
        /// connectable observable, the subject is subscribed to the source exactly one, and messages are forwarded to the observers registered with
        /// the connectable observable. For specializations with fixed subject types, see Publish, PublishLast, and Replay.
        /// </summary>
        /// <typeparam name="TSource">The type of the elements in the source sequence.</typeparam>
        /// <typeparam name="TResult">The type of the elements in the result sequence.</typeparam>
        /// <param name="source">Source sequence whose elements will be pushed into the specified subject.</param>
        /// <param name="subject">Subject to push source elements into.</param>
        /// <returns>A connectable observable sequence that upon connection causes the source sequence to push results into the specified subject.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="source"/> or <paramref name="subject"/> is null.</exception>
        public static IConnectableObservable<TResult> Multicast<TSource, TResult>(this IObservable<TSource> source, ISubject<TSource, TResult> subject)
        {
            if (source == null)
            {
                throw new ArgumentNullException(nameof(source));
            }

            if (subject == null)
            {
                throw new ArgumentNullException(nameof(subject));
            }

            return s_impl.Multicast(source, subject);
        }

        /// <summary>
        /// Multicasts the source sequence notifications through an instantiated subject into all uses of the sequence within a selector function. Each
        /// subscription to the resulting sequence causes a separate multicast invocation, exposing the sequence resulting from the selector function's
        /// invocation. For specializations with fixed subject types, see Publish, PublishLast, and Replay.
        /// </summary>
        /// <typeparam name="TSource">The type of the elements in the source sequence.</typeparam>
        /// <typeparam name="TIntermediate">The type of the elements produced by the intermediate subject.</typeparam>
        /// <typeparam name="TResult">The type of the elements in the result sequence.</typeparam>
        /// <param name="source">Source sequence which will be multicasted in the specified selector function.</param>
        /// <param name="subjectSelector">Factory function to create an intermediate subject through which the source sequence's elements will be multicast to the selector function.</param>
        /// <param name="selector">Selector function which can use the multicasted source sequence subject to the policies enforced by the created subject.</param>
        /// <returns>An observable sequence that contains the elements of a sequence produced by multicasting the source sequence within a selector function.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="source"/> or <paramref name="subjectSelector"/> or <paramref name="selector"/> is null.</exception>
        public static IObservable<TResult> Multicast<TSource, TIntermediate, TResult>(this IObservable<TSource> source, Func<ISubject<TSource, TIntermediate>> subjectSelector, Func<IObservable<TIntermediate>, IObservable<TResult>> selector)
        {
            if (source == null)
            {
                throw new ArgumentNullException(nameof(source));
            }

            if (subjectSelector == null)
            {
                throw new ArgumentNullException(nameof(subjectSelector));
            }

            if (selector == null)
            {
                throw new ArgumentNullException(nameof(selector));
            }

            return s_impl.Multicast(source, subjectSelector, selector);
        }

        #endregion

        #region + Publish +

        /// <summary>
        /// Returns a connectable observable sequence that shares a single subscription to the underlying sequence.
        /// This operator is a specialization of Multicast using a regular <see cref="Subject{T}"/>.
        /// </summary>
        /// <typeparam name="TSource">The type of the elements in the source sequence.</typeparam>
        /// <param name="source">Source sequence whose elements will be multicasted through a single shared subscription.</param>
        /// <returns>A connectable observable sequence that shares a single subscription to the underlying sequence.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="source"/> is null.</exception>
        /// <remarks>Subscribers will receive all notifications of the source from the time of the subscription on.</remarks>
        /// <seealso cref="Subject{T}"/>
        public static IConnectableObservable<TSource> Publish<TSource>(this IObservable<TSource> source)
        {
            if (source == null)
            {
                throw new ArgumentNullException(nameof(source));
            }

            return s_impl.Publish(source);
        }

        /// <summary>
        /// Returns an observable sequence that is the result of invoking the selector on a connectable observable sequence that shares a single subscription to the underlying sequence.
        /// This operator is a specialization of Multicast using a regular <see cref="Subject{T}"/>.
        /// </summary>
        /// <typeparam name="TSource">The type of the elements in the source sequence.</typeparam>
        /// <typeparam name="TResult">The type of the elements in the result sequence.</typeparam>
        /// <param name="source">Source sequence whose elements will be multicasted through a single shared subscription.</param>
        /// <param name="selector">Selector function which can use the multicasted source sequence as many times as needed, without causing multiple subscriptions to the source sequence. Subscribers to the given source will receive all notifications of the source from the time of the subscription on.</param>
        /// <returns>An observable sequence that contains the elements of a sequence produced by multicasting the source sequence within a selector function.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="source"/> or <paramref name="selector"/> is null.</exception>
        /// <seealso cref="Subject{T}"/>
        public static IObservable<TResult> Publish<TSource, TResult>(this IObservable<TSource> source, Func<IObservable<TSource>, IObservable<TResult>> selector)
        {
            if (source == null)
            {
                throw new ArgumentNullException(nameof(source));
            }

            if (selector == null)
            {
                throw new ArgumentNullException(nameof(selector));
            }

            return s_impl.Publish(source, selector);
        }

        /// <summary>
        /// Returns a connectable observable sequence that shares a single subscription to the underlying sequence and starts with initialValue.
        /// This operator is a specialization of Multicast using a <see cref="BehaviorSubject{T}"/>.
        /// </summary>
        /// <typeparam name="TSource">The type of the elements in the source sequence.</typeparam>
        /// <param name="source">Source sequence whose elements will be multicasted through a single shared subscription.</param>
        /// <param name="initialValue">Initial value received by observers upon subscription.</param>
        /// <returns>A connectable observable sequence that shares a single subscription to the underlying sequence.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="source"/> is null.</exception>
        /// <remarks>Subscribers will receive immediately receive the initial value, followed by all notifications of the source from the time of the subscription on.</remarks>
        /// <seealso cref="BehaviorSubject{T}"/>
        public static IConnectableObservable<TSource> Publish<TSource>(this IObservable<TSource> source, TSource initialValue)
        {
            if (source == null)
            {
                throw new ArgumentNullException(nameof(source));
            }

            return s_impl.Publish(source, initialValue);
        }

        /// <summary>
        /// Returns an observable sequence that is the result of invoking the selector on a connectable observable sequence that shares a single subscription to the underlying sequence and starts with initialValue.
        /// This operator is a specialization of Multicast using a <see cref="BehaviorSubject{T}"/>.
        /// </summary>
        /// <typeparam name="TSource">The type of the elements in the source sequence.</typeparam>
        /// <typeparam name="TResult">The type of the elements in the result sequence.</typeparam>
        /// <param name="source">Source sequence whose elements will be multicasted through a single shared subscription.</param>
        /// <param name="selector">Selector function which can use the multicasted source sequence as many times as needed, without causing multiple subscriptions to the source sequence. Subscribers to the given source will receive immediately receive the initial value, followed by all notifications of the source from the time of the subscription on.</param>
        /// <param name="initialValue">Initial value received by observers upon subscription.</param>
        /// <returns>An observable sequence that contains the elements of a sequence produced by multicasting the source sequence within a selector function.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="source"/> or <paramref name="selector"/> is null.</exception>
        /// <seealso cref="BehaviorSubject{T}"/>
        public static IObservable<TResult> Publish<TSource, TResult>(this IObservable<TSource> source, Func<IObservable<TSource>, IObservable<TResult>> selector, TSource initialValue)
        {
            if (source == null)
            {
                throw new ArgumentNullException(nameof(source));
            }

            if (selector == null)
            {
                throw new ArgumentNullException(nameof(selector));
            }

            return s_impl.Publish(source, selector, initialValue);
        }

        #endregion

        #region + PublishLast +

        /// <summary>
        /// Returns a connectable observable sequence that shares a single subscription to the underlying sequence containing only the last notification.
        /// This operator is a specialization of Multicast using a <see cref="AsyncSubject{T}"/>.
        /// </summary>
        /// <typeparam name="TSource">The type of the elements in the source sequence.</typeparam>
        /// <param name="source">Source sequence whose elements will be multicasted through a single shared subscription.</param>
        /// <returns>A connectable observable sequence that shares a single subscription to the underlying sequence.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="source"/> is null.</exception>
        /// <remarks>Subscribers will only receive the last notification of the source.</remarks>
        /// <seealso cref="AsyncSubject{T}"/>
        public static IConnectableObservable<TSource> PublishLast<TSource>(this IObservable<TSource> source)
        {
            if (source == null)
            {
                throw new ArgumentNullException(nameof(source));
            }

            return s_impl.PublishLast(source);
        }

        /// <summary>
        /// Returns an observable sequence that is the result of invoking the selector on a connectable observable sequence that shares a single subscription to the underlying sequence containing only the last notification.
        /// This operator is a specialization of Multicast using a <see cref="AsyncSubject{T}"/>.
        /// </summary>
        /// <typeparam name="TSource">The type of the elements in the source sequence.</typeparam>
        /// <typeparam name="TResult">The type of the elements in the result sequence.</typeparam>
        /// <param name="source">Source sequence whose elements will be multicasted through a single shared subscription.</param>
        /// <param name="selector">Selector function which can use the multicasted source sequence as many times as needed, without causing multiple subscriptions to the source sequence. Subscribers to the given source will only receive the last notification of the source.</param>
        /// <returns>An observable sequence that contains the elements of a sequence produced by multicasting the source sequence within a selector function.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="source"/> or <paramref name="selector"/> is null.</exception>
        /// <seealso cref="AsyncSubject{T}"/>
        public static IObservable<TResult> PublishLast<TSource, TResult>(this IObservable<TSource> source, Func<IObservable<TSource>, IObservable<TResult>> selector)
        {
            if (source == null)
            {
                throw new ArgumentNullException(nameof(source));
            }

            if (selector == null)
            {
                throw new ArgumentNullException(nameof(selector));
            }

            return s_impl.PublishLast(source, selector);
        }

        #endregion

        #region + RefCount +

        /// <summary>
        /// Returns an observable sequence that stays connected to the source as long as there is at least one subscription to the observable sequence.
        /// </summary>
        /// <typeparam name="TSource">The type of the elements in the source sequence.</typeparam>
        /// <param name="source">Connectable observable sequence.</param>
        /// <returns>An observable sequence that stays connected to the source as long as there is at least one subscription to the observable sequence.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="source"/> is null.</exception>
        public static IObservable<TSource> RefCount<TSource>(this IConnectableObservable<TSource> source)
        {
            if (source == null)
            {
                throw new ArgumentNullException(nameof(source));
            }

            return s_impl.RefCount(source);
        }

        /// <summary>
        /// Returns an observable sequence that stays connected to the source as long as there is at least one subscription to the observable sequence.
        /// </summary>
        /// <typeparam name="TSource">The type of the elements in the source sequence.</typeparam>
        /// <param name="source">Connectable observable sequence.</param>
        /// <param name="disconnectDelay">The time span that should be waited before possibly unsubscribing from the connectable observable.</param>
        /// <returns>An observable sequence that stays connected to the source as long as there is at least one subscription to the observable sequence.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="source"/> is null.</exception>
        public static IObservable<TSource> RefCount<TSource>(this IConnectableObservable<TSource> source, TimeSpan disconnectDelay)
        {
            if (source == null)
            {
                throw new ArgumentNullException(nameof(source));
            }

            if (disconnectDelay < TimeSpan.Zero)
            {
                throw new ArgumentException("Delay cannot be less than zero", nameof(disconnectDelay));
            }

            return s_impl.RefCount(source, disconnectDelay);
        }

        /// <summary>
        /// Returns an observable sequence that stays connected to the source as long as there is at least one subscription to the observable sequence.
        /// </summary>
        /// <typeparam name="TSource">The type of the elements in the source sequence.</typeparam>
        /// <param name="source">Connectable observable sequence.</param>
        /// <param name="disconnectDelay">The time span that should be waited before possibly unsubscribing from the connectable observable.</param>
        /// <param name="scheduler">The scheduler to use for delayed unsubscription.</param>
        /// <returns>An observable sequence that stays connected to the source as long as there is at least one subscription to the observable sequence.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="source"/> is null.</exception>
        public static IObservable<TSource> RefCount<TSource>(this IConnectableObservable<TSource> source, TimeSpan disconnectDelay, IScheduler scheduler)
        {
            if (source == null)
            {
                throw new ArgumentNullException(nameof(source));
            }

            if (scheduler == null)
            {
                throw new ArgumentNullException(nameof(scheduler));
            }

            if (disconnectDelay < TimeSpan.Zero)
            {
                throw new ArgumentException("Delay cannot be less than zero", nameof(disconnectDelay));
            }

            return s_impl.RefCount(source, disconnectDelay, scheduler);
        }

        /// <summary>
        /// Returns an observable sequence that stays connected to the source as long as there is at least one subscription to the observable sequence.
        /// </summary>
        /// <typeparam name="TSource">The type of the elements in the source sequence.</typeparam>
        /// <param name="source">Connectable observable sequence.</param>
        /// <param name="minObservers">The minimum number of observers required to subscribe before establishing the connection to the source.</param>
        /// <returns>An observable sequence that stays connected to the source as long as there is at least one subscription to the observable sequence.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="source"/> is null.</exception>
        /// <exception cref="ArgumentOutOfRangeException"><paramref name="minObservers"/> is non-positive.</exception>
        public static IObservable<TSource> RefCount<TSource>(this IConnectableObservable<TSource> source, int minObservers)
        {
            if (source == null)
            {
                throw new ArgumentNullException(nameof(source));
            }
            if (minObservers <= 0)
            {
                throw new ArgumentOutOfRangeException(nameof(minObservers));
            }

            return s_impl.RefCount(source, minObservers);
        }

        /// <summary>
        /// Returns an observable sequence that stays connected to the source as long as there is at least one subscription to the observable sequence.
        /// </summary>
        /// <typeparam name="TSource">The type of the elements in the source sequence.</typeparam>
        /// <param name="source">Connectable observable sequence.</param>
        /// <param name="minObservers">The minimum number of observers required to subscribe before establishing the connection to the source.</param>
        /// <param name="disconnectDelay">The time span that should be waited before possibly unsubscribing from the connectable observable.</param>
        /// <returns>An observable sequence that stays connected to the source as long as there is at least one subscription to the observable sequence.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="source"/> is null.</exception>
        /// <exception cref="ArgumentOutOfRangeException"><paramref name="minObservers"/> is non-positive.</exception>
        public static IObservable<TSource> RefCount<TSource>(this IConnectableObservable<TSource> source, int minObservers, TimeSpan disconnectDelay)
        {
            if (source == null)
            {
                throw new ArgumentNullException(nameof(source));
            }

            if (disconnectDelay < TimeSpan.Zero)
            {
                throw new ArgumentException("Delay cannot be less than zero", nameof(disconnectDelay));
            }
            if (minObservers <= 0)
            {
                throw new ArgumentOutOfRangeException(nameof(minObservers));
            }

            return s_impl.RefCount(source, minObservers, disconnectDelay);
        }

        /// <summary>
        /// Returns an observable sequence that stays connected to the source as long as there is at least one subscription to the observable sequence.
        /// </summary>
        /// <typeparam name="TSource">The type of the elements in the source sequence.</typeparam>
        /// <param name="source">Connectable observable sequence.</param>
        /// <param name="minObservers">The minimum number of observers required to subscribe before establishing the connection to the source.</param>
        /// <param name="disconnectDelay">The time span that should be waited before possibly unsubscribing from the connectable observable.</param>
        /// <param name="scheduler">The scheduler to use for delayed unsubscription.</param>
        /// <returns>An observable sequence that stays connected to the source as long as there is at least one subscription to the observable sequence.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="source"/> is null.</exception>
        /// <exception cref="ArgumentOutOfRangeException"><paramref name="minObservers"/> is non-positive.</exception>
        public static IObservable<TSource> RefCount<TSource>(this IConnectableObservable<TSource> source, int minObservers, TimeSpan disconnectDelay, IScheduler scheduler)
        {
            if (source == null)
            {
                throw new ArgumentNullException(nameof(source));
            }

            if (scheduler == null)
            {
                throw new ArgumentNullException(nameof(scheduler));
            }

            if (disconnectDelay < TimeSpan.Zero)
            {
                throw new ArgumentException("Delay cannot be less than zero", nameof(disconnectDelay));
            }
            if (minObservers <= 0)
            {
                throw new ArgumentOutOfRangeException(nameof(minObservers));
            }


            return s_impl.RefCount(source, minObservers, disconnectDelay, scheduler);
        }

        #endregion

        #region + AutoConnect +

        /// <summary>
        /// Automatically connect the upstream IConnectableObservable at most once when the
        /// specified number of IObservers have subscribed to this IObservable.
        /// </summary>
        /// <typeparam name="TSource">The type of the elements in the source sequence.</typeparam>
        /// <param name="source">Connectable observable sequence.</param>
        /// <param name="minObservers">The number of observers required to subscribe before the connection to source happens, non-positive value will trigger an immediate subscription.</param>
        /// <param name="onConnect">If not null, the connection's IDisposable is provided to it.</param>
        /// <returns>An observable sequence that connects to the source at most once when the given number of observers have subscribed to it.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="source"/> is null.</exception>
        public static IObservable<TSource> AutoConnect<TSource>(this IConnectableObservable<TSource> source, int minObservers = 1, Action<IDisposable>? onConnect = null)
        {
            if (source == null)
            {
                throw new ArgumentNullException(nameof(source));
            }

            return s_impl.AutoConnect(source, minObservers, onConnect);
        }

        #endregion

        #region + Replay +

        /// <summary>
        /// Returns a connectable observable sequence that shares a single subscription to the underlying sequence replaying all notifications.
        /// This operator is a specialization of Multicast using a <see cref="ReplaySubject{T}"/>.
        /// </summary>
        /// <typeparam name="TSource">The type of the elements in the source sequence.</typeparam>
        /// <param name="source">Source sequence whose elements will be multicasted through a single shared subscription.</param>
        /// <returns>A connectable observable sequence that shares a single subscription to the underlying sequence.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="source"/> is null.</exception>
        /// <remarks>Subscribers will receive all the notifications of the source.</remarks>
        /// <seealso cref="ReplaySubject{T}"/>
        public static IConnectableObservable<TSource> Replay<TSource>(this IObservable<TSource> source)
        {
            if (source == null)
            {
                throw new ArgumentNullException(nameof(source));
            }

            return s_impl.Replay(source);
        }

        /// <summary>
        /// Returns a connectable observable sequence that shares a single subscription to the underlying sequence replaying all notifications.
        /// This operator is a specialization of Multicast using a <see cref="ReplaySubject{T}"/>.
        /// </summary>
        /// <typeparam name="TSource">The type of the elements in the source sequence.</typeparam>
        /// <param name="source">Source sequence whose elements will be multicasted through a single shared subscription.</param>
        /// <param name="scheduler">Scheduler where connected observers will be invoked on.</param>
        /// <returns>A connectable observable sequence that shares a single subscription to the underlying sequence.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="source"/> or <paramref name="scheduler"/> is null.</exception>
        /// <remarks>Subscribers will receive all the notifications of the source.</remarks>
        /// <seealso cref="ReplaySubject{T}"/>
        public static IConnectableObservable<TSource> Replay<TSource>(this IObservable<TSource> source, IScheduler scheduler)
        {
            if (source == null)
            {
                throw new ArgumentNullException(nameof(source));
            }

            if (scheduler == null)
            {
                throw new ArgumentNullException(nameof(scheduler));
            }

            return s_impl.Replay(source, scheduler);
        }

        /// <summary>
        /// Returns an observable sequence that is the result of invoking the selector on a connectable observable sequence that shares a single subscription to the underlying sequence replaying all notifications.
        /// This operator is a specialization of Multicast using a <see cref="ReplaySubject{T}"/>.
        /// </summary>
        /// <typeparam name="TSource">The type of the elements in the source sequence.</typeparam>
        /// <typeparam name="TResult">The type of the elements in the result sequence.</typeparam>
        /// <param name="source">Source sequence whose elements will be multicasted through a single shared subscription.</param>
        /// <param name="selector">Selector function which can use the multicasted source sequence as many times as needed, without causing multiple subscriptions to the source sequence. Subscribers to the given source will receive all the notifications of the source.</param>
        /// <returns>An observable sequence that contains the elements of a sequence produced by multicasting the source sequence within a selector function.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="source"/> or <paramref name="selector"/> is null.</exception>
        /// <seealso cref="ReplaySubject{T}"/>
        public static IObservable<TResult> Replay<TSource, TResult>(this IObservable<TSource> source, Func<IObservable<TSource>, IObservable<TResult>> selector)
        {
            if (source == null)
            {
                throw new ArgumentNullException(nameof(source));
            }

            if (selector == null)
            {
                throw new ArgumentNullException(nameof(selector));
            }

            return s_impl.Replay(source, selector);
        }

        /// <summary>
        /// Returns an observable sequence that is the result of invoking the selector on a connectable observable sequence that shares a single subscription to the underlying sequence replaying all notifications.
        /// This operator is a specialization of Multicast using a <see cref="ReplaySubject{T}"/>.
        /// </summary>
        /// <typeparam name="TSource">The type of the elements in the source sequence.</typeparam>
        /// <typeparam name="TResult">The type of the elements in the result sequence.</typeparam>
        /// <param name="source">Source sequence whose elements will be multicasted through a single shared subscription.</param>
        /// <param name="selector">Selector function which can use the multicasted source sequence as many times as needed, without causing multiple subscriptions to the source sequence. Subscribers to the given source will receive all the notifications of the source.</param>
        /// <param name="scheduler">Scheduler where connected observers within the selector function will be invoked on.</param>
        /// <returns>An observable sequence that contains the elements of a sequence produced by multicasting the source sequence within a selector function.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="source"/> or <paramref name="selector"/> or <paramref name="scheduler"/> is null.</exception>
        /// <seealso cref="ReplaySubject{T}"/>
        public static IObservable<TResult> Replay<TSource, TResult>(this IObservable<TSource> source, Func<IObservable<TSource>, IObservable<TResult>> selector, IScheduler scheduler)
        {
            if (source == null)
            {
                throw new ArgumentNullException(nameof(source));
            }

            if (selector == null)
            {
                throw new ArgumentNullException(nameof(selector));
            }

            if (scheduler == null)
            {
                throw new ArgumentNullException(nameof(scheduler));
            }

            return s_impl.Replay(source, selector, scheduler);
        }

        /// <summary>
        /// Returns a connectable observable sequence that shares a single subscription to the underlying sequence replaying notifications subject to a maximum time length for the replay buffer.
        /// This operator is a specialization of Multicast using a <see cref="ReplaySubject{T}"/>.
        /// </summary>
        /// <typeparam name="TSource">The type of the elements in the source sequence.</typeparam>
        /// <param name="source">Source sequence whose elements will be multicasted through a single shared subscription.</param>
        /// <param name="window">Maximum time length of the replay buffer.</param>
        /// <returns>A connectable observable sequence that shares a single subscription to the underlying sequence.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="source"/> is null.</exception>
        /// <exception cref="ArgumentOutOfRangeException"><paramref name="window"/> is less than TimeSpan.Zero.</exception>
        /// <remarks>Subscribers will receive all the notifications of the source subject to the specified replay buffer trimming policy.</remarks>
        /// <seealso cref="ReplaySubject{T}"/>
        public static IConnectableObservable<TSource> Replay<TSource>(this IObservable<TSource> source, TimeSpan window)
        {
            if (source == null)
            {
                throw new ArgumentNullException(nameof(source));
            }

            if (window < TimeSpan.Zero)
            {
                throw new ArgumentOutOfRangeException(nameof(window));
            }

            return s_impl.Replay(source, window);
        }

        /// <summary>
        /// Returns an observable sequence that is the result of invoking the selector on a connectable observable sequence that shares a single subscription to the underlying sequence replaying notifications subject to a maximum time length for the replay buffer.
        /// This operator is a specialization of Multicast using a <see cref="ReplaySubject{T}"/>.
        /// </summary>
        /// <typeparam name="TSource">The type of the elements in the source sequence.</typeparam>
        /// <typeparam name="TResult">The type of the elements in the result sequence.</typeparam>
        /// <param name="source">Source sequence whose elements will be multicasted through a single shared subscription.</param>
        /// <param name="selector">Selector function which can use the multicasted source sequence as many times as needed, without causing multiple subscriptions to the source sequence. Subscribers to the given source will receive all the notifications of the source subject to the specified replay buffer trimming policy.</param>
        /// <param name="window">Maximum time length of the replay buffer.</param>
        /// <returns>An observable sequence that contains the elements of a sequence produced by multicasting the source sequence within a selector function.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="source"/> or <paramref name="selector"/> is null.</exception>
        /// <exception cref="ArgumentOutOfRangeException"><paramref name="window"/> is less than TimeSpan.Zero.</exception>
        /// <seealso cref="ReplaySubject{T}"/>
        public static IObservable<TResult> Replay<TSource, TResult>(this IObservable<TSource> source, Func<IObservable<TSource>, IObservable<TResult>> selector, TimeSpan window)
        {
            if (source == null)
            {
                throw new ArgumentNullException(nameof(source));
            }

            if (selector == null)
            {
                throw new ArgumentNullException(nameof(selector));
            }

            if (window < TimeSpan.Zero)
            {
                throw new ArgumentOutOfRangeException(nameof(window));
            }

            return s_impl.Replay(source, selector, window);
        }

        /// <summary>
        /// Returns a connectable observable sequence that shares a single subscription to the underlying sequence replaying notifications subject to a maximum time length for the replay buffer.
        /// This operator is a specialization of Multicast using a <see cref="ReplaySubject{T}"/>.
        /// </summary>
        /// <typeparam name="TSource">The type of the elements in the source sequence.</typeparam>
        /// <param name="source">Source sequence whose elements will be multicasted through a single shared subscription.</param>
        /// <param name="window">Maximum time length of the replay buffer.</param>
        /// <param name="scheduler">Scheduler where connected observers will be invoked on.</param>
        /// <returns>A connectable observable sequence that shares a single subscription to the underlying sequence.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="source"/> or <paramref name="scheduler"/> is null.</exception>
        /// <exception cref="ArgumentOutOfRangeException"><paramref name="window"/> is less than TimeSpan.Zero.</exception>
        /// <remarks>Subscribers will receive all the notifications of the source subject to the specified replay buffer trimming policy.</remarks>
        /// <seealso cref="ReplaySubject{T}"/>
        public static IConnectableObservable<TSource> Replay<TSource>(this IObservable<TSource> source, TimeSpan window, IScheduler scheduler)
        {
            if (source == null)
            {
                throw new ArgumentNullException(nameof(source));
            }

            if (window < TimeSpan.Zero)
            {
                throw new ArgumentOutOfRangeException(nameof(window));
            }

            if (scheduler == null)
            {
                throw new ArgumentNullException(nameof(scheduler));
            }

            return s_impl.Replay(source, window, scheduler);
        }

        /// <summary>
        /// Returns an observable sequence that is the result of invoking the selector on a connectable observable sequence that shares a single subscription to the underlying sequence replaying notifications subject to a maximum time length for the replay buffer.
        /// This operator is a specialization of Multicast using a <see cref="ReplaySubject{T}"/>.
        /// </summary>
        /// <typeparam name="TSource">The type of the elements in the source sequence.</typeparam>
        /// <typeparam name="TResult">The type of the elements in the result sequence.</typeparam>
        /// <param name="source">Source sequence whose elements will be multicasted through a single shared subscription.</param>
        /// <param name="selector">Selector function which can use the multicasted source sequence as many times as needed, without causing multiple subscriptions to the source sequence. Subscribers to the given source will receive all the notifications of the source subject to the specified replay buffer trimming policy.</param>
        /// <param name="window">Maximum time length of the replay buffer.</param>
        /// <param name="scheduler">Scheduler where connected observers within the selector function will be invoked on.</param>
        /// <returns>An observable sequence that contains the elements of a sequence produced by multicasting the source sequence within a selector function.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="source"/> or <paramref name="selector"/> or <paramref name="scheduler"/> is null.</exception>
        /// <exception cref="ArgumentOutOfRangeException"><paramref name="window"/> is less than TimeSpan.Zero.</exception>
        /// <seealso cref="ReplaySubject{T}"/>
        public static IObservable<TResult> Replay<TSource, TResult>(this IObservable<TSource> source, Func<IObservable<TSource>, IObservable<TResult>> selector, TimeSpan window, IScheduler scheduler)
        {
            if (source == null)
            {
                throw new ArgumentNullException(nameof(source));
            }

            if (selector == null)
            {
                throw new ArgumentNullException(nameof(selector));
            }

            if (window < TimeSpan.Zero)
            {
                throw new ArgumentOutOfRangeException(nameof(window));
            }

            if (scheduler == null)
            {
                throw new ArgumentNullException(nameof(scheduler));
            }

            return s_impl.Replay(source, selector, window, scheduler);
        }

        /// <summary>
        /// Returns a connectable observable sequence that shares a single subscription to the underlying sequence replaying bufferSize notifications.
        /// This operator is a specialization of Multicast using a <see cref="ReplaySubject{T}"/>.
        /// </summary>
        /// <typeparam name="TSource">The type of the elements in the source sequence.</typeparam>
        /// <param name="source">Source sequence whose elements will be multicasted through a single shared subscription.</param>
        /// <param name="bufferSize">Maximum element count of the replay buffer.</param>
        /// <param name="scheduler">Scheduler where connected observers will be invoked on.</param>
        /// <returns>A connectable observable sequence that shares a single subscription to the underlying sequence.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="source"/> or <paramref name="scheduler"/> is null.</exception>
        /// <exception cref="ArgumentOutOfRangeException"><paramref name="bufferSize"/> is less than zero.</exception>
        /// <remarks>Subscribers will receive all the notifications of the source subject to the specified replay buffer trimming policy.</remarks>
        /// <seealso cref="ReplaySubject{T}"/>
        public static IConnectableObservable<TSource> Replay<TSource>(this IObservable<TSource> source, int bufferSize, IScheduler scheduler)
        {
            if (source == null)
            {
                throw new ArgumentNullException(nameof(source));
            }

            if (bufferSize < 0)
            {
                throw new ArgumentOutOfRangeException(nameof(bufferSize));
            }

            if (scheduler == null)
            {
                throw new ArgumentNullException(nameof(scheduler));
            }

            return s_impl.Replay(source, bufferSize, scheduler);
        }

        /// <summary>
        /// Returns an observable sequence that is the result of invoking the selector on a connectable observable sequence that shares a single subscription to the underlying sequence replaying notifications subject to a maximum element count for the replay buffer.
        /// This operator is a specialization of Multicast using a <see cref="ReplaySubject{T}"/>.
        /// </summary>
        /// <typeparam name="TSource">The type of the elements in the source sequence.</typeparam>
        /// <typeparam name="TResult">The type of the elements in the result sequence.</typeparam>
        /// <param name="source">Source sequence whose elements will be multicasted through a single shared subscription.</param>
        /// <param name="selector">Selector function which can use the multicasted source sequence as many times as needed, without causing multiple subscriptions to the source sequence. Subscribers to the given source will receive all the notifications of the source subject to the specified replay buffer trimming policy.</param>
        /// <param name="bufferSize">Maximum element count of the replay buffer.</param>
        /// <param name="scheduler">Scheduler where connected observers within the selector function will be invoked on.</param>
        /// <returns>An observable sequence that contains the elements of a sequence produced by multicasting the source sequence within a selector function.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="source"/> or <paramref name="selector"/> or <paramref name="scheduler"/> is null.</exception>
        /// <exception cref="ArgumentOutOfRangeException"><paramref name="bufferSize"/> is less than zero.</exception>
        /// <seealso cref="ReplaySubject{T}"/>
        public static IObservable<TResult> Replay<TSource, TResult>(this IObservable<TSource> source, Func<IObservable<TSource>, IObservable<TResult>> selector, int bufferSize, IScheduler scheduler)
        {
            if (source == null)
            {
                throw new ArgumentNullException(nameof(source));
            }

            if (selector == null)
            {
                throw new ArgumentNullException(nameof(selector));
            }

            if (bufferSize < 0)
            {
                throw new ArgumentOutOfRangeException(nameof(bufferSize));
            }

            if (scheduler == null)
            {
                throw new ArgumentNullException(nameof(scheduler));
            }

            return s_impl.Replay(source, selector, bufferSize, scheduler);
        }

        /// <summary>
        /// Returns a connectable observable sequence that shares a single subscription to the underlying sequence replaying notifications subject to a maximum element count for the replay buffer.
        /// This operator is a specialization of Multicast using a <see cref="ReplaySubject{T}"/>.
        /// </summary>
        /// <typeparam name="TSource">The type of the elements in the source sequence.</typeparam>
        /// <param name="source">Source sequence whose elements will be multicasted through a single shared subscription.</param>
        /// <param name="bufferSize">Maximum element count of the replay buffer.</param>
        /// <returns>A connectable observable sequence that shares a single subscription to the underlying sequence.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="source"/> is null.</exception>
        /// <exception cref="ArgumentOutOfRangeException"><paramref name="bufferSize"/> is less than zero.</exception>
        /// <remarks>Subscribers will receive all the notifications of the source subject to the specified replay buffer trimming policy.</remarks>
        /// <seealso cref="ReplaySubject{T}"/>
        public static IConnectableObservable<TSource> Replay<TSource>(this IObservable<TSource> source, int bufferSize)
        {
            if (source == null)
            {
                throw new ArgumentNullException(nameof(source));
            }

            if (bufferSize < 0)
            {
                throw new ArgumentOutOfRangeException(nameof(bufferSize));
            }

            return s_impl.Replay(source, bufferSize);
        }

        /// <summary>
        /// Returns an observable sequence that is the result of invoking the selector on a connectable observable sequence that shares a single subscription to the underlying sequence replaying notifications subject to a maximum element count for the replay buffer.
        /// This operator is a specialization of Multicast using a <see cref="ReplaySubject{T}"/>.
        /// </summary>
        /// <typeparam name="TSource">The type of the elements in the source sequence.</typeparam>
        /// <typeparam name="TResult">The type of the elements in the result sequence.</typeparam>
        /// <param name="source">Source sequence whose elements will be multicasted through a single shared subscription.</param>
        /// <param name="selector">Selector function which can use the multicasted source sequence as many times as needed, without causing multiple subscriptions to the source sequence. Subscribers to the given source will receive all the notifications of the source subject to the specified replay buffer trimming policy.</param>
        /// <param name="bufferSize">Maximum element count of the replay buffer.</param>
        /// <returns>An observable sequence that contains the elements of a sequence produced by multicasting the source sequence within a selector function.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="source"/> or <paramref name="selector"/> is null.</exception>
        /// <exception cref="ArgumentOutOfRangeException"><paramref name="bufferSize"/> is less than zero.</exception>
        /// <seealso cref="ReplaySubject{T}"/>
        public static IObservable<TResult> Replay<TSource, TResult>(this IObservable<TSource> source, Func<IObservable<TSource>, IObservable<TResult>> selector, int bufferSize)
        {
            if (source == null)
            {
                throw new ArgumentNullException(nameof(source));
            }

            if (selector == null)
            {
                throw new ArgumentNullException(nameof(selector));
            }

            if (bufferSize < 0)
            {
                throw new ArgumentOutOfRangeException(nameof(bufferSize));
            }

            return s_impl.Replay(source, selector, bufferSize);
        }

        /// <summary>
        /// Returns a connectable observable sequence that shares a single subscription to the underlying sequence replaying notifications subject to a maximum time length and element count for the replay buffer.
        /// This operator is a specialization of Multicast using a <see cref="ReplaySubject{T}"/>.
        /// </summary>
        /// <typeparam name="TSource">The type of the elements in the source sequence.</typeparam>
        /// <param name="source">Source sequence whose elements will be multicasted through a single shared subscription.</param>
        /// <param name="bufferSize">Maximum element count of the replay buffer.</param>
        /// <param name="window">Maximum time length of the replay buffer.</param>
        /// <returns>A connectable observable sequence that shares a single subscription to the underlying sequence.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="source"/> is null.</exception>
        /// <exception cref="ArgumentOutOfRangeException"><paramref name="bufferSize"/> is less than zero.</exception>
        /// <exception cref="ArgumentOutOfRangeException"><paramref name="window"/> is less than TimeSpan.Zero.</exception>
        /// <remarks>Subscribers will receive all the notifications of the source subject to the specified replay buffer trimming policy.</remarks>
        /// <seealso cref="ReplaySubject{T}"/>
        public static IConnectableObservable<TSource> Replay<TSource>(this IObservable<TSource> source, int bufferSize, TimeSpan window)
        {
            if (source == null)
            {
                throw new ArgumentNullException(nameof(source));
            }

            if (bufferSize < 0)
            {
                throw new ArgumentOutOfRangeException(nameof(bufferSize));
            }

            if (window < TimeSpan.Zero)
            {
                throw new ArgumentOutOfRangeException(nameof(window));
            }

            return s_impl.Replay(source, bufferSize, window);
        }

        /// <summary>
        /// Returns an observable sequence that is the result of invoking the selector on a connectable observable sequence that shares a single subscription to the underlying sequence replaying notifications subject to a maximum time length and element count for the replay buffer.
        /// This operator is a specialization of Multicast using a <see cref="ReplaySubject{T}"/>.
        /// </summary>
        /// <typeparam name="TSource">The type of the elements in the source sequence.</typeparam>
        /// <typeparam name="TResult">The type of the elements in the result sequence.</typeparam>
        /// <param name="source">Source sequence whose elements will be multicasted through a single shared subscription.</param>
        /// <param name="selector">Selector function which can use the multicasted source sequence as many times as needed, without causing multiple subscriptions to the source sequence. Subscribers to the given source will receive all the notifications of the source subject to the specified replay buffer trimming policy.</param>
        /// <param name="bufferSize">Maximum element count of the replay buffer.</param>
        /// <param name="window">Maximum time length of the replay buffer.</param>
        /// <returns>An observable sequence that contains the elements of a sequence produced by multicasting the source sequence within a selector function.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="source"/> or <paramref name="selector"/> is null.</exception>
        /// <exception cref="ArgumentOutOfRangeException"><paramref name="bufferSize"/> is less than zero.</exception>
        /// <exception cref="ArgumentOutOfRangeException"><paramref name="window"/> is less than TimeSpan.Zero.</exception>
        /// <seealso cref="ReplaySubject{T}"/>
        public static IObservable<TResult> Replay<TSource, TResult>(this IObservable<TSource> source, Func<IObservable<TSource>, IObservable<TResult>> selector, int bufferSize, TimeSpan window)
        {
            if (source == null)
            {
                throw new ArgumentNullException(nameof(source));
            }

            if (selector == null)
            {
                throw new ArgumentNullException(nameof(selector));
            }

            if (bufferSize < 0)
            {
                throw new ArgumentOutOfRangeException(nameof(bufferSize));
            }

            if (window < TimeSpan.Zero)
            {
                throw new ArgumentOutOfRangeException(nameof(window));
            }

            return s_impl.Replay(source, selector, bufferSize, window);
        }

        /// <summary>
        /// Returns a connectable observable sequence that shares a single subscription to the underlying sequence replaying notifications subject to a maximum time length and element count for the replay buffer.
        /// This operator is a specialization of Multicast using a <see cref="ReplaySubject{T}"/>.
        /// </summary>
        /// <typeparam name="TSource">The type of the elements in the source sequence.</typeparam>
        /// <param name="source">Source sequence whose elements will be multicasted through a single shared subscription.</param>
        /// <param name="bufferSize">Maximum element count of the replay buffer.</param>
        /// <param name="window">Maximum time length of the replay buffer.</param>
        /// <param name="scheduler">Scheduler where connected observers will be invoked on.</param>
        /// <returns>A connectable observable sequence that shares a single subscription to the underlying sequence.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="source"/> or <paramref name="scheduler"/> is null.</exception>
        /// <exception cref="ArgumentOutOfRangeException"><paramref name="bufferSize"/> is less than zero.</exception>
        /// <exception cref="ArgumentOutOfRangeException"><paramref name="window"/> is less than TimeSpan.Zero.</exception>
        /// <remarks>Subscribers will receive all the notifications of the source subject to the specified replay buffer trimming policy.</remarks>
        /// <seealso cref="ReplaySubject{T}"/>
        public static IConnectableObservable<TSource> Replay<TSource>(this IObservable<TSource> source, int bufferSize, TimeSpan window, IScheduler scheduler)
        {
            if (source == null)
            {
                throw new ArgumentNullException(nameof(source));
            }

            if (bufferSize < 0)
            {
                throw new ArgumentOutOfRangeException(nameof(bufferSize));
            }

            if (window < TimeSpan.Zero)
            {
                throw new ArgumentOutOfRangeException(nameof(window));
            }

            if (scheduler == null)
            {
                throw new ArgumentNullException(nameof(scheduler));
            }

            return s_impl.Replay(source, bufferSize, window, scheduler);
        }

        /// <summary>
        /// Returns an observable sequence that is the result of invoking the selector on a connectable observable sequence that shares a single subscription to the underlying sequence replaying notifications subject to a maximum time length and element count for the replay buffer.
        /// This operator is a specialization of Multicast using a <see cref="ReplaySubject{T}"/>.
        /// </summary>
        /// <typeparam name="TSource">The type of the elements in the source sequence.</typeparam>
        /// <typeparam name="TResult">The type of the elements in the result sequence.</typeparam>
        /// <param name="source">Source sequence whose elements will be multicasted through a single shared subscription.</param>
        /// <param name="selector">Selector function which can use the multicasted source sequence as many times as needed, without causing multiple subscriptions to the source sequence. Subscribers to the given source will receive all the notifications of the source subject to the specified replay buffer trimming policy.</param>
        /// <param name="bufferSize">Maximum element count of the replay buffer.</param>
        /// <param name="window">Maximum time length of the replay buffer.</param>
        /// <param name="scheduler">Scheduler where connected observers within the selector function will be invoked on.</param>
        /// <returns>An observable sequence that contains the elements of a sequence produced by multicasting the source sequence within a selector function.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="source"/> or <paramref name="selector"/> or <paramref name="scheduler"/> is null.</exception>
        /// <exception cref="ArgumentOutOfRangeException"><paramref name="bufferSize"/> is less than zero.</exception>
        /// <exception cref="ArgumentOutOfRangeException"><paramref name="window"/> is less than TimeSpan.Zero.</exception>
        /// <seealso cref="ReplaySubject{T}"/>
        public static IObservable<TResult> Replay<TSource, TResult>(this IObservable<TSource> source, Func<IObservable<TSource>, IObservable<TResult>> selector, int bufferSize, TimeSpan window, IScheduler scheduler)
        {
            if (source == null)
            {
                throw new ArgumentNullException(nameof(source));
            }

            if (selector == null)
            {
                throw new ArgumentNullException(nameof(selector));
            }

            if (bufferSize < 0)
            {
                throw new ArgumentOutOfRangeException(nameof(bufferSize));
            }

            if (window < TimeSpan.Zero)
            {
                throw new ArgumentOutOfRangeException(nameof(window));
            }

            if (scheduler == null)
            {
                throw new ArgumentNullException(nameof(scheduler));
            }

            return s_impl.Replay(source, selector, bufferSize, window, scheduler);
        }

        #endregion
    }
}

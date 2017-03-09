// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the Apache 2.0 License.
// See the LICENSE file in the project root for more information. 

using System.Collections.Generic;
using System.Reactive.Concurrency;

namespace System.Reactive.Linq
{
    public static partial class Observable
    {
        #region + Buffer +

        #region TimeSpan only

        /// <summary>
        /// Projects each element of an observable sequence into consecutive non-overlapping buffers which are produced based on timing information.
        /// </summary>
        /// <typeparam name="TSource">The type of the elements in the source sequence, and in the lists in the result sequence.</typeparam>
        /// <param name="source">Source sequence to produce buffers over.</param>
        /// <param name="timeSpan">Length of each buffer.</param>
        /// <returns>An observable sequence of buffers.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="source"/> is null.</exception>
        /// <exception cref="ArgumentOutOfRangeException"><paramref name="timeSpan"/> is less than TimeSpan.Zero.</exception>
        /// <remarks>
        /// Specifying a TimeSpan.Zero value for <paramref name="timeSpan"/> is not recommended but supported, causing the scheduler to create buffers as fast as it can.
        /// Because all source sequence elements end up in one of the buffers, some buffers won't have a zero time span. This is a side-effect of the asynchrony introduced
        /// by the scheduler, where the action to close the current buffer and to create a new buffer may not execute immediately, despite the TimeSpan.Zero due time.
        /// </remarks>
        public static IObservable<IList<TSource>> Buffer<TSource>(this IObservable<TSource> source, TimeSpan timeSpan)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));
            if (timeSpan < TimeSpan.Zero)
                throw new ArgumentOutOfRangeException(nameof(timeSpan));

            return s_impl.Buffer<TSource>(source, timeSpan);
        }

        /// <summary>
        /// Projects each element of an observable sequence into consecutive non-overlapping buffers which are produced based on timing information, using the specified scheduler to run timers.
        /// </summary>
        /// <typeparam name="TSource">The type of the elements in the source sequence, and in the lists in the result sequence.</typeparam>
        /// <param name="source">Source sequence to produce buffers over.</param>
        /// <param name="timeSpan">Length of each buffer.</param>
        /// <param name="scheduler">Scheduler to run buffering timers on.</param>
        /// <returns>An observable sequence of buffers.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="source"/> or <paramref name="scheduler"/> is null.</exception>
        /// <exception cref="ArgumentOutOfRangeException"><paramref name="timeSpan"/> is less than TimeSpan.Zero.</exception>
        /// <remarks>
        /// Specifying a TimeSpan.Zero value for <paramref name="timeSpan"/> is not recommended but supported, causing the scheduler to create buffers as fast as it can.
        /// Because all source sequence elements end up in one of the buffers, some buffers won't have a zero time span. This is a side-effect of the asynchrony introduced
        /// by the scheduler, where the action to close the current buffer and to create a new buffer may not execute immediately, despite the TimeSpan.Zero due time.
        /// </remarks>
        public static IObservable<IList<TSource>> Buffer<TSource>(this IObservable<TSource> source, TimeSpan timeSpan, IScheduler scheduler)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));
            if (timeSpan < TimeSpan.Zero)
                throw new ArgumentOutOfRangeException(nameof(timeSpan));
            if (scheduler == null)
                throw new ArgumentNullException(nameof(scheduler));

            return s_impl.Buffer<TSource>(source, timeSpan, scheduler);
        }

        /// <summary>
        /// Projects each element of an observable sequence into zero or more buffers which are produced based on timing information.
        /// </summary>
        /// <typeparam name="TSource">The type of the elements in the source sequence, and in the lists in the result sequence.</typeparam>
        /// <param name="source">Source sequence to produce buffers over.</param>
        /// <param name="timeSpan">Length of each buffer.</param>
        /// <param name="timeShift">Interval between creation of consecutive buffers.</param>
        /// <returns>An observable sequence of buffers.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="source"/> is null.</exception>
        /// <exception cref="ArgumentOutOfRangeException"><paramref name="timeSpan"/> or <paramref name="timeSpan"/> is less than TimeSpan.Zero.</exception>
        /// <remarks>
        /// <para>
        /// Specifying a TimeSpan.Zero value for <paramref name="timeSpan"/> is not recommended but supported, causing the scheduler to create buffers with minimum duration
        /// length. However, some buffers won't have a zero time span. This is a side-effect of the asynchrony introduced by the scheduler, where the action to close the
        /// current buffer may not execute immediately, despite the TimeSpan.Zero due time.
        /// </para>
        /// <para>
        /// Specifying a TimeSpan.Zero value for <paramref name="timeShift"/> is not recommended but supported, causing the scheduler to create buffers as fast as it can.
        /// However, this doesn't mean all buffers will start at the beginning of the source sequence. This is a side-effect of the asynchrony introduced by the scheduler,
        /// where the action to create a new buffer may not execute immediately, despite the TimeSpan.Zero due time.
        /// </para>
        /// </remarks>
        public static IObservable<IList<TSource>> Buffer<TSource>(this IObservable<TSource> source, TimeSpan timeSpan, TimeSpan timeShift)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));
            if (timeSpan < TimeSpan.Zero)
                throw new ArgumentOutOfRangeException(nameof(timeSpan));
            if (timeShift < TimeSpan.Zero)
                throw new ArgumentOutOfRangeException(nameof(timeShift));

            return s_impl.Buffer<TSource>(source, timeSpan, timeShift);
        }

        /// <summary>
        /// Projects each element of an observable sequence into zero or more buffers which are produced based on timing information, using the specified scheduler to run timers.
        /// </summary>
        /// <typeparam name="TSource">The type of the elements in the source sequence, and in the lists in the result sequence.</typeparam>
        /// <param name="source">Source sequence to produce buffers over.</param>
        /// <param name="timeSpan">Length of each buffer.</param>
        /// <param name="timeShift">Interval between creation of consecutive buffers.</param>
        /// <param name="scheduler">Scheduler to run buffering timers on.</param>
        /// <returns>An observable sequence of buffers.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="source"/> or <paramref name="scheduler"/> is null.</exception>
        /// <exception cref="ArgumentOutOfRangeException"><paramref name="timeSpan"/> or <paramref name="timeSpan"/> is less than TimeSpan.Zero.</exception>
        /// <remarks>
        /// <para>
        /// Specifying a TimeSpan.Zero value for <paramref name="timeSpan"/> is not recommended but supported, causing the scheduler to create buffers with minimum duration
        /// length. However, some buffers won't have a zero time span. This is a side-effect of the asynchrony introduced by the scheduler, where the action to close the
        /// current buffer may not execute immediately, despite the TimeSpan.Zero due time.
        /// </para>
        /// <para>
        /// Specifying a TimeSpan.Zero value for <paramref name="timeShift"/> is not recommended but supported, causing the scheduler to create buffers as fast as it can.
        /// However, this doesn't mean all buffers will start at the beginning of the source sequence. This is a side-effect of the asynchrony introduced by the scheduler,
        /// where the action to create a new buffer may not execute immediately, despite the TimeSpan.Zero due time.
        /// </para>
        /// </remarks>
        public static IObservable<IList<TSource>> Buffer<TSource>(this IObservable<TSource> source, TimeSpan timeSpan, TimeSpan timeShift, IScheduler scheduler)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));
            if (timeSpan < TimeSpan.Zero)
                throw new ArgumentOutOfRangeException(nameof(timeSpan));
            if (timeShift < TimeSpan.Zero)
                throw new ArgumentOutOfRangeException(nameof(timeShift));
            if (scheduler == null)
                throw new ArgumentNullException(nameof(scheduler));

            return s_impl.Buffer<TSource>(source, timeSpan, timeShift, scheduler);
        }

        #endregion

        #region TimeSpan + int

        /// <summary>
        /// Projects each element of an observable sequence into a buffer that's sent out when either it's full or a given amount of time has elapsed.
        /// A useful real-world analogy of this overload is the behavior of a ferry leaving the dock when all seats are taken, or at the scheduled time of departure, whichever event occurs first.
        /// </summary>
        /// <typeparam name="TSource">The type of the elements in the source sequence, and in the lists in the result sequence.</typeparam>
        /// <param name="source">Source sequence to produce buffers over.</param>
        /// <param name="timeSpan">Maximum time length of a window.</param>
        /// <param name="count">Maximum element count of a window.</param>
        /// <returns>An observable sequence of buffers.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="source"/> is null.</exception>
        /// <exception cref="ArgumentOutOfRangeException"><paramref name="timeSpan"/> is less than TimeSpan.Zero. -or- <paramref name="count"/> is less than or equal to zero.</exception>
        /// <remarks>
        /// Specifying a TimeSpan.Zero value for <paramref name="timeSpan"/> is not recommended but supported, causing the scheduler to create buffers as fast as it can.
        /// Because all source sequence elements end up in one of the buffers, some buffers won't have a zero time span. This is a side-effect of the asynchrony introduced
        /// by the scheduler, where the action to close the current buffer and to create a new buffer may not execute immediately, despite the TimeSpan.Zero due time.
        /// </remarks>
        public static IObservable<IList<TSource>> Buffer<TSource>(this IObservable<TSource> source, TimeSpan timeSpan, int count)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));
            if (timeSpan < TimeSpan.Zero)
                throw new ArgumentOutOfRangeException(nameof(timeSpan));
            if (count <= 0)
                throw new ArgumentOutOfRangeException(nameof(count));

            return s_impl.Buffer<TSource>(source, timeSpan, count);
        }

        /// <summary>
        /// Projects each element of an observable sequence into a buffer that's sent out when either it's full or a given amount of time has elapsed, using the specified scheduler to run timers.
        /// A useful real-world analogy of this overload is the behavior of a ferry leaving the dock when all seats are taken, or at the scheduled time of departure, whichever event occurs first.
        /// </summary>
        /// <typeparam name="TSource">The type of the elements in the source sequence, and in the lists in the result sequence.</typeparam>
        /// <param name="source">Source sequence to produce buffers over.</param>
        /// <param name="timeSpan">Maximum time length of a buffer.</param>
        /// <param name="count">Maximum element count of a buffer.</param>
        /// <param name="scheduler">Scheduler to run buffering timers on.</param>
        /// <returns>An observable sequence of buffers.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="source"/> or <paramref name="scheduler"/> is null.</exception>
        /// <exception cref="ArgumentOutOfRangeException"><paramref name="timeSpan"/> is less than TimeSpan.Zero. -or- <paramref name="count"/> is less than or equal to zero.</exception>
        /// <remarks>
        /// Specifying a TimeSpan.Zero value for <paramref name="timeSpan"/> is not recommended but supported, causing the scheduler to create buffers as fast as it can.
        /// Because all source sequence elements end up in one of the buffers, some buffers won't have a zero time span. This is a side-effect of the asynchrony introduced
        /// by the scheduler, where the action to close the current buffer and to create a new buffer may not execute immediately, despite the TimeSpan.Zero due time.
        /// </remarks>
        public static IObservable<IList<TSource>> Buffer<TSource>(this IObservable<TSource> source, TimeSpan timeSpan, int count, IScheduler scheduler)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));
            if (timeSpan < TimeSpan.Zero)
                throw new ArgumentOutOfRangeException(nameof(timeSpan));
            if (count <= 0)
                throw new ArgumentOutOfRangeException(nameof(count));
            if (scheduler == null)
                throw new ArgumentNullException(nameof(scheduler));

            return s_impl.Buffer<TSource>(source, timeSpan, count, scheduler);
        }

        #endregion

        #endregion

        #region + Delay +

        #region TimeSpan

        /// <summary>
        /// Time shifts the observable sequence by the specified relative time duration.
        /// The relative time intervals between the values are preserved.
        /// </summary>
        /// <typeparam name="TSource">The type of the elements in the source sequence.</typeparam>
        /// <param name="source">Source sequence to delay values for.</param>
        /// <param name="dueTime">Relative time by which to shift the observable sequence. If this value is equal to TimeSpan.Zero, the scheduler will dispatch observer callbacks as soon as possible.</param>
        /// <returns>Time-shifted sequence.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="source"/> is null.</exception>
        /// <exception cref="ArgumentOutOfRangeException"><paramref name="dueTime"/> is less than TimeSpan.Zero.</exception>
        /// <remarks>
        /// <para>
        /// This operator is less efficient than <see cref="Observable.DelaySubscription{T}(IObservable{T}, TimeSpan)">DelaySubscription</see> because it records all notifications and time-delays those. This allows for immediate propagation of errors.
        /// </para>
        /// <para>
        /// Observer callbacks for the resulting sequence will be run on the default scheduler. This effect is similar to using ObserveOn.
        /// </para>
        /// <para>
        /// Exceptions signaled by the source sequence through an OnError callback are forwarded immediately to the result sequence. Any OnNext notifications that were in the queue at the point of the OnError callback will be dropped.
        /// In order to delay error propagation, consider using the <see cref="Observable.Materialize">Observable.Materialize</see> and <see cref="Observable.Dematerialize">Observable.Dematerialize</see> operators, or use <see cref="Observable.DelaySubscription{T}(IObservable{T}, TimeSpan)">DelaySubscription</see>.
        /// </para>
        /// </remarks>
        public static IObservable<TSource> Delay<TSource>(this IObservable<TSource> source, TimeSpan dueTime)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));
            if (dueTime < TimeSpan.Zero)
                throw new ArgumentOutOfRangeException(nameof(dueTime));

            return s_impl.Delay<TSource>(source, dueTime);
        }

        /// <summary>
        /// Time shifts the observable sequence by the specified relative time duration, using the specified scheduler to run timers.
        /// The relative time intervals between the values are preserved.
        /// </summary>
        /// <typeparam name="TSource">The type of the elements in the source sequence.</typeparam>
        /// <param name="source">Source sequence to delay values for.</param>
        /// <param name="dueTime">Relative time by which to shift the observable sequence. If this value is equal to TimeSpan.Zero, the scheduler will dispatch observer callbacks as soon as possible.</param>
        /// <param name="scheduler">Scheduler to run the delay timers on.</param>
        /// <returns>Time-shifted sequence.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="source"/> or <paramref name="scheduler"/> is null.</exception>
        /// <exception cref="ArgumentOutOfRangeException"><paramref name="dueTime"/> is less than TimeSpan.Zero.</exception>
        /// <remarks>
        /// <para>
        /// This operator is less efficient than <see cref="Observable.DelaySubscription{T}(IObservable{T}, TimeSpan, IScheduler)">DelaySubscription</see> because it records all notifications and time-delays those. This allows for immediate propagation of errors.
        /// </para>
        /// <para>
        /// Observer callbacks for the resulting sequence will be run on the specified scheduler. This effect is similar to using ObserveOn.
        /// </para>
        /// <para>
        /// Exceptions signaled by the source sequence through an OnError callback are forwarded immediately to the result sequence. Any OnNext notifications that were in the queue at the point of the OnError callback will be dropped.
        /// </para>
        /// <para>
        /// Exceptions signaled by the source sequence through an OnError callback are forwarded immediately to the result sequence. Any OnNext notifications that were in the queue at the point of the OnError callback will be dropped.
        /// In order to delay error propagation, consider using the <see cref="Observable.Materialize">Observable.Materialize</see> and <see cref="Observable.Dematerialize">Observable.Dematerialize</see> operators, or use <see cref="Observable.DelaySubscription{T}(IObservable{T}, TimeSpan, IScheduler)">DelaySubscription</see>.
        /// </para>
        /// </remarks>
        public static IObservable<TSource> Delay<TSource>(this IObservable<TSource> source, TimeSpan dueTime, IScheduler scheduler)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));
            if (dueTime < TimeSpan.Zero)
                throw new ArgumentOutOfRangeException(nameof(dueTime));
            if (scheduler == null)
                throw new ArgumentNullException(nameof(scheduler));

            return s_impl.Delay<TSource>(source, dueTime, scheduler);
        }

        #endregion

        #region DateTimeOffset

        /// <summary>
        /// Time shifts the observable sequence to start propagating notifications at the specified absolute time.
        /// The relative time intervals between the values are preserved.
        /// </summary>
        /// <typeparam name="TSource">The type of the elements in the source sequence.</typeparam>
        /// <param name="source">Source sequence to delay values for.</param>
        /// <param name="dueTime">Absolute time used to shift the observable sequence; the relative time shift gets computed upon subscription. If this value is less than or equal to DateTimeOffset.UtcNow, the scheduler will dispatch observer callbacks as soon as possible.</param>
        /// <returns>Time-shifted sequence.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="source"/> is null.</exception>
        /// <remarks>
        /// <para>
        /// This operator is less efficient than <see cref="Observable.DelaySubscription{T}(IObservable{T}, DateTimeOffset)">DelaySubscription</see> because it records all notifications and time-delays those. This allows for immediate propagation of errors.
        /// </para>
        /// <para>
        /// Observer callbacks for the resulting sequence will be run on the default scheduler. This effect is similar to using ObserveOn.
        /// </para>
        /// <para>
        /// Exceptions signaled by the source sequence through an OnError callback are forwarded immediately to the result sequence. Any OnNext notifications that were in the queue at the point of the OnError callback will be dropped.
        /// In order to delay error propagation, consider using the <see cref="Observable.Materialize">Observable.Materialize</see> and <see cref="Observable.Dematerialize">Observable.Dematerialize</see> operators, or use <see cref="Observable.DelaySubscription{T}(IObservable{T}, DateTimeOffset)">DelaySubscription</see>.
        /// </para>
        /// </remarks>
        public static IObservable<TSource> Delay<TSource>(this IObservable<TSource> source, DateTimeOffset dueTime)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));

            return s_impl.Delay<TSource>(source, dueTime);
        }

        /// <summary>
        /// Time shifts the observable sequence to start propagating notifications at the specified absolute time, using the specified scheduler to run timers.
        /// The relative time intervals between the values are preserved.
        /// </summary>
        /// <typeparam name="TSource">The type of the elements in the source sequence.</typeparam>
        /// <param name="source">Source sequence to delay values for.</param>
        /// <param name="dueTime">Absolute time used to shift the observable sequence; the relative time shift gets computed upon subscription. If this value is less than or equal to DateTimeOffset.UtcNow, the scheduler will dispatch observer callbacks as soon as possible.</param>
        /// <param name="scheduler">Scheduler to run the delay timers on.</param>
        /// <returns>Time-shifted sequence.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="source"/> or <paramref name="scheduler"/> is null.</exception>
        /// <remarks>
        /// <para>
        /// This operator is less efficient than <see cref="Observable.DelaySubscription{T}(IObservable{T}, DateTimeOffset, IScheduler)">DelaySubscription</see> because it records all notifications and time-delays those. This allows for immediate propagation of errors.
        /// </para>
        /// <para>
        /// Observer callbacks for the resulting sequence will be run on the specified scheduler. This effect is similar to using ObserveOn.
        /// </para>
        /// <para>
        /// Exceptions signaled by the source sequence through an OnError callback are forwarded immediately to the result sequence. Any OnNext notifications that were in the queue at the point of the OnError callback will be dropped.
        /// In order to delay error propagation, consider using the <see cref="Observable.Materialize">Observable.Materialize</see> and <see cref="Observable.Dematerialize">Observable.Dematerialize</see> operators, or use <see cref="Observable.DelaySubscription{T}(IObservable{T}, DateTimeOffset, IScheduler)">DelaySubscription</see>.
        /// </para>
        /// </remarks>
        public static IObservable<TSource> Delay<TSource>(this IObservable<TSource> source, DateTimeOffset dueTime, IScheduler scheduler)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));
            if (scheduler == null)
                throw new ArgumentNullException(nameof(scheduler));

            return s_impl.Delay<TSource>(source, dueTime, scheduler);
        }

        #endregion

        #region Duration selector

        /// <summary>
        /// Time shifts the observable sequence based on a delay selector function for each element.
        /// </summary>
        /// <typeparam name="TSource">The type of the elements in the source sequence.</typeparam>
        /// <typeparam name="TDelay">The type of the elements in the delay sequences used to denote the delay duration of each element in the source sequence.</typeparam>
        /// <param name="source">Source sequence to delay values for.</param>
        /// <param name="delayDurationSelector">Selector function to retrieve a sequence indicating the delay for each given element.</param>
        /// <returns>Time-shifted sequence.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="source"/> or <paramref name="delayDurationSelector"/> is null.</exception>
        public static IObservable<TSource> Delay<TSource, TDelay>(this IObservable<TSource> source, Func<TSource, IObservable<TDelay>> delayDurationSelector)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));
            if (delayDurationSelector == null)
                throw new ArgumentNullException(nameof(delayDurationSelector));

            return s_impl.Delay<TSource, TDelay>(source, delayDurationSelector);
        }

        /// <summary>
        /// Time shifts the observable sequence based on a subscription delay and a delay selector function for each element.
        /// </summary>
        /// <typeparam name="TSource">The type of the elements in the source sequence.</typeparam>
        /// <typeparam name="TDelay">The type of the elements in the delay sequences used to denote the delay duration of each element in the source sequence.</typeparam>
        /// <param name="source">Source sequence to delay values for.</param>
        /// <param name="subscriptionDelay">Sequence indicating the delay for the subscription to the source.</param>
        /// <param name="delayDurationSelector">Selector function to retrieve a sequence indicating the delay for each given element.</param>
        /// <returns>Time-shifted sequence.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="source"/> or <paramref name="subscriptionDelay"/> or <paramref name="delayDurationSelector"/> is null.</exception>
        public static IObservable<TSource> Delay<TSource, TDelay>(this IObservable<TSource> source, IObservable<TDelay> subscriptionDelay, Func<TSource, IObservable<TDelay>> delayDurationSelector)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));
            if (subscriptionDelay == null)
                throw new ArgumentNullException(nameof(subscriptionDelay));
            if (delayDurationSelector == null)
                throw new ArgumentNullException(nameof(delayDurationSelector));

            return s_impl.Delay<TSource, TDelay>(source, subscriptionDelay, delayDurationSelector);
        }

        #endregion

        #endregion

        #region + DelaySubscription +

        /// <summary>
        /// Time shifts the observable sequence by delaying the subscription with the specified relative time duration.
        /// </summary>
        /// <typeparam name="TSource">The type of the elements in the source sequence.</typeparam>
        /// <param name="source">Source sequence to delay subscription for.</param>
        /// <param name="dueTime">Relative time shift of the subscription.</param>
        /// <returns>Time-shifted sequence.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="source"/> is null.</exception>
        /// <exception cref="ArgumentOutOfRangeException"><paramref name="dueTime"/> is less than TimeSpan.Zero.</exception>
        /// <remarks>
        /// <para>
        /// This operator is more efficient than <see cref="Observable.Delay{T}(IObservable{T}, TimeSpan)">Delay</see> but postpones all side-effects of subscription and affects error propagation timing.
        /// </para>
        /// <para>
        /// The side-effects of subscribing to the source sequence will be run on the default scheduler. Observer callbacks will not be affected.
        /// </para>
        /// </remarks>
        public static IObservable<TSource> DelaySubscription<TSource>(this IObservable<TSource> source, TimeSpan dueTime)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));
            if (dueTime < TimeSpan.Zero)
                throw new ArgumentOutOfRangeException(nameof(dueTime));

            return s_impl.DelaySubscription<TSource>(source, dueTime);
        }

        /// <summary>
        /// Time shifts the observable sequence by delaying the subscription with the specified relative time duration, using the specified scheduler to run timers.
        /// </summary>
        /// <typeparam name="TSource">The type of the elements in the source sequence.</typeparam>
        /// <param name="source">Source sequence to delay subscription for.</param>
        /// <param name="dueTime">Relative time shift of the subscription.</param>
        /// <param name="scheduler">Scheduler to run the subscription delay timer on.</param>
        /// <returns>Time-shifted sequence.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="source"/> or <paramref name="scheduler"/> is null.</exception>
        /// <exception cref="ArgumentOutOfRangeException"><paramref name="dueTime"/> is less than TimeSpan.Zero.</exception>
        /// <remarks>
        /// <para>
        /// This operator is more efficient than <see cref="Observable.Delay{T}(IObservable{T}, TimeSpan, IScheduler)">Delay</see> but postpones all side-effects of subscription and affects error propagation timing.
        /// </para>
        /// <para>
        /// The side-effects of subscribing to the source sequence will be run on the specified scheduler. Observer callbacks will not be affected.
        /// </para>
        /// </remarks>
        public static IObservable<TSource> DelaySubscription<TSource>(this IObservable<TSource> source, TimeSpan dueTime, IScheduler scheduler)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));
            if (dueTime < TimeSpan.Zero)
                throw new ArgumentOutOfRangeException(nameof(dueTime));
            if (scheduler == null)
                throw new ArgumentNullException(nameof(scheduler));

            return s_impl.DelaySubscription<TSource>(source, dueTime, scheduler);
        }

        /// <summary>
        /// Time shifts the observable sequence by delaying the subscription to the specified absolute time.
        /// </summary>
        /// <typeparam name="TSource">The type of the elements in the source sequence.</typeparam>
        /// <param name="source">Source sequence to delay subscription for.</param>
        /// <param name="dueTime">Absolute time to perform the subscription at.</param>
        /// <returns>Time-shifted sequence.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="source"/> is null.</exception>
        /// <remarks>
        /// <para>
        /// This operator is more efficient than <see cref="Observable.Delay{T}(IObservable{T}, DateTimeOffset)">Delay</see> but postpones all side-effects of subscription and affects error propagation timing.
        /// </para>
        /// <para>
        /// The side-effects of subscribing to the source sequence will be run on the default scheduler. Observer callbacks will not be affected.
        /// </para>
        /// </remarks>
        public static IObservable<TSource> DelaySubscription<TSource>(this IObservable<TSource> source, DateTimeOffset dueTime)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));

            return s_impl.DelaySubscription<TSource>(source, dueTime);
        }

        /// <summary>
        /// Time shifts the observable sequence by delaying the subscription to the specified absolute time, using the specified scheduler to run timers.
        /// </summary>
        /// <typeparam name="TSource">The type of the elements in the source sequence.</typeparam>
        /// <param name="source">Source sequence to delay subscription for.</param>
        /// <param name="dueTime">Absolute time to perform the subscription at.</param>
        /// <param name="scheduler">Scheduler to run the subscription delay timer on.</param>
        /// <returns>Time-shifted sequence.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="source"/> or <paramref name="scheduler"/> is null.</exception>
        /// <remarks>
        /// <para>
        /// This operator is more efficient than <see cref="Observable.Delay{T}(IObservable{T}, DateTimeOffset, IScheduler)">Delay</see> but postpones all side-effects of subscription and affects error propagation timing.
        /// </para>
        /// <para>
        /// The side-effects of subscribing to the source sequence will be run on the specified scheduler. Observer callbacks will not be affected.
        /// </para>
        /// </remarks>
        public static IObservable<TSource> DelaySubscription<TSource>(this IObservable<TSource> source, DateTimeOffset dueTime, IScheduler scheduler)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));
            if (scheduler == null)
                throw new ArgumentNullException(nameof(scheduler));

            return s_impl.DelaySubscription<TSource>(source, dueTime, scheduler);
        }

        #endregion

        #region + Generate +

        /// <summary>
        /// Generates an observable sequence by running a state-driven and temporal loop producing the sequence's elements.
        /// </summary>
        /// <typeparam name="TState">The type of the state used in the generator loop.</typeparam>
        /// <typeparam name="TResult">The type of the elements in the produced sequence.</typeparam>
        /// <param name="initialState">Initial state.</param>
        /// <param name="condition">Condition to terminate generation (upon returning false).</param>
        /// <param name="iterate">Iteration step function.</param>
        /// <param name="resultSelector">Selector function for results produced in the sequence.</param>
        /// <param name="timeSelector">Time selector function to control the speed of values being produced each iteration.</param>
        /// <returns>The generated sequence.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="condition"/> or <paramref name="iterate"/> or <paramref name="resultSelector"/> or <paramref name="timeSelector"/> is null.</exception>
        public static IObservable<TResult> Generate<TState, TResult>(TState initialState, Func<TState, bool> condition, Func<TState, TState> iterate, Func<TState, TResult> resultSelector, Func<TState, TimeSpan> timeSelector)
        {
            if (condition == null)
                throw new ArgumentNullException(nameof(condition));
            if (iterate == null)
                throw new ArgumentNullException(nameof(iterate));
            if (resultSelector == null)
                throw new ArgumentNullException(nameof(resultSelector));
            if (timeSelector == null)
                throw new ArgumentNullException(nameof(timeSelector));

            return s_impl.Generate<TState, TResult>(initialState, condition, iterate, resultSelector, timeSelector);
        }

        /// <summary>
        /// Generates an observable sequence by running a state-driven and temporal loop producing the sequence's elements, using the specified scheduler to run timers and to send out observer messages.
        /// </summary>
        /// <typeparam name="TState">The type of the state used in the generator loop.</typeparam>
        /// <typeparam name="TResult">The type of the elements in the produced sequence.</typeparam>
        /// <param name="initialState">Initial state.</param>
        /// <param name="condition">Condition to terminate generation (upon returning false).</param>
        /// <param name="iterate">Iteration step function.</param>
        /// <param name="resultSelector">Selector function for results produced in the sequence.</param>
        /// <param name="timeSelector">Time selector function to control the speed of values being produced each iteration.</param>
        /// <param name="scheduler">Scheduler on which to run the generator loop.</param>
        /// <returns>The generated sequence.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="condition"/> or <paramref name="iterate"/> or <paramref name="resultSelector"/> or <paramref name="timeSelector"/> or <paramref name="scheduler"/> is null.</exception>
        public static IObservable<TResult> Generate<TState, TResult>(TState initialState, Func<TState, bool> condition, Func<TState, TState> iterate, Func<TState, TResult> resultSelector, Func<TState, TimeSpan> timeSelector, IScheduler scheduler)
        {
            if (condition == null)
                throw new ArgumentNullException(nameof(condition));
            if (iterate == null)
                throw new ArgumentNullException(nameof(iterate));
            if (resultSelector == null)
                throw new ArgumentNullException(nameof(resultSelector));
            if (timeSelector == null)
                throw new ArgumentNullException(nameof(timeSelector));
            if (scheduler == null)
                throw new ArgumentNullException(nameof(scheduler));

            return s_impl.Generate<TState, TResult>(initialState, condition, iterate, resultSelector, timeSelector, scheduler);
        }

        /// <summary>
        /// Generates an observable sequence by running a state-driven and temporal loop producing the sequence's elements.
        /// </summary>
        /// <typeparam name="TState">The type of the state used in the generator loop.</typeparam>
        /// <typeparam name="TResult">The type of the elements in the produced sequence.</typeparam>
        /// <param name="initialState">Initial state.</param>
        /// <param name="condition">Condition to terminate generation (upon returning false).</param>
        /// <param name="iterate">Iteration step function.</param>
        /// <param name="resultSelector">Selector function for results produced in the sequence.</param>
        /// <param name="timeSelector">Time selector function to control the speed of values being produced each iteration.</param>
        /// <returns>The generated sequence.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="condition"/> or <paramref name="iterate"/> or <paramref name="resultSelector"/> or <paramref name="timeSelector"/> is null.</exception>
        public static IObservable<TResult> Generate<TState, TResult>(TState initialState, Func<TState, bool> condition, Func<TState, TState> iterate, Func<TState, TResult> resultSelector, Func<TState, DateTimeOffset> timeSelector)
        {
            if (condition == null)
                throw new ArgumentNullException(nameof(condition));
            if (iterate == null)
                throw new ArgumentNullException(nameof(iterate));
            if (resultSelector == null)
                throw new ArgumentNullException(nameof(resultSelector));
            if (timeSelector == null)
                throw new ArgumentNullException(nameof(timeSelector));

            return s_impl.Generate<TState, TResult>(initialState, condition, iterate, resultSelector, timeSelector);
        }

        /// <summary>
        /// Generates an observable sequence by running a state-driven and temporal loop producing the sequence's elements, using the specified scheduler to run timers and to send out observer messages.
        /// </summary>
        /// <typeparam name="TState">The type of the state used in the generator loop.</typeparam>
        /// <typeparam name="TResult">The type of the elements in the produced sequence.</typeparam>
        /// <param name="initialState">Initial state.</param>
        /// <param name="condition">Condition to terminate generation (upon returning false).</param>
        /// <param name="iterate">Iteration step function.</param>
        /// <param name="resultSelector">Selector function for results produced in the sequence.</param>
        /// <param name="timeSelector">Time selector function to control the speed of values being produced each iteration.</param>
        /// <param name="scheduler">Scheduler on which to run the generator loop.</param>
        /// <returns>The generated sequence.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="condition"/> or <paramref name="iterate"/> or <paramref name="resultSelector"/> or <paramref name="timeSelector"/> or <paramref name="scheduler"/> is null.</exception>
        public static IObservable<TResult> Generate<TState, TResult>(TState initialState, Func<TState, bool> condition, Func<TState, TState> iterate, Func<TState, TResult> resultSelector, Func<TState, DateTimeOffset> timeSelector, IScheduler scheduler)
        {
            if (condition == null)
                throw new ArgumentNullException(nameof(condition));
            if (iterate == null)
                throw new ArgumentNullException(nameof(iterate));
            if (resultSelector == null)
                throw new ArgumentNullException(nameof(resultSelector));
            if (timeSelector == null)
                throw new ArgumentNullException(nameof(timeSelector));
            if (scheduler == null)
                throw new ArgumentNullException(nameof(scheduler));

            return s_impl.Generate<TState, TResult>(initialState, condition, iterate, resultSelector, timeSelector, scheduler);
        }

        #endregion

        #region + Interval +

        /// <summary>
        /// Returns an observable sequence that produces a value after each period.
        /// </summary>
        /// <param name="period">Period for producing the values in the resulting sequence. If this value is equal to TimeSpan.Zero, the timer will recur as fast as possible.</param>
        /// <returns>An observable sequence that produces a value after each period.</returns>
        /// <exception cref="ArgumentOutOfRangeException"><paramref name="period"/> is less than TimeSpan.Zero.</exception>
        /// <remarks>
        /// Intervals are measured between the start of subsequent notifications, not between the end of the previous and the start of the next notification.
        /// If the observer takes longer than the interval period to handle the message, the subsequent notification will be delivered immediately after the
        /// current one has been handled. In case you need to control the time between the end and the start of consecutive notifications, consider using the
        /// <see cref="Observable.Generate{TState, TResult}(TState, Func{TState, bool}, Func{TState, TState}, Func{TState, TResult}, Func{TState, TimeSpan})"/>
        /// operator instead.
        /// </remarks>
        public static IObservable<long> Interval(TimeSpan period)
        {
            if (period < TimeSpan.Zero)
                throw new ArgumentOutOfRangeException(nameof(period));

            return s_impl.Interval(period);
        }

        /// <summary>
        /// Returns an observable sequence that produces a value after each period, using the specified scheduler to run timers and to send out observer messages.
        /// </summary>
        /// <param name="period">Period for producing the values in the resulting sequence. If this value is equal to TimeSpan.Zero, the timer will recur as fast as possible.</param>
        /// <param name="scheduler">Scheduler to run the timer on.</param>
        /// <returns>An observable sequence that produces a value after each period.</returns>
        /// <exception cref="ArgumentOutOfRangeException"><paramref name="period"/> is less than TimeSpan.Zero.</exception>
        /// <exception cref="ArgumentNullException"><paramref name="scheduler"/> is null.</exception>
        /// <remarks>
        /// Intervals are measured between the start of subsequent notifications, not between the end of the previous and the start of the next notification.
        /// If the observer takes longer than the interval period to handle the message, the subsequent notification will be delivered immediately after the
        /// current one has been handled. In case you need to control the time between the end and the start of consecutive notifications, consider using the
        /// <see cref="Observable.Generate{TState, TResult}(TState, Func{TState, bool}, Func{TState, TState}, Func{TState, TResult}, Func{TState, TimeSpan}, IScheduler)"/>
        /// operator instead.
        /// </remarks>
        public static IObservable<long> Interval(TimeSpan period, IScheduler scheduler)
        {
            if (period < TimeSpan.Zero)
                throw new ArgumentOutOfRangeException(nameof(period));
            if (scheduler == null)
                throw new ArgumentNullException(nameof(scheduler));

            return s_impl.Interval(period, scheduler);
        }

        #endregion

        #region + Sample +

        /// <summary>
        /// Samples the observable sequence at each interval.
        /// Upon each sampling tick, the latest element (if any) in the source sequence during the last sampling interval is sent to the resulting sequence.
        /// </summary>
        /// <typeparam name="TSource">The type of the elements in the source sequence.</typeparam>
        /// <param name="source">Source sequence to sample.</param>
        /// <param name="interval">Interval at which to sample. If this value is equal to TimeSpan.Zero, the scheduler will continuously sample the stream.</param>
        /// <returns>Sampled observable sequence.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="source"/> is null.</exception>
        /// <exception cref="ArgumentOutOfRangeException"><paramref name="interval"/> is less than TimeSpan.Zero.</exception>
        /// <remarks>
        /// Specifying a TimeSpan.Zero value for <paramref name="interval"/> doesn't guarantee all source sequence elements will be preserved. This is a side-effect
        /// of the asynchrony introduced by the scheduler, where the sampling action may not execute immediately, despite the TimeSpan.Zero due time.
        /// </remarks>
        public static IObservable<TSource> Sample<TSource>(this IObservable<TSource> source, TimeSpan interval)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));
            if (interval < TimeSpan.Zero)
                throw new ArgumentOutOfRangeException(nameof(interval));

            return s_impl.Sample<TSource>(source, interval);
        }

        /// <summary>
        /// Samples the observable sequence at each interval, using the specified scheduler to run sampling timers.
        /// Upon each sampling tick, the latest element (if any) in the source sequence during the last sampling interval is sent to the resulting sequence.
        /// </summary>
        /// <typeparam name="TSource">The type of the elements in the source sequence.</typeparam>
        /// <param name="source">Source sequence to sample.</param>
        /// <param name="interval">Interval at which to sample. If this value is equal to TimeSpan.Zero, the scheduler will continuously sample the stream.</param>
        /// <param name="scheduler">Scheduler to run the sampling timer on.</param>
        /// <returns>Sampled observable sequence.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="source"/> or <paramref name="scheduler"/> is null.</exception>
        /// <exception cref="ArgumentOutOfRangeException"><paramref name="interval"/> is less than TimeSpan.Zero.</exception>
        /// <remarks>
        /// Specifying a TimeSpan.Zero value for <paramref name="interval"/> doesn't guarantee all source sequence elements will be preserved. This is a side-effect
        /// of the asynchrony introduced by the scheduler, where the sampling action may not execute immediately, despite the TimeSpan.Zero due time.
        /// </remarks>
        public static IObservable<TSource> Sample<TSource>(this IObservable<TSource> source, TimeSpan interval, IScheduler scheduler)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));
            if (interval < TimeSpan.Zero)
                throw new ArgumentOutOfRangeException(nameof(interval));
            if (scheduler == null)
                throw new ArgumentNullException(nameof(scheduler));

            return s_impl.Sample<TSource>(source, interval, scheduler);
        }

        /// <summary>
        /// Samples the source observable sequence using a sampler observable sequence producing sampling ticks.
        /// Upon each sampling tick, the latest element (if any) in the source sequence during the last sampling interval is sent to the resulting sequence.
        /// </summary>
        /// <typeparam name="TSource">The type of the elements in the source sequence.</typeparam>
        /// <typeparam name="TSample">The type of the elements in the sampling sequence.</typeparam>
        /// <param name="source">Source sequence to sample.</param>
        /// <param name="sampler">Sampling tick sequence.</param>
        /// <returns>Sampled observable sequence.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="source"/> or <paramref name="sampler"/> is null.</exception>
        public static IObservable<TSource> Sample<TSource, TSample>(this IObservable<TSource> source, IObservable<TSample> sampler)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));
            if (sampler == null)
                throw new ArgumentNullException(nameof(sampler));

            return s_impl.Sample<TSource, TSample>(source, sampler);
        }

        #endregion

        #region + Skip +

        /// <summary>
        /// Skips elements for the specified duration from the start of the observable source sequence.
        /// </summary>
        /// <typeparam name="TSource">The type of the elements in the source sequence.</typeparam>
        /// <param name="source">Source sequence to skip elements for.</param>
        /// <param name="duration">Duration for skipping elements from the start of the sequence.</param>
        /// <returns>An observable sequence with the elements skipped during the specified duration from the start of the source sequence.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="source"/> is null.</exception>
        /// <exception cref="ArgumentOutOfRangeException"><paramref name="duration"/> is less than TimeSpan.Zero.</exception>
        /// <remarks>
        /// <para>
        /// Specifying a TimeSpan.Zero value for <paramref name="duration"/> doesn't guarantee no elements will be dropped from the start of the source sequence.
        /// This is a side-effect of the asynchrony introduced by the scheduler, where the action that causes callbacks from the source sequence to be forwarded
        /// may not execute immediately, despite the TimeSpan.Zero due time.
        /// </para>
        /// <para>
        /// Errors produced by the source sequence are always forwarded to the result sequence, even if the error occurs before the <paramref name="duration"/>.
        /// </para>
        /// </remarks>
        public static IObservable<TSource> Skip<TSource>(this IObservable<TSource> source, TimeSpan duration)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));
            if (duration < TimeSpan.Zero)
                throw new ArgumentOutOfRangeException(nameof(duration));

            return s_impl.Skip<TSource>(source, duration);
        }

        /// <summary>
        /// Skips elements for the specified duration from the start of the observable source sequence, using the specified scheduler to run timers.
        /// </summary>
        /// <typeparam name="TSource">The type of the elements in the source sequence.</typeparam>
        /// <param name="source">Source sequence to skip elements for.</param>
        /// <param name="duration">Duration for skipping elements from the start of the sequence.</param>
        /// <param name="scheduler">Scheduler to run the timer on.</param>
        /// <returns>An observable sequence with the elements skipped during the specified duration from the start of the source sequence.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="source"/> or <paramref name="scheduler"/> is null.</exception>
        /// <exception cref="ArgumentOutOfRangeException"><paramref name="duration"/> is less than TimeSpan.Zero.</exception>
        /// <remarks>
        /// <para>
        /// Specifying a TimeSpan.Zero value for <paramref name="duration"/> doesn't guarantee no elements will be dropped from the start of the source sequence.
        /// This is a side-effect of the asynchrony introduced by the scheduler, where the action that causes callbacks from the source sequence to be forwarded
        /// may not execute immediately, despite the TimeSpan.Zero due time.
        /// </para>
        /// <para>
        /// Errors produced by the source sequence are always forwarded to the result sequence, even if the error occurs before the <paramref name="duration"/>.
        /// </para>
        /// </remarks>
        public static IObservable<TSource> Skip<TSource>(this IObservable<TSource> source, TimeSpan duration, IScheduler scheduler)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));
            if (duration < TimeSpan.Zero)
                throw new ArgumentOutOfRangeException(nameof(duration));
            if (scheduler == null)
                throw new ArgumentNullException(nameof(scheduler));

            return s_impl.Skip<TSource>(source, duration, scheduler);
        }

        #endregion

        #region + SkipLast +

        /// <summary>
        /// Skips elements for the specified duration from the end of the observable source sequence.
        /// </summary>
        /// <typeparam name="TSource">The type of the elements in the source sequence.</typeparam>
        /// <param name="source">Source sequence to skip elements for.</param>
        /// <param name="duration">Duration for skipping elements from the end of the sequence.</param>
        /// <returns>An observable sequence with the elements skipped during the specified duration from the end of the source sequence.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="source"/> is null.</exception>
        /// <exception cref="ArgumentOutOfRangeException"><paramref name="duration"/> is less than TimeSpan.Zero.</exception>
        /// <remarks>
        /// This operator accumulates a queue with a length enough to store elements received during the initial <paramref name="duration"/> window.
        /// As more elements are received, elements older than the specified <paramref name="duration"/> are taken from the queue and produced on the
        /// result sequence. This causes elements to be delayed with <paramref name="duration"/>.
        /// </remarks>
        public static IObservable<TSource> SkipLast<TSource>(this IObservable<TSource> source, TimeSpan duration)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));
            if (duration < TimeSpan.Zero)
                throw new ArgumentOutOfRangeException(nameof(duration));

            return s_impl.SkipLast<TSource>(source, duration);
        }

        /// <summary>
        /// Skips elements for the specified duration from the end of the observable source sequence, using the specified scheduler to run timers.
        /// </summary>
        /// <typeparam name="TSource">The type of the elements in the source sequence.</typeparam>
        /// <param name="source">Source sequence to skip elements for.</param>
        /// <param name="duration">Duration for skipping elements from the end of the sequence.</param>
        /// <param name="scheduler">Scheduler to run the timer on.</param>
        /// <returns>An observable sequence with the elements skipped during the specified duration from the end of the source sequence.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="source"/> or <paramref name="scheduler"/> is null.</exception>
        /// <exception cref="ArgumentOutOfRangeException"><paramref name="duration"/> is less than TimeSpan.Zero.</exception>
        /// <remarks>
        /// This operator accumulates a queue with a length enough to store elements received during the initial <paramref name="duration"/> window.
        /// As more elements are received, elements older than the specified <paramref name="duration"/> are taken from the queue and produced on the
        /// result sequence. This causes elements to be delayed with <paramref name="duration"/>.
        /// </remarks>
        public static IObservable<TSource> SkipLast<TSource>(this IObservable<TSource> source, TimeSpan duration, IScheduler scheduler)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));
            if (duration < TimeSpan.Zero)
                throw new ArgumentOutOfRangeException(nameof(duration));
            if (scheduler == null)
                throw new ArgumentNullException(nameof(scheduler));

            return s_impl.SkipLast<TSource>(source, duration, scheduler);
        }

        #endregion

        #region + SkipUntil +

        /// <summary>
        /// Skips elements from the observable source sequence until the specified start time.
        /// </summary>
        /// <typeparam name="TSource">The type of the elements in the source sequence.</typeparam>
        /// <param name="source">Source sequence to skip elements for.</param>
        /// <param name="startTime">Time to start taking elements from the source sequence. If this value is less than or equal to DateTimeOffset.UtcNow, no elements will be skipped.</param>
        /// <returns>An observable sequence with the elements skipped until the specified start time.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="source"/> is null.</exception>
        /// <remarks>
        /// Errors produced by the source sequence are always forwarded to the result sequence, even if the error occurs before the <paramref name="startTime"/>.
        /// </remarks>
        public static IObservable<TSource> SkipUntil<TSource>(this IObservable<TSource> source, DateTimeOffset startTime)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));

            return s_impl.SkipUntil<TSource>(source, startTime);
        }

        /// <summary>
        /// Skips elements from the observable source sequence until the specified start time, using the specified scheduler to run timers.
        /// </summary>
        /// <typeparam name="TSource">The type of the elements in the source sequence.</typeparam>
        /// <param name="source">Source sequence to skip elements for.</param>
        /// <param name="startTime">Time to start taking elements from the source sequence. If this value is less than or equal to DateTimeOffset.UtcNow, no elements will be skipped.</param>
        /// <param name="scheduler">Scheduler to run the timer on.</param>
        /// <returns>An observable sequence with the elements skipped until the specified start time.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="source"/> or <paramref name="scheduler"/> is null.</exception>
        /// <remarks>
        /// Errors produced by the source sequence are always forwarded to the result sequence, even if the error occurs before the <paramref name="startTime"/>.
        /// </remarks>
        public static IObservable<TSource> SkipUntil<TSource>(this IObservable<TSource> source, DateTimeOffset startTime, IScheduler scheduler)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));
            if (scheduler == null)
                throw new ArgumentNullException(nameof(scheduler));

            return s_impl.SkipUntil<TSource>(source, startTime, scheduler);
        }

        #endregion

        #region + Take +

        /// <summary>
        /// Takes elements for the specified duration from the start of the observable source sequence.
        /// </summary>
        /// <typeparam name="TSource">The type of the elements in the source sequence.</typeparam>
        /// <param name="source">Source sequence to take elements from.</param>
        /// <param name="duration">Duration for taking elements from the start of the sequence.</param>
        /// <returns>An observable sequence with the elements taken during the specified duration from the start of the source sequence.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="source"/> is null.</exception>
        /// <exception cref="ArgumentOutOfRangeException"><paramref name="duration"/> is less than TimeSpan.Zero.</exception>
        /// <remarks>
        /// Specifying a TimeSpan.Zero value for <paramref name="duration"/> doesn't guarantee an empty sequence will be returned. This is a side-effect
        /// of the asynchrony introduced by the scheduler, where the action that stops forwarding callbacks from the source sequence may not execute
        /// immediately, despite the TimeSpan.Zero due time.
        /// </remarks>
        public static IObservable<TSource> Take<TSource>(this IObservable<TSource> source, TimeSpan duration)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));
            if (duration < TimeSpan.Zero)
                throw new ArgumentOutOfRangeException(nameof(duration));

            return s_impl.Take<TSource>(source, duration);
        }

        /// <summary>
        /// Takes elements for the specified duration from the start of the observable source sequence, using the specified scheduler to run timers.
        /// </summary>
        /// <typeparam name="TSource">The type of the elements in the source sequence.</typeparam>
        /// <param name="source">Source sequence to take elements from.</param>
        /// <param name="duration">Duration for taking elements from the start of the sequence.</param>
        /// <param name="scheduler">Scheduler to run the timer on.</param>
        /// <returns>An observable sequence with the elements taken during the specified duration from the start of the source sequence.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="source"/> or <paramref name="scheduler"/> is null.</exception>
        /// <exception cref="ArgumentOutOfRangeException"><paramref name="duration"/> is less than TimeSpan.Zero.</exception>
        /// <remarks>
        /// Specifying a TimeSpan.Zero value for <paramref name="duration"/> doesn't guarantee an empty sequence will be returned. This is a side-effect
        /// of the asynchrony introduced by the scheduler, where the action that stops forwarding callbacks from the source sequence may not execute
        /// immediately, despite the TimeSpan.Zero due time.
        /// </remarks>
        public static IObservable<TSource> Take<TSource>(this IObservable<TSource> source, TimeSpan duration, IScheduler scheduler)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));
            if (duration < TimeSpan.Zero)
                throw new ArgumentOutOfRangeException(nameof(duration));
            if (scheduler == null)
                throw new ArgumentNullException(nameof(scheduler));

            return s_impl.Take<TSource>(source, duration, scheduler);
        }

        #endregion

        #region + TakeLast +

        /// <summary>
        /// Returns elements within the specified duration from the end of the observable source sequence.
        /// </summary>
        /// <typeparam name="TSource">The type of the elements in the source sequence.</typeparam>
        /// <param name="source">Source sequence to take elements from.</param>
        /// <param name="duration">Duration for taking elements from the end of the sequence.</param>
        /// <returns>An observable sequence with the elements taken during the specified duration from the end of the source sequence.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="source"/> is null.</exception>
        /// <exception cref="ArgumentOutOfRangeException"><paramref name="duration"/> is less than TimeSpan.Zero.</exception>
        /// <remarks>
        /// This operator accumulates a buffer with a length enough to store elements for any <paramref name="duration"/> window during the lifetime of
        /// the source sequence. Upon completion of the source sequence, this buffer is drained on the result sequence. This causes the result elements
        /// to be delayed with <paramref name="duration"/>.
        /// </remarks>
        public static IObservable<TSource> TakeLast<TSource>(this IObservable<TSource> source, TimeSpan duration)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));
            if (duration < TimeSpan.Zero)
                throw new ArgumentOutOfRangeException(nameof(duration));

            return s_impl.TakeLast<TSource>(source, duration);
        }

        /// <summary>
        /// Returns elements within the specified duration from the end of the observable source sequence, using the specified scheduler to run timers.
        /// </summary>
        /// <typeparam name="TSource">The type of the elements in the source sequence.</typeparam>
        /// <param name="source">Source sequence to take elements from.</param>
        /// <param name="duration">Duration for taking elements from the end of the sequence.</param>
        /// <param name="scheduler">Scheduler to run the timer on.</param>
        /// <returns>An observable sequence with the elements taken during the specified duration from the end of the source sequence.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="source"/> or <paramref name="scheduler"/> is null.</exception>
        /// <exception cref="ArgumentOutOfRangeException"><paramref name="duration"/> is less than TimeSpan.Zero.</exception>
        /// <remarks>
        /// This operator accumulates a buffer with a length enough to store elements for any <paramref name="duration"/> window during the lifetime of
        /// the source sequence. Upon completion of the source sequence, this buffer is drained on the result sequence. This causes the result elements
        /// to be delayed with <paramref name="duration"/>.
        /// </remarks>
        public static IObservable<TSource> TakeLast<TSource>(this IObservable<TSource> source, TimeSpan duration, IScheduler scheduler)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));
            if (duration < TimeSpan.Zero)
                throw new ArgumentOutOfRangeException(nameof(duration));
            if (scheduler == null)
                throw new ArgumentNullException(nameof(scheduler));

            return s_impl.TakeLast<TSource>(source, duration, scheduler);
        }

        /// <summary>
        /// Returns elements within the specified duration from the end of the observable source sequence, using the specified schedulers to run timers and to drain the collected elements.
        /// </summary>
        /// <typeparam name="TSource">The type of the elements in the source sequence.</typeparam>
        /// <param name="source">Source sequence to take elements from.</param>
        /// <param name="duration">Duration for taking elements from the end of the sequence.</param>
        /// <param name="timerScheduler">Scheduler to run the timer on.</param>
        /// <param name="loopScheduler">Scheduler to drain the collected elements.</param>
        /// <returns>An observable sequence with the elements taken during the specified duration from the end of the source sequence.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="source"/> or <paramref name="timerScheduler"/> or <paramref name="loopScheduler"/> is null.</exception>
        /// <exception cref="ArgumentOutOfRangeException"><paramref name="duration"/> is less than TimeSpan.Zero.</exception>
        /// <remarks>
        /// This operator accumulates a buffer with a length enough to store elements for any <paramref name="duration"/> window during the lifetime of
        /// the source sequence. Upon completion of the source sequence, this buffer is drained on the result sequence. This causes the result elements
        /// to be delayed with <paramref name="duration"/>.
        /// </remarks>
        public static IObservable<TSource> TakeLast<TSource>(this IObservable<TSource> source, TimeSpan duration, IScheduler timerScheduler, IScheduler loopScheduler)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));
            if (duration < TimeSpan.Zero)
                throw new ArgumentOutOfRangeException(nameof(duration));
            if (timerScheduler == null)
                throw new ArgumentNullException(nameof(timerScheduler));
            if (loopScheduler == null)
                throw new ArgumentNullException(nameof(loopScheduler));

            return s_impl.TakeLast<TSource>(source, duration, timerScheduler, loopScheduler);
        }

        #endregion

        #region + TakeLastBuffer +

        /// <summary>
        /// Returns a list with the elements within the specified duration from the end of the observable source sequence.
        /// </summary>
        /// <typeparam name="TSource">The type of the elements in the source sequence.</typeparam>
        /// <param name="source">Source sequence to take elements from.</param>
        /// <param name="duration">Duration for taking elements from the end of the sequence.</param>
        /// <returns>An observable sequence containing a single list with the elements taken during the specified duration from the end of the source sequence.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="source"/> is null.</exception>
        /// <exception cref="ArgumentOutOfRangeException"><paramref name="duration"/> is less than TimeSpan.Zero.</exception>
        /// <remarks>
        /// This operator accumulates a buffer with a length enough to store elements for any <paramref name="duration"/> window during the lifetime of
        /// the source sequence. Upon completion of the source sequence, this buffer is produced on the result sequence.
        /// </remarks>
        public static IObservable<IList<TSource>> TakeLastBuffer<TSource>(this IObservable<TSource> source, TimeSpan duration)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));
            if (duration < TimeSpan.Zero)
                throw new ArgumentOutOfRangeException(nameof(duration));

            return s_impl.TakeLastBuffer<TSource>(source, duration);
        }

        /// <summary>
        /// Returns a list with the elements within the specified duration from the end of the observable source sequence, using the specified scheduler to run timers.
        /// </summary>
        /// <typeparam name="TSource">The type of the elements in the source sequence.</typeparam>
        /// <param name="source">Source sequence to take elements from.</param>
        /// <param name="duration">Duration for taking elements from the end of the sequence.</param>
        /// <param name="scheduler">Scheduler to run the timer on.</param>
        /// <returns>An observable sequence containing a single list with the elements taken during the specified duration from the end of the source sequence.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="source"/> or <paramref name="scheduler"/> is null.</exception>
        /// <exception cref="ArgumentOutOfRangeException"><paramref name="duration"/> is less than TimeSpan.Zero.</exception>
        /// <remarks>
        /// This operator accumulates a buffer with a length enough to store elements for any <paramref name="duration"/> window during the lifetime of
        /// the source sequence. Upon completion of the source sequence, this buffer is produced on the result sequence.
        /// </remarks>
        public static IObservable<IList<TSource>> TakeLastBuffer<TSource>(this IObservable<TSource> source, TimeSpan duration, IScheduler scheduler)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));
            if (duration < TimeSpan.Zero)
                throw new ArgumentOutOfRangeException(nameof(duration));
            if (scheduler == null)
                throw new ArgumentNullException(nameof(scheduler));

            return s_impl.TakeLastBuffer<TSource>(source, duration, scheduler);
        }

        #endregion

        #region + TakeUntil +

        /// <summary>
        /// Takes elements for the specified duration until the specified end time.
        /// </summary>
        /// <typeparam name="TSource">The type of the elements in the source sequence.</typeparam>
        /// <param name="source">Source sequence to take elements from.</param>
        /// <param name="endTime">Time to stop taking elements from the source sequence. If this value is less than or equal to DateTimeOffset.UtcNow, the result stream will complete immediately.</param>
        /// <returns>An observable sequence with the elements taken until the specified end time.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="source"/> is null.</exception>
        public static IObservable<TSource> TakeUntil<TSource>(this IObservable<TSource> source, DateTimeOffset endTime)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));

            return s_impl.TakeUntil<TSource>(source, endTime);
        }

        /// <summary>
        /// Takes elements for the specified duration until the specified end time, using the specified scheduler to run timers.
        /// </summary>
        /// <typeparam name="TSource">The type of the elements in the source sequence.</typeparam>
        /// <param name="source">Source sequence to take elements from.</param>
        /// <param name="endTime">Time to stop taking elements from the source sequence. If this value is less than or equal to DateTimeOffset.UtcNow, the result stream will complete immediately.</param>
        /// <param name="scheduler">Scheduler to run the timer on.</param>
        /// <returns>An observable sequence with the elements taken until the specified end time.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="source"/> or <paramref name="scheduler"/> is null.</exception>
        public static IObservable<TSource> TakeUntil<TSource>(this IObservable<TSource> source, DateTimeOffset endTime, IScheduler scheduler)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));
            if (scheduler == null)
                throw new ArgumentNullException(nameof(scheduler));

            return s_impl.TakeUntil<TSource>(source, endTime, scheduler);
        }

        #endregion

        #region + Throttle +

        /// <summary>
        /// Ignores elements from an observable sequence which are followed by another element within a specified relative time duration.
        /// </summary>
        /// <typeparam name="TSource">The type of the elements in the source sequence.</typeparam>
        /// <param name="source">Source sequence to throttle.</param>
        /// <param name="dueTime">Throttling duration for each element.</param>
        /// <returns>The throttled sequence.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="source"/> is null.</exception>
        /// <exception cref="ArgumentOutOfRangeException"><paramref name="dueTime"/> is less than TimeSpan.Zero.</exception>
        /// <remarks>
        /// <para>
        /// This operator throttles the source sequence by holding on to each element for the duration specified in <paramref name="dueTime"/>. If another
        /// element is produced within this time window, the element is dropped and a new timer is started for the current element, repeating this whole
        /// process. For streams that never have gaps larger than or equal to <paramref name="dueTime"/> between elements, the resulting stream won't
        /// produce any elements. In order to reduce the volume of a stream whilst guaranteeing the periodic production of elements, consider using the
        /// Observable.Sample set of operators.
        /// </para>
        /// <para>
        /// Specifying a TimeSpan.Zero value for <paramref name="dueTime"/> is not recommended but supported, causing throttling timers to be scheduled
        /// that are due immediately. However, this doesn't guarantee all elements will be retained in the result sequence. This is a side-effect of the
        /// asynchrony introduced by the scheduler, where the action to forward the current element may not execute immediately, despite the TimeSpan.Zero
        /// due time. In such cases, the next element may arrive before the scheduler gets a chance to run the throttling action.
        /// </para>
        /// </remarks>
        public static IObservable<TSource> Throttle<TSource>(this IObservable<TSource> source, TimeSpan dueTime)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));
            if (dueTime < TimeSpan.Zero)
                throw new ArgumentOutOfRangeException(nameof(dueTime));

            return s_impl.Throttle<TSource>(source, dueTime);
        }

        /// <summary>
        /// Ignores elements from an observable sequence which are followed by another element within a specified relative time duration, using the specified scheduler to run throttling timers.
        /// </summary>
        /// <typeparam name="TSource">The type of the elements in the source sequence.</typeparam>
        /// <param name="source">Source sequence to throttle.</param>
        /// <param name="dueTime">Throttling duration for each element.</param>
        /// <param name="scheduler">Scheduler to run the throttle timers on.</param>
        /// <returns>The throttled sequence.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="source"/> or <paramref name="scheduler"/> is null.</exception>
        /// <exception cref="ArgumentOutOfRangeException"><paramref name="dueTime"/> is less than TimeSpan.Zero.</exception>
        /// <remarks>
        /// <para>
        /// This operator throttles the source sequence by holding on to each element for the duration specified in <paramref name="dueTime"/>. If another
        /// element is produced within this time window, the element is dropped and a new timer is started for the current element, repeating this whole
        /// process. For streams that never have gaps larger than or equal to <paramref name="dueTime"/> between elements, the resulting stream won't
        /// produce any elements. In order to reduce the volume of a stream whilst guaranteeing the periodic production of elements, consider using the
        /// Observable.Sample set of operators.
        /// </para>
        /// <para>
        /// Specifying a TimeSpan.Zero value for <paramref name="dueTime"/> is not recommended but supported, causing throttling timers to be scheduled
        /// that are due immediately. However, this doesn't guarantee all elements will be retained in the result sequence. This is a side-effect of the
        /// asynchrony introduced by the scheduler, where the action to forward the current element may not execute immediately, despite the TimeSpan.Zero
        /// due time. In such cases, the next element may arrive before the scheduler gets a chance to run the throttling action.
        /// </para>
        /// </remarks>
        public static IObservable<TSource> Throttle<TSource>(this IObservable<TSource> source, TimeSpan dueTime, IScheduler scheduler)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));
            if (dueTime < TimeSpan.Zero)
                throw new ArgumentOutOfRangeException(nameof(dueTime));
            if (scheduler == null)
                throw new ArgumentNullException(nameof(scheduler));

            return s_impl.Throttle<TSource>(source, dueTime, scheduler);
        }

        /// <summary>
        /// Ignores elements from an observable sequence which are followed by another value within a computed throttle duration.
        /// </summary>
        /// <typeparam name="TSource">The type of the elements in the source sequence.</typeparam>
        /// <typeparam name="TThrottle">The type of the elements in the throttle sequences selected for each element in the source sequence.</typeparam>
        /// <param name="source">Source sequence to throttle.</param>
        /// <param name="throttleDurationSelector">Selector function to retrieve a sequence indicating the throttle duration for each given element.</param>
        /// <returns>The throttled sequence.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="source"/> or <paramref name="throttleDurationSelector"/> is null.</exception>
        /// <remarks>
        /// This operator throttles the source sequence by holding on to each element for the duration denoted by <paramref name="throttleDurationSelector"/>.
        /// If another element is produced within this time window, the element is dropped and a new timer is started for the current element, repeating this
        /// whole process. For streams where the duration computed by applying the <paramref name="throttleDurationSelector"/> to each element overlaps with
        /// the occurrence of the successor element, the resulting stream won't produce any elements. In order to reduce the volume of a stream whilst
        /// guaranteeing the periodic production of elements, consider using the Observable.Sample set of operators.
        /// </remarks>
        public static IObservable<TSource> Throttle<TSource, TThrottle>(this IObservable<TSource> source, Func<TSource, IObservable<TThrottle>> throttleDurationSelector)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));
            if (throttleDurationSelector == null)
                throw new ArgumentNullException(nameof(throttleDurationSelector));

            return s_impl.Throttle<TSource, TThrottle>(source, throttleDurationSelector);
        }

        #endregion

        #region + TimeInterval +

        /// <summary>
        /// Records the time interval between consecutive elements in an observable sequence.
        /// </summary>
        /// <typeparam name="TSource">The type of the elements in the source sequence.</typeparam>
        /// <param name="source">Source sequence to record time intervals for.</param>
        /// <returns>An observable sequence with time interval information on elements.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="source"/> is null.</exception>
        public static IObservable<System.Reactive.TimeInterval<TSource>> TimeInterval<TSource>(this IObservable<TSource> source)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));

            return s_impl.TimeInterval<TSource>(source);
        }

        /// <summary>
        /// Records the time interval between consecutive elements in an observable sequence, using the specified scheduler to compute time intervals.
        /// </summary>
        /// <typeparam name="TSource">The type of the elements in the source sequence.</typeparam>
        /// <param name="source">Source sequence to record time intervals for.</param>
        /// <param name="scheduler">Scheduler used to compute time intervals.</param>
        /// <returns>An observable sequence with time interval information on elements.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="source"/> or <paramref name="scheduler"/> is null.</exception>
        public static IObservable<System.Reactive.TimeInterval<TSource>> TimeInterval<TSource>(this IObservable<TSource> source, IScheduler scheduler)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));
            if (scheduler == null)
                throw new ArgumentNullException(nameof(scheduler));

            return s_impl.TimeInterval<TSource>(source, scheduler);
        }

        #endregion

        #region + Timeout +

        #region TimeSpan

        /// <summary>
        /// Applies a timeout policy for each element in the observable sequence.
        /// If the next element isn't received within the specified timeout duration starting from its predecessor, a TimeoutException is propagated to the observer.
        /// </summary>
        /// <typeparam name="TSource">The type of the elements in the source sequence.</typeparam>
        /// <param name="source">Source sequence to perform a timeout for.</param>
        /// <param name="dueTime">Maximum duration between values before a timeout occurs.</param>
        /// <returns>The source sequence with a TimeoutException in case of a timeout.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="source"/> is null.</exception>
        /// <exception cref="ArgumentOutOfRangeException"><paramref name="dueTime"/> is less than TimeSpan.Zero.</exception>
        /// <exception cref="TimeoutException">(Asynchronous) If no element is produced within <paramref name="dueTime"/> from the previous element.</exception>
        /// <remarks>
        /// <para>
        /// In case you only want to timeout on the first element, consider using the <see cref="Observable.Amb{TSource}(IObservable{TSource}, IObservable{TSource})"/>
        /// operator applied to the source sequence and a delayed <see cref="Observable.Throw{TResult}(Exception)"/> sequence. Alternatively, the general-purpose overload
        /// of Timeout, <see cref="Timeout{TSource, TTimeout}(IObservable{TSource}, IObservable{TTimeout}, Func{TSource, IObservable{TTimeout}})"/> can be used.
        /// </para>
        /// <para>
        /// Specifying a TimeSpan.Zero value for <paramref name="dueTime"/> is not recommended but supported, causing timeout timers to be scheduled that are due
        /// immediately. However, this doesn't guarantee a timeout will occur, even for the first element. This is a side-effect of the asynchrony introduced by the
        /// scheduler, where the action to propagate a timeout may not execute immediately, despite the TimeSpan.Zero due time. In such cases, the next element may
        /// arrive before the scheduler gets a chance to run the timeout action.
        /// </para>
        /// </remarks>
        public static IObservable<TSource> Timeout<TSource>(this IObservable<TSource> source, TimeSpan dueTime)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));
            if (dueTime < TimeSpan.Zero)
                throw new ArgumentOutOfRangeException(nameof(dueTime));

            return s_impl.Timeout<TSource>(source, dueTime);
        }

        /// <summary>
        /// Applies a timeout policy for each element in the observable sequence, using the specified scheduler to run timeout timers.
        /// If the next element isn't received within the specified timeout duration starting from its predecessor, a TimeoutException is propagated to the observer.
        /// </summary>
        /// <typeparam name="TSource">The type of the elements in the source sequence.</typeparam>
        /// <param name="source">Source sequence to perform a timeout for.</param>
        /// <param name="dueTime">Maximum duration between values before a timeout occurs.</param>
        /// <param name="scheduler">Scheduler to run the timeout timers on.</param>
        /// <returns>The source sequence with a TimeoutException in case of a timeout.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="source"/> or <paramref name="scheduler"/> is null.</exception>
        /// <exception cref="ArgumentOutOfRangeException"><paramref name="dueTime"/> is less than TimeSpan.Zero.</exception>
        /// <exception cref="TimeoutException">(Asynchronous) If no element is produced within <paramref name="dueTime"/> from the previous element.</exception>
        /// <remarks>
        /// <para>
        /// In case you only want to timeout on the first element, consider using the <see cref="Observable.Amb{TSource}(IObservable{TSource}, IObservable{TSource})"/>
        /// operator applied to the source sequence and a delayed <see cref="Observable.Throw{TResult}(Exception)"/> sequence. Alternatively, the general-purpose overload
        /// of Timeout, <see cref="Timeout{TSource, TTimeout}(IObservable{TSource}, IObservable{TTimeout}, Func{TSource, IObservable{TTimeout}})"/> can be used.
        /// </para>
        /// <para>
        /// Specifying a TimeSpan.Zero value for <paramref name="dueTime"/> is not recommended but supported, causing timeout timers to be scheduled that are due
        /// immediately. However, this doesn't guarantee a timeout will occur, even for the first element. This is a side-effect of the asynchrony introduced by the
        /// scheduler, where the action to propagate a timeout may not execute immediately, despite the TimeSpan.Zero due time. In such cases, the next element may
        /// arrive before the scheduler gets a chance to run the timeout action.
        /// </para>
        /// </remarks>
        public static IObservable<TSource> Timeout<TSource>(this IObservable<TSource> source, TimeSpan dueTime, IScheduler scheduler)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));
            if (dueTime < TimeSpan.Zero)
                throw new ArgumentOutOfRangeException(nameof(dueTime));
            if (scheduler == null)
                throw new ArgumentNullException(nameof(scheduler));

            return s_impl.Timeout<TSource>(source, dueTime, scheduler);
        }

        /// <summary>
        /// Applies a timeout policy for each element in the observable sequence.
        /// If the next element isn't received within the specified timeout duration starting from its predecessor, the other observable sequence is used to produce future messages from that point on.
        /// </summary>
        /// <typeparam name="TSource">The type of the elements in the source sequence and the other sequence used upon a timeout.</typeparam>
        /// <param name="source">Source sequence to perform a timeout for.</param>
        /// <param name="dueTime">Maximum duration between values before a timeout occurs.</param>
        /// <param name="other">Sequence to return in case of a timeout.</param>
        /// <returns>The source sequence switching to the other sequence in case of a timeout.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="source"/> or <paramref name="other"/> is null.</exception>
        /// <exception cref="ArgumentOutOfRangeException"><paramref name="dueTime"/> is less than TimeSpan.Zero.</exception>
        /// <remarks>
        /// <para>
        /// In case you only want to timeout on the first element, consider using the <see cref="Observable.Amb{TSource}(IObservable{TSource}, IObservable{TSource})"/>
        /// operator applied to the source sequence and a delayed <see cref="Observable.Throw{TResult}(Exception)"/> sequence. Alternatively, the general-purpose overload
        /// of Timeout, <see cref="Timeout{TSource, TTimeout}(IObservable{TSource}, IObservable{TTimeout}, Func{TSource, IObservable{TTimeout}})"/> can be used.
        /// </para>
        /// <para>
        /// Specifying a TimeSpan.Zero value for <paramref name="dueTime"/> is not recommended but supported, causing timeout timers to be scheduled that are due
        /// immediately. However, this doesn't guarantee a timeout will occur, even for the first element. This is a side-effect of the asynchrony introduced by the
        /// scheduler, where the action to propagate a timeout may not execute immediately, despite the TimeSpan.Zero due time. In such cases, the next element may
        /// arrive before the scheduler gets a chance to run the timeout action.
        /// </para>
        /// </remarks>
        public static IObservable<TSource> Timeout<TSource>(this IObservable<TSource> source, TimeSpan dueTime, IObservable<TSource> other)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));
            if (dueTime < TimeSpan.Zero)
                throw new ArgumentOutOfRangeException(nameof(dueTime));
            if (other == null)
                throw new ArgumentNullException(nameof(other));

            return s_impl.Timeout<TSource>(source, dueTime, other);
        }

        /// <summary>
        /// Applies a timeout policy for each element in the observable sequence, using the specified scheduler to run timeout timers.
        /// If the next element isn't received within the specified timeout duration starting from its predecessor, the other observable sequence is used to produce future messages from that point on.
        /// </summary>
        /// <typeparam name="TSource">The type of the elements in the source sequence and the other sequence used upon a timeout.</typeparam>
        /// <param name="source">Source sequence to perform a timeout for.</param>
        /// <param name="dueTime">Maximum duration between values before a timeout occurs.</param>
        /// <param name="other">Sequence to return in case of a timeout.</param>
        /// <param name="scheduler">Scheduler to run the timeout timers on.</param>
        /// <returns>The source sequence switching to the other sequence in case of a timeout.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="source"/> or <paramref name="other"/> or <paramref name="scheduler"/> is null.</exception>
        /// <exception cref="ArgumentOutOfRangeException"><paramref name="dueTime"/> is less than TimeSpan.Zero.</exception>
        /// <remarks>
        /// <para>
        /// In case you only want to timeout on the first element, consider using the <see cref="Observable.Amb{TSource}(IObservable{TSource}, IObservable{TSource})"/>
        /// operator applied to the source sequence and a delayed <see cref="Observable.Throw{TResult}(Exception)"/> sequence. Alternatively, the general-purpose overload
        /// of Timeout, <see cref="Timeout{TSource, TTimeout}(IObservable{TSource}, IObservable{TTimeout}, Func{TSource, IObservable{TTimeout}})"/> can be used.
        /// </para>
        /// <para>
        /// Specifying a TimeSpan.Zero value for <paramref name="dueTime"/> is not recommended but supported, causing timeout timers to be scheduled that are due
        /// immediately. However, this doesn't guarantee a timeout will occur, even for the first element. This is a side-effect of the asynchrony introduced by the
        /// scheduler, where the action to propagate a timeout may not execute immediately, despite the TimeSpan.Zero due time. In such cases, the next element may
        /// arrive before the scheduler gets a chance to run the timeout action.
        /// </para>
        /// </remarks>
        public static IObservable<TSource> Timeout<TSource>(this IObservable<TSource> source, TimeSpan dueTime, IObservable<TSource> other, IScheduler scheduler)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));
            if (dueTime < TimeSpan.Zero)
                throw new ArgumentOutOfRangeException(nameof(dueTime));
            if (other == null)
                throw new ArgumentNullException(nameof(other));
            if (scheduler == null)
                throw new ArgumentNullException(nameof(scheduler));

            return s_impl.Timeout<TSource>(source, dueTime, other, scheduler);
        }

        #endregion

        #region DateTimeOffset

        /// <summary>
        /// Applies a timeout policy to the observable sequence based on an absolute time.
        /// If the sequence doesn't terminate before the specified absolute due time, a TimeoutException is propagated to the observer.
        /// </summary>
        /// <typeparam name="TSource">The type of the elements in the source sequence.</typeparam>
        /// <param name="source">Source sequence to perform a timeout for.</param>
        /// <param name="dueTime">Time when a timeout occurs. If this value is less than or equal to DateTimeOffset.UtcNow, the timeout occurs immediately.</param>
        /// <returns>The source sequence with a TimeoutException in case of a timeout.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="source"/> is null.</exception>
        /// <exception cref="TimeoutException">(Asynchronous) If the sequence hasn't terminated before <paramref name="dueTime"/>.</exception>
        /// <remarks>
        /// In case you only want to timeout on the first element, consider using the <see cref="Observable.Amb{TSource}(IObservable{TSource}, IObservable{TSource})"/>
        /// operator applied to the source sequence and a delayed <see cref="Observable.Throw{TResult}(Exception)"/> sequence. Alternatively, the general-purpose overload
        /// of Timeout, <see cref="Timeout{TSource, TTimeout}(IObservable{TSource}, IObservable{TTimeout}, Func{TSource, IObservable{TTimeout}})"/> can be used.
        /// </remarks>
        public static IObservable<TSource> Timeout<TSource>(this IObservable<TSource> source, DateTimeOffset dueTime)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));

            return s_impl.Timeout<TSource>(source, dueTime);
        }

        /// <summary>
        /// Applies a timeout policy to the observable sequence based on an absolute time, using the specified scheduler to run timeout timers.
        /// If the sequence doesn't terminate before the specified absolute due time, a TimeoutException is propagated to the observer.
        /// </summary>
        /// <typeparam name="TSource">The type of the elements in the source sequence.</typeparam>
        /// <param name="source">Source sequence to perform a timeout for.</param>
        /// <param name="dueTime">Time when a timeout occurs. If this value is less than or equal to DateTimeOffset.UtcNow, the timeout occurs immediately.</param>
        /// <param name="scheduler">Scheduler to run the timeout timers on.</param>
        /// <returns>The source sequence with a TimeoutException in case of a timeout.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="source"/> or <paramref name="scheduler"/> is null.</exception>
        /// <exception cref="TimeoutException">(Asynchronous) If the sequence hasn't terminated before <paramref name="dueTime"/>.</exception>
        /// <remarks>
        /// In case you only want to timeout on the first element, consider using the <see cref="Observable.Amb{TSource}(IObservable{TSource}, IObservable{TSource})"/>
        /// operator applied to the source sequence and a delayed <see cref="Observable.Throw{TResult}(Exception)"/> sequence. Alternatively, the general-purpose overload
        /// of Timeout, <see cref="Timeout{TSource, TTimeout}(IObservable{TSource}, IObservable{TTimeout}, Func{TSource, IObservable{TTimeout}})"/> can be used.
        /// </remarks>
        public static IObservable<TSource> Timeout<TSource>(this IObservable<TSource> source, DateTimeOffset dueTime, IScheduler scheduler)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));
            if (scheduler == null)
                throw new ArgumentNullException(nameof(scheduler));

            return s_impl.Timeout<TSource>(source, dueTime, scheduler);
        }

        /// <summary>
        /// Applies a timeout policy to the observable sequence based on an absolute time.
        /// If the sequence doesn't terminate before the specified absolute due time, the other observable sequence is used to produce future messages from that point on.
        /// </summary>
        /// <typeparam name="TSource">The type of the elements in the source sequence and the other sequence used upon a timeout.</typeparam>
        /// <param name="source">Source sequence to perform a timeout for.</param>
        /// <param name="dueTime">Time when a timeout occurs. If this value is less than or equal to DateTimeOffset.UtcNow, the timeout occurs immediately.</param>
        /// <param name="other">Sequence to return in case of a timeout.</param>
        /// <returns>The source sequence switching to the other sequence in case of a timeout.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="source"/> or <paramref name="other"/> is null.</exception>
        /// <remarks>
        /// In case you only want to timeout on the first element, consider using the <see cref="Observable.Amb{TSource}(IObservable{TSource}, IObservable{TSource})"/>
        /// operator applied to the source sequence and a delayed <see cref="Observable.Throw{TResult}(Exception)"/> sequence. Alternatively, the general-purpose overload
        /// of Timeout, <see cref="Timeout{TSource, TTimeout}(IObservable{TSource}, IObservable{TTimeout}, Func{TSource, IObservable{TTimeout}})"/> can be used.
        /// </remarks>
        public static IObservable<TSource> Timeout<TSource>(this IObservable<TSource> source, DateTimeOffset dueTime, IObservable<TSource> other)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));
            if (other == null)
                throw new ArgumentNullException(nameof(other));

            return s_impl.Timeout<TSource>(source, dueTime, other);
        }

        /// <summary>
        /// Applies a timeout policy to the observable sequence based on an absolute time, using the specified scheduler to run timeout timers.
        /// If the sequence doesn't terminate before the specified absolute due time, the other observable sequence is used to produce future messages from that point on.
        /// </summary>
        /// <typeparam name="TSource">The type of the elements in the source sequence and the other sequence used upon a timeout.</typeparam>
        /// <param name="source">Source sequence to perform a timeout for.</param>
        /// <param name="dueTime">Time when a timeout occurs. If this value is less than or equal to DateTimeOffset.UtcNow, the timeout occurs immediately.</param>
        /// <param name="other">Sequence to return in case of a timeout.</param>
        /// <param name="scheduler">Scheduler to run the timeout timers on.</param>
        /// <returns>The source sequence switching to the other sequence in case of a timeout.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="source"/> or <paramref name="other"/> or <paramref name="scheduler"/> is null.</exception>
        /// <remarks>
        /// In case you only want to timeout on the first element, consider using the <see cref="Observable.Amb{TSource}(IObservable{TSource}, IObservable{TSource})"/>
        /// operator applied to the source sequence and a delayed <see cref="Observable.Throw{TResult}(Exception)"/> sequence. Alternatively, the general-purpose overload
        /// of Timeout, <see cref="Timeout{TSource, TTimeout}(IObservable{TSource}, IObservable{TTimeout}, Func{TSource, IObservable{TTimeout}})"/> can be used.
        /// </remarks>
        public static IObservable<TSource> Timeout<TSource>(this IObservable<TSource> source, DateTimeOffset dueTime, IObservable<TSource> other, IScheduler scheduler)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));
            if (scheduler == null)
                throw new ArgumentNullException(nameof(scheduler));
            if (other == null)
                throw new ArgumentNullException(nameof(other));

            return s_impl.Timeout<TSource>(source, dueTime, other, scheduler);
        }

        #endregion

        #region Duration selector

        /// <summary>
        /// Applies a timeout policy to the observable sequence based on a timeout duration computed for each element.
        /// If the next element isn't received within the computed duration starting from its predecessor, a TimeoutException is propagated to the observer.
        /// </summary>
        /// <typeparam name="TSource">The type of the elements in the source sequence.</typeparam>
        /// <typeparam name="TTimeout">The type of the elements in the timeout sequences used to indicate the timeout duration for each element in the source sequence.</typeparam>
        /// <param name="source">Source sequence to perform a timeout for.</param>
        /// <param name="timeoutDurationSelector">Selector to retrieve an observable sequence that represents the timeout between the current element and the next element.</param>
        /// <returns>The source sequence with a TimeoutException in case of a timeout.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="source"/> or <paramref name="timeoutDurationSelector"/> is null.</exception>
        public static IObservable<TSource> Timeout<TSource, TTimeout>(this IObservable<TSource> source, Func<TSource, IObservable<TTimeout>> timeoutDurationSelector)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));
            if (timeoutDurationSelector == null)
                throw new ArgumentNullException(nameof(timeoutDurationSelector));

            return s_impl.Timeout<TSource, TTimeout>(source, timeoutDurationSelector);
        }

        /// <summary>
        /// Applies a timeout policy to the observable sequence based on a timeout duration computed for each element.
        /// If the next element isn't received within the computed duration starting from its predecessor, the other observable sequence is used to produce future messages from that point on.
        /// </summary>
        /// <typeparam name="TSource">The type of the elements in the source sequence and the other sequence used upon a timeout.</typeparam>
        /// <typeparam name="TTimeout">The type of the elements in the timeout sequences used to indicate the timeout duration for each element in the source sequence.</typeparam>
        /// <param name="source">Source sequence to perform a timeout for.</param>
        /// <param name="timeoutDurationSelector">Selector to retrieve an observable sequence that represents the timeout between the current element and the next element.</param>
        /// <param name="other">Sequence to return in case of a timeout.</param>
        /// <returns>The source sequence switching to the other sequence in case of a timeout.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="source"/> or <paramref name="timeoutDurationSelector"/> or <paramref name="other"/> is null.</exception>
        public static IObservable<TSource> Timeout<TSource, TTimeout>(this IObservable<TSource> source, Func<TSource, IObservable<TTimeout>> timeoutDurationSelector, IObservable<TSource> other)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));
            if (timeoutDurationSelector == null)
                throw new ArgumentNullException(nameof(timeoutDurationSelector));
            if (other == null)
                throw new ArgumentNullException(nameof(other));

            return s_impl.Timeout<TSource, TTimeout>(source, timeoutDurationSelector, other);
        }

        /// <summary>
        /// Applies a timeout policy to the observable sequence based on an initial timeout duration for the first element, and a timeout duration computed for each subsequent element.
        /// If the next element isn't received within the computed duration starting from its predecessor, a TimeoutException is propagated to the observer.
        /// </summary>
        /// <typeparam name="TSource">The type of the elements in the source sequence.</typeparam>
        /// <typeparam name="TTimeout">The type of the elements in the timeout sequences used to indicate the timeout duration for each element in the source sequence.</typeparam>
        /// <param name="source">Source sequence to perform a timeout for.</param>
        /// <param name="firstTimeout">Observable sequence that represents the timeout for the first element.</param>
        /// <param name="timeoutDurationSelector">Selector to retrieve an observable sequence that represents the timeout between the current element and the next element.</param>
        /// <returns>The source sequence with a TimeoutException in case of a timeout.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="source"/> or <paramref name="firstTimeout"/> or <paramref name="timeoutDurationSelector"/> is null.</exception>
        public static IObservable<TSource> Timeout<TSource, TTimeout>(this IObservable<TSource> source, IObservable<TTimeout> firstTimeout, Func<TSource, IObservable<TTimeout>> timeoutDurationSelector)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));
            if (firstTimeout == null)
                throw new ArgumentNullException(nameof(firstTimeout));
            if (timeoutDurationSelector == null)
                throw new ArgumentNullException(nameof(timeoutDurationSelector));

            return s_impl.Timeout<TSource, TTimeout>(source, firstTimeout, timeoutDurationSelector);
        }

        /// <summary>
        /// Applies a timeout policy to the observable sequence based on an initial timeout duration for the first element, and a timeout duration computed for each subsequent element.
        /// If the next element isn't received within the computed duration starting from its predecessor, the other observable sequence is used to produce future messages from that point on.
        /// </summary>
        /// <typeparam name="TSource">The type of the elements in the source sequence and the other sequence used upon a timeout.</typeparam>
        /// <typeparam name="TTimeout">The type of the elements in the timeout sequences used to indicate the timeout duration for each element in the source sequence.</typeparam>
        /// <param name="source">Source sequence to perform a timeout for.</param>
        /// <param name="firstTimeout">Observable sequence that represents the timeout for the first element.</param>
        /// <param name="timeoutDurationSelector">Selector to retrieve an observable sequence that represents the timeout between the current element and the next element.</param>
        /// <param name="other">Sequence to return in case of a timeout.</param>
        /// <returns>The source sequence switching to the other sequence in case of a timeout.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="source"/> or <paramref name="firstTimeout"/> or <paramref name="timeoutDurationSelector"/> or <paramref name="other"/> is null.</exception>
        public static IObservable<TSource> Timeout<TSource, TTimeout>(this IObservable<TSource> source, IObservable<TTimeout> firstTimeout, Func<TSource, IObservable<TTimeout>> timeoutDurationSelector, IObservable<TSource> other)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));
            if (firstTimeout == null)
                throw new ArgumentNullException(nameof(firstTimeout));
            if (timeoutDurationSelector == null)
                throw new ArgumentNullException(nameof(timeoutDurationSelector));
            if (other == null)
                throw new ArgumentNullException(nameof(other));

            return s_impl.Timeout<TSource, TTimeout>(source, firstTimeout, timeoutDurationSelector, other);
        }

        #endregion

        #endregion

        #region + Timer +

        /// <summary>
        /// Returns an observable sequence that produces a single value after the specified relative due time has elapsed.
        /// </summary>
        /// <param name="dueTime">Relative time at which to produce the value. If this value is less than or equal to TimeSpan.Zero, the timer will fire as soon as possible.</param>
        /// <returns>An observable sequence that produces a value after the due time has elapsed.</returns>
        public static IObservable<long> Timer(TimeSpan dueTime)
        {
            return s_impl.Timer(dueTime);
        }

        /// <summary>
        /// Returns an observable sequence that produces a single value at the specified absolute due time.
        /// </summary>
        /// <param name="dueTime">Absolute time at which to produce the value. If this value is less than or equal to DateTimeOffset.UtcNow, the timer will fire as soon as possible.</param>
        /// <returns>An observable sequence that produces a value at due time.</returns>
        public static IObservable<long> Timer(DateTimeOffset dueTime)
        {
            return s_impl.Timer(dueTime);
        }

        /// <summary>
        /// Returns an observable sequence that periodically produces a value after the specified initial relative due time has elapsed.
        /// </summary>
        /// <param name="dueTime">Relative time at which to produce the first value. If this value is less than or equal to TimeSpan.Zero, the timer will fire as soon as possible.</param>
        /// <param name="period">Period to produce subsequent values. If this value is equal to TimeSpan.Zero, the timer will recur as fast as possible.</param>
        /// <returns>An observable sequence that produces a value after due time has elapsed and then after each period.</returns>
        /// <exception cref="ArgumentOutOfRangeException"><paramref name="period"/> is less than TimeSpan.Zero.</exception>
        public static IObservable<long> Timer(TimeSpan dueTime, TimeSpan period)
        {
            if (period < TimeSpan.Zero)
                throw new ArgumentOutOfRangeException(nameof(period));

            return s_impl.Timer(dueTime, period);
        }

        /// <summary>
        /// Returns an observable sequence that periodically produces a value starting at the specified initial absolute due time.
        /// </summary>
        /// <param name="dueTime">Absolute time at which to produce the first value. If this value is less than or equal to DateTimeOffset.UtcNow, the timer will fire as soon as possible.</param>
        /// <param name="period">Period to produce subsequent values. If this value is equal to TimeSpan.Zero, the timer will recur as fast as possible.</param>
        /// <returns>An observable sequence that produces a value at due time and then after each period.</returns>
        /// <exception cref="ArgumentOutOfRangeException"><paramref name="period"/> is less than TimeSpan.Zero.</exception>
        public static IObservable<long> Timer(DateTimeOffset dueTime, TimeSpan period)
        {
            if (period < TimeSpan.Zero)
                throw new ArgumentOutOfRangeException(nameof(period));

            return s_impl.Timer(dueTime, period);
        }

        /// <summary>
        /// Returns an observable sequence that produces a single value after the specified relative due time has elapsed, using the specified scheduler to run the timer.
        /// </summary>
        /// <param name="dueTime">Relative time at which to produce the value. If this value is less than or equal to TimeSpan.Zero, the timer will fire as soon as possible.</param>
        /// <param name="scheduler">Scheduler to run the timer on.</param>
        /// <returns>An observable sequence that produces a value after the due time has elapsed.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="scheduler"/> is null.</exception>
        public static IObservable<long> Timer(TimeSpan dueTime, IScheduler scheduler)
        {
            if (scheduler == null)
                throw new ArgumentNullException(nameof(scheduler));

            return s_impl.Timer(dueTime, scheduler);
        }

        /// <summary>
        /// Returns an observable sequence that produces a single value at the specified absolute due time, using the specified scheduler to run the timer.
        /// </summary>
        /// <param name="dueTime">Absolute time at which to produce the value. If this value is less than or equal to DateTimeOffset.UtcNow, the timer will fire as soon as possible.</param>
        /// <param name="scheduler">Scheduler to run the timer on.</param>
        /// <returns>An observable sequence that produces a value at due time.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="scheduler"/> is null.</exception>
        public static IObservable<long> Timer(DateTimeOffset dueTime, IScheduler scheduler)
        {
            if (scheduler == null)
                throw new ArgumentNullException(nameof(scheduler));

            return s_impl.Timer(dueTime, scheduler);
        }

        /// <summary>
        /// Returns an observable sequence that periodically produces a value after the specified initial relative due time has elapsed, using the specified scheduler to run timers.
        /// </summary>
        /// <param name="dueTime">Relative time at which to produce the first value. If this value is less than or equal to TimeSpan.Zero, the timer will fire as soon as possible.</param>
        /// <param name="period">Period to produce subsequent values. If this value is equal to TimeSpan.Zero, the timer will recur as fast as possible.</param>
        /// <param name="scheduler">Scheduler to run timers on.</param>
        /// <returns>An observable sequence that produces a value after due time has elapsed and then each period.</returns>
        /// <exception cref="ArgumentOutOfRangeException"><paramref name="period"/> is less than TimeSpan.Zero.</exception>
        /// <exception cref="ArgumentNullException"><paramref name="scheduler"/> is null.</exception>
        public static IObservable<long> Timer(TimeSpan dueTime, TimeSpan period, IScheduler scheduler)
        {
            if (period < TimeSpan.Zero)
                throw new ArgumentOutOfRangeException(nameof(period));
            if (scheduler == null)
                throw new ArgumentNullException(nameof(scheduler));

            return s_impl.Timer(dueTime, period, scheduler);
        }

        /// <summary>
        /// Returns an observable sequence that periodically produces a value starting at the specified initial absolute due time, using the specified scheduler to run timers.
        /// </summary>
        /// <param name="dueTime">Absolute time at which to produce the first value. If this value is less than or equal to DateTimeOffset.UtcNow, the timer will fire as soon as possible.</param>
        /// <param name="period">Period to produce subsequent values. If this value is equal to TimeSpan.Zero, the timer will recur as fast as possible.</param>
        /// <param name="scheduler">Scheduler to run timers on.</param>
        /// <returns>An observable sequence that produces a value at due time and then after each period.</returns>
        /// <exception cref="ArgumentOutOfRangeException"><paramref name="period"/> is less than TimeSpan.Zero.</exception>
        /// <exception cref="ArgumentNullException"><paramref name="scheduler"/> is null.</exception>
        public static IObservable<long> Timer(DateTimeOffset dueTime, TimeSpan period, IScheduler scheduler)
        {
            if (period < TimeSpan.Zero)
                throw new ArgumentOutOfRangeException(nameof(period));
            if (scheduler == null)
                throw new ArgumentNullException(nameof(scheduler));

            return s_impl.Timer(dueTime, period, scheduler);
        }

        #endregion

        #region + Timestamp +

        /// <summary>
        /// Timestamps each element in an observable sequence using the local system clock.
        /// </summary>
        /// <typeparam name="TSource">The type of the elements in the source sequence.</typeparam>
        /// <param name="source">Source sequence to timestamp elements for.</param>
        /// <returns>An observable sequence with timestamp information on elements.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="source"/> is null.</exception>
        public static IObservable<Timestamped<TSource>> Timestamp<TSource>(this IObservable<TSource> source)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));

            return s_impl.Timestamp<TSource>(source);
        }

        /// <summary>
        /// Timestamp each element in an observable sequence using the clock of the specified scheduler.
        /// </summary>
        /// <typeparam name="TSource">The type of the elements in the source sequence.</typeparam>
        /// <param name="source">Source sequence to timestamp elements for.</param>
        /// <param name="scheduler">Scheduler used to compute timestamps.</param>
        /// <returns>An observable sequence with timestamp information on elements.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="source"/> or <paramref name="scheduler"/> is null.</exception>
        public static IObservable<Timestamped<TSource>> Timestamp<TSource>(this IObservable<TSource> source, IScheduler scheduler)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));
            if (scheduler == null)
                throw new ArgumentNullException(nameof(scheduler));

            return s_impl.Timestamp<TSource>(source, scheduler);
        }

        #endregion

        #region + Window +

        #region TimeSpan only

        /// <summary>
        /// Projects each element of an observable sequence into consecutive non-overlapping windows which are produced based on timing information.
        /// </summary>
        /// <typeparam name="TSource">The type of the elements in the source sequence, and in the windows in the result sequence.</typeparam>
        /// <param name="source">Source sequence to produce windows over.</param>
        /// <param name="timeSpan">Length of each window.</param>
        /// <returns>The sequence of windows.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="source"/> is null.</exception>
        /// <exception cref="ArgumentOutOfRangeException"><paramref name="timeSpan"/> is less than TimeSpan.Zero.</exception>
        /// <remarks>
        /// Specifying a TimeSpan.Zero value for <paramref name="timeSpan"/> is not recommended but supported, causing the scheduler to create windows as fast as it can.
        /// Because all source sequence elements end up in one of the windows, some windows won't have a zero time span. This is a side-effect of the asynchrony introduced
        /// by the scheduler, where the action to close the current window and to create a new window may not execute immediately, despite the TimeSpan.Zero due time.
        /// </remarks>
        public static IObservable<IObservable<TSource>> Window<TSource>(this IObservable<TSource> source, TimeSpan timeSpan)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));
            if (timeSpan < TimeSpan.Zero)
                throw new ArgumentOutOfRangeException(nameof(timeSpan));

            return s_impl.Window<TSource>(source, timeSpan);
        }

        /// <summary>
        /// Projects each element of an observable sequence into consecutive non-overlapping windows which are produced based on timing information, using the specified scheduler to run timers.
        /// </summary>
        /// <typeparam name="TSource">The type of the elements in the source sequence, and in the windows in the result sequence.</typeparam>
        /// <param name="source">Source sequence to produce windows over.</param>
        /// <param name="timeSpan">Length of each window.</param>
        /// <param name="scheduler">Scheduler to run windowing timers on.</param>
        /// <returns>An observable sequence of windows.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="source"/> or <paramref name="scheduler"/> is null.</exception>
        /// <exception cref="ArgumentOutOfRangeException"><paramref name="timeSpan"/> is less than TimeSpan.Zero.</exception>
        /// <remarks>
        /// Specifying a TimeSpan.Zero value for <paramref name="timeSpan"/> is not recommended but supported, causing the scheduler to create windows as fast as it can.
        /// Because all source sequence elements end up in one of the windows, some windows won't have a zero time span. This is a side-effect of the asynchrony introduced
        /// by the scheduler, where the action to close the current window and to create a new window may not execute immediately, despite the TimeSpan.Zero due time.
        /// </remarks>
        public static IObservable<IObservable<TSource>> Window<TSource>(this IObservable<TSource> source, TimeSpan timeSpan, IScheduler scheduler)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));
            if (timeSpan < TimeSpan.Zero)
                throw new ArgumentOutOfRangeException(nameof(timeSpan));
            if (scheduler == null)
                throw new ArgumentNullException(nameof(scheduler));

            return s_impl.Window<TSource>(source, timeSpan, scheduler);
        }

        /// <summary>
        /// Projects each element of an observable sequence into zero or more windows which are produced based on timing information.
        /// </summary>
        /// <typeparam name="TSource">The type of the elements in the source sequence, and in the windows in the result sequence.</typeparam>
        /// <param name="source">Source sequence to produce windows over.</param>
        /// <param name="timeSpan">Length of each window.</param>
        /// <param name="timeShift">Interval between creation of consecutive windows.</param>
        /// <returns>An observable sequence of windows.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="source"/> is null.</exception>
        /// <exception cref="ArgumentOutOfRangeException"><paramref name="timeSpan"/> or <paramref name="timeSpan"/> is less than TimeSpan.Zero.</exception>
        /// <remarks>
        /// <para>
        /// Specifying a TimeSpan.Zero value for <paramref name="timeSpan"/> is not recommended but supported, causing the scheduler to create windows with minimum duration
        /// length. However, some windows won't have a zero time span. This is a side-effect of the asynchrony introduced by the scheduler, where the action to close the
        /// current window may not execute immediately, despite the TimeSpan.Zero due time.
        /// </para>
        /// <para>
        /// Specifying a TimeSpan.Zero value for <paramref name="timeShift"/> is not recommended but supported, causing the scheduler to create windows as fast as it can.
        /// However, this doesn't mean all windows will start at the beginning of the source sequence. This is a side-effect of the asynchrony introduced by the scheduler,
        /// where the action to create a new window may not execute immediately, despite the TimeSpan.Zero due time.
        /// </para>
        /// </remarks>
        public static IObservable<IObservable<TSource>> Window<TSource>(this IObservable<TSource> source, TimeSpan timeSpan, TimeSpan timeShift)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));
            if (timeSpan < TimeSpan.Zero)
                throw new ArgumentOutOfRangeException(nameof(timeSpan));
            if (timeShift < TimeSpan.Zero)
                throw new ArgumentOutOfRangeException(nameof(timeShift));

            return s_impl.Window<TSource>(source, timeSpan, timeShift);
        }

        /// <summary>
        /// Projects each element of an observable sequence into zero or more windows which are produced based on timing information, using the specified scheduler to run timers.
        /// </summary>
        /// <typeparam name="TSource">The type of the elements in the source sequence, and in the windows in the result sequence.</typeparam>
        /// <param name="source">Source sequence to produce windows over.</param>
        /// <param name="timeSpan">Length of each window.</param>
        /// <param name="timeShift">Interval between creation of consecutive windows.</param>
        /// <param name="scheduler">Scheduler to run windowing timers on.</param>
        /// <returns>An observable sequence of windows.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="source"/> or <paramref name="scheduler"/> is null.</exception>
        /// <exception cref="ArgumentOutOfRangeException"><paramref name="timeSpan"/> or <paramref name="timeSpan"/> is less than TimeSpan.Zero.</exception>
        /// <remarks>
        /// <para>
        /// Specifying a TimeSpan.Zero value for <paramref name="timeSpan"/> is not recommended but supported, causing the scheduler to create windows with minimum duration
        /// length. However, some windows won't have a zero time span. This is a side-effect of the asynchrony introduced by the scheduler, where the action to close the
        /// current window may not execute immediately, despite the TimeSpan.Zero due time.
        /// </para>
        /// <para>
        /// Specifying a TimeSpan.Zero value for <paramref name="timeShift"/> is not recommended but supported, causing the scheduler to create windows as fast as it can.
        /// However, this doesn't mean all windows will start at the beginning of the source sequence. This is a side-effect of the asynchrony introduced by the scheduler,
        /// where the action to create a new window may not execute immediately, despite the TimeSpan.Zero due time.
        /// </para>
        /// </remarks>
        public static IObservable<IObservable<TSource>> Window<TSource>(this IObservable<TSource> source, TimeSpan timeSpan, TimeSpan timeShift, IScheduler scheduler)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));
            if (timeSpan < TimeSpan.Zero)
                throw new ArgumentOutOfRangeException(nameof(timeSpan));
            if (timeShift < TimeSpan.Zero)
                throw new ArgumentOutOfRangeException(nameof(timeShift));
            if (scheduler == null)
                throw new ArgumentNullException(nameof(scheduler));

            return s_impl.Window<TSource>(source, timeSpan, timeShift, scheduler);
        }

        #endregion

        #region TimeSpan + int

        /// <summary>
        /// Projects each element of an observable sequence into a window that is completed when either it's full or a given amount of time has elapsed.
        /// A useful real-world analogy of this overload is the behavior of a ferry leaving the dock when all seats are taken, or at the scheduled time of departure, whichever event occurs first.
        /// </summary>
        /// <typeparam name="TSource">The type of the elements in the source sequence, and in the windows in the result sequence.</typeparam>
        /// <param name="source">Source sequence to produce windows over.</param>
        /// <param name="timeSpan">Maximum time length of a window.</param>
        /// <param name="count">Maximum element count of a window.</param>
        /// <returns>An observable sequence of windows.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="source"/> is null.</exception>
        /// <exception cref="ArgumentOutOfRangeException"><paramref name="timeSpan"/> is less than TimeSpan.Zero. -or- <paramref name="count"/> is less than or equal to zero.</exception>
        /// <remarks>
        /// Specifying a TimeSpan.Zero value for <paramref name="timeSpan"/> is not recommended but supported, causing the scheduler to create windows as fast as it can.
        /// Because all source sequence elements end up in one of the windows, some windows won't have a zero time span. This is a side-effect of the asynchrony introduced
        /// by the scheduler, where the action to close the current window and to create a new window may not execute immediately, despite the TimeSpan.Zero due time.
        /// </remarks>
        public static IObservable<IObservable<TSource>> Window<TSource>(this IObservable<TSource> source, TimeSpan timeSpan, int count)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));
            if (timeSpan < TimeSpan.Zero)
                throw new ArgumentOutOfRangeException(nameof(timeSpan));
            if (count <= 0)
                throw new ArgumentOutOfRangeException(nameof(count));

            return s_impl.Window<TSource>(source, timeSpan, count);
        }

        /// <summary>
        /// Projects each element of an observable sequence into a window that is completed when either it's full or a given amount of time has elapsed, using the specified scheduler to run timers.
        /// A useful real-world analogy of this overload is the behavior of a ferry leaving the dock when all seats are taken, or at the scheduled time of departure, whichever event occurs first.
        /// </summary>
        /// <typeparam name="TSource">The type of the elements in the source sequence, and in the windows in the result sequence.</typeparam>
        /// <param name="source">Source sequence to produce windows over.</param>
        /// <param name="timeSpan">Maximum time length of a window.</param>
        /// <param name="count">Maximum element count of a window.</param>
        /// <param name="scheduler">Scheduler to run windowing timers on.</param>
        /// <returns>An observable sequence of windows.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="source"/> or <paramref name="scheduler"/> is null.</exception>
        /// <exception cref="ArgumentOutOfRangeException"><paramref name="timeSpan"/> is less than TimeSpan.Zero. -or- <paramref name="count"/> is less than or equal to zero.</exception>
        /// <remarks>
        /// Specifying a TimeSpan.Zero value for <paramref name="timeSpan"/> is not recommended but supported, causing the scheduler to create windows as fast as it can.
        /// Because all source sequence elements end up in one of the windows, some windows won't have a zero time span. This is a side-effect of the asynchrony introduced
        /// by the scheduler, where the action to close the current window and to create a new window may not execute immediately, despite the TimeSpan.Zero due time.
        /// </remarks>
        public static IObservable<IObservable<TSource>> Window<TSource>(this IObservable<TSource> source, TimeSpan timeSpan, int count, IScheduler scheduler)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));
            if (timeSpan < TimeSpan.Zero)
                throw new ArgumentOutOfRangeException(nameof(timeSpan));
            if (count <= 0)
                throw new ArgumentOutOfRangeException(nameof(count));
            if (scheduler == null)
                throw new ArgumentNullException(nameof(scheduler));

            return s_impl.Window<TSource>(source, timeSpan, count, scheduler);
        }

        #endregion

        #endregion
    }
}

﻿// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information. 

using System.Collections.Generic;
using System.Reactive.Concurrency;

namespace System.Reactive.Linq
{
    public static partial class Observable
    {
        #region + Append +

        /// <summary>
        /// Append a value to an observable sequence.
        /// </summary>
        /// <typeparam name="TSource">The type of the elements in the source sequence.</typeparam>
        /// <param name="source">Source sequence to append the value to.</param>
        /// <param name="value">Value to append to the specified sequence.</param>
        /// <returns>The source sequence appended with the specified value.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="source"/> is null.</exception>
        public static IObservable<TSource> Append<TSource>(this IObservable<TSource> source, TSource value)
        {
            if (source == null)
            {
                throw new ArgumentNullException(nameof(source));
            }

            return s_impl.Append(source, value);
        }

        /// <summary>
        /// Append a value to an observable sequence.
        /// </summary>
        /// <typeparam name="TSource">The type of the elements in the source sequence.</typeparam>
        /// <param name="source">Source sequence to append the value to.</param>
        /// <param name="value">Value to append to the specified sequence.</param>
        /// <param name="scheduler">Scheduler to emit the append values on.</param>
        /// <returns>The source sequence appended with the specified value.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="source"/> is null.</exception>
        public static IObservable<TSource> Append<TSource>(this IObservable<TSource> source, TSource value, IScheduler scheduler)
        {
            if (source == null)
            {
                throw new ArgumentNullException(nameof(source));
            }

            if (scheduler == null)
            {
                throw new ArgumentNullException(nameof(scheduler));
            }

            return s_impl.Append(source, value, scheduler);
        }

        #endregion

        #region + AsObservable +

        /// <summary>
        /// Hides the identity of an observable sequence.
        /// </summary>
        /// <typeparam name="TSource">The type of the elements in the source sequence.</typeparam>
        /// <param name="source">An observable sequence whose identity to hide.</param>
        /// <returns>An observable sequence that hides the identity of the source sequence.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="source"/> is null.</exception>
        public static IObservable<TSource> AsObservable<TSource>(this IObservable<TSource> source)
        {
            if (source == null)
            {
                throw new ArgumentNullException(nameof(source));
            }

            return s_impl.AsObservable(source);
        }

        #endregion

        #region + Buffer +

        /// <summary>
        /// Projects each element of an observable sequence into consecutive non-overlapping buffers which are produced based on element count information.
        /// </summary>
        /// <typeparam name="TSource">The type of the elements in the source sequence, and in the lists in the result sequence.</typeparam>
        /// <param name="source">Source sequence to produce buffers over.</param>
        /// <param name="count">Length of each buffer.</param>
        /// <returns>An observable sequence of buffers.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="source"/> is null.</exception>
        /// <exception cref="ArgumentOutOfRangeException"><paramref name="count"/> is less than or equal to zero.</exception>
        public static IObservable<IList<TSource>> Buffer<TSource>(this IObservable<TSource> source, int count)
        {
            if (source == null)
            {
                throw new ArgumentNullException(nameof(source));
            }

            if (count <= 0)
            {
                throw new ArgumentOutOfRangeException(nameof(count));
            }

            return s_impl.Buffer(source, count);
        }

        /// <summary>
        /// Projects each element of an observable sequence into zero or more buffers which are produced based on element count information.
        /// </summary>
        /// <typeparam name="TSource">The type of the elements in the source sequence, and in the lists in the result sequence.</typeparam>
        /// <param name="source">Source sequence to produce buffers over.</param>
        /// <param name="count">Length of each buffer.</param>
        /// <param name="skip">Number of elements to skip between creation of consecutive buffers.</param>
        /// <returns>An observable sequence of buffers.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="source"/> is null.</exception>
        /// <exception cref="ArgumentOutOfRangeException"><paramref name="count"/> or <paramref name="skip"/> is less than or equal to zero.</exception>
        public static IObservable<IList<TSource>> Buffer<TSource>(this IObservable<TSource> source, int count, int skip)
        {
            if (source == null)
            {
                throw new ArgumentNullException(nameof(source));
            }

            if (count <= 0)
            {
                throw new ArgumentOutOfRangeException(nameof(count));
            }

            if (skip <= 0)
            {
                throw new ArgumentOutOfRangeException(nameof(skip));
            }

            return s_impl.Buffer(source, count, skip);
        }

        #endregion

        #region + Dematerialize +

        /// <summary>
        /// Dematerializes the explicit notification values of an observable sequence as implicit notifications.
        /// </summary>
        /// <typeparam name="TSource">The type of the elements materialized in the source sequence notification objects.</typeparam>
        /// <param name="source">An observable sequence containing explicit notification values which have to be turned into implicit notifications.</param>
        /// <returns>An observable sequence exhibiting the behavior corresponding to the source sequence's notification values.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="source"/> is null.</exception>
        public static IObservable<TSource> Dematerialize<TSource>(this IObservable<Notification<TSource>> source)
        {
            if (source == null)
            {
                throw new ArgumentNullException(nameof(source));
            }

            return s_impl.Dematerialize(source);
        }

        #endregion

        #region + DistinctUntilChanged +

        /// <summary>
        /// Returns an observable sequence that contains only distinct contiguous elements.
        /// </summary>
        /// <typeparam name="TSource">The type of the elements in the source sequence.</typeparam>
        /// <param name="source">An observable sequence to retain distinct contiguous elements for.</param>
        /// <returns>An observable sequence only containing the distinct contiguous elements from the source sequence.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="source"/> is null.</exception>
        public static IObservable<TSource> DistinctUntilChanged<TSource>(this IObservable<TSource> source)
        {
            if (source == null)
            {
                throw new ArgumentNullException(nameof(source));
            }

            return s_impl.DistinctUntilChanged(source);
        }

        /// <summary>
        /// Returns an observable sequence that contains only distinct contiguous elements according to the comparer.
        /// </summary>
        /// <typeparam name="TSource">The type of the elements in the source sequence.</typeparam>
        /// <param name="source">An observable sequence to retain distinct contiguous elements for.</param>
        /// <param name="comparer">Equality comparer for source elements.</param>
        /// <returns>An observable sequence only containing the distinct contiguous elements from the source sequence.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="source"/> or <paramref name="comparer"/> is null.</exception>
        public static IObservable<TSource> DistinctUntilChanged<TSource>(this IObservable<TSource> source, IEqualityComparer<TSource> comparer)
        {
            if (source == null)
            {
                throw new ArgumentNullException(nameof(source));
            }

            if (comparer == null)
            {
                throw new ArgumentNullException(nameof(comparer));
            }

            return s_impl.DistinctUntilChanged(source, comparer);
        }

        /// <summary>
        /// Returns an observable sequence that contains only distinct contiguous elements according to the keySelector.
        /// </summary>
        /// <typeparam name="TSource">The type of the elements in the source sequence.</typeparam>
        /// <typeparam name="TKey">The type of the discriminator key computed for each element in the source sequence.</typeparam>
        /// <param name="source">An observable sequence to retain distinct contiguous elements for, based on a computed key value.</param>
        /// <param name="keySelector">A function to compute the comparison key for each element.</param>
        /// <returns>An observable sequence only containing the distinct contiguous elements, based on a computed key value, from the source sequence.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="source"/> or <paramref name="keySelector"/> is null.</exception>
        public static IObservable<TSource> DistinctUntilChanged<TSource, TKey>(this IObservable<TSource> source, Func<TSource, TKey> keySelector)
        {
            if (source == null)
            {
                throw new ArgumentNullException(nameof(source));
            }

            if (keySelector == null)
            {
                throw new ArgumentNullException(nameof(keySelector));
            }

            return s_impl.DistinctUntilChanged(source, keySelector);
        }

        /// <summary>
        /// Returns an observable sequence that contains only distinct contiguous elements according to the keySelector and the comparer.
        /// </summary>
        /// <typeparam name="TSource">The type of the elements in the source sequence.</typeparam>
        /// <typeparam name="TKey">The type of the discriminator key computed for each element in the source sequence.</typeparam>
        /// <param name="source">An observable sequence to retain distinct contiguous elements for, based on a computed key value.</param>
        /// <param name="keySelector">A function to compute the comparison key for each element.</param>
        /// <param name="comparer">Equality comparer for computed key values.</param>
        /// <returns>An observable sequence only containing the distinct contiguous elements, based on a computed key value, from the source sequence.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="source"/> or <paramref name="keySelector"/> or <paramref name="comparer"/> is null.</exception>
        public static IObservable<TSource> DistinctUntilChanged<TSource, TKey>(this IObservable<TSource> source, Func<TSource, TKey> keySelector, IEqualityComparer<TKey> comparer)
        {
            if (source == null)
            {
                throw new ArgumentNullException(nameof(source));
            }

            if (keySelector == null)
            {
                throw new ArgumentNullException(nameof(keySelector));
            }

            if (comparer == null)
            {
                throw new ArgumentNullException(nameof(comparer));
            }

            return s_impl.DistinctUntilChanged(source, keySelector, comparer);
        }

        #endregion

        #region + Do +

        /// <summary>
        /// Invokes an action for each element in the observable sequence, and propagates all observer messages through the result sequence.
        /// This method can be used for debugging, logging, etc. of query behavior by intercepting the message stream to run arbitrary actions for messages on the pipeline.
        /// </summary>
        /// <typeparam name="TSource">The type of the elements in the source sequence.</typeparam>
        /// <param name="source">Source sequence.</param>
        /// <param name="onNext">Action to invoke for each element in the observable sequence.</param>
        /// <returns>The source sequence with the side-effecting behavior applied.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="source"/> or <paramref name="onNext"/> is null.</exception>
        public static IObservable<TSource> Do<TSource>(this IObservable<TSource> source, Action<TSource> onNext)
        {
            if (source == null)
            {
                throw new ArgumentNullException(nameof(source));
            }

            if (onNext == null)
            {
                throw new ArgumentNullException(nameof(onNext));
            }

            return s_impl.Do(source, onNext);
        }

        /// <summary>
        /// Invokes an action for each element in the observable sequence and invokes an action upon graceful termination of the observable sequence.
        /// This method can be used for debugging, logging, etc. of query behavior by intercepting the message stream to run arbitrary actions for messages on the pipeline.
        /// </summary>
        /// <typeparam name="TSource">The type of the elements in the source sequence.</typeparam>
        /// <param name="source">Source sequence.</param>
        /// <param name="onNext">Action to invoke for each element in the observable sequence.</param>
        /// <param name="onCompleted">Action to invoke upon graceful termination of the observable sequence.</param>
        /// <returns>The source sequence with the side-effecting behavior applied.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="source"/> or <paramref name="onNext"/> or <paramref name="onCompleted"/> is null.</exception>
        public static IObservable<TSource> Do<TSource>(this IObservable<TSource> source, Action<TSource> onNext, Action onCompleted)
        {
            if (source == null)
            {
                throw new ArgumentNullException(nameof(source));
            }

            if (onNext == null)
            {
                throw new ArgumentNullException(nameof(onNext));
            }

            if (onCompleted == null)
            {
                throw new ArgumentNullException(nameof(onCompleted));
            }

            return s_impl.Do(source, onNext, onCompleted);
        }

        /// <summary>
        /// Invokes an action for each element in the observable sequence and invokes an action upon exceptional termination of the observable sequence.
        /// This method can be used for debugging, logging, etc. of query behavior by intercepting the message stream to run arbitrary actions for messages on the pipeline.
        /// </summary>
        /// <typeparam name="TSource">The type of the elements in the source sequence.</typeparam>
        /// <param name="source">Source sequence.</param>
        /// <param name="onNext">Action to invoke for each element in the observable sequence.</param>
        /// <param name="onError">Action to invoke upon exceptional termination of the observable sequence.</param>
        /// <returns>The source sequence with the side-effecting behavior applied.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="source"/> or <paramref name="onNext"/> or <paramref name="onError"/> is null.</exception>
        public static IObservable<TSource> Do<TSource>(this IObservable<TSource> source, Action<TSource> onNext, Action<Exception> onError)
        {
            if (source == null)
            {
                throw new ArgumentNullException(nameof(source));
            }

            if (onNext == null)
            {
                throw new ArgumentNullException(nameof(onNext));
            }

            if (onError == null)
            {
                throw new ArgumentNullException(nameof(onError));
            }

            return s_impl.Do(source, onNext, onError);
        }

        /// <summary>
        /// Invokes an action for each element in the observable sequence and invokes an action upon graceful or exceptional termination of the observable sequence.
        /// This method can be used for debugging, logging, etc. of query behavior by intercepting the message stream to run arbitrary actions for messages on the pipeline.
        /// </summary>
        /// <typeparam name="TSource">The type of the elements in the source sequence.</typeparam>
        /// <param name="source">Source sequence.</param>
        /// <param name="onNext">Action to invoke for each element in the observable sequence.</param>
        /// <param name="onError">Action to invoke upon exceptional termination of the observable sequence.</param>
        /// <param name="onCompleted">Action to invoke upon graceful termination of the observable sequence.</param>
        /// <returns>The source sequence with the side-effecting behavior applied.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="source"/> or <paramref name="onNext"/> or <paramref name="onError"/> or <paramref name="onCompleted"/> is null.</exception>
        public static IObservable<TSource> Do<TSource>(this IObservable<TSource> source, Action<TSource> onNext, Action<Exception> onError, Action onCompleted)
        {
            if (source == null)
            {
                throw new ArgumentNullException(nameof(source));
            }

            if (onNext == null)
            {
                throw new ArgumentNullException(nameof(onNext));
            }

            if (onError == null)
            {
                throw new ArgumentNullException(nameof(onError));
            }

            if (onCompleted == null)
            {
                throw new ArgumentNullException(nameof(onCompleted));
            }

            return s_impl.Do(source, onNext, onError, onCompleted);
        }

        /// <summary>
        /// Invokes the observer's methods for each message in the source sequence.
        /// This method can be used for debugging, logging, etc. of query behavior by intercepting the message stream to run arbitrary actions for messages on the pipeline.
        /// </summary>
        /// <typeparam name="TSource">The type of the elements in the source sequence.</typeparam>
        /// <param name="source">Source sequence.</param>
        /// <param name="observer">Observer whose methods to invoke as part of the source sequence's observation.</param>
        /// <returns>The source sequence with the side-effecting behavior applied.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="source"/> or <paramref name="observer"/> is null.</exception>
        public static IObservable<TSource> Do<TSource>(this IObservable<TSource> source, IObserver<TSource> observer)
        {
            if (source == null)
            {
                throw new ArgumentNullException(nameof(source));
            }

            if (observer == null)
            {
                throw new ArgumentNullException(nameof(observer));
            }

            return s_impl.Do(source, observer);
        }

        #endregion

        #region + Finally +

        /// <summary>
        /// Invokes a specified action after the source observable sequence terminates gracefully or exceptionally.
        /// </summary>
        /// <typeparam name="TSource">The type of the elements in the source sequence.</typeparam>
        /// <param name="source">Source sequence.</param>
        /// <param name="finallyAction">Action to invoke after the source observable sequence terminates.</param>
        /// <returns>Source sequence with the action-invoking termination behavior applied.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="source"/> or <paramref name="finallyAction"/> is null.</exception>
        public static IObservable<TSource> Finally<TSource>(this IObservable<TSource> source, Action finallyAction)
        {
            if (source == null)
            {
                throw new ArgumentNullException(nameof(source));
            }

            if (finallyAction == null)
            {
                throw new ArgumentNullException(nameof(finallyAction));
            }

            return s_impl.Finally(source, finallyAction);
        }

        #endregion

        #region + IgnoreElements +

        /// <summary>
        /// Ignores all elements in an observable sequence leaving only the termination messages.
        /// </summary>
        /// <typeparam name="TSource">The type of the elements in the source sequence.</typeparam>
        /// <param name="source">Source sequence.</param>
        /// <returns>An empty observable sequence that signals termination, successful or exceptional, of the source sequence.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="source"/> is null.</exception>
        public static IObservable<TSource> IgnoreElements<TSource>(this IObservable<TSource> source)
        {
            if (source == null)
            {
                throw new ArgumentNullException(nameof(source));
            }

            return s_impl.IgnoreElements(source);
        }

        #endregion

        #region + Materialize +

        /// <summary>
        /// Materializes the implicit notifications of an observable sequence as explicit notification values.
        /// </summary>
        /// <typeparam name="TSource">The type of the elements in the source sequence.</typeparam>
        /// <param name="source">An observable sequence to get notification values for.</param>
        /// <returns>An observable sequence containing the materialized notification values from the source sequence.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="source"/> is null.</exception>
        public static IObservable<Notification<TSource>> Materialize<TSource>(this IObservable<TSource> source)
        {
            if (source == null)
            {
                throw new ArgumentNullException(nameof(source));
            }

            return s_impl.Materialize(source);
        }

        #endregion

        #region + Prepend +

        /// <summary>
        /// Prepend a value to an observable sequence.
        /// </summary>
        /// <typeparam name="TSource">The type of the elements in the source sequence.</typeparam>
        /// <param name="source">Source sequence to prepend the value to.</param>
        /// <param name="value">Value to prepend to the specified sequence.</param>
        /// <returns>The source sequence prepended with the specified value.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="source"/> is null.</exception>
        public static IObservable<TSource> Prepend<TSource>(this IObservable<TSource> source, TSource value)
        {
            if (source == null)
            {
                throw new ArgumentNullException(nameof(source));
            }

            return s_impl.Prepend(source, value);
        }

        /// <summary>
        /// Prepend a value to an observable sequence.
        /// </summary>
        /// <typeparam name="TSource">The type of the elements in the source sequence.</typeparam>
        /// <param name="source">Source sequence to prepend the value to.</param>
        /// <param name="value">Value to prepend to the specified sequence.</param>
        /// <param name="scheduler">Scheduler to emit the prepend values on.</param>
        /// <returns>The source sequence prepended with the specified value.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="source"/> is null.</exception>
        public static IObservable<TSource> Prepend<TSource>(this IObservable<TSource> source, TSource value, IScheduler scheduler)
        {
            if (source == null)
            {
                throw new ArgumentNullException(nameof(source));
            }

            if (scheduler == null)
            {
                throw new ArgumentNullException(nameof(scheduler));
            }

            return s_impl.Prepend(source, value, scheduler);
        }

        #endregion

        #region + Repeat +

        /// <summary>
        /// Repeats the observable sequence indefinitely.
        /// </summary>
        /// <typeparam name="TSource">The type of the elements in the source sequence.</typeparam>
        /// <param name="source">Observable sequence to repeat.</param>
        /// <returns>The observable sequence producing the elements of the given sequence repeatedly and sequentially.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="source"/> is null.</exception>
        public static IObservable<TSource> Repeat<TSource>(this IObservable<TSource> source)
        {
            if (source == null)
            {
                throw new ArgumentNullException(nameof(source));
            }

            return s_impl.Repeat(source);
        }

        /// <summary>
        /// Repeats the observable sequence a specified number of times.
        /// </summary>
        /// <typeparam name="TSource">The type of the elements in the source sequence.</typeparam>
        /// <param name="source">Observable sequence to repeat.</param>
        /// <param name="repeatCount">Number of times to repeat the sequence.</param>
        /// <returns>The observable sequence producing the elements of the given sequence repeatedly.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="source"/> is null.</exception>
        /// <exception cref="ArgumentOutOfRangeException"><paramref name="repeatCount"/> is less than zero.</exception>
        public static IObservable<TSource> Repeat<TSource>(this IObservable<TSource> source, int repeatCount)
        {
            if (source == null)
            {
                throw new ArgumentNullException(nameof(source));
            }

            if (repeatCount < 0)
            {
                throw new ArgumentOutOfRangeException(nameof(repeatCount));
            }

            return s_impl.Repeat(source, repeatCount);
        }

        /// <summary>
        /// Repeatedly resubscribes to the source observable after a normal completion and when the observable
        /// returned by a handler produces an arbitrary item.
        /// </summary>
        /// <typeparam name="TSource">The type of the elements in the source sequence.</typeparam>
        /// <typeparam name="TSignal">The arbitrary element type signaled by the handler observable.</typeparam>
        /// <param name="source">Observable sequence to keep repeating when it successfully terminates.</param>
        /// <param name="handler">The function that is called for each observer and takes an observable sequence of objects.
        /// It should return an observable of arbitrary items that should signal that arbitrary item in
        /// response to receiving the completion signal from the source observable. If this observable signals
        /// a terminal event, the sequence is terminated with that signal instead.</param>
        /// <returns>An observable sequence producing the elements of the given sequence repeatedly while each repetition terminates successfully.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="source"/> is null.</exception>
        /// <exception cref="ArgumentNullException"><paramref name="handler"/> is null.</exception>
        public static IObservable<TSource> RepeatWhen<TSource, TSignal>(this IObservable<TSource> source, Func<IObservable<object>, IObservable<TSignal>> handler)
        {
            if (source == null)
            {
                throw new ArgumentNullException(nameof(source));
            }

            if (handler == null)
            {
                throw new ArgumentNullException(nameof(handler));
            }

            return s_impl.RepeatWhen(source, handler);
        }


        #endregion

        #region + Retry +

        /// <summary>
        /// Repeats the source observable sequence until it successfully terminates.
        /// </summary>
        /// <typeparam name="TSource">The type of the elements in the source sequence.</typeparam>
        /// <param name="source">Observable sequence to repeat until it successfully terminates.</param>
        /// <returns>An observable sequence producing the elements of the given sequence repeatedly until it terminates successfully.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="source"/> is null.</exception>
        public static IObservable<TSource> Retry<TSource>(this IObservable<TSource> source)
        {
            if (source == null)
            {
                throw new ArgumentNullException(nameof(source));
            }

            return s_impl.Retry(source);
        }

        /// <summary>
        /// Repeats the source observable sequence the specified number of times or until it successfully terminates.
        /// </summary>
        /// <typeparam name="TSource">The type of the elements in the source sequence.</typeparam>
        /// <param name="source">Observable sequence to repeat until it successfully terminates.</param>
        /// <param name="retryCount">Number of times to repeat the sequence.</param>
        /// <returns>An observable sequence producing the elements of the given sequence repeatedly until it terminates successfully.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="source"/> is null.</exception>
        /// <exception cref="ArgumentOutOfRangeException"><paramref name="retryCount"/> is less than zero.</exception>
        public static IObservable<TSource> Retry<TSource>(this IObservable<TSource> source, int retryCount)
        {
            if (source == null)
            {
                throw new ArgumentNullException(nameof(source));
            }

            if (retryCount < 0)
            {
                throw new ArgumentOutOfRangeException(nameof(retryCount));
            }

            return s_impl.Retry(source, retryCount);
        }

        /// <summary>
        /// Retries (resubscribes to) the source observable after a failure and when the observable
        /// returned by a handler produces an arbitrary item.
        /// </summary>
        /// <typeparam name="TSource">The type of the elements in the source sequence.</typeparam>
        /// <typeparam name="TSignal">The arbitrary element type signaled by the handler observable.</typeparam>
        /// <param name="source">Observable sequence to repeat until it successfully terminates.</param>
        /// <param name="handler">The function that is called for each observer and takes an observable sequence of
        /// errors. It should return an observable of arbitrary items that should signal that arbitrary item in
        /// response to receiving the failure Exception from the source observable. If this observable signals
        /// a terminal event, the sequence is terminated with that signal instead.</param>
        /// <returns>An observable sequence producing the elements of the given sequence repeatedly until it terminates successfully.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="source"/> is null.</exception>
        /// <exception cref="ArgumentNullException"><paramref name="handler"/> is null.</exception>
        public static IObservable<TSource> RetryWhen<TSource, TSignal>(this IObservable<TSource> source, Func<IObservable<Exception>, IObservable<TSignal>> handler)
        {
            if (source == null)
            {
                throw new ArgumentNullException(nameof(source));
            }

            if (handler == null)
            {
                throw new ArgumentNullException(nameof(handler));
            }

            return s_impl.RetryWhen(source, handler);
        }


        #endregion

        #region + Scan +

        /// <summary>
        /// Applies an accumulator function over an observable sequence and returns each intermediate result. The specified seed value is used as the initial accumulator value.
        /// For aggregation behavior with no intermediate results, see <see cref="Observable.Aggregate{TSource, Accumulate}"/>.
        /// </summary>
        /// <typeparam name="TSource">The type of the elements in the source sequence.</typeparam>
        /// <typeparam name="TAccumulate">The type of the result of the aggregation.</typeparam>
        /// <param name="source">An observable sequence to accumulate over.</param>
        /// <param name="seed">The initial accumulator value.</param>
        /// <param name="accumulator">An accumulator function to be invoked on each element.</param>
        /// <returns>An observable sequence containing the accumulated values.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="source"/> or <paramref name="accumulator"/> is null.</exception>
        public static IObservable<TAccumulate> Scan<TSource, TAccumulate>(this IObservable<TSource> source, TAccumulate seed, Func<TAccumulate, TSource, TAccumulate> accumulator)
        {
            if (source == null)
            {
                throw new ArgumentNullException(nameof(source));
            }

            if (accumulator == null)
            {
                throw new ArgumentNullException(nameof(accumulator));
            }

            return s_impl.Scan(source, seed, accumulator);
        }

        /// <summary>
        /// Applies an accumulator function over an observable sequence and returns each intermediate result.
        /// For aggregation behavior with no intermediate results, see <see cref="Observable.Aggregate{TSource}"/>.
        /// </summary>
        /// <typeparam name="TSource">The type of the elements in the source sequence and the result of the aggregation.</typeparam>
        /// <param name="source">An observable sequence to accumulate over.</param>
        /// <param name="accumulator">An accumulator function to be invoked on each element.</param>
        /// <returns>An observable sequence containing the accumulated values.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="source"/> or <paramref name="accumulator"/> is null.</exception>
        public static IObservable<TSource> Scan<TSource>(this IObservable<TSource> source, Func<TSource, TSource, TSource> accumulator)
        {
            if (source == null)
            {
                throw new ArgumentNullException(nameof(source));
            }

            if (accumulator == null)
            {
                throw new ArgumentNullException(nameof(accumulator));
            }

            return s_impl.Scan(source, accumulator);
        }

        #endregion

        #region + SkipLast +

        /// <summary>
        /// Bypasses a specified number of elements at the end of an observable sequence.
        /// </summary>
        /// <typeparam name="TSource">The type of the elements in the source sequence.</typeparam>
        /// <param name="source">Source sequence.</param>
        /// <param name="count">Number of elements to bypass at the end of the source sequence.</param>
        /// <returns>An observable sequence containing the source sequence elements except for the bypassed ones at the end.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="source"/> is null.</exception>
        /// <exception cref="ArgumentOutOfRangeException"><paramref name="count"/> is less than zero.</exception>
        /// <remarks>
        /// This operator accumulates a queue with a length enough to store the first <paramref name="count"/> elements. As more elements are
        /// received, elements are taken from the front of the queue and produced on the result sequence. This causes elements to be delayed.
        /// </remarks>
        public static IObservable<TSource> SkipLast<TSource>(this IObservable<TSource> source, int count)
        {
            if (source == null)
            {
                throw new ArgumentNullException(nameof(source));
            }

            if (count < 0)
            {
                throw new ArgumentOutOfRangeException(nameof(count));
            }

            return s_impl.SkipLast(source, count);
        }

        #endregion

        #region + StartWith +

        /// <summary>
        /// Prepends a sequence of values to an observable sequence.
        /// </summary>
        /// <typeparam name="TSource">The type of the elements in the source sequence.</typeparam>
        /// <param name="source">Source sequence to prepend values to.</param>
        /// <param name="values">Values to prepend to the specified sequence.</param>
        /// <returns>The source sequence prepended with the specified values.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="source"/> or <paramref name="values"/> is null.</exception>
        public static IObservable<TSource> StartWith<TSource>(this IObservable<TSource> source, params TSource[] values)
        {
            if (source == null)
            {
                throw new ArgumentNullException(nameof(source));
            }

            if (values == null)
            {
                throw new ArgumentNullException(nameof(values));
            }

            return s_impl.StartWith(source, values);
        }

        /// <summary>
        /// Prepends a sequence of values to an observable sequence.
        /// </summary>
        /// <typeparam name="TSource">The type of the elements in the source sequence.</typeparam>
        /// <param name="source">Source sequence to prepend values to.</param>
        /// <param name="values">Values to prepend to the specified sequence.</param>
        /// <returns>The source sequence prepended with the specified values.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="source"/> or <paramref name="values"/> is null.</exception>
        public static IObservable<TSource> StartWith<TSource>(this IObservable<TSource> source, IEnumerable<TSource> values)
        {
            if (source == null)
            {
                throw new ArgumentNullException(nameof(source));
            }

            if (values == null)
            {
                throw new ArgumentNullException(nameof(values));
            }

            return s_impl.StartWith(source, values);
        }

        /// <summary>
        /// Prepends a sequence of values to an observable sequence.
        /// </summary>
        /// <typeparam name="TSource">The type of the elements in the source sequence.</typeparam>
        /// <param name="source">Source sequence to prepend values to.</param>
        /// <param name="scheduler">Scheduler to emit the prepended values on.</param>
        /// <param name="values">Values to prepend to the specified sequence.</param>
        /// <returns>The source sequence prepended with the specified values.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="source"/> or <paramref name="scheduler"/> or <paramref name="values"/> is null.</exception>
        public static IObservable<TSource> StartWith<TSource>(this IObservable<TSource> source, IScheduler scheduler, params TSource[] values)
        {
            if (source == null)
            {
                throw new ArgumentNullException(nameof(source));
            }

            if (scheduler == null)
            {
                throw new ArgumentNullException(nameof(scheduler));
            }

            if (values == null)
            {
                throw new ArgumentNullException(nameof(values));
            }

            return s_impl.StartWith(source, scheduler, values);
        }

        /// <summary>
        /// Prepends a sequence of values to an observable sequence.
        /// </summary>
        /// <typeparam name="TSource">The type of the elements in the source sequence.</typeparam>
        /// <param name="source">Source sequence to prepend values to.</param>
        /// <param name="scheduler">Scheduler to emit the prepended values on.</param>
        /// <param name="values">Values to prepend to the specified sequence.</param>
        /// <returns>The source sequence prepended with the specified values.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="source"/> or <paramref name="scheduler"/> or <paramref name="values"/> is null.</exception>
        public static IObservable<TSource> StartWith<TSource>(this IObservable<TSource> source, IScheduler scheduler, IEnumerable<TSource> values)
        {
            //
            // NOTE: For some reason, someone introduced this signature which is inconsistent with the Rx pattern of putting the IScheduler last.
            //       We can't change it at this point because of compatibility.
            //

            if (source == null)
            {
                throw new ArgumentNullException(nameof(source));
            }

            if (scheduler == null)
            {
                throw new ArgumentNullException(nameof(scheduler));
            }

            if (values == null)
            {
                throw new ArgumentNullException(nameof(values));
            }

            return s_impl.StartWith(source, scheduler, values);
        }

        #endregion

        #region + TakeLast +

        /// <summary>
        /// Returns a specified number of contiguous elements from the end of an observable sequence.
        /// </summary>
        /// <typeparam name="TSource">The type of the elements in the source sequence.</typeparam>
        /// <param name="source">Source sequence.</param>
        /// <param name="count">Number of elements to take from the end of the source sequence.</param>
        /// <returns>An observable sequence containing the specified number of elements from the end of the source sequence.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="source"/> is null.</exception>
        /// <exception cref="ArgumentOutOfRangeException"><paramref name="count"/> is less than zero.</exception>
        /// <remarks>
        /// This operator accumulates a buffer with a length enough to store elements <paramref name="count"/> elements. Upon completion of
        /// the source sequence, this buffer is drained on the result sequence. This causes the elements to be delayed.
        /// </remarks>
        public static IObservable<TSource> TakeLast<TSource>(this IObservable<TSource> source, int count)
        {
            if (source == null)
            {
                throw new ArgumentNullException(nameof(source));
            }

            if (count < 0)
            {
                throw new ArgumentOutOfRangeException(nameof(count));
            }

            return s_impl.TakeLast(source, count);
        }

        /// <summary>
        /// Returns a specified number of contiguous elements from the end of an observable sequence, using the specified scheduler to drain the queue.
        /// </summary>
        /// <typeparam name="TSource">The type of the elements in the source sequence.</typeparam>
        /// <param name="source">Source sequence.</param>
        /// <param name="count">Number of elements to take from the end of the source sequence.</param>
        /// <param name="scheduler">Scheduler used to drain the queue upon completion of the source sequence.</param>
        /// <returns>An observable sequence containing the specified number of elements from the end of the source sequence.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="source"/> or <paramref name="scheduler"/> is null.</exception>
        /// <exception cref="ArgumentOutOfRangeException"><paramref name="count"/> is less than zero.</exception>
        /// <remarks>
        /// This operator accumulates a buffer with a length enough to store elements <paramref name="count"/> elements. Upon completion of
        /// the source sequence, this buffer is drained on the result sequence. This causes the elements to be delayed.
        /// </remarks>
        public static IObservable<TSource> TakeLast<TSource>(this IObservable<TSource> source, int count, IScheduler scheduler)
        {
            if (source == null)
            {
                throw new ArgumentNullException(nameof(source));
            }

            if (count < 0)
            {
                throw new ArgumentOutOfRangeException(nameof(count));
            }

            if (scheduler == null)
            {
                throw new ArgumentNullException(nameof(scheduler));
            }

            return s_impl.TakeLast(source, count, scheduler);
        }

        #endregion

        #region + TakeLastBuffer +

        /// <summary>
        /// Returns a list with the specified number of contiguous elements from the end of an observable sequence.
        /// </summary>
        /// <typeparam name="TSource">The type of the elements in the source sequence.</typeparam>
        /// <param name="source">Source sequence.</param>
        /// <param name="count">Number of elements to take from the end of the source sequence.</param>
        /// <returns>An observable sequence containing a single list with the specified number of elements from the end of the source sequence.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="source"/> is null.</exception>
        /// <exception cref="ArgumentOutOfRangeException"><paramref name="count"/> is less than zero.</exception>
        /// <remarks>
        /// This operator accumulates a buffer with a length enough to store <paramref name="count"/> elements. Upon completion of the
        /// source sequence, this buffer is produced on the result sequence.
        /// </remarks>
        public static IObservable<IList<TSource>> TakeLastBuffer<TSource>(this IObservable<TSource> source, int count)
        {
            if (source == null)
            {
                throw new ArgumentNullException(nameof(source));
            }

            if (count < 0)
            {
                throw new ArgumentOutOfRangeException(nameof(count));
            }

            return s_impl.TakeLastBuffer(source, count);
        }

        #endregion

        #region + Window +

        /// <summary>
        /// Projects each element of an observable sequence into consecutive non-overlapping windows which are produced based on element count information.
        /// </summary>
        /// <typeparam name="TSource">The type of the elements in the source sequence, and in the windows in the result sequence.</typeparam>
        /// <param name="source">Source sequence to produce windows over.</param>
        /// <param name="count">Length of each window.</param>
        /// <returns>An observable sequence of windows.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="source"/> is null.</exception>
        /// <exception cref="ArgumentOutOfRangeException"><paramref name="count"/> is less than or equal to zero.</exception>
        public static IObservable<IObservable<TSource>> Window<TSource>(this IObservable<TSource> source, int count)
        {
            if (source == null)
            {
                throw new ArgumentNullException(nameof(source));
            }

            if (count <= 0)
            {
                throw new ArgumentOutOfRangeException(nameof(count));
            }

            return s_impl.Window(source, count);
        }

        /// <summary>
        /// Projects each element of an observable sequence into zero or more windows which are produced based on element count information.
        /// </summary>
        /// <typeparam name="TSource">The type of the elements in the source sequence, and in the windows in the result sequence.</typeparam>
        /// <param name="source">Source sequence to produce windows over.</param>
        /// <param name="count">Length of each window.</param>
        /// <param name="skip">Number of elements to skip between creation of consecutive windows.</param>
        /// <returns>An observable sequence of windows.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="source"/> is null.</exception>
        /// <exception cref="ArgumentOutOfRangeException"><paramref name="count"/> or <paramref name="skip"/> is less than or equal to zero.</exception>
        public static IObservable<IObservable<TSource>> Window<TSource>(this IObservable<TSource> source, int count, int skip)
        {
            if (source == null)
            {
                throw new ArgumentNullException(nameof(source));
            }

            if (count <= 0)
            {
                throw new ArgumentOutOfRangeException(nameof(count));
            }

            if (skip <= 0)
            {
                throw new ArgumentOutOfRangeException(nameof(skip));
            }

            return s_impl.Window(source, count, skip);
        }

        #endregion
    }
}

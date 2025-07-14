﻿// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information. 

using System.Collections.Generic;
using System.Linq;

namespace System.Reactive.Linq
{
    public static partial class Observable
    {
        #region + Aggregate +

        /// <summary>
        /// Applies an accumulator function over an observable sequence, returning the result of the aggregation as a single element in the result sequence. The specified seed value is used as the initial accumulator value.
        /// For aggregation behavior with incremental intermediate results, see <see cref="Observable.Scan{TSource, Accumulate}"/>.
        /// </summary>
        /// <typeparam name="TSource">The type of the elements in the source sequence.</typeparam>
        /// <typeparam name="TAccumulate">The type of the result of the aggregation.</typeparam>
        /// <param name="source">An observable sequence to aggregate over.</param>
        /// <param name="seed">The initial accumulator value.</param>
        /// <param name="accumulator">An accumulator function to be invoked on each element.</param>
        /// <returns>An observable sequence containing a single element with the final accumulator value.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="source"/> or <paramref name="accumulator"/> is null.</exception>
        /// <remarks>The return type of this operator differs from the corresponding operator on IEnumerable in order to retain asynchronous behavior.</remarks>
        public static IObservable<TAccumulate> Aggregate<TSource, TAccumulate>(this IObservable<TSource> source, TAccumulate seed, Func<TAccumulate, TSource, TAccumulate> accumulator)
        {
            if (source == null)
            {
                throw new ArgumentNullException(nameof(source));
            }

            if (accumulator == null)
            {
                throw new ArgumentNullException(nameof(accumulator));
            }

            return s_impl.Aggregate(source, seed, accumulator);
        }

        /// <summary>
        /// Applies an accumulator function over an observable sequence, returning the result of the aggregation as a single element in the result sequence. The specified seed value is used as the initial accumulator value,
        /// and the specified result selector function is used to select the result value.
        /// </summary>
        /// <typeparam name="TSource">The type of the elements in the source sequence.</typeparam>
        /// <typeparam name="TAccumulate">The type of the accumulator value.</typeparam>
        /// <typeparam name="TResult">The type of the resulting value.</typeparam>
        /// <param name="source">An observable sequence to aggregate over.</param>
        /// <param name="seed">The initial accumulator value.</param>
        /// <param name="accumulator">An accumulator function to be invoked on each element.</param>
        /// <param name="resultSelector">A function to transform the final accumulator value into the result value.</param>
        /// <returns>An observable sequence containing a single element with the final accumulator value.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="source"/> or <paramref name="accumulator"/> or <paramref name="resultSelector"/> is null.</exception>
        /// <remarks>The return type of this operator differs from the corresponding operator on IEnumerable in order to retain asynchronous behavior.</remarks>
        public static IObservable<TResult> Aggregate<TSource, TAccumulate, TResult>(this IObservable<TSource> source, TAccumulate seed, Func<TAccumulate, TSource, TAccumulate> accumulator, Func<TAccumulate, TResult> resultSelector)
        {
            if (source == null)
            {
                throw new ArgumentNullException(nameof(source));
            }

            if (accumulator == null)
            {
                throw new ArgumentNullException(nameof(accumulator));
            }

            if (resultSelector == null)
            {
                throw new ArgumentNullException(nameof(resultSelector));
            }

            return s_impl.Aggregate(source, seed, accumulator, resultSelector);
        }

        /// <summary>
        /// Applies an accumulator function over an observable sequence, returning the result of the aggregation as a single element in the result sequence.
        /// For aggregation behavior with incremental intermediate results, see <see cref="Observable.Scan{TSource}"/>.
        /// </summary>
        /// <typeparam name="TSource">The type of the elements in the source sequence and the result of the aggregation.</typeparam>
        /// <param name="source">An observable sequence to aggregate over.</param>
        /// <param name="accumulator">An accumulator function to be invoked on each element.</param>
        /// <returns>An observable sequence containing a single element with the final accumulator value.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="source"/> or <paramref name="accumulator"/> is null.</exception>
        /// <exception cref="InvalidOperationException">(Asynchronous) The source sequence is empty.</exception>
        /// <remarks>The return type of this operator differs from the corresponding operator on IEnumerable in order to retain asynchronous behavior.</remarks>
        public static IObservable<TSource> Aggregate<TSource>(this IObservable<TSource> source, Func<TSource, TSource, TSource> accumulator)
        {
            if (source == null)
            {
                throw new ArgumentNullException(nameof(source));
            }

            if (accumulator == null)
            {
                throw new ArgumentNullException(nameof(accumulator));
            }

            return s_impl.Aggregate(source, accumulator);
        }

        #endregion

        #region + All +

        /// <summary>
        /// Determines whether all elements of an observable sequence satisfy a condition.
        /// </summary>
        /// <typeparam name="TSource">The type of the elements in the source sequence.</typeparam>
        /// <param name="source">An observable sequence whose elements to apply the predicate to.</param>
        /// <param name="predicate">A function to test each element for a condition.</param>
        /// <returns>An observable sequence containing a single element determining whether all elements in the source sequence pass the test in the specified predicate.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="source"/> or <paramref name="predicate"/> is null.</exception>
        /// <remarks>The return type of this operator differs from the corresponding operator on IEnumerable in order to retain asynchronous behavior.</remarks>
        public static IObservable<bool> All<TSource>(this IObservable<TSource> source, Func<TSource, bool> predicate)
        {
            if (source == null)
            {
                throw new ArgumentNullException(nameof(source));
            }

            if (predicate == null)
            {
                throw new ArgumentNullException(nameof(predicate));
            }

            return s_impl.All(source, predicate);
        }

        #endregion

        #region + Any +

        /// <summary>
        /// Determines whether an observable sequence contains any elements.
        /// </summary>
        /// <typeparam name="TSource">The type of the elements in the source sequence.</typeparam>
        /// <param name="source">An observable sequence to check for non-emptiness.</param>
        /// <returns>An observable sequence containing a single element determining whether the source sequence contains any elements.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="source"/> is null.</exception>
        /// <remarks>The return type of this operator differs from the corresponding operator on IEnumerable in order to retain asynchronous behavior.</remarks>
        public static IObservable<bool> Any<TSource>(this IObservable<TSource> source)
        {
            if (source == null)
            {
                throw new ArgumentNullException(nameof(source));
            }

            return s_impl.Any(source);
        }

        /// <summary>
        /// Determines whether any element of an observable sequence satisfies a condition.
        /// </summary>
        /// <typeparam name="TSource">The type of the elements in the source sequence.</typeparam>
        /// <param name="source">An observable sequence whose elements to apply the predicate to.</param>
        /// <param name="predicate">A function to test each element for a condition.</param>
        /// <returns>An observable sequence containing a single element determining whether any elements in the source sequence pass the test in the specified predicate.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="source"/> or <paramref name="predicate"/> is null.</exception>
        /// <remarks>The return type of this operator differs from the corresponding operator on IEnumerable in order to retain asynchronous behavior.</remarks>
        public static IObservable<bool> Any<TSource>(this IObservable<TSource> source, Func<TSource, bool> predicate)
        {
            if (source == null)
            {
                throw new ArgumentNullException(nameof(source));
            }

            if (predicate == null)
            {
                throw new ArgumentNullException(nameof(predicate));
            }

            return s_impl.Any(source, predicate);
        }

        #endregion

        #region + Average +

        /// <summary>
        /// Computes the average of an observable sequence of <see cref="double" /> values.
        /// </summary>
        /// <param name="source">A sequence of <see cref="double" /> values to calculate the average of.</param>
        /// <returns>An observable sequence containing a single element with the average of the sequence of values.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="source"/> is null.</exception>
        /// <exception cref="InvalidOperationException">(Asynchronous) The source sequence is empty.</exception>
        /// <remarks>The return type of this operator differs from the corresponding operator on IEnumerable in order to retain asynchronous behavior.</remarks>
        public static IObservable<double> Average(this IObservable<double> source)
        {
            if (source == null)
            {
                throw new ArgumentNullException(nameof(source));
            }

            return s_impl.Average(source);
        }

        /// <summary>
        /// Computes the average of an observable sequence of <see cref="float" /> values.
        /// </summary>
        /// <param name="source">A sequence of <see cref="float" /> values to calculate the average of.</param>
        /// <returns>An observable sequence containing a single element with the average of the sequence of values.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="source"/> is null.</exception>
        /// <exception cref="InvalidOperationException">(Asynchronous) The source sequence is empty.</exception>
        /// <remarks>The return type of this operator differs from the corresponding operator on IEnumerable in order to retain asynchronous behavior.</remarks>
        public static IObservable<float> Average(this IObservable<float> source)
        {
            if (source == null)
            {
                throw new ArgumentNullException(nameof(source));
            }

            return s_impl.Average(source);
        }

        /// <summary>
        /// Computes the average of an observable sequence of <see cref="decimal" /> values.
        /// </summary>
        /// <param name="source">A sequence of <see cref="decimal" /> values to calculate the average of.</param>
        /// <returns>An observable sequence containing a single element with the average of the sequence of values.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="source"/> is null.</exception>
        /// <exception cref="InvalidOperationException">(Asynchronous) The source sequence is empty.</exception>
        /// <exception cref="OverflowException">(Asynchronous) The sum of the elements in the source sequence is larger than <see cref="decimal.MaxValue"/>.</exception>
        /// <remarks>The return type of this operator differs from the corresponding operator on IEnumerable in order to retain asynchronous behavior.</remarks>
        public static IObservable<decimal> Average(this IObservable<decimal> source)
        {
            if (source == null)
            {
                throw new ArgumentNullException(nameof(source));
            }

            return s_impl.Average(source);
        }

        /// <summary>
        /// Computes the average of an observable sequence of <see cref="int" /> values.
        /// </summary>
        /// <param name="source">A sequence of <see cref="int" /> values to calculate the average of.</param>
        /// <returns>An observable sequence containing a single element with the average of the sequence of values.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="source"/> is null.</exception>
        /// <exception cref="InvalidOperationException">(Asynchronous) The source sequence is empty.</exception>
        /// <exception cref="OverflowException">(Asynchronous) The sum of the elements in the source sequence is larger than <see cref="long.MaxValue"/>.</exception>
        /// <remarks>The return type of this operator differs from the corresponding operator on IEnumerable in order to retain asynchronous behavior.</remarks>
        public static IObservable<double> Average(this IObservable<int> source)
        {
            if (source == null)
            {
                throw new ArgumentNullException(nameof(source));
            }

            return s_impl.Average(source);
        }

        /// <summary>
        /// Computes the average of an observable sequence of <see cref="long" /> values.
        /// </summary>
        /// <param name="source">A sequence of <see cref="long" /> values to calculate the average of.</param>
        /// <returns>An observable sequence containing a single element with the average of the sequence of values.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="source"/> is null.</exception>
        /// <exception cref="InvalidOperationException">(Asynchronous) The source sequence is empty.</exception>
        /// <exception cref="OverflowException">(Asynchronous) The sum of the elements in the source sequence is larger than <see cref="long.MaxValue"/>.</exception>
        /// <remarks>The return type of this operator differs from the corresponding operator on IEnumerable in order to retain asynchronous behavior.</remarks>
        public static IObservable<double> Average(this IObservable<long> source)
        {
            if (source == null)
            {
                throw new ArgumentNullException(nameof(source));
            }

            return s_impl.Average(source);
        }

        /// <summary>
        /// Computes the average of an observable sequence of nullable <see cref="double" /> values.
        /// </summary>
        /// <param name="source">A sequence of nullable <see cref="double" /> values to calculate the average of.</param>
        /// <returns>An observable sequence containing a single element with the average of the sequence of values, or null if the source sequence is empty or contains only values that are null.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="source"/> is null.</exception>
        /// <exception cref="InvalidOperationException">(Asynchronous) The source sequence is empty.</exception>
        /// <remarks>The return type of this operator differs from the corresponding operator on IEnumerable in order to retain asynchronous behavior.</remarks>
        public static IObservable<double?> Average(this IObservable<double?> source)
        {
            if (source == null)
            {
                throw new ArgumentNullException(nameof(source));
            }

            return s_impl.Average(source);
        }

        /// <summary>
        /// Computes the average of an observable sequence of nullable <see cref="float" /> values.
        /// </summary>
        /// <param name="source">A sequence of nullable <see cref="float" /> values to calculate the average of.</param>
        /// <returns>An observable sequence containing a single element with the average of the sequence of values, or null if the source sequence is empty or contains only values that are null.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="source"/> is null.</exception>
        /// <exception cref="InvalidOperationException">(Asynchronous) The source sequence is empty.</exception>
        /// <remarks>The return type of this operator differs from the corresponding operator on IEnumerable in order to retain asynchronous behavior.</remarks>
        public static IObservable<float?> Average(this IObservable<float?> source)
        {
            if (source == null)
            {
                throw new ArgumentNullException(nameof(source));
            }

            return s_impl.Average(source);
        }

        /// <summary>
        /// Computes the average of an observable sequence of nullable <see cref="decimal" /> values.
        /// </summary>
        /// <param name="source">A sequence of nullable <see cref="decimal" /> values to calculate the average of.</param>
        /// <returns>An observable sequence containing a single element with the average of the sequence of values, or null if the source sequence is empty or contains only values that are null.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="source"/> is null.</exception>
        /// <exception cref="InvalidOperationException">(Asynchronous) The source sequence is empty.</exception>
        /// <exception cref="OverflowException">(Asynchronous) The sum of the elements in the source sequence is larger than <see cref="decimal.MaxValue"/>.</exception>
        /// <remarks>The return type of this operator differs from the corresponding operator on IEnumerable in order to retain asynchronous behavior.</remarks>
        public static IObservable<decimal?> Average(this IObservable<decimal?> source)
        {
            if (source == null)
            {
                throw new ArgumentNullException(nameof(source));
            }

            return s_impl.Average(source);
        }

        /// <summary>
        /// Computes the average of an observable sequence of nullable <see cref="int" /> values.
        /// </summary>
        /// <param name="source">A sequence of nullable <see cref="int" /> values to calculate the average of.</param>
        /// <returns>An observable sequence containing a single element with the average of the sequence of values, or null if the source sequence is empty or contains only values that are null.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="source"/> is null.</exception>
        /// <exception cref="InvalidOperationException">(Asynchronous) The source sequence is empty.</exception>
        /// <remarks>The return type of this operator differs from the corresponding operator on IEnumerable in order to retain asynchronous behavior.</remarks>
        /// <exception cref="OverflowException">(Asynchronous) The sum of the elements in the source sequence is larger than <see cref="long.MaxValue"/>.</exception>
        public static IObservable<double?> Average(this IObservable<int?> source)
        {
            if (source == null)
            {
                throw new ArgumentNullException(nameof(source));
            }

            return s_impl.Average(source);
        }

        /// <summary>
        /// Computes the average of an observable sequence of nullable <see cref="long" /> values.
        /// </summary>
        /// <param name="source">A sequence of nullable <see cref="long" /> values to calculate the average of.</param>
        /// <returns>An observable sequence containing a single element with the average of the sequence of values, or null if the source sequence is empty or contains only values that are null.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="source"/> is null.</exception>
        /// <exception cref="InvalidOperationException">(Asynchronous) The source sequence is empty.</exception>
        /// <exception cref="OverflowException">(Asynchronous) The sum of the elements in the source sequence is larger than <see cref="long.MaxValue"/>.</exception>
        /// <remarks>The return type of this operator differs from the corresponding operator on IEnumerable in order to retain asynchronous behavior.</remarks>
        public static IObservable<double?> Average(this IObservable<long?> source)
        {
            if (source == null)
            {
                throw new ArgumentNullException(nameof(source));
            }

            return s_impl.Average(source);
        }

        /// <summary>
        /// Computes the average of an observable sequence of <see cref="decimal" /> values that are obtained by invoking a transform function on each element of the input sequence.
        /// </summary>
        /// <typeparam name="TSource">The type of the elements in the source sequence.</typeparam>
        /// <param name="source">A sequence of values to calculate the average of.</param>
        /// <param name="selector">A transform function to apply to each element.</param>
        /// <returns>An observable sequence containing a single element with the average of the sequence of values.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="source"/> or <paramref name="selector"/> is null.</exception>
        /// <exception cref="InvalidOperationException">(Asynchronous) The source sequence is empty.</exception>
        /// <exception cref="OverflowException">(Asynchronous) The sum of the projected values for the elements in the source sequence is larger than <see cref="decimal.MaxValue"/>.</exception>
        /// <remarks>The return type of this operator differs from the corresponding operator on IEnumerable in order to retain asynchronous behavior.</remarks>
        public static IObservable<decimal> Average<TSource>(this IObservable<TSource> source, Func<TSource, decimal> selector)
        {
            if (source == null)
            {
                throw new ArgumentNullException(nameof(source));
            }

            if (selector == null)
            {
                throw new ArgumentNullException(nameof(selector));
            }

            return s_impl.Average(source, selector);
        }

        /// <summary>
        /// Computes the average of an observable sequence of <see cref="double" /> values that are obtained by invoking a transform function on each element of the input sequence.
        /// </summary>
        /// <typeparam name="TSource">The type of the elements in the source sequence.</typeparam>
        /// <param name="source">A sequence of values to calculate the average of.</param>
        /// <param name="selector">A transform function to apply to each element.</param>
        /// <returns>An observable sequence containing a single element with the average of the sequence of values.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="source"/> or <paramref name="selector"/> is null.</exception>
        /// <exception cref="InvalidOperationException">(Asynchronous) The source sequence is empty.</exception>
        /// <remarks>The return type of this operator differs from the corresponding operator on IEnumerable in order to retain asynchronous behavior.</remarks>
        public static IObservable<double> Average<TSource>(this IObservable<TSource> source, Func<TSource, double> selector)
        {
            if (source == null)
            {
                throw new ArgumentNullException(nameof(source));
            }

            if (selector == null)
            {
                throw new ArgumentNullException(nameof(selector));
            }

            return s_impl.Average(source, selector);
        }

        /// <summary>
        /// Computes the average of an observable sequence of <see cref="float" /> values that are obtained by invoking a transform function on each element of the input sequence.
        /// </summary>
        /// <typeparam name="TSource">The type of the elements in the source sequence.</typeparam>
        /// <param name="source">A sequence of values to calculate the average of.</param>
        /// <param name="selector">A transform function to apply to each element.</param>
        /// <returns>An observable sequence containing a single element with the average of the sequence of values.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="source"/> or <paramref name="selector"/> is null.</exception>
        /// <exception cref="InvalidOperationException">(Asynchronous) The source sequence is empty.</exception>
        /// <remarks>The return type of this operator differs from the corresponding operator on IEnumerable in order to retain asynchronous behavior.</remarks>
        public static IObservable<float> Average<TSource>(this IObservable<TSource> source, Func<TSource, float> selector)
        {
            if (source == null)
            {
                throw new ArgumentNullException(nameof(source));
            }

            if (selector == null)
            {
                throw new ArgumentNullException(nameof(selector));
            }

            return s_impl.Average(source, selector);
        }

        /// <summary>
        /// Computes the average of an observable sequence of <see cref="int" /> values that are obtained by invoking a transform function on each element of the input sequence.
        /// </summary>
        /// <typeparam name="TSource">The type of the elements in the source sequence.</typeparam>
        /// <param name="source">A sequence of values to calculate the average of.</param>
        /// <param name="selector">A transform function to apply to each element.</param>
        /// <returns>An observable sequence containing a single element with the average of the sequence of values.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="source"/> or <paramref name="selector"/> is null.</exception>
        /// <exception cref="InvalidOperationException">(Asynchronous) The source sequence is empty.</exception>
        /// <exception cref="OverflowException">(Asynchronous) The sum of the projected values for the elements in the source sequence is larger than <see cref="long.MaxValue"/>.</exception>
        /// <remarks>The return type of this operator differs from the corresponding operator on IEnumerable in order to retain asynchronous behavior.</remarks>
        public static IObservable<double> Average<TSource>(this IObservable<TSource> source, Func<TSource, int> selector)
        {
            if (source == null)
            {
                throw new ArgumentNullException(nameof(source));
            }

            if (selector == null)
            {
                throw new ArgumentNullException(nameof(selector));
            }

            return s_impl.Average(source, selector);
        }

        /// <summary>
        /// Computes the average of an observable sequence of <see cref="long" /> values that are obtained by invoking a transform function on each element of the input sequence.
        /// </summary>
        /// <typeparam name="TSource">The type of the elements in the source sequence.</typeparam>
        /// <param name="source">A sequence of values to calculate the average of.</param>
        /// <param name="selector">A transform function to apply to each element.</param>
        /// <returns>An observable sequence containing a single element with the average of the sequence of values.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="source"/> or <paramref name="selector"/> is null.</exception>
        /// <exception cref="InvalidOperationException">(Asynchronous) The source sequence is empty.</exception>
        /// <exception cref="OverflowException">(Asynchronous) The sum of the projected values for the elements in the source sequence is larger than <see cref="long.MaxValue"/>.</exception>
        /// <remarks>The return type of this operator differs from the corresponding operator on IEnumerable in order to retain asynchronous behavior.</remarks>
        public static IObservable<double> Average<TSource>(this IObservable<TSource> source, Func<TSource, long> selector)
        {
            if (source == null)
            {
                throw new ArgumentNullException(nameof(source));
            }

            if (selector == null)
            {
                throw new ArgumentNullException(nameof(selector));
            }

            return s_impl.Average(source, selector);
        }

        /// <summary>
        /// Computes the average of an observable sequence of nullable <see cref="decimal" /> values that are obtained by invoking a transform function on each element of the input sequence.
        /// </summary>
        /// <typeparam name="TSource">The type of the elements in the source sequence.</typeparam>
        /// <param name="source">A sequence of values to calculate the average of.</param>
        /// <param name="selector">A transform function to apply to each element.</param>
        /// <returns>An observable sequence containing a single element with the average of the sequence of values, or null if the source sequence is empty or contains only values that are null.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="source"/> or <paramref name="selector"/> is null.</exception>
        /// <exception cref="InvalidOperationException">(Asynchronous) The source sequence is empty.</exception>
        /// <exception cref="OverflowException">(Asynchronous) The sum of the projected values for the elements in the source sequence is larger than <see cref="decimal.MaxValue"/>.</exception>
        /// <remarks>The return type of this operator differs from the corresponding operator on IEnumerable in order to retain asynchronous behavior.</remarks>
        public static IObservable<decimal?> Average<TSource>(this IObservable<TSource> source, Func<TSource, decimal?> selector)
        {
            if (source == null)
            {
                throw new ArgumentNullException(nameof(source));
            }

            if (selector == null)
            {
                throw new ArgumentNullException(nameof(selector));
            }

            return s_impl.Average(source, selector);
        }

        /// <summary>
        /// Computes the average of an observable sequence of nullable <see cref="double" /> values that are obtained by invoking a transform function on each element of the input sequence.
        /// </summary>
        /// <typeparam name="TSource">The type of the elements in the source sequence.</typeparam>
        /// <param name="source">A sequence of values to calculate the average of.</param>
        /// <param name="selector">A transform function to apply to each element.</param>
        /// <returns>An observable sequence containing a single element with the average of the sequence of values, or null if the source sequence is empty or contains only values that are null.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="source"/> or <paramref name="selector"/> is null.</exception>
        /// <exception cref="InvalidOperationException">(Asynchronous) The source sequence is empty.</exception>
        /// <remarks>The return type of this operator differs from the corresponding operator on IEnumerable in order to retain asynchronous behavior.</remarks>
        public static IObservable<double?> Average<TSource>(this IObservable<TSource> source, Func<TSource, double?> selector)
        {
            if (source == null)
            {
                throw new ArgumentNullException(nameof(source));
            }

            if (selector == null)
            {
                throw new ArgumentNullException(nameof(selector));
            }

            return s_impl.Average(source, selector);
        }

        /// <summary>
        /// Computes the average of an observable sequence of nullable <see cref="float" /> values that are obtained by invoking a transform function on each element of the input sequence.
        /// </summary>
        /// <typeparam name="TSource">The type of the elements in the source sequence.</typeparam>
        /// <param name="source">A sequence of values to calculate the average of.</param>
        /// <param name="selector">A transform function to apply to each element.</param>
        /// <returns>An observable sequence containing a single element with the average of the sequence of values, or null if the source sequence is empty or contains only values that are null.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="source"/> or <paramref name="selector"/> is null.</exception>
        /// <exception cref="InvalidOperationException">(Asynchronous) The source sequence is empty.</exception>
        /// <remarks>The return type of this operator differs from the corresponding operator on IEnumerable in order to retain asynchronous behavior.</remarks>
        public static IObservable<float?> Average<TSource>(this IObservable<TSource> source, Func<TSource, float?> selector)
        {
            if (source == null)
            {
                throw new ArgumentNullException(nameof(source));
            }

            if (selector == null)
            {
                throw new ArgumentNullException(nameof(selector));
            }

            return s_impl.Average(source, selector);
        }

        /// <summary>
        /// Computes the average of an observable sequence of nullable <see cref="int" /> values that are obtained by invoking a transform function on each element of the input sequence.
        /// </summary>
        /// <typeparam name="TSource">The type of the elements in the source sequence.</typeparam>
        /// <param name="source">A sequence of values to calculate the average of.</param>
        /// <param name="selector">A transform function to apply to each element.</param>
        /// <returns>An observable sequence containing a single element with the average of the sequence of values, or null if the source sequence is empty or contains only values that are null.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="source"/> or <paramref name="selector"/> is null.</exception>
        /// <exception cref="InvalidOperationException">(Asynchronous) The source sequence is empty.</exception>
        /// <exception cref="OverflowException">(Asynchronous) The sum of the projected values for the elements in the source sequence is larger than <see cref="long.MaxValue"/>.</exception>
        /// <remarks>The return type of this operator differs from the corresponding operator on IEnumerable in order to retain asynchronous behavior.</remarks>
        public static IObservable<double?> Average<TSource>(this IObservable<TSource> source, Func<TSource, int?> selector)
        {
            if (source == null)
            {
                throw new ArgumentNullException(nameof(source));
            }

            if (selector == null)
            {
                throw new ArgumentNullException(nameof(selector));
            }

            return s_impl.Average(source, selector);
        }

        /// <summary>
        /// Computes the average of an observable sequence of nullable <see cref="long" /> values that are obtained by invoking a transform function on each element of the input sequence.
        /// </summary>
        /// <typeparam name="TSource">The type of the elements in the source sequence.</typeparam>
        /// <param name="source">A sequence of values to calculate the average of.</param>
        /// <param name="selector">A transform function to apply to each element.</param>
        /// <returns>An observable sequence containing a single element with the average of the sequence of values, or null if the source sequence is empty or contains only values that are null.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="source"/> or <paramref name="selector"/> is null.</exception>
        /// <exception cref="InvalidOperationException">(Asynchronous) The source sequence is empty.</exception>
        /// <exception cref="OverflowException">(Asynchronous) The sum of the projected values for the elements in the source sequence is larger than <see cref="long.MaxValue"/>.</exception>
        /// <remarks>The return type of this operator differs from the corresponding operator on IEnumerable in order to retain asynchronous behavior.</remarks>
        public static IObservable<double?> Average<TSource>(this IObservable<TSource> source, Func<TSource, long?> selector)
        {
            if (source == null)
            {
                throw new ArgumentNullException(nameof(source));
            }

            if (selector == null)
            {
                throw new ArgumentNullException(nameof(selector));
            }

            return s_impl.Average(source, selector);
        }

        #endregion

        #region + Contains +

        /// <summary>
        /// Determines whether an observable sequence contains a specified element by using the default equality comparer.
        /// </summary>
        /// <typeparam name="TSource">The type of the elements in the source sequence.</typeparam>
        /// <param name="source">An observable sequence in which to locate a value.</param>
        /// <param name="value">The value to locate in the source sequence.</param>
        /// <returns>An observable sequence containing a single element determining whether the source sequence contains an element that has the specified value.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="source"/> is null.</exception>
        /// <remarks>The return type of this operator differs from the corresponding operator on IEnumerable in order to retain asynchronous behavior.</remarks>
        public static IObservable<bool> Contains<TSource>(this IObservable<TSource> source, TSource value)
        {
            if (source == null)
            {
                throw new ArgumentNullException(nameof(source));
            }

            return s_impl.Contains(source, value);
        }

        /// <summary>
        /// Determines whether an observable sequence contains a specified element by using a specified System.Collections.Generic.IEqualityComparer{T}.
        /// </summary>
        /// <typeparam name="TSource">The type of the elements in the source sequence.</typeparam>
        /// <param name="source">An observable sequence in which to locate a value.</param>
        /// <param name="value">The value to locate in the source sequence.</param>
        /// <param name="comparer">An equality comparer to compare elements.</param>
        /// <returns>An observable sequence containing a single element determining whether the source sequence contains an element that has the specified value.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="source"/> or <paramref name="comparer"/> is null.</exception>
        /// <remarks>The return type of this operator differs from the corresponding operator on IEnumerable in order to retain asynchronous behavior.</remarks>
        public static IObservable<bool> Contains<TSource>(this IObservable<TSource> source, TSource value, IEqualityComparer<TSource> comparer)
        {
            if (source == null)
            {
                throw new ArgumentNullException(nameof(source));
            }

            if (comparer == null)
            {
                throw new ArgumentNullException(nameof(comparer));
            }

            return s_impl.Contains(source, value, comparer);
        }

        #endregion

        #region + Count +

        /// <summary>
        /// Returns an observable sequence containing an <see cref="int" /> that represents the total number of elements in an observable sequence.
        /// </summary>
        /// <typeparam name="TSource">The type of the elements in the source sequence.</typeparam>
        /// <param name="source">An observable sequence that contains elements to be counted.</param>
        /// <returns>An observable sequence containing a single element with the number of elements in the input sequence.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="source"/> is null.</exception>
        /// <exception cref="OverflowException">(Asynchronous) The number of elements in the source sequence is larger than <see cref="long.MaxValue"/>.</exception>
        /// <remarks>The return type of this operator differs from the corresponding operator on IEnumerable in order to retain asynchronous behavior.</remarks>
        public static IObservable<int> Count<TSource>(this IObservable<TSource> source)
        {
            if (source == null)
            {
                throw new ArgumentNullException(nameof(source));
            }

            return s_impl.Count(source);
        }

        /// <summary>
        /// Returns an observable sequence containing an <see cref="int" /> that represents how many elements in the specified observable sequence satisfy a condition.
        /// </summary>
        /// <typeparam name="TSource">The type of the elements in the source sequence.</typeparam>
        /// <param name="source">An observable sequence that contains elements to be counted.</param>
        /// <param name="predicate">A function to test each element for a condition.</param>
        /// <returns>An observable sequence containing a single element with a number that represents how many elements in the input sequence satisfy the condition in the predicate function.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="source"/> or <paramref name="predicate"/> is null.</exception>
        /// <remarks>The return type of this operator differs from the corresponding operator on IEnumerable in order to retain asynchronous behavior.</remarks>
        public static IObservable<int> Count<TSource>(this IObservable<TSource> source, Func<TSource, bool> predicate)
        {
            if (source == null)
            {
                throw new ArgumentNullException(nameof(source));
            }

            if (predicate == null)
            {
                throw new ArgumentNullException(nameof(predicate));
            }

            return s_impl.Count(source, predicate);
        }

        #endregion

        #region + ElementAt +

        /// <summary>
        /// Returns the element at a specified index in a sequence.
        /// </summary>
        /// <typeparam name="TSource">The type of the elements in the source sequence.</typeparam>
        /// <param name="source">Observable sequence to return the element from.</param>
        /// <param name="index">The zero-based index of the element to retrieve.</param>
        /// <returns>An observable sequence that produces the element at the specified position in the source sequence.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="source"/> is null.</exception>
        /// <exception cref="ArgumentOutOfRangeException"><paramref name="index"/> is less than zero.</exception>
        /// <exception cref="ArgumentOutOfRangeException">(Asynchronous) <paramref name="index"/> is greater than or equal to the number of elements in the source sequence.</exception>
        public static IObservable<TSource> ElementAt<TSource>(this IObservable<TSource> source, int index)
        {
            if (source == null)
            {
                throw new ArgumentNullException(nameof(source));
            }

            if (index < 0)
            {
                throw new ArgumentOutOfRangeException(nameof(index));
            }

            return s_impl.ElementAt(source, index);
        }

        #endregion

        #region + ElementAtOrDefault +

        /// <summary>
        /// Returns the element at a specified index in a sequence or a default value if the index is out of range.
        /// </summary>
        /// <typeparam name="TSource">The type of the elements in the source sequence.</typeparam>
        /// <param name="source">Observable sequence to return the element from.</param>
        /// <param name="index">The zero-based index of the element to retrieve.</param>
        /// <returns>An observable sequence that produces the element at the specified position in the source sequence, or a default value if the index is outside the bounds of the source sequence.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="source"/> is null.</exception>
        /// <exception cref="ArgumentOutOfRangeException"><paramref name="index"/> is less than zero.</exception>
        public static IObservable<TSource?> ElementAtOrDefault<TSource>(this IObservable<TSource> source, int index)
        {
            if (source == null)
            {
                throw new ArgumentNullException(nameof(source));
            }

            if (index < 0)
            {
                throw new ArgumentOutOfRangeException(nameof(index));
            }

            return s_impl.ElementAtOrDefault(source, index);
        }

        #endregion

        #region + FirstAsync +

        /// <summary>
        /// Returns the first element of an observable sequence.
        /// </summary>
        /// <typeparam name="TSource">The type of the elements in the source sequence.</typeparam>
        /// <param name="source">Source observable sequence.</param>
        /// <returns>Sequence containing the first element in the observable sequence.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="source"/> is null.</exception>
        /// <exception cref="InvalidOperationException">(Asynchronous) The source sequence is empty.</exception>
        public static IObservable<TSource> FirstAsync<TSource>(this IObservable<TSource> source)
        {
            if (source == null)
            {
                throw new ArgumentNullException(nameof(source));
            }

            return s_impl.FirstAsync(source);
        }

        /// <summary>
        /// Returns the first element of an observable sequence that satisfies the condition in the predicate.
        /// </summary>
        /// <typeparam name="TSource">The type of the elements in the source sequence.</typeparam>
        /// <param name="source">Source observable sequence.</param>
        /// <param name="predicate">A predicate function to evaluate for elements in the source sequence.</param>
        /// <returns>Sequence containing the first element in the observable sequence that satisfies the condition in the predicate.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="source"/> or <paramref name="predicate"/> is null.</exception>
        /// <exception cref="InvalidOperationException">(Asynchronous) No element satisfies the condition in the predicate. -or- The source sequence is empty.</exception>
        public static IObservable<TSource> FirstAsync<TSource>(this IObservable<TSource> source, Func<TSource, bool> predicate)
        {
            if (source == null)
            {
                throw new ArgumentNullException(nameof(source));
            }

            if (predicate == null)
            {
                throw new ArgumentNullException(nameof(predicate));
            }

            return s_impl.FirstAsync(source, predicate);
        }

        #endregion

        #region + FirstOrDefaultAsync +

        /// <summary>
        /// Returns the first element of an observable sequence, or a default value if no such element exists.
        /// </summary>
        /// <typeparam name="TSource">The type of the elements in the source sequence.</typeparam>
        /// <param name="source">Source observable sequence.</param>
        /// <returns>Sequence containing the first element in the observable sequence, or a default value if no such element exists.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="source"/> is null.</exception>
        public static IObservable<TSource?> FirstOrDefaultAsync<TSource>(this IObservable<TSource> source)
        {
            if (source == null)
            {
                throw new ArgumentNullException(nameof(source));
            }

            return s_impl.FirstOrDefaultAsync(source);
        }

        /// <summary>
        /// Returns the first element of an observable sequence that satisfies the condition in the predicate, or a default value if no such element exists.
        /// </summary>
        /// <typeparam name="TSource">The type of the elements in the source sequence.</typeparam>
        /// <param name="source">Source observable sequence.</param>
        /// <param name="predicate">A predicate function to evaluate for elements in the source sequence.</param>
        /// <returns>Sequence containing the first element in the observable sequence that satisfies the condition in the predicate, or a default value if no such element exists.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="source"/> or <paramref name="predicate"/> is null.</exception>
        public static IObservable<TSource?> FirstOrDefaultAsync<TSource>(this IObservable<TSource> source, Func<TSource, bool> predicate)
        {
            if (source == null)
            {
                throw new ArgumentNullException(nameof(source));
            }

            if (predicate == null)
            {
                throw new ArgumentNullException(nameof(predicate));
            }

            return s_impl.FirstOrDefaultAsync(source, predicate);
        }

        #endregion

        #region + IsEmpty +

        /// <summary>
        /// Determines whether an observable sequence is empty.
        /// </summary>
        /// <typeparam name="TSource">The type of the elements in the source sequence.</typeparam>
        /// <param name="source">An observable sequence to check for emptiness.</param>
        /// <returns>An observable sequence containing a single element determining whether the source sequence is empty.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="source"/> is null.</exception>
        public static IObservable<bool> IsEmpty<TSource>(this IObservable<TSource> source)
        {
            if (source == null)
            {
                throw new ArgumentNullException(nameof(source));
            }

            return s_impl.IsEmpty(source);
        }

        #endregion

        #region + LastAsync +

        /// <summary>
        /// Returns the last element of an observable sequence.
        /// </summary>
        /// <typeparam name="TSource">The type of the elements in the source sequence.</typeparam>
        /// <param name="source">Source observable sequence.</param>
        /// <returns>Sequence containing the last element in the observable sequence.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="source"/> is null.</exception>
        /// <exception cref="InvalidOperationException">(Asynchronous) The source sequence is empty.</exception>
        public static IObservable<TSource> LastAsync<TSource>(this IObservable<TSource> source)
        {
            if (source == null)
            {
                throw new ArgumentNullException(nameof(source));
            }

            return s_impl.LastAsync(source);
        }

        /// <summary>
        /// Returns the last element of an observable sequence that satisfies the condition in the predicate.
        /// </summary>
        /// <typeparam name="TSource">The type of the elements in the source sequence.</typeparam>
        /// <param name="source">Source observable sequence.</param>
        /// <param name="predicate">A predicate function to evaluate for elements in the source sequence.</param>
        /// <returns>Sequence containing the last element in the observable sequence that satisfies the condition in the predicate.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="source"/> or <paramref name="predicate"/> is null.</exception>
        /// <exception cref="InvalidOperationException">(Asynchronous) No element satisfies the condition in the predicate. -or- The source sequence is empty.</exception>
        public static IObservable<TSource> LastAsync<TSource>(this IObservable<TSource> source, Func<TSource, bool> predicate)
        {
            if (source == null)
            {
                throw new ArgumentNullException(nameof(source));
            }

            if (predicate == null)
            {
                throw new ArgumentNullException(nameof(predicate));
            }

            return s_impl.LastAsync(source, predicate);
        }

        #endregion

        #region + LastOrDefaultAsync +

        /// <summary>
        /// Returns the last element of an observable sequence, or a default value if no such element exists.
        /// </summary>
        /// <typeparam name="TSource">The type of the elements in the source sequence.</typeparam>
        /// <param name="source">Source observable sequence.</param>
        /// <returns>Sequence containing the last element in the observable sequence, or a default value if no such element exists.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="source"/> is null.</exception>
        public static IObservable<TSource?> LastOrDefaultAsync<TSource>(this IObservable<TSource> source)
        {
            if (source == null)
            {
                throw new ArgumentNullException(nameof(source));
            }

            return s_impl.LastOrDefaultAsync(source);
        }

        /// <summary>
        /// Returns the last element of an observable sequence that satisfies the condition in the predicate, or a default value if no such element exists.
        /// </summary>
        /// <typeparam name="TSource">The type of the elements in the source sequence.</typeparam>
        /// <param name="source">Source observable sequence.</param>
        /// <param name="predicate">A predicate function to evaluate for elements in the source sequence.</param>
        /// <returns>Sequence containing the last element in the observable sequence that satisfies the condition in the predicate, or a default value if no such element exists.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="source"/> or <paramref name="predicate"/> is null.</exception>
        public static IObservable<TSource?> LastOrDefaultAsync<TSource>(this IObservable<TSource> source, Func<TSource, bool> predicate)
        {
            if (source == null)
            {
                throw new ArgumentNullException(nameof(source));
            }

            if (predicate == null)
            {
                throw new ArgumentNullException(nameof(predicate));
            }

            return s_impl.LastOrDefaultAsync(source, predicate);
        }

        #endregion

        #region + LongCount +

        /// <summary>
        /// Returns an observable sequence containing an <see cref="long" /> that represents the total number of elements in an observable sequence.
        /// </summary>
        /// <typeparam name="TSource">The type of the elements in the source sequence.</typeparam>
        /// <param name="source">An observable sequence that contains elements to be counted.</param>
        /// <returns>An observable sequence containing a single element with the number of elements in the input sequence.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="source"/> is null.</exception>
        /// <exception cref="OverflowException">(Asynchronous) The number of elements in the source sequence is larger than <see cref="long.MaxValue"/>.</exception>
        /// <remarks>The return type of this operator differs from the corresponding operator on IEnumerable in order to retain asynchronous behavior.</remarks>
        public static IObservable<long> LongCount<TSource>(this IObservable<TSource> source)
        {
            if (source == null)
            {
                throw new ArgumentNullException(nameof(source));
            }

            return s_impl.LongCount(source);
        }

        /// <summary>
        /// Returns an observable sequence containing an <see cref="long" /> that represents how many elements in the specified observable sequence satisfy a condition.
        /// </summary>
        /// <typeparam name="TSource">The type of the elements in the source sequence.</typeparam>
        /// <param name="source">An observable sequence that contains elements to be counted.</param>
        /// <param name="predicate">A function to test each element for a condition.</param>
        /// <returns>An observable sequence containing a single element with a number that represents how many elements in the input sequence satisfy the condition in the predicate function.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="source"/> or <paramref name="predicate"/> is null.</exception>
        /// <remarks>The return type of this operator differs from the corresponding operator on IEnumerable in order to retain asynchronous behavior.</remarks>
        public static IObservable<long> LongCount<TSource>(this IObservable<TSource> source, Func<TSource, bool> predicate)
        {
            if (source == null)
            {
                throw new ArgumentNullException(nameof(source));
            }

            if (predicate == null)
            {
                throw new ArgumentNullException(nameof(predicate));
            }

            return s_impl.LongCount(source, predicate);
        }

        #endregion

        #region + Max +

        /// <summary>
        /// Returns the maximum element in an observable sequence.
        /// </summary>
        /// <typeparam name="TSource">The type of the elements in the source sequence.</typeparam>
        /// <param name="source">An observable sequence to determine the maximum element of.</param>
        /// <returns>An observable sequence containing a single element with the maximum element in the source sequence.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="source"/> is null.</exception>
        /// <remarks>The return type of this operator differs from the corresponding operator on IEnumerable in order to retain asynchronous behavior.</remarks>
        public static IObservable<TSource> Max<TSource>(this IObservable<TSource> source)
        {
            if (source == null)
            {
                throw new ArgumentNullException(nameof(source));
            }

            return s_impl.Max(source);
        }

        /// <summary>
        /// Returns the maximum value in an observable sequence according to the specified comparer.
        /// </summary>
        /// <typeparam name="TSource">The type of the elements in the source sequence.</typeparam>
        /// <param name="source">An observable sequence to determine the maximum element of.</param>
        /// <param name="comparer">Comparer used to compare elements.</param>
        /// <returns>An observable sequence containing a single element with the maximum element in the source sequence.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="source"/> or <paramref name="comparer"/> is null.</exception>
        /// <remarks>The return type of this operator differs from the corresponding operator on IEnumerable in order to retain asynchronous behavior.</remarks>
        public static IObservable<TSource> Max<TSource>(this IObservable<TSource> source, IComparer<TSource> comparer)
        {
            if (source == null)
            {
                throw new ArgumentNullException(nameof(source));
            }

            if (comparer == null)
            {
                throw new ArgumentNullException(nameof(comparer));
            }

            return s_impl.Max(source, comparer);
        }

        /// <summary>
        /// Returns the maximum value in an observable sequence of <see cref="double" /> values.
        /// </summary>
        /// <param name="source">A sequence of <see cref="double" /> values to determine the maximum value of.</param>
        /// <returns>An observable sequence containing a single element with the maximum value in the source sequence.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="source"/> is null.</exception>
        /// <remarks>The return type of this operator differs from the corresponding operator on IEnumerable in order to retain asynchronous behavior.</remarks>
        public static IObservable<double> Max(this IObservable<double> source)
        {
            if (source == null)
            {
                throw new ArgumentNullException(nameof(source));
            }

            return s_impl.Max(source);
        }

        /// <summary>
        /// Returns the maximum value in an observable sequence of <see cref="float" /> values.
        /// </summary>
        /// <param name="source">A sequence of <see cref="float" /> values to determine the maximum value of.</param>
        /// <returns>An observable sequence containing a single element with the maximum value in the source sequence.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="source"/> is null.</exception>
        /// <remarks>The return type of this operator differs from the corresponding operator on IEnumerable in order to retain asynchronous behavior.</remarks>
        public static IObservable<float> Max(this IObservable<float> source)
        {
            if (source == null)
            {
                throw new ArgumentNullException(nameof(source));
            }

            return s_impl.Max(source);
        }

        /// <summary>
        /// Returns the maximum value in an observable sequence of <see cref="decimal" /> values.
        /// </summary>
        /// <param name="source">A sequence of <see cref="decimal" /> values to determine the maximum value of.</param>
        /// <returns>An observable sequence containing a single element with the maximum value in the source sequence.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="source"/> is null.</exception>
        /// <remarks>The return type of this operator differs from the corresponding operator on IEnumerable in order to retain asynchronous behavior.</remarks>
        public static IObservable<decimal> Max(this IObservable<decimal> source)
        {
            if (source == null)
            {
                throw new ArgumentNullException(nameof(source));
            }

            return s_impl.Max(source);
        }

        /// <summary>
        /// Returns the maximum value in an observable sequence of <see cref="int" /> values.
        /// </summary>
        /// <param name="source">A sequence of <see cref="int" /> values to determine the maximum value of.</param>
        /// <returns>An observable sequence containing a single element with the maximum value in the source sequence.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="source"/> is null.</exception>
        /// <remarks>The return type of this operator differs from the corresponding operator on IEnumerable in order to retain asynchronous behavior.</remarks>
        public static IObservable<int> Max(this IObservable<int> source)
        {
            if (source == null)
            {
                throw new ArgumentNullException(nameof(source));
            }

            return s_impl.Max(source);
        }

        /// <summary>
        /// Returns the maximum value in an observable sequence of <see cref="long" /> values.
        /// </summary>
        /// <param name="source">A sequence of <see cref="long" /> values to determine the maximum value of.</param>
        /// <returns>An observable sequence containing a single element with the maximum value in the source sequence.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="source"/> is null.</exception>
        /// <remarks>The return type of this operator differs from the corresponding operator on IEnumerable in order to retain asynchronous behavior.</remarks>
        public static IObservable<long> Max(this IObservable<long> source)
        {
            if (source == null)
            {
                throw new ArgumentNullException(nameof(source));
            }

            return s_impl.Max(source);
        }

        /// <summary>
        /// Returns the maximum value in an observable sequence of nullable <see cref="double" /> values.
        /// </summary>
        /// <param name="source">A sequence of nullable <see cref="double" /> values to determine the maximum value of.</param>
        /// <returns>An observable sequence containing a single element with the maximum value in the source sequence.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="source"/> is null.</exception>
        /// <remarks>The return type of this operator differs from the corresponding operator on IEnumerable in order to retain asynchronous behavior.</remarks>
        public static IObservable<double?> Max(this IObservable<double?> source)
        {
            if (source == null)
            {
                throw new ArgumentNullException(nameof(source));
            }

            return s_impl.Max(source);
        }

        /// <summary>
        /// Returns the maximum value in an observable sequence of nullable <see cref="float" /> values.
        /// </summary>
        /// <param name="source">A sequence of nullable <see cref="float" /> values to determine the maximum value of.</param>
        /// <returns>An observable sequence containing a single element with the maximum value in the source sequence.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="source"/> is null.</exception>
        /// <remarks>The return type of this operator differs from the corresponding operator on IEnumerable in order to retain asynchronous behavior.</remarks>
        public static IObservable<float?> Max(this IObservable<float?> source)
        {
            if (source == null)
            {
                throw new ArgumentNullException(nameof(source));
            }

            return s_impl.Max(source);
        }

        /// <summary>
        /// Returns the maximum value in an observable sequence of nullable <see cref="decimal" /> values.
        /// </summary>
        /// <param name="source">A sequence of nullable <see cref="decimal" /> values to determine the maximum value of.</param>
        /// <returns>An observable sequence containing a single element with the maximum value in the source sequence.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="source"/> is null.</exception>
        /// <remarks>The return type of this operator differs from the corresponding operator on IEnumerable in order to retain asynchronous behavior.</remarks>
        public static IObservable<decimal?> Max(this IObservable<decimal?> source)
        {
            if (source == null)
            {
                throw new ArgumentNullException(nameof(source));
            }

            return s_impl.Max(source);
        }

        /// <summary>
        /// Returns the maximum value in an observable sequence of nullable <see cref="int" /> values.
        /// </summary>
        /// <param name="source">A sequence of nullable <see cref="int" /> values to determine the maximum value of.</param>
        /// <returns>An observable sequence containing a single element with the maximum value in the source sequence.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="source"/> is null.</exception>
        /// <remarks>The return type of this operator differs from the corresponding operator on IEnumerable in order to retain asynchronous behavior.</remarks>
        public static IObservable<int?> Max(this IObservable<int?> source)
        {
            if (source == null)
            {
                throw new ArgumentNullException(nameof(source));
            }

            return s_impl.Max(source);
        }

        /// <summary>
        /// Returns the maximum value in an observable sequence of nullable <see cref="long" /> values.
        /// </summary>
        /// <param name="source">A sequence of nullable <see cref="long" /> values to determine the maximum value of.</param>
        /// <returns>An observable sequence containing a single element with the maximum value in the source sequence.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="source"/> is null.</exception>
        /// <remarks>The return type of this operator differs from the corresponding operator on IEnumerable in order to retain asynchronous behavior.</remarks>
        public static IObservable<long?> Max(this IObservable<long?> source)
        {
            if (source == null)
            {
                throw new ArgumentNullException(nameof(source));
            }

            return s_impl.Max(source);
        }

        /// <summary>
        /// Invokes a transform function on each element of a sequence and returns the maximum value.
        /// </summary>
        /// <typeparam name="TSource">The type of the elements in the source sequence.</typeparam>
        /// <typeparam name="TResult">The type of the objects derived from the elements in the source sequence to determine the maximum of.</typeparam>
        /// <param name="source">An observable sequence to determine the minimum element of.</param>
        /// <param name="selector">A transform function to apply to each element.</param>
        /// <returns>An observable sequence containing a single element with the value that corresponds to the maximum element in the source sequence.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="source"/> or <paramref name="selector"/> is null.</exception>
        /// <remarks>The return type of this operator differs from the corresponding operator on IEnumerable in order to retain asynchronous behavior.</remarks>
        public static IObservable<TResult> Max<TSource, TResult>(this IObservable<TSource> source, Func<TSource, TResult> selector)
        {
            if (source == null)
            {
                throw new ArgumentNullException(nameof(source));
            }

            if (selector == null)
            {
                throw new ArgumentNullException(nameof(selector));
            }

            return s_impl.Max(source, selector);
        }

        /// <summary>
        /// Invokes a transform function on each element of a sequence and returns the maximum value according to the specified comparer.
        /// </summary>
        /// <typeparam name="TSource">The type of the elements in the source sequence.</typeparam>
        /// <typeparam name="TResult">The type of the objects derived from the elements in the source sequence to determine the maximum of.</typeparam>
        /// <param name="source">An observable sequence to determine the minimum element of.</param>
        /// <param name="selector">A transform function to apply to each element.</param>
        /// <param name="comparer">Comparer used to compare elements.</param>
        /// <returns>An observable sequence containing a single element with the value that corresponds to the maximum element in the source sequence.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="source"/> or <paramref name="selector"/> or <paramref name="comparer"/> is null.</exception>
        /// <remarks>The return type of this operator differs from the corresponding operator on IEnumerable in order to retain asynchronous behavior.</remarks>
        public static IObservable<TResult> Max<TSource, TResult>(this IObservable<TSource> source, Func<TSource, TResult> selector, IComparer<TResult> comparer)
        {
            if (source == null)
            {
                throw new ArgumentNullException(nameof(source));
            }

            if (selector == null)
            {
                throw new ArgumentNullException(nameof(selector));
            }

            if (comparer == null)
            {
                throw new ArgumentNullException(nameof(comparer));
            }

            return s_impl.Max(source, selector, comparer);
        }

        /// <summary>
        /// Invokes a transform function on each element of a sequence and returns the maximum <see cref="double" /> value.
        /// </summary>
        /// <typeparam name="TSource">The type of the elements in the source sequence.</typeparam>
        /// <param name="source">A sequence of values to determine the maximum value of.</param>
        /// <param name="selector">A transform function to apply to each element.</param>
        /// <returns>An observable sequence containing a single element with the value of type <see cref="double" /> that corresponds to the maximum value in the source sequence.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="source"/> or <paramref name="selector"/> is null.</exception>
        /// <remarks>The return type of this operator differs from the corresponding operator on IEnumerable in order to retain asynchronous behavior.</remarks>
        public static IObservable<double> Max<TSource>(this IObservable<TSource> source, Func<TSource, double> selector)
        {
            if (source == null)
            {
                throw new ArgumentNullException(nameof(source));
            }

            if (selector == null)
            {
                throw new ArgumentNullException(nameof(selector));
            }

            return s_impl.Max(source, selector);
        }

        /// <summary>
        /// Invokes a transform function on each element of a sequence and returns the maximum <see cref="float" /> value.
        /// </summary>
        /// <typeparam name="TSource">The type of the elements in the source sequence.</typeparam>
        /// <param name="source">A sequence of values to determine the maximum value of.</param>
        /// <param name="selector">A transform function to apply to each element.</param>
        /// <returns>An observable sequence containing a single element with the value of type <see cref="float" /> that corresponds to the maximum value in the source sequence.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="source"/> or <paramref name="selector"/> is null.</exception>
        /// <remarks>The return type of this operator differs from the corresponding operator on IEnumerable in order to retain asynchronous behavior.</remarks>
        public static IObservable<float> Max<TSource>(this IObservable<TSource> source, Func<TSource, float> selector)
        {
            if (source == null)
            {
                throw new ArgumentNullException(nameof(source));
            }

            if (selector == null)
            {
                throw new ArgumentNullException(nameof(selector));
            }

            return s_impl.Max(source, selector);
        }

        /// <summary>
        /// Invokes a transform function on each element of a sequence and returns the maximum <see cref="decimal" /> value.
        /// </summary>
        /// <typeparam name="TSource">The type of the elements in the source sequence.</typeparam>
        /// <param name="source">A sequence of values to determine the maximum value of.</param>
        /// <param name="selector">A transform function to apply to each element.</param>
        /// <returns>An observable sequence containing a single element with the value of type <see cref="decimal" /> that corresponds to the maximum value in the source sequence.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="source"/> or <paramref name="selector"/> is null.</exception>
        /// <remarks>The return type of this operator differs from the corresponding operator on IEnumerable in order to retain asynchronous behavior.</remarks>
        public static IObservable<decimal> Max<TSource>(this IObservable<TSource> source, Func<TSource, decimal> selector)
        {
            if (source == null)
            {
                throw new ArgumentNullException(nameof(source));
            }

            if (selector == null)
            {
                throw new ArgumentNullException(nameof(selector));
            }

            return s_impl.Max(source, selector);
        }

        /// <summary>
        /// Invokes a transform function on each element of a sequence and returns the maximum <see cref="int" /> value.
        /// </summary>
        /// <typeparam name="TSource">The type of the elements in the source sequence.</typeparam>
        /// <param name="source">A sequence of values to determine the maximum value of.</param>
        /// <param name="selector">A transform function to apply to each element.</param>
        /// <returns>An observable sequence containing a single element with the value of type <see cref="int" /> that corresponds to the maximum value in the source sequence.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="source"/> or <paramref name="selector"/> is null.</exception>
        /// <remarks>The return type of this operator differs from the corresponding operator on IEnumerable in order to retain asynchronous behavior.</remarks>
        public static IObservable<int> Max<TSource>(this IObservable<TSource> source, Func<TSource, int> selector)
        {
            if (source == null)
            {
                throw new ArgumentNullException(nameof(source));
            }

            if (selector == null)
            {
                throw new ArgumentNullException(nameof(selector));
            }

            return s_impl.Max(source, selector);
        }

        /// <summary>
        /// Invokes a transform function on each element of a sequence and returns the maximum <see cref="long" /> value.
        /// </summary>
        /// <typeparam name="TSource">The type of the elements in the source sequence.</typeparam>
        /// <param name="source">A sequence of values to determine the maximum value of.</param>
        /// <param name="selector">A transform function to apply to each element.</param>
        /// <returns>An observable sequence containing a single element with the value of type <see cref="long" /> that corresponds to the maximum value in the source sequence.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="source"/> or <paramref name="selector"/> is null.</exception>
        /// <remarks>The return type of this operator differs from the corresponding operator on IEnumerable in order to retain asynchronous behavior.</remarks>
        public static IObservable<long> Max<TSource>(this IObservable<TSource> source, Func<TSource, long> selector)
        {
            if (source == null)
            {
                throw new ArgumentNullException(nameof(source));
            }

            if (selector == null)
            {
                throw new ArgumentNullException(nameof(selector));
            }

            return s_impl.Max(source, selector);
        }

        /// <summary>
        /// Invokes a transform function on each element of a sequence and returns the maximum nullable <see cref="double" /> value.
        /// </summary>
        /// <typeparam name="TSource">The type of the elements in the source sequence.</typeparam>
        /// <param name="source">A sequence of values to determine the maximum value of.</param>
        /// <param name="selector">A transform function to apply to each element.</param>
        /// <returns>An observable sequence containing a single element with the value of type <see cref="Nullable{Double}" /> that corresponds to the maximum value in the source sequence.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="source"/> or <paramref name="selector"/> is null.</exception>
        /// <remarks>The return type of this operator differs from the corresponding operator on IEnumerable in order to retain asynchronous behavior.</remarks>
        public static IObservable<double?> Max<TSource>(this IObservable<TSource> source, Func<TSource, double?> selector)
        {
            if (source == null)
            {
                throw new ArgumentNullException(nameof(source));
            }

            if (selector == null)
            {
                throw new ArgumentNullException(nameof(selector));
            }

            return s_impl.Max(source, selector);
        }

        /// <summary>
        /// Invokes a transform function on each element of a sequence and returns the maximum nullable <see cref="float" /> value.
        /// </summary>
        /// <typeparam name="TSource">The type of the elements in the source sequence.</typeparam>
        /// <param name="source">A sequence of values to determine the maximum value of.</param>
        /// <param name="selector">A transform function to apply to each element.</param>
        /// <returns>An observable sequence containing a single element with the value of type <see cref="Nullable{Single}" /> that corresponds to the maximum value in the source sequence.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="source"/> or <paramref name="selector"/> is null.</exception>
        /// <remarks>The return type of this operator differs from the corresponding operator on IEnumerable in order to retain asynchronous behavior.</remarks>
        public static IObservable<float?> Max<TSource>(this IObservable<TSource> source, Func<TSource, float?> selector)
        {
            if (source == null)
            {
                throw new ArgumentNullException(nameof(source));
            }

            if (selector == null)
            {
                throw new ArgumentNullException(nameof(selector));
            }

            return s_impl.Max(source, selector);
        }

        /// <summary>
        /// Invokes a transform function on each element of a sequence and returns the maximum nullable <see cref="decimal" /> value.
        /// </summary>
        /// <typeparam name="TSource">The type of the elements in the source sequence.</typeparam>
        /// <param name="source">A sequence of values to determine the maximum value of.</param>
        /// <param name="selector">A transform function to apply to each element.</param>
        /// <returns>An observable sequence containing a single element with the value of type <see cref="Nullable{Decimal}" /> that corresponds to the maximum value in the source sequence.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="source"/> or <paramref name="selector"/> is null.</exception>
        /// <remarks>The return type of this operator differs from the corresponding operator on IEnumerable in order to retain asynchronous behavior.</remarks>
        public static IObservable<decimal?> Max<TSource>(this IObservable<TSource> source, Func<TSource, decimal?> selector)
        {
            if (source == null)
            {
                throw new ArgumentNullException(nameof(source));
            }

            if (selector == null)
            {
                throw new ArgumentNullException(nameof(selector));
            }

            return s_impl.Max(source, selector);
        }

        /// <summary>
        /// Invokes a transform function on each element of a sequence and returns the maximum nullable <see cref="int" /> value.
        /// </summary>
        /// <typeparam name="TSource">The type of the elements in the source sequence.</typeparam>
        /// <param name="source">A sequence of values to determine the maximum value of.</param>
        /// <param name="selector">A transform function to apply to each element.</param>
        /// <returns>An observable sequence containing a single element with the value of type <see cref="Nullable{Int32}" /> that corresponds to the maximum value in the source sequence.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="source"/> or <paramref name="selector"/> is null.</exception>
        /// <remarks>The return type of this operator differs from the corresponding operator on IEnumerable in order to retain asynchronous behavior.</remarks>
        public static IObservable<int?> Max<TSource>(this IObservable<TSource> source, Func<TSource, int?> selector)
        {
            if (source == null)
            {
                throw new ArgumentNullException(nameof(source));
            }

            if (selector == null)
            {
                throw new ArgumentNullException(nameof(selector));
            }

            return s_impl.Max(source, selector);
        }

        /// <summary>
        /// Invokes a transform function on each element of a sequence and returns the maximum nullable <see cref="long" /> value.
        /// </summary>
        /// <typeparam name="TSource">The type of the elements in the source sequence.</typeparam>
        /// <param name="source">A sequence of values to determine the maximum value of.</param>
        /// <param name="selector">A transform function to apply to each element.</param>
        /// <returns>An observable sequence containing a single element with the value of type <see cref="Nullable{Int64}" /> that corresponds to the maximum value in the source sequence.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="source"/> or <paramref name="selector"/> is null.</exception>
        /// <remarks>The return type of this operator differs from the corresponding operator on IEnumerable in order to retain asynchronous behavior.</remarks>
        public static IObservable<long?> Max<TSource>(this IObservable<TSource> source, Func<TSource, long?> selector)
        {
            if (source == null)
            {
                throw new ArgumentNullException(nameof(source));
            }

            if (selector == null)
            {
                throw new ArgumentNullException(nameof(selector));
            }

            return s_impl.Max(source, selector);
        }

        #endregion

        #region + MaxBy +

        /// <summary>
        /// Returns the elements in an observable sequence with the maximum key value.
        /// </summary>
        /// <typeparam name="TSource">The type of the elements in the source sequence.</typeparam>
        /// <typeparam name="TKey">The type of the key computed for each element in the source sequence.</typeparam>
        /// <param name="source">An observable sequence to get the maximum elements for.</param>
        /// <param name="keySelector">Key selector function.</param>
        /// <returns>An observable sequence containing a list of zero or more elements that have a maximum key value.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="source"/> or <paramref name="keySelector"/> is null.</exception>
        /// <remarks>The return type of this operator differs from the corresponding operator on IEnumerable in order to retain asynchronous behavior.</remarks>
        public static IObservable<IList<TSource>> MaxBy<TSource, TKey>(this IObservable<TSource> source, Func<TSource, TKey> keySelector)
        {
            if (source == null)
            {
                throw new ArgumentNullException(nameof(source));
            }

            if (keySelector == null)
            {
                throw new ArgumentNullException(nameof(keySelector));
            }

            return s_impl.MaxBy(source, keySelector);
        }

        /// <summary>
        /// Returns the elements in an observable sequence with the maximum key value according to the specified comparer.
        /// </summary>
        /// <typeparam name="TSource">The type of the elements in the source sequence.</typeparam>
        /// <typeparam name="TKey">The type of the key computed for each element in the source sequence.</typeparam>
        /// <param name="source">An observable sequence to get the maximum elements for.</param>
        /// <param name="keySelector">Key selector function.</param>
        /// <param name="comparer">Comparer used to compare key values.</param>
        /// <returns>An observable sequence containing a list of zero or more elements that have a maximum key value.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="source"/> or <paramref name="keySelector"/> or <paramref name="comparer"/> is null.</exception>
        /// <remarks>The return type of this operator differs from the corresponding operator on IEnumerable in order to retain asynchronous behavior.</remarks>
        public static IObservable<IList<TSource>> MaxBy<TSource, TKey>(this IObservable<TSource> source, Func<TSource, TKey> keySelector, IComparer<TKey> comparer)
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

            return s_impl.MaxBy(source, keySelector, comparer);
        }

        #endregion

        #region + Min +

        /// <summary>
        /// Returns the minimum element in an observable sequence.
        /// </summary>
        /// <typeparam name="TSource">The type of the elements in the source sequence.</typeparam>
        /// <param name="source">An observable sequence to determine the minimum element of.</param>
        /// <returns>An observable sequence containing a single element with the minimum element in the source sequence.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="source"/> is null.</exception>
        /// <remarks>The return type of this operator differs from the corresponding operator on IEnumerable in order to retain asynchronous behavior.</remarks>
        public static IObservable<TSource> Min<TSource>(this IObservable<TSource> source)
        {
            if (source == null)
            {
                throw new ArgumentNullException(nameof(source));
            }

            return s_impl.Min(source);
        }

        /// <summary>
        /// Returns the minimum element in an observable sequence according to the specified comparer.
        /// </summary>
        /// <typeparam name="TSource">The type of the elements in the source sequence.</typeparam>
        /// <param name="source">An observable sequence to determine the minimum element of.</param>
        /// <param name="comparer">Comparer used to compare elements.</param>
        /// <returns>An observable sequence containing a single element with the minimum element in the source sequence.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="source"/> or <paramref name="comparer"/> is null.</exception>
        /// <remarks>The return type of this operator differs from the corresponding operator on IEnumerable in order to retain asynchronous behavior.</remarks>
        public static IObservable<TSource> Min<TSource>(this IObservable<TSource> source, IComparer<TSource> comparer)
        {
            if (source == null)
            {
                throw new ArgumentNullException(nameof(source));
            }

            if (comparer == null)
            {
                throw new ArgumentNullException(nameof(comparer));
            }

            return s_impl.Min(source, comparer);
        }

        /// <summary>
        /// Returns the minimum value in an observable sequence of <see cref="double" /> values.
        /// </summary>
        /// <param name="source">A sequence of <see cref="double" /> values to determine the minimum value of.</param>
        /// <returns>An observable sequence containing a single element with the minimum value in the source sequence.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="source"/> is null.</exception>
        /// <remarks>The return type of this operator differs from the corresponding operator on IEnumerable in order to retain asynchronous behavior.</remarks>
        public static IObservable<double> Min(this IObservable<double> source)
        {
            if (source == null)
            {
                throw new ArgumentNullException(nameof(source));
            }

            return s_impl.Min(source);
        }

        /// <summary>
        /// Returns the minimum value in an observable sequence of <see cref="float" /> values.
        /// </summary>
        /// <param name="source">A sequence of <see cref="float" /> values to determine the minimum value of.</param>
        /// <returns>An observable sequence containing a single element with the minimum value in the source sequence.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="source"/> is null.</exception>
        /// <remarks>The return type of this operator differs from the corresponding operator on IEnumerable in order to retain asynchronous behavior.</remarks>
        public static IObservable<float> Min(this IObservable<float> source)
        {
            if (source == null)
            {
                throw new ArgumentNullException(nameof(source));
            }

            return s_impl.Min(source);
        }

        /// <summary>
        /// Returns the minimum value in an observable sequence of <see cref="decimal" /> values.
        /// </summary>
        /// <param name="source">A sequence of <see cref="decimal" /> values to determine the minimum value of.</param>
        /// <returns>An observable sequence containing a single element with the minimum value in the source sequence.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="source"/> is null.</exception>
        /// <remarks>The return type of this operator differs from the corresponding operator on IEnumerable in order to retain asynchronous behavior.</remarks>
        public static IObservable<decimal> Min(this IObservable<decimal> source)
        {
            if (source == null)
            {
                throw new ArgumentNullException(nameof(source));
            }

            return s_impl.Min(source);
        }

        /// <summary>
        /// Returns the minimum value in an observable sequence of <see cref="int" /> values.
        /// </summary>
        /// <param name="source">A sequence of <see cref="int" /> values to determine the minimum value of.</param>
        /// <returns>An observable sequence containing a single element with the minimum value in the source sequence.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="source"/> is null.</exception>
        /// <remarks>The return type of this operator differs from the corresponding operator on IEnumerable in order to retain asynchronous behavior.</remarks>
        public static IObservable<int> Min(this IObservable<int> source)
        {
            if (source == null)
            {
                throw new ArgumentNullException(nameof(source));
            }

            return s_impl.Min(source);
        }

        /// <summary>
        /// Returns the minimum value in an observable sequence of <see cref="long" /> values.
        /// </summary>
        /// <param name="source">A sequence of <see cref="long" /> values to determine the minimum value of.</param>
        /// <returns>An observable sequence containing a single element with the minimum value in the source sequence.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="source"/> is null.</exception>
        /// <remarks>The return type of this operator differs from the corresponding operator on IEnumerable in order to retain asynchronous behavior.</remarks>
        public static IObservable<long> Min(this IObservable<long> source)
        {
            if (source == null)
            {
                throw new ArgumentNullException(nameof(source));
            }

            return s_impl.Min(source);
        }

        /// <summary>
        /// Returns the minimum value in an observable sequence of nullable <see cref="double" /> values.
        /// </summary>
        /// <param name="source">A sequence of nullable <see cref="double" /> values to determine the minimum value of.</param>
        /// <returns>An observable sequence containing a single element with the minimum value in the source sequence.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="source"/> is null.</exception>
        /// <remarks>The return type of this operator differs from the corresponding operator on IEnumerable in order to retain asynchronous behavior.</remarks>
        public static IObservable<double?> Min(this IObservable<double?> source)
        {
            if (source == null)
            {
                throw new ArgumentNullException(nameof(source));
            }

            return s_impl.Min(source);
        }

        /// <summary>
        /// Returns the minimum value in an observable sequence of nullable <see cref="float" /> values.
        /// </summary>
        /// <param name="source">A sequence of nullable <see cref="float" /> values to determine the minimum value of.</param>
        /// <returns>An observable sequence containing a single element with the minimum value in the source sequence.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="source"/> is null.</exception>
        /// <remarks>The return type of this operator differs from the corresponding operator on IEnumerable in order to retain asynchronous behavior.</remarks>
        public static IObservable<float?> Min(this IObservable<float?> source)
        {
            if (source == null)
            {
                throw new ArgumentNullException(nameof(source));
            }

            return s_impl.Min(source);
        }

        /// <summary>
        /// Returns the minimum value in an observable sequence of nullable <see cref="decimal" /> values.
        /// </summary>
        /// <param name="source">A sequence of nullable <see cref="decimal" /> values to determine the minimum value of.</param>
        /// <returns>An observable sequence containing a single element with the minimum value in the source sequence.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="source"/> is null.</exception>
        /// <remarks>The return type of this operator differs from the corresponding operator on IEnumerable in order to retain asynchronous behavior.</remarks>
        public static IObservable<decimal?> Min(this IObservable<decimal?> source)
        {
            if (source == null)
            {
                throw new ArgumentNullException(nameof(source));
            }

            return s_impl.Min(source);
        }

        /// <summary>
        /// Returns the minimum value in an observable sequence of nullable <see cref="int" /> values.
        /// </summary>
        /// <param name="source">A sequence of nullable <see cref="int" /> values to determine the minimum value of.</param>
        /// <returns>An observable sequence containing a single element with the minimum value in the source sequence.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="source"/> is null.</exception>
        /// <remarks>The return type of this operator differs from the corresponding operator on IEnumerable in order to retain asynchronous behavior.</remarks>
        public static IObservable<int?> Min(this IObservable<int?> source)
        {
            if (source == null)
            {
                throw new ArgumentNullException(nameof(source));
            }

            return s_impl.Min(source);
        }

        /// <summary>
        /// Returns the minimum value in an observable sequence of nullable <see cref="long" /> values.
        /// </summary>
        /// <param name="source">A sequence of nullable <see cref="long" /> values to determine the minimum value of.</param>
        /// <returns>An observable sequence containing a single element with the minimum value in the source sequence.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="source"/> is null.</exception>
        /// <remarks>The return type of this operator differs from the corresponding operator on IEnumerable in order to retain asynchronous behavior.</remarks>
        public static IObservable<long?> Min(this IObservable<long?> source)
        {
            if (source == null)
            {
                throw new ArgumentNullException(nameof(source));
            }

            return s_impl.Min(source);
        }

        /// <summary>
        /// Invokes a transform function on each element of a sequence and returns the minimum value.
        /// </summary>
        /// <typeparam name="TSource">The type of the elements in the source sequence.</typeparam>
        /// <typeparam name="TResult">The type of the objects derived from the elements in the source sequence to determine the minimum of.</typeparam>
        /// <param name="source">An observable sequence to determine the minimum element of.</param>
        /// <param name="selector">A transform function to apply to each element.</param>
        /// <returns>An observable sequence containing a single element with the value that corresponds to the minimum element in the source sequence.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="source"/> or <paramref name="selector"/> is null.</exception>
        /// <remarks>The return type of this operator differs from the corresponding operator on IEnumerable in order to retain asynchronous behavior.</remarks>
        public static IObservable<TResult> Min<TSource, TResult>(this IObservable<TSource> source, Func<TSource, TResult> selector)
        {
            if (source == null)
            {
                throw new ArgumentNullException(nameof(source));
            }

            if (selector == null)
            {
                throw new ArgumentNullException(nameof(selector));
            }

            return s_impl.Min(source, selector);
        }

        /// <summary>
        /// Invokes a transform function on each element of a sequence and returns the minimum value according to the specified comparer.
        /// </summary>
        /// <typeparam name="TSource">The type of the elements in the source sequence.</typeparam>
        /// <typeparam name="TResult">The type of the objects derived from the elements in the source sequence to determine the minimum of.</typeparam>
        /// <param name="source">An observable sequence to determine the minimum element of.</param>
        /// <param name="selector">A transform function to apply to each element.</param>
        /// <param name="comparer">Comparer used to compare elements.</param>
        /// <returns>An observable sequence containing a single element with the value that corresponds to the minimum element in the source sequence.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="source"/> or <paramref name="selector"/> or <paramref name="comparer"/> is null.</exception>
        /// <remarks>The return type of this operator differs from the corresponding operator on IEnumerable in order to retain asynchronous behavior.</remarks>
        public static IObservable<TResult> Min<TSource, TResult>(this IObservable<TSource> source, Func<TSource, TResult> selector, IComparer<TResult> comparer)
        {
            if (source == null)
            {
                throw new ArgumentNullException(nameof(source));
            }

            if (selector == null)
            {
                throw new ArgumentNullException(nameof(selector));
            }

            if (comparer == null)
            {
                throw new ArgumentNullException(nameof(comparer));
            }

            return s_impl.Min(source, selector, comparer);
        }

        /// <summary>
        /// Invokes a transform function on each element of a sequence and returns the minimum <see cref="double" /> value.
        /// </summary>
        /// <typeparam name="TSource">The type of the elements in the source sequence.</typeparam>
        /// <param name="source">A sequence of values to determine the minimum value of.</param>
        /// <param name="selector">A transform function to apply to each element.</param>
        /// <returns>An observable sequence containing a single element with the value of type <see cref="double" /> that corresponds to the minimum value in the source sequence.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="source"/> or <paramref name="selector"/> is null.</exception>
        /// <remarks>The return type of this operator differs from the corresponding operator on IEnumerable in order to retain asynchronous behavior.</remarks>
        public static IObservable<double> Min<TSource>(this IObservable<TSource> source, Func<TSource, double> selector)
        {
            if (source == null)
            {
                throw new ArgumentNullException(nameof(source));
            }

            if (selector == null)
            {
                throw new ArgumentNullException(nameof(selector));
            }

            return s_impl.Min(source, selector);
        }

        /// <summary>
        /// Invokes a transform function on each element of a sequence and returns the minimum <see cref="float" /> value.
        /// </summary>
        /// <typeparam name="TSource">The type of the elements in the source sequence.</typeparam>
        /// <param name="source">A sequence of values to determine the minimum value of.</param>
        /// <param name="selector">A transform function to apply to each element.</param>
        /// <returns>An observable sequence containing a single element with the value of type <see cref="float" /> that corresponds to the minimum value in the source sequence.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="source"/> or <paramref name="selector"/> is null.</exception>
        /// <remarks>The return type of this operator differs from the corresponding operator on IEnumerable in order to retain asynchronous behavior.</remarks>
        public static IObservable<float> Min<TSource>(this IObservable<TSource> source, Func<TSource, float> selector)
        {
            if (source == null)
            {
                throw new ArgumentNullException(nameof(source));
            }

            if (selector == null)
            {
                throw new ArgumentNullException(nameof(selector));
            }

            return s_impl.Min(source, selector);
        }

        /// <summary>
        /// Invokes a transform function on each element of a sequence and returns the minimum <see cref="decimal" /> value.
        /// </summary>
        /// <typeparam name="TSource">The type of the elements in the source sequence.</typeparam>
        /// <param name="source">A sequence of values to determine the minimum value of.</param>
        /// <param name="selector">A transform function to apply to each element.</param>
        /// <returns>An observable sequence containing a single element with the value of type <see cref="decimal" /> that corresponds to the minimum value in the source sequence.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="source"/> or <paramref name="selector"/> is null.</exception>
        /// <remarks>The return type of this operator differs from the corresponding operator on IEnumerable in order to retain asynchronous behavior.</remarks>
        public static IObservable<decimal> Min<TSource>(this IObservable<TSource> source, Func<TSource, decimal> selector)
        {
            if (source == null)
            {
                throw new ArgumentNullException(nameof(source));
            }

            if (selector == null)
            {
                throw new ArgumentNullException(nameof(selector));
            }

            return s_impl.Min(source, selector);
        }

        /// <summary>
        /// Invokes a transform function on each element of a sequence and returns the minimum <see cref="int" /> value.
        /// </summary>
        /// <typeparam name="TSource">The type of the elements in the source sequence.</typeparam>
        /// <param name="source">A sequence of values to determine the minimum value of.</param>
        /// <param name="selector">A transform function to apply to each element.</param>
        /// <returns>An observable sequence containing a single element with the value of type <see cref="int" /> that corresponds to the minimum value in the source sequence.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="source"/> or <paramref name="selector"/> is null.</exception>
        /// <remarks>The return type of this operator differs from the corresponding operator on IEnumerable in order to retain asynchronous behavior.</remarks>
        public static IObservable<int> Min<TSource>(this IObservable<TSource> source, Func<TSource, int> selector)
        {
            if (source == null)
            {
                throw new ArgumentNullException(nameof(source));
            }

            if (selector == null)
            {
                throw new ArgumentNullException(nameof(selector));
            }

            return s_impl.Min(source, selector);
        }

        /// <summary>
        /// Invokes a transform function on each element of a sequence and returns the minimum <see cref="long" /> value.
        /// </summary>
        /// <typeparam name="TSource">The type of the elements in the source sequence.</typeparam>
        /// <param name="source">A sequence of values to determine the minimum value of.</param>
        /// <param name="selector">A transform function to apply to each element.</param>
        /// <returns>An observable sequence containing a single element with the value of type <see cref="long" /> that corresponds to the minimum value in the source sequence.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="source"/> or <paramref name="selector"/> is null.</exception>
        /// <remarks>The return type of this operator differs from the corresponding operator on IEnumerable in order to retain asynchronous behavior.</remarks>
        public static IObservable<long> Min<TSource>(this IObservable<TSource> source, Func<TSource, long> selector)
        {
            if (source == null)
            {
                throw new ArgumentNullException(nameof(source));
            }

            if (selector == null)
            {
                throw new ArgumentNullException(nameof(selector));
            }

            return s_impl.Min(source, selector);
        }

        /// <summary>
        /// Invokes a transform function on each element of a sequence and returns the minimum nullable <see cref="double" /> value.
        /// </summary>
        /// <typeparam name="TSource">The type of the elements in the source sequence.</typeparam>
        /// <param name="source">A sequence of values to determine the minimum value of.</param>
        /// <param name="selector">A transform function to apply to each element.</param>
        /// <returns>An observable sequence containing a single element with the value of type <see cref="Nullable{Double}" /> that corresponds to the minimum value in the source sequence.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="source"/> or <paramref name="selector"/> is null.</exception>
        /// <remarks>The return type of this operator differs from the corresponding operator on IEnumerable in order to retain asynchronous behavior.</remarks>
        public static IObservable<double?> Min<TSource>(this IObservable<TSource> source, Func<TSource, double?> selector)
        {
            if (source == null)
            {
                throw new ArgumentNullException(nameof(source));
            }

            if (selector == null)
            {
                throw new ArgumentNullException(nameof(selector));
            }

            return s_impl.Min(source, selector);
        }

        /// <summary>
        /// Invokes a transform function on each element of a sequence and returns the minimum nullable <see cref="float" /> value.
        /// </summary>
        /// <typeparam name="TSource">The type of the elements in the source sequence.</typeparam>
        /// <param name="source">A sequence of values to determine the minimum value of.</param>
        /// <param name="selector">A transform function to apply to each element.</param>
        /// <returns>An observable sequence containing a single element with the value of type <see cref="Nullable{Single}" /> that corresponds to the minimum value in the source sequence.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="source"/> or <paramref name="selector"/> is null.</exception>
        /// <remarks>The return type of this operator differs from the corresponding operator on IEnumerable in order to retain asynchronous behavior.</remarks>
        public static IObservable<float?> Min<TSource>(this IObservable<TSource> source, Func<TSource, float?> selector)
        {
            if (source == null)
            {
                throw new ArgumentNullException(nameof(source));
            }

            if (selector == null)
            {
                throw new ArgumentNullException(nameof(selector));
            }

            return s_impl.Min(source, selector);
        }

        /// <summary>
        /// Invokes a transform function on each element of a sequence and returns the minimum nullable <see cref="decimal" /> value.
        /// </summary>
        /// <typeparam name="TSource">The type of the elements in the source sequence.</typeparam>
        /// <param name="source">A sequence of values to determine the minimum value of.</param>
        /// <param name="selector">A transform function to apply to each element.</param>
        /// <returns>An observable sequence containing a single element with the value of type <see cref="Nullable{Decimal}" /> that corresponds to the minimum value in the source sequence.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="source"/> or <paramref name="selector"/> is null.</exception>
        /// <remarks>The return type of this operator differs from the corresponding operator on IEnumerable in order to retain asynchronous behavior.</remarks>
        public static IObservable<decimal?> Min<TSource>(this IObservable<TSource> source, Func<TSource, decimal?> selector)
        {
            if (source == null)
            {
                throw new ArgumentNullException(nameof(source));
            }

            if (selector == null)
            {
                throw new ArgumentNullException(nameof(selector));
            }

            return s_impl.Min(source, selector);
        }

        /// <summary>
        /// Invokes a transform function on each element of a sequence and returns the minimum nullable <see cref="int" /> value.
        /// </summary>
        /// <typeparam name="TSource">The type of the elements in the source sequence.</typeparam>
        /// <param name="source">A sequence of values to determine the minimum value of.</param>
        /// <param name="selector">A transform function to apply to each element.</param>
        /// <returns>An observable sequence containing a single element with the value of type <see cref="Nullable{Int32}" /> that corresponds to the minimum value in the source sequence.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="source"/> or <paramref name="selector"/> is null.</exception>
        /// <remarks>The return type of this operator differs from the corresponding operator on IEnumerable in order to retain asynchronous behavior.</remarks>
        public static IObservable<int?> Min<TSource>(this IObservable<TSource> source, Func<TSource, int?> selector)
        {
            if (source == null)
            {
                throw new ArgumentNullException(nameof(source));
            }

            if (selector == null)
            {
                throw new ArgumentNullException(nameof(selector));
            }

            return s_impl.Min(source, selector);
        }

        /// <summary>
        /// Invokes a transform function on each element of a sequence and returns the minimum nullable <see cref="long" /> value.
        /// </summary>
        /// <typeparam name="TSource">The type of the elements in the source sequence.</typeparam>
        /// <param name="source">A sequence of values to determine the minimum value of.</param>
        /// <param name="selector">A transform function to apply to each element.</param>
        /// <returns>An observable sequence containing a single element with the value of type <see cref="Nullable{Int64}" /> that corresponds to the minimum value in the source sequence.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="source"/> or <paramref name="selector"/> is null.</exception>
        /// <remarks>The return type of this operator differs from the corresponding operator on IEnumerable in order to retain asynchronous behavior.</remarks>
        public static IObservable<long?> Min<TSource>(this IObservable<TSource> source, Func<TSource, long?> selector)
        {
            if (source == null)
            {
                throw new ArgumentNullException(nameof(source));
            }

            if (selector == null)
            {
                throw new ArgumentNullException(nameof(selector));
            }

            return s_impl.Min(source, selector);
        }

        #endregion

        #region + MinBy +

        /// <summary>
        /// Returns the elements in an observable sequence with the minimum key value.
        /// </summary>
        /// <typeparam name="TSource">The type of the elements in the source sequence.</typeparam>
        /// <typeparam name="TKey">The type of the key computed for each element in the source sequence.</typeparam>
        /// <param name="source">An observable sequence to get the minimum elements for.</param>
        /// <param name="keySelector">Key selector function.</param>
        /// <returns>An observable sequence containing a list of zero or more elements that have a minimum key value.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="source"/> or <paramref name="keySelector"/> is null.</exception>
        /// <remarks>The return type of this operator differs from the corresponding operator on IEnumerable in order to retain asynchronous behavior.</remarks>
        public static IObservable<IList<TSource>> MinBy<TSource, TKey>(this IObservable<TSource> source, Func<TSource, TKey> keySelector)
        {
            if (source == null)
            {
                throw new ArgumentNullException(nameof(source));
            }

            if (keySelector == null)
            {
                throw new ArgumentNullException(nameof(keySelector));
            }

            return s_impl.MinBy(source, keySelector);
        }

        /// <summary>
        /// Returns the elements in an observable sequence with the minimum key value according to the specified comparer.
        /// </summary>
        /// <typeparam name="TSource">The type of the elements in the source sequence.</typeparam>
        /// <typeparam name="TKey">The type of the key computed for each element in the source sequence.</typeparam>
        /// <param name="source">An observable sequence to get the minimum elements for.</param>
        /// <param name="keySelector">Key selector function.</param>
        /// <param name="comparer">Comparer used to compare key values.</param>
        /// <returns>An observable sequence containing a list of zero or more elements that have a minimum key value.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="source"/> or <paramref name="keySelector"/> or <paramref name="comparer"/> is null.</exception>
        /// <remarks>The return type of this operator differs from the corresponding operator on IEnumerable in order to retain asynchronous behavior.</remarks>
        public static IObservable<IList<TSource>> MinBy<TSource, TKey>(this IObservable<TSource> source, Func<TSource, TKey> keySelector, IComparer<TKey> comparer)
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

            return s_impl.MinBy(source, keySelector, comparer);
        }

        #endregion

        #region + SequenceEqual +

        /// <summary>
        /// Determines whether two sequences are equal by comparing the elements pairwise.
        /// </summary>
        /// <typeparam name="TSource">The type of the elements in the source sequence.</typeparam>
        /// <param name="first">First observable sequence to compare.</param>
        /// <param name="second">Second observable sequence to compare.</param>
        /// <returns>An observable sequence that contains a single element which indicates whether both sequences are of equal length and their corresponding elements are equal according to the default equality comparer for their type.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="first"/> or <paramref name="second"/> is null.</exception>
        /// <remarks>The return type of this operator differs from the corresponding operator on IEnumerable in order to retain asynchronous behavior.</remarks>
        public static IObservable<bool> SequenceEqual<TSource>(this IObservable<TSource> first, IObservable<TSource> second)
        {
            if (first == null)
            {
                throw new ArgumentNullException(nameof(first));
            }

            if (second == null)
            {
                throw new ArgumentNullException(nameof(second));
            }

            return s_impl.SequenceEqual(first, second);
        }

        /// <summary>
        /// Determines whether two sequences are equal by comparing the elements pairwise using a specified equality comparer.
        /// </summary>
        /// <typeparam name="TSource">The type of the elements in the source sequence.</typeparam>
        /// <param name="first">First observable sequence to compare.</param>
        /// <param name="second">Second observable sequence to compare.</param>
        /// <param name="comparer">Comparer used to compare elements of both sequences.</param>
        /// <returns>An observable sequence that contains a single element which indicates whether both sequences are of equal length and their corresponding elements are equal according to the specified equality comparer.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="first"/> or <paramref name="second"/> or <paramref name="comparer"/> is null.</exception>
        /// <remarks>The return type of this operator differs from the corresponding operator on IEnumerable in order to retain asynchronous behavior.</remarks>
        public static IObservable<bool> SequenceEqual<TSource>(this IObservable<TSource> first, IObservable<TSource> second, IEqualityComparer<TSource> comparer)
        {
            if (first == null)
            {
                throw new ArgumentNullException(nameof(first));
            }

            if (second == null)
            {
                throw new ArgumentNullException(nameof(second));
            }

            if (comparer == null)
            {
                throw new ArgumentNullException(nameof(comparer));
            }

            return s_impl.SequenceEqual(first, second, comparer);
        }

        /// <summary>
        /// Determines whether an observable and enumerable sequence are equal by comparing the elements pairwise.
        /// </summary>
        /// <typeparam name="TSource">The type of the elements in the source sequence.</typeparam>
        /// <param name="first">First observable sequence to compare.</param>
        /// <param name="second">Second observable sequence to compare.</param>
        /// <returns>An observable sequence that contains a single element which indicates whether both sequences are of equal length and their corresponding elements are equal according to the default equality comparer for their type.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="first"/> or <paramref name="second"/> is null.</exception>
        /// <remarks>The return type of this operator differs from the corresponding operator on IEnumerable in order to retain asynchronous behavior.</remarks>
        public static IObservable<bool> SequenceEqual<TSource>(this IObservable<TSource> first, IEnumerable<TSource> second)
        {
            if (first == null)
            {
                throw new ArgumentNullException(nameof(first));
            }

            if (second == null)
            {
                throw new ArgumentNullException(nameof(second));
            }

            return s_impl.SequenceEqual(first, second);
        }

        /// <summary>
        /// Determines whether an observable and enumerable sequence are equal by comparing the elements pairwise using a specified equality comparer.
        /// </summary>
        /// <typeparam name="TSource">The type of the elements in the source sequence.</typeparam>
        /// <param name="first">First observable sequence to compare.</param>
        /// <param name="second">Second observable sequence to compare.</param>
        /// <param name="comparer">Comparer used to compare elements of both sequences.</param>
        /// <returns>An observable sequence that contains a single element which indicates whether both sequences are of equal length and their corresponding elements are equal according to the specified equality comparer.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="first"/> or <paramref name="second"/> or <paramref name="comparer"/> is null.</exception>
        /// <remarks>The return type of this operator differs from the corresponding operator on IEnumerable in order to retain asynchronous behavior.</remarks>
        public static IObservable<bool> SequenceEqual<TSource>(this IObservable<TSource> first, IEnumerable<TSource> second, IEqualityComparer<TSource> comparer)
        {
            if (first == null)
            {
                throw new ArgumentNullException(nameof(first));
            }

            if (second == null)
            {
                throw new ArgumentNullException(nameof(second));
            }

            if (comparer == null)
            {
                throw new ArgumentNullException(nameof(comparer));
            }

            return s_impl.SequenceEqual(first, second, comparer);
        }

        #endregion

        #region + SingleAsync +

        /// <summary>
        /// Returns the only element of an observable sequence, and reports an exception if there is not exactly one element in the observable sequence.
        /// </summary>
        /// <typeparam name="TSource">The type of the elements in the source sequence.</typeparam>
        /// <param name="source">Source observable sequence.</param>
        /// <returns>Sequence containing the single element in the observable sequence.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="source"/> is null.</exception>
        /// <exception cref="InvalidOperationException">(Asynchronous) The source sequence contains more than one element. -or- The source sequence is empty.</exception>
        public static IObservable<TSource> SingleAsync<TSource>(this IObservable<TSource> source)
        {
            if (source == null)
            {
                throw new ArgumentNullException(nameof(source));
            }

            return s_impl.SingleAsync(source);
        }

        /// <summary>
        /// Returns the only element of an observable sequence that satisfies the condition in the predicate, and reports an exception if there is not exactly one element in the observable sequence.
        /// </summary>
        /// <typeparam name="TSource">The type of the elements in the source sequence.</typeparam>
        /// <param name="source">Source observable sequence.</param>
        /// <param name="predicate">A predicate function to evaluate for elements in the source sequence.</param>
        /// <returns>Sequence containing the single element in the observable sequence that satisfies the condition in the predicate.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="source"/> or <paramref name="predicate"/> is null.</exception>
        /// <exception cref="InvalidOperationException">(Asynchronous) No element satisfies the condition in the predicate. -or- More than one element satisfies the condition in the predicate. -or- The source sequence is empty.</exception>
        public static IObservable<TSource> SingleAsync<TSource>(this IObservable<TSource> source, Func<TSource, bool> predicate)
        {
            if (source == null)
            {
                throw new ArgumentNullException(nameof(source));
            }

            if (predicate == null)
            {
                throw new ArgumentNullException(nameof(predicate));
            }

            return s_impl.SingleAsync(source, predicate);
        }

        #endregion

        #region + SingleOrDefaultAsync +

        /// <summary>
        /// Returns the only element of an observable sequence, or a default value if the observable sequence is empty; this method reports an exception if there is more than one element in the observable sequence.
        /// </summary>
        /// <typeparam name="TSource">The type of the elements in the source sequence.</typeparam>
        /// <param name="source">Source observable sequence.</param>
        /// <returns>Sequence containing the single element in the observable sequence, or a default value if no such element exists.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="source"/> is null.</exception>
        /// <exception cref="InvalidOperationException">(Asynchronous) The source sequence contains more than one element.</exception>
        public static IObservable<TSource?> SingleOrDefaultAsync<TSource>(this IObservable<TSource> source)
        {
            if (source == null)
            {
                throw new ArgumentNullException(nameof(source));
            }

            return s_impl.SingleOrDefaultAsync(source);
        }

        /// <summary>
        /// Returns the only element of an observable sequence that matches the predicate, or a default value if no such element exists; this method reports an exception if there is more than one element in the observable sequence.
        /// </summary>
        /// <typeparam name="TSource">The type of the elements in the source sequence.</typeparam>
        /// <param name="source">Source observable sequence.</param>
        /// <param name="predicate">A predicate function to evaluate for elements in the source sequence.</param>
        /// <returns>Sequence containing the single element in the observable sequence that satisfies the condition in the predicate, or a default value if no such element exists.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="source"/> or <paramref name="predicate"/> is null.</exception>
        /// <exception cref="InvalidOperationException">(Asynchronous) The sequence contains more than one element that satisfies the condition in the predicate.</exception>
        public static IObservable<TSource?> SingleOrDefaultAsync<TSource>(this IObservable<TSource> source, Func<TSource, bool> predicate)
        {
            if (source == null)
            {
                throw new ArgumentNullException(nameof(source));
            }

            if (predicate == null)
            {
                throw new ArgumentNullException(nameof(predicate));
            }

            return s_impl.SingleOrDefaultAsync(source, predicate);
        }

        #endregion

        #region + Sum +

        /// <summary>
        /// Computes the sum of a sequence of <see cref="double" /> values.
        /// </summary>
        /// <param name="source">A sequence of <see cref="double" /> values to calculate the sum of.</param>
        /// <returns>An observable sequence containing a single element with the sum of the values in the source sequence.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="source"/> is null.</exception>
        /// <remarks>The return type of this operator differs from the corresponding operator on IEnumerable in order to retain asynchronous behavior.</remarks>
        public static IObservable<double> Sum(this IObservable<double> source)
        {
            if (source == null)
            {
                throw new ArgumentNullException(nameof(source));
            }

            return s_impl.Sum(source);
        }

        /// <summary>
        /// Computes the sum of a sequence of <see cref="float" /> values.
        /// </summary>
        /// <param name="source">A sequence of <see cref="float" /> values to calculate the sum of.</param>
        /// <returns>An observable sequence containing a single element with the sum of the values in the source sequence.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="source"/> is null.</exception>
        /// <remarks>The return type of this operator differs from the corresponding operator on IEnumerable in order to retain asynchronous behavior.</remarks>
        public static IObservable<float> Sum(this IObservable<float> source)
        {
            if (source == null)
            {
                throw new ArgumentNullException(nameof(source));
            }

            return s_impl.Sum(source);
        }

        /// <summary>
        /// Computes the sum of a sequence of <see cref="decimal" /> values.
        /// </summary>
        /// <param name="source">A sequence of <see cref="decimal" /> values to calculate the sum of.</param>
        /// <returns>An observable sequence containing a single element with the sum of the values in the source sequence.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="source"/> is null.</exception>
        /// <exception cref="OverflowException">(Asynchronous) The sum of the elements in the source sequence is larger than <see cref="decimal.MaxValue"/>.</exception>
        /// <remarks>The return type of this operator differs from the corresponding operator on IEnumerable in order to retain asynchronous behavior.</remarks>
        public static IObservable<decimal> Sum(this IObservable<decimal> source)
        {
            if (source == null)
            {
                throw new ArgumentNullException(nameof(source));
            }

            return s_impl.Sum(source);
        }

        /// <summary>
        /// Computes the sum of a sequence of <see cref="int" /> values.
        /// </summary>
        /// <param name="source">A sequence of <see cref="int" /> values to calculate the sum of.</param>
        /// <returns>An observable sequence containing a single element with the sum of the values in the source sequence.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="source"/> is null.</exception>
        /// <exception cref="OverflowException">(Asynchronous) The sum of the elements in the source sequence is larger than <see cref="int.MaxValue"/>.</exception>
        /// <remarks>The return type of this operator differs from the corresponding operator on IEnumerable in order to retain asynchronous behavior.</remarks>
        public static IObservable<int> Sum(this IObservable<int> source)
        {
            if (source == null)
            {
                throw new ArgumentNullException(nameof(source));
            }

            return s_impl.Sum(source);
        }

        /// <summary>
        /// Computes the sum of a sequence of <see cref="long" /> values.
        /// </summary>
        /// <param name="source">A sequence of <see cref="long" /> values to calculate the sum of.</param>
        /// <returns>An observable sequence containing a single element with the sum of the values in the source sequence.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="source"/> is null.</exception>
        /// <exception cref="OverflowException">(Asynchronous) The sum of the elements in the source sequence is larger than <see cref="long.MaxValue"/>.</exception>
        /// <remarks>The return type of this operator differs from the corresponding operator on IEnumerable in order to retain asynchronous behavior.</remarks>
        public static IObservable<long> Sum(this IObservable<long> source)
        {
            if (source == null)
            {
                throw new ArgumentNullException(nameof(source));
            }

            return s_impl.Sum(source);
        }

        /// <summary>
        /// Computes the sum of a sequence of nullable <see cref="double" /> values.
        /// </summary>
        /// <param name="source">A sequence of nullable <see cref="double" /> values to calculate the sum of.</param>
        /// <returns>An observable sequence containing a single element with the sum of the values in the source sequence.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="source"/> is null.</exception>
        /// <remarks>The return type of this operator differs from the corresponding operator on IEnumerable in order to retain asynchronous behavior.</remarks>
        public static IObservable<double?> Sum(this IObservable<double?> source)
        {
            if (source == null)
            {
                throw new ArgumentNullException(nameof(source));
            }

            return s_impl.Sum(source);
        }

        /// <summary>
        /// Computes the sum of a sequence of nullable <see cref="float" /> values.
        /// </summary>
        /// <param name="source">A sequence of nullable <see cref="float" /> values to calculate the sum of.</param>
        /// <returns>An observable sequence containing a single element with the sum of the values in the source sequence.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="source"/> is null.</exception>
        /// <remarks>The return type of this operator differs from the corresponding operator on IEnumerable in order to retain asynchronous behavior.</remarks>
        public static IObservable<float?> Sum(this IObservable<float?> source)
        {
            if (source == null)
            {
                throw new ArgumentNullException(nameof(source));
            }

            return s_impl.Sum(source);
        }

        /// <summary>
        /// Computes the sum of a sequence of nullable <see cref="decimal" /> values.
        /// </summary>
        /// <param name="source">A sequence of nullable <see cref="decimal" /> values to calculate the sum of.</param>
        /// <returns>An observable sequence containing a single element with the sum of the values in the source sequence.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="source"/> is null.</exception>
        /// <exception cref="OverflowException">(Asynchronous) The sum of the elements in the source sequence is larger than <see cref="decimal.MaxValue"/>.</exception>
        /// <remarks>The return type of this operator differs from the corresponding operator on IEnumerable in order to retain asynchronous behavior.</remarks>
        public static IObservable<decimal?> Sum(this IObservable<decimal?> source)
        {
            if (source == null)
            {
                throw new ArgumentNullException(nameof(source));
            }

            return s_impl.Sum(source);
        }

        /// <summary>
        /// Computes the sum of a sequence of nullable <see cref="int" /> values.
        /// </summary>
        /// <param name="source">A sequence of nullable <see cref="int" /> values to calculate the sum of.</param>
        /// <returns>An observable sequence containing a single element with the sum of the values in the source sequence.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="source"/> is null.</exception>
        /// <exception cref="OverflowException">(Asynchronous) The sum of the elements in the source sequence is larger than <see cref="int.MaxValue"/>.</exception>
        /// <remarks>The return type of this operator differs from the corresponding operator on IEnumerable in order to retain asynchronous behavior.</remarks>
        public static IObservable<int?> Sum(this IObservable<int?> source)
        {
            if (source == null)
            {
                throw new ArgumentNullException(nameof(source));
            }

            return s_impl.Sum(source);
        }

        /// <summary>
        /// Computes the sum of a sequence of nullable <see cref="long" /> values.
        /// </summary>
        /// <param name="source">A sequence of nullable <see cref="long" /> values to calculate the sum of.</param>
        /// <returns>An observable sequence containing a single element with the sum of the values in the source sequence.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="source"/> is null.</exception>
        /// <exception cref="OverflowException">(Asynchronous) The sum of the elements in the source sequence is larger than <see cref="long.MaxValue"/>.</exception>
        /// <remarks>The return type of this operator differs from the corresponding operator on IEnumerable in order to retain asynchronous behavior.</remarks>
        public static IObservable<long?> Sum(this IObservable<long?> source)
        {
            if (source == null)
            {
                throw new ArgumentNullException(nameof(source));
            }

            return s_impl.Sum(source);
        }

        /// <summary>
        /// Computes the sum of a sequence of <see cref="double" /> values that are obtained by invoking a transform function on each element of the input sequence.
        /// </summary>
        /// <typeparam name="TSource">The type of the elements in the source sequence.</typeparam>
        /// <param name="source">A sequence of values that are used to calculate a sum.</param>
        /// <param name="selector">A transform function to apply to each element.</param>
        /// <returns>An observable sequence containing a single element with the sum of the values in the source sequence.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="source"/> or <paramref name="selector"/> is null.</exception>
        /// <remarks>The return type of this operator differs from the corresponding operator on IEnumerable in order to retain asynchronous behavior.</remarks>
        public static IObservable<double> Sum<TSource>(this IObservable<TSource> source, Func<TSource, double> selector)
        {
            if (source == null)
            {
                throw new ArgumentNullException(nameof(source));
            }

            if (selector == null)
            {
                throw new ArgumentNullException(nameof(selector));
            }

            return s_impl.Sum(source, selector);
        }

        /// <summary>
        /// Computes the sum of a sequence of <see cref="float" /> values that are obtained by invoking a transform function on each element of the input sequence.
        /// </summary>
        /// <typeparam name="TSource">The type of the elements in the source sequence.</typeparam>
        /// <param name="source">A sequence of values that are used to calculate a sum.</param>
        /// <param name="selector">A transform function to apply to each element.</param>
        /// <returns>An observable sequence containing a single element with the sum of the values in the source sequence.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="source"/> or <paramref name="selector"/> is null.</exception>
        /// <remarks>The return type of this operator differs from the corresponding operator on IEnumerable in order to retain asynchronous behavior.</remarks>
        public static IObservable<float> Sum<TSource>(this IObservable<TSource> source, Func<TSource, float> selector)
        {
            if (source == null)
            {
                throw new ArgumentNullException(nameof(source));
            }

            if (selector == null)
            {
                throw new ArgumentNullException(nameof(selector));
            }

            return s_impl.Sum(source, selector);
        }

        /// <summary>
        /// Computes the sum of a sequence of <see cref="decimal" /> values that are obtained by invoking a transform function on each element of the input sequence.
        /// </summary>
        /// <typeparam name="TSource">The type of the elements in the source sequence.</typeparam>
        /// <param name="source">A sequence of values that are used to calculate a sum.</param>
        /// <param name="selector">A transform function to apply to each element.</param>
        /// <returns>An observable sequence containing a single element with the sum of the values in the source sequence.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="source"/> or <paramref name="selector"/> is null.</exception>
        /// <exception cref="OverflowException">(Asynchronous) The sum of the projected values for the elements in the source sequence is larger than <see cref="decimal.MaxValue"/>.</exception>
        /// <remarks>The return type of this operator differs from the corresponding operator on IEnumerable in order to retain asynchronous behavior.</remarks>
        public static IObservable<decimal> Sum<TSource>(this IObservable<TSource> source, Func<TSource, decimal> selector)
        {
            if (source == null)
            {
                throw new ArgumentNullException(nameof(source));
            }

            if (selector == null)
            {
                throw new ArgumentNullException(nameof(selector));
            }

            return s_impl.Sum(source, selector);
        }

        /// <summary>
        /// Computes the sum of a sequence of <see cref="int" /> values that are obtained by invoking a transform function on each element of the input sequence.
        /// </summary>
        /// <typeparam name="TSource">The type of the elements in the source sequence.</typeparam>
        /// <param name="source">A sequence of values that are used to calculate a sum.</param>
        /// <param name="selector">A transform function to apply to each element.</param>
        /// <returns>An observable sequence containing a single element with the sum of the values in the source sequence.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="source"/> or <paramref name="selector"/> is null.</exception>
        /// <exception cref="OverflowException">(Asynchronous) The sum of the projected values for the elements in the source sequence is larger than <see cref="int.MaxValue"/>.</exception>
        /// <remarks>The return type of this operator differs from the corresponding operator on IEnumerable in order to retain asynchronous behavior.</remarks>
        public static IObservable<int> Sum<TSource>(this IObservable<TSource> source, Func<TSource, int> selector)
        {
            if (source == null)
            {
                throw new ArgumentNullException(nameof(source));
            }

            if (selector == null)
            {
                throw new ArgumentNullException(nameof(selector));
            }

            return s_impl.Sum(source, selector);
        }

        /// <summary>
        /// Computes the sum of a sequence of <see cref="long" /> values that are obtained by invoking a transform function on each element of the input sequence.
        /// </summary>
        /// <typeparam name="TSource">The type of the elements in the source sequence.</typeparam>
        /// <param name="source">A sequence of values that are used to calculate a sum.</param>
        /// <param name="selector">A transform function to apply to each element.</param>
        /// <returns>An observable sequence containing a single element with the sum of the values in the source sequence.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="source"/> or <paramref name="selector"/> is null.</exception>
        /// <exception cref="OverflowException">(Asynchronous) The sum of the projected values for the elements in the source sequence is larger than <see cref="long.MaxValue"/>.</exception>
        /// <remarks>The return type of this operator differs from the corresponding operator on IEnumerable in order to retain asynchronous behavior.</remarks>
        public static IObservable<long> Sum<TSource>(this IObservable<TSource> source, Func<TSource, long> selector)
        {
            if (source == null)
            {
                throw new ArgumentNullException(nameof(source));
            }

            if (selector == null)
            {
                throw new ArgumentNullException(nameof(selector));
            }

            return s_impl.Sum(source, selector);
        }

        /// <summary>
        /// Computes the sum of a sequence of nullable <see cref="double" /> values that are obtained by invoking a transform function on each element of the input sequence.
        /// </summary>
        /// <typeparam name="TSource">The type of the elements in the source sequence.</typeparam>
        /// <param name="source">A sequence of values that are used to calculate a sum.</param>
        /// <param name="selector">A transform function to apply to each element.</param>
        /// <returns>An observable sequence containing a single element with the sum of the values in the source sequence.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="source"/> or <paramref name="selector"/> is null.</exception>
        /// <remarks>The return type of this operator differs from the corresponding operator on IEnumerable in order to retain asynchronous behavior.</remarks>
        public static IObservable<double?> Sum<TSource>(this IObservable<TSource> source, Func<TSource, double?> selector)
        {
            if (source == null)
            {
                throw new ArgumentNullException(nameof(source));
            }

            if (selector == null)
            {
                throw new ArgumentNullException(nameof(selector));
            }

            return s_impl.Sum(source, selector);
        }

        /// <summary>
        /// Computes the sum of a sequence of nullable <see cref="float" /> values that are obtained by invoking a transform function on each element of the input sequence.
        /// </summary>
        /// <typeparam name="TSource">The type of the elements in the source sequence.</typeparam>
        /// <param name="source">A sequence of values that are used to calculate a sum.</param>
        /// <param name="selector">A transform function to apply to each element.</param>
        /// <returns>An observable sequence containing a single element with the sum of the values in the source sequence.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="source"/> or <paramref name="selector"/> is null.</exception>
        /// <remarks>The return type of this operator differs from the corresponding operator on IEnumerable in order to retain asynchronous behavior.</remarks>
        public static IObservable<float?> Sum<TSource>(this IObservable<TSource> source, Func<TSource, float?> selector)
        {
            if (source == null)
            {
                throw new ArgumentNullException(nameof(source));
            }

            if (selector == null)
            {
                throw new ArgumentNullException(nameof(selector));
            }

            return s_impl.Sum(source, selector);
        }

        /// <summary>
        /// Computes the sum of a sequence of nullable <see cref="decimal" /> values that are obtained by invoking a transform function on each element of the input sequence.
        /// </summary>
        /// <typeparam name="TSource">The type of the elements in the source sequence.</typeparam>
        /// <param name="source">A sequence of values that are used to calculate a sum.</param>
        /// <param name="selector">A transform function to apply to each element.</param>
        /// <returns>An observable sequence containing a single element with the sum of the values in the source sequence.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="source"/> or <paramref name="selector"/> is null.</exception>
        /// <exception cref="OverflowException">(Asynchronous) The sum of the projected values for the elements in the source sequence is larger than <see cref="decimal.MaxValue"/>.</exception>
        /// <remarks>The return type of this operator differs from the corresponding operator on IEnumerable in order to retain asynchronous behavior.</remarks>
        public static IObservable<decimal?> Sum<TSource>(this IObservable<TSource> source, Func<TSource, decimal?> selector)
        {
            if (source == null)
            {
                throw new ArgumentNullException(nameof(source));
            }

            if (selector == null)
            {
                throw new ArgumentNullException(nameof(selector));
            }

            return s_impl.Sum(source, selector);
        }

        /// <summary>
        /// Computes the sum of a sequence of nullable <see cref="int" /> values that are obtained by invoking a transform function on each element of the input sequence.
        /// </summary>
        /// <typeparam name="TSource">The type of the elements in the source sequence.</typeparam>
        /// <param name="source">A sequence of values that are used to calculate a sum.</param>
        /// <param name="selector">A transform function to apply to each element.</param>
        /// <returns>An observable sequence containing a single element with the sum of the values in the source sequence.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="source"/> or <paramref name="selector"/> is null.</exception>
        /// <exception cref="OverflowException">(Asynchronous) The sum of the projected values for the elements in the source sequence is larger than <see cref="int.MaxValue"/>.</exception>
        /// <remarks>The return type of this operator differs from the corresponding operator on IEnumerable in order to retain asynchronous behavior.</remarks>
        public static IObservable<int?> Sum<TSource>(this IObservable<TSource> source, Func<TSource, int?> selector)
        {
            if (source == null)
            {
                throw new ArgumentNullException(nameof(source));
            }

            if (selector == null)
            {
                throw new ArgumentNullException(nameof(selector));
            }

            return s_impl.Sum(source, selector);
        }

        /// <summary>
        /// Computes the sum of a sequence of nullable <see cref="long" /> values that are obtained by invoking a transform function on each element of the input sequence.
        /// </summary>
        /// <typeparam name="TSource">The type of the elements in the source sequence.</typeparam>
        /// <param name="source">A sequence of values that are used to calculate a sum.</param>
        /// <param name="selector">A transform function to apply to each element.</param>
        /// <returns>An observable sequence containing a single element with the sum of the values in the source sequence.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="source"/> or <paramref name="selector"/> is null.</exception>
        /// <exception cref="OverflowException">(Asynchronous) The sum of the projected values for the elements in the source sequence is larger than <see cref="long.MaxValue"/>.</exception>
        /// <remarks>The return type of this operator differs from the corresponding operator on IEnumerable in order to retain asynchronous behavior.</remarks>
        public static IObservable<long?> Sum<TSource>(this IObservable<TSource> source, Func<TSource, long?> selector)
        {
            if (source == null)
            {
                throw new ArgumentNullException(nameof(source));
            }

            if (selector == null)
            {
                throw new ArgumentNullException(nameof(selector));
            }

            return s_impl.Sum(source, selector);
        }

        #endregion

        #region + ToArray +

        /// <summary>
        /// Creates an array from an observable sequence.
        /// </summary>
        /// <typeparam name="TSource">The type of the elements in the source sequence.</typeparam>
        /// <param name="source">The source observable sequence to get an array of elements for.</param>
        /// <returns>An observable sequence containing a single element with an array containing all the elements of the source sequence.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="source"/> is null.</exception>
        /// <remarks>The return type of this operator differs from the corresponding operator on IEnumerable in order to retain asynchronous behavior.</remarks>
        public static IObservable<TSource[]> ToArray<TSource>(this IObservable<TSource> source)
        {
            if (source == null)
            {
                throw new ArgumentNullException(nameof(source));
            }

            return s_impl.ToArray(source);
        }

        #endregion

        #region + ToDictionary +

        /// <summary>
        /// Creates a dictionary from an observable sequence according to a specified key selector function.
        /// </summary>
        /// <typeparam name="TSource">The type of the elements in the source sequence.</typeparam>
        /// <typeparam name="TKey">The type of the dictionary key computed for each element in the source sequence.</typeparam>
        /// <param name="source">An observable sequence to create a dictionary for.</param>
        /// <param name="keySelector">A function to extract a key from each element.</param>
        /// <returns>An observable sequence containing a single element with a dictionary mapping unique key values onto the corresponding source sequence's element.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="source"/> or <paramref name="keySelector"/> is null.</exception>
        /// <remarks>The return type of this operator differs from the corresponding operator on IEnumerable in order to retain asynchronous behavior.</remarks>
        public static IObservable<IDictionary<TKey, TSource>> ToDictionary<TSource, TKey>(this IObservable<TSource> source, Func<TSource, TKey> keySelector)
            where TKey : notnull
        {
            if (source == null)
            {
                throw new ArgumentNullException(nameof(source));
            }

            if (keySelector == null)
            {
                throw new ArgumentNullException(nameof(keySelector));
            }

            return s_impl.ToDictionary(source, keySelector);
        }

        /// <summary>
        /// Creates a dictionary from an observable sequence according to a specified key selector function, and a comparer.
        /// </summary>
        /// <typeparam name="TSource">The type of the elements in the source sequence.</typeparam>
        /// <typeparam name="TKey">The type of the dictionary key computed for each element in the source sequence.</typeparam>
        /// <param name="source">An observable sequence to create a dictionary for.</param>
        /// <param name="keySelector">A function to extract a key from each element.</param>
        /// <param name="comparer">An equality comparer to compare keys.</param>
        /// <returns>An observable sequence containing a single element with a dictionary mapping unique key values onto the corresponding source sequence's element.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="source"/> or <paramref name="keySelector"/> or <paramref name="comparer"/> is null.</exception>
        /// <remarks>The return type of this operator differs from the corresponding operator on IEnumerable in order to retain asynchronous behavior.</remarks>
        public static IObservable<IDictionary<TKey, TSource>> ToDictionary<TSource, TKey>(this IObservable<TSource> source, Func<TSource, TKey> keySelector, IEqualityComparer<TKey> comparer)
            where TKey : notnull
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

            return s_impl.ToDictionary(source, keySelector, comparer);
        }

        /// <summary>
        /// Creates a dictionary from an observable sequence according to a specified key selector function, and an element selector function.
        /// </summary>
        /// <typeparam name="TSource">The type of the elements in the source sequence.</typeparam>
        /// <typeparam name="TKey">The type of the dictionary key computed for each element in the source sequence.</typeparam>
        /// <typeparam name="TElement">The type of the dictionary value computed for each element in the source sequence.</typeparam>
        /// <param name="source">An observable sequence to create a dictionary for.</param>
        /// <param name="keySelector">A function to extract a key from each element.</param>
        /// <param name="elementSelector">A transform function to produce a result element value from each element.</param>
        /// <returns>An observable sequence containing a single element with a dictionary mapping unique key values onto the corresponding source sequence's element.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="source"/> or <paramref name="keySelector"/> or <paramref name="elementSelector"/> is null.</exception>
        /// <remarks>The return type of this operator differs from the corresponding operator on IEnumerable in order to retain asynchronous behavior.</remarks>
        public static IObservable<IDictionary<TKey, TElement>> ToDictionary<TSource, TKey, TElement>(this IObservable<TSource> source, Func<TSource, TKey> keySelector, Func<TSource, TElement> elementSelector)
            where TKey : notnull
        {
            if (source == null)
            {
                throw new ArgumentNullException(nameof(source));
            }

            if (keySelector == null)
            {
                throw new ArgumentNullException(nameof(keySelector));
            }

            if (elementSelector == null)
            {
                throw new ArgumentNullException(nameof(elementSelector));
            }

            return s_impl.ToDictionary(source, keySelector, elementSelector);
        }

        /// <summary>
        /// Creates a dictionary from an observable sequence according to a specified key selector function, a comparer, and an element selector function.
        /// </summary>
        /// <typeparam name="TSource">The type of the elements in the source sequence.</typeparam>
        /// <typeparam name="TKey">The type of the dictionary key computed for each element in the source sequence.</typeparam>
        /// <typeparam name="TElement">The type of the dictionary value computed for each element in the source sequence.</typeparam>
        /// <param name="source">An observable sequence to create a dictionary for.</param>
        /// <param name="keySelector">A function to extract a key from each element.</param>
        /// <param name="elementSelector">A transform function to produce a result element value from each element.</param>
        /// <param name="comparer">An equality comparer to compare keys.</param>
        /// <returns>An observable sequence containing a single element with a dictionary mapping unique key values onto the corresponding source sequence's element.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="source"/> or <paramref name="keySelector"/> or <paramref name="elementSelector"/> or <paramref name="comparer"/> is null.</exception>
        /// <remarks>The return type of this operator differs from the corresponding operator on IEnumerable in order to retain asynchronous behavior.</remarks>
        public static IObservable<IDictionary<TKey, TElement>> ToDictionary<TSource, TKey, TElement>(this IObservable<TSource> source, Func<TSource, TKey> keySelector, Func<TSource, TElement> elementSelector, IEqualityComparer<TKey> comparer)
            where TKey : notnull
        {
            if (source == null)
            {
                throw new ArgumentNullException(nameof(source));
            }

            if (keySelector == null)
            {
                throw new ArgumentNullException(nameof(keySelector));
            }

            if (elementSelector == null)
            {
                throw new ArgumentNullException(nameof(elementSelector));
            }

            if (comparer == null)
            {
                throw new ArgumentNullException(nameof(comparer));
            }

            return s_impl.ToDictionary(source, keySelector, elementSelector, comparer);
        }

        #endregion

        #region + ToList +

        /// <summary>
        /// Creates a list from an observable sequence.
        /// </summary>
        /// <typeparam name="TSource">The type of the elements in the source sequence.</typeparam>
        /// <param name="source">The source observable sequence to get a list of elements for.</param>
        /// <returns>An observable sequence containing a single element with a list containing all the elements of the source sequence.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="source"/> is null.</exception>
        /// <remarks>The return type of this operator differs from the corresponding operator on IEnumerable in order to retain asynchronous behavior.</remarks>
        public static IObservable<IList<TSource>> ToList<TSource>(this IObservable<TSource> source)
        {
            if (source == null)
            {
                throw new ArgumentNullException(nameof(source));
            }

            return s_impl.ToList(source);
        }

        #endregion

        #region + ToLookup +

        /// <summary>
        /// Creates a lookup from an observable sequence according to a specified key selector function.
        /// </summary>
        /// <typeparam name="TSource">The type of the elements in the source sequence.</typeparam>
        /// <typeparam name="TKey">The type of the lookup key computed for each element in the source sequence.</typeparam>
        /// <param name="source">An observable sequence to create a lookup for.</param>
        /// <param name="keySelector">A function to extract a key from each element.</param>
        /// <returns>An observable sequence containing a single element with a lookup mapping unique key values onto the corresponding source sequence's elements.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="source"/> or <paramref name="keySelector"/> is null.</exception>
        /// <remarks>The return type of this operator differs from the corresponding operator on IEnumerable in order to retain asynchronous behavior.</remarks>
        public static IObservable<ILookup<TKey, TSource>> ToLookup<TSource, TKey>(this IObservable<TSource> source, Func<TSource, TKey> keySelector)
        {
            if (source == null)
            {
                throw new ArgumentNullException(nameof(source));
            }

            if (keySelector == null)
            {
                throw new ArgumentNullException(nameof(keySelector));
            }

            return s_impl.ToLookup(source, keySelector);
        }

        /// <summary>
        /// Creates a lookup from an observable sequence according to a specified key selector function, and a comparer.
        /// </summary>
        /// <typeparam name="TSource">The type of the elements in the source sequence.</typeparam>
        /// <typeparam name="TKey">The type of the lookup key computed for each element in the source sequence.</typeparam>
        /// <param name="source">An observable sequence to create a lookup for.</param>
        /// <param name="keySelector">A function to extract a key from each element.</param>
        /// <param name="comparer">An equality comparer to compare keys.</param>
        /// <returns>An observable sequence containing a single element with a lookup mapping unique key values onto the corresponding source sequence's elements.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="source"/> or <paramref name="keySelector"/> or <paramref name="comparer"/> is null.</exception>
        /// <remarks>The return type of this operator differs from the corresponding operator on IEnumerable in order to retain asynchronous behavior.</remarks>
        public static IObservable<ILookup<TKey, TSource>> ToLookup<TSource, TKey>(this IObservable<TSource> source, Func<TSource, TKey> keySelector, IEqualityComparer<TKey> comparer)
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

            return s_impl.ToLookup(source, keySelector, comparer);
        }

        /// <summary>
        /// Creates a lookup from an observable sequence according to a specified key selector function, and an element selector function.
        /// </summary>
        /// <typeparam name="TSource">The type of the elements in the source sequence.</typeparam>
        /// <typeparam name="TKey">The type of the lookup key computed for each element in the source sequence.</typeparam>
        /// <typeparam name="TElement">The type of the lookup value computed for each element in the source sequence.</typeparam>
        /// <param name="source">An observable sequence to create a lookup for.</param>
        /// <param name="keySelector">A function to extract a key from each element.</param>
        /// <param name="elementSelector">A transform function to produce a result element value from each element.</param>
        /// <returns>An observable sequence containing a single element with a lookup mapping unique key values onto the corresponding source sequence's elements.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="source"/> or <paramref name="keySelector"/> or <paramref name="elementSelector"/> is null.</exception>
        /// <remarks>The return type of this operator differs from the corresponding operator on IEnumerable in order to retain asynchronous behavior.</remarks>
        public static IObservable<ILookup<TKey, TElement>> ToLookup<TSource, TKey, TElement>(this IObservable<TSource> source, Func<TSource, TKey> keySelector, Func<TSource, TElement> elementSelector)
        {
            if (source == null)
            {
                throw new ArgumentNullException(nameof(source));
            }

            if (keySelector == null)
            {
                throw new ArgumentNullException(nameof(keySelector));
            }

            if (elementSelector == null)
            {
                throw new ArgumentNullException(nameof(elementSelector));
            }

            return s_impl.ToLookup(source, keySelector, elementSelector);
        }

        /// <summary>
        /// Creates a lookup from an observable sequence according to a specified key selector function, a comparer, and an element selector function.
        /// </summary>
        /// <typeparam name="TSource">The type of the elements in the source sequence.</typeparam>
        /// <typeparam name="TKey">The type of the lookup key computed for each element in the source sequence.</typeparam>
        /// <typeparam name="TElement">The type of the lookup value computed for each element in the source sequence.</typeparam>
        /// <param name="source">An observable sequence to create a lookup for.</param>
        /// <param name="keySelector">A function to extract a key from each element.</param>
        /// <param name="elementSelector">A transform function to produce a result element value from each element.</param>
        /// <param name="comparer">An equality comparer to compare keys.</param>
        /// <returns>An observable sequence containing a single element with a lookup mapping unique key values onto the corresponding source sequence's elements.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="source"/> or <paramref name="keySelector"/> or <paramref name="elementSelector"/> or <paramref name="comparer"/> is null.</exception>
        /// <remarks>The return type of this operator differs from the corresponding operator on IEnumerable in order to retain asynchronous behavior.</remarks>
        public static IObservable<ILookup<TKey, TElement>> ToLookup<TSource, TKey, TElement>(this IObservable<TSource> source, Func<TSource, TKey> keySelector, Func<TSource, TElement> elementSelector, IEqualityComparer<TKey> comparer)
        {
            if (source == null)
            {
                throw new ArgumentNullException(nameof(source));
            }

            if (keySelector == null)
            {
                throw new ArgumentNullException(nameof(keySelector));
            }

            if (elementSelector == null)
            {
                throw new ArgumentNullException(nameof(elementSelector));
            }

            if (comparer == null)
            {
                throw new ArgumentNullException(nameof(comparer));
            }

            return s_impl.ToLookup(source, keySelector, elementSelector, comparer);
        }

        #endregion
    }
}

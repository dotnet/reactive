// Copyright (c) Microsoft Open Technologies, Inc. All rights reserved. See License.txt in the project root for license information.

using System.Collections.Generic;
using System.Reactive.Disposables;
using System.Reactive.Joins;

namespace System.Reactive.Linq
{
	public static partial class Observable
    {
        #region And

        /// <summary>
        /// Creates a pattern that matches when both observable sequences have an available element.
        /// </summary>
        /// <typeparam name="TLeft">The type of the elements in the left sequence.</typeparam>
        /// <typeparam name="TRight">The type of the elements in the right sequence.</typeparam>
        /// <param name="left">Observable sequence to match with the right sequence.</param>
        /// <param name="right">Observable sequence to match with the left sequence.</param>
        /// <returns>Pattern object that matches when both observable sequences have an available element.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="left"/> or <paramref name="right"/> is null.</exception>
        public static Pattern<TLeft, TRight> And<TLeft, TRight>(this IObservable<TLeft> left, IObservable<TRight> right)
        {
            if (left == null)
                throw new ArgumentNullException("left");
            if (right == null)
                throw new ArgumentNullException("right");

            return s_impl.And<TLeft, TRight>(left, right);
        }

        #endregion

        #region Then

        /// <summary>
        /// Matches when the observable sequence has an available element and projects the element by invoking the selector function.
        /// </summary>
        /// <typeparam name="TSource">The type of the elements in the source sequence.</typeparam>
        /// <typeparam name="TResult">The type of the elements in the result sequence, returned by the selector function.</typeparam>
        /// <param name="source">Observable sequence to apply the selector on.</param>
        /// <param name="selector">Selector that will be invoked for elements in the source sequence.</param>
        /// <returns>Plan that produces the projected results, to be fed (with other plans) to the When operator.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="source"/> or <paramref name="selector"/> is null.</exception>
        public static Plan<TResult> Then<TSource, TResult>(this IObservable<TSource> source, Func<TSource, TResult> selector)
        {
            if (source == null)
                throw new ArgumentNullException("source");
            if (selector == null)
                throw new ArgumentNullException("selector");

            return s_impl.Then<TSource, TResult>(source, selector);
        }

        #endregion

        #region When

        /// <summary>
        /// Joins together the results from several patterns.
        /// </summary>
        /// <typeparam name="TResult">The type of the elements in the result sequence, obtained from the specified patterns.</typeparam>
        /// <param name="plans">A series of plans created by use of the Then operator on patterns.</param>
        /// <returns>An observable sequence with the results from matching several patterns.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="plans"/> is null.</exception>
        public static IObservable<TResult> When<TResult>(params Plan<TResult>[] plans)
        {
            if (plans == null)
                throw new ArgumentNullException("plans");

            return s_impl.When<TResult>(plans);
        }

        /// <summary>
        /// Joins together the results from several patterns.
        /// </summary>
        /// <typeparam name="TResult">The type of the elements in the result sequence, obtained from the specified patterns.</typeparam>
        /// <param name="plans">A series of plans created by use of the Then operator on patterns.</param>
        /// <returns>An observable sequence with the results form matching several patterns.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="plans"/> is null.</exception>
        public static IObservable<TResult> When<TResult>(this IEnumerable<Plan<TResult>> plans)
        {
            if (plans == null)
                throw new ArgumentNullException("plans");

            return s_impl.When<TResult>(plans);
        }

        #endregion
    }
}

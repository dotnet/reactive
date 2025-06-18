﻿// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information. 

using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reactive.Joins;
using System.Reflection;

namespace System.Reactive.Linq
{
    public static partial class Qbservable
    {
        /* NOTE: Keep XML docs consistent with the corresponding Observable methods (modulo the IQbservableProvider parameters of course). */

        /// <summary>
        /// Creates a pattern that matches when both observable sequences have an available element.
        /// </summary>
        /// <typeparam name="TLeft">The type of the elements in the left sequence.</typeparam>
        /// <typeparam name="TRight">The type of the elements in the right sequence.</typeparam>
        /// <param name="left">Observable sequence to match with the right sequence.</param>
        /// <param name="right">Observable sequence to match with the left sequence.</param>
        /// <returns>Pattern object that matches when both observable sequences have an available element.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="left"/> or <paramref name="right"/> is null.</exception>
        public static QueryablePattern<TLeft, TRight> And<TLeft, TRight>(this IQbservable<TLeft> left, IObservable<TRight> right)
        {
            if (left == null)
            {
                throw new ArgumentNullException(nameof(left));
            }

            if (right == null)
            {
                throw new ArgumentNullException(nameof(right));
            }

            return new QueryablePattern<TLeft, TRight>(
                Expression.Call(
                    null,
#pragma warning disable IL2060 // ('System.Reflection.MethodInfo.MakeGenericMethod' can not be statically analyzed.) This gets the MethodInfo for the method running right now, so it can't have been trimmed
                    ((MethodInfo)MethodBase.GetCurrentMethod()!).MakeGenericMethod(typeof(TLeft), typeof(TRight)),
#pragma warning restore IL2060
                    left.Expression,
                    GetSourceExpression(right)
                )
            );
        }

        /// <summary>
        /// Matches when the observable sequence has an available element and projects the element by invoking the selector function.
        /// </summary>
        /// <typeparam name="TSource">The type of the elements in the source sequence.</typeparam>
        /// <typeparam name="TResult">The type of the elements in the result sequence, returned by the selector function.</typeparam>
        /// <param name="source">Observable sequence to apply the selector on.</param>
        /// <param name="selector">Selector that will be invoked for elements in the source sequence.</param>
        /// <returns>Plan that produces the projected results, to be fed (with other plans) to the When operator.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="source"/> or <paramref name="selector"/> is null.</exception>
        public static QueryablePlan<TResult> Then<TSource, TResult>(this IQbservable<TSource> source, Expression<Func<TSource, TResult>> selector)
        {
            if (source == null)
            {
                throw new ArgumentNullException(nameof(source));
            }

            if (selector == null)
            {
                throw new ArgumentNullException(nameof(selector));
            }

            return new QueryablePlan<TResult>(
                Expression.Call(
                    null,
#pragma warning disable IL2060 // ('System.Reflection.MethodInfo.MakeGenericMethod' can not be statically analyzed.) This gets the MethodInfo for the method running right now, so it can't have been trimmed
                    ((MethodInfo)MethodBase.GetCurrentMethod()!).MakeGenericMethod(typeof(TSource), typeof(TResult)),
#pragma warning restore IL2060
                    source.Expression,
                    selector
                )
            );
        }

        /// <summary>
        /// Joins together the results from several patterns.
        /// </summary>
        /// <typeparam name="TResult">The type of the elements in the result sequence, obtained from the specified patterns.</typeparam>
        /// <param name="provider">Query provider used to construct the <see cref="IQbservable{T}"/> data source.</param>
        /// <param name="plans">A series of plans created by use of the Then operator on patterns.</param>
        /// <returns>An observable sequence with the results from matching several patterns.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="provider"/> or <paramref name="plans"/> is null.</exception>
        public static IQbservable<TResult> When<TResult>(this IQbservableProvider provider, params QueryablePlan<TResult>[] plans)
        {
            if (provider == null)
            {
                throw new ArgumentNullException(nameof(provider));
            }

            if (plans == null)
            {
                throw new ArgumentNullException(nameof(plans));
            }

            return provider.CreateQuery<TResult>(
                Expression.Call(
                    null,
#pragma warning disable IL2060 // ('System.Reflection.MethodInfo.MakeGenericMethod' can not be statically analyzed.) This gets the MethodInfo for the method running right now, so it can't have been trimmed
                    ((MethodInfo)MethodBase.GetCurrentMethod()!).MakeGenericMethod(typeof(TResult)),
#pragma warning restore IL2060
                    Expression.Constant(provider, typeof(IQbservableProvider)),
                    Expression.NewArrayInit(
                        typeof(QueryablePlan<TResult>),
                        plans.Select(p => p.Expression)
                    )
                )
            );
        }

        /// <summary>
        /// Joins together the results from several patterns.
        /// </summary>
        /// <typeparam name="TResult">The type of the elements in the result sequence, obtained from the specified patterns.</typeparam>
        /// <param name="provider">Query provider used to construct the <see cref="IQbservable{T}"/> data source.</param>
        /// <param name="plans">A series of plans created by use of the Then operator on patterns.</param>
        /// <returns>An observable sequence with the results form matching several patterns.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="provider"/> or <paramref name="plans"/> is null.</exception>
        public static IQbservable<TResult> When<TResult>(this IQbservableProvider provider, IEnumerable<QueryablePlan<TResult>> plans)
        {
            if (provider == null)
            {
                throw new ArgumentNullException(nameof(provider));
            }

            if (plans == null)
            {
                throw new ArgumentNullException(nameof(plans));
            }

            return provider.CreateQuery<TResult>(
                Expression.Call(
                    null,
#pragma warning disable IL2060 // ('System.Reflection.MethodInfo.MakeGenericMethod' can not be statically analyzed.) This gets the MethodInfo for the method running right now, so it can't have been trimmed
                    ((MethodInfo)MethodBase.GetCurrentMethod()!).MakeGenericMethod(typeof(TResult)),
#pragma warning restore IL2060
                    Expression.Constant(provider, typeof(IQbservableProvider)),
                    Expression.Constant(plans, typeof(IEnumerable<QueryablePlan<TResult>>))
                )
            );
        }
    }
}

// Copyright (c) Microsoft Open Technologies, Inc. All rights reserved. See License.txt in the project root for license information.

#pragma warning disable 1591

using System.Collections.Generic;
using System.Reactive.Joins;
using System.Linq.Expressions;
using System.Reflection;
using System.Linq;

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
                throw new ArgumentNullException("left");
            if (right == null)
                throw new ArgumentNullException("right");

            return new QueryablePattern<TLeft, TRight>(
                Expression.Call(
                    null,
#if CRIPPLED_REFLECTION
                    InfoOf(() => Qbservable.And<TLeft, TRight>(default(IQbservable<TLeft>), default(IObservable<TRight>))),
#else
                    ((MethodInfo)MethodInfo.GetCurrentMethod()).MakeGenericMethod(typeof(TLeft), typeof(TRight)),
#endif
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
                throw new ArgumentNullException("source");
            if (selector == null)
                throw new ArgumentNullException("selector");

            return new QueryablePlan<TResult>(
                Expression.Call(
                    null,
#if CRIPPLED_REFLECTION
                    InfoOf(() => Qbservable.Then<TSource, TResult>(default(IQbservable<TSource>), default(Expression<Func<TSource, TResult>>))),
#else
                    ((MethodInfo)MethodInfo.GetCurrentMethod()).MakeGenericMethod(typeof(TSource), typeof(TResult)),
#endif
                    source.Expression,
                    selector
                )
            );
        }

        /// <summary>
        /// Joins together the results from several patterns.
        /// </summary>
        /// <typeparam name="TResult">The type of the elements in the result sequence, obtained from the specified patterns.</typeparam>
        /// <param name="provider">Query provider used to construct the IQbservable&lt;T&gt; data source.</param>
        /// <param name="plans">A series of plans created by use of the Then operator on patterns.</param>
        /// <returns>An observable sequence with the results from matching several patterns.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="provider"/> or <paramref name="plans"/> is null.</exception>
        public static IQbservable<TResult> When<TResult>(this IQbservableProvider provider, params QueryablePlan<TResult>[] plans)
        {
            if (provider == null)
                throw new ArgumentNullException("provider");
            if (plans == null)
                throw new ArgumentNullException("plans");

            return provider.CreateQuery<TResult>(
                Expression.Call(
                    null,
#if CRIPPLED_REFLECTION
                    InfoOf(() => Qbservable.When<TResult>(default(IQbservableProvider), default(QueryablePlan<TResult>[]))),
#else
                    ((MethodInfo)MethodInfo.GetCurrentMethod()).MakeGenericMethod(typeof(TResult)),
#endif
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
        /// <param name="provider">Query provider used to construct the IQbservable&lt;T&gt; data source.</param>
        /// <param name="plans">A series of plans created by use of the Then operator on patterns.</param>
        /// <returns>An observable sequence with the results form matching several patterns.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="provider"/> or <paramref name="plans"/> is null.</exception>
        public static IQbservable<TResult> When<TResult>(this IQbservableProvider provider, IEnumerable<QueryablePlan<TResult>> plans)
        {
            if (provider == null)
                throw new ArgumentNullException("provider");
            if (plans == null)
                throw new ArgumentNullException("plans");

            return provider.CreateQuery<TResult>(
                Expression.Call(
                    null,
#if CRIPPLED_REFLECTION
                    InfoOf(() => Qbservable.When<TResult>(default(IQbservableProvider), default(IEnumerable<QueryablePlan<TResult>>))),
#else
                    ((MethodInfo)MethodInfo.GetCurrentMethod()).MakeGenericMethod(typeof(TResult)),
#endif
                    Expression.Constant(provider, typeof(IQbservableProvider)),
                    Expression.Constant(plans, typeof(IEnumerable<QueryablePlan<TResult>>))
                )
            );
        }
    }
}

#pragma warning restore 1591
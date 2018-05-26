// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the Apache 2.0 License.
// See the LICENSE file in the project root for more information. 

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
                throw new ArgumentNullException(nameof(left));
            if (right == null)
                throw new ArgumentNullException(nameof(right));

            return new Pattern<TLeft, TRight>(left, right);
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
                throw new ArgumentNullException(nameof(source));
            if (selector == null)
                throw new ArgumentNullException(nameof(selector));

            return new Pattern<TSource>(source).Then(selector);
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
                throw new ArgumentNullException(nameof(plans));

            return When((IEnumerable<Plan<TResult>>)plans);
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
                throw new ArgumentNullException(nameof(plans));

            return new AnonymousObservable<TResult>(observer =>
            {
                var externalSubscriptions = new Dictionary<object, IJoinObserver>();
                var gate = new object();
                var activePlans = new List<ActivePlan>();
                var outObserver = Observer.Create<TResult>(observer.OnNext,
                    exception =>
                    {
                        foreach (var po in externalSubscriptions.Values)
                        {
                            po.Dispose();
                        }
                        observer.OnError(exception);
                    },
                    observer.OnCompleted);
                try
                {
                    foreach (var plan in plans)
                        activePlans.Add(plan.Activate(externalSubscriptions, outObserver,
                                                      activePlan =>
                                                      {
                                                          activePlans.Remove(activePlan);
                                                          if (activePlans.Count == 0)
                                                              outObserver.OnCompleted();
                                                      }));
                }
                catch (Exception e)
                {
                    //
                    // [OK] Use of unsafe Subscribe: we're calling into a known producer implementation.
                    //
                    return Throw<TResult>(e).Subscribe/*Unsafe*/(observer);
                }

                var group = new CompositeDisposable(externalSubscriptions.Values.Count);
                foreach (var joinObserver in externalSubscriptions.Values)
                {
                    joinObserver.Subscribe(gate);
                    group.Add(joinObserver);
                }
                return group;
            });
        }

        #endregion
    }
}

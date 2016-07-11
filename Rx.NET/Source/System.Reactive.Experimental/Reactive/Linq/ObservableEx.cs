// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the Apache 2.0 License.
// See the LICENSE file in the project root for more information. 

using System.Collections.Generic;
using System.Reactive.Concurrency;

namespace System.Reactive.Linq
{
    /// <summary>
    /// Provides a set of static methods for writing in-memory queries over observable sequences.
    /// </summary>
    public static class ObservableEx
    {
        private static IQueryLanguageEx s_impl = QueryServices.GetQueryImpl<IQueryLanguageEx>(new QueryLanguageEx());

        #region Create

        /// <summary>
        /// Subscribes to each observable sequence returned by the iteratorMethod in sequence and returns the observable sequence of values sent to the observer given to the iteratorMethod.
        /// </summary>
        /// <typeparam name="TResult">The type of the elements in the produced sequence.</typeparam>
        /// <param name="iteratorMethod">Iterator method that produces elements in the resulting sequence by calling the given observer.</param>
        /// <returns>An observable sequence obtained by running the iterator and returning the elements that were sent to the observer.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="iteratorMethod"/> is null.</exception>
        [Experimental]
        public static IObservable<TResult> Create<TResult>(Func<IObserver<TResult>, IEnumerable<IObservable<object>>> iteratorMethod)
        {
            if (iteratorMethod == null)
                throw new ArgumentNullException(nameof(iteratorMethod));

            return s_impl.Create<TResult>(iteratorMethod);
        }

        /// <summary>
        /// Subscribes to each observable sequence returned by the iteratorMethod in sequence and produces a Unit value on the resulting sequence for each step of the iteration.
        /// </summary>
        /// <param name="iteratorMethod">Iterator method that drives the resulting observable sequence.</param>
        /// <returns>An observable sequence obtained by running the iterator and returning Unit values for each iteration step.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="iteratorMethod"/> is null.</exception>
        [Experimental]
        public static IObservable<Unit> Create(Func<IEnumerable<IObservable<object>>> iteratorMethod)
        {
            if (iteratorMethod == null)
                throw new ArgumentNullException(nameof(iteratorMethod));

            return s_impl.Create(iteratorMethod);
        }

        #endregion

        #region Expand

        /// <summary>
        /// Expands an observable sequence by recursively invoking selector, using the specified scheduler to enumerate the queue of obtained sequences.
        /// </summary>
        /// <typeparam name="TSource">The type of the elements in the source sequence and each of the recursively expanded sources obtained by running the selector function.</typeparam>
        /// <param name="source">Source sequence with the initial elements.</param>
        /// <param name="selector">Selector function to invoke for each produced element, resulting in another sequence to which the selector will be invoked recursively again.</param>
        /// <param name="scheduler">Scheduler on which to perform the expansion by enumerating the internal queue of obtained sequences.</param>
        /// <returns>An observable sequence containing all the elements produced by the recursive expansion.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="source"/> or <paramref name="selector"/> or <paramref name="scheduler"/> is null.</exception>
        [Experimental]
        public static IObservable<TSource> Expand<TSource>(this IObservable<TSource> source, Func<TSource, IObservable<TSource>> selector, IScheduler scheduler)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));
            if (selector == null)
                throw new ArgumentNullException(nameof(selector));
            if (scheduler == null)
                throw new ArgumentNullException(nameof(scheduler));

            return s_impl.Expand<TSource>(source, selector, scheduler);
        }

        /// <summary>
        /// Expands an observable sequence by recursively invoking selector.
        /// </summary>
        /// <typeparam name="TSource">The type of the elements in the source sequence and each of the recursively expanded sources obtained by running the selector function.</typeparam>
        /// <param name="source">Source sequence with the initial elements.</param>
        /// <param name="selector">Selector function to invoke for each produced element, resulting in another sequence to which the selector will be invoked recursively again.</param>
        /// <returns>An observable sequence containing all the elements produced by the recursive expansion.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="source"/> or <paramref name="selector"/> is null.</exception>
        [Experimental]
        public static IObservable<TSource> Expand<TSource>(this IObservable<TSource> source, Func<TSource, IObservable<TSource>> selector)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));
            if (selector == null)
                throw new ArgumentNullException(nameof(selector));

            return s_impl.Expand<TSource>(source, selector);
        }

        #endregion

        #region ForkJoin

        /// <summary>
        /// Runs two observable sequences in parallel and combines their last elemenets.
        /// </summary>
        /// <typeparam name="TSource1">The type of the elements in the first source sequence.</typeparam>
        /// <typeparam name="TSource2">The type of the elements in the second source sequence.</typeparam>
        /// <typeparam name="TResult">The type of the elements in the result sequence, returned by the selector function.</typeparam>
        /// <param name="first">First observable sequence.</param>
        /// <param name="second">Second observable sequence.</param>
        /// <param name="resultSelector">Result selector function to invoke with the last elements of both sequences.</param>
        /// <returns>An observable sequence with the result of calling the selector function with the last elements of both input sequences.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="first"/> or <paramref name="second"/> or <paramref name="resultSelector"/> is null.</exception>
        [Experimental]
        public static IObservable<TResult> ForkJoin<TSource1, TSource2, TResult>(this IObservable<TSource1> first, IObservable<TSource2> second, Func<TSource1, TSource2, TResult> resultSelector)
        {
            if (first == null)
                throw new ArgumentNullException(nameof(first));
            if (second == null)
                throw new ArgumentNullException(nameof(second));
            if (resultSelector == null)
                throw new ArgumentNullException(nameof(resultSelector));

            return s_impl.ForkJoin<TSource1, TSource2, TResult>(first, second, resultSelector);
        }

        /// <summary>
        /// Runs all specified observable sequences in parallel and collects their last elements.
        /// </summary>
        /// <typeparam name="TSource">The type of the elements in the source sequences.</typeparam>
        /// <param name="sources">Observable sequence to collect the last elements for.</param>
        /// <returns>An observable sequence with an array collecting the last elements of all the input sequences.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="sources"/> is null.</exception>
        [Experimental]
        public static IObservable<TSource[]> ForkJoin<TSource>(params IObservable<TSource>[] sources)
        {
            if (sources == null)
                throw new ArgumentNullException(nameof(sources));

            return s_impl.ForkJoin<TSource>(sources);
        }

        /// <summary>
        /// Runs all observable sequences in the enumerable sources sequence in parallel and collect their last elements.
        /// </summary>
        /// <typeparam name="TSource">The type of the elements in the source sequences.</typeparam>
        /// <param name="sources">Observable sequence to collect the last elements for.</param>
        /// <returns>An observable sequence with an array collecting the last elements of all the input sequences.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="sources"/> is null.</exception>
        [Experimental]
        public static IObservable<TSource[]> ForkJoin<TSource>(this IEnumerable<IObservable<TSource>> sources)
        {
            if (sources == null)
                throw new ArgumentNullException(nameof(sources));

            return s_impl.ForkJoin<TSource>(sources);
        }

        #endregion

        #region Let

        /// <summary>
        /// Returns an observable sequence that is the result of invoking the selector on the source sequence, without sharing subscriptions.
        /// This operator allows for a fluent style of writing queries that use the same sequence multiple times.
        /// </summary>
        /// <typeparam name="TSource">The type of the elements in the source sequence.</typeparam>
        /// <typeparam name="TResult">The type of the elements in the result sequence.</typeparam>
        /// <param name="source">Source sequence that will be shared in the selector function.</param>
        /// <param name="selector">Selector function which can use the source sequence as many times as needed, without sharing subscriptions to the source sequence.</param>
        /// <returns>An observable sequence that contains the elements of a sequence produced by multicasting the source sequence within a selector function.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="source"/> or <paramref name="selector"/> is null.</exception>
        [Experimental]
        public static IObservable<TResult> Let<TSource, TResult>(this IObservable<TSource> source, Func<IObservable<TSource>, IObservable<TResult>> selector)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));
            if (selector == null)
                throw new ArgumentNullException(nameof(selector));

            return s_impl.Let<TSource, TResult>(source, selector);
        }

        #endregion

        #region ManySelect

        /// <summary>
        /// Comonadic bind operator.
        /// </summary>
        [Experimental]
        public static IObservable<TResult> ManySelect<TSource, TResult>(this IObservable<TSource> source, Func<IObservable<TSource>, TResult> selector, IScheduler scheduler)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));
            if (selector == null)
                throw new ArgumentNullException(nameof(selector));
            if (scheduler == null)
                throw new ArgumentNullException(nameof(scheduler));

            return s_impl.ManySelect<TSource, TResult>(source, selector, scheduler);
        }

        /// <summary>
        /// Comonadic bind operator.
        /// </summary>
        [Experimental]
        public static IObservable<TResult> ManySelect<TSource, TResult>(this IObservable<TSource> source, Func<IObservable<TSource>, TResult> selector)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));
            if (selector == null)
                throw new ArgumentNullException(nameof(selector));

            return s_impl.ManySelect<TSource, TResult>(source, selector);
        }

        #endregion

        #region ToListObservable

        /// <summary>
        /// Immediately subscribes to source and retains the elements in the observable sequence.
        /// </summary>
        /// <typeparam name="TSource">The type of the elements in the source sequence.</typeparam>
        /// <param name="source">Source sequence.</param>
        /// <returns>Object that's both an observable sequence and a list which can be used to access the source sequence's elements.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="source"/> is null.</exception>
        [Experimental]
        public static ListObservable<TSource> ToListObservable<TSource>(this IObservable<TSource> source)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));

            return s_impl.ToListObservable<TSource>(source);
        }

        #endregion
    }
}

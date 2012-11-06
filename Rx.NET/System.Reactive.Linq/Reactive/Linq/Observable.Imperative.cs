// Copyright (c) Microsoft Open Technologies, Inc. All rights reserved. See License.txt in the project root for license information.

using System.Collections.Generic;
using System.Reactive.Concurrency;
using System.Threading;

#if !NO_TPL
using System.Threading.Tasks;
#endif

namespace System.Reactive.Linq
{
    public static partial class Observable
    {
        #region ForEachAsync

#if !NO_TPL
        /// <summary>
        /// Invokes an action for each element in the observable sequence, and returns a Task object that will get signaled when the sequence terminates.
        /// </summary>
        /// <typeparam name="TSource">The type of the elements in the source sequence.</typeparam>
        /// <param name="source">Source sequence.</param>
        /// <param name="onNext">Action to invoke for each element in the observable sequence.</param>
        /// <returns>Task that signals the termination of the sequence.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="source"/> or <paramref name="onNext"/> is null.</exception>
        /// <remarks>This operator is especially useful in conjunction with the asynchronous programming features introduced in C# 5.0 and Visual Basic 11.</remarks>
        public static Task ForEachAsync<TSource>(this IObservable<TSource> source, Action<TSource> onNext)
        {
            if (source == null)
                throw new ArgumentNullException("source");
            if (onNext == null)
                throw new ArgumentNullException("onNext");

            return s_impl.ForEachAsync<TSource>(source, onNext);
        }

        /// <summary>
        /// Invokes an action for each element in the observable sequence, and returns a Task object that will get signaled when the sequence terminates.
        /// The loop can be quit prematurely by setting the specified cancellation token.
        /// </summary>
        /// <typeparam name="TSource">The type of the elements in the source sequence.</typeparam>
        /// <param name="source">Source sequence.</param>
        /// <param name="onNext">Action to invoke for each element in the observable sequence.</param>
        /// <param name="cancellationToken">Cancellation token used to stop the loop.</param>
        /// <returns>Task that signals the termination of the sequence.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="source"/> or <paramref name="onNext"/> is null.</exception>
        /// <remarks>This operator is especially useful in conjunction with the asynchronous programming features introduced in C# 5.0 and Visual Basic 11.</remarks>
        public static Task ForEachAsync<TSource>(this IObservable<TSource> source, Action<TSource> onNext, CancellationToken cancellationToken)
        {
            if (source == null)
                throw new ArgumentNullException("source");
            if (onNext == null)
                throw new ArgumentNullException("onNext");

            return s_impl.ForEachAsync<TSource>(source, onNext, cancellationToken);
        }

        /// <summary>
        /// Invokes an action for each element in the observable sequence, incorporating the element's index, and returns a Task object that will get signaled when the sequence terminates.
        /// </summary>
        /// <typeparam name="TSource">The type of the elements in the source sequence.</typeparam>
        /// <param name="source">Source sequence.</param>
        /// <param name="onNext">Action to invoke for each element in the observable sequence.</param>
        /// <returns>Task that signals the termination of the sequence.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="source"/> or <paramref name="onNext"/> is null.</exception>
        /// <remarks>This operator is especially useful in conjunction with the asynchronous programming features introduced in C# 5.0 and Visual Basic 11.</remarks>
        public static Task ForEachAsync<TSource>(this IObservable<TSource> source, Action<TSource, int> onNext)
        {
            if (source == null)
                throw new ArgumentNullException("source");
            if (onNext == null)
                throw new ArgumentNullException("onNext");

            return s_impl.ForEachAsync<TSource>(source, onNext);
        }

        /// <summary>
        /// Invokes an action for each element in the observable sequence, incorporating the element's index, and returns a Task object that will get signaled when the sequence terminates.
        /// The loop can be quit prematurely by setting the specified cancellation token.
        /// </summary>
        /// <typeparam name="TSource">The type of the elements in the source sequence.</typeparam>
        /// <param name="source">Source sequence.</param>
        /// <param name="onNext">Action to invoke for each element in the observable sequence.</param>
        /// <param name="cancellationToken">Cancellation token used to stop the loop.</param>
        /// <returns>Task that signals the termination of the sequence.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="source"/> or <paramref name="onNext"/> is null.</exception>
        /// <remarks>This operator is especially useful in conjunction with the asynchronous programming features introduced in C# 5.0 and Visual Basic 11.</remarks>
        public static Task ForEachAsync<TSource>(this IObservable<TSource> source, Action<TSource, int> onNext, CancellationToken cancellationToken)
        {
            if (source == null)
                throw new ArgumentNullException("source");
            if (onNext == null)
                throw new ArgumentNullException("onNext");

            return s_impl.ForEachAsync<TSource>(source, onNext, cancellationToken);
        }
#endif

        #endregion

        #region + Case +

        /// <summary>
        /// Uses <paramref name="selector"/> to determine which source in <paramref name="sources"/> to return, choosing <paramref name="defaultSource"/> if no match is found.
        /// </summary>
        /// <typeparam name="TValue">The type of the value returned by the selector function, used to look up the resulting source.</typeparam>
        /// <typeparam name="TResult">The type of the elements in the result sequence.</typeparam>
        /// <param name="selector">Selector function invoked to determine the source to lookup in the <paramref name="sources"/> dictionary.</param>
        /// <param name="sources">Dictionary of sources to select from based on the <paramref name="selector"/> invocation result.</param>
        /// <param name="defaultSource">Default source to select in case no matching source in <paramref name="sources"/> is found.</param>
        /// <returns>The observable sequence retrieved from the <paramref name="sources"/> dictionary based on the <paramref name="selector"/> invocation result, or <paramref name="defaultSource"/> if no match is found.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="selector"/> or <paramref name="sources"/> or <paramref name="defaultSource"/> is null.</exception>
        public static IObservable<TResult> Case<TValue, TResult>(Func<TValue> selector, IDictionary<TValue, IObservable<TResult>> sources, IObservable<TResult> defaultSource)
        {
            if (selector == null)
                throw new ArgumentNullException("selector");
            if (sources == null)
                throw new ArgumentNullException("sources");
            if (defaultSource == null)
                throw new ArgumentNullException("defaultSource");

            return s_impl.Case<TValue, TResult>(selector, sources, defaultSource);
        }

        /// <summary>
        /// Uses <paramref name="selector"/> to determine which source in <paramref name="sources"/> to return, choosing an empty sequence on the specified scheduler if no match is found.
        /// </summary>
        /// <typeparam name="TValue">The type of the value returned by the selector function, used to look up the resulting source.</typeparam>
        /// <typeparam name="TResult">The type of the elements in the result sequence.</typeparam>
        /// <param name="selector">Selector function invoked to determine the source to lookup in the <paramref name="sources"/> dictionary.</param>
        /// <param name="sources">Dictionary of sources to select from based on the <paramref name="selector"/> invocation result.</param>
        /// <param name="scheduler">Scheduler to generate an empty sequence on in case no matching source in <paramref name="sources"/> is found.</param>
        /// <returns>The observable sequence retrieved from the <paramref name="sources"/> dictionary based on the <paramref name="selector"/> invocation result, or an empty sequence if no match is found.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="selector"/> or <paramref name="sources"/> or <paramref name="scheduler"/> is null.</exception>
        public static IObservable<TResult> Case<TValue, TResult>(Func<TValue> selector, IDictionary<TValue, IObservable<TResult>> sources, IScheduler scheduler)
        {
            if (selector == null)
                throw new ArgumentNullException("selector");
            if (sources == null)
                throw new ArgumentNullException("sources");
            if (scheduler == null)
                throw new ArgumentNullException("scheduler");

            return s_impl.Case<TValue, TResult>(selector, sources, scheduler);
        }

        /// <summary>
        /// Uses <paramref name="selector"/> to determine which source in <paramref name="sources"/> to return, choosing an empty sequence if no match is found.
        /// </summary>
        /// <typeparam name="TValue">The type of the value returned by the selector function, used to look up the resulting source.</typeparam>
        /// <typeparam name="TResult">The type of the elements in the result sequence.</typeparam>
        /// <param name="selector">Selector function invoked to determine the source to lookup in the <paramref name="sources"/> dictionary.</param>
        /// <param name="sources">Dictionary of sources to select from based on the <paramref name="selector"/> invocation result.</param>
        /// <returns>The observable sequence retrieved from the <paramref name="sources"/> dictionary based on the <paramref name="selector"/> invocation result, or an empty sequence if no match is found.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="selector"/> or <paramref name="sources"/> is null.</exception>
        public static IObservable<TResult> Case<TValue, TResult>(Func<TValue> selector, IDictionary<TValue, IObservable<TResult>> sources)
        {
            if (selector == null)
                throw new ArgumentNullException("selector");
            if (sources == null)
                throw new ArgumentNullException("sources");

            return s_impl.Case<TValue, TResult>(selector, sources);
        }

        #endregion

        #region + DoWhile +

        /// <summary>
        /// Repeats the given <paramref name="source"/> as long as the specified <paramref name="condition"/> holds, where the <paramref name="condition"/> is evaluated after each repeated <paramref name="source"/> completed.
        /// </summary>
        /// <typeparam name="TSource">The type of the elements in the source sequence.</typeparam>
        /// <param name="source">Source to repeat as long as the <paramref name="condition"/> function evaluates to true.</param>
        /// <param name="condition">Condition that will be evaluated upon the completion of an iteration through the <paramref name="source"/>, to determine whether repetition of the source is required.</param>
        /// <returns>The observable sequence obtained by concatenating the <paramref name="source"/> sequence as long as the <paramref name="condition"/> holds.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="source"/> or <paramref name="condition"/> is null.</exception>
        public static IObservable<TSource> DoWhile<TSource>(this IObservable<TSource> source, Func<bool> condition)
        {
            if (source == null)
                throw new ArgumentNullException("source");
            if (condition == null)
                throw new ArgumentNullException("condition");

            return s_impl.DoWhile<TSource>(source, condition);
        }

        #endregion

        #region + For +

        /// <summary>
        /// Concatenates the observable sequences obtained by running the <paramref name="resultSelector"/> for each element in the given enumerable <paramref name="source"/>.
        /// </summary>
        /// <typeparam name="TSource">The type of the elements in the enumerable source sequence.</typeparam>
        /// <typeparam name="TResult">The type of the elements in the observable result sequence.</typeparam>
        /// <param name="source">Enumerable source for which each element will be mapped onto an observable source that will be concatenated in the result sequence.</param>
        /// <param name="resultSelector">Function to select an observable source for each element in the <paramref name="source"/>.</param>
        /// <returns>The observable sequence obtained by concatenating the sources returned by <paramref name="resultSelector"/> for each element in the <paramref name="source"/>.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="source"/> or <paramref name="resultSelector"/> is null.</exception>
        public static IObservable<TResult> For<TSource, TResult>(IEnumerable<TSource> source, Func<TSource, IObservable<TResult>> resultSelector)
        {
            if (source == null)
                throw new ArgumentNullException("source");
            if (resultSelector == null)
                throw new ArgumentNullException("resultSelector");

            return s_impl.For<TSource, TResult>(source, resultSelector);
        }

        #endregion

        #region + If +

        /// <summary>
        /// If the specified <paramref name="condition"/> evaluates true, select the <paramref name="thenSource"/> sequence. Otherwise, select the <paramref name="elseSource"/> sequence.
        /// </summary>
        /// <typeparam name="TResult">The type of the elements in the result sequence.</typeparam>
        /// <param name="condition">Condition evaluated to decide which sequence to return.</param>
        /// <param name="thenSource">Sequence returned in case <paramref name="condition"/> evaluates true.</param>
        /// <param name="elseSource">Sequence returned in case <paramref name="condition"/> evaluates false.</param>
        /// <returns><paramref name="thenSource"/> if <paramref name="condition"/> evaluates true; <paramref name="elseSource"/> otherwise.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="condition"/> or <paramref name="thenSource"/> or <paramref name="elseSource"/> is null.</exception>
        public static IObservable<TResult> If<TResult>(Func<bool> condition, IObservable<TResult> thenSource, IObservable<TResult> elseSource)
        {
            if (condition == null)
                throw new ArgumentNullException("condition");
            if (thenSource == null)
                throw new ArgumentNullException("thenSource");
            if (elseSource == null)
                throw new ArgumentNullException("elseSource");

            return s_impl.If<TResult>(condition, thenSource, elseSource);
        }

        /// <summary>
        /// If the specified <paramref name="condition"/> evaluates true, select the <paramref name="thenSource"/> sequence. Otherwise, return an empty sequence.
        /// </summary>
        /// <typeparam name="TResult">The type of the elements in the result sequence.</typeparam>
        /// <param name="condition">Condition evaluated to decide which sequence to return.</param>
        /// <param name="thenSource">Sequence returned in case <paramref name="condition"/> evaluates true.</param>
        /// <returns><paramref name="thenSource"/> if <paramref name="condition"/> evaluates true; an empty sequence otherwise.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="condition"/> or <paramref name="thenSource"/> is null.</exception>
        public static IObservable<TResult> If<TResult>(Func<bool> condition, IObservable<TResult> thenSource)
        {
            if (condition == null)
                throw new ArgumentNullException("condition");
            if (thenSource == null)
                throw new ArgumentNullException("thenSource");

            return s_impl.If<TResult>(condition, thenSource);
        }

        /// <summary>
        /// If the specified <paramref name="condition"/> evaluates true, select the <paramref name="thenSource"/> sequence. Otherwise, return an empty sequence generated on the specified scheduler.
        /// </summary>
        /// <typeparam name="TResult">The type of the elements in the result sequence.</typeparam>
        /// <param name="condition">Condition evaluated to decide which sequence to return.</param>
        /// <param name="thenSource">Sequence returned in case <paramref name="condition"/> evaluates true.</param>
        /// <param name="scheduler">Scheduler to generate an empty sequence on in case <paramref name="condition"/> evaluates false.</param>
        /// <returns><paramref name="thenSource"/> if <paramref name="condition"/> evaluates true; an empty sequence otherwise.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="condition"/> or <paramref name="thenSource"/> or <paramref name="scheduler"/> is null.</exception>
        public static IObservable<TResult> If<TResult>(Func<bool> condition, IObservable<TResult> thenSource, IScheduler scheduler)
        {
            if (condition == null)
                throw new ArgumentNullException("condition");
            if (thenSource == null)
                throw new ArgumentNullException("thenSource");
            if (scheduler == null)
                throw new ArgumentNullException("scheduler");

            return s_impl.If<TResult>(condition, thenSource, scheduler);
        }

        #endregion

        #region + While +

        /// <summary>
        /// Repeats the given <paramref name="source"/> as long as the specified <paramref name="condition"/> holds, where the <paramref name="condition"/> is evaluated before each repeated <paramref name="source"/> is subscribed to.
        /// </summary>
        /// <typeparam name="TSource">The type of the elements in the source sequence.</typeparam>
        /// <param name="source">Source to repeat as long as the <paramref name="condition"/> function evaluates to true.</param>
        /// <param name="condition">Condition that will be evaluated before subscription to the <paramref name="source"/>, to determine whether repetition of the source is required.</param>
        /// <returns>The observable sequence obtained by concatenating the <paramref name="source"/> sequence as long as the <paramref name="condition"/> holds.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="condition"/> or <paramref name="source"/> is null.</exception>
        public static IObservable<TSource> While<TSource>(Func<bool> condition, IObservable<TSource> source)
        {
            if (condition == null)
                throw new ArgumentNullException("condition");
            if (source == null)
                throw new ArgumentNullException("source");

            return s_impl.While<TSource>(condition, source);
        }

        #endregion
    }
}
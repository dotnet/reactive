/*
 * WARNING: Auto-generated file (2026/03/16 11:20:01)
 * Run Rx's auto-homoiconizer tool to generate this file (in the HomoIcon directory).
 */

#nullable enable
#pragma warning disable 1591

using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reactive.Concurrency;
using System.Reactive.Subjects;
using System.Reflection;
using System.Threading;
using System.Threading.Tasks;

namespace System.Reactive.Linq
{
    public static partial class QbservableEx
    {
        /// <summary>
        /// Merges the specified observable sequences into one observable sequence of tuple values whenever any of the observable sequences produces an element.
        /// </summary>
        /// <typeparam name="TFirst">The type of the elements in the first source sequence.</typeparam>
        /// <typeparam name="TSecond">The type of the elements in the second source sequence.</typeparam>
        /// <param name="first">First observable source.</param>
        /// <param name="second">Second observable source.</param>
        /// <returns>An observable sequence containing the result of combining elements of the sources using tuple values.</returns>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="first" /> or <paramref name="second" /> is null.</exception>
        public static IQbservable<ValueTuple<TFirst, TSecond>> CombineLatest<TFirst, TSecond>(this IQbservable<TFirst> first, IObservable<TSecond> second)
        {
            if (first == null)
                throw new ArgumentNullException(nameof(first));
            if (second == null)
                throw new ArgumentNullException(nameof(second));

            return first.Provider.CreateQuery<ValueTuple<TFirst, TSecond>>(
                Expression.Call(
                    null,
                    ((MethodInfo)MethodInfo.GetCurrentMethod()!).MakeGenericMethod(typeof(TFirst), typeof(TSecond)),
                    first.Expression,
                    GetSourceExpression(second)
                )
            );
        }

        /// <summary>
        /// Merges the specified observable sequences into one observable sequence of tuple values whenever any of the observable sequences produces an element.
        /// </summary>
        /// <typeparam name="TFirst">The type of the elements in the first source sequence.</typeparam>
        /// <typeparam name="TSecond">The type of the elements in the second source sequence.</typeparam>
        /// <typeparam name="TThird">The type of the elements in the third source sequence.</typeparam>
        /// <param name="first">First observable source.</param>
        /// <param name="second">Second observable source.</param>
        /// <param name="third">Third observable source.</param>
        /// <returns>An observable sequence containing the result of combining elements of the sources using tuple values.</returns>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="first" /> or <paramref name="second" /> or <paramref name="third" /> is null.</exception>
        public static IQbservable<ValueTuple<TFirst, TSecond, TThird>> CombineLatest<TFirst, TSecond, TThird>(this IQbservable<TFirst> first, IObservable<TSecond> second, IObservable<TThird> third)
        {
            if (first == null)
                throw new ArgumentNullException(nameof(first));
            if (second == null)
                throw new ArgumentNullException(nameof(second));
            if (third == null)
                throw new ArgumentNullException(nameof(third));

            return first.Provider.CreateQuery<ValueTuple<TFirst, TSecond, TThird>>(
                Expression.Call(
                    null,
                    ((MethodInfo)MethodInfo.GetCurrentMethod()!).MakeGenericMethod(typeof(TFirst), typeof(TSecond), typeof(TThird)),
                    first.Expression,
                    GetSourceExpression(second),
                    GetSourceExpression(third)
                )
            );
        }

        /// <summary>
        /// Merges the specified observable sequences into one observable sequence of tuple values whenever any of the observable sequences produces an element.
        /// </summary>
        /// <typeparam name="TFirst">The type of the elements in the first source sequence.</typeparam>
        /// <typeparam name="TSecond">The type of the elements in the second source sequence.</typeparam>
        /// <typeparam name="TThird">The type of the elements in the third source sequence.</typeparam>
        /// <typeparam name="TFourth">The type of the elements in the fourth source sequence.</typeparam>
        /// <param name="first">First observable source.</param>
        /// <param name="second">Second observable source.</param>
        /// <param name="third">Third observable source.</param>
        /// <param name="fourth">Fourth observable source.</param>
        /// <returns>An observable sequence containing the result of combining elements of the sources using tuple values.</returns>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="first" /> or <paramref name="second" /> or <paramref name="third" /> or <paramref name="fourth" /> is null.</exception>
        public static IQbservable<ValueTuple<TFirst, TSecond, TThird, TFourth>> CombineLatest<TFirst, TSecond, TThird, TFourth>(this IQbservable<TFirst> first, IObservable<TSecond> second, IObservable<TThird> third, IObservable<TFourth> fourth)
        {
            if (first == null)
                throw new ArgumentNullException(nameof(first));
            if (second == null)
                throw new ArgumentNullException(nameof(second));
            if (third == null)
                throw new ArgumentNullException(nameof(third));
            if (fourth == null)
                throw new ArgumentNullException(nameof(fourth));

            return first.Provider.CreateQuery<ValueTuple<TFirst, TSecond, TThird, TFourth>>(
                Expression.Call(
                    null,
                    ((MethodInfo)MethodInfo.GetCurrentMethod()!).MakeGenericMethod(typeof(TFirst), typeof(TSecond), typeof(TThird), typeof(TFourth)),
                    first.Expression,
                    GetSourceExpression(second),
                    GetSourceExpression(third),
                    GetSourceExpression(fourth)
                )
            );
        }

        /// <summary>
        /// Merges the specified observable sequences into one observable sequence of tuple values whenever any of the observable sequences produces an element.
        /// </summary>
        /// <typeparam name="TFirst">The type of the elements in the first source sequence.</typeparam>
        /// <typeparam name="TSecond">The type of the elements in the second source sequence.</typeparam>
        /// <typeparam name="TThird">The type of the elements in the third source sequence.</typeparam>
        /// <typeparam name="TFourth">The type of the elements in the fourth source sequence.</typeparam>
        /// <typeparam name="TFifth">The type of the elements in the fifth source sequence.</typeparam>
        /// <param name="first">First observable source.</param>
        /// <param name="second">Second observable source.</param>
        /// <param name="third">Third observable source.</param>
        /// <param name="fourth">Fourth observable source.</param>
        /// <param name="fifth">Fifth observable source.</param>
        /// <returns>An observable sequence containing the result of combining elements of the sources using tuple values.</returns>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="first" /> or <paramref name="second" /> or <paramref name="third" /> or <paramref name="fourth" /> or <paramref name="fifth" /> is null.</exception>
        public static IQbservable<ValueTuple<TFirst, TSecond, TThird, TFourth, TFifth>> CombineLatest<TFirst, TSecond, TThird, TFourth, TFifth>(this IQbservable<TFirst> first, IObservable<TSecond> second, IObservable<TThird> third, IObservable<TFourth> fourth, IObservable<TFifth> fifth)
        {
            if (first == null)
                throw new ArgumentNullException(nameof(first));
            if (second == null)
                throw new ArgumentNullException(nameof(second));
            if (third == null)
                throw new ArgumentNullException(nameof(third));
            if (fourth == null)
                throw new ArgumentNullException(nameof(fourth));
            if (fifth == null)
                throw new ArgumentNullException(nameof(fifth));

            return first.Provider.CreateQuery<ValueTuple<TFirst, TSecond, TThird, TFourth, TFifth>>(
                Expression.Call(
                    null,
                    ((MethodInfo)MethodInfo.GetCurrentMethod()!).MakeGenericMethod(typeof(TFirst), typeof(TSecond), typeof(TThird), typeof(TFourth), typeof(TFifth)),
                    first.Expression,
                    GetSourceExpression(second),
                    GetSourceExpression(third),
                    GetSourceExpression(fourth),
                    GetSourceExpression(fifth)
                )
            );
        }

        /// <summary>
        /// Merges the specified observable sequences into one observable sequence of tuple values whenever any of the observable sequences produces an element.
        /// </summary>
        /// <typeparam name="TFirst">The type of the elements in the first source sequence.</typeparam>
        /// <typeparam name="TSecond">The type of the elements in the second source sequence.</typeparam>
        /// <typeparam name="TThird">The type of the elements in the third source sequence.</typeparam>
        /// <typeparam name="TFourth">The type of the elements in the fourth source sequence.</typeparam>
        /// <typeparam name="TFifth">The type of the elements in the fifth source sequence.</typeparam>
        /// <typeparam name="TSixth">The type of the elements in the sixth source sequence.</typeparam>
        /// <param name="first">First observable source.</param>
        /// <param name="second">Second observable source.</param>
        /// <param name="third">Third observable source.</param>
        /// <param name="fourth">Fourth observable source.</param>
        /// <param name="fifth">Fifth observable source.</param>
        /// <param name="sixth">Sixth observable source.</param>
        /// <returns>An observable sequence containing the result of combining elements of the sources using tuple values.</returns>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="first" /> or <paramref name="second" /> or <paramref name="third" /> or <paramref name="fourth" /> or <paramref name="fifth" /> or <paramref name="sixth" /> is null.</exception>
        public static IQbservable<ValueTuple<TFirst, TSecond, TThird, TFourth, TFifth, TSixth>> CombineLatest<TFirst, TSecond, TThird, TFourth, TFifth, TSixth>(this IQbservable<TFirst> first, IObservable<TSecond> second, IObservable<TThird> third, IObservable<TFourth> fourth, IObservable<TFifth> fifth, IObservable<TSixth> sixth)
        {
            if (first == null)
                throw new ArgumentNullException(nameof(first));
            if (second == null)
                throw new ArgumentNullException(nameof(second));
            if (third == null)
                throw new ArgumentNullException(nameof(third));
            if (fourth == null)
                throw new ArgumentNullException(nameof(fourth));
            if (fifth == null)
                throw new ArgumentNullException(nameof(fifth));
            if (sixth == null)
                throw new ArgumentNullException(nameof(sixth));

            return first.Provider.CreateQuery<ValueTuple<TFirst, TSecond, TThird, TFourth, TFifth, TSixth>>(
                Expression.Call(
                    null,
                    ((MethodInfo)MethodInfo.GetCurrentMethod()!).MakeGenericMethod(typeof(TFirst), typeof(TSecond), typeof(TThird), typeof(TFourth), typeof(TFifth), typeof(TSixth)),
                    first.Expression,
                    GetSourceExpression(second),
                    GetSourceExpression(third),
                    GetSourceExpression(fourth),
                    GetSourceExpression(fifth),
                    GetSourceExpression(sixth)
                )
            );
        }

        /// <summary>
        /// Merges the specified observable sequences into one observable sequence of tuple values whenever any of the observable sequences produces an element.
        /// </summary>
        /// <typeparam name="TFirst">The type of the elements in the first source sequence.</typeparam>
        /// <typeparam name="TSecond">The type of the elements in the second source sequence.</typeparam>
        /// <typeparam name="TThird">The type of the elements in the third source sequence.</typeparam>
        /// <typeparam name="TFourth">The type of the elements in the fourth source sequence.</typeparam>
        /// <typeparam name="TFifth">The type of the elements in the fifth source sequence.</typeparam>
        /// <typeparam name="TSixth">The type of the elements in the sixth source sequence.</typeparam>
        /// <typeparam name="TSeventh">The type of the elements in the seventh source sequence.</typeparam>
        /// <param name="first">First observable source.</param>
        /// <param name="second">Second observable source.</param>
        /// <param name="third">Third observable source.</param>
        /// <param name="fourth">Fourth observable source.</param>
        /// <param name="fifth">Fifth observable source.</param>
        /// <param name="sixth">Sixth observable source.</param>
        /// <param name="seventh">Seventh observable source.</param>
        /// <returns>An observable sequence containing the result of combining elements of the sources using tuple values.</returns>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="first" /> or <paramref name="second" /> or <paramref name="third" /> or <paramref name="fourth" /> or <paramref name="fifth" /> or <paramref name="sixth" /> or <paramref name="seventh" /> is null.</exception>
        public static IQbservable<ValueTuple<TFirst, TSecond, TThird, TFourth, TFifth, TSixth, TSeventh>> CombineLatest<TFirst, TSecond, TThird, TFourth, TFifth, TSixth, TSeventh>(this IQbservable<TFirst> first, IObservable<TSecond> second, IObservable<TThird> third, IObservable<TFourth> fourth, IObservable<TFifth> fifth, IObservable<TSixth> sixth, IObservable<TSeventh> seventh)
        {
            if (first == null)
                throw new ArgumentNullException(nameof(first));
            if (second == null)
                throw new ArgumentNullException(nameof(second));
            if (third == null)
                throw new ArgumentNullException(nameof(third));
            if (fourth == null)
                throw new ArgumentNullException(nameof(fourth));
            if (fifth == null)
                throw new ArgumentNullException(nameof(fifth));
            if (sixth == null)
                throw new ArgumentNullException(nameof(sixth));
            if (seventh == null)
                throw new ArgumentNullException(nameof(seventh));

            return first.Provider.CreateQuery<ValueTuple<TFirst, TSecond, TThird, TFourth, TFifth, TSixth, TSeventh>>(
                Expression.Call(
                    null,
                    ((MethodInfo)MethodInfo.GetCurrentMethod()!).MakeGenericMethod(typeof(TFirst), typeof(TSecond), typeof(TThird), typeof(TFourth), typeof(TFifth), typeof(TSixth), typeof(TSeventh)),
                    first.Expression,
                    GetSourceExpression(second),
                    GetSourceExpression(third),
                    GetSourceExpression(fourth),
                    GetSourceExpression(fifth),
                    GetSourceExpression(sixth),
                    GetSourceExpression(seventh)
                )
            );
        }

        /// <summary>
        /// Merges the specified observable sequences into one observable sequence of tuple values whenever any of the observable sequences produces an element.
        /// </summary>
        /// <typeparam name="TFirst">The type of the elements in the first source sequence.</typeparam>
        /// <typeparam name="TSecond">The type of the elements in the second source sequence.</typeparam>
        /// <typeparam name="TThird">The type of the elements in the third source sequence.</typeparam>
        /// <typeparam name="TFourth">The type of the elements in the fourth source sequence.</typeparam>
        /// <typeparam name="TFifth">The type of the elements in the fifth source sequence.</typeparam>
        /// <typeparam name="TSixth">The type of the elements in the sixth source sequence.</typeparam>
        /// <typeparam name="TSeventh">The type of the elements in the seventh source sequence.</typeparam>
        /// <typeparam name="TEighth">The type of the elements in the eighth source sequence.</typeparam>
        /// <param name="first">First observable source.</param>
        /// <param name="second">Second observable source.</param>
        /// <param name="third">Third observable source.</param>
        /// <param name="fourth">Fourth observable source.</param>
        /// <param name="fifth">Fifth observable source.</param>
        /// <param name="sixth">Sixth observable source.</param>
        /// <param name="seventh">Seventh observable source.</param>
        /// <param name="eighth">Eighth observable source.</param>
        /// <returns>An observable sequence containing the result of combining elements of the sources using tuple values.</returns>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="first" /> or <paramref name="second" /> or <paramref name="third" /> or <paramref name="fourth" /> or <paramref name="fifth" /> or <paramref name="sixth" /> or <paramref name="seventh" /> or <paramref name="eighth" /> is null.</exception>
        public static IQbservable<ValueTuple<TFirst, TSecond, TThird, TFourth, TFifth, TSixth, TSeventh, ValueTuple<TEighth>>> CombineLatest<TFirst, TSecond, TThird, TFourth, TFifth, TSixth, TSeventh, TEighth>(this IQbservable<TFirst> first, IObservable<TSecond> second, IObservable<TThird> third, IObservable<TFourth> fourth, IObservable<TFifth> fifth, IObservable<TSixth> sixth, IObservable<TSeventh> seventh, IObservable<TEighth> eighth)
        {
            if (first == null)
                throw new ArgumentNullException(nameof(first));
            if (second == null)
                throw new ArgumentNullException(nameof(second));
            if (third == null)
                throw new ArgumentNullException(nameof(third));
            if (fourth == null)
                throw new ArgumentNullException(nameof(fourth));
            if (fifth == null)
                throw new ArgumentNullException(nameof(fifth));
            if (sixth == null)
                throw new ArgumentNullException(nameof(sixth));
            if (seventh == null)
                throw new ArgumentNullException(nameof(seventh));
            if (eighth == null)
                throw new ArgumentNullException(nameof(eighth));

            return first.Provider.CreateQuery<ValueTuple<TFirst, TSecond, TThird, TFourth, TFifth, TSixth, TSeventh, ValueTuple<TEighth>>>(
                Expression.Call(
                    null,
                    ((MethodInfo)MethodInfo.GetCurrentMethod()!).MakeGenericMethod(typeof(TFirst), typeof(TSecond), typeof(TThird), typeof(TFourth), typeof(TFifth), typeof(TSixth), typeof(TSeventh), typeof(TEighth)),
                    first.Expression,
                    GetSourceExpression(second),
                    GetSourceExpression(third),
                    GetSourceExpression(fourth),
                    GetSourceExpression(fifth),
                    GetSourceExpression(sixth),
                    GetSourceExpression(seventh),
                    GetSourceExpression(eighth)
                )
            );
        }

#if !STABLE
        /// <summary>
        /// Subscribes to each observable sequence returned by the iteratorMethod in sequence and produces a Unit value on the resulting sequence for each step of the iteration.
        /// </summary>
        /// <param name="provider">Query provider used to construct the <see cref="IQbservable{T}"/> data source.</param>
        /// <param name="iteratorMethod">Iterator method that drives the resulting observable sequence.</param>
        /// <returns>An observable sequence obtained by running the iterator and returning Unit values for each iteration step.</returns>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="iteratorMethod" /> is null.</exception>
        [Experimental]
        public static IQbservable<Unit> Create(this IQbservableProvider provider, Expression<Func<IEnumerable<IObservable<object>>>> iteratorMethod)
        {
            if (provider == null)
                throw new ArgumentNullException(nameof(provider));
            if (iteratorMethod == null)
                throw new ArgumentNullException(nameof(iteratorMethod));

            return provider.CreateQuery<Unit>(
                Expression.Call(
                    null,
                    (MethodInfo)MethodInfo.GetCurrentMethod()!,
                    Expression.Constant(provider, typeof(IQbservableProvider)),
                    iteratorMethod
                )
            );
        }
#endif

#if !STABLE
        /// <summary>
        /// Subscribes to each observable sequence returned by the iteratorMethod in sequence and returns the observable sequence of values sent to the observer given to the iteratorMethod.
        /// </summary>
        /// <param name="provider">Query provider used to construct the <see cref="IQbservable{T}"/> data source.</param>
        /// <typeparam name="TResult">The type of the elements in the produced sequence.</typeparam>
        /// <param name="iteratorMethod">Iterator method that produces elements in the resulting sequence by calling the given observer.</param>
        /// <returns>An observable sequence obtained by running the iterator and returning the elements that were sent to the observer.</returns>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="iteratorMethod" /> is null.</exception>
        [Experimental]
        public static IQbservable<TResult> Create<TResult>(this IQbservableProvider provider, Expression<Func<IObserver<TResult>, IEnumerable<IObservable<object>>>> iteratorMethod)
        {
            if (provider == null)
                throw new ArgumentNullException(nameof(provider));
            if (iteratorMethod == null)
                throw new ArgumentNullException(nameof(iteratorMethod));

            return provider.CreateQuery<TResult>(
                Expression.Call(
                    null,
                    ((MethodInfo)MethodInfo.GetCurrentMethod()!).MakeGenericMethod(typeof(TResult)),
                    Expression.Constant(provider, typeof(IQbservableProvider)),
                    iteratorMethod
                )
            );
        }
#endif

#if !STABLE
        /// <summary>
        /// Expands an observable sequence by recursively invoking selector.
        /// </summary>
        /// <typeparam name="TSource">The type of the elements in the source sequence and each of the recursively expanded sources obtained by running the selector function.</typeparam>
        /// <param name="source">Source sequence with the initial elements.</param>
        /// <param name="selector">Selector function to invoke for each produced element, resulting in another sequence to which the selector will be invoked recursively again.</param>
        /// <returns>An observable sequence containing all the elements produced by the recursive expansion.</returns>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="source" /> or <paramref name="selector" /> is null.</exception>
        [Experimental]
        public static IQbservable<TSource> Expand<TSource>(this IQbservable<TSource> source, Expression<Func<TSource, IObservable<TSource>>> selector)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));
            if (selector == null)
                throw new ArgumentNullException(nameof(selector));

            return source.Provider.CreateQuery<TSource>(
                Expression.Call(
                    null,
                    ((MethodInfo)MethodInfo.GetCurrentMethod()!).MakeGenericMethod(typeof(TSource)),
                    source.Expression,
                    selector
                )
            );
        }
#endif

#if !STABLE
        /// <summary>
        /// Expands an observable sequence by recursively invoking selector, using the specified scheduler to enumerate the queue of obtained sequences.
        /// </summary>
        /// <typeparam name="TSource">The type of the elements in the source sequence and each of the recursively expanded sources obtained by running the selector function.</typeparam>
        /// <param name="source">Source sequence with the initial elements.</param>
        /// <param name="selector">Selector function to invoke for each produced element, resulting in another sequence to which the selector will be invoked recursively again.</param>
        /// <param name="scheduler">Scheduler on which to perform the expansion by enumerating the internal queue of obtained sequences.</param>
        /// <returns>An observable sequence containing all the elements produced by the recursive expansion.</returns>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="source" /> or <paramref name="selector" /> or <paramref name="scheduler" /> is null.</exception>
        [Experimental]
        public static IQbservable<TSource> Expand<TSource>(this IQbservable<TSource> source, Expression<Func<TSource, IObservable<TSource>>> selector, IScheduler scheduler)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));
            if (selector == null)
                throw new ArgumentNullException(nameof(selector));
            if (scheduler == null)
                throw new ArgumentNullException(nameof(scheduler));

            return source.Provider.CreateQuery<TSource>(
                Expression.Call(
                    null,
                    ((MethodInfo)MethodInfo.GetCurrentMethod()!).MakeGenericMethod(typeof(TSource)),
                    source.Expression,
                    selector,
                    Expression.Constant(scheduler, typeof(IScheduler))
                )
            );
        }
#endif

#if !STABLE
        /// <summary>
        /// Runs all specified observable sequences in parallel and collects their last elements.
        /// </summary>
        /// <param name="provider">Query provider used to construct the <see cref="IQbservable{T}"/> data source.</param>
        /// <typeparam name="TSource">The type of the elements in the source sequences.</typeparam>
        /// <param name="sources">Observable sequence to collect the last elements for.</param>
        /// <returns>An observable sequence with an array collecting the last elements of all the input sequences.</returns>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="sources" /> is null.</exception>
        [Experimental]
        public static IQbservable<TSource[]> ForkJoin<TSource>(this IQbservableProvider provider, params IObservable<TSource>[] sources)
        {
            if (provider == null)
                throw new ArgumentNullException(nameof(provider));
            if (sources == null)
                throw new ArgumentNullException(nameof(sources));

            return provider.CreateQuery<TSource[]>(
                Expression.Call(
                    null,
                    ((MethodInfo)MethodInfo.GetCurrentMethod()!).MakeGenericMethod(typeof(TSource)),
                    Expression.Constant(provider, typeof(IQbservableProvider)),
                    GetSourceExpression(sources)
                )
            );
        }
#endif

#if !STABLE
        /// <summary>
        /// Runs all observable sequences in the enumerable sources sequence in parallel and collect their last elements.
        /// </summary>
        /// <param name="provider">Query provider used to construct the <see cref="IQbservable{T}"/> data source.</param>
        /// <typeparam name="TSource">The type of the elements in the source sequences.</typeparam>
        /// <param name="sources">Observable sequence to collect the last elements for.</param>
        /// <returns>An observable sequence with an array collecting the last elements of all the input sequences.</returns>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="sources" /> is null.</exception>
        [Experimental]
        public static IQbservable<TSource[]> ForkJoin<TSource>(this IQbservableProvider provider, IEnumerable<IObservable<TSource>> sources)
        {
            if (provider == null)
                throw new ArgumentNullException(nameof(provider));
            if (sources == null)
                throw new ArgumentNullException(nameof(sources));

            return provider.CreateQuery<TSource[]>(
                Expression.Call(
                    null,
                    ((MethodInfo)MethodInfo.GetCurrentMethod()!).MakeGenericMethod(typeof(TSource)),
                    Expression.Constant(provider, typeof(IQbservableProvider)),
                    GetSourceExpression(sources)
                )
            );
        }
#endif

#if !STABLE
        /// <summary>
        /// Runs two observable sequences in parallel and combines their last elements.
        /// </summary>
        /// <typeparam name="TSource1">The type of the elements in the first source sequence.</typeparam>
        /// <typeparam name="TSource2">The type of the elements in the second source sequence.</typeparam>
        /// <typeparam name="TResult">The type of the elements in the result sequence, returned by the selector function.</typeparam>
        /// <param name="first">First observable sequence.</param>
        /// <param name="second">Second observable sequence.</param>
        /// <param name="resultSelector">Result selector function to invoke with the last elements of both sequences.</param>
        /// <returns>An observable sequence with the result of calling the selector function with the last elements of both input sequences.</returns>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="first" /> or <paramref name="second" /> or <paramref name="resultSelector" /> is null.</exception>
        [Experimental]
        public static IQbservable<TResult> ForkJoin<TSource1, TSource2, TResult>(this IQbservable<TSource1> first, IObservable<TSource2> second, Expression<Func<TSource1, TSource2, TResult>> resultSelector)
        {
            if (first == null)
                throw new ArgumentNullException(nameof(first));
            if (second == null)
                throw new ArgumentNullException(nameof(second));
            if (resultSelector == null)
                throw new ArgumentNullException(nameof(resultSelector));

            return first.Provider.CreateQuery<TResult>(
                Expression.Call(
                    null,
                    ((MethodInfo)MethodInfo.GetCurrentMethod()!).MakeGenericMethod(typeof(TSource1), typeof(TSource2), typeof(TResult)),
                    first.Expression,
                    GetSourceExpression(second),
                    resultSelector
                )
            );
        }
#endif

#if !STABLE
        /// <summary>
        /// Returns an observable sequence that is the result of invoking the selector on the source sequence, without sharing subscriptions.
        /// This operator allows for a fluent style of writing queries that use the same sequence multiple times.
        /// </summary>
        /// <typeparam name="TSource">The type of the elements in the source sequence.</typeparam>
        /// <typeparam name="TResult">The type of the elements in the result sequence.</typeparam>
        /// <param name="source">Source sequence that will be shared in the selector function.</param>
        /// <param name="selector">Selector function which can use the source sequence as many times as needed, without sharing subscriptions to the source sequence.</param>
        /// <returns>An observable sequence that contains the elements of a sequence produced by multicasting the source sequence within a selector function.</returns>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="source" /> or <paramref name="selector" /> is null.</exception>
        [Experimental]
        public static IQbservable<TResult> Let<TSource, TResult>(this IQbservable<TSource> source, Expression<Func<IObservable<TSource>, IObservable<TResult>>> selector)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));
            if (selector == null)
                throw new ArgumentNullException(nameof(selector));

            return source.Provider.CreateQuery<TResult>(
                Expression.Call(
                    null,
                    ((MethodInfo)MethodInfo.GetCurrentMethod()!).MakeGenericMethod(typeof(TSource), typeof(TResult)),
                    source.Expression,
                    selector
                )
            );
        }
#endif

#if !STABLE
        /// <summary>
        /// Comonadic bind operator.
        /// </summary>
        [Experimental]
        public static IQbservable<TResult> ManySelect<TSource, TResult>(this IQbservable<TSource> source, Expression<Func<IObservable<TSource>, TResult>> selector)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));
            if (selector == null)
                throw new ArgumentNullException(nameof(selector));

            return source.Provider.CreateQuery<TResult>(
                Expression.Call(
                    null,
                    ((MethodInfo)MethodInfo.GetCurrentMethod()!).MakeGenericMethod(typeof(TSource), typeof(TResult)),
                    source.Expression,
                    selector
                )
            );
        }
#endif

#if !STABLE
        /// <summary>
        /// Comonadic bind operator.
        /// </summary>
        [Experimental]
        public static IQbservable<TResult> ManySelect<TSource, TResult>(this IQbservable<TSource> source, Expression<Func<IObservable<TSource>, TResult>> selector, IScheduler scheduler)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));
            if (selector == null)
                throw new ArgumentNullException(nameof(selector));
            if (scheduler == null)
                throw new ArgumentNullException(nameof(scheduler));

            return source.Provider.CreateQuery<TResult>(
                Expression.Call(
                    null,
                    ((MethodInfo)MethodInfo.GetCurrentMethod()!).MakeGenericMethod(typeof(TSource), typeof(TResult)),
                    source.Expression,
                    selector,
                    Expression.Constant(scheduler, typeof(IScheduler))
                )
            );
        }
#endif

        /// <summary>
        /// Merges two observable sequences into one observable sequence by combining each element from the first source with the latest element from the second source, if any.
        /// </summary>
        /// <typeparam name="TFirst">The type of the elements in the first source sequence.</typeparam>
        /// <typeparam name="TSecond">The type of the elements in the second source sequence.</typeparam>
        /// <param name="first">First observable source.</param>
        /// <param name="second">Second observable source.</param>
        /// <returns>An observable sequence containing the result of combining each element of the first source with the latest element from the second source, if any, as a tuple value.</returns>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="first" /> or <paramref name="second" /> is null.</exception>
        public static IQbservable<ValueTuple<TFirst, TSecond>> WithLatestFrom<TFirst, TSecond>(this IQbservable<TFirst> first, IObservable<TSecond> second)
        {
            if (first == null)
                throw new ArgumentNullException(nameof(first));
            if (second == null)
                throw new ArgumentNullException(nameof(second));

            return first.Provider.CreateQuery<ValueTuple<TFirst, TSecond>>(
                Expression.Call(
                    null,
                    ((MethodInfo)MethodInfo.GetCurrentMethod()!).MakeGenericMethod(typeof(TFirst), typeof(TSecond)),
                    first.Expression,
                    GetSourceExpression(second)
                )
            );
        }

        /// <summary>
        /// Merges an observable sequence and an enumerable sequence into one observable sequence of tuple values.
        /// </summary>
        /// <typeparam name="TFirst">The type of the elements in the first observable source sequence.</typeparam>
        /// <typeparam name="TSecond">The type of the elements in the second enumerable source sequence.</typeparam>
        /// <param name="first">First observable source.</param>
        /// <param name="second">Second enumerable source.</param>
        /// <returns>An observable sequence containing the result of pairwise combining the elements of the first and second source as a tuple value.</returns>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="first" /> or <paramref name="second" /> is null.</exception>
        public static IQbservable<ValueTuple<TFirst, TSecond>> Zip<TFirst, TSecond>(this IQbservable<TFirst> first, IEnumerable<TSecond> second)
        {
            if (first == null)
                throw new ArgumentNullException(nameof(first));
            if (second == null)
                throw new ArgumentNullException(nameof(second));

            return first.Provider.CreateQuery<ValueTuple<TFirst, TSecond>>(
                Expression.Call(
                    null,
                    ((MethodInfo)MethodInfo.GetCurrentMethod()!).MakeGenericMethod(typeof(TFirst), typeof(TSecond)),
                    first.Expression,
                    GetSourceExpression(second)
                )
            );
        }

        /// <summary>
        /// Merges the specified observable sequences into one observable sequence of tuple values whenever all of the observable sequences have produced an element at a corresponding index.
        /// </summary>
        /// <typeparam name="TFirst">The type of the elements in the first source sequence.</typeparam>
        /// <typeparam name="TSecond">The type of the elements in the second source sequence.</typeparam>
        /// <param name="first">First observable source.</param>
        /// <param name="second">Second observable source.</param>
        /// <returns>An observable sequence containing the result of combining elements of the sources using tuple values.</returns>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="first" /> or <paramref name="second" /> is null.</exception>
        public static IQbservable<ValueTuple<TFirst, TSecond>> Zip<TFirst, TSecond>(this IQbservable<TFirst> first, IObservable<TSecond> second)
        {
            if (first == null)
                throw new ArgumentNullException(nameof(first));
            if (second == null)
                throw new ArgumentNullException(nameof(second));

            return first.Provider.CreateQuery<ValueTuple<TFirst, TSecond>>(
                Expression.Call(
                    null,
                    ((MethodInfo)MethodInfo.GetCurrentMethod()!).MakeGenericMethod(typeof(TFirst), typeof(TSecond)),
                    first.Expression,
                    GetSourceExpression(second)
                )
            );
        }

        /// <summary>
        /// Merges the specified observable sequences into one observable sequence of tuple values whenever all of the observable sequences have produced an element at a corresponding index.
        /// </summary>
        /// <typeparam name="TFirst">The type of the elements in the first source sequence.</typeparam>
        /// <typeparam name="TSecond">The type of the elements in the second source sequence.</typeparam>
        /// <typeparam name="TThird">The type of the elements in the third source sequence.</typeparam>
        /// <param name="first">First observable source.</param>
        /// <param name="second">Second observable source.</param>
        /// <param name="third">Third observable source.</param>
        /// <returns>An observable sequence containing the result of combining elements of the sources using tuple values.</returns>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="first" /> or <paramref name="second" /> or <paramref name="third" /> is null.</exception>
        public static IQbservable<ValueTuple<TFirst, TSecond, TThird>> Zip<TFirst, TSecond, TThird>(this IQbservable<TFirst> first, IObservable<TSecond> second, IObservable<TThird> third)
        {
            if (first == null)
                throw new ArgumentNullException(nameof(first));
            if (second == null)
                throw new ArgumentNullException(nameof(second));
            if (third == null)
                throw new ArgumentNullException(nameof(third));

            return first.Provider.CreateQuery<ValueTuple<TFirst, TSecond, TThird>>(
                Expression.Call(
                    null,
                    ((MethodInfo)MethodInfo.GetCurrentMethod()!).MakeGenericMethod(typeof(TFirst), typeof(TSecond), typeof(TThird)),
                    first.Expression,
                    GetSourceExpression(second),
                    GetSourceExpression(third)
                )
            );
        }

        /// <summary>
        /// Merges the specified observable sequences into one observable sequence of tuple values whenever all of the observable sequences have produced an element at a corresponding index.
        /// </summary>
        /// <typeparam name="TFirst">The type of the elements in the first source sequence.</typeparam>
        /// <typeparam name="TSecond">The type of the elements in the second source sequence.</typeparam>
        /// <typeparam name="TThird">The type of the elements in the third source sequence.</typeparam>
        /// <typeparam name="TFourth">The type of the elements in the fourth source sequence.</typeparam>
        /// <param name="first">First observable source.</param>
        /// <param name="second">Second observable source.</param>
        /// <param name="third">Third observable source.</param>
        /// <param name="fourth">Fourth observable source.</param>
        /// <returns>An observable sequence containing the result of combining elements of the sources using tuple values.</returns>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="first" /> or <paramref name="second" /> or <paramref name="third" /> or <paramref name="fourth" /> is null.</exception>
        public static IQbservable<ValueTuple<TFirst, TSecond, TThird, TFourth>> Zip<TFirst, TSecond, TThird, TFourth>(this IQbservable<TFirst> first, IObservable<TSecond> second, IObservable<TThird> third, IObservable<TFourth> fourth)
        {
            if (first == null)
                throw new ArgumentNullException(nameof(first));
            if (second == null)
                throw new ArgumentNullException(nameof(second));
            if (third == null)
                throw new ArgumentNullException(nameof(third));
            if (fourth == null)
                throw new ArgumentNullException(nameof(fourth));

            return first.Provider.CreateQuery<ValueTuple<TFirst, TSecond, TThird, TFourth>>(
                Expression.Call(
                    null,
                    ((MethodInfo)MethodInfo.GetCurrentMethod()!).MakeGenericMethod(typeof(TFirst), typeof(TSecond), typeof(TThird), typeof(TFourth)),
                    first.Expression,
                    GetSourceExpression(second),
                    GetSourceExpression(third),
                    GetSourceExpression(fourth)
                )
            );
        }

        /// <summary>
        /// Merges the specified observable sequences into one observable sequence of tuple values whenever all of the observable sequences have produced an element at a corresponding index.
        /// </summary>
        /// <typeparam name="TFirst">The type of the elements in the first source sequence.</typeparam>
        /// <typeparam name="TSecond">The type of the elements in the second source sequence.</typeparam>
        /// <typeparam name="TThird">The type of the elements in the third source sequence.</typeparam>
        /// <typeparam name="TFourth">The type of the elements in the fourth source sequence.</typeparam>
        /// <typeparam name="TFifth">The type of the elements in the fifth source sequence.</typeparam>
        /// <param name="first">First observable source.</param>
        /// <param name="second">Second observable source.</param>
        /// <param name="third">Third observable source.</param>
        /// <param name="fourth">Fourth observable source.</param>
        /// <param name="fifth">Fifth observable source.</param>
        /// <returns>An observable sequence containing the result of combining elements of the sources using tuple values.</returns>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="first" /> or <paramref name="second" /> or <paramref name="third" /> or <paramref name="fourth" /> or <paramref name="fifth" /> is null.</exception>
        public static IQbservable<ValueTuple<TFirst, TSecond, TThird, TFourth, TFifth>> Zip<TFirst, TSecond, TThird, TFourth, TFifth>(this IQbservable<TFirst> first, IObservable<TSecond> second, IObservable<TThird> third, IObservable<TFourth> fourth, IObservable<TFifth> fifth)
        {
            if (first == null)
                throw new ArgumentNullException(nameof(first));
            if (second == null)
                throw new ArgumentNullException(nameof(second));
            if (third == null)
                throw new ArgumentNullException(nameof(third));
            if (fourth == null)
                throw new ArgumentNullException(nameof(fourth));
            if (fifth == null)
                throw new ArgumentNullException(nameof(fifth));

            return first.Provider.CreateQuery<ValueTuple<TFirst, TSecond, TThird, TFourth, TFifth>>(
                Expression.Call(
                    null,
                    ((MethodInfo)MethodInfo.GetCurrentMethod()!).MakeGenericMethod(typeof(TFirst), typeof(TSecond), typeof(TThird), typeof(TFourth), typeof(TFifth)),
                    first.Expression,
                    GetSourceExpression(second),
                    GetSourceExpression(third),
                    GetSourceExpression(fourth),
                    GetSourceExpression(fifth)
                )
            );
        }

        /// <summary>
        /// Merges the specified observable sequences into one observable sequence of tuple values whenever all of the observable sequences have produced an element at a corresponding index.
        /// </summary>
        /// <typeparam name="TFirst">The type of the elements in the first source sequence.</typeparam>
        /// <typeparam name="TSecond">The type of the elements in the second source sequence.</typeparam>
        /// <typeparam name="TThird">The type of the elements in the third source sequence.</typeparam>
        /// <typeparam name="TFourth">The type of the elements in the fourth source sequence.</typeparam>
        /// <typeparam name="TFifth">The type of the elements in the fifth source sequence.</typeparam>
        /// <typeparam name="TSixth">The type of the elements in the sixth source sequence.</typeparam>
        /// <param name="first">First observable source.</param>
        /// <param name="second">Second observable source.</param>
        /// <param name="third">Third observable source.</param>
        /// <param name="fourth">Fourth observable source.</param>
        /// <param name="fifth">Fifth observable source.</param>
        /// <param name="sixth">Sixth observable source.</param>
        /// <returns>An observable sequence containing the result of combining elements of the sources using tuple values.</returns>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="first" /> or <paramref name="second" /> or <paramref name="third" /> or <paramref name="fourth" /> or <paramref name="fifth" /> or <paramref name="sixth" /> is null.</exception>
        public static IQbservable<ValueTuple<TFirst, TSecond, TThird, TFourth, TFifth, TSixth>> Zip<TFirst, TSecond, TThird, TFourth, TFifth, TSixth>(this IQbservable<TFirst> first, IObservable<TSecond> second, IObservable<TThird> third, IObservable<TFourth> fourth, IObservable<TFifth> fifth, IObservable<TSixth> sixth)
        {
            if (first == null)
                throw new ArgumentNullException(nameof(first));
            if (second == null)
                throw new ArgumentNullException(nameof(second));
            if (third == null)
                throw new ArgumentNullException(nameof(third));
            if (fourth == null)
                throw new ArgumentNullException(nameof(fourth));
            if (fifth == null)
                throw new ArgumentNullException(nameof(fifth));
            if (sixth == null)
                throw new ArgumentNullException(nameof(sixth));

            return first.Provider.CreateQuery<ValueTuple<TFirst, TSecond, TThird, TFourth, TFifth, TSixth>>(
                Expression.Call(
                    null,
                    ((MethodInfo)MethodInfo.GetCurrentMethod()!).MakeGenericMethod(typeof(TFirst), typeof(TSecond), typeof(TThird), typeof(TFourth), typeof(TFifth), typeof(TSixth)),
                    first.Expression,
                    GetSourceExpression(second),
                    GetSourceExpression(third),
                    GetSourceExpression(fourth),
                    GetSourceExpression(fifth),
                    GetSourceExpression(sixth)
                )
            );
        }

        /// <summary>
        /// Merges the specified observable sequences into one observable sequence of tuple values whenever all of the observable sequences have produced an element at a corresponding index.
        /// </summary>
        /// <typeparam name="TFirst">The type of the elements in the first source sequence.</typeparam>
        /// <typeparam name="TSecond">The type of the elements in the second source sequence.</typeparam>
        /// <typeparam name="TThird">The type of the elements in the third source sequence.</typeparam>
        /// <typeparam name="TFourth">The type of the elements in the fourth source sequence.</typeparam>
        /// <typeparam name="TFifth">The type of the elements in the fifth source sequence.</typeparam>
        /// <typeparam name="TSixth">The type of the elements in the sixth source sequence.</typeparam>
        /// <typeparam name="TSeventh">The type of the elements in the seventh source sequence.</typeparam>
        /// <param name="first">First observable source.</param>
        /// <param name="second">Second observable source.</param>
        /// <param name="third">Third observable source.</param>
        /// <param name="fourth">Fourth observable source.</param>
        /// <param name="fifth">Fifth observable source.</param>
        /// <param name="sixth">Sixth observable source.</param>
        /// <param name="seventh">Seventh observable source.</param>
        /// <returns>An observable sequence containing the result of combining elements of the sources using tuple values.</returns>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="first" /> or <paramref name="second" /> or <paramref name="third" /> or <paramref name="fourth" /> or <paramref name="fifth" /> or <paramref name="sixth" /> or <paramref name="seventh" /> is null.</exception>
        public static IQbservable<ValueTuple<TFirst, TSecond, TThird, TFourth, TFifth, TSixth, TSeventh>> Zip<TFirst, TSecond, TThird, TFourth, TFifth, TSixth, TSeventh>(this IQbservable<TFirst> first, IObservable<TSecond> second, IObservable<TThird> third, IObservable<TFourth> fourth, IObservable<TFifth> fifth, IObservable<TSixth> sixth, IObservable<TSeventh> seventh)
        {
            if (first == null)
                throw new ArgumentNullException(nameof(first));
            if (second == null)
                throw new ArgumentNullException(nameof(second));
            if (third == null)
                throw new ArgumentNullException(nameof(third));
            if (fourth == null)
                throw new ArgumentNullException(nameof(fourth));
            if (fifth == null)
                throw new ArgumentNullException(nameof(fifth));
            if (sixth == null)
                throw new ArgumentNullException(nameof(sixth));
            if (seventh == null)
                throw new ArgumentNullException(nameof(seventh));

            return first.Provider.CreateQuery<ValueTuple<TFirst, TSecond, TThird, TFourth, TFifth, TSixth, TSeventh>>(
                Expression.Call(
                    null,
                    ((MethodInfo)MethodInfo.GetCurrentMethod()!).MakeGenericMethod(typeof(TFirst), typeof(TSecond), typeof(TThird), typeof(TFourth), typeof(TFifth), typeof(TSixth), typeof(TSeventh)),
                    first.Expression,
                    GetSourceExpression(second),
                    GetSourceExpression(third),
                    GetSourceExpression(fourth),
                    GetSourceExpression(fifth),
                    GetSourceExpression(sixth),
                    GetSourceExpression(seventh)
                )
            );
        }

        /// <summary>
        /// Merges the specified observable sequences into one observable sequence of tuple values whenever all of the observable sequences have produced an element at a corresponding index.
        /// </summary>
        /// <typeparam name="TFirst">The type of the elements in the first source sequence.</typeparam>
        /// <typeparam name="TSecond">The type of the elements in the second source sequence.</typeparam>
        /// <typeparam name="TThird">The type of the elements in the third source sequence.</typeparam>
        /// <typeparam name="TFourth">The type of the elements in the fourth source sequence.</typeparam>
        /// <typeparam name="TFifth">The type of the elements in the fifth source sequence.</typeparam>
        /// <typeparam name="TSixth">The type of the elements in the sixth source sequence.</typeparam>
        /// <typeparam name="TSeventh">The type of the elements in the seventh source sequence.</typeparam>
        /// <typeparam name="TEighth">The type of the elements in the eighth source sequence.</typeparam>
        /// <param name="first">First observable source.</param>
        /// <param name="second">Second observable source.</param>
        /// <param name="third">Third observable source.</param>
        /// <param name="fourth">Fourth observable source.</param>
        /// <param name="fifth">Fifth observable source.</param>
        /// <param name="sixth">Sixth observable source.</param>
        /// <param name="seventh">Seventh observable source.</param>
        /// <param name="eighth">Eighth observable source.</param>
        /// <returns>An observable sequence containing the result of combining elements of the sources using tuple values.</returns>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="first" /> or <paramref name="second" /> or <paramref name="third" /> or <paramref name="fourth" /> or <paramref name="fifth" /> or <paramref name="sixth" /> or <paramref name="seventh" /> or <paramref name="eighth" /> is null.</exception>
        public static IQbservable<ValueTuple<TFirst, TSecond, TThird, TFourth, TFifth, TSixth, TSeventh, ValueTuple<TEighth>>> Zip<TFirst, TSecond, TThird, TFourth, TFifth, TSixth, TSeventh, TEighth>(this IQbservable<TFirst> first, IObservable<TSecond> second, IObservable<TThird> third, IObservable<TFourth> fourth, IObservable<TFifth> fifth, IObservable<TSixth> sixth, IObservable<TSeventh> seventh, IObservable<TEighth> eighth)
        {
            if (first == null)
                throw new ArgumentNullException(nameof(first));
            if (second == null)
                throw new ArgumentNullException(nameof(second));
            if (third == null)
                throw new ArgumentNullException(nameof(third));
            if (fourth == null)
                throw new ArgumentNullException(nameof(fourth));
            if (fifth == null)
                throw new ArgumentNullException(nameof(fifth));
            if (sixth == null)
                throw new ArgumentNullException(nameof(sixth));
            if (seventh == null)
                throw new ArgumentNullException(nameof(seventh));
            if (eighth == null)
                throw new ArgumentNullException(nameof(eighth));

            return first.Provider.CreateQuery<ValueTuple<TFirst, TSecond, TThird, TFourth, TFifth, TSixth, TSeventh, ValueTuple<TEighth>>>(
                Expression.Call(
                    null,
                    ((MethodInfo)MethodInfo.GetCurrentMethod()!).MakeGenericMethod(typeof(TFirst), typeof(TSecond), typeof(TThird), typeof(TFourth), typeof(TFifth), typeof(TSixth), typeof(TSeventh), typeof(TEighth)),
                    first.Expression,
                    GetSourceExpression(second),
                    GetSourceExpression(third),
                    GetSourceExpression(fourth),
                    GetSourceExpression(fifth),
                    GetSourceExpression(sixth),
                    GetSourceExpression(seventh),
                    GetSourceExpression(eighth)
                )
            );
        }

    }
}

#pragma warning restore 1591


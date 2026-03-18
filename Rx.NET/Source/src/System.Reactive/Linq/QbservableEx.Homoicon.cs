/*
 * WARNING: Auto-generated file (2026/03/18 13:52:29)
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
        public static IQbservable<(TFirst First, TSecond Second)> CombineLatest<TFirst, TSecond>(this IQbservable<TFirst> first, IObservable<TSecond> second)
        {
            if (first == null)
                throw new ArgumentNullException(nameof(first));
            if (second == null)
                throw new ArgumentNullException(nameof(second));

            return first.Provider.CreateQuery<(TFirst First, TSecond Second)>(
                Expression.Call(
                    null,
                    new Func<IQbservable<TFirst>, IObservable<TSecond>, IQbservable<(TFirst First, TSecond Second)>>(CombineLatest<TFirst, TSecond>).Method,
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
        public static IQbservable<(TFirst First, TSecond Second, TThird Third)> CombineLatest<TFirst, TSecond, TThird>(this IQbservable<TFirst> first, IObservable<TSecond> second, IObservable<TThird> third)
        {
            if (first == null)
                throw new ArgumentNullException(nameof(first));
            if (second == null)
                throw new ArgumentNullException(nameof(second));
            if (third == null)
                throw new ArgumentNullException(nameof(third));

            return first.Provider.CreateQuery<(TFirst First, TSecond Second, TThird Third)>(
                Expression.Call(
                    null,
                    new Func<IQbservable<TFirst>, IObservable<TSecond>, IObservable<TThird>, IQbservable<(TFirst First, TSecond Second, TThird Third)>>(CombineLatest<TFirst, TSecond, TThird>).Method,
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
        public static IQbservable<(TFirst First, TSecond Second, TThird Third, TFourth Fourth)> CombineLatest<TFirst, TSecond, TThird, TFourth>(this IQbservable<TFirst> first, IObservable<TSecond> second, IObservable<TThird> third, IObservable<TFourth> fourth)
        {
            if (first == null)
                throw new ArgumentNullException(nameof(first));
            if (second == null)
                throw new ArgumentNullException(nameof(second));
            if (third == null)
                throw new ArgumentNullException(nameof(third));
            if (fourth == null)
                throw new ArgumentNullException(nameof(fourth));

            return first.Provider.CreateQuery<(TFirst First, TSecond Second, TThird Third, TFourth Fourth)>(
                Expression.Call(
                    null,
                    new Func<IQbservable<TFirst>, IObservable<TSecond>, IObservable<TThird>, IObservable<TFourth>, IQbservable<(TFirst First, TSecond Second, TThird Third, TFourth Fourth)>>(CombineLatest<TFirst, TSecond, TThird, TFourth>).Method,
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
        public static IQbservable<(TFirst First, TSecond Second, TThird Third, TFourth Fourth, TFifth Fifth)> CombineLatest<TFirst, TSecond, TThird, TFourth, TFifth>(this IQbservable<TFirst> first, IObservable<TSecond> second, IObservable<TThird> third, IObservable<TFourth> fourth, IObservable<TFifth> fifth)
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

            return first.Provider.CreateQuery<(TFirst First, TSecond Second, TThird Third, TFourth Fourth, TFifth Fifth)>(
                Expression.Call(
                    null,
                    new Func<IQbservable<TFirst>, IObservable<TSecond>, IObservable<TThird>, IObservable<TFourth>, IObservable<TFifth>, IQbservable<(TFirst First, TSecond Second, TThird Third, TFourth Fourth, TFifth Fifth)>>(CombineLatest<TFirst, TSecond, TThird, TFourth, TFifth>).Method,
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
        public static IQbservable<(TFirst First, TSecond Second, TThird Third, TFourth Fourth, TFifth Fifth, TSixth Sixth)> CombineLatest<TFirst, TSecond, TThird, TFourth, TFifth, TSixth>(this IQbservable<TFirst> first, IObservable<TSecond> second, IObservable<TThird> third, IObservable<TFourth> fourth, IObservable<TFifth> fifth, IObservable<TSixth> sixth)
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

            return first.Provider.CreateQuery<(TFirst First, TSecond Second, TThird Third, TFourth Fourth, TFifth Fifth, TSixth Sixth)>(
                Expression.Call(
                    null,
                    new Func<IQbservable<TFirst>, IObservable<TSecond>, IObservable<TThird>, IObservable<TFourth>, IObservable<TFifth>, IObservable<TSixth>, IQbservable<(TFirst First, TSecond Second, TThird Third, TFourth Fourth, TFifth Fifth, TSixth Sixth)>>(CombineLatest<TFirst, TSecond, TThird, TFourth, TFifth, TSixth>).Method,
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
        public static IQbservable<(TFirst First, TSecond Second, TThird Third, TFourth Fourth, TFifth Fifth, TSixth Sixth, TSeventh Seventh)> CombineLatest<TFirst, TSecond, TThird, TFourth, TFifth, TSixth, TSeventh>(this IQbservable<TFirst> first, IObservable<TSecond> second, IObservable<TThird> third, IObservable<TFourth> fourth, IObservable<TFifth> fifth, IObservable<TSixth> sixth, IObservable<TSeventh> seventh)
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

            return first.Provider.CreateQuery<(TFirst First, TSecond Second, TThird Third, TFourth Fourth, TFifth Fifth, TSixth Sixth, TSeventh Seventh)>(
                Expression.Call(
                    null,
                    new Func<IQbservable<TFirst>, IObservable<TSecond>, IObservable<TThird>, IObservable<TFourth>, IObservable<TFifth>, IObservable<TSixth>, IObservable<TSeventh>, IQbservable<(TFirst First, TSecond Second, TThird Third, TFourth Fourth, TFifth Fifth, TSixth Sixth, TSeventh Seventh)>>(CombineLatest<TFirst, TSecond, TThird, TFourth, TFifth, TSixth, TSeventh>).Method,
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
        public static IQbservable<(TFirst First, TSecond Second, TThird Third, TFourth Fourth, TFifth Fifth, TSixth Sixth, TSeventh Seventh, TEighth Eighth)> CombineLatest<TFirst, TSecond, TThird, TFourth, TFifth, TSixth, TSeventh, TEighth>(this IQbservable<TFirst> first, IObservable<TSecond> second, IObservable<TThird> third, IObservable<TFourth> fourth, IObservable<TFifth> fifth, IObservable<TSixth> sixth, IObservable<TSeventh> seventh, IObservable<TEighth> eighth)
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

            return first.Provider.CreateQuery<(TFirst First, TSecond Second, TThird Third, TFourth Fourth, TFifth Fifth, TSixth Sixth, TSeventh Seventh, TEighth Eighth)>(
                Expression.Call(
                    null,
                    new Func<IQbservable<TFirst>, IObservable<TSecond>, IObservable<TThird>, IObservable<TFourth>, IObservable<TFifth>, IObservable<TSixth>, IObservable<TSeventh>, IObservable<TEighth>, IQbservable<(TFirst First, TSecond Second, TThird Third, TFourth Fourth, TFifth Fifth, TSixth Sixth, TSeventh Seventh, TEighth Eighth)>>(CombineLatest<TFirst, TSecond, TThird, TFourth, TFifth, TSixth, TSeventh, TEighth>).Method,
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
        /// <typeparam name="TNinth">The type of the elements in the ninth source sequence.</typeparam>
        /// <param name="first">First observable source.</param>
        /// <param name="second">Second observable source.</param>
        /// <param name="third">Third observable source.</param>
        /// <param name="fourth">Fourth observable source.</param>
        /// <param name="fifth">Fifth observable source.</param>
        /// <param name="sixth">Sixth observable source.</param>
        /// <param name="seventh">Seventh observable source.</param>
        /// <param name="eighth">Eighth observable source.</param>
        /// <param name="ninth">Ninth observable source.</param>
        /// <returns>An observable sequence containing the result of combining elements of the sources using tuple values.</returns>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="first" /> or <paramref name="second" /> or <paramref name="third" /> or <paramref name="fourth" /> or <paramref name="fifth" /> or <paramref name="sixth" /> or <paramref name="seventh" /> or <paramref name="eighth" /> or <paramref name="ninth" /> is null.</exception>
        public static IQbservable<(TFirst First, TSecond Second, TThird Third, TFourth Fourth, TFifth Fifth, TSixth Sixth, TSeventh Seventh, TEighth Eighth, TNinth Ninth)> CombineLatest<TFirst, TSecond, TThird, TFourth, TFifth, TSixth, TSeventh, TEighth, TNinth>(this IQbservable<TFirst> first, IObservable<TSecond> second, IObservable<TThird> third, IObservable<TFourth> fourth, IObservable<TFifth> fifth, IObservable<TSixth> sixth, IObservable<TSeventh> seventh, IObservable<TEighth> eighth, IObservable<TNinth> ninth)
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
            if (ninth == null)
                throw new ArgumentNullException(nameof(ninth));

            return first.Provider.CreateQuery<(TFirst First, TSecond Second, TThird Third, TFourth Fourth, TFifth Fifth, TSixth Sixth, TSeventh Seventh, TEighth Eighth, TNinth Ninth)>(
                Expression.Call(
                    null,
                    new Func<IQbservable<TFirst>, IObservable<TSecond>, IObservable<TThird>, IObservable<TFourth>, IObservable<TFifth>, IObservable<TSixth>, IObservable<TSeventh>, IObservable<TEighth>, IObservable<TNinth>, IQbservable<(TFirst First, TSecond Second, TThird Third, TFourth Fourth, TFifth Fifth, TSixth Sixth, TSeventh Seventh, TEighth Eighth, TNinth Ninth)>>(CombineLatest<TFirst, TSecond, TThird, TFourth, TFifth, TSixth, TSeventh, TEighth, TNinth>).Method,
                    first.Expression,
                    GetSourceExpression(second),
                    GetSourceExpression(third),
                    GetSourceExpression(fourth),
                    GetSourceExpression(fifth),
                    GetSourceExpression(sixth),
                    GetSourceExpression(seventh),
                    GetSourceExpression(eighth),
                    GetSourceExpression(ninth)
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
        /// <typeparam name="TNinth">The type of the elements in the ninth source sequence.</typeparam>
        /// <typeparam name="TTenth">The type of the elements in the tenth source sequence.</typeparam>
        /// <param name="first">First observable source.</param>
        /// <param name="second">Second observable source.</param>
        /// <param name="third">Third observable source.</param>
        /// <param name="fourth">Fourth observable source.</param>
        /// <param name="fifth">Fifth observable source.</param>
        /// <param name="sixth">Sixth observable source.</param>
        /// <param name="seventh">Seventh observable source.</param>
        /// <param name="eighth">Eighth observable source.</param>
        /// <param name="ninth">Ninth observable source.</param>
        /// <param name="tenth">Tenth observable source.</param>
        /// <returns>An observable sequence containing the result of combining elements of the sources using tuple values.</returns>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="first" /> or <paramref name="second" /> or <paramref name="third" /> or <paramref name="fourth" /> or <paramref name="fifth" /> or <paramref name="sixth" /> or <paramref name="seventh" /> or <paramref name="eighth" /> or <paramref name="ninth" /> or <paramref name="tenth" /> is null.</exception>
        public static IQbservable<(TFirst First, TSecond Second, TThird Third, TFourth Fourth, TFifth Fifth, TSixth Sixth, TSeventh Seventh, TEighth Eighth, TNinth Ninth, TTenth Tenth)> CombineLatest<TFirst, TSecond, TThird, TFourth, TFifth, TSixth, TSeventh, TEighth, TNinth, TTenth>(this IQbservable<TFirst> first, IObservable<TSecond> second, IObservable<TThird> third, IObservable<TFourth> fourth, IObservable<TFifth> fifth, IObservable<TSixth> sixth, IObservable<TSeventh> seventh, IObservable<TEighth> eighth, IObservable<TNinth> ninth, IObservable<TTenth> tenth)
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
            if (ninth == null)
                throw new ArgumentNullException(nameof(ninth));
            if (tenth == null)
                throw new ArgumentNullException(nameof(tenth));

            return first.Provider.CreateQuery<(TFirst First, TSecond Second, TThird Third, TFourth Fourth, TFifth Fifth, TSixth Sixth, TSeventh Seventh, TEighth Eighth, TNinth Ninth, TTenth Tenth)>(
                Expression.Call(
                    null,
                    new Func<IQbservable<TFirst>, IObservable<TSecond>, IObservable<TThird>, IObservable<TFourth>, IObservable<TFifth>, IObservable<TSixth>, IObservable<TSeventh>, IObservable<TEighth>, IObservable<TNinth>, IObservable<TTenth>, IQbservable<(TFirst First, TSecond Second, TThird Third, TFourth Fourth, TFifth Fifth, TSixth Sixth, TSeventh Seventh, TEighth Eighth, TNinth Ninth, TTenth Tenth)>>(CombineLatest<TFirst, TSecond, TThird, TFourth, TFifth, TSixth, TSeventh, TEighth, TNinth, TTenth>).Method,
                    first.Expression,
                    GetSourceExpression(second),
                    GetSourceExpression(third),
                    GetSourceExpression(fourth),
                    GetSourceExpression(fifth),
                    GetSourceExpression(sixth),
                    GetSourceExpression(seventh),
                    GetSourceExpression(eighth),
                    GetSourceExpression(ninth),
                    GetSourceExpression(tenth)
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
        /// <typeparam name="TNinth">The type of the elements in the ninth source sequence.</typeparam>
        /// <typeparam name="TTenth">The type of the elements in the tenth source sequence.</typeparam>
        /// <typeparam name="TEleventh">The type of the elements in the eleventh source sequence.</typeparam>
        /// <param name="first">First observable source.</param>
        /// <param name="second">Second observable source.</param>
        /// <param name="third">Third observable source.</param>
        /// <param name="fourth">Fourth observable source.</param>
        /// <param name="fifth">Fifth observable source.</param>
        /// <param name="sixth">Sixth observable source.</param>
        /// <param name="seventh">Seventh observable source.</param>
        /// <param name="eighth">Eighth observable source.</param>
        /// <param name="ninth">Ninth observable source.</param>
        /// <param name="tenth">Tenth observable source.</param>
        /// <param name="eleventh">Eleventh observable source.</param>
        /// <returns>An observable sequence containing the result of combining elements of the sources using tuple values.</returns>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="first" /> or <paramref name="second" /> or <paramref name="third" /> or <paramref name="fourth" /> or <paramref name="fifth" /> or <paramref name="sixth" /> or <paramref name="seventh" /> or <paramref name="eighth" /> or <paramref name="ninth" /> or <paramref name="tenth" /> or <paramref name="eleventh" /> is null.</exception>
        public static IQbservable<(TFirst First, TSecond Second, TThird Third, TFourth Fourth, TFifth Fifth, TSixth Sixth, TSeventh Seventh, TEighth Eighth, TNinth Ninth, TTenth Tenth, TEleventh Eleventh)> CombineLatest<TFirst, TSecond, TThird, TFourth, TFifth, TSixth, TSeventh, TEighth, TNinth, TTenth, TEleventh>(this IQbservable<TFirst> first, IObservable<TSecond> second, IObservable<TThird> third, IObservable<TFourth> fourth, IObservable<TFifth> fifth, IObservable<TSixth> sixth, IObservable<TSeventh> seventh, IObservable<TEighth> eighth, IObservable<TNinth> ninth, IObservable<TTenth> tenth, IObservable<TEleventh> eleventh)
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
            if (ninth == null)
                throw new ArgumentNullException(nameof(ninth));
            if (tenth == null)
                throw new ArgumentNullException(nameof(tenth));
            if (eleventh == null)
                throw new ArgumentNullException(nameof(eleventh));

            return first.Provider.CreateQuery<(TFirst First, TSecond Second, TThird Third, TFourth Fourth, TFifth Fifth, TSixth Sixth, TSeventh Seventh, TEighth Eighth, TNinth Ninth, TTenth Tenth, TEleventh Eleventh)>(
                Expression.Call(
                    null,
                    new Func<IQbservable<TFirst>, IObservable<TSecond>, IObservable<TThird>, IObservable<TFourth>, IObservable<TFifth>, IObservable<TSixth>, IObservable<TSeventh>, IObservable<TEighth>, IObservable<TNinth>, IObservable<TTenth>, IObservable<TEleventh>, IQbservable<(TFirst First, TSecond Second, TThird Third, TFourth Fourth, TFifth Fifth, TSixth Sixth, TSeventh Seventh, TEighth Eighth, TNinth Ninth, TTenth Tenth, TEleventh Eleventh)>>(CombineLatest<TFirst, TSecond, TThird, TFourth, TFifth, TSixth, TSeventh, TEighth, TNinth, TTenth, TEleventh>).Method,
                    first.Expression,
                    GetSourceExpression(second),
                    GetSourceExpression(third),
                    GetSourceExpression(fourth),
                    GetSourceExpression(fifth),
                    GetSourceExpression(sixth),
                    GetSourceExpression(seventh),
                    GetSourceExpression(eighth),
                    GetSourceExpression(ninth),
                    GetSourceExpression(tenth),
                    GetSourceExpression(eleventh)
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
        /// <typeparam name="TNinth">The type of the elements in the ninth source sequence.</typeparam>
        /// <typeparam name="TTenth">The type of the elements in the tenth source sequence.</typeparam>
        /// <typeparam name="TEleventh">The type of the elements in the eleventh source sequence.</typeparam>
        /// <typeparam name="TTwelfth">The type of the elements in the twelfth source sequence.</typeparam>
        /// <param name="first">First observable source.</param>
        /// <param name="second">Second observable source.</param>
        /// <param name="third">Third observable source.</param>
        /// <param name="fourth">Fourth observable source.</param>
        /// <param name="fifth">Fifth observable source.</param>
        /// <param name="sixth">Sixth observable source.</param>
        /// <param name="seventh">Seventh observable source.</param>
        /// <param name="eighth">Eighth observable source.</param>
        /// <param name="ninth">Ninth observable source.</param>
        /// <param name="tenth">Tenth observable source.</param>
        /// <param name="eleventh">Eleventh observable source.</param>
        /// <param name="twelfth">Twelfth observable source.</param>
        /// <returns>An observable sequence containing the result of combining elements of the sources using tuple values.</returns>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="first" /> or <paramref name="second" /> or <paramref name="third" /> or <paramref name="fourth" /> or <paramref name="fifth" /> or <paramref name="sixth" /> or <paramref name="seventh" /> or <paramref name="eighth" /> or <paramref name="ninth" /> or <paramref name="tenth" /> or <paramref name="eleventh" /> or <paramref name="twelfth" /> is null.</exception>
        public static IQbservable<(TFirst First, TSecond Second, TThird Third, TFourth Fourth, TFifth Fifth, TSixth Sixth, TSeventh Seventh, TEighth Eighth, TNinth Ninth, TTenth Tenth, TEleventh Eleventh, TTwelfth Twelfth)> CombineLatest<TFirst, TSecond, TThird, TFourth, TFifth, TSixth, TSeventh, TEighth, TNinth, TTenth, TEleventh, TTwelfth>(this IQbservable<TFirst> first, IObservable<TSecond> second, IObservable<TThird> third, IObservable<TFourth> fourth, IObservable<TFifth> fifth, IObservable<TSixth> sixth, IObservable<TSeventh> seventh, IObservable<TEighth> eighth, IObservable<TNinth> ninth, IObservable<TTenth> tenth, IObservable<TEleventh> eleventh, IObservable<TTwelfth> twelfth)
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
            if (ninth == null)
                throw new ArgumentNullException(nameof(ninth));
            if (tenth == null)
                throw new ArgumentNullException(nameof(tenth));
            if (eleventh == null)
                throw new ArgumentNullException(nameof(eleventh));
            if (twelfth == null)
                throw new ArgumentNullException(nameof(twelfth));

            return first.Provider.CreateQuery<(TFirst First, TSecond Second, TThird Third, TFourth Fourth, TFifth Fifth, TSixth Sixth, TSeventh Seventh, TEighth Eighth, TNinth Ninth, TTenth Tenth, TEleventh Eleventh, TTwelfth Twelfth)>(
                Expression.Call(
                    null,
                    new Func<IQbservable<TFirst>, IObservable<TSecond>, IObservable<TThird>, IObservable<TFourth>, IObservable<TFifth>, IObservable<TSixth>, IObservable<TSeventh>, IObservable<TEighth>, IObservable<TNinth>, IObservable<TTenth>, IObservable<TEleventh>, IObservable<TTwelfth>, IQbservable<(TFirst First, TSecond Second, TThird Third, TFourth Fourth, TFifth Fifth, TSixth Sixth, TSeventh Seventh, TEighth Eighth, TNinth Ninth, TTenth Tenth, TEleventh Eleventh, TTwelfth Twelfth)>>(CombineLatest<TFirst, TSecond, TThird, TFourth, TFifth, TSixth, TSeventh, TEighth, TNinth, TTenth, TEleventh, TTwelfth>).Method,
                    first.Expression,
                    GetSourceExpression(second),
                    GetSourceExpression(third),
                    GetSourceExpression(fourth),
                    GetSourceExpression(fifth),
                    GetSourceExpression(sixth),
                    GetSourceExpression(seventh),
                    GetSourceExpression(eighth),
                    GetSourceExpression(ninth),
                    GetSourceExpression(tenth),
                    GetSourceExpression(eleventh),
                    GetSourceExpression(twelfth)
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
        /// <typeparam name="TNinth">The type of the elements in the ninth source sequence.</typeparam>
        /// <typeparam name="TTenth">The type of the elements in the tenth source sequence.</typeparam>
        /// <typeparam name="TEleventh">The type of the elements in the eleventh source sequence.</typeparam>
        /// <typeparam name="TTwelfth">The type of the elements in the twelfth source sequence.</typeparam>
        /// <typeparam name="TThirteenth">The type of the elements in the thirteenth source sequence.</typeparam>
        /// <param name="first">First observable source.</param>
        /// <param name="second">Second observable source.</param>
        /// <param name="third">Third observable source.</param>
        /// <param name="fourth">Fourth observable source.</param>
        /// <param name="fifth">Fifth observable source.</param>
        /// <param name="sixth">Sixth observable source.</param>
        /// <param name="seventh">Seventh observable source.</param>
        /// <param name="eighth">Eighth observable source.</param>
        /// <param name="ninth">Ninth observable source.</param>
        /// <param name="tenth">Tenth observable source.</param>
        /// <param name="eleventh">Eleventh observable source.</param>
        /// <param name="twelfth">Twelfth observable source.</param>
        /// <param name="thirteenth">Thirteenth observable source.</param>
        /// <returns>An observable sequence containing the result of combining elements of the sources using tuple values.</returns>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="first" /> or <paramref name="second" /> or <paramref name="third" /> or <paramref name="fourth" /> or <paramref name="fifth" /> or <paramref name="sixth" /> or <paramref name="seventh" /> or <paramref name="eighth" /> or <paramref name="ninth" /> or <paramref name="tenth" /> or <paramref name="eleventh" /> or <paramref name="twelfth" /> or <paramref name="thirteenth" /> is null.</exception>
        public static IQbservable<(TFirst First, TSecond Second, TThird Third, TFourth Fourth, TFifth Fifth, TSixth Sixth, TSeventh Seventh, TEighth Eighth, TNinth Ninth, TTenth Tenth, TEleventh Eleventh, TTwelfth Twelfth, TThirteenth Thirteenth)> CombineLatest<TFirst, TSecond, TThird, TFourth, TFifth, TSixth, TSeventh, TEighth, TNinth, TTenth, TEleventh, TTwelfth, TThirteenth>(this IQbservable<TFirst> first, IObservable<TSecond> second, IObservable<TThird> third, IObservable<TFourth> fourth, IObservable<TFifth> fifth, IObservable<TSixth> sixth, IObservable<TSeventh> seventh, IObservable<TEighth> eighth, IObservable<TNinth> ninth, IObservable<TTenth> tenth, IObservable<TEleventh> eleventh, IObservable<TTwelfth> twelfth, IObservable<TThirteenth> thirteenth)
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
            if (ninth == null)
                throw new ArgumentNullException(nameof(ninth));
            if (tenth == null)
                throw new ArgumentNullException(nameof(tenth));
            if (eleventh == null)
                throw new ArgumentNullException(nameof(eleventh));
            if (twelfth == null)
                throw new ArgumentNullException(nameof(twelfth));
            if (thirteenth == null)
                throw new ArgumentNullException(nameof(thirteenth));

            return first.Provider.CreateQuery<(TFirst First, TSecond Second, TThird Third, TFourth Fourth, TFifth Fifth, TSixth Sixth, TSeventh Seventh, TEighth Eighth, TNinth Ninth, TTenth Tenth, TEleventh Eleventh, TTwelfth Twelfth, TThirteenth Thirteenth)>(
                Expression.Call(
                    null,
                    new Func<IQbservable<TFirst>, IObservable<TSecond>, IObservable<TThird>, IObservable<TFourth>, IObservable<TFifth>, IObservable<TSixth>, IObservable<TSeventh>, IObservable<TEighth>, IObservable<TNinth>, IObservable<TTenth>, IObservable<TEleventh>, IObservable<TTwelfth>, IObservable<TThirteenth>, IQbservable<(TFirst First, TSecond Second, TThird Third, TFourth Fourth, TFifth Fifth, TSixth Sixth, TSeventh Seventh, TEighth Eighth, TNinth Ninth, TTenth Tenth, TEleventh Eleventh, TTwelfth Twelfth, TThirteenth Thirteenth)>>(CombineLatest<TFirst, TSecond, TThird, TFourth, TFifth, TSixth, TSeventh, TEighth, TNinth, TTenth, TEleventh, TTwelfth, TThirteenth>).Method,
                    first.Expression,
                    GetSourceExpression(second),
                    GetSourceExpression(third),
                    GetSourceExpression(fourth),
                    GetSourceExpression(fifth),
                    GetSourceExpression(sixth),
                    GetSourceExpression(seventh),
                    GetSourceExpression(eighth),
                    GetSourceExpression(ninth),
                    GetSourceExpression(tenth),
                    GetSourceExpression(eleventh),
                    GetSourceExpression(twelfth),
                    GetSourceExpression(thirteenth)
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
        /// <typeparam name="TNinth">The type of the elements in the ninth source sequence.</typeparam>
        /// <typeparam name="TTenth">The type of the elements in the tenth source sequence.</typeparam>
        /// <typeparam name="TEleventh">The type of the elements in the eleventh source sequence.</typeparam>
        /// <typeparam name="TTwelfth">The type of the elements in the twelfth source sequence.</typeparam>
        /// <typeparam name="TThirteenth">The type of the elements in the thirteenth source sequence.</typeparam>
        /// <typeparam name="TFourteenth">The type of the elements in the fourteenth source sequence.</typeparam>
        /// <param name="first">First observable source.</param>
        /// <param name="second">Second observable source.</param>
        /// <param name="third">Third observable source.</param>
        /// <param name="fourth">Fourth observable source.</param>
        /// <param name="fifth">Fifth observable source.</param>
        /// <param name="sixth">Sixth observable source.</param>
        /// <param name="seventh">Seventh observable source.</param>
        /// <param name="eighth">Eighth observable source.</param>
        /// <param name="ninth">Ninth observable source.</param>
        /// <param name="tenth">Tenth observable source.</param>
        /// <param name="eleventh">Eleventh observable source.</param>
        /// <param name="twelfth">Twelfth observable source.</param>
        /// <param name="thirteenth">Thirteenth observable source.</param>
        /// <param name="fourteenth">Fourteenth observable source.</param>
        /// <returns>An observable sequence containing the result of combining elements of the sources using tuple values.</returns>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="first" /> or <paramref name="second" /> or <paramref name="third" /> or <paramref name="fourth" /> or <paramref name="fifth" /> or <paramref name="sixth" /> or <paramref name="seventh" /> or <paramref name="eighth" /> or <paramref name="ninth" /> or <paramref name="tenth" /> or <paramref name="eleventh" /> or <paramref name="twelfth" /> or <paramref name="thirteenth" /> or <paramref name="fourteenth" /> is null.</exception>
        public static IQbservable<(TFirst First, TSecond Second, TThird Third, TFourth Fourth, TFifth Fifth, TSixth Sixth, TSeventh Seventh, TEighth Eighth, TNinth Ninth, TTenth Tenth, TEleventh Eleventh, TTwelfth Twelfth, TThirteenth Thirteenth, TFourteenth Fourteenth)> CombineLatest<TFirst, TSecond, TThird, TFourth, TFifth, TSixth, TSeventh, TEighth, TNinth, TTenth, TEleventh, TTwelfth, TThirteenth, TFourteenth>(this IQbservable<TFirst> first, IObservable<TSecond> second, IObservable<TThird> third, IObservable<TFourth> fourth, IObservable<TFifth> fifth, IObservable<TSixth> sixth, IObservable<TSeventh> seventh, IObservable<TEighth> eighth, IObservable<TNinth> ninth, IObservable<TTenth> tenth, IObservable<TEleventh> eleventh, IObservable<TTwelfth> twelfth, IObservable<TThirteenth> thirteenth, IObservable<TFourteenth> fourteenth)
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
            if (ninth == null)
                throw new ArgumentNullException(nameof(ninth));
            if (tenth == null)
                throw new ArgumentNullException(nameof(tenth));
            if (eleventh == null)
                throw new ArgumentNullException(nameof(eleventh));
            if (twelfth == null)
                throw new ArgumentNullException(nameof(twelfth));
            if (thirteenth == null)
                throw new ArgumentNullException(nameof(thirteenth));
            if (fourteenth == null)
                throw new ArgumentNullException(nameof(fourteenth));

            return first.Provider.CreateQuery<(TFirst First, TSecond Second, TThird Third, TFourth Fourth, TFifth Fifth, TSixth Sixth, TSeventh Seventh, TEighth Eighth, TNinth Ninth, TTenth Tenth, TEleventh Eleventh, TTwelfth Twelfth, TThirteenth Thirteenth, TFourteenth Fourteenth)>(
                Expression.Call(
                    null,
                    new Func<IQbservable<TFirst>, IObservable<TSecond>, IObservable<TThird>, IObservable<TFourth>, IObservable<TFifth>, IObservable<TSixth>, IObservable<TSeventh>, IObservable<TEighth>, IObservable<TNinth>, IObservable<TTenth>, IObservable<TEleventh>, IObservable<TTwelfth>, IObservable<TThirteenth>, IObservable<TFourteenth>, IQbservable<(TFirst First, TSecond Second, TThird Third, TFourth Fourth, TFifth Fifth, TSixth Sixth, TSeventh Seventh, TEighth Eighth, TNinth Ninth, TTenth Tenth, TEleventh Eleventh, TTwelfth Twelfth, TThirteenth Thirteenth, TFourteenth Fourteenth)>>(CombineLatest<TFirst, TSecond, TThird, TFourth, TFifth, TSixth, TSeventh, TEighth, TNinth, TTenth, TEleventh, TTwelfth, TThirteenth, TFourteenth>).Method,
                    first.Expression,
                    GetSourceExpression(second),
                    GetSourceExpression(third),
                    GetSourceExpression(fourth),
                    GetSourceExpression(fifth),
                    GetSourceExpression(sixth),
                    GetSourceExpression(seventh),
                    GetSourceExpression(eighth),
                    GetSourceExpression(ninth),
                    GetSourceExpression(tenth),
                    GetSourceExpression(eleventh),
                    GetSourceExpression(twelfth),
                    GetSourceExpression(thirteenth),
                    GetSourceExpression(fourteenth)
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
        /// <typeparam name="TNinth">The type of the elements in the ninth source sequence.</typeparam>
        /// <typeparam name="TTenth">The type of the elements in the tenth source sequence.</typeparam>
        /// <typeparam name="TEleventh">The type of the elements in the eleventh source sequence.</typeparam>
        /// <typeparam name="TTwelfth">The type of the elements in the twelfth source sequence.</typeparam>
        /// <typeparam name="TThirteenth">The type of the elements in the thirteenth source sequence.</typeparam>
        /// <typeparam name="TFourteenth">The type of the elements in the fourteenth source sequence.</typeparam>
        /// <typeparam name="TFifteenth">The type of the elements in the fifteenth source sequence.</typeparam>
        /// <param name="first">First observable source.</param>
        /// <param name="second">Second observable source.</param>
        /// <param name="third">Third observable source.</param>
        /// <param name="fourth">Fourth observable source.</param>
        /// <param name="fifth">Fifth observable source.</param>
        /// <param name="sixth">Sixth observable source.</param>
        /// <param name="seventh">Seventh observable source.</param>
        /// <param name="eighth">Eighth observable source.</param>
        /// <param name="ninth">Ninth observable source.</param>
        /// <param name="tenth">Tenth observable source.</param>
        /// <param name="eleventh">Eleventh observable source.</param>
        /// <param name="twelfth">Twelfth observable source.</param>
        /// <param name="thirteenth">Thirteenth observable source.</param>
        /// <param name="fourteenth">Fourteenth observable source.</param>
        /// <param name="fifteenth">Fifteenth observable source.</param>
        /// <returns>An observable sequence containing the result of combining elements of the sources using tuple values.</returns>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="first" /> or <paramref name="second" /> or <paramref name="third" /> or <paramref name="fourth" /> or <paramref name="fifth" /> or <paramref name="sixth" /> or <paramref name="seventh" /> or <paramref name="eighth" /> or <paramref name="ninth" /> or <paramref name="tenth" /> or <paramref name="eleventh" /> or <paramref name="twelfth" /> or <paramref name="thirteenth" /> or <paramref name="fourteenth" /> or <paramref name="fifteenth" /> is null.</exception>
        public static IQbservable<(TFirst First, TSecond Second, TThird Third, TFourth Fourth, TFifth Fifth, TSixth Sixth, TSeventh Seventh, TEighth Eighth, TNinth Ninth, TTenth Tenth, TEleventh Eleventh, TTwelfth Twelfth, TThirteenth Thirteenth, TFourteenth Fourteenth, TFifteenth Fifteenth)> CombineLatest<TFirst, TSecond, TThird, TFourth, TFifth, TSixth, TSeventh, TEighth, TNinth, TTenth, TEleventh, TTwelfth, TThirteenth, TFourteenth, TFifteenth>(this IQbservable<TFirst> first, IObservable<TSecond> second, IObservable<TThird> third, IObservable<TFourth> fourth, IObservable<TFifth> fifth, IObservable<TSixth> sixth, IObservable<TSeventh> seventh, IObservable<TEighth> eighth, IObservable<TNinth> ninth, IObservable<TTenth> tenth, IObservable<TEleventh> eleventh, IObservable<TTwelfth> twelfth, IObservable<TThirteenth> thirteenth, IObservable<TFourteenth> fourteenth, IObservable<TFifteenth> fifteenth)
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
            if (ninth == null)
                throw new ArgumentNullException(nameof(ninth));
            if (tenth == null)
                throw new ArgumentNullException(nameof(tenth));
            if (eleventh == null)
                throw new ArgumentNullException(nameof(eleventh));
            if (twelfth == null)
                throw new ArgumentNullException(nameof(twelfth));
            if (thirteenth == null)
                throw new ArgumentNullException(nameof(thirteenth));
            if (fourteenth == null)
                throw new ArgumentNullException(nameof(fourteenth));
            if (fifteenth == null)
                throw new ArgumentNullException(nameof(fifteenth));

            return first.Provider.CreateQuery<(TFirst First, TSecond Second, TThird Third, TFourth Fourth, TFifth Fifth, TSixth Sixth, TSeventh Seventh, TEighth Eighth, TNinth Ninth, TTenth Tenth, TEleventh Eleventh, TTwelfth Twelfth, TThirteenth Thirteenth, TFourteenth Fourteenth, TFifteenth Fifteenth)>(
                Expression.Call(
                    null,
                    new Func<IQbservable<TFirst>, IObservable<TSecond>, IObservable<TThird>, IObservable<TFourth>, IObservable<TFifth>, IObservable<TSixth>, IObservable<TSeventh>, IObservable<TEighth>, IObservable<TNinth>, IObservable<TTenth>, IObservable<TEleventh>, IObservable<TTwelfth>, IObservable<TThirteenth>, IObservable<TFourteenth>, IObservable<TFifteenth>, IQbservable<(TFirst First, TSecond Second, TThird Third, TFourth Fourth, TFifth Fifth, TSixth Sixth, TSeventh Seventh, TEighth Eighth, TNinth Ninth, TTenth Tenth, TEleventh Eleventh, TTwelfth Twelfth, TThirteenth Thirteenth, TFourteenth Fourteenth, TFifteenth Fifteenth)>>(CombineLatest<TFirst, TSecond, TThird, TFourth, TFifth, TSixth, TSeventh, TEighth, TNinth, TTenth, TEleventh, TTwelfth, TThirteenth, TFourteenth, TFifteenth>).Method,
                    first.Expression,
                    GetSourceExpression(second),
                    GetSourceExpression(third),
                    GetSourceExpression(fourth),
                    GetSourceExpression(fifth),
                    GetSourceExpression(sixth),
                    GetSourceExpression(seventh),
                    GetSourceExpression(eighth),
                    GetSourceExpression(ninth),
                    GetSourceExpression(tenth),
                    GetSourceExpression(eleventh),
                    GetSourceExpression(twelfth),
                    GetSourceExpression(thirteenth),
                    GetSourceExpression(fourteenth),
                    GetSourceExpression(fifteenth)
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
        /// <typeparam name="TNinth">The type of the elements in the ninth source sequence.</typeparam>
        /// <typeparam name="TTenth">The type of the elements in the tenth source sequence.</typeparam>
        /// <typeparam name="TEleventh">The type of the elements in the eleventh source sequence.</typeparam>
        /// <typeparam name="TTwelfth">The type of the elements in the twelfth source sequence.</typeparam>
        /// <typeparam name="TThirteenth">The type of the elements in the thirteenth source sequence.</typeparam>
        /// <typeparam name="TFourteenth">The type of the elements in the fourteenth source sequence.</typeparam>
        /// <typeparam name="TFifteenth">The type of the elements in the fifteenth source sequence.</typeparam>
        /// <typeparam name="TSixteenth">The type of the elements in the sixteenth source sequence.</typeparam>
        /// <param name="first">First observable source.</param>
        /// <param name="second">Second observable source.</param>
        /// <param name="third">Third observable source.</param>
        /// <param name="fourth">Fourth observable source.</param>
        /// <param name="fifth">Fifth observable source.</param>
        /// <param name="sixth">Sixth observable source.</param>
        /// <param name="seventh">Seventh observable source.</param>
        /// <param name="eighth">Eighth observable source.</param>
        /// <param name="ninth">Ninth observable source.</param>
        /// <param name="tenth">Tenth observable source.</param>
        /// <param name="eleventh">Eleventh observable source.</param>
        /// <param name="twelfth">Twelfth observable source.</param>
        /// <param name="thirteenth">Thirteenth observable source.</param>
        /// <param name="fourteenth">Fourteenth observable source.</param>
        /// <param name="fifteenth">Fifteenth observable source.</param>
        /// <param name="sixteenth">Sixteenth observable source.</param>
        /// <returns>An observable sequence containing the result of combining elements of the sources using tuple values.</returns>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="first" /> or <paramref name="second" /> or <paramref name="third" /> or <paramref name="fourth" /> or <paramref name="fifth" /> or <paramref name="sixth" /> or <paramref name="seventh" /> or <paramref name="eighth" /> or <paramref name="ninth" /> or <paramref name="tenth" /> or <paramref name="eleventh" /> or <paramref name="twelfth" /> or <paramref name="thirteenth" /> or <paramref name="fourteenth" /> or <paramref name="fifteenth" /> or <paramref name="sixteenth" /> is null.</exception>
        public static IQbservable<(TFirst First, TSecond Second, TThird Third, TFourth Fourth, TFifth Fifth, TSixth Sixth, TSeventh Seventh, TEighth Eighth, TNinth Ninth, TTenth Tenth, TEleventh Eleventh, TTwelfth Twelfth, TThirteenth Thirteenth, TFourteenth Fourteenth, TFifteenth Fifteenth, TSixteenth Sixteenth)> CombineLatest<TFirst, TSecond, TThird, TFourth, TFifth, TSixth, TSeventh, TEighth, TNinth, TTenth, TEleventh, TTwelfth, TThirteenth, TFourteenth, TFifteenth, TSixteenth>(this IQbservable<TFirst> first, IObservable<TSecond> second, IObservable<TThird> third, IObservable<TFourth> fourth, IObservable<TFifth> fifth, IObservable<TSixth> sixth, IObservable<TSeventh> seventh, IObservable<TEighth> eighth, IObservable<TNinth> ninth, IObservable<TTenth> tenth, IObservable<TEleventh> eleventh, IObservable<TTwelfth> twelfth, IObservable<TThirteenth> thirteenth, IObservable<TFourteenth> fourteenth, IObservable<TFifteenth> fifteenth, IObservable<TSixteenth> sixteenth)
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
            if (ninth == null)
                throw new ArgumentNullException(nameof(ninth));
            if (tenth == null)
                throw new ArgumentNullException(nameof(tenth));
            if (eleventh == null)
                throw new ArgumentNullException(nameof(eleventh));
            if (twelfth == null)
                throw new ArgumentNullException(nameof(twelfth));
            if (thirteenth == null)
                throw new ArgumentNullException(nameof(thirteenth));
            if (fourteenth == null)
                throw new ArgumentNullException(nameof(fourteenth));
            if (fifteenth == null)
                throw new ArgumentNullException(nameof(fifteenth));
            if (sixteenth == null)
                throw new ArgumentNullException(nameof(sixteenth));

            return first.Provider.CreateQuery<(TFirst First, TSecond Second, TThird Third, TFourth Fourth, TFifth Fifth, TSixth Sixth, TSeventh Seventh, TEighth Eighth, TNinth Ninth, TTenth Tenth, TEleventh Eleventh, TTwelfth Twelfth, TThirteenth Thirteenth, TFourteenth Fourteenth, TFifteenth Fifteenth, TSixteenth Sixteenth)>(
                Expression.Call(
                    null,
                    new Func<IQbservable<TFirst>, IObservable<TSecond>, IObservable<TThird>, IObservable<TFourth>, IObservable<TFifth>, IObservable<TSixth>, IObservable<TSeventh>, IObservable<TEighth>, IObservable<TNinth>, IObservable<TTenth>, IObservable<TEleventh>, IObservable<TTwelfth>, IObservable<TThirteenth>, IObservable<TFourteenth>, IObservable<TFifteenth>, IObservable<TSixteenth>, IQbservable<(TFirst First, TSecond Second, TThird Third, TFourth Fourth, TFifth Fifth, TSixth Sixth, TSeventh Seventh, TEighth Eighth, TNinth Ninth, TTenth Tenth, TEleventh Eleventh, TTwelfth Twelfth, TThirteenth Thirteenth, TFourteenth Fourteenth, TFifteenth Fifteenth, TSixteenth Sixteenth)>>(CombineLatest<TFirst, TSecond, TThird, TFourth, TFifth, TSixth, TSeventh, TEighth, TNinth, TTenth, TEleventh, TTwelfth, TThirteenth, TFourteenth, TFifteenth, TSixteenth>).Method,
                    first.Expression,
                    GetSourceExpression(second),
                    GetSourceExpression(third),
                    GetSourceExpression(fourth),
                    GetSourceExpression(fifth),
                    GetSourceExpression(sixth),
                    GetSourceExpression(seventh),
                    GetSourceExpression(eighth),
                    GetSourceExpression(ninth),
                    GetSourceExpression(tenth),
                    GetSourceExpression(eleventh),
                    GetSourceExpression(twelfth),
                    GetSourceExpression(thirteenth),
                    GetSourceExpression(fourteenth),
                    GetSourceExpression(fifteenth),
                    GetSourceExpression(sixteenth)
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
                    new Func<IQbservableProvider, Expression<Func<IEnumerable<IObservable<object>>>>, IQbservable<Unit>>(Create).Method,
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
                    new Func<IQbservableProvider, Expression<Func<IObserver<TResult>, IEnumerable<IObservable<object>>>>, IQbservable<TResult>>(Create<TResult>).Method,
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
                    new Func<IQbservable<TSource>, Expression<Func<TSource, IObservable<TSource>>>, IQbservable<TSource>>(Expand<TSource>).Method,
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
                    new Func<IQbservable<TSource>, Expression<Func<TSource, IObservable<TSource>>>, IScheduler, IQbservable<TSource>>(Expand<TSource>).Method,
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
                    new Func<IQbservableProvider, IObservable<TSource>[], IQbservable<TSource[]>>(ForkJoin<TSource>).Method,
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
                    new Func<IQbservableProvider, IEnumerable<IObservable<TSource>>, IQbservable<TSource[]>>(ForkJoin<TSource>).Method,
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
                    new Func<IQbservable<TSource1>, IObservable<TSource2>, Expression<Func<TSource1, TSource2, TResult>>, IQbservable<TResult>>(ForkJoin<TSource1, TSource2, TResult>).Method,
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
                    new Func<IQbservable<TSource>, Expression<Func<IObservable<TSource>, IObservable<TResult>>>, IQbservable<TResult>>(Let<TSource, TResult>).Method,
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
                    new Func<IQbservable<TSource>, Expression<Func<IObservable<TSource>, TResult>>, IQbservable<TResult>>(ManySelect<TSource, TResult>).Method,
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
                    new Func<IQbservable<TSource>, Expression<Func<IObservable<TSource>, TResult>>, IScheduler, IQbservable<TResult>>(ManySelect<TSource, TResult>).Method,
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
        public static IQbservable<(TFirst First, TSecond Second)> WithLatestFrom<TFirst, TSecond>(this IQbservable<TFirst> first, IObservable<TSecond> second)
        {
            if (first == null)
                throw new ArgumentNullException(nameof(first));
            if (second == null)
                throw new ArgumentNullException(nameof(second));

            return first.Provider.CreateQuery<(TFirst First, TSecond Second)>(
                Expression.Call(
                    null,
                    new Func<IQbservable<TFirst>, IObservable<TSecond>, IQbservable<(TFirst First, TSecond Second)>>(WithLatestFrom<TFirst, TSecond>).Method,
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
        public static IQbservable<(TFirst First, TSecond Second)> Zip<TFirst, TSecond>(this IQbservable<TFirst> first, IEnumerable<TSecond> second)
        {
            if (first == null)
                throw new ArgumentNullException(nameof(first));
            if (second == null)
                throw new ArgumentNullException(nameof(second));

            return first.Provider.CreateQuery<(TFirst First, TSecond Second)>(
                Expression.Call(
                    null,
                    new Func<IQbservable<TFirst>, IEnumerable<TSecond>, IQbservable<(TFirst First, TSecond Second)>>(Zip<TFirst, TSecond>).Method,
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
        public static IQbservable<(TFirst First, TSecond Second)> Zip<TFirst, TSecond>(this IQbservable<TFirst> first, IObservable<TSecond> second)
        {
            if (first == null)
                throw new ArgumentNullException(nameof(first));
            if (second == null)
                throw new ArgumentNullException(nameof(second));

            return first.Provider.CreateQuery<(TFirst First, TSecond Second)>(
                Expression.Call(
                    null,
                    new Func<IQbservable<TFirst>, IObservable<TSecond>, IQbservable<(TFirst First, TSecond Second)>>(Zip<TFirst, TSecond>).Method,
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
        public static IQbservable<(TFirst First, TSecond Second, TThird Third)> Zip<TFirst, TSecond, TThird>(this IQbservable<TFirst> first, IObservable<TSecond> second, IObservable<TThird> third)
        {
            if (first == null)
                throw new ArgumentNullException(nameof(first));
            if (second == null)
                throw new ArgumentNullException(nameof(second));
            if (third == null)
                throw new ArgumentNullException(nameof(third));

            return first.Provider.CreateQuery<(TFirst First, TSecond Second, TThird Third)>(
                Expression.Call(
                    null,
                    new Func<IQbservable<TFirst>, IObservable<TSecond>, IObservable<TThird>, IQbservable<(TFirst First, TSecond Second, TThird Third)>>(Zip<TFirst, TSecond, TThird>).Method,
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
        public static IQbservable<(TFirst First, TSecond Second, TThird Third, TFourth Fourth)> Zip<TFirst, TSecond, TThird, TFourth>(this IQbservable<TFirst> first, IObservable<TSecond> second, IObservable<TThird> third, IObservable<TFourth> fourth)
        {
            if (first == null)
                throw new ArgumentNullException(nameof(first));
            if (second == null)
                throw new ArgumentNullException(nameof(second));
            if (third == null)
                throw new ArgumentNullException(nameof(third));
            if (fourth == null)
                throw new ArgumentNullException(nameof(fourth));

            return first.Provider.CreateQuery<(TFirst First, TSecond Second, TThird Third, TFourth Fourth)>(
                Expression.Call(
                    null,
                    new Func<IQbservable<TFirst>, IObservable<TSecond>, IObservable<TThird>, IObservable<TFourth>, IQbservable<(TFirst First, TSecond Second, TThird Third, TFourth Fourth)>>(Zip<TFirst, TSecond, TThird, TFourth>).Method,
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
        public static IQbservable<(TFirst First, TSecond Second, TThird Third, TFourth Fourth, TFifth Fifth)> Zip<TFirst, TSecond, TThird, TFourth, TFifth>(this IQbservable<TFirst> first, IObservable<TSecond> second, IObservable<TThird> third, IObservable<TFourth> fourth, IObservable<TFifth> fifth)
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

            return first.Provider.CreateQuery<(TFirst First, TSecond Second, TThird Third, TFourth Fourth, TFifth Fifth)>(
                Expression.Call(
                    null,
                    new Func<IQbservable<TFirst>, IObservable<TSecond>, IObservable<TThird>, IObservable<TFourth>, IObservable<TFifth>, IQbservable<(TFirst First, TSecond Second, TThird Third, TFourth Fourth, TFifth Fifth)>>(Zip<TFirst, TSecond, TThird, TFourth, TFifth>).Method,
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
        public static IQbservable<(TFirst First, TSecond Second, TThird Third, TFourth Fourth, TFifth Fifth, TSixth Sixth)> Zip<TFirst, TSecond, TThird, TFourth, TFifth, TSixth>(this IQbservable<TFirst> first, IObservable<TSecond> second, IObservable<TThird> third, IObservable<TFourth> fourth, IObservable<TFifth> fifth, IObservable<TSixth> sixth)
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

            return first.Provider.CreateQuery<(TFirst First, TSecond Second, TThird Third, TFourth Fourth, TFifth Fifth, TSixth Sixth)>(
                Expression.Call(
                    null,
                    new Func<IQbservable<TFirst>, IObservable<TSecond>, IObservable<TThird>, IObservable<TFourth>, IObservable<TFifth>, IObservable<TSixth>, IQbservable<(TFirst First, TSecond Second, TThird Third, TFourth Fourth, TFifth Fifth, TSixth Sixth)>>(Zip<TFirst, TSecond, TThird, TFourth, TFifth, TSixth>).Method,
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
        public static IQbservable<(TFirst First, TSecond Second, TThird Third, TFourth Fourth, TFifth Fifth, TSixth Sixth, TSeventh Seventh)> Zip<TFirst, TSecond, TThird, TFourth, TFifth, TSixth, TSeventh>(this IQbservable<TFirst> first, IObservable<TSecond> second, IObservable<TThird> third, IObservable<TFourth> fourth, IObservable<TFifth> fifth, IObservable<TSixth> sixth, IObservable<TSeventh> seventh)
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

            return first.Provider.CreateQuery<(TFirst First, TSecond Second, TThird Third, TFourth Fourth, TFifth Fifth, TSixth Sixth, TSeventh Seventh)>(
                Expression.Call(
                    null,
                    new Func<IQbservable<TFirst>, IObservable<TSecond>, IObservable<TThird>, IObservable<TFourth>, IObservable<TFifth>, IObservable<TSixth>, IObservable<TSeventh>, IQbservable<(TFirst First, TSecond Second, TThird Third, TFourth Fourth, TFifth Fifth, TSixth Sixth, TSeventh Seventh)>>(Zip<TFirst, TSecond, TThird, TFourth, TFifth, TSixth, TSeventh>).Method,
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
        public static IQbservable<(TFirst First, TSecond Second, TThird Third, TFourth Fourth, TFifth Fifth, TSixth Sixth, TSeventh Seventh, TEighth Eighth)> Zip<TFirst, TSecond, TThird, TFourth, TFifth, TSixth, TSeventh, TEighth>(this IQbservable<TFirst> first, IObservable<TSecond> second, IObservable<TThird> third, IObservable<TFourth> fourth, IObservable<TFifth> fifth, IObservable<TSixth> sixth, IObservable<TSeventh> seventh, IObservable<TEighth> eighth)
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

            return first.Provider.CreateQuery<(TFirst First, TSecond Second, TThird Third, TFourth Fourth, TFifth Fifth, TSixth Sixth, TSeventh Seventh, TEighth Eighth)>(
                Expression.Call(
                    null,
                    new Func<IQbservable<TFirst>, IObservable<TSecond>, IObservable<TThird>, IObservable<TFourth>, IObservable<TFifth>, IObservable<TSixth>, IObservable<TSeventh>, IObservable<TEighth>, IQbservable<(TFirst First, TSecond Second, TThird Third, TFourth Fourth, TFifth Fifth, TSixth Sixth, TSeventh Seventh, TEighth Eighth)>>(Zip<TFirst, TSecond, TThird, TFourth, TFifth, TSixth, TSeventh, TEighth>).Method,
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
        /// <typeparam name="TNinth">The type of the elements in the ninth source sequence.</typeparam>
        /// <param name="first">First observable source.</param>
        /// <param name="second">Second observable source.</param>
        /// <param name="third">Third observable source.</param>
        /// <param name="fourth">Fourth observable source.</param>
        /// <param name="fifth">Fifth observable source.</param>
        /// <param name="sixth">Sixth observable source.</param>
        /// <param name="seventh">Seventh observable source.</param>
        /// <param name="eighth">Eighth observable source.</param>
        /// <param name="ninth">Ninth observable source.</param>
        /// <returns>An observable sequence containing the result of combining elements of the sources using tuple values.</returns>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="first" /> or <paramref name="second" /> or <paramref name="third" /> or <paramref name="fourth" /> or <paramref name="fifth" /> or <paramref name="sixth" /> or <paramref name="seventh" /> or <paramref name="eighth" /> or <paramref name="ninth" /> is null.</exception>
        public static IQbservable<(TFirst First, TSecond Second, TThird Third, TFourth Fourth, TFifth Fifth, TSixth Sixth, TSeventh Seventh, TEighth Eighth, TNinth Ninth)> Zip<TFirst, TSecond, TThird, TFourth, TFifth, TSixth, TSeventh, TEighth, TNinth>(this IQbservable<TFirst> first, IObservable<TSecond> second, IObservable<TThird> third, IObservable<TFourth> fourth, IObservable<TFifth> fifth, IObservable<TSixth> sixth, IObservable<TSeventh> seventh, IObservable<TEighth> eighth, IObservable<TNinth> ninth)
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
            if (ninth == null)
                throw new ArgumentNullException(nameof(ninth));

            return first.Provider.CreateQuery<(TFirst First, TSecond Second, TThird Third, TFourth Fourth, TFifth Fifth, TSixth Sixth, TSeventh Seventh, TEighth Eighth, TNinth Ninth)>(
                Expression.Call(
                    null,
                    new Func<IQbservable<TFirst>, IObservable<TSecond>, IObservable<TThird>, IObservable<TFourth>, IObservable<TFifth>, IObservable<TSixth>, IObservable<TSeventh>, IObservable<TEighth>, IObservable<TNinth>, IQbservable<(TFirst First, TSecond Second, TThird Third, TFourth Fourth, TFifth Fifth, TSixth Sixth, TSeventh Seventh, TEighth Eighth, TNinth Ninth)>>(Zip<TFirst, TSecond, TThird, TFourth, TFifth, TSixth, TSeventh, TEighth, TNinth>).Method,
                    first.Expression,
                    GetSourceExpression(second),
                    GetSourceExpression(third),
                    GetSourceExpression(fourth),
                    GetSourceExpression(fifth),
                    GetSourceExpression(sixth),
                    GetSourceExpression(seventh),
                    GetSourceExpression(eighth),
                    GetSourceExpression(ninth)
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
        /// <typeparam name="TNinth">The type of the elements in the ninth source sequence.</typeparam>
        /// <typeparam name="TTenth">The type of the elements in the tenth source sequence.</typeparam>
        /// <param name="first">First observable source.</param>
        /// <param name="second">Second observable source.</param>
        /// <param name="third">Third observable source.</param>
        /// <param name="fourth">Fourth observable source.</param>
        /// <param name="fifth">Fifth observable source.</param>
        /// <param name="sixth">Sixth observable source.</param>
        /// <param name="seventh">Seventh observable source.</param>
        /// <param name="eighth">Eighth observable source.</param>
        /// <param name="ninth">Ninth observable source.</param>
        /// <param name="tenth">Tenth observable source.</param>
        /// <returns>An observable sequence containing the result of combining elements of the sources using tuple values.</returns>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="first" /> or <paramref name="second" /> or <paramref name="third" /> or <paramref name="fourth" /> or <paramref name="fifth" /> or <paramref name="sixth" /> or <paramref name="seventh" /> or <paramref name="eighth" /> or <paramref name="ninth" /> or <paramref name="tenth" /> is null.</exception>
        public static IQbservable<(TFirst First, TSecond Second, TThird Third, TFourth Fourth, TFifth Fifth, TSixth Sixth, TSeventh Seventh, TEighth Eighth, TNinth Ninth, TTenth Tenth)> Zip<TFirst, TSecond, TThird, TFourth, TFifth, TSixth, TSeventh, TEighth, TNinth, TTenth>(this IQbservable<TFirst> first, IObservable<TSecond> second, IObservable<TThird> third, IObservable<TFourth> fourth, IObservable<TFifth> fifth, IObservable<TSixth> sixth, IObservable<TSeventh> seventh, IObservable<TEighth> eighth, IObservable<TNinth> ninth, IObservable<TTenth> tenth)
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
            if (ninth == null)
                throw new ArgumentNullException(nameof(ninth));
            if (tenth == null)
                throw new ArgumentNullException(nameof(tenth));

            return first.Provider.CreateQuery<(TFirst First, TSecond Second, TThird Third, TFourth Fourth, TFifth Fifth, TSixth Sixth, TSeventh Seventh, TEighth Eighth, TNinth Ninth, TTenth Tenth)>(
                Expression.Call(
                    null,
                    new Func<IQbservable<TFirst>, IObservable<TSecond>, IObservable<TThird>, IObservable<TFourth>, IObservable<TFifth>, IObservable<TSixth>, IObservable<TSeventh>, IObservable<TEighth>, IObservable<TNinth>, IObservable<TTenth>, IQbservable<(TFirst First, TSecond Second, TThird Third, TFourth Fourth, TFifth Fifth, TSixth Sixth, TSeventh Seventh, TEighth Eighth, TNinth Ninth, TTenth Tenth)>>(Zip<TFirst, TSecond, TThird, TFourth, TFifth, TSixth, TSeventh, TEighth, TNinth, TTenth>).Method,
                    first.Expression,
                    GetSourceExpression(second),
                    GetSourceExpression(third),
                    GetSourceExpression(fourth),
                    GetSourceExpression(fifth),
                    GetSourceExpression(sixth),
                    GetSourceExpression(seventh),
                    GetSourceExpression(eighth),
                    GetSourceExpression(ninth),
                    GetSourceExpression(tenth)
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
        /// <typeparam name="TNinth">The type of the elements in the ninth source sequence.</typeparam>
        /// <typeparam name="TTenth">The type of the elements in the tenth source sequence.</typeparam>
        /// <typeparam name="TEleventh">The type of the elements in the eleventh source sequence.</typeparam>
        /// <param name="first">First observable source.</param>
        /// <param name="second">Second observable source.</param>
        /// <param name="third">Third observable source.</param>
        /// <param name="fourth">Fourth observable source.</param>
        /// <param name="fifth">Fifth observable source.</param>
        /// <param name="sixth">Sixth observable source.</param>
        /// <param name="seventh">Seventh observable source.</param>
        /// <param name="eighth">Eighth observable source.</param>
        /// <param name="ninth">Ninth observable source.</param>
        /// <param name="tenth">Tenth observable source.</param>
        /// <param name="eleventh">Eleventh observable source.</param>
        /// <returns>An observable sequence containing the result of combining elements of the sources using tuple values.</returns>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="first" /> or <paramref name="second" /> or <paramref name="third" /> or <paramref name="fourth" /> or <paramref name="fifth" /> or <paramref name="sixth" /> or <paramref name="seventh" /> or <paramref name="eighth" /> or <paramref name="ninth" /> or <paramref name="tenth" /> or <paramref name="eleventh" /> is null.</exception>
        public static IQbservable<(TFirst First, TSecond Second, TThird Third, TFourth Fourth, TFifth Fifth, TSixth Sixth, TSeventh Seventh, TEighth Eighth, TNinth Ninth, TTenth Tenth, TEleventh Eleventh)> Zip<TFirst, TSecond, TThird, TFourth, TFifth, TSixth, TSeventh, TEighth, TNinth, TTenth, TEleventh>(this IQbservable<TFirst> first, IObservable<TSecond> second, IObservable<TThird> third, IObservable<TFourth> fourth, IObservable<TFifth> fifth, IObservable<TSixth> sixth, IObservable<TSeventh> seventh, IObservable<TEighth> eighth, IObservable<TNinth> ninth, IObservable<TTenth> tenth, IObservable<TEleventh> eleventh)
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
            if (ninth == null)
                throw new ArgumentNullException(nameof(ninth));
            if (tenth == null)
                throw new ArgumentNullException(nameof(tenth));
            if (eleventh == null)
                throw new ArgumentNullException(nameof(eleventh));

            return first.Provider.CreateQuery<(TFirst First, TSecond Second, TThird Third, TFourth Fourth, TFifth Fifth, TSixth Sixth, TSeventh Seventh, TEighth Eighth, TNinth Ninth, TTenth Tenth, TEleventh Eleventh)>(
                Expression.Call(
                    null,
                    new Func<IQbservable<TFirst>, IObservable<TSecond>, IObservable<TThird>, IObservable<TFourth>, IObservable<TFifth>, IObservable<TSixth>, IObservable<TSeventh>, IObservable<TEighth>, IObservable<TNinth>, IObservable<TTenth>, IObservable<TEleventh>, IQbservable<(TFirst First, TSecond Second, TThird Third, TFourth Fourth, TFifth Fifth, TSixth Sixth, TSeventh Seventh, TEighth Eighth, TNinth Ninth, TTenth Tenth, TEleventh Eleventh)>>(Zip<TFirst, TSecond, TThird, TFourth, TFifth, TSixth, TSeventh, TEighth, TNinth, TTenth, TEleventh>).Method,
                    first.Expression,
                    GetSourceExpression(second),
                    GetSourceExpression(third),
                    GetSourceExpression(fourth),
                    GetSourceExpression(fifth),
                    GetSourceExpression(sixth),
                    GetSourceExpression(seventh),
                    GetSourceExpression(eighth),
                    GetSourceExpression(ninth),
                    GetSourceExpression(tenth),
                    GetSourceExpression(eleventh)
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
        /// <typeparam name="TNinth">The type of the elements in the ninth source sequence.</typeparam>
        /// <typeparam name="TTenth">The type of the elements in the tenth source sequence.</typeparam>
        /// <typeparam name="TEleventh">The type of the elements in the eleventh source sequence.</typeparam>
        /// <typeparam name="TTwelfth">The type of the elements in the twelfth source sequence.</typeparam>
        /// <param name="first">First observable source.</param>
        /// <param name="second">Second observable source.</param>
        /// <param name="third">Third observable source.</param>
        /// <param name="fourth">Fourth observable source.</param>
        /// <param name="fifth">Fifth observable source.</param>
        /// <param name="sixth">Sixth observable source.</param>
        /// <param name="seventh">Seventh observable source.</param>
        /// <param name="eighth">Eighth observable source.</param>
        /// <param name="ninth">Ninth observable source.</param>
        /// <param name="tenth">Tenth observable source.</param>
        /// <param name="eleventh">Eleventh observable source.</param>
        /// <param name="twelfth">Twelfth observable source.</param>
        /// <returns>An observable sequence containing the result of combining elements of the sources using tuple values.</returns>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="first" /> or <paramref name="second" /> or <paramref name="third" /> or <paramref name="fourth" /> or <paramref name="fifth" /> or <paramref name="sixth" /> or <paramref name="seventh" /> or <paramref name="eighth" /> or <paramref name="ninth" /> or <paramref name="tenth" /> or <paramref name="eleventh" /> or <paramref name="twelfth" /> is null.</exception>
        public static IQbservable<(TFirst First, TSecond Second, TThird Third, TFourth Fourth, TFifth Fifth, TSixth Sixth, TSeventh Seventh, TEighth Eighth, TNinth Ninth, TTenth Tenth, TEleventh Eleventh, TTwelfth Twelfth)> Zip<TFirst, TSecond, TThird, TFourth, TFifth, TSixth, TSeventh, TEighth, TNinth, TTenth, TEleventh, TTwelfth>(this IQbservable<TFirst> first, IObservable<TSecond> second, IObservable<TThird> third, IObservable<TFourth> fourth, IObservable<TFifth> fifth, IObservable<TSixth> sixth, IObservable<TSeventh> seventh, IObservable<TEighth> eighth, IObservable<TNinth> ninth, IObservable<TTenth> tenth, IObservable<TEleventh> eleventh, IObservable<TTwelfth> twelfth)
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
            if (ninth == null)
                throw new ArgumentNullException(nameof(ninth));
            if (tenth == null)
                throw new ArgumentNullException(nameof(tenth));
            if (eleventh == null)
                throw new ArgumentNullException(nameof(eleventh));
            if (twelfth == null)
                throw new ArgumentNullException(nameof(twelfth));

            return first.Provider.CreateQuery<(TFirst First, TSecond Second, TThird Third, TFourth Fourth, TFifth Fifth, TSixth Sixth, TSeventh Seventh, TEighth Eighth, TNinth Ninth, TTenth Tenth, TEleventh Eleventh, TTwelfth Twelfth)>(
                Expression.Call(
                    null,
                    new Func<IQbservable<TFirst>, IObservable<TSecond>, IObservable<TThird>, IObservable<TFourth>, IObservable<TFifth>, IObservable<TSixth>, IObservable<TSeventh>, IObservable<TEighth>, IObservable<TNinth>, IObservable<TTenth>, IObservable<TEleventh>, IObservable<TTwelfth>, IQbservable<(TFirst First, TSecond Second, TThird Third, TFourth Fourth, TFifth Fifth, TSixth Sixth, TSeventh Seventh, TEighth Eighth, TNinth Ninth, TTenth Tenth, TEleventh Eleventh, TTwelfth Twelfth)>>(Zip<TFirst, TSecond, TThird, TFourth, TFifth, TSixth, TSeventh, TEighth, TNinth, TTenth, TEleventh, TTwelfth>).Method,
                    first.Expression,
                    GetSourceExpression(second),
                    GetSourceExpression(third),
                    GetSourceExpression(fourth),
                    GetSourceExpression(fifth),
                    GetSourceExpression(sixth),
                    GetSourceExpression(seventh),
                    GetSourceExpression(eighth),
                    GetSourceExpression(ninth),
                    GetSourceExpression(tenth),
                    GetSourceExpression(eleventh),
                    GetSourceExpression(twelfth)
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
        /// <typeparam name="TNinth">The type of the elements in the ninth source sequence.</typeparam>
        /// <typeparam name="TTenth">The type of the elements in the tenth source sequence.</typeparam>
        /// <typeparam name="TEleventh">The type of the elements in the eleventh source sequence.</typeparam>
        /// <typeparam name="TTwelfth">The type of the elements in the twelfth source sequence.</typeparam>
        /// <typeparam name="TThirteenth">The type of the elements in the thirteenth source sequence.</typeparam>
        /// <param name="first">First observable source.</param>
        /// <param name="second">Second observable source.</param>
        /// <param name="third">Third observable source.</param>
        /// <param name="fourth">Fourth observable source.</param>
        /// <param name="fifth">Fifth observable source.</param>
        /// <param name="sixth">Sixth observable source.</param>
        /// <param name="seventh">Seventh observable source.</param>
        /// <param name="eighth">Eighth observable source.</param>
        /// <param name="ninth">Ninth observable source.</param>
        /// <param name="tenth">Tenth observable source.</param>
        /// <param name="eleventh">Eleventh observable source.</param>
        /// <param name="twelfth">Twelfth observable source.</param>
        /// <param name="thirteenth">Thirteenth observable source.</param>
        /// <returns>An observable sequence containing the result of combining elements of the sources using tuple values.</returns>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="first" /> or <paramref name="second" /> or <paramref name="third" /> or <paramref name="fourth" /> or <paramref name="fifth" /> or <paramref name="sixth" /> or <paramref name="seventh" /> or <paramref name="eighth" /> or <paramref name="ninth" /> or <paramref name="tenth" /> or <paramref name="eleventh" /> or <paramref name="twelfth" /> or <paramref name="thirteenth" /> is null.</exception>
        public static IQbservable<(TFirst First, TSecond Second, TThird Third, TFourth Fourth, TFifth Fifth, TSixth Sixth, TSeventh Seventh, TEighth Eighth, TNinth Ninth, TTenth Tenth, TEleventh Eleventh, TTwelfth Twelfth, TThirteenth Thirteenth)> Zip<TFirst, TSecond, TThird, TFourth, TFifth, TSixth, TSeventh, TEighth, TNinth, TTenth, TEleventh, TTwelfth, TThirteenth>(this IQbservable<TFirst> first, IObservable<TSecond> second, IObservable<TThird> third, IObservable<TFourth> fourth, IObservable<TFifth> fifth, IObservable<TSixth> sixth, IObservable<TSeventh> seventh, IObservable<TEighth> eighth, IObservable<TNinth> ninth, IObservable<TTenth> tenth, IObservable<TEleventh> eleventh, IObservable<TTwelfth> twelfth, IObservable<TThirteenth> thirteenth)
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
            if (ninth == null)
                throw new ArgumentNullException(nameof(ninth));
            if (tenth == null)
                throw new ArgumentNullException(nameof(tenth));
            if (eleventh == null)
                throw new ArgumentNullException(nameof(eleventh));
            if (twelfth == null)
                throw new ArgumentNullException(nameof(twelfth));
            if (thirteenth == null)
                throw new ArgumentNullException(nameof(thirteenth));

            return first.Provider.CreateQuery<(TFirst First, TSecond Second, TThird Third, TFourth Fourth, TFifth Fifth, TSixth Sixth, TSeventh Seventh, TEighth Eighth, TNinth Ninth, TTenth Tenth, TEleventh Eleventh, TTwelfth Twelfth, TThirteenth Thirteenth)>(
                Expression.Call(
                    null,
                    new Func<IQbservable<TFirst>, IObservable<TSecond>, IObservable<TThird>, IObservable<TFourth>, IObservable<TFifth>, IObservable<TSixth>, IObservable<TSeventh>, IObservable<TEighth>, IObservable<TNinth>, IObservable<TTenth>, IObservable<TEleventh>, IObservable<TTwelfth>, IObservable<TThirteenth>, IQbservable<(TFirst First, TSecond Second, TThird Third, TFourth Fourth, TFifth Fifth, TSixth Sixth, TSeventh Seventh, TEighth Eighth, TNinth Ninth, TTenth Tenth, TEleventh Eleventh, TTwelfth Twelfth, TThirteenth Thirteenth)>>(Zip<TFirst, TSecond, TThird, TFourth, TFifth, TSixth, TSeventh, TEighth, TNinth, TTenth, TEleventh, TTwelfth, TThirteenth>).Method,
                    first.Expression,
                    GetSourceExpression(second),
                    GetSourceExpression(third),
                    GetSourceExpression(fourth),
                    GetSourceExpression(fifth),
                    GetSourceExpression(sixth),
                    GetSourceExpression(seventh),
                    GetSourceExpression(eighth),
                    GetSourceExpression(ninth),
                    GetSourceExpression(tenth),
                    GetSourceExpression(eleventh),
                    GetSourceExpression(twelfth),
                    GetSourceExpression(thirteenth)
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
        /// <typeparam name="TNinth">The type of the elements in the ninth source sequence.</typeparam>
        /// <typeparam name="TTenth">The type of the elements in the tenth source sequence.</typeparam>
        /// <typeparam name="TEleventh">The type of the elements in the eleventh source sequence.</typeparam>
        /// <typeparam name="TTwelfth">The type of the elements in the twelfth source sequence.</typeparam>
        /// <typeparam name="TThirteenth">The type of the elements in the thirteenth source sequence.</typeparam>
        /// <typeparam name="TFourteenth">The type of the elements in the fourteenth source sequence.</typeparam>
        /// <param name="first">First observable source.</param>
        /// <param name="second">Second observable source.</param>
        /// <param name="third">Third observable source.</param>
        /// <param name="fourth">Fourth observable source.</param>
        /// <param name="fifth">Fifth observable source.</param>
        /// <param name="sixth">Sixth observable source.</param>
        /// <param name="seventh">Seventh observable source.</param>
        /// <param name="eighth">Eighth observable source.</param>
        /// <param name="ninth">Ninth observable source.</param>
        /// <param name="tenth">Tenth observable source.</param>
        /// <param name="eleventh">Eleventh observable source.</param>
        /// <param name="twelfth">Twelfth observable source.</param>
        /// <param name="thirteenth">Thirteenth observable source.</param>
        /// <param name="fourteenth">Fourteenth observable source.</param>
        /// <returns>An observable sequence containing the result of combining elements of the sources using tuple values.</returns>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="first" /> or <paramref name="second" /> or <paramref name="third" /> or <paramref name="fourth" /> or <paramref name="fifth" /> or <paramref name="sixth" /> or <paramref name="seventh" /> or <paramref name="eighth" /> or <paramref name="ninth" /> or <paramref name="tenth" /> or <paramref name="eleventh" /> or <paramref name="twelfth" /> or <paramref name="thirteenth" /> or <paramref name="fourteenth" /> is null.</exception>
        public static IQbservable<(TFirst First, TSecond Second, TThird Third, TFourth Fourth, TFifth Fifth, TSixth Sixth, TSeventh Seventh, TEighth Eighth, TNinth Ninth, TTenth Tenth, TEleventh Eleventh, TTwelfth Twelfth, TThirteenth Thirteenth, TFourteenth Fourteenth)> Zip<TFirst, TSecond, TThird, TFourth, TFifth, TSixth, TSeventh, TEighth, TNinth, TTenth, TEleventh, TTwelfth, TThirteenth, TFourteenth>(this IQbservable<TFirst> first, IObservable<TSecond> second, IObservable<TThird> third, IObservable<TFourth> fourth, IObservable<TFifth> fifth, IObservable<TSixth> sixth, IObservable<TSeventh> seventh, IObservable<TEighth> eighth, IObservable<TNinth> ninth, IObservable<TTenth> tenth, IObservable<TEleventh> eleventh, IObservable<TTwelfth> twelfth, IObservable<TThirteenth> thirteenth, IObservable<TFourteenth> fourteenth)
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
            if (ninth == null)
                throw new ArgumentNullException(nameof(ninth));
            if (tenth == null)
                throw new ArgumentNullException(nameof(tenth));
            if (eleventh == null)
                throw new ArgumentNullException(nameof(eleventh));
            if (twelfth == null)
                throw new ArgumentNullException(nameof(twelfth));
            if (thirteenth == null)
                throw new ArgumentNullException(nameof(thirteenth));
            if (fourteenth == null)
                throw new ArgumentNullException(nameof(fourteenth));

            return first.Provider.CreateQuery<(TFirst First, TSecond Second, TThird Third, TFourth Fourth, TFifth Fifth, TSixth Sixth, TSeventh Seventh, TEighth Eighth, TNinth Ninth, TTenth Tenth, TEleventh Eleventh, TTwelfth Twelfth, TThirteenth Thirteenth, TFourteenth Fourteenth)>(
                Expression.Call(
                    null,
                    new Func<IQbservable<TFirst>, IObservable<TSecond>, IObservable<TThird>, IObservable<TFourth>, IObservable<TFifth>, IObservable<TSixth>, IObservable<TSeventh>, IObservable<TEighth>, IObservable<TNinth>, IObservable<TTenth>, IObservable<TEleventh>, IObservable<TTwelfth>, IObservable<TThirteenth>, IObservable<TFourteenth>, IQbservable<(TFirst First, TSecond Second, TThird Third, TFourth Fourth, TFifth Fifth, TSixth Sixth, TSeventh Seventh, TEighth Eighth, TNinth Ninth, TTenth Tenth, TEleventh Eleventh, TTwelfth Twelfth, TThirteenth Thirteenth, TFourteenth Fourteenth)>>(Zip<TFirst, TSecond, TThird, TFourth, TFifth, TSixth, TSeventh, TEighth, TNinth, TTenth, TEleventh, TTwelfth, TThirteenth, TFourteenth>).Method,
                    first.Expression,
                    GetSourceExpression(second),
                    GetSourceExpression(third),
                    GetSourceExpression(fourth),
                    GetSourceExpression(fifth),
                    GetSourceExpression(sixth),
                    GetSourceExpression(seventh),
                    GetSourceExpression(eighth),
                    GetSourceExpression(ninth),
                    GetSourceExpression(tenth),
                    GetSourceExpression(eleventh),
                    GetSourceExpression(twelfth),
                    GetSourceExpression(thirteenth),
                    GetSourceExpression(fourteenth)
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
        /// <typeparam name="TNinth">The type of the elements in the ninth source sequence.</typeparam>
        /// <typeparam name="TTenth">The type of the elements in the tenth source sequence.</typeparam>
        /// <typeparam name="TEleventh">The type of the elements in the eleventh source sequence.</typeparam>
        /// <typeparam name="TTwelfth">The type of the elements in the twelfth source sequence.</typeparam>
        /// <typeparam name="TThirteenth">The type of the elements in the thirteenth source sequence.</typeparam>
        /// <typeparam name="TFourteenth">The type of the elements in the fourteenth source sequence.</typeparam>
        /// <typeparam name="TFifteenth">The type of the elements in the fifteenth source sequence.</typeparam>
        /// <param name="first">First observable source.</param>
        /// <param name="second">Second observable source.</param>
        /// <param name="third">Third observable source.</param>
        /// <param name="fourth">Fourth observable source.</param>
        /// <param name="fifth">Fifth observable source.</param>
        /// <param name="sixth">Sixth observable source.</param>
        /// <param name="seventh">Seventh observable source.</param>
        /// <param name="eighth">Eighth observable source.</param>
        /// <param name="ninth">Ninth observable source.</param>
        /// <param name="tenth">Tenth observable source.</param>
        /// <param name="eleventh">Eleventh observable source.</param>
        /// <param name="twelfth">Twelfth observable source.</param>
        /// <param name="thirteenth">Thirteenth observable source.</param>
        /// <param name="fourteenth">Fourteenth observable source.</param>
        /// <param name="fifteenth">Fifteenth observable source.</param>
        /// <returns>An observable sequence containing the result of combining elements of the sources using tuple values.</returns>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="first" /> or <paramref name="second" /> or <paramref name="third" /> or <paramref name="fourth" /> or <paramref name="fifth" /> or <paramref name="sixth" /> or <paramref name="seventh" /> or <paramref name="eighth" /> or <paramref name="ninth" /> or <paramref name="tenth" /> or <paramref name="eleventh" /> or <paramref name="twelfth" /> or <paramref name="thirteenth" /> or <paramref name="fourteenth" /> or <paramref name="fifteenth" /> is null.</exception>
        public static IQbservable<(TFirst First, TSecond Second, TThird Third, TFourth Fourth, TFifth Fifth, TSixth Sixth, TSeventh Seventh, TEighth Eighth, TNinth Ninth, TTenth Tenth, TEleventh Eleventh, TTwelfth Twelfth, TThirteenth Thirteenth, TFourteenth Fourteenth, TFifteenth Fifteenth)> Zip<TFirst, TSecond, TThird, TFourth, TFifth, TSixth, TSeventh, TEighth, TNinth, TTenth, TEleventh, TTwelfth, TThirteenth, TFourteenth, TFifteenth>(this IQbservable<TFirst> first, IObservable<TSecond> second, IObservable<TThird> third, IObservable<TFourth> fourth, IObservable<TFifth> fifth, IObservable<TSixth> sixth, IObservable<TSeventh> seventh, IObservable<TEighth> eighth, IObservable<TNinth> ninth, IObservable<TTenth> tenth, IObservable<TEleventh> eleventh, IObservable<TTwelfth> twelfth, IObservable<TThirteenth> thirteenth, IObservable<TFourteenth> fourteenth, IObservable<TFifteenth> fifteenth)
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
            if (ninth == null)
                throw new ArgumentNullException(nameof(ninth));
            if (tenth == null)
                throw new ArgumentNullException(nameof(tenth));
            if (eleventh == null)
                throw new ArgumentNullException(nameof(eleventh));
            if (twelfth == null)
                throw new ArgumentNullException(nameof(twelfth));
            if (thirteenth == null)
                throw new ArgumentNullException(nameof(thirteenth));
            if (fourteenth == null)
                throw new ArgumentNullException(nameof(fourteenth));
            if (fifteenth == null)
                throw new ArgumentNullException(nameof(fifteenth));

            return first.Provider.CreateQuery<(TFirst First, TSecond Second, TThird Third, TFourth Fourth, TFifth Fifth, TSixth Sixth, TSeventh Seventh, TEighth Eighth, TNinth Ninth, TTenth Tenth, TEleventh Eleventh, TTwelfth Twelfth, TThirteenth Thirteenth, TFourteenth Fourteenth, TFifteenth Fifteenth)>(
                Expression.Call(
                    null,
                    new Func<IQbservable<TFirst>, IObservable<TSecond>, IObservable<TThird>, IObservable<TFourth>, IObservable<TFifth>, IObservable<TSixth>, IObservable<TSeventh>, IObservable<TEighth>, IObservable<TNinth>, IObservable<TTenth>, IObservable<TEleventh>, IObservable<TTwelfth>, IObservable<TThirteenth>, IObservable<TFourteenth>, IObservable<TFifteenth>, IQbservable<(TFirst First, TSecond Second, TThird Third, TFourth Fourth, TFifth Fifth, TSixth Sixth, TSeventh Seventh, TEighth Eighth, TNinth Ninth, TTenth Tenth, TEleventh Eleventh, TTwelfth Twelfth, TThirteenth Thirteenth, TFourteenth Fourteenth, TFifteenth Fifteenth)>>(Zip<TFirst, TSecond, TThird, TFourth, TFifth, TSixth, TSeventh, TEighth, TNinth, TTenth, TEleventh, TTwelfth, TThirteenth, TFourteenth, TFifteenth>).Method,
                    first.Expression,
                    GetSourceExpression(second),
                    GetSourceExpression(third),
                    GetSourceExpression(fourth),
                    GetSourceExpression(fifth),
                    GetSourceExpression(sixth),
                    GetSourceExpression(seventh),
                    GetSourceExpression(eighth),
                    GetSourceExpression(ninth),
                    GetSourceExpression(tenth),
                    GetSourceExpression(eleventh),
                    GetSourceExpression(twelfth),
                    GetSourceExpression(thirteenth),
                    GetSourceExpression(fourteenth),
                    GetSourceExpression(fifteenth)
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
        /// <typeparam name="TNinth">The type of the elements in the ninth source sequence.</typeparam>
        /// <typeparam name="TTenth">The type of the elements in the tenth source sequence.</typeparam>
        /// <typeparam name="TEleventh">The type of the elements in the eleventh source sequence.</typeparam>
        /// <typeparam name="TTwelfth">The type of the elements in the twelfth source sequence.</typeparam>
        /// <typeparam name="TThirteenth">The type of the elements in the thirteenth source sequence.</typeparam>
        /// <typeparam name="TFourteenth">The type of the elements in the fourteenth source sequence.</typeparam>
        /// <typeparam name="TFifteenth">The type of the elements in the fifteenth source sequence.</typeparam>
        /// <typeparam name="TSixteenth">The type of the elements in the sixteenth source sequence.</typeparam>
        /// <param name="first">First observable source.</param>
        /// <param name="second">Second observable source.</param>
        /// <param name="third">Third observable source.</param>
        /// <param name="fourth">Fourth observable source.</param>
        /// <param name="fifth">Fifth observable source.</param>
        /// <param name="sixth">Sixth observable source.</param>
        /// <param name="seventh">Seventh observable source.</param>
        /// <param name="eighth">Eighth observable source.</param>
        /// <param name="ninth">Ninth observable source.</param>
        /// <param name="tenth">Tenth observable source.</param>
        /// <param name="eleventh">Eleventh observable source.</param>
        /// <param name="twelfth">Twelfth observable source.</param>
        /// <param name="thirteenth">Thirteenth observable source.</param>
        /// <param name="fourteenth">Fourteenth observable source.</param>
        /// <param name="fifteenth">Fifteenth observable source.</param>
        /// <param name="sixteenth">Sixteenth observable source.</param>
        /// <returns>An observable sequence containing the result of combining elements of the sources using tuple values.</returns>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="first" /> or <paramref name="second" /> or <paramref name="third" /> or <paramref name="fourth" /> or <paramref name="fifth" /> or <paramref name="sixth" /> or <paramref name="seventh" /> or <paramref name="eighth" /> or <paramref name="ninth" /> or <paramref name="tenth" /> or <paramref name="eleventh" /> or <paramref name="twelfth" /> or <paramref name="thirteenth" /> or <paramref name="fourteenth" /> or <paramref name="fifteenth" /> or <paramref name="sixteenth" /> is null.</exception>
        public static IQbservable<(TFirst First, TSecond Second, TThird Third, TFourth Fourth, TFifth Fifth, TSixth Sixth, TSeventh Seventh, TEighth Eighth, TNinth Ninth, TTenth Tenth, TEleventh Eleventh, TTwelfth Twelfth, TThirteenth Thirteenth, TFourteenth Fourteenth, TFifteenth Fifteenth, TSixteenth Sixteenth)> Zip<TFirst, TSecond, TThird, TFourth, TFifth, TSixth, TSeventh, TEighth, TNinth, TTenth, TEleventh, TTwelfth, TThirteenth, TFourteenth, TFifteenth, TSixteenth>(this IQbservable<TFirst> first, IObservable<TSecond> second, IObservable<TThird> third, IObservable<TFourth> fourth, IObservable<TFifth> fifth, IObservable<TSixth> sixth, IObservable<TSeventh> seventh, IObservable<TEighth> eighth, IObservable<TNinth> ninth, IObservable<TTenth> tenth, IObservable<TEleventh> eleventh, IObservable<TTwelfth> twelfth, IObservable<TThirteenth> thirteenth, IObservable<TFourteenth> fourteenth, IObservable<TFifteenth> fifteenth, IObservable<TSixteenth> sixteenth)
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
            if (ninth == null)
                throw new ArgumentNullException(nameof(ninth));
            if (tenth == null)
                throw new ArgumentNullException(nameof(tenth));
            if (eleventh == null)
                throw new ArgumentNullException(nameof(eleventh));
            if (twelfth == null)
                throw new ArgumentNullException(nameof(twelfth));
            if (thirteenth == null)
                throw new ArgumentNullException(nameof(thirteenth));
            if (fourteenth == null)
                throw new ArgumentNullException(nameof(fourteenth));
            if (fifteenth == null)
                throw new ArgumentNullException(nameof(fifteenth));
            if (sixteenth == null)
                throw new ArgumentNullException(nameof(sixteenth));

            return first.Provider.CreateQuery<(TFirst First, TSecond Second, TThird Third, TFourth Fourth, TFifth Fifth, TSixth Sixth, TSeventh Seventh, TEighth Eighth, TNinth Ninth, TTenth Tenth, TEleventh Eleventh, TTwelfth Twelfth, TThirteenth Thirteenth, TFourteenth Fourteenth, TFifteenth Fifteenth, TSixteenth Sixteenth)>(
                Expression.Call(
                    null,
                    new Func<IQbservable<TFirst>, IObservable<TSecond>, IObservable<TThird>, IObservable<TFourth>, IObservable<TFifth>, IObservable<TSixth>, IObservable<TSeventh>, IObservable<TEighth>, IObservable<TNinth>, IObservable<TTenth>, IObservable<TEleventh>, IObservable<TTwelfth>, IObservable<TThirteenth>, IObservable<TFourteenth>, IObservable<TFifteenth>, IObservable<TSixteenth>, IQbservable<(TFirst First, TSecond Second, TThird Third, TFourth Fourth, TFifth Fifth, TSixth Sixth, TSeventh Seventh, TEighth Eighth, TNinth Ninth, TTenth Tenth, TEleventh Eleventh, TTwelfth Twelfth, TThirteenth Thirteenth, TFourteenth Fourteenth, TFifteenth Fifteenth, TSixteenth Sixteenth)>>(Zip<TFirst, TSecond, TThird, TFourth, TFifth, TSixth, TSeventh, TEighth, TNinth, TTenth, TEleventh, TTwelfth, TThirteenth, TFourteenth, TFifteenth, TSixteenth>).Method,
                    first.Expression,
                    GetSourceExpression(second),
                    GetSourceExpression(third),
                    GetSourceExpression(fourth),
                    GetSourceExpression(fifth),
                    GetSourceExpression(sixth),
                    GetSourceExpression(seventh),
                    GetSourceExpression(eighth),
                    GetSourceExpression(ninth),
                    GetSourceExpression(tenth),
                    GetSourceExpression(eleventh),
                    GetSourceExpression(twelfth),
                    GetSourceExpression(thirteenth),
                    GetSourceExpression(fourteenth),
                    GetSourceExpression(fifteenth),
                    GetSourceExpression(sixteenth)
                )
            );
        }

    }
}

#pragma warning restore 1591


// Copyright (c) Microsoft Open Technologies, Inc. All rights reserved. See License.txt in the project root for license information.

#if !NO_EXPRESSIONS

#pragma warning disable 1591

using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Reactive.Concurrency;

namespace System.Reactive.Linq
{
    /// <summary>
    /// Provides a set of static methods for writing queries over observable sequences, allowing translation to a target query language.
    /// </summary>
    public static partial class Qbservable
    {
        /// <summary>
        /// Returns the input typed as an IObservable&lt;TSource&gt;.
        /// This operator is used to separate the part of the query that's captured as an expression tree from the part that's executed locally.
        /// </summary>
        /// <typeparam name="TSource">The type of the elements in the source sequence.</typeparam>
        /// <param name="source">An IQbservable&lt;TSource&gt; sequence to convert to an IObservable&lt;TSource&gt; sequence.</param>
        /// <returns>The original source object, but typed as an IObservable&lt;TSource&gt;.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="source"/> is null.</exception>
        public static IObservable<TSource> AsObservable<TSource>(this IQbservable<TSource> source)
        {
            if (source == null)
                throw new ArgumentNullException("source");

            return source;
        }

        /// <summary>
        /// Converts an enumerable sequence to an observable sequence.
        /// </summary>
        /// <typeparam name="TSource">The type of the elements in the source sequence.</typeparam>
        /// <param name="source">Enumerable sequence to convert to an observable sequence.</param>
        /// <returns>The observable sequence whose elements are pulled from the given enumerable sequence.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="source"/> is null.</exception>
        /// <remarks>This operator requires the source's <see cref="IQueryProvider"/> object (see <see cref="IQueryable.Provider"/>) to implement <see cref="IQbservableProvider"/>.</remarks>
        public static IQbservable<TSource> ToQbservable<TSource>(this IQueryable<TSource> source)
        {
            if (source == null)
                throw new ArgumentNullException("source");

            return ((IQbservableProvider)source.Provider).CreateQuery<TSource>(
                Expression.Call(
                    null,
#if CRIPPLED_REFLECTION
                    InfoOf(() => Qbservable.ToQbservable<TSource>(default(IQueryable<TSource>))),
#else
                    ((MethodInfo)MethodInfo.GetCurrentMethod()).MakeGenericMethod(typeof(TSource)),
#endif
                    source.Expression
                )
            );
        }

        /// <summary>
        /// Converts an enumerable sequence to an observable sequence, using the specified scheduler to run the enumeration loop.
        /// </summary>
        /// <typeparam name="TSource">The type of the elements in the source sequence.</typeparam>
        /// <param name="source">Enumerable sequence to convert to an observable sequence.</param>
        /// <param name="scheduler">Scheduler to run the enumeration of the input sequence on.</param>
        /// <returns>The observable sequence whose elements are pulled from the given enumerable sequence.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="source"/> or <paramref name="scheduler"/> is null.</exception>
        /// <remarks>This operator requires the source's <see cref="IQueryProvider"/> object (see <see cref="IQueryable.Provider"/>) to implement <see cref="IQbservableProvider"/>.</remarks>
        public static IQbservable<TSource> ToQbservable<TSource>(this IQueryable<TSource> source, IScheduler scheduler)
        {
            if (source == null)
                throw new ArgumentNullException("source");
            if (scheduler == null)
                throw new ArgumentNullException("scheduler");

            return ((IQbservableProvider)source.Provider).CreateQuery<TSource>(
                Expression.Call(
                    null,
#if CRIPPLED_REFLECTION
                    InfoOf(() => Qbservable.ToQbservable<TSource>(default(IQueryable<TSource>))),
#else
                    ((MethodInfo)MethodInfo.GetCurrentMethod()).MakeGenericMethod(typeof(TSource)),
#endif
                    source.Expression,
                    Expression.Constant(scheduler)
                )
            );
        }

        internal static Expression GetSourceExpression<TSource>(IObservable<TSource> source)
        {
            var q = source as IQbservable<TSource>;
            if (q != null)
                return q.Expression;

            return Expression.Constant(source, typeof(IObservable<TSource>));
        }

        internal static Expression GetSourceExpression<TSource>(IEnumerable<TSource> source)
        {
            var q = source as IQueryable<TSource>;
            if (q != null)
                return q.Expression;

            return Expression.Constant(source, typeof(IEnumerable<TSource>));
        }

        internal static Expression GetSourceExpression<TSource>(IObservable<TSource>[] sources)
        {
            return Expression.NewArrayInit(
                typeof(IObservable<TSource>),
                sources.Select(source => GetSourceExpression(source))
            );
        }

        internal static Expression GetSourceExpression<TSource>(IEnumerable<TSource>[] sources)
        {
            return Expression.NewArrayInit(
                typeof(IEnumerable<TSource>),
                sources.Select(source => GetSourceExpression(source))
            );
        }

        internal static MethodInfo InfoOf<R>(Expression<Func<R>> f)
        {
            return ((MethodCallExpression)f.Body).Method;
        }
    }
}

#pragma warning restore 1591

#endif
// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the Apache 2.0 License.
// See the LICENSE file in the project root for more information. 
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Reflection;

namespace System.Linq
{
    /// <summary>
    /// Provides a set of extension methods for asynchronous enumerable sequences represented using expression trees.
    /// </summary>
    public static partial class AsyncQueryable
    {
        /// <summary>
        /// Converts the specified asynchronous enumerable sequence to an expression representation.
        /// </summary>
        /// <typeparam name="TElement">The type of the elements in the sequence.</typeparam>
        /// <param name="source">The asynchronous enumerable sequence to represent using an expression tree.</param>
        /// <returns>An asynchronous enumerable sequence using an expression tree to represent the specified asynchronous enumerable sequence.</returns>
        public static IAsyncQueryable<TElement> AsAsyncQueryable<TElement>(this IAsyncEnumerable<TElement> source)
        {
            if (source == null)
            {
                throw new ArgumentNullException(nameof(source));
            }

            if (source is IAsyncQueryable<TElement> queryable)
            {
                return queryable;
            }

            return new AsyncEnumerableQuery<TElement>(source);
        }

        private static Expression GetSourceExpression<TSource>(IAsyncEnumerable<TSource> source)
        {
            if (source is IAsyncQueryable<TSource> queryable)
            {
                return queryable.Expression;
            }

            return Expression.Constant(source, typeof(IAsyncEnumerable<TSource>));
        }

        internal static MethodInfo InfoOf<R>(Expression<Func<R>> f)
        {
            return ((MethodCallExpression)f.Body).Method;
        }
    }
}

// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the Apache 2.0 License.
// See the LICENSE file in the project root for more information. 

using System.Collections.Generic;
using System.Linq.Expressions;

namespace System.Linq
{
    /// <summary>
    /// Provides a set of extension methods for asynchronous enumerable sequences represented using expression trees.
    /// </summary>
    [LocalQueryMethodImplementationType(typeof(AsyncEnumerableEx))]
    public static partial class AsyncQueryableEx
    {
        private static Expression GetSourceExpression<TSource>(IAsyncEnumerable<TSource> source)
        {
            if (source is IAsyncQueryable<TSource> queryable)
            {
                return queryable.Expression;
            }

            return Expression.Constant(source, typeof(IAsyncEnumerable<TSource>));
        }
    }
}

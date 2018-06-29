// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the Apache 2.0 License.
// See the LICENSE file in the project root for more information. 

using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;

namespace System.Reactive.Linq
{
    /// <summary>
    /// Provides a set of static methods for writing queries over observable sequences, allowing translation to a target query language.
    /// </summary>
    [LocalQueryMethodImplementationType(typeof(ObservableEx))]
    public static partial class QbservableEx
    {
        internal static Expression GetSourceExpression<TSource>(IObservable<TSource> source)
        {
            if (source is IQbservable<TSource> q)
            {
                return q.Expression;
            }

            return Expression.Constant(source, typeof(IObservable<TSource>));
        }

        internal static Expression GetSourceExpression<TSource>(IEnumerable<TSource> source)
        {
            if (source is IQueryable<TSource> q)
            {
                return q.Expression;
            }

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

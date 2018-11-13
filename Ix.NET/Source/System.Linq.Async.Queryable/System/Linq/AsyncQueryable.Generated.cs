// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the Apache 2.0 License.
// See the LICENSE file in the project root for more information. 

using System.Collections.Generic;
using System.Linq.Expressions;
using System.Reflection;
using System.Threading;
using System.Threading.Tasks;

namespace System.Linq
{
    public static partial class AsyncQueryable
    {
        public static Task<TSource> Aggregate<TSource>(this IAsyncQueryable<TSource> source, Expression<Func<TSource, TSource, Task<TSource>>> accumulator)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));
            if (accumulator == null)
                throw new ArgumentNullException(nameof(accumulator));

#if CRIPPLED_REFLECTION
            return source.Provider.ExecuteAsync<TSource>(Expression.Call(InfoOf(() => AsyncQueryable.Aggregate<TSource>(default(IAsyncQueryable<TSource>), default(Expression<Func<TSource, TSource, Task<TSource>>>))), source.Expression, accumulator), CancellationToken.None);
#else
            return source.Provider.ExecuteAsync<TSource>(Expression.Call(((MethodInfo)MethodBase.GetCurrentMethod()).MakeGenericMethod(typeof(TSource)), source.Expression, accumulator), CancellationToken.None);
#endif
        }

        public static Task<TSource> Aggregate<TSource>(this IAsyncQueryable<TSource> source, Expression<Func<TSource, TSource, TSource>> accumulator)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));
            if (accumulator == null)
                throw new ArgumentNullException(nameof(accumulator));

#if CRIPPLED_REFLECTION
            return source.Provider.ExecuteAsync<TSource>(Expression.Call(InfoOf(() => AsyncQueryable.Aggregate<TSource>(default(IAsyncQueryable<TSource>), default(Expression<Func<TSource, TSource, TSource>>))), source.Expression, accumulator), CancellationToken.None);
#else
            return source.Provider.ExecuteAsync<TSource>(Expression.Call(((MethodInfo)MethodBase.GetCurrentMethod()).MakeGenericMethod(typeof(TSource)), source.Expression, accumulator), CancellationToken.None);
#endif
        }

        public static Task<TSource> Aggregate<TSource>(this IAsyncQueryable<TSource> source, Expression<Func<TSource, TSource, Task<TSource>>> accumulator, CancellationToken cancellationToken)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));
            if (accumulator == null)
                throw new ArgumentNullException(nameof(accumulator));

#if CRIPPLED_REFLECTION
            return source.Provider.ExecuteAsync<TSource>(Expression.Call(InfoOf(() => AsyncQueryable.Aggregate<TSource>(default(IAsyncQueryable<TSource>), default(Expression<Func<TSource, TSource, Task<TSource>>>), default(CancellationToken))), source.Expression, accumulator, Expression.Constant(cancellationToken, typeof(CancellationToken))), cancellationToken);
#else
            return source.Provider.ExecuteAsync<TSource>(Expression.Call(((MethodInfo)MethodBase.GetCurrentMethod()).MakeGenericMethod(typeof(TSource)), source.Expression, accumulator, Expression.Constant(cancellationToken, typeof(CancellationToken))), cancellationToken);
#endif
        }

        public static Task<TSource> Aggregate<TSource>(this IAsyncQueryable<TSource> source, Expression<Func<TSource, TSource, TSource>> accumulator, CancellationToken cancellationToken)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));
            if (accumulator == null)
                throw new ArgumentNullException(nameof(accumulator));

#if CRIPPLED_REFLECTION
            return source.Provider.ExecuteAsync<TSource>(Expression.Call(InfoOf(() => AsyncQueryable.Aggregate<TSource>(default(IAsyncQueryable<TSource>), default(Expression<Func<TSource, TSource, TSource>>), default(CancellationToken))), source.Expression, accumulator, Expression.Constant(cancellationToken, typeof(CancellationToken))), cancellationToken);
#else
            return source.Provider.ExecuteAsync<TSource>(Expression.Call(((MethodInfo)MethodBase.GetCurrentMethod()).MakeGenericMethod(typeof(TSource)), source.Expression, accumulator, Expression.Constant(cancellationToken, typeof(CancellationToken))), cancellationToken);
#endif
        }

        public static Task<TAccumulate> Aggregate<TSource, TAccumulate>(this IAsyncQueryable<TSource> source, TAccumulate seed, Expression<Func<TAccumulate, TSource, TAccumulate>> accumulator)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));
            if (accumulator == null)
                throw new ArgumentNullException(nameof(accumulator));

#if CRIPPLED_REFLECTION
            return source.Provider.ExecuteAsync<TAccumulate>(Expression.Call(InfoOf(() => AsyncQueryable.Aggregate<TSource, TAccumulate>(default(IAsyncQueryable<TSource>), default(TAccumulate), default(Expression<Func<TAccumulate, TSource, TAccumulate>>))), source.Expression, Expression.Constant(seed, typeof(TAccumulate)), accumulator), CancellationToken.None);
#else
            return source.Provider.ExecuteAsync<TAccumulate>(Expression.Call(((MethodInfo)MethodBase.GetCurrentMethod()).MakeGenericMethod(typeof(TSource), typeof(TAccumulate)), source.Expression, Expression.Constant(seed, typeof(TAccumulate)), accumulator), CancellationToken.None);
#endif
        }

        public static Task<TAccumulate> Aggregate<TSource, TAccumulate>(this IAsyncQueryable<TSource> source, TAccumulate seed, Expression<Func<TAccumulate, TSource, Task<TAccumulate>>> accumulator)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));
            if (accumulator == null)
                throw new ArgumentNullException(nameof(accumulator));

#if CRIPPLED_REFLECTION
            return source.Provider.ExecuteAsync<TAccumulate>(Expression.Call(InfoOf(() => AsyncQueryable.Aggregate<TSource, TAccumulate>(default(IAsyncQueryable<TSource>), default(TAccumulate), default(Expression<Func<TAccumulate, TSource, Task<TAccumulate>>>))), source.Expression, Expression.Constant(seed, typeof(TAccumulate)), accumulator), CancellationToken.None);
#else
            return source.Provider.ExecuteAsync<TAccumulate>(Expression.Call(((MethodInfo)MethodBase.GetCurrentMethod()).MakeGenericMethod(typeof(TSource), typeof(TAccumulate)), source.Expression, Expression.Constant(seed, typeof(TAccumulate)), accumulator), CancellationToken.None);
#endif
        }

        public static Task<TAccumulate> Aggregate<TSource, TAccumulate>(this IAsyncQueryable<TSource> source, TAccumulate seed, Expression<Func<TAccumulate, TSource, TAccumulate>> accumulator, CancellationToken cancellationToken)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));
            if (accumulator == null)
                throw new ArgumentNullException(nameof(accumulator));

#if CRIPPLED_REFLECTION
            return source.Provider.ExecuteAsync<TAccumulate>(Expression.Call(InfoOf(() => AsyncQueryable.Aggregate<TSource, TAccumulate>(default(IAsyncQueryable<TSource>), default(TAccumulate), default(Expression<Func<TAccumulate, TSource, TAccumulate>>), default(CancellationToken))), source.Expression, Expression.Constant(seed, typeof(TAccumulate)), accumulator, Expression.Constant(cancellationToken, typeof(CancellationToken))), cancellationToken);
#else
            return source.Provider.ExecuteAsync<TAccumulate>(Expression.Call(((MethodInfo)MethodBase.GetCurrentMethod()).MakeGenericMethod(typeof(TSource), typeof(TAccumulate)), source.Expression, Expression.Constant(seed, typeof(TAccumulate)), accumulator, Expression.Constant(cancellationToken, typeof(CancellationToken))), cancellationToken);
#endif
        }

        public static Task<TAccumulate> Aggregate<TSource, TAccumulate>(this IAsyncQueryable<TSource> source, TAccumulate seed, Expression<Func<TAccumulate, TSource, Task<TAccumulate>>> accumulator, CancellationToken cancellationToken)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));
            if (accumulator == null)
                throw new ArgumentNullException(nameof(accumulator));

#if CRIPPLED_REFLECTION
            return source.Provider.ExecuteAsync<TAccumulate>(Expression.Call(InfoOf(() => AsyncQueryable.Aggregate<TSource, TAccumulate>(default(IAsyncQueryable<TSource>), default(TAccumulate), default(Expression<Func<TAccumulate, TSource, Task<TAccumulate>>>), default(CancellationToken))), source.Expression, Expression.Constant(seed, typeof(TAccumulate)), accumulator, Expression.Constant(cancellationToken, typeof(CancellationToken))), cancellationToken);
#else
            return source.Provider.ExecuteAsync<TAccumulate>(Expression.Call(((MethodInfo)MethodBase.GetCurrentMethod()).MakeGenericMethod(typeof(TSource), typeof(TAccumulate)), source.Expression, Expression.Constant(seed, typeof(TAccumulate)), accumulator, Expression.Constant(cancellationToken, typeof(CancellationToken))), cancellationToken);
#endif
        }

        public static Task<TResult> Aggregate<TSource, TAccumulate, TResult>(this IAsyncQueryable<TSource> source, TAccumulate seed, Expression<Func<TAccumulate, TSource, TAccumulate>> accumulator, Expression<Func<TAccumulate, TResult>> resultSelector)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));
            if (accumulator == null)
                throw new ArgumentNullException(nameof(accumulator));
            if (resultSelector == null)
                throw new ArgumentNullException(nameof(resultSelector));

#if CRIPPLED_REFLECTION
            return source.Provider.ExecuteAsync<TResult>(Expression.Call(InfoOf(() => AsyncQueryable.Aggregate<TSource, TAccumulate, TResult>(default(IAsyncQueryable<TSource>), default(TAccumulate), default(Expression<Func<TAccumulate, TSource, TAccumulate>>), default(Expression<Func<TAccumulate, TResult>>))), source.Expression, Expression.Constant(seed, typeof(TAccumulate)), accumulator, resultSelector), CancellationToken.None);
#else
            return source.Provider.ExecuteAsync<TResult>(Expression.Call(((MethodInfo)MethodBase.GetCurrentMethod()).MakeGenericMethod(typeof(TSource), typeof(TAccumulate), typeof(TResult)), source.Expression, Expression.Constant(seed, typeof(TAccumulate)), accumulator, resultSelector), CancellationToken.None);
#endif
        }

        public static Task<TResult> Aggregate<TSource, TAccumulate, TResult>(this IAsyncQueryable<TSource> source, TAccumulate seed, Expression<Func<TAccumulate, TSource, Task<TAccumulate>>> accumulator, Expression<Func<TAccumulate, Task<TResult>>> resultSelector)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));
            if (accumulator == null)
                throw new ArgumentNullException(nameof(accumulator));
            if (resultSelector == null)
                throw new ArgumentNullException(nameof(resultSelector));

#if CRIPPLED_REFLECTION
            return source.Provider.ExecuteAsync<TResult>(Expression.Call(InfoOf(() => AsyncQueryable.Aggregate<TSource, TAccumulate, TResult>(default(IAsyncQueryable<TSource>), default(TAccumulate), default(Expression<Func<TAccumulate, TSource, Task<TAccumulate>>>), default(Expression<Func<TAccumulate, Task<TResult>>>))), source.Expression, Expression.Constant(seed, typeof(TAccumulate)), accumulator, resultSelector), CancellationToken.None);
#else
            return source.Provider.ExecuteAsync<TResult>(Expression.Call(((MethodInfo)MethodBase.GetCurrentMethod()).MakeGenericMethod(typeof(TSource), typeof(TAccumulate), typeof(TResult)), source.Expression, Expression.Constant(seed, typeof(TAccumulate)), accumulator, resultSelector), CancellationToken.None);
#endif
        }

        public static Task<TResult> Aggregate<TSource, TAccumulate, TResult>(this IAsyncQueryable<TSource> source, TAccumulate seed, Expression<Func<TAccumulate, TSource, TAccumulate>> accumulator, Expression<Func<TAccumulate, TResult>> resultSelector, CancellationToken cancellationToken)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));
            if (accumulator == null)
                throw new ArgumentNullException(nameof(accumulator));
            if (resultSelector == null)
                throw new ArgumentNullException(nameof(resultSelector));

#if CRIPPLED_REFLECTION
            return source.Provider.ExecuteAsync<TResult>(Expression.Call(InfoOf(() => AsyncQueryable.Aggregate<TSource, TAccumulate, TResult>(default(IAsyncQueryable<TSource>), default(TAccumulate), default(Expression<Func<TAccumulate, TSource, TAccumulate>>), default(Expression<Func<TAccumulate, TResult>>), default(CancellationToken))), source.Expression, Expression.Constant(seed, typeof(TAccumulate)), accumulator, resultSelector, Expression.Constant(cancellationToken, typeof(CancellationToken))), cancellationToken);
#else
            return source.Provider.ExecuteAsync<TResult>(Expression.Call(((MethodInfo)MethodBase.GetCurrentMethod()).MakeGenericMethod(typeof(TSource), typeof(TAccumulate), typeof(TResult)), source.Expression, Expression.Constant(seed, typeof(TAccumulate)), accumulator, resultSelector, Expression.Constant(cancellationToken, typeof(CancellationToken))), cancellationToken);
#endif
        }

        public static Task<TResult> Aggregate<TSource, TAccumulate, TResult>(this IAsyncQueryable<TSource> source, TAccumulate seed, Expression<Func<TAccumulate, TSource, Task<TAccumulate>>> accumulator, Expression<Func<TAccumulate, Task<TResult>>> resultSelector, CancellationToken cancellationToken)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));
            if (accumulator == null)
                throw new ArgumentNullException(nameof(accumulator));
            if (resultSelector == null)
                throw new ArgumentNullException(nameof(resultSelector));

#if CRIPPLED_REFLECTION
            return source.Provider.ExecuteAsync<TResult>(Expression.Call(InfoOf(() => AsyncQueryable.Aggregate<TSource, TAccumulate, TResult>(default(IAsyncQueryable<TSource>), default(TAccumulate), default(Expression<Func<TAccumulate, TSource, Task<TAccumulate>>>), default(Expression<Func<TAccumulate, Task<TResult>>>), default(CancellationToken))), source.Expression, Expression.Constant(seed, typeof(TAccumulate)), accumulator, resultSelector, Expression.Constant(cancellationToken, typeof(CancellationToken))), cancellationToken);
#else
            return source.Provider.ExecuteAsync<TResult>(Expression.Call(((MethodInfo)MethodBase.GetCurrentMethod()).MakeGenericMethod(typeof(TSource), typeof(TAccumulate), typeof(TResult)), source.Expression, Expression.Constant(seed, typeof(TAccumulate)), accumulator, resultSelector, Expression.Constant(cancellationToken, typeof(CancellationToken))), cancellationToken);
#endif
        }

        public static Task<bool> All<TSource>(this IAsyncQueryable<TSource> source, Expression<Func<TSource, bool>> predicate)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));
            if (predicate == null)
                throw new ArgumentNullException(nameof(predicate));

#if CRIPPLED_REFLECTION
            return source.Provider.ExecuteAsync<bool>(Expression.Call(InfoOf(() => AsyncQueryable.All<TSource>(default(IAsyncQueryable<TSource>), default(Expression<Func<TSource, bool>>))), source.Expression, predicate), CancellationToken.None);
#else
            return source.Provider.ExecuteAsync<bool>(Expression.Call(((MethodInfo)MethodBase.GetCurrentMethod()).MakeGenericMethod(typeof(TSource)), source.Expression, predicate), CancellationToken.None);
#endif
        }

        public static Task<bool> All<TSource>(this IAsyncQueryable<TSource> source, Expression<Func<TSource, Task<bool>>> predicate)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));
            if (predicate == null)
                throw new ArgumentNullException(nameof(predicate));

#if CRIPPLED_REFLECTION
            return source.Provider.ExecuteAsync<bool>(Expression.Call(InfoOf(() => AsyncQueryable.All<TSource>(default(IAsyncQueryable<TSource>), default(Expression<Func<TSource, Task<bool>>>))), source.Expression, predicate), CancellationToken.None);
#else
            return source.Provider.ExecuteAsync<bool>(Expression.Call(((MethodInfo)MethodBase.GetCurrentMethod()).MakeGenericMethod(typeof(TSource)), source.Expression, predicate), CancellationToken.None);
#endif
        }

        public static Task<bool> All<TSource>(this IAsyncQueryable<TSource> source, Expression<Func<TSource, bool>> predicate, CancellationToken cancellationToken)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));
            if (predicate == null)
                throw new ArgumentNullException(nameof(predicate));

#if CRIPPLED_REFLECTION
            return source.Provider.ExecuteAsync<bool>(Expression.Call(InfoOf(() => AsyncQueryable.All<TSource>(default(IAsyncQueryable<TSource>), default(Expression<Func<TSource, bool>>), default(CancellationToken))), source.Expression, predicate, Expression.Constant(cancellationToken, typeof(CancellationToken))), cancellationToken);
#else
            return source.Provider.ExecuteAsync<bool>(Expression.Call(((MethodInfo)MethodBase.GetCurrentMethod()).MakeGenericMethod(typeof(TSource)), source.Expression, predicate, Expression.Constant(cancellationToken, typeof(CancellationToken))), cancellationToken);
#endif
        }

        public static Task<bool> All<TSource>(this IAsyncQueryable<TSource> source, Expression<Func<TSource, Task<bool>>> predicate, CancellationToken cancellationToken)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));
            if (predicate == null)
                throw new ArgumentNullException(nameof(predicate));

#if CRIPPLED_REFLECTION
            return source.Provider.ExecuteAsync<bool>(Expression.Call(InfoOf(() => AsyncQueryable.All<TSource>(default(IAsyncQueryable<TSource>), default(Expression<Func<TSource, Task<bool>>>), default(CancellationToken))), source.Expression, predicate, Expression.Constant(cancellationToken, typeof(CancellationToken))), cancellationToken);
#else
            return source.Provider.ExecuteAsync<bool>(Expression.Call(((MethodInfo)MethodBase.GetCurrentMethod()).MakeGenericMethod(typeof(TSource)), source.Expression, predicate, Expression.Constant(cancellationToken, typeof(CancellationToken))), cancellationToken);
#endif
        }

        public static Task<bool> Any<TSource>(this IAsyncQueryable<TSource> source)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));

#if CRIPPLED_REFLECTION
            return source.Provider.ExecuteAsync<bool>(Expression.Call(InfoOf(() => AsyncQueryable.Any<TSource>(default(IAsyncQueryable<TSource>))), source.Expression), CancellationToken.None);
#else
            return source.Provider.ExecuteAsync<bool>(Expression.Call(((MethodInfo)MethodBase.GetCurrentMethod()).MakeGenericMethod(typeof(TSource)), source.Expression), CancellationToken.None);
#endif
        }

        public static Task<bool> Any<TSource>(this IAsyncQueryable<TSource> source, CancellationToken cancellationToken)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));

#if CRIPPLED_REFLECTION
            return source.Provider.ExecuteAsync<bool>(Expression.Call(InfoOf(() => AsyncQueryable.Any<TSource>(default(IAsyncQueryable<TSource>), default(CancellationToken))), source.Expression, Expression.Constant(cancellationToken, typeof(CancellationToken))), cancellationToken);
#else
            return source.Provider.ExecuteAsync<bool>(Expression.Call(((MethodInfo)MethodBase.GetCurrentMethod()).MakeGenericMethod(typeof(TSource)), source.Expression, Expression.Constant(cancellationToken, typeof(CancellationToken))), cancellationToken);
#endif
        }

        public static Task<bool> Any<TSource>(this IAsyncQueryable<TSource> source, Expression<Func<TSource, bool>> predicate)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));
            if (predicate == null)
                throw new ArgumentNullException(nameof(predicate));

#if CRIPPLED_REFLECTION
            return source.Provider.ExecuteAsync<bool>(Expression.Call(InfoOf(() => AsyncQueryable.Any<TSource>(default(IAsyncQueryable<TSource>), default(Expression<Func<TSource, bool>>))), source.Expression, predicate), CancellationToken.None);
#else
            return source.Provider.ExecuteAsync<bool>(Expression.Call(((MethodInfo)MethodBase.GetCurrentMethod()).MakeGenericMethod(typeof(TSource)), source.Expression, predicate), CancellationToken.None);
#endif
        }

        public static Task<bool> Any<TSource>(this IAsyncQueryable<TSource> source, Expression<Func<TSource, Task<bool>>> predicate)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));
            if (predicate == null)
                throw new ArgumentNullException(nameof(predicate));

#if CRIPPLED_REFLECTION
            return source.Provider.ExecuteAsync<bool>(Expression.Call(InfoOf(() => AsyncQueryable.Any<TSource>(default(IAsyncQueryable<TSource>), default(Expression<Func<TSource, Task<bool>>>))), source.Expression, predicate), CancellationToken.None);
#else
            return source.Provider.ExecuteAsync<bool>(Expression.Call(((MethodInfo)MethodBase.GetCurrentMethod()).MakeGenericMethod(typeof(TSource)), source.Expression, predicate), CancellationToken.None);
#endif
        }

        public static Task<bool> Any<TSource>(this IAsyncQueryable<TSource> source, Expression<Func<TSource, bool>> predicate, CancellationToken cancellationToken)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));
            if (predicate == null)
                throw new ArgumentNullException(nameof(predicate));

#if CRIPPLED_REFLECTION
            return source.Provider.ExecuteAsync<bool>(Expression.Call(InfoOf(() => AsyncQueryable.Any<TSource>(default(IAsyncQueryable<TSource>), default(Expression<Func<TSource, bool>>), default(CancellationToken))), source.Expression, predicate, Expression.Constant(cancellationToken, typeof(CancellationToken))), cancellationToken);
#else
            return source.Provider.ExecuteAsync<bool>(Expression.Call(((MethodInfo)MethodBase.GetCurrentMethod()).MakeGenericMethod(typeof(TSource)), source.Expression, predicate, Expression.Constant(cancellationToken, typeof(CancellationToken))), cancellationToken);
#endif
        }

        public static Task<bool> Any<TSource>(this IAsyncQueryable<TSource> source, Expression<Func<TSource, Task<bool>>> predicate, CancellationToken cancellationToken)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));
            if (predicate == null)
                throw new ArgumentNullException(nameof(predicate));

#if CRIPPLED_REFLECTION
            return source.Provider.ExecuteAsync<bool>(Expression.Call(InfoOf(() => AsyncQueryable.Any<TSource>(default(IAsyncQueryable<TSource>), default(Expression<Func<TSource, Task<bool>>>), default(CancellationToken))), source.Expression, predicate, Expression.Constant(cancellationToken, typeof(CancellationToken))), cancellationToken);
#else
            return source.Provider.ExecuteAsync<bool>(Expression.Call(((MethodInfo)MethodBase.GetCurrentMethod()).MakeGenericMethod(typeof(TSource)), source.Expression, predicate, Expression.Constant(cancellationToken, typeof(CancellationToken))), cancellationToken);
#endif
        }

        public static IAsyncQueryable<TSource> Append<TSource>(this IAsyncQueryable<TSource> source, TSource element)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));

#if CRIPPLED_REFLECTION
            return source.Provider.CreateQuery<TSource>(Expression.Call(InfoOf(() => AsyncQueryable.Append<TSource>(default(IAsyncQueryable<TSource>), default(TSource))), source.Expression, Expression.Constant(element, typeof(TSource))));
#else
            return source.Provider.CreateQuery<TSource>(Expression.Call(((MethodInfo)MethodBase.GetCurrentMethod()).MakeGenericMethod(typeof(TSource)), source.Expression, Expression.Constant(element, typeof(TSource))));
#endif
        }

        public static Task<decimal?> Average(this IAsyncQueryable<decimal?> source)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));

#if CRIPPLED_REFLECTION
            return source.Provider.ExecuteAsync<decimal?>(Expression.Call(InfoOf(() => AsyncQueryable.Average(default(IAsyncQueryable<decimal?>))), source.Expression), CancellationToken.None);
#else
            return source.Provider.ExecuteAsync<decimal?>(Expression.Call((MethodInfo)MethodBase.GetCurrentMethod(), source.Expression), CancellationToken.None);
#endif
        }

        public static Task<decimal> Average(this IAsyncQueryable<decimal> source)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));

#if CRIPPLED_REFLECTION
            return source.Provider.ExecuteAsync<decimal>(Expression.Call(InfoOf(() => AsyncQueryable.Average(default(IAsyncQueryable<decimal>))), source.Expression), CancellationToken.None);
#else
            return source.Provider.ExecuteAsync<decimal>(Expression.Call((MethodInfo)MethodBase.GetCurrentMethod(), source.Expression), CancellationToken.None);
#endif
        }

        public static Task<double?> Average(this IAsyncQueryable<double?> source)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));

#if CRIPPLED_REFLECTION
            return source.Provider.ExecuteAsync<double?>(Expression.Call(InfoOf(() => AsyncQueryable.Average(default(IAsyncQueryable<double?>))), source.Expression), CancellationToken.None);
#else
            return source.Provider.ExecuteAsync<double?>(Expression.Call((MethodInfo)MethodBase.GetCurrentMethod(), source.Expression), CancellationToken.None);
#endif
        }

        public static Task<double> Average(this IAsyncQueryable<double> source)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));

#if CRIPPLED_REFLECTION
            return source.Provider.ExecuteAsync<double>(Expression.Call(InfoOf(() => AsyncQueryable.Average(default(IAsyncQueryable<double>))), source.Expression), CancellationToken.None);
#else
            return source.Provider.ExecuteAsync<double>(Expression.Call((MethodInfo)MethodBase.GetCurrentMethod(), source.Expression), CancellationToken.None);
#endif
        }

        public static Task<float?> Average(this IAsyncQueryable<float?> source)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));

#if CRIPPLED_REFLECTION
            return source.Provider.ExecuteAsync<float?>(Expression.Call(InfoOf(() => AsyncQueryable.Average(default(IAsyncQueryable<float?>))), source.Expression), CancellationToken.None);
#else
            return source.Provider.ExecuteAsync<float?>(Expression.Call((MethodInfo)MethodBase.GetCurrentMethod(), source.Expression), CancellationToken.None);
#endif
        }

        public static Task<float> Average(this IAsyncQueryable<float> source)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));

#if CRIPPLED_REFLECTION
            return source.Provider.ExecuteAsync<float>(Expression.Call(InfoOf(() => AsyncQueryable.Average(default(IAsyncQueryable<float>))), source.Expression), CancellationToken.None);
#else
            return source.Provider.ExecuteAsync<float>(Expression.Call((MethodInfo)MethodBase.GetCurrentMethod(), source.Expression), CancellationToken.None);
#endif
        }

        public static Task<double?> Average(this IAsyncQueryable<int?> source)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));

#if CRIPPLED_REFLECTION
            return source.Provider.ExecuteAsync<double?>(Expression.Call(InfoOf(() => AsyncQueryable.Average(default(IAsyncQueryable<int?>))), source.Expression), CancellationToken.None);
#else
            return source.Provider.ExecuteAsync<double?>(Expression.Call((MethodInfo)MethodBase.GetCurrentMethod(), source.Expression), CancellationToken.None);
#endif
        }

        public static Task<double> Average(this IAsyncQueryable<int> source)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));

#if CRIPPLED_REFLECTION
            return source.Provider.ExecuteAsync<double>(Expression.Call(InfoOf(() => AsyncQueryable.Average(default(IAsyncQueryable<int>))), source.Expression), CancellationToken.None);
#else
            return source.Provider.ExecuteAsync<double>(Expression.Call((MethodInfo)MethodBase.GetCurrentMethod(), source.Expression), CancellationToken.None);
#endif
        }

        public static Task<double?> Average(this IAsyncQueryable<long?> source)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));

#if CRIPPLED_REFLECTION
            return source.Provider.ExecuteAsync<double?>(Expression.Call(InfoOf(() => AsyncQueryable.Average(default(IAsyncQueryable<long?>))), source.Expression), CancellationToken.None);
#else
            return source.Provider.ExecuteAsync<double?>(Expression.Call((MethodInfo)MethodBase.GetCurrentMethod(), source.Expression), CancellationToken.None);
#endif
        }

        public static Task<double> Average(this IAsyncQueryable<long> source)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));

#if CRIPPLED_REFLECTION
            return source.Provider.ExecuteAsync<double>(Expression.Call(InfoOf(() => AsyncQueryable.Average(default(IAsyncQueryable<long>))), source.Expression), CancellationToken.None);
#else
            return source.Provider.ExecuteAsync<double>(Expression.Call((MethodInfo)MethodBase.GetCurrentMethod(), source.Expression), CancellationToken.None);
#endif
        }

        public static Task<decimal?> Average(this IAsyncQueryable<decimal?> source, CancellationToken cancellationToken)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));

#if CRIPPLED_REFLECTION
            return source.Provider.ExecuteAsync<decimal?>(Expression.Call(InfoOf(() => AsyncQueryable.Average(default(IAsyncQueryable<decimal?>), default(CancellationToken))), source.Expression, Expression.Constant(cancellationToken, typeof(CancellationToken))), cancellationToken);
#else
            return source.Provider.ExecuteAsync<decimal?>(Expression.Call((MethodInfo)MethodBase.GetCurrentMethod(), source.Expression, Expression.Constant(cancellationToken, typeof(CancellationToken))), cancellationToken);
#endif
        }

        public static Task<decimal> Average(this IAsyncQueryable<decimal> source, CancellationToken cancellationToken)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));

#if CRIPPLED_REFLECTION
            return source.Provider.ExecuteAsync<decimal>(Expression.Call(InfoOf(() => AsyncQueryable.Average(default(IAsyncQueryable<decimal>), default(CancellationToken))), source.Expression, Expression.Constant(cancellationToken, typeof(CancellationToken))), cancellationToken);
#else
            return source.Provider.ExecuteAsync<decimal>(Expression.Call((MethodInfo)MethodBase.GetCurrentMethod(), source.Expression, Expression.Constant(cancellationToken, typeof(CancellationToken))), cancellationToken);
#endif
        }

        public static Task<double?> Average(this IAsyncQueryable<double?> source, CancellationToken cancellationToken)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));

#if CRIPPLED_REFLECTION
            return source.Provider.ExecuteAsync<double?>(Expression.Call(InfoOf(() => AsyncQueryable.Average(default(IAsyncQueryable<double?>), default(CancellationToken))), source.Expression, Expression.Constant(cancellationToken, typeof(CancellationToken))), cancellationToken);
#else
            return source.Provider.ExecuteAsync<double?>(Expression.Call((MethodInfo)MethodBase.GetCurrentMethod(), source.Expression, Expression.Constant(cancellationToken, typeof(CancellationToken))), cancellationToken);
#endif
        }

        public static Task<double> Average(this IAsyncQueryable<double> source, CancellationToken cancellationToken)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));

#if CRIPPLED_REFLECTION
            return source.Provider.ExecuteAsync<double>(Expression.Call(InfoOf(() => AsyncQueryable.Average(default(IAsyncQueryable<double>), default(CancellationToken))), source.Expression, Expression.Constant(cancellationToken, typeof(CancellationToken))), cancellationToken);
#else
            return source.Provider.ExecuteAsync<double>(Expression.Call((MethodInfo)MethodBase.GetCurrentMethod(), source.Expression, Expression.Constant(cancellationToken, typeof(CancellationToken))), cancellationToken);
#endif
        }

        public static Task<float?> Average(this IAsyncQueryable<float?> source, CancellationToken cancellationToken)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));

#if CRIPPLED_REFLECTION
            return source.Provider.ExecuteAsync<float?>(Expression.Call(InfoOf(() => AsyncQueryable.Average(default(IAsyncQueryable<float?>), default(CancellationToken))), source.Expression, Expression.Constant(cancellationToken, typeof(CancellationToken))), cancellationToken);
#else
            return source.Provider.ExecuteAsync<float?>(Expression.Call((MethodInfo)MethodBase.GetCurrentMethod(), source.Expression, Expression.Constant(cancellationToken, typeof(CancellationToken))), cancellationToken);
#endif
        }

        public static Task<float> Average(this IAsyncQueryable<float> source, CancellationToken cancellationToken)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));

#if CRIPPLED_REFLECTION
            return source.Provider.ExecuteAsync<float>(Expression.Call(InfoOf(() => AsyncQueryable.Average(default(IAsyncQueryable<float>), default(CancellationToken))), source.Expression, Expression.Constant(cancellationToken, typeof(CancellationToken))), cancellationToken);
#else
            return source.Provider.ExecuteAsync<float>(Expression.Call((MethodInfo)MethodBase.GetCurrentMethod(), source.Expression, Expression.Constant(cancellationToken, typeof(CancellationToken))), cancellationToken);
#endif
        }

        public static Task<double?> Average(this IAsyncQueryable<int?> source, CancellationToken cancellationToken)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));

#if CRIPPLED_REFLECTION
            return source.Provider.ExecuteAsync<double?>(Expression.Call(InfoOf(() => AsyncQueryable.Average(default(IAsyncQueryable<int?>), default(CancellationToken))), source.Expression, Expression.Constant(cancellationToken, typeof(CancellationToken))), cancellationToken);
#else
            return source.Provider.ExecuteAsync<double?>(Expression.Call((MethodInfo)MethodBase.GetCurrentMethod(), source.Expression, Expression.Constant(cancellationToken, typeof(CancellationToken))), cancellationToken);
#endif
        }

        public static Task<double> Average(this IAsyncQueryable<int> source, CancellationToken cancellationToken)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));

#if CRIPPLED_REFLECTION
            return source.Provider.ExecuteAsync<double>(Expression.Call(InfoOf(() => AsyncQueryable.Average(default(IAsyncQueryable<int>), default(CancellationToken))), source.Expression, Expression.Constant(cancellationToken, typeof(CancellationToken))), cancellationToken);
#else
            return source.Provider.ExecuteAsync<double>(Expression.Call((MethodInfo)MethodBase.GetCurrentMethod(), source.Expression, Expression.Constant(cancellationToken, typeof(CancellationToken))), cancellationToken);
#endif
        }

        public static Task<double?> Average(this IAsyncQueryable<long?> source, CancellationToken cancellationToken)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));

#if CRIPPLED_REFLECTION
            return source.Provider.ExecuteAsync<double?>(Expression.Call(InfoOf(() => AsyncQueryable.Average(default(IAsyncQueryable<long?>), default(CancellationToken))), source.Expression, Expression.Constant(cancellationToken, typeof(CancellationToken))), cancellationToken);
#else
            return source.Provider.ExecuteAsync<double?>(Expression.Call((MethodInfo)MethodBase.GetCurrentMethod(), source.Expression, Expression.Constant(cancellationToken, typeof(CancellationToken))), cancellationToken);
#endif
        }

        public static Task<double> Average(this IAsyncQueryable<long> source, CancellationToken cancellationToken)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));

#if CRIPPLED_REFLECTION
            return source.Provider.ExecuteAsync<double>(Expression.Call(InfoOf(() => AsyncQueryable.Average(default(IAsyncQueryable<long>), default(CancellationToken))), source.Expression, Expression.Constant(cancellationToken, typeof(CancellationToken))), cancellationToken);
#else
            return source.Provider.ExecuteAsync<double>(Expression.Call((MethodInfo)MethodBase.GetCurrentMethod(), source.Expression, Expression.Constant(cancellationToken, typeof(CancellationToken))), cancellationToken);
#endif
        }

        public static Task<decimal?> Average<TSource>(this IAsyncQueryable<TSource> source, Expression<Func<TSource, decimal?>> selector)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));
            if (selector == null)
                throw new ArgumentNullException(nameof(selector));

#if CRIPPLED_REFLECTION
            return source.Provider.ExecuteAsync<decimal?>(Expression.Call(InfoOf(() => AsyncQueryable.Average<TSource>(default(IAsyncQueryable<TSource>), default(Expression<Func<TSource, decimal?>>))), source.Expression, selector), CancellationToken.None);
#else
            return source.Provider.ExecuteAsync<decimal?>(Expression.Call(((MethodInfo)MethodBase.GetCurrentMethod()).MakeGenericMethod(typeof(TSource)), source.Expression, selector), CancellationToken.None);
#endif
        }

        public static Task<decimal> Average<TSource>(this IAsyncQueryable<TSource> source, Expression<Func<TSource, decimal>> selector)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));
            if (selector == null)
                throw new ArgumentNullException(nameof(selector));

#if CRIPPLED_REFLECTION
            return source.Provider.ExecuteAsync<decimal>(Expression.Call(InfoOf(() => AsyncQueryable.Average<TSource>(default(IAsyncQueryable<TSource>), default(Expression<Func<TSource, decimal>>))), source.Expression, selector), CancellationToken.None);
#else
            return source.Provider.ExecuteAsync<decimal>(Expression.Call(((MethodInfo)MethodBase.GetCurrentMethod()).MakeGenericMethod(typeof(TSource)), source.Expression, selector), CancellationToken.None);
#endif
        }

        public static Task<double?> Average<TSource>(this IAsyncQueryable<TSource> source, Expression<Func<TSource, double?>> selector)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));
            if (selector == null)
                throw new ArgumentNullException(nameof(selector));

#if CRIPPLED_REFLECTION
            return source.Provider.ExecuteAsync<double?>(Expression.Call(InfoOf(() => AsyncQueryable.Average<TSource>(default(IAsyncQueryable<TSource>), default(Expression<Func<TSource, double?>>))), source.Expression, selector), CancellationToken.None);
#else
            return source.Provider.ExecuteAsync<double?>(Expression.Call(((MethodInfo)MethodBase.GetCurrentMethod()).MakeGenericMethod(typeof(TSource)), source.Expression, selector), CancellationToken.None);
#endif
        }

        public static Task<double> Average<TSource>(this IAsyncQueryable<TSource> source, Expression<Func<TSource, double>> selector)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));
            if (selector == null)
                throw new ArgumentNullException(nameof(selector));

#if CRIPPLED_REFLECTION
            return source.Provider.ExecuteAsync<double>(Expression.Call(InfoOf(() => AsyncQueryable.Average<TSource>(default(IAsyncQueryable<TSource>), default(Expression<Func<TSource, double>>))), source.Expression, selector), CancellationToken.None);
#else
            return source.Provider.ExecuteAsync<double>(Expression.Call(((MethodInfo)MethodBase.GetCurrentMethod()).MakeGenericMethod(typeof(TSource)), source.Expression, selector), CancellationToken.None);
#endif
        }

        public static Task<float?> Average<TSource>(this IAsyncQueryable<TSource> source, Expression<Func<TSource, float?>> selector)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));
            if (selector == null)
                throw new ArgumentNullException(nameof(selector));

#if CRIPPLED_REFLECTION
            return source.Provider.ExecuteAsync<float?>(Expression.Call(InfoOf(() => AsyncQueryable.Average<TSource>(default(IAsyncQueryable<TSource>), default(Expression<Func<TSource, float?>>))), source.Expression, selector), CancellationToken.None);
#else
            return source.Provider.ExecuteAsync<float?>(Expression.Call(((MethodInfo)MethodBase.GetCurrentMethod()).MakeGenericMethod(typeof(TSource)), source.Expression, selector), CancellationToken.None);
#endif
        }

        public static Task<float> Average<TSource>(this IAsyncQueryable<TSource> source, Expression<Func<TSource, float>> selector)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));
            if (selector == null)
                throw new ArgumentNullException(nameof(selector));

#if CRIPPLED_REFLECTION
            return source.Provider.ExecuteAsync<float>(Expression.Call(InfoOf(() => AsyncQueryable.Average<TSource>(default(IAsyncQueryable<TSource>), default(Expression<Func<TSource, float>>))), source.Expression, selector), CancellationToken.None);
#else
            return source.Provider.ExecuteAsync<float>(Expression.Call(((MethodInfo)MethodBase.GetCurrentMethod()).MakeGenericMethod(typeof(TSource)), source.Expression, selector), CancellationToken.None);
#endif
        }

        public static Task<double?> Average<TSource>(this IAsyncQueryable<TSource> source, Expression<Func<TSource, int?>> selector)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));
            if (selector == null)
                throw new ArgumentNullException(nameof(selector));

#if CRIPPLED_REFLECTION
            return source.Provider.ExecuteAsync<double?>(Expression.Call(InfoOf(() => AsyncQueryable.Average<TSource>(default(IAsyncQueryable<TSource>), default(Expression<Func<TSource, int?>>))), source.Expression, selector), CancellationToken.None);
#else
            return source.Provider.ExecuteAsync<double?>(Expression.Call(((MethodInfo)MethodBase.GetCurrentMethod()).MakeGenericMethod(typeof(TSource)), source.Expression, selector), CancellationToken.None);
#endif
        }

        public static Task<double> Average<TSource>(this IAsyncQueryable<TSource> source, Expression<Func<TSource, int>> selector)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));
            if (selector == null)
                throw new ArgumentNullException(nameof(selector));

#if CRIPPLED_REFLECTION
            return source.Provider.ExecuteAsync<double>(Expression.Call(InfoOf(() => AsyncQueryable.Average<TSource>(default(IAsyncQueryable<TSource>), default(Expression<Func<TSource, int>>))), source.Expression, selector), CancellationToken.None);
#else
            return source.Provider.ExecuteAsync<double>(Expression.Call(((MethodInfo)MethodBase.GetCurrentMethod()).MakeGenericMethod(typeof(TSource)), source.Expression, selector), CancellationToken.None);
#endif
        }

        public static Task<double?> Average<TSource>(this IAsyncQueryable<TSource> source, Expression<Func<TSource, long?>> selector)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));
            if (selector == null)
                throw new ArgumentNullException(nameof(selector));

#if CRIPPLED_REFLECTION
            return source.Provider.ExecuteAsync<double?>(Expression.Call(InfoOf(() => AsyncQueryable.Average<TSource>(default(IAsyncQueryable<TSource>), default(Expression<Func<TSource, long?>>))), source.Expression, selector), CancellationToken.None);
#else
            return source.Provider.ExecuteAsync<double?>(Expression.Call(((MethodInfo)MethodBase.GetCurrentMethod()).MakeGenericMethod(typeof(TSource)), source.Expression, selector), CancellationToken.None);
#endif
        }

        public static Task<double> Average<TSource>(this IAsyncQueryable<TSource> source, Expression<Func<TSource, long>> selector)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));
            if (selector == null)
                throw new ArgumentNullException(nameof(selector));

#if CRIPPLED_REFLECTION
            return source.Provider.ExecuteAsync<double>(Expression.Call(InfoOf(() => AsyncQueryable.Average<TSource>(default(IAsyncQueryable<TSource>), default(Expression<Func<TSource, long>>))), source.Expression, selector), CancellationToken.None);
#else
            return source.Provider.ExecuteAsync<double>(Expression.Call(((MethodInfo)MethodBase.GetCurrentMethod()).MakeGenericMethod(typeof(TSource)), source.Expression, selector), CancellationToken.None);
#endif
        }

        public static Task<decimal?> Average<TSource>(this IAsyncQueryable<TSource> source, Expression<Func<TSource, Task<decimal?>>> selector)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));
            if (selector == null)
                throw new ArgumentNullException(nameof(selector));

#if CRIPPLED_REFLECTION
            return source.Provider.ExecuteAsync<decimal?>(Expression.Call(InfoOf(() => AsyncQueryable.Average<TSource>(default(IAsyncQueryable<TSource>), default(Expression<Func<TSource, Task<decimal?>>>))), source.Expression, selector), CancellationToken.None);
#else
            return source.Provider.ExecuteAsync<decimal?>(Expression.Call(((MethodInfo)MethodBase.GetCurrentMethod()).MakeGenericMethod(typeof(TSource)), source.Expression, selector), CancellationToken.None);
#endif
        }

        public static Task<decimal> Average<TSource>(this IAsyncQueryable<TSource> source, Expression<Func<TSource, Task<decimal>>> selector)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));
            if (selector == null)
                throw new ArgumentNullException(nameof(selector));

#if CRIPPLED_REFLECTION
            return source.Provider.ExecuteAsync<decimal>(Expression.Call(InfoOf(() => AsyncQueryable.Average<TSource>(default(IAsyncQueryable<TSource>), default(Expression<Func<TSource, Task<decimal>>>))), source.Expression, selector), CancellationToken.None);
#else
            return source.Provider.ExecuteAsync<decimal>(Expression.Call(((MethodInfo)MethodBase.GetCurrentMethod()).MakeGenericMethod(typeof(TSource)), source.Expression, selector), CancellationToken.None);
#endif
        }

        public static Task<double?> Average<TSource>(this IAsyncQueryable<TSource> source, Expression<Func<TSource, Task<double?>>> selector)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));
            if (selector == null)
                throw new ArgumentNullException(nameof(selector));

#if CRIPPLED_REFLECTION
            return source.Provider.ExecuteAsync<double?>(Expression.Call(InfoOf(() => AsyncQueryable.Average<TSource>(default(IAsyncQueryable<TSource>), default(Expression<Func<TSource, Task<double?>>>))), source.Expression, selector), CancellationToken.None);
#else
            return source.Provider.ExecuteAsync<double?>(Expression.Call(((MethodInfo)MethodBase.GetCurrentMethod()).MakeGenericMethod(typeof(TSource)), source.Expression, selector), CancellationToken.None);
#endif
        }

        public static Task<double> Average<TSource>(this IAsyncQueryable<TSource> source, Expression<Func<TSource, Task<double>>> selector)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));
            if (selector == null)
                throw new ArgumentNullException(nameof(selector));

#if CRIPPLED_REFLECTION
            return source.Provider.ExecuteAsync<double>(Expression.Call(InfoOf(() => AsyncQueryable.Average<TSource>(default(IAsyncQueryable<TSource>), default(Expression<Func<TSource, Task<double>>>))), source.Expression, selector), CancellationToken.None);
#else
            return source.Provider.ExecuteAsync<double>(Expression.Call(((MethodInfo)MethodBase.GetCurrentMethod()).MakeGenericMethod(typeof(TSource)), source.Expression, selector), CancellationToken.None);
#endif
        }

        public static Task<float?> Average<TSource>(this IAsyncQueryable<TSource> source, Expression<Func<TSource, Task<float?>>> selector)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));
            if (selector == null)
                throw new ArgumentNullException(nameof(selector));

#if CRIPPLED_REFLECTION
            return source.Provider.ExecuteAsync<float?>(Expression.Call(InfoOf(() => AsyncQueryable.Average<TSource>(default(IAsyncQueryable<TSource>), default(Expression<Func<TSource, Task<float?>>>))), source.Expression, selector), CancellationToken.None);
#else
            return source.Provider.ExecuteAsync<float?>(Expression.Call(((MethodInfo)MethodBase.GetCurrentMethod()).MakeGenericMethod(typeof(TSource)), source.Expression, selector), CancellationToken.None);
#endif
        }

        public static Task<float> Average<TSource>(this IAsyncQueryable<TSource> source, Expression<Func<TSource, Task<float>>> selector)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));
            if (selector == null)
                throw new ArgumentNullException(nameof(selector));

#if CRIPPLED_REFLECTION
            return source.Provider.ExecuteAsync<float>(Expression.Call(InfoOf(() => AsyncQueryable.Average<TSource>(default(IAsyncQueryable<TSource>), default(Expression<Func<TSource, Task<float>>>))), source.Expression, selector), CancellationToken.None);
#else
            return source.Provider.ExecuteAsync<float>(Expression.Call(((MethodInfo)MethodBase.GetCurrentMethod()).MakeGenericMethod(typeof(TSource)), source.Expression, selector), CancellationToken.None);
#endif
        }

        public static Task<double?> Average<TSource>(this IAsyncQueryable<TSource> source, Expression<Func<TSource, Task<int?>>> selector)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));
            if (selector == null)
                throw new ArgumentNullException(nameof(selector));

#if CRIPPLED_REFLECTION
            return source.Provider.ExecuteAsync<double?>(Expression.Call(InfoOf(() => AsyncQueryable.Average<TSource>(default(IAsyncQueryable<TSource>), default(Expression<Func<TSource, Task<int?>>>))), source.Expression, selector), CancellationToken.None);
#else
            return source.Provider.ExecuteAsync<double?>(Expression.Call(((MethodInfo)MethodBase.GetCurrentMethod()).MakeGenericMethod(typeof(TSource)), source.Expression, selector), CancellationToken.None);
#endif
        }

        public static Task<double> Average<TSource>(this IAsyncQueryable<TSource> source, Expression<Func<TSource, Task<int>>> selector)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));
            if (selector == null)
                throw new ArgumentNullException(nameof(selector));

#if CRIPPLED_REFLECTION
            return source.Provider.ExecuteAsync<double>(Expression.Call(InfoOf(() => AsyncQueryable.Average<TSource>(default(IAsyncQueryable<TSource>), default(Expression<Func<TSource, Task<int>>>))), source.Expression, selector), CancellationToken.None);
#else
            return source.Provider.ExecuteAsync<double>(Expression.Call(((MethodInfo)MethodBase.GetCurrentMethod()).MakeGenericMethod(typeof(TSource)), source.Expression, selector), CancellationToken.None);
#endif
        }

        public static Task<double?> Average<TSource>(this IAsyncQueryable<TSource> source, Expression<Func<TSource, Task<long?>>> selector)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));
            if (selector == null)
                throw new ArgumentNullException(nameof(selector));

#if CRIPPLED_REFLECTION
            return source.Provider.ExecuteAsync<double?>(Expression.Call(InfoOf(() => AsyncQueryable.Average<TSource>(default(IAsyncQueryable<TSource>), default(Expression<Func<TSource, Task<long?>>>))), source.Expression, selector), CancellationToken.None);
#else
            return source.Provider.ExecuteAsync<double?>(Expression.Call(((MethodInfo)MethodBase.GetCurrentMethod()).MakeGenericMethod(typeof(TSource)), source.Expression, selector), CancellationToken.None);
#endif
        }

        public static Task<double> Average<TSource>(this IAsyncQueryable<TSource> source, Expression<Func<TSource, Task<long>>> selector)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));
            if (selector == null)
                throw new ArgumentNullException(nameof(selector));

#if CRIPPLED_REFLECTION
            return source.Provider.ExecuteAsync<double>(Expression.Call(InfoOf(() => AsyncQueryable.Average<TSource>(default(IAsyncQueryable<TSource>), default(Expression<Func<TSource, Task<long>>>))), source.Expression, selector), CancellationToken.None);
#else
            return source.Provider.ExecuteAsync<double>(Expression.Call(((MethodInfo)MethodBase.GetCurrentMethod()).MakeGenericMethod(typeof(TSource)), source.Expression, selector), CancellationToken.None);
#endif
        }

        public static Task<decimal?> Average<TSource>(this IAsyncQueryable<TSource> source, Expression<Func<TSource, decimal?>> selector, CancellationToken cancellationToken)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));
            if (selector == null)
                throw new ArgumentNullException(nameof(selector));

#if CRIPPLED_REFLECTION
            return source.Provider.ExecuteAsync<decimal?>(Expression.Call(InfoOf(() => AsyncQueryable.Average<TSource>(default(IAsyncQueryable<TSource>), default(Expression<Func<TSource, decimal?>>), default(CancellationToken))), source.Expression, selector, Expression.Constant(cancellationToken, typeof(CancellationToken))), cancellationToken);
#else
            return source.Provider.ExecuteAsync<decimal?>(Expression.Call(((MethodInfo)MethodBase.GetCurrentMethod()).MakeGenericMethod(typeof(TSource)), source.Expression, selector, Expression.Constant(cancellationToken, typeof(CancellationToken))), cancellationToken);
#endif
        }

        public static Task<decimal> Average<TSource>(this IAsyncQueryable<TSource> source, Expression<Func<TSource, decimal>> selector, CancellationToken cancellationToken)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));
            if (selector == null)
                throw new ArgumentNullException(nameof(selector));

#if CRIPPLED_REFLECTION
            return source.Provider.ExecuteAsync<decimal>(Expression.Call(InfoOf(() => AsyncQueryable.Average<TSource>(default(IAsyncQueryable<TSource>), default(Expression<Func<TSource, decimal>>), default(CancellationToken))), source.Expression, selector, Expression.Constant(cancellationToken, typeof(CancellationToken))), cancellationToken);
#else
            return source.Provider.ExecuteAsync<decimal>(Expression.Call(((MethodInfo)MethodBase.GetCurrentMethod()).MakeGenericMethod(typeof(TSource)), source.Expression, selector, Expression.Constant(cancellationToken, typeof(CancellationToken))), cancellationToken);
#endif
        }

        public static Task<double?> Average<TSource>(this IAsyncQueryable<TSource> source, Expression<Func<TSource, double?>> selector, CancellationToken cancellationToken)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));
            if (selector == null)
                throw new ArgumentNullException(nameof(selector));

#if CRIPPLED_REFLECTION
            return source.Provider.ExecuteAsync<double?>(Expression.Call(InfoOf(() => AsyncQueryable.Average<TSource>(default(IAsyncQueryable<TSource>), default(Expression<Func<TSource, double?>>), default(CancellationToken))), source.Expression, selector, Expression.Constant(cancellationToken, typeof(CancellationToken))), cancellationToken);
#else
            return source.Provider.ExecuteAsync<double?>(Expression.Call(((MethodInfo)MethodBase.GetCurrentMethod()).MakeGenericMethod(typeof(TSource)), source.Expression, selector, Expression.Constant(cancellationToken, typeof(CancellationToken))), cancellationToken);
#endif
        }

        public static Task<double> Average<TSource>(this IAsyncQueryable<TSource> source, Expression<Func<TSource, double>> selector, CancellationToken cancellationToken)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));
            if (selector == null)
                throw new ArgumentNullException(nameof(selector));

#if CRIPPLED_REFLECTION
            return source.Provider.ExecuteAsync<double>(Expression.Call(InfoOf(() => AsyncQueryable.Average<TSource>(default(IAsyncQueryable<TSource>), default(Expression<Func<TSource, double>>), default(CancellationToken))), source.Expression, selector, Expression.Constant(cancellationToken, typeof(CancellationToken))), cancellationToken);
#else
            return source.Provider.ExecuteAsync<double>(Expression.Call(((MethodInfo)MethodBase.GetCurrentMethod()).MakeGenericMethod(typeof(TSource)), source.Expression, selector, Expression.Constant(cancellationToken, typeof(CancellationToken))), cancellationToken);
#endif
        }

        public static Task<float?> Average<TSource>(this IAsyncQueryable<TSource> source, Expression<Func<TSource, float?>> selector, CancellationToken cancellationToken)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));
            if (selector == null)
                throw new ArgumentNullException(nameof(selector));

#if CRIPPLED_REFLECTION
            return source.Provider.ExecuteAsync<float?>(Expression.Call(InfoOf(() => AsyncQueryable.Average<TSource>(default(IAsyncQueryable<TSource>), default(Expression<Func<TSource, float?>>), default(CancellationToken))), source.Expression, selector, Expression.Constant(cancellationToken, typeof(CancellationToken))), cancellationToken);
#else
            return source.Provider.ExecuteAsync<float?>(Expression.Call(((MethodInfo)MethodBase.GetCurrentMethod()).MakeGenericMethod(typeof(TSource)), source.Expression, selector, Expression.Constant(cancellationToken, typeof(CancellationToken))), cancellationToken);
#endif
        }

        public static Task<float> Average<TSource>(this IAsyncQueryable<TSource> source, Expression<Func<TSource, float>> selector, CancellationToken cancellationToken)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));
            if (selector == null)
                throw new ArgumentNullException(nameof(selector));

#if CRIPPLED_REFLECTION
            return source.Provider.ExecuteAsync<float>(Expression.Call(InfoOf(() => AsyncQueryable.Average<TSource>(default(IAsyncQueryable<TSource>), default(Expression<Func<TSource, float>>), default(CancellationToken))), source.Expression, selector, Expression.Constant(cancellationToken, typeof(CancellationToken))), cancellationToken);
#else
            return source.Provider.ExecuteAsync<float>(Expression.Call(((MethodInfo)MethodBase.GetCurrentMethod()).MakeGenericMethod(typeof(TSource)), source.Expression, selector, Expression.Constant(cancellationToken, typeof(CancellationToken))), cancellationToken);
#endif
        }

        public static Task<double?> Average<TSource>(this IAsyncQueryable<TSource> source, Expression<Func<TSource, int?>> selector, CancellationToken cancellationToken)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));
            if (selector == null)
                throw new ArgumentNullException(nameof(selector));

#if CRIPPLED_REFLECTION
            return source.Provider.ExecuteAsync<double?>(Expression.Call(InfoOf(() => AsyncQueryable.Average<TSource>(default(IAsyncQueryable<TSource>), default(Expression<Func<TSource, int?>>), default(CancellationToken))), source.Expression, selector, Expression.Constant(cancellationToken, typeof(CancellationToken))), cancellationToken);
#else
            return source.Provider.ExecuteAsync<double?>(Expression.Call(((MethodInfo)MethodBase.GetCurrentMethod()).MakeGenericMethod(typeof(TSource)), source.Expression, selector, Expression.Constant(cancellationToken, typeof(CancellationToken))), cancellationToken);
#endif
        }

        public static Task<double> Average<TSource>(this IAsyncQueryable<TSource> source, Expression<Func<TSource, int>> selector, CancellationToken cancellationToken)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));
            if (selector == null)
                throw new ArgumentNullException(nameof(selector));

#if CRIPPLED_REFLECTION
            return source.Provider.ExecuteAsync<double>(Expression.Call(InfoOf(() => AsyncQueryable.Average<TSource>(default(IAsyncQueryable<TSource>), default(Expression<Func<TSource, int>>), default(CancellationToken))), source.Expression, selector, Expression.Constant(cancellationToken, typeof(CancellationToken))), cancellationToken);
#else
            return source.Provider.ExecuteAsync<double>(Expression.Call(((MethodInfo)MethodBase.GetCurrentMethod()).MakeGenericMethod(typeof(TSource)), source.Expression, selector, Expression.Constant(cancellationToken, typeof(CancellationToken))), cancellationToken);
#endif
        }

        public static Task<double?> Average<TSource>(this IAsyncQueryable<TSource> source, Expression<Func<TSource, long?>> selector, CancellationToken cancellationToken)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));
            if (selector == null)
                throw new ArgumentNullException(nameof(selector));

#if CRIPPLED_REFLECTION
            return source.Provider.ExecuteAsync<double?>(Expression.Call(InfoOf(() => AsyncQueryable.Average<TSource>(default(IAsyncQueryable<TSource>), default(Expression<Func<TSource, long?>>), default(CancellationToken))), source.Expression, selector, Expression.Constant(cancellationToken, typeof(CancellationToken))), cancellationToken);
#else
            return source.Provider.ExecuteAsync<double?>(Expression.Call(((MethodInfo)MethodBase.GetCurrentMethod()).MakeGenericMethod(typeof(TSource)), source.Expression, selector, Expression.Constant(cancellationToken, typeof(CancellationToken))), cancellationToken);
#endif
        }

        public static Task<double> Average<TSource>(this IAsyncQueryable<TSource> source, Expression<Func<TSource, long>> selector, CancellationToken cancellationToken)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));
            if (selector == null)
                throw new ArgumentNullException(nameof(selector));

#if CRIPPLED_REFLECTION
            return source.Provider.ExecuteAsync<double>(Expression.Call(InfoOf(() => AsyncQueryable.Average<TSource>(default(IAsyncQueryable<TSource>), default(Expression<Func<TSource, long>>), default(CancellationToken))), source.Expression, selector, Expression.Constant(cancellationToken, typeof(CancellationToken))), cancellationToken);
#else
            return source.Provider.ExecuteAsync<double>(Expression.Call(((MethodInfo)MethodBase.GetCurrentMethod()).MakeGenericMethod(typeof(TSource)), source.Expression, selector, Expression.Constant(cancellationToken, typeof(CancellationToken))), cancellationToken);
#endif
        }

        public static Task<decimal?> Average<TSource>(this IAsyncQueryable<TSource> source, Expression<Func<TSource, Task<decimal?>>> selector, CancellationToken cancellationToken)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));
            if (selector == null)
                throw new ArgumentNullException(nameof(selector));

#if CRIPPLED_REFLECTION
            return source.Provider.ExecuteAsync<decimal?>(Expression.Call(InfoOf(() => AsyncQueryable.Average<TSource>(default(IAsyncQueryable<TSource>), default(Expression<Func<TSource, Task<decimal?>>>), default(CancellationToken))), source.Expression, selector, Expression.Constant(cancellationToken, typeof(CancellationToken))), cancellationToken);
#else
            return source.Provider.ExecuteAsync<decimal?>(Expression.Call(((MethodInfo)MethodBase.GetCurrentMethod()).MakeGenericMethod(typeof(TSource)), source.Expression, selector, Expression.Constant(cancellationToken, typeof(CancellationToken))), cancellationToken);
#endif
        }

        public static Task<decimal> Average<TSource>(this IAsyncQueryable<TSource> source, Expression<Func<TSource, Task<decimal>>> selector, CancellationToken cancellationToken)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));
            if (selector == null)
                throw new ArgumentNullException(nameof(selector));

#if CRIPPLED_REFLECTION
            return source.Provider.ExecuteAsync<decimal>(Expression.Call(InfoOf(() => AsyncQueryable.Average<TSource>(default(IAsyncQueryable<TSource>), default(Expression<Func<TSource, Task<decimal>>>), default(CancellationToken))), source.Expression, selector, Expression.Constant(cancellationToken, typeof(CancellationToken))), cancellationToken);
#else
            return source.Provider.ExecuteAsync<decimal>(Expression.Call(((MethodInfo)MethodBase.GetCurrentMethod()).MakeGenericMethod(typeof(TSource)), source.Expression, selector, Expression.Constant(cancellationToken, typeof(CancellationToken))), cancellationToken);
#endif
        }

        public static Task<double?> Average<TSource>(this IAsyncQueryable<TSource> source, Expression<Func<TSource, Task<double?>>> selector, CancellationToken cancellationToken)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));
            if (selector == null)
                throw new ArgumentNullException(nameof(selector));

#if CRIPPLED_REFLECTION
            return source.Provider.ExecuteAsync<double?>(Expression.Call(InfoOf(() => AsyncQueryable.Average<TSource>(default(IAsyncQueryable<TSource>), default(Expression<Func<TSource, Task<double?>>>), default(CancellationToken))), source.Expression, selector, Expression.Constant(cancellationToken, typeof(CancellationToken))), cancellationToken);
#else
            return source.Provider.ExecuteAsync<double?>(Expression.Call(((MethodInfo)MethodBase.GetCurrentMethod()).MakeGenericMethod(typeof(TSource)), source.Expression, selector, Expression.Constant(cancellationToken, typeof(CancellationToken))), cancellationToken);
#endif
        }

        public static Task<double> Average<TSource>(this IAsyncQueryable<TSource> source, Expression<Func<TSource, Task<double>>> selector, CancellationToken cancellationToken)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));
            if (selector == null)
                throw new ArgumentNullException(nameof(selector));

#if CRIPPLED_REFLECTION
            return source.Provider.ExecuteAsync<double>(Expression.Call(InfoOf(() => AsyncQueryable.Average<TSource>(default(IAsyncQueryable<TSource>), default(Expression<Func<TSource, Task<double>>>), default(CancellationToken))), source.Expression, selector, Expression.Constant(cancellationToken, typeof(CancellationToken))), cancellationToken);
#else
            return source.Provider.ExecuteAsync<double>(Expression.Call(((MethodInfo)MethodBase.GetCurrentMethod()).MakeGenericMethod(typeof(TSource)), source.Expression, selector, Expression.Constant(cancellationToken, typeof(CancellationToken))), cancellationToken);
#endif
        }

        public static Task<float?> Average<TSource>(this IAsyncQueryable<TSource> source, Expression<Func<TSource, Task<float?>>> selector, CancellationToken cancellationToken)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));
            if (selector == null)
                throw new ArgumentNullException(nameof(selector));

#if CRIPPLED_REFLECTION
            return source.Provider.ExecuteAsync<float?>(Expression.Call(InfoOf(() => AsyncQueryable.Average<TSource>(default(IAsyncQueryable<TSource>), default(Expression<Func<TSource, Task<float?>>>), default(CancellationToken))), source.Expression, selector, Expression.Constant(cancellationToken, typeof(CancellationToken))), cancellationToken);
#else
            return source.Provider.ExecuteAsync<float?>(Expression.Call(((MethodInfo)MethodBase.GetCurrentMethod()).MakeGenericMethod(typeof(TSource)), source.Expression, selector, Expression.Constant(cancellationToken, typeof(CancellationToken))), cancellationToken);
#endif
        }

        public static Task<float> Average<TSource>(this IAsyncQueryable<TSource> source, Expression<Func<TSource, Task<float>>> selector, CancellationToken cancellationToken)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));
            if (selector == null)
                throw new ArgumentNullException(nameof(selector));

#if CRIPPLED_REFLECTION
            return source.Provider.ExecuteAsync<float>(Expression.Call(InfoOf(() => AsyncQueryable.Average<TSource>(default(IAsyncQueryable<TSource>), default(Expression<Func<TSource, Task<float>>>), default(CancellationToken))), source.Expression, selector, Expression.Constant(cancellationToken, typeof(CancellationToken))), cancellationToken);
#else
            return source.Provider.ExecuteAsync<float>(Expression.Call(((MethodInfo)MethodBase.GetCurrentMethod()).MakeGenericMethod(typeof(TSource)), source.Expression, selector, Expression.Constant(cancellationToken, typeof(CancellationToken))), cancellationToken);
#endif
        }

        public static Task<double?> Average<TSource>(this IAsyncQueryable<TSource> source, Expression<Func<TSource, Task<int?>>> selector, CancellationToken cancellationToken)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));
            if (selector == null)
                throw new ArgumentNullException(nameof(selector));

#if CRIPPLED_REFLECTION
            return source.Provider.ExecuteAsync<double?>(Expression.Call(InfoOf(() => AsyncQueryable.Average<TSource>(default(IAsyncQueryable<TSource>), default(Expression<Func<TSource, Task<int?>>>), default(CancellationToken))), source.Expression, selector, Expression.Constant(cancellationToken, typeof(CancellationToken))), cancellationToken);
#else
            return source.Provider.ExecuteAsync<double?>(Expression.Call(((MethodInfo)MethodBase.GetCurrentMethod()).MakeGenericMethod(typeof(TSource)), source.Expression, selector, Expression.Constant(cancellationToken, typeof(CancellationToken))), cancellationToken);
#endif
        }

        public static Task<double> Average<TSource>(this IAsyncQueryable<TSource> source, Expression<Func<TSource, Task<int>>> selector, CancellationToken cancellationToken)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));
            if (selector == null)
                throw new ArgumentNullException(nameof(selector));

#if CRIPPLED_REFLECTION
            return source.Provider.ExecuteAsync<double>(Expression.Call(InfoOf(() => AsyncQueryable.Average<TSource>(default(IAsyncQueryable<TSource>), default(Expression<Func<TSource, Task<int>>>), default(CancellationToken))), source.Expression, selector, Expression.Constant(cancellationToken, typeof(CancellationToken))), cancellationToken);
#else
            return source.Provider.ExecuteAsync<double>(Expression.Call(((MethodInfo)MethodBase.GetCurrentMethod()).MakeGenericMethod(typeof(TSource)), source.Expression, selector, Expression.Constant(cancellationToken, typeof(CancellationToken))), cancellationToken);
#endif
        }

        public static Task<double?> Average<TSource>(this IAsyncQueryable<TSource> source, Expression<Func<TSource, Task<long?>>> selector, CancellationToken cancellationToken)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));
            if (selector == null)
                throw new ArgumentNullException(nameof(selector));

#if CRIPPLED_REFLECTION
            return source.Provider.ExecuteAsync<double?>(Expression.Call(InfoOf(() => AsyncQueryable.Average<TSource>(default(IAsyncQueryable<TSource>), default(Expression<Func<TSource, Task<long?>>>), default(CancellationToken))), source.Expression, selector, Expression.Constant(cancellationToken, typeof(CancellationToken))), cancellationToken);
#else
            return source.Provider.ExecuteAsync<double?>(Expression.Call(((MethodInfo)MethodBase.GetCurrentMethod()).MakeGenericMethod(typeof(TSource)), source.Expression, selector, Expression.Constant(cancellationToken, typeof(CancellationToken))), cancellationToken);
#endif
        }

        public static Task<double> Average<TSource>(this IAsyncQueryable<TSource> source, Expression<Func<TSource, Task<long>>> selector, CancellationToken cancellationToken)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));
            if (selector == null)
                throw new ArgumentNullException(nameof(selector));

#if CRIPPLED_REFLECTION
            return source.Provider.ExecuteAsync<double>(Expression.Call(InfoOf(() => AsyncQueryable.Average<TSource>(default(IAsyncQueryable<TSource>), default(Expression<Func<TSource, Task<long>>>), default(CancellationToken))), source.Expression, selector, Expression.Constant(cancellationToken, typeof(CancellationToken))), cancellationToken);
#else
            return source.Provider.ExecuteAsync<double>(Expression.Call(((MethodInfo)MethodBase.GetCurrentMethod()).MakeGenericMethod(typeof(TSource)), source.Expression, selector, Expression.Constant(cancellationToken, typeof(CancellationToken))), cancellationToken);
#endif
        }

        public static IAsyncQueryable<TResult> Cast<TResult>(this IAsyncQueryable<object> source)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));

#if CRIPPLED_REFLECTION
            return source.Provider.CreateQuery<TResult>(Expression.Call(InfoOf(() => AsyncQueryable.Cast<TResult>(default(IAsyncQueryable<object>))), source.Expression));
#else
            return source.Provider.CreateQuery<TResult>(Expression.Call(((MethodInfo)MethodBase.GetCurrentMethod()).MakeGenericMethod(typeof(TResult)), source.Expression));
#endif
        }

        public static IAsyncQueryable<TSource> Concat<TSource>(this IAsyncQueryable<TSource> first, IAsyncEnumerable<TSource> second)
        {
            if (first == null)
                throw new ArgumentNullException(nameof(first));
            if (second == null)
                throw new ArgumentNullException(nameof(second));

#if CRIPPLED_REFLECTION
            return first.Provider.CreateQuery<TSource>(Expression.Call(InfoOf(() => AsyncQueryable.Concat<TSource>(default(IAsyncQueryable<TSource>), default(IAsyncEnumerable<TSource>))), first.Expression, GetSourceExpression(second)));
#else
            return first.Provider.CreateQuery<TSource>(Expression.Call(((MethodInfo)MethodBase.GetCurrentMethod()).MakeGenericMethod(typeof(TSource)), first.Expression, GetSourceExpression(second)));
#endif
        }

        public static Task<bool> Contains<TSource>(this IAsyncQueryable<TSource> source, TSource value)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));

#if CRIPPLED_REFLECTION
            return source.Provider.ExecuteAsync<bool>(Expression.Call(InfoOf(() => AsyncQueryable.Contains<TSource>(default(IAsyncQueryable<TSource>), default(TSource))), source.Expression, Expression.Constant(value, typeof(TSource))), CancellationToken.None);
#else
            return source.Provider.ExecuteAsync<bool>(Expression.Call(((MethodInfo)MethodBase.GetCurrentMethod()).MakeGenericMethod(typeof(TSource)), source.Expression, Expression.Constant(value, typeof(TSource))), CancellationToken.None);
#endif
        }

        public static Task<bool> Contains<TSource>(this IAsyncQueryable<TSource> source, TSource value, CancellationToken cancellationToken)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));

#if CRIPPLED_REFLECTION
            return source.Provider.ExecuteAsync<bool>(Expression.Call(InfoOf(() => AsyncQueryable.Contains<TSource>(default(IAsyncQueryable<TSource>), default(TSource), default(CancellationToken))), source.Expression, Expression.Constant(value, typeof(TSource)), Expression.Constant(cancellationToken, typeof(CancellationToken))), cancellationToken);
#else
            return source.Provider.ExecuteAsync<bool>(Expression.Call(((MethodInfo)MethodBase.GetCurrentMethod()).MakeGenericMethod(typeof(TSource)), source.Expression, Expression.Constant(value, typeof(TSource)), Expression.Constant(cancellationToken, typeof(CancellationToken))), cancellationToken);
#endif
        }

        public static Task<bool> Contains<TSource>(this IAsyncQueryable<TSource> source, TSource value, IEqualityComparer<TSource> comparer)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));
            if (comparer == null)
                throw new ArgumentNullException(nameof(comparer));

#if CRIPPLED_REFLECTION
            return source.Provider.ExecuteAsync<bool>(Expression.Call(InfoOf(() => AsyncQueryable.Contains<TSource>(default(IAsyncQueryable<TSource>), default(TSource), default(IEqualityComparer<TSource>))), source.Expression, Expression.Constant(value, typeof(TSource)), Expression.Constant(comparer, typeof(IEqualityComparer<TSource>))), CancellationToken.None);
#else
            return source.Provider.ExecuteAsync<bool>(Expression.Call(((MethodInfo)MethodBase.GetCurrentMethod()).MakeGenericMethod(typeof(TSource)), source.Expression, Expression.Constant(value, typeof(TSource)), Expression.Constant(comparer, typeof(IEqualityComparer<TSource>))), CancellationToken.None);
#endif
        }

        public static Task<bool> Contains<TSource>(this IAsyncQueryable<TSource> source, TSource value, IEqualityComparer<TSource> comparer, CancellationToken cancellationToken)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));
            if (comparer == null)
                throw new ArgumentNullException(nameof(comparer));

#if CRIPPLED_REFLECTION
            return source.Provider.ExecuteAsync<bool>(Expression.Call(InfoOf(() => AsyncQueryable.Contains<TSource>(default(IAsyncQueryable<TSource>), default(TSource), default(IEqualityComparer<TSource>), default(CancellationToken))), source.Expression, Expression.Constant(value, typeof(TSource)), Expression.Constant(comparer, typeof(IEqualityComparer<TSource>)), Expression.Constant(cancellationToken, typeof(CancellationToken))), cancellationToken);
#else
            return source.Provider.ExecuteAsync<bool>(Expression.Call(((MethodInfo)MethodBase.GetCurrentMethod()).MakeGenericMethod(typeof(TSource)), source.Expression, Expression.Constant(value, typeof(TSource)), Expression.Constant(comparer, typeof(IEqualityComparer<TSource>)), Expression.Constant(cancellationToken, typeof(CancellationToken))), cancellationToken);
#endif
        }

        public static Task<int> Count<TSource>(this IAsyncQueryable<TSource> source)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));

#if CRIPPLED_REFLECTION
            return source.Provider.ExecuteAsync<int>(Expression.Call(InfoOf(() => AsyncQueryable.Count<TSource>(default(IAsyncQueryable<TSource>))), source.Expression), CancellationToken.None);
#else
            return source.Provider.ExecuteAsync<int>(Expression.Call(((MethodInfo)MethodBase.GetCurrentMethod()).MakeGenericMethod(typeof(TSource)), source.Expression), CancellationToken.None);
#endif
        }

        public static Task<int> Count<TSource>(this IAsyncQueryable<TSource> source, CancellationToken cancellationToken)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));

#if CRIPPLED_REFLECTION
            return source.Provider.ExecuteAsync<int>(Expression.Call(InfoOf(() => AsyncQueryable.Count<TSource>(default(IAsyncQueryable<TSource>), default(CancellationToken))), source.Expression, Expression.Constant(cancellationToken, typeof(CancellationToken))), cancellationToken);
#else
            return source.Provider.ExecuteAsync<int>(Expression.Call(((MethodInfo)MethodBase.GetCurrentMethod()).MakeGenericMethod(typeof(TSource)), source.Expression, Expression.Constant(cancellationToken, typeof(CancellationToken))), cancellationToken);
#endif
        }

        public static Task<int> Count<TSource>(this IAsyncQueryable<TSource> source, Expression<Func<TSource, bool>> predicate)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));
            if (predicate == null)
                throw new ArgumentNullException(nameof(predicate));

#if CRIPPLED_REFLECTION
            return source.Provider.ExecuteAsync<int>(Expression.Call(InfoOf(() => AsyncQueryable.Count<TSource>(default(IAsyncQueryable<TSource>), default(Expression<Func<TSource, bool>>))), source.Expression, predicate), CancellationToken.None);
#else
            return source.Provider.ExecuteAsync<int>(Expression.Call(((MethodInfo)MethodBase.GetCurrentMethod()).MakeGenericMethod(typeof(TSource)), source.Expression, predicate), CancellationToken.None);
#endif
        }

        public static Task<int> Count<TSource>(this IAsyncQueryable<TSource> source, Expression<Func<TSource, Task<bool>>> predicate)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));
            if (predicate == null)
                throw new ArgumentNullException(nameof(predicate));

#if CRIPPLED_REFLECTION
            return source.Provider.ExecuteAsync<int>(Expression.Call(InfoOf(() => AsyncQueryable.Count<TSource>(default(IAsyncQueryable<TSource>), default(Expression<Func<TSource, Task<bool>>>))), source.Expression, predicate), CancellationToken.None);
#else
            return source.Provider.ExecuteAsync<int>(Expression.Call(((MethodInfo)MethodBase.GetCurrentMethod()).MakeGenericMethod(typeof(TSource)), source.Expression, predicate), CancellationToken.None);
#endif
        }

        public static Task<int> Count<TSource>(this IAsyncQueryable<TSource> source, Expression<Func<TSource, bool>> predicate, CancellationToken cancellationToken)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));
            if (predicate == null)
                throw new ArgumentNullException(nameof(predicate));

#if CRIPPLED_REFLECTION
            return source.Provider.ExecuteAsync<int>(Expression.Call(InfoOf(() => AsyncQueryable.Count<TSource>(default(IAsyncQueryable<TSource>), default(Expression<Func<TSource, bool>>), default(CancellationToken))), source.Expression, predicate, Expression.Constant(cancellationToken, typeof(CancellationToken))), cancellationToken);
#else
            return source.Provider.ExecuteAsync<int>(Expression.Call(((MethodInfo)MethodBase.GetCurrentMethod()).MakeGenericMethod(typeof(TSource)), source.Expression, predicate, Expression.Constant(cancellationToken, typeof(CancellationToken))), cancellationToken);
#endif
        }

        public static Task<int> Count<TSource>(this IAsyncQueryable<TSource> source, Expression<Func<TSource, Task<bool>>> predicate, CancellationToken cancellationToken)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));
            if (predicate == null)
                throw new ArgumentNullException(nameof(predicate));

#if CRIPPLED_REFLECTION
            return source.Provider.ExecuteAsync<int>(Expression.Call(InfoOf(() => AsyncQueryable.Count<TSource>(default(IAsyncQueryable<TSource>), default(Expression<Func<TSource, Task<bool>>>), default(CancellationToken))), source.Expression, predicate, Expression.Constant(cancellationToken, typeof(CancellationToken))), cancellationToken);
#else
            return source.Provider.ExecuteAsync<int>(Expression.Call(((MethodInfo)MethodBase.GetCurrentMethod()).MakeGenericMethod(typeof(TSource)), source.Expression, predicate, Expression.Constant(cancellationToken, typeof(CancellationToken))), cancellationToken);
#endif
        }

        public static IAsyncQueryable<TSource> DefaultIfEmpty<TSource>(this IAsyncQueryable<TSource> source)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));

#if CRIPPLED_REFLECTION
            return source.Provider.CreateQuery<TSource>(Expression.Call(InfoOf(() => AsyncQueryable.DefaultIfEmpty<TSource>(default(IAsyncQueryable<TSource>))), source.Expression));
#else
            return source.Provider.CreateQuery<TSource>(Expression.Call(((MethodInfo)MethodBase.GetCurrentMethod()).MakeGenericMethod(typeof(TSource)), source.Expression));
#endif
        }

        public static IAsyncQueryable<TSource> DefaultIfEmpty<TSource>(this IAsyncQueryable<TSource> source, TSource defaultValue)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));

#if CRIPPLED_REFLECTION
            return source.Provider.CreateQuery<TSource>(Expression.Call(InfoOf(() => AsyncQueryable.DefaultIfEmpty<TSource>(default(IAsyncQueryable<TSource>), default(TSource))), source.Expression, Expression.Constant(defaultValue, typeof(TSource))));
#else
            return source.Provider.CreateQuery<TSource>(Expression.Call(((MethodInfo)MethodBase.GetCurrentMethod()).MakeGenericMethod(typeof(TSource)), source.Expression, Expression.Constant(defaultValue, typeof(TSource))));
#endif
        }

        public static IAsyncQueryable<TSource> Distinct<TSource>(this IAsyncQueryable<TSource> source)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));

#if CRIPPLED_REFLECTION
            return source.Provider.CreateQuery<TSource>(Expression.Call(InfoOf(() => AsyncQueryable.Distinct<TSource>(default(IAsyncQueryable<TSource>))), source.Expression));
#else
            return source.Provider.CreateQuery<TSource>(Expression.Call(((MethodInfo)MethodBase.GetCurrentMethod()).MakeGenericMethod(typeof(TSource)), source.Expression));
#endif
        }

        public static IAsyncQueryable<TSource> Distinct<TSource>(this IAsyncQueryable<TSource> source, IEqualityComparer<TSource> comparer)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));
            if (comparer == null)
                throw new ArgumentNullException(nameof(comparer));

#if CRIPPLED_REFLECTION
            return source.Provider.CreateQuery<TSource>(Expression.Call(InfoOf(() => AsyncQueryable.Distinct<TSource>(default(IAsyncQueryable<TSource>), default(IEqualityComparer<TSource>))), source.Expression, Expression.Constant(comparer, typeof(IEqualityComparer<TSource>))));
#else
            return source.Provider.CreateQuery<TSource>(Expression.Call(((MethodInfo)MethodBase.GetCurrentMethod()).MakeGenericMethod(typeof(TSource)), source.Expression, Expression.Constant(comparer, typeof(IEqualityComparer<TSource>))));
#endif
        }

        public static Task<TSource> ElementAt<TSource>(this IAsyncQueryable<TSource> source, int index)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));

#if CRIPPLED_REFLECTION
            return source.Provider.ExecuteAsync<TSource>(Expression.Call(InfoOf(() => AsyncQueryable.ElementAt<TSource>(default(IAsyncQueryable<TSource>), default(int))), source.Expression, Expression.Constant(index, typeof(int))), CancellationToken.None);
#else
            return source.Provider.ExecuteAsync<TSource>(Expression.Call(((MethodInfo)MethodBase.GetCurrentMethod()).MakeGenericMethod(typeof(TSource)), source.Expression, Expression.Constant(index, typeof(int))), CancellationToken.None);
#endif
        }

        public static Task<TSource> ElementAt<TSource>(this IAsyncQueryable<TSource> source, int index, CancellationToken cancellationToken)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));

#if CRIPPLED_REFLECTION
            return source.Provider.ExecuteAsync<TSource>(Expression.Call(InfoOf(() => AsyncQueryable.ElementAt<TSource>(default(IAsyncQueryable<TSource>), default(int), default(CancellationToken))), source.Expression, Expression.Constant(index, typeof(int)), Expression.Constant(cancellationToken, typeof(CancellationToken))), cancellationToken);
#else
            return source.Provider.ExecuteAsync<TSource>(Expression.Call(((MethodInfo)MethodBase.GetCurrentMethod()).MakeGenericMethod(typeof(TSource)), source.Expression, Expression.Constant(index, typeof(int)), Expression.Constant(cancellationToken, typeof(CancellationToken))), cancellationToken);
#endif
        }

        public static Task<TSource> ElementAtOrDefault<TSource>(this IAsyncQueryable<TSource> source, int index)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));

#if CRIPPLED_REFLECTION
            return source.Provider.ExecuteAsync<TSource>(Expression.Call(InfoOf(() => AsyncQueryable.ElementAtOrDefault<TSource>(default(IAsyncQueryable<TSource>), default(int))), source.Expression, Expression.Constant(index, typeof(int))), CancellationToken.None);
#else
            return source.Provider.ExecuteAsync<TSource>(Expression.Call(((MethodInfo)MethodBase.GetCurrentMethod()).MakeGenericMethod(typeof(TSource)), source.Expression, Expression.Constant(index, typeof(int))), CancellationToken.None);
#endif
        }

        public static Task<TSource> ElementAtOrDefault<TSource>(this IAsyncQueryable<TSource> source, int index, CancellationToken cancellationToken)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));

#if CRIPPLED_REFLECTION
            return source.Provider.ExecuteAsync<TSource>(Expression.Call(InfoOf(() => AsyncQueryable.ElementAtOrDefault<TSource>(default(IAsyncQueryable<TSource>), default(int), default(CancellationToken))), source.Expression, Expression.Constant(index, typeof(int)), Expression.Constant(cancellationToken, typeof(CancellationToken))), cancellationToken);
#else
            return source.Provider.ExecuteAsync<TSource>(Expression.Call(((MethodInfo)MethodBase.GetCurrentMethod()).MakeGenericMethod(typeof(TSource)), source.Expression, Expression.Constant(index, typeof(int)), Expression.Constant(cancellationToken, typeof(CancellationToken))), cancellationToken);
#endif
        }

        public static IAsyncQueryable<TSource> Except<TSource>(this IAsyncQueryable<TSource> first, IAsyncEnumerable<TSource> second)
        {
            if (first == null)
                throw new ArgumentNullException(nameof(first));
            if (second == null)
                throw new ArgumentNullException(nameof(second));

#if CRIPPLED_REFLECTION
            return first.Provider.CreateQuery<TSource>(Expression.Call(InfoOf(() => AsyncQueryable.Except<TSource>(default(IAsyncQueryable<TSource>), default(IAsyncEnumerable<TSource>))), first.Expression, GetSourceExpression(second)));
#else
            return first.Provider.CreateQuery<TSource>(Expression.Call(((MethodInfo)MethodBase.GetCurrentMethod()).MakeGenericMethod(typeof(TSource)), first.Expression, GetSourceExpression(second)));
#endif
        }

        public static IAsyncQueryable<TSource> Except<TSource>(this IAsyncQueryable<TSource> first, IAsyncEnumerable<TSource> second, IEqualityComparer<TSource> comparer)
        {
            if (first == null)
                throw new ArgumentNullException(nameof(first));
            if (second == null)
                throw new ArgumentNullException(nameof(second));
            if (comparer == null)
                throw new ArgumentNullException(nameof(comparer));

#if CRIPPLED_REFLECTION
            return first.Provider.CreateQuery<TSource>(Expression.Call(InfoOf(() => AsyncQueryable.Except<TSource>(default(IAsyncQueryable<TSource>), default(IAsyncEnumerable<TSource>), default(IEqualityComparer<TSource>))), first.Expression, GetSourceExpression(second), Expression.Constant(comparer, typeof(IEqualityComparer<TSource>))));
#else
            return first.Provider.CreateQuery<TSource>(Expression.Call(((MethodInfo)MethodBase.GetCurrentMethod()).MakeGenericMethod(typeof(TSource)), first.Expression, GetSourceExpression(second), Expression.Constant(comparer, typeof(IEqualityComparer<TSource>))));
#endif
        }

        public static Task<TSource> First<TSource>(this IAsyncQueryable<TSource> source)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));

#if CRIPPLED_REFLECTION
            return source.Provider.ExecuteAsync<TSource>(Expression.Call(InfoOf(() => AsyncQueryable.First<TSource>(default(IAsyncQueryable<TSource>))), source.Expression), CancellationToken.None);
#else
            return source.Provider.ExecuteAsync<TSource>(Expression.Call(((MethodInfo)MethodBase.GetCurrentMethod()).MakeGenericMethod(typeof(TSource)), source.Expression), CancellationToken.None);
#endif
        }

        public static Task<TSource> First<TSource>(this IAsyncQueryable<TSource> source, CancellationToken cancellationToken)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));

#if CRIPPLED_REFLECTION
            return source.Provider.ExecuteAsync<TSource>(Expression.Call(InfoOf(() => AsyncQueryable.First<TSource>(default(IAsyncQueryable<TSource>), default(CancellationToken))), source.Expression, Expression.Constant(cancellationToken, typeof(CancellationToken))), cancellationToken);
#else
            return source.Provider.ExecuteAsync<TSource>(Expression.Call(((MethodInfo)MethodBase.GetCurrentMethod()).MakeGenericMethod(typeof(TSource)), source.Expression, Expression.Constant(cancellationToken, typeof(CancellationToken))), cancellationToken);
#endif
        }

        public static Task<TSource> First<TSource>(this IAsyncQueryable<TSource> source, Expression<Func<TSource, bool>> predicate)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));
            if (predicate == null)
                throw new ArgumentNullException(nameof(predicate));

#if CRIPPLED_REFLECTION
            return source.Provider.ExecuteAsync<TSource>(Expression.Call(InfoOf(() => AsyncQueryable.First<TSource>(default(IAsyncQueryable<TSource>), default(Expression<Func<TSource, bool>>))), source.Expression, predicate), CancellationToken.None);
#else
            return source.Provider.ExecuteAsync<TSource>(Expression.Call(((MethodInfo)MethodBase.GetCurrentMethod()).MakeGenericMethod(typeof(TSource)), source.Expression, predicate), CancellationToken.None);
#endif
        }

        public static Task<TSource> First<TSource>(this IAsyncQueryable<TSource> source, Expression<Func<TSource, Task<bool>>> predicate)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));
            if (predicate == null)
                throw new ArgumentNullException(nameof(predicate));

#if CRIPPLED_REFLECTION
            return source.Provider.ExecuteAsync<TSource>(Expression.Call(InfoOf(() => AsyncQueryable.First<TSource>(default(IAsyncQueryable<TSource>), default(Expression<Func<TSource, Task<bool>>>))), source.Expression, predicate), CancellationToken.None);
#else
            return source.Provider.ExecuteAsync<TSource>(Expression.Call(((MethodInfo)MethodBase.GetCurrentMethod()).MakeGenericMethod(typeof(TSource)), source.Expression, predicate), CancellationToken.None);
#endif
        }

        public static Task<TSource> First<TSource>(this IAsyncQueryable<TSource> source, Expression<Func<TSource, bool>> predicate, CancellationToken cancellationToken)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));
            if (predicate == null)
                throw new ArgumentNullException(nameof(predicate));

#if CRIPPLED_REFLECTION
            return source.Provider.ExecuteAsync<TSource>(Expression.Call(InfoOf(() => AsyncQueryable.First<TSource>(default(IAsyncQueryable<TSource>), default(Expression<Func<TSource, bool>>), default(CancellationToken))), source.Expression, predicate, Expression.Constant(cancellationToken, typeof(CancellationToken))), cancellationToken);
#else
            return source.Provider.ExecuteAsync<TSource>(Expression.Call(((MethodInfo)MethodBase.GetCurrentMethod()).MakeGenericMethod(typeof(TSource)), source.Expression, predicate, Expression.Constant(cancellationToken, typeof(CancellationToken))), cancellationToken);
#endif
        }

        public static Task<TSource> First<TSource>(this IAsyncQueryable<TSource> source, Expression<Func<TSource, Task<bool>>> predicate, CancellationToken cancellationToken)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));
            if (predicate == null)
                throw new ArgumentNullException(nameof(predicate));

#if CRIPPLED_REFLECTION
            return source.Provider.ExecuteAsync<TSource>(Expression.Call(InfoOf(() => AsyncQueryable.First<TSource>(default(IAsyncQueryable<TSource>), default(Expression<Func<TSource, Task<bool>>>), default(CancellationToken))), source.Expression, predicate, Expression.Constant(cancellationToken, typeof(CancellationToken))), cancellationToken);
#else
            return source.Provider.ExecuteAsync<TSource>(Expression.Call(((MethodInfo)MethodBase.GetCurrentMethod()).MakeGenericMethod(typeof(TSource)), source.Expression, predicate, Expression.Constant(cancellationToken, typeof(CancellationToken))), cancellationToken);
#endif
        }

        public static Task<TSource> FirstOrDefault<TSource>(this IAsyncQueryable<TSource> source)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));

#if CRIPPLED_REFLECTION
            return source.Provider.ExecuteAsync<TSource>(Expression.Call(InfoOf(() => AsyncQueryable.FirstOrDefault<TSource>(default(IAsyncQueryable<TSource>))), source.Expression), CancellationToken.None);
#else
            return source.Provider.ExecuteAsync<TSource>(Expression.Call(((MethodInfo)MethodBase.GetCurrentMethod()).MakeGenericMethod(typeof(TSource)), source.Expression), CancellationToken.None);
#endif
        }

        public static Task<TSource> FirstOrDefault<TSource>(this IAsyncQueryable<TSource> source, CancellationToken cancellationToken)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));

#if CRIPPLED_REFLECTION
            return source.Provider.ExecuteAsync<TSource>(Expression.Call(InfoOf(() => AsyncQueryable.FirstOrDefault<TSource>(default(IAsyncQueryable<TSource>), default(CancellationToken))), source.Expression, Expression.Constant(cancellationToken, typeof(CancellationToken))), cancellationToken);
#else
            return source.Provider.ExecuteAsync<TSource>(Expression.Call(((MethodInfo)MethodBase.GetCurrentMethod()).MakeGenericMethod(typeof(TSource)), source.Expression, Expression.Constant(cancellationToken, typeof(CancellationToken))), cancellationToken);
#endif
        }

        public static Task<TSource> FirstOrDefault<TSource>(this IAsyncQueryable<TSource> source, Expression<Func<TSource, bool>> predicate)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));
            if (predicate == null)
                throw new ArgumentNullException(nameof(predicate));

#if CRIPPLED_REFLECTION
            return source.Provider.ExecuteAsync<TSource>(Expression.Call(InfoOf(() => AsyncQueryable.FirstOrDefault<TSource>(default(IAsyncQueryable<TSource>), default(Expression<Func<TSource, bool>>))), source.Expression, predicate), CancellationToken.None);
#else
            return source.Provider.ExecuteAsync<TSource>(Expression.Call(((MethodInfo)MethodBase.GetCurrentMethod()).MakeGenericMethod(typeof(TSource)), source.Expression, predicate), CancellationToken.None);
#endif
        }

        public static Task<TSource> FirstOrDefault<TSource>(this IAsyncQueryable<TSource> source, Expression<Func<TSource, Task<bool>>> predicate)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));
            if (predicate == null)
                throw new ArgumentNullException(nameof(predicate));

#if CRIPPLED_REFLECTION
            return source.Provider.ExecuteAsync<TSource>(Expression.Call(InfoOf(() => AsyncQueryable.FirstOrDefault<TSource>(default(IAsyncQueryable<TSource>), default(Expression<Func<TSource, Task<bool>>>))), source.Expression, predicate), CancellationToken.None);
#else
            return source.Provider.ExecuteAsync<TSource>(Expression.Call(((MethodInfo)MethodBase.GetCurrentMethod()).MakeGenericMethod(typeof(TSource)), source.Expression, predicate), CancellationToken.None);
#endif
        }

        public static Task<TSource> FirstOrDefault<TSource>(this IAsyncQueryable<TSource> source, Expression<Func<TSource, bool>> predicate, CancellationToken cancellationToken)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));
            if (predicate == null)
                throw new ArgumentNullException(nameof(predicate));

#if CRIPPLED_REFLECTION
            return source.Provider.ExecuteAsync<TSource>(Expression.Call(InfoOf(() => AsyncQueryable.FirstOrDefault<TSource>(default(IAsyncQueryable<TSource>), default(Expression<Func<TSource, bool>>), default(CancellationToken))), source.Expression, predicate, Expression.Constant(cancellationToken, typeof(CancellationToken))), cancellationToken);
#else
            return source.Provider.ExecuteAsync<TSource>(Expression.Call(((MethodInfo)MethodBase.GetCurrentMethod()).MakeGenericMethod(typeof(TSource)), source.Expression, predicate, Expression.Constant(cancellationToken, typeof(CancellationToken))), cancellationToken);
#endif
        }

        public static Task<TSource> FirstOrDefault<TSource>(this IAsyncQueryable<TSource> source, Expression<Func<TSource, Task<bool>>> predicate, CancellationToken cancellationToken)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));
            if (predicate == null)
                throw new ArgumentNullException(nameof(predicate));

#if CRIPPLED_REFLECTION
            return source.Provider.ExecuteAsync<TSource>(Expression.Call(InfoOf(() => AsyncQueryable.FirstOrDefault<TSource>(default(IAsyncQueryable<TSource>), default(Expression<Func<TSource, Task<bool>>>), default(CancellationToken))), source.Expression, predicate, Expression.Constant(cancellationToken, typeof(CancellationToken))), cancellationToken);
#else
            return source.Provider.ExecuteAsync<TSource>(Expression.Call(((MethodInfo)MethodBase.GetCurrentMethod()).MakeGenericMethod(typeof(TSource)), source.Expression, predicate, Expression.Constant(cancellationToken, typeof(CancellationToken))), cancellationToken);
#endif
        }

        public static IAsyncQueryable<IAsyncGrouping<TKey, TSource>> GroupBy<TSource, TKey>(this IAsyncQueryable<TSource> source, Expression<Func<TSource, Task<TKey>>> keySelector)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));
            if (keySelector == null)
                throw new ArgumentNullException(nameof(keySelector));

#if CRIPPLED_REFLECTION
            return source.Provider.CreateQuery<IAsyncGrouping<TKey, TSource>>(Expression.Call(InfoOf(() => AsyncQueryable.GroupBy<TSource, TKey>(default(IAsyncQueryable<TSource>), default(Expression<Func<TSource, Task<TKey>>>))), source.Expression, keySelector));
#else
            return source.Provider.CreateQuery<IAsyncGrouping<TKey, TSource>>(Expression.Call(((MethodInfo)MethodBase.GetCurrentMethod()).MakeGenericMethod(typeof(TSource), typeof(TKey)), source.Expression, keySelector));
#endif
        }

        public static IAsyncQueryable<IAsyncGrouping<TKey, TSource>> GroupBy<TSource, TKey>(this IAsyncQueryable<TSource> source, Expression<Func<TSource, TKey>> keySelector)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));
            if (keySelector == null)
                throw new ArgumentNullException(nameof(keySelector));

#if CRIPPLED_REFLECTION
            return source.Provider.CreateQuery<IAsyncGrouping<TKey, TSource>>(Expression.Call(InfoOf(() => AsyncQueryable.GroupBy<TSource, TKey>(default(IAsyncQueryable<TSource>), default(Expression<Func<TSource, TKey>>))), source.Expression, keySelector));
#else
            return source.Provider.CreateQuery<IAsyncGrouping<TKey, TSource>>(Expression.Call(((MethodInfo)MethodBase.GetCurrentMethod()).MakeGenericMethod(typeof(TSource), typeof(TKey)), source.Expression, keySelector));
#endif
        }

        public static IAsyncQueryable<IAsyncGrouping<TKey, TSource>> GroupBy<TSource, TKey>(this IAsyncQueryable<TSource> source, Expression<Func<TSource, Task<TKey>>> keySelector, IEqualityComparer<TKey> comparer)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));
            if (keySelector == null)
                throw new ArgumentNullException(nameof(keySelector));
            if (comparer == null)
                throw new ArgumentNullException(nameof(comparer));

#if CRIPPLED_REFLECTION
            return source.Provider.CreateQuery<IAsyncGrouping<TKey, TSource>>(Expression.Call(InfoOf(() => AsyncQueryable.GroupBy<TSource, TKey>(default(IAsyncQueryable<TSource>), default(Expression<Func<TSource, Task<TKey>>>), default(IEqualityComparer<TKey>))), source.Expression, keySelector, Expression.Constant(comparer, typeof(IEqualityComparer<TKey>))));
#else
            return source.Provider.CreateQuery<IAsyncGrouping<TKey, TSource>>(Expression.Call(((MethodInfo)MethodBase.GetCurrentMethod()).MakeGenericMethod(typeof(TSource), typeof(TKey)), source.Expression, keySelector, Expression.Constant(comparer, typeof(IEqualityComparer<TKey>))));
#endif
        }

        public static IAsyncQueryable<IAsyncGrouping<TKey, TSource>> GroupBy<TSource, TKey>(this IAsyncQueryable<TSource> source, Expression<Func<TSource, TKey>> keySelector, IEqualityComparer<TKey> comparer)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));
            if (keySelector == null)
                throw new ArgumentNullException(nameof(keySelector));
            if (comparer == null)
                throw new ArgumentNullException(nameof(comparer));

#if CRIPPLED_REFLECTION
            return source.Provider.CreateQuery<IAsyncGrouping<TKey, TSource>>(Expression.Call(InfoOf(() => AsyncQueryable.GroupBy<TSource, TKey>(default(IAsyncQueryable<TSource>), default(Expression<Func<TSource, TKey>>), default(IEqualityComparer<TKey>))), source.Expression, keySelector, Expression.Constant(comparer, typeof(IEqualityComparer<TKey>))));
#else
            return source.Provider.CreateQuery<IAsyncGrouping<TKey, TSource>>(Expression.Call(((MethodInfo)MethodBase.GetCurrentMethod()).MakeGenericMethod(typeof(TSource), typeof(TKey)), source.Expression, keySelector, Expression.Constant(comparer, typeof(IEqualityComparer<TKey>))));
#endif
        }

        public static IAsyncQueryable<TResult> GroupBy<TSource, TKey, TResult>(this IAsyncQueryable<TSource> source, Expression<Func<TSource, Task<TKey>>> keySelector, Expression<Func<TKey, IAsyncEnumerable<TSource>, Task<TResult>>> resultSelector)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));
            if (keySelector == null)
                throw new ArgumentNullException(nameof(keySelector));
            if (resultSelector == null)
                throw new ArgumentNullException(nameof(resultSelector));

#if CRIPPLED_REFLECTION
            return source.Provider.CreateQuery<TResult>(Expression.Call(InfoOf(() => AsyncQueryable.GroupBy<TSource, TKey, TResult>(default(IAsyncQueryable<TSource>), default(Expression<Func<TSource, Task<TKey>>>), default(Expression<Func<TKey, IAsyncEnumerable<TSource>, Task<TResult>>>))), source.Expression, keySelector, resultSelector));
#else
            return source.Provider.CreateQuery<TResult>(Expression.Call(((MethodInfo)MethodBase.GetCurrentMethod()).MakeGenericMethod(typeof(TSource), typeof(TKey), typeof(TResult)), source.Expression, keySelector, resultSelector));
#endif
        }

        public static IAsyncQueryable<IAsyncGrouping<TKey, TElement>> GroupBy<TSource, TKey, TElement>(this IAsyncQueryable<TSource> source, Expression<Func<TSource, Task<TKey>>> keySelector, Expression<Func<TSource, Task<TElement>>> elementSelector)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));
            if (keySelector == null)
                throw new ArgumentNullException(nameof(keySelector));
            if (elementSelector == null)
                throw new ArgumentNullException(nameof(elementSelector));

#if CRIPPLED_REFLECTION
            return source.Provider.CreateQuery<IAsyncGrouping<TKey, TElement>>(Expression.Call(InfoOf(() => AsyncQueryable.GroupBy<TSource, TKey, TElement>(default(IAsyncQueryable<TSource>), default(Expression<Func<TSource, Task<TKey>>>), default(Expression<Func<TSource, Task<TElement>>>))), source.Expression, keySelector, elementSelector));
#else
            return source.Provider.CreateQuery<IAsyncGrouping<TKey, TElement>>(Expression.Call(((MethodInfo)MethodBase.GetCurrentMethod()).MakeGenericMethod(typeof(TSource), typeof(TKey), typeof(TElement)), source.Expression, keySelector, elementSelector));
#endif
        }

        public static IAsyncQueryable<TResult> GroupBy<TSource, TKey, TResult>(this IAsyncQueryable<TSource> source, Expression<Func<TSource, TKey>> keySelector, Expression<Func<TKey, IAsyncEnumerable<TSource>, TResult>> resultSelector)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));
            if (keySelector == null)
                throw new ArgumentNullException(nameof(keySelector));
            if (resultSelector == null)
                throw new ArgumentNullException(nameof(resultSelector));

#if CRIPPLED_REFLECTION
            return source.Provider.CreateQuery<TResult>(Expression.Call(InfoOf(() => AsyncQueryable.GroupBy<TSource, TKey, TResult>(default(IAsyncQueryable<TSource>), default(Expression<Func<TSource, TKey>>), default(Expression<Func<TKey, IAsyncEnumerable<TSource>, TResult>>))), source.Expression, keySelector, resultSelector));
#else
            return source.Provider.CreateQuery<TResult>(Expression.Call(((MethodInfo)MethodBase.GetCurrentMethod()).MakeGenericMethod(typeof(TSource), typeof(TKey), typeof(TResult)), source.Expression, keySelector, resultSelector));
#endif
        }

        public static IAsyncQueryable<IAsyncGrouping<TKey, TElement>> GroupBy<TSource, TKey, TElement>(this IAsyncQueryable<TSource> source, Expression<Func<TSource, TKey>> keySelector, Expression<Func<TSource, TElement>> elementSelector)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));
            if (keySelector == null)
                throw new ArgumentNullException(nameof(keySelector));
            if (elementSelector == null)
                throw new ArgumentNullException(nameof(elementSelector));

#if CRIPPLED_REFLECTION
            return source.Provider.CreateQuery<IAsyncGrouping<TKey, TElement>>(Expression.Call(InfoOf(() => AsyncQueryable.GroupBy<TSource, TKey, TElement>(default(IAsyncQueryable<TSource>), default(Expression<Func<TSource, TKey>>), default(Expression<Func<TSource, TElement>>))), source.Expression, keySelector, elementSelector));
#else
            return source.Provider.CreateQuery<IAsyncGrouping<TKey, TElement>>(Expression.Call(((MethodInfo)MethodBase.GetCurrentMethod()).MakeGenericMethod(typeof(TSource), typeof(TKey), typeof(TElement)), source.Expression, keySelector, elementSelector));
#endif
        }

        public static IAsyncQueryable<TResult> GroupBy<TSource, TKey, TResult>(this IAsyncQueryable<TSource> source, Expression<Func<TSource, Task<TKey>>> keySelector, Expression<Func<TKey, IAsyncEnumerable<TSource>, Task<TResult>>> resultSelector, IEqualityComparer<TKey> comparer)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));
            if (keySelector == null)
                throw new ArgumentNullException(nameof(keySelector));
            if (resultSelector == null)
                throw new ArgumentNullException(nameof(resultSelector));
            if (comparer == null)
                throw new ArgumentNullException(nameof(comparer));

#if CRIPPLED_REFLECTION
            return source.Provider.CreateQuery<TResult>(Expression.Call(InfoOf(() => AsyncQueryable.GroupBy<TSource, TKey, TResult>(default(IAsyncQueryable<TSource>), default(Expression<Func<TSource, Task<TKey>>>), default(Expression<Func<TKey, IAsyncEnumerable<TSource>, Task<TResult>>>), default(IEqualityComparer<TKey>))), source.Expression, keySelector, resultSelector, Expression.Constant(comparer, typeof(IEqualityComparer<TKey>))));
#else
            return source.Provider.CreateQuery<TResult>(Expression.Call(((MethodInfo)MethodBase.GetCurrentMethod()).MakeGenericMethod(typeof(TSource), typeof(TKey), typeof(TResult)), source.Expression, keySelector, resultSelector, Expression.Constant(comparer, typeof(IEqualityComparer<TKey>))));
#endif
        }

        public static IAsyncQueryable<IAsyncGrouping<TKey, TElement>> GroupBy<TSource, TKey, TElement>(this IAsyncQueryable<TSource> source, Expression<Func<TSource, Task<TKey>>> keySelector, Expression<Func<TSource, Task<TElement>>> elementSelector, IEqualityComparer<TKey> comparer)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));
            if (keySelector == null)
                throw new ArgumentNullException(nameof(keySelector));
            if (elementSelector == null)
                throw new ArgumentNullException(nameof(elementSelector));
            if (comparer == null)
                throw new ArgumentNullException(nameof(comparer));

#if CRIPPLED_REFLECTION
            return source.Provider.CreateQuery<IAsyncGrouping<TKey, TElement>>(Expression.Call(InfoOf(() => AsyncQueryable.GroupBy<TSource, TKey, TElement>(default(IAsyncQueryable<TSource>), default(Expression<Func<TSource, Task<TKey>>>), default(Expression<Func<TSource, Task<TElement>>>), default(IEqualityComparer<TKey>))), source.Expression, keySelector, elementSelector, Expression.Constant(comparer, typeof(IEqualityComparer<TKey>))));
#else
            return source.Provider.CreateQuery<IAsyncGrouping<TKey, TElement>>(Expression.Call(((MethodInfo)MethodBase.GetCurrentMethod()).MakeGenericMethod(typeof(TSource), typeof(TKey), typeof(TElement)), source.Expression, keySelector, elementSelector, Expression.Constant(comparer, typeof(IEqualityComparer<TKey>))));
#endif
        }

        public static IAsyncQueryable<TResult> GroupBy<TSource, TKey, TResult>(this IAsyncQueryable<TSource> source, Expression<Func<TSource, TKey>> keySelector, Expression<Func<TKey, IAsyncEnumerable<TSource>, TResult>> resultSelector, IEqualityComparer<TKey> comparer)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));
            if (keySelector == null)
                throw new ArgumentNullException(nameof(keySelector));
            if (resultSelector == null)
                throw new ArgumentNullException(nameof(resultSelector));
            if (comparer == null)
                throw new ArgumentNullException(nameof(comparer));

#if CRIPPLED_REFLECTION
            return source.Provider.CreateQuery<TResult>(Expression.Call(InfoOf(() => AsyncQueryable.GroupBy<TSource, TKey, TResult>(default(IAsyncQueryable<TSource>), default(Expression<Func<TSource, TKey>>), default(Expression<Func<TKey, IAsyncEnumerable<TSource>, TResult>>), default(IEqualityComparer<TKey>))), source.Expression, keySelector, resultSelector, Expression.Constant(comparer, typeof(IEqualityComparer<TKey>))));
#else
            return source.Provider.CreateQuery<TResult>(Expression.Call(((MethodInfo)MethodBase.GetCurrentMethod()).MakeGenericMethod(typeof(TSource), typeof(TKey), typeof(TResult)), source.Expression, keySelector, resultSelector, Expression.Constant(comparer, typeof(IEqualityComparer<TKey>))));
#endif
        }

        public static IAsyncQueryable<IAsyncGrouping<TKey, TElement>> GroupBy<TSource, TKey, TElement>(this IAsyncQueryable<TSource> source, Expression<Func<TSource, TKey>> keySelector, Expression<Func<TSource, TElement>> elementSelector, IEqualityComparer<TKey> comparer)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));
            if (keySelector == null)
                throw new ArgumentNullException(nameof(keySelector));
            if (elementSelector == null)
                throw new ArgumentNullException(nameof(elementSelector));
            if (comparer == null)
                throw new ArgumentNullException(nameof(comparer));

#if CRIPPLED_REFLECTION
            return source.Provider.CreateQuery<IAsyncGrouping<TKey, TElement>>(Expression.Call(InfoOf(() => AsyncQueryable.GroupBy<TSource, TKey, TElement>(default(IAsyncQueryable<TSource>), default(Expression<Func<TSource, TKey>>), default(Expression<Func<TSource, TElement>>), default(IEqualityComparer<TKey>))), source.Expression, keySelector, elementSelector, Expression.Constant(comparer, typeof(IEqualityComparer<TKey>))));
#else
            return source.Provider.CreateQuery<IAsyncGrouping<TKey, TElement>>(Expression.Call(((MethodInfo)MethodBase.GetCurrentMethod()).MakeGenericMethod(typeof(TSource), typeof(TKey), typeof(TElement)), source.Expression, keySelector, elementSelector, Expression.Constant(comparer, typeof(IEqualityComparer<TKey>))));
#endif
        }

        public static IAsyncQueryable<TResult> GroupBy<TSource, TKey, TElement, TResult>(this IAsyncQueryable<TSource> source, Expression<Func<TSource, Task<TKey>>> keySelector, Expression<Func<TSource, Task<TElement>>> elementSelector, Expression<Func<TKey, IAsyncEnumerable<TElement>, Task<TResult>>> resultSelector)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));
            if (keySelector == null)
                throw new ArgumentNullException(nameof(keySelector));
            if (elementSelector == null)
                throw new ArgumentNullException(nameof(elementSelector));
            if (resultSelector == null)
                throw new ArgumentNullException(nameof(resultSelector));

#if CRIPPLED_REFLECTION
            return source.Provider.CreateQuery<TResult>(Expression.Call(InfoOf(() => AsyncQueryable.GroupBy<TSource, TKey, TElement, TResult>(default(IAsyncQueryable<TSource>), default(Expression<Func<TSource, Task<TKey>>>), default(Expression<Func<TSource, Task<TElement>>>), default(Expression<Func<TKey, IAsyncEnumerable<TElement>, Task<TResult>>>))), source.Expression, keySelector, elementSelector, resultSelector));
#else
            return source.Provider.CreateQuery<TResult>(Expression.Call(((MethodInfo)MethodBase.GetCurrentMethod()).MakeGenericMethod(typeof(TSource), typeof(TKey), typeof(TElement), typeof(TResult)), source.Expression, keySelector, elementSelector, resultSelector));
#endif
        }

        public static IAsyncQueryable<TResult> GroupBy<TSource, TKey, TElement, TResult>(this IAsyncQueryable<TSource> source, Expression<Func<TSource, TKey>> keySelector, Expression<Func<TSource, TElement>> elementSelector, Expression<Func<TKey, IAsyncEnumerable<TElement>, TResult>> resultSelector)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));
            if (keySelector == null)
                throw new ArgumentNullException(nameof(keySelector));
            if (elementSelector == null)
                throw new ArgumentNullException(nameof(elementSelector));
            if (resultSelector == null)
                throw new ArgumentNullException(nameof(resultSelector));

#if CRIPPLED_REFLECTION
            return source.Provider.CreateQuery<TResult>(Expression.Call(InfoOf(() => AsyncQueryable.GroupBy<TSource, TKey, TElement, TResult>(default(IAsyncQueryable<TSource>), default(Expression<Func<TSource, TKey>>), default(Expression<Func<TSource, TElement>>), default(Expression<Func<TKey, IAsyncEnumerable<TElement>, TResult>>))), source.Expression, keySelector, elementSelector, resultSelector));
#else
            return source.Provider.CreateQuery<TResult>(Expression.Call(((MethodInfo)MethodBase.GetCurrentMethod()).MakeGenericMethod(typeof(TSource), typeof(TKey), typeof(TElement), typeof(TResult)), source.Expression, keySelector, elementSelector, resultSelector));
#endif
        }

        public static IAsyncQueryable<TResult> GroupBy<TSource, TKey, TElement, TResult>(this IAsyncQueryable<TSource> source, Expression<Func<TSource, Task<TKey>>> keySelector, Expression<Func<TSource, Task<TElement>>> elementSelector, Expression<Func<TKey, IAsyncEnumerable<TElement>, Task<TResult>>> resultSelector, IEqualityComparer<TKey> comparer)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));
            if (keySelector == null)
                throw new ArgumentNullException(nameof(keySelector));
            if (elementSelector == null)
                throw new ArgumentNullException(nameof(elementSelector));
            if (resultSelector == null)
                throw new ArgumentNullException(nameof(resultSelector));
            if (comparer == null)
                throw new ArgumentNullException(nameof(comparer));

#if CRIPPLED_REFLECTION
            return source.Provider.CreateQuery<TResult>(Expression.Call(InfoOf(() => AsyncQueryable.GroupBy<TSource, TKey, TElement, TResult>(default(IAsyncQueryable<TSource>), default(Expression<Func<TSource, Task<TKey>>>), default(Expression<Func<TSource, Task<TElement>>>), default(Expression<Func<TKey, IAsyncEnumerable<TElement>, Task<TResult>>>), default(IEqualityComparer<TKey>))), source.Expression, keySelector, elementSelector, resultSelector, Expression.Constant(comparer, typeof(IEqualityComparer<TKey>))));
#else
            return source.Provider.CreateQuery<TResult>(Expression.Call(((MethodInfo)MethodBase.GetCurrentMethod()).MakeGenericMethod(typeof(TSource), typeof(TKey), typeof(TElement), typeof(TResult)), source.Expression, keySelector, elementSelector, resultSelector, Expression.Constant(comparer, typeof(IEqualityComparer<TKey>))));
#endif
        }

        public static IAsyncQueryable<TResult> GroupBy<TSource, TKey, TElement, TResult>(this IAsyncQueryable<TSource> source, Expression<Func<TSource, TKey>> keySelector, Expression<Func<TSource, TElement>> elementSelector, Expression<Func<TKey, IAsyncEnumerable<TElement>, TResult>> resultSelector, IEqualityComparer<TKey> comparer)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));
            if (keySelector == null)
                throw new ArgumentNullException(nameof(keySelector));
            if (elementSelector == null)
                throw new ArgumentNullException(nameof(elementSelector));
            if (resultSelector == null)
                throw new ArgumentNullException(nameof(resultSelector));
            if (comparer == null)
                throw new ArgumentNullException(nameof(comparer));

#if CRIPPLED_REFLECTION
            return source.Provider.CreateQuery<TResult>(Expression.Call(InfoOf(() => AsyncQueryable.GroupBy<TSource, TKey, TElement, TResult>(default(IAsyncQueryable<TSource>), default(Expression<Func<TSource, TKey>>), default(Expression<Func<TSource, TElement>>), default(Expression<Func<TKey, IAsyncEnumerable<TElement>, TResult>>), default(IEqualityComparer<TKey>))), source.Expression, keySelector, elementSelector, resultSelector, Expression.Constant(comparer, typeof(IEqualityComparer<TKey>))));
#else
            return source.Provider.CreateQuery<TResult>(Expression.Call(((MethodInfo)MethodBase.GetCurrentMethod()).MakeGenericMethod(typeof(TSource), typeof(TKey), typeof(TElement), typeof(TResult)), source.Expression, keySelector, elementSelector, resultSelector, Expression.Constant(comparer, typeof(IEqualityComparer<TKey>))));
#endif
        }

        public static IAsyncQueryable<TResult> GroupJoin<TOuter, TInner, TKey, TResult>(this IAsyncQueryable<TOuter> outer, IAsyncEnumerable<TInner> inner, Expression<Func<TOuter, Task<TKey>>> outerKeySelector, Expression<Func<TInner, Task<TKey>>> innerKeySelector, Expression<Func<TOuter, IAsyncEnumerable<TInner>, Task<TResult>>> resultSelector)
        {
            if (outer == null)
                throw new ArgumentNullException(nameof(outer));
            if (inner == null)
                throw new ArgumentNullException(nameof(inner));
            if (outerKeySelector == null)
                throw new ArgumentNullException(nameof(outerKeySelector));
            if (innerKeySelector == null)
                throw new ArgumentNullException(nameof(innerKeySelector));
            if (resultSelector == null)
                throw new ArgumentNullException(nameof(resultSelector));

#if CRIPPLED_REFLECTION
            return outer.Provider.CreateQuery<TResult>(Expression.Call(InfoOf(() => AsyncQueryable.GroupJoin<TOuter, TInner, TKey, TResult>(default(IAsyncQueryable<TOuter>), default(IAsyncEnumerable<TInner>), default(Expression<Func<TOuter, Task<TKey>>>), default(Expression<Func<TInner, Task<TKey>>>), default(Expression<Func<TOuter, IAsyncEnumerable<TInner>, Task<TResult>>>))), outer.Expression, GetSourceExpression(inner), outerKeySelector, innerKeySelector, resultSelector));
#else
            return outer.Provider.CreateQuery<TResult>(Expression.Call(((MethodInfo)MethodBase.GetCurrentMethod()).MakeGenericMethod(typeof(TOuter), typeof(TInner), typeof(TKey), typeof(TResult)), outer.Expression, GetSourceExpression(inner), outerKeySelector, innerKeySelector, resultSelector));
#endif
        }

        public static IAsyncQueryable<TResult> GroupJoin<TOuter, TInner, TKey, TResult>(this IAsyncQueryable<TOuter> outer, IAsyncEnumerable<TInner> inner, Expression<Func<TOuter, TKey>> outerKeySelector, Expression<Func<TInner, TKey>> innerKeySelector, Expression<Func<TOuter, IAsyncEnumerable<TInner>, TResult>> resultSelector)
        {
            if (outer == null)
                throw new ArgumentNullException(nameof(outer));
            if (inner == null)
                throw new ArgumentNullException(nameof(inner));
            if (outerKeySelector == null)
                throw new ArgumentNullException(nameof(outerKeySelector));
            if (innerKeySelector == null)
                throw new ArgumentNullException(nameof(innerKeySelector));
            if (resultSelector == null)
                throw new ArgumentNullException(nameof(resultSelector));

#if CRIPPLED_REFLECTION
            return outer.Provider.CreateQuery<TResult>(Expression.Call(InfoOf(() => AsyncQueryable.GroupJoin<TOuter, TInner, TKey, TResult>(default(IAsyncQueryable<TOuter>), default(IAsyncEnumerable<TInner>), default(Expression<Func<TOuter, TKey>>), default(Expression<Func<TInner, TKey>>), default(Expression<Func<TOuter, IAsyncEnumerable<TInner>, TResult>>))), outer.Expression, GetSourceExpression(inner), outerKeySelector, innerKeySelector, resultSelector));
#else
            return outer.Provider.CreateQuery<TResult>(Expression.Call(((MethodInfo)MethodBase.GetCurrentMethod()).MakeGenericMethod(typeof(TOuter), typeof(TInner), typeof(TKey), typeof(TResult)), outer.Expression, GetSourceExpression(inner), outerKeySelector, innerKeySelector, resultSelector));
#endif
        }

        public static IAsyncQueryable<TResult> GroupJoin<TOuter, TInner, TKey, TResult>(this IAsyncQueryable<TOuter> outer, IAsyncEnumerable<TInner> inner, Expression<Func<TOuter, Task<TKey>>> outerKeySelector, Expression<Func<TInner, Task<TKey>>> innerKeySelector, Expression<Func<TOuter, IAsyncEnumerable<TInner>, Task<TResult>>> resultSelector, IEqualityComparer<TKey> comparer)
        {
            if (outer == null)
                throw new ArgumentNullException(nameof(outer));
            if (inner == null)
                throw new ArgumentNullException(nameof(inner));
            if (outerKeySelector == null)
                throw new ArgumentNullException(nameof(outerKeySelector));
            if (innerKeySelector == null)
                throw new ArgumentNullException(nameof(innerKeySelector));
            if (resultSelector == null)
                throw new ArgumentNullException(nameof(resultSelector));
            if (comparer == null)
                throw new ArgumentNullException(nameof(comparer));

#if CRIPPLED_REFLECTION
            return outer.Provider.CreateQuery<TResult>(Expression.Call(InfoOf(() => AsyncQueryable.GroupJoin<TOuter, TInner, TKey, TResult>(default(IAsyncQueryable<TOuter>), default(IAsyncEnumerable<TInner>), default(Expression<Func<TOuter, Task<TKey>>>), default(Expression<Func<TInner, Task<TKey>>>), default(Expression<Func<TOuter, IAsyncEnumerable<TInner>, Task<TResult>>>), default(IEqualityComparer<TKey>))), outer.Expression, GetSourceExpression(inner), outerKeySelector, innerKeySelector, resultSelector, Expression.Constant(comparer, typeof(IEqualityComparer<TKey>))));
#else
            return outer.Provider.CreateQuery<TResult>(Expression.Call(((MethodInfo)MethodBase.GetCurrentMethod()).MakeGenericMethod(typeof(TOuter), typeof(TInner), typeof(TKey), typeof(TResult)), outer.Expression, GetSourceExpression(inner), outerKeySelector, innerKeySelector, resultSelector, Expression.Constant(comparer, typeof(IEqualityComparer<TKey>))));
#endif
        }

        public static IAsyncQueryable<TResult> GroupJoin<TOuter, TInner, TKey, TResult>(this IAsyncQueryable<TOuter> outer, IAsyncEnumerable<TInner> inner, Expression<Func<TOuter, TKey>> outerKeySelector, Expression<Func<TInner, TKey>> innerKeySelector, Expression<Func<TOuter, IAsyncEnumerable<TInner>, TResult>> resultSelector, IEqualityComparer<TKey> comparer)
        {
            if (outer == null)
                throw new ArgumentNullException(nameof(outer));
            if (inner == null)
                throw new ArgumentNullException(nameof(inner));
            if (outerKeySelector == null)
                throw new ArgumentNullException(nameof(outerKeySelector));
            if (innerKeySelector == null)
                throw new ArgumentNullException(nameof(innerKeySelector));
            if (resultSelector == null)
                throw new ArgumentNullException(nameof(resultSelector));
            if (comparer == null)
                throw new ArgumentNullException(nameof(comparer));

#if CRIPPLED_REFLECTION
            return outer.Provider.CreateQuery<TResult>(Expression.Call(InfoOf(() => AsyncQueryable.GroupJoin<TOuter, TInner, TKey, TResult>(default(IAsyncQueryable<TOuter>), default(IAsyncEnumerable<TInner>), default(Expression<Func<TOuter, TKey>>), default(Expression<Func<TInner, TKey>>), default(Expression<Func<TOuter, IAsyncEnumerable<TInner>, TResult>>), default(IEqualityComparer<TKey>))), outer.Expression, GetSourceExpression(inner), outerKeySelector, innerKeySelector, resultSelector, Expression.Constant(comparer, typeof(IEqualityComparer<TKey>))));
#else
            return outer.Provider.CreateQuery<TResult>(Expression.Call(((MethodInfo)MethodBase.GetCurrentMethod()).MakeGenericMethod(typeof(TOuter), typeof(TInner), typeof(TKey), typeof(TResult)), outer.Expression, GetSourceExpression(inner), outerKeySelector, innerKeySelector, resultSelector, Expression.Constant(comparer, typeof(IEqualityComparer<TKey>))));
#endif
        }

        public static IAsyncQueryable<TSource> Intersect<TSource>(this IAsyncQueryable<TSource> first, IAsyncEnumerable<TSource> second)
        {
            if (first == null)
                throw new ArgumentNullException(nameof(first));
            if (second == null)
                throw new ArgumentNullException(nameof(second));

#if CRIPPLED_REFLECTION
            return first.Provider.CreateQuery<TSource>(Expression.Call(InfoOf(() => AsyncQueryable.Intersect<TSource>(default(IAsyncQueryable<TSource>), default(IAsyncEnumerable<TSource>))), first.Expression, GetSourceExpression(second)));
#else
            return first.Provider.CreateQuery<TSource>(Expression.Call(((MethodInfo)MethodBase.GetCurrentMethod()).MakeGenericMethod(typeof(TSource)), first.Expression, GetSourceExpression(second)));
#endif
        }

        public static IAsyncQueryable<TSource> Intersect<TSource>(this IAsyncQueryable<TSource> first, IAsyncEnumerable<TSource> second, IEqualityComparer<TSource> comparer)
        {
            if (first == null)
                throw new ArgumentNullException(nameof(first));
            if (second == null)
                throw new ArgumentNullException(nameof(second));
            if (comparer == null)
                throw new ArgumentNullException(nameof(comparer));

#if CRIPPLED_REFLECTION
            return first.Provider.CreateQuery<TSource>(Expression.Call(InfoOf(() => AsyncQueryable.Intersect<TSource>(default(IAsyncQueryable<TSource>), default(IAsyncEnumerable<TSource>), default(IEqualityComparer<TSource>))), first.Expression, GetSourceExpression(second), Expression.Constant(comparer, typeof(IEqualityComparer<TSource>))));
#else
            return first.Provider.CreateQuery<TSource>(Expression.Call(((MethodInfo)MethodBase.GetCurrentMethod()).MakeGenericMethod(typeof(TSource)), first.Expression, GetSourceExpression(second), Expression.Constant(comparer, typeof(IEqualityComparer<TSource>))));
#endif
        }

        public static IAsyncQueryable<TResult> Join<TOuter, TInner, TKey, TResult>(this IAsyncQueryable<TOuter> outer, IAsyncEnumerable<TInner> inner, Expression<Func<TOuter, Task<TKey>>> outerKeySelector, Expression<Func<TInner, Task<TKey>>> innerKeySelector, Expression<Func<TOuter, TInner, Task<TResult>>> resultSelector)
        {
            if (outer == null)
                throw new ArgumentNullException(nameof(outer));
            if (inner == null)
                throw new ArgumentNullException(nameof(inner));
            if (outerKeySelector == null)
                throw new ArgumentNullException(nameof(outerKeySelector));
            if (innerKeySelector == null)
                throw new ArgumentNullException(nameof(innerKeySelector));
            if (resultSelector == null)
                throw new ArgumentNullException(nameof(resultSelector));

#if CRIPPLED_REFLECTION
            return outer.Provider.CreateQuery<TResult>(Expression.Call(InfoOf(() => AsyncQueryable.Join<TOuter, TInner, TKey, TResult>(default(IAsyncQueryable<TOuter>), default(IAsyncEnumerable<TInner>), default(Expression<Func<TOuter, Task<TKey>>>), default(Expression<Func<TInner, Task<TKey>>>), default(Expression<Func<TOuter, TInner, Task<TResult>>>))), outer.Expression, GetSourceExpression(inner), outerKeySelector, innerKeySelector, resultSelector));
#else
            return outer.Provider.CreateQuery<TResult>(Expression.Call(((MethodInfo)MethodBase.GetCurrentMethod()).MakeGenericMethod(typeof(TOuter), typeof(TInner), typeof(TKey), typeof(TResult)), outer.Expression, GetSourceExpression(inner), outerKeySelector, innerKeySelector, resultSelector));
#endif
        }

        public static IAsyncQueryable<TResult> Join<TOuter, TInner, TKey, TResult>(this IAsyncQueryable<TOuter> outer, IAsyncEnumerable<TInner> inner, Expression<Func<TOuter, TKey>> outerKeySelector, Expression<Func<TInner, TKey>> innerKeySelector, Expression<Func<TOuter, TInner, TResult>> resultSelector)
        {
            if (outer == null)
                throw new ArgumentNullException(nameof(outer));
            if (inner == null)
                throw new ArgumentNullException(nameof(inner));
            if (outerKeySelector == null)
                throw new ArgumentNullException(nameof(outerKeySelector));
            if (innerKeySelector == null)
                throw new ArgumentNullException(nameof(innerKeySelector));
            if (resultSelector == null)
                throw new ArgumentNullException(nameof(resultSelector));

#if CRIPPLED_REFLECTION
            return outer.Provider.CreateQuery<TResult>(Expression.Call(InfoOf(() => AsyncQueryable.Join<TOuter, TInner, TKey, TResult>(default(IAsyncQueryable<TOuter>), default(IAsyncEnumerable<TInner>), default(Expression<Func<TOuter, TKey>>), default(Expression<Func<TInner, TKey>>), default(Expression<Func<TOuter, TInner, TResult>>))), outer.Expression, GetSourceExpression(inner), outerKeySelector, innerKeySelector, resultSelector));
#else
            return outer.Provider.CreateQuery<TResult>(Expression.Call(((MethodInfo)MethodBase.GetCurrentMethod()).MakeGenericMethod(typeof(TOuter), typeof(TInner), typeof(TKey), typeof(TResult)), outer.Expression, GetSourceExpression(inner), outerKeySelector, innerKeySelector, resultSelector));
#endif
        }

        public static IAsyncQueryable<TResult> Join<TOuter, TInner, TKey, TResult>(this IAsyncQueryable<TOuter> outer, IAsyncEnumerable<TInner> inner, Expression<Func<TOuter, Task<TKey>>> outerKeySelector, Expression<Func<TInner, Task<TKey>>> innerKeySelector, Expression<Func<TOuter, TInner, Task<TResult>>> resultSelector, IEqualityComparer<TKey> comparer)
        {
            if (outer == null)
                throw new ArgumentNullException(nameof(outer));
            if (inner == null)
                throw new ArgumentNullException(nameof(inner));
            if (outerKeySelector == null)
                throw new ArgumentNullException(nameof(outerKeySelector));
            if (innerKeySelector == null)
                throw new ArgumentNullException(nameof(innerKeySelector));
            if (resultSelector == null)
                throw new ArgumentNullException(nameof(resultSelector));
            if (comparer == null)
                throw new ArgumentNullException(nameof(comparer));

#if CRIPPLED_REFLECTION
            return outer.Provider.CreateQuery<TResult>(Expression.Call(InfoOf(() => AsyncQueryable.Join<TOuter, TInner, TKey, TResult>(default(IAsyncQueryable<TOuter>), default(IAsyncEnumerable<TInner>), default(Expression<Func<TOuter, Task<TKey>>>), default(Expression<Func<TInner, Task<TKey>>>), default(Expression<Func<TOuter, TInner, Task<TResult>>>), default(IEqualityComparer<TKey>))), outer.Expression, GetSourceExpression(inner), outerKeySelector, innerKeySelector, resultSelector, Expression.Constant(comparer, typeof(IEqualityComparer<TKey>))));
#else
            return outer.Provider.CreateQuery<TResult>(Expression.Call(((MethodInfo)MethodBase.GetCurrentMethod()).MakeGenericMethod(typeof(TOuter), typeof(TInner), typeof(TKey), typeof(TResult)), outer.Expression, GetSourceExpression(inner), outerKeySelector, innerKeySelector, resultSelector, Expression.Constant(comparer, typeof(IEqualityComparer<TKey>))));
#endif
        }

        public static IAsyncQueryable<TResult> Join<TOuter, TInner, TKey, TResult>(this IAsyncQueryable<TOuter> outer, IAsyncEnumerable<TInner> inner, Expression<Func<TOuter, TKey>> outerKeySelector, Expression<Func<TInner, TKey>> innerKeySelector, Expression<Func<TOuter, TInner, TResult>> resultSelector, IEqualityComparer<TKey> comparer)
        {
            if (outer == null)
                throw new ArgumentNullException(nameof(outer));
            if (inner == null)
                throw new ArgumentNullException(nameof(inner));
            if (outerKeySelector == null)
                throw new ArgumentNullException(nameof(outerKeySelector));
            if (innerKeySelector == null)
                throw new ArgumentNullException(nameof(innerKeySelector));
            if (resultSelector == null)
                throw new ArgumentNullException(nameof(resultSelector));
            if (comparer == null)
                throw new ArgumentNullException(nameof(comparer));

#if CRIPPLED_REFLECTION
            return outer.Provider.CreateQuery<TResult>(Expression.Call(InfoOf(() => AsyncQueryable.Join<TOuter, TInner, TKey, TResult>(default(IAsyncQueryable<TOuter>), default(IAsyncEnumerable<TInner>), default(Expression<Func<TOuter, TKey>>), default(Expression<Func<TInner, TKey>>), default(Expression<Func<TOuter, TInner, TResult>>), default(IEqualityComparer<TKey>))), outer.Expression, GetSourceExpression(inner), outerKeySelector, innerKeySelector, resultSelector, Expression.Constant(comparer, typeof(IEqualityComparer<TKey>))));
#else
            return outer.Provider.CreateQuery<TResult>(Expression.Call(((MethodInfo)MethodBase.GetCurrentMethod()).MakeGenericMethod(typeof(TOuter), typeof(TInner), typeof(TKey), typeof(TResult)), outer.Expression, GetSourceExpression(inner), outerKeySelector, innerKeySelector, resultSelector, Expression.Constant(comparer, typeof(IEqualityComparer<TKey>))));
#endif
        }

        public static Task<TSource> Last<TSource>(this IAsyncQueryable<TSource> source)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));

#if CRIPPLED_REFLECTION
            return source.Provider.ExecuteAsync<TSource>(Expression.Call(InfoOf(() => AsyncQueryable.Last<TSource>(default(IAsyncQueryable<TSource>))), source.Expression), CancellationToken.None);
#else
            return source.Provider.ExecuteAsync<TSource>(Expression.Call(((MethodInfo)MethodBase.GetCurrentMethod()).MakeGenericMethod(typeof(TSource)), source.Expression), CancellationToken.None);
#endif
        }

        public static Task<TSource> Last<TSource>(this IAsyncQueryable<TSource> source, CancellationToken cancellationToken)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));

#if CRIPPLED_REFLECTION
            return source.Provider.ExecuteAsync<TSource>(Expression.Call(InfoOf(() => AsyncQueryable.Last<TSource>(default(IAsyncQueryable<TSource>), default(CancellationToken))), source.Expression, Expression.Constant(cancellationToken, typeof(CancellationToken))), cancellationToken);
#else
            return source.Provider.ExecuteAsync<TSource>(Expression.Call(((MethodInfo)MethodBase.GetCurrentMethod()).MakeGenericMethod(typeof(TSource)), source.Expression, Expression.Constant(cancellationToken, typeof(CancellationToken))), cancellationToken);
#endif
        }

        public static Task<TSource> Last<TSource>(this IAsyncQueryable<TSource> source, Expression<Func<TSource, bool>> predicate)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));
            if (predicate == null)
                throw new ArgumentNullException(nameof(predicate));

#if CRIPPLED_REFLECTION
            return source.Provider.ExecuteAsync<TSource>(Expression.Call(InfoOf(() => AsyncQueryable.Last<TSource>(default(IAsyncQueryable<TSource>), default(Expression<Func<TSource, bool>>))), source.Expression, predicate), CancellationToken.None);
#else
            return source.Provider.ExecuteAsync<TSource>(Expression.Call(((MethodInfo)MethodBase.GetCurrentMethod()).MakeGenericMethod(typeof(TSource)), source.Expression, predicate), CancellationToken.None);
#endif
        }

        public static Task<TSource> Last<TSource>(this IAsyncQueryable<TSource> source, Expression<Func<TSource, Task<bool>>> predicate)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));
            if (predicate == null)
                throw new ArgumentNullException(nameof(predicate));

#if CRIPPLED_REFLECTION
            return source.Provider.ExecuteAsync<TSource>(Expression.Call(InfoOf(() => AsyncQueryable.Last<TSource>(default(IAsyncQueryable<TSource>), default(Expression<Func<TSource, Task<bool>>>))), source.Expression, predicate), CancellationToken.None);
#else
            return source.Provider.ExecuteAsync<TSource>(Expression.Call(((MethodInfo)MethodBase.GetCurrentMethod()).MakeGenericMethod(typeof(TSource)), source.Expression, predicate), CancellationToken.None);
#endif
        }

        public static Task<TSource> Last<TSource>(this IAsyncQueryable<TSource> source, Expression<Func<TSource, bool>> predicate, CancellationToken cancellationToken)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));
            if (predicate == null)
                throw new ArgumentNullException(nameof(predicate));

#if CRIPPLED_REFLECTION
            return source.Provider.ExecuteAsync<TSource>(Expression.Call(InfoOf(() => AsyncQueryable.Last<TSource>(default(IAsyncQueryable<TSource>), default(Expression<Func<TSource, bool>>), default(CancellationToken))), source.Expression, predicate, Expression.Constant(cancellationToken, typeof(CancellationToken))), cancellationToken);
#else
            return source.Provider.ExecuteAsync<TSource>(Expression.Call(((MethodInfo)MethodBase.GetCurrentMethod()).MakeGenericMethod(typeof(TSource)), source.Expression, predicate, Expression.Constant(cancellationToken, typeof(CancellationToken))), cancellationToken);
#endif
        }

        public static Task<TSource> Last<TSource>(this IAsyncQueryable<TSource> source, Expression<Func<TSource, Task<bool>>> predicate, CancellationToken cancellationToken)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));
            if (predicate == null)
                throw new ArgumentNullException(nameof(predicate));

#if CRIPPLED_REFLECTION
            return source.Provider.ExecuteAsync<TSource>(Expression.Call(InfoOf(() => AsyncQueryable.Last<TSource>(default(IAsyncQueryable<TSource>), default(Expression<Func<TSource, Task<bool>>>), default(CancellationToken))), source.Expression, predicate, Expression.Constant(cancellationToken, typeof(CancellationToken))), cancellationToken);
#else
            return source.Provider.ExecuteAsync<TSource>(Expression.Call(((MethodInfo)MethodBase.GetCurrentMethod()).MakeGenericMethod(typeof(TSource)), source.Expression, predicate, Expression.Constant(cancellationToken, typeof(CancellationToken))), cancellationToken);
#endif
        }

        public static Task<TSource> LastOrDefault<TSource>(this IAsyncQueryable<TSource> source)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));

#if CRIPPLED_REFLECTION
            return source.Provider.ExecuteAsync<TSource>(Expression.Call(InfoOf(() => AsyncQueryable.LastOrDefault<TSource>(default(IAsyncQueryable<TSource>))), source.Expression), CancellationToken.None);
#else
            return source.Provider.ExecuteAsync<TSource>(Expression.Call(((MethodInfo)MethodBase.GetCurrentMethod()).MakeGenericMethod(typeof(TSource)), source.Expression), CancellationToken.None);
#endif
        }

        public static Task<TSource> LastOrDefault<TSource>(this IAsyncQueryable<TSource> source, CancellationToken cancellationToken)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));

#if CRIPPLED_REFLECTION
            return source.Provider.ExecuteAsync<TSource>(Expression.Call(InfoOf(() => AsyncQueryable.LastOrDefault<TSource>(default(IAsyncQueryable<TSource>), default(CancellationToken))), source.Expression, Expression.Constant(cancellationToken, typeof(CancellationToken))), cancellationToken);
#else
            return source.Provider.ExecuteAsync<TSource>(Expression.Call(((MethodInfo)MethodBase.GetCurrentMethod()).MakeGenericMethod(typeof(TSource)), source.Expression, Expression.Constant(cancellationToken, typeof(CancellationToken))), cancellationToken);
#endif
        }

        public static Task<TSource> LastOrDefault<TSource>(this IAsyncQueryable<TSource> source, Expression<Func<TSource, bool>> predicate)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));
            if (predicate == null)
                throw new ArgumentNullException(nameof(predicate));

#if CRIPPLED_REFLECTION
            return source.Provider.ExecuteAsync<TSource>(Expression.Call(InfoOf(() => AsyncQueryable.LastOrDefault<TSource>(default(IAsyncQueryable<TSource>), default(Expression<Func<TSource, bool>>))), source.Expression, predicate), CancellationToken.None);
#else
            return source.Provider.ExecuteAsync<TSource>(Expression.Call(((MethodInfo)MethodBase.GetCurrentMethod()).MakeGenericMethod(typeof(TSource)), source.Expression, predicate), CancellationToken.None);
#endif
        }

        public static Task<TSource> LastOrDefault<TSource>(this IAsyncQueryable<TSource> source, Expression<Func<TSource, Task<bool>>> predicate)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));
            if (predicate == null)
                throw new ArgumentNullException(nameof(predicate));

#if CRIPPLED_REFLECTION
            return source.Provider.ExecuteAsync<TSource>(Expression.Call(InfoOf(() => AsyncQueryable.LastOrDefault<TSource>(default(IAsyncQueryable<TSource>), default(Expression<Func<TSource, Task<bool>>>))), source.Expression, predicate), CancellationToken.None);
#else
            return source.Provider.ExecuteAsync<TSource>(Expression.Call(((MethodInfo)MethodBase.GetCurrentMethod()).MakeGenericMethod(typeof(TSource)), source.Expression, predicate), CancellationToken.None);
#endif
        }

        public static Task<TSource> LastOrDefault<TSource>(this IAsyncQueryable<TSource> source, Expression<Func<TSource, bool>> predicate, CancellationToken cancellationToken)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));
            if (predicate == null)
                throw new ArgumentNullException(nameof(predicate));

#if CRIPPLED_REFLECTION
            return source.Provider.ExecuteAsync<TSource>(Expression.Call(InfoOf(() => AsyncQueryable.LastOrDefault<TSource>(default(IAsyncQueryable<TSource>), default(Expression<Func<TSource, bool>>), default(CancellationToken))), source.Expression, predicate, Expression.Constant(cancellationToken, typeof(CancellationToken))), cancellationToken);
#else
            return source.Provider.ExecuteAsync<TSource>(Expression.Call(((MethodInfo)MethodBase.GetCurrentMethod()).MakeGenericMethod(typeof(TSource)), source.Expression, predicate, Expression.Constant(cancellationToken, typeof(CancellationToken))), cancellationToken);
#endif
        }

        public static Task<TSource> LastOrDefault<TSource>(this IAsyncQueryable<TSource> source, Expression<Func<TSource, Task<bool>>> predicate, CancellationToken cancellationToken)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));
            if (predicate == null)
                throw new ArgumentNullException(nameof(predicate));

#if CRIPPLED_REFLECTION
            return source.Provider.ExecuteAsync<TSource>(Expression.Call(InfoOf(() => AsyncQueryable.LastOrDefault<TSource>(default(IAsyncQueryable<TSource>), default(Expression<Func<TSource, Task<bool>>>), default(CancellationToken))), source.Expression, predicate, Expression.Constant(cancellationToken, typeof(CancellationToken))), cancellationToken);
#else
            return source.Provider.ExecuteAsync<TSource>(Expression.Call(((MethodInfo)MethodBase.GetCurrentMethod()).MakeGenericMethod(typeof(TSource)), source.Expression, predicate, Expression.Constant(cancellationToken, typeof(CancellationToken))), cancellationToken);
#endif
        }

        public static Task<long> LongCount<TSource>(this IAsyncQueryable<TSource> source)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));

#if CRIPPLED_REFLECTION
            return source.Provider.ExecuteAsync<long>(Expression.Call(InfoOf(() => AsyncQueryable.LongCount<TSource>(default(IAsyncQueryable<TSource>))), source.Expression), CancellationToken.None);
#else
            return source.Provider.ExecuteAsync<long>(Expression.Call(((MethodInfo)MethodBase.GetCurrentMethod()).MakeGenericMethod(typeof(TSource)), source.Expression), CancellationToken.None);
#endif
        }

        public static Task<long> LongCount<TSource>(this IAsyncQueryable<TSource> source, CancellationToken cancellationToken)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));

#if CRIPPLED_REFLECTION
            return source.Provider.ExecuteAsync<long>(Expression.Call(InfoOf(() => AsyncQueryable.LongCount<TSource>(default(IAsyncQueryable<TSource>), default(CancellationToken))), source.Expression, Expression.Constant(cancellationToken, typeof(CancellationToken))), cancellationToken);
#else
            return source.Provider.ExecuteAsync<long>(Expression.Call(((MethodInfo)MethodBase.GetCurrentMethod()).MakeGenericMethod(typeof(TSource)), source.Expression, Expression.Constant(cancellationToken, typeof(CancellationToken))), cancellationToken);
#endif
        }

        public static Task<long> LongCount<TSource>(this IAsyncQueryable<TSource> source, Expression<Func<TSource, bool>> predicate)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));
            if (predicate == null)
                throw new ArgumentNullException(nameof(predicate));

#if CRIPPLED_REFLECTION
            return source.Provider.ExecuteAsync<long>(Expression.Call(InfoOf(() => AsyncQueryable.LongCount<TSource>(default(IAsyncQueryable<TSource>), default(Expression<Func<TSource, bool>>))), source.Expression, predicate), CancellationToken.None);
#else
            return source.Provider.ExecuteAsync<long>(Expression.Call(((MethodInfo)MethodBase.GetCurrentMethod()).MakeGenericMethod(typeof(TSource)), source.Expression, predicate), CancellationToken.None);
#endif
        }

        public static Task<long> LongCount<TSource>(this IAsyncQueryable<TSource> source, Expression<Func<TSource, Task<bool>>> predicate)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));
            if (predicate == null)
                throw new ArgumentNullException(nameof(predicate));

#if CRIPPLED_REFLECTION
            return source.Provider.ExecuteAsync<long>(Expression.Call(InfoOf(() => AsyncQueryable.LongCount<TSource>(default(IAsyncQueryable<TSource>), default(Expression<Func<TSource, Task<bool>>>))), source.Expression, predicate), CancellationToken.None);
#else
            return source.Provider.ExecuteAsync<long>(Expression.Call(((MethodInfo)MethodBase.GetCurrentMethod()).MakeGenericMethod(typeof(TSource)), source.Expression, predicate), CancellationToken.None);
#endif
        }

        public static Task<long> LongCount<TSource>(this IAsyncQueryable<TSource> source, Expression<Func<TSource, bool>> predicate, CancellationToken cancellationToken)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));
            if (predicate == null)
                throw new ArgumentNullException(nameof(predicate));

#if CRIPPLED_REFLECTION
            return source.Provider.ExecuteAsync<long>(Expression.Call(InfoOf(() => AsyncQueryable.LongCount<TSource>(default(IAsyncQueryable<TSource>), default(Expression<Func<TSource, bool>>), default(CancellationToken))), source.Expression, predicate, Expression.Constant(cancellationToken, typeof(CancellationToken))), cancellationToken);
#else
            return source.Provider.ExecuteAsync<long>(Expression.Call(((MethodInfo)MethodBase.GetCurrentMethod()).MakeGenericMethod(typeof(TSource)), source.Expression, predicate, Expression.Constant(cancellationToken, typeof(CancellationToken))), cancellationToken);
#endif
        }

        public static Task<long> LongCount<TSource>(this IAsyncQueryable<TSource> source, Expression<Func<TSource, Task<bool>>> predicate, CancellationToken cancellationToken)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));
            if (predicate == null)
                throw new ArgumentNullException(nameof(predicate));

#if CRIPPLED_REFLECTION
            return source.Provider.ExecuteAsync<long>(Expression.Call(InfoOf(() => AsyncQueryable.LongCount<TSource>(default(IAsyncQueryable<TSource>), default(Expression<Func<TSource, Task<bool>>>), default(CancellationToken))), source.Expression, predicate, Expression.Constant(cancellationToken, typeof(CancellationToken))), cancellationToken);
#else
            return source.Provider.ExecuteAsync<long>(Expression.Call(((MethodInfo)MethodBase.GetCurrentMethod()).MakeGenericMethod(typeof(TSource)), source.Expression, predicate, Expression.Constant(cancellationToken, typeof(CancellationToken))), cancellationToken);
#endif
        }

        public static Task<decimal?> Max(this IAsyncQueryable<decimal?> source)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));

#if CRIPPLED_REFLECTION
            return source.Provider.ExecuteAsync<decimal?>(Expression.Call(InfoOf(() => AsyncQueryable.Max(default(IAsyncQueryable<decimal?>))), source.Expression), CancellationToken.None);
#else
            return source.Provider.ExecuteAsync<decimal?>(Expression.Call((MethodInfo)MethodBase.GetCurrentMethod(), source.Expression), CancellationToken.None);
#endif
        }

        public static Task<decimal> Max(this IAsyncQueryable<decimal> source)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));

#if CRIPPLED_REFLECTION
            return source.Provider.ExecuteAsync<decimal>(Expression.Call(InfoOf(() => AsyncQueryable.Max(default(IAsyncQueryable<decimal>))), source.Expression), CancellationToken.None);
#else
            return source.Provider.ExecuteAsync<decimal>(Expression.Call((MethodInfo)MethodBase.GetCurrentMethod(), source.Expression), CancellationToken.None);
#endif
        }

        public static Task<double?> Max(this IAsyncQueryable<double?> source)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));

#if CRIPPLED_REFLECTION
            return source.Provider.ExecuteAsync<double?>(Expression.Call(InfoOf(() => AsyncQueryable.Max(default(IAsyncQueryable<double?>))), source.Expression), CancellationToken.None);
#else
            return source.Provider.ExecuteAsync<double?>(Expression.Call((MethodInfo)MethodBase.GetCurrentMethod(), source.Expression), CancellationToken.None);
#endif
        }

        public static Task<double> Max(this IAsyncQueryable<double> source)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));

#if CRIPPLED_REFLECTION
            return source.Provider.ExecuteAsync<double>(Expression.Call(InfoOf(() => AsyncQueryable.Max(default(IAsyncQueryable<double>))), source.Expression), CancellationToken.None);
#else
            return source.Provider.ExecuteAsync<double>(Expression.Call((MethodInfo)MethodBase.GetCurrentMethod(), source.Expression), CancellationToken.None);
#endif
        }

        public static Task<float?> Max(this IAsyncQueryable<float?> source)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));

#if CRIPPLED_REFLECTION
            return source.Provider.ExecuteAsync<float?>(Expression.Call(InfoOf(() => AsyncQueryable.Max(default(IAsyncQueryable<float?>))), source.Expression), CancellationToken.None);
#else
            return source.Provider.ExecuteAsync<float?>(Expression.Call((MethodInfo)MethodBase.GetCurrentMethod(), source.Expression), CancellationToken.None);
#endif
        }

        public static Task<float> Max(this IAsyncQueryable<float> source)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));

#if CRIPPLED_REFLECTION
            return source.Provider.ExecuteAsync<float>(Expression.Call(InfoOf(() => AsyncQueryable.Max(default(IAsyncQueryable<float>))), source.Expression), CancellationToken.None);
#else
            return source.Provider.ExecuteAsync<float>(Expression.Call((MethodInfo)MethodBase.GetCurrentMethod(), source.Expression), CancellationToken.None);
#endif
        }

        public static Task<int?> Max(this IAsyncQueryable<int?> source)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));

#if CRIPPLED_REFLECTION
            return source.Provider.ExecuteAsync<int?>(Expression.Call(InfoOf(() => AsyncQueryable.Max(default(IAsyncQueryable<int?>))), source.Expression), CancellationToken.None);
#else
            return source.Provider.ExecuteAsync<int?>(Expression.Call((MethodInfo)MethodBase.GetCurrentMethod(), source.Expression), CancellationToken.None);
#endif
        }

        public static Task<int> Max(this IAsyncQueryable<int> source)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));

#if CRIPPLED_REFLECTION
            return source.Provider.ExecuteAsync<int>(Expression.Call(InfoOf(() => AsyncQueryable.Max(default(IAsyncQueryable<int>))), source.Expression), CancellationToken.None);
#else
            return source.Provider.ExecuteAsync<int>(Expression.Call((MethodInfo)MethodBase.GetCurrentMethod(), source.Expression), CancellationToken.None);
#endif
        }

        public static Task<long?> Max(this IAsyncQueryable<long?> source)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));

#if CRIPPLED_REFLECTION
            return source.Provider.ExecuteAsync<long?>(Expression.Call(InfoOf(() => AsyncQueryable.Max(default(IAsyncQueryable<long?>))), source.Expression), CancellationToken.None);
#else
            return source.Provider.ExecuteAsync<long?>(Expression.Call((MethodInfo)MethodBase.GetCurrentMethod(), source.Expression), CancellationToken.None);
#endif
        }

        public static Task<long> Max(this IAsyncQueryable<long> source)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));

#if CRIPPLED_REFLECTION
            return source.Provider.ExecuteAsync<long>(Expression.Call(InfoOf(() => AsyncQueryable.Max(default(IAsyncQueryable<long>))), source.Expression), CancellationToken.None);
#else
            return source.Provider.ExecuteAsync<long>(Expression.Call((MethodInfo)MethodBase.GetCurrentMethod(), source.Expression), CancellationToken.None);
#endif
        }

        public static Task<decimal?> Max(this IAsyncQueryable<decimal?> source, CancellationToken cancellationToken)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));

#if CRIPPLED_REFLECTION
            return source.Provider.ExecuteAsync<decimal?>(Expression.Call(InfoOf(() => AsyncQueryable.Max(default(IAsyncQueryable<decimal?>), default(CancellationToken))), source.Expression, Expression.Constant(cancellationToken, typeof(CancellationToken))), cancellationToken);
#else
            return source.Provider.ExecuteAsync<decimal?>(Expression.Call((MethodInfo)MethodBase.GetCurrentMethod(), source.Expression, Expression.Constant(cancellationToken, typeof(CancellationToken))), cancellationToken);
#endif
        }

        public static Task<decimal> Max(this IAsyncQueryable<decimal> source, CancellationToken cancellationToken)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));

#if CRIPPLED_REFLECTION
            return source.Provider.ExecuteAsync<decimal>(Expression.Call(InfoOf(() => AsyncQueryable.Max(default(IAsyncQueryable<decimal>), default(CancellationToken))), source.Expression, Expression.Constant(cancellationToken, typeof(CancellationToken))), cancellationToken);
#else
            return source.Provider.ExecuteAsync<decimal>(Expression.Call((MethodInfo)MethodBase.GetCurrentMethod(), source.Expression, Expression.Constant(cancellationToken, typeof(CancellationToken))), cancellationToken);
#endif
        }

        public static Task<double?> Max(this IAsyncQueryable<double?> source, CancellationToken cancellationToken)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));

#if CRIPPLED_REFLECTION
            return source.Provider.ExecuteAsync<double?>(Expression.Call(InfoOf(() => AsyncQueryable.Max(default(IAsyncQueryable<double?>), default(CancellationToken))), source.Expression, Expression.Constant(cancellationToken, typeof(CancellationToken))), cancellationToken);
#else
            return source.Provider.ExecuteAsync<double?>(Expression.Call((MethodInfo)MethodBase.GetCurrentMethod(), source.Expression, Expression.Constant(cancellationToken, typeof(CancellationToken))), cancellationToken);
#endif
        }

        public static Task<double> Max(this IAsyncQueryable<double> source, CancellationToken cancellationToken)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));

#if CRIPPLED_REFLECTION
            return source.Provider.ExecuteAsync<double>(Expression.Call(InfoOf(() => AsyncQueryable.Max(default(IAsyncQueryable<double>), default(CancellationToken))), source.Expression, Expression.Constant(cancellationToken, typeof(CancellationToken))), cancellationToken);
#else
            return source.Provider.ExecuteAsync<double>(Expression.Call((MethodInfo)MethodBase.GetCurrentMethod(), source.Expression, Expression.Constant(cancellationToken, typeof(CancellationToken))), cancellationToken);
#endif
        }

        public static Task<float?> Max(this IAsyncQueryable<float?> source, CancellationToken cancellationToken)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));

#if CRIPPLED_REFLECTION
            return source.Provider.ExecuteAsync<float?>(Expression.Call(InfoOf(() => AsyncQueryable.Max(default(IAsyncQueryable<float?>), default(CancellationToken))), source.Expression, Expression.Constant(cancellationToken, typeof(CancellationToken))), cancellationToken);
#else
            return source.Provider.ExecuteAsync<float?>(Expression.Call((MethodInfo)MethodBase.GetCurrentMethod(), source.Expression, Expression.Constant(cancellationToken, typeof(CancellationToken))), cancellationToken);
#endif
        }

        public static Task<float> Max(this IAsyncQueryable<float> source, CancellationToken cancellationToken)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));

#if CRIPPLED_REFLECTION
            return source.Provider.ExecuteAsync<float>(Expression.Call(InfoOf(() => AsyncQueryable.Max(default(IAsyncQueryable<float>), default(CancellationToken))), source.Expression, Expression.Constant(cancellationToken, typeof(CancellationToken))), cancellationToken);
#else
            return source.Provider.ExecuteAsync<float>(Expression.Call((MethodInfo)MethodBase.GetCurrentMethod(), source.Expression, Expression.Constant(cancellationToken, typeof(CancellationToken))), cancellationToken);
#endif
        }

        public static Task<int?> Max(this IAsyncQueryable<int?> source, CancellationToken cancellationToken)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));

#if CRIPPLED_REFLECTION
            return source.Provider.ExecuteAsync<int?>(Expression.Call(InfoOf(() => AsyncQueryable.Max(default(IAsyncQueryable<int?>), default(CancellationToken))), source.Expression, Expression.Constant(cancellationToken, typeof(CancellationToken))), cancellationToken);
#else
            return source.Provider.ExecuteAsync<int?>(Expression.Call((MethodInfo)MethodBase.GetCurrentMethod(), source.Expression, Expression.Constant(cancellationToken, typeof(CancellationToken))), cancellationToken);
#endif
        }

        public static Task<int> Max(this IAsyncQueryable<int> source, CancellationToken cancellationToken)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));

#if CRIPPLED_REFLECTION
            return source.Provider.ExecuteAsync<int>(Expression.Call(InfoOf(() => AsyncQueryable.Max(default(IAsyncQueryable<int>), default(CancellationToken))), source.Expression, Expression.Constant(cancellationToken, typeof(CancellationToken))), cancellationToken);
#else
            return source.Provider.ExecuteAsync<int>(Expression.Call((MethodInfo)MethodBase.GetCurrentMethod(), source.Expression, Expression.Constant(cancellationToken, typeof(CancellationToken))), cancellationToken);
#endif
        }

        public static Task<long?> Max(this IAsyncQueryable<long?> source, CancellationToken cancellationToken)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));

#if CRIPPLED_REFLECTION
            return source.Provider.ExecuteAsync<long?>(Expression.Call(InfoOf(() => AsyncQueryable.Max(default(IAsyncQueryable<long?>), default(CancellationToken))), source.Expression, Expression.Constant(cancellationToken, typeof(CancellationToken))), cancellationToken);
#else
            return source.Provider.ExecuteAsync<long?>(Expression.Call((MethodInfo)MethodBase.GetCurrentMethod(), source.Expression, Expression.Constant(cancellationToken, typeof(CancellationToken))), cancellationToken);
#endif
        }

        public static Task<long> Max(this IAsyncQueryable<long> source, CancellationToken cancellationToken)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));

#if CRIPPLED_REFLECTION
            return source.Provider.ExecuteAsync<long>(Expression.Call(InfoOf(() => AsyncQueryable.Max(default(IAsyncQueryable<long>), default(CancellationToken))), source.Expression, Expression.Constant(cancellationToken, typeof(CancellationToken))), cancellationToken);
#else
            return source.Provider.ExecuteAsync<long>(Expression.Call((MethodInfo)MethodBase.GetCurrentMethod(), source.Expression, Expression.Constant(cancellationToken, typeof(CancellationToken))), cancellationToken);
#endif
        }

        public static Task<TSource> Max<TSource>(this IAsyncQueryable<TSource> source)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));

#if CRIPPLED_REFLECTION
            return source.Provider.ExecuteAsync<TSource>(Expression.Call(InfoOf(() => AsyncQueryable.Max<TSource>(default(IAsyncQueryable<TSource>))), source.Expression), CancellationToken.None);
#else
            return source.Provider.ExecuteAsync<TSource>(Expression.Call(((MethodInfo)MethodBase.GetCurrentMethod()).MakeGenericMethod(typeof(TSource)), source.Expression), CancellationToken.None);
#endif
        }

        public static Task<TSource> Max<TSource>(this IAsyncQueryable<TSource> source, CancellationToken cancellationToken)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));

#if CRIPPLED_REFLECTION
            return source.Provider.ExecuteAsync<TSource>(Expression.Call(InfoOf(() => AsyncQueryable.Max<TSource>(default(IAsyncQueryable<TSource>), default(CancellationToken))), source.Expression, Expression.Constant(cancellationToken, typeof(CancellationToken))), cancellationToken);
#else
            return source.Provider.ExecuteAsync<TSource>(Expression.Call(((MethodInfo)MethodBase.GetCurrentMethod()).MakeGenericMethod(typeof(TSource)), source.Expression, Expression.Constant(cancellationToken, typeof(CancellationToken))), cancellationToken);
#endif
        }

        public static Task<decimal?> Max<TSource>(this IAsyncQueryable<TSource> source, Expression<Func<TSource, decimal?>> selector)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));
            if (selector == null)
                throw new ArgumentNullException(nameof(selector));

#if CRIPPLED_REFLECTION
            return source.Provider.ExecuteAsync<decimal?>(Expression.Call(InfoOf(() => AsyncQueryable.Max<TSource>(default(IAsyncQueryable<TSource>), default(Expression<Func<TSource, decimal?>>))), source.Expression, selector), CancellationToken.None);
#else
            return source.Provider.ExecuteAsync<decimal?>(Expression.Call(((MethodInfo)MethodBase.GetCurrentMethod()).MakeGenericMethod(typeof(TSource)), source.Expression, selector), CancellationToken.None);
#endif
        }

        public static Task<decimal> Max<TSource>(this IAsyncQueryable<TSource> source, Expression<Func<TSource, decimal>> selector)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));
            if (selector == null)
                throw new ArgumentNullException(nameof(selector));

#if CRIPPLED_REFLECTION
            return source.Provider.ExecuteAsync<decimal>(Expression.Call(InfoOf(() => AsyncQueryable.Max<TSource>(default(IAsyncQueryable<TSource>), default(Expression<Func<TSource, decimal>>))), source.Expression, selector), CancellationToken.None);
#else
            return source.Provider.ExecuteAsync<decimal>(Expression.Call(((MethodInfo)MethodBase.GetCurrentMethod()).MakeGenericMethod(typeof(TSource)), source.Expression, selector), CancellationToken.None);
#endif
        }

        public static Task<double?> Max<TSource>(this IAsyncQueryable<TSource> source, Expression<Func<TSource, double?>> selector)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));
            if (selector == null)
                throw new ArgumentNullException(nameof(selector));

#if CRIPPLED_REFLECTION
            return source.Provider.ExecuteAsync<double?>(Expression.Call(InfoOf(() => AsyncQueryable.Max<TSource>(default(IAsyncQueryable<TSource>), default(Expression<Func<TSource, double?>>))), source.Expression, selector), CancellationToken.None);
#else
            return source.Provider.ExecuteAsync<double?>(Expression.Call(((MethodInfo)MethodBase.GetCurrentMethod()).MakeGenericMethod(typeof(TSource)), source.Expression, selector), CancellationToken.None);
#endif
        }

        public static Task<double> Max<TSource>(this IAsyncQueryable<TSource> source, Expression<Func<TSource, double>> selector)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));
            if (selector == null)
                throw new ArgumentNullException(nameof(selector));

#if CRIPPLED_REFLECTION
            return source.Provider.ExecuteAsync<double>(Expression.Call(InfoOf(() => AsyncQueryable.Max<TSource>(default(IAsyncQueryable<TSource>), default(Expression<Func<TSource, double>>))), source.Expression, selector), CancellationToken.None);
#else
            return source.Provider.ExecuteAsync<double>(Expression.Call(((MethodInfo)MethodBase.GetCurrentMethod()).MakeGenericMethod(typeof(TSource)), source.Expression, selector), CancellationToken.None);
#endif
        }

        public static Task<float?> Max<TSource>(this IAsyncQueryable<TSource> source, Expression<Func<TSource, float?>> selector)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));
            if (selector == null)
                throw new ArgumentNullException(nameof(selector));

#if CRIPPLED_REFLECTION
            return source.Provider.ExecuteAsync<float?>(Expression.Call(InfoOf(() => AsyncQueryable.Max<TSource>(default(IAsyncQueryable<TSource>), default(Expression<Func<TSource, float?>>))), source.Expression, selector), CancellationToken.None);
#else
            return source.Provider.ExecuteAsync<float?>(Expression.Call(((MethodInfo)MethodBase.GetCurrentMethod()).MakeGenericMethod(typeof(TSource)), source.Expression, selector), CancellationToken.None);
#endif
        }

        public static Task<float> Max<TSource>(this IAsyncQueryable<TSource> source, Expression<Func<TSource, float>> selector)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));
            if (selector == null)
                throw new ArgumentNullException(nameof(selector));

#if CRIPPLED_REFLECTION
            return source.Provider.ExecuteAsync<float>(Expression.Call(InfoOf(() => AsyncQueryable.Max<TSource>(default(IAsyncQueryable<TSource>), default(Expression<Func<TSource, float>>))), source.Expression, selector), CancellationToken.None);
#else
            return source.Provider.ExecuteAsync<float>(Expression.Call(((MethodInfo)MethodBase.GetCurrentMethod()).MakeGenericMethod(typeof(TSource)), source.Expression, selector), CancellationToken.None);
#endif
        }

        public static Task<int?> Max<TSource>(this IAsyncQueryable<TSource> source, Expression<Func<TSource, int?>> selector)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));
            if (selector == null)
                throw new ArgumentNullException(nameof(selector));

#if CRIPPLED_REFLECTION
            return source.Provider.ExecuteAsync<int?>(Expression.Call(InfoOf(() => AsyncQueryable.Max<TSource>(default(IAsyncQueryable<TSource>), default(Expression<Func<TSource, int?>>))), source.Expression, selector), CancellationToken.None);
#else
            return source.Provider.ExecuteAsync<int?>(Expression.Call(((MethodInfo)MethodBase.GetCurrentMethod()).MakeGenericMethod(typeof(TSource)), source.Expression, selector), CancellationToken.None);
#endif
        }

        public static Task<int> Max<TSource>(this IAsyncQueryable<TSource> source, Expression<Func<TSource, int>> selector)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));
            if (selector == null)
                throw new ArgumentNullException(nameof(selector));

#if CRIPPLED_REFLECTION
            return source.Provider.ExecuteAsync<int>(Expression.Call(InfoOf(() => AsyncQueryable.Max<TSource>(default(IAsyncQueryable<TSource>), default(Expression<Func<TSource, int>>))), source.Expression, selector), CancellationToken.None);
#else
            return source.Provider.ExecuteAsync<int>(Expression.Call(((MethodInfo)MethodBase.GetCurrentMethod()).MakeGenericMethod(typeof(TSource)), source.Expression, selector), CancellationToken.None);
#endif
        }

        public static Task<long?> Max<TSource>(this IAsyncQueryable<TSource> source, Expression<Func<TSource, long?>> selector)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));
            if (selector == null)
                throw new ArgumentNullException(nameof(selector));

#if CRIPPLED_REFLECTION
            return source.Provider.ExecuteAsync<long?>(Expression.Call(InfoOf(() => AsyncQueryable.Max<TSource>(default(IAsyncQueryable<TSource>), default(Expression<Func<TSource, long?>>))), source.Expression, selector), CancellationToken.None);
#else
            return source.Provider.ExecuteAsync<long?>(Expression.Call(((MethodInfo)MethodBase.GetCurrentMethod()).MakeGenericMethod(typeof(TSource)), source.Expression, selector), CancellationToken.None);
#endif
        }

        public static Task<long> Max<TSource>(this IAsyncQueryable<TSource> source, Expression<Func<TSource, long>> selector)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));
            if (selector == null)
                throw new ArgumentNullException(nameof(selector));

#if CRIPPLED_REFLECTION
            return source.Provider.ExecuteAsync<long>(Expression.Call(InfoOf(() => AsyncQueryable.Max<TSource>(default(IAsyncQueryable<TSource>), default(Expression<Func<TSource, long>>))), source.Expression, selector), CancellationToken.None);
#else
            return source.Provider.ExecuteAsync<long>(Expression.Call(((MethodInfo)MethodBase.GetCurrentMethod()).MakeGenericMethod(typeof(TSource)), source.Expression, selector), CancellationToken.None);
#endif
        }

        public static Task<decimal?> Max<TSource>(this IAsyncQueryable<TSource> source, Expression<Func<TSource, Task<decimal?>>> selector)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));
            if (selector == null)
                throw new ArgumentNullException(nameof(selector));

#if CRIPPLED_REFLECTION
            return source.Provider.ExecuteAsync<decimal?>(Expression.Call(InfoOf(() => AsyncQueryable.Max<TSource>(default(IAsyncQueryable<TSource>), default(Expression<Func<TSource, Task<decimal?>>>))), source.Expression, selector), CancellationToken.None);
#else
            return source.Provider.ExecuteAsync<decimal?>(Expression.Call(((MethodInfo)MethodBase.GetCurrentMethod()).MakeGenericMethod(typeof(TSource)), source.Expression, selector), CancellationToken.None);
#endif
        }

        public static Task<decimal> Max<TSource>(this IAsyncQueryable<TSource> source, Expression<Func<TSource, Task<decimal>>> selector)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));
            if (selector == null)
                throw new ArgumentNullException(nameof(selector));

#if CRIPPLED_REFLECTION
            return source.Provider.ExecuteAsync<decimal>(Expression.Call(InfoOf(() => AsyncQueryable.Max<TSource>(default(IAsyncQueryable<TSource>), default(Expression<Func<TSource, Task<decimal>>>))), source.Expression, selector), CancellationToken.None);
#else
            return source.Provider.ExecuteAsync<decimal>(Expression.Call(((MethodInfo)MethodBase.GetCurrentMethod()).MakeGenericMethod(typeof(TSource)), source.Expression, selector), CancellationToken.None);
#endif
        }

        public static Task<double?> Max<TSource>(this IAsyncQueryable<TSource> source, Expression<Func<TSource, Task<double?>>> selector)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));
            if (selector == null)
                throw new ArgumentNullException(nameof(selector));

#if CRIPPLED_REFLECTION
            return source.Provider.ExecuteAsync<double?>(Expression.Call(InfoOf(() => AsyncQueryable.Max<TSource>(default(IAsyncQueryable<TSource>), default(Expression<Func<TSource, Task<double?>>>))), source.Expression, selector), CancellationToken.None);
#else
            return source.Provider.ExecuteAsync<double?>(Expression.Call(((MethodInfo)MethodBase.GetCurrentMethod()).MakeGenericMethod(typeof(TSource)), source.Expression, selector), CancellationToken.None);
#endif
        }

        public static Task<double> Max<TSource>(this IAsyncQueryable<TSource> source, Expression<Func<TSource, Task<double>>> selector)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));
            if (selector == null)
                throw new ArgumentNullException(nameof(selector));

#if CRIPPLED_REFLECTION
            return source.Provider.ExecuteAsync<double>(Expression.Call(InfoOf(() => AsyncQueryable.Max<TSource>(default(IAsyncQueryable<TSource>), default(Expression<Func<TSource, Task<double>>>))), source.Expression, selector), CancellationToken.None);
#else
            return source.Provider.ExecuteAsync<double>(Expression.Call(((MethodInfo)MethodBase.GetCurrentMethod()).MakeGenericMethod(typeof(TSource)), source.Expression, selector), CancellationToken.None);
#endif
        }

        public static Task<float?> Max<TSource>(this IAsyncQueryable<TSource> source, Expression<Func<TSource, Task<float?>>> selector)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));
            if (selector == null)
                throw new ArgumentNullException(nameof(selector));

#if CRIPPLED_REFLECTION
            return source.Provider.ExecuteAsync<float?>(Expression.Call(InfoOf(() => AsyncQueryable.Max<TSource>(default(IAsyncQueryable<TSource>), default(Expression<Func<TSource, Task<float?>>>))), source.Expression, selector), CancellationToken.None);
#else
            return source.Provider.ExecuteAsync<float?>(Expression.Call(((MethodInfo)MethodBase.GetCurrentMethod()).MakeGenericMethod(typeof(TSource)), source.Expression, selector), CancellationToken.None);
#endif
        }

        public static Task<float> Max<TSource>(this IAsyncQueryable<TSource> source, Expression<Func<TSource, Task<float>>> selector)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));
            if (selector == null)
                throw new ArgumentNullException(nameof(selector));

#if CRIPPLED_REFLECTION
            return source.Provider.ExecuteAsync<float>(Expression.Call(InfoOf(() => AsyncQueryable.Max<TSource>(default(IAsyncQueryable<TSource>), default(Expression<Func<TSource, Task<float>>>))), source.Expression, selector), CancellationToken.None);
#else
            return source.Provider.ExecuteAsync<float>(Expression.Call(((MethodInfo)MethodBase.GetCurrentMethod()).MakeGenericMethod(typeof(TSource)), source.Expression, selector), CancellationToken.None);
#endif
        }

        public static Task<int?> Max<TSource>(this IAsyncQueryable<TSource> source, Expression<Func<TSource, Task<int?>>> selector)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));
            if (selector == null)
                throw new ArgumentNullException(nameof(selector));

#if CRIPPLED_REFLECTION
            return source.Provider.ExecuteAsync<int?>(Expression.Call(InfoOf(() => AsyncQueryable.Max<TSource>(default(IAsyncQueryable<TSource>), default(Expression<Func<TSource, Task<int?>>>))), source.Expression, selector), CancellationToken.None);
#else
            return source.Provider.ExecuteAsync<int?>(Expression.Call(((MethodInfo)MethodBase.GetCurrentMethod()).MakeGenericMethod(typeof(TSource)), source.Expression, selector), CancellationToken.None);
#endif
        }

        public static Task<int> Max<TSource>(this IAsyncQueryable<TSource> source, Expression<Func<TSource, Task<int>>> selector)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));
            if (selector == null)
                throw new ArgumentNullException(nameof(selector));

#if CRIPPLED_REFLECTION
            return source.Provider.ExecuteAsync<int>(Expression.Call(InfoOf(() => AsyncQueryable.Max<TSource>(default(IAsyncQueryable<TSource>), default(Expression<Func<TSource, Task<int>>>))), source.Expression, selector), CancellationToken.None);
#else
            return source.Provider.ExecuteAsync<int>(Expression.Call(((MethodInfo)MethodBase.GetCurrentMethod()).MakeGenericMethod(typeof(TSource)), source.Expression, selector), CancellationToken.None);
#endif
        }

        public static Task<long?> Max<TSource>(this IAsyncQueryable<TSource> source, Expression<Func<TSource, Task<long?>>> selector)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));
            if (selector == null)
                throw new ArgumentNullException(nameof(selector));

#if CRIPPLED_REFLECTION
            return source.Provider.ExecuteAsync<long?>(Expression.Call(InfoOf(() => AsyncQueryable.Max<TSource>(default(IAsyncQueryable<TSource>), default(Expression<Func<TSource, Task<long?>>>))), source.Expression, selector), CancellationToken.None);
#else
            return source.Provider.ExecuteAsync<long?>(Expression.Call(((MethodInfo)MethodBase.GetCurrentMethod()).MakeGenericMethod(typeof(TSource)), source.Expression, selector), CancellationToken.None);
#endif
        }

        public static Task<long> Max<TSource>(this IAsyncQueryable<TSource> source, Expression<Func<TSource, Task<long>>> selector)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));
            if (selector == null)
                throw new ArgumentNullException(nameof(selector));

#if CRIPPLED_REFLECTION
            return source.Provider.ExecuteAsync<long>(Expression.Call(InfoOf(() => AsyncQueryable.Max<TSource>(default(IAsyncQueryable<TSource>), default(Expression<Func<TSource, Task<long>>>))), source.Expression, selector), CancellationToken.None);
#else
            return source.Provider.ExecuteAsync<long>(Expression.Call(((MethodInfo)MethodBase.GetCurrentMethod()).MakeGenericMethod(typeof(TSource)), source.Expression, selector), CancellationToken.None);
#endif
        }

        public static Task<decimal?> Max<TSource>(this IAsyncQueryable<TSource> source, Expression<Func<TSource, decimal?>> selector, CancellationToken cancellationToken)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));
            if (selector == null)
                throw new ArgumentNullException(nameof(selector));

#if CRIPPLED_REFLECTION
            return source.Provider.ExecuteAsync<decimal?>(Expression.Call(InfoOf(() => AsyncQueryable.Max<TSource>(default(IAsyncQueryable<TSource>), default(Expression<Func<TSource, decimal?>>), default(CancellationToken))), source.Expression, selector, Expression.Constant(cancellationToken, typeof(CancellationToken))), cancellationToken);
#else
            return source.Provider.ExecuteAsync<decimal?>(Expression.Call(((MethodInfo)MethodBase.GetCurrentMethod()).MakeGenericMethod(typeof(TSource)), source.Expression, selector, Expression.Constant(cancellationToken, typeof(CancellationToken))), cancellationToken);
#endif
        }

        public static Task<decimal> Max<TSource>(this IAsyncQueryable<TSource> source, Expression<Func<TSource, decimal>> selector, CancellationToken cancellationToken)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));
            if (selector == null)
                throw new ArgumentNullException(nameof(selector));

#if CRIPPLED_REFLECTION
            return source.Provider.ExecuteAsync<decimal>(Expression.Call(InfoOf(() => AsyncQueryable.Max<TSource>(default(IAsyncQueryable<TSource>), default(Expression<Func<TSource, decimal>>), default(CancellationToken))), source.Expression, selector, Expression.Constant(cancellationToken, typeof(CancellationToken))), cancellationToken);
#else
            return source.Provider.ExecuteAsync<decimal>(Expression.Call(((MethodInfo)MethodBase.GetCurrentMethod()).MakeGenericMethod(typeof(TSource)), source.Expression, selector, Expression.Constant(cancellationToken, typeof(CancellationToken))), cancellationToken);
#endif
        }

        public static Task<double?> Max<TSource>(this IAsyncQueryable<TSource> source, Expression<Func<TSource, double?>> selector, CancellationToken cancellationToken)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));
            if (selector == null)
                throw new ArgumentNullException(nameof(selector));

#if CRIPPLED_REFLECTION
            return source.Provider.ExecuteAsync<double?>(Expression.Call(InfoOf(() => AsyncQueryable.Max<TSource>(default(IAsyncQueryable<TSource>), default(Expression<Func<TSource, double?>>), default(CancellationToken))), source.Expression, selector, Expression.Constant(cancellationToken, typeof(CancellationToken))), cancellationToken);
#else
            return source.Provider.ExecuteAsync<double?>(Expression.Call(((MethodInfo)MethodBase.GetCurrentMethod()).MakeGenericMethod(typeof(TSource)), source.Expression, selector, Expression.Constant(cancellationToken, typeof(CancellationToken))), cancellationToken);
#endif
        }

        public static Task<double> Max<TSource>(this IAsyncQueryable<TSource> source, Expression<Func<TSource, double>> selector, CancellationToken cancellationToken)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));
            if (selector == null)
                throw new ArgumentNullException(nameof(selector));

#if CRIPPLED_REFLECTION
            return source.Provider.ExecuteAsync<double>(Expression.Call(InfoOf(() => AsyncQueryable.Max<TSource>(default(IAsyncQueryable<TSource>), default(Expression<Func<TSource, double>>), default(CancellationToken))), source.Expression, selector, Expression.Constant(cancellationToken, typeof(CancellationToken))), cancellationToken);
#else
            return source.Provider.ExecuteAsync<double>(Expression.Call(((MethodInfo)MethodBase.GetCurrentMethod()).MakeGenericMethod(typeof(TSource)), source.Expression, selector, Expression.Constant(cancellationToken, typeof(CancellationToken))), cancellationToken);
#endif
        }

        public static Task<float?> Max<TSource>(this IAsyncQueryable<TSource> source, Expression<Func<TSource, float?>> selector, CancellationToken cancellationToken)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));
            if (selector == null)
                throw new ArgumentNullException(nameof(selector));

#if CRIPPLED_REFLECTION
            return source.Provider.ExecuteAsync<float?>(Expression.Call(InfoOf(() => AsyncQueryable.Max<TSource>(default(IAsyncQueryable<TSource>), default(Expression<Func<TSource, float?>>), default(CancellationToken))), source.Expression, selector, Expression.Constant(cancellationToken, typeof(CancellationToken))), cancellationToken);
#else
            return source.Provider.ExecuteAsync<float?>(Expression.Call(((MethodInfo)MethodBase.GetCurrentMethod()).MakeGenericMethod(typeof(TSource)), source.Expression, selector, Expression.Constant(cancellationToken, typeof(CancellationToken))), cancellationToken);
#endif
        }

        public static Task<float> Max<TSource>(this IAsyncQueryable<TSource> source, Expression<Func<TSource, float>> selector, CancellationToken cancellationToken)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));
            if (selector == null)
                throw new ArgumentNullException(nameof(selector));

#if CRIPPLED_REFLECTION
            return source.Provider.ExecuteAsync<float>(Expression.Call(InfoOf(() => AsyncQueryable.Max<TSource>(default(IAsyncQueryable<TSource>), default(Expression<Func<TSource, float>>), default(CancellationToken))), source.Expression, selector, Expression.Constant(cancellationToken, typeof(CancellationToken))), cancellationToken);
#else
            return source.Provider.ExecuteAsync<float>(Expression.Call(((MethodInfo)MethodBase.GetCurrentMethod()).MakeGenericMethod(typeof(TSource)), source.Expression, selector, Expression.Constant(cancellationToken, typeof(CancellationToken))), cancellationToken);
#endif
        }

        public static Task<int?> Max<TSource>(this IAsyncQueryable<TSource> source, Expression<Func<TSource, int?>> selector, CancellationToken cancellationToken)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));
            if (selector == null)
                throw new ArgumentNullException(nameof(selector));

#if CRIPPLED_REFLECTION
            return source.Provider.ExecuteAsync<int?>(Expression.Call(InfoOf(() => AsyncQueryable.Max<TSource>(default(IAsyncQueryable<TSource>), default(Expression<Func<TSource, int?>>), default(CancellationToken))), source.Expression, selector, Expression.Constant(cancellationToken, typeof(CancellationToken))), cancellationToken);
#else
            return source.Provider.ExecuteAsync<int?>(Expression.Call(((MethodInfo)MethodBase.GetCurrentMethod()).MakeGenericMethod(typeof(TSource)), source.Expression, selector, Expression.Constant(cancellationToken, typeof(CancellationToken))), cancellationToken);
#endif
        }

        public static Task<int> Max<TSource>(this IAsyncQueryable<TSource> source, Expression<Func<TSource, int>> selector, CancellationToken cancellationToken)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));
            if (selector == null)
                throw new ArgumentNullException(nameof(selector));

#if CRIPPLED_REFLECTION
            return source.Provider.ExecuteAsync<int>(Expression.Call(InfoOf(() => AsyncQueryable.Max<TSource>(default(IAsyncQueryable<TSource>), default(Expression<Func<TSource, int>>), default(CancellationToken))), source.Expression, selector, Expression.Constant(cancellationToken, typeof(CancellationToken))), cancellationToken);
#else
            return source.Provider.ExecuteAsync<int>(Expression.Call(((MethodInfo)MethodBase.GetCurrentMethod()).MakeGenericMethod(typeof(TSource)), source.Expression, selector, Expression.Constant(cancellationToken, typeof(CancellationToken))), cancellationToken);
#endif
        }

        public static Task<long?> Max<TSource>(this IAsyncQueryable<TSource> source, Expression<Func<TSource, long?>> selector, CancellationToken cancellationToken)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));
            if (selector == null)
                throw new ArgumentNullException(nameof(selector));

#if CRIPPLED_REFLECTION
            return source.Provider.ExecuteAsync<long?>(Expression.Call(InfoOf(() => AsyncQueryable.Max<TSource>(default(IAsyncQueryable<TSource>), default(Expression<Func<TSource, long?>>), default(CancellationToken))), source.Expression, selector, Expression.Constant(cancellationToken, typeof(CancellationToken))), cancellationToken);
#else
            return source.Provider.ExecuteAsync<long?>(Expression.Call(((MethodInfo)MethodBase.GetCurrentMethod()).MakeGenericMethod(typeof(TSource)), source.Expression, selector, Expression.Constant(cancellationToken, typeof(CancellationToken))), cancellationToken);
#endif
        }

        public static Task<long> Max<TSource>(this IAsyncQueryable<TSource> source, Expression<Func<TSource, long>> selector, CancellationToken cancellationToken)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));
            if (selector == null)
                throw new ArgumentNullException(nameof(selector));

#if CRIPPLED_REFLECTION
            return source.Provider.ExecuteAsync<long>(Expression.Call(InfoOf(() => AsyncQueryable.Max<TSource>(default(IAsyncQueryable<TSource>), default(Expression<Func<TSource, long>>), default(CancellationToken))), source.Expression, selector, Expression.Constant(cancellationToken, typeof(CancellationToken))), cancellationToken);
#else
            return source.Provider.ExecuteAsync<long>(Expression.Call(((MethodInfo)MethodBase.GetCurrentMethod()).MakeGenericMethod(typeof(TSource)), source.Expression, selector, Expression.Constant(cancellationToken, typeof(CancellationToken))), cancellationToken);
#endif
        }

        public static Task<decimal?> Max<TSource>(this IAsyncQueryable<TSource> source, Expression<Func<TSource, Task<decimal?>>> selector, CancellationToken cancellationToken)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));
            if (selector == null)
                throw new ArgumentNullException(nameof(selector));

#if CRIPPLED_REFLECTION
            return source.Provider.ExecuteAsync<decimal?>(Expression.Call(InfoOf(() => AsyncQueryable.Max<TSource>(default(IAsyncQueryable<TSource>), default(Expression<Func<TSource, Task<decimal?>>>), default(CancellationToken))), source.Expression, selector, Expression.Constant(cancellationToken, typeof(CancellationToken))), cancellationToken);
#else
            return source.Provider.ExecuteAsync<decimal?>(Expression.Call(((MethodInfo)MethodBase.GetCurrentMethod()).MakeGenericMethod(typeof(TSource)), source.Expression, selector, Expression.Constant(cancellationToken, typeof(CancellationToken))), cancellationToken);
#endif
        }

        public static Task<decimal> Max<TSource>(this IAsyncQueryable<TSource> source, Expression<Func<TSource, Task<decimal>>> selector, CancellationToken cancellationToken)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));
            if (selector == null)
                throw new ArgumentNullException(nameof(selector));

#if CRIPPLED_REFLECTION
            return source.Provider.ExecuteAsync<decimal>(Expression.Call(InfoOf(() => AsyncQueryable.Max<TSource>(default(IAsyncQueryable<TSource>), default(Expression<Func<TSource, Task<decimal>>>), default(CancellationToken))), source.Expression, selector, Expression.Constant(cancellationToken, typeof(CancellationToken))), cancellationToken);
#else
            return source.Provider.ExecuteAsync<decimal>(Expression.Call(((MethodInfo)MethodBase.GetCurrentMethod()).MakeGenericMethod(typeof(TSource)), source.Expression, selector, Expression.Constant(cancellationToken, typeof(CancellationToken))), cancellationToken);
#endif
        }

        public static Task<double?> Max<TSource>(this IAsyncQueryable<TSource> source, Expression<Func<TSource, Task<double?>>> selector, CancellationToken cancellationToken)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));
            if (selector == null)
                throw new ArgumentNullException(nameof(selector));

#if CRIPPLED_REFLECTION
            return source.Provider.ExecuteAsync<double?>(Expression.Call(InfoOf(() => AsyncQueryable.Max<TSource>(default(IAsyncQueryable<TSource>), default(Expression<Func<TSource, Task<double?>>>), default(CancellationToken))), source.Expression, selector, Expression.Constant(cancellationToken, typeof(CancellationToken))), cancellationToken);
#else
            return source.Provider.ExecuteAsync<double?>(Expression.Call(((MethodInfo)MethodBase.GetCurrentMethod()).MakeGenericMethod(typeof(TSource)), source.Expression, selector, Expression.Constant(cancellationToken, typeof(CancellationToken))), cancellationToken);
#endif
        }

        public static Task<double> Max<TSource>(this IAsyncQueryable<TSource> source, Expression<Func<TSource, Task<double>>> selector, CancellationToken cancellationToken)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));
            if (selector == null)
                throw new ArgumentNullException(nameof(selector));

#if CRIPPLED_REFLECTION
            return source.Provider.ExecuteAsync<double>(Expression.Call(InfoOf(() => AsyncQueryable.Max<TSource>(default(IAsyncQueryable<TSource>), default(Expression<Func<TSource, Task<double>>>), default(CancellationToken))), source.Expression, selector, Expression.Constant(cancellationToken, typeof(CancellationToken))), cancellationToken);
#else
            return source.Provider.ExecuteAsync<double>(Expression.Call(((MethodInfo)MethodBase.GetCurrentMethod()).MakeGenericMethod(typeof(TSource)), source.Expression, selector, Expression.Constant(cancellationToken, typeof(CancellationToken))), cancellationToken);
#endif
        }

        public static Task<float?> Max<TSource>(this IAsyncQueryable<TSource> source, Expression<Func<TSource, Task<float?>>> selector, CancellationToken cancellationToken)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));
            if (selector == null)
                throw new ArgumentNullException(nameof(selector));

#if CRIPPLED_REFLECTION
            return source.Provider.ExecuteAsync<float?>(Expression.Call(InfoOf(() => AsyncQueryable.Max<TSource>(default(IAsyncQueryable<TSource>), default(Expression<Func<TSource, Task<float?>>>), default(CancellationToken))), source.Expression, selector, Expression.Constant(cancellationToken, typeof(CancellationToken))), cancellationToken);
#else
            return source.Provider.ExecuteAsync<float?>(Expression.Call(((MethodInfo)MethodBase.GetCurrentMethod()).MakeGenericMethod(typeof(TSource)), source.Expression, selector, Expression.Constant(cancellationToken, typeof(CancellationToken))), cancellationToken);
#endif
        }

        public static Task<float> Max<TSource>(this IAsyncQueryable<TSource> source, Expression<Func<TSource, Task<float>>> selector, CancellationToken cancellationToken)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));
            if (selector == null)
                throw new ArgumentNullException(nameof(selector));

#if CRIPPLED_REFLECTION
            return source.Provider.ExecuteAsync<float>(Expression.Call(InfoOf(() => AsyncQueryable.Max<TSource>(default(IAsyncQueryable<TSource>), default(Expression<Func<TSource, Task<float>>>), default(CancellationToken))), source.Expression, selector, Expression.Constant(cancellationToken, typeof(CancellationToken))), cancellationToken);
#else
            return source.Provider.ExecuteAsync<float>(Expression.Call(((MethodInfo)MethodBase.GetCurrentMethod()).MakeGenericMethod(typeof(TSource)), source.Expression, selector, Expression.Constant(cancellationToken, typeof(CancellationToken))), cancellationToken);
#endif
        }

        public static Task<int?> Max<TSource>(this IAsyncQueryable<TSource> source, Expression<Func<TSource, Task<int?>>> selector, CancellationToken cancellationToken)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));
            if (selector == null)
                throw new ArgumentNullException(nameof(selector));

#if CRIPPLED_REFLECTION
            return source.Provider.ExecuteAsync<int?>(Expression.Call(InfoOf(() => AsyncQueryable.Max<TSource>(default(IAsyncQueryable<TSource>), default(Expression<Func<TSource, Task<int?>>>), default(CancellationToken))), source.Expression, selector, Expression.Constant(cancellationToken, typeof(CancellationToken))), cancellationToken);
#else
            return source.Provider.ExecuteAsync<int?>(Expression.Call(((MethodInfo)MethodBase.GetCurrentMethod()).MakeGenericMethod(typeof(TSource)), source.Expression, selector, Expression.Constant(cancellationToken, typeof(CancellationToken))), cancellationToken);
#endif
        }

        public static Task<int> Max<TSource>(this IAsyncQueryable<TSource> source, Expression<Func<TSource, Task<int>>> selector, CancellationToken cancellationToken)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));
            if (selector == null)
                throw new ArgumentNullException(nameof(selector));

#if CRIPPLED_REFLECTION
            return source.Provider.ExecuteAsync<int>(Expression.Call(InfoOf(() => AsyncQueryable.Max<TSource>(default(IAsyncQueryable<TSource>), default(Expression<Func<TSource, Task<int>>>), default(CancellationToken))), source.Expression, selector, Expression.Constant(cancellationToken, typeof(CancellationToken))), cancellationToken);
#else
            return source.Provider.ExecuteAsync<int>(Expression.Call(((MethodInfo)MethodBase.GetCurrentMethod()).MakeGenericMethod(typeof(TSource)), source.Expression, selector, Expression.Constant(cancellationToken, typeof(CancellationToken))), cancellationToken);
#endif
        }

        public static Task<long?> Max<TSource>(this IAsyncQueryable<TSource> source, Expression<Func<TSource, Task<long?>>> selector, CancellationToken cancellationToken)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));
            if (selector == null)
                throw new ArgumentNullException(nameof(selector));

#if CRIPPLED_REFLECTION
            return source.Provider.ExecuteAsync<long?>(Expression.Call(InfoOf(() => AsyncQueryable.Max<TSource>(default(IAsyncQueryable<TSource>), default(Expression<Func<TSource, Task<long?>>>), default(CancellationToken))), source.Expression, selector, Expression.Constant(cancellationToken, typeof(CancellationToken))), cancellationToken);
#else
            return source.Provider.ExecuteAsync<long?>(Expression.Call(((MethodInfo)MethodBase.GetCurrentMethod()).MakeGenericMethod(typeof(TSource)), source.Expression, selector, Expression.Constant(cancellationToken, typeof(CancellationToken))), cancellationToken);
#endif
        }

        public static Task<long> Max<TSource>(this IAsyncQueryable<TSource> source, Expression<Func<TSource, Task<long>>> selector, CancellationToken cancellationToken)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));
            if (selector == null)
                throw new ArgumentNullException(nameof(selector));

#if CRIPPLED_REFLECTION
            return source.Provider.ExecuteAsync<long>(Expression.Call(InfoOf(() => AsyncQueryable.Max<TSource>(default(IAsyncQueryable<TSource>), default(Expression<Func<TSource, Task<long>>>), default(CancellationToken))), source.Expression, selector, Expression.Constant(cancellationToken, typeof(CancellationToken))), cancellationToken);
#else
            return source.Provider.ExecuteAsync<long>(Expression.Call(((MethodInfo)MethodBase.GetCurrentMethod()).MakeGenericMethod(typeof(TSource)), source.Expression, selector, Expression.Constant(cancellationToken, typeof(CancellationToken))), cancellationToken);
#endif
        }

        public static Task<TResult> Max<TSource, TResult>(this IAsyncQueryable<TSource> source, Expression<Func<TSource, Task<TResult>>> selector)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));
            if (selector == null)
                throw new ArgumentNullException(nameof(selector));

#if CRIPPLED_REFLECTION
            return source.Provider.ExecuteAsync<TResult>(Expression.Call(InfoOf(() => AsyncQueryable.Max<TSource, TResult>(default(IAsyncQueryable<TSource>), default(Expression<Func<TSource, Task<TResult>>>))), source.Expression, selector), CancellationToken.None);
#else
            return source.Provider.ExecuteAsync<TResult>(Expression.Call(((MethodInfo)MethodBase.GetCurrentMethod()).MakeGenericMethod(typeof(TSource), typeof(TResult)), source.Expression, selector), CancellationToken.None);
#endif
        }

        public static Task<TResult> Max<TSource, TResult>(this IAsyncQueryable<TSource> source, Expression<Func<TSource, TResult>> selector)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));
            if (selector == null)
                throw new ArgumentNullException(nameof(selector));

#if CRIPPLED_REFLECTION
            return source.Provider.ExecuteAsync<TResult>(Expression.Call(InfoOf(() => AsyncQueryable.Max<TSource, TResult>(default(IAsyncQueryable<TSource>), default(Expression<Func<TSource, TResult>>))), source.Expression, selector), CancellationToken.None);
#else
            return source.Provider.ExecuteAsync<TResult>(Expression.Call(((MethodInfo)MethodBase.GetCurrentMethod()).MakeGenericMethod(typeof(TSource), typeof(TResult)), source.Expression, selector), CancellationToken.None);
#endif
        }

        public static Task<TResult> Max<TSource, TResult>(this IAsyncQueryable<TSource> source, Expression<Func<TSource, Task<TResult>>> selector, CancellationToken cancellationToken)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));
            if (selector == null)
                throw new ArgumentNullException(nameof(selector));

#if CRIPPLED_REFLECTION
            return source.Provider.ExecuteAsync<TResult>(Expression.Call(InfoOf(() => AsyncQueryable.Max<TSource, TResult>(default(IAsyncQueryable<TSource>), default(Expression<Func<TSource, Task<TResult>>>), default(CancellationToken))), source.Expression, selector, Expression.Constant(cancellationToken, typeof(CancellationToken))), cancellationToken);
#else
            return source.Provider.ExecuteAsync<TResult>(Expression.Call(((MethodInfo)MethodBase.GetCurrentMethod()).MakeGenericMethod(typeof(TSource), typeof(TResult)), source.Expression, selector, Expression.Constant(cancellationToken, typeof(CancellationToken))), cancellationToken);
#endif
        }

        public static Task<TResult> Max<TSource, TResult>(this IAsyncQueryable<TSource> source, Expression<Func<TSource, TResult>> selector, CancellationToken cancellationToken)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));
            if (selector == null)
                throw new ArgumentNullException(nameof(selector));

#if CRIPPLED_REFLECTION
            return source.Provider.ExecuteAsync<TResult>(Expression.Call(InfoOf(() => AsyncQueryable.Max<TSource, TResult>(default(IAsyncQueryable<TSource>), default(Expression<Func<TSource, TResult>>), default(CancellationToken))), source.Expression, selector, Expression.Constant(cancellationToken, typeof(CancellationToken))), cancellationToken);
#else
            return source.Provider.ExecuteAsync<TResult>(Expression.Call(((MethodInfo)MethodBase.GetCurrentMethod()).MakeGenericMethod(typeof(TSource), typeof(TResult)), source.Expression, selector, Expression.Constant(cancellationToken, typeof(CancellationToken))), cancellationToken);
#endif
        }

        public static Task<decimal?> Min(this IAsyncQueryable<decimal?> source)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));

#if CRIPPLED_REFLECTION
            return source.Provider.ExecuteAsync<decimal?>(Expression.Call(InfoOf(() => AsyncQueryable.Min(default(IAsyncQueryable<decimal?>))), source.Expression), CancellationToken.None);
#else
            return source.Provider.ExecuteAsync<decimal?>(Expression.Call((MethodInfo)MethodBase.GetCurrentMethod(), source.Expression), CancellationToken.None);
#endif
        }

        public static Task<decimal> Min(this IAsyncQueryable<decimal> source)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));

#if CRIPPLED_REFLECTION
            return source.Provider.ExecuteAsync<decimal>(Expression.Call(InfoOf(() => AsyncQueryable.Min(default(IAsyncQueryable<decimal>))), source.Expression), CancellationToken.None);
#else
            return source.Provider.ExecuteAsync<decimal>(Expression.Call((MethodInfo)MethodBase.GetCurrentMethod(), source.Expression), CancellationToken.None);
#endif
        }

        public static Task<double?> Min(this IAsyncQueryable<double?> source)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));

#if CRIPPLED_REFLECTION
            return source.Provider.ExecuteAsync<double?>(Expression.Call(InfoOf(() => AsyncQueryable.Min(default(IAsyncQueryable<double?>))), source.Expression), CancellationToken.None);
#else
            return source.Provider.ExecuteAsync<double?>(Expression.Call((MethodInfo)MethodBase.GetCurrentMethod(), source.Expression), CancellationToken.None);
#endif
        }

        public static Task<double> Min(this IAsyncQueryable<double> source)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));

#if CRIPPLED_REFLECTION
            return source.Provider.ExecuteAsync<double>(Expression.Call(InfoOf(() => AsyncQueryable.Min(default(IAsyncQueryable<double>))), source.Expression), CancellationToken.None);
#else
            return source.Provider.ExecuteAsync<double>(Expression.Call((MethodInfo)MethodBase.GetCurrentMethod(), source.Expression), CancellationToken.None);
#endif
        }

        public static Task<float?> Min(this IAsyncQueryable<float?> source)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));

#if CRIPPLED_REFLECTION
            return source.Provider.ExecuteAsync<float?>(Expression.Call(InfoOf(() => AsyncQueryable.Min(default(IAsyncQueryable<float?>))), source.Expression), CancellationToken.None);
#else
            return source.Provider.ExecuteAsync<float?>(Expression.Call((MethodInfo)MethodBase.GetCurrentMethod(), source.Expression), CancellationToken.None);
#endif
        }

        public static Task<float> Min(this IAsyncQueryable<float> source)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));

#if CRIPPLED_REFLECTION
            return source.Provider.ExecuteAsync<float>(Expression.Call(InfoOf(() => AsyncQueryable.Min(default(IAsyncQueryable<float>))), source.Expression), CancellationToken.None);
#else
            return source.Provider.ExecuteAsync<float>(Expression.Call((MethodInfo)MethodBase.GetCurrentMethod(), source.Expression), CancellationToken.None);
#endif
        }

        public static Task<int?> Min(this IAsyncQueryable<int?> source)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));

#if CRIPPLED_REFLECTION
            return source.Provider.ExecuteAsync<int?>(Expression.Call(InfoOf(() => AsyncQueryable.Min(default(IAsyncQueryable<int?>))), source.Expression), CancellationToken.None);
#else
            return source.Provider.ExecuteAsync<int?>(Expression.Call((MethodInfo)MethodBase.GetCurrentMethod(), source.Expression), CancellationToken.None);
#endif
        }

        public static Task<int> Min(this IAsyncQueryable<int> source)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));

#if CRIPPLED_REFLECTION
            return source.Provider.ExecuteAsync<int>(Expression.Call(InfoOf(() => AsyncQueryable.Min(default(IAsyncQueryable<int>))), source.Expression), CancellationToken.None);
#else
            return source.Provider.ExecuteAsync<int>(Expression.Call((MethodInfo)MethodBase.GetCurrentMethod(), source.Expression), CancellationToken.None);
#endif
        }

        public static Task<long?> Min(this IAsyncQueryable<long?> source)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));

#if CRIPPLED_REFLECTION
            return source.Provider.ExecuteAsync<long?>(Expression.Call(InfoOf(() => AsyncQueryable.Min(default(IAsyncQueryable<long?>))), source.Expression), CancellationToken.None);
#else
            return source.Provider.ExecuteAsync<long?>(Expression.Call((MethodInfo)MethodBase.GetCurrentMethod(), source.Expression), CancellationToken.None);
#endif
        }

        public static Task<long> Min(this IAsyncQueryable<long> source)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));

#if CRIPPLED_REFLECTION
            return source.Provider.ExecuteAsync<long>(Expression.Call(InfoOf(() => AsyncQueryable.Min(default(IAsyncQueryable<long>))), source.Expression), CancellationToken.None);
#else
            return source.Provider.ExecuteAsync<long>(Expression.Call((MethodInfo)MethodBase.GetCurrentMethod(), source.Expression), CancellationToken.None);
#endif
        }

        public static Task<decimal?> Min(this IAsyncQueryable<decimal?> source, CancellationToken cancellationToken)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));

#if CRIPPLED_REFLECTION
            return source.Provider.ExecuteAsync<decimal?>(Expression.Call(InfoOf(() => AsyncQueryable.Min(default(IAsyncQueryable<decimal?>), default(CancellationToken))), source.Expression, Expression.Constant(cancellationToken, typeof(CancellationToken))), cancellationToken);
#else
            return source.Provider.ExecuteAsync<decimal?>(Expression.Call((MethodInfo)MethodBase.GetCurrentMethod(), source.Expression, Expression.Constant(cancellationToken, typeof(CancellationToken))), cancellationToken);
#endif
        }

        public static Task<decimal> Min(this IAsyncQueryable<decimal> source, CancellationToken cancellationToken)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));

#if CRIPPLED_REFLECTION
            return source.Provider.ExecuteAsync<decimal>(Expression.Call(InfoOf(() => AsyncQueryable.Min(default(IAsyncQueryable<decimal>), default(CancellationToken))), source.Expression, Expression.Constant(cancellationToken, typeof(CancellationToken))), cancellationToken);
#else
            return source.Provider.ExecuteAsync<decimal>(Expression.Call((MethodInfo)MethodBase.GetCurrentMethod(), source.Expression, Expression.Constant(cancellationToken, typeof(CancellationToken))), cancellationToken);
#endif
        }

        public static Task<double?> Min(this IAsyncQueryable<double?> source, CancellationToken cancellationToken)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));

#if CRIPPLED_REFLECTION
            return source.Provider.ExecuteAsync<double?>(Expression.Call(InfoOf(() => AsyncQueryable.Min(default(IAsyncQueryable<double?>), default(CancellationToken))), source.Expression, Expression.Constant(cancellationToken, typeof(CancellationToken))), cancellationToken);
#else
            return source.Provider.ExecuteAsync<double?>(Expression.Call((MethodInfo)MethodBase.GetCurrentMethod(), source.Expression, Expression.Constant(cancellationToken, typeof(CancellationToken))), cancellationToken);
#endif
        }

        public static Task<double> Min(this IAsyncQueryable<double> source, CancellationToken cancellationToken)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));

#if CRIPPLED_REFLECTION
            return source.Provider.ExecuteAsync<double>(Expression.Call(InfoOf(() => AsyncQueryable.Min(default(IAsyncQueryable<double>), default(CancellationToken))), source.Expression, Expression.Constant(cancellationToken, typeof(CancellationToken))), cancellationToken);
#else
            return source.Provider.ExecuteAsync<double>(Expression.Call((MethodInfo)MethodBase.GetCurrentMethod(), source.Expression, Expression.Constant(cancellationToken, typeof(CancellationToken))), cancellationToken);
#endif
        }

        public static Task<float?> Min(this IAsyncQueryable<float?> source, CancellationToken cancellationToken)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));

#if CRIPPLED_REFLECTION
            return source.Provider.ExecuteAsync<float?>(Expression.Call(InfoOf(() => AsyncQueryable.Min(default(IAsyncQueryable<float?>), default(CancellationToken))), source.Expression, Expression.Constant(cancellationToken, typeof(CancellationToken))), cancellationToken);
#else
            return source.Provider.ExecuteAsync<float?>(Expression.Call((MethodInfo)MethodBase.GetCurrentMethod(), source.Expression, Expression.Constant(cancellationToken, typeof(CancellationToken))), cancellationToken);
#endif
        }

        public static Task<float> Min(this IAsyncQueryable<float> source, CancellationToken cancellationToken)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));

#if CRIPPLED_REFLECTION
            return source.Provider.ExecuteAsync<float>(Expression.Call(InfoOf(() => AsyncQueryable.Min(default(IAsyncQueryable<float>), default(CancellationToken))), source.Expression, Expression.Constant(cancellationToken, typeof(CancellationToken))), cancellationToken);
#else
            return source.Provider.ExecuteAsync<float>(Expression.Call((MethodInfo)MethodBase.GetCurrentMethod(), source.Expression, Expression.Constant(cancellationToken, typeof(CancellationToken))), cancellationToken);
#endif
        }

        public static Task<int?> Min(this IAsyncQueryable<int?> source, CancellationToken cancellationToken)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));

#if CRIPPLED_REFLECTION
            return source.Provider.ExecuteAsync<int?>(Expression.Call(InfoOf(() => AsyncQueryable.Min(default(IAsyncQueryable<int?>), default(CancellationToken))), source.Expression, Expression.Constant(cancellationToken, typeof(CancellationToken))), cancellationToken);
#else
            return source.Provider.ExecuteAsync<int?>(Expression.Call((MethodInfo)MethodBase.GetCurrentMethod(), source.Expression, Expression.Constant(cancellationToken, typeof(CancellationToken))), cancellationToken);
#endif
        }

        public static Task<int> Min(this IAsyncQueryable<int> source, CancellationToken cancellationToken)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));

#if CRIPPLED_REFLECTION
            return source.Provider.ExecuteAsync<int>(Expression.Call(InfoOf(() => AsyncQueryable.Min(default(IAsyncQueryable<int>), default(CancellationToken))), source.Expression, Expression.Constant(cancellationToken, typeof(CancellationToken))), cancellationToken);
#else
            return source.Provider.ExecuteAsync<int>(Expression.Call((MethodInfo)MethodBase.GetCurrentMethod(), source.Expression, Expression.Constant(cancellationToken, typeof(CancellationToken))), cancellationToken);
#endif
        }

        public static Task<long?> Min(this IAsyncQueryable<long?> source, CancellationToken cancellationToken)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));

#if CRIPPLED_REFLECTION
            return source.Provider.ExecuteAsync<long?>(Expression.Call(InfoOf(() => AsyncQueryable.Min(default(IAsyncQueryable<long?>), default(CancellationToken))), source.Expression, Expression.Constant(cancellationToken, typeof(CancellationToken))), cancellationToken);
#else
            return source.Provider.ExecuteAsync<long?>(Expression.Call((MethodInfo)MethodBase.GetCurrentMethod(), source.Expression, Expression.Constant(cancellationToken, typeof(CancellationToken))), cancellationToken);
#endif
        }

        public static Task<long> Min(this IAsyncQueryable<long> source, CancellationToken cancellationToken)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));

#if CRIPPLED_REFLECTION
            return source.Provider.ExecuteAsync<long>(Expression.Call(InfoOf(() => AsyncQueryable.Min(default(IAsyncQueryable<long>), default(CancellationToken))), source.Expression, Expression.Constant(cancellationToken, typeof(CancellationToken))), cancellationToken);
#else
            return source.Provider.ExecuteAsync<long>(Expression.Call((MethodInfo)MethodBase.GetCurrentMethod(), source.Expression, Expression.Constant(cancellationToken, typeof(CancellationToken))), cancellationToken);
#endif
        }

        public static Task<TSource> Min<TSource>(this IAsyncQueryable<TSource> source)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));

#if CRIPPLED_REFLECTION
            return source.Provider.ExecuteAsync<TSource>(Expression.Call(InfoOf(() => AsyncQueryable.Min<TSource>(default(IAsyncQueryable<TSource>))), source.Expression), CancellationToken.None);
#else
            return source.Provider.ExecuteAsync<TSource>(Expression.Call(((MethodInfo)MethodBase.GetCurrentMethod()).MakeGenericMethod(typeof(TSource)), source.Expression), CancellationToken.None);
#endif
        }

        public static Task<TSource> Min<TSource>(this IAsyncQueryable<TSource> source, CancellationToken cancellationToken)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));

#if CRIPPLED_REFLECTION
            return source.Provider.ExecuteAsync<TSource>(Expression.Call(InfoOf(() => AsyncQueryable.Min<TSource>(default(IAsyncQueryable<TSource>), default(CancellationToken))), source.Expression, Expression.Constant(cancellationToken, typeof(CancellationToken))), cancellationToken);
#else
            return source.Provider.ExecuteAsync<TSource>(Expression.Call(((MethodInfo)MethodBase.GetCurrentMethod()).MakeGenericMethod(typeof(TSource)), source.Expression, Expression.Constant(cancellationToken, typeof(CancellationToken))), cancellationToken);
#endif
        }

        public static Task<decimal?> Min<TSource>(this IAsyncQueryable<TSource> source, Expression<Func<TSource, decimal?>> selector)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));
            if (selector == null)
                throw new ArgumentNullException(nameof(selector));

#if CRIPPLED_REFLECTION
            return source.Provider.ExecuteAsync<decimal?>(Expression.Call(InfoOf(() => AsyncQueryable.Min<TSource>(default(IAsyncQueryable<TSource>), default(Expression<Func<TSource, decimal?>>))), source.Expression, selector), CancellationToken.None);
#else
            return source.Provider.ExecuteAsync<decimal?>(Expression.Call(((MethodInfo)MethodBase.GetCurrentMethod()).MakeGenericMethod(typeof(TSource)), source.Expression, selector), CancellationToken.None);
#endif
        }

        public static Task<decimal> Min<TSource>(this IAsyncQueryable<TSource> source, Expression<Func<TSource, decimal>> selector)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));
            if (selector == null)
                throw new ArgumentNullException(nameof(selector));

#if CRIPPLED_REFLECTION
            return source.Provider.ExecuteAsync<decimal>(Expression.Call(InfoOf(() => AsyncQueryable.Min<TSource>(default(IAsyncQueryable<TSource>), default(Expression<Func<TSource, decimal>>))), source.Expression, selector), CancellationToken.None);
#else
            return source.Provider.ExecuteAsync<decimal>(Expression.Call(((MethodInfo)MethodBase.GetCurrentMethod()).MakeGenericMethod(typeof(TSource)), source.Expression, selector), CancellationToken.None);
#endif
        }

        public static Task<double?> Min<TSource>(this IAsyncQueryable<TSource> source, Expression<Func<TSource, double?>> selector)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));
            if (selector == null)
                throw new ArgumentNullException(nameof(selector));

#if CRIPPLED_REFLECTION
            return source.Provider.ExecuteAsync<double?>(Expression.Call(InfoOf(() => AsyncQueryable.Min<TSource>(default(IAsyncQueryable<TSource>), default(Expression<Func<TSource, double?>>))), source.Expression, selector), CancellationToken.None);
#else
            return source.Provider.ExecuteAsync<double?>(Expression.Call(((MethodInfo)MethodBase.GetCurrentMethod()).MakeGenericMethod(typeof(TSource)), source.Expression, selector), CancellationToken.None);
#endif
        }

        public static Task<double> Min<TSource>(this IAsyncQueryable<TSource> source, Expression<Func<TSource, double>> selector)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));
            if (selector == null)
                throw new ArgumentNullException(nameof(selector));

#if CRIPPLED_REFLECTION
            return source.Provider.ExecuteAsync<double>(Expression.Call(InfoOf(() => AsyncQueryable.Min<TSource>(default(IAsyncQueryable<TSource>), default(Expression<Func<TSource, double>>))), source.Expression, selector), CancellationToken.None);
#else
            return source.Provider.ExecuteAsync<double>(Expression.Call(((MethodInfo)MethodBase.GetCurrentMethod()).MakeGenericMethod(typeof(TSource)), source.Expression, selector), CancellationToken.None);
#endif
        }

        public static Task<float?> Min<TSource>(this IAsyncQueryable<TSource> source, Expression<Func<TSource, float?>> selector)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));
            if (selector == null)
                throw new ArgumentNullException(nameof(selector));

#if CRIPPLED_REFLECTION
            return source.Provider.ExecuteAsync<float?>(Expression.Call(InfoOf(() => AsyncQueryable.Min<TSource>(default(IAsyncQueryable<TSource>), default(Expression<Func<TSource, float?>>))), source.Expression, selector), CancellationToken.None);
#else
            return source.Provider.ExecuteAsync<float?>(Expression.Call(((MethodInfo)MethodBase.GetCurrentMethod()).MakeGenericMethod(typeof(TSource)), source.Expression, selector), CancellationToken.None);
#endif
        }

        public static Task<float> Min<TSource>(this IAsyncQueryable<TSource> source, Expression<Func<TSource, float>> selector)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));
            if (selector == null)
                throw new ArgumentNullException(nameof(selector));

#if CRIPPLED_REFLECTION
            return source.Provider.ExecuteAsync<float>(Expression.Call(InfoOf(() => AsyncQueryable.Min<TSource>(default(IAsyncQueryable<TSource>), default(Expression<Func<TSource, float>>))), source.Expression, selector), CancellationToken.None);
#else
            return source.Provider.ExecuteAsync<float>(Expression.Call(((MethodInfo)MethodBase.GetCurrentMethod()).MakeGenericMethod(typeof(TSource)), source.Expression, selector), CancellationToken.None);
#endif
        }

        public static Task<int?> Min<TSource>(this IAsyncQueryable<TSource> source, Expression<Func<TSource, int?>> selector)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));
            if (selector == null)
                throw new ArgumentNullException(nameof(selector));

#if CRIPPLED_REFLECTION
            return source.Provider.ExecuteAsync<int?>(Expression.Call(InfoOf(() => AsyncQueryable.Min<TSource>(default(IAsyncQueryable<TSource>), default(Expression<Func<TSource, int?>>))), source.Expression, selector), CancellationToken.None);
#else
            return source.Provider.ExecuteAsync<int?>(Expression.Call(((MethodInfo)MethodBase.GetCurrentMethod()).MakeGenericMethod(typeof(TSource)), source.Expression, selector), CancellationToken.None);
#endif
        }

        public static Task<int> Min<TSource>(this IAsyncQueryable<TSource> source, Expression<Func<TSource, int>> selector)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));
            if (selector == null)
                throw new ArgumentNullException(nameof(selector));

#if CRIPPLED_REFLECTION
            return source.Provider.ExecuteAsync<int>(Expression.Call(InfoOf(() => AsyncQueryable.Min<TSource>(default(IAsyncQueryable<TSource>), default(Expression<Func<TSource, int>>))), source.Expression, selector), CancellationToken.None);
#else
            return source.Provider.ExecuteAsync<int>(Expression.Call(((MethodInfo)MethodBase.GetCurrentMethod()).MakeGenericMethod(typeof(TSource)), source.Expression, selector), CancellationToken.None);
#endif
        }

        public static Task<long?> Min<TSource>(this IAsyncQueryable<TSource> source, Expression<Func<TSource, long?>> selector)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));
            if (selector == null)
                throw new ArgumentNullException(nameof(selector));

#if CRIPPLED_REFLECTION
            return source.Provider.ExecuteAsync<long?>(Expression.Call(InfoOf(() => AsyncQueryable.Min<TSource>(default(IAsyncQueryable<TSource>), default(Expression<Func<TSource, long?>>))), source.Expression, selector), CancellationToken.None);
#else
            return source.Provider.ExecuteAsync<long?>(Expression.Call(((MethodInfo)MethodBase.GetCurrentMethod()).MakeGenericMethod(typeof(TSource)), source.Expression, selector), CancellationToken.None);
#endif
        }

        public static Task<long> Min<TSource>(this IAsyncQueryable<TSource> source, Expression<Func<TSource, long>> selector)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));
            if (selector == null)
                throw new ArgumentNullException(nameof(selector));

#if CRIPPLED_REFLECTION
            return source.Provider.ExecuteAsync<long>(Expression.Call(InfoOf(() => AsyncQueryable.Min<TSource>(default(IAsyncQueryable<TSource>), default(Expression<Func<TSource, long>>))), source.Expression, selector), CancellationToken.None);
#else
            return source.Provider.ExecuteAsync<long>(Expression.Call(((MethodInfo)MethodBase.GetCurrentMethod()).MakeGenericMethod(typeof(TSource)), source.Expression, selector), CancellationToken.None);
#endif
        }

        public static Task<decimal?> Min<TSource>(this IAsyncQueryable<TSource> source, Expression<Func<TSource, Task<decimal?>>> selector)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));
            if (selector == null)
                throw new ArgumentNullException(nameof(selector));

#if CRIPPLED_REFLECTION
            return source.Provider.ExecuteAsync<decimal?>(Expression.Call(InfoOf(() => AsyncQueryable.Min<TSource>(default(IAsyncQueryable<TSource>), default(Expression<Func<TSource, Task<decimal?>>>))), source.Expression, selector), CancellationToken.None);
#else
            return source.Provider.ExecuteAsync<decimal?>(Expression.Call(((MethodInfo)MethodBase.GetCurrentMethod()).MakeGenericMethod(typeof(TSource)), source.Expression, selector), CancellationToken.None);
#endif
        }

        public static Task<decimal> Min<TSource>(this IAsyncQueryable<TSource> source, Expression<Func<TSource, Task<decimal>>> selector)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));
            if (selector == null)
                throw new ArgumentNullException(nameof(selector));

#if CRIPPLED_REFLECTION
            return source.Provider.ExecuteAsync<decimal>(Expression.Call(InfoOf(() => AsyncQueryable.Min<TSource>(default(IAsyncQueryable<TSource>), default(Expression<Func<TSource, Task<decimal>>>))), source.Expression, selector), CancellationToken.None);
#else
            return source.Provider.ExecuteAsync<decimal>(Expression.Call(((MethodInfo)MethodBase.GetCurrentMethod()).MakeGenericMethod(typeof(TSource)), source.Expression, selector), CancellationToken.None);
#endif
        }

        public static Task<double?> Min<TSource>(this IAsyncQueryable<TSource> source, Expression<Func<TSource, Task<double?>>> selector)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));
            if (selector == null)
                throw new ArgumentNullException(nameof(selector));

#if CRIPPLED_REFLECTION
            return source.Provider.ExecuteAsync<double?>(Expression.Call(InfoOf(() => AsyncQueryable.Min<TSource>(default(IAsyncQueryable<TSource>), default(Expression<Func<TSource, Task<double?>>>))), source.Expression, selector), CancellationToken.None);
#else
            return source.Provider.ExecuteAsync<double?>(Expression.Call(((MethodInfo)MethodBase.GetCurrentMethod()).MakeGenericMethod(typeof(TSource)), source.Expression, selector), CancellationToken.None);
#endif
        }

        public static Task<double> Min<TSource>(this IAsyncQueryable<TSource> source, Expression<Func<TSource, Task<double>>> selector)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));
            if (selector == null)
                throw new ArgumentNullException(nameof(selector));

#if CRIPPLED_REFLECTION
            return source.Provider.ExecuteAsync<double>(Expression.Call(InfoOf(() => AsyncQueryable.Min<TSource>(default(IAsyncQueryable<TSource>), default(Expression<Func<TSource, Task<double>>>))), source.Expression, selector), CancellationToken.None);
#else
            return source.Provider.ExecuteAsync<double>(Expression.Call(((MethodInfo)MethodBase.GetCurrentMethod()).MakeGenericMethod(typeof(TSource)), source.Expression, selector), CancellationToken.None);
#endif
        }

        public static Task<float?> Min<TSource>(this IAsyncQueryable<TSource> source, Expression<Func<TSource, Task<float?>>> selector)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));
            if (selector == null)
                throw new ArgumentNullException(nameof(selector));

#if CRIPPLED_REFLECTION
            return source.Provider.ExecuteAsync<float?>(Expression.Call(InfoOf(() => AsyncQueryable.Min<TSource>(default(IAsyncQueryable<TSource>), default(Expression<Func<TSource, Task<float?>>>))), source.Expression, selector), CancellationToken.None);
#else
            return source.Provider.ExecuteAsync<float?>(Expression.Call(((MethodInfo)MethodBase.GetCurrentMethod()).MakeGenericMethod(typeof(TSource)), source.Expression, selector), CancellationToken.None);
#endif
        }

        public static Task<float> Min<TSource>(this IAsyncQueryable<TSource> source, Expression<Func<TSource, Task<float>>> selector)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));
            if (selector == null)
                throw new ArgumentNullException(nameof(selector));

#if CRIPPLED_REFLECTION
            return source.Provider.ExecuteAsync<float>(Expression.Call(InfoOf(() => AsyncQueryable.Min<TSource>(default(IAsyncQueryable<TSource>), default(Expression<Func<TSource, Task<float>>>))), source.Expression, selector), CancellationToken.None);
#else
            return source.Provider.ExecuteAsync<float>(Expression.Call(((MethodInfo)MethodBase.GetCurrentMethod()).MakeGenericMethod(typeof(TSource)), source.Expression, selector), CancellationToken.None);
#endif
        }

        public static Task<int?> Min<TSource>(this IAsyncQueryable<TSource> source, Expression<Func<TSource, Task<int?>>> selector)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));
            if (selector == null)
                throw new ArgumentNullException(nameof(selector));

#if CRIPPLED_REFLECTION
            return source.Provider.ExecuteAsync<int?>(Expression.Call(InfoOf(() => AsyncQueryable.Min<TSource>(default(IAsyncQueryable<TSource>), default(Expression<Func<TSource, Task<int?>>>))), source.Expression, selector), CancellationToken.None);
#else
            return source.Provider.ExecuteAsync<int?>(Expression.Call(((MethodInfo)MethodBase.GetCurrentMethod()).MakeGenericMethod(typeof(TSource)), source.Expression, selector), CancellationToken.None);
#endif
        }

        public static Task<int> Min<TSource>(this IAsyncQueryable<TSource> source, Expression<Func<TSource, Task<int>>> selector)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));
            if (selector == null)
                throw new ArgumentNullException(nameof(selector));

#if CRIPPLED_REFLECTION
            return source.Provider.ExecuteAsync<int>(Expression.Call(InfoOf(() => AsyncQueryable.Min<TSource>(default(IAsyncQueryable<TSource>), default(Expression<Func<TSource, Task<int>>>))), source.Expression, selector), CancellationToken.None);
#else
            return source.Provider.ExecuteAsync<int>(Expression.Call(((MethodInfo)MethodBase.GetCurrentMethod()).MakeGenericMethod(typeof(TSource)), source.Expression, selector), CancellationToken.None);
#endif
        }

        public static Task<long?> Min<TSource>(this IAsyncQueryable<TSource> source, Expression<Func<TSource, Task<long?>>> selector)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));
            if (selector == null)
                throw new ArgumentNullException(nameof(selector));

#if CRIPPLED_REFLECTION
            return source.Provider.ExecuteAsync<long?>(Expression.Call(InfoOf(() => AsyncQueryable.Min<TSource>(default(IAsyncQueryable<TSource>), default(Expression<Func<TSource, Task<long?>>>))), source.Expression, selector), CancellationToken.None);
#else
            return source.Provider.ExecuteAsync<long?>(Expression.Call(((MethodInfo)MethodBase.GetCurrentMethod()).MakeGenericMethod(typeof(TSource)), source.Expression, selector), CancellationToken.None);
#endif
        }

        public static Task<long> Min<TSource>(this IAsyncQueryable<TSource> source, Expression<Func<TSource, Task<long>>> selector)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));
            if (selector == null)
                throw new ArgumentNullException(nameof(selector));

#if CRIPPLED_REFLECTION
            return source.Provider.ExecuteAsync<long>(Expression.Call(InfoOf(() => AsyncQueryable.Min<TSource>(default(IAsyncQueryable<TSource>), default(Expression<Func<TSource, Task<long>>>))), source.Expression, selector), CancellationToken.None);
#else
            return source.Provider.ExecuteAsync<long>(Expression.Call(((MethodInfo)MethodBase.GetCurrentMethod()).MakeGenericMethod(typeof(TSource)), source.Expression, selector), CancellationToken.None);
#endif
        }

        public static Task<decimal?> Min<TSource>(this IAsyncQueryable<TSource> source, Expression<Func<TSource, decimal?>> selector, CancellationToken cancellationToken)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));
            if (selector == null)
                throw new ArgumentNullException(nameof(selector));

#if CRIPPLED_REFLECTION
            return source.Provider.ExecuteAsync<decimal?>(Expression.Call(InfoOf(() => AsyncQueryable.Min<TSource>(default(IAsyncQueryable<TSource>), default(Expression<Func<TSource, decimal?>>), default(CancellationToken))), source.Expression, selector, Expression.Constant(cancellationToken, typeof(CancellationToken))), cancellationToken);
#else
            return source.Provider.ExecuteAsync<decimal?>(Expression.Call(((MethodInfo)MethodBase.GetCurrentMethod()).MakeGenericMethod(typeof(TSource)), source.Expression, selector, Expression.Constant(cancellationToken, typeof(CancellationToken))), cancellationToken);
#endif
        }

        public static Task<decimal> Min<TSource>(this IAsyncQueryable<TSource> source, Expression<Func<TSource, decimal>> selector, CancellationToken cancellationToken)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));
            if (selector == null)
                throw new ArgumentNullException(nameof(selector));

#if CRIPPLED_REFLECTION
            return source.Provider.ExecuteAsync<decimal>(Expression.Call(InfoOf(() => AsyncQueryable.Min<TSource>(default(IAsyncQueryable<TSource>), default(Expression<Func<TSource, decimal>>), default(CancellationToken))), source.Expression, selector, Expression.Constant(cancellationToken, typeof(CancellationToken))), cancellationToken);
#else
            return source.Provider.ExecuteAsync<decimal>(Expression.Call(((MethodInfo)MethodBase.GetCurrentMethod()).MakeGenericMethod(typeof(TSource)), source.Expression, selector, Expression.Constant(cancellationToken, typeof(CancellationToken))), cancellationToken);
#endif
        }

        public static Task<double?> Min<TSource>(this IAsyncQueryable<TSource> source, Expression<Func<TSource, double?>> selector, CancellationToken cancellationToken)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));
            if (selector == null)
                throw new ArgumentNullException(nameof(selector));

#if CRIPPLED_REFLECTION
            return source.Provider.ExecuteAsync<double?>(Expression.Call(InfoOf(() => AsyncQueryable.Min<TSource>(default(IAsyncQueryable<TSource>), default(Expression<Func<TSource, double?>>), default(CancellationToken))), source.Expression, selector, Expression.Constant(cancellationToken, typeof(CancellationToken))), cancellationToken);
#else
            return source.Provider.ExecuteAsync<double?>(Expression.Call(((MethodInfo)MethodBase.GetCurrentMethod()).MakeGenericMethod(typeof(TSource)), source.Expression, selector, Expression.Constant(cancellationToken, typeof(CancellationToken))), cancellationToken);
#endif
        }

        public static Task<double> Min<TSource>(this IAsyncQueryable<TSource> source, Expression<Func<TSource, double>> selector, CancellationToken cancellationToken)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));
            if (selector == null)
                throw new ArgumentNullException(nameof(selector));

#if CRIPPLED_REFLECTION
            return source.Provider.ExecuteAsync<double>(Expression.Call(InfoOf(() => AsyncQueryable.Min<TSource>(default(IAsyncQueryable<TSource>), default(Expression<Func<TSource, double>>), default(CancellationToken))), source.Expression, selector, Expression.Constant(cancellationToken, typeof(CancellationToken))), cancellationToken);
#else
            return source.Provider.ExecuteAsync<double>(Expression.Call(((MethodInfo)MethodBase.GetCurrentMethod()).MakeGenericMethod(typeof(TSource)), source.Expression, selector, Expression.Constant(cancellationToken, typeof(CancellationToken))), cancellationToken);
#endif
        }

        public static Task<float?> Min<TSource>(this IAsyncQueryable<TSource> source, Expression<Func<TSource, float?>> selector, CancellationToken cancellationToken)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));
            if (selector == null)
                throw new ArgumentNullException(nameof(selector));

#if CRIPPLED_REFLECTION
            return source.Provider.ExecuteAsync<float?>(Expression.Call(InfoOf(() => AsyncQueryable.Min<TSource>(default(IAsyncQueryable<TSource>), default(Expression<Func<TSource, float?>>), default(CancellationToken))), source.Expression, selector, Expression.Constant(cancellationToken, typeof(CancellationToken))), cancellationToken);
#else
            return source.Provider.ExecuteAsync<float?>(Expression.Call(((MethodInfo)MethodBase.GetCurrentMethod()).MakeGenericMethod(typeof(TSource)), source.Expression, selector, Expression.Constant(cancellationToken, typeof(CancellationToken))), cancellationToken);
#endif
        }

        public static Task<float> Min<TSource>(this IAsyncQueryable<TSource> source, Expression<Func<TSource, float>> selector, CancellationToken cancellationToken)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));
            if (selector == null)
                throw new ArgumentNullException(nameof(selector));

#if CRIPPLED_REFLECTION
            return source.Provider.ExecuteAsync<float>(Expression.Call(InfoOf(() => AsyncQueryable.Min<TSource>(default(IAsyncQueryable<TSource>), default(Expression<Func<TSource, float>>), default(CancellationToken))), source.Expression, selector, Expression.Constant(cancellationToken, typeof(CancellationToken))), cancellationToken);
#else
            return source.Provider.ExecuteAsync<float>(Expression.Call(((MethodInfo)MethodBase.GetCurrentMethod()).MakeGenericMethod(typeof(TSource)), source.Expression, selector, Expression.Constant(cancellationToken, typeof(CancellationToken))), cancellationToken);
#endif
        }

        public static Task<int?> Min<TSource>(this IAsyncQueryable<TSource> source, Expression<Func<TSource, int?>> selector, CancellationToken cancellationToken)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));
            if (selector == null)
                throw new ArgumentNullException(nameof(selector));

#if CRIPPLED_REFLECTION
            return source.Provider.ExecuteAsync<int?>(Expression.Call(InfoOf(() => AsyncQueryable.Min<TSource>(default(IAsyncQueryable<TSource>), default(Expression<Func<TSource, int?>>), default(CancellationToken))), source.Expression, selector, Expression.Constant(cancellationToken, typeof(CancellationToken))), cancellationToken);
#else
            return source.Provider.ExecuteAsync<int?>(Expression.Call(((MethodInfo)MethodBase.GetCurrentMethod()).MakeGenericMethod(typeof(TSource)), source.Expression, selector, Expression.Constant(cancellationToken, typeof(CancellationToken))), cancellationToken);
#endif
        }

        public static Task<int> Min<TSource>(this IAsyncQueryable<TSource> source, Expression<Func<TSource, int>> selector, CancellationToken cancellationToken)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));
            if (selector == null)
                throw new ArgumentNullException(nameof(selector));

#if CRIPPLED_REFLECTION
            return source.Provider.ExecuteAsync<int>(Expression.Call(InfoOf(() => AsyncQueryable.Min<TSource>(default(IAsyncQueryable<TSource>), default(Expression<Func<TSource, int>>), default(CancellationToken))), source.Expression, selector, Expression.Constant(cancellationToken, typeof(CancellationToken))), cancellationToken);
#else
            return source.Provider.ExecuteAsync<int>(Expression.Call(((MethodInfo)MethodBase.GetCurrentMethod()).MakeGenericMethod(typeof(TSource)), source.Expression, selector, Expression.Constant(cancellationToken, typeof(CancellationToken))), cancellationToken);
#endif
        }

        public static Task<long?> Min<TSource>(this IAsyncQueryable<TSource> source, Expression<Func<TSource, long?>> selector, CancellationToken cancellationToken)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));
            if (selector == null)
                throw new ArgumentNullException(nameof(selector));

#if CRIPPLED_REFLECTION
            return source.Provider.ExecuteAsync<long?>(Expression.Call(InfoOf(() => AsyncQueryable.Min<TSource>(default(IAsyncQueryable<TSource>), default(Expression<Func<TSource, long?>>), default(CancellationToken))), source.Expression, selector, Expression.Constant(cancellationToken, typeof(CancellationToken))), cancellationToken);
#else
            return source.Provider.ExecuteAsync<long?>(Expression.Call(((MethodInfo)MethodBase.GetCurrentMethod()).MakeGenericMethod(typeof(TSource)), source.Expression, selector, Expression.Constant(cancellationToken, typeof(CancellationToken))), cancellationToken);
#endif
        }

        public static Task<long> Min<TSource>(this IAsyncQueryable<TSource> source, Expression<Func<TSource, long>> selector, CancellationToken cancellationToken)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));
            if (selector == null)
                throw new ArgumentNullException(nameof(selector));

#if CRIPPLED_REFLECTION
            return source.Provider.ExecuteAsync<long>(Expression.Call(InfoOf(() => AsyncQueryable.Min<TSource>(default(IAsyncQueryable<TSource>), default(Expression<Func<TSource, long>>), default(CancellationToken))), source.Expression, selector, Expression.Constant(cancellationToken, typeof(CancellationToken))), cancellationToken);
#else
            return source.Provider.ExecuteAsync<long>(Expression.Call(((MethodInfo)MethodBase.GetCurrentMethod()).MakeGenericMethod(typeof(TSource)), source.Expression, selector, Expression.Constant(cancellationToken, typeof(CancellationToken))), cancellationToken);
#endif
        }

        public static Task<decimal?> Min<TSource>(this IAsyncQueryable<TSource> source, Expression<Func<TSource, Task<decimal?>>> selector, CancellationToken cancellationToken)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));
            if (selector == null)
                throw new ArgumentNullException(nameof(selector));

#if CRIPPLED_REFLECTION
            return source.Provider.ExecuteAsync<decimal?>(Expression.Call(InfoOf(() => AsyncQueryable.Min<TSource>(default(IAsyncQueryable<TSource>), default(Expression<Func<TSource, Task<decimal?>>>), default(CancellationToken))), source.Expression, selector, Expression.Constant(cancellationToken, typeof(CancellationToken))), cancellationToken);
#else
            return source.Provider.ExecuteAsync<decimal?>(Expression.Call(((MethodInfo)MethodBase.GetCurrentMethod()).MakeGenericMethod(typeof(TSource)), source.Expression, selector, Expression.Constant(cancellationToken, typeof(CancellationToken))), cancellationToken);
#endif
        }

        public static Task<decimal> Min<TSource>(this IAsyncQueryable<TSource> source, Expression<Func<TSource, Task<decimal>>> selector, CancellationToken cancellationToken)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));
            if (selector == null)
                throw new ArgumentNullException(nameof(selector));

#if CRIPPLED_REFLECTION
            return source.Provider.ExecuteAsync<decimal>(Expression.Call(InfoOf(() => AsyncQueryable.Min<TSource>(default(IAsyncQueryable<TSource>), default(Expression<Func<TSource, Task<decimal>>>), default(CancellationToken))), source.Expression, selector, Expression.Constant(cancellationToken, typeof(CancellationToken))), cancellationToken);
#else
            return source.Provider.ExecuteAsync<decimal>(Expression.Call(((MethodInfo)MethodBase.GetCurrentMethod()).MakeGenericMethod(typeof(TSource)), source.Expression, selector, Expression.Constant(cancellationToken, typeof(CancellationToken))), cancellationToken);
#endif
        }

        public static Task<double?> Min<TSource>(this IAsyncQueryable<TSource> source, Expression<Func<TSource, Task<double?>>> selector, CancellationToken cancellationToken)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));
            if (selector == null)
                throw new ArgumentNullException(nameof(selector));

#if CRIPPLED_REFLECTION
            return source.Provider.ExecuteAsync<double?>(Expression.Call(InfoOf(() => AsyncQueryable.Min<TSource>(default(IAsyncQueryable<TSource>), default(Expression<Func<TSource, Task<double?>>>), default(CancellationToken))), source.Expression, selector, Expression.Constant(cancellationToken, typeof(CancellationToken))), cancellationToken);
#else
            return source.Provider.ExecuteAsync<double?>(Expression.Call(((MethodInfo)MethodBase.GetCurrentMethod()).MakeGenericMethod(typeof(TSource)), source.Expression, selector, Expression.Constant(cancellationToken, typeof(CancellationToken))), cancellationToken);
#endif
        }

        public static Task<double> Min<TSource>(this IAsyncQueryable<TSource> source, Expression<Func<TSource, Task<double>>> selector, CancellationToken cancellationToken)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));
            if (selector == null)
                throw new ArgumentNullException(nameof(selector));

#if CRIPPLED_REFLECTION
            return source.Provider.ExecuteAsync<double>(Expression.Call(InfoOf(() => AsyncQueryable.Min<TSource>(default(IAsyncQueryable<TSource>), default(Expression<Func<TSource, Task<double>>>), default(CancellationToken))), source.Expression, selector, Expression.Constant(cancellationToken, typeof(CancellationToken))), cancellationToken);
#else
            return source.Provider.ExecuteAsync<double>(Expression.Call(((MethodInfo)MethodBase.GetCurrentMethod()).MakeGenericMethod(typeof(TSource)), source.Expression, selector, Expression.Constant(cancellationToken, typeof(CancellationToken))), cancellationToken);
#endif
        }

        public static Task<float?> Min<TSource>(this IAsyncQueryable<TSource> source, Expression<Func<TSource, Task<float?>>> selector, CancellationToken cancellationToken)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));
            if (selector == null)
                throw new ArgumentNullException(nameof(selector));

#if CRIPPLED_REFLECTION
            return source.Provider.ExecuteAsync<float?>(Expression.Call(InfoOf(() => AsyncQueryable.Min<TSource>(default(IAsyncQueryable<TSource>), default(Expression<Func<TSource, Task<float?>>>), default(CancellationToken))), source.Expression, selector, Expression.Constant(cancellationToken, typeof(CancellationToken))), cancellationToken);
#else
            return source.Provider.ExecuteAsync<float?>(Expression.Call(((MethodInfo)MethodBase.GetCurrentMethod()).MakeGenericMethod(typeof(TSource)), source.Expression, selector, Expression.Constant(cancellationToken, typeof(CancellationToken))), cancellationToken);
#endif
        }

        public static Task<float> Min<TSource>(this IAsyncQueryable<TSource> source, Expression<Func<TSource, Task<float>>> selector, CancellationToken cancellationToken)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));
            if (selector == null)
                throw new ArgumentNullException(nameof(selector));

#if CRIPPLED_REFLECTION
            return source.Provider.ExecuteAsync<float>(Expression.Call(InfoOf(() => AsyncQueryable.Min<TSource>(default(IAsyncQueryable<TSource>), default(Expression<Func<TSource, Task<float>>>), default(CancellationToken))), source.Expression, selector, Expression.Constant(cancellationToken, typeof(CancellationToken))), cancellationToken);
#else
            return source.Provider.ExecuteAsync<float>(Expression.Call(((MethodInfo)MethodBase.GetCurrentMethod()).MakeGenericMethod(typeof(TSource)), source.Expression, selector, Expression.Constant(cancellationToken, typeof(CancellationToken))), cancellationToken);
#endif
        }

        public static Task<int?> Min<TSource>(this IAsyncQueryable<TSource> source, Expression<Func<TSource, Task<int?>>> selector, CancellationToken cancellationToken)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));
            if (selector == null)
                throw new ArgumentNullException(nameof(selector));

#if CRIPPLED_REFLECTION
            return source.Provider.ExecuteAsync<int?>(Expression.Call(InfoOf(() => AsyncQueryable.Min<TSource>(default(IAsyncQueryable<TSource>), default(Expression<Func<TSource, Task<int?>>>), default(CancellationToken))), source.Expression, selector, Expression.Constant(cancellationToken, typeof(CancellationToken))), cancellationToken);
#else
            return source.Provider.ExecuteAsync<int?>(Expression.Call(((MethodInfo)MethodBase.GetCurrentMethod()).MakeGenericMethod(typeof(TSource)), source.Expression, selector, Expression.Constant(cancellationToken, typeof(CancellationToken))), cancellationToken);
#endif
        }

        public static Task<int> Min<TSource>(this IAsyncQueryable<TSource> source, Expression<Func<TSource, Task<int>>> selector, CancellationToken cancellationToken)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));
            if (selector == null)
                throw new ArgumentNullException(nameof(selector));

#if CRIPPLED_REFLECTION
            return source.Provider.ExecuteAsync<int>(Expression.Call(InfoOf(() => AsyncQueryable.Min<TSource>(default(IAsyncQueryable<TSource>), default(Expression<Func<TSource, Task<int>>>), default(CancellationToken))), source.Expression, selector, Expression.Constant(cancellationToken, typeof(CancellationToken))), cancellationToken);
#else
            return source.Provider.ExecuteAsync<int>(Expression.Call(((MethodInfo)MethodBase.GetCurrentMethod()).MakeGenericMethod(typeof(TSource)), source.Expression, selector, Expression.Constant(cancellationToken, typeof(CancellationToken))), cancellationToken);
#endif
        }

        public static Task<long?> Min<TSource>(this IAsyncQueryable<TSource> source, Expression<Func<TSource, Task<long?>>> selector, CancellationToken cancellationToken)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));
            if (selector == null)
                throw new ArgumentNullException(nameof(selector));

#if CRIPPLED_REFLECTION
            return source.Provider.ExecuteAsync<long?>(Expression.Call(InfoOf(() => AsyncQueryable.Min<TSource>(default(IAsyncQueryable<TSource>), default(Expression<Func<TSource, Task<long?>>>), default(CancellationToken))), source.Expression, selector, Expression.Constant(cancellationToken, typeof(CancellationToken))), cancellationToken);
#else
            return source.Provider.ExecuteAsync<long?>(Expression.Call(((MethodInfo)MethodBase.GetCurrentMethod()).MakeGenericMethod(typeof(TSource)), source.Expression, selector, Expression.Constant(cancellationToken, typeof(CancellationToken))), cancellationToken);
#endif
        }

        public static Task<long> Min<TSource>(this IAsyncQueryable<TSource> source, Expression<Func<TSource, Task<long>>> selector, CancellationToken cancellationToken)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));
            if (selector == null)
                throw new ArgumentNullException(nameof(selector));

#if CRIPPLED_REFLECTION
            return source.Provider.ExecuteAsync<long>(Expression.Call(InfoOf(() => AsyncQueryable.Min<TSource>(default(IAsyncQueryable<TSource>), default(Expression<Func<TSource, Task<long>>>), default(CancellationToken))), source.Expression, selector, Expression.Constant(cancellationToken, typeof(CancellationToken))), cancellationToken);
#else
            return source.Provider.ExecuteAsync<long>(Expression.Call(((MethodInfo)MethodBase.GetCurrentMethod()).MakeGenericMethod(typeof(TSource)), source.Expression, selector, Expression.Constant(cancellationToken, typeof(CancellationToken))), cancellationToken);
#endif
        }

        public static Task<TResult> Min<TSource, TResult>(this IAsyncQueryable<TSource> source, Expression<Func<TSource, Task<TResult>>> selector)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));
            if (selector == null)
                throw new ArgumentNullException(nameof(selector));

#if CRIPPLED_REFLECTION
            return source.Provider.ExecuteAsync<TResult>(Expression.Call(InfoOf(() => AsyncQueryable.Min<TSource, TResult>(default(IAsyncQueryable<TSource>), default(Expression<Func<TSource, Task<TResult>>>))), source.Expression, selector), CancellationToken.None);
#else
            return source.Provider.ExecuteAsync<TResult>(Expression.Call(((MethodInfo)MethodBase.GetCurrentMethod()).MakeGenericMethod(typeof(TSource), typeof(TResult)), source.Expression, selector), CancellationToken.None);
#endif
        }

        public static Task<TResult> Min<TSource, TResult>(this IAsyncQueryable<TSource> source, Expression<Func<TSource, TResult>> selector)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));
            if (selector == null)
                throw new ArgumentNullException(nameof(selector));

#if CRIPPLED_REFLECTION
            return source.Provider.ExecuteAsync<TResult>(Expression.Call(InfoOf(() => AsyncQueryable.Min<TSource, TResult>(default(IAsyncQueryable<TSource>), default(Expression<Func<TSource, TResult>>))), source.Expression, selector), CancellationToken.None);
#else
            return source.Provider.ExecuteAsync<TResult>(Expression.Call(((MethodInfo)MethodBase.GetCurrentMethod()).MakeGenericMethod(typeof(TSource), typeof(TResult)), source.Expression, selector), CancellationToken.None);
#endif
        }

        public static Task<TResult> Min<TSource, TResult>(this IAsyncQueryable<TSource> source, Expression<Func<TSource, Task<TResult>>> selector, CancellationToken cancellationToken)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));
            if (selector == null)
                throw new ArgumentNullException(nameof(selector));

#if CRIPPLED_REFLECTION
            return source.Provider.ExecuteAsync<TResult>(Expression.Call(InfoOf(() => AsyncQueryable.Min<TSource, TResult>(default(IAsyncQueryable<TSource>), default(Expression<Func<TSource, Task<TResult>>>), default(CancellationToken))), source.Expression, selector, Expression.Constant(cancellationToken, typeof(CancellationToken))), cancellationToken);
#else
            return source.Provider.ExecuteAsync<TResult>(Expression.Call(((MethodInfo)MethodBase.GetCurrentMethod()).MakeGenericMethod(typeof(TSource), typeof(TResult)), source.Expression, selector, Expression.Constant(cancellationToken, typeof(CancellationToken))), cancellationToken);
#endif
        }

        public static Task<TResult> Min<TSource, TResult>(this IAsyncQueryable<TSource> source, Expression<Func<TSource, TResult>> selector, CancellationToken cancellationToken)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));
            if (selector == null)
                throw new ArgumentNullException(nameof(selector));

#if CRIPPLED_REFLECTION
            return source.Provider.ExecuteAsync<TResult>(Expression.Call(InfoOf(() => AsyncQueryable.Min<TSource, TResult>(default(IAsyncQueryable<TSource>), default(Expression<Func<TSource, TResult>>), default(CancellationToken))), source.Expression, selector, Expression.Constant(cancellationToken, typeof(CancellationToken))), cancellationToken);
#else
            return source.Provider.ExecuteAsync<TResult>(Expression.Call(((MethodInfo)MethodBase.GetCurrentMethod()).MakeGenericMethod(typeof(TSource), typeof(TResult)), source.Expression, selector, Expression.Constant(cancellationToken, typeof(CancellationToken))), cancellationToken);
#endif
        }

        public static IAsyncQueryable<TResult> OfType<TResult>(this IAsyncQueryable<object> source)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));

#if CRIPPLED_REFLECTION
            return source.Provider.CreateQuery<TResult>(Expression.Call(InfoOf(() => AsyncQueryable.OfType<TResult>(default(IAsyncQueryable<object>))), source.Expression));
#else
            return source.Provider.CreateQuery<TResult>(Expression.Call(((MethodInfo)MethodBase.GetCurrentMethod()).MakeGenericMethod(typeof(TResult)), source.Expression));
#endif
        }

        public static IOrderedAsyncQueryable<TSource> OrderBy<TSource, TKey>(this IAsyncQueryable<TSource> source, Expression<Func<TSource, Task<TKey>>> keySelector)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));
            if (keySelector == null)
                throw new ArgumentNullException(nameof(keySelector));

#if CRIPPLED_REFLECTION
            return (IOrderedAsyncQueryable<TSource>)source.Provider.CreateQuery<TSource>(Expression.Call(InfoOf(() => AsyncQueryable.OrderBy<TSource, TKey>(default(IAsyncQueryable<TSource>), default(Expression<Func<TSource, Task<TKey>>>))), source.Expression, keySelector));
#else
            return (IOrderedAsyncQueryable<TSource>)source.Provider.CreateQuery<TSource>(Expression.Call(((MethodInfo)MethodBase.GetCurrentMethod()).MakeGenericMethod(typeof(TSource), typeof(TKey)), source.Expression, keySelector));
#endif
        }

        public static IOrderedAsyncQueryable<TSource> OrderBy<TSource, TKey>(this IAsyncQueryable<TSource> source, Expression<Func<TSource, TKey>> keySelector)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));
            if (keySelector == null)
                throw new ArgumentNullException(nameof(keySelector));

#if CRIPPLED_REFLECTION
            return (IOrderedAsyncQueryable<TSource>)source.Provider.CreateQuery<TSource>(Expression.Call(InfoOf(() => AsyncQueryable.OrderBy<TSource, TKey>(default(IAsyncQueryable<TSource>), default(Expression<Func<TSource, TKey>>))), source.Expression, keySelector));
#else
            return (IOrderedAsyncQueryable<TSource>)source.Provider.CreateQuery<TSource>(Expression.Call(((MethodInfo)MethodBase.GetCurrentMethod()).MakeGenericMethod(typeof(TSource), typeof(TKey)), source.Expression, keySelector));
#endif
        }

        public static IOrderedAsyncQueryable<TSource> OrderBy<TSource, TKey>(this IAsyncQueryable<TSource> source, Expression<Func<TSource, Task<TKey>>> keySelector, IComparer<TKey> comparer)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));
            if (keySelector == null)
                throw new ArgumentNullException(nameof(keySelector));
            if (comparer == null)
                throw new ArgumentNullException(nameof(comparer));

#if CRIPPLED_REFLECTION
            return (IOrderedAsyncQueryable<TSource>)source.Provider.CreateQuery<TSource>(Expression.Call(InfoOf(() => AsyncQueryable.OrderBy<TSource, TKey>(default(IAsyncQueryable<TSource>), default(Expression<Func<TSource, Task<TKey>>>), default(IComparer<TKey>))), source.Expression, keySelector, Expression.Constant(comparer, typeof(IComparer<TKey>))));
#else
            return (IOrderedAsyncQueryable<TSource>)source.Provider.CreateQuery<TSource>(Expression.Call(((MethodInfo)MethodBase.GetCurrentMethod()).MakeGenericMethod(typeof(TSource), typeof(TKey)), source.Expression, keySelector, Expression.Constant(comparer, typeof(IComparer<TKey>))));
#endif
        }

        public static IOrderedAsyncQueryable<TSource> OrderBy<TSource, TKey>(this IAsyncQueryable<TSource> source, Expression<Func<TSource, TKey>> keySelector, IComparer<TKey> comparer)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));
            if (keySelector == null)
                throw new ArgumentNullException(nameof(keySelector));
            if (comparer == null)
                throw new ArgumentNullException(nameof(comparer));

#if CRIPPLED_REFLECTION
            return (IOrderedAsyncQueryable<TSource>)source.Provider.CreateQuery<TSource>(Expression.Call(InfoOf(() => AsyncQueryable.OrderBy<TSource, TKey>(default(IAsyncQueryable<TSource>), default(Expression<Func<TSource, TKey>>), default(IComparer<TKey>))), source.Expression, keySelector, Expression.Constant(comparer, typeof(IComparer<TKey>))));
#else
            return (IOrderedAsyncQueryable<TSource>)source.Provider.CreateQuery<TSource>(Expression.Call(((MethodInfo)MethodBase.GetCurrentMethod()).MakeGenericMethod(typeof(TSource), typeof(TKey)), source.Expression, keySelector, Expression.Constant(comparer, typeof(IComparer<TKey>))));
#endif
        }

        public static IOrderedAsyncQueryable<TSource> OrderByDescending<TSource, TKey>(this IAsyncQueryable<TSource> source, Expression<Func<TSource, Task<TKey>>> keySelector)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));
            if (keySelector == null)
                throw new ArgumentNullException(nameof(keySelector));

#if CRIPPLED_REFLECTION
            return (IOrderedAsyncQueryable<TSource>)source.Provider.CreateQuery<TSource>(Expression.Call(InfoOf(() => AsyncQueryable.OrderByDescending<TSource, TKey>(default(IAsyncQueryable<TSource>), default(Expression<Func<TSource, Task<TKey>>>))), source.Expression, keySelector));
#else
            return (IOrderedAsyncQueryable<TSource>)source.Provider.CreateQuery<TSource>(Expression.Call(((MethodInfo)MethodBase.GetCurrentMethod()).MakeGenericMethod(typeof(TSource), typeof(TKey)), source.Expression, keySelector));
#endif
        }

        public static IOrderedAsyncQueryable<TSource> OrderByDescending<TSource, TKey>(this IAsyncQueryable<TSource> source, Expression<Func<TSource, TKey>> keySelector)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));
            if (keySelector == null)
                throw new ArgumentNullException(nameof(keySelector));

#if CRIPPLED_REFLECTION
            return (IOrderedAsyncQueryable<TSource>)source.Provider.CreateQuery<TSource>(Expression.Call(InfoOf(() => AsyncQueryable.OrderByDescending<TSource, TKey>(default(IAsyncQueryable<TSource>), default(Expression<Func<TSource, TKey>>))), source.Expression, keySelector));
#else
            return (IOrderedAsyncQueryable<TSource>)source.Provider.CreateQuery<TSource>(Expression.Call(((MethodInfo)MethodBase.GetCurrentMethod()).MakeGenericMethod(typeof(TSource), typeof(TKey)), source.Expression, keySelector));
#endif
        }

        public static IOrderedAsyncQueryable<TSource> OrderByDescending<TSource, TKey>(this IAsyncQueryable<TSource> source, Expression<Func<TSource, Task<TKey>>> keySelector, IComparer<TKey> comparer)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));
            if (keySelector == null)
                throw new ArgumentNullException(nameof(keySelector));
            if (comparer == null)
                throw new ArgumentNullException(nameof(comparer));

#if CRIPPLED_REFLECTION
            return (IOrderedAsyncQueryable<TSource>)source.Provider.CreateQuery<TSource>(Expression.Call(InfoOf(() => AsyncQueryable.OrderByDescending<TSource, TKey>(default(IAsyncQueryable<TSource>), default(Expression<Func<TSource, Task<TKey>>>), default(IComparer<TKey>))), source.Expression, keySelector, Expression.Constant(comparer, typeof(IComparer<TKey>))));
#else
            return (IOrderedAsyncQueryable<TSource>)source.Provider.CreateQuery<TSource>(Expression.Call(((MethodInfo)MethodBase.GetCurrentMethod()).MakeGenericMethod(typeof(TSource), typeof(TKey)), source.Expression, keySelector, Expression.Constant(comparer, typeof(IComparer<TKey>))));
#endif
        }

        public static IOrderedAsyncQueryable<TSource> OrderByDescending<TSource, TKey>(this IAsyncQueryable<TSource> source, Expression<Func<TSource, TKey>> keySelector, IComparer<TKey> comparer)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));
            if (keySelector == null)
                throw new ArgumentNullException(nameof(keySelector));
            if (comparer == null)
                throw new ArgumentNullException(nameof(comparer));

#if CRIPPLED_REFLECTION
            return (IOrderedAsyncQueryable<TSource>)source.Provider.CreateQuery<TSource>(Expression.Call(InfoOf(() => AsyncQueryable.OrderByDescending<TSource, TKey>(default(IAsyncQueryable<TSource>), default(Expression<Func<TSource, TKey>>), default(IComparer<TKey>))), source.Expression, keySelector, Expression.Constant(comparer, typeof(IComparer<TKey>))));
#else
            return (IOrderedAsyncQueryable<TSource>)source.Provider.CreateQuery<TSource>(Expression.Call(((MethodInfo)MethodBase.GetCurrentMethod()).MakeGenericMethod(typeof(TSource), typeof(TKey)), source.Expression, keySelector, Expression.Constant(comparer, typeof(IComparer<TKey>))));
#endif
        }

        public static IAsyncQueryable<TSource> Prepend<TSource>(this IAsyncQueryable<TSource> source, TSource element)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));

#if CRIPPLED_REFLECTION
            return source.Provider.CreateQuery<TSource>(Expression.Call(InfoOf(() => AsyncQueryable.Prepend<TSource>(default(IAsyncQueryable<TSource>), default(TSource))), source.Expression, Expression.Constant(element, typeof(TSource))));
#else
            return source.Provider.CreateQuery<TSource>(Expression.Call(((MethodInfo)MethodBase.GetCurrentMethod()).MakeGenericMethod(typeof(TSource)), source.Expression, Expression.Constant(element, typeof(TSource))));
#endif
        }

        public static IAsyncQueryable<TSource> Reverse<TSource>(this IAsyncQueryable<TSource> source)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));

#if CRIPPLED_REFLECTION
            return source.Provider.CreateQuery<TSource>(Expression.Call(InfoOf(() => AsyncQueryable.Reverse<TSource>(default(IAsyncQueryable<TSource>))), source.Expression));
#else
            return source.Provider.CreateQuery<TSource>(Expression.Call(((MethodInfo)MethodBase.GetCurrentMethod()).MakeGenericMethod(typeof(TSource)), source.Expression));
#endif
        }

        public static IAsyncQueryable<TResult> Select<TSource, TResult>(this IAsyncQueryable<TSource> source, Expression<Func<TSource, int, Task<TResult>>> selector)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));
            if (selector == null)
                throw new ArgumentNullException(nameof(selector));

#if CRIPPLED_REFLECTION
            return source.Provider.CreateQuery<TResult>(Expression.Call(InfoOf(() => AsyncQueryable.Select<TSource, TResult>(default(IAsyncQueryable<TSource>), default(Expression<Func<TSource, int, Task<TResult>>>))), source.Expression, selector));
#else
            return source.Provider.CreateQuery<TResult>(Expression.Call(((MethodInfo)MethodBase.GetCurrentMethod()).MakeGenericMethod(typeof(TSource), typeof(TResult)), source.Expression, selector));
#endif
        }

        public static IAsyncQueryable<TResult> Select<TSource, TResult>(this IAsyncQueryable<TSource> source, Expression<Func<TSource, int, TResult>> selector)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));
            if (selector == null)
                throw new ArgumentNullException(nameof(selector));

#if CRIPPLED_REFLECTION
            return source.Provider.CreateQuery<TResult>(Expression.Call(InfoOf(() => AsyncQueryable.Select<TSource, TResult>(default(IAsyncQueryable<TSource>), default(Expression<Func<TSource, int, TResult>>))), source.Expression, selector));
#else
            return source.Provider.CreateQuery<TResult>(Expression.Call(((MethodInfo)MethodBase.GetCurrentMethod()).MakeGenericMethod(typeof(TSource), typeof(TResult)), source.Expression, selector));
#endif
        }

        public static IAsyncQueryable<TResult> Select<TSource, TResult>(this IAsyncQueryable<TSource> source, Expression<Func<TSource, Task<TResult>>> selector)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));
            if (selector == null)
                throw new ArgumentNullException(nameof(selector));

#if CRIPPLED_REFLECTION
            return source.Provider.CreateQuery<TResult>(Expression.Call(InfoOf(() => AsyncQueryable.Select<TSource, TResult>(default(IAsyncQueryable<TSource>), default(Expression<Func<TSource, Task<TResult>>>))), source.Expression, selector));
#else
            return source.Provider.CreateQuery<TResult>(Expression.Call(((MethodInfo)MethodBase.GetCurrentMethod()).MakeGenericMethod(typeof(TSource), typeof(TResult)), source.Expression, selector));
#endif
        }

        public static IAsyncQueryable<TResult> Select<TSource, TResult>(this IAsyncQueryable<TSource> source, Expression<Func<TSource, TResult>> selector)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));
            if (selector == null)
                throw new ArgumentNullException(nameof(selector));

#if CRIPPLED_REFLECTION
            return source.Provider.CreateQuery<TResult>(Expression.Call(InfoOf(() => AsyncQueryable.Select<TSource, TResult>(default(IAsyncQueryable<TSource>), default(Expression<Func<TSource, TResult>>))), source.Expression, selector));
#else
            return source.Provider.CreateQuery<TResult>(Expression.Call(((MethodInfo)MethodBase.GetCurrentMethod()).MakeGenericMethod(typeof(TSource), typeof(TResult)), source.Expression, selector));
#endif
        }

        public static IAsyncQueryable<TResult> SelectMany<TSource, TResult>(this IAsyncQueryable<TSource> source, Expression<Func<TSource, IAsyncEnumerable<TResult>>> selector)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));
            if (selector == null)
                throw new ArgumentNullException(nameof(selector));

#if CRIPPLED_REFLECTION
            return source.Provider.CreateQuery<TResult>(Expression.Call(InfoOf(() => AsyncQueryable.SelectMany<TSource, TResult>(default(IAsyncQueryable<TSource>), default(Expression<Func<TSource, IAsyncEnumerable<TResult>>>))), source.Expression, selector));
#else
            return source.Provider.CreateQuery<TResult>(Expression.Call(((MethodInfo)MethodBase.GetCurrentMethod()).MakeGenericMethod(typeof(TSource), typeof(TResult)), source.Expression, selector));
#endif
        }

        public static IAsyncQueryable<TResult> SelectMany<TSource, TResult>(this IAsyncQueryable<TSource> source, Expression<Func<TSource, int, IAsyncEnumerable<TResult>>> selector)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));
            if (selector == null)
                throw new ArgumentNullException(nameof(selector));

#if CRIPPLED_REFLECTION
            return source.Provider.CreateQuery<TResult>(Expression.Call(InfoOf(() => AsyncQueryable.SelectMany<TSource, TResult>(default(IAsyncQueryable<TSource>), default(Expression<Func<TSource, int, IAsyncEnumerable<TResult>>>))), source.Expression, selector));
#else
            return source.Provider.CreateQuery<TResult>(Expression.Call(((MethodInfo)MethodBase.GetCurrentMethod()).MakeGenericMethod(typeof(TSource), typeof(TResult)), source.Expression, selector));
#endif
        }

        public static IAsyncQueryable<TResult> SelectMany<TSource, TResult>(this IAsyncQueryable<TSource> source, Expression<Func<TSource, int, Task<IAsyncEnumerable<TResult>>>> selector)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));
            if (selector == null)
                throw new ArgumentNullException(nameof(selector));

#if CRIPPLED_REFLECTION
            return source.Provider.CreateQuery<TResult>(Expression.Call(InfoOf(() => AsyncQueryable.SelectMany<TSource, TResult>(default(IAsyncQueryable<TSource>), default(Expression<Func<TSource, int, Task<IAsyncEnumerable<TResult>>>>))), source.Expression, selector));
#else
            return source.Provider.CreateQuery<TResult>(Expression.Call(((MethodInfo)MethodBase.GetCurrentMethod()).MakeGenericMethod(typeof(TSource), typeof(TResult)), source.Expression, selector));
#endif
        }

        public static IAsyncQueryable<TResult> SelectMany<TSource, TResult>(this IAsyncQueryable<TSource> source, Expression<Func<TSource, Task<IAsyncEnumerable<TResult>>>> selector)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));
            if (selector == null)
                throw new ArgumentNullException(nameof(selector));

#if CRIPPLED_REFLECTION
            return source.Provider.CreateQuery<TResult>(Expression.Call(InfoOf(() => AsyncQueryable.SelectMany<TSource, TResult>(default(IAsyncQueryable<TSource>), default(Expression<Func<TSource, Task<IAsyncEnumerable<TResult>>>>))), source.Expression, selector));
#else
            return source.Provider.CreateQuery<TResult>(Expression.Call(((MethodInfo)MethodBase.GetCurrentMethod()).MakeGenericMethod(typeof(TSource), typeof(TResult)), source.Expression, selector));
#endif
        }

        public static IAsyncQueryable<TResult> SelectMany<TSource, TCollection, TResult>(this IAsyncQueryable<TSource> source, Expression<Func<TSource, IAsyncEnumerable<TCollection>>> selector, Expression<Func<TSource, TCollection, TResult>> resultSelector)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));
            if (selector == null)
                throw new ArgumentNullException(nameof(selector));
            if (resultSelector == null)
                throw new ArgumentNullException(nameof(resultSelector));

#if CRIPPLED_REFLECTION
            return source.Provider.CreateQuery<TResult>(Expression.Call(InfoOf(() => AsyncQueryable.SelectMany<TSource, TCollection, TResult>(default(IAsyncQueryable<TSource>), default(Expression<Func<TSource, IAsyncEnumerable<TCollection>>>), default(Expression<Func<TSource, TCollection, TResult>>))), source.Expression, selector, resultSelector));
#else
            return source.Provider.CreateQuery<TResult>(Expression.Call(((MethodInfo)MethodBase.GetCurrentMethod()).MakeGenericMethod(typeof(TSource), typeof(TCollection), typeof(TResult)), source.Expression, selector, resultSelector));
#endif
        }

        public static IAsyncQueryable<TResult> SelectMany<TSource, TCollection, TResult>(this IAsyncQueryable<TSource> source, Expression<Func<TSource, int, IAsyncEnumerable<TCollection>>> selector, Expression<Func<TSource, TCollection, TResult>> resultSelector)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));
            if (selector == null)
                throw new ArgumentNullException(nameof(selector));
            if (resultSelector == null)
                throw new ArgumentNullException(nameof(resultSelector));

#if CRIPPLED_REFLECTION
            return source.Provider.CreateQuery<TResult>(Expression.Call(InfoOf(() => AsyncQueryable.SelectMany<TSource, TCollection, TResult>(default(IAsyncQueryable<TSource>), default(Expression<Func<TSource, int, IAsyncEnumerable<TCollection>>>), default(Expression<Func<TSource, TCollection, TResult>>))), source.Expression, selector, resultSelector));
#else
            return source.Provider.CreateQuery<TResult>(Expression.Call(((MethodInfo)MethodBase.GetCurrentMethod()).MakeGenericMethod(typeof(TSource), typeof(TCollection), typeof(TResult)), source.Expression, selector, resultSelector));
#endif
        }

        public static IAsyncQueryable<TResult> SelectMany<TSource, TCollection, TResult>(this IAsyncQueryable<TSource> source, Expression<Func<TSource, int, Task<IAsyncEnumerable<TCollection>>>> selector, Expression<Func<TSource, TCollection, Task<TResult>>> resultSelector)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));
            if (selector == null)
                throw new ArgumentNullException(nameof(selector));
            if (resultSelector == null)
                throw new ArgumentNullException(nameof(resultSelector));

#if CRIPPLED_REFLECTION
            return source.Provider.CreateQuery<TResult>(Expression.Call(InfoOf(() => AsyncQueryable.SelectMany<TSource, TCollection, TResult>(default(IAsyncQueryable<TSource>), default(Expression<Func<TSource, int, Task<IAsyncEnumerable<TCollection>>>>), default(Expression<Func<TSource, TCollection, Task<TResult>>>))), source.Expression, selector, resultSelector));
#else
            return source.Provider.CreateQuery<TResult>(Expression.Call(((MethodInfo)MethodBase.GetCurrentMethod()).MakeGenericMethod(typeof(TSource), typeof(TCollection), typeof(TResult)), source.Expression, selector, resultSelector));
#endif
        }

        public static IAsyncQueryable<TResult> SelectMany<TSource, TCollection, TResult>(this IAsyncQueryable<TSource> source, Expression<Func<TSource, Task<IAsyncEnumerable<TCollection>>>> selector, Expression<Func<TSource, TCollection, Task<TResult>>> resultSelector)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));
            if (selector == null)
                throw new ArgumentNullException(nameof(selector));
            if (resultSelector == null)
                throw new ArgumentNullException(nameof(resultSelector));

#if CRIPPLED_REFLECTION
            return source.Provider.CreateQuery<TResult>(Expression.Call(InfoOf(() => AsyncQueryable.SelectMany<TSource, TCollection, TResult>(default(IAsyncQueryable<TSource>), default(Expression<Func<TSource, Task<IAsyncEnumerable<TCollection>>>>), default(Expression<Func<TSource, TCollection, Task<TResult>>>))), source.Expression, selector, resultSelector));
#else
            return source.Provider.CreateQuery<TResult>(Expression.Call(((MethodInfo)MethodBase.GetCurrentMethod()).MakeGenericMethod(typeof(TSource), typeof(TCollection), typeof(TResult)), source.Expression, selector, resultSelector));
#endif
        }

        public static Task<bool> SequenceEqual<TSource>(this IAsyncQueryable<TSource> first, IAsyncEnumerable<TSource> second)
        {
            if (first == null)
                throw new ArgumentNullException(nameof(first));
            if (second == null)
                throw new ArgumentNullException(nameof(second));

#if CRIPPLED_REFLECTION
            return first.Provider.ExecuteAsync<bool>(Expression.Call(InfoOf(() => AsyncQueryable.SequenceEqual<TSource>(default(IAsyncQueryable<TSource>), default(IAsyncEnumerable<TSource>))), first.Expression, GetSourceExpression(second)), CancellationToken.None);
#else
            return first.Provider.ExecuteAsync<bool>(Expression.Call(((MethodInfo)MethodBase.GetCurrentMethod()).MakeGenericMethod(typeof(TSource)), first.Expression, GetSourceExpression(second)), CancellationToken.None);
#endif
        }

        public static Task<bool> SequenceEqual<TSource>(this IAsyncQueryable<TSource> first, IAsyncEnumerable<TSource> second, CancellationToken cancellationToken)
        {
            if (first == null)
                throw new ArgumentNullException(nameof(first));
            if (second == null)
                throw new ArgumentNullException(nameof(second));

#if CRIPPLED_REFLECTION
            return first.Provider.ExecuteAsync<bool>(Expression.Call(InfoOf(() => AsyncQueryable.SequenceEqual<TSource>(default(IAsyncQueryable<TSource>), default(IAsyncEnumerable<TSource>), default(CancellationToken))), first.Expression, GetSourceExpression(second), Expression.Constant(cancellationToken, typeof(CancellationToken))), cancellationToken);
#else
            return first.Provider.ExecuteAsync<bool>(Expression.Call(((MethodInfo)MethodBase.GetCurrentMethod()).MakeGenericMethod(typeof(TSource)), first.Expression, GetSourceExpression(second), Expression.Constant(cancellationToken, typeof(CancellationToken))), cancellationToken);
#endif
        }

        public static Task<bool> SequenceEqual<TSource>(this IAsyncQueryable<TSource> first, IAsyncEnumerable<TSource> second, IEqualityComparer<TSource> comparer)
        {
            if (first == null)
                throw new ArgumentNullException(nameof(first));
            if (second == null)
                throw new ArgumentNullException(nameof(second));
            if (comparer == null)
                throw new ArgumentNullException(nameof(comparer));

#if CRIPPLED_REFLECTION
            return first.Provider.ExecuteAsync<bool>(Expression.Call(InfoOf(() => AsyncQueryable.SequenceEqual<TSource>(default(IAsyncQueryable<TSource>), default(IAsyncEnumerable<TSource>), default(IEqualityComparer<TSource>))), first.Expression, GetSourceExpression(second), Expression.Constant(comparer, typeof(IEqualityComparer<TSource>))), CancellationToken.None);
#else
            return first.Provider.ExecuteAsync<bool>(Expression.Call(((MethodInfo)MethodBase.GetCurrentMethod()).MakeGenericMethod(typeof(TSource)), first.Expression, GetSourceExpression(second), Expression.Constant(comparer, typeof(IEqualityComparer<TSource>))), CancellationToken.None);
#endif
        }

        public static Task<bool> SequenceEqual<TSource>(this IAsyncQueryable<TSource> first, IAsyncEnumerable<TSource> second, IEqualityComparer<TSource> comparer, CancellationToken cancellationToken)
        {
            if (first == null)
                throw new ArgumentNullException(nameof(first));
            if (second == null)
                throw new ArgumentNullException(nameof(second));
            if (comparer == null)
                throw new ArgumentNullException(nameof(comparer));

#if CRIPPLED_REFLECTION
            return first.Provider.ExecuteAsync<bool>(Expression.Call(InfoOf(() => AsyncQueryable.SequenceEqual<TSource>(default(IAsyncQueryable<TSource>), default(IAsyncEnumerable<TSource>), default(IEqualityComparer<TSource>), default(CancellationToken))), first.Expression, GetSourceExpression(second), Expression.Constant(comparer, typeof(IEqualityComparer<TSource>)), Expression.Constant(cancellationToken, typeof(CancellationToken))), cancellationToken);
#else
            return first.Provider.ExecuteAsync<bool>(Expression.Call(((MethodInfo)MethodBase.GetCurrentMethod()).MakeGenericMethod(typeof(TSource)), first.Expression, GetSourceExpression(second), Expression.Constant(comparer, typeof(IEqualityComparer<TSource>)), Expression.Constant(cancellationToken, typeof(CancellationToken))), cancellationToken);
#endif
        }

        public static Task<TSource> Single<TSource>(this IAsyncQueryable<TSource> source)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));

#if CRIPPLED_REFLECTION
            return source.Provider.ExecuteAsync<TSource>(Expression.Call(InfoOf(() => AsyncQueryable.Single<TSource>(default(IAsyncQueryable<TSource>))), source.Expression), CancellationToken.None);
#else
            return source.Provider.ExecuteAsync<TSource>(Expression.Call(((MethodInfo)MethodBase.GetCurrentMethod()).MakeGenericMethod(typeof(TSource)), source.Expression), CancellationToken.None);
#endif
        }

        public static Task<TSource> Single<TSource>(this IAsyncQueryable<TSource> source, CancellationToken cancellationToken)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));

#if CRIPPLED_REFLECTION
            return source.Provider.ExecuteAsync<TSource>(Expression.Call(InfoOf(() => AsyncQueryable.Single<TSource>(default(IAsyncQueryable<TSource>), default(CancellationToken))), source.Expression, Expression.Constant(cancellationToken, typeof(CancellationToken))), cancellationToken);
#else
            return source.Provider.ExecuteAsync<TSource>(Expression.Call(((MethodInfo)MethodBase.GetCurrentMethod()).MakeGenericMethod(typeof(TSource)), source.Expression, Expression.Constant(cancellationToken, typeof(CancellationToken))), cancellationToken);
#endif
        }

        public static Task<TSource> Single<TSource>(this IAsyncQueryable<TSource> source, Expression<Func<TSource, bool>> predicate)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));
            if (predicate == null)
                throw new ArgumentNullException(nameof(predicate));

#if CRIPPLED_REFLECTION
            return source.Provider.ExecuteAsync<TSource>(Expression.Call(InfoOf(() => AsyncQueryable.Single<TSource>(default(IAsyncQueryable<TSource>), default(Expression<Func<TSource, bool>>))), source.Expression, predicate), CancellationToken.None);
#else
            return source.Provider.ExecuteAsync<TSource>(Expression.Call(((MethodInfo)MethodBase.GetCurrentMethod()).MakeGenericMethod(typeof(TSource)), source.Expression, predicate), CancellationToken.None);
#endif
        }

        public static Task<TSource> Single<TSource>(this IAsyncQueryable<TSource> source, Expression<Func<TSource, Task<bool>>> predicate)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));
            if (predicate == null)
                throw new ArgumentNullException(nameof(predicate));

#if CRIPPLED_REFLECTION
            return source.Provider.ExecuteAsync<TSource>(Expression.Call(InfoOf(() => AsyncQueryable.Single<TSource>(default(IAsyncQueryable<TSource>), default(Expression<Func<TSource, Task<bool>>>))), source.Expression, predicate), CancellationToken.None);
#else
            return source.Provider.ExecuteAsync<TSource>(Expression.Call(((MethodInfo)MethodBase.GetCurrentMethod()).MakeGenericMethod(typeof(TSource)), source.Expression, predicate), CancellationToken.None);
#endif
        }

        public static Task<TSource> Single<TSource>(this IAsyncQueryable<TSource> source, Expression<Func<TSource, bool>> predicate, CancellationToken cancellationToken)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));
            if (predicate == null)
                throw new ArgumentNullException(nameof(predicate));

#if CRIPPLED_REFLECTION
            return source.Provider.ExecuteAsync<TSource>(Expression.Call(InfoOf(() => AsyncQueryable.Single<TSource>(default(IAsyncQueryable<TSource>), default(Expression<Func<TSource, bool>>), default(CancellationToken))), source.Expression, predicate, Expression.Constant(cancellationToken, typeof(CancellationToken))), cancellationToken);
#else
            return source.Provider.ExecuteAsync<TSource>(Expression.Call(((MethodInfo)MethodBase.GetCurrentMethod()).MakeGenericMethod(typeof(TSource)), source.Expression, predicate, Expression.Constant(cancellationToken, typeof(CancellationToken))), cancellationToken);
#endif
        }

        public static Task<TSource> Single<TSource>(this IAsyncQueryable<TSource> source, Expression<Func<TSource, Task<bool>>> predicate, CancellationToken cancellationToken)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));
            if (predicate == null)
                throw new ArgumentNullException(nameof(predicate));

#if CRIPPLED_REFLECTION
            return source.Provider.ExecuteAsync<TSource>(Expression.Call(InfoOf(() => AsyncQueryable.Single<TSource>(default(IAsyncQueryable<TSource>), default(Expression<Func<TSource, Task<bool>>>), default(CancellationToken))), source.Expression, predicate, Expression.Constant(cancellationToken, typeof(CancellationToken))), cancellationToken);
#else
            return source.Provider.ExecuteAsync<TSource>(Expression.Call(((MethodInfo)MethodBase.GetCurrentMethod()).MakeGenericMethod(typeof(TSource)), source.Expression, predicate, Expression.Constant(cancellationToken, typeof(CancellationToken))), cancellationToken);
#endif
        }

        public static Task<TSource> SingleOrDefault<TSource>(this IAsyncQueryable<TSource> source)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));

#if CRIPPLED_REFLECTION
            return source.Provider.ExecuteAsync<TSource>(Expression.Call(InfoOf(() => AsyncQueryable.SingleOrDefault<TSource>(default(IAsyncQueryable<TSource>))), source.Expression), CancellationToken.None);
#else
            return source.Provider.ExecuteAsync<TSource>(Expression.Call(((MethodInfo)MethodBase.GetCurrentMethod()).MakeGenericMethod(typeof(TSource)), source.Expression), CancellationToken.None);
#endif
        }

        public static Task<TSource> SingleOrDefault<TSource>(this IAsyncQueryable<TSource> source, CancellationToken cancellationToken)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));

#if CRIPPLED_REFLECTION
            return source.Provider.ExecuteAsync<TSource>(Expression.Call(InfoOf(() => AsyncQueryable.SingleOrDefault<TSource>(default(IAsyncQueryable<TSource>), default(CancellationToken))), source.Expression, Expression.Constant(cancellationToken, typeof(CancellationToken))), cancellationToken);
#else
            return source.Provider.ExecuteAsync<TSource>(Expression.Call(((MethodInfo)MethodBase.GetCurrentMethod()).MakeGenericMethod(typeof(TSource)), source.Expression, Expression.Constant(cancellationToken, typeof(CancellationToken))), cancellationToken);
#endif
        }

        public static Task<TSource> SingleOrDefault<TSource>(this IAsyncQueryable<TSource> source, Expression<Func<TSource, bool>> predicate)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));
            if (predicate == null)
                throw new ArgumentNullException(nameof(predicate));

#if CRIPPLED_REFLECTION
            return source.Provider.ExecuteAsync<TSource>(Expression.Call(InfoOf(() => AsyncQueryable.SingleOrDefault<TSource>(default(IAsyncQueryable<TSource>), default(Expression<Func<TSource, bool>>))), source.Expression, predicate), CancellationToken.None);
#else
            return source.Provider.ExecuteAsync<TSource>(Expression.Call(((MethodInfo)MethodBase.GetCurrentMethod()).MakeGenericMethod(typeof(TSource)), source.Expression, predicate), CancellationToken.None);
#endif
        }

        public static Task<TSource> SingleOrDefault<TSource>(this IAsyncQueryable<TSource> source, Expression<Func<TSource, Task<bool>>> predicate)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));
            if (predicate == null)
                throw new ArgumentNullException(nameof(predicate));

#if CRIPPLED_REFLECTION
            return source.Provider.ExecuteAsync<TSource>(Expression.Call(InfoOf(() => AsyncQueryable.SingleOrDefault<TSource>(default(IAsyncQueryable<TSource>), default(Expression<Func<TSource, Task<bool>>>))), source.Expression, predicate), CancellationToken.None);
#else
            return source.Provider.ExecuteAsync<TSource>(Expression.Call(((MethodInfo)MethodBase.GetCurrentMethod()).MakeGenericMethod(typeof(TSource)), source.Expression, predicate), CancellationToken.None);
#endif
        }

        public static Task<TSource> SingleOrDefault<TSource>(this IAsyncQueryable<TSource> source, Expression<Func<TSource, bool>> predicate, CancellationToken cancellationToken)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));
            if (predicate == null)
                throw new ArgumentNullException(nameof(predicate));

#if CRIPPLED_REFLECTION
            return source.Provider.ExecuteAsync<TSource>(Expression.Call(InfoOf(() => AsyncQueryable.SingleOrDefault<TSource>(default(IAsyncQueryable<TSource>), default(Expression<Func<TSource, bool>>), default(CancellationToken))), source.Expression, predicate, Expression.Constant(cancellationToken, typeof(CancellationToken))), cancellationToken);
#else
            return source.Provider.ExecuteAsync<TSource>(Expression.Call(((MethodInfo)MethodBase.GetCurrentMethod()).MakeGenericMethod(typeof(TSource)), source.Expression, predicate, Expression.Constant(cancellationToken, typeof(CancellationToken))), cancellationToken);
#endif
        }

        public static Task<TSource> SingleOrDefault<TSource>(this IAsyncQueryable<TSource> source, Expression<Func<TSource, Task<bool>>> predicate, CancellationToken cancellationToken)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));
            if (predicate == null)
                throw new ArgumentNullException(nameof(predicate));

#if CRIPPLED_REFLECTION
            return source.Provider.ExecuteAsync<TSource>(Expression.Call(InfoOf(() => AsyncQueryable.SingleOrDefault<TSource>(default(IAsyncQueryable<TSource>), default(Expression<Func<TSource, Task<bool>>>), default(CancellationToken))), source.Expression, predicate, Expression.Constant(cancellationToken, typeof(CancellationToken))), cancellationToken);
#else
            return source.Provider.ExecuteAsync<TSource>(Expression.Call(((MethodInfo)MethodBase.GetCurrentMethod()).MakeGenericMethod(typeof(TSource)), source.Expression, predicate, Expression.Constant(cancellationToken, typeof(CancellationToken))), cancellationToken);
#endif
        }

        public static IAsyncQueryable<TSource> Skip<TSource>(this IAsyncQueryable<TSource> source, int count)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));

#if CRIPPLED_REFLECTION
            return source.Provider.CreateQuery<TSource>(Expression.Call(InfoOf(() => AsyncQueryable.Skip<TSource>(default(IAsyncQueryable<TSource>), default(int))), source.Expression, Expression.Constant(count, typeof(int))));
#else
            return source.Provider.CreateQuery<TSource>(Expression.Call(((MethodInfo)MethodBase.GetCurrentMethod()).MakeGenericMethod(typeof(TSource)), source.Expression, Expression.Constant(count, typeof(int))));
#endif
        }

        public static IAsyncQueryable<TSource> SkipLast<TSource>(this IAsyncQueryable<TSource> source, int count)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));

#if CRIPPLED_REFLECTION
            return source.Provider.CreateQuery<TSource>(Expression.Call(InfoOf(() => AsyncQueryable.SkipLast<TSource>(default(IAsyncQueryable<TSource>), default(int))), source.Expression, Expression.Constant(count, typeof(int))));
#else
            return source.Provider.CreateQuery<TSource>(Expression.Call(((MethodInfo)MethodBase.GetCurrentMethod()).MakeGenericMethod(typeof(TSource)), source.Expression, Expression.Constant(count, typeof(int))));
#endif
        }

        public static IAsyncQueryable<TSource> SkipWhile<TSource>(this IAsyncQueryable<TSource> source, Expression<Func<TSource, bool>> predicate)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));
            if (predicate == null)
                throw new ArgumentNullException(nameof(predicate));

#if CRIPPLED_REFLECTION
            return source.Provider.CreateQuery<TSource>(Expression.Call(InfoOf(() => AsyncQueryable.SkipWhile<TSource>(default(IAsyncQueryable<TSource>), default(Expression<Func<TSource, bool>>))), source.Expression, predicate));
#else
            return source.Provider.CreateQuery<TSource>(Expression.Call(((MethodInfo)MethodBase.GetCurrentMethod()).MakeGenericMethod(typeof(TSource)), source.Expression, predicate));
#endif
        }

        public static IAsyncQueryable<TSource> SkipWhile<TSource>(this IAsyncQueryable<TSource> source, Expression<Func<TSource, int, bool>> predicate)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));
            if (predicate == null)
                throw new ArgumentNullException(nameof(predicate));

#if CRIPPLED_REFLECTION
            return source.Provider.CreateQuery<TSource>(Expression.Call(InfoOf(() => AsyncQueryable.SkipWhile<TSource>(default(IAsyncQueryable<TSource>), default(Expression<Func<TSource, int, bool>>))), source.Expression, predicate));
#else
            return source.Provider.CreateQuery<TSource>(Expression.Call(((MethodInfo)MethodBase.GetCurrentMethod()).MakeGenericMethod(typeof(TSource)), source.Expression, predicate));
#endif
        }

        public static IAsyncQueryable<TSource> SkipWhile<TSource>(this IAsyncQueryable<TSource> source, Expression<Func<TSource, int, Task<bool>>> predicate)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));
            if (predicate == null)
                throw new ArgumentNullException(nameof(predicate));

#if CRIPPLED_REFLECTION
            return source.Provider.CreateQuery<TSource>(Expression.Call(InfoOf(() => AsyncQueryable.SkipWhile<TSource>(default(IAsyncQueryable<TSource>), default(Expression<Func<TSource, int, Task<bool>>>))), source.Expression, predicate));
#else
            return source.Provider.CreateQuery<TSource>(Expression.Call(((MethodInfo)MethodBase.GetCurrentMethod()).MakeGenericMethod(typeof(TSource)), source.Expression, predicate));
#endif
        }

        public static IAsyncQueryable<TSource> SkipWhile<TSource>(this IAsyncQueryable<TSource> source, Expression<Func<TSource, Task<bool>>> predicate)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));
            if (predicate == null)
                throw new ArgumentNullException(nameof(predicate));

#if CRIPPLED_REFLECTION
            return source.Provider.CreateQuery<TSource>(Expression.Call(InfoOf(() => AsyncQueryable.SkipWhile<TSource>(default(IAsyncQueryable<TSource>), default(Expression<Func<TSource, Task<bool>>>))), source.Expression, predicate));
#else
            return source.Provider.CreateQuery<TSource>(Expression.Call(((MethodInfo)MethodBase.GetCurrentMethod()).MakeGenericMethod(typeof(TSource)), source.Expression, predicate));
#endif
        }

        public static Task<decimal?> Sum(this IAsyncQueryable<decimal?> source)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));

#if CRIPPLED_REFLECTION
            return source.Provider.ExecuteAsync<decimal?>(Expression.Call(InfoOf(() => AsyncQueryable.Sum(default(IAsyncQueryable<decimal?>))), source.Expression), CancellationToken.None);
#else
            return source.Provider.ExecuteAsync<decimal?>(Expression.Call((MethodInfo)MethodBase.GetCurrentMethod(), source.Expression), CancellationToken.None);
#endif
        }

        public static Task<decimal> Sum(this IAsyncQueryable<decimal> source)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));

#if CRIPPLED_REFLECTION
            return source.Provider.ExecuteAsync<decimal>(Expression.Call(InfoOf(() => AsyncQueryable.Sum(default(IAsyncQueryable<decimal>))), source.Expression), CancellationToken.None);
#else
            return source.Provider.ExecuteAsync<decimal>(Expression.Call((MethodInfo)MethodBase.GetCurrentMethod(), source.Expression), CancellationToken.None);
#endif
        }

        public static Task<double?> Sum(this IAsyncQueryable<double?> source)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));

#if CRIPPLED_REFLECTION
            return source.Provider.ExecuteAsync<double?>(Expression.Call(InfoOf(() => AsyncQueryable.Sum(default(IAsyncQueryable<double?>))), source.Expression), CancellationToken.None);
#else
            return source.Provider.ExecuteAsync<double?>(Expression.Call((MethodInfo)MethodBase.GetCurrentMethod(), source.Expression), CancellationToken.None);
#endif
        }

        public static Task<double> Sum(this IAsyncQueryable<double> source)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));

#if CRIPPLED_REFLECTION
            return source.Provider.ExecuteAsync<double>(Expression.Call(InfoOf(() => AsyncQueryable.Sum(default(IAsyncQueryable<double>))), source.Expression), CancellationToken.None);
#else
            return source.Provider.ExecuteAsync<double>(Expression.Call((MethodInfo)MethodBase.GetCurrentMethod(), source.Expression), CancellationToken.None);
#endif
        }

        public static Task<float?> Sum(this IAsyncQueryable<float?> source)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));

#if CRIPPLED_REFLECTION
            return source.Provider.ExecuteAsync<float?>(Expression.Call(InfoOf(() => AsyncQueryable.Sum(default(IAsyncQueryable<float?>))), source.Expression), CancellationToken.None);
#else
            return source.Provider.ExecuteAsync<float?>(Expression.Call((MethodInfo)MethodBase.GetCurrentMethod(), source.Expression), CancellationToken.None);
#endif
        }

        public static Task<float> Sum(this IAsyncQueryable<float> source)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));

#if CRIPPLED_REFLECTION
            return source.Provider.ExecuteAsync<float>(Expression.Call(InfoOf(() => AsyncQueryable.Sum(default(IAsyncQueryable<float>))), source.Expression), CancellationToken.None);
#else
            return source.Provider.ExecuteAsync<float>(Expression.Call((MethodInfo)MethodBase.GetCurrentMethod(), source.Expression), CancellationToken.None);
#endif
        }

        public static Task<int?> Sum(this IAsyncQueryable<int?> source)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));

#if CRIPPLED_REFLECTION
            return source.Provider.ExecuteAsync<int?>(Expression.Call(InfoOf(() => AsyncQueryable.Sum(default(IAsyncQueryable<int?>))), source.Expression), CancellationToken.None);
#else
            return source.Provider.ExecuteAsync<int?>(Expression.Call((MethodInfo)MethodBase.GetCurrentMethod(), source.Expression), CancellationToken.None);
#endif
        }

        public static Task<int> Sum(this IAsyncQueryable<int> source)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));

#if CRIPPLED_REFLECTION
            return source.Provider.ExecuteAsync<int>(Expression.Call(InfoOf(() => AsyncQueryable.Sum(default(IAsyncQueryable<int>))), source.Expression), CancellationToken.None);
#else
            return source.Provider.ExecuteAsync<int>(Expression.Call((MethodInfo)MethodBase.GetCurrentMethod(), source.Expression), CancellationToken.None);
#endif
        }

        public static Task<long?> Sum(this IAsyncQueryable<long?> source)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));

#if CRIPPLED_REFLECTION
            return source.Provider.ExecuteAsync<long?>(Expression.Call(InfoOf(() => AsyncQueryable.Sum(default(IAsyncQueryable<long?>))), source.Expression), CancellationToken.None);
#else
            return source.Provider.ExecuteAsync<long?>(Expression.Call((MethodInfo)MethodBase.GetCurrentMethod(), source.Expression), CancellationToken.None);
#endif
        }

        public static Task<long> Sum(this IAsyncQueryable<long> source)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));

#if CRIPPLED_REFLECTION
            return source.Provider.ExecuteAsync<long>(Expression.Call(InfoOf(() => AsyncQueryable.Sum(default(IAsyncQueryable<long>))), source.Expression), CancellationToken.None);
#else
            return source.Provider.ExecuteAsync<long>(Expression.Call((MethodInfo)MethodBase.GetCurrentMethod(), source.Expression), CancellationToken.None);
#endif
        }

        public static Task<decimal?> Sum(this IAsyncQueryable<decimal?> source, CancellationToken cancellationToken)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));

#if CRIPPLED_REFLECTION
            return source.Provider.ExecuteAsync<decimal?>(Expression.Call(InfoOf(() => AsyncQueryable.Sum(default(IAsyncQueryable<decimal?>), default(CancellationToken))), source.Expression, Expression.Constant(cancellationToken, typeof(CancellationToken))), cancellationToken);
#else
            return source.Provider.ExecuteAsync<decimal?>(Expression.Call((MethodInfo)MethodBase.GetCurrentMethod(), source.Expression, Expression.Constant(cancellationToken, typeof(CancellationToken))), cancellationToken);
#endif
        }

        public static Task<decimal> Sum(this IAsyncQueryable<decimal> source, CancellationToken cancellationToken)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));

#if CRIPPLED_REFLECTION
            return source.Provider.ExecuteAsync<decimal>(Expression.Call(InfoOf(() => AsyncQueryable.Sum(default(IAsyncQueryable<decimal>), default(CancellationToken))), source.Expression, Expression.Constant(cancellationToken, typeof(CancellationToken))), cancellationToken);
#else
            return source.Provider.ExecuteAsync<decimal>(Expression.Call((MethodInfo)MethodBase.GetCurrentMethod(), source.Expression, Expression.Constant(cancellationToken, typeof(CancellationToken))), cancellationToken);
#endif
        }

        public static Task<double?> Sum(this IAsyncQueryable<double?> source, CancellationToken cancellationToken)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));

#if CRIPPLED_REFLECTION
            return source.Provider.ExecuteAsync<double?>(Expression.Call(InfoOf(() => AsyncQueryable.Sum(default(IAsyncQueryable<double?>), default(CancellationToken))), source.Expression, Expression.Constant(cancellationToken, typeof(CancellationToken))), cancellationToken);
#else
            return source.Provider.ExecuteAsync<double?>(Expression.Call((MethodInfo)MethodBase.GetCurrentMethod(), source.Expression, Expression.Constant(cancellationToken, typeof(CancellationToken))), cancellationToken);
#endif
        }

        public static Task<double> Sum(this IAsyncQueryable<double> source, CancellationToken cancellationToken)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));

#if CRIPPLED_REFLECTION
            return source.Provider.ExecuteAsync<double>(Expression.Call(InfoOf(() => AsyncQueryable.Sum(default(IAsyncQueryable<double>), default(CancellationToken))), source.Expression, Expression.Constant(cancellationToken, typeof(CancellationToken))), cancellationToken);
#else
            return source.Provider.ExecuteAsync<double>(Expression.Call((MethodInfo)MethodBase.GetCurrentMethod(), source.Expression, Expression.Constant(cancellationToken, typeof(CancellationToken))), cancellationToken);
#endif
        }

        public static Task<float?> Sum(this IAsyncQueryable<float?> source, CancellationToken cancellationToken)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));

#if CRIPPLED_REFLECTION
            return source.Provider.ExecuteAsync<float?>(Expression.Call(InfoOf(() => AsyncQueryable.Sum(default(IAsyncQueryable<float?>), default(CancellationToken))), source.Expression, Expression.Constant(cancellationToken, typeof(CancellationToken))), cancellationToken);
#else
            return source.Provider.ExecuteAsync<float?>(Expression.Call((MethodInfo)MethodBase.GetCurrentMethod(), source.Expression, Expression.Constant(cancellationToken, typeof(CancellationToken))), cancellationToken);
#endif
        }

        public static Task<float> Sum(this IAsyncQueryable<float> source, CancellationToken cancellationToken)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));

#if CRIPPLED_REFLECTION
            return source.Provider.ExecuteAsync<float>(Expression.Call(InfoOf(() => AsyncQueryable.Sum(default(IAsyncQueryable<float>), default(CancellationToken))), source.Expression, Expression.Constant(cancellationToken, typeof(CancellationToken))), cancellationToken);
#else
            return source.Provider.ExecuteAsync<float>(Expression.Call((MethodInfo)MethodBase.GetCurrentMethod(), source.Expression, Expression.Constant(cancellationToken, typeof(CancellationToken))), cancellationToken);
#endif
        }

        public static Task<int?> Sum(this IAsyncQueryable<int?> source, CancellationToken cancellationToken)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));

#if CRIPPLED_REFLECTION
            return source.Provider.ExecuteAsync<int?>(Expression.Call(InfoOf(() => AsyncQueryable.Sum(default(IAsyncQueryable<int?>), default(CancellationToken))), source.Expression, Expression.Constant(cancellationToken, typeof(CancellationToken))), cancellationToken);
#else
            return source.Provider.ExecuteAsync<int?>(Expression.Call((MethodInfo)MethodBase.GetCurrentMethod(), source.Expression, Expression.Constant(cancellationToken, typeof(CancellationToken))), cancellationToken);
#endif
        }

        public static Task<int> Sum(this IAsyncQueryable<int> source, CancellationToken cancellationToken)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));

#if CRIPPLED_REFLECTION
            return source.Provider.ExecuteAsync<int>(Expression.Call(InfoOf(() => AsyncQueryable.Sum(default(IAsyncQueryable<int>), default(CancellationToken))), source.Expression, Expression.Constant(cancellationToken, typeof(CancellationToken))), cancellationToken);
#else
            return source.Provider.ExecuteAsync<int>(Expression.Call((MethodInfo)MethodBase.GetCurrentMethod(), source.Expression, Expression.Constant(cancellationToken, typeof(CancellationToken))), cancellationToken);
#endif
        }

        public static Task<long?> Sum(this IAsyncQueryable<long?> source, CancellationToken cancellationToken)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));

#if CRIPPLED_REFLECTION
            return source.Provider.ExecuteAsync<long?>(Expression.Call(InfoOf(() => AsyncQueryable.Sum(default(IAsyncQueryable<long?>), default(CancellationToken))), source.Expression, Expression.Constant(cancellationToken, typeof(CancellationToken))), cancellationToken);
#else
            return source.Provider.ExecuteAsync<long?>(Expression.Call((MethodInfo)MethodBase.GetCurrentMethod(), source.Expression, Expression.Constant(cancellationToken, typeof(CancellationToken))), cancellationToken);
#endif
        }

        public static Task<long> Sum(this IAsyncQueryable<long> source, CancellationToken cancellationToken)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));

#if CRIPPLED_REFLECTION
            return source.Provider.ExecuteAsync<long>(Expression.Call(InfoOf(() => AsyncQueryable.Sum(default(IAsyncQueryable<long>), default(CancellationToken))), source.Expression, Expression.Constant(cancellationToken, typeof(CancellationToken))), cancellationToken);
#else
            return source.Provider.ExecuteAsync<long>(Expression.Call((MethodInfo)MethodBase.GetCurrentMethod(), source.Expression, Expression.Constant(cancellationToken, typeof(CancellationToken))), cancellationToken);
#endif
        }

        public static Task<decimal?> Sum<TSource>(this IAsyncQueryable<TSource> source, Expression<Func<TSource, decimal?>> selector)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));
            if (selector == null)
                throw new ArgumentNullException(nameof(selector));

#if CRIPPLED_REFLECTION
            return source.Provider.ExecuteAsync<decimal?>(Expression.Call(InfoOf(() => AsyncQueryable.Sum<TSource>(default(IAsyncQueryable<TSource>), default(Expression<Func<TSource, decimal?>>))), source.Expression, selector), CancellationToken.None);
#else
            return source.Provider.ExecuteAsync<decimal?>(Expression.Call(((MethodInfo)MethodBase.GetCurrentMethod()).MakeGenericMethod(typeof(TSource)), source.Expression, selector), CancellationToken.None);
#endif
        }

        public static Task<decimal> Sum<TSource>(this IAsyncQueryable<TSource> source, Expression<Func<TSource, decimal>> selector)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));
            if (selector == null)
                throw new ArgumentNullException(nameof(selector));

#if CRIPPLED_REFLECTION
            return source.Provider.ExecuteAsync<decimal>(Expression.Call(InfoOf(() => AsyncQueryable.Sum<TSource>(default(IAsyncQueryable<TSource>), default(Expression<Func<TSource, decimal>>))), source.Expression, selector), CancellationToken.None);
#else
            return source.Provider.ExecuteAsync<decimal>(Expression.Call(((MethodInfo)MethodBase.GetCurrentMethod()).MakeGenericMethod(typeof(TSource)), source.Expression, selector), CancellationToken.None);
#endif
        }

        public static Task<double?> Sum<TSource>(this IAsyncQueryable<TSource> source, Expression<Func<TSource, double?>> selector)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));
            if (selector == null)
                throw new ArgumentNullException(nameof(selector));

#if CRIPPLED_REFLECTION
            return source.Provider.ExecuteAsync<double?>(Expression.Call(InfoOf(() => AsyncQueryable.Sum<TSource>(default(IAsyncQueryable<TSource>), default(Expression<Func<TSource, double?>>))), source.Expression, selector), CancellationToken.None);
#else
            return source.Provider.ExecuteAsync<double?>(Expression.Call(((MethodInfo)MethodBase.GetCurrentMethod()).MakeGenericMethod(typeof(TSource)), source.Expression, selector), CancellationToken.None);
#endif
        }

        public static Task<double> Sum<TSource>(this IAsyncQueryable<TSource> source, Expression<Func<TSource, double>> selector)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));
            if (selector == null)
                throw new ArgumentNullException(nameof(selector));

#if CRIPPLED_REFLECTION
            return source.Provider.ExecuteAsync<double>(Expression.Call(InfoOf(() => AsyncQueryable.Sum<TSource>(default(IAsyncQueryable<TSource>), default(Expression<Func<TSource, double>>))), source.Expression, selector), CancellationToken.None);
#else
            return source.Provider.ExecuteAsync<double>(Expression.Call(((MethodInfo)MethodBase.GetCurrentMethod()).MakeGenericMethod(typeof(TSource)), source.Expression, selector), CancellationToken.None);
#endif
        }

        public static Task<float?> Sum<TSource>(this IAsyncQueryable<TSource> source, Expression<Func<TSource, float?>> selector)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));
            if (selector == null)
                throw new ArgumentNullException(nameof(selector));

#if CRIPPLED_REFLECTION
            return source.Provider.ExecuteAsync<float?>(Expression.Call(InfoOf(() => AsyncQueryable.Sum<TSource>(default(IAsyncQueryable<TSource>), default(Expression<Func<TSource, float?>>))), source.Expression, selector), CancellationToken.None);
#else
            return source.Provider.ExecuteAsync<float?>(Expression.Call(((MethodInfo)MethodBase.GetCurrentMethod()).MakeGenericMethod(typeof(TSource)), source.Expression, selector), CancellationToken.None);
#endif
        }

        public static Task<float> Sum<TSource>(this IAsyncQueryable<TSource> source, Expression<Func<TSource, float>> selector)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));
            if (selector == null)
                throw new ArgumentNullException(nameof(selector));

#if CRIPPLED_REFLECTION
            return source.Provider.ExecuteAsync<float>(Expression.Call(InfoOf(() => AsyncQueryable.Sum<TSource>(default(IAsyncQueryable<TSource>), default(Expression<Func<TSource, float>>))), source.Expression, selector), CancellationToken.None);
#else
            return source.Provider.ExecuteAsync<float>(Expression.Call(((MethodInfo)MethodBase.GetCurrentMethod()).MakeGenericMethod(typeof(TSource)), source.Expression, selector), CancellationToken.None);
#endif
        }

        public static Task<int?> Sum<TSource>(this IAsyncQueryable<TSource> source, Expression<Func<TSource, int?>> selector)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));
            if (selector == null)
                throw new ArgumentNullException(nameof(selector));

#if CRIPPLED_REFLECTION
            return source.Provider.ExecuteAsync<int?>(Expression.Call(InfoOf(() => AsyncQueryable.Sum<TSource>(default(IAsyncQueryable<TSource>), default(Expression<Func<TSource, int?>>))), source.Expression, selector), CancellationToken.None);
#else
            return source.Provider.ExecuteAsync<int?>(Expression.Call(((MethodInfo)MethodBase.GetCurrentMethod()).MakeGenericMethod(typeof(TSource)), source.Expression, selector), CancellationToken.None);
#endif
        }

        public static Task<int> Sum<TSource>(this IAsyncQueryable<TSource> source, Expression<Func<TSource, int>> selector)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));
            if (selector == null)
                throw new ArgumentNullException(nameof(selector));

#if CRIPPLED_REFLECTION
            return source.Provider.ExecuteAsync<int>(Expression.Call(InfoOf(() => AsyncQueryable.Sum<TSource>(default(IAsyncQueryable<TSource>), default(Expression<Func<TSource, int>>))), source.Expression, selector), CancellationToken.None);
#else
            return source.Provider.ExecuteAsync<int>(Expression.Call(((MethodInfo)MethodBase.GetCurrentMethod()).MakeGenericMethod(typeof(TSource)), source.Expression, selector), CancellationToken.None);
#endif
        }

        public static Task<long?> Sum<TSource>(this IAsyncQueryable<TSource> source, Expression<Func<TSource, long?>> selector)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));
            if (selector == null)
                throw new ArgumentNullException(nameof(selector));

#if CRIPPLED_REFLECTION
            return source.Provider.ExecuteAsync<long?>(Expression.Call(InfoOf(() => AsyncQueryable.Sum<TSource>(default(IAsyncQueryable<TSource>), default(Expression<Func<TSource, long?>>))), source.Expression, selector), CancellationToken.None);
#else
            return source.Provider.ExecuteAsync<long?>(Expression.Call(((MethodInfo)MethodBase.GetCurrentMethod()).MakeGenericMethod(typeof(TSource)), source.Expression, selector), CancellationToken.None);
#endif
        }

        public static Task<long> Sum<TSource>(this IAsyncQueryable<TSource> source, Expression<Func<TSource, long>> selector)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));
            if (selector == null)
                throw new ArgumentNullException(nameof(selector));

#if CRIPPLED_REFLECTION
            return source.Provider.ExecuteAsync<long>(Expression.Call(InfoOf(() => AsyncQueryable.Sum<TSource>(default(IAsyncQueryable<TSource>), default(Expression<Func<TSource, long>>))), source.Expression, selector), CancellationToken.None);
#else
            return source.Provider.ExecuteAsync<long>(Expression.Call(((MethodInfo)MethodBase.GetCurrentMethod()).MakeGenericMethod(typeof(TSource)), source.Expression, selector), CancellationToken.None);
#endif
        }

        public static Task<decimal?> Sum<TSource>(this IAsyncQueryable<TSource> source, Expression<Func<TSource, Task<decimal?>>> selector)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));
            if (selector == null)
                throw new ArgumentNullException(nameof(selector));

#if CRIPPLED_REFLECTION
            return source.Provider.ExecuteAsync<decimal?>(Expression.Call(InfoOf(() => AsyncQueryable.Sum<TSource>(default(IAsyncQueryable<TSource>), default(Expression<Func<TSource, Task<decimal?>>>))), source.Expression, selector), CancellationToken.None);
#else
            return source.Provider.ExecuteAsync<decimal?>(Expression.Call(((MethodInfo)MethodBase.GetCurrentMethod()).MakeGenericMethod(typeof(TSource)), source.Expression, selector), CancellationToken.None);
#endif
        }

        public static Task<decimal> Sum<TSource>(this IAsyncQueryable<TSource> source, Expression<Func<TSource, Task<decimal>>> selector)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));
            if (selector == null)
                throw new ArgumentNullException(nameof(selector));

#if CRIPPLED_REFLECTION
            return source.Provider.ExecuteAsync<decimal>(Expression.Call(InfoOf(() => AsyncQueryable.Sum<TSource>(default(IAsyncQueryable<TSource>), default(Expression<Func<TSource, Task<decimal>>>))), source.Expression, selector), CancellationToken.None);
#else
            return source.Provider.ExecuteAsync<decimal>(Expression.Call(((MethodInfo)MethodBase.GetCurrentMethod()).MakeGenericMethod(typeof(TSource)), source.Expression, selector), CancellationToken.None);
#endif
        }

        public static Task<double?> Sum<TSource>(this IAsyncQueryable<TSource> source, Expression<Func<TSource, Task<double?>>> selector)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));
            if (selector == null)
                throw new ArgumentNullException(nameof(selector));

#if CRIPPLED_REFLECTION
            return source.Provider.ExecuteAsync<double?>(Expression.Call(InfoOf(() => AsyncQueryable.Sum<TSource>(default(IAsyncQueryable<TSource>), default(Expression<Func<TSource, Task<double?>>>))), source.Expression, selector), CancellationToken.None);
#else
            return source.Provider.ExecuteAsync<double?>(Expression.Call(((MethodInfo)MethodBase.GetCurrentMethod()).MakeGenericMethod(typeof(TSource)), source.Expression, selector), CancellationToken.None);
#endif
        }

        public static Task<double> Sum<TSource>(this IAsyncQueryable<TSource> source, Expression<Func<TSource, Task<double>>> selector)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));
            if (selector == null)
                throw new ArgumentNullException(nameof(selector));

#if CRIPPLED_REFLECTION
            return source.Provider.ExecuteAsync<double>(Expression.Call(InfoOf(() => AsyncQueryable.Sum<TSource>(default(IAsyncQueryable<TSource>), default(Expression<Func<TSource, Task<double>>>))), source.Expression, selector), CancellationToken.None);
#else
            return source.Provider.ExecuteAsync<double>(Expression.Call(((MethodInfo)MethodBase.GetCurrentMethod()).MakeGenericMethod(typeof(TSource)), source.Expression, selector), CancellationToken.None);
#endif
        }

        public static Task<float?> Sum<TSource>(this IAsyncQueryable<TSource> source, Expression<Func<TSource, Task<float?>>> selector)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));
            if (selector == null)
                throw new ArgumentNullException(nameof(selector));

#if CRIPPLED_REFLECTION
            return source.Provider.ExecuteAsync<float?>(Expression.Call(InfoOf(() => AsyncQueryable.Sum<TSource>(default(IAsyncQueryable<TSource>), default(Expression<Func<TSource, Task<float?>>>))), source.Expression, selector), CancellationToken.None);
#else
            return source.Provider.ExecuteAsync<float?>(Expression.Call(((MethodInfo)MethodBase.GetCurrentMethod()).MakeGenericMethod(typeof(TSource)), source.Expression, selector), CancellationToken.None);
#endif
        }

        public static Task<float> Sum<TSource>(this IAsyncQueryable<TSource> source, Expression<Func<TSource, Task<float>>> selector)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));
            if (selector == null)
                throw new ArgumentNullException(nameof(selector));

#if CRIPPLED_REFLECTION
            return source.Provider.ExecuteAsync<float>(Expression.Call(InfoOf(() => AsyncQueryable.Sum<TSource>(default(IAsyncQueryable<TSource>), default(Expression<Func<TSource, Task<float>>>))), source.Expression, selector), CancellationToken.None);
#else
            return source.Provider.ExecuteAsync<float>(Expression.Call(((MethodInfo)MethodBase.GetCurrentMethod()).MakeGenericMethod(typeof(TSource)), source.Expression, selector), CancellationToken.None);
#endif
        }

        public static Task<int?> Sum<TSource>(this IAsyncQueryable<TSource> source, Expression<Func<TSource, Task<int?>>> selector)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));
            if (selector == null)
                throw new ArgumentNullException(nameof(selector));

#if CRIPPLED_REFLECTION
            return source.Provider.ExecuteAsync<int?>(Expression.Call(InfoOf(() => AsyncQueryable.Sum<TSource>(default(IAsyncQueryable<TSource>), default(Expression<Func<TSource, Task<int?>>>))), source.Expression, selector), CancellationToken.None);
#else
            return source.Provider.ExecuteAsync<int?>(Expression.Call(((MethodInfo)MethodBase.GetCurrentMethod()).MakeGenericMethod(typeof(TSource)), source.Expression, selector), CancellationToken.None);
#endif
        }

        public static Task<int> Sum<TSource>(this IAsyncQueryable<TSource> source, Expression<Func<TSource, Task<int>>> selector)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));
            if (selector == null)
                throw new ArgumentNullException(nameof(selector));

#if CRIPPLED_REFLECTION
            return source.Provider.ExecuteAsync<int>(Expression.Call(InfoOf(() => AsyncQueryable.Sum<TSource>(default(IAsyncQueryable<TSource>), default(Expression<Func<TSource, Task<int>>>))), source.Expression, selector), CancellationToken.None);
#else
            return source.Provider.ExecuteAsync<int>(Expression.Call(((MethodInfo)MethodBase.GetCurrentMethod()).MakeGenericMethod(typeof(TSource)), source.Expression, selector), CancellationToken.None);
#endif
        }

        public static Task<long?> Sum<TSource>(this IAsyncQueryable<TSource> source, Expression<Func<TSource, Task<long?>>> selector)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));
            if (selector == null)
                throw new ArgumentNullException(nameof(selector));

#if CRIPPLED_REFLECTION
            return source.Provider.ExecuteAsync<long?>(Expression.Call(InfoOf(() => AsyncQueryable.Sum<TSource>(default(IAsyncQueryable<TSource>), default(Expression<Func<TSource, Task<long?>>>))), source.Expression, selector), CancellationToken.None);
#else
            return source.Provider.ExecuteAsync<long?>(Expression.Call(((MethodInfo)MethodBase.GetCurrentMethod()).MakeGenericMethod(typeof(TSource)), source.Expression, selector), CancellationToken.None);
#endif
        }

        public static Task<long> Sum<TSource>(this IAsyncQueryable<TSource> source, Expression<Func<TSource, Task<long>>> selector)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));
            if (selector == null)
                throw new ArgumentNullException(nameof(selector));

#if CRIPPLED_REFLECTION
            return source.Provider.ExecuteAsync<long>(Expression.Call(InfoOf(() => AsyncQueryable.Sum<TSource>(default(IAsyncQueryable<TSource>), default(Expression<Func<TSource, Task<long>>>))), source.Expression, selector), CancellationToken.None);
#else
            return source.Provider.ExecuteAsync<long>(Expression.Call(((MethodInfo)MethodBase.GetCurrentMethod()).MakeGenericMethod(typeof(TSource)), source.Expression, selector), CancellationToken.None);
#endif
        }

        public static Task<decimal?> Sum<TSource>(this IAsyncQueryable<TSource> source, Expression<Func<TSource, decimal?>> selector, CancellationToken cancellationToken)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));
            if (selector == null)
                throw new ArgumentNullException(nameof(selector));

#if CRIPPLED_REFLECTION
            return source.Provider.ExecuteAsync<decimal?>(Expression.Call(InfoOf(() => AsyncQueryable.Sum<TSource>(default(IAsyncQueryable<TSource>), default(Expression<Func<TSource, decimal?>>), default(CancellationToken))), source.Expression, selector, Expression.Constant(cancellationToken, typeof(CancellationToken))), cancellationToken);
#else
            return source.Provider.ExecuteAsync<decimal?>(Expression.Call(((MethodInfo)MethodBase.GetCurrentMethod()).MakeGenericMethod(typeof(TSource)), source.Expression, selector, Expression.Constant(cancellationToken, typeof(CancellationToken))), cancellationToken);
#endif
        }

        public static Task<decimal> Sum<TSource>(this IAsyncQueryable<TSource> source, Expression<Func<TSource, decimal>> selector, CancellationToken cancellationToken)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));
            if (selector == null)
                throw new ArgumentNullException(nameof(selector));

#if CRIPPLED_REFLECTION
            return source.Provider.ExecuteAsync<decimal>(Expression.Call(InfoOf(() => AsyncQueryable.Sum<TSource>(default(IAsyncQueryable<TSource>), default(Expression<Func<TSource, decimal>>), default(CancellationToken))), source.Expression, selector, Expression.Constant(cancellationToken, typeof(CancellationToken))), cancellationToken);
#else
            return source.Provider.ExecuteAsync<decimal>(Expression.Call(((MethodInfo)MethodBase.GetCurrentMethod()).MakeGenericMethod(typeof(TSource)), source.Expression, selector, Expression.Constant(cancellationToken, typeof(CancellationToken))), cancellationToken);
#endif
        }

        public static Task<double?> Sum<TSource>(this IAsyncQueryable<TSource> source, Expression<Func<TSource, double?>> selector, CancellationToken cancellationToken)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));
            if (selector == null)
                throw new ArgumentNullException(nameof(selector));

#if CRIPPLED_REFLECTION
            return source.Provider.ExecuteAsync<double?>(Expression.Call(InfoOf(() => AsyncQueryable.Sum<TSource>(default(IAsyncQueryable<TSource>), default(Expression<Func<TSource, double?>>), default(CancellationToken))), source.Expression, selector, Expression.Constant(cancellationToken, typeof(CancellationToken))), cancellationToken);
#else
            return source.Provider.ExecuteAsync<double?>(Expression.Call(((MethodInfo)MethodBase.GetCurrentMethod()).MakeGenericMethod(typeof(TSource)), source.Expression, selector, Expression.Constant(cancellationToken, typeof(CancellationToken))), cancellationToken);
#endif
        }

        public static Task<double> Sum<TSource>(this IAsyncQueryable<TSource> source, Expression<Func<TSource, double>> selector, CancellationToken cancellationToken)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));
            if (selector == null)
                throw new ArgumentNullException(nameof(selector));

#if CRIPPLED_REFLECTION
            return source.Provider.ExecuteAsync<double>(Expression.Call(InfoOf(() => AsyncQueryable.Sum<TSource>(default(IAsyncQueryable<TSource>), default(Expression<Func<TSource, double>>), default(CancellationToken))), source.Expression, selector, Expression.Constant(cancellationToken, typeof(CancellationToken))), cancellationToken);
#else
            return source.Provider.ExecuteAsync<double>(Expression.Call(((MethodInfo)MethodBase.GetCurrentMethod()).MakeGenericMethod(typeof(TSource)), source.Expression, selector, Expression.Constant(cancellationToken, typeof(CancellationToken))), cancellationToken);
#endif
        }

        public static Task<float?> Sum<TSource>(this IAsyncQueryable<TSource> source, Expression<Func<TSource, float?>> selector, CancellationToken cancellationToken)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));
            if (selector == null)
                throw new ArgumentNullException(nameof(selector));

#if CRIPPLED_REFLECTION
            return source.Provider.ExecuteAsync<float?>(Expression.Call(InfoOf(() => AsyncQueryable.Sum<TSource>(default(IAsyncQueryable<TSource>), default(Expression<Func<TSource, float?>>), default(CancellationToken))), source.Expression, selector, Expression.Constant(cancellationToken, typeof(CancellationToken))), cancellationToken);
#else
            return source.Provider.ExecuteAsync<float?>(Expression.Call(((MethodInfo)MethodBase.GetCurrentMethod()).MakeGenericMethod(typeof(TSource)), source.Expression, selector, Expression.Constant(cancellationToken, typeof(CancellationToken))), cancellationToken);
#endif
        }

        public static Task<float> Sum<TSource>(this IAsyncQueryable<TSource> source, Expression<Func<TSource, float>> selector, CancellationToken cancellationToken)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));
            if (selector == null)
                throw new ArgumentNullException(nameof(selector));

#if CRIPPLED_REFLECTION
            return source.Provider.ExecuteAsync<float>(Expression.Call(InfoOf(() => AsyncQueryable.Sum<TSource>(default(IAsyncQueryable<TSource>), default(Expression<Func<TSource, float>>), default(CancellationToken))), source.Expression, selector, Expression.Constant(cancellationToken, typeof(CancellationToken))), cancellationToken);
#else
            return source.Provider.ExecuteAsync<float>(Expression.Call(((MethodInfo)MethodBase.GetCurrentMethod()).MakeGenericMethod(typeof(TSource)), source.Expression, selector, Expression.Constant(cancellationToken, typeof(CancellationToken))), cancellationToken);
#endif
        }

        public static Task<int?> Sum<TSource>(this IAsyncQueryable<TSource> source, Expression<Func<TSource, int?>> selector, CancellationToken cancellationToken)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));
            if (selector == null)
                throw new ArgumentNullException(nameof(selector));

#if CRIPPLED_REFLECTION
            return source.Provider.ExecuteAsync<int?>(Expression.Call(InfoOf(() => AsyncQueryable.Sum<TSource>(default(IAsyncQueryable<TSource>), default(Expression<Func<TSource, int?>>), default(CancellationToken))), source.Expression, selector, Expression.Constant(cancellationToken, typeof(CancellationToken))), cancellationToken);
#else
            return source.Provider.ExecuteAsync<int?>(Expression.Call(((MethodInfo)MethodBase.GetCurrentMethod()).MakeGenericMethod(typeof(TSource)), source.Expression, selector, Expression.Constant(cancellationToken, typeof(CancellationToken))), cancellationToken);
#endif
        }

        public static Task<int> Sum<TSource>(this IAsyncQueryable<TSource> source, Expression<Func<TSource, int>> selector, CancellationToken cancellationToken)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));
            if (selector == null)
                throw new ArgumentNullException(nameof(selector));

#if CRIPPLED_REFLECTION
            return source.Provider.ExecuteAsync<int>(Expression.Call(InfoOf(() => AsyncQueryable.Sum<TSource>(default(IAsyncQueryable<TSource>), default(Expression<Func<TSource, int>>), default(CancellationToken))), source.Expression, selector, Expression.Constant(cancellationToken, typeof(CancellationToken))), cancellationToken);
#else
            return source.Provider.ExecuteAsync<int>(Expression.Call(((MethodInfo)MethodBase.GetCurrentMethod()).MakeGenericMethod(typeof(TSource)), source.Expression, selector, Expression.Constant(cancellationToken, typeof(CancellationToken))), cancellationToken);
#endif
        }

        public static Task<long?> Sum<TSource>(this IAsyncQueryable<TSource> source, Expression<Func<TSource, long?>> selector, CancellationToken cancellationToken)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));
            if (selector == null)
                throw new ArgumentNullException(nameof(selector));

#if CRIPPLED_REFLECTION
            return source.Provider.ExecuteAsync<long?>(Expression.Call(InfoOf(() => AsyncQueryable.Sum<TSource>(default(IAsyncQueryable<TSource>), default(Expression<Func<TSource, long?>>), default(CancellationToken))), source.Expression, selector, Expression.Constant(cancellationToken, typeof(CancellationToken))), cancellationToken);
#else
            return source.Provider.ExecuteAsync<long?>(Expression.Call(((MethodInfo)MethodBase.GetCurrentMethod()).MakeGenericMethod(typeof(TSource)), source.Expression, selector, Expression.Constant(cancellationToken, typeof(CancellationToken))), cancellationToken);
#endif
        }

        public static Task<long> Sum<TSource>(this IAsyncQueryable<TSource> source, Expression<Func<TSource, long>> selector, CancellationToken cancellationToken)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));
            if (selector == null)
                throw new ArgumentNullException(nameof(selector));

#if CRIPPLED_REFLECTION
            return source.Provider.ExecuteAsync<long>(Expression.Call(InfoOf(() => AsyncQueryable.Sum<TSource>(default(IAsyncQueryable<TSource>), default(Expression<Func<TSource, long>>), default(CancellationToken))), source.Expression, selector, Expression.Constant(cancellationToken, typeof(CancellationToken))), cancellationToken);
#else
            return source.Provider.ExecuteAsync<long>(Expression.Call(((MethodInfo)MethodBase.GetCurrentMethod()).MakeGenericMethod(typeof(TSource)), source.Expression, selector, Expression.Constant(cancellationToken, typeof(CancellationToken))), cancellationToken);
#endif
        }

        public static Task<decimal?> Sum<TSource>(this IAsyncQueryable<TSource> source, Expression<Func<TSource, Task<decimal?>>> selector, CancellationToken cancellationToken)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));
            if (selector == null)
                throw new ArgumentNullException(nameof(selector));

#if CRIPPLED_REFLECTION
            return source.Provider.ExecuteAsync<decimal?>(Expression.Call(InfoOf(() => AsyncQueryable.Sum<TSource>(default(IAsyncQueryable<TSource>), default(Expression<Func<TSource, Task<decimal?>>>), default(CancellationToken))), source.Expression, selector, Expression.Constant(cancellationToken, typeof(CancellationToken))), cancellationToken);
#else
            return source.Provider.ExecuteAsync<decimal?>(Expression.Call(((MethodInfo)MethodBase.GetCurrentMethod()).MakeGenericMethod(typeof(TSource)), source.Expression, selector, Expression.Constant(cancellationToken, typeof(CancellationToken))), cancellationToken);
#endif
        }

        public static Task<decimal> Sum<TSource>(this IAsyncQueryable<TSource> source, Expression<Func<TSource, Task<decimal>>> selector, CancellationToken cancellationToken)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));
            if (selector == null)
                throw new ArgumentNullException(nameof(selector));

#if CRIPPLED_REFLECTION
            return source.Provider.ExecuteAsync<decimal>(Expression.Call(InfoOf(() => AsyncQueryable.Sum<TSource>(default(IAsyncQueryable<TSource>), default(Expression<Func<TSource, Task<decimal>>>), default(CancellationToken))), source.Expression, selector, Expression.Constant(cancellationToken, typeof(CancellationToken))), cancellationToken);
#else
            return source.Provider.ExecuteAsync<decimal>(Expression.Call(((MethodInfo)MethodBase.GetCurrentMethod()).MakeGenericMethod(typeof(TSource)), source.Expression, selector, Expression.Constant(cancellationToken, typeof(CancellationToken))), cancellationToken);
#endif
        }

        public static Task<double?> Sum<TSource>(this IAsyncQueryable<TSource> source, Expression<Func<TSource, Task<double?>>> selector, CancellationToken cancellationToken)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));
            if (selector == null)
                throw new ArgumentNullException(nameof(selector));

#if CRIPPLED_REFLECTION
            return source.Provider.ExecuteAsync<double?>(Expression.Call(InfoOf(() => AsyncQueryable.Sum<TSource>(default(IAsyncQueryable<TSource>), default(Expression<Func<TSource, Task<double?>>>), default(CancellationToken))), source.Expression, selector, Expression.Constant(cancellationToken, typeof(CancellationToken))), cancellationToken);
#else
            return source.Provider.ExecuteAsync<double?>(Expression.Call(((MethodInfo)MethodBase.GetCurrentMethod()).MakeGenericMethod(typeof(TSource)), source.Expression, selector, Expression.Constant(cancellationToken, typeof(CancellationToken))), cancellationToken);
#endif
        }

        public static Task<double> Sum<TSource>(this IAsyncQueryable<TSource> source, Expression<Func<TSource, Task<double>>> selector, CancellationToken cancellationToken)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));
            if (selector == null)
                throw new ArgumentNullException(nameof(selector));

#if CRIPPLED_REFLECTION
            return source.Provider.ExecuteAsync<double>(Expression.Call(InfoOf(() => AsyncQueryable.Sum<TSource>(default(IAsyncQueryable<TSource>), default(Expression<Func<TSource, Task<double>>>), default(CancellationToken))), source.Expression, selector, Expression.Constant(cancellationToken, typeof(CancellationToken))), cancellationToken);
#else
            return source.Provider.ExecuteAsync<double>(Expression.Call(((MethodInfo)MethodBase.GetCurrentMethod()).MakeGenericMethod(typeof(TSource)), source.Expression, selector, Expression.Constant(cancellationToken, typeof(CancellationToken))), cancellationToken);
#endif
        }

        public static Task<float?> Sum<TSource>(this IAsyncQueryable<TSource> source, Expression<Func<TSource, Task<float?>>> selector, CancellationToken cancellationToken)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));
            if (selector == null)
                throw new ArgumentNullException(nameof(selector));

#if CRIPPLED_REFLECTION
            return source.Provider.ExecuteAsync<float?>(Expression.Call(InfoOf(() => AsyncQueryable.Sum<TSource>(default(IAsyncQueryable<TSource>), default(Expression<Func<TSource, Task<float?>>>), default(CancellationToken))), source.Expression, selector, Expression.Constant(cancellationToken, typeof(CancellationToken))), cancellationToken);
#else
            return source.Provider.ExecuteAsync<float?>(Expression.Call(((MethodInfo)MethodBase.GetCurrentMethod()).MakeGenericMethod(typeof(TSource)), source.Expression, selector, Expression.Constant(cancellationToken, typeof(CancellationToken))), cancellationToken);
#endif
        }

        public static Task<float> Sum<TSource>(this IAsyncQueryable<TSource> source, Expression<Func<TSource, Task<float>>> selector, CancellationToken cancellationToken)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));
            if (selector == null)
                throw new ArgumentNullException(nameof(selector));

#if CRIPPLED_REFLECTION
            return source.Provider.ExecuteAsync<float>(Expression.Call(InfoOf(() => AsyncQueryable.Sum<TSource>(default(IAsyncQueryable<TSource>), default(Expression<Func<TSource, Task<float>>>), default(CancellationToken))), source.Expression, selector, Expression.Constant(cancellationToken, typeof(CancellationToken))), cancellationToken);
#else
            return source.Provider.ExecuteAsync<float>(Expression.Call(((MethodInfo)MethodBase.GetCurrentMethod()).MakeGenericMethod(typeof(TSource)), source.Expression, selector, Expression.Constant(cancellationToken, typeof(CancellationToken))), cancellationToken);
#endif
        }

        public static Task<int?> Sum<TSource>(this IAsyncQueryable<TSource> source, Expression<Func<TSource, Task<int?>>> selector, CancellationToken cancellationToken)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));
            if (selector == null)
                throw new ArgumentNullException(nameof(selector));

#if CRIPPLED_REFLECTION
            return source.Provider.ExecuteAsync<int?>(Expression.Call(InfoOf(() => AsyncQueryable.Sum<TSource>(default(IAsyncQueryable<TSource>), default(Expression<Func<TSource, Task<int?>>>), default(CancellationToken))), source.Expression, selector, Expression.Constant(cancellationToken, typeof(CancellationToken))), cancellationToken);
#else
            return source.Provider.ExecuteAsync<int?>(Expression.Call(((MethodInfo)MethodBase.GetCurrentMethod()).MakeGenericMethod(typeof(TSource)), source.Expression, selector, Expression.Constant(cancellationToken, typeof(CancellationToken))), cancellationToken);
#endif
        }

        public static Task<int> Sum<TSource>(this IAsyncQueryable<TSource> source, Expression<Func<TSource, Task<int>>> selector, CancellationToken cancellationToken)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));
            if (selector == null)
                throw new ArgumentNullException(nameof(selector));

#if CRIPPLED_REFLECTION
            return source.Provider.ExecuteAsync<int>(Expression.Call(InfoOf(() => AsyncQueryable.Sum<TSource>(default(IAsyncQueryable<TSource>), default(Expression<Func<TSource, Task<int>>>), default(CancellationToken))), source.Expression, selector, Expression.Constant(cancellationToken, typeof(CancellationToken))), cancellationToken);
#else
            return source.Provider.ExecuteAsync<int>(Expression.Call(((MethodInfo)MethodBase.GetCurrentMethod()).MakeGenericMethod(typeof(TSource)), source.Expression, selector, Expression.Constant(cancellationToken, typeof(CancellationToken))), cancellationToken);
#endif
        }

        public static Task<long?> Sum<TSource>(this IAsyncQueryable<TSource> source, Expression<Func<TSource, Task<long?>>> selector, CancellationToken cancellationToken)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));
            if (selector == null)
                throw new ArgumentNullException(nameof(selector));

#if CRIPPLED_REFLECTION
            return source.Provider.ExecuteAsync<long?>(Expression.Call(InfoOf(() => AsyncQueryable.Sum<TSource>(default(IAsyncQueryable<TSource>), default(Expression<Func<TSource, Task<long?>>>), default(CancellationToken))), source.Expression, selector, Expression.Constant(cancellationToken, typeof(CancellationToken))), cancellationToken);
#else
            return source.Provider.ExecuteAsync<long?>(Expression.Call(((MethodInfo)MethodBase.GetCurrentMethod()).MakeGenericMethod(typeof(TSource)), source.Expression, selector, Expression.Constant(cancellationToken, typeof(CancellationToken))), cancellationToken);
#endif
        }

        public static Task<long> Sum<TSource>(this IAsyncQueryable<TSource> source, Expression<Func<TSource, Task<long>>> selector, CancellationToken cancellationToken)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));
            if (selector == null)
                throw new ArgumentNullException(nameof(selector));

#if CRIPPLED_REFLECTION
            return source.Provider.ExecuteAsync<long>(Expression.Call(InfoOf(() => AsyncQueryable.Sum<TSource>(default(IAsyncQueryable<TSource>), default(Expression<Func<TSource, Task<long>>>), default(CancellationToken))), source.Expression, selector, Expression.Constant(cancellationToken, typeof(CancellationToken))), cancellationToken);
#else
            return source.Provider.ExecuteAsync<long>(Expression.Call(((MethodInfo)MethodBase.GetCurrentMethod()).MakeGenericMethod(typeof(TSource)), source.Expression, selector, Expression.Constant(cancellationToken, typeof(CancellationToken))), cancellationToken);
#endif
        }

        public static IAsyncQueryable<TSource> Take<TSource>(this IAsyncQueryable<TSource> source, int count)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));

#if CRIPPLED_REFLECTION
            return source.Provider.CreateQuery<TSource>(Expression.Call(InfoOf(() => AsyncQueryable.Take<TSource>(default(IAsyncQueryable<TSource>), default(int))), source.Expression, Expression.Constant(count, typeof(int))));
#else
            return source.Provider.CreateQuery<TSource>(Expression.Call(((MethodInfo)MethodBase.GetCurrentMethod()).MakeGenericMethod(typeof(TSource)), source.Expression, Expression.Constant(count, typeof(int))));
#endif
        }

        public static IAsyncQueryable<TSource> TakeLast<TSource>(this IAsyncQueryable<TSource> source, int count)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));

#if CRIPPLED_REFLECTION
            return source.Provider.CreateQuery<TSource>(Expression.Call(InfoOf(() => AsyncQueryable.TakeLast<TSource>(default(IAsyncQueryable<TSource>), default(int))), source.Expression, Expression.Constant(count, typeof(int))));
#else
            return source.Provider.CreateQuery<TSource>(Expression.Call(((MethodInfo)MethodBase.GetCurrentMethod()).MakeGenericMethod(typeof(TSource)), source.Expression, Expression.Constant(count, typeof(int))));
#endif
        }

        public static IAsyncQueryable<TSource> TakeWhile<TSource>(this IAsyncQueryable<TSource> source, Expression<Func<TSource, bool>> predicate)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));
            if (predicate == null)
                throw new ArgumentNullException(nameof(predicate));

#if CRIPPLED_REFLECTION
            return source.Provider.CreateQuery<TSource>(Expression.Call(InfoOf(() => AsyncQueryable.TakeWhile<TSource>(default(IAsyncQueryable<TSource>), default(Expression<Func<TSource, bool>>))), source.Expression, predicate));
#else
            return source.Provider.CreateQuery<TSource>(Expression.Call(((MethodInfo)MethodBase.GetCurrentMethod()).MakeGenericMethod(typeof(TSource)), source.Expression, predicate));
#endif
        }

        public static IAsyncQueryable<TSource> TakeWhile<TSource>(this IAsyncQueryable<TSource> source, Expression<Func<TSource, int, bool>> predicate)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));
            if (predicate == null)
                throw new ArgumentNullException(nameof(predicate));

#if CRIPPLED_REFLECTION
            return source.Provider.CreateQuery<TSource>(Expression.Call(InfoOf(() => AsyncQueryable.TakeWhile<TSource>(default(IAsyncQueryable<TSource>), default(Expression<Func<TSource, int, bool>>))), source.Expression, predicate));
#else
            return source.Provider.CreateQuery<TSource>(Expression.Call(((MethodInfo)MethodBase.GetCurrentMethod()).MakeGenericMethod(typeof(TSource)), source.Expression, predicate));
#endif
        }

        public static IAsyncQueryable<TSource> TakeWhile<TSource>(this IAsyncQueryable<TSource> source, Expression<Func<TSource, int, Task<bool>>> predicate)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));
            if (predicate == null)
                throw new ArgumentNullException(nameof(predicate));

#if CRIPPLED_REFLECTION
            return source.Provider.CreateQuery<TSource>(Expression.Call(InfoOf(() => AsyncQueryable.TakeWhile<TSource>(default(IAsyncQueryable<TSource>), default(Expression<Func<TSource, int, Task<bool>>>))), source.Expression, predicate));
#else
            return source.Provider.CreateQuery<TSource>(Expression.Call(((MethodInfo)MethodBase.GetCurrentMethod()).MakeGenericMethod(typeof(TSource)), source.Expression, predicate));
#endif
        }

        public static IAsyncQueryable<TSource> TakeWhile<TSource>(this IAsyncQueryable<TSource> source, Expression<Func<TSource, Task<bool>>> predicate)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));
            if (predicate == null)
                throw new ArgumentNullException(nameof(predicate));

#if CRIPPLED_REFLECTION
            return source.Provider.CreateQuery<TSource>(Expression.Call(InfoOf(() => AsyncQueryable.TakeWhile<TSource>(default(IAsyncQueryable<TSource>), default(Expression<Func<TSource, Task<bool>>>))), source.Expression, predicate));
#else
            return source.Provider.CreateQuery<TSource>(Expression.Call(((MethodInfo)MethodBase.GetCurrentMethod()).MakeGenericMethod(typeof(TSource)), source.Expression, predicate));
#endif
        }

        public static IOrderedAsyncQueryable<TSource> ThenBy<TSource, TKey>(this IOrderedAsyncQueryable<TSource> source, Expression<Func<TSource, Task<TKey>>> keySelector)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));
            if (keySelector == null)
                throw new ArgumentNullException(nameof(keySelector));

#if CRIPPLED_REFLECTION
            return (IOrderedAsyncQueryable<TSource>)source.Provider.CreateQuery<TSource>(Expression.Call(InfoOf(() => AsyncQueryable.ThenBy<TSource, TKey>(default(IOrderedAsyncQueryable<TSource>), default(Expression<Func<TSource, Task<TKey>>>))), source.Expression, keySelector));
#else
            return (IOrderedAsyncQueryable<TSource>)source.Provider.CreateQuery<TSource>(Expression.Call(((MethodInfo)MethodBase.GetCurrentMethod()).MakeGenericMethod(typeof(TSource), typeof(TKey)), source.Expression, keySelector));
#endif
        }

        public static IOrderedAsyncQueryable<TSource> ThenBy<TSource, TKey>(this IOrderedAsyncQueryable<TSource> source, Expression<Func<TSource, TKey>> keySelector)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));
            if (keySelector == null)
                throw new ArgumentNullException(nameof(keySelector));

#if CRIPPLED_REFLECTION
            return (IOrderedAsyncQueryable<TSource>)source.Provider.CreateQuery<TSource>(Expression.Call(InfoOf(() => AsyncQueryable.ThenBy<TSource, TKey>(default(IOrderedAsyncQueryable<TSource>), default(Expression<Func<TSource, TKey>>))), source.Expression, keySelector));
#else
            return (IOrderedAsyncQueryable<TSource>)source.Provider.CreateQuery<TSource>(Expression.Call(((MethodInfo)MethodBase.GetCurrentMethod()).MakeGenericMethod(typeof(TSource), typeof(TKey)), source.Expression, keySelector));
#endif
        }

        public static IOrderedAsyncQueryable<TSource> ThenBy<TSource, TKey>(this IOrderedAsyncQueryable<TSource> source, Expression<Func<TSource, Task<TKey>>> keySelector, IComparer<TKey> comparer)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));
            if (keySelector == null)
                throw new ArgumentNullException(nameof(keySelector));
            if (comparer == null)
                throw new ArgumentNullException(nameof(comparer));

#if CRIPPLED_REFLECTION
            return (IOrderedAsyncQueryable<TSource>)source.Provider.CreateQuery<TSource>(Expression.Call(InfoOf(() => AsyncQueryable.ThenBy<TSource, TKey>(default(IOrderedAsyncQueryable<TSource>), default(Expression<Func<TSource, Task<TKey>>>), default(IComparer<TKey>))), source.Expression, keySelector, Expression.Constant(comparer, typeof(IComparer<TKey>))));
#else
            return (IOrderedAsyncQueryable<TSource>)source.Provider.CreateQuery<TSource>(Expression.Call(((MethodInfo)MethodBase.GetCurrentMethod()).MakeGenericMethod(typeof(TSource), typeof(TKey)), source.Expression, keySelector, Expression.Constant(comparer, typeof(IComparer<TKey>))));
#endif
        }

        public static IOrderedAsyncQueryable<TSource> ThenBy<TSource, TKey>(this IOrderedAsyncQueryable<TSource> source, Expression<Func<TSource, TKey>> keySelector, IComparer<TKey> comparer)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));
            if (keySelector == null)
                throw new ArgumentNullException(nameof(keySelector));
            if (comparer == null)
                throw new ArgumentNullException(nameof(comparer));

#if CRIPPLED_REFLECTION
            return (IOrderedAsyncQueryable<TSource>)source.Provider.CreateQuery<TSource>(Expression.Call(InfoOf(() => AsyncQueryable.ThenBy<TSource, TKey>(default(IOrderedAsyncQueryable<TSource>), default(Expression<Func<TSource, TKey>>), default(IComparer<TKey>))), source.Expression, keySelector, Expression.Constant(comparer, typeof(IComparer<TKey>))));
#else
            return (IOrderedAsyncQueryable<TSource>)source.Provider.CreateQuery<TSource>(Expression.Call(((MethodInfo)MethodBase.GetCurrentMethod()).MakeGenericMethod(typeof(TSource), typeof(TKey)), source.Expression, keySelector, Expression.Constant(comparer, typeof(IComparer<TKey>))));
#endif
        }

        public static IOrderedAsyncQueryable<TSource> ThenByDescending<TSource, TKey>(this IOrderedAsyncQueryable<TSource> source, Expression<Func<TSource, Task<TKey>>> keySelector)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));
            if (keySelector == null)
                throw new ArgumentNullException(nameof(keySelector));

#if CRIPPLED_REFLECTION
            return (IOrderedAsyncQueryable<TSource>)source.Provider.CreateQuery<TSource>(Expression.Call(InfoOf(() => AsyncQueryable.ThenByDescending<TSource, TKey>(default(IOrderedAsyncQueryable<TSource>), default(Expression<Func<TSource, Task<TKey>>>))), source.Expression, keySelector));
#else
            return (IOrderedAsyncQueryable<TSource>)source.Provider.CreateQuery<TSource>(Expression.Call(((MethodInfo)MethodBase.GetCurrentMethod()).MakeGenericMethod(typeof(TSource), typeof(TKey)), source.Expression, keySelector));
#endif
        }

        public static IOrderedAsyncQueryable<TSource> ThenByDescending<TSource, TKey>(this IOrderedAsyncQueryable<TSource> source, Expression<Func<TSource, TKey>> keySelector)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));
            if (keySelector == null)
                throw new ArgumentNullException(nameof(keySelector));

#if CRIPPLED_REFLECTION
            return (IOrderedAsyncQueryable<TSource>)source.Provider.CreateQuery<TSource>(Expression.Call(InfoOf(() => AsyncQueryable.ThenByDescending<TSource, TKey>(default(IOrderedAsyncQueryable<TSource>), default(Expression<Func<TSource, TKey>>))), source.Expression, keySelector));
#else
            return (IOrderedAsyncQueryable<TSource>)source.Provider.CreateQuery<TSource>(Expression.Call(((MethodInfo)MethodBase.GetCurrentMethod()).MakeGenericMethod(typeof(TSource), typeof(TKey)), source.Expression, keySelector));
#endif
        }

        public static IOrderedAsyncQueryable<TSource> ThenByDescending<TSource, TKey>(this IOrderedAsyncQueryable<TSource> source, Expression<Func<TSource, Task<TKey>>> keySelector, IComparer<TKey> comparer)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));
            if (keySelector == null)
                throw new ArgumentNullException(nameof(keySelector));
            if (comparer == null)
                throw new ArgumentNullException(nameof(comparer));

#if CRIPPLED_REFLECTION
            return (IOrderedAsyncQueryable<TSource>)source.Provider.CreateQuery<TSource>(Expression.Call(InfoOf(() => AsyncQueryable.ThenByDescending<TSource, TKey>(default(IOrderedAsyncQueryable<TSource>), default(Expression<Func<TSource, Task<TKey>>>), default(IComparer<TKey>))), source.Expression, keySelector, Expression.Constant(comparer, typeof(IComparer<TKey>))));
#else
            return (IOrderedAsyncQueryable<TSource>)source.Provider.CreateQuery<TSource>(Expression.Call(((MethodInfo)MethodBase.GetCurrentMethod()).MakeGenericMethod(typeof(TSource), typeof(TKey)), source.Expression, keySelector, Expression.Constant(comparer, typeof(IComparer<TKey>))));
#endif
        }

        public static IOrderedAsyncQueryable<TSource> ThenByDescending<TSource, TKey>(this IOrderedAsyncQueryable<TSource> source, Expression<Func<TSource, TKey>> keySelector, IComparer<TKey> comparer)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));
            if (keySelector == null)
                throw new ArgumentNullException(nameof(keySelector));
            if (comparer == null)
                throw new ArgumentNullException(nameof(comparer));

#if CRIPPLED_REFLECTION
            return (IOrderedAsyncQueryable<TSource>)source.Provider.CreateQuery<TSource>(Expression.Call(InfoOf(() => AsyncQueryable.ThenByDescending<TSource, TKey>(default(IOrderedAsyncQueryable<TSource>), default(Expression<Func<TSource, TKey>>), default(IComparer<TKey>))), source.Expression, keySelector, Expression.Constant(comparer, typeof(IComparer<TKey>))));
#else
            return (IOrderedAsyncQueryable<TSource>)source.Provider.CreateQuery<TSource>(Expression.Call(((MethodInfo)MethodBase.GetCurrentMethod()).MakeGenericMethod(typeof(TSource), typeof(TKey)), source.Expression, keySelector, Expression.Constant(comparer, typeof(IComparer<TKey>))));
#endif
        }

        public static Task<TSource[]> ToArray<TSource>(this IAsyncQueryable<TSource> source)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));

#if CRIPPLED_REFLECTION
            return source.Provider.ExecuteAsync<TSource[]>(Expression.Call(InfoOf(() => AsyncQueryable.ToArray<TSource>(default(IAsyncQueryable<TSource>))), source.Expression), CancellationToken.None);
#else
            return source.Provider.ExecuteAsync<TSource[]>(Expression.Call(((MethodInfo)MethodBase.GetCurrentMethod()).MakeGenericMethod(typeof(TSource)), source.Expression), CancellationToken.None);
#endif
        }

        public static Task<TSource[]> ToArray<TSource>(this IAsyncQueryable<TSource> source, CancellationToken cancellationToken)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));

#if CRIPPLED_REFLECTION
            return source.Provider.ExecuteAsync<TSource[]>(Expression.Call(InfoOf(() => AsyncQueryable.ToArray<TSource>(default(IAsyncQueryable<TSource>), default(CancellationToken))), source.Expression, Expression.Constant(cancellationToken, typeof(CancellationToken))), cancellationToken);
#else
            return source.Provider.ExecuteAsync<TSource[]>(Expression.Call(((MethodInfo)MethodBase.GetCurrentMethod()).MakeGenericMethod(typeof(TSource)), source.Expression, Expression.Constant(cancellationToken, typeof(CancellationToken))), cancellationToken);
#endif
        }

        public static Task<Dictionary<TKey, TSource>> ToDictionary<TSource, TKey>(this IAsyncQueryable<TSource> source, Expression<Func<TSource, Task<TKey>>> keySelector)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));
            if (keySelector == null)
                throw new ArgumentNullException(nameof(keySelector));

#if CRIPPLED_REFLECTION
            return source.Provider.ExecuteAsync<Dictionary<TKey, TSource>>(Expression.Call(InfoOf(() => AsyncQueryable.ToDictionary<TSource, TKey>(default(IAsyncQueryable<TSource>), default(Expression<Func<TSource, Task<TKey>>>))), source.Expression, keySelector), CancellationToken.None);
#else
            return source.Provider.ExecuteAsync<Dictionary<TKey, TSource>>(Expression.Call(((MethodInfo)MethodBase.GetCurrentMethod()).MakeGenericMethod(typeof(TSource), typeof(TKey)), source.Expression, keySelector), CancellationToken.None);
#endif
        }

        public static Task<Dictionary<TKey, TSource>> ToDictionary<TSource, TKey>(this IAsyncQueryable<TSource> source, Expression<Func<TSource, TKey>> keySelector)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));
            if (keySelector == null)
                throw new ArgumentNullException(nameof(keySelector));

#if CRIPPLED_REFLECTION
            return source.Provider.ExecuteAsync<Dictionary<TKey, TSource>>(Expression.Call(InfoOf(() => AsyncQueryable.ToDictionary<TSource, TKey>(default(IAsyncQueryable<TSource>), default(Expression<Func<TSource, TKey>>))), source.Expression, keySelector), CancellationToken.None);
#else
            return source.Provider.ExecuteAsync<Dictionary<TKey, TSource>>(Expression.Call(((MethodInfo)MethodBase.GetCurrentMethod()).MakeGenericMethod(typeof(TSource), typeof(TKey)), source.Expression, keySelector), CancellationToken.None);
#endif
        }

        public static Task<Dictionary<TKey, TSource>> ToDictionary<TSource, TKey>(this IAsyncQueryable<TSource> source, Expression<Func<TSource, Task<TKey>>> keySelector, CancellationToken cancellationToken)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));
            if (keySelector == null)
                throw new ArgumentNullException(nameof(keySelector));

#if CRIPPLED_REFLECTION
            return source.Provider.ExecuteAsync<Dictionary<TKey, TSource>>(Expression.Call(InfoOf(() => AsyncQueryable.ToDictionary<TSource, TKey>(default(IAsyncQueryable<TSource>), default(Expression<Func<TSource, Task<TKey>>>), default(CancellationToken))), source.Expression, keySelector, Expression.Constant(cancellationToken, typeof(CancellationToken))), cancellationToken);
#else
            return source.Provider.ExecuteAsync<Dictionary<TKey, TSource>>(Expression.Call(((MethodInfo)MethodBase.GetCurrentMethod()).MakeGenericMethod(typeof(TSource), typeof(TKey)), source.Expression, keySelector, Expression.Constant(cancellationToken, typeof(CancellationToken))), cancellationToken);
#endif
        }

        public static Task<Dictionary<TKey, TSource>> ToDictionary<TSource, TKey>(this IAsyncQueryable<TSource> source, Expression<Func<TSource, Task<TKey>>> keySelector, IEqualityComparer<TKey> comparer)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));
            if (keySelector == null)
                throw new ArgumentNullException(nameof(keySelector));
            if (comparer == null)
                throw new ArgumentNullException(nameof(comparer));

#if CRIPPLED_REFLECTION
            return source.Provider.ExecuteAsync<Dictionary<TKey, TSource>>(Expression.Call(InfoOf(() => AsyncQueryable.ToDictionary<TSource, TKey>(default(IAsyncQueryable<TSource>), default(Expression<Func<TSource, Task<TKey>>>), default(IEqualityComparer<TKey>))), source.Expression, keySelector, Expression.Constant(comparer, typeof(IEqualityComparer<TKey>))), CancellationToken.None);
#else
            return source.Provider.ExecuteAsync<Dictionary<TKey, TSource>>(Expression.Call(((MethodInfo)MethodBase.GetCurrentMethod()).MakeGenericMethod(typeof(TSource), typeof(TKey)), source.Expression, keySelector, Expression.Constant(comparer, typeof(IEqualityComparer<TKey>))), CancellationToken.None);
#endif
        }

        public static Task<Dictionary<TKey, TSource>> ToDictionary<TSource, TKey>(this IAsyncQueryable<TSource> source, Expression<Func<TSource, TKey>> keySelector, CancellationToken cancellationToken)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));
            if (keySelector == null)
                throw new ArgumentNullException(nameof(keySelector));

#if CRIPPLED_REFLECTION
            return source.Provider.ExecuteAsync<Dictionary<TKey, TSource>>(Expression.Call(InfoOf(() => AsyncQueryable.ToDictionary<TSource, TKey>(default(IAsyncQueryable<TSource>), default(Expression<Func<TSource, TKey>>), default(CancellationToken))), source.Expression, keySelector, Expression.Constant(cancellationToken, typeof(CancellationToken))), cancellationToken);
#else
            return source.Provider.ExecuteAsync<Dictionary<TKey, TSource>>(Expression.Call(((MethodInfo)MethodBase.GetCurrentMethod()).MakeGenericMethod(typeof(TSource), typeof(TKey)), source.Expression, keySelector, Expression.Constant(cancellationToken, typeof(CancellationToken))), cancellationToken);
#endif
        }

        public static Task<Dictionary<TKey, TSource>> ToDictionary<TSource, TKey>(this IAsyncQueryable<TSource> source, Expression<Func<TSource, TKey>> keySelector, IEqualityComparer<TKey> comparer)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));
            if (keySelector == null)
                throw new ArgumentNullException(nameof(keySelector));
            if (comparer == null)
                throw new ArgumentNullException(nameof(comparer));

#if CRIPPLED_REFLECTION
            return source.Provider.ExecuteAsync<Dictionary<TKey, TSource>>(Expression.Call(InfoOf(() => AsyncQueryable.ToDictionary<TSource, TKey>(default(IAsyncQueryable<TSource>), default(Expression<Func<TSource, TKey>>), default(IEqualityComparer<TKey>))), source.Expression, keySelector, Expression.Constant(comparer, typeof(IEqualityComparer<TKey>))), CancellationToken.None);
#else
            return source.Provider.ExecuteAsync<Dictionary<TKey, TSource>>(Expression.Call(((MethodInfo)MethodBase.GetCurrentMethod()).MakeGenericMethod(typeof(TSource), typeof(TKey)), source.Expression, keySelector, Expression.Constant(comparer, typeof(IEqualityComparer<TKey>))), CancellationToken.None);
#endif
        }

        public static Task<Dictionary<TKey, TSource>> ToDictionary<TSource, TKey>(this IAsyncQueryable<TSource> source, Expression<Func<TSource, Task<TKey>>> keySelector, IEqualityComparer<TKey> comparer, CancellationToken cancellationToken)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));
            if (keySelector == null)
                throw new ArgumentNullException(nameof(keySelector));
            if (comparer == null)
                throw new ArgumentNullException(nameof(comparer));

#if CRIPPLED_REFLECTION
            return source.Provider.ExecuteAsync<Dictionary<TKey, TSource>>(Expression.Call(InfoOf(() => AsyncQueryable.ToDictionary<TSource, TKey>(default(IAsyncQueryable<TSource>), default(Expression<Func<TSource, Task<TKey>>>), default(IEqualityComparer<TKey>), default(CancellationToken))), source.Expression, keySelector, Expression.Constant(comparer, typeof(IEqualityComparer<TKey>)), Expression.Constant(cancellationToken, typeof(CancellationToken))), cancellationToken);
#else
            return source.Provider.ExecuteAsync<Dictionary<TKey, TSource>>(Expression.Call(((MethodInfo)MethodBase.GetCurrentMethod()).MakeGenericMethod(typeof(TSource), typeof(TKey)), source.Expression, keySelector, Expression.Constant(comparer, typeof(IEqualityComparer<TKey>)), Expression.Constant(cancellationToken, typeof(CancellationToken))), cancellationToken);
#endif
        }

        public static Task<Dictionary<TKey, TSource>> ToDictionary<TSource, TKey>(this IAsyncQueryable<TSource> source, Expression<Func<TSource, TKey>> keySelector, IEqualityComparer<TKey> comparer, CancellationToken cancellationToken)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));
            if (keySelector == null)
                throw new ArgumentNullException(nameof(keySelector));
            if (comparer == null)
                throw new ArgumentNullException(nameof(comparer));

#if CRIPPLED_REFLECTION
            return source.Provider.ExecuteAsync<Dictionary<TKey, TSource>>(Expression.Call(InfoOf(() => AsyncQueryable.ToDictionary<TSource, TKey>(default(IAsyncQueryable<TSource>), default(Expression<Func<TSource, TKey>>), default(IEqualityComparer<TKey>), default(CancellationToken))), source.Expression, keySelector, Expression.Constant(comparer, typeof(IEqualityComparer<TKey>)), Expression.Constant(cancellationToken, typeof(CancellationToken))), cancellationToken);
#else
            return source.Provider.ExecuteAsync<Dictionary<TKey, TSource>>(Expression.Call(((MethodInfo)MethodBase.GetCurrentMethod()).MakeGenericMethod(typeof(TSource), typeof(TKey)), source.Expression, keySelector, Expression.Constant(comparer, typeof(IEqualityComparer<TKey>)), Expression.Constant(cancellationToken, typeof(CancellationToken))), cancellationToken);
#endif
        }

        public static Task<Dictionary<TKey, TElement>> ToDictionary<TSource, TKey, TElement>(this IAsyncQueryable<TSource> source, Expression<Func<TSource, Task<TKey>>> keySelector, Expression<Func<TSource, Task<TElement>>> elementSelector)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));
            if (keySelector == null)
                throw new ArgumentNullException(nameof(keySelector));
            if (elementSelector == null)
                throw new ArgumentNullException(nameof(elementSelector));

#if CRIPPLED_REFLECTION
            return source.Provider.ExecuteAsync<Dictionary<TKey, TElement>>(Expression.Call(InfoOf(() => AsyncQueryable.ToDictionary<TSource, TKey, TElement>(default(IAsyncQueryable<TSource>), default(Expression<Func<TSource, Task<TKey>>>), default(Expression<Func<TSource, Task<TElement>>>))), source.Expression, keySelector, elementSelector), CancellationToken.None);
#else
            return source.Provider.ExecuteAsync<Dictionary<TKey, TElement>>(Expression.Call(((MethodInfo)MethodBase.GetCurrentMethod()).MakeGenericMethod(typeof(TSource), typeof(TKey), typeof(TElement)), source.Expression, keySelector, elementSelector), CancellationToken.None);
#endif
        }

        public static Task<Dictionary<TKey, TElement>> ToDictionary<TSource, TKey, TElement>(this IAsyncQueryable<TSource> source, Expression<Func<TSource, TKey>> keySelector, Expression<Func<TSource, TElement>> elementSelector)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));
            if (keySelector == null)
                throw new ArgumentNullException(nameof(keySelector));
            if (elementSelector == null)
                throw new ArgumentNullException(nameof(elementSelector));

#if CRIPPLED_REFLECTION
            return source.Provider.ExecuteAsync<Dictionary<TKey, TElement>>(Expression.Call(InfoOf(() => AsyncQueryable.ToDictionary<TSource, TKey, TElement>(default(IAsyncQueryable<TSource>), default(Expression<Func<TSource, TKey>>), default(Expression<Func<TSource, TElement>>))), source.Expression, keySelector, elementSelector), CancellationToken.None);
#else
            return source.Provider.ExecuteAsync<Dictionary<TKey, TElement>>(Expression.Call(((MethodInfo)MethodBase.GetCurrentMethod()).MakeGenericMethod(typeof(TSource), typeof(TKey), typeof(TElement)), source.Expression, keySelector, elementSelector), CancellationToken.None);
#endif
        }

        public static Task<Dictionary<TKey, TElement>> ToDictionary<TSource, TKey, TElement>(this IAsyncQueryable<TSource> source, Expression<Func<TSource, Task<TKey>>> keySelector, Expression<Func<TSource, Task<TElement>>> elementSelector, CancellationToken cancellationToken)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));
            if (keySelector == null)
                throw new ArgumentNullException(nameof(keySelector));
            if (elementSelector == null)
                throw new ArgumentNullException(nameof(elementSelector));

#if CRIPPLED_REFLECTION
            return source.Provider.ExecuteAsync<Dictionary<TKey, TElement>>(Expression.Call(InfoOf(() => AsyncQueryable.ToDictionary<TSource, TKey, TElement>(default(IAsyncQueryable<TSource>), default(Expression<Func<TSource, Task<TKey>>>), default(Expression<Func<TSource, Task<TElement>>>), default(CancellationToken))), source.Expression, keySelector, elementSelector, Expression.Constant(cancellationToken, typeof(CancellationToken))), cancellationToken);
#else
            return source.Provider.ExecuteAsync<Dictionary<TKey, TElement>>(Expression.Call(((MethodInfo)MethodBase.GetCurrentMethod()).MakeGenericMethod(typeof(TSource), typeof(TKey), typeof(TElement)), source.Expression, keySelector, elementSelector, Expression.Constant(cancellationToken, typeof(CancellationToken))), cancellationToken);
#endif
        }

        public static Task<Dictionary<TKey, TElement>> ToDictionary<TSource, TKey, TElement>(this IAsyncQueryable<TSource> source, Expression<Func<TSource, Task<TKey>>> keySelector, Expression<Func<TSource, Task<TElement>>> elementSelector, IEqualityComparer<TKey> comparer)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));
            if (keySelector == null)
                throw new ArgumentNullException(nameof(keySelector));
            if (elementSelector == null)
                throw new ArgumentNullException(nameof(elementSelector));
            if (comparer == null)
                throw new ArgumentNullException(nameof(comparer));

#if CRIPPLED_REFLECTION
            return source.Provider.ExecuteAsync<Dictionary<TKey, TElement>>(Expression.Call(InfoOf(() => AsyncQueryable.ToDictionary<TSource, TKey, TElement>(default(IAsyncQueryable<TSource>), default(Expression<Func<TSource, Task<TKey>>>), default(Expression<Func<TSource, Task<TElement>>>), default(IEqualityComparer<TKey>))), source.Expression, keySelector, elementSelector, Expression.Constant(comparer, typeof(IEqualityComparer<TKey>))), CancellationToken.None);
#else
            return source.Provider.ExecuteAsync<Dictionary<TKey, TElement>>(Expression.Call(((MethodInfo)MethodBase.GetCurrentMethod()).MakeGenericMethod(typeof(TSource), typeof(TKey), typeof(TElement)), source.Expression, keySelector, elementSelector, Expression.Constant(comparer, typeof(IEqualityComparer<TKey>))), CancellationToken.None);
#endif
        }

        public static Task<Dictionary<TKey, TElement>> ToDictionary<TSource, TKey, TElement>(this IAsyncQueryable<TSource> source, Expression<Func<TSource, TKey>> keySelector, Expression<Func<TSource, TElement>> elementSelector, CancellationToken cancellationToken)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));
            if (keySelector == null)
                throw new ArgumentNullException(nameof(keySelector));
            if (elementSelector == null)
                throw new ArgumentNullException(nameof(elementSelector));

#if CRIPPLED_REFLECTION
            return source.Provider.ExecuteAsync<Dictionary<TKey, TElement>>(Expression.Call(InfoOf(() => AsyncQueryable.ToDictionary<TSource, TKey, TElement>(default(IAsyncQueryable<TSource>), default(Expression<Func<TSource, TKey>>), default(Expression<Func<TSource, TElement>>), default(CancellationToken))), source.Expression, keySelector, elementSelector, Expression.Constant(cancellationToken, typeof(CancellationToken))), cancellationToken);
#else
            return source.Provider.ExecuteAsync<Dictionary<TKey, TElement>>(Expression.Call(((MethodInfo)MethodBase.GetCurrentMethod()).MakeGenericMethod(typeof(TSource), typeof(TKey), typeof(TElement)), source.Expression, keySelector, elementSelector, Expression.Constant(cancellationToken, typeof(CancellationToken))), cancellationToken);
#endif
        }

        public static Task<Dictionary<TKey, TElement>> ToDictionary<TSource, TKey, TElement>(this IAsyncQueryable<TSource> source, Expression<Func<TSource, TKey>> keySelector, Expression<Func<TSource, TElement>> elementSelector, IEqualityComparer<TKey> comparer)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));
            if (keySelector == null)
                throw new ArgumentNullException(nameof(keySelector));
            if (elementSelector == null)
                throw new ArgumentNullException(nameof(elementSelector));
            if (comparer == null)
                throw new ArgumentNullException(nameof(comparer));

#if CRIPPLED_REFLECTION
            return source.Provider.ExecuteAsync<Dictionary<TKey, TElement>>(Expression.Call(InfoOf(() => AsyncQueryable.ToDictionary<TSource, TKey, TElement>(default(IAsyncQueryable<TSource>), default(Expression<Func<TSource, TKey>>), default(Expression<Func<TSource, TElement>>), default(IEqualityComparer<TKey>))), source.Expression, keySelector, elementSelector, Expression.Constant(comparer, typeof(IEqualityComparer<TKey>))), CancellationToken.None);
#else
            return source.Provider.ExecuteAsync<Dictionary<TKey, TElement>>(Expression.Call(((MethodInfo)MethodBase.GetCurrentMethod()).MakeGenericMethod(typeof(TSource), typeof(TKey), typeof(TElement)), source.Expression, keySelector, elementSelector, Expression.Constant(comparer, typeof(IEqualityComparer<TKey>))), CancellationToken.None);
#endif
        }

        public static Task<Dictionary<TKey, TElement>> ToDictionary<TSource, TKey, TElement>(this IAsyncQueryable<TSource> source, Expression<Func<TSource, Task<TKey>>> keySelector, Expression<Func<TSource, Task<TElement>>> elementSelector, IEqualityComparer<TKey> comparer, CancellationToken cancellationToken)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));
            if (keySelector == null)
                throw new ArgumentNullException(nameof(keySelector));
            if (elementSelector == null)
                throw new ArgumentNullException(nameof(elementSelector));
            if (comparer == null)
                throw new ArgumentNullException(nameof(comparer));

#if CRIPPLED_REFLECTION
            return source.Provider.ExecuteAsync<Dictionary<TKey, TElement>>(Expression.Call(InfoOf(() => AsyncQueryable.ToDictionary<TSource, TKey, TElement>(default(IAsyncQueryable<TSource>), default(Expression<Func<TSource, Task<TKey>>>), default(Expression<Func<TSource, Task<TElement>>>), default(IEqualityComparer<TKey>), default(CancellationToken))), source.Expression, keySelector, elementSelector, Expression.Constant(comparer, typeof(IEqualityComparer<TKey>)), Expression.Constant(cancellationToken, typeof(CancellationToken))), cancellationToken);
#else
            return source.Provider.ExecuteAsync<Dictionary<TKey, TElement>>(Expression.Call(((MethodInfo)MethodBase.GetCurrentMethod()).MakeGenericMethod(typeof(TSource), typeof(TKey), typeof(TElement)), source.Expression, keySelector, elementSelector, Expression.Constant(comparer, typeof(IEqualityComparer<TKey>)), Expression.Constant(cancellationToken, typeof(CancellationToken))), cancellationToken);
#endif
        }

        public static Task<Dictionary<TKey, TElement>> ToDictionary<TSource, TKey, TElement>(this IAsyncQueryable<TSource> source, Expression<Func<TSource, TKey>> keySelector, Expression<Func<TSource, TElement>> elementSelector, IEqualityComparer<TKey> comparer, CancellationToken cancellationToken)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));
            if (keySelector == null)
                throw new ArgumentNullException(nameof(keySelector));
            if (elementSelector == null)
                throw new ArgumentNullException(nameof(elementSelector));
            if (comparer == null)
                throw new ArgumentNullException(nameof(comparer));

#if CRIPPLED_REFLECTION
            return source.Provider.ExecuteAsync<Dictionary<TKey, TElement>>(Expression.Call(InfoOf(() => AsyncQueryable.ToDictionary<TSource, TKey, TElement>(default(IAsyncQueryable<TSource>), default(Expression<Func<TSource, TKey>>), default(Expression<Func<TSource, TElement>>), default(IEqualityComparer<TKey>), default(CancellationToken))), source.Expression, keySelector, elementSelector, Expression.Constant(comparer, typeof(IEqualityComparer<TKey>)), Expression.Constant(cancellationToken, typeof(CancellationToken))), cancellationToken);
#else
            return source.Provider.ExecuteAsync<Dictionary<TKey, TElement>>(Expression.Call(((MethodInfo)MethodBase.GetCurrentMethod()).MakeGenericMethod(typeof(TSource), typeof(TKey), typeof(TElement)), source.Expression, keySelector, elementSelector, Expression.Constant(comparer, typeof(IEqualityComparer<TKey>)), Expression.Constant(cancellationToken, typeof(CancellationToken))), cancellationToken);
#endif
        }

        public static Task<HashSet<TSource>> ToHashSet<TSource>(this IAsyncQueryable<TSource> source)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));

#if CRIPPLED_REFLECTION
            return source.Provider.ExecuteAsync<HashSet<TSource>>(Expression.Call(InfoOf(() => AsyncQueryable.ToHashSet<TSource>(default(IAsyncQueryable<TSource>))), source.Expression), CancellationToken.None);
#else
            return source.Provider.ExecuteAsync<HashSet<TSource>>(Expression.Call(((MethodInfo)MethodBase.GetCurrentMethod()).MakeGenericMethod(typeof(TSource)), source.Expression), CancellationToken.None);
#endif
        }

        public static Task<HashSet<TSource>> ToHashSet<TSource>(this IAsyncQueryable<TSource> source, CancellationToken cancellationToken)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));

#if CRIPPLED_REFLECTION
            return source.Provider.ExecuteAsync<HashSet<TSource>>(Expression.Call(InfoOf(() => AsyncQueryable.ToHashSet<TSource>(default(IAsyncQueryable<TSource>), default(CancellationToken))), source.Expression, Expression.Constant(cancellationToken, typeof(CancellationToken))), cancellationToken);
#else
            return source.Provider.ExecuteAsync<HashSet<TSource>>(Expression.Call(((MethodInfo)MethodBase.GetCurrentMethod()).MakeGenericMethod(typeof(TSource)), source.Expression, Expression.Constant(cancellationToken, typeof(CancellationToken))), cancellationToken);
#endif
        }

        public static Task<HashSet<TSource>> ToHashSet<TSource>(this IAsyncQueryable<TSource> source, IEqualityComparer<TSource> comparer)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));
            if (comparer == null)
                throw new ArgumentNullException(nameof(comparer));

#if CRIPPLED_REFLECTION
            return source.Provider.ExecuteAsync<HashSet<TSource>>(Expression.Call(InfoOf(() => AsyncQueryable.ToHashSet<TSource>(default(IAsyncQueryable<TSource>), default(IEqualityComparer<TSource>))), source.Expression, Expression.Constant(comparer, typeof(IEqualityComparer<TSource>))), CancellationToken.None);
#else
            return source.Provider.ExecuteAsync<HashSet<TSource>>(Expression.Call(((MethodInfo)MethodBase.GetCurrentMethod()).MakeGenericMethod(typeof(TSource)), source.Expression, Expression.Constant(comparer, typeof(IEqualityComparer<TSource>))), CancellationToken.None);
#endif
        }

        public static Task<HashSet<TSource>> ToHashSet<TSource>(this IAsyncQueryable<TSource> source, IEqualityComparer<TSource> comparer, CancellationToken cancellationToken)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));
            if (comparer == null)
                throw new ArgumentNullException(nameof(comparer));

#if CRIPPLED_REFLECTION
            return source.Provider.ExecuteAsync<HashSet<TSource>>(Expression.Call(InfoOf(() => AsyncQueryable.ToHashSet<TSource>(default(IAsyncQueryable<TSource>), default(IEqualityComparer<TSource>), default(CancellationToken))), source.Expression, Expression.Constant(comparer, typeof(IEqualityComparer<TSource>)), Expression.Constant(cancellationToken, typeof(CancellationToken))), cancellationToken);
#else
            return source.Provider.ExecuteAsync<HashSet<TSource>>(Expression.Call(((MethodInfo)MethodBase.GetCurrentMethod()).MakeGenericMethod(typeof(TSource)), source.Expression, Expression.Constant(comparer, typeof(IEqualityComparer<TSource>)), Expression.Constant(cancellationToken, typeof(CancellationToken))), cancellationToken);
#endif
        }

        public static Task<List<TSource>> ToList<TSource>(this IAsyncQueryable<TSource> source)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));

#if CRIPPLED_REFLECTION
            return source.Provider.ExecuteAsync<List<TSource>>(Expression.Call(InfoOf(() => AsyncQueryable.ToList<TSource>(default(IAsyncQueryable<TSource>))), source.Expression), CancellationToken.None);
#else
            return source.Provider.ExecuteAsync<List<TSource>>(Expression.Call(((MethodInfo)MethodBase.GetCurrentMethod()).MakeGenericMethod(typeof(TSource)), source.Expression), CancellationToken.None);
#endif
        }

        public static Task<List<TSource>> ToList<TSource>(this IAsyncQueryable<TSource> source, CancellationToken cancellationToken)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));

#if CRIPPLED_REFLECTION
            return source.Provider.ExecuteAsync<List<TSource>>(Expression.Call(InfoOf(() => AsyncQueryable.ToList<TSource>(default(IAsyncQueryable<TSource>), default(CancellationToken))), source.Expression, Expression.Constant(cancellationToken, typeof(CancellationToken))), cancellationToken);
#else
            return source.Provider.ExecuteAsync<List<TSource>>(Expression.Call(((MethodInfo)MethodBase.GetCurrentMethod()).MakeGenericMethod(typeof(TSource)), source.Expression, Expression.Constant(cancellationToken, typeof(CancellationToken))), cancellationToken);
#endif
        }

        public static Task<ILookup<TKey, TSource>> ToLookup<TSource, TKey>(this IAsyncQueryable<TSource> source, Expression<Func<TSource, Task<TKey>>> keySelector)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));
            if (keySelector == null)
                throw new ArgumentNullException(nameof(keySelector));

#if CRIPPLED_REFLECTION
            return source.Provider.ExecuteAsync<ILookup<TKey, TSource>>(Expression.Call(InfoOf(() => AsyncQueryable.ToLookup<TSource, TKey>(default(IAsyncQueryable<TSource>), default(Expression<Func<TSource, Task<TKey>>>))), source.Expression, keySelector), CancellationToken.None);
#else
            return source.Provider.ExecuteAsync<ILookup<TKey, TSource>>(Expression.Call(((MethodInfo)MethodBase.GetCurrentMethod()).MakeGenericMethod(typeof(TSource), typeof(TKey)), source.Expression, keySelector), CancellationToken.None);
#endif
        }

        public static Task<ILookup<TKey, TSource>> ToLookup<TSource, TKey>(this IAsyncQueryable<TSource> source, Expression<Func<TSource, TKey>> keySelector)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));
            if (keySelector == null)
                throw new ArgumentNullException(nameof(keySelector));

#if CRIPPLED_REFLECTION
            return source.Provider.ExecuteAsync<ILookup<TKey, TSource>>(Expression.Call(InfoOf(() => AsyncQueryable.ToLookup<TSource, TKey>(default(IAsyncQueryable<TSource>), default(Expression<Func<TSource, TKey>>))), source.Expression, keySelector), CancellationToken.None);
#else
            return source.Provider.ExecuteAsync<ILookup<TKey, TSource>>(Expression.Call(((MethodInfo)MethodBase.GetCurrentMethod()).MakeGenericMethod(typeof(TSource), typeof(TKey)), source.Expression, keySelector), CancellationToken.None);
#endif
        }

        public static Task<ILookup<TKey, TSource>> ToLookup<TSource, TKey>(this IAsyncQueryable<TSource> source, Expression<Func<TSource, Task<TKey>>> keySelector, CancellationToken cancellationToken)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));
            if (keySelector == null)
                throw new ArgumentNullException(nameof(keySelector));

#if CRIPPLED_REFLECTION
            return source.Provider.ExecuteAsync<ILookup<TKey, TSource>>(Expression.Call(InfoOf(() => AsyncQueryable.ToLookup<TSource, TKey>(default(IAsyncQueryable<TSource>), default(Expression<Func<TSource, Task<TKey>>>), default(CancellationToken))), source.Expression, keySelector, Expression.Constant(cancellationToken, typeof(CancellationToken))), cancellationToken);
#else
            return source.Provider.ExecuteAsync<ILookup<TKey, TSource>>(Expression.Call(((MethodInfo)MethodBase.GetCurrentMethod()).MakeGenericMethod(typeof(TSource), typeof(TKey)), source.Expression, keySelector, Expression.Constant(cancellationToken, typeof(CancellationToken))), cancellationToken);
#endif
        }

        public static Task<ILookup<TKey, TSource>> ToLookup<TSource, TKey>(this IAsyncQueryable<TSource> source, Expression<Func<TSource, Task<TKey>>> keySelector, IEqualityComparer<TKey> comparer)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));
            if (keySelector == null)
                throw new ArgumentNullException(nameof(keySelector));
            if (comparer == null)
                throw new ArgumentNullException(nameof(comparer));

#if CRIPPLED_REFLECTION
            return source.Provider.ExecuteAsync<ILookup<TKey, TSource>>(Expression.Call(InfoOf(() => AsyncQueryable.ToLookup<TSource, TKey>(default(IAsyncQueryable<TSource>), default(Expression<Func<TSource, Task<TKey>>>), default(IEqualityComparer<TKey>))), source.Expression, keySelector, Expression.Constant(comparer, typeof(IEqualityComparer<TKey>))), CancellationToken.None);
#else
            return source.Provider.ExecuteAsync<ILookup<TKey, TSource>>(Expression.Call(((MethodInfo)MethodBase.GetCurrentMethod()).MakeGenericMethod(typeof(TSource), typeof(TKey)), source.Expression, keySelector, Expression.Constant(comparer, typeof(IEqualityComparer<TKey>))), CancellationToken.None);
#endif
        }

        public static Task<ILookup<TKey, TSource>> ToLookup<TSource, TKey>(this IAsyncQueryable<TSource> source, Expression<Func<TSource, TKey>> keySelector, CancellationToken cancellationToken)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));
            if (keySelector == null)
                throw new ArgumentNullException(nameof(keySelector));

#if CRIPPLED_REFLECTION
            return source.Provider.ExecuteAsync<ILookup<TKey, TSource>>(Expression.Call(InfoOf(() => AsyncQueryable.ToLookup<TSource, TKey>(default(IAsyncQueryable<TSource>), default(Expression<Func<TSource, TKey>>), default(CancellationToken))), source.Expression, keySelector, Expression.Constant(cancellationToken, typeof(CancellationToken))), cancellationToken);
#else
            return source.Provider.ExecuteAsync<ILookup<TKey, TSource>>(Expression.Call(((MethodInfo)MethodBase.GetCurrentMethod()).MakeGenericMethod(typeof(TSource), typeof(TKey)), source.Expression, keySelector, Expression.Constant(cancellationToken, typeof(CancellationToken))), cancellationToken);
#endif
        }

        public static Task<ILookup<TKey, TSource>> ToLookup<TSource, TKey>(this IAsyncQueryable<TSource> source, Expression<Func<TSource, TKey>> keySelector, IEqualityComparer<TKey> comparer)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));
            if (keySelector == null)
                throw new ArgumentNullException(nameof(keySelector));
            if (comparer == null)
                throw new ArgumentNullException(nameof(comparer));

#if CRIPPLED_REFLECTION
            return source.Provider.ExecuteAsync<ILookup<TKey, TSource>>(Expression.Call(InfoOf(() => AsyncQueryable.ToLookup<TSource, TKey>(default(IAsyncQueryable<TSource>), default(Expression<Func<TSource, TKey>>), default(IEqualityComparer<TKey>))), source.Expression, keySelector, Expression.Constant(comparer, typeof(IEqualityComparer<TKey>))), CancellationToken.None);
#else
            return source.Provider.ExecuteAsync<ILookup<TKey, TSource>>(Expression.Call(((MethodInfo)MethodBase.GetCurrentMethod()).MakeGenericMethod(typeof(TSource), typeof(TKey)), source.Expression, keySelector, Expression.Constant(comparer, typeof(IEqualityComparer<TKey>))), CancellationToken.None);
#endif
        }

        public static Task<ILookup<TKey, TSource>> ToLookup<TSource, TKey>(this IAsyncQueryable<TSource> source, Expression<Func<TSource, Task<TKey>>> keySelector, IEqualityComparer<TKey> comparer, CancellationToken cancellationToken)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));
            if (keySelector == null)
                throw new ArgumentNullException(nameof(keySelector));
            if (comparer == null)
                throw new ArgumentNullException(nameof(comparer));

#if CRIPPLED_REFLECTION
            return source.Provider.ExecuteAsync<ILookup<TKey, TSource>>(Expression.Call(InfoOf(() => AsyncQueryable.ToLookup<TSource, TKey>(default(IAsyncQueryable<TSource>), default(Expression<Func<TSource, Task<TKey>>>), default(IEqualityComparer<TKey>), default(CancellationToken))), source.Expression, keySelector, Expression.Constant(comparer, typeof(IEqualityComparer<TKey>)), Expression.Constant(cancellationToken, typeof(CancellationToken))), cancellationToken);
#else
            return source.Provider.ExecuteAsync<ILookup<TKey, TSource>>(Expression.Call(((MethodInfo)MethodBase.GetCurrentMethod()).MakeGenericMethod(typeof(TSource), typeof(TKey)), source.Expression, keySelector, Expression.Constant(comparer, typeof(IEqualityComparer<TKey>)), Expression.Constant(cancellationToken, typeof(CancellationToken))), cancellationToken);
#endif
        }

        public static Task<ILookup<TKey, TSource>> ToLookup<TSource, TKey>(this IAsyncQueryable<TSource> source, Expression<Func<TSource, TKey>> keySelector, IEqualityComparer<TKey> comparer, CancellationToken cancellationToken)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));
            if (keySelector == null)
                throw new ArgumentNullException(nameof(keySelector));
            if (comparer == null)
                throw new ArgumentNullException(nameof(comparer));

#if CRIPPLED_REFLECTION
            return source.Provider.ExecuteAsync<ILookup<TKey, TSource>>(Expression.Call(InfoOf(() => AsyncQueryable.ToLookup<TSource, TKey>(default(IAsyncQueryable<TSource>), default(Expression<Func<TSource, TKey>>), default(IEqualityComparer<TKey>), default(CancellationToken))), source.Expression, keySelector, Expression.Constant(comparer, typeof(IEqualityComparer<TKey>)), Expression.Constant(cancellationToken, typeof(CancellationToken))), cancellationToken);
#else
            return source.Provider.ExecuteAsync<ILookup<TKey, TSource>>(Expression.Call(((MethodInfo)MethodBase.GetCurrentMethod()).MakeGenericMethod(typeof(TSource), typeof(TKey)), source.Expression, keySelector, Expression.Constant(comparer, typeof(IEqualityComparer<TKey>)), Expression.Constant(cancellationToken, typeof(CancellationToken))), cancellationToken);
#endif
        }

        public static Task<ILookup<TKey, TElement>> ToLookup<TSource, TKey, TElement>(this IAsyncQueryable<TSource> source, Expression<Func<TSource, Task<TKey>>> keySelector, Expression<Func<TSource, Task<TElement>>> elementSelector)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));
            if (keySelector == null)
                throw new ArgumentNullException(nameof(keySelector));
            if (elementSelector == null)
                throw new ArgumentNullException(nameof(elementSelector));

#if CRIPPLED_REFLECTION
            return source.Provider.ExecuteAsync<ILookup<TKey, TElement>>(Expression.Call(InfoOf(() => AsyncQueryable.ToLookup<TSource, TKey, TElement>(default(IAsyncQueryable<TSource>), default(Expression<Func<TSource, Task<TKey>>>), default(Expression<Func<TSource, Task<TElement>>>))), source.Expression, keySelector, elementSelector), CancellationToken.None);
#else
            return source.Provider.ExecuteAsync<ILookup<TKey, TElement>>(Expression.Call(((MethodInfo)MethodBase.GetCurrentMethod()).MakeGenericMethod(typeof(TSource), typeof(TKey), typeof(TElement)), source.Expression, keySelector, elementSelector), CancellationToken.None);
#endif
        }

        public static Task<ILookup<TKey, TElement>> ToLookup<TSource, TKey, TElement>(this IAsyncQueryable<TSource> source, Expression<Func<TSource, TKey>> keySelector, Expression<Func<TSource, TElement>> elementSelector)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));
            if (keySelector == null)
                throw new ArgumentNullException(nameof(keySelector));
            if (elementSelector == null)
                throw new ArgumentNullException(nameof(elementSelector));

#if CRIPPLED_REFLECTION
            return source.Provider.ExecuteAsync<ILookup<TKey, TElement>>(Expression.Call(InfoOf(() => AsyncQueryable.ToLookup<TSource, TKey, TElement>(default(IAsyncQueryable<TSource>), default(Expression<Func<TSource, TKey>>), default(Expression<Func<TSource, TElement>>))), source.Expression, keySelector, elementSelector), CancellationToken.None);
#else
            return source.Provider.ExecuteAsync<ILookup<TKey, TElement>>(Expression.Call(((MethodInfo)MethodBase.GetCurrentMethod()).MakeGenericMethod(typeof(TSource), typeof(TKey), typeof(TElement)), source.Expression, keySelector, elementSelector), CancellationToken.None);
#endif
        }

        public static Task<ILookup<TKey, TElement>> ToLookup<TSource, TKey, TElement>(this IAsyncQueryable<TSource> source, Expression<Func<TSource, Task<TKey>>> keySelector, Expression<Func<TSource, Task<TElement>>> elementSelector, CancellationToken cancellationToken)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));
            if (keySelector == null)
                throw new ArgumentNullException(nameof(keySelector));
            if (elementSelector == null)
                throw new ArgumentNullException(nameof(elementSelector));

#if CRIPPLED_REFLECTION
            return source.Provider.ExecuteAsync<ILookup<TKey, TElement>>(Expression.Call(InfoOf(() => AsyncQueryable.ToLookup<TSource, TKey, TElement>(default(IAsyncQueryable<TSource>), default(Expression<Func<TSource, Task<TKey>>>), default(Expression<Func<TSource, Task<TElement>>>), default(CancellationToken))), source.Expression, keySelector, elementSelector, Expression.Constant(cancellationToken, typeof(CancellationToken))), cancellationToken);
#else
            return source.Provider.ExecuteAsync<ILookup<TKey, TElement>>(Expression.Call(((MethodInfo)MethodBase.GetCurrentMethod()).MakeGenericMethod(typeof(TSource), typeof(TKey), typeof(TElement)), source.Expression, keySelector, elementSelector, Expression.Constant(cancellationToken, typeof(CancellationToken))), cancellationToken);
#endif
        }

        public static Task<ILookup<TKey, TElement>> ToLookup<TSource, TKey, TElement>(this IAsyncQueryable<TSource> source, Expression<Func<TSource, Task<TKey>>> keySelector, Expression<Func<TSource, Task<TElement>>> elementSelector, IEqualityComparer<TKey> comparer)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));
            if (keySelector == null)
                throw new ArgumentNullException(nameof(keySelector));
            if (elementSelector == null)
                throw new ArgumentNullException(nameof(elementSelector));
            if (comparer == null)
                throw new ArgumentNullException(nameof(comparer));

#if CRIPPLED_REFLECTION
            return source.Provider.ExecuteAsync<ILookup<TKey, TElement>>(Expression.Call(InfoOf(() => AsyncQueryable.ToLookup<TSource, TKey, TElement>(default(IAsyncQueryable<TSource>), default(Expression<Func<TSource, Task<TKey>>>), default(Expression<Func<TSource, Task<TElement>>>), default(IEqualityComparer<TKey>))), source.Expression, keySelector, elementSelector, Expression.Constant(comparer, typeof(IEqualityComparer<TKey>))), CancellationToken.None);
#else
            return source.Provider.ExecuteAsync<ILookup<TKey, TElement>>(Expression.Call(((MethodInfo)MethodBase.GetCurrentMethod()).MakeGenericMethod(typeof(TSource), typeof(TKey), typeof(TElement)), source.Expression, keySelector, elementSelector, Expression.Constant(comparer, typeof(IEqualityComparer<TKey>))), CancellationToken.None);
#endif
        }

        public static Task<ILookup<TKey, TElement>> ToLookup<TSource, TKey, TElement>(this IAsyncQueryable<TSource> source, Expression<Func<TSource, TKey>> keySelector, Expression<Func<TSource, TElement>> elementSelector, CancellationToken cancellationToken)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));
            if (keySelector == null)
                throw new ArgumentNullException(nameof(keySelector));
            if (elementSelector == null)
                throw new ArgumentNullException(nameof(elementSelector));

#if CRIPPLED_REFLECTION
            return source.Provider.ExecuteAsync<ILookup<TKey, TElement>>(Expression.Call(InfoOf(() => AsyncQueryable.ToLookup<TSource, TKey, TElement>(default(IAsyncQueryable<TSource>), default(Expression<Func<TSource, TKey>>), default(Expression<Func<TSource, TElement>>), default(CancellationToken))), source.Expression, keySelector, elementSelector, Expression.Constant(cancellationToken, typeof(CancellationToken))), cancellationToken);
#else
            return source.Provider.ExecuteAsync<ILookup<TKey, TElement>>(Expression.Call(((MethodInfo)MethodBase.GetCurrentMethod()).MakeGenericMethod(typeof(TSource), typeof(TKey), typeof(TElement)), source.Expression, keySelector, elementSelector, Expression.Constant(cancellationToken, typeof(CancellationToken))), cancellationToken);
#endif
        }

        public static Task<ILookup<TKey, TElement>> ToLookup<TSource, TKey, TElement>(this IAsyncQueryable<TSource> source, Expression<Func<TSource, TKey>> keySelector, Expression<Func<TSource, TElement>> elementSelector, IEqualityComparer<TKey> comparer)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));
            if (keySelector == null)
                throw new ArgumentNullException(nameof(keySelector));
            if (elementSelector == null)
                throw new ArgumentNullException(nameof(elementSelector));
            if (comparer == null)
                throw new ArgumentNullException(nameof(comparer));

#if CRIPPLED_REFLECTION
            return source.Provider.ExecuteAsync<ILookup<TKey, TElement>>(Expression.Call(InfoOf(() => AsyncQueryable.ToLookup<TSource, TKey, TElement>(default(IAsyncQueryable<TSource>), default(Expression<Func<TSource, TKey>>), default(Expression<Func<TSource, TElement>>), default(IEqualityComparer<TKey>))), source.Expression, keySelector, elementSelector, Expression.Constant(comparer, typeof(IEqualityComparer<TKey>))), CancellationToken.None);
#else
            return source.Provider.ExecuteAsync<ILookup<TKey, TElement>>(Expression.Call(((MethodInfo)MethodBase.GetCurrentMethod()).MakeGenericMethod(typeof(TSource), typeof(TKey), typeof(TElement)), source.Expression, keySelector, elementSelector, Expression.Constant(comparer, typeof(IEqualityComparer<TKey>))), CancellationToken.None);
#endif
        }

        public static Task<ILookup<TKey, TElement>> ToLookup<TSource, TKey, TElement>(this IAsyncQueryable<TSource> source, Expression<Func<TSource, Task<TKey>>> keySelector, Expression<Func<TSource, Task<TElement>>> elementSelector, IEqualityComparer<TKey> comparer, CancellationToken cancellationToken)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));
            if (keySelector == null)
                throw new ArgumentNullException(nameof(keySelector));
            if (elementSelector == null)
                throw new ArgumentNullException(nameof(elementSelector));
            if (comparer == null)
                throw new ArgumentNullException(nameof(comparer));

#if CRIPPLED_REFLECTION
            return source.Provider.ExecuteAsync<ILookup<TKey, TElement>>(Expression.Call(InfoOf(() => AsyncQueryable.ToLookup<TSource, TKey, TElement>(default(IAsyncQueryable<TSource>), default(Expression<Func<TSource, Task<TKey>>>), default(Expression<Func<TSource, Task<TElement>>>), default(IEqualityComparer<TKey>), default(CancellationToken))), source.Expression, keySelector, elementSelector, Expression.Constant(comparer, typeof(IEqualityComparer<TKey>)), Expression.Constant(cancellationToken, typeof(CancellationToken))), cancellationToken);
#else
            return source.Provider.ExecuteAsync<ILookup<TKey, TElement>>(Expression.Call(((MethodInfo)MethodBase.GetCurrentMethod()).MakeGenericMethod(typeof(TSource), typeof(TKey), typeof(TElement)), source.Expression, keySelector, elementSelector, Expression.Constant(comparer, typeof(IEqualityComparer<TKey>)), Expression.Constant(cancellationToken, typeof(CancellationToken))), cancellationToken);
#endif
        }

        public static Task<ILookup<TKey, TElement>> ToLookup<TSource, TKey, TElement>(this IAsyncQueryable<TSource> source, Expression<Func<TSource, TKey>> keySelector, Expression<Func<TSource, TElement>> elementSelector, IEqualityComparer<TKey> comparer, CancellationToken cancellationToken)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));
            if (keySelector == null)
                throw new ArgumentNullException(nameof(keySelector));
            if (elementSelector == null)
                throw new ArgumentNullException(nameof(elementSelector));
            if (comparer == null)
                throw new ArgumentNullException(nameof(comparer));

#if CRIPPLED_REFLECTION
            return source.Provider.ExecuteAsync<ILookup<TKey, TElement>>(Expression.Call(InfoOf(() => AsyncQueryable.ToLookup<TSource, TKey, TElement>(default(IAsyncQueryable<TSource>), default(Expression<Func<TSource, TKey>>), default(Expression<Func<TSource, TElement>>), default(IEqualityComparer<TKey>), default(CancellationToken))), source.Expression, keySelector, elementSelector, Expression.Constant(comparer, typeof(IEqualityComparer<TKey>)), Expression.Constant(cancellationToken, typeof(CancellationToken))), cancellationToken);
#else
            return source.Provider.ExecuteAsync<ILookup<TKey, TElement>>(Expression.Call(((MethodInfo)MethodBase.GetCurrentMethod()).MakeGenericMethod(typeof(TSource), typeof(TKey), typeof(TElement)), source.Expression, keySelector, elementSelector, Expression.Constant(comparer, typeof(IEqualityComparer<TKey>)), Expression.Constant(cancellationToken, typeof(CancellationToken))), cancellationToken);
#endif
        }

        public static IAsyncQueryable<TSource> Union<TSource>(this IAsyncQueryable<TSource> first, IAsyncEnumerable<TSource> second)
        {
            if (first == null)
                throw new ArgumentNullException(nameof(first));
            if (second == null)
                throw new ArgumentNullException(nameof(second));

#if CRIPPLED_REFLECTION
            return first.Provider.CreateQuery<TSource>(Expression.Call(InfoOf(() => AsyncQueryable.Union<TSource>(default(IAsyncQueryable<TSource>), default(IAsyncEnumerable<TSource>))), first.Expression, GetSourceExpression(second)));
#else
            return first.Provider.CreateQuery<TSource>(Expression.Call(((MethodInfo)MethodBase.GetCurrentMethod()).MakeGenericMethod(typeof(TSource)), first.Expression, GetSourceExpression(second)));
#endif
        }

        public static IAsyncQueryable<TSource> Union<TSource>(this IAsyncQueryable<TSource> first, IAsyncEnumerable<TSource> second, IEqualityComparer<TSource> comparer)
        {
            if (first == null)
                throw new ArgumentNullException(nameof(first));
            if (second == null)
                throw new ArgumentNullException(nameof(second));
            if (comparer == null)
                throw new ArgumentNullException(nameof(comparer));

#if CRIPPLED_REFLECTION
            return first.Provider.CreateQuery<TSource>(Expression.Call(InfoOf(() => AsyncQueryable.Union<TSource>(default(IAsyncQueryable<TSource>), default(IAsyncEnumerable<TSource>), default(IEqualityComparer<TSource>))), first.Expression, GetSourceExpression(second), Expression.Constant(comparer, typeof(IEqualityComparer<TSource>))));
#else
            return first.Provider.CreateQuery<TSource>(Expression.Call(((MethodInfo)MethodBase.GetCurrentMethod()).MakeGenericMethod(typeof(TSource)), first.Expression, GetSourceExpression(second), Expression.Constant(comparer, typeof(IEqualityComparer<TSource>))));
#endif
        }

        public static IAsyncQueryable<TSource> Where<TSource>(this IAsyncQueryable<TSource> source, Expression<Func<TSource, bool>> predicate)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));
            if (predicate == null)
                throw new ArgumentNullException(nameof(predicate));

#if CRIPPLED_REFLECTION
            return source.Provider.CreateQuery<TSource>(Expression.Call(InfoOf(() => AsyncQueryable.Where<TSource>(default(IAsyncQueryable<TSource>), default(Expression<Func<TSource, bool>>))), source.Expression, predicate));
#else
            return source.Provider.CreateQuery<TSource>(Expression.Call(((MethodInfo)MethodBase.GetCurrentMethod()).MakeGenericMethod(typeof(TSource)), source.Expression, predicate));
#endif
        }

        public static IAsyncQueryable<TSource> Where<TSource>(this IAsyncQueryable<TSource> source, Expression<Func<TSource, int, bool>> predicate)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));
            if (predicate == null)
                throw new ArgumentNullException(nameof(predicate));

#if CRIPPLED_REFLECTION
            return source.Provider.CreateQuery<TSource>(Expression.Call(InfoOf(() => AsyncQueryable.Where<TSource>(default(IAsyncQueryable<TSource>), default(Expression<Func<TSource, int, bool>>))), source.Expression, predicate));
#else
            return source.Provider.CreateQuery<TSource>(Expression.Call(((MethodInfo)MethodBase.GetCurrentMethod()).MakeGenericMethod(typeof(TSource)), source.Expression, predicate));
#endif
        }

        public static IAsyncQueryable<TSource> Where<TSource>(this IAsyncQueryable<TSource> source, Expression<Func<TSource, int, Task<bool>>> predicate)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));
            if (predicate == null)
                throw new ArgumentNullException(nameof(predicate));

#if CRIPPLED_REFLECTION
            return source.Provider.CreateQuery<TSource>(Expression.Call(InfoOf(() => AsyncQueryable.Where<TSource>(default(IAsyncQueryable<TSource>), default(Expression<Func<TSource, int, Task<bool>>>))), source.Expression, predicate));
#else
            return source.Provider.CreateQuery<TSource>(Expression.Call(((MethodInfo)MethodBase.GetCurrentMethod()).MakeGenericMethod(typeof(TSource)), source.Expression, predicate));
#endif
        }

        public static IAsyncQueryable<TSource> Where<TSource>(this IAsyncQueryable<TSource> source, Expression<Func<TSource, Task<bool>>> predicate)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));
            if (predicate == null)
                throw new ArgumentNullException(nameof(predicate));

#if CRIPPLED_REFLECTION
            return source.Provider.CreateQuery<TSource>(Expression.Call(InfoOf(() => AsyncQueryable.Where<TSource>(default(IAsyncQueryable<TSource>), default(Expression<Func<TSource, Task<bool>>>))), source.Expression, predicate));
#else
            return source.Provider.CreateQuery<TSource>(Expression.Call(((MethodInfo)MethodBase.GetCurrentMethod()).MakeGenericMethod(typeof(TSource)), source.Expression, predicate));
#endif
        }

        public static IAsyncQueryable<TResult> Zip<TFirst, TSecond, TResult>(this IAsyncQueryable<TFirst> first, IAsyncEnumerable<TSecond> second, Expression<Func<TFirst, TSecond, Task<TResult>>> selector)
        {
            if (first == null)
                throw new ArgumentNullException(nameof(first));
            if (second == null)
                throw new ArgumentNullException(nameof(second));
            if (selector == null)
                throw new ArgumentNullException(nameof(selector));

#if CRIPPLED_REFLECTION
            return first.Provider.CreateQuery<TResult>(Expression.Call(InfoOf(() => AsyncQueryable.Zip<TFirst, TSecond, TResult>(default(IAsyncQueryable<TFirst>), default(IAsyncEnumerable<TSecond>), default(Expression<Func<TFirst, TSecond, Task<TResult>>>))), first.Expression, GetSourceExpression(second), selector));
#else
            return first.Provider.CreateQuery<TResult>(Expression.Call(((MethodInfo)MethodBase.GetCurrentMethod()).MakeGenericMethod(typeof(TFirst), typeof(TSecond), typeof(TResult)), first.Expression, GetSourceExpression(second), selector));
#endif
        }

        public static IAsyncQueryable<TResult> Zip<TFirst, TSecond, TResult>(this IAsyncQueryable<TFirst> first, IAsyncEnumerable<TSecond> second, Expression<Func<TFirst, TSecond, TResult>> selector)
        {
            if (first == null)
                throw new ArgumentNullException(nameof(first));
            if (second == null)
                throw new ArgumentNullException(nameof(second));
            if (selector == null)
                throw new ArgumentNullException(nameof(selector));

#if CRIPPLED_REFLECTION
            return first.Provider.CreateQuery<TResult>(Expression.Call(InfoOf(() => AsyncQueryable.Zip<TFirst, TSecond, TResult>(default(IAsyncQueryable<TFirst>), default(IAsyncEnumerable<TSecond>), default(Expression<Func<TFirst, TSecond, TResult>>))), first.Expression, GetSourceExpression(second), selector));
#else
            return first.Provider.CreateQuery<TResult>(Expression.Call(((MethodInfo)MethodBase.GetCurrentMethod()).MakeGenericMethod(typeof(TFirst), typeof(TSecond), typeof(TResult)), first.Expression, GetSourceExpression(second), selector));
#endif
        }

    }
}
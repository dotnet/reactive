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
        public static Task<TSource> AggregateAsync<TSource>(this IAsyncQueryable<TSource> source, Expression<Func<TSource, TSource, CancellationToken, ValueTask<TSource>>> accumulator, CancellationToken cancellationToken = default)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));
            if (accumulator == null)
                throw new ArgumentNullException(nameof(accumulator));

#if CRIPPLED_REFLECTION
            return source.Provider.ExecuteAsync<TSource>(Expression.Call(InfoOf(() => AsyncQueryable.AggregateAsync<TSource>(default(IAsyncQueryable<TSource>), default(Expression<Func<TSource, TSource, CancellationToken, ValueTask<TSource>>>), default(CancellationToken))), source.Expression, accumulator, Expression.Constant(cancellationToken, typeof(CancellationToken))), cancellationToken);
#else
            return source.Provider.ExecuteAsync<TSource>(Expression.Call(((MethodInfo)MethodBase.GetCurrentMethod()).MakeGenericMethod(typeof(TSource)), source.Expression, accumulator, Expression.Constant(cancellationToken, typeof(CancellationToken))), cancellationToken);
#endif
        }

        public static Task<TSource> AggregateAsync<TSource>(this IAsyncQueryable<TSource> source, Expression<Func<TSource, TSource, TSource>> accumulator, CancellationToken cancellationToken = default)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));
            if (accumulator == null)
                throw new ArgumentNullException(nameof(accumulator));

#if CRIPPLED_REFLECTION
            return source.Provider.ExecuteAsync<TSource>(Expression.Call(InfoOf(() => AsyncQueryable.AggregateAsync<TSource>(default(IAsyncQueryable<TSource>), default(Expression<Func<TSource, TSource, TSource>>), default(CancellationToken))), source.Expression, accumulator, Expression.Constant(cancellationToken, typeof(CancellationToken))), cancellationToken);
#else
            return source.Provider.ExecuteAsync<TSource>(Expression.Call(((MethodInfo)MethodBase.GetCurrentMethod()).MakeGenericMethod(typeof(TSource)), source.Expression, accumulator, Expression.Constant(cancellationToken, typeof(CancellationToken))), cancellationToken);
#endif
        }

        public static Task<TSource> AggregateAsync<TSource>(this IAsyncQueryable<TSource> source, Expression<Func<TSource, TSource, ValueTask<TSource>>> accumulator, CancellationToken cancellationToken = default)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));
            if (accumulator == null)
                throw new ArgumentNullException(nameof(accumulator));

#if CRIPPLED_REFLECTION
            return source.Provider.ExecuteAsync<TSource>(Expression.Call(InfoOf(() => AsyncQueryable.AggregateAsync<TSource>(default(IAsyncQueryable<TSource>), default(Expression<Func<TSource, TSource, ValueTask<TSource>>>), default(CancellationToken))), source.Expression, accumulator, Expression.Constant(cancellationToken, typeof(CancellationToken))), cancellationToken);
#else
            return source.Provider.ExecuteAsync<TSource>(Expression.Call(((MethodInfo)MethodBase.GetCurrentMethod()).MakeGenericMethod(typeof(TSource)), source.Expression, accumulator, Expression.Constant(cancellationToken, typeof(CancellationToken))), cancellationToken);
#endif
        }

        public static Task<TAccumulate> AggregateAsync<TSource, TAccumulate>(this IAsyncQueryable<TSource> source, TAccumulate seed, Expression<Func<TAccumulate, TSource, CancellationToken, ValueTask<TAccumulate>>> accumulator, CancellationToken cancellationToken = default)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));
            if (accumulator == null)
                throw new ArgumentNullException(nameof(accumulator));

#if CRIPPLED_REFLECTION
            return source.Provider.ExecuteAsync<TAccumulate>(Expression.Call(InfoOf(() => AsyncQueryable.AggregateAsync<TSource, TAccumulate>(default(IAsyncQueryable<TSource>), default(TAccumulate), default(Expression<Func<TAccumulate, TSource, CancellationToken, ValueTask<TAccumulate>>>), default(CancellationToken))), source.Expression, Expression.Constant(seed, typeof(TAccumulate)), accumulator, Expression.Constant(cancellationToken, typeof(CancellationToken))), cancellationToken);
#else
            return source.Provider.ExecuteAsync<TAccumulate>(Expression.Call(((MethodInfo)MethodBase.GetCurrentMethod()).MakeGenericMethod(typeof(TSource), typeof(TAccumulate)), source.Expression, Expression.Constant(seed, typeof(TAccumulate)), accumulator, Expression.Constant(cancellationToken, typeof(CancellationToken))), cancellationToken);
#endif
        }

        public static Task<TAccumulate> AggregateAsync<TSource, TAccumulate>(this IAsyncQueryable<TSource> source, TAccumulate seed, Expression<Func<TAccumulate, TSource, TAccumulate>> accumulator, CancellationToken cancellationToken = default)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));
            if (accumulator == null)
                throw new ArgumentNullException(nameof(accumulator));

#if CRIPPLED_REFLECTION
            return source.Provider.ExecuteAsync<TAccumulate>(Expression.Call(InfoOf(() => AsyncQueryable.AggregateAsync<TSource, TAccumulate>(default(IAsyncQueryable<TSource>), default(TAccumulate), default(Expression<Func<TAccumulate, TSource, TAccumulate>>), default(CancellationToken))), source.Expression, Expression.Constant(seed, typeof(TAccumulate)), accumulator, Expression.Constant(cancellationToken, typeof(CancellationToken))), cancellationToken);
#else
            return source.Provider.ExecuteAsync<TAccumulate>(Expression.Call(((MethodInfo)MethodBase.GetCurrentMethod()).MakeGenericMethod(typeof(TSource), typeof(TAccumulate)), source.Expression, Expression.Constant(seed, typeof(TAccumulate)), accumulator, Expression.Constant(cancellationToken, typeof(CancellationToken))), cancellationToken);
#endif
        }

        public static Task<TAccumulate> AggregateAsync<TSource, TAccumulate>(this IAsyncQueryable<TSource> source, TAccumulate seed, Expression<Func<TAccumulate, TSource, ValueTask<TAccumulate>>> accumulator, CancellationToken cancellationToken = default)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));
            if (accumulator == null)
                throw new ArgumentNullException(nameof(accumulator));

#if CRIPPLED_REFLECTION
            return source.Provider.ExecuteAsync<TAccumulate>(Expression.Call(InfoOf(() => AsyncQueryable.AggregateAsync<TSource, TAccumulate>(default(IAsyncQueryable<TSource>), default(TAccumulate), default(Expression<Func<TAccumulate, TSource, ValueTask<TAccumulate>>>), default(CancellationToken))), source.Expression, Expression.Constant(seed, typeof(TAccumulate)), accumulator, Expression.Constant(cancellationToken, typeof(CancellationToken))), cancellationToken);
#else
            return source.Provider.ExecuteAsync<TAccumulate>(Expression.Call(((MethodInfo)MethodBase.GetCurrentMethod()).MakeGenericMethod(typeof(TSource), typeof(TAccumulate)), source.Expression, Expression.Constant(seed, typeof(TAccumulate)), accumulator, Expression.Constant(cancellationToken, typeof(CancellationToken))), cancellationToken);
#endif
        }

        public static Task<TResult> AggregateAsync<TSource, TAccumulate, TResult>(this IAsyncQueryable<TSource> source, TAccumulate seed, Expression<Func<TAccumulate, TSource, CancellationToken, ValueTask<TAccumulate>>> accumulator, Expression<Func<TAccumulate, CancellationToken, ValueTask<TResult>>> resultSelector, CancellationToken cancellationToken = default)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));
            if (accumulator == null)
                throw new ArgumentNullException(nameof(accumulator));
            if (resultSelector == null)
                throw new ArgumentNullException(nameof(resultSelector));

#if CRIPPLED_REFLECTION
            return source.Provider.ExecuteAsync<TResult>(Expression.Call(InfoOf(() => AsyncQueryable.AggregateAsync<TSource, TAccumulate, TResult>(default(IAsyncQueryable<TSource>), default(TAccumulate), default(Expression<Func<TAccumulate, TSource, CancellationToken, ValueTask<TAccumulate>>>), default(Expression<Func<TAccumulate, CancellationToken, ValueTask<TResult>>>), default(CancellationToken))), source.Expression, Expression.Constant(seed, typeof(TAccumulate)), accumulator, resultSelector, Expression.Constant(cancellationToken, typeof(CancellationToken))), cancellationToken);
#else
            return source.Provider.ExecuteAsync<TResult>(Expression.Call(((MethodInfo)MethodBase.GetCurrentMethod()).MakeGenericMethod(typeof(TSource), typeof(TAccumulate), typeof(TResult)), source.Expression, Expression.Constant(seed, typeof(TAccumulate)), accumulator, resultSelector, Expression.Constant(cancellationToken, typeof(CancellationToken))), cancellationToken);
#endif
        }

        public static Task<TResult> AggregateAsync<TSource, TAccumulate, TResult>(this IAsyncQueryable<TSource> source, TAccumulate seed, Expression<Func<TAccumulate, TSource, TAccumulate>> accumulator, Expression<Func<TAccumulate, TResult>> resultSelector, CancellationToken cancellationToken = default)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));
            if (accumulator == null)
                throw new ArgumentNullException(nameof(accumulator));
            if (resultSelector == null)
                throw new ArgumentNullException(nameof(resultSelector));

#if CRIPPLED_REFLECTION
            return source.Provider.ExecuteAsync<TResult>(Expression.Call(InfoOf(() => AsyncQueryable.AggregateAsync<TSource, TAccumulate, TResult>(default(IAsyncQueryable<TSource>), default(TAccumulate), default(Expression<Func<TAccumulate, TSource, TAccumulate>>), default(Expression<Func<TAccumulate, TResult>>), default(CancellationToken))), source.Expression, Expression.Constant(seed, typeof(TAccumulate)), accumulator, resultSelector, Expression.Constant(cancellationToken, typeof(CancellationToken))), cancellationToken);
#else
            return source.Provider.ExecuteAsync<TResult>(Expression.Call(((MethodInfo)MethodBase.GetCurrentMethod()).MakeGenericMethod(typeof(TSource), typeof(TAccumulate), typeof(TResult)), source.Expression, Expression.Constant(seed, typeof(TAccumulate)), accumulator, resultSelector, Expression.Constant(cancellationToken, typeof(CancellationToken))), cancellationToken);
#endif
        }

        public static Task<TResult> AggregateAsync<TSource, TAccumulate, TResult>(this IAsyncQueryable<TSource> source, TAccumulate seed, Expression<Func<TAccumulate, TSource, ValueTask<TAccumulate>>> accumulator, Expression<Func<TAccumulate, ValueTask<TResult>>> resultSelector, CancellationToken cancellationToken = default)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));
            if (accumulator == null)
                throw new ArgumentNullException(nameof(accumulator));
            if (resultSelector == null)
                throw new ArgumentNullException(nameof(resultSelector));

#if CRIPPLED_REFLECTION
            return source.Provider.ExecuteAsync<TResult>(Expression.Call(InfoOf(() => AsyncQueryable.AggregateAsync<TSource, TAccumulate, TResult>(default(IAsyncQueryable<TSource>), default(TAccumulate), default(Expression<Func<TAccumulate, TSource, ValueTask<TAccumulate>>>), default(Expression<Func<TAccumulate, ValueTask<TResult>>>), default(CancellationToken))), source.Expression, Expression.Constant(seed, typeof(TAccumulate)), accumulator, resultSelector, Expression.Constant(cancellationToken, typeof(CancellationToken))), cancellationToken);
#else
            return source.Provider.ExecuteAsync<TResult>(Expression.Call(((MethodInfo)MethodBase.GetCurrentMethod()).MakeGenericMethod(typeof(TSource), typeof(TAccumulate), typeof(TResult)), source.Expression, Expression.Constant(seed, typeof(TAccumulate)), accumulator, resultSelector, Expression.Constant(cancellationToken, typeof(CancellationToken))), cancellationToken);
#endif
        }

        public static Task<bool> AllAsync<TSource>(this IAsyncQueryable<TSource> source, Expression<Func<TSource, bool>> predicate, CancellationToken cancellationToken = default)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));
            if (predicate == null)
                throw new ArgumentNullException(nameof(predicate));

#if CRIPPLED_REFLECTION
            return source.Provider.ExecuteAsync<bool>(Expression.Call(InfoOf(() => AsyncQueryable.AllAsync<TSource>(default(IAsyncQueryable<TSource>), default(Expression<Func<TSource, bool>>), default(CancellationToken))), source.Expression, predicate, Expression.Constant(cancellationToken, typeof(CancellationToken))), cancellationToken);
#else
            return source.Provider.ExecuteAsync<bool>(Expression.Call(((MethodInfo)MethodBase.GetCurrentMethod()).MakeGenericMethod(typeof(TSource)), source.Expression, predicate, Expression.Constant(cancellationToken, typeof(CancellationToken))), cancellationToken);
#endif
        }

        public static Task<bool> AllAsync<TSource>(this IAsyncQueryable<TSource> source, Expression<Func<TSource, CancellationToken, ValueTask<bool>>> predicate, CancellationToken cancellationToken = default)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));
            if (predicate == null)
                throw new ArgumentNullException(nameof(predicate));

#if CRIPPLED_REFLECTION
            return source.Provider.ExecuteAsync<bool>(Expression.Call(InfoOf(() => AsyncQueryable.AllAsync<TSource>(default(IAsyncQueryable<TSource>), default(Expression<Func<TSource, CancellationToken, ValueTask<bool>>>), default(CancellationToken))), source.Expression, predicate, Expression.Constant(cancellationToken, typeof(CancellationToken))), cancellationToken);
#else
            return source.Provider.ExecuteAsync<bool>(Expression.Call(((MethodInfo)MethodBase.GetCurrentMethod()).MakeGenericMethod(typeof(TSource)), source.Expression, predicate, Expression.Constant(cancellationToken, typeof(CancellationToken))), cancellationToken);
#endif
        }

        public static Task<bool> AllAsync<TSource>(this IAsyncQueryable<TSource> source, Expression<Func<TSource, ValueTask<bool>>> predicate, CancellationToken cancellationToken = default)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));
            if (predicate == null)
                throw new ArgumentNullException(nameof(predicate));

#if CRIPPLED_REFLECTION
            return source.Provider.ExecuteAsync<bool>(Expression.Call(InfoOf(() => AsyncQueryable.AllAsync<TSource>(default(IAsyncQueryable<TSource>), default(Expression<Func<TSource, ValueTask<bool>>>), default(CancellationToken))), source.Expression, predicate, Expression.Constant(cancellationToken, typeof(CancellationToken))), cancellationToken);
#else
            return source.Provider.ExecuteAsync<bool>(Expression.Call(((MethodInfo)MethodBase.GetCurrentMethod()).MakeGenericMethod(typeof(TSource)), source.Expression, predicate, Expression.Constant(cancellationToken, typeof(CancellationToken))), cancellationToken);
#endif
        }

        public static Task<bool> AnyAsync<TSource>(this IAsyncQueryable<TSource> source, CancellationToken cancellationToken = default)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));

#if CRIPPLED_REFLECTION
            return source.Provider.ExecuteAsync<bool>(Expression.Call(InfoOf(() => AsyncQueryable.AnyAsync<TSource>(default(IAsyncQueryable<TSource>), default(CancellationToken))), source.Expression, Expression.Constant(cancellationToken, typeof(CancellationToken))), cancellationToken);
#else
            return source.Provider.ExecuteAsync<bool>(Expression.Call(((MethodInfo)MethodBase.GetCurrentMethod()).MakeGenericMethod(typeof(TSource)), source.Expression, Expression.Constant(cancellationToken, typeof(CancellationToken))), cancellationToken);
#endif
        }

        public static Task<bool> AnyAsync<TSource>(this IAsyncQueryable<TSource> source, Expression<Func<TSource, bool>> predicate, CancellationToken cancellationToken = default)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));
            if (predicate == null)
                throw new ArgumentNullException(nameof(predicate));

#if CRIPPLED_REFLECTION
            return source.Provider.ExecuteAsync<bool>(Expression.Call(InfoOf(() => AsyncQueryable.AnyAsync<TSource>(default(IAsyncQueryable<TSource>), default(Expression<Func<TSource, bool>>), default(CancellationToken))), source.Expression, predicate, Expression.Constant(cancellationToken, typeof(CancellationToken))), cancellationToken);
#else
            return source.Provider.ExecuteAsync<bool>(Expression.Call(((MethodInfo)MethodBase.GetCurrentMethod()).MakeGenericMethod(typeof(TSource)), source.Expression, predicate, Expression.Constant(cancellationToken, typeof(CancellationToken))), cancellationToken);
#endif
        }

        public static Task<bool> AnyAsync<TSource>(this IAsyncQueryable<TSource> source, Expression<Func<TSource, CancellationToken, ValueTask<bool>>> predicate, CancellationToken cancellationToken = default)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));
            if (predicate == null)
                throw new ArgumentNullException(nameof(predicate));

#if CRIPPLED_REFLECTION
            return source.Provider.ExecuteAsync<bool>(Expression.Call(InfoOf(() => AsyncQueryable.AnyAsync<TSource>(default(IAsyncQueryable<TSource>), default(Expression<Func<TSource, CancellationToken, ValueTask<bool>>>), default(CancellationToken))), source.Expression, predicate, Expression.Constant(cancellationToken, typeof(CancellationToken))), cancellationToken);
#else
            return source.Provider.ExecuteAsync<bool>(Expression.Call(((MethodInfo)MethodBase.GetCurrentMethod()).MakeGenericMethod(typeof(TSource)), source.Expression, predicate, Expression.Constant(cancellationToken, typeof(CancellationToken))), cancellationToken);
#endif
        }

        public static Task<bool> AnyAsync<TSource>(this IAsyncQueryable<TSource> source, Expression<Func<TSource, ValueTask<bool>>> predicate, CancellationToken cancellationToken = default)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));
            if (predicate == null)
                throw new ArgumentNullException(nameof(predicate));

#if CRIPPLED_REFLECTION
            return source.Provider.ExecuteAsync<bool>(Expression.Call(InfoOf(() => AsyncQueryable.AnyAsync<TSource>(default(IAsyncQueryable<TSource>), default(Expression<Func<TSource, ValueTask<bool>>>), default(CancellationToken))), source.Expression, predicate, Expression.Constant(cancellationToken, typeof(CancellationToken))), cancellationToken);
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

        public static Task<decimal?> AverageAsync(this IAsyncQueryable<decimal?> source, CancellationToken cancellationToken = default)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));

#if CRIPPLED_REFLECTION
            return source.Provider.ExecuteAsync<decimal?>(Expression.Call(InfoOf(() => AsyncQueryable.AverageAsync(default(IAsyncQueryable<decimal?>), default(CancellationToken))), source.Expression, Expression.Constant(cancellationToken, typeof(CancellationToken))), cancellationToken);
#else
            return source.Provider.ExecuteAsync<decimal?>(Expression.Call((MethodInfo)MethodBase.GetCurrentMethod(), source.Expression, Expression.Constant(cancellationToken, typeof(CancellationToken))), cancellationToken);
#endif
        }

        public static Task<decimal> AverageAsync(this IAsyncQueryable<decimal> source, CancellationToken cancellationToken = default)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));

#if CRIPPLED_REFLECTION
            return source.Provider.ExecuteAsync<decimal>(Expression.Call(InfoOf(() => AsyncQueryable.AverageAsync(default(IAsyncQueryable<decimal>), default(CancellationToken))), source.Expression, Expression.Constant(cancellationToken, typeof(CancellationToken))), cancellationToken);
#else
            return source.Provider.ExecuteAsync<decimal>(Expression.Call((MethodInfo)MethodBase.GetCurrentMethod(), source.Expression, Expression.Constant(cancellationToken, typeof(CancellationToken))), cancellationToken);
#endif
        }

        public static Task<double?> AverageAsync(this IAsyncQueryable<double?> source, CancellationToken cancellationToken = default)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));

#if CRIPPLED_REFLECTION
            return source.Provider.ExecuteAsync<double?>(Expression.Call(InfoOf(() => AsyncQueryable.AverageAsync(default(IAsyncQueryable<double?>), default(CancellationToken))), source.Expression, Expression.Constant(cancellationToken, typeof(CancellationToken))), cancellationToken);
#else
            return source.Provider.ExecuteAsync<double?>(Expression.Call((MethodInfo)MethodBase.GetCurrentMethod(), source.Expression, Expression.Constant(cancellationToken, typeof(CancellationToken))), cancellationToken);
#endif
        }

        public static Task<double> AverageAsync(this IAsyncQueryable<double> source, CancellationToken cancellationToken = default)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));

#if CRIPPLED_REFLECTION
            return source.Provider.ExecuteAsync<double>(Expression.Call(InfoOf(() => AsyncQueryable.AverageAsync(default(IAsyncQueryable<double>), default(CancellationToken))), source.Expression, Expression.Constant(cancellationToken, typeof(CancellationToken))), cancellationToken);
#else
            return source.Provider.ExecuteAsync<double>(Expression.Call((MethodInfo)MethodBase.GetCurrentMethod(), source.Expression, Expression.Constant(cancellationToken, typeof(CancellationToken))), cancellationToken);
#endif
        }

        public static Task<float?> AverageAsync(this IAsyncQueryable<float?> source, CancellationToken cancellationToken = default)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));

#if CRIPPLED_REFLECTION
            return source.Provider.ExecuteAsync<float?>(Expression.Call(InfoOf(() => AsyncQueryable.AverageAsync(default(IAsyncQueryable<float?>), default(CancellationToken))), source.Expression, Expression.Constant(cancellationToken, typeof(CancellationToken))), cancellationToken);
#else
            return source.Provider.ExecuteAsync<float?>(Expression.Call((MethodInfo)MethodBase.GetCurrentMethod(), source.Expression, Expression.Constant(cancellationToken, typeof(CancellationToken))), cancellationToken);
#endif
        }

        public static Task<float> AverageAsync(this IAsyncQueryable<float> source, CancellationToken cancellationToken = default)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));

#if CRIPPLED_REFLECTION
            return source.Provider.ExecuteAsync<float>(Expression.Call(InfoOf(() => AsyncQueryable.AverageAsync(default(IAsyncQueryable<float>), default(CancellationToken))), source.Expression, Expression.Constant(cancellationToken, typeof(CancellationToken))), cancellationToken);
#else
            return source.Provider.ExecuteAsync<float>(Expression.Call((MethodInfo)MethodBase.GetCurrentMethod(), source.Expression, Expression.Constant(cancellationToken, typeof(CancellationToken))), cancellationToken);
#endif
        }

        public static Task<double?> AverageAsync(this IAsyncQueryable<int?> source, CancellationToken cancellationToken = default)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));

#if CRIPPLED_REFLECTION
            return source.Provider.ExecuteAsync<double?>(Expression.Call(InfoOf(() => AsyncQueryable.AverageAsync(default(IAsyncQueryable<int?>), default(CancellationToken))), source.Expression, Expression.Constant(cancellationToken, typeof(CancellationToken))), cancellationToken);
#else
            return source.Provider.ExecuteAsync<double?>(Expression.Call((MethodInfo)MethodBase.GetCurrentMethod(), source.Expression, Expression.Constant(cancellationToken, typeof(CancellationToken))), cancellationToken);
#endif
        }

        public static Task<double> AverageAsync(this IAsyncQueryable<int> source, CancellationToken cancellationToken = default)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));

#if CRIPPLED_REFLECTION
            return source.Provider.ExecuteAsync<double>(Expression.Call(InfoOf(() => AsyncQueryable.AverageAsync(default(IAsyncQueryable<int>), default(CancellationToken))), source.Expression, Expression.Constant(cancellationToken, typeof(CancellationToken))), cancellationToken);
#else
            return source.Provider.ExecuteAsync<double>(Expression.Call((MethodInfo)MethodBase.GetCurrentMethod(), source.Expression, Expression.Constant(cancellationToken, typeof(CancellationToken))), cancellationToken);
#endif
        }

        public static Task<double?> AverageAsync(this IAsyncQueryable<long?> source, CancellationToken cancellationToken = default)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));

#if CRIPPLED_REFLECTION
            return source.Provider.ExecuteAsync<double?>(Expression.Call(InfoOf(() => AsyncQueryable.AverageAsync(default(IAsyncQueryable<long?>), default(CancellationToken))), source.Expression, Expression.Constant(cancellationToken, typeof(CancellationToken))), cancellationToken);
#else
            return source.Provider.ExecuteAsync<double?>(Expression.Call((MethodInfo)MethodBase.GetCurrentMethod(), source.Expression, Expression.Constant(cancellationToken, typeof(CancellationToken))), cancellationToken);
#endif
        }

        public static Task<double> AverageAsync(this IAsyncQueryable<long> source, CancellationToken cancellationToken = default)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));

#if CRIPPLED_REFLECTION
            return source.Provider.ExecuteAsync<double>(Expression.Call(InfoOf(() => AsyncQueryable.AverageAsync(default(IAsyncQueryable<long>), default(CancellationToken))), source.Expression, Expression.Constant(cancellationToken, typeof(CancellationToken))), cancellationToken);
#else
            return source.Provider.ExecuteAsync<double>(Expression.Call((MethodInfo)MethodBase.GetCurrentMethod(), source.Expression, Expression.Constant(cancellationToken, typeof(CancellationToken))), cancellationToken);
#endif
        }

        public static Task<decimal?> AverageAsync<TSource>(this IAsyncQueryable<TSource> source, Expression<Func<TSource, CancellationToken, ValueTask<decimal?>>> selector, CancellationToken cancellationToken = default)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));
            if (selector == null)
                throw new ArgumentNullException(nameof(selector));

#if CRIPPLED_REFLECTION
            return source.Provider.ExecuteAsync<decimal?>(Expression.Call(InfoOf(() => AsyncQueryable.AverageAsync<TSource>(default(IAsyncQueryable<TSource>), default(Expression<Func<TSource, CancellationToken, ValueTask<decimal?>>>), default(CancellationToken))), source.Expression, selector, Expression.Constant(cancellationToken, typeof(CancellationToken))), cancellationToken);
#else
            return source.Provider.ExecuteAsync<decimal?>(Expression.Call(((MethodInfo)MethodBase.GetCurrentMethod()).MakeGenericMethod(typeof(TSource)), source.Expression, selector, Expression.Constant(cancellationToken, typeof(CancellationToken))), cancellationToken);
#endif
        }

        public static Task<decimal> AverageAsync<TSource>(this IAsyncQueryable<TSource> source, Expression<Func<TSource, CancellationToken, ValueTask<decimal>>> selector, CancellationToken cancellationToken = default)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));
            if (selector == null)
                throw new ArgumentNullException(nameof(selector));

#if CRIPPLED_REFLECTION
            return source.Provider.ExecuteAsync<decimal>(Expression.Call(InfoOf(() => AsyncQueryable.AverageAsync<TSource>(default(IAsyncQueryable<TSource>), default(Expression<Func<TSource, CancellationToken, ValueTask<decimal>>>), default(CancellationToken))), source.Expression, selector, Expression.Constant(cancellationToken, typeof(CancellationToken))), cancellationToken);
#else
            return source.Provider.ExecuteAsync<decimal>(Expression.Call(((MethodInfo)MethodBase.GetCurrentMethod()).MakeGenericMethod(typeof(TSource)), source.Expression, selector, Expression.Constant(cancellationToken, typeof(CancellationToken))), cancellationToken);
#endif
        }

        public static Task<double?> AverageAsync<TSource>(this IAsyncQueryable<TSource> source, Expression<Func<TSource, CancellationToken, ValueTask<double?>>> selector, CancellationToken cancellationToken = default)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));
            if (selector == null)
                throw new ArgumentNullException(nameof(selector));

#if CRIPPLED_REFLECTION
            return source.Provider.ExecuteAsync<double?>(Expression.Call(InfoOf(() => AsyncQueryable.AverageAsync<TSource>(default(IAsyncQueryable<TSource>), default(Expression<Func<TSource, CancellationToken, ValueTask<double?>>>), default(CancellationToken))), source.Expression, selector, Expression.Constant(cancellationToken, typeof(CancellationToken))), cancellationToken);
#else
            return source.Provider.ExecuteAsync<double?>(Expression.Call(((MethodInfo)MethodBase.GetCurrentMethod()).MakeGenericMethod(typeof(TSource)), source.Expression, selector, Expression.Constant(cancellationToken, typeof(CancellationToken))), cancellationToken);
#endif
        }

        public static Task<double> AverageAsync<TSource>(this IAsyncQueryable<TSource> source, Expression<Func<TSource, CancellationToken, ValueTask<double>>> selector, CancellationToken cancellationToken = default)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));
            if (selector == null)
                throw new ArgumentNullException(nameof(selector));

#if CRIPPLED_REFLECTION
            return source.Provider.ExecuteAsync<double>(Expression.Call(InfoOf(() => AsyncQueryable.AverageAsync<TSource>(default(IAsyncQueryable<TSource>), default(Expression<Func<TSource, CancellationToken, ValueTask<double>>>), default(CancellationToken))), source.Expression, selector, Expression.Constant(cancellationToken, typeof(CancellationToken))), cancellationToken);
#else
            return source.Provider.ExecuteAsync<double>(Expression.Call(((MethodInfo)MethodBase.GetCurrentMethod()).MakeGenericMethod(typeof(TSource)), source.Expression, selector, Expression.Constant(cancellationToken, typeof(CancellationToken))), cancellationToken);
#endif
        }

        public static Task<float?> AverageAsync<TSource>(this IAsyncQueryable<TSource> source, Expression<Func<TSource, CancellationToken, ValueTask<float?>>> selector, CancellationToken cancellationToken = default)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));
            if (selector == null)
                throw new ArgumentNullException(nameof(selector));

#if CRIPPLED_REFLECTION
            return source.Provider.ExecuteAsync<float?>(Expression.Call(InfoOf(() => AsyncQueryable.AverageAsync<TSource>(default(IAsyncQueryable<TSource>), default(Expression<Func<TSource, CancellationToken, ValueTask<float?>>>), default(CancellationToken))), source.Expression, selector, Expression.Constant(cancellationToken, typeof(CancellationToken))), cancellationToken);
#else
            return source.Provider.ExecuteAsync<float?>(Expression.Call(((MethodInfo)MethodBase.GetCurrentMethod()).MakeGenericMethod(typeof(TSource)), source.Expression, selector, Expression.Constant(cancellationToken, typeof(CancellationToken))), cancellationToken);
#endif
        }

        public static Task<float> AverageAsync<TSource>(this IAsyncQueryable<TSource> source, Expression<Func<TSource, CancellationToken, ValueTask<float>>> selector, CancellationToken cancellationToken = default)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));
            if (selector == null)
                throw new ArgumentNullException(nameof(selector));

#if CRIPPLED_REFLECTION
            return source.Provider.ExecuteAsync<float>(Expression.Call(InfoOf(() => AsyncQueryable.AverageAsync<TSource>(default(IAsyncQueryable<TSource>), default(Expression<Func<TSource, CancellationToken, ValueTask<float>>>), default(CancellationToken))), source.Expression, selector, Expression.Constant(cancellationToken, typeof(CancellationToken))), cancellationToken);
#else
            return source.Provider.ExecuteAsync<float>(Expression.Call(((MethodInfo)MethodBase.GetCurrentMethod()).MakeGenericMethod(typeof(TSource)), source.Expression, selector, Expression.Constant(cancellationToken, typeof(CancellationToken))), cancellationToken);
#endif
        }

        public static Task<double?> AverageAsync<TSource>(this IAsyncQueryable<TSource> source, Expression<Func<TSource, CancellationToken, ValueTask<int?>>> selector, CancellationToken cancellationToken = default)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));
            if (selector == null)
                throw new ArgumentNullException(nameof(selector));

#if CRIPPLED_REFLECTION
            return source.Provider.ExecuteAsync<double?>(Expression.Call(InfoOf(() => AsyncQueryable.AverageAsync<TSource>(default(IAsyncQueryable<TSource>), default(Expression<Func<TSource, CancellationToken, ValueTask<int?>>>), default(CancellationToken))), source.Expression, selector, Expression.Constant(cancellationToken, typeof(CancellationToken))), cancellationToken);
#else
            return source.Provider.ExecuteAsync<double?>(Expression.Call(((MethodInfo)MethodBase.GetCurrentMethod()).MakeGenericMethod(typeof(TSource)), source.Expression, selector, Expression.Constant(cancellationToken, typeof(CancellationToken))), cancellationToken);
#endif
        }

        public static Task<double> AverageAsync<TSource>(this IAsyncQueryable<TSource> source, Expression<Func<TSource, CancellationToken, ValueTask<int>>> selector, CancellationToken cancellationToken = default)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));
            if (selector == null)
                throw new ArgumentNullException(nameof(selector));

#if CRIPPLED_REFLECTION
            return source.Provider.ExecuteAsync<double>(Expression.Call(InfoOf(() => AsyncQueryable.AverageAsync<TSource>(default(IAsyncQueryable<TSource>), default(Expression<Func<TSource, CancellationToken, ValueTask<int>>>), default(CancellationToken))), source.Expression, selector, Expression.Constant(cancellationToken, typeof(CancellationToken))), cancellationToken);
#else
            return source.Provider.ExecuteAsync<double>(Expression.Call(((MethodInfo)MethodBase.GetCurrentMethod()).MakeGenericMethod(typeof(TSource)), source.Expression, selector, Expression.Constant(cancellationToken, typeof(CancellationToken))), cancellationToken);
#endif
        }

        public static Task<double?> AverageAsync<TSource>(this IAsyncQueryable<TSource> source, Expression<Func<TSource, CancellationToken, ValueTask<long?>>> selector, CancellationToken cancellationToken = default)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));
            if (selector == null)
                throw new ArgumentNullException(nameof(selector));

#if CRIPPLED_REFLECTION
            return source.Provider.ExecuteAsync<double?>(Expression.Call(InfoOf(() => AsyncQueryable.AverageAsync<TSource>(default(IAsyncQueryable<TSource>), default(Expression<Func<TSource, CancellationToken, ValueTask<long?>>>), default(CancellationToken))), source.Expression, selector, Expression.Constant(cancellationToken, typeof(CancellationToken))), cancellationToken);
#else
            return source.Provider.ExecuteAsync<double?>(Expression.Call(((MethodInfo)MethodBase.GetCurrentMethod()).MakeGenericMethod(typeof(TSource)), source.Expression, selector, Expression.Constant(cancellationToken, typeof(CancellationToken))), cancellationToken);
#endif
        }

        public static Task<double> AverageAsync<TSource>(this IAsyncQueryable<TSource> source, Expression<Func<TSource, CancellationToken, ValueTask<long>>> selector, CancellationToken cancellationToken = default)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));
            if (selector == null)
                throw new ArgumentNullException(nameof(selector));

#if CRIPPLED_REFLECTION
            return source.Provider.ExecuteAsync<double>(Expression.Call(InfoOf(() => AsyncQueryable.AverageAsync<TSource>(default(IAsyncQueryable<TSource>), default(Expression<Func<TSource, CancellationToken, ValueTask<long>>>), default(CancellationToken))), source.Expression, selector, Expression.Constant(cancellationToken, typeof(CancellationToken))), cancellationToken);
#else
            return source.Provider.ExecuteAsync<double>(Expression.Call(((MethodInfo)MethodBase.GetCurrentMethod()).MakeGenericMethod(typeof(TSource)), source.Expression, selector, Expression.Constant(cancellationToken, typeof(CancellationToken))), cancellationToken);
#endif
        }

        public static Task<decimal?> AverageAsync<TSource>(this IAsyncQueryable<TSource> source, Expression<Func<TSource, decimal?>> selector, CancellationToken cancellationToken = default)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));
            if (selector == null)
                throw new ArgumentNullException(nameof(selector));

#if CRIPPLED_REFLECTION
            return source.Provider.ExecuteAsync<decimal?>(Expression.Call(InfoOf(() => AsyncQueryable.AverageAsync<TSource>(default(IAsyncQueryable<TSource>), default(Expression<Func<TSource, decimal?>>), default(CancellationToken))), source.Expression, selector, Expression.Constant(cancellationToken, typeof(CancellationToken))), cancellationToken);
#else
            return source.Provider.ExecuteAsync<decimal?>(Expression.Call(((MethodInfo)MethodBase.GetCurrentMethod()).MakeGenericMethod(typeof(TSource)), source.Expression, selector, Expression.Constant(cancellationToken, typeof(CancellationToken))), cancellationToken);
#endif
        }

        public static Task<decimal> AverageAsync<TSource>(this IAsyncQueryable<TSource> source, Expression<Func<TSource, decimal>> selector, CancellationToken cancellationToken = default)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));
            if (selector == null)
                throw new ArgumentNullException(nameof(selector));

#if CRIPPLED_REFLECTION
            return source.Provider.ExecuteAsync<decimal>(Expression.Call(InfoOf(() => AsyncQueryable.AverageAsync<TSource>(default(IAsyncQueryable<TSource>), default(Expression<Func<TSource, decimal>>), default(CancellationToken))), source.Expression, selector, Expression.Constant(cancellationToken, typeof(CancellationToken))), cancellationToken);
#else
            return source.Provider.ExecuteAsync<decimal>(Expression.Call(((MethodInfo)MethodBase.GetCurrentMethod()).MakeGenericMethod(typeof(TSource)), source.Expression, selector, Expression.Constant(cancellationToken, typeof(CancellationToken))), cancellationToken);
#endif
        }

        public static Task<double?> AverageAsync<TSource>(this IAsyncQueryable<TSource> source, Expression<Func<TSource, double?>> selector, CancellationToken cancellationToken = default)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));
            if (selector == null)
                throw new ArgumentNullException(nameof(selector));

#if CRIPPLED_REFLECTION
            return source.Provider.ExecuteAsync<double?>(Expression.Call(InfoOf(() => AsyncQueryable.AverageAsync<TSource>(default(IAsyncQueryable<TSource>), default(Expression<Func<TSource, double?>>), default(CancellationToken))), source.Expression, selector, Expression.Constant(cancellationToken, typeof(CancellationToken))), cancellationToken);
#else
            return source.Provider.ExecuteAsync<double?>(Expression.Call(((MethodInfo)MethodBase.GetCurrentMethod()).MakeGenericMethod(typeof(TSource)), source.Expression, selector, Expression.Constant(cancellationToken, typeof(CancellationToken))), cancellationToken);
#endif
        }

        public static Task<double> AverageAsync<TSource>(this IAsyncQueryable<TSource> source, Expression<Func<TSource, double>> selector, CancellationToken cancellationToken = default)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));
            if (selector == null)
                throw new ArgumentNullException(nameof(selector));

#if CRIPPLED_REFLECTION
            return source.Provider.ExecuteAsync<double>(Expression.Call(InfoOf(() => AsyncQueryable.AverageAsync<TSource>(default(IAsyncQueryable<TSource>), default(Expression<Func<TSource, double>>), default(CancellationToken))), source.Expression, selector, Expression.Constant(cancellationToken, typeof(CancellationToken))), cancellationToken);
#else
            return source.Provider.ExecuteAsync<double>(Expression.Call(((MethodInfo)MethodBase.GetCurrentMethod()).MakeGenericMethod(typeof(TSource)), source.Expression, selector, Expression.Constant(cancellationToken, typeof(CancellationToken))), cancellationToken);
#endif
        }

        public static Task<float?> AverageAsync<TSource>(this IAsyncQueryable<TSource> source, Expression<Func<TSource, float?>> selector, CancellationToken cancellationToken = default)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));
            if (selector == null)
                throw new ArgumentNullException(nameof(selector));

#if CRIPPLED_REFLECTION
            return source.Provider.ExecuteAsync<float?>(Expression.Call(InfoOf(() => AsyncQueryable.AverageAsync<TSource>(default(IAsyncQueryable<TSource>), default(Expression<Func<TSource, float?>>), default(CancellationToken))), source.Expression, selector, Expression.Constant(cancellationToken, typeof(CancellationToken))), cancellationToken);
#else
            return source.Provider.ExecuteAsync<float?>(Expression.Call(((MethodInfo)MethodBase.GetCurrentMethod()).MakeGenericMethod(typeof(TSource)), source.Expression, selector, Expression.Constant(cancellationToken, typeof(CancellationToken))), cancellationToken);
#endif
        }

        public static Task<float> AverageAsync<TSource>(this IAsyncQueryable<TSource> source, Expression<Func<TSource, float>> selector, CancellationToken cancellationToken = default)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));
            if (selector == null)
                throw new ArgumentNullException(nameof(selector));

#if CRIPPLED_REFLECTION
            return source.Provider.ExecuteAsync<float>(Expression.Call(InfoOf(() => AsyncQueryable.AverageAsync<TSource>(default(IAsyncQueryable<TSource>), default(Expression<Func<TSource, float>>), default(CancellationToken))), source.Expression, selector, Expression.Constant(cancellationToken, typeof(CancellationToken))), cancellationToken);
#else
            return source.Provider.ExecuteAsync<float>(Expression.Call(((MethodInfo)MethodBase.GetCurrentMethod()).MakeGenericMethod(typeof(TSource)), source.Expression, selector, Expression.Constant(cancellationToken, typeof(CancellationToken))), cancellationToken);
#endif
        }

        public static Task<double?> AverageAsync<TSource>(this IAsyncQueryable<TSource> source, Expression<Func<TSource, int?>> selector, CancellationToken cancellationToken = default)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));
            if (selector == null)
                throw new ArgumentNullException(nameof(selector));

#if CRIPPLED_REFLECTION
            return source.Provider.ExecuteAsync<double?>(Expression.Call(InfoOf(() => AsyncQueryable.AverageAsync<TSource>(default(IAsyncQueryable<TSource>), default(Expression<Func<TSource, int?>>), default(CancellationToken))), source.Expression, selector, Expression.Constant(cancellationToken, typeof(CancellationToken))), cancellationToken);
#else
            return source.Provider.ExecuteAsync<double?>(Expression.Call(((MethodInfo)MethodBase.GetCurrentMethod()).MakeGenericMethod(typeof(TSource)), source.Expression, selector, Expression.Constant(cancellationToken, typeof(CancellationToken))), cancellationToken);
#endif
        }

        public static Task<double> AverageAsync<TSource>(this IAsyncQueryable<TSource> source, Expression<Func<TSource, int>> selector, CancellationToken cancellationToken = default)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));
            if (selector == null)
                throw new ArgumentNullException(nameof(selector));

#if CRIPPLED_REFLECTION
            return source.Provider.ExecuteAsync<double>(Expression.Call(InfoOf(() => AsyncQueryable.AverageAsync<TSource>(default(IAsyncQueryable<TSource>), default(Expression<Func<TSource, int>>), default(CancellationToken))), source.Expression, selector, Expression.Constant(cancellationToken, typeof(CancellationToken))), cancellationToken);
#else
            return source.Provider.ExecuteAsync<double>(Expression.Call(((MethodInfo)MethodBase.GetCurrentMethod()).MakeGenericMethod(typeof(TSource)), source.Expression, selector, Expression.Constant(cancellationToken, typeof(CancellationToken))), cancellationToken);
#endif
        }

        public static Task<double?> AverageAsync<TSource>(this IAsyncQueryable<TSource> source, Expression<Func<TSource, long?>> selector, CancellationToken cancellationToken = default)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));
            if (selector == null)
                throw new ArgumentNullException(nameof(selector));

#if CRIPPLED_REFLECTION
            return source.Provider.ExecuteAsync<double?>(Expression.Call(InfoOf(() => AsyncQueryable.AverageAsync<TSource>(default(IAsyncQueryable<TSource>), default(Expression<Func<TSource, long?>>), default(CancellationToken))), source.Expression, selector, Expression.Constant(cancellationToken, typeof(CancellationToken))), cancellationToken);
#else
            return source.Provider.ExecuteAsync<double?>(Expression.Call(((MethodInfo)MethodBase.GetCurrentMethod()).MakeGenericMethod(typeof(TSource)), source.Expression, selector, Expression.Constant(cancellationToken, typeof(CancellationToken))), cancellationToken);
#endif
        }

        public static Task<double> AverageAsync<TSource>(this IAsyncQueryable<TSource> source, Expression<Func<TSource, long>> selector, CancellationToken cancellationToken = default)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));
            if (selector == null)
                throw new ArgumentNullException(nameof(selector));

#if CRIPPLED_REFLECTION
            return source.Provider.ExecuteAsync<double>(Expression.Call(InfoOf(() => AsyncQueryable.AverageAsync<TSource>(default(IAsyncQueryable<TSource>), default(Expression<Func<TSource, long>>), default(CancellationToken))), source.Expression, selector, Expression.Constant(cancellationToken, typeof(CancellationToken))), cancellationToken);
#else
            return source.Provider.ExecuteAsync<double>(Expression.Call(((MethodInfo)MethodBase.GetCurrentMethod()).MakeGenericMethod(typeof(TSource)), source.Expression, selector, Expression.Constant(cancellationToken, typeof(CancellationToken))), cancellationToken);
#endif
        }

        public static Task<decimal?> AverageAsync<TSource>(this IAsyncQueryable<TSource> source, Expression<Func<TSource, ValueTask<decimal?>>> selector, CancellationToken cancellationToken = default)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));
            if (selector == null)
                throw new ArgumentNullException(nameof(selector));

#if CRIPPLED_REFLECTION
            return source.Provider.ExecuteAsync<decimal?>(Expression.Call(InfoOf(() => AsyncQueryable.AverageAsync<TSource>(default(IAsyncQueryable<TSource>), default(Expression<Func<TSource, ValueTask<decimal?>>>), default(CancellationToken))), source.Expression, selector, Expression.Constant(cancellationToken, typeof(CancellationToken))), cancellationToken);
#else
            return source.Provider.ExecuteAsync<decimal?>(Expression.Call(((MethodInfo)MethodBase.GetCurrentMethod()).MakeGenericMethod(typeof(TSource)), source.Expression, selector, Expression.Constant(cancellationToken, typeof(CancellationToken))), cancellationToken);
#endif
        }

        public static Task<decimal> AverageAsync<TSource>(this IAsyncQueryable<TSource> source, Expression<Func<TSource, ValueTask<decimal>>> selector, CancellationToken cancellationToken = default)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));
            if (selector == null)
                throw new ArgumentNullException(nameof(selector));

#if CRIPPLED_REFLECTION
            return source.Provider.ExecuteAsync<decimal>(Expression.Call(InfoOf(() => AsyncQueryable.AverageAsync<TSource>(default(IAsyncQueryable<TSource>), default(Expression<Func<TSource, ValueTask<decimal>>>), default(CancellationToken))), source.Expression, selector, Expression.Constant(cancellationToken, typeof(CancellationToken))), cancellationToken);
#else
            return source.Provider.ExecuteAsync<decimal>(Expression.Call(((MethodInfo)MethodBase.GetCurrentMethod()).MakeGenericMethod(typeof(TSource)), source.Expression, selector, Expression.Constant(cancellationToken, typeof(CancellationToken))), cancellationToken);
#endif
        }

        public static Task<double?> AverageAsync<TSource>(this IAsyncQueryable<TSource> source, Expression<Func<TSource, ValueTask<double?>>> selector, CancellationToken cancellationToken = default)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));
            if (selector == null)
                throw new ArgumentNullException(nameof(selector));

#if CRIPPLED_REFLECTION
            return source.Provider.ExecuteAsync<double?>(Expression.Call(InfoOf(() => AsyncQueryable.AverageAsync<TSource>(default(IAsyncQueryable<TSource>), default(Expression<Func<TSource, ValueTask<double?>>>), default(CancellationToken))), source.Expression, selector, Expression.Constant(cancellationToken, typeof(CancellationToken))), cancellationToken);
#else
            return source.Provider.ExecuteAsync<double?>(Expression.Call(((MethodInfo)MethodBase.GetCurrentMethod()).MakeGenericMethod(typeof(TSource)), source.Expression, selector, Expression.Constant(cancellationToken, typeof(CancellationToken))), cancellationToken);
#endif
        }

        public static Task<double> AverageAsync<TSource>(this IAsyncQueryable<TSource> source, Expression<Func<TSource, ValueTask<double>>> selector, CancellationToken cancellationToken = default)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));
            if (selector == null)
                throw new ArgumentNullException(nameof(selector));

#if CRIPPLED_REFLECTION
            return source.Provider.ExecuteAsync<double>(Expression.Call(InfoOf(() => AsyncQueryable.AverageAsync<TSource>(default(IAsyncQueryable<TSource>), default(Expression<Func<TSource, ValueTask<double>>>), default(CancellationToken))), source.Expression, selector, Expression.Constant(cancellationToken, typeof(CancellationToken))), cancellationToken);
#else
            return source.Provider.ExecuteAsync<double>(Expression.Call(((MethodInfo)MethodBase.GetCurrentMethod()).MakeGenericMethod(typeof(TSource)), source.Expression, selector, Expression.Constant(cancellationToken, typeof(CancellationToken))), cancellationToken);
#endif
        }

        public static Task<float?> AverageAsync<TSource>(this IAsyncQueryable<TSource> source, Expression<Func<TSource, ValueTask<float?>>> selector, CancellationToken cancellationToken = default)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));
            if (selector == null)
                throw new ArgumentNullException(nameof(selector));

#if CRIPPLED_REFLECTION
            return source.Provider.ExecuteAsync<float?>(Expression.Call(InfoOf(() => AsyncQueryable.AverageAsync<TSource>(default(IAsyncQueryable<TSource>), default(Expression<Func<TSource, ValueTask<float?>>>), default(CancellationToken))), source.Expression, selector, Expression.Constant(cancellationToken, typeof(CancellationToken))), cancellationToken);
#else
            return source.Provider.ExecuteAsync<float?>(Expression.Call(((MethodInfo)MethodBase.GetCurrentMethod()).MakeGenericMethod(typeof(TSource)), source.Expression, selector, Expression.Constant(cancellationToken, typeof(CancellationToken))), cancellationToken);
#endif
        }

        public static Task<float> AverageAsync<TSource>(this IAsyncQueryable<TSource> source, Expression<Func<TSource, ValueTask<float>>> selector, CancellationToken cancellationToken = default)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));
            if (selector == null)
                throw new ArgumentNullException(nameof(selector));

#if CRIPPLED_REFLECTION
            return source.Provider.ExecuteAsync<float>(Expression.Call(InfoOf(() => AsyncQueryable.AverageAsync<TSource>(default(IAsyncQueryable<TSource>), default(Expression<Func<TSource, ValueTask<float>>>), default(CancellationToken))), source.Expression, selector, Expression.Constant(cancellationToken, typeof(CancellationToken))), cancellationToken);
#else
            return source.Provider.ExecuteAsync<float>(Expression.Call(((MethodInfo)MethodBase.GetCurrentMethod()).MakeGenericMethod(typeof(TSource)), source.Expression, selector, Expression.Constant(cancellationToken, typeof(CancellationToken))), cancellationToken);
#endif
        }

        public static Task<double?> AverageAsync<TSource>(this IAsyncQueryable<TSource> source, Expression<Func<TSource, ValueTask<int?>>> selector, CancellationToken cancellationToken = default)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));
            if (selector == null)
                throw new ArgumentNullException(nameof(selector));

#if CRIPPLED_REFLECTION
            return source.Provider.ExecuteAsync<double?>(Expression.Call(InfoOf(() => AsyncQueryable.AverageAsync<TSource>(default(IAsyncQueryable<TSource>), default(Expression<Func<TSource, ValueTask<int?>>>), default(CancellationToken))), source.Expression, selector, Expression.Constant(cancellationToken, typeof(CancellationToken))), cancellationToken);
#else
            return source.Provider.ExecuteAsync<double?>(Expression.Call(((MethodInfo)MethodBase.GetCurrentMethod()).MakeGenericMethod(typeof(TSource)), source.Expression, selector, Expression.Constant(cancellationToken, typeof(CancellationToken))), cancellationToken);
#endif
        }

        public static Task<double> AverageAsync<TSource>(this IAsyncQueryable<TSource> source, Expression<Func<TSource, ValueTask<int>>> selector, CancellationToken cancellationToken = default)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));
            if (selector == null)
                throw new ArgumentNullException(nameof(selector));

#if CRIPPLED_REFLECTION
            return source.Provider.ExecuteAsync<double>(Expression.Call(InfoOf(() => AsyncQueryable.AverageAsync<TSource>(default(IAsyncQueryable<TSource>), default(Expression<Func<TSource, ValueTask<int>>>), default(CancellationToken))), source.Expression, selector, Expression.Constant(cancellationToken, typeof(CancellationToken))), cancellationToken);
#else
            return source.Provider.ExecuteAsync<double>(Expression.Call(((MethodInfo)MethodBase.GetCurrentMethod()).MakeGenericMethod(typeof(TSource)), source.Expression, selector, Expression.Constant(cancellationToken, typeof(CancellationToken))), cancellationToken);
#endif
        }

        public static Task<double?> AverageAsync<TSource>(this IAsyncQueryable<TSource> source, Expression<Func<TSource, ValueTask<long?>>> selector, CancellationToken cancellationToken = default)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));
            if (selector == null)
                throw new ArgumentNullException(nameof(selector));

#if CRIPPLED_REFLECTION
            return source.Provider.ExecuteAsync<double?>(Expression.Call(InfoOf(() => AsyncQueryable.AverageAsync<TSource>(default(IAsyncQueryable<TSource>), default(Expression<Func<TSource, ValueTask<long?>>>), default(CancellationToken))), source.Expression, selector, Expression.Constant(cancellationToken, typeof(CancellationToken))), cancellationToken);
#else
            return source.Provider.ExecuteAsync<double?>(Expression.Call(((MethodInfo)MethodBase.GetCurrentMethod()).MakeGenericMethod(typeof(TSource)), source.Expression, selector, Expression.Constant(cancellationToken, typeof(CancellationToken))), cancellationToken);
#endif
        }

        public static Task<double> AverageAsync<TSource>(this IAsyncQueryable<TSource> source, Expression<Func<TSource, ValueTask<long>>> selector, CancellationToken cancellationToken = default)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));
            if (selector == null)
                throw new ArgumentNullException(nameof(selector));

#if CRIPPLED_REFLECTION
            return source.Provider.ExecuteAsync<double>(Expression.Call(InfoOf(() => AsyncQueryable.AverageAsync<TSource>(default(IAsyncQueryable<TSource>), default(Expression<Func<TSource, ValueTask<long>>>), default(CancellationToken))), source.Expression, selector, Expression.Constant(cancellationToken, typeof(CancellationToken))), cancellationToken);
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

        public static Task<bool> ContainsAsync<TSource>(this IAsyncQueryable<TSource> source, TSource value, CancellationToken cancellationToken = default)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));

#if CRIPPLED_REFLECTION
            return source.Provider.ExecuteAsync<bool>(Expression.Call(InfoOf(() => AsyncQueryable.ContainsAsync<TSource>(default(IAsyncQueryable<TSource>), default(TSource), default(CancellationToken))), source.Expression, Expression.Constant(value, typeof(TSource)), Expression.Constant(cancellationToken, typeof(CancellationToken))), cancellationToken);
#else
            return source.Provider.ExecuteAsync<bool>(Expression.Call(((MethodInfo)MethodBase.GetCurrentMethod()).MakeGenericMethod(typeof(TSource)), source.Expression, Expression.Constant(value, typeof(TSource)), Expression.Constant(cancellationToken, typeof(CancellationToken))), cancellationToken);
#endif
        }

        public static Task<bool> ContainsAsync<TSource>(this IAsyncQueryable<TSource> source, TSource value, IEqualityComparer<TSource> comparer, CancellationToken cancellationToken = default)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));

#if CRIPPLED_REFLECTION
            return source.Provider.ExecuteAsync<bool>(Expression.Call(InfoOf(() => AsyncQueryable.ContainsAsync<TSource>(default(IAsyncQueryable<TSource>), default(TSource), default(IEqualityComparer<TSource>), default(CancellationToken))), source.Expression, Expression.Constant(value, typeof(TSource)), Expression.Constant(comparer, typeof(IEqualityComparer<TSource>)), Expression.Constant(cancellationToken, typeof(CancellationToken))), cancellationToken);
#else
            return source.Provider.ExecuteAsync<bool>(Expression.Call(((MethodInfo)MethodBase.GetCurrentMethod()).MakeGenericMethod(typeof(TSource)), source.Expression, Expression.Constant(value, typeof(TSource)), Expression.Constant(comparer, typeof(IEqualityComparer<TSource>)), Expression.Constant(cancellationToken, typeof(CancellationToken))), cancellationToken);
#endif
        }

        public static Task<int> CountAsync<TSource>(this IAsyncQueryable<TSource> source, CancellationToken cancellationToken = default)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));

#if CRIPPLED_REFLECTION
            return source.Provider.ExecuteAsync<int>(Expression.Call(InfoOf(() => AsyncQueryable.CountAsync<TSource>(default(IAsyncQueryable<TSource>), default(CancellationToken))), source.Expression, Expression.Constant(cancellationToken, typeof(CancellationToken))), cancellationToken);
#else
            return source.Provider.ExecuteAsync<int>(Expression.Call(((MethodInfo)MethodBase.GetCurrentMethod()).MakeGenericMethod(typeof(TSource)), source.Expression, Expression.Constant(cancellationToken, typeof(CancellationToken))), cancellationToken);
#endif
        }

        public static Task<int> CountAsync<TSource>(this IAsyncQueryable<TSource> source, Expression<Func<TSource, bool>> predicate, CancellationToken cancellationToken = default)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));
            if (predicate == null)
                throw new ArgumentNullException(nameof(predicate));

#if CRIPPLED_REFLECTION
            return source.Provider.ExecuteAsync<int>(Expression.Call(InfoOf(() => AsyncQueryable.CountAsync<TSource>(default(IAsyncQueryable<TSource>), default(Expression<Func<TSource, bool>>), default(CancellationToken))), source.Expression, predicate, Expression.Constant(cancellationToken, typeof(CancellationToken))), cancellationToken);
#else
            return source.Provider.ExecuteAsync<int>(Expression.Call(((MethodInfo)MethodBase.GetCurrentMethod()).MakeGenericMethod(typeof(TSource)), source.Expression, predicate, Expression.Constant(cancellationToken, typeof(CancellationToken))), cancellationToken);
#endif
        }

        public static Task<int> CountAsync<TSource>(this IAsyncQueryable<TSource> source, Expression<Func<TSource, CancellationToken, ValueTask<bool>>> predicate, CancellationToken cancellationToken = default)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));
            if (predicate == null)
                throw new ArgumentNullException(nameof(predicate));

#if CRIPPLED_REFLECTION
            return source.Provider.ExecuteAsync<int>(Expression.Call(InfoOf(() => AsyncQueryable.CountAsync<TSource>(default(IAsyncQueryable<TSource>), default(Expression<Func<TSource, CancellationToken, ValueTask<bool>>>), default(CancellationToken))), source.Expression, predicate, Expression.Constant(cancellationToken, typeof(CancellationToken))), cancellationToken);
#else
            return source.Provider.ExecuteAsync<int>(Expression.Call(((MethodInfo)MethodBase.GetCurrentMethod()).MakeGenericMethod(typeof(TSource)), source.Expression, predicate, Expression.Constant(cancellationToken, typeof(CancellationToken))), cancellationToken);
#endif
        }

        public static Task<int> CountAsync<TSource>(this IAsyncQueryable<TSource> source, Expression<Func<TSource, ValueTask<bool>>> predicate, CancellationToken cancellationToken = default)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));
            if (predicate == null)
                throw new ArgumentNullException(nameof(predicate));

#if CRIPPLED_REFLECTION
            return source.Provider.ExecuteAsync<int>(Expression.Call(InfoOf(() => AsyncQueryable.CountAsync<TSource>(default(IAsyncQueryable<TSource>), default(Expression<Func<TSource, ValueTask<bool>>>), default(CancellationToken))), source.Expression, predicate, Expression.Constant(cancellationToken, typeof(CancellationToken))), cancellationToken);
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

#if CRIPPLED_REFLECTION
            return source.Provider.CreateQuery<TSource>(Expression.Call(InfoOf(() => AsyncQueryable.Distinct<TSource>(default(IAsyncQueryable<TSource>), default(IEqualityComparer<TSource>))), source.Expression, Expression.Constant(comparer, typeof(IEqualityComparer<TSource>))));
#else
            return source.Provider.CreateQuery<TSource>(Expression.Call(((MethodInfo)MethodBase.GetCurrentMethod()).MakeGenericMethod(typeof(TSource)), source.Expression, Expression.Constant(comparer, typeof(IEqualityComparer<TSource>))));
#endif
        }

        public static Task<TSource> ElementAtAsync<TSource>(this IAsyncQueryable<TSource> source, int index, CancellationToken cancellationToken = default)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));

#if CRIPPLED_REFLECTION
            return source.Provider.ExecuteAsync<TSource>(Expression.Call(InfoOf(() => AsyncQueryable.ElementAtAsync<TSource>(default(IAsyncQueryable<TSource>), default(int), default(CancellationToken))), source.Expression, Expression.Constant(index, typeof(int)), Expression.Constant(cancellationToken, typeof(CancellationToken))), cancellationToken);
#else
            return source.Provider.ExecuteAsync<TSource>(Expression.Call(((MethodInfo)MethodBase.GetCurrentMethod()).MakeGenericMethod(typeof(TSource)), source.Expression, Expression.Constant(index, typeof(int)), Expression.Constant(cancellationToken, typeof(CancellationToken))), cancellationToken);
#endif
        }

        public static Task<TSource> ElementAtOrDefaultAsync<TSource>(this IAsyncQueryable<TSource> source, int index, CancellationToken cancellationToken = default)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));

#if CRIPPLED_REFLECTION
            return source.Provider.ExecuteAsync<TSource>(Expression.Call(InfoOf(() => AsyncQueryable.ElementAtOrDefaultAsync<TSource>(default(IAsyncQueryable<TSource>), default(int), default(CancellationToken))), source.Expression, Expression.Constant(index, typeof(int)), Expression.Constant(cancellationToken, typeof(CancellationToken))), cancellationToken);
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

#if CRIPPLED_REFLECTION
            return first.Provider.CreateQuery<TSource>(Expression.Call(InfoOf(() => AsyncQueryable.Except<TSource>(default(IAsyncQueryable<TSource>), default(IAsyncEnumerable<TSource>), default(IEqualityComparer<TSource>))), first.Expression, GetSourceExpression(second), Expression.Constant(comparer, typeof(IEqualityComparer<TSource>))));
#else
            return first.Provider.CreateQuery<TSource>(Expression.Call(((MethodInfo)MethodBase.GetCurrentMethod()).MakeGenericMethod(typeof(TSource)), first.Expression, GetSourceExpression(second), Expression.Constant(comparer, typeof(IEqualityComparer<TSource>))));
#endif
        }

        public static Task<TSource> FirstAsync<TSource>(this IAsyncQueryable<TSource> source, CancellationToken cancellationToken = default)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));

#if CRIPPLED_REFLECTION
            return source.Provider.ExecuteAsync<TSource>(Expression.Call(InfoOf(() => AsyncQueryable.FirstAsync<TSource>(default(IAsyncQueryable<TSource>), default(CancellationToken))), source.Expression, Expression.Constant(cancellationToken, typeof(CancellationToken))), cancellationToken);
#else
            return source.Provider.ExecuteAsync<TSource>(Expression.Call(((MethodInfo)MethodBase.GetCurrentMethod()).MakeGenericMethod(typeof(TSource)), source.Expression, Expression.Constant(cancellationToken, typeof(CancellationToken))), cancellationToken);
#endif
        }

        public static Task<TSource> FirstAsync<TSource>(this IAsyncQueryable<TSource> source, Expression<Func<TSource, bool>> predicate, CancellationToken cancellationToken = default)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));
            if (predicate == null)
                throw new ArgumentNullException(nameof(predicate));

#if CRIPPLED_REFLECTION
            return source.Provider.ExecuteAsync<TSource>(Expression.Call(InfoOf(() => AsyncQueryable.FirstAsync<TSource>(default(IAsyncQueryable<TSource>), default(Expression<Func<TSource, bool>>), default(CancellationToken))), source.Expression, predicate, Expression.Constant(cancellationToken, typeof(CancellationToken))), cancellationToken);
#else
            return source.Provider.ExecuteAsync<TSource>(Expression.Call(((MethodInfo)MethodBase.GetCurrentMethod()).MakeGenericMethod(typeof(TSource)), source.Expression, predicate, Expression.Constant(cancellationToken, typeof(CancellationToken))), cancellationToken);
#endif
        }

        public static Task<TSource> FirstAsync<TSource>(this IAsyncQueryable<TSource> source, Expression<Func<TSource, CancellationToken, ValueTask<bool>>> predicate, CancellationToken cancellationToken = default)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));
            if (predicate == null)
                throw new ArgumentNullException(nameof(predicate));

#if CRIPPLED_REFLECTION
            return source.Provider.ExecuteAsync<TSource>(Expression.Call(InfoOf(() => AsyncQueryable.FirstAsync<TSource>(default(IAsyncQueryable<TSource>), default(Expression<Func<TSource, CancellationToken, ValueTask<bool>>>), default(CancellationToken))), source.Expression, predicate, Expression.Constant(cancellationToken, typeof(CancellationToken))), cancellationToken);
#else
            return source.Provider.ExecuteAsync<TSource>(Expression.Call(((MethodInfo)MethodBase.GetCurrentMethod()).MakeGenericMethod(typeof(TSource)), source.Expression, predicate, Expression.Constant(cancellationToken, typeof(CancellationToken))), cancellationToken);
#endif
        }

        public static Task<TSource> FirstAsync<TSource>(this IAsyncQueryable<TSource> source, Expression<Func<TSource, ValueTask<bool>>> predicate, CancellationToken cancellationToken = default)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));
            if (predicate == null)
                throw new ArgumentNullException(nameof(predicate));

#if CRIPPLED_REFLECTION
            return source.Provider.ExecuteAsync<TSource>(Expression.Call(InfoOf(() => AsyncQueryable.FirstAsync<TSource>(default(IAsyncQueryable<TSource>), default(Expression<Func<TSource, ValueTask<bool>>>), default(CancellationToken))), source.Expression, predicate, Expression.Constant(cancellationToken, typeof(CancellationToken))), cancellationToken);
#else
            return source.Provider.ExecuteAsync<TSource>(Expression.Call(((MethodInfo)MethodBase.GetCurrentMethod()).MakeGenericMethod(typeof(TSource)), source.Expression, predicate, Expression.Constant(cancellationToken, typeof(CancellationToken))), cancellationToken);
#endif
        }

        public static Task<TSource> FirstOrDefaultAsync<TSource>(this IAsyncQueryable<TSource> source, CancellationToken cancellationToken = default)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));

#if CRIPPLED_REFLECTION
            return source.Provider.ExecuteAsync<TSource>(Expression.Call(InfoOf(() => AsyncQueryable.FirstOrDefaultAsync<TSource>(default(IAsyncQueryable<TSource>), default(CancellationToken))), source.Expression, Expression.Constant(cancellationToken, typeof(CancellationToken))), cancellationToken);
#else
            return source.Provider.ExecuteAsync<TSource>(Expression.Call(((MethodInfo)MethodBase.GetCurrentMethod()).MakeGenericMethod(typeof(TSource)), source.Expression, Expression.Constant(cancellationToken, typeof(CancellationToken))), cancellationToken);
#endif
        }

        public static Task<TSource> FirstOrDefaultAsync<TSource>(this IAsyncQueryable<TSource> source, Expression<Func<TSource, bool>> predicate, CancellationToken cancellationToken = default)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));
            if (predicate == null)
                throw new ArgumentNullException(nameof(predicate));

#if CRIPPLED_REFLECTION
            return source.Provider.ExecuteAsync<TSource>(Expression.Call(InfoOf(() => AsyncQueryable.FirstOrDefaultAsync<TSource>(default(IAsyncQueryable<TSource>), default(Expression<Func<TSource, bool>>), default(CancellationToken))), source.Expression, predicate, Expression.Constant(cancellationToken, typeof(CancellationToken))), cancellationToken);
#else
            return source.Provider.ExecuteAsync<TSource>(Expression.Call(((MethodInfo)MethodBase.GetCurrentMethod()).MakeGenericMethod(typeof(TSource)), source.Expression, predicate, Expression.Constant(cancellationToken, typeof(CancellationToken))), cancellationToken);
#endif
        }

        public static Task<TSource> FirstOrDefaultAsync<TSource>(this IAsyncQueryable<TSource> source, Expression<Func<TSource, CancellationToken, ValueTask<bool>>> predicate, CancellationToken cancellationToken = default)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));
            if (predicate == null)
                throw new ArgumentNullException(nameof(predicate));

#if CRIPPLED_REFLECTION
            return source.Provider.ExecuteAsync<TSource>(Expression.Call(InfoOf(() => AsyncQueryable.FirstOrDefaultAsync<TSource>(default(IAsyncQueryable<TSource>), default(Expression<Func<TSource, CancellationToken, ValueTask<bool>>>), default(CancellationToken))), source.Expression, predicate, Expression.Constant(cancellationToken, typeof(CancellationToken))), cancellationToken);
#else
            return source.Provider.ExecuteAsync<TSource>(Expression.Call(((MethodInfo)MethodBase.GetCurrentMethod()).MakeGenericMethod(typeof(TSource)), source.Expression, predicate, Expression.Constant(cancellationToken, typeof(CancellationToken))), cancellationToken);
#endif
        }

        public static Task<TSource> FirstOrDefaultAsync<TSource>(this IAsyncQueryable<TSource> source, Expression<Func<TSource, ValueTask<bool>>> predicate, CancellationToken cancellationToken = default)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));
            if (predicate == null)
                throw new ArgumentNullException(nameof(predicate));

#if CRIPPLED_REFLECTION
            return source.Provider.ExecuteAsync<TSource>(Expression.Call(InfoOf(() => AsyncQueryable.FirstOrDefaultAsync<TSource>(default(IAsyncQueryable<TSource>), default(Expression<Func<TSource, ValueTask<bool>>>), default(CancellationToken))), source.Expression, predicate, Expression.Constant(cancellationToken, typeof(CancellationToken))), cancellationToken);
#else
            return source.Provider.ExecuteAsync<TSource>(Expression.Call(((MethodInfo)MethodBase.GetCurrentMethod()).MakeGenericMethod(typeof(TSource)), source.Expression, predicate, Expression.Constant(cancellationToken, typeof(CancellationToken))), cancellationToken);
#endif
        }

        public static IAsyncQueryable<IAsyncGrouping<TKey, TSource>> GroupBy<TSource, TKey>(this IAsyncQueryable<TSource> source, Expression<Func<TSource, CancellationToken, ValueTask<TKey>>> keySelector)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));
            if (keySelector == null)
                throw new ArgumentNullException(nameof(keySelector));

#if CRIPPLED_REFLECTION
            return source.Provider.CreateQuery<IAsyncGrouping<TKey, TSource>>(Expression.Call(InfoOf(() => AsyncQueryable.GroupBy<TSource, TKey>(default(IAsyncQueryable<TSource>), default(Expression<Func<TSource, CancellationToken, ValueTask<TKey>>>))), source.Expression, keySelector));
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

        public static IAsyncQueryable<IAsyncGrouping<TKey, TSource>> GroupBy<TSource, TKey>(this IAsyncQueryable<TSource> source, Expression<Func<TSource, ValueTask<TKey>>> keySelector)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));
            if (keySelector == null)
                throw new ArgumentNullException(nameof(keySelector));

#if CRIPPLED_REFLECTION
            return source.Provider.CreateQuery<IAsyncGrouping<TKey, TSource>>(Expression.Call(InfoOf(() => AsyncQueryable.GroupBy<TSource, TKey>(default(IAsyncQueryable<TSource>), default(Expression<Func<TSource, ValueTask<TKey>>>))), source.Expression, keySelector));
#else
            return source.Provider.CreateQuery<IAsyncGrouping<TKey, TSource>>(Expression.Call(((MethodInfo)MethodBase.GetCurrentMethod()).MakeGenericMethod(typeof(TSource), typeof(TKey)), source.Expression, keySelector));
#endif
        }

        public static IAsyncQueryable<IAsyncGrouping<TKey, TSource>> GroupBy<TSource, TKey>(this IAsyncQueryable<TSource> source, Expression<Func<TSource, CancellationToken, ValueTask<TKey>>> keySelector, IEqualityComparer<TKey> comparer)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));
            if (keySelector == null)
                throw new ArgumentNullException(nameof(keySelector));

#if CRIPPLED_REFLECTION
            return source.Provider.CreateQuery<IAsyncGrouping<TKey, TSource>>(Expression.Call(InfoOf(() => AsyncQueryable.GroupBy<TSource, TKey>(default(IAsyncQueryable<TSource>), default(Expression<Func<TSource, CancellationToken, ValueTask<TKey>>>), default(IEqualityComparer<TKey>))), source.Expression, keySelector, Expression.Constant(comparer, typeof(IEqualityComparer<TKey>))));
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

#if CRIPPLED_REFLECTION
            return source.Provider.CreateQuery<IAsyncGrouping<TKey, TSource>>(Expression.Call(InfoOf(() => AsyncQueryable.GroupBy<TSource, TKey>(default(IAsyncQueryable<TSource>), default(Expression<Func<TSource, TKey>>), default(IEqualityComparer<TKey>))), source.Expression, keySelector, Expression.Constant(comparer, typeof(IEqualityComparer<TKey>))));
#else
            return source.Provider.CreateQuery<IAsyncGrouping<TKey, TSource>>(Expression.Call(((MethodInfo)MethodBase.GetCurrentMethod()).MakeGenericMethod(typeof(TSource), typeof(TKey)), source.Expression, keySelector, Expression.Constant(comparer, typeof(IEqualityComparer<TKey>))));
#endif
        }

        public static IAsyncQueryable<IAsyncGrouping<TKey, TSource>> GroupBy<TSource, TKey>(this IAsyncQueryable<TSource> source, Expression<Func<TSource, ValueTask<TKey>>> keySelector, IEqualityComparer<TKey> comparer)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));
            if (keySelector == null)
                throw new ArgumentNullException(nameof(keySelector));

#if CRIPPLED_REFLECTION
            return source.Provider.CreateQuery<IAsyncGrouping<TKey, TSource>>(Expression.Call(InfoOf(() => AsyncQueryable.GroupBy<TSource, TKey>(default(IAsyncQueryable<TSource>), default(Expression<Func<TSource, ValueTask<TKey>>>), default(IEqualityComparer<TKey>))), source.Expression, keySelector, Expression.Constant(comparer, typeof(IEqualityComparer<TKey>))));
#else
            return source.Provider.CreateQuery<IAsyncGrouping<TKey, TSource>>(Expression.Call(((MethodInfo)MethodBase.GetCurrentMethod()).MakeGenericMethod(typeof(TSource), typeof(TKey)), source.Expression, keySelector, Expression.Constant(comparer, typeof(IEqualityComparer<TKey>))));
#endif
        }

        public static IAsyncQueryable<TResult> GroupBy<TSource, TKey, TResult>(this IAsyncQueryable<TSource> source, Expression<Func<TSource, CancellationToken, ValueTask<TKey>>> keySelector, Expression<Func<TKey, IAsyncEnumerable<TSource>, CancellationToken, ValueTask<TResult>>> resultSelector)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));
            if (keySelector == null)
                throw new ArgumentNullException(nameof(keySelector));
            if (resultSelector == null)
                throw new ArgumentNullException(nameof(resultSelector));

#if CRIPPLED_REFLECTION
            return source.Provider.CreateQuery<TResult>(Expression.Call(InfoOf(() => AsyncQueryable.GroupBy<TSource, TKey, TResult>(default(IAsyncQueryable<TSource>), default(Expression<Func<TSource, CancellationToken, ValueTask<TKey>>>), default(Expression<Func<TKey, IAsyncEnumerable<TSource>, CancellationToken, ValueTask<TResult>>>))), source.Expression, keySelector, resultSelector));
#else
            return source.Provider.CreateQuery<TResult>(Expression.Call(((MethodInfo)MethodBase.GetCurrentMethod()).MakeGenericMethod(typeof(TSource), typeof(TKey), typeof(TResult)), source.Expression, keySelector, resultSelector));
#endif
        }

        public static IAsyncQueryable<IAsyncGrouping<TKey, TElement>> GroupBy<TSource, TKey, TElement>(this IAsyncQueryable<TSource> source, Expression<Func<TSource, CancellationToken, ValueTask<TKey>>> keySelector, Expression<Func<TSource, CancellationToken, ValueTask<TElement>>> elementSelector)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));
            if (keySelector == null)
                throw new ArgumentNullException(nameof(keySelector));
            if (elementSelector == null)
                throw new ArgumentNullException(nameof(elementSelector));

#if CRIPPLED_REFLECTION
            return source.Provider.CreateQuery<IAsyncGrouping<TKey, TElement>>(Expression.Call(InfoOf(() => AsyncQueryable.GroupBy<TSource, TKey, TElement>(default(IAsyncQueryable<TSource>), default(Expression<Func<TSource, CancellationToken, ValueTask<TKey>>>), default(Expression<Func<TSource, CancellationToken, ValueTask<TElement>>>))), source.Expression, keySelector, elementSelector));
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

        public static IAsyncQueryable<TResult> GroupBy<TSource, TKey, TResult>(this IAsyncQueryable<TSource> source, Expression<Func<TSource, ValueTask<TKey>>> keySelector, Expression<Func<TKey, IAsyncEnumerable<TSource>, ValueTask<TResult>>> resultSelector)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));
            if (keySelector == null)
                throw new ArgumentNullException(nameof(keySelector));
            if (resultSelector == null)
                throw new ArgumentNullException(nameof(resultSelector));

#if CRIPPLED_REFLECTION
            return source.Provider.CreateQuery<TResult>(Expression.Call(InfoOf(() => AsyncQueryable.GroupBy<TSource, TKey, TResult>(default(IAsyncQueryable<TSource>), default(Expression<Func<TSource, ValueTask<TKey>>>), default(Expression<Func<TKey, IAsyncEnumerable<TSource>, ValueTask<TResult>>>))), source.Expression, keySelector, resultSelector));
#else
            return source.Provider.CreateQuery<TResult>(Expression.Call(((MethodInfo)MethodBase.GetCurrentMethod()).MakeGenericMethod(typeof(TSource), typeof(TKey), typeof(TResult)), source.Expression, keySelector, resultSelector));
#endif
        }

        public static IAsyncQueryable<IAsyncGrouping<TKey, TElement>> GroupBy<TSource, TKey, TElement>(this IAsyncQueryable<TSource> source, Expression<Func<TSource, ValueTask<TKey>>> keySelector, Expression<Func<TSource, ValueTask<TElement>>> elementSelector)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));
            if (keySelector == null)
                throw new ArgumentNullException(nameof(keySelector));
            if (elementSelector == null)
                throw new ArgumentNullException(nameof(elementSelector));

#if CRIPPLED_REFLECTION
            return source.Provider.CreateQuery<IAsyncGrouping<TKey, TElement>>(Expression.Call(InfoOf(() => AsyncQueryable.GroupBy<TSource, TKey, TElement>(default(IAsyncQueryable<TSource>), default(Expression<Func<TSource, ValueTask<TKey>>>), default(Expression<Func<TSource, ValueTask<TElement>>>))), source.Expression, keySelector, elementSelector));
#else
            return source.Provider.CreateQuery<IAsyncGrouping<TKey, TElement>>(Expression.Call(((MethodInfo)MethodBase.GetCurrentMethod()).MakeGenericMethod(typeof(TSource), typeof(TKey), typeof(TElement)), source.Expression, keySelector, elementSelector));
#endif
        }

        public static IAsyncQueryable<TResult> GroupBy<TSource, TKey, TResult>(this IAsyncQueryable<TSource> source, Expression<Func<TSource, CancellationToken, ValueTask<TKey>>> keySelector, Expression<Func<TKey, IAsyncEnumerable<TSource>, CancellationToken, ValueTask<TResult>>> resultSelector, IEqualityComparer<TKey> comparer)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));
            if (keySelector == null)
                throw new ArgumentNullException(nameof(keySelector));
            if (resultSelector == null)
                throw new ArgumentNullException(nameof(resultSelector));

#if CRIPPLED_REFLECTION
            return source.Provider.CreateQuery<TResult>(Expression.Call(InfoOf(() => AsyncQueryable.GroupBy<TSource, TKey, TResult>(default(IAsyncQueryable<TSource>), default(Expression<Func<TSource, CancellationToken, ValueTask<TKey>>>), default(Expression<Func<TKey, IAsyncEnumerable<TSource>, CancellationToken, ValueTask<TResult>>>), default(IEqualityComparer<TKey>))), source.Expression, keySelector, resultSelector, Expression.Constant(comparer, typeof(IEqualityComparer<TKey>))));
#else
            return source.Provider.CreateQuery<TResult>(Expression.Call(((MethodInfo)MethodBase.GetCurrentMethod()).MakeGenericMethod(typeof(TSource), typeof(TKey), typeof(TResult)), source.Expression, keySelector, resultSelector, Expression.Constant(comparer, typeof(IEqualityComparer<TKey>))));
#endif
        }

        public static IAsyncQueryable<IAsyncGrouping<TKey, TElement>> GroupBy<TSource, TKey, TElement>(this IAsyncQueryable<TSource> source, Expression<Func<TSource, CancellationToken, ValueTask<TKey>>> keySelector, Expression<Func<TSource, CancellationToken, ValueTask<TElement>>> elementSelector, IEqualityComparer<TKey> comparer)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));
            if (keySelector == null)
                throw new ArgumentNullException(nameof(keySelector));
            if (elementSelector == null)
                throw new ArgumentNullException(nameof(elementSelector));

#if CRIPPLED_REFLECTION
            return source.Provider.CreateQuery<IAsyncGrouping<TKey, TElement>>(Expression.Call(InfoOf(() => AsyncQueryable.GroupBy<TSource, TKey, TElement>(default(IAsyncQueryable<TSource>), default(Expression<Func<TSource, CancellationToken, ValueTask<TKey>>>), default(Expression<Func<TSource, CancellationToken, ValueTask<TElement>>>), default(IEqualityComparer<TKey>))), source.Expression, keySelector, elementSelector, Expression.Constant(comparer, typeof(IEqualityComparer<TKey>))));
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

#if CRIPPLED_REFLECTION
            return source.Provider.CreateQuery<IAsyncGrouping<TKey, TElement>>(Expression.Call(InfoOf(() => AsyncQueryable.GroupBy<TSource, TKey, TElement>(default(IAsyncQueryable<TSource>), default(Expression<Func<TSource, TKey>>), default(Expression<Func<TSource, TElement>>), default(IEqualityComparer<TKey>))), source.Expression, keySelector, elementSelector, Expression.Constant(comparer, typeof(IEqualityComparer<TKey>))));
#else
            return source.Provider.CreateQuery<IAsyncGrouping<TKey, TElement>>(Expression.Call(((MethodInfo)MethodBase.GetCurrentMethod()).MakeGenericMethod(typeof(TSource), typeof(TKey), typeof(TElement)), source.Expression, keySelector, elementSelector, Expression.Constant(comparer, typeof(IEqualityComparer<TKey>))));
#endif
        }

        public static IAsyncQueryable<TResult> GroupBy<TSource, TKey, TResult>(this IAsyncQueryable<TSource> source, Expression<Func<TSource, ValueTask<TKey>>> keySelector, Expression<Func<TKey, IAsyncEnumerable<TSource>, ValueTask<TResult>>> resultSelector, IEqualityComparer<TKey> comparer)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));
            if (keySelector == null)
                throw new ArgumentNullException(nameof(keySelector));
            if (resultSelector == null)
                throw new ArgumentNullException(nameof(resultSelector));

#if CRIPPLED_REFLECTION
            return source.Provider.CreateQuery<TResult>(Expression.Call(InfoOf(() => AsyncQueryable.GroupBy<TSource, TKey, TResult>(default(IAsyncQueryable<TSource>), default(Expression<Func<TSource, ValueTask<TKey>>>), default(Expression<Func<TKey, IAsyncEnumerable<TSource>, ValueTask<TResult>>>), default(IEqualityComparer<TKey>))), source.Expression, keySelector, resultSelector, Expression.Constant(comparer, typeof(IEqualityComparer<TKey>))));
#else
            return source.Provider.CreateQuery<TResult>(Expression.Call(((MethodInfo)MethodBase.GetCurrentMethod()).MakeGenericMethod(typeof(TSource), typeof(TKey), typeof(TResult)), source.Expression, keySelector, resultSelector, Expression.Constant(comparer, typeof(IEqualityComparer<TKey>))));
#endif
        }

        public static IAsyncQueryable<IAsyncGrouping<TKey, TElement>> GroupBy<TSource, TKey, TElement>(this IAsyncQueryable<TSource> source, Expression<Func<TSource, ValueTask<TKey>>> keySelector, Expression<Func<TSource, ValueTask<TElement>>> elementSelector, IEqualityComparer<TKey> comparer)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));
            if (keySelector == null)
                throw new ArgumentNullException(nameof(keySelector));
            if (elementSelector == null)
                throw new ArgumentNullException(nameof(elementSelector));

#if CRIPPLED_REFLECTION
            return source.Provider.CreateQuery<IAsyncGrouping<TKey, TElement>>(Expression.Call(InfoOf(() => AsyncQueryable.GroupBy<TSource, TKey, TElement>(default(IAsyncQueryable<TSource>), default(Expression<Func<TSource, ValueTask<TKey>>>), default(Expression<Func<TSource, ValueTask<TElement>>>), default(IEqualityComparer<TKey>))), source.Expression, keySelector, elementSelector, Expression.Constant(comparer, typeof(IEqualityComparer<TKey>))));
#else
            return source.Provider.CreateQuery<IAsyncGrouping<TKey, TElement>>(Expression.Call(((MethodInfo)MethodBase.GetCurrentMethod()).MakeGenericMethod(typeof(TSource), typeof(TKey), typeof(TElement)), source.Expression, keySelector, elementSelector, Expression.Constant(comparer, typeof(IEqualityComparer<TKey>))));
#endif
        }

        public static IAsyncQueryable<TResult> GroupBy<TSource, TKey, TElement, TResult>(this IAsyncQueryable<TSource> source, Expression<Func<TSource, CancellationToken, ValueTask<TKey>>> keySelector, Expression<Func<TSource, CancellationToken, ValueTask<TElement>>> elementSelector, Expression<Func<TKey, IAsyncEnumerable<TElement>, CancellationToken, ValueTask<TResult>>> resultSelector)
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
            return source.Provider.CreateQuery<TResult>(Expression.Call(InfoOf(() => AsyncQueryable.GroupBy<TSource, TKey, TElement, TResult>(default(IAsyncQueryable<TSource>), default(Expression<Func<TSource, CancellationToken, ValueTask<TKey>>>), default(Expression<Func<TSource, CancellationToken, ValueTask<TElement>>>), default(Expression<Func<TKey, IAsyncEnumerable<TElement>, CancellationToken, ValueTask<TResult>>>))), source.Expression, keySelector, elementSelector, resultSelector));
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

        public static IAsyncQueryable<TResult> GroupBy<TSource, TKey, TElement, TResult>(this IAsyncQueryable<TSource> source, Expression<Func<TSource, ValueTask<TKey>>> keySelector, Expression<Func<TSource, ValueTask<TElement>>> elementSelector, Expression<Func<TKey, IAsyncEnumerable<TElement>, ValueTask<TResult>>> resultSelector)
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
            return source.Provider.CreateQuery<TResult>(Expression.Call(InfoOf(() => AsyncQueryable.GroupBy<TSource, TKey, TElement, TResult>(default(IAsyncQueryable<TSource>), default(Expression<Func<TSource, ValueTask<TKey>>>), default(Expression<Func<TSource, ValueTask<TElement>>>), default(Expression<Func<TKey, IAsyncEnumerable<TElement>, ValueTask<TResult>>>))), source.Expression, keySelector, elementSelector, resultSelector));
#else
            return source.Provider.CreateQuery<TResult>(Expression.Call(((MethodInfo)MethodBase.GetCurrentMethod()).MakeGenericMethod(typeof(TSource), typeof(TKey), typeof(TElement), typeof(TResult)), source.Expression, keySelector, elementSelector, resultSelector));
#endif
        }

        public static IAsyncQueryable<TResult> GroupBy<TSource, TKey, TElement, TResult>(this IAsyncQueryable<TSource> source, Expression<Func<TSource, CancellationToken, ValueTask<TKey>>> keySelector, Expression<Func<TSource, CancellationToken, ValueTask<TElement>>> elementSelector, Expression<Func<TKey, IAsyncEnumerable<TElement>, CancellationToken, ValueTask<TResult>>> resultSelector, IEqualityComparer<TKey> comparer)
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
            return source.Provider.CreateQuery<TResult>(Expression.Call(InfoOf(() => AsyncQueryable.GroupBy<TSource, TKey, TElement, TResult>(default(IAsyncQueryable<TSource>), default(Expression<Func<TSource, CancellationToken, ValueTask<TKey>>>), default(Expression<Func<TSource, CancellationToken, ValueTask<TElement>>>), default(Expression<Func<TKey, IAsyncEnumerable<TElement>, CancellationToken, ValueTask<TResult>>>), default(IEqualityComparer<TKey>))), source.Expression, keySelector, elementSelector, resultSelector, Expression.Constant(comparer, typeof(IEqualityComparer<TKey>))));
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

#if CRIPPLED_REFLECTION
            return source.Provider.CreateQuery<TResult>(Expression.Call(InfoOf(() => AsyncQueryable.GroupBy<TSource, TKey, TElement, TResult>(default(IAsyncQueryable<TSource>), default(Expression<Func<TSource, TKey>>), default(Expression<Func<TSource, TElement>>), default(Expression<Func<TKey, IAsyncEnumerable<TElement>, TResult>>), default(IEqualityComparer<TKey>))), source.Expression, keySelector, elementSelector, resultSelector, Expression.Constant(comparer, typeof(IEqualityComparer<TKey>))));
#else
            return source.Provider.CreateQuery<TResult>(Expression.Call(((MethodInfo)MethodBase.GetCurrentMethod()).MakeGenericMethod(typeof(TSource), typeof(TKey), typeof(TElement), typeof(TResult)), source.Expression, keySelector, elementSelector, resultSelector, Expression.Constant(comparer, typeof(IEqualityComparer<TKey>))));
#endif
        }

        public static IAsyncQueryable<TResult> GroupBy<TSource, TKey, TElement, TResult>(this IAsyncQueryable<TSource> source, Expression<Func<TSource, ValueTask<TKey>>> keySelector, Expression<Func<TSource, ValueTask<TElement>>> elementSelector, Expression<Func<TKey, IAsyncEnumerable<TElement>, ValueTask<TResult>>> resultSelector, IEqualityComparer<TKey> comparer)
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
            return source.Provider.CreateQuery<TResult>(Expression.Call(InfoOf(() => AsyncQueryable.GroupBy<TSource, TKey, TElement, TResult>(default(IAsyncQueryable<TSource>), default(Expression<Func<TSource, ValueTask<TKey>>>), default(Expression<Func<TSource, ValueTask<TElement>>>), default(Expression<Func<TKey, IAsyncEnumerable<TElement>, ValueTask<TResult>>>), default(IEqualityComparer<TKey>))), source.Expression, keySelector, elementSelector, resultSelector, Expression.Constant(comparer, typeof(IEqualityComparer<TKey>))));
#else
            return source.Provider.CreateQuery<TResult>(Expression.Call(((MethodInfo)MethodBase.GetCurrentMethod()).MakeGenericMethod(typeof(TSource), typeof(TKey), typeof(TElement), typeof(TResult)), source.Expression, keySelector, elementSelector, resultSelector, Expression.Constant(comparer, typeof(IEqualityComparer<TKey>))));
#endif
        }

        public static IAsyncQueryable<TResult> GroupJoin<TOuter, TInner, TKey, TResult>(this IAsyncQueryable<TOuter> outer, IAsyncEnumerable<TInner> inner, Expression<Func<TOuter, CancellationToken, ValueTask<TKey>>> outerKeySelector, Expression<Func<TInner, CancellationToken, ValueTask<TKey>>> innerKeySelector, Expression<Func<TOuter, IAsyncEnumerable<TInner>, CancellationToken, ValueTask<TResult>>> resultSelector)
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
            return outer.Provider.CreateQuery<TResult>(Expression.Call(InfoOf(() => AsyncQueryable.GroupJoin<TOuter, TInner, TKey, TResult>(default(IAsyncQueryable<TOuter>), default(IAsyncEnumerable<TInner>), default(Expression<Func<TOuter, CancellationToken, ValueTask<TKey>>>), default(Expression<Func<TInner, CancellationToken, ValueTask<TKey>>>), default(Expression<Func<TOuter, IAsyncEnumerable<TInner>, CancellationToken, ValueTask<TResult>>>))), outer.Expression, GetSourceExpression(inner), outerKeySelector, innerKeySelector, resultSelector));
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

        public static IAsyncQueryable<TResult> GroupJoin<TOuter, TInner, TKey, TResult>(this IAsyncQueryable<TOuter> outer, IAsyncEnumerable<TInner> inner, Expression<Func<TOuter, ValueTask<TKey>>> outerKeySelector, Expression<Func<TInner, ValueTask<TKey>>> innerKeySelector, Expression<Func<TOuter, IAsyncEnumerable<TInner>, ValueTask<TResult>>> resultSelector)
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
            return outer.Provider.CreateQuery<TResult>(Expression.Call(InfoOf(() => AsyncQueryable.GroupJoin<TOuter, TInner, TKey, TResult>(default(IAsyncQueryable<TOuter>), default(IAsyncEnumerable<TInner>), default(Expression<Func<TOuter, ValueTask<TKey>>>), default(Expression<Func<TInner, ValueTask<TKey>>>), default(Expression<Func<TOuter, IAsyncEnumerable<TInner>, ValueTask<TResult>>>))), outer.Expression, GetSourceExpression(inner), outerKeySelector, innerKeySelector, resultSelector));
#else
            return outer.Provider.CreateQuery<TResult>(Expression.Call(((MethodInfo)MethodBase.GetCurrentMethod()).MakeGenericMethod(typeof(TOuter), typeof(TInner), typeof(TKey), typeof(TResult)), outer.Expression, GetSourceExpression(inner), outerKeySelector, innerKeySelector, resultSelector));
#endif
        }

        public static IAsyncQueryable<TResult> GroupJoin<TOuter, TInner, TKey, TResult>(this IAsyncQueryable<TOuter> outer, IAsyncEnumerable<TInner> inner, Expression<Func<TOuter, CancellationToken, ValueTask<TKey>>> outerKeySelector, Expression<Func<TInner, CancellationToken, ValueTask<TKey>>> innerKeySelector, Expression<Func<TOuter, IAsyncEnumerable<TInner>, CancellationToken, ValueTask<TResult>>> resultSelector, IEqualityComparer<TKey> comparer)
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
            return outer.Provider.CreateQuery<TResult>(Expression.Call(InfoOf(() => AsyncQueryable.GroupJoin<TOuter, TInner, TKey, TResult>(default(IAsyncQueryable<TOuter>), default(IAsyncEnumerable<TInner>), default(Expression<Func<TOuter, CancellationToken, ValueTask<TKey>>>), default(Expression<Func<TInner, CancellationToken, ValueTask<TKey>>>), default(Expression<Func<TOuter, IAsyncEnumerable<TInner>, CancellationToken, ValueTask<TResult>>>), default(IEqualityComparer<TKey>))), outer.Expression, GetSourceExpression(inner), outerKeySelector, innerKeySelector, resultSelector, Expression.Constant(comparer, typeof(IEqualityComparer<TKey>))));
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

#if CRIPPLED_REFLECTION
            return outer.Provider.CreateQuery<TResult>(Expression.Call(InfoOf(() => AsyncQueryable.GroupJoin<TOuter, TInner, TKey, TResult>(default(IAsyncQueryable<TOuter>), default(IAsyncEnumerable<TInner>), default(Expression<Func<TOuter, TKey>>), default(Expression<Func<TInner, TKey>>), default(Expression<Func<TOuter, IAsyncEnumerable<TInner>, TResult>>), default(IEqualityComparer<TKey>))), outer.Expression, GetSourceExpression(inner), outerKeySelector, innerKeySelector, resultSelector, Expression.Constant(comparer, typeof(IEqualityComparer<TKey>))));
#else
            return outer.Provider.CreateQuery<TResult>(Expression.Call(((MethodInfo)MethodBase.GetCurrentMethod()).MakeGenericMethod(typeof(TOuter), typeof(TInner), typeof(TKey), typeof(TResult)), outer.Expression, GetSourceExpression(inner), outerKeySelector, innerKeySelector, resultSelector, Expression.Constant(comparer, typeof(IEqualityComparer<TKey>))));
#endif
        }

        public static IAsyncQueryable<TResult> GroupJoin<TOuter, TInner, TKey, TResult>(this IAsyncQueryable<TOuter> outer, IAsyncEnumerable<TInner> inner, Expression<Func<TOuter, ValueTask<TKey>>> outerKeySelector, Expression<Func<TInner, ValueTask<TKey>>> innerKeySelector, Expression<Func<TOuter, IAsyncEnumerable<TInner>, ValueTask<TResult>>> resultSelector, IEqualityComparer<TKey> comparer)
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
            return outer.Provider.CreateQuery<TResult>(Expression.Call(InfoOf(() => AsyncQueryable.GroupJoin<TOuter, TInner, TKey, TResult>(default(IAsyncQueryable<TOuter>), default(IAsyncEnumerable<TInner>), default(Expression<Func<TOuter, ValueTask<TKey>>>), default(Expression<Func<TInner, ValueTask<TKey>>>), default(Expression<Func<TOuter, IAsyncEnumerable<TInner>, ValueTask<TResult>>>), default(IEqualityComparer<TKey>))), outer.Expression, GetSourceExpression(inner), outerKeySelector, innerKeySelector, resultSelector, Expression.Constant(comparer, typeof(IEqualityComparer<TKey>))));
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

#if CRIPPLED_REFLECTION
            return first.Provider.CreateQuery<TSource>(Expression.Call(InfoOf(() => AsyncQueryable.Intersect<TSource>(default(IAsyncQueryable<TSource>), default(IAsyncEnumerable<TSource>), default(IEqualityComparer<TSource>))), first.Expression, GetSourceExpression(second), Expression.Constant(comparer, typeof(IEqualityComparer<TSource>))));
#else
            return first.Provider.CreateQuery<TSource>(Expression.Call(((MethodInfo)MethodBase.GetCurrentMethod()).MakeGenericMethod(typeof(TSource)), first.Expression, GetSourceExpression(second), Expression.Constant(comparer, typeof(IEqualityComparer<TSource>))));
#endif
        }

        public static IAsyncQueryable<TResult> Join<TOuter, TInner, TKey, TResult>(this IAsyncQueryable<TOuter> outer, IAsyncEnumerable<TInner> inner, Expression<Func<TOuter, CancellationToken, ValueTask<TKey>>> outerKeySelector, Expression<Func<TInner, CancellationToken, ValueTask<TKey>>> innerKeySelector, Expression<Func<TOuter, TInner, CancellationToken, ValueTask<TResult>>> resultSelector)
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
            return outer.Provider.CreateQuery<TResult>(Expression.Call(InfoOf(() => AsyncQueryable.Join<TOuter, TInner, TKey, TResult>(default(IAsyncQueryable<TOuter>), default(IAsyncEnumerable<TInner>), default(Expression<Func<TOuter, CancellationToken, ValueTask<TKey>>>), default(Expression<Func<TInner, CancellationToken, ValueTask<TKey>>>), default(Expression<Func<TOuter, TInner, CancellationToken, ValueTask<TResult>>>))), outer.Expression, GetSourceExpression(inner), outerKeySelector, innerKeySelector, resultSelector));
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

        public static IAsyncQueryable<TResult> Join<TOuter, TInner, TKey, TResult>(this IAsyncQueryable<TOuter> outer, IAsyncEnumerable<TInner> inner, Expression<Func<TOuter, ValueTask<TKey>>> outerKeySelector, Expression<Func<TInner, ValueTask<TKey>>> innerKeySelector, Expression<Func<TOuter, TInner, ValueTask<TResult>>> resultSelector)
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
            return outer.Provider.CreateQuery<TResult>(Expression.Call(InfoOf(() => AsyncQueryable.Join<TOuter, TInner, TKey, TResult>(default(IAsyncQueryable<TOuter>), default(IAsyncEnumerable<TInner>), default(Expression<Func<TOuter, ValueTask<TKey>>>), default(Expression<Func<TInner, ValueTask<TKey>>>), default(Expression<Func<TOuter, TInner, ValueTask<TResult>>>))), outer.Expression, GetSourceExpression(inner), outerKeySelector, innerKeySelector, resultSelector));
#else
            return outer.Provider.CreateQuery<TResult>(Expression.Call(((MethodInfo)MethodBase.GetCurrentMethod()).MakeGenericMethod(typeof(TOuter), typeof(TInner), typeof(TKey), typeof(TResult)), outer.Expression, GetSourceExpression(inner), outerKeySelector, innerKeySelector, resultSelector));
#endif
        }

        public static IAsyncQueryable<TResult> Join<TOuter, TInner, TKey, TResult>(this IAsyncQueryable<TOuter> outer, IAsyncEnumerable<TInner> inner, Expression<Func<TOuter, CancellationToken, ValueTask<TKey>>> outerKeySelector, Expression<Func<TInner, CancellationToken, ValueTask<TKey>>> innerKeySelector, Expression<Func<TOuter, TInner, CancellationToken, ValueTask<TResult>>> resultSelector, IEqualityComparer<TKey> comparer)
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
            return outer.Provider.CreateQuery<TResult>(Expression.Call(InfoOf(() => AsyncQueryable.Join<TOuter, TInner, TKey, TResult>(default(IAsyncQueryable<TOuter>), default(IAsyncEnumerable<TInner>), default(Expression<Func<TOuter, CancellationToken, ValueTask<TKey>>>), default(Expression<Func<TInner, CancellationToken, ValueTask<TKey>>>), default(Expression<Func<TOuter, TInner, CancellationToken, ValueTask<TResult>>>), default(IEqualityComparer<TKey>))), outer.Expression, GetSourceExpression(inner), outerKeySelector, innerKeySelector, resultSelector, Expression.Constant(comparer, typeof(IEqualityComparer<TKey>))));
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

#if CRIPPLED_REFLECTION
            return outer.Provider.CreateQuery<TResult>(Expression.Call(InfoOf(() => AsyncQueryable.Join<TOuter, TInner, TKey, TResult>(default(IAsyncQueryable<TOuter>), default(IAsyncEnumerable<TInner>), default(Expression<Func<TOuter, TKey>>), default(Expression<Func<TInner, TKey>>), default(Expression<Func<TOuter, TInner, TResult>>), default(IEqualityComparer<TKey>))), outer.Expression, GetSourceExpression(inner), outerKeySelector, innerKeySelector, resultSelector, Expression.Constant(comparer, typeof(IEqualityComparer<TKey>))));
#else
            return outer.Provider.CreateQuery<TResult>(Expression.Call(((MethodInfo)MethodBase.GetCurrentMethod()).MakeGenericMethod(typeof(TOuter), typeof(TInner), typeof(TKey), typeof(TResult)), outer.Expression, GetSourceExpression(inner), outerKeySelector, innerKeySelector, resultSelector, Expression.Constant(comparer, typeof(IEqualityComparer<TKey>))));
#endif
        }

        public static IAsyncQueryable<TResult> Join<TOuter, TInner, TKey, TResult>(this IAsyncQueryable<TOuter> outer, IAsyncEnumerable<TInner> inner, Expression<Func<TOuter, ValueTask<TKey>>> outerKeySelector, Expression<Func<TInner, ValueTask<TKey>>> innerKeySelector, Expression<Func<TOuter, TInner, ValueTask<TResult>>> resultSelector, IEqualityComparer<TKey> comparer)
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
            return outer.Provider.CreateQuery<TResult>(Expression.Call(InfoOf(() => AsyncQueryable.Join<TOuter, TInner, TKey, TResult>(default(IAsyncQueryable<TOuter>), default(IAsyncEnumerable<TInner>), default(Expression<Func<TOuter, ValueTask<TKey>>>), default(Expression<Func<TInner, ValueTask<TKey>>>), default(Expression<Func<TOuter, TInner, ValueTask<TResult>>>), default(IEqualityComparer<TKey>))), outer.Expression, GetSourceExpression(inner), outerKeySelector, innerKeySelector, resultSelector, Expression.Constant(comparer, typeof(IEqualityComparer<TKey>))));
#else
            return outer.Provider.CreateQuery<TResult>(Expression.Call(((MethodInfo)MethodBase.GetCurrentMethod()).MakeGenericMethod(typeof(TOuter), typeof(TInner), typeof(TKey), typeof(TResult)), outer.Expression, GetSourceExpression(inner), outerKeySelector, innerKeySelector, resultSelector, Expression.Constant(comparer, typeof(IEqualityComparer<TKey>))));
#endif
        }

        public static Task<TSource> LastAsync<TSource>(this IAsyncQueryable<TSource> source, CancellationToken cancellationToken = default)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));

#if CRIPPLED_REFLECTION
            return source.Provider.ExecuteAsync<TSource>(Expression.Call(InfoOf(() => AsyncQueryable.LastAsync<TSource>(default(IAsyncQueryable<TSource>), default(CancellationToken))), source.Expression, Expression.Constant(cancellationToken, typeof(CancellationToken))), cancellationToken);
#else
            return source.Provider.ExecuteAsync<TSource>(Expression.Call(((MethodInfo)MethodBase.GetCurrentMethod()).MakeGenericMethod(typeof(TSource)), source.Expression, Expression.Constant(cancellationToken, typeof(CancellationToken))), cancellationToken);
#endif
        }

        public static Task<TSource> LastAsync<TSource>(this IAsyncQueryable<TSource> source, Expression<Func<TSource, bool>> predicate, CancellationToken cancellationToken = default)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));
            if (predicate == null)
                throw new ArgumentNullException(nameof(predicate));

#if CRIPPLED_REFLECTION
            return source.Provider.ExecuteAsync<TSource>(Expression.Call(InfoOf(() => AsyncQueryable.LastAsync<TSource>(default(IAsyncQueryable<TSource>), default(Expression<Func<TSource, bool>>), default(CancellationToken))), source.Expression, predicate, Expression.Constant(cancellationToken, typeof(CancellationToken))), cancellationToken);
#else
            return source.Provider.ExecuteAsync<TSource>(Expression.Call(((MethodInfo)MethodBase.GetCurrentMethod()).MakeGenericMethod(typeof(TSource)), source.Expression, predicate, Expression.Constant(cancellationToken, typeof(CancellationToken))), cancellationToken);
#endif
        }

        public static Task<TSource> LastAsync<TSource>(this IAsyncQueryable<TSource> source, Expression<Func<TSource, CancellationToken, ValueTask<bool>>> predicate, CancellationToken cancellationToken = default)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));
            if (predicate == null)
                throw new ArgumentNullException(nameof(predicate));

#if CRIPPLED_REFLECTION
            return source.Provider.ExecuteAsync<TSource>(Expression.Call(InfoOf(() => AsyncQueryable.LastAsync<TSource>(default(IAsyncQueryable<TSource>), default(Expression<Func<TSource, CancellationToken, ValueTask<bool>>>), default(CancellationToken))), source.Expression, predicate, Expression.Constant(cancellationToken, typeof(CancellationToken))), cancellationToken);
#else
            return source.Provider.ExecuteAsync<TSource>(Expression.Call(((MethodInfo)MethodBase.GetCurrentMethod()).MakeGenericMethod(typeof(TSource)), source.Expression, predicate, Expression.Constant(cancellationToken, typeof(CancellationToken))), cancellationToken);
#endif
        }

        public static Task<TSource> LastAsync<TSource>(this IAsyncQueryable<TSource> source, Expression<Func<TSource, ValueTask<bool>>> predicate, CancellationToken cancellationToken = default)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));
            if (predicate == null)
                throw new ArgumentNullException(nameof(predicate));

#if CRIPPLED_REFLECTION
            return source.Provider.ExecuteAsync<TSource>(Expression.Call(InfoOf(() => AsyncQueryable.LastAsync<TSource>(default(IAsyncQueryable<TSource>), default(Expression<Func<TSource, ValueTask<bool>>>), default(CancellationToken))), source.Expression, predicate, Expression.Constant(cancellationToken, typeof(CancellationToken))), cancellationToken);
#else
            return source.Provider.ExecuteAsync<TSource>(Expression.Call(((MethodInfo)MethodBase.GetCurrentMethod()).MakeGenericMethod(typeof(TSource)), source.Expression, predicate, Expression.Constant(cancellationToken, typeof(CancellationToken))), cancellationToken);
#endif
        }

        public static Task<TSource> LastOrDefaultAsync<TSource>(this IAsyncQueryable<TSource> source, CancellationToken cancellationToken = default)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));

#if CRIPPLED_REFLECTION
            return source.Provider.ExecuteAsync<TSource>(Expression.Call(InfoOf(() => AsyncQueryable.LastOrDefaultAsync<TSource>(default(IAsyncQueryable<TSource>), default(CancellationToken))), source.Expression, Expression.Constant(cancellationToken, typeof(CancellationToken))), cancellationToken);
#else
            return source.Provider.ExecuteAsync<TSource>(Expression.Call(((MethodInfo)MethodBase.GetCurrentMethod()).MakeGenericMethod(typeof(TSource)), source.Expression, Expression.Constant(cancellationToken, typeof(CancellationToken))), cancellationToken);
#endif
        }

        public static Task<TSource> LastOrDefaultAsync<TSource>(this IAsyncQueryable<TSource> source, Expression<Func<TSource, bool>> predicate, CancellationToken cancellationToken = default)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));
            if (predicate == null)
                throw new ArgumentNullException(nameof(predicate));

#if CRIPPLED_REFLECTION
            return source.Provider.ExecuteAsync<TSource>(Expression.Call(InfoOf(() => AsyncQueryable.LastOrDefaultAsync<TSource>(default(IAsyncQueryable<TSource>), default(Expression<Func<TSource, bool>>), default(CancellationToken))), source.Expression, predicate, Expression.Constant(cancellationToken, typeof(CancellationToken))), cancellationToken);
#else
            return source.Provider.ExecuteAsync<TSource>(Expression.Call(((MethodInfo)MethodBase.GetCurrentMethod()).MakeGenericMethod(typeof(TSource)), source.Expression, predicate, Expression.Constant(cancellationToken, typeof(CancellationToken))), cancellationToken);
#endif
        }

        public static Task<TSource> LastOrDefaultAsync<TSource>(this IAsyncQueryable<TSource> source, Expression<Func<TSource, CancellationToken, ValueTask<bool>>> predicate, CancellationToken cancellationToken = default)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));
            if (predicate == null)
                throw new ArgumentNullException(nameof(predicate));

#if CRIPPLED_REFLECTION
            return source.Provider.ExecuteAsync<TSource>(Expression.Call(InfoOf(() => AsyncQueryable.LastOrDefaultAsync<TSource>(default(IAsyncQueryable<TSource>), default(Expression<Func<TSource, CancellationToken, ValueTask<bool>>>), default(CancellationToken))), source.Expression, predicate, Expression.Constant(cancellationToken, typeof(CancellationToken))), cancellationToken);
#else
            return source.Provider.ExecuteAsync<TSource>(Expression.Call(((MethodInfo)MethodBase.GetCurrentMethod()).MakeGenericMethod(typeof(TSource)), source.Expression, predicate, Expression.Constant(cancellationToken, typeof(CancellationToken))), cancellationToken);
#endif
        }

        public static Task<TSource> LastOrDefaultAsync<TSource>(this IAsyncQueryable<TSource> source, Expression<Func<TSource, ValueTask<bool>>> predicate, CancellationToken cancellationToken = default)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));
            if (predicate == null)
                throw new ArgumentNullException(nameof(predicate));

#if CRIPPLED_REFLECTION
            return source.Provider.ExecuteAsync<TSource>(Expression.Call(InfoOf(() => AsyncQueryable.LastOrDefaultAsync<TSource>(default(IAsyncQueryable<TSource>), default(Expression<Func<TSource, ValueTask<bool>>>), default(CancellationToken))), source.Expression, predicate, Expression.Constant(cancellationToken, typeof(CancellationToken))), cancellationToken);
#else
            return source.Provider.ExecuteAsync<TSource>(Expression.Call(((MethodInfo)MethodBase.GetCurrentMethod()).MakeGenericMethod(typeof(TSource)), source.Expression, predicate, Expression.Constant(cancellationToken, typeof(CancellationToken))), cancellationToken);
#endif
        }

        public static Task<long> LongCountAsync<TSource>(this IAsyncQueryable<TSource> source, CancellationToken cancellationToken = default)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));

#if CRIPPLED_REFLECTION
            return source.Provider.ExecuteAsync<long>(Expression.Call(InfoOf(() => AsyncQueryable.LongCountAsync<TSource>(default(IAsyncQueryable<TSource>), default(CancellationToken))), source.Expression, Expression.Constant(cancellationToken, typeof(CancellationToken))), cancellationToken);
#else
            return source.Provider.ExecuteAsync<long>(Expression.Call(((MethodInfo)MethodBase.GetCurrentMethod()).MakeGenericMethod(typeof(TSource)), source.Expression, Expression.Constant(cancellationToken, typeof(CancellationToken))), cancellationToken);
#endif
        }

        public static Task<long> LongCountAsync<TSource>(this IAsyncQueryable<TSource> source, Expression<Func<TSource, bool>> predicate, CancellationToken cancellationToken = default)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));
            if (predicate == null)
                throw new ArgumentNullException(nameof(predicate));

#if CRIPPLED_REFLECTION
            return source.Provider.ExecuteAsync<long>(Expression.Call(InfoOf(() => AsyncQueryable.LongCountAsync<TSource>(default(IAsyncQueryable<TSource>), default(Expression<Func<TSource, bool>>), default(CancellationToken))), source.Expression, predicate, Expression.Constant(cancellationToken, typeof(CancellationToken))), cancellationToken);
#else
            return source.Provider.ExecuteAsync<long>(Expression.Call(((MethodInfo)MethodBase.GetCurrentMethod()).MakeGenericMethod(typeof(TSource)), source.Expression, predicate, Expression.Constant(cancellationToken, typeof(CancellationToken))), cancellationToken);
#endif
        }

        public static Task<long> LongCountAsync<TSource>(this IAsyncQueryable<TSource> source, Expression<Func<TSource, CancellationToken, ValueTask<bool>>> predicate, CancellationToken cancellationToken = default)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));
            if (predicate == null)
                throw new ArgumentNullException(nameof(predicate));

#if CRIPPLED_REFLECTION
            return source.Provider.ExecuteAsync<long>(Expression.Call(InfoOf(() => AsyncQueryable.LongCountAsync<TSource>(default(IAsyncQueryable<TSource>), default(Expression<Func<TSource, CancellationToken, ValueTask<bool>>>), default(CancellationToken))), source.Expression, predicate, Expression.Constant(cancellationToken, typeof(CancellationToken))), cancellationToken);
#else
            return source.Provider.ExecuteAsync<long>(Expression.Call(((MethodInfo)MethodBase.GetCurrentMethod()).MakeGenericMethod(typeof(TSource)), source.Expression, predicate, Expression.Constant(cancellationToken, typeof(CancellationToken))), cancellationToken);
#endif
        }

        public static Task<long> LongCountAsync<TSource>(this IAsyncQueryable<TSource> source, Expression<Func<TSource, ValueTask<bool>>> predicate, CancellationToken cancellationToken = default)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));
            if (predicate == null)
                throw new ArgumentNullException(nameof(predicate));

#if CRIPPLED_REFLECTION
            return source.Provider.ExecuteAsync<long>(Expression.Call(InfoOf(() => AsyncQueryable.LongCountAsync<TSource>(default(IAsyncQueryable<TSource>), default(Expression<Func<TSource, ValueTask<bool>>>), default(CancellationToken))), source.Expression, predicate, Expression.Constant(cancellationToken, typeof(CancellationToken))), cancellationToken);
#else
            return source.Provider.ExecuteAsync<long>(Expression.Call(((MethodInfo)MethodBase.GetCurrentMethod()).MakeGenericMethod(typeof(TSource)), source.Expression, predicate, Expression.Constant(cancellationToken, typeof(CancellationToken))), cancellationToken);
#endif
        }

        public static Task<decimal?> MaxAsync(this IAsyncQueryable<decimal?> source, CancellationToken cancellationToken = default)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));

#if CRIPPLED_REFLECTION
            return source.Provider.ExecuteAsync<decimal?>(Expression.Call(InfoOf(() => AsyncQueryable.MaxAsync(default(IAsyncQueryable<decimal?>), default(CancellationToken))), source.Expression, Expression.Constant(cancellationToken, typeof(CancellationToken))), cancellationToken);
#else
            return source.Provider.ExecuteAsync<decimal?>(Expression.Call((MethodInfo)MethodBase.GetCurrentMethod(), source.Expression, Expression.Constant(cancellationToken, typeof(CancellationToken))), cancellationToken);
#endif
        }

        public static Task<decimal> MaxAsync(this IAsyncQueryable<decimal> source, CancellationToken cancellationToken = default)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));

#if CRIPPLED_REFLECTION
            return source.Provider.ExecuteAsync<decimal>(Expression.Call(InfoOf(() => AsyncQueryable.MaxAsync(default(IAsyncQueryable<decimal>), default(CancellationToken))), source.Expression, Expression.Constant(cancellationToken, typeof(CancellationToken))), cancellationToken);
#else
            return source.Provider.ExecuteAsync<decimal>(Expression.Call((MethodInfo)MethodBase.GetCurrentMethod(), source.Expression, Expression.Constant(cancellationToken, typeof(CancellationToken))), cancellationToken);
#endif
        }

        public static Task<double?> MaxAsync(this IAsyncQueryable<double?> source, CancellationToken cancellationToken = default)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));

#if CRIPPLED_REFLECTION
            return source.Provider.ExecuteAsync<double?>(Expression.Call(InfoOf(() => AsyncQueryable.MaxAsync(default(IAsyncQueryable<double?>), default(CancellationToken))), source.Expression, Expression.Constant(cancellationToken, typeof(CancellationToken))), cancellationToken);
#else
            return source.Provider.ExecuteAsync<double?>(Expression.Call((MethodInfo)MethodBase.GetCurrentMethod(), source.Expression, Expression.Constant(cancellationToken, typeof(CancellationToken))), cancellationToken);
#endif
        }

        public static Task<double> MaxAsync(this IAsyncQueryable<double> source, CancellationToken cancellationToken = default)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));

#if CRIPPLED_REFLECTION
            return source.Provider.ExecuteAsync<double>(Expression.Call(InfoOf(() => AsyncQueryable.MaxAsync(default(IAsyncQueryable<double>), default(CancellationToken))), source.Expression, Expression.Constant(cancellationToken, typeof(CancellationToken))), cancellationToken);
#else
            return source.Provider.ExecuteAsync<double>(Expression.Call((MethodInfo)MethodBase.GetCurrentMethod(), source.Expression, Expression.Constant(cancellationToken, typeof(CancellationToken))), cancellationToken);
#endif
        }

        public static Task<float?> MaxAsync(this IAsyncQueryable<float?> source, CancellationToken cancellationToken = default)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));

#if CRIPPLED_REFLECTION
            return source.Provider.ExecuteAsync<float?>(Expression.Call(InfoOf(() => AsyncQueryable.MaxAsync(default(IAsyncQueryable<float?>), default(CancellationToken))), source.Expression, Expression.Constant(cancellationToken, typeof(CancellationToken))), cancellationToken);
#else
            return source.Provider.ExecuteAsync<float?>(Expression.Call((MethodInfo)MethodBase.GetCurrentMethod(), source.Expression, Expression.Constant(cancellationToken, typeof(CancellationToken))), cancellationToken);
#endif
        }

        public static Task<float> MaxAsync(this IAsyncQueryable<float> source, CancellationToken cancellationToken = default)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));

#if CRIPPLED_REFLECTION
            return source.Provider.ExecuteAsync<float>(Expression.Call(InfoOf(() => AsyncQueryable.MaxAsync(default(IAsyncQueryable<float>), default(CancellationToken))), source.Expression, Expression.Constant(cancellationToken, typeof(CancellationToken))), cancellationToken);
#else
            return source.Provider.ExecuteAsync<float>(Expression.Call((MethodInfo)MethodBase.GetCurrentMethod(), source.Expression, Expression.Constant(cancellationToken, typeof(CancellationToken))), cancellationToken);
#endif
        }

        public static Task<int?> MaxAsync(this IAsyncQueryable<int?> source, CancellationToken cancellationToken = default)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));

#if CRIPPLED_REFLECTION
            return source.Provider.ExecuteAsync<int?>(Expression.Call(InfoOf(() => AsyncQueryable.MaxAsync(default(IAsyncQueryable<int?>), default(CancellationToken))), source.Expression, Expression.Constant(cancellationToken, typeof(CancellationToken))), cancellationToken);
#else
            return source.Provider.ExecuteAsync<int?>(Expression.Call((MethodInfo)MethodBase.GetCurrentMethod(), source.Expression, Expression.Constant(cancellationToken, typeof(CancellationToken))), cancellationToken);
#endif
        }

        public static Task<int> MaxAsync(this IAsyncQueryable<int> source, CancellationToken cancellationToken = default)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));

#if CRIPPLED_REFLECTION
            return source.Provider.ExecuteAsync<int>(Expression.Call(InfoOf(() => AsyncQueryable.MaxAsync(default(IAsyncQueryable<int>), default(CancellationToken))), source.Expression, Expression.Constant(cancellationToken, typeof(CancellationToken))), cancellationToken);
#else
            return source.Provider.ExecuteAsync<int>(Expression.Call((MethodInfo)MethodBase.GetCurrentMethod(), source.Expression, Expression.Constant(cancellationToken, typeof(CancellationToken))), cancellationToken);
#endif
        }

        public static Task<long?> MaxAsync(this IAsyncQueryable<long?> source, CancellationToken cancellationToken = default)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));

#if CRIPPLED_REFLECTION
            return source.Provider.ExecuteAsync<long?>(Expression.Call(InfoOf(() => AsyncQueryable.MaxAsync(default(IAsyncQueryable<long?>), default(CancellationToken))), source.Expression, Expression.Constant(cancellationToken, typeof(CancellationToken))), cancellationToken);
#else
            return source.Provider.ExecuteAsync<long?>(Expression.Call((MethodInfo)MethodBase.GetCurrentMethod(), source.Expression, Expression.Constant(cancellationToken, typeof(CancellationToken))), cancellationToken);
#endif
        }

        public static Task<long> MaxAsync(this IAsyncQueryable<long> source, CancellationToken cancellationToken = default)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));

#if CRIPPLED_REFLECTION
            return source.Provider.ExecuteAsync<long>(Expression.Call(InfoOf(() => AsyncQueryable.MaxAsync(default(IAsyncQueryable<long>), default(CancellationToken))), source.Expression, Expression.Constant(cancellationToken, typeof(CancellationToken))), cancellationToken);
#else
            return source.Provider.ExecuteAsync<long>(Expression.Call((MethodInfo)MethodBase.GetCurrentMethod(), source.Expression, Expression.Constant(cancellationToken, typeof(CancellationToken))), cancellationToken);
#endif
        }

        public static Task<TSource> MaxAsync<TSource>(this IAsyncQueryable<TSource> source, CancellationToken cancellationToken = default)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));

#if CRIPPLED_REFLECTION
            return source.Provider.ExecuteAsync<TSource>(Expression.Call(InfoOf(() => AsyncQueryable.MaxAsync<TSource>(default(IAsyncQueryable<TSource>), default(CancellationToken))), source.Expression, Expression.Constant(cancellationToken, typeof(CancellationToken))), cancellationToken);
#else
            return source.Provider.ExecuteAsync<TSource>(Expression.Call(((MethodInfo)MethodBase.GetCurrentMethod()).MakeGenericMethod(typeof(TSource)), source.Expression, Expression.Constant(cancellationToken, typeof(CancellationToken))), cancellationToken);
#endif
        }

        public static Task<decimal?> MaxAsync<TSource>(this IAsyncQueryable<TSource> source, Expression<Func<TSource, CancellationToken, ValueTask<decimal?>>> selector, CancellationToken cancellationToken = default)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));
            if (selector == null)
                throw new ArgumentNullException(nameof(selector));

#if CRIPPLED_REFLECTION
            return source.Provider.ExecuteAsync<decimal?>(Expression.Call(InfoOf(() => AsyncQueryable.MaxAsync<TSource>(default(IAsyncQueryable<TSource>), default(Expression<Func<TSource, CancellationToken, ValueTask<decimal?>>>), default(CancellationToken))), source.Expression, selector, Expression.Constant(cancellationToken, typeof(CancellationToken))), cancellationToken);
#else
            return source.Provider.ExecuteAsync<decimal?>(Expression.Call(((MethodInfo)MethodBase.GetCurrentMethod()).MakeGenericMethod(typeof(TSource)), source.Expression, selector, Expression.Constant(cancellationToken, typeof(CancellationToken))), cancellationToken);
#endif
        }

        public static Task<decimal> MaxAsync<TSource>(this IAsyncQueryable<TSource> source, Expression<Func<TSource, CancellationToken, ValueTask<decimal>>> selector, CancellationToken cancellationToken = default)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));
            if (selector == null)
                throw new ArgumentNullException(nameof(selector));

#if CRIPPLED_REFLECTION
            return source.Provider.ExecuteAsync<decimal>(Expression.Call(InfoOf(() => AsyncQueryable.MaxAsync<TSource>(default(IAsyncQueryable<TSource>), default(Expression<Func<TSource, CancellationToken, ValueTask<decimal>>>), default(CancellationToken))), source.Expression, selector, Expression.Constant(cancellationToken, typeof(CancellationToken))), cancellationToken);
#else
            return source.Provider.ExecuteAsync<decimal>(Expression.Call(((MethodInfo)MethodBase.GetCurrentMethod()).MakeGenericMethod(typeof(TSource)), source.Expression, selector, Expression.Constant(cancellationToken, typeof(CancellationToken))), cancellationToken);
#endif
        }

        public static Task<double?> MaxAsync<TSource>(this IAsyncQueryable<TSource> source, Expression<Func<TSource, CancellationToken, ValueTask<double?>>> selector, CancellationToken cancellationToken = default)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));
            if (selector == null)
                throw new ArgumentNullException(nameof(selector));

#if CRIPPLED_REFLECTION
            return source.Provider.ExecuteAsync<double?>(Expression.Call(InfoOf(() => AsyncQueryable.MaxAsync<TSource>(default(IAsyncQueryable<TSource>), default(Expression<Func<TSource, CancellationToken, ValueTask<double?>>>), default(CancellationToken))), source.Expression, selector, Expression.Constant(cancellationToken, typeof(CancellationToken))), cancellationToken);
#else
            return source.Provider.ExecuteAsync<double?>(Expression.Call(((MethodInfo)MethodBase.GetCurrentMethod()).MakeGenericMethod(typeof(TSource)), source.Expression, selector, Expression.Constant(cancellationToken, typeof(CancellationToken))), cancellationToken);
#endif
        }

        public static Task<double> MaxAsync<TSource>(this IAsyncQueryable<TSource> source, Expression<Func<TSource, CancellationToken, ValueTask<double>>> selector, CancellationToken cancellationToken = default)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));
            if (selector == null)
                throw new ArgumentNullException(nameof(selector));

#if CRIPPLED_REFLECTION
            return source.Provider.ExecuteAsync<double>(Expression.Call(InfoOf(() => AsyncQueryable.MaxAsync<TSource>(default(IAsyncQueryable<TSource>), default(Expression<Func<TSource, CancellationToken, ValueTask<double>>>), default(CancellationToken))), source.Expression, selector, Expression.Constant(cancellationToken, typeof(CancellationToken))), cancellationToken);
#else
            return source.Provider.ExecuteAsync<double>(Expression.Call(((MethodInfo)MethodBase.GetCurrentMethod()).MakeGenericMethod(typeof(TSource)), source.Expression, selector, Expression.Constant(cancellationToken, typeof(CancellationToken))), cancellationToken);
#endif
        }

        public static Task<float?> MaxAsync<TSource>(this IAsyncQueryable<TSource> source, Expression<Func<TSource, CancellationToken, ValueTask<float?>>> selector, CancellationToken cancellationToken = default)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));
            if (selector == null)
                throw new ArgumentNullException(nameof(selector));

#if CRIPPLED_REFLECTION
            return source.Provider.ExecuteAsync<float?>(Expression.Call(InfoOf(() => AsyncQueryable.MaxAsync<TSource>(default(IAsyncQueryable<TSource>), default(Expression<Func<TSource, CancellationToken, ValueTask<float?>>>), default(CancellationToken))), source.Expression, selector, Expression.Constant(cancellationToken, typeof(CancellationToken))), cancellationToken);
#else
            return source.Provider.ExecuteAsync<float?>(Expression.Call(((MethodInfo)MethodBase.GetCurrentMethod()).MakeGenericMethod(typeof(TSource)), source.Expression, selector, Expression.Constant(cancellationToken, typeof(CancellationToken))), cancellationToken);
#endif
        }

        public static Task<float> MaxAsync<TSource>(this IAsyncQueryable<TSource> source, Expression<Func<TSource, CancellationToken, ValueTask<float>>> selector, CancellationToken cancellationToken = default)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));
            if (selector == null)
                throw new ArgumentNullException(nameof(selector));

#if CRIPPLED_REFLECTION
            return source.Provider.ExecuteAsync<float>(Expression.Call(InfoOf(() => AsyncQueryable.MaxAsync<TSource>(default(IAsyncQueryable<TSource>), default(Expression<Func<TSource, CancellationToken, ValueTask<float>>>), default(CancellationToken))), source.Expression, selector, Expression.Constant(cancellationToken, typeof(CancellationToken))), cancellationToken);
#else
            return source.Provider.ExecuteAsync<float>(Expression.Call(((MethodInfo)MethodBase.GetCurrentMethod()).MakeGenericMethod(typeof(TSource)), source.Expression, selector, Expression.Constant(cancellationToken, typeof(CancellationToken))), cancellationToken);
#endif
        }

        public static Task<int?> MaxAsync<TSource>(this IAsyncQueryable<TSource> source, Expression<Func<TSource, CancellationToken, ValueTask<int?>>> selector, CancellationToken cancellationToken = default)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));
            if (selector == null)
                throw new ArgumentNullException(nameof(selector));

#if CRIPPLED_REFLECTION
            return source.Provider.ExecuteAsync<int?>(Expression.Call(InfoOf(() => AsyncQueryable.MaxAsync<TSource>(default(IAsyncQueryable<TSource>), default(Expression<Func<TSource, CancellationToken, ValueTask<int?>>>), default(CancellationToken))), source.Expression, selector, Expression.Constant(cancellationToken, typeof(CancellationToken))), cancellationToken);
#else
            return source.Provider.ExecuteAsync<int?>(Expression.Call(((MethodInfo)MethodBase.GetCurrentMethod()).MakeGenericMethod(typeof(TSource)), source.Expression, selector, Expression.Constant(cancellationToken, typeof(CancellationToken))), cancellationToken);
#endif
        }

        public static Task<int> MaxAsync<TSource>(this IAsyncQueryable<TSource> source, Expression<Func<TSource, CancellationToken, ValueTask<int>>> selector, CancellationToken cancellationToken = default)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));
            if (selector == null)
                throw new ArgumentNullException(nameof(selector));

#if CRIPPLED_REFLECTION
            return source.Provider.ExecuteAsync<int>(Expression.Call(InfoOf(() => AsyncQueryable.MaxAsync<TSource>(default(IAsyncQueryable<TSource>), default(Expression<Func<TSource, CancellationToken, ValueTask<int>>>), default(CancellationToken))), source.Expression, selector, Expression.Constant(cancellationToken, typeof(CancellationToken))), cancellationToken);
#else
            return source.Provider.ExecuteAsync<int>(Expression.Call(((MethodInfo)MethodBase.GetCurrentMethod()).MakeGenericMethod(typeof(TSource)), source.Expression, selector, Expression.Constant(cancellationToken, typeof(CancellationToken))), cancellationToken);
#endif
        }

        public static Task<long?> MaxAsync<TSource>(this IAsyncQueryable<TSource> source, Expression<Func<TSource, CancellationToken, ValueTask<long?>>> selector, CancellationToken cancellationToken = default)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));
            if (selector == null)
                throw new ArgumentNullException(nameof(selector));

#if CRIPPLED_REFLECTION
            return source.Provider.ExecuteAsync<long?>(Expression.Call(InfoOf(() => AsyncQueryable.MaxAsync<TSource>(default(IAsyncQueryable<TSource>), default(Expression<Func<TSource, CancellationToken, ValueTask<long?>>>), default(CancellationToken))), source.Expression, selector, Expression.Constant(cancellationToken, typeof(CancellationToken))), cancellationToken);
#else
            return source.Provider.ExecuteAsync<long?>(Expression.Call(((MethodInfo)MethodBase.GetCurrentMethod()).MakeGenericMethod(typeof(TSource)), source.Expression, selector, Expression.Constant(cancellationToken, typeof(CancellationToken))), cancellationToken);
#endif
        }

        public static Task<long> MaxAsync<TSource>(this IAsyncQueryable<TSource> source, Expression<Func<TSource, CancellationToken, ValueTask<long>>> selector, CancellationToken cancellationToken = default)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));
            if (selector == null)
                throw new ArgumentNullException(nameof(selector));

#if CRIPPLED_REFLECTION
            return source.Provider.ExecuteAsync<long>(Expression.Call(InfoOf(() => AsyncQueryable.MaxAsync<TSource>(default(IAsyncQueryable<TSource>), default(Expression<Func<TSource, CancellationToken, ValueTask<long>>>), default(CancellationToken))), source.Expression, selector, Expression.Constant(cancellationToken, typeof(CancellationToken))), cancellationToken);
#else
            return source.Provider.ExecuteAsync<long>(Expression.Call(((MethodInfo)MethodBase.GetCurrentMethod()).MakeGenericMethod(typeof(TSource)), source.Expression, selector, Expression.Constant(cancellationToken, typeof(CancellationToken))), cancellationToken);
#endif
        }

        public static Task<decimal?> MaxAsync<TSource>(this IAsyncQueryable<TSource> source, Expression<Func<TSource, decimal?>> selector, CancellationToken cancellationToken = default)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));
            if (selector == null)
                throw new ArgumentNullException(nameof(selector));

#if CRIPPLED_REFLECTION
            return source.Provider.ExecuteAsync<decimal?>(Expression.Call(InfoOf(() => AsyncQueryable.MaxAsync<TSource>(default(IAsyncQueryable<TSource>), default(Expression<Func<TSource, decimal?>>), default(CancellationToken))), source.Expression, selector, Expression.Constant(cancellationToken, typeof(CancellationToken))), cancellationToken);
#else
            return source.Provider.ExecuteAsync<decimal?>(Expression.Call(((MethodInfo)MethodBase.GetCurrentMethod()).MakeGenericMethod(typeof(TSource)), source.Expression, selector, Expression.Constant(cancellationToken, typeof(CancellationToken))), cancellationToken);
#endif
        }

        public static Task<decimal> MaxAsync<TSource>(this IAsyncQueryable<TSource> source, Expression<Func<TSource, decimal>> selector, CancellationToken cancellationToken = default)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));
            if (selector == null)
                throw new ArgumentNullException(nameof(selector));

#if CRIPPLED_REFLECTION
            return source.Provider.ExecuteAsync<decimal>(Expression.Call(InfoOf(() => AsyncQueryable.MaxAsync<TSource>(default(IAsyncQueryable<TSource>), default(Expression<Func<TSource, decimal>>), default(CancellationToken))), source.Expression, selector, Expression.Constant(cancellationToken, typeof(CancellationToken))), cancellationToken);
#else
            return source.Provider.ExecuteAsync<decimal>(Expression.Call(((MethodInfo)MethodBase.GetCurrentMethod()).MakeGenericMethod(typeof(TSource)), source.Expression, selector, Expression.Constant(cancellationToken, typeof(CancellationToken))), cancellationToken);
#endif
        }

        public static Task<double?> MaxAsync<TSource>(this IAsyncQueryable<TSource> source, Expression<Func<TSource, double?>> selector, CancellationToken cancellationToken = default)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));
            if (selector == null)
                throw new ArgumentNullException(nameof(selector));

#if CRIPPLED_REFLECTION
            return source.Provider.ExecuteAsync<double?>(Expression.Call(InfoOf(() => AsyncQueryable.MaxAsync<TSource>(default(IAsyncQueryable<TSource>), default(Expression<Func<TSource, double?>>), default(CancellationToken))), source.Expression, selector, Expression.Constant(cancellationToken, typeof(CancellationToken))), cancellationToken);
#else
            return source.Provider.ExecuteAsync<double?>(Expression.Call(((MethodInfo)MethodBase.GetCurrentMethod()).MakeGenericMethod(typeof(TSource)), source.Expression, selector, Expression.Constant(cancellationToken, typeof(CancellationToken))), cancellationToken);
#endif
        }

        public static Task<double> MaxAsync<TSource>(this IAsyncQueryable<TSource> source, Expression<Func<TSource, double>> selector, CancellationToken cancellationToken = default)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));
            if (selector == null)
                throw new ArgumentNullException(nameof(selector));

#if CRIPPLED_REFLECTION
            return source.Provider.ExecuteAsync<double>(Expression.Call(InfoOf(() => AsyncQueryable.MaxAsync<TSource>(default(IAsyncQueryable<TSource>), default(Expression<Func<TSource, double>>), default(CancellationToken))), source.Expression, selector, Expression.Constant(cancellationToken, typeof(CancellationToken))), cancellationToken);
#else
            return source.Provider.ExecuteAsync<double>(Expression.Call(((MethodInfo)MethodBase.GetCurrentMethod()).MakeGenericMethod(typeof(TSource)), source.Expression, selector, Expression.Constant(cancellationToken, typeof(CancellationToken))), cancellationToken);
#endif
        }

        public static Task<float?> MaxAsync<TSource>(this IAsyncQueryable<TSource> source, Expression<Func<TSource, float?>> selector, CancellationToken cancellationToken = default)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));
            if (selector == null)
                throw new ArgumentNullException(nameof(selector));

#if CRIPPLED_REFLECTION
            return source.Provider.ExecuteAsync<float?>(Expression.Call(InfoOf(() => AsyncQueryable.MaxAsync<TSource>(default(IAsyncQueryable<TSource>), default(Expression<Func<TSource, float?>>), default(CancellationToken))), source.Expression, selector, Expression.Constant(cancellationToken, typeof(CancellationToken))), cancellationToken);
#else
            return source.Provider.ExecuteAsync<float?>(Expression.Call(((MethodInfo)MethodBase.GetCurrentMethod()).MakeGenericMethod(typeof(TSource)), source.Expression, selector, Expression.Constant(cancellationToken, typeof(CancellationToken))), cancellationToken);
#endif
        }

        public static Task<float> MaxAsync<TSource>(this IAsyncQueryable<TSource> source, Expression<Func<TSource, float>> selector, CancellationToken cancellationToken = default)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));
            if (selector == null)
                throw new ArgumentNullException(nameof(selector));

#if CRIPPLED_REFLECTION
            return source.Provider.ExecuteAsync<float>(Expression.Call(InfoOf(() => AsyncQueryable.MaxAsync<TSource>(default(IAsyncQueryable<TSource>), default(Expression<Func<TSource, float>>), default(CancellationToken))), source.Expression, selector, Expression.Constant(cancellationToken, typeof(CancellationToken))), cancellationToken);
#else
            return source.Provider.ExecuteAsync<float>(Expression.Call(((MethodInfo)MethodBase.GetCurrentMethod()).MakeGenericMethod(typeof(TSource)), source.Expression, selector, Expression.Constant(cancellationToken, typeof(CancellationToken))), cancellationToken);
#endif
        }

        public static Task<int?> MaxAsync<TSource>(this IAsyncQueryable<TSource> source, Expression<Func<TSource, int?>> selector, CancellationToken cancellationToken = default)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));
            if (selector == null)
                throw new ArgumentNullException(nameof(selector));

#if CRIPPLED_REFLECTION
            return source.Provider.ExecuteAsync<int?>(Expression.Call(InfoOf(() => AsyncQueryable.MaxAsync<TSource>(default(IAsyncQueryable<TSource>), default(Expression<Func<TSource, int?>>), default(CancellationToken))), source.Expression, selector, Expression.Constant(cancellationToken, typeof(CancellationToken))), cancellationToken);
#else
            return source.Provider.ExecuteAsync<int?>(Expression.Call(((MethodInfo)MethodBase.GetCurrentMethod()).MakeGenericMethod(typeof(TSource)), source.Expression, selector, Expression.Constant(cancellationToken, typeof(CancellationToken))), cancellationToken);
#endif
        }

        public static Task<int> MaxAsync<TSource>(this IAsyncQueryable<TSource> source, Expression<Func<TSource, int>> selector, CancellationToken cancellationToken = default)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));
            if (selector == null)
                throw new ArgumentNullException(nameof(selector));

#if CRIPPLED_REFLECTION
            return source.Provider.ExecuteAsync<int>(Expression.Call(InfoOf(() => AsyncQueryable.MaxAsync<TSource>(default(IAsyncQueryable<TSource>), default(Expression<Func<TSource, int>>), default(CancellationToken))), source.Expression, selector, Expression.Constant(cancellationToken, typeof(CancellationToken))), cancellationToken);
#else
            return source.Provider.ExecuteAsync<int>(Expression.Call(((MethodInfo)MethodBase.GetCurrentMethod()).MakeGenericMethod(typeof(TSource)), source.Expression, selector, Expression.Constant(cancellationToken, typeof(CancellationToken))), cancellationToken);
#endif
        }

        public static Task<long?> MaxAsync<TSource>(this IAsyncQueryable<TSource> source, Expression<Func<TSource, long?>> selector, CancellationToken cancellationToken = default)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));
            if (selector == null)
                throw new ArgumentNullException(nameof(selector));

#if CRIPPLED_REFLECTION
            return source.Provider.ExecuteAsync<long?>(Expression.Call(InfoOf(() => AsyncQueryable.MaxAsync<TSource>(default(IAsyncQueryable<TSource>), default(Expression<Func<TSource, long?>>), default(CancellationToken))), source.Expression, selector, Expression.Constant(cancellationToken, typeof(CancellationToken))), cancellationToken);
#else
            return source.Provider.ExecuteAsync<long?>(Expression.Call(((MethodInfo)MethodBase.GetCurrentMethod()).MakeGenericMethod(typeof(TSource)), source.Expression, selector, Expression.Constant(cancellationToken, typeof(CancellationToken))), cancellationToken);
#endif
        }

        public static Task<long> MaxAsync<TSource>(this IAsyncQueryable<TSource> source, Expression<Func<TSource, long>> selector, CancellationToken cancellationToken = default)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));
            if (selector == null)
                throw new ArgumentNullException(nameof(selector));

#if CRIPPLED_REFLECTION
            return source.Provider.ExecuteAsync<long>(Expression.Call(InfoOf(() => AsyncQueryable.MaxAsync<TSource>(default(IAsyncQueryable<TSource>), default(Expression<Func<TSource, long>>), default(CancellationToken))), source.Expression, selector, Expression.Constant(cancellationToken, typeof(CancellationToken))), cancellationToken);
#else
            return source.Provider.ExecuteAsync<long>(Expression.Call(((MethodInfo)MethodBase.GetCurrentMethod()).MakeGenericMethod(typeof(TSource)), source.Expression, selector, Expression.Constant(cancellationToken, typeof(CancellationToken))), cancellationToken);
#endif
        }

        public static Task<decimal?> MaxAsync<TSource>(this IAsyncQueryable<TSource> source, Expression<Func<TSource, ValueTask<decimal?>>> selector, CancellationToken cancellationToken = default)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));
            if (selector == null)
                throw new ArgumentNullException(nameof(selector));

#if CRIPPLED_REFLECTION
            return source.Provider.ExecuteAsync<decimal?>(Expression.Call(InfoOf(() => AsyncQueryable.MaxAsync<TSource>(default(IAsyncQueryable<TSource>), default(Expression<Func<TSource, ValueTask<decimal?>>>), default(CancellationToken))), source.Expression, selector, Expression.Constant(cancellationToken, typeof(CancellationToken))), cancellationToken);
#else
            return source.Provider.ExecuteAsync<decimal?>(Expression.Call(((MethodInfo)MethodBase.GetCurrentMethod()).MakeGenericMethod(typeof(TSource)), source.Expression, selector, Expression.Constant(cancellationToken, typeof(CancellationToken))), cancellationToken);
#endif
        }

        public static Task<decimal> MaxAsync<TSource>(this IAsyncQueryable<TSource> source, Expression<Func<TSource, ValueTask<decimal>>> selector, CancellationToken cancellationToken = default)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));
            if (selector == null)
                throw new ArgumentNullException(nameof(selector));

#if CRIPPLED_REFLECTION
            return source.Provider.ExecuteAsync<decimal>(Expression.Call(InfoOf(() => AsyncQueryable.MaxAsync<TSource>(default(IAsyncQueryable<TSource>), default(Expression<Func<TSource, ValueTask<decimal>>>), default(CancellationToken))), source.Expression, selector, Expression.Constant(cancellationToken, typeof(CancellationToken))), cancellationToken);
#else
            return source.Provider.ExecuteAsync<decimal>(Expression.Call(((MethodInfo)MethodBase.GetCurrentMethod()).MakeGenericMethod(typeof(TSource)), source.Expression, selector, Expression.Constant(cancellationToken, typeof(CancellationToken))), cancellationToken);
#endif
        }

        public static Task<double?> MaxAsync<TSource>(this IAsyncQueryable<TSource> source, Expression<Func<TSource, ValueTask<double?>>> selector, CancellationToken cancellationToken = default)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));
            if (selector == null)
                throw new ArgumentNullException(nameof(selector));

#if CRIPPLED_REFLECTION
            return source.Provider.ExecuteAsync<double?>(Expression.Call(InfoOf(() => AsyncQueryable.MaxAsync<TSource>(default(IAsyncQueryable<TSource>), default(Expression<Func<TSource, ValueTask<double?>>>), default(CancellationToken))), source.Expression, selector, Expression.Constant(cancellationToken, typeof(CancellationToken))), cancellationToken);
#else
            return source.Provider.ExecuteAsync<double?>(Expression.Call(((MethodInfo)MethodBase.GetCurrentMethod()).MakeGenericMethod(typeof(TSource)), source.Expression, selector, Expression.Constant(cancellationToken, typeof(CancellationToken))), cancellationToken);
#endif
        }

        public static Task<double> MaxAsync<TSource>(this IAsyncQueryable<TSource> source, Expression<Func<TSource, ValueTask<double>>> selector, CancellationToken cancellationToken = default)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));
            if (selector == null)
                throw new ArgumentNullException(nameof(selector));

#if CRIPPLED_REFLECTION
            return source.Provider.ExecuteAsync<double>(Expression.Call(InfoOf(() => AsyncQueryable.MaxAsync<TSource>(default(IAsyncQueryable<TSource>), default(Expression<Func<TSource, ValueTask<double>>>), default(CancellationToken))), source.Expression, selector, Expression.Constant(cancellationToken, typeof(CancellationToken))), cancellationToken);
#else
            return source.Provider.ExecuteAsync<double>(Expression.Call(((MethodInfo)MethodBase.GetCurrentMethod()).MakeGenericMethod(typeof(TSource)), source.Expression, selector, Expression.Constant(cancellationToken, typeof(CancellationToken))), cancellationToken);
#endif
        }

        public static Task<float?> MaxAsync<TSource>(this IAsyncQueryable<TSource> source, Expression<Func<TSource, ValueTask<float?>>> selector, CancellationToken cancellationToken = default)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));
            if (selector == null)
                throw new ArgumentNullException(nameof(selector));

#if CRIPPLED_REFLECTION
            return source.Provider.ExecuteAsync<float?>(Expression.Call(InfoOf(() => AsyncQueryable.MaxAsync<TSource>(default(IAsyncQueryable<TSource>), default(Expression<Func<TSource, ValueTask<float?>>>), default(CancellationToken))), source.Expression, selector, Expression.Constant(cancellationToken, typeof(CancellationToken))), cancellationToken);
#else
            return source.Provider.ExecuteAsync<float?>(Expression.Call(((MethodInfo)MethodBase.GetCurrentMethod()).MakeGenericMethod(typeof(TSource)), source.Expression, selector, Expression.Constant(cancellationToken, typeof(CancellationToken))), cancellationToken);
#endif
        }

        public static Task<float> MaxAsync<TSource>(this IAsyncQueryable<TSource> source, Expression<Func<TSource, ValueTask<float>>> selector, CancellationToken cancellationToken = default)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));
            if (selector == null)
                throw new ArgumentNullException(nameof(selector));

#if CRIPPLED_REFLECTION
            return source.Provider.ExecuteAsync<float>(Expression.Call(InfoOf(() => AsyncQueryable.MaxAsync<TSource>(default(IAsyncQueryable<TSource>), default(Expression<Func<TSource, ValueTask<float>>>), default(CancellationToken))), source.Expression, selector, Expression.Constant(cancellationToken, typeof(CancellationToken))), cancellationToken);
#else
            return source.Provider.ExecuteAsync<float>(Expression.Call(((MethodInfo)MethodBase.GetCurrentMethod()).MakeGenericMethod(typeof(TSource)), source.Expression, selector, Expression.Constant(cancellationToken, typeof(CancellationToken))), cancellationToken);
#endif
        }

        public static Task<int?> MaxAsync<TSource>(this IAsyncQueryable<TSource> source, Expression<Func<TSource, ValueTask<int?>>> selector, CancellationToken cancellationToken = default)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));
            if (selector == null)
                throw new ArgumentNullException(nameof(selector));

#if CRIPPLED_REFLECTION
            return source.Provider.ExecuteAsync<int?>(Expression.Call(InfoOf(() => AsyncQueryable.MaxAsync<TSource>(default(IAsyncQueryable<TSource>), default(Expression<Func<TSource, ValueTask<int?>>>), default(CancellationToken))), source.Expression, selector, Expression.Constant(cancellationToken, typeof(CancellationToken))), cancellationToken);
#else
            return source.Provider.ExecuteAsync<int?>(Expression.Call(((MethodInfo)MethodBase.GetCurrentMethod()).MakeGenericMethod(typeof(TSource)), source.Expression, selector, Expression.Constant(cancellationToken, typeof(CancellationToken))), cancellationToken);
#endif
        }

        public static Task<int> MaxAsync<TSource>(this IAsyncQueryable<TSource> source, Expression<Func<TSource, ValueTask<int>>> selector, CancellationToken cancellationToken = default)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));
            if (selector == null)
                throw new ArgumentNullException(nameof(selector));

#if CRIPPLED_REFLECTION
            return source.Provider.ExecuteAsync<int>(Expression.Call(InfoOf(() => AsyncQueryable.MaxAsync<TSource>(default(IAsyncQueryable<TSource>), default(Expression<Func<TSource, ValueTask<int>>>), default(CancellationToken))), source.Expression, selector, Expression.Constant(cancellationToken, typeof(CancellationToken))), cancellationToken);
#else
            return source.Provider.ExecuteAsync<int>(Expression.Call(((MethodInfo)MethodBase.GetCurrentMethod()).MakeGenericMethod(typeof(TSource)), source.Expression, selector, Expression.Constant(cancellationToken, typeof(CancellationToken))), cancellationToken);
#endif
        }

        public static Task<long?> MaxAsync<TSource>(this IAsyncQueryable<TSource> source, Expression<Func<TSource, ValueTask<long?>>> selector, CancellationToken cancellationToken = default)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));
            if (selector == null)
                throw new ArgumentNullException(nameof(selector));

#if CRIPPLED_REFLECTION
            return source.Provider.ExecuteAsync<long?>(Expression.Call(InfoOf(() => AsyncQueryable.MaxAsync<TSource>(default(IAsyncQueryable<TSource>), default(Expression<Func<TSource, ValueTask<long?>>>), default(CancellationToken))), source.Expression, selector, Expression.Constant(cancellationToken, typeof(CancellationToken))), cancellationToken);
#else
            return source.Provider.ExecuteAsync<long?>(Expression.Call(((MethodInfo)MethodBase.GetCurrentMethod()).MakeGenericMethod(typeof(TSource)), source.Expression, selector, Expression.Constant(cancellationToken, typeof(CancellationToken))), cancellationToken);
#endif
        }

        public static Task<long> MaxAsync<TSource>(this IAsyncQueryable<TSource> source, Expression<Func<TSource, ValueTask<long>>> selector, CancellationToken cancellationToken = default)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));
            if (selector == null)
                throw new ArgumentNullException(nameof(selector));

#if CRIPPLED_REFLECTION
            return source.Provider.ExecuteAsync<long>(Expression.Call(InfoOf(() => AsyncQueryable.MaxAsync<TSource>(default(IAsyncQueryable<TSource>), default(Expression<Func<TSource, ValueTask<long>>>), default(CancellationToken))), source.Expression, selector, Expression.Constant(cancellationToken, typeof(CancellationToken))), cancellationToken);
#else
            return source.Provider.ExecuteAsync<long>(Expression.Call(((MethodInfo)MethodBase.GetCurrentMethod()).MakeGenericMethod(typeof(TSource)), source.Expression, selector, Expression.Constant(cancellationToken, typeof(CancellationToken))), cancellationToken);
#endif
        }

        public static Task<TResult> MaxAsync<TSource, TResult>(this IAsyncQueryable<TSource> source, Expression<Func<TSource, CancellationToken, ValueTask<TResult>>> selector, CancellationToken cancellationToken = default)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));
            if (selector == null)
                throw new ArgumentNullException(nameof(selector));

#if CRIPPLED_REFLECTION
            return source.Provider.ExecuteAsync<TResult>(Expression.Call(InfoOf(() => AsyncQueryable.MaxAsync<TSource, TResult>(default(IAsyncQueryable<TSource>), default(Expression<Func<TSource, CancellationToken, ValueTask<TResult>>>), default(CancellationToken))), source.Expression, selector, Expression.Constant(cancellationToken, typeof(CancellationToken))), cancellationToken);
#else
            return source.Provider.ExecuteAsync<TResult>(Expression.Call(((MethodInfo)MethodBase.GetCurrentMethod()).MakeGenericMethod(typeof(TSource), typeof(TResult)), source.Expression, selector, Expression.Constant(cancellationToken, typeof(CancellationToken))), cancellationToken);
#endif
        }

        public static Task<TResult> MaxAsync<TSource, TResult>(this IAsyncQueryable<TSource> source, Expression<Func<TSource, TResult>> selector, CancellationToken cancellationToken = default)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));
            if (selector == null)
                throw new ArgumentNullException(nameof(selector));

#if CRIPPLED_REFLECTION
            return source.Provider.ExecuteAsync<TResult>(Expression.Call(InfoOf(() => AsyncQueryable.MaxAsync<TSource, TResult>(default(IAsyncQueryable<TSource>), default(Expression<Func<TSource, TResult>>), default(CancellationToken))), source.Expression, selector, Expression.Constant(cancellationToken, typeof(CancellationToken))), cancellationToken);
#else
            return source.Provider.ExecuteAsync<TResult>(Expression.Call(((MethodInfo)MethodBase.GetCurrentMethod()).MakeGenericMethod(typeof(TSource), typeof(TResult)), source.Expression, selector, Expression.Constant(cancellationToken, typeof(CancellationToken))), cancellationToken);
#endif
        }

        public static Task<TResult> MaxAsync<TSource, TResult>(this IAsyncQueryable<TSource> source, Expression<Func<TSource, ValueTask<TResult>>> selector, CancellationToken cancellationToken = default)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));
            if (selector == null)
                throw new ArgumentNullException(nameof(selector));

#if CRIPPLED_REFLECTION
            return source.Provider.ExecuteAsync<TResult>(Expression.Call(InfoOf(() => AsyncQueryable.MaxAsync<TSource, TResult>(default(IAsyncQueryable<TSource>), default(Expression<Func<TSource, ValueTask<TResult>>>), default(CancellationToken))), source.Expression, selector, Expression.Constant(cancellationToken, typeof(CancellationToken))), cancellationToken);
#else
            return source.Provider.ExecuteAsync<TResult>(Expression.Call(((MethodInfo)MethodBase.GetCurrentMethod()).MakeGenericMethod(typeof(TSource), typeof(TResult)), source.Expression, selector, Expression.Constant(cancellationToken, typeof(CancellationToken))), cancellationToken);
#endif
        }

        public static Task<decimal?> MinAsync(this IAsyncQueryable<decimal?> source, CancellationToken cancellationToken = default)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));

#if CRIPPLED_REFLECTION
            return source.Provider.ExecuteAsync<decimal?>(Expression.Call(InfoOf(() => AsyncQueryable.MinAsync(default(IAsyncQueryable<decimal?>), default(CancellationToken))), source.Expression, Expression.Constant(cancellationToken, typeof(CancellationToken))), cancellationToken);
#else
            return source.Provider.ExecuteAsync<decimal?>(Expression.Call((MethodInfo)MethodBase.GetCurrentMethod(), source.Expression, Expression.Constant(cancellationToken, typeof(CancellationToken))), cancellationToken);
#endif
        }

        public static Task<decimal> MinAsync(this IAsyncQueryable<decimal> source, CancellationToken cancellationToken = default)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));

#if CRIPPLED_REFLECTION
            return source.Provider.ExecuteAsync<decimal>(Expression.Call(InfoOf(() => AsyncQueryable.MinAsync(default(IAsyncQueryable<decimal>), default(CancellationToken))), source.Expression, Expression.Constant(cancellationToken, typeof(CancellationToken))), cancellationToken);
#else
            return source.Provider.ExecuteAsync<decimal>(Expression.Call((MethodInfo)MethodBase.GetCurrentMethod(), source.Expression, Expression.Constant(cancellationToken, typeof(CancellationToken))), cancellationToken);
#endif
        }

        public static Task<double?> MinAsync(this IAsyncQueryable<double?> source, CancellationToken cancellationToken = default)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));

#if CRIPPLED_REFLECTION
            return source.Provider.ExecuteAsync<double?>(Expression.Call(InfoOf(() => AsyncQueryable.MinAsync(default(IAsyncQueryable<double?>), default(CancellationToken))), source.Expression, Expression.Constant(cancellationToken, typeof(CancellationToken))), cancellationToken);
#else
            return source.Provider.ExecuteAsync<double?>(Expression.Call((MethodInfo)MethodBase.GetCurrentMethod(), source.Expression, Expression.Constant(cancellationToken, typeof(CancellationToken))), cancellationToken);
#endif
        }

        public static Task<double> MinAsync(this IAsyncQueryable<double> source, CancellationToken cancellationToken = default)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));

#if CRIPPLED_REFLECTION
            return source.Provider.ExecuteAsync<double>(Expression.Call(InfoOf(() => AsyncQueryable.MinAsync(default(IAsyncQueryable<double>), default(CancellationToken))), source.Expression, Expression.Constant(cancellationToken, typeof(CancellationToken))), cancellationToken);
#else
            return source.Provider.ExecuteAsync<double>(Expression.Call((MethodInfo)MethodBase.GetCurrentMethod(), source.Expression, Expression.Constant(cancellationToken, typeof(CancellationToken))), cancellationToken);
#endif
        }

        public static Task<float?> MinAsync(this IAsyncQueryable<float?> source, CancellationToken cancellationToken = default)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));

#if CRIPPLED_REFLECTION
            return source.Provider.ExecuteAsync<float?>(Expression.Call(InfoOf(() => AsyncQueryable.MinAsync(default(IAsyncQueryable<float?>), default(CancellationToken))), source.Expression, Expression.Constant(cancellationToken, typeof(CancellationToken))), cancellationToken);
#else
            return source.Provider.ExecuteAsync<float?>(Expression.Call((MethodInfo)MethodBase.GetCurrentMethod(), source.Expression, Expression.Constant(cancellationToken, typeof(CancellationToken))), cancellationToken);
#endif
        }

        public static Task<float> MinAsync(this IAsyncQueryable<float> source, CancellationToken cancellationToken = default)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));

#if CRIPPLED_REFLECTION
            return source.Provider.ExecuteAsync<float>(Expression.Call(InfoOf(() => AsyncQueryable.MinAsync(default(IAsyncQueryable<float>), default(CancellationToken))), source.Expression, Expression.Constant(cancellationToken, typeof(CancellationToken))), cancellationToken);
#else
            return source.Provider.ExecuteAsync<float>(Expression.Call((MethodInfo)MethodBase.GetCurrentMethod(), source.Expression, Expression.Constant(cancellationToken, typeof(CancellationToken))), cancellationToken);
#endif
        }

        public static Task<int?> MinAsync(this IAsyncQueryable<int?> source, CancellationToken cancellationToken = default)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));

#if CRIPPLED_REFLECTION
            return source.Provider.ExecuteAsync<int?>(Expression.Call(InfoOf(() => AsyncQueryable.MinAsync(default(IAsyncQueryable<int?>), default(CancellationToken))), source.Expression, Expression.Constant(cancellationToken, typeof(CancellationToken))), cancellationToken);
#else
            return source.Provider.ExecuteAsync<int?>(Expression.Call((MethodInfo)MethodBase.GetCurrentMethod(), source.Expression, Expression.Constant(cancellationToken, typeof(CancellationToken))), cancellationToken);
#endif
        }

        public static Task<int> MinAsync(this IAsyncQueryable<int> source, CancellationToken cancellationToken = default)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));

#if CRIPPLED_REFLECTION
            return source.Provider.ExecuteAsync<int>(Expression.Call(InfoOf(() => AsyncQueryable.MinAsync(default(IAsyncQueryable<int>), default(CancellationToken))), source.Expression, Expression.Constant(cancellationToken, typeof(CancellationToken))), cancellationToken);
#else
            return source.Provider.ExecuteAsync<int>(Expression.Call((MethodInfo)MethodBase.GetCurrentMethod(), source.Expression, Expression.Constant(cancellationToken, typeof(CancellationToken))), cancellationToken);
#endif
        }

        public static Task<long?> MinAsync(this IAsyncQueryable<long?> source, CancellationToken cancellationToken = default)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));

#if CRIPPLED_REFLECTION
            return source.Provider.ExecuteAsync<long?>(Expression.Call(InfoOf(() => AsyncQueryable.MinAsync(default(IAsyncQueryable<long?>), default(CancellationToken))), source.Expression, Expression.Constant(cancellationToken, typeof(CancellationToken))), cancellationToken);
#else
            return source.Provider.ExecuteAsync<long?>(Expression.Call((MethodInfo)MethodBase.GetCurrentMethod(), source.Expression, Expression.Constant(cancellationToken, typeof(CancellationToken))), cancellationToken);
#endif
        }

        public static Task<long> MinAsync(this IAsyncQueryable<long> source, CancellationToken cancellationToken = default)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));

#if CRIPPLED_REFLECTION
            return source.Provider.ExecuteAsync<long>(Expression.Call(InfoOf(() => AsyncQueryable.MinAsync(default(IAsyncQueryable<long>), default(CancellationToken))), source.Expression, Expression.Constant(cancellationToken, typeof(CancellationToken))), cancellationToken);
#else
            return source.Provider.ExecuteAsync<long>(Expression.Call((MethodInfo)MethodBase.GetCurrentMethod(), source.Expression, Expression.Constant(cancellationToken, typeof(CancellationToken))), cancellationToken);
#endif
        }

        public static Task<TSource> MinAsync<TSource>(this IAsyncQueryable<TSource> source, CancellationToken cancellationToken = default)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));

#if CRIPPLED_REFLECTION
            return source.Provider.ExecuteAsync<TSource>(Expression.Call(InfoOf(() => AsyncQueryable.MinAsync<TSource>(default(IAsyncQueryable<TSource>), default(CancellationToken))), source.Expression, Expression.Constant(cancellationToken, typeof(CancellationToken))), cancellationToken);
#else
            return source.Provider.ExecuteAsync<TSource>(Expression.Call(((MethodInfo)MethodBase.GetCurrentMethod()).MakeGenericMethod(typeof(TSource)), source.Expression, Expression.Constant(cancellationToken, typeof(CancellationToken))), cancellationToken);
#endif
        }

        public static Task<decimal?> MinAsync<TSource>(this IAsyncQueryable<TSource> source, Expression<Func<TSource, CancellationToken, ValueTask<decimal?>>> selector, CancellationToken cancellationToken = default)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));
            if (selector == null)
                throw new ArgumentNullException(nameof(selector));

#if CRIPPLED_REFLECTION
            return source.Provider.ExecuteAsync<decimal?>(Expression.Call(InfoOf(() => AsyncQueryable.MinAsync<TSource>(default(IAsyncQueryable<TSource>), default(Expression<Func<TSource, CancellationToken, ValueTask<decimal?>>>), default(CancellationToken))), source.Expression, selector, Expression.Constant(cancellationToken, typeof(CancellationToken))), cancellationToken);
#else
            return source.Provider.ExecuteAsync<decimal?>(Expression.Call(((MethodInfo)MethodBase.GetCurrentMethod()).MakeGenericMethod(typeof(TSource)), source.Expression, selector, Expression.Constant(cancellationToken, typeof(CancellationToken))), cancellationToken);
#endif
        }

        public static Task<decimal> MinAsync<TSource>(this IAsyncQueryable<TSource> source, Expression<Func<TSource, CancellationToken, ValueTask<decimal>>> selector, CancellationToken cancellationToken = default)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));
            if (selector == null)
                throw new ArgumentNullException(nameof(selector));

#if CRIPPLED_REFLECTION
            return source.Provider.ExecuteAsync<decimal>(Expression.Call(InfoOf(() => AsyncQueryable.MinAsync<TSource>(default(IAsyncQueryable<TSource>), default(Expression<Func<TSource, CancellationToken, ValueTask<decimal>>>), default(CancellationToken))), source.Expression, selector, Expression.Constant(cancellationToken, typeof(CancellationToken))), cancellationToken);
#else
            return source.Provider.ExecuteAsync<decimal>(Expression.Call(((MethodInfo)MethodBase.GetCurrentMethod()).MakeGenericMethod(typeof(TSource)), source.Expression, selector, Expression.Constant(cancellationToken, typeof(CancellationToken))), cancellationToken);
#endif
        }

        public static Task<double?> MinAsync<TSource>(this IAsyncQueryable<TSource> source, Expression<Func<TSource, CancellationToken, ValueTask<double?>>> selector, CancellationToken cancellationToken = default)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));
            if (selector == null)
                throw new ArgumentNullException(nameof(selector));

#if CRIPPLED_REFLECTION
            return source.Provider.ExecuteAsync<double?>(Expression.Call(InfoOf(() => AsyncQueryable.MinAsync<TSource>(default(IAsyncQueryable<TSource>), default(Expression<Func<TSource, CancellationToken, ValueTask<double?>>>), default(CancellationToken))), source.Expression, selector, Expression.Constant(cancellationToken, typeof(CancellationToken))), cancellationToken);
#else
            return source.Provider.ExecuteAsync<double?>(Expression.Call(((MethodInfo)MethodBase.GetCurrentMethod()).MakeGenericMethod(typeof(TSource)), source.Expression, selector, Expression.Constant(cancellationToken, typeof(CancellationToken))), cancellationToken);
#endif
        }

        public static Task<double> MinAsync<TSource>(this IAsyncQueryable<TSource> source, Expression<Func<TSource, CancellationToken, ValueTask<double>>> selector, CancellationToken cancellationToken = default)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));
            if (selector == null)
                throw new ArgumentNullException(nameof(selector));

#if CRIPPLED_REFLECTION
            return source.Provider.ExecuteAsync<double>(Expression.Call(InfoOf(() => AsyncQueryable.MinAsync<TSource>(default(IAsyncQueryable<TSource>), default(Expression<Func<TSource, CancellationToken, ValueTask<double>>>), default(CancellationToken))), source.Expression, selector, Expression.Constant(cancellationToken, typeof(CancellationToken))), cancellationToken);
#else
            return source.Provider.ExecuteAsync<double>(Expression.Call(((MethodInfo)MethodBase.GetCurrentMethod()).MakeGenericMethod(typeof(TSource)), source.Expression, selector, Expression.Constant(cancellationToken, typeof(CancellationToken))), cancellationToken);
#endif
        }

        public static Task<float?> MinAsync<TSource>(this IAsyncQueryable<TSource> source, Expression<Func<TSource, CancellationToken, ValueTask<float?>>> selector, CancellationToken cancellationToken = default)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));
            if (selector == null)
                throw new ArgumentNullException(nameof(selector));

#if CRIPPLED_REFLECTION
            return source.Provider.ExecuteAsync<float?>(Expression.Call(InfoOf(() => AsyncQueryable.MinAsync<TSource>(default(IAsyncQueryable<TSource>), default(Expression<Func<TSource, CancellationToken, ValueTask<float?>>>), default(CancellationToken))), source.Expression, selector, Expression.Constant(cancellationToken, typeof(CancellationToken))), cancellationToken);
#else
            return source.Provider.ExecuteAsync<float?>(Expression.Call(((MethodInfo)MethodBase.GetCurrentMethod()).MakeGenericMethod(typeof(TSource)), source.Expression, selector, Expression.Constant(cancellationToken, typeof(CancellationToken))), cancellationToken);
#endif
        }

        public static Task<float> MinAsync<TSource>(this IAsyncQueryable<TSource> source, Expression<Func<TSource, CancellationToken, ValueTask<float>>> selector, CancellationToken cancellationToken = default)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));
            if (selector == null)
                throw new ArgumentNullException(nameof(selector));

#if CRIPPLED_REFLECTION
            return source.Provider.ExecuteAsync<float>(Expression.Call(InfoOf(() => AsyncQueryable.MinAsync<TSource>(default(IAsyncQueryable<TSource>), default(Expression<Func<TSource, CancellationToken, ValueTask<float>>>), default(CancellationToken))), source.Expression, selector, Expression.Constant(cancellationToken, typeof(CancellationToken))), cancellationToken);
#else
            return source.Provider.ExecuteAsync<float>(Expression.Call(((MethodInfo)MethodBase.GetCurrentMethod()).MakeGenericMethod(typeof(TSource)), source.Expression, selector, Expression.Constant(cancellationToken, typeof(CancellationToken))), cancellationToken);
#endif
        }

        public static Task<int?> MinAsync<TSource>(this IAsyncQueryable<TSource> source, Expression<Func<TSource, CancellationToken, ValueTask<int?>>> selector, CancellationToken cancellationToken = default)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));
            if (selector == null)
                throw new ArgumentNullException(nameof(selector));

#if CRIPPLED_REFLECTION
            return source.Provider.ExecuteAsync<int?>(Expression.Call(InfoOf(() => AsyncQueryable.MinAsync<TSource>(default(IAsyncQueryable<TSource>), default(Expression<Func<TSource, CancellationToken, ValueTask<int?>>>), default(CancellationToken))), source.Expression, selector, Expression.Constant(cancellationToken, typeof(CancellationToken))), cancellationToken);
#else
            return source.Provider.ExecuteAsync<int?>(Expression.Call(((MethodInfo)MethodBase.GetCurrentMethod()).MakeGenericMethod(typeof(TSource)), source.Expression, selector, Expression.Constant(cancellationToken, typeof(CancellationToken))), cancellationToken);
#endif
        }

        public static Task<int> MinAsync<TSource>(this IAsyncQueryable<TSource> source, Expression<Func<TSource, CancellationToken, ValueTask<int>>> selector, CancellationToken cancellationToken = default)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));
            if (selector == null)
                throw new ArgumentNullException(nameof(selector));

#if CRIPPLED_REFLECTION
            return source.Provider.ExecuteAsync<int>(Expression.Call(InfoOf(() => AsyncQueryable.MinAsync<TSource>(default(IAsyncQueryable<TSource>), default(Expression<Func<TSource, CancellationToken, ValueTask<int>>>), default(CancellationToken))), source.Expression, selector, Expression.Constant(cancellationToken, typeof(CancellationToken))), cancellationToken);
#else
            return source.Provider.ExecuteAsync<int>(Expression.Call(((MethodInfo)MethodBase.GetCurrentMethod()).MakeGenericMethod(typeof(TSource)), source.Expression, selector, Expression.Constant(cancellationToken, typeof(CancellationToken))), cancellationToken);
#endif
        }

        public static Task<long?> MinAsync<TSource>(this IAsyncQueryable<TSource> source, Expression<Func<TSource, CancellationToken, ValueTask<long?>>> selector, CancellationToken cancellationToken = default)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));
            if (selector == null)
                throw new ArgumentNullException(nameof(selector));

#if CRIPPLED_REFLECTION
            return source.Provider.ExecuteAsync<long?>(Expression.Call(InfoOf(() => AsyncQueryable.MinAsync<TSource>(default(IAsyncQueryable<TSource>), default(Expression<Func<TSource, CancellationToken, ValueTask<long?>>>), default(CancellationToken))), source.Expression, selector, Expression.Constant(cancellationToken, typeof(CancellationToken))), cancellationToken);
#else
            return source.Provider.ExecuteAsync<long?>(Expression.Call(((MethodInfo)MethodBase.GetCurrentMethod()).MakeGenericMethod(typeof(TSource)), source.Expression, selector, Expression.Constant(cancellationToken, typeof(CancellationToken))), cancellationToken);
#endif
        }

        public static Task<long> MinAsync<TSource>(this IAsyncQueryable<TSource> source, Expression<Func<TSource, CancellationToken, ValueTask<long>>> selector, CancellationToken cancellationToken = default)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));
            if (selector == null)
                throw new ArgumentNullException(nameof(selector));

#if CRIPPLED_REFLECTION
            return source.Provider.ExecuteAsync<long>(Expression.Call(InfoOf(() => AsyncQueryable.MinAsync<TSource>(default(IAsyncQueryable<TSource>), default(Expression<Func<TSource, CancellationToken, ValueTask<long>>>), default(CancellationToken))), source.Expression, selector, Expression.Constant(cancellationToken, typeof(CancellationToken))), cancellationToken);
#else
            return source.Provider.ExecuteAsync<long>(Expression.Call(((MethodInfo)MethodBase.GetCurrentMethod()).MakeGenericMethod(typeof(TSource)), source.Expression, selector, Expression.Constant(cancellationToken, typeof(CancellationToken))), cancellationToken);
#endif
        }

        public static Task<decimal?> MinAsync<TSource>(this IAsyncQueryable<TSource> source, Expression<Func<TSource, decimal?>> selector, CancellationToken cancellationToken = default)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));
            if (selector == null)
                throw new ArgumentNullException(nameof(selector));

#if CRIPPLED_REFLECTION
            return source.Provider.ExecuteAsync<decimal?>(Expression.Call(InfoOf(() => AsyncQueryable.MinAsync<TSource>(default(IAsyncQueryable<TSource>), default(Expression<Func<TSource, decimal?>>), default(CancellationToken))), source.Expression, selector, Expression.Constant(cancellationToken, typeof(CancellationToken))), cancellationToken);
#else
            return source.Provider.ExecuteAsync<decimal?>(Expression.Call(((MethodInfo)MethodBase.GetCurrentMethod()).MakeGenericMethod(typeof(TSource)), source.Expression, selector, Expression.Constant(cancellationToken, typeof(CancellationToken))), cancellationToken);
#endif
        }

        public static Task<decimal> MinAsync<TSource>(this IAsyncQueryable<TSource> source, Expression<Func<TSource, decimal>> selector, CancellationToken cancellationToken = default)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));
            if (selector == null)
                throw new ArgumentNullException(nameof(selector));

#if CRIPPLED_REFLECTION
            return source.Provider.ExecuteAsync<decimal>(Expression.Call(InfoOf(() => AsyncQueryable.MinAsync<TSource>(default(IAsyncQueryable<TSource>), default(Expression<Func<TSource, decimal>>), default(CancellationToken))), source.Expression, selector, Expression.Constant(cancellationToken, typeof(CancellationToken))), cancellationToken);
#else
            return source.Provider.ExecuteAsync<decimal>(Expression.Call(((MethodInfo)MethodBase.GetCurrentMethod()).MakeGenericMethod(typeof(TSource)), source.Expression, selector, Expression.Constant(cancellationToken, typeof(CancellationToken))), cancellationToken);
#endif
        }

        public static Task<double?> MinAsync<TSource>(this IAsyncQueryable<TSource> source, Expression<Func<TSource, double?>> selector, CancellationToken cancellationToken = default)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));
            if (selector == null)
                throw new ArgumentNullException(nameof(selector));

#if CRIPPLED_REFLECTION
            return source.Provider.ExecuteAsync<double?>(Expression.Call(InfoOf(() => AsyncQueryable.MinAsync<TSource>(default(IAsyncQueryable<TSource>), default(Expression<Func<TSource, double?>>), default(CancellationToken))), source.Expression, selector, Expression.Constant(cancellationToken, typeof(CancellationToken))), cancellationToken);
#else
            return source.Provider.ExecuteAsync<double?>(Expression.Call(((MethodInfo)MethodBase.GetCurrentMethod()).MakeGenericMethod(typeof(TSource)), source.Expression, selector, Expression.Constant(cancellationToken, typeof(CancellationToken))), cancellationToken);
#endif
        }

        public static Task<double> MinAsync<TSource>(this IAsyncQueryable<TSource> source, Expression<Func<TSource, double>> selector, CancellationToken cancellationToken = default)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));
            if (selector == null)
                throw new ArgumentNullException(nameof(selector));

#if CRIPPLED_REFLECTION
            return source.Provider.ExecuteAsync<double>(Expression.Call(InfoOf(() => AsyncQueryable.MinAsync<TSource>(default(IAsyncQueryable<TSource>), default(Expression<Func<TSource, double>>), default(CancellationToken))), source.Expression, selector, Expression.Constant(cancellationToken, typeof(CancellationToken))), cancellationToken);
#else
            return source.Provider.ExecuteAsync<double>(Expression.Call(((MethodInfo)MethodBase.GetCurrentMethod()).MakeGenericMethod(typeof(TSource)), source.Expression, selector, Expression.Constant(cancellationToken, typeof(CancellationToken))), cancellationToken);
#endif
        }

        public static Task<float?> MinAsync<TSource>(this IAsyncQueryable<TSource> source, Expression<Func<TSource, float?>> selector, CancellationToken cancellationToken = default)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));
            if (selector == null)
                throw new ArgumentNullException(nameof(selector));

#if CRIPPLED_REFLECTION
            return source.Provider.ExecuteAsync<float?>(Expression.Call(InfoOf(() => AsyncQueryable.MinAsync<TSource>(default(IAsyncQueryable<TSource>), default(Expression<Func<TSource, float?>>), default(CancellationToken))), source.Expression, selector, Expression.Constant(cancellationToken, typeof(CancellationToken))), cancellationToken);
#else
            return source.Provider.ExecuteAsync<float?>(Expression.Call(((MethodInfo)MethodBase.GetCurrentMethod()).MakeGenericMethod(typeof(TSource)), source.Expression, selector, Expression.Constant(cancellationToken, typeof(CancellationToken))), cancellationToken);
#endif
        }

        public static Task<float> MinAsync<TSource>(this IAsyncQueryable<TSource> source, Expression<Func<TSource, float>> selector, CancellationToken cancellationToken = default)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));
            if (selector == null)
                throw new ArgumentNullException(nameof(selector));

#if CRIPPLED_REFLECTION
            return source.Provider.ExecuteAsync<float>(Expression.Call(InfoOf(() => AsyncQueryable.MinAsync<TSource>(default(IAsyncQueryable<TSource>), default(Expression<Func<TSource, float>>), default(CancellationToken))), source.Expression, selector, Expression.Constant(cancellationToken, typeof(CancellationToken))), cancellationToken);
#else
            return source.Provider.ExecuteAsync<float>(Expression.Call(((MethodInfo)MethodBase.GetCurrentMethod()).MakeGenericMethod(typeof(TSource)), source.Expression, selector, Expression.Constant(cancellationToken, typeof(CancellationToken))), cancellationToken);
#endif
        }

        public static Task<int?> MinAsync<TSource>(this IAsyncQueryable<TSource> source, Expression<Func<TSource, int?>> selector, CancellationToken cancellationToken = default)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));
            if (selector == null)
                throw new ArgumentNullException(nameof(selector));

#if CRIPPLED_REFLECTION
            return source.Provider.ExecuteAsync<int?>(Expression.Call(InfoOf(() => AsyncQueryable.MinAsync<TSource>(default(IAsyncQueryable<TSource>), default(Expression<Func<TSource, int?>>), default(CancellationToken))), source.Expression, selector, Expression.Constant(cancellationToken, typeof(CancellationToken))), cancellationToken);
#else
            return source.Provider.ExecuteAsync<int?>(Expression.Call(((MethodInfo)MethodBase.GetCurrentMethod()).MakeGenericMethod(typeof(TSource)), source.Expression, selector, Expression.Constant(cancellationToken, typeof(CancellationToken))), cancellationToken);
#endif
        }

        public static Task<int> MinAsync<TSource>(this IAsyncQueryable<TSource> source, Expression<Func<TSource, int>> selector, CancellationToken cancellationToken = default)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));
            if (selector == null)
                throw new ArgumentNullException(nameof(selector));

#if CRIPPLED_REFLECTION
            return source.Provider.ExecuteAsync<int>(Expression.Call(InfoOf(() => AsyncQueryable.MinAsync<TSource>(default(IAsyncQueryable<TSource>), default(Expression<Func<TSource, int>>), default(CancellationToken))), source.Expression, selector, Expression.Constant(cancellationToken, typeof(CancellationToken))), cancellationToken);
#else
            return source.Provider.ExecuteAsync<int>(Expression.Call(((MethodInfo)MethodBase.GetCurrentMethod()).MakeGenericMethod(typeof(TSource)), source.Expression, selector, Expression.Constant(cancellationToken, typeof(CancellationToken))), cancellationToken);
#endif
        }

        public static Task<long?> MinAsync<TSource>(this IAsyncQueryable<TSource> source, Expression<Func<TSource, long?>> selector, CancellationToken cancellationToken = default)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));
            if (selector == null)
                throw new ArgumentNullException(nameof(selector));

#if CRIPPLED_REFLECTION
            return source.Provider.ExecuteAsync<long?>(Expression.Call(InfoOf(() => AsyncQueryable.MinAsync<TSource>(default(IAsyncQueryable<TSource>), default(Expression<Func<TSource, long?>>), default(CancellationToken))), source.Expression, selector, Expression.Constant(cancellationToken, typeof(CancellationToken))), cancellationToken);
#else
            return source.Provider.ExecuteAsync<long?>(Expression.Call(((MethodInfo)MethodBase.GetCurrentMethod()).MakeGenericMethod(typeof(TSource)), source.Expression, selector, Expression.Constant(cancellationToken, typeof(CancellationToken))), cancellationToken);
#endif
        }

        public static Task<long> MinAsync<TSource>(this IAsyncQueryable<TSource> source, Expression<Func<TSource, long>> selector, CancellationToken cancellationToken = default)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));
            if (selector == null)
                throw new ArgumentNullException(nameof(selector));

#if CRIPPLED_REFLECTION
            return source.Provider.ExecuteAsync<long>(Expression.Call(InfoOf(() => AsyncQueryable.MinAsync<TSource>(default(IAsyncQueryable<TSource>), default(Expression<Func<TSource, long>>), default(CancellationToken))), source.Expression, selector, Expression.Constant(cancellationToken, typeof(CancellationToken))), cancellationToken);
#else
            return source.Provider.ExecuteAsync<long>(Expression.Call(((MethodInfo)MethodBase.GetCurrentMethod()).MakeGenericMethod(typeof(TSource)), source.Expression, selector, Expression.Constant(cancellationToken, typeof(CancellationToken))), cancellationToken);
#endif
        }

        public static Task<decimal?> MinAsync<TSource>(this IAsyncQueryable<TSource> source, Expression<Func<TSource, ValueTask<decimal?>>> selector, CancellationToken cancellationToken = default)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));
            if (selector == null)
                throw new ArgumentNullException(nameof(selector));

#if CRIPPLED_REFLECTION
            return source.Provider.ExecuteAsync<decimal?>(Expression.Call(InfoOf(() => AsyncQueryable.MinAsync<TSource>(default(IAsyncQueryable<TSource>), default(Expression<Func<TSource, ValueTask<decimal?>>>), default(CancellationToken))), source.Expression, selector, Expression.Constant(cancellationToken, typeof(CancellationToken))), cancellationToken);
#else
            return source.Provider.ExecuteAsync<decimal?>(Expression.Call(((MethodInfo)MethodBase.GetCurrentMethod()).MakeGenericMethod(typeof(TSource)), source.Expression, selector, Expression.Constant(cancellationToken, typeof(CancellationToken))), cancellationToken);
#endif
        }

        public static Task<decimal> MinAsync<TSource>(this IAsyncQueryable<TSource> source, Expression<Func<TSource, ValueTask<decimal>>> selector, CancellationToken cancellationToken = default)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));
            if (selector == null)
                throw new ArgumentNullException(nameof(selector));

#if CRIPPLED_REFLECTION
            return source.Provider.ExecuteAsync<decimal>(Expression.Call(InfoOf(() => AsyncQueryable.MinAsync<TSource>(default(IAsyncQueryable<TSource>), default(Expression<Func<TSource, ValueTask<decimal>>>), default(CancellationToken))), source.Expression, selector, Expression.Constant(cancellationToken, typeof(CancellationToken))), cancellationToken);
#else
            return source.Provider.ExecuteAsync<decimal>(Expression.Call(((MethodInfo)MethodBase.GetCurrentMethod()).MakeGenericMethod(typeof(TSource)), source.Expression, selector, Expression.Constant(cancellationToken, typeof(CancellationToken))), cancellationToken);
#endif
        }

        public static Task<double?> MinAsync<TSource>(this IAsyncQueryable<TSource> source, Expression<Func<TSource, ValueTask<double?>>> selector, CancellationToken cancellationToken = default)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));
            if (selector == null)
                throw new ArgumentNullException(nameof(selector));

#if CRIPPLED_REFLECTION
            return source.Provider.ExecuteAsync<double?>(Expression.Call(InfoOf(() => AsyncQueryable.MinAsync<TSource>(default(IAsyncQueryable<TSource>), default(Expression<Func<TSource, ValueTask<double?>>>), default(CancellationToken))), source.Expression, selector, Expression.Constant(cancellationToken, typeof(CancellationToken))), cancellationToken);
#else
            return source.Provider.ExecuteAsync<double?>(Expression.Call(((MethodInfo)MethodBase.GetCurrentMethod()).MakeGenericMethod(typeof(TSource)), source.Expression, selector, Expression.Constant(cancellationToken, typeof(CancellationToken))), cancellationToken);
#endif
        }

        public static Task<double> MinAsync<TSource>(this IAsyncQueryable<TSource> source, Expression<Func<TSource, ValueTask<double>>> selector, CancellationToken cancellationToken = default)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));
            if (selector == null)
                throw new ArgumentNullException(nameof(selector));

#if CRIPPLED_REFLECTION
            return source.Provider.ExecuteAsync<double>(Expression.Call(InfoOf(() => AsyncQueryable.MinAsync<TSource>(default(IAsyncQueryable<TSource>), default(Expression<Func<TSource, ValueTask<double>>>), default(CancellationToken))), source.Expression, selector, Expression.Constant(cancellationToken, typeof(CancellationToken))), cancellationToken);
#else
            return source.Provider.ExecuteAsync<double>(Expression.Call(((MethodInfo)MethodBase.GetCurrentMethod()).MakeGenericMethod(typeof(TSource)), source.Expression, selector, Expression.Constant(cancellationToken, typeof(CancellationToken))), cancellationToken);
#endif
        }

        public static Task<float?> MinAsync<TSource>(this IAsyncQueryable<TSource> source, Expression<Func<TSource, ValueTask<float?>>> selector, CancellationToken cancellationToken = default)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));
            if (selector == null)
                throw new ArgumentNullException(nameof(selector));

#if CRIPPLED_REFLECTION
            return source.Provider.ExecuteAsync<float?>(Expression.Call(InfoOf(() => AsyncQueryable.MinAsync<TSource>(default(IAsyncQueryable<TSource>), default(Expression<Func<TSource, ValueTask<float?>>>), default(CancellationToken))), source.Expression, selector, Expression.Constant(cancellationToken, typeof(CancellationToken))), cancellationToken);
#else
            return source.Provider.ExecuteAsync<float?>(Expression.Call(((MethodInfo)MethodBase.GetCurrentMethod()).MakeGenericMethod(typeof(TSource)), source.Expression, selector, Expression.Constant(cancellationToken, typeof(CancellationToken))), cancellationToken);
#endif
        }

        public static Task<float> MinAsync<TSource>(this IAsyncQueryable<TSource> source, Expression<Func<TSource, ValueTask<float>>> selector, CancellationToken cancellationToken = default)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));
            if (selector == null)
                throw new ArgumentNullException(nameof(selector));

#if CRIPPLED_REFLECTION
            return source.Provider.ExecuteAsync<float>(Expression.Call(InfoOf(() => AsyncQueryable.MinAsync<TSource>(default(IAsyncQueryable<TSource>), default(Expression<Func<TSource, ValueTask<float>>>), default(CancellationToken))), source.Expression, selector, Expression.Constant(cancellationToken, typeof(CancellationToken))), cancellationToken);
#else
            return source.Provider.ExecuteAsync<float>(Expression.Call(((MethodInfo)MethodBase.GetCurrentMethod()).MakeGenericMethod(typeof(TSource)), source.Expression, selector, Expression.Constant(cancellationToken, typeof(CancellationToken))), cancellationToken);
#endif
        }

        public static Task<int?> MinAsync<TSource>(this IAsyncQueryable<TSource> source, Expression<Func<TSource, ValueTask<int?>>> selector, CancellationToken cancellationToken = default)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));
            if (selector == null)
                throw new ArgumentNullException(nameof(selector));

#if CRIPPLED_REFLECTION
            return source.Provider.ExecuteAsync<int?>(Expression.Call(InfoOf(() => AsyncQueryable.MinAsync<TSource>(default(IAsyncQueryable<TSource>), default(Expression<Func<TSource, ValueTask<int?>>>), default(CancellationToken))), source.Expression, selector, Expression.Constant(cancellationToken, typeof(CancellationToken))), cancellationToken);
#else
            return source.Provider.ExecuteAsync<int?>(Expression.Call(((MethodInfo)MethodBase.GetCurrentMethod()).MakeGenericMethod(typeof(TSource)), source.Expression, selector, Expression.Constant(cancellationToken, typeof(CancellationToken))), cancellationToken);
#endif
        }

        public static Task<int> MinAsync<TSource>(this IAsyncQueryable<TSource> source, Expression<Func<TSource, ValueTask<int>>> selector, CancellationToken cancellationToken = default)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));
            if (selector == null)
                throw new ArgumentNullException(nameof(selector));

#if CRIPPLED_REFLECTION
            return source.Provider.ExecuteAsync<int>(Expression.Call(InfoOf(() => AsyncQueryable.MinAsync<TSource>(default(IAsyncQueryable<TSource>), default(Expression<Func<TSource, ValueTask<int>>>), default(CancellationToken))), source.Expression, selector, Expression.Constant(cancellationToken, typeof(CancellationToken))), cancellationToken);
#else
            return source.Provider.ExecuteAsync<int>(Expression.Call(((MethodInfo)MethodBase.GetCurrentMethod()).MakeGenericMethod(typeof(TSource)), source.Expression, selector, Expression.Constant(cancellationToken, typeof(CancellationToken))), cancellationToken);
#endif
        }

        public static Task<long?> MinAsync<TSource>(this IAsyncQueryable<TSource> source, Expression<Func<TSource, ValueTask<long?>>> selector, CancellationToken cancellationToken = default)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));
            if (selector == null)
                throw new ArgumentNullException(nameof(selector));

#if CRIPPLED_REFLECTION
            return source.Provider.ExecuteAsync<long?>(Expression.Call(InfoOf(() => AsyncQueryable.MinAsync<TSource>(default(IAsyncQueryable<TSource>), default(Expression<Func<TSource, ValueTask<long?>>>), default(CancellationToken))), source.Expression, selector, Expression.Constant(cancellationToken, typeof(CancellationToken))), cancellationToken);
#else
            return source.Provider.ExecuteAsync<long?>(Expression.Call(((MethodInfo)MethodBase.GetCurrentMethod()).MakeGenericMethod(typeof(TSource)), source.Expression, selector, Expression.Constant(cancellationToken, typeof(CancellationToken))), cancellationToken);
#endif
        }

        public static Task<long> MinAsync<TSource>(this IAsyncQueryable<TSource> source, Expression<Func<TSource, ValueTask<long>>> selector, CancellationToken cancellationToken = default)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));
            if (selector == null)
                throw new ArgumentNullException(nameof(selector));

#if CRIPPLED_REFLECTION
            return source.Provider.ExecuteAsync<long>(Expression.Call(InfoOf(() => AsyncQueryable.MinAsync<TSource>(default(IAsyncQueryable<TSource>), default(Expression<Func<TSource, ValueTask<long>>>), default(CancellationToken))), source.Expression, selector, Expression.Constant(cancellationToken, typeof(CancellationToken))), cancellationToken);
#else
            return source.Provider.ExecuteAsync<long>(Expression.Call(((MethodInfo)MethodBase.GetCurrentMethod()).MakeGenericMethod(typeof(TSource)), source.Expression, selector, Expression.Constant(cancellationToken, typeof(CancellationToken))), cancellationToken);
#endif
        }

        public static Task<TResult> MinAsync<TSource, TResult>(this IAsyncQueryable<TSource> source, Expression<Func<TSource, CancellationToken, ValueTask<TResult>>> selector, CancellationToken cancellationToken = default)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));
            if (selector == null)
                throw new ArgumentNullException(nameof(selector));

#if CRIPPLED_REFLECTION
            return source.Provider.ExecuteAsync<TResult>(Expression.Call(InfoOf(() => AsyncQueryable.MinAsync<TSource, TResult>(default(IAsyncQueryable<TSource>), default(Expression<Func<TSource, CancellationToken, ValueTask<TResult>>>), default(CancellationToken))), source.Expression, selector, Expression.Constant(cancellationToken, typeof(CancellationToken))), cancellationToken);
#else
            return source.Provider.ExecuteAsync<TResult>(Expression.Call(((MethodInfo)MethodBase.GetCurrentMethod()).MakeGenericMethod(typeof(TSource), typeof(TResult)), source.Expression, selector, Expression.Constant(cancellationToken, typeof(CancellationToken))), cancellationToken);
#endif
        }

        public static Task<TResult> MinAsync<TSource, TResult>(this IAsyncQueryable<TSource> source, Expression<Func<TSource, TResult>> selector, CancellationToken cancellationToken = default)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));
            if (selector == null)
                throw new ArgumentNullException(nameof(selector));

#if CRIPPLED_REFLECTION
            return source.Provider.ExecuteAsync<TResult>(Expression.Call(InfoOf(() => AsyncQueryable.MinAsync<TSource, TResult>(default(IAsyncQueryable<TSource>), default(Expression<Func<TSource, TResult>>), default(CancellationToken))), source.Expression, selector, Expression.Constant(cancellationToken, typeof(CancellationToken))), cancellationToken);
#else
            return source.Provider.ExecuteAsync<TResult>(Expression.Call(((MethodInfo)MethodBase.GetCurrentMethod()).MakeGenericMethod(typeof(TSource), typeof(TResult)), source.Expression, selector, Expression.Constant(cancellationToken, typeof(CancellationToken))), cancellationToken);
#endif
        }

        public static Task<TResult> MinAsync<TSource, TResult>(this IAsyncQueryable<TSource> source, Expression<Func<TSource, ValueTask<TResult>>> selector, CancellationToken cancellationToken = default)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));
            if (selector == null)
                throw new ArgumentNullException(nameof(selector));

#if CRIPPLED_REFLECTION
            return source.Provider.ExecuteAsync<TResult>(Expression.Call(InfoOf(() => AsyncQueryable.MinAsync<TSource, TResult>(default(IAsyncQueryable<TSource>), default(Expression<Func<TSource, ValueTask<TResult>>>), default(CancellationToken))), source.Expression, selector, Expression.Constant(cancellationToken, typeof(CancellationToken))), cancellationToken);
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

        public static IOrderedAsyncQueryable<TSource> OrderBy<TSource, TKey>(this IAsyncQueryable<TSource> source, Expression<Func<TSource, CancellationToken, ValueTask<TKey>>> keySelector)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));
            if (keySelector == null)
                throw new ArgumentNullException(nameof(keySelector));

#if CRIPPLED_REFLECTION
            return (IOrderedAsyncQueryable<TSource>)source.Provider.CreateQuery<TSource>(Expression.Call(InfoOf(() => AsyncQueryable.OrderBy<TSource, TKey>(default(IAsyncQueryable<TSource>), default(Expression<Func<TSource, CancellationToken, ValueTask<TKey>>>))), source.Expression, keySelector));
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

        public static IOrderedAsyncQueryable<TSource> OrderBy<TSource, TKey>(this IAsyncQueryable<TSource> source, Expression<Func<TSource, ValueTask<TKey>>> keySelector)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));
            if (keySelector == null)
                throw new ArgumentNullException(nameof(keySelector));

#if CRIPPLED_REFLECTION
            return (IOrderedAsyncQueryable<TSource>)source.Provider.CreateQuery<TSource>(Expression.Call(InfoOf(() => AsyncQueryable.OrderBy<TSource, TKey>(default(IAsyncQueryable<TSource>), default(Expression<Func<TSource, ValueTask<TKey>>>))), source.Expression, keySelector));
#else
            return (IOrderedAsyncQueryable<TSource>)source.Provider.CreateQuery<TSource>(Expression.Call(((MethodInfo)MethodBase.GetCurrentMethod()).MakeGenericMethod(typeof(TSource), typeof(TKey)), source.Expression, keySelector));
#endif
        }

        public static IOrderedAsyncQueryable<TSource> OrderBy<TSource, TKey>(this IAsyncQueryable<TSource> source, Expression<Func<TSource, CancellationToken, ValueTask<TKey>>> keySelector, IComparer<TKey> comparer)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));
            if (keySelector == null)
                throw new ArgumentNullException(nameof(keySelector));

#if CRIPPLED_REFLECTION
            return (IOrderedAsyncQueryable<TSource>)source.Provider.CreateQuery<TSource>(Expression.Call(InfoOf(() => AsyncQueryable.OrderBy<TSource, TKey>(default(IAsyncQueryable<TSource>), default(Expression<Func<TSource, CancellationToken, ValueTask<TKey>>>), default(IComparer<TKey>))), source.Expression, keySelector, Expression.Constant(comparer, typeof(IComparer<TKey>))));
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

#if CRIPPLED_REFLECTION
            return (IOrderedAsyncQueryable<TSource>)source.Provider.CreateQuery<TSource>(Expression.Call(InfoOf(() => AsyncQueryable.OrderBy<TSource, TKey>(default(IAsyncQueryable<TSource>), default(Expression<Func<TSource, TKey>>), default(IComparer<TKey>))), source.Expression, keySelector, Expression.Constant(comparer, typeof(IComparer<TKey>))));
#else
            return (IOrderedAsyncQueryable<TSource>)source.Provider.CreateQuery<TSource>(Expression.Call(((MethodInfo)MethodBase.GetCurrentMethod()).MakeGenericMethod(typeof(TSource), typeof(TKey)), source.Expression, keySelector, Expression.Constant(comparer, typeof(IComparer<TKey>))));
#endif
        }

        public static IOrderedAsyncQueryable<TSource> OrderBy<TSource, TKey>(this IAsyncQueryable<TSource> source, Expression<Func<TSource, ValueTask<TKey>>> keySelector, IComparer<TKey> comparer)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));
            if (keySelector == null)
                throw new ArgumentNullException(nameof(keySelector));

#if CRIPPLED_REFLECTION
            return (IOrderedAsyncQueryable<TSource>)source.Provider.CreateQuery<TSource>(Expression.Call(InfoOf(() => AsyncQueryable.OrderBy<TSource, TKey>(default(IAsyncQueryable<TSource>), default(Expression<Func<TSource, ValueTask<TKey>>>), default(IComparer<TKey>))), source.Expression, keySelector, Expression.Constant(comparer, typeof(IComparer<TKey>))));
#else
            return (IOrderedAsyncQueryable<TSource>)source.Provider.CreateQuery<TSource>(Expression.Call(((MethodInfo)MethodBase.GetCurrentMethod()).MakeGenericMethod(typeof(TSource), typeof(TKey)), source.Expression, keySelector, Expression.Constant(comparer, typeof(IComparer<TKey>))));
#endif
        }

        public static IOrderedAsyncQueryable<TSource> OrderByDescending<TSource, TKey>(this IAsyncQueryable<TSource> source, Expression<Func<TSource, CancellationToken, ValueTask<TKey>>> keySelector)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));
            if (keySelector == null)
                throw new ArgumentNullException(nameof(keySelector));

#if CRIPPLED_REFLECTION
            return (IOrderedAsyncQueryable<TSource>)source.Provider.CreateQuery<TSource>(Expression.Call(InfoOf(() => AsyncQueryable.OrderByDescending<TSource, TKey>(default(IAsyncQueryable<TSource>), default(Expression<Func<TSource, CancellationToken, ValueTask<TKey>>>))), source.Expression, keySelector));
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

        public static IOrderedAsyncQueryable<TSource> OrderByDescending<TSource, TKey>(this IAsyncQueryable<TSource> source, Expression<Func<TSource, ValueTask<TKey>>> keySelector)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));
            if (keySelector == null)
                throw new ArgumentNullException(nameof(keySelector));

#if CRIPPLED_REFLECTION
            return (IOrderedAsyncQueryable<TSource>)source.Provider.CreateQuery<TSource>(Expression.Call(InfoOf(() => AsyncQueryable.OrderByDescending<TSource, TKey>(default(IAsyncQueryable<TSource>), default(Expression<Func<TSource, ValueTask<TKey>>>))), source.Expression, keySelector));
#else
            return (IOrderedAsyncQueryable<TSource>)source.Provider.CreateQuery<TSource>(Expression.Call(((MethodInfo)MethodBase.GetCurrentMethod()).MakeGenericMethod(typeof(TSource), typeof(TKey)), source.Expression, keySelector));
#endif
        }

        public static IOrderedAsyncQueryable<TSource> OrderByDescending<TSource, TKey>(this IAsyncQueryable<TSource> source, Expression<Func<TSource, CancellationToken, ValueTask<TKey>>> keySelector, IComparer<TKey> comparer)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));
            if (keySelector == null)
                throw new ArgumentNullException(nameof(keySelector));

#if CRIPPLED_REFLECTION
            return (IOrderedAsyncQueryable<TSource>)source.Provider.CreateQuery<TSource>(Expression.Call(InfoOf(() => AsyncQueryable.OrderByDescending<TSource, TKey>(default(IAsyncQueryable<TSource>), default(Expression<Func<TSource, CancellationToken, ValueTask<TKey>>>), default(IComparer<TKey>))), source.Expression, keySelector, Expression.Constant(comparer, typeof(IComparer<TKey>))));
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

#if CRIPPLED_REFLECTION
            return (IOrderedAsyncQueryable<TSource>)source.Provider.CreateQuery<TSource>(Expression.Call(InfoOf(() => AsyncQueryable.OrderByDescending<TSource, TKey>(default(IAsyncQueryable<TSource>), default(Expression<Func<TSource, TKey>>), default(IComparer<TKey>))), source.Expression, keySelector, Expression.Constant(comparer, typeof(IComparer<TKey>))));
#else
            return (IOrderedAsyncQueryable<TSource>)source.Provider.CreateQuery<TSource>(Expression.Call(((MethodInfo)MethodBase.GetCurrentMethod()).MakeGenericMethod(typeof(TSource), typeof(TKey)), source.Expression, keySelector, Expression.Constant(comparer, typeof(IComparer<TKey>))));
#endif
        }

        public static IOrderedAsyncQueryable<TSource> OrderByDescending<TSource, TKey>(this IAsyncQueryable<TSource> source, Expression<Func<TSource, ValueTask<TKey>>> keySelector, IComparer<TKey> comparer)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));
            if (keySelector == null)
                throw new ArgumentNullException(nameof(keySelector));

#if CRIPPLED_REFLECTION
            return (IOrderedAsyncQueryable<TSource>)source.Provider.CreateQuery<TSource>(Expression.Call(InfoOf(() => AsyncQueryable.OrderByDescending<TSource, TKey>(default(IAsyncQueryable<TSource>), default(Expression<Func<TSource, ValueTask<TKey>>>), default(IComparer<TKey>))), source.Expression, keySelector, Expression.Constant(comparer, typeof(IComparer<TKey>))));
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

        public static IAsyncQueryable<TResult> Select<TSource, TResult>(this IAsyncQueryable<TSource> source, Expression<Func<TSource, CancellationToken, ValueTask<TResult>>> selector)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));
            if (selector == null)
                throw new ArgumentNullException(nameof(selector));

#if CRIPPLED_REFLECTION
            return source.Provider.CreateQuery<TResult>(Expression.Call(InfoOf(() => AsyncQueryable.Select<TSource, TResult>(default(IAsyncQueryable<TSource>), default(Expression<Func<TSource, CancellationToken, ValueTask<TResult>>>))), source.Expression, selector));
#else
            return source.Provider.CreateQuery<TResult>(Expression.Call(((MethodInfo)MethodBase.GetCurrentMethod()).MakeGenericMethod(typeof(TSource), typeof(TResult)), source.Expression, selector));
#endif
        }

        public static IAsyncQueryable<TResult> Select<TSource, TResult>(this IAsyncQueryable<TSource> source, Expression<Func<TSource, int, CancellationToken, ValueTask<TResult>>> selector)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));
            if (selector == null)
                throw new ArgumentNullException(nameof(selector));

#if CRIPPLED_REFLECTION
            return source.Provider.CreateQuery<TResult>(Expression.Call(InfoOf(() => AsyncQueryable.Select<TSource, TResult>(default(IAsyncQueryable<TSource>), default(Expression<Func<TSource, int, CancellationToken, ValueTask<TResult>>>))), source.Expression, selector));
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

        public static IAsyncQueryable<TResult> Select<TSource, TResult>(this IAsyncQueryable<TSource> source, Expression<Func<TSource, int, ValueTask<TResult>>> selector)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));
            if (selector == null)
                throw new ArgumentNullException(nameof(selector));

#if CRIPPLED_REFLECTION
            return source.Provider.CreateQuery<TResult>(Expression.Call(InfoOf(() => AsyncQueryable.Select<TSource, TResult>(default(IAsyncQueryable<TSource>), default(Expression<Func<TSource, int, ValueTask<TResult>>>))), source.Expression, selector));
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

        public static IAsyncQueryable<TResult> Select<TSource, TResult>(this IAsyncQueryable<TSource> source, Expression<Func<TSource, ValueTask<TResult>>> selector)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));
            if (selector == null)
                throw new ArgumentNullException(nameof(selector));

#if CRIPPLED_REFLECTION
            return source.Provider.CreateQuery<TResult>(Expression.Call(InfoOf(() => AsyncQueryable.Select<TSource, TResult>(default(IAsyncQueryable<TSource>), default(Expression<Func<TSource, ValueTask<TResult>>>))), source.Expression, selector));
#else
            return source.Provider.CreateQuery<TResult>(Expression.Call(((MethodInfo)MethodBase.GetCurrentMethod()).MakeGenericMethod(typeof(TSource), typeof(TResult)), source.Expression, selector));
#endif
        }

        public static IAsyncQueryable<TResult> SelectMany<TSource, TResult>(this IAsyncQueryable<TSource> source, Expression<Func<TSource, CancellationToken, ValueTask<IAsyncEnumerable<TResult>>>> selector)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));
            if (selector == null)
                throw new ArgumentNullException(nameof(selector));

#if CRIPPLED_REFLECTION
            return source.Provider.CreateQuery<TResult>(Expression.Call(InfoOf(() => AsyncQueryable.SelectMany<TSource, TResult>(default(IAsyncQueryable<TSource>), default(Expression<Func<TSource, CancellationToken, ValueTask<IAsyncEnumerable<TResult>>>>))), source.Expression, selector));
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

        public static IAsyncQueryable<TResult> SelectMany<TSource, TResult>(this IAsyncQueryable<TSource> source, Expression<Func<TSource, int, CancellationToken, ValueTask<IAsyncEnumerable<TResult>>>> selector)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));
            if (selector == null)
                throw new ArgumentNullException(nameof(selector));

#if CRIPPLED_REFLECTION
            return source.Provider.CreateQuery<TResult>(Expression.Call(InfoOf(() => AsyncQueryable.SelectMany<TSource, TResult>(default(IAsyncQueryable<TSource>), default(Expression<Func<TSource, int, CancellationToken, ValueTask<IAsyncEnumerable<TResult>>>>))), source.Expression, selector));
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

        public static IAsyncQueryable<TResult> SelectMany<TSource, TResult>(this IAsyncQueryable<TSource> source, Expression<Func<TSource, int, ValueTask<IAsyncEnumerable<TResult>>>> selector)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));
            if (selector == null)
                throw new ArgumentNullException(nameof(selector));

#if CRIPPLED_REFLECTION
            return source.Provider.CreateQuery<TResult>(Expression.Call(InfoOf(() => AsyncQueryable.SelectMany<TSource, TResult>(default(IAsyncQueryable<TSource>), default(Expression<Func<TSource, int, ValueTask<IAsyncEnumerable<TResult>>>>))), source.Expression, selector));
#else
            return source.Provider.CreateQuery<TResult>(Expression.Call(((MethodInfo)MethodBase.GetCurrentMethod()).MakeGenericMethod(typeof(TSource), typeof(TResult)), source.Expression, selector));
#endif
        }

        public static IAsyncQueryable<TResult> SelectMany<TSource, TResult>(this IAsyncQueryable<TSource> source, Expression<Func<TSource, ValueTask<IAsyncEnumerable<TResult>>>> selector)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));
            if (selector == null)
                throw new ArgumentNullException(nameof(selector));

#if CRIPPLED_REFLECTION
            return source.Provider.CreateQuery<TResult>(Expression.Call(InfoOf(() => AsyncQueryable.SelectMany<TSource, TResult>(default(IAsyncQueryable<TSource>), default(Expression<Func<TSource, ValueTask<IAsyncEnumerable<TResult>>>>))), source.Expression, selector));
#else
            return source.Provider.CreateQuery<TResult>(Expression.Call(((MethodInfo)MethodBase.GetCurrentMethod()).MakeGenericMethod(typeof(TSource), typeof(TResult)), source.Expression, selector));
#endif
        }

        public static IAsyncQueryable<TResult> SelectMany<TSource, TCollection, TResult>(this IAsyncQueryable<TSource> source, Expression<Func<TSource, CancellationToken, ValueTask<IAsyncEnumerable<TCollection>>>> collectionSelector, Expression<Func<TSource, TCollection, CancellationToken, ValueTask<TResult>>> resultSelector)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));
            if (collectionSelector == null)
                throw new ArgumentNullException(nameof(collectionSelector));
            if (resultSelector == null)
                throw new ArgumentNullException(nameof(resultSelector));

#if CRIPPLED_REFLECTION
            return source.Provider.CreateQuery<TResult>(Expression.Call(InfoOf(() => AsyncQueryable.SelectMany<TSource, TCollection, TResult>(default(IAsyncQueryable<TSource>), default(Expression<Func<TSource, CancellationToken, ValueTask<IAsyncEnumerable<TCollection>>>>), default(Expression<Func<TSource, TCollection, CancellationToken, ValueTask<TResult>>>))), source.Expression, collectionSelector, resultSelector));
#else
            return source.Provider.CreateQuery<TResult>(Expression.Call(((MethodInfo)MethodBase.GetCurrentMethod()).MakeGenericMethod(typeof(TSource), typeof(TCollection), typeof(TResult)), source.Expression, collectionSelector, resultSelector));
#endif
        }

        public static IAsyncQueryable<TResult> SelectMany<TSource, TCollection, TResult>(this IAsyncQueryable<TSource> source, Expression<Func<TSource, IAsyncEnumerable<TCollection>>> collectionSelector, Expression<Func<TSource, TCollection, TResult>> resultSelector)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));
            if (collectionSelector == null)
                throw new ArgumentNullException(nameof(collectionSelector));
            if (resultSelector == null)
                throw new ArgumentNullException(nameof(resultSelector));

#if CRIPPLED_REFLECTION
            return source.Provider.CreateQuery<TResult>(Expression.Call(InfoOf(() => AsyncQueryable.SelectMany<TSource, TCollection, TResult>(default(IAsyncQueryable<TSource>), default(Expression<Func<TSource, IAsyncEnumerable<TCollection>>>), default(Expression<Func<TSource, TCollection, TResult>>))), source.Expression, collectionSelector, resultSelector));
#else
            return source.Provider.CreateQuery<TResult>(Expression.Call(((MethodInfo)MethodBase.GetCurrentMethod()).MakeGenericMethod(typeof(TSource), typeof(TCollection), typeof(TResult)), source.Expression, collectionSelector, resultSelector));
#endif
        }

        public static IAsyncQueryable<TResult> SelectMany<TSource, TCollection, TResult>(this IAsyncQueryable<TSource> source, Expression<Func<TSource, int, CancellationToken, ValueTask<IAsyncEnumerable<TCollection>>>> collectionSelector, Expression<Func<TSource, TCollection, CancellationToken, ValueTask<TResult>>> resultSelector)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));
            if (collectionSelector == null)
                throw new ArgumentNullException(nameof(collectionSelector));
            if (resultSelector == null)
                throw new ArgumentNullException(nameof(resultSelector));

#if CRIPPLED_REFLECTION
            return source.Provider.CreateQuery<TResult>(Expression.Call(InfoOf(() => AsyncQueryable.SelectMany<TSource, TCollection, TResult>(default(IAsyncQueryable<TSource>), default(Expression<Func<TSource, int, CancellationToken, ValueTask<IAsyncEnumerable<TCollection>>>>), default(Expression<Func<TSource, TCollection, CancellationToken, ValueTask<TResult>>>))), source.Expression, collectionSelector, resultSelector));
#else
            return source.Provider.CreateQuery<TResult>(Expression.Call(((MethodInfo)MethodBase.GetCurrentMethod()).MakeGenericMethod(typeof(TSource), typeof(TCollection), typeof(TResult)), source.Expression, collectionSelector, resultSelector));
#endif
        }

        public static IAsyncQueryable<TResult> SelectMany<TSource, TCollection, TResult>(this IAsyncQueryable<TSource> source, Expression<Func<TSource, int, IAsyncEnumerable<TCollection>>> collectionSelector, Expression<Func<TSource, TCollection, TResult>> resultSelector)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));
            if (collectionSelector == null)
                throw new ArgumentNullException(nameof(collectionSelector));
            if (resultSelector == null)
                throw new ArgumentNullException(nameof(resultSelector));

#if CRIPPLED_REFLECTION
            return source.Provider.CreateQuery<TResult>(Expression.Call(InfoOf(() => AsyncQueryable.SelectMany<TSource, TCollection, TResult>(default(IAsyncQueryable<TSource>), default(Expression<Func<TSource, int, IAsyncEnumerable<TCollection>>>), default(Expression<Func<TSource, TCollection, TResult>>))), source.Expression, collectionSelector, resultSelector));
#else
            return source.Provider.CreateQuery<TResult>(Expression.Call(((MethodInfo)MethodBase.GetCurrentMethod()).MakeGenericMethod(typeof(TSource), typeof(TCollection), typeof(TResult)), source.Expression, collectionSelector, resultSelector));
#endif
        }

        public static IAsyncQueryable<TResult> SelectMany<TSource, TCollection, TResult>(this IAsyncQueryable<TSource> source, Expression<Func<TSource, int, ValueTask<IAsyncEnumerable<TCollection>>>> collectionSelector, Expression<Func<TSource, TCollection, ValueTask<TResult>>> resultSelector)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));
            if (collectionSelector == null)
                throw new ArgumentNullException(nameof(collectionSelector));
            if (resultSelector == null)
                throw new ArgumentNullException(nameof(resultSelector));

#if CRIPPLED_REFLECTION
            return source.Provider.CreateQuery<TResult>(Expression.Call(InfoOf(() => AsyncQueryable.SelectMany<TSource, TCollection, TResult>(default(IAsyncQueryable<TSource>), default(Expression<Func<TSource, int, ValueTask<IAsyncEnumerable<TCollection>>>>), default(Expression<Func<TSource, TCollection, ValueTask<TResult>>>))), source.Expression, collectionSelector, resultSelector));
#else
            return source.Provider.CreateQuery<TResult>(Expression.Call(((MethodInfo)MethodBase.GetCurrentMethod()).MakeGenericMethod(typeof(TSource), typeof(TCollection), typeof(TResult)), source.Expression, collectionSelector, resultSelector));
#endif
        }

        public static IAsyncQueryable<TResult> SelectMany<TSource, TCollection, TResult>(this IAsyncQueryable<TSource> source, Expression<Func<TSource, ValueTask<IAsyncEnumerable<TCollection>>>> collectionSelector, Expression<Func<TSource, TCollection, ValueTask<TResult>>> resultSelector)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));
            if (collectionSelector == null)
                throw new ArgumentNullException(nameof(collectionSelector));
            if (resultSelector == null)
                throw new ArgumentNullException(nameof(resultSelector));

#if CRIPPLED_REFLECTION
            return source.Provider.CreateQuery<TResult>(Expression.Call(InfoOf(() => AsyncQueryable.SelectMany<TSource, TCollection, TResult>(default(IAsyncQueryable<TSource>), default(Expression<Func<TSource, ValueTask<IAsyncEnumerable<TCollection>>>>), default(Expression<Func<TSource, TCollection, ValueTask<TResult>>>))), source.Expression, collectionSelector, resultSelector));
#else
            return source.Provider.CreateQuery<TResult>(Expression.Call(((MethodInfo)MethodBase.GetCurrentMethod()).MakeGenericMethod(typeof(TSource), typeof(TCollection), typeof(TResult)), source.Expression, collectionSelector, resultSelector));
#endif
        }

        public static Task<bool> SequenceEqualAsync<TSource>(this IAsyncQueryable<TSource> first, IAsyncEnumerable<TSource> second, CancellationToken cancellationToken = default)
        {
            if (first == null)
                throw new ArgumentNullException(nameof(first));
            if (second == null)
                throw new ArgumentNullException(nameof(second));

#if CRIPPLED_REFLECTION
            return first.Provider.ExecuteAsync<bool>(Expression.Call(InfoOf(() => AsyncQueryable.SequenceEqualAsync<TSource>(default(IAsyncQueryable<TSource>), default(IAsyncEnumerable<TSource>), default(CancellationToken))), first.Expression, GetSourceExpression(second), Expression.Constant(cancellationToken, typeof(CancellationToken))), cancellationToken);
#else
            return first.Provider.ExecuteAsync<bool>(Expression.Call(((MethodInfo)MethodBase.GetCurrentMethod()).MakeGenericMethod(typeof(TSource)), first.Expression, GetSourceExpression(second), Expression.Constant(cancellationToken, typeof(CancellationToken))), cancellationToken);
#endif
        }

        public static Task<bool> SequenceEqualAsync<TSource>(this IAsyncQueryable<TSource> first, IAsyncEnumerable<TSource> second, IEqualityComparer<TSource> comparer, CancellationToken cancellationToken = default)
        {
            if (first == null)
                throw new ArgumentNullException(nameof(first));
            if (second == null)
                throw new ArgumentNullException(nameof(second));

#if CRIPPLED_REFLECTION
            return first.Provider.ExecuteAsync<bool>(Expression.Call(InfoOf(() => AsyncQueryable.SequenceEqualAsync<TSource>(default(IAsyncQueryable<TSource>), default(IAsyncEnumerable<TSource>), default(IEqualityComparer<TSource>), default(CancellationToken))), first.Expression, GetSourceExpression(second), Expression.Constant(comparer, typeof(IEqualityComparer<TSource>)), Expression.Constant(cancellationToken, typeof(CancellationToken))), cancellationToken);
#else
            return first.Provider.ExecuteAsync<bool>(Expression.Call(((MethodInfo)MethodBase.GetCurrentMethod()).MakeGenericMethod(typeof(TSource)), first.Expression, GetSourceExpression(second), Expression.Constant(comparer, typeof(IEqualityComparer<TSource>)), Expression.Constant(cancellationToken, typeof(CancellationToken))), cancellationToken);
#endif
        }

        public static Task<TSource> SingleAsync<TSource>(this IAsyncQueryable<TSource> source, CancellationToken cancellationToken = default)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));

#if CRIPPLED_REFLECTION
            return source.Provider.ExecuteAsync<TSource>(Expression.Call(InfoOf(() => AsyncQueryable.SingleAsync<TSource>(default(IAsyncQueryable<TSource>), default(CancellationToken))), source.Expression, Expression.Constant(cancellationToken, typeof(CancellationToken))), cancellationToken);
#else
            return source.Provider.ExecuteAsync<TSource>(Expression.Call(((MethodInfo)MethodBase.GetCurrentMethod()).MakeGenericMethod(typeof(TSource)), source.Expression, Expression.Constant(cancellationToken, typeof(CancellationToken))), cancellationToken);
#endif
        }

        public static Task<TSource> SingleAsync<TSource>(this IAsyncQueryable<TSource> source, Expression<Func<TSource, bool>> predicate, CancellationToken cancellationToken = default)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));
            if (predicate == null)
                throw new ArgumentNullException(nameof(predicate));

#if CRIPPLED_REFLECTION
            return source.Provider.ExecuteAsync<TSource>(Expression.Call(InfoOf(() => AsyncQueryable.SingleAsync<TSource>(default(IAsyncQueryable<TSource>), default(Expression<Func<TSource, bool>>), default(CancellationToken))), source.Expression, predicate, Expression.Constant(cancellationToken, typeof(CancellationToken))), cancellationToken);
#else
            return source.Provider.ExecuteAsync<TSource>(Expression.Call(((MethodInfo)MethodBase.GetCurrentMethod()).MakeGenericMethod(typeof(TSource)), source.Expression, predicate, Expression.Constant(cancellationToken, typeof(CancellationToken))), cancellationToken);
#endif
        }

        public static Task<TSource> SingleAsync<TSource>(this IAsyncQueryable<TSource> source, Expression<Func<TSource, CancellationToken, ValueTask<bool>>> predicate, CancellationToken cancellationToken = default)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));
            if (predicate == null)
                throw new ArgumentNullException(nameof(predicate));

#if CRIPPLED_REFLECTION
            return source.Provider.ExecuteAsync<TSource>(Expression.Call(InfoOf(() => AsyncQueryable.SingleAsync<TSource>(default(IAsyncQueryable<TSource>), default(Expression<Func<TSource, CancellationToken, ValueTask<bool>>>), default(CancellationToken))), source.Expression, predicate, Expression.Constant(cancellationToken, typeof(CancellationToken))), cancellationToken);
#else
            return source.Provider.ExecuteAsync<TSource>(Expression.Call(((MethodInfo)MethodBase.GetCurrentMethod()).MakeGenericMethod(typeof(TSource)), source.Expression, predicate, Expression.Constant(cancellationToken, typeof(CancellationToken))), cancellationToken);
#endif
        }

        public static Task<TSource> SingleAsync<TSource>(this IAsyncQueryable<TSource> source, Expression<Func<TSource, ValueTask<bool>>> predicate, CancellationToken cancellationToken = default)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));
            if (predicate == null)
                throw new ArgumentNullException(nameof(predicate));

#if CRIPPLED_REFLECTION
            return source.Provider.ExecuteAsync<TSource>(Expression.Call(InfoOf(() => AsyncQueryable.SingleAsync<TSource>(default(IAsyncQueryable<TSource>), default(Expression<Func<TSource, ValueTask<bool>>>), default(CancellationToken))), source.Expression, predicate, Expression.Constant(cancellationToken, typeof(CancellationToken))), cancellationToken);
#else
            return source.Provider.ExecuteAsync<TSource>(Expression.Call(((MethodInfo)MethodBase.GetCurrentMethod()).MakeGenericMethod(typeof(TSource)), source.Expression, predicate, Expression.Constant(cancellationToken, typeof(CancellationToken))), cancellationToken);
#endif
        }

        public static Task<TSource> SingleOrDefaultAsync<TSource>(this IAsyncQueryable<TSource> source, CancellationToken cancellationToken = default)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));

#if CRIPPLED_REFLECTION
            return source.Provider.ExecuteAsync<TSource>(Expression.Call(InfoOf(() => AsyncQueryable.SingleOrDefaultAsync<TSource>(default(IAsyncQueryable<TSource>), default(CancellationToken))), source.Expression, Expression.Constant(cancellationToken, typeof(CancellationToken))), cancellationToken);
#else
            return source.Provider.ExecuteAsync<TSource>(Expression.Call(((MethodInfo)MethodBase.GetCurrentMethod()).MakeGenericMethod(typeof(TSource)), source.Expression, Expression.Constant(cancellationToken, typeof(CancellationToken))), cancellationToken);
#endif
        }

        public static Task<TSource> SingleOrDefaultAsync<TSource>(this IAsyncQueryable<TSource> source, Expression<Func<TSource, bool>> predicate, CancellationToken cancellationToken = default)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));
            if (predicate == null)
                throw new ArgumentNullException(nameof(predicate));

#if CRIPPLED_REFLECTION
            return source.Provider.ExecuteAsync<TSource>(Expression.Call(InfoOf(() => AsyncQueryable.SingleOrDefaultAsync<TSource>(default(IAsyncQueryable<TSource>), default(Expression<Func<TSource, bool>>), default(CancellationToken))), source.Expression, predicate, Expression.Constant(cancellationToken, typeof(CancellationToken))), cancellationToken);
#else
            return source.Provider.ExecuteAsync<TSource>(Expression.Call(((MethodInfo)MethodBase.GetCurrentMethod()).MakeGenericMethod(typeof(TSource)), source.Expression, predicate, Expression.Constant(cancellationToken, typeof(CancellationToken))), cancellationToken);
#endif
        }

        public static Task<TSource> SingleOrDefaultAsync<TSource>(this IAsyncQueryable<TSource> source, Expression<Func<TSource, CancellationToken, ValueTask<bool>>> predicate, CancellationToken cancellationToken = default)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));
            if (predicate == null)
                throw new ArgumentNullException(nameof(predicate));

#if CRIPPLED_REFLECTION
            return source.Provider.ExecuteAsync<TSource>(Expression.Call(InfoOf(() => AsyncQueryable.SingleOrDefaultAsync<TSource>(default(IAsyncQueryable<TSource>), default(Expression<Func<TSource, CancellationToken, ValueTask<bool>>>), default(CancellationToken))), source.Expression, predicate, Expression.Constant(cancellationToken, typeof(CancellationToken))), cancellationToken);
#else
            return source.Provider.ExecuteAsync<TSource>(Expression.Call(((MethodInfo)MethodBase.GetCurrentMethod()).MakeGenericMethod(typeof(TSource)), source.Expression, predicate, Expression.Constant(cancellationToken, typeof(CancellationToken))), cancellationToken);
#endif
        }

        public static Task<TSource> SingleOrDefaultAsync<TSource>(this IAsyncQueryable<TSource> source, Expression<Func<TSource, ValueTask<bool>>> predicate, CancellationToken cancellationToken = default)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));
            if (predicate == null)
                throw new ArgumentNullException(nameof(predicate));

#if CRIPPLED_REFLECTION
            return source.Provider.ExecuteAsync<TSource>(Expression.Call(InfoOf(() => AsyncQueryable.SingleOrDefaultAsync<TSource>(default(IAsyncQueryable<TSource>), default(Expression<Func<TSource, ValueTask<bool>>>), default(CancellationToken))), source.Expression, predicate, Expression.Constant(cancellationToken, typeof(CancellationToken))), cancellationToken);
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

        public static IAsyncQueryable<TSource> SkipWhile<TSource>(this IAsyncQueryable<TSource> source, Expression<Func<TSource, CancellationToken, ValueTask<bool>>> predicate)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));
            if (predicate == null)
                throw new ArgumentNullException(nameof(predicate));

#if CRIPPLED_REFLECTION
            return source.Provider.CreateQuery<TSource>(Expression.Call(InfoOf(() => AsyncQueryable.SkipWhile<TSource>(default(IAsyncQueryable<TSource>), default(Expression<Func<TSource, CancellationToken, ValueTask<bool>>>))), source.Expression, predicate));
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

        public static IAsyncQueryable<TSource> SkipWhile<TSource>(this IAsyncQueryable<TSource> source, Expression<Func<TSource, int, CancellationToken, ValueTask<bool>>> predicate)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));
            if (predicate == null)
                throw new ArgumentNullException(nameof(predicate));

#if CRIPPLED_REFLECTION
            return source.Provider.CreateQuery<TSource>(Expression.Call(InfoOf(() => AsyncQueryable.SkipWhile<TSource>(default(IAsyncQueryable<TSource>), default(Expression<Func<TSource, int, CancellationToken, ValueTask<bool>>>))), source.Expression, predicate));
#else
            return source.Provider.CreateQuery<TSource>(Expression.Call(((MethodInfo)MethodBase.GetCurrentMethod()).MakeGenericMethod(typeof(TSource)), source.Expression, predicate));
#endif
        }

        public static IAsyncQueryable<TSource> SkipWhile<TSource>(this IAsyncQueryable<TSource> source, Expression<Func<TSource, int, ValueTask<bool>>> predicate)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));
            if (predicate == null)
                throw new ArgumentNullException(nameof(predicate));

#if CRIPPLED_REFLECTION
            return source.Provider.CreateQuery<TSource>(Expression.Call(InfoOf(() => AsyncQueryable.SkipWhile<TSource>(default(IAsyncQueryable<TSource>), default(Expression<Func<TSource, int, ValueTask<bool>>>))), source.Expression, predicate));
#else
            return source.Provider.CreateQuery<TSource>(Expression.Call(((MethodInfo)MethodBase.GetCurrentMethod()).MakeGenericMethod(typeof(TSource)), source.Expression, predicate));
#endif
        }

        public static IAsyncQueryable<TSource> SkipWhile<TSource>(this IAsyncQueryable<TSource> source, Expression<Func<TSource, ValueTask<bool>>> predicate)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));
            if (predicate == null)
                throw new ArgumentNullException(nameof(predicate));

#if CRIPPLED_REFLECTION
            return source.Provider.CreateQuery<TSource>(Expression.Call(InfoOf(() => AsyncQueryable.SkipWhile<TSource>(default(IAsyncQueryable<TSource>), default(Expression<Func<TSource, ValueTask<bool>>>))), source.Expression, predicate));
#else
            return source.Provider.CreateQuery<TSource>(Expression.Call(((MethodInfo)MethodBase.GetCurrentMethod()).MakeGenericMethod(typeof(TSource)), source.Expression, predicate));
#endif
        }

        public static Task<decimal?> SumAsync(this IAsyncQueryable<decimal?> source, CancellationToken cancellationToken = default)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));

#if CRIPPLED_REFLECTION
            return source.Provider.ExecuteAsync<decimal?>(Expression.Call(InfoOf(() => AsyncQueryable.SumAsync(default(IAsyncQueryable<decimal?>), default(CancellationToken))), source.Expression, Expression.Constant(cancellationToken, typeof(CancellationToken))), cancellationToken);
#else
            return source.Provider.ExecuteAsync<decimal?>(Expression.Call((MethodInfo)MethodBase.GetCurrentMethod(), source.Expression, Expression.Constant(cancellationToken, typeof(CancellationToken))), cancellationToken);
#endif
        }

        public static Task<decimal> SumAsync(this IAsyncQueryable<decimal> source, CancellationToken cancellationToken = default)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));

#if CRIPPLED_REFLECTION
            return source.Provider.ExecuteAsync<decimal>(Expression.Call(InfoOf(() => AsyncQueryable.SumAsync(default(IAsyncQueryable<decimal>), default(CancellationToken))), source.Expression, Expression.Constant(cancellationToken, typeof(CancellationToken))), cancellationToken);
#else
            return source.Provider.ExecuteAsync<decimal>(Expression.Call((MethodInfo)MethodBase.GetCurrentMethod(), source.Expression, Expression.Constant(cancellationToken, typeof(CancellationToken))), cancellationToken);
#endif
        }

        public static Task<double?> SumAsync(this IAsyncQueryable<double?> source, CancellationToken cancellationToken = default)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));

#if CRIPPLED_REFLECTION
            return source.Provider.ExecuteAsync<double?>(Expression.Call(InfoOf(() => AsyncQueryable.SumAsync(default(IAsyncQueryable<double?>), default(CancellationToken))), source.Expression, Expression.Constant(cancellationToken, typeof(CancellationToken))), cancellationToken);
#else
            return source.Provider.ExecuteAsync<double?>(Expression.Call((MethodInfo)MethodBase.GetCurrentMethod(), source.Expression, Expression.Constant(cancellationToken, typeof(CancellationToken))), cancellationToken);
#endif
        }

        public static Task<double> SumAsync(this IAsyncQueryable<double> source, CancellationToken cancellationToken = default)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));

#if CRIPPLED_REFLECTION
            return source.Provider.ExecuteAsync<double>(Expression.Call(InfoOf(() => AsyncQueryable.SumAsync(default(IAsyncQueryable<double>), default(CancellationToken))), source.Expression, Expression.Constant(cancellationToken, typeof(CancellationToken))), cancellationToken);
#else
            return source.Provider.ExecuteAsync<double>(Expression.Call((MethodInfo)MethodBase.GetCurrentMethod(), source.Expression, Expression.Constant(cancellationToken, typeof(CancellationToken))), cancellationToken);
#endif
        }

        public static Task<float?> SumAsync(this IAsyncQueryable<float?> source, CancellationToken cancellationToken = default)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));

#if CRIPPLED_REFLECTION
            return source.Provider.ExecuteAsync<float?>(Expression.Call(InfoOf(() => AsyncQueryable.SumAsync(default(IAsyncQueryable<float?>), default(CancellationToken))), source.Expression, Expression.Constant(cancellationToken, typeof(CancellationToken))), cancellationToken);
#else
            return source.Provider.ExecuteAsync<float?>(Expression.Call((MethodInfo)MethodBase.GetCurrentMethod(), source.Expression, Expression.Constant(cancellationToken, typeof(CancellationToken))), cancellationToken);
#endif
        }

        public static Task<float> SumAsync(this IAsyncQueryable<float> source, CancellationToken cancellationToken = default)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));

#if CRIPPLED_REFLECTION
            return source.Provider.ExecuteAsync<float>(Expression.Call(InfoOf(() => AsyncQueryable.SumAsync(default(IAsyncQueryable<float>), default(CancellationToken))), source.Expression, Expression.Constant(cancellationToken, typeof(CancellationToken))), cancellationToken);
#else
            return source.Provider.ExecuteAsync<float>(Expression.Call((MethodInfo)MethodBase.GetCurrentMethod(), source.Expression, Expression.Constant(cancellationToken, typeof(CancellationToken))), cancellationToken);
#endif
        }

        public static Task<int?> SumAsync(this IAsyncQueryable<int?> source, CancellationToken cancellationToken = default)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));

#if CRIPPLED_REFLECTION
            return source.Provider.ExecuteAsync<int?>(Expression.Call(InfoOf(() => AsyncQueryable.SumAsync(default(IAsyncQueryable<int?>), default(CancellationToken))), source.Expression, Expression.Constant(cancellationToken, typeof(CancellationToken))), cancellationToken);
#else
            return source.Provider.ExecuteAsync<int?>(Expression.Call((MethodInfo)MethodBase.GetCurrentMethod(), source.Expression, Expression.Constant(cancellationToken, typeof(CancellationToken))), cancellationToken);
#endif
        }

        public static Task<int> SumAsync(this IAsyncQueryable<int> source, CancellationToken cancellationToken = default)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));

#if CRIPPLED_REFLECTION
            return source.Provider.ExecuteAsync<int>(Expression.Call(InfoOf(() => AsyncQueryable.SumAsync(default(IAsyncQueryable<int>), default(CancellationToken))), source.Expression, Expression.Constant(cancellationToken, typeof(CancellationToken))), cancellationToken);
#else
            return source.Provider.ExecuteAsync<int>(Expression.Call((MethodInfo)MethodBase.GetCurrentMethod(), source.Expression, Expression.Constant(cancellationToken, typeof(CancellationToken))), cancellationToken);
#endif
        }

        public static Task<long?> SumAsync(this IAsyncQueryable<long?> source, CancellationToken cancellationToken = default)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));

#if CRIPPLED_REFLECTION
            return source.Provider.ExecuteAsync<long?>(Expression.Call(InfoOf(() => AsyncQueryable.SumAsync(default(IAsyncQueryable<long?>), default(CancellationToken))), source.Expression, Expression.Constant(cancellationToken, typeof(CancellationToken))), cancellationToken);
#else
            return source.Provider.ExecuteAsync<long?>(Expression.Call((MethodInfo)MethodBase.GetCurrentMethod(), source.Expression, Expression.Constant(cancellationToken, typeof(CancellationToken))), cancellationToken);
#endif
        }

        public static Task<long> SumAsync(this IAsyncQueryable<long> source, CancellationToken cancellationToken = default)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));

#if CRIPPLED_REFLECTION
            return source.Provider.ExecuteAsync<long>(Expression.Call(InfoOf(() => AsyncQueryable.SumAsync(default(IAsyncQueryable<long>), default(CancellationToken))), source.Expression, Expression.Constant(cancellationToken, typeof(CancellationToken))), cancellationToken);
#else
            return source.Provider.ExecuteAsync<long>(Expression.Call((MethodInfo)MethodBase.GetCurrentMethod(), source.Expression, Expression.Constant(cancellationToken, typeof(CancellationToken))), cancellationToken);
#endif
        }

        public static Task<decimal?> SumAsync<TSource>(this IAsyncQueryable<TSource> source, Expression<Func<TSource, CancellationToken, ValueTask<decimal?>>> selector, CancellationToken cancellationToken = default)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));
            if (selector == null)
                throw new ArgumentNullException(nameof(selector));

#if CRIPPLED_REFLECTION
            return source.Provider.ExecuteAsync<decimal?>(Expression.Call(InfoOf(() => AsyncQueryable.SumAsync<TSource>(default(IAsyncQueryable<TSource>), default(Expression<Func<TSource, CancellationToken, ValueTask<decimal?>>>), default(CancellationToken))), source.Expression, selector, Expression.Constant(cancellationToken, typeof(CancellationToken))), cancellationToken);
#else
            return source.Provider.ExecuteAsync<decimal?>(Expression.Call(((MethodInfo)MethodBase.GetCurrentMethod()).MakeGenericMethod(typeof(TSource)), source.Expression, selector, Expression.Constant(cancellationToken, typeof(CancellationToken))), cancellationToken);
#endif
        }

        public static Task<decimal> SumAsync<TSource>(this IAsyncQueryable<TSource> source, Expression<Func<TSource, CancellationToken, ValueTask<decimal>>> selector, CancellationToken cancellationToken = default)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));
            if (selector == null)
                throw new ArgumentNullException(nameof(selector));

#if CRIPPLED_REFLECTION
            return source.Provider.ExecuteAsync<decimal>(Expression.Call(InfoOf(() => AsyncQueryable.SumAsync<TSource>(default(IAsyncQueryable<TSource>), default(Expression<Func<TSource, CancellationToken, ValueTask<decimal>>>), default(CancellationToken))), source.Expression, selector, Expression.Constant(cancellationToken, typeof(CancellationToken))), cancellationToken);
#else
            return source.Provider.ExecuteAsync<decimal>(Expression.Call(((MethodInfo)MethodBase.GetCurrentMethod()).MakeGenericMethod(typeof(TSource)), source.Expression, selector, Expression.Constant(cancellationToken, typeof(CancellationToken))), cancellationToken);
#endif
        }

        public static Task<double?> SumAsync<TSource>(this IAsyncQueryable<TSource> source, Expression<Func<TSource, CancellationToken, ValueTask<double?>>> selector, CancellationToken cancellationToken = default)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));
            if (selector == null)
                throw new ArgumentNullException(nameof(selector));

#if CRIPPLED_REFLECTION
            return source.Provider.ExecuteAsync<double?>(Expression.Call(InfoOf(() => AsyncQueryable.SumAsync<TSource>(default(IAsyncQueryable<TSource>), default(Expression<Func<TSource, CancellationToken, ValueTask<double?>>>), default(CancellationToken))), source.Expression, selector, Expression.Constant(cancellationToken, typeof(CancellationToken))), cancellationToken);
#else
            return source.Provider.ExecuteAsync<double?>(Expression.Call(((MethodInfo)MethodBase.GetCurrentMethod()).MakeGenericMethod(typeof(TSource)), source.Expression, selector, Expression.Constant(cancellationToken, typeof(CancellationToken))), cancellationToken);
#endif
        }

        public static Task<double> SumAsync<TSource>(this IAsyncQueryable<TSource> source, Expression<Func<TSource, CancellationToken, ValueTask<double>>> selector, CancellationToken cancellationToken = default)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));
            if (selector == null)
                throw new ArgumentNullException(nameof(selector));

#if CRIPPLED_REFLECTION
            return source.Provider.ExecuteAsync<double>(Expression.Call(InfoOf(() => AsyncQueryable.SumAsync<TSource>(default(IAsyncQueryable<TSource>), default(Expression<Func<TSource, CancellationToken, ValueTask<double>>>), default(CancellationToken))), source.Expression, selector, Expression.Constant(cancellationToken, typeof(CancellationToken))), cancellationToken);
#else
            return source.Provider.ExecuteAsync<double>(Expression.Call(((MethodInfo)MethodBase.GetCurrentMethod()).MakeGenericMethod(typeof(TSource)), source.Expression, selector, Expression.Constant(cancellationToken, typeof(CancellationToken))), cancellationToken);
#endif
        }

        public static Task<float?> SumAsync<TSource>(this IAsyncQueryable<TSource> source, Expression<Func<TSource, CancellationToken, ValueTask<float?>>> selector, CancellationToken cancellationToken = default)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));
            if (selector == null)
                throw new ArgumentNullException(nameof(selector));

#if CRIPPLED_REFLECTION
            return source.Provider.ExecuteAsync<float?>(Expression.Call(InfoOf(() => AsyncQueryable.SumAsync<TSource>(default(IAsyncQueryable<TSource>), default(Expression<Func<TSource, CancellationToken, ValueTask<float?>>>), default(CancellationToken))), source.Expression, selector, Expression.Constant(cancellationToken, typeof(CancellationToken))), cancellationToken);
#else
            return source.Provider.ExecuteAsync<float?>(Expression.Call(((MethodInfo)MethodBase.GetCurrentMethod()).MakeGenericMethod(typeof(TSource)), source.Expression, selector, Expression.Constant(cancellationToken, typeof(CancellationToken))), cancellationToken);
#endif
        }

        public static Task<float> SumAsync<TSource>(this IAsyncQueryable<TSource> source, Expression<Func<TSource, CancellationToken, ValueTask<float>>> selector, CancellationToken cancellationToken = default)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));
            if (selector == null)
                throw new ArgumentNullException(nameof(selector));

#if CRIPPLED_REFLECTION
            return source.Provider.ExecuteAsync<float>(Expression.Call(InfoOf(() => AsyncQueryable.SumAsync<TSource>(default(IAsyncQueryable<TSource>), default(Expression<Func<TSource, CancellationToken, ValueTask<float>>>), default(CancellationToken))), source.Expression, selector, Expression.Constant(cancellationToken, typeof(CancellationToken))), cancellationToken);
#else
            return source.Provider.ExecuteAsync<float>(Expression.Call(((MethodInfo)MethodBase.GetCurrentMethod()).MakeGenericMethod(typeof(TSource)), source.Expression, selector, Expression.Constant(cancellationToken, typeof(CancellationToken))), cancellationToken);
#endif
        }

        public static Task<int?> SumAsync<TSource>(this IAsyncQueryable<TSource> source, Expression<Func<TSource, CancellationToken, ValueTask<int?>>> selector, CancellationToken cancellationToken = default)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));
            if (selector == null)
                throw new ArgumentNullException(nameof(selector));

#if CRIPPLED_REFLECTION
            return source.Provider.ExecuteAsync<int?>(Expression.Call(InfoOf(() => AsyncQueryable.SumAsync<TSource>(default(IAsyncQueryable<TSource>), default(Expression<Func<TSource, CancellationToken, ValueTask<int?>>>), default(CancellationToken))), source.Expression, selector, Expression.Constant(cancellationToken, typeof(CancellationToken))), cancellationToken);
#else
            return source.Provider.ExecuteAsync<int?>(Expression.Call(((MethodInfo)MethodBase.GetCurrentMethod()).MakeGenericMethod(typeof(TSource)), source.Expression, selector, Expression.Constant(cancellationToken, typeof(CancellationToken))), cancellationToken);
#endif
        }

        public static Task<int> SumAsync<TSource>(this IAsyncQueryable<TSource> source, Expression<Func<TSource, CancellationToken, ValueTask<int>>> selector, CancellationToken cancellationToken = default)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));
            if (selector == null)
                throw new ArgumentNullException(nameof(selector));

#if CRIPPLED_REFLECTION
            return source.Provider.ExecuteAsync<int>(Expression.Call(InfoOf(() => AsyncQueryable.SumAsync<TSource>(default(IAsyncQueryable<TSource>), default(Expression<Func<TSource, CancellationToken, ValueTask<int>>>), default(CancellationToken))), source.Expression, selector, Expression.Constant(cancellationToken, typeof(CancellationToken))), cancellationToken);
#else
            return source.Provider.ExecuteAsync<int>(Expression.Call(((MethodInfo)MethodBase.GetCurrentMethod()).MakeGenericMethod(typeof(TSource)), source.Expression, selector, Expression.Constant(cancellationToken, typeof(CancellationToken))), cancellationToken);
#endif
        }

        public static Task<long?> SumAsync<TSource>(this IAsyncQueryable<TSource> source, Expression<Func<TSource, CancellationToken, ValueTask<long?>>> selector, CancellationToken cancellationToken = default)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));
            if (selector == null)
                throw new ArgumentNullException(nameof(selector));

#if CRIPPLED_REFLECTION
            return source.Provider.ExecuteAsync<long?>(Expression.Call(InfoOf(() => AsyncQueryable.SumAsync<TSource>(default(IAsyncQueryable<TSource>), default(Expression<Func<TSource, CancellationToken, ValueTask<long?>>>), default(CancellationToken))), source.Expression, selector, Expression.Constant(cancellationToken, typeof(CancellationToken))), cancellationToken);
#else
            return source.Provider.ExecuteAsync<long?>(Expression.Call(((MethodInfo)MethodBase.GetCurrentMethod()).MakeGenericMethod(typeof(TSource)), source.Expression, selector, Expression.Constant(cancellationToken, typeof(CancellationToken))), cancellationToken);
#endif
        }

        public static Task<long> SumAsync<TSource>(this IAsyncQueryable<TSource> source, Expression<Func<TSource, CancellationToken, ValueTask<long>>> selector, CancellationToken cancellationToken = default)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));
            if (selector == null)
                throw new ArgumentNullException(nameof(selector));

#if CRIPPLED_REFLECTION
            return source.Provider.ExecuteAsync<long>(Expression.Call(InfoOf(() => AsyncQueryable.SumAsync<TSource>(default(IAsyncQueryable<TSource>), default(Expression<Func<TSource, CancellationToken, ValueTask<long>>>), default(CancellationToken))), source.Expression, selector, Expression.Constant(cancellationToken, typeof(CancellationToken))), cancellationToken);
#else
            return source.Provider.ExecuteAsync<long>(Expression.Call(((MethodInfo)MethodBase.GetCurrentMethod()).MakeGenericMethod(typeof(TSource)), source.Expression, selector, Expression.Constant(cancellationToken, typeof(CancellationToken))), cancellationToken);
#endif
        }

        public static Task<decimal?> SumAsync<TSource>(this IAsyncQueryable<TSource> source, Expression<Func<TSource, decimal?>> selector, CancellationToken cancellationToken = default)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));
            if (selector == null)
                throw new ArgumentNullException(nameof(selector));

#if CRIPPLED_REFLECTION
            return source.Provider.ExecuteAsync<decimal?>(Expression.Call(InfoOf(() => AsyncQueryable.SumAsync<TSource>(default(IAsyncQueryable<TSource>), default(Expression<Func<TSource, decimal?>>), default(CancellationToken))), source.Expression, selector, Expression.Constant(cancellationToken, typeof(CancellationToken))), cancellationToken);
#else
            return source.Provider.ExecuteAsync<decimal?>(Expression.Call(((MethodInfo)MethodBase.GetCurrentMethod()).MakeGenericMethod(typeof(TSource)), source.Expression, selector, Expression.Constant(cancellationToken, typeof(CancellationToken))), cancellationToken);
#endif
        }

        public static Task<decimal> SumAsync<TSource>(this IAsyncQueryable<TSource> source, Expression<Func<TSource, decimal>> selector, CancellationToken cancellationToken = default)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));
            if (selector == null)
                throw new ArgumentNullException(nameof(selector));

#if CRIPPLED_REFLECTION
            return source.Provider.ExecuteAsync<decimal>(Expression.Call(InfoOf(() => AsyncQueryable.SumAsync<TSource>(default(IAsyncQueryable<TSource>), default(Expression<Func<TSource, decimal>>), default(CancellationToken))), source.Expression, selector, Expression.Constant(cancellationToken, typeof(CancellationToken))), cancellationToken);
#else
            return source.Provider.ExecuteAsync<decimal>(Expression.Call(((MethodInfo)MethodBase.GetCurrentMethod()).MakeGenericMethod(typeof(TSource)), source.Expression, selector, Expression.Constant(cancellationToken, typeof(CancellationToken))), cancellationToken);
#endif
        }

        public static Task<double?> SumAsync<TSource>(this IAsyncQueryable<TSource> source, Expression<Func<TSource, double?>> selector, CancellationToken cancellationToken = default)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));
            if (selector == null)
                throw new ArgumentNullException(nameof(selector));

#if CRIPPLED_REFLECTION
            return source.Provider.ExecuteAsync<double?>(Expression.Call(InfoOf(() => AsyncQueryable.SumAsync<TSource>(default(IAsyncQueryable<TSource>), default(Expression<Func<TSource, double?>>), default(CancellationToken))), source.Expression, selector, Expression.Constant(cancellationToken, typeof(CancellationToken))), cancellationToken);
#else
            return source.Provider.ExecuteAsync<double?>(Expression.Call(((MethodInfo)MethodBase.GetCurrentMethod()).MakeGenericMethod(typeof(TSource)), source.Expression, selector, Expression.Constant(cancellationToken, typeof(CancellationToken))), cancellationToken);
#endif
        }

        public static Task<double> SumAsync<TSource>(this IAsyncQueryable<TSource> source, Expression<Func<TSource, double>> selector, CancellationToken cancellationToken = default)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));
            if (selector == null)
                throw new ArgumentNullException(nameof(selector));

#if CRIPPLED_REFLECTION
            return source.Provider.ExecuteAsync<double>(Expression.Call(InfoOf(() => AsyncQueryable.SumAsync<TSource>(default(IAsyncQueryable<TSource>), default(Expression<Func<TSource, double>>), default(CancellationToken))), source.Expression, selector, Expression.Constant(cancellationToken, typeof(CancellationToken))), cancellationToken);
#else
            return source.Provider.ExecuteAsync<double>(Expression.Call(((MethodInfo)MethodBase.GetCurrentMethod()).MakeGenericMethod(typeof(TSource)), source.Expression, selector, Expression.Constant(cancellationToken, typeof(CancellationToken))), cancellationToken);
#endif
        }

        public static Task<float?> SumAsync<TSource>(this IAsyncQueryable<TSource> source, Expression<Func<TSource, float?>> selector, CancellationToken cancellationToken = default)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));
            if (selector == null)
                throw new ArgumentNullException(nameof(selector));

#if CRIPPLED_REFLECTION
            return source.Provider.ExecuteAsync<float?>(Expression.Call(InfoOf(() => AsyncQueryable.SumAsync<TSource>(default(IAsyncQueryable<TSource>), default(Expression<Func<TSource, float?>>), default(CancellationToken))), source.Expression, selector, Expression.Constant(cancellationToken, typeof(CancellationToken))), cancellationToken);
#else
            return source.Provider.ExecuteAsync<float?>(Expression.Call(((MethodInfo)MethodBase.GetCurrentMethod()).MakeGenericMethod(typeof(TSource)), source.Expression, selector, Expression.Constant(cancellationToken, typeof(CancellationToken))), cancellationToken);
#endif
        }

        public static Task<float> SumAsync<TSource>(this IAsyncQueryable<TSource> source, Expression<Func<TSource, float>> selector, CancellationToken cancellationToken = default)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));
            if (selector == null)
                throw new ArgumentNullException(nameof(selector));

#if CRIPPLED_REFLECTION
            return source.Provider.ExecuteAsync<float>(Expression.Call(InfoOf(() => AsyncQueryable.SumAsync<TSource>(default(IAsyncQueryable<TSource>), default(Expression<Func<TSource, float>>), default(CancellationToken))), source.Expression, selector, Expression.Constant(cancellationToken, typeof(CancellationToken))), cancellationToken);
#else
            return source.Provider.ExecuteAsync<float>(Expression.Call(((MethodInfo)MethodBase.GetCurrentMethod()).MakeGenericMethod(typeof(TSource)), source.Expression, selector, Expression.Constant(cancellationToken, typeof(CancellationToken))), cancellationToken);
#endif
        }

        public static Task<int?> SumAsync<TSource>(this IAsyncQueryable<TSource> source, Expression<Func<TSource, int?>> selector, CancellationToken cancellationToken = default)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));
            if (selector == null)
                throw new ArgumentNullException(nameof(selector));

#if CRIPPLED_REFLECTION
            return source.Provider.ExecuteAsync<int?>(Expression.Call(InfoOf(() => AsyncQueryable.SumAsync<TSource>(default(IAsyncQueryable<TSource>), default(Expression<Func<TSource, int?>>), default(CancellationToken))), source.Expression, selector, Expression.Constant(cancellationToken, typeof(CancellationToken))), cancellationToken);
#else
            return source.Provider.ExecuteAsync<int?>(Expression.Call(((MethodInfo)MethodBase.GetCurrentMethod()).MakeGenericMethod(typeof(TSource)), source.Expression, selector, Expression.Constant(cancellationToken, typeof(CancellationToken))), cancellationToken);
#endif
        }

        public static Task<int> SumAsync<TSource>(this IAsyncQueryable<TSource> source, Expression<Func<TSource, int>> selector, CancellationToken cancellationToken = default)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));
            if (selector == null)
                throw new ArgumentNullException(nameof(selector));

#if CRIPPLED_REFLECTION
            return source.Provider.ExecuteAsync<int>(Expression.Call(InfoOf(() => AsyncQueryable.SumAsync<TSource>(default(IAsyncQueryable<TSource>), default(Expression<Func<TSource, int>>), default(CancellationToken))), source.Expression, selector, Expression.Constant(cancellationToken, typeof(CancellationToken))), cancellationToken);
#else
            return source.Provider.ExecuteAsync<int>(Expression.Call(((MethodInfo)MethodBase.GetCurrentMethod()).MakeGenericMethod(typeof(TSource)), source.Expression, selector, Expression.Constant(cancellationToken, typeof(CancellationToken))), cancellationToken);
#endif
        }

        public static Task<long?> SumAsync<TSource>(this IAsyncQueryable<TSource> source, Expression<Func<TSource, long?>> selector, CancellationToken cancellationToken = default)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));
            if (selector == null)
                throw new ArgumentNullException(nameof(selector));

#if CRIPPLED_REFLECTION
            return source.Provider.ExecuteAsync<long?>(Expression.Call(InfoOf(() => AsyncQueryable.SumAsync<TSource>(default(IAsyncQueryable<TSource>), default(Expression<Func<TSource, long?>>), default(CancellationToken))), source.Expression, selector, Expression.Constant(cancellationToken, typeof(CancellationToken))), cancellationToken);
#else
            return source.Provider.ExecuteAsync<long?>(Expression.Call(((MethodInfo)MethodBase.GetCurrentMethod()).MakeGenericMethod(typeof(TSource)), source.Expression, selector, Expression.Constant(cancellationToken, typeof(CancellationToken))), cancellationToken);
#endif
        }

        public static Task<long> SumAsync<TSource>(this IAsyncQueryable<TSource> source, Expression<Func<TSource, long>> selector, CancellationToken cancellationToken = default)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));
            if (selector == null)
                throw new ArgumentNullException(nameof(selector));

#if CRIPPLED_REFLECTION
            return source.Provider.ExecuteAsync<long>(Expression.Call(InfoOf(() => AsyncQueryable.SumAsync<TSource>(default(IAsyncQueryable<TSource>), default(Expression<Func<TSource, long>>), default(CancellationToken))), source.Expression, selector, Expression.Constant(cancellationToken, typeof(CancellationToken))), cancellationToken);
#else
            return source.Provider.ExecuteAsync<long>(Expression.Call(((MethodInfo)MethodBase.GetCurrentMethod()).MakeGenericMethod(typeof(TSource)), source.Expression, selector, Expression.Constant(cancellationToken, typeof(CancellationToken))), cancellationToken);
#endif
        }

        public static Task<decimal?> SumAsync<TSource>(this IAsyncQueryable<TSource> source, Expression<Func<TSource, ValueTask<decimal?>>> selector, CancellationToken cancellationToken = default)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));
            if (selector == null)
                throw new ArgumentNullException(nameof(selector));

#if CRIPPLED_REFLECTION
            return source.Provider.ExecuteAsync<decimal?>(Expression.Call(InfoOf(() => AsyncQueryable.SumAsync<TSource>(default(IAsyncQueryable<TSource>), default(Expression<Func<TSource, ValueTask<decimal?>>>), default(CancellationToken))), source.Expression, selector, Expression.Constant(cancellationToken, typeof(CancellationToken))), cancellationToken);
#else
            return source.Provider.ExecuteAsync<decimal?>(Expression.Call(((MethodInfo)MethodBase.GetCurrentMethod()).MakeGenericMethod(typeof(TSource)), source.Expression, selector, Expression.Constant(cancellationToken, typeof(CancellationToken))), cancellationToken);
#endif
        }

        public static Task<decimal> SumAsync<TSource>(this IAsyncQueryable<TSource> source, Expression<Func<TSource, ValueTask<decimal>>> selector, CancellationToken cancellationToken = default)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));
            if (selector == null)
                throw new ArgumentNullException(nameof(selector));

#if CRIPPLED_REFLECTION
            return source.Provider.ExecuteAsync<decimal>(Expression.Call(InfoOf(() => AsyncQueryable.SumAsync<TSource>(default(IAsyncQueryable<TSource>), default(Expression<Func<TSource, ValueTask<decimal>>>), default(CancellationToken))), source.Expression, selector, Expression.Constant(cancellationToken, typeof(CancellationToken))), cancellationToken);
#else
            return source.Provider.ExecuteAsync<decimal>(Expression.Call(((MethodInfo)MethodBase.GetCurrentMethod()).MakeGenericMethod(typeof(TSource)), source.Expression, selector, Expression.Constant(cancellationToken, typeof(CancellationToken))), cancellationToken);
#endif
        }

        public static Task<double?> SumAsync<TSource>(this IAsyncQueryable<TSource> source, Expression<Func<TSource, ValueTask<double?>>> selector, CancellationToken cancellationToken = default)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));
            if (selector == null)
                throw new ArgumentNullException(nameof(selector));

#if CRIPPLED_REFLECTION
            return source.Provider.ExecuteAsync<double?>(Expression.Call(InfoOf(() => AsyncQueryable.SumAsync<TSource>(default(IAsyncQueryable<TSource>), default(Expression<Func<TSource, ValueTask<double?>>>), default(CancellationToken))), source.Expression, selector, Expression.Constant(cancellationToken, typeof(CancellationToken))), cancellationToken);
#else
            return source.Provider.ExecuteAsync<double?>(Expression.Call(((MethodInfo)MethodBase.GetCurrentMethod()).MakeGenericMethod(typeof(TSource)), source.Expression, selector, Expression.Constant(cancellationToken, typeof(CancellationToken))), cancellationToken);
#endif
        }

        public static Task<double> SumAsync<TSource>(this IAsyncQueryable<TSource> source, Expression<Func<TSource, ValueTask<double>>> selector, CancellationToken cancellationToken = default)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));
            if (selector == null)
                throw new ArgumentNullException(nameof(selector));

#if CRIPPLED_REFLECTION
            return source.Provider.ExecuteAsync<double>(Expression.Call(InfoOf(() => AsyncQueryable.SumAsync<TSource>(default(IAsyncQueryable<TSource>), default(Expression<Func<TSource, ValueTask<double>>>), default(CancellationToken))), source.Expression, selector, Expression.Constant(cancellationToken, typeof(CancellationToken))), cancellationToken);
#else
            return source.Provider.ExecuteAsync<double>(Expression.Call(((MethodInfo)MethodBase.GetCurrentMethod()).MakeGenericMethod(typeof(TSource)), source.Expression, selector, Expression.Constant(cancellationToken, typeof(CancellationToken))), cancellationToken);
#endif
        }

        public static Task<float?> SumAsync<TSource>(this IAsyncQueryable<TSource> source, Expression<Func<TSource, ValueTask<float?>>> selector, CancellationToken cancellationToken = default)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));
            if (selector == null)
                throw new ArgumentNullException(nameof(selector));

#if CRIPPLED_REFLECTION
            return source.Provider.ExecuteAsync<float?>(Expression.Call(InfoOf(() => AsyncQueryable.SumAsync<TSource>(default(IAsyncQueryable<TSource>), default(Expression<Func<TSource, ValueTask<float?>>>), default(CancellationToken))), source.Expression, selector, Expression.Constant(cancellationToken, typeof(CancellationToken))), cancellationToken);
#else
            return source.Provider.ExecuteAsync<float?>(Expression.Call(((MethodInfo)MethodBase.GetCurrentMethod()).MakeGenericMethod(typeof(TSource)), source.Expression, selector, Expression.Constant(cancellationToken, typeof(CancellationToken))), cancellationToken);
#endif
        }

        public static Task<float> SumAsync<TSource>(this IAsyncQueryable<TSource> source, Expression<Func<TSource, ValueTask<float>>> selector, CancellationToken cancellationToken = default)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));
            if (selector == null)
                throw new ArgumentNullException(nameof(selector));

#if CRIPPLED_REFLECTION
            return source.Provider.ExecuteAsync<float>(Expression.Call(InfoOf(() => AsyncQueryable.SumAsync<TSource>(default(IAsyncQueryable<TSource>), default(Expression<Func<TSource, ValueTask<float>>>), default(CancellationToken))), source.Expression, selector, Expression.Constant(cancellationToken, typeof(CancellationToken))), cancellationToken);
#else
            return source.Provider.ExecuteAsync<float>(Expression.Call(((MethodInfo)MethodBase.GetCurrentMethod()).MakeGenericMethod(typeof(TSource)), source.Expression, selector, Expression.Constant(cancellationToken, typeof(CancellationToken))), cancellationToken);
#endif
        }

        public static Task<int?> SumAsync<TSource>(this IAsyncQueryable<TSource> source, Expression<Func<TSource, ValueTask<int?>>> selector, CancellationToken cancellationToken = default)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));
            if (selector == null)
                throw new ArgumentNullException(nameof(selector));

#if CRIPPLED_REFLECTION
            return source.Provider.ExecuteAsync<int?>(Expression.Call(InfoOf(() => AsyncQueryable.SumAsync<TSource>(default(IAsyncQueryable<TSource>), default(Expression<Func<TSource, ValueTask<int?>>>), default(CancellationToken))), source.Expression, selector, Expression.Constant(cancellationToken, typeof(CancellationToken))), cancellationToken);
#else
            return source.Provider.ExecuteAsync<int?>(Expression.Call(((MethodInfo)MethodBase.GetCurrentMethod()).MakeGenericMethod(typeof(TSource)), source.Expression, selector, Expression.Constant(cancellationToken, typeof(CancellationToken))), cancellationToken);
#endif
        }

        public static Task<int> SumAsync<TSource>(this IAsyncQueryable<TSource> source, Expression<Func<TSource, ValueTask<int>>> selector, CancellationToken cancellationToken = default)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));
            if (selector == null)
                throw new ArgumentNullException(nameof(selector));

#if CRIPPLED_REFLECTION
            return source.Provider.ExecuteAsync<int>(Expression.Call(InfoOf(() => AsyncQueryable.SumAsync<TSource>(default(IAsyncQueryable<TSource>), default(Expression<Func<TSource, ValueTask<int>>>), default(CancellationToken))), source.Expression, selector, Expression.Constant(cancellationToken, typeof(CancellationToken))), cancellationToken);
#else
            return source.Provider.ExecuteAsync<int>(Expression.Call(((MethodInfo)MethodBase.GetCurrentMethod()).MakeGenericMethod(typeof(TSource)), source.Expression, selector, Expression.Constant(cancellationToken, typeof(CancellationToken))), cancellationToken);
#endif
        }

        public static Task<long?> SumAsync<TSource>(this IAsyncQueryable<TSource> source, Expression<Func<TSource, ValueTask<long?>>> selector, CancellationToken cancellationToken = default)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));
            if (selector == null)
                throw new ArgumentNullException(nameof(selector));

#if CRIPPLED_REFLECTION
            return source.Provider.ExecuteAsync<long?>(Expression.Call(InfoOf(() => AsyncQueryable.SumAsync<TSource>(default(IAsyncQueryable<TSource>), default(Expression<Func<TSource, ValueTask<long?>>>), default(CancellationToken))), source.Expression, selector, Expression.Constant(cancellationToken, typeof(CancellationToken))), cancellationToken);
#else
            return source.Provider.ExecuteAsync<long?>(Expression.Call(((MethodInfo)MethodBase.GetCurrentMethod()).MakeGenericMethod(typeof(TSource)), source.Expression, selector, Expression.Constant(cancellationToken, typeof(CancellationToken))), cancellationToken);
#endif
        }

        public static Task<long> SumAsync<TSource>(this IAsyncQueryable<TSource> source, Expression<Func<TSource, ValueTask<long>>> selector, CancellationToken cancellationToken = default)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));
            if (selector == null)
                throw new ArgumentNullException(nameof(selector));

#if CRIPPLED_REFLECTION
            return source.Provider.ExecuteAsync<long>(Expression.Call(InfoOf(() => AsyncQueryable.SumAsync<TSource>(default(IAsyncQueryable<TSource>), default(Expression<Func<TSource, ValueTask<long>>>), default(CancellationToken))), source.Expression, selector, Expression.Constant(cancellationToken, typeof(CancellationToken))), cancellationToken);
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

        public static IAsyncQueryable<TSource> TakeWhile<TSource>(this IAsyncQueryable<TSource> source, Expression<Func<TSource, CancellationToken, ValueTask<bool>>> predicate)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));
            if (predicate == null)
                throw new ArgumentNullException(nameof(predicate));

#if CRIPPLED_REFLECTION
            return source.Provider.CreateQuery<TSource>(Expression.Call(InfoOf(() => AsyncQueryable.TakeWhile<TSource>(default(IAsyncQueryable<TSource>), default(Expression<Func<TSource, CancellationToken, ValueTask<bool>>>))), source.Expression, predicate));
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

        public static IAsyncQueryable<TSource> TakeWhile<TSource>(this IAsyncQueryable<TSource> source, Expression<Func<TSource, int, CancellationToken, ValueTask<bool>>> predicate)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));
            if (predicate == null)
                throw new ArgumentNullException(nameof(predicate));

#if CRIPPLED_REFLECTION
            return source.Provider.CreateQuery<TSource>(Expression.Call(InfoOf(() => AsyncQueryable.TakeWhile<TSource>(default(IAsyncQueryable<TSource>), default(Expression<Func<TSource, int, CancellationToken, ValueTask<bool>>>))), source.Expression, predicate));
#else
            return source.Provider.CreateQuery<TSource>(Expression.Call(((MethodInfo)MethodBase.GetCurrentMethod()).MakeGenericMethod(typeof(TSource)), source.Expression, predicate));
#endif
        }

        public static IAsyncQueryable<TSource> TakeWhile<TSource>(this IAsyncQueryable<TSource> source, Expression<Func<TSource, int, ValueTask<bool>>> predicate)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));
            if (predicate == null)
                throw new ArgumentNullException(nameof(predicate));

#if CRIPPLED_REFLECTION
            return source.Provider.CreateQuery<TSource>(Expression.Call(InfoOf(() => AsyncQueryable.TakeWhile<TSource>(default(IAsyncQueryable<TSource>), default(Expression<Func<TSource, int, ValueTask<bool>>>))), source.Expression, predicate));
#else
            return source.Provider.CreateQuery<TSource>(Expression.Call(((MethodInfo)MethodBase.GetCurrentMethod()).MakeGenericMethod(typeof(TSource)), source.Expression, predicate));
#endif
        }

        public static IAsyncQueryable<TSource> TakeWhile<TSource>(this IAsyncQueryable<TSource> source, Expression<Func<TSource, ValueTask<bool>>> predicate)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));
            if (predicate == null)
                throw new ArgumentNullException(nameof(predicate));

#if CRIPPLED_REFLECTION
            return source.Provider.CreateQuery<TSource>(Expression.Call(InfoOf(() => AsyncQueryable.TakeWhile<TSource>(default(IAsyncQueryable<TSource>), default(Expression<Func<TSource, ValueTask<bool>>>))), source.Expression, predicate));
#else
            return source.Provider.CreateQuery<TSource>(Expression.Call(((MethodInfo)MethodBase.GetCurrentMethod()).MakeGenericMethod(typeof(TSource)), source.Expression, predicate));
#endif
        }

        public static IOrderedAsyncQueryable<TSource> ThenBy<TSource, TKey>(this IOrderedAsyncQueryable<TSource> source, Expression<Func<TSource, CancellationToken, ValueTask<TKey>>> keySelector)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));
            if (keySelector == null)
                throw new ArgumentNullException(nameof(keySelector));

#if CRIPPLED_REFLECTION
            return (IOrderedAsyncQueryable<TSource>)source.Provider.CreateQuery<TSource>(Expression.Call(InfoOf(() => AsyncQueryable.ThenBy<TSource, TKey>(default(IOrderedAsyncQueryable<TSource>), default(Expression<Func<TSource, CancellationToken, ValueTask<TKey>>>))), source.Expression, keySelector));
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

        public static IOrderedAsyncQueryable<TSource> ThenBy<TSource, TKey>(this IOrderedAsyncQueryable<TSource> source, Expression<Func<TSource, ValueTask<TKey>>> keySelector)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));
            if (keySelector == null)
                throw new ArgumentNullException(nameof(keySelector));

#if CRIPPLED_REFLECTION
            return (IOrderedAsyncQueryable<TSource>)source.Provider.CreateQuery<TSource>(Expression.Call(InfoOf(() => AsyncQueryable.ThenBy<TSource, TKey>(default(IOrderedAsyncQueryable<TSource>), default(Expression<Func<TSource, ValueTask<TKey>>>))), source.Expression, keySelector));
#else
            return (IOrderedAsyncQueryable<TSource>)source.Provider.CreateQuery<TSource>(Expression.Call(((MethodInfo)MethodBase.GetCurrentMethod()).MakeGenericMethod(typeof(TSource), typeof(TKey)), source.Expression, keySelector));
#endif
        }

        public static IOrderedAsyncQueryable<TSource> ThenBy<TSource, TKey>(this IOrderedAsyncQueryable<TSource> source, Expression<Func<TSource, CancellationToken, ValueTask<TKey>>> keySelector, IComparer<TKey> comparer)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));
            if (keySelector == null)
                throw new ArgumentNullException(nameof(keySelector));

#if CRIPPLED_REFLECTION
            return (IOrderedAsyncQueryable<TSource>)source.Provider.CreateQuery<TSource>(Expression.Call(InfoOf(() => AsyncQueryable.ThenBy<TSource, TKey>(default(IOrderedAsyncQueryable<TSource>), default(Expression<Func<TSource, CancellationToken, ValueTask<TKey>>>), default(IComparer<TKey>))), source.Expression, keySelector, Expression.Constant(comparer, typeof(IComparer<TKey>))));
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

#if CRIPPLED_REFLECTION
            return (IOrderedAsyncQueryable<TSource>)source.Provider.CreateQuery<TSource>(Expression.Call(InfoOf(() => AsyncQueryable.ThenBy<TSource, TKey>(default(IOrderedAsyncQueryable<TSource>), default(Expression<Func<TSource, TKey>>), default(IComparer<TKey>))), source.Expression, keySelector, Expression.Constant(comparer, typeof(IComparer<TKey>))));
#else
            return (IOrderedAsyncQueryable<TSource>)source.Provider.CreateQuery<TSource>(Expression.Call(((MethodInfo)MethodBase.GetCurrentMethod()).MakeGenericMethod(typeof(TSource), typeof(TKey)), source.Expression, keySelector, Expression.Constant(comparer, typeof(IComparer<TKey>))));
#endif
        }

        public static IOrderedAsyncQueryable<TSource> ThenBy<TSource, TKey>(this IOrderedAsyncQueryable<TSource> source, Expression<Func<TSource, ValueTask<TKey>>> keySelector, IComparer<TKey> comparer)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));
            if (keySelector == null)
                throw new ArgumentNullException(nameof(keySelector));

#if CRIPPLED_REFLECTION
            return (IOrderedAsyncQueryable<TSource>)source.Provider.CreateQuery<TSource>(Expression.Call(InfoOf(() => AsyncQueryable.ThenBy<TSource, TKey>(default(IOrderedAsyncQueryable<TSource>), default(Expression<Func<TSource, ValueTask<TKey>>>), default(IComparer<TKey>))), source.Expression, keySelector, Expression.Constant(comparer, typeof(IComparer<TKey>))));
#else
            return (IOrderedAsyncQueryable<TSource>)source.Provider.CreateQuery<TSource>(Expression.Call(((MethodInfo)MethodBase.GetCurrentMethod()).MakeGenericMethod(typeof(TSource), typeof(TKey)), source.Expression, keySelector, Expression.Constant(comparer, typeof(IComparer<TKey>))));
#endif
        }

        public static IOrderedAsyncQueryable<TSource> ThenByDescending<TSource, TKey>(this IOrderedAsyncQueryable<TSource> source, Expression<Func<TSource, CancellationToken, ValueTask<TKey>>> keySelector)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));
            if (keySelector == null)
                throw new ArgumentNullException(nameof(keySelector));

#if CRIPPLED_REFLECTION
            return (IOrderedAsyncQueryable<TSource>)source.Provider.CreateQuery<TSource>(Expression.Call(InfoOf(() => AsyncQueryable.ThenByDescending<TSource, TKey>(default(IOrderedAsyncQueryable<TSource>), default(Expression<Func<TSource, CancellationToken, ValueTask<TKey>>>))), source.Expression, keySelector));
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

        public static IOrderedAsyncQueryable<TSource> ThenByDescending<TSource, TKey>(this IOrderedAsyncQueryable<TSource> source, Expression<Func<TSource, ValueTask<TKey>>> keySelector)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));
            if (keySelector == null)
                throw new ArgumentNullException(nameof(keySelector));

#if CRIPPLED_REFLECTION
            return (IOrderedAsyncQueryable<TSource>)source.Provider.CreateQuery<TSource>(Expression.Call(InfoOf(() => AsyncQueryable.ThenByDescending<TSource, TKey>(default(IOrderedAsyncQueryable<TSource>), default(Expression<Func<TSource, ValueTask<TKey>>>))), source.Expression, keySelector));
#else
            return (IOrderedAsyncQueryable<TSource>)source.Provider.CreateQuery<TSource>(Expression.Call(((MethodInfo)MethodBase.GetCurrentMethod()).MakeGenericMethod(typeof(TSource), typeof(TKey)), source.Expression, keySelector));
#endif
        }

        public static IOrderedAsyncQueryable<TSource> ThenByDescending<TSource, TKey>(this IOrderedAsyncQueryable<TSource> source, Expression<Func<TSource, CancellationToken, ValueTask<TKey>>> keySelector, IComparer<TKey> comparer)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));
            if (keySelector == null)
                throw new ArgumentNullException(nameof(keySelector));

#if CRIPPLED_REFLECTION
            return (IOrderedAsyncQueryable<TSource>)source.Provider.CreateQuery<TSource>(Expression.Call(InfoOf(() => AsyncQueryable.ThenByDescending<TSource, TKey>(default(IOrderedAsyncQueryable<TSource>), default(Expression<Func<TSource, CancellationToken, ValueTask<TKey>>>), default(IComparer<TKey>))), source.Expression, keySelector, Expression.Constant(comparer, typeof(IComparer<TKey>))));
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

#if CRIPPLED_REFLECTION
            return (IOrderedAsyncQueryable<TSource>)source.Provider.CreateQuery<TSource>(Expression.Call(InfoOf(() => AsyncQueryable.ThenByDescending<TSource, TKey>(default(IOrderedAsyncQueryable<TSource>), default(Expression<Func<TSource, TKey>>), default(IComparer<TKey>))), source.Expression, keySelector, Expression.Constant(comparer, typeof(IComparer<TKey>))));
#else
            return (IOrderedAsyncQueryable<TSource>)source.Provider.CreateQuery<TSource>(Expression.Call(((MethodInfo)MethodBase.GetCurrentMethod()).MakeGenericMethod(typeof(TSource), typeof(TKey)), source.Expression, keySelector, Expression.Constant(comparer, typeof(IComparer<TKey>))));
#endif
        }

        public static IOrderedAsyncQueryable<TSource> ThenByDescending<TSource, TKey>(this IOrderedAsyncQueryable<TSource> source, Expression<Func<TSource, ValueTask<TKey>>> keySelector, IComparer<TKey> comparer)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));
            if (keySelector == null)
                throw new ArgumentNullException(nameof(keySelector));

#if CRIPPLED_REFLECTION
            return (IOrderedAsyncQueryable<TSource>)source.Provider.CreateQuery<TSource>(Expression.Call(InfoOf(() => AsyncQueryable.ThenByDescending<TSource, TKey>(default(IOrderedAsyncQueryable<TSource>), default(Expression<Func<TSource, ValueTask<TKey>>>), default(IComparer<TKey>))), source.Expression, keySelector, Expression.Constant(comparer, typeof(IComparer<TKey>))));
#else
            return (IOrderedAsyncQueryable<TSource>)source.Provider.CreateQuery<TSource>(Expression.Call(((MethodInfo)MethodBase.GetCurrentMethod()).MakeGenericMethod(typeof(TSource), typeof(TKey)), source.Expression, keySelector, Expression.Constant(comparer, typeof(IComparer<TKey>))));
#endif
        }

        public static Task<TSource[]> ToArrayAsync<TSource>(this IAsyncQueryable<TSource> source, CancellationToken cancellationToken = default)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));

#if CRIPPLED_REFLECTION
            return source.Provider.ExecuteAsync<TSource[]>(Expression.Call(InfoOf(() => AsyncQueryable.ToArrayAsync<TSource>(default(IAsyncQueryable<TSource>), default(CancellationToken))), source.Expression, Expression.Constant(cancellationToken, typeof(CancellationToken))), cancellationToken);
#else
            return source.Provider.ExecuteAsync<TSource[]>(Expression.Call(((MethodInfo)MethodBase.GetCurrentMethod()).MakeGenericMethod(typeof(TSource)), source.Expression, Expression.Constant(cancellationToken, typeof(CancellationToken))), cancellationToken);
#endif
        }

        public static Task<Dictionary<TKey, TSource>> ToDictionaryAsync<TSource, TKey>(this IAsyncQueryable<TSource> source, Expression<Func<TSource, CancellationToken, ValueTask<TKey>>> keySelector, CancellationToken cancellationToken = default)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));
            if (keySelector == null)
                throw new ArgumentNullException(nameof(keySelector));

#if CRIPPLED_REFLECTION
            return source.Provider.ExecuteAsync<Dictionary<TKey, TSource>>(Expression.Call(InfoOf(() => AsyncQueryable.ToDictionaryAsync<TSource, TKey>(default(IAsyncQueryable<TSource>), default(Expression<Func<TSource, CancellationToken, ValueTask<TKey>>>), default(CancellationToken))), source.Expression, keySelector, Expression.Constant(cancellationToken, typeof(CancellationToken))), cancellationToken);
#else
            return source.Provider.ExecuteAsync<Dictionary<TKey, TSource>>(Expression.Call(((MethodInfo)MethodBase.GetCurrentMethod()).MakeGenericMethod(typeof(TSource), typeof(TKey)), source.Expression, keySelector, Expression.Constant(cancellationToken, typeof(CancellationToken))), cancellationToken);
#endif
        }

        public static Task<Dictionary<TKey, TSource>> ToDictionaryAsync<TSource, TKey>(this IAsyncQueryable<TSource> source, Expression<Func<TSource, TKey>> keySelector, CancellationToken cancellationToken = default)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));
            if (keySelector == null)
                throw new ArgumentNullException(nameof(keySelector));

#if CRIPPLED_REFLECTION
            return source.Provider.ExecuteAsync<Dictionary<TKey, TSource>>(Expression.Call(InfoOf(() => AsyncQueryable.ToDictionaryAsync<TSource, TKey>(default(IAsyncQueryable<TSource>), default(Expression<Func<TSource, TKey>>), default(CancellationToken))), source.Expression, keySelector, Expression.Constant(cancellationToken, typeof(CancellationToken))), cancellationToken);
#else
            return source.Provider.ExecuteAsync<Dictionary<TKey, TSource>>(Expression.Call(((MethodInfo)MethodBase.GetCurrentMethod()).MakeGenericMethod(typeof(TSource), typeof(TKey)), source.Expression, keySelector, Expression.Constant(cancellationToken, typeof(CancellationToken))), cancellationToken);
#endif
        }

        public static Task<Dictionary<TKey, TSource>> ToDictionaryAsync<TSource, TKey>(this IAsyncQueryable<TSource> source, Expression<Func<TSource, ValueTask<TKey>>> keySelector, CancellationToken cancellationToken = default)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));
            if (keySelector == null)
                throw new ArgumentNullException(nameof(keySelector));

#if CRIPPLED_REFLECTION
            return source.Provider.ExecuteAsync<Dictionary<TKey, TSource>>(Expression.Call(InfoOf(() => AsyncQueryable.ToDictionaryAsync<TSource, TKey>(default(IAsyncQueryable<TSource>), default(Expression<Func<TSource, ValueTask<TKey>>>), default(CancellationToken))), source.Expression, keySelector, Expression.Constant(cancellationToken, typeof(CancellationToken))), cancellationToken);
#else
            return source.Provider.ExecuteAsync<Dictionary<TKey, TSource>>(Expression.Call(((MethodInfo)MethodBase.GetCurrentMethod()).MakeGenericMethod(typeof(TSource), typeof(TKey)), source.Expression, keySelector, Expression.Constant(cancellationToken, typeof(CancellationToken))), cancellationToken);
#endif
        }

        public static Task<Dictionary<TKey, TSource>> ToDictionaryAsync<TSource, TKey>(this IAsyncQueryable<TSource> source, Expression<Func<TSource, CancellationToken, ValueTask<TKey>>> keySelector, IEqualityComparer<TKey> comparer, CancellationToken cancellationToken = default)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));
            if (keySelector == null)
                throw new ArgumentNullException(nameof(keySelector));

#if CRIPPLED_REFLECTION
            return source.Provider.ExecuteAsync<Dictionary<TKey, TSource>>(Expression.Call(InfoOf(() => AsyncQueryable.ToDictionaryAsync<TSource, TKey>(default(IAsyncQueryable<TSource>), default(Expression<Func<TSource, CancellationToken, ValueTask<TKey>>>), default(IEqualityComparer<TKey>), default(CancellationToken))), source.Expression, keySelector, Expression.Constant(comparer, typeof(IEqualityComparer<TKey>)), Expression.Constant(cancellationToken, typeof(CancellationToken))), cancellationToken);
#else
            return source.Provider.ExecuteAsync<Dictionary<TKey, TSource>>(Expression.Call(((MethodInfo)MethodBase.GetCurrentMethod()).MakeGenericMethod(typeof(TSource), typeof(TKey)), source.Expression, keySelector, Expression.Constant(comparer, typeof(IEqualityComparer<TKey>)), Expression.Constant(cancellationToken, typeof(CancellationToken))), cancellationToken);
#endif
        }

        public static Task<Dictionary<TKey, TSource>> ToDictionaryAsync<TSource, TKey>(this IAsyncQueryable<TSource> source, Expression<Func<TSource, TKey>> keySelector, IEqualityComparer<TKey> comparer, CancellationToken cancellationToken = default)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));
            if (keySelector == null)
                throw new ArgumentNullException(nameof(keySelector));

#if CRIPPLED_REFLECTION
            return source.Provider.ExecuteAsync<Dictionary<TKey, TSource>>(Expression.Call(InfoOf(() => AsyncQueryable.ToDictionaryAsync<TSource, TKey>(default(IAsyncQueryable<TSource>), default(Expression<Func<TSource, TKey>>), default(IEqualityComparer<TKey>), default(CancellationToken))), source.Expression, keySelector, Expression.Constant(comparer, typeof(IEqualityComparer<TKey>)), Expression.Constant(cancellationToken, typeof(CancellationToken))), cancellationToken);
#else
            return source.Provider.ExecuteAsync<Dictionary<TKey, TSource>>(Expression.Call(((MethodInfo)MethodBase.GetCurrentMethod()).MakeGenericMethod(typeof(TSource), typeof(TKey)), source.Expression, keySelector, Expression.Constant(comparer, typeof(IEqualityComparer<TKey>)), Expression.Constant(cancellationToken, typeof(CancellationToken))), cancellationToken);
#endif
        }

        public static Task<Dictionary<TKey, TSource>> ToDictionaryAsync<TSource, TKey>(this IAsyncQueryable<TSource> source, Expression<Func<TSource, ValueTask<TKey>>> keySelector, IEqualityComparer<TKey> comparer, CancellationToken cancellationToken = default)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));
            if (keySelector == null)
                throw new ArgumentNullException(nameof(keySelector));

#if CRIPPLED_REFLECTION
            return source.Provider.ExecuteAsync<Dictionary<TKey, TSource>>(Expression.Call(InfoOf(() => AsyncQueryable.ToDictionaryAsync<TSource, TKey>(default(IAsyncQueryable<TSource>), default(Expression<Func<TSource, ValueTask<TKey>>>), default(IEqualityComparer<TKey>), default(CancellationToken))), source.Expression, keySelector, Expression.Constant(comparer, typeof(IEqualityComparer<TKey>)), Expression.Constant(cancellationToken, typeof(CancellationToken))), cancellationToken);
#else
            return source.Provider.ExecuteAsync<Dictionary<TKey, TSource>>(Expression.Call(((MethodInfo)MethodBase.GetCurrentMethod()).MakeGenericMethod(typeof(TSource), typeof(TKey)), source.Expression, keySelector, Expression.Constant(comparer, typeof(IEqualityComparer<TKey>)), Expression.Constant(cancellationToken, typeof(CancellationToken))), cancellationToken);
#endif
        }

        public static Task<Dictionary<TKey, TElement>> ToDictionaryAsync<TSource, TKey, TElement>(this IAsyncQueryable<TSource> source, Expression<Func<TSource, CancellationToken, ValueTask<TKey>>> keySelector, Expression<Func<TSource, CancellationToken, ValueTask<TElement>>> elementSelector, CancellationToken cancellationToken = default)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));
            if (keySelector == null)
                throw new ArgumentNullException(nameof(keySelector));
            if (elementSelector == null)
                throw new ArgumentNullException(nameof(elementSelector));

#if CRIPPLED_REFLECTION
            return source.Provider.ExecuteAsync<Dictionary<TKey, TElement>>(Expression.Call(InfoOf(() => AsyncQueryable.ToDictionaryAsync<TSource, TKey, TElement>(default(IAsyncQueryable<TSource>), default(Expression<Func<TSource, CancellationToken, ValueTask<TKey>>>), default(Expression<Func<TSource, CancellationToken, ValueTask<TElement>>>), default(CancellationToken))), source.Expression, keySelector, elementSelector, Expression.Constant(cancellationToken, typeof(CancellationToken))), cancellationToken);
#else
            return source.Provider.ExecuteAsync<Dictionary<TKey, TElement>>(Expression.Call(((MethodInfo)MethodBase.GetCurrentMethod()).MakeGenericMethod(typeof(TSource), typeof(TKey), typeof(TElement)), source.Expression, keySelector, elementSelector, Expression.Constant(cancellationToken, typeof(CancellationToken))), cancellationToken);
#endif
        }

        public static Task<Dictionary<TKey, TElement>> ToDictionaryAsync<TSource, TKey, TElement>(this IAsyncQueryable<TSource> source, Expression<Func<TSource, TKey>> keySelector, Expression<Func<TSource, TElement>> elementSelector, CancellationToken cancellationToken = default)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));
            if (keySelector == null)
                throw new ArgumentNullException(nameof(keySelector));
            if (elementSelector == null)
                throw new ArgumentNullException(nameof(elementSelector));

#if CRIPPLED_REFLECTION
            return source.Provider.ExecuteAsync<Dictionary<TKey, TElement>>(Expression.Call(InfoOf(() => AsyncQueryable.ToDictionaryAsync<TSource, TKey, TElement>(default(IAsyncQueryable<TSource>), default(Expression<Func<TSource, TKey>>), default(Expression<Func<TSource, TElement>>), default(CancellationToken))), source.Expression, keySelector, elementSelector, Expression.Constant(cancellationToken, typeof(CancellationToken))), cancellationToken);
#else
            return source.Provider.ExecuteAsync<Dictionary<TKey, TElement>>(Expression.Call(((MethodInfo)MethodBase.GetCurrentMethod()).MakeGenericMethod(typeof(TSource), typeof(TKey), typeof(TElement)), source.Expression, keySelector, elementSelector, Expression.Constant(cancellationToken, typeof(CancellationToken))), cancellationToken);
#endif
        }

        public static Task<Dictionary<TKey, TElement>> ToDictionaryAsync<TSource, TKey, TElement>(this IAsyncQueryable<TSource> source, Expression<Func<TSource, ValueTask<TKey>>> keySelector, Expression<Func<TSource, ValueTask<TElement>>> elementSelector, CancellationToken cancellationToken = default)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));
            if (keySelector == null)
                throw new ArgumentNullException(nameof(keySelector));
            if (elementSelector == null)
                throw new ArgumentNullException(nameof(elementSelector));

#if CRIPPLED_REFLECTION
            return source.Provider.ExecuteAsync<Dictionary<TKey, TElement>>(Expression.Call(InfoOf(() => AsyncQueryable.ToDictionaryAsync<TSource, TKey, TElement>(default(IAsyncQueryable<TSource>), default(Expression<Func<TSource, ValueTask<TKey>>>), default(Expression<Func<TSource, ValueTask<TElement>>>), default(CancellationToken))), source.Expression, keySelector, elementSelector, Expression.Constant(cancellationToken, typeof(CancellationToken))), cancellationToken);
#else
            return source.Provider.ExecuteAsync<Dictionary<TKey, TElement>>(Expression.Call(((MethodInfo)MethodBase.GetCurrentMethod()).MakeGenericMethod(typeof(TSource), typeof(TKey), typeof(TElement)), source.Expression, keySelector, elementSelector, Expression.Constant(cancellationToken, typeof(CancellationToken))), cancellationToken);
#endif
        }

        public static Task<Dictionary<TKey, TElement>> ToDictionaryAsync<TSource, TKey, TElement>(this IAsyncQueryable<TSource> source, Expression<Func<TSource, CancellationToken, ValueTask<TKey>>> keySelector, Expression<Func<TSource, CancellationToken, ValueTask<TElement>>> elementSelector, IEqualityComparer<TKey> comparer, CancellationToken cancellationToken = default)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));
            if (keySelector == null)
                throw new ArgumentNullException(nameof(keySelector));
            if (elementSelector == null)
                throw new ArgumentNullException(nameof(elementSelector));

#if CRIPPLED_REFLECTION
            return source.Provider.ExecuteAsync<Dictionary<TKey, TElement>>(Expression.Call(InfoOf(() => AsyncQueryable.ToDictionaryAsync<TSource, TKey, TElement>(default(IAsyncQueryable<TSource>), default(Expression<Func<TSource, CancellationToken, ValueTask<TKey>>>), default(Expression<Func<TSource, CancellationToken, ValueTask<TElement>>>), default(IEqualityComparer<TKey>), default(CancellationToken))), source.Expression, keySelector, elementSelector, Expression.Constant(comparer, typeof(IEqualityComparer<TKey>)), Expression.Constant(cancellationToken, typeof(CancellationToken))), cancellationToken);
#else
            return source.Provider.ExecuteAsync<Dictionary<TKey, TElement>>(Expression.Call(((MethodInfo)MethodBase.GetCurrentMethod()).MakeGenericMethod(typeof(TSource), typeof(TKey), typeof(TElement)), source.Expression, keySelector, elementSelector, Expression.Constant(comparer, typeof(IEqualityComparer<TKey>)), Expression.Constant(cancellationToken, typeof(CancellationToken))), cancellationToken);
#endif
        }

        public static Task<Dictionary<TKey, TElement>> ToDictionaryAsync<TSource, TKey, TElement>(this IAsyncQueryable<TSource> source, Expression<Func<TSource, TKey>> keySelector, Expression<Func<TSource, TElement>> elementSelector, IEqualityComparer<TKey> comparer, CancellationToken cancellationToken = default)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));
            if (keySelector == null)
                throw new ArgumentNullException(nameof(keySelector));
            if (elementSelector == null)
                throw new ArgumentNullException(nameof(elementSelector));

#if CRIPPLED_REFLECTION
            return source.Provider.ExecuteAsync<Dictionary<TKey, TElement>>(Expression.Call(InfoOf(() => AsyncQueryable.ToDictionaryAsync<TSource, TKey, TElement>(default(IAsyncQueryable<TSource>), default(Expression<Func<TSource, TKey>>), default(Expression<Func<TSource, TElement>>), default(IEqualityComparer<TKey>), default(CancellationToken))), source.Expression, keySelector, elementSelector, Expression.Constant(comparer, typeof(IEqualityComparer<TKey>)), Expression.Constant(cancellationToken, typeof(CancellationToken))), cancellationToken);
#else
            return source.Provider.ExecuteAsync<Dictionary<TKey, TElement>>(Expression.Call(((MethodInfo)MethodBase.GetCurrentMethod()).MakeGenericMethod(typeof(TSource), typeof(TKey), typeof(TElement)), source.Expression, keySelector, elementSelector, Expression.Constant(comparer, typeof(IEqualityComparer<TKey>)), Expression.Constant(cancellationToken, typeof(CancellationToken))), cancellationToken);
#endif
        }

        public static Task<Dictionary<TKey, TElement>> ToDictionaryAsync<TSource, TKey, TElement>(this IAsyncQueryable<TSource> source, Expression<Func<TSource, ValueTask<TKey>>> keySelector, Expression<Func<TSource, ValueTask<TElement>>> elementSelector, IEqualityComparer<TKey> comparer, CancellationToken cancellationToken = default)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));
            if (keySelector == null)
                throw new ArgumentNullException(nameof(keySelector));
            if (elementSelector == null)
                throw new ArgumentNullException(nameof(elementSelector));

#if CRIPPLED_REFLECTION
            return source.Provider.ExecuteAsync<Dictionary<TKey, TElement>>(Expression.Call(InfoOf(() => AsyncQueryable.ToDictionaryAsync<TSource, TKey, TElement>(default(IAsyncQueryable<TSource>), default(Expression<Func<TSource, ValueTask<TKey>>>), default(Expression<Func<TSource, ValueTask<TElement>>>), default(IEqualityComparer<TKey>), default(CancellationToken))), source.Expression, keySelector, elementSelector, Expression.Constant(comparer, typeof(IEqualityComparer<TKey>)), Expression.Constant(cancellationToken, typeof(CancellationToken))), cancellationToken);
#else
            return source.Provider.ExecuteAsync<Dictionary<TKey, TElement>>(Expression.Call(((MethodInfo)MethodBase.GetCurrentMethod()).MakeGenericMethod(typeof(TSource), typeof(TKey), typeof(TElement)), source.Expression, keySelector, elementSelector, Expression.Constant(comparer, typeof(IEqualityComparer<TKey>)), Expression.Constant(cancellationToken, typeof(CancellationToken))), cancellationToken);
#endif
        }

        public static Task<HashSet<TSource>> ToHashSetAsync<TSource>(this IAsyncQueryable<TSource> source, CancellationToken cancellationToken = default)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));

#if CRIPPLED_REFLECTION
            return source.Provider.ExecuteAsync<HashSet<TSource>>(Expression.Call(InfoOf(() => AsyncQueryable.ToHashSetAsync<TSource>(default(IAsyncQueryable<TSource>), default(CancellationToken))), source.Expression, Expression.Constant(cancellationToken, typeof(CancellationToken))), cancellationToken);
#else
            return source.Provider.ExecuteAsync<HashSet<TSource>>(Expression.Call(((MethodInfo)MethodBase.GetCurrentMethod()).MakeGenericMethod(typeof(TSource)), source.Expression, Expression.Constant(cancellationToken, typeof(CancellationToken))), cancellationToken);
#endif
        }

        public static Task<HashSet<TSource>> ToHashSetAsync<TSource>(this IAsyncQueryable<TSource> source, IEqualityComparer<TSource> comparer, CancellationToken cancellationToken = default)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));

#if CRIPPLED_REFLECTION
            return source.Provider.ExecuteAsync<HashSet<TSource>>(Expression.Call(InfoOf(() => AsyncQueryable.ToHashSetAsync<TSource>(default(IAsyncQueryable<TSource>), default(IEqualityComparer<TSource>), default(CancellationToken))), source.Expression, Expression.Constant(comparer, typeof(IEqualityComparer<TSource>)), Expression.Constant(cancellationToken, typeof(CancellationToken))), cancellationToken);
#else
            return source.Provider.ExecuteAsync<HashSet<TSource>>(Expression.Call(((MethodInfo)MethodBase.GetCurrentMethod()).MakeGenericMethod(typeof(TSource)), source.Expression, Expression.Constant(comparer, typeof(IEqualityComparer<TSource>)), Expression.Constant(cancellationToken, typeof(CancellationToken))), cancellationToken);
#endif
        }

        public static Task<List<TSource>> ToListAsync<TSource>(this IAsyncQueryable<TSource> source, CancellationToken cancellationToken = default)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));

#if CRIPPLED_REFLECTION
            return source.Provider.ExecuteAsync<List<TSource>>(Expression.Call(InfoOf(() => AsyncQueryable.ToListAsync<TSource>(default(IAsyncQueryable<TSource>), default(CancellationToken))), source.Expression, Expression.Constant(cancellationToken, typeof(CancellationToken))), cancellationToken);
#else
            return source.Provider.ExecuteAsync<List<TSource>>(Expression.Call(((MethodInfo)MethodBase.GetCurrentMethod()).MakeGenericMethod(typeof(TSource)), source.Expression, Expression.Constant(cancellationToken, typeof(CancellationToken))), cancellationToken);
#endif
        }

        public static Task<ILookup<TKey, TSource>> ToLookupAsync<TSource, TKey>(this IAsyncQueryable<TSource> source, Expression<Func<TSource, CancellationToken, ValueTask<TKey>>> keySelector, CancellationToken cancellationToken = default)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));
            if (keySelector == null)
                throw new ArgumentNullException(nameof(keySelector));

#if CRIPPLED_REFLECTION
            return source.Provider.ExecuteAsync<ILookup<TKey, TSource>>(Expression.Call(InfoOf(() => AsyncQueryable.ToLookupAsync<TSource, TKey>(default(IAsyncQueryable<TSource>), default(Expression<Func<TSource, CancellationToken, ValueTask<TKey>>>), default(CancellationToken))), source.Expression, keySelector, Expression.Constant(cancellationToken, typeof(CancellationToken))), cancellationToken);
#else
            return source.Provider.ExecuteAsync<ILookup<TKey, TSource>>(Expression.Call(((MethodInfo)MethodBase.GetCurrentMethod()).MakeGenericMethod(typeof(TSource), typeof(TKey)), source.Expression, keySelector, Expression.Constant(cancellationToken, typeof(CancellationToken))), cancellationToken);
#endif
        }

        public static Task<ILookup<TKey, TSource>> ToLookupAsync<TSource, TKey>(this IAsyncQueryable<TSource> source, Expression<Func<TSource, TKey>> keySelector, CancellationToken cancellationToken = default)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));
            if (keySelector == null)
                throw new ArgumentNullException(nameof(keySelector));

#if CRIPPLED_REFLECTION
            return source.Provider.ExecuteAsync<ILookup<TKey, TSource>>(Expression.Call(InfoOf(() => AsyncQueryable.ToLookupAsync<TSource, TKey>(default(IAsyncQueryable<TSource>), default(Expression<Func<TSource, TKey>>), default(CancellationToken))), source.Expression, keySelector, Expression.Constant(cancellationToken, typeof(CancellationToken))), cancellationToken);
#else
            return source.Provider.ExecuteAsync<ILookup<TKey, TSource>>(Expression.Call(((MethodInfo)MethodBase.GetCurrentMethod()).MakeGenericMethod(typeof(TSource), typeof(TKey)), source.Expression, keySelector, Expression.Constant(cancellationToken, typeof(CancellationToken))), cancellationToken);
#endif
        }

        public static Task<ILookup<TKey, TSource>> ToLookupAsync<TSource, TKey>(this IAsyncQueryable<TSource> source, Expression<Func<TSource, ValueTask<TKey>>> keySelector, CancellationToken cancellationToken = default)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));
            if (keySelector == null)
                throw new ArgumentNullException(nameof(keySelector));

#if CRIPPLED_REFLECTION
            return source.Provider.ExecuteAsync<ILookup<TKey, TSource>>(Expression.Call(InfoOf(() => AsyncQueryable.ToLookupAsync<TSource, TKey>(default(IAsyncQueryable<TSource>), default(Expression<Func<TSource, ValueTask<TKey>>>), default(CancellationToken))), source.Expression, keySelector, Expression.Constant(cancellationToken, typeof(CancellationToken))), cancellationToken);
#else
            return source.Provider.ExecuteAsync<ILookup<TKey, TSource>>(Expression.Call(((MethodInfo)MethodBase.GetCurrentMethod()).MakeGenericMethod(typeof(TSource), typeof(TKey)), source.Expression, keySelector, Expression.Constant(cancellationToken, typeof(CancellationToken))), cancellationToken);
#endif
        }

        public static Task<ILookup<TKey, TSource>> ToLookupAsync<TSource, TKey>(this IAsyncQueryable<TSource> source, Expression<Func<TSource, CancellationToken, ValueTask<TKey>>> keySelector, IEqualityComparer<TKey> comparer, CancellationToken cancellationToken = default)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));
            if (keySelector == null)
                throw new ArgumentNullException(nameof(keySelector));

#if CRIPPLED_REFLECTION
            return source.Provider.ExecuteAsync<ILookup<TKey, TSource>>(Expression.Call(InfoOf(() => AsyncQueryable.ToLookupAsync<TSource, TKey>(default(IAsyncQueryable<TSource>), default(Expression<Func<TSource, CancellationToken, ValueTask<TKey>>>), default(IEqualityComparer<TKey>), default(CancellationToken))), source.Expression, keySelector, Expression.Constant(comparer, typeof(IEqualityComparer<TKey>)), Expression.Constant(cancellationToken, typeof(CancellationToken))), cancellationToken);
#else
            return source.Provider.ExecuteAsync<ILookup<TKey, TSource>>(Expression.Call(((MethodInfo)MethodBase.GetCurrentMethod()).MakeGenericMethod(typeof(TSource), typeof(TKey)), source.Expression, keySelector, Expression.Constant(comparer, typeof(IEqualityComparer<TKey>)), Expression.Constant(cancellationToken, typeof(CancellationToken))), cancellationToken);
#endif
        }

        public static Task<ILookup<TKey, TSource>> ToLookupAsync<TSource, TKey>(this IAsyncQueryable<TSource> source, Expression<Func<TSource, TKey>> keySelector, IEqualityComparer<TKey> comparer, CancellationToken cancellationToken = default)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));
            if (keySelector == null)
                throw new ArgumentNullException(nameof(keySelector));

#if CRIPPLED_REFLECTION
            return source.Provider.ExecuteAsync<ILookup<TKey, TSource>>(Expression.Call(InfoOf(() => AsyncQueryable.ToLookupAsync<TSource, TKey>(default(IAsyncQueryable<TSource>), default(Expression<Func<TSource, TKey>>), default(IEqualityComparer<TKey>), default(CancellationToken))), source.Expression, keySelector, Expression.Constant(comparer, typeof(IEqualityComparer<TKey>)), Expression.Constant(cancellationToken, typeof(CancellationToken))), cancellationToken);
#else
            return source.Provider.ExecuteAsync<ILookup<TKey, TSource>>(Expression.Call(((MethodInfo)MethodBase.GetCurrentMethod()).MakeGenericMethod(typeof(TSource), typeof(TKey)), source.Expression, keySelector, Expression.Constant(comparer, typeof(IEqualityComparer<TKey>)), Expression.Constant(cancellationToken, typeof(CancellationToken))), cancellationToken);
#endif
        }

        public static Task<ILookup<TKey, TSource>> ToLookupAsync<TSource, TKey>(this IAsyncQueryable<TSource> source, Expression<Func<TSource, ValueTask<TKey>>> keySelector, IEqualityComparer<TKey> comparer, CancellationToken cancellationToken = default)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));
            if (keySelector == null)
                throw new ArgumentNullException(nameof(keySelector));

#if CRIPPLED_REFLECTION
            return source.Provider.ExecuteAsync<ILookup<TKey, TSource>>(Expression.Call(InfoOf(() => AsyncQueryable.ToLookupAsync<TSource, TKey>(default(IAsyncQueryable<TSource>), default(Expression<Func<TSource, ValueTask<TKey>>>), default(IEqualityComparer<TKey>), default(CancellationToken))), source.Expression, keySelector, Expression.Constant(comparer, typeof(IEqualityComparer<TKey>)), Expression.Constant(cancellationToken, typeof(CancellationToken))), cancellationToken);
#else
            return source.Provider.ExecuteAsync<ILookup<TKey, TSource>>(Expression.Call(((MethodInfo)MethodBase.GetCurrentMethod()).MakeGenericMethod(typeof(TSource), typeof(TKey)), source.Expression, keySelector, Expression.Constant(comparer, typeof(IEqualityComparer<TKey>)), Expression.Constant(cancellationToken, typeof(CancellationToken))), cancellationToken);
#endif
        }

        public static Task<ILookup<TKey, TElement>> ToLookupAsync<TSource, TKey, TElement>(this IAsyncQueryable<TSource> source, Expression<Func<TSource, CancellationToken, ValueTask<TKey>>> keySelector, Expression<Func<TSource, CancellationToken, ValueTask<TElement>>> elementSelector, CancellationToken cancellationToken = default)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));
            if (keySelector == null)
                throw new ArgumentNullException(nameof(keySelector));
            if (elementSelector == null)
                throw new ArgumentNullException(nameof(elementSelector));

#if CRIPPLED_REFLECTION
            return source.Provider.ExecuteAsync<ILookup<TKey, TElement>>(Expression.Call(InfoOf(() => AsyncQueryable.ToLookupAsync<TSource, TKey, TElement>(default(IAsyncQueryable<TSource>), default(Expression<Func<TSource, CancellationToken, ValueTask<TKey>>>), default(Expression<Func<TSource, CancellationToken, ValueTask<TElement>>>), default(CancellationToken))), source.Expression, keySelector, elementSelector, Expression.Constant(cancellationToken, typeof(CancellationToken))), cancellationToken);
#else
            return source.Provider.ExecuteAsync<ILookup<TKey, TElement>>(Expression.Call(((MethodInfo)MethodBase.GetCurrentMethod()).MakeGenericMethod(typeof(TSource), typeof(TKey), typeof(TElement)), source.Expression, keySelector, elementSelector, Expression.Constant(cancellationToken, typeof(CancellationToken))), cancellationToken);
#endif
        }

        public static Task<ILookup<TKey, TElement>> ToLookupAsync<TSource, TKey, TElement>(this IAsyncQueryable<TSource> source, Expression<Func<TSource, TKey>> keySelector, Expression<Func<TSource, TElement>> elementSelector, CancellationToken cancellationToken = default)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));
            if (keySelector == null)
                throw new ArgumentNullException(nameof(keySelector));
            if (elementSelector == null)
                throw new ArgumentNullException(nameof(elementSelector));

#if CRIPPLED_REFLECTION
            return source.Provider.ExecuteAsync<ILookup<TKey, TElement>>(Expression.Call(InfoOf(() => AsyncQueryable.ToLookupAsync<TSource, TKey, TElement>(default(IAsyncQueryable<TSource>), default(Expression<Func<TSource, TKey>>), default(Expression<Func<TSource, TElement>>), default(CancellationToken))), source.Expression, keySelector, elementSelector, Expression.Constant(cancellationToken, typeof(CancellationToken))), cancellationToken);
#else
            return source.Provider.ExecuteAsync<ILookup<TKey, TElement>>(Expression.Call(((MethodInfo)MethodBase.GetCurrentMethod()).MakeGenericMethod(typeof(TSource), typeof(TKey), typeof(TElement)), source.Expression, keySelector, elementSelector, Expression.Constant(cancellationToken, typeof(CancellationToken))), cancellationToken);
#endif
        }

        public static Task<ILookup<TKey, TElement>> ToLookupAsync<TSource, TKey, TElement>(this IAsyncQueryable<TSource> source, Expression<Func<TSource, ValueTask<TKey>>> keySelector, Expression<Func<TSource, ValueTask<TElement>>> elementSelector, CancellationToken cancellationToken = default)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));
            if (keySelector == null)
                throw new ArgumentNullException(nameof(keySelector));
            if (elementSelector == null)
                throw new ArgumentNullException(nameof(elementSelector));

#if CRIPPLED_REFLECTION
            return source.Provider.ExecuteAsync<ILookup<TKey, TElement>>(Expression.Call(InfoOf(() => AsyncQueryable.ToLookupAsync<TSource, TKey, TElement>(default(IAsyncQueryable<TSource>), default(Expression<Func<TSource, ValueTask<TKey>>>), default(Expression<Func<TSource, ValueTask<TElement>>>), default(CancellationToken))), source.Expression, keySelector, elementSelector, Expression.Constant(cancellationToken, typeof(CancellationToken))), cancellationToken);
#else
            return source.Provider.ExecuteAsync<ILookup<TKey, TElement>>(Expression.Call(((MethodInfo)MethodBase.GetCurrentMethod()).MakeGenericMethod(typeof(TSource), typeof(TKey), typeof(TElement)), source.Expression, keySelector, elementSelector, Expression.Constant(cancellationToken, typeof(CancellationToken))), cancellationToken);
#endif
        }

        public static Task<ILookup<TKey, TElement>> ToLookupAsync<TSource, TKey, TElement>(this IAsyncQueryable<TSource> source, Expression<Func<TSource, CancellationToken, ValueTask<TKey>>> keySelector, Expression<Func<TSource, CancellationToken, ValueTask<TElement>>> elementSelector, IEqualityComparer<TKey> comparer, CancellationToken cancellationToken = default)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));
            if (keySelector == null)
                throw new ArgumentNullException(nameof(keySelector));
            if (elementSelector == null)
                throw new ArgumentNullException(nameof(elementSelector));

#if CRIPPLED_REFLECTION
            return source.Provider.ExecuteAsync<ILookup<TKey, TElement>>(Expression.Call(InfoOf(() => AsyncQueryable.ToLookupAsync<TSource, TKey, TElement>(default(IAsyncQueryable<TSource>), default(Expression<Func<TSource, CancellationToken, ValueTask<TKey>>>), default(Expression<Func<TSource, CancellationToken, ValueTask<TElement>>>), default(IEqualityComparer<TKey>), default(CancellationToken))), source.Expression, keySelector, elementSelector, Expression.Constant(comparer, typeof(IEqualityComparer<TKey>)), Expression.Constant(cancellationToken, typeof(CancellationToken))), cancellationToken);
#else
            return source.Provider.ExecuteAsync<ILookup<TKey, TElement>>(Expression.Call(((MethodInfo)MethodBase.GetCurrentMethod()).MakeGenericMethod(typeof(TSource), typeof(TKey), typeof(TElement)), source.Expression, keySelector, elementSelector, Expression.Constant(comparer, typeof(IEqualityComparer<TKey>)), Expression.Constant(cancellationToken, typeof(CancellationToken))), cancellationToken);
#endif
        }

        public static Task<ILookup<TKey, TElement>> ToLookupAsync<TSource, TKey, TElement>(this IAsyncQueryable<TSource> source, Expression<Func<TSource, TKey>> keySelector, Expression<Func<TSource, TElement>> elementSelector, IEqualityComparer<TKey> comparer, CancellationToken cancellationToken = default)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));
            if (keySelector == null)
                throw new ArgumentNullException(nameof(keySelector));
            if (elementSelector == null)
                throw new ArgumentNullException(nameof(elementSelector));

#if CRIPPLED_REFLECTION
            return source.Provider.ExecuteAsync<ILookup<TKey, TElement>>(Expression.Call(InfoOf(() => AsyncQueryable.ToLookupAsync<TSource, TKey, TElement>(default(IAsyncQueryable<TSource>), default(Expression<Func<TSource, TKey>>), default(Expression<Func<TSource, TElement>>), default(IEqualityComparer<TKey>), default(CancellationToken))), source.Expression, keySelector, elementSelector, Expression.Constant(comparer, typeof(IEqualityComparer<TKey>)), Expression.Constant(cancellationToken, typeof(CancellationToken))), cancellationToken);
#else
            return source.Provider.ExecuteAsync<ILookup<TKey, TElement>>(Expression.Call(((MethodInfo)MethodBase.GetCurrentMethod()).MakeGenericMethod(typeof(TSource), typeof(TKey), typeof(TElement)), source.Expression, keySelector, elementSelector, Expression.Constant(comparer, typeof(IEqualityComparer<TKey>)), Expression.Constant(cancellationToken, typeof(CancellationToken))), cancellationToken);
#endif
        }

        public static Task<ILookup<TKey, TElement>> ToLookupAsync<TSource, TKey, TElement>(this IAsyncQueryable<TSource> source, Expression<Func<TSource, ValueTask<TKey>>> keySelector, Expression<Func<TSource, ValueTask<TElement>>> elementSelector, IEqualityComparer<TKey> comparer, CancellationToken cancellationToken = default)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));
            if (keySelector == null)
                throw new ArgumentNullException(nameof(keySelector));
            if (elementSelector == null)
                throw new ArgumentNullException(nameof(elementSelector));

#if CRIPPLED_REFLECTION
            return source.Provider.ExecuteAsync<ILookup<TKey, TElement>>(Expression.Call(InfoOf(() => AsyncQueryable.ToLookupAsync<TSource, TKey, TElement>(default(IAsyncQueryable<TSource>), default(Expression<Func<TSource, ValueTask<TKey>>>), default(Expression<Func<TSource, ValueTask<TElement>>>), default(IEqualityComparer<TKey>), default(CancellationToken))), source.Expression, keySelector, elementSelector, Expression.Constant(comparer, typeof(IEqualityComparer<TKey>)), Expression.Constant(cancellationToken, typeof(CancellationToken))), cancellationToken);
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

        public static IAsyncQueryable<TSource> Where<TSource>(this IAsyncQueryable<TSource> source, Expression<Func<TSource, CancellationToken, ValueTask<bool>>> predicate)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));
            if (predicate == null)
                throw new ArgumentNullException(nameof(predicate));

#if CRIPPLED_REFLECTION
            return source.Provider.CreateQuery<TSource>(Expression.Call(InfoOf(() => AsyncQueryable.Where<TSource>(default(IAsyncQueryable<TSource>), default(Expression<Func<TSource, CancellationToken, ValueTask<bool>>>))), source.Expression, predicate));
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

        public static IAsyncQueryable<TSource> Where<TSource>(this IAsyncQueryable<TSource> source, Expression<Func<TSource, int, CancellationToken, ValueTask<bool>>> predicate)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));
            if (predicate == null)
                throw new ArgumentNullException(nameof(predicate));

#if CRIPPLED_REFLECTION
            return source.Provider.CreateQuery<TSource>(Expression.Call(InfoOf(() => AsyncQueryable.Where<TSource>(default(IAsyncQueryable<TSource>), default(Expression<Func<TSource, int, CancellationToken, ValueTask<bool>>>))), source.Expression, predicate));
#else
            return source.Provider.CreateQuery<TSource>(Expression.Call(((MethodInfo)MethodBase.GetCurrentMethod()).MakeGenericMethod(typeof(TSource)), source.Expression, predicate));
#endif
        }

        public static IAsyncQueryable<TSource> Where<TSource>(this IAsyncQueryable<TSource> source, Expression<Func<TSource, int, ValueTask<bool>>> predicate)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));
            if (predicate == null)
                throw new ArgumentNullException(nameof(predicate));

#if CRIPPLED_REFLECTION
            return source.Provider.CreateQuery<TSource>(Expression.Call(InfoOf(() => AsyncQueryable.Where<TSource>(default(IAsyncQueryable<TSource>), default(Expression<Func<TSource, int, ValueTask<bool>>>))), source.Expression, predicate));
#else
            return source.Provider.CreateQuery<TSource>(Expression.Call(((MethodInfo)MethodBase.GetCurrentMethod()).MakeGenericMethod(typeof(TSource)), source.Expression, predicate));
#endif
        }

        public static IAsyncQueryable<TSource> Where<TSource>(this IAsyncQueryable<TSource> source, Expression<Func<TSource, ValueTask<bool>>> predicate)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));
            if (predicate == null)
                throw new ArgumentNullException(nameof(predicate));

#if CRIPPLED_REFLECTION
            return source.Provider.CreateQuery<TSource>(Expression.Call(InfoOf(() => AsyncQueryable.Where<TSource>(default(IAsyncQueryable<TSource>), default(Expression<Func<TSource, ValueTask<bool>>>))), source.Expression, predicate));
#else
            return source.Provider.CreateQuery<TSource>(Expression.Call(((MethodInfo)MethodBase.GetCurrentMethod()).MakeGenericMethod(typeof(TSource)), source.Expression, predicate));
#endif
        }

        public static IAsyncQueryable<TResult> Zip<TFirst, TSecond, TResult>(this IAsyncQueryable<TFirst> first, IAsyncEnumerable<TSecond> second, Expression<Func<TFirst, TSecond, CancellationToken, ValueTask<TResult>>> selector)
        {
            if (first == null)
                throw new ArgumentNullException(nameof(first));
            if (second == null)
                throw new ArgumentNullException(nameof(second));
            if (selector == null)
                throw new ArgumentNullException(nameof(selector));

#if CRIPPLED_REFLECTION
            return first.Provider.CreateQuery<TResult>(Expression.Call(InfoOf(() => AsyncQueryable.Zip<TFirst, TSecond, TResult>(default(IAsyncQueryable<TFirst>), default(IAsyncEnumerable<TSecond>), default(Expression<Func<TFirst, TSecond, CancellationToken, ValueTask<TResult>>>))), first.Expression, GetSourceExpression(second), selector));
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

        public static IAsyncQueryable<TResult> Zip<TFirst, TSecond, TResult>(this IAsyncQueryable<TFirst> first, IAsyncEnumerable<TSecond> second, Expression<Func<TFirst, TSecond, ValueTask<TResult>>> selector)
        {
            if (first == null)
                throw new ArgumentNullException(nameof(first));
            if (second == null)
                throw new ArgumentNullException(nameof(second));
            if (selector == null)
                throw new ArgumentNullException(nameof(selector));

#if CRIPPLED_REFLECTION
            return first.Provider.CreateQuery<TResult>(Expression.Call(InfoOf(() => AsyncQueryable.Zip<TFirst, TSecond, TResult>(default(IAsyncQueryable<TFirst>), default(IAsyncEnumerable<TSecond>), default(Expression<Func<TFirst, TSecond, ValueTask<TResult>>>))), first.Expression, GetSourceExpression(second), selector));
#else
            return first.Provider.CreateQuery<TResult>(Expression.Call(((MethodInfo)MethodBase.GetCurrentMethod()).MakeGenericMethod(typeof(TFirst), typeof(TSecond), typeof(TResult)), first.Expression, GetSourceExpression(second), selector));
#endif
        }

    }
}
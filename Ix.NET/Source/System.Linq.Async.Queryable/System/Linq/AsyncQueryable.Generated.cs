// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the Apache 2.0 License.
// See the LICENSE file in the project root for more information. 

#nullable enable

using System.Collections.Generic;
using System.Linq.Expressions;
using System.Reflection;
using System.Threading;
using System.Threading.Tasks;

namespace System.Linq
{
    public static partial class AsyncQueryable
    {
        private static MethodInfo? s_AggregateAsync__TSource__3__0;
        
        private static MethodInfo? AggregateAsync__TSource__3__0(Type TSource) =>
            (s_AggregateAsync__TSource__3__0 ??
            (s_AggregateAsync__TSource__3__0 = new Func<IAsyncQueryable<object>, Expression<Func<object, object, object>>, CancellationToken, ValueTask<object>>(AggregateAsync<object>).GetMethodInfo()!.GetGenericMethodDefinition())).MakeGenericMethod(TSource);

        public static ValueTask<TSource> AggregateAsync<TSource>(this IAsyncQueryable<TSource> source, Expression<Func<TSource, TSource, TSource>> accumulator, CancellationToken cancellationToken = default)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));
            if (accumulator == null)
                throw new ArgumentNullException(nameof(accumulator));

            return source.Provider.ExecuteAsync<TSource>(Expression.Call(AggregateAsync__TSource__3__0(typeof(TSource)), source.Expression, accumulator, Expression.Constant(cancellationToken, typeof(CancellationToken))), cancellationToken);
        }

        private static MethodInfo? s_AggregateAsync__TSource_TAccumulate__4__0;
        
        private static MethodInfo? AggregateAsync__TSource_TAccumulate__4__0(Type TSource, Type TAccumulate) =>
            (s_AggregateAsync__TSource_TAccumulate__4__0 ??
            (s_AggregateAsync__TSource_TAccumulate__4__0 = new Func<IAsyncQueryable<object>, object, Expression<Func<object, object, object>>, CancellationToken, ValueTask<object>>(AggregateAsync<object, object>).GetMethodInfo()!.GetGenericMethodDefinition())).MakeGenericMethod(TSource, TAccumulate);

        public static ValueTask<TAccumulate> AggregateAsync<TSource, TAccumulate>(this IAsyncQueryable<TSource> source, TAccumulate seed, Expression<Func<TAccumulate, TSource, TAccumulate>> accumulator, CancellationToken cancellationToken = default)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));
            if (accumulator == null)
                throw new ArgumentNullException(nameof(accumulator));

            return source.Provider.ExecuteAsync<TAccumulate>(Expression.Call(AggregateAsync__TSource_TAccumulate__4__0(typeof(TSource), typeof(TAccumulate)), source.Expression, Expression.Constant(seed, typeof(TAccumulate)), accumulator, Expression.Constant(cancellationToken, typeof(CancellationToken))), cancellationToken);
        }

        private static MethodInfo? s_AggregateAsync__TSource_TAccumulate_TResult__5__0;
        
        private static MethodInfo? AggregateAsync__TSource_TAccumulate_TResult__5__0(Type TSource, Type TAccumulate, Type TResult) =>
            (s_AggregateAsync__TSource_TAccumulate_TResult__5__0 ??
            (s_AggregateAsync__TSource_TAccumulate_TResult__5__0 = new Func<IAsyncQueryable<object>, object, Expression<Func<object, object, object>>, Expression<Func<object, object>>, CancellationToken, ValueTask<object>>(AggregateAsync<object, object, object>).GetMethodInfo()!.GetGenericMethodDefinition())).MakeGenericMethod(TSource, TAccumulate, TResult);

        public static ValueTask<TResult> AggregateAsync<TSource, TAccumulate, TResult>(this IAsyncQueryable<TSource> source, TAccumulate seed, Expression<Func<TAccumulate, TSource, TAccumulate>> accumulator, Expression<Func<TAccumulate, TResult>> resultSelector, CancellationToken cancellationToken = default)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));
            if (accumulator == null)
                throw new ArgumentNullException(nameof(accumulator));
            if (resultSelector == null)
                throw new ArgumentNullException(nameof(resultSelector));

            return source.Provider.ExecuteAsync<TResult>(Expression.Call(AggregateAsync__TSource_TAccumulate_TResult__5__0(typeof(TSource), typeof(TAccumulate), typeof(TResult)), source.Expression, Expression.Constant(seed, typeof(TAccumulate)), accumulator, resultSelector, Expression.Constant(cancellationToken, typeof(CancellationToken))), cancellationToken);
        }

        private static MethodInfo? s_AggregateAwaitAsync__TSource__3__0;
        
        private static MethodInfo? AggregateAwaitAsync__TSource__3__0(Type TSource) =>
            (s_AggregateAwaitAsync__TSource__3__0 ??
            (s_AggregateAwaitAsync__TSource__3__0 = new Func<IAsyncQueryable<object>, Expression<Func<object, object, ValueTask<object>>>, CancellationToken, ValueTask<object>>(AggregateAwaitAsync<object>).GetMethodInfo()!.GetGenericMethodDefinition())).MakeGenericMethod(TSource);

        public static ValueTask<TSource> AggregateAwaitAsync<TSource>(this IAsyncQueryable<TSource> source, Expression<Func<TSource, TSource, ValueTask<TSource>>> accumulator, CancellationToken cancellationToken = default)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));
            if (accumulator == null)
                throw new ArgumentNullException(nameof(accumulator));

            return source.Provider.ExecuteAsync<TSource>(Expression.Call(AggregateAwaitAsync__TSource__3__0(typeof(TSource)), source.Expression, accumulator, Expression.Constant(cancellationToken, typeof(CancellationToken))), cancellationToken);
        }

        private static MethodInfo? s_AggregateAwaitAsync__TSource_TAccumulate__4__0;
        
        private static MethodInfo? AggregateAwaitAsync__TSource_TAccumulate__4__0(Type TSource, Type TAccumulate) =>
            (s_AggregateAwaitAsync__TSource_TAccumulate__4__0 ??
            (s_AggregateAwaitAsync__TSource_TAccumulate__4__0 = new Func<IAsyncQueryable<object>, object, Expression<Func<object, object, ValueTask<object>>>, CancellationToken, ValueTask<object>>(AggregateAwaitAsync<object, object>).GetMethodInfo()!.GetGenericMethodDefinition())).MakeGenericMethod(TSource, TAccumulate);

        public static ValueTask<TAccumulate> AggregateAwaitAsync<TSource, TAccumulate>(this IAsyncQueryable<TSource> source, TAccumulate seed, Expression<Func<TAccumulate, TSource, ValueTask<TAccumulate>>> accumulator, CancellationToken cancellationToken = default)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));
            if (accumulator == null)
                throw new ArgumentNullException(nameof(accumulator));

            return source.Provider.ExecuteAsync<TAccumulate>(Expression.Call(AggregateAwaitAsync__TSource_TAccumulate__4__0(typeof(TSource), typeof(TAccumulate)), source.Expression, Expression.Constant(seed, typeof(TAccumulate)), accumulator, Expression.Constant(cancellationToken, typeof(CancellationToken))), cancellationToken);
        }

        private static MethodInfo? s_AggregateAwaitAsync__TSource_TAccumulate_TResult__5__0;
        
        private static MethodInfo? AggregateAwaitAsync__TSource_TAccumulate_TResult__5__0(Type TSource, Type TAccumulate, Type TResult) =>
            (s_AggregateAwaitAsync__TSource_TAccumulate_TResult__5__0 ??
            (s_AggregateAwaitAsync__TSource_TAccumulate_TResult__5__0 = new Func<IAsyncQueryable<object>, object, Expression<Func<object, object, ValueTask<object>>>, Expression<Func<object, ValueTask<object>>>, CancellationToken, ValueTask<object>>(AggregateAwaitAsync<object, object, object>).GetMethodInfo()!.GetGenericMethodDefinition())).MakeGenericMethod(TSource, TAccumulate, TResult);

        public static ValueTask<TResult> AggregateAwaitAsync<TSource, TAccumulate, TResult>(this IAsyncQueryable<TSource> source, TAccumulate seed, Expression<Func<TAccumulate, TSource, ValueTask<TAccumulate>>> accumulator, Expression<Func<TAccumulate, ValueTask<TResult>>> resultSelector, CancellationToken cancellationToken = default)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));
            if (accumulator == null)
                throw new ArgumentNullException(nameof(accumulator));
            if (resultSelector == null)
                throw new ArgumentNullException(nameof(resultSelector));

            return source.Provider.ExecuteAsync<TResult>(Expression.Call(AggregateAwaitAsync__TSource_TAccumulate_TResult__5__0(typeof(TSource), typeof(TAccumulate), typeof(TResult)), source.Expression, Expression.Constant(seed, typeof(TAccumulate)), accumulator, resultSelector, Expression.Constant(cancellationToken, typeof(CancellationToken))), cancellationToken);
        }

        private static MethodInfo? s_AggregateAwaitWithCancellationAsync__TSource__3__0;
        
        private static MethodInfo? AggregateAwaitWithCancellationAsync__TSource__3__0(Type TSource) =>
            (s_AggregateAwaitWithCancellationAsync__TSource__3__0 ??
            (s_AggregateAwaitWithCancellationAsync__TSource__3__0 = new Func<IAsyncQueryable<object>, Expression<Func<object, object, CancellationToken, ValueTask<object>>>, CancellationToken, ValueTask<object>>(AggregateAwaitWithCancellationAsync<object>).GetMethodInfo()!.GetGenericMethodDefinition())).MakeGenericMethod(TSource);

        public static ValueTask<TSource> AggregateAwaitWithCancellationAsync<TSource>(this IAsyncQueryable<TSource> source, Expression<Func<TSource, TSource, CancellationToken, ValueTask<TSource>>> accumulator, CancellationToken cancellationToken = default)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));
            if (accumulator == null)
                throw new ArgumentNullException(nameof(accumulator));

            return source.Provider.ExecuteAsync<TSource>(Expression.Call(AggregateAwaitWithCancellationAsync__TSource__3__0(typeof(TSource)), source.Expression, accumulator, Expression.Constant(cancellationToken, typeof(CancellationToken))), cancellationToken);
        }

        private static MethodInfo? s_AggregateAwaitWithCancellationAsync__TSource_TAccumulate__4__0;
        
        private static MethodInfo? AggregateAwaitWithCancellationAsync__TSource_TAccumulate__4__0(Type TSource, Type TAccumulate) =>
            (s_AggregateAwaitWithCancellationAsync__TSource_TAccumulate__4__0 ??
            (s_AggregateAwaitWithCancellationAsync__TSource_TAccumulate__4__0 = new Func<IAsyncQueryable<object>, object, Expression<Func<object, object, CancellationToken, ValueTask<object>>>, CancellationToken, ValueTask<object>>(AggregateAwaitWithCancellationAsync<object, object>).GetMethodInfo()!.GetGenericMethodDefinition())).MakeGenericMethod(TSource, TAccumulate);

        public static ValueTask<TAccumulate> AggregateAwaitWithCancellationAsync<TSource, TAccumulate>(this IAsyncQueryable<TSource> source, TAccumulate seed, Expression<Func<TAccumulate, TSource, CancellationToken, ValueTask<TAccumulate>>> accumulator, CancellationToken cancellationToken = default)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));
            if (accumulator == null)
                throw new ArgumentNullException(nameof(accumulator));

            return source.Provider.ExecuteAsync<TAccumulate>(Expression.Call(AggregateAwaitWithCancellationAsync__TSource_TAccumulate__4__0(typeof(TSource), typeof(TAccumulate)), source.Expression, Expression.Constant(seed, typeof(TAccumulate)), accumulator, Expression.Constant(cancellationToken, typeof(CancellationToken))), cancellationToken);
        }

        private static MethodInfo? s_AggregateAwaitWithCancellationAsync__TSource_TAccumulate_TResult__5__0;
        
        private static MethodInfo? AggregateAwaitWithCancellationAsync__TSource_TAccumulate_TResult__5__0(Type TSource, Type TAccumulate, Type TResult) =>
            (s_AggregateAwaitWithCancellationAsync__TSource_TAccumulate_TResult__5__0 ??
            (s_AggregateAwaitWithCancellationAsync__TSource_TAccumulate_TResult__5__0 = new Func<IAsyncQueryable<object>, object, Expression<Func<object, object, CancellationToken, ValueTask<object>>>, Expression<Func<object, CancellationToken, ValueTask<object>>>, CancellationToken, ValueTask<object>>(AggregateAwaitWithCancellationAsync<object, object, object>).GetMethodInfo()!.GetGenericMethodDefinition())).MakeGenericMethod(TSource, TAccumulate, TResult);

        public static ValueTask<TResult> AggregateAwaitWithCancellationAsync<TSource, TAccumulate, TResult>(this IAsyncQueryable<TSource> source, TAccumulate seed, Expression<Func<TAccumulate, TSource, CancellationToken, ValueTask<TAccumulate>>> accumulator, Expression<Func<TAccumulate, CancellationToken, ValueTask<TResult>>> resultSelector, CancellationToken cancellationToken = default)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));
            if (accumulator == null)
                throw new ArgumentNullException(nameof(accumulator));
            if (resultSelector == null)
                throw new ArgumentNullException(nameof(resultSelector));

            return source.Provider.ExecuteAsync<TResult>(Expression.Call(AggregateAwaitWithCancellationAsync__TSource_TAccumulate_TResult__5__0(typeof(TSource), typeof(TAccumulate), typeof(TResult)), source.Expression, Expression.Constant(seed, typeof(TAccumulate)), accumulator, resultSelector, Expression.Constant(cancellationToken, typeof(CancellationToken))), cancellationToken);
        }

        private static MethodInfo? s_AllAsync__TSource__3__0;
        
        private static MethodInfo? AllAsync__TSource__3__0(Type TSource) =>
            (s_AllAsync__TSource__3__0 ??
            (s_AllAsync__TSource__3__0 = new Func<IAsyncQueryable<object>, Expression<Func<object, bool>>, CancellationToken, ValueTask<bool>>(AllAsync<object>).GetMethodInfo()!.GetGenericMethodDefinition())).MakeGenericMethod(TSource);

        public static ValueTask<bool> AllAsync<TSource>(this IAsyncQueryable<TSource> source, Expression<Func<TSource, bool>> predicate, CancellationToken cancellationToken = default)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));
            if (predicate == null)
                throw new ArgumentNullException(nameof(predicate));

            return source.Provider.ExecuteAsync<bool>(Expression.Call(AllAsync__TSource__3__0(typeof(TSource)), source.Expression, predicate, Expression.Constant(cancellationToken, typeof(CancellationToken))), cancellationToken);
        }

        private static MethodInfo? s_AllAwaitAsync__TSource__3__0;
        
        private static MethodInfo? AllAwaitAsync__TSource__3__0(Type TSource) =>
            (s_AllAwaitAsync__TSource__3__0 ??
            (s_AllAwaitAsync__TSource__3__0 = new Func<IAsyncQueryable<object>, Expression<Func<object, ValueTask<bool>>>, CancellationToken, ValueTask<bool>>(AllAwaitAsync<object>).GetMethodInfo()!.GetGenericMethodDefinition())).MakeGenericMethod(TSource);

        public static ValueTask<bool> AllAwaitAsync<TSource>(this IAsyncQueryable<TSource> source, Expression<Func<TSource, ValueTask<bool>>> predicate, CancellationToken cancellationToken = default)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));
            if (predicate == null)
                throw new ArgumentNullException(nameof(predicate));

            return source.Provider.ExecuteAsync<bool>(Expression.Call(AllAwaitAsync__TSource__3__0(typeof(TSource)), source.Expression, predicate, Expression.Constant(cancellationToken, typeof(CancellationToken))), cancellationToken);
        }

        private static MethodInfo? s_AllAwaitWithCancellationAsync__TSource__3__0;
        
        private static MethodInfo? AllAwaitWithCancellationAsync__TSource__3__0(Type TSource) =>
            (s_AllAwaitWithCancellationAsync__TSource__3__0 ??
            (s_AllAwaitWithCancellationAsync__TSource__3__0 = new Func<IAsyncQueryable<object>, Expression<Func<object, CancellationToken, ValueTask<bool>>>, CancellationToken, ValueTask<bool>>(AllAwaitWithCancellationAsync<object>).GetMethodInfo()!.GetGenericMethodDefinition())).MakeGenericMethod(TSource);

        public static ValueTask<bool> AllAwaitWithCancellationAsync<TSource>(this IAsyncQueryable<TSource> source, Expression<Func<TSource, CancellationToken, ValueTask<bool>>> predicate, CancellationToken cancellationToken = default)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));
            if (predicate == null)
                throw new ArgumentNullException(nameof(predicate));

            return source.Provider.ExecuteAsync<bool>(Expression.Call(AllAwaitWithCancellationAsync__TSource__3__0(typeof(TSource)), source.Expression, predicate, Expression.Constant(cancellationToken, typeof(CancellationToken))), cancellationToken);
        }

        private static MethodInfo? s_AnyAsync__TSource__2__0;
        
        private static MethodInfo? AnyAsync__TSource__2__0(Type TSource) =>
            (s_AnyAsync__TSource__2__0 ??
            (s_AnyAsync__TSource__2__0 = new Func<IAsyncQueryable<object>, CancellationToken, ValueTask<bool>>(AnyAsync<object>).GetMethodInfo()!.GetGenericMethodDefinition())).MakeGenericMethod(TSource);

        public static ValueTask<bool> AnyAsync<TSource>(this IAsyncQueryable<TSource> source, CancellationToken cancellationToken = default)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));

            return source.Provider.ExecuteAsync<bool>(Expression.Call(AnyAsync__TSource__2__0(typeof(TSource)), source.Expression, Expression.Constant(cancellationToken, typeof(CancellationToken))), cancellationToken);
        }

        private static MethodInfo? s_AnyAsync__TSource__3__0;
        
        private static MethodInfo? AnyAsync__TSource__3__0(Type TSource) =>
            (s_AnyAsync__TSource__3__0 ??
            (s_AnyAsync__TSource__3__0 = new Func<IAsyncQueryable<object>, Expression<Func<object, bool>>, CancellationToken, ValueTask<bool>>(AnyAsync<object>).GetMethodInfo()!.GetGenericMethodDefinition())).MakeGenericMethod(TSource);

        public static ValueTask<bool> AnyAsync<TSource>(this IAsyncQueryable<TSource> source, Expression<Func<TSource, bool>> predicate, CancellationToken cancellationToken = default)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));
            if (predicate == null)
                throw new ArgumentNullException(nameof(predicate));

            return source.Provider.ExecuteAsync<bool>(Expression.Call(AnyAsync__TSource__3__0(typeof(TSource)), source.Expression, predicate, Expression.Constant(cancellationToken, typeof(CancellationToken))), cancellationToken);
        }

        private static MethodInfo? s_AnyAwaitAsync__TSource__3__0;
        
        private static MethodInfo? AnyAwaitAsync__TSource__3__0(Type TSource) =>
            (s_AnyAwaitAsync__TSource__3__0 ??
            (s_AnyAwaitAsync__TSource__3__0 = new Func<IAsyncQueryable<object>, Expression<Func<object, ValueTask<bool>>>, CancellationToken, ValueTask<bool>>(AnyAwaitAsync<object>).GetMethodInfo()!.GetGenericMethodDefinition())).MakeGenericMethod(TSource);

        public static ValueTask<bool> AnyAwaitAsync<TSource>(this IAsyncQueryable<TSource> source, Expression<Func<TSource, ValueTask<bool>>> predicate, CancellationToken cancellationToken = default)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));
            if (predicate == null)
                throw new ArgumentNullException(nameof(predicate));

            return source.Provider.ExecuteAsync<bool>(Expression.Call(AnyAwaitAsync__TSource__3__0(typeof(TSource)), source.Expression, predicate, Expression.Constant(cancellationToken, typeof(CancellationToken))), cancellationToken);
        }

        private static MethodInfo? s_AnyAwaitWithCancellationAsync__TSource__3__0;
        
        private static MethodInfo? AnyAwaitWithCancellationAsync__TSource__3__0(Type TSource) =>
            (s_AnyAwaitWithCancellationAsync__TSource__3__0 ??
            (s_AnyAwaitWithCancellationAsync__TSource__3__0 = new Func<IAsyncQueryable<object>, Expression<Func<object, CancellationToken, ValueTask<bool>>>, CancellationToken, ValueTask<bool>>(AnyAwaitWithCancellationAsync<object>).GetMethodInfo()!.GetGenericMethodDefinition())).MakeGenericMethod(TSource);

        public static ValueTask<bool> AnyAwaitWithCancellationAsync<TSource>(this IAsyncQueryable<TSource> source, Expression<Func<TSource, CancellationToken, ValueTask<bool>>> predicate, CancellationToken cancellationToken = default)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));
            if (predicate == null)
                throw new ArgumentNullException(nameof(predicate));

            return source.Provider.ExecuteAsync<bool>(Expression.Call(AnyAwaitWithCancellationAsync__TSource__3__0(typeof(TSource)), source.Expression, predicate, Expression.Constant(cancellationToken, typeof(CancellationToken))), cancellationToken);
        }

        private static MethodInfo? s_Append__TSource__2__0;
        
        private static MethodInfo? Append__TSource__2__0(Type TSource) =>
            (s_Append__TSource__2__0 ??
            (s_Append__TSource__2__0 = new Func<IAsyncQueryable<object>, object, IAsyncQueryable<object>>(Append<object>).GetMethodInfo()!.GetGenericMethodDefinition())).MakeGenericMethod(TSource);

        public static IAsyncQueryable<TSource> Append<TSource>(this IAsyncQueryable<TSource> source, TSource element)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));

            return source.Provider.CreateQuery<TSource>(Expression.Call(Append__TSource__2__0(typeof(TSource)), source.Expression, Expression.Constant(element, typeof(TSource))));
        }

        private static MethodInfo? s_AverageAsync__2__0;
        
        private static MethodInfo? AverageAsync__2__0 =>
            (s_AverageAsync__2__0 ??
            (s_AverageAsync__2__0 = new Func<IAsyncQueryable<decimal?>, CancellationToken, ValueTask<decimal?>>(AverageAsync).GetMethodInfo()));

        public static ValueTask<decimal?> AverageAsync(this IAsyncQueryable<decimal?> source, CancellationToken cancellationToken = default)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));

            return source.Provider.ExecuteAsync<decimal?>(Expression.Call(AverageAsync__2__0, source.Expression, Expression.Constant(cancellationToken, typeof(CancellationToken))), cancellationToken);
        }

        private static MethodInfo? s_AverageAsync__2__1;
        
        private static MethodInfo? AverageAsync__2__1 =>
            (s_AverageAsync__2__1 ??
            (s_AverageAsync__2__1 = new Func<IAsyncQueryable<decimal>, CancellationToken, ValueTask<decimal>>(AverageAsync).GetMethodInfo()));

        public static ValueTask<decimal> AverageAsync(this IAsyncQueryable<decimal> source, CancellationToken cancellationToken = default)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));

            return source.Provider.ExecuteAsync<decimal>(Expression.Call(AverageAsync__2__1, source.Expression, Expression.Constant(cancellationToken, typeof(CancellationToken))), cancellationToken);
        }

        private static MethodInfo? s_AverageAsync__2__2;
        
        private static MethodInfo? AverageAsync__2__2 =>
            (s_AverageAsync__2__2 ??
            (s_AverageAsync__2__2 = new Func<IAsyncQueryable<double?>, CancellationToken, ValueTask<double?>>(AverageAsync).GetMethodInfo()));

        public static ValueTask<double?> AverageAsync(this IAsyncQueryable<double?> source, CancellationToken cancellationToken = default)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));

            return source.Provider.ExecuteAsync<double?>(Expression.Call(AverageAsync__2__2, source.Expression, Expression.Constant(cancellationToken, typeof(CancellationToken))), cancellationToken);
        }

        private static MethodInfo? s_AverageAsync__2__3;
        
        private static MethodInfo? AverageAsync__2__3 =>
            (s_AverageAsync__2__3 ??
            (s_AverageAsync__2__3 = new Func<IAsyncQueryable<double>, CancellationToken, ValueTask<double>>(AverageAsync).GetMethodInfo()));

        public static ValueTask<double> AverageAsync(this IAsyncQueryable<double> source, CancellationToken cancellationToken = default)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));

            return source.Provider.ExecuteAsync<double>(Expression.Call(AverageAsync__2__3, source.Expression, Expression.Constant(cancellationToken, typeof(CancellationToken))), cancellationToken);
        }

        private static MethodInfo? s_AverageAsync__2__4;
        
        private static MethodInfo? AverageAsync__2__4 =>
            (s_AverageAsync__2__4 ??
            (s_AverageAsync__2__4 = new Func<IAsyncQueryable<float?>, CancellationToken, ValueTask<float?>>(AverageAsync).GetMethodInfo()));

        public static ValueTask<float?> AverageAsync(this IAsyncQueryable<float?> source, CancellationToken cancellationToken = default)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));

            return source.Provider.ExecuteAsync<float?>(Expression.Call(AverageAsync__2__4, source.Expression, Expression.Constant(cancellationToken, typeof(CancellationToken))), cancellationToken);
        }

        private static MethodInfo? s_AverageAsync__2__5;
        
        private static MethodInfo? AverageAsync__2__5 =>
            (s_AverageAsync__2__5 ??
            (s_AverageAsync__2__5 = new Func<IAsyncQueryable<float>, CancellationToken, ValueTask<float>>(AverageAsync).GetMethodInfo()));

        public static ValueTask<float> AverageAsync(this IAsyncQueryable<float> source, CancellationToken cancellationToken = default)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));

            return source.Provider.ExecuteAsync<float>(Expression.Call(AverageAsync__2__5, source.Expression, Expression.Constant(cancellationToken, typeof(CancellationToken))), cancellationToken);
        }

        private static MethodInfo? s_AverageAsync__2__6;
        
        private static MethodInfo? AverageAsync__2__6 =>
            (s_AverageAsync__2__6 ??
            (s_AverageAsync__2__6 = new Func<IAsyncQueryable<int?>, CancellationToken, ValueTask<double?>>(AverageAsync).GetMethodInfo()));

        public static ValueTask<double?> AverageAsync(this IAsyncQueryable<int?> source, CancellationToken cancellationToken = default)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));

            return source.Provider.ExecuteAsync<double?>(Expression.Call(AverageAsync__2__6, source.Expression, Expression.Constant(cancellationToken, typeof(CancellationToken))), cancellationToken);
        }

        private static MethodInfo? s_AverageAsync__2__7;
        
        private static MethodInfo? AverageAsync__2__7 =>
            (s_AverageAsync__2__7 ??
            (s_AverageAsync__2__7 = new Func<IAsyncQueryable<int>, CancellationToken, ValueTask<double>>(AverageAsync).GetMethodInfo()));

        public static ValueTask<double> AverageAsync(this IAsyncQueryable<int> source, CancellationToken cancellationToken = default)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));

            return source.Provider.ExecuteAsync<double>(Expression.Call(AverageAsync__2__7, source.Expression, Expression.Constant(cancellationToken, typeof(CancellationToken))), cancellationToken);
        }

        private static MethodInfo? s_AverageAsync__2__8;
        
        private static MethodInfo? AverageAsync__2__8 =>
            (s_AverageAsync__2__8 ??
            (s_AverageAsync__2__8 = new Func<IAsyncQueryable<long?>, CancellationToken, ValueTask<double?>>(AverageAsync).GetMethodInfo()));

        public static ValueTask<double?> AverageAsync(this IAsyncQueryable<long?> source, CancellationToken cancellationToken = default)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));

            return source.Provider.ExecuteAsync<double?>(Expression.Call(AverageAsync__2__8, source.Expression, Expression.Constant(cancellationToken, typeof(CancellationToken))), cancellationToken);
        }

        private static MethodInfo? s_AverageAsync__2__9;
        
        private static MethodInfo? AverageAsync__2__9 =>
            (s_AverageAsync__2__9 ??
            (s_AverageAsync__2__9 = new Func<IAsyncQueryable<long>, CancellationToken, ValueTask<double>>(AverageAsync).GetMethodInfo()));

        public static ValueTask<double> AverageAsync(this IAsyncQueryable<long> source, CancellationToken cancellationToken = default)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));

            return source.Provider.ExecuteAsync<double>(Expression.Call(AverageAsync__2__9, source.Expression, Expression.Constant(cancellationToken, typeof(CancellationToken))), cancellationToken);
        }

        private static MethodInfo? s_AverageAsync__TSource__3__0;
        
        private static MethodInfo? AverageAsync__TSource__3__0(Type TSource) =>
            (s_AverageAsync__TSource__3__0 ??
            (s_AverageAsync__TSource__3__0 = new Func<IAsyncQueryable<object>, Expression<Func<object, decimal?>>, CancellationToken, ValueTask<decimal?>>(AverageAsync<object>).GetMethodInfo()!.GetGenericMethodDefinition())).MakeGenericMethod(TSource);

        public static ValueTask<decimal?> AverageAsync<TSource>(this IAsyncQueryable<TSource> source, Expression<Func<TSource, decimal?>> selector, CancellationToken cancellationToken = default)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));
            if (selector == null)
                throw new ArgumentNullException(nameof(selector));

            return source.Provider.ExecuteAsync<decimal?>(Expression.Call(AverageAsync__TSource__3__0(typeof(TSource)), source.Expression, selector, Expression.Constant(cancellationToken, typeof(CancellationToken))), cancellationToken);
        }

        private static MethodInfo? s_AverageAsync__TSource__3__1;
        
        private static MethodInfo? AverageAsync__TSource__3__1(Type TSource) =>
            (s_AverageAsync__TSource__3__1 ??
            (s_AverageAsync__TSource__3__1 = new Func<IAsyncQueryable<object>, Expression<Func<object, decimal>>, CancellationToken, ValueTask<decimal>>(AverageAsync<object>).GetMethodInfo()!.GetGenericMethodDefinition())).MakeGenericMethod(TSource);

        public static ValueTask<decimal> AverageAsync<TSource>(this IAsyncQueryable<TSource> source, Expression<Func<TSource, decimal>> selector, CancellationToken cancellationToken = default)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));
            if (selector == null)
                throw new ArgumentNullException(nameof(selector));

            return source.Provider.ExecuteAsync<decimal>(Expression.Call(AverageAsync__TSource__3__1(typeof(TSource)), source.Expression, selector, Expression.Constant(cancellationToken, typeof(CancellationToken))), cancellationToken);
        }

        private static MethodInfo? s_AverageAsync__TSource__3__2;
        
        private static MethodInfo? AverageAsync__TSource__3__2(Type TSource) =>
            (s_AverageAsync__TSource__3__2 ??
            (s_AverageAsync__TSource__3__2 = new Func<IAsyncQueryable<object>, Expression<Func<object, double?>>, CancellationToken, ValueTask<double?>>(AverageAsync<object>).GetMethodInfo()!.GetGenericMethodDefinition())).MakeGenericMethod(TSource);

        public static ValueTask<double?> AverageAsync<TSource>(this IAsyncQueryable<TSource> source, Expression<Func<TSource, double?>> selector, CancellationToken cancellationToken = default)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));
            if (selector == null)
                throw new ArgumentNullException(nameof(selector));

            return source.Provider.ExecuteAsync<double?>(Expression.Call(AverageAsync__TSource__3__2(typeof(TSource)), source.Expression, selector, Expression.Constant(cancellationToken, typeof(CancellationToken))), cancellationToken);
        }

        private static MethodInfo? s_AverageAsync__TSource__3__3;
        
        private static MethodInfo? AverageAsync__TSource__3__3(Type TSource) =>
            (s_AverageAsync__TSource__3__3 ??
            (s_AverageAsync__TSource__3__3 = new Func<IAsyncQueryable<object>, Expression<Func<object, double>>, CancellationToken, ValueTask<double>>(AverageAsync<object>).GetMethodInfo()!.GetGenericMethodDefinition())).MakeGenericMethod(TSource);

        public static ValueTask<double> AverageAsync<TSource>(this IAsyncQueryable<TSource> source, Expression<Func<TSource, double>> selector, CancellationToken cancellationToken = default)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));
            if (selector == null)
                throw new ArgumentNullException(nameof(selector));

            return source.Provider.ExecuteAsync<double>(Expression.Call(AverageAsync__TSource__3__3(typeof(TSource)), source.Expression, selector, Expression.Constant(cancellationToken, typeof(CancellationToken))), cancellationToken);
        }

        private static MethodInfo? s_AverageAsync__TSource__3__4;
        
        private static MethodInfo? AverageAsync__TSource__3__4(Type TSource) =>
            (s_AverageAsync__TSource__3__4 ??
            (s_AverageAsync__TSource__3__4 = new Func<IAsyncQueryable<object>, Expression<Func<object, float?>>, CancellationToken, ValueTask<float?>>(AverageAsync<object>).GetMethodInfo()!.GetGenericMethodDefinition())).MakeGenericMethod(TSource);

        public static ValueTask<float?> AverageAsync<TSource>(this IAsyncQueryable<TSource> source, Expression<Func<TSource, float?>> selector, CancellationToken cancellationToken = default)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));
            if (selector == null)
                throw new ArgumentNullException(nameof(selector));

            return source.Provider.ExecuteAsync<float?>(Expression.Call(AverageAsync__TSource__3__4(typeof(TSource)), source.Expression, selector, Expression.Constant(cancellationToken, typeof(CancellationToken))), cancellationToken);
        }

        private static MethodInfo? s_AverageAsync__TSource__3__5;
        
        private static MethodInfo? AverageAsync__TSource__3__5(Type TSource) =>
            (s_AverageAsync__TSource__3__5 ??
            (s_AverageAsync__TSource__3__5 = new Func<IAsyncQueryable<object>, Expression<Func<object, float>>, CancellationToken, ValueTask<float>>(AverageAsync<object>).GetMethodInfo()!.GetGenericMethodDefinition())).MakeGenericMethod(TSource);

        public static ValueTask<float> AverageAsync<TSource>(this IAsyncQueryable<TSource> source, Expression<Func<TSource, float>> selector, CancellationToken cancellationToken = default)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));
            if (selector == null)
                throw new ArgumentNullException(nameof(selector));

            return source.Provider.ExecuteAsync<float>(Expression.Call(AverageAsync__TSource__3__5(typeof(TSource)), source.Expression, selector, Expression.Constant(cancellationToken, typeof(CancellationToken))), cancellationToken);
        }

        private static MethodInfo? s_AverageAsync__TSource__3__6;
        
        private static MethodInfo? AverageAsync__TSource__3__6(Type TSource) =>
            (s_AverageAsync__TSource__3__6 ??
            (s_AverageAsync__TSource__3__6 = new Func<IAsyncQueryable<object>, Expression<Func<object, int?>>, CancellationToken, ValueTask<double?>>(AverageAsync<object>).GetMethodInfo()!.GetGenericMethodDefinition())).MakeGenericMethod(TSource);

        public static ValueTask<double?> AverageAsync<TSource>(this IAsyncQueryable<TSource> source, Expression<Func<TSource, int?>> selector, CancellationToken cancellationToken = default)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));
            if (selector == null)
                throw new ArgumentNullException(nameof(selector));

            return source.Provider.ExecuteAsync<double?>(Expression.Call(AverageAsync__TSource__3__6(typeof(TSource)), source.Expression, selector, Expression.Constant(cancellationToken, typeof(CancellationToken))), cancellationToken);
        }

        private static MethodInfo? s_AverageAsync__TSource__3__7;
        
        private static MethodInfo? AverageAsync__TSource__3__7(Type TSource) =>
            (s_AverageAsync__TSource__3__7 ??
            (s_AverageAsync__TSource__3__7 = new Func<IAsyncQueryable<object>, Expression<Func<object, int>>, CancellationToken, ValueTask<double>>(AverageAsync<object>).GetMethodInfo()!.GetGenericMethodDefinition())).MakeGenericMethod(TSource);

        public static ValueTask<double> AverageAsync<TSource>(this IAsyncQueryable<TSource> source, Expression<Func<TSource, int>> selector, CancellationToken cancellationToken = default)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));
            if (selector == null)
                throw new ArgumentNullException(nameof(selector));

            return source.Provider.ExecuteAsync<double>(Expression.Call(AverageAsync__TSource__3__7(typeof(TSource)), source.Expression, selector, Expression.Constant(cancellationToken, typeof(CancellationToken))), cancellationToken);
        }

        private static MethodInfo? s_AverageAsync__TSource__3__8;
        
        private static MethodInfo? AverageAsync__TSource__3__8(Type TSource) =>
            (s_AverageAsync__TSource__3__8 ??
            (s_AverageAsync__TSource__3__8 = new Func<IAsyncQueryable<object>, Expression<Func<object, long?>>, CancellationToken, ValueTask<double?>>(AverageAsync<object>).GetMethodInfo()!.GetGenericMethodDefinition())).MakeGenericMethod(TSource);

        public static ValueTask<double?> AverageAsync<TSource>(this IAsyncQueryable<TSource> source, Expression<Func<TSource, long?>> selector, CancellationToken cancellationToken = default)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));
            if (selector == null)
                throw new ArgumentNullException(nameof(selector));

            return source.Provider.ExecuteAsync<double?>(Expression.Call(AverageAsync__TSource__3__8(typeof(TSource)), source.Expression, selector, Expression.Constant(cancellationToken, typeof(CancellationToken))), cancellationToken);
        }

        private static MethodInfo? s_AverageAsync__TSource__3__9;
        
        private static MethodInfo? AverageAsync__TSource__3__9(Type TSource) =>
            (s_AverageAsync__TSource__3__9 ??
            (s_AverageAsync__TSource__3__9 = new Func<IAsyncQueryable<object>, Expression<Func<object, long>>, CancellationToken, ValueTask<double>>(AverageAsync<object>).GetMethodInfo()!.GetGenericMethodDefinition())).MakeGenericMethod(TSource);

        public static ValueTask<double> AverageAsync<TSource>(this IAsyncQueryable<TSource> source, Expression<Func<TSource, long>> selector, CancellationToken cancellationToken = default)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));
            if (selector == null)
                throw new ArgumentNullException(nameof(selector));

            return source.Provider.ExecuteAsync<double>(Expression.Call(AverageAsync__TSource__3__9(typeof(TSource)), source.Expression, selector, Expression.Constant(cancellationToken, typeof(CancellationToken))), cancellationToken);
        }

        private static MethodInfo? s_AverageAwaitAsync__TSource__3__0;
        
        private static MethodInfo? AverageAwaitAsync__TSource__3__0(Type TSource) =>
            (s_AverageAwaitAsync__TSource__3__0 ??
            (s_AverageAwaitAsync__TSource__3__0 = new Func<IAsyncQueryable<object>, Expression<Func<object, ValueTask<decimal?>>>, CancellationToken, ValueTask<decimal?>>(AverageAwaitAsync<object>).GetMethodInfo()!.GetGenericMethodDefinition())).MakeGenericMethod(TSource);

        public static ValueTask<decimal?> AverageAwaitAsync<TSource>(this IAsyncQueryable<TSource> source, Expression<Func<TSource, ValueTask<decimal?>>> selector, CancellationToken cancellationToken = default)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));
            if (selector == null)
                throw new ArgumentNullException(nameof(selector));

            return source.Provider.ExecuteAsync<decimal?>(Expression.Call(AverageAwaitAsync__TSource__3__0(typeof(TSource)), source.Expression, selector, Expression.Constant(cancellationToken, typeof(CancellationToken))), cancellationToken);
        }

        private static MethodInfo? s_AverageAwaitAsync__TSource__3__1;
        
        private static MethodInfo? AverageAwaitAsync__TSource__3__1(Type TSource) =>
            (s_AverageAwaitAsync__TSource__3__1 ??
            (s_AverageAwaitAsync__TSource__3__1 = new Func<IAsyncQueryable<object>, Expression<Func<object, ValueTask<decimal>>>, CancellationToken, ValueTask<decimal>>(AverageAwaitAsync<object>).GetMethodInfo()!.GetGenericMethodDefinition())).MakeGenericMethod(TSource);

        public static ValueTask<decimal> AverageAwaitAsync<TSource>(this IAsyncQueryable<TSource> source, Expression<Func<TSource, ValueTask<decimal>>> selector, CancellationToken cancellationToken = default)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));
            if (selector == null)
                throw new ArgumentNullException(nameof(selector));

            return source.Provider.ExecuteAsync<decimal>(Expression.Call(AverageAwaitAsync__TSource__3__1(typeof(TSource)), source.Expression, selector, Expression.Constant(cancellationToken, typeof(CancellationToken))), cancellationToken);
        }

        private static MethodInfo? s_AverageAwaitAsync__TSource__3__2;
        
        private static MethodInfo? AverageAwaitAsync__TSource__3__2(Type TSource) =>
            (s_AverageAwaitAsync__TSource__3__2 ??
            (s_AverageAwaitAsync__TSource__3__2 = new Func<IAsyncQueryable<object>, Expression<Func<object, ValueTask<double?>>>, CancellationToken, ValueTask<double?>>(AverageAwaitAsync<object>).GetMethodInfo()!.GetGenericMethodDefinition())).MakeGenericMethod(TSource);

        public static ValueTask<double?> AverageAwaitAsync<TSource>(this IAsyncQueryable<TSource> source, Expression<Func<TSource, ValueTask<double?>>> selector, CancellationToken cancellationToken = default)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));
            if (selector == null)
                throw new ArgumentNullException(nameof(selector));

            return source.Provider.ExecuteAsync<double?>(Expression.Call(AverageAwaitAsync__TSource__3__2(typeof(TSource)), source.Expression, selector, Expression.Constant(cancellationToken, typeof(CancellationToken))), cancellationToken);
        }

        private static MethodInfo? s_AverageAwaitAsync__TSource__3__3;
        
        private static MethodInfo? AverageAwaitAsync__TSource__3__3(Type TSource) =>
            (s_AverageAwaitAsync__TSource__3__3 ??
            (s_AverageAwaitAsync__TSource__3__3 = new Func<IAsyncQueryable<object>, Expression<Func<object, ValueTask<double>>>, CancellationToken, ValueTask<double>>(AverageAwaitAsync<object>).GetMethodInfo()!.GetGenericMethodDefinition())).MakeGenericMethod(TSource);

        public static ValueTask<double> AverageAwaitAsync<TSource>(this IAsyncQueryable<TSource> source, Expression<Func<TSource, ValueTask<double>>> selector, CancellationToken cancellationToken = default)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));
            if (selector == null)
                throw new ArgumentNullException(nameof(selector));

            return source.Provider.ExecuteAsync<double>(Expression.Call(AverageAwaitAsync__TSource__3__3(typeof(TSource)), source.Expression, selector, Expression.Constant(cancellationToken, typeof(CancellationToken))), cancellationToken);
        }

        private static MethodInfo? s_AverageAwaitAsync__TSource__3__4;
        
        private static MethodInfo? AverageAwaitAsync__TSource__3__4(Type TSource) =>
            (s_AverageAwaitAsync__TSource__3__4 ??
            (s_AverageAwaitAsync__TSource__3__4 = new Func<IAsyncQueryable<object>, Expression<Func<object, ValueTask<float?>>>, CancellationToken, ValueTask<float?>>(AverageAwaitAsync<object>).GetMethodInfo()!.GetGenericMethodDefinition())).MakeGenericMethod(TSource);

        public static ValueTask<float?> AverageAwaitAsync<TSource>(this IAsyncQueryable<TSource> source, Expression<Func<TSource, ValueTask<float?>>> selector, CancellationToken cancellationToken = default)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));
            if (selector == null)
                throw new ArgumentNullException(nameof(selector));

            return source.Provider.ExecuteAsync<float?>(Expression.Call(AverageAwaitAsync__TSource__3__4(typeof(TSource)), source.Expression, selector, Expression.Constant(cancellationToken, typeof(CancellationToken))), cancellationToken);
        }

        private static MethodInfo? s_AverageAwaitAsync__TSource__3__5;
        
        private static MethodInfo? AverageAwaitAsync__TSource__3__5(Type TSource) =>
            (s_AverageAwaitAsync__TSource__3__5 ??
            (s_AverageAwaitAsync__TSource__3__5 = new Func<IAsyncQueryable<object>, Expression<Func<object, ValueTask<float>>>, CancellationToken, ValueTask<float>>(AverageAwaitAsync<object>).GetMethodInfo()!.GetGenericMethodDefinition())).MakeGenericMethod(TSource);

        public static ValueTask<float> AverageAwaitAsync<TSource>(this IAsyncQueryable<TSource> source, Expression<Func<TSource, ValueTask<float>>> selector, CancellationToken cancellationToken = default)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));
            if (selector == null)
                throw new ArgumentNullException(nameof(selector));

            return source.Provider.ExecuteAsync<float>(Expression.Call(AverageAwaitAsync__TSource__3__5(typeof(TSource)), source.Expression, selector, Expression.Constant(cancellationToken, typeof(CancellationToken))), cancellationToken);
        }

        private static MethodInfo? s_AverageAwaitAsync__TSource__3__6;
        
        private static MethodInfo? AverageAwaitAsync__TSource__3__6(Type TSource) =>
            (s_AverageAwaitAsync__TSource__3__6 ??
            (s_AverageAwaitAsync__TSource__3__6 = new Func<IAsyncQueryable<object>, Expression<Func<object, ValueTask<int?>>>, CancellationToken, ValueTask<double?>>(AverageAwaitAsync<object>).GetMethodInfo()!.GetGenericMethodDefinition())).MakeGenericMethod(TSource);

        public static ValueTask<double?> AverageAwaitAsync<TSource>(this IAsyncQueryable<TSource> source, Expression<Func<TSource, ValueTask<int?>>> selector, CancellationToken cancellationToken = default)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));
            if (selector == null)
                throw new ArgumentNullException(nameof(selector));

            return source.Provider.ExecuteAsync<double?>(Expression.Call(AverageAwaitAsync__TSource__3__6(typeof(TSource)), source.Expression, selector, Expression.Constant(cancellationToken, typeof(CancellationToken))), cancellationToken);
        }

        private static MethodInfo? s_AverageAwaitAsync__TSource__3__7;
        
        private static MethodInfo? AverageAwaitAsync__TSource__3__7(Type TSource) =>
            (s_AverageAwaitAsync__TSource__3__7 ??
            (s_AverageAwaitAsync__TSource__3__7 = new Func<IAsyncQueryable<object>, Expression<Func<object, ValueTask<int>>>, CancellationToken, ValueTask<double>>(AverageAwaitAsync<object>).GetMethodInfo()!.GetGenericMethodDefinition())).MakeGenericMethod(TSource);

        public static ValueTask<double> AverageAwaitAsync<TSource>(this IAsyncQueryable<TSource> source, Expression<Func<TSource, ValueTask<int>>> selector, CancellationToken cancellationToken = default)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));
            if (selector == null)
                throw new ArgumentNullException(nameof(selector));

            return source.Provider.ExecuteAsync<double>(Expression.Call(AverageAwaitAsync__TSource__3__7(typeof(TSource)), source.Expression, selector, Expression.Constant(cancellationToken, typeof(CancellationToken))), cancellationToken);
        }

        private static MethodInfo? s_AverageAwaitAsync__TSource__3__8;
        
        private static MethodInfo? AverageAwaitAsync__TSource__3__8(Type TSource) =>
            (s_AverageAwaitAsync__TSource__3__8 ??
            (s_AverageAwaitAsync__TSource__3__8 = new Func<IAsyncQueryable<object>, Expression<Func<object, ValueTask<long?>>>, CancellationToken, ValueTask<double?>>(AverageAwaitAsync<object>).GetMethodInfo()!.GetGenericMethodDefinition())).MakeGenericMethod(TSource);

        public static ValueTask<double?> AverageAwaitAsync<TSource>(this IAsyncQueryable<TSource> source, Expression<Func<TSource, ValueTask<long?>>> selector, CancellationToken cancellationToken = default)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));
            if (selector == null)
                throw new ArgumentNullException(nameof(selector));

            return source.Provider.ExecuteAsync<double?>(Expression.Call(AverageAwaitAsync__TSource__3__8(typeof(TSource)), source.Expression, selector, Expression.Constant(cancellationToken, typeof(CancellationToken))), cancellationToken);
        }

        private static MethodInfo? s_AverageAwaitAsync__TSource__3__9;
        
        private static MethodInfo? AverageAwaitAsync__TSource__3__9(Type TSource) =>
            (s_AverageAwaitAsync__TSource__3__9 ??
            (s_AverageAwaitAsync__TSource__3__9 = new Func<IAsyncQueryable<object>, Expression<Func<object, ValueTask<long>>>, CancellationToken, ValueTask<double>>(AverageAwaitAsync<object>).GetMethodInfo()!.GetGenericMethodDefinition())).MakeGenericMethod(TSource);

        public static ValueTask<double> AverageAwaitAsync<TSource>(this IAsyncQueryable<TSource> source, Expression<Func<TSource, ValueTask<long>>> selector, CancellationToken cancellationToken = default)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));
            if (selector == null)
                throw new ArgumentNullException(nameof(selector));

            return source.Provider.ExecuteAsync<double>(Expression.Call(AverageAwaitAsync__TSource__3__9(typeof(TSource)), source.Expression, selector, Expression.Constant(cancellationToken, typeof(CancellationToken))), cancellationToken);
        }

        private static MethodInfo? s_AverageAwaitWithCancellationAsync__TSource__3__0;
        
        private static MethodInfo? AverageAwaitWithCancellationAsync__TSource__3__0(Type TSource) =>
            (s_AverageAwaitWithCancellationAsync__TSource__3__0 ??
            (s_AverageAwaitWithCancellationAsync__TSource__3__0 = new Func<IAsyncQueryable<object>, Expression<Func<object, CancellationToken, ValueTask<decimal?>>>, CancellationToken, ValueTask<decimal?>>(AverageAwaitWithCancellationAsync<object>).GetMethodInfo()!.GetGenericMethodDefinition())).MakeGenericMethod(TSource);

        public static ValueTask<decimal?> AverageAwaitWithCancellationAsync<TSource>(this IAsyncQueryable<TSource> source, Expression<Func<TSource, CancellationToken, ValueTask<decimal?>>> selector, CancellationToken cancellationToken = default)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));
            if (selector == null)
                throw new ArgumentNullException(nameof(selector));

            return source.Provider.ExecuteAsync<decimal?>(Expression.Call(AverageAwaitWithCancellationAsync__TSource__3__0(typeof(TSource)), source.Expression, selector, Expression.Constant(cancellationToken, typeof(CancellationToken))), cancellationToken);
        }

        private static MethodInfo? s_AverageAwaitWithCancellationAsync__TSource__3__1;
        
        private static MethodInfo? AverageAwaitWithCancellationAsync__TSource__3__1(Type TSource) =>
            (s_AverageAwaitWithCancellationAsync__TSource__3__1 ??
            (s_AverageAwaitWithCancellationAsync__TSource__3__1 = new Func<IAsyncQueryable<object>, Expression<Func<object, CancellationToken, ValueTask<decimal>>>, CancellationToken, ValueTask<decimal>>(AverageAwaitWithCancellationAsync<object>).GetMethodInfo()!.GetGenericMethodDefinition())).MakeGenericMethod(TSource);

        public static ValueTask<decimal> AverageAwaitWithCancellationAsync<TSource>(this IAsyncQueryable<TSource> source, Expression<Func<TSource, CancellationToken, ValueTask<decimal>>> selector, CancellationToken cancellationToken = default)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));
            if (selector == null)
                throw new ArgumentNullException(nameof(selector));

            return source.Provider.ExecuteAsync<decimal>(Expression.Call(AverageAwaitWithCancellationAsync__TSource__3__1(typeof(TSource)), source.Expression, selector, Expression.Constant(cancellationToken, typeof(CancellationToken))), cancellationToken);
        }

        private static MethodInfo? s_AverageAwaitWithCancellationAsync__TSource__3__2;
        
        private static MethodInfo? AverageAwaitWithCancellationAsync__TSource__3__2(Type TSource) =>
            (s_AverageAwaitWithCancellationAsync__TSource__3__2 ??
            (s_AverageAwaitWithCancellationAsync__TSource__3__2 = new Func<IAsyncQueryable<object>, Expression<Func<object, CancellationToken, ValueTask<double?>>>, CancellationToken, ValueTask<double?>>(AverageAwaitWithCancellationAsync<object>).GetMethodInfo()!.GetGenericMethodDefinition())).MakeGenericMethod(TSource);

        public static ValueTask<double?> AverageAwaitWithCancellationAsync<TSource>(this IAsyncQueryable<TSource> source, Expression<Func<TSource, CancellationToken, ValueTask<double?>>> selector, CancellationToken cancellationToken = default)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));
            if (selector == null)
                throw new ArgumentNullException(nameof(selector));

            return source.Provider.ExecuteAsync<double?>(Expression.Call(AverageAwaitWithCancellationAsync__TSource__3__2(typeof(TSource)), source.Expression, selector, Expression.Constant(cancellationToken, typeof(CancellationToken))), cancellationToken);
        }

        private static MethodInfo? s_AverageAwaitWithCancellationAsync__TSource__3__3;
        
        private static MethodInfo? AverageAwaitWithCancellationAsync__TSource__3__3(Type TSource) =>
            (s_AverageAwaitWithCancellationAsync__TSource__3__3 ??
            (s_AverageAwaitWithCancellationAsync__TSource__3__3 = new Func<IAsyncQueryable<object>, Expression<Func<object, CancellationToken, ValueTask<double>>>, CancellationToken, ValueTask<double>>(AverageAwaitWithCancellationAsync<object>).GetMethodInfo()!.GetGenericMethodDefinition())).MakeGenericMethod(TSource);

        public static ValueTask<double> AverageAwaitWithCancellationAsync<TSource>(this IAsyncQueryable<TSource> source, Expression<Func<TSource, CancellationToken, ValueTask<double>>> selector, CancellationToken cancellationToken = default)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));
            if (selector == null)
                throw new ArgumentNullException(nameof(selector));

            return source.Provider.ExecuteAsync<double>(Expression.Call(AverageAwaitWithCancellationAsync__TSource__3__3(typeof(TSource)), source.Expression, selector, Expression.Constant(cancellationToken, typeof(CancellationToken))), cancellationToken);
        }

        private static MethodInfo? s_AverageAwaitWithCancellationAsync__TSource__3__4;
        
        private static MethodInfo? AverageAwaitWithCancellationAsync__TSource__3__4(Type TSource) =>
            (s_AverageAwaitWithCancellationAsync__TSource__3__4 ??
            (s_AverageAwaitWithCancellationAsync__TSource__3__4 = new Func<IAsyncQueryable<object>, Expression<Func<object, CancellationToken, ValueTask<float?>>>, CancellationToken, ValueTask<float?>>(AverageAwaitWithCancellationAsync<object>).GetMethodInfo()!.GetGenericMethodDefinition())).MakeGenericMethod(TSource);

        public static ValueTask<float?> AverageAwaitWithCancellationAsync<TSource>(this IAsyncQueryable<TSource> source, Expression<Func<TSource, CancellationToken, ValueTask<float?>>> selector, CancellationToken cancellationToken = default)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));
            if (selector == null)
                throw new ArgumentNullException(nameof(selector));

            return source.Provider.ExecuteAsync<float?>(Expression.Call(AverageAwaitWithCancellationAsync__TSource__3__4(typeof(TSource)), source.Expression, selector, Expression.Constant(cancellationToken, typeof(CancellationToken))), cancellationToken);
        }

        private static MethodInfo? s_AverageAwaitWithCancellationAsync__TSource__3__5;
        
        private static MethodInfo? AverageAwaitWithCancellationAsync__TSource__3__5(Type TSource) =>
            (s_AverageAwaitWithCancellationAsync__TSource__3__5 ??
            (s_AverageAwaitWithCancellationAsync__TSource__3__5 = new Func<IAsyncQueryable<object>, Expression<Func<object, CancellationToken, ValueTask<float>>>, CancellationToken, ValueTask<float>>(AverageAwaitWithCancellationAsync<object>).GetMethodInfo()!.GetGenericMethodDefinition())).MakeGenericMethod(TSource);

        public static ValueTask<float> AverageAwaitWithCancellationAsync<TSource>(this IAsyncQueryable<TSource> source, Expression<Func<TSource, CancellationToken, ValueTask<float>>> selector, CancellationToken cancellationToken = default)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));
            if (selector == null)
                throw new ArgumentNullException(nameof(selector));

            return source.Provider.ExecuteAsync<float>(Expression.Call(AverageAwaitWithCancellationAsync__TSource__3__5(typeof(TSource)), source.Expression, selector, Expression.Constant(cancellationToken, typeof(CancellationToken))), cancellationToken);
        }

        private static MethodInfo? s_AverageAwaitWithCancellationAsync__TSource__3__6;
        
        private static MethodInfo? AverageAwaitWithCancellationAsync__TSource__3__6(Type TSource) =>
            (s_AverageAwaitWithCancellationAsync__TSource__3__6 ??
            (s_AverageAwaitWithCancellationAsync__TSource__3__6 = new Func<IAsyncQueryable<object>, Expression<Func<object, CancellationToken, ValueTask<int?>>>, CancellationToken, ValueTask<double?>>(AverageAwaitWithCancellationAsync<object>).GetMethodInfo()!.GetGenericMethodDefinition())).MakeGenericMethod(TSource);

        public static ValueTask<double?> AverageAwaitWithCancellationAsync<TSource>(this IAsyncQueryable<TSource> source, Expression<Func<TSource, CancellationToken, ValueTask<int?>>> selector, CancellationToken cancellationToken = default)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));
            if (selector == null)
                throw new ArgumentNullException(nameof(selector));

            return source.Provider.ExecuteAsync<double?>(Expression.Call(AverageAwaitWithCancellationAsync__TSource__3__6(typeof(TSource)), source.Expression, selector, Expression.Constant(cancellationToken, typeof(CancellationToken))), cancellationToken);
        }

        private static MethodInfo? s_AverageAwaitWithCancellationAsync__TSource__3__7;
        
        private static MethodInfo? AverageAwaitWithCancellationAsync__TSource__3__7(Type TSource) =>
            (s_AverageAwaitWithCancellationAsync__TSource__3__7 ??
            (s_AverageAwaitWithCancellationAsync__TSource__3__7 = new Func<IAsyncQueryable<object>, Expression<Func<object, CancellationToken, ValueTask<int>>>, CancellationToken, ValueTask<double>>(AverageAwaitWithCancellationAsync<object>).GetMethodInfo()!.GetGenericMethodDefinition())).MakeGenericMethod(TSource);

        public static ValueTask<double> AverageAwaitWithCancellationAsync<TSource>(this IAsyncQueryable<TSource> source, Expression<Func<TSource, CancellationToken, ValueTask<int>>> selector, CancellationToken cancellationToken = default)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));
            if (selector == null)
                throw new ArgumentNullException(nameof(selector));

            return source.Provider.ExecuteAsync<double>(Expression.Call(AverageAwaitWithCancellationAsync__TSource__3__7(typeof(TSource)), source.Expression, selector, Expression.Constant(cancellationToken, typeof(CancellationToken))), cancellationToken);
        }

        private static MethodInfo? s_AverageAwaitWithCancellationAsync__TSource__3__8;
        
        private static MethodInfo? AverageAwaitWithCancellationAsync__TSource__3__8(Type TSource) =>
            (s_AverageAwaitWithCancellationAsync__TSource__3__8 ??
            (s_AverageAwaitWithCancellationAsync__TSource__3__8 = new Func<IAsyncQueryable<object>, Expression<Func<object, CancellationToken, ValueTask<long?>>>, CancellationToken, ValueTask<double?>>(AverageAwaitWithCancellationAsync<object>).GetMethodInfo()!.GetGenericMethodDefinition())).MakeGenericMethod(TSource);

        public static ValueTask<double?> AverageAwaitWithCancellationAsync<TSource>(this IAsyncQueryable<TSource> source, Expression<Func<TSource, CancellationToken, ValueTask<long?>>> selector, CancellationToken cancellationToken = default)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));
            if (selector == null)
                throw new ArgumentNullException(nameof(selector));

            return source.Provider.ExecuteAsync<double?>(Expression.Call(AverageAwaitWithCancellationAsync__TSource__3__8(typeof(TSource)), source.Expression, selector, Expression.Constant(cancellationToken, typeof(CancellationToken))), cancellationToken);
        }

        private static MethodInfo? s_AverageAwaitWithCancellationAsync__TSource__3__9;
        
        private static MethodInfo? AverageAwaitWithCancellationAsync__TSource__3__9(Type TSource) =>
            (s_AverageAwaitWithCancellationAsync__TSource__3__9 ??
            (s_AverageAwaitWithCancellationAsync__TSource__3__9 = new Func<IAsyncQueryable<object>, Expression<Func<object, CancellationToken, ValueTask<long>>>, CancellationToken, ValueTask<double>>(AverageAwaitWithCancellationAsync<object>).GetMethodInfo()!.GetGenericMethodDefinition())).MakeGenericMethod(TSource);

        public static ValueTask<double> AverageAwaitWithCancellationAsync<TSource>(this IAsyncQueryable<TSource> source, Expression<Func<TSource, CancellationToken, ValueTask<long>>> selector, CancellationToken cancellationToken = default)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));
            if (selector == null)
                throw new ArgumentNullException(nameof(selector));

            return source.Provider.ExecuteAsync<double>(Expression.Call(AverageAwaitWithCancellationAsync__TSource__3__9(typeof(TSource)), source.Expression, selector, Expression.Constant(cancellationToken, typeof(CancellationToken))), cancellationToken);
        }

        private static MethodInfo? s_Cast__TResult__1__0;
        
        private static MethodInfo? Cast__TResult__1__0(Type TResult) =>
            (s_Cast__TResult__1__0 ??
            (s_Cast__TResult__1__0 = new Func<IAsyncQueryable<object>, IAsyncQueryable<object>>(Cast<object>).GetMethodInfo()!.GetGenericMethodDefinition())).MakeGenericMethod(TResult);

        public static IAsyncQueryable<TResult> Cast<TResult>(this IAsyncQueryable<object> source)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));

            return source.Provider.CreateQuery<TResult>(Expression.Call(Cast__TResult__1__0(typeof(TResult)), source.Expression));
        }

        private static MethodInfo? s_Concat__TSource__2__0;
        
        private static MethodInfo? Concat__TSource__2__0(Type TSource) =>
            (s_Concat__TSource__2__0 ??
            (s_Concat__TSource__2__0 = new Func<IAsyncQueryable<object>, IAsyncEnumerable<object>, IAsyncQueryable<object>>(Concat<object>).GetMethodInfo()!.GetGenericMethodDefinition())).MakeGenericMethod(TSource);

        public static IAsyncQueryable<TSource> Concat<TSource>(this IAsyncQueryable<TSource> first, IAsyncEnumerable<TSource> second)
        {
            if (first == null)
                throw new ArgumentNullException(nameof(first));
            if (second == null)
                throw new ArgumentNullException(nameof(second));

            return first.Provider.CreateQuery<TSource>(Expression.Call(Concat__TSource__2__0(typeof(TSource)), first.Expression, GetSourceExpression(second)));
        }

        private static MethodInfo? s_ContainsAsync__TSource__3__0;
        
        private static MethodInfo? ContainsAsync__TSource__3__0(Type TSource) =>
            (s_ContainsAsync__TSource__3__0 ??
            (s_ContainsAsync__TSource__3__0 = new Func<IAsyncQueryable<object>, object, CancellationToken, ValueTask<bool>>(ContainsAsync<object>).GetMethodInfo()!.GetGenericMethodDefinition())).MakeGenericMethod(TSource);

        public static ValueTask<bool> ContainsAsync<TSource>(this IAsyncQueryable<TSource> source, TSource value, CancellationToken cancellationToken = default)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));

            return source.Provider.ExecuteAsync<bool>(Expression.Call(ContainsAsync__TSource__3__0(typeof(TSource)), source.Expression, Expression.Constant(value, typeof(TSource)), Expression.Constant(cancellationToken, typeof(CancellationToken))), cancellationToken);
        }

        private static MethodInfo? s_ContainsAsync__TSource__4__0;
        
        private static MethodInfo? ContainsAsync__TSource__4__0(Type TSource) =>
            (s_ContainsAsync__TSource__4__0 ??
            (s_ContainsAsync__TSource__4__0 = new Func<IAsyncQueryable<object>, object, IEqualityComparer<object>, CancellationToken, ValueTask<bool>>(ContainsAsync<object>).GetMethodInfo()!.GetGenericMethodDefinition())).MakeGenericMethod(TSource);

        public static ValueTask<bool> ContainsAsync<TSource>(this IAsyncQueryable<TSource> source, TSource value, IEqualityComparer<TSource>? comparer, CancellationToken cancellationToken = default)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));

            return source.Provider.ExecuteAsync<bool>(Expression.Call(ContainsAsync__TSource__4__0(typeof(TSource)), source.Expression, Expression.Constant(value, typeof(TSource)), Expression.Constant(comparer, typeof(IEqualityComparer<TSource>)), Expression.Constant(cancellationToken, typeof(CancellationToken))), cancellationToken);
        }

        private static MethodInfo? s_CountAsync__TSource__2__0;
        
        private static MethodInfo? CountAsync__TSource__2__0(Type TSource) =>
            (s_CountAsync__TSource__2__0 ??
            (s_CountAsync__TSource__2__0 = new Func<IAsyncQueryable<object>, CancellationToken, ValueTask<int>>(CountAsync<object>).GetMethodInfo()!.GetGenericMethodDefinition())).MakeGenericMethod(TSource);

        public static ValueTask<int> CountAsync<TSource>(this IAsyncQueryable<TSource> source, CancellationToken cancellationToken = default)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));

            return source.Provider.ExecuteAsync<int>(Expression.Call(CountAsync__TSource__2__0(typeof(TSource)), source.Expression, Expression.Constant(cancellationToken, typeof(CancellationToken))), cancellationToken);
        }

        private static MethodInfo? s_CountAsync__TSource__3__0;
        
        private static MethodInfo? CountAsync__TSource__3__0(Type TSource) =>
            (s_CountAsync__TSource__3__0 ??
            (s_CountAsync__TSource__3__0 = new Func<IAsyncQueryable<object>, Expression<Func<object, bool>>, CancellationToken, ValueTask<int>>(CountAsync<object>).GetMethodInfo()!.GetGenericMethodDefinition())).MakeGenericMethod(TSource);

        public static ValueTask<int> CountAsync<TSource>(this IAsyncQueryable<TSource> source, Expression<Func<TSource, bool>> predicate, CancellationToken cancellationToken = default)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));
            if (predicate == null)
                throw new ArgumentNullException(nameof(predicate));

            return source.Provider.ExecuteAsync<int>(Expression.Call(CountAsync__TSource__3__0(typeof(TSource)), source.Expression, predicate, Expression.Constant(cancellationToken, typeof(CancellationToken))), cancellationToken);
        }

        private static MethodInfo? s_CountAwaitAsync__TSource__3__0;
        
        private static MethodInfo? CountAwaitAsync__TSource__3__0(Type TSource) =>
            (s_CountAwaitAsync__TSource__3__0 ??
            (s_CountAwaitAsync__TSource__3__0 = new Func<IAsyncQueryable<object>, Expression<Func<object, ValueTask<bool>>>, CancellationToken, ValueTask<int>>(CountAwaitAsync<object>).GetMethodInfo()!.GetGenericMethodDefinition())).MakeGenericMethod(TSource);

        public static ValueTask<int> CountAwaitAsync<TSource>(this IAsyncQueryable<TSource> source, Expression<Func<TSource, ValueTask<bool>>> predicate, CancellationToken cancellationToken = default)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));
            if (predicate == null)
                throw new ArgumentNullException(nameof(predicate));

            return source.Provider.ExecuteAsync<int>(Expression.Call(CountAwaitAsync__TSource__3__0(typeof(TSource)), source.Expression, predicate, Expression.Constant(cancellationToken, typeof(CancellationToken))), cancellationToken);
        }

        private static MethodInfo? s_CountAwaitWithCancellationAsync__TSource__3__0;
        
        private static MethodInfo? CountAwaitWithCancellationAsync__TSource__3__0(Type TSource) =>
            (s_CountAwaitWithCancellationAsync__TSource__3__0 ??
            (s_CountAwaitWithCancellationAsync__TSource__3__0 = new Func<IAsyncQueryable<object>, Expression<Func<object, CancellationToken, ValueTask<bool>>>, CancellationToken, ValueTask<int>>(CountAwaitWithCancellationAsync<object>).GetMethodInfo()!.GetGenericMethodDefinition())).MakeGenericMethod(TSource);

        public static ValueTask<int> CountAwaitWithCancellationAsync<TSource>(this IAsyncQueryable<TSource> source, Expression<Func<TSource, CancellationToken, ValueTask<bool>>> predicate, CancellationToken cancellationToken = default)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));
            if (predicate == null)
                throw new ArgumentNullException(nameof(predicate));

            return source.Provider.ExecuteAsync<int>(Expression.Call(CountAwaitWithCancellationAsync__TSource__3__0(typeof(TSource)), source.Expression, predicate, Expression.Constant(cancellationToken, typeof(CancellationToken))), cancellationToken);
        }

        private static MethodInfo? s_DefaultIfEmpty__TSource__1__0;
        
        private static MethodInfo? DefaultIfEmpty__TSource__1__0(Type TSource) =>
            (s_DefaultIfEmpty__TSource__1__0 ??
            (s_DefaultIfEmpty__TSource__1__0 = new Func<IAsyncQueryable<object>, IAsyncQueryable<object>>(DefaultIfEmpty<object>).GetMethodInfo()!.GetGenericMethodDefinition())).MakeGenericMethod(TSource);

        public static IAsyncQueryable<TSource> DefaultIfEmpty<TSource>(this IAsyncQueryable<TSource> source)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));

            return source.Provider.CreateQuery<TSource>(Expression.Call(DefaultIfEmpty__TSource__1__0(typeof(TSource)), source.Expression));
        }

        private static MethodInfo? s_DefaultIfEmpty__TSource__2__0;
        
        private static MethodInfo? DefaultIfEmpty__TSource__2__0(Type TSource) =>
            (s_DefaultIfEmpty__TSource__2__0 ??
            (s_DefaultIfEmpty__TSource__2__0 = new Func<IAsyncQueryable<object>, object, IAsyncQueryable<object>>(DefaultIfEmpty<object>).GetMethodInfo()!.GetGenericMethodDefinition())).MakeGenericMethod(TSource);

        public static IAsyncQueryable<TSource> DefaultIfEmpty<TSource>(this IAsyncQueryable<TSource> source, TSource defaultValue)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));

            return source.Provider.CreateQuery<TSource>(Expression.Call(DefaultIfEmpty__TSource__2__0(typeof(TSource)), source.Expression, Expression.Constant(defaultValue, typeof(TSource))));
        }

        private static MethodInfo? s_Distinct__TSource__1__0;
        
        private static MethodInfo? Distinct__TSource__1__0(Type TSource) =>
            (s_Distinct__TSource__1__0 ??
            (s_Distinct__TSource__1__0 = new Func<IAsyncQueryable<object>, IAsyncQueryable<object>>(Distinct<object>).GetMethodInfo()!.GetGenericMethodDefinition())).MakeGenericMethod(TSource);

        public static IAsyncQueryable<TSource> Distinct<TSource>(this IAsyncQueryable<TSource> source)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));

            return source.Provider.CreateQuery<TSource>(Expression.Call(Distinct__TSource__1__0(typeof(TSource)), source.Expression));
        }

        private static MethodInfo? s_Distinct__TSource__2__0;
        
        private static MethodInfo? Distinct__TSource__2__0(Type TSource) =>
            (s_Distinct__TSource__2__0 ??
            (s_Distinct__TSource__2__0 = new Func<IAsyncQueryable<object>, IEqualityComparer<object>, IAsyncQueryable<object>>(Distinct<object>).GetMethodInfo()!.GetGenericMethodDefinition())).MakeGenericMethod(TSource);

        public static IAsyncQueryable<TSource> Distinct<TSource>(this IAsyncQueryable<TSource> source, IEqualityComparer<TSource>? comparer)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));

            return source.Provider.CreateQuery<TSource>(Expression.Call(Distinct__TSource__2__0(typeof(TSource)), source.Expression, Expression.Constant(comparer, typeof(IEqualityComparer<TSource>))));
        }

        private static MethodInfo? s_ElementAtAsync__TSource__3__0;
        
        private static MethodInfo? ElementAtAsync__TSource__3__0(Type TSource) =>
            (s_ElementAtAsync__TSource__3__0 ??
            (s_ElementAtAsync__TSource__3__0 = new Func<IAsyncQueryable<object>, int, CancellationToken, ValueTask<object>>(ElementAtAsync<object>).GetMethodInfo()!.GetGenericMethodDefinition())).MakeGenericMethod(TSource);

        public static ValueTask<TSource> ElementAtAsync<TSource>(this IAsyncQueryable<TSource> source, int index, CancellationToken cancellationToken = default)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));

            return source.Provider.ExecuteAsync<TSource>(Expression.Call(ElementAtAsync__TSource__3__0(typeof(TSource)), source.Expression, Expression.Constant(index, typeof(int)), Expression.Constant(cancellationToken, typeof(CancellationToken))), cancellationToken);
        }

        private static MethodInfo? s_ElementAtOrDefaultAsync__TSource__3__0;
        
        private static MethodInfo? ElementAtOrDefaultAsync__TSource__3__0(Type TSource) =>
            (s_ElementAtOrDefaultAsync__TSource__3__0 ??
            (s_ElementAtOrDefaultAsync__TSource__3__0 = new Func<IAsyncQueryable<object>, int, CancellationToken, ValueTask<object>>(ElementAtOrDefaultAsync<object>).GetMethodInfo()!.GetGenericMethodDefinition())).MakeGenericMethod(TSource);

        public static ValueTask<TSource> ElementAtOrDefaultAsync<TSource>(this IAsyncQueryable<TSource> source, int index, CancellationToken cancellationToken = default)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));

            return source.Provider.ExecuteAsync<TSource>(Expression.Call(ElementAtOrDefaultAsync__TSource__3__0(typeof(TSource)), source.Expression, Expression.Constant(index, typeof(int)), Expression.Constant(cancellationToken, typeof(CancellationToken))), cancellationToken);
        }

        private static MethodInfo? s_Except__TSource__2__0;
        
        private static MethodInfo? Except__TSource__2__0(Type TSource) =>
            (s_Except__TSource__2__0 ??
            (s_Except__TSource__2__0 = new Func<IAsyncQueryable<object>, IAsyncEnumerable<object>, IAsyncQueryable<object>>(Except<object>).GetMethodInfo()!.GetGenericMethodDefinition())).MakeGenericMethod(TSource);

        public static IAsyncQueryable<TSource> Except<TSource>(this IAsyncQueryable<TSource> first, IAsyncEnumerable<TSource> second)
        {
            if (first == null)
                throw new ArgumentNullException(nameof(first));
            if (second == null)
                throw new ArgumentNullException(nameof(second));

            return first.Provider.CreateQuery<TSource>(Expression.Call(Except__TSource__2__0(typeof(TSource)), first.Expression, GetSourceExpression(second)));
        }

        private static MethodInfo? s_Except__TSource__3__0;
        
        private static MethodInfo? Except__TSource__3__0(Type TSource) =>
            (s_Except__TSource__3__0 ??
            (s_Except__TSource__3__0 = new Func<IAsyncQueryable<object>, IAsyncEnumerable<object>, IEqualityComparer<object>, IAsyncQueryable<object>>(Except<object>).GetMethodInfo()!.GetGenericMethodDefinition())).MakeGenericMethod(TSource);

        public static IAsyncQueryable<TSource> Except<TSource>(this IAsyncQueryable<TSource> first, IAsyncEnumerable<TSource> second, IEqualityComparer<TSource>? comparer)
        {
            if (first == null)
                throw new ArgumentNullException(nameof(first));
            if (second == null)
                throw new ArgumentNullException(nameof(second));

            return first.Provider.CreateQuery<TSource>(Expression.Call(Except__TSource__3__0(typeof(TSource)), first.Expression, GetSourceExpression(second), Expression.Constant(comparer, typeof(IEqualityComparer<TSource>))));
        }

        private static MethodInfo? s_FirstAsync__TSource__2__0;
        
        private static MethodInfo? FirstAsync__TSource__2__0(Type TSource) =>
            (s_FirstAsync__TSource__2__0 ??
            (s_FirstAsync__TSource__2__0 = new Func<IAsyncQueryable<object>, CancellationToken, ValueTask<object>>(FirstAsync<object>).GetMethodInfo()!.GetGenericMethodDefinition())).MakeGenericMethod(TSource);

        public static ValueTask<TSource> FirstAsync<TSource>(this IAsyncQueryable<TSource> source, CancellationToken cancellationToken = default)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));

            return source.Provider.ExecuteAsync<TSource>(Expression.Call(FirstAsync__TSource__2__0(typeof(TSource)), source.Expression, Expression.Constant(cancellationToken, typeof(CancellationToken))), cancellationToken);
        }

        private static MethodInfo? s_FirstAsync__TSource__3__0;
        
        private static MethodInfo? FirstAsync__TSource__3__0(Type TSource) =>
            (s_FirstAsync__TSource__3__0 ??
            (s_FirstAsync__TSource__3__0 = new Func<IAsyncQueryable<object>, Expression<Func<object, bool>>, CancellationToken, ValueTask<object>>(FirstAsync<object>).GetMethodInfo()!.GetGenericMethodDefinition())).MakeGenericMethod(TSource);

        public static ValueTask<TSource> FirstAsync<TSource>(this IAsyncQueryable<TSource> source, Expression<Func<TSource, bool>> predicate, CancellationToken cancellationToken = default)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));
            if (predicate == null)
                throw new ArgumentNullException(nameof(predicate));

            return source.Provider.ExecuteAsync<TSource>(Expression.Call(FirstAsync__TSource__3__0(typeof(TSource)), source.Expression, predicate, Expression.Constant(cancellationToken, typeof(CancellationToken))), cancellationToken);
        }

        private static MethodInfo? s_FirstAwaitAsync__TSource__3__0;
        
        private static MethodInfo? FirstAwaitAsync__TSource__3__0(Type TSource) =>
            (s_FirstAwaitAsync__TSource__3__0 ??
            (s_FirstAwaitAsync__TSource__3__0 = new Func<IAsyncQueryable<object>, Expression<Func<object, ValueTask<bool>>>, CancellationToken, ValueTask<object>>(FirstAwaitAsync<object>).GetMethodInfo()!.GetGenericMethodDefinition())).MakeGenericMethod(TSource);

        public static ValueTask<TSource> FirstAwaitAsync<TSource>(this IAsyncQueryable<TSource> source, Expression<Func<TSource, ValueTask<bool>>> predicate, CancellationToken cancellationToken = default)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));
            if (predicate == null)
                throw new ArgumentNullException(nameof(predicate));

            return source.Provider.ExecuteAsync<TSource>(Expression.Call(FirstAwaitAsync__TSource__3__0(typeof(TSource)), source.Expression, predicate, Expression.Constant(cancellationToken, typeof(CancellationToken))), cancellationToken);
        }

        private static MethodInfo? s_FirstAwaitWithCancellationAsync__TSource__3__0;
        
        private static MethodInfo? FirstAwaitWithCancellationAsync__TSource__3__0(Type TSource) =>
            (s_FirstAwaitWithCancellationAsync__TSource__3__0 ??
            (s_FirstAwaitWithCancellationAsync__TSource__3__0 = new Func<IAsyncQueryable<object>, Expression<Func<object, CancellationToken, ValueTask<bool>>>, CancellationToken, ValueTask<object>>(FirstAwaitWithCancellationAsync<object>).GetMethodInfo()!.GetGenericMethodDefinition())).MakeGenericMethod(TSource);

        public static ValueTask<TSource> FirstAwaitWithCancellationAsync<TSource>(this IAsyncQueryable<TSource> source, Expression<Func<TSource, CancellationToken, ValueTask<bool>>> predicate, CancellationToken cancellationToken = default)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));
            if (predicate == null)
                throw new ArgumentNullException(nameof(predicate));

            return source.Provider.ExecuteAsync<TSource>(Expression.Call(FirstAwaitWithCancellationAsync__TSource__3__0(typeof(TSource)), source.Expression, predicate, Expression.Constant(cancellationToken, typeof(CancellationToken))), cancellationToken);
        }

        private static MethodInfo? s_FirstOrDefaultAsync__TSource__2__0;
        
        private static MethodInfo? FirstOrDefaultAsync__TSource__2__0(Type TSource) =>
            (s_FirstOrDefaultAsync__TSource__2__0 ??
            (s_FirstOrDefaultAsync__TSource__2__0 = new Func<IAsyncQueryable<object>, CancellationToken, ValueTask<object>>(FirstOrDefaultAsync<object>).GetMethodInfo()!.GetGenericMethodDefinition())).MakeGenericMethod(TSource);

        public static ValueTask<TSource> FirstOrDefaultAsync<TSource>(this IAsyncQueryable<TSource> source, CancellationToken cancellationToken = default)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));

            return source.Provider.ExecuteAsync<TSource>(Expression.Call(FirstOrDefaultAsync__TSource__2__0(typeof(TSource)), source.Expression, Expression.Constant(cancellationToken, typeof(CancellationToken))), cancellationToken);
        }

        private static MethodInfo? s_FirstOrDefaultAsync__TSource__3__0;
        
        private static MethodInfo? FirstOrDefaultAsync__TSource__3__0(Type TSource) =>
            (s_FirstOrDefaultAsync__TSource__3__0 ??
            (s_FirstOrDefaultAsync__TSource__3__0 = new Func<IAsyncQueryable<object>, Expression<Func<object, bool>>, CancellationToken, ValueTask<object>>(FirstOrDefaultAsync<object>).GetMethodInfo()!.GetGenericMethodDefinition())).MakeGenericMethod(TSource);

        public static ValueTask<TSource> FirstOrDefaultAsync<TSource>(this IAsyncQueryable<TSource> source, Expression<Func<TSource, bool>> predicate, CancellationToken cancellationToken = default)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));
            if (predicate == null)
                throw new ArgumentNullException(nameof(predicate));

            return source.Provider.ExecuteAsync<TSource>(Expression.Call(FirstOrDefaultAsync__TSource__3__0(typeof(TSource)), source.Expression, predicate, Expression.Constant(cancellationToken, typeof(CancellationToken))), cancellationToken);
        }

        private static MethodInfo? s_FirstOrDefaultAwaitAsync__TSource__3__0;
        
        private static MethodInfo? FirstOrDefaultAwaitAsync__TSource__3__0(Type TSource) =>
            (s_FirstOrDefaultAwaitAsync__TSource__3__0 ??
            (s_FirstOrDefaultAwaitAsync__TSource__3__0 = new Func<IAsyncQueryable<object>, Expression<Func<object, ValueTask<bool>>>, CancellationToken, ValueTask<object>>(FirstOrDefaultAwaitAsync<object>).GetMethodInfo()!.GetGenericMethodDefinition())).MakeGenericMethod(TSource);

        public static ValueTask<TSource> FirstOrDefaultAwaitAsync<TSource>(this IAsyncQueryable<TSource> source, Expression<Func<TSource, ValueTask<bool>>> predicate, CancellationToken cancellationToken = default)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));
            if (predicate == null)
                throw new ArgumentNullException(nameof(predicate));

            return source.Provider.ExecuteAsync<TSource>(Expression.Call(FirstOrDefaultAwaitAsync__TSource__3__0(typeof(TSource)), source.Expression, predicate, Expression.Constant(cancellationToken, typeof(CancellationToken))), cancellationToken);
        }

        private static MethodInfo? s_FirstOrDefaultAwaitWithCancellationAsync__TSource__3__0;
        
        private static MethodInfo? FirstOrDefaultAwaitWithCancellationAsync__TSource__3__0(Type TSource) =>
            (s_FirstOrDefaultAwaitWithCancellationAsync__TSource__3__0 ??
            (s_FirstOrDefaultAwaitWithCancellationAsync__TSource__3__0 = new Func<IAsyncQueryable<object>, Expression<Func<object, CancellationToken, ValueTask<bool>>>, CancellationToken, ValueTask<object>>(FirstOrDefaultAwaitWithCancellationAsync<object>).GetMethodInfo()!.GetGenericMethodDefinition())).MakeGenericMethod(TSource);

        public static ValueTask<TSource> FirstOrDefaultAwaitWithCancellationAsync<TSource>(this IAsyncQueryable<TSource> source, Expression<Func<TSource, CancellationToken, ValueTask<bool>>> predicate, CancellationToken cancellationToken = default)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));
            if (predicate == null)
                throw new ArgumentNullException(nameof(predicate));

            return source.Provider.ExecuteAsync<TSource>(Expression.Call(FirstOrDefaultAwaitWithCancellationAsync__TSource__3__0(typeof(TSource)), source.Expression, predicate, Expression.Constant(cancellationToken, typeof(CancellationToken))), cancellationToken);
        }

        private static MethodInfo? s_GroupBy__TSource_TKey__2__0;
        
        private static MethodInfo? GroupBy__TSource_TKey__2__0(Type TSource, Type TKey) =>
            (s_GroupBy__TSource_TKey__2__0 ??
            (s_GroupBy__TSource_TKey__2__0 = new Func<IAsyncQueryable<object>, Expression<Func<object, object>>, IAsyncQueryable<IAsyncGrouping<object, object>>>(GroupBy<object, object>).GetMethodInfo()!.GetGenericMethodDefinition())).MakeGenericMethod(TSource, TKey);

        public static IAsyncQueryable<IAsyncGrouping<TKey, TSource>> GroupBy<TSource, TKey>(this IAsyncQueryable<TSource> source, Expression<Func<TSource, TKey>> keySelector)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));
            if (keySelector == null)
                throw new ArgumentNullException(nameof(keySelector));

            return source.Provider.CreateQuery<IAsyncGrouping<TKey, TSource>>(Expression.Call(GroupBy__TSource_TKey__2__0(typeof(TSource), typeof(TKey)), source.Expression, keySelector));
        }

        private static MethodInfo? s_GroupBy__TSource_TKey__3__0;
        
        private static MethodInfo? GroupBy__TSource_TKey__3__0(Type TSource, Type TKey) =>
            (s_GroupBy__TSource_TKey__3__0 ??
            (s_GroupBy__TSource_TKey__3__0 = new Func<IAsyncQueryable<object>, Expression<Func<object, object>>, IEqualityComparer<object>, IAsyncQueryable<IAsyncGrouping<object, object>>>(GroupBy<object, object>).GetMethodInfo()!.GetGenericMethodDefinition())).MakeGenericMethod(TSource, TKey);

        public static IAsyncQueryable<IAsyncGrouping<TKey, TSource>> GroupBy<TSource, TKey>(this IAsyncQueryable<TSource> source, Expression<Func<TSource, TKey>> keySelector, IEqualityComparer<TKey>? comparer)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));
            if (keySelector == null)
                throw new ArgumentNullException(nameof(keySelector));

            return source.Provider.CreateQuery<IAsyncGrouping<TKey, TSource>>(Expression.Call(GroupBy__TSource_TKey__3__0(typeof(TSource), typeof(TKey)), source.Expression, keySelector, Expression.Constant(comparer, typeof(IEqualityComparer<TKey>))));
        }

        private static MethodInfo? s_GroupBy__TSource_TKey_TResult__3__0;
        
        private static MethodInfo? GroupBy__TSource_TKey_TResult__3__0(Type TSource, Type TKey, Type TResult) =>
            (s_GroupBy__TSource_TKey_TResult__3__0 ??
            (s_GroupBy__TSource_TKey_TResult__3__0 = new Func<IAsyncQueryable<object>, Expression<Func<object, object>>, Expression<Func<object, IAsyncEnumerable<object>, object>>, IAsyncQueryable<object>>(GroupBy<object, object, object>).GetMethodInfo()!.GetGenericMethodDefinition())).MakeGenericMethod(TSource, TKey, TResult);

        public static IAsyncQueryable<TResult> GroupBy<TSource, TKey, TResult>(this IAsyncQueryable<TSource> source, Expression<Func<TSource, TKey>> keySelector, Expression<Func<TKey, IAsyncEnumerable<TSource>, TResult>> resultSelector)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));
            if (keySelector == null)
                throw new ArgumentNullException(nameof(keySelector));
            if (resultSelector == null)
                throw new ArgumentNullException(nameof(resultSelector));

            return source.Provider.CreateQuery<TResult>(Expression.Call(GroupBy__TSource_TKey_TResult__3__0(typeof(TSource), typeof(TKey), typeof(TResult)), source.Expression, keySelector, resultSelector));
        }

        private static MethodInfo? s_GroupBy__TSource_TKey_TElement__3__0;
        
        private static MethodInfo? GroupBy__TSource_TKey_TElement__3__0(Type TSource, Type TKey, Type TElement) =>
            (s_GroupBy__TSource_TKey_TElement__3__0 ??
            (s_GroupBy__TSource_TKey_TElement__3__0 = new Func<IAsyncQueryable<object>, Expression<Func<object, object>>, Expression<Func<object, object>>, IAsyncQueryable<IAsyncGrouping<object, object>>>(GroupBy<object, object, object>).GetMethodInfo()!.GetGenericMethodDefinition())).MakeGenericMethod(TSource, TKey, TElement);

        public static IAsyncQueryable<IAsyncGrouping<TKey, TElement>> GroupBy<TSource, TKey, TElement>(this IAsyncQueryable<TSource> source, Expression<Func<TSource, TKey>> keySelector, Expression<Func<TSource, TElement>> elementSelector)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));
            if (keySelector == null)
                throw new ArgumentNullException(nameof(keySelector));
            if (elementSelector == null)
                throw new ArgumentNullException(nameof(elementSelector));

            return source.Provider.CreateQuery<IAsyncGrouping<TKey, TElement>>(Expression.Call(GroupBy__TSource_TKey_TElement__3__0(typeof(TSource), typeof(TKey), typeof(TElement)), source.Expression, keySelector, elementSelector));
        }

        private static MethodInfo? s_GroupBy__TSource_TKey_TResult__4__0;
        
        private static MethodInfo? GroupBy__TSource_TKey_TResult__4__0(Type TSource, Type TKey, Type TResult) =>
            (s_GroupBy__TSource_TKey_TResult__4__0 ??
            (s_GroupBy__TSource_TKey_TResult__4__0 = new Func<IAsyncQueryable<object>, Expression<Func<object, object>>, Expression<Func<object, IAsyncEnumerable<object>, object>>, IEqualityComparer<object>, IAsyncQueryable<object>>(GroupBy<object, object, object>).GetMethodInfo()!.GetGenericMethodDefinition())).MakeGenericMethod(TSource, TKey, TResult);

        public static IAsyncQueryable<TResult> GroupBy<TSource, TKey, TResult>(this IAsyncQueryable<TSource> source, Expression<Func<TSource, TKey>> keySelector, Expression<Func<TKey, IAsyncEnumerable<TSource>, TResult>> resultSelector, IEqualityComparer<TKey>? comparer)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));
            if (keySelector == null)
                throw new ArgumentNullException(nameof(keySelector));
            if (resultSelector == null)
                throw new ArgumentNullException(nameof(resultSelector));

            return source.Provider.CreateQuery<TResult>(Expression.Call(GroupBy__TSource_TKey_TResult__4__0(typeof(TSource), typeof(TKey), typeof(TResult)), source.Expression, keySelector, resultSelector, Expression.Constant(comparer, typeof(IEqualityComparer<TKey>))));
        }

        private static MethodInfo? s_GroupBy__TSource_TKey_TElement__4__0;
        
        private static MethodInfo? GroupBy__TSource_TKey_TElement__4__0(Type TSource, Type TKey, Type TElement) =>
            (s_GroupBy__TSource_TKey_TElement__4__0 ??
            (s_GroupBy__TSource_TKey_TElement__4__0 = new Func<IAsyncQueryable<object>, Expression<Func<object, object>>, Expression<Func<object, object>>, IEqualityComparer<object>, IAsyncQueryable<IAsyncGrouping<object, object>>>(GroupBy<object, object, object>).GetMethodInfo()!.GetGenericMethodDefinition())).MakeGenericMethod(TSource, TKey, TElement);

        public static IAsyncQueryable<IAsyncGrouping<TKey, TElement>> GroupBy<TSource, TKey, TElement>(this IAsyncQueryable<TSource> source, Expression<Func<TSource, TKey>> keySelector, Expression<Func<TSource, TElement>> elementSelector, IEqualityComparer<TKey>? comparer)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));
            if (keySelector == null)
                throw new ArgumentNullException(nameof(keySelector));
            if (elementSelector == null)
                throw new ArgumentNullException(nameof(elementSelector));

            return source.Provider.CreateQuery<IAsyncGrouping<TKey, TElement>>(Expression.Call(GroupBy__TSource_TKey_TElement__4__0(typeof(TSource), typeof(TKey), typeof(TElement)), source.Expression, keySelector, elementSelector, Expression.Constant(comparer, typeof(IEqualityComparer<TKey>))));
        }

        private static MethodInfo? s_GroupBy__TSource_TKey_TElement_TResult__4__0;
        
        private static MethodInfo? GroupBy__TSource_TKey_TElement_TResult__4__0(Type TSource, Type TKey, Type TElement, Type TResult) =>
            (s_GroupBy__TSource_TKey_TElement_TResult__4__0 ??
            (s_GroupBy__TSource_TKey_TElement_TResult__4__0 = new Func<IAsyncQueryable<object>, Expression<Func<object, object>>, Expression<Func<object, object>>, Expression<Func<object, IAsyncEnumerable<object>, object>>, IAsyncQueryable<object>>(GroupBy<object, object, object, object>).GetMethodInfo()!.GetGenericMethodDefinition())).MakeGenericMethod(TSource, TKey, TElement, TResult);

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

            return source.Provider.CreateQuery<TResult>(Expression.Call(GroupBy__TSource_TKey_TElement_TResult__4__0(typeof(TSource), typeof(TKey), typeof(TElement), typeof(TResult)), source.Expression, keySelector, elementSelector, resultSelector));
        }

        private static MethodInfo? s_GroupBy__TSource_TKey_TElement_TResult__5__0;
        
        private static MethodInfo? GroupBy__TSource_TKey_TElement_TResult__5__0(Type TSource, Type TKey, Type TElement, Type TResult) =>
            (s_GroupBy__TSource_TKey_TElement_TResult__5__0 ??
            (s_GroupBy__TSource_TKey_TElement_TResult__5__0 = new Func<IAsyncQueryable<object>, Expression<Func<object, object>>, Expression<Func<object, object>>, Expression<Func<object, IAsyncEnumerable<object>, object>>, IEqualityComparer<object>, IAsyncQueryable<object>>(GroupBy<object, object, object, object>).GetMethodInfo()!.GetGenericMethodDefinition())).MakeGenericMethod(TSource, TKey, TElement, TResult);

        public static IAsyncQueryable<TResult> GroupBy<TSource, TKey, TElement, TResult>(this IAsyncQueryable<TSource> source, Expression<Func<TSource, TKey>> keySelector, Expression<Func<TSource, TElement>> elementSelector, Expression<Func<TKey, IAsyncEnumerable<TElement>, TResult>> resultSelector, IEqualityComparer<TKey>? comparer)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));
            if (keySelector == null)
                throw new ArgumentNullException(nameof(keySelector));
            if (elementSelector == null)
                throw new ArgumentNullException(nameof(elementSelector));
            if (resultSelector == null)
                throw new ArgumentNullException(nameof(resultSelector));

            return source.Provider.CreateQuery<TResult>(Expression.Call(GroupBy__TSource_TKey_TElement_TResult__5__0(typeof(TSource), typeof(TKey), typeof(TElement), typeof(TResult)), source.Expression, keySelector, elementSelector, resultSelector, Expression.Constant(comparer, typeof(IEqualityComparer<TKey>))));
        }

        private static MethodInfo? s_GroupByAwait__TSource_TKey__2__0;
        
        private static MethodInfo? GroupByAwait__TSource_TKey__2__0(Type TSource, Type TKey) =>
            (s_GroupByAwait__TSource_TKey__2__0 ??
            (s_GroupByAwait__TSource_TKey__2__0 = new Func<IAsyncQueryable<object>, Expression<Func<object, ValueTask<object>>>, IAsyncQueryable<IAsyncGrouping<object, object>>>(GroupByAwait<object, object>).GetMethodInfo()!.GetGenericMethodDefinition())).MakeGenericMethod(TSource, TKey);

        public static IAsyncQueryable<IAsyncGrouping<TKey, TSource>> GroupByAwait<TSource, TKey>(this IAsyncQueryable<TSource> source, Expression<Func<TSource, ValueTask<TKey>>> keySelector)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));
            if (keySelector == null)
                throw new ArgumentNullException(nameof(keySelector));

            return source.Provider.CreateQuery<IAsyncGrouping<TKey, TSource>>(Expression.Call(GroupByAwait__TSource_TKey__2__0(typeof(TSource), typeof(TKey)), source.Expression, keySelector));
        }

        private static MethodInfo? s_GroupByAwait__TSource_TKey__3__0;
        
        private static MethodInfo? GroupByAwait__TSource_TKey__3__0(Type TSource, Type TKey) =>
            (s_GroupByAwait__TSource_TKey__3__0 ??
            (s_GroupByAwait__TSource_TKey__3__0 = new Func<IAsyncQueryable<object>, Expression<Func<object, ValueTask<object>>>, IEqualityComparer<object>, IAsyncQueryable<IAsyncGrouping<object, object>>>(GroupByAwait<object, object>).GetMethodInfo()!.GetGenericMethodDefinition())).MakeGenericMethod(TSource, TKey);

        public static IAsyncQueryable<IAsyncGrouping<TKey, TSource>> GroupByAwait<TSource, TKey>(this IAsyncQueryable<TSource> source, Expression<Func<TSource, ValueTask<TKey>>> keySelector, IEqualityComparer<TKey>? comparer)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));
            if (keySelector == null)
                throw new ArgumentNullException(nameof(keySelector));

            return source.Provider.CreateQuery<IAsyncGrouping<TKey, TSource>>(Expression.Call(GroupByAwait__TSource_TKey__3__0(typeof(TSource), typeof(TKey)), source.Expression, keySelector, Expression.Constant(comparer, typeof(IEqualityComparer<TKey>))));
        }

        private static MethodInfo? s_GroupByAwait__TSource_TKey_TResult__3__0;
        
        private static MethodInfo? GroupByAwait__TSource_TKey_TResult__3__0(Type TSource, Type TKey, Type TResult) =>
            (s_GroupByAwait__TSource_TKey_TResult__3__0 ??
            (s_GroupByAwait__TSource_TKey_TResult__3__0 = new Func<IAsyncQueryable<object>, Expression<Func<object, ValueTask<object>>>, Expression<Func<object, IAsyncEnumerable<object>, ValueTask<object>>>, IAsyncQueryable<object>>(GroupByAwait<object, object, object>).GetMethodInfo()!.GetGenericMethodDefinition())).MakeGenericMethod(TSource, TKey, TResult);

        public static IAsyncQueryable<TResult> GroupByAwait<TSource, TKey, TResult>(this IAsyncQueryable<TSource> source, Expression<Func<TSource, ValueTask<TKey>>> keySelector, Expression<Func<TKey, IAsyncEnumerable<TSource>, ValueTask<TResult>>> resultSelector)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));
            if (keySelector == null)
                throw new ArgumentNullException(nameof(keySelector));
            if (resultSelector == null)
                throw new ArgumentNullException(nameof(resultSelector));

            return source.Provider.CreateQuery<TResult>(Expression.Call(GroupByAwait__TSource_TKey_TResult__3__0(typeof(TSource), typeof(TKey), typeof(TResult)), source.Expression, keySelector, resultSelector));
        }

        private static MethodInfo? s_GroupByAwait__TSource_TKey_TElement__3__0;
        
        private static MethodInfo? GroupByAwait__TSource_TKey_TElement__3__0(Type TSource, Type TKey, Type TElement) =>
            (s_GroupByAwait__TSource_TKey_TElement__3__0 ??
            (s_GroupByAwait__TSource_TKey_TElement__3__0 = new Func<IAsyncQueryable<object>, Expression<Func<object, ValueTask<object>>>, Expression<Func<object, ValueTask<object>>>, IAsyncQueryable<IAsyncGrouping<object, object>>>(GroupByAwait<object, object, object>).GetMethodInfo()!.GetGenericMethodDefinition())).MakeGenericMethod(TSource, TKey, TElement);

        public static IAsyncQueryable<IAsyncGrouping<TKey, TElement>> GroupByAwait<TSource, TKey, TElement>(this IAsyncQueryable<TSource> source, Expression<Func<TSource, ValueTask<TKey>>> keySelector, Expression<Func<TSource, ValueTask<TElement>>> elementSelector)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));
            if (keySelector == null)
                throw new ArgumentNullException(nameof(keySelector));
            if (elementSelector == null)
                throw new ArgumentNullException(nameof(elementSelector));

            return source.Provider.CreateQuery<IAsyncGrouping<TKey, TElement>>(Expression.Call(GroupByAwait__TSource_TKey_TElement__3__0(typeof(TSource), typeof(TKey), typeof(TElement)), source.Expression, keySelector, elementSelector));
        }

        private static MethodInfo? s_GroupByAwait__TSource_TKey_TResult__4__0;
        
        private static MethodInfo? GroupByAwait__TSource_TKey_TResult__4__0(Type TSource, Type TKey, Type TResult) =>
            (s_GroupByAwait__TSource_TKey_TResult__4__0 ??
            (s_GroupByAwait__TSource_TKey_TResult__4__0 = new Func<IAsyncQueryable<object>, Expression<Func<object, ValueTask<object>>>, Expression<Func<object, IAsyncEnumerable<object>, ValueTask<object>>>, IEqualityComparer<object>, IAsyncQueryable<object>>(GroupByAwait<object, object, object>).GetMethodInfo()!.GetGenericMethodDefinition())).MakeGenericMethod(TSource, TKey, TResult);

        public static IAsyncQueryable<TResult> GroupByAwait<TSource, TKey, TResult>(this IAsyncQueryable<TSource> source, Expression<Func<TSource, ValueTask<TKey>>> keySelector, Expression<Func<TKey, IAsyncEnumerable<TSource>, ValueTask<TResult>>> resultSelector, IEqualityComparer<TKey>? comparer)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));
            if (keySelector == null)
                throw new ArgumentNullException(nameof(keySelector));
            if (resultSelector == null)
                throw new ArgumentNullException(nameof(resultSelector));

            return source.Provider.CreateQuery<TResult>(Expression.Call(GroupByAwait__TSource_TKey_TResult__4__0(typeof(TSource), typeof(TKey), typeof(TResult)), source.Expression, keySelector, resultSelector, Expression.Constant(comparer, typeof(IEqualityComparer<TKey>))));
        }

        private static MethodInfo? s_GroupByAwait__TSource_TKey_TElement__4__0;
        
        private static MethodInfo? GroupByAwait__TSource_TKey_TElement__4__0(Type TSource, Type TKey, Type TElement) =>
            (s_GroupByAwait__TSource_TKey_TElement__4__0 ??
            (s_GroupByAwait__TSource_TKey_TElement__4__0 = new Func<IAsyncQueryable<object>, Expression<Func<object, ValueTask<object>>>, Expression<Func<object, ValueTask<object>>>, IEqualityComparer<object>, IAsyncQueryable<IAsyncGrouping<object, object>>>(GroupByAwait<object, object, object>).GetMethodInfo()!.GetGenericMethodDefinition())).MakeGenericMethod(TSource, TKey, TElement);

        public static IAsyncQueryable<IAsyncGrouping<TKey, TElement>> GroupByAwait<TSource, TKey, TElement>(this IAsyncQueryable<TSource> source, Expression<Func<TSource, ValueTask<TKey>>> keySelector, Expression<Func<TSource, ValueTask<TElement>>> elementSelector, IEqualityComparer<TKey>? comparer)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));
            if (keySelector == null)
                throw new ArgumentNullException(nameof(keySelector));
            if (elementSelector == null)
                throw new ArgumentNullException(nameof(elementSelector));

            return source.Provider.CreateQuery<IAsyncGrouping<TKey, TElement>>(Expression.Call(GroupByAwait__TSource_TKey_TElement__4__0(typeof(TSource), typeof(TKey), typeof(TElement)), source.Expression, keySelector, elementSelector, Expression.Constant(comparer, typeof(IEqualityComparer<TKey>))));
        }

        private static MethodInfo? s_GroupByAwait__TSource_TKey_TElement_TResult__4__0;
        
        private static MethodInfo? GroupByAwait__TSource_TKey_TElement_TResult__4__0(Type TSource, Type TKey, Type TElement, Type TResult) =>
            (s_GroupByAwait__TSource_TKey_TElement_TResult__4__0 ??
            (s_GroupByAwait__TSource_TKey_TElement_TResult__4__0 = new Func<IAsyncQueryable<object>, Expression<Func<object, ValueTask<object>>>, Expression<Func<object, ValueTask<object>>>, Expression<Func<object, IAsyncEnumerable<object>, ValueTask<object>>>, IAsyncQueryable<object>>(GroupByAwait<object, object, object, object>).GetMethodInfo()!.GetGenericMethodDefinition())).MakeGenericMethod(TSource, TKey, TElement, TResult);

        public static IAsyncQueryable<TResult> GroupByAwait<TSource, TKey, TElement, TResult>(this IAsyncQueryable<TSource> source, Expression<Func<TSource, ValueTask<TKey>>> keySelector, Expression<Func<TSource, ValueTask<TElement>>> elementSelector, Expression<Func<TKey, IAsyncEnumerable<TElement>, ValueTask<TResult>>> resultSelector)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));
            if (keySelector == null)
                throw new ArgumentNullException(nameof(keySelector));
            if (elementSelector == null)
                throw new ArgumentNullException(nameof(elementSelector));
            if (resultSelector == null)
                throw new ArgumentNullException(nameof(resultSelector));

            return source.Provider.CreateQuery<TResult>(Expression.Call(GroupByAwait__TSource_TKey_TElement_TResult__4__0(typeof(TSource), typeof(TKey), typeof(TElement), typeof(TResult)), source.Expression, keySelector, elementSelector, resultSelector));
        }

        private static MethodInfo? s_GroupByAwait__TSource_TKey_TElement_TResult__5__0;
        
        private static MethodInfo? GroupByAwait__TSource_TKey_TElement_TResult__5__0(Type TSource, Type TKey, Type TElement, Type TResult) =>
            (s_GroupByAwait__TSource_TKey_TElement_TResult__5__0 ??
            (s_GroupByAwait__TSource_TKey_TElement_TResult__5__0 = new Func<IAsyncQueryable<object>, Expression<Func<object, ValueTask<object>>>, Expression<Func<object, ValueTask<object>>>, Expression<Func<object, IAsyncEnumerable<object>, ValueTask<object>>>, IEqualityComparer<object>, IAsyncQueryable<object>>(GroupByAwait<object, object, object, object>).GetMethodInfo()!.GetGenericMethodDefinition())).MakeGenericMethod(TSource, TKey, TElement, TResult);

        public static IAsyncQueryable<TResult> GroupByAwait<TSource, TKey, TElement, TResult>(this IAsyncQueryable<TSource> source, Expression<Func<TSource, ValueTask<TKey>>> keySelector, Expression<Func<TSource, ValueTask<TElement>>> elementSelector, Expression<Func<TKey, IAsyncEnumerable<TElement>, ValueTask<TResult>>> resultSelector, IEqualityComparer<TKey>? comparer)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));
            if (keySelector == null)
                throw new ArgumentNullException(nameof(keySelector));
            if (elementSelector == null)
                throw new ArgumentNullException(nameof(elementSelector));
            if (resultSelector == null)
                throw new ArgumentNullException(nameof(resultSelector));

            return source.Provider.CreateQuery<TResult>(Expression.Call(GroupByAwait__TSource_TKey_TElement_TResult__5__0(typeof(TSource), typeof(TKey), typeof(TElement), typeof(TResult)), source.Expression, keySelector, elementSelector, resultSelector, Expression.Constant(comparer, typeof(IEqualityComparer<TKey>))));
        }

        private static MethodInfo? s_GroupByAwaitWithCancellation__TSource_TKey__2__0;
        
        private static MethodInfo? GroupByAwaitWithCancellation__TSource_TKey__2__0(Type TSource, Type TKey) =>
            (s_GroupByAwaitWithCancellation__TSource_TKey__2__0 ??
            (s_GroupByAwaitWithCancellation__TSource_TKey__2__0 = new Func<IAsyncQueryable<object>, Expression<Func<object, CancellationToken, ValueTask<object>>>, IAsyncQueryable<IAsyncGrouping<object, object>>>(GroupByAwaitWithCancellation<object, object>).GetMethodInfo()!.GetGenericMethodDefinition())).MakeGenericMethod(TSource, TKey);

        public static IAsyncQueryable<IAsyncGrouping<TKey, TSource>> GroupByAwaitWithCancellation<TSource, TKey>(this IAsyncQueryable<TSource> source, Expression<Func<TSource, CancellationToken, ValueTask<TKey>>> keySelector)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));
            if (keySelector == null)
                throw new ArgumentNullException(nameof(keySelector));

            return source.Provider.CreateQuery<IAsyncGrouping<TKey, TSource>>(Expression.Call(GroupByAwaitWithCancellation__TSource_TKey__2__0(typeof(TSource), typeof(TKey)), source.Expression, keySelector));
        }

        private static MethodInfo? s_GroupByAwaitWithCancellation__TSource_TKey__3__0;
        
        private static MethodInfo? GroupByAwaitWithCancellation__TSource_TKey__3__0(Type TSource, Type TKey) =>
            (s_GroupByAwaitWithCancellation__TSource_TKey__3__0 ??
            (s_GroupByAwaitWithCancellation__TSource_TKey__3__0 = new Func<IAsyncQueryable<object>, Expression<Func<object, CancellationToken, ValueTask<object>>>, IEqualityComparer<object>, IAsyncQueryable<IAsyncGrouping<object, object>>>(GroupByAwaitWithCancellation<object, object>).GetMethodInfo()!.GetGenericMethodDefinition())).MakeGenericMethod(TSource, TKey);

        public static IAsyncQueryable<IAsyncGrouping<TKey, TSource>> GroupByAwaitWithCancellation<TSource, TKey>(this IAsyncQueryable<TSource> source, Expression<Func<TSource, CancellationToken, ValueTask<TKey>>> keySelector, IEqualityComparer<TKey>? comparer)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));
            if (keySelector == null)
                throw new ArgumentNullException(nameof(keySelector));

            return source.Provider.CreateQuery<IAsyncGrouping<TKey, TSource>>(Expression.Call(GroupByAwaitWithCancellation__TSource_TKey__3__0(typeof(TSource), typeof(TKey)), source.Expression, keySelector, Expression.Constant(comparer, typeof(IEqualityComparer<TKey>))));
        }

        private static MethodInfo? s_GroupByAwaitWithCancellation__TSource_TKey_TResult__3__0;
        
        private static MethodInfo? GroupByAwaitWithCancellation__TSource_TKey_TResult__3__0(Type TSource, Type TKey, Type TResult) =>
            (s_GroupByAwaitWithCancellation__TSource_TKey_TResult__3__0 ??
            (s_GroupByAwaitWithCancellation__TSource_TKey_TResult__3__0 = new Func<IAsyncQueryable<object>, Expression<Func<object, CancellationToken, ValueTask<object>>>, Expression<Func<object, IAsyncEnumerable<object>, CancellationToken, ValueTask<object>>>, IAsyncQueryable<object>>(GroupByAwaitWithCancellation<object, object, object>).GetMethodInfo()!.GetGenericMethodDefinition())).MakeGenericMethod(TSource, TKey, TResult);

        public static IAsyncQueryable<TResult> GroupByAwaitWithCancellation<TSource, TKey, TResult>(this IAsyncQueryable<TSource> source, Expression<Func<TSource, CancellationToken, ValueTask<TKey>>> keySelector, Expression<Func<TKey, IAsyncEnumerable<TSource>, CancellationToken, ValueTask<TResult>>> resultSelector)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));
            if (keySelector == null)
                throw new ArgumentNullException(nameof(keySelector));
            if (resultSelector == null)
                throw new ArgumentNullException(nameof(resultSelector));

            return source.Provider.CreateQuery<TResult>(Expression.Call(GroupByAwaitWithCancellation__TSource_TKey_TResult__3__0(typeof(TSource), typeof(TKey), typeof(TResult)), source.Expression, keySelector, resultSelector));
        }

        private static MethodInfo? s_GroupByAwaitWithCancellation__TSource_TKey_TElement__3__0;
        
        private static MethodInfo? GroupByAwaitWithCancellation__TSource_TKey_TElement__3__0(Type TSource, Type TKey, Type TElement) =>
            (s_GroupByAwaitWithCancellation__TSource_TKey_TElement__3__0 ??
            (s_GroupByAwaitWithCancellation__TSource_TKey_TElement__3__0 = new Func<IAsyncQueryable<object>, Expression<Func<object, CancellationToken, ValueTask<object>>>, Expression<Func<object, CancellationToken, ValueTask<object>>>, IAsyncQueryable<IAsyncGrouping<object, object>>>(GroupByAwaitWithCancellation<object, object, object>).GetMethodInfo()!.GetGenericMethodDefinition())).MakeGenericMethod(TSource, TKey, TElement);

        public static IAsyncQueryable<IAsyncGrouping<TKey, TElement>> GroupByAwaitWithCancellation<TSource, TKey, TElement>(this IAsyncQueryable<TSource> source, Expression<Func<TSource, CancellationToken, ValueTask<TKey>>> keySelector, Expression<Func<TSource, CancellationToken, ValueTask<TElement>>> elementSelector)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));
            if (keySelector == null)
                throw new ArgumentNullException(nameof(keySelector));
            if (elementSelector == null)
                throw new ArgumentNullException(nameof(elementSelector));

            return source.Provider.CreateQuery<IAsyncGrouping<TKey, TElement>>(Expression.Call(GroupByAwaitWithCancellation__TSource_TKey_TElement__3__0(typeof(TSource), typeof(TKey), typeof(TElement)), source.Expression, keySelector, elementSelector));
        }

        private static MethodInfo? s_GroupByAwaitWithCancellation__TSource_TKey_TResult__4__0;
        
        private static MethodInfo? GroupByAwaitWithCancellation__TSource_TKey_TResult__4__0(Type TSource, Type TKey, Type TResult) =>
            (s_GroupByAwaitWithCancellation__TSource_TKey_TResult__4__0 ??
            (s_GroupByAwaitWithCancellation__TSource_TKey_TResult__4__0 = new Func<IAsyncQueryable<object>, Expression<Func<object, CancellationToken, ValueTask<object>>>, Expression<Func<object, IAsyncEnumerable<object>, CancellationToken, ValueTask<object>>>, IEqualityComparer<object>, IAsyncQueryable<object>>(GroupByAwaitWithCancellation<object, object, object>).GetMethodInfo()!.GetGenericMethodDefinition())).MakeGenericMethod(TSource, TKey, TResult);

        public static IAsyncQueryable<TResult> GroupByAwaitWithCancellation<TSource, TKey, TResult>(this IAsyncQueryable<TSource> source, Expression<Func<TSource, CancellationToken, ValueTask<TKey>>> keySelector, Expression<Func<TKey, IAsyncEnumerable<TSource>, CancellationToken, ValueTask<TResult>>> resultSelector, IEqualityComparer<TKey>? comparer)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));
            if (keySelector == null)
                throw new ArgumentNullException(nameof(keySelector));
            if (resultSelector == null)
                throw new ArgumentNullException(nameof(resultSelector));

            return source.Provider.CreateQuery<TResult>(Expression.Call(GroupByAwaitWithCancellation__TSource_TKey_TResult__4__0(typeof(TSource), typeof(TKey), typeof(TResult)), source.Expression, keySelector, resultSelector, Expression.Constant(comparer, typeof(IEqualityComparer<TKey>))));
        }

        private static MethodInfo? s_GroupByAwaitWithCancellation__TSource_TKey_TElement__4__0;
        
        private static MethodInfo? GroupByAwaitWithCancellation__TSource_TKey_TElement__4__0(Type TSource, Type TKey, Type TElement) =>
            (s_GroupByAwaitWithCancellation__TSource_TKey_TElement__4__0 ??
            (s_GroupByAwaitWithCancellation__TSource_TKey_TElement__4__0 = new Func<IAsyncQueryable<object>, Expression<Func<object, CancellationToken, ValueTask<object>>>, Expression<Func<object, CancellationToken, ValueTask<object>>>, IEqualityComparer<object>, IAsyncQueryable<IAsyncGrouping<object, object>>>(GroupByAwaitWithCancellation<object, object, object>).GetMethodInfo()!.GetGenericMethodDefinition())).MakeGenericMethod(TSource, TKey, TElement);

        public static IAsyncQueryable<IAsyncGrouping<TKey, TElement>> GroupByAwaitWithCancellation<TSource, TKey, TElement>(this IAsyncQueryable<TSource> source, Expression<Func<TSource, CancellationToken, ValueTask<TKey>>> keySelector, Expression<Func<TSource, CancellationToken, ValueTask<TElement>>> elementSelector, IEqualityComparer<TKey>? comparer)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));
            if (keySelector == null)
                throw new ArgumentNullException(nameof(keySelector));
            if (elementSelector == null)
                throw new ArgumentNullException(nameof(elementSelector));

            return source.Provider.CreateQuery<IAsyncGrouping<TKey, TElement>>(Expression.Call(GroupByAwaitWithCancellation__TSource_TKey_TElement__4__0(typeof(TSource), typeof(TKey), typeof(TElement)), source.Expression, keySelector, elementSelector, Expression.Constant(comparer, typeof(IEqualityComparer<TKey>))));
        }

        private static MethodInfo? s_GroupByAwaitWithCancellation__TSource_TKey_TElement_TResult__4__0;
        
        private static MethodInfo? GroupByAwaitWithCancellation__TSource_TKey_TElement_TResult__4__0(Type TSource, Type TKey, Type TElement, Type TResult) =>
            (s_GroupByAwaitWithCancellation__TSource_TKey_TElement_TResult__4__0 ??
            (s_GroupByAwaitWithCancellation__TSource_TKey_TElement_TResult__4__0 = new Func<IAsyncQueryable<object>, Expression<Func<object, CancellationToken, ValueTask<object>>>, Expression<Func<object, CancellationToken, ValueTask<object>>>, Expression<Func<object, IAsyncEnumerable<object>, CancellationToken, ValueTask<object>>>, IAsyncQueryable<object>>(GroupByAwaitWithCancellation<object, object, object, object>).GetMethodInfo()!.GetGenericMethodDefinition())).MakeGenericMethod(TSource, TKey, TElement, TResult);

        public static IAsyncQueryable<TResult> GroupByAwaitWithCancellation<TSource, TKey, TElement, TResult>(this IAsyncQueryable<TSource> source, Expression<Func<TSource, CancellationToken, ValueTask<TKey>>> keySelector, Expression<Func<TSource, CancellationToken, ValueTask<TElement>>> elementSelector, Expression<Func<TKey, IAsyncEnumerable<TElement>, CancellationToken, ValueTask<TResult>>> resultSelector)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));
            if (keySelector == null)
                throw new ArgumentNullException(nameof(keySelector));
            if (elementSelector == null)
                throw new ArgumentNullException(nameof(elementSelector));
            if (resultSelector == null)
                throw new ArgumentNullException(nameof(resultSelector));

            return source.Provider.CreateQuery<TResult>(Expression.Call(GroupByAwaitWithCancellation__TSource_TKey_TElement_TResult__4__0(typeof(TSource), typeof(TKey), typeof(TElement), typeof(TResult)), source.Expression, keySelector, elementSelector, resultSelector));
        }

        private static MethodInfo? s_GroupByAwaitWithCancellation__TSource_TKey_TElement_TResult__5__0;
        
        private static MethodInfo? GroupByAwaitWithCancellation__TSource_TKey_TElement_TResult__5__0(Type TSource, Type TKey, Type TElement, Type TResult) =>
            (s_GroupByAwaitWithCancellation__TSource_TKey_TElement_TResult__5__0 ??
            (s_GroupByAwaitWithCancellation__TSource_TKey_TElement_TResult__5__0 = new Func<IAsyncQueryable<object>, Expression<Func<object, CancellationToken, ValueTask<object>>>, Expression<Func<object, CancellationToken, ValueTask<object>>>, Expression<Func<object, IAsyncEnumerable<object>, CancellationToken, ValueTask<object>>>, IEqualityComparer<object>, IAsyncQueryable<object>>(GroupByAwaitWithCancellation<object, object, object, object>).GetMethodInfo()!.GetGenericMethodDefinition())).MakeGenericMethod(TSource, TKey, TElement, TResult);

        public static IAsyncQueryable<TResult> GroupByAwaitWithCancellation<TSource, TKey, TElement, TResult>(this IAsyncQueryable<TSource> source, Expression<Func<TSource, CancellationToken, ValueTask<TKey>>> keySelector, Expression<Func<TSource, CancellationToken, ValueTask<TElement>>> elementSelector, Expression<Func<TKey, IAsyncEnumerable<TElement>, CancellationToken, ValueTask<TResult>>> resultSelector, IEqualityComparer<TKey>? comparer)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));
            if (keySelector == null)
                throw new ArgumentNullException(nameof(keySelector));
            if (elementSelector == null)
                throw new ArgumentNullException(nameof(elementSelector));
            if (resultSelector == null)
                throw new ArgumentNullException(nameof(resultSelector));

            return source.Provider.CreateQuery<TResult>(Expression.Call(GroupByAwaitWithCancellation__TSource_TKey_TElement_TResult__5__0(typeof(TSource), typeof(TKey), typeof(TElement), typeof(TResult)), source.Expression, keySelector, elementSelector, resultSelector, Expression.Constant(comparer, typeof(IEqualityComparer<TKey>))));
        }

        private static MethodInfo? s_GroupJoin__TOuter_TInner_TKey_TResult__5__0;
        
        private static MethodInfo? GroupJoin__TOuter_TInner_TKey_TResult__5__0(Type TOuter, Type TInner, Type TKey, Type TResult) =>
            (s_GroupJoin__TOuter_TInner_TKey_TResult__5__0 ??
            (s_GroupJoin__TOuter_TInner_TKey_TResult__5__0 = new Func<IAsyncQueryable<object>, IAsyncEnumerable<object>, Expression<Func<object, object>>, Expression<Func<object, object>>, Expression<Func<object, IAsyncEnumerable<object>, object>>, IAsyncQueryable<object>>(GroupJoin<object, object, object, object>).GetMethodInfo()!.GetGenericMethodDefinition())).MakeGenericMethod(TOuter, TInner, TKey, TResult);

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

            return outer.Provider.CreateQuery<TResult>(Expression.Call(GroupJoin__TOuter_TInner_TKey_TResult__5__0(typeof(TOuter), typeof(TInner), typeof(TKey), typeof(TResult)), outer.Expression, GetSourceExpression(inner), outerKeySelector, innerKeySelector, resultSelector));
        }

        private static MethodInfo? s_GroupJoin__TOuter_TInner_TKey_TResult__6__0;
        
        private static MethodInfo? GroupJoin__TOuter_TInner_TKey_TResult__6__0(Type TOuter, Type TInner, Type TKey, Type TResult) =>
            (s_GroupJoin__TOuter_TInner_TKey_TResult__6__0 ??
            (s_GroupJoin__TOuter_TInner_TKey_TResult__6__0 = new Func<IAsyncQueryable<object>, IAsyncEnumerable<object>, Expression<Func<object, object>>, Expression<Func<object, object>>, Expression<Func<object, IAsyncEnumerable<object>, object>>, IEqualityComparer<object>, IAsyncQueryable<object>>(GroupJoin<object, object, object, object>).GetMethodInfo()!.GetGenericMethodDefinition())).MakeGenericMethod(TOuter, TInner, TKey, TResult);

        public static IAsyncQueryable<TResult> GroupJoin<TOuter, TInner, TKey, TResult>(this IAsyncQueryable<TOuter> outer, IAsyncEnumerable<TInner> inner, Expression<Func<TOuter, TKey>> outerKeySelector, Expression<Func<TInner, TKey>> innerKeySelector, Expression<Func<TOuter, IAsyncEnumerable<TInner>, TResult>> resultSelector, IEqualityComparer<TKey>? comparer)
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

            return outer.Provider.CreateQuery<TResult>(Expression.Call(GroupJoin__TOuter_TInner_TKey_TResult__6__0(typeof(TOuter), typeof(TInner), typeof(TKey), typeof(TResult)), outer.Expression, GetSourceExpression(inner), outerKeySelector, innerKeySelector, resultSelector, Expression.Constant(comparer, typeof(IEqualityComparer<TKey>))));
        }

        private static MethodInfo? s_GroupJoinAwait__TOuter_TInner_TKey_TResult__5__0;
        
        private static MethodInfo? GroupJoinAwait__TOuter_TInner_TKey_TResult__5__0(Type TOuter, Type TInner, Type TKey, Type TResult) =>
            (s_GroupJoinAwait__TOuter_TInner_TKey_TResult__5__0 ??
            (s_GroupJoinAwait__TOuter_TInner_TKey_TResult__5__0 = new Func<IAsyncQueryable<object>, IAsyncEnumerable<object>, Expression<Func<object, ValueTask<object>>>, Expression<Func<object, ValueTask<object>>>, Expression<Func<object, IAsyncEnumerable<object>, ValueTask<object>>>, IAsyncQueryable<object>>(GroupJoinAwait<object, object, object, object>).GetMethodInfo()!.GetGenericMethodDefinition())).MakeGenericMethod(TOuter, TInner, TKey, TResult);

        public static IAsyncQueryable<TResult> GroupJoinAwait<TOuter, TInner, TKey, TResult>(this IAsyncQueryable<TOuter> outer, IAsyncEnumerable<TInner> inner, Expression<Func<TOuter, ValueTask<TKey>>> outerKeySelector, Expression<Func<TInner, ValueTask<TKey>>> innerKeySelector, Expression<Func<TOuter, IAsyncEnumerable<TInner>, ValueTask<TResult>>> resultSelector)
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

            return outer.Provider.CreateQuery<TResult>(Expression.Call(GroupJoinAwait__TOuter_TInner_TKey_TResult__5__0(typeof(TOuter), typeof(TInner), typeof(TKey), typeof(TResult)), outer.Expression, GetSourceExpression(inner), outerKeySelector, innerKeySelector, resultSelector));
        }

        private static MethodInfo? s_GroupJoinAwait__TOuter_TInner_TKey_TResult__6__0;
        
        private static MethodInfo? GroupJoinAwait__TOuter_TInner_TKey_TResult__6__0(Type TOuter, Type TInner, Type TKey, Type TResult) =>
            (s_GroupJoinAwait__TOuter_TInner_TKey_TResult__6__0 ??
            (s_GroupJoinAwait__TOuter_TInner_TKey_TResult__6__0 = new Func<IAsyncQueryable<object>, IAsyncEnumerable<object>, Expression<Func<object, ValueTask<object>>>, Expression<Func<object, ValueTask<object>>>, Expression<Func<object, IAsyncEnumerable<object>, ValueTask<object>>>, IEqualityComparer<object>, IAsyncQueryable<object>>(GroupJoinAwait<object, object, object, object>).GetMethodInfo()!.GetGenericMethodDefinition())).MakeGenericMethod(TOuter, TInner, TKey, TResult);

        public static IAsyncQueryable<TResult> GroupJoinAwait<TOuter, TInner, TKey, TResult>(this IAsyncQueryable<TOuter> outer, IAsyncEnumerable<TInner> inner, Expression<Func<TOuter, ValueTask<TKey>>> outerKeySelector, Expression<Func<TInner, ValueTask<TKey>>> innerKeySelector, Expression<Func<TOuter, IAsyncEnumerable<TInner>, ValueTask<TResult>>> resultSelector, IEqualityComparer<TKey>? comparer)
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

            return outer.Provider.CreateQuery<TResult>(Expression.Call(GroupJoinAwait__TOuter_TInner_TKey_TResult__6__0(typeof(TOuter), typeof(TInner), typeof(TKey), typeof(TResult)), outer.Expression, GetSourceExpression(inner), outerKeySelector, innerKeySelector, resultSelector, Expression.Constant(comparer, typeof(IEqualityComparer<TKey>))));
        }

        private static MethodInfo? s_GroupJoinAwaitWithCancellation__TOuter_TInner_TKey_TResult__5__0;
        
        private static MethodInfo? GroupJoinAwaitWithCancellation__TOuter_TInner_TKey_TResult__5__0(Type TOuter, Type TInner, Type TKey, Type TResult) =>
            (s_GroupJoinAwaitWithCancellation__TOuter_TInner_TKey_TResult__5__0 ??
            (s_GroupJoinAwaitWithCancellation__TOuter_TInner_TKey_TResult__5__0 = new Func<IAsyncQueryable<object>, IAsyncEnumerable<object>, Expression<Func<object, CancellationToken, ValueTask<object>>>, Expression<Func<object, CancellationToken, ValueTask<object>>>, Expression<Func<object, IAsyncEnumerable<object>, CancellationToken, ValueTask<object>>>, IAsyncQueryable<object>>(GroupJoinAwaitWithCancellation<object, object, object, object>).GetMethodInfo()!.GetGenericMethodDefinition())).MakeGenericMethod(TOuter, TInner, TKey, TResult);

        public static IAsyncQueryable<TResult> GroupJoinAwaitWithCancellation<TOuter, TInner, TKey, TResult>(this IAsyncQueryable<TOuter> outer, IAsyncEnumerable<TInner> inner, Expression<Func<TOuter, CancellationToken, ValueTask<TKey>>> outerKeySelector, Expression<Func<TInner, CancellationToken, ValueTask<TKey>>> innerKeySelector, Expression<Func<TOuter, IAsyncEnumerable<TInner>, CancellationToken, ValueTask<TResult>>> resultSelector)
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

            return outer.Provider.CreateQuery<TResult>(Expression.Call(GroupJoinAwaitWithCancellation__TOuter_TInner_TKey_TResult__5__0(typeof(TOuter), typeof(TInner), typeof(TKey), typeof(TResult)), outer.Expression, GetSourceExpression(inner), outerKeySelector, innerKeySelector, resultSelector));
        }

        private static MethodInfo? s_GroupJoinAwaitWithCancellation__TOuter_TInner_TKey_TResult__6__0;
        
        private static MethodInfo? GroupJoinAwaitWithCancellation__TOuter_TInner_TKey_TResult__6__0(Type TOuter, Type TInner, Type TKey, Type TResult) =>
            (s_GroupJoinAwaitWithCancellation__TOuter_TInner_TKey_TResult__6__0 ??
            (s_GroupJoinAwaitWithCancellation__TOuter_TInner_TKey_TResult__6__0 = new Func<IAsyncQueryable<object>, IAsyncEnumerable<object>, Expression<Func<object, CancellationToken, ValueTask<object>>>, Expression<Func<object, CancellationToken, ValueTask<object>>>, Expression<Func<object, IAsyncEnumerable<object>, CancellationToken, ValueTask<object>>>, IEqualityComparer<object>, IAsyncQueryable<object>>(GroupJoinAwaitWithCancellation<object, object, object, object>).GetMethodInfo()!.GetGenericMethodDefinition())).MakeGenericMethod(TOuter, TInner, TKey, TResult);

        public static IAsyncQueryable<TResult> GroupJoinAwaitWithCancellation<TOuter, TInner, TKey, TResult>(this IAsyncQueryable<TOuter> outer, IAsyncEnumerable<TInner> inner, Expression<Func<TOuter, CancellationToken, ValueTask<TKey>>> outerKeySelector, Expression<Func<TInner, CancellationToken, ValueTask<TKey>>> innerKeySelector, Expression<Func<TOuter, IAsyncEnumerable<TInner>, CancellationToken, ValueTask<TResult>>> resultSelector, IEqualityComparer<TKey>? comparer)
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

            return outer.Provider.CreateQuery<TResult>(Expression.Call(GroupJoinAwaitWithCancellation__TOuter_TInner_TKey_TResult__6__0(typeof(TOuter), typeof(TInner), typeof(TKey), typeof(TResult)), outer.Expression, GetSourceExpression(inner), outerKeySelector, innerKeySelector, resultSelector, Expression.Constant(comparer, typeof(IEqualityComparer<TKey>))));
        }

        private static MethodInfo? s_Intersect__TSource__2__0;
        
        private static MethodInfo? Intersect__TSource__2__0(Type TSource) =>
            (s_Intersect__TSource__2__0 ??
            (s_Intersect__TSource__2__0 = new Func<IAsyncQueryable<object>, IAsyncEnumerable<object>, IAsyncQueryable<object>>(Intersect<object>).GetMethodInfo()!.GetGenericMethodDefinition())).MakeGenericMethod(TSource);

        public static IAsyncQueryable<TSource> Intersect<TSource>(this IAsyncQueryable<TSource> first, IAsyncEnumerable<TSource> second)
        {
            if (first == null)
                throw new ArgumentNullException(nameof(first));
            if (second == null)
                throw new ArgumentNullException(nameof(second));

            return first.Provider.CreateQuery<TSource>(Expression.Call(Intersect__TSource__2__0(typeof(TSource)), first.Expression, GetSourceExpression(second)));
        }

        private static MethodInfo? s_Intersect__TSource__3__0;
        
        private static MethodInfo? Intersect__TSource__3__0(Type TSource) =>
            (s_Intersect__TSource__3__0 ??
            (s_Intersect__TSource__3__0 = new Func<IAsyncQueryable<object>, IAsyncEnumerable<object>, IEqualityComparer<object>, IAsyncQueryable<object>>(Intersect<object>).GetMethodInfo()!.GetGenericMethodDefinition())).MakeGenericMethod(TSource);

        public static IAsyncQueryable<TSource> Intersect<TSource>(this IAsyncQueryable<TSource> first, IAsyncEnumerable<TSource> second, IEqualityComparer<TSource>? comparer)
        {
            if (first == null)
                throw new ArgumentNullException(nameof(first));
            if (second == null)
                throw new ArgumentNullException(nameof(second));

            return first.Provider.CreateQuery<TSource>(Expression.Call(Intersect__TSource__3__0(typeof(TSource)), first.Expression, GetSourceExpression(second), Expression.Constant(comparer, typeof(IEqualityComparer<TSource>))));
        }

        private static MethodInfo? s_Join__TOuter_TInner_TKey_TResult__5__0;
        
        private static MethodInfo? Join__TOuter_TInner_TKey_TResult__5__0(Type TOuter, Type TInner, Type TKey, Type TResult) =>
            (s_Join__TOuter_TInner_TKey_TResult__5__0 ??
            (s_Join__TOuter_TInner_TKey_TResult__5__0 = new Func<IAsyncQueryable<object>, IAsyncEnumerable<object>, Expression<Func<object, object>>, Expression<Func<object, object>>, Expression<Func<object, object, object>>, IAsyncQueryable<object>>(Join<object, object, object, object>).GetMethodInfo()!.GetGenericMethodDefinition())).MakeGenericMethod(TOuter, TInner, TKey, TResult);

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

            return outer.Provider.CreateQuery<TResult>(Expression.Call(Join__TOuter_TInner_TKey_TResult__5__0(typeof(TOuter), typeof(TInner), typeof(TKey), typeof(TResult)), outer.Expression, GetSourceExpression(inner), outerKeySelector, innerKeySelector, resultSelector));
        }

        private static MethodInfo? s_Join__TOuter_TInner_TKey_TResult__6__0;
        
        private static MethodInfo? Join__TOuter_TInner_TKey_TResult__6__0(Type TOuter, Type TInner, Type TKey, Type TResult) =>
            (s_Join__TOuter_TInner_TKey_TResult__6__0 ??
            (s_Join__TOuter_TInner_TKey_TResult__6__0 = new Func<IAsyncQueryable<object>, IAsyncEnumerable<object>, Expression<Func<object, object>>, Expression<Func<object, object>>, Expression<Func<object, object, object>>, IEqualityComparer<object>, IAsyncQueryable<object>>(Join<object, object, object, object>).GetMethodInfo()!.GetGenericMethodDefinition())).MakeGenericMethod(TOuter, TInner, TKey, TResult);

        public static IAsyncQueryable<TResult> Join<TOuter, TInner, TKey, TResult>(this IAsyncQueryable<TOuter> outer, IAsyncEnumerable<TInner> inner, Expression<Func<TOuter, TKey>> outerKeySelector, Expression<Func<TInner, TKey>> innerKeySelector, Expression<Func<TOuter, TInner, TResult>> resultSelector, IEqualityComparer<TKey>? comparer)
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

            return outer.Provider.CreateQuery<TResult>(Expression.Call(Join__TOuter_TInner_TKey_TResult__6__0(typeof(TOuter), typeof(TInner), typeof(TKey), typeof(TResult)), outer.Expression, GetSourceExpression(inner), outerKeySelector, innerKeySelector, resultSelector, Expression.Constant(comparer, typeof(IEqualityComparer<TKey>))));
        }

        private static MethodInfo? s_JoinAwait__TOuter_TInner_TKey_TResult__5__0;
        
        private static MethodInfo? JoinAwait__TOuter_TInner_TKey_TResult__5__0(Type TOuter, Type TInner, Type TKey, Type TResult) =>
            (s_JoinAwait__TOuter_TInner_TKey_TResult__5__0 ??
            (s_JoinAwait__TOuter_TInner_TKey_TResult__5__0 = new Func<IAsyncQueryable<object>, IAsyncEnumerable<object>, Expression<Func<object, ValueTask<object>>>, Expression<Func<object, ValueTask<object>>>, Expression<Func<object, object, ValueTask<object>>>, IAsyncQueryable<object>>(JoinAwait<object, object, object, object>).GetMethodInfo()!.GetGenericMethodDefinition())).MakeGenericMethod(TOuter, TInner, TKey, TResult);

        public static IAsyncQueryable<TResult> JoinAwait<TOuter, TInner, TKey, TResult>(this IAsyncQueryable<TOuter> outer, IAsyncEnumerable<TInner> inner, Expression<Func<TOuter, ValueTask<TKey>>> outerKeySelector, Expression<Func<TInner, ValueTask<TKey>>> innerKeySelector, Expression<Func<TOuter, TInner, ValueTask<TResult>>> resultSelector)
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

            return outer.Provider.CreateQuery<TResult>(Expression.Call(JoinAwait__TOuter_TInner_TKey_TResult__5__0(typeof(TOuter), typeof(TInner), typeof(TKey), typeof(TResult)), outer.Expression, GetSourceExpression(inner), outerKeySelector, innerKeySelector, resultSelector));
        }

        private static MethodInfo? s_JoinAwait__TOuter_TInner_TKey_TResult__6__0;
        
        private static MethodInfo? JoinAwait__TOuter_TInner_TKey_TResult__6__0(Type TOuter, Type TInner, Type TKey, Type TResult) =>
            (s_JoinAwait__TOuter_TInner_TKey_TResult__6__0 ??
            (s_JoinAwait__TOuter_TInner_TKey_TResult__6__0 = new Func<IAsyncQueryable<object>, IAsyncEnumerable<object>, Expression<Func<object, ValueTask<object>>>, Expression<Func<object, ValueTask<object>>>, Expression<Func<object, object, ValueTask<object>>>, IEqualityComparer<object>, IAsyncQueryable<object>>(JoinAwait<object, object, object, object>).GetMethodInfo()!.GetGenericMethodDefinition())).MakeGenericMethod(TOuter, TInner, TKey, TResult);

        public static IAsyncQueryable<TResult> JoinAwait<TOuter, TInner, TKey, TResult>(this IAsyncQueryable<TOuter> outer, IAsyncEnumerable<TInner> inner, Expression<Func<TOuter, ValueTask<TKey>>> outerKeySelector, Expression<Func<TInner, ValueTask<TKey>>> innerKeySelector, Expression<Func<TOuter, TInner, ValueTask<TResult>>> resultSelector, IEqualityComparer<TKey>? comparer)
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

            return outer.Provider.CreateQuery<TResult>(Expression.Call(JoinAwait__TOuter_TInner_TKey_TResult__6__0(typeof(TOuter), typeof(TInner), typeof(TKey), typeof(TResult)), outer.Expression, GetSourceExpression(inner), outerKeySelector, innerKeySelector, resultSelector, Expression.Constant(comparer, typeof(IEqualityComparer<TKey>))));
        }

        private static MethodInfo? s_JoinAwaitWithCancellation__TOuter_TInner_TKey_TResult__5__0;
        
        private static MethodInfo? JoinAwaitWithCancellation__TOuter_TInner_TKey_TResult__5__0(Type TOuter, Type TInner, Type TKey, Type TResult) =>
            (s_JoinAwaitWithCancellation__TOuter_TInner_TKey_TResult__5__0 ??
            (s_JoinAwaitWithCancellation__TOuter_TInner_TKey_TResult__5__0 = new Func<IAsyncQueryable<object>, IAsyncEnumerable<object>, Expression<Func<object, CancellationToken, ValueTask<object>>>, Expression<Func<object, CancellationToken, ValueTask<object>>>, Expression<Func<object, object, CancellationToken, ValueTask<object>>>, IAsyncQueryable<object>>(JoinAwaitWithCancellation<object, object, object, object>).GetMethodInfo()!.GetGenericMethodDefinition())).MakeGenericMethod(TOuter, TInner, TKey, TResult);

        public static IAsyncQueryable<TResult> JoinAwaitWithCancellation<TOuter, TInner, TKey, TResult>(this IAsyncQueryable<TOuter> outer, IAsyncEnumerable<TInner> inner, Expression<Func<TOuter, CancellationToken, ValueTask<TKey>>> outerKeySelector, Expression<Func<TInner, CancellationToken, ValueTask<TKey>>> innerKeySelector, Expression<Func<TOuter, TInner, CancellationToken, ValueTask<TResult>>> resultSelector)
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

            return outer.Provider.CreateQuery<TResult>(Expression.Call(JoinAwaitWithCancellation__TOuter_TInner_TKey_TResult__5__0(typeof(TOuter), typeof(TInner), typeof(TKey), typeof(TResult)), outer.Expression, GetSourceExpression(inner), outerKeySelector, innerKeySelector, resultSelector));
        }

        private static MethodInfo? s_JoinAwaitWithCancellation__TOuter_TInner_TKey_TResult__6__0;
        
        private static MethodInfo? JoinAwaitWithCancellation__TOuter_TInner_TKey_TResult__6__0(Type TOuter, Type TInner, Type TKey, Type TResult) =>
            (s_JoinAwaitWithCancellation__TOuter_TInner_TKey_TResult__6__0 ??
            (s_JoinAwaitWithCancellation__TOuter_TInner_TKey_TResult__6__0 = new Func<IAsyncQueryable<object>, IAsyncEnumerable<object>, Expression<Func<object, CancellationToken, ValueTask<object>>>, Expression<Func<object, CancellationToken, ValueTask<object>>>, Expression<Func<object, object, CancellationToken, ValueTask<object>>>, IEqualityComparer<object>, IAsyncQueryable<object>>(JoinAwaitWithCancellation<object, object, object, object>).GetMethodInfo()!.GetGenericMethodDefinition())).MakeGenericMethod(TOuter, TInner, TKey, TResult);

        public static IAsyncQueryable<TResult> JoinAwaitWithCancellation<TOuter, TInner, TKey, TResult>(this IAsyncQueryable<TOuter> outer, IAsyncEnumerable<TInner> inner, Expression<Func<TOuter, CancellationToken, ValueTask<TKey>>> outerKeySelector, Expression<Func<TInner, CancellationToken, ValueTask<TKey>>> innerKeySelector, Expression<Func<TOuter, TInner, CancellationToken, ValueTask<TResult>>> resultSelector, IEqualityComparer<TKey>? comparer)
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

            return outer.Provider.CreateQuery<TResult>(Expression.Call(JoinAwaitWithCancellation__TOuter_TInner_TKey_TResult__6__0(typeof(TOuter), typeof(TInner), typeof(TKey), typeof(TResult)), outer.Expression, GetSourceExpression(inner), outerKeySelector, innerKeySelector, resultSelector, Expression.Constant(comparer, typeof(IEqualityComparer<TKey>))));
        }

        private static MethodInfo? s_LastAsync__TSource__2__0;
        
        private static MethodInfo? LastAsync__TSource__2__0(Type TSource) =>
            (s_LastAsync__TSource__2__0 ??
            (s_LastAsync__TSource__2__0 = new Func<IAsyncQueryable<object>, CancellationToken, ValueTask<object>>(LastAsync<object>).GetMethodInfo()!.GetGenericMethodDefinition())).MakeGenericMethod(TSource);

        public static ValueTask<TSource> LastAsync<TSource>(this IAsyncQueryable<TSource> source, CancellationToken cancellationToken = default)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));

            return source.Provider.ExecuteAsync<TSource>(Expression.Call(LastAsync__TSource__2__0(typeof(TSource)), source.Expression, Expression.Constant(cancellationToken, typeof(CancellationToken))), cancellationToken);
        }

        private static MethodInfo? s_LastAsync__TSource__3__0;
        
        private static MethodInfo? LastAsync__TSource__3__0(Type TSource) =>
            (s_LastAsync__TSource__3__0 ??
            (s_LastAsync__TSource__3__0 = new Func<IAsyncQueryable<object>, Expression<Func<object, bool>>, CancellationToken, ValueTask<object>>(LastAsync<object>).GetMethodInfo()!.GetGenericMethodDefinition())).MakeGenericMethod(TSource);

        public static ValueTask<TSource> LastAsync<TSource>(this IAsyncQueryable<TSource> source, Expression<Func<TSource, bool>> predicate, CancellationToken cancellationToken = default)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));
            if (predicate == null)
                throw new ArgumentNullException(nameof(predicate));

            return source.Provider.ExecuteAsync<TSource>(Expression.Call(LastAsync__TSource__3__0(typeof(TSource)), source.Expression, predicate, Expression.Constant(cancellationToken, typeof(CancellationToken))), cancellationToken);
        }

        private static MethodInfo? s_LastAwaitAsync__TSource__3__0;
        
        private static MethodInfo? LastAwaitAsync__TSource__3__0(Type TSource) =>
            (s_LastAwaitAsync__TSource__3__0 ??
            (s_LastAwaitAsync__TSource__3__0 = new Func<IAsyncQueryable<object>, Expression<Func<object, ValueTask<bool>>>, CancellationToken, ValueTask<object>>(LastAwaitAsync<object>).GetMethodInfo()!.GetGenericMethodDefinition())).MakeGenericMethod(TSource);

        public static ValueTask<TSource> LastAwaitAsync<TSource>(this IAsyncQueryable<TSource> source, Expression<Func<TSource, ValueTask<bool>>> predicate, CancellationToken cancellationToken = default)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));
            if (predicate == null)
                throw new ArgumentNullException(nameof(predicate));

            return source.Provider.ExecuteAsync<TSource>(Expression.Call(LastAwaitAsync__TSource__3__0(typeof(TSource)), source.Expression, predicate, Expression.Constant(cancellationToken, typeof(CancellationToken))), cancellationToken);
        }

        private static MethodInfo? s_LastAwaitWithCancellationAsync__TSource__3__0;
        
        private static MethodInfo? LastAwaitWithCancellationAsync__TSource__3__0(Type TSource) =>
            (s_LastAwaitWithCancellationAsync__TSource__3__0 ??
            (s_LastAwaitWithCancellationAsync__TSource__3__0 = new Func<IAsyncQueryable<object>, Expression<Func<object, CancellationToken, ValueTask<bool>>>, CancellationToken, ValueTask<object>>(LastAwaitWithCancellationAsync<object>).GetMethodInfo()!.GetGenericMethodDefinition())).MakeGenericMethod(TSource);

        public static ValueTask<TSource> LastAwaitWithCancellationAsync<TSource>(this IAsyncQueryable<TSource> source, Expression<Func<TSource, CancellationToken, ValueTask<bool>>> predicate, CancellationToken cancellationToken = default)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));
            if (predicate == null)
                throw new ArgumentNullException(nameof(predicate));

            return source.Provider.ExecuteAsync<TSource>(Expression.Call(LastAwaitWithCancellationAsync__TSource__3__0(typeof(TSource)), source.Expression, predicate, Expression.Constant(cancellationToken, typeof(CancellationToken))), cancellationToken);
        }

        private static MethodInfo? s_LastOrDefaultAsync__TSource__2__0;
        
        private static MethodInfo? LastOrDefaultAsync__TSource__2__0(Type TSource) =>
            (s_LastOrDefaultAsync__TSource__2__0 ??
            (s_LastOrDefaultAsync__TSource__2__0 = new Func<IAsyncQueryable<object>, CancellationToken, ValueTask<object>>(LastOrDefaultAsync<object>).GetMethodInfo()!.GetGenericMethodDefinition())).MakeGenericMethod(TSource);

        public static ValueTask<TSource> LastOrDefaultAsync<TSource>(this IAsyncQueryable<TSource> source, CancellationToken cancellationToken = default)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));

            return source.Provider.ExecuteAsync<TSource>(Expression.Call(LastOrDefaultAsync__TSource__2__0(typeof(TSource)), source.Expression, Expression.Constant(cancellationToken, typeof(CancellationToken))), cancellationToken);
        }

        private static MethodInfo? s_LastOrDefaultAsync__TSource__3__0;
        
        private static MethodInfo? LastOrDefaultAsync__TSource__3__0(Type TSource) =>
            (s_LastOrDefaultAsync__TSource__3__0 ??
            (s_LastOrDefaultAsync__TSource__3__0 = new Func<IAsyncQueryable<object>, Expression<Func<object, bool>>, CancellationToken, ValueTask<object>>(LastOrDefaultAsync<object>).GetMethodInfo()!.GetGenericMethodDefinition())).MakeGenericMethod(TSource);

        public static ValueTask<TSource> LastOrDefaultAsync<TSource>(this IAsyncQueryable<TSource> source, Expression<Func<TSource, bool>> predicate, CancellationToken cancellationToken = default)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));
            if (predicate == null)
                throw new ArgumentNullException(nameof(predicate));

            return source.Provider.ExecuteAsync<TSource>(Expression.Call(LastOrDefaultAsync__TSource__3__0(typeof(TSource)), source.Expression, predicate, Expression.Constant(cancellationToken, typeof(CancellationToken))), cancellationToken);
        }

        private static MethodInfo? s_LastOrDefaultAwaitAsync__TSource__3__0;
        
        private static MethodInfo? LastOrDefaultAwaitAsync__TSource__3__0(Type TSource) =>
            (s_LastOrDefaultAwaitAsync__TSource__3__0 ??
            (s_LastOrDefaultAwaitAsync__TSource__3__0 = new Func<IAsyncQueryable<object>, Expression<Func<object, ValueTask<bool>>>, CancellationToken, ValueTask<object>>(LastOrDefaultAwaitAsync<object>).GetMethodInfo()!.GetGenericMethodDefinition())).MakeGenericMethod(TSource);

        public static ValueTask<TSource> LastOrDefaultAwaitAsync<TSource>(this IAsyncQueryable<TSource> source, Expression<Func<TSource, ValueTask<bool>>> predicate, CancellationToken cancellationToken = default)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));
            if (predicate == null)
                throw new ArgumentNullException(nameof(predicate));

            return source.Provider.ExecuteAsync<TSource>(Expression.Call(LastOrDefaultAwaitAsync__TSource__3__0(typeof(TSource)), source.Expression, predicate, Expression.Constant(cancellationToken, typeof(CancellationToken))), cancellationToken);
        }

        private static MethodInfo? s_LastOrDefaultAwaitWithCancellationAsync__TSource__3__0;
        
        private static MethodInfo? LastOrDefaultAwaitWithCancellationAsync__TSource__3__0(Type TSource) =>
            (s_LastOrDefaultAwaitWithCancellationAsync__TSource__3__0 ??
            (s_LastOrDefaultAwaitWithCancellationAsync__TSource__3__0 = new Func<IAsyncQueryable<object>, Expression<Func<object, CancellationToken, ValueTask<bool>>>, CancellationToken, ValueTask<object>>(LastOrDefaultAwaitWithCancellationAsync<object>).GetMethodInfo()!.GetGenericMethodDefinition())).MakeGenericMethod(TSource);

        public static ValueTask<TSource> LastOrDefaultAwaitWithCancellationAsync<TSource>(this IAsyncQueryable<TSource> source, Expression<Func<TSource, CancellationToken, ValueTask<bool>>> predicate, CancellationToken cancellationToken = default)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));
            if (predicate == null)
                throw new ArgumentNullException(nameof(predicate));

            return source.Provider.ExecuteAsync<TSource>(Expression.Call(LastOrDefaultAwaitWithCancellationAsync__TSource__3__0(typeof(TSource)), source.Expression, predicate, Expression.Constant(cancellationToken, typeof(CancellationToken))), cancellationToken);
        }

        private static MethodInfo? s_LongCountAsync__TSource__2__0;
        
        private static MethodInfo? LongCountAsync__TSource__2__0(Type TSource) =>
            (s_LongCountAsync__TSource__2__0 ??
            (s_LongCountAsync__TSource__2__0 = new Func<IAsyncQueryable<object>, CancellationToken, ValueTask<long>>(LongCountAsync<object>).GetMethodInfo()!.GetGenericMethodDefinition())).MakeGenericMethod(TSource);

        public static ValueTask<long> LongCountAsync<TSource>(this IAsyncQueryable<TSource> source, CancellationToken cancellationToken = default)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));

            return source.Provider.ExecuteAsync<long>(Expression.Call(LongCountAsync__TSource__2__0(typeof(TSource)), source.Expression, Expression.Constant(cancellationToken, typeof(CancellationToken))), cancellationToken);
        }

        private static MethodInfo? s_LongCountAsync__TSource__3__0;
        
        private static MethodInfo? LongCountAsync__TSource__3__0(Type TSource) =>
            (s_LongCountAsync__TSource__3__0 ??
            (s_LongCountAsync__TSource__3__0 = new Func<IAsyncQueryable<object>, Expression<Func<object, bool>>, CancellationToken, ValueTask<long>>(LongCountAsync<object>).GetMethodInfo()!.GetGenericMethodDefinition())).MakeGenericMethod(TSource);

        public static ValueTask<long> LongCountAsync<TSource>(this IAsyncQueryable<TSource> source, Expression<Func<TSource, bool>> predicate, CancellationToken cancellationToken = default)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));
            if (predicate == null)
                throw new ArgumentNullException(nameof(predicate));

            return source.Provider.ExecuteAsync<long>(Expression.Call(LongCountAsync__TSource__3__0(typeof(TSource)), source.Expression, predicate, Expression.Constant(cancellationToken, typeof(CancellationToken))), cancellationToken);
        }

        private static MethodInfo? s_LongCountAwaitAsync__TSource__3__0;
        
        private static MethodInfo? LongCountAwaitAsync__TSource__3__0(Type TSource) =>
            (s_LongCountAwaitAsync__TSource__3__0 ??
            (s_LongCountAwaitAsync__TSource__3__0 = new Func<IAsyncQueryable<object>, Expression<Func<object, ValueTask<bool>>>, CancellationToken, ValueTask<long>>(LongCountAwaitAsync<object>).GetMethodInfo()!.GetGenericMethodDefinition())).MakeGenericMethod(TSource);

        public static ValueTask<long> LongCountAwaitAsync<TSource>(this IAsyncQueryable<TSource> source, Expression<Func<TSource, ValueTask<bool>>> predicate, CancellationToken cancellationToken = default)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));
            if (predicate == null)
                throw new ArgumentNullException(nameof(predicate));

            return source.Provider.ExecuteAsync<long>(Expression.Call(LongCountAwaitAsync__TSource__3__0(typeof(TSource)), source.Expression, predicate, Expression.Constant(cancellationToken, typeof(CancellationToken))), cancellationToken);
        }

        private static MethodInfo? s_LongCountAwaitWithCancellationAsync__TSource__3__0;
        
        private static MethodInfo? LongCountAwaitWithCancellationAsync__TSource__3__0(Type TSource) =>
            (s_LongCountAwaitWithCancellationAsync__TSource__3__0 ??
            (s_LongCountAwaitWithCancellationAsync__TSource__3__0 = new Func<IAsyncQueryable<object>, Expression<Func<object, CancellationToken, ValueTask<bool>>>, CancellationToken, ValueTask<long>>(LongCountAwaitWithCancellationAsync<object>).GetMethodInfo()!.GetGenericMethodDefinition())).MakeGenericMethod(TSource);

        public static ValueTask<long> LongCountAwaitWithCancellationAsync<TSource>(this IAsyncQueryable<TSource> source, Expression<Func<TSource, CancellationToken, ValueTask<bool>>> predicate, CancellationToken cancellationToken = default)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));
            if (predicate == null)
                throw new ArgumentNullException(nameof(predicate));

            return source.Provider.ExecuteAsync<long>(Expression.Call(LongCountAwaitWithCancellationAsync__TSource__3__0(typeof(TSource)), source.Expression, predicate, Expression.Constant(cancellationToken, typeof(CancellationToken))), cancellationToken);
        }

        private static MethodInfo? s_MaxAsync__2__0;
        
        private static MethodInfo? MaxAsync__2__0 =>
            (s_MaxAsync__2__0 ??
            (s_MaxAsync__2__0 = new Func<IAsyncQueryable<decimal?>, CancellationToken, ValueTask<decimal?>>(MaxAsync).GetMethodInfo()));

        public static ValueTask<decimal?> MaxAsync(this IAsyncQueryable<decimal?> source, CancellationToken cancellationToken = default)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));

            return source.Provider.ExecuteAsync<decimal?>(Expression.Call(MaxAsync__2__0, source.Expression, Expression.Constant(cancellationToken, typeof(CancellationToken))), cancellationToken);
        }

        private static MethodInfo? s_MaxAsync__2__1;
        
        private static MethodInfo? MaxAsync__2__1 =>
            (s_MaxAsync__2__1 ??
            (s_MaxAsync__2__1 = new Func<IAsyncQueryable<decimal>, CancellationToken, ValueTask<decimal>>(MaxAsync).GetMethodInfo()));

        public static ValueTask<decimal> MaxAsync(this IAsyncQueryable<decimal> source, CancellationToken cancellationToken = default)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));

            return source.Provider.ExecuteAsync<decimal>(Expression.Call(MaxAsync__2__1, source.Expression, Expression.Constant(cancellationToken, typeof(CancellationToken))), cancellationToken);
        }

        private static MethodInfo? s_MaxAsync__2__2;
        
        private static MethodInfo? MaxAsync__2__2 =>
            (s_MaxAsync__2__2 ??
            (s_MaxAsync__2__2 = new Func<IAsyncQueryable<double?>, CancellationToken, ValueTask<double?>>(MaxAsync).GetMethodInfo()));

        public static ValueTask<double?> MaxAsync(this IAsyncQueryable<double?> source, CancellationToken cancellationToken = default)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));

            return source.Provider.ExecuteAsync<double?>(Expression.Call(MaxAsync__2__2, source.Expression, Expression.Constant(cancellationToken, typeof(CancellationToken))), cancellationToken);
        }

        private static MethodInfo? s_MaxAsync__2__3;
        
        private static MethodInfo? MaxAsync__2__3 =>
            (s_MaxAsync__2__3 ??
            (s_MaxAsync__2__3 = new Func<IAsyncQueryable<double>, CancellationToken, ValueTask<double>>(MaxAsync).GetMethodInfo()));

        public static ValueTask<double> MaxAsync(this IAsyncQueryable<double> source, CancellationToken cancellationToken = default)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));

            return source.Provider.ExecuteAsync<double>(Expression.Call(MaxAsync__2__3, source.Expression, Expression.Constant(cancellationToken, typeof(CancellationToken))), cancellationToken);
        }

        private static MethodInfo? s_MaxAsync__2__4;
        
        private static MethodInfo? MaxAsync__2__4 =>
            (s_MaxAsync__2__4 ??
            (s_MaxAsync__2__4 = new Func<IAsyncQueryable<float?>, CancellationToken, ValueTask<float?>>(MaxAsync).GetMethodInfo()));

        public static ValueTask<float?> MaxAsync(this IAsyncQueryable<float?> source, CancellationToken cancellationToken = default)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));

            return source.Provider.ExecuteAsync<float?>(Expression.Call(MaxAsync__2__4, source.Expression, Expression.Constant(cancellationToken, typeof(CancellationToken))), cancellationToken);
        }

        private static MethodInfo? s_MaxAsync__2__5;
        
        private static MethodInfo? MaxAsync__2__5 =>
            (s_MaxAsync__2__5 ??
            (s_MaxAsync__2__5 = new Func<IAsyncQueryable<float>, CancellationToken, ValueTask<float>>(MaxAsync).GetMethodInfo()));

        public static ValueTask<float> MaxAsync(this IAsyncQueryable<float> source, CancellationToken cancellationToken = default)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));

            return source.Provider.ExecuteAsync<float>(Expression.Call(MaxAsync__2__5, source.Expression, Expression.Constant(cancellationToken, typeof(CancellationToken))), cancellationToken);
        }

        private static MethodInfo? s_MaxAsync__2__6;
        
        private static MethodInfo? MaxAsync__2__6 =>
            (s_MaxAsync__2__6 ??
            (s_MaxAsync__2__6 = new Func<IAsyncQueryable<int?>, CancellationToken, ValueTask<int?>>(MaxAsync).GetMethodInfo()));

        public static ValueTask<int?> MaxAsync(this IAsyncQueryable<int?> source, CancellationToken cancellationToken = default)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));

            return source.Provider.ExecuteAsync<int?>(Expression.Call(MaxAsync__2__6, source.Expression, Expression.Constant(cancellationToken, typeof(CancellationToken))), cancellationToken);
        }

        private static MethodInfo? s_MaxAsync__2__7;
        
        private static MethodInfo? MaxAsync__2__7 =>
            (s_MaxAsync__2__7 ??
            (s_MaxAsync__2__7 = new Func<IAsyncQueryable<int>, CancellationToken, ValueTask<int>>(MaxAsync).GetMethodInfo()));

        public static ValueTask<int> MaxAsync(this IAsyncQueryable<int> source, CancellationToken cancellationToken = default)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));

            return source.Provider.ExecuteAsync<int>(Expression.Call(MaxAsync__2__7, source.Expression, Expression.Constant(cancellationToken, typeof(CancellationToken))), cancellationToken);
        }

        private static MethodInfo? s_MaxAsync__2__8;
        
        private static MethodInfo? MaxAsync__2__8 =>
            (s_MaxAsync__2__8 ??
            (s_MaxAsync__2__8 = new Func<IAsyncQueryable<long?>, CancellationToken, ValueTask<long?>>(MaxAsync).GetMethodInfo()));

        public static ValueTask<long?> MaxAsync(this IAsyncQueryable<long?> source, CancellationToken cancellationToken = default)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));

            return source.Provider.ExecuteAsync<long?>(Expression.Call(MaxAsync__2__8, source.Expression, Expression.Constant(cancellationToken, typeof(CancellationToken))), cancellationToken);
        }

        private static MethodInfo? s_MaxAsync__2__9;
        
        private static MethodInfo? MaxAsync__2__9 =>
            (s_MaxAsync__2__9 ??
            (s_MaxAsync__2__9 = new Func<IAsyncQueryable<long>, CancellationToken, ValueTask<long>>(MaxAsync).GetMethodInfo()));

        public static ValueTask<long> MaxAsync(this IAsyncQueryable<long> source, CancellationToken cancellationToken = default)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));

            return source.Provider.ExecuteAsync<long>(Expression.Call(MaxAsync__2__9, source.Expression, Expression.Constant(cancellationToken, typeof(CancellationToken))), cancellationToken);
        }

        private static MethodInfo? s_MaxAsync__TSource__2__0;
        
        private static MethodInfo? MaxAsync__TSource__2__0(Type TSource) =>
            (s_MaxAsync__TSource__2__0 ??
            (s_MaxAsync__TSource__2__0 = new Func<IAsyncQueryable<object>, CancellationToken, ValueTask<object>>(MaxAsync<object>).GetMethodInfo()!.GetGenericMethodDefinition())).MakeGenericMethod(TSource);

        public static ValueTask<TSource> MaxAsync<TSource>(this IAsyncQueryable<TSource> source, CancellationToken cancellationToken = default)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));

            return source.Provider.ExecuteAsync<TSource>(Expression.Call(MaxAsync__TSource__2__0(typeof(TSource)), source.Expression, Expression.Constant(cancellationToken, typeof(CancellationToken))), cancellationToken);
        }

        private static MethodInfo? s_MaxAsync__TSource__3__0;
        
        private static MethodInfo? MaxAsync__TSource__3__0(Type TSource) =>
            (s_MaxAsync__TSource__3__0 ??
            (s_MaxAsync__TSource__3__0 = new Func<IAsyncQueryable<object>, Expression<Func<object, decimal?>>, CancellationToken, ValueTask<decimal?>>(MaxAsync<object>).GetMethodInfo()!.GetGenericMethodDefinition())).MakeGenericMethod(TSource);

        public static ValueTask<decimal?> MaxAsync<TSource>(this IAsyncQueryable<TSource> source, Expression<Func<TSource, decimal?>> selector, CancellationToken cancellationToken = default)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));
            if (selector == null)
                throw new ArgumentNullException(nameof(selector));

            return source.Provider.ExecuteAsync<decimal?>(Expression.Call(MaxAsync__TSource__3__0(typeof(TSource)), source.Expression, selector, Expression.Constant(cancellationToken, typeof(CancellationToken))), cancellationToken);
        }

        private static MethodInfo? s_MaxAsync__TSource__3__1;
        
        private static MethodInfo? MaxAsync__TSource__3__1(Type TSource) =>
            (s_MaxAsync__TSource__3__1 ??
            (s_MaxAsync__TSource__3__1 = new Func<IAsyncQueryable<object>, Expression<Func<object, decimal>>, CancellationToken, ValueTask<decimal>>(MaxAsync<object>).GetMethodInfo()!.GetGenericMethodDefinition())).MakeGenericMethod(TSource);

        public static ValueTask<decimal> MaxAsync<TSource>(this IAsyncQueryable<TSource> source, Expression<Func<TSource, decimal>> selector, CancellationToken cancellationToken = default)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));
            if (selector == null)
                throw new ArgumentNullException(nameof(selector));

            return source.Provider.ExecuteAsync<decimal>(Expression.Call(MaxAsync__TSource__3__1(typeof(TSource)), source.Expression, selector, Expression.Constant(cancellationToken, typeof(CancellationToken))), cancellationToken);
        }

        private static MethodInfo? s_MaxAsync__TSource__3__2;
        
        private static MethodInfo? MaxAsync__TSource__3__2(Type TSource) =>
            (s_MaxAsync__TSource__3__2 ??
            (s_MaxAsync__TSource__3__2 = new Func<IAsyncQueryable<object>, Expression<Func<object, double?>>, CancellationToken, ValueTask<double?>>(MaxAsync<object>).GetMethodInfo()!.GetGenericMethodDefinition())).MakeGenericMethod(TSource);

        public static ValueTask<double?> MaxAsync<TSource>(this IAsyncQueryable<TSource> source, Expression<Func<TSource, double?>> selector, CancellationToken cancellationToken = default)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));
            if (selector == null)
                throw new ArgumentNullException(nameof(selector));

            return source.Provider.ExecuteAsync<double?>(Expression.Call(MaxAsync__TSource__3__2(typeof(TSource)), source.Expression, selector, Expression.Constant(cancellationToken, typeof(CancellationToken))), cancellationToken);
        }

        private static MethodInfo? s_MaxAsync__TSource__3__3;
        
        private static MethodInfo? MaxAsync__TSource__3__3(Type TSource) =>
            (s_MaxAsync__TSource__3__3 ??
            (s_MaxAsync__TSource__3__3 = new Func<IAsyncQueryable<object>, Expression<Func<object, double>>, CancellationToken, ValueTask<double>>(MaxAsync<object>).GetMethodInfo()!.GetGenericMethodDefinition())).MakeGenericMethod(TSource);

        public static ValueTask<double> MaxAsync<TSource>(this IAsyncQueryable<TSource> source, Expression<Func<TSource, double>> selector, CancellationToken cancellationToken = default)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));
            if (selector == null)
                throw new ArgumentNullException(nameof(selector));

            return source.Provider.ExecuteAsync<double>(Expression.Call(MaxAsync__TSource__3__3(typeof(TSource)), source.Expression, selector, Expression.Constant(cancellationToken, typeof(CancellationToken))), cancellationToken);
        }

        private static MethodInfo? s_MaxAsync__TSource__3__4;
        
        private static MethodInfo? MaxAsync__TSource__3__4(Type TSource) =>
            (s_MaxAsync__TSource__3__4 ??
            (s_MaxAsync__TSource__3__4 = new Func<IAsyncQueryable<object>, Expression<Func<object, float?>>, CancellationToken, ValueTask<float?>>(MaxAsync<object>).GetMethodInfo()!.GetGenericMethodDefinition())).MakeGenericMethod(TSource);

        public static ValueTask<float?> MaxAsync<TSource>(this IAsyncQueryable<TSource> source, Expression<Func<TSource, float?>> selector, CancellationToken cancellationToken = default)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));
            if (selector == null)
                throw new ArgumentNullException(nameof(selector));

            return source.Provider.ExecuteAsync<float?>(Expression.Call(MaxAsync__TSource__3__4(typeof(TSource)), source.Expression, selector, Expression.Constant(cancellationToken, typeof(CancellationToken))), cancellationToken);
        }

        private static MethodInfo? s_MaxAsync__TSource__3__5;
        
        private static MethodInfo? MaxAsync__TSource__3__5(Type TSource) =>
            (s_MaxAsync__TSource__3__5 ??
            (s_MaxAsync__TSource__3__5 = new Func<IAsyncQueryable<object>, Expression<Func<object, float>>, CancellationToken, ValueTask<float>>(MaxAsync<object>).GetMethodInfo()!.GetGenericMethodDefinition())).MakeGenericMethod(TSource);

        public static ValueTask<float> MaxAsync<TSource>(this IAsyncQueryable<TSource> source, Expression<Func<TSource, float>> selector, CancellationToken cancellationToken = default)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));
            if (selector == null)
                throw new ArgumentNullException(nameof(selector));

            return source.Provider.ExecuteAsync<float>(Expression.Call(MaxAsync__TSource__3__5(typeof(TSource)), source.Expression, selector, Expression.Constant(cancellationToken, typeof(CancellationToken))), cancellationToken);
        }

        private static MethodInfo? s_MaxAsync__TSource__3__6;
        
        private static MethodInfo? MaxAsync__TSource__3__6(Type TSource) =>
            (s_MaxAsync__TSource__3__6 ??
            (s_MaxAsync__TSource__3__6 = new Func<IAsyncQueryable<object>, Expression<Func<object, int?>>, CancellationToken, ValueTask<int?>>(MaxAsync<object>).GetMethodInfo()!.GetGenericMethodDefinition())).MakeGenericMethod(TSource);

        public static ValueTask<int?> MaxAsync<TSource>(this IAsyncQueryable<TSource> source, Expression<Func<TSource, int?>> selector, CancellationToken cancellationToken = default)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));
            if (selector == null)
                throw new ArgumentNullException(nameof(selector));

            return source.Provider.ExecuteAsync<int?>(Expression.Call(MaxAsync__TSource__3__6(typeof(TSource)), source.Expression, selector, Expression.Constant(cancellationToken, typeof(CancellationToken))), cancellationToken);
        }

        private static MethodInfo? s_MaxAsync__TSource__3__7;
        
        private static MethodInfo? MaxAsync__TSource__3__7(Type TSource) =>
            (s_MaxAsync__TSource__3__7 ??
            (s_MaxAsync__TSource__3__7 = new Func<IAsyncQueryable<object>, Expression<Func<object, int>>, CancellationToken, ValueTask<int>>(MaxAsync<object>).GetMethodInfo()!.GetGenericMethodDefinition())).MakeGenericMethod(TSource);

        public static ValueTask<int> MaxAsync<TSource>(this IAsyncQueryable<TSource> source, Expression<Func<TSource, int>> selector, CancellationToken cancellationToken = default)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));
            if (selector == null)
                throw new ArgumentNullException(nameof(selector));

            return source.Provider.ExecuteAsync<int>(Expression.Call(MaxAsync__TSource__3__7(typeof(TSource)), source.Expression, selector, Expression.Constant(cancellationToken, typeof(CancellationToken))), cancellationToken);
        }

        private static MethodInfo? s_MaxAsync__TSource__3__8;
        
        private static MethodInfo? MaxAsync__TSource__3__8(Type TSource) =>
            (s_MaxAsync__TSource__3__8 ??
            (s_MaxAsync__TSource__3__8 = new Func<IAsyncQueryable<object>, Expression<Func<object, long?>>, CancellationToken, ValueTask<long?>>(MaxAsync<object>).GetMethodInfo()!.GetGenericMethodDefinition())).MakeGenericMethod(TSource);

        public static ValueTask<long?> MaxAsync<TSource>(this IAsyncQueryable<TSource> source, Expression<Func<TSource, long?>> selector, CancellationToken cancellationToken = default)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));
            if (selector == null)
                throw new ArgumentNullException(nameof(selector));

            return source.Provider.ExecuteAsync<long?>(Expression.Call(MaxAsync__TSource__3__8(typeof(TSource)), source.Expression, selector, Expression.Constant(cancellationToken, typeof(CancellationToken))), cancellationToken);
        }

        private static MethodInfo? s_MaxAsync__TSource__3__9;
        
        private static MethodInfo? MaxAsync__TSource__3__9(Type TSource) =>
            (s_MaxAsync__TSource__3__9 ??
            (s_MaxAsync__TSource__3__9 = new Func<IAsyncQueryable<object>, Expression<Func<object, long>>, CancellationToken, ValueTask<long>>(MaxAsync<object>).GetMethodInfo()!.GetGenericMethodDefinition())).MakeGenericMethod(TSource);

        public static ValueTask<long> MaxAsync<TSource>(this IAsyncQueryable<TSource> source, Expression<Func<TSource, long>> selector, CancellationToken cancellationToken = default)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));
            if (selector == null)
                throw new ArgumentNullException(nameof(selector));

            return source.Provider.ExecuteAsync<long>(Expression.Call(MaxAsync__TSource__3__9(typeof(TSource)), source.Expression, selector, Expression.Constant(cancellationToken, typeof(CancellationToken))), cancellationToken);
        }

        private static MethodInfo? s_MaxAsync__TSource_TResult__3__0;
        
        private static MethodInfo? MaxAsync__TSource_TResult__3__0(Type TSource, Type TResult) =>
            (s_MaxAsync__TSource_TResult__3__0 ??
            (s_MaxAsync__TSource_TResult__3__0 = new Func<IAsyncQueryable<object>, Expression<Func<object, object>>, CancellationToken, ValueTask<object>>(MaxAsync<object, object>).GetMethodInfo()!.GetGenericMethodDefinition())).MakeGenericMethod(TSource, TResult);

        public static ValueTask<TResult> MaxAsync<TSource, TResult>(this IAsyncQueryable<TSource> source, Expression<Func<TSource, TResult>> selector, CancellationToken cancellationToken = default)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));
            if (selector == null)
                throw new ArgumentNullException(nameof(selector));

            return source.Provider.ExecuteAsync<TResult>(Expression.Call(MaxAsync__TSource_TResult__3__0(typeof(TSource), typeof(TResult)), source.Expression, selector, Expression.Constant(cancellationToken, typeof(CancellationToken))), cancellationToken);
        }

        private static MethodInfo? s_MaxAwaitAsync__TSource__3__0;
        
        private static MethodInfo? MaxAwaitAsync__TSource__3__0(Type TSource) =>
            (s_MaxAwaitAsync__TSource__3__0 ??
            (s_MaxAwaitAsync__TSource__3__0 = new Func<IAsyncQueryable<object>, Expression<Func<object, ValueTask<decimal?>>>, CancellationToken, ValueTask<decimal?>>(MaxAwaitAsync<object>).GetMethodInfo()!.GetGenericMethodDefinition())).MakeGenericMethod(TSource);

        public static ValueTask<decimal?> MaxAwaitAsync<TSource>(this IAsyncQueryable<TSource> source, Expression<Func<TSource, ValueTask<decimal?>>> selector, CancellationToken cancellationToken = default)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));
            if (selector == null)
                throw new ArgumentNullException(nameof(selector));

            return source.Provider.ExecuteAsync<decimal?>(Expression.Call(MaxAwaitAsync__TSource__3__0(typeof(TSource)), source.Expression, selector, Expression.Constant(cancellationToken, typeof(CancellationToken))), cancellationToken);
        }

        private static MethodInfo? s_MaxAwaitAsync__TSource__3__1;
        
        private static MethodInfo? MaxAwaitAsync__TSource__3__1(Type TSource) =>
            (s_MaxAwaitAsync__TSource__3__1 ??
            (s_MaxAwaitAsync__TSource__3__1 = new Func<IAsyncQueryable<object>, Expression<Func<object, ValueTask<decimal>>>, CancellationToken, ValueTask<decimal>>(MaxAwaitAsync<object>).GetMethodInfo()!.GetGenericMethodDefinition())).MakeGenericMethod(TSource);

        public static ValueTask<decimal> MaxAwaitAsync<TSource>(this IAsyncQueryable<TSource> source, Expression<Func<TSource, ValueTask<decimal>>> selector, CancellationToken cancellationToken = default)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));
            if (selector == null)
                throw new ArgumentNullException(nameof(selector));

            return source.Provider.ExecuteAsync<decimal>(Expression.Call(MaxAwaitAsync__TSource__3__1(typeof(TSource)), source.Expression, selector, Expression.Constant(cancellationToken, typeof(CancellationToken))), cancellationToken);
        }

        private static MethodInfo? s_MaxAwaitAsync__TSource__3__2;
        
        private static MethodInfo? MaxAwaitAsync__TSource__3__2(Type TSource) =>
            (s_MaxAwaitAsync__TSource__3__2 ??
            (s_MaxAwaitAsync__TSource__3__2 = new Func<IAsyncQueryable<object>, Expression<Func<object, ValueTask<double?>>>, CancellationToken, ValueTask<double?>>(MaxAwaitAsync<object>).GetMethodInfo()!.GetGenericMethodDefinition())).MakeGenericMethod(TSource);

        public static ValueTask<double?> MaxAwaitAsync<TSource>(this IAsyncQueryable<TSource> source, Expression<Func<TSource, ValueTask<double?>>> selector, CancellationToken cancellationToken = default)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));
            if (selector == null)
                throw new ArgumentNullException(nameof(selector));

            return source.Provider.ExecuteAsync<double?>(Expression.Call(MaxAwaitAsync__TSource__3__2(typeof(TSource)), source.Expression, selector, Expression.Constant(cancellationToken, typeof(CancellationToken))), cancellationToken);
        }

        private static MethodInfo? s_MaxAwaitAsync__TSource__3__3;
        
        private static MethodInfo? MaxAwaitAsync__TSource__3__3(Type TSource) =>
            (s_MaxAwaitAsync__TSource__3__3 ??
            (s_MaxAwaitAsync__TSource__3__3 = new Func<IAsyncQueryable<object>, Expression<Func<object, ValueTask<double>>>, CancellationToken, ValueTask<double>>(MaxAwaitAsync<object>).GetMethodInfo()!.GetGenericMethodDefinition())).MakeGenericMethod(TSource);

        public static ValueTask<double> MaxAwaitAsync<TSource>(this IAsyncQueryable<TSource> source, Expression<Func<TSource, ValueTask<double>>> selector, CancellationToken cancellationToken = default)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));
            if (selector == null)
                throw new ArgumentNullException(nameof(selector));

            return source.Provider.ExecuteAsync<double>(Expression.Call(MaxAwaitAsync__TSource__3__3(typeof(TSource)), source.Expression, selector, Expression.Constant(cancellationToken, typeof(CancellationToken))), cancellationToken);
        }

        private static MethodInfo? s_MaxAwaitAsync__TSource__3__4;
        
        private static MethodInfo? MaxAwaitAsync__TSource__3__4(Type TSource) =>
            (s_MaxAwaitAsync__TSource__3__4 ??
            (s_MaxAwaitAsync__TSource__3__4 = new Func<IAsyncQueryable<object>, Expression<Func<object, ValueTask<float?>>>, CancellationToken, ValueTask<float?>>(MaxAwaitAsync<object>).GetMethodInfo()!.GetGenericMethodDefinition())).MakeGenericMethod(TSource);

        public static ValueTask<float?> MaxAwaitAsync<TSource>(this IAsyncQueryable<TSource> source, Expression<Func<TSource, ValueTask<float?>>> selector, CancellationToken cancellationToken = default)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));
            if (selector == null)
                throw new ArgumentNullException(nameof(selector));

            return source.Provider.ExecuteAsync<float?>(Expression.Call(MaxAwaitAsync__TSource__3__4(typeof(TSource)), source.Expression, selector, Expression.Constant(cancellationToken, typeof(CancellationToken))), cancellationToken);
        }

        private static MethodInfo? s_MaxAwaitAsync__TSource__3__5;
        
        private static MethodInfo? MaxAwaitAsync__TSource__3__5(Type TSource) =>
            (s_MaxAwaitAsync__TSource__3__5 ??
            (s_MaxAwaitAsync__TSource__3__5 = new Func<IAsyncQueryable<object>, Expression<Func<object, ValueTask<float>>>, CancellationToken, ValueTask<float>>(MaxAwaitAsync<object>).GetMethodInfo()!.GetGenericMethodDefinition())).MakeGenericMethod(TSource);

        public static ValueTask<float> MaxAwaitAsync<TSource>(this IAsyncQueryable<TSource> source, Expression<Func<TSource, ValueTask<float>>> selector, CancellationToken cancellationToken = default)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));
            if (selector == null)
                throw new ArgumentNullException(nameof(selector));

            return source.Provider.ExecuteAsync<float>(Expression.Call(MaxAwaitAsync__TSource__3__5(typeof(TSource)), source.Expression, selector, Expression.Constant(cancellationToken, typeof(CancellationToken))), cancellationToken);
        }

        private static MethodInfo? s_MaxAwaitAsync__TSource__3__6;
        
        private static MethodInfo? MaxAwaitAsync__TSource__3__6(Type TSource) =>
            (s_MaxAwaitAsync__TSource__3__6 ??
            (s_MaxAwaitAsync__TSource__3__6 = new Func<IAsyncQueryable<object>, Expression<Func<object, ValueTask<int?>>>, CancellationToken, ValueTask<int?>>(MaxAwaitAsync<object>).GetMethodInfo()!.GetGenericMethodDefinition())).MakeGenericMethod(TSource);

        public static ValueTask<int?> MaxAwaitAsync<TSource>(this IAsyncQueryable<TSource> source, Expression<Func<TSource, ValueTask<int?>>> selector, CancellationToken cancellationToken = default)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));
            if (selector == null)
                throw new ArgumentNullException(nameof(selector));

            return source.Provider.ExecuteAsync<int?>(Expression.Call(MaxAwaitAsync__TSource__3__6(typeof(TSource)), source.Expression, selector, Expression.Constant(cancellationToken, typeof(CancellationToken))), cancellationToken);
        }

        private static MethodInfo? s_MaxAwaitAsync__TSource__3__7;
        
        private static MethodInfo? MaxAwaitAsync__TSource__3__7(Type TSource) =>
            (s_MaxAwaitAsync__TSource__3__7 ??
            (s_MaxAwaitAsync__TSource__3__7 = new Func<IAsyncQueryable<object>, Expression<Func<object, ValueTask<int>>>, CancellationToken, ValueTask<int>>(MaxAwaitAsync<object>).GetMethodInfo()!.GetGenericMethodDefinition())).MakeGenericMethod(TSource);

        public static ValueTask<int> MaxAwaitAsync<TSource>(this IAsyncQueryable<TSource> source, Expression<Func<TSource, ValueTask<int>>> selector, CancellationToken cancellationToken = default)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));
            if (selector == null)
                throw new ArgumentNullException(nameof(selector));

            return source.Provider.ExecuteAsync<int>(Expression.Call(MaxAwaitAsync__TSource__3__7(typeof(TSource)), source.Expression, selector, Expression.Constant(cancellationToken, typeof(CancellationToken))), cancellationToken);
        }

        private static MethodInfo? s_MaxAwaitAsync__TSource__3__8;
        
        private static MethodInfo? MaxAwaitAsync__TSource__3__8(Type TSource) =>
            (s_MaxAwaitAsync__TSource__3__8 ??
            (s_MaxAwaitAsync__TSource__3__8 = new Func<IAsyncQueryable<object>, Expression<Func<object, ValueTask<long?>>>, CancellationToken, ValueTask<long?>>(MaxAwaitAsync<object>).GetMethodInfo()!.GetGenericMethodDefinition())).MakeGenericMethod(TSource);

        public static ValueTask<long?> MaxAwaitAsync<TSource>(this IAsyncQueryable<TSource> source, Expression<Func<TSource, ValueTask<long?>>> selector, CancellationToken cancellationToken = default)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));
            if (selector == null)
                throw new ArgumentNullException(nameof(selector));

            return source.Provider.ExecuteAsync<long?>(Expression.Call(MaxAwaitAsync__TSource__3__8(typeof(TSource)), source.Expression, selector, Expression.Constant(cancellationToken, typeof(CancellationToken))), cancellationToken);
        }

        private static MethodInfo? s_MaxAwaitAsync__TSource__3__9;
        
        private static MethodInfo? MaxAwaitAsync__TSource__3__9(Type TSource) =>
            (s_MaxAwaitAsync__TSource__3__9 ??
            (s_MaxAwaitAsync__TSource__3__9 = new Func<IAsyncQueryable<object>, Expression<Func<object, ValueTask<long>>>, CancellationToken, ValueTask<long>>(MaxAwaitAsync<object>).GetMethodInfo()!.GetGenericMethodDefinition())).MakeGenericMethod(TSource);

        public static ValueTask<long> MaxAwaitAsync<TSource>(this IAsyncQueryable<TSource> source, Expression<Func<TSource, ValueTask<long>>> selector, CancellationToken cancellationToken = default)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));
            if (selector == null)
                throw new ArgumentNullException(nameof(selector));

            return source.Provider.ExecuteAsync<long>(Expression.Call(MaxAwaitAsync__TSource__3__9(typeof(TSource)), source.Expression, selector, Expression.Constant(cancellationToken, typeof(CancellationToken))), cancellationToken);
        }

        private static MethodInfo? s_MaxAwaitAsync__TSource_TResult__3__0;
        
        private static MethodInfo? MaxAwaitAsync__TSource_TResult__3__0(Type TSource, Type TResult) =>
            (s_MaxAwaitAsync__TSource_TResult__3__0 ??
            (s_MaxAwaitAsync__TSource_TResult__3__0 = new Func<IAsyncQueryable<object>, Expression<Func<object, ValueTask<object>>>, CancellationToken, ValueTask<object>>(MaxAwaitAsync<object, object>).GetMethodInfo()!.GetGenericMethodDefinition())).MakeGenericMethod(TSource, TResult);

        public static ValueTask<TResult> MaxAwaitAsync<TSource, TResult>(this IAsyncQueryable<TSource> source, Expression<Func<TSource, ValueTask<TResult>>> selector, CancellationToken cancellationToken = default)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));
            if (selector == null)
                throw new ArgumentNullException(nameof(selector));

            return source.Provider.ExecuteAsync<TResult>(Expression.Call(MaxAwaitAsync__TSource_TResult__3__0(typeof(TSource), typeof(TResult)), source.Expression, selector, Expression.Constant(cancellationToken, typeof(CancellationToken))), cancellationToken);
        }

        private static MethodInfo? s_MaxAwaitWithCancellationAsync__TSource__3__0;
        
        private static MethodInfo? MaxAwaitWithCancellationAsync__TSource__3__0(Type TSource) =>
            (s_MaxAwaitWithCancellationAsync__TSource__3__0 ??
            (s_MaxAwaitWithCancellationAsync__TSource__3__0 = new Func<IAsyncQueryable<object>, Expression<Func<object, CancellationToken, ValueTask<decimal?>>>, CancellationToken, ValueTask<decimal?>>(MaxAwaitWithCancellationAsync<object>).GetMethodInfo()!.GetGenericMethodDefinition())).MakeGenericMethod(TSource);

        public static ValueTask<decimal?> MaxAwaitWithCancellationAsync<TSource>(this IAsyncQueryable<TSource> source, Expression<Func<TSource, CancellationToken, ValueTask<decimal?>>> selector, CancellationToken cancellationToken = default)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));
            if (selector == null)
                throw new ArgumentNullException(nameof(selector));

            return source.Provider.ExecuteAsync<decimal?>(Expression.Call(MaxAwaitWithCancellationAsync__TSource__3__0(typeof(TSource)), source.Expression, selector, Expression.Constant(cancellationToken, typeof(CancellationToken))), cancellationToken);
        }

        private static MethodInfo? s_MaxAwaitWithCancellationAsync__TSource__3__1;
        
        private static MethodInfo? MaxAwaitWithCancellationAsync__TSource__3__1(Type TSource) =>
            (s_MaxAwaitWithCancellationAsync__TSource__3__1 ??
            (s_MaxAwaitWithCancellationAsync__TSource__3__1 = new Func<IAsyncQueryable<object>, Expression<Func<object, CancellationToken, ValueTask<decimal>>>, CancellationToken, ValueTask<decimal>>(MaxAwaitWithCancellationAsync<object>).GetMethodInfo()!.GetGenericMethodDefinition())).MakeGenericMethod(TSource);

        public static ValueTask<decimal> MaxAwaitWithCancellationAsync<TSource>(this IAsyncQueryable<TSource> source, Expression<Func<TSource, CancellationToken, ValueTask<decimal>>> selector, CancellationToken cancellationToken = default)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));
            if (selector == null)
                throw new ArgumentNullException(nameof(selector));

            return source.Provider.ExecuteAsync<decimal>(Expression.Call(MaxAwaitWithCancellationAsync__TSource__3__1(typeof(TSource)), source.Expression, selector, Expression.Constant(cancellationToken, typeof(CancellationToken))), cancellationToken);
        }

        private static MethodInfo? s_MaxAwaitWithCancellationAsync__TSource__3__2;
        
        private static MethodInfo? MaxAwaitWithCancellationAsync__TSource__3__2(Type TSource) =>
            (s_MaxAwaitWithCancellationAsync__TSource__3__2 ??
            (s_MaxAwaitWithCancellationAsync__TSource__3__2 = new Func<IAsyncQueryable<object>, Expression<Func<object, CancellationToken, ValueTask<double?>>>, CancellationToken, ValueTask<double?>>(MaxAwaitWithCancellationAsync<object>).GetMethodInfo()!.GetGenericMethodDefinition())).MakeGenericMethod(TSource);

        public static ValueTask<double?> MaxAwaitWithCancellationAsync<TSource>(this IAsyncQueryable<TSource> source, Expression<Func<TSource, CancellationToken, ValueTask<double?>>> selector, CancellationToken cancellationToken = default)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));
            if (selector == null)
                throw new ArgumentNullException(nameof(selector));

            return source.Provider.ExecuteAsync<double?>(Expression.Call(MaxAwaitWithCancellationAsync__TSource__3__2(typeof(TSource)), source.Expression, selector, Expression.Constant(cancellationToken, typeof(CancellationToken))), cancellationToken);
        }

        private static MethodInfo? s_MaxAwaitWithCancellationAsync__TSource__3__3;
        
        private static MethodInfo? MaxAwaitWithCancellationAsync__TSource__3__3(Type TSource) =>
            (s_MaxAwaitWithCancellationAsync__TSource__3__3 ??
            (s_MaxAwaitWithCancellationAsync__TSource__3__3 = new Func<IAsyncQueryable<object>, Expression<Func<object, CancellationToken, ValueTask<double>>>, CancellationToken, ValueTask<double>>(MaxAwaitWithCancellationAsync<object>).GetMethodInfo()!.GetGenericMethodDefinition())).MakeGenericMethod(TSource);

        public static ValueTask<double> MaxAwaitWithCancellationAsync<TSource>(this IAsyncQueryable<TSource> source, Expression<Func<TSource, CancellationToken, ValueTask<double>>> selector, CancellationToken cancellationToken = default)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));
            if (selector == null)
                throw new ArgumentNullException(nameof(selector));

            return source.Provider.ExecuteAsync<double>(Expression.Call(MaxAwaitWithCancellationAsync__TSource__3__3(typeof(TSource)), source.Expression, selector, Expression.Constant(cancellationToken, typeof(CancellationToken))), cancellationToken);
        }

        private static MethodInfo? s_MaxAwaitWithCancellationAsync__TSource__3__4;
        
        private static MethodInfo? MaxAwaitWithCancellationAsync__TSource__3__4(Type TSource) =>
            (s_MaxAwaitWithCancellationAsync__TSource__3__4 ??
            (s_MaxAwaitWithCancellationAsync__TSource__3__4 = new Func<IAsyncQueryable<object>, Expression<Func<object, CancellationToken, ValueTask<float?>>>, CancellationToken, ValueTask<float?>>(MaxAwaitWithCancellationAsync<object>).GetMethodInfo()!.GetGenericMethodDefinition())).MakeGenericMethod(TSource);

        public static ValueTask<float?> MaxAwaitWithCancellationAsync<TSource>(this IAsyncQueryable<TSource> source, Expression<Func<TSource, CancellationToken, ValueTask<float?>>> selector, CancellationToken cancellationToken = default)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));
            if (selector == null)
                throw new ArgumentNullException(nameof(selector));

            return source.Provider.ExecuteAsync<float?>(Expression.Call(MaxAwaitWithCancellationAsync__TSource__3__4(typeof(TSource)), source.Expression, selector, Expression.Constant(cancellationToken, typeof(CancellationToken))), cancellationToken);
        }

        private static MethodInfo? s_MaxAwaitWithCancellationAsync__TSource__3__5;
        
        private static MethodInfo? MaxAwaitWithCancellationAsync__TSource__3__5(Type TSource) =>
            (s_MaxAwaitWithCancellationAsync__TSource__3__5 ??
            (s_MaxAwaitWithCancellationAsync__TSource__3__5 = new Func<IAsyncQueryable<object>, Expression<Func<object, CancellationToken, ValueTask<float>>>, CancellationToken, ValueTask<float>>(MaxAwaitWithCancellationAsync<object>).GetMethodInfo()!.GetGenericMethodDefinition())).MakeGenericMethod(TSource);

        public static ValueTask<float> MaxAwaitWithCancellationAsync<TSource>(this IAsyncQueryable<TSource> source, Expression<Func<TSource, CancellationToken, ValueTask<float>>> selector, CancellationToken cancellationToken = default)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));
            if (selector == null)
                throw new ArgumentNullException(nameof(selector));

            return source.Provider.ExecuteAsync<float>(Expression.Call(MaxAwaitWithCancellationAsync__TSource__3__5(typeof(TSource)), source.Expression, selector, Expression.Constant(cancellationToken, typeof(CancellationToken))), cancellationToken);
        }

        private static MethodInfo? s_MaxAwaitWithCancellationAsync__TSource__3__6;
        
        private static MethodInfo? MaxAwaitWithCancellationAsync__TSource__3__6(Type TSource) =>
            (s_MaxAwaitWithCancellationAsync__TSource__3__6 ??
            (s_MaxAwaitWithCancellationAsync__TSource__3__6 = new Func<IAsyncQueryable<object>, Expression<Func<object, CancellationToken, ValueTask<int?>>>, CancellationToken, ValueTask<int?>>(MaxAwaitWithCancellationAsync<object>).GetMethodInfo()!.GetGenericMethodDefinition())).MakeGenericMethod(TSource);

        public static ValueTask<int?> MaxAwaitWithCancellationAsync<TSource>(this IAsyncQueryable<TSource> source, Expression<Func<TSource, CancellationToken, ValueTask<int?>>> selector, CancellationToken cancellationToken = default)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));
            if (selector == null)
                throw new ArgumentNullException(nameof(selector));

            return source.Provider.ExecuteAsync<int?>(Expression.Call(MaxAwaitWithCancellationAsync__TSource__3__6(typeof(TSource)), source.Expression, selector, Expression.Constant(cancellationToken, typeof(CancellationToken))), cancellationToken);
        }

        private static MethodInfo? s_MaxAwaitWithCancellationAsync__TSource__3__7;
        
        private static MethodInfo? MaxAwaitWithCancellationAsync__TSource__3__7(Type TSource) =>
            (s_MaxAwaitWithCancellationAsync__TSource__3__7 ??
            (s_MaxAwaitWithCancellationAsync__TSource__3__7 = new Func<IAsyncQueryable<object>, Expression<Func<object, CancellationToken, ValueTask<int>>>, CancellationToken, ValueTask<int>>(MaxAwaitWithCancellationAsync<object>).GetMethodInfo()!.GetGenericMethodDefinition())).MakeGenericMethod(TSource);

        public static ValueTask<int> MaxAwaitWithCancellationAsync<TSource>(this IAsyncQueryable<TSource> source, Expression<Func<TSource, CancellationToken, ValueTask<int>>> selector, CancellationToken cancellationToken = default)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));
            if (selector == null)
                throw new ArgumentNullException(nameof(selector));

            return source.Provider.ExecuteAsync<int>(Expression.Call(MaxAwaitWithCancellationAsync__TSource__3__7(typeof(TSource)), source.Expression, selector, Expression.Constant(cancellationToken, typeof(CancellationToken))), cancellationToken);
        }

        private static MethodInfo? s_MaxAwaitWithCancellationAsync__TSource__3__8;
        
        private static MethodInfo? MaxAwaitWithCancellationAsync__TSource__3__8(Type TSource) =>
            (s_MaxAwaitWithCancellationAsync__TSource__3__8 ??
            (s_MaxAwaitWithCancellationAsync__TSource__3__8 = new Func<IAsyncQueryable<object>, Expression<Func<object, CancellationToken, ValueTask<long?>>>, CancellationToken, ValueTask<long?>>(MaxAwaitWithCancellationAsync<object>).GetMethodInfo()!.GetGenericMethodDefinition())).MakeGenericMethod(TSource);

        public static ValueTask<long?> MaxAwaitWithCancellationAsync<TSource>(this IAsyncQueryable<TSource> source, Expression<Func<TSource, CancellationToken, ValueTask<long?>>> selector, CancellationToken cancellationToken = default)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));
            if (selector == null)
                throw new ArgumentNullException(nameof(selector));

            return source.Provider.ExecuteAsync<long?>(Expression.Call(MaxAwaitWithCancellationAsync__TSource__3__8(typeof(TSource)), source.Expression, selector, Expression.Constant(cancellationToken, typeof(CancellationToken))), cancellationToken);
        }

        private static MethodInfo? s_MaxAwaitWithCancellationAsync__TSource__3__9;
        
        private static MethodInfo? MaxAwaitWithCancellationAsync__TSource__3__9(Type TSource) =>
            (s_MaxAwaitWithCancellationAsync__TSource__3__9 ??
            (s_MaxAwaitWithCancellationAsync__TSource__3__9 = new Func<IAsyncQueryable<object>, Expression<Func<object, CancellationToken, ValueTask<long>>>, CancellationToken, ValueTask<long>>(MaxAwaitWithCancellationAsync<object>).GetMethodInfo()!.GetGenericMethodDefinition())).MakeGenericMethod(TSource);

        public static ValueTask<long> MaxAwaitWithCancellationAsync<TSource>(this IAsyncQueryable<TSource> source, Expression<Func<TSource, CancellationToken, ValueTask<long>>> selector, CancellationToken cancellationToken = default)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));
            if (selector == null)
                throw new ArgumentNullException(nameof(selector));

            return source.Provider.ExecuteAsync<long>(Expression.Call(MaxAwaitWithCancellationAsync__TSource__3__9(typeof(TSource)), source.Expression, selector, Expression.Constant(cancellationToken, typeof(CancellationToken))), cancellationToken);
        }

        private static MethodInfo? s_MaxAwaitWithCancellationAsync__TSource_TResult__3__0;
        
        private static MethodInfo? MaxAwaitWithCancellationAsync__TSource_TResult__3__0(Type TSource, Type TResult) =>
            (s_MaxAwaitWithCancellationAsync__TSource_TResult__3__0 ??
            (s_MaxAwaitWithCancellationAsync__TSource_TResult__3__0 = new Func<IAsyncQueryable<object>, Expression<Func<object, CancellationToken, ValueTask<object>>>, CancellationToken, ValueTask<object>>(MaxAwaitWithCancellationAsync<object, object>).GetMethodInfo()!.GetGenericMethodDefinition())).MakeGenericMethod(TSource, TResult);

        public static ValueTask<TResult> MaxAwaitWithCancellationAsync<TSource, TResult>(this IAsyncQueryable<TSource> source, Expression<Func<TSource, CancellationToken, ValueTask<TResult>>> selector, CancellationToken cancellationToken = default)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));
            if (selector == null)
                throw new ArgumentNullException(nameof(selector));

            return source.Provider.ExecuteAsync<TResult>(Expression.Call(MaxAwaitWithCancellationAsync__TSource_TResult__3__0(typeof(TSource), typeof(TResult)), source.Expression, selector, Expression.Constant(cancellationToken, typeof(CancellationToken))), cancellationToken);
        }

        private static MethodInfo? s_MinAsync__2__0;
        
        private static MethodInfo? MinAsync__2__0 =>
            (s_MinAsync__2__0 ??
            (s_MinAsync__2__0 = new Func<IAsyncQueryable<decimal?>, CancellationToken, ValueTask<decimal?>>(MinAsync).GetMethodInfo()));

        public static ValueTask<decimal?> MinAsync(this IAsyncQueryable<decimal?> source, CancellationToken cancellationToken = default)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));

            return source.Provider.ExecuteAsync<decimal?>(Expression.Call(MinAsync__2__0, source.Expression, Expression.Constant(cancellationToken, typeof(CancellationToken))), cancellationToken);
        }

        private static MethodInfo? s_MinAsync__2__1;
        
        private static MethodInfo? MinAsync__2__1 =>
            (s_MinAsync__2__1 ??
            (s_MinAsync__2__1 = new Func<IAsyncQueryable<decimal>, CancellationToken, ValueTask<decimal>>(MinAsync).GetMethodInfo()));

        public static ValueTask<decimal> MinAsync(this IAsyncQueryable<decimal> source, CancellationToken cancellationToken = default)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));

            return source.Provider.ExecuteAsync<decimal>(Expression.Call(MinAsync__2__1, source.Expression, Expression.Constant(cancellationToken, typeof(CancellationToken))), cancellationToken);
        }

        private static MethodInfo? s_MinAsync__2__2;
        
        private static MethodInfo? MinAsync__2__2 =>
            (s_MinAsync__2__2 ??
            (s_MinAsync__2__2 = new Func<IAsyncQueryable<double?>, CancellationToken, ValueTask<double?>>(MinAsync).GetMethodInfo()));

        public static ValueTask<double?> MinAsync(this IAsyncQueryable<double?> source, CancellationToken cancellationToken = default)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));

            return source.Provider.ExecuteAsync<double?>(Expression.Call(MinAsync__2__2, source.Expression, Expression.Constant(cancellationToken, typeof(CancellationToken))), cancellationToken);
        }

        private static MethodInfo? s_MinAsync__2__3;
        
        private static MethodInfo? MinAsync__2__3 =>
            (s_MinAsync__2__3 ??
            (s_MinAsync__2__3 = new Func<IAsyncQueryable<double>, CancellationToken, ValueTask<double>>(MinAsync).GetMethodInfo()));

        public static ValueTask<double> MinAsync(this IAsyncQueryable<double> source, CancellationToken cancellationToken = default)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));

            return source.Provider.ExecuteAsync<double>(Expression.Call(MinAsync__2__3, source.Expression, Expression.Constant(cancellationToken, typeof(CancellationToken))), cancellationToken);
        }

        private static MethodInfo? s_MinAsync__2__4;
        
        private static MethodInfo? MinAsync__2__4 =>
            (s_MinAsync__2__4 ??
            (s_MinAsync__2__4 = new Func<IAsyncQueryable<float?>, CancellationToken, ValueTask<float?>>(MinAsync).GetMethodInfo()));

        public static ValueTask<float?> MinAsync(this IAsyncQueryable<float?> source, CancellationToken cancellationToken = default)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));

            return source.Provider.ExecuteAsync<float?>(Expression.Call(MinAsync__2__4, source.Expression, Expression.Constant(cancellationToken, typeof(CancellationToken))), cancellationToken);
        }

        private static MethodInfo? s_MinAsync__2__5;
        
        private static MethodInfo? MinAsync__2__5 =>
            (s_MinAsync__2__5 ??
            (s_MinAsync__2__5 = new Func<IAsyncQueryable<float>, CancellationToken, ValueTask<float>>(MinAsync).GetMethodInfo()));

        public static ValueTask<float> MinAsync(this IAsyncQueryable<float> source, CancellationToken cancellationToken = default)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));

            return source.Provider.ExecuteAsync<float>(Expression.Call(MinAsync__2__5, source.Expression, Expression.Constant(cancellationToken, typeof(CancellationToken))), cancellationToken);
        }

        private static MethodInfo? s_MinAsync__2__6;
        
        private static MethodInfo? MinAsync__2__6 =>
            (s_MinAsync__2__6 ??
            (s_MinAsync__2__6 = new Func<IAsyncQueryable<int?>, CancellationToken, ValueTask<int?>>(MinAsync).GetMethodInfo()));

        public static ValueTask<int?> MinAsync(this IAsyncQueryable<int?> source, CancellationToken cancellationToken = default)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));

            return source.Provider.ExecuteAsync<int?>(Expression.Call(MinAsync__2__6, source.Expression, Expression.Constant(cancellationToken, typeof(CancellationToken))), cancellationToken);
        }

        private static MethodInfo? s_MinAsync__2__7;
        
        private static MethodInfo? MinAsync__2__7 =>
            (s_MinAsync__2__7 ??
            (s_MinAsync__2__7 = new Func<IAsyncQueryable<int>, CancellationToken, ValueTask<int>>(MinAsync).GetMethodInfo()));

        public static ValueTask<int> MinAsync(this IAsyncQueryable<int> source, CancellationToken cancellationToken = default)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));

            return source.Provider.ExecuteAsync<int>(Expression.Call(MinAsync__2__7, source.Expression, Expression.Constant(cancellationToken, typeof(CancellationToken))), cancellationToken);
        }

        private static MethodInfo? s_MinAsync__2__8;
        
        private static MethodInfo? MinAsync__2__8 =>
            (s_MinAsync__2__8 ??
            (s_MinAsync__2__8 = new Func<IAsyncQueryable<long?>, CancellationToken, ValueTask<long?>>(MinAsync).GetMethodInfo()));

        public static ValueTask<long?> MinAsync(this IAsyncQueryable<long?> source, CancellationToken cancellationToken = default)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));

            return source.Provider.ExecuteAsync<long?>(Expression.Call(MinAsync__2__8, source.Expression, Expression.Constant(cancellationToken, typeof(CancellationToken))), cancellationToken);
        }

        private static MethodInfo? s_MinAsync__2__9;
        
        private static MethodInfo? MinAsync__2__9 =>
            (s_MinAsync__2__9 ??
            (s_MinAsync__2__9 = new Func<IAsyncQueryable<long>, CancellationToken, ValueTask<long>>(MinAsync).GetMethodInfo()));

        public static ValueTask<long> MinAsync(this IAsyncQueryable<long> source, CancellationToken cancellationToken = default)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));

            return source.Provider.ExecuteAsync<long>(Expression.Call(MinAsync__2__9, source.Expression, Expression.Constant(cancellationToken, typeof(CancellationToken))), cancellationToken);
        }

        private static MethodInfo? s_MinAsync__TSource__2__0;
        
        private static MethodInfo? MinAsync__TSource__2__0(Type TSource) =>
            (s_MinAsync__TSource__2__0 ??
            (s_MinAsync__TSource__2__0 = new Func<IAsyncQueryable<object>, CancellationToken, ValueTask<object>>(MinAsync<object>).GetMethodInfo()!.GetGenericMethodDefinition())).MakeGenericMethod(TSource);

        public static ValueTask<TSource> MinAsync<TSource>(this IAsyncQueryable<TSource> source, CancellationToken cancellationToken = default)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));

            return source.Provider.ExecuteAsync<TSource>(Expression.Call(MinAsync__TSource__2__0(typeof(TSource)), source.Expression, Expression.Constant(cancellationToken, typeof(CancellationToken))), cancellationToken);
        }

        private static MethodInfo? s_MinAsync__TSource__3__0;
        
        private static MethodInfo? MinAsync__TSource__3__0(Type TSource) =>
            (s_MinAsync__TSource__3__0 ??
            (s_MinAsync__TSource__3__0 = new Func<IAsyncQueryable<object>, Expression<Func<object, decimal?>>, CancellationToken, ValueTask<decimal?>>(MinAsync<object>).GetMethodInfo()!.GetGenericMethodDefinition())).MakeGenericMethod(TSource);

        public static ValueTask<decimal?> MinAsync<TSource>(this IAsyncQueryable<TSource> source, Expression<Func<TSource, decimal?>> selector, CancellationToken cancellationToken = default)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));
            if (selector == null)
                throw new ArgumentNullException(nameof(selector));

            return source.Provider.ExecuteAsync<decimal?>(Expression.Call(MinAsync__TSource__3__0(typeof(TSource)), source.Expression, selector, Expression.Constant(cancellationToken, typeof(CancellationToken))), cancellationToken);
        }

        private static MethodInfo? s_MinAsync__TSource__3__1;
        
        private static MethodInfo? MinAsync__TSource__3__1(Type TSource) =>
            (s_MinAsync__TSource__3__1 ??
            (s_MinAsync__TSource__3__1 = new Func<IAsyncQueryable<object>, Expression<Func<object, decimal>>, CancellationToken, ValueTask<decimal>>(MinAsync<object>).GetMethodInfo()!.GetGenericMethodDefinition())).MakeGenericMethod(TSource);

        public static ValueTask<decimal> MinAsync<TSource>(this IAsyncQueryable<TSource> source, Expression<Func<TSource, decimal>> selector, CancellationToken cancellationToken = default)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));
            if (selector == null)
                throw new ArgumentNullException(nameof(selector));

            return source.Provider.ExecuteAsync<decimal>(Expression.Call(MinAsync__TSource__3__1(typeof(TSource)), source.Expression, selector, Expression.Constant(cancellationToken, typeof(CancellationToken))), cancellationToken);
        }

        private static MethodInfo? s_MinAsync__TSource__3__2;
        
        private static MethodInfo? MinAsync__TSource__3__2(Type TSource) =>
            (s_MinAsync__TSource__3__2 ??
            (s_MinAsync__TSource__3__2 = new Func<IAsyncQueryable<object>, Expression<Func<object, double?>>, CancellationToken, ValueTask<double?>>(MinAsync<object>).GetMethodInfo()!.GetGenericMethodDefinition())).MakeGenericMethod(TSource);

        public static ValueTask<double?> MinAsync<TSource>(this IAsyncQueryable<TSource> source, Expression<Func<TSource, double?>> selector, CancellationToken cancellationToken = default)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));
            if (selector == null)
                throw new ArgumentNullException(nameof(selector));

            return source.Provider.ExecuteAsync<double?>(Expression.Call(MinAsync__TSource__3__2(typeof(TSource)), source.Expression, selector, Expression.Constant(cancellationToken, typeof(CancellationToken))), cancellationToken);
        }

        private static MethodInfo? s_MinAsync__TSource__3__3;
        
        private static MethodInfo? MinAsync__TSource__3__3(Type TSource) =>
            (s_MinAsync__TSource__3__3 ??
            (s_MinAsync__TSource__3__3 = new Func<IAsyncQueryable<object>, Expression<Func<object, double>>, CancellationToken, ValueTask<double>>(MinAsync<object>).GetMethodInfo()!.GetGenericMethodDefinition())).MakeGenericMethod(TSource);

        public static ValueTask<double> MinAsync<TSource>(this IAsyncQueryable<TSource> source, Expression<Func<TSource, double>> selector, CancellationToken cancellationToken = default)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));
            if (selector == null)
                throw new ArgumentNullException(nameof(selector));

            return source.Provider.ExecuteAsync<double>(Expression.Call(MinAsync__TSource__3__3(typeof(TSource)), source.Expression, selector, Expression.Constant(cancellationToken, typeof(CancellationToken))), cancellationToken);
        }

        private static MethodInfo? s_MinAsync__TSource__3__4;
        
        private static MethodInfo? MinAsync__TSource__3__4(Type TSource) =>
            (s_MinAsync__TSource__3__4 ??
            (s_MinAsync__TSource__3__4 = new Func<IAsyncQueryable<object>, Expression<Func<object, float?>>, CancellationToken, ValueTask<float?>>(MinAsync<object>).GetMethodInfo()!.GetGenericMethodDefinition())).MakeGenericMethod(TSource);

        public static ValueTask<float?> MinAsync<TSource>(this IAsyncQueryable<TSource> source, Expression<Func<TSource, float?>> selector, CancellationToken cancellationToken = default)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));
            if (selector == null)
                throw new ArgumentNullException(nameof(selector));

            return source.Provider.ExecuteAsync<float?>(Expression.Call(MinAsync__TSource__3__4(typeof(TSource)), source.Expression, selector, Expression.Constant(cancellationToken, typeof(CancellationToken))), cancellationToken);
        }

        private static MethodInfo? s_MinAsync__TSource__3__5;
        
        private static MethodInfo? MinAsync__TSource__3__5(Type TSource) =>
            (s_MinAsync__TSource__3__5 ??
            (s_MinAsync__TSource__3__5 = new Func<IAsyncQueryable<object>, Expression<Func<object, float>>, CancellationToken, ValueTask<float>>(MinAsync<object>).GetMethodInfo()!.GetGenericMethodDefinition())).MakeGenericMethod(TSource);

        public static ValueTask<float> MinAsync<TSource>(this IAsyncQueryable<TSource> source, Expression<Func<TSource, float>> selector, CancellationToken cancellationToken = default)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));
            if (selector == null)
                throw new ArgumentNullException(nameof(selector));

            return source.Provider.ExecuteAsync<float>(Expression.Call(MinAsync__TSource__3__5(typeof(TSource)), source.Expression, selector, Expression.Constant(cancellationToken, typeof(CancellationToken))), cancellationToken);
        }

        private static MethodInfo? s_MinAsync__TSource__3__6;
        
        private static MethodInfo? MinAsync__TSource__3__6(Type TSource) =>
            (s_MinAsync__TSource__3__6 ??
            (s_MinAsync__TSource__3__6 = new Func<IAsyncQueryable<object>, Expression<Func<object, int?>>, CancellationToken, ValueTask<int?>>(MinAsync<object>).GetMethodInfo()!.GetGenericMethodDefinition())).MakeGenericMethod(TSource);

        public static ValueTask<int?> MinAsync<TSource>(this IAsyncQueryable<TSource> source, Expression<Func<TSource, int?>> selector, CancellationToken cancellationToken = default)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));
            if (selector == null)
                throw new ArgumentNullException(nameof(selector));

            return source.Provider.ExecuteAsync<int?>(Expression.Call(MinAsync__TSource__3__6(typeof(TSource)), source.Expression, selector, Expression.Constant(cancellationToken, typeof(CancellationToken))), cancellationToken);
        }

        private static MethodInfo? s_MinAsync__TSource__3__7;
        
        private static MethodInfo? MinAsync__TSource__3__7(Type TSource) =>
            (s_MinAsync__TSource__3__7 ??
            (s_MinAsync__TSource__3__7 = new Func<IAsyncQueryable<object>, Expression<Func<object, int>>, CancellationToken, ValueTask<int>>(MinAsync<object>).GetMethodInfo()!.GetGenericMethodDefinition())).MakeGenericMethod(TSource);

        public static ValueTask<int> MinAsync<TSource>(this IAsyncQueryable<TSource> source, Expression<Func<TSource, int>> selector, CancellationToken cancellationToken = default)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));
            if (selector == null)
                throw new ArgumentNullException(nameof(selector));

            return source.Provider.ExecuteAsync<int>(Expression.Call(MinAsync__TSource__3__7(typeof(TSource)), source.Expression, selector, Expression.Constant(cancellationToken, typeof(CancellationToken))), cancellationToken);
        }

        private static MethodInfo? s_MinAsync__TSource__3__8;
        
        private static MethodInfo? MinAsync__TSource__3__8(Type TSource) =>
            (s_MinAsync__TSource__3__8 ??
            (s_MinAsync__TSource__3__8 = new Func<IAsyncQueryable<object>, Expression<Func<object, long?>>, CancellationToken, ValueTask<long?>>(MinAsync<object>).GetMethodInfo()!.GetGenericMethodDefinition())).MakeGenericMethod(TSource);

        public static ValueTask<long?> MinAsync<TSource>(this IAsyncQueryable<TSource> source, Expression<Func<TSource, long?>> selector, CancellationToken cancellationToken = default)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));
            if (selector == null)
                throw new ArgumentNullException(nameof(selector));

            return source.Provider.ExecuteAsync<long?>(Expression.Call(MinAsync__TSource__3__8(typeof(TSource)), source.Expression, selector, Expression.Constant(cancellationToken, typeof(CancellationToken))), cancellationToken);
        }

        private static MethodInfo? s_MinAsync__TSource__3__9;
        
        private static MethodInfo? MinAsync__TSource__3__9(Type TSource) =>
            (s_MinAsync__TSource__3__9 ??
            (s_MinAsync__TSource__3__9 = new Func<IAsyncQueryable<object>, Expression<Func<object, long>>, CancellationToken, ValueTask<long>>(MinAsync<object>).GetMethodInfo()!.GetGenericMethodDefinition())).MakeGenericMethod(TSource);

        public static ValueTask<long> MinAsync<TSource>(this IAsyncQueryable<TSource> source, Expression<Func<TSource, long>> selector, CancellationToken cancellationToken = default)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));
            if (selector == null)
                throw new ArgumentNullException(nameof(selector));

            return source.Provider.ExecuteAsync<long>(Expression.Call(MinAsync__TSource__3__9(typeof(TSource)), source.Expression, selector, Expression.Constant(cancellationToken, typeof(CancellationToken))), cancellationToken);
        }

        private static MethodInfo? s_MinAsync__TSource_TResult__3__0;
        
        private static MethodInfo? MinAsync__TSource_TResult__3__0(Type TSource, Type TResult) =>
            (s_MinAsync__TSource_TResult__3__0 ??
            (s_MinAsync__TSource_TResult__3__0 = new Func<IAsyncQueryable<object>, Expression<Func<object, object>>, CancellationToken, ValueTask<object>>(MinAsync<object, object>).GetMethodInfo()!.GetGenericMethodDefinition())).MakeGenericMethod(TSource, TResult);

        public static ValueTask<TResult> MinAsync<TSource, TResult>(this IAsyncQueryable<TSource> source, Expression<Func<TSource, TResult>> selector, CancellationToken cancellationToken = default)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));
            if (selector == null)
                throw new ArgumentNullException(nameof(selector));

            return source.Provider.ExecuteAsync<TResult>(Expression.Call(MinAsync__TSource_TResult__3__0(typeof(TSource), typeof(TResult)), source.Expression, selector, Expression.Constant(cancellationToken, typeof(CancellationToken))), cancellationToken);
        }

        private static MethodInfo? s_MinAwaitAsync__TSource__3__0;
        
        private static MethodInfo? MinAwaitAsync__TSource__3__0(Type TSource) =>
            (s_MinAwaitAsync__TSource__3__0 ??
            (s_MinAwaitAsync__TSource__3__0 = new Func<IAsyncQueryable<object>, Expression<Func<object, ValueTask<decimal?>>>, CancellationToken, ValueTask<decimal?>>(MinAwaitAsync<object>).GetMethodInfo()!.GetGenericMethodDefinition())).MakeGenericMethod(TSource);

        public static ValueTask<decimal?> MinAwaitAsync<TSource>(this IAsyncQueryable<TSource> source, Expression<Func<TSource, ValueTask<decimal?>>> selector, CancellationToken cancellationToken = default)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));
            if (selector == null)
                throw new ArgumentNullException(nameof(selector));

            return source.Provider.ExecuteAsync<decimal?>(Expression.Call(MinAwaitAsync__TSource__3__0(typeof(TSource)), source.Expression, selector, Expression.Constant(cancellationToken, typeof(CancellationToken))), cancellationToken);
        }

        private static MethodInfo? s_MinAwaitAsync__TSource__3__1;
        
        private static MethodInfo? MinAwaitAsync__TSource__3__1(Type TSource) =>
            (s_MinAwaitAsync__TSource__3__1 ??
            (s_MinAwaitAsync__TSource__3__1 = new Func<IAsyncQueryable<object>, Expression<Func<object, ValueTask<decimal>>>, CancellationToken, ValueTask<decimal>>(MinAwaitAsync<object>).GetMethodInfo()!.GetGenericMethodDefinition())).MakeGenericMethod(TSource);

        public static ValueTask<decimal> MinAwaitAsync<TSource>(this IAsyncQueryable<TSource> source, Expression<Func<TSource, ValueTask<decimal>>> selector, CancellationToken cancellationToken = default)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));
            if (selector == null)
                throw new ArgumentNullException(nameof(selector));

            return source.Provider.ExecuteAsync<decimal>(Expression.Call(MinAwaitAsync__TSource__3__1(typeof(TSource)), source.Expression, selector, Expression.Constant(cancellationToken, typeof(CancellationToken))), cancellationToken);
        }

        private static MethodInfo? s_MinAwaitAsync__TSource__3__2;
        
        private static MethodInfo? MinAwaitAsync__TSource__3__2(Type TSource) =>
            (s_MinAwaitAsync__TSource__3__2 ??
            (s_MinAwaitAsync__TSource__3__2 = new Func<IAsyncQueryable<object>, Expression<Func<object, ValueTask<double?>>>, CancellationToken, ValueTask<double?>>(MinAwaitAsync<object>).GetMethodInfo()!.GetGenericMethodDefinition())).MakeGenericMethod(TSource);

        public static ValueTask<double?> MinAwaitAsync<TSource>(this IAsyncQueryable<TSource> source, Expression<Func<TSource, ValueTask<double?>>> selector, CancellationToken cancellationToken = default)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));
            if (selector == null)
                throw new ArgumentNullException(nameof(selector));

            return source.Provider.ExecuteAsync<double?>(Expression.Call(MinAwaitAsync__TSource__3__2(typeof(TSource)), source.Expression, selector, Expression.Constant(cancellationToken, typeof(CancellationToken))), cancellationToken);
        }

        private static MethodInfo? s_MinAwaitAsync__TSource__3__3;
        
        private static MethodInfo? MinAwaitAsync__TSource__3__3(Type TSource) =>
            (s_MinAwaitAsync__TSource__3__3 ??
            (s_MinAwaitAsync__TSource__3__3 = new Func<IAsyncQueryable<object>, Expression<Func<object, ValueTask<double>>>, CancellationToken, ValueTask<double>>(MinAwaitAsync<object>).GetMethodInfo()!.GetGenericMethodDefinition())).MakeGenericMethod(TSource);

        public static ValueTask<double> MinAwaitAsync<TSource>(this IAsyncQueryable<TSource> source, Expression<Func<TSource, ValueTask<double>>> selector, CancellationToken cancellationToken = default)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));
            if (selector == null)
                throw new ArgumentNullException(nameof(selector));

            return source.Provider.ExecuteAsync<double>(Expression.Call(MinAwaitAsync__TSource__3__3(typeof(TSource)), source.Expression, selector, Expression.Constant(cancellationToken, typeof(CancellationToken))), cancellationToken);
        }

        private static MethodInfo? s_MinAwaitAsync__TSource__3__4;
        
        private static MethodInfo? MinAwaitAsync__TSource__3__4(Type TSource) =>
            (s_MinAwaitAsync__TSource__3__4 ??
            (s_MinAwaitAsync__TSource__3__4 = new Func<IAsyncQueryable<object>, Expression<Func<object, ValueTask<float?>>>, CancellationToken, ValueTask<float?>>(MinAwaitAsync<object>).GetMethodInfo()!.GetGenericMethodDefinition())).MakeGenericMethod(TSource);

        public static ValueTask<float?> MinAwaitAsync<TSource>(this IAsyncQueryable<TSource> source, Expression<Func<TSource, ValueTask<float?>>> selector, CancellationToken cancellationToken = default)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));
            if (selector == null)
                throw new ArgumentNullException(nameof(selector));

            return source.Provider.ExecuteAsync<float?>(Expression.Call(MinAwaitAsync__TSource__3__4(typeof(TSource)), source.Expression, selector, Expression.Constant(cancellationToken, typeof(CancellationToken))), cancellationToken);
        }

        private static MethodInfo? s_MinAwaitAsync__TSource__3__5;
        
        private static MethodInfo? MinAwaitAsync__TSource__3__5(Type TSource) =>
            (s_MinAwaitAsync__TSource__3__5 ??
            (s_MinAwaitAsync__TSource__3__5 = new Func<IAsyncQueryable<object>, Expression<Func<object, ValueTask<float>>>, CancellationToken, ValueTask<float>>(MinAwaitAsync<object>).GetMethodInfo()!.GetGenericMethodDefinition())).MakeGenericMethod(TSource);

        public static ValueTask<float> MinAwaitAsync<TSource>(this IAsyncQueryable<TSource> source, Expression<Func<TSource, ValueTask<float>>> selector, CancellationToken cancellationToken = default)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));
            if (selector == null)
                throw new ArgumentNullException(nameof(selector));

            return source.Provider.ExecuteAsync<float>(Expression.Call(MinAwaitAsync__TSource__3__5(typeof(TSource)), source.Expression, selector, Expression.Constant(cancellationToken, typeof(CancellationToken))), cancellationToken);
        }

        private static MethodInfo? s_MinAwaitAsync__TSource__3__6;
        
        private static MethodInfo? MinAwaitAsync__TSource__3__6(Type TSource) =>
            (s_MinAwaitAsync__TSource__3__6 ??
            (s_MinAwaitAsync__TSource__3__6 = new Func<IAsyncQueryable<object>, Expression<Func<object, ValueTask<int?>>>, CancellationToken, ValueTask<int?>>(MinAwaitAsync<object>).GetMethodInfo()!.GetGenericMethodDefinition())).MakeGenericMethod(TSource);

        public static ValueTask<int?> MinAwaitAsync<TSource>(this IAsyncQueryable<TSource> source, Expression<Func<TSource, ValueTask<int?>>> selector, CancellationToken cancellationToken = default)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));
            if (selector == null)
                throw new ArgumentNullException(nameof(selector));

            return source.Provider.ExecuteAsync<int?>(Expression.Call(MinAwaitAsync__TSource__3__6(typeof(TSource)), source.Expression, selector, Expression.Constant(cancellationToken, typeof(CancellationToken))), cancellationToken);
        }

        private static MethodInfo? s_MinAwaitAsync__TSource__3__7;
        
        private static MethodInfo? MinAwaitAsync__TSource__3__7(Type TSource) =>
            (s_MinAwaitAsync__TSource__3__7 ??
            (s_MinAwaitAsync__TSource__3__7 = new Func<IAsyncQueryable<object>, Expression<Func<object, ValueTask<int>>>, CancellationToken, ValueTask<int>>(MinAwaitAsync<object>).GetMethodInfo()!.GetGenericMethodDefinition())).MakeGenericMethod(TSource);

        public static ValueTask<int> MinAwaitAsync<TSource>(this IAsyncQueryable<TSource> source, Expression<Func<TSource, ValueTask<int>>> selector, CancellationToken cancellationToken = default)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));
            if (selector == null)
                throw new ArgumentNullException(nameof(selector));

            return source.Provider.ExecuteAsync<int>(Expression.Call(MinAwaitAsync__TSource__3__7(typeof(TSource)), source.Expression, selector, Expression.Constant(cancellationToken, typeof(CancellationToken))), cancellationToken);
        }

        private static MethodInfo? s_MinAwaitAsync__TSource__3__8;
        
        private static MethodInfo? MinAwaitAsync__TSource__3__8(Type TSource) =>
            (s_MinAwaitAsync__TSource__3__8 ??
            (s_MinAwaitAsync__TSource__3__8 = new Func<IAsyncQueryable<object>, Expression<Func<object, ValueTask<long?>>>, CancellationToken, ValueTask<long?>>(MinAwaitAsync<object>).GetMethodInfo()!.GetGenericMethodDefinition())).MakeGenericMethod(TSource);

        public static ValueTask<long?> MinAwaitAsync<TSource>(this IAsyncQueryable<TSource> source, Expression<Func<TSource, ValueTask<long?>>> selector, CancellationToken cancellationToken = default)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));
            if (selector == null)
                throw new ArgumentNullException(nameof(selector));

            return source.Provider.ExecuteAsync<long?>(Expression.Call(MinAwaitAsync__TSource__3__8(typeof(TSource)), source.Expression, selector, Expression.Constant(cancellationToken, typeof(CancellationToken))), cancellationToken);
        }

        private static MethodInfo? s_MinAwaitAsync__TSource__3__9;
        
        private static MethodInfo? MinAwaitAsync__TSource__3__9(Type TSource) =>
            (s_MinAwaitAsync__TSource__3__9 ??
            (s_MinAwaitAsync__TSource__3__9 = new Func<IAsyncQueryable<object>, Expression<Func<object, ValueTask<long>>>, CancellationToken, ValueTask<long>>(MinAwaitAsync<object>).GetMethodInfo()!.GetGenericMethodDefinition())).MakeGenericMethod(TSource);

        public static ValueTask<long> MinAwaitAsync<TSource>(this IAsyncQueryable<TSource> source, Expression<Func<TSource, ValueTask<long>>> selector, CancellationToken cancellationToken = default)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));
            if (selector == null)
                throw new ArgumentNullException(nameof(selector));

            return source.Provider.ExecuteAsync<long>(Expression.Call(MinAwaitAsync__TSource__3__9(typeof(TSource)), source.Expression, selector, Expression.Constant(cancellationToken, typeof(CancellationToken))), cancellationToken);
        }

        private static MethodInfo? s_MinAwaitAsync__TSource_TResult__3__0;
        
        private static MethodInfo? MinAwaitAsync__TSource_TResult__3__0(Type TSource, Type TResult) =>
            (s_MinAwaitAsync__TSource_TResult__3__0 ??
            (s_MinAwaitAsync__TSource_TResult__3__0 = new Func<IAsyncQueryable<object>, Expression<Func<object, ValueTask<object>>>, CancellationToken, ValueTask<object>>(MinAwaitAsync<object, object>).GetMethodInfo()!.GetGenericMethodDefinition())).MakeGenericMethod(TSource, TResult);

        public static ValueTask<TResult> MinAwaitAsync<TSource, TResult>(this IAsyncQueryable<TSource> source, Expression<Func<TSource, ValueTask<TResult>>> selector, CancellationToken cancellationToken = default)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));
            if (selector == null)
                throw new ArgumentNullException(nameof(selector));

            return source.Provider.ExecuteAsync<TResult>(Expression.Call(MinAwaitAsync__TSource_TResult__3__0(typeof(TSource), typeof(TResult)), source.Expression, selector, Expression.Constant(cancellationToken, typeof(CancellationToken))), cancellationToken);
        }

        private static MethodInfo? s_MinAwaitWithCancellationAsync__TSource__3__0;
        
        private static MethodInfo? MinAwaitWithCancellationAsync__TSource__3__0(Type TSource) =>
            (s_MinAwaitWithCancellationAsync__TSource__3__0 ??
            (s_MinAwaitWithCancellationAsync__TSource__3__0 = new Func<IAsyncQueryable<object>, Expression<Func<object, CancellationToken, ValueTask<decimal?>>>, CancellationToken, ValueTask<decimal?>>(MinAwaitWithCancellationAsync<object>).GetMethodInfo()!.GetGenericMethodDefinition())).MakeGenericMethod(TSource);

        public static ValueTask<decimal?> MinAwaitWithCancellationAsync<TSource>(this IAsyncQueryable<TSource> source, Expression<Func<TSource, CancellationToken, ValueTask<decimal?>>> selector, CancellationToken cancellationToken = default)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));
            if (selector == null)
                throw new ArgumentNullException(nameof(selector));

            return source.Provider.ExecuteAsync<decimal?>(Expression.Call(MinAwaitWithCancellationAsync__TSource__3__0(typeof(TSource)), source.Expression, selector, Expression.Constant(cancellationToken, typeof(CancellationToken))), cancellationToken);
        }

        private static MethodInfo? s_MinAwaitWithCancellationAsync__TSource__3__1;
        
        private static MethodInfo? MinAwaitWithCancellationAsync__TSource__3__1(Type TSource) =>
            (s_MinAwaitWithCancellationAsync__TSource__3__1 ??
            (s_MinAwaitWithCancellationAsync__TSource__3__1 = new Func<IAsyncQueryable<object>, Expression<Func<object, CancellationToken, ValueTask<decimal>>>, CancellationToken, ValueTask<decimal>>(MinAwaitWithCancellationAsync<object>).GetMethodInfo()!.GetGenericMethodDefinition())).MakeGenericMethod(TSource);

        public static ValueTask<decimal> MinAwaitWithCancellationAsync<TSource>(this IAsyncQueryable<TSource> source, Expression<Func<TSource, CancellationToken, ValueTask<decimal>>> selector, CancellationToken cancellationToken = default)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));
            if (selector == null)
                throw new ArgumentNullException(nameof(selector));

            return source.Provider.ExecuteAsync<decimal>(Expression.Call(MinAwaitWithCancellationAsync__TSource__3__1(typeof(TSource)), source.Expression, selector, Expression.Constant(cancellationToken, typeof(CancellationToken))), cancellationToken);
        }

        private static MethodInfo? s_MinAwaitWithCancellationAsync__TSource__3__2;
        
        private static MethodInfo? MinAwaitWithCancellationAsync__TSource__3__2(Type TSource) =>
            (s_MinAwaitWithCancellationAsync__TSource__3__2 ??
            (s_MinAwaitWithCancellationAsync__TSource__3__2 = new Func<IAsyncQueryable<object>, Expression<Func<object, CancellationToken, ValueTask<double?>>>, CancellationToken, ValueTask<double?>>(MinAwaitWithCancellationAsync<object>).GetMethodInfo()!.GetGenericMethodDefinition())).MakeGenericMethod(TSource);

        public static ValueTask<double?> MinAwaitWithCancellationAsync<TSource>(this IAsyncQueryable<TSource> source, Expression<Func<TSource, CancellationToken, ValueTask<double?>>> selector, CancellationToken cancellationToken = default)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));
            if (selector == null)
                throw new ArgumentNullException(nameof(selector));

            return source.Provider.ExecuteAsync<double?>(Expression.Call(MinAwaitWithCancellationAsync__TSource__3__2(typeof(TSource)), source.Expression, selector, Expression.Constant(cancellationToken, typeof(CancellationToken))), cancellationToken);
        }

        private static MethodInfo? s_MinAwaitWithCancellationAsync__TSource__3__3;
        
        private static MethodInfo? MinAwaitWithCancellationAsync__TSource__3__3(Type TSource) =>
            (s_MinAwaitWithCancellationAsync__TSource__3__3 ??
            (s_MinAwaitWithCancellationAsync__TSource__3__3 = new Func<IAsyncQueryable<object>, Expression<Func<object, CancellationToken, ValueTask<double>>>, CancellationToken, ValueTask<double>>(MinAwaitWithCancellationAsync<object>).GetMethodInfo()!.GetGenericMethodDefinition())).MakeGenericMethod(TSource);

        public static ValueTask<double> MinAwaitWithCancellationAsync<TSource>(this IAsyncQueryable<TSource> source, Expression<Func<TSource, CancellationToken, ValueTask<double>>> selector, CancellationToken cancellationToken = default)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));
            if (selector == null)
                throw new ArgumentNullException(nameof(selector));

            return source.Provider.ExecuteAsync<double>(Expression.Call(MinAwaitWithCancellationAsync__TSource__3__3(typeof(TSource)), source.Expression, selector, Expression.Constant(cancellationToken, typeof(CancellationToken))), cancellationToken);
        }

        private static MethodInfo? s_MinAwaitWithCancellationAsync__TSource__3__4;
        
        private static MethodInfo? MinAwaitWithCancellationAsync__TSource__3__4(Type TSource) =>
            (s_MinAwaitWithCancellationAsync__TSource__3__4 ??
            (s_MinAwaitWithCancellationAsync__TSource__3__4 = new Func<IAsyncQueryable<object>, Expression<Func<object, CancellationToken, ValueTask<float?>>>, CancellationToken, ValueTask<float?>>(MinAwaitWithCancellationAsync<object>).GetMethodInfo()!.GetGenericMethodDefinition())).MakeGenericMethod(TSource);

        public static ValueTask<float?> MinAwaitWithCancellationAsync<TSource>(this IAsyncQueryable<TSource> source, Expression<Func<TSource, CancellationToken, ValueTask<float?>>> selector, CancellationToken cancellationToken = default)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));
            if (selector == null)
                throw new ArgumentNullException(nameof(selector));

            return source.Provider.ExecuteAsync<float?>(Expression.Call(MinAwaitWithCancellationAsync__TSource__3__4(typeof(TSource)), source.Expression, selector, Expression.Constant(cancellationToken, typeof(CancellationToken))), cancellationToken);
        }

        private static MethodInfo? s_MinAwaitWithCancellationAsync__TSource__3__5;
        
        private static MethodInfo? MinAwaitWithCancellationAsync__TSource__3__5(Type TSource) =>
            (s_MinAwaitWithCancellationAsync__TSource__3__5 ??
            (s_MinAwaitWithCancellationAsync__TSource__3__5 = new Func<IAsyncQueryable<object>, Expression<Func<object, CancellationToken, ValueTask<float>>>, CancellationToken, ValueTask<float>>(MinAwaitWithCancellationAsync<object>).GetMethodInfo()!.GetGenericMethodDefinition())).MakeGenericMethod(TSource);

        public static ValueTask<float> MinAwaitWithCancellationAsync<TSource>(this IAsyncQueryable<TSource> source, Expression<Func<TSource, CancellationToken, ValueTask<float>>> selector, CancellationToken cancellationToken = default)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));
            if (selector == null)
                throw new ArgumentNullException(nameof(selector));

            return source.Provider.ExecuteAsync<float>(Expression.Call(MinAwaitWithCancellationAsync__TSource__3__5(typeof(TSource)), source.Expression, selector, Expression.Constant(cancellationToken, typeof(CancellationToken))), cancellationToken);
        }

        private static MethodInfo? s_MinAwaitWithCancellationAsync__TSource__3__6;
        
        private static MethodInfo? MinAwaitWithCancellationAsync__TSource__3__6(Type TSource) =>
            (s_MinAwaitWithCancellationAsync__TSource__3__6 ??
            (s_MinAwaitWithCancellationAsync__TSource__3__6 = new Func<IAsyncQueryable<object>, Expression<Func<object, CancellationToken, ValueTask<int?>>>, CancellationToken, ValueTask<int?>>(MinAwaitWithCancellationAsync<object>).GetMethodInfo()!.GetGenericMethodDefinition())).MakeGenericMethod(TSource);

        public static ValueTask<int?> MinAwaitWithCancellationAsync<TSource>(this IAsyncQueryable<TSource> source, Expression<Func<TSource, CancellationToken, ValueTask<int?>>> selector, CancellationToken cancellationToken = default)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));
            if (selector == null)
                throw new ArgumentNullException(nameof(selector));

            return source.Provider.ExecuteAsync<int?>(Expression.Call(MinAwaitWithCancellationAsync__TSource__3__6(typeof(TSource)), source.Expression, selector, Expression.Constant(cancellationToken, typeof(CancellationToken))), cancellationToken);
        }

        private static MethodInfo? s_MinAwaitWithCancellationAsync__TSource__3__7;
        
        private static MethodInfo? MinAwaitWithCancellationAsync__TSource__3__7(Type TSource) =>
            (s_MinAwaitWithCancellationAsync__TSource__3__7 ??
            (s_MinAwaitWithCancellationAsync__TSource__3__7 = new Func<IAsyncQueryable<object>, Expression<Func<object, CancellationToken, ValueTask<int>>>, CancellationToken, ValueTask<int>>(MinAwaitWithCancellationAsync<object>).GetMethodInfo()!.GetGenericMethodDefinition())).MakeGenericMethod(TSource);

        public static ValueTask<int> MinAwaitWithCancellationAsync<TSource>(this IAsyncQueryable<TSource> source, Expression<Func<TSource, CancellationToken, ValueTask<int>>> selector, CancellationToken cancellationToken = default)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));
            if (selector == null)
                throw new ArgumentNullException(nameof(selector));

            return source.Provider.ExecuteAsync<int>(Expression.Call(MinAwaitWithCancellationAsync__TSource__3__7(typeof(TSource)), source.Expression, selector, Expression.Constant(cancellationToken, typeof(CancellationToken))), cancellationToken);
        }

        private static MethodInfo? s_MinAwaitWithCancellationAsync__TSource__3__8;
        
        private static MethodInfo? MinAwaitWithCancellationAsync__TSource__3__8(Type TSource) =>
            (s_MinAwaitWithCancellationAsync__TSource__3__8 ??
            (s_MinAwaitWithCancellationAsync__TSource__3__8 = new Func<IAsyncQueryable<object>, Expression<Func<object, CancellationToken, ValueTask<long?>>>, CancellationToken, ValueTask<long?>>(MinAwaitWithCancellationAsync<object>).GetMethodInfo()!.GetGenericMethodDefinition())).MakeGenericMethod(TSource);

        public static ValueTask<long?> MinAwaitWithCancellationAsync<TSource>(this IAsyncQueryable<TSource> source, Expression<Func<TSource, CancellationToken, ValueTask<long?>>> selector, CancellationToken cancellationToken = default)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));
            if (selector == null)
                throw new ArgumentNullException(nameof(selector));

            return source.Provider.ExecuteAsync<long?>(Expression.Call(MinAwaitWithCancellationAsync__TSource__3__8(typeof(TSource)), source.Expression, selector, Expression.Constant(cancellationToken, typeof(CancellationToken))), cancellationToken);
        }

        private static MethodInfo? s_MinAwaitWithCancellationAsync__TSource__3__9;
        
        private static MethodInfo? MinAwaitWithCancellationAsync__TSource__3__9(Type TSource) =>
            (s_MinAwaitWithCancellationAsync__TSource__3__9 ??
            (s_MinAwaitWithCancellationAsync__TSource__3__9 = new Func<IAsyncQueryable<object>, Expression<Func<object, CancellationToken, ValueTask<long>>>, CancellationToken, ValueTask<long>>(MinAwaitWithCancellationAsync<object>).GetMethodInfo()!.GetGenericMethodDefinition())).MakeGenericMethod(TSource);

        public static ValueTask<long> MinAwaitWithCancellationAsync<TSource>(this IAsyncQueryable<TSource> source, Expression<Func<TSource, CancellationToken, ValueTask<long>>> selector, CancellationToken cancellationToken = default)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));
            if (selector == null)
                throw new ArgumentNullException(nameof(selector));

            return source.Provider.ExecuteAsync<long>(Expression.Call(MinAwaitWithCancellationAsync__TSource__3__9(typeof(TSource)), source.Expression, selector, Expression.Constant(cancellationToken, typeof(CancellationToken))), cancellationToken);
        }

        private static MethodInfo? s_MinAwaitWithCancellationAsync__TSource_TResult__3__0;
        
        private static MethodInfo? MinAwaitWithCancellationAsync__TSource_TResult__3__0(Type TSource, Type TResult) =>
            (s_MinAwaitWithCancellationAsync__TSource_TResult__3__0 ??
            (s_MinAwaitWithCancellationAsync__TSource_TResult__3__0 = new Func<IAsyncQueryable<object>, Expression<Func<object, CancellationToken, ValueTask<object>>>, CancellationToken, ValueTask<object>>(MinAwaitWithCancellationAsync<object, object>).GetMethodInfo()!.GetGenericMethodDefinition())).MakeGenericMethod(TSource, TResult);

        public static ValueTask<TResult> MinAwaitWithCancellationAsync<TSource, TResult>(this IAsyncQueryable<TSource> source, Expression<Func<TSource, CancellationToken, ValueTask<TResult>>> selector, CancellationToken cancellationToken = default)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));
            if (selector == null)
                throw new ArgumentNullException(nameof(selector));

            return source.Provider.ExecuteAsync<TResult>(Expression.Call(MinAwaitWithCancellationAsync__TSource_TResult__3__0(typeof(TSource), typeof(TResult)), source.Expression, selector, Expression.Constant(cancellationToken, typeof(CancellationToken))), cancellationToken);
        }

        private static MethodInfo? s_OfType__TResult__1__0;
        
        private static MethodInfo? OfType__TResult__1__0(Type TResult) =>
            (s_OfType__TResult__1__0 ??
            (s_OfType__TResult__1__0 = new Func<IAsyncQueryable<object>, IAsyncQueryable<object>>(OfType<object>).GetMethodInfo()!.GetGenericMethodDefinition())).MakeGenericMethod(TResult);

        public static IAsyncQueryable<TResult> OfType<TResult>(this IAsyncQueryable<object> source)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));

            return source.Provider.CreateQuery<TResult>(Expression.Call(OfType__TResult__1__0(typeof(TResult)), source.Expression));
        }

        private static MethodInfo? s_OrderBy__TSource_TKey__2__0;
        
        private static MethodInfo? OrderBy__TSource_TKey__2__0(Type TSource, Type TKey) =>
            (s_OrderBy__TSource_TKey__2__0 ??
            (s_OrderBy__TSource_TKey__2__0 = new Func<IAsyncQueryable<object>, Expression<Func<object, object>>, IOrderedAsyncQueryable<object>>(OrderBy<object, object>).GetMethodInfo()!.GetGenericMethodDefinition())).MakeGenericMethod(TSource, TKey);

        public static IOrderedAsyncQueryable<TSource> OrderBy<TSource, TKey>(this IAsyncQueryable<TSource> source, Expression<Func<TSource, TKey>> keySelector)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));
            if (keySelector == null)
                throw new ArgumentNullException(nameof(keySelector));

            return (IOrderedAsyncQueryable<TSource>)source.Provider.CreateQuery<TSource>(Expression.Call(OrderBy__TSource_TKey__2__0(typeof(TSource), typeof(TKey)), source.Expression, keySelector));
        }

        private static MethodInfo? s_OrderBy__TSource_TKey__3__0;
        
        private static MethodInfo? OrderBy__TSource_TKey__3__0(Type TSource, Type TKey) =>
            (s_OrderBy__TSource_TKey__3__0 ??
            (s_OrderBy__TSource_TKey__3__0 = new Func<IAsyncQueryable<object>, Expression<Func<object, object>>, IComparer<object>, IOrderedAsyncQueryable<object>>(OrderBy<object, object>).GetMethodInfo()!.GetGenericMethodDefinition())).MakeGenericMethod(TSource, TKey);

        public static IOrderedAsyncQueryable<TSource> OrderBy<TSource, TKey>(this IAsyncQueryable<TSource> source, Expression<Func<TSource, TKey>> keySelector, IComparer<TKey>? comparer)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));
            if (keySelector == null)
                throw new ArgumentNullException(nameof(keySelector));

            return (IOrderedAsyncQueryable<TSource>)source.Provider.CreateQuery<TSource>(Expression.Call(OrderBy__TSource_TKey__3__0(typeof(TSource), typeof(TKey)), source.Expression, keySelector, Expression.Constant(comparer, typeof(IComparer<TKey>))));
        }

        private static MethodInfo? s_OrderByAwait__TSource_TKey__2__0;
        
        private static MethodInfo? OrderByAwait__TSource_TKey__2__0(Type TSource, Type TKey) =>
            (s_OrderByAwait__TSource_TKey__2__0 ??
            (s_OrderByAwait__TSource_TKey__2__0 = new Func<IAsyncQueryable<object>, Expression<Func<object, ValueTask<object>>>, IOrderedAsyncQueryable<object>>(OrderByAwait<object, object>).GetMethodInfo()!.GetGenericMethodDefinition())).MakeGenericMethod(TSource, TKey);

        public static IOrderedAsyncQueryable<TSource> OrderByAwait<TSource, TKey>(this IAsyncQueryable<TSource> source, Expression<Func<TSource, ValueTask<TKey>>> keySelector)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));
            if (keySelector == null)
                throw new ArgumentNullException(nameof(keySelector));

            return (IOrderedAsyncQueryable<TSource>)source.Provider.CreateQuery<TSource>(Expression.Call(OrderByAwait__TSource_TKey__2__0(typeof(TSource), typeof(TKey)), source.Expression, keySelector));
        }

        private static MethodInfo? s_OrderByAwait__TSource_TKey__3__0;
        
        private static MethodInfo? OrderByAwait__TSource_TKey__3__0(Type TSource, Type TKey) =>
            (s_OrderByAwait__TSource_TKey__3__0 ??
            (s_OrderByAwait__TSource_TKey__3__0 = new Func<IAsyncQueryable<object>, Expression<Func<object, ValueTask<object>>>, IComparer<object>, IOrderedAsyncQueryable<object>>(OrderByAwait<object, object>).GetMethodInfo()!.GetGenericMethodDefinition())).MakeGenericMethod(TSource, TKey);

        public static IOrderedAsyncQueryable<TSource> OrderByAwait<TSource, TKey>(this IAsyncQueryable<TSource> source, Expression<Func<TSource, ValueTask<TKey>>> keySelector, IComparer<TKey>? comparer)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));
            if (keySelector == null)
                throw new ArgumentNullException(nameof(keySelector));

            return (IOrderedAsyncQueryable<TSource>)source.Provider.CreateQuery<TSource>(Expression.Call(OrderByAwait__TSource_TKey__3__0(typeof(TSource), typeof(TKey)), source.Expression, keySelector, Expression.Constant(comparer, typeof(IComparer<TKey>))));
        }

        private static MethodInfo? s_OrderByAwaitWithCancellation__TSource_TKey__2__0;
        
        private static MethodInfo? OrderByAwaitWithCancellation__TSource_TKey__2__0(Type TSource, Type TKey) =>
            (s_OrderByAwaitWithCancellation__TSource_TKey__2__0 ??
            (s_OrderByAwaitWithCancellation__TSource_TKey__2__0 = new Func<IAsyncQueryable<object>, Expression<Func<object, CancellationToken, ValueTask<object>>>, IOrderedAsyncQueryable<object>>(OrderByAwaitWithCancellation<object, object>).GetMethodInfo()!.GetGenericMethodDefinition())).MakeGenericMethod(TSource, TKey);

        public static IOrderedAsyncQueryable<TSource> OrderByAwaitWithCancellation<TSource, TKey>(this IAsyncQueryable<TSource> source, Expression<Func<TSource, CancellationToken, ValueTask<TKey>>> keySelector)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));
            if (keySelector == null)
                throw new ArgumentNullException(nameof(keySelector));

            return (IOrderedAsyncQueryable<TSource>)source.Provider.CreateQuery<TSource>(Expression.Call(OrderByAwaitWithCancellation__TSource_TKey__2__0(typeof(TSource), typeof(TKey)), source.Expression, keySelector));
        }

        private static MethodInfo? s_OrderByAwaitWithCancellation__TSource_TKey__3__0;
        
        private static MethodInfo? OrderByAwaitWithCancellation__TSource_TKey__3__0(Type TSource, Type TKey) =>
            (s_OrderByAwaitWithCancellation__TSource_TKey__3__0 ??
            (s_OrderByAwaitWithCancellation__TSource_TKey__3__0 = new Func<IAsyncQueryable<object>, Expression<Func<object, CancellationToken, ValueTask<object>>>, IComparer<object>, IOrderedAsyncQueryable<object>>(OrderByAwaitWithCancellation<object, object>).GetMethodInfo()!.GetGenericMethodDefinition())).MakeGenericMethod(TSource, TKey);

        public static IOrderedAsyncQueryable<TSource> OrderByAwaitWithCancellation<TSource, TKey>(this IAsyncQueryable<TSource> source, Expression<Func<TSource, CancellationToken, ValueTask<TKey>>> keySelector, IComparer<TKey>? comparer)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));
            if (keySelector == null)
                throw new ArgumentNullException(nameof(keySelector));

            return (IOrderedAsyncQueryable<TSource>)source.Provider.CreateQuery<TSource>(Expression.Call(OrderByAwaitWithCancellation__TSource_TKey__3__0(typeof(TSource), typeof(TKey)), source.Expression, keySelector, Expression.Constant(comparer, typeof(IComparer<TKey>))));
        }

        private static MethodInfo? s_OrderByDescending__TSource_TKey__2__0;
        
        private static MethodInfo? OrderByDescending__TSource_TKey__2__0(Type TSource, Type TKey) =>
            (s_OrderByDescending__TSource_TKey__2__0 ??
            (s_OrderByDescending__TSource_TKey__2__0 = new Func<IAsyncQueryable<object>, Expression<Func<object, object>>, IOrderedAsyncQueryable<object>>(OrderByDescending<object, object>).GetMethodInfo()!.GetGenericMethodDefinition())).MakeGenericMethod(TSource, TKey);

        public static IOrderedAsyncQueryable<TSource> OrderByDescending<TSource, TKey>(this IAsyncQueryable<TSource> source, Expression<Func<TSource, TKey>> keySelector)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));
            if (keySelector == null)
                throw new ArgumentNullException(nameof(keySelector));

            return (IOrderedAsyncQueryable<TSource>)source.Provider.CreateQuery<TSource>(Expression.Call(OrderByDescending__TSource_TKey__2__0(typeof(TSource), typeof(TKey)), source.Expression, keySelector));
        }

        private static MethodInfo? s_OrderByDescending__TSource_TKey__3__0;
        
        private static MethodInfo? OrderByDescending__TSource_TKey__3__0(Type TSource, Type TKey) =>
            (s_OrderByDescending__TSource_TKey__3__0 ??
            (s_OrderByDescending__TSource_TKey__3__0 = new Func<IAsyncQueryable<object>, Expression<Func<object, object>>, IComparer<object>, IOrderedAsyncQueryable<object>>(OrderByDescending<object, object>).GetMethodInfo()!.GetGenericMethodDefinition())).MakeGenericMethod(TSource, TKey);

        public static IOrderedAsyncQueryable<TSource> OrderByDescending<TSource, TKey>(this IAsyncQueryable<TSource> source, Expression<Func<TSource, TKey>> keySelector, IComparer<TKey>? comparer)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));
            if (keySelector == null)
                throw new ArgumentNullException(nameof(keySelector));

            return (IOrderedAsyncQueryable<TSource>)source.Provider.CreateQuery<TSource>(Expression.Call(OrderByDescending__TSource_TKey__3__0(typeof(TSource), typeof(TKey)), source.Expression, keySelector, Expression.Constant(comparer, typeof(IComparer<TKey>))));
        }

        private static MethodInfo? s_OrderByDescendingAwait__TSource_TKey__2__0;
        
        private static MethodInfo? OrderByDescendingAwait__TSource_TKey__2__0(Type TSource, Type TKey) =>
            (s_OrderByDescendingAwait__TSource_TKey__2__0 ??
            (s_OrderByDescendingAwait__TSource_TKey__2__0 = new Func<IAsyncQueryable<object>, Expression<Func<object, ValueTask<object>>>, IOrderedAsyncQueryable<object>>(OrderByDescendingAwait<object, object>).GetMethodInfo()!.GetGenericMethodDefinition())).MakeGenericMethod(TSource, TKey);

        public static IOrderedAsyncQueryable<TSource> OrderByDescendingAwait<TSource, TKey>(this IAsyncQueryable<TSource> source, Expression<Func<TSource, ValueTask<TKey>>> keySelector)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));
            if (keySelector == null)
                throw new ArgumentNullException(nameof(keySelector));

            return (IOrderedAsyncQueryable<TSource>)source.Provider.CreateQuery<TSource>(Expression.Call(OrderByDescendingAwait__TSource_TKey__2__0(typeof(TSource), typeof(TKey)), source.Expression, keySelector));
        }

        private static MethodInfo? s_OrderByDescendingAwait__TSource_TKey__3__0;
        
        private static MethodInfo? OrderByDescendingAwait__TSource_TKey__3__0(Type TSource, Type TKey) =>
            (s_OrderByDescendingAwait__TSource_TKey__3__0 ??
            (s_OrderByDescendingAwait__TSource_TKey__3__0 = new Func<IAsyncQueryable<object>, Expression<Func<object, ValueTask<object>>>, IComparer<object>, IOrderedAsyncQueryable<object>>(OrderByDescendingAwait<object, object>).GetMethodInfo()!.GetGenericMethodDefinition())).MakeGenericMethod(TSource, TKey);

        public static IOrderedAsyncQueryable<TSource> OrderByDescendingAwait<TSource, TKey>(this IAsyncQueryable<TSource> source, Expression<Func<TSource, ValueTask<TKey>>> keySelector, IComparer<TKey>? comparer)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));
            if (keySelector == null)
                throw new ArgumentNullException(nameof(keySelector));

            return (IOrderedAsyncQueryable<TSource>)source.Provider.CreateQuery<TSource>(Expression.Call(OrderByDescendingAwait__TSource_TKey__3__0(typeof(TSource), typeof(TKey)), source.Expression, keySelector, Expression.Constant(comparer, typeof(IComparer<TKey>))));
        }

        private static MethodInfo? s_OrderByDescendingAwaitWithCancellation__TSource_TKey__2__0;
        
        private static MethodInfo? OrderByDescendingAwaitWithCancellation__TSource_TKey__2__0(Type TSource, Type TKey) =>
            (s_OrderByDescendingAwaitWithCancellation__TSource_TKey__2__0 ??
            (s_OrderByDescendingAwaitWithCancellation__TSource_TKey__2__0 = new Func<IAsyncQueryable<object>, Expression<Func<object, CancellationToken, ValueTask<object>>>, IOrderedAsyncQueryable<object>>(OrderByDescendingAwaitWithCancellation<object, object>).GetMethodInfo()!.GetGenericMethodDefinition())).MakeGenericMethod(TSource, TKey);

        public static IOrderedAsyncQueryable<TSource> OrderByDescendingAwaitWithCancellation<TSource, TKey>(this IAsyncQueryable<TSource> source, Expression<Func<TSource, CancellationToken, ValueTask<TKey>>> keySelector)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));
            if (keySelector == null)
                throw new ArgumentNullException(nameof(keySelector));

            return (IOrderedAsyncQueryable<TSource>)source.Provider.CreateQuery<TSource>(Expression.Call(OrderByDescendingAwaitWithCancellation__TSource_TKey__2__0(typeof(TSource), typeof(TKey)), source.Expression, keySelector));
        }

        private static MethodInfo? s_OrderByDescendingAwaitWithCancellation__TSource_TKey__3__0;
        
        private static MethodInfo? OrderByDescendingAwaitWithCancellation__TSource_TKey__3__0(Type TSource, Type TKey) =>
            (s_OrderByDescendingAwaitWithCancellation__TSource_TKey__3__0 ??
            (s_OrderByDescendingAwaitWithCancellation__TSource_TKey__3__0 = new Func<IAsyncQueryable<object>, Expression<Func<object, CancellationToken, ValueTask<object>>>, IComparer<object>, IOrderedAsyncQueryable<object>>(OrderByDescendingAwaitWithCancellation<object, object>).GetMethodInfo()!.GetGenericMethodDefinition())).MakeGenericMethod(TSource, TKey);

        public static IOrderedAsyncQueryable<TSource> OrderByDescendingAwaitWithCancellation<TSource, TKey>(this IAsyncQueryable<TSource> source, Expression<Func<TSource, CancellationToken, ValueTask<TKey>>> keySelector, IComparer<TKey>? comparer)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));
            if (keySelector == null)
                throw new ArgumentNullException(nameof(keySelector));

            return (IOrderedAsyncQueryable<TSource>)source.Provider.CreateQuery<TSource>(Expression.Call(OrderByDescendingAwaitWithCancellation__TSource_TKey__3__0(typeof(TSource), typeof(TKey)), source.Expression, keySelector, Expression.Constant(comparer, typeof(IComparer<TKey>))));
        }

        private static MethodInfo? s_Prepend__TSource__2__0;
        
        private static MethodInfo? Prepend__TSource__2__0(Type TSource) =>
            (s_Prepend__TSource__2__0 ??
            (s_Prepend__TSource__2__0 = new Func<IAsyncQueryable<object>, object, IAsyncQueryable<object>>(Prepend<object>).GetMethodInfo()!.GetGenericMethodDefinition())).MakeGenericMethod(TSource);

        public static IAsyncQueryable<TSource> Prepend<TSource>(this IAsyncQueryable<TSource> source, TSource element)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));

            return source.Provider.CreateQuery<TSource>(Expression.Call(Prepend__TSource__2__0(typeof(TSource)), source.Expression, Expression.Constant(element, typeof(TSource))));
        }

        private static MethodInfo? s_Reverse__TSource__1__0;
        
        private static MethodInfo? Reverse__TSource__1__0(Type TSource) =>
            (s_Reverse__TSource__1__0 ??
            (s_Reverse__TSource__1__0 = new Func<IAsyncQueryable<object>, IAsyncQueryable<object>>(Reverse<object>).GetMethodInfo()!.GetGenericMethodDefinition())).MakeGenericMethod(TSource);

        public static IAsyncQueryable<TSource> Reverse<TSource>(this IAsyncQueryable<TSource> source)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));

            return source.Provider.CreateQuery<TSource>(Expression.Call(Reverse__TSource__1__0(typeof(TSource)), source.Expression));
        }

        private static MethodInfo? s_Select__TSource_TResult__2__0;
        
        private static MethodInfo? Select__TSource_TResult__2__0(Type TSource, Type TResult) =>
            (s_Select__TSource_TResult__2__0 ??
            (s_Select__TSource_TResult__2__0 = new Func<IAsyncQueryable<object>, Expression<Func<object, int, object>>, IAsyncQueryable<object>>(Select<object, object>).GetMethodInfo()!.GetGenericMethodDefinition())).MakeGenericMethod(TSource, TResult);

        public static IAsyncQueryable<TResult> Select<TSource, TResult>(this IAsyncQueryable<TSource> source, Expression<Func<TSource, int, TResult>> selector)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));
            if (selector == null)
                throw new ArgumentNullException(nameof(selector));

            return source.Provider.CreateQuery<TResult>(Expression.Call(Select__TSource_TResult__2__0(typeof(TSource), typeof(TResult)), source.Expression, selector));
        }

        private static MethodInfo? s_Select__TSource_TResult__2__1;
        
        private static MethodInfo? Select__TSource_TResult__2__1(Type TSource, Type TResult) =>
            (s_Select__TSource_TResult__2__1 ??
            (s_Select__TSource_TResult__2__1 = new Func<IAsyncQueryable<object>, Expression<Func<object, object>>, IAsyncQueryable<object>>(Select<object, object>).GetMethodInfo()!.GetGenericMethodDefinition())).MakeGenericMethod(TSource, TResult);

        public static IAsyncQueryable<TResult> Select<TSource, TResult>(this IAsyncQueryable<TSource> source, Expression<Func<TSource, TResult>> selector)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));
            if (selector == null)
                throw new ArgumentNullException(nameof(selector));

            return source.Provider.CreateQuery<TResult>(Expression.Call(Select__TSource_TResult__2__1(typeof(TSource), typeof(TResult)), source.Expression, selector));
        }

        private static MethodInfo? s_SelectAwait__TSource_TResult__2__0;
        
        private static MethodInfo? SelectAwait__TSource_TResult__2__0(Type TSource, Type TResult) =>
            (s_SelectAwait__TSource_TResult__2__0 ??
            (s_SelectAwait__TSource_TResult__2__0 = new Func<IAsyncQueryable<object>, Expression<Func<object, int, ValueTask<object>>>, IAsyncQueryable<object>>(SelectAwait<object, object>).GetMethodInfo()!.GetGenericMethodDefinition())).MakeGenericMethod(TSource, TResult);

        public static IAsyncQueryable<TResult> SelectAwait<TSource, TResult>(this IAsyncQueryable<TSource> source, Expression<Func<TSource, int, ValueTask<TResult>>> selector)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));
            if (selector == null)
                throw new ArgumentNullException(nameof(selector));

            return source.Provider.CreateQuery<TResult>(Expression.Call(SelectAwait__TSource_TResult__2__0(typeof(TSource), typeof(TResult)), source.Expression, selector));
        }

        private static MethodInfo? s_SelectAwait__TSource_TResult__2__1;
        
        private static MethodInfo? SelectAwait__TSource_TResult__2__1(Type TSource, Type TResult) =>
            (s_SelectAwait__TSource_TResult__2__1 ??
            (s_SelectAwait__TSource_TResult__2__1 = new Func<IAsyncQueryable<object>, Expression<Func<object, ValueTask<object>>>, IAsyncQueryable<object>>(SelectAwait<object, object>).GetMethodInfo()!.GetGenericMethodDefinition())).MakeGenericMethod(TSource, TResult);

        public static IAsyncQueryable<TResult> SelectAwait<TSource, TResult>(this IAsyncQueryable<TSource> source, Expression<Func<TSource, ValueTask<TResult>>> selector)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));
            if (selector == null)
                throw new ArgumentNullException(nameof(selector));

            return source.Provider.CreateQuery<TResult>(Expression.Call(SelectAwait__TSource_TResult__2__1(typeof(TSource), typeof(TResult)), source.Expression, selector));
        }

        private static MethodInfo? s_SelectAwaitWithCancellation__TSource_TResult__2__0;
        
        private static MethodInfo? SelectAwaitWithCancellation__TSource_TResult__2__0(Type TSource, Type TResult) =>
            (s_SelectAwaitWithCancellation__TSource_TResult__2__0 ??
            (s_SelectAwaitWithCancellation__TSource_TResult__2__0 = new Func<IAsyncQueryable<object>, Expression<Func<object, CancellationToken, ValueTask<object>>>, IAsyncQueryable<object>>(SelectAwaitWithCancellation<object, object>).GetMethodInfo()!.GetGenericMethodDefinition())).MakeGenericMethod(TSource, TResult);

        public static IAsyncQueryable<TResult> SelectAwaitWithCancellation<TSource, TResult>(this IAsyncQueryable<TSource> source, Expression<Func<TSource, CancellationToken, ValueTask<TResult>>> selector)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));
            if (selector == null)
                throw new ArgumentNullException(nameof(selector));

            return source.Provider.CreateQuery<TResult>(Expression.Call(SelectAwaitWithCancellation__TSource_TResult__2__0(typeof(TSource), typeof(TResult)), source.Expression, selector));
        }

        private static MethodInfo? s_SelectAwaitWithCancellation__TSource_TResult__2__1;
        
        private static MethodInfo? SelectAwaitWithCancellation__TSource_TResult__2__1(Type TSource, Type TResult) =>
            (s_SelectAwaitWithCancellation__TSource_TResult__2__1 ??
            (s_SelectAwaitWithCancellation__TSource_TResult__2__1 = new Func<IAsyncQueryable<object>, Expression<Func<object, int, CancellationToken, ValueTask<object>>>, IAsyncQueryable<object>>(SelectAwaitWithCancellation<object, object>).GetMethodInfo()!.GetGenericMethodDefinition())).MakeGenericMethod(TSource, TResult);

        public static IAsyncQueryable<TResult> SelectAwaitWithCancellation<TSource, TResult>(this IAsyncQueryable<TSource> source, Expression<Func<TSource, int, CancellationToken, ValueTask<TResult>>> selector)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));
            if (selector == null)
                throw new ArgumentNullException(nameof(selector));

            return source.Provider.CreateQuery<TResult>(Expression.Call(SelectAwaitWithCancellation__TSource_TResult__2__1(typeof(TSource), typeof(TResult)), source.Expression, selector));
        }

        private static MethodInfo? s_SelectMany__TSource_TResult__2__0;
        
        private static MethodInfo? SelectMany__TSource_TResult__2__0(Type TSource, Type TResult) =>
            (s_SelectMany__TSource_TResult__2__0 ??
            (s_SelectMany__TSource_TResult__2__0 = new Func<IAsyncQueryable<object>, Expression<Func<object, IAsyncEnumerable<object>>>, IAsyncQueryable<object>>(SelectMany<object, object>).GetMethodInfo()!.GetGenericMethodDefinition())).MakeGenericMethod(TSource, TResult);

        public static IAsyncQueryable<TResult> SelectMany<TSource, TResult>(this IAsyncQueryable<TSource> source, Expression<Func<TSource, IAsyncEnumerable<TResult>>> selector)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));
            if (selector == null)
                throw new ArgumentNullException(nameof(selector));

            return source.Provider.CreateQuery<TResult>(Expression.Call(SelectMany__TSource_TResult__2__0(typeof(TSource), typeof(TResult)), source.Expression, selector));
        }

        private static MethodInfo? s_SelectMany__TSource_TResult__2__1;
        
        private static MethodInfo? SelectMany__TSource_TResult__2__1(Type TSource, Type TResult) =>
            (s_SelectMany__TSource_TResult__2__1 ??
            (s_SelectMany__TSource_TResult__2__1 = new Func<IAsyncQueryable<object>, Expression<Func<object, int, IAsyncEnumerable<object>>>, IAsyncQueryable<object>>(SelectMany<object, object>).GetMethodInfo()!.GetGenericMethodDefinition())).MakeGenericMethod(TSource, TResult);

        public static IAsyncQueryable<TResult> SelectMany<TSource, TResult>(this IAsyncQueryable<TSource> source, Expression<Func<TSource, int, IAsyncEnumerable<TResult>>> selector)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));
            if (selector == null)
                throw new ArgumentNullException(nameof(selector));

            return source.Provider.CreateQuery<TResult>(Expression.Call(SelectMany__TSource_TResult__2__1(typeof(TSource), typeof(TResult)), source.Expression, selector));
        }

        private static MethodInfo? s_SelectMany__TSource_TCollection_TResult__3__0;
        
        private static MethodInfo? SelectMany__TSource_TCollection_TResult__3__0(Type TSource, Type TCollection, Type TResult) =>
            (s_SelectMany__TSource_TCollection_TResult__3__0 ??
            (s_SelectMany__TSource_TCollection_TResult__3__0 = new Func<IAsyncQueryable<object>, Expression<Func<object, IAsyncEnumerable<object>>>, Expression<Func<object, object, object>>, IAsyncQueryable<object>>(SelectMany<object, object, object>).GetMethodInfo()!.GetGenericMethodDefinition())).MakeGenericMethod(TSource, TCollection, TResult);

        public static IAsyncQueryable<TResult> SelectMany<TSource, TCollection, TResult>(this IAsyncQueryable<TSource> source, Expression<Func<TSource, IAsyncEnumerable<TCollection>>> collectionSelector, Expression<Func<TSource, TCollection, TResult>> resultSelector)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));
            if (collectionSelector == null)
                throw new ArgumentNullException(nameof(collectionSelector));
            if (resultSelector == null)
                throw new ArgumentNullException(nameof(resultSelector));

            return source.Provider.CreateQuery<TResult>(Expression.Call(SelectMany__TSource_TCollection_TResult__3__0(typeof(TSource), typeof(TCollection), typeof(TResult)), source.Expression, collectionSelector, resultSelector));
        }

        private static MethodInfo? s_SelectMany__TSource_TCollection_TResult__3__1;
        
        private static MethodInfo? SelectMany__TSource_TCollection_TResult__3__1(Type TSource, Type TCollection, Type TResult) =>
            (s_SelectMany__TSource_TCollection_TResult__3__1 ??
            (s_SelectMany__TSource_TCollection_TResult__3__1 = new Func<IAsyncQueryable<object>, Expression<Func<object, int, IAsyncEnumerable<object>>>, Expression<Func<object, object, object>>, IAsyncQueryable<object>>(SelectMany<object, object, object>).GetMethodInfo()!.GetGenericMethodDefinition())).MakeGenericMethod(TSource, TCollection, TResult);

        public static IAsyncQueryable<TResult> SelectMany<TSource, TCollection, TResult>(this IAsyncQueryable<TSource> source, Expression<Func<TSource, int, IAsyncEnumerable<TCollection>>> collectionSelector, Expression<Func<TSource, TCollection, TResult>> resultSelector)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));
            if (collectionSelector == null)
                throw new ArgumentNullException(nameof(collectionSelector));
            if (resultSelector == null)
                throw new ArgumentNullException(nameof(resultSelector));

            return source.Provider.CreateQuery<TResult>(Expression.Call(SelectMany__TSource_TCollection_TResult__3__1(typeof(TSource), typeof(TCollection), typeof(TResult)), source.Expression, collectionSelector, resultSelector));
        }

        private static MethodInfo? s_SelectManyAwait__TSource_TResult__2__0;
        
        private static MethodInfo? SelectManyAwait__TSource_TResult__2__0(Type TSource, Type TResult) =>
            (s_SelectManyAwait__TSource_TResult__2__0 ??
            (s_SelectManyAwait__TSource_TResult__2__0 = new Func<IAsyncQueryable<object>, Expression<Func<object, int, ValueTask<IAsyncEnumerable<object>>>>, IAsyncQueryable<object>>(SelectManyAwait<object, object>).GetMethodInfo()!.GetGenericMethodDefinition())).MakeGenericMethod(TSource, TResult);

        public static IAsyncQueryable<TResult> SelectManyAwait<TSource, TResult>(this IAsyncQueryable<TSource> source, Expression<Func<TSource, int, ValueTask<IAsyncEnumerable<TResult>>>> selector)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));
            if (selector == null)
                throw new ArgumentNullException(nameof(selector));

            return source.Provider.CreateQuery<TResult>(Expression.Call(SelectManyAwait__TSource_TResult__2__0(typeof(TSource), typeof(TResult)), source.Expression, selector));
        }

        private static MethodInfo? s_SelectManyAwait__TSource_TResult__2__1;
        
        private static MethodInfo? SelectManyAwait__TSource_TResult__2__1(Type TSource, Type TResult) =>
            (s_SelectManyAwait__TSource_TResult__2__1 ??
            (s_SelectManyAwait__TSource_TResult__2__1 = new Func<IAsyncQueryable<object>, Expression<Func<object, ValueTask<IAsyncEnumerable<object>>>>, IAsyncQueryable<object>>(SelectManyAwait<object, object>).GetMethodInfo()!.GetGenericMethodDefinition())).MakeGenericMethod(TSource, TResult);

        public static IAsyncQueryable<TResult> SelectManyAwait<TSource, TResult>(this IAsyncQueryable<TSource> source, Expression<Func<TSource, ValueTask<IAsyncEnumerable<TResult>>>> selector)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));
            if (selector == null)
                throw new ArgumentNullException(nameof(selector));

            return source.Provider.CreateQuery<TResult>(Expression.Call(SelectManyAwait__TSource_TResult__2__1(typeof(TSource), typeof(TResult)), source.Expression, selector));
        }

        private static MethodInfo? s_SelectManyAwait__TSource_TCollection_TResult__3__0;
        
        private static MethodInfo? SelectManyAwait__TSource_TCollection_TResult__3__0(Type TSource, Type TCollection, Type TResult) =>
            (s_SelectManyAwait__TSource_TCollection_TResult__3__0 ??
            (s_SelectManyAwait__TSource_TCollection_TResult__3__0 = new Func<IAsyncQueryable<object>, Expression<Func<object, int, ValueTask<IAsyncEnumerable<object>>>>, Expression<Func<object, object, ValueTask<object>>>, IAsyncQueryable<object>>(SelectManyAwait<object, object, object>).GetMethodInfo()!.GetGenericMethodDefinition())).MakeGenericMethod(TSource, TCollection, TResult);

        public static IAsyncQueryable<TResult> SelectManyAwait<TSource, TCollection, TResult>(this IAsyncQueryable<TSource> source, Expression<Func<TSource, int, ValueTask<IAsyncEnumerable<TCollection>>>> collectionSelector, Expression<Func<TSource, TCollection, ValueTask<TResult>>> resultSelector)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));
            if (collectionSelector == null)
                throw new ArgumentNullException(nameof(collectionSelector));
            if (resultSelector == null)
                throw new ArgumentNullException(nameof(resultSelector));

            return source.Provider.CreateQuery<TResult>(Expression.Call(SelectManyAwait__TSource_TCollection_TResult__3__0(typeof(TSource), typeof(TCollection), typeof(TResult)), source.Expression, collectionSelector, resultSelector));
        }

        private static MethodInfo? s_SelectManyAwait__TSource_TCollection_TResult__3__1;
        
        private static MethodInfo? SelectManyAwait__TSource_TCollection_TResult__3__1(Type TSource, Type TCollection, Type TResult) =>
            (s_SelectManyAwait__TSource_TCollection_TResult__3__1 ??
            (s_SelectManyAwait__TSource_TCollection_TResult__3__1 = new Func<IAsyncQueryable<object>, Expression<Func<object, ValueTask<IAsyncEnumerable<object>>>>, Expression<Func<object, object, ValueTask<object>>>, IAsyncQueryable<object>>(SelectManyAwait<object, object, object>).GetMethodInfo()!.GetGenericMethodDefinition())).MakeGenericMethod(TSource, TCollection, TResult);

        public static IAsyncQueryable<TResult> SelectManyAwait<TSource, TCollection, TResult>(this IAsyncQueryable<TSource> source, Expression<Func<TSource, ValueTask<IAsyncEnumerable<TCollection>>>> collectionSelector, Expression<Func<TSource, TCollection, ValueTask<TResult>>> resultSelector)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));
            if (collectionSelector == null)
                throw new ArgumentNullException(nameof(collectionSelector));
            if (resultSelector == null)
                throw new ArgumentNullException(nameof(resultSelector));

            return source.Provider.CreateQuery<TResult>(Expression.Call(SelectManyAwait__TSource_TCollection_TResult__3__1(typeof(TSource), typeof(TCollection), typeof(TResult)), source.Expression, collectionSelector, resultSelector));
        }

        private static MethodInfo? s_SelectManyAwaitWithCancellation__TSource_TResult__2__0;
        
        private static MethodInfo? SelectManyAwaitWithCancellation__TSource_TResult__2__0(Type TSource, Type TResult) =>
            (s_SelectManyAwaitWithCancellation__TSource_TResult__2__0 ??
            (s_SelectManyAwaitWithCancellation__TSource_TResult__2__0 = new Func<IAsyncQueryable<object>, Expression<Func<object, CancellationToken, ValueTask<IAsyncEnumerable<object>>>>, IAsyncQueryable<object>>(SelectManyAwaitWithCancellation<object, object>).GetMethodInfo()!.GetGenericMethodDefinition())).MakeGenericMethod(TSource, TResult);

        public static IAsyncQueryable<TResult> SelectManyAwaitWithCancellation<TSource, TResult>(this IAsyncQueryable<TSource> source, Expression<Func<TSource, CancellationToken, ValueTask<IAsyncEnumerable<TResult>>>> selector)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));
            if (selector == null)
                throw new ArgumentNullException(nameof(selector));

            return source.Provider.CreateQuery<TResult>(Expression.Call(SelectManyAwaitWithCancellation__TSource_TResult__2__0(typeof(TSource), typeof(TResult)), source.Expression, selector));
        }

        private static MethodInfo? s_SelectManyAwaitWithCancellation__TSource_TResult__2__1;
        
        private static MethodInfo? SelectManyAwaitWithCancellation__TSource_TResult__2__1(Type TSource, Type TResult) =>
            (s_SelectManyAwaitWithCancellation__TSource_TResult__2__1 ??
            (s_SelectManyAwaitWithCancellation__TSource_TResult__2__1 = new Func<IAsyncQueryable<object>, Expression<Func<object, int, CancellationToken, ValueTask<IAsyncEnumerable<object>>>>, IAsyncQueryable<object>>(SelectManyAwaitWithCancellation<object, object>).GetMethodInfo()!.GetGenericMethodDefinition())).MakeGenericMethod(TSource, TResult);

        public static IAsyncQueryable<TResult> SelectManyAwaitWithCancellation<TSource, TResult>(this IAsyncQueryable<TSource> source, Expression<Func<TSource, int, CancellationToken, ValueTask<IAsyncEnumerable<TResult>>>> selector)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));
            if (selector == null)
                throw new ArgumentNullException(nameof(selector));

            return source.Provider.CreateQuery<TResult>(Expression.Call(SelectManyAwaitWithCancellation__TSource_TResult__2__1(typeof(TSource), typeof(TResult)), source.Expression, selector));
        }

        private static MethodInfo? s_SelectManyAwaitWithCancellation__TSource_TCollection_TResult__3__0;
        
        private static MethodInfo? SelectManyAwaitWithCancellation__TSource_TCollection_TResult__3__0(Type TSource, Type TCollection, Type TResult) =>
            (s_SelectManyAwaitWithCancellation__TSource_TCollection_TResult__3__0 ??
            (s_SelectManyAwaitWithCancellation__TSource_TCollection_TResult__3__0 = new Func<IAsyncQueryable<object>, Expression<Func<object, CancellationToken, ValueTask<IAsyncEnumerable<object>>>>, Expression<Func<object, object, CancellationToken, ValueTask<object>>>, IAsyncQueryable<object>>(SelectManyAwaitWithCancellation<object, object, object>).GetMethodInfo()!.GetGenericMethodDefinition())).MakeGenericMethod(TSource, TCollection, TResult);

        public static IAsyncQueryable<TResult> SelectManyAwaitWithCancellation<TSource, TCollection, TResult>(this IAsyncQueryable<TSource> source, Expression<Func<TSource, CancellationToken, ValueTask<IAsyncEnumerable<TCollection>>>> collectionSelector, Expression<Func<TSource, TCollection, CancellationToken, ValueTask<TResult>>> resultSelector)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));
            if (collectionSelector == null)
                throw new ArgumentNullException(nameof(collectionSelector));
            if (resultSelector == null)
                throw new ArgumentNullException(nameof(resultSelector));

            return source.Provider.CreateQuery<TResult>(Expression.Call(SelectManyAwaitWithCancellation__TSource_TCollection_TResult__3__0(typeof(TSource), typeof(TCollection), typeof(TResult)), source.Expression, collectionSelector, resultSelector));
        }

        private static MethodInfo? s_SelectManyAwaitWithCancellation__TSource_TCollection_TResult__3__1;
        
        private static MethodInfo? SelectManyAwaitWithCancellation__TSource_TCollection_TResult__3__1(Type TSource, Type TCollection, Type TResult) =>
            (s_SelectManyAwaitWithCancellation__TSource_TCollection_TResult__3__1 ??
            (s_SelectManyAwaitWithCancellation__TSource_TCollection_TResult__3__1 = new Func<IAsyncQueryable<object>, Expression<Func<object, int, CancellationToken, ValueTask<IAsyncEnumerable<object>>>>, Expression<Func<object, object, CancellationToken, ValueTask<object>>>, IAsyncQueryable<object>>(SelectManyAwaitWithCancellation<object, object, object>).GetMethodInfo()!.GetGenericMethodDefinition())).MakeGenericMethod(TSource, TCollection, TResult);

        public static IAsyncQueryable<TResult> SelectManyAwaitWithCancellation<TSource, TCollection, TResult>(this IAsyncQueryable<TSource> source, Expression<Func<TSource, int, CancellationToken, ValueTask<IAsyncEnumerable<TCollection>>>> collectionSelector, Expression<Func<TSource, TCollection, CancellationToken, ValueTask<TResult>>> resultSelector)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));
            if (collectionSelector == null)
                throw new ArgumentNullException(nameof(collectionSelector));
            if (resultSelector == null)
                throw new ArgumentNullException(nameof(resultSelector));

            return source.Provider.CreateQuery<TResult>(Expression.Call(SelectManyAwaitWithCancellation__TSource_TCollection_TResult__3__1(typeof(TSource), typeof(TCollection), typeof(TResult)), source.Expression, collectionSelector, resultSelector));
        }

        private static MethodInfo? s_SequenceEqualAsync__TSource__3__0;
        
        private static MethodInfo? SequenceEqualAsync__TSource__3__0(Type TSource) =>
            (s_SequenceEqualAsync__TSource__3__0 ??
            (s_SequenceEqualAsync__TSource__3__0 = new Func<IAsyncQueryable<object>, IAsyncEnumerable<object>, CancellationToken, ValueTask<bool>>(SequenceEqualAsync<object>).GetMethodInfo()!.GetGenericMethodDefinition())).MakeGenericMethod(TSource);

        public static ValueTask<bool> SequenceEqualAsync<TSource>(this IAsyncQueryable<TSource> first, IAsyncEnumerable<TSource> second, CancellationToken cancellationToken = default)
        {
            if (first == null)
                throw new ArgumentNullException(nameof(first));
            if (second == null)
                throw new ArgumentNullException(nameof(second));

            return first.Provider.ExecuteAsync<bool>(Expression.Call(SequenceEqualAsync__TSource__3__0(typeof(TSource)), first.Expression, GetSourceExpression(second), Expression.Constant(cancellationToken, typeof(CancellationToken))), cancellationToken);
        }

        private static MethodInfo? s_SequenceEqualAsync__TSource__4__0;
        
        private static MethodInfo? SequenceEqualAsync__TSource__4__0(Type TSource) =>
            (s_SequenceEqualAsync__TSource__4__0 ??
            (s_SequenceEqualAsync__TSource__4__0 = new Func<IAsyncQueryable<object>, IAsyncEnumerable<object>, IEqualityComparer<object>, CancellationToken, ValueTask<bool>>(SequenceEqualAsync<object>).GetMethodInfo()!.GetGenericMethodDefinition())).MakeGenericMethod(TSource);

        public static ValueTask<bool> SequenceEqualAsync<TSource>(this IAsyncQueryable<TSource> first, IAsyncEnumerable<TSource> second, IEqualityComparer<TSource>? comparer, CancellationToken cancellationToken = default)
        {
            if (first == null)
                throw new ArgumentNullException(nameof(first));
            if (second == null)
                throw new ArgumentNullException(nameof(second));

            return first.Provider.ExecuteAsync<bool>(Expression.Call(SequenceEqualAsync__TSource__4__0(typeof(TSource)), first.Expression, GetSourceExpression(second), Expression.Constant(comparer, typeof(IEqualityComparer<TSource>)), Expression.Constant(cancellationToken, typeof(CancellationToken))), cancellationToken);
        }

        private static MethodInfo? s_SingleAsync__TSource__2__0;
        
        private static MethodInfo? SingleAsync__TSource__2__0(Type TSource) =>
            (s_SingleAsync__TSource__2__0 ??
            (s_SingleAsync__TSource__2__0 = new Func<IAsyncQueryable<object>, CancellationToken, ValueTask<object>>(SingleAsync<object>).GetMethodInfo()!.GetGenericMethodDefinition())).MakeGenericMethod(TSource);

        public static ValueTask<TSource> SingleAsync<TSource>(this IAsyncQueryable<TSource> source, CancellationToken cancellationToken = default)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));

            return source.Provider.ExecuteAsync<TSource>(Expression.Call(SingleAsync__TSource__2__0(typeof(TSource)), source.Expression, Expression.Constant(cancellationToken, typeof(CancellationToken))), cancellationToken);
        }

        private static MethodInfo? s_SingleAsync__TSource__3__0;
        
        private static MethodInfo? SingleAsync__TSource__3__0(Type TSource) =>
            (s_SingleAsync__TSource__3__0 ??
            (s_SingleAsync__TSource__3__0 = new Func<IAsyncQueryable<object>, Expression<Func<object, bool>>, CancellationToken, ValueTask<object>>(SingleAsync<object>).GetMethodInfo()!.GetGenericMethodDefinition())).MakeGenericMethod(TSource);

        public static ValueTask<TSource> SingleAsync<TSource>(this IAsyncQueryable<TSource> source, Expression<Func<TSource, bool>> predicate, CancellationToken cancellationToken = default)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));
            if (predicate == null)
                throw new ArgumentNullException(nameof(predicate));

            return source.Provider.ExecuteAsync<TSource>(Expression.Call(SingleAsync__TSource__3__0(typeof(TSource)), source.Expression, predicate, Expression.Constant(cancellationToken, typeof(CancellationToken))), cancellationToken);
        }

        private static MethodInfo? s_SingleAwaitAsync__TSource__3__0;
        
        private static MethodInfo? SingleAwaitAsync__TSource__3__0(Type TSource) =>
            (s_SingleAwaitAsync__TSource__3__0 ??
            (s_SingleAwaitAsync__TSource__3__0 = new Func<IAsyncQueryable<object>, Expression<Func<object, ValueTask<bool>>>, CancellationToken, ValueTask<object>>(SingleAwaitAsync<object>).GetMethodInfo()!.GetGenericMethodDefinition())).MakeGenericMethod(TSource);

        public static ValueTask<TSource> SingleAwaitAsync<TSource>(this IAsyncQueryable<TSource> source, Expression<Func<TSource, ValueTask<bool>>> predicate, CancellationToken cancellationToken = default)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));
            if (predicate == null)
                throw new ArgumentNullException(nameof(predicate));

            return source.Provider.ExecuteAsync<TSource>(Expression.Call(SingleAwaitAsync__TSource__3__0(typeof(TSource)), source.Expression, predicate, Expression.Constant(cancellationToken, typeof(CancellationToken))), cancellationToken);
        }

        private static MethodInfo? s_SingleAwaitWithCancellationAsync__TSource__3__0;
        
        private static MethodInfo? SingleAwaitWithCancellationAsync__TSource__3__0(Type TSource) =>
            (s_SingleAwaitWithCancellationAsync__TSource__3__0 ??
            (s_SingleAwaitWithCancellationAsync__TSource__3__0 = new Func<IAsyncQueryable<object>, Expression<Func<object, CancellationToken, ValueTask<bool>>>, CancellationToken, ValueTask<object>>(SingleAwaitWithCancellationAsync<object>).GetMethodInfo()!.GetGenericMethodDefinition())).MakeGenericMethod(TSource);

        public static ValueTask<TSource> SingleAwaitWithCancellationAsync<TSource>(this IAsyncQueryable<TSource> source, Expression<Func<TSource, CancellationToken, ValueTask<bool>>> predicate, CancellationToken cancellationToken = default)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));
            if (predicate == null)
                throw new ArgumentNullException(nameof(predicate));

            return source.Provider.ExecuteAsync<TSource>(Expression.Call(SingleAwaitWithCancellationAsync__TSource__3__0(typeof(TSource)), source.Expression, predicate, Expression.Constant(cancellationToken, typeof(CancellationToken))), cancellationToken);
        }

        private static MethodInfo? s_SingleOrDefaultAsync__TSource__2__0;
        
        private static MethodInfo? SingleOrDefaultAsync__TSource__2__0(Type TSource) =>
            (s_SingleOrDefaultAsync__TSource__2__0 ??
            (s_SingleOrDefaultAsync__TSource__2__0 = new Func<IAsyncQueryable<object>, CancellationToken, ValueTask<object>>(SingleOrDefaultAsync<object>).GetMethodInfo()!.GetGenericMethodDefinition())).MakeGenericMethod(TSource);

        public static ValueTask<TSource> SingleOrDefaultAsync<TSource>(this IAsyncQueryable<TSource> source, CancellationToken cancellationToken = default)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));

            return source.Provider.ExecuteAsync<TSource>(Expression.Call(SingleOrDefaultAsync__TSource__2__0(typeof(TSource)), source.Expression, Expression.Constant(cancellationToken, typeof(CancellationToken))), cancellationToken);
        }

        private static MethodInfo? s_SingleOrDefaultAsync__TSource__3__0;
        
        private static MethodInfo? SingleOrDefaultAsync__TSource__3__0(Type TSource) =>
            (s_SingleOrDefaultAsync__TSource__3__0 ??
            (s_SingleOrDefaultAsync__TSource__3__0 = new Func<IAsyncQueryable<object>, Expression<Func<object, bool>>, CancellationToken, ValueTask<object>>(SingleOrDefaultAsync<object>).GetMethodInfo()!.GetGenericMethodDefinition())).MakeGenericMethod(TSource);

        public static ValueTask<TSource> SingleOrDefaultAsync<TSource>(this IAsyncQueryable<TSource> source, Expression<Func<TSource, bool>> predicate, CancellationToken cancellationToken = default)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));
            if (predicate == null)
                throw new ArgumentNullException(nameof(predicate));

            return source.Provider.ExecuteAsync<TSource>(Expression.Call(SingleOrDefaultAsync__TSource__3__0(typeof(TSource)), source.Expression, predicate, Expression.Constant(cancellationToken, typeof(CancellationToken))), cancellationToken);
        }

        private static MethodInfo? s_SingleOrDefaultAwaitAsync__TSource__3__0;
        
        private static MethodInfo? SingleOrDefaultAwaitAsync__TSource__3__0(Type TSource) =>
            (s_SingleOrDefaultAwaitAsync__TSource__3__0 ??
            (s_SingleOrDefaultAwaitAsync__TSource__3__0 = new Func<IAsyncQueryable<object>, Expression<Func<object, ValueTask<bool>>>, CancellationToken, ValueTask<object>>(SingleOrDefaultAwaitAsync<object>).GetMethodInfo()!.GetGenericMethodDefinition())).MakeGenericMethod(TSource);

        public static ValueTask<TSource> SingleOrDefaultAwaitAsync<TSource>(this IAsyncQueryable<TSource> source, Expression<Func<TSource, ValueTask<bool>>> predicate, CancellationToken cancellationToken = default)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));
            if (predicate == null)
                throw new ArgumentNullException(nameof(predicate));

            return source.Provider.ExecuteAsync<TSource>(Expression.Call(SingleOrDefaultAwaitAsync__TSource__3__0(typeof(TSource)), source.Expression, predicate, Expression.Constant(cancellationToken, typeof(CancellationToken))), cancellationToken);
        }

        private static MethodInfo? s_SingleOrDefaultAwaitWithCancellationAsync__TSource__3__0;
        
        private static MethodInfo? SingleOrDefaultAwaitWithCancellationAsync__TSource__3__0(Type TSource) =>
            (s_SingleOrDefaultAwaitWithCancellationAsync__TSource__3__0 ??
            (s_SingleOrDefaultAwaitWithCancellationAsync__TSource__3__0 = new Func<IAsyncQueryable<object>, Expression<Func<object, CancellationToken, ValueTask<bool>>>, CancellationToken, ValueTask<object>>(SingleOrDefaultAwaitWithCancellationAsync<object>).GetMethodInfo()!.GetGenericMethodDefinition())).MakeGenericMethod(TSource);

        public static ValueTask<TSource> SingleOrDefaultAwaitWithCancellationAsync<TSource>(this IAsyncQueryable<TSource> source, Expression<Func<TSource, CancellationToken, ValueTask<bool>>> predicate, CancellationToken cancellationToken = default)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));
            if (predicate == null)
                throw new ArgumentNullException(nameof(predicate));

            return source.Provider.ExecuteAsync<TSource>(Expression.Call(SingleOrDefaultAwaitWithCancellationAsync__TSource__3__0(typeof(TSource)), source.Expression, predicate, Expression.Constant(cancellationToken, typeof(CancellationToken))), cancellationToken);
        }

        private static MethodInfo? s_Skip__TSource__2__0;
        
        private static MethodInfo? Skip__TSource__2__0(Type TSource) =>
            (s_Skip__TSource__2__0 ??
            (s_Skip__TSource__2__0 = new Func<IAsyncQueryable<object>, int, IAsyncQueryable<object>>(Skip<object>).GetMethodInfo()!.GetGenericMethodDefinition())).MakeGenericMethod(TSource);

        public static IAsyncQueryable<TSource> Skip<TSource>(this IAsyncQueryable<TSource> source, int count)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));

            return source.Provider.CreateQuery<TSource>(Expression.Call(Skip__TSource__2__0(typeof(TSource)), source.Expression, Expression.Constant(count, typeof(int))));
        }

        private static MethodInfo? s_SkipLast__TSource__2__0;
        
        private static MethodInfo? SkipLast__TSource__2__0(Type TSource) =>
            (s_SkipLast__TSource__2__0 ??
            (s_SkipLast__TSource__2__0 = new Func<IAsyncQueryable<object>, int, IAsyncQueryable<object>>(SkipLast<object>).GetMethodInfo()!.GetGenericMethodDefinition())).MakeGenericMethod(TSource);

        public static IAsyncQueryable<TSource> SkipLast<TSource>(this IAsyncQueryable<TSource> source, int count)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));

            return source.Provider.CreateQuery<TSource>(Expression.Call(SkipLast__TSource__2__0(typeof(TSource)), source.Expression, Expression.Constant(count, typeof(int))));
        }

        private static MethodInfo? s_SkipWhile__TSource__2__0;
        
        private static MethodInfo? SkipWhile__TSource__2__0(Type TSource) =>
            (s_SkipWhile__TSource__2__0 ??
            (s_SkipWhile__TSource__2__0 = new Func<IAsyncQueryable<object>, Expression<Func<object, bool>>, IAsyncQueryable<object>>(SkipWhile<object>).GetMethodInfo()!.GetGenericMethodDefinition())).MakeGenericMethod(TSource);

        public static IAsyncQueryable<TSource> SkipWhile<TSource>(this IAsyncQueryable<TSource> source, Expression<Func<TSource, bool>> predicate)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));
            if (predicate == null)
                throw new ArgumentNullException(nameof(predicate));

            return source.Provider.CreateQuery<TSource>(Expression.Call(SkipWhile__TSource__2__0(typeof(TSource)), source.Expression, predicate));
        }

        private static MethodInfo? s_SkipWhile__TSource__2__1;
        
        private static MethodInfo? SkipWhile__TSource__2__1(Type TSource) =>
            (s_SkipWhile__TSource__2__1 ??
            (s_SkipWhile__TSource__2__1 = new Func<IAsyncQueryable<object>, Expression<Func<object, int, bool>>, IAsyncQueryable<object>>(SkipWhile<object>).GetMethodInfo()!.GetGenericMethodDefinition())).MakeGenericMethod(TSource);

        public static IAsyncQueryable<TSource> SkipWhile<TSource>(this IAsyncQueryable<TSource> source, Expression<Func<TSource, int, bool>> predicate)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));
            if (predicate == null)
                throw new ArgumentNullException(nameof(predicate));

            return source.Provider.CreateQuery<TSource>(Expression.Call(SkipWhile__TSource__2__1(typeof(TSource)), source.Expression, predicate));
        }

        private static MethodInfo? s_SkipWhileAwait__TSource__2__0;
        
        private static MethodInfo? SkipWhileAwait__TSource__2__0(Type TSource) =>
            (s_SkipWhileAwait__TSource__2__0 ??
            (s_SkipWhileAwait__TSource__2__0 = new Func<IAsyncQueryable<object>, Expression<Func<object, int, ValueTask<bool>>>, IAsyncQueryable<object>>(SkipWhileAwait<object>).GetMethodInfo()!.GetGenericMethodDefinition())).MakeGenericMethod(TSource);

        public static IAsyncQueryable<TSource> SkipWhileAwait<TSource>(this IAsyncQueryable<TSource> source, Expression<Func<TSource, int, ValueTask<bool>>> predicate)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));
            if (predicate == null)
                throw new ArgumentNullException(nameof(predicate));

            return source.Provider.CreateQuery<TSource>(Expression.Call(SkipWhileAwait__TSource__2__0(typeof(TSource)), source.Expression, predicate));
        }

        private static MethodInfo? s_SkipWhileAwait__TSource__2__1;
        
        private static MethodInfo? SkipWhileAwait__TSource__2__1(Type TSource) =>
            (s_SkipWhileAwait__TSource__2__1 ??
            (s_SkipWhileAwait__TSource__2__1 = new Func<IAsyncQueryable<object>, Expression<Func<object, ValueTask<bool>>>, IAsyncQueryable<object>>(SkipWhileAwait<object>).GetMethodInfo()!.GetGenericMethodDefinition())).MakeGenericMethod(TSource);

        public static IAsyncQueryable<TSource> SkipWhileAwait<TSource>(this IAsyncQueryable<TSource> source, Expression<Func<TSource, ValueTask<bool>>> predicate)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));
            if (predicate == null)
                throw new ArgumentNullException(nameof(predicate));

            return source.Provider.CreateQuery<TSource>(Expression.Call(SkipWhileAwait__TSource__2__1(typeof(TSource)), source.Expression, predicate));
        }

        private static MethodInfo? s_SkipWhileAwaitWithCancellation__TSource__2__0;
        
        private static MethodInfo? SkipWhileAwaitWithCancellation__TSource__2__0(Type TSource) =>
            (s_SkipWhileAwaitWithCancellation__TSource__2__0 ??
            (s_SkipWhileAwaitWithCancellation__TSource__2__0 = new Func<IAsyncQueryable<object>, Expression<Func<object, CancellationToken, ValueTask<bool>>>, IAsyncQueryable<object>>(SkipWhileAwaitWithCancellation<object>).GetMethodInfo()!.GetGenericMethodDefinition())).MakeGenericMethod(TSource);

        public static IAsyncQueryable<TSource> SkipWhileAwaitWithCancellation<TSource>(this IAsyncQueryable<TSource> source, Expression<Func<TSource, CancellationToken, ValueTask<bool>>> predicate)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));
            if (predicate == null)
                throw new ArgumentNullException(nameof(predicate));

            return source.Provider.CreateQuery<TSource>(Expression.Call(SkipWhileAwaitWithCancellation__TSource__2__0(typeof(TSource)), source.Expression, predicate));
        }

        private static MethodInfo? s_SkipWhileAwaitWithCancellation__TSource__2__1;
        
        private static MethodInfo? SkipWhileAwaitWithCancellation__TSource__2__1(Type TSource) =>
            (s_SkipWhileAwaitWithCancellation__TSource__2__1 ??
            (s_SkipWhileAwaitWithCancellation__TSource__2__1 = new Func<IAsyncQueryable<object>, Expression<Func<object, int, CancellationToken, ValueTask<bool>>>, IAsyncQueryable<object>>(SkipWhileAwaitWithCancellation<object>).GetMethodInfo()!.GetGenericMethodDefinition())).MakeGenericMethod(TSource);

        public static IAsyncQueryable<TSource> SkipWhileAwaitWithCancellation<TSource>(this IAsyncQueryable<TSource> source, Expression<Func<TSource, int, CancellationToken, ValueTask<bool>>> predicate)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));
            if (predicate == null)
                throw new ArgumentNullException(nameof(predicate));

            return source.Provider.CreateQuery<TSource>(Expression.Call(SkipWhileAwaitWithCancellation__TSource__2__1(typeof(TSource)), source.Expression, predicate));
        }

        private static MethodInfo? s_SumAsync__2__0;
        
        private static MethodInfo? SumAsync__2__0 =>
            (s_SumAsync__2__0 ??
            (s_SumAsync__2__0 = new Func<IAsyncQueryable<decimal?>, CancellationToken, ValueTask<decimal?>>(SumAsync).GetMethodInfo()));

        public static ValueTask<decimal?> SumAsync(this IAsyncQueryable<decimal?> source, CancellationToken cancellationToken = default)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));

            return source.Provider.ExecuteAsync<decimal?>(Expression.Call(SumAsync__2__0, source.Expression, Expression.Constant(cancellationToken, typeof(CancellationToken))), cancellationToken);
        }

        private static MethodInfo? s_SumAsync__2__1;
        
        private static MethodInfo? SumAsync__2__1 =>
            (s_SumAsync__2__1 ??
            (s_SumAsync__2__1 = new Func<IAsyncQueryable<decimal>, CancellationToken, ValueTask<decimal>>(SumAsync).GetMethodInfo()));

        public static ValueTask<decimal> SumAsync(this IAsyncQueryable<decimal> source, CancellationToken cancellationToken = default)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));

            return source.Provider.ExecuteAsync<decimal>(Expression.Call(SumAsync__2__1, source.Expression, Expression.Constant(cancellationToken, typeof(CancellationToken))), cancellationToken);
        }

        private static MethodInfo? s_SumAsync__2__2;
        
        private static MethodInfo? SumAsync__2__2 =>
            (s_SumAsync__2__2 ??
            (s_SumAsync__2__2 = new Func<IAsyncQueryable<double?>, CancellationToken, ValueTask<double?>>(SumAsync).GetMethodInfo()));

        public static ValueTask<double?> SumAsync(this IAsyncQueryable<double?> source, CancellationToken cancellationToken = default)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));

            return source.Provider.ExecuteAsync<double?>(Expression.Call(SumAsync__2__2, source.Expression, Expression.Constant(cancellationToken, typeof(CancellationToken))), cancellationToken);
        }

        private static MethodInfo? s_SumAsync__2__3;
        
        private static MethodInfo? SumAsync__2__3 =>
            (s_SumAsync__2__3 ??
            (s_SumAsync__2__3 = new Func<IAsyncQueryable<double>, CancellationToken, ValueTask<double>>(SumAsync).GetMethodInfo()));

        public static ValueTask<double> SumAsync(this IAsyncQueryable<double> source, CancellationToken cancellationToken = default)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));

            return source.Provider.ExecuteAsync<double>(Expression.Call(SumAsync__2__3, source.Expression, Expression.Constant(cancellationToken, typeof(CancellationToken))), cancellationToken);
        }

        private static MethodInfo? s_SumAsync__2__4;
        
        private static MethodInfo? SumAsync__2__4 =>
            (s_SumAsync__2__4 ??
            (s_SumAsync__2__4 = new Func<IAsyncQueryable<float?>, CancellationToken, ValueTask<float?>>(SumAsync).GetMethodInfo()));

        public static ValueTask<float?> SumAsync(this IAsyncQueryable<float?> source, CancellationToken cancellationToken = default)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));

            return source.Provider.ExecuteAsync<float?>(Expression.Call(SumAsync__2__4, source.Expression, Expression.Constant(cancellationToken, typeof(CancellationToken))), cancellationToken);
        }

        private static MethodInfo? s_SumAsync__2__5;
        
        private static MethodInfo? SumAsync__2__5 =>
            (s_SumAsync__2__5 ??
            (s_SumAsync__2__5 = new Func<IAsyncQueryable<float>, CancellationToken, ValueTask<float>>(SumAsync).GetMethodInfo()));

        public static ValueTask<float> SumAsync(this IAsyncQueryable<float> source, CancellationToken cancellationToken = default)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));

            return source.Provider.ExecuteAsync<float>(Expression.Call(SumAsync__2__5, source.Expression, Expression.Constant(cancellationToken, typeof(CancellationToken))), cancellationToken);
        }

        private static MethodInfo? s_SumAsync__2__6;
        
        private static MethodInfo? SumAsync__2__6 =>
            (s_SumAsync__2__6 ??
            (s_SumAsync__2__6 = new Func<IAsyncQueryable<int?>, CancellationToken, ValueTask<int?>>(SumAsync).GetMethodInfo()));

        public static ValueTask<int?> SumAsync(this IAsyncQueryable<int?> source, CancellationToken cancellationToken = default)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));

            return source.Provider.ExecuteAsync<int?>(Expression.Call(SumAsync__2__6, source.Expression, Expression.Constant(cancellationToken, typeof(CancellationToken))), cancellationToken);
        }

        private static MethodInfo? s_SumAsync__2__7;
        
        private static MethodInfo? SumAsync__2__7 =>
            (s_SumAsync__2__7 ??
            (s_SumAsync__2__7 = new Func<IAsyncQueryable<int>, CancellationToken, ValueTask<int>>(SumAsync).GetMethodInfo()));

        public static ValueTask<int> SumAsync(this IAsyncQueryable<int> source, CancellationToken cancellationToken = default)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));

            return source.Provider.ExecuteAsync<int>(Expression.Call(SumAsync__2__7, source.Expression, Expression.Constant(cancellationToken, typeof(CancellationToken))), cancellationToken);
        }

        private static MethodInfo? s_SumAsync__2__8;
        
        private static MethodInfo? SumAsync__2__8 =>
            (s_SumAsync__2__8 ??
            (s_SumAsync__2__8 = new Func<IAsyncQueryable<long?>, CancellationToken, ValueTask<long?>>(SumAsync).GetMethodInfo()));

        public static ValueTask<long?> SumAsync(this IAsyncQueryable<long?> source, CancellationToken cancellationToken = default)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));

            return source.Provider.ExecuteAsync<long?>(Expression.Call(SumAsync__2__8, source.Expression, Expression.Constant(cancellationToken, typeof(CancellationToken))), cancellationToken);
        }

        private static MethodInfo? s_SumAsync__2__9;
        
        private static MethodInfo? SumAsync__2__9 =>
            (s_SumAsync__2__9 ??
            (s_SumAsync__2__9 = new Func<IAsyncQueryable<long>, CancellationToken, ValueTask<long>>(SumAsync).GetMethodInfo()));

        public static ValueTask<long> SumAsync(this IAsyncQueryable<long> source, CancellationToken cancellationToken = default)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));

            return source.Provider.ExecuteAsync<long>(Expression.Call(SumAsync__2__9, source.Expression, Expression.Constant(cancellationToken, typeof(CancellationToken))), cancellationToken);
        }

        private static MethodInfo? s_SumAsync__TSource__3__0;
        
        private static MethodInfo? SumAsync__TSource__3__0(Type TSource) =>
            (s_SumAsync__TSource__3__0 ??
            (s_SumAsync__TSource__3__0 = new Func<IAsyncQueryable<object>, Expression<Func<object, decimal?>>, CancellationToken, ValueTask<decimal?>>(SumAsync<object>).GetMethodInfo()!.GetGenericMethodDefinition())).MakeGenericMethod(TSource);

        public static ValueTask<decimal?> SumAsync<TSource>(this IAsyncQueryable<TSource> source, Expression<Func<TSource, decimal?>> selector, CancellationToken cancellationToken = default)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));
            if (selector == null)
                throw new ArgumentNullException(nameof(selector));

            return source.Provider.ExecuteAsync<decimal?>(Expression.Call(SumAsync__TSource__3__0(typeof(TSource)), source.Expression, selector, Expression.Constant(cancellationToken, typeof(CancellationToken))), cancellationToken);
        }

        private static MethodInfo? s_SumAsync__TSource__3__1;
        
        private static MethodInfo? SumAsync__TSource__3__1(Type TSource) =>
            (s_SumAsync__TSource__3__1 ??
            (s_SumAsync__TSource__3__1 = new Func<IAsyncQueryable<object>, Expression<Func<object, decimal>>, CancellationToken, ValueTask<decimal>>(SumAsync<object>).GetMethodInfo()!.GetGenericMethodDefinition())).MakeGenericMethod(TSource);

        public static ValueTask<decimal> SumAsync<TSource>(this IAsyncQueryable<TSource> source, Expression<Func<TSource, decimal>> selector, CancellationToken cancellationToken = default)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));
            if (selector == null)
                throw new ArgumentNullException(nameof(selector));

            return source.Provider.ExecuteAsync<decimal>(Expression.Call(SumAsync__TSource__3__1(typeof(TSource)), source.Expression, selector, Expression.Constant(cancellationToken, typeof(CancellationToken))), cancellationToken);
        }

        private static MethodInfo? s_SumAsync__TSource__3__2;
        
        private static MethodInfo? SumAsync__TSource__3__2(Type TSource) =>
            (s_SumAsync__TSource__3__2 ??
            (s_SumAsync__TSource__3__2 = new Func<IAsyncQueryable<object>, Expression<Func<object, double?>>, CancellationToken, ValueTask<double?>>(SumAsync<object>).GetMethodInfo()!.GetGenericMethodDefinition())).MakeGenericMethod(TSource);

        public static ValueTask<double?> SumAsync<TSource>(this IAsyncQueryable<TSource> source, Expression<Func<TSource, double?>> selector, CancellationToken cancellationToken = default)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));
            if (selector == null)
                throw new ArgumentNullException(nameof(selector));

            return source.Provider.ExecuteAsync<double?>(Expression.Call(SumAsync__TSource__3__2(typeof(TSource)), source.Expression, selector, Expression.Constant(cancellationToken, typeof(CancellationToken))), cancellationToken);
        }

        private static MethodInfo? s_SumAsync__TSource__3__3;
        
        private static MethodInfo? SumAsync__TSource__3__3(Type TSource) =>
            (s_SumAsync__TSource__3__3 ??
            (s_SumAsync__TSource__3__3 = new Func<IAsyncQueryable<object>, Expression<Func<object, double>>, CancellationToken, ValueTask<double>>(SumAsync<object>).GetMethodInfo()!.GetGenericMethodDefinition())).MakeGenericMethod(TSource);

        public static ValueTask<double> SumAsync<TSource>(this IAsyncQueryable<TSource> source, Expression<Func<TSource, double>> selector, CancellationToken cancellationToken = default)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));
            if (selector == null)
                throw new ArgumentNullException(nameof(selector));

            return source.Provider.ExecuteAsync<double>(Expression.Call(SumAsync__TSource__3__3(typeof(TSource)), source.Expression, selector, Expression.Constant(cancellationToken, typeof(CancellationToken))), cancellationToken);
        }

        private static MethodInfo? s_SumAsync__TSource__3__4;
        
        private static MethodInfo? SumAsync__TSource__3__4(Type TSource) =>
            (s_SumAsync__TSource__3__4 ??
            (s_SumAsync__TSource__3__4 = new Func<IAsyncQueryable<object>, Expression<Func<object, float?>>, CancellationToken, ValueTask<float?>>(SumAsync<object>).GetMethodInfo()!.GetGenericMethodDefinition())).MakeGenericMethod(TSource);

        public static ValueTask<float?> SumAsync<TSource>(this IAsyncQueryable<TSource> source, Expression<Func<TSource, float?>> selector, CancellationToken cancellationToken = default)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));
            if (selector == null)
                throw new ArgumentNullException(nameof(selector));

            return source.Provider.ExecuteAsync<float?>(Expression.Call(SumAsync__TSource__3__4(typeof(TSource)), source.Expression, selector, Expression.Constant(cancellationToken, typeof(CancellationToken))), cancellationToken);
        }

        private static MethodInfo? s_SumAsync__TSource__3__5;
        
        private static MethodInfo? SumAsync__TSource__3__5(Type TSource) =>
            (s_SumAsync__TSource__3__5 ??
            (s_SumAsync__TSource__3__5 = new Func<IAsyncQueryable<object>, Expression<Func<object, float>>, CancellationToken, ValueTask<float>>(SumAsync<object>).GetMethodInfo()!.GetGenericMethodDefinition())).MakeGenericMethod(TSource);

        public static ValueTask<float> SumAsync<TSource>(this IAsyncQueryable<TSource> source, Expression<Func<TSource, float>> selector, CancellationToken cancellationToken = default)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));
            if (selector == null)
                throw new ArgumentNullException(nameof(selector));

            return source.Provider.ExecuteAsync<float>(Expression.Call(SumAsync__TSource__3__5(typeof(TSource)), source.Expression, selector, Expression.Constant(cancellationToken, typeof(CancellationToken))), cancellationToken);
        }

        private static MethodInfo? s_SumAsync__TSource__3__6;
        
        private static MethodInfo? SumAsync__TSource__3__6(Type TSource) =>
            (s_SumAsync__TSource__3__6 ??
            (s_SumAsync__TSource__3__6 = new Func<IAsyncQueryable<object>, Expression<Func<object, int?>>, CancellationToken, ValueTask<int?>>(SumAsync<object>).GetMethodInfo()!.GetGenericMethodDefinition())).MakeGenericMethod(TSource);

        public static ValueTask<int?> SumAsync<TSource>(this IAsyncQueryable<TSource> source, Expression<Func<TSource, int?>> selector, CancellationToken cancellationToken = default)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));
            if (selector == null)
                throw new ArgumentNullException(nameof(selector));

            return source.Provider.ExecuteAsync<int?>(Expression.Call(SumAsync__TSource__3__6(typeof(TSource)), source.Expression, selector, Expression.Constant(cancellationToken, typeof(CancellationToken))), cancellationToken);
        }

        private static MethodInfo? s_SumAsync__TSource__3__7;
        
        private static MethodInfo? SumAsync__TSource__3__7(Type TSource) =>
            (s_SumAsync__TSource__3__7 ??
            (s_SumAsync__TSource__3__7 = new Func<IAsyncQueryable<object>, Expression<Func<object, int>>, CancellationToken, ValueTask<int>>(SumAsync<object>).GetMethodInfo()!.GetGenericMethodDefinition())).MakeGenericMethod(TSource);

        public static ValueTask<int> SumAsync<TSource>(this IAsyncQueryable<TSource> source, Expression<Func<TSource, int>> selector, CancellationToken cancellationToken = default)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));
            if (selector == null)
                throw new ArgumentNullException(nameof(selector));

            return source.Provider.ExecuteAsync<int>(Expression.Call(SumAsync__TSource__3__7(typeof(TSource)), source.Expression, selector, Expression.Constant(cancellationToken, typeof(CancellationToken))), cancellationToken);
        }

        private static MethodInfo? s_SumAsync__TSource__3__8;
        
        private static MethodInfo? SumAsync__TSource__3__8(Type TSource) =>
            (s_SumAsync__TSource__3__8 ??
            (s_SumAsync__TSource__3__8 = new Func<IAsyncQueryable<object>, Expression<Func<object, long?>>, CancellationToken, ValueTask<long?>>(SumAsync<object>).GetMethodInfo()!.GetGenericMethodDefinition())).MakeGenericMethod(TSource);

        public static ValueTask<long?> SumAsync<TSource>(this IAsyncQueryable<TSource> source, Expression<Func<TSource, long?>> selector, CancellationToken cancellationToken = default)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));
            if (selector == null)
                throw new ArgumentNullException(nameof(selector));

            return source.Provider.ExecuteAsync<long?>(Expression.Call(SumAsync__TSource__3__8(typeof(TSource)), source.Expression, selector, Expression.Constant(cancellationToken, typeof(CancellationToken))), cancellationToken);
        }

        private static MethodInfo? s_SumAsync__TSource__3__9;
        
        private static MethodInfo? SumAsync__TSource__3__9(Type TSource) =>
            (s_SumAsync__TSource__3__9 ??
            (s_SumAsync__TSource__3__9 = new Func<IAsyncQueryable<object>, Expression<Func<object, long>>, CancellationToken, ValueTask<long>>(SumAsync<object>).GetMethodInfo()!.GetGenericMethodDefinition())).MakeGenericMethod(TSource);

        public static ValueTask<long> SumAsync<TSource>(this IAsyncQueryable<TSource> source, Expression<Func<TSource, long>> selector, CancellationToken cancellationToken = default)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));
            if (selector == null)
                throw new ArgumentNullException(nameof(selector));

            return source.Provider.ExecuteAsync<long>(Expression.Call(SumAsync__TSource__3__9(typeof(TSource)), source.Expression, selector, Expression.Constant(cancellationToken, typeof(CancellationToken))), cancellationToken);
        }

        private static MethodInfo? s_SumAwaitAsync__TSource__3__0;
        
        private static MethodInfo? SumAwaitAsync__TSource__3__0(Type TSource) =>
            (s_SumAwaitAsync__TSource__3__0 ??
            (s_SumAwaitAsync__TSource__3__0 = new Func<IAsyncQueryable<object>, Expression<Func<object, ValueTask<decimal?>>>, CancellationToken, ValueTask<decimal?>>(SumAwaitAsync<object>).GetMethodInfo()!.GetGenericMethodDefinition())).MakeGenericMethod(TSource);

        public static ValueTask<decimal?> SumAwaitAsync<TSource>(this IAsyncQueryable<TSource> source, Expression<Func<TSource, ValueTask<decimal?>>> selector, CancellationToken cancellationToken = default)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));
            if (selector == null)
                throw new ArgumentNullException(nameof(selector));

            return source.Provider.ExecuteAsync<decimal?>(Expression.Call(SumAwaitAsync__TSource__3__0(typeof(TSource)), source.Expression, selector, Expression.Constant(cancellationToken, typeof(CancellationToken))), cancellationToken);
        }

        private static MethodInfo? s_SumAwaitAsync__TSource__3__1;
        
        private static MethodInfo? SumAwaitAsync__TSource__3__1(Type TSource) =>
            (s_SumAwaitAsync__TSource__3__1 ??
            (s_SumAwaitAsync__TSource__3__1 = new Func<IAsyncQueryable<object>, Expression<Func<object, ValueTask<decimal>>>, CancellationToken, ValueTask<decimal>>(SumAwaitAsync<object>).GetMethodInfo()!.GetGenericMethodDefinition())).MakeGenericMethod(TSource);

        public static ValueTask<decimal> SumAwaitAsync<TSource>(this IAsyncQueryable<TSource> source, Expression<Func<TSource, ValueTask<decimal>>> selector, CancellationToken cancellationToken = default)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));
            if (selector == null)
                throw new ArgumentNullException(nameof(selector));

            return source.Provider.ExecuteAsync<decimal>(Expression.Call(SumAwaitAsync__TSource__3__1(typeof(TSource)), source.Expression, selector, Expression.Constant(cancellationToken, typeof(CancellationToken))), cancellationToken);
        }

        private static MethodInfo? s_SumAwaitAsync__TSource__3__2;
        
        private static MethodInfo? SumAwaitAsync__TSource__3__2(Type TSource) =>
            (s_SumAwaitAsync__TSource__3__2 ??
            (s_SumAwaitAsync__TSource__3__2 = new Func<IAsyncQueryable<object>, Expression<Func<object, ValueTask<double?>>>, CancellationToken, ValueTask<double?>>(SumAwaitAsync<object>).GetMethodInfo()!.GetGenericMethodDefinition())).MakeGenericMethod(TSource);

        public static ValueTask<double?> SumAwaitAsync<TSource>(this IAsyncQueryable<TSource> source, Expression<Func<TSource, ValueTask<double?>>> selector, CancellationToken cancellationToken = default)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));
            if (selector == null)
                throw new ArgumentNullException(nameof(selector));

            return source.Provider.ExecuteAsync<double?>(Expression.Call(SumAwaitAsync__TSource__3__2(typeof(TSource)), source.Expression, selector, Expression.Constant(cancellationToken, typeof(CancellationToken))), cancellationToken);
        }

        private static MethodInfo? s_SumAwaitAsync__TSource__3__3;
        
        private static MethodInfo? SumAwaitAsync__TSource__3__3(Type TSource) =>
            (s_SumAwaitAsync__TSource__3__3 ??
            (s_SumAwaitAsync__TSource__3__3 = new Func<IAsyncQueryable<object>, Expression<Func<object, ValueTask<double>>>, CancellationToken, ValueTask<double>>(SumAwaitAsync<object>).GetMethodInfo()!.GetGenericMethodDefinition())).MakeGenericMethod(TSource);

        public static ValueTask<double> SumAwaitAsync<TSource>(this IAsyncQueryable<TSource> source, Expression<Func<TSource, ValueTask<double>>> selector, CancellationToken cancellationToken = default)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));
            if (selector == null)
                throw new ArgumentNullException(nameof(selector));

            return source.Provider.ExecuteAsync<double>(Expression.Call(SumAwaitAsync__TSource__3__3(typeof(TSource)), source.Expression, selector, Expression.Constant(cancellationToken, typeof(CancellationToken))), cancellationToken);
        }

        private static MethodInfo? s_SumAwaitAsync__TSource__3__4;
        
        private static MethodInfo? SumAwaitAsync__TSource__3__4(Type TSource) =>
            (s_SumAwaitAsync__TSource__3__4 ??
            (s_SumAwaitAsync__TSource__3__4 = new Func<IAsyncQueryable<object>, Expression<Func<object, ValueTask<float?>>>, CancellationToken, ValueTask<float?>>(SumAwaitAsync<object>).GetMethodInfo()!.GetGenericMethodDefinition())).MakeGenericMethod(TSource);

        public static ValueTask<float?> SumAwaitAsync<TSource>(this IAsyncQueryable<TSource> source, Expression<Func<TSource, ValueTask<float?>>> selector, CancellationToken cancellationToken = default)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));
            if (selector == null)
                throw new ArgumentNullException(nameof(selector));

            return source.Provider.ExecuteAsync<float?>(Expression.Call(SumAwaitAsync__TSource__3__4(typeof(TSource)), source.Expression, selector, Expression.Constant(cancellationToken, typeof(CancellationToken))), cancellationToken);
        }

        private static MethodInfo? s_SumAwaitAsync__TSource__3__5;
        
        private static MethodInfo? SumAwaitAsync__TSource__3__5(Type TSource) =>
            (s_SumAwaitAsync__TSource__3__5 ??
            (s_SumAwaitAsync__TSource__3__5 = new Func<IAsyncQueryable<object>, Expression<Func<object, ValueTask<float>>>, CancellationToken, ValueTask<float>>(SumAwaitAsync<object>).GetMethodInfo()!.GetGenericMethodDefinition())).MakeGenericMethod(TSource);

        public static ValueTask<float> SumAwaitAsync<TSource>(this IAsyncQueryable<TSource> source, Expression<Func<TSource, ValueTask<float>>> selector, CancellationToken cancellationToken = default)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));
            if (selector == null)
                throw new ArgumentNullException(nameof(selector));

            return source.Provider.ExecuteAsync<float>(Expression.Call(SumAwaitAsync__TSource__3__5(typeof(TSource)), source.Expression, selector, Expression.Constant(cancellationToken, typeof(CancellationToken))), cancellationToken);
        }

        private static MethodInfo? s_SumAwaitAsync__TSource__3__6;
        
        private static MethodInfo? SumAwaitAsync__TSource__3__6(Type TSource) =>
            (s_SumAwaitAsync__TSource__3__6 ??
            (s_SumAwaitAsync__TSource__3__6 = new Func<IAsyncQueryable<object>, Expression<Func<object, ValueTask<int?>>>, CancellationToken, ValueTask<int?>>(SumAwaitAsync<object>).GetMethodInfo()!.GetGenericMethodDefinition())).MakeGenericMethod(TSource);

        public static ValueTask<int?> SumAwaitAsync<TSource>(this IAsyncQueryable<TSource> source, Expression<Func<TSource, ValueTask<int?>>> selector, CancellationToken cancellationToken = default)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));
            if (selector == null)
                throw new ArgumentNullException(nameof(selector));

            return source.Provider.ExecuteAsync<int?>(Expression.Call(SumAwaitAsync__TSource__3__6(typeof(TSource)), source.Expression, selector, Expression.Constant(cancellationToken, typeof(CancellationToken))), cancellationToken);
        }

        private static MethodInfo? s_SumAwaitAsync__TSource__3__7;
        
        private static MethodInfo? SumAwaitAsync__TSource__3__7(Type TSource) =>
            (s_SumAwaitAsync__TSource__3__7 ??
            (s_SumAwaitAsync__TSource__3__7 = new Func<IAsyncQueryable<object>, Expression<Func<object, ValueTask<int>>>, CancellationToken, ValueTask<int>>(SumAwaitAsync<object>).GetMethodInfo()!.GetGenericMethodDefinition())).MakeGenericMethod(TSource);

        public static ValueTask<int> SumAwaitAsync<TSource>(this IAsyncQueryable<TSource> source, Expression<Func<TSource, ValueTask<int>>> selector, CancellationToken cancellationToken = default)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));
            if (selector == null)
                throw new ArgumentNullException(nameof(selector));

            return source.Provider.ExecuteAsync<int>(Expression.Call(SumAwaitAsync__TSource__3__7(typeof(TSource)), source.Expression, selector, Expression.Constant(cancellationToken, typeof(CancellationToken))), cancellationToken);
        }

        private static MethodInfo? s_SumAwaitAsync__TSource__3__8;
        
        private static MethodInfo? SumAwaitAsync__TSource__3__8(Type TSource) =>
            (s_SumAwaitAsync__TSource__3__8 ??
            (s_SumAwaitAsync__TSource__3__8 = new Func<IAsyncQueryable<object>, Expression<Func<object, ValueTask<long?>>>, CancellationToken, ValueTask<long?>>(SumAwaitAsync<object>).GetMethodInfo()!.GetGenericMethodDefinition())).MakeGenericMethod(TSource);

        public static ValueTask<long?> SumAwaitAsync<TSource>(this IAsyncQueryable<TSource> source, Expression<Func<TSource, ValueTask<long?>>> selector, CancellationToken cancellationToken = default)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));
            if (selector == null)
                throw new ArgumentNullException(nameof(selector));

            return source.Provider.ExecuteAsync<long?>(Expression.Call(SumAwaitAsync__TSource__3__8(typeof(TSource)), source.Expression, selector, Expression.Constant(cancellationToken, typeof(CancellationToken))), cancellationToken);
        }

        private static MethodInfo? s_SumAwaitAsync__TSource__3__9;
        
        private static MethodInfo? SumAwaitAsync__TSource__3__9(Type TSource) =>
            (s_SumAwaitAsync__TSource__3__9 ??
            (s_SumAwaitAsync__TSource__3__9 = new Func<IAsyncQueryable<object>, Expression<Func<object, ValueTask<long>>>, CancellationToken, ValueTask<long>>(SumAwaitAsync<object>).GetMethodInfo()!.GetGenericMethodDefinition())).MakeGenericMethod(TSource);

        public static ValueTask<long> SumAwaitAsync<TSource>(this IAsyncQueryable<TSource> source, Expression<Func<TSource, ValueTask<long>>> selector, CancellationToken cancellationToken = default)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));
            if (selector == null)
                throw new ArgumentNullException(nameof(selector));

            return source.Provider.ExecuteAsync<long>(Expression.Call(SumAwaitAsync__TSource__3__9(typeof(TSource)), source.Expression, selector, Expression.Constant(cancellationToken, typeof(CancellationToken))), cancellationToken);
        }

        private static MethodInfo? s_SumAwaitWithCancellationAsync__TSource__3__0;
        
        private static MethodInfo? SumAwaitWithCancellationAsync__TSource__3__0(Type TSource) =>
            (s_SumAwaitWithCancellationAsync__TSource__3__0 ??
            (s_SumAwaitWithCancellationAsync__TSource__3__0 = new Func<IAsyncQueryable<object>, Expression<Func<object, CancellationToken, ValueTask<decimal?>>>, CancellationToken, ValueTask<decimal?>>(SumAwaitWithCancellationAsync<object>).GetMethodInfo()!.GetGenericMethodDefinition())).MakeGenericMethod(TSource);

        public static ValueTask<decimal?> SumAwaitWithCancellationAsync<TSource>(this IAsyncQueryable<TSource> source, Expression<Func<TSource, CancellationToken, ValueTask<decimal?>>> selector, CancellationToken cancellationToken = default)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));
            if (selector == null)
                throw new ArgumentNullException(nameof(selector));

            return source.Provider.ExecuteAsync<decimal?>(Expression.Call(SumAwaitWithCancellationAsync__TSource__3__0(typeof(TSource)), source.Expression, selector, Expression.Constant(cancellationToken, typeof(CancellationToken))), cancellationToken);
        }

        private static MethodInfo? s_SumAwaitWithCancellationAsync__TSource__3__1;
        
        private static MethodInfo? SumAwaitWithCancellationAsync__TSource__3__1(Type TSource) =>
            (s_SumAwaitWithCancellationAsync__TSource__3__1 ??
            (s_SumAwaitWithCancellationAsync__TSource__3__1 = new Func<IAsyncQueryable<object>, Expression<Func<object, CancellationToken, ValueTask<decimal>>>, CancellationToken, ValueTask<decimal>>(SumAwaitWithCancellationAsync<object>).GetMethodInfo()!.GetGenericMethodDefinition())).MakeGenericMethod(TSource);

        public static ValueTask<decimal> SumAwaitWithCancellationAsync<TSource>(this IAsyncQueryable<TSource> source, Expression<Func<TSource, CancellationToken, ValueTask<decimal>>> selector, CancellationToken cancellationToken = default)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));
            if (selector == null)
                throw new ArgumentNullException(nameof(selector));

            return source.Provider.ExecuteAsync<decimal>(Expression.Call(SumAwaitWithCancellationAsync__TSource__3__1(typeof(TSource)), source.Expression, selector, Expression.Constant(cancellationToken, typeof(CancellationToken))), cancellationToken);
        }

        private static MethodInfo? s_SumAwaitWithCancellationAsync__TSource__3__2;
        
        private static MethodInfo? SumAwaitWithCancellationAsync__TSource__3__2(Type TSource) =>
            (s_SumAwaitWithCancellationAsync__TSource__3__2 ??
            (s_SumAwaitWithCancellationAsync__TSource__3__2 = new Func<IAsyncQueryable<object>, Expression<Func<object, CancellationToken, ValueTask<double?>>>, CancellationToken, ValueTask<double?>>(SumAwaitWithCancellationAsync<object>).GetMethodInfo()!.GetGenericMethodDefinition())).MakeGenericMethod(TSource);

        public static ValueTask<double?> SumAwaitWithCancellationAsync<TSource>(this IAsyncQueryable<TSource> source, Expression<Func<TSource, CancellationToken, ValueTask<double?>>> selector, CancellationToken cancellationToken = default)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));
            if (selector == null)
                throw new ArgumentNullException(nameof(selector));

            return source.Provider.ExecuteAsync<double?>(Expression.Call(SumAwaitWithCancellationAsync__TSource__3__2(typeof(TSource)), source.Expression, selector, Expression.Constant(cancellationToken, typeof(CancellationToken))), cancellationToken);
        }

        private static MethodInfo? s_SumAwaitWithCancellationAsync__TSource__3__3;
        
        private static MethodInfo? SumAwaitWithCancellationAsync__TSource__3__3(Type TSource) =>
            (s_SumAwaitWithCancellationAsync__TSource__3__3 ??
            (s_SumAwaitWithCancellationAsync__TSource__3__3 = new Func<IAsyncQueryable<object>, Expression<Func<object, CancellationToken, ValueTask<double>>>, CancellationToken, ValueTask<double>>(SumAwaitWithCancellationAsync<object>).GetMethodInfo()!.GetGenericMethodDefinition())).MakeGenericMethod(TSource);

        public static ValueTask<double> SumAwaitWithCancellationAsync<TSource>(this IAsyncQueryable<TSource> source, Expression<Func<TSource, CancellationToken, ValueTask<double>>> selector, CancellationToken cancellationToken = default)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));
            if (selector == null)
                throw new ArgumentNullException(nameof(selector));

            return source.Provider.ExecuteAsync<double>(Expression.Call(SumAwaitWithCancellationAsync__TSource__3__3(typeof(TSource)), source.Expression, selector, Expression.Constant(cancellationToken, typeof(CancellationToken))), cancellationToken);
        }

        private static MethodInfo? s_SumAwaitWithCancellationAsync__TSource__3__4;
        
        private static MethodInfo? SumAwaitWithCancellationAsync__TSource__3__4(Type TSource) =>
            (s_SumAwaitWithCancellationAsync__TSource__3__4 ??
            (s_SumAwaitWithCancellationAsync__TSource__3__4 = new Func<IAsyncQueryable<object>, Expression<Func<object, CancellationToken, ValueTask<float?>>>, CancellationToken, ValueTask<float?>>(SumAwaitWithCancellationAsync<object>).GetMethodInfo()!.GetGenericMethodDefinition())).MakeGenericMethod(TSource);

        public static ValueTask<float?> SumAwaitWithCancellationAsync<TSource>(this IAsyncQueryable<TSource> source, Expression<Func<TSource, CancellationToken, ValueTask<float?>>> selector, CancellationToken cancellationToken = default)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));
            if (selector == null)
                throw new ArgumentNullException(nameof(selector));

            return source.Provider.ExecuteAsync<float?>(Expression.Call(SumAwaitWithCancellationAsync__TSource__3__4(typeof(TSource)), source.Expression, selector, Expression.Constant(cancellationToken, typeof(CancellationToken))), cancellationToken);
        }

        private static MethodInfo? s_SumAwaitWithCancellationAsync__TSource__3__5;
        
        private static MethodInfo? SumAwaitWithCancellationAsync__TSource__3__5(Type TSource) =>
            (s_SumAwaitWithCancellationAsync__TSource__3__5 ??
            (s_SumAwaitWithCancellationAsync__TSource__3__5 = new Func<IAsyncQueryable<object>, Expression<Func<object, CancellationToken, ValueTask<float>>>, CancellationToken, ValueTask<float>>(SumAwaitWithCancellationAsync<object>).GetMethodInfo()!.GetGenericMethodDefinition())).MakeGenericMethod(TSource);

        public static ValueTask<float> SumAwaitWithCancellationAsync<TSource>(this IAsyncQueryable<TSource> source, Expression<Func<TSource, CancellationToken, ValueTask<float>>> selector, CancellationToken cancellationToken = default)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));
            if (selector == null)
                throw new ArgumentNullException(nameof(selector));

            return source.Provider.ExecuteAsync<float>(Expression.Call(SumAwaitWithCancellationAsync__TSource__3__5(typeof(TSource)), source.Expression, selector, Expression.Constant(cancellationToken, typeof(CancellationToken))), cancellationToken);
        }

        private static MethodInfo? s_SumAwaitWithCancellationAsync__TSource__3__6;
        
        private static MethodInfo? SumAwaitWithCancellationAsync__TSource__3__6(Type TSource) =>
            (s_SumAwaitWithCancellationAsync__TSource__3__6 ??
            (s_SumAwaitWithCancellationAsync__TSource__3__6 = new Func<IAsyncQueryable<object>, Expression<Func<object, CancellationToken, ValueTask<int?>>>, CancellationToken, ValueTask<int?>>(SumAwaitWithCancellationAsync<object>).GetMethodInfo()!.GetGenericMethodDefinition())).MakeGenericMethod(TSource);

        public static ValueTask<int?> SumAwaitWithCancellationAsync<TSource>(this IAsyncQueryable<TSource> source, Expression<Func<TSource, CancellationToken, ValueTask<int?>>> selector, CancellationToken cancellationToken = default)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));
            if (selector == null)
                throw new ArgumentNullException(nameof(selector));

            return source.Provider.ExecuteAsync<int?>(Expression.Call(SumAwaitWithCancellationAsync__TSource__3__6(typeof(TSource)), source.Expression, selector, Expression.Constant(cancellationToken, typeof(CancellationToken))), cancellationToken);
        }

        private static MethodInfo? s_SumAwaitWithCancellationAsync__TSource__3__7;
        
        private static MethodInfo? SumAwaitWithCancellationAsync__TSource__3__7(Type TSource) =>
            (s_SumAwaitWithCancellationAsync__TSource__3__7 ??
            (s_SumAwaitWithCancellationAsync__TSource__3__7 = new Func<IAsyncQueryable<object>, Expression<Func<object, CancellationToken, ValueTask<int>>>, CancellationToken, ValueTask<int>>(SumAwaitWithCancellationAsync<object>).GetMethodInfo()!.GetGenericMethodDefinition())).MakeGenericMethod(TSource);

        public static ValueTask<int> SumAwaitWithCancellationAsync<TSource>(this IAsyncQueryable<TSource> source, Expression<Func<TSource, CancellationToken, ValueTask<int>>> selector, CancellationToken cancellationToken = default)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));
            if (selector == null)
                throw new ArgumentNullException(nameof(selector));

            return source.Provider.ExecuteAsync<int>(Expression.Call(SumAwaitWithCancellationAsync__TSource__3__7(typeof(TSource)), source.Expression, selector, Expression.Constant(cancellationToken, typeof(CancellationToken))), cancellationToken);
        }

        private static MethodInfo? s_SumAwaitWithCancellationAsync__TSource__3__8;
        
        private static MethodInfo? SumAwaitWithCancellationAsync__TSource__3__8(Type TSource) =>
            (s_SumAwaitWithCancellationAsync__TSource__3__8 ??
            (s_SumAwaitWithCancellationAsync__TSource__3__8 = new Func<IAsyncQueryable<object>, Expression<Func<object, CancellationToken, ValueTask<long?>>>, CancellationToken, ValueTask<long?>>(SumAwaitWithCancellationAsync<object>).GetMethodInfo()!.GetGenericMethodDefinition())).MakeGenericMethod(TSource);

        public static ValueTask<long?> SumAwaitWithCancellationAsync<TSource>(this IAsyncQueryable<TSource> source, Expression<Func<TSource, CancellationToken, ValueTask<long?>>> selector, CancellationToken cancellationToken = default)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));
            if (selector == null)
                throw new ArgumentNullException(nameof(selector));

            return source.Provider.ExecuteAsync<long?>(Expression.Call(SumAwaitWithCancellationAsync__TSource__3__8(typeof(TSource)), source.Expression, selector, Expression.Constant(cancellationToken, typeof(CancellationToken))), cancellationToken);
        }

        private static MethodInfo? s_SumAwaitWithCancellationAsync__TSource__3__9;
        
        private static MethodInfo? SumAwaitWithCancellationAsync__TSource__3__9(Type TSource) =>
            (s_SumAwaitWithCancellationAsync__TSource__3__9 ??
            (s_SumAwaitWithCancellationAsync__TSource__3__9 = new Func<IAsyncQueryable<object>, Expression<Func<object, CancellationToken, ValueTask<long>>>, CancellationToken, ValueTask<long>>(SumAwaitWithCancellationAsync<object>).GetMethodInfo()!.GetGenericMethodDefinition())).MakeGenericMethod(TSource);

        public static ValueTask<long> SumAwaitWithCancellationAsync<TSource>(this IAsyncQueryable<TSource> source, Expression<Func<TSource, CancellationToken, ValueTask<long>>> selector, CancellationToken cancellationToken = default)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));
            if (selector == null)
                throw new ArgumentNullException(nameof(selector));

            return source.Provider.ExecuteAsync<long>(Expression.Call(SumAwaitWithCancellationAsync__TSource__3__9(typeof(TSource)), source.Expression, selector, Expression.Constant(cancellationToken, typeof(CancellationToken))), cancellationToken);
        }

        private static MethodInfo? s_Take__TSource__2__0;
        
        private static MethodInfo? Take__TSource__2__0(Type TSource) =>
            (s_Take__TSource__2__0 ??
            (s_Take__TSource__2__0 = new Func<IAsyncQueryable<object>, int, IAsyncQueryable<object>>(Take<object>).GetMethodInfo()!.GetGenericMethodDefinition())).MakeGenericMethod(TSource);

        public static IAsyncQueryable<TSource> Take<TSource>(this IAsyncQueryable<TSource> source, int count)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));

            return source.Provider.CreateQuery<TSource>(Expression.Call(Take__TSource__2__0(typeof(TSource)), source.Expression, Expression.Constant(count, typeof(int))));
        }

        private static MethodInfo? s_TakeLast__TSource__2__0;
        
        private static MethodInfo? TakeLast__TSource__2__0(Type TSource) =>
            (s_TakeLast__TSource__2__0 ??
            (s_TakeLast__TSource__2__0 = new Func<IAsyncQueryable<object>, int, IAsyncQueryable<object>>(TakeLast<object>).GetMethodInfo()!.GetGenericMethodDefinition())).MakeGenericMethod(TSource);

        public static IAsyncQueryable<TSource> TakeLast<TSource>(this IAsyncQueryable<TSource> source, int count)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));

            return source.Provider.CreateQuery<TSource>(Expression.Call(TakeLast__TSource__2__0(typeof(TSource)), source.Expression, Expression.Constant(count, typeof(int))));
        }

        private static MethodInfo? s_TakeWhile__TSource__2__0;
        
        private static MethodInfo? TakeWhile__TSource__2__0(Type TSource) =>
            (s_TakeWhile__TSource__2__0 ??
            (s_TakeWhile__TSource__2__0 = new Func<IAsyncQueryable<object>, Expression<Func<object, bool>>, IAsyncQueryable<object>>(TakeWhile<object>).GetMethodInfo()!.GetGenericMethodDefinition())).MakeGenericMethod(TSource);

        public static IAsyncQueryable<TSource> TakeWhile<TSource>(this IAsyncQueryable<TSource> source, Expression<Func<TSource, bool>> predicate)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));
            if (predicate == null)
                throw new ArgumentNullException(nameof(predicate));

            return source.Provider.CreateQuery<TSource>(Expression.Call(TakeWhile__TSource__2__0(typeof(TSource)), source.Expression, predicate));
        }

        private static MethodInfo? s_TakeWhile__TSource__2__1;
        
        private static MethodInfo? TakeWhile__TSource__2__1(Type TSource) =>
            (s_TakeWhile__TSource__2__1 ??
            (s_TakeWhile__TSource__2__1 = new Func<IAsyncQueryable<object>, Expression<Func<object, int, bool>>, IAsyncQueryable<object>>(TakeWhile<object>).GetMethodInfo()!.GetGenericMethodDefinition())).MakeGenericMethod(TSource);

        public static IAsyncQueryable<TSource> TakeWhile<TSource>(this IAsyncQueryable<TSource> source, Expression<Func<TSource, int, bool>> predicate)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));
            if (predicate == null)
                throw new ArgumentNullException(nameof(predicate));

            return source.Provider.CreateQuery<TSource>(Expression.Call(TakeWhile__TSource__2__1(typeof(TSource)), source.Expression, predicate));
        }

        private static MethodInfo? s_TakeWhileAwait__TSource__2__0;
        
        private static MethodInfo? TakeWhileAwait__TSource__2__0(Type TSource) =>
            (s_TakeWhileAwait__TSource__2__0 ??
            (s_TakeWhileAwait__TSource__2__0 = new Func<IAsyncQueryable<object>, Expression<Func<object, int, ValueTask<bool>>>, IAsyncQueryable<object>>(TakeWhileAwait<object>).GetMethodInfo()!.GetGenericMethodDefinition())).MakeGenericMethod(TSource);

        public static IAsyncQueryable<TSource> TakeWhileAwait<TSource>(this IAsyncQueryable<TSource> source, Expression<Func<TSource, int, ValueTask<bool>>> predicate)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));
            if (predicate == null)
                throw new ArgumentNullException(nameof(predicate));

            return source.Provider.CreateQuery<TSource>(Expression.Call(TakeWhileAwait__TSource__2__0(typeof(TSource)), source.Expression, predicate));
        }

        private static MethodInfo? s_TakeWhileAwait__TSource__2__1;
        
        private static MethodInfo? TakeWhileAwait__TSource__2__1(Type TSource) =>
            (s_TakeWhileAwait__TSource__2__1 ??
            (s_TakeWhileAwait__TSource__2__1 = new Func<IAsyncQueryable<object>, Expression<Func<object, ValueTask<bool>>>, IAsyncQueryable<object>>(TakeWhileAwait<object>).GetMethodInfo()!.GetGenericMethodDefinition())).MakeGenericMethod(TSource);

        public static IAsyncQueryable<TSource> TakeWhileAwait<TSource>(this IAsyncQueryable<TSource> source, Expression<Func<TSource, ValueTask<bool>>> predicate)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));
            if (predicate == null)
                throw new ArgumentNullException(nameof(predicate));

            return source.Provider.CreateQuery<TSource>(Expression.Call(TakeWhileAwait__TSource__2__1(typeof(TSource)), source.Expression, predicate));
        }

        private static MethodInfo? s_TakeWhileAwaitWithCancellation__TSource__2__0;
        
        private static MethodInfo? TakeWhileAwaitWithCancellation__TSource__2__0(Type TSource) =>
            (s_TakeWhileAwaitWithCancellation__TSource__2__0 ??
            (s_TakeWhileAwaitWithCancellation__TSource__2__0 = new Func<IAsyncQueryable<object>, Expression<Func<object, CancellationToken, ValueTask<bool>>>, IAsyncQueryable<object>>(TakeWhileAwaitWithCancellation<object>).GetMethodInfo()!.GetGenericMethodDefinition())).MakeGenericMethod(TSource);

        public static IAsyncQueryable<TSource> TakeWhileAwaitWithCancellation<TSource>(this IAsyncQueryable<TSource> source, Expression<Func<TSource, CancellationToken, ValueTask<bool>>> predicate)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));
            if (predicate == null)
                throw new ArgumentNullException(nameof(predicate));

            return source.Provider.CreateQuery<TSource>(Expression.Call(TakeWhileAwaitWithCancellation__TSource__2__0(typeof(TSource)), source.Expression, predicate));
        }

        private static MethodInfo? s_TakeWhileAwaitWithCancellation__TSource__2__1;
        
        private static MethodInfo? TakeWhileAwaitWithCancellation__TSource__2__1(Type TSource) =>
            (s_TakeWhileAwaitWithCancellation__TSource__2__1 ??
            (s_TakeWhileAwaitWithCancellation__TSource__2__1 = new Func<IAsyncQueryable<object>, Expression<Func<object, int, CancellationToken, ValueTask<bool>>>, IAsyncQueryable<object>>(TakeWhileAwaitWithCancellation<object>).GetMethodInfo()!.GetGenericMethodDefinition())).MakeGenericMethod(TSource);

        public static IAsyncQueryable<TSource> TakeWhileAwaitWithCancellation<TSource>(this IAsyncQueryable<TSource> source, Expression<Func<TSource, int, CancellationToken, ValueTask<bool>>> predicate)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));
            if (predicate == null)
                throw new ArgumentNullException(nameof(predicate));

            return source.Provider.CreateQuery<TSource>(Expression.Call(TakeWhileAwaitWithCancellation__TSource__2__1(typeof(TSource)), source.Expression, predicate));
        }

        private static MethodInfo? s_ThenBy__TSource_TKey__2__0;
        
        private static MethodInfo? ThenBy__TSource_TKey__2__0(Type TSource, Type TKey) =>
            (s_ThenBy__TSource_TKey__2__0 ??
            (s_ThenBy__TSource_TKey__2__0 = new Func<IOrderedAsyncQueryable<object>, Expression<Func<object, object>>, IOrderedAsyncQueryable<object>>(ThenBy<object, object>).GetMethodInfo()!.GetGenericMethodDefinition())).MakeGenericMethod(TSource, TKey);

        public static IOrderedAsyncQueryable<TSource> ThenBy<TSource, TKey>(this IOrderedAsyncQueryable<TSource> source, Expression<Func<TSource, TKey>> keySelector)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));
            if (keySelector == null)
                throw new ArgumentNullException(nameof(keySelector));

            return (IOrderedAsyncQueryable<TSource>)source.Provider.CreateQuery<TSource>(Expression.Call(ThenBy__TSource_TKey__2__0(typeof(TSource), typeof(TKey)), source.Expression, keySelector));
        }

        private static MethodInfo? s_ThenBy__TSource_TKey__3__0;
        
        private static MethodInfo? ThenBy__TSource_TKey__3__0(Type TSource, Type TKey) =>
            (s_ThenBy__TSource_TKey__3__0 ??
            (s_ThenBy__TSource_TKey__3__0 = new Func<IOrderedAsyncQueryable<object>, Expression<Func<object, object>>, IComparer<object>, IOrderedAsyncQueryable<object>>(ThenBy<object, object>).GetMethodInfo()!.GetGenericMethodDefinition())).MakeGenericMethod(TSource, TKey);

        public static IOrderedAsyncQueryable<TSource> ThenBy<TSource, TKey>(this IOrderedAsyncQueryable<TSource> source, Expression<Func<TSource, TKey>> keySelector, IComparer<TKey>? comparer)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));
            if (keySelector == null)
                throw new ArgumentNullException(nameof(keySelector));

            return (IOrderedAsyncQueryable<TSource>)source.Provider.CreateQuery<TSource>(Expression.Call(ThenBy__TSource_TKey__3__0(typeof(TSource), typeof(TKey)), source.Expression, keySelector, Expression.Constant(comparer, typeof(IComparer<TKey>))));
        }

        private static MethodInfo? s_ThenByAwait__TSource_TKey__2__0;
        
        private static MethodInfo? ThenByAwait__TSource_TKey__2__0(Type TSource, Type TKey) =>
            (s_ThenByAwait__TSource_TKey__2__0 ??
            (s_ThenByAwait__TSource_TKey__2__0 = new Func<IOrderedAsyncQueryable<object>, Expression<Func<object, ValueTask<object>>>, IOrderedAsyncQueryable<object>>(ThenByAwait<object, object>).GetMethodInfo()!.GetGenericMethodDefinition())).MakeGenericMethod(TSource, TKey);

        public static IOrderedAsyncQueryable<TSource> ThenByAwait<TSource, TKey>(this IOrderedAsyncQueryable<TSource> source, Expression<Func<TSource, ValueTask<TKey>>> keySelector)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));
            if (keySelector == null)
                throw new ArgumentNullException(nameof(keySelector));

            return (IOrderedAsyncQueryable<TSource>)source.Provider.CreateQuery<TSource>(Expression.Call(ThenByAwait__TSource_TKey__2__0(typeof(TSource), typeof(TKey)), source.Expression, keySelector));
        }

        private static MethodInfo? s_ThenByAwait__TSource_TKey__3__0;
        
        private static MethodInfo? ThenByAwait__TSource_TKey__3__0(Type TSource, Type TKey) =>
            (s_ThenByAwait__TSource_TKey__3__0 ??
            (s_ThenByAwait__TSource_TKey__3__0 = new Func<IOrderedAsyncQueryable<object>, Expression<Func<object, ValueTask<object>>>, IComparer<object>, IOrderedAsyncQueryable<object>>(ThenByAwait<object, object>).GetMethodInfo()!.GetGenericMethodDefinition())).MakeGenericMethod(TSource, TKey);

        public static IOrderedAsyncQueryable<TSource> ThenByAwait<TSource, TKey>(this IOrderedAsyncQueryable<TSource> source, Expression<Func<TSource, ValueTask<TKey>>> keySelector, IComparer<TKey>? comparer)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));
            if (keySelector == null)
                throw new ArgumentNullException(nameof(keySelector));

            return (IOrderedAsyncQueryable<TSource>)source.Provider.CreateQuery<TSource>(Expression.Call(ThenByAwait__TSource_TKey__3__0(typeof(TSource), typeof(TKey)), source.Expression, keySelector, Expression.Constant(comparer, typeof(IComparer<TKey>))));
        }

        private static MethodInfo? s_ThenByAwaitWithCancellation__TSource_TKey__2__0;
        
        private static MethodInfo? ThenByAwaitWithCancellation__TSource_TKey__2__0(Type TSource, Type TKey) =>
            (s_ThenByAwaitWithCancellation__TSource_TKey__2__0 ??
            (s_ThenByAwaitWithCancellation__TSource_TKey__2__0 = new Func<IOrderedAsyncQueryable<object>, Expression<Func<object, CancellationToken, ValueTask<object>>>, IOrderedAsyncQueryable<object>>(ThenByAwaitWithCancellation<object, object>).GetMethodInfo()!.GetGenericMethodDefinition())).MakeGenericMethod(TSource, TKey);

        public static IOrderedAsyncQueryable<TSource> ThenByAwaitWithCancellation<TSource, TKey>(this IOrderedAsyncQueryable<TSource> source, Expression<Func<TSource, CancellationToken, ValueTask<TKey>>> keySelector)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));
            if (keySelector == null)
                throw new ArgumentNullException(nameof(keySelector));

            return (IOrderedAsyncQueryable<TSource>)source.Provider.CreateQuery<TSource>(Expression.Call(ThenByAwaitWithCancellation__TSource_TKey__2__0(typeof(TSource), typeof(TKey)), source.Expression, keySelector));
        }

        private static MethodInfo? s_ThenByAwaitWithCancellation__TSource_TKey__3__0;
        
        private static MethodInfo? ThenByAwaitWithCancellation__TSource_TKey__3__0(Type TSource, Type TKey) =>
            (s_ThenByAwaitWithCancellation__TSource_TKey__3__0 ??
            (s_ThenByAwaitWithCancellation__TSource_TKey__3__0 = new Func<IOrderedAsyncQueryable<object>, Expression<Func<object, CancellationToken, ValueTask<object>>>, IComparer<object>, IOrderedAsyncQueryable<object>>(ThenByAwaitWithCancellation<object, object>).GetMethodInfo()!.GetGenericMethodDefinition())).MakeGenericMethod(TSource, TKey);

        public static IOrderedAsyncQueryable<TSource> ThenByAwaitWithCancellation<TSource, TKey>(this IOrderedAsyncQueryable<TSource> source, Expression<Func<TSource, CancellationToken, ValueTask<TKey>>> keySelector, IComparer<TKey>? comparer)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));
            if (keySelector == null)
                throw new ArgumentNullException(nameof(keySelector));

            return (IOrderedAsyncQueryable<TSource>)source.Provider.CreateQuery<TSource>(Expression.Call(ThenByAwaitWithCancellation__TSource_TKey__3__0(typeof(TSource), typeof(TKey)), source.Expression, keySelector, Expression.Constant(comparer, typeof(IComparer<TKey>))));
        }

        private static MethodInfo? s_ThenByDescending__TSource_TKey__2__0;
        
        private static MethodInfo? ThenByDescending__TSource_TKey__2__0(Type TSource, Type TKey) =>
            (s_ThenByDescending__TSource_TKey__2__0 ??
            (s_ThenByDescending__TSource_TKey__2__0 = new Func<IOrderedAsyncQueryable<object>, Expression<Func<object, object>>, IOrderedAsyncQueryable<object>>(ThenByDescending<object, object>).GetMethodInfo()!.GetGenericMethodDefinition())).MakeGenericMethod(TSource, TKey);

        public static IOrderedAsyncQueryable<TSource> ThenByDescending<TSource, TKey>(this IOrderedAsyncQueryable<TSource> source, Expression<Func<TSource, TKey>> keySelector)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));
            if (keySelector == null)
                throw new ArgumentNullException(nameof(keySelector));

            return (IOrderedAsyncQueryable<TSource>)source.Provider.CreateQuery<TSource>(Expression.Call(ThenByDescending__TSource_TKey__2__0(typeof(TSource), typeof(TKey)), source.Expression, keySelector));
        }

        private static MethodInfo? s_ThenByDescending__TSource_TKey__3__0;
        
        private static MethodInfo? ThenByDescending__TSource_TKey__3__0(Type TSource, Type TKey) =>
            (s_ThenByDescending__TSource_TKey__3__0 ??
            (s_ThenByDescending__TSource_TKey__3__0 = new Func<IOrderedAsyncQueryable<object>, Expression<Func<object, object>>, IComparer<object>, IOrderedAsyncQueryable<object>>(ThenByDescending<object, object>).GetMethodInfo()!.GetGenericMethodDefinition())).MakeGenericMethod(TSource, TKey);

        public static IOrderedAsyncQueryable<TSource> ThenByDescending<TSource, TKey>(this IOrderedAsyncQueryable<TSource> source, Expression<Func<TSource, TKey>> keySelector, IComparer<TKey>? comparer)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));
            if (keySelector == null)
                throw new ArgumentNullException(nameof(keySelector));

            return (IOrderedAsyncQueryable<TSource>)source.Provider.CreateQuery<TSource>(Expression.Call(ThenByDescending__TSource_TKey__3__0(typeof(TSource), typeof(TKey)), source.Expression, keySelector, Expression.Constant(comparer, typeof(IComparer<TKey>))));
        }

        private static MethodInfo? s_ThenByDescendingAwait__TSource_TKey__2__0;
        
        private static MethodInfo? ThenByDescendingAwait__TSource_TKey__2__0(Type TSource, Type TKey) =>
            (s_ThenByDescendingAwait__TSource_TKey__2__0 ??
            (s_ThenByDescendingAwait__TSource_TKey__2__0 = new Func<IOrderedAsyncQueryable<object>, Expression<Func<object, ValueTask<object>>>, IOrderedAsyncQueryable<object>>(ThenByDescendingAwait<object, object>).GetMethodInfo()!.GetGenericMethodDefinition())).MakeGenericMethod(TSource, TKey);

        public static IOrderedAsyncQueryable<TSource> ThenByDescendingAwait<TSource, TKey>(this IOrderedAsyncQueryable<TSource> source, Expression<Func<TSource, ValueTask<TKey>>> keySelector)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));
            if (keySelector == null)
                throw new ArgumentNullException(nameof(keySelector));

            return (IOrderedAsyncQueryable<TSource>)source.Provider.CreateQuery<TSource>(Expression.Call(ThenByDescendingAwait__TSource_TKey__2__0(typeof(TSource), typeof(TKey)), source.Expression, keySelector));
        }

        private static MethodInfo? s_ThenByDescendingAwait__TSource_TKey__3__0;
        
        private static MethodInfo? ThenByDescendingAwait__TSource_TKey__3__0(Type TSource, Type TKey) =>
            (s_ThenByDescendingAwait__TSource_TKey__3__0 ??
            (s_ThenByDescendingAwait__TSource_TKey__3__0 = new Func<IOrderedAsyncQueryable<object>, Expression<Func<object, ValueTask<object>>>, IComparer<object>, IOrderedAsyncQueryable<object>>(ThenByDescendingAwait<object, object>).GetMethodInfo()!.GetGenericMethodDefinition())).MakeGenericMethod(TSource, TKey);

        public static IOrderedAsyncQueryable<TSource> ThenByDescendingAwait<TSource, TKey>(this IOrderedAsyncQueryable<TSource> source, Expression<Func<TSource, ValueTask<TKey>>> keySelector, IComparer<TKey>? comparer)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));
            if (keySelector == null)
                throw new ArgumentNullException(nameof(keySelector));

            return (IOrderedAsyncQueryable<TSource>)source.Provider.CreateQuery<TSource>(Expression.Call(ThenByDescendingAwait__TSource_TKey__3__0(typeof(TSource), typeof(TKey)), source.Expression, keySelector, Expression.Constant(comparer, typeof(IComparer<TKey>))));
        }

        private static MethodInfo? s_ThenByDescendingAwaitWithCancellation__TSource_TKey__2__0;
        
        private static MethodInfo? ThenByDescendingAwaitWithCancellation__TSource_TKey__2__0(Type TSource, Type TKey) =>
            (s_ThenByDescendingAwaitWithCancellation__TSource_TKey__2__0 ??
            (s_ThenByDescendingAwaitWithCancellation__TSource_TKey__2__0 = new Func<IOrderedAsyncQueryable<object>, Expression<Func<object, CancellationToken, ValueTask<object>>>, IOrderedAsyncQueryable<object>>(ThenByDescendingAwaitWithCancellation<object, object>).GetMethodInfo()!.GetGenericMethodDefinition())).MakeGenericMethod(TSource, TKey);

        public static IOrderedAsyncQueryable<TSource> ThenByDescendingAwaitWithCancellation<TSource, TKey>(this IOrderedAsyncQueryable<TSource> source, Expression<Func<TSource, CancellationToken, ValueTask<TKey>>> keySelector)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));
            if (keySelector == null)
                throw new ArgumentNullException(nameof(keySelector));

            return (IOrderedAsyncQueryable<TSource>)source.Provider.CreateQuery<TSource>(Expression.Call(ThenByDescendingAwaitWithCancellation__TSource_TKey__2__0(typeof(TSource), typeof(TKey)), source.Expression, keySelector));
        }

        private static MethodInfo? s_ThenByDescendingAwaitWithCancellation__TSource_TKey__3__0;
        
        private static MethodInfo? ThenByDescendingAwaitWithCancellation__TSource_TKey__3__0(Type TSource, Type TKey) =>
            (s_ThenByDescendingAwaitWithCancellation__TSource_TKey__3__0 ??
            (s_ThenByDescendingAwaitWithCancellation__TSource_TKey__3__0 = new Func<IOrderedAsyncQueryable<object>, Expression<Func<object, CancellationToken, ValueTask<object>>>, IComparer<object>, IOrderedAsyncQueryable<object>>(ThenByDescendingAwaitWithCancellation<object, object>).GetMethodInfo()!.GetGenericMethodDefinition())).MakeGenericMethod(TSource, TKey);

        public static IOrderedAsyncQueryable<TSource> ThenByDescendingAwaitWithCancellation<TSource, TKey>(this IOrderedAsyncQueryable<TSource> source, Expression<Func<TSource, CancellationToken, ValueTask<TKey>>> keySelector, IComparer<TKey>? comparer)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));
            if (keySelector == null)
                throw new ArgumentNullException(nameof(keySelector));

            return (IOrderedAsyncQueryable<TSource>)source.Provider.CreateQuery<TSource>(Expression.Call(ThenByDescendingAwaitWithCancellation__TSource_TKey__3__0(typeof(TSource), typeof(TKey)), source.Expression, keySelector, Expression.Constant(comparer, typeof(IComparer<TKey>))));
        }

        private static MethodInfo? s_ToArrayAsync__TSource__2__0;
        
        private static MethodInfo? ToArrayAsync__TSource__2__0(Type TSource) =>
            (s_ToArrayAsync__TSource__2__0 ??
            (s_ToArrayAsync__TSource__2__0 = new Func<IAsyncQueryable<object>, CancellationToken, ValueTask<object[]>>(ToArrayAsync<object>).GetMethodInfo()!.GetGenericMethodDefinition())).MakeGenericMethod(TSource);

        public static ValueTask<TSource[]> ToArrayAsync<TSource>(this IAsyncQueryable<TSource> source, CancellationToken cancellationToken = default)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));

            return source.Provider.ExecuteAsync<TSource[]>(Expression.Call(ToArrayAsync__TSource__2__0(typeof(TSource)), source.Expression, Expression.Constant(cancellationToken, typeof(CancellationToken))), cancellationToken);
        }

        private static MethodInfo? s_ToDictionaryAsync__TSource_TKey__3__0;
        
        private static MethodInfo? ToDictionaryAsync__TSource_TKey__3__0(Type TSource, Type TKey) =>
            (s_ToDictionaryAsync__TSource_TKey__3__0 ??
            (s_ToDictionaryAsync__TSource_TKey__3__0 = new Func<IAsyncQueryable<object>, Expression<Func<object, object>>, CancellationToken, ValueTask<Dictionary<object, object>>>(ToDictionaryAsync<object, object>).GetMethodInfo()!.GetGenericMethodDefinition())).MakeGenericMethod(TSource, TKey);

        public static ValueTask<Dictionary<TKey, TSource>> ToDictionaryAsync<TSource, TKey>(this IAsyncQueryable<TSource> source, Expression<Func<TSource, TKey>> keySelector, CancellationToken cancellationToken = default) where TKey : notnull
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));
            if (keySelector == null)
                throw new ArgumentNullException(nameof(keySelector));

            return source.Provider.ExecuteAsync<Dictionary<TKey, TSource>>(Expression.Call(ToDictionaryAsync__TSource_TKey__3__0(typeof(TSource), typeof(TKey)), source.Expression, keySelector, Expression.Constant(cancellationToken, typeof(CancellationToken))), cancellationToken);
        }

        private static MethodInfo? s_ToDictionaryAsync__TSource_TKey__4__0;
        
        private static MethodInfo? ToDictionaryAsync__TSource_TKey__4__0(Type TSource, Type TKey) =>
            (s_ToDictionaryAsync__TSource_TKey__4__0 ??
            (s_ToDictionaryAsync__TSource_TKey__4__0 = new Func<IAsyncQueryable<object>, Expression<Func<object, object>>, IEqualityComparer<object>, CancellationToken, ValueTask<Dictionary<object, object>>>(ToDictionaryAsync<object, object>).GetMethodInfo()!.GetGenericMethodDefinition())).MakeGenericMethod(TSource, TKey);

        public static ValueTask<Dictionary<TKey, TSource>> ToDictionaryAsync<TSource, TKey>(this IAsyncQueryable<TSource> source, Expression<Func<TSource, TKey>> keySelector, IEqualityComparer<TKey>? comparer, CancellationToken cancellationToken = default) where TKey : notnull
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));
            if (keySelector == null)
                throw new ArgumentNullException(nameof(keySelector));

            return source.Provider.ExecuteAsync<Dictionary<TKey, TSource>>(Expression.Call(ToDictionaryAsync__TSource_TKey__4__0(typeof(TSource), typeof(TKey)), source.Expression, keySelector, Expression.Constant(comparer, typeof(IEqualityComparer<TKey>)), Expression.Constant(cancellationToken, typeof(CancellationToken))), cancellationToken);
        }

        private static MethodInfo? s_ToDictionaryAsync__TSource_TKey_TElement__4__0;
        
        private static MethodInfo? ToDictionaryAsync__TSource_TKey_TElement__4__0(Type TSource, Type TKey, Type TElement) =>
            (s_ToDictionaryAsync__TSource_TKey_TElement__4__0 ??
            (s_ToDictionaryAsync__TSource_TKey_TElement__4__0 = new Func<IAsyncQueryable<object>, Expression<Func<object, object>>, Expression<Func<object, object>>, CancellationToken, ValueTask<Dictionary<object, object>>>(ToDictionaryAsync<object, object, object>).GetMethodInfo()!.GetGenericMethodDefinition())).MakeGenericMethod(TSource, TKey, TElement);

        public static ValueTask<Dictionary<TKey, TElement>> ToDictionaryAsync<TSource, TKey, TElement>(this IAsyncQueryable<TSource> source, Expression<Func<TSource, TKey>> keySelector, Expression<Func<TSource, TElement>> elementSelector, CancellationToken cancellationToken = default) where TKey : notnull
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));
            if (keySelector == null)
                throw new ArgumentNullException(nameof(keySelector));
            if (elementSelector == null)
                throw new ArgumentNullException(nameof(elementSelector));

            return source.Provider.ExecuteAsync<Dictionary<TKey, TElement>>(Expression.Call(ToDictionaryAsync__TSource_TKey_TElement__4__0(typeof(TSource), typeof(TKey), typeof(TElement)), source.Expression, keySelector, elementSelector, Expression.Constant(cancellationToken, typeof(CancellationToken))), cancellationToken);
        }

        private static MethodInfo? s_ToDictionaryAsync__TSource_TKey_TElement__5__0;
        
        private static MethodInfo? ToDictionaryAsync__TSource_TKey_TElement__5__0(Type TSource, Type TKey, Type TElement) =>
            (s_ToDictionaryAsync__TSource_TKey_TElement__5__0 ??
            (s_ToDictionaryAsync__TSource_TKey_TElement__5__0 = new Func<IAsyncQueryable<object>, Expression<Func<object, object>>, Expression<Func<object, object>>, IEqualityComparer<object>, CancellationToken, ValueTask<Dictionary<object, object>>>(ToDictionaryAsync<object, object, object>).GetMethodInfo()!.GetGenericMethodDefinition())).MakeGenericMethod(TSource, TKey, TElement);

        public static ValueTask<Dictionary<TKey, TElement>> ToDictionaryAsync<TSource, TKey, TElement>(this IAsyncQueryable<TSource> source, Expression<Func<TSource, TKey>> keySelector, Expression<Func<TSource, TElement>> elementSelector, IEqualityComparer<TKey>? comparer, CancellationToken cancellationToken = default) where TKey : notnull
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));
            if (keySelector == null)
                throw new ArgumentNullException(nameof(keySelector));
            if (elementSelector == null)
                throw new ArgumentNullException(nameof(elementSelector));

            return source.Provider.ExecuteAsync<Dictionary<TKey, TElement>>(Expression.Call(ToDictionaryAsync__TSource_TKey_TElement__5__0(typeof(TSource), typeof(TKey), typeof(TElement)), source.Expression, keySelector, elementSelector, Expression.Constant(comparer, typeof(IEqualityComparer<TKey>)), Expression.Constant(cancellationToken, typeof(CancellationToken))), cancellationToken);
        }

        private static MethodInfo? s_ToDictionaryAwaitAsync__TSource_TKey__3__0;
        
        private static MethodInfo? ToDictionaryAwaitAsync__TSource_TKey__3__0(Type TSource, Type TKey) =>
            (s_ToDictionaryAwaitAsync__TSource_TKey__3__0 ??
            (s_ToDictionaryAwaitAsync__TSource_TKey__3__0 = new Func<IAsyncQueryable<object>, Expression<Func<object, ValueTask<object>>>, CancellationToken, ValueTask<Dictionary<object, object>>>(ToDictionaryAwaitAsync<object, object>).GetMethodInfo()!.GetGenericMethodDefinition())).MakeGenericMethod(TSource, TKey);

        public static ValueTask<Dictionary<TKey, TSource>> ToDictionaryAwaitAsync<TSource, TKey>(this IAsyncQueryable<TSource> source, Expression<Func<TSource, ValueTask<TKey>>> keySelector, CancellationToken cancellationToken = default) where TKey : notnull
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));
            if (keySelector == null)
                throw new ArgumentNullException(nameof(keySelector));

            return source.Provider.ExecuteAsync<Dictionary<TKey, TSource>>(Expression.Call(ToDictionaryAwaitAsync__TSource_TKey__3__0(typeof(TSource), typeof(TKey)), source.Expression, keySelector, Expression.Constant(cancellationToken, typeof(CancellationToken))), cancellationToken);
        }

        private static MethodInfo? s_ToDictionaryAwaitAsync__TSource_TKey__4__0;
        
        private static MethodInfo? ToDictionaryAwaitAsync__TSource_TKey__4__0(Type TSource, Type TKey) =>
            (s_ToDictionaryAwaitAsync__TSource_TKey__4__0 ??
            (s_ToDictionaryAwaitAsync__TSource_TKey__4__0 = new Func<IAsyncQueryable<object>, Expression<Func<object, ValueTask<object>>>, IEqualityComparer<object>, CancellationToken, ValueTask<Dictionary<object, object>>>(ToDictionaryAwaitAsync<object, object>).GetMethodInfo()!.GetGenericMethodDefinition())).MakeGenericMethod(TSource, TKey);

        public static ValueTask<Dictionary<TKey, TSource>> ToDictionaryAwaitAsync<TSource, TKey>(this IAsyncQueryable<TSource> source, Expression<Func<TSource, ValueTask<TKey>>> keySelector, IEqualityComparer<TKey>? comparer, CancellationToken cancellationToken = default) where TKey : notnull
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));
            if (keySelector == null)
                throw new ArgumentNullException(nameof(keySelector));

            return source.Provider.ExecuteAsync<Dictionary<TKey, TSource>>(Expression.Call(ToDictionaryAwaitAsync__TSource_TKey__4__0(typeof(TSource), typeof(TKey)), source.Expression, keySelector, Expression.Constant(comparer, typeof(IEqualityComparer<TKey>)), Expression.Constant(cancellationToken, typeof(CancellationToken))), cancellationToken);
        }

        private static MethodInfo? s_ToDictionaryAwaitAsync__TSource_TKey_TElement__4__0;
        
        private static MethodInfo? ToDictionaryAwaitAsync__TSource_TKey_TElement__4__0(Type TSource, Type TKey, Type TElement) =>
            (s_ToDictionaryAwaitAsync__TSource_TKey_TElement__4__0 ??
            (s_ToDictionaryAwaitAsync__TSource_TKey_TElement__4__0 = new Func<IAsyncQueryable<object>, Expression<Func<object, ValueTask<object>>>, Expression<Func<object, ValueTask<object>>>, CancellationToken, ValueTask<Dictionary<object, object>>>(ToDictionaryAwaitAsync<object, object, object>).GetMethodInfo()!.GetGenericMethodDefinition())).MakeGenericMethod(TSource, TKey, TElement);

        public static ValueTask<Dictionary<TKey, TElement>> ToDictionaryAwaitAsync<TSource, TKey, TElement>(this IAsyncQueryable<TSource> source, Expression<Func<TSource, ValueTask<TKey>>> keySelector, Expression<Func<TSource, ValueTask<TElement>>> elementSelector, CancellationToken cancellationToken = default) where TKey : notnull
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));
            if (keySelector == null)
                throw new ArgumentNullException(nameof(keySelector));
            if (elementSelector == null)
                throw new ArgumentNullException(nameof(elementSelector));

            return source.Provider.ExecuteAsync<Dictionary<TKey, TElement>>(Expression.Call(ToDictionaryAwaitAsync__TSource_TKey_TElement__4__0(typeof(TSource), typeof(TKey), typeof(TElement)), source.Expression, keySelector, elementSelector, Expression.Constant(cancellationToken, typeof(CancellationToken))), cancellationToken);
        }

        private static MethodInfo? s_ToDictionaryAwaitAsync__TSource_TKey_TElement__5__0;
        
        private static MethodInfo? ToDictionaryAwaitAsync__TSource_TKey_TElement__5__0(Type TSource, Type TKey, Type TElement) =>
            (s_ToDictionaryAwaitAsync__TSource_TKey_TElement__5__0 ??
            (s_ToDictionaryAwaitAsync__TSource_TKey_TElement__5__0 = new Func<IAsyncQueryable<object>, Expression<Func<object, ValueTask<object>>>, Expression<Func<object, ValueTask<object>>>, IEqualityComparer<object>, CancellationToken, ValueTask<Dictionary<object, object>>>(ToDictionaryAwaitAsync<object, object, object>).GetMethodInfo()!.GetGenericMethodDefinition())).MakeGenericMethod(TSource, TKey, TElement);

        public static ValueTask<Dictionary<TKey, TElement>> ToDictionaryAwaitAsync<TSource, TKey, TElement>(this IAsyncQueryable<TSource> source, Expression<Func<TSource, ValueTask<TKey>>> keySelector, Expression<Func<TSource, ValueTask<TElement>>> elementSelector, IEqualityComparer<TKey>? comparer, CancellationToken cancellationToken = default) where TKey : notnull
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));
            if (keySelector == null)
                throw new ArgumentNullException(nameof(keySelector));
            if (elementSelector == null)
                throw new ArgumentNullException(nameof(elementSelector));

            return source.Provider.ExecuteAsync<Dictionary<TKey, TElement>>(Expression.Call(ToDictionaryAwaitAsync__TSource_TKey_TElement__5__0(typeof(TSource), typeof(TKey), typeof(TElement)), source.Expression, keySelector, elementSelector, Expression.Constant(comparer, typeof(IEqualityComparer<TKey>)), Expression.Constant(cancellationToken, typeof(CancellationToken))), cancellationToken);
        }

        private static MethodInfo? s_ToDictionaryAwaitWithCancellationAsync__TSource_TKey__3__0;
        
        private static MethodInfo? ToDictionaryAwaitWithCancellationAsync__TSource_TKey__3__0(Type TSource, Type TKey) =>
            (s_ToDictionaryAwaitWithCancellationAsync__TSource_TKey__3__0 ??
            (s_ToDictionaryAwaitWithCancellationAsync__TSource_TKey__3__0 = new Func<IAsyncQueryable<object>, Expression<Func<object, CancellationToken, ValueTask<object>>>, CancellationToken, ValueTask<Dictionary<object, object>>>(ToDictionaryAwaitWithCancellationAsync<object, object>).GetMethodInfo()!.GetGenericMethodDefinition())).MakeGenericMethod(TSource, TKey);

        public static ValueTask<Dictionary<TKey, TSource>> ToDictionaryAwaitWithCancellationAsync<TSource, TKey>(this IAsyncQueryable<TSource> source, Expression<Func<TSource, CancellationToken, ValueTask<TKey>>> keySelector, CancellationToken cancellationToken = default) where TKey : notnull
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));
            if (keySelector == null)
                throw new ArgumentNullException(nameof(keySelector));

            return source.Provider.ExecuteAsync<Dictionary<TKey, TSource>>(Expression.Call(ToDictionaryAwaitWithCancellationAsync__TSource_TKey__3__0(typeof(TSource), typeof(TKey)), source.Expression, keySelector, Expression.Constant(cancellationToken, typeof(CancellationToken))), cancellationToken);
        }

        private static MethodInfo? s_ToDictionaryAwaitWithCancellationAsync__TSource_TKey__4__0;
        
        private static MethodInfo? ToDictionaryAwaitWithCancellationAsync__TSource_TKey__4__0(Type TSource, Type TKey) =>
            (s_ToDictionaryAwaitWithCancellationAsync__TSource_TKey__4__0 ??
            (s_ToDictionaryAwaitWithCancellationAsync__TSource_TKey__4__0 = new Func<IAsyncQueryable<object>, Expression<Func<object, CancellationToken, ValueTask<object>>>, IEqualityComparer<object>, CancellationToken, ValueTask<Dictionary<object, object>>>(ToDictionaryAwaitWithCancellationAsync<object, object>).GetMethodInfo()!.GetGenericMethodDefinition())).MakeGenericMethod(TSource, TKey);

        public static ValueTask<Dictionary<TKey, TSource>> ToDictionaryAwaitWithCancellationAsync<TSource, TKey>(this IAsyncQueryable<TSource> source, Expression<Func<TSource, CancellationToken, ValueTask<TKey>>> keySelector, IEqualityComparer<TKey>? comparer, CancellationToken cancellationToken = default) where TKey : notnull
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));
            if (keySelector == null)
                throw new ArgumentNullException(nameof(keySelector));

            return source.Provider.ExecuteAsync<Dictionary<TKey, TSource>>(Expression.Call(ToDictionaryAwaitWithCancellationAsync__TSource_TKey__4__0(typeof(TSource), typeof(TKey)), source.Expression, keySelector, Expression.Constant(comparer, typeof(IEqualityComparer<TKey>)), Expression.Constant(cancellationToken, typeof(CancellationToken))), cancellationToken);
        }

        private static MethodInfo? s_ToDictionaryAwaitWithCancellationAsync__TSource_TKey_TElement__4__0;
        
        private static MethodInfo? ToDictionaryAwaitWithCancellationAsync__TSource_TKey_TElement__4__0(Type TSource, Type TKey, Type TElement) =>
            (s_ToDictionaryAwaitWithCancellationAsync__TSource_TKey_TElement__4__0 ??
            (s_ToDictionaryAwaitWithCancellationAsync__TSource_TKey_TElement__4__0 = new Func<IAsyncQueryable<object>, Expression<Func<object, CancellationToken, ValueTask<object>>>, Expression<Func<object, CancellationToken, ValueTask<object>>>, CancellationToken, ValueTask<Dictionary<object, object>>>(ToDictionaryAwaitWithCancellationAsync<object, object, object>).GetMethodInfo()!.GetGenericMethodDefinition())).MakeGenericMethod(TSource, TKey, TElement);

        public static ValueTask<Dictionary<TKey, TElement>> ToDictionaryAwaitWithCancellationAsync<TSource, TKey, TElement>(this IAsyncQueryable<TSource> source, Expression<Func<TSource, CancellationToken, ValueTask<TKey>>> keySelector, Expression<Func<TSource, CancellationToken, ValueTask<TElement>>> elementSelector, CancellationToken cancellationToken = default) where TKey : notnull
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));
            if (keySelector == null)
                throw new ArgumentNullException(nameof(keySelector));
            if (elementSelector == null)
                throw new ArgumentNullException(nameof(elementSelector));

            return source.Provider.ExecuteAsync<Dictionary<TKey, TElement>>(Expression.Call(ToDictionaryAwaitWithCancellationAsync__TSource_TKey_TElement__4__0(typeof(TSource), typeof(TKey), typeof(TElement)), source.Expression, keySelector, elementSelector, Expression.Constant(cancellationToken, typeof(CancellationToken))), cancellationToken);
        }

        private static MethodInfo? s_ToDictionaryAwaitWithCancellationAsync__TSource_TKey_TElement__5__0;
        
        private static MethodInfo? ToDictionaryAwaitWithCancellationAsync__TSource_TKey_TElement__5__0(Type TSource, Type TKey, Type TElement) =>
            (s_ToDictionaryAwaitWithCancellationAsync__TSource_TKey_TElement__5__0 ??
            (s_ToDictionaryAwaitWithCancellationAsync__TSource_TKey_TElement__5__0 = new Func<IAsyncQueryable<object>, Expression<Func<object, CancellationToken, ValueTask<object>>>, Expression<Func<object, CancellationToken, ValueTask<object>>>, IEqualityComparer<object>, CancellationToken, ValueTask<Dictionary<object, object>>>(ToDictionaryAwaitWithCancellationAsync<object, object, object>).GetMethodInfo()!.GetGenericMethodDefinition())).MakeGenericMethod(TSource, TKey, TElement);

        public static ValueTask<Dictionary<TKey, TElement>> ToDictionaryAwaitWithCancellationAsync<TSource, TKey, TElement>(this IAsyncQueryable<TSource> source, Expression<Func<TSource, CancellationToken, ValueTask<TKey>>> keySelector, Expression<Func<TSource, CancellationToken, ValueTask<TElement>>> elementSelector, IEqualityComparer<TKey>? comparer, CancellationToken cancellationToken = default) where TKey : notnull
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));
            if (keySelector == null)
                throw new ArgumentNullException(nameof(keySelector));
            if (elementSelector == null)
                throw new ArgumentNullException(nameof(elementSelector));

            return source.Provider.ExecuteAsync<Dictionary<TKey, TElement>>(Expression.Call(ToDictionaryAwaitWithCancellationAsync__TSource_TKey_TElement__5__0(typeof(TSource), typeof(TKey), typeof(TElement)), source.Expression, keySelector, elementSelector, Expression.Constant(comparer, typeof(IEqualityComparer<TKey>)), Expression.Constant(cancellationToken, typeof(CancellationToken))), cancellationToken);
        }

        private static MethodInfo? s_ToHashSetAsync__TSource__2__0;
        
        private static MethodInfo? ToHashSetAsync__TSource__2__0(Type TSource) =>
            (s_ToHashSetAsync__TSource__2__0 ??
            (s_ToHashSetAsync__TSource__2__0 = new Func<IAsyncQueryable<object>, CancellationToken, ValueTask<HashSet<object>>>(ToHashSetAsync<object>).GetMethodInfo()!.GetGenericMethodDefinition())).MakeGenericMethod(TSource);

        public static ValueTask<HashSet<TSource>> ToHashSetAsync<TSource>(this IAsyncQueryable<TSource> source, CancellationToken cancellationToken = default)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));

            return source.Provider.ExecuteAsync<HashSet<TSource>>(Expression.Call(ToHashSetAsync__TSource__2__0(typeof(TSource)), source.Expression, Expression.Constant(cancellationToken, typeof(CancellationToken))), cancellationToken);
        }

        private static MethodInfo? s_ToHashSetAsync__TSource__3__0;
        
        private static MethodInfo? ToHashSetAsync__TSource__3__0(Type TSource) =>
            (s_ToHashSetAsync__TSource__3__0 ??
            (s_ToHashSetAsync__TSource__3__0 = new Func<IAsyncQueryable<object>, IEqualityComparer<object>, CancellationToken, ValueTask<HashSet<object>>>(ToHashSetAsync<object>).GetMethodInfo()!.GetGenericMethodDefinition())).MakeGenericMethod(TSource);

        public static ValueTask<HashSet<TSource>> ToHashSetAsync<TSource>(this IAsyncQueryable<TSource> source, IEqualityComparer<TSource>? comparer, CancellationToken cancellationToken = default)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));

            return source.Provider.ExecuteAsync<HashSet<TSource>>(Expression.Call(ToHashSetAsync__TSource__3__0(typeof(TSource)), source.Expression, Expression.Constant(comparer, typeof(IEqualityComparer<TSource>)), Expression.Constant(cancellationToken, typeof(CancellationToken))), cancellationToken);
        }

        private static MethodInfo? s_ToListAsync__TSource__2__0;
        
        private static MethodInfo? ToListAsync__TSource__2__0(Type TSource) =>
            (s_ToListAsync__TSource__2__0 ??
            (s_ToListAsync__TSource__2__0 = new Func<IAsyncQueryable<object>, CancellationToken, ValueTask<List<object>>>(ToListAsync<object>).GetMethodInfo()!.GetGenericMethodDefinition())).MakeGenericMethod(TSource);

        public static ValueTask<List<TSource>> ToListAsync<TSource>(this IAsyncQueryable<TSource> source, CancellationToken cancellationToken = default)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));

            return source.Provider.ExecuteAsync<List<TSource>>(Expression.Call(ToListAsync__TSource__2__0(typeof(TSource)), source.Expression, Expression.Constant(cancellationToken, typeof(CancellationToken))), cancellationToken);
        }

        private static MethodInfo? s_ToLookupAsync__TSource_TKey__3__0;
        
        private static MethodInfo? ToLookupAsync__TSource_TKey__3__0(Type TSource, Type TKey) =>
            (s_ToLookupAsync__TSource_TKey__3__0 ??
            (s_ToLookupAsync__TSource_TKey__3__0 = new Func<IAsyncQueryable<object>, Expression<Func<object, object>>, CancellationToken, ValueTask<ILookup<object, object>>>(ToLookupAsync<object, object>).GetMethodInfo()!.GetGenericMethodDefinition())).MakeGenericMethod(TSource, TKey);

        public static ValueTask<ILookup<TKey, TSource>> ToLookupAsync<TSource, TKey>(this IAsyncQueryable<TSource> source, Expression<Func<TSource, TKey>> keySelector, CancellationToken cancellationToken = default)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));
            if (keySelector == null)
                throw new ArgumentNullException(nameof(keySelector));

            return source.Provider.ExecuteAsync<ILookup<TKey, TSource>>(Expression.Call(ToLookupAsync__TSource_TKey__3__0(typeof(TSource), typeof(TKey)), source.Expression, keySelector, Expression.Constant(cancellationToken, typeof(CancellationToken))), cancellationToken);
        }

        private static MethodInfo? s_ToLookupAsync__TSource_TKey__4__0;
        
        private static MethodInfo? ToLookupAsync__TSource_TKey__4__0(Type TSource, Type TKey) =>
            (s_ToLookupAsync__TSource_TKey__4__0 ??
            (s_ToLookupAsync__TSource_TKey__4__0 = new Func<IAsyncQueryable<object>, Expression<Func<object, object>>, IEqualityComparer<object>, CancellationToken, ValueTask<ILookup<object, object>>>(ToLookupAsync<object, object>).GetMethodInfo()!.GetGenericMethodDefinition())).MakeGenericMethod(TSource, TKey);

        public static ValueTask<ILookup<TKey, TSource>> ToLookupAsync<TSource, TKey>(this IAsyncQueryable<TSource> source, Expression<Func<TSource, TKey>> keySelector, IEqualityComparer<TKey>? comparer, CancellationToken cancellationToken = default)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));
            if (keySelector == null)
                throw new ArgumentNullException(nameof(keySelector));

            return source.Provider.ExecuteAsync<ILookup<TKey, TSource>>(Expression.Call(ToLookupAsync__TSource_TKey__4__0(typeof(TSource), typeof(TKey)), source.Expression, keySelector, Expression.Constant(comparer, typeof(IEqualityComparer<TKey>)), Expression.Constant(cancellationToken, typeof(CancellationToken))), cancellationToken);
        }

        private static MethodInfo? s_ToLookupAsync__TSource_TKey_TElement__4__0;
        
        private static MethodInfo? ToLookupAsync__TSource_TKey_TElement__4__0(Type TSource, Type TKey, Type TElement) =>
            (s_ToLookupAsync__TSource_TKey_TElement__4__0 ??
            (s_ToLookupAsync__TSource_TKey_TElement__4__0 = new Func<IAsyncQueryable<object>, Expression<Func<object, object>>, Expression<Func<object, object>>, CancellationToken, ValueTask<ILookup<object, object>>>(ToLookupAsync<object, object, object>).GetMethodInfo()!.GetGenericMethodDefinition())).MakeGenericMethod(TSource, TKey, TElement);

        public static ValueTask<ILookup<TKey, TElement>> ToLookupAsync<TSource, TKey, TElement>(this IAsyncQueryable<TSource> source, Expression<Func<TSource, TKey>> keySelector, Expression<Func<TSource, TElement>> elementSelector, CancellationToken cancellationToken = default)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));
            if (keySelector == null)
                throw new ArgumentNullException(nameof(keySelector));
            if (elementSelector == null)
                throw new ArgumentNullException(nameof(elementSelector));

            return source.Provider.ExecuteAsync<ILookup<TKey, TElement>>(Expression.Call(ToLookupAsync__TSource_TKey_TElement__4__0(typeof(TSource), typeof(TKey), typeof(TElement)), source.Expression, keySelector, elementSelector, Expression.Constant(cancellationToken, typeof(CancellationToken))), cancellationToken);
        }

        private static MethodInfo? s_ToLookupAsync__TSource_TKey_TElement__5__0;
        
        private static MethodInfo? ToLookupAsync__TSource_TKey_TElement__5__0(Type TSource, Type TKey, Type TElement) =>
            (s_ToLookupAsync__TSource_TKey_TElement__5__0 ??
            (s_ToLookupAsync__TSource_TKey_TElement__5__0 = new Func<IAsyncQueryable<object>, Expression<Func<object, object>>, Expression<Func<object, object>>, IEqualityComparer<object>, CancellationToken, ValueTask<ILookup<object, object>>>(ToLookupAsync<object, object, object>).GetMethodInfo()!.GetGenericMethodDefinition())).MakeGenericMethod(TSource, TKey, TElement);

        public static ValueTask<ILookup<TKey, TElement>> ToLookupAsync<TSource, TKey, TElement>(this IAsyncQueryable<TSource> source, Expression<Func<TSource, TKey>> keySelector, Expression<Func<TSource, TElement>> elementSelector, IEqualityComparer<TKey>? comparer, CancellationToken cancellationToken = default)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));
            if (keySelector == null)
                throw new ArgumentNullException(nameof(keySelector));
            if (elementSelector == null)
                throw new ArgumentNullException(nameof(elementSelector));

            return source.Provider.ExecuteAsync<ILookup<TKey, TElement>>(Expression.Call(ToLookupAsync__TSource_TKey_TElement__5__0(typeof(TSource), typeof(TKey), typeof(TElement)), source.Expression, keySelector, elementSelector, Expression.Constant(comparer, typeof(IEqualityComparer<TKey>)), Expression.Constant(cancellationToken, typeof(CancellationToken))), cancellationToken);
        }

        private static MethodInfo? s_ToLookupAwaitAsync__TSource_TKey__3__0;
        
        private static MethodInfo? ToLookupAwaitAsync__TSource_TKey__3__0(Type TSource, Type TKey) =>
            (s_ToLookupAwaitAsync__TSource_TKey__3__0 ??
            (s_ToLookupAwaitAsync__TSource_TKey__3__0 = new Func<IAsyncQueryable<object>, Expression<Func<object, ValueTask<object>>>, CancellationToken, ValueTask<ILookup<object, object>>>(ToLookupAwaitAsync<object, object>).GetMethodInfo()!.GetGenericMethodDefinition())).MakeGenericMethod(TSource, TKey);

        public static ValueTask<ILookup<TKey, TSource>> ToLookupAwaitAsync<TSource, TKey>(this IAsyncQueryable<TSource> source, Expression<Func<TSource, ValueTask<TKey>>> keySelector, CancellationToken cancellationToken = default)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));
            if (keySelector == null)
                throw new ArgumentNullException(nameof(keySelector));

            return source.Provider.ExecuteAsync<ILookup<TKey, TSource>>(Expression.Call(ToLookupAwaitAsync__TSource_TKey__3__0(typeof(TSource), typeof(TKey)), source.Expression, keySelector, Expression.Constant(cancellationToken, typeof(CancellationToken))), cancellationToken);
        }

        private static MethodInfo? s_ToLookupAwaitAsync__TSource_TKey__4__0;
        
        private static MethodInfo? ToLookupAwaitAsync__TSource_TKey__4__0(Type TSource, Type TKey) =>
            (s_ToLookupAwaitAsync__TSource_TKey__4__0 ??
            (s_ToLookupAwaitAsync__TSource_TKey__4__0 = new Func<IAsyncQueryable<object>, Expression<Func<object, ValueTask<object>>>, IEqualityComparer<object>, CancellationToken, ValueTask<ILookup<object, object>>>(ToLookupAwaitAsync<object, object>).GetMethodInfo()!.GetGenericMethodDefinition())).MakeGenericMethod(TSource, TKey);

        public static ValueTask<ILookup<TKey, TSource>> ToLookupAwaitAsync<TSource, TKey>(this IAsyncQueryable<TSource> source, Expression<Func<TSource, ValueTask<TKey>>> keySelector, IEqualityComparer<TKey>? comparer, CancellationToken cancellationToken = default)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));
            if (keySelector == null)
                throw new ArgumentNullException(nameof(keySelector));

            return source.Provider.ExecuteAsync<ILookup<TKey, TSource>>(Expression.Call(ToLookupAwaitAsync__TSource_TKey__4__0(typeof(TSource), typeof(TKey)), source.Expression, keySelector, Expression.Constant(comparer, typeof(IEqualityComparer<TKey>)), Expression.Constant(cancellationToken, typeof(CancellationToken))), cancellationToken);
        }

        private static MethodInfo? s_ToLookupAwaitAsync__TSource_TKey_TElement__4__0;
        
        private static MethodInfo? ToLookupAwaitAsync__TSource_TKey_TElement__4__0(Type TSource, Type TKey, Type TElement) =>
            (s_ToLookupAwaitAsync__TSource_TKey_TElement__4__0 ??
            (s_ToLookupAwaitAsync__TSource_TKey_TElement__4__0 = new Func<IAsyncQueryable<object>, Expression<Func<object, ValueTask<object>>>, Expression<Func<object, ValueTask<object>>>, CancellationToken, ValueTask<ILookup<object, object>>>(ToLookupAwaitAsync<object, object, object>).GetMethodInfo()!.GetGenericMethodDefinition())).MakeGenericMethod(TSource, TKey, TElement);

        public static ValueTask<ILookup<TKey, TElement>> ToLookupAwaitAsync<TSource, TKey, TElement>(this IAsyncQueryable<TSource> source, Expression<Func<TSource, ValueTask<TKey>>> keySelector, Expression<Func<TSource, ValueTask<TElement>>> elementSelector, CancellationToken cancellationToken = default)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));
            if (keySelector == null)
                throw new ArgumentNullException(nameof(keySelector));
            if (elementSelector == null)
                throw new ArgumentNullException(nameof(elementSelector));

            return source.Provider.ExecuteAsync<ILookup<TKey, TElement>>(Expression.Call(ToLookupAwaitAsync__TSource_TKey_TElement__4__0(typeof(TSource), typeof(TKey), typeof(TElement)), source.Expression, keySelector, elementSelector, Expression.Constant(cancellationToken, typeof(CancellationToken))), cancellationToken);
        }

        private static MethodInfo? s_ToLookupAwaitAsync__TSource_TKey_TElement__5__0;
        
        private static MethodInfo? ToLookupAwaitAsync__TSource_TKey_TElement__5__0(Type TSource, Type TKey, Type TElement) =>
            (s_ToLookupAwaitAsync__TSource_TKey_TElement__5__0 ??
            (s_ToLookupAwaitAsync__TSource_TKey_TElement__5__0 = new Func<IAsyncQueryable<object>, Expression<Func<object, ValueTask<object>>>, Expression<Func<object, ValueTask<object>>>, IEqualityComparer<object>, CancellationToken, ValueTask<ILookup<object, object>>>(ToLookupAwaitAsync<object, object, object>).GetMethodInfo()!.GetGenericMethodDefinition())).MakeGenericMethod(TSource, TKey, TElement);

        public static ValueTask<ILookup<TKey, TElement>> ToLookupAwaitAsync<TSource, TKey, TElement>(this IAsyncQueryable<TSource> source, Expression<Func<TSource, ValueTask<TKey>>> keySelector, Expression<Func<TSource, ValueTask<TElement>>> elementSelector, IEqualityComparer<TKey>? comparer, CancellationToken cancellationToken = default)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));
            if (keySelector == null)
                throw new ArgumentNullException(nameof(keySelector));
            if (elementSelector == null)
                throw new ArgumentNullException(nameof(elementSelector));

            return source.Provider.ExecuteAsync<ILookup<TKey, TElement>>(Expression.Call(ToLookupAwaitAsync__TSource_TKey_TElement__5__0(typeof(TSource), typeof(TKey), typeof(TElement)), source.Expression, keySelector, elementSelector, Expression.Constant(comparer, typeof(IEqualityComparer<TKey>)), Expression.Constant(cancellationToken, typeof(CancellationToken))), cancellationToken);
        }

        private static MethodInfo? s_ToLookupAwaitWithCancellationAsync__TSource_TKey__3__0;
        
        private static MethodInfo? ToLookupAwaitWithCancellationAsync__TSource_TKey__3__0(Type TSource, Type TKey) =>
            (s_ToLookupAwaitWithCancellationAsync__TSource_TKey__3__0 ??
            (s_ToLookupAwaitWithCancellationAsync__TSource_TKey__3__0 = new Func<IAsyncQueryable<object>, Expression<Func<object, CancellationToken, ValueTask<object>>>, CancellationToken, ValueTask<ILookup<object, object>>>(ToLookupAwaitWithCancellationAsync<object, object>).GetMethodInfo()!.GetGenericMethodDefinition())).MakeGenericMethod(TSource, TKey);

        public static ValueTask<ILookup<TKey, TSource>> ToLookupAwaitWithCancellationAsync<TSource, TKey>(this IAsyncQueryable<TSource> source, Expression<Func<TSource, CancellationToken, ValueTask<TKey>>> keySelector, CancellationToken cancellationToken = default)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));
            if (keySelector == null)
                throw new ArgumentNullException(nameof(keySelector));

            return source.Provider.ExecuteAsync<ILookup<TKey, TSource>>(Expression.Call(ToLookupAwaitWithCancellationAsync__TSource_TKey__3__0(typeof(TSource), typeof(TKey)), source.Expression, keySelector, Expression.Constant(cancellationToken, typeof(CancellationToken))), cancellationToken);
        }

        private static MethodInfo? s_ToLookupAwaitWithCancellationAsync__TSource_TKey__4__0;
        
        private static MethodInfo? ToLookupAwaitWithCancellationAsync__TSource_TKey__4__0(Type TSource, Type TKey) =>
            (s_ToLookupAwaitWithCancellationAsync__TSource_TKey__4__0 ??
            (s_ToLookupAwaitWithCancellationAsync__TSource_TKey__4__0 = new Func<IAsyncQueryable<object>, Expression<Func<object, CancellationToken, ValueTask<object>>>, IEqualityComparer<object>, CancellationToken, ValueTask<ILookup<object, object>>>(ToLookupAwaitWithCancellationAsync<object, object>).GetMethodInfo()!.GetGenericMethodDefinition())).MakeGenericMethod(TSource, TKey);

        public static ValueTask<ILookup<TKey, TSource>> ToLookupAwaitWithCancellationAsync<TSource, TKey>(this IAsyncQueryable<TSource> source, Expression<Func<TSource, CancellationToken, ValueTask<TKey>>> keySelector, IEqualityComparer<TKey>? comparer, CancellationToken cancellationToken = default)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));
            if (keySelector == null)
                throw new ArgumentNullException(nameof(keySelector));

            return source.Provider.ExecuteAsync<ILookup<TKey, TSource>>(Expression.Call(ToLookupAwaitWithCancellationAsync__TSource_TKey__4__0(typeof(TSource), typeof(TKey)), source.Expression, keySelector, Expression.Constant(comparer, typeof(IEqualityComparer<TKey>)), Expression.Constant(cancellationToken, typeof(CancellationToken))), cancellationToken);
        }

        private static MethodInfo? s_ToLookupAwaitWithCancellationAsync__TSource_TKey_TElement__4__0;
        
        private static MethodInfo? ToLookupAwaitWithCancellationAsync__TSource_TKey_TElement__4__0(Type TSource, Type TKey, Type TElement) =>
            (s_ToLookupAwaitWithCancellationAsync__TSource_TKey_TElement__4__0 ??
            (s_ToLookupAwaitWithCancellationAsync__TSource_TKey_TElement__4__0 = new Func<IAsyncQueryable<object>, Expression<Func<object, CancellationToken, ValueTask<object>>>, Expression<Func<object, CancellationToken, ValueTask<object>>>, CancellationToken, ValueTask<ILookup<object, object>>>(ToLookupAwaitWithCancellationAsync<object, object, object>).GetMethodInfo()!.GetGenericMethodDefinition())).MakeGenericMethod(TSource, TKey, TElement);

        public static ValueTask<ILookup<TKey, TElement>> ToLookupAwaitWithCancellationAsync<TSource, TKey, TElement>(this IAsyncQueryable<TSource> source, Expression<Func<TSource, CancellationToken, ValueTask<TKey>>> keySelector, Expression<Func<TSource, CancellationToken, ValueTask<TElement>>> elementSelector, CancellationToken cancellationToken = default)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));
            if (keySelector == null)
                throw new ArgumentNullException(nameof(keySelector));
            if (elementSelector == null)
                throw new ArgumentNullException(nameof(elementSelector));

            return source.Provider.ExecuteAsync<ILookup<TKey, TElement>>(Expression.Call(ToLookupAwaitWithCancellationAsync__TSource_TKey_TElement__4__0(typeof(TSource), typeof(TKey), typeof(TElement)), source.Expression, keySelector, elementSelector, Expression.Constant(cancellationToken, typeof(CancellationToken))), cancellationToken);
        }

        private static MethodInfo? s_ToLookupAwaitWithCancellationAsync__TSource_TKey_TElement__5__0;
        
        private static MethodInfo? ToLookupAwaitWithCancellationAsync__TSource_TKey_TElement__5__0(Type TSource, Type TKey, Type TElement) =>
            (s_ToLookupAwaitWithCancellationAsync__TSource_TKey_TElement__5__0 ??
            (s_ToLookupAwaitWithCancellationAsync__TSource_TKey_TElement__5__0 = new Func<IAsyncQueryable<object>, Expression<Func<object, CancellationToken, ValueTask<object>>>, Expression<Func<object, CancellationToken, ValueTask<object>>>, IEqualityComparer<object>, CancellationToken, ValueTask<ILookup<object, object>>>(ToLookupAwaitWithCancellationAsync<object, object, object>).GetMethodInfo()!.GetGenericMethodDefinition())).MakeGenericMethod(TSource, TKey, TElement);

        public static ValueTask<ILookup<TKey, TElement>> ToLookupAwaitWithCancellationAsync<TSource, TKey, TElement>(this IAsyncQueryable<TSource> source, Expression<Func<TSource, CancellationToken, ValueTask<TKey>>> keySelector, Expression<Func<TSource, CancellationToken, ValueTask<TElement>>> elementSelector, IEqualityComparer<TKey>? comparer, CancellationToken cancellationToken = default)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));
            if (keySelector == null)
                throw new ArgumentNullException(nameof(keySelector));
            if (elementSelector == null)
                throw new ArgumentNullException(nameof(elementSelector));

            return source.Provider.ExecuteAsync<ILookup<TKey, TElement>>(Expression.Call(ToLookupAwaitWithCancellationAsync__TSource_TKey_TElement__5__0(typeof(TSource), typeof(TKey), typeof(TElement)), source.Expression, keySelector, elementSelector, Expression.Constant(comparer, typeof(IEqualityComparer<TKey>)), Expression.Constant(cancellationToken, typeof(CancellationToken))), cancellationToken);
        }

        private static MethodInfo? s_Union__TSource__2__0;
        
        private static MethodInfo? Union__TSource__2__0(Type TSource) =>
            (s_Union__TSource__2__0 ??
            (s_Union__TSource__2__0 = new Func<IAsyncQueryable<object>, IAsyncEnumerable<object>, IAsyncQueryable<object>>(Union<object>).GetMethodInfo()!.GetGenericMethodDefinition())).MakeGenericMethod(TSource);

        public static IAsyncQueryable<TSource> Union<TSource>(this IAsyncQueryable<TSource> first, IAsyncEnumerable<TSource> second)
        {
            if (first == null)
                throw new ArgumentNullException(nameof(first));
            if (second == null)
                throw new ArgumentNullException(nameof(second));

            return first.Provider.CreateQuery<TSource>(Expression.Call(Union__TSource__2__0(typeof(TSource)), first.Expression, GetSourceExpression(second)));
        }

        private static MethodInfo? s_Union__TSource__3__0;
        
        private static MethodInfo? Union__TSource__3__0(Type TSource) =>
            (s_Union__TSource__3__0 ??
            (s_Union__TSource__3__0 = new Func<IAsyncQueryable<object>, IAsyncEnumerable<object>, IEqualityComparer<object>, IAsyncQueryable<object>>(Union<object>).GetMethodInfo()!.GetGenericMethodDefinition())).MakeGenericMethod(TSource);

        public static IAsyncQueryable<TSource> Union<TSource>(this IAsyncQueryable<TSource> first, IAsyncEnumerable<TSource> second, IEqualityComparer<TSource>? comparer)
        {
            if (first == null)
                throw new ArgumentNullException(nameof(first));
            if (second == null)
                throw new ArgumentNullException(nameof(second));

            return first.Provider.CreateQuery<TSource>(Expression.Call(Union__TSource__3__0(typeof(TSource)), first.Expression, GetSourceExpression(second), Expression.Constant(comparer, typeof(IEqualityComparer<TSource>))));
        }

        private static MethodInfo? s_Where__TSource__2__0;
        
        private static MethodInfo? Where__TSource__2__0(Type TSource) =>
            (s_Where__TSource__2__0 ??
            (s_Where__TSource__2__0 = new Func<IAsyncQueryable<object>, Expression<Func<object, bool>>, IAsyncQueryable<object>>(Where<object>).GetMethodInfo()!.GetGenericMethodDefinition())).MakeGenericMethod(TSource);

        public static IAsyncQueryable<TSource> Where<TSource>(this IAsyncQueryable<TSource> source, Expression<Func<TSource, bool>> predicate)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));
            if (predicate == null)
                throw new ArgumentNullException(nameof(predicate));

            return source.Provider.CreateQuery<TSource>(Expression.Call(Where__TSource__2__0(typeof(TSource)), source.Expression, predicate));
        }

        private static MethodInfo? s_Where__TSource__2__1;
        
        private static MethodInfo? Where__TSource__2__1(Type TSource) =>
            (s_Where__TSource__2__1 ??
            (s_Where__TSource__2__1 = new Func<IAsyncQueryable<object>, Expression<Func<object, int, bool>>, IAsyncQueryable<object>>(Where<object>).GetMethodInfo()!.GetGenericMethodDefinition())).MakeGenericMethod(TSource);

        public static IAsyncQueryable<TSource> Where<TSource>(this IAsyncQueryable<TSource> source, Expression<Func<TSource, int, bool>> predicate)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));
            if (predicate == null)
                throw new ArgumentNullException(nameof(predicate));

            return source.Provider.CreateQuery<TSource>(Expression.Call(Where__TSource__2__1(typeof(TSource)), source.Expression, predicate));
        }

        private static MethodInfo? s_WhereAwait__TSource__2__0;
        
        private static MethodInfo? WhereAwait__TSource__2__0(Type TSource) =>
            (s_WhereAwait__TSource__2__0 ??
            (s_WhereAwait__TSource__2__0 = new Func<IAsyncQueryable<object>, Expression<Func<object, int, ValueTask<bool>>>, IAsyncQueryable<object>>(WhereAwait<object>).GetMethodInfo()!.GetGenericMethodDefinition())).MakeGenericMethod(TSource);

        public static IAsyncQueryable<TSource> WhereAwait<TSource>(this IAsyncQueryable<TSource> source, Expression<Func<TSource, int, ValueTask<bool>>> predicate)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));
            if (predicate == null)
                throw new ArgumentNullException(nameof(predicate));

            return source.Provider.CreateQuery<TSource>(Expression.Call(WhereAwait__TSource__2__0(typeof(TSource)), source.Expression, predicate));
        }

        private static MethodInfo? s_WhereAwait__TSource__2__1;
        
        private static MethodInfo? WhereAwait__TSource__2__1(Type TSource) =>
            (s_WhereAwait__TSource__2__1 ??
            (s_WhereAwait__TSource__2__1 = new Func<IAsyncQueryable<object>, Expression<Func<object, ValueTask<bool>>>, IAsyncQueryable<object>>(WhereAwait<object>).GetMethodInfo()!.GetGenericMethodDefinition())).MakeGenericMethod(TSource);

        public static IAsyncQueryable<TSource> WhereAwait<TSource>(this IAsyncQueryable<TSource> source, Expression<Func<TSource, ValueTask<bool>>> predicate)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));
            if (predicate == null)
                throw new ArgumentNullException(nameof(predicate));

            return source.Provider.CreateQuery<TSource>(Expression.Call(WhereAwait__TSource__2__1(typeof(TSource)), source.Expression, predicate));
        }

        private static MethodInfo? s_WhereAwaitWithCancellation__TSource__2__0;
        
        private static MethodInfo? WhereAwaitWithCancellation__TSource__2__0(Type TSource) =>
            (s_WhereAwaitWithCancellation__TSource__2__0 ??
            (s_WhereAwaitWithCancellation__TSource__2__0 = new Func<IAsyncQueryable<object>, Expression<Func<object, CancellationToken, ValueTask<bool>>>, IAsyncQueryable<object>>(WhereAwaitWithCancellation<object>).GetMethodInfo()!.GetGenericMethodDefinition())).MakeGenericMethod(TSource);

        public static IAsyncQueryable<TSource> WhereAwaitWithCancellation<TSource>(this IAsyncQueryable<TSource> source, Expression<Func<TSource, CancellationToken, ValueTask<bool>>> predicate)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));
            if (predicate == null)
                throw new ArgumentNullException(nameof(predicate));

            return source.Provider.CreateQuery<TSource>(Expression.Call(WhereAwaitWithCancellation__TSource__2__0(typeof(TSource)), source.Expression, predicate));
        }

        private static MethodInfo? s_WhereAwaitWithCancellation__TSource__2__1;
        
        private static MethodInfo? WhereAwaitWithCancellation__TSource__2__1(Type TSource) =>
            (s_WhereAwaitWithCancellation__TSource__2__1 ??
            (s_WhereAwaitWithCancellation__TSource__2__1 = new Func<IAsyncQueryable<object>, Expression<Func<object, int, CancellationToken, ValueTask<bool>>>, IAsyncQueryable<object>>(WhereAwaitWithCancellation<object>).GetMethodInfo()!.GetGenericMethodDefinition())).MakeGenericMethod(TSource);

        public static IAsyncQueryable<TSource> WhereAwaitWithCancellation<TSource>(this IAsyncQueryable<TSource> source, Expression<Func<TSource, int, CancellationToken, ValueTask<bool>>> predicate)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));
            if (predicate == null)
                throw new ArgumentNullException(nameof(predicate));

            return source.Provider.CreateQuery<TSource>(Expression.Call(WhereAwaitWithCancellation__TSource__2__1(typeof(TSource)), source.Expression, predicate));
        }

        private static MethodInfo? s_Zip__TFirst_TSecond_TResult__3__0;
        
        private static MethodInfo? Zip__TFirst_TSecond_TResult__3__0(Type TFirst, Type TSecond, Type TResult) =>
            (s_Zip__TFirst_TSecond_TResult__3__0 ??
            (s_Zip__TFirst_TSecond_TResult__3__0 = new Func<IAsyncQueryable<object>, IAsyncEnumerable<object>, Expression<Func<object, object, object>>, IAsyncQueryable<object>>(Zip<object, object, object>).GetMethodInfo()!.GetGenericMethodDefinition())).MakeGenericMethod(TFirst, TSecond, TResult);

        public static IAsyncQueryable<TResult> Zip<TFirst, TSecond, TResult>(this IAsyncQueryable<TFirst> first, IAsyncEnumerable<TSecond> second, Expression<Func<TFirst, TSecond, TResult>> selector)
        {
            if (first == null)
                throw new ArgumentNullException(nameof(first));
            if (second == null)
                throw new ArgumentNullException(nameof(second));
            if (selector == null)
                throw new ArgumentNullException(nameof(selector));

            return first.Provider.CreateQuery<TResult>(Expression.Call(Zip__TFirst_TSecond_TResult__3__0(typeof(TFirst), typeof(TSecond), typeof(TResult)), first.Expression, GetSourceExpression(second), selector));
        }

        private static MethodInfo? s_ZipAwait__TFirst_TSecond_TResult__3__0;
        
        private static MethodInfo? ZipAwait__TFirst_TSecond_TResult__3__0(Type TFirst, Type TSecond, Type TResult) =>
            (s_ZipAwait__TFirst_TSecond_TResult__3__0 ??
            (s_ZipAwait__TFirst_TSecond_TResult__3__0 = new Func<IAsyncQueryable<object>, IAsyncEnumerable<object>, Expression<Func<object, object, ValueTask<object>>>, IAsyncQueryable<object>>(ZipAwait<object, object, object>).GetMethodInfo()!.GetGenericMethodDefinition())).MakeGenericMethod(TFirst, TSecond, TResult);

        public static IAsyncQueryable<TResult> ZipAwait<TFirst, TSecond, TResult>(this IAsyncQueryable<TFirst> first, IAsyncEnumerable<TSecond> second, Expression<Func<TFirst, TSecond, ValueTask<TResult>>> selector)
        {
            if (first == null)
                throw new ArgumentNullException(nameof(first));
            if (second == null)
                throw new ArgumentNullException(nameof(second));
            if (selector == null)
                throw new ArgumentNullException(nameof(selector));

            return first.Provider.CreateQuery<TResult>(Expression.Call(ZipAwait__TFirst_TSecond_TResult__3__0(typeof(TFirst), typeof(TSecond), typeof(TResult)), first.Expression, GetSourceExpression(second), selector));
        }

        private static MethodInfo? s_ZipAwaitWithCancellation__TFirst_TSecond_TResult__3__0;
        
        private static MethodInfo? ZipAwaitWithCancellation__TFirst_TSecond_TResult__3__0(Type TFirst, Type TSecond, Type TResult) =>
            (s_ZipAwaitWithCancellation__TFirst_TSecond_TResult__3__0 ??
            (s_ZipAwaitWithCancellation__TFirst_TSecond_TResult__3__0 = new Func<IAsyncQueryable<object>, IAsyncEnumerable<object>, Expression<Func<object, object, CancellationToken, ValueTask<object>>>, IAsyncQueryable<object>>(ZipAwaitWithCancellation<object, object, object>).GetMethodInfo()!.GetGenericMethodDefinition())).MakeGenericMethod(TFirst, TSecond, TResult);

        public static IAsyncQueryable<TResult> ZipAwaitWithCancellation<TFirst, TSecond, TResult>(this IAsyncQueryable<TFirst> first, IAsyncEnumerable<TSecond> second, Expression<Func<TFirst, TSecond, CancellationToken, ValueTask<TResult>>> selector)
        {
            if (first == null)
                throw new ArgumentNullException(nameof(first));
            if (second == null)
                throw new ArgumentNullException(nameof(second));
            if (selector == null)
                throw new ArgumentNullException(nameof(selector));

            return first.Provider.CreateQuery<TResult>(Expression.Call(ZipAwaitWithCancellation__TFirst_TSecond_TResult__3__0(typeof(TFirst), typeof(TSecond), typeof(TResult)), first.Expression, GetSourceExpression(second), selector));
        }

    }
}

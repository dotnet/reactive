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
    public static partial class AsyncQueryableEx
    {
        private static MethodInfo? s_Amb__TSource__2__0;
        
        private static MethodInfo? Amb__TSource__2__0(Type TSource) =>
            (s_Amb__TSource__2__0 ??
            (s_Amb__TSource__2__0 = new Func<IAsyncQueryable<object>, IAsyncEnumerable<object>, IAsyncQueryable<object>>(Amb<object>).GetMethodInfo()!.GetGenericMethodDefinition())).MakeGenericMethod(TSource);

        public static IAsyncQueryable<TSource> Amb<TSource>(this IAsyncQueryable<TSource> first, IAsyncEnumerable<TSource> second)
        {
            if (first == null)
                throw new ArgumentNullException(nameof(first));
            if (second == null)
                throw new ArgumentNullException(nameof(second));

            return first.Provider.CreateQuery<TSource>(Expression.Call(Amb__TSource__2__0(typeof(TSource)), first.Expression, GetSourceExpression(second)));
        }

        private static MethodInfo? s_Buffer__TSource__2__0;
        
        private static MethodInfo? Buffer__TSource__2__0(Type TSource) =>
            (s_Buffer__TSource__2__0 ??
            (s_Buffer__TSource__2__0 = new Func<IAsyncQueryable<object>, int, IAsyncQueryable<IList<object>>>(Buffer<object>).GetMethodInfo()!.GetGenericMethodDefinition())).MakeGenericMethod(TSource);

        public static IAsyncQueryable<IList<TSource>> Buffer<TSource>(this IAsyncQueryable<TSource> source, int count)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));

            return source.Provider.CreateQuery<IList<TSource>>(Expression.Call(Buffer__TSource__2__0(typeof(TSource)), source.Expression, Expression.Constant(count, typeof(int))));
        }

        private static MethodInfo? s_Buffer__TSource__3__0;
        
        private static MethodInfo? Buffer__TSource__3__0(Type TSource) =>
            (s_Buffer__TSource__3__0 ??
            (s_Buffer__TSource__3__0 = new Func<IAsyncQueryable<object>, int, int, IAsyncQueryable<IList<object>>>(Buffer<object>).GetMethodInfo()!.GetGenericMethodDefinition())).MakeGenericMethod(TSource);

        public static IAsyncQueryable<IList<TSource>> Buffer<TSource>(this IAsyncQueryable<TSource> source, int count, int skip)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));

            return source.Provider.CreateQuery<IList<TSource>>(Expression.Call(Buffer__TSource__3__0(typeof(TSource)), source.Expression, Expression.Constant(count, typeof(int)), Expression.Constant(skip, typeof(int))));
        }

        private static MethodInfo? s_Catch__TSource__2__0;
        
        private static MethodInfo? Catch__TSource__2__0(Type TSource) =>
            (s_Catch__TSource__2__0 ??
            (s_Catch__TSource__2__0 = new Func<IAsyncQueryable<object>, IAsyncEnumerable<object>, IAsyncQueryable<object>>(Catch<object>).GetMethodInfo()!.GetGenericMethodDefinition())).MakeGenericMethod(TSource);

        public static IAsyncQueryable<TSource> Catch<TSource>(this IAsyncQueryable<TSource> first, IAsyncEnumerable<TSource> second)
        {
            if (first == null)
                throw new ArgumentNullException(nameof(first));
            if (second == null)
                throw new ArgumentNullException(nameof(second));

            return first.Provider.CreateQuery<TSource>(Expression.Call(Catch__TSource__2__0(typeof(TSource)), first.Expression, GetSourceExpression(second)));
        }

        private static MethodInfo? s_Catch__TSource_TException__2__0;
        
        private static MethodInfo? Catch__TSource_TException__2__0(Type TSource, Type TException) =>
            (s_Catch__TSource_TException__2__0 ??
            (s_Catch__TSource_TException__2__0 = new Func<IAsyncQueryable<object>, Expression<Func<object, CancellationToken, ValueTask<IAsyncEnumerable<object>>>>, IAsyncQueryable<object>>(Catch<object, object>).GetMethodInfo()!.GetGenericMethodDefinition())).MakeGenericMethod(TSource, TException);

        public static IAsyncQueryable<TSource> Catch<TSource, TException>(this IAsyncQueryable<TSource> source, Expression<Func<TException, CancellationToken, ValueTask<IAsyncEnumerable<TSource>>>> handler)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));
            if (handler == null)
                throw new ArgumentNullException(nameof(handler));

            return source.Provider.CreateQuery<TSource>(Expression.Call(Catch__TSource_TException__2__0(typeof(TSource), typeof(TException)), source.Expression, handler));
        }

        private static MethodInfo? s_Catch__TSource_TException__2__1;
        
        private static MethodInfo? Catch__TSource_TException__2__1(Type TSource, Type TException) =>
            (s_Catch__TSource_TException__2__1 ??
            (s_Catch__TSource_TException__2__1 = new Func<IAsyncQueryable<object>, Expression<Func<object, IAsyncEnumerable<object>>>, IAsyncQueryable<object>>(Catch<object, object>).GetMethodInfo()!.GetGenericMethodDefinition())).MakeGenericMethod(TSource, TException);

        public static IAsyncQueryable<TSource> Catch<TSource, TException>(this IAsyncQueryable<TSource> source, Expression<Func<TException, IAsyncEnumerable<TSource>>> handler)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));
            if (handler == null)
                throw new ArgumentNullException(nameof(handler));

            return source.Provider.CreateQuery<TSource>(Expression.Call(Catch__TSource_TException__2__1(typeof(TSource), typeof(TException)), source.Expression, handler));
        }

        private static MethodInfo? s_Catch__TSource_TException__2__2;
        
        private static MethodInfo? Catch__TSource_TException__2__2(Type TSource, Type TException) =>
            (s_Catch__TSource_TException__2__2 ??
            (s_Catch__TSource_TException__2__2 = new Func<IAsyncQueryable<object>, Expression<Func<object, ValueTask<IAsyncEnumerable<object>>>>, IAsyncQueryable<object>>(Catch<object, object>).GetMethodInfo()!.GetGenericMethodDefinition())).MakeGenericMethod(TSource, TException);

        public static IAsyncQueryable<TSource> Catch<TSource, TException>(this IAsyncQueryable<TSource> source, Expression<Func<TException, ValueTask<IAsyncEnumerable<TSource>>>> handler)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));
            if (handler == null)
                throw new ArgumentNullException(nameof(handler));

            return source.Provider.CreateQuery<TSource>(Expression.Call(Catch__TSource_TException__2__2(typeof(TSource), typeof(TException)), source.Expression, handler));
        }

        private static MethodInfo? s_Concat__TSource__1__0;
        
        private static MethodInfo? Concat__TSource__1__0(Type TSource) =>
            (s_Concat__TSource__1__0 ??
            (s_Concat__TSource__1__0 = new Func<IAsyncQueryable<IAsyncEnumerable<object>>, IAsyncQueryable<object>>(Concat<object>).GetMethodInfo()!.GetGenericMethodDefinition())).MakeGenericMethod(TSource);

        public static IAsyncQueryable<TSource> Concat<TSource>(this IAsyncQueryable<IAsyncEnumerable<TSource>> sources)
        {
            if (sources == null)
                throw new ArgumentNullException(nameof(sources));

            return sources.Provider.CreateQuery<TSource>(Expression.Call(Concat__TSource__1__0(typeof(TSource)), sources.Expression));
        }

        private static MethodInfo? s_Distinct__TSource_TKey__2__0;
        
        private static MethodInfo? Distinct__TSource_TKey__2__0(Type TSource, Type TKey) =>
            (s_Distinct__TSource_TKey__2__0 ??
            (s_Distinct__TSource_TKey__2__0 = new Func<IAsyncQueryable<object>, Expression<Func<object, CancellationToken, ValueTask<object>>>, IAsyncQueryable<object>>(Distinct<object, object>).GetMethodInfo()!.GetGenericMethodDefinition())).MakeGenericMethod(TSource, TKey);

        public static IAsyncQueryable<TSource> Distinct<TSource, TKey>(this IAsyncQueryable<TSource> source, Expression<Func<TSource, CancellationToken, ValueTask<TKey>>> keySelector)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));
            if (keySelector == null)
                throw new ArgumentNullException(nameof(keySelector));

            return source.Provider.CreateQuery<TSource>(Expression.Call(Distinct__TSource_TKey__2__0(typeof(TSource), typeof(TKey)), source.Expression, keySelector));
        }

        private static MethodInfo? s_Distinct__TSource_TKey__2__1;
        
        private static MethodInfo? Distinct__TSource_TKey__2__1(Type TSource, Type TKey) =>
            (s_Distinct__TSource_TKey__2__1 ??
            (s_Distinct__TSource_TKey__2__1 = new Func<IAsyncQueryable<object>, Expression<Func<object, object>>, IAsyncQueryable<object>>(Distinct<object, object>).GetMethodInfo()!.GetGenericMethodDefinition())).MakeGenericMethod(TSource, TKey);

        public static IAsyncQueryable<TSource> Distinct<TSource, TKey>(this IAsyncQueryable<TSource> source, Expression<Func<TSource, TKey>> keySelector)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));
            if (keySelector == null)
                throw new ArgumentNullException(nameof(keySelector));

            return source.Provider.CreateQuery<TSource>(Expression.Call(Distinct__TSource_TKey__2__1(typeof(TSource), typeof(TKey)), source.Expression, keySelector));
        }

        private static MethodInfo? s_Distinct__TSource_TKey__2__2;
        
        private static MethodInfo? Distinct__TSource_TKey__2__2(Type TSource, Type TKey) =>
            (s_Distinct__TSource_TKey__2__2 ??
            (s_Distinct__TSource_TKey__2__2 = new Func<IAsyncQueryable<object>, Expression<Func<object, ValueTask<object>>>, IAsyncQueryable<object>>(Distinct<object, object>).GetMethodInfo()!.GetGenericMethodDefinition())).MakeGenericMethod(TSource, TKey);

        public static IAsyncQueryable<TSource> Distinct<TSource, TKey>(this IAsyncQueryable<TSource> source, Expression<Func<TSource, ValueTask<TKey>>> keySelector)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));
            if (keySelector == null)
                throw new ArgumentNullException(nameof(keySelector));

            return source.Provider.CreateQuery<TSource>(Expression.Call(Distinct__TSource_TKey__2__2(typeof(TSource), typeof(TKey)), source.Expression, keySelector));
        }

        private static MethodInfo? s_Distinct__TSource_TKey__3__0;
        
        private static MethodInfo? Distinct__TSource_TKey__3__0(Type TSource, Type TKey) =>
            (s_Distinct__TSource_TKey__3__0 ??
            (s_Distinct__TSource_TKey__3__0 = new Func<IAsyncQueryable<object>, Expression<Func<object, CancellationToken, ValueTask<object>>>, IEqualityComparer<object>, IAsyncQueryable<object>>(Distinct<object, object>).GetMethodInfo()!.GetGenericMethodDefinition())).MakeGenericMethod(TSource, TKey);

        public static IAsyncQueryable<TSource> Distinct<TSource, TKey>(this IAsyncQueryable<TSource> source, Expression<Func<TSource, CancellationToken, ValueTask<TKey>>> keySelector, IEqualityComparer<TKey>? comparer)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));
            if (keySelector == null)
                throw new ArgumentNullException(nameof(keySelector));

            return source.Provider.CreateQuery<TSource>(Expression.Call(Distinct__TSource_TKey__3__0(typeof(TSource), typeof(TKey)), source.Expression, keySelector, Expression.Constant(comparer, typeof(IEqualityComparer<TKey>))));
        }

        private static MethodInfo? s_Distinct__TSource_TKey__3__1;
        
        private static MethodInfo? Distinct__TSource_TKey__3__1(Type TSource, Type TKey) =>
            (s_Distinct__TSource_TKey__3__1 ??
            (s_Distinct__TSource_TKey__3__1 = new Func<IAsyncQueryable<object>, Expression<Func<object, object>>, IEqualityComparer<object>, IAsyncQueryable<object>>(Distinct<object, object>).GetMethodInfo()!.GetGenericMethodDefinition())).MakeGenericMethod(TSource, TKey);

        public static IAsyncQueryable<TSource> Distinct<TSource, TKey>(this IAsyncQueryable<TSource> source, Expression<Func<TSource, TKey>> keySelector, IEqualityComparer<TKey>? comparer)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));
            if (keySelector == null)
                throw new ArgumentNullException(nameof(keySelector));

            return source.Provider.CreateQuery<TSource>(Expression.Call(Distinct__TSource_TKey__3__1(typeof(TSource), typeof(TKey)), source.Expression, keySelector, Expression.Constant(comparer, typeof(IEqualityComparer<TKey>))));
        }

        private static MethodInfo? s_Distinct__TSource_TKey__3__2;
        
        private static MethodInfo? Distinct__TSource_TKey__3__2(Type TSource, Type TKey) =>
            (s_Distinct__TSource_TKey__3__2 ??
            (s_Distinct__TSource_TKey__3__2 = new Func<IAsyncQueryable<object>, Expression<Func<object, ValueTask<object>>>, IEqualityComparer<object>, IAsyncQueryable<object>>(Distinct<object, object>).GetMethodInfo()!.GetGenericMethodDefinition())).MakeGenericMethod(TSource, TKey);

        public static IAsyncQueryable<TSource> Distinct<TSource, TKey>(this IAsyncQueryable<TSource> source, Expression<Func<TSource, ValueTask<TKey>>> keySelector, IEqualityComparer<TKey>? comparer)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));
            if (keySelector == null)
                throw new ArgumentNullException(nameof(keySelector));

            return source.Provider.CreateQuery<TSource>(Expression.Call(Distinct__TSource_TKey__3__2(typeof(TSource), typeof(TKey)), source.Expression, keySelector, Expression.Constant(comparer, typeof(IEqualityComparer<TKey>))));
        }

        private static MethodInfo? s_DistinctUntilChanged__TSource__1__0;
        
        private static MethodInfo? DistinctUntilChanged__TSource__1__0(Type TSource) =>
            (s_DistinctUntilChanged__TSource__1__0 ??
            (s_DistinctUntilChanged__TSource__1__0 = new Func<IAsyncQueryable<object>, IAsyncQueryable<object>>(DistinctUntilChanged<object>).GetMethodInfo()!.GetGenericMethodDefinition())).MakeGenericMethod(TSource);

        public static IAsyncQueryable<TSource> DistinctUntilChanged<TSource>(this IAsyncQueryable<TSource> source)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));

            return source.Provider.CreateQuery<TSource>(Expression.Call(DistinctUntilChanged__TSource__1__0(typeof(TSource)), source.Expression));
        }

        private static MethodInfo? s_DistinctUntilChanged__TSource__2__0;
        
        private static MethodInfo? DistinctUntilChanged__TSource__2__0(Type TSource) =>
            (s_DistinctUntilChanged__TSource__2__0 ??
            (s_DistinctUntilChanged__TSource__2__0 = new Func<IAsyncQueryable<object>, IEqualityComparer<object>, IAsyncQueryable<object>>(DistinctUntilChanged<object>).GetMethodInfo()!.GetGenericMethodDefinition())).MakeGenericMethod(TSource);

        public static IAsyncQueryable<TSource> DistinctUntilChanged<TSource>(this IAsyncQueryable<TSource> source, IEqualityComparer<TSource>? comparer)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));

            return source.Provider.CreateQuery<TSource>(Expression.Call(DistinctUntilChanged__TSource__2__0(typeof(TSource)), source.Expression, Expression.Constant(comparer, typeof(IEqualityComparer<TSource>))));
        }

        private static MethodInfo? s_DistinctUntilChanged__TSource_TKey__2__0;
        
        private static MethodInfo? DistinctUntilChanged__TSource_TKey__2__0(Type TSource, Type TKey) =>
            (s_DistinctUntilChanged__TSource_TKey__2__0 ??
            (s_DistinctUntilChanged__TSource_TKey__2__0 = new Func<IAsyncQueryable<object>, Expression<Func<object, CancellationToken, ValueTask<object>>>, IAsyncQueryable<object>>(DistinctUntilChanged<object, object>).GetMethodInfo()!.GetGenericMethodDefinition())).MakeGenericMethod(TSource, TKey);

        public static IAsyncQueryable<TSource> DistinctUntilChanged<TSource, TKey>(this IAsyncQueryable<TSource> source, Expression<Func<TSource, CancellationToken, ValueTask<TKey>>> keySelector)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));
            if (keySelector == null)
                throw new ArgumentNullException(nameof(keySelector));

            return source.Provider.CreateQuery<TSource>(Expression.Call(DistinctUntilChanged__TSource_TKey__2__0(typeof(TSource), typeof(TKey)), source.Expression, keySelector));
        }

        private static MethodInfo? s_DistinctUntilChanged__TSource_TKey__2__1;
        
        private static MethodInfo? DistinctUntilChanged__TSource_TKey__2__1(Type TSource, Type TKey) =>
            (s_DistinctUntilChanged__TSource_TKey__2__1 ??
            (s_DistinctUntilChanged__TSource_TKey__2__1 = new Func<IAsyncQueryable<object>, Expression<Func<object, object>>, IAsyncQueryable<object>>(DistinctUntilChanged<object, object>).GetMethodInfo()!.GetGenericMethodDefinition())).MakeGenericMethod(TSource, TKey);

        public static IAsyncQueryable<TSource> DistinctUntilChanged<TSource, TKey>(this IAsyncQueryable<TSource> source, Expression<Func<TSource, TKey>> keySelector)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));
            if (keySelector == null)
                throw new ArgumentNullException(nameof(keySelector));

            return source.Provider.CreateQuery<TSource>(Expression.Call(DistinctUntilChanged__TSource_TKey__2__1(typeof(TSource), typeof(TKey)), source.Expression, keySelector));
        }

        private static MethodInfo? s_DistinctUntilChanged__TSource_TKey__2__2;
        
        private static MethodInfo? DistinctUntilChanged__TSource_TKey__2__2(Type TSource, Type TKey) =>
            (s_DistinctUntilChanged__TSource_TKey__2__2 ??
            (s_DistinctUntilChanged__TSource_TKey__2__2 = new Func<IAsyncQueryable<object>, Expression<Func<object, ValueTask<object>>>, IAsyncQueryable<object>>(DistinctUntilChanged<object, object>).GetMethodInfo()!.GetGenericMethodDefinition())).MakeGenericMethod(TSource, TKey);

        public static IAsyncQueryable<TSource> DistinctUntilChanged<TSource, TKey>(this IAsyncQueryable<TSource> source, Expression<Func<TSource, ValueTask<TKey>>> keySelector)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));
            if (keySelector == null)
                throw new ArgumentNullException(nameof(keySelector));

            return source.Provider.CreateQuery<TSource>(Expression.Call(DistinctUntilChanged__TSource_TKey__2__2(typeof(TSource), typeof(TKey)), source.Expression, keySelector));
        }

        private static MethodInfo? s_DistinctUntilChanged__TSource_TKey__3__0;
        
        private static MethodInfo? DistinctUntilChanged__TSource_TKey__3__0(Type TSource, Type TKey) =>
            (s_DistinctUntilChanged__TSource_TKey__3__0 ??
            (s_DistinctUntilChanged__TSource_TKey__3__0 = new Func<IAsyncQueryable<object>, Expression<Func<object, CancellationToken, ValueTask<object>>>, IEqualityComparer<object>, IAsyncQueryable<object>>(DistinctUntilChanged<object, object>).GetMethodInfo()!.GetGenericMethodDefinition())).MakeGenericMethod(TSource, TKey);

        public static IAsyncQueryable<TSource> DistinctUntilChanged<TSource, TKey>(this IAsyncQueryable<TSource> source, Expression<Func<TSource, CancellationToken, ValueTask<TKey>>> keySelector, IEqualityComparer<TKey>? comparer)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));
            if (keySelector == null)
                throw new ArgumentNullException(nameof(keySelector));

            return source.Provider.CreateQuery<TSource>(Expression.Call(DistinctUntilChanged__TSource_TKey__3__0(typeof(TSource), typeof(TKey)), source.Expression, keySelector, Expression.Constant(comparer, typeof(IEqualityComparer<TKey>))));
        }

        private static MethodInfo? s_DistinctUntilChanged__TSource_TKey__3__1;
        
        private static MethodInfo? DistinctUntilChanged__TSource_TKey__3__1(Type TSource, Type TKey) =>
            (s_DistinctUntilChanged__TSource_TKey__3__1 ??
            (s_DistinctUntilChanged__TSource_TKey__3__1 = new Func<IAsyncQueryable<object>, Expression<Func<object, object>>, IEqualityComparer<object>, IAsyncQueryable<object>>(DistinctUntilChanged<object, object>).GetMethodInfo()!.GetGenericMethodDefinition())).MakeGenericMethod(TSource, TKey);

        public static IAsyncQueryable<TSource> DistinctUntilChanged<TSource, TKey>(this IAsyncQueryable<TSource> source, Expression<Func<TSource, TKey>> keySelector, IEqualityComparer<TKey>? comparer)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));
            if (keySelector == null)
                throw new ArgumentNullException(nameof(keySelector));

            return source.Provider.CreateQuery<TSource>(Expression.Call(DistinctUntilChanged__TSource_TKey__3__1(typeof(TSource), typeof(TKey)), source.Expression, keySelector, Expression.Constant(comparer, typeof(IEqualityComparer<TKey>))));
        }

        private static MethodInfo? s_DistinctUntilChanged__TSource_TKey__3__2;
        
        private static MethodInfo? DistinctUntilChanged__TSource_TKey__3__2(Type TSource, Type TKey) =>
            (s_DistinctUntilChanged__TSource_TKey__3__2 ??
            (s_DistinctUntilChanged__TSource_TKey__3__2 = new Func<IAsyncQueryable<object>, Expression<Func<object, ValueTask<object>>>, IEqualityComparer<object>, IAsyncQueryable<object>>(DistinctUntilChanged<object, object>).GetMethodInfo()!.GetGenericMethodDefinition())).MakeGenericMethod(TSource, TKey);

        public static IAsyncQueryable<TSource> DistinctUntilChanged<TSource, TKey>(this IAsyncQueryable<TSource> source, Expression<Func<TSource, ValueTask<TKey>>> keySelector, IEqualityComparer<TKey>? comparer)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));
            if (keySelector == null)
                throw new ArgumentNullException(nameof(keySelector));

            return source.Provider.CreateQuery<TSource>(Expression.Call(DistinctUntilChanged__TSource_TKey__3__2(typeof(TSource), typeof(TKey)), source.Expression, keySelector, Expression.Constant(comparer, typeof(IEqualityComparer<TKey>))));
        }

        private static MethodInfo? s_Do__TSource__2__0;
        
        private static MethodInfo? Do__TSource__2__0(Type TSource) =>
            (s_Do__TSource__2__0 ??
            (s_Do__TSource__2__0 = new Func<IAsyncQueryable<object>, Expression<Action<object>>, IAsyncQueryable<object>>(Do<object>).GetMethodInfo()!.GetGenericMethodDefinition())).MakeGenericMethod(TSource);

        public static IAsyncQueryable<TSource> Do<TSource>(this IAsyncQueryable<TSource> source, Expression<Action<TSource>> onNext)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));
            if (onNext == null)
                throw new ArgumentNullException(nameof(onNext));

            return source.Provider.CreateQuery<TSource>(Expression.Call(Do__TSource__2__0(typeof(TSource)), source.Expression, onNext));
        }

        private static MethodInfo? s_Do__TSource__2__1;
        
        private static MethodInfo? Do__TSource__2__1(Type TSource) =>
            (s_Do__TSource__2__1 ??
            (s_Do__TSource__2__1 = new Func<IAsyncQueryable<object>, Expression<Func<object, CancellationToken, Task>>, IAsyncQueryable<object>>(Do<object>).GetMethodInfo()!.GetGenericMethodDefinition())).MakeGenericMethod(TSource);

        public static IAsyncQueryable<TSource> Do<TSource>(this IAsyncQueryable<TSource> source, Expression<Func<TSource, CancellationToken, Task>> onNext)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));
            if (onNext == null)
                throw new ArgumentNullException(nameof(onNext));

            return source.Provider.CreateQuery<TSource>(Expression.Call(Do__TSource__2__1(typeof(TSource)), source.Expression, onNext));
        }

        private static MethodInfo? s_Do__TSource__2__2;
        
        private static MethodInfo? Do__TSource__2__2(Type TSource) =>
            (s_Do__TSource__2__2 ??
            (s_Do__TSource__2__2 = new Func<IAsyncQueryable<object>, Expression<Func<object, Task>>, IAsyncQueryable<object>>(Do<object>).GetMethodInfo()!.GetGenericMethodDefinition())).MakeGenericMethod(TSource);

        public static IAsyncQueryable<TSource> Do<TSource>(this IAsyncQueryable<TSource> source, Expression<Func<TSource, Task>> onNext)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));
            if (onNext == null)
                throw new ArgumentNullException(nameof(onNext));

            return source.Provider.CreateQuery<TSource>(Expression.Call(Do__TSource__2__2(typeof(TSource)), source.Expression, onNext));
        }

        private static MethodInfo? s_Do__TSource__2__3;
        
        private static MethodInfo? Do__TSource__2__3(Type TSource) =>
            (s_Do__TSource__2__3 ??
            (s_Do__TSource__2__3 = new Func<IAsyncQueryable<object>, IObserver<object>, IAsyncQueryable<object>>(Do<object>).GetMethodInfo()!.GetGenericMethodDefinition())).MakeGenericMethod(TSource);

        public static IAsyncQueryable<TSource> Do<TSource>(this IAsyncQueryable<TSource> source, IObserver<TSource> observer)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));
            if (observer == null)
                throw new ArgumentNullException(nameof(observer));

            return source.Provider.CreateQuery<TSource>(Expression.Call(Do__TSource__2__3(typeof(TSource)), source.Expression, Expression.Constant(observer, typeof(IObserver<TSource>))));
        }

        private static MethodInfo? s_Do__TSource__3__0;
        
        private static MethodInfo? Do__TSource__3__0(Type TSource) =>
            (s_Do__TSource__3__0 ??
            (s_Do__TSource__3__0 = new Func<IAsyncQueryable<object>, Expression<Action<object>>, Action, IAsyncQueryable<object>>(Do<object>).GetMethodInfo()!.GetGenericMethodDefinition())).MakeGenericMethod(TSource);

        public static IAsyncQueryable<TSource> Do<TSource>(this IAsyncQueryable<TSource> source, Expression<Action<TSource>> onNext, Action onCompleted)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));
            if (onNext == null)
                throw new ArgumentNullException(nameof(onNext));
            if (onCompleted == null)
                throw new ArgumentNullException(nameof(onCompleted));

            return source.Provider.CreateQuery<TSource>(Expression.Call(Do__TSource__3__0(typeof(TSource)), source.Expression, onNext, Expression.Constant(onCompleted, typeof(Action))));
        }

        private static MethodInfo? s_Do__TSource__3__1;
        
        private static MethodInfo? Do__TSource__3__1(Type TSource) =>
            (s_Do__TSource__3__1 ??
            (s_Do__TSource__3__1 = new Func<IAsyncQueryable<object>, Expression<Action<object>>, Expression<Action<Exception>>, IAsyncQueryable<object>>(Do<object>).GetMethodInfo()!.GetGenericMethodDefinition())).MakeGenericMethod(TSource);

        public static IAsyncQueryable<TSource> Do<TSource>(this IAsyncQueryable<TSource> source, Expression<Action<TSource>> onNext, Expression<Action<Exception>> onError)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));
            if (onNext == null)
                throw new ArgumentNullException(nameof(onNext));
            if (onError == null)
                throw new ArgumentNullException(nameof(onError));

            return source.Provider.CreateQuery<TSource>(Expression.Call(Do__TSource__3__1(typeof(TSource)), source.Expression, onNext, onError));
        }

        private static MethodInfo? s_Do__TSource__3__2;
        
        private static MethodInfo? Do__TSource__3__2(Type TSource) =>
            (s_Do__TSource__3__2 ??
            (s_Do__TSource__3__2 = new Func<IAsyncQueryable<object>, Expression<Func<object, CancellationToken, Task>>, Expression<Func<CancellationToken, Task>>, IAsyncQueryable<object>>(Do<object>).GetMethodInfo()!.GetGenericMethodDefinition())).MakeGenericMethod(TSource);

        public static IAsyncQueryable<TSource> Do<TSource>(this IAsyncQueryable<TSource> source, Expression<Func<TSource, CancellationToken, Task>> onNext, Expression<Func<CancellationToken, Task>> onCompleted)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));
            if (onNext == null)
                throw new ArgumentNullException(nameof(onNext));
            if (onCompleted == null)
                throw new ArgumentNullException(nameof(onCompleted));

            return source.Provider.CreateQuery<TSource>(Expression.Call(Do__TSource__3__2(typeof(TSource)), source.Expression, onNext, onCompleted));
        }

        private static MethodInfo? s_Do__TSource__3__3;
        
        private static MethodInfo? Do__TSource__3__3(Type TSource) =>
            (s_Do__TSource__3__3 ??
            (s_Do__TSource__3__3 = new Func<IAsyncQueryable<object>, Expression<Func<object, CancellationToken, Task>>, Expression<Func<Exception, CancellationToken, Task>>, IAsyncQueryable<object>>(Do<object>).GetMethodInfo()!.GetGenericMethodDefinition())).MakeGenericMethod(TSource);

        public static IAsyncQueryable<TSource> Do<TSource>(this IAsyncQueryable<TSource> source, Expression<Func<TSource, CancellationToken, Task>> onNext, Expression<Func<Exception, CancellationToken, Task>> onError)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));
            if (onNext == null)
                throw new ArgumentNullException(nameof(onNext));
            if (onError == null)
                throw new ArgumentNullException(nameof(onError));

            return source.Provider.CreateQuery<TSource>(Expression.Call(Do__TSource__3__3(typeof(TSource)), source.Expression, onNext, onError));
        }

        private static MethodInfo? s_Do__TSource__3__4;
        
        private static MethodInfo? Do__TSource__3__4(Type TSource) =>
            (s_Do__TSource__3__4 ??
            (s_Do__TSource__3__4 = new Func<IAsyncQueryable<object>, Expression<Func<object, Task>>, Expression<Func<Exception, Task>>, IAsyncQueryable<object>>(Do<object>).GetMethodInfo()!.GetGenericMethodDefinition())).MakeGenericMethod(TSource);

        public static IAsyncQueryable<TSource> Do<TSource>(this IAsyncQueryable<TSource> source, Expression<Func<TSource, Task>> onNext, Expression<Func<Exception, Task>> onError)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));
            if (onNext == null)
                throw new ArgumentNullException(nameof(onNext));
            if (onError == null)
                throw new ArgumentNullException(nameof(onError));

            return source.Provider.CreateQuery<TSource>(Expression.Call(Do__TSource__3__4(typeof(TSource)), source.Expression, onNext, onError));
        }

        private static MethodInfo? s_Do__TSource__3__5;
        
        private static MethodInfo? Do__TSource__3__5(Type TSource) =>
            (s_Do__TSource__3__5 ??
            (s_Do__TSource__3__5 = new Func<IAsyncQueryable<object>, Expression<Func<object, Task>>, Expression<Func<Task>>, IAsyncQueryable<object>>(Do<object>).GetMethodInfo()!.GetGenericMethodDefinition())).MakeGenericMethod(TSource);

        public static IAsyncQueryable<TSource> Do<TSource>(this IAsyncQueryable<TSource> source, Expression<Func<TSource, Task>> onNext, Expression<Func<Task>> onCompleted)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));
            if (onNext == null)
                throw new ArgumentNullException(nameof(onNext));
            if (onCompleted == null)
                throw new ArgumentNullException(nameof(onCompleted));

            return source.Provider.CreateQuery<TSource>(Expression.Call(Do__TSource__3__5(typeof(TSource)), source.Expression, onNext, onCompleted));
        }

        private static MethodInfo? s_Do__TSource__4__0;
        
        private static MethodInfo? Do__TSource__4__0(Type TSource) =>
            (s_Do__TSource__4__0 ??
            (s_Do__TSource__4__0 = new Func<IAsyncQueryable<object>, Expression<Action<object>>, Expression<Action<Exception>>, Action, IAsyncQueryable<object>>(Do<object>).GetMethodInfo()!.GetGenericMethodDefinition())).MakeGenericMethod(TSource);

        public static IAsyncQueryable<TSource> Do<TSource>(this IAsyncQueryable<TSource> source, Expression<Action<TSource>> onNext, Expression<Action<Exception>> onError, Action onCompleted)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));
            if (onNext == null)
                throw new ArgumentNullException(nameof(onNext));
            if (onError == null)
                throw new ArgumentNullException(nameof(onError));
            if (onCompleted == null)
                throw new ArgumentNullException(nameof(onCompleted));

            return source.Provider.CreateQuery<TSource>(Expression.Call(Do__TSource__4__0(typeof(TSource)), source.Expression, onNext, onError, Expression.Constant(onCompleted, typeof(Action))));
        }

        private static MethodInfo? s_Do__TSource__4__1;
        
        private static MethodInfo? Do__TSource__4__1(Type TSource) =>
            (s_Do__TSource__4__1 ??
            (s_Do__TSource__4__1 = new Func<IAsyncQueryable<object>, Expression<Func<object, CancellationToken, Task>>, Expression<Func<Exception, CancellationToken, Task>>, Expression<Func<CancellationToken, Task>>, IAsyncQueryable<object>>(Do<object>).GetMethodInfo()!.GetGenericMethodDefinition())).MakeGenericMethod(TSource);

        public static IAsyncQueryable<TSource> Do<TSource>(this IAsyncQueryable<TSource> source, Expression<Func<TSource, CancellationToken, Task>> onNext, Expression<Func<Exception, CancellationToken, Task>> onError, Expression<Func<CancellationToken, Task>> onCompleted)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));
            if (onNext == null)
                throw new ArgumentNullException(nameof(onNext));
            if (onError == null)
                throw new ArgumentNullException(nameof(onError));
            if (onCompleted == null)
                throw new ArgumentNullException(nameof(onCompleted));

            return source.Provider.CreateQuery<TSource>(Expression.Call(Do__TSource__4__1(typeof(TSource)), source.Expression, onNext, onError, onCompleted));
        }

        private static MethodInfo? s_Do__TSource__4__2;
        
        private static MethodInfo? Do__TSource__4__2(Type TSource) =>
            (s_Do__TSource__4__2 ??
            (s_Do__TSource__4__2 = new Func<IAsyncQueryable<object>, Expression<Func<object, Task>>, Expression<Func<Exception, Task>>, Expression<Func<Task>>, IAsyncQueryable<object>>(Do<object>).GetMethodInfo()!.GetGenericMethodDefinition())).MakeGenericMethod(TSource);

        public static IAsyncQueryable<TSource> Do<TSource>(this IAsyncQueryable<TSource> source, Expression<Func<TSource, Task>> onNext, Expression<Func<Exception, Task>> onError, Expression<Func<Task>> onCompleted)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));
            if (onNext == null)
                throw new ArgumentNullException(nameof(onNext));
            if (onError == null)
                throw new ArgumentNullException(nameof(onError));
            if (onCompleted == null)
                throw new ArgumentNullException(nameof(onCompleted));

            return source.Provider.CreateQuery<TSource>(Expression.Call(Do__TSource__4__2(typeof(TSource)), source.Expression, onNext, onError, onCompleted));
        }

        private static MethodInfo? s_Expand__TSource__2__0;
        
        private static MethodInfo? Expand__TSource__2__0(Type TSource) =>
            (s_Expand__TSource__2__0 ??
            (s_Expand__TSource__2__0 = new Func<IAsyncQueryable<object>, Expression<Func<object, CancellationToken, ValueTask<IAsyncEnumerable<object>>>>, IAsyncQueryable<object>>(Expand<object>).GetMethodInfo()!.GetGenericMethodDefinition())).MakeGenericMethod(TSource);

        public static IAsyncQueryable<TSource> Expand<TSource>(this IAsyncQueryable<TSource> source, Expression<Func<TSource, CancellationToken, ValueTask<IAsyncEnumerable<TSource>>>> selector)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));
            if (selector == null)
                throw new ArgumentNullException(nameof(selector));

            return source.Provider.CreateQuery<TSource>(Expression.Call(Expand__TSource__2__0(typeof(TSource)), source.Expression, selector));
        }

        private static MethodInfo? s_Expand__TSource__2__1;
        
        private static MethodInfo? Expand__TSource__2__1(Type TSource) =>
            (s_Expand__TSource__2__1 ??
            (s_Expand__TSource__2__1 = new Func<IAsyncQueryable<object>, Expression<Func<object, IAsyncEnumerable<object>>>, IAsyncQueryable<object>>(Expand<object>).GetMethodInfo()!.GetGenericMethodDefinition())).MakeGenericMethod(TSource);

        public static IAsyncQueryable<TSource> Expand<TSource>(this IAsyncQueryable<TSource> source, Expression<Func<TSource, IAsyncEnumerable<TSource>>> selector)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));
            if (selector == null)
                throw new ArgumentNullException(nameof(selector));

            return source.Provider.CreateQuery<TSource>(Expression.Call(Expand__TSource__2__1(typeof(TSource)), source.Expression, selector));
        }

        private static MethodInfo? s_Expand__TSource__2__2;
        
        private static MethodInfo? Expand__TSource__2__2(Type TSource) =>
            (s_Expand__TSource__2__2 ??
            (s_Expand__TSource__2__2 = new Func<IAsyncQueryable<object>, Expression<Func<object, ValueTask<IAsyncEnumerable<object>>>>, IAsyncQueryable<object>>(Expand<object>).GetMethodInfo()!.GetGenericMethodDefinition())).MakeGenericMethod(TSource);

        public static IAsyncQueryable<TSource> Expand<TSource>(this IAsyncQueryable<TSource> source, Expression<Func<TSource, ValueTask<IAsyncEnumerable<TSource>>>> selector)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));
            if (selector == null)
                throw new ArgumentNullException(nameof(selector));

            return source.Provider.CreateQuery<TSource>(Expression.Call(Expand__TSource__2__2(typeof(TSource)), source.Expression, selector));
        }

        private static MethodInfo? s_Finally__TSource__2__0;
        
        private static MethodInfo? Finally__TSource__2__0(Type TSource) =>
            (s_Finally__TSource__2__0 ??
            (s_Finally__TSource__2__0 = new Func<IAsyncQueryable<object>, Action, IAsyncQueryable<object>>(Finally<object>).GetMethodInfo()!.GetGenericMethodDefinition())).MakeGenericMethod(TSource);

        public static IAsyncQueryable<TSource> Finally<TSource>(this IAsyncQueryable<TSource> source, Action finallyAction)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));
            if (finallyAction == null)
                throw new ArgumentNullException(nameof(finallyAction));

            return source.Provider.CreateQuery<TSource>(Expression.Call(Finally__TSource__2__0(typeof(TSource)), source.Expression, Expression.Constant(finallyAction, typeof(Action))));
        }

        private static MethodInfo? s_Finally__TSource__2__1;
        
        private static MethodInfo? Finally__TSource__2__1(Type TSource) =>
            (s_Finally__TSource__2__1 ??
            (s_Finally__TSource__2__1 = new Func<IAsyncQueryable<object>, Expression<Func<Task>>, IAsyncQueryable<object>>(Finally<object>).GetMethodInfo()!.GetGenericMethodDefinition())).MakeGenericMethod(TSource);

        public static IAsyncQueryable<TSource> Finally<TSource>(this IAsyncQueryable<TSource> source, Expression<Func<Task>> finallyAction)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));
            if (finallyAction == null)
                throw new ArgumentNullException(nameof(finallyAction));

            return source.Provider.CreateQuery<TSource>(Expression.Call(Finally__TSource__2__1(typeof(TSource)), source.Expression, finallyAction));
        }

        private static MethodInfo? s_IgnoreElements__TSource__1__0;
        
        private static MethodInfo? IgnoreElements__TSource__1__0(Type TSource) =>
            (s_IgnoreElements__TSource__1__0 ??
            (s_IgnoreElements__TSource__1__0 = new Func<IAsyncQueryable<object>, IAsyncQueryable<object>>(IgnoreElements<object>).GetMethodInfo()!.GetGenericMethodDefinition())).MakeGenericMethod(TSource);

        public static IAsyncQueryable<TSource> IgnoreElements<TSource>(this IAsyncQueryable<TSource> source)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));

            return source.Provider.CreateQuery<TSource>(Expression.Call(IgnoreElements__TSource__1__0(typeof(TSource)), source.Expression));
        }

        private static MethodInfo? s_IsEmptyAsync__TSource__2__0;
        
        private static MethodInfo? IsEmptyAsync__TSource__2__0(Type TSource) =>
            (s_IsEmptyAsync__TSource__2__0 ??
            (s_IsEmptyAsync__TSource__2__0 = new Func<IAsyncQueryable<object>, CancellationToken, ValueTask<bool>>(IsEmptyAsync<object>).GetMethodInfo()!.GetGenericMethodDefinition())).MakeGenericMethod(TSource);

        public static ValueTask<bool> IsEmptyAsync<TSource>(this IAsyncQueryable<TSource> source, CancellationToken cancellationToken = default)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));

            return source.Provider.ExecuteAsync<bool>(Expression.Call(IsEmptyAsync__TSource__2__0(typeof(TSource)), source.Expression, Expression.Constant(cancellationToken, typeof(CancellationToken))), cancellationToken);
        }

        private static MethodInfo? s_MaxAsync__TSource__3__0;
        
        private static MethodInfo? MaxAsync__TSource__3__0(Type TSource) =>
            (s_MaxAsync__TSource__3__0 ??
            (s_MaxAsync__TSource__3__0 = new Func<IAsyncQueryable<object>, IComparer<object>, CancellationToken, ValueTask<object>>(MaxAsync<object>).GetMethodInfo()!.GetGenericMethodDefinition())).MakeGenericMethod(TSource);

        public static ValueTask<TSource> MaxAsync<TSource>(this IAsyncQueryable<TSource> source, IComparer<TSource>? comparer, CancellationToken cancellationToken = default)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));

            return source.Provider.ExecuteAsync<TSource>(Expression.Call(MaxAsync__TSource__3__0(typeof(TSource)), source.Expression, Expression.Constant(comparer, typeof(IComparer<TSource>)), Expression.Constant(cancellationToken, typeof(CancellationToken))), cancellationToken);
        }

        private static MethodInfo? s_MaxByAsync__TSource_TKey__3__0;
        
        private static MethodInfo? MaxByAsync__TSource_TKey__3__0(Type TSource, Type TKey) =>
            (s_MaxByAsync__TSource_TKey__3__0 ??
            (s_MaxByAsync__TSource_TKey__3__0 = new Func<IAsyncQueryable<object>, Expression<Func<object, CancellationToken, ValueTask<object>>>, CancellationToken, ValueTask<IList<object>>>(MaxByAsync<object, object>).GetMethodInfo()!.GetGenericMethodDefinition())).MakeGenericMethod(TSource, TKey);

        public static ValueTask<IList<TSource>> MaxByAsync<TSource, TKey>(this IAsyncQueryable<TSource> source, Expression<Func<TSource, CancellationToken, ValueTask<TKey>>> keySelector, CancellationToken cancellationToken = default)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));
            if (keySelector == null)
                throw new ArgumentNullException(nameof(keySelector));

            return source.Provider.ExecuteAsync<IList<TSource>>(Expression.Call(MaxByAsync__TSource_TKey__3__0(typeof(TSource), typeof(TKey)), source.Expression, keySelector, Expression.Constant(cancellationToken, typeof(CancellationToken))), cancellationToken);
        }

        private static MethodInfo? s_MaxByAsync__TSource_TKey__3__1;
        
        private static MethodInfo? MaxByAsync__TSource_TKey__3__1(Type TSource, Type TKey) =>
            (s_MaxByAsync__TSource_TKey__3__1 ??
            (s_MaxByAsync__TSource_TKey__3__1 = new Func<IAsyncQueryable<object>, Expression<Func<object, object>>, CancellationToken, ValueTask<IList<object>>>(MaxByAsync<object, object>).GetMethodInfo()!.GetGenericMethodDefinition())).MakeGenericMethod(TSource, TKey);

        public static ValueTask<IList<TSource>> MaxByAsync<TSource, TKey>(this IAsyncQueryable<TSource> source, Expression<Func<TSource, TKey>> keySelector, CancellationToken cancellationToken = default)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));
            if (keySelector == null)
                throw new ArgumentNullException(nameof(keySelector));

            return source.Provider.ExecuteAsync<IList<TSource>>(Expression.Call(MaxByAsync__TSource_TKey__3__1(typeof(TSource), typeof(TKey)), source.Expression, keySelector, Expression.Constant(cancellationToken, typeof(CancellationToken))), cancellationToken);
        }

        private static MethodInfo? s_MaxByAsync__TSource_TKey__3__2;
        
        private static MethodInfo? MaxByAsync__TSource_TKey__3__2(Type TSource, Type TKey) =>
            (s_MaxByAsync__TSource_TKey__3__2 ??
            (s_MaxByAsync__TSource_TKey__3__2 = new Func<IAsyncQueryable<object>, Expression<Func<object, ValueTask<object>>>, CancellationToken, ValueTask<IList<object>>>(MaxByAsync<object, object>).GetMethodInfo()!.GetGenericMethodDefinition())).MakeGenericMethod(TSource, TKey);

        public static ValueTask<IList<TSource>> MaxByAsync<TSource, TKey>(this IAsyncQueryable<TSource> source, Expression<Func<TSource, ValueTask<TKey>>> keySelector, CancellationToken cancellationToken = default)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));
            if (keySelector == null)
                throw new ArgumentNullException(nameof(keySelector));

            return source.Provider.ExecuteAsync<IList<TSource>>(Expression.Call(MaxByAsync__TSource_TKey__3__2(typeof(TSource), typeof(TKey)), source.Expression, keySelector, Expression.Constant(cancellationToken, typeof(CancellationToken))), cancellationToken);
        }

        private static MethodInfo? s_MaxByAsync__TSource_TKey__4__0;
        
        private static MethodInfo? MaxByAsync__TSource_TKey__4__0(Type TSource, Type TKey) =>
            (s_MaxByAsync__TSource_TKey__4__0 ??
            (s_MaxByAsync__TSource_TKey__4__0 = new Func<IAsyncQueryable<object>, Expression<Func<object, CancellationToken, ValueTask<object>>>, IComparer<object>, CancellationToken, ValueTask<IList<object>>>(MaxByAsync<object, object>).GetMethodInfo()!.GetGenericMethodDefinition())).MakeGenericMethod(TSource, TKey);

        public static ValueTask<IList<TSource>> MaxByAsync<TSource, TKey>(this IAsyncQueryable<TSource> source, Expression<Func<TSource, CancellationToken, ValueTask<TKey>>> keySelector, IComparer<TKey>? comparer, CancellationToken cancellationToken = default)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));
            if (keySelector == null)
                throw new ArgumentNullException(nameof(keySelector));

            return source.Provider.ExecuteAsync<IList<TSource>>(Expression.Call(MaxByAsync__TSource_TKey__4__0(typeof(TSource), typeof(TKey)), source.Expression, keySelector, Expression.Constant(comparer, typeof(IComparer<TKey>)), Expression.Constant(cancellationToken, typeof(CancellationToken))), cancellationToken);
        }

        private static MethodInfo? s_MaxByAsync__TSource_TKey__4__1;
        
        private static MethodInfo? MaxByAsync__TSource_TKey__4__1(Type TSource, Type TKey) =>
            (s_MaxByAsync__TSource_TKey__4__1 ??
            (s_MaxByAsync__TSource_TKey__4__1 = new Func<IAsyncQueryable<object>, Expression<Func<object, object>>, IComparer<object>, CancellationToken, ValueTask<IList<object>>>(MaxByAsync<object, object>).GetMethodInfo()!.GetGenericMethodDefinition())).MakeGenericMethod(TSource, TKey);

        public static ValueTask<IList<TSource>> MaxByAsync<TSource, TKey>(this IAsyncQueryable<TSource> source, Expression<Func<TSource, TKey>> keySelector, IComparer<TKey>? comparer, CancellationToken cancellationToken = default)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));
            if (keySelector == null)
                throw new ArgumentNullException(nameof(keySelector));

            return source.Provider.ExecuteAsync<IList<TSource>>(Expression.Call(MaxByAsync__TSource_TKey__4__1(typeof(TSource), typeof(TKey)), source.Expression, keySelector, Expression.Constant(comparer, typeof(IComparer<TKey>)), Expression.Constant(cancellationToken, typeof(CancellationToken))), cancellationToken);
        }

        private static MethodInfo? s_MaxByAsync__TSource_TKey__4__2;
        
        private static MethodInfo? MaxByAsync__TSource_TKey__4__2(Type TSource, Type TKey) =>
            (s_MaxByAsync__TSource_TKey__4__2 ??
            (s_MaxByAsync__TSource_TKey__4__2 = new Func<IAsyncQueryable<object>, Expression<Func<object, ValueTask<object>>>, IComparer<object>, CancellationToken, ValueTask<IList<object>>>(MaxByAsync<object, object>).GetMethodInfo()!.GetGenericMethodDefinition())).MakeGenericMethod(TSource, TKey);

        public static ValueTask<IList<TSource>> MaxByAsync<TSource, TKey>(this IAsyncQueryable<TSource> source, Expression<Func<TSource, ValueTask<TKey>>> keySelector, IComparer<TKey>? comparer, CancellationToken cancellationToken = default)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));
            if (keySelector == null)
                throw new ArgumentNullException(nameof(keySelector));

            return source.Provider.ExecuteAsync<IList<TSource>>(Expression.Call(MaxByAsync__TSource_TKey__4__2(typeof(TSource), typeof(TKey)), source.Expression, keySelector, Expression.Constant(comparer, typeof(IComparer<TKey>)), Expression.Constant(cancellationToken, typeof(CancellationToken))), cancellationToken);
        }

        private static MethodInfo? s_Merge__TSource__1__0;
        
        private static MethodInfo? Merge__TSource__1__0(Type TSource) =>
            (s_Merge__TSource__1__0 ??
            (s_Merge__TSource__1__0 = new Func<IAsyncQueryable<IAsyncEnumerable<object>>, IAsyncQueryable<object>>(Merge<object>).GetMethodInfo()!.GetGenericMethodDefinition())).MakeGenericMethod(TSource);

        public static IAsyncQueryable<TSource> Merge<TSource>(this IAsyncQueryable<IAsyncEnumerable<TSource>> sources)
        {
            if (sources == null)
                throw new ArgumentNullException(nameof(sources));

            return sources.Provider.CreateQuery<TSource>(Expression.Call(Merge__TSource__1__0(typeof(TSource)), sources.Expression));
        }

        private static MethodInfo? s_MinAsync__TSource__3__0;
        
        private static MethodInfo? MinAsync__TSource__3__0(Type TSource) =>
            (s_MinAsync__TSource__3__0 ??
            (s_MinAsync__TSource__3__0 = new Func<IAsyncQueryable<object>, IComparer<object>, CancellationToken, ValueTask<object>>(MinAsync<object>).GetMethodInfo()!.GetGenericMethodDefinition())).MakeGenericMethod(TSource);

        public static ValueTask<TSource> MinAsync<TSource>(this IAsyncQueryable<TSource> source, IComparer<TSource>? comparer, CancellationToken cancellationToken = default)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));

            return source.Provider.ExecuteAsync<TSource>(Expression.Call(MinAsync__TSource__3__0(typeof(TSource)), source.Expression, Expression.Constant(comparer, typeof(IComparer<TSource>)), Expression.Constant(cancellationToken, typeof(CancellationToken))), cancellationToken);
        }

        private static MethodInfo? s_MinByAsync__TSource_TKey__3__0;
        
        private static MethodInfo? MinByAsync__TSource_TKey__3__0(Type TSource, Type TKey) =>
            (s_MinByAsync__TSource_TKey__3__0 ??
            (s_MinByAsync__TSource_TKey__3__0 = new Func<IAsyncQueryable<object>, Expression<Func<object, CancellationToken, ValueTask<object>>>, CancellationToken, ValueTask<IList<object>>>(MinByAsync<object, object>).GetMethodInfo()!.GetGenericMethodDefinition())).MakeGenericMethod(TSource, TKey);

        public static ValueTask<IList<TSource>> MinByAsync<TSource, TKey>(this IAsyncQueryable<TSource> source, Expression<Func<TSource, CancellationToken, ValueTask<TKey>>> keySelector, CancellationToken cancellationToken = default)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));
            if (keySelector == null)
                throw new ArgumentNullException(nameof(keySelector));

            return source.Provider.ExecuteAsync<IList<TSource>>(Expression.Call(MinByAsync__TSource_TKey__3__0(typeof(TSource), typeof(TKey)), source.Expression, keySelector, Expression.Constant(cancellationToken, typeof(CancellationToken))), cancellationToken);
        }

        private static MethodInfo? s_MinByAsync__TSource_TKey__3__1;
        
        private static MethodInfo? MinByAsync__TSource_TKey__3__1(Type TSource, Type TKey) =>
            (s_MinByAsync__TSource_TKey__3__1 ??
            (s_MinByAsync__TSource_TKey__3__1 = new Func<IAsyncQueryable<object>, Expression<Func<object, object>>, CancellationToken, ValueTask<IList<object>>>(MinByAsync<object, object>).GetMethodInfo()!.GetGenericMethodDefinition())).MakeGenericMethod(TSource, TKey);

        public static ValueTask<IList<TSource>> MinByAsync<TSource, TKey>(this IAsyncQueryable<TSource> source, Expression<Func<TSource, TKey>> keySelector, CancellationToken cancellationToken = default)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));
            if (keySelector == null)
                throw new ArgumentNullException(nameof(keySelector));

            return source.Provider.ExecuteAsync<IList<TSource>>(Expression.Call(MinByAsync__TSource_TKey__3__1(typeof(TSource), typeof(TKey)), source.Expression, keySelector, Expression.Constant(cancellationToken, typeof(CancellationToken))), cancellationToken);
        }

        private static MethodInfo? s_MinByAsync__TSource_TKey__3__2;
        
        private static MethodInfo? MinByAsync__TSource_TKey__3__2(Type TSource, Type TKey) =>
            (s_MinByAsync__TSource_TKey__3__2 ??
            (s_MinByAsync__TSource_TKey__3__2 = new Func<IAsyncQueryable<object>, Expression<Func<object, ValueTask<object>>>, CancellationToken, ValueTask<IList<object>>>(MinByAsync<object, object>).GetMethodInfo()!.GetGenericMethodDefinition())).MakeGenericMethod(TSource, TKey);

        public static ValueTask<IList<TSource>> MinByAsync<TSource, TKey>(this IAsyncQueryable<TSource> source, Expression<Func<TSource, ValueTask<TKey>>> keySelector, CancellationToken cancellationToken = default)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));
            if (keySelector == null)
                throw new ArgumentNullException(nameof(keySelector));

            return source.Provider.ExecuteAsync<IList<TSource>>(Expression.Call(MinByAsync__TSource_TKey__3__2(typeof(TSource), typeof(TKey)), source.Expression, keySelector, Expression.Constant(cancellationToken, typeof(CancellationToken))), cancellationToken);
        }

        private static MethodInfo? s_MinByAsync__TSource_TKey__4__0;
        
        private static MethodInfo? MinByAsync__TSource_TKey__4__0(Type TSource, Type TKey) =>
            (s_MinByAsync__TSource_TKey__4__0 ??
            (s_MinByAsync__TSource_TKey__4__0 = new Func<IAsyncQueryable<object>, Expression<Func<object, CancellationToken, ValueTask<object>>>, IComparer<object>, CancellationToken, ValueTask<IList<object>>>(MinByAsync<object, object>).GetMethodInfo()!.GetGenericMethodDefinition())).MakeGenericMethod(TSource, TKey);

        public static ValueTask<IList<TSource>> MinByAsync<TSource, TKey>(this IAsyncQueryable<TSource> source, Expression<Func<TSource, CancellationToken, ValueTask<TKey>>> keySelector, IComparer<TKey>? comparer, CancellationToken cancellationToken = default)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));
            if (keySelector == null)
                throw new ArgumentNullException(nameof(keySelector));

            return source.Provider.ExecuteAsync<IList<TSource>>(Expression.Call(MinByAsync__TSource_TKey__4__0(typeof(TSource), typeof(TKey)), source.Expression, keySelector, Expression.Constant(comparer, typeof(IComparer<TKey>)), Expression.Constant(cancellationToken, typeof(CancellationToken))), cancellationToken);
        }

        private static MethodInfo? s_MinByAsync__TSource_TKey__4__1;
        
        private static MethodInfo? MinByAsync__TSource_TKey__4__1(Type TSource, Type TKey) =>
            (s_MinByAsync__TSource_TKey__4__1 ??
            (s_MinByAsync__TSource_TKey__4__1 = new Func<IAsyncQueryable<object>, Expression<Func<object, object>>, IComparer<object>, CancellationToken, ValueTask<IList<object>>>(MinByAsync<object, object>).GetMethodInfo()!.GetGenericMethodDefinition())).MakeGenericMethod(TSource, TKey);

        public static ValueTask<IList<TSource>> MinByAsync<TSource, TKey>(this IAsyncQueryable<TSource> source, Expression<Func<TSource, TKey>> keySelector, IComparer<TKey>? comparer, CancellationToken cancellationToken = default)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));
            if (keySelector == null)
                throw new ArgumentNullException(nameof(keySelector));

            return source.Provider.ExecuteAsync<IList<TSource>>(Expression.Call(MinByAsync__TSource_TKey__4__1(typeof(TSource), typeof(TKey)), source.Expression, keySelector, Expression.Constant(comparer, typeof(IComparer<TKey>)), Expression.Constant(cancellationToken, typeof(CancellationToken))), cancellationToken);
        }

        private static MethodInfo? s_MinByAsync__TSource_TKey__4__2;
        
        private static MethodInfo? MinByAsync__TSource_TKey__4__2(Type TSource, Type TKey) =>
            (s_MinByAsync__TSource_TKey__4__2 ??
            (s_MinByAsync__TSource_TKey__4__2 = new Func<IAsyncQueryable<object>, Expression<Func<object, ValueTask<object>>>, IComparer<object>, CancellationToken, ValueTask<IList<object>>>(MinByAsync<object, object>).GetMethodInfo()!.GetGenericMethodDefinition())).MakeGenericMethod(TSource, TKey);

        public static ValueTask<IList<TSource>> MinByAsync<TSource, TKey>(this IAsyncQueryable<TSource> source, Expression<Func<TSource, ValueTask<TKey>>> keySelector, IComparer<TKey>? comparer, CancellationToken cancellationToken = default)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));
            if (keySelector == null)
                throw new ArgumentNullException(nameof(keySelector));

            return source.Provider.ExecuteAsync<IList<TSource>>(Expression.Call(MinByAsync__TSource_TKey__4__2(typeof(TSource), typeof(TKey)), source.Expression, keySelector, Expression.Constant(comparer, typeof(IComparer<TKey>)), Expression.Constant(cancellationToken, typeof(CancellationToken))), cancellationToken);
        }

        private static MethodInfo? s_OnErrorResumeNext__TSource__2__0;
        
        private static MethodInfo? OnErrorResumeNext__TSource__2__0(Type TSource) =>
            (s_OnErrorResumeNext__TSource__2__0 ??
            (s_OnErrorResumeNext__TSource__2__0 = new Func<IAsyncQueryable<object>, IAsyncEnumerable<object>, IAsyncQueryable<object>>(OnErrorResumeNext<object>).GetMethodInfo()!.GetGenericMethodDefinition())).MakeGenericMethod(TSource);

        public static IAsyncQueryable<TSource> OnErrorResumeNext<TSource>(this IAsyncQueryable<TSource> first, IAsyncEnumerable<TSource> second)
        {
            if (first == null)
                throw new ArgumentNullException(nameof(first));
            if (second == null)
                throw new ArgumentNullException(nameof(second));

            return first.Provider.CreateQuery<TSource>(Expression.Call(OnErrorResumeNext__TSource__2__0(typeof(TSource)), first.Expression, GetSourceExpression(second)));
        }

        private static MethodInfo? s_Repeat__TSource__1__0;
        
        private static MethodInfo? Repeat__TSource__1__0(Type TSource) =>
            (s_Repeat__TSource__1__0 ??
            (s_Repeat__TSource__1__0 = new Func<IAsyncQueryable<object>, IAsyncQueryable<object>>(Repeat<object>).GetMethodInfo()!.GetGenericMethodDefinition())).MakeGenericMethod(TSource);

        public static IAsyncQueryable<TSource> Repeat<TSource>(this IAsyncQueryable<TSource> source)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));

            return source.Provider.CreateQuery<TSource>(Expression.Call(Repeat__TSource__1__0(typeof(TSource)), source.Expression));
        }

        private static MethodInfo? s_Repeat__TSource__2__0;
        
        private static MethodInfo? Repeat__TSource__2__0(Type TSource) =>
            (s_Repeat__TSource__2__0 ??
            (s_Repeat__TSource__2__0 = new Func<IAsyncQueryable<object>, int, IAsyncQueryable<object>>(Repeat<object>).GetMethodInfo()!.GetGenericMethodDefinition())).MakeGenericMethod(TSource);

        public static IAsyncQueryable<TSource> Repeat<TSource>(this IAsyncQueryable<TSource> source, int count)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));

            return source.Provider.CreateQuery<TSource>(Expression.Call(Repeat__TSource__2__0(typeof(TSource)), source.Expression, Expression.Constant(count, typeof(int))));
        }

        private static MethodInfo? s_Retry__TSource__1__0;
        
        private static MethodInfo? Retry__TSource__1__0(Type TSource) =>
            (s_Retry__TSource__1__0 ??
            (s_Retry__TSource__1__0 = new Func<IAsyncQueryable<object>, IAsyncQueryable<object>>(Retry<object>).GetMethodInfo()!.GetGenericMethodDefinition())).MakeGenericMethod(TSource);

        public static IAsyncQueryable<TSource> Retry<TSource>(this IAsyncQueryable<TSource> source)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));

            return source.Provider.CreateQuery<TSource>(Expression.Call(Retry__TSource__1__0(typeof(TSource)), source.Expression));
        }

        private static MethodInfo? s_Retry__TSource__2__0;
        
        private static MethodInfo? Retry__TSource__2__0(Type TSource) =>
            (s_Retry__TSource__2__0 ??
            (s_Retry__TSource__2__0 = new Func<IAsyncQueryable<object>, int, IAsyncQueryable<object>>(Retry<object>).GetMethodInfo()!.GetGenericMethodDefinition())).MakeGenericMethod(TSource);

        public static IAsyncQueryable<TSource> Retry<TSource>(this IAsyncQueryable<TSource> source, int retryCount)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));

            return source.Provider.CreateQuery<TSource>(Expression.Call(Retry__TSource__2__0(typeof(TSource)), source.Expression, Expression.Constant(retryCount, typeof(int))));
        }

        private static MethodInfo? s_Scan__TSource__2__0;
        
        private static MethodInfo? Scan__TSource__2__0(Type TSource) =>
            (s_Scan__TSource__2__0 ??
            (s_Scan__TSource__2__0 = new Func<IAsyncQueryable<object>, Expression<Func<object, object, CancellationToken, ValueTask<object>>>, IAsyncQueryable<object>>(Scan<object>).GetMethodInfo()!.GetGenericMethodDefinition())).MakeGenericMethod(TSource);

        public static IAsyncQueryable<TSource> Scan<TSource>(this IAsyncQueryable<TSource> source, Expression<Func<TSource, TSource, CancellationToken, ValueTask<TSource>>> accumulator)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));
            if (accumulator == null)
                throw new ArgumentNullException(nameof(accumulator));

            return source.Provider.CreateQuery<TSource>(Expression.Call(Scan__TSource__2__0(typeof(TSource)), source.Expression, accumulator));
        }

        private static MethodInfo? s_Scan__TSource__2__1;
        
        private static MethodInfo? Scan__TSource__2__1(Type TSource) =>
            (s_Scan__TSource__2__1 ??
            (s_Scan__TSource__2__1 = new Func<IAsyncQueryable<object>, Expression<Func<object, object, object>>, IAsyncQueryable<object>>(Scan<object>).GetMethodInfo()!.GetGenericMethodDefinition())).MakeGenericMethod(TSource);

        public static IAsyncQueryable<TSource> Scan<TSource>(this IAsyncQueryable<TSource> source, Expression<Func<TSource, TSource, TSource>> accumulator)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));
            if (accumulator == null)
                throw new ArgumentNullException(nameof(accumulator));

            return source.Provider.CreateQuery<TSource>(Expression.Call(Scan__TSource__2__1(typeof(TSource)), source.Expression, accumulator));
        }

        private static MethodInfo? s_Scan__TSource__2__2;
        
        private static MethodInfo? Scan__TSource__2__2(Type TSource) =>
            (s_Scan__TSource__2__2 ??
            (s_Scan__TSource__2__2 = new Func<IAsyncQueryable<object>, Expression<Func<object, object, ValueTask<object>>>, IAsyncQueryable<object>>(Scan<object>).GetMethodInfo()!.GetGenericMethodDefinition())).MakeGenericMethod(TSource);

        public static IAsyncQueryable<TSource> Scan<TSource>(this IAsyncQueryable<TSource> source, Expression<Func<TSource, TSource, ValueTask<TSource>>> accumulator)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));
            if (accumulator == null)
                throw new ArgumentNullException(nameof(accumulator));

            return source.Provider.CreateQuery<TSource>(Expression.Call(Scan__TSource__2__2(typeof(TSource)), source.Expression, accumulator));
        }

        private static MethodInfo? s_Scan__TSource_TAccumulate__3__0;
        
        private static MethodInfo? Scan__TSource_TAccumulate__3__0(Type TSource, Type TAccumulate) =>
            (s_Scan__TSource_TAccumulate__3__0 ??
            (s_Scan__TSource_TAccumulate__3__0 = new Func<IAsyncQueryable<object>, object, Expression<Func<object, object, CancellationToken, ValueTask<object>>>, IAsyncQueryable<object>>(Scan<object, object>).GetMethodInfo()!.GetGenericMethodDefinition())).MakeGenericMethod(TSource, TAccumulate);

        public static IAsyncQueryable<TAccumulate> Scan<TSource, TAccumulate>(this IAsyncQueryable<TSource> source, TAccumulate seed, Expression<Func<TAccumulate, TSource, CancellationToken, ValueTask<TAccumulate>>> accumulator)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));
            if (accumulator == null)
                throw new ArgumentNullException(nameof(accumulator));

            return source.Provider.CreateQuery<TAccumulate>(Expression.Call(Scan__TSource_TAccumulate__3__0(typeof(TSource), typeof(TAccumulate)), source.Expression, Expression.Constant(seed, typeof(TAccumulate)), accumulator));
        }

        private static MethodInfo? s_Scan__TSource_TAccumulate__3__1;
        
        private static MethodInfo? Scan__TSource_TAccumulate__3__1(Type TSource, Type TAccumulate) =>
            (s_Scan__TSource_TAccumulate__3__1 ??
            (s_Scan__TSource_TAccumulate__3__1 = new Func<IAsyncQueryable<object>, object, Expression<Func<object, object, object>>, IAsyncQueryable<object>>(Scan<object, object>).GetMethodInfo()!.GetGenericMethodDefinition())).MakeGenericMethod(TSource, TAccumulate);

        public static IAsyncQueryable<TAccumulate> Scan<TSource, TAccumulate>(this IAsyncQueryable<TSource> source, TAccumulate seed, Expression<Func<TAccumulate, TSource, TAccumulate>> accumulator)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));
            if (accumulator == null)
                throw new ArgumentNullException(nameof(accumulator));

            return source.Provider.CreateQuery<TAccumulate>(Expression.Call(Scan__TSource_TAccumulate__3__1(typeof(TSource), typeof(TAccumulate)), source.Expression, Expression.Constant(seed, typeof(TAccumulate)), accumulator));
        }

        private static MethodInfo? s_Scan__TSource_TAccumulate__3__2;
        
        private static MethodInfo? Scan__TSource_TAccumulate__3__2(Type TSource, Type TAccumulate) =>
            (s_Scan__TSource_TAccumulate__3__2 ??
            (s_Scan__TSource_TAccumulate__3__2 = new Func<IAsyncQueryable<object>, object, Expression<Func<object, object, ValueTask<object>>>, IAsyncQueryable<object>>(Scan<object, object>).GetMethodInfo()!.GetGenericMethodDefinition())).MakeGenericMethod(TSource, TAccumulate);

        public static IAsyncQueryable<TAccumulate> Scan<TSource, TAccumulate>(this IAsyncQueryable<TSource> source, TAccumulate seed, Expression<Func<TAccumulate, TSource, ValueTask<TAccumulate>>> accumulator)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));
            if (accumulator == null)
                throw new ArgumentNullException(nameof(accumulator));

            return source.Provider.CreateQuery<TAccumulate>(Expression.Call(Scan__TSource_TAccumulate__3__2(typeof(TSource), typeof(TAccumulate)), source.Expression, Expression.Constant(seed, typeof(TAccumulate)), accumulator));
        }

        private static MethodInfo? s_SelectMany__TSource_TOther__2__0;
        
        private static MethodInfo? SelectMany__TSource_TOther__2__0(Type TSource, Type TOther) =>
            (s_SelectMany__TSource_TOther__2__0 ??
            (s_SelectMany__TSource_TOther__2__0 = new Func<IAsyncQueryable<object>, IAsyncEnumerable<object>, IAsyncQueryable<object>>(SelectMany<object, object>).GetMethodInfo()!.GetGenericMethodDefinition())).MakeGenericMethod(TSource, TOther);

        public static IAsyncQueryable<TOther> SelectMany<TSource, TOther>(this IAsyncQueryable<TSource> source, IAsyncEnumerable<TOther> other)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));
            if (other == null)
                throw new ArgumentNullException(nameof(other));

            return source.Provider.CreateQuery<TOther>(Expression.Call(SelectMany__TSource_TOther__2__0(typeof(TSource), typeof(TOther)), source.Expression, GetSourceExpression(other)));
        }

        private static MethodInfo? s_StartWith__TSource__2__0;
        
        private static MethodInfo? StartWith__TSource__2__0(Type TSource) =>
            (s_StartWith__TSource__2__0 ??
            (s_StartWith__TSource__2__0 = new Func<IAsyncQueryable<object>, object[], IAsyncQueryable<object>>(StartWith<object>).GetMethodInfo()!.GetGenericMethodDefinition())).MakeGenericMethod(TSource);

        public static IAsyncQueryable<TSource> StartWith<TSource>(this IAsyncQueryable<TSource> source, params TSource[] values)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));
            if (values == null)
                throw new ArgumentNullException(nameof(values));

            return source.Provider.CreateQuery<TSource>(Expression.Call(StartWith__TSource__2__0(typeof(TSource)), source.Expression, Expression.Constant(values, typeof(TSource[]))));
        }

        private static MethodInfo? s_Timeout__TSource__2__0;
        
        private static MethodInfo? Timeout__TSource__2__0(Type TSource) =>
            (s_Timeout__TSource__2__0 ??
            (s_Timeout__TSource__2__0 = new Func<IAsyncQueryable<object>, TimeSpan, IAsyncQueryable<object>>(Timeout<object>).GetMethodInfo()!.GetGenericMethodDefinition())).MakeGenericMethod(TSource);

        public static IAsyncQueryable<TSource> Timeout<TSource>(this IAsyncQueryable<TSource> source, TimeSpan timeout)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));

            return source.Provider.CreateQuery<TSource>(Expression.Call(Timeout__TSource__2__0(typeof(TSource)), source.Expression, Expression.Constant(timeout, typeof(TimeSpan))));
        }

    }
}

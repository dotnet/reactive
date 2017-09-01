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
    public static partial class AsyncQueryableEx
    {
        public static IAsyncQueryable<IList<TSource>> Buffer<TSource>(this IAsyncQueryable<TSource> source, int count)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));

#if CRIPPLED_REFLECTION
            return source.Provider.CreateQuery<IList<TSource>>(Expression.Call(InfoOf(() => AsyncQueryableEx.Buffer<TSource>(default(IAsyncQueryable<TSource>), default(int))), source.Expression, Expression.Constant(count, typeof(int))));
#else
            return source.Provider.CreateQuery<IList<TSource>>(Expression.Call(((MethodInfo)MethodBase.GetCurrentMethod()).MakeGenericMethod(typeof(TSource)), source.Expression, Expression.Constant(count, typeof(int))));
#endif
        }

        public static IAsyncQueryable<IList<TSource>> Buffer<TSource>(this IAsyncQueryable<TSource> source, int count, int skip)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));

#if CRIPPLED_REFLECTION
            return source.Provider.CreateQuery<IList<TSource>>(Expression.Call(InfoOf(() => AsyncQueryableEx.Buffer<TSource>(default(IAsyncQueryable<TSource>), default(int), default(int))), source.Expression, Expression.Constant(count, typeof(int)), Expression.Constant(skip, typeof(int))));
#else
            return source.Provider.CreateQuery<IList<TSource>>(Expression.Call(((MethodInfo)MethodBase.GetCurrentMethod()).MakeGenericMethod(typeof(TSource)), source.Expression, Expression.Constant(count, typeof(int)), Expression.Constant(skip, typeof(int))));
#endif
        }

        public static IAsyncQueryable<TSource> Catch<TSource, TException>(this IAsyncQueryable<TSource> source, Expression<Func<TException, IAsyncEnumerable<TSource>>> handler)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));
            if (handler == null)
                throw new ArgumentNullException(nameof(handler));

#if CRIPPLED_REFLECTION
            return source.Provider.CreateQuery<TSource>(Expression.Call(InfoOf(() => AsyncQueryableEx.Catch<TSource, TException>(default(IAsyncQueryable<TSource>), default(Expression<Func<TException, IAsyncEnumerable<TSource>>>))), source.Expression, handler));
#else
            return source.Provider.CreateQuery<TSource>(Expression.Call(((MethodInfo)MethodBase.GetCurrentMethod()).MakeGenericMethod(typeof(TSource), typeof(TException)), source.Expression, handler));
#endif
        }

        public static IAsyncQueryable<TSource> Catch<TSource, TException>(this IAsyncQueryable<TSource> source, Expression<Func<TException, Task<IAsyncEnumerable<TSource>>>> handler)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));
            if (handler == null)
                throw new ArgumentNullException(nameof(handler));

#if CRIPPLED_REFLECTION
            return source.Provider.CreateQuery<TSource>(Expression.Call(InfoOf(() => AsyncQueryableEx.Catch<TSource, TException>(default(IAsyncQueryable<TSource>), default(Expression<Func<TException, Task<IAsyncEnumerable<TSource>>>>))), source.Expression, handler));
#else
            return source.Provider.CreateQuery<TSource>(Expression.Call(((MethodInfo)MethodBase.GetCurrentMethod()).MakeGenericMethod(typeof(TSource), typeof(TException)), source.Expression, handler));
#endif
        }

        public static IAsyncQueryable<TSource> Catch<TSource>(this IAsyncQueryable<TSource> first, IAsyncEnumerable<TSource> second)
        {
            if (first == null)
                throw new ArgumentNullException(nameof(first));
            if (second == null)
                throw new ArgumentNullException(nameof(second));

#if CRIPPLED_REFLECTION
            return first.Provider.CreateQuery<TSource>(Expression.Call(InfoOf(() => AsyncQueryableEx.Catch<TSource>(default(IAsyncQueryable<TSource>), default(IAsyncEnumerable<TSource>))), first.Expression, GetSourceExpression(second)));
#else
            return first.Provider.CreateQuery<TSource>(Expression.Call(((MethodInfo)MethodBase.GetCurrentMethod()).MakeGenericMethod(typeof(TSource)), first.Expression, GetSourceExpression(second)));
#endif
        }

        public static IAsyncQueryable<TSource> Distinct<TSource, TKey>(this IAsyncQueryable<TSource> source, Expression<Func<TSource, TKey>> keySelector)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));
            if (keySelector == null)
                throw new ArgumentNullException(nameof(keySelector));

#if CRIPPLED_REFLECTION
            return source.Provider.CreateQuery<TSource>(Expression.Call(InfoOf(() => AsyncQueryableEx.Distinct<TSource, TKey>(default(IAsyncQueryable<TSource>), default(Expression<Func<TSource, TKey>>))), source.Expression, keySelector));
#else
            return source.Provider.CreateQuery<TSource>(Expression.Call(((MethodInfo)MethodBase.GetCurrentMethod()).MakeGenericMethod(typeof(TSource), typeof(TKey)), source.Expression, keySelector));
#endif
        }

        public static IAsyncQueryable<TSource> Distinct<TSource, TKey>(this IAsyncQueryable<TSource> source, Expression<Func<TSource, Task<TKey>>> keySelector)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));
            if (keySelector == null)
                throw new ArgumentNullException(nameof(keySelector));

#if CRIPPLED_REFLECTION
            return source.Provider.CreateQuery<TSource>(Expression.Call(InfoOf(() => AsyncQueryableEx.Distinct<TSource, TKey>(default(IAsyncQueryable<TSource>), default(Expression<Func<TSource, Task<TKey>>>))), source.Expression, keySelector));
#else
            return source.Provider.CreateQuery<TSource>(Expression.Call(((MethodInfo)MethodBase.GetCurrentMethod()).MakeGenericMethod(typeof(TSource), typeof(TKey)), source.Expression, keySelector));
#endif
        }

        public static IAsyncQueryable<TSource> Distinct<TSource, TKey>(this IAsyncQueryable<TSource> source, Expression<Func<TSource, TKey>> keySelector, IEqualityComparer<TKey> comparer)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));
            if (keySelector == null)
                throw new ArgumentNullException(nameof(keySelector));
            if (comparer == null)
                throw new ArgumentNullException(nameof(comparer));

#if CRIPPLED_REFLECTION
            return source.Provider.CreateQuery<TSource>(Expression.Call(InfoOf(() => AsyncQueryableEx.Distinct<TSource, TKey>(default(IAsyncQueryable<TSource>), default(Expression<Func<TSource, TKey>>), default(IEqualityComparer<TKey>))), source.Expression, keySelector, Expression.Constant(comparer, typeof(IEqualityComparer<TKey>))));
#else
            return source.Provider.CreateQuery<TSource>(Expression.Call(((MethodInfo)MethodBase.GetCurrentMethod()).MakeGenericMethod(typeof(TSource), typeof(TKey)), source.Expression, keySelector, Expression.Constant(comparer, typeof(IEqualityComparer<TKey>))));
#endif
        }

        public static IAsyncQueryable<TSource> Distinct<TSource, TKey>(this IAsyncQueryable<TSource> source, Expression<Func<TSource, Task<TKey>>> keySelector, IEqualityComparer<TKey> comparer)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));
            if (keySelector == null)
                throw new ArgumentNullException(nameof(keySelector));
            if (comparer == null)
                throw new ArgumentNullException(nameof(comparer));

#if CRIPPLED_REFLECTION
            return source.Provider.CreateQuery<TSource>(Expression.Call(InfoOf(() => AsyncQueryableEx.Distinct<TSource, TKey>(default(IAsyncQueryable<TSource>), default(Expression<Func<TSource, Task<TKey>>>), default(IEqualityComparer<TKey>))), source.Expression, keySelector, Expression.Constant(comparer, typeof(IEqualityComparer<TKey>))));
#else
            return source.Provider.CreateQuery<TSource>(Expression.Call(((MethodInfo)MethodBase.GetCurrentMethod()).MakeGenericMethod(typeof(TSource), typeof(TKey)), source.Expression, keySelector, Expression.Constant(comparer, typeof(IEqualityComparer<TKey>))));
#endif
        }

        public static IAsyncQueryable<TSource> DistinctUntilChanged<TSource>(this IAsyncQueryable<TSource> source)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));

#if CRIPPLED_REFLECTION
            return source.Provider.CreateQuery<TSource>(Expression.Call(InfoOf(() => AsyncQueryableEx.DistinctUntilChanged<TSource>(default(IAsyncQueryable<TSource>))), source.Expression));
#else
            return source.Provider.CreateQuery<TSource>(Expression.Call(((MethodInfo)MethodBase.GetCurrentMethod()).MakeGenericMethod(typeof(TSource)), source.Expression));
#endif
        }

        public static IAsyncQueryable<TSource> DistinctUntilChanged<TSource>(this IAsyncQueryable<TSource> source, IEqualityComparer<TSource> comparer)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));
            if (comparer == null)
                throw new ArgumentNullException(nameof(comparer));

#if CRIPPLED_REFLECTION
            return source.Provider.CreateQuery<TSource>(Expression.Call(InfoOf(() => AsyncQueryableEx.DistinctUntilChanged<TSource>(default(IAsyncQueryable<TSource>), default(IEqualityComparer<TSource>))), source.Expression, Expression.Constant(comparer, typeof(IEqualityComparer<TSource>))));
#else
            return source.Provider.CreateQuery<TSource>(Expression.Call(((MethodInfo)MethodBase.GetCurrentMethod()).MakeGenericMethod(typeof(TSource)), source.Expression, Expression.Constant(comparer, typeof(IEqualityComparer<TSource>))));
#endif
        }

        public static IAsyncQueryable<TSource> DistinctUntilChanged<TSource, TKey>(this IAsyncQueryable<TSource> source, Expression<Func<TSource, TKey>> keySelector)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));
            if (keySelector == null)
                throw new ArgumentNullException(nameof(keySelector));

#if CRIPPLED_REFLECTION
            return source.Provider.CreateQuery<TSource>(Expression.Call(InfoOf(() => AsyncQueryableEx.DistinctUntilChanged<TSource, TKey>(default(IAsyncQueryable<TSource>), default(Expression<Func<TSource, TKey>>))), source.Expression, keySelector));
#else
            return source.Provider.CreateQuery<TSource>(Expression.Call(((MethodInfo)MethodBase.GetCurrentMethod()).MakeGenericMethod(typeof(TSource), typeof(TKey)), source.Expression, keySelector));
#endif
        }

        public static IAsyncQueryable<TSource> DistinctUntilChanged<TSource, TKey>(this IAsyncQueryable<TSource> source, Expression<Func<TSource, Task<TKey>>> keySelector)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));
            if (keySelector == null)
                throw new ArgumentNullException(nameof(keySelector));

#if CRIPPLED_REFLECTION
            return source.Provider.CreateQuery<TSource>(Expression.Call(InfoOf(() => AsyncQueryableEx.DistinctUntilChanged<TSource, TKey>(default(IAsyncQueryable<TSource>), default(Expression<Func<TSource, Task<TKey>>>))), source.Expression, keySelector));
#else
            return source.Provider.CreateQuery<TSource>(Expression.Call(((MethodInfo)MethodBase.GetCurrentMethod()).MakeGenericMethod(typeof(TSource), typeof(TKey)), source.Expression, keySelector));
#endif
        }

        public static IAsyncQueryable<TSource> DistinctUntilChanged<TSource, TKey>(this IAsyncQueryable<TSource> source, Expression<Func<TSource, TKey>> keySelector, IEqualityComparer<TKey> comparer)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));
            if (keySelector == null)
                throw new ArgumentNullException(nameof(keySelector));
            if (comparer == null)
                throw new ArgumentNullException(nameof(comparer));

#if CRIPPLED_REFLECTION
            return source.Provider.CreateQuery<TSource>(Expression.Call(InfoOf(() => AsyncQueryableEx.DistinctUntilChanged<TSource, TKey>(default(IAsyncQueryable<TSource>), default(Expression<Func<TSource, TKey>>), default(IEqualityComparer<TKey>))), source.Expression, keySelector, Expression.Constant(comparer, typeof(IEqualityComparer<TKey>))));
#else
            return source.Provider.CreateQuery<TSource>(Expression.Call(((MethodInfo)MethodBase.GetCurrentMethod()).MakeGenericMethod(typeof(TSource), typeof(TKey)), source.Expression, keySelector, Expression.Constant(comparer, typeof(IEqualityComparer<TKey>))));
#endif
        }

        public static IAsyncQueryable<TSource> DistinctUntilChanged<TSource, TKey>(this IAsyncQueryable<TSource> source, Expression<Func<TSource, Task<TKey>>> keySelector, IEqualityComparer<TKey> comparer)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));
            if (keySelector == null)
                throw new ArgumentNullException(nameof(keySelector));
            if (comparer == null)
                throw new ArgumentNullException(nameof(comparer));

#if CRIPPLED_REFLECTION
            return source.Provider.CreateQuery<TSource>(Expression.Call(InfoOf(() => AsyncQueryableEx.DistinctUntilChanged<TSource, TKey>(default(IAsyncQueryable<TSource>), default(Expression<Func<TSource, Task<TKey>>>), default(IEqualityComparer<TKey>))), source.Expression, keySelector, Expression.Constant(comparer, typeof(IEqualityComparer<TKey>))));
#else
            return source.Provider.CreateQuery<TSource>(Expression.Call(((MethodInfo)MethodBase.GetCurrentMethod()).MakeGenericMethod(typeof(TSource), typeof(TKey)), source.Expression, keySelector, Expression.Constant(comparer, typeof(IEqualityComparer<TKey>))));
#endif
        }

        public static IAsyncQueryable<TSource> Do<TSource>(this IAsyncQueryable<TSource> source, Expression<Action<TSource>> onNext)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));
            if (onNext == null)
                throw new ArgumentNullException(nameof(onNext));

#if CRIPPLED_REFLECTION
            return source.Provider.CreateQuery<TSource>(Expression.Call(InfoOf(() => AsyncQueryableEx.Do<TSource>(default(IAsyncQueryable<TSource>), default(Expression<Action<TSource>>))), source.Expression, onNext));
#else
            return source.Provider.CreateQuery<TSource>(Expression.Call(((MethodInfo)MethodBase.GetCurrentMethod()).MakeGenericMethod(typeof(TSource)), source.Expression, onNext));
#endif
        }

        public static IAsyncQueryable<TSource> Do<TSource>(this IAsyncQueryable<TSource> source, Expression<Func<TSource, Task>> onNext)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));
            if (onNext == null)
                throw new ArgumentNullException(nameof(onNext));

#if CRIPPLED_REFLECTION
            return source.Provider.CreateQuery<TSource>(Expression.Call(InfoOf(() => AsyncQueryableEx.Do<TSource>(default(IAsyncQueryable<TSource>), default(Expression<Func<TSource, Task>>))), source.Expression, onNext));
#else
            return source.Provider.CreateQuery<TSource>(Expression.Call(((MethodInfo)MethodBase.GetCurrentMethod()).MakeGenericMethod(typeof(TSource)), source.Expression, onNext));
#endif
        }

        public static IAsyncQueryable<TSource> Do<TSource>(this IAsyncQueryable<TSource> source, IObserver<TSource> observer)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));
            if (observer == null)
                throw new ArgumentNullException(nameof(observer));

#if CRIPPLED_REFLECTION
            return source.Provider.CreateQuery<TSource>(Expression.Call(InfoOf(() => AsyncQueryableEx.Do<TSource>(default(IAsyncQueryable<TSource>), default(IObserver<TSource>))), source.Expression, Expression.Constant(observer, typeof(IObserver<TSource>))));
#else
            return source.Provider.CreateQuery<TSource>(Expression.Call(((MethodInfo)MethodBase.GetCurrentMethod()).MakeGenericMethod(typeof(TSource)), source.Expression, Expression.Constant(observer, typeof(IObserver<TSource>))));
#endif
        }

        public static IAsyncQueryable<TSource> Do<TSource>(this IAsyncQueryable<TSource> source, Expression<Action<TSource>> onNext, Action onCompleted)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));
            if (onNext == null)
                throw new ArgumentNullException(nameof(onNext));
            if (onCompleted == null)
                throw new ArgumentNullException(nameof(onCompleted));

#if CRIPPLED_REFLECTION
            return source.Provider.CreateQuery<TSource>(Expression.Call(InfoOf(() => AsyncQueryableEx.Do<TSource>(default(IAsyncQueryable<TSource>), default(Expression<Action<TSource>>), default(Action))), source.Expression, onNext, Expression.Constant(onCompleted, typeof(Action))));
#else
            return source.Provider.CreateQuery<TSource>(Expression.Call(((MethodInfo)MethodBase.GetCurrentMethod()).MakeGenericMethod(typeof(TSource)), source.Expression, onNext, Expression.Constant(onCompleted, typeof(Action))));
#endif
        }

        public static IAsyncQueryable<TSource> Do<TSource>(this IAsyncQueryable<TSource> source, Expression<Action<TSource>> onNext, Expression<Action<Exception>> onError)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));
            if (onNext == null)
                throw new ArgumentNullException(nameof(onNext));
            if (onError == null)
                throw new ArgumentNullException(nameof(onError));

#if CRIPPLED_REFLECTION
            return source.Provider.CreateQuery<TSource>(Expression.Call(InfoOf(() => AsyncQueryableEx.Do<TSource>(default(IAsyncQueryable<TSource>), default(Expression<Action<TSource>>), default(Expression<Action<Exception>>))), source.Expression, onNext, onError));
#else
            return source.Provider.CreateQuery<TSource>(Expression.Call(((MethodInfo)MethodBase.GetCurrentMethod()).MakeGenericMethod(typeof(TSource)), source.Expression, onNext, onError));
#endif
        }

        public static IAsyncQueryable<TSource> Do<TSource>(this IAsyncQueryable<TSource> source, Expression<Func<TSource, Task>> onNext, Expression<Func<Task>> onCompleted)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));
            if (onNext == null)
                throw new ArgumentNullException(nameof(onNext));
            if (onCompleted == null)
                throw new ArgumentNullException(nameof(onCompleted));

#if CRIPPLED_REFLECTION
            return source.Provider.CreateQuery<TSource>(Expression.Call(InfoOf(() => AsyncQueryableEx.Do<TSource>(default(IAsyncQueryable<TSource>), default(Expression<Func<TSource, Task>>), default(Expression<Func<Task>>))), source.Expression, onNext, onCompleted));
#else
            return source.Provider.CreateQuery<TSource>(Expression.Call(((MethodInfo)MethodBase.GetCurrentMethod()).MakeGenericMethod(typeof(TSource)), source.Expression, onNext, onCompleted));
#endif
        }

        public static IAsyncQueryable<TSource> Do<TSource>(this IAsyncQueryable<TSource> source, Expression<Func<TSource, Task>> onNext, Expression<Func<Exception, Task>> onError)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));
            if (onNext == null)
                throw new ArgumentNullException(nameof(onNext));
            if (onError == null)
                throw new ArgumentNullException(nameof(onError));

#if CRIPPLED_REFLECTION
            return source.Provider.CreateQuery<TSource>(Expression.Call(InfoOf(() => AsyncQueryableEx.Do<TSource>(default(IAsyncQueryable<TSource>), default(Expression<Func<TSource, Task>>), default(Expression<Func<Exception, Task>>))), source.Expression, onNext, onError));
#else
            return source.Provider.CreateQuery<TSource>(Expression.Call(((MethodInfo)MethodBase.GetCurrentMethod()).MakeGenericMethod(typeof(TSource)), source.Expression, onNext, onError));
#endif
        }

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

#if CRIPPLED_REFLECTION
            return source.Provider.CreateQuery<TSource>(Expression.Call(InfoOf(() => AsyncQueryableEx.Do<TSource>(default(IAsyncQueryable<TSource>), default(Expression<Action<TSource>>), default(Expression<Action<Exception>>), default(Action))), source.Expression, onNext, onError, Expression.Constant(onCompleted, typeof(Action))));
#else
            return source.Provider.CreateQuery<TSource>(Expression.Call(((MethodInfo)MethodBase.GetCurrentMethod()).MakeGenericMethod(typeof(TSource)), source.Expression, onNext, onError, Expression.Constant(onCompleted, typeof(Action))));
#endif
        }

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

#if CRIPPLED_REFLECTION
            return source.Provider.CreateQuery<TSource>(Expression.Call(InfoOf(() => AsyncQueryableEx.Do<TSource>(default(IAsyncQueryable<TSource>), default(Expression<Func<TSource, Task>>), default(Expression<Func<Exception, Task>>), default(Expression<Func<Task>>))), source.Expression, onNext, onError, onCompleted));
#else
            return source.Provider.CreateQuery<TSource>(Expression.Call(((MethodInfo)MethodBase.GetCurrentMethod()).MakeGenericMethod(typeof(TSource)), source.Expression, onNext, onError, onCompleted));
#endif
        }

        public static IAsyncQueryable<TSource> Expand<TSource>(this IAsyncQueryable<TSource> source, Expression<Func<TSource, IAsyncEnumerable<TSource>>> selector)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));
            if (selector == null)
                throw new ArgumentNullException(nameof(selector));

#if CRIPPLED_REFLECTION
            return source.Provider.CreateQuery<TSource>(Expression.Call(InfoOf(() => AsyncQueryableEx.Expand<TSource>(default(IAsyncQueryable<TSource>), default(Expression<Func<TSource, IAsyncEnumerable<TSource>>>))), source.Expression, selector));
#else
            return source.Provider.CreateQuery<TSource>(Expression.Call(((MethodInfo)MethodBase.GetCurrentMethod()).MakeGenericMethod(typeof(TSource)), source.Expression, selector));
#endif
        }

        public static IAsyncQueryable<TSource> Expand<TSource>(this IAsyncQueryable<TSource> source, Expression<Func<TSource, Task<IAsyncEnumerable<TSource>>>> selector)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));
            if (selector == null)
                throw new ArgumentNullException(nameof(selector));

#if CRIPPLED_REFLECTION
            return source.Provider.CreateQuery<TSource>(Expression.Call(InfoOf(() => AsyncQueryableEx.Expand<TSource>(default(IAsyncQueryable<TSource>), default(Expression<Func<TSource, Task<IAsyncEnumerable<TSource>>>>))), source.Expression, selector));
#else
            return source.Provider.CreateQuery<TSource>(Expression.Call(((MethodInfo)MethodBase.GetCurrentMethod()).MakeGenericMethod(typeof(TSource)), source.Expression, selector));
#endif
        }

        public static IAsyncQueryable<TSource> Finally<TSource>(this IAsyncQueryable<TSource> source, Action finallyAction)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));
            if (finallyAction == null)
                throw new ArgumentNullException(nameof(finallyAction));

#if CRIPPLED_REFLECTION
            return source.Provider.CreateQuery<TSource>(Expression.Call(InfoOf(() => AsyncQueryableEx.Finally<TSource>(default(IAsyncQueryable<TSource>), default(Action))), source.Expression, Expression.Constant(finallyAction, typeof(Action))));
#else
            return source.Provider.CreateQuery<TSource>(Expression.Call(((MethodInfo)MethodBase.GetCurrentMethod()).MakeGenericMethod(typeof(TSource)), source.Expression, Expression.Constant(finallyAction, typeof(Action))));
#endif
        }

        public static IAsyncQueryable<TSource> Finally<TSource>(this IAsyncQueryable<TSource> source, Expression<Func<Task>> finallyAction)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));
            if (finallyAction == null)
                throw new ArgumentNullException(nameof(finallyAction));

#if CRIPPLED_REFLECTION
            return source.Provider.CreateQuery<TSource>(Expression.Call(InfoOf(() => AsyncQueryableEx.Finally<TSource>(default(IAsyncQueryable<TSource>), default(Expression<Func<Task>>))), source.Expression, finallyAction));
#else
            return source.Provider.CreateQuery<TSource>(Expression.Call(((MethodInfo)MethodBase.GetCurrentMethod()).MakeGenericMethod(typeof(TSource)), source.Expression, finallyAction));
#endif
        }

        public static IAsyncQueryable<TSource> IgnoreElements<TSource>(this IAsyncQueryable<TSource> source)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));

#if CRIPPLED_REFLECTION
            return source.Provider.CreateQuery<TSource>(Expression.Call(InfoOf(() => AsyncQueryableEx.IgnoreElements<TSource>(default(IAsyncQueryable<TSource>))), source.Expression));
#else
            return source.Provider.CreateQuery<TSource>(Expression.Call(((MethodInfo)MethodBase.GetCurrentMethod()).MakeGenericMethod(typeof(TSource)), source.Expression));
#endif
        }

        public static Task<IList<TSource>> MaxBy<TSource, TKey>(this IAsyncQueryable<TSource> source, Expression<Func<TSource, TKey>> keySelector)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));
            if (keySelector == null)
                throw new ArgumentNullException(nameof(keySelector));

#if CRIPPLED_REFLECTION
            return source.Provider.ExecuteAsync<IList<TSource>>(Expression.Call(InfoOf(() => AsyncQueryableEx.MaxBy<TSource, TKey>(default(IAsyncQueryable<TSource>), default(Expression<Func<TSource, TKey>>))), source.Expression, keySelector), CancellationToken.None);
#else
            return source.Provider.ExecuteAsync<IList<TSource>>(Expression.Call(((MethodInfo)MethodBase.GetCurrentMethod()).MakeGenericMethod(typeof(TSource), typeof(TKey)), source.Expression, keySelector), CancellationToken.None);
#endif
        }

        public static Task<IList<TSource>> MaxBy<TSource, TKey>(this IAsyncQueryable<TSource> source, Expression<Func<TSource, Task<TKey>>> keySelector)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));
            if (keySelector == null)
                throw new ArgumentNullException(nameof(keySelector));

#if CRIPPLED_REFLECTION
            return source.Provider.ExecuteAsync<IList<TSource>>(Expression.Call(InfoOf(() => AsyncQueryableEx.MaxBy<TSource, TKey>(default(IAsyncQueryable<TSource>), default(Expression<Func<TSource, Task<TKey>>>))), source.Expression, keySelector), CancellationToken.None);
#else
            return source.Provider.ExecuteAsync<IList<TSource>>(Expression.Call(((MethodInfo)MethodBase.GetCurrentMethod()).MakeGenericMethod(typeof(TSource), typeof(TKey)), source.Expression, keySelector), CancellationToken.None);
#endif
        }

        public static Task<IList<TSource>> MaxBy<TSource, TKey>(this IAsyncQueryable<TSource> source, Expression<Func<TSource, TKey>> keySelector, CancellationToken cancellationToken)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));
            if (keySelector == null)
                throw new ArgumentNullException(nameof(keySelector));

#if CRIPPLED_REFLECTION
            return source.Provider.ExecuteAsync<IList<TSource>>(Expression.Call(InfoOf(() => AsyncQueryableEx.MaxBy<TSource, TKey>(default(IAsyncQueryable<TSource>), default(Expression<Func<TSource, TKey>>), default(CancellationToken))), source.Expression, keySelector, Expression.Constant(cancellationToken, typeof(CancellationToken))), cancellationToken);
#else
            return source.Provider.ExecuteAsync<IList<TSource>>(Expression.Call(((MethodInfo)MethodBase.GetCurrentMethod()).MakeGenericMethod(typeof(TSource), typeof(TKey)), source.Expression, keySelector, Expression.Constant(cancellationToken, typeof(CancellationToken))), cancellationToken);
#endif
        }

        public static Task<IList<TSource>> MaxBy<TSource, TKey>(this IAsyncQueryable<TSource> source, Expression<Func<TSource, TKey>> keySelector, IComparer<TKey> comparer)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));
            if (keySelector == null)
                throw new ArgumentNullException(nameof(keySelector));
            if (comparer == null)
                throw new ArgumentNullException(nameof(comparer));

#if CRIPPLED_REFLECTION
            return source.Provider.ExecuteAsync<IList<TSource>>(Expression.Call(InfoOf(() => AsyncQueryableEx.MaxBy<TSource, TKey>(default(IAsyncQueryable<TSource>), default(Expression<Func<TSource, TKey>>), default(IComparer<TKey>))), source.Expression, keySelector, Expression.Constant(comparer, typeof(IComparer<TKey>))), CancellationToken.None);
#else
            return source.Provider.ExecuteAsync<IList<TSource>>(Expression.Call(((MethodInfo)MethodBase.GetCurrentMethod()).MakeGenericMethod(typeof(TSource), typeof(TKey)), source.Expression, keySelector, Expression.Constant(comparer, typeof(IComparer<TKey>))), CancellationToken.None);
#endif
        }

        public static Task<IList<TSource>> MaxBy<TSource, TKey>(this IAsyncQueryable<TSource> source, Expression<Func<TSource, Task<TKey>>> keySelector, CancellationToken cancellationToken)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));
            if (keySelector == null)
                throw new ArgumentNullException(nameof(keySelector));

#if CRIPPLED_REFLECTION
            return source.Provider.ExecuteAsync<IList<TSource>>(Expression.Call(InfoOf(() => AsyncQueryableEx.MaxBy<TSource, TKey>(default(IAsyncQueryable<TSource>), default(Expression<Func<TSource, Task<TKey>>>), default(CancellationToken))), source.Expression, keySelector, Expression.Constant(cancellationToken, typeof(CancellationToken))), cancellationToken);
#else
            return source.Provider.ExecuteAsync<IList<TSource>>(Expression.Call(((MethodInfo)MethodBase.GetCurrentMethod()).MakeGenericMethod(typeof(TSource), typeof(TKey)), source.Expression, keySelector, Expression.Constant(cancellationToken, typeof(CancellationToken))), cancellationToken);
#endif
        }

        public static Task<IList<TSource>> MaxBy<TSource, TKey>(this IAsyncQueryable<TSource> source, Expression<Func<TSource, Task<TKey>>> keySelector, IComparer<TKey> comparer)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));
            if (keySelector == null)
                throw new ArgumentNullException(nameof(keySelector));
            if (comparer == null)
                throw new ArgumentNullException(nameof(comparer));

#if CRIPPLED_REFLECTION
            return source.Provider.ExecuteAsync<IList<TSource>>(Expression.Call(InfoOf(() => AsyncQueryableEx.MaxBy<TSource, TKey>(default(IAsyncQueryable<TSource>), default(Expression<Func<TSource, Task<TKey>>>), default(IComparer<TKey>))), source.Expression, keySelector, Expression.Constant(comparer, typeof(IComparer<TKey>))), CancellationToken.None);
#else
            return source.Provider.ExecuteAsync<IList<TSource>>(Expression.Call(((MethodInfo)MethodBase.GetCurrentMethod()).MakeGenericMethod(typeof(TSource), typeof(TKey)), source.Expression, keySelector, Expression.Constant(comparer, typeof(IComparer<TKey>))), CancellationToken.None);
#endif
        }

        public static Task<IList<TSource>> MaxBy<TSource, TKey>(this IAsyncQueryable<TSource> source, Expression<Func<TSource, TKey>> keySelector, IComparer<TKey> comparer, CancellationToken cancellationToken)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));
            if (keySelector == null)
                throw new ArgumentNullException(nameof(keySelector));
            if (comparer == null)
                throw new ArgumentNullException(nameof(comparer));

#if CRIPPLED_REFLECTION
            return source.Provider.ExecuteAsync<IList<TSource>>(Expression.Call(InfoOf(() => AsyncQueryableEx.MaxBy<TSource, TKey>(default(IAsyncQueryable<TSource>), default(Expression<Func<TSource, TKey>>), default(IComparer<TKey>), default(CancellationToken))), source.Expression, keySelector, Expression.Constant(comparer, typeof(IComparer<TKey>)), Expression.Constant(cancellationToken, typeof(CancellationToken))), cancellationToken);
#else
            return source.Provider.ExecuteAsync<IList<TSource>>(Expression.Call(((MethodInfo)MethodBase.GetCurrentMethod()).MakeGenericMethod(typeof(TSource), typeof(TKey)), source.Expression, keySelector, Expression.Constant(comparer, typeof(IComparer<TKey>)), Expression.Constant(cancellationToken, typeof(CancellationToken))), cancellationToken);
#endif
        }

        public static Task<IList<TSource>> MaxBy<TSource, TKey>(this IAsyncQueryable<TSource> source, Expression<Func<TSource, Task<TKey>>> keySelector, IComparer<TKey> comparer, CancellationToken cancellationToken)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));
            if (keySelector == null)
                throw new ArgumentNullException(nameof(keySelector));
            if (comparer == null)
                throw new ArgumentNullException(nameof(comparer));

#if CRIPPLED_REFLECTION
            return source.Provider.ExecuteAsync<IList<TSource>>(Expression.Call(InfoOf(() => AsyncQueryableEx.MaxBy<TSource, TKey>(default(IAsyncQueryable<TSource>), default(Expression<Func<TSource, Task<TKey>>>), default(IComparer<TKey>), default(CancellationToken))), source.Expression, keySelector, Expression.Constant(comparer, typeof(IComparer<TKey>)), Expression.Constant(cancellationToken, typeof(CancellationToken))), cancellationToken);
#else
            return source.Provider.ExecuteAsync<IList<TSource>>(Expression.Call(((MethodInfo)MethodBase.GetCurrentMethod()).MakeGenericMethod(typeof(TSource), typeof(TKey)), source.Expression, keySelector, Expression.Constant(comparer, typeof(IComparer<TKey>)), Expression.Constant(cancellationToken, typeof(CancellationToken))), cancellationToken);
#endif
        }

        public static Task<IList<TSource>> MinBy<TSource, TKey>(this IAsyncQueryable<TSource> source, Expression<Func<TSource, Task<TKey>>> keySelector)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));
            if (keySelector == null)
                throw new ArgumentNullException(nameof(keySelector));

#if CRIPPLED_REFLECTION
            return source.Provider.ExecuteAsync<IList<TSource>>(Expression.Call(InfoOf(() => AsyncQueryableEx.MinBy<TSource, TKey>(default(IAsyncQueryable<TSource>), default(Expression<Func<TSource, Task<TKey>>>))), source.Expression, keySelector), CancellationToken.None);
#else
            return source.Provider.ExecuteAsync<IList<TSource>>(Expression.Call(((MethodInfo)MethodBase.GetCurrentMethod()).MakeGenericMethod(typeof(TSource), typeof(TKey)), source.Expression, keySelector), CancellationToken.None);
#endif
        }

        public static Task<IList<TSource>> MinBy<TSource, TKey>(this IAsyncQueryable<TSource> source, Expression<Func<TSource, TKey>> keySelector)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));
            if (keySelector == null)
                throw new ArgumentNullException(nameof(keySelector));

#if CRIPPLED_REFLECTION
            return source.Provider.ExecuteAsync<IList<TSource>>(Expression.Call(InfoOf(() => AsyncQueryableEx.MinBy<TSource, TKey>(default(IAsyncQueryable<TSource>), default(Expression<Func<TSource, TKey>>))), source.Expression, keySelector), CancellationToken.None);
#else
            return source.Provider.ExecuteAsync<IList<TSource>>(Expression.Call(((MethodInfo)MethodBase.GetCurrentMethod()).MakeGenericMethod(typeof(TSource), typeof(TKey)), source.Expression, keySelector), CancellationToken.None);
#endif
        }

        public static Task<IList<TSource>> MinBy<TSource, TKey>(this IAsyncQueryable<TSource> source, Expression<Func<TSource, Task<TKey>>> keySelector, CancellationToken cancellationToken)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));
            if (keySelector == null)
                throw new ArgumentNullException(nameof(keySelector));

#if CRIPPLED_REFLECTION
            return source.Provider.ExecuteAsync<IList<TSource>>(Expression.Call(InfoOf(() => AsyncQueryableEx.MinBy<TSource, TKey>(default(IAsyncQueryable<TSource>), default(Expression<Func<TSource, Task<TKey>>>), default(CancellationToken))), source.Expression, keySelector, Expression.Constant(cancellationToken, typeof(CancellationToken))), cancellationToken);
#else
            return source.Provider.ExecuteAsync<IList<TSource>>(Expression.Call(((MethodInfo)MethodBase.GetCurrentMethod()).MakeGenericMethod(typeof(TSource), typeof(TKey)), source.Expression, keySelector, Expression.Constant(cancellationToken, typeof(CancellationToken))), cancellationToken);
#endif
        }

        public static Task<IList<TSource>> MinBy<TSource, TKey>(this IAsyncQueryable<TSource> source, Expression<Func<TSource, Task<TKey>>> keySelector, IComparer<TKey> comparer)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));
            if (keySelector == null)
                throw new ArgumentNullException(nameof(keySelector));
            if (comparer == null)
                throw new ArgumentNullException(nameof(comparer));

#if CRIPPLED_REFLECTION
            return source.Provider.ExecuteAsync<IList<TSource>>(Expression.Call(InfoOf(() => AsyncQueryableEx.MinBy<TSource, TKey>(default(IAsyncQueryable<TSource>), default(Expression<Func<TSource, Task<TKey>>>), default(IComparer<TKey>))), source.Expression, keySelector, Expression.Constant(comparer, typeof(IComparer<TKey>))), CancellationToken.None);
#else
            return source.Provider.ExecuteAsync<IList<TSource>>(Expression.Call(((MethodInfo)MethodBase.GetCurrentMethod()).MakeGenericMethod(typeof(TSource), typeof(TKey)), source.Expression, keySelector, Expression.Constant(comparer, typeof(IComparer<TKey>))), CancellationToken.None);
#endif
        }

        public static Task<IList<TSource>> MinBy<TSource, TKey>(this IAsyncQueryable<TSource> source, Expression<Func<TSource, TKey>> keySelector, CancellationToken cancellationToken)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));
            if (keySelector == null)
                throw new ArgumentNullException(nameof(keySelector));

#if CRIPPLED_REFLECTION
            return source.Provider.ExecuteAsync<IList<TSource>>(Expression.Call(InfoOf(() => AsyncQueryableEx.MinBy<TSource, TKey>(default(IAsyncQueryable<TSource>), default(Expression<Func<TSource, TKey>>), default(CancellationToken))), source.Expression, keySelector, Expression.Constant(cancellationToken, typeof(CancellationToken))), cancellationToken);
#else
            return source.Provider.ExecuteAsync<IList<TSource>>(Expression.Call(((MethodInfo)MethodBase.GetCurrentMethod()).MakeGenericMethod(typeof(TSource), typeof(TKey)), source.Expression, keySelector, Expression.Constant(cancellationToken, typeof(CancellationToken))), cancellationToken);
#endif
        }

        public static Task<IList<TSource>> MinBy<TSource, TKey>(this IAsyncQueryable<TSource> source, Expression<Func<TSource, TKey>> keySelector, IComparer<TKey> comparer)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));
            if (keySelector == null)
                throw new ArgumentNullException(nameof(keySelector));
            if (comparer == null)
                throw new ArgumentNullException(nameof(comparer));

#if CRIPPLED_REFLECTION
            return source.Provider.ExecuteAsync<IList<TSource>>(Expression.Call(InfoOf(() => AsyncQueryableEx.MinBy<TSource, TKey>(default(IAsyncQueryable<TSource>), default(Expression<Func<TSource, TKey>>), default(IComparer<TKey>))), source.Expression, keySelector, Expression.Constant(comparer, typeof(IComparer<TKey>))), CancellationToken.None);
#else
            return source.Provider.ExecuteAsync<IList<TSource>>(Expression.Call(((MethodInfo)MethodBase.GetCurrentMethod()).MakeGenericMethod(typeof(TSource), typeof(TKey)), source.Expression, keySelector, Expression.Constant(comparer, typeof(IComparer<TKey>))), CancellationToken.None);
#endif
        }

        public static Task<IList<TSource>> MinBy<TSource, TKey>(this IAsyncQueryable<TSource> source, Expression<Func<TSource, TKey>> keySelector, IComparer<TKey> comparer, CancellationToken cancellationToken)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));
            if (keySelector == null)
                throw new ArgumentNullException(nameof(keySelector));
            if (comparer == null)
                throw new ArgumentNullException(nameof(comparer));

#if CRIPPLED_REFLECTION
            return source.Provider.ExecuteAsync<IList<TSource>>(Expression.Call(InfoOf(() => AsyncQueryableEx.MinBy<TSource, TKey>(default(IAsyncQueryable<TSource>), default(Expression<Func<TSource, TKey>>), default(IComparer<TKey>), default(CancellationToken))), source.Expression, keySelector, Expression.Constant(comparer, typeof(IComparer<TKey>)), Expression.Constant(cancellationToken, typeof(CancellationToken))), cancellationToken);
#else
            return source.Provider.ExecuteAsync<IList<TSource>>(Expression.Call(((MethodInfo)MethodBase.GetCurrentMethod()).MakeGenericMethod(typeof(TSource), typeof(TKey)), source.Expression, keySelector, Expression.Constant(comparer, typeof(IComparer<TKey>)), Expression.Constant(cancellationToken, typeof(CancellationToken))), cancellationToken);
#endif
        }

        public static Task<IList<TSource>> MinBy<TSource, TKey>(this IAsyncQueryable<TSource> source, Expression<Func<TSource, Task<TKey>>> keySelector, IComparer<TKey> comparer, CancellationToken cancellationToken)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));
            if (keySelector == null)
                throw new ArgumentNullException(nameof(keySelector));
            if (comparer == null)
                throw new ArgumentNullException(nameof(comparer));

#if CRIPPLED_REFLECTION
            return source.Provider.ExecuteAsync<IList<TSource>>(Expression.Call(InfoOf(() => AsyncQueryableEx.MinBy<TSource, TKey>(default(IAsyncQueryable<TSource>), default(Expression<Func<TSource, Task<TKey>>>), default(IComparer<TKey>), default(CancellationToken))), source.Expression, keySelector, Expression.Constant(comparer, typeof(IComparer<TKey>)), Expression.Constant(cancellationToken, typeof(CancellationToken))), cancellationToken);
#else
            return source.Provider.ExecuteAsync<IList<TSource>>(Expression.Call(((MethodInfo)MethodBase.GetCurrentMethod()).MakeGenericMethod(typeof(TSource), typeof(TKey)), source.Expression, keySelector, Expression.Constant(comparer, typeof(IComparer<TKey>)), Expression.Constant(cancellationToken, typeof(CancellationToken))), cancellationToken);
#endif
        }

        public static IAsyncQueryable<TSource> OnErrorResumeNext<TSource>(this IAsyncQueryable<TSource> first, IAsyncEnumerable<TSource> second)
        {
            if (first == null)
                throw new ArgumentNullException(nameof(first));
            if (second == null)
                throw new ArgumentNullException(nameof(second));

#if CRIPPLED_REFLECTION
            return first.Provider.CreateQuery<TSource>(Expression.Call(InfoOf(() => AsyncQueryableEx.OnErrorResumeNext<TSource>(default(IAsyncQueryable<TSource>), default(IAsyncEnumerable<TSource>))), first.Expression, GetSourceExpression(second)));
#else
            return first.Provider.CreateQuery<TSource>(Expression.Call(((MethodInfo)MethodBase.GetCurrentMethod()).MakeGenericMethod(typeof(TSource)), first.Expression, GetSourceExpression(second)));
#endif
        }

        public static IAsyncQueryable<TSource> Retry<TSource>(this IAsyncQueryable<TSource> source)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));

#if CRIPPLED_REFLECTION
            return source.Provider.CreateQuery<TSource>(Expression.Call(InfoOf(() => AsyncQueryableEx.Retry<TSource>(default(IAsyncQueryable<TSource>))), source.Expression));
#else
            return source.Provider.CreateQuery<TSource>(Expression.Call(((MethodInfo)MethodBase.GetCurrentMethod()).MakeGenericMethod(typeof(TSource)), source.Expression));
#endif
        }

        public static IAsyncQueryable<TSource> Retry<TSource>(this IAsyncQueryable<TSource> source, int retryCount)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));

#if CRIPPLED_REFLECTION
            return source.Provider.CreateQuery<TSource>(Expression.Call(InfoOf(() => AsyncQueryableEx.Retry<TSource>(default(IAsyncQueryable<TSource>), default(int))), source.Expression, Expression.Constant(retryCount, typeof(int))));
#else
            return source.Provider.CreateQuery<TSource>(Expression.Call(((MethodInfo)MethodBase.GetCurrentMethod()).MakeGenericMethod(typeof(TSource)), source.Expression, Expression.Constant(retryCount, typeof(int))));
#endif
        }

        public static IAsyncQueryable<TSource> Scan<TSource>(this IAsyncQueryable<TSource> source, Expression<Func<TSource, TSource, TSource>> accumulator)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));
            if (accumulator == null)
                throw new ArgumentNullException(nameof(accumulator));

#if CRIPPLED_REFLECTION
            return source.Provider.CreateQuery<TSource>(Expression.Call(InfoOf(() => AsyncQueryableEx.Scan<TSource>(default(IAsyncQueryable<TSource>), default(Expression<Func<TSource, TSource, TSource>>))), source.Expression, accumulator));
#else
            return source.Provider.CreateQuery<TSource>(Expression.Call(((MethodInfo)MethodBase.GetCurrentMethod()).MakeGenericMethod(typeof(TSource)), source.Expression, accumulator));
#endif
        }

        public static IAsyncQueryable<TSource> Scan<TSource>(this IAsyncQueryable<TSource> source, Expression<Func<TSource, TSource, Task<TSource>>> accumulator)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));
            if (accumulator == null)
                throw new ArgumentNullException(nameof(accumulator));

#if CRIPPLED_REFLECTION
            return source.Provider.CreateQuery<TSource>(Expression.Call(InfoOf(() => AsyncQueryableEx.Scan<TSource>(default(IAsyncQueryable<TSource>), default(Expression<Func<TSource, TSource, Task<TSource>>>))), source.Expression, accumulator));
#else
            return source.Provider.CreateQuery<TSource>(Expression.Call(((MethodInfo)MethodBase.GetCurrentMethod()).MakeGenericMethod(typeof(TSource)), source.Expression, accumulator));
#endif
        }

        public static IAsyncQueryable<TAccumulate> Scan<TSource, TAccumulate>(this IAsyncQueryable<TSource> source, TAccumulate seed, Expression<Func<TAccumulate, TSource, TAccumulate>> accumulator)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));
            if (accumulator == null)
                throw new ArgumentNullException(nameof(accumulator));

#if CRIPPLED_REFLECTION
            return source.Provider.CreateQuery<TAccumulate>(Expression.Call(InfoOf(() => AsyncQueryableEx.Scan<TSource, TAccumulate>(default(IAsyncQueryable<TSource>), default(TAccumulate), default(Expression<Func<TAccumulate, TSource, TAccumulate>>))), source.Expression, Expression.Constant(seed, typeof(TAccumulate)), accumulator));
#else
            return source.Provider.CreateQuery<TAccumulate>(Expression.Call(((MethodInfo)MethodBase.GetCurrentMethod()).MakeGenericMethod(typeof(TSource), typeof(TAccumulate)), source.Expression, Expression.Constant(seed, typeof(TAccumulate)), accumulator));
#endif
        }

        public static IAsyncQueryable<TAccumulate> Scan<TSource, TAccumulate>(this IAsyncQueryable<TSource> source, TAccumulate seed, Expression<Func<TAccumulate, TSource, Task<TAccumulate>>> accumulator)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));
            if (accumulator == null)
                throw new ArgumentNullException(nameof(accumulator));

#if CRIPPLED_REFLECTION
            return source.Provider.CreateQuery<TAccumulate>(Expression.Call(InfoOf(() => AsyncQueryableEx.Scan<TSource, TAccumulate>(default(IAsyncQueryable<TSource>), default(TAccumulate), default(Expression<Func<TAccumulate, TSource, Task<TAccumulate>>>))), source.Expression, Expression.Constant(seed, typeof(TAccumulate)), accumulator));
#else
            return source.Provider.CreateQuery<TAccumulate>(Expression.Call(((MethodInfo)MethodBase.GetCurrentMethod()).MakeGenericMethod(typeof(TSource), typeof(TAccumulate)), source.Expression, Expression.Constant(seed, typeof(TAccumulate)), accumulator));
#endif
        }

        public static IAsyncQueryable<TOther> SelectMany<TSource, TOther>(this IAsyncQueryable<TSource> source, IAsyncEnumerable<TOther> other)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));
            if (other == null)
                throw new ArgumentNullException(nameof(other));

#if CRIPPLED_REFLECTION
            return source.Provider.CreateQuery<TOther>(Expression.Call(InfoOf(() => AsyncQueryableEx.SelectMany<TSource, TOther>(default(IAsyncQueryable<TSource>), default(IAsyncEnumerable<TOther>))), source.Expression, GetSourceExpression(other)));
#else
            return source.Provider.CreateQuery<TOther>(Expression.Call(((MethodInfo)MethodBase.GetCurrentMethod()).MakeGenericMethod(typeof(TSource), typeof(TOther)), source.Expression, GetSourceExpression(other)));
#endif
        }

        public static IAsyncQueryable<TSource> StartWith<TSource>(this IAsyncQueryable<TSource> source, params TSource[] values)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));
            if (values == null)
                throw new ArgumentNullException(nameof(values));

#if CRIPPLED_REFLECTION
            return source.Provider.CreateQuery<TSource>(Expression.Call(InfoOf(() => AsyncQueryableEx.StartWith<TSource>(default(IAsyncQueryable<TSource>), default(TSource[]))), source.Expression, Expression.Constant(values, typeof(TSource[]))));
#else
            return source.Provider.CreateQuery<TSource>(Expression.Call(((MethodInfo)MethodBase.GetCurrentMethod()).MakeGenericMethod(typeof(TSource)), source.Expression, Expression.Constant(values, typeof(TSource[]))));
#endif
        }

    }
}
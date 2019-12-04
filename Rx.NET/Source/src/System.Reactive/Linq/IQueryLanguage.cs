// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the Apache 2.0 License.
// See the LICENSE file in the project root for more information. 

using System.Collections.Generic;
using System.Linq;
using System.Reactive.Concurrency;
using System.Reactive.Joins;
using System.Reactive.Subjects;
using System.Threading;
using System.Threading.Tasks;


namespace System.Reactive.Linq
{
    /// <summary>
    /// Internal interface describing the LINQ to Events query language.
    /// </summary>
    internal interface IQueryLanguage
    {
        #region * Aggregates *

        IObservable<TAccumulate> Aggregate<TSource, TAccumulate>(IObservable<TSource> source, TAccumulate seed, Func<TAccumulate, TSource, TAccumulate> accumulator);
        IObservable<TResult> Aggregate<TSource, TAccumulate, TResult>(IObservable<TSource> source, TAccumulate seed, Func<TAccumulate, TSource, TAccumulate> accumulator, Func<TAccumulate, TResult> resultSelector);
        IObservable<TSource> Aggregate<TSource>(IObservable<TSource> source, Func<TSource, TSource, TSource> accumulator);
        IObservable<bool> All<TSource>(IObservable<TSource> source, Func<TSource, bool> predicate);
        IObservable<bool> Any<TSource>(IObservable<TSource> source);
        IObservable<bool> Any<TSource>(IObservable<TSource> source, Func<TSource, bool> predicate);
        IObservable<double> Average(IObservable<double> source);
        IObservable<float> Average(IObservable<float> source);
        IObservable<decimal> Average(IObservable<decimal> source);
        IObservable<double> Average(IObservable<int> source);
        IObservable<double> Average(IObservable<long> source);
        IObservable<double?> Average(IObservable<double?> source);
        IObservable<float?> Average(IObservable<float?> source);
        IObservable<decimal?> Average(IObservable<decimal?> source);
        IObservable<double?> Average(IObservable<int?> source);
        IObservable<double?> Average(IObservable<long?> source);
        IObservable<double> Average<TSource>(IObservable<TSource> source, Func<TSource, double> selector);
        IObservable<float> Average<TSource>(IObservable<TSource> source, Func<TSource, float> selector);
        IObservable<decimal> Average<TSource>(IObservable<TSource> source, Func<TSource, decimal> selector);
        IObservable<double> Average<TSource>(IObservable<TSource> source, Func<TSource, int> selector);
        IObservable<double> Average<TSource>(IObservable<TSource> source, Func<TSource, long> selector);
        IObservable<double?> Average<TSource>(IObservable<TSource> source, Func<TSource, double?> selector);
        IObservable<float?> Average<TSource>(IObservable<TSource> source, Func<TSource, float?> selector);
        IObservable<decimal?> Average<TSource>(IObservable<TSource> source, Func<TSource, decimal?> selector);
        IObservable<double?> Average<TSource>(IObservable<TSource> source, Func<TSource, int?> selector);
        IObservable<double?> Average<TSource>(IObservable<TSource> source, Func<TSource, long?> selector);
        IObservable<bool> Contains<TSource>(IObservable<TSource> source, TSource value);
        IObservable<bool> Contains<TSource>(IObservable<TSource> source, TSource value, IEqualityComparer<TSource> comparer);
        IObservable<int> Count<TSource>(IObservable<TSource> source);
        IObservable<int> Count<TSource>(IObservable<TSource> source, Func<TSource, bool> predicate);
        IObservable<TSource> ElementAt<TSource>(IObservable<TSource> source, int index);
        IObservable<TSource> ElementAtOrDefault<TSource>(IObservable<TSource> source, int index);
        IObservable<TSource> FirstAsync<TSource>(IObservable<TSource> source);
        IObservable<TSource> FirstAsync<TSource>(IObservable<TSource> source, Func<TSource, bool> predicate);
        IObservable<TSource> FirstOrDefaultAsync<TSource>(IObservable<TSource> source);
        IObservable<TSource> FirstOrDefaultAsync<TSource>(IObservable<TSource> source, Func<TSource, bool> predicate);
        IObservable<bool> IsEmpty<TSource>(IObservable<TSource> source);
        IObservable<TSource> LastAsync<TSource>(IObservable<TSource> source);
        IObservable<TSource> LastAsync<TSource>(IObservable<TSource> source, Func<TSource, bool> predicate);
        IObservable<TSource> LastOrDefaultAsync<TSource>(IObservable<TSource> source);
        IObservable<TSource> LastOrDefaultAsync<TSource>(IObservable<TSource> source, Func<TSource, bool> predicate);
        IObservable<long> LongCount<TSource>(IObservable<TSource> source);
        IObservable<long> LongCount<TSource>(IObservable<TSource> source, Func<TSource, bool> predicate);
        IObservable<TSource> Max<TSource>(IObservable<TSource> source);
        IObservable<TSource> Max<TSource>(IObservable<TSource> source, IComparer<TSource> comparer);
        IObservable<double> Max(IObservable<double> source);
        IObservable<float> Max(IObservable<float> source);
        IObservable<decimal> Max(IObservable<decimal> source);
        IObservable<int> Max(IObservable<int> source);
        IObservable<long> Max(IObservable<long> source);
        IObservable<double?> Max(IObservable<double?> source);
        IObservable<float?> Max(IObservable<float?> source);
        IObservable<decimal?> Max(IObservable<decimal?> source);
        IObservable<int?> Max(IObservable<int?> source);
        IObservable<long?> Max(IObservable<long?> source);
        IObservable<TResult> Max<TSource, TResult>(IObservable<TSource> source, Func<TSource, TResult> selector);
        IObservable<TResult> Max<TSource, TResult>(IObservable<TSource> source, Func<TSource, TResult> selector, IComparer<TResult> comparer);
        IObservable<double> Max<TSource>(IObservable<TSource> source, Func<TSource, double> selector);
        IObservable<float> Max<TSource>(IObservable<TSource> source, Func<TSource, float> selector);
        IObservable<decimal> Max<TSource>(IObservable<TSource> source, Func<TSource, decimal> selector);
        IObservable<int> Max<TSource>(IObservable<TSource> source, Func<TSource, int> selector);
        IObservable<long> Max<TSource>(IObservable<TSource> source, Func<TSource, long> selector);
        IObservable<double?> Max<TSource>(IObservable<TSource> source, Func<TSource, double?> selector);
        IObservable<float?> Max<TSource>(IObservable<TSource> source, Func<TSource, float?> selector);
        IObservable<decimal?> Max<TSource>(IObservable<TSource> source, Func<TSource, decimal?> selector);
        IObservable<int?> Max<TSource>(IObservable<TSource> source, Func<TSource, int?> selector);
        IObservable<long?> Max<TSource>(IObservable<TSource> source, Func<TSource, long?> selector);
        IObservable<IList<TSource>> MaxBy<TSource, TKey>(IObservable<TSource> source, Func<TSource, TKey> keySelector);
        IObservable<IList<TSource>> MaxBy<TSource, TKey>(IObservable<TSource> source, Func<TSource, TKey> keySelector, IComparer<TKey> comparer);
        IObservable<TSource> Min<TSource>(IObservable<TSource> source);
        IObservable<TSource> Min<TSource>(IObservable<TSource> source, IComparer<TSource> comparer);
        IObservable<double> Min(IObservable<double> source);
        IObservable<float> Min(IObservable<float> source);
        IObservable<decimal> Min(IObservable<decimal> source);
        IObservable<int> Min(IObservable<int> source);
        IObservable<long> Min(IObservable<long> source);
        IObservable<double?> Min(IObservable<double?> source);
        IObservable<float?> Min(IObservable<float?> source);
        IObservable<decimal?> Min(IObservable<decimal?> source);
        IObservable<int?> Min(IObservable<int?> source);
        IObservable<long?> Min(IObservable<long?> source);
        IObservable<TResult> Min<TSource, TResult>(IObservable<TSource> source, Func<TSource, TResult> selector);
        IObservable<TResult> Min<TSource, TResult>(IObservable<TSource> source, Func<TSource, TResult> selector, IComparer<TResult> comparer);
        IObservable<double> Min<TSource>(IObservable<TSource> source, Func<TSource, double> selector);
        IObservable<float> Min<TSource>(IObservable<TSource> source, Func<TSource, float> selector);
        IObservable<decimal> Min<TSource>(IObservable<TSource> source, Func<TSource, decimal> selector);
        IObservable<int> Min<TSource>(IObservable<TSource> source, Func<TSource, int> selector);
        IObservable<long> Min<TSource>(IObservable<TSource> source, Func<TSource, long> selector);
        IObservable<double?> Min<TSource>(IObservable<TSource> source, Func<TSource, double?> selector);
        IObservable<float?> Min<TSource>(IObservable<TSource> source, Func<TSource, float?> selector);
        IObservable<decimal?> Min<TSource>(IObservable<TSource> source, Func<TSource, decimal?> selector);
        IObservable<int?> Min<TSource>(IObservable<TSource> source, Func<TSource, int?> selector);
        IObservable<long?> Min<TSource>(IObservable<TSource> source, Func<TSource, long?> selector);
        IObservable<IList<TSource>> MinBy<TSource, TKey>(IObservable<TSource> source, Func<TSource, TKey> keySelector);
        IObservable<IList<TSource>> MinBy<TSource, TKey>(IObservable<TSource> source, Func<TSource, TKey> keySelector, IComparer<TKey> comparer);
        IObservable<bool> SequenceEqual<TSource>(IObservable<TSource> first, IObservable<TSource> second);
        IObservable<bool> SequenceEqual<TSource>(IObservable<TSource> first, IObservable<TSource> second, IEqualityComparer<TSource> comparer);
        IObservable<bool> SequenceEqual<TSource>(IObservable<TSource> first, IEnumerable<TSource> second);
        IObservable<bool> SequenceEqual<TSource>(IObservable<TSource> first, IEnumerable<TSource> second, IEqualityComparer<TSource> comparer);
        IObservable<TSource> SingleAsync<TSource>(IObservable<TSource> source);
        IObservable<TSource> SingleAsync<TSource>(IObservable<TSource> source, Func<TSource, bool> predicate);
        IObservable<TSource> SingleOrDefaultAsync<TSource>(IObservable<TSource> source);
        IObservable<TSource> SingleOrDefaultAsync<TSource>(IObservable<TSource> source, Func<TSource, bool> predicate);
        IObservable<double> Sum(IObservable<double> source);
        IObservable<float> Sum(IObservable<float> source);
        IObservable<decimal> Sum(IObservable<decimal> source);
        IObservable<int> Sum(IObservable<int> source);
        IObservable<long> Sum(IObservable<long> source);
        IObservable<double?> Sum(IObservable<double?> source);
        IObservable<float?> Sum(IObservable<float?> source);
        IObservable<decimal?> Sum(IObservable<decimal?> source);
        IObservable<int?> Sum(IObservable<int?> source);
        IObservable<long?> Sum(IObservable<long?> source);
        IObservable<double> Sum<TSource>(IObservable<TSource> source, Func<TSource, double> selector);
        IObservable<float> Sum<TSource>(IObservable<TSource> source, Func<TSource, float> selector);
        IObservable<decimal> Sum<TSource>(IObservable<TSource> source, Func<TSource, decimal> selector);
        IObservable<int> Sum<TSource>(IObservable<TSource> source, Func<TSource, int> selector);
        IObservable<long> Sum<TSource>(IObservable<TSource> source, Func<TSource, long> selector);
        IObservable<double?> Sum<TSource>(IObservable<TSource> source, Func<TSource, double?> selector);
        IObservable<float?> Sum<TSource>(IObservable<TSource> source, Func<TSource, float?> selector);
        IObservable<decimal?> Sum<TSource>(IObservable<TSource> source, Func<TSource, decimal?> selector);
        IObservable<int?> Sum<TSource>(IObservable<TSource> source, Func<TSource, int?> selector);
        IObservable<long?> Sum<TSource>(IObservable<TSource> source, Func<TSource, long?> selector);
        IObservable<TSource[]> ToArray<TSource>(IObservable<TSource> source);
        IObservable<IDictionary<TKey, TElement>> ToDictionary<TSource, TKey, TElement>(IObservable<TSource> source, Func<TSource, TKey> keySelector, Func<TSource, TElement> elementSelector, IEqualityComparer<TKey> comparer);
        IObservable<IDictionary<TKey, TElement>> ToDictionary<TSource, TKey, TElement>(IObservable<TSource> source, Func<TSource, TKey> keySelector, Func<TSource, TElement> elementSelector);
        IObservable<IDictionary<TKey, TSource>> ToDictionary<TSource, TKey>(IObservable<TSource> source, Func<TSource, TKey> keySelector, IEqualityComparer<TKey> comparer);
        IObservable<IDictionary<TKey, TSource>> ToDictionary<TSource, TKey>(IObservable<TSource> source, Func<TSource, TKey> keySelector);
        IObservable<IList<TSource>> ToList<TSource>(IObservable<TSource> source);
        IObservable<ILookup<TKey, TElement>> ToLookup<TSource, TKey, TElement>(IObservable<TSource> source, Func<TSource, TKey> keySelector, Func<TSource, TElement> elementSelector, IEqualityComparer<TKey> comparer);
        IObservable<ILookup<TKey, TSource>> ToLookup<TSource, TKey>(IObservable<TSource> source, Func<TSource, TKey> keySelector, IEqualityComparer<TKey> comparer);
        IObservable<ILookup<TKey, TElement>> ToLookup<TSource, TKey, TElement>(IObservable<TSource> source, Func<TSource, TKey> keySelector, Func<TSource, TElement> elementSelector);
        IObservable<ILookup<TKey, TSource>> ToLookup<TSource, TKey>(IObservable<TSource> source, Func<TSource, TKey> keySelector);

        #endregion

        #region * Async *

        Func<IObservable<TResult>> FromAsyncPattern<TResult>(Func<AsyncCallback, object, IAsyncResult> begin, Func<IAsyncResult, TResult> end);
        Func<T1, IObservable<TResult>> FromAsyncPattern<T1, TResult>(Func<T1, AsyncCallback, object, IAsyncResult> begin, Func<IAsyncResult, TResult> end);
        Func<T1, T2, IObservable<TResult>> FromAsyncPattern<T1, T2, TResult>(Func<T1, T2, AsyncCallback, object, IAsyncResult> begin, Func<IAsyncResult, TResult> end);
        Func<T1, T2, T3, IObservable<TResult>> FromAsyncPattern<T1, T2, T3, TResult>(Func<T1, T2, T3, AsyncCallback, object, IAsyncResult> begin, Func<IAsyncResult, TResult> end);
        Func<T1, T2, T3, T4, IObservable<TResult>> FromAsyncPattern<T1, T2, T3, T4, TResult>(Func<T1, T2, T3, T4, AsyncCallback, object, IAsyncResult> begin, Func<IAsyncResult, TResult> end);
        Func<T1, T2, T3, T4, T5, IObservable<TResult>> FromAsyncPattern<T1, T2, T3, T4, T5, TResult>(Func<T1, T2, T3, T4, T5, AsyncCallback, object, IAsyncResult> begin, Func<IAsyncResult, TResult> end);
        Func<T1, T2, T3, T4, T5, T6, IObservable<TResult>> FromAsyncPattern<T1, T2, T3, T4, T5, T6, TResult>(Func<T1, T2, T3, T4, T5, T6, AsyncCallback, object, IAsyncResult> begin, Func<IAsyncResult, TResult> end);
        Func<T1, T2, T3, T4, T5, T6, T7, IObservable<TResult>> FromAsyncPattern<T1, T2, T3, T4, T5, T6, T7, TResult>(Func<T1, T2, T3, T4, T5, T6, T7, AsyncCallback, object, IAsyncResult> begin, Func<IAsyncResult, TResult> end);
        Func<T1, T2, T3, T4, T5, T6, T7, T8, IObservable<TResult>> FromAsyncPattern<T1, T2, T3, T4, T5, T6, T7, T8, TResult>(Func<T1, T2, T3, T4, T5, T6, T7, T8, AsyncCallback, object, IAsyncResult> begin, Func<IAsyncResult, TResult> end);
        Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, IObservable<TResult>> FromAsyncPattern<T1, T2, T3, T4, T5, T6, T7, T8, T9, TResult>(Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, AsyncCallback, object, IAsyncResult> begin, Func<IAsyncResult, TResult> end);
        Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, IObservable<TResult>> FromAsyncPattern<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, TResult>(Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, AsyncCallback, object, IAsyncResult> begin, Func<IAsyncResult, TResult> end);
        Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, IObservable<TResult>> FromAsyncPattern<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, TResult>(Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, AsyncCallback, object, IAsyncResult> begin, Func<IAsyncResult, TResult> end);
        Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, IObservable<TResult>> FromAsyncPattern<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, TResult>(Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, AsyncCallback, object, IAsyncResult> begin, Func<IAsyncResult, TResult> end);
        Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, IObservable<TResult>> FromAsyncPattern<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, TResult>(Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, AsyncCallback, object, IAsyncResult> begin, Func<IAsyncResult, TResult> end);
        Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, IObservable<TResult>> FromAsyncPattern<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, TResult>(Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, AsyncCallback, object, IAsyncResult> begin, Func<IAsyncResult, TResult> end);

        Func<IObservable<Unit>> FromAsyncPattern(Func<AsyncCallback, object, IAsyncResult> begin, Action<IAsyncResult> end);
        Func<T1, IObservable<Unit>> FromAsyncPattern<T1>(Func<T1, AsyncCallback, object, IAsyncResult> begin, Action<IAsyncResult> end);
        Func<T1, T2, IObservable<Unit>> FromAsyncPattern<T1, T2>(Func<T1, T2, AsyncCallback, object, IAsyncResult> begin, Action<IAsyncResult> end);
        Func<T1, T2, T3, IObservable<Unit>> FromAsyncPattern<T1, T2, T3>(Func<T1, T2, T3, AsyncCallback, object, IAsyncResult> begin, Action<IAsyncResult> end);
        Func<T1, T2, T3, T4, IObservable<Unit>> FromAsyncPattern<T1, T2, T3, T4>(Func<T1, T2, T3, T4, AsyncCallback, object, IAsyncResult> begin, Action<IAsyncResult> end);
        Func<T1, T2, T3, T4, T5, IObservable<Unit>> FromAsyncPattern<T1, T2, T3, T4, T5>(Func<T1, T2, T3, T4, T5, AsyncCallback, object, IAsyncResult> begin, Action<IAsyncResult> end);
        Func<T1, T2, T3, T4, T5, T6, IObservable<Unit>> FromAsyncPattern<T1, T2, T3, T4, T5, T6>(Func<T1, T2, T3, T4, T5, T6, AsyncCallback, object, IAsyncResult> begin, Action<IAsyncResult> end);
        Func<T1, T2, T3, T4, T5, T6, T7, IObservable<Unit>> FromAsyncPattern<T1, T2, T3, T4, T5, T6, T7>(Func<T1, T2, T3, T4, T5, T6, T7, AsyncCallback, object, IAsyncResult> begin, Action<IAsyncResult> end);
        Func<T1, T2, T3, T4, T5, T6, T7, T8, IObservable<Unit>> FromAsyncPattern<T1, T2, T3, T4, T5, T6, T7, T8>(Func<T1, T2, T3, T4, T5, T6, T7, T8, AsyncCallback, object, IAsyncResult> begin, Action<IAsyncResult> end);
        Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, IObservable<Unit>> FromAsyncPattern<T1, T2, T3, T4, T5, T6, T7, T8, T9>(Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, AsyncCallback, object, IAsyncResult> begin, Action<IAsyncResult> end);
        Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, IObservable<Unit>> FromAsyncPattern<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10>(Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, AsyncCallback, object, IAsyncResult> begin, Action<IAsyncResult> end);
        Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, IObservable<Unit>> FromAsyncPattern<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11>(Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, AsyncCallback, object, IAsyncResult> begin, Action<IAsyncResult> end);
        Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, IObservable<Unit>> FromAsyncPattern<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12>(Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, AsyncCallback, object, IAsyncResult> begin, Action<IAsyncResult> end);
        Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, IObservable<Unit>> FromAsyncPattern<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13>(Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, AsyncCallback, object, IAsyncResult> begin, Action<IAsyncResult> end);
        Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, IObservable<Unit>> FromAsyncPattern<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14>(Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, AsyncCallback, object, IAsyncResult> begin, Action<IAsyncResult> end);

        IObservable<TSource> Start<TSource>(Func<TSource> function);
        IObservable<TSource> Start<TSource>(Func<TSource> function, IScheduler scheduler);

        IObservable<TSource> StartAsync<TSource>(Func<Task<TSource>> functionAsync);
        IObservable<TSource> StartAsync<TSource>(Func<CancellationToken, Task<TSource>> functionAsync);
        IObservable<TSource> StartAsync<TSource>(Func<Task<TSource>> functionAsync, IScheduler scheduler);
        IObservable<TSource> StartAsync<TSource>(Func<CancellationToken, Task<TSource>> functionAsync, IScheduler scheduler);

        IObservable<Unit> Start(Action action);
        IObservable<Unit> Start(Action action, IScheduler scheduler);

        IObservable<Unit> StartAsync(Func<Task> actionAsync);
        IObservable<Unit> StartAsync(Func<CancellationToken, Task> actionAsync);
        IObservable<Unit> StartAsync(Func<Task> actionAsync, IScheduler scheduler);
        IObservable<Unit> StartAsync(Func<CancellationToken, Task> actionAsync, IScheduler scheduler);

        IObservable<TResult> FromAsync<TResult>(Func<Task<TResult>> functionAsync);
        IObservable<TResult> FromAsync<TResult>(Func<CancellationToken, Task<TResult>> functionAsync);
        IObservable<Unit> FromAsync(Func<Task> actionAsync);
        IObservable<Unit> FromAsync(Func<CancellationToken, Task> actionAsync);
        IObservable<TResult> FromAsync<TResult>(Func<Task<TResult>> functionAsync, IScheduler scheduler);
        IObservable<TResult> FromAsync<TResult>(Func<CancellationToken, Task<TResult>> functionAsync, IScheduler scheduler);
        IObservable<Unit> FromAsync(Func<Task> actionAsync, IScheduler scheduler);
        IObservable<Unit> FromAsync(Func<CancellationToken, Task> actionAsync, IScheduler scheduler);

        Func<IObservable<TResult>> ToAsync<TResult>(Func<TResult> function);
        Func<IObservable<TResult>> ToAsync<TResult>(Func<TResult> function, IScheduler scheduler);
        Func<T, IObservable<TResult>> ToAsync<T, TResult>(Func<T, TResult> function);
        Func<T, IObservable<TResult>> ToAsync<T, TResult>(Func<T, TResult> function, IScheduler scheduler);
        Func<T1, T2, IObservable<TResult>> ToAsync<T1, T2, TResult>(Func<T1, T2, TResult> function);
        Func<T1, T2, IObservable<TResult>> ToAsync<T1, T2, TResult>(Func<T1, T2, TResult> function, IScheduler scheduler);
        Func<T1, T2, T3, IObservable<TResult>> ToAsync<T1, T2, T3, TResult>(Func<T1, T2, T3, TResult> function);
        Func<T1, T2, T3, IObservable<TResult>> ToAsync<T1, T2, T3, TResult>(Func<T1, T2, T3, TResult> function, IScheduler scheduler);
        Func<T1, T2, T3, T4, IObservable<TResult>> ToAsync<T1, T2, T3, T4, TResult>(Func<T1, T2, T3, T4, TResult> function);
        Func<T1, T2, T3, T4, IObservable<TResult>> ToAsync<T1, T2, T3, T4, TResult>(Func<T1, T2, T3, T4, TResult> function, IScheduler scheduler);
        Func<T1, T2, T3, T4, T5, IObservable<TResult>> ToAsync<T1, T2, T3, T4, T5, TResult>(Func<T1, T2, T3, T4, T5, TResult> function);
        Func<T1, T2, T3, T4, T5, IObservable<TResult>> ToAsync<T1, T2, T3, T4, T5, TResult>(Func<T1, T2, T3, T4, T5, TResult> function, IScheduler scheduler);
        Func<T1, T2, T3, T4, T5, T6, IObservable<TResult>> ToAsync<T1, T2, T3, T4, T5, T6, TResult>(Func<T1, T2, T3, T4, T5, T6, TResult> function);
        Func<T1, T2, T3, T4, T5, T6, IObservable<TResult>> ToAsync<T1, T2, T3, T4, T5, T6, TResult>(Func<T1, T2, T3, T4, T5, T6, TResult> function, IScheduler scheduler);
        Func<T1, T2, T3, T4, T5, T6, T7, IObservable<TResult>> ToAsync<T1, T2, T3, T4, T5, T6, T7, TResult>(Func<T1, T2, T3, T4, T5, T6, T7, TResult> function);
        Func<T1, T2, T3, T4, T5, T6, T7, IObservable<TResult>> ToAsync<T1, T2, T3, T4, T5, T6, T7, TResult>(Func<T1, T2, T3, T4, T5, T6, T7, TResult> function, IScheduler scheduler);
        Func<T1, T2, T3, T4, T5, T6, T7, T8, IObservable<TResult>> ToAsync<T1, T2, T3, T4, T5, T6, T7, T8, TResult>(Func<T1, T2, T3, T4, T5, T6, T7, T8, TResult> function);
        Func<T1, T2, T3, T4, T5, T6, T7, T8, IObservable<TResult>> ToAsync<T1, T2, T3, T4, T5, T6, T7, T8, TResult>(Func<T1, T2, T3, T4, T5, T6, T7, T8, TResult> function, IScheduler scheduler);
        Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, IObservable<TResult>> ToAsync<T1, T2, T3, T4, T5, T6, T7, T8, T9, TResult>(Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, TResult> function);
        Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, IObservable<TResult>> ToAsync<T1, T2, T3, T4, T5, T6, T7, T8, T9, TResult>(Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, TResult> function, IScheduler scheduler);
        Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, IObservable<TResult>> ToAsync<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, TResult>(Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, TResult> function);
        Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, IObservable<TResult>> ToAsync<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, TResult>(Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, TResult> function, IScheduler scheduler);
        Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, IObservable<TResult>> ToAsync<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, TResult>(Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, TResult> function);
        Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, IObservable<TResult>> ToAsync<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, TResult>(Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, TResult> function, IScheduler scheduler);
        Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, IObservable<TResult>> ToAsync<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, TResult>(Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, TResult> function);
        Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, IObservable<TResult>> ToAsync<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, TResult>(Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, TResult> function, IScheduler scheduler);
        Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, IObservable<TResult>> ToAsync<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, TResult>(Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, TResult> function);
        Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, IObservable<TResult>> ToAsync<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, TResult>(Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, TResult> function, IScheduler scheduler);
        Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, IObservable<TResult>> ToAsync<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, TResult>(Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, TResult> function);
        Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, IObservable<TResult>> ToAsync<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, TResult>(Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, TResult> function, IScheduler scheduler);
        Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, IObservable<TResult>> ToAsync<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, TResult>(Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, TResult> function);
        Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, IObservable<TResult>> ToAsync<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, TResult>(Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, TResult> function, IScheduler scheduler);
        Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16, IObservable<TResult>> ToAsync<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16, TResult>(Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16, TResult> function);
        Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16, IObservable<TResult>> ToAsync<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16, TResult>(Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16, TResult> function, IScheduler scheduler);

        Func<IObservable<Unit>> ToAsync(Action action);
        Func<IObservable<Unit>> ToAsync(Action action, IScheduler scheduler);
        Func<TSource, IObservable<Unit>> ToAsync<TSource>(Action<TSource> action);
        Func<TSource, IObservable<Unit>> ToAsync<TSource>(Action<TSource> action, IScheduler scheduler);
        Func<T1, T2, IObservable<Unit>> ToAsync<T1, T2>(Action<T1, T2> action);
        Func<T1, T2, IObservable<Unit>> ToAsync<T1, T2>(Action<T1, T2> action, IScheduler scheduler);
        Func<T1, T2, T3, IObservable<Unit>> ToAsync<T1, T2, T3>(Action<T1, T2, T3> action);
        Func<T1, T2, T3, IObservable<Unit>> ToAsync<T1, T2, T3>(Action<T1, T2, T3> action, IScheduler scheduler);
        Func<T1, T2, T3, T4, IObservable<Unit>> ToAsync<T1, T2, T3, T4>(Action<T1, T2, T3, T4> action);
        Func<T1, T2, T3, T4, IObservable<Unit>> ToAsync<T1, T2, T3, T4>(Action<T1, T2, T3, T4> action, IScheduler scheduler);
        Func<T1, T2, T3, T4, T5, IObservable<Unit>> ToAsync<T1, T2, T3, T4, T5>(Action<T1, T2, T3, T4, T5> action);
        Func<T1, T2, T3, T4, T5, IObservable<Unit>> ToAsync<T1, T2, T3, T4, T5>(Action<T1, T2, T3, T4, T5> action, IScheduler scheduler);
        Func<T1, T2, T3, T4, T5, T6, IObservable<Unit>> ToAsync<T1, T2, T3, T4, T5, T6>(Action<T1, T2, T3, T4, T5, T6> action);
        Func<T1, T2, T3, T4, T5, T6, IObservable<Unit>> ToAsync<T1, T2, T3, T4, T5, T6>(Action<T1, T2, T3, T4, T5, T6> action, IScheduler scheduler);
        Func<T1, T2, T3, T4, T5, T6, T7, IObservable<Unit>> ToAsync<T1, T2, T3, T4, T5, T6, T7>(Action<T1, T2, T3, T4, T5, T6, T7> action);
        Func<T1, T2, T3, T4, T5, T6, T7, IObservable<Unit>> ToAsync<T1, T2, T3, T4, T5, T6, T7>(Action<T1, T2, T3, T4, T5, T6, T7> action, IScheduler scheduler);
        Func<T1, T2, T3, T4, T5, T6, T7, T8, IObservable<Unit>> ToAsync<T1, T2, T3, T4, T5, T6, T7, T8>(Action<T1, T2, T3, T4, T5, T6, T7, T8> action);
        Func<T1, T2, T3, T4, T5, T6, T7, T8, IObservable<Unit>> ToAsync<T1, T2, T3, T4, T5, T6, T7, T8>(Action<T1, T2, T3, T4, T5, T6, T7, T8> action, IScheduler scheduler);
        Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, IObservable<Unit>> ToAsync<T1, T2, T3, T4, T5, T6, T7, T8, T9>(Action<T1, T2, T3, T4, T5, T6, T7, T8, T9> action);
        Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, IObservable<Unit>> ToAsync<T1, T2, T3, T4, T5, T6, T7, T8, T9>(Action<T1, T2, T3, T4, T5, T6, T7, T8, T9> action, IScheduler scheduler);
        Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, IObservable<Unit>> ToAsync<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10>(Action<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10> action);
        Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, IObservable<Unit>> ToAsync<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10>(Action<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10> action, IScheduler scheduler);
        Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, IObservable<Unit>> ToAsync<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11>(Action<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11> action);
        Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, IObservable<Unit>> ToAsync<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11>(Action<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11> action, IScheduler scheduler);
        Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, IObservable<Unit>> ToAsync<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12>(Action<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12> action);
        Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, IObservable<Unit>> ToAsync<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12>(Action<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12> action, IScheduler scheduler);
        Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, IObservable<Unit>> ToAsync<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13>(Action<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13> action);
        Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, IObservable<Unit>> ToAsync<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13>(Action<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13> action, IScheduler scheduler);
        Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, IObservable<Unit>> ToAsync<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14>(Action<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14> action);
        Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, IObservable<Unit>> ToAsync<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14>(Action<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14> action, IScheduler scheduler);
        Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, IObservable<Unit>> ToAsync<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15>(Action<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15> action);
        Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, IObservable<Unit>> ToAsync<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15>(Action<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15> action, IScheduler scheduler);
        Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16, IObservable<Unit>> ToAsync<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16>(Action<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16> action);
        Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16, IObservable<Unit>> ToAsync<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16>(Action<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16> action, IScheduler scheduler);

        #endregion

        #region * Awaiter *

        AsyncSubject<TSource> GetAwaiter<TSource>(IObservable<TSource> source);
        AsyncSubject<TSource> GetAwaiter<TSource>(IConnectableObservable<TSource> source);
        AsyncSubject<TSource> RunAsync<TSource>(IObservable<TSource> source, CancellationToken cancellationToken);
        AsyncSubject<TSource> RunAsync<TSource>(IConnectableObservable<TSource> source, CancellationToken cancellationToken);

        #endregion

        #region * Binding *

        IConnectableObservable<TResult> Multicast<TSource, TResult>(IObservable<TSource> source, ISubject<TSource, TResult> subject);
        IObservable<TResult> Multicast<TSource, TIntermediate, TResult>(IObservable<TSource> source, Func<ISubject<TSource, TIntermediate>> subjectSelector, Func<IObservable<TIntermediate>, IObservable<TResult>> selector);
        IConnectableObservable<TSource> Publish<TSource>(IObservable<TSource> source);
        IObservable<TResult> Publish<TSource, TResult>(IObservable<TSource> source, Func<IObservable<TSource>, IObservable<TResult>> selector);
        IConnectableObservable<TSource> Publish<TSource>(IObservable<TSource> source, TSource initialValue);
        IObservable<TResult> Publish<TSource, TResult>(IObservable<TSource> source, Func<IObservable<TSource>, IObservable<TResult>> selector, TSource initialValue);
        IConnectableObservable<TSource> PublishLast<TSource>(IObservable<TSource> source);
        IObservable<TResult> PublishLast<TSource, TResult>(IObservable<TSource> source, Func<IObservable<TSource>, IObservable<TResult>> selector);
        IObservable<TSource> RefCount<TSource>(IConnectableObservable<TSource> source);
        IObservable<TSource> RefCount<TSource>(IConnectableObservable<TSource> source, TimeSpan disconnectDelay);
        IObservable<TSource> RefCount<TSource>(IConnectableObservable<TSource> source, TimeSpan disconnectDelay, IScheduler schedulder);
        IObservable<TSource> RefCount<TSource>(IConnectableObservable<TSource> source, int minObservers);
        IObservable<TSource> RefCount<TSource>(IConnectableObservable<TSource> source, int minObservers, TimeSpan disconnectDelay);
        IObservable<TSource> RefCount<TSource>(IConnectableObservable<TSource> source, int minObservers, TimeSpan disconnectDelay, IScheduler schedulder);
        IConnectableObservable<TSource> Replay<TSource>(IObservable<TSource> source);
        IConnectableObservable<TSource> Replay<TSource>(IObservable<TSource> source, IScheduler scheduler);
        IObservable<TResult> Replay<TSource, TResult>(IObservable<TSource> source, Func<IObservable<TSource>, IObservable<TResult>> selector);
        IObservable<TResult> Replay<TSource, TResult>(IObservable<TSource> source, Func<IObservable<TSource>, IObservable<TResult>> selector, IScheduler scheduler);
        IConnectableObservable<TSource> Replay<TSource>(IObservable<TSource> source, TimeSpan window);
        IObservable<TResult> Replay<TSource, TResult>(IObservable<TSource> source, Func<IObservable<TSource>, IObservable<TResult>> selector, TimeSpan window);
        IConnectableObservable<TSource> Replay<TSource>(IObservable<TSource> source, TimeSpan window, IScheduler scheduler);
        IObservable<TResult> Replay<TSource, TResult>(IObservable<TSource> source, Func<IObservable<TSource>, IObservable<TResult>> selector, TimeSpan window, IScheduler scheduler);
        IConnectableObservable<TSource> Replay<TSource>(IObservable<TSource> source, int bufferSize, IScheduler scheduler);
        IObservable<TResult> Replay<TSource, TResult>(IObservable<TSource> source, Func<IObservable<TSource>, IObservable<TResult>> selector, int bufferSize, IScheduler scheduler);
        IConnectableObservable<TSource> Replay<TSource>(IObservable<TSource> source, int bufferSize);
        IObservable<TResult> Replay<TSource, TResult>(IObservable<TSource> source, Func<IObservable<TSource>, IObservable<TResult>> selector, int bufferSize);
        IConnectableObservable<TSource> Replay<TSource>(IObservable<TSource> source, int bufferSize, TimeSpan window);
        IObservable<TResult> Replay<TSource, TResult>(IObservable<TSource> source, Func<IObservable<TSource>, IObservable<TResult>> selector, int bufferSize, TimeSpan window);
        IConnectableObservable<TSource> Replay<TSource>(IObservable<TSource> source, int bufferSize, TimeSpan window, IScheduler scheduler);
        IObservable<TResult> Replay<TSource, TResult>(IObservable<TSource> source, Func<IObservable<TSource>, IObservable<TResult>> selector, int bufferSize, TimeSpan window, IScheduler scheduler);
        IObservable<TSource> AutoConnect<TSource>(IConnectableObservable<TSource> source, int minObservers, Action<IDisposable> onConnect);

        #endregion

        #region * Blocking *

        IEnumerable<IList<TSource>> Chunkify<TSource>(IObservable<TSource> source);
        IEnumerable<TResult> Collect<TSource, TResult>(IObservable<TSource> source, Func<TResult> newCollector, Func<TResult, TSource, TResult> merge);
        IEnumerable<TResult> Collect<TSource, TResult>(IObservable<TSource> source, Func<TResult> getInitialCollector, Func<TResult, TSource, TResult> merge, Func<TResult, TResult> getNewCollector);
        TSource First<TSource>(IObservable<TSource> source);
        TSource First<TSource>(IObservable<TSource> source, Func<TSource, bool> predicate);
        TSource FirstOrDefault<TSource>(IObservable<TSource> source);
        TSource FirstOrDefault<TSource>(IObservable<TSource> source, Func<TSource, bool> predicate);
        void ForEach<TSource>(IObservable<TSource> source, Action<TSource> onNext);
        void ForEach<TSource>(IObservable<TSource> source, Action<TSource, int> onNext);
        IEnumerator<TSource> GetEnumerator<TSource>(IObservable<TSource> source);
        TSource Last<TSource>(IObservable<TSource> source);
        TSource Last<TSource>(IObservable<TSource> source, Func<TSource, bool> predicate);
        TSource LastOrDefault<TSource>(IObservable<TSource> source);
        TSource LastOrDefault<TSource>(IObservable<TSource> source, Func<TSource, bool> predicate);
        IEnumerable<TSource> Latest<TSource>(IObservable<TSource> source);
        IEnumerable<TSource> MostRecent<TSource>(IObservable<TSource> source, TSource initialValue);
        IEnumerable<TSource> Next<TSource>(IObservable<TSource> source);
        TSource Single<TSource>(IObservable<TSource> source);
        TSource SingleOrDefault<TSource>(IObservable<TSource> source, Func<TSource, bool> predicate);
        TSource SingleOrDefault<TSource>(IObservable<TSource> source);
        TSource Single<TSource>(IObservable<TSource> source, Func<TSource, bool> predicate);
        TSource Wait<TSource>(IObservable<TSource> source);

        #endregion

        #region * Concurrency *

        IObservable<TSource> ObserveOn<TSource>(IObservable<TSource> source, IScheduler scheduler);
        IObservable<TSource> ObserveOn<TSource>(IObservable<TSource> source, SynchronizationContext context);

        IObservable<TSource> SubscribeOn<TSource>(IObservable<TSource> source, IScheduler scheduler);
        IObservable<TSource> SubscribeOn<TSource>(IObservable<TSource> source, SynchronizationContext context);

        IObservable<TSource> Synchronize<TSource>(IObservable<TSource> source);
        IObservable<TSource> Synchronize<TSource>(IObservable<TSource> source, object gate);

        #endregion

        #region * Conversions *

        IDisposable Subscribe<TSource>(IEnumerable<TSource> source, IObserver<TSource> observer);
        IDisposable Subscribe<TSource>(IEnumerable<TSource> source, IObserver<TSource> observer, IScheduler scheduler);
        IEnumerable<TSource> ToEnumerable<TSource>(IObservable<TSource> source);
        IEventSource<Unit> ToEvent(IObservable<Unit> source);
        IEventSource<TSource> ToEvent<TSource>(IObservable<TSource> source);
        IEventPatternSource<TEventArgs> ToEventPattern<TEventArgs>(IObservable<EventPattern<TEventArgs>> source);
        IObservable<TSource> ToObservable<TSource>(IEnumerable<TSource> source);
        IObservable<TSource> ToObservable<TSource>(IEnumerable<TSource> source, IScheduler scheduler);

        #endregion

        #region * Creation *

        IObservable<TSource> Create<TSource>(Func<IObserver<TSource>, IDisposable> subscribe);
        IObservable<TSource> Create<TSource>(Func<IObserver<TSource>, Action> subscribe);
        IObservable<TResult> Create<TResult>(Func<IObserver<TResult>, CancellationToken, Task> subscribeAsync);
        IObservable<TResult> Create<TResult>(Func<IObserver<TResult>, Task> subscribeAsync);
        IObservable<TResult> Create<TResult>(Func<IObserver<TResult>, CancellationToken, Task<IDisposable>> subscribeAsync);
        IObservable<TResult> Create<TResult>(Func<IObserver<TResult>, Task<IDisposable>> subscribeAsync);
        IObservable<TResult> Create<TResult>(Func<IObserver<TResult>, CancellationToken, Task<Action>> subscribeAsync);
        IObservable<TResult> Create<TResult>(Func<IObserver<TResult>, Task<Action>> subscribeAsync);

        IObservable<TValue> Defer<TValue>(Func<IObservable<TValue>> observableFactory);

        IObservable<TValue> Defer<TValue>(Func<Task<IObservable<TValue>>> observableFactoryAsync);
        IObservable<TValue> Defer<TValue>(Func<CancellationToken, Task<IObservable<TValue>>> observableFactoryAsync);

        IObservable<TResult> Empty<TResult>();
        IObservable<TResult> Empty<TResult>(IScheduler scheduler);
        IObservable<TResult> Generate<TState, TResult>(TState initialState, Func<TState, bool> condition, Func<TState, TState> iterate, Func<TState, TResult> resultSelector);
        IObservable<TResult> Generate<TState, TResult>(TState initialState, Func<TState, bool> condition, Func<TState, TState> iterate, Func<TState, TResult> resultSelector, IScheduler scheduler);
        IObservable<TResult> Never<TResult>();
        IObservable<int> Range(int start, int count);
        IObservable<int> Range(int start, int count, IScheduler scheduler);
        IObservable<TResult> Repeat<TResult>(TResult value);
        IObservable<TResult> Repeat<TResult>(TResult value, IScheduler scheduler);
        IObservable<TResult> Repeat<TResult>(TResult value, int repeatCount);
        IObservable<TResult> Repeat<TResult>(TResult value, int repeatCount, IScheduler scheduler);
        IObservable<TResult> Return<TResult>(TResult value);
        IObservable<TResult> Return<TResult>(TResult value, IScheduler scheduler);
        IObservable<TResult> Throw<TResult>(Exception exception);
        IObservable<TResult> Throw<TResult>(Exception exception, IScheduler scheduler);
        IObservable<TSource> Using<TSource, TResource>(Func<TResource> resourceFactory, Func<TResource, IObservable<TSource>> observableFactory) where TResource : IDisposable;
        IObservable<TSource> Using<TSource, TResource>(Func<CancellationToken, Task<TResource>> resourceFactoryAsync, Func<TResource, CancellationToken, Task<IObservable<TSource>>> observableFactoryAsync) where TResource : IDisposable;

        #endregion

        #region * Events *

        IObservable<EventPattern<object>> FromEventPattern(Action<EventHandler> addHandler, Action<EventHandler> removeHandler);
        IObservable<EventPattern<object>> FromEventPattern(Action<EventHandler> addHandler, Action<EventHandler> removeHandler, IScheduler scheduler);
        IObservable<EventPattern<TEventArgs>> FromEventPattern<TDelegate, TEventArgs>(Action<TDelegate> addHandler, Action<TDelegate> removeHandler);
        IObservable<EventPattern<TEventArgs>> FromEventPattern<TDelegate, TEventArgs>(Action<TDelegate> addHandler, Action<TDelegate> removeHandler, IScheduler scheduler);
        IObservable<EventPattern<TEventArgs>> FromEventPattern<TDelegate, TEventArgs>(Func<EventHandler<TEventArgs>, TDelegate> conversion, Action<TDelegate> addHandler, Action<TDelegate> removeHandler);
        IObservable<EventPattern<TEventArgs>> FromEventPattern<TDelegate, TEventArgs>(Func<EventHandler<TEventArgs>, TDelegate> conversion, Action<TDelegate> addHandler, Action<TDelegate> removeHandler, IScheduler scheduler);
        IObservable<EventPattern<TEventArgs>> FromEventPattern<TEventArgs>(Action<EventHandler<TEventArgs>> addHandler, Action<EventHandler<TEventArgs>> removeHandler);
        IObservable<EventPattern<TEventArgs>> FromEventPattern<TEventArgs>(Action<EventHandler<TEventArgs>> addHandler, Action<EventHandler<TEventArgs>> removeHandler, IScheduler scheduler);
        IObservable<EventPattern<TSender, TEventArgs>> FromEventPattern<TDelegate, TSender, TEventArgs>(Action<TDelegate> addHandler, Action<TDelegate> removeHandler);
        IObservable<EventPattern<TSender, TEventArgs>> FromEventPattern<TDelegate, TSender, TEventArgs>(Action<TDelegate> addHandler, Action<TDelegate> removeHandler, IScheduler scheduler);
        IObservable<EventPattern<object>> FromEventPattern(object target, string eventName);
        IObservable<EventPattern<object>> FromEventPattern(object target, string eventName, IScheduler scheduler);
        IObservable<EventPattern<TEventArgs>> FromEventPattern<TEventArgs>(object target, string eventName);
        IObservable<EventPattern<TEventArgs>> FromEventPattern<TEventArgs>(object target, string eventName, IScheduler scheduler);
        IObservable<EventPattern<TSender, TEventArgs>> FromEventPattern<TSender, TEventArgs>(object target, string eventName);
        IObservable<EventPattern<TSender, TEventArgs>> FromEventPattern<TSender, TEventArgs>(object target, string eventName, IScheduler scheduler);
        IObservable<EventPattern<object>> FromEventPattern(Type type, string eventName);
        IObservable<EventPattern<object>> FromEventPattern(Type type, string eventName, IScheduler scheduler);
        IObservable<EventPattern<TEventArgs>> FromEventPattern<TEventArgs>(Type type, string eventName);
        IObservable<EventPattern<TEventArgs>> FromEventPattern<TEventArgs>(Type type, string eventName, IScheduler scheduler);
        IObservable<EventPattern<TSender, TEventArgs>> FromEventPattern<TSender, TEventArgs>(Type type, string eventName);
        IObservable<EventPattern<TSender, TEventArgs>> FromEventPattern<TSender, TEventArgs>(Type type, string eventName, IScheduler scheduler);

        IObservable<TEventArgs> FromEvent<TDelegate, TEventArgs>(Func<Action<TEventArgs>, TDelegate> conversion, Action<TDelegate> addHandler, Action<TDelegate> removeHandler);
        IObservable<TEventArgs> FromEvent<TDelegate, TEventArgs>(Func<Action<TEventArgs>, TDelegate> conversion, Action<TDelegate> addHandler, Action<TDelegate> removeHandler, IScheduler scheduler);
        IObservable<TEventArgs> FromEvent<TDelegate, TEventArgs>(Action<TDelegate> addHandler, Action<TDelegate> removeHandler);
        IObservable<TEventArgs> FromEvent<TDelegate, TEventArgs>(Action<TDelegate> addHandler, Action<TDelegate> removeHandler, IScheduler scheduler);
        IObservable<TEventArgs> FromEvent<TEventArgs>(Action<Action<TEventArgs>> addHandler, Action<Action<TEventArgs>> removeHandler);
        IObservable<TEventArgs> FromEvent<TEventArgs>(Action<Action<TEventArgs>> addHandler, Action<Action<TEventArgs>> removeHandler, IScheduler scheduler);
        IObservable<Unit> FromEvent(Action<Action> addHandler, Action<Action> removeHandler);
        IObservable<Unit> FromEvent(Action<Action> addHandler, Action<Action> removeHandler, IScheduler scheduler);

        #endregion

        #region * Imperative *

        Task ForEachAsync<TSource>(IObservable<TSource> source, Action<TSource> onNext);
        Task ForEachAsync<TSource>(IObservable<TSource> source, Action<TSource> onNext, CancellationToken cancellationToken);
        Task ForEachAsync<TSource>(IObservable<TSource> source, Action<TSource, int> onNext);
        Task ForEachAsync<TSource>(IObservable<TSource> source, Action<TSource, int> onNext, CancellationToken cancellationToken);

        IObservable<TResult> Case<TValue, TResult>(Func<TValue> selector, IDictionary<TValue, IObservable<TResult>> sources, IObservable<TResult> defaultSource);
        IObservable<TResult> Case<TValue, TResult>(Func<TValue> selector, IDictionary<TValue, IObservable<TResult>> sources, IScheduler scheduler);
        IObservable<TResult> Case<TValue, TResult>(Func<TValue> selector, IDictionary<TValue, IObservable<TResult>> sources);
        IObservable<TSource> DoWhile<TSource>(IObservable<TSource> source, Func<bool> condition);
        IObservable<TResult> For<TSource, TResult>(IEnumerable<TSource> source, Func<TSource, IObservable<TResult>> resultSelector);
        IObservable<TResult> If<TResult>(Func<bool> condition, IObservable<TResult> thenSource, IObservable<TResult> elseSource);
        IObservable<TResult> If<TResult>(Func<bool> condition, IObservable<TResult> thenSource);
        IObservable<TResult> If<TResult>(Func<bool> condition, IObservable<TResult> thenSource, IScheduler scheduler);
        IObservable<TSource> While<TSource>(Func<bool> condition, IObservable<TSource> source);

        #endregion

        #region * Joins *

        Pattern<TLeft, TRight> And<TLeft, TRight>(IObservable<TLeft> left, IObservable<TRight> right);
        Plan<TResult> Then<TSource, TResult>(IObservable<TSource> source, Func<TSource, TResult> selector);
        IObservable<TResult> When<TResult>(params Plan<TResult>[] plans);
        IObservable<TResult> When<TResult>(IEnumerable<Plan<TResult>> plans);

        #endregion

        #region * Multiple *

        IObservable<TSource> Amb<TSource>(IObservable<TSource> first, IObservable<TSource> second);
        IObservable<TSource> Amb<TSource>(params IObservable<TSource>[] sources);
        IObservable<TSource> Amb<TSource>(IEnumerable<IObservable<TSource>> sources);
        IObservable<IList<TSource>> Buffer<TSource, TBufferClosing>(IObservable<TSource> source, Func<IObservable<TBufferClosing>> bufferClosingSelector);
        IObservable<IList<TSource>> Buffer<TSource, TBufferOpening, TBufferClosing>(IObservable<TSource> source, IObservable<TBufferOpening> bufferOpenings, Func<TBufferOpening, IObservable<TBufferClosing>> bufferClosingSelector);
        IObservable<IList<TSource>> Buffer<TSource, TBufferBoundary>(IObservable<TSource> source, IObservable<TBufferBoundary> bufferBoundaries);
        IObservable<TSource> Catch<TSource, TException>(IObservable<TSource> source, Func<TException, IObservable<TSource>> handler) where TException : Exception;
        IObservable<TSource> Catch<TSource>(IObservable<TSource> first, IObservable<TSource> second);
        IObservable<TSource> Catch<TSource>(params IObservable<TSource>[] sources);
        IObservable<TSource> Catch<TSource>(IEnumerable<IObservable<TSource>> sources);
        IObservable<TResult> CombineLatest<TFirst, TSecond, TResult>(IObservable<TFirst> first, IObservable<TSecond> second, Func<TFirst, TSecond, TResult> resultSelector);
        IObservable<TResult> CombineLatest<T1, T2, T3, TResult>(IObservable<T1> source1, IObservable<T2> source2, IObservable<T3> source3, Func<T1, T2, T3, TResult> resultSelector);
        IObservable<TResult> CombineLatest<T1, T2, T3, T4, TResult>(IObservable<T1> source1, IObservable<T2> source2, IObservable<T3> source3, IObservable<T4> source4, Func<T1, T2, T3, T4, TResult> resultSelector);
        IObservable<TResult> CombineLatest<T1, T2, T3, T4, T5, TResult>(IObservable<T1> source1, IObservable<T2> source2, IObservable<T3> source3, IObservable<T4> source4, IObservable<T5> source5, Func<T1, T2, T3, T4, T5, TResult> resultSelector);
        IObservable<TResult> CombineLatest<T1, T2, T3, T4, T5, T6, TResult>(IObservable<T1> source1, IObservable<T2> source2, IObservable<T3> source3, IObservable<T4> source4, IObservable<T5> source5, IObservable<T6> source6, Func<T1, T2, T3, T4, T5, T6, TResult> resultSelector);
        IObservable<TResult> CombineLatest<T1, T2, T3, T4, T5, T6, T7, TResult>(IObservable<T1> source1, IObservable<T2> source2, IObservable<T3> source3, IObservable<T4> source4, IObservable<T5> source5, IObservable<T6> source6, IObservable<T7> source7, Func<T1, T2, T3, T4, T5, T6, T7, TResult> resultSelector);
        IObservable<TResult> CombineLatest<T1, T2, T3, T4, T5, T6, T7, T8, TResult>(IObservable<T1> source1, IObservable<T2> source2, IObservable<T3> source3, IObservable<T4> source4, IObservable<T5> source5, IObservable<T6> source6, IObservable<T7> source7, IObservable<T8> source8, Func<T1, T2, T3, T4, T5, T6, T7, T8, TResult> resultSelector);
        IObservable<TResult> CombineLatest<T1, T2, T3, T4, T5, T6, T7, T8, T9, TResult>(IObservable<T1> source1, IObservable<T2> source2, IObservable<T3> source3, IObservable<T4> source4, IObservable<T5> source5, IObservable<T6> source6, IObservable<T7> source7, IObservable<T8> source8, IObservable<T9> source9, Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, TResult> resultSelector);
        IObservable<TResult> CombineLatest<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, TResult>(IObservable<T1> source1, IObservable<T2> source2, IObservable<T3> source3, IObservable<T4> source4, IObservable<T5> source5, IObservable<T6> source6, IObservable<T7> source7, IObservable<T8> source8, IObservable<T9> source9, IObservable<T10> source10, Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, TResult> resultSelector);
        IObservable<TResult> CombineLatest<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, TResult>(IObservable<T1> source1, IObservable<T2> source2, IObservable<T3> source3, IObservable<T4> source4, IObservable<T5> source5, IObservable<T6> source6, IObservable<T7> source7, IObservable<T8> source8, IObservable<T9> source9, IObservable<T10> source10, IObservable<T11> source11, Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, TResult> resultSelector);
        IObservable<TResult> CombineLatest<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, TResult>(IObservable<T1> source1, IObservable<T2> source2, IObservable<T3> source3, IObservable<T4> source4, IObservable<T5> source5, IObservable<T6> source6, IObservable<T7> source7, IObservable<T8> source8, IObservable<T9> source9, IObservable<T10> source10, IObservable<T11> source11, IObservable<T12> source12, Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, TResult> resultSelector);
        IObservable<TResult> CombineLatest<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, TResult>(IObservable<T1> source1, IObservable<T2> source2, IObservable<T3> source3, IObservable<T4> source4, IObservable<T5> source5, IObservable<T6> source6, IObservable<T7> source7, IObservable<T8> source8, IObservable<T9> source9, IObservable<T10> source10, IObservable<T11> source11, IObservable<T12> source12, IObservable<T13> source13, Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, TResult> resultSelector);
        IObservable<TResult> CombineLatest<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, TResult>(IObservable<T1> source1, IObservable<T2> source2, IObservable<T3> source3, IObservable<T4> source4, IObservable<T5> source5, IObservable<T6> source6, IObservable<T7> source7, IObservable<T8> source8, IObservable<T9> source9, IObservable<T10> source10, IObservable<T11> source11, IObservable<T12> source12, IObservable<T13> source13, IObservable<T14> source14, Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, TResult> resultSelector);
        IObservable<TResult> CombineLatest<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, TResult>(IObservable<T1> source1, IObservable<T2> source2, IObservable<T3> source3, IObservable<T4> source4, IObservable<T5> source5, IObservable<T6> source6, IObservable<T7> source7, IObservable<T8> source8, IObservable<T9> source9, IObservable<T10> source10, IObservable<T11> source11, IObservable<T12> source12, IObservable<T13> source13, IObservable<T14> source14, IObservable<T15> source15, Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, TResult> resultSelector);
        IObservable<TResult> CombineLatest<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16, TResult>(IObservable<T1> source1, IObservable<T2> source2, IObservable<T3> source3, IObservable<T4> source4, IObservable<T5> source5, IObservable<T6> source6, IObservable<T7> source7, IObservable<T8> source8, IObservable<T9> source9, IObservable<T10> source10, IObservable<T11> source11, IObservable<T12> source12, IObservable<T13> source13, IObservable<T14> source14, IObservable<T15> source15, IObservable<T16> source16, Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16, TResult> resultSelector);
        IObservable<TResult> CombineLatest<TSource, TResult>(IEnumerable<IObservable<TSource>> sources, Func<IList<TSource>, TResult> resultSelector);
        IObservable<IList<TSource>> CombineLatest<TSource>(IEnumerable<IObservable<TSource>> sources);
        IObservable<IList<TSource>> CombineLatest<TSource>(params IObservable<TSource>[] sources);
        IObservable<TSource> Concat<TSource>(IObservable<TSource> first, IObservable<TSource> second);
        IObservable<TSource> Concat<TSource>(params IObservable<TSource>[] sources);
        IObservable<TSource> Concat<TSource>(IEnumerable<IObservable<TSource>> sources);
        IObservable<TSource> Concat<TSource>(IObservable<IObservable<TSource>> sources);
        IObservable<TSource> Merge<TSource>(IObservable<IObservable<TSource>> sources);
        IObservable<TSource> Merge<TSource>(IObservable<IObservable<TSource>> sources, int maxConcurrent);
        IObservable<TSource> Merge<TSource>(IEnumerable<IObservable<TSource>> sources, int maxConcurrent);
        IObservable<TSource> Merge<TSource>(IEnumerable<IObservable<TSource>> sources, int maxConcurrent, IScheduler scheduler);
        IObservable<TSource> Merge<TSource>(IObservable<TSource> first, IObservable<TSource> second);
        IObservable<TSource> Merge<TSource>(IObservable<TSource> first, IObservable<TSource> second, IScheduler scheduler);
        IObservable<TSource> Merge<TSource>(params IObservable<TSource>[] sources);
        IObservable<TSource> Merge<TSource>(IScheduler scheduler, params IObservable<TSource>[] sources);
        IObservable<TSource> Merge<TSource>(IEnumerable<IObservable<TSource>> sources);
        IObservable<TSource> Merge<TSource>(IEnumerable<IObservable<TSource>> sources, IScheduler scheduler);
        IObservable<TSource> OnErrorResumeNext<TSource>(IObservable<TSource> first, IObservable<TSource> second);
        IObservable<TSource> OnErrorResumeNext<TSource>(params IObservable<TSource>[] sources);
        IObservable<TSource> OnErrorResumeNext<TSource>(IEnumerable<IObservable<TSource>> sources);
        IObservable<TSource> SkipUntil<TSource, TOther>(IObservable<TSource> source, IObservable<TOther> other);
        IObservable<TSource> Switch<TSource>(IObservable<IObservable<TSource>> sources);
        IObservable<TSource> TakeUntil<TSource, TOther>(IObservable<TSource> source, IObservable<TOther> other);
        IObservable<TSource> TakeUntil<TSource>(IObservable<TSource> source, Func<TSource, bool> stopPredicate);
        IObservable<IObservable<TSource>> Window<TSource, TWindowClosing>(IObservable<TSource> source, Func<IObservable<TWindowClosing>> windowClosingSelector);
        IObservable<IObservable<TSource>> Window<TSource, TWindowOpening, TWindowClosing>(IObservable<TSource> source, IObservable<TWindowOpening> windowOpenings, Func<TWindowOpening, IObservable<TWindowClosing>> windowClosingSelector);
        IObservable<IObservable<TSource>> Window<TSource, TWindowBoundary>(IObservable<TSource> source, IObservable<TWindowBoundary> windowBoundaries);
        IObservable<TResult> WithLatestFrom<TFirst, TSecond, TResult>(IObservable<TFirst> first, IObservable<TSecond> second, Func<TFirst, TSecond, TResult> resultSelector);
        IObservable<TResult> Zip<TFirst, TSecond, TResult>(IObservable<TFirst> first, IObservable<TSecond> second, Func<TFirst, TSecond, TResult> resultSelector);
        IObservable<TResult> Zip<TSource, TResult>(IEnumerable<IObservable<TSource>> sources, Func<IList<TSource>, TResult> resultSelector);
        IObservable<IList<TSource>> Zip<TSource>(IEnumerable<IObservable<TSource>> sources);
        IObservable<IList<TSource>> Zip<TSource>(params IObservable<TSource>[] sources);
        IObservable<TResult> Zip<T1, T2, T3, TResult>(IObservable<T1> source1, IObservable<T2> source2, IObservable<T3> source3, Func<T1, T2, T3, TResult> resultSelector);
        IObservable<TResult> Zip<T1, T2, T3, T4, TResult>(IObservable<T1> source1, IObservable<T2> source2, IObservable<T3> source3, IObservable<T4> source4, Func<T1, T2, T3, T4, TResult> resultSelector);
        IObservable<TResult> Zip<T1, T2, T3, T4, T5, TResult>(IObservable<T1> source1, IObservable<T2> source2, IObservable<T3> source3, IObservable<T4> source4, IObservable<T5> source5, Func<T1, T2, T3, T4, T5, TResult> resultSelector);
        IObservable<TResult> Zip<T1, T2, T3, T4, T5, T6, TResult>(IObservable<T1> source1, IObservable<T2> source2, IObservable<T3> source3, IObservable<T4> source4, IObservable<T5> source5, IObservable<T6> source6, Func<T1, T2, T3, T4, T5, T6, TResult> resultSelector);
        IObservable<TResult> Zip<T1, T2, T3, T4, T5, T6, T7, TResult>(IObservable<T1> source1, IObservable<T2> source2, IObservable<T3> source3, IObservable<T4> source4, IObservable<T5> source5, IObservable<T6> source6, IObservable<T7> source7, Func<T1, T2, T3, T4, T5, T6, T7, TResult> resultSelector);
        IObservable<TResult> Zip<T1, T2, T3, T4, T5, T6, T7, T8, TResult>(IObservable<T1> source1, IObservable<T2> source2, IObservable<T3> source3, IObservable<T4> source4, IObservable<T5> source5, IObservable<T6> source6, IObservable<T7> source7, IObservable<T8> source8, Func<T1, T2, T3, T4, T5, T6, T7, T8, TResult> resultSelector);
        IObservable<TResult> Zip<T1, T2, T3, T4, T5, T6, T7, T8, T9, TResult>(IObservable<T1> source1, IObservable<T2> source2, IObservable<T3> source3, IObservable<T4> source4, IObservable<T5> source5, IObservable<T6> source6, IObservable<T7> source7, IObservable<T8> source8, IObservable<T9> source9, Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, TResult> resultSelector);
        IObservable<TResult> Zip<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, TResult>(IObservable<T1> source1, IObservable<T2> source2, IObservable<T3> source3, IObservable<T4> source4, IObservable<T5> source5, IObservable<T6> source6, IObservable<T7> source7, IObservable<T8> source8, IObservable<T9> source9, IObservable<T10> source10, Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, TResult> resultSelector);
        IObservable<TResult> Zip<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, TResult>(IObservable<T1> source1, IObservable<T2> source2, IObservable<T3> source3, IObservable<T4> source4, IObservable<T5> source5, IObservable<T6> source6, IObservable<T7> source7, IObservable<T8> source8, IObservable<T9> source9, IObservable<T10> source10, IObservable<T11> source11, Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, TResult> resultSelector);
        IObservable<TResult> Zip<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, TResult>(IObservable<T1> source1, IObservable<T2> source2, IObservable<T3> source3, IObservable<T4> source4, IObservable<T5> source5, IObservable<T6> source6, IObservable<T7> source7, IObservable<T8> source8, IObservable<T9> source9, IObservable<T10> source10, IObservable<T11> source11, IObservable<T12> source12, Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, TResult> resultSelector);
        IObservable<TResult> Zip<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, TResult>(IObservable<T1> source1, IObservable<T2> source2, IObservable<T3> source3, IObservable<T4> source4, IObservable<T5> source5, IObservable<T6> source6, IObservable<T7> source7, IObservable<T8> source8, IObservable<T9> source9, IObservable<T10> source10, IObservable<T11> source11, IObservable<T12> source12, IObservable<T13> source13, Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, TResult> resultSelector);
        IObservable<TResult> Zip<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, TResult>(IObservable<T1> source1, IObservable<T2> source2, IObservable<T3> source3, IObservable<T4> source4, IObservable<T5> source5, IObservable<T6> source6, IObservable<T7> source7, IObservable<T8> source8, IObservable<T9> source9, IObservable<T10> source10, IObservable<T11> source11, IObservable<T12> source12, IObservable<T13> source13, IObservable<T14> source14, Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, TResult> resultSelector);
        IObservable<TResult> Zip<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, TResult>(IObservable<T1> source1, IObservable<T2> source2, IObservable<T3> source3, IObservable<T4> source4, IObservable<T5> source5, IObservable<T6> source6, IObservable<T7> source7, IObservable<T8> source8, IObservable<T9> source9, IObservable<T10> source10, IObservable<T11> source11, IObservable<T12> source12, IObservable<T13> source13, IObservable<T14> source14, IObservable<T15> source15, Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, TResult> resultSelector);
        IObservable<TResult> Zip<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16, TResult>(IObservable<T1> source1, IObservable<T2> source2, IObservable<T3> source3, IObservable<T4> source4, IObservable<T5> source5, IObservable<T6> source6, IObservable<T7> source7, IObservable<T8> source8, IObservable<T9> source9, IObservable<T10> source10, IObservable<T11> source11, IObservable<T12> source12, IObservable<T13> source13, IObservable<T14> source14, IObservable<T15> source15, IObservable<T16> source16, Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16, TResult> resultSelector);
        IObservable<TResult> Zip<TFirst, TSecond, TResult>(IObservable<TFirst> first, IEnumerable<TSecond> second, Func<TFirst, TSecond, TResult> resultSelector);

        IObservable<TSource> Concat<TSource>(IObservable<Task<TSource>> sources);
        IObservable<TSource> Merge<TSource>(IObservable<Task<TSource>> sources);
        IObservable<TSource> Switch<TSource>(IObservable<Task<TSource>> sources);

        #endregion

        #region * Single *

        IObservable<TSource> Append<TSource>(IObservable<TSource> source, TSource value);
        IObservable<TSource> Append<TSource>(IObservable<TSource> source, TSource value, IScheduler scheduler);
        IObservable<TSource> AsObservable<TSource>(IObservable<TSource> source);
        IObservable<IList<TSource>> Buffer<TSource>(IObservable<TSource> source, int count);
        IObservable<IList<TSource>> Buffer<TSource>(IObservable<TSource> source, int count, int skip);
        IObservable<TSource> Dematerialize<TSource>(IObservable<Notification<TSource>> source);
        IObservable<TSource> DistinctUntilChanged<TSource>(IObservable<TSource> source);
        IObservable<TSource> DistinctUntilChanged<TSource>(IObservable<TSource> source, IEqualityComparer<TSource> comparer);
        IObservable<TSource> DistinctUntilChanged<TSource, TKey>(IObservable<TSource> source, Func<TSource, TKey> keySelector);
        IObservable<TSource> DistinctUntilChanged<TSource, TKey>(IObservable<TSource> source, Func<TSource, TKey> keySelector, IEqualityComparer<TKey> comparer);
        IObservable<TSource> Do<TSource>(IObservable<TSource> source, Action<TSource> onNext);
        IObservable<TSource> Do<TSource>(IObservable<TSource> source, Action<TSource> onNext, Action onCompleted);
        IObservable<TSource> Do<TSource>(IObservable<TSource> source, Action<TSource> onNext, Action<Exception> onError);
        IObservable<TSource> Do<TSource>(IObservable<TSource> source, Action<TSource> onNext, Action<Exception> onError, Action onCompleted);
        IObservable<TSource> Do<TSource>(IObservable<TSource> source, IObserver<TSource> observer);
        IObservable<TSource> Finally<TSource>(IObservable<TSource> source, Action finallyAction);
        IObservable<TSource> IgnoreElements<TSource>(IObservable<TSource> source);
        IObservable<Notification<TSource>> Materialize<TSource>(IObservable<TSource> source);
        IObservable<TSource> Prepend<TSource>(IObservable<TSource> source, TSource value);
        IObservable<TSource> Prepend<TSource>(IObservable<TSource> source, TSource value, IScheduler scheduler);
        IObservable<TSource> Repeat<TSource>(IObservable<TSource> source);
        IObservable<TSource> Repeat<TSource>(IObservable<TSource> source, int repeatCount);
        IObservable<TSource> RepeatWhen<TSource, TSignal>(IObservable<TSource> source, Func<IObservable<object>, IObservable<TSignal>> handler);
        IObservable<TSource> Retry<TSource>(IObservable<TSource> source);
        IObservable<TSource> Retry<TSource>(IObservable<TSource> source, int retryCount);
        IObservable<TSource> RetryWhen<TSource, TSignal>(IObservable<TSource> source, Func<IObservable<Exception>, IObservable<TSignal>> handler);
        IObservable<TAccumulate> Scan<TSource, TAccumulate>(IObservable<TSource> source, TAccumulate seed, Func<TAccumulate, TSource, TAccumulate> accumulator);
        IObservable<TSource> Scan<TSource>(IObservable<TSource> source, Func<TSource, TSource, TSource> accumulator);
        IObservable<TSource> SkipLast<TSource>(IObservable<TSource> source, int count);
        IObservable<TSource> StartWith<TSource>(IObservable<TSource> source, params TSource[] values);
        IObservable<TSource> StartWith<TSource>(IObservable<TSource> source, IScheduler scheduler, params TSource[] values);
        IObservable<TSource> StartWith<TSource>(IObservable<TSource> source, IEnumerable<TSource> values);
        IObservable<TSource> StartWith<TSource>(IObservable<TSource> source, IScheduler scheduler, IEnumerable<TSource> values);
        IObservable<TSource> TakeLast<TSource>(IObservable<TSource> source, int count);
        IObservable<TSource> TakeLast<TSource>(IObservable<TSource> source, int count, IScheduler scheduler);
        IObservable<IList<TSource>> TakeLastBuffer<TSource>(IObservable<TSource> source, int count);
        IObservable<IObservable<TSource>> Window<TSource>(IObservable<TSource> source, int count, int skip);
        IObservable<IObservable<TSource>> Window<TSource>(IObservable<TSource> source, int count);

        #endregion

        #region * StandardSequenceOperators *

        IObservable<TResult> Cast<TResult>(IObservable<object> source);
        IObservable<TSource> DefaultIfEmpty<TSource>(IObservable<TSource> source);
        IObservable<TSource> DefaultIfEmpty<TSource>(IObservable<TSource> source, TSource defaultValue);
        IObservable<TSource> Distinct<TSource>(IObservable<TSource> source);
        IObservable<TSource> Distinct<TSource>(IObservable<TSource> source, IEqualityComparer<TSource> comparer);
        IObservable<TSource> Distinct<TSource, TKey>(IObservable<TSource> source, Func<TSource, TKey> keySelector);
        IObservable<TSource> Distinct<TSource, TKey>(IObservable<TSource> source, Func<TSource, TKey> keySelector, IEqualityComparer<TKey> comparer);
        IObservable<IGroupedObservable<TKey, TElement>> GroupBy<TSource, TKey, TElement>(IObservable<TSource> source, Func<TSource, TKey> keySelector, Func<TSource, TElement> elementSelector);
        IObservable<IGroupedObservable<TKey, TSource>> GroupBy<TSource, TKey>(IObservable<TSource> source, Func<TSource, TKey> keySelector, IEqualityComparer<TKey> comparer);
        IObservable<IGroupedObservable<TKey, TSource>> GroupBy<TSource, TKey>(IObservable<TSource> source, Func<TSource, TKey> keySelector);
        IObservable<IGroupedObservable<TKey, TElement>> GroupBy<TSource, TKey, TElement>(IObservable<TSource> source, Func<TSource, TKey> keySelector, Func<TSource, TElement> elementSelector, IEqualityComparer<TKey> comparer);
        IObservable<IGroupedObservable<TKey, TElement>> GroupBy<TSource, TKey, TElement>(IObservable<TSource> source, Func<TSource, TKey> keySelector, Func<TSource, TElement> elementSelector, int capacity);
        IObservable<IGroupedObservable<TKey, TSource>> GroupBy<TSource, TKey>(IObservable<TSource> source, Func<TSource, TKey> keySelector, int capacity, IEqualityComparer<TKey> comparer);
        IObservable<IGroupedObservable<TKey, TSource>> GroupBy<TSource, TKey>(IObservable<TSource> source, Func<TSource, TKey> keySelector, int capacity);
        IObservable<IGroupedObservable<TKey, TElement>> GroupBy<TSource, TKey, TElement>(IObservable<TSource> source, Func<TSource, TKey> keySelector, Func<TSource, TElement> elementSelector, int capacity, IEqualityComparer<TKey> comparer);
        IObservable<IGroupedObservable<TKey, TElement>> GroupByUntil<TSource, TKey, TElement, TDuration>(IObservable<TSource> source, Func<TSource, TKey> keySelector, Func<TSource, TElement> elementSelector, Func<IGroupedObservable<TKey, TElement>, IObservable<TDuration>> durationSelector, IEqualityComparer<TKey> comparer);
        IObservable<IGroupedObservable<TKey, TElement>> GroupByUntil<TSource, TKey, TElement, TDuration>(IObservable<TSource> source, Func<TSource, TKey> keySelector, Func<TSource, TElement> elementSelector, Func<IGroupedObservable<TKey, TElement>, IObservable<TDuration>> durationSelector);
        IObservable<IGroupedObservable<TKey, TSource>> GroupByUntil<TSource, TKey, TDuration>(IObservable<TSource> source, Func<TSource, TKey> keySelector, Func<IGroupedObservable<TKey, TSource>, IObservable<TDuration>> durationSelector, IEqualityComparer<TKey> comparer);
        IObservable<IGroupedObservable<TKey, TSource>> GroupByUntil<TSource, TKey, TDuration>(IObservable<TSource> source, Func<TSource, TKey> keySelector, Func<IGroupedObservable<TKey, TSource>, IObservable<TDuration>> durationSelector);
        IObservable<IGroupedObservable<TKey, TElement>> GroupByUntil<TSource, TKey, TElement, TDuration>(IObservable<TSource> source, Func<TSource, TKey> keySelector, Func<TSource, TElement> elementSelector, Func<IGroupedObservable<TKey, TElement>, IObservable<TDuration>> durationSelector, int capacity, IEqualityComparer<TKey> comparer);
        IObservable<IGroupedObservable<TKey, TElement>> GroupByUntil<TSource, TKey, TElement, TDuration>(IObservable<TSource> source, Func<TSource, TKey> keySelector, Func<TSource, TElement> elementSelector, Func<IGroupedObservable<TKey, TElement>, IObservable<TDuration>> durationSelector, int capacity);
        IObservable<IGroupedObservable<TKey, TSource>> GroupByUntil<TSource, TKey, TDuration>(IObservable<TSource> source, Func<TSource, TKey> keySelector, Func<IGroupedObservable<TKey, TSource>, IObservable<TDuration>> durationSelector, int capacity, IEqualityComparer<TKey> comparer);
        IObservable<IGroupedObservable<TKey, TSource>> GroupByUntil<TSource, TKey, TDuration>(IObservable<TSource> source, Func<TSource, TKey> keySelector, Func<IGroupedObservable<TKey, TSource>, IObservable<TDuration>> durationSelector, int capacity);
        IObservable<TResult> GroupJoin<TLeft, TRight, TLeftDuration, TRightDuration, TResult>(IObservable<TLeft> left, IObservable<TRight> right, Func<TLeft, IObservable<TLeftDuration>> leftDurationSelector, Func<TRight, IObservable<TRightDuration>> rightDurationSelector, Func<TLeft, IObservable<TRight>, TResult> resultSelector);
        IObservable<TResult> Join<TLeft, TRight, TLeftDuration, TRightDuration, TResult>(IObservable<TLeft> left, IObservable<TRight> right, Func<TLeft, IObservable<TLeftDuration>> leftDurationSelector, Func<TRight, IObservable<TRightDuration>> rightDurationSelector, Func<TLeft, TRight, TResult> resultSelector);
        IObservable<TResult> OfType<TResult>(IObservable<object> source);
        IObservable<TResult> Select<TSource, TResult>(IObservable<TSource> source, Func<TSource, TResult> selector);
        IObservable<TResult> Select<TSource, TResult>(IObservable<TSource> source, Func<TSource, int, TResult> selector);
        IObservable<TOther> SelectMany<TSource, TOther>(IObservable<TSource> source, IObservable<TOther> other);
        IObservable<TResult> SelectMany<TSource, TResult>(IObservable<TSource> source, Func<TSource, IObservable<TResult>> selector);
        IObservable<TResult> SelectMany<TSource, TResult>(IObservable<TSource> source, Func<TSource, int, IObservable<TResult>> selector);
        IObservable<TResult> SelectMany<TSource, TCollection, TResult>(IObservable<TSource> source, Func<TSource, IObservable<TCollection>> collectionSelector, Func<TSource, TCollection, TResult> resultSelector);
        IObservable<TResult> SelectMany<TSource, TCollection, TResult>(IObservable<TSource> source, Func<TSource, int, IObservable<TCollection>> collectionSelector, Func<TSource, int, TCollection, int, TResult> resultSelector);
        IObservable<TResult> SelectMany<TSource, TResult>(IObservable<TSource> source, Func<TSource, IObservable<TResult>> onNext, Func<Exception, IObservable<TResult>> onError, Func<IObservable<TResult>> onCompleted);
        IObservable<TResult> SelectMany<TSource, TResult>(IObservable<TSource> source, Func<TSource, int, IObservable<TResult>> onNext, Func<Exception, IObservable<TResult>> onError, Func<IObservable<TResult>> onCompleted);
        IObservable<TResult> SelectMany<TSource, TResult>(IObservable<TSource> source, Func<TSource, IEnumerable<TResult>> selector);
        IObservable<TResult> SelectMany<TSource, TResult>(IObservable<TSource> source, Func<TSource, int, IEnumerable<TResult>> selector);
        IObservable<TResult> SelectMany<TSource, TCollection, TResult>(IObservable<TSource> source, Func<TSource, IEnumerable<TCollection>> collectionSelector, Func<TSource, TCollection, TResult> resultSelector);
        IObservable<TResult> SelectMany<TSource, TCollection, TResult>(IObservable<TSource> source, Func<TSource, int, IEnumerable<TCollection>> collectionSelector, Func<TSource, int, TCollection, int, TResult> resultSelector);
        IObservable<TSource> Skip<TSource>(IObservable<TSource> source, int count);
        IObservable<TSource> SkipWhile<TSource>(IObservable<TSource> source, Func<TSource, bool> predicate);
        IObservable<TSource> SkipWhile<TSource>(IObservable<TSource> source, Func<TSource, int, bool> predicate);
        IObservable<TSource> Take<TSource>(IObservable<TSource> source, int count);
        IObservable<TSource> Take<TSource>(IObservable<TSource> source, int count, IScheduler scheduler);
        IObservable<TSource> TakeWhile<TSource>(IObservable<TSource> source, Func<TSource, bool> predicate);
        IObservable<TSource> TakeWhile<TSource>(IObservable<TSource> source, Func<TSource, int, bool> predicate);
        IObservable<TSource> Where<TSource>(IObservable<TSource> source, Func<TSource, bool> predicate);
        IObservable<TSource> Where<TSource>(IObservable<TSource> source, Func<TSource, int, bool> predicate);

        IObservable<TResult> SelectMany<TSource, TResult>(IObservable<TSource> source, Func<TSource, Task<TResult>> selector);
        IObservable<TResult> SelectMany<TSource, TResult>(IObservable<TSource> source, Func<TSource, int, Task<TResult>> selector);
        IObservable<TResult> SelectMany<TSource, TResult>(IObservable<TSource> source, Func<TSource, CancellationToken, Task<TResult>> selector);
        IObservable<TResult> SelectMany<TSource, TResult>(IObservable<TSource> source, Func<TSource, int, CancellationToken, Task<TResult>> selector);
        IObservable<TResult> SelectMany<TSource, TTaskResult, TResult>(IObservable<TSource> source, Func<TSource, Task<TTaskResult>> taskSelector, Func<TSource, TTaskResult, TResult> resultSelector);
        IObservable<TResult> SelectMany<TSource, TTaskResult, TResult>(IObservable<TSource> source, Func<TSource, int, Task<TTaskResult>> taskSelector, Func<TSource, int, TTaskResult, TResult> resultSelector);
        IObservable<TResult> SelectMany<TSource, TTaskResult, TResult>(IObservable<TSource> source, Func<TSource, CancellationToken, Task<TTaskResult>> taskSelector, Func<TSource, TTaskResult, TResult> resultSelector);
        IObservable<TResult> SelectMany<TSource, TTaskResult, TResult>(IObservable<TSource> source, Func<TSource, int, CancellationToken, Task<TTaskResult>> taskSelector, Func<TSource, int, TTaskResult, TResult> resultSelector);

        #endregion

        #region * Time *

        IObservable<IList<TSource>> Buffer<TSource>(IObservable<TSource> source, TimeSpan timeSpan);
        IObservable<IList<TSource>> Buffer<TSource>(IObservable<TSource> source, TimeSpan timeSpan, IScheduler scheduler);
        IObservable<IList<TSource>> Buffer<TSource>(IObservable<TSource> source, TimeSpan timeSpan, TimeSpan timeShift);
        IObservable<IList<TSource>> Buffer<TSource>(IObservable<TSource> source, TimeSpan timeSpan, TimeSpan timeShift, IScheduler scheduler);
        IObservable<IList<TSource>> Buffer<TSource>(IObservable<TSource> source, TimeSpan timeSpan, int count);
        IObservable<IList<TSource>> Buffer<TSource>(IObservable<TSource> source, TimeSpan timeSpan, int count, IScheduler scheduler);
        IObservable<TSource> Delay<TSource>(IObservable<TSource> source, TimeSpan dueTime);
        IObservable<TSource> Delay<TSource>(IObservable<TSource> source, TimeSpan dueTime, IScheduler scheduler);
        IObservable<TSource> Delay<TSource>(IObservable<TSource> source, DateTimeOffset dueTime);
        IObservable<TSource> Delay<TSource>(IObservable<TSource> source, DateTimeOffset dueTime, IScheduler scheduler);
        IObservable<TSource> Delay<TSource, TDelay>(IObservable<TSource> source, Func<TSource, IObservable<TDelay>> delayDurationSelector);
        IObservable<TSource> Delay<TSource, TDelay>(IObservable<TSource> source, IObservable<TDelay> subscriptionDelay, Func<TSource, IObservable<TDelay>> delayDurationSelector);
        IObservable<TSource> DelaySubscription<TSource>(IObservable<TSource> source, TimeSpan dueTime);
        IObservable<TSource> DelaySubscription<TSource>(IObservable<TSource> source, TimeSpan dueTime, IScheduler scheduler);
        IObservable<TSource> DelaySubscription<TSource>(IObservable<TSource> source, DateTimeOffset dueTime);
        IObservable<TSource> DelaySubscription<TSource>(IObservable<TSource> source, DateTimeOffset dueTime, IScheduler scheduler);
        IObservable<TResult> Generate<TState, TResult>(TState initialState, Func<TState, bool> condition, Func<TState, TState> iterate, Func<TState, TResult> resultSelector, Func<TState, TimeSpan> timeSelector);
        IObservable<TResult> Generate<TState, TResult>(TState initialState, Func<TState, bool> condition, Func<TState, TState> iterate, Func<TState, TResult> resultSelector, Func<TState, TimeSpan> timeSelector, IScheduler scheduler);
        IObservable<TResult> Generate<TState, TResult>(TState initialState, Func<TState, bool> condition, Func<TState, TState> iterate, Func<TState, TResult> resultSelector, Func<TState, DateTimeOffset> timeSelector);
        IObservable<TResult> Generate<TState, TResult>(TState initialState, Func<TState, bool> condition, Func<TState, TState> iterate, Func<TState, TResult> resultSelector, Func<TState, DateTimeOffset> timeSelector, IScheduler scheduler);
        IObservable<long> Interval(TimeSpan period);
        IObservable<long> Interval(TimeSpan period, IScheduler scheduler);
        IObservable<TSource> Sample<TSource>(IObservable<TSource> source, TimeSpan interval);
        IObservable<TSource> Sample<TSource>(IObservable<TSource> source, TimeSpan interval, IScheduler scheduler);
        IObservable<TSource> Sample<TSource, TSample>(IObservable<TSource> source, IObservable<TSample> sampler);
        IObservable<TSource> Skip<TSource>(IObservable<TSource> source, TimeSpan duration);
        IObservable<TSource> Skip<TSource>(IObservable<TSource> source, TimeSpan duration, IScheduler scheduler);
        IObservable<TSource> SkipLast<TSource>(IObservable<TSource> source, TimeSpan duration);
        IObservable<TSource> SkipLast<TSource>(IObservable<TSource> source, TimeSpan duration, IScheduler scheduler);
        IObservable<TSource> SkipUntil<TSource>(IObservable<TSource> source, DateTimeOffset startTime);
        IObservable<TSource> SkipUntil<TSource>(IObservable<TSource> source, DateTimeOffset startTime, IScheduler scheduler);
        IObservable<TSource> Take<TSource>(IObservable<TSource> source, TimeSpan duration);
        IObservable<TSource> Take<TSource>(IObservable<TSource> source, TimeSpan duration, IScheduler scheduler);
        IObservable<TSource> TakeLast<TSource>(IObservable<TSource> source, TimeSpan duration);
        IObservable<TSource> TakeLast<TSource>(IObservable<TSource> source, TimeSpan duration, IScheduler scheduler);
        IObservable<TSource> TakeLast<TSource>(IObservable<TSource> source, TimeSpan duration, IScheduler timerScheduler, IScheduler loopScheduler);
        IObservable<IList<TSource>> TakeLastBuffer<TSource>(IObservable<TSource> source, TimeSpan duration);
        IObservable<IList<TSource>> TakeLastBuffer<TSource>(IObservable<TSource> source, TimeSpan duration, IScheduler scheduler);
        IObservable<TSource> TakeUntil<TSource>(IObservable<TSource> source, DateTimeOffset endTime);
        IObservable<TSource> TakeUntil<TSource>(IObservable<TSource> source, DateTimeOffset endTime, IScheduler scheduler);
        IObservable<TSource> Throttle<TSource>(IObservable<TSource> source, TimeSpan dueTime);
        IObservable<TSource> Throttle<TSource>(IObservable<TSource> source, TimeSpan dueTime, IScheduler scheduler);
        IObservable<TSource> Throttle<TSource, TThrottle>(IObservable<TSource> source, Func<TSource, IObservable<TThrottle>> throttleDurationSelector);
        IObservable<TimeInterval<TSource>> TimeInterval<TSource>(IObservable<TSource> source);
        IObservable<TimeInterval<TSource>> TimeInterval<TSource>(IObservable<TSource> source, IScheduler scheduler);
        IObservable<TSource> Timeout<TSource>(IObservable<TSource> source, TimeSpan dueTime);
        IObservable<TSource> Timeout<TSource>(IObservable<TSource> source, TimeSpan dueTime, IScheduler scheduler);
        IObservable<TSource> Timeout<TSource>(IObservable<TSource> source, TimeSpan dueTime, IObservable<TSource> other);
        IObservable<TSource> Timeout<TSource>(IObservable<TSource> source, TimeSpan dueTime, IObservable<TSource> other, IScheduler scheduler);
        IObservable<TSource> Timeout<TSource>(IObservable<TSource> source, DateTimeOffset dueTime);
        IObservable<TSource> Timeout<TSource>(IObservable<TSource> source, DateTimeOffset dueTime, IScheduler scheduler);
        IObservable<TSource> Timeout<TSource>(IObservable<TSource> source, DateTimeOffset dueTime, IObservable<TSource> other);
        IObservable<TSource> Timeout<TSource>(IObservable<TSource> source, DateTimeOffset dueTime, IObservable<TSource> other, IScheduler scheduler);
        IObservable<TSource> Timeout<TSource, TTimeout>(IObservable<TSource> source, Func<TSource, IObservable<TTimeout>> timeoutDurationSelector);
        IObservable<TSource> Timeout<TSource, TTimeout>(IObservable<TSource> source, Func<TSource, IObservable<TTimeout>> timeoutDurationSelector, IObservable<TSource> other);
        IObservable<TSource> Timeout<TSource, TTimeout>(IObservable<TSource> source, IObservable<TTimeout> firstTimeout, Func<TSource, IObservable<TTimeout>> timeoutDurationSelector);
        IObservable<TSource> Timeout<TSource, TTimeout>(IObservable<TSource> source, IObservable<TTimeout> firstTimeout, Func<TSource, IObservable<TTimeout>> timeoutDurationSelector, IObservable<TSource> other);
        IObservable<long> Timer(TimeSpan dueTime);
        IObservable<long> Timer(DateTimeOffset dueTime);
        IObservable<long> Timer(TimeSpan dueTime, TimeSpan period);
        IObservable<long> Timer(DateTimeOffset dueTime, TimeSpan period);
        IObservable<long> Timer(TimeSpan dueTime, IScheduler scheduler);
        IObservable<long> Timer(DateTimeOffset dueTime, IScheduler scheduler);
        IObservable<long> Timer(TimeSpan dueTime, TimeSpan period, IScheduler scheduler);
        IObservable<long> Timer(DateTimeOffset dueTime, TimeSpan period, IScheduler scheduler);
        IObservable<Timestamped<TSource>> Timestamp<TSource>(IObservable<TSource> source);
        IObservable<Timestamped<TSource>> Timestamp<TSource>(IObservable<TSource> source, IScheduler scheduler);
        IObservable<IObservable<TSource>> Window<TSource>(IObservable<TSource> source, TimeSpan timeSpan);
        IObservable<IObservable<TSource>> Window<TSource>(IObservable<TSource> source, TimeSpan timeSpan, IScheduler scheduler);
        IObservable<IObservable<TSource>> Window<TSource>(IObservable<TSource> source, TimeSpan timeSpan, TimeSpan timeShift);
        IObservable<IObservable<TSource>> Window<TSource>(IObservable<TSource> source, TimeSpan timeSpan, TimeSpan timeShift, IScheduler scheduler);
        IObservable<IObservable<TSource>> Window<TSource>(IObservable<TSource> source, TimeSpan timeSpan, int count);
        IObservable<IObservable<TSource>> Window<TSource>(IObservable<TSource> source, TimeSpan timeSpan, int count, IScheduler scheduler);

        #endregion
    }
}

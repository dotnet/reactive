// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information.

using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace System.Linq
{
    partial class AsyncEnumerable
    {
#if SUPPORT_FLAT_ASYNC_API
        public static ValueTask<long> MaxAsync<TSource>(this IAsyncEnumerable<TSource> source, Func<TSource, ValueTask<long>> selector, CancellationToken cancellationToken = default) => MaxAwaitAsyncCore<TSource>(source, selector, cancellationToken);
        public static ValueTask<long?> MaxAsync<TSource>(this IAsyncEnumerable<TSource> source, Func<TSource, ValueTask<long?>> selector, CancellationToken cancellationToken = default) => MaxAwaitAsyncCore<TSource>(source, selector, cancellationToken);
        public static ValueTask<float> MaxAsync<TSource>(this IAsyncEnumerable<TSource> source, Func<TSource, ValueTask<float>> selector, CancellationToken cancellationToken = default) => MaxAwaitAsyncCore<TSource>(source, selector, cancellationToken);
        public static ValueTask<float?> MaxAsync<TSource>(this IAsyncEnumerable<TSource> source, Func<TSource, ValueTask<float?>> selector, CancellationToken cancellationToken = default) => MaxAwaitAsyncCore<TSource>(source, selector, cancellationToken);
        public static ValueTask<double> MaxAsync<TSource>(this IAsyncEnumerable<TSource> source, Func<TSource, ValueTask<double>> selector, CancellationToken cancellationToken = default) => MaxAwaitAsyncCore<TSource>(source, selector, cancellationToken);
        public static ValueTask<double?> MaxAsync<TSource>(this IAsyncEnumerable<TSource> source, Func<TSource, ValueTask<double?>> selector, CancellationToken cancellationToken = default) => MaxAwaitAsyncCore<TSource>(source, selector, cancellationToken);
        public static ValueTask<decimal> MaxAsync<TSource>(this IAsyncEnumerable<TSource> source, Func<TSource, ValueTask<decimal>> selector, CancellationToken cancellationToken = default) => MaxAwaitAsyncCore<TSource>(source, selector, cancellationToken);
        public static ValueTask<decimal?> MaxAsync<TSource>(this IAsyncEnumerable<TSource> source, Func<TSource, ValueTask<decimal?>> selector, CancellationToken cancellationToken = default) => MaxAwaitAsyncCore<TSource>(source, selector, cancellationToken);
        public static ValueTask<TResult> MaxAsync<TSource, TResult>(this IAsyncEnumerable<TSource> source, Func<TSource, ValueTask<TResult>> selector, CancellationToken cancellationToken = default) => MaxAwaitAsyncCore<TSource, TResult>(source, selector, cancellationToken);
        public static ValueTask<int> MaxAsync<TSource>(this IAsyncEnumerable<TSource> source, Func<TSource, ValueTask<int>> selector, CancellationToken cancellationToken = default) => MaxAwaitAsyncCore<TSource>(source, selector, cancellationToken);
        public static ValueTask<int?> MaxAsync<TSource>(this IAsyncEnumerable<TSource> source, Func<TSource, ValueTask<int?>> selector, CancellationToken cancellationToken = default) => MaxAwaitAsyncCore<TSource>(source, selector, cancellationToken);
        public static ValueTask<double> MinAsync<TSource>(this IAsyncEnumerable<TSource> source, Func<TSource, ValueTask<double>> selector, CancellationToken cancellationToken = default) => MinAwaitAsyncCore<TSource>(source, selector, cancellationToken);
        public static ValueTask<double?> MinAsync<TSource>(this IAsyncEnumerable<TSource> source, Func<TSource, ValueTask<double?>> selector, CancellationToken cancellationToken = default) => MinAwaitAsyncCore<TSource>(source, selector, cancellationToken);
        public static ValueTask<decimal> MinAsync<TSource>(this IAsyncEnumerable<TSource> source, Func<TSource, ValueTask<decimal>> selector, CancellationToken cancellationToken = default) => MinAwaitAsyncCore<TSource>(source, selector, cancellationToken);
        public static ValueTask<decimal?> MinAsync<TSource>(this IAsyncEnumerable<TSource> source, Func<TSource, ValueTask<decimal?>> selector, CancellationToken cancellationToken = default) => MinAwaitAsyncCore<TSource>(source, selector, cancellationToken);
        public static ValueTask<int> MinAsync<TSource>(this IAsyncEnumerable<TSource> source, Func<TSource, ValueTask<int>> selector, CancellationToken cancellationToken = default) => MinAwaitAsyncCore<TSource>(source, selector, cancellationToken);
        public static ValueTask<int?> MinAsync<TSource>(this IAsyncEnumerable<TSource> source, Func<TSource, ValueTask<int?>> selector, CancellationToken cancellationToken = default) => MinAwaitAsyncCore<TSource>(source, selector, cancellationToken);
        public static ValueTask<long> MinAsync<TSource>(this IAsyncEnumerable<TSource> source, Func<TSource, ValueTask<long>> selector, CancellationToken cancellationToken = default) => MinAwaitAsyncCore<TSource>(source, selector, cancellationToken);
        public static ValueTask<long?> MinAsync<TSource>(this IAsyncEnumerable<TSource> source, Func<TSource, ValueTask<long?>> selector, CancellationToken cancellationToken = default) => MinAwaitAsyncCore<TSource>(source, selector, cancellationToken);
        public static ValueTask<float> MinAsync<TSource>(this IAsyncEnumerable<TSource> source, Func<TSource, ValueTask<float>> selector, CancellationToken cancellationToken = default) => MinAwaitAsyncCore<TSource>(source, selector, cancellationToken);
        public static ValueTask<float?> MinAsync<TSource>(this IAsyncEnumerable<TSource> source, Func<TSource, ValueTask<float?>> selector, CancellationToken cancellationToken = default) => MinAwaitAsyncCore<TSource>(source, selector, cancellationToken);
        public static ValueTask<TResult> MinAsync<TSource, TResult>(this IAsyncEnumerable<TSource> source, Func<TSource, ValueTask<TResult>> selector, CancellationToken cancellationToken = default) => MinAwaitAsyncCore<TSource, TResult>(source, selector, cancellationToken);
        public static IOrderedAsyncEnumerable<TSource> OrderBy<TSource, TKey>(this IAsyncEnumerable<TSource> source, Func<TSource, ValueTask<TKey>> keySelector) => OrderByAwaitCore<TSource, TKey>(source, keySelector);
        public static IOrderedAsyncEnumerable<TSource> OrderBy<TSource, TKey>(this IAsyncEnumerable<TSource> source, Func<TSource, ValueTask<TKey>> keySelector, IComparer<TKey> comparer) => OrderByAwaitCore<TSource, TKey>(source, keySelector, comparer);
        public static IOrderedAsyncEnumerable<TSource> OrderByDescending<TSource, TKey>(this IAsyncEnumerable<TSource> source, Func<TSource, ValueTask<TKey>> keySelector) => OrderByDescendingAwaitCore<TSource, TKey>(source, keySelector);
        public static IOrderedAsyncEnumerable<TSource> OrderByDescending<TSource, TKey>(this IAsyncEnumerable<TSource> source, Func<TSource, ValueTask<TKey>> keySelector, IComparer<TKey> comparer) => OrderByDescendingAwaitCore<TSource, TKey>(source, keySelector, comparer);
        public static IAsyncEnumerable<TResult> Select<TSource, TResult>(this IAsyncEnumerable<TSource> source, Func<TSource, ValueTask<TResult>> selector) => SelectAwaitCore<TSource, TResult>(source, selector);
        public static IAsyncEnumerable<TResult> Select<TSource, TResult>(this IAsyncEnumerable<TSource> source, Func<TSource, int, ValueTask<TResult>> selector) => SelectAwaitCore<TSource, TResult>(source, selector);
        public static IAsyncEnumerable<TResult> SelectMany<TSource, TResult>(this IAsyncEnumerable<TSource> source, Func<TSource, ValueTask<IAsyncEnumerable<TResult>>> selector) => SelectManyAwaitCore<TSource, TResult>(source, selector);
        public static IAsyncEnumerable<TResult> SelectMany<TSource, TResult>(this IAsyncEnumerable<TSource> source, Func<TSource, int, ValueTask<IAsyncEnumerable<TResult>>> selector) => SelectManyAwaitCore<TSource, TResult>(source, selector);
        public static IAsyncEnumerable<TResult> SelectMany<TSource, TCollection, TResult>(this IAsyncEnumerable<TSource> source, Func<TSource, ValueTask<IAsyncEnumerable<TCollection>>> collectionSelector, Func<TSource, TCollection, ValueTask<TResult>> resultSelector) => SelectManyAwaitCore<TSource, TCollection, TResult>(source, collectionSelector, resultSelector);
        public static IAsyncEnumerable<TResult> SelectMany<TSource, TCollection, TResult>(this IAsyncEnumerable<TSource> source, Func<TSource, int, ValueTask<IAsyncEnumerable<TCollection>>> collectionSelector, Func<TSource, TCollection, ValueTask<TResult>> resultSelector) => SelectManyAwaitCore<TSource, TCollection, TResult>(source, collectionSelector, resultSelector);
        public static ValueTask<TSource> SingleAsync<TSource>(this IAsyncEnumerable<TSource> source, Func<TSource, ValueTask<bool>> predicate, CancellationToken cancellationToken = default) => SingleAwaitAsyncCore<TSource>(source, predicate, cancellationToken);
        public static ValueTask<TSource?> SingleOrDefaultAsync<TSource>(this IAsyncEnumerable<TSource> source, Func<TSource, ValueTask<bool>> predicate, CancellationToken cancellationToken = default) => SingleOrDefaultAwaitAsyncCore<TSource>(source, predicate, cancellationToken);
        public static IAsyncEnumerable<TSource> SkipWhile<TSource>(this IAsyncEnumerable<TSource> source, Func<TSource, ValueTask<bool>> predicate) => SkipWhileAwaitCore<TSource>(source, predicate);
        public static IAsyncEnumerable<TSource> SkipWhile<TSource>(this IAsyncEnumerable<TSource> source, Func<TSource, int, ValueTask<bool>> predicate) => SkipWhileAwaitCore<TSource>(source, predicate);
        public static ValueTask<long?> SumAsync<TSource>(this IAsyncEnumerable<TSource> source, Func<TSource, ValueTask<long?>> selector, CancellationToken cancellationToken = default) => SumAwaitAsyncCore<TSource>(source, selector, cancellationToken);
        public static ValueTask<float?> SumAsync<TSource>(this IAsyncEnumerable<TSource> source, Func<TSource, ValueTask<float?>> selector, CancellationToken cancellationToken = default) => SumAwaitAsyncCore<TSource>(source, selector, cancellationToken);
        public static ValueTask<double?> SumAsync<TSource>(this IAsyncEnumerable<TSource> source, Func<TSource, ValueTask<double?>> selector, CancellationToken cancellationToken = default) => SumAwaitAsyncCore<TSource>(source, selector, cancellationToken);
        public static ValueTask<decimal?> SumAsync<TSource>(this IAsyncEnumerable<TSource> source, Func<TSource, ValueTask<decimal?>> selector, CancellationToken cancellationToken = default) => SumAwaitAsyncCore<TSource>(source, selector, cancellationToken);
        public static ValueTask<int> SumAsync<TSource>(this IAsyncEnumerable<TSource> source, Func<TSource, ValueTask<int>> selector, CancellationToken cancellationToken = default) => SumAwaitAsyncCore<TSource>(source, selector, cancellationToken);
        public static ValueTask<long> SumAsync<TSource>(this IAsyncEnumerable<TSource> source, Func<TSource, ValueTask<long>> selector, CancellationToken cancellationToken = default) => SumAwaitAsyncCore<TSource>(source, selector, cancellationToken);
        public static ValueTask<float> SumAsync<TSource>(this IAsyncEnumerable<TSource> source, Func<TSource, ValueTask<float>> selector, CancellationToken cancellationToken = default) => SumAwaitAsyncCore<TSource>(source, selector, cancellationToken);
        public static ValueTask<double> SumAsync<TSource>(this IAsyncEnumerable<TSource> source, Func<TSource, ValueTask<double>> selector, CancellationToken cancellationToken = default) => SumAwaitAsyncCore<TSource>(source, selector, cancellationToken);
        public static ValueTask<decimal> SumAsync<TSource>(this IAsyncEnumerable<TSource> source, Func<TSource, ValueTask<decimal>> selector, CancellationToken cancellationToken = default) => SumAwaitAsyncCore<TSource>(source, selector, cancellationToken);
        public static ValueTask<int?> SumAsync<TSource>(this IAsyncEnumerable<TSource> source, Func<TSource, ValueTask<int?>> selector, CancellationToken cancellationToken = default) => SumAwaitAsyncCore<TSource>(source, selector, cancellationToken);
        public static IAsyncEnumerable<TSource> TakeWhile<TSource>(this IAsyncEnumerable<TSource> source, Func<TSource, ValueTask<bool>> predicate) => TakeWhileAwaitCore<TSource>(source, predicate);
        public static IAsyncEnumerable<TSource> TakeWhile<TSource>(this IAsyncEnumerable<TSource> source, Func<TSource, int, ValueTask<bool>> predicate) => TakeWhileAwaitCore<TSource>(source, predicate);
        public static IOrderedAsyncEnumerable<TSource> ThenBy<TSource, TKey>(this IOrderedAsyncEnumerable<TSource> source, Func<TSource, ValueTask<TKey>> keySelector) => ThenByAwaitCore<TSource, TKey>(source, keySelector);
        public static IOrderedAsyncEnumerable<TSource> ThenBy<TSource, TKey>(this IOrderedAsyncEnumerable<TSource> source, Func<TSource, ValueTask<TKey>> keySelector, IComparer<TKey> comparer) => ThenByAwaitCore<TSource, TKey>(source, keySelector, comparer);
        public static IOrderedAsyncEnumerable<TSource> ThenByDescending<TSource, TKey>(this IOrderedAsyncEnumerable<TSource> source, Func<TSource, ValueTask<TKey>> keySelector) => ThenByDescendingAwaitCore<TSource, TKey>(source, keySelector);
        public static IOrderedAsyncEnumerable<TSource> ThenByDescending<TSource, TKey>(this IOrderedAsyncEnumerable<TSource> source, Func<TSource, ValueTask<TKey>> keySelector, IComparer<TKey> comparer) => ThenByDescendingAwaitCore<TSource, TKey>(source, keySelector, comparer);
        public static ValueTask<Dictionary<TKey, TSource>> ToDictionaryAsync<TSource, TKey>(this IAsyncEnumerable<TSource> source, Func<TSource, ValueTask<TKey>> keySelector, CancellationToken cancellationToken = default) where TKey : notnull => ToDictionaryAwaitAsyncCore<TSource, TKey>(source, keySelector, cancellationToken);
        public static ValueTask<Dictionary<TKey, TSource>> ToDictionaryAsync<TSource, TKey>(this IAsyncEnumerable<TSource> source, Func<TSource, ValueTask<TKey>> keySelector, IEqualityComparer<TKey> comparer, CancellationToken cancellationToken = default) where TKey : notnull => ToDictionaryAwaitAsyncCore<TSource, TKey>(source, keySelector, comparer, cancellationToken);
        public static ValueTask<Dictionary<TKey, TElement>> ToDictionaryAsync<TSource, TKey, TElement>(this IAsyncEnumerable<TSource> source, Func<TSource, ValueTask<TKey>> keySelector, Func<TSource, ValueTask<TElement>> elementSelector, CancellationToken cancellationToken = default) where TKey : notnull => ToDictionaryAwaitAsyncCore<TSource, TKey, TElement>(source, keySelector, elementSelector, cancellationToken);
        public static ValueTask<Dictionary<TKey, TElement>> ToDictionaryAsync<TSource, TKey, TElement>(this IAsyncEnumerable<TSource> source, Func<TSource, ValueTask<TKey>> keySelector, Func<TSource, ValueTask<TElement>> elementSelector, IEqualityComparer<TKey> comparer, CancellationToken cancellationToken = default) where TKey : notnull => ToDictionaryAwaitAsyncCore<TSource, TKey, TElement>(source, keySelector, elementSelector, comparer, cancellationToken);
        public static ValueTask<ILookup<TKey, TSource>> ToLookupAsync<TSource, TKey>(this IAsyncEnumerable<TSource> source, Func<TSource, ValueTask<TKey>> keySelector, CancellationToken cancellationToken = default) => ToLookupAwaitAsyncCore<TSource, TKey>(source, keySelector, cancellationToken);
        public static ValueTask<ILookup<TKey, TElement>> ToLookupAsync<TSource, TKey, TElement>(this IAsyncEnumerable<TSource> source, Func<TSource, ValueTask<TKey>> keySelector, Func<TSource, ValueTask<TElement>> elementSelector, CancellationToken cancellationToken = default) => ToLookupAwaitAsyncCore<TSource, TKey, TElement>(source, keySelector, elementSelector, cancellationToken);
        public static ValueTask<ILookup<TKey, TSource>> ToLookupAsync<TSource, TKey>(this IAsyncEnumerable<TSource> source, Func<TSource, ValueTask<TKey>> keySelector, IEqualityComparer<TKey> comparer, CancellationToken cancellationToken = default) => ToLookupAwaitAsyncCore<TSource, TKey>(source, keySelector, comparer, cancellationToken);
        public static ValueTask<ILookup<TKey, TElement>> ToLookupAsync<TSource, TKey, TElement>(this IAsyncEnumerable<TSource> source, Func<TSource, ValueTask<TKey>> keySelector, Func<TSource, ValueTask<TElement>> elementSelector, IEqualityComparer<TKey> comparer, CancellationToken cancellationToken = default) => ToLookupAwaitAsyncCore<TSource, TKey, TElement>(source, keySelector, elementSelector, comparer, cancellationToken);
        public static IAsyncEnumerable<TSource> Where<TSource>(this IAsyncEnumerable<TSource> source, Func<TSource, ValueTask<bool>> predicate) => WhereAwaitCore<TSource>(source, predicate);
        public static IAsyncEnumerable<TSource> Where<TSource>(this IAsyncEnumerable<TSource> source, Func<TSource, int, ValueTask<bool>> predicate) => WhereAwaitCore<TSource>(source, predicate);
        public static IAsyncEnumerable<TResult> Zip<TFirst, TSecond, TResult>(this IAsyncEnumerable<TFirst> first, IAsyncEnumerable<TSecond> second, Func<TFirst, TSecond, ValueTask<TResult>> selector) => ZipAwaitCore<TFirst, TSecond, TResult>(first, second, selector);

#if !NO_DEEP_CANCELLATION
        public static ValueTask<long> MaxAsync<TSource>(this IAsyncEnumerable<TSource> source, Func<TSource, CancellationToken, ValueTask<long>> selector, CancellationToken cancellationToken = default) => MaxAwaitWithCancellationAsyncCore<TSource>(source, selector, cancellationToken);
        public static ValueTask<long?> MaxAsync<TSource>(this IAsyncEnumerable<TSource> source, Func<TSource, CancellationToken, ValueTask<long?>> selector, CancellationToken cancellationToken = default) => MaxAwaitWithCancellationAsyncCore<TSource>(source, selector, cancellationToken);
        public static ValueTask<float> MaxAsync<TSource>(this IAsyncEnumerable<TSource> source, Func<TSource, CancellationToken, ValueTask<float>> selector, CancellationToken cancellationToken = default) => MaxAwaitWithCancellationAsyncCore<TSource>(source, selector, cancellationToken);
        public static ValueTask<float?> MaxAsync<TSource>(this IAsyncEnumerable<TSource> source, Func<TSource, CancellationToken, ValueTask<float?>> selector, CancellationToken cancellationToken = default) => MaxAwaitWithCancellationAsyncCore<TSource>(source, selector, cancellationToken);
        public static ValueTask<double> MaxAsync<TSource>(this IAsyncEnumerable<TSource> source, Func<TSource, CancellationToken, ValueTask<double>> selector, CancellationToken cancellationToken = default) => MaxAwaitWithCancellationAsyncCore<TSource>(source, selector, cancellationToken);
        public static ValueTask<double?> MaxAsync<TSource>(this IAsyncEnumerable<TSource> source, Func<TSource, CancellationToken, ValueTask<double?>> selector, CancellationToken cancellationToken = default) => MaxAwaitWithCancellationAsyncCore<TSource>(source, selector, cancellationToken);
        public static ValueTask<decimal> MaxAsync<TSource>(this IAsyncEnumerable<TSource> source, Func<TSource, CancellationToken, ValueTask<decimal>> selector, CancellationToken cancellationToken = default) => MaxAwaitWithCancellationAsyncCore<TSource>(source, selector, cancellationToken);
        public static ValueTask<decimal?> MaxAsync<TSource>(this IAsyncEnumerable<TSource> source, Func<TSource, CancellationToken, ValueTask<decimal?>> selector, CancellationToken cancellationToken = default) => MaxAwaitWithCancellationAsyncCore<TSource>(source, selector, cancellationToken);
        public static ValueTask<TResult> MaxAsync<TSource, TResult>(this IAsyncEnumerable<TSource> source, Func<TSource, CancellationToken, ValueTask<TResult>> selector, CancellationToken cancellationToken = default) => MaxAwaitWithCancellationAsyncCore<TSource, TResult>(source, selector, cancellationToken);
        public static ValueTask<int> MaxAsync<TSource>(this IAsyncEnumerable<TSource> source, Func<TSource, CancellationToken, ValueTask<int>> selector, CancellationToken cancellationToken = default) => MaxAwaitWithCancellationAsyncCore<TSource>(source, selector, cancellationToken);
        public static ValueTask<int?> MaxAsync<TSource>(this IAsyncEnumerable<TSource> source, Func<TSource, CancellationToken, ValueTask<int?>> selector, CancellationToken cancellationToken = default) => MaxAwaitWithCancellationAsyncCore<TSource>(source, selector, cancellationToken);
        public static ValueTask<double> MinAsync<TSource>(this IAsyncEnumerable<TSource> source, Func<TSource, CancellationToken, ValueTask<double>> selector, CancellationToken cancellationToken = default) => MinAwaitWithCancellationAsyncCore<TSource>(source, selector, cancellationToken);
        public static ValueTask<double?> MinAsync<TSource>(this IAsyncEnumerable<TSource> source, Func<TSource, CancellationToken, ValueTask<double?>> selector, CancellationToken cancellationToken = default) => MinAwaitWithCancellationAsyncCore<TSource>(source, selector, cancellationToken);
        public static ValueTask<decimal> MinAsync<TSource>(this IAsyncEnumerable<TSource> source, Func<TSource, CancellationToken, ValueTask<decimal>> selector, CancellationToken cancellationToken = default) => MinAwaitWithCancellationAsyncCore<TSource>(source, selector, cancellationToken);
        public static ValueTask<decimal?> MinAsync<TSource>(this IAsyncEnumerable<TSource> source, Func<TSource, CancellationToken, ValueTask<decimal?>> selector, CancellationToken cancellationToken = default) => MinAwaitWithCancellationAsyncCore<TSource>(source, selector, cancellationToken);
        public static ValueTask<int> MinAsync<TSource>(this IAsyncEnumerable<TSource> source, Func<TSource, CancellationToken, ValueTask<int>> selector, CancellationToken cancellationToken = default) => MinAwaitWithCancellationAsyncCore<TSource>(source, selector, cancellationToken);
        public static ValueTask<int?> MinAsync<TSource>(this IAsyncEnumerable<TSource> source, Func<TSource, CancellationToken, ValueTask<int?>> selector, CancellationToken cancellationToken = default) => MinAwaitWithCancellationAsyncCore<TSource>(source, selector, cancellationToken);
        public static ValueTask<long> MinAsync<TSource>(this IAsyncEnumerable<TSource> source, Func<TSource, CancellationToken, ValueTask<long>> selector, CancellationToken cancellationToken = default) => MinAwaitWithCancellationAsyncCore<TSource>(source, selector, cancellationToken);
        public static ValueTask<long?> MinAsync<TSource>(this IAsyncEnumerable<TSource> source, Func<TSource, CancellationToken, ValueTask<long?>> selector, CancellationToken cancellationToken = default) => MinAwaitWithCancellationAsyncCore<TSource>(source, selector, cancellationToken);
        public static ValueTask<float> MinAsync<TSource>(this IAsyncEnumerable<TSource> source, Func<TSource, CancellationToken, ValueTask<float>> selector, CancellationToken cancellationToken = default) => MinAwaitWithCancellationAsyncCore<TSource>(source, selector, cancellationToken);
        public static ValueTask<float?> MinAsync<TSource>(this IAsyncEnumerable<TSource> source, Func<TSource, CancellationToken, ValueTask<float?>> selector, CancellationToken cancellationToken = default) => MinAwaitWithCancellationAsyncCore<TSource>(source, selector, cancellationToken);
        public static ValueTask<TResult> MinAsync<TSource, TResult>(this IAsyncEnumerable<TSource> source, Func<TSource, CancellationToken, ValueTask<TResult>> selector, CancellationToken cancellationToken = default) => MinAwaitWithCancellationAsyncCore<TSource, TResult>(source, selector, cancellationToken);
        public static IOrderedAsyncEnumerable<TSource> OrderBy<TSource, TKey>(this IAsyncEnumerable<TSource> source, Func<TSource, CancellationToken, ValueTask<TKey>> keySelector) => OrderByAwaitWithCancellationCore<TSource, TKey>(source, keySelector);
        public static IOrderedAsyncEnumerable<TSource> OrderBy<TSource, TKey>(this IAsyncEnumerable<TSource> source, Func<TSource, CancellationToken, ValueTask<TKey>> keySelector, IComparer<TKey> comparer) => OrderByAwaitWithCancellationCore<TSource, TKey>(source, keySelector, comparer);
        public static IOrderedAsyncEnumerable<TSource> OrderByDescending<TSource, TKey>(this IAsyncEnumerable<TSource> source, Func<TSource, CancellationToken, ValueTask<TKey>> keySelector) => OrderByDescendingAwaitWithCancellationCore<TSource, TKey>(source, keySelector);
        public static IOrderedAsyncEnumerable<TSource> OrderByDescending<TSource, TKey>(this IAsyncEnumerable<TSource> source, Func<TSource, CancellationToken, ValueTask<TKey>> keySelector, IComparer<TKey> comparer) => OrderByDescendingAwaitWithCancellationCore<TSource, TKey>(source, keySelector, comparer);
        public static IAsyncEnumerable<TResult> Select<TSource, TResult>(this IAsyncEnumerable<TSource> source, Func<TSource, CancellationToken, ValueTask<TResult>> selector) => SelectAwaitWithCancellationCore<TSource, TResult>(source, selector);
        public static IAsyncEnumerable<TResult> Select<TSource, TResult>(this IAsyncEnumerable<TSource> source, Func<TSource, int, CancellationToken, ValueTask<TResult>> selector) => SelectAwaitWithCancellationCore<TSource, TResult>(source, selector);
        public static IAsyncEnumerable<TResult> SelectMany<TSource, TResult>(this IAsyncEnumerable<TSource> source, Func<TSource, CancellationToken, ValueTask<IAsyncEnumerable<TResult>>> selector) => SelectManyAwaitWithCancellationCore<TSource, TResult>(source, selector);
        public static IAsyncEnumerable<TResult> SelectMany<TSource, TResult>(this IAsyncEnumerable<TSource> source, Func<TSource, int, CancellationToken, ValueTask<IAsyncEnumerable<TResult>>> selector) => SelectManyAwaitWithCancellationCore<TSource, TResult>(source, selector);
        public static IAsyncEnumerable<TResult> SelectMany<TSource, TCollection, TResult>(this IAsyncEnumerable<TSource> source, Func<TSource, CancellationToken, ValueTask<IAsyncEnumerable<TCollection>>> collectionSelector, Func<TSource, TCollection, CancellationToken, ValueTask<TResult>> resultSelector) => SelectManyAwaitWithCancellationCore<TSource, TCollection, TResult>(source, collectionSelector, resultSelector);
        public static IAsyncEnumerable<TResult> SelectMany<TSource, TCollection, TResult>(this IAsyncEnumerable<TSource> source, Func<TSource, int, CancellationToken, ValueTask<IAsyncEnumerable<TCollection>>> collectionSelector, Func<TSource, TCollection, CancellationToken, ValueTask<TResult>> resultSelector) => SelectManyAwaitWithCancellationCore<TSource, TCollection, TResult>(source, collectionSelector, resultSelector);
        public static ValueTask<TSource> SingleAsync<TSource>(this IAsyncEnumerable<TSource> source, Func<TSource, CancellationToken, ValueTask<bool>> predicate, CancellationToken cancellationToken = default) => SingleAwaitWithCancellationAsyncCore<TSource>(source, predicate, cancellationToken);
        public static ValueTask<TSource?> SingleOrDefaultAsync<TSource>(this IAsyncEnumerable<TSource> source, Func<TSource, CancellationToken, ValueTask<bool>> predicate, CancellationToken cancellationToken = default) => SingleOrDefaultAwaitWithCancellationAsyncCore<TSource>(source, predicate, cancellationToken);
        public static IAsyncEnumerable<TSource> SkipWhile<TSource>(this IAsyncEnumerable<TSource> source, Func<TSource, CancellationToken, ValueTask<bool>> predicate) => SkipWhileAwaitWithCancellationCore<TSource>(source, predicate);
        public static IAsyncEnumerable<TSource> SkipWhile<TSource>(this IAsyncEnumerable<TSource> source, Func<TSource, int, CancellationToken, ValueTask<bool>> predicate) => SkipWhileAwaitWithCancellationCore<TSource>(source, predicate);
        public static ValueTask<int?> SumAsync<TSource>(this IAsyncEnumerable<TSource> source, Func<TSource, CancellationToken, ValueTask<int?>> selector, CancellationToken cancellationToken = default) => SumAwaitWithCancellationAsyncCore<TSource>(source, selector, cancellationToken);
        public static ValueTask<long?> SumAsync<TSource>(this IAsyncEnumerable<TSource> source, Func<TSource, CancellationToken, ValueTask<long?>> selector, CancellationToken cancellationToken = default) => SumAwaitWithCancellationAsyncCore<TSource>(source, selector, cancellationToken);
        public static ValueTask<float?> SumAsync<TSource>(this IAsyncEnumerable<TSource> source, Func<TSource, CancellationToken, ValueTask<float?>> selector, CancellationToken cancellationToken = default) => SumAwaitWithCancellationAsyncCore<TSource>(source, selector, cancellationToken);
        public static ValueTask<double?> SumAsync<TSource>(this IAsyncEnumerable<TSource> source, Func<TSource, CancellationToken, ValueTask<double?>> selector, CancellationToken cancellationToken = default) => SumAwaitWithCancellationAsyncCore<TSource>(source, selector, cancellationToken);
        public static ValueTask<decimal?> SumAsync<TSource>(this IAsyncEnumerable<TSource> source, Func<TSource, CancellationToken, ValueTask<decimal?>> selector, CancellationToken cancellationToken = default) => SumAwaitWithCancellationAsyncCore<TSource>(source, selector, cancellationToken);
        public static ValueTask<int> SumAsync<TSource>(this IAsyncEnumerable<TSource> source, Func<TSource, CancellationToken, ValueTask<int>> selector, CancellationToken cancellationToken = default) => SumAwaitWithCancellationAsyncCore<TSource>(source, selector, cancellationToken);
        public static ValueTask<long> SumAsync<TSource>(this IAsyncEnumerable<TSource> source, Func<TSource, CancellationToken, ValueTask<long>> selector, CancellationToken cancellationToken = default) => SumAwaitWithCancellationAsyncCore<TSource>(source, selector, cancellationToken);
        public static ValueTask<float> SumAsync<TSource>(this IAsyncEnumerable<TSource> source, Func<TSource, CancellationToken, ValueTask<float>> selector, CancellationToken cancellationToken = default) => SumAwaitWithCancellationAsyncCore<TSource>(source, selector, cancellationToken);
        public static ValueTask<double> SumAsync<TSource>(this IAsyncEnumerable<TSource> source, Func<TSource, CancellationToken, ValueTask<double>> selector, CancellationToken cancellationToken = default) => SumAwaitWithCancellationAsyncCore<TSource>(source, selector, cancellationToken);
        public static ValueTask<decimal> SumAsync<TSource>(this IAsyncEnumerable<TSource> source, Func<TSource, CancellationToken, ValueTask<decimal>> selector, CancellationToken cancellationToken = default) => SumAwaitWithCancellationAsyncCore<TSource>(source, selector, cancellationToken);
        public static IAsyncEnumerable<TSource> TakeWhile<TSource>(this IAsyncEnumerable<TSource> source, Func<TSource, CancellationToken, ValueTask<bool>> predicate) => TakeWhileAwaitWithCancellationCore<TSource>(source, predicate);
        public static IAsyncEnumerable<TSource> TakeWhile<TSource>(this IAsyncEnumerable<TSource> source, Func<TSource, int, CancellationToken, ValueTask<bool>> predicate) => TakeWhileAwaitWithCancellationCore<TSource>(source, predicate);
        public static IOrderedAsyncEnumerable<TSource> ThenBy<TSource, TKey>(this IOrderedAsyncEnumerable<TSource> source, Func<TSource, CancellationToken, ValueTask<TKey>> keySelector) => ThenByAwaitWithCancellationCore<TSource, TKey>(source, keySelector);
        public static IOrderedAsyncEnumerable<TSource> ThenBy<TSource, TKey>(this IOrderedAsyncEnumerable<TSource> source, Func<TSource, CancellationToken, ValueTask<TKey>> keySelector, IComparer<TKey> comparer) => ThenByAwaitWithCancellationCore<TSource, TKey>(source, keySelector, comparer);
        public static IOrderedAsyncEnumerable<TSource> ThenByDescending<TSource, TKey>(this IOrderedAsyncEnumerable<TSource> source, Func<TSource, CancellationToken, ValueTask<TKey>> keySelector) => ThenByDescendingAwaitWithCancellationCore<TSource, TKey>(source, keySelector);
        public static IOrderedAsyncEnumerable<TSource> ThenByDescending<TSource, TKey>(this IOrderedAsyncEnumerable<TSource> source, Func<TSource, CancellationToken, ValueTask<TKey>> keySelector, IComparer<TKey> comparer) => ThenByDescendingAwaitWithCancellationCore<TSource, TKey>(source, keySelector, comparer);
        public static ValueTask<Dictionary<TKey, TSource>> ToDictionaryAsync<TSource, TKey>(this IAsyncEnumerable<TSource> source, Func<TSource, CancellationToken, ValueTask<TKey>> keySelector, CancellationToken cancellationToken = default) where TKey : notnull => ToDictionaryAwaitWithCancellationAsyncCore<TSource, TKey>(source, keySelector, cancellationToken);
        public static ValueTask<Dictionary<TKey, TSource>> ToDictionaryAsync<TSource, TKey>(this IAsyncEnumerable<TSource> source, Func<TSource, CancellationToken, ValueTask<TKey>> keySelector, IEqualityComparer<TKey> comparer, CancellationToken cancellationToken = default) where TKey : notnull => ToDictionaryAwaitWithCancellationAsyncCore<TSource, TKey>(source, keySelector, comparer, cancellationToken);
        public static ValueTask<Dictionary<TKey, TElement>> ToDictionaryAsync<TSource, TKey, TElement>(this IAsyncEnumerable<TSource> source, Func<TSource, CancellationToken, ValueTask<TKey>> keySelector, Func<TSource, CancellationToken, ValueTask<TElement>> elementSelector, CancellationToken cancellationToken = default) where TKey : notnull => ToDictionaryAwaitWithCancellationAsyncCore<TSource, TKey, TElement>(source, keySelector, elementSelector, cancellationToken);
        public static ValueTask<Dictionary<TKey, TElement>> ToDictionaryAsync<TSource, TKey, TElement>(this IAsyncEnumerable<TSource> source, Func<TSource, CancellationToken, ValueTask<TKey>> keySelector, Func<TSource, CancellationToken, ValueTask<TElement>> elementSelector, IEqualityComparer<TKey> comparer, CancellationToken cancellationToken = default) where TKey : notnull => ToDictionaryAwaitWithCancellationAsyncCore<TSource, TKey, TElement>(source, keySelector, elementSelector, comparer, cancellationToken);
        public static ValueTask<ILookup<TKey, TSource>> ToLookupAsync<TSource, TKey>(this IAsyncEnumerable<TSource> source, Func<TSource, CancellationToken, ValueTask<TKey>> keySelector, CancellationToken cancellationToken = default) => ToLookupAwaitWithCancellationAsyncCore<TSource, TKey>(source, keySelector, cancellationToken);
        public static ValueTask<ILookup<TKey, TElement>> ToLookupAsync<TSource, TKey, TElement>(this IAsyncEnumerable<TSource> source, Func<TSource, CancellationToken, ValueTask<TKey>> keySelector, Func<TSource, CancellationToken, ValueTask<TElement>> elementSelector, CancellationToken cancellationToken = default) => ToLookupAwaitWithCancellationAsyncCore<TSource, TKey, TElement>(source, keySelector, elementSelector, cancellationToken);
        public static ValueTask<ILookup<TKey, TSource>> ToLookupAsync<TSource, TKey>(this IAsyncEnumerable<TSource> source, Func<TSource, CancellationToken, ValueTask<TKey>> keySelector, IEqualityComparer<TKey> comparer, CancellationToken cancellationToken = default) => ToLookupAwaitWithCancellationAsyncCore<TSource, TKey>(source, keySelector, comparer, cancellationToken);
        public static ValueTask<ILookup<TKey, TElement>> ToLookupAsync<TSource, TKey, TElement>(this IAsyncEnumerable<TSource> source, Func<TSource, CancellationToken, ValueTask<TKey>> keySelector, Func<TSource, CancellationToken, ValueTask<TElement>> elementSelector, IEqualityComparer<TKey> comparer, CancellationToken cancellationToken = default) => ToLookupAwaitWithCancellationAsyncCore<TSource, TKey, TElement>(source, keySelector, elementSelector, comparer, cancellationToken);
        public static IAsyncEnumerable<TSource> Where<TSource>(this IAsyncEnumerable<TSource> source, Func<TSource, CancellationToken, ValueTask<bool>> predicate) => WhereAwaitWithCancellationCore<TSource>(source, predicate);
        public static IAsyncEnumerable<TSource> Where<TSource>(this IAsyncEnumerable<TSource> source, Func<TSource, int, CancellationToken, ValueTask<bool>> predicate) => WhereAwaitWithCancellationCore<TSource>(source, predicate);
        public static IAsyncEnumerable<TResult> Zip<TFirst, TSecond, TResult>(this IAsyncEnumerable<TFirst> first, IAsyncEnumerable<TSecond> second, Func<TFirst, TSecond, CancellationToken, ValueTask<TResult>> selector) => ZipAwaitWithCancellationCore<TFirst, TSecond, TResult>(first, second, selector);
#endif
#else
        /// <summary>
        /// Invokes and awaits a transform function on each element of a sequence and returns the maximum <see cref="long"/> value.
        /// </summary>
        /// <typeparam name="TSource">Type of elements in the source sequence.</typeparam>
        /// <param name="source">The source sequence.</param>
        /// <param name="selector">An asynchronous transform function to invoke and await on each element of the source.</param>
        /// <param name="cancellationToken">The optional cancellation token to be usef for cancelling the sequence at any time.</param>
        /// <returns>A ValueTask containing the maximum value in the sequence.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="source"/> or <paramref name="selector"/> is <see langword="null"/>.</exception>
        public static ValueTask<long> MaxAwaitAsync<TSource>(this IAsyncEnumerable<TSource> source, Func<TSource, ValueTask<long>> selector, CancellationToken cancellationToken = default) => MaxAwaitAsyncCore<TSource>(source, selector, cancellationToken);

        /// <summary>
        /// Invokes and awaits a transform function on each element of a sequence and returns the maximum <see cref="Nullable{Long}"/> value.
        /// </summary>
        /// <typeparam name="TSource">Type of elements in the source sequence.</typeparam>
        /// <param name="source">The source sequence.</param>
        /// <param name="selector">An asynchronous transform function to invoke and await on each element of the source.</param>
        /// <param name="cancellationToken">The optional cancellation token to be usef for cancelling the sequence at any time.</param>
        /// <returns>A ValueTask containing the maximum value in the sequence.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="source"/> or <paramref name="selector"/> is <see langword="null"/>.</exception>
        public static ValueTask<long?> MaxAwaitAsync<TSource>(this IAsyncEnumerable<TSource> source, Func<TSource, ValueTask<long?>> selector, CancellationToken cancellationToken = default) => MaxAwaitAsyncCore<TSource>(source, selector, cancellationToken);

        /// <summary>
        /// Invokes and awaits a transform function on each element of a sequence and returns the maximum <see cref="float"/> value.
        /// </summary>
        /// <typeparam name="TSource">Type of elements in the source sequence.</typeparam>
        /// <param name="source">The source sequence.</param>
        /// <param name="selector">An asynchronous transform function to invoke and await on each element of the source.</param>
        /// <param name="cancellationToken">The optional cancellation token to be usef for cancelling the sequence at any time.</param>
        /// <returns>A ValueTask containing the maximum value in the sequence.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="source"/> or <paramref name="selector"/> is <see langword="null"/>.</exception>
        public static ValueTask<float> MaxAwaitAsync<TSource>(this IAsyncEnumerable<TSource> source, Func<TSource, ValueTask<float>> selector, CancellationToken cancellationToken = default) => MaxAwaitAsyncCore<TSource>(source, selector, cancellationToken);

        /// <summary>
        /// Invokes and awaits a transform function on each element of a sequence and returns the maximum <see cref="Nullable{Float}"/> value.
        /// </summary>
        /// <typeparam name="TSource">Type of elements in the source sequence.</typeparam>
        /// <param name="source">The source sequence.</param>
        /// <param name="selector">An asynchronous transform function to invoke and await on each element of the source.</param>
        /// <param name="cancellationToken">The optional cancellation token to be usef for cancelling the sequence at any time.</param>
        /// <returns>A ValueTask containing the maximum value in the sequence.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="source"/> or <paramref name="selector"/> is <see langword="null"/>.</exception>
        public static ValueTask<float?> MaxAwaitAsync<TSource>(this IAsyncEnumerable<TSource> source, Func<TSource, ValueTask<float?>> selector, CancellationToken cancellationToken = default) => MaxAwaitAsyncCore<TSource>(source, selector, cancellationToken);

        /// <summary>
        /// Invokes and awaits a transform function on each element of a sequence and returns the maximum <see cref="double"/> value.
        /// </summary>
        /// <typeparam name="TSource">Type of elements in the source sequence.</typeparam>
        /// <param name="source">The source sequence.</param>
        /// <param name="selector">An asynchronous transform function to invoke and await on each element of the source.</param>
        /// <param name="cancellationToken">The optional cancellation token to be usef for cancelling the sequence at any time.</param>
        /// <returns>A ValueTask containing the maximum value in the sequence.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="source"/> or <paramref name="selector"/> is <see langword="null"/>.</exception>
        public static ValueTask<double> MaxAwaitAsync<TSource>(this IAsyncEnumerable<TSource> source, Func<TSource, ValueTask<double>> selector, CancellationToken cancellationToken = default) => MaxAwaitAsyncCore<TSource>(source, selector, cancellationToken);

        /// <summary>
        /// Invokes and awaits a transform function on each element of a sequence and returns the maximum <see cref="Nullable{Double}"/> value.
        /// </summary>
        /// <typeparam name="TSource">Type of elements in the source sequence.</typeparam>
        /// <param name="source">The source sequence.</param>
        /// <param name="selector">An asynchronous transform function to invoke and await on each element of the source.</param>
        /// <param name="cancellationToken">The optional cancellation token to be usef for cancelling the sequence at any time.</param>
        /// <returns>A ValueTask containing the maximum value in the sequence.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="source"/> or <paramref name="selector"/> is <see langword="null"/>.</exception>
        public static ValueTask<double?> MaxAwaitAsync<TSource>(this IAsyncEnumerable<TSource> source, Func<TSource, ValueTask<double?>> selector, CancellationToken cancellationToken = default) => MaxAwaitAsyncCore<TSource>(source, selector, cancellationToken);

        /// <summary>
        /// Invokes and awaits a transform function on each element of a sequence and returns the maximum <see cref="decimal"/> value.
        /// </summary>
        /// <typeparam name="TSource">Type of elements in the source sequence.</typeparam>
        /// <param name="source">The source sequence.</param>
        /// <param name="selector">An asynchronous transform function to invoke and await on each element of the source.</param>
        /// <param name="cancellationToken">The optional cancellation token to be usef for cancelling the sequence at any time.</param>
        /// <returns>A ValueTask containing the maximum value in the sequence.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="source"/> or <paramref name="selector"/> is <see langword="null"/>.</exception>
        public static ValueTask<decimal> MaxAwaitAsync<TSource>(this IAsyncEnumerable<TSource> source, Func<TSource, ValueTask<decimal>> selector, CancellationToken cancellationToken = default) => MaxAwaitAsyncCore<TSource>(source, selector, cancellationToken);

        /// <summary>
        /// Invokes and awaits a transform function on each element of a sequence and returns the maximum <see cref="Nullable{Decimal}"/> value.
        /// </summary>
        /// <typeparam name="TSource">Type of elements in the source sequence.</typeparam>
        /// <param name="source">The source sequence.</param>
        /// <param name="selector">An asynchronous transform function to invoke and await on each element of the source.</param>
        /// <param name="cancellationToken">The optional cancellation token to be usef for cancelling the sequence at any time.</param>
        /// <returns>A ValueTask containing the maximum value in the sequence.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="source"/> or <paramref name="selector"/> is <see langword="null"/>.</exception>
        public static ValueTask<decimal?> MaxAwaitAsync<TSource>(this IAsyncEnumerable<TSource> source, Func<TSource, ValueTask<decimal?>> selector, CancellationToken cancellationToken = default) => MaxAwaitAsyncCore<TSource>(source, selector, cancellationToken);

        /// <summary>
        /// Invokes and awaits a transform function on each element of a sequence and returns the maximum value.
        /// </summary>
        /// <typeparam name="TSource">The type of the elements in the source sequence.</typeparam>
        /// <typeparam name="TResult">The type of the objects derived from the elements in the source sequence to determine the maximum of.</typeparam>
        /// <param name="source">An async-enumerable sequence to determine the minimum element of.</param>
        /// <param name="selector">An asynchronous transform function to invoke and await on each element.</param>
        /// <param name="cancellationToken">The optional cancellation token to be used for cancelling the sequence at any time.</param>
        /// <returns>A ValueTask containing a single element with the value that corresponds to the maximum element in the source sequence.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="source"/> or <paramref name="selector"/> is null.</exception>
        /// <remarks>The return type of this operator differs from the corresponding operator on IEnumerable in order to retain asynchronous behavior.</remarks>
        public static ValueTask<TResult> MaxAwaitAsync<TSource, TResult>(this IAsyncEnumerable<TSource> source, Func<TSource, ValueTask<TResult>> selector, CancellationToken cancellationToken = default) => MaxAwaitAsyncCore<TSource, TResult>(source, selector, cancellationToken);

        /// <summary>
        /// Returns the maximum <see cref="int"/> value in an async-enumerable sequence.
        /// </summary>
        /// <typeparam name="TSource">Type of elements in the source sequence.</typeparam>
        /// <param name="source">The source sequence.</param>
        /// <param name="selector">An asynchronous transform function to invoke and await on each element of the source.</param>
        /// <param name="cancellationToken">The optional cancellation token to be usef for cancelling the sequence at any time.</param>
        /// <returns>A ValueTask containing the maximum value in the sequence.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="source"/> or <paramref name="selector"/> is <see langword="null"/>.</exception>
        public static ValueTask<int> MaxAwaitAsync<TSource>(this IAsyncEnumerable<TSource> source, Func<TSource, ValueTask<int>> selector, CancellationToken cancellationToken = default) => MaxAwaitAsyncCore<TSource>(source, selector, cancellationToken);

        /// <summary>
        /// Returns the maximum <see cref="Nullable{Int}"/> value in an async-enumerable sequence.
        /// </summary>
        /// <typeparam name="TSource">Type of elements in the source sequence.</typeparam>
        /// <param name="source">The source sequence.</param>
        /// <param name="selector">An asynchronous transform function to invoke and await on each element of the source.</param>
        /// <param name="cancellationToken">The optional cancellation token to be usef for cancelling the sequence at any time.</param>
        /// <returns>A ValueTask containing the maximum value in the sequence.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="source"/> or <paramref name="selector"/> is <see langword="null"/>.</exception>
        public static ValueTask<int?> MaxAwaitAsync<TSource>(this IAsyncEnumerable<TSource> source, Func<TSource, ValueTask<int?>> selector, CancellationToken cancellationToken = default) => MaxAwaitAsyncCore<TSource>(source, selector, cancellationToken);

        /// <summary>
        /// Invokes and awaits a transform function on each element of a sequence and returns the minimum <see cref="double"/> value.
        /// </summary>
        /// <typeparam name="TSource">The type of the elements in the source sequence.</typeparam>
        /// <param name="source">An async-enumerable sequence to determine the minimum element of.</param>
        /// <param name="selector">An asynchronous transform function to invoke and await on each element.</param>
        /// <param name="cancellationToken">The optional cancellation token to be used for cancelling the sequence at any time.</param>
        /// <returns>A ValueTask sequence containing a single element with the value that corresponds to the minimum element in the source sequence.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="source"/> or <paramref name="selector"/> is null.</exception>
        /// <remarks>The return type of this operator differs from the corresponding operator on IEnumerable in order to retain asynchronous behavior.</remarks>
        public static ValueTask<double> MinAwaitAsync<TSource>(this IAsyncEnumerable<TSource> source, Func<TSource, ValueTask<double>> selector, CancellationToken cancellationToken = default) => MinAwaitAsyncCore<TSource>(source, selector, cancellationToken);

        /// <summary>
        /// Invokes and awaits a transform function on each element of a sequence and returns the minimum <see cref="Nullable{Double}"/> value.
        /// </summary>
        /// <typeparam name="TSource">The type of the elements in the source sequence.</typeparam>
        /// <param name="source">An async-enumerable sequence to determine the minimum element of.</param>
        /// <param name="selector">An asynchronous transform function to invoke and await on each element.</param>
        /// <param name="cancellationToken">The optional cancellation token to be used for cancelling the sequence at any time.</param>
        /// <returns>A ValueTask sequence containing a single element with the value that corresponds to the minimum element in the source sequence.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="source"/> or <paramref name="selector"/> is null.</exception>
        /// <remarks>The return type of this operator differs from the corresponding operator on IEnumerable in order to retain asynchronous behavior.</remarks>
        public static ValueTask<double?> MinAwaitAsync<TSource>(this IAsyncEnumerable<TSource> source, Func<TSource, ValueTask<double?>> selector, CancellationToken cancellationToken = default) => MinAwaitAsyncCore<TSource>(source, selector, cancellationToken);

        /// <summary>
        /// Invokes and awaits a transform function on each element of a sequence and returns the minimum <see cref="decimal"/> value.
        /// </summary>
        /// <typeparam name="TSource">The type of the elements in the source sequence.</typeparam>
        /// <param name="source">An async-enumerable sequence to determine the minimum element of.</param>
        /// <param name="selector">An asynchronous transform function to invoke and await on each element.</param>
        /// <param name="cancellationToken">The optional cancellation token to be used for cancelling the sequence at any time.</param>
        /// <returns>A ValueTask sequence containing a single element with the value that corresponds to the minimum element in the source sequence.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="source"/> or <paramref name="selector"/> is null.</exception>
        /// <remarks>The return type of this operator differs from the corresponding operator on IEnumerable in order to retain asynchronous behavior.</remarks>
        public static ValueTask<decimal> MinAwaitAsync<TSource>(this IAsyncEnumerable<TSource> source, Func<TSource, ValueTask<decimal>> selector, CancellationToken cancellationToken = default) => MinAwaitAsyncCore<TSource>(source, selector, cancellationToken);

        /// <summary>
        /// Invokes and awaits a transform function on each element of a sequence and returns the minimum <see cref="Nullable{Decimal}"/> value.
        /// </summary>
        /// <typeparam name="TSource">The type of the elements in the source sequence.</typeparam>
        /// <param name="source">An async-enumerable sequence to determine the minimum element of.</param>
        /// <param name="selector">An asynchronous transform function to invoke and await on each element.</param>
        /// <param name="cancellationToken">The optional cancellation token to be used for cancelling the sequence at any time.</param>
        /// <returns>A ValueTask sequence containing a single element with the value that corresponds to the minimum element in the source sequence.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="source"/> or <paramref name="selector"/> is null.</exception>
        /// <remarks>The return type of this operator differs from the corresponding operator on IEnumerable in order to retain asynchronous behavior.</remarks>
        public static ValueTask<decimal?> MinAwaitAsync<TSource>(this IAsyncEnumerable<TSource> source, Func<TSource, ValueTask<decimal?>> selector, CancellationToken cancellationToken = default) => MinAwaitAsyncCore<TSource>(source, selector, cancellationToken);

        /// <summary>
        /// Invokes and awaits a transform function on each element of a sequence and returns the minimum <see cref="int"/> value.
        /// </summary>
        /// <typeparam name="TSource">The type of the elements in the source sequence.</typeparam>
        /// <param name="source">An async-enumerable sequence to determine the minimum element of.</param>
        /// <param name="selector">An asynchronous transform function to invoke and await on each element.</param>
        /// <param name="cancellationToken">The optional cancellation token to be used for cancelling the sequence at any time.</param>
        /// <returns>A ValueTask sequence containing a single element with the value that corresponds to the minimum element in the source sequence.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="source"/> or <paramref name="selector"/> is null.</exception>
        /// <remarks>The return type of this operator differs from the corresponding operator on IEnumerable in order to retain asynchronous behavior.</remarks>
        public static ValueTask<int> MinAwaitAsync<TSource>(this IAsyncEnumerable<TSource> source, Func<TSource, ValueTask<int>> selector, CancellationToken cancellationToken = default) => MinAwaitAsyncCore<TSource>(source, selector, cancellationToken);

        /// <summary>
        /// Invokes and awaits a transform function on each element of a sequence and returns the minimum <see cref="Nullable{Int}"/> value.
        /// </summary>
        /// <typeparam name="TSource">The type of the elements in the source sequence.</typeparam>
        /// <param name="source">An async-enumerable sequence to determine the minimum element of.</param>
        /// <param name="selector">An asynchronous transform function to invoke and await on each element.</param>
        /// <param name="cancellationToken">The optional cancellation token to be used for cancelling the sequence at any time.</param>
        /// <returns>A ValueTask sequence containing a single element with the value that corresponds to the minimum element in the source sequence.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="source"/> or <paramref name="selector"/> is null.</exception>
        /// <remarks>The return type of this operator differs from the corresponding operator on IEnumerable in order to retain asynchronous behavior.</remarks>
        public static ValueTask<int?> MinAwaitAsync<TSource>(this IAsyncEnumerable<TSource> source, Func<TSource, ValueTask<int?>> selector, CancellationToken cancellationToken = default) => MinAwaitAsyncCore<TSource>(source, selector, cancellationToken);

        /// <summary>
        /// Invokes and awaits a transform function on each element of a sequence and returns the minimum <see cref="long"/> value.
        /// </summary>
        /// <typeparam name="TSource">The type of the elements in the source sequence.</typeparam>
        /// <param name="source">An async-enumerable sequence to determine the minimum element of.</param>
        /// <param name="selector">An asynchronous transform function to invoke and await on each element.</param>
        /// <param name="cancellationToken">The optional cancellation token to be used for cancelling the sequence at any time.</param>
        /// <returns>A ValueTask sequence containing a single element with the value that corresponds to the minimum element in the source sequence.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="source"/> or <paramref name="selector"/> is null.</exception>
        /// <remarks>The return type of this operator differs from the corresponding operator on IEnumerable in order to retain asynchronous behavior.</remarks>
        public static ValueTask<long> MinAwaitAsync<TSource>(this IAsyncEnumerable<TSource> source, Func<TSource, ValueTask<long>> selector, CancellationToken cancellationToken = default) => MinAwaitAsyncCore<TSource>(source, selector, cancellationToken);

        /// <summary>
        /// Invokes and awaits a transform function on each element of a sequence and returns the minimum <see cref="Nullable{Long}"/> value.
        /// </summary>
        /// <typeparam name="TSource">The type of the elements in the source sequence.</typeparam>
        /// <param name="source">An async-enumerable sequence to determine the minimum element of.</param>
        /// <param name="selector">An asynchronous transform function to invoke and await on each element.</param>
        /// <param name="cancellationToken">The optional cancellation token to be used for cancelling the sequence at any time.</param>
        /// <returns>A ValueTask sequence containing a single element with the value that corresponds to the minimum element in the source sequence.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="source"/> or <paramref name="selector"/> is null.</exception>
        /// <remarks>The return type of this operator differs from the corresponding operator on IEnumerable in order to retain asynchronous behavior.</remarks>
        public static ValueTask<long?> MinAwaitAsync<TSource>(this IAsyncEnumerable<TSource> source, Func<TSource, ValueTask<long?>> selector, CancellationToken cancellationToken = default) => MinAwaitAsyncCore<TSource>(source, selector, cancellationToken);

        /// <summary>
        /// Invokes and awaits a transform function on each element of a sequence and returns the minimum <see cref="float"/> value.
        /// </summary>
        /// <typeparam name="TSource">The type of the elements in the source sequence.</typeparam>
        /// <param name="source">An async-enumerable sequence to determine the minimum element of.</param>
        /// <param name="selector">An asynchronous transform function to invoke and await on each element.</param>
        /// <param name="cancellationToken">The optional cancellation token to be used for cancelling the sequence at any time.</param>
        /// <returns>A ValueTask sequence containing a single element with the value that corresponds to the minimum element in the source sequence.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="source"/> or <paramref name="selector"/> is null.</exception>
        /// <remarks>The return type of this operator differs from the corresponding operator on IEnumerable in order to retain asynchronous behavior.</remarks>
        public static ValueTask<float> MinAwaitAsync<TSource>(this IAsyncEnumerable<TSource> source, Func<TSource, ValueTask<float>> selector, CancellationToken cancellationToken = default) => MinAwaitAsyncCore<TSource>(source, selector, cancellationToken);

        /// <summary>
        /// Invokes and awaits a transform function on each element of a sequence and returns the minimum <see cref="Nullable{Float}"/> value.
        /// </summary>
        /// <typeparam name="TSource">The type of the elements in the source sequence.</typeparam>
        /// <param name="source">An async-enumerable sequence to determine the minimum element of.</param>
        /// <param name="selector">An asynchronous transform function to invoke and await on each element.</param>
        /// <param name="cancellationToken">The optional cancellation token to be used for cancelling the sequence at any time.</param>
        /// <returns>A ValueTask sequence containing a single element with the value that corresponds to the minimum element in the source sequence.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="source"/> or <paramref name="selector"/> is null.</exception>
        /// <remarks>The return type of this operator differs from the corresponding operator on IEnumerable in order to retain asynchronous behavior.</remarks>
        public static ValueTask<float?> MinAwaitAsync<TSource>(this IAsyncEnumerable<TSource> source, Func<TSource, ValueTask<float?>> selector, CancellationToken cancellationToken = default) => MinAwaitAsyncCore<TSource>(source, selector, cancellationToken);

        /// <summary>
        /// Invokes and awaits a transform function on each element of a sequence and returns the minimum value.
        /// </summary>
        /// <typeparam name="TSource">The type of the elements in the source sequence.</typeparam>
        /// <typeparam name="TResult">The type of the objects derived from the elements in the source sequence to determine the minimum of.</typeparam>
        /// <param name="source">An async-enumerable sequence to determine the minimum element of.</param>
        /// <param name="selector">An asynchronous transform function to invoke and await on each element.</param>
        /// <param name="cancellationToken">The optional cancellation token to be used for cancelling the sequence at any time.</param>
        /// <returns>A ValueTask sequence containing a single element with the value that corresponds to the minimum element in the source sequence.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="source"/> or <paramref name="selector"/> is null.</exception>
        /// <remarks>The return type of this operator differs from the corresponding operator on IEnumerable in order to retain asynchronous behavior.</remarks>
        public static ValueTask<TResult> MinAwaitAsync<TSource, TResult>(this IAsyncEnumerable<TSource> source, Func<TSource, ValueTask<TResult>> selector, CancellationToken cancellationToken = default) => MinAwaitAsyncCore<TSource, TResult>(source, selector, cancellationToken);

        /// <summary>
        /// Sorts the elements of a sequence in ascending order according to a key obtained by invoking a transform function on each element and awaiting the result.
        /// </summary>
        /// <typeparam name="TSource">The type of the elements of source.</typeparam>
        /// <typeparam name="TKey">The type of the key returned by keySelector.</typeparam>
        /// <param name="source">An async-enumerable sequence of values to order.</param>
        /// <param name="keySelector">An asynchronous function to extract a key from an element.</param>
        /// <returns>An ordered async-enumerable sequence whose elements are sorted according to a key.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="source"/> or <paramref name="keySelector"/> is null.</exception>
        public static IOrderedAsyncEnumerable<TSource> OrderByAwait<TSource, TKey>(this IAsyncEnumerable<TSource> source, Func<TSource, ValueTask<TKey>> keySelector) => OrderByAwaitCore<TSource, TKey>(source, keySelector);

        /// <summary>
        /// Sorts the elements of a sequence in ascending order by using a specified comparer. The keys are obtained by invoking the transform function on each element and awaiting the result.
        /// </summary>
        /// <typeparam name="TSource">The type of the elements of source.</typeparam>
        /// <typeparam name="TKey">The type of the key returned by keySelector.</typeparam>
        /// <param name="source">An async-enumerable sequence of values to order.</param>
        /// <param name="keySelector">An asynchronous function to extract a key from an element.</param>
        /// <param name="comparer">A comparer to compare keys.</param>
        /// <returns>An ordered async-enumerable sequence whose elements are sorted according to a key.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="source"/> or <paramref name="keySelector"/> is null.</exception>
        public static IOrderedAsyncEnumerable<TSource> OrderByAwait<TSource, TKey>(this IAsyncEnumerable<TSource> source, Func<TSource, ValueTask<TKey>> keySelector, IComparer<TKey> comparer) => OrderByAwaitCore<TSource, TKey>(source, keySelector, comparer);

        /// <summary>
        /// Sorts the elements of a sequence in descending order according to a key obtained by invoking a transform function on each element and awaiting the result.
        /// </summary>
        /// <typeparam name="TSource">The type of the elements of source.</typeparam>
        /// <typeparam name="TKey">The type of the key returned by keySelector.</typeparam>
        /// <param name="source">An async-enumerable sequence of values to order.</param>
        /// <param name="keySelector">An asynchronous function to extract a key from an element.</param>
        /// <returns>An ordered async-enumerable sequence whose elements are sorted in descending order according to a key.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="source"/> or <paramref name="keySelector"/> is null.</exception>
        public static IOrderedAsyncEnumerable<TSource> OrderByDescendingAwait<TSource, TKey>(this IAsyncEnumerable<TSource> source, Func<TSource, ValueTask<TKey>> keySelector) => OrderByDescendingAwaitCore<TSource, TKey>(source, keySelector);

        /// <summary>
        /// Sorts the elements of a sequence in descending order by using a specified comparer. The keys are obtained by invoking the transform function on each element and awaiting the result.
        /// </summary>
        /// <typeparam name="TSource">The type of the elements of source.</typeparam>
        /// <typeparam name="TKey">The type of the key returned by keySelector.</typeparam>
        /// <param name="source">An async-enumerable sequence of values to order.</param>
        /// <param name="keySelector">An asynchronous function to extract a key from an element.</param>
        /// <param name="comparer">A comparer to compare keys.</param>
        /// <returns>An ordered async-enumerable sequence whose elements are sorted in descending order according to a key.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="source"/> or <paramref name="keySelector"/> is null.</exception>
        public static IOrderedAsyncEnumerable<TSource> OrderByDescendingAwait<TSource, TKey>(this IAsyncEnumerable<TSource> source, Func<TSource, ValueTask<TKey>> keySelector, IComparer<TKey> comparer) => OrderByDescendingAwaitCore<TSource, TKey>(source, keySelector, comparer);

        /// <summary>
        /// Projects each element of an async-enumerable sequence into a new form by applying an asynchronous selector function to each member of the source sequence and awaiting the result.
        /// </summary>
        /// <typeparam name="TSource">The type of the elements in the source sequence.</typeparam>
        /// <typeparam name="TResult">The type of the elements in the result sequence, obtained by running the selector function for each element in the source sequence and awaiting the result.</typeparam>
        /// <param name="source">A sequence of elements to invoke a transform function on.</param>
        /// <param name="selector">An asynchronous transform function to apply to each source element.</param>
        /// <returns>An async-enumerable sequence whose elements are the result of invoking the transform function on each element of the source sequence and awaiting the result.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="source"/> or <paramref name="selector"/> is null.</exception>
        public static IAsyncEnumerable<TResult> SelectAwait<TSource, TResult>(this IAsyncEnumerable<TSource> source, Func<TSource, ValueTask<TResult>> selector) => SelectAwaitCore<TSource, TResult>(source, selector);

        /// <summary>
        /// Projects each element of an async-enumerable sequence into a new form by applying an asynchronous selector function that incorporates each element's index to each element of the source sequence and awaiting the result.
        /// </summary>
        /// <typeparam name="TSource">The type of elements in the source sequence.</typeparam>
        /// <typeparam name="TResult">The type of elements in the result sequence, obtained by running the selector function for each element and its index, and awaiting the result.</typeparam>
        /// <param name="source">A sequence of elements to invoke a transform function on.</param>
        /// <param name="selector">An asynchronous transform function to apply to each source element; the second parameter represents the index of the element.</param>
        /// <returns>An async-enumerable sequence whose elements are the result of invoking the transform function on each element and its index of the source sequence and awaiting the result.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="source"/> or <paramref name="selector"/> is null.</exception>
        public static IAsyncEnumerable<TResult> SelectAwait<TSource, TResult>(this IAsyncEnumerable<TSource> source, Func<TSource, int, ValueTask<TResult>> selector) => SelectAwaitCore<TSource, TResult>(source, selector);

        /// <summary>
        /// Projects each element of an async-enumerable sequence into an async-enumerable sequence and merges the resulting async-enumerable sequences into one async-enumerable sequence.
        /// </summary>
        /// <typeparam name="TSource">The type of elements in the source sequence.</typeparam>
        /// <typeparam name="TResult">The type of elements in the projected inner sequences and the merged result sequence.</typeparam>
        /// <param name="source">An async-enumerable sequence of elements to project.</param>
        /// <param name="selector">An asynchronous selector function to apply to each element of the source sequence.</param>
        /// <returns>An async-enumerable sequence whose elements are the result of invoking the one-to-many transform function on each element of the source sequence and awaiting the result.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="source"/> or <paramref name="selector"/> is null.</exception>
        public static IAsyncEnumerable<TResult> SelectManyAwait<TSource, TResult>(this IAsyncEnumerable<TSource> source, Func<TSource, ValueTask<IAsyncEnumerable<TResult>>> selector) => SelectManyAwaitCore<TSource, TResult>(source, selector);

        /// <summary>
        /// Projects each element of an async-enumerable sequence into an async-enumerable sequence by incorporating the element's index and merges the resulting async-enumerable sequences into an async-enumerable sequence.
        /// </summary>
        /// <typeparam name="TSource">The type of elements in the source sequence.</typeparam>
        /// <typeparam name="TResult">The type of elements in the projected inner sequences and the merged result sequence.</typeparam>
        /// <param name="source">An async-enumerable sequence of elements to project.</param>
        /// <param name="selector">An asynchronous selector function to apply to each element; the second parameter represents the index of the element.</param>
        /// <returns>An async-enumerable sequence who's elements are the result of invoking the one-to-many transform function on each element of the source sequence and awaiting the result.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="source"/> or <paramref name="selector"/> is null.</exception>
        public static IAsyncEnumerable<TResult> SelectManyAwait<TSource, TResult>(this IAsyncEnumerable<TSource> source, Func<TSource, int, ValueTask<IAsyncEnumerable<TResult>>> selector) => SelectManyAwaitCore<TSource, TResult>(source, selector);

        /// <summary>
        /// Projects each element of an async-enumerable sequence to an async-enumerable sequence by awaiting the result of a transform function, invokes the result selector for each of the source elements and each of the corrasponding inner-sequence's elements and awaits the result, and merges the results into one async-enumerable sequence.
        /// </summary>
        /// <typeparam name="TSource">The type of elements in the source sequence.</typeparam>
        /// <typeparam name="TCollection">The type of elements in the projected intermediate sequences.</typeparam>
        /// <typeparam name="TResult">The type of elements in the result sequence.</typeparam>
        /// <param name="source">An async-enumerable sequence of elements to project.</param>
        /// <param name="collectionSelector">An asynchronous transform function to apply to each source element.</param>
        /// <param name="resultSelector">An asynchronous transform function to apply to each element of the intermediate sequence.</param>
        /// <returns>An async-enumerable sequence whose elements are the result of invoking the one-to-many transform function <paramref name="collectionSelector"/> on each element of the input sequence, awaiting the result, applying <paramref name="resultSelector"/> to each element of the intermediate sequences along with their corrasponding source element and awaiting the result.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="source"/>, <paramref name="collectionSelector"/>, or <paramref name="resultSelector"/> is <see langword="null"/>.</exception>
        public static IAsyncEnumerable<TResult> SelectManyAwait<TSource, TCollection, TResult>(this IAsyncEnumerable<TSource> source, Func<TSource, ValueTask<IAsyncEnumerable<TCollection>>> collectionSelector, Func<TSource, TCollection, ValueTask<TResult>> resultSelector) => SelectManyAwaitCore<TSource, TCollection, TResult>(source, collectionSelector, resultSelector);

        /// <summary>
        /// Projects each element of an async-enumerable sequence to an async-enumerable sequence by awaiting the result of a transform function that incorporates each element's index,
        /// invokes the result selector for the source element and each of the corrasponding inner-sequence's elements and awaits the result, and merges the results into one async-enumerable sequence.
        /// </summary>
        /// <typeparam name="TSource">The type of elements in the source sequence.</typeparam>
        /// <typeparam name="TCollection">The type of elements in the projected intermediate sequences.</typeparam>
        /// <typeparam name="TResult">The type of elements in the result sequence.</typeparam>
        /// <param name="source">An async-enumerable sequence of elements to project.</param>
        /// <param name="collectionSelector">An asynchronous transform function to apply to each source element; the second parameter represents the index of the element.</param>
        /// <param name="resultSelector">An asynchronous transform function to apply to each element of the intermediate sequence.</param>
        /// <returns>An async-enumerable sequence whose elements are the result of invoking the one-to-many transform function <paramref name="collectionSelector"/> on each element of the input sequence, awaiting the result, applying <paramref name="resultSelector"/> to each element of the intermediate sequences olong with their corrasponding source element and awaiting the result.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="source"/>, <paramref name="collectionSelector"/>, or <paramref name="resultSelector"/> is <see langword="null"/>.</exception>
        public static IAsyncEnumerable<TResult> SelectManyAwait<TSource, TCollection, TResult>(this IAsyncEnumerable<TSource> source, Func<TSource, int, ValueTask<IAsyncEnumerable<TCollection>>> collectionSelector, Func<TSource, TCollection, ValueTask<TResult>> resultSelector) => SelectManyAwaitCore<TSource, TCollection, TResult>(source, collectionSelector, resultSelector);

        /// <summary>
        /// Returns the only element of an async-enumerable sequence that satisfies the condition in the asynchronous predicate, and reports an exception if there is not exactly one element in the async-enumerable sequence that matches the predicate.
        /// </summary>
        /// <typeparam name="TSource">The type of elements in the source sequence.</typeparam>
        /// <param name="source">Source async-enumerable sequence.</param>
        /// <param name="predicate">An asynchronous predicate that will be applied to each element of the source sequence.</param>
        /// <param name="cancellationToken">The optional cancellation token to be used for cancelling the sequence at any time.</param>
        /// <returns>ValueTask containing the only element in the async-enumerable sequence that satisfies the condition in the asynchronous predicate.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="source"/> or <paramref name="predicate"/> is null.</exception>
        /// <exception cref="InvalidOperationException">(Asynchronous) No element satisfies the condition in the predicate. -or- More than one element satisfies the condition in the predicate. -or- The source sequence is empty.</exception>
        public static ValueTask<TSource> SingleAwaitAsync<TSource>(this IAsyncEnumerable<TSource> source, Func<TSource, ValueTask<bool>> predicate, CancellationToken cancellationToken = default) => SingleAwaitAsyncCore<TSource>(source, predicate, cancellationToken);

        /// <summary>
        /// Returns the only element of an async-enumerable sequence that satisfies the condition in the asynchronous predicate, or a default value if no such element exists, and reports an exception if there is more than one element in the async-enumerable sequence that matches the predicate.
        /// </summary>
        /// <typeparam name="TSource">The type of elements in the source sequence.</typeparam>
        /// <param name="source">Source async-enumerable sequence.</param>
        /// <param name="predicate">An asynchronous predicate that will be applied to each element of the source sequence. </param>
        /// <param name="cancellationToken">The optional cancellation token to be used for cancelling the sequence at any time.</param>
        /// <returns>ValueTask containing the only element in the async-enumerable sequence that satisfies the condition in the asynchronous predicate, or a default value if no such element exists.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="source"/> or <paramref name="predicate"/> is null.</exception>
        /// <exception cref="InvalidOperationException">(Asynchronous) More than one element satisfies the condition in the predicate.</exception>
        public static ValueTask<TSource?> SingleOrDefaultAwaitAsync<TSource>(this IAsyncEnumerable<TSource> source, Func<TSource, ValueTask<bool>> predicate, CancellationToken cancellationToken = default) => SingleOrDefaultAwaitAsyncCore<TSource>(source, predicate, cancellationToken);

        /// <summary>
        /// Bypasses elements in an async-enumerable sequence as long as a condition is true, and then returns the remaining elements.
        /// </summary>
        /// <typeparam name="TSource">The type of elements in the source sequence.</typeparam>
        /// <param name="source">An async-enumerable sequence to return elements from.</param>
        /// <param name="predicate">An asynchronous function to test each element for a condition.</param>
        /// <returns>An async-enumerable sequence containing the elements in the source sequence starting at the first element that does not pass the test specified by the predicate.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="source"/> or <paramref name="predicate"/> is <see langword="null"/>.</exception>
        public static IAsyncEnumerable<TSource> SkipWhileAwait<TSource>(this IAsyncEnumerable<TSource> source, Func<TSource, ValueTask<bool>> predicate) => SkipWhileAwaitCore<TSource>(source, predicate);

        /// <summary>
        /// Bypasses elements in an async-enumerable sequence as long as a condition is true, and then returns the remaining elements.
        /// The index of the element is used by the predicate.
        /// </summary>
        /// <typeparam name="TSource">The type of elements in the source sequence.</typeparam>
        /// <param name="source">An async-enumerable sequence to return elements from.</param>
        /// <param name="predicate">An asynchronous function to test each element for a condition; the second parameter of the function represents the index of the element.</param>
        /// <returns>An async-enumerable sequence containing the elements in the source sequence starting at the first element that does not pass the test specified by the predicate.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="source"/> or <paramref name="predicate"/> is <see langword="null"/>.</exception>
        public static IAsyncEnumerable<TSource> SkipWhileAwait<TSource>(this IAsyncEnumerable<TSource> source, Func<TSource, int, ValueTask<bool>> predicate) => SkipWhileAwaitCore<TSource>(source, predicate);

        /// <summary>
        /// Computes the sum of a sequence of <see cref="Nullable{Long}"/> values that are obtained by invoking a transform function on each element of the source sequence and awaiting the result.
        /// </summary>
        /// <typeparam name="TSource">The type of elements in the source sequence.</typeparam>
        /// <param name="source">A sequence of values that are used to calculate a sum.</param>
        /// <param name="selector">An asynchronous transform function to apply to each element.</param>
        /// <param name="cancellationToken">The optional cancellation token to be used for cancelling the sequence at any time.</param>
        /// <returns>A ValueTask containing the sum of the values in the source sequence.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="source"/> or <paramref name="selector"/> is <see langword="null"/>.</exception>
        /// <remarks>The return type of this operator differs from the corresponding operator on IEnumerable in order to retain asynchronous behavior.</remarks>
        public static ValueTask<long?> SumAwaitAsync<TSource>(this IAsyncEnumerable<TSource> source, Func<TSource, ValueTask<long?>> selector, CancellationToken cancellationToken = default) => SumAwaitAsyncCore<TSource>(source, selector, cancellationToken);

        /// <summary>
        /// Computes the sum of a sequence of <see cref="Nullable{Float}"/> values that are obtained by invoking a transform function on each element of the source sequence and awaiting the result.
        /// </summary>
        /// <typeparam name="TSource">The type of elements in the source sequence.</typeparam>
        /// <param name="source">A sequence of values that are used to calculate a sum.</param>
        /// <param name="selector">An asynchronous transform function to apply to each element.</param>
        /// <param name="cancellationToken">The optional cancellation token to be used for cancelling the sequence at any time.</param>
        /// <returns>A ValueTask containing the sum of the values in the source sequence.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="source"/> or <paramref name="selector"/> is <see langword="null"/>.</exception>
        /// <remarks>The return type of this operator differs from the corresponding operator on IEnumerable in order to retain asynchronous behavior.</remarks>
        public static ValueTask<float?> SumAwaitAsync<TSource>(this IAsyncEnumerable<TSource> source, Func<TSource, ValueTask<float?>> selector, CancellationToken cancellationToken = default) => SumAwaitAsyncCore<TSource>(source, selector, cancellationToken);

        /// <summary>
        /// Computes the sum of a sequence of <see cref="Nullable{Double}"/> values that are obtained by invoking a transform function on each element of the source sequence and awaiting the result.
        /// </summary>
        /// <typeparam name="TSource">The type of elements in the source sequence.</typeparam>
        /// <param name="source">A sequence of values that are used to calculate a sum.</param>
        /// <param name="selector">An asynchronous transform function to apply to each element.</param>
        /// <param name="cancellationToken">The optional cancellation token to be used for cancelling the sequence at any time.</param>
        /// <returns>A ValueTask containing the sum of the values in the source sequence.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="source"/> or <paramref name="selector"/> is <see langword="null"/>.</exception>
        /// <remarks>The return type of this operator differs from the corresponding operator on IEnumerable in order to retain asynchronous behavior.</remarks>
        public static ValueTask<double?> SumAwaitAsync<TSource>(this IAsyncEnumerable<TSource> source, Func<TSource, ValueTask<double?>> selector, CancellationToken cancellationToken = default) => SumAwaitAsyncCore<TSource>(source, selector, cancellationToken);

        /// <summary>
        /// Computes the sum of a sequence of <see cref="Nullable{Decimal}"/> values that are obtained by invoking a transform function on each element of the source sequence and awaiting the result.
        /// </summary>
        /// <typeparam name="TSource">The type of elements in the source sequence.</typeparam>
        /// <param name="source">A sequence of values that are used to calculate a sum.</param>
        /// <param name="selector">An asynchronous transform function to apply to each element.</param>
        /// <param name="cancellationToken">The optional cancellation token to be used for cancelling the sequence at any time.</param>
        /// <returns>A ValueTask containing the sum of the values in the source sequence.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="source"/> or <paramref name="selector"/> is <see langword="null"/>.</exception>
        /// <remarks>The return type of this operator differs from the corresponding operator on IEnumerable in order to retain asynchronous behavior.</remarks>
        public static ValueTask<decimal?> SumAwaitAsync<TSource>(this IAsyncEnumerable<TSource> source, Func<TSource, ValueTask<decimal?>> selector, CancellationToken cancellationToken = default) => SumAwaitAsyncCore<TSource>(source, selector, cancellationToken);

        /// <summary>
        /// Computes the sum of a sequence of <see cref="int"/> values that are obtained by invoking a transform function on each element of the source sequence and awaiting the result.
        /// </summary>
        /// <typeparam name="TSource">The type of elements in the source sequence.</typeparam>
        /// <param name="source">A sequence of values that are used to calculate a sum.</param>
        /// <param name="selector">An asynchronous transform function to apply to each element.</param>
        /// <param name="cancellationToken">The optional cancellation token to be used for cancelling the sequence at any time.</param>
        /// <returns>A ValueTask containing the sum of the values in the source sequence.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="source"/> or <paramref name="selector"/> is <see langword="null"/>.</exception>
        /// <remarks>The return type of this operator differs from the corresponding operator on IEnumerable in order to retain asynchronous behavior.</remarks>
        public static ValueTask<int> SumAwaitAsync<TSource>(this IAsyncEnumerable<TSource> source, Func<TSource, ValueTask<int>> selector, CancellationToken cancellationToken = default) => SumAwaitAsyncCore<TSource>(source, selector, cancellationToken);

        /// <summary>
        /// Computes the sum of a sequence of <see cref="long"/> values that are obtained by invoking a transform function on each element of the source sequence and awaiting the result.
        /// </summary>
        /// <typeparam name="TSource">The type of elements in the source sequence.</typeparam>
        /// <param name="source">A sequence of values that are used to calculate a sum.</param>
        /// <param name="selector">An asynchronous transform function to apply to each element.</param>
        /// <param name="cancellationToken">The optional cancellation token to be used for cancelling the sequence at any time.</param>
        /// <returns>A ValueTask containing the sum of the values in the source sequence.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="source"/> or <paramref name="selector"/> is <see langword="null"/>.</exception>
        /// <remarks>The return type of this operator differs from the corresponding operator on IEnumerable in order to retain asynchronous behavior.</remarks>
        public static ValueTask<long> SumAwaitAsync<TSource>(this IAsyncEnumerable<TSource> source, Func<TSource, ValueTask<long>> selector, CancellationToken cancellationToken = default) => SumAwaitAsyncCore<TSource>(source, selector, cancellationToken);

        /// <summary>
        /// Computes the sum of a sequence of <see cref="float"/> values that are obtained by invoking a transform function on each element of the source sequence and awaiting the result.
        /// </summary>
        /// <typeparam name="TSource">The type of elements in the source sequence.</typeparam>
        /// <param name="source">A sequence of values that are used to calculate a sum.</param>
        /// <param name="selector">An asynchronous transform function to apply to each element.</param>
        /// <param name="cancellationToken">The optional cancellation token to be used for cancelling the sequence at any time.</param>
        /// <returns>A ValueTask containing the sum of the values in the source sequence.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="source"/> or <paramref name="selector"/> is <see langword="null"/>.</exception>
        /// <remarks>The return type of this operator differs from the corresponding operator on IEnumerable in order to retain asynchronous behavior.</remarks>
        public static ValueTask<float> SumAwaitAsync<TSource>(this IAsyncEnumerable<TSource> source, Func<TSource, ValueTask<float>> selector, CancellationToken cancellationToken = default) => SumAwaitAsyncCore<TSource>(source, selector, cancellationToken);

        /// <summary>
        /// Computes the sum of a sequence of <see cref="double"/> values that are obtained by invoking a transform function on each element of the source sequence and awaiting the result.
        /// </summary>
        /// <typeparam name="TSource">The type of elements in the source sequence.</typeparam>
        /// <param name="source">A sequence of values that are used to calculate a sum.</param>
        /// <param name="selector">An asynchronous transform function to apply to each element.</param>
        /// <param name="cancellationToken">The optional cancellation token to be used for cancelling the sequence at any time.</param>
        /// <returns>A ValueTask containing the sum of the values in the source sequence.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="source"/> or <paramref name="selector"/> is <see langword="null"/>.</exception>
        /// <remarks>The return type of this operator differs from the corresponding operator on IEnumerable in order to retain asynchronous behavior.</remarks>
        public static ValueTask<double> SumAwaitAsync<TSource>(this IAsyncEnumerable<TSource> source, Func<TSource, ValueTask<double>> selector, CancellationToken cancellationToken = default) => SumAwaitAsyncCore<TSource>(source, selector, cancellationToken);

        /// <summary>
        /// Computes the sum of a sequence of <see cref="decimal"/> values that are obtained by invoking a transform function on each element of the source sequence and awaiting the result.
        /// </summary>
        /// <typeparam name="TSource">The type of elements in the source sequence.</typeparam>
        /// <param name="source">A sequence of values that are used to calculate a sum.</param>
        /// <param name="selector">An asynchronous transform function to apply to each element.</param>
        /// <param name="cancellationToken">The optional cancellation token to be used for cancelling the sequence at any time.</param>
        /// <returns>A ValueTask containing the sum of the values in the source sequence.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="source"/> or <paramref name="selector"/> is <see langword="null"/>.</exception>
        /// <remarks>The return type of this operator differs from the corresponding operator on IEnumerable in order to retain asynchronous behavior.</remarks>
        public static ValueTask<decimal> SumAwaitAsync<TSource>(this IAsyncEnumerable<TSource> source, Func<TSource, ValueTask<decimal>> selector, CancellationToken cancellationToken = default) => SumAwaitAsyncCore<TSource>(source, selector, cancellationToken);

        /// <summary>
        /// Computes the sum of a sequence of <see cref="Nullable{Int}"/> values that are obtained by invoking a transform function on each element of the source sequence and awaiting the result.
        /// </summary>
        /// <typeparam name="TSource">The type of elements in the source sequence.</typeparam>
        /// <param name="source">A sequence of values that are used to calculate a sum.</param>
        /// <param name="selector">An asynchronous transform function to apply to each element.</param>
        /// <param name="cancellationToken">The optional cancellation token to be used for cancelling the sequence at any time.</param>
        /// <returns>A ValueTask containing the sum of the values in the source sequence.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="source"/> or <paramref name="selector"/> is <see langword="null"/>.</exception>
        /// <remarks>The return type of this operator differs from the corresponding operator on IEnumerable in order to retain asynchronous behavior.</remarks>
        public static ValueTask<int?> SumAwaitAsync<TSource>(this IAsyncEnumerable<TSource> source, Func<TSource, ValueTask<int?>> selector, CancellationToken cancellationToken = default) => SumAwaitAsyncCore<TSource>(source, selector, cancellationToken);

        /// <summary>
        /// Returns elements from an async-enumerable sequence as long as a specified condition is true.
        /// </summary>
        /// <typeparam name="TSource">The type of the elements in the source sequence.</typeparam>
        /// <param name="source">A sequence to return elements from.</param>
        /// <param name="predicate">An asynchronous predicate to test each element for a condition.</param>
        /// <returns>An async-enumerable sequence that contains the elements from the input sequence that occur before the element at which the test no longer passes.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="source"/> or <paramref name="predicate"/> is null.</exception>
        public static IAsyncEnumerable<TSource> TakeWhileAwait<TSource>(this IAsyncEnumerable<TSource> source, Func<TSource, ValueTask<bool>> predicate) => TakeWhileAwaitCore<TSource>(source, predicate);

        /// <summary>
        /// Returns elements from an async-enumerable sequence as long as a specified condition is true.
        /// The element's index is used in the logic of the predicate function.
        /// </summary>
        /// <typeparam name="TSource">The type of the elements in the source sequence.</typeparam>
        /// <param name="source">A sequence to return elements from.</param>
        /// <param name="predicate">An asynchronous function to test each element for a condition; the second parameter of the function represents the index of the source element.</param>
        /// <returns>An async-enumerable sequence that contains the elements from the input sequence that occur before the element at which the test no longer passes.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="source"/> or <paramref name="predicate"/> is null.</exception>
        public static IAsyncEnumerable<TSource> TakeWhileAwait<TSource>(this IAsyncEnumerable<TSource> source, Func<TSource, int, ValueTask<bool>> predicate) => TakeWhileAwaitCore<TSource>(source, predicate);

        /// <summary>
        /// Performs a subsequent ordering of the elements in a sequence in ascending order according to a key obtained by invoking a transform function on each element and awaiting the result.
        /// </summary>
        /// <typeparam name="TSource">The type of the elements of source.</typeparam>
        /// <typeparam name="TKey">The type of the key returned by keySelector.</typeparam>
        /// <param name="source">An ordered async-enumerable sequence that contains elements to sort.</param>
        /// <param name="keySelector">An asynchronous function to extract a key from each element.</param>
        /// <returns>An ordered async-enumerable whose elements are sorted according to a key.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="source"/> or <paramref name="keySelector"/> is null.</exception>
        public static IOrderedAsyncEnumerable<TSource> ThenByAwait<TSource, TKey>(this IOrderedAsyncEnumerable<TSource> source, Func<TSource, ValueTask<TKey>> keySelector) => ThenByAwaitCore<TSource, TKey>(source, keySelector);

        /// <summary>
        /// Performs a subsequent ordering of the elements in a sequence in ascending order by using a specified comparer. The keys are obtained by invoking a transform function on each element and awaiting the result.
        /// </summary>
        /// <typeparam name="TSource">The type of the elements of source.</typeparam>
        /// <typeparam name="TKey">The type of the key returned by keySelector.</typeparam>
        /// <param name="source">An ordered async-enumerable sequence that contains elements to sort.</param>
        /// <param name="keySelector">An asynchronous function to extract a key from each element.</param>
        /// <param name="comparer">A comparer to compare keys.</param>
        /// <returns>An ordered async-enumerable whose elements are sorted according to a key.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="source"/> or <paramref name="keySelector"/> is null.</exception>
        public static IOrderedAsyncEnumerable<TSource> ThenByAwait<TSource, TKey>(this IOrderedAsyncEnumerable<TSource> source, Func<TSource, ValueTask<TKey>> keySelector, IComparer<TKey> comparer) => ThenByAwaitCore<TSource, TKey>(source, keySelector, comparer);

        /// <summary>
        /// Performs a subsequent ordering of the elements in a sequence in descending order, according to a key obtained by invoking a transform function on each element and awaiting the result.
        /// </summary>
        /// <typeparam name="TSource">The type of the elements of source.</typeparam>
        /// <typeparam name="TKey">The type of the key returned by keySelector.</typeparam>
        /// <param name="source">An ordered async-enumerable sequence that contains elements to sort.</param>
        /// <param name="keySelector">An asynchronous function to extract a key from each element.</param>
        /// <returns>An ordered async-enumerable sequence whose elements are sorted in descending order according to a key.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="source"/> or <paramref name="keySelector"/> is null.</exception>
        public static IOrderedAsyncEnumerable<TSource> ThenByDescendingAwait<TSource, TKey>(this IOrderedAsyncEnumerable<TSource> source, Func<TSource, ValueTask<TKey>> keySelector) => ThenByDescendingAwaitCore<TSource, TKey>(source, keySelector);

        /// <summary>
        /// Performs a subsequent ordering of the elements in a sequence in descending order by using a specified comparer. The keys are obtained by invoking a transform function on each element and awaiting the result.
        /// </summary>
        /// <typeparam name="TSource">The type of the elements of source.</typeparam>
        /// <typeparam name="TKey">The type of the key returned by keySelector.</typeparam>
        /// <param name="source">An ordered async-enumerable sequence that contains elements to sort.</param>
        /// <param name="keySelector">An asynchronous function to extract a key from each element.</param>
        /// <param name="comparer">A comparer to compare keys.</param>
        /// <returns>An ordered async-enumerable sequence whose elements are sorted in descending order according to a key.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="source"/> or <paramref name="keySelector"/> is null.</exception>
        public static IOrderedAsyncEnumerable<TSource> ThenByDescendingAwait<TSource, TKey>(this IOrderedAsyncEnumerable<TSource> source, Func<TSource, ValueTask<TKey>> keySelector, IComparer<TKey> comparer) => ThenByDescendingAwaitCore<TSource, TKey>(source, keySelector, comparer);

        /// <summary>
        /// Creates a dictionary from an async-enumerable sequence by invoking a key-selector function on each element and awaiting the result.
        /// </summary>
        /// <typeparam name="TSource">The type of the elements in the source sequence.</typeparam>
        /// <typeparam name="TKey">The type of the dictionary key computed for each element in the source sequence.</typeparam>
        /// <param name="source">An async-enumerable sequence to create a dictionary for.</param>
        /// <param name="keySelector">An asynchronous function to extract a key from each element.</param>
        /// <param name="cancellationToken">The optional cancellation token to be used for cancelling the sequence at any time.</param>
        /// <returns>A ValueTask containing a dictionary mapping unique key values onto the corresponding source sequence's element.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="source"/> or <paramref name="keySelector"/> is null.</exception>
        /// <remarks>The return type of this operator differs from the corresponding operator on IEnumerable in order to retain asynchronous behavior.</remarks>
        public static ValueTask<Dictionary<TKey, TSource>> ToDictionaryAwaitAsync<TSource, TKey>(this IAsyncEnumerable<TSource> source, Func<TSource, ValueTask<TKey>> keySelector, CancellationToken cancellationToken = default) where TKey : notnull => ToDictionaryAwaitAsyncCore<TSource, TKey>(source, keySelector, cancellationToken);

        /// <summary>
        /// Creates a dictionary from an async-enumerable sequence by invoking a key-selector function on each element and awaiting the result.
        /// </summary>
        /// <typeparam name="TSource">The type of the elements in the source sequence.</typeparam>
        /// <typeparam name="TKey">The type of the dictionary key computed for each element in the source sequence.</typeparam>
        /// <param name="source">An async-enumerable sequence to create a dictionary for.</param>
        /// <param name="keySelector">An asynchronous function to extract a key from each element.</param>
        /// <param name="comparer">An equality comparer to compare keys.</param>
        /// <param name="cancellationToken">The optional cancellation token to be used for cancelling the sequence at any time.</param>
        /// <returns>A ValueTask containing a dictionary mapping unique key values onto the corresponding source sequence's element.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="source"/> or <paramref name="keySelector"/> or <paramref name="comparer"/> is null.</exception>
        /// <remarks>The return type of this operator differs from the corresponding operator on IEnumerable in order to retain asynchronous behavior.</remarks>
        public static ValueTask<Dictionary<TKey, TSource>> ToDictionaryAwaitAsync<TSource, TKey>(this IAsyncEnumerable<TSource> source, Func<TSource, ValueTask<TKey>> keySelector, IEqualityComparer<TKey> comparer, CancellationToken cancellationToken = default) where TKey : notnull => ToDictionaryAwaitAsyncCore<TSource, TKey>(source, keySelector, comparer, cancellationToken);

        /// <summary>
        /// Creates a dictionary from an async-enumerable sequence using the specified asynchronous key and element selector functions.
        /// </summary>
        /// <typeparam name="TSource">The type of the elements in the source sequence.</typeparam>
        /// <typeparam name="TKey">The type of the dictionary key computed for each element in the source sequence.</typeparam>
        /// <typeparam name="TElement">The type of the dictionary value computed for each element in the source sequence.</typeparam>
        /// <param name="source">An async-enumerable sequence to create a dictionary for.</param>
        /// <param name="keySelector">An asynchronous function to extract a key from each element.</param>
        /// <param name="elementSelector">An asynchronous transform function to produce a result element value from each element.</param>
        /// <param name="cancellationToken">The optional cancellation token to be used for cancelling the sequence at any time.</param>
        /// <returns>A ValueTask containing a dictionary mapping unique key values onto the corresponding source sequence's element.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="source"/> or <paramref name="keySelector"/> or <paramref name="elementSelector"/> is null.</exception>
        /// <remarks>The return type of this operator differs from the corresponding operator on IEnumerable in order to retain asynchronous behavior.</remarks>
        public static ValueTask<Dictionary<TKey, TElement>> ToDictionaryAwaitAsync<TSource, TKey, TElement>(this IAsyncEnumerable<TSource> source, Func<TSource, ValueTask<TKey>> keySelector, Func<TSource, ValueTask<TElement>> elementSelector, CancellationToken cancellationToken = default) where TKey : notnull => ToDictionaryAwaitAsyncCore<TSource, TKey, TElement>(source, keySelector, elementSelector, cancellationToken);

        /// <summary>
        /// Creates a dictionary from an async-enumerable sequence using the specified asynchronous key and element selector functions.
        /// </summary>
        /// <typeparam name="TSource">The type of the elements in the source sequence.</typeparam>
        /// <typeparam name="TKey">The type of the dictionary key computed for each element in the source sequence.</typeparam>
        /// <typeparam name="TElement">The type of the dictionary value computed for each element in the source sequence.</typeparam>
        /// <param name="source">An async-enumerable sequence to create a dictionary for.</param>
        /// <param name="keySelector">An asynchronous function to extract a key from each element.</param>
        /// <param name="elementSelector">An asynchronous transform function to produce a result element value from each element.</param>
        /// <param name="comparer">An equality comparer to compare keys.</param>
        /// <param name="cancellationToken">The optional cancellation token to be used for cancelling the sequence at any time.</param>
        /// <returns>A ValueTask containing a dictionary mapping unique key values onto the corresponding source sequence's element.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="source"/> or <paramref name="keySelector"/> or <paramref name="elementSelector"/> or <paramref name="comparer"/> is null.</exception>
        /// <remarks>The return type of this operator differs from the corresponding operator on IEnumerable in order to retain asynchronous behavior.</remarks>
        public static ValueTask<Dictionary<TKey, TElement>> ToDictionaryAwaitAsync<TSource, TKey, TElement>(this IAsyncEnumerable<TSource> source, Func<TSource, ValueTask<TKey>> keySelector, Func<TSource, ValueTask<TElement>> elementSelector, IEqualityComparer<TKey> comparer, CancellationToken cancellationToken = default) where TKey : notnull => ToDictionaryAwaitAsyncCore<TSource, TKey, TElement>(source, keySelector, elementSelector, comparer, cancellationToken);

        /// <summary>
        /// Creates a lookup from an async-enumerable sequence by invoking a key-selector function on each element and awaiting the result.
        /// </summary>
        /// <typeparam name="TSource">The type of the elements in the source sequence.</typeparam>
        /// <typeparam name="TKey">The type of the lookup key computed for each element in the source sequence.</typeparam>
        /// <param name="source">An async-enumerable sequence to create a lookup for.</param>
        /// <param name="keySelector">An asynchronous function to extract a key from each element.</param>
        /// <param name="cancellationToken">The optional cancellation token to be used for cancelling the sequence at any time.</param>
        /// <returns>A ValueTask containing a lookup mapping unique key values onto the corresponding source sequence's elements.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="source"/> or <paramref name="keySelector"/> is null.</exception>
        /// <remarks>The return type of this operator differs from the corresponding operator on IEnumerable in order to retain asynchronous behavior.</remarks>
        public static ValueTask<ILookup<TKey, TSource>> ToLookupAwaitAsync<TSource, TKey>(this IAsyncEnumerable<TSource> source, Func<TSource, ValueTask<TKey>> keySelector, CancellationToken cancellationToken = default) => ToLookupAwaitAsyncCore<TSource, TKey>(source, keySelector, cancellationToken);

        /// <summary>
        /// Creates a lookup from an async-enumerable sequence by invoking key and element selector functions on each source element and awaiting the results.
        /// </summary>
        /// <typeparam name="TSource">The type of the elements in the source sequence.</typeparam>
        /// <typeparam name="TKey">The type of the lookup key computed for each element in the source sequence.</typeparam>
        /// <typeparam name="TElement">The type of the lookup value computed for each element in the source sequence.</typeparam>
        /// <param name="source">An async-enumerable sequence to create a lookup for.</param>
        /// <param name="keySelector">An asynchronous function to extract a key from each element.</param>
        /// <param name="elementSelector">An asynchronous transform function to produce a result element value from each element.</param>
        /// <param name="cancellationToken">The optional cancellation token to be used for cancelling the sequence at any time.</param>
        /// <returns>An async-enumerable sequence containing a single element with a lookup mapping unique key values onto the corresponding source sequence's elements.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="source"/> or <paramref name="keySelector"/> or <paramref name="elementSelector"/> is null.</exception>
        /// <remarks>The return type of this operator differs from the corresponding operator on IEnumerable in order to retain asynchronous behavior.</remarks>
        public static ValueTask<ILookup<TKey, TElement>> ToLookupAwaitAsync<TSource, TKey, TElement>(this IAsyncEnumerable<TSource> source, Func<TSource, ValueTask<TKey>> keySelector, Func<TSource, ValueTask<TElement>> elementSelector, CancellationToken cancellationToken = default) => ToLookupAwaitAsyncCore<TSource, TKey, TElement>(source, keySelector, elementSelector, cancellationToken);

        /// <summary>
        /// Creates a lookup from an async-enumerable sequence by invoking a key-selector function on each element and awaiting the result.
        /// </summary>
        /// <typeparam name="TSource">The type of the elements in the source sequence.</typeparam>
        /// <typeparam name="TKey">The type of the lookup key computed for each element in the source sequence.</typeparam>
        /// <param name="source">An async-enumerable sequence to create a lookup for.</param>
        /// <param name="keySelector">An asynchronous function to extract a key from each element.</param>
        /// <param name="comparer">An equality comparer to compare keys.</param>
        /// <param name="cancellationToken">The optional cancellation token to be used for cancelling the sequence at any time.</param>
        /// <returns>A ValueTask containing a lookup mapping unique key values onto the corresponding source sequence's elements.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="source"/> or <paramref name="keySelector"/> or <paramref name="comparer"/> is null.</exception>
        /// <remarks>The return type of this operator differs from the corresponding operator on IEnumerable in order to retain asynchronous behavior.</remarks>
        public static ValueTask<ILookup<TKey, TSource>> ToLookupAwaitAsync<TSource, TKey>(this IAsyncEnumerable<TSource> source, Func<TSource, ValueTask<TKey>> keySelector, IEqualityComparer<TKey> comparer, CancellationToken cancellationToken = default) => ToLookupAwaitAsyncCore<TSource, TKey>(source, keySelector, comparer, cancellationToken);

        /// <summary>
        /// Creates a lookup from an async-enumerable sequence by invoking key and element selector functions on each source element and awaiting the results.
        /// </summary>
        /// <typeparam name="TSource">The type of the elements in the source sequence.</typeparam>
        /// <typeparam name="TKey">The type of the lookup key computed for each element in the source sequence.</typeparam>
        /// <typeparam name="TElement">The type of the lookup value computed for each element in the source sequence.</typeparam>
        /// <param name="source">An async-enumerable sequence to create a lookup for.</param>
        /// <param name="keySelector">An asynchronous function to extract a key from each element.</param>
        /// <param name="elementSelector">An asynchronous transform function to produce a result element value from each source element.</param>
        /// <param name="comparer">An equality comparer to compare keys.</param>
        /// <param name="cancellationToken">The optional cancellation token to be used for cancelling the sequence at any time.</param>
        /// <returns>A ValueTask containing a lookup mapping unique key values onto the corresponding source sequence's elements.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="source"/> or <paramref name="keySelector"/> or <paramref name="elementSelector"/> or <paramref name="comparer"/> is null.</exception>
        /// <remarks>The return type of this operator differs from the corresponding operator on IEnumerable in order to retain asynchronous behavior.</remarks>
        public static ValueTask<ILookup<TKey, TElement>> ToLookupAwaitAsync<TSource, TKey, TElement>(this IAsyncEnumerable<TSource> source, Func<TSource, ValueTask<TKey>> keySelector, Func<TSource, ValueTask<TElement>> elementSelector, IEqualityComparer<TKey> comparer, CancellationToken cancellationToken = default) => ToLookupAwaitAsyncCore<TSource, TKey, TElement>(source, keySelector, elementSelector, comparer, cancellationToken);

        /// <summary>
        /// Filters the elements of an async-enumerable sequence based on an asynchronous predicate.
        /// </summary>
        /// <typeparam name="TSource">The type of the elements in the source sequence.</typeparam>
        /// <param name="source">An async-enumerable sequence whose elements to filter.</param>
        /// <param name="predicate">An asynchronous predicate to test each source element for a condition.</param>
        /// <returns>An async-enumerable sequence that contains elements from the input sequence that satisfy the condition.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="source"/> or <paramref name="predicate"/> is null.</exception>
        public static IAsyncEnumerable<TSource> WhereAwait<TSource>(this IAsyncEnumerable<TSource> source, Func<TSource, ValueTask<bool>> predicate) => WhereAwaitCore<TSource>(source, predicate);

        /// <summary>
        /// Filters the elements of an async-enumerable sequence based on an asynchronous predicate that incorporates the element's index.
        /// </summary>
        /// <typeparam name="TSource">The type of the elements in the source sequence.</typeparam>
        /// <param name="source">An async-enumerable sequence whose elements to filter.</param>
        /// <param name="predicate">An asynchronous predicate to test each source element for a condition; the second parameter of the function represents the index of the source element.</param>
        /// <returns>An async-enumerable sequence that contains elements from the input sequence that satisfy the condition.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="source"/> or <paramref name="predicate"/> is null.</exception>
        public static IAsyncEnumerable<TSource> WhereAwait<TSource>(this IAsyncEnumerable<TSource> source, Func<TSource, int, ValueTask<bool>> predicate) => WhereAwaitCore<TSource>(source, predicate);

        /// <summary>
        /// Merges two async-enumerable sequences into one async-enumerable sequence by combining their elements in a pairwise fashion.
        /// </summary>
        /// <typeparam name="TFirst">The type of the elements in the first source sequence.</typeparam>
        /// <typeparam name="TSecond">The type of the elements in the second source sequence.</typeparam>
        /// <typeparam name="TResult">The type of the elements in the result sequence, returned by the selector function.</typeparam>
        /// <param name="first">First async-enumerable source.</param>
        /// <param name="second">Second async-enumerable source.</param>
        /// <param name="selector">An asynchronous function to invoke and await for each consecutive pair of elements from the first and second source.</param>
        /// <returns>An async-enumerable sequence containing the result of pairwise combining the elements of the first and second source using the specified result selector function.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="first"/> or <paramref name="second"/> or <paramref name="selector"/> is null.</exception>
        public static IAsyncEnumerable<TResult> ZipAwait<TFirst, TSecond, TResult>(this IAsyncEnumerable<TFirst> first, IAsyncEnumerable<TSecond> second, Func<TFirst, TSecond, ValueTask<TResult>> selector) => ZipAwaitCore<TFirst, TSecond, TResult>(first, second, selector);

#if !NO_DEEP_CANCELLATION
        public static ValueTask<long> MaxAwaitWithCancellationAsync<TSource>(this IAsyncEnumerable<TSource> source, Func<TSource, CancellationToken, ValueTask<long>> selector, CancellationToken cancellationToken = default) => MaxAwaitWithCancellationAsyncCore<TSource>(source, selector, cancellationToken);
        public static ValueTask<long?> MaxAwaitWithCancellationAsync<TSource>(this IAsyncEnumerable<TSource> source, Func<TSource, CancellationToken, ValueTask<long?>> selector, CancellationToken cancellationToken = default) => MaxAwaitWithCancellationAsyncCore<TSource>(source, selector, cancellationToken);
        public static ValueTask<float> MaxAwaitWithCancellationAsync<TSource>(this IAsyncEnumerable<TSource> source, Func<TSource, CancellationToken, ValueTask<float>> selector, CancellationToken cancellationToken = default) => MaxAwaitWithCancellationAsyncCore<TSource>(source, selector, cancellationToken);
        public static ValueTask<float?> MaxAwaitWithCancellationAsync<TSource>(this IAsyncEnumerable<TSource> source, Func<TSource, CancellationToken, ValueTask<float?>> selector, CancellationToken cancellationToken = default) => MaxAwaitWithCancellationAsyncCore<TSource>(source, selector, cancellationToken);
        public static ValueTask<double> MaxAwaitWithCancellationAsync<TSource>(this IAsyncEnumerable<TSource> source, Func<TSource, CancellationToken, ValueTask<double>> selector, CancellationToken cancellationToken = default) => MaxAwaitWithCancellationAsyncCore<TSource>(source, selector, cancellationToken);
        public static ValueTask<double?> MaxAwaitWithCancellationAsync<TSource>(this IAsyncEnumerable<TSource> source, Func<TSource, CancellationToken, ValueTask<double?>> selector, CancellationToken cancellationToken = default) => MaxAwaitWithCancellationAsyncCore<TSource>(source, selector, cancellationToken);
        public static ValueTask<decimal> MaxAwaitWithCancellationAsync<TSource>(this IAsyncEnumerable<TSource> source, Func<TSource, CancellationToken, ValueTask<decimal>> selector, CancellationToken cancellationToken = default) => MaxAwaitWithCancellationAsyncCore<TSource>(source, selector, cancellationToken);
        public static ValueTask<decimal?> MaxAwaitWithCancellationAsync<TSource>(this IAsyncEnumerable<TSource> source, Func<TSource, CancellationToken, ValueTask<decimal?>> selector, CancellationToken cancellationToken = default) => MaxAwaitWithCancellationAsyncCore<TSource>(source, selector, cancellationToken);
        public static ValueTask<TResult> MaxAwaitWithCancellationAsync<TSource, TResult>(this IAsyncEnumerable<TSource> source, Func<TSource, CancellationToken, ValueTask<TResult>> selector, CancellationToken cancellationToken = default) => MaxAwaitWithCancellationAsyncCore<TSource, TResult>(source, selector, cancellationToken);
        public static ValueTask<int> MaxAwaitWithCancellationAsync<TSource>(this IAsyncEnumerable<TSource> source, Func<TSource, CancellationToken, ValueTask<int>> selector, CancellationToken cancellationToken = default) => MaxAwaitWithCancellationAsyncCore<TSource>(source, selector, cancellationToken);
        public static ValueTask<int?> MaxAwaitWithCancellationAsync<TSource>(this IAsyncEnumerable<TSource> source, Func<TSource, CancellationToken, ValueTask<int?>> selector, CancellationToken cancellationToken = default) => MaxAwaitWithCancellationAsyncCore<TSource>(source, selector, cancellationToken);
        public static ValueTask<double> MinAwaitWithCancellationAsync<TSource>(this IAsyncEnumerable<TSource> source, Func<TSource, CancellationToken, ValueTask<double>> selector, CancellationToken cancellationToken = default) => MinAwaitWithCancellationAsyncCore<TSource>(source, selector, cancellationToken);
        public static ValueTask<double?> MinAwaitWithCancellationAsync<TSource>(this IAsyncEnumerable<TSource> source, Func<TSource, CancellationToken, ValueTask<double?>> selector, CancellationToken cancellationToken = default) => MinAwaitWithCancellationAsyncCore<TSource>(source, selector, cancellationToken);
        public static ValueTask<decimal> MinAwaitWithCancellationAsync<TSource>(this IAsyncEnumerable<TSource> source, Func<TSource, CancellationToken, ValueTask<decimal>> selector, CancellationToken cancellationToken = default) => MinAwaitWithCancellationAsyncCore<TSource>(source, selector, cancellationToken);
        public static ValueTask<decimal?> MinAwaitWithCancellationAsync<TSource>(this IAsyncEnumerable<TSource> source, Func<TSource, CancellationToken, ValueTask<decimal?>> selector, CancellationToken cancellationToken = default) => MinAwaitWithCancellationAsyncCore<TSource>(source, selector, cancellationToken);
        public static ValueTask<int> MinAwaitWithCancellationAsync<TSource>(this IAsyncEnumerable<TSource> source, Func<TSource, CancellationToken, ValueTask<int>> selector, CancellationToken cancellationToken = default) => MinAwaitWithCancellationAsyncCore<TSource>(source, selector, cancellationToken);
        public static ValueTask<int?> MinAwaitWithCancellationAsync<TSource>(this IAsyncEnumerable<TSource> source, Func<TSource, CancellationToken, ValueTask<int?>> selector, CancellationToken cancellationToken = default) => MinAwaitWithCancellationAsyncCore<TSource>(source, selector, cancellationToken);
        public static ValueTask<long> MinAwaitWithCancellationAsync<TSource>(this IAsyncEnumerable<TSource> source, Func<TSource, CancellationToken, ValueTask<long>> selector, CancellationToken cancellationToken = default) => MinAwaitWithCancellationAsyncCore<TSource>(source, selector, cancellationToken);
        public static ValueTask<long?> MinAwaitWithCancellationAsync<TSource>(this IAsyncEnumerable<TSource> source, Func<TSource, CancellationToken, ValueTask<long?>> selector, CancellationToken cancellationToken = default) => MinAwaitWithCancellationAsyncCore<TSource>(source, selector, cancellationToken);
        public static ValueTask<float> MinAwaitWithCancellationAsync<TSource>(this IAsyncEnumerable<TSource> source, Func<TSource, CancellationToken, ValueTask<float>> selector, CancellationToken cancellationToken = default) => MinAwaitWithCancellationAsyncCore<TSource>(source, selector, cancellationToken);
        public static ValueTask<float?> MinAwaitWithCancellationAsync<TSource>(this IAsyncEnumerable<TSource> source, Func<TSource, CancellationToken, ValueTask<float?>> selector, CancellationToken cancellationToken = default) => MinAwaitWithCancellationAsyncCore<TSource>(source, selector, cancellationToken);
        public static ValueTask<TResult> MinAwaitWithCancellationAsync<TSource, TResult>(this IAsyncEnumerable<TSource> source, Func<TSource, CancellationToken, ValueTask<TResult>> selector, CancellationToken cancellationToken = default) => MinAwaitWithCancellationAsyncCore<TSource, TResult>(source, selector, cancellationToken);
        public static IOrderedAsyncEnumerable<TSource> OrderByAwaitWithCancellation<TSource, TKey>(this IAsyncEnumerable<TSource> source, Func<TSource, CancellationToken, ValueTask<TKey>> keySelector) => OrderByAwaitWithCancellationCore<TSource, TKey>(source, keySelector);
        public static IOrderedAsyncEnumerable<TSource> OrderByAwaitWithCancellation<TSource, TKey>(this IAsyncEnumerable<TSource> source, Func<TSource, CancellationToken, ValueTask<TKey>> keySelector, IComparer<TKey> comparer) => OrderByAwaitWithCancellationCore<TSource, TKey>(source, keySelector, comparer);
        public static IOrderedAsyncEnumerable<TSource> OrderByDescendingAwaitWithCancellation<TSource, TKey>(this IAsyncEnumerable<TSource> source, Func<TSource, CancellationToken, ValueTask<TKey>> keySelector) => OrderByDescendingAwaitWithCancellationCore<TSource, TKey>(source, keySelector);
        public static IOrderedAsyncEnumerable<TSource> OrderByDescendingAwaitWithCancellation<TSource, TKey>(this IAsyncEnumerable<TSource> source, Func<TSource, CancellationToken, ValueTask<TKey>> keySelector, IComparer<TKey> comparer) => OrderByDescendingAwaitWithCancellationCore<TSource, TKey>(source, keySelector, comparer);
        public static IAsyncEnumerable<TResult> SelectAwaitWithCancellation<TSource, TResult>(this IAsyncEnumerable<TSource> source, Func<TSource, CancellationToken, ValueTask<TResult>> selector) => SelectAwaitWithCancellationCore<TSource, TResult>(source, selector);
        public static IAsyncEnumerable<TResult> SelectAwaitWithCancellation<TSource, TResult>(this IAsyncEnumerable<TSource> source, Func<TSource, int, CancellationToken, ValueTask<TResult>> selector) => SelectAwaitWithCancellationCore<TSource, TResult>(source, selector);
        public static IAsyncEnumerable<TResult> SelectManyAwaitWithCancellation<TSource, TResult>(this IAsyncEnumerable<TSource> source, Func<TSource, CancellationToken, ValueTask<IAsyncEnumerable<TResult>>> selector) => SelectManyAwaitWithCancellationCore<TSource, TResult>(source, selector);
        public static IAsyncEnumerable<TResult> SelectManyAwaitWithCancellation<TSource, TResult>(this IAsyncEnumerable<TSource> source, Func<TSource, int, CancellationToken, ValueTask<IAsyncEnumerable<TResult>>> selector) => SelectManyAwaitWithCancellationCore<TSource, TResult>(source, selector);
        public static IAsyncEnumerable<TResult> SelectManyAwaitWithCancellation<TSource, TCollection, TResult>(this IAsyncEnumerable<TSource> source, Func<TSource, CancellationToken, ValueTask<IAsyncEnumerable<TCollection>>> collectionSelector, Func<TSource, TCollection, CancellationToken, ValueTask<TResult>> resultSelector) => SelectManyAwaitWithCancellationCore<TSource, TCollection, TResult>(source, collectionSelector, resultSelector);
        public static IAsyncEnumerable<TResult> SelectManyAwaitWithCancellation<TSource, TCollection, TResult>(this IAsyncEnumerable<TSource> source, Func<TSource, int, CancellationToken, ValueTask<IAsyncEnumerable<TCollection>>> collectionSelector, Func<TSource, TCollection, CancellationToken, ValueTask<TResult>> resultSelector) => SelectManyAwaitWithCancellationCore<TSource, TCollection, TResult>(source, collectionSelector, resultSelector);
        public static ValueTask<TSource> SingleAwaitWithCancellationAsync<TSource>(this IAsyncEnumerable<TSource> source, Func<TSource, CancellationToken, ValueTask<bool>> predicate, CancellationToken cancellationToken = default) => SingleAwaitWithCancellationAsyncCore<TSource>(source, predicate, cancellationToken);
        public static ValueTask<TSource?> SingleOrDefaultAwaitWithCancellationAsync<TSource>(this IAsyncEnumerable<TSource> source, Func<TSource, CancellationToken, ValueTask<bool>> predicate, CancellationToken cancellationToken = default) => SingleOrDefaultAwaitWithCancellationAsyncCore<TSource>(source, predicate, cancellationToken);
        public static IAsyncEnumerable<TSource> SkipWhileAwaitWithCancellation<TSource>(this IAsyncEnumerable<TSource> source, Func<TSource, CancellationToken, ValueTask<bool>> predicate) => SkipWhileAwaitWithCancellationCore<TSource>(source, predicate);
        public static IAsyncEnumerable<TSource> SkipWhileAwaitWithCancellation<TSource>(this IAsyncEnumerable<TSource> source, Func<TSource, int, CancellationToken, ValueTask<bool>> predicate) => SkipWhileAwaitWithCancellationCore<TSource>(source, predicate);
        public static ValueTask<int?> SumAwaitWithCancellationAsync<TSource>(this IAsyncEnumerable<TSource> source, Func<TSource, CancellationToken, ValueTask<int?>> selector, CancellationToken cancellationToken = default) => SumAwaitWithCancellationAsyncCore<TSource>(source, selector, cancellationToken);
        public static ValueTask<long?> SumAwaitWithCancellationAsync<TSource>(this IAsyncEnumerable<TSource> source, Func<TSource, CancellationToken, ValueTask<long?>> selector, CancellationToken cancellationToken = default) => SumAwaitWithCancellationAsyncCore<TSource>(source, selector, cancellationToken);
        public static ValueTask<float?> SumAwaitWithCancellationAsync<TSource>(this IAsyncEnumerable<TSource> source, Func<TSource, CancellationToken, ValueTask<float?>> selector, CancellationToken cancellationToken = default) => SumAwaitWithCancellationAsyncCore<TSource>(source, selector, cancellationToken);
        public static ValueTask<double?> SumAwaitWithCancellationAsync<TSource>(this IAsyncEnumerable<TSource> source, Func<TSource, CancellationToken, ValueTask<double?>> selector, CancellationToken cancellationToken = default) => SumAwaitWithCancellationAsyncCore<TSource>(source, selector, cancellationToken);
        public static ValueTask<decimal?> SumAwaitWithCancellationAsync<TSource>(this IAsyncEnumerable<TSource> source, Func<TSource, CancellationToken, ValueTask<decimal?>> selector, CancellationToken cancellationToken = default) => SumAwaitWithCancellationAsyncCore<TSource>(source, selector, cancellationToken);
        public static ValueTask<int> SumAwaitWithCancellationAsync<TSource>(this IAsyncEnumerable<TSource> source, Func<TSource, CancellationToken, ValueTask<int>> selector, CancellationToken cancellationToken = default) => SumAwaitWithCancellationAsyncCore<TSource>(source, selector, cancellationToken);
        public static ValueTask<long> SumAwaitWithCancellationAsync<TSource>(this IAsyncEnumerable<TSource> source, Func<TSource, CancellationToken, ValueTask<long>> selector, CancellationToken cancellationToken = default) => SumAwaitWithCancellationAsyncCore<TSource>(source, selector, cancellationToken);
        public static ValueTask<float> SumAwaitWithCancellationAsync<TSource>(this IAsyncEnumerable<TSource> source, Func<TSource, CancellationToken, ValueTask<float>> selector, CancellationToken cancellationToken = default) => SumAwaitWithCancellationAsyncCore<TSource>(source, selector, cancellationToken);
        public static ValueTask<double> SumAwaitWithCancellationAsync<TSource>(this IAsyncEnumerable<TSource> source, Func<TSource, CancellationToken, ValueTask<double>> selector, CancellationToken cancellationToken = default) => SumAwaitWithCancellationAsyncCore<TSource>(source, selector, cancellationToken);
        public static ValueTask<decimal> SumAwaitWithCancellationAsync<TSource>(this IAsyncEnumerable<TSource> source, Func<TSource, CancellationToken, ValueTask<decimal>> selector, CancellationToken cancellationToken = default) => SumAwaitWithCancellationAsyncCore<TSource>(source, selector, cancellationToken);
        public static IAsyncEnumerable<TSource> TakeWhileAwaitWithCancellation<TSource>(this IAsyncEnumerable<TSource> source, Func<TSource, CancellationToken, ValueTask<bool>> predicate) => TakeWhileAwaitWithCancellationCore<TSource>(source, predicate);
        public static IAsyncEnumerable<TSource> TakeWhileAwaitWithCancellation<TSource>(this IAsyncEnumerable<TSource> source, Func<TSource, int, CancellationToken, ValueTask<bool>> predicate) => TakeWhileAwaitWithCancellationCore<TSource>(source, predicate);
        public static IOrderedAsyncEnumerable<TSource> ThenByAwaitWithCancellation<TSource, TKey>(this IOrderedAsyncEnumerable<TSource> source, Func<TSource, CancellationToken, ValueTask<TKey>> keySelector) => ThenByAwaitWithCancellationCore<TSource, TKey>(source, keySelector);
        public static IOrderedAsyncEnumerable<TSource> ThenByAwaitWithCancellation<TSource, TKey>(this IOrderedAsyncEnumerable<TSource> source, Func<TSource, CancellationToken, ValueTask<TKey>> keySelector, IComparer<TKey> comparer) => ThenByAwaitWithCancellationCore<TSource, TKey>(source, keySelector, comparer);
        public static IOrderedAsyncEnumerable<TSource> ThenByDescendingAwaitWithCancellation<TSource, TKey>(this IOrderedAsyncEnumerable<TSource> source, Func<TSource, CancellationToken, ValueTask<TKey>> keySelector) => ThenByDescendingAwaitWithCancellationCore<TSource, TKey>(source, keySelector);
        public static IOrderedAsyncEnumerable<TSource> ThenByDescendingAwaitWithCancellation<TSource, TKey>(this IOrderedAsyncEnumerable<TSource> source, Func<TSource, CancellationToken, ValueTask<TKey>> keySelector, IComparer<TKey> comparer) => ThenByDescendingAwaitWithCancellationCore<TSource, TKey>(source, keySelector, comparer);
        public static ValueTask<Dictionary<TKey, TSource>> ToDictionaryAwaitWithCancellationAsync<TSource, TKey>(this IAsyncEnumerable<TSource> source, Func<TSource, CancellationToken, ValueTask<TKey>> keySelector, CancellationToken cancellationToken = default) where TKey : notnull => ToDictionaryAwaitWithCancellationAsyncCore<TSource, TKey>(source, keySelector, cancellationToken);
        public static ValueTask<Dictionary<TKey, TSource>> ToDictionaryAwaitWithCancellationAsync<TSource, TKey>(this IAsyncEnumerable<TSource> source, Func<TSource, CancellationToken, ValueTask<TKey>> keySelector, IEqualityComparer<TKey> comparer, CancellationToken cancellationToken = default) where TKey : notnull => ToDictionaryAwaitWithCancellationAsyncCore<TSource, TKey>(source, keySelector, comparer, cancellationToken);
        public static ValueTask<Dictionary<TKey, TElement>> ToDictionaryAwaitWithCancellationAsync<TSource, TKey, TElement>(this IAsyncEnumerable<TSource> source, Func<TSource, CancellationToken, ValueTask<TKey>> keySelector, Func<TSource, CancellationToken, ValueTask<TElement>> elementSelector, CancellationToken cancellationToken = default) where TKey : notnull => ToDictionaryAwaitWithCancellationAsyncCore<TSource, TKey, TElement>(source, keySelector, elementSelector, cancellationToken);
        public static ValueTask<Dictionary<TKey, TElement>> ToDictionaryAwaitWithCancellationAsync<TSource, TKey, TElement>(this IAsyncEnumerable<TSource> source, Func<TSource, CancellationToken, ValueTask<TKey>> keySelector, Func<TSource, CancellationToken, ValueTask<TElement>> elementSelector, IEqualityComparer<TKey> comparer, CancellationToken cancellationToken = default) where TKey : notnull => ToDictionaryAwaitWithCancellationAsyncCore<TSource, TKey, TElement>(source, keySelector, elementSelector, comparer, cancellationToken);
        public static ValueTask<ILookup<TKey, TSource>> ToLookupAwaitWithCancellationAsync<TSource, TKey>(this IAsyncEnumerable<TSource> source, Func<TSource, CancellationToken, ValueTask<TKey>> keySelector, CancellationToken cancellationToken = default) => ToLookupAwaitWithCancellationAsyncCore<TSource, TKey>(source, keySelector, cancellationToken);
        public static ValueTask<ILookup<TKey, TElement>> ToLookupAwaitWithCancellationAsync<TSource, TKey, TElement>(this IAsyncEnumerable<TSource> source, Func<TSource, CancellationToken, ValueTask<TKey>> keySelector, Func<TSource, CancellationToken, ValueTask<TElement>> elementSelector, CancellationToken cancellationToken = default) => ToLookupAwaitWithCancellationAsyncCore<TSource, TKey, TElement>(source, keySelector, elementSelector, cancellationToken);
        public static ValueTask<ILookup<TKey, TSource>> ToLookupAwaitWithCancellationAsync<TSource, TKey>(this IAsyncEnumerable<TSource> source, Func<TSource, CancellationToken, ValueTask<TKey>> keySelector, IEqualityComparer<TKey> comparer, CancellationToken cancellationToken = default) => ToLookupAwaitWithCancellationAsyncCore<TSource, TKey>(source, keySelector, comparer, cancellationToken);
        public static ValueTask<ILookup<TKey, TElement>> ToLookupAwaitWithCancellationAsync<TSource, TKey, TElement>(this IAsyncEnumerable<TSource> source, Func<TSource, CancellationToken, ValueTask<TKey>> keySelector, Func<TSource, CancellationToken, ValueTask<TElement>> elementSelector, IEqualityComparer<TKey> comparer, CancellationToken cancellationToken = default) => ToLookupAwaitWithCancellationAsyncCore<TSource, TKey, TElement>(source, keySelector, elementSelector, comparer, cancellationToken);
        public static IAsyncEnumerable<TSource> WhereAwaitWithCancellation<TSource>(this IAsyncEnumerable<TSource> source, Func<TSource, CancellationToken, ValueTask<bool>> predicate) => WhereAwaitWithCancellationCore<TSource>(source, predicate);
        public static IAsyncEnumerable<TSource> WhereAwaitWithCancellation<TSource>(this IAsyncEnumerable<TSource> source, Func<TSource, int, CancellationToken, ValueTask<bool>> predicate) => WhereAwaitWithCancellationCore<TSource>(source, predicate);
        public static IAsyncEnumerable<TResult> ZipAwaitWithCancellation<TFirst, TSecond, TResult>(this IAsyncEnumerable<TFirst> first, IAsyncEnumerable<TSecond> second, Func<TFirst, TSecond, CancellationToken, ValueTask<TResult>> selector) => ZipAwaitWithCancellationCore<TFirst, TSecond, TResult>(first, second, selector);
#endif
#endif
    }
}

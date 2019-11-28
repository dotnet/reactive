// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the Apache 2.0 License.
// See the LICENSE file in the project root for more information. 

using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace System.Linq
{
    /// <summary>
    /// Represents a sorted async-enumerable sequence.
    /// </summary>
    /// <typeparam name="TElement">The type of the elements of the sequence.</typeparam>
    public interface IOrderedAsyncEnumerable<out TElement> : IAsyncEnumerable<TElement>
    {
        /// <summary>
        /// Performs a subsequent ordering on the elements of an ordered async-enumerable according to a key.
        /// </summary>
        /// <typeparam name="TKey">The type of the key produced by keySelector.</typeparam>
        /// <param name="keySelector">The function used to extract the key for each element.</param>
        /// <param name="comparer">The comparer used to compare keys for placement in the returned sequence.</param>
        /// <param name="descending">true to sort the elements in descending order; false to sort the elements in ascending order.</param>
        /// <returns>An ordered async-enumerable whose elements are sorted according to a key.</returns>
        IOrderedAsyncEnumerable<TElement> CreateOrderedEnumerable<TKey>(Func<TElement, TKey> keySelector, IComparer<TKey>? comparer, bool descending);

        /// <summary>
        /// Performs a subsequent ordering on the elements of an ordered async-enumerable according to a key provided via a ValueTask.
        /// </summary>
        /// <typeparam name="TKey">The type of the key produced by keySelector.</typeparam>
        /// <param name="keySelector">The function used to extract the key for each element as a ValueTask.</param>
        /// <param name="comparer">The comparer used to compare keys for placement in the returned sequence.</param>
        /// <param name="descending">true to sort the elements in descending order; false to sort the elements in ascending order.</param>
        /// <returns>An ordered async-enumerable whose elements are sorted according to a key.</returns>
        IOrderedAsyncEnumerable<TElement> CreateOrderedEnumerable<TKey>(Func<TElement, ValueTask<TKey>> keySelector, IComparer<TKey>? comparer, bool descending);

#if !NO_DEEP_CANCELLATION
        /// <summary>
        /// Performs a subsequent ordering on the elements of an ordered async-enumerable according to a key provided via a ValueTask.
        /// </summary>
        /// <typeparam name="TKey">The type of the key produced by keySelector.</typeparam>
        /// <param name="keySelector">The function used to extract the key for each element as a ValueTask supporting cancellation.</param>
        /// <param name="comparer">The comparer used to compare keys for placement in the returned sequence.</param>
        /// <param name="descending">true to sort the elements in descending order; false to sort the elements in ascending order.</param>
        /// <returns>An ordered async-enumerable whose elements are sorted according to a key.</returns>
        IOrderedAsyncEnumerable<TElement> CreateOrderedEnumerable<TKey>(Func<TElement, CancellationToken, ValueTask<TKey>> keySelector, IComparer<TKey>? comparer, bool descending);
#endif
    }
}

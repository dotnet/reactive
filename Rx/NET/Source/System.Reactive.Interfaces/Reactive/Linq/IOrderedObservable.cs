// Copyright (c) Microsoft Open Technologies, Inc. All rights reserved. See License.txt in the project root for license information.

using System.Collections.Generic;

namespace System.Reactive.Linq
{
    /// <summary>
    /// Represents a sorted observable sequence.
    /// </summary>
    /// <typeparam name="T">
    /// The type of the data in the data source.
    /// This type parameter is covariant. That is, you can use either the type you specified or any type that is more derived. For more information about covariance and contravariance, see Covariance and Contravariance in Generics.
    /// </typeparam>
#if !NO_VARIANCE
    public interface IOrderedObservable<out T> : IObservable<T>
#else
    public interface IOrderedObservable<T> : IObservable<T>
#endif
    {
        /// <summary>
        /// Performs a subsequent ordering on the elements of an <see cref="IOrderedObservable{T}"/> according to a key.
        /// </summary>
        /// <typeparam name="TKey">The type of the key produced by <paramref name="keySelector"/>.</typeparam>
        /// <param name="keySelector">The function used to extract the key for each element.</param>
        /// <param name="comparer">The <see cref="IComparer{TKey}"/> used to compare keys for placement in the returned sequence.</param>
        /// <param name="descending"><see langword="True"/> to sort the elements in descending order; <see langword="false"/> to sort the elements in ascending order.</param>
        /// <returns>An <see cref="IOrderedObservable{T}"/> whose elements are sorted according to a key.</returns>
        IOrderedObservable<T> CreateOrderedObservable<TKey>(Func<T, TKey> keySelector, IComparer<TKey> comparer, bool descending);

        /// <summary>
        /// Performs a subsequent ordering on the elements of an <see cref="IOrderedObservable{T}"/> according to other observable sequences.
        /// </summary>
        /// <typeparam name="TOther">The type of the elements in the observable returned by <paramref name="timeSelector"/>.</typeparam>
        /// <param name="timeSelector">A function that returns an observable for an element indicating the time at which that element should appear in the ordering.</param>
        /// <param name="descending"><see langword="True"/> to sort the elements in descending order; <see langword="false"/> to sort the elements in ascending order.</param>
        /// <returns>An <see cref="IOrderedObservable{T}"/> whose elements are sorted according to the times at which corresponding observable sequences produce their first notification or complete.</returns>
        IOrderedObservable<T> CreateOrderedObservable<TOther>(Func<T, IObservable<TOther>> timeSelector, bool descending);
    }
}
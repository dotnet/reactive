// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the Apache 2.0 License.
// See the LICENSE file in the project root for more information. 

namespace System.Reactive.Linq
{
    /// <summary>
    /// Represents an observable sequence of elements that have a common key.
    /// </summary>
    /// <typeparam name="TKey">
    /// The type of the key shared by all elements in the group.
    /// This type parameter is covariant. That is, you can use either the type you specified or any type that is more derived. For more information about covariance and contravariance, see Covariance and Contravariance in Generics.
    /// </typeparam>
    /// <typeparam name="TElement">
    /// The type of the elements in the group.
    /// This type parameter is covariant. That is, you can use either the type you specified or any type that is more derived. For more information about covariance and contravariance, see Covariance and Contravariance in Generics.
    /// </typeparam>
    public interface IGroupedObservable<out TKey, out TElement> : IObservable<TElement>
    {
        /// <summary>
        /// Gets the common key.
        /// </summary>
        TKey Key { get; }
    }
}

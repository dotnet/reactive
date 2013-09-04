// Copyright (c) Microsoft Open Technologies, Inc. All rights reserved. See License.txt in the project root for license information.

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
#if !NO_VARIANCE
    public interface IGroupedObservable<out TKey, out TElement> : IObservable<TElement>
#else
    public interface IGroupedObservable<TKey, TElement> : IObservable<TElement>
#endif
    {
        /// <summary>
        /// Gets the common key.
        /// </summary>
        TKey Key { get; }
    }
}

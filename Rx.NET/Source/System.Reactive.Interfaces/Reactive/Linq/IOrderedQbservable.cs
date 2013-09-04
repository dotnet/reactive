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
    public interface IOrderedQbservable<out T> : IQbservable<T>
#else
    public interface IOrderedQbservable<T> : IQbservable<T>
#endif
    {
    }
}
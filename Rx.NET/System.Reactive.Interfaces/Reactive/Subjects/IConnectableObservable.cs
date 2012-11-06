// Copyright (c) Microsoft Open Technologies, Inc. All rights reserved. See License.txt in the project root for license information.

namespace System.Reactive.Subjects
{
    /// <summary>
    /// Represents an observable wrapper that can be connected and disconnected from its underlying observable sequence.
    /// </summary>
    /// <typeparam name="T">
    /// The type of the elements in the sequence.
    /// This type parameter is covariant. That is, you can use either the type you specified or any type that is more derived. For more information about covariance and contravariance, see Covariance and Contravariance in Generics.
    /// </typeparam>
#if !NO_VARIANCE
    public interface IConnectableObservable<out T> : IObservable<T>
#else
    public interface IConnectableObservable<T> : IObservable<T>
#endif
    {
        /// <summary>
        /// Connects the observable wrapper to its source. All subscribed observers will receive values from the underlying observable sequence as long as the connection is established.
        /// </summary>
        /// <returns>Disposable used to disconnect the observable wrapper from its source, causing subscribed observer to stop receiving values from the underlying observable sequence.</returns>
        IDisposable Connect();
    }
}

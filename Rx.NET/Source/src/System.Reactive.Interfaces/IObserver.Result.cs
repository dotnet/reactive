// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the Apache 2.0 License.
// See the LICENSE file in the project root for more information. 

namespace System.Reactive
{
    /// <summary>
    /// Provides a mechanism for receiving push-based notifications and returning a response.
    /// </summary>
    /// <typeparam name="TValue">
    /// The type of the elements received by the observer.
    /// This type parameter is contravariant. That is, you can use either the type you specified or any type that is less derived. For more information about covariance and contravariance, see Covariance and Contravariance in Generics.
    /// </typeparam>
    /// <typeparam name="TResult">
    /// The type of the result returned from the observer's notification handlers.
    /// This type parameter is covariant. That is, you can use either the type you specified or any type that is more derived. For more information about covariance and contravariance, see Covariance and Contravariance in Generics.
    /// </typeparam>
    public interface IObserver<in TValue, out TResult>
    {
        /// <summary>
        /// Notifies the observer of a new element in the sequence.
        /// </summary>
        /// <param name="value">The new element in the sequence.</param>
        /// <returns>Result returned upon observation of a new element.</returns>
        TResult OnNext(TValue value);

        /// <summary>
        /// Notifies the observer that an exception has occurred.
        /// </summary>
        /// <param name="exception">The exception that occurred.</param>
        /// <returns>Result returned upon observation of an error.</returns>
        TResult OnError(Exception exception);

        /// <summary>
        /// Notifies the observer of the end of the sequence.
        /// </summary>
        /// <returns>Result returned upon observation of the sequence completion.</returns>
        TResult OnCompleted();
    }
}
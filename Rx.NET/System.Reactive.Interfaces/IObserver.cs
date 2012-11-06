// Copyright (c) Microsoft Open Technologies, Inc. All rights reserved. See License.txt in the project root for license information.

#if NO_RXINTERFACES
namespace System
{
    /// <summary>
    /// Supports push-style iteration over an observable sequence.
    /// </summary>
#if !NO_VARIANCE
    public interface IObserver<in T>
#else
    public interface IObserver<T>
#endif
    {
        /// <summary>
        /// Notifies the observer of a new element in the sequence.
        /// </summary>
        /// <param name="value">Next element in the sequence.</param>
        void OnNext(T value);

        /// <summary>
        /// Notifies the observer that an exception has occurred.
        /// </summary>
        /// <param name="error">The error that has occurred.</param>
        void OnError(Exception error);

        /// <summary>
        /// Notifies the observer of the end of the sequence.
        /// </summary>
        void OnCompleted();
    }
}
#endif
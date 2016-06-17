// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the Apache 2.0 License.
// See the LICENSE file in the project root for more information. 

#if NO_RXINTERFACES
namespace System
{
    /// <summary>
    /// Represents a push-style collection.
    /// </summary>
#if !NO_VARIANCE
    public interface IObservable<out T>
#else
    public interface IObservable<T>
#endif
    {
        /// <summary>
        /// Subscribes an observer to the observable sequence.
        /// </summary>
        IDisposable Subscribe(IObserver<T> observer);
    }
}
#elif !WINDOWSPHONE7 // TypeForwardedTo is not present on Windows Phone 7 so we can't really target
[assembly: System.Runtime.CompilerServices.TypeForwardedTo(typeof(System.IObservable<>))]
#endif


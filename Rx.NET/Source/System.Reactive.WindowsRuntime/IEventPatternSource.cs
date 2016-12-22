// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the Apache 2.0 License.
// See the LICENSE file in the project root for more information. 

#if HAS_WINRT
using Windows.Foundation;

namespace System.Reactive
{
    /// <summary>
    /// Represents a data stream signaling its elements by means of an event.
    /// </summary>
    /// <typeparam name="TSender">Sender type.</typeparam>
    /// <typeparam name="TEventArgs">Event arguments type.</typeparam>
    [CLSCompliant(false)]
    public interface IEventPatternSource<TSender, TEventArgs>
    {
        /// <summary>
        /// Event signaling the next element in the data stream.
        /// </summary>
        event TypedEventHandler<TSender, TEventArgs> OnNext;
    }
}
#endif
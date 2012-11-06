// Copyright (c) Microsoft Open Technologies, Inc. All rights reserved. See License.txt in the project root for license information.

#if HAS_WINRT
using Windows.Foundation;

namespace System.Reactive
{
    /// <summary>
    /// Represents a data stream signaling its elements by means of an event.
    /// </summary>
    /// <typeparam name="TSender">Sender type.</typeparam>
    /// <typeparam name="TEventArgs">Event arguments type.</typeparam>
    public interface IEventPatternSource<TSender, TEventArgs>
    {
        /// <summary>
        /// Event signaling the next element in the data stream.
        /// </summary>
        event TypedEventHandler<TSender, TEventArgs> OnNext;
    }
}
#endif
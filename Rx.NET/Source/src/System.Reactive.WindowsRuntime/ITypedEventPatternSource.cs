﻿// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information. 

using Windows.Foundation;

namespace System.Reactive.WindowsRuntime
{
    /// <summary>
    /// Represents a data stream signaling its elements by means of a Windows Runtime
    /// <see cref="TypedEventHandler{TSender, TResult}"/>.
    /// </summary>
    /// <typeparam name="TSender">Sender type.</typeparam>
    /// <typeparam name="TEventArgs">Event arguments type.</typeparam>
    [CLSCompliant(false)]
    public interface ITypedEventPatternSource<TSender, TEventArgs>
    {
        /// <summary>
        /// Event signaling the next element in the data stream.
        /// </summary>
#pragma warning disable CA1003 // (Use generic EventHandler.) The use of the Windows.Foundation handler type is by design
        event TypedEventHandler<TSender, TEventArgs> OnNext;
#pragma warning restore CA1003
    }
}

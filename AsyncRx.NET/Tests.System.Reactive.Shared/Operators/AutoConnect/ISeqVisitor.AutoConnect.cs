// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information. 

namespace Tests.System.Reactive.Shared;

public partial interface ISeqVisitor
{
    /// <summary>
    /// Materializes an <see cref="AutoConnectSeq{T}"/> as the target's own
    /// <c>AutoConnect(minObservers, onConnect)</c>.
    /// </summary>
    /// <remarks>
    /// Built by
    /// <see cref="AutoConnectExtensions.AutoConnect{T}(ConnectableSeq{T}, int, Action{IAsyncDisposable})"/>.
    /// </remarks>
    Realized<Seq<T>> AutoConnect<T>(AutoConnectSeq<T> seq);
}

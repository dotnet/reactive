// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information. 

namespace Tests.System.Reactive.Shared;

/// <summary>The <c>AutoConnect</c> operator, as an extension method on <see cref="ConnectableSeq{T}"/>.</summary>
/// <remarks>
/// Where an Rx.NET test writes <c>xs.Replay().AutoConnect(2)</c>, a shared scenario writes the
/// same over a <see cref="ConnectableSeq{T}"/>; Rx.NET's one overload, with its optional
/// parameters, so the same call resolves the same way on both sides. The one node, in this
/// folder, is materialized by a target through
/// <see cref="ISeqVisitor.AutoConnect{T}(AutoConnectSeq{T})"/>.
/// </remarks>
public static class AutoConnectExtensions
{
    /// <summary>Describes <c>source.AutoConnect(minObservers, onConnect)</c>.</summary>
    /// <typeparam name="T">The type of the elements in the source sequence.</typeparam>
    /// <param name="source">Connectable observable sequence.</param>
    /// <param name="minObservers">
    /// The number of observers required to subscribe before the connection to source happens,
    /// non-positive value will trigger an immediate subscription.
    /// </param>
    /// <param name="onConnect">If not null, the connection's disposable is provided to it.</param>
    /// <remarks>
    /// Builds an <see cref="AutoConnectSeq{T}"/>, which each target materializes through
    /// <see cref="ISeqVisitor.AutoConnect{T}(AutoConnectSeq{T})"/>.
    /// </remarks>
    public static Seq<T> AutoConnect<T>(
        this ConnectableSeq<T> source,
        int minObservers = 1,
        Action<IAsyncDisposable>? onConnect = null) =>
        new AutoConnectSeq<T>(source, minObservers, onConnect);
}

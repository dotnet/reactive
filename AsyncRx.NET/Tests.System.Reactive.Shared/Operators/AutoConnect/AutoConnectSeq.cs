// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information. 

namespace Tests.System.Reactive.Shared;

/// <summary>Describes <c>source.AutoConnect(minObservers, onConnect)</c>.</summary>
/// <remarks>
/// Built by
/// <see cref="AutoConnectExtensions.AutoConnect{T}(ConnectableSeq{T}, int, Action{IAsyncDisposable})"/>;
/// materialized by each target through
/// <see cref="ISeqVisitor.AutoConnect{T}(AutoConnectSeq{T})"/>. The connection handed to
/// <see cref="OnConnect"/> is the kit's asynchronous disposable; the Rx.NET target wraps its
/// own.
/// </remarks>
public sealed class AutoConnectSeq<T>(
    ConnectableSeq<T> source,
    int minObservers,
    Action<IAsyncDisposable>? onConnect)
    : Seq<T>
{
    /// <summary>Connectable observable sequence.</summary>
    public ConnectableSeq<T> Source => source;

    /// <summary>
    /// The number of observers required to subscribe before the connection to source happens,
    /// non-positive value will trigger an immediate subscription.
    /// </summary>
    public int MinObservers => minObservers;

    /// <summary>If not null, the connection's disposable is provided to it.</summary>
    public Action<IAsyncDisposable>? OnConnect => onConnect;

    /// <inheritdoc/>
    protected override Realized<Seq<T>> AcceptCore(ISeqVisitor visitor) =>
        visitor.AutoConnect(this);

    /// <inheritdoc/>
    public override string ToString() =>
        onConnect is null
            ? $"{source}.AutoConnect({minObservers})"
            : $"{source}.AutoConnect({minObservers}, onConnect)";
}

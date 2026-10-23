// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information. 

namespace Tests.System.Reactive.Shared;

/// <summary>Describes <c>source.RefCount(disconnectDelay)</c>.</summary>
/// <remarks>
/// Built by <see cref="RefCountExtensions.RefCount{T}(ConnectableSeq{T}, TimeSpan)"/>;
/// materialized by each target through <see cref="ISeqVisitor.RefCountDelay{T}(RefCountDelaySeq{T})"/>.
/// </remarks>
public sealed class RefCountDelaySeq<T>(ConnectableSeq<T> source, TimeSpan disconnectDelay) : Seq<T>
{
    /// <summary>Connectable observable sequence.</summary>
    public ConnectableSeq<T> Source => source;

    /// <summary>
    /// The time to wait before disconnecting after all observers have unsubscribed.
    /// </summary>
    public TimeSpan DisconnectDelay => disconnectDelay;

    /// <inheritdoc/>
    protected override Realized<Seq<T>> AcceptCore(ISeqVisitor visitor) => visitor.RefCountDelay(this);

    /// <inheritdoc/>
    public override string ToString() => $"{source}.RefCount({disconnectDelay.Ticks} ticks)";
}

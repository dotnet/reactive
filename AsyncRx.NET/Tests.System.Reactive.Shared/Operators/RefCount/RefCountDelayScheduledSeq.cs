// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information. 

namespace Tests.System.Reactive.Shared;

/// <summary>Describes <c>source.RefCount(disconnectDelay, scheduler)</c>.</summary>
/// <remarks>
/// Built by <see cref="RefCountExtensions.RefCount{T}(ConnectableSeq{T}, TimeSpan, SchedulerRef)"/>;
/// materialized by each target through <see cref="ISeqVisitor.RefCountDelayScheduled{T}(RefCountDelayScheduledSeq{T})"/>.
/// </remarks>
public sealed class RefCountDelayScheduledSeq<T>(ConnectableSeq<T> source, TimeSpan disconnectDelay, SchedulerRef scheduler) : Seq<T>
{
    /// <summary>Connectable observable sequence.</summary>
    public ConnectableSeq<T> Source => source;

    /// <summary>
    /// The time to wait before disconnecting after all observers have unsubscribed.
    /// </summary>
    public TimeSpan DisconnectDelay => disconnectDelay;

    /// <summary>The scheduler to use for delayed unsubscription.</summary>
    public SchedulerRef Scheduler => scheduler;

    /// <inheritdoc/>
    protected override Realized<Seq<T>> AcceptCore(ISeqVisitor visitor) => visitor.RefCountDelayScheduled(this);

    /// <inheritdoc/>
    public override string ToString() => $"{source}.RefCount({disconnectDelay.Ticks} ticks, {scheduler})";
}

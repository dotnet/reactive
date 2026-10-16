// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information. 

namespace Tests.System.Reactive.Shared;

/// <summary>Describes <c>source.RefCount(minObservers, disconnectDelay, scheduler)</c>.</summary>
/// <remarks>
/// Built by <see cref="RefCountExtensions.RefCount{T}(ConnectableSeq{T}, int, TimeSpan, SchedulerRef)"/>;
/// materialized by each target through <see cref="ISeqVisitor.RefCountMinObserversDelayScheduled{T}(RefCountMinObserversDelayScheduledSeq{T})"/>.
/// </remarks>
public sealed class RefCountMinObserversDelayScheduledSeq<T>(ConnectableSeq<T> source, int minObservers, TimeSpan disconnectDelay, SchedulerRef scheduler) : Seq<T>
{
    /// <summary>Connectable observable sequence.</summary>
    public ConnectableSeq<T> Source => source;

    /// <summary>The minimum number of observers required to connect.</summary>
    public int MinObservers => minObservers;

    /// <summary>
    /// The time to wait before disconnecting after all observers have unsubscribed.
    /// </summary>
    public TimeSpan DisconnectDelay => disconnectDelay;

    /// <summary>The scheduler to use for delayed unsubscription.</summary>
    public SchedulerRef Scheduler => scheduler;

    /// <inheritdoc/>
    protected override Realized<Seq<T>> AcceptCore(ISeqVisitor visitor) => visitor.RefCountMinObserversDelayScheduled(this);

    /// <inheritdoc/>
    public override string ToString() => $"{source}.RefCount({minObservers}, {disconnectDelay.Ticks} ticks, {scheduler})";
}

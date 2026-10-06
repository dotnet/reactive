// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information. 

namespace Tests.System.Reactive.Shared;

/// <summary>Describes <c>source.Replay(bufferSize, scheduler)</c>.</summary>
/// <remarks>
/// Built by
/// <see cref="ReplayExtensions.Replay{T}(Seq{T}, int, SchedulerRef)"/>;
/// materialized by each target through
/// <see cref="ISeqVisitor.ReplayCountScheduled{T}(ReplayCountScheduledSeq{T})"/>.
/// </remarks>
public sealed class ReplayCountScheduledSeq<T>(
    Seq<T> source,
    int bufferSize,
    SchedulerRef scheduler) : ConnectableSeq<T>
{
    /// <summary>
    /// Source sequence whose elements will be multicasted through a single shared subscription.
    /// </summary>
    public Seq<T> Source => source;

    /// <summary>Maximum element count of the replay buffer.</summary>
    public int BufferSize => bufferSize;

    /// <summary>Scheduler where connected observers will be invoked on.</summary>
    public SchedulerRef Scheduler => scheduler;

    /// <inheritdoc/>
    protected override Realized<Seq<T>> AcceptCore(ISeqVisitor visitor) =>
        visitor.ReplayCountScheduled(this);

    /// <inheritdoc/>
    public override string ToString() => $"{source}.Replay({bufferSize}, {scheduler})";
}

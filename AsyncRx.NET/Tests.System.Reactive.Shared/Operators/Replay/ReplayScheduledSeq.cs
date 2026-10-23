// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information. 

namespace Tests.System.Reactive.Shared;

/// <summary>Describes <c>source.Replay(scheduler)</c>.</summary>
/// <remarks>
/// Built by
/// <see cref="ReplayExtensions.Replay{T}(Seq{T}, SchedulerRef)"/>;
/// materialized by each target through
/// <see cref="ISeqVisitor.ReplayScheduled{T}(ReplayScheduledSeq{T})"/>.
/// </remarks>
public sealed class ReplayScheduledSeq<T>(Seq<T> source, SchedulerRef scheduler) : ConnectableSeq<T>
{
    /// <summary>
    /// Source sequence whose elements will be multicasted through a single shared subscription.
    /// </summary>
    public Seq<T> Source => source;

    /// <summary>Scheduler where connected observers will be invoked on.</summary>
    public SchedulerRef Scheduler => scheduler;

    /// <inheritdoc/>
    protected override Realized<Seq<T>> AcceptCore(ISeqVisitor visitor) =>
        visitor.ReplayScheduled(this);

    /// <inheritdoc/>
    public override string ToString() => $"{source}.Replay({scheduler})";
}

// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information. 

namespace Tests.System.Reactive.Shared;

/// <summary>Describes <c>source.Replay(window, scheduler)</c>.</summary>
/// <remarks>
/// Built by
/// <see cref="ReplayExtensions.Replay{T}(Seq{T}, TimeSpan, SchedulerRef)"/>;
/// materialized by each target through
/// <see cref="ISeqVisitor.ReplayTimeScheduled{T}(ReplayTimeScheduledSeq{T})"/>.
/// </remarks>
public sealed class ReplayTimeScheduledSeq<T>(
    Seq<T> source,
    TimeSpan window,
    SchedulerRef scheduler) : ConnectableSeq<T>
{
    /// <summary>
    /// Source sequence whose elements will be multicasted through a single shared subscription.
    /// </summary>
    public Seq<T> Source => source;

    /// <summary>Maximum time length of the replay buffer.</summary>
    public TimeSpan Window => window;

    /// <summary>Scheduler where connected observers will be invoked on.</summary>
    public SchedulerRef Scheduler => scheduler;

    /// <inheritdoc/>
    protected override Realized<Seq<T>> AcceptCore(ISeqVisitor visitor) =>
        visitor.ReplayTimeScheduled(this);

    /// <inheritdoc/>
    public override string ToString() => $"{source}.Replay({window}, {scheduler})";
}

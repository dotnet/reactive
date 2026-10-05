// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information. 

namespace Tests.System.Reactive.Shared;

/// <summary>Describes <c>source.Take(count, scheduler)</c>.</summary>
/// <remarks>
/// Built by <see cref="TakeExtensions.Take{T}(Seq{T}, int, SchedulerRef)"/>;
/// materialized by each target through
/// <see cref="ISeqVisitor.TakeScheduled{T}(TakeScheduledSeq{T})"/>.
/// </remarks>
public sealed class TakeScheduledSeq<T>(Seq<T> source, int count, SchedulerRef scheduler) : Seq<T>
{
    /// <summary>The sequence to take elements from.</summary>
    public Seq<T> Source => source;

    /// <summary>The number of elements to return.</summary>
    public int Count => count;

    /// <summary>
    /// Scheduler used to produce an OnCompleted message in case <c>count</c> is set to 0.
    /// </summary>
    public SchedulerRef Scheduler => scheduler;

    /// <inheritdoc/>
    protected override Realized<Seq<T>> AcceptCore(ISeqVisitor visitor) => visitor.TakeScheduled(this);

    /// <inheritdoc/>
    public override string ToString() => $"{source}.Take({count}, {scheduler})";
}

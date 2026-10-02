// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information. 

namespace Tests.System.Reactive.Shared;

/// <summary>Describes <c>Seq.Range(start, count, scheduler)</c>.</summary>
/// <remarks>
/// Built by <see cref="Seq.Range(int, int, SchedulerRef)"/>; materialized by each target through
/// <see cref="ISeqVisitor.RangeScheduled(RangeScheduledSeq)"/>.
/// </remarks>
public sealed class RangeScheduledSeq(int start, int count, SchedulerRef scheduler) : Seq<int>
{
    /// <summary>The value of the first integer in the sequence.</summary>
    public int Start => start;

    /// <summary>The number of sequential integers to generate.</summary>
    public int Count => count;

    /// <summary>Scheduler to run the generator loop on.</summary>
    public SchedulerRef Scheduler => scheduler;

    /// <inheritdoc/>
    public override Realized<Seq<int>> Accept(ISeqVisitor visitor) => visitor.RangeScheduled(this);

    /// <inheritdoc/>
    public override string ToString() => $"Seq.Range({start}, {count}, {scheduler})";
}

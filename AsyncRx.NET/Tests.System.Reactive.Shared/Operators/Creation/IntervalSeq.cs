// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information. 

namespace Tests.System.Reactive.Shared;

/// <summary>Describes <c>Seq.Interval(period, scheduler)</c>.</summary>
/// <remarks>
/// Built by <see cref="Seq.Interval(TimeSpan, SchedulerRef)"/>;
/// materialized by each target through <see cref="ISeqVisitor.Interval(IntervalSeq)"/>.
/// </remarks>
public sealed class IntervalSeq(TimeSpan period, SchedulerRef scheduler) : Seq<long>
{
    /// <summary>Period for producing the values in the resulting sequence.</summary>
    /// <remarks>
    /// If this value is equal to TimeSpan.Zero, the timer will recur as fast as possible.
    /// </remarks>
    public TimeSpan Period => period;

    /// <summary>Scheduler to run the timer on.</summary>
    public SchedulerRef Scheduler => scheduler;

    /// <inheritdoc/>
    protected override Realized<Seq<long>> AcceptCore(ISeqVisitor visitor) =>
        visitor.Interval(this);

    /// <inheritdoc/>
    public override string ToString() => $"Seq.Interval({period.Ticks} ticks, {scheduler})";
}

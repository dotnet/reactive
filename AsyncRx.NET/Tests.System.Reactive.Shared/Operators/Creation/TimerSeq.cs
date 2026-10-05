// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information. 

namespace Tests.System.Reactive.Shared;

/// <summary>Describes <c>Seq.Timer(dueTime, scheduler)</c>.</summary>
/// <remarks>
/// Built by <see cref="Seq.Timer(TimeSpan, SchedulerRef)"/>;
/// materialized by each target through <see cref="ISeqVisitor.Timer(TimerSeq)"/>.
/// </remarks>
public sealed class TimerSeq(TimeSpan dueTime, SchedulerRef scheduler) : Seq<long>
{
    /// <summary>Relative time at which to produce the value.</summary>
    /// <remarks>
    /// If this value is less than or equal to TimeSpan.Zero, the timer will fire as soon as
    /// possible.
    /// </remarks>
    public TimeSpan DueTime => dueTime;

    /// <summary>Scheduler to run the timer on.</summary>
    public SchedulerRef Scheduler => scheduler;

    /// <inheritdoc/>
    protected override Realized<Seq<long>> AcceptCore(ISeqVisitor visitor) => visitor.Timer(this);

    /// <inheritdoc/>
    public override string ToString() => $"Seq.Timer({dueTime.Ticks} ticks, {scheduler})";
}

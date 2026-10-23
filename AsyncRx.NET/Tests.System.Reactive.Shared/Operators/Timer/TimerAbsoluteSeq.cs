// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information. 

namespace Tests.System.Reactive.Shared;

/// <summary>Describes <c>Seq.Timer(dueTime[, scheduler])</c> with an absolute due time.</summary>
/// <remarks>
/// Built by <see cref="Seq.Timer(DateTimeOffset)"/> or
/// <see cref="Seq.Timer(DateTimeOffset, SchedulerRef)"/>; materialized by each target through
/// <see cref="ISeqVisitor.TimerAbsolute(TimerAbsoluteSeq)"/>.
/// </remarks>
public sealed class TimerAbsoluteSeq(DateTimeOffset dueTime, SchedulerRef? scheduler) : Seq<long>
{
    /// <summary>Absolute time at which to produce the value.</summary>
    /// <remarks>
    /// If this value is less than or equal to the current time, the timer will fire as soon as
    /// possible.
    /// </remarks>
    public DateTimeOffset DueTime => dueTime;

    /// <summary>Scheduler to run the timer on, or null for the target's default.</summary>
    public SchedulerRef? Scheduler => scheduler;

    /// <inheritdoc/>
    protected override Realized<Seq<long>> AcceptCore(ISeqVisitor visitor) =>
        visitor.TimerAbsolute(this);

    /// <inheritdoc/>
    public override string ToString() =>
        scheduler is null
            ? $"Seq.Timer(at {dueTime.Ticks} ticks)"
            : $"Seq.Timer(at {dueTime.Ticks} ticks, {scheduler})";
}

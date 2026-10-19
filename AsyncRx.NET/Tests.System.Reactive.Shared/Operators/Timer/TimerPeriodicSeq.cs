// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information. 

namespace Tests.System.Reactive.Shared;

/// <summary>
/// Describes <c>Seq.Timer(dueTime, period[, scheduler])</c> with a relative due time.
/// </summary>
/// <remarks>
/// Built by <see cref="Seq.Timer(TimeSpan, TimeSpan)"/> or
/// <see cref="Seq.Timer(TimeSpan, TimeSpan, SchedulerRef)"/>; materialized by each target through
/// <see cref="ISeqVisitor.TimerPeriodic(TimerPeriodicSeq)"/>.
/// </remarks>
public sealed class TimerPeriodicSeq(TimeSpan dueTime, TimeSpan period, SchedulerRef? scheduler)
    : Seq<long>
{
    /// <summary>Relative time at which to produce the first value.</summary>
    /// <remarks>
    /// If this value is less than or equal to TimeSpan.Zero, the timer will fire as soon as
    /// possible.
    /// </remarks>
    public TimeSpan DueTime => dueTime;

    /// <summary>Period to produce subsequent values.</summary>
    /// <remarks>
    /// If this value is equal to TimeSpan.Zero, the timer will recur as fast as possible.
    /// </remarks>
    public TimeSpan Period => period;

    /// <summary>Scheduler to run timers on, or null for the target's default.</summary>
    public SchedulerRef? Scheduler => scheduler;

    /// <inheritdoc/>
    protected override Realized<Seq<long>> AcceptCore(ISeqVisitor visitor) =>
        visitor.TimerPeriodic(this);

    /// <inheritdoc/>
    public override string ToString() =>
        scheduler is null
            ? $"Seq.Timer({dueTime.Ticks} ticks, every {period.Ticks} ticks)"
            : $"Seq.Timer({dueTime.Ticks} ticks, every {period.Ticks} ticks, {scheduler})";
}

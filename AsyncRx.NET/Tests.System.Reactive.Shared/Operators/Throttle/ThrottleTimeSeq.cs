// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information. 

namespace Tests.System.Reactive.Shared;

/// <summary>Describes <c>source.Throttle(dueTime[, scheduler])</c>.</summary>
/// <remarks>
/// Built by <see cref="ThrottleExtensions.Throttle{T}(Seq{T}, TimeSpan, SchedulerRef)"/> or
/// <see cref="ThrottleExtensions.Throttle{T}(Seq{T}, TimeSpan)"/>; materialized by each target
/// through <see cref="ISeqVisitor.ThrottleTime{T}(ThrottleTimeSeq{T})"/>.
/// </remarks>
public sealed class ThrottleTimeSeq<T>(Seq<T> source, TimeSpan dueTime, SchedulerRef? scheduler)
    : Seq<T>
{
    /// <summary>Source sequence to throttle.</summary>
    public Seq<T> Source => source;

    /// <summary>Throttling duration for each element.</summary>
    public TimeSpan DueTime => dueTime;

    /// <summary>
    /// Scheduler to run the throttle timers on, or null for the target's default.
    /// </summary>
    public SchedulerRef? Scheduler => scheduler;

    /// <inheritdoc/>
    protected override Realized<Seq<T>> AcceptCore(ISeqVisitor visitor) =>
        visitor.ThrottleTime(this);

    /// <inheritdoc/>
    public override string ToString() =>
        scheduler is null
            ? $"{source}.Throttle({dueTime.Ticks} ticks)"
            : $"{source}.Throttle({dueTime.Ticks} ticks, {scheduler})";
}

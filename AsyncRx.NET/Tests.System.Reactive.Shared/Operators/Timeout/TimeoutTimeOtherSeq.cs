// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information. 

namespace Tests.System.Reactive.Shared;

/// <summary>
/// Describes <c>source.Timeout(dueTime, other[, scheduler])</c> with a relative due time.
/// </summary>
/// <remarks>
/// Built by
/// <see cref="TimeoutExtensions.Timeout{T}(Seq{T}, TimeSpan, Seq{T}, SchedulerRef)"/> or, with no
/// scheduler, by <see cref="TimeoutExtensions.Timeout{T}(Seq{T}, TimeSpan, Seq{T})"/>;
/// materialized by each target through
/// <see cref="ISeqVisitor.TimeoutTimeOther{T}(TimeoutTimeOtherSeq{T})"/>.
/// </remarks>
public sealed class TimeoutTimeOtherSeq<T>(
    Seq<T> source,
    TimeSpan dueTime,
    Seq<T> other,
    SchedulerRef? scheduler)
    : Seq<T>
{
    /// <summary>Source sequence to perform a timeout for.</summary>
    public Seq<T> Source => source;

    /// <summary>Maximum duration between values before a timeout occurs.</summary>
    public TimeSpan DueTime => dueTime;

    /// <summary>Sequence to return in case of a timeout.</summary>
    public Seq<T> Other => other;

    /// <summary>Scheduler to run the timeout timers on, or null for the target's default.</summary>
    public SchedulerRef? Scheduler => scheduler;

    /// <inheritdoc/>
    protected override Realized<Seq<T>> AcceptCore(ISeqVisitor visitor) =>
        visitor.TimeoutTimeOther(this);

    /// <inheritdoc/>
    public override string ToString() =>
        scheduler is null
            ? $"{source}.Timeout({dueTime.Ticks} ticks, {other})"
            : $"{source}.Timeout({dueTime.Ticks} ticks, {other}, {scheduler})";
}

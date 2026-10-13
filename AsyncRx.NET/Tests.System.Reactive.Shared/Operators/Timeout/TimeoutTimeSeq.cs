// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information. 

namespace Tests.System.Reactive.Shared;

/// <summary>
/// Describes <c>source.Timeout(dueTime[, scheduler])</c> with a relative due time.
/// </summary>
/// <remarks>
/// Built by <see cref="TimeoutExtensions.Timeout{T}(Seq{T}, TimeSpan, SchedulerRef)"/> or, with no
/// scheduler, by <see cref="TimeoutExtensions.Timeout{T}(Seq{T}, TimeSpan)"/>;
/// materialized by each target through
/// <see cref="ISeqVisitor.TimeoutTime{T}(TimeoutTimeSeq{T})"/>.
/// </remarks>
public sealed class TimeoutTimeSeq<T>(
    Seq<T> source,
    TimeSpan dueTime,
    SchedulerRef? scheduler)
    : Seq<T>
{
    /// <summary>Source sequence to perform a timeout for.</summary>
    public Seq<T> Source => source;

    /// <summary>Maximum duration between values before a timeout occurs.</summary>
    public TimeSpan DueTime => dueTime;

    /// <summary>Scheduler to run the timeout timers on, or null for the target's default.</summary>
    public SchedulerRef? Scheduler => scheduler;

    /// <inheritdoc/>
    protected override Realized<Seq<T>> AcceptCore(ISeqVisitor visitor) =>
        visitor.TimeoutTime(this);

    /// <inheritdoc/>
    public override string ToString() =>
        scheduler is null
            ? $"{source}.Timeout({dueTime.Ticks} ticks)"
            : $"{source}.Timeout({dueTime.Ticks} ticks, {scheduler})";
}

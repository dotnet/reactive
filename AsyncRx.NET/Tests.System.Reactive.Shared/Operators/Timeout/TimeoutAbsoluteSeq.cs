// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information. 

namespace Tests.System.Reactive.Shared;

/// <summary>
/// Describes <c>source.Timeout(dueTime[, scheduler])</c> with an absolute due time.
/// </summary>
/// <remarks>
/// Built by <see cref="TimeoutExtensions.Timeout{T}(Seq{T}, DateTimeOffset, SchedulerRef)"/> or,
/// with no scheduler, by <see cref="TimeoutExtensions.Timeout{T}(Seq{T}, DateTimeOffset)"/>;
/// materialized by each target through
/// <see cref="ISeqVisitor.TimeoutAbsolute{T}(TimeoutAbsoluteSeq{T})"/>.
/// </remarks>
public sealed class TimeoutAbsoluteSeq<T>(
    Seq<T> source,
    DateTimeOffset dueTime,
    SchedulerRef? scheduler)
    : Seq<T>
{
    /// <summary>Source sequence to perform a timeout for.</summary>
    public Seq<T> Source => source;

    /// <summary>Time when a timeout occurs.</summary>
    /// <remarks>
    /// If this value is less than or equal to DateTimeOffset.UtcNow, the timeout occurs
    /// immediately.
    /// </remarks>
    public DateTimeOffset DueTime => dueTime;

    /// <summary>Scheduler to run the timeout timers on, or null for the target's default.</summary>
    public SchedulerRef? Scheduler => scheduler;

    /// <inheritdoc/>
    protected override Realized<Seq<T>> AcceptCore(ISeqVisitor visitor) =>
        visitor.TimeoutAbsolute(this);

    /// <inheritdoc/>
    public override string ToString() =>
        scheduler is null
            ? $"{source}.Timeout({dueTime.Ticks} ticks)"
            : $"{source}.Timeout({dueTime.Ticks} ticks, {scheduler})";
}

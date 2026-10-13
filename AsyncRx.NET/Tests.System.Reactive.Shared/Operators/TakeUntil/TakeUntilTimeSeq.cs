// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information. 

namespace Tests.System.Reactive.Shared;

/// <summary>Describes <c>source.TakeUntil(endTime[, scheduler])</c>.</summary>
/// <remarks>
/// Built by <see cref="TakeUntilExtensions.TakeUntil{T}(Seq{T}, DateTimeOffset, SchedulerRef)"/>
/// or, with no scheduler, by
/// <see cref="TakeUntilExtensions.TakeUntil{T}(Seq{T}, DateTimeOffset)"/>; materialized by each
/// target through <see cref="ISeqVisitor.TakeUntilTime{T}(TakeUntilTimeSeq{T})"/>.
/// </remarks>
public sealed class TakeUntilTimeSeq<T>(
    Seq<T> source,
    DateTimeOffset endTime,
    SchedulerRef? scheduler)
    : Seq<T>
{
    /// <summary>Source sequence to take elements from.</summary>
    public Seq<T> Source => source;

    /// <summary>Time to stop taking elements from the source sequence.</summary>
    /// <remarks>
    /// If this value is less than or equal to DateTimeOffset.UtcNow, the result stream will
    /// complete immediately.
    /// </remarks>
    public DateTimeOffset EndTime => endTime;

    /// <summary>Scheduler to run the timer on, or null for the target's default.</summary>
    public SchedulerRef? Scheduler => scheduler;

    /// <inheritdoc/>
    protected override Realized<Seq<T>> AcceptCore(ISeqVisitor visitor) =>
        visitor.TakeUntilTime(this);

    /// <inheritdoc/>
    public override string ToString() =>
        scheduler is null
            ? $"{source}.TakeUntil({endTime.Ticks} ticks)"
            : $"{source}.TakeUntil({endTime.Ticks} ticks, {scheduler})";
}

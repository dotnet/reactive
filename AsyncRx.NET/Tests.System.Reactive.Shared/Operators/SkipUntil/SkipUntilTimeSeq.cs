// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information. 

namespace Tests.System.Reactive.Shared;

/// <summary>Describes <c>source.SkipUntil(startTime[, scheduler])</c>.</summary>
/// <remarks>
/// Built by <see cref="SkipUntilExtensions.SkipUntil{T}(Seq{T}, DateTimeOffset, SchedulerRef)"/>
/// or, with no scheduler, by
/// <see cref="SkipUntilExtensions.SkipUntil{T}(Seq{T}, DateTimeOffset)"/>; materialized by each
/// target through <see cref="ISeqVisitor.SkipUntilTime{T}(SkipUntilTimeSeq{T})"/>.
/// </remarks>
public sealed class SkipUntilTimeSeq<T>(
    Seq<T> source,
    DateTimeOffset startTime,
    SchedulerRef? scheduler)
    : Seq<T>
{
    /// <summary>Source sequence to skip elements for.</summary>
    public Seq<T> Source => source;

    /// <summary>Time to start taking elements from the source sequence.</summary>
    /// <remarks>
    /// If this value is less than or equal to DateTimeOffset.UtcNow, no elements will be skipped.
    /// </remarks>
    public DateTimeOffset StartTime => startTime;

    /// <summary>Scheduler to run the timer on, or null for the target's default.</summary>
    public SchedulerRef? Scheduler => scheduler;

    /// <inheritdoc/>
    protected override Realized<Seq<T>> AcceptCore(ISeqVisitor visitor) =>
        visitor.SkipUntilTime(this);

    /// <inheritdoc/>
    public override string ToString() =>
        scheduler is null
            ? $"{source}.SkipUntil({startTime.Ticks} ticks)"
            : $"{source}.SkipUntil({startTime.Ticks} ticks, {scheduler})";
}

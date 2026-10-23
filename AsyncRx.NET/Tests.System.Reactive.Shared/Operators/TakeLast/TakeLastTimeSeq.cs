// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information. 

namespace Tests.System.Reactive.Shared;

/// <summary>Describes <c>source.TakeLast(duration[, scheduler])</c>.</summary>
/// <remarks>
/// Built by <see cref="TakeLastExtensions.TakeLast{T}(Seq{T}, TimeSpan, SchedulerRef)"/> or, with
/// no scheduler, by <see cref="TakeLastExtensions.TakeLast{T}(Seq{T}, TimeSpan)"/>; materialized
/// by each target through <see cref="ISeqVisitor.TakeLastTime{T}(TakeLastTimeSeq{T})"/>.
/// </remarks>
public sealed class TakeLastTimeSeq<T>(Seq<T> source, TimeSpan duration, SchedulerRef? scheduler)
    : Seq<T>
{
    /// <summary>Source sequence to take elements from.</summary>
    public Seq<T> Source => source;

    /// <summary>Duration for taking elements from the end of the sequence.</summary>
    public TimeSpan Duration => duration;

    /// <summary>Scheduler to run the timer on, or null for the target's default.</summary>
    public SchedulerRef? Scheduler => scheduler;

    /// <inheritdoc/>
    protected override Realized<Seq<T>> AcceptCore(ISeqVisitor visitor) =>
        visitor.TakeLastTime(this);

    /// <inheritdoc/>
    public override string ToString() =>
        scheduler is null
            ? $"{source}.TakeLast({duration.Ticks} ticks)"
            : $"{source}.TakeLast({duration.Ticks} ticks, {scheduler})";
}

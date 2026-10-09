// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information. 

namespace Tests.System.Reactive.Shared;

/// <summary>Describes <c>source.Delay(dueTime[, scheduler])</c> with a relative due time.</summary>
/// <remarks>
/// Built by <see cref="DelayExtensions.Delay{T}(Seq{T}, TimeSpan, SchedulerRef)"/> or, with no
/// scheduler, by <see cref="DelayExtensions.Delay{T}(Seq{T}, TimeSpan)"/>; materialized by each
/// target through <see cref="ISeqVisitor.DelayTime{T}(DelayTimeSeq{T})"/>.
/// </remarks>
public sealed class DelayTimeSeq<T>(Seq<T> source, TimeSpan dueTime, SchedulerRef? scheduler)
    : Seq<T>
{
    /// <summary>Source sequence to delay values for.</summary>
    public Seq<T> Source => source;

    /// <summary>Relative time by which to shift the observable sequence.</summary>
    /// <remarks>
    /// If this value is equal to TimeSpan.Zero, the scheduler will dispatch observer callbacks as
    /// soon as possible.
    /// </remarks>
    public TimeSpan DueTime => dueTime;

    /// <summary>Scheduler to run the delay timers on, or null for the target's default.</summary>
    public SchedulerRef? Scheduler => scheduler;

    /// <inheritdoc/>
    protected override Realized<Seq<T>> AcceptCore(ISeqVisitor visitor) => visitor.DelayTime(this);

    /// <inheritdoc/>
    public override string ToString() =>
        scheduler is null
            ? $"{source}.Delay({dueTime.Ticks} ticks)"
            : $"{source}.Delay({dueTime.Ticks} ticks, {scheduler})";
}

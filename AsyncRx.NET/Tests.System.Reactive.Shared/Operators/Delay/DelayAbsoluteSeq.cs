// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information. 

namespace Tests.System.Reactive.Shared;

/// <summary>Describes <c>source.Delay(dueTime, scheduler)</c> with an absolute due time.</summary>
/// <remarks>
/// Built by <see cref="DelayExtensions.Delay{T}(Seq{T}, DateTimeOffset, SchedulerRef)"/>;
/// materialized by each target through
/// <see cref="ISeqVisitor.DelayAbsolute{T}(DelayAbsoluteSeq{T})"/>.
/// </remarks>
public sealed class DelayAbsoluteSeq<T>(Seq<T> source, DateTimeOffset dueTime, SchedulerRef scheduler) : Seq<T>
{
    /// <summary>Source sequence to delay values for.</summary>
    public Seq<T> Source => source;

    /// <summary>Absolute time used to shift the observable sequence.</summary>
    /// <remarks>
    /// The relative time shift gets computed upon subscription. If this value is less than or
    /// equal to DateTimeOffset.UtcNow, the scheduler will dispatch observer callbacks as soon as
    /// possible.
    /// </remarks>
    public DateTimeOffset DueTime => dueTime;

    /// <summary>Scheduler to run the delay timers on.</summary>
    public SchedulerRef Scheduler => scheduler;

    /// <inheritdoc/>
    protected override Realized<Seq<T>> AcceptCore(ISeqVisitor visitor) => visitor.DelayAbsolute(this);

    /// <inheritdoc/>
    public override string ToString() => $"{source}.Delay(@{dueTime.Ticks}, {scheduler})";
}

// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information. 

namespace Tests.System.Reactive.Shared;

/// <summary>Describes <c>source.Take(duration, scheduler)</c>.</summary>
/// <remarks>
/// Built by <see cref="TakeExtensions.Take{T}(Seq{T}, TimeSpan, SchedulerRef)"/>;
/// materialized by each target through <see cref="ISeqVisitor.TakeTime{T}(TakeTimeSeq{T})"/>.
/// </remarks>
public sealed class TakeTimeSeq<T>(Seq<T> source, TimeSpan duration, SchedulerRef scheduler) : Seq<T>
{
    /// <summary>Source sequence to take elements from.</summary>
    public Seq<T> Source => source;

    /// <summary>Duration for taking elements from the start of the sequence.</summary>
    public TimeSpan Duration => duration;

    /// <summary>Scheduler to run the timer on.</summary>
    public SchedulerRef Scheduler => scheduler;

    /// <inheritdoc/>
    public override object Accept(ISeqVisitor visitor) => visitor.TakeTime(this);

    /// <inheritdoc/>
    public override string ToString() => $"{source}.Take({duration.Ticks} ticks, {scheduler})";
}

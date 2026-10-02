// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information. 

namespace Tests.System.Reactive.Shared;

/// <summary>Describes <c>source.Skip(duration, scheduler)</c>.</summary>
/// <remarks>
/// Built by <see cref="SkipExtensions.Skip{T}(Seq{T}, TimeSpan, SchedulerRef)"/>;
/// materialized by each target through <see cref="ISeqVisitor.SkipTime{T}(SkipTimeSeq{T})"/>.
/// </remarks>
public sealed class SkipTimeSeq<T>(Seq<T> source, TimeSpan duration, SchedulerRef scheduler) : Seq<T>
{
    /// <summary>Source sequence to skip elements for.</summary>
    public Seq<T> Source => source;

    /// <summary>Duration for skipping elements from the start of the sequence.</summary>
    public TimeSpan Duration => duration;

    /// <summary>Scheduler to run the timer on.</summary>
    public SchedulerRef Scheduler => scheduler;

    /// <inheritdoc/>
    public override Realized<Seq<T>> Accept(ISeqVisitor visitor) => visitor.SkipTime(this);

    /// <inheritdoc/>
    public override string ToString() => $"{source}.Skip({duration.Ticks} ticks, {scheduler})";
}

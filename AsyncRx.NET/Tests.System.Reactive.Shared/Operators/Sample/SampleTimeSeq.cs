// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information. 

namespace Tests.System.Reactive.Shared;

/// <summary>Describes <c>source.Sample(interval[, scheduler])</c>.</summary>
/// <remarks>
/// Built by <see cref="SampleExtensions.Sample{T}(Seq{T}, TimeSpan, SchedulerRef)"/> or
/// <see cref="SampleExtensions.Sample{T}(Seq{T}, TimeSpan)"/>; materialized by each target
/// through <see cref="ISeqVisitor.SampleTime{T}(SampleTimeSeq{T})"/>.
/// </remarks>
public sealed class SampleTimeSeq<T>(Seq<T> source, TimeSpan interval, SchedulerRef? scheduler)
    : Seq<T>
{
    /// <summary>Source sequence to sample.</summary>
    public Seq<T> Source => source;

    /// <summary>Interval at which to sample.</summary>
    /// <remarks>
    /// If this value is equal to TimeSpan.Zero, the scheduler will continuously sample the
    /// stream.
    /// </remarks>
    public TimeSpan Interval => interval;

    /// <summary>
    /// Scheduler to run the sampling timer on, or null for the target's default.
    /// </summary>
    public SchedulerRef? Scheduler => scheduler;

    /// <inheritdoc/>
    protected override Realized<Seq<T>> AcceptCore(ISeqVisitor visitor) =>
        visitor.SampleTime(this);

    /// <inheritdoc/>
    public override string ToString() =>
        scheduler is null
            ? $"{source}.Sample({interval.Ticks} ticks)"
            : $"{source}.Sample({interval.Ticks} ticks, {scheduler})";
}

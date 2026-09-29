// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information. 

namespace Tests.System.Reactive.Shared;

/// <summary>Describes <c>source.Window(timeSpan, timeShift, scheduler)</c>.</summary>
/// <remarks>
/// Built by <see cref="WindowExtensions.Window{T}(Seq{T}, TimeSpan, TimeSpan, SchedulerRef)"/>;
/// materialized by each target through
/// <see cref="ISeqVisitor.WindowTimeShift{T}(WindowTimeShiftSeq{T})"/>.
/// </remarks>
public sealed class WindowTimeShiftSeq<T>(Seq<T> source, TimeSpan timeSpan, TimeSpan timeShift, SchedulerRef scheduler) : Nested<T>
{
    /// <summary>Source sequence to produce windows over.</summary>
    public Seq<T> Source => source;

    /// <summary>Length of each window.</summary>
    public TimeSpan TimeSpan => timeSpan;

    /// <summary>Interval between creation of consecutive windows.</summary>
    public TimeSpan TimeShift => timeShift;

    /// <summary>Scheduler to run windowing timers on.</summary>
    public SchedulerRef Scheduler => scheduler;

    /// <inheritdoc/>
    public override RSeq<ISeq<T>> Accept(ISeqVisitor visitor) => new RSeqResult(visitor.WindowTimeShift(this));

    /// <inheritdoc/>
    public override string ToString() => $"{source}.Window({timeSpan.Ticks} ticks, {timeShift.Ticks} ticks, {scheduler})";
}

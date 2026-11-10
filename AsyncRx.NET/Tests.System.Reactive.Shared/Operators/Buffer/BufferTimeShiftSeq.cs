// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information. 

namespace Tests.System.Reactive.Shared;

/// <summary>Describes <c>source.Buffer(timeSpan, timeShift, scheduler)</c>.</summary>
/// <remarks>
/// Built by <see cref="BufferExtensions.Buffer{T}(Seq{T}, TimeSpan, TimeSpan, SchedulerRef)"/>;
/// materialized by each target through
/// <see cref="ISeqVisitor.BufferTimeShift{T}(BufferTimeShiftSeq{T})"/>.
/// </remarks>
public sealed class BufferTimeShiftSeq<T>(
    Seq<T> source,
    TimeSpan timeSpan,
    TimeSpan timeShift,
    SchedulerRef scheduler)
    : Seq<IList<T>>
{
    /// <summary>Source sequence to produce buffers over.</summary>
    public Seq<T> Source => source;

    /// <summary>Length of each buffer.</summary>
    public TimeSpan TimeSpan => timeSpan;

    /// <summary>Interval between creation of consecutive buffers.</summary>
    public TimeSpan TimeShift => timeShift;

    /// <summary>Scheduler to run buffering timers on.</summary>
    public SchedulerRef Scheduler => scheduler;

    /// <inheritdoc/>
    protected override Realized<Seq<IList<T>>> AcceptCore(ISeqVisitor visitor) =>
        visitor.BufferTimeShift(this);

    /// <inheritdoc/>
    public override string ToString() =>
        $"{source}.Buffer({timeSpan.Ticks} ticks, {timeShift.Ticks} ticks, {scheduler})";
}

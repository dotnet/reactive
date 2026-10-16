// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information. 

namespace Tests.System.Reactive.Shared;

/// <summary>Describes <c>source.Buffer(timeSpan, scheduler)</c>.</summary>
/// <remarks>
/// Built by <see cref="BufferExtensions.Buffer{T}(Seq{T}, TimeSpan, SchedulerRef)"/>;
/// materialized by each target through <see cref="ISeqVisitor.BufferTime{T}(BufferTimeSeq{T})"/>.
/// </remarks>
public sealed class BufferTimeSeq<T>(
    Seq<T> source,
    TimeSpan timeSpan,
    SchedulerRef scheduler)
    : Seq<IList<T>>
{
    /// <summary>Source sequence to produce buffers over.</summary>
    public Seq<T> Source => source;

    /// <summary>Length of each buffer.</summary>
    public TimeSpan TimeSpan => timeSpan;

    /// <summary>Scheduler to run buffering timers on.</summary>
    public SchedulerRef Scheduler => scheduler;

    /// <inheritdoc/>
    protected override Realized<Seq<IList<T>>> AcceptCore(ISeqVisitor visitor) =>
        visitor.BufferTime(this);

    /// <inheritdoc/>
    public override string ToString() => $"{source}.Buffer({timeSpan.Ticks} ticks, {scheduler})";
}

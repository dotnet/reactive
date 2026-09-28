// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information. 

namespace Tests.System.Reactive.Shared;

/// <summary>Describes <c>source.Window(timeSpan, scheduler)</c>.</summary>
/// <remarks>
/// Built by <see cref="WindowExtensions.Window{T}(Seq{T}, TimeSpan, SchedulerRef)"/>;
/// materialized by each target through <see cref="ISeqVisitor.WindowTime{T}(WindowTimeSeq{T})"/>.
/// </remarks>
public sealed class WindowTimeSeq<T>(Seq<T> source, TimeSpan timeSpan, SchedulerRef scheduler) : Nested<T>
{
    /// <summary>Source sequence to produce windows over.</summary>
    public Seq<T> Source => source;

    /// <summary>Length of each window.</summary>
    public TimeSpan TimeSpan => timeSpan;

    /// <summary>Scheduler to run windowing timers on.</summary>
    public SchedulerRef Scheduler => scheduler;

    /// <inheritdoc/>
    public override object Accept(ISeqVisitor visitor) => visitor.WindowTime(this);

    /// <inheritdoc/>
    public override string ToString() => $"{source}.Window({timeSpan.Ticks} ticks, {scheduler})";
}

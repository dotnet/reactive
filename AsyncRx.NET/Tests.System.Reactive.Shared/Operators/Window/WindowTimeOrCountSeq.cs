// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information. 

namespace Tests.System.Reactive.Shared;

/// <summary>Describes <c>source.Window(timeSpan, count, scheduler)</c>.</summary>
/// <remarks>Built by <see cref="WindowExtensions.Window{T}(Seq{T}, TimeSpan, int, SchedulerRef)"/>; materialized by each target through <see cref="ISeqVisitor.WindowTimeOrCount{T}(WindowTimeOrCountSeq{T})"/>.</remarks>
public sealed class WindowTimeOrCountSeq<T>(Seq<T> source, TimeSpan timeSpan, int count, SchedulerRef scheduler) : Nested<T>
{
    public Seq<T> Source => source;

    public TimeSpan TimeSpan => timeSpan;

    public int Count => count;

    public SchedulerRef Scheduler => scheduler;

    public override object Accept(ISeqVisitor visitor) => visitor.WindowTimeOrCount(this);

    public override string ToString() => $"{source}.Window({timeSpan.Ticks} ticks, {count}, {scheduler})";
}

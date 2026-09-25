// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information. 

namespace Tests.System.Reactive.Shared;

/// <summary>Describes <c>source.Delay(dueTime, scheduler)</c> with an absolute due time.</summary>
/// <remarks>Built by <see cref="DelayExtensions.Delay{T}(Seq{T}, DateTimeOffset, SchedulerRef)"/>; materialized by each target through <see cref="ISeqVisitor.DelayAbsolute{T}(DelayAbsoluteSeq{T})"/>.</remarks>
public sealed class DelayAbsoluteSeq<T>(Seq<T> source, DateTimeOffset dueTime, SchedulerRef scheduler) : Seq<T>
{
    public Seq<T> Source => source;

    public DateTimeOffset DueTime => dueTime;

    public SchedulerRef Scheduler => scheduler;

    public override object Accept(ISeqVisitor visitor) => visitor.DelayAbsolute(this);

    public override string ToString() => $"{source}.Delay(@{dueTime.Ticks}, {scheduler})";
}

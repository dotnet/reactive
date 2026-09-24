// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information. 

namespace Tests.System.Reactive.Shared;

public sealed class TakeTimeSeq<T>(Seq<T> source, TimeSpan duration, SchedulerRef scheduler) : Seq<T>
{
    public Seq<T> Source => source;

    public TimeSpan Duration => duration;

    public SchedulerRef Scheduler => scheduler;

    public override object Accept(ISeqVisitor visitor) => visitor.TakeTime(this);

    public override string ToString() => $"{source}.Take({duration.Ticks} ticks, {scheduler})";
}

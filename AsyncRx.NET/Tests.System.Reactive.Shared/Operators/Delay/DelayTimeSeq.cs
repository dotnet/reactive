// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information. 

namespace Tests.System.Reactive.Shared;

public sealed class DelayTimeSeq<T>(Seq<T> source, TimeSpan dueTime, SchedulerRef scheduler) : Seq<T>
{
    public Seq<T> Source => source;

    public TimeSpan DueTime => dueTime;

    public SchedulerRef Scheduler => scheduler;

    public override object Accept(ISeqVisitor visitor) => visitor.DelayTime(this);

    public override string ToString() => $"{source}.Delay({dueTime.Ticks} ticks, {scheduler})";
}

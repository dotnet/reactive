// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information. 

namespace Tests.System.Reactive.Shared;

public sealed class TimerSeq(TimeSpan dueTime, SchedulerRef scheduler) : Seq<long>
{
    public TimeSpan DueTime => dueTime;

    public SchedulerRef Scheduler => scheduler;

    public override object Accept(ISeqVisitor visitor) => visitor.Timer(this);

    public override string ToString() => $"Observable.Timer({dueTime.Ticks} ticks, {scheduler})";
}

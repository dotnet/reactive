// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information. 

namespace Tests.System.Reactive.Shared;

public sealed class WindowTimeShiftSeq<T>(Seq<T> source, TimeSpan timeSpan, TimeSpan timeShift, SchedulerRef scheduler) : Nested<T>
{
    public Seq<T> Source => source;

    public TimeSpan TimeSpan => timeSpan;

    public TimeSpan TimeShift => timeShift;

    public SchedulerRef Scheduler => scheduler;

    public override object Accept(ISeqVisitor visitor) => visitor.WindowTimeShift(this);

    public override string ToString() => $"{source}.Window({timeSpan.Ticks} ticks, {timeShift.Ticks} ticks, {scheduler})";
}

// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information. 

namespace Tests.System.Reactive.Shared;

public sealed class TakeScheduledSeq<T>(Seq<T> source, int count, SchedulerRef scheduler) : Seq<T>
{
    public Seq<T> Source => source;

    public int Count => count;

    public SchedulerRef Scheduler => scheduler;

    public override object Accept(ISeqVisitor visitor) => visitor.TakeScheduled(this);

    public override string ToString() => $"{source}.Take({count}, {scheduler})";
}

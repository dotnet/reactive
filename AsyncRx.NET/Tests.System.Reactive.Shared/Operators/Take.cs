// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information. 

namespace Tests.System.Reactive.Shared;

// The Take operator as data: one node per overload, one fluent method per overload restoring
// the sync suite's spelling, and one materializer method per overload for each platform to
// implement. This file is the whole of what the kit says about Take.

public sealed class TakeSeq<T>(Seq<T> source, int count) : Seq<T>
{
    public Seq<T> Source => source;

    public int Count => count;

    public override object Accept(ISeqVisitor visitor) => visitor.Take(this);

    public override string ToString() => $"{source}.Take({count})";
}

public sealed class TakeScheduledSeq<T>(Seq<T> source, int count, SchedulerRef scheduler) : Seq<T>
{
    public Seq<T> Source => source;

    public int Count => count;

    public SchedulerRef Scheduler => scheduler;

    public override object Accept(ISeqVisitor visitor) => visitor.TakeScheduled(this);

    public override string ToString() => $"{source}.Take({count}, {scheduler})";
}

public sealed class TakeTimeSeq<T>(Seq<T> source, TimeSpan duration, SchedulerRef scheduler) : Seq<T>
{
    public Seq<T> Source => source;

    public TimeSpan Duration => duration;

    public SchedulerRef Scheduler => scheduler;

    public override object Accept(ISeqVisitor visitor) => visitor.TakeTime(this);

    public override string ToString() => $"{source}.Take({duration.Ticks} ticks, {scheduler})";
}

public partial interface ISeqVisitor
{
    object Take<T>(TakeSeq<T> seq);

    object TakeScheduled<T>(TakeScheduledSeq<T> seq);

    object TakeTime<T>(TakeTimeSeq<T> seq);
}

public static class TakeExtensions
{
    public static Seq<T> Take<T>(this Seq<T> source, int count) => new TakeSeq<T>(source, count);

    public static Seq<T> Take<T>(this Seq<T> source, int count, SchedulerRef scheduler) => new TakeScheduledSeq<T>(source, count, scheduler);

    public static Seq<T> Take<T>(this Seq<T> source, TimeSpan duration, SchedulerRef scheduler) => new TakeTimeSeq<T>(source, duration, scheduler);
}

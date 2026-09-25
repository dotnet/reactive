// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information. 

namespace Tests.System.Reactive.Shared;

// The Take operator as data: one node per overload, one fluent method per overload restoring
// the sync suite's spelling, and one materializer method per overload for each target to
// implement. This folder is the whole of what the shared library says about Take.

public static class TakeExtensions
{
    public static Seq<T> Take<T>(this Seq<T> source, int count) => new TakeSeq<T>(source, count);

    public static Seq<T> Take<T>(this Seq<T> source, int count, SchedulerRef scheduler) => new TakeScheduledSeq<T>(source, count, scheduler);

    public static Seq<T> Take<T>(this Seq<T> source, TimeSpan duration, SchedulerRef scheduler) => new TakeTimeSeq<T>(source, duration, scheduler);
}

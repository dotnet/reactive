// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information. 

namespace Tests.System.Reactive.Shared;

/// <summary>
/// The creation operators, under the sync suite's own name so that <c>Observable.Timer(t, Scheduler)</c>
/// is the sync text with <c>scheduler</c> capitalised. Adapters that also import
/// <c>System.Reactive.Linq</c> alias one of the two.
/// </summary>
public static class Observable
{
    public static Seq<long> Timer(TimeSpan dueTime, SchedulerRef scheduler) => new TimerSeq(dueTime, scheduler);

    public static Seq<T> Return<T>(T value) => new ReturnSeq<T>(value);

    public static Seq<int> Range(int start, int count) => new RangeSeq(start, count);

    public static Seq<T> Empty<T>() => new EmptySeq<T>();

    public static Seq<T> Throw<T>(Exception error) => new ThrowSeq<T>(error, null);

    public static Seq<T> Throw<T>(Exception error, SchedulerRef scheduler) => new ThrowSeq<T>(error, scheduler);
}

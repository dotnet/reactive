// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information. 

using System.Runtime.CompilerServices;

namespace Tests.System.Reactive.Shared;

// Delay as data: the two time-based forms (relative and absolute, both taking the scheduler)
// and the two selector forms. Delay is both under test and plumbing (Window_Closings_Empty
// closes its windows with Empty().Delay(n, Scheduler)), and here that distinction does not
// exist: it is one node type either way.

public static class DelayExtensions
{
    public static Seq<T> Delay<T>(this Seq<T> source, TimeSpan dueTime, SchedulerRef scheduler) => new DelayTimeSeq<T>(source, dueTime, scheduler);

    public static Seq<T> Delay<T>(this Seq<T> source, DateTimeOffset dueTime, SchedulerRef scheduler) => new DelayAbsoluteSeq<T>(source, dueTime, scheduler);

    public static Seq<T> Delay<T, TDelay>(this Seq<T> source, Func<T, Seq<TDelay>> delayDurationSelector, [CallerArgumentExpression(nameof(delayDurationSelector))] string text = "") =>
        new DelaySelectorSeq<T, TDelay>(source, delayDurationSelector, text);

    public static Seq<T> Delay<T, TDelay>(this Seq<T> source, Seq<TDelay> subscriptionDelay, Func<T, Seq<TDelay>> delayDurationSelector, [CallerArgumentExpression(nameof(delayDurationSelector))] string text = "") =>
        new DelaySubscriptionSeq<T, TDelay>(source, subscriptionDelay, delayDurationSelector, text);
}

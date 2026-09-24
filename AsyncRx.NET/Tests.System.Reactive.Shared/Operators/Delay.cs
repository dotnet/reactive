// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information. 

using System.Runtime.CompilerServices;

namespace Tests.System.Reactive.Shared;

// Delay as data: the two time-based forms (relative and absolute, both taking the scheduler)
// and the two selector forms. Delay is both under test and plumbing (Window_Closings_Empty
// closes its windows with Empty().Delay(n, Scheduler)), and here that distinction does not
// exist: it is one node type either way.

public sealed class DelayTimeSeq<T>(Seq<T> source, TimeSpan dueTime, SchedulerRef scheduler) : Seq<T>
{
    public Seq<T> Source => source;

    public TimeSpan DueTime => dueTime;

    public SchedulerRef Scheduler => scheduler;

    public override object Accept(ISeqVisitor visitor) => visitor.DelayTime(this);

    public override string ToString() => $"{source}.Delay({dueTime.Ticks} ticks, {scheduler})";
}

public sealed class DelayAbsoluteSeq<T>(Seq<T> source, DateTimeOffset dueTime, SchedulerRef scheduler) : Seq<T>
{
    public Seq<T> Source => source;

    public DateTimeOffset DueTime => dueTime;

    public SchedulerRef Scheduler => scheduler;

    public override object Accept(ISeqVisitor visitor) => visitor.DelayAbsolute(this);

    public override string ToString() => $"{source}.Delay(@{dueTime.Ticks}, {scheduler})";
}

public sealed class DelaySelectorSeq<T, TDelay>(Seq<T> source, Func<T, Seq<TDelay>> delayDurationSelector, string text) : Seq<T>
{
    public Seq<T> Source => source;

    public Func<T, Seq<TDelay>> DelayDurationSelector => delayDurationSelector;

    public override object Accept(ISeqVisitor visitor) => visitor.DelaySelector(this);

    public override string ToString() => $"{source}.Delay({text})";
}

public sealed class DelaySubscriptionSeq<T, TDelay>(Seq<T> source, Seq<TDelay> subscriptionDelay, Func<T, Seq<TDelay>> delayDurationSelector, string text) : Seq<T>
{
    public Seq<T> Source => source;

    public Seq<TDelay> SubscriptionDelay => subscriptionDelay;

    public Func<T, Seq<TDelay>> DelayDurationSelector => delayDurationSelector;

    public override object Accept(ISeqVisitor visitor) => visitor.DelaySubscription(this);

    public override string ToString() => $"{source}.Delay({subscriptionDelay}, {text})";
}

public partial interface ISeqVisitor
{
    object DelayTime<T>(DelayTimeSeq<T> seq);

    object DelayAbsolute<T>(DelayAbsoluteSeq<T> seq);

    object DelaySelector<T, TDelay>(DelaySelectorSeq<T, TDelay> seq);

    object DelaySubscription<T, TDelay>(DelaySubscriptionSeq<T, TDelay> seq);
}

public static class DelayExtensions
{
    public static Seq<T> Delay<T>(this Seq<T> source, TimeSpan dueTime, SchedulerRef scheduler) => new DelayTimeSeq<T>(source, dueTime, scheduler);

    public static Seq<T> Delay<T>(this Seq<T> source, DateTimeOffset dueTime, SchedulerRef scheduler) => new DelayAbsoluteSeq<T>(source, dueTime, scheduler);

    public static Seq<T> Delay<T, TDelay>(this Seq<T> source, Func<T, Seq<TDelay>> delayDurationSelector, [CallerArgumentExpression(nameof(delayDurationSelector))] string text = "") =>
        new DelaySelectorSeq<T, TDelay>(source, delayDurationSelector, text);

    public static Seq<T> Delay<T, TDelay>(this Seq<T> source, Seq<TDelay> subscriptionDelay, Func<T, Seq<TDelay>> delayDurationSelector, [CallerArgumentExpression(nameof(delayDurationSelector))] string text = "") =>
        new DelaySubscriptionSeq<T, TDelay>(source, subscriptionDelay, delayDurationSelector, text);
}

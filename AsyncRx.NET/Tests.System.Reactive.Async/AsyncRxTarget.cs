// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information. 

using System.Reactive;
using System.Reactive.Concurrency;
using System.Reactive.Linq;

using Microsoft.Reactive.Testing;
using Microsoft.Reactive.Testing.Async;

using Tests.System.Reactive.Shared;


namespace Tests.System.Reactive.Async;

/// <summary>
/// The AsyncRx.NET target.
/// </summary>
/// <remarks>
/// The harness and raw surface over <c>TestAsyncScheduler</c> plus the materializer. The
/// execution shape is a constructor argument, since the target is an instance.
/// </remarks>
public sealed class AsyncRxTarget(ExecutionShape shape) : IRxTarget
{
    private static TestAsyncScheduler Unwrap(TestSchedulerRef scheduler) => (TestAsyncScheduler)scheduler.Native;

    private static IAsyncScheduler Unwrap(SchedulerRef scheduler) => (IAsyncScheduler)scheduler.Native;

    private IAsyncObservable<T> Materialize<T>(Seq<T> seq) => seq.Accept(this).Get<IAsyncObservable<T>>();

    private IAsyncObservable<IAsyncObservable<T>> Materialize<T>(Nested<T> seq) => seq.Accept(this).Get<IAsyncObservable<IAsyncObservable<T>>>();

    // ---- Harness ----

    public object CreateTestScheduler() => new TestAsyncScheduler(shape);

    // No optimisation interfaces to hide on this target: the scheduler is its own unoptimized form.
    public SchedulerRef DisableOptimizations(TestSchedulerRef scheduler) => scheduler;

    public TestableSeq<T> CreateHotObservable<T>(TestSchedulerRef scheduler, Recorded<Notification<T>>[] messages)
    {
        var source = Unwrap(scheduler).CreateHotObservable(messages);
        return new(this, source, source.Messages, $"Hot({messages.Length} messages)");
    }

    public TestableSeq<T> CreateColdObservable<T>(TestSchedulerRef scheduler, Recorded<Notification<T>>[] messages)
    {
        var source = Unwrap(scheduler).CreateColdObservable(messages);
        return new(this, source, source.Messages, $"Cold({messages.Length} messages)");
    }

    public TestableObserver<T> Start<T>(TestSchedulerRef scheduler, Func<Seq<T>> create, long created, long subscribed, long disposed)
    {
        ArgumentNullException.ThrowIfNull(create);

        var query = "";
        var observer = Unwrap(scheduler).Start(
            () =>
            {
                var seq = create();
                query = seq.ToString();
                return Materialize(seq);
            },
            created,
            subscribed,
            disposed);
        return new(this, observer, query);
    }

    // The pump runs work due now within the current tick.
    public long ScheduledAt(long tick) => tick;

    // ---- Raw surface ----

    public long Clock(TestSchedulerRef scheduler) => Unwrap(scheduler).Clock;

    public void ScheduleAbsolute(TestSchedulerRef scheduler, long tick, Func<ValueTask> action)
    {
        ArgumentNullException.ThrowIfNull(action);

        Unwrap(scheduler).ScheduleAbsolute(tick, _ => action(), $"scenario work scheduled at tick {tick}");
    }

    public TestableObserver<T> CreateObserver<T>(TestSchedulerRef scheduler) => new(this, Unwrap(scheduler).CreateObserver<T>(), "");

    public ValueTask<IAsyncDisposable> SubscribeAsync<T>(Seq<T> source, TestableObserver<T> observer) =>
        Materialize(source).SubscribeAsync((ITestableAsyncObserver<T>)observer.Native);

    public ValueTask<IAsyncDisposable> SubscribeAsync<T>(TestSchedulerRef scheduler, Seq<T> source, Func<T, ValueTask> onNext)
    {
        ArgumentNullException.ThrowIfNull(onNext);

        var pump = Unwrap(scheduler);
        return Materialize(source).SubscribeAsync(AsyncObserver.Create<T>(x =>
        {
            pump.EnsurePumpThread("delivery to a scenario's OnNext handler");
            return onNext(x);
        }));
    }

    public ValueTask<IAsyncDisposable> SubscribeAsync<T>(TestSchedulerRef scheduler, Nested<T> source, Func<Seq<T>, ValueTask> onNext)
    {
        ArgumentNullException.ThrowIfNull(onNext);

        var pump = Unwrap(scheduler);
        return Materialize(source).SubscribeAsync(AsyncObserver.Create<IAsyncObservable<T>>(window =>
        {
            pump.EnsurePumpThread("delivery of a window to a scenario's handler");
            return onNext(new NativeSeq<T>(window, "window"));
        }));
    }

    public void Run(TestSchedulerRef scheduler) => Unwrap(scheduler).Start();

    // ---- Assertions (compact form: delivery started and completed at the tick; all four subscription timestamps) ----

    public void AssertMessages<T>(TestableObserver<T> observer, Recorded<Notification<T>>[] expected) =>
        ((ITestableAsyncObserver<T>)observer.Native).Messages.AssertEqual(expected);

    public void AssertSubscriptions<T>(TestableSeq<T> source, Subscription[] expected) =>
        ((ITestableAsyncObservable<T>)source.Native).Subscriptions.AssertEqual(expected);

    // ---- The visitor: leaves and creation ----

    public RSeq<T> Native<T>(NativeSeq<T> seq) => AsRSeq((IAsyncObservable<T>)seq.Native);

    public RSeq<long> Timer(TimerSeq seq) => AsRSeq(AsyncObservable.Timer(seq.DueTime, Unwrap(seq.Scheduler)));

    public RSeq<T> Return<T>(ReturnSeq<T> seq) => AsRSeq(AsyncObservable.Return(seq.Value));

    // Rx.NET's Range(start, count) runs on the current-thread scheduler; the immediate scheduler
    // is the equivalent here (a plumbing decision).
    public RSeq<int> Range(RangeSeq seq) => AsRSeq(AsyncObservable.Range(seq.Start, seq.Count, ImmediateAsyncScheduler.Instance));

    public RSeq<T> Empty<T>(EmptySeq<T> seq) => AsRSeq(AsyncObservable.Empty<T>());

    public RSeq<T> Throw<T>(ThrowSeq<T> seq) => seq.Scheduler is null ? AsRSeq(AsyncObservable.Throw<T>(seq.Error)) : AsRSeq(AsyncObservable.Throw<T>(seq.Error, Unwrap(seq.Scheduler)));

    // ---- Plumbing ----

    public RSeq<TOut> Select<TIn, TOut>(SelectSeq<TIn, TOut> seq) => AsRSeq(Materialize(seq.Source).Select(seq.Selector));

    public RSeq<TOut> SelectIndexed<TIn, TOut>(SelectIndexedSeq<TIn, TOut> seq) => AsRSeq(Materialize(seq.Source).Select(seq.Selector));

    public RSeq<T> Where<T>(WhereSeq<T> seq) => AsRSeq(Materialize(seq.Source).Where(seq.Predicate));

    // AsyncRx.NET has no SelectMany(other) overload; Rx.NET defines it as SelectMany(_ => other).
    public RSeq<TOut> SelectMany<TIn, TOut>(SelectManySeq<TIn, TOut> seq)
    {
        var other = Materialize(seq.Other);
        return AsRSeq(Materialize(seq.Source).SelectMany(_ => other));
    }

    public RSeq<T> Concat<T>(ConcatSeq<T> seq) => AsRSeq(Materialize(seq.First).Concat(Materialize(seq.Second)));

    public RSeq<T> Merge<T>(MergeSeq<T> seq) => AsRSeq(Materialize(seq.Sources).Merge());

    public RSeq<RSeq<TOut>> SelectNested<TIn, TOut>(SelectNestedSeq<TIn, TOut> seq) =>
        AsNestedRSeq(Materialize(seq.Source).Select((window, i) => Materialize(seq.Selector(new NativeSeq<TIn>(window, "window"), i))));

    // ---- Take ----

    public RSeq<T> Take<T>(TakeSeq<T> seq) => AsRSeq(Materialize(seq.Source).Take(seq.Count));

    public RSeq<T> TakeScheduled<T>(TakeScheduledSeq<T> seq) => AsRSeq(Materialize(seq.Source).Take(seq.Count, Unwrap(seq.Scheduler)));

    public RSeq<T> TakeTime<T>(TakeTimeSeq<T> seq) => AsRSeq(Materialize(seq.Source).Take(seq.Duration, Unwrap(seq.Scheduler)));

    // ---- Window ----

    public RSeq<RSeq<T>> WindowClosings<T, TWindowClosing>(WindowClosingsSeq<T, TWindowClosing> seq) =>
        AsNestedRSeq(Materialize(seq.Source).Window(() => Materialize(seq.WindowClosingSelector())));

    public RSeq<RSeq<T>> WindowOpenings<T, TWindowOpening, TWindowClosing>(WindowOpeningsSeq<T, TWindowOpening, TWindowClosing> seq) =>
        AsNestedRSeq(Materialize(seq.Source).Window(Materialize(seq.WindowOpenings), opening => Materialize(seq.WindowClosingSelector(opening))));

    public RSeq<RSeq<T>> WindowBoundaries<T, TWindowBoundary>(WindowBoundariesSeq<T, TWindowBoundary> seq) =>
        AsNestedRSeq(Materialize(seq.Source).Window(Materialize(seq.WindowBoundaries)));

    public RSeq<RSeq<T>> WindowCount<T>(WindowCountSeq<T> seq) => AsNestedRSeq(Materialize(seq.Source).Window(seq.Count, seq.Skip));

    public RSeq<RSeq<T>> WindowTime<T>(WindowTimeSeq<T> seq) => AsNestedRSeq(Materialize(seq.Source).Window(seq.TimeSpan, Unwrap(seq.Scheduler)));

    public RSeq<RSeq<T>> WindowTimeShift<T>(WindowTimeShiftSeq<T> seq) => AsNestedRSeq(Materialize(seq.Source).Window(seq.TimeSpan, seq.TimeShift, Unwrap(seq.Scheduler)));

    public RSeq<RSeq<T>> WindowTimeOrCount<T>(WindowTimeOrCountSeq<T> seq) => AsNestedRSeq(Materialize(seq.Source).Window(seq.TimeSpan, seq.Count, Unwrap(seq.Scheduler)));

    // ---- GroupJoin ----

    public RSeq<RSeq<TResult>> GroupJoin<TLeft, TRight, TLeftDuration, TRightDuration, TResult>(GroupJoinSeq<TLeft, TRight, TLeftDuration, TRightDuration, TResult> seq)
    {
        var left = Materialize(seq.Left);
        var right = Materialize(seq.Right);
        var output = left.GroupJoin(
            right,
            left => Materialize(seq.LeftDurationSelector(left)),
            right => Materialize(seq.RightDurationSelector(right)),
            (left, group) => Materialize(seq.ResultSelector(left, new NativeSeq<TRight>(group, "group"))));
        return AsNestedRSeq(output);
    }

    public RSeq<TResult> GroupJoinValue<TLeft, TRight, TLeftDuration, TRightDuration, TResult>(GroupJoinValueSeq<TLeft, TRight, TLeftDuration, TRightDuration, TResult> seq) =>
        AsRSeq(Materialize(seq.Left).GroupJoin(
            Materialize(seq.Right),
            left => Materialize(seq.LeftDurationSelector(left)),
            right => Materialize(seq.RightDurationSelector(right)),
            (left, group) => seq.ResultSelector(left, new NativeSeq<TRight>(group, "group"))));

    // ---- Delay ----

    public RSeq<T> DelayTime<T>(DelayTimeSeq<T> seq) => AsRSeq(Materialize(seq.Source).Delay(seq.DueTime, Unwrap(seq.Scheduler)));

    public RSeq<T> DelayAbsolute<T>(DelayAbsoluteSeq<T> seq) => AsRSeq(Materialize(seq.Source).Delay(seq.DueTime, Unwrap(seq.Scheduler)));

    public RSeq<T> DelaySelector<T, TDelay>(DelaySelectorSeq<T, TDelay> seq) => AsRSeq(Materialize(seq.Source).Delay(x => Materialize(seq.DelayDurationSelector(x))));

    public RSeq<T> DelaySubscription<T, TDelay>(DelaySubscriptionSeq<T, TDelay> seq) =>
        AsRSeq(Materialize(seq.Source).Delay(Materialize(seq.SubscriptionDelay), x => Materialize(seq.DelayDurationSelector(x))));

    private static RSeq<T> AsRSeq<T>(IAsyncObservable<T> source) => new AsyncRSeq<T>(source);
    private static RSeq<RSeq<T>> AsNestedRSeq<T>(IAsyncObservable<IAsyncObservable<T>> source) => new NestedAsyncRSeq<T>(source);

    private class AsyncRSeq<T>(IAsyncObservable<T> source) : RSeq<T>
    {
        public override TS Get<TS>()
        {
            if (typeof(TS) != typeof(IAsyncObservable<T>))
            {
                throw new InvalidOperationException($"This sequence is an IAsyncObservable<T>, but the requested type was {typeof(TS)}.");
            }

            return (TS)(object)source;
        }
    }

    private class NestedAsyncRSeq<T>(IAsyncObservable<IAsyncObservable<T>> source) : RSeq<RSeq<T>>
    {
        public override TS Get<TS>()
        {
            if (typeof(TS) != typeof(IAsyncObservable<IAsyncObservable<T>>))
            {
                throw new InvalidOperationException($"This sequence is an IAsyncObservable<IAsyncObservable<T>>, but the requested type was {typeof(TS)}.");
            }

            return (TS)(object)source;
        }
    }
}

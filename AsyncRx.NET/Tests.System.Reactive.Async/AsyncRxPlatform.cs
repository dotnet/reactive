// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information. 

using System.Reactive;
using System.Reactive.Concurrency;
using System.Reactive.Linq;
using Microsoft.Reactive.Testing;
using Microsoft.Reactive.Testing.Async;
using Tests.System.Reactive.Shared;
using TestScheduler = Tests.System.Reactive.Shared.TestScheduler;

namespace Tests.System.Reactive.Async;

/// <summary>
/// The AsyncRx.NET platform: the environment and raw surface over <c>TestAsyncScheduler</c>
/// plus the materializer. The execution shape is a constructor argument, since the platform
/// is an instance.
/// </summary>
public sealed class AsyncRxPlatform(ExecutionShape shape) : IPlatform
{
    private static TestAsyncScheduler Unwrap(TestScheduler scheduler) => (TestAsyncScheduler)scheduler.Native;

    private static IAsyncScheduler Unwrap(SchedulerRef scheduler) => (IAsyncScheduler)scheduler.Native;

    private IAsyncObservable<T> Materialize<T>(Seq<T> seq) => (IAsyncObservable<T>)seq.Accept(this);

    private IAsyncObservable<IAsyncObservable<T>> Materialize<T>(Nested<T> seq) => (IAsyncObservable<IAsyncObservable<T>>)seq.Accept(this);

    // ---- Environment ----

    public object CreateTestScheduler() => new TestAsyncScheduler(shape);

    // No optimisation interfaces to hide on this platform: the scheduler is its own unoptimized form.
    public SchedulerRef DisableOptimizations(TestScheduler scheduler) => scheduler;

    public TestableSeq<T> CreateHotObservable<T>(TestScheduler scheduler, Recorded<Notification<T>>[] messages)
    {
        var source = Unwrap(scheduler).CreateHotObservable(messages);
        return new(this, source, source.Messages, $"Hot({messages.Length} messages)");
    }

    public TestableSeq<T> CreateColdObservable<T>(TestScheduler scheduler, Recorded<Notification<T>>[] messages)
    {
        var source = Unwrap(scheduler).CreateColdObservable(messages);
        return new(this, source, source.Messages, $"Cold({messages.Length} messages)");
    }

    public TestableObserver<T> Start<T>(TestScheduler scheduler, Func<Seq<T>> create, long created, long subscribed, long disposed)
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

    public long Clock(TestScheduler scheduler) => Unwrap(scheduler).Clock;

    public void ScheduleAbsolute(TestScheduler scheduler, long tick, Func<ValueTask> action)
    {
        ArgumentNullException.ThrowIfNull(action);

        Unwrap(scheduler).ScheduleAbsolute(tick, _ => action(), $"scenario work scheduled at tick {tick}");
    }

    public TestableObserver<T> CreateObserver<T>(TestScheduler scheduler) => new(this, Unwrap(scheduler).CreateObserver<T>(), "");

    public ValueTask<IAsyncDisposable> SubscribeAsync<T>(Seq<T> source, TestableObserver<T> observer) =>
        Materialize(source).SubscribeAsync((ITestableAsyncObserver<T>)observer.Native);

    public ValueTask<IAsyncDisposable> SubscribeAsync<T>(TestScheduler scheduler, Seq<T> source, Func<T, ValueTask> onNext)
    {
        ArgumentNullException.ThrowIfNull(onNext);

        var pump = Unwrap(scheduler);
        return Materialize(source).SubscribeAsync(AsyncObserver.Create<T>(x =>
        {
            pump.EnsurePumpThread("delivery to a scenario's OnNext handler");
            return onNext(x);
        }));
    }

    public ValueTask<IAsyncDisposable> SubscribeAsync<T>(TestScheduler scheduler, Nested<T> source, Func<Seq<T>, ValueTask> onNext)
    {
        ArgumentNullException.ThrowIfNull(onNext);

        var pump = Unwrap(scheduler);
        return Materialize(source).SubscribeAsync(AsyncObserver.Create<IAsyncObservable<T>>(window =>
        {
            pump.EnsurePumpThread("delivery of a window to a scenario's handler");
            return onNext(new NativeSeq<T>(window, "window"));
        }));
    }

    public void Run(TestScheduler scheduler) => Unwrap(scheduler).Start();

    // ---- Assertions (compact form: delivery started and completed at the tick; all four subscription timestamps) ----

    public void AssertEqual<T>(MessageLog<T> actual, Recorded<Notification<T>>[] expected) =>
        ((ITestableAsyncObserver<T>)actual.Native).Messages.AssertEqual(expected);

    public void AssertEqual<T>(SubscriptionLog<T> actual, Subscription[] expected) =>
        ((ITestableAsyncObservable<T>)actual.Native).Subscriptions.AssertEqual(expected);

    // ---- The visitor: leaves and creation ----

    public object Native<T>(NativeSeq<T> seq) => seq.Native;

    public object Timer(TimerSeq seq) => AsyncObservable.Timer(seq.DueTime, Unwrap(seq.Scheduler));

    public object Return<T>(ReturnSeq<T> seq) => AsyncObservable.Return(seq.Value);

    // Rx.NET's Range(start, count) runs on the current-thread scheduler; the immediate scheduler
    // is the equivalent here (a plumbing decision).
    public object Range(RangeSeq seq) => AsyncObservable.Range(seq.Start, seq.Count, ImmediateAsyncScheduler.Instance);

    public object Empty<T>(EmptySeq<T> seq) => AsyncObservable.Empty<T>();

    public object Throw<T>(ThrowSeq<T> seq) => seq.Scheduler is null ? AsyncObservable.Throw<T>(seq.Error) : AsyncObservable.Throw<T>(seq.Error, Unwrap(seq.Scheduler));

    // ---- Plumbing ----

    public object Select<TIn, TOut>(SelectSeq<TIn, TOut> seq) => Materialize(seq.Source).Select(seq.Selector);

    public object SelectIndexed<TIn, TOut>(SelectIndexedSeq<TIn, TOut> seq) => Materialize(seq.Source).Select(seq.Selector);

    public object Where<T>(WhereSeq<T> seq) => Materialize(seq.Source).Where(seq.Predicate);

    // AsyncRx.NET has no SelectMany(other) overload; Rx.NET defines it as SelectMany(_ => other).
    public object SelectMany<TIn, TOut>(SelectManySeq<TIn, TOut> seq)
    {
        var other = Materialize(seq.Other);
        return Materialize(seq.Source).SelectMany(_ => other);
    }

    public object Concat<T>(ConcatSeq<T> seq) => Materialize(seq.First).Concat(Materialize(seq.Second));

    public object Merge<T>(MergeSeq<T> seq) => Materialize(seq.Sources).Merge();

    public object SelectNested<TIn, TOut>(SelectNestedSeq<TIn, TOut> seq) =>
        Materialize(seq.Source).Select((window, i) => Materialize(seq.Selector(new NativeSeq<TIn>(window, "window"), i)));

    // ---- Take ----

    public object Take<T>(TakeSeq<T> seq) => Materialize(seq.Source).Take(seq.Count);

    public object TakeScheduled<T>(TakeScheduledSeq<T> seq) => Materialize(seq.Source).Take(seq.Count, Unwrap(seq.Scheduler));

    public object TakeTime<T>(TakeTimeSeq<T> seq) => Materialize(seq.Source).Take(seq.Duration, Unwrap(seq.Scheduler));

    // ---- Window ----

    public object WindowClosings<T, TWindowClosing>(WindowClosingsSeq<T, TWindowClosing> seq) =>
        Materialize(seq.Source).Window(() => Materialize(seq.WindowClosingSelector()));

    public object WindowOpenings<T, TWindowOpening, TWindowClosing>(WindowOpeningsSeq<T, TWindowOpening, TWindowClosing> seq) =>
        Materialize(seq.Source).Window(Materialize(seq.WindowOpenings), opening => Materialize(seq.WindowClosingSelector(opening)));

    public object WindowBoundaries<T, TWindowBoundary>(WindowBoundariesSeq<T, TWindowBoundary> seq) =>
        Materialize(seq.Source).Window(Materialize(seq.WindowBoundaries));

    public object WindowCount<T>(WindowCountSeq<T> seq) => Materialize(seq.Source).Window(seq.Count, seq.Skip);

    public object WindowTime<T>(WindowTimeSeq<T> seq) => Materialize(seq.Source).Window(seq.TimeSpan, Unwrap(seq.Scheduler));

    public object WindowTimeShift<T>(WindowTimeShiftSeq<T> seq) => Materialize(seq.Source).Window(seq.TimeSpan, seq.TimeShift, Unwrap(seq.Scheduler));

    public object WindowTimeOrCount<T>(WindowTimeOrCountSeq<T> seq) => Materialize(seq.Source).Window(seq.TimeSpan, seq.Count, Unwrap(seq.Scheduler));

    // ---- GroupJoin ----

    public object GroupJoin<TLeft, TRight, TLeftDuration, TRightDuration, TResult>(GroupJoinSeq<TLeft, TRight, TLeftDuration, TRightDuration, TResult> seq) =>
        Materialize(seq.Left).GroupJoin(
            Materialize(seq.Right),
            left => Materialize(seq.LeftDurationSelector(left)),
            right => Materialize(seq.RightDurationSelector(right)),
            (left, group) => Materialize(seq.ResultSelector(left, new NativeSeq<TRight>(group, "group"))));

    public object GroupJoinValue<TLeft, TRight, TLeftDuration, TRightDuration, TResult>(GroupJoinValueSeq<TLeft, TRight, TLeftDuration, TRightDuration, TResult> seq) =>
        Materialize(seq.Left).GroupJoin(
            Materialize(seq.Right),
            left => Materialize(seq.LeftDurationSelector(left)),
            right => Materialize(seq.RightDurationSelector(right)),
            (left, group) => seq.ResultSelector(left, new NativeSeq<TRight>(group, "group")));

    // ---- Delay ----

    public object DelayTime<T>(DelayTimeSeq<T> seq) => Materialize(seq.Source).Delay(seq.DueTime, Unwrap(seq.Scheduler));

    public object DelayAbsolute<T>(DelayAbsoluteSeq<T> seq) => Materialize(seq.Source).Delay(seq.DueTime, Unwrap(seq.Scheduler));

    public object DelaySelector<T, TDelay>(DelaySelectorSeq<T, TDelay> seq) => Materialize(seq.Source).Delay(x => Materialize(seq.DelayDurationSelector(x)));

    public object DelaySubscription<T, TDelay>(DelaySubscriptionSeq<T, TDelay> seq) =>
        Materialize(seq.Source).Delay(Materialize(seq.SubscriptionDelay), x => Materialize(seq.DelayDurationSelector(x)));
}

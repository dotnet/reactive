// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information. 

using System.Reactive;
using System.Reactive.Concurrency;
using System.Reactive.Linq;

using Microsoft.Reactive.Testing;
using RxTestScheduler = Microsoft.Reactive.Testing.TestScheduler;

namespace Tests.System.Reactive.Shared.Rx;

/// <summary>The Rx.NET target.</summary>
/// <remarks>
/// The harness and raw surface (one line each over <c>TestScheduler</c>) plus the materializer
/// (one line per node). The only casts are of leaves' <c>Native</c>; the pipeline a scenario
/// describes is built by the target's own operators with nothing in between.
/// </remarks>
public sealed class RxTarget : IRxTarget
{
    public static RxTarget Instance { get; } = new();

    private static RxTestScheduler Unwrap(TestSchedulerRef scheduler) => (RxTestScheduler)scheduler.Native;

    private static IScheduler Unwrap(SchedulerRef scheduler) => (IScheduler)scheduler.Native;

    private IObservable<T> Materialize<T>(Seq<T> seq) => seq.Accept(this).Get<IObservable<T>>();

    private IObservable<IObservable<T>> Materialize<T>(Nested<T> seq) => seq.Accept(this).Get<IObservable<IObservable<T>>>();

    // ---- Harness ----

    public object CreateTestScheduler() => new RxTestScheduler();

    public SchedulerRef DisableOptimizations(TestSchedulerRef scheduler) => new(Unwrap(scheduler).DisableOptimizations(), "Scheduler.DisableOptimizations()");

    public TestableSeq<T> CreateHotObservable<T>(TestSchedulerRef scheduler, Recorded<Notification<T>>[] messages)
    {
        var source = Unwrap(scheduler).CreateHotObservable(messages);
        return new(this, source, (IReadOnlyList<Recorded<Notification<T>>>)source.Messages, $"Hot({messages.Length} messages)");
    }

    public TestableSeq<T> CreateColdObservable<T>(TestSchedulerRef scheduler, Recorded<Notification<T>>[] messages)
    {
        var source = Unwrap(scheduler).CreateColdObservable(messages);
        return new(this, source, (IReadOnlyList<Recorded<Notification<T>>>)source.Messages, $"Cold({messages.Length} messages)");
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

    // TestScheduler.ScheduleAbsolute bumps work due now (or in the past) to Clock + 1.
    public long ScheduledAt(long tick) => tick + 1;

    // ---- Raw surface ----
    //
    // The shared delegates are async-shaped; on this target everything they can await
    // completes synchronously, and Complete() enforces that.

    public long Clock(TestSchedulerRef scheduler) => Unwrap(scheduler).Clock;

    public void ScheduleAbsolute(TestSchedulerRef scheduler, long tick, Func<ValueTask> action)
    {
        ArgumentNullException.ThrowIfNull(action);

        Unwrap(scheduler).ScheduleAbsolute(tick, () => Complete(action()));
    }

    public TestableObserver<T> CreateObserver<T>(TestSchedulerRef scheduler) => new(this, Unwrap(scheduler).CreateObserver<T>(), "");

    public ValueTask<IAsyncDisposable> SubscribeAsync<T>(Seq<T> source, TestableObserver<T> observer) =>
        new(new RxDisposable(Materialize(source).Subscribe((ITestableObserver<T>)observer.Native)));

    public ValueTask<IAsyncDisposable> SubscribeAsync<T>(TestSchedulerRef scheduler, Seq<T> source, Func<T, ValueTask> onNext)
    {
        ArgumentNullException.ThrowIfNull(onNext);

        return new(new RxDisposable(Materialize(source).Subscribe(x => Complete(onNext(x)))));
    }

    public ValueTask<IAsyncDisposable> SubscribeAsync<T>(TestSchedulerRef scheduler, Nested<T> source, Func<Seq<T>, ValueTask> onNext)
    {
        ArgumentNullException.ThrowIfNull(onNext);

        return new(new RxDisposable(Materialize(source).Subscribe(window => Complete(onNext(new NativeSeq<T>(window, "window"))))));
    }

    public void Run(TestSchedulerRef scheduler) => Unwrap(scheduler).Start();

    private static void Complete(ValueTask task)
    {
        if (!task.IsCompleted)
        {
            throw new InvalidOperationException(
                "On the Rx.NET target every operation a shared scenario can await completes synchronously, but this delegate returned an incomplete task.");
        }

        task.GetAwaiter().GetResult();
    }

    // ---- Assertions ----

    public void AssertMessages<T>(TestableObserver<T> observer, Recorded<Notification<T>>[] expected) =>
        ((ITestableObserver<T>)observer.Native).Messages.AssertEqual(expected);

    public void AssertSubscriptions<T>(TestableSeq<T> source, Subscription[] expected) =>
        ((ITestableObservable<T>)source.Native).Subscriptions.AssertEqual(expected);

    // ---- The visitor: leaves and creation ----

    public RSeq<T> Native<T>(NativeSeq<T> seq) => AsRSeq((IObservable<T>)seq.Native);

    public RSeq<long> Timer(TimerSeq seq) => AsRSeq(Observable.Timer(seq.DueTime, Unwrap(seq.Scheduler)));

    public RSeq<T> Return<T>(ReturnSeq<T> seq) => AsRSeq(Observable.Return(seq.Value));

    public RSeq<int> Range(RangeSeq seq) => AsRSeq(Observable.Range(seq.Start, seq.Count));

    public RSeq<T> Empty<T>(EmptySeq<T> seq) => AsRSeq(Observable.Empty<T>());

    public RSeq<T> Throw<T>(ThrowSeq<T> seq) => seq.Scheduler is null ? AsRSeq(Observable.Throw<T>(seq.Error)) : AsRSeq(Observable.Throw<T>(seq.Error, Unwrap(seq.Scheduler)));

    // ---- Plumbing ----

    public RSeq<TOut> Select<TIn, TOut>(SelectSeq<TIn, TOut> seq) => AsRSeq(Materialize(seq.Source).Select(seq.Selector));

    public RSeq<TOut> SelectIndexed<TIn, TOut>(SelectIndexedSeq<TIn, TOut> seq) => AsRSeq(Materialize(seq.Source).Select(seq.Selector));

    public RSeq<T> Where<T>(WhereSeq<T> seq) => AsRSeq(Materialize(seq.Source).Where(seq.Predicate));

    public RSeq<TOut> SelectMany<TIn, TOut>(SelectManySeq<TIn, TOut> seq) => AsRSeq(Materialize(seq.Source).SelectMany(Materialize(seq.Other)));

    public RSeq<T> Concat<T>(ConcatSeq<T> seq) => AsRSeq(Materialize(seq.First).Concat(Materialize(seq.Second)));

    public RSeq<T> Merge<T>(MergeSeq<T> seq) => AsRSeq(Materialize(seq.Sources).Merge());

    // The inner window reaches the callback as a value; what the callback returns is
    // materialized in place, over the real window. No Select wraps the windows.
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

    private static RSeq<T> AsRSeq<T>(IObservable<T> source) => new SyncRSeq<T>(source);

    private static RSeq<RSeq<T>> AsNestedRSeq<T>(IObservable<IObservable<T>> source) => new NestedRSeq<T>(source);

    private class SyncRSeq<T>(IObservable<T> source) : RSeq<T>
    {
        public override TS Get<TS>()
        {
            if (typeof(TS) != typeof(IObservable<T>))
            {
                throw new InvalidOperationException($"This sequence is an IObservable<T>, but the requested type was {typeof(TS)}.");
            }

            return (TS)(object)source;
        }
    }

    private class NestedRSeq<T>(IObservable<IObservable<T>> source) : RSeq<RSeq<T>>
    {
        public override TS Get<TS>()
        {
            if (typeof(TS) != typeof(IObservable<IObservable<T>>))
            {
                throw new InvalidOperationException($"This sequence is an IObservable<IObservable<T>>, but the requested type was {typeof(TS)}.");
            }

            return (TS)(object)source;
        }
    }
}

/// <summary>Presents a sync subscription through the shared, async-shaped raw surface.</summary>
internal sealed class RxDisposable(IDisposable disposable) : IAsyncDisposable
{
    public ValueTask DisposeAsync()
    {
        disposable.Dispose();
        return default;
    }
}

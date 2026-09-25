// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information. 

using System.Reactive;
using System.Reactive.Concurrency;
using System.Reactive.Linq;

using Microsoft.Reactive.Testing;
// The kit declares its own Observable (the neutral creation operators) in the enclosing
// namespace, which C# finds before any using directive; Rx.NET's is reached by alias.
using RxObservable = System.Reactive.Linq.Observable;
using RxTestScheduler = Microsoft.Reactive.Testing.TestScheduler;

namespace Tests.System.Reactive.Shared.Rx;

/// <summary>
/// The Rx.NET target: the harness and raw surface (one line each over
/// <c>TestScheduler</c>) plus the materializer (one line per node). The only casts
/// are of leaves' <c>Native</c>; the pipeline a scenario describes is built by the target's
/// own operators with nothing in between.
/// </summary>
public sealed class RxTarget : IRxTarget
{
    public static RxTarget Instance { get; } = new();

    private static RxTestScheduler Unwrap(TestScheduler scheduler) => (RxTestScheduler)scheduler.Native;

    private static IScheduler Unwrap(SchedulerRef scheduler) => (IScheduler)scheduler.Native;

    private IObservable<T> Materialize<T>(Seq<T> seq) => (IObservable<T>)seq.Accept(this);

    private IObservable<IObservable<T>> Materialize<T>(Nested<T> seq) => (IObservable<IObservable<T>>)seq.Accept(this);

    // ---- Harness ----

    public object CreateTestScheduler() => new RxTestScheduler();

    public SchedulerRef DisableOptimizations(TestScheduler scheduler) => new(Unwrap(scheduler).DisableOptimizations(), "Scheduler.DisableOptimizations()");

    public TestableSeq<T> CreateHotObservable<T>(TestScheduler scheduler, Recorded<Notification<T>>[] messages)
    {
        var source = Unwrap(scheduler).CreateHotObservable(messages);
        return new(this, source, (IReadOnlyList<Recorded<Notification<T>>>)source.Messages, $"Hot({messages.Length} messages)");
    }

    public TestableSeq<T> CreateColdObservable<T>(TestScheduler scheduler, Recorded<Notification<T>>[] messages)
    {
        var source = Unwrap(scheduler).CreateColdObservable(messages);
        return new(this, source, (IReadOnlyList<Recorded<Notification<T>>>)source.Messages, $"Cold({messages.Length} messages)");
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

    // TestScheduler.ScheduleAbsolute bumps work due now (or in the past) to Clock + 1.
    public long ScheduledAt(long tick) => tick + 1;

    // ---- Raw surface ----
    //
    // The shared delegates are async-shaped; on this target everything they can await
    // completes synchronously, and Complete() enforces that.

    public long Clock(TestScheduler scheduler) => Unwrap(scheduler).Clock;

    public void ScheduleAbsolute(TestScheduler scheduler, long tick, Func<ValueTask> action)
    {
        ArgumentNullException.ThrowIfNull(action);

        Unwrap(scheduler).ScheduleAbsolute(tick, () => Complete(action()));
    }

    public TestableObserver<T> CreateObserver<T>(TestScheduler scheduler) => new(this, Unwrap(scheduler).CreateObserver<T>(), "");

    public ValueTask<IAsyncDisposable> SubscribeAsync<T>(Seq<T> source, TestableObserver<T> observer) =>
        new(new RxDisposable(Materialize(source).Subscribe((ITestableObserver<T>)observer.Native)));

    public ValueTask<IAsyncDisposable> SubscribeAsync<T>(TestScheduler scheduler, Seq<T> source, Func<T, ValueTask> onNext)
    {
        ArgumentNullException.ThrowIfNull(onNext);

        return new(new RxDisposable(Materialize(source).Subscribe(x => Complete(onNext(x)))));
    }

    public ValueTask<IAsyncDisposable> SubscribeAsync<T>(TestScheduler scheduler, Nested<T> source, Func<Seq<T>, ValueTask> onNext)
    {
        ArgumentNullException.ThrowIfNull(onNext);

        return new(new RxDisposable(Materialize(source).Subscribe(window => Complete(onNext(new NativeSeq<T>(window, "window"))))));
    }

    public void Run(TestScheduler scheduler) => Unwrap(scheduler).Start();

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

    public void AssertEqual<T>(MessageLog<T> actual, Recorded<Notification<T>>[] expected) =>
        ((ITestableObserver<T>)actual.Native).Messages.AssertEqual(expected);

    public void AssertEqual<T>(SubscriptionLog<T> actual, Subscription[] expected) =>
        ((ITestableObservable<T>)actual.Native).Subscriptions.AssertEqual(expected);

    // ---- The visitor: leaves and creation ----

    public object Native<T>(NativeSeq<T> seq) => seq.Native;

    public object Timer(TimerSeq seq) => RxObservable.Timer(seq.DueTime, Unwrap(seq.Scheduler));

    public object Return<T>(ReturnSeq<T> seq) => RxObservable.Return(seq.Value);

    public object Range(RangeSeq seq) => RxObservable.Range(seq.Start, seq.Count);

    public object Empty<T>(EmptySeq<T> seq) => RxObservable.Empty<T>();

    public object Throw<T>(ThrowSeq<T> seq) => seq.Scheduler is null ? RxObservable.Throw<T>(seq.Error) : RxObservable.Throw<T>(seq.Error, Unwrap(seq.Scheduler));

    // ---- Plumbing ----

    public object Select<TIn, TOut>(SelectSeq<TIn, TOut> seq) => Materialize(seq.Source).Select(seq.Selector);

    public object SelectIndexed<TIn, TOut>(SelectIndexedSeq<TIn, TOut> seq) => Materialize(seq.Source).Select(seq.Selector);

    public object Where<T>(WhereSeq<T> seq) => Materialize(seq.Source).Where(seq.Predicate);

    public object SelectMany<TIn, TOut>(SelectManySeq<TIn, TOut> seq) => Materialize(seq.Source).SelectMany(Materialize(seq.Other));

    public object Concat<T>(ConcatSeq<T> seq) => Materialize(seq.First).Concat(Materialize(seq.Second));

    public object Merge<T>(MergeSeq<T> seq) => Materialize(seq.Sources).Merge();

    // The inner window reaches the callback as a value; what the callback returns is
    // materialized in place, over the real window. No Select wraps the windows.
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

/// <summary>Presents a sync subscription through the kit's async-shaped raw surface.</summary>
internal sealed class RxDisposable(IDisposable disposable) : IAsyncDisposable
{
    public ValueTask DisposeAsync()
    {
        disposable.Dispose();
        return default;
    }
}

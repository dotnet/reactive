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

/// <summary>The AsyncRx.NET target.</summary>
/// <remarks>
/// As <c>RxTarget</c>: each operator is an ordinary generic method over AsyncRx.NET's own types
/// (the <c>*Impl</c> methods), reached through the <see cref="DescriptionBridge"/>. This file
/// holds the harness, the raw surface and the assertions; each operator's visitor members and
/// <c>*Impl</c> methods sit together in a partial file named for the operator's folder under
/// <c>Operators/</c>. The execution shape is a constructor argument, since the target is an
/// instance.
/// </remarks>
public sealed partial class AsyncRxTarget : IRxTarget
{
    private readonly ExecutionShape _shape;
    private readonly DescriptionBridge _bridge;

    public AsyncRxTarget(ExecutionShape shape)
    {
        _shape = shape;
        _bridge = new DescriptionBridge(this, new(
            ObservableOf: element => typeof(IAsyncObservable<>).MakeGenericType(element),
            GroupedOf: (key, element) => typeof(IGroupedAsyncObservable<,>).MakeGenericType(key, element),
            KeyOf: group => group.GetType().GetInterfaces()
                .First(i => i.IsGenericType && i.GetGenericTypeDefinition() == typeof(IGroupedAsyncObservable<,>))
                .GetProperty("Key")!.GetValue(group)));
    }

    private static TestAsyncScheduler Unwrap(TestSchedulerRef scheduler) =>
        (TestAsyncScheduler)scheduler.Native;

    private static IAsyncScheduler Unwrap(SchedulerRef scheduler) =>
        (IAsyncScheduler)scheduler.Native;

    private IAsyncObservable<T> Materialize<T>(Seq<T> seq) =>
        seq.Accept(this).Get<IAsyncObservable<T>>();

    // ---- Harness ----

    public object CreateTestScheduler() => new TestAsyncScheduler(_shape);

    // No optimisation interfaces to hide on this target: the scheduler is its own unoptimized form.
    public SchedulerRef DisableOptimizations(TestSchedulerRef scheduler) => scheduler;

    public TestableSeq<T> CreateHotObservable<T>(TestSchedulerRef scheduler, Recorded<Notification<T>>[] messages) =>
        new(this, _bridge.Run<Seq<T>>(HotImpl<T>, Unwrap(scheduler), messages), messages, $"Hot({messages.Length} messages)");

    public TestableSeq<T> CreateColdObservable<T>(TestSchedulerRef scheduler, Recorded<Notification<T>>[] messages) =>
        new(this, _bridge.Run<Seq<T>>(ColdImpl<T>, Unwrap(scheduler), messages), messages, $"Cold({messages.Length} messages)");

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

    public TestableObserver<T> CreateObserver<T>(TestSchedulerRef scheduler) =>
        new(this, Unwrap(scheduler).CreateObserver<T>(), "");

    public ValueTask<IAsyncDisposable> SubscribeAsync<T>(Seq<T> source, TestableObserver<T> observer) =>
        Materialize(source).SubscribeAsync((ITestableAsyncObserver<T>)observer.Native);

    public ValueTask<IAsyncDisposable> SubscribeAsync<T>(TestSchedulerRef scheduler, Seq<T> source, Func<T, ValueTask> onNext)
    {
        ArgumentNullException.ThrowIfNull(onNext);

        return _bridge.Call<ValueTask<IAsyncDisposable>>(SubscribeImpl<T>, source, onNext, Unwrap(scheduler));
    }

    public ValueTask<IAsyncDisposable> SubscribeAsync<T>(TestSchedulerRef scheduler, Seq<Seq<T>> source, Func<Seq<T>, ValueTask> onNext)
    {
        ArgumentNullException.ThrowIfNull(onNext);

        return _bridge.Call<ValueTask<IAsyncDisposable>>(SubscribeImpl<Seq<T>>, source, onNext, Unwrap(scheduler));
    }

    public void Run(TestSchedulerRef scheduler) => Unwrap(scheduler).Start();

    // ---- Assertions ----
    //
    // Compact form: delivery started and completed at the tick; all four subscription timestamps.

    public void AssertMessages<T>(TestableObserver<T> observer, Recorded<Notification<T>>[] expected) =>
        ((ITestableAsyncObserver<T>)observer.Native).Messages.AssertEqual(expected);

    public void AssertSubscriptions<T>(TestableSeq<T> source, Subscription[] expected) =>
        _bridge.Call<IReadOnlyList<AsyncSubscription>>(SubscriptionsImpl<T>, source.Native).AssertEqual(expected);

    // ---- The visitor: leaves ----

    Realized<Seq<T>> ISeqVisitor.Native<T>(NativeSeq<T> seq) => seq.Native;

    // ---- The real calls into AsyncRx.NET, for the harness ----

    private static ITestableAsyncObservable<T> HotImpl<T>(
        TestAsyncScheduler scheduler,
        Recorded<Notification<T>>[] messages) =>
        scheduler.CreateHotObservable(messages);

    private static ITestableAsyncObservable<T> ColdImpl<T>(
        TestAsyncScheduler scheduler,
        Recorded<Notification<T>>[] messages) =>
        scheduler.CreateColdObservable(messages);

    private static IReadOnlyList<AsyncSubscription> SubscriptionsImpl<T>(
        ITestableAsyncObservable<T> source) =>
        source.Subscriptions;

    private static ValueTask<IAsyncDisposable> SubscribeImpl<T>(
        IAsyncObservable<T> source,
        Func<T, ValueTask> onNext,
        TestAsyncScheduler pump) =>
        source.SubscribeAsync(AsyncObserver.Create<T>(x =>
        {
            pump.EnsurePumpThread("delivery to a scenario's handler");
            return onNext(x);
        }));
}

// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information. 

using System.Reactive;
using System.Reactive.Concurrency;
using System.Reactive.Linq;
using System.Reactive.Subjects;
using System.Reflection;

using Microsoft.Reactive.Testing;
using Microsoft.Reactive.Testing.Async;

using Tests.System.Reactive.Shared;

using Tests.System.Reactive.Shared.Scenarios;

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
                .GetProperty("Key")!.GetValue(group),
            ObserverOf: element => typeof(IAsyncObserver<>).MakeGenericType(element),
            WrapObserver: observer => typeof(AsyncRxTarget)
                .GetMethod(nameof(WrapObserverImpl), BindingFlags.NonPublic | BindingFlags.Static)!
                .MakeGenericMethod(observer.GetType().GetInterfaces()
                    .First(i => i.IsGenericType && i.GetGenericTypeDefinition() == typeof(IAsyncObserver<>))
                    .GetGenericArguments()[0])
                .Invoke(null, [observer])!,
            UnwrapObserver: observer => typeof(AsyncRxTarget)
                .GetMethod(
                    nameof(UnwrapObserverImpl), BindingFlags.NonPublic | BindingFlags.Static)!
                .MakeGenericMethod(observer.GetType().GetGenericArguments()[0])
                .Invoke(null, [observer])!,
            ConnectableOf: element => typeof(IConnectableAsyncObservable<>).MakeGenericType(element),
            SubjectOf: element => typeof(IAsyncSubject<>).MakeGenericType(element)));
    }

    private static TestAsyncScheduler Unwrap(TestSchedulerRef scheduler) =>
        scheduler.Native.Get<TestAsyncScheduler>();

    private static IAsyncScheduler Unwrap(SchedulerRef scheduler) =>
        scheduler.Native.Get<IAsyncScheduler>();

    // The result as this target's own observable. When the elements are themselves descriptions
    // (a Seq<Group<string, int>> result, say) the real elements are grouped observables, and each
    // is presented as the Group the scenario wrote; IAsyncObservable's covariance gives the
    // untyped view, since every such element is a reference type.
    private IAsyncObservable<T> Materialize<T>(Seq<T> seq)
    {
        var realized = seq.Accept(this);
        return _bridge.Describes(typeof(T))
            ? realized.Get<IAsyncObservable<object>>().Select(_bridge.Present<T>)
            : realized.Get<IAsyncObservable<T>>();
    }

    // ---- Harness ----

    public Realized<TestSchedulerRef> CreateTestScheduler() =>
        Realized.Of<TestSchedulerRef>(new TestAsyncScheduler(_shape));

    // No optimisation interfaces to hide on this target: the scheduler is its own unoptimized form.
    public SchedulerRef DisableOptimizations(SchedulerRef scheduler) => scheduler;

    public SchedulerRef ImmediateScheduler { get; } =
        new(Realized.Of<SchedulerRef>(ImmediateAsyncScheduler.Instance), "Scheduler.Immediate");

    // The scheduler AsyncRx.NET's scheduler-less overloads use.
    public SchedulerRef DefaultScheduler { get; } =
        new(Realized.Of<SchedulerRef>(TaskPoolAsyncScheduler.Default), "Scheduler.Default");

    // AsyncRx.NET has one pool scheduler, with no long-running path to distinguish it by.
    public SchedulerRef ThreadPoolScheduler { get; } =
        new(
            Realized.Of<SchedulerRef>(TaskPoolAsyncScheduler.Default),
            "ThreadPoolScheduler.Instance");

    // As on the Rx.NET target: awaiting the sequence directly would resume the scenario inline
    // on whichever thread completed it, a task-pool thread or the pump's, and the test runner
    // would go on from there; a completion source that runs its continuations asynchronously
    // keeps the scenario off the producer's thread.
    public async ValueTask<IList<T>> ToListAsync<T>(Seq<T> source)
    {
        var result = new TaskCompletionSource<IList<T>>(
            TaskCreationOptions.RunContinuationsAsynchronously);

        await using var subscription = await Materialize(source).ToList().SubscribeAsync(
            AsyncObserver.Create<IList<T>>(
                list =>
                {
                    result.TrySetResult(list);
                    return default;
                },
                error =>
                {
                    result.TrySetException(error);
                    return default;
                },
                () => default));

        return await result.Task;
    }

    public SubjectSeq<T> CreateReplaySubject<T>(int bufferSize) =>
        Wrap(new SequentialReplayAsyncSubject<T>(bufferSize));

    public SubjectSeq<T> CreateBehaviorSubject<T>(T value) => Wrap(new SequentialBehaviorAsyncSubject<T>(value));

    public Realized<Seq<T>> CreateRefCountTestConnectable<T>(RefCountTests.SerialSingleNotificationConnectable<T>.State state) =>
        Realized.Of<Seq<T>>(new AsyncRxSerialSingleNotificationConnectable<T>(state, this));

    public SubjectSeq<int> CreateMySubject(MySubject.State state) => Wrap(new AsyncRxMySubject(state));

    public Realized<SchedulerDouble> CreateScheduler(SchedulerDouble scheduler) =>
        Realized.Of<SchedulerDouble>(new AsyncRxSchedulerDouble(scheduler));

    public Realized<Seq<T>> CreateRefCountTestConnectableIgnoringConnect<T>(RefCountTests.SerialConnectableIgnoringConnect<T>.State state) =>
        Realized.Of<Seq<T>>(new AsyncRxSerialConnectableIgnoringConnect<T>(state, this));

    // For the test doubles, which subscribe to a description a scenario handed them.
    internal IAsyncObservable<T> MaterializeForDouble<T>(Seq<T> seq) => Materialize(seq);

    public SubjectSeq<T> CreateSubject<T>() => Wrap(new SequentialSimpleAsyncSubject<T>());

    private static SubjectSeq<T> Wrap<T>(IAsyncSubject<T> subject)
    {
        return new(
            Realized.Of<Seq<T>>(subject),
            subject.OnNextAsync,
            subject.OnErrorAsync,
            subject.OnCompletedAsync,
            subject is SimpleAsyncSubject<T> reporting ? () => reporting.HasObservers : null);
    }

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
        return new(this, scheduler, Realized.Of<TestableObserver<T>>(observer), query);
    }

    // The pump runs work due now within the current tick.
    public long ScheduledAt(long tick) => tick;

    // ---- Raw surface ----

    public long Clock(TestSchedulerRef scheduler) => Unwrap(scheduler).Clock;

    public IDisposable ScheduleAbsolute(
        TestSchedulerRef scheduler,
        long tick,
        Func<ValueTask> action)
    {
        ArgumentNullException.ThrowIfNull(action);

        return Unwrap(scheduler).ScheduleAbsolute(
            tick, _ => action(), $"scenario work scheduled at tick {tick}");
    }

    public ValueTask<IAsyncDisposable> SubscribeAsync<T>(
        TestSchedulerRef scheduler,
        Seq<T> source,
        ObserverRef<T> observer)
    {
        ArgumentNullException.ThrowIfNull(observer);

        return FromBody(
            Unwrap(scheduler),
            _bridge.Call<ValueTask<IAsyncDisposable>>(SubscribeObserverImpl<T>, source, observer));
    }

    public TestableObserver<T> CreateObserver<T>(TestSchedulerRef scheduler) =>
        new(
            this,
            scheduler,
            Realized.Of<TestableObserver<T>>(Unwrap(scheduler).CreateObserver<T>()),
            "");

    // A body-level subscription like the handler forms below: under ForcedYield a testable
    // source's subscribe yields, and only the pump completes it.
    public ValueTask<IAsyncDisposable> SubscribeAsync<T>(Seq<T> source, TestableObserver<T> observer) =>
        FromBody(
            Unwrap(observer.Scheduler),
            Materialize(source).SubscribeAsync(observer.Native.Get<ITestableAsyncObserver<T>>()));

    public ValueTask<IAsyncDisposable> SubscribeAsync<T>(TestSchedulerRef scheduler, Seq<T> source, Func<T, ValueTask> onNext)
    {
        ArgumentNullException.ThrowIfNull(onNext);

        return FromBody(
            Unwrap(scheduler),
            _bridge.Call<ValueTask<IAsyncDisposable>>(
                SubscribeImpl<T>,
                source,
                onNext,
                Unwrap(scheduler)));
    }

    public ValueTask<IAsyncDisposable> SubscribeAsync<T>(TestSchedulerRef scheduler, Seq<Seq<T>> source, Func<Seq<T>, ValueTask> onNext)
    {
        ArgumentNullException.ThrowIfNull(onNext);

        return FromBody(
            Unwrap(scheduler),
            _bridge.Call<ValueTask<IAsyncDisposable>>(
                SubscribeImpl<Seq<T>>,
                source,
                onNext,
                Unwrap(scheduler)));
    }

    public ValueTask<IAsyncDisposable> SubscribeAsync<T>(
        TestSchedulerRef scheduler,
        Seq<T> source,
        Func<T, ValueTask> onNext,
        Func<Exception, ValueTask> onError,
        Func<ValueTask>? onCompleted)
    {
        ArgumentNullException.ThrowIfNull(onNext);
        ArgumentNullException.ThrowIfNull(onError);

        return FromBody(
            Unwrap(scheduler),
            _bridge.Call<ValueTask<IAsyncDisposable>>(
                SubscribeWithHandlersImpl<T>,
                source,
                onNext,
                onError,
                onCompleted ?? (() => default),
                Unwrap(scheduler)));
    }

    public ValueTask OnNextAsync<T>(TestableObserver<T> observer, T value) =>
        observer.Native.Get<ITestableAsyncObserver<T>>().OnNextAsync(value);

    public ValueTask OnErrorAsync<T>(TestableObserver<T> observer, Exception error) =>
        observer.Native.Get<ITestableAsyncObserver<T>>().OnErrorAsync(error);

    public ValueTask OnCompletedAsync<T>(TestableObserver<T> observer) =>
        observer.Native.Get<ITestableAsyncObserver<T>>().OnCompletedAsync();

    public void Run(TestSchedulerRef scheduler) => Unwrap(scheduler).Start();

    public void AdvanceBy(TestSchedulerRef scheduler, long ticks) => Unwrap(scheduler).AdvanceBy(ticks);

    public ValueTask<IAsyncDisposable> ConnectAsync<T>(TestSchedulerRef scheduler, ConnectableSeq<T> source) =>
        FromBody(
            Unwrap(scheduler),
            _bridge.Call<ValueTask<IAsyncDisposable>>(ConnectImpl<T>, source));

    public ValueTask<IAsyncDisposable> SubscribeAsync<T>(
        TestSchedulerRef scheduler,
        Seq<T> source,
        Func<T, ValueTask> onNext,
        Func<ValueTask> onCompleted)
    {
        ArgumentNullException.ThrowIfNull(onNext);
        ArgumentNullException.ThrowIfNull(onCompleted);

        return FromBody(
            Unwrap(scheduler),
            _bridge.Call<ValueTask<IAsyncDisposable>>(
                SubscribeWithCompletionImpl<T>,
                source,
                onNext,
                onCompleted,
                Unwrap(scheduler)));
    }

    public ValueTask<IAsyncDisposable> SubscribeAsync<T>(
        IEnumerable<T> source,
        TestableObserver<T> observer,
        SchedulerRef scheduler)
    {
        ArgumentNullException.ThrowIfNull(observer);

        return FromBody(
            Unwrap(observer.Scheduler),
            _bridge.Call<ValueTask<IAsyncDisposable>>(
                SubscribeEnumerableOnImpl<T>,
                source,
                observer.Native.Get<ITestableAsyncObserver<T>>(),
                Unwrap(scheduler)));
    }

    public ValueTask<IAsyncDisposable> SubscribeAsync<T>(
        TestSchedulerRef scheduler,
        IEnumerable<T> source,
        ObserverRef<T> observer,
        SchedulerRef? enumerateOn)
    {
        ArgumentNullException.ThrowIfNull(observer);

        return FromBody(
            Unwrap(scheduler),
            enumerateOn is null
                ? _bridge.Call<ValueTask<IAsyncDisposable>>(
                    SubscribeEnumerableImpl<T>, source, observer)
                : _bridge.Call<ValueTask<IAsyncDisposable>>(
                    SubscribeEnumerableOnImpl<T>, source, observer, Unwrap(enumerateOn)));
    }

    // The raw surface a scenario can call from its test body: a subscription or connection
    // started there completes at the current virtual time (the sync tests' Subscribe or
    // Connect before Start), and what comes back disposes the same way. From scheduled work,
    // both are simply awaited, and the wrapper only preserves identity.
    private static ValueTask<IAsyncDisposable> FromBody(
        TestAsyncScheduler scheduler,
        ValueTask<IAsyncDisposable> operation) =>
        Wrap(scheduler, scheduler.RunToCompletion(operation));

    private static async ValueTask<IAsyncDisposable> Wrap(
        TestAsyncScheduler scheduler,
        ValueTask<IAsyncDisposable> operation) =>
        AsyncRxDisposable.For(await operation, scheduler);

    // ---- Assertions ----
    //
    // Compact form: delivery started and completed at the tick; all four subscription timestamps.

    public void AssertMessages<T>(TestableObserver<T> observer, Recorded<Notification<T>>[] expected) =>
        observer.Native.Get<ITestableAsyncObserver<T>>().Messages.AssertEqual(expected);

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

    private static ValueTask<IAsyncDisposable> ConnectImpl<T>(IConnectableAsyncObservable<T> connectable) =>
        connectable.ConnectAsync();

    [AdaptsLibraryCall]
    private static IReadOnlyList<AsyncSubscription> SubscriptionsImpl<T>(
        ITestableAsyncObservable<T> source) =>
        source.Subscriptions;

    // The observer a Create callback drives.
    [AdaptsLibraryCall]
    private static ObserverRef<T> WrapObserverImpl<T>(IAsyncObserver<T> observer) =>
        new(observer.OnNextAsync, observer.OnErrorAsync, observer.OnCompletedAsync);

    // A scenario's observer (ObserverRef.Create), as the library's own.
    [AdaptsLibraryCall]
    private static IAsyncObserver<T> UnwrapObserverImpl<T>(ObserverRef<T> observer) =>
        AsyncObserver.Create<T>(
            observer.OnNextAsync,
            observer.OnErrorAsync,
            observer.OnCompletedAsync);

    private static ValueTask<IAsyncDisposable> SubscribeObserverImpl<T>(
        IAsyncObservable<T> source,
        IAsyncObserver<T> observer) =>
        source.SubscribeAsync(observer);

    [AdaptsLibraryCall]
    private static ValueTask<IAsyncDisposable> SubscribeImpl<T>(
        IAsyncObservable<T> source,
        Func<T, ValueTask> onNext,
        TestAsyncScheduler pump) =>
        source.SubscribeAsync(AsyncObserver.Create<T>(x =>
        {
            pump.EnsurePumpThread("delivery to a scenario's handler");
            return onNext(x);
        }));

    [AdaptsLibraryCall]
    private static ValueTask<IAsyncDisposable> SubscribeWithCompletionImpl<T>(
        IAsyncObservable<T> source,
        Func<T, ValueTask> onNext,
        Func<ValueTask> onCompleted,
        TestAsyncScheduler pump) =>
        source.SubscribeAsync(
            x =>
            {
                pump.EnsurePumpThread("delivery to a scenario's handler");
                return onNext(x);
            },
            () =>
            {
                pump.EnsurePumpThread("delivery of completion to a scenario's handler");
                return onCompleted();
            });

    // AsyncRx.NET has no Subscribe over an enumerable; Rx.NET's is its ToObservable followed by
    // a subscription, and this is the same over AsyncRx.NET's ToAsyncObservable.
    [AdaptsLibraryCall]
    private static ValueTask<IAsyncDisposable> SubscribeEnumerableImpl<T>(
        IEnumerable<T> source,
        IAsyncObserver<T> observer) =>
        source.ToAsyncObservable().SubscribeAsync(observer);

    [AdaptsLibraryCall]
    private static ValueTask<IAsyncDisposable> SubscribeEnumerableOnImpl<T>(
        IEnumerable<T> source,
        IAsyncObserver<T> observer,
        IAsyncScheduler scheduler) =>
        source.ToAsyncObservable(scheduler).SubscribeAsync(observer);

    [AdaptsLibraryCall]
    private static ValueTask<IAsyncDisposable> SubscribeWithHandlersImpl<T>(
        IAsyncObservable<T> source,
        Func<T, ValueTask> onNext,
        Func<Exception, ValueTask> onError,
        Func<ValueTask> onCompleted,
        TestAsyncScheduler pump) =>
        source.SubscribeAsync(AsyncObserver.Create<T>(
            x =>
            {
                pump.EnsurePumpThread("delivery to a scenario's handler");
                return onNext(x);
            },
            ex =>
            {
                pump.EnsurePumpThread("delivery of an error to a scenario's handler");
                return onError(ex);
            },
            () =>
            {
                pump.EnsurePumpThread("delivery of completion to a scenario's handler");
                return onCompleted();
            }));
}

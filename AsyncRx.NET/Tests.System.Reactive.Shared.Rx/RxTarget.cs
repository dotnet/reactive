// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information. 

using System.Reactive;
using System.Reactive.Concurrency;
using System.Reactive.Linq;
using System.Reactive.Subjects;
using System.Reflection;

using Microsoft.Reactive.Testing;

using RxTestScheduler = Microsoft.Reactive.Testing.TestScheduler;

using Tests.System.Reactive.Shared.Scenarios;

namespace Tests.System.Reactive.Shared.Rx;

/// <summary>The Rx.NET target.</summary>
/// <remarks>
/// Each operator is an ordinary generic method over Rx.NET's own types: the <c>*Impl</c>
/// methods, which are the real, breakpointable calls into the library. A visitor member hands
/// that method to the <see cref="DescriptionBridge"/> with the node's descriptions and delegates,
/// and the bridge instantiates it at the real types, so nesting of any depth is one code path.
/// The only casts are of the scheduler and observer handles. This file holds the harness, the
/// raw surface and the assertions; each operator's visitor members and <c>*Impl</c> methods sit
/// together in a partial file named for the operator's folder under <c>Operators/</c>, as the
/// <see cref="ISeqVisitor"/> parts are.
/// </remarks>
public sealed partial class RxTarget : IRxTarget
{
    public static RxTarget Instance { get; } = new();

    private readonly DescriptionBridge _bridge;

    private RxTarget()
    {
        _bridge = new DescriptionBridge(this, new(
            ObservableOf: element => typeof(IObservable<>).MakeGenericType(element),
            GroupedOf: (key, element) => typeof(IGroupedObservable<,>).MakeGenericType(key, element),
            KeyOf: group => group.GetType().GetInterfaces()
                .First(i => i.IsGenericType && i.GetGenericTypeDefinition() == typeof(IGroupedObservable<,>))
                .GetProperty("Key")!.GetValue(group),
            ObserverOf: element => typeof(IObserver<>).MakeGenericType(element),
            WrapObserver: observer => typeof(RxTarget)
                .GetMethod(nameof(WrapObserverImpl), BindingFlags.NonPublic | BindingFlags.Static)!
                .MakeGenericMethod(observer.GetType().GetInterfaces()
                    .First(i => i.IsGenericType && i.GetGenericTypeDefinition() == typeof(IObserver<>))
                    .GetGenericArguments()[0])
                .Invoke(null, [observer])!,
            ConnectableOf: element => typeof(IConnectableObservable<>).MakeGenericType(element)));
    }

    private static RxTestScheduler Unwrap(TestSchedulerRef scheduler) =>
        (RxTestScheduler)scheduler.Native;

    private static IScheduler Unwrap(SchedulerRef scheduler) => (IScheduler)scheduler.Native;

    // For a result whose elements are values (what Start records); nested results go through
    // the bridge.
    private IObservable<T> Materialize<T>(Seq<T> seq) => seq.Accept(this).Get<IObservable<T>>();

    // ---- Harness ----

    public object CreateTestScheduler() => new RxTestScheduler();

    public SchedulerRef DisableOptimizations(TestSchedulerRef scheduler) =>
        new(Unwrap(scheduler).DisableOptimizations(), "Scheduler.DisableOptimizations()");

    public SchedulerRef ImmediateScheduler { get; } = new(Scheduler.Immediate, "Scheduler.Immediate");

    public SchedulerRef DefaultScheduler { get; } = new(Scheduler.Default, "Scheduler.Default");

    public async ValueTask<IList<T>> ToListAsync<T>(Seq<T> source) =>
        await Materialize(source).ToList();

    public SubjectSeq<T> CreateReplaySubject<T>(int bufferSize) => Wrap(new ReplaySubject<T>(bufferSize));

    public SubjectSeq<T> CreateBehaviorSubject<T>(T value) => Wrap(new BehaviorSubject<T>(value));

    public Realized<Seq<T>> CreateRefCountTestConnectable<T>(RefCountTests.SerialSingleNotificationConnectable<T>.State state) =>
        Realized.Of<Seq<T>>(new RxSerialSingleNotificationConnectable<T>(state));

    public SubjectSeq<T> CreateSubject<T>() => Wrap(new Subject<T>());

    private static SubjectSeq<T> Wrap<T>(ISubject<T> subject)
    {
        return new(
            Realized.Of<Seq<T>>(subject),
            x =>
            {
                subject.OnNext(x);
                return default;
            },
            ex =>
            {
                subject.OnError(ex);
                return default;
            },
            () =>
            {
                subject.OnCompleted();
                return default;
            });
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

    public TestableObserver<T> CreateObserver<T>(TestSchedulerRef scheduler) =>
        new(this, Unwrap(scheduler).CreateObserver<T>(), "");

    public ValueTask<IAsyncDisposable> SubscribeAsync<T>(Seq<T> source, TestableObserver<T> observer) =>
        new(RxDisposable.For(Materialize(source).Subscribe((ITestableObserver<T>)observer.Native)));

    public ValueTask<IAsyncDisposable> SubscribeAsync<T>(TestSchedulerRef scheduler, Seq<T> source, Func<T, ValueTask> onNext)
    {
        ArgumentNullException.ThrowIfNull(onNext);

        return new(RxDisposable.For(_bridge.Call<IDisposable>(SubscribeImpl<T>, source, onNext)));
    }

    public ValueTask<IAsyncDisposable> SubscribeAsync<T>(TestSchedulerRef scheduler, Seq<Seq<T>> source, Func<Seq<T>, ValueTask> onNext)
    {
        ArgumentNullException.ThrowIfNull(onNext);

        // The same impl as above, instantiated at Seq<T>: the bridge subscribes to the real
        // IObservable<IObservable<T>> and hands each window to onNext as a leaf description.
        return new(RxDisposable.For(_bridge.Call<IDisposable>(SubscribeImpl<Seq<T>>, source, onNext)));
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

        var subscription = onCompleted is null
            ? _bridge.Call<IDisposable>(SubscribeWithErrorImpl<T>, source, onNext, onError)
            : _bridge.Call<IDisposable>(SubscribeWithHandlersImpl<T>, source, onNext, onError, onCompleted);
        return new(RxDisposable.For(subscription));
    }

    public ValueTask OnNextAsync<T>(TestableObserver<T> observer, T value)
    {
        ((ITestableObserver<T>)observer.Native).OnNext(value);
        return default;
    }

    public ValueTask OnErrorAsync<T>(TestableObserver<T> observer, Exception error)
    {
        ((ITestableObserver<T>)observer.Native).OnError(error);
        return default;
    }

    public ValueTask OnCompletedAsync<T>(TestableObserver<T> observer)
    {
        ((ITestableObserver<T>)observer.Native).OnCompleted();
        return default;
    }

    public void Run(TestSchedulerRef scheduler) => Unwrap(scheduler).Start();

    public ValueTask<IAsyncDisposable> ConnectAsync<T>(TestSchedulerRef scheduler, ConnectableSeq<T> source) =>
        new(RxDisposable.For(_bridge.Call<IDisposable>(ConnectImpl<T>, source)));

    private static TResult Complete<TResult>(ValueTask<TResult> task)
    {
        if (!task.IsCompleted)
        {
            throw new InvalidOperationException(
                "On the Rx.NET target every operation a shared scenario can await completes synchronously, but this delegate returned an incomplete task.");
        }

        return task.GetAwaiter().GetResult();
    }

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
        _bridge.Call<IList<Subscription>>(SubscriptionsImpl<T>, source.Native).AssertEqual(expected);

    // ---- The visitor: leaves ----

    Realized<Seq<T>> ISeqVisitor.Native<T>(NativeSeq<T> seq) => seq.Native;

    // ---- The real calls into Rx.NET, for the harness ----
    //
    // Every *Impl, here and in the operator files, is written over Rx.NET's own types with the
    // same genericity as the library's signatures, so the bridge can instantiate it at nested
    // element types (ImplSignatureTests checks this). A breakpoint in one is a breakpoint on the
    // call under test.

    private static ITestableObservable<T> HotImpl<T>(
        RxTestScheduler scheduler,
        Recorded<Notification<T>>[] messages) =>
        scheduler.CreateHotObservable(messages);

    private static ITestableObservable<T> ColdImpl<T>(
        RxTestScheduler scheduler,
        Recorded<Notification<T>>[] messages) =>
        scheduler.CreateColdObservable(messages);

    private static IDisposable ConnectImpl<T>(IConnectableObservable<T> connectable) =>
        connectable.Connect();

    private static IList<Subscription> SubscriptionsImpl<T>(ITestableObservable<T> source) =>
        source.Subscriptions;

    // The observer a Create callback drives: each call completes synchronously here.
    private static ObserverRef<T> WrapObserverImpl<T>(IObserver<T> observer) =>
        new(
            x =>
            {
                observer.OnNext(x);
                return default;
            },
            ex =>
            {
                observer.OnError(ex);
                return default;
            },
            () =>
            {
                observer.OnCompleted();
                return default;
            });

    private static IDisposable SubscribeImpl<T>(IObservable<T> source, Func<T, ValueTask> onNext) =>
        source.Subscribe(x => Complete(onNext(x)));

    private static IDisposable SubscribeWithErrorImpl<T>(
        IObservable<T> source,
        Func<T, ValueTask> onNext,
        Func<Exception, ValueTask> onError) =>
        source.Subscribe(x => Complete(onNext(x)), ex => Complete(onError(ex)));

    private static IDisposable SubscribeWithHandlersImpl<T>(
        IObservable<T> source,
        Func<T, ValueTask> onNext,
        Func<Exception, ValueTask> onError,
        Func<ValueTask> onCompleted) =>
        source.Subscribe(
            x => Complete(onNext(x)),
            ex => Complete(onError(ex)),
            () => Complete(onCompleted()));
}

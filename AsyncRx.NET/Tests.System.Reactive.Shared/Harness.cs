// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information. 

using System.Reactive;
using Microsoft.Reactive.Testing;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Tests.System.Reactive.Shared;

// The environment a shared test runs in: a per-test virtual-time scheduler that is both the
// harness (CreateHotObservable, Start) and the scheduler argument to operators, testable sources
// with Subscriptions, a recording observer with Messages, the raw surface, and the two assertions —
// all forwarding to a platform *instance* (no static abstract members, so nothing here needs a
// runtime newer than .NET Framework's). The sequences a test holds are descriptions (Seq<T>,
// Nested<T>), so the platform's part is a visitor plus this environment, and the only place a
// platform object is wrapped is at a leaf.

/// <summary>What a platform supplies: the environment (the harness operations, as instance members) and the visitor that materializes descriptions.</summary>
public interface IPlatform : ISeqVisitor
{
    // ---- Environment ----

    /// <summary>The platform's own virtual-time scheduler, for a <see cref="TestScheduler"/> to carry.</summary>
    object CreateTestScheduler();

    /// <summary>The scheduler with its optional capabilities hidden (the sync <c>DisableOptimizations()</c>), or itself where the concept does not exist.</summary>
    SchedulerRef DisableOptimizations(TestScheduler scheduler);

    TestableSeq<T> CreateHotObservable<T>(TestScheduler scheduler, Recorded<Notification<T>>[] messages);

    TestableSeq<T> CreateColdObservable<T>(TestScheduler scheduler, Recorded<Notification<T>>[] messages);

    /// <summary>Runs a scenario: calls <paramref name="create"/> at <paramref name="created"/>, materializes what it returns, subscribes at <paramref name="subscribed"/>, disposes at <paramref name="disposed"/>.</summary>
    TestableObserver<T> Start<T>(TestScheduler scheduler, Func<Seq<T>> create, long created, long subscribed, long disposed);

    /// <summary>The tick at which work scheduled "now" at <paramref name="tick"/> actually runs (the sync scheduler bumps it; the pump does not).</summary>
    long ScheduledAt(long tick);

    // ---- Raw surface (async-shaped, so the same test text runs on both platforms; the sync platform completes synchronously) ----

    long Clock(TestScheduler scheduler);

    void ScheduleAbsolute(TestScheduler scheduler, long tick, Func<ValueTask> action);

    TestableObserver<T> CreateObserver<T>(TestScheduler scheduler);

    ValueTask<IAsyncDisposable> SubscribeAsync<T>(Seq<T> source, TestableObserver<T> observer);

    ValueTask<IAsyncDisposable> SubscribeAsync<T>(TestScheduler scheduler, Seq<T> source, Func<T, ValueTask> onNext);

    /// <summary>Subscribes to a nested sequence; each inner sequence reaches the handler as a <see cref="NativeSeq{T}"/> (a wrap at the test's own observer, not in the pipeline).</summary>
    ValueTask<IAsyncDisposable> SubscribeAsync<T>(TestScheduler scheduler, Nested<T> source, Func<Seq<T>, ValueTask> onNext);

    void Run(TestScheduler scheduler);

    // ---- Assertions ----

    void AssertEqual<T>(MessageLog<T> actual, Recorded<Notification<T>>[] expected);

    void AssertEqual<T>(SubscriptionLog<T> actual, Subscription[] expected);
}

/// <summary>
/// The per-test virtual-time scheduler: the kit's counterpart of Rx.NET's <c>TestScheduler</c>,
/// usable both as the harness and as the scheduler argument to operators (it is a
/// <see cref="SchedulerRef"/>), the two roles the sync suite's <c>scheduler</c> local plays.
/// </summary>
public sealed class TestScheduler : SchedulerRef
{
    public TestScheduler(IPlatform platform)
        : base(platform.CreateTestScheduler(), "Scheduler")
    {
        Platform = platform;
    }

    public IPlatform Platform { get; }

    public TestableSeq<T> CreateHotObservable<T>(params Recorded<Notification<T>>[] messages) => Platform.CreateHotObservable(this, messages);

    public TestableSeq<T> CreateColdObservable<T>(params Recorded<Notification<T>>[] messages) => Platform.CreateColdObservable(this, messages);

    public TestableObserver<T> Start<T>(Func<Seq<T>> create) => Platform.Start(this, create, ReactiveTest.Created, ReactiveTest.Subscribed, ReactiveTest.Disposed);

    public TestableObserver<T> Start<T>(Func<Seq<T>> create, long disposed) => Platform.Start(this, create, ReactiveTest.Created, ReactiveTest.Subscribed, disposed);

    public TestableObserver<T> Start<T>(Func<Seq<T>> create, long created, long subscribed, long disposed) => Platform.Start(this, create, created, subscribed, disposed);

    /// <summary>Runs virtual time to exhaustion (the parameterless <c>TestScheduler.Start()</c>).</summary>
    public void Start() => Platform.Run(this);

    public SchedulerRef DisableOptimizations() => Platform.DisableOptimizations(this);

    public long Clock => Platform.Clock(this);

    public void ScheduleAbsolute(long tick, Func<ValueTask> action) => Platform.ScheduleAbsolute(this, tick, action);

    public void ScheduleAbsolute(long tick, Action action)
    {
        ArgumentNullException.ThrowIfNull(action);

        Platform.ScheduleAbsolute(this, tick, () =>
        {
            action();
            return default;
        });
    }

    public TestableObserver<T> CreateObserver<T>() => Platform.CreateObserver<T>(this);
}

/// <summary>A testable source (hot or cold) the platform created: a leaf that also records its subscriptions and knows the messages it plays.</summary>
public sealed class TestableSeq<T>(IPlatform platform, object native, IReadOnlyList<Recorded<Notification<T>>> messages, string description) : NativeSeq<T>(native, description)
{
    public SubscriptionLog<T> Subscriptions => new(platform, Native, ToString());

    public IReadOnlyList<Recorded<Notification<T>>> Messages => messages;
}

/// <summary>The recording observer returned by <see cref="TestScheduler.Start{T}(Func{Seq{T}})"/>, or created for the raw surface.</summary>
public sealed class TestableObserver<T>(IPlatform platform, object native, string query)
{
    public IPlatform Platform => platform;

    public object Native => native;

    /// <summary>The query this observer was started over (its description), for diagnostics.</summary>
    public string Query => query;

    public MessageLog<T> Messages => new(platform, native, query);
}

/// <summary>Recorded notifications, assertable in the shared (sync) vocabulary; a failure is prefixed with the query.</summary>
public readonly struct MessageLog<T>(IPlatform platform, object native, string query)
{
    public object Native => native;

    public void AssertEqual(params Recorded<Notification<T>>[] expected)
    {
        try
        {
            platform.AssertEqual(this, expected);
        }
        catch (Exception ex) when (query != "")
        {
            throw new AssertFailedException($"Messages of {query}: {ex.Message}", ex);
        }
    }

    public void AssertEqual(IEnumerable<Recorded<Notification<T>>> expected) => AssertEqual(expected.ToArray());
}

/// <summary>Recorded subscriptions, assertable in the shared (sync) vocabulary.</summary>
public readonly struct SubscriptionLog<T>(IPlatform platform, object native, string source)
{
    public object Native => native;

    public void AssertEqual(params Subscription[] expected)
    {
        try
        {
            platform.AssertEqual(this, expected);
        }
        catch (Exception ex)
        {
            throw new AssertFailedException($"Subscriptions of {source}: {ex.Message}", ex);
        }
    }
}

/// <summary>The raw surface's subscribe, as extension methods so it reads as in the sync suite.</summary>
public static class SubscribeExtensions
{
    public static ValueTask<IAsyncDisposable> SubscribeAsync<T>(this Seq<T> source, TestableObserver<T> observer)
    {
        ArgumentNullException.ThrowIfNull(observer);

        return observer.Platform.SubscribeAsync(source, observer);
    }

    public static ValueTask<IAsyncDisposable> SubscribeAsync<T>(this Seq<T> source, TestScheduler scheduler, Func<T, ValueTask> onNext)
    {
        ArgumentNullException.ThrowIfNull(scheduler);

        return scheduler.Platform.SubscribeAsync(scheduler, source, onNext);
    }

    public static ValueTask<IAsyncDisposable> SubscribeAsync<T>(this Seq<T> source, TestScheduler scheduler, Action<T> onNext)
    {
        ArgumentNullException.ThrowIfNull(onNext);

        return source.SubscribeAsync(scheduler, x =>
        {
            onNext(x);
            return default;
        });
    }

    public static ValueTask<IAsyncDisposable> SubscribeAsync<T>(this Nested<T> source, TestScheduler scheduler, Func<Seq<T>, ValueTask> onNext)
    {
        ArgumentNullException.ThrowIfNull(scheduler);

        return scheduler.Platform.SubscribeAsync(scheduler, source, onNext);
    }

    public static ValueTask<IAsyncDisposable> SubscribeAsync<T>(this Nested<T> source, TestScheduler scheduler, Action<Seq<T>> onNext)
    {
        ArgumentNullException.ThrowIfNull(onNext);

        return source.SubscribeAsync(scheduler, w =>
        {
            onNext(w);
            return default;
        });
    }
}

/// <summary>Base class for the shared test classes: the vocabulary (from <see cref="ReactiveTest"/>), a fresh <see cref="Scheduler"/> per test, and the platform's scheduler artifact.</summary>
#pragma warning disable CA1052 // Tests inherit from this to bring the static vocabulary into scope
public abstract class DescribedTest : ReactiveTest
#pragma warning restore CA1052
{
    /// <summary>Supplied by the adapter subclass.</summary>
    protected abstract IPlatform Platform { get; }

    protected TestScheduler Scheduler { get; private set; } = null!;

    [TestInitialize]
    public void CreateScheduler() => Scheduler = new TestScheduler(Platform);

    protected long ScheduledAt(long tick) => Platform.ScheduledAt(tick);
}

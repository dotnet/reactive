// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information. 

using System.Reactive;

using Microsoft.Reactive.Testing;

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

// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information. 

using System.Reactive;

using Microsoft.Reactive.Testing;

namespace Tests.System.Reactive.Shared;

// What the shared tests need from one Rx implementation in order to run against it. The
// scenarios are written once, in the sync suite's vocabulary; everything that differs between
// one Rx-like LINQ implementation and another is behind this interface: the observable and
// observer types themselves (reached only through a target's own objects at the leaves of a
// description), the test scheduler and testable sources, the target's timing conventions
// (ScheduledAt), the materialization of a Seq<T> description into the target's real query (the
// ISeqVisitor half), and the assertions over its recorded messages and subscriptions. A target
// is an instance (no static abstract members), so nothing here needs a runtime newer than .NET
// Framework's.

/// <summary>
/// One Rx implementation as the shared tests see it: its test scheduler, testable sources and
/// timing conventions (the harness half), and the visitor that materializes a description into
/// its own query (the <see cref="ISeqVisitor"/> half).
/// </summary>
public interface IRxTarget : ISeqVisitor
{
    // ---- Harness ----

    /// <summary>The target's own virtual-time scheduler, for a <see cref="TestScheduler"/> to carry.</summary>
    object CreateTestScheduler();

    /// <summary>The scheduler with its optional capabilities hidden (the sync <c>DisableOptimizations()</c>), or itself where the concept does not exist.</summary>
    SchedulerRef DisableOptimizations(TestScheduler scheduler);

    TestableSeq<T> CreateHotObservable<T>(TestScheduler scheduler, Recorded<Notification<T>>[] messages);

    TestableSeq<T> CreateColdObservable<T>(TestScheduler scheduler, Recorded<Notification<T>>[] messages);

    /// <summary>Runs a scenario: calls <paramref name="create"/> at <paramref name="created"/>, materializes what it returns, subscribes at <paramref name="subscribed"/>, disposes at <paramref name="disposed"/>.</summary>
    TestableObserver<T> Start<T>(TestScheduler scheduler, Func<Seq<T>> create, long created, long subscribed, long disposed);

    /// <summary>The tick at which work scheduled "now" at <paramref name="tick"/> actually runs (the sync scheduler bumps it; the pump does not).</summary>
    long ScheduledAt(long tick);

    // ---- Raw surface (async-shaped, so the same test text runs on both targets; the sync target completes synchronously) ----

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

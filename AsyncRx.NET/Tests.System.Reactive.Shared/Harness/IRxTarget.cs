// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information. 

using System.Reactive;

using Microsoft.Reactive.Testing;

namespace Tests.System.Reactive.Shared;

/// <summary>
/// One Rx implementation as the shared tests see it.
/// </summary>
/// <remarks>
/// Its test scheduler, testable sources and timing conventions (the harness half), and the
/// visitor that materializes a description into its own query (the <see cref="ISeqVisitor"/>
/// half). This is what the shared tests need from one Rx implementation in order to run against
/// it. The scenarios are written once, in the sync suite's vocabulary; everything that differs
/// between one Rx-like LINQ implementation and another is behind this interface: the observable
/// and observer types themselves (reached only through a target's own objects at the leaves of a
/// description), the test scheduler and testable sources, the target's timing conventions
/// (<see cref="ScheduledAt"/>), the materialization of a <see cref="Seq{T}"/> into the target's
/// real query (the <see cref="ISeqVisitor"/> members), and the assertions over its recorded
/// messages and subscriptions. A target is an instance (no static abstract members), so nothing
/// here needs a runtime newer than .NET Framework's. The two implementations in this repository
/// are <c>RxTarget</c> (Rx.NET) and <c>AsyncRxTarget</c> (AsyncRx.NET).
/// </remarks>
public interface IRxTarget : ISeqVisitor
{
    // ---- Harness ----

    /// <summary>
    /// The target's own virtual-time scheduler, for a <see cref="TestSchedulerRef"/> to carry.
    /// </summary>
    object CreateTestScheduler();

    /// <summary>
    /// The scheduler with its optional capabilities hidden (the sync
    /// <c>DisableOptimizations()</c>), or itself where the concept does not exist.
    /// </summary>
    SchedulerRef DisableOptimizations(TestSchedulerRef scheduler);

    /// <summary>The target's default scheduler, for real-time scenarios.</summary>
    /// <remarks>
    /// Rx.NET's <c>Scheduler.Default</c>; on AsyncRx.NET the task pool scheduler that its
    /// scheduler-less overloads use. A real-time scenario names it where the Rx.NET test has
    /// <c>Scheduler.Default</c>, and awaits completion through <see cref="ToListAsync{T}"/>
    /// rather than driving a <see cref="TestSchedulerRef"/>.
    /// </remarks>
    SchedulerRef DefaultScheduler { get; }

    /// <summary>
    /// Materializes <paramref name="source"/>, subscribes, and completes with every element it
    /// produced once it completes.
    /// </summary>
    /// <remarks>
    /// The shared replacement for the Rx.NET tests' <c>Subscribe(lst.Add, () => e.Set())</c>
    /// followed by <c>e.WaitOne()</c>: a real-time scenario is an <c>async Task</c> test method
    /// that awaits this. An error from the sequence faults the task.
    /// </remarks>
    ValueTask<IList<T>> ToListAsync<T>(Seq<T> source);

    /// <summary>
    /// Creates the target's own hot testable observable, playing <paramref name="messages"/> at
    /// their absolute virtual times, wrapped as a leaf.
    /// </summary>
    TestableSeq<T> CreateHotObservable<T>(TestSchedulerRef scheduler, Recorded<Notification<T>>[] messages);

    /// <summary>
    /// Creates the target's own cold testable observable, playing <paramref name="messages"/>
    /// relative to each subscription, wrapped as a leaf.
    /// </summary>
    TestableSeq<T> CreateColdObservable<T>(TestSchedulerRef scheduler, Recorded<Notification<T>>[] messages);

    /// <summary>Runs a scenario.</summary>
    /// <remarks>
    /// Calls <paramref name="create"/> at <paramref name="created"/>, materializes what it
    /// returns, subscribes at <paramref name="subscribed"/> and disposes at
    /// <paramref name="disposed"/>.
    /// </remarks>
    TestableObserver<T> Start<T>(TestSchedulerRef scheduler, Func<Seq<T>> create, long created, long subscribed, long disposed);

    /// <summary>
    /// The tick at which work scheduled "now" at <paramref name="tick"/> actually runs.
    /// </summary>
    /// <remarks>
    /// Rx.NET's <c>TestScheduler</c> bumps it by one; AsyncRx.NET's pump does not.
    /// </remarks>
    long ScheduledAt(long tick);

    // ---- Raw surface ----
    //
    // Async-shaped, so the same test text runs on both targets; the sync target completes
    // synchronously.

    /// <summary>The current virtual time of <paramref name="scheduler"/>.</summary>
    long Clock(TestSchedulerRef scheduler);

    /// <summary>
    /// Schedules <paramref name="action"/> to run at absolute virtual time <paramref name="tick"/>.
    /// </summary>
    /// <remarks>On the sync target the returned task is expected to be complete.</remarks>
    void ScheduleAbsolute(TestSchedulerRef scheduler, long tick, Func<ValueTask> action);

    /// <summary>
    /// Creates the target's own recording observer, wrapped, for a scenario that subscribes by
    /// hand rather than through <see cref="Start{T}"/>.
    /// </summary>
    TestableObserver<T> CreateObserver<T>(TestSchedulerRef scheduler);

    /// <summary>
    /// Materializes <paramref name="source"/> and subscribes <paramref name="observer"/>'s
    /// <see cref="TestableObserver{T}.Native"/> to it.
    /// </summary>
    ValueTask<IAsyncDisposable> SubscribeAsync<T>(Seq<T> source, TestableObserver<T> observer);

    /// <summary>Materializes <paramref name="source"/> and subscribes a handler to it.</summary>
    /// <remarks>Each element reaches <paramref name="onNext"/> as a value.</remarks>
    ValueTask<IAsyncDisposable> SubscribeAsync<T>(TestSchedulerRef scheduler, Seq<T> source, Func<T, ValueTask> onNext);

    /// <summary>Subscribes to a nested sequence.</summary>
    /// <remarks>
    /// Each inner sequence reaches the handler as a <see cref="NativeSeq{T}"/> (a wrap at the
    /// test's own observer, not in the pipeline).
    /// </remarks>
    ValueTask<IAsyncDisposable> SubscribeAsync<T>(TestSchedulerRef scheduler, Seq<Seq<T>> source, Func<Seq<T>, ValueTask> onNext);

    /// <summary>
    /// Runs virtual time to exhaustion (the parameterless <c>TestScheduler.Start()</c>).
    /// </summary>
    void Run(TestSchedulerRef scheduler);

    // ---- Assertions ----
    //
    // The target compares, because the records live in its own observer/observable (the handles'
    // Native) in its own record type, and its diagnostics are the ones worth reporting.

    /// <summary>
    /// Asserts that the messages <paramref name="observer"/> recorded are
    /// <paramref name="expected"/>.
    /// </summary>
    /// <remarks>
    /// Compares the records in <see cref="TestableObserver{T}.Native"/> against the compact form,
    /// in which a message is delivered and completed at its tick.
    /// </remarks>
    void AssertMessages<T>(TestableObserver<T> observer, Recorded<Notification<T>>[] expected);

    /// <summary>
    /// Asserts that the subscriptions <paramref name="source"/> recorded are
    /// <paramref name="expected"/>.
    /// </summary>
    /// <remarks>
    /// Compares the records in <see cref="NativeSeq{T}.Native"/> against the compact form, in
    /// which a subscription is subscribed and disposed at its two ticks.
    /// </remarks>
    void AssertSubscriptions<T>(TestableSeq<T> source, Subscription[] expected);
}

// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information. 

using System.Reactive;

using Microsoft.Reactive.Testing;

namespace Tests.System.Reactive.Shared;

/// <summary>
/// The per-test virtual-time scheduler.
/// </summary>
/// <remarks>
/// The target-neutral counterpart of Rx.NET's <c>TestScheduler</c>, usable both as the harness and
/// as the scheduler argument to operators (it is a <see cref="SchedulerRef"/>), the two roles the
/// sync suite's <c>scheduler</c> local plays. It is named after its base class rather than after
/// <c>TestScheduler</c> so that the two are never confused: this is a reference to the target's
/// test scheduler, not a test scheduler itself. Every member forwards to the <see cref="Target"/>.
/// Its <see cref="SchedulerRef.Native"/> is the target's own test scheduler —
/// <c>Microsoft.Reactive.Testing.TestScheduler</c> on Rx.NET, <c>TestAsyncScheduler</c> on
/// AsyncRx.NET — which target-specific tests may cast to reach features the shared surface does
/// not expose.
/// </remarks>
public sealed class TestSchedulerRef : SchedulerRef
{
    /// <summary>Creates a scheduler over <paramref name="target"/>'s own test scheduler.</summary>
    /// <param name="target">
    /// The target this scheduler, and every source and observer created through it, belongs to.
    /// </param>
    /// <remarks><see cref="SharedReactiveTest"/> does this once per test.</remarks>
    public TestSchedulerRef(IRxTarget target)
        : base(target.CreateTestScheduler(), "Scheduler")
    {
        Target = target;
    }

    /// <summary>The target every member forwards to.</summary>
    public IRxTarget Target { get; }

    /// <summary>
    /// Creates a hot testable source that plays <paramref name="messages"/> at their absolute
    /// virtual times, whether or not anything is subscribed (as
    /// <c>TestScheduler.CreateHotObservable</c>).
    /// </summary>
    /// <typeparam name="T">The type of the elements.</typeparam>
    /// <param name="messages">
    /// The notifications to play, in the shared vocabulary (<c>OnNext(210, 1)</c>,
    /// <c>OnCompleted&lt;int&gt;(300)</c>, ...).
    /// </param>
    public TestableSeq<T> CreateHotObservable<T>(params Recorded<Notification<T>>[] messages) => Target.CreateHotObservable(this, messages);

    /// <summary>
    /// Creates a cold testable source that plays <paramref name="messages"/> relative to the time
    /// of each subscription (as <c>TestScheduler.CreateColdObservable</c>).
    /// </summary>
    /// <typeparam name="T">The type of the elements.</typeparam>
    /// <param name="messages">
    /// The notifications to play, at times relative to subscription, in the shared vocabulary.
    /// </param>
    public TestableSeq<T> CreateColdObservable<T>(params Recorded<Notification<T>>[] messages) => Target.CreateColdObservable(this, messages);

    /// <summary>Runs a scenario with the conventional times.</summary>
    /// <typeparam name="T">The type of the elements the query produces.</typeparam>
    /// <param name="create">
    /// Builds the query under test; it runs at the creation tick, and what it returns is
    /// materialized on the target.
    /// </param>
    /// <returns>
    /// The recording observer, whose <see cref="TestableObserver{T}.Messages"/> the scenario
    /// asserts over.
    /// </returns>
    /// <remarks>
    /// Calls <paramref name="create"/> at <see cref="ReactiveTest.Created"/> (100), subscribes to
    /// what it returns at <see cref="ReactiveTest.Subscribed"/> (200), disposes at
    /// <see cref="ReactiveTest.Disposed"/> (1000), and runs virtual time to exhaustion.
    /// </remarks>
    public TestableObserver<T> Start<T>(Func<Seq<T>> create) => Target.Start(this, create, ReactiveTest.Created, ReactiveTest.Subscribed, ReactiveTest.Disposed);

    /// <summary>
    /// As <see cref="Start{T}(Func{Seq{T}})"/>, disposing at <paramref name="disposed"/> instead of
    /// the conventional 1000.
    /// </summary>
    /// <typeparam name="T">The type of the elements the query produces.</typeparam>
    /// <param name="create">Builds the query under test.</param>
    /// <param name="disposed">The virtual time at which the subscription is disposed.</param>
    /// <returns>The recording observer.</returns>
    public TestableObserver<T> Start<T>(Func<Seq<T>> create, long disposed) => Target.Start(this, create, ReactiveTest.Created, ReactiveTest.Subscribed, disposed);

    /// <summary>As <see cref="Start{T}(Func{Seq{T}})"/>, with all three times supplied.</summary>
    /// <typeparam name="T">The type of the elements the query produces.</typeparam>
    /// <param name="create">Builds the query under test.</param>
    /// <param name="created">The virtual time at which <paramref name="create"/> is called.</param>
    /// <param name="subscribed">The virtual time at which the query is subscribed to.</param>
    /// <param name="disposed">The virtual time at which the subscription is disposed.</param>
    /// <returns>The recording observer.</returns>
    public TestableObserver<T> Start<T>(Func<Seq<T>> create, long created, long subscribed, long disposed) => Target.Start(this, create, created, subscribed, disposed);

    /// <summary>
    /// Runs virtual time to exhaustion (the parameterless <c>TestScheduler.Start()</c>).
    /// </summary>
    public void Start() => Target.Run(this);

    /// <summary>
    /// Advances virtual time by <paramref name="ticks"/>, running work due on the way.
    /// </summary>
    /// <param name="ticks">The number of ticks to advance by.</param>
    /// <remarks>
    /// The sync <c>AdvanceBy</c>, for scenarios that drive the query by hand between advances.
    /// </remarks>
    public void AdvanceBy(long ticks) => Target.AdvanceBy(this, ticks);

    /// <summary>
    /// This scheduler with its optional capabilities hidden, for passing to an operator.
    /// </summary>
    /// <remarks>
    /// The sync suite's <c>scheduler.DisableOptimizations()</c>. On a target without that concept,
    /// the scheduler itself.
    /// </remarks>
    public SchedulerRef DisableOptimizations() => Target.DisableOptimizations(this);

    /// <summary>The current virtual time.</summary>
    public long Clock => Target.Clock(this);

    /// <summary>
    /// Schedules <paramref name="action"/> to run at absolute virtual time <paramref name="tick"/>
    /// (the raw surface, for scenarios that drive subscriptions by hand).
    /// </summary>
    /// <param name="tick">The absolute virtual time to run at.</param>
    /// <param name="action">
    /// The work; async-shaped so the same scenario text runs on both targets.
    /// </param>
    public void ScheduleAbsolute(long tick, Func<ValueTask> action) => Target.ScheduleAbsolute(this, tick, action);

    /// <summary>
    /// Schedules synchronous <paramref name="action"/> to run at absolute virtual time
    /// <paramref name="tick"/>.
    /// </summary>
    /// <param name="tick">The absolute virtual time to run at.</param>
    /// <param name="action">The work.</param>
    public void ScheduleAbsolute(long tick, Action action)
    {
        ArgumentNullException.ThrowIfNull(action);

        Target.ScheduleAbsolute(this, tick, () =>
        {
            action();
            return default;
        });
    }

    /// <summary>
    /// Creates a recording observer for a scenario that subscribes by hand rather than through
    /// <see cref="Start{T}(Func{Seq{T}})"/>.
    /// </summary>
    /// <typeparam name="T">The type of the elements it records.</typeparam>
    public TestableObserver<T> CreateObserver<T>() => Target.CreateObserver<T>(this);

    /// <summary>Schedules an action a number of ticks from now.</summary>
    /// <param name="dueTime">The number of ticks after the current time to run the action.</param>
    /// <param name="action">The action to run.</param>
    /// <remarks>
    /// The sync <c>ScheduleRelative</c>: an absolute scheduling at the current clock plus the
    /// due time, which is how the virtual-time schedulers define it.
    /// </remarks>
    public void ScheduleRelative(long dueTime, Func<ValueTask> action) =>
        ScheduleAbsolute(Clock + dueTime, action);

    /// <summary>
    /// As <see cref="ScheduleRelative(long, Func{ValueTask})"/>, for a synchronous action.
    /// </summary>
    /// <param name="dueTime">The number of ticks after the current time to run the action.</param>
    /// <param name="action">The action to run.</param>
    public void ScheduleRelative(long dueTime, Action action) =>
        ScheduleAbsolute(Clock + dueTime, action);
}

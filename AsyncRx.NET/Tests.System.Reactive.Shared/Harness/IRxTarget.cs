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
    Realized<TestSchedulerRef> CreateTestScheduler();

    /// <summary>
    /// The scheduler with its optional capabilities hidden (the sync
    /// <c>DisableOptimizations()</c>), or itself where the concept does not exist.
    /// </summary>
    /// <param name="scheduler">Any scheduler reference: the test scheduler or a real one.</param>
    SchedulerRef DisableOptimizations(SchedulerRef scheduler);

    /// <summary>The target's immediate scheduler, which runs work where it is scheduled.</summary>
    /// <remarks>
    /// Rx.NET's <c>Scheduler.Immediate</c>; AsyncRx.NET's <c>ImmediateAsyncScheduler.Instance</c>.
    /// For scenarios that name it where the Rx.NET test does.
    /// </remarks>
    SchedulerRef ImmediateScheduler { get; }

    /// <summary>The target's default scheduler, for real-time scenarios.</summary>
    /// <remarks>
    /// Rx.NET's <c>Scheduler.Default</c>; on AsyncRx.NET the task pool scheduler that its
    /// scheduler-less overloads use. A real-time scenario names it where the Rx.NET test has
    /// <c>Scheduler.Default</c>, and awaits completion through <see cref="ToListAsync{T}"/>
    /// rather than driving a <see cref="TestSchedulerRef"/>.
    /// </remarks>
    SchedulerRef DefaultScheduler { get; }

    /// <summary>The target's thread pool scheduler, for real-time scenarios.</summary>
    /// <remarks>
    /// Rx.NET's <c>ThreadPoolScheduler.Instance</c>, which unlike <c>Scheduler.Default</c> offers
    /// the long-running path that some operators take; on AsyncRx.NET the task pool scheduler,
    /// the same object as <see cref="DefaultScheduler"/>. A real-time scenario names it where the
    /// Rx.NET test has <c>ThreadPoolScheduler.Instance</c>.
    /// </remarks>
    SchedulerRef ThreadPoolScheduler { get; }

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

    /// <summary>Materializes <paramref name="source"/> and exposes it as a .NET event.</summary>
    /// <remarks>
    /// The target's own <c>ToEvent</c>. The result is System.Reactive's
    /// <see cref="IEventSource{T}"/>, which both libraries use, so a scenario attaches and
    /// detaches handlers on it directly.
    /// </remarks>
    IEventSource<T> ToEvent<T>(Seq<T> source);

    /// <summary>
    /// Materializes <paramref name="source"/> and exposes it as a .NET event following the event
    /// pattern.
    /// </summary>
    /// <remarks>
    /// The target's own <c>ToEventPattern</c>. The result is System.Reactive's
    /// <see cref="IEventPatternSource{TEventArgs}"/>, which both libraries use.
    /// </remarks>
    IEventPatternSource<TEventArgs> ToEventPattern<TEventArgs>(
        Seq<EventPattern<TEventArgs>> source);

    /// <summary>
    /// Runs <paramref name="onNext"/> for each element of <paramref name="source"/>.
    /// </summary>
    /// <remarks>
    /// The target's own <c>ForEachAsync(onNext, token)</c>, and the task is its own: complete
    /// when the sequence completes, faulted with the sequence's error or the handler's, and
    /// cancelled when <paramref name="token"/> is, which also unsubscribes.
    /// </remarks>
    Task ForEachAsync<T>(Seq<T> source, Action<T> onNext, CancellationToken token);

    /// <summary>Runs <paramref name="onNext"/> for each element and its index.</summary>
    /// <remarks>
    /// As <see cref="ForEachAsync{T}(Seq{T}, Action{T}, CancellationToken)"/>, for the indexed
    /// handler.
    /// </remarks>
    Task ForEachAsync<T>(Seq<T> source, Action<T, int> onNext, CancellationToken token);

    /// <summary>Subscribes to <paramref name="source"/> and returns its awaiter.</summary>
    /// <remarks>
    /// The target's own <c>GetAwaiter()</c>, over its own subject, as a
    /// <see cref="SeqAwaiter{T}"/>.
    /// </remarks>
    SeqAwaiter<T> GetAwaiter<T>(Seq<T> source);

    /// <summary>
    /// Runs a virtual-time scenario body so that the continuations the target hangs off a
    /// <c>Create</c> callback's task run inline on the test scheduler's thread.
    /// </summary>
    /// <remarks>
    /// Rx.NET's <c>CreateAsyncTest</c> runs its bodies as a task on a scheduler that executes
    /// inline, since the continuations its task-based <c>Create</c> registers follow the current
    /// task scheduler; its target does the same. AsyncRx.NET's continuations are awaits, which
    /// a current task scheduler other than the default would push to the thread pool instead,
    /// so its target runs the body as it is.
    /// </remarks>
    void RunSynchronously(Action body);

    /// <summary>
    /// Subscribes to and connects <paramref name="source"/>, and returns its awaiter.
    /// </summary>
    /// <remarks>The target's own <c>GetAwaiter()</c> for a connectable sequence.</remarks>
    SeqAwaiter<T> GetAwaiter<T>(ConnectableSeq<T> source);

    /// <summary>
    /// Subscribes to <paramref name="source"/> and returns its awaiter, which
    /// <paramref name="token"/> can cancel.
    /// </summary>
    /// <remarks>
    /// The target's own <c>RunAsync(token)</c>: cancelling unsubscribes and fails the awaiter
    /// with an <see cref="OperationCanceledException"/>, and a token already cancelled never
    /// subscribes.
    /// </remarks>
    SeqAwaiter<T> RunAsync<T>(Seq<T> source, CancellationToken token);

    /// <summary>
    /// Subscribes to and connects <paramref name="source"/>, and returns its awaiter, which
    /// <paramref name="token"/> can cancel.
    /// </summary>
    /// <remarks>The target's own <c>RunAsync(token)</c> for a connectable sequence.</remarks>
    SeqAwaiter<T> RunAsync<T>(ConnectableSeq<T> source, CancellationToken token);

    /// <summary>Creates a subject the scenario drives by hand, as a leaf.</summary>
    /// <remarks>
    /// Rx.NET's <c>Subject&lt;T&gt;</c>; AsyncRx.NET's sequential simple subject. For the
    /// real-time scenarios that push values from the test body.
    /// </remarks>
    SubjectSeq<T> CreateSubject<T>();

    /// <summary>Builds the connectable behind the <c>RefCount</c> tests' double.</summary>
    /// <param name="state">The double's state, which the object records into.</param>
    /// <remarks>
    /// The double itself, <c>RefCountTests.SerialSingleNotificationConnectable&lt;T&gt;</c>,
    /// belongs to those tests and is declared there, as its Rx.NET original is a private nested
    /// type of <c>RefCountTest</c>; only its per-target object is a target's business. The object
    /// must behave as that type's documentation says, over this target's own subjects.
    /// </remarks>
    Realized<Seq<T>> CreateRefCountTestConnectable<T>(Scenarios.RefCountTests.SerialSingleNotificationConnectable<T>.State state);

    /// <summary>Builds the target's scheduler over a <see cref="SchedulerDouble"/>.</summary>
    /// <param name="scheduler">The double, which decides what happens to each unit of work.</param>
    /// <returns>
    /// An <c>IScheduler</c> on Rx.NET, an <c>IAsyncScheduler</c> on AsyncRx.NET, for the double
    /// to carry as its <see cref="SchedulerRef.Native"/>.
    /// </returns>
    /// <remarks>
    /// One adapter per target serves every scheduler double in the suite; the doubles
    /// themselves are written once, in the shared project, as the originals are written once.
    /// Called from the double's constructor, so the adapter must not call back into it until
    /// work is scheduled.
    /// </remarks>
    Realized<SchedulerDouble> CreateScheduler(SchedulerDouble scheduler);

    /// <summary>Builds the subject behind the <see cref="Scenarios.MySubject"/> double.</summary>
    /// <param name="state">The double's state, which the object records into and reads.</param>
    /// <remarks>
    /// The double is shared by scenario classes, as Rx.NET's <c>MySubject</c> is shared across
    /// its test assembly; only the per-target object is a target's business. It must behave as
    /// that type's documentation says: count subscriptions, record a disposal, and dispose what
    /// the state registers for a value after forwarding it. The result is the plain wrapper, from
    /// which the double is built.
    /// </remarks>
    SubjectSeq<int> CreateMySubject(Scenarios.MySubject.State state);

    /// <summary>
    /// Builds the connectable behind the <c>RefCount</c> tests' connect-ignoring double.
    /// </summary>
    /// <param name="state">
    /// The double's state, which the object records into and reads from.
    /// </param>
    /// <remarks>
    /// As <see cref="CreateRefCountTestConnectable{T}"/>, for
    /// <c>RefCountTests.SerialConnectableIgnoringConnect&lt;T&gt;</c>: subscriptions go to the
    /// state's current source, materialized on this target, and <c>Connect()</c> only logs.
    /// </remarks>
    Realized<Seq<T>> CreateRefCountTestConnectableIgnoringConnect<T>(Scenarios.RefCountTests.SerialConnectableIgnoringConnect<T>.State state);

    /// <summary>Creates a replay subject the scenario drives by hand, as a leaf.</summary>
    /// <param name="bufferSize">The number of elements replayed to a new subscriber.</param>
    /// <remarks>
    /// Rx.NET's <c>ReplaySubject&lt;T&gt;</c>; AsyncRx.NET's sequential replay subject.
    /// </remarks>
    SubjectSeq<T> CreateReplaySubject<T>(int bufferSize);

    /// <summary>Creates a behavior subject the scenario drives by hand, as a leaf.</summary>
    /// <param name="value">The subject's initial value.</param>
    /// <remarks>
    /// Rx.NET's <c>BehaviorSubject&lt;T&gt;</c>; AsyncRx.NET's sequential behavior subject.
    /// </remarks>
    SubjectSeq<T> CreateBehaviorSubject<T>(T value);

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
    /// <remarks>
    /// On the sync target the returned task is expected to be complete. Returns the target's
    /// own cancellation of the work, as <c>TestScheduler.ScheduleAbsolute</c> does, for the
    /// scenarios that hand it on as a subscription's resource.
    /// </remarks>
    IDisposable ScheduleAbsolute(TestSchedulerRef scheduler, long tick, Func<ValueTask> action);

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

    /// <summary>
    /// Materializes <paramref name="source"/> and subscribes an observer written over
    /// descriptions to it: Rx.NET's <c>Subscribe(IObserver)</c>.
    /// </summary>
    /// <param name="scheduler">The test's scheduler, which carries the target.</param>
    /// <param name="source">The sequence to subscribe to.</param>
    /// <param name="observer">
    /// The observer, typically the one a <c>Create</c> body was handed; the target unwraps it
    /// into its own observer, as it does for an operator that takes one.
    /// </param>
    ValueTask<IAsyncDisposable> SubscribeAsync<T>(
        TestSchedulerRef scheduler,
        Seq<T> source,
        ObserverRef<T> observer);

    /// <summary>Materializes <paramref name="source"/> and subscribes a handler to it.</summary>
    /// <remarks>
    /// <para>Each element reaches <paramref name="onNext"/> as a value.</para>
    /// <para>
    /// This and the other members that take the scheduler are the raw surface a scenario may
    /// call from its test body as well as from scheduled work. The Rx.NET tests call
    /// <c>Subscribe</c>, <c>Connect</c> and <c>Dispose</c> before <c>Start()</c> and expect
    /// them to have happened, at the current clock, when the call returns. A target whose
    /// operations may suspend must make a body call complete at the current virtual time, and
    /// make what it returns dispose the same way; from scheduled work, the operation is simply
    /// awaited.
    /// </para>
    /// </remarks>
    ValueTask<IAsyncDisposable> SubscribeAsync<T>(TestSchedulerRef scheduler, Seq<T> source, Func<T, ValueTask> onNext);

    /// <summary>Subscribes to a nested sequence.</summary>
    /// <remarks>
    /// Each inner sequence reaches the handler as a <see cref="NativeSeq{T}"/> (a wrap at the
    /// test's own observer, not in the pipeline).
    /// </remarks>
    ValueTask<IAsyncDisposable> SubscribeAsync<T>(TestSchedulerRef scheduler, Seq<Seq<T>> source, Func<Seq<T>, ValueTask> onNext);

    /// <summary>
    /// Subscribes the three handlers to <paramref name="source"/>, the target's own
    /// <c>Subscribe(onNext, onError, onCompleted)</c>.
    /// </summary>
    /// <remarks>
    /// As the single-handler form, for the scenarios that route a group sequence's error and
    /// completion to a separate observer. <paramref name="onError"/> may be the only extra
    /// handler, for the target's <c>Subscribe(onNext, onError)</c>.
    /// </remarks>
    ValueTask<IAsyncDisposable> SubscribeAsync<T>(
        TestSchedulerRef scheduler,
        Seq<T> source,
        Func<T, ValueTask> onNext,
        Func<Exception, ValueTask> onError,
        Func<ValueTask>? onCompleted);

    /// <summary>
    /// Subscribes an element and a completion handler to <paramref name="source"/>, the target's
    /// own <c>Subscribe(onNext, onCompleted)</c>, whose error handling is the library's default.
    /// </summary>
    ValueTask<IAsyncDisposable> SubscribeAsync<T>(
        TestSchedulerRef scheduler,
        Seq<T> source,
        Func<T, ValueTask> onNext,
        Func<ValueTask> onCompleted);

    /// <summary>
    /// Subscribes a testable observer to an enumerable, enumerated on
    /// <paramref name="scheduler"/>: Rx.NET's <c>Subscribe(IEnumerable, IObserver, IScheduler)</c>.
    /// </summary>
    /// <remarks>
    /// AsyncRx.NET has no such operator; its target converts the enumerable with
    /// <c>ToAsyncObservable(scheduler)</c> and subscribes, which is what Rx.NET's does inside.
    /// </remarks>
    ValueTask<IAsyncDisposable> SubscribeAsync<T>(
        IEnumerable<T> source,
        TestableObserver<T> observer,
        SchedulerRef scheduler);

    /// <summary>
    /// Subscribes an observer to an enumerable, enumerated on <paramref name="enumerateOn"/>
    /// or, when that is null, on the library's default: Rx.NET's
    /// <c>Subscribe(IEnumerable, IObserver[, IScheduler])</c>.
    /// </summary>
    /// <param name="scheduler">The test's scheduler, which carries the target.</param>
    /// <param name="source">The enumerable to subscribe to.</param>
    /// <param name="observer">The observer, whose handlers may dispose the subscription.</param>
    /// <param name="enumerateOn">The scheduler to enumerate on, or null for the default.</param>
    ValueTask<IAsyncDisposable> SubscribeAsync<T>(
        TestSchedulerRef scheduler,
        IEnumerable<T> source,
        ObserverRef<T> observer,
        SchedulerRef? enumerateOn);

    /// <summary>Pushes a value into a testable observer, which records it.</summary>
    ValueTask OnNextAsync<T>(TestableObserver<T> observer, T value);

    /// <summary>Pushes an error into a testable observer.</summary>
    ValueTask OnErrorAsync<T>(TestableObserver<T> observer, Exception error);

    /// <summary>Completes a testable observer.</summary>
    ValueTask OnCompletedAsync<T>(TestableObserver<T> observer);

    /// <summary>
    /// Runs virtual time to exhaustion (the parameterless <c>TestScheduler.Start()</c>).
    /// </summary>
    void Run(TestSchedulerRef scheduler);

    /// <summary>
    /// Advances virtual time by <paramref name="ticks"/>, running work due on the way.
    /// </summary>
    /// <remarks>
    /// The target's own <c>AdvanceBy</c>. For scenarios that drive the query by hand between
    /// advances, rather than scheduling everything and calling <c>Start()</c>.
    /// </remarks>
    void AdvanceBy(TestSchedulerRef scheduler, long ticks);

    /// <summary>Connects a connectable sequence, the target's own <c>Connect()</c>.</summary>
    /// <remarks>
    /// Returns the connection, whose disposal disconnects. Callable from a test body, as
    /// <see cref="SubscribeAsync{T}(TestSchedulerRef, Seq{T}, Func{T, ValueTask})"/> describes.
    /// </remarks>
    ValueTask<IAsyncDisposable> ConnectAsync<T>(TestSchedulerRef scheduler, ConnectableSeq<T> source);

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

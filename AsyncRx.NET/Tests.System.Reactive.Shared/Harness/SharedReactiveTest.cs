// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information. 

using System.Reactive;

using Microsoft.Reactive.Testing;
using Microsoft.Reactive.Testing.Async;

namespace Tests.System.Reactive.Shared;

/// <summary>
/// Base class for the shared test classes and for target-specific tests written beside them.
/// </summary>
/// <remarks>
/// Supplies the expectation vocabulary by inheritance — the sync forms from
/// <see cref="ReactiveTest"/> (<c>OnNext(210, 1)</c>, <c>Subscribe(200, 300)</c>,
/// <c>Created</c>/<c>Subscribed</c>/<c>Disposed</c>) and the extended forms from
/// <see cref="AsyncReactiveTest"/> (<c>OnNext((210, 260), 1)</c>, four-timestamp
/// <c>Subscribe</c>) that only the async target can produce — plus a fresh
/// <see cref="Scheduler"/> per test and the target's scheduling convention.
/// </remarks>
public abstract class SharedReactiveTest : AsyncReactiveTest
{
    /// <summary>The target the tests run against.</summary>
    /// <remarks>Supplied by the test class in the target's own test project.</remarks>
    protected abstract IRxTarget Target { get; }

    /// <summary>The virtual-time scheduler for the current test, fresh for each one.</summary>
    /// <remarks>
    /// The harness (<c>CreateHotObservable</c>, <c>Start</c>) and the scheduler argument to
    /// operators, where the Rx.NET test had a <c>scheduler</c> local.
    /// </remarks>
    protected TestSchedulerRef Scheduler { get; private set; } = null!;

    /// <summary>The target's immediate scheduler.</summary>
    /// <remarks>
    /// A scenario names this where the Rx.NET test writes <c>Scheduler.Immediate</c>.
    /// </remarks>
    protected SchedulerRef ImmediateScheduler => Target.ImmediateScheduler;

    /// <summary>The target's default scheduler, for real-time scenarios.</summary>
    /// <remarks>
    /// A scenario names this where the Rx.NET test writes <c>Scheduler.Default</c>. See
    /// <see cref="ToListAsync{T}"/>.
    /// </remarks>
    protected SchedulerRef DefaultScheduler => Target.DefaultScheduler;

    /// <summary>The target's thread pool scheduler, for real-time scenarios.</summary>
    /// <remarks>
    /// A scenario names this where the Rx.NET test writes <c>ThreadPoolScheduler.Instance</c>.
    /// </remarks>
    protected SchedulerRef ThreadPoolScheduler => Target.ThreadPoolScheduler;

    /// <summary>A scheduler with its optional capabilities hidden.</summary>
    /// <param name="scheduler">The scheduler.</param>
    /// <remarks>
    /// A scenario calls this where the Rx.NET test writes <c>scheduler.DisableOptimizations()</c>
    /// on a scheduler other than the test scheduler, which has the method itself. On a target
    /// without the concept, the scheduler itself.
    /// </remarks>
    protected SchedulerRef DisableOptimizations(SchedulerRef scheduler) =>
        Target.DisableOptimizations(scheduler);

    /// <summary>Creates <see cref="Scheduler"/> before each test.</summary>
    /// <remarks>Called by MSTest.</remarks>
    [TestInitialize]
    public void CreateScheduler() => Scheduler = new TestSchedulerRef(Target);

    /// <summary>
    /// The virtual time at which work scheduled "now" at <paramref name="tick"/> runs on this
    /// target.
    /// </summary>
    /// <param name="tick">The virtual time at which the work was scheduled.</param>
    /// <remarks>
    /// Rx.NET's <c>TestScheduler</c> bumps it by one, AsyncRx.NET's pump does not. For the few
    /// scenarios whose expectations depend on that.
    /// </remarks>
    protected long ScheduledAt(long tick) => Target.ScheduledAt(tick);

    /// <summary>
    /// The virtual time at which work reached through <paramref name="hops"/> successive
    /// "now" schedulings from <paramref name="tick"/> runs on this target.
    /// </summary>
    /// <param name="tick">The virtual time at which the first scheduling happened.</param>
    /// <param name="hops">The number of successive schedulings.</param>
    /// <remarks>
    /// For operators that subscribe to each of several sources in its own scheduled step, such
    /// as <c>Merge(scheduler, xs, ys)</c>: the Rx.NET test expects <c>Subscribe(201, ...)</c>
    /// and <c>Subscribe(202, ...)</c>, which is <c>ScheduledAt(200, 1)</c> and
    /// <c>ScheduledAt(200, 2)</c> here.
    /// </remarks>
    protected long ScheduledAt(long tick, int hops)
    {
        for (var i = 0; i < hops; i++)
        {
            tick = Target.ScheduledAt(tick);
        }

        return tick;
    }

    /// <summary>Creates a subject the scenario drives by hand.</summary>
    /// <typeparam name="T">The type of the elements the subject carries.</typeparam>
    /// <remarks>
    /// A scenario calls this where the Rx.NET test writes <c>new Subject&lt;T&gt;()</c>.
    /// </remarks>
    protected SubjectSeq<T> CreateSubject<T>() => Target.CreateSubject<T>();

    /// <summary>Creates the counting subject double on the target under test.</summary>
    /// <remarks>
    /// A scenario calls this where the Rx.NET test writes <c>new MySubject()</c>.
    /// </remarks>
    protected Scenarios.MySubject CreateMySubject()
    {
        var state = new Scenarios.MySubject.State();
        return new(Target.CreateMySubject(state), state);
    }

    /// <summary>Creates a replay subject the scenario drives by hand.</summary>
    /// <typeparam name="T">The type of the elements the subject carries.</typeparam>
    /// <param name="bufferSize">The number of elements replayed to a new subscriber.</param>
    /// <remarks>
    /// A scenario calls this where the Rx.NET test writes <c>new ReplaySubject&lt;T&gt;(n)</c>.
    /// </remarks>
    protected SubjectSeq<T> CreateReplaySubject<T>(int bufferSize) => Target.CreateReplaySubject<T>(bufferSize);

    /// <summary>Creates a behavior subject the scenario drives by hand.</summary>
    /// <typeparam name="T">The type of the elements the subject carries.</typeparam>
    /// <param name="value">The subject's initial value.</param>
    /// <remarks>
    /// A scenario calls this where the Rx.NET test writes <c>new BehaviorSubject&lt;T&gt;(v)</c>.
    /// </remarks>
    protected SubjectSeq<T> CreateBehaviorSubject<T>(T value) => Target.CreateBehaviorSubject(value);

    /// <summary>Runs a real-time query to completion and returns everything it produced.</summary>
    /// <param name="source">The query to run.</param>
    /// <remarks>
    /// For the scenarios that run on <see cref="DefaultScheduler"/> in real time rather than on
    /// <see cref="Scheduler"/> in virtual time. Where the Rx.NET test subscribes with
    /// <c>lst.Add</c> and blocks on a <c>ManualResetEvent</c> until completion, the shared
    /// scenario is an <c>async Task</c> test method that awaits this instead.
    /// </remarks>
    protected ValueTask<IList<T>> ToListAsync<T>(Seq<T> source) => Target.ToListAsync(source);

    /// <summary>Runs the target's operator calls for <paramref name="source"/> now.</summary>
    /// <param name="source">The description to realize.</param>
    /// <returns>The target's own object, for a claim about its identity.</returns>
    /// <remarks>
    /// A description is realized when it is first used, which is usually at <c>Start</c> or at a
    /// subscription. A scenario whose claim is about the thread the operator is called on (an
    /// event conversion captures the synchronization context of its caller) must make that call
    /// where the original does; this realizes the description there, and the later use finds it
    /// realized. A scenario whose claim is about identity (<c>AsObservable</c> hides its
    /// source's) compares what this returns for two descriptions, since the descriptions
    /// themselves are always distinct objects.
    /// </remarks>
    protected object Realize<T>(Seq<T> source)
    {
        ArgumentNullException.ThrowIfNull(source);
        return source.Accept(Target).Raw;
    }

    /// <summary>
    /// Exposes <paramref name="source"/> as a .NET event on the target under test.
    /// </summary>
    /// <param name="source">The sequence to expose.</param>
    /// <remarks>
    /// Where the Rx.NET test writes <c>src.ToEvent()</c>, a scenario writes <c>ToEvent(src)</c>;
    /// the result is the same System.Reactive type on both targets, so the handler attachment
    /// that follows is the original's.
    /// </remarks>
    protected IEventSource<T> ToEvent<T>(Seq<T> source) => Target.ToEvent(source);

    /// <summary>
    /// Exposes <paramref name="source"/> as a .NET event following the event pattern, on the
    /// target under test.
    /// </summary>
    /// <param name="source">The sequence of event patterns to expose.</param>
    /// <remarks>As <see cref="ToEvent{T}"/>, for <c>src.ToEventPattern()</c>.</remarks>
    protected IEventPatternSource<TEventArgs> ToEventPattern<TEventArgs>(
        Seq<EventPattern<TEventArgs>> source) =>
        Target.ToEventPattern(source);

    /// <summary>
    /// Runs <paramref name="onNext"/> for each element of <paramref name="source"/>.
    /// </summary>
    /// <param name="source">The sequence to run.</param>
    /// <param name="onNext">The handler for each element.</param>
    /// <remarks>
    /// Where the Rx.NET test writes <c>xs.ForEachAsync(onNext)</c>, a scenario writes
    /// <c>ForEachAsync(xs, onNext)</c>, and inspects the task as the original does: it has the
    /// target's task's status, result, fault and cancellation. It is not that task itself: the
    /// target's completes on whichever thread completed the sequence, often a producer's own,
    /// and a scenario awaiting it there would resume on that thread and block it (a
    /// <c>Create</c> callback waiting on a flag its own continuation sets), so, as
    /// <see cref="ToListAsync{T}"/> does, the scenario resumes elsewhere.
    /// </remarks>
    protected Task ForEachAsync<T>(Seq<T> source, Action<T> onNext) =>
        Detached(Target.ForEachAsync(source, onNext, CancellationToken.None));

    /// <summary>
    /// Runs <paramref name="onNext"/> for each element of <paramref name="source"/> until
    /// <paramref name="token"/> is cancelled.
    /// </summary>
    /// <param name="source">The sequence to run.</param>
    /// <param name="onNext">The handler for each element.</param>
    /// <param name="token">The token whose cancellation unsubscribes and cancels the task.</param>
    protected Task ForEachAsync<T>(Seq<T> source, Action<T> onNext, CancellationToken token) =>
        Detached(Target.ForEachAsync(source, onNext, token));

    /// <summary>Runs <paramref name="onNext"/> for each element and its index.</summary>
    /// <param name="source">The sequence to run.</param>
    /// <param name="onNext">The handler for each element and its index.</param>
    protected Task ForEachAsync<T>(Seq<T> source, Action<T, int> onNext) =>
        Detached(Target.ForEachAsync(source, onNext, CancellationToken.None));

    /// <summary>
    /// Runs <paramref name="onNext"/> for each element and its index until
    /// <paramref name="token"/> is cancelled.
    /// </summary>
    /// <param name="source">The sequence to run.</param>
    /// <param name="onNext">The handler for each element and its index.</param>
    /// <param name="token">The token whose cancellation unsubscribes and cancels the task.</param>
    protected Task ForEachAsync<T>(Seq<T> source, Action<T, int> onNext, CancellationToken token) =>
        Detached(Target.ForEachAsync(source, onNext, token));

    /// <summary>
    /// Runs a virtual-time scenario body as the original does, so that the continuations the
    /// target hangs off a <c>Create</c> callback's task run inline.
    /// </summary>
    /// <param name="body">The body.</param>
    /// <remarks>
    /// The <c>CreateAsync</c> scenarios keep the original's <c>RunSynchronously(() =&gt; …)</c>;
    /// what it does is the target's, see <see cref="IRxTarget.RunSynchronously"/>.
    /// </remarks>
    protected void RunSynchronously(Action body) => Target.RunSynchronously(body);

    // The target's task mirrored, completed inline when it completes (no thread-pool hop, which
    // the virtual-time pump would report), into a task whose continuations run asynchronously.
    private static Task Detached(Task task)
    {
        var mirror = new TaskCompletionSource(TaskCreationOptions.RunContinuationsAsynchronously);
        task.ContinueWith(
            t =>
            {
                if (t.IsCanceled)
                {
                    mirror.SetCanceled();
                }
                else if (t.IsFaulted)
                {
                    mirror.SetException(t.Exception!.InnerExceptions);
                }
                else
                {
                    mirror.SetResult();
                }
            },
            CancellationToken.None,
            TaskContinuationOptions.ExecuteSynchronously,
            TaskScheduler.Default);
        return mirror.Task;
    }

    /// <summary>Subscribes to <paramref name="source"/> and returns its awaiter.</summary>
    /// <param name="source">The sequence to await.</param>
    /// <remarks>
    /// Where the Rx.NET test writes <c>xs.GetAwaiter()</c> and holds an <c>AsyncSubject</c>, a
    /// scenario writes <c>GetAwaiter(xs)</c> and holds a <see cref="SeqAwaiter{T}"/>, on which
    /// it registers its continuation and reads the result as the original does.
    /// </remarks>
    protected SeqAwaiter<T> GetAwaiter<T>(Seq<T> source) => Target.GetAwaiter(source);

    /// <summary>
    /// Subscribes to and connects <paramref name="source"/>, and returns its awaiter.
    /// </summary>
    /// <param name="source">The connectable sequence to await.</param>
    protected SeqAwaiter<T> GetAwaiter<T>(ConnectableSeq<T> source) => Target.GetAwaiter(source);

    /// <summary>
    /// Subscribes to <paramref name="source"/> and returns its awaiter, which
    /// <paramref name="token"/> can cancel.
    /// </summary>
    /// <param name="source">The sequence to run.</param>
    /// <param name="token">The token whose cancellation unsubscribes and fails the awaiter.</param>
    /// <remarks>As <see cref="GetAwaiter{T}(Seq{T})"/>, for <c>xs.RunAsync(token)</c>.</remarks>
    protected SeqAwaiter<T> RunAsync<T>(Seq<T> source, CancellationToken token) =>
        Target.RunAsync(source, token);

    /// <summary>
    /// Subscribes to and connects <paramref name="source"/>, and returns its awaiter, which
    /// <paramref name="token"/> can cancel.
    /// </summary>
    /// <param name="source">The connectable sequence to run.</param>
    /// <param name="token">The token whose cancellation unsubscribes and fails the awaiter.</param>
    protected SeqAwaiter<T> RunAsync<T>(ConnectableSeq<T> source, CancellationToken token) =>
        Target.RunAsync(source, token);
}

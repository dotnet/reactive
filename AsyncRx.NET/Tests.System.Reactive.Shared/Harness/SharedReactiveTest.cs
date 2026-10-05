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
}

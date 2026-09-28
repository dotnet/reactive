// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information. 

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
}

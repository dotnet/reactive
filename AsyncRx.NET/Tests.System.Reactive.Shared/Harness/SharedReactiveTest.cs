// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information. 

using Microsoft.Reactive.Testing;
using Microsoft.Reactive.Testing.Async;

namespace Tests.System.Reactive.Shared;

/// <summary>
/// Base class for the shared test classes and for target-specific tests written beside them.
/// Supplies the expectation vocabulary by inheritance — the sync forms from
/// <see cref="ReactiveTest"/> (<c>OnNext(210, 1)</c>, <c>Subscribe(200, 300)</c>,
/// <c>Created</c>/<c>Subscribed</c>/<c>Disposed</c>) and the extended forms from
/// <see cref="AsyncReactiveTest"/> (<c>OnNext((210, 260), 1)</c>, four-timestamp
/// <c>Subscribe</c>) that only the async target can produce — plus a fresh
/// <see cref="Scheduler"/> per test and the target's scheduling convention.
/// </summary>
public abstract class SharedReactiveTest : AsyncReactiveTest
{
    /// <summary>Supplied by the adapter subclass.</summary>
    protected abstract IRxTarget Target { get; }

    protected TestScheduler Scheduler { get; private set; } = null!;

    [TestInitialize]
    public void CreateScheduler() => Scheduler = new TestScheduler(Target);

    protected long ScheduledAt(long tick) => Target.ScheduledAt(tick);
}

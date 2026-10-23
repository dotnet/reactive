// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information. 

namespace Tests.System.Reactive.Shared.Scenarios;

/// <summary>A synchronization context that posts its callbacks to the test scheduler.</summary>
/// <param name="scheduler">The test scheduler to post to.</param>
/// <remarks>
/// Rx.NET's <c>MyCtx</c>, declared in its <c>ObserveOnTest.cs</c> and shared with
/// <c>SubscribeOnTest</c>, so a top-level type here. It schedules each <c>Post</c> on the
/// scheduler at the current clock, which Rx.NET's <c>TestScheduler</c> bumps by one tick and the
/// async pump runs at the same tick, so a scenario reads the ticks of what went through the
/// context with <c>ScheduledAt</c>. A context is the same type on both targets, so one class
/// serves both and no target builds anything for it.
/// </remarks>
public sealed class MyCtx(TestSchedulerRef scheduler) : SynchronizationContext
{
    /// <inheritdoc/>
    public override void Post(SendOrPostCallback d, object? state) =>
        scheduler.ScheduleAbsolute(scheduler.Clock, () => d(state));
}

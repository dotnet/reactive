// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information. 

namespace Microsoft.Reactive.Testing.Async;

/// <summary>
/// Describes how yield points under the control of a test should behave.
/// </summary>
/// <remarks>
/// <para>
/// This determines how <see cref="TestAsyncScheduler.YieldPoint"/> behaves. The
/// <see cref="ITestableAsyncObservable{T}"/> implementations supplied by the scheduler's
/// <see cref="TestScheduler.CreateHotObservable{T}(Microsoft.Reactive.Testing.Recorded{System.Reactive.Notification{T}}[])"/> and
/// <see cref="TestScheduler.CreateColdObservable{T}(Microsoft.Reactive.Testing.Recorded{System.Reactive.Notification{T}}[])"/>
/// and also the <see cref="ITestableAsyncObserver{T}"/> supplied by
/// <see cref="TestScheduler.CreateObserver{T}"/> all call <see cref="TestAsyncScheduler.YieldPoint"/>
/// at various points to enable tests to determine what happens when the code under test uses
/// <c>await</c> expressions.
/// </para>
/// <para>
/// Specifically, the fake observables call <see cref="TestAsyncScheduler.YieldPoint"/> at:
/// </para>
/// <list type="bullet">
/// <item>Subscription</item>
/// <item>Before delivery of each notification to the observer</item>
/// <item>Unsubscription (<c>DisposeAsync</c> of the subscription object)</item>
/// </list>
/// <para>
/// and the fake observers call it directly after recording each notification received from the
/// observable (<c>OnNextAsync</c>, etc.), and before completing the task returned by the
/// notification handler.
/// </para>
/// <para>
/// Note that the intent here is path coverage, not schedule exploration: a test will attempt
/// to deliver exactly the same sequence of notifications and on the same schedule regardless of
/// yield point behaviour. The goal is to discover logic errors in the code under test that cause
/// unintended differences in behaviour depending on whether an <c>await</c> expression actually
/// suspends or completes synchronously. (Since tests use virtual time and can therefore fully
/// control the nominal time, the nominal timing of everything the test does is unchanged by
/// this mode, so if the code under test is correctly implemented, we would also expect the timing
/// of the observed behaviour not to change just because a particular await point does or does not
/// yield.)
/// </para>
/// </remarks>
public enum ExecutionShape
{
    /// <summary>
    /// Test harness await points complete synchronously.
    /// </summary>
    /// <remarks>
    /// This causes <see cref="TestAsyncScheduler.YieldPoint"/> to return <see cref="ValueTask"/>s
    /// that are already complete. This in turn will cause <c>await</c> expressions in the code
    /// under test to complete synchronously.
    /// </remarks>
    SynchronousCompletion,

    /// <summary>
    /// Test harness await points do not complete synchronously.
    /// </summary>
    /// <remarks>
    /// <para>
    /// Even in cases where the operation being awaited is logically instantaneous (i.e., it should
    /// complete in the current virtual tick), <see cref="TestAsyncScheduler.YieldPoint"/> will
    /// return a <see cref="ValueTask"/> that is not yet complete. It will be completed before
    /// virtual time advances to the next tick, but this forces the caller's deferred completion
    /// code path to be exercised.
    /// </para>
    /// <para>
    /// The completion is scheduled via the pump's continuation queue. This means it will run
    /// ahead of other work at the same virtual time, ensuring that work still executes in the
    /// same order as it would have if the <c>await</c> had completed synchronously.
    /// </para>
    /// </remarks>
    ForcedYield,
}

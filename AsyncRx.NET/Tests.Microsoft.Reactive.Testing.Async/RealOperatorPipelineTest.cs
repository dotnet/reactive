// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information. 

using System.Reactive.Linq;

using Microsoft.Reactive.Testing.Async;

namespace Tests.Microsoft.Reactive.Testing.Async;

/// <summary>
/// Verifies that the harness hosts real AsyncRx.NET operator code correctly. These are not
/// operator tests: AsyncRx.NET's operators are tested in <c>Tests.System.Reactive.Async</c>.
/// Every other test in this project drives the pump with the harness's own testables, observers
/// and scheduled lambdas; these two are the project's own evidence that completion ownership,
/// inline resumption of <c>ConfigureAwait(false)</c> continuations and the pump's tracking of
/// subscribe and dispose all hold when the code awaiting inside <c>SubscribeAsync</c> and
/// <c>OnNextAsync</c> is the library's, under both execution shapes. The expectations are the
/// ones Rx.NET's <c>TestScheduler</c> produces for the same scenarios, so a divergence here is a
/// harness or library defect, not a test-authoring question.
/// </summary>
[TestClass]
public class RealOperatorPipelineTest : AsyncReactiveTest
{
    /// <summary>
    /// A pipeline of two real operators over a hot source. The point of interest is the teardown
    /// tick: <c>Take</c> completes from within the delivery of its third value and auto-detaches
    /// from the source, and the harness must record that disposal at the same virtual tick as the
    /// delivery (270), not a tick later, under both shapes.
    /// </summary>
    [TestMethod]
    [DataRow(ExecutionShape.SynchronousCompletion)]
    [DataRow(ExecutionShape.ForcedYield)]
    public void Real_operator_pipeline_runs_on_the_pump_and_auto_detaches_at_the_completing_tick(ExecutionShape shape)
    {
        var scheduler = new TestAsyncScheduler(shape);

        var xs = scheduler.CreateHotObservable(
            OnNext(150, 4),
            OnNext(210, 9),
            OnNext(220, -1),
            OnNext(230, 13),
            OnNext(270, 7),
            OnNext(280, 1),
            OnCompleted<int>(690));

        var res = scheduler.Start(() =>
            xs.Where(x => x > 0).Take(3));

        res.Messages.AssertEqual(
            OnNext(210, 9),
            OnNext(230, 13),
            OnNext(270, 7),
            OnCompleted<int>(270));

        xs.Subscriptions.AssertEqual(
            Subscribe(200, 270));
    }

    /// <summary>
    /// An operator that terminates the observer during <c>SubscribeAsync</c> itself, before
    /// subscribing to its source. The harness must record the completion at the subscribe tick
    /// and the source must show no subscription at all.
    /// </summary>
    [TestMethod]
    [DataRow(ExecutionShape.SynchronousCompletion)]
    [DataRow(ExecutionShape.ForcedYield)]
    public void Operator_completing_during_subscribe_records_immediate_completion_and_no_subscription(ExecutionShape shape)
    {
        var scheduler = new TestAsyncScheduler(shape);

        var xs = scheduler.CreateHotObservable(
            OnNext(150, 4),
            OnNext(210, 9),
            OnNext(230, 13));

        var res = scheduler.Start(() =>
            xs.Take(0));

        res.Messages.AssertEqual(
            OnCompleted<int>(200));

        xs.Subscriptions.AssertEmpty();
    }
}

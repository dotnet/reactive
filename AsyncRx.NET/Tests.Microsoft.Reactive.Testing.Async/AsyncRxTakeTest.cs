// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information. 

using System.Reactive.Linq;
using Microsoft.Reactive.Testing.Async;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Tests.Microsoft.Reactive.Testing.Async;

/// <summary>
/// Real AsyncRx.NET operator tests written against the harness, mirroring the Rx.NET
/// TakeTest scenarios. Each runs under both execution shapes; expectations are identical
/// to the sync tests'.
/// </summary>
[TestClass]
public class AsyncRxTakeTest : AsyncReactiveTest
{
    [TestMethod]
    [DataRow(ExecutionShape.SynchronousCompletion)]
    [DataRow(ExecutionShape.ForcedYield)]
    public void Take_Complete_After(ExecutionShape shape)
    {
        var scheduler = new TestAsyncScheduler(shape);

        var xs = scheduler.CreateHotObservable(
            OnNext(70, 6),
            OnNext(150, 4),
            OnNext(210, 9),
            OnNext(230, 13),
            OnNext(270, 7),
            OnNext(280, 1),
            OnNext(300, -1),
            OnNext(310, 3),
            OnNext(340, 8),
            OnNext(370, 11),
            OnCompleted<int>(690));

        var res = scheduler.Start(() =>
            xs.Take(20));

        res.Messages.AssertEqual(
            OnNext(210, 9),
            OnNext(230, 13),
            OnNext(270, 7),
            OnNext(280, 1),
            OnNext(300, -1),
            OnNext(310, 3),
            OnNext(340, 8),
            OnNext(370, 11),
            OnCompleted<int>(690));

        xs.Subscriptions.AssertEqual(
            Subscribe(200, 690));
    }

    [TestMethod]
    [DataRow(ExecutionShape.SynchronousCompletion)]
    [DataRow(ExecutionShape.ForcedYield)]
    public void Take_Complete_Before(ExecutionShape shape)
    {
        var scheduler = new TestAsyncScheduler(shape);

        var xs = scheduler.CreateHotObservable(
            OnNext(70, 6),
            OnNext(150, 4),
            OnNext(210, 9),
            OnNext(230, 13),
            OnNext(270, 7),
            OnNext(280, 1),
            OnNext(300, -1),
            OnCompleted<int>(690));

        var res = scheduler.Start(() =>
            xs.Take(3));

        // Take completes the sequence from within the delivery of the third value, and the
        // auto-detach behaviour tears the source subscription down at that same tick.
        res.Messages.AssertEqual(
            OnNext(210, 9),
            OnNext(230, 13),
            OnNext(270, 7),
            OnCompleted<int>(270));

        xs.Subscriptions.AssertEqual(
            Subscribe(200, 270));
    }

    [TestMethod]
    [DataRow(ExecutionShape.SynchronousCompletion)]
    [DataRow(ExecutionShape.ForcedYield)]
    public void Take_Dispose_Before(ExecutionShape shape)
    {
        var scheduler = new TestAsyncScheduler(shape);

        var xs = scheduler.CreateHotObservable(
            OnNext(70, 6),
            OnNext(150, 4),
            OnNext(210, 9),
            OnNext(230, 13),
            OnNext(270, 7),
            OnNext(280, 1));

        var res = scheduler.Start(() =>
            xs.Take(3),
            250);

        res.Messages.AssertEqual(
            OnNext(210, 9),
            OnNext(230, 13));

        xs.Subscriptions.AssertEqual(
            Subscribe(200, 250));
    }

    [TestMethod]
    [DataRow(ExecutionShape.SynchronousCompletion)]
    [DataRow(ExecutionShape.ForcedYield)]
    public void Take_Error_After(ExecutionShape shape)
    {
        var scheduler = new TestAsyncScheduler(shape);
        var ex = new Exception();

        var xs = scheduler.CreateHotObservable(
            OnNext(210, 9),
            OnNext(230, 13),
            OnError<int>(690, ex));

        var res = scheduler.Start(() =>
            xs.Take(20));

        res.Messages.AssertEqual(
            OnNext(210, 9),
            OnNext(230, 13),
            OnError<int>(690, ex));

        xs.Subscriptions.AssertEqual(
            Subscribe(200, 690));
    }

    [TestMethod]
    [DataRow(ExecutionShape.SynchronousCompletion)]
    [DataRow(ExecutionShape.ForcedYield)]
    public void Take_0_DefaultScheduler(ExecutionShape shape)
    {
        var scheduler = new TestAsyncScheduler(shape);

        var xs = scheduler.CreateHotObservable(
            OnNext(70, 6),
            OnNext(150, 4),
            OnNext(210, 9),
            OnNext(230, 13));

        var res = scheduler.Start(() =>
            xs.Take(0));

        res.Messages.AssertEqual(
            OnCompleted<int>(200)); // Immediate

        xs.Subscriptions.AssertEqual();
    }
}

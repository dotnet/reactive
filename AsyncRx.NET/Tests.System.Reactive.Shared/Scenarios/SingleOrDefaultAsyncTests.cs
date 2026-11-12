// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information. 

namespace Tests.System.Reactive.Shared.Scenarios;

/// <summary>
/// Shared <c>SingleOrDefaultAsync</c> scenarios, from Rx.NET's <c>SingleOrDefaultAsyncTest.cs</c>.
/// </summary>
/// <remarks>
/// 10 of the file's 11 tests (the argument-checking test is the code-generated stratum), all
/// transcribed as they are: the plain and predicate forms over
/// empty, single, several and failing sources, where an empty source or no match yields the
/// default value but more than one element or match fails, and a predicate that throws.
/// </remarks>
public abstract class SingleOrDefaultAsyncTests : SharedReactiveTest
{
    [TestMethod]
    public void SingleOrDefaultAsync_Empty()
    {
        var scheduler = Scheduler;

        var xs = scheduler.CreateHotObservable(
            OnNext(150, 1),
            OnCompleted<int>(250)
        );

        var res = scheduler.Start(() =>
            xs.SingleOrDefaultAsync()
        );

        res.Messages.AssertEqual(
            OnNext(250, 0),
            OnCompleted<int>(250)
        );

        xs.Subscriptions.AssertEqual(
            Subscribe(200, 250)
        );
    }

    [TestMethod]
    public void SingleOrDefaultAsync_One()
    {
        var scheduler = Scheduler;

        var xs = scheduler.CreateHotObservable(
            OnNext(150, 1),
            OnNext(210, 2),
            OnCompleted<int>(250)
        );

        var res = scheduler.Start(() =>
            xs.SingleOrDefaultAsync()
        );

        res.Messages.AssertEqual(
            OnNext(250, 2),
            OnCompleted<int>(250)
        );

        xs.Subscriptions.AssertEqual(
            Subscribe(200, 250)
        );
    }

    [TestMethod]
    public void SingleOrDefaultAsync_Many()
    {
        var scheduler = Scheduler;

        var xs = scheduler.CreateHotObservable(
            OnNext(150, 1),
            OnNext(210, 2),
            OnNext(220, 3),
            OnCompleted<int>(250)
        );

        var res = scheduler.Start(() =>
            xs.SingleOrDefaultAsync()
        );

        res.Messages.AssertEqual(
            OnError<int>(220, e => e is InvalidOperationException)
        );

        xs.Subscriptions.AssertEqual(
            Subscribe(200, 220)
        );
    }

    [TestMethod]
    public void SingleOrDefaultAsync_Error()
    {
        var scheduler = Scheduler;

        var ex = new Exception();

        var xs = scheduler.CreateHotObservable(
            OnNext(150, 1),
            OnError<int>(210, ex)
        );

        var res = scheduler.Start(() =>
            xs.SingleOrDefaultAsync()
        );

        res.Messages.AssertEqual(
            OnError<int>(210, ex)
        );

        xs.Subscriptions.AssertEqual(
            Subscribe(200, 210)
        );
    }

    [TestMethod]
    public void SingleOrDefaultAsync_Predicate()
    {
        var scheduler = Scheduler;

        var ex = new Exception();

        var xs = scheduler.CreateHotObservable(
            OnNext(150, 1),
            OnNext(210, 2),
            OnNext(220, 3),
            OnNext(230, 4),
            OnNext(240, 5),
            OnCompleted<int>(250)
        );

        var res = scheduler.Start(() =>
            xs.SingleOrDefaultAsync(x => x % 2 == 1)
        );

        res.Messages.AssertEqual(
            OnError<int>(240, e => e is InvalidOperationException)
        );

        xs.Subscriptions.AssertEqual(
            Subscribe(200, 240)
        );
    }

    [TestMethod]
    public void SingleOrDefaultAsync_Predicate_Empty()
    {
        var scheduler = Scheduler;

        var ex = new Exception();

        var xs = scheduler.CreateHotObservable(
            OnNext(150, 1),
            OnCompleted<int>(250)
        );

        var res = scheduler.Start(() =>
            xs.SingleOrDefaultAsync(x => x % 2 == 1)
        );

        res.Messages.AssertEqual(
            OnNext(250, 0),
            OnCompleted<int>(250)
        );

        xs.Subscriptions.AssertEqual(
            Subscribe(200, 250)
        );
    }

    [TestMethod]
    public void SingleOrDefaultAsync_Predicate_One()
    {
        var scheduler = Scheduler;

        var ex = new Exception();

        var xs = scheduler.CreateHotObservable(
            OnNext(150, 1),
            OnNext(210, 2),
            OnNext(220, 3),
            OnNext(230, 4),
            OnNext(240, 5),
            OnCompleted<int>(250)
        );

        var res = scheduler.Start(() =>
            xs.SingleOrDefaultAsync(x => x == 4)
        );

        res.Messages.AssertEqual(
            OnNext(250, 4),
            OnCompleted<int>(250)
        );

        xs.Subscriptions.AssertEqual(
            Subscribe(200, 250)
        );
    }

    [TestMethod]
    public void SingleOrDefaultAsync_Predicate_None()
    {
        var scheduler = Scheduler;

        var ex = new Exception();

        var xs = scheduler.CreateHotObservable(
            OnNext(150, 1),
            OnNext(210, 2),
            OnNext(220, 3),
            OnNext(230, 4),
            OnNext(240, 5),
            OnCompleted<int>(250)
        );

        var res = scheduler.Start(() =>
            xs.SingleOrDefaultAsync(x => x > 10)
        );

        res.Messages.AssertEqual(
            OnNext(250, 0),
            OnCompleted<int>(250)
        );

        xs.Subscriptions.AssertEqual(
            Subscribe(200, 250)
        );
    }

    [TestMethod]
    public void SingleOrDefaultAsync_Predicate_Throw()
    {
        var scheduler = Scheduler;

        var ex = new Exception();

        var xs = scheduler.CreateHotObservable(
            OnNext(150, 1),
            OnError<int>(210, ex)
        );

        var res = scheduler.Start(() =>
            xs.SingleOrDefaultAsync(x => x > 10)
        );

        res.Messages.AssertEqual(
            OnError<int>(210, ex)
        );

        xs.Subscriptions.AssertEqual(
            Subscribe(200, 210)
        );
    }

    [TestMethod]
    public void SingleOrDefaultAsync_PredicateThrows()
    {
        var scheduler = Scheduler;

        var ex = new Exception();

        var xs = scheduler.CreateHotObservable(
            OnNext(150, 1),
            OnNext(210, 2),
            OnNext(220, 3),
            OnNext(230, 4),
            OnNext(240, 5),
            OnCompleted<int>(250)
        );

        var res = scheduler.Start(() =>
            xs.SingleOrDefaultAsync(x => { if (x < 4) { return false; } throw ex; })
        );

        res.Messages.AssertEqual(
            OnError<int>(230, ex)
        );

        xs.Subscriptions.AssertEqual(
            Subscribe(200, 230)
        );
    }
}

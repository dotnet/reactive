// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information. 

namespace Tests.System.Reactive.Shared.Scenarios;

/// <summary>
/// Shared <c>FirstOrDefaultAsync</c> scenarios, from Rx.NET's <c>FirstOrDefaultAsyncTest.cs</c>.
/// </summary>
/// <remarks>
/// 8 of the file's 9 tests (the argument-checking test is the code-generated stratum), all
/// transcribed as they are: the plain and predicate forms over empty, single, several and
/// failing sources, and a predicate that throws;
/// an empty source or no match yields the default value.
/// </remarks>
public abstract class FirstOrDefaultAsyncTests : SharedReactiveTest
{
    [TestMethod]
    public void FirstOrDefaultAsync_Empty()
    {
        var scheduler = Scheduler;

        var xs = scheduler.CreateHotObservable(
            OnNext(150, 1),
            OnCompleted<int>(250)
        );

        var res = scheduler.Start(() =>
            xs.FirstOrDefaultAsync()
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
    public void FirstOrDefaultAsync_One()
    {
        var scheduler = Scheduler;

        var xs = scheduler.CreateHotObservable(
            OnNext(150, 1),
            OnNext(210, 2),
            OnCompleted<int>(250)
        );

        var res = scheduler.Start(() =>
            xs.FirstOrDefaultAsync()
        );

        res.Messages.AssertEqual(
            OnNext(210, 2),
            OnCompleted<int>(210)
        );

        xs.Subscriptions.AssertEqual(
            Subscribe(200, 210)
        );
    }

    [TestMethod]
    public void FirstOrDefaultAsync_Many()
    {
        var scheduler = Scheduler;

        var xs = scheduler.CreateHotObservable(
            OnNext(150, 1),
            OnNext(210, 2),
            OnNext(220, 3),
            OnCompleted<int>(250)
        );

        var res = scheduler.Start(() =>
            xs.FirstOrDefaultAsync()
        );

        res.Messages.AssertEqual(
            OnNext(210, 2),
            OnCompleted<int>(210)
        );

        xs.Subscriptions.AssertEqual(
            Subscribe(200, 210)
        );
    }

    [TestMethod]
    public void FirstOrDefaultAsync_Error()
    {
        var scheduler = Scheduler;

        var ex = new Exception();

        var xs = scheduler.CreateHotObservable(
            OnNext(150, 1),
            OnError<int>(210, ex)
        );

        var res = scheduler.Start(() =>
            xs.FirstOrDefaultAsync()
        );

        res.Messages.AssertEqual(
            OnError<int>(210, ex)
        );

        xs.Subscriptions.AssertEqual(
            Subscribe(200, 210)
        );
    }

    [TestMethod]
    public void FirstOrDefaultAsync_Predicate()
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
            xs.FirstOrDefaultAsync(x => x % 2 == 1)
        );

        res.Messages.AssertEqual(
            OnNext(220, 3),
            OnCompleted<int>(220)
        );

        xs.Subscriptions.AssertEqual(
            Subscribe(200, 220)
        );
    }

    [TestMethod]
    public void FirstOrDefaultAsync_Predicate_None()
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
            xs.FirstOrDefaultAsync(x => x > 10)
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
    public void FirstOrDefaultAsync_Predicate_Throw()
    {
        var scheduler = Scheduler;

        var ex = new Exception();

        var xs = scheduler.CreateHotObservable(
            OnNext(150, 1),
            OnNext(210, 2),
            OnError<int>(220, ex)
        );

        var res = scheduler.Start(() =>
            xs.FirstOrDefaultAsync(x => x % 2 == 1)
        );

        res.Messages.AssertEqual(
            OnError<int>(220, ex)
        );

        xs.Subscriptions.AssertEqual(
            Subscribe(200, 220)
        );
    }

    [TestMethod]
    public void FirstOrDefaultAsync_PredicateThrows()
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
            xs.FirstOrDefaultAsync(x => { if (x < 4) { return false; } throw ex; })
        );

        res.Messages.AssertEqual(
            OnError<int>(230, ex)
        );

        xs.Subscriptions.AssertEqual(
            Subscribe(200, 230)
        );
    }
}

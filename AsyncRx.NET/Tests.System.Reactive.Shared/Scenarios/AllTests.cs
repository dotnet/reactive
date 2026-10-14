// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information. 

namespace Tests.System.Reactive.Shared.Scenarios;

/// <summary>Shared <c>All</c> scenarios, from Rx.NET's <c>AllTest.cs</c>.</summary>
/// <remarks>
/// 9 of the file's 10 tests (the <c>All_ArgumentChecking</c> test is the code-generated
/// stratum), all transcribed as they are: over empty, single, several, failing and
/// never-ending sources, where the first element that fails the predicate ends the result with
/// <c>false</c> and completion without one yields <c>true</c>, and a predicate that throws.
/// </remarks>
public abstract class AllTests : SharedReactiveTest
{
    [TestMethod]
    public void All_Empty()
    {
        var scheduler = Scheduler;

        var xs = scheduler.CreateHotObservable(
            OnNext(150, 1),
            OnCompleted<int>(250)
        );

        var res = scheduler.Start(() =>
            xs.All(x => x > 0)
        );

        res.Messages.AssertEqual(
            OnNext(250, true),
            OnCompleted<bool>(250)
        );

        xs.Subscriptions.AssertEqual(
            Subscribe(200, 250)
        );
    }

    [TestMethod]
    public void All_Return()
    {
        var scheduler = Scheduler;

        var xs = scheduler.CreateHotObservable(
            OnNext(150, 1),
            OnNext(210, 2),
            OnCompleted<int>(250)
        );

        var res = scheduler.Start(() =>
            xs.All(x => x > 0)
        );

        res.Messages.AssertEqual(
            OnNext(250, true),
            OnCompleted<bool>(250)
        );

        xs.Subscriptions.AssertEqual(
            Subscribe(200, 250)
        );
    }

    [TestMethod]
    public void All_ReturnNotMatch()
    {
        var scheduler = Scheduler;

        var xs = scheduler.CreateHotObservable(
            OnNext(150, 1),
            OnNext(210, -2),
            OnCompleted<int>(250)
        );

        var res = scheduler.Start(() =>
            xs.All(x => x > 0)
        );

        res.Messages.AssertEqual(
            OnNext(210, false),
            OnCompleted<bool>(210)
        );

        xs.Subscriptions.AssertEqual(
            Subscribe(200, 210)
        );
    }

    [TestMethod]
    public void All_SomeNoneMatch()
    {
        var scheduler = Scheduler;

        var xs = scheduler.CreateHotObservable(
            OnNext(150, 1),
            OnNext(210, -2),
            OnNext(220, -3),
            OnNext(230, -4),
            OnCompleted<int>(250)
        );

        var res = scheduler.Start(() =>
            xs.All(x => x > 0)
        );

        res.Messages.AssertEqual(
            OnNext(210, false),
            OnCompleted<bool>(210)
        );

        xs.Subscriptions.AssertEqual(
            Subscribe(200, 210)
        );
    }

    [TestMethod]
    public void All_SomeMatch()
    {
        var scheduler = Scheduler;

        var xs = scheduler.CreateHotObservable(
            OnNext(150, 1),
            OnNext(210, -2),
            OnNext(220, 3),
            OnNext(230, -4),
            OnCompleted<int>(250)
        );

        var res = scheduler.Start(() =>
            xs.All(x => x > 0)
        );

        res.Messages.AssertEqual(
            OnNext(210, false),
            OnCompleted<bool>(210)
        );

        xs.Subscriptions.AssertEqual(
            Subscribe(200, 210)
        );
    }

    [TestMethod]
    public void All_SomeAllMatch()
    {
        var scheduler = Scheduler;

        var xs = scheduler.CreateHotObservable(
            OnNext(150, 1),
            OnNext(210, 2),
            OnNext(220, 3),
            OnNext(230, 4),
            OnCompleted<int>(250)
        );

        var res = scheduler.Start(() =>
            xs.All(x => x > 0)
        );

        res.Messages.AssertEqual(
            OnNext(250, true),
            OnCompleted<bool>(250)
        );

        xs.Subscriptions.AssertEqual(
            Subscribe(200, 250)
        );
    }

    [TestMethod]
    public void All_Throw()
    {
        var ex = new Exception();

        var scheduler = Scheduler;

        var xs = scheduler.CreateHotObservable(
            OnNext(150, 1),
            OnError<int>(210, ex)
        );

        var res = scheduler.Start(() =>
            xs.All(x => x > 0)
        );

        res.Messages.AssertEqual(
            OnError<bool>(210, ex)
        );

        xs.Subscriptions.AssertEqual(
            Subscribe(200, 210)
        );
    }

    [TestMethod]
    public void All_Never()
    {
        var ex = new Exception();

        var scheduler = Scheduler;

        var xs = scheduler.CreateHotObservable(
            OnNext(150, 1)
        );

        var res = scheduler.Start(() =>
            xs.All(x => x > 0)
        );

        res.Messages.AssertEqual(
        );

        xs.Subscriptions.AssertEqual(
            Subscribe(200, 1000)
        );
    }

    [TestMethod]
    public void All_PredicateThrows()
    {
        var ex = new Exception();

        var scheduler = Scheduler;

        var xs = scheduler.CreateHotObservable(
            OnNext(150, 1),
            OnNext(210, 2),
            OnNext(220, 3),
            OnNext(230, 4),
            OnCompleted<int>(250)
        );

        var res = scheduler.Start(() =>
            xs.All(x => { if (x < 4) { return true; } throw ex; })
        );

        res.Messages.AssertEqual(
            OnError<bool>(230, ex)
        );

        xs.Subscriptions.AssertEqual(
            Subscribe(200, 230)
        );
    }
}

// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information. 

namespace Tests.System.Reactive.Shared.Scenarios;

/// <summary>Shared <c>Any</c> scenarios, from Rx.NET's <c>AnyTest.cs</c>.</summary>
/// <remarks>
/// 12 of the file's 13 tests (the <c>Any_ArgumentChecking</c> test is the code-generated
/// stratum), all transcribed as they are: the plain and predicate forms over empty, single,
/// several, failing and never-ending sources, where the first element (or match) ends the
/// result with <c>true</c> and completion without one yields <c>false</c>, and a predicate
/// that throws.
/// </remarks>
public abstract class AnyTests : SharedReactiveTest
{
    [TestMethod]
    public void Any_Empty()
    {
        var scheduler = Scheduler;

        var xs = scheduler.CreateHotObservable(
            OnNext(150, 1),
            OnCompleted<int>(250)
        );

        var res = scheduler.Start(() =>
            xs.Any()
        );

        res.Messages.AssertEqual(
            OnNext(250, false),
            OnCompleted<bool>(250)
        );

        xs.Subscriptions.AssertEqual(
            Subscribe(200, 250)
        );
    }

    [TestMethod]
    public void Any_Return()
    {
        var scheduler = Scheduler;

        var xs = scheduler.CreateHotObservable(
            OnNext(150, 1),
            OnNext(210, 2),
            OnCompleted<int>(250)
        );

        var res = scheduler.Start(() =>
            xs.Any()
        );

        res.Messages.AssertEqual(
            OnNext(210, true),
            OnCompleted<bool>(210)
        );

        xs.Subscriptions.AssertEqual(
            Subscribe(200, 210)
        );
    }

    [TestMethod]
    public void Any_Throw()
    {
        var ex = new Exception();

        var scheduler = Scheduler;

        var xs = scheduler.CreateHotObservable(
            OnNext(150, 1),
            OnError<int>(210, ex)
        );

        var res = scheduler.Start(() =>
            xs.Any()
        );

        res.Messages.AssertEqual(
            OnError<bool>(210, ex)
        );

        xs.Subscriptions.AssertEqual(
            Subscribe(200, 210)
        );
    }

    [TestMethod]
    public void Any_Never()
    {
        var ex = new Exception();

        var scheduler = Scheduler;

        var xs = scheduler.CreateHotObservable(
            OnNext(150, 1)
        );

        var res = scheduler.Start(() =>
            xs.Any()
        );

        res.Messages.AssertEqual(
        );

        xs.Subscriptions.AssertEqual(
            Subscribe(200, 1000)
        );
    }

    [TestMethod]
    public void Any_Predicate_Empty()
    {
        var scheduler = Scheduler;

        var xs = scheduler.CreateHotObservable(
            OnNext(150, 1),
            OnCompleted<int>(250)
        );

        var res = scheduler.Start(() =>
            xs.Any(x => x > 0)
        );

        res.Messages.AssertEqual(
            OnNext(250, false),
            OnCompleted<bool>(250)
        );

        xs.Subscriptions.AssertEqual(
            Subscribe(200, 250)
        );
    }

    [TestMethod]
    public void Any_Predicate_Return()
    {
        var scheduler = Scheduler;

        var xs = scheduler.CreateHotObservable(
            OnNext(150, 1),
            OnNext(210, 2),
            OnCompleted<int>(250)
        );

        var res = scheduler.Start(() =>
            xs.Any(x => x > 0)
        );

        res.Messages.AssertEqual(
            OnNext(210, true),
            OnCompleted<bool>(210)
        );

        xs.Subscriptions.AssertEqual(
            Subscribe(200, 210)
        );
    }

    [TestMethod]
    public void Any_Predicate_ReturnNotMatch()
    {
        var scheduler = Scheduler;

        var xs = scheduler.CreateHotObservable(
            OnNext(150, 1),
            OnNext(210, -2),
            OnCompleted<int>(250)
        );

        var res = scheduler.Start(() =>
            xs.Any(x => x > 0)
        );

        res.Messages.AssertEqual(
            OnNext(250, false),
            OnCompleted<bool>(250)
        );

        xs.Subscriptions.AssertEqual(
            Subscribe(200, 250)
        );
    }

    [TestMethod]
    public void Any_Predicate_SomeNoneMatch()
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
            xs.Any(x => x > 0)
        );

        res.Messages.AssertEqual(
            OnNext(250, false),
            OnCompleted<bool>(250)
        );

        xs.Subscriptions.AssertEqual(
            Subscribe(200, 250)
        );
    }

    [TestMethod]
    public void Any_Predicate_SomeMatch()
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
            xs.Any(x => x > 0)
        );

        res.Messages.AssertEqual(
            OnNext(220, true),
            OnCompleted<bool>(220)
        );

        xs.Subscriptions.AssertEqual(
            Subscribe(200, 220)
        );
    }

    [TestMethod]
    public void Any_Predicate_Throw()
    {
        var ex = new Exception();

        var scheduler = Scheduler;

        var xs = scheduler.CreateHotObservable(
            OnNext(150, 1),
            OnError<int>(210, ex)
        );

        var res = scheduler.Start(() =>
            xs.Any(x => x > 0)
        );

        res.Messages.AssertEqual(
            OnError<bool>(210, ex)
        );

        xs.Subscriptions.AssertEqual(
            Subscribe(200, 210)
        );
    }

    [TestMethod]
    public void Any_Predicate_Never()
    {
        var scheduler = Scheduler;

        var xs = scheduler.CreateHotObservable(
            OnNext(150, 1)
        );

        var res = scheduler.Start(() =>
            xs.Any(x => x > 0)
        );

        res.Messages.AssertEqual(
        );

        xs.Subscriptions.AssertEqual(
            Subscribe(200, 1000)
        );
    }

    [TestMethod]
    public void Any_Predicate_PredicateThrows()
    {
        var ex = new Exception();

        var scheduler = Scheduler;

        var xs = scheduler.CreateHotObservable(
            OnNext(150, 1),
            OnNext(210, -2),
            OnNext(220, 3),
            OnNext(230, -4),
            OnCompleted<int>(250)
        );

        var res = scheduler.Start(() =>
            xs.Any(x => { if (x != -4) { return false; } throw ex; })
        );

        res.Messages.AssertEqual(
            OnError<bool>(230, ex)
        );

        xs.Subscriptions.AssertEqual(
            Subscribe(200, 230)
        );
    }
}

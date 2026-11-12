// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information. 

namespace Tests.System.Reactive.Shared.Scenarios;

/// <summary>Shared <c>LongCount</c> scenarios, from Rx.NET's <c>LongCountTest.cs</c>.</summary>
/// <remarks>
/// 16 of the file's 19 tests, all transcribed as they are: the plain and predicate forms over
/// empty, single, several, failing and never-ending sources, and a predicate that throws. Not
/// here: <c>LongCount_ArgumentChecking</c> (the code-generated stratum) and the two
/// <c>*_InjectOverflow</c> tests, which set the running count inside Rx.NET's sink by
/// reflection and so are Rx.NET's own, in its runner, as the <c>Average</c> ones are; the count
/// cannot overflow any other way, so there is no shared shape with the same effect.
/// </remarks>
public abstract class LongCountTests : SharedReactiveTest
{
    [TestMethod]
    public void LongCount_Empty()
    {
        var scheduler = Scheduler;

        var xs = scheduler.CreateHotObservable(
            OnNext(150, 1),
            OnCompleted<int>(250)
        );

        var res = scheduler.Start(() =>
            xs.LongCount()
        );

        res.Messages.AssertEqual(
            OnNext(250, 0L),
            OnCompleted<long>(250)
        );

        xs.Subscriptions.AssertEqual(
            Subscribe(200, 250)
        );
    }

    [TestMethod]
    public void LongCount_Return()
    {
        var scheduler = Scheduler;

        var xs = scheduler.CreateHotObservable(
            OnNext(150, 1),
            OnNext(210, 2),
            OnCompleted<int>(250)
        );

        var res = scheduler.Start(() =>
            xs.LongCount()
        );

        res.Messages.AssertEqual(
            OnNext(250, 1L),
            OnCompleted<long>(250)
        );

        xs.Subscriptions.AssertEqual(
            Subscribe(200, 250)
        );
    }

    [TestMethod]
    public void LongCount_Some()
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
            xs.LongCount()
        );

        res.Messages.AssertEqual(
            OnNext(250, 3L),
            OnCompleted<long>(250)
        );

        xs.Subscriptions.AssertEqual(
            Subscribe(200, 250)
        );
    }

    [TestMethod]
    public void LongCount_Throw()
    {
        var ex = new Exception();

        var scheduler = Scheduler;

        var xs = scheduler.CreateHotObservable(
            OnNext(150, 1),
            OnError<int>(210, ex)
        );

        var res = scheduler.Start(() =>
            xs.LongCount()
        );

        res.Messages.AssertEqual(
            OnError<long>(210, ex)
        );

        xs.Subscriptions.AssertEqual(
            Subscribe(200, 210)
        );
    }

    [TestMethod]
    public void LongCount_Never()
    {
        var scheduler = Scheduler;

        var xs = scheduler.CreateHotObservable(
            OnNext(150, 1)
        );

        var res = scheduler.Start(() =>
            xs.LongCount()
        );

        res.Messages.AssertEqual(
        );

        xs.Subscriptions.AssertEqual(
            Subscribe(200, 1000)
        );
    }

    [TestMethod]
    public void LongCount_Predicate_Empty_True()
    {
        var scheduler = Scheduler;

        var xs = scheduler.CreateHotObservable(
            OnNext(150, 1),
            OnCompleted<int>(250)
        );

        var res = scheduler.Start(() =>
            xs.LongCount(_ => true)
        );

        res.Messages.AssertEqual(
            OnNext(250, 0L),
            OnCompleted<long>(250)
        );

        xs.Subscriptions.AssertEqual(
            Subscribe(200, 250)
        );
    }

    [TestMethod]
    public void LongCount_Predicate_Empty_False()
    {
        var scheduler = Scheduler;

        var xs = scheduler.CreateHotObservable(
            OnNext(150, 1),
            OnCompleted<int>(250)
        );

        var res = scheduler.Start(() =>
            xs.LongCount(_ => false)
        );

        res.Messages.AssertEqual(
            OnNext(250, 0L),
            OnCompleted<long>(250)
        );

        xs.Subscriptions.AssertEqual(
            Subscribe(200, 250)
        );
    }

    [TestMethod]
    public void LongCount_Predicate_Return_True()
    {
        var scheduler = Scheduler;

        var xs = scheduler.CreateHotObservable(
            OnNext(150, 1),
            OnNext(210, 2),
            OnCompleted<int>(250)
        );

        var res = scheduler.Start(() =>
            xs.LongCount(_ => true)
        );

        res.Messages.AssertEqual(
            OnNext(250, 1L),
            OnCompleted<long>(250)
        );

        xs.Subscriptions.AssertEqual(
            Subscribe(200, 250)
        );
    }

    [TestMethod]
    public void LongCount_Predicate_Return_False()
    {
        var scheduler = Scheduler;

        var xs = scheduler.CreateHotObservable(
            OnNext(150, 1),
            OnNext(210, 2),
            OnCompleted<int>(250)
        );

        var res = scheduler.Start(() =>
            xs.LongCount(_ => false)
        );

        res.Messages.AssertEqual(
            OnNext(250, 0L),
            OnCompleted<long>(250)
        );

        xs.Subscriptions.AssertEqual(
            Subscribe(200, 250)
        );
    }

    [TestMethod]
    public void LongCount_Predicate_Some_All()
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
            xs.LongCount(x => x < 10)
        );

        res.Messages.AssertEqual(
            OnNext(250, 3L),
            OnCompleted<long>(250)
        );

        xs.Subscriptions.AssertEqual(
            Subscribe(200, 250)
        );
    }

    [TestMethod]
    public void LongCount_Predicate_Some_None()
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
            xs.LongCount(x => x > 10)
        );

        res.Messages.AssertEqual(
            OnNext(250, 0L),
            OnCompleted<long>(250)
        );

        xs.Subscriptions.AssertEqual(
            Subscribe(200, 250)
        );
    }

    [TestMethod]
    public void LongCount_Predicate_Some_Even()
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
            xs.LongCount(x => x % 2 == 0)
        );

        res.Messages.AssertEqual(
            OnNext(250, 2L),
            OnCompleted<long>(250)
        );

        xs.Subscriptions.AssertEqual(
            Subscribe(200, 250)
        );
    }

    [TestMethod]
    public void LongCount_Predicate_Throw_True()
    {
        var ex = new Exception();

        var scheduler = Scheduler;

        var xs = scheduler.CreateHotObservable(
            OnNext(150, 1),
            OnError<int>(210, ex)
        );

        var res = scheduler.Start(() =>
            xs.LongCount(_ => true)
        );

        res.Messages.AssertEqual(
            OnError<long>(210, ex)
        );

        xs.Subscriptions.AssertEqual(
            Subscribe(200, 210)
        );
    }

    [TestMethod]
    public void LongCount_Predicate_Throw_False()
    {
        var ex = new Exception();

        var scheduler = Scheduler;

        var xs = scheduler.CreateHotObservable(
            OnNext(150, 1),
            OnError<int>(210, ex)
        );

        var res = scheduler.Start(() =>
            xs.LongCount(_ => false)
        );

        res.Messages.AssertEqual(
            OnError<long>(210, ex)
        );

        xs.Subscriptions.AssertEqual(
            Subscribe(200, 210)
        );
    }

    [TestMethod]
    public void LongCount_Predicate_Never()
    {
        var scheduler = Scheduler;

        var xs = scheduler.CreateHotObservable(
            OnNext(150, 1)
        );

        var res = scheduler.Start(() =>
            xs.LongCount(_ => true)
        );

        res.Messages.AssertEqual(
        );

        xs.Subscriptions.AssertEqual(
            Subscribe(200, 1000)
        );
    }

    [TestMethod]
    public void LongCount_Predicate_PredicateThrows()
    {
        var scheduler = Scheduler;

        var xs = scheduler.CreateHotObservable(
            OnNext(150, 1),
            OnNext(210, 2),
            OnNext(230, 3),
            OnCompleted<int>(240)
        );

        var ex = new Exception();

        var res = scheduler.Start(() =>
            xs.LongCount(x =>
            {
                if (x == 3)
                {
                    throw ex;
                }

                return true;
            })
        );

        res.Messages.AssertEqual(
            OnError<long>(230, ex)
        );

        xs.Subscriptions.AssertEqual(
            Subscribe(200, 230)
        );
    }
}

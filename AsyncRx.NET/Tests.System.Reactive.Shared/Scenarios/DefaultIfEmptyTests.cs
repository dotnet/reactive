// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information. 

namespace Tests.System.Reactive.Shared.Scenarios;

/// <summary>
/// Shared <c>DefaultIfEmpty</c> scenarios, from Rx.NET's <c>DefaultIfEmptyTest.cs</c>.
/// </summary>
/// <remarks>
/// 6 of the file's 7 tests (the <c>DefaultIfEmpty_ArgumentChecking</c> test is the
/// code-generated stratum), all transcribed as they are: both forms over a source with
/// elements, an empty source (which yields the type's default or the given value) and a failing
/// one.
/// </remarks>
public abstract class DefaultIfEmptyTests : SharedReactiveTest
{
    [TestMethod]
    public void DefaultIfEmpty_NonEmpty1()
    {
        var scheduler = Scheduler;

        var xs = scheduler.CreateHotObservable(
            OnNext(280, 42),
            OnNext(360, 43),
            OnCompleted<int>(420)
        );

        var res = scheduler.Start(() =>
            xs.DefaultIfEmpty()
        );

        res.Messages.AssertEqual(
            OnNext(280, 42),
            OnNext(360, 43),
            OnCompleted<int>(420)
        );

        xs.Subscriptions.AssertEqual(
            Subscribe(200, 420)
        );
    }

    [TestMethod]
    public void DefaultIfEmpty_NonEmpty2()
    {
        var scheduler = Scheduler;

        var xs = scheduler.CreateHotObservable(
            OnNext(280, 42),
            OnNext(360, 43),
            OnCompleted<int>(420)
        );

        var res = scheduler.Start(() =>
            xs.DefaultIfEmpty(-1)
        );

        res.Messages.AssertEqual(
            OnNext(280, 42),
            OnNext(360, 43),
            OnCompleted<int>(420)
        );

        xs.Subscriptions.AssertEqual(
            Subscribe(200, 420)
        );
    }

    [TestMethod]
    public void DefaultIfEmpty_Empty1()
    {
        var scheduler = Scheduler;

        var xs = scheduler.CreateHotObservable(
            OnCompleted<int>(420)
        );

        var res = scheduler.Start(() =>
            xs.DefaultIfEmpty()
        );

        res.Messages.AssertEqual(
            OnNext(420, 0),
            OnCompleted<int>(420)
        );

        xs.Subscriptions.AssertEqual(
            Subscribe(200, 420)
        );
    }

    [TestMethod]
    public void DefaultIfEmpty_Empty2()
    {
        var scheduler = Scheduler;

        var xs = scheduler.CreateHotObservable(
            OnCompleted<int>(420)
        );

        var res = scheduler.Start(() =>
            xs.DefaultIfEmpty(-1)
        );

        res.Messages.AssertEqual(
            OnNext(420, -1),
            OnCompleted<int>(420)
        );

        xs.Subscriptions.AssertEqual(
            Subscribe(200, 420)
        );
    }

    [TestMethod]
    public void DefaultIfEmpty_Throw1()
    {
        var ex = new Exception();

        var scheduler = Scheduler;

        var xs = scheduler.CreateHotObservable(
            OnError<int>(420, ex)
        );

        var res = scheduler.Start(() =>
            xs.DefaultIfEmpty()
        );

        res.Messages.AssertEqual(
            OnError<int>(420, ex)
        );

        xs.Subscriptions.AssertEqual(
            Subscribe(200, 420)
        );
    }

    [TestMethod]
    public void DefaultIfEmpty_Throw2()
    {
        var ex = new Exception();

        var scheduler = Scheduler;

        var xs = scheduler.CreateHotObservable(
            OnError<int>(420, ex)
        );

        var res = scheduler.Start(() =>
            xs.DefaultIfEmpty(42)
        );

        res.Messages.AssertEqual(
            OnError<int>(420, ex)
        );

        xs.Subscriptions.AssertEqual(
            Subscribe(200, 420)
        );
    }
}

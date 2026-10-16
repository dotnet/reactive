// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information. 

namespace Tests.System.Reactive.Shared.Scenarios;

/// <summary>Shared <c>IsEmpty</c> scenarios, from Rx.NET's <c>IsEmptyTest.cs</c>.</summary>
/// <remarks>
/// 4 of the file's 5 tests (the <c>IsEmpty_ArgumentChecking</c> test is the code-generated
/// stratum), all transcribed as they are: an empty, a single-element, a failing and a
/// never-ending source.
/// </remarks>
public abstract class IsEmptyTests : SharedReactiveTest
{
    [TestMethod]
    public void IsEmpty_Empty()
    {
        var scheduler = Scheduler;

        var xs = scheduler.CreateHotObservable(
            OnNext(150, 1),
            OnCompleted<int>(250)
        );

        var res = scheduler.Start(() =>
            xs.IsEmpty()
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
    public void IsEmpty_Return()
    {
        var scheduler = Scheduler;

        var xs = scheduler.CreateHotObservable(
            OnNext(150, 1),
            OnNext(210, 2),
            OnCompleted<int>(250)
        );

        var res = scheduler.Start(() =>
            xs.IsEmpty()
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
    public void IsEmpty_Throw()
    {
        var ex = new Exception();

        var scheduler = Scheduler;

        var xs = scheduler.CreateHotObservable(
            OnNext(150, 1),
            OnError<int>(210, ex)
        );

        var res = scheduler.Start(() =>
            xs.IsEmpty()
        );

        res.Messages.AssertEqual(
            OnError<bool>(210, ex)
        );

        xs.Subscriptions.AssertEqual(
            Subscribe(200, 210)
        );
    }

    [TestMethod]
    public void IsEmpty_Never()
    {
        var ex = new Exception();

        var scheduler = Scheduler;

        var xs = scheduler.CreateHotObservable(
            OnNext(150, 1)
        );

        var res = scheduler.Start(() =>
            xs.IsEmpty()
        );

        res.Messages.AssertEqual(
        );

        xs.Subscriptions.AssertEqual(
            Subscribe(200, 1000)
        );
    }
}

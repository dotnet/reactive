// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information. 

namespace Tests.System.Reactive.Shared.Scenarios;

/// <summary>
/// Shared <c>ElementAt</c> scenarios, from Rx.NET's <c>ElementAtTest.cs</c>.
/// </summary>
/// <remarks>
/// 4 of the file's 5 tests (the argument-checking test is the code-generated stratum), all
/// transcribed as they are: the first element, a later one, an index past the end (which
/// fails with <c>ArgumentOutOfRangeException</c> when the source completes), and a failing source.
/// </remarks>
public abstract class ElementAtTests : SharedReactiveTest
{
    [TestMethod]
    public void ElementAt_First()
    {
        var scheduler = Scheduler;

        var xs = scheduler.CreateHotObservable(
            OnNext(280, 42),
            OnNext(360, 43),
            OnNext(470, 44),
            OnCompleted<int>(600)
        );

        var res = scheduler.Start(() =>
            xs.ElementAt(0)
        );

        res.Messages.AssertEqual(
            OnNext(280, 42),
            OnCompleted<int>(280)
        );

        xs.Subscriptions.AssertEqual(
            Subscribe(200, 280)
        );
    }

    [TestMethod]
    public void ElementAt_Other()
    {
        var scheduler = Scheduler;

        var xs = scheduler.CreateHotObservable(
            OnNext(280, 42),
            OnNext(360, 43),
            OnNext(470, 44),
            OnCompleted<int>(600)
        );

        var res = scheduler.Start(() =>
            xs.ElementAt(2)
        );

        res.Messages.AssertEqual(
            OnNext(470, 44),
            OnCompleted<int>(470)
        );

        xs.Subscriptions.AssertEqual(
            Subscribe(200, 470)
        );
    }

    [TestMethod]
    public void ElementAt_OutOfRange()
    {
        var scheduler = Scheduler;

        var xs = scheduler.CreateHotObservable(
            OnNext(280, 42),
            OnNext(360, 43),
            OnNext(470, 44),
            OnCompleted<int>(600)
        );

        var res = scheduler.Start(() =>
            xs.ElementAt(3)
        );

        res.Messages.AssertEqual(
            OnError<int>(600, e => e is ArgumentOutOfRangeException)
        );
    }

    [TestMethod]
    public void ElementAt_Error()
    {
        var scheduler = Scheduler;

        var ex = new Exception();

        var xs = scheduler.CreateHotObservable(
            OnNext(280, 42),
            OnNext(360, 43),
            OnError<int>(420, ex)
        );

        var res = scheduler.Start(() =>
            xs.ElementAt(3)
        );

        res.Messages.AssertEqual(
            OnError<int>(420, ex)
        );

        xs.Subscriptions.AssertEqual(
            Subscribe(200, 420)
        );
    }
}

// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information. 

namespace Tests.System.Reactive.Shared.Scenarios;

/// <summary>
/// Shared <c>ElementAtOrDefault</c> scenarios, from Rx.NET's <c>ElementAtOrDefaultTest.cs</c>.
/// </summary>
/// <remarks>
/// 4 of the file's 5 tests (the argument-checking test is the code-generated stratum), all
/// transcribed as they are: the first element, a later one, an index past the end (which
/// yields the default value when the source completes), and a failing source.
/// </remarks>
public abstract class ElementAtOrDefaultTests : SharedReactiveTest
{
    [TestMethod]
    public void ElementAtOrDefault_First()
    {
        var scheduler = Scheduler;

        var xs = scheduler.CreateHotObservable(
            OnNext(280, 42),
            OnNext(360, 43),
            OnNext(470, 44),
            OnCompleted<int>(600)
        );

        var res = scheduler.Start(() =>
            xs.ElementAtOrDefault(0)
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
    public void ElementAtOrDefault_Other()
    {
        var scheduler = Scheduler;

        var xs = scheduler.CreateHotObservable(
            OnNext(280, 42),
            OnNext(360, 43),
            OnNext(470, 44),
            OnCompleted<int>(600)
        );

        var res = scheduler.Start(() =>
            xs.ElementAtOrDefault(2)
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
    public void ElementAtOrDefault_OutOfRange()
    {
        var scheduler = Scheduler;

        var xs = scheduler.CreateHotObservable(
            OnNext(280, 42),
            OnNext(360, 43),
            OnNext(470, 44),
            OnCompleted<int>(600)
        );

        var res = scheduler.Start(() =>
            xs.ElementAtOrDefault(3)
        );

        res.Messages.AssertEqual(
            OnNext(600, 0),
            OnCompleted<int>(600)
        );

        xs.Subscriptions.AssertEqual(
            Subscribe(200, 600)
        );
    }

    [TestMethod]
    public void ElementAtOrDefault_Error()
    {
        var scheduler = Scheduler;

        var ex = new Exception();

        var xs = scheduler.CreateHotObservable(
            OnNext(280, 42),
            OnNext(360, 43),
            OnError<int>(420, ex)
        );

        var res = scheduler.Start(() =>
            xs.ElementAtOrDefault(3)
        );

        res.Messages.AssertEqual(
            OnError<int>(420, ex)
        );

        xs.Subscriptions.AssertEqual(
            Subscribe(200, 420)
        );
    }
}

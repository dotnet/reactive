// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information. 

using System.Reactive;

namespace Tests.System.Reactive.Shared.Scenarios;

/// <summary>
/// Shared <c>Dematerialize</c> scenarios, from Rx.NET's <c>DematerializeTest.cs</c>.
/// </summary>
/// <remarks>
/// 8 of the file's 9 tests (the <c>Dematerialize_ArgumentChecking</c> test is the
/// code-generated stratum), all transcribed as they are (with <c>Seq.Never</c> for
/// <c>Observable.Never</c>): sources of notifications ending by completion, by an error
/// notification or by an error of their own, and the round trip through <c>Materialize</c>
/// for a never-ending, an empty, a single-element and a failing source.
/// </remarks>
public abstract class DematerializeTests : SharedReactiveTest
{
    [TestMethod]
    public void Dematerialize_Range1()
    {
        var scheduler = Scheduler;

        var xs = scheduler.CreateHotObservable(
            OnNext(150, Notification.CreateOnNext(41)),
            OnNext(210, Notification.CreateOnNext(42)),
            OnNext(220, Notification.CreateOnNext(43)),
            OnCompleted<Notification<int>>(250)
        );
        var res = scheduler.Start(() =>
            xs.Dematerialize()
        );

        res.Messages.AssertEqual(
            OnNext(210, 42),
            OnNext(220, 43),
            OnCompleted<int>(250)
        );

        xs.Subscriptions.AssertEqual(
            Subscribe(200, 250)
        );
    }

    [TestMethod]
    public void Dematerialize_Range2()
    {
        var scheduler = Scheduler;

        var xs = scheduler.CreateHotObservable(
            OnNext(150, Notification.CreateOnNext(41)),
            OnNext(210, Notification.CreateOnNext(42)),
            OnNext(220, Notification.CreateOnNext(43)),
            OnNext(230, Notification.CreateOnCompleted<int>())
        );
        var res = scheduler.Start(() =>
            xs.Dematerialize()
        );

        res.Messages.AssertEqual(
            OnNext(210, 42),
            OnNext(220, 43),
            OnCompleted<int>(230)
        );

        xs.Subscriptions.AssertEqual(
            Subscribe(200, 230)
        );
    }

    [TestMethod]
    public void Dematerialize_Error1()
    {
        var scheduler = Scheduler;

        var ex = new Exception();

        var xs = scheduler.CreateHotObservable(
            OnNext(150, Notification.CreateOnNext(41)),
            OnNext(210, Notification.CreateOnNext(42)),
            OnNext(220, Notification.CreateOnNext(43)),
            OnError<Notification<int>>(230, ex)
        );
        var res = scheduler.Start(() =>
            xs.Dematerialize()
        );

        res.Messages.AssertEqual(
            OnNext(210, 42),
            OnNext(220, 43),
            OnError<int>(230, ex)
        );

        xs.Subscriptions.AssertEqual(
            Subscribe(200, 230)
        );
    }

    [TestMethod]
    public void Dematerialize_Error2()
    {
        var scheduler = Scheduler;

        var ex = new Exception();

        var xs = scheduler.CreateHotObservable(
            OnNext(150, Notification.CreateOnNext(41)),
            OnNext(210, Notification.CreateOnNext(42)),
            OnNext(220, Notification.CreateOnNext(43)),
            OnNext(230, Notification.CreateOnError<int>(ex))
        );
        var res = scheduler.Start(() =>
            xs.Dematerialize()
        );

        res.Messages.AssertEqual(
            OnNext(210, 42),
            OnNext(220, 43),
            OnError<int>(230, ex)
        );

        xs.Subscriptions.AssertEqual(
            Subscribe(200, 230)
        );
    }

    [TestMethod]
    public void Materialize_Dematerialize_Never()
    {
        var scheduler = Scheduler;

        var xs = Seq.Never<int>();

        var res = scheduler.Start(() =>
            xs.Materialize().Dematerialize()
        );

        res.Messages.AssertEqual(
        );
    }

    [TestMethod]
    public void Materialize_Dematerialize_Empty()
    {
        var scheduler = Scheduler;

        var xs = scheduler.CreateHotObservable(
            OnNext(150, 1),
            OnCompleted<int>(250)
        );

        var res = scheduler.Start(() =>
            xs.Materialize().Dematerialize()
        );

        res.Messages.AssertEqual(
            OnCompleted<int>(250)
        );

        xs.Subscriptions.AssertEqual(
            Subscribe(200, 250)
        );
    }

    [TestMethod]
    public void Materialize_Dematerialize_Return()
    {
        var scheduler = Scheduler;

        var xs = scheduler.CreateHotObservable(
            OnNext(150, 1),
            OnNext(210, 2),
            OnCompleted<int>(250)
        );

        var res = scheduler.Start(() =>
            xs.Materialize().Dematerialize()
        );

        res.Messages.AssertEqual(
            OnNext(210, 2),
            OnCompleted<int>(250)
        );

        xs.Subscriptions.AssertEqual(
            Subscribe(200, 250)
        );
    }

    [TestMethod]
    public void Materialize_Dematerialize_Throw()
    {
        var scheduler = Scheduler;

        var ex = new Exception();

        var xs = scheduler.CreateHotObservable(
            OnNext(150, 1),
            OnError<int>(250, ex)
        );

        var res = scheduler.Start(() =>
            xs.Materialize().Dematerialize()
        );

        res.Messages.AssertEqual(
            OnError<int>(250, ex)
        );

        xs.Subscriptions.AssertEqual(
            Subscribe(200, 250)
        );
    }
}

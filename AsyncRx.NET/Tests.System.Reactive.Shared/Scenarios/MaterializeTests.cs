// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information. 

using System.Reactive;

namespace Tests.System.Reactive.Shared.Scenarios;

/// <summary>
/// Shared <c>Materialize</c> scenarios, from Rx.NET's <c>MaterializeTest.cs</c>.
/// </summary>
/// <remarks>
/// 4 of the file's 5 tests (the <c>Materialize_ArgumentChecking</c> test is the code-generated
/// stratum), all transcribed as they are (with <c>Seq.Never</c> for <c>Observable.Never</c>):
/// a never-ending, an empty, a single-element and a failing source, whose notifications become
/// elements and whose end, error included, becomes a completion.
/// </remarks>
public abstract class MaterializeTests : SharedReactiveTest
{
    [TestMethod]
    public void Materialize_Never()
    {
        var scheduler = Scheduler;
        var res = scheduler.Start(() =>
            Seq.Never<int>().Materialize()
        );

        res.Messages.AssertEqual(
        );
    }

    [TestMethod]
    public void Materialize_Empty()
    {
        var scheduler = Scheduler;

        var xs = scheduler.CreateHotObservable(
            OnNext(150, 1),
            OnCompleted<int>(250)
        );

        var res = scheduler.Start(() =>
            xs.Materialize()
        );

        res.Messages.AssertEqual(
            OnNext(250, Notification.CreateOnCompleted<int>()),
            OnCompleted<Notification<int>>(250)
        );

        xs.Subscriptions.AssertEqual(
            Subscribe(200, 250)
        );
    }

    [TestMethod]
    public void Materialize_Return()
    {
        var scheduler = Scheduler;

        var xs = scheduler.CreateHotObservable(
            OnNext(150, 1),
            OnNext(210, 2),
            OnCompleted<int>(250)
        );

        var res = scheduler.Start(() =>
            xs.Materialize()
        );

        res.Messages.AssertEqual(
            OnNext(210, Notification.CreateOnNext(2)),
            OnNext(250, Notification.CreateOnCompleted<int>()),
            OnCompleted<Notification<int>>(250)
        );

        xs.Subscriptions.AssertEqual(
            Subscribe(200, 250)
        );
    }

    [TestMethod]
    public void Materialize_Throw()
    {
        var scheduler = Scheduler;

        var ex = new Exception();

        var xs = scheduler.CreateHotObservable(
            OnNext(150, 1),
            OnError<int>(250, ex)
        );

        var res = scheduler.Start(() =>
            xs.Materialize()
        );

        res.Messages.AssertEqual(
            OnNext(250, Notification.CreateOnError<int>(ex)),
            OnCompleted<Notification<int>>(250)
        );

        xs.Subscriptions.AssertEqual(
            Subscribe(200, 250)
        );
    }
}

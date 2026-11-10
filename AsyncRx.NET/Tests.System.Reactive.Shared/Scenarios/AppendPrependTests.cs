// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information. 

namespace Tests.System.Reactive.Shared.Scenarios;

/// <summary>
/// Shared <c>Append</c>/<c>Prepend</c> chain scenarios, from Rx.NET's <c>AppendPrependTest.cs</c>.
/// </summary>
/// <remarks>
/// 11 of the file's 13 tests, transcribed as they are: the two operators chained in both orders,
/// in threes, mixed, with and without a scheduler, and with schedulers on some links only. Where
/// the original expects a scheduled value one tick after the subscription or the completion, with
/// each link's hop a tick later again, the scenario spells it with <c>ScheduledAt</c>, as the
/// <c>Append</c> and <c>Prepend</c> scenarios do (Step 61). The two real-time tests over an
/// <c>EventLoopScheduler</c> and a long-running <c>NewThreadScheduler</c>, schedulers AsyncRx.NET
/// does not have, are the Rx.NET runner's.
/// </remarks>
public abstract class AppendPrependTests : SharedReactiveTest
{
    [TestMethod]
    public void AppendPrepend()
    {

        var xs = Scheduler.CreateHotObservable(
            OnNext(150, 1),
            OnNext(220, 2),
            OnCompleted<int>(250)
        );

        var res = Scheduler.Start(() =>
            xs.Append(3).Prepend(4)
        );

        res.Messages.AssertEqual(
            OnNext(200, 4),
            OnNext(220, 2),
            OnNext(250, 3),
            OnCompleted<int>(250)
        );
    }

    [TestMethod]
    public void PrependAppend()
    {

        var xs = Scheduler.CreateHotObservable(
            OnNext(150, 1),
            OnNext(220, 2),
            OnCompleted<int>(250)
        );

        var res = Scheduler.Start(() =>
            xs.Prepend(4).Append(3)
        );

        res.Messages.AssertEqual(
            OnNext(200, 4),
            OnNext(220, 2),
            OnNext(250, 3),
            OnCompleted<int>(250)
        );
    }

    [TestMethod]
    public void AppendPrepend_Scheduler()
    {

        var xs = Scheduler.CreateHotObservable(
            OnNext(150, 1),
            OnNext(220, 2),
            OnCompleted<int>(250)
        );

        var res = Scheduler.Start(() =>
            xs.Append(3, Scheduler).Prepend(4, Scheduler)
        );

        res.Messages.AssertEqual(
            OnNext(ScheduledAt(200), 4),
            OnNext(220, 2),
            OnNext(ScheduledAt(250), 3),
            OnCompleted<int>(ScheduledAt(250))
        );
    }

    [TestMethod]
    public void PrependAppend_Scheduler()
    {

        var xs = Scheduler.CreateHotObservable(
            OnNext(150, 1),
            OnNext(220, 2),
            OnCompleted<int>(250)
        );

        var res = Scheduler.Start(() =>
            xs.Prepend(4, Scheduler).Append(3, Scheduler)
        );

        res.Messages.AssertEqual(
            OnNext(ScheduledAt(200), 4),
            OnNext(220, 2),
            OnNext(ScheduledAt(250), 3),
            OnCompleted<int>(ScheduledAt(250))
        );
    }

    [TestMethod]
    public void AppendPrepend_Many()
    {

        var xs = Scheduler.CreateHotObservable(
            OnNext(150, 1),
            OnNext(220, 2),
            OnCompleted<int>(250)
        );

        var res = Scheduler.Start(() =>
            xs.Append(3).Append(4).Append(5).Prepend(6).Prepend(7).Prepend(8)
        );

        res.Messages.AssertEqual(
            OnNext(200, 8),
            OnNext(200, 7),
            OnNext(200, 6),
            OnNext(220, 2),
            OnNext(250, 3),
            OnNext(250, 4),
            OnNext(250, 5),
            OnCompleted<int>(250)
        );
    }

    [TestMethod]
    public void PrependAppend_Many()
    {

        var xs = Scheduler.CreateHotObservable(
            OnNext(150, 1),
            OnNext(220, 2),
            OnCompleted<int>(250)
        );

        var res = Scheduler.Start(() =>
            xs.Prepend(6).Prepend(7).Prepend(8).Append(3).Append(4).Append(5)
        );

        res.Messages.AssertEqual(
            OnNext(200, 8),
            OnNext(200, 7),
            OnNext(200, 6),
            OnNext(220, 2),
            OnNext(250, 3),
            OnNext(250, 4),
            OnNext(250, 5),
            OnCompleted<int>(250)
        );
    }

    [TestMethod]
    public void AppendPrepend_Many_Scheduler()
    {

        var xs = Scheduler.CreateHotObservable(
            OnNext(150, 1),
            OnNext(220, 2),
            OnCompleted<int>(250)
        );

        var res = Scheduler.Start(() =>
            xs.Append(3, Scheduler)
              .Append(4, Scheduler)
              .Append(5, Scheduler)
              .Prepend(6, Scheduler)
              .Prepend(7, Scheduler)
              .Prepend(8, Scheduler)
        );

        res.Messages.AssertEqual(
            OnNext(ScheduledAt(200), 8),
            OnNext(ScheduledAt(200, 2), 7),
            OnNext(ScheduledAt(200, 3), 6),
            OnNext(220, 2),
            OnNext(ScheduledAt(250), 3),
            OnNext(ScheduledAt(250, 2), 4),
            OnNext(ScheduledAt(250, 3), 5),
            OnCompleted<int>(ScheduledAt(250, 3))
        );
    }

    [TestMethod]
    public void PrependAppend_Many_Scheduler()
    {

        var xs = Scheduler.CreateHotObservable(
            OnNext(150, 1),
            OnNext(220, 2),
            OnCompleted<int>(250)
        );

        var res = Scheduler.Start(() =>
            xs.Prepend(6, Scheduler)
              .Prepend(7, Scheduler)
              .Prepend(8, Scheduler)
              .Append(3, Scheduler)
              .Append(4, Scheduler)
              .Append(5, Scheduler)
        );

        res.Messages.AssertEqual(
            OnNext(ScheduledAt(200), 8),
            OnNext(ScheduledAt(200, 2), 7),
            OnNext(ScheduledAt(200, 3), 6),
            OnNext(220, 2),
            OnNext(ScheduledAt(250), 3),
            OnNext(ScheduledAt(250, 2), 4),
            OnNext(ScheduledAt(250, 3), 5),
            OnCompleted<int>(ScheduledAt(250, 3))
        );
    }

    [TestMethod]
    public void AppendPrepend_Mixed()
    {

        var xs = Scheduler.CreateHotObservable(
            OnNext(150, 1),
            OnNext(220, 2),
            OnCompleted<int>(250)
        );

        var res = Scheduler.Start(() =>
            xs.Append(3).Prepend(6).Append(4).Prepend(7).Prepend(8).Append(5)
        );

        res.Messages.AssertEqual(
            OnNext(200, 8),
            OnNext(200, 7),
            OnNext(200, 6),
            OnNext(220, 2),
            OnNext(250, 3),
            OnNext(250, 4),
            OnNext(250, 5),
            OnCompleted<int>(250)
        );
    }

    [TestMethod]
    public void AppendPrepend_Mixed_Scheduler()
    {

        var xs = Scheduler.CreateHotObservable(
            OnNext(150, 1),
            OnNext(220, 2),
            OnCompleted<int>(250)
        );

        var res = Scheduler.Start(() =>
            xs.Append(3, Scheduler)
              .Prepend(6, Scheduler)
              .Append(4, Scheduler)
              .Prepend(7, Scheduler)
              .Prepend(8, Scheduler)
              .Append(5, Scheduler)
        );

        res.Messages.AssertEqual(
            OnNext(ScheduledAt(200), 8),
            OnNext(ScheduledAt(200, 2), 7),
            OnNext(ScheduledAt(200, 3), 6),
            OnNext(220, 2),
            OnNext(ScheduledAt(250), 3),
            OnNext(ScheduledAt(250, 2), 4),
            OnNext(ScheduledAt(250, 3), 5),
            OnCompleted<int>(ScheduledAt(250, 3))
        );
    }

    [TestMethod]
    public void AppendPrepend_MixedSchedulers()
    {

        var xs = Scheduler.CreateHotObservable(
            OnNext(150, 1),
            OnNext(220, 2),
            OnCompleted<int>(250)
        );

        var res = Scheduler.Start(() =>
            xs.Append(3, Scheduler)
              .Prepend(6)
              .Append(4, Scheduler)
              .Prepend(7)
              .Prepend(8, Scheduler)
              .Append(5)
        );

        res.Messages.AssertEqual(
            OnNext(ScheduledAt(200), 8),
            OnNext(ScheduledAt(200), 7),
            OnNext(ScheduledAt(200), 6),
            OnNext(220, 2),
            OnNext(ScheduledAt(250), 3),
            OnNext(ScheduledAt(250, 2), 4),
            OnNext(ScheduledAt(250, 2), 5),
            OnCompleted<int>(ScheduledAt(250, 2))
        );
    }
}

// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information. 

namespace Tests.System.Reactive.Shared.Scenarios;

/// <summary>
/// Shared <c>Take</c> scenarios: every behavioural test from Rx.NET's <c>TakeTest.cs</c>. The
/// scenario data, expectations and vocabulary are target-neutral; <c>xs.Take(20)</c> builds a
/// description (<see cref="Seq{T}"/>) that the target materializes inside <c>Start</c>, so the
/// operator application is written exactly as in the sync suite, and the scheduler is the base
/// class's <c>Scheduler</c> where the sync suite has a <c>scheduler</c> local.
/// </summary>
/// <remarks>
/// Transcribed mechanically from the sync file. Deliberately not here: the two
/// <c>*_ArgumentChecking</c> tests (the code-generated stratum), and two tests that belong to the
/// per-target native stratum — <c>Take_DecrementsCountFirst</c> (a reentrancy regression test
/// with no assertion, driving a <c>BehaviorSubject</c> back into itself and relying on a stack
/// overflow to signal failure) and <c>Take_Default</c> (runs <c>Range</c> on the real default
/// scheduler and blocks on a <c>ManualResetEvent</c>).
/// </remarks>
public abstract class TakeTests : SharedReactiveTest
{
    [TestMethod]
    public void Take_Complete_After()
    {

        var xs = Scheduler.CreateHotObservable(
            OnNext(70, 6),
            OnNext(150, 4),
            OnNext(210, 9),
            OnNext(230, 13),
            OnNext(270, 7),
            OnNext(280, 1),
            OnNext(300, -1),
            OnNext(310, 3),
            OnNext(340, 8),
            OnNext(370, 11),
            OnNext(410, 15),
            OnNext(415, 16),
            OnNext(460, 72),
            OnNext(510, 76),
            OnNext(560, 32),
            OnNext(570, -100),
            OnNext(580, -3),
            OnNext(590, 5),
            OnNext(630, 10),
            OnCompleted<int>(690)
        );

        var res = Scheduler.Start(() =>
            xs.Take(20)
        );

        res.Messages.AssertEqual(
            OnNext(210, 9),
            OnNext(230, 13),
            OnNext(270, 7),
            OnNext(280, 1),
            OnNext(300, -1),
            OnNext(310, 3),
            OnNext(340, 8),
            OnNext(370, 11),
            OnNext(410, 15),
            OnNext(415, 16),
            OnNext(460, 72),
            OnNext(510, 76),
            OnNext(560, 32),
            OnNext(570, -100),
            OnNext(580, -3),
            OnNext(590, 5),
            OnNext(630, 10),
            OnCompleted<int>(690)
        );

        xs.Subscriptions.AssertEqual(
            Subscribe(200, 690)
        );
    }

    [TestMethod]
    public void Take_Complete_Same()
    {

        var xs = Scheduler.CreateHotObservable(
            OnNext(70, 6),
            OnNext(150, 4),
            OnNext(210, 9),
            OnNext(230, 13),
            OnNext(270, 7),
            OnNext(280, 1),
            OnNext(300, -1),
            OnNext(310, 3),
            OnNext(340, 8),
            OnNext(370, 11),
            OnNext(410, 15),
            OnNext(415, 16),
            OnNext(460, 72),
            OnNext(510, 76),
            OnNext(560, 32),
            OnNext(570, -100),
            OnNext(580, -3),
            OnNext(590, 5),
            OnNext(630, 10),
            OnCompleted<int>(690)
        );

        var res = Scheduler.Start(() =>
            xs.Take(17)
        );

        res.Messages.AssertEqual(
            OnNext(210, 9),
            OnNext(230, 13),
            OnNext(270, 7),
            OnNext(280, 1),
            OnNext(300, -1),
            OnNext(310, 3),
            OnNext(340, 8),
            OnNext(370, 11),
            OnNext(410, 15),
            OnNext(415, 16),
            OnNext(460, 72),
            OnNext(510, 76),
            OnNext(560, 32),
            OnNext(570, -100),
            OnNext(580, -3),
            OnNext(590, 5),
            OnNext(630, 10),
            OnCompleted<int>(630)
        );

        xs.Subscriptions.AssertEqual(
            Subscribe(200, 630)
        );
    }

    [TestMethod]
    public void Take_Complete_Before()
    {

        var xs = Scheduler.CreateHotObservable(
            OnNext(70, 6),
            OnNext(150, 4),
            OnNext(210, 9),
            OnNext(230, 13),
            OnNext(270, 7),
            OnNext(280, 1),
            OnNext(300, -1),
            OnNext(310, 3),
            OnNext(340, 8),
            OnNext(370, 11),
            OnNext(410, 15),
            OnNext(415, 16),
            OnNext(460, 72),
            OnNext(510, 76),
            OnNext(560, 32),
            OnNext(570, -100),
            OnNext(580, -3),
            OnNext(590, 5),
            OnNext(630, 10),
            OnCompleted<int>(690)
        );

        var res = Scheduler.Start(() =>
            xs.Take(10)
        );

        res.Messages.AssertEqual(
            OnNext(210, 9),
            OnNext(230, 13),
            OnNext(270, 7),
            OnNext(280, 1),
            OnNext(300, -1),
            OnNext(310, 3),
            OnNext(340, 8),
            OnNext(370, 11),
            OnNext(410, 15),
            OnNext(415, 16),
            OnCompleted<int>(415)
        );

        xs.Subscriptions.AssertEqual(
            Subscribe(200, 415)
        );
    }

    [TestMethod]
    public void Take_Error_After()
    {

        var ex = new Exception();

        var xs = Scheduler.CreateHotObservable(
            OnNext(70, 6),
            OnNext(150, 4),
            OnNext(210, 9),
            OnNext(230, 13),
            OnNext(270, 7),
            OnNext(280, 1),
            OnNext(300, -1),
            OnNext(310, 3),
            OnNext(340, 8),
            OnNext(370, 11),
            OnNext(410, 15),
            OnNext(415, 16),
            OnNext(460, 72),
            OnNext(510, 76),
            OnNext(560, 32),
            OnNext(570, -100),
            OnNext(580, -3),
            OnNext(590, 5),
            OnNext(630, 10),
            OnError<int>(690, ex)
        );

        var res = Scheduler.Start(() =>
            xs.Take(20)
        );

        res.Messages.AssertEqual(
            OnNext(210, 9),
            OnNext(230, 13),
            OnNext(270, 7),
            OnNext(280, 1),
            OnNext(300, -1),
            OnNext(310, 3),
            OnNext(340, 8),
            OnNext(370, 11),
            OnNext(410, 15),
            OnNext(415, 16),
            OnNext(460, 72),
            OnNext(510, 76),
            OnNext(560, 32),
            OnNext(570, -100),
            OnNext(580, -3),
            OnNext(590, 5),
            OnNext(630, 10),
            OnError<int>(690, ex)
        );

        xs.Subscriptions.AssertEqual(
            Subscribe(200, 690)
        );
    }

    [TestMethod]
    public void Take_Error_Same()
    {

        var xs = Scheduler.CreateHotObservable(
            OnNext(70, 6),
            OnNext(150, 4),
            OnNext(210, 9),
            OnNext(230, 13),
            OnNext(270, 7),
            OnNext(280, 1),
            OnNext(300, -1),
            OnNext(310, 3),
            OnNext(340, 8),
            OnNext(370, 11),
            OnNext(410, 15),
            OnNext(415, 16),
            OnNext(460, 72),
            OnNext(510, 76),
            OnNext(560, 32),
            OnNext(570, -100),
            OnNext(580, -3),
            OnNext(590, 5),
            OnNext(630, 10),
            OnError<int>(690, new Exception())
        );

        var res = Scheduler.Start(() =>
            xs.Take(17)
        );

        res.Messages.AssertEqual(
            OnNext(210, 9),
            OnNext(230, 13),
            OnNext(270, 7),
            OnNext(280, 1),
            OnNext(300, -1),
            OnNext(310, 3),
            OnNext(340, 8),
            OnNext(370, 11),
            OnNext(410, 15),
            OnNext(415, 16),
            OnNext(460, 72),
            OnNext(510, 76),
            OnNext(560, 32),
            OnNext(570, -100),
            OnNext(580, -3),
            OnNext(590, 5),
            OnNext(630, 10),
            OnCompleted<int>(630)
        );

        xs.Subscriptions.AssertEqual(
            Subscribe(200, 630)
        );
    }

    [TestMethod]
    public void Take_Error_Before()
    {

        var xs = Scheduler.CreateHotObservable(
            OnNext(70, 6),
            OnNext(150, 4),
            OnNext(210, 9),
            OnNext(230, 13),
            OnNext(270, 7),
            OnNext(280, 1),
            OnNext(300, -1),
            OnNext(310, 3),
            OnNext(340, 8),
            OnNext(370, 11),
            OnNext(410, 15),
            OnNext(415, 16),
            OnNext(460, 72),
            OnNext(510, 76),
            OnNext(560, 32),
            OnNext(570, -100),
            OnNext(580, -3),
            OnNext(590, 5),
            OnNext(630, 10),
            OnError<int>(690, new Exception())
        );

        var res = Scheduler.Start(() =>
            xs.Take(3)
        );

        res.Messages.AssertEqual(
            OnNext(210, 9),
            OnNext(230, 13),
            OnNext(270, 7),
            OnCompleted<int>(270)
        );

        xs.Subscriptions.AssertEqual(
            Subscribe(200, 270)
        );
    }

    [TestMethod]
    public void Take_Dispose_Before()
    {

        var xs = Scheduler.CreateHotObservable(
            OnNext(70, 6),
            OnNext(150, 4),
            OnNext(210, 9),
            OnNext(230, 13),
            OnNext(270, 7),
            OnNext(280, 1),
            OnNext(300, -1),
            OnNext(310, 3),
            OnNext(340, 8),
            OnNext(370, 11),
            OnNext(410, 15),
            OnNext(415, 16),
            OnNext(460, 72),
            OnNext(510, 76),
            OnNext(560, 32),
            OnNext(570, -100),
            OnNext(580, -3),
            OnNext(590, 5),
            OnNext(630, 10)
        );

        var res = Scheduler.Start(() =>
            xs.Take(3),
            250
        );

        res.Messages.AssertEqual(
            OnNext(210, 9),
            OnNext(230, 13)
        );

        xs.Subscriptions.AssertEqual(
            Subscribe(200, 250)
        );
    }

    [TestMethod]
    public void Take_Dispose_After()
    {

        var xs = Scheduler.CreateHotObservable(
            OnNext(70, 6),
            OnNext(150, 4),
            OnNext(210, 9),
            OnNext(230, 13),
            OnNext(270, 7),
            OnNext(280, 1),
            OnNext(300, -1),
            OnNext(310, 3),
            OnNext(340, 8),
            OnNext(370, 11),
            OnNext(410, 15),
            OnNext(415, 16),
            OnNext(460, 72),
            OnNext(510, 76),
            OnNext(560, 32),
            OnNext(570, -100),
            OnNext(580, -3),
            OnNext(590, 5),
            OnNext(630, 10)
        );

        var res = Scheduler.Start(() =>
            xs.Take(3),
            400
        );

        res.Messages.AssertEqual(
            OnNext(210, 9),
            OnNext(230, 13),
            OnNext(270, 7),
            OnCompleted<int>(270)
        );

        xs.Subscriptions.AssertEqual(
            Subscribe(200, 270)
        );
    }

    [TestMethod]
    public void Take_0_Scheduler()
    {

        var xs = Scheduler.CreateHotObservable(
            OnNext(70, 6),
            OnNext(150, 4),
            OnNext(210, 9),
            OnNext(230, 13)
        );

        var res = Scheduler.Start(() =>
            xs.Take(0, Scheduler)
        );

        res.Messages.AssertEqual(
            OnCompleted<int>(ScheduledAt(200)) // Extra scheduling call by Empty
        );

        xs.Subscriptions.AssertEqual(
        );
    }

    [TestMethod]
    public void Take_0_DefaultScheduler()
    {

        var xs = Scheduler.CreateHotObservable(
            OnNext(70, 6),
            OnNext(150, 4),
            OnNext(210, 9),
            OnNext(230, 13)
        );

        var res = Scheduler.Start(() =>
            xs.Take(0)
        );

        res.Messages.AssertEqual(
            OnCompleted<int>(200) // Immediate
        );

        xs.Subscriptions.AssertEqual(
        );
    }

    [TestMethod]
    public void Take_Non0_Scheduler()
    {

        var xs = Scheduler.CreateHotObservable(
            OnNext(70, 6),
            OnNext(150, 4),
            OnNext(210, 9),
            OnNext(230, 13)
        );

        var res = Scheduler.Start(() =>
            xs.Take(1, Scheduler)
        );

        res.Messages.AssertEqual(
            OnNext(210, 9),
            OnCompleted<int>(210)
        );

        xs.Subscriptions.AssertEqual(
            Subscribe(200, 210)
        );
    }

    [TestMethod]
    public void Take_Take1()
    {

        var xs = Scheduler.CreateHotObservable(
            OnNext(70, 6),
            OnNext(150, 4),
            OnNext(210, 9),
            OnNext(230, 13),
            OnNext(270, 7),
            OnNext(280, 1),
            OnNext(300, -1),
            OnNext(310, 3),
            OnNext(340, 8),
            OnNext(370, 11),
            OnCompleted<int>(400)
        );

        var res = Scheduler.Start(() =>
            xs.Take(3).Take(4)
        );

        res.Messages.AssertEqual(
            OnNext(210, 9),
            OnNext(230, 13),
            OnNext(270, 7),
            OnCompleted<int>(270)
        );

        xs.Subscriptions.AssertEqual(
            Subscribe(200, 270)
        );
    }

    [TestMethod]
    public void Take_Take2()
    {

        var xs = Scheduler.CreateHotObservable(
            OnNext(70, 6),
            OnNext(150, 4),
            OnNext(210, 9),
            OnNext(230, 13),
            OnNext(270, 7),
            OnNext(280, 1),
            OnNext(300, -1),
            OnNext(310, 3),
            OnNext(340, 8),
            OnNext(370, 11),
            OnCompleted<int>(400)
        );

        var res = Scheduler.Start(() =>
            xs.Take(4).Take(3)
        );

        res.Messages.AssertEqual(
            OnNext(210, 9),
            OnNext(230, 13),
            OnNext(270, 7),
            OnCompleted<int>(270)
        );

        xs.Subscriptions.AssertEqual(
            Subscribe(200, 270)
        );
    }

    [TestMethod]
    public void Take_Zero()
    {

        var xs = Scheduler.CreateHotObservable(
            OnNext(210, 1),
            OnNext(220, 2),
            OnCompleted<int>(230)
        );

        var res = Scheduler.Start(() =>
            xs.Take(TimeSpan.Zero, Scheduler)
        );

        res.Messages.AssertEqual(
            OnCompleted<int>(ScheduledAt(200))
        );

        xs.Subscriptions.AssertEqual(
            Subscribe(200, ScheduledAt(200))
        );
    }

    [TestMethod]
    public void Take_Some()
    {

        var xs = Scheduler.CreateHotObservable(
            OnNext(210, 1),
            OnNext(220, 2),
            OnNext(230, 3),
            OnCompleted<int>(240)
        );

        var res = Scheduler.Start(() =>
            xs.Take(TimeSpan.FromTicks(25), Scheduler)
        );

        res.Messages.AssertEqual(
            OnNext(210, 1),
            OnNext(220, 2),
            OnCompleted<int>(225)
        );

        xs.Subscriptions.AssertEqual(
            Subscribe(200, 225)
        );
    }

    [TestMethod]
    public void Take_Late()
    {

        var xs = Scheduler.CreateHotObservable(
            OnNext(210, 1),
            OnNext(220, 2),
            OnCompleted<int>(230)
        );

        var res = Scheduler.Start(() =>
            xs.Take(TimeSpan.FromTicks(50), Scheduler)
        );

        res.Messages.AssertEqual(
            OnNext(210, 1),
            OnNext(220, 2),
            OnCompleted<int>(230)
        );

        xs.Subscriptions.AssertEqual(
            Subscribe(200, 230)
        );
    }

    [TestMethod]
    public void Take_Error()
    {

        var ex = new Exception();

        var xs = Scheduler.CreateHotObservable(
            OnError<int>(210, ex)
        );

        var res = Scheduler.Start(() =>
            xs.Take(TimeSpan.FromTicks(50), Scheduler)
        );

        res.Messages.AssertEqual(
            OnError<int>(210, ex)
        );

        xs.Subscriptions.AssertEqual(
            Subscribe(200, 210)
        );
    }

    [TestMethod]
    public void Take_Never()
    {

        var ex = new Exception();

        var xs = Scheduler.CreateHotObservable<int>(
        );

        var res = Scheduler.Start(() =>
            xs.Take(TimeSpan.FromTicks(50), Scheduler)
        );

        res.Messages.AssertEqual(
            OnCompleted<int>(250)
        );

        xs.Subscriptions.AssertEqual(
            Subscribe(200, 250)
        );
    }

    [TestMethod]
    public void Take_Twice1()
    {

        var ex = new Exception();

        var xs = Scheduler.CreateHotObservable(
            OnNext(210, 1),
            OnNext(220, 2),
            OnNext(230, 3),
            OnNext(240, 4),
            OnNext(250, 5),
            OnNext(260, 6),
            OnCompleted<int>(270)
        );

        var res = Scheduler.Start(() =>
            xs.Take(TimeSpan.FromTicks(55), Scheduler).Take(TimeSpan.FromTicks(35), Scheduler)
        );

        res.Messages.AssertEqual(
            OnNext(210, 1),
            OnNext(220, 2),
            OnNext(230, 3),
            OnCompleted<int>(235)
        );

        xs.Subscriptions.AssertEqual(
            Subscribe(200, 235)
        );
    }

    [TestMethod]
    public void Take_Twice2()
    {

        var ex = new Exception();

        var xs = Scheduler.CreateHotObservable(
            OnNext(210, 1),
            OnNext(220, 2),
            OnNext(230, 3),
            OnNext(240, 4),
            OnNext(250, 5),
            OnNext(260, 6),
            OnCompleted<int>(270)
        );

        var res = Scheduler.Start(() =>
            xs.Take(TimeSpan.FromTicks(35), Scheduler).Take(TimeSpan.FromTicks(55), Scheduler)
        );

        res.Messages.AssertEqual(
            OnNext(210, 1),
            OnNext(220, 2),
            OnNext(230, 3),
            OnCompleted<int>(235)
        );

        xs.Subscriptions.AssertEqual(
            Subscribe(200, 235)
        );
    }
}

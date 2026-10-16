// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information. 

namespace Tests.System.Reactive.Shared.Scenarios;

/// <summary>Shared <c>Skip</c> scenarios, from Rx.NET's <c>SkipTest.cs</c>.</summary>
/// <remarks>
/// Every behavioural test from that file, transcribed mechanically: 18 of its 20. Deliberately
/// not here: the two <c>*_ArgumentChecking</c> tests (the code-generated stratum). Unused
/// <c>ex</c> locals that the original declares in three of the timed tests are omitted.
/// <c>Skip_Default</c>, a real-time test, runs on <see cref="SharedReactiveTest.DefaultScheduler"/>
/// and awaits completion through <see cref="SharedReactiveTest.ToListAsync{T}"/> where the
/// original blocks on a <c>ManualResetEvent</c>.
/// </remarks>
public abstract class SkipTests : SharedReactiveTest
{
    [TestMethod]
    public void Skip_Complete_After()
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
            xs.Skip(20)
        );

        res.Messages.AssertEqual(
            OnCompleted<int>(690)
        );

        xs.Subscriptions.AssertEqual(
            Subscribe(200, 690)
        );
    }

    [TestMethod]
    public void Skip_Complete_Same()
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
            xs.Skip(17)
        );

        res.Messages.AssertEqual(
            OnCompleted<int>(690)
        );

        xs.Subscriptions.AssertEqual(
            Subscribe(200, 690)
        );
    }

    [TestMethod]
    public void Skip_Complete_Before()
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
            xs.Skip(10)
        );

        res.Messages.AssertEqual(
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
    public void Skip_Complete_Zero()
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
            xs.Skip(0)
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
    public void Skip_Error_After()
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
            xs.Skip(20)
        );

        res.Messages.AssertEqual(
            OnError<int>(690, ex)
        );

        xs.Subscriptions.AssertEqual(
            Subscribe(200, 690)
        );
    }

    [TestMethod]
    public void Skip_Error_Same()
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
            xs.Skip(17)
        );

        res.Messages.AssertEqual(
            OnError<int>(690, ex)
        );

        xs.Subscriptions.AssertEqual(
            Subscribe(200, 690)
        );
    }

    [TestMethod]
    public void Skip_Error_Before()
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
            xs.Skip(3)
        );

        res.Messages.AssertEqual(
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
    public void Skip_Dispose_Before()
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
            xs.Skip(3),
            250
        );

        res.Messages.AssertEqual(
        );

        xs.Subscriptions.AssertEqual(
            Subscribe(200, 250)
        );
    }

    [TestMethod]
    public void Skip_Dispose_After()
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
            xs.Skip(3),
            400
        );

        res.Messages.AssertEqual(
            OnNext(280, 1),
            OnNext(300, -1),
            OnNext(310, 3),
            OnNext(340, 8),
            OnNext(370, 11)
        );

        xs.Subscriptions.AssertEqual(
            Subscribe(200, 400)
        );
    }

    [TestMethod]
    public void Skip_Skip1()
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
            xs.Skip(3).Skip(2)
        );

        res.Messages.AssertEqual(
            OnNext(310, 3),
            OnNext(340, 8),
            OnNext(370, 11),
            OnCompleted<int>(400)
        );

        xs.Subscriptions.AssertEqual(
            Subscribe(200, 400)
        );
    }

    [TestMethod]
    public void Skip_Zero()
    {
        var xs = Scheduler.CreateHotObservable(
            OnNext(210, 1),
            OnNext(220, 2),
            OnCompleted<int>(230)
        );

        var res = Scheduler.Start(() =>
            xs.Skip(TimeSpan.Zero, Scheduler)
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
    public void Skip_Some()
    {
        var xs = Scheduler.CreateHotObservable(
            OnNext(210, 1),
            OnNext(220, 2),
            OnCompleted<int>(230)
        );

        var res = Scheduler.Start(() =>
            xs.Skip(TimeSpan.FromTicks(15), Scheduler)
        );

        res.Messages.AssertEqual(
            OnNext(220, 2),
            OnCompleted<int>(230)
        );

        xs.Subscriptions.AssertEqual(
            Subscribe(200, 230)
        );
    }

    [TestMethod]
    public void Skip_Late()
    {
        var xs = Scheduler.CreateHotObservable(
            OnNext(210, 1),
            OnNext(220, 2),
            OnCompleted<int>(230)
        );

        var res = Scheduler.Start(() =>
            xs.Skip(TimeSpan.FromTicks(50), Scheduler)
        );

        res.Messages.AssertEqual(
            OnCompleted<int>(230)
        );

        xs.Subscriptions.AssertEqual(
            Subscribe(200, 230)
        );
    }

    [TestMethod]
    public void Skip_Error()
    {
        var ex = new Exception();

        var xs = Scheduler.CreateHotObservable(
            OnError<int>(210, ex)
        );

        var res = Scheduler.Start(() =>
            xs.Skip(TimeSpan.FromTicks(50), Scheduler)
        );

        res.Messages.AssertEqual(
            OnError<int>(210, ex)
        );

        xs.Subscriptions.AssertEqual(
            Subscribe(200, 210)
        );
    }

    [TestMethod]
    public void Skip_Never()
    {
        var xs = Scheduler.CreateHotObservable<int>(
        );

        var res = Scheduler.Start(() =>
            xs.Skip(TimeSpan.FromTicks(50), Scheduler)
        );

        res.Messages.AssertEqual(
        );

        xs.Subscriptions.AssertEqual(
            Subscribe(200, 1000)
        );
    }

    [TestMethod]
    public void Skip_Twice1()
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
            xs.Skip(TimeSpan.FromTicks(15), Scheduler).Skip(TimeSpan.FromTicks(30), Scheduler)
        );

        res.Messages.AssertEqual(
            OnNext(240, 4),
            OnNext(250, 5),
            OnNext(260, 6),
            OnCompleted<int>(270)
        );

        xs.Subscriptions.AssertEqual(
            Subscribe(200, 270)
        );
    }

    [TestMethod]
    public void Skip_Twice2()
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
            xs.Skip(TimeSpan.FromTicks(30), Scheduler).Skip(TimeSpan.FromTicks(15), Scheduler)
        );

        res.Messages.AssertEqual(
            OnNext(240, 4),
            OnNext(250, 5),
            OnNext(260, 6),
            OnCompleted<int>(270)
        );

        xs.Subscriptions.AssertEqual(
            Subscribe(200, 270)
        );
    }

    [TestMethod]
    public async Task Skip_Default()
    {
        var xs = Seq.Range(0, 10, DefaultScheduler);

        var res = xs.Skip(TimeSpan.FromSeconds(60));

        var lst = await ToListAsync(res);

        Assert.IsEmpty(lst);
    }
}

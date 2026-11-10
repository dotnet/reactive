// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information. 

namespace Tests.System.Reactive.Shared.Scenarios;

/// <summary>Shared <c>Timeout</c> scenarios, from Rx.NET's <c>TimeoutTest.cs</c>.</summary>
/// <remarks>
/// 33 of the file's 35 tests (the two <c>*_ArgumentChecking</c> tests are the code-generated
/// stratum). Twenty-nine transcribe as they are: the relative and absolute time-based forms, with
/// and without a fallback sequence, and the duration-selector forms, all virtual-time, with the
/// absolute due times written as <c>DateTimeOffset</c>s of the virtual clock as the original
/// writes them. The four <c>*_DefaultScheduler</c> tests run the scheduler-less overloads on the
/// real default scheduler: the originals block on <c>ToEnumerable().Single()</c>; here they await
/// the result through <see cref="SharedReactiveTest.ToListAsync{T}"/>.
/// </remarks>
public abstract class TimeoutTests : SharedReactiveTest
{
    [TestMethod]
    public void Timeout_InTime()
    {
        var scheduler = Scheduler;

        var xs = scheduler.CreateHotObservable(
            OnNext(150, 1),
            OnNext(210, 2),
            OnNext(230, 3),
            OnNext(260, 4),
            OnNext(300, 5),
            OnNext(350, 6),
            OnCompleted<int>(400)
        );

        var res = scheduler.Start(() =>
            xs.Timeout(TimeSpan.FromTicks(500), scheduler)
        );

        res.Messages.AssertEqual(
            OnNext(210, 2),
            OnNext(230, 3),
            OnNext(260, 4),
            OnNext(300, 5),
            OnNext(350, 6),
            OnCompleted<int>(400)
        );

        xs.Subscriptions.AssertEqual(
            Subscribe(200, 400)
        );
    }

    [TestMethod]
    public void Timeout_DateTimeOffset_TimeoutOccurs_WithDefaultException()
    {
        var scheduler = Scheduler;

        var xs = scheduler.CreateHotObservable(
            OnNext(410, 1)
        );

        var res = scheduler.Start(() =>
            xs.Timeout(new DateTimeOffset(new DateTime(400), TimeSpan.Zero), scheduler)
        );

        res.Messages.AssertEqual(
            OnError<int>(400, ex => ex is TimeoutException)
        );

        xs.Subscriptions.AssertEqual(
            Subscribe(200, 400)
        );
    }

    [TestMethod]
    public void Timeout_TimeSpan_TimeoutOccurs_WithDefaultException()
    {
        var scheduler = Scheduler;

        var xs = scheduler.CreateHotObservable(
            OnNext(410, 1)
        );

        var res = scheduler.Start(() =>
            xs.Timeout(TimeSpan.FromTicks(200), scheduler)
        );

        res.Messages.AssertEqual(
            OnError<int>(400, ex => ex is TimeoutException)
        );

        xs.Subscriptions.AssertEqual(
            Subscribe(200, 400)
        );
    }

    [TestMethod]
    public async Task Timeout_TimeSpan_DefaultScheduler()
    {
        var res = await ToListAsync(Seq.Return(1).Timeout(TimeSpan.FromSeconds(10)));

        Assert.IsTrue(res.Single() == 1);
    }

    [TestMethod]
    public async Task Timeout_TimeSpan_Observable_DefaultScheduler()
    {
        var res = await ToListAsync(Seq.Return(1).Timeout(TimeSpan.FromSeconds(10), Seq.Return(2)));

        Assert.IsTrue(res.Single() == 1);
    }

    [TestMethod]
    public async Task Timeout_DateTimeOffset_DefaultScheduler()
    {
        var res = await ToListAsync(
            Seq.Return(1).Timeout(DateTimeOffset.UtcNow + TimeSpan.FromSeconds(10)));

        Assert.IsTrue(res.Single() == 1);
    }

    [TestMethod]
    public async Task Timeout_DateTimeOffset_Observable_DefaultScheduler()
    {
        var res = await ToListAsync(
            Seq.Return(1).Timeout(DateTimeOffset.UtcNow + TimeSpan.FromSeconds(10), Seq.Return(2)));

        Assert.IsTrue(res.Single() == 1);
    }

    [TestMethod]
    public void Timeout_TimeoutOccurs_1()
    {
        var scheduler = Scheduler;

        var xs = scheduler.CreateHotObservable(
            OnNext(70, 1),
            OnNext(130, 2),
            OnNext(310, 3),
            OnNext(400, 4),
            OnCompleted<int>(500)
        );

        var ys = scheduler.CreateColdObservable(
            OnNext(50, -1),
            OnNext(200, -2),
            OnNext(310, -3),
            OnCompleted<int>(320)
        );

        var res = scheduler.Start(() =>
            xs.Timeout(TimeSpan.FromTicks(100), ys, scheduler)
        );

        res.Messages.AssertEqual(
            OnNext(350, -1),
            OnNext(500, -2),
            OnNext(610, -3),
            OnCompleted<int>(620)
        );

        xs.Subscriptions.AssertEqual(
            Subscribe(200, 300)
        );

        ys.Subscriptions.AssertEqual(
            Subscribe(300, 620)
        );
    }

    [TestMethod]
    public void Timeout_TimeoutOccurs_2()
    {
        var scheduler = Scheduler;

        var xs = scheduler.CreateHotObservable(
            OnNext(70, 1),
            OnNext(130, 2),
            OnNext(240, 3),
            OnNext(310, 4),
            OnNext(430, 5),
            OnCompleted<int>(500)
        );

        var ys = scheduler.CreateColdObservable(
            OnNext(50, -1),
            OnNext(200, -2),
            OnNext(310, -3),
            OnCompleted<int>(320)
        );

        var res = scheduler.Start(() =>
            xs.Timeout(TimeSpan.FromTicks(100), ys, scheduler)
        );

        res.Messages.AssertEqual(
            OnNext(240, 3),
            OnNext(310, 4),
            OnNext(460, -1),
            OnNext(610, -2),
            OnNext(720, -3),
            OnCompleted<int>(730)
        );

        xs.Subscriptions.AssertEqual(
            Subscribe(200, 410)
        );

        ys.Subscriptions.AssertEqual(
            Subscribe(410, 730)
        );
    }

    [TestMethod]
    public void Timeout_TimeoutOccurs_Never()
    {
        var scheduler = Scheduler;

        var xs = scheduler.CreateHotObservable(
            OnNext(70, 1),
            OnNext(130, 2),
            OnNext(240, 3),
            OnNext(310, 4),
            OnNext(430, 5),
            OnCompleted<int>(500)
        );

        var ys = scheduler.CreateColdObservable<int>(
        );

        var res = scheduler.Start(() =>
            xs.Timeout(TimeSpan.FromTicks(100), ys, scheduler)
        );

        res.Messages.AssertEqual(
            OnNext(240, 3),
            OnNext(310, 4)
        );

        xs.Subscriptions.AssertEqual(
            Subscribe(200, 410)
        );

        ys.Subscriptions.AssertEqual(
            Subscribe(410, 1000)
        );
    }

    [TestMethod]
    public void Timeout_TimeoutOccurs_Completed()
    {
        var scheduler = Scheduler;

        var xs = scheduler.CreateHotObservable(
            OnCompleted<int>(500)
        );

        var ys = scheduler.CreateColdObservable(
            OnNext(100, -1)
        );

        var res = scheduler.Start(() =>
            xs.Timeout(TimeSpan.FromTicks(100), ys, scheduler)
        );

        res.Messages.AssertEqual(
            OnNext(400, -1)
        );

        xs.Subscriptions.AssertEqual(
            Subscribe(200, 300)
        );

        ys.Subscriptions.AssertEqual(
            Subscribe(300, 1000)
        );
    }

    [TestMethod]
    public void Timeout_TimeoutOccurs_Error()
    {
        var scheduler = Scheduler;

        var xs = scheduler.CreateHotObservable(
            OnError<int>(500, new Exception())
        );

        var ys = scheduler.CreateColdObservable(
            OnNext(100, -1)
        );

        var res = scheduler.Start(() =>
            xs.Timeout(TimeSpan.FromTicks(100), ys, scheduler)
        );

        res.Messages.AssertEqual(
            OnNext(400, -1)
        );

        xs.Subscriptions.AssertEqual(
            Subscribe(200, 300)
        );

        ys.Subscriptions.AssertEqual(
            Subscribe(300, 1000)
        );
    }

    [TestMethod]
    public void Timeout_TimeoutOccurs_NextIsError()
    {
        var ex = new Exception();

        var scheduler = Scheduler;

        var xs = scheduler.CreateHotObservable(
            OnNext(500, 42)
        );

        var ys = scheduler.CreateColdObservable(
            OnError<int>(100, ex)
        );

        var res = scheduler.Start(() =>
            xs.Timeout(TimeSpan.FromTicks(100), ys, scheduler)
        );

        res.Messages.AssertEqual(
            OnError<int>(400, ex)
        );

        xs.Subscriptions.AssertEqual(
            Subscribe(200, 300)
        );

        ys.Subscriptions.AssertEqual(
            Subscribe(300, 400)
        );
    }

    [TestMethod]
    public void Timeout_TimeoutNotOccurs_Completed()
    {
        var scheduler = Scheduler;

        var xs = scheduler.CreateHotObservable(
            OnCompleted<int>(250)
        );

        var ys = scheduler.CreateColdObservable(
            OnNext(100, -1)
        );

        var res = scheduler.Start(() =>
            xs.Timeout(TimeSpan.FromTicks(100), ys, scheduler)
        );

        res.Messages.AssertEqual(
            OnCompleted<int>(250)
        );

        xs.Subscriptions.AssertEqual(
            Subscribe(200, 250)
        );

        ys.Subscriptions.AssertEqual(
        );
    }

    [TestMethod]
    public void Timeout_TimeoutNotOccurs_Error()
    {
        var scheduler = Scheduler;

        var ex = new Exception();

        var xs = scheduler.CreateHotObservable(
            OnError<int>(250, ex)
        );

        var ys = scheduler.CreateColdObservable(
            OnNext(100, -1)
        );

        var res = scheduler.Start(() =>
            xs.Timeout(TimeSpan.FromTicks(100), ys, scheduler)
        );

        res.Messages.AssertEqual(
            OnError<int>(250, ex)
        );

        xs.Subscriptions.AssertEqual(
            Subscribe(200, 250)
        );

        ys.Subscriptions.AssertEqual(
        );
    }

    [TestMethod]
    public void Timeout_TimeoutDoesNotOccur()
    {
        var scheduler = Scheduler;

        var xs = scheduler.CreateHotObservable(
            OnNext(70, 1),
            OnNext(130, 2),
            OnNext(240, 3),
            OnNext(320, 4),
            OnNext(410, 5),
            OnCompleted<int>(500)
        );

        var ys = scheduler.CreateColdObservable(
            OnNext(50, -1),
            OnNext(200, -2),
            OnNext(310, -3),
            OnCompleted<int>(320)
        );

        var res = scheduler.Start(() =>
            xs.Timeout(TimeSpan.FromTicks(100), ys, scheduler)
        );

        res.Messages.AssertEqual(
            OnNext(240, 3),
            OnNext(320, 4),
            OnNext(410, 5),
            OnCompleted<int>(500)
        );

        xs.Subscriptions.AssertEqual(
            Subscribe(200, 500)
        );

        ys.Subscriptions.AssertEqual(
        );
    }

    [TestMethod]
    public void Timeout_DateTimeOffset_TimeoutOccurs()
    {
        var scheduler = Scheduler;

        var xs = scheduler.CreateHotObservable(
            OnNext(410, 1)
        );

        var ys = scheduler.CreateColdObservable(
            OnNext(100, -1)
        );

        var res = scheduler.Start(() =>
            xs.Timeout(new DateTimeOffset(new DateTime(400), TimeSpan.Zero), ys, scheduler)
        );

        res.Messages.AssertEqual(
            OnNext(500, -1)
        );

        xs.Subscriptions.AssertEqual(
            Subscribe(200, 400)
        );

        ys.Subscriptions.AssertEqual(
            Subscribe(400, 1000)
        );
    }

    [TestMethod]
    public void Timeout_DateTimeOffset_TimeoutDoesNotOccur_Completed()
    {
        var scheduler = Scheduler;

        var xs = scheduler.CreateHotObservable(
            OnNext(310, 1),
            OnCompleted<int>(390)
        );

        var ys = scheduler.CreateColdObservable(
            OnNext(100, -1)
        );

        var res = scheduler.Start(() =>
            xs.Timeout(new DateTimeOffset(new DateTime(400), TimeSpan.Zero), ys, scheduler)
        );

        res.Messages.AssertEqual(
            OnNext(310, 1),
            OnCompleted<int>(390)
        );

        xs.Subscriptions.AssertEqual(
            Subscribe(200, 390)
        );

        ys.Subscriptions.AssertEqual(
        );
    }

    [TestMethod]
    public void Timeout_DateTimeOffset_TimeoutDoesNotOccur_Error()
    {
        var scheduler = Scheduler;

        var ex = new Exception();

        var xs = scheduler.CreateHotObservable(
            OnNext(310, 1),
            OnError<int>(390, ex)
        );

        var ys = scheduler.CreateColdObservable(
            OnNext(100, -1)
        );

        var res = scheduler.Start(() =>
            xs.Timeout(new DateTimeOffset(new DateTime(400), TimeSpan.Zero), ys, scheduler)
        );

        res.Messages.AssertEqual(
            OnNext(310, 1),
            OnError<int>(390, ex)
        );

        xs.Subscriptions.AssertEqual(
            Subscribe(200, 390)
        );

        ys.Subscriptions.AssertEqual(
        );
    }

    [TestMethod]
    public void Timeout_DateTimeOffset_TimeoutOccur_2()
    {
        var scheduler = Scheduler;

        var xs = scheduler.CreateHotObservable(
            OnNext(310, 1),
            OnNext(350, 2),
            OnNext(420, 3),
            OnCompleted<int>(450)
        );

        var ys = scheduler.CreateColdObservable(
            OnNext(100, -1)
        );

        var res = scheduler.Start(() =>
            xs.Timeout(new DateTimeOffset(new DateTime(400), TimeSpan.Zero), ys, scheduler)
        );

        res.Messages.AssertEqual(
            OnNext(310, 1),
            OnNext(350, 2),
            OnNext(500, -1)
        );

        xs.Subscriptions.AssertEqual(
            Subscribe(200, 400)
        );

        ys.Subscriptions.AssertEqual(
            Subscribe(400, 1000)
        );
    }

    [TestMethod]
    public void Timeout_DateTimeOffset_TimeoutOccur_3()
    {
        var scheduler = Scheduler;

        var xs = scheduler.CreateHotObservable(
            OnNext(310, 1),
            OnNext(350, 2),
            OnNext(420, 3),
            OnCompleted<int>(450)
        );

        var ys = scheduler.CreateColdObservable<int>(
        );

        var res = scheduler.Start(() =>
            xs.Timeout(new DateTimeOffset(new DateTime(400), TimeSpan.Zero), ys, scheduler)
        );

        res.Messages.AssertEqual(
            OnNext(310, 1),
            OnNext(350, 2)
        );

        xs.Subscriptions.AssertEqual(
            Subscribe(200, 400)
        );

        ys.Subscriptions.AssertEqual(
            Subscribe(400, 1000)
        );
    }

    [TestMethod]
    public void Timeout_Duration_Simple_Never()
    {
        var scheduler = Scheduler;

        var xs = scheduler.CreateHotObservable(
            OnNext(310, 1),
            OnNext(350, 2),
            OnNext(420, 3),
            OnCompleted<int>(450)
        );

        var ys = scheduler.CreateColdObservable<int>(
        );

        var res = scheduler.Start(() =>
            xs.Timeout(ys, _ => ys)
        );

        res.Messages.AssertEqual(
            OnNext(310, 1),
            OnNext(350, 2),
            OnNext(420, 3),
            OnCompleted<int>(450)
        );

        xs.Subscriptions.AssertEqual(
            Subscribe(200, 450)
        );

        ys.Subscriptions.AssertEqual(
            Subscribe(200, 310),
            Subscribe(310, 350),
            Subscribe(350, 420),
            Subscribe(420, 450)
        );
    }

    [TestMethod]
    public void Timeout_Duration_Simple_TimeoutFirst()
    {
        var scheduler = Scheduler;

        var xs = scheduler.CreateHotObservable(
            OnNext(310, 1),
            OnNext(350, 2),
            OnNext(420, 3),
            OnCompleted<int>(450)
        );

        var ys = scheduler.CreateColdObservable(
            OnNext(100, "Boo!")
        );

        var zs = scheduler.CreateColdObservable<string>(
        );

        var res = scheduler.Start(() =>
            xs.Timeout(ys, _ => zs)
        );

        res.Messages.AssertEqual(
            OnError<int>(300, ex => ex is TimeoutException)
        );

        xs.Subscriptions.AssertEqual(
            Subscribe(200, 300)
        );

        ys.Subscriptions.AssertEqual(
            Subscribe(200, 300)
        );

        zs.Subscriptions.AssertEqual(
        );
    }

    [TestMethod]
    public void Timeout_Duration_Simple_TimeoutFirst_Other()
    {
        var scheduler = Scheduler;

        var xs = scheduler.CreateHotObservable(
            OnNext(310, 1),
            OnNext(350, 2),
            OnNext(420, 3),
            OnCompleted<int>(450)
        );

        var ys = scheduler.CreateColdObservable(
            OnNext(100, "Boo!")
        );

        var zs = scheduler.CreateColdObservable<string>(
        );

        var ts = scheduler.CreateColdObservable(
            OnNext(50, 42),
            OnCompleted<int>(70)
        );

        var res = scheduler.Start(() =>
            xs.Timeout(ys, _ => zs, ts)
        );

        res.Messages.AssertEqual(
            OnNext(350, 42),
            OnCompleted<int>(370)
        );

        xs.Subscriptions.AssertEqual(
            Subscribe(200, 300)
        );

        ys.Subscriptions.AssertEqual(
            Subscribe(200, 300)
        );

        zs.Subscriptions.AssertEqual(
        );

        ts.Subscriptions.AssertEqual(
            Subscribe(300, 370)
        );
    }

    [TestMethod]
    public void Timeout_Duration_Simple_TimeoutLater()
    {
        var scheduler = Scheduler;

        var xs = scheduler.CreateHotObservable(
            OnNext(310, 1),
            OnNext(350, 2),
            OnNext(420, 3),
            OnCompleted<int>(450)
        );

        var ys = scheduler.CreateColdObservable<string>(
        );

        var zs = scheduler.CreateColdObservable(
            OnNext(50, "Boo!")
        );

        var res = scheduler.Start(() =>
            xs.Timeout(ys, _ => zs)
        );

        res.Messages.AssertEqual(
            OnNext(310, 1),
            OnNext(350, 2),
            OnError<int>(400, ex => ex is TimeoutException)
        );

        xs.Subscriptions.AssertEqual(
            Subscribe(200, 400)
        );

        ys.Subscriptions.AssertEqual(
            Subscribe(200, 310)
        );

        zs.Subscriptions.AssertEqual(
            Subscribe(310, 350),
            Subscribe(350, 400)
        );
    }

    [TestMethod]
    public void Timeout_Duration_Simple_TimeoutLater_Other()
    {
        var scheduler = Scheduler;

        var xs = scheduler.CreateHotObservable(
            OnNext(310, 1),
            OnNext(350, 2),
            OnNext(420, 3),
            OnCompleted<int>(450)
        );

        var ys = scheduler.CreateColdObservable<string>(
        );

        var zs = scheduler.CreateColdObservable(
            OnNext(50, "Boo!")
        );

        var ts = scheduler.CreateColdObservable(
            OnNext(50, 42),
            OnCompleted<int>(70)
        );

        var res = scheduler.Start(() =>
            xs.Timeout(ys, _ => zs, ts)
        );

        res.Messages.AssertEqual(
            OnNext(310, 1),
            OnNext(350, 2),
            OnNext(450, 42),
            OnCompleted<int>(470)
        );

        xs.Subscriptions.AssertEqual(
            Subscribe(200, 400)
        );

        ys.Subscriptions.AssertEqual(
            Subscribe(200, 310)
        );

        zs.Subscriptions.AssertEqual(
            Subscribe(310, 350),
            Subscribe(350, 400)
        );

        ts.Subscriptions.AssertEqual(
            Subscribe(400, 470)
        );
    }

    [TestMethod]
    public void Timeout_Duration_Simple_TimeoutLater_NoFirst()
    {
        var scheduler = Scheduler;

        var xs = scheduler.CreateHotObservable(
            OnNext(310, 1),
            OnNext(350, 2),
            OnNext(420, 3),
            OnCompleted<int>(450)
        );

        var zs = scheduler.CreateColdObservable(
            OnNext(50, "Boo!")
        );

        var res = scheduler.Start(() =>
            xs.Timeout(_ => zs)
        );

        res.Messages.AssertEqual(
            OnNext(310, 1),
            OnNext(350, 2),
            OnError<int>(400, ex => ex is TimeoutException)
        );

        xs.Subscriptions.AssertEqual(
            Subscribe(200, 400)
        );

        zs.Subscriptions.AssertEqual(
            Subscribe(310, 350),
            Subscribe(350, 400)
        );
    }

    [TestMethod]
    public void Timeout_Duration_Simple_TimeoutLater_Other_NoFirst()
    {
        var scheduler = Scheduler;

        var xs = scheduler.CreateHotObservable(
            OnNext(310, 1),
            OnNext(350, 2),
            OnNext(420, 3),
            OnCompleted<int>(450)
        );

        var zs = scheduler.CreateColdObservable(
            OnNext(50, "Boo!")
        );

        var ts = scheduler.CreateColdObservable(
            OnNext(50, 42),
            OnCompleted<int>(70)
        );

        var res = scheduler.Start(() =>
            xs.Timeout(_ => zs, ts)
        );

        res.Messages.AssertEqual(
            OnNext(310, 1),
            OnNext(350, 2),
            OnNext(450, 42),
            OnCompleted<int>(470)
        );

        xs.Subscriptions.AssertEqual(
            Subscribe(200, 400)
        );

        zs.Subscriptions.AssertEqual(
            Subscribe(310, 350),
            Subscribe(350, 400)
        );

        ts.Subscriptions.AssertEqual(
            Subscribe(400, 470)
        );
    }

    [TestMethod]
    public void Timeout_Duration_Simple_TimeoutByCompletion()
    {
        var scheduler = Scheduler;

        var xs = scheduler.CreateHotObservable(
            OnNext(310, 1),
            OnNext(350, 2),
            OnNext(420, 3),
            OnCompleted<int>(450)
        );

        var ys = scheduler.CreateColdObservable<string>(
        );

        var zs = scheduler.CreateColdObservable(
            OnCompleted<string>(50)
        );

        var res = scheduler.Start(() =>
            xs.Timeout(ys, _ => zs)
        );

        res.Messages.AssertEqual(
            OnNext(310, 1),
            OnNext(350, 2),
            OnError<int>(400, ex => ex is TimeoutException)
        );

        xs.Subscriptions.AssertEqual(
            Subscribe(200, 400)
        );

        ys.Subscriptions.AssertEqual(
            Subscribe(200, 310)
        );

        zs.Subscriptions.AssertEqual(
            Subscribe(310, 350),
            Subscribe(350, 400)
        );
    }

    [TestMethod]
    public void Timeout_Duration_Simple_SelectorThrows()
    {
        var scheduler = Scheduler;

        var xs = scheduler.CreateHotObservable(
            OnNext(310, 1),
            OnNext(350, 2),
            OnNext(420, 3),
            OnCompleted<int>(450)
        );

        var ys = scheduler.CreateColdObservable<string>(
        );

        var zs = scheduler.CreateColdObservable<string>(
        );

        var ex = new Exception();

        var res = scheduler.Start(() =>
            xs.Timeout(ys, x =>
            {
                if (x < 3)
                {
                    return zs;
                }
                else
                {
                    throw ex;
                }
            })
        );

        res.Messages.AssertEqual(
            OnNext(310, 1),
            OnNext(350, 2),
            OnNext(420, 3),
            OnError<int>(420, ex)
        );

        xs.Subscriptions.AssertEqual(
            Subscribe(200, 420)
        );

        ys.Subscriptions.AssertEqual(
            Subscribe(200, 310)
        );

        zs.Subscriptions.AssertEqual(
            Subscribe(310, 350),
            Subscribe(350, 420)
        );
    }

    [TestMethod]
    public void Timeout_Duration_Simple_InnerThrows()
    {
        var ex = new Exception();

        var scheduler = Scheduler;

        var xs = scheduler.CreateHotObservable(
            OnNext(310, 1),
            OnNext(350, 2),
            OnNext(420, 3),
            OnCompleted<int>(450)
        );

        var ys = scheduler.CreateColdObservable<string>(
        );

        var zs = scheduler.CreateColdObservable(
            OnError<string>(50, ex)
        );

        var res = scheduler.Start(() =>
            xs.Timeout(ys, x => zs)
        );

        res.Messages.AssertEqual(
            OnNext(310, 1),
            OnNext(350, 2),
            OnError<int>(400, ex)
        );

        xs.Subscriptions.AssertEqual(
            Subscribe(200, 400)
        );

        ys.Subscriptions.AssertEqual(
            Subscribe(200, 310)
        );

        zs.Subscriptions.AssertEqual(
            Subscribe(310, 350),
            Subscribe(350, 400)
        );
    }

    [TestMethod]
    public void Timeout_Duration_Simple_FirstThrows()
    {
        var ex = new Exception();

        var scheduler = Scheduler;

        var xs = scheduler.CreateHotObservable(
            OnNext(310, 1),
            OnNext(350, 2),
            OnNext(420, 3),
            OnCompleted<int>(450)
        );

        var ys = scheduler.CreateColdObservable(
            OnError<string>(50, ex)
        );

        var zs = scheduler.CreateColdObservable<string>(
        );

        var res = scheduler.Start(() =>
            xs.Timeout(ys, x => zs)
        );

        res.Messages.AssertEqual(
            OnError<int>(250, ex)
        );

        xs.Subscriptions.AssertEqual(
            Subscribe(200, 250)
        );

        ys.Subscriptions.AssertEqual(
            Subscribe(200, 250)
        );

        zs.Subscriptions.AssertEqual(
        );
    }

    [TestMethod]
    public void Timeout_Duration_Simple_SourceThrows()
    {
        var ex = new Exception();

        var scheduler = Scheduler;

        var xs = scheduler.CreateHotObservable(
            OnNext(310, 1),
            OnNext(350, 2),
            OnNext(420, 3),
            OnError<int>(450, ex)
        );

        var ys = scheduler.CreateColdObservable<string>(
        );

        var zs = scheduler.CreateColdObservable<string>(
        );

        var res = scheduler.Start(() =>
            xs.Timeout(ys, x => zs)
        );

        res.Messages.AssertEqual(
            OnNext(310, 1),
            OnNext(350, 2),
            OnNext(420, 3),
            OnError<int>(450, ex)
        );

        xs.Subscriptions.AssertEqual(
            Subscribe(200, 450)
        );

        ys.Subscriptions.AssertEqual(
            Subscribe(200, 310)
        );

        zs.Subscriptions.AssertEqual(
            Subscribe(310, 350),
            Subscribe(350, 420),
            Subscribe(420, 450)
        );
    }
}

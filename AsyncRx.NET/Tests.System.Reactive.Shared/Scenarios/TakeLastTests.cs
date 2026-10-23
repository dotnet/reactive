// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information. 

namespace Tests.System.Reactive.Shared.Scenarios;

/// <summary>Shared <c>TakeLast</c> scenarios, from Rx.NET's <c>TakeLastTest.cs</c>.</summary>
/// <remarks>
/// <para>
/// 33 of the file's 35 tests (the two <c>*_ArgumentChecking</c> tests are the code-generated
/// stratum). The count and duration forms transcribe as they are. The nine
/// <c>*_WithLoopScheduler</c> tests drain on the test scheduler, one element per scheduled step
/// in Rx.NET, so where the original expects the drained elements and the completion one tick
/// apart after the source's end (<c>OnNext(231, 1), OnNext(232, 2), OnCompleted(233)</c>), the
/// shared text writes <c>ScheduledAt(230, 1)</c>, <c>ScheduledAt(230, 2)</c>,
/// <c>ScheduledAt(230, 3)</c>: Rx.NET's <c>TestScheduler</c> moves one tick per scheduling and
/// AsyncRx.NET's pump does not.
/// </para>
/// <para>
/// The six real-time tests await the result through
/// <see cref="SharedReactiveTest.ToListAsync{T}"/> where the originals block on an event or on
/// <c>ForEach</c>. The two <c>*_LongRunning_Regular</c> tests drain on the shared
/// <see cref="TestLongRunningScheduler"/>, which runs each unit of work on its own thread, where
/// the original uses <c>NewThreadScheduler</c>; on Rx.NET that is the long-running drain loop
/// the original exercises.
/// </para>
/// </remarks>
public abstract class TakeLastTests : SharedReactiveTest
{
    [TestMethod]
    public void TakeLast_Zero_Completed()
    {
        var scheduler = Scheduler;

        var xs = scheduler.CreateHotObservable(
            OnNext(180, 1),
            OnNext(210, 2),
            OnNext(250, 3),
            OnNext(270, 4),
            OnNext(310, 5),
            OnNext(360, 6),
            OnNext(380, 7),
            OnNext(410, 8),
            OnNext(590, 9),
            OnCompleted<int>(650)
        );

        var res = scheduler.Start(() =>
            xs.TakeLast(0)
        );

        res.Messages.AssertEqual(
            OnCompleted<int>(650)
        );

        xs.Subscriptions.AssertEqual(
            Subscribe(200, 650)
        );
    }

    [TestMethod]
    public void TakeLast_Zero_Error()
    {
        var scheduler = Scheduler;

        var ex = new Exception();

        var xs = scheduler.CreateHotObservable(
            OnNext(180, 1),
            OnNext(210, 2),
            OnNext(250, 3),
            OnNext(270, 4),
            OnNext(310, 5),
            OnNext(360, 6),
            OnNext(380, 7),
            OnNext(410, 8),
            OnNext(590, 9),
            OnError<int>(650, ex)
        );

        var res = scheduler.Start(() =>
            xs.TakeLast(0)
        );

        res.Messages.AssertEqual(
            OnError<int>(650, ex)
        );

        xs.Subscriptions.AssertEqual(
            Subscribe(200, 650)
        );
    }

    [TestMethod]
    public void TakeLast_Zero_Disposed()
    {
        var scheduler = Scheduler;

        var xs = scheduler.CreateHotObservable(
            OnNext(180, 1),
            OnNext(210, 2),
            OnNext(250, 3),
            OnNext(270, 4),
            OnNext(310, 5),
            OnNext(360, 6),
            OnNext(380, 7),
            OnNext(410, 8),
            OnNext(590, 9)
        );

        var res = scheduler.Start(() =>
            xs.TakeLast(0)
        );

        res.Messages.AssertEqual(
        );

        xs.Subscriptions.AssertEqual(
            Subscribe(200, 1000)
        );
    }

    [TestMethod]
    public void TakeLast_One_Completed()
    {
        var scheduler = Scheduler;

        var xs = scheduler.CreateHotObservable(
            OnNext(180, 1),
            OnNext(210, 2),
            OnNext(250, 3),
            OnNext(270, 4),
            OnNext(310, 5),
            OnNext(360, 6),
            OnNext(380, 7),
            OnNext(410, 8),
            OnNext(590, 9),
            OnCompleted<int>(650)
        );

        var res = scheduler.Start(() =>
            xs.TakeLast(1)
        );

        res.Messages.AssertEqual(
            OnNext(650, 9),
            OnCompleted<int>(650)
        );

        xs.Subscriptions.AssertEqual(
            Subscribe(200, 650)
        );
    }

    [TestMethod]
    public void TakeLast_One_Error()
    {
        var scheduler = Scheduler;

        var ex = new Exception();

        var xs = scheduler.CreateHotObservable(
            OnNext(180, 1),
            OnNext(210, 2),
            OnNext(250, 3),
            OnNext(270, 4),
            OnNext(310, 5),
            OnNext(360, 6),
            OnNext(380, 7),
            OnNext(410, 8),
            OnNext(590, 9),
            OnError<int>(650, ex)
        );

        var res = scheduler.Start(() =>
            xs.TakeLast(1)
        );

        res.Messages.AssertEqual(
            OnError<int>(650, ex)
        );

        xs.Subscriptions.AssertEqual(
            Subscribe(200, 650)
        );
    }

    [TestMethod]
    public void TakeLast_One_Disposed()
    {
        var scheduler = Scheduler;

        var xs = scheduler.CreateHotObservable(
            OnNext(180, 1),
            OnNext(210, 2),
            OnNext(250, 3),
            OnNext(270, 4),
            OnNext(310, 5),
            OnNext(360, 6),
            OnNext(380, 7),
            OnNext(410, 8),
            OnNext(590, 9)
        );

        var res = scheduler.Start(() =>
            xs.TakeLast(1)
        );

        res.Messages.AssertEqual(
        );

        xs.Subscriptions.AssertEqual(
            Subscribe(200, 1000)
        );
    }

    [TestMethod]
    public void TakeLast_Three_Completed()
    {
        var scheduler = Scheduler;

        var xs = scheduler.CreateHotObservable(
            OnNext(180, 1),
            OnNext(210, 2),
            OnNext(250, 3),
            OnNext(270, 4),
            OnNext(310, 5),
            OnNext(360, 6),
            OnNext(380, 7),
            OnNext(410, 8),
            OnNext(590, 9),
            OnCompleted<int>(650)
        );

        var res = scheduler.Start(() =>
            xs.TakeLast(3)
        );

        res.Messages.AssertEqual(
            OnNext(650, 7),
            OnNext(650, 8),
            OnNext(650, 9),
            OnCompleted<int>(650)
        );

        xs.Subscriptions.AssertEqual(
            Subscribe(200, 650)
        );
    }

    [TestMethod]
    public void TakeLast_Three_Error()
    {
        var scheduler = Scheduler;

        var ex = new Exception();

        var xs = scheduler.CreateHotObservable(
            OnNext(180, 1),
            OnNext(210, 2),
            OnNext(250, 3),
            OnNext(270, 4),
            OnNext(310, 5),
            OnNext(360, 6),
            OnNext(380, 7),
            OnNext(410, 8),
            OnNext(590, 9),
            OnError<int>(650, ex)
        );

        var res = scheduler.Start(() =>
            xs.TakeLast(3)
        );

        res.Messages.AssertEqual(
            OnError<int>(650, ex)
        );

        xs.Subscriptions.AssertEqual(
            Subscribe(200, 650)
        );
    }

    [TestMethod]
    public void TakeLast_Three_Disposed()
    {
        var scheduler = Scheduler;

        var xs = scheduler.CreateHotObservable(
            OnNext(180, 1),
            OnNext(210, 2),
            OnNext(250, 3),
            OnNext(270, 4),
            OnNext(310, 5),
            OnNext(360, 6),
            OnNext(380, 7),
            OnNext(410, 8),
            OnNext(590, 9)
        );

        var res = scheduler.Start(() =>
            xs.TakeLast(3)
        );

        res.Messages.AssertEqual(
        );

        xs.Subscriptions.AssertEqual(
            Subscribe(200, 1000)
        );
    }

    [TestMethod]
    public async Task TakeLast_LongRunning_Regular()
    {
        var res = await ToListAsync(
            Seq.Range(0, 100, DefaultScheduler).TakeLast(10, new TestLongRunningScheduler(Target)));

        Assert.IsTrue(Enumerable.Range(90, 10).SequenceEqual(res));
    }

    [TestMethod]
    public void TakeLast_Zero1()
    {
        var scheduler = Scheduler;

        var xs = scheduler.CreateHotObservable(
            OnNext(210, 1),
            OnNext(220, 2),
            OnCompleted<int>(230)
        );

        var res = scheduler.Start(() =>
            xs.TakeLast(TimeSpan.Zero, scheduler)
        );

        res.Messages.AssertEqual(
            OnCompleted<int>(230)
        );

        xs.Subscriptions.AssertEqual(
            Subscribe(200, 230)
        );
    }

    [TestMethod]
    public void TakeLast_Zero1_WithLoopScheduler()
    {
        var scheduler = Scheduler;

        var xs = scheduler.CreateHotObservable(
            OnNext(210, 1),
            OnNext(220, 2),
            OnCompleted<int>(230)
        );

        var res = scheduler.Start(() =>
            xs.TakeLast(TimeSpan.Zero, scheduler, scheduler)
        );

        res.Messages.AssertEqual(
            OnCompleted<int>(ScheduledAt(230, 1))
        );

        xs.Subscriptions.AssertEqual(
            Subscribe(200, 230)
        );
    }

    [TestMethod]
    public void TakeLast_Zero2()
    {
        var scheduler = Scheduler;

        var xs = scheduler.CreateHotObservable(
            OnNext(210, 1),
            OnNext(220, 2),
            OnNext(230, 3),
            OnCompleted<int>(230)
        );

        var res = scheduler.Start(() =>
            xs.TakeLast(TimeSpan.Zero, scheduler)
        );

        res.Messages.AssertEqual(
            OnCompleted<int>(230)
        );

        xs.Subscriptions.AssertEqual(
            Subscribe(200, 230)
        );
    }

    [TestMethod]
    public void TakeLast_Zero2_WithLoopScheduler()
    {
        var scheduler = Scheduler;

        var xs = scheduler.CreateHotObservable(
            OnNext(210, 1),
            OnNext(220, 2),
            OnNext(230, 3),
            OnCompleted<int>(230)
        );

        var res = scheduler.Start(() =>
            xs.TakeLast(TimeSpan.Zero, scheduler, scheduler)
        );

        res.Messages.AssertEqual(
            OnCompleted<int>(ScheduledAt(230, 1))
        );

        xs.Subscriptions.AssertEqual(
            Subscribe(200, 230)
        );
    }

    [TestMethod]
    public void TakeLast_Some1()
    {
        var scheduler = Scheduler;

        var xs = scheduler.CreateHotObservable(
            OnNext(210, 1),
            OnNext(220, 2),
            OnNext(230, 3),
            OnCompleted<int>(240)
        );

        var res = scheduler.Start(() =>
            xs.TakeLast(TimeSpan.FromTicks(25), scheduler)
        );

        res.Messages.AssertEqual(
            OnNext(240, 2),
            OnNext(240, 3),
            OnCompleted<int>(240)
        );

        xs.Subscriptions.AssertEqual(
            Subscribe(200, 240)
        );
    }

    [TestMethod]
    public void TakeLast_Some1_WithLoopScheduler()
    {
        var scheduler = Scheduler;

        var xs = scheduler.CreateHotObservable(
            OnNext(210, 1),
            OnNext(220, 2),
            OnNext(230, 3),
            OnCompleted<int>(240)
        );

        var res = scheduler.Start(() =>
            xs.TakeLast(TimeSpan.FromTicks(25), scheduler, scheduler)
        );

        res.Messages.AssertEqual(
            OnNext(ScheduledAt(240, 1), 2),
            OnNext(ScheduledAt(240, 2), 3),
            OnCompleted<int>(ScheduledAt(240, 3))
        );

        xs.Subscriptions.AssertEqual(
            Subscribe(200, 240)
        );
    }

    [TestMethod]
    public void TakeLast_Some2()
    {
        var scheduler = Scheduler;

        var xs = scheduler.CreateHotObservable(
            OnNext(210, 1),
            OnNext(220, 2),
            OnNext(230, 3),
            OnCompleted<int>(300)
        );

        var res = scheduler.Start(() =>
            xs.TakeLast(TimeSpan.FromTicks(25), scheduler)
        );

        res.Messages.AssertEqual(
            OnCompleted<int>(300)
        );

        xs.Subscriptions.AssertEqual(
            Subscribe(200, 300)
        );
    }

    [TestMethod]
    public void TakeLast_Some2_WithLoopScheduler()
    {
        var scheduler = Scheduler;

        var xs = scheduler.CreateHotObservable(
            OnNext(210, 1),
            OnNext(220, 2),
            OnNext(230, 3),
            OnCompleted<int>(300)
        );

        var res = scheduler.Start(() =>
            xs.TakeLast(TimeSpan.FromTicks(25), scheduler, scheduler)
        );

        res.Messages.AssertEqual(
            OnCompleted<int>(ScheduledAt(300, 1))
        );

        xs.Subscriptions.AssertEqual(
            Subscribe(200, 300)
        );
    }

    [TestMethod]
    public void TakeLast_Some3()
    {
        var scheduler = Scheduler;

        var xs = scheduler.CreateHotObservable(
            OnNext(210, 1),
            OnNext(220, 2),
            OnNext(230, 3),
            OnNext(240, 4),
            OnNext(250, 5),
            OnNext(260, 6),
            OnNext(270, 7),
            OnNext(280, 8),
            OnNext(290, 9),
            OnCompleted<int>(300)
        );

        var res = scheduler.Start(() =>
            xs.TakeLast(TimeSpan.FromTicks(45), scheduler)
        );

        res.Messages.AssertEqual(
            OnNext(300, 6),
            OnNext(300, 7),
            OnNext(300, 8),
            OnNext(300, 9),
            OnCompleted<int>(300)
        );

        xs.Subscriptions.AssertEqual(
            Subscribe(200, 300)
        );
    }

    [TestMethod]
    public void TakeLast_Some3_WithLoopScheduler()
    {
        var scheduler = Scheduler;

        var xs = scheduler.CreateHotObservable(
            OnNext(210, 1),
            OnNext(220, 2),
            OnNext(230, 3),
            OnNext(240, 4),
            OnNext(250, 5),
            OnNext(260, 6),
            OnNext(270, 7),
            OnNext(280, 8),
            OnNext(290, 9),
            OnCompleted<int>(300)
        );

        var res = scheduler.Start(() =>
            xs.TakeLast(TimeSpan.FromTicks(45), scheduler, scheduler)
        );

        res.Messages.AssertEqual(
            OnNext(ScheduledAt(300, 1), 6),
            OnNext(ScheduledAt(300, 2), 7),
            OnNext(ScheduledAt(300, 3), 8),
            OnNext(ScheduledAt(300, 4), 9),
            OnCompleted<int>(ScheduledAt(300, 5))
        );

        xs.Subscriptions.AssertEqual(
            Subscribe(200, 300)
        );
    }

    [TestMethod]
    public void TakeLast_Some4()
    {
        var scheduler = Scheduler;

        var xs = scheduler.CreateHotObservable(
            OnNext(210, 1),
            OnNext(240, 2),
            OnNext(250, 3),
            OnNext(280, 4),
            OnNext(290, 5),
            OnNext(300, 6),
            OnCompleted<int>(350)
        );

        var res = scheduler.Start(() =>
            xs.TakeLast(TimeSpan.FromTicks(25), scheduler)
        );

        res.Messages.AssertEqual(
            OnCompleted<int>(350)
        );

        xs.Subscriptions.AssertEqual(
            Subscribe(200, 350)
        );
    }

    [TestMethod]
    public void TakeLast_Some4_WithLoopScheduler()
    {
        var scheduler = Scheduler;

        var xs = scheduler.CreateHotObservable(
            OnNext(210, 1),
            OnNext(240, 2),
            OnNext(250, 3),
            OnNext(280, 4),
            OnNext(290, 5),
            OnNext(300, 6),
            OnCompleted<int>(350)
        );

        var res = scheduler.Start(() =>
            xs.TakeLast(TimeSpan.FromTicks(25), scheduler, scheduler)
        );

        res.Messages.AssertEqual(
            OnCompleted<int>(ScheduledAt(350, 1))
        );

        xs.Subscriptions.AssertEqual(
            Subscribe(200, 350)
        );
    }

    [TestMethod]
    public void TakeLast_All()
    {
        var scheduler = Scheduler;

        var xs = scheduler.CreateHotObservable(
            OnNext(210, 1),
            OnNext(220, 2),
            OnCompleted<int>(230)
        );

        var res = scheduler.Start(() =>
            xs.TakeLast(TimeSpan.FromTicks(50), scheduler)
        );

        res.Messages.AssertEqual(
            OnNext(230, 1),
            OnNext(230, 2),
            OnCompleted<int>(230)
        );

        xs.Subscriptions.AssertEqual(
            Subscribe(200, 230)
        );
    }

    [TestMethod]
    public void TakeLast_All_WithLoopScheduler()
    {
        var scheduler = Scheduler;

        var xs = scheduler.CreateHotObservable(
            OnNext(210, 1),
            OnNext(220, 2),
            OnCompleted<int>(230)
        );

        var res = scheduler.Start(() =>
            xs.TakeLast(TimeSpan.FromTicks(50), scheduler, scheduler)
        );

        res.Messages.AssertEqual(
            OnNext(ScheduledAt(230, 1), 1),
            OnNext(ScheduledAt(230, 2), 2),
            OnCompleted<int>(ScheduledAt(230, 3))
        );

        xs.Subscriptions.AssertEqual(
            Subscribe(200, 230)
        );
    }

    [TestMethod]
    public void TakeLast_Error()
    {
        var scheduler = Scheduler;

        var ex = new Exception();

        var xs = scheduler.CreateHotObservable(
            OnError<int>(210, ex)
        );

        var res = scheduler.Start(() =>
            xs.TakeLast(TimeSpan.FromTicks(50), scheduler)
        );

        res.Messages.AssertEqual(
            OnError<int>(210, ex)
        );

        xs.Subscriptions.AssertEqual(
            Subscribe(200, 210)
        );
    }

    [TestMethod]
    public void TakeLast_Error_WithLoopScheduler()
    {
        var scheduler = Scheduler;

        var ex = new Exception();

        var xs = scheduler.CreateHotObservable(
            OnError<int>(210, ex)
        );

        var res = scheduler.Start(() =>
            xs.TakeLast(TimeSpan.FromTicks(50), scheduler, scheduler)
        );

        res.Messages.AssertEqual(
            OnError<int>(210, ex)
        );

        xs.Subscriptions.AssertEqual(
            Subscribe(200, 210)
        );
    }

    [TestMethod]
    public void TakeLast_Never()
    {
        var scheduler = Scheduler;

        var ex = new Exception();

        var xs = scheduler.CreateHotObservable<int>(
        );

        var res = scheduler.Start(() =>
            xs.TakeLast(TimeSpan.FromTicks(50), scheduler)
        );

        res.Messages.AssertEqual(
        );

        xs.Subscriptions.AssertEqual(
            Subscribe(200, 1000)
        );
    }

    [TestMethod]
    public void TakeLast_Never_WithLoopScheduler()
    {
        var scheduler = Scheduler;

        var ex = new Exception();

        var xs = scheduler.CreateHotObservable<int>(
        );

        var res = scheduler.Start(() =>
            xs.TakeLast(TimeSpan.FromTicks(50), scheduler, scheduler)
        );

        res.Messages.AssertEqual(
        );

        xs.Subscriptions.AssertEqual(
            Subscribe(200, 1000)
        );
    }

    [TestMethod]
    public async Task TakeLast_Default1()
    {
        var res = await ToListAsync(
            Seq.Range(0, 10, DefaultScheduler).TakeLast(TimeSpan.FromSeconds(60)));

        Assert.IsTrue(res.SequenceEqual(Enumerable.Range(0, 10)));
    }

    [TestMethod]
    public async Task TakeLast_Default2()
    {
        var res = await ToListAsync(
            Seq.Range(0, 10, DefaultScheduler)
                .TakeLast(TimeSpan.FromSeconds(60), DisableOptimizations(DefaultScheduler)));

        Assert.IsTrue(res.SequenceEqual(Enumerable.Range(0, 10)));
    }

    [TestMethod]
    public async Task TakeLast_Default3()
    {
        var res = await ToListAsync(Seq.Range(0, 10, DefaultScheduler).TakeLast(TimeSpan.Zero));

        Assert.IsEmpty(res);
    }

    [TestMethod]
    public async Task TakeLast_Default4()
    {
        var res = await ToListAsync(
            Seq.Range(0, 10, DefaultScheduler)
                .TakeLast(TimeSpan.Zero, DisableOptimizations(DefaultScheduler)));

        Assert.IsEmpty(res);
    }

    [TestMethod]
    public async Task TakeLast_Timed_LongRunning_Regular()
    {
        var res = await ToListAsync(
            Seq.Range(0, 10, DefaultScheduler)
                .TakeLast(
                    TimeSpan.FromSeconds(60),
                    DefaultScheduler,
                    new TestLongRunningScheduler(Target)));

        Assert.IsTrue(Enumerable.Range(0, 10).SequenceEqual(res));
    }
}

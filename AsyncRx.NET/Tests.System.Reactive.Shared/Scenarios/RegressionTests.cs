// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information. 

namespace Tests.System.Reactive.Shared.Scenarios;

/// <summary>
/// Shared regression scenarios, from Rx.NET's <c>RegressionTest.cs</c>.
/// </summary>
/// <remarks>
/// 15 of the file's 22 tests, one of them in a reframed form: the regression guards over operators
/// both libraries have, each named for the Rx.NET issue it pins. Eleven are virtual-time tests
/// transcribed as they are (<c>Window</c> over time with a per-window <c>Return</c> and
/// <c>Merge</c>, <c>Throttle</c>, <c>Catch</c> and <c>OnErrorResumeNext</c> over no source, one
/// and several, <c>Delay</c>), with <c>ScheduledAt</c> where the original expects a scheduled
/// delivery a tick late. <c>Bug_1381</c> multicasts through a replay subject on the scheduler and
/// reads its connection, subscription and disposal as scheduled actions; it claims the order of
/// what a late subscriber receives, the replayed elements then the live ones, and the source's
/// subscription window. The original claims exact ticks too, which are Rx.NET's delivery model
/// (one scheduled unit per element, so live elements queue behind the replay) where AsyncRx.NET
/// replays within the tick and delivers live elements as they arrive; that form stays in the
/// Rx.NET runner as <c>Bug_1381_OneDeliveryPerTick</c>. <c>Bug_1286</c> subscribes and disposes a
/// <c>GroupBy</c> over an infinite <c>Repeat</c> on the default scheduler twice, awaiting a
/// millisecond where the original sleeps. The two <c>Merge_Trampoline</c> tests claim the
/// interleaving of a published enumerable merged with itself; the original reads it from a
/// synchronous subscribe on Rx.NET's current-thread trampoline, the scenario from
/// <c>ToListAsync</c>, which both targets complete. The other seven are the Rx.NET runner's: four
/// block on <c>First</c>, <c>ForEach</c> or a thread, two drive <c>Subject.Synchronize</c>
/// re-entrantly, which AsyncRx.NET has not got and refuses by the Step 81 decision, and
/// <c>Bug_1130</c> is over Rx.NET's own <c>Start</c> type, with an equivalent in the AsyncRx.NET
/// runner.
/// </remarks>
public abstract class RegressionTests : SharedReactiveTest
{
    [TestMethod]
    public void Bug_1283()
    {
    var scheduler = Scheduler;

        var xs = scheduler.CreateHotObservable(
            OnNext(100, 1),
            OnNext(220, 2),
            OnNext(240, 3),
            OnNext(300, 4),
            OnNext(310, 5),
            OnCompleted<int>(350)
        );

        var results = scheduler.Start(() =>
            xs.Window(TimeSpan.FromTicks(100), scheduler).Select((x, i) => x.Select(y => i.ToString() + " " + y.ToString()).Concat(Seq.Return(i.ToString() + " end", scheduler))).Merge()
        );

        results.Messages.AssertEqual(
            OnNext(220, "0 2"),
            OnNext(240, "0 3"),
            OnNext(300, "0 4"),
            OnNext(ScheduledAt(300), "0 end"),
            OnNext(310, "1 5"),
            OnNext(ScheduledAt(350), "1 end"),
            OnCompleted<string>(ScheduledAt(350))
        );
    }

    [TestMethod]
    public void Bug_1261()
    {
    var scheduler = Scheduler;

        var xs = scheduler.CreateHotObservable(
            OnNext(205, 1),
            OnNext(210, 2),
            OnNext(215, 3),
            OnNext(220, 4),
            OnNext(225, 5),
            OnNext(230, 6),
            OnCompleted<int>(230));

        var results = scheduler.Start(() =>
            xs.Window(TimeSpan.FromTicks(10), scheduler).Select((x, i) => x.Select(y => i.ToString() + " " + y.ToString()).Concat(Seq.Return(i.ToString() + " end", scheduler))).Merge()
        );

        results.Messages.AssertEqual(
            OnNext(205, "0 1"),
            OnNext(210, "0 2"),
            OnNext(ScheduledAt(210), "0 end"),
            OnNext(215, "1 3"),
            OnNext(220, "1 4"),
            OnNext(ScheduledAt(220), "1 end"),
            OnNext(225, "2 5"),
            OnNext(230, "2 6"),
            OnNext(ScheduledAt(230), "2 end"),
            OnCompleted<string>(ScheduledAt(230))
        );
    }

    [TestMethod]
    public async Task Bug_1286()
    {
        var infinite = Seq.Return(new { Name = "test", Value = 0d }, DefaultScheduler).Repeat();
        var grouped = infinite.GroupBy(x => x.Name, x => x.Value);

        var disp = await grouped.SubscribeAsync(Scheduler, _ => { });
        await Task.Delay(1);
        await disp.DisposeAsync();

        disp = await grouped.SubscribeAsync(Scheduler, _ => { });
        await Task.Delay(1);
        await disp.DisposeAsync();
    }

    [TestMethod]
    public void Bug_1295_Completed()
    {
    var scheduler = Scheduler;

        var xs = scheduler.CreateHotObservable(
            OnNext(300, 1),
            OnNext(350, 2),
            OnNext(500, 3),
            OnCompleted<int>(550)
        );

        var results = scheduler.Start(() =>
            xs.Throttle(TimeSpan.FromTicks(100), scheduler)
        );

        results.Messages.AssertEqual(
            OnNext(450, 2),
            OnNext(550, 3),
            OnCompleted<int>(550)
        );

        xs.Subscriptions.AssertEqual(
            Subscribe(200, 550)
        );
    }

    [TestMethod]
    public void Bug_1295_Error()
    {
    var scheduler = Scheduler;

        var ex = new Exception();

        var xs = scheduler.CreateHotObservable(
            OnNext(300, 1),
            OnNext(350, 2),
            OnNext(500, 3),
            OnError<int>(550, ex)
        );

        var results = scheduler.Start(() =>
            xs.Throttle(TimeSpan.FromTicks(100), scheduler)
        );

        results.Messages.AssertEqual(
            OnNext(450, 2),
            OnError<int>(550, ex)
        );

        xs.Subscriptions.AssertEqual(
            Subscribe(200, 550)
        );
    }

    [TestMethod]
    public void Bug_1297_Catch_None()
    {
    var scheduler = Scheduler;

        var results = scheduler.Start(() =>
            Seq.Catch<int>()
        );

        results.Messages.AssertEqual(
            OnCompleted<int>(200)
        );
    }

    [TestMethod]
    public void Bug_1297_OnErrorResumeNext_None()
    {
    var scheduler = Scheduler;

        var results = scheduler.Start(() =>
            Seq.OnErrorResumeNext<int>()
        );

        results.Messages.AssertEqual(
            OnCompleted<int>(200)
        );
    }

    [TestMethod]
    public void Bug_1297_Catch_Single()
    {
    var scheduler = Scheduler;

        var ex = new Exception();

        var xs = Seq.Throw<int>(ex, scheduler);

        var results = scheduler.Start(() =>
            Seq.Catch(xs)
        );

        results.Messages.AssertEqual(
            OnError<int>(ScheduledAt(200), ex)
        );
    }

    [TestMethod]
    public void Bug_1297_OnErrorResumeNext_Single()
    {
    var scheduler = Scheduler;

        var xs = Seq.Throw<int>(new Exception(), scheduler);

        var results = scheduler.Start(() =>
            Seq.OnErrorResumeNext(xs)
        );

        results.Messages.AssertEqual(
            OnCompleted<int>(ScheduledAt(200))
        );
    }

    [TestMethod]
    public void Bug_1297_Catch_Multi()
    {
    var scheduler = Scheduler;

        var ex1 = new Exception();
        var ex2 = new Exception();
        var ex3 = new Exception();

        var xs = Seq.Throw<int>(ex1, scheduler);
        var ys = Seq.Throw<int>(ex2, scheduler);
        var zs = Seq.Throw<int>(ex3, scheduler);

        var results = scheduler.Start(() =>
            Seq.Catch(xs, ys, zs)
        );

        results.Messages.AssertEqual(
            OnError<int>(ScheduledAt(200, 3), ex3)
        );
    }

    [TestMethod]
    public void Bug_1297_OnErrorResumeNext_Multi()
    {
    var scheduler = Scheduler;

        var ex1 = new Exception();
        var ex2 = new Exception();
        var ex3 = new Exception();

        var xs = Seq.Throw<int>(ex1, scheduler);
        var ys = Seq.Throw<int>(ex2, scheduler);
        var zs = Seq.Throw<int>(ex3, scheduler);

        var results = scheduler.Start(() =>
            Seq.OnErrorResumeNext(xs, ys, zs)
        );

        results.Messages.AssertEqual(
            OnCompleted<int>(ScheduledAt(200, 3))
        );
    }

    [TestMethod]
    public void Bug_1380()
    {
    var scheduler = Scheduler;

        var ex = new Exception();

        var xs = scheduler.CreateHotObservable(
            OnNext(220, 1),
            OnNext(250, 2),
            OnNext(270, 3),
            OnNext(290, 4),
            OnNext(310, 5),
            OnNext(340, 6),
            OnNext(360, 7),
            OnError<int>(380, ex)
        );

        var results = scheduler.Start(() =>
            xs.Delay(TimeSpan.FromTicks(100), scheduler)
        );

        results.Messages.AssertEqual(
            OnNext(320, 1),
            OnNext(350, 2),
            OnNext(370, 3),
            OnError<int>(380, ex)
        );

        xs.Subscriptions.AssertEqual(
            Subscribe(200, 380)
        );
    }

    [TestMethod]
    public void Bug_1381()
    {
        var scheduler = Scheduler;

        var xs = scheduler.CreateHotObservable(
            OnNext(90, 1),
            OnNext(110, 2),
            OnNext(250, 3),
            OnNext(270, 4),
            OnNext(280, 5),
            OnNext(301, 6),
            OnNext(302, 7),
            OnNext(400, 8),
            OnNext(401, 9),
            OnNext(510, 10)
        );

        var results = new List<int>();

        ConnectableSeq<int> ys = null!;
        IAsyncDisposable connection = null!;
        IAsyncDisposable subscription = null!;

        scheduler.ScheduleAbsolute(100, () => ys = xs.Multicast(CreateReplaySubject<int>(scheduler)));
        scheduler.ScheduleAbsolute(200, async () => connection = await ys.ConnectAsync(scheduler));
        scheduler.ScheduleAbsolute(300, async () => subscription = await ys.SubscribeAsync(scheduler, results.Add));
        scheduler.ScheduleAbsolute(500, async () => await subscription.DisposeAsync());
        scheduler.ScheduleAbsolute(600, async () => await connection.DisposeAsync());

        scheduler.Start();

        results.AssertEqual(3, 4, 5, 6, 7, 8, 9);

        xs.Subscriptions.AssertEqual(
            Subscribe(200, 600)
        );
    }

    [TestMethod]
    public async Task Merge_Trampoline1()
    {
        var ys = new[] { 1, 2, 3 }.ToSeq().Publish(xs => xs.Merge(xs));

        var list = await ToListAsync(ys);

        list.AssertEqual(1, 1, 2, 2, 3, 3);
    }

    [TestMethod]
    public async Task Merge_Trampoline2()
    {
        var ys = new[] { 1, 2, 3 }.ToSeq().Publish(xs => Seq.Merge(xs, xs, xs, xs));

        var list = await ToListAsync(ys);

        list.AssertEqual(1, 1, 1, 1, 2, 2, 2, 2, 3, 3, 3, 3);
    }
}

// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information. 

namespace Tests.System.Reactive.Shared.Scenarios;

/// <summary>Shared <c>Replay</c> scenarios, from Rx.NET's <c>ReplayTest.cs</c>.</summary>
/// <remarks>
/// <para>
/// Every behavioural test from that file: 24 of its 25 (the <c>*_ArgumentChecking</c> test is
/// the code-generated stratum). Two are bounded where the original is not:
/// <c>ReplayCountLambda_Zip_Complete</c> and <c>ReplayTimeLambda_Zip_Complete</c> write
/// <c>Take(6).Repeat(6)</c> and <c>Take(6).Repeat(8)</c> for the original's <c>Repeat()</c>.
/// What those tests verify is that a selector's resubscription to the replayed sequence after
/// the source has completed receives the buffer and then the completion, round after round;
/// the original shows two such rounds and then stops only because Rx.NET's
/// <c>TestScheduler</c> moves one tick per scheduling until the disposal at 610, a loop that a
/// pump which does not move time would never leave. The bounded form has the same rounds and
/// the same values, and then completes, on both targets; the counts are the number of
/// subscriptions the original makes before its disposal cuts it off. The replay subject
/// delivers through its scheduler, so where the
/// original expects a message one tick after the event that caused it (<c>OnNext(451, 5)</c>
/// for a replay on subscription at 450, <c>OnNext(521, 11)</c> for a live value at 520), the
/// shared text writes <c>ScheduledAt(450, 1)</c> and <c>ScheduledAt(520)</c>: Rx.NET's
/// <c>TestScheduler</c> moves one tick per scheduling, AsyncRx.NET's pump does not. The two
/// <c>MultipleConnections</c> tests connect from the test body, and use the scenario's
/// <c>Scheduler</c> where the original makes a throwaway <c>TestScheduler</c>. The eight
/// <c>*_Default*</c> tests compare the scheduler-less overload with its explicit
/// <c>DefaultScheduler</c> twin by collecting both through
/// <see cref="SharedReactiveTest.ToListAsync{T}"/>, where the original's
/// <c>ReactiveAssert.AssertEqual</c> blocks on each sequence.
/// </para>
/// </remarks>
public abstract class ReplayTests : SharedReactiveTest
{
    [TestMethod]
    public void ReplayCount_Basic()
    {
        var xs = Scheduler.CreateHotObservable(
            OnNext(110, 7),
            OnNext(220, 3),
            OnNext(280, 4),
            OnNext(290, 1),
            OnNext(340, 8),
            OnNext(360, 5),
            OnNext(370, 6),
            OnNext(390, 7),
            OnNext(410, 13),
            OnNext(430, 2),
            OnNext(450, 9),
            OnNext(520, 11),
            OnNext(560, 20),
            OnCompleted<int>(600)
        );

        var ys = default(ConnectableSeq<int>);
        var subscription = default(IAsyncDisposable);
        var connection = default(IAsyncDisposable);
        var res = Scheduler.CreateObserver<int>();

        Scheduler.ScheduleAbsolute(Created, () => ys = xs.Replay(3, Scheduler));
        Scheduler.ScheduleAbsolute(450, async () => subscription = await ys!.SubscribeAsync(res));
        Scheduler.ScheduleAbsolute(Disposed, async () => await subscription!.DisposeAsync());

        Scheduler.ScheduleAbsolute(300, async () => connection = await ys!.ConnectAsync(Scheduler));
        Scheduler.ScheduleAbsolute(400, async () => await connection!.DisposeAsync());

        Scheduler.ScheduleAbsolute(500, async () => connection = await ys!.ConnectAsync(Scheduler));
        Scheduler.ScheduleAbsolute(550, async () => await connection!.DisposeAsync());

        Scheduler.ScheduleAbsolute(650, async () => connection = await ys!.ConnectAsync(Scheduler));
        Scheduler.ScheduleAbsolute(800, async () => await connection!.DisposeAsync());

        Scheduler.Start();

        res.Messages.AssertEqual(
            OnNext(ScheduledAt(450), 5),
            OnNext(ScheduledAt(450, 2), 6),
            OnNext(ScheduledAt(450, 3), 7),
            OnNext(ScheduledAt(520), 11)
        );

        xs.Subscriptions.AssertEqual(
            Subscribe(300, 400),
            Subscribe(500, 550),
            Subscribe(650, 800)
        );
    }

    [TestMethod]
    public void ReplayCount_Error()
    {
        var ex = new Exception();

        var xs = Scheduler.CreateHotObservable(
            OnNext(110, 7),
            OnNext(220, 3),
            OnNext(280, 4),
            OnNext(290, 1),
            OnNext(340, 8),
            OnNext(360, 5),
            OnNext(370, 6),
            OnNext(390, 7),
            OnNext(410, 13),
            OnNext(430, 2),
            OnNext(450, 9),
            OnNext(520, 11),
            OnNext(560, 20),
            OnError<int>(600, ex)
        );

        var ys = default(ConnectableSeq<int>);
        var subscription = default(IAsyncDisposable);
        var connection = default(IAsyncDisposable);
        var res = Scheduler.CreateObserver<int>();

        Scheduler.ScheduleAbsolute(Created, () => ys = xs.Replay(3, Scheduler));
        Scheduler.ScheduleAbsolute(450, async () => subscription = await ys!.SubscribeAsync(res));
        Scheduler.ScheduleAbsolute(Disposed, async () => await subscription!.DisposeAsync());

        Scheduler.ScheduleAbsolute(300, async () => connection = await ys!.ConnectAsync(Scheduler));
        Scheduler.ScheduleAbsolute(400, async () => await connection!.DisposeAsync());

        Scheduler.ScheduleAbsolute(500, async () => connection = await ys!.ConnectAsync(Scheduler));
        Scheduler.ScheduleAbsolute(800, async () => await connection!.DisposeAsync());

        Scheduler.Start();

        res.Messages.AssertEqual(
            OnNext(ScheduledAt(450), 5),
            OnNext(ScheduledAt(450, 2), 6),
            OnNext(ScheduledAt(450, 3), 7),
            OnNext(ScheduledAt(520), 11),
            OnNext(ScheduledAt(560), 20),
            OnError<int>(ScheduledAt(600), ex)
        );

        xs.Subscriptions.AssertEqual(
            Subscribe(300, 400),
            Subscribe(500, 600)
        );
    }

    [TestMethod]
    public void ReplayCount_Complete()
    {
        var xs = Scheduler.CreateHotObservable(
            OnNext(110, 7),
            OnNext(220, 3),
            OnNext(280, 4),
            OnNext(290, 1),
            OnNext(340, 8),
            OnNext(360, 5),
            OnNext(370, 6),
            OnNext(390, 7),
            OnNext(410, 13),
            OnNext(430, 2),
            OnNext(450, 9),
            OnNext(520, 11),
            OnNext(560, 20),
            OnCompleted<int>(600)
        );

        var ys = default(ConnectableSeq<int>);
        var subscription = default(IAsyncDisposable);
        var connection = default(IAsyncDisposable);
        var res = Scheduler.CreateObserver<int>();

        Scheduler.ScheduleAbsolute(Created, () => ys = xs.Replay(3, Scheduler));
        Scheduler.ScheduleAbsolute(450, async () => subscription = await ys!.SubscribeAsync(res));
        Scheduler.ScheduleAbsolute(Disposed, async () => await subscription!.DisposeAsync());

        Scheduler.ScheduleAbsolute(300, async () => connection = await ys!.ConnectAsync(Scheduler));
        Scheduler.ScheduleAbsolute(400, async () => await connection!.DisposeAsync());

        Scheduler.ScheduleAbsolute(500, async () => connection = await ys!.ConnectAsync(Scheduler));
        Scheduler.ScheduleAbsolute(800, async () => await connection!.DisposeAsync());

        Scheduler.Start();

        res.Messages.AssertEqual(
            OnNext(ScheduledAt(450), 5),
            OnNext(ScheduledAt(450, 2), 6),
            OnNext(ScheduledAt(450, 3), 7),
            OnNext(ScheduledAt(520), 11),
            OnNext(ScheduledAt(560), 20),
            OnCompleted<int>(ScheduledAt(600))
        );

        xs.Subscriptions.AssertEqual(
            Subscribe(300, 400),
            Subscribe(500, 600)
        );
    }

    [TestMethod]
    public void ReplayCount_Dispose()
    {
        var xs = Scheduler.CreateHotObservable(
            OnNext(110, 7),
            OnNext(220, 3),
            OnNext(280, 4),
            OnNext(290, 1),
            OnNext(340, 8),
            OnNext(360, 5),
            OnNext(370, 6),
            OnNext(390, 7),
            OnNext(410, 13),
            OnNext(430, 2),
            OnNext(450, 9),
            OnNext(520, 11),
            OnNext(560, 20),
            OnCompleted<int>(600)
        );

        var ys = default(ConnectableSeq<int>);
        var subscription = default(IAsyncDisposable);
        var connection = default(IAsyncDisposable);
        var res = Scheduler.CreateObserver<int>();

        Scheduler.ScheduleAbsolute(Created, () => ys = xs.Replay(3, Scheduler));
        Scheduler.ScheduleAbsolute(450, async () => subscription = await ys!.SubscribeAsync(res));
        Scheduler.ScheduleAbsolute(475, async () => await subscription!.DisposeAsync());

        Scheduler.ScheduleAbsolute(300, async () => connection = await ys!.ConnectAsync(Scheduler));
        Scheduler.ScheduleAbsolute(400, async () => await connection!.DisposeAsync());

        Scheduler.ScheduleAbsolute(500, async () => connection = await ys!.ConnectAsync(Scheduler));
        Scheduler.ScheduleAbsolute(550, async () => await connection!.DisposeAsync());

        Scheduler.ScheduleAbsolute(650, async () => connection = await ys!.ConnectAsync(Scheduler));
        Scheduler.ScheduleAbsolute(800, async () => await connection!.DisposeAsync());

        Scheduler.Start();

        res.Messages.AssertEqual(
            OnNext(ScheduledAt(450), 5),
            OnNext(ScheduledAt(450, 2), 6),
            OnNext(ScheduledAt(450, 3), 7)
        );

        xs.Subscriptions.AssertEqual(
            Subscribe(300, 400),
            Subscribe(500, 550),
            Subscribe(650, 800)
        );
    }

    [TestMethod]
    public async Task ReplayCount_MultipleConnections()
    {
        var xs = Seq.Never<int>();
        var ys = xs.Replay(3, Scheduler);

        var connection1 = await ys.ConnectAsync(Scheduler);
        var connection2 = await ys.ConnectAsync(Scheduler);

        Assert.AreSame(connection1, connection2);

        await connection1.DisposeAsync();
        await connection2.DisposeAsync();

        var connection3 = await ys.ConnectAsync(Scheduler);

        Assert.AreNotSame(connection1, connection3);

        await connection3.DisposeAsync();
    }

    [TestMethod]
    public void ReplayCountLambda_Zip_Complete()
    {
        var xs = Scheduler.CreateHotObservable(
            OnNext(110, 7),
            OnNext(220, 3),
            OnNext(280, 4),
            OnNext(290, 1),
            OnNext(340, 8),
            OnNext(360, 5),
            OnNext(370, 6),
            OnNext(390, 7),
            OnNext(410, 13),
            OnNext(430, 2),
            OnNext(450, 9),
            OnNext(520, 11),
            OnNext(560, 20),
            OnCompleted<int>(600)
        );

        var res = Scheduler.Start(() =>
            xs.Replay(replayed => replayed.Take(6).Repeat(6), 3, Scheduler),
            610
        );

        res.Messages.AssertEqual(
            OnNext(ScheduledAt(220), 3),
            OnNext(ScheduledAt(280), 4),
            OnNext(ScheduledAt(290), 1),
            OnNext(ScheduledAt(340), 8),
            OnNext(ScheduledAt(360), 5),
            OnNext(ScheduledAt(370), 6),
            OnNext(ScheduledAt(370, 2), 8),
            OnNext(ScheduledAt(370, 3), 5),
            OnNext(ScheduledAt(370, 4), 6),
            OnNext(ScheduledAt(390), 7),
            OnNext(ScheduledAt(410), 13),
            OnNext(ScheduledAt(430), 2),
            OnNext(ScheduledAt(430, 2), 7),
            OnNext(ScheduledAt(430, 3), 13),
            OnNext(ScheduledAt(430, 4), 2),
            OnNext(ScheduledAt(450), 9),
            OnNext(ScheduledAt(520), 11),
            OnNext(ScheduledAt(560), 20),
            OnNext(ScheduledAt(560, 2), 9),
            OnNext(ScheduledAt(560, 3), 11),
            OnNext(ScheduledAt(560, 4), 20),
            OnNext(ScheduledAt(600, 2), 9),
            OnNext(ScheduledAt(600, 3), 11),
            OnNext(ScheduledAt(600, 4), 20),
            OnNext(ScheduledAt(600, 6), 9),
            OnNext(ScheduledAt(600, 7), 11),
            OnNext(ScheduledAt(600, 8), 20),
            OnCompleted<int>(ScheduledAt(600, 9))
        );

        xs.Subscriptions.AssertEqual(
            Subscribe(200, 600)
        );
    }



    [TestMethod]
    public void ReplayCountLambda_Zip_Error()
    {
        var ex = new Exception();

        var xs = Scheduler.CreateHotObservable(
            OnNext(110, 7),
            OnNext(220, 3),
            OnNext(280, 4),
            OnNext(290, 1),
            OnNext(340, 8),
            OnNext(360, 5),
            OnNext(370, 6),
            OnNext(390, 7),
            OnNext(410, 13),
            OnNext(430, 2),
            OnNext(450, 9),
            OnNext(520, 11),
            OnNext(560, 20),
            OnError<int>(600, ex)
        );

        var res = Scheduler.Start(() =>
            xs.Replay(replayed => replayed.Take(6).Repeat(), 3, Scheduler)
        );

        res.Messages.AssertEqual(
            OnNext(ScheduledAt(220), 3),
            OnNext(ScheduledAt(280), 4),
            OnNext(ScheduledAt(290), 1),
            OnNext(ScheduledAt(340), 8),
            OnNext(ScheduledAt(360), 5),
            OnNext(ScheduledAt(370), 6),
            OnNext(ScheduledAt(370, 2), 8),
            OnNext(ScheduledAt(370, 3), 5),
            OnNext(ScheduledAt(370, 4), 6),
            OnNext(ScheduledAt(390), 7),
            OnNext(ScheduledAt(410), 13),
            OnNext(ScheduledAt(430), 2),
            OnNext(ScheduledAt(430, 2), 7),
            OnNext(ScheduledAt(430, 3), 13),
            OnNext(ScheduledAt(430, 4), 2),
            OnNext(ScheduledAt(450), 9),
            OnNext(ScheduledAt(520), 11),
            OnNext(ScheduledAt(560), 20),
            OnNext(ScheduledAt(560, 2), 9),
            OnNext(ScheduledAt(560, 3), 11),
            OnNext(ScheduledAt(560, 4), 20),
            OnError<int>(ScheduledAt(600), ex)
        );

        xs.Subscriptions.AssertEqual(
            Subscribe(200, 600)
        );
    }

    [TestMethod]
    public void ReplayCountLambda_Zip_Dispose()
    {
        var xs = Scheduler.CreateHotObservable(
            OnNext(110, 7),
            OnNext(220, 3),
            OnNext(280, 4),
            OnNext(290, 1),
            OnNext(340, 8),
            OnNext(360, 5),
            OnNext(370, 6),
            OnNext(390, 7),
            OnNext(410, 13),
            OnNext(430, 2),
            OnNext(450, 9),
            OnNext(520, 11),
            OnNext(560, 20),
            OnCompleted<int>(600)
        );

        var res = Scheduler.Start(() =>
            xs.Replay(replayed => replayed.Take(6).Repeat(), 3, Scheduler),
            470
        );

        res.Messages.AssertEqual(
            OnNext(ScheduledAt(220), 3),
            OnNext(ScheduledAt(280), 4),
            OnNext(ScheduledAt(290), 1),
            OnNext(ScheduledAt(340), 8),
            OnNext(ScheduledAt(360), 5),
            OnNext(ScheduledAt(370), 6),
            OnNext(ScheduledAt(370, 2), 8),
            OnNext(ScheduledAt(370, 3), 5),
            OnNext(ScheduledAt(370, 4), 6),
            OnNext(ScheduledAt(390), 7),
            OnNext(ScheduledAt(410), 13),
            OnNext(ScheduledAt(430), 2),
            OnNext(ScheduledAt(430, 2), 7),
            OnNext(ScheduledAt(430, 3), 13),
            OnNext(ScheduledAt(430, 4), 2),
            OnNext(ScheduledAt(450), 9)
        );

        xs.Subscriptions.AssertEqual(
            Subscribe(200, 470)
        );
    }

    [TestMethod]
    public void ReplayTime_Basic()
    {
        var xs = Scheduler.CreateHotObservable(
            OnNext(110, 7),
            OnNext(220, 3),
            OnNext(280, 4),
            OnNext(290, 1),
            OnNext(340, 8),
            OnNext(360, 5),
            OnNext(370, 6),
            OnNext(390, 7),
            OnNext(410, 13),
            OnNext(430, 2),
            OnNext(450, 9),
            OnNext(520, 11),
            OnNext(560, 20),
            OnCompleted<int>(600)
        );

        var ys = default(ConnectableSeq<int>);
        var subscription = default(IAsyncDisposable);
        var connection = default(IAsyncDisposable);
        var res = Scheduler.CreateObserver<int>();

        Scheduler.ScheduleAbsolute(
            Created,
            () => ys = xs.Replay(TimeSpan.FromTicks(150), Scheduler));
        Scheduler.ScheduleAbsolute(450, async () => subscription = await ys!.SubscribeAsync(res));
        Scheduler.ScheduleAbsolute(Disposed, async () => await subscription!.DisposeAsync());

        Scheduler.ScheduleAbsolute(300, async () => connection = await ys!.ConnectAsync(Scheduler));
        Scheduler.ScheduleAbsolute(400, async () => await connection!.DisposeAsync());

        Scheduler.ScheduleAbsolute(500, async () => connection = await ys!.ConnectAsync(Scheduler));
        Scheduler.ScheduleAbsolute(550, async () => await connection!.DisposeAsync());

        Scheduler.ScheduleAbsolute(650, async () => connection = await ys!.ConnectAsync(Scheduler));
        Scheduler.ScheduleAbsolute(800, async () => await connection!.DisposeAsync());

        Scheduler.Start();

        res.Messages.AssertEqual(
            OnNext(ScheduledAt(450), 8),
            OnNext(ScheduledAt(450, 2), 5),
            OnNext(ScheduledAt(450, 3), 6),
            OnNext(ScheduledAt(450, 4), 7),
            OnNext(ScheduledAt(520), 11)
        );

        xs.Subscriptions.AssertEqual(
            Subscribe(300, 400),
            Subscribe(500, 550),
            Subscribe(650, 800)
        );
    }

    [TestMethod]
    public void ReplayTime_Error()
    {
        var ex = new Exception();

        var xs = Scheduler.CreateHotObservable(
            OnNext(110, 7),
            OnNext(220, 3),
            OnNext(280, 4),
            OnNext(290, 1),
            OnNext(340, 8),
            OnNext(360, 5),
            OnNext(370, 6),
            OnNext(390, 7),
            OnNext(410, 13),
            OnNext(430, 2),
            OnNext(450, 9),
            OnNext(520, 11),
            OnNext(560, 20),
            OnError<int>(600, ex)
        );

        var ys = default(ConnectableSeq<int>);
        var subscription = default(IAsyncDisposable);
        var connection = default(IAsyncDisposable);
        var res = Scheduler.CreateObserver<int>();

        Scheduler.ScheduleAbsolute(
            Created,
            () => ys = xs.Replay(TimeSpan.FromTicks(75), Scheduler));
        Scheduler.ScheduleAbsolute(450, async () => subscription = await ys!.SubscribeAsync(res));
        Scheduler.ScheduleAbsolute(Disposed, async () => await subscription!.DisposeAsync());

        Scheduler.ScheduleAbsolute(300, async () => connection = await ys!.ConnectAsync(Scheduler));
        Scheduler.ScheduleAbsolute(400, async () => await connection!.DisposeAsync());

        Scheduler.ScheduleAbsolute(500, async () => connection = await ys!.ConnectAsync(Scheduler));
        Scheduler.ScheduleAbsolute(800, async () => await connection!.DisposeAsync());

        Scheduler.Start();

        res.Messages.AssertEqual(
            OnNext(ScheduledAt(450), 7),
            OnNext(ScheduledAt(520), 11),
            OnNext(ScheduledAt(560), 20),
            OnError<int>(ScheduledAt(600), ex)
        );

        xs.Subscriptions.AssertEqual(
            Subscribe(300, 400),
            Subscribe(500, 600)
        );
    }

    [TestMethod]
    public void ReplayTime_Complete()
    {
        var xs = Scheduler.CreateHotObservable(
            OnNext(110, 7),
            OnNext(220, 3),
            OnNext(280, 4),
            OnNext(290, 1),
            OnNext(340, 8),
            OnNext(360, 5),
            OnNext(370, 6),
            OnNext(390, 7),
            OnNext(410, 13),
            OnNext(430, 2),
            OnNext(450, 9),
            OnNext(520, 11),
            OnNext(560, 20),
            OnCompleted<int>(600)
        );

        var ys = default(ConnectableSeq<int>);
        var subscription = default(IAsyncDisposable);
        var connection = default(IAsyncDisposable);
        var res = Scheduler.CreateObserver<int>();

        Scheduler.ScheduleAbsolute(
            Created,
            () => ys = xs.Replay(TimeSpan.FromTicks(85), Scheduler));
        Scheduler.ScheduleAbsolute(450, async () => subscription = await ys!.SubscribeAsync(res));
        Scheduler.ScheduleAbsolute(Disposed, async () => await subscription!.DisposeAsync());

        Scheduler.ScheduleAbsolute(300, async () => connection = await ys!.ConnectAsync(Scheduler));
        Scheduler.ScheduleAbsolute(400, async () => await connection!.DisposeAsync());

        Scheduler.ScheduleAbsolute(500, async () => connection = await ys!.ConnectAsync(Scheduler));
        Scheduler.ScheduleAbsolute(800, async () => await connection!.DisposeAsync());

        Scheduler.Start();

        res.Messages.AssertEqual(
            OnNext(ScheduledAt(450), 6),
            OnNext(ScheduledAt(450, 2), 7),
            OnNext(ScheduledAt(520), 11),
            OnNext(ScheduledAt(560), 20),
            OnCompleted<int>(ScheduledAt(600))
        );

        xs.Subscriptions.AssertEqual(
            Subscribe(300, 400),
            Subscribe(500, 600)
        );
    }

    [TestMethod]
    public void ReplayTime_Dispose()
    {
        var xs = Scheduler.CreateHotObservable(
            OnNext(110, 7),
            OnNext(220, 3),
            OnNext(280, 4),
            OnNext(290, 1),
            OnNext(340, 8),
            OnNext(360, 5),
            OnNext(370, 6),
            OnNext(390, 7),
            OnNext(410, 13),
            OnNext(430, 2),
            OnNext(450, 9),
            OnNext(520, 11),
            OnNext(560, 20),
            OnCompleted<int>(600)
        );

        var ys = default(ConnectableSeq<int>);
        var subscription = default(IAsyncDisposable);
        var connection = default(IAsyncDisposable);
        var res = Scheduler.CreateObserver<int>();

        Scheduler.ScheduleAbsolute(
            Created,
            () => ys = xs.Replay(TimeSpan.FromTicks(100), Scheduler));
        Scheduler.ScheduleAbsolute(450, async () => subscription = await ys!.SubscribeAsync(res));
        Scheduler.ScheduleAbsolute(475, async () => await subscription!.DisposeAsync());

        Scheduler.ScheduleAbsolute(300, async () => connection = await ys!.ConnectAsync(Scheduler));
        Scheduler.ScheduleAbsolute(400, async () => await connection!.DisposeAsync());

        Scheduler.ScheduleAbsolute(500, async () => connection = await ys!.ConnectAsync(Scheduler));
        Scheduler.ScheduleAbsolute(550, async () => await connection!.DisposeAsync());

        Scheduler.ScheduleAbsolute(650, async () => connection = await ys!.ConnectAsync(Scheduler));
        Scheduler.ScheduleAbsolute(800, async () => await connection!.DisposeAsync());

        Scheduler.Start();

        res.Messages.AssertEqual(
            OnNext(ScheduledAt(450), 5),
            OnNext(ScheduledAt(450, 2), 6),
            OnNext(ScheduledAt(450, 3), 7)
        );

        xs.Subscriptions.AssertEqual(
            Subscribe(300, 400),
            Subscribe(500, 550),
            Subscribe(650, 800)
        );
    }

    [TestMethod]
    public async Task ReplayTime_MultipleConnections()
    {
        var xs = Seq.Never<int>();
        var ys = xs.Replay(TimeSpan.FromTicks(100), Scheduler);

        var connection1 = await ys.ConnectAsync(Scheduler);
        var connection2 = await ys.ConnectAsync(Scheduler);

        Assert.AreSame(connection1, connection2);

        await connection1.DisposeAsync();
        await connection2.DisposeAsync();

        var connection3 = await ys.ConnectAsync(Scheduler);

        Assert.AreNotSame(connection1, connection3);

        await connection3.DisposeAsync();
    }

    [TestMethod]
    public void ReplayTimeLambda_Zip_Complete()
    {
        var xs = Scheduler.CreateHotObservable(
            OnNext(110, 7),
            OnNext(220, 3),
            OnNext(280, 4),
            OnNext(290, 1),
            OnNext(340, 8),
            OnNext(360, 5),
            OnNext(370, 6),
            OnNext(390, 7),
            OnNext(410, 13),
            OnNext(430, 2),
            OnNext(450, 9),
            OnNext(520, 11),
            OnNext(560, 20),
            OnCompleted<int>(600)
        );

        var res = Scheduler.Start(() =>
            xs.Replay(
                replayed => replayed.Take(6).Repeat(8),
                TimeSpan.FromTicks(50),
                Scheduler),
            610
        );

        res.Messages.AssertEqual(
            OnNext(ScheduledAt(220), 3),
            OnNext(ScheduledAt(280), 4),
            OnNext(ScheduledAt(290), 1),
            OnNext(ScheduledAt(340), 8),
            OnNext(ScheduledAt(360), 5),
            OnNext(ScheduledAt(370), 6),
            OnNext(ScheduledAt(370, 2), 8),
            OnNext(ScheduledAt(370, 3), 5),
            OnNext(ScheduledAt(370, 4), 6),
            OnNext(ScheduledAt(390), 7),
            OnNext(ScheduledAt(410), 13),
            OnNext(ScheduledAt(430), 2),
            OnNext(ScheduledAt(430, 2), 7),
            OnNext(ScheduledAt(430, 3), 13),
            OnNext(ScheduledAt(430, 4), 2),
            OnNext(ScheduledAt(450), 9),
            OnNext(ScheduledAt(520), 11),
            OnNext(ScheduledAt(560), 20),
            OnNext(ScheduledAt(560, 2), 11),
            OnNext(ScheduledAt(560, 3), 20),
            OnNext(ScheduledAt(600, 2), 20),
            OnNext(ScheduledAt(600, 4), 20),
            OnNext(ScheduledAt(600, 6), 20),
            OnNext(ScheduledAt(600, 8), 20),
            OnCompleted<int>(ScheduledAt(600, 9))
        );

        xs.Subscriptions.AssertEqual(
            Subscribe(200, 600)
        );
    }

    [TestMethod]
    public void ReplayTimeLambda_Zip_Error()
    {
        var ex = new Exception();

        var xs = Scheduler.CreateHotObservable(
            OnNext(110, 7),
            OnNext(220, 3),
            OnNext(280, 4),
            OnNext(290, 1),
            OnNext(340, 8),
            OnNext(360, 5),
            OnNext(370, 6),
            OnNext(390, 7),
            OnNext(410, 13),
            OnNext(430, 2),
            OnNext(450, 9),
            OnNext(520, 11),
            OnNext(560, 20),
            OnError<int>(600, ex)
        );

        var res = Scheduler.Start(() =>
            xs.Replay(
                replayed => replayed.Take(6).Repeat(),
                TimeSpan.FromTicks(50),
                Scheduler)
        );

        res.Messages.AssertEqual(
            OnNext(ScheduledAt(220), 3),
            OnNext(ScheduledAt(280), 4),
            OnNext(ScheduledAt(290), 1),
            OnNext(ScheduledAt(340), 8),
            OnNext(ScheduledAt(360), 5),
            OnNext(ScheduledAt(370), 6),
            OnNext(ScheduledAt(370, 2), 8),
            OnNext(ScheduledAt(370, 3), 5),
            OnNext(ScheduledAt(370, 4), 6),
            OnNext(ScheduledAt(390), 7),
            OnNext(ScheduledAt(410), 13),
            OnNext(ScheduledAt(430), 2),
            OnNext(ScheduledAt(430, 2), 7),
            OnNext(ScheduledAt(430, 3), 13),
            OnNext(ScheduledAt(430, 4), 2),
            OnNext(ScheduledAt(450), 9),
            OnNext(ScheduledAt(520), 11),
            OnNext(ScheduledAt(560), 20),
            OnNext(ScheduledAt(560, 2), 11),
            OnNext(ScheduledAt(560, 3), 20),
            OnError<int>(ScheduledAt(600), ex)
        );

        xs.Subscriptions.AssertEqual(
            Subscribe(200, 600)
        );
    }

    [TestMethod]
    public void ReplayTimeLambda_Zip_Dispose()
    {
        var xs = Scheduler.CreateHotObservable(
            OnNext(110, 7),
            OnNext(220, 3),
            OnNext(280, 4),
            OnNext(290, 1),
            OnNext(340, 8),
            OnNext(360, 5),
            OnNext(370, 6),
            OnNext(390, 7),
            OnNext(410, 13),
            OnNext(430, 2),
            OnNext(450, 9),
            OnNext(520, 11),
            OnNext(560, 20),
            OnCompleted<int>(600)
        );

        var res = Scheduler.Start(() =>
            xs.Replay(
                replayed => replayed.Take(6).Repeat(),
                TimeSpan.FromTicks(50),
                Scheduler),
            470
        );

        res.Messages.AssertEqual(
            OnNext(ScheduledAt(220), 3),
            OnNext(ScheduledAt(280), 4),
            OnNext(ScheduledAt(290), 1),
            OnNext(ScheduledAt(340), 8),
            OnNext(ScheduledAt(360), 5),
            OnNext(ScheduledAt(370), 6),
            OnNext(ScheduledAt(370, 2), 8),
            OnNext(ScheduledAt(370, 3), 5),
            OnNext(ScheduledAt(370, 4), 6),
            OnNext(ScheduledAt(390), 7),
            OnNext(ScheduledAt(410), 13),
            OnNext(ScheduledAt(430), 2),
            OnNext(ScheduledAt(430, 2), 7),
            OnNext(ScheduledAt(430, 3), 13),
            OnNext(ScheduledAt(430, 4), 2),
            OnNext(ScheduledAt(450), 9)
        );

        xs.Subscriptions.AssertEqual(
            Subscribe(200, 470)
        );
    }

    [TestMethod]
    public async Task Replay_Default1()
    {
        var s = CreateSubject<int>();
        var xs = s.Replay(100, DefaultScheduler);
        var ys = s.Replay(100);

        await xs.ConnectAsync(Scheduler);
        await ys.ConnectAsync(Scheduler);

        await s.OnNextAsync(1);
        await s.OnNextAsync(2);
        await s.OnCompletedAsync();

        Assert.IsTrue((await ToListAsync(xs)).SequenceEqual(await ToListAsync(ys)));
    }

    [TestMethod]
    public async Task Replay_Default2()
    {
        var s = CreateSubject<int>();
        var xs = s.Replay(TimeSpan.FromHours(1), DefaultScheduler);
        var ys = s.Replay(TimeSpan.FromHours(1));

        await xs.ConnectAsync(Scheduler);
        await ys.ConnectAsync(Scheduler);

        await s.OnNextAsync(1);
        await s.OnNextAsync(2);
        await s.OnCompletedAsync();

        Assert.IsTrue((await ToListAsync(xs)).SequenceEqual(await ToListAsync(ys)));
    }

    [TestMethod]
    public async Task Replay_Default3()
    {
        var s = CreateSubject<int>();
        var xs = s.Replay(100, TimeSpan.FromHours(1), DefaultScheduler);
        var ys = s.Replay(100, TimeSpan.FromHours(1));

        await xs.ConnectAsync(Scheduler);
        await ys.ConnectAsync(Scheduler);

        await s.OnNextAsync(1);
        await s.OnNextAsync(2);
        await s.OnCompletedAsync();

        Assert.IsTrue((await ToListAsync(xs)).SequenceEqual(await ToListAsync(ys)));
    }

    [TestMethod]
    public async Task Replay_Default4()
    {
        var s = CreateSubject<int>();
        var xs = s.Replay(DefaultScheduler);
        var ys = s.Replay();

        await xs.ConnectAsync(Scheduler);
        await ys.ConnectAsync(Scheduler);

        await s.OnNextAsync(1);
        await s.OnNextAsync(2);
        await s.OnCompletedAsync();

        Assert.IsTrue((await ToListAsync(xs)).SequenceEqual(await ToListAsync(ys)));
    }

    [TestMethod]
    public async Task ReplayLambda_Default1()
    {
        var xs = Seq.Range(1, 10).Replay(replayed => replayed, 100, DefaultScheduler);
        var ys = Seq.Range(1, 10).Replay(replayed => replayed, 100);

        Assert.IsTrue((await ToListAsync(xs)).SequenceEqual(await ToListAsync(ys)));
    }

    [TestMethod]
    public async Task ReplayLambda_Default2()
    {
        var xs = Seq.Range(1, 10)
            .Replay(replayed => replayed, TimeSpan.FromHours(1), DefaultScheduler);
        var ys = Seq.Range(1, 10).Replay(replayed => replayed, TimeSpan.FromHours(1));

        Assert.IsTrue((await ToListAsync(xs)).SequenceEqual(await ToListAsync(ys)));
    }

    [TestMethod]
    public async Task ReplayLambda_Default3()
    {
        var xs = Seq.Range(1, 10)
            .Replay(replayed => replayed, 100, TimeSpan.FromHours(1), DefaultScheduler);
        var ys = Seq.Range(1, 10).Replay(replayed => replayed, 100, TimeSpan.FromHours(1));

        Assert.IsTrue((await ToListAsync(xs)).SequenceEqual(await ToListAsync(ys)));
    }

    [TestMethod]
    public async Task ReplayLambda_Default4()
    {
        var xs = Seq.Range(1, 10).Replay(replayed => replayed, DefaultScheduler);
        var ys = Seq.Range(1, 10).Replay(replayed => replayed);

        Assert.IsTrue((await ToListAsync(xs)).SequenceEqual(await ToListAsync(ys)));
    }
}

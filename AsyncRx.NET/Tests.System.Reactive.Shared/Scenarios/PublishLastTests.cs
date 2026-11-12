// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information. 

namespace Tests.System.Reactive.Shared.Scenarios;

/// <summary>Shared <c>PublishLast</c> scenarios, from Rx.NET's <c>PublishLastTest.cs</c>.</summary>
/// <remarks>
/// Every behavioural test from that file: 8 of its 9 (the <c>*_ArgumentChecking</c> test is the
/// code-generated stratum). The shape is <c>Publish</c>'s: a connectable built at one tick,
/// subscribed and connected at others, and a selector form, here over the async subject that
/// delivers only the last value, on completion, with no scheduler and so no scheduled-delivery
/// ticks. <c>PublishLast_MultipleConnections</c> connects from the test body.
/// </remarks>
public abstract class PublishLastTests : SharedReactiveTest
{
    [TestMethod]
    public void PublishLast_Basic()
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

        Scheduler.ScheduleAbsolute(Created, () => ys = xs.PublishLast());
        Scheduler.ScheduleAbsolute(
            Subscribed,
            async () => subscription = await ys!.SubscribeAsync(res));
        Scheduler.ScheduleAbsolute(Disposed, async () => await subscription!.DisposeAsync());

        Scheduler.ScheduleAbsolute(300, async () => connection = await ys!.ConnectAsync(Scheduler));
        Scheduler.ScheduleAbsolute(400, async () => await connection!.DisposeAsync());

        Scheduler.ScheduleAbsolute(500, async () => connection = await ys!.ConnectAsync(Scheduler));
        Scheduler.ScheduleAbsolute(550, async () => await connection!.DisposeAsync());

        Scheduler.ScheduleAbsolute(650, async () => connection = await ys!.ConnectAsync(Scheduler));
        Scheduler.ScheduleAbsolute(800, async () => await connection!.DisposeAsync());

        Scheduler.Start();

        res.Messages.AssertEqual(
        );

        xs.Subscriptions.AssertEqual(
            Subscribe(300, 400),
            Subscribe(500, 550),
            Subscribe(650, 800)
        );
    }

    [TestMethod]
    public void PublishLast_Error()
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

        Scheduler.ScheduleAbsolute(Created, () => ys = xs.PublishLast());
        Scheduler.ScheduleAbsolute(
            Subscribed,
            async () => subscription = await ys!.SubscribeAsync(res));
        Scheduler.ScheduleAbsolute(Disposed, async () => await subscription!.DisposeAsync());

        Scheduler.ScheduleAbsolute(300, async () => connection = await ys!.ConnectAsync(Scheduler));
        Scheduler.ScheduleAbsolute(400, async () => await connection!.DisposeAsync());

        Scheduler.ScheduleAbsolute(500, async () => connection = await ys!.ConnectAsync(Scheduler));
        Scheduler.ScheduleAbsolute(800, async () => await connection!.DisposeAsync());

        Scheduler.Start();

        res.Messages.AssertEqual(
            OnError<int>(600, ex)
        );

        xs.Subscriptions.AssertEqual(
            Subscribe(300, 400),
            Subscribe(500, 600)
        );
    }

    [TestMethod]
    public void PublishLast_Complete()
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

        Scheduler.ScheduleAbsolute(Created, () => ys = xs.PublishLast());
        Scheduler.ScheduleAbsolute(
            Subscribed,
            async () => subscription = await ys!.SubscribeAsync(res));
        Scheduler.ScheduleAbsolute(Disposed, async () => await subscription!.DisposeAsync());

        Scheduler.ScheduleAbsolute(300, async () => connection = await ys!.ConnectAsync(Scheduler));
        Scheduler.ScheduleAbsolute(400, async () => await connection!.DisposeAsync());

        Scheduler.ScheduleAbsolute(500, async () => connection = await ys!.ConnectAsync(Scheduler));
        Scheduler.ScheduleAbsolute(800, async () => await connection!.DisposeAsync());

        Scheduler.Start();

        res.Messages.AssertEqual(
            OnNext(600, 20),
            OnCompleted<int>(600)
        );

        xs.Subscriptions.AssertEqual(
            Subscribe(300, 400),
            Subscribe(500, 600)
        );
    }

    [TestMethod]
    public void PublishLast_Dispose()
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

        Scheduler.ScheduleAbsolute(Created, () => ys = xs.PublishLast());
        Scheduler.ScheduleAbsolute(
            Subscribed,
            async () => subscription = await ys!.SubscribeAsync(res));
        Scheduler.ScheduleAbsolute(350, async () => await subscription!.DisposeAsync());

        Scheduler.ScheduleAbsolute(300, async () => connection = await ys!.ConnectAsync(Scheduler));
        Scheduler.ScheduleAbsolute(400, async () => await connection!.DisposeAsync());

        Scheduler.ScheduleAbsolute(500, async () => connection = await ys!.ConnectAsync(Scheduler));
        Scheduler.ScheduleAbsolute(550, async () => await connection!.DisposeAsync());

        Scheduler.ScheduleAbsolute(650, async () => connection = await ys!.ConnectAsync(Scheduler));
        Scheduler.ScheduleAbsolute(800, async () => await connection!.DisposeAsync());

        Scheduler.Start();

        res.Messages.AssertEqual(
        );

        xs.Subscriptions.AssertEqual(
            Subscribe(300, 400),
            Subscribe(500, 550),
            Subscribe(650, 800)
        );
    }

    [TestMethod]
    public async Task PublishLast_MultipleConnections()
    {
        var xs = Seq.Never<int>();
        var ys = xs.PublishLast();

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
    public void PublishLastLambda_Zip_Complete()
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
            xs.PublishLast(published => published.Zip(published, (x, y) => x + y))
        );

        res.Messages.AssertEqual(
            OnNext(600, 40),
            OnCompleted<int>(600)
        );

        xs.Subscriptions.AssertEqual(
            Subscribe(200, 600)
        );
    }

    [TestMethod]
    public void PublishLastLambda_Zip_Error()
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
            xs.PublishLast(published => published.Zip(published, (x, y) => x + y))
        );

        res.Messages.AssertEqual(
            OnError<int>(600, ex)
        );

        xs.Subscriptions.AssertEqual(
            Subscribe(200, 600)
        );
    }

    [TestMethod]
    public void PublishLastLambda_Zip_Dispose()
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
            xs.PublishLast(published => published.Zip(published, (x, y) => x + y)),
            470
        );

        res.Messages.AssertEqual(
        );

        xs.Subscriptions.AssertEqual(
            Subscribe(200, 470)
        );
    }
}

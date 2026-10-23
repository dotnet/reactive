// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information. 

namespace Tests.System.Reactive.Shared.Scenarios;

/// <summary>Shared <c>Publish</c> scenarios, from Rx.NET's <c>PublishTest.cs</c>.</summary>
/// <remarks>
/// <para>
/// Every behavioural test from that file: 18 of its 20. Deliberately not here: the two
/// <c>*_ArgumentChecking</c> tests (the code-generated stratum).
/// </para>
/// <para>
/// The first operator whose result has identity. A scenario builds <c>ys = xs.Publish()</c> at
/// one tick, subscribes to <c>ys</c> at another and connects it at several more, and every use
/// must reach the same subject; a description materializes once, so it does. <c>ys</c> is a
/// <see cref="ConnectableSeq{T}"/>, connected through its <c>ConnectAsync</c> on the raw surface,
/// which returns the connection to dispose. <c>Publish_MultipleConnections</c> compares two
/// connections by identity, as the original does.
/// </para>
/// <para>
/// <c>PublishWithInitialValue_SanityCheck</c> compares two whole sequences through
/// <see cref="SharedReactiveTest.ToListAsync{T}"/> where the original uses the blocking
/// <c>AssertEqual</c> over observables; its unused <c>someObservable</c> local is dropped. The
/// <c>PublishLambda_Zip_*</c> tests name their selector's parameter <c>ys</c> where the original
/// has <c>_xs</c>, which the repository's naming rule for parameters does not allow.
/// </para>
/// </remarks>
public abstract class PublishTests : SharedReactiveTest
{
    [TestMethod]
    public void Publish_Cold_Zip()
    {
        var xs = Scheduler.CreateHotObservable(
            OnNext(40, 0),
            OnNext(90, 1),
            OnNext(150, 2),
            OnNext(210, 3),
            OnNext(240, 4),
            OnNext(270, 5),
            OnNext(330, 6),
            OnNext(340, 7),
            OnCompleted<int>(390)
        );

        var res = Scheduler.Start(() =>
            xs.Publish(ys => ys.Zip(ys, (a, b) => a + b))
        );

        res.Messages.AssertEqual(
            OnNext(210, 6),
            OnNext(240, 8),
            OnNext(270, 10),
            OnNext(330, 12),
            OnNext(340, 14),
            OnCompleted<int>(390)
        );

        xs.Subscriptions.AssertEqual(
            Subscribe(200, 390)
        );
    }

    [TestMethod]
    public void Publish_Basic()
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

        Scheduler.ScheduleAbsolute(Created, () => ys = xs.Publish());
        Scheduler.ScheduleAbsolute(Subscribed, async () => subscription = await ys!.SubscribeAsync(res));
        Scheduler.ScheduleAbsolute(Disposed, async () => await subscription!.DisposeAsync());

        Scheduler.ScheduleAbsolute(300, async () => connection = await ys!.ConnectAsync(Scheduler));
        Scheduler.ScheduleAbsolute(400, async () => await connection!.DisposeAsync());

        Scheduler.ScheduleAbsolute(500, async () => connection = await ys!.ConnectAsync(Scheduler));
        Scheduler.ScheduleAbsolute(550, async () => await connection!.DisposeAsync());

        Scheduler.ScheduleAbsolute(650, async () => connection = await ys!.ConnectAsync(Scheduler));
        Scheduler.ScheduleAbsolute(800, async () => await connection!.DisposeAsync());

        Scheduler.Start();

        res.Messages.AssertEqual(
            OnNext(340, 8),
            OnNext(360, 5),
            OnNext(370, 6),
            OnNext(390, 7),
            OnNext(520, 11)
        );

        xs.Subscriptions.AssertEqual(
            Subscribe(300, 400),
            Subscribe(500, 550),
            Subscribe(650, 800)
        );
    }

    [TestMethod]
    public void Publish_Error()
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

        Scheduler.ScheduleAbsolute(Created, () => ys = xs.Publish());
        Scheduler.ScheduleAbsolute(Subscribed, async () => subscription = await ys!.SubscribeAsync(res));
        Scheduler.ScheduleAbsolute(Disposed, async () => await subscription!.DisposeAsync());

        Scheduler.ScheduleAbsolute(300, async () => connection = await ys!.ConnectAsync(Scheduler));
        Scheduler.ScheduleAbsolute(400, async () => await connection!.DisposeAsync());

        Scheduler.ScheduleAbsolute(500, async () => connection = await ys!.ConnectAsync(Scheduler));
        Scheduler.ScheduleAbsolute(800, async () => await connection!.DisposeAsync());

        Scheduler.Start();

        res.Messages.AssertEqual(
            OnNext(340, 8),
            OnNext(360, 5),
            OnNext(370, 6),
            OnNext(390, 7),
            OnNext(520, 11),
            OnNext(560, 20),
            OnError<int>(600, ex)
        );

        xs.Subscriptions.AssertEqual(
            Subscribe(300, 400),
            Subscribe(500, 600)
        );
    }

    [TestMethod]
    public void Publish_Complete()
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

        Scheduler.ScheduleAbsolute(Created, () => ys = xs.Publish());
        Scheduler.ScheduleAbsolute(Subscribed, async () => subscription = await ys!.SubscribeAsync(res));
        Scheduler.ScheduleAbsolute(Disposed, async () => await subscription!.DisposeAsync());

        Scheduler.ScheduleAbsolute(300, async () => connection = await ys!.ConnectAsync(Scheduler));
        Scheduler.ScheduleAbsolute(400, async () => await connection!.DisposeAsync());

        Scheduler.ScheduleAbsolute(500, async () => connection = await ys!.ConnectAsync(Scheduler));
        Scheduler.ScheduleAbsolute(800, async () => await connection!.DisposeAsync());

        Scheduler.Start();

        res.Messages.AssertEqual(
            OnNext(340, 8),
            OnNext(360, 5),
            OnNext(370, 6),
            OnNext(390, 7),
            OnNext(520, 11),
            OnNext(560, 20),
            OnCompleted<int>(600)
        );

        xs.Subscriptions.AssertEqual(
            Subscribe(300, 400),
            Subscribe(500, 600)
        );
    }

    [TestMethod]
    public void Publish_Dispose()
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

        Scheduler.ScheduleAbsolute(Created, () => ys = xs.Publish());
        Scheduler.ScheduleAbsolute(Subscribed, async () => subscription = await ys!.SubscribeAsync(res));
        Scheduler.ScheduleAbsolute(350, async () => await subscription!.DisposeAsync());

        Scheduler.ScheduleAbsolute(300, async () => connection = await ys!.ConnectAsync(Scheduler));
        Scheduler.ScheduleAbsolute(400, async () => await connection!.DisposeAsync());

        Scheduler.ScheduleAbsolute(500, async () => connection = await ys!.ConnectAsync(Scheduler));
        Scheduler.ScheduleAbsolute(550, async () => await connection!.DisposeAsync());

        Scheduler.ScheduleAbsolute(650, async () => connection = await ys!.ConnectAsync(Scheduler));
        Scheduler.ScheduleAbsolute(800, async () => await connection!.DisposeAsync());

        Scheduler.Start();

        res.Messages.AssertEqual(
            OnNext(340, 8)
        );

        xs.Subscriptions.AssertEqual(
            Subscribe(300, 400),
            Subscribe(500, 550),
            Subscribe(650, 800)
        );
    }

    [TestMethod]
    public async Task Publish_MultipleConnections()
    {
        var xs = Seq.Never<int>();
        var ys = xs.Publish();

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
    public void PublishLambda_Zip_Complete()
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
            xs.Publish(ys => ys.Zip(ys.Skip(1), (prev, cur) => cur + prev))
        );

        res.Messages.AssertEqual(
            OnNext(280, 7),
            OnNext(290, 5),
            OnNext(340, 9),
            OnNext(360, 13),
            OnNext(370, 11),
            OnNext(390, 13),
            OnNext(410, 20),
            OnNext(430, 15),
            OnNext(450, 11),
            OnNext(520, 20),
            OnNext(560, 31),
            OnCompleted<int>(600)
        );

        xs.Subscriptions.AssertEqual(
            Subscribe(200, 600)
        );
    }

    [TestMethod]
    public void PublishLambda_Zip_Error()
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
            xs.Publish(ys => ys.Zip(ys.Skip(1), (prev, cur) => cur + prev))
        );

        res.Messages.AssertEqual(
            OnNext(280, 7),
            OnNext(290, 5),
            OnNext(340, 9),
            OnNext(360, 13),
            OnNext(370, 11),
            OnNext(390, 13),
            OnNext(410, 20),
            OnNext(430, 15),
            OnNext(450, 11),
            OnNext(520, 20),
            OnNext(560, 31),
            OnError<int>(600, ex)
        );

        xs.Subscriptions.AssertEqual(
            Subscribe(200, 600)
        );
    }

    [TestMethod]
    public void PublishLambda_Zip_Dispose()
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
            xs.Publish(ys => ys.Zip(ys.Skip(1), (prev, cur) => cur + prev)),
            470
        );

        res.Messages.AssertEqual(
            OnNext(280, 7),
            OnNext(290, 5),
            OnNext(340, 9),
            OnNext(360, 13),
            OnNext(370, 11),
            OnNext(390, 13),
            OnNext(410, 20),
            OnNext(430, 15),
            OnNext(450, 11)
        );

        xs.Subscriptions.AssertEqual(
            Subscribe(200, 470)
        );
    }

    [TestMethod]
    public async Task PublishWithInitialValue_SanityCheck()
    {
        var res = await ToListAsync(Seq.Publish(Seq.Range(1, 10), x => x, 0));

        Assert.IsTrue(res.SequenceEqual(await ToListAsync(Seq.Range(0, 11))));
    }

    [TestMethod]
    public void PublishWithInitialValue_Basic()
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

        Scheduler.ScheduleAbsolute(Created, () => ys = xs.Publish(1979));
        Scheduler.ScheduleAbsolute(Subscribed, async () => subscription = await ys!.SubscribeAsync(res));
        Scheduler.ScheduleAbsolute(Disposed, async () => await subscription!.DisposeAsync());

        Scheduler.ScheduleAbsolute(300, async () => connection = await ys!.ConnectAsync(Scheduler));
        Scheduler.ScheduleAbsolute(400, async () => await connection!.DisposeAsync());

        Scheduler.ScheduleAbsolute(500, async () => connection = await ys!.ConnectAsync(Scheduler));
        Scheduler.ScheduleAbsolute(550, async () => await connection!.DisposeAsync());

        Scheduler.ScheduleAbsolute(650, async () => connection = await ys!.ConnectAsync(Scheduler));
        Scheduler.ScheduleAbsolute(800, async () => await connection!.DisposeAsync());

        Scheduler.Start();

        res.Messages.AssertEqual(
            OnNext(200, 1979),
            OnNext(340, 8),
            OnNext(360, 5),
            OnNext(370, 6),
            OnNext(390, 7),
            OnNext(520, 11)
        );

        xs.Subscriptions.AssertEqual(
            Subscribe(300, 400),
            Subscribe(500, 550),
            Subscribe(650, 800)
        );
    }

    [TestMethod]
    public void PublishWithInitialValue_Error()
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

        Scheduler.ScheduleAbsolute(Created, () => ys = xs.Publish(1979));
        Scheduler.ScheduleAbsolute(Subscribed, async () => subscription = await ys!.SubscribeAsync(res));
        Scheduler.ScheduleAbsolute(Disposed, async () => await subscription!.DisposeAsync());

        Scheduler.ScheduleAbsolute(300, async () => connection = await ys!.ConnectAsync(Scheduler));
        Scheduler.ScheduleAbsolute(400, async () => await connection!.DisposeAsync());

        Scheduler.ScheduleAbsolute(500, async () => connection = await ys!.ConnectAsync(Scheduler));
        Scheduler.ScheduleAbsolute(800, async () => await connection!.DisposeAsync());

        Scheduler.Start();

        res.Messages.AssertEqual(
            OnNext(200, 1979),
            OnNext(340, 8),
            OnNext(360, 5),
            OnNext(370, 6),
            OnNext(390, 7),
            OnNext(520, 11),
            OnNext(560, 20),
            OnError<int>(600, ex)
        );

        xs.Subscriptions.AssertEqual(
            Subscribe(300, 400),
            Subscribe(500, 600)
        );
    }

    [TestMethod]
    public void PublishWithInitialValue_Complete()
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

        Scheduler.ScheduleAbsolute(Created, () => ys = xs.Publish(1979));
        Scheduler.ScheduleAbsolute(Subscribed, async () => subscription = await ys!.SubscribeAsync(res));
        Scheduler.ScheduleAbsolute(Disposed, async () => await subscription!.DisposeAsync());

        Scheduler.ScheduleAbsolute(300, async () => connection = await ys!.ConnectAsync(Scheduler));
        Scheduler.ScheduleAbsolute(400, async () => await connection!.DisposeAsync());

        Scheduler.ScheduleAbsolute(500, async () => connection = await ys!.ConnectAsync(Scheduler));
        Scheduler.ScheduleAbsolute(800, async () => await connection!.DisposeAsync());

        Scheduler.Start();

        res.Messages.AssertEqual(
            OnNext(200, 1979),
            OnNext(340, 8),
            OnNext(360, 5),
            OnNext(370, 6),
            OnNext(390, 7),
            OnNext(520, 11),
            OnNext(560, 20),
            OnCompleted<int>(600)
        );

        xs.Subscriptions.AssertEqual(
            Subscribe(300, 400),
            Subscribe(500, 600)
        );
    }

    [TestMethod]
    public void PublishWithInitialValue_Dispose()
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

        Scheduler.ScheduleAbsolute(Created, () => ys = xs.Publish(1979));
        Scheduler.ScheduleAbsolute(Subscribed, async () => subscription = await ys!.SubscribeAsync(res));
        Scheduler.ScheduleAbsolute(350, async () => await subscription!.DisposeAsync());

        Scheduler.ScheduleAbsolute(300, async () => connection = await ys!.ConnectAsync(Scheduler));
        Scheduler.ScheduleAbsolute(400, async () => await connection!.DisposeAsync());

        Scheduler.ScheduleAbsolute(500, async () => connection = await ys!.ConnectAsync(Scheduler));
        Scheduler.ScheduleAbsolute(550, async () => await connection!.DisposeAsync());

        Scheduler.ScheduleAbsolute(650, async () => connection = await ys!.ConnectAsync(Scheduler));
        Scheduler.ScheduleAbsolute(800, async () => await connection!.DisposeAsync());

        Scheduler.Start();

        res.Messages.AssertEqual(
            OnNext(200, 1979),
            OnNext(340, 8)
        );

        xs.Subscriptions.AssertEqual(
            Subscribe(300, 400),
            Subscribe(500, 550),
            Subscribe(650, 800)
        );
    }

    [TestMethod]
    public async Task PublishWithInitialValue_MultipleConnections()
    {
        var xs = Seq.Never<int>();
        var ys = xs.Publish(1979);

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
    public void PublishWithInitialValueLambda_Zip_Complete()
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
            xs.Publish(ys => ys.Zip(ys.Skip(1), (prev, cur) => cur + prev), 1979)
        );

        res.Messages.AssertEqual(
            OnNext(220, 1982),
            OnNext(280, 7),
            OnNext(290, 5),
            OnNext(340, 9),
            OnNext(360, 13),
            OnNext(370, 11),
            OnNext(390, 13),
            OnNext(410, 20),
            OnNext(430, 15),
            OnNext(450, 11),
            OnNext(520, 20),
            OnNext(560, 31),
            OnCompleted<int>(600)
        );

        xs.Subscriptions.AssertEqual(
            Subscribe(200, 600)
        );
    }

    [TestMethod]
    public void PublishWithInitialValueLambda_Zip_Error()
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
            xs.Publish(ys => ys.Zip(ys.Skip(1), (prev, cur) => cur + prev), 1979)
        );

        res.Messages.AssertEqual(
            OnNext(220, 1982),
            OnNext(280, 7),
            OnNext(290, 5),
            OnNext(340, 9),
            OnNext(360, 13),
            OnNext(370, 11),
            OnNext(390, 13),
            OnNext(410, 20),
            OnNext(430, 15),
            OnNext(450, 11),
            OnNext(520, 20),
            OnNext(560, 31),
            OnError<int>(600, ex)
        );

        xs.Subscriptions.AssertEqual(
            Subscribe(200, 600)
        );
    }

    [TestMethod]
    public void PublishWithInitialValueLambda_Zip_Dispose()
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
            xs.Publish(ys => ys.Zip(ys.Skip(1), (prev, cur) => cur + prev), 1979),
            470
        );

        res.Messages.AssertEqual(
            OnNext(220, 1982),
            OnNext(280, 7),
            OnNext(290, 5),
            OnNext(340, 9),
            OnNext(360, 13),
            OnNext(370, 11),
            OnNext(390, 13),
            OnNext(410, 20),
            OnNext(430, 15),
            OnNext(450, 11)
        );

        xs.Subscriptions.AssertEqual(
            Subscribe(200, 470)
        );
    }
}

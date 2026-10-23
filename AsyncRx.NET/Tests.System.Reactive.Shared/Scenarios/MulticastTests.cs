// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information. 

namespace Tests.System.Reactive.Shared.Scenarios;

/// <summary>Shared <c>Multicast</c> scenarios, from Rx.NET's <c>MulticastTest.cs</c>.</summary>
/// <remarks>
/// Every behavioural test from that file: 12 of its 13 (the <c>*_ArgumentChecking</c> test is
/// the code-generated stratum). This is the first operator whose input is a subject:
/// <c>new Subject&lt;int&gt;()</c> in the original is <c>CreateSubject&lt;int&gt;()</c>, a
/// <see cref="SubjectSeq{T}"/> the bridge maps to the target's subject type, and a subject
/// factory returns one likewise.
/// </remarks>
public abstract class MulticastTests : SharedReactiveTest
{
    [TestMethod]
    public void Multicast_Hot_1()
    {
        var s = CreateSubject<int>();

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

        var c = default(ConnectableSeq<int>);
        var o = Scheduler.CreateObserver<int>();
        var d1 = default(IAsyncDisposable);
        var d2 = default(IAsyncDisposable);

        Scheduler.ScheduleAbsolute(50, () => c = xs.Multicast(s));
        Scheduler.ScheduleAbsolute(100, async () => d1 = await c!.SubscribeAsync(o));
        Scheduler.ScheduleAbsolute(200, async () => d2 = await c!.ConnectAsync(Scheduler));
        Scheduler.ScheduleAbsolute(300, async () => await d1!.DisposeAsync());

        Scheduler.Start();

        o.Messages.AssertEqual(
            OnNext(210, 3),
            OnNext(240, 4),
            OnNext(270, 5)
        );

        xs.Subscriptions.AssertEqual(
            Subscribe(200, 390)
        );
    }

    [TestMethod]
    public void Multicast_Hot_2()
    {
        var s = CreateSubject<int>();

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

        var c = default(ConnectableSeq<int>);
        var o = Scheduler.CreateObserver<int>();
        var d1 = default(IAsyncDisposable);
        var d2 = default(IAsyncDisposable);

        Scheduler.ScheduleAbsolute(50, () => c = xs.Multicast(s));
        Scheduler.ScheduleAbsolute(100, async () => d2 = await c!.ConnectAsync(Scheduler));
        Scheduler.ScheduleAbsolute(200, async () => d1 = await c!.SubscribeAsync(o));
        Scheduler.ScheduleAbsolute(300, async () => await d1!.DisposeAsync());

        Scheduler.Start();

        o.Messages.AssertEqual(
            OnNext(210, 3),
            OnNext(240, 4),
            OnNext(270, 5)
        );

        xs.Subscriptions.AssertEqual(
            Subscribe(100, 390)
        );
    }

    [TestMethod]
    public void Multicast_Hot_3()
    {
        var s = CreateSubject<int>();

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

        var c = default(ConnectableSeq<int>);
        var o = Scheduler.CreateObserver<int>();
        var d1 = default(IAsyncDisposable);
        var d2 = default(IAsyncDisposable);

        Scheduler.ScheduleAbsolute(50, () => c = xs.Multicast(s));
        Scheduler.ScheduleAbsolute(100, async () => d2 = await c!.ConnectAsync(Scheduler));
        Scheduler.ScheduleAbsolute(200, async () => d1 = await c!.SubscribeAsync(o));
        Scheduler.ScheduleAbsolute(300, async () => await d2!.DisposeAsync());
        Scheduler.ScheduleAbsolute(335, async () => d2 = await c!.ConnectAsync(Scheduler));

        Scheduler.Start();

        o.Messages.AssertEqual(
            OnNext(210, 3),
            OnNext(240, 4),
            OnNext(270, 5),
            OnNext(340, 7),
            OnCompleted<int>(390)
        );

        xs.Subscriptions.AssertEqual(
            Subscribe(100, 300),
            Subscribe(335, 390)
        );
    }

    [TestMethod]
    public void Multicast_Hot_4()
    {
        var s = CreateSubject<int>();
        var ex = new Exception();

        var xs = Scheduler.CreateHotObservable(
            OnNext(40, 0),
            OnNext(90, 1),
            OnNext(150, 2),
            OnNext(210, 3),
            OnNext(240, 4),
            OnNext(270, 5),
            OnNext(330, 6),
            OnNext(340, 7),
            OnError<int>(390, ex)
        );

        var c = default(ConnectableSeq<int>);
        var o = Scheduler.CreateObserver<int>();
        var d1 = default(IAsyncDisposable);
        var d2 = default(IAsyncDisposable);

        Scheduler.ScheduleAbsolute(50, () => c = xs.Multicast(s));
        Scheduler.ScheduleAbsolute(100, async () => d2 = await c!.ConnectAsync(Scheduler));
        Scheduler.ScheduleAbsolute(200, async () => d1 = await c!.SubscribeAsync(o));
        Scheduler.ScheduleAbsolute(300, async () => await d2!.DisposeAsync());
        Scheduler.ScheduleAbsolute(335, async () => d2 = await c!.ConnectAsync(Scheduler));

        Scheduler.Start();

        o.Messages.AssertEqual(
            OnNext(210, 3),
            OnNext(240, 4),
            OnNext(270, 5),
            OnNext(340, 7),
            OnError<int>(390, ex)
        );

        xs.Subscriptions.AssertEqual(
            Subscribe(100, 300),
            Subscribe(335, 390)
        );
    }

    [TestMethod]
    public void Multicast_Hot_5()
    {
        var s = CreateSubject<int>();
        var ex = new Exception();

        var xs = Scheduler.CreateHotObservable(
            OnNext(40, 0),
            OnNext(90, 1),
            OnNext(150, 2),
            OnNext(210, 3),
            OnNext(240, 4),
            OnNext(270, 5),
            OnNext(330, 6),
            OnNext(340, 7),
            OnError<int>(390, ex)
        );

        var c = default(ConnectableSeq<int>);
        var o = Scheduler.CreateObserver<int>();
        var d1 = default(IAsyncDisposable);
        var d2 = default(IAsyncDisposable);

        Scheduler.ScheduleAbsolute(50, () => c = xs.Multicast(s));
        Scheduler.ScheduleAbsolute(100, async () => d2 = await c!.ConnectAsync(Scheduler));
        Scheduler.ScheduleAbsolute(400, async () => d1 = await c!.SubscribeAsync(o));

        Scheduler.Start();

        o.Messages.AssertEqual(
            OnError<int>(400, ex)
        );

        xs.Subscriptions.AssertEqual(
            Subscribe(100, 390)
        );
    }

    [TestMethod]
    public void Multicast_Hot_6()
    {
        var s = CreateSubject<int>();

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

        var c = default(ConnectableSeq<int>);
        var o = Scheduler.CreateObserver<int>();
        var d1 = default(IAsyncDisposable);
        var d2 = default(IAsyncDisposable);

        Scheduler.ScheduleAbsolute(50, () => c = xs.Multicast(s));
        Scheduler.ScheduleAbsolute(100, async () => d2 = await c!.ConnectAsync(Scheduler));
        Scheduler.ScheduleAbsolute(400, async () => d1 = await c!.SubscribeAsync(o));

        Scheduler.Start();

        o.Messages.AssertEqual(
            OnCompleted<int>(400)
        );

        xs.Subscriptions.AssertEqual(
            Subscribe(100, 390)
        );
    }

    [TestMethod]
    public void Multicast_Cold_Completed()
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
            xs.Multicast(() => CreateSubject<int>(), ys => ys)
        );

        res.Messages.AssertEqual(
            OnNext(210, 3),
            OnNext(240, 4),
            OnNext(270, 5),
            OnNext(330, 6),
            OnNext(340, 7),
            OnCompleted<int>(390)
        );

        xs.Subscriptions.AssertEqual(
            Subscribe(200, 390)
        );
    }

    [TestMethod]
    public void Multicast_Cold_Error()
    {
        var ex = new Exception();

        var xs = Scheduler.CreateHotObservable(
            OnNext(40, 0),
            OnNext(90, 1),
            OnNext(150, 2),
            OnNext(210, 3),
            OnNext(240, 4),
            OnNext(270, 5),
            OnNext(330, 6),
            OnNext(340, 7),
            OnError<int>(390, ex)
        );

        var res = Scheduler.Start(() =>
            xs.Multicast(() => CreateSubject<int>(), ys => ys)
        );

        res.Messages.AssertEqual(
            OnNext(210, 3),
            OnNext(240, 4),
            OnNext(270, 5),
            OnNext(330, 6),
            OnNext(340, 7),
            OnError<int>(390, ex)
        );

        xs.Subscriptions.AssertEqual(
            Subscribe(200, 390)
        );
    }

    [TestMethod]
    public void Multicast_Cold_Dispose()
    {
        var xs = Scheduler.CreateHotObservable(
            OnNext(40, 0),
            OnNext(90, 1),
            OnNext(150, 2),
            OnNext(210, 3),
            OnNext(240, 4),
            OnNext(270, 5),
            OnNext(330, 6),
            OnNext(340, 7)
        );

        var res = Scheduler.Start(() =>
            xs.Multicast(() => CreateSubject<int>(), ys => ys)
        );

        res.Messages.AssertEqual(
            OnNext(210, 3),
            OnNext(240, 4),
            OnNext(270, 5),
            OnNext(330, 6),
            OnNext(340, 7)
        );

        xs.Subscriptions.AssertEqual(
            Subscribe(200, 1000)
        );
    }

    [TestMethod]
    public void Multicast_Cold_Zip()
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
            xs.Multicast(() => CreateSubject<int>(), ys => ys.Zip(ys, (a, b) => a + b))
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
    public void Multicast_SubjectSelectorThrows()
    {
        var ex = new Exception();

        var xs = Scheduler.CreateHotObservable(
            OnNext(210, 1),
            OnNext(240, 2),
            OnCompleted<int>(300)
        );

        var res = Scheduler.Start(() =>
            xs.Multicast<int, int>(() => throw ex, _ => _)
        );

        res.Messages.AssertEqual(
            OnError<int>(200, ex)
        );

        xs.Subscriptions.AssertEqual(
        );
    }

    [TestMethod]
    public void Multicast_SelectorThrows()
    {
        var ex = new Exception();

        var xs = Scheduler.CreateHotObservable(
            OnNext(210, 1),
            OnNext(240, 2),
            OnCompleted<int>(300)
        );

        var res = Scheduler.Start(() =>
            xs.Multicast<int, int>(() => CreateSubject<int>(), _ => throw ex)
        );

        res.Messages.AssertEqual(
            OnError<int>(200, ex)
        );

        xs.Subscriptions.AssertEqual(
        );
    }
}

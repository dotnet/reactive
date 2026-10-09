// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information. 

namespace Tests.System.Reactive.Shared.Scenarios;

/// <summary>
/// Shared connectable-observable scenarios, from Rx.NET's <c>ConnectableObservableTest.cs</c>.
/// </summary>
/// <remarks>
/// Every behavioural test from that file: 6 of its 7 (the <c>*_ArgumentChecking</c> test, a
/// null-observer probe on a published sequence, is the code-generated stratum). The original
/// tests the internal <c>ConnectableObservable&lt;int&gt;(source, subject)</c> type directly,
/// which the shared suite cannot reach in either library; <c>source.Multicast(subject)</c> is
/// that constructor's public spelling, and is what the last test of the file uses itself, so
/// every scenario here is written that way. Four of them drive a <see cref="MySubject"/>, and
/// <c>ConnectableObservable_DisconnectFuture</c> is the one that uses its <c>DisposeOn</c>.
/// </remarks>
public abstract class ConnectableObservableTests : SharedReactiveTest
{
    [TestMethod]
    public async Task ConnectableObservable_Creation()
    {
        var y = 0;

        var s2 = CreateSubject<int>();
        var co2 = Seq.Return(1).Multicast(s2);

        await co2.SubscribeAsync(Scheduler, x => y = x);
        Assert.AreNotEqual(1, y);

        await co2.ConnectAsync(Scheduler);
        Assert.AreEqual(1, y);
    }

    [TestMethod]
    public async Task ConnectableObservable_Connected()
    {
        var xs = Scheduler.CreateHotObservable(
            OnNext(210, 1),
            OnNext(220, 2),
            OnNext(230, 3),
            OnNext(240, 4),
            OnCompleted<int>(250)
        );

        var subject = CreateMySubject();

        var conn = xs.Multicast(subject);
        var disconnect = await conn.ConnectAsync(Scheduler);

        var res = Scheduler.Start(() => conn);

        res.Messages.AssertEqual(
            OnNext(210, 1),
            OnNext(220, 2),
            OnNext(230, 3),
            OnNext(240, 4),
            OnCompleted<int>(250)
        );
    }

    [TestMethod]
    public void ConnectableObservable_NotConnected()
    {
        var xs = Scheduler.CreateHotObservable(
            OnNext(210, 1),
            OnNext(220, 2),
            OnNext(230, 3),
            OnNext(240, 4),
            OnCompleted<int>(250)
        );

        var subject = CreateMySubject();

        var conn = xs.Multicast(subject);

        var res = Scheduler.Start(() => conn);

        res.Messages.AssertEqual(
        );
    }

    [TestMethod]
    public async Task ConnectableObservable_Disconnected()
    {
        var xs = Scheduler.CreateHotObservable(
            OnNext(210, 1),
            OnNext(220, 2),
            OnNext(230, 3),
            OnNext(240, 4),
            OnCompleted<int>(250)
        );

        var subject = CreateMySubject();

        var conn = xs.Multicast(subject);
        var disconnect = await conn.ConnectAsync(Scheduler);
        await disconnect.DisposeAsync();

        var res = Scheduler.Start(() => conn);

        res.Messages.AssertEqual(
        );
    }

    [TestMethod]
    public async Task ConnectableObservable_DisconnectFuture()
    {
        var xs = Scheduler.CreateHotObservable(
            OnNext(210, 1),
            OnNext(220, 2),
            OnNext(230, 3),
            OnNext(240, 4),
            OnCompleted<int>(250)
        );

        var subject = CreateMySubject();

        var conn = xs.Multicast(subject);
        subject.DisposeOn(3, await conn.ConnectAsync(Scheduler));

        var res = Scheduler.Start(() => conn);

        res.Messages.AssertEqual(
            OnNext(210, 1),
            OnNext(220, 2),
            OnNext(230, 3)
        );
    }

    [TestMethod]
    public void ConnectableObservable_MultipleNonOverlappedConnections()
    {
        var xs = Scheduler.CreateHotObservable(
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

        var subject = CreateSubject<int>();

        var conn = xs.Multicast(subject);

        var c1 = default(IAsyncDisposable);
        Scheduler.ScheduleAbsolute(225, async () => { c1 = await conn.ConnectAsync(Scheduler); });
        Scheduler.ScheduleAbsolute(241, async () => { await c1!.DisposeAsync(); });
        // idempotency tests
        Scheduler.ScheduleAbsolute(245, async () => { await c1!.DisposeAsync(); });
        Scheduler.ScheduleAbsolute(251, async () => { await c1!.DisposeAsync(); });
        Scheduler.ScheduleAbsolute(260, async () => { await c1!.DisposeAsync(); });

        var c2 = default(IAsyncDisposable);
        Scheduler.ScheduleAbsolute(249, async () => { c2 = await conn.ConnectAsync(Scheduler); });
        Scheduler.ScheduleAbsolute(255, async () => { await c2!.DisposeAsync(); });
        // idempotency tests
        Scheduler.ScheduleAbsolute(265, async () => { await c2!.DisposeAsync(); });
        Scheduler.ScheduleAbsolute(280, async () => { await c2!.DisposeAsync(); });

        var c3 = default(IAsyncDisposable);
        Scheduler.ScheduleAbsolute(275, async () => { c3 = await conn.ConnectAsync(Scheduler); });
        Scheduler.ScheduleAbsolute(295, async () => { await c3!.DisposeAsync(); });

        var res = Scheduler.Start(() => conn);

        res.Messages.AssertEqual(
            OnNext(230, 3),
            OnNext(240, 4),
            OnNext(250, 5),
            OnNext(280, 8),
            OnNext(290, 9)
        );

        xs.Subscriptions.AssertEqual(
            Subscribe(225, 241),
            Subscribe(249, 255),
            Subscribe(275, 295)
        );
    }
}

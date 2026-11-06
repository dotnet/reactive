// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information. 

using System.Collections;
using System.Reactive.Concurrency;
using System.Reactive.Linq;

using Microsoft.Reactive.Testing;

namespace Tests.System.Reactive.Shared.Rx.Tests;

/// <summary>Rx.NET's own <c>Latest</c> tests, from its <c>LatestTest.cs</c>.</summary>
/// <remarks>
/// <c>Latest</c> is a push-to-pull adapter: each <c>MoveNext</c> blocks until an element has
/// arrived since the last one and yields the latest. AsyncRx.NET has no pull side, so these run
/// here against Rx.NET only, under the Step 46 decision for the blocking operators. 3 of the
/// file's 4 tests (the argument-checking test is the code-generated stratum), as the originals but
/// for MSTest's assertions and <c>null!</c> for the enumerator locals the scheduled actions fill
/// in; <c>Latest1</c>'s commented-out assertion is left out. <c>Latest2</c> drives Rx.NET's own
/// <c>TestScheduler</c>, so the class derives from Rx.NET's <c>ReactiveTest</c>; the other two
/// produce from a task and block on events, as the originals do.
/// </remarks>
[TestClass]
public sealed class RxLatestTests : ReactiveTest
{
    [TestMethod]
    public void Latest1()
    {
        var disposed = false;
        var evt = new AutoResetEvent(false);
        var src = Observable.Create<int>(obs =>
        {
            Task.Run(() =>
            {
                evt.WaitOne();
                obs.OnNext(1);
                evt.WaitOne();
                obs.OnNext(2);
                evt.WaitOne();
                obs.OnCompleted();
            });

            return () => { disposed = true; };
        });

        var res = src.Latest().GetEnumerator();

        Task.Run(async () =>
        {
            await Task.Delay(250, TestContext.CancellationToken);
            evt.Set();
        });

        Assert.IsTrue(res.MoveNext());
        Assert.AreEqual(1, res.Current);

        evt.Set();
        Assert.IsTrue(res.MoveNext());
        Assert.AreEqual(2, ((IEnumerator)res).Current);

        evt.Set();
        Assert.IsFalse(res.MoveNext());

        Assert.ThrowsExactly<NotSupportedException>(() => res.Reset());

        res.Dispose();
        //Assert.ThrowsExactly<ObjectDisposedException>(() => res.MoveNext());
        Assert.IsTrue(disposed);
    }

    [TestMethod]
    public void Latest2()
    {
        var scheduler = new TestScheduler();

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

        var res = xs.Latest();

        IEnumerator<int> e1 = null!;
        scheduler.ScheduleAbsolute(205, () =>
        {
            e1 = res.GetEnumerator();
        });

        var o1 = new List<int>();
        scheduler.ScheduleAbsolute(235, () =>
        {
            Assert.IsTrue(e1.MoveNext());
            o1.Add(e1.Current);
        });
        scheduler.ScheduleAbsolute(265, () =>
        {
            Assert.IsTrue(e1.MoveNext());
            o1.Add(e1.Current);
        });

        scheduler.ScheduleAbsolute(285, () => e1.Dispose());

        IEnumerator e2 = null!;
        scheduler.ScheduleAbsolute(255, () =>
        {
            e2 = ((IEnumerable)res).GetEnumerator();
        });

        var o2 = new List<int>();
        scheduler.ScheduleAbsolute(265, () =>
        {
            Assert.IsTrue(e2.MoveNext());
            o2.Add((int)e2.Current);
        });
        scheduler.ScheduleAbsolute(275, () =>
        {
            Assert.IsTrue(e2.MoveNext());
            o2.Add((int)e2.Current);
        });

        scheduler.Start();

        xs.Subscriptions.AssertEqual(
            Subscribe(205, 285),
            Subscribe(255, 300)
        );

        o1.AssertEqual(3, 6);
        o2.AssertEqual(6, 7);
    }

    [TestMethod]
    public void Latest_Error()
    {
        SynchronizationContext.SetSynchronizationContext(null);

        var ex = new Exception();

        var evt = new AutoResetEvent(false);
        var src = Observable.Create<int>(obs =>
        {
            Task.Run(() =>
            {
                evt.WaitOne();
                obs.OnNext(1);
                evt.WaitOne();
                obs.OnError(ex);
            });

            return () => { };
        });

        var res = src.Latest().GetEnumerator();

        Task.Run(async () =>
        {
            await Task.Delay(250, TestContext.CancellationToken);
            evt.Set();
        });

        Assert.IsTrue(res.MoveNext());
        Assert.AreEqual(1, res.Current);

        evt.Set();

        Assert.AreSame(ex, Assert.ThrowsExactly<Exception>(() => res.MoveNext()));
    }

    public TestContext TestContext { get; set; } = null!;
}

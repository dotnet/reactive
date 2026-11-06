// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information. 

using System.Collections;
using System.Reactive.Concurrency;
using System.Reactive.Linq;

using Microsoft.Reactive.Testing;

namespace Tests.System.Reactive.Shared.Rx.Tests;

/// <summary>Rx.NET's own <c>MostRecent</c> tests, from its <c>MostRecentTest.cs</c>.</summary>
/// <remarks>
/// <c>MostRecent</c> is a push-to-pull adapter that never blocks: each <c>MoveNext</c> yields the
/// most recent element, or the initial value before any, as often as it is asked. AsyncRx.NET has
/// no pull side, so these run here against Rx.NET only, under the Step 46 decision for the
/// blocking operators. 3 of the file's 4 tests (the argument-checking test is the code-generated
/// stratum), as the originals but for MSTest's assertions and <c>null!</c> for the enumerator
/// locals the scheduled actions fill in; <c>MostRecent2</c> drives Rx.NET's own
/// <c>TestScheduler</c>, so the class derives from Rx.NET's <c>ReactiveTest</c>.
/// </remarks>
[TestClass]
public sealed class RxMostRecentTests : ReactiveTest
{
    [TestMethod]
    public void MostRecent1()
    {
        var evt = new AutoResetEvent(false);
        var nxt = new AutoResetEvent(false);
        var src = Observable.Create<int>(obs =>
        {
            Task.Run(() =>
            {
                evt.WaitOne();
                obs.OnNext(1);
                nxt.Set();
                evt.WaitOne();
                obs.OnNext(2);
                nxt.Set();
                evt.WaitOne();
                obs.OnCompleted();
                nxt.Set();
            });

            return () => { };
        });

        var res = src.MostRecent(42).GetEnumerator();

        Assert.IsTrue(res.MoveNext());
        Assert.AreEqual(42, res.Current);
        Assert.IsTrue(res.MoveNext());
        Assert.AreEqual(42, res.Current);

        for (var i = 1; i <= 2; i++)
        {
            evt.Set();
            nxt.WaitOne();
            Assert.IsTrue(res.MoveNext());
            Assert.AreEqual(i, res.Current);
            Assert.IsTrue(res.MoveNext());
            Assert.AreEqual(i, res.Current);
        }

        evt.Set();
        nxt.WaitOne();
        Assert.IsFalse(res.MoveNext());
    }

    [TestMethod]
    public void MostRecent2()
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

        var res = xs.MostRecent(0);

        IEnumerator<int> e1 = null!;
        scheduler.ScheduleAbsolute(200, () =>
        {
            e1 = res.GetEnumerator();
        });

        var o1 = new List<int>();
        scheduler.ScheduleAbsolute(205, () =>
        {
            Assert.IsTrue(e1.MoveNext());
            o1.Add(e1.Current);
        });
        scheduler.ScheduleAbsolute(232, () =>
        {
            Assert.IsTrue(e1.MoveNext());
            o1.Add(e1.Current);
        });
        scheduler.ScheduleAbsolute(234, () =>
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
        scheduler.ScheduleAbsolute(258, () =>
        {
            Assert.IsTrue(e2.MoveNext());
            o2.Add((int)e2.Current);
        });
        scheduler.ScheduleAbsolute(262, () =>
        {
            Assert.IsTrue(e2.MoveNext());
            o2.Add((int)e2.Current);
        });
        scheduler.ScheduleAbsolute(264, () =>
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
            Subscribe(200, 285),
            Subscribe(255, 300)
        );

        o1.AssertEqual(0, 3, 3, 6);
        o2.AssertEqual(0, 6, 6, 7);
    }

    [TestMethod]
    public void MostRecent_Error()
    {
        var ex = new Exception();

        var evt = new AutoResetEvent(false);
        var nxt = new AutoResetEvent(false);
        var src = Observable.Create<int>(obs =>
        {
            Task.Run(() =>
            {
                evt.WaitOne();
                obs.OnNext(1);
                nxt.Set();
                evt.WaitOne();
                obs.OnError(ex);
                nxt.Set();
            });

            return () => { };
        });

        var res = src.MostRecent(42).GetEnumerator();

        Assert.IsTrue(res.MoveNext());
        Assert.AreEqual(42, res.Current);
        Assert.IsTrue(res.MoveNext());
        Assert.AreEqual(42, res.Current);

        evt.Set();
        nxt.WaitOne();
        Assert.IsTrue(res.MoveNext());
        Assert.AreEqual(1, res.Current);
        Assert.IsTrue(res.MoveNext());
        Assert.AreEqual(1, res.Current);

        evt.Set();
        nxt.WaitOne();

        Assert.AreSame(ex, Assert.ThrowsExactly<Exception>(() => res.MoveNext()));
    }
}

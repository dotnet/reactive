// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information. 

using System.Reactive.Concurrency;
using System.Reactive.Linq;

using Microsoft.Reactive.Testing;

namespace Tests.System.Reactive.Shared.Rx.Tests;

/// <summary>Rx.NET's own <c>Collect</c> tests, from its <c>CollectTest.cs</c>.</summary>
/// <remarks>
/// <c>Collect</c> is <c>Chunkify</c> with a collector: each <c>MoveNext</c> yields what the merge
/// function folded the elements since the last one into, starting each chunk from a fresh
/// collector. AsyncRx.NET has no pull side, so these run here against Rx.NET only, under the Step
/// 46 decision for the blocking operators; they belong with the Step 85 group and were missed
/// there. 6 of the file's 7 tests (the argument-checking test is the code-generated stratum), as
/// the originals but for MSTest's assertions and <c>null!</c> for the enumerator local the
/// scheduled actions fill in: the two regular shapes, and the initial, the second and the new
/// collector and the merge throwing, each ending the enumeration with that exception.
/// </remarks>
[TestClass]
public sealed class RxCollectTests : ReactiveTest
{
    [TestMethod]
    public void Collect_Regular1()
    {
        var scheduler = new TestScheduler();

        var xs = scheduler.CreateHotObservable(
            OnNext(100, 1),
            OnNext(200, 2),
            OnNext(300, 3),
            OnNext(400, 4),
            OnNext(500, 5),
            OnNext(600, 6),
            OnNext(700, 7),
            OnNext(800, 8),
            OnCompleted<int>(900)
        );

        var ys = xs.Collect(() => 0, (x, y) => x + y);
        IEnumerator<int> e = null!;

        var res = new List<int>();

        var log = new Action(() =>
        {
            Assert.IsTrue(e.MoveNext());
            res.Add(e.Current);
        });

        scheduler.ScheduleAbsolute(250, () => { e = ys.GetEnumerator(); });
        scheduler.ScheduleAbsolute(270, log);
        scheduler.ScheduleAbsolute(310, log);
        scheduler.ScheduleAbsolute(450, log);
        scheduler.ScheduleAbsolute(470, log);
        scheduler.ScheduleAbsolute(750, log);
        scheduler.ScheduleAbsolute(850, log);
        scheduler.ScheduleAbsolute(950, log);
        scheduler.ScheduleAbsolute(980, () => Assert.IsFalse(e.MoveNext()));

        scheduler.Start();

        xs.Subscriptions.AssertEqual(
            Subscribe(250, 900)
        );

        Assert.HasCount(7, res);
        Assert.AreEqual(res[0], new int[] { }.Sum());
        Assert.AreEqual(res[1], new int[] { 3 }.Sum());
        Assert.AreEqual(res[2], new int[] { 4 }.Sum());
        Assert.AreEqual(res[3], new int[] { }.Sum());
        Assert.AreEqual(res[4], new int[] { 5, 6, 7 }.Sum());
        Assert.AreEqual(res[5], new int[] { 8 }.Sum());
        Assert.AreEqual(res[6], new int[] { }.Sum());
    }

    [TestMethod]
    public void Collect_Regular2()
    {
        var scheduler = new TestScheduler();

        var xs = scheduler.CreateHotObservable(
            OnNext(100, 1),
            OnNext(200, 2),
            OnNext(300, 3),
            OnNext(400, 4),
            OnNext(500, 5),
            OnNext(600, 6),
            OnNext(700, 7),
            OnNext(800, 8),
            OnCompleted<int>(900)
        );

        var ys = xs.Collect(() => 0, (x, y) => x + y);
        IEnumerator<int> e = null!;

        var res = new List<int>();

        var log = new Action(() =>
        {
            Assert.IsTrue(e.MoveNext());
            res.Add(e.Current);
        });

        scheduler.ScheduleAbsolute(250, () => { e = ys.GetEnumerator(); });
        scheduler.ScheduleAbsolute(550, log);
        scheduler.ScheduleAbsolute(950, log);
        scheduler.ScheduleAbsolute(980, () => Assert.IsFalse(e.MoveNext()));

        scheduler.Start();

        xs.Subscriptions.AssertEqual(
            Subscribe(250, 900)
        );

        Assert.HasCount(2, res);
        Assert.AreEqual(res[0], new int[] { 3, 4, 5 }.Sum());
        Assert.AreEqual(res[1], new int[] { 6, 7, 8 }.Sum());
    }

    [TestMethod]
    public void Collect_InitialCollectorThrows()
    {
        var scheduler = new TestScheduler();

        var xs = scheduler.CreateHotObservable(
            OnNext(100, 1),
            OnNext(200, 2),
            OnNext(300, 3),
            OnNext(400, 4),
            OnCompleted<int>(500)
        );

        var ex = new Exception();
        var ys = xs.Collect<int, int>(() => { throw ex; }, (x, y) => x + y);

        var ex_ = default(Exception);

        scheduler.ScheduleAbsolute(250, () =>
        {
            try
            {
                ys.GetEnumerator();
            }
            catch (Exception err)
            {
                ex_ = err;
            }
        });

        scheduler.Start();

        xs.Subscriptions.AssertEqual(
        );

        Assert.AreSame(ex_, ex);
    }

    [TestMethod]
    public void Collect_SecondCollectorThrows()
    {
        var scheduler = new TestScheduler();

        var xs = scheduler.CreateHotObservable(
            OnNext(100, 1),
            OnNext(200, 2),
            OnNext(300, 3),
            OnNext(400, 4),
            OnCompleted<int>(500)
        );

        var ex = new Exception();
        var n = 0;
        var ys = xs.Collect(() =>
        {
            if (n++ == 0)
            {
                return 0;
            }
            else
            {
                throw ex;
            }
        }, (x, y) => x + y);
        IEnumerator<int> e = null!;

        var ex_ = default(Exception);

        scheduler.ScheduleAbsolute(250, () => e = ys.GetEnumerator());
        scheduler.ScheduleAbsolute(350, () =>
        {
            try
            {
                e.MoveNext();
            }
            catch (Exception err)
            {
                ex_ = err;
            }
        });

        scheduler.Start();

        xs.Subscriptions.AssertEqual(
            Subscribe(250, 350)
        );

        Assert.AreSame(ex_, ex);
    }

    [TestMethod]
    public void Collect_NewCollectorThrows()
    {
        var scheduler = new TestScheduler();

        var xs = scheduler.CreateHotObservable(
            OnNext(100, 1),
            OnNext(200, 2),
            OnNext(300, 3),
            OnNext(400, 4),
            OnCompleted<int>(500)
        );

        var ex = new Exception();
        var ys = xs.Collect(() => 0, (x, y) => x + y, x => { throw ex; });
        IEnumerator<int> e = null!;

        var ex_ = default(Exception);

        scheduler.ScheduleAbsolute(250, () => e = ys.GetEnumerator());
        scheduler.ScheduleAbsolute(350, () =>
        {
            try
            {
                e.MoveNext();
            }
            catch (Exception err)
            {
                ex_ = err;
            }
        });

        scheduler.Start();

        xs.Subscriptions.AssertEqual(
            Subscribe(250, 350)
        );

        Assert.AreSame(ex_, ex);
    }

    [TestMethod]
    public void Collect_MergeThrows()
    {
        var scheduler = new TestScheduler();

        var xs = scheduler.CreateHotObservable(
            OnNext(100, 1),
            OnNext(200, 2),
            OnNext(300, 3),
            OnNext(400, 4),
            OnCompleted<int>(500)
        );

        var ex = new Exception();
        var ys = xs.Collect(() => 0, (x, y) => { throw ex; });
        IEnumerator<int> e = null!;

        var ex_ = default(Exception);

        scheduler.ScheduleAbsolute(250, () => { e = ys.GetEnumerator(); });
        scheduler.ScheduleAbsolute(350, () =>
        {
            try
            {
                e.MoveNext();
            }
            catch (Exception err)
            {
                ex_ = err;
            }
        });

        scheduler.Start();

        xs.Subscriptions.AssertEqual(
            Subscribe(250, 300)
        );

        Assert.AreSame(ex_, ex);
    }
}

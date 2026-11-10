// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information. 

using System.Reactive.Concurrency;
using System.Reactive.Linq;

using Microsoft.Reactive.Testing;

namespace Tests.System.Reactive.Shared.Rx.Tests;

/// <summary>Rx.NET's own <c>Chunkify</c> tests, from its <c>ChunkifyTest.cs</c>.</summary>
/// <remarks>
/// <c>Chunkify</c> is a push-to-pull adapter that never blocks: each <c>MoveNext</c> yields the
/// elements that arrived since the last one as a list, empty if none did. AsyncRx.NET has no pull
/// side, so these run here against Rx.NET only, under the Step 46 decision for the blocking
/// operators; they belong with the Step 85 group and were missed there. 3 of the file's 4 tests
/// (the argument-checking test is the code-generated stratum), as the originals but for MSTest's
/// assertions and <c>null!</c> for the enumerator local the scheduled actions fill in.
/// </remarks>
[TestClass]
public sealed class RxChunkifyTests : ReactiveTest
{
    [TestMethod]
    public void Chunkify_Regular1()
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

        var ys = xs.Chunkify();
        IEnumerator<IList<int>> e = null!;

        var res = new List<IList<int>>();

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
        Assert.IsTrue(res[0].SequenceEqual([]));
        Assert.IsTrue(res[1].SequenceEqual([3]));
        Assert.IsTrue(res[2].SequenceEqual([4]));
        Assert.IsTrue(res[3].SequenceEqual([]));
        Assert.IsTrue(res[4].SequenceEqual([5, 6, 7]));
        Assert.IsTrue(res[5].SequenceEqual([8]));
        Assert.IsTrue(res[6].SequenceEqual([]));
    }

    [TestMethod]
    public void Chunkify_Regular2()
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

        var ys = xs.Chunkify();
        IEnumerator<IList<int>> e = null!;

        var res = new List<IList<int>>();

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
        Assert.IsTrue(res[0].SequenceEqual([3, 4, 5]));
        Assert.IsTrue(res[1].SequenceEqual([6, 7, 8]));
    }

    [TestMethod]
    public void Chunkify_Error()
    {
        var ex = new Exception();

        var scheduler = new TestScheduler();

        var xs = scheduler.CreateHotObservable(
            OnNext(100, 1),
            OnNext(200, 2),
            OnNext(300, 3),
            OnNext(400, 4),
            OnNext(500, 5),
            OnNext(600, 6),
            OnError<int>(700, ex)
        );

        var ys = xs.Chunkify();
        IEnumerator<IList<int>> e = null!;

        var res = new List<IList<int>>();

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
        scheduler.ScheduleAbsolute(750, () =>
        {
            try
            {
                e.MoveNext();
                Assert.Fail();
            }
            catch (Exception error)
            {
                Assert.AreSame(ex, error);
            }
        });

        scheduler.Start();

        xs.Subscriptions.AssertEqual(
            Subscribe(250, 700)
        );

        Assert.HasCount(4, res);
        Assert.IsTrue(res[0].SequenceEqual([]));
        Assert.IsTrue(res[1].SequenceEqual([3]));
        Assert.IsTrue(res[2].SequenceEqual([4]));
        Assert.IsTrue(res[3].SequenceEqual([]));
    }
}

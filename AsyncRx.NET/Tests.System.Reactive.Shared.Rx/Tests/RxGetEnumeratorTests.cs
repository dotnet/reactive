// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information. 

using System.Reactive.Disposables;
using System.Reactive.Linq;

using Microsoft.Reactive.Testing;

namespace Tests.System.Reactive.Shared.Rx.Tests;

/// <summary>
/// Rx.NET's own <c>GetEnumerator</c> tests, from its <c>GetEnumeratorTest.cs</c>.
/// </summary>
/// <remarks>
/// <c>GetEnumerator</c> is the first of the push-to-pull adapters: an enumerator whose
/// <c>MoveNext</c> blocks until the next element has arrived. AsyncRx.NET has no pull side at all
/// (its consumer awaits, and an <c>IAsyncEnumerable</c> bridge is a separate question), so these
/// run here against Rx.NET only, under the Step 46 decision for the blocking operators. 3 of the
/// file's 4 tests (the argument-checking test is the code-generated stratum), as the originals but
/// for MSTest's assertions and <c>null!</c> for the enumerator locals the scheduled actions fill
/// in; they drive Rx.NET's own <c>TestScheduler</c>, so the class derives from Rx.NET's
/// <c>ReactiveTest</c> for the tick helpers.
/// </remarks>
[TestClass]
public sealed class RxGetEnumeratorTests : ReactiveTest
{
    [TestMethod]
    public void GetEnumerator_Regular1()
    {
        var scheduler = new TestScheduler();

        var xs = scheduler.CreateColdObservable(
            OnNext(10, 2),
            OnNext(20, 3),
            OnNext(30, 5),
            OnNext(40, 7),
            OnCompleted<int>(50)
        );

        IEnumerator<int> res = null!;

        scheduler.ScheduleAbsolute(default(object), 100, (self, _) => { res = xs.GetEnumerator(); return Disposable.Empty; });

        var hasNext = new List<bool>();
        var vals = new List<Tuple<long, int>>();
        for (long i = 200; i <= 250; i += 10)
        {
            var t = i;
            scheduler.ScheduleAbsolute(default(object), t, (self, _) =>
            {
                var b = res.MoveNext();
                hasNext.Add(b);
                if (b)
                {
                    vals.Add(new Tuple<long, int>(scheduler.Clock, res.Current));
                }

                return Disposable.Empty;
            });
        }

        scheduler.Start();

        xs.Subscriptions.AssertEqual(
            Subscribe(100, 150)
        );

        Assert.HasCount(6, hasNext);
        Assert.IsTrue(hasNext.Take(4).All(_ => _));
        Assert.IsTrue(hasNext.Skip(4).All(_ => !_));

        Assert.HasCount(4, vals);
        Assert.IsTrue(vals[0].Item1 == 200 && vals[0].Item2 == 2);
        Assert.IsTrue(vals[1].Item1 == 210 && vals[1].Item2 == 3);
        Assert.IsTrue(vals[2].Item1 == 220 && vals[2].Item2 == 5);
        Assert.IsTrue(vals[3].Item1 == 230 && vals[3].Item2 == 7);
    }

    [TestMethod]
    public void GetEnumerator_Regular2()
    {
        var scheduler = new TestScheduler();

        var xs = scheduler.CreateColdObservable(
            OnNext(10, 2),
            OnNext(30, 3),
            OnNext(50, 5),
            OnNext(70, 7),
            OnCompleted<int>(90)
        );

        IEnumerator<int> res = null!;

        scheduler.ScheduleAbsolute(default(object), 100, (self, _) => { res = xs.GetEnumerator(); return Disposable.Empty; });

        var hasNext = new List<bool>();
        var vals = new List<Tuple<long, int>>();
        for (long i = 120; i <= 220; i += 20)
        {
            var t = i;
            scheduler.ScheduleAbsolute(default(object), t, (self, _) =>
            {
                var b = res.MoveNext();
                hasNext.Add(b);
                if (b)
                {
                    vals.Add(new Tuple<long, int>(scheduler.Clock, res.Current));
                }

                return Disposable.Empty;
            });
        }

        scheduler.Start();

        xs.Subscriptions.AssertEqual(
            Subscribe(100, 190)
        );

        Assert.HasCount(6, hasNext);
        Assert.IsTrue(hasNext.Take(4).All(_ => _));
        Assert.IsTrue(hasNext.Skip(4).All(_ => !_));

        Assert.HasCount(4, vals);
        Assert.IsTrue(vals[0].Item1 == 120 && vals[0].Item2 == 2);
        Assert.IsTrue(vals[1].Item1 == 140 && vals[1].Item2 == 3);
        Assert.IsTrue(vals[2].Item1 == 160 && vals[2].Item2 == 5);
        Assert.IsTrue(vals[3].Item1 == 180 && vals[3].Item2 == 7);
    }

    [TestMethod]
    public void GetEnumerator_Dispose()
    {
        var scheduler = new TestScheduler();

        var xs = scheduler.CreateColdObservable(
            OnNext(10, 2),
            OnNext(30, 3),
            OnNext(50, 5),
            OnNext(70, 7),
            OnCompleted<int>(90)
        );

        IEnumerator<int> res = null!;

        scheduler.ScheduleAbsolute(default(object), 100, (self, _) => { res = xs.GetEnumerator(); return Disposable.Empty; });

        scheduler.ScheduleAbsolute(default(object), 140, (self, _) =>
        {
            Assert.IsTrue(res.MoveNext());
            Assert.AreEqual(2, res.Current);

            Assert.IsTrue(res.MoveNext());
            Assert.AreEqual(3, res.Current);

            res.Dispose();

            return Disposable.Empty;
        });

        scheduler.ScheduleAbsolute(default(object), 160, (self, _) =>
        {
            Assert.ThrowsExactly<ObjectDisposedException>(() => res.MoveNext());
            return Disposable.Empty;
        });

        scheduler.Start();

        xs.Subscriptions.AssertEqual(
            Subscribe(100, 140)
        );
    }
}

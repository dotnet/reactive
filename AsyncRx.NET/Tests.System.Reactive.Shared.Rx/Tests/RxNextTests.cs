// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information. 

using System.Collections;
using System.Reactive.Concurrency;
using System.Reactive.Linq;

using Microsoft.Reactive.Testing;

namespace Tests.System.Reactive.Shared.Rx.Tests;

/// <summary>Rx.NET's own <c>Next</c> tests, from its <c>NextTest.cs</c>.</summary>
/// <remarks>
/// <c>Next</c> is a push-to-pull adapter: each <c>MoveNext</c> blocks until the next element
/// arrives after it was called, so elements arriving while nobody is waiting are lost. AsyncRx.NET
/// has no pull side, so these run here against Rx.NET only, under the Step 46 decision for the
/// blocking operators. 5 of the file's 6 tests (the argument-checking test is the code-generated
/// stratum), as the originals but for MSTest's assertions and <c>null!</c> for the enumerator
/// locals the scheduled actions fill in; <c>Next2</c> drives Rx.NET's own <c>TestScheduler</c>, so
/// the class derives from Rx.NET's <c>ReactiveTest</c>; the others produce from a task or a thread
/// and block, as the originals do.
/// </remarks>
[TestClass]
public sealed class RxNextTests : ReactiveTest
{
    [TestMethod]
    public void Next1()
    {
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

            return () => { };
        });

        var res = src.Next().GetEnumerator();

        void release() => Task.Run(async () =>
        {
            await Task.Delay(250, TestContext.CancellationToken);
            evt.Set();
        });

        release();
        Assert.IsTrue(res.MoveNext());
        Assert.AreEqual(1, res.Current);

        release();
        Assert.IsTrue(res.MoveNext());
        Assert.AreEqual(2, res.Current);

        release();
        Assert.IsFalse(res.MoveNext());
    }

    [TestMethod]
    public void Next2()
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

        var res = xs.Next();

        IEnumerator<int> e1 = null!;
        scheduler.ScheduleAbsolute(200, () =>
        {
            e1 = res.GetEnumerator();
        });

        scheduler.ScheduleAbsolute(285, () => e1.Dispose());

        IEnumerator e2 = null!;
        scheduler.ScheduleAbsolute(255, () =>
        {
            e2 = ((IEnumerable)res).GetEnumerator();
        });

        scheduler.Start();

        xs.Subscriptions.AssertEqual(
            Subscribe(200, 285),
            Subscribe(255, 300)
        );
    }

    [TestMethod]
    public void Next_DoesNotBlock()
    {
        var evt = new ManualResetEvent(false);

        var xs = Observable.Empty<int>().Do(_ => { }, () => evt.Set());

        var e = xs.Next().GetEnumerator();

        evt.WaitOne();

        Assert.IsFalse(e.MoveNext());
    }

    [TestMethod]
    public void Next_SomeResults()
    {
        var xs = Observable.Range(0, 100, Scheduler.Default);

        var res = xs.Next().ToList();

        Assert.IsTrue(res.All(x => x < 100));
        Assert.IsTrue(res.Count == res.Distinct().Count());
    }

    [TestMethod]
    public void Next_Error()
    {
        var ex = new Exception();

        var evt = new AutoResetEvent(false);
        var src = Observable.Create<int>(obs =>
        {
            new Thread(() =>
            {
                evt.WaitOne();
                obs.OnNext(1);
                evt.WaitOne();
                obs.OnError(ex);
            }).Start();

            return () => { };
        });

        var res = src.Next().GetEnumerator();

        void release() => new Thread(() =>
        {
            Thread.Sleep(250);
            evt.Set();
        }).Start();

        release();
        Assert.IsTrue(res.MoveNext());
        Assert.AreEqual(1, res.Current);

        release();

        Assert.AreSame(ex, Assert.ThrowsExactly<Exception>(() => res.MoveNext()));
    }

    public TestContext TestContext { get; set; } = null!;
}

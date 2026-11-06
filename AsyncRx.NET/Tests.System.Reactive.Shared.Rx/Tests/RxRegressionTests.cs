// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information. 

using System.Collections.Concurrent;
using System.Reactive;
using System.Reactive.Concurrency;
using System.Reactive.Linq;
using System.Reactive.Subjects;
using Microsoft.Reactive.Testing;
using Tests.System.Reactive.Shared.Scenarios;

// Rx.NET marks its blocking operators obsolete, pointing at the async forms; these tests use
// them as the originals do under a project-wide suppression.
#pragma warning disable CS0618

namespace Tests.System.Reactive.Shared.Rx.Tests;

/// <summary>
/// The shared <c>RegressionTests</c> on Rx.NET, plus the Rx.NET-only originals.
/// </summary>
/// <remarks>
/// The seven originals that stay Rx.NET-only, and the exact-tick form of an eighth, as they stand
/// but for MSTest's assertions and the static schedulers named in full:
/// <c>Bug_ConcurrentMerge</c>, <c>Bug_1287</c> and <c>Bug_1356</c> block on <c>ForEach</c> or
/// <c>First</c>; <c>Bug_1333</c> blocks on an <c>AsyncSubject</c> from an enumerator on another
/// thread; <c>Reentrant_Subject1</c> and <c>2</c> push back into a <c>Subject.Synchronize</c> from
/// its own handler, an operator AsyncRx.NET has not got, over re-entrancy its observers refuse
/// (Step 81); <c>Bug_1130</c> asserts that <c>Start</c>'s result is not a subject, which the
/// AsyncRx.NET runner claims of its own. <c>Bug_1381_OneDeliveryPerTick</c> is the original
/// <c>Bug_1381</c> with its tick expectations, which pin Rx.NET's one scheduled delivery per
/// element: the live elements arriving during the replay queue behind it; the shared
/// <c>Bug_1381</c> claims the order alone.
/// </remarks>
[TestClass]
public sealed class RxRegressionTests : RegressionTests
{
    protected override IRxTarget Target => RxTarget.Instance;

    [TestMethod]
    public void Bug_ConcurrentMerge()
    {
        const int reps = 1000;
        var source = Enumerable.Range(0, reps).ToObservable();

        var resultQueue = new ConcurrentQueue<int>();
        var r = new Random();

        source.Select(i => Observable.Create<Unit>(o =>
        {
            resultQueue.Enqueue(i);
            Task.Factory.StartNew(
                () =>
                {
                    Thread.Sleep(r.Next(10));
                    o.OnCompleted();
                });
            return () => { };
        })).Merge(3).ForEach(_ => { });

        Assert.IsTrue(Enumerable.Range(0, reps).ToList().SequenceEqual(resultQueue.ToList()));
    }

    [TestMethod]
    public void Bug_1130()
    {
        var xs = Observable.Start(() => 5);
        Assert.IsNull(xs as ISubject<int, int>);
    }

    [TestMethod]
    public void Bug_1287()
    {
        var flag = false;
        var x = Observable.Return(1, global::System.Reactive.Concurrency.Scheduler.CurrentThread).Concat(Observable.Never<int>()).Finally(() => flag = true).First();
        Assert.AreEqual(1, x);
        Assert.IsTrue(flag);
    }

    [TestMethod]
    public void Bug_1333()
    {
        var sema = new Semaphore(0, 1);
        var d = new AsyncSubject<IDisposable>();
        var e = Bug_1333_Enumerable(d, sema).ToObservable(global::System.Reactive.Concurrency.DefaultScheduler.Instance).Subscribe();
        d.OnNext(e);
        d.OnCompleted();
        sema.WaitOne();
    }

    [TestMethod]
    public void Bug_1356()
    {
        var run = false;
        Observable.Range(0, 10).Finally(() => run = true).Take(5).ForEach(_ => { });
        Assert.IsTrue(run);
    }

    [TestMethod]
    public void Bug_1381_OneDeliveryPerTick()
    {
        var scheduler = new TestScheduler();

        var xs = scheduler.CreateHotObservable(
            OnNext(90, 1),
            OnNext(110, 2),
            OnNext(250, 3),
            OnNext(270, 4),
            OnNext(280, 5),
            OnNext(301, 6),
            OnNext(302, 7),
            OnNext(400, 8),
            OnNext(401, 9),
            OnNext(510, 10)
        );

        var results = scheduler.CreateObserver<int>();
        IConnectableObservable<int> ys = null!;
        IDisposable connection = null!;
        IDisposable subscription = null!;

        scheduler.ScheduleAbsolute(100, () => ys = xs.Multicast(new ReplaySubject<int>(scheduler)));
        scheduler.ScheduleAbsolute(200, () => connection = ys.Connect());
        scheduler.ScheduleAbsolute(300, () => subscription = ys.Subscribe(results));
        scheduler.ScheduleAbsolute(500, () => subscription.Dispose());
        scheduler.ScheduleAbsolute(600, () => connection.Dispose());

        scheduler.Start();

        results.Messages.AssertEqual(
            OnNext(301, 3),
            OnNext(302, 4),
            OnNext(303, 5),
            OnNext(304, 6),
            OnNext(305, 7),
            OnNext(401, 8),
            OnNext(402, 9)
        );

        xs.Subscriptions.AssertEqual(
            Subscribe(200, 600)
        );
    }

    [TestMethod]
    public void Reentrant_Subject1()
    {
        var s = Subject.Synchronize((ISubject<int, int>)new Subject<int>(), global::System.Reactive.Concurrency.Scheduler.Immediate);
        var list = new List<int>();

        s.Subscribe(
            x =>
            {
                list.Add(x);
                if (x < 3)
                {
                    s.OnNext(x + 1);
                }

                list.Add(-x);
            });

        s.OnNext(1);

        list.AssertEqual(1, -1, 2, -2, 3, -3);
    }

    [TestMethod]
    public void Reentrant_Subject2()
    {
        var s = Subject.Synchronize(new Subject<int>(), global::System.Reactive.Concurrency.Scheduler.Immediate);
        var list = new List<int>();

        s.Subscribe(
            x =>
            {
                list.Add(x);
                if (x < 3)
                {
                    s.OnNext(x + 1);
                }

                list.Add(-x);
            });

        s.OnNext(1);

        list.AssertEqual(1, -1, 2, -2, 3, -3);
    }

    private static IEnumerable<int> Bug_1333_Enumerable(AsyncSubject<IDisposable> s, Semaphore sema)
    {
        var d = s.First();
        var t = new Thread(() => { d.Dispose(); sema.Release(); });
        t.Start();
        t.Join();
        yield return 1;
    }
}

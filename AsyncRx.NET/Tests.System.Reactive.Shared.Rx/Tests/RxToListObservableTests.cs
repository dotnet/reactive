// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information. 

using System.Reactive;
using System.Reactive.Concurrency;
using System.Reactive.Linq;

using Microsoft.Reactive.Testing;

namespace Tests.System.Reactive.Shared.Rx.Tests;

/// <summary>
/// Rx.NET's own <c>ToListObservable</c> tests, from its <c>ToListObservableTest.cs</c>.
/// </summary>
/// <remarks>
/// <c>ToListObservable</c> subscribes at the call and returns a <c>ListObservable&lt;T&gt;</c>: an
/// <c>IList&lt;T&gt;</c> of the elements so far, whose <c>Value</c> is the last one, and an
/// <c>IObservable&lt;object&gt;</c> that signals only the end. Rx.NET marks it
/// <c>[Experimental]</c>, in <c>System.Reactive.Linq.ObservableEx</c>; AsyncRx.NET has no
/// experimental surface and nothing of this shape, so these run here against Rx.NET only (the
/// maintainer's decision, Step 87: the suite keeps the upstream coverage). <c>ObservableEx</c> is
/// outside the code-generated stratum, so the argument-checking test comes too, with the upstream
/// <c>Dummy*</c> doubles replaced inline. All 6 of the file's tests, as the originals but for
/// MSTest's assertions and <c>null!</c> for the locals the scheduled actions fill in; the list
/// type itself has its own class, <c>RxListObservableTests</c>.
/// </remarks>
[TestClass]
public sealed class RxToListObservableTests : ReactiveTest
{
    [TestMethod]
    public void ToListObservable_ArgumentChecking()
    {
        Assert.ThrowsExactly<ArgumentNullException>(() => ((IObservable<int>)null!).ToListObservable());
        Assert.ThrowsExactly<ArgumentNullException>(() => Observable.Never<int>().ToListObservable().Subscribe(null!));
    }

    [TestMethod]
    public void ToListObservable_OnNext()
    {
        var scheduler = new TestScheduler();

        var xs = scheduler.CreateHotObservable(
            OnNext(300, 1),
            OnNext(400, 2),
            OnNext(500, 3),
            OnNext(600, 4)
        );

        var res = scheduler.Start(() =>
            xs.ToListObservable()
        );

        res.Messages.AssertEqual(
        );
    }

    [TestMethod]
    public void ToListObservable_OnError()
    {
        var scheduler = new TestScheduler();

        var ex = new InvalidOperationException();

        var xs = scheduler.CreateHotObservable(
            OnNext(300, 1),
            OnNext(400, 2),
            OnNext(500, 3),
            OnError<int>(600, ex)
        );

        ListObservable<int> s = null!;
        IDisposable subscription = null!;
        var res = scheduler.CreateObserver<object>();

        scheduler.ScheduleAbsolute(Created, () => s = xs.ToListObservable());
        scheduler.ScheduleAbsolute(Subscribed, () => subscription = s.Subscribe(res));
        scheduler.ScheduleAbsolute(Disposed, () => subscription.Dispose());

        scheduler.Start();

        Assert.ThrowsExactly<InvalidOperationException>(() => { var t = s.Value; });

        res.Messages.AssertEqual(
            OnError<object>(600, ex)
        );
    }

    [TestMethod]
    public void ToListObservable_OnCompleted()
    {
        var scheduler = new TestScheduler();

        var xs = scheduler.CreateHotObservable(
            OnNext(300, 1),
            OnNext(400, 2),
            OnNext(500, 3),
            OnCompleted<int>(600)
        );

        ListObservable<int> s = null!;

        var res = scheduler.Start(() =>
            s = xs.ToListObservable()
        );

        s.AssertEqual(1, 2, 3);

        res.Messages.AssertEqual(
            OnCompleted<object>(600)
        );

        Assert.AreEqual(3, s.Value);
    }

    [TestMethod]
    public void ToListObservable_Disposed()
    {
        var scheduler = new TestScheduler();

        var xs = scheduler.CreateHotObservable(
            OnNext(300, 1),
            OnNext(400, 2),
            OnNext(500, 3),
            OnNext(1050, 4),
            OnCompleted<int>(1100)
        );

        ListObservable<int> s = null!;

        var res = scheduler.Start(() =>
            s = xs.ToListObservable()
        );

        res.Messages.AssertEqual(
        );
    }

    [TestMethod]
    public void ToListObservable_Never()
    {
        var scheduler = new TestScheduler();

        var xs = scheduler.CreateHotObservable<int>(
        );

        var res = scheduler.Start(() =>
            xs.ToListObservable()
        );

        res.Messages.AssertEqual(
        );
    }
}

// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information. 

using System.Reactive.Linq;

using Microsoft.Reactive.Testing;

// Rx.NET marks its blocking operators obsolete, pointing at the async forms; these tests use
// them as the originals do under a project-wide suppression.
#pragma warning disable CS0618

namespace Tests.System.Reactive.Shared.Rx.Tests;

/// <summary>Rx.NET's own <c>ForkJoin</c> tests, from its <c>ForkJoinTest.cs</c>.</summary>
/// <remarks>
/// <c>ForkJoin</c> runs sequences in parallel and combines their last elements, pairwise through a
/// selector or as an array over <c>params</c> or an enumerable, completing empty if any source
/// was. Rx.NET marks it <c>[Experimental]</c>, in <c>System.Reactive.Linq.ObservableEx</c>;
/// AsyncRx.NET has no experimental surface and nothing of this shape, so these run here against
/// Rx.NET only (the maintainer's decision, Step 87: the suite keeps the upstream coverage).
/// <c>ObservableEx</c> is outside the code-generated stratum, so the argument-checking test comes
/// too, with the upstream <c>Dummy*</c> doubles replaced inline. All 22 of the file's tests, as
/// the originals but for MSTest's assertions and the two <c>TODO: fix ForkJoin behavior</c>
/// remarks left out (they mark expectations the original's author doubted but the tests pin);
/// <c>ForkJoin_Nary_Immediate</c> blocks on <c>First</c>, hence the obsolete-warning suppression.
/// The four <c>Bug_1302_*</c> tests are the regression guards for a selector throwing, or a side
/// having no element, at the second completion.
/// </remarks>
[TestClass]
public sealed class RxForkJoinTests : ReactiveTest
{
    [TestMethod]
    public void ForkJoin_ArgumentChecking()
    {
        var someObservable = Observable.Never<int>();

        Assert.ThrowsExactly<ArgumentNullException>(() => ObservableEx.ForkJoin(someObservable, someObservable, (Func<int, int, int>)null!));
        Assert.ThrowsExactly<ArgumentNullException>(() => ObservableEx.ForkJoin(someObservable, (IObservable<int>)null!, (_, __) => _ + __));
        Assert.ThrowsExactly<ArgumentNullException>(() => ObservableEx.ForkJoin((IObservable<int>)null!, someObservable, (_, __) => _ + __));
        Assert.ThrowsExactly<ArgumentNullException>(() => ObservableEx.ForkJoin((IObservable<int>[])null!));
        Assert.ThrowsExactly<ArgumentNullException>(() => ObservableEx.ForkJoin((IEnumerable<IObservable<int>>)null!));
    }

    [TestMethod]
    public void ForkJoin_EmptyEmpty()
    {
        var scheduler = new TestScheduler();

        var msgs1 = new[] {
            OnNext(150, 1),
            OnCompleted<int>(230)
        };

        var msgs2 = new[] {
            OnNext(150, 1),
            OnCompleted<int>(250)
        };

        var o = scheduler.CreateHotObservable(msgs1);
        var e = scheduler.CreateHotObservable(msgs2);

        var res = scheduler.Start(() => e.ForkJoin(o, (x, y) => x + y));
        res.Messages.AssertEqual(
            OnCompleted<int>(250)
        );
    }

    [TestMethod]
    public void ForkJoin_None()
    {
        var scheduler = new TestScheduler();

        var res = scheduler.Start(() => ObservableEx.ForkJoin<int>());
        res.Messages.AssertEqual(
            OnCompleted<int[]>(200)
        );
    }

    [TestMethod]
    public void ForkJoin_EmptyReturn()
    {
        var scheduler = new TestScheduler();

        var msgs1 = new[] {
            OnNext(150, 1),
            OnCompleted<int>(230)
        };

        var msgs2 = new[] {
            OnNext(150, 1),
            OnNext(210, 2),
            OnCompleted<int>(250)
        };

        var o = scheduler.CreateHotObservable(msgs1);
        var e = scheduler.CreateHotObservable(msgs2);

        var res = scheduler.Start(() => e.ForkJoin(o, (x, y) => x + y));
        res.Messages.AssertEqual(
            OnCompleted<int>(250)
        );
    }

    [TestMethod]
    public void ForkJoin_ReturnEmpty()
    {
        var scheduler = new TestScheduler();

        var msgs1 = new[] {
            OnNext(150, 1),
            OnNext(210, 2),
            OnCompleted<int>(230)
        };

        var msgs2 = new[] {
            OnNext(150, 1),
            OnCompleted<int>(250)
        };

        var o = scheduler.CreateHotObservable(msgs1);
        var e = scheduler.CreateHotObservable(msgs2);

        var res = scheduler.Start(() => e.ForkJoin(o, (x, y) => x + y));
        res.Messages.AssertEqual(
            OnCompleted<int>(250)
        );
    }

    [TestMethod]
    public void ForkJoin_ReturnReturn()
    {
        var scheduler = new TestScheduler();

        var msgs1 = new[] {
            OnNext(150, 1),
            OnNext(210, 2),
            OnCompleted<int>(230)
        };

        var msgs2 = new[] {
            OnNext(150, 1),
            OnNext(220, 3),
            OnCompleted<int>(250)
        };

        var o = scheduler.CreateHotObservable(msgs1);
        var e = scheduler.CreateHotObservable(msgs2);

        var res = scheduler.Start(() => e.ForkJoin(o, (x, y) => x + y));
        res.Messages.AssertEqual(
            OnNext(250, 2 + 3),
            OnCompleted<int>(250)
        );
    }

    [TestMethod]
    public void ForkJoin_EmptyThrow()
    {
        var ex = new Exception();

        var scheduler = new TestScheduler();

        var msgs1 = new[] {
            OnNext(150, 1),
            OnCompleted<int>(230)
        };

        var msgs2 = new[] {
            OnNext(150, 1),
            OnError<int>(210, ex),
            OnCompleted<int>(250)
        };

        var o = scheduler.CreateHotObservable(msgs1);
        var e = scheduler.CreateHotObservable(msgs2);

        var res = scheduler.Start(() => e.ForkJoin(o, (x, y) => x + y));
        res.Messages.AssertEqual(
            OnError<int>(210, ex)
        );
    }

    [TestMethod]
    public void ForkJoin_ThrowEmpty()
    {
        var ex = new Exception();

        var scheduler = new TestScheduler();

        var msgs1 = new[] {
            OnNext(150, 1),
            OnError<int>(210, ex),
            OnCompleted<int>(230)
        };

        var msgs2 = new[] {
            OnNext(150, 1),
            OnCompleted<int>(250)
        };

        var o = scheduler.CreateHotObservable(msgs1);
        var e = scheduler.CreateHotObservable(msgs2);

        var res = scheduler.Start(() => e.ForkJoin(o, (x, y) => x + y));
        res.Messages.AssertEqual(
            OnError<int>(210, ex)
        );
    }

    [TestMethod]
    public void ForkJoin_ReturnThrow()
    {
        var ex = new Exception();

        var scheduler = new TestScheduler();

        var msgs1 = new[] {
            OnNext(150, 1),
            OnNext(210, 2),
            OnCompleted<int>(230)
        };

        var msgs2 = new[] {
            OnNext(150, 1),
            OnError<int>(220, ex),
            OnCompleted<int>(250)
        };

        var o = scheduler.CreateHotObservable(msgs1);
        var e = scheduler.CreateHotObservable(msgs2);

        var res = scheduler.Start(() => e.ForkJoin(o, (x, y) => x + y));
        res.Messages.AssertEqual(
            OnError<int>(220, ex)
        );
    }

    [TestMethod]
    public void ForkJoin_ThrowReturn()
    {
        var ex = new Exception();

        var scheduler = new TestScheduler();

        var msgs1 = new[] {
            OnNext(150, 1),
            OnError<int>(220, ex),
            OnCompleted<int>(230)
        };

        var msgs2 = new[] {
            OnNext(150, 1),
            OnNext(210, 2),
            OnCompleted<int>(250)
        };

        var o = scheduler.CreateHotObservable(msgs1);
        var e = scheduler.CreateHotObservable(msgs2);

        var res = scheduler.Start(() => e.ForkJoin(o, (x, y) => x + y));
        res.Messages.AssertEqual(
            OnError<int>(220, ex)
        );
    }

    [TestMethod]
    public void ForkJoin_Binary()
    {
        var scheduler = new TestScheduler();

        var msgs1 = new[] {
            OnNext(150, 1),
            OnNext(215, 2),
            OnNext(225, 4),
            OnCompleted<int>(230)
        };

        var msgs2 = new[] {
            OnNext(150, 1),
            OnNext(235, 6),
            OnNext(240, 7),
            OnCompleted<int>(250)
        };

        var o = scheduler.CreateHotObservable(msgs1);
        var e = scheduler.CreateHotObservable(msgs2);

        var res = scheduler.Start(() => e.ForkJoin(o, (x, y) => x + y));
        res.Messages.AssertEqual(
            OnNext(250, 4 + 7),
            OnCompleted<int>(250)
        );
    }

    [TestMethod]
    public void ForkJoin_NaryParams()
    {
        var scheduler = new TestScheduler();

        var msgs1 = new[] {
            OnNext(150, 1),
            OnNext(215, 2),
            OnNext(225, 4),
            OnCompleted<int>(230)
        };

        var msgs2 = new[] {
            OnNext(150, 1),
            OnNext(235, 6),
            OnNext(240, 7),
            OnCompleted<int>(250)
        };

        var msgs3 = new[] {
            OnNext(150, 1),
            OnNext(230, 3),
            OnNext(245, 5),
            OnCompleted<int>(270)
        };

        var o1 = scheduler.CreateHotObservable(msgs1);
        var o2 = scheduler.CreateHotObservable(msgs2);
        var o3 = scheduler.CreateHotObservable(msgs3);

        var res = scheduler.Start(() => ObservableEx.ForkJoin(o1, o2, o3));

        res.Messages.AssertEqual(
            OnNext<int[]>(270, l => l.SequenceEqual([4, 7, 5])),
            OnCompleted<int[]>(270)
        );
    }

    [TestMethod]
    public void ForkJoin_NaryParamsEmpty()
    {
        var scheduler = new TestScheduler();

        var msgs1 = new[] {
            OnNext(150, 1),
            OnNext(215, 2),
            OnNext(225, 4),
            OnCompleted<int>(230)
        };

        var msgs2 = new[] {
            OnNext(150, 1),
            OnNext(235, 6),
            OnNext(240, 7),
            OnCompleted<int>(250)
        };

        var msgs3 = new[] {
            OnCompleted<int>(270)
        };

        var o1 = scheduler.CreateHotObservable(msgs1);
        var o2 = scheduler.CreateHotObservable(msgs2);
        var o3 = scheduler.CreateHotObservable(msgs3);

        var res = scheduler.Start(() => ObservableEx.ForkJoin(o1, o2, o3));

        res.Messages.AssertEqual(
            OnCompleted<int[]>(270)
        );
    }

    [TestMethod]
    public void ForkJoin_NaryParamsEmptyBeforeEnd()
    {
        var scheduler = new TestScheduler();

        var msgs1 = new[] {
            OnNext(150, 1),
            OnNext(215, 2),
            OnNext(225, 4),
            OnCompleted<int>(230)
        };

        var msgs2 = new[] {
            OnCompleted<int>(235)
        };

        var msgs3 = new[] {
            OnNext(150, 1),
            OnNext(230, 3),
            OnNext(245, 5),
            OnCompleted<int>(270)
        };

        var o1 = scheduler.CreateHotObservable(msgs1);
        var o2 = scheduler.CreateHotObservable(msgs2);
        var o3 = scheduler.CreateHotObservable(msgs3);

        var res = scheduler.Start(() => ObservableEx.ForkJoin(o1, o2, o3));

        res.Messages.AssertEqual(
            OnCompleted<int[]>(235)
        );
    }

    [TestMethod]
    public void ForkJoin_Nary_Immediate()
    {
        ObservableEx.ForkJoin(Observable.Return(1), Observable.Return(2)).First().AssertEqual([1, 2]);
    }

    [TestMethod]
    public void ForkJoin_Nary_Virtual_And_Immediate()
    {
        var scheduler = new TestScheduler();

        var msgs1 = new[] {
            OnNext(150, 1),
            OnNext(215, 2),
            OnNext(225, 4),
            OnCompleted<int>(230)
        };

        var msgs2 = new[] {
            OnNext(150, 1),
            OnNext(235, 6),
            OnNext(240, 7),
            OnCompleted<int>(250)
        };

        var msgs3 = new[] {
            OnNext(150, 1),
            OnNext(230, 3),
            OnNext(245, 5),
            OnCompleted<int>(270)
        };

        var o1 = scheduler.CreateHotObservable(msgs1);
        var o2 = scheduler.CreateHotObservable(msgs2);
        var o3 = scheduler.CreateHotObservable(msgs3);

        var res = scheduler.Start(() => ObservableEx.ForkJoin(new List<IObservable<int>> { o1, o2, o3, Observable.Return(20) }));

        res.Messages.AssertEqual(
            OnNext<int[]>(270, l => l.SequenceEqual([4, 7, 5, 20])),
            OnCompleted<int[]>(270)
        );
    }

    [TestMethod]
    public void ForkJoin_Nary_Immediate_And_Virtual()
    {
        var scheduler = new TestScheduler();

        var msgs1 = new[] {
            OnNext(150, 1),
            OnNext(215, 2),
            OnNext(225, 4),
            OnCompleted<int>(230)
        };

        var msgs2 = new[] {
            OnNext(150, 1),
            OnNext(235, 6),
            OnNext(240, 7),
            OnCompleted<int>(250)
        };

        var msgs3 = new[] {
            OnNext(150, 1),
            OnNext(230, 3),
            OnNext(245, 5),
            OnCompleted<int>(270)
        };

        var o1 = scheduler.CreateHotObservable(msgs1);
        var o2 = scheduler.CreateHotObservable(msgs2);
        var o3 = scheduler.CreateHotObservable(msgs3);

        var res = scheduler.Start(() => ObservableEx.ForkJoin(new List<IObservable<int>> { Observable.Return(20), o1, o2, o3 }));

        res.Messages.AssertEqual(
            OnNext<int[]>(270, l => l.SequenceEqual([20, 4, 7, 5])),
            OnCompleted<int[]>(270)
        );
    }

    [TestMethod]
    public void ForkJoin_Nary()
    {
        var scheduler = new TestScheduler();

        var msgs1 = new[] {
            OnNext(150, 1),
            OnNext(215, 2),
            OnNext(225, 4),
            OnCompleted<int>(230)
        };

        var msgs2 = new[] {
            OnNext(150, 1),
            OnNext(235, 6),
            OnNext(240, 7),
            OnCompleted<int>(250)
        };

        var msgs3 = new[] {
            OnNext(150, 1),
            OnNext(230, 3),
            OnNext(245, 5),
            OnCompleted<int>(270)
        };

        var o1 = scheduler.CreateHotObservable(msgs1);
        var o2 = scheduler.CreateHotObservable(msgs2);
        var o3 = scheduler.CreateHotObservable(msgs3);

        var res = scheduler.Start(() => ObservableEx.ForkJoin(new List<IObservable<int>> { o1, o2, o3 }));

        res.Messages.AssertEqual(
            OnNext<int[]>(270, l => l.SequenceEqual([4, 7, 5])),
            OnCompleted<int[]>(270)
        );
    }

    [TestMethod]
    public void Bug_1302_SelectorThrows_LeftLast()
    {
        var scheduler = new TestScheduler();

        var xs = scheduler.CreateHotObservable(
            OnNext(210, 1),
            OnCompleted<int>(220)
        );

        var ys = scheduler.CreateHotObservable(
            OnNext(215, 2),
            OnCompleted<int>(217)
        );

        var ex = new Exception();

        var results = scheduler.Start(() => xs.ForkJoin<int, int, int>(ys, (x, y) => { throw ex; }));

        results.Messages.AssertEqual(
            OnError<int>(220, ex)
        );

        xs.Subscriptions.AssertEqual(
            Subscribe(200, 220)
        );

        ys.Subscriptions.AssertEqual(
            Subscribe(200, 217)
        );
    }

    [TestMethod]
    public void Bug_1302_SelectorThrows_RightLast()
    {
        var scheduler = new TestScheduler();

        var xs = scheduler.CreateHotObservable(
            OnNext(210, 1),
            OnCompleted<int>(217)
        );

        var ys = scheduler.CreateHotObservable(
            OnNext(215, 2),
            OnCompleted<int>(220)
        );

        var ex = new Exception();

        var results = scheduler.Start(() => xs.ForkJoin<int, int, int>(ys, (x, y) => { throw ex; }));

        results.Messages.AssertEqual(
            OnError<int>(220, ex)
        );

        xs.Subscriptions.AssertEqual(
            Subscribe(200, 217)
        );

        ys.Subscriptions.AssertEqual(
            Subscribe(200, 220)
        );
    }

    [TestMethod]
    public void Bug_1302_RightLast_NoLeft()
    {
        var scheduler = new TestScheduler();

        var xs = scheduler.CreateHotObservable(
            OnCompleted<int>(217)
        );

        var ys = scheduler.CreateHotObservable(
            OnNext(215, 2),
            OnCompleted<int>(220)
        );

        var results = scheduler.Start(() => xs.ForkJoin(ys, (x, y) => x + y));

        results.Messages.AssertEqual(
            OnCompleted<int>(220)
        );

        xs.Subscriptions.AssertEqual(
            Subscribe(200, 217)
        );

        ys.Subscriptions.AssertEqual(
            Subscribe(200, 220)
        );
    }

    [TestMethod]
    public void Bug_1302_RightLast_NoRight()
    {
        var scheduler = new TestScheduler();

        var xs = scheduler.CreateHotObservable(
            OnNext(215, 2),
            OnCompleted<int>(217)
        );

        var ys = scheduler.CreateHotObservable(
            OnCompleted<int>(220)
        );

        var results = scheduler.Start(() => xs.ForkJoin(ys, (x, y) => x + y));

        results.Messages.AssertEqual(
            OnCompleted<int>(220)
        );

        xs.Subscriptions.AssertEqual(
            Subscribe(200, 217)
        );

        ys.Subscriptions.AssertEqual(
            Subscribe(200, 220)
        );
    }
}

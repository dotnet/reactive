// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information. 

using System.Reactive.Concurrency;
using System.Reactive.Linq;

using Microsoft.Reactive.Testing;

// Rx.NET marks its blocking operators obsolete, pointing at the async forms; these tests use
// them as the originals do under a project-wide suppression.
#pragma warning disable CS0618
#pragma warning disable IDE0039 // Use local function

namespace Tests.System.Reactive.Shared.Rx.Tests;

/// <summary>Rx.NET's own <c>ManySelect</c> tests, from its <c>ManySelectTest.cs</c>.</summary>
/// <remarks>
/// <c>ManySelect</c> is the comonadic bind: the selector sees, for each element, the sequence from
/// that element onward, and its results form the output, on a scheduler or the default. Rx.NET
/// marks it <c>[Experimental]</c>, in <c>System.Reactive.Linq.ObservableEx</c>; AsyncRx.NET has no
/// experimental surface and nothing of this shape, so these run here against Rx.NET only (the
/// maintainer's decision, Step 87: the suite keeps the upstream coverage). <c>ObservableEx</c> is
/// outside the code-generated stratum, so the argument-checking test comes too, with the upstream
/// <c>Dummy*</c> doubles replaced inline. All 6 of the file's tests, as the originals but for
/// MSTest's assertions: the three comonad laws, checked by blocking on <c>First</c> and
/// <c>Last</c> (hence the obsolete-warning suppression, and the original's <c>IDE0039</c> one for
/// its delegate locals), and two virtual-time shapes. Of the experimental operators this is the
/// one with no simple composition of operators AsyncRx.NET has.
/// </remarks>
[TestClass]
public sealed class RxManySelectTests : ReactiveTest
{
    [TestMethod]
    public void ManySelect_ArgumentChecking()
    {
        Assert.ThrowsExactly<ArgumentNullException>(() => ObservableEx.ManySelect(null!, new Func<IObservable<int>, int>(_ => throw new NotImplementedException()), CurrentThreadScheduler.Instance));
        Assert.ThrowsExactly<ArgumentNullException>(() => ObservableEx.ManySelect<int, int>(Observable.Never<int>(), null!, CurrentThreadScheduler.Instance));
        Assert.ThrowsExactly<ArgumentNullException>(() => ObservableEx.ManySelect(Observable.Never<int>(), new Func<IObservable<int>, int>(_ => throw new NotImplementedException()), null!));
        Assert.ThrowsExactly<ArgumentNullException>(() => ObservableEx.ManySelect(null!, new Func<IObservable<int>, int>(_ => throw new NotImplementedException())));
        Assert.ThrowsExactly<ArgumentNullException>(() => ObservableEx.ManySelect<int, int>(Observable.Never<int>(), null!));
    }

    [TestMethod]
    public void ManySelect_Law_1()
    {
        var xs = Observable.Range(1, 0);

        var left = xs.ManySelect(Observable.First);
        var right = xs;

        Assert.IsTrue(left.SequenceEqual(right).First());
    }

    [TestMethod]
    public void ManySelect_Law_2()
    {
        var xs = Observable.Range(1, 10);
        Func<IObservable<int>, int> f = ys => ys.Count().First();

        var left = xs.ManySelect(f).First();
        var right = f(xs);

        Assert.AreEqual(left, right);
    }

    [TestMethod]
    public void ManySelect_Law_3()
    {
        var xs = Observable.Range(1, 10);
        Func<IObservable<int>, int> f = ys => ys.Count().First();
        Func<IObservable<int>, int> g = ys => ys.Last();

        var left = xs.ManySelect(f).ManySelect(g);
        var right = xs.ManySelect(ys => g(ys.ManySelect(f)));

        Assert.IsTrue(left.SequenceEqual(right).First());
    }

    [TestMethod]
    public void ManySelect_Basic()
    {
        var scheduler = new TestScheduler();

        var xs = scheduler.CreateHotObservable(
            OnNext(100, 1),
            OnNext(220, 2),
            OnNext(270, 3),
            OnNext(410, 4),
            OnCompleted<int>(500)
        );

        var res = scheduler.Start(() =>
            xs.ManySelect(ys => ys.First(), scheduler)
        );

        res.Messages.AssertEqual(
            OnNext(221, 2),
            OnNext(271, 3),
            OnNext(411, 4),
            OnCompleted<int>(501)
        );

        xs.Subscriptions.AssertEqual(
            Subscribe(200, 500)
        );
    }

    [TestMethod]
    public void ManySelect_Error()
    {
        var scheduler = new TestScheduler();

        var ex = new Exception();

        var xs = scheduler.CreateHotObservable(
            OnNext(100, 1),
            OnNext(220, 2),
            OnNext(270, 3),
            OnNext(410, 4),
            OnError<int>(500, ex)
        );

        var res = scheduler.Start(() =>
            xs.ManySelect(ys => ys.First(), scheduler)
        );

        res.Messages.AssertEqual(
            OnNext(221, 2),
            OnNext(271, 3),
            OnNext(411, 4),
            OnError<int>(501, ex)
        );

        xs.Subscriptions.AssertEqual(
            Subscribe(200, 500)
        );
    }
}

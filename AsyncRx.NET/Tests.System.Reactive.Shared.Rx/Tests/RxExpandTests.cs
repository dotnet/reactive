// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information. 

using System.Reactive.Concurrency;
using System.Reactive.Linq;

using Microsoft.Reactive.Testing;

// Rx.NET marks its blocking operators obsolete, pointing at the async forms; these tests use
// them as the originals do under a project-wide suppression.
#pragma warning disable CS0618

namespace Tests.System.Reactive.Shared.Rx.Tests;

/// <summary>Rx.NET's own <c>Expand</c> tests, from its <c>ExpandTest.cs</c>.</summary>
/// <remarks>
/// <c>Expand</c> recursively applies a selector to each element, merging what it returns back into
/// the sequence, on a scheduler or the default. Rx.NET marks it <c>[Experimental]</c>, in
/// <c>System.Reactive.Linq.ObservableEx</c>; AsyncRx.NET has no experimental surface and nothing
/// of this shape, so these run here against Rx.NET only (the maintainer's decision, Step 87: the
/// suite keeps the upstream coverage). <c>ObservableEx</c> is outside the code-generated stratum,
/// so the argument-checking test comes too, with the upstream <c>Dummy*</c> doubles replaced
/// inline. All 7 of the file's tests, as the originals but for MSTest's assertions;
/// <c>Expand_Default</c> blocks on <c>First</c>, hence the obsolete-warning suppression.
/// </remarks>
[TestClass]
public sealed class RxExpandTests : ReactiveTest
{
    [TestMethod]
    public void Expand_ArgumentChecking()
    {
        Assert.ThrowsExactly<ArgumentNullException>(() => ObservableEx.Expand(null!, new Func<int, IObservable<int>>(_ => throw new NotImplementedException()), CurrentThreadScheduler.Instance));
        Assert.ThrowsExactly<ArgumentNullException>(() => ObservableEx.Expand(Observable.Never<int>(), null!, CurrentThreadScheduler.Instance));
        Assert.ThrowsExactly<ArgumentNullException>(() => ObservableEx.Expand(Observable.Never<int>(), new Func<int, IObservable<int>>(_ => throw new NotImplementedException()), null!));
        Assert.ThrowsExactly<ArgumentNullException>(() => ObservableEx.Expand(null!, new Func<int, IObservable<int>>(_ => throw new NotImplementedException())));
        Assert.ThrowsExactly<ArgumentNullException>(() => ObservableEx.Expand(Observable.Never<int>(), null!));
    }

    [TestMethod]
    public void Expand_Default()
    {
        var b = Observable.Return(1).Expand(x => x < 10 ? Observable.Return(x + 1) : Observable.Empty<int>())
                    .SequenceEqual(Observable.Range(1, 10)).First();

        Assert.IsTrue(b);
    }

    [TestMethod]
    public void Expand_Empty()
    {
        var scheduler = new TestScheduler();

        var xs = scheduler.CreateHotObservable(
            OnCompleted<int>(300)
        );

        var res = scheduler.Start(() =>
            xs.Expand(x => scheduler.CreateColdObservable(
                OnNext(100, 1),
                OnNext(200, 2),
                OnCompleted<int>(300)
            ), scheduler)
        );

        res.Messages.AssertEqual(
            OnCompleted<int>(300)
        );

        xs.Subscriptions.AssertEqual(
            Subscribe(201, 300)
        );
    }

    [TestMethod]
    public void Expand_Error()
    {
        var scheduler = new TestScheduler();

        var ex = new Exception();

        var xs = scheduler.CreateHotObservable(
            OnError<int>(300, ex)
        );

        var res = scheduler.Start(() =>
            xs.Expand(x => scheduler.CreateColdObservable(
                OnNext(100 + x, 2 * x),
                OnNext(200 + x, 3 * x),
                OnCompleted<int>(300 + x)
            ), scheduler)
        );

        res.Messages.AssertEqual(
            OnError<int>(300, ex)
        );

        xs.Subscriptions.AssertEqual(
            Subscribe(201, 300)
        );
    }

    [TestMethod]
    public void Expand_Never()
    {
        var scheduler = new TestScheduler();

        var xs = scheduler.CreateHotObservable<int>(
        );

        var res = scheduler.Start(() =>
            xs.Expand(x => scheduler.CreateColdObservable(
                OnNext(100 + x, 2 * x),
                OnNext(200 + x, 3 * x),
                OnCompleted<int>(300 + x)
            ), scheduler)
        );

        res.Messages.AssertEqual(
        );

        xs.Subscriptions.AssertEqual(
            Subscribe(201, 1000)
        );
    }

    [TestMethod]
    public void Expand_Basic()
    {
        var scheduler = new TestScheduler();

        var xs = scheduler.CreateHotObservable(
            OnNext(550, 1),
            OnNext(850, 2),
            OnCompleted<int>(950)
        );

        var res = scheduler.Start(() =>
            xs.Expand(x => scheduler.CreateColdObservable(
                OnNext(100, 2 * x),
                OnNext(200, 3 * x),
                OnCompleted<int>(300)
            ), scheduler)
        );

        res.Messages.AssertEqual(
            OnNext(550, 1),
            OnNext(651, 2),
            OnNext(751, 3),
            OnNext(752, 4),
            OnNext(850, 2),
            OnNext(852, 6),
            OnNext(852, 6),
            OnNext(853, 8),
            OnNext(951, 4),
            OnNext(952, 9),
            OnNext(952, 12),
            OnNext(953, 12),
            OnNext(953, 12),
            OnNext(954, 16)
        );

        xs.Subscriptions.AssertEqual(
            Subscribe(201, 950)
        );
    }

    [TestMethod]
    public void Expand_Throw()
    {
        var scheduler = new TestScheduler();

        var xs = scheduler.CreateHotObservable(
            OnNext(550, 1),
            OnNext(850, 2),
            OnCompleted<int>(950)
        );

        var ex = new Exception();

        var res = scheduler.Start(() =>
            xs.Expand(x => { throw ex; }, scheduler)
        );

        res.Messages.AssertEqual(
            OnNext(550, 1),
            OnError<int>(550, ex)
        );

        xs.Subscriptions.AssertEqual(
            Subscribe(201, 550)
        );
    }
}

// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information. 

using System.Reactive;
using System.Reactive.Linq;

using Microsoft.Reactive.Testing;

namespace Tests.System.Reactive.Shared.Rx.Tests;

/// <summary>Rx.NET's own <c>CreateIterator</c> tests, from its <c>CreateTest.cs</c>.</summary>
/// <remarks>
/// The iterator form of <c>Create</c>: an iterator method yields observables, each is subscribed
/// in turn and the iteration moves on when it completes, and what the method sends to the observer
/// it is given (or nothing, in the <c>Unit</c>-valued form without one) is the result. Rx.NET
/// marks it <c>[Experimental]</c>, in <c>System.Reactive.Linq.ObservableEx</c>; AsyncRx.NET has no
/// experimental surface and nothing of this shape, so these run here against Rx.NET only (the
/// maintainer's decision, Step 87: the suite keeps the upstream coverage). <c>ObservableEx</c> is
/// outside the code-generated stratum, so the argument-checking test comes too, with the upstream
/// <c>Dummy*</c> doubles replaced inline. The 14 <c>Iterate*</c> and <c>IteratorScenario*</c>
/// tests of Rx.NET's <c>CreateTest.cs</c>, which Step 28 left out of the shared
/// <c>CreateTests</c>, as the originals but for MSTest's assertions and <c>default!</c> where the
/// nullable context would warn, with the eleven iterator methods they drive.
/// </remarks>
[TestClass]
public sealed class RxCreateIteratorTests : ReactiveTest
{
    [TestMethod]
    public void Iterate_ArgumentChecking()
    {
        Assert.ThrowsExactly<ArgumentNullException>(() => ObservableEx.Create<int>(default!));
        Assert.ThrowsExactly<ArgumentNullException>(() => ObservableEx.Create(new Func<IObserver<int>, IEnumerable<IObservable<object>>>(_ => throw new NotImplementedException())).Subscribe(null!));
    }

    [TestMethod]
    public void Iterate_Complete()
    {
        var scheduler = new TestScheduler();

        var xs = scheduler.CreateColdObservable(
            OnNext(10, 1),
            OnNext(20, 2),
            OnNext(30, 3),
            OnNext(40, 4),
            OnCompleted<int>(50)
        );

        var ys = scheduler.CreateColdObservable(
            OnNext(10, 1),
            OnNext(20, 2),
            OnCompleted<int>(30)
        );

        var zs = scheduler.CreateColdObservable(
            OnNext(10, 1),
            OnNext(20, 2),
            OnNext(30, 3),
            OnNext(40, 4),
            OnNext(50, 5),
            OnCompleted<int>(60)
        );

        var res = scheduler.Start(() => ObservableEx.Create<int>(observer => ToIterate_Complete(xs, ys, zs, observer)));

        res.Messages.AssertEqual(
            OnNext(200, 1),
            OnNext(250, 2),
            OnNext(280, 3),
            OnCompleted<int>(280)
        );

        xs.Subscriptions.AssertEqual(
            Subscribe(200, 250)
        );

        ys.Subscriptions.AssertEqual(
            Subscribe(250, 280)
        );

        zs.Subscriptions.AssertEqual(
            Subscribe(280, 280)
        );
    }

    [TestMethod]
    public void Iterate_Complete_Implicit()
    {
        var scheduler = new TestScheduler();

        var xs = scheduler.CreateColdObservable(
            OnNext(10, 1),
            OnNext(20, 2),
            OnNext(30, 3),
            OnNext(40, 4),
            OnCompleted<int>(50)
        );

        var ys = scheduler.CreateColdObservable(
            OnNext(10, 1),
            OnNext(20, 2),
            OnCompleted<int>(30)
        );

        var zs = scheduler.CreateColdObservable(
            OnNext(10, 1),
            OnNext(20, 2),
            OnNext(30, 3),
            OnNext(40, 4),
            OnNext(50, 5),
            OnCompleted<int>(60)
        );

        var res = scheduler.Start(() => ObservableEx.Create<int>(observer => ToIterate_Complete_Implicit(xs, ys, zs, observer)));

        res.Messages.AssertEqual(
            OnNext(200, 1),
            OnNext(250, 2),
            OnNext(280, 3),
            OnNext(340, 4),
            OnCompleted<int>(340)
        );

        xs.Subscriptions.AssertEqual(
            Subscribe(200, 250)
        );

        ys.Subscriptions.AssertEqual(
            Subscribe(250, 280)
        );

        zs.Subscriptions.AssertEqual(
            Subscribe(280, 340)
        );
    }

    [TestMethod]
    public void Iterate_Iterator_Throw()
    {
        var scheduler = new TestScheduler();

        var xs = scheduler.CreateColdObservable(
            OnNext(10, 1),
            OnNext(20, 2),
            OnNext(30, 3),
            OnNext(40, 4),
            OnCompleted<int>(50)
        );

        var ys = scheduler.CreateColdObservable(
            OnNext(10, 1),
            OnNext(20, 2),
            OnCompleted<int>(30)
        );

        var zs = scheduler.CreateColdObservable(
            OnNext(10, 1),
            OnNext(20, 2),
            OnNext(30, 3),
            OnNext(40, 4),
            OnNext(50, 5),
            OnCompleted<int>(60)
        );

        var ex = new Exception();

        var res = scheduler.Start(() => ObservableEx.Create<int>(observer => ToIterate_Throw(xs, ys, zs, observer, ex)));

        res.Messages.AssertEqual(
            OnNext(200, 1),
            OnNext(250, 2),
            OnNext(280, 3),
            OnError<int>(280, ex)
        );

        xs.Subscriptions.AssertEqual(
            Subscribe(200, 250)
        );

        ys.Subscriptions.AssertEqual(
            Subscribe(250, 280)
        );

        zs.Subscriptions.AssertEqual(
        );
    }

    [TestMethod]
    public void Iterate_Iterator_Error()
    {
        var scheduler = new TestScheduler();

        var ex = new Exception();

        var xs = scheduler.CreateColdObservable(
            OnNext(10, 1),
            OnNext(20, 2),
            OnNext(30, 3),
            OnNext(40, 4),
            OnCompleted<int>(50)
        );

        var ys = scheduler.CreateColdObservable(
            OnNext(10, 1),
            OnNext(20, 2),
            OnCompleted<int>(30)
        );

        var zs = scheduler.CreateColdObservable(
            OnNext(10, 1),
            OnNext(20, 2),
            OnNext(30, 3),
            OnNext(40, 4),
            OnNext(50, 5),
            OnCompleted<int>(60)
        );

        var res = scheduler.Start(() => ObservableEx.Create<int>(observer => ToIterate_Error(xs, ys, zs, observer, ex)));

        res.Messages.AssertEqual(
            OnNext(200, 1),
            OnNext(250, 2),
            OnError<int>(250, ex)
        );

        xs.Subscriptions.AssertEqual(
            Subscribe(200, 250)
        );

        ys.Subscriptions.AssertEqual(
            Subscribe(250, 250)
        );

        zs.Subscriptions.AssertEqual(
        );
    }

    [TestMethod]
    public void Iterate_Complete_Dispose()
    {
        var scheduler = new TestScheduler();

        var xs = scheduler.CreateColdObservable(
            OnNext(10, 1),
            OnNext(20, 2),
            OnNext(30, 3),
            OnNext(40, 4),
            OnCompleted<int>(50)
        );

        var ys = scheduler.CreateColdObservable(
            OnNext(10, 1),
            OnNext(20, 2),
            OnCompleted<int>(30)
        );

        var zs = scheduler.CreateColdObservable(
            OnNext(100, 1),
            OnNext(200, 2),
            OnNext(300, 3),
            OnNext(400, 4),
            OnNext(500, 5),
            OnNext(600, 6),
            OnNext(700, 7),
            OnNext(800, 8),
            OnNext(900, 9),
            OnNext(1000, 10)
        );

        var res = scheduler.Start(() => ObservableEx.Create<int>(observer => ToIterate_Complete_Dispose(xs, ys, zs, observer)));

        res.Messages.AssertEqual(
            OnNext(200, 1),
            OnNext(250, 2),
            OnNext(280, 3)
        );

        xs.Subscriptions.AssertEqual(
            Subscribe(200, 250)
        );

        ys.Subscriptions.AssertEqual(
            Subscribe(250, 280)
        );

        zs.Subscriptions.AssertEqual(
            Subscribe(280, 1000)
        );
    }

    [TestMethod]
    public void IteratorScenario()
    {
        var xs = ObservableEx.Create<int>(o => _IteratorScenario(100, 1000, o));

        xs.AssertEqual(new[] { 100, 1000 }.ToObservable());
    }

    [TestMethod]
    public void Iterate_Void_ArgumentChecking()
    {
        Assert.ThrowsExactly<ArgumentNullException>(() => ObservableEx.Create(default!));
        Assert.ThrowsExactly<ArgumentNullException>(() => ObservableEx.Create(new Func<IEnumerable<IObservable<object>>>(() => throw new NotImplementedException())).Subscribe(null!));
    }

    [TestMethod]
    public void Iterate_Void_Complete()
    {
        var scheduler = new TestScheduler();

        var xs = scheduler.CreateColdObservable(
            OnNext(10, 1),
            OnNext(20, 2),
            OnNext(30, 3),
            OnNext(40, 4),
            OnCompleted<int>(50)
        );

        var ys = scheduler.CreateColdObservable(
            OnNext(10, 1),
            OnNext(20, 2),
            OnCompleted<int>(30)
        );

        var zs = scheduler.CreateColdObservable(
            OnNext(10, 1),
            OnNext(20, 2),
            OnNext(30, 3),
            OnNext(40, 4),
            OnNext(50, 5),
            OnCompleted<int>(60)
        );

        var res = scheduler.Start(() => ObservableEx.Create(() => ToIterate_Void_Complete(xs, ys, zs)));

        res.Messages.AssertEqual(
            OnCompleted<Unit>(340)
        );

        xs.Subscriptions.AssertEqual(
            Subscribe(200, 250)
        );

        ys.Subscriptions.AssertEqual(
            Subscribe(250, 280)
        );

        zs.Subscriptions.AssertEqual(
            Subscribe(280, 340)
        );
    }

    [TestMethod]
    public void Iterate_Void_Complete_Implicit()
    {
        var scheduler = new TestScheduler();

        var xs = scheduler.CreateColdObservable(
            OnNext(10, 1),
            OnNext(20, 2),
            OnNext(30, 3),
            OnNext(40, 4),
            OnCompleted<int>(50)
        );

        var ys = scheduler.CreateColdObservable(
            OnNext(10, 1),
            OnNext(20, 2),
            OnCompleted<int>(30)
        );

        var zs = scheduler.CreateColdObservable(
            OnNext(10, 1),
            OnNext(20, 2),
            OnNext(30, 3),
            OnNext(40, 4),
            OnNext(50, 5),
            OnCompleted<int>(60)
        );

        var res = scheduler.Start(() => ObservableEx.Create(() => ToIterate_Void_Complete_Implicit(xs, ys, zs)));

        res.Messages.AssertEqual(
            OnCompleted<Unit>(340)
        );

        xs.Subscriptions.AssertEqual(
            Subscribe(200, 250)
        );

        ys.Subscriptions.AssertEqual(
            Subscribe(250, 280)
        );

        zs.Subscriptions.AssertEqual(
            Subscribe(280, 340)
        );
    }

    [TestMethod]
    public void Iterate_Void_Iterator_Throw()
    {
        var scheduler = new TestScheduler();

        var xs = scheduler.CreateColdObservable(
            OnNext(10, 1),
            OnNext(20, 2),
            OnNext(30, 3),
            OnNext(40, 4),
            OnCompleted<int>(50)
        );

        var ys = scheduler.CreateColdObservable(
            OnNext(10, 1),
            OnNext(20, 2),
            OnCompleted<int>(30)
        );

        var zs = scheduler.CreateColdObservable(
            OnNext(10, 1),
            OnNext(20, 2),
            OnNext(30, 3),
            OnNext(40, 4),
            OnNext(50, 5),
            OnCompleted<int>(60)
        );

        var ex = new Exception();

        var res = scheduler.Start(() => ObservableEx.Create(() => ToIterate_Void_Throw(xs, ys, zs, ex)));

        res.Messages.AssertEqual(
            OnError<Unit>(280, ex)
        );

        xs.Subscriptions.AssertEqual(
            Subscribe(200, 250)
        );

        ys.Subscriptions.AssertEqual(
            Subscribe(250, 280)
        );

        zs.Subscriptions.AssertEqual(
        );
    }

    [TestMethod]
    public void Iterate_Void_Complete_Dispose()
    {
        var scheduler = new TestScheduler();

        var xs = scheduler.CreateColdObservable(
            OnNext(10, 1),
            OnNext(20, 2),
            OnNext(30, 3),
            OnNext(40, 4),
            OnCompleted<int>(50)
        );

        var ys = scheduler.CreateColdObservable(
            OnNext(10, 1),
            OnNext(20, 2),
            OnCompleted<int>(30)
        );

        var zs = scheduler.CreateColdObservable(
            OnNext(100, 1),
            OnNext(200, 2),
            OnNext(300, 3),
            OnNext(400, 4),
            OnNext(500, 5),
            OnNext(600, 6),
            OnNext(700, 7),
            OnNext(800, 8),
            OnNext(900, 9),
            OnNext(1000, 10)
        );

        var res = scheduler.Start(() => ObservableEx.Create(() => ToIterate_Void_Complete_Dispose(xs, ys, zs)));

        res.Messages.AssertEqual(
        );

        xs.Subscriptions.AssertEqual(
            Subscribe(200, 250)
        );

        ys.Subscriptions.AssertEqual(
            Subscribe(250, 280)
        );

        zs.Subscriptions.AssertEqual(
            Subscribe(280, 1000)
        );
    }

    [TestMethod]
    public void Iterate_Void_Func_Throw()
    {
        var scheduler = new TestScheduler();

        var obs = scheduler.Start(() => ObservableEx.Create(() => { throw new InvalidOperationException(); }));

        Assert.AreEqual(1, obs.Messages.Count);

        var notification = obs.Messages[0].Value;
        Assert.AreEqual(NotificationKind.OnError, notification.Kind);
        Assert.IsInstanceOfType<InvalidOperationException>(notification.Exception);
    }

    [TestMethod]
    public void IteratorScenario_Void()
    {
        var xs = ObservableEx.Create(() => _IteratorScenario_Void(100, 1000));

        xs.AssertEqual(new Unit[] { }.ToObservable());
    }

    private IEnumerable<IObservable<object>> ToIterate_Complete(IObservable<int> xs, IObservable<int> ys, IObservable<int> zs, IObserver<int> observer)
    {
        observer.OnNext(1);
        yield return xs.Select(x => new object());

        observer.OnNext(2);
        yield return ys.Select(x => new object());

        observer.OnNext(3);
        observer.OnCompleted();
        yield return zs.Select(x => new object());

        observer.OnNext(4);
    }

    private IEnumerable<IObservable<object>> ToIterate_Complete_Implicit(IObservable<int> xs, IObservable<int> ys, IObservable<int> zs, IObserver<int> observer)
    {
        observer.OnNext(1);
        yield return xs.Select(x => new object());

        observer.OnNext(2);
        yield return ys.Select(x => new object());

        observer.OnNext(3);
        yield return zs.Select(x => new object());

        observer.OnNext(4);
    }

    private IEnumerable<IObservable<object>> ToIterate_Throw(IObservable<int> xs, IObservable<int> ys, IObservable<int> zs, IObserver<int> observer, Exception ex)
    {
        observer.OnNext(1);
        yield return xs.Select(x => new object());

        observer.OnNext(2);
        yield return ys.Select(x => new object());

        observer.OnNext(3);

        if (xs != null)
        {
            throw ex;
        }

        yield return zs.Select(x => new object());

        observer.OnNext(4);
        observer.OnCompleted();
    }

    private IEnumerable<IObservable<object>> ToIterate_Error(IObservable<int> xs, IObservable<int> ys, IObservable<int> zs, IObserver<int> observer, Exception ex)
    {
        observer.OnNext(1);
        yield return xs.Select(x => new object());

        observer.OnNext(2);
        observer.OnError(ex);

        yield return ys.Select(x => new object());

        observer.OnNext(3);

        yield return zs.Select(x => new object());

        observer.OnNext(4);
        observer.OnCompleted();
    }

    private IEnumerable<IObservable<object>> ToIterate_Complete_Dispose(IObservable<int> xs, IObservable<int> ys, IObservable<int> zs, IObserver<int> observer)
    {
        observer.OnNext(1);
        yield return xs.Select(x => new object());

        observer.OnNext(2);
        yield return ys.Select(x => new object());

        observer.OnNext(3);
        yield return zs.Select(x => new object());

        observer.OnNext(4);
    }

    private static IEnumerable<IObservable<object>> _IteratorScenario(int x, int y, IObserver<int> results)
    {
        var xs = Observable.Range(1, x).ToListObservable();
        yield return xs;

        results.OnNext(xs.Value);

        var ys = Observable.Range(1, y).ToListObservable();
        yield return ys;

        results.OnNext(ys.Value);
    }

    private IEnumerable<IObservable<object>> ToIterate_Void_Complete(IObservable<int> xs, IObservable<int> ys, IObservable<int> zs)
    {
        yield return xs.Select(x => new object());

        yield return ys.Select(x => new object());

        yield return zs.Select(x => new object());
    }

    private IEnumerable<IObservable<object>> ToIterate_Void_Complete_Implicit(IObservable<int> xs, IObservable<int> ys, IObservable<int> zs)
    {
        yield return xs.Select(x => new object());

        yield return ys.Select(x => new object());

        yield return zs.Select(x => new object());
    }

    private IEnumerable<IObservable<object>> ToIterate_Void_Throw(IObservable<int> xs, IObservable<int> ys, IObservable<int> zs, Exception ex)
    {
        yield return xs.Select(x => new object());

        yield return ys.Select(x => new object());

        if (xs != null)
        {
            throw ex;
        }

        yield return zs.Select(x => new object());
    }

    private IEnumerable<IObservable<object>> ToIterate_Void_Complete_Dispose(IObservable<int> xs, IObservable<int> ys, IObservable<int> zs)
    {
        yield return xs.Select(x => new object());

        yield return ys.Select(x => new object());

        yield return zs.Select(x => new object());
    }

    private static IEnumerable<IObservable<object>> _IteratorScenario_Void(int x, int y)
    {
        var xs = Observable.Range(1, x).ToListObservable();
        yield return xs;

        var ys = Observable.Range(1, y).ToListObservable();
        yield return ys;
    }
}

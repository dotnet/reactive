// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information. 

using System.Reactive;
using System.Reactive.Concurrency;
using System.Reactive.Linq;
using System.Reactive.Subjects;

using Microsoft.Reactive.Testing;

using Tests.System.Reactive.Shared.Scenarios;

namespace Tests.System.Reactive.Shared.Rx.Tests;

/// <summary>
/// The shared <c>SubjectTests</c> on Rx.NET, plus the Rx.NET-only originals.
/// </summary>
/// <remarks>
/// The ten originals that stay Rx.NET-only, as they stand but for MSTest's assertions and
/// <c>CurrentThreadScheduler.Instance</c> where the original passes its test project's dummy
/// scheduler or <c>Scheduler.Immediate</c> (names the base class uses for its own members): the
/// four argument-checking tests (the subjects are not in the code-generated stratum, which covers
/// the operator surface); <c>SubjectDisposed</c> and the three <c>HasObservers_Dispose*</c>, which
/// dispose the subject itself; and the two <c>Subject_Synchronize*</c>, which push from ten
/// threads through <c>Subject.Synchronize</c>, an operator AsyncRx.NET does not have.
/// </remarks>
[TestClass]
public sealed class RxSubjectTests : SubjectTests
{
    protected override IRxTarget Target => RxTarget.Instance;

    [TestMethod]
    public void Subscribe_ArgumentChecking()
    {
        Assert.ThrowsExactly<ArgumentNullException>(() => new Subject<int>().Subscribe(null!));
    }

    [TestMethod]
    public void OnError_ArgumentChecking()
    {
        Assert.ThrowsExactly<ArgumentNullException>(() => new Subject<int>().OnError(null!));
    }

    [TestMethod]
    public void SubjectDisposed()
    {
        var scheduler = new TestScheduler();

        var subject = default(Subject<int>);

        var results1 = scheduler.CreateObserver<int>();
        var subscription1 = default(IDisposable);

        var results2 = scheduler.CreateObserver<int>();
        var subscription2 = default(IDisposable);

        var results3 = scheduler.CreateObserver<int>();
        var subscription3 = default(IDisposable);

        scheduler.ScheduleAbsolute(100, () => subject = new Subject<int>());
        scheduler.ScheduleAbsolute(200, () => subscription1 = subject!.Subscribe(results1));
        scheduler.ScheduleAbsolute(300, () => subscription2 = subject!.Subscribe(results2));
        scheduler.ScheduleAbsolute(400, () => subscription3 = subject!.Subscribe(results3));
        scheduler.ScheduleAbsolute(500, () => subscription1!.Dispose());
        scheduler.ScheduleAbsolute(600, () => subject!.Dispose());
        scheduler.ScheduleAbsolute(700, () => subscription2!.Dispose());
        scheduler.ScheduleAbsolute(800, () => subscription3!.Dispose());

        scheduler.ScheduleAbsolute(150, () => subject!.OnNext(1));
        scheduler.ScheduleAbsolute(250, () => subject!.OnNext(2));
        scheduler.ScheduleAbsolute(350, () => subject!.OnNext(3));
        scheduler.ScheduleAbsolute(450, () => subject!.OnNext(4));
        scheduler.ScheduleAbsolute(550, () => subject!.OnNext(5));
        scheduler.ScheduleAbsolute(650, () => Assert.ThrowsExactly<ObjectDisposedException>(() => subject!.OnNext(6)));
        scheduler.ScheduleAbsolute(750, () => Assert.ThrowsExactly<ObjectDisposedException>(() => subject!.OnCompleted()));
        scheduler.ScheduleAbsolute(850, () => Assert.ThrowsExactly<ObjectDisposedException>(() => subject!.OnError(new Exception())));
        scheduler.ScheduleAbsolute(950, () => Assert.ThrowsExactly<ObjectDisposedException>(() => subject!.Subscribe()));

        scheduler.Start();

        results1.Messages.AssertEqual(
            OnNext(250, 2),
            OnNext(350, 3),
            OnNext(450, 4)
        );

        results2.Messages.AssertEqual(
            OnNext(350, 3),
            OnNext(450, 4),
            OnNext(550, 5)
        );

        results3.Messages.AssertEqual(
            OnNext(450, 4),
            OnNext(550, 5)
        );
    }

    [TestMethod]
    public void Subject_Create_ArgumentChecking()
    {
        Assert.ThrowsExactly<ArgumentNullException>(() => Subject.Create<int, int>(null!, Observable.Return(42)));
        Assert.ThrowsExactly<ArgumentNullException>(() => Subject.Create<int, int>(Observer.Create<int>(x => { }), null!));

        Assert.ThrowsExactly<ArgumentNullException>(() => Subject.Create(null!, Observable.Return(42)));
        Assert.ThrowsExactly<ArgumentNullException>(() => Subject.Create(Observer.Create<int>(x => { }), null!));
    }

    [TestMethod]
    public void Subject_Synchronize_ArgumentChecking()
    {
        var s = new Subject<int>();

        Assert.ThrowsExactly<ArgumentNullException>(() => Subject.Synchronize(default(ISubject<int, int>)!));
        Assert.ThrowsExactly<ArgumentNullException>(() => Subject.Synchronize(default(ISubject<int, int>)!, CurrentThreadScheduler.Instance));
        Assert.ThrowsExactly<ArgumentNullException>(() => Subject.Synchronize((ISubject<int, int>)s, null!));

        Assert.ThrowsExactly<ArgumentNullException>(() => Subject.Synchronize(default(ISubject<int>)!));
        Assert.ThrowsExactly<ArgumentNullException>(() => Subject.Synchronize(default(ISubject<int>)!, CurrentThreadScheduler.Instance));
        Assert.ThrowsExactly<ArgumentNullException>(() => Subject.Synchronize(s, null!));
    }

    [TestMethod]
    public void Subject_Synchronize1()
    {
        var N = 10;

        var y = 0;
        var o = Observer.Create<int>(x => y += x);
        var s = Subject.Synchronize(Subject.Create(o, Observable.Empty<string>()));

        var e = new ManualResetEvent(false);
        var t = Enumerable.Range(0, N).Select(x => new Thread(() => { e.WaitOne(); s.OnNext(x); })).ToArray();

        foreach (var u in t)
        {
            u.Start();
        }

        e.Set();

        foreach (var u in t)
        {
            u.Join();
        }

        Assert.AreEqual(Enumerable.Range(0, N).Sum(), y);
    }

    [TestMethod]
    public void Subject_Synchronize2()
    {
        var N = 10;

        var s = Subject.Synchronize(new Subject<int>());

        var y = 0;
        var d = s.Subscribe(x => y += x);

        var e = new ManualResetEvent(false);
        var t = Enumerable.Range(0, N).Select(x => new Thread(() => { e.WaitOne(); s.OnNext(x); })).ToArray();

        foreach (var u in t)
        {
            u.Start();
        }

        e.Set();

        foreach (var u in t)
        {
            u.Join();
        }

        Assert.AreEqual(Enumerable.Range(0, N).Sum(), y);
    }

    [TestMethod]
    public void HasObservers_Dispose1()
    {
        var s = new Subject<int>();
        Assert.IsFalse(s.HasObservers);
        Assert.IsFalse(s.IsDisposed);

        var d = s.Subscribe(_ => { });
        Assert.IsTrue(s.HasObservers);
        Assert.IsFalse(s.IsDisposed);

        s.Dispose();
        Assert.IsFalse(s.HasObservers);
        Assert.IsTrue(s.IsDisposed);

        d.Dispose();
        Assert.IsFalse(s.HasObservers);
        Assert.IsTrue(s.IsDisposed);
    }

    [TestMethod]
    public void HasObservers_Dispose2()
    {
        var s = new Subject<int>();
        Assert.IsFalse(s.HasObservers);
        Assert.IsFalse(s.IsDisposed);

        var d = s.Subscribe(_ => { });
        Assert.IsTrue(s.HasObservers);
        Assert.IsFalse(s.IsDisposed);

        d.Dispose();
        Assert.IsFalse(s.HasObservers);
        Assert.IsFalse(s.IsDisposed);

        s.Dispose();
        Assert.IsFalse(s.HasObservers);
        Assert.IsTrue(s.IsDisposed);
    }

    [TestMethod]
    public void HasObservers_Dispose3()
    {
        var s = new Subject<int>();
        Assert.IsFalse(s.HasObservers);
        Assert.IsFalse(s.IsDisposed);

        s.Dispose();
        Assert.IsFalse(s.HasObservers);
        Assert.IsTrue(s.IsDisposed);
    }
}

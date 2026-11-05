// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information. 

using System.Reactive.Concurrency;
using System.Reactive.Subjects;

using Microsoft.Reactive.Testing;

using Tests.System.Reactive.Shared.Scenarios;

namespace Tests.System.Reactive.Shared.Rx.Tests;

/// <summary>
/// The shared <c>BehaviorSubjectTests</c> on Rx.NET, plus the Rx.NET-only originals.
/// </summary>
/// <remarks>
/// The seven originals that stay Rx.NET-only, as they stand but for MSTest's assertions: the two
/// argument-checking tests (the subjects are not in the code-generated stratum), and the five that
/// dispose the subject itself.
/// </remarks>
[TestClass]
public sealed class RxBehaviorSubjectTests : BehaviorSubjectTests
{
    protected override IRxTarget Target => RxTarget.Instance;

    [TestMethod]
    public void Subscribe_ArgumentChecking()
    {
        Assert.ThrowsExactly<ArgumentNullException>(() => new BehaviorSubject<int>(1).Subscribe(null!));
    }

    [TestMethod]
    public void OnError_ArgumentChecking()
    {
        Assert.ThrowsExactly<ArgumentNullException>(() => new BehaviorSubject<int>(1).OnError(null!));
    }

    [TestMethod]
    public void SubjectDisposed()
    {
        var scheduler = new TestScheduler();

        var subject = default(BehaviorSubject<int>);

        var results1 = scheduler.CreateObserver<int>();
        var subscription1 = default(IDisposable);

        var results2 = scheduler.CreateObserver<int>();
        var subscription2 = default(IDisposable);

        var results3 = scheduler.CreateObserver<int>();
        var subscription3 = default(IDisposable);

        scheduler.ScheduleAbsolute(100, () => subject = new BehaviorSubject<int>(0));
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
            OnNext(200, 1),
            OnNext(250, 2),
            OnNext(350, 3),
            OnNext(450, 4)
        );

        results2.Messages.AssertEqual(
            OnNext(300, 2),
            OnNext(350, 3),
            OnNext(450, 4),
            OnNext(550, 5)
        );

        results3.Messages.AssertEqual(
            OnNext(400, 3),
            OnNext(450, 4),
            OnNext(550, 5)
        );
    }

    [TestMethod]
    public void HasObservers_Dispose1()
    {
        var s = new BehaviorSubject<int>(42);
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
        var s = new BehaviorSubject<int>(42);
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
        var s = new BehaviorSubject<int>(42);
        Assert.IsFalse(s.HasObservers);
        Assert.IsFalse(s.IsDisposed);

        s.Dispose();
        Assert.IsFalse(s.HasObservers);
        Assert.IsTrue(s.IsDisposed);
    }

    [TestMethod]
    public void Value_ThrowsOnDispose()
    {
        var s = new BehaviorSubject<int>(42);
        Assert.AreEqual(42, s.Value);

        s.Dispose();

        Assert.ThrowsExactly<ObjectDisposedException>(() =>
        {
            var ignored = s.Value;
        });

        Assert.IsFalse(s.TryGetValue(out var x));
    }
}

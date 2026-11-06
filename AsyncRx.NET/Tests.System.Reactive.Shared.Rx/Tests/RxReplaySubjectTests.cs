// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information. 

using System.Reactive.Concurrency;
using System.Reactive.Subjects;

using Microsoft.Reactive.Testing;

using Tests.System.Reactive.Shared.Scenarios;

namespace Tests.System.Reactive.Shared.Rx.Tests;

/// <summary>
/// The shared <c>ReplaySubjectTests</c> on Rx.NET, plus the Rx.NET-only originals.
/// </summary>
/// <remarks>
/// The eleven originals that stay Rx.NET-only, as they stand but for MSTest's assertions and
/// <c>CurrentThreadScheduler.Instance</c> where the original passes its test project's dummy
/// scheduler: the three argument-checking tests (the subjects are not in the code-generated
/// stratum); the seven that dispose the subject itself; and <c>ReplaySubject_Reentrant</c>, whose
/// handler pushes back into the subject delivering to it, which AsyncRx.NET's observers refuse
/// (Step 81). The six <c>FastImmediateObserver_*</c> originals are over an internal type and are
/// not here.
/// </remarks>
[TestClass]
public sealed class RxReplaySubjectTests : ReplaySubjectTests
{
    protected override IRxTarget Target => RxTarget.Instance;

    [TestMethod]
    public void Subscribe_ArgumentChecking()
    {
        Assert.ThrowsExactly<ArgumentNullException>(() => new ReplaySubject<int>().Subscribe(null!));
        Assert.ThrowsExactly<ArgumentNullException>(() => new ReplaySubject<int>(1).Subscribe(null!));
        Assert.ThrowsExactly<ArgumentNullException>(() => new ReplaySubject<int>(2).Subscribe(null!));
        Assert.ThrowsExactly<ArgumentNullException>(() => new ReplaySubject<int>(CurrentThreadScheduler.Instance).Subscribe(null!));
    }

    [TestMethod]
    public void OnError_ArgumentChecking()
    {
        Assert.ThrowsExactly<ArgumentNullException>(() => new ReplaySubject<int>().OnError(null!));
        Assert.ThrowsExactly<ArgumentNullException>(() => new ReplaySubject<int>(1).OnError(null!));
        Assert.ThrowsExactly<ArgumentNullException>(() => new ReplaySubject<int>(2).OnError(null!));
        Assert.ThrowsExactly<ArgumentNullException>(() => new ReplaySubject<int>(CurrentThreadScheduler.Instance).OnError(null!));
    }

    [TestMethod]
    public void Constructor_ArgumentChecking()
    {
#pragma warning disable CA1806 // (Unused new instance.) We are just testing whether or not the constructor throws.
        Assert.ThrowsExactly<ArgumentOutOfRangeException>(() => new ReplaySubject<int>(-1));
        Assert.ThrowsExactly<ArgumentOutOfRangeException>(() => new ReplaySubject<int>(-1, CurrentThreadScheduler.Instance));
        Assert.ThrowsExactly<ArgumentOutOfRangeException>(() => new ReplaySubject<int>(-1, TimeSpan.Zero));
        Assert.ThrowsExactly<ArgumentOutOfRangeException>(() => new ReplaySubject<int>(-1, TimeSpan.Zero, CurrentThreadScheduler.Instance));

        Assert.ThrowsExactly<ArgumentOutOfRangeException>(() => new ReplaySubject<int>(TimeSpan.FromTicks(-1)));
        Assert.ThrowsExactly<ArgumentOutOfRangeException>(() => new ReplaySubject<int>(TimeSpan.FromTicks(-1), CurrentThreadScheduler.Instance));
        Assert.ThrowsExactly<ArgumentOutOfRangeException>(() => new ReplaySubject<int>(0, TimeSpan.FromTicks(-1)));
        Assert.ThrowsExactly<ArgumentOutOfRangeException>(() => new ReplaySubject<int>(0, TimeSpan.FromTicks(-1), CurrentThreadScheduler.Instance));

        Assert.ThrowsExactly<ArgumentNullException>(() => new ReplaySubject<int>(null!));
        Assert.ThrowsExactly<ArgumentNullException>(() => new ReplaySubject<int>(0, null!));
        Assert.ThrowsExactly<ArgumentNullException>(() => new ReplaySubject<int>(TimeSpan.Zero, null!));
        Assert.ThrowsExactly<ArgumentNullException>(() => new ReplaySubject<int>(0, TimeSpan.Zero, null!));

        // zero allowed
        new ReplaySubject<int>(0);
        new ReplaySubject<int>(TimeSpan.Zero);
        new ReplaySubject<int>(0, TimeSpan.Zero);
        new ReplaySubject<int>(0, CurrentThreadScheduler.Instance);
        new ReplaySubject<int>(TimeSpan.Zero, CurrentThreadScheduler.Instance);
        new ReplaySubject<int>(0, TimeSpan.Zero, CurrentThreadScheduler.Instance);
#pragma warning restore CA1806
    }

    [TestMethod]
    public void SubjectDisposed()
    {
        var scheduler = new TestScheduler();

        var subject = default(ReplaySubject<int>);

        var results1 = scheduler.CreateObserver<int>();
        var subscription1 = default(IDisposable);

        var results2 = scheduler.CreateObserver<int>();
        var subscription2 = default(IDisposable);

        var results3 = scheduler.CreateObserver<int>();
        var subscription3 = default(IDisposable);

        scheduler.ScheduleAbsolute(100, () => subject = new ReplaySubject<int>(scheduler));
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
            OnNext(201, 1),
            OnNext(251, 2),
            OnNext(351, 3),
            OnNext(451, 4)
        );

        results2.Messages.AssertEqual(
            OnNext(301, 1),
            OnNext(302, 2),
            OnNext(351, 3),
            OnNext(451, 4),
            OnNext(551, 5)
        );

        results3.Messages.AssertEqual(
            OnNext(401, 1),
            OnNext(402, 2),
            OnNext(403, 3),
            OnNext(451, 4),
            OnNext(551, 5)
        );
    }

    [TestMethod]
    public void SubjectDisposed_ReplayOne()
    {
        var scheduler = new TestScheduler();

        var subject = default(ReplaySubject<int>);

        var results1 = scheduler.CreateObserver<int>();
        var subscription1 = default(IDisposable);

        var results2 = scheduler.CreateObserver<int>();
        var subscription2 = default(IDisposable);

        var results3 = scheduler.CreateObserver<int>();
        var subscription3 = default(IDisposable);

        scheduler.ScheduleAbsolute(100, () => subject = new ReplaySubject<int>(1));
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
    public void SubjectDisposed_ReplayMany()
    {
        var scheduler = new TestScheduler();

        var subject = default(ReplaySubject<int>);

        var results1 = scheduler.CreateObserver<int>();
        var subscription1 = default(IDisposable);

        var results2 = scheduler.CreateObserver<int>();
        var subscription2 = default(IDisposable);

        var results3 = scheduler.CreateObserver<int>();
        var subscription3 = default(IDisposable);

        scheduler.ScheduleAbsolute(100, () => subject = new ReplaySubject<int>(3));
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
            OnNext(300, 1),
            OnNext(300, 2),
            OnNext(350, 3),
            OnNext(450, 4),
            OnNext(550, 5)
        );

        results3.Messages.AssertEqual(
            OnNext(400, 1),
            OnNext(400, 2),
            OnNext(400, 3),
            OnNext(450, 4),
            OnNext(550, 5)
        );
    }

    [TestMethod]
    public void SubjectDisposed_ReplayAll()
    {
        var scheduler = new TestScheduler();

        var subject = default(ReplaySubject<int>);

        var results1 = scheduler.CreateObserver<int>();
        var subscription1 = default(IDisposable);

        var results2 = scheduler.CreateObserver<int>();
        var subscription2 = default(IDisposable);

        var results3 = scheduler.CreateObserver<int>();
        var subscription3 = default(IDisposable);

        scheduler.ScheduleAbsolute(100, () => subject = new ReplaySubject<int>());
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
            OnNext(300, 1),
            OnNext(300, 2),
            OnNext(350, 3),
            OnNext(450, 4),
            OnNext(550, 5)
        );

        results3.Messages.AssertEqual(
            OnNext(400, 1),
            OnNext(400, 2),
            OnNext(400, 3),
            OnNext(450, 4),
            OnNext(550, 5)
        );
    }

    [TestMethod]
    public void HasObservers_Dispose1()
    {
        HasObservers_Dispose1Impl(new ReplaySubject<int>());
        HasObservers_Dispose1Impl(new ReplaySubject<int>(1));
        HasObservers_Dispose1Impl(new ReplaySubject<int>(3));
        HasObservers_Dispose1Impl(new ReplaySubject<int>(TimeSpan.FromSeconds(1)));
    }

    [TestMethod]
    public void HasObservers_Dispose2()
    {
        HasObservers_Dispose2Impl(new ReplaySubject<int>());
        HasObservers_Dispose2Impl(new ReplaySubject<int>(1));
        HasObservers_Dispose2Impl(new ReplaySubject<int>(3));
        HasObservers_Dispose2Impl(new ReplaySubject<int>(TimeSpan.FromSeconds(1)));
    }

    [TestMethod]
    public void HasObservers_Dispose3()
    {
        HasObservers_Dispose3Impl(new ReplaySubject<int>());
        HasObservers_Dispose3Impl(new ReplaySubject<int>(1));
        HasObservers_Dispose3Impl(new ReplaySubject<int>(3));
        HasObservers_Dispose3Impl(new ReplaySubject<int>(TimeSpan.FromSeconds(1)));
    }

    [TestMethod]
    public void ReplaySubject_Reentrant()
    {
        var r = new ReplaySubject<int>(4);

        r.OnNext(0);
        r.OnNext(1);
        r.OnNext(2);
        r.OnNext(3);
        r.OnNext(4);

        var xs = new List<int>();

        var i = 0;
        r.Subscribe(x =>
        {
            xs.Add(x);

            if (++i <= 10)
            {
                r.OnNext(x);
            }
        });

        r.OnNext(5);

        Assert.IsTrue(xs.SequenceEqual(
        [
            1, 2, 3, 4, // original
            1, 2, 3, 4, // reentrant (+ fed back)
            1, 2, 3, 4, // reentrant (+ first two fed back)
            1, 2,       // reentrant
            5           // tune in
        ]));
    }


    private static void HasObservers_Dispose1Impl(ReplaySubject<int> s)
    {
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


    private static void HasObservers_Dispose2Impl(ReplaySubject<int> s)
    {
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


    private static void HasObservers_Dispose3Impl(ReplaySubject<int> s)
    {
        Assert.IsFalse(s.HasObservers);
        Assert.IsFalse(s.IsDisposed);

        s.Dispose();
        Assert.IsFalse(s.HasObservers);
        Assert.IsTrue(s.IsDisposed);
    }
}

// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information. 

using System.Reactive.Concurrency;
using System.Reactive.Subjects;

using Microsoft.Reactive.Testing;

using Tests.System.Reactive.Shared.Scenarios;

namespace Tests.System.Reactive.Shared.Rx.Tests;

/// <summary>
/// The shared <c>AsyncSubjectTests</c> on Rx.NET, plus the Rx.NET-only originals.
/// </summary>
/// <remarks>
/// The eleven originals that stay Rx.NET-only, as they stand but for MSTest's assertions: the two
/// argument-checking tests (the subjects are not in the code-generated stratum);
/// <c>SubjectDisposed</c> and the three <c>HasObservers_Dispose*</c>, which dispose the subject
/// itself; and the five that block a thread in <c>GetResult</c> or register the awaiter under a
/// synchronization context, whose claim in the awaitable shape is shared.
/// </remarks>
[TestClass]
public sealed class RxAsyncSubjectTests : AsyncSubjectTests
{
    protected override IRxTarget Target => RxTarget.Instance;

    [TestMethod]
    public void Subscribe_ArgumentChecking()
    {
        Assert.ThrowsExactly<ArgumentNullException>(() => new AsyncSubject<int>().Subscribe(null!));
    }

    [TestMethod]
    public void OnError_ArgumentChecking()
    {
        Assert.ThrowsExactly<ArgumentNullException>(() => new AsyncSubject<int>().OnError(null!));
    }

    [TestMethod]
    public void SubjectDisposed()
    {
        var scheduler = new TestScheduler();

        var subject = default(AsyncSubject<int>);

        var results1 = scheduler.CreateObserver<int>();
        var subscription1 = default(IDisposable);

        var results2 = scheduler.CreateObserver<int>();
        var subscription2 = default(IDisposable);

        var results3 = scheduler.CreateObserver<int>();
        var subscription3 = default(IDisposable);

        scheduler.ScheduleAbsolute(100, () => subject = new AsyncSubject<int>());
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
        );

        results2.Messages.AssertEqual(
        );

        results3.Messages.AssertEqual(
        );
    }

    [TestMethod]
    public void Await_Blocking()
    {
        var s = new AsyncSubject<int>();
        GetResult_BlockingImpl(s.GetAwaiter());
    }

    [TestMethod]
    public void Await_Throw()
    {
        var s = new AsyncSubject<int>();
        GetResult_Blocking_ThrowImpl(s.GetAwaiter());
    }

    [TestMethod]
    public void GetResult_Blocking()
    {
        GetResult_BlockingImpl(new AsyncSubject<int>());
    }

    [TestMethod]
    public void GetResult_Blocking_Throw()
    {
        GetResult_Blocking_ThrowImpl(new AsyncSubject<int>());
    }

    [TestMethod]
    public void GetResult_Context()
    {
        var x = new AsyncSubject<int>();

        var ctx = new MyContext();
        var e = new ManualResetEvent(false);

        Task.Run(() =>
        {
            SynchronizationContext.SetSynchronizationContext(ctx);

            var a = x.GetAwaiter();
            a.OnCompleted(() =>
            {
                e.Set();
            });
        });

        x.OnNext(42);
        x.OnCompleted();

        e.WaitOne();

        Assert.IsTrue(ctx.Ran);
    }

    [TestMethod]
    public void HasObservers_Dispose1()
    {
        var s = new AsyncSubject<int>();
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
        var s = new AsyncSubject<int>();
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
        var s = new AsyncSubject<int>();
        Assert.IsFalse(s.HasObservers);
        Assert.IsFalse(s.IsDisposed);

        s.Dispose();
        Assert.IsFalse(s.HasObservers);
        Assert.IsTrue(s.IsDisposed);
    }


    private void GetResult_BlockingImpl(AsyncSubject<int> s)
    {
        Assert.IsFalse(s.IsCompleted);

        var e = new ManualResetEvent(false);

        new Thread(() => { e.WaitOne(); s.OnNext(42); s.OnCompleted(); }).Start();

        var y = default(int);
        var t = new Thread(() => { y = s.GetResult(); });
        t.Start();

        while (t.ThreadState != ThreadState.WaitSleepJoin)
        {
            ;
        }

        e.Set();
        t.Join();

        Assert.AreEqual(42, y);
        Assert.IsTrue(s.IsCompleted);
    }


    private void GetResult_Blocking_ThrowImpl(AsyncSubject<int> s)
    {
        Assert.IsFalse(s.IsCompleted);

        var e = new ManualResetEvent(false);

        var ex = new Exception();

        new Thread(() => { e.WaitOne(); s.OnError(ex); }).Start();

        var y = default(Exception);
        var t = new Thread(() =>
        {
            try
            {
                s.GetResult();
            }
            catch (Exception ex_)
            {
                y = ex_;
            }
        });
        t.Start();

        while (t.ThreadState != ThreadState.WaitSleepJoin)
        {
            ;
        }

        e.Set();
        t.Join();

        Assert.AreSame(ex, y);
        Assert.IsTrue(s.IsCompleted);
    }

    private class MyContext : SynchronizationContext
    {
        public bool Ran;

        public override void Post(SendOrPostCallback d, object? state)
        {
            Ran = true;
            d(state);
        }
    }
}

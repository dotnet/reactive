// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information. 

using System.Reactive.Concurrency;
using System.Reactive.Linq;
using System.Reactive.Subjects;

using Tests.System.Reactive.Shared.Scenarios;

namespace Tests.System.Reactive.Shared.Rx.Tests;

/// <summary>
/// The shared <c>ObserveOn</c> scenarios on Rx.NET, plus nine over its own schedulers.
/// </summary>
/// <remarks>
/// <c>ObserveOn_EventLoop_Long</c>, <c>ObserveOn_LongRunning_SameThread</c> and
/// <c>ObserveOn_LongRunning_DisableOptimizations</c> are the originals: they exercise
/// <c>EventLoopScheduler</c> and the task pool scheduler's <c>ISchedulerLongRunning</c> path,
/// with and without <c>DisableOptimizations</c>, none of which AsyncRx.NET has, so they are
/// written against Rx.NET directly. The six <c>ObserveOn_LongRunning_*</c> tests are the
/// originals too, over Rx.NET's own <see cref="RxTestLongRunningScheduler"/>, as they stand but
/// for MSTest's assertions: what they claim about the long-running dispatch loop being one unit
/// of work, started once and stopped once, is Rx.NET's long-running optimization, which
/// AsyncRx.NET does not have; the claims they make that hold on any scheduler of its own threads
/// are the shared <c>ObserveOn_RealScheduler_*</c> six.
/// </remarks>
[TestClass]
public sealed class RxObserveOnTests : ObserveOnTests
{
    private static readonly TimeSpan MaxWaitTime = TimeSpan.FromSeconds(10);

    protected override IRxTarget Target => RxTarget.Instance;

    [TestMethod]
    public void ObserveOn_EventLoop_Long()
    {
        using var scheduler1 = new EventLoopScheduler();
        var n = 1_000_000;
        var cde = new CountdownEvent(1);

        Observable.Range(1, n).ObserveOn(scheduler1)
            .Subscribe(v => { }, () => cde.Signal());

        Assert.IsTrue(cde.Wait(MaxWaitTime), "Timeout!");
    }

    [TestMethod]
    public void ObserveOn_LongRunning_SameThread()
    {
        var scheduler = TaskPoolScheduler.Default;
        Assert.IsNotNull(scheduler.AsLongRunning());

        var n = 1_000_000;
        var threads = new HashSet<long>();
        var cde = new CountdownEvent(1);

        Observable.Range(1, n)
            .ObserveOn(scheduler)
            .Subscribe(
                v => threads.Add(Environment.CurrentManagedThreadId),
                e => cde.Signal(),
                () => cde.Signal()
            );

        Assert.IsTrue(cde.Wait(MaxWaitTime), "Timeout!");
        Assert.HasCount(1, threads);
    }

    [TestMethod]
    public void ObserveOn_LongRunning_DisableOptimizations()
    {
        var scheduler = TaskPoolScheduler.Default.DisableOptimizations();
        Assert.IsNull(scheduler.AsLongRunning());

        var n = 1_000_000;
        var threads = new HashSet<long>();
        var cde = new CountdownEvent(1);

        Observable.Range(1, n)
            .ObserveOn(scheduler)
            .Subscribe(
                v => threads.Add(Environment.CurrentManagedThreadId),
                e => cde.Signal(),
                () => cde.Signal()
            );

        Assert.IsTrue(cde.Wait(MaxWaitTime), "Timeout!");
        Assert.IsTrue(threads.Count >= 1);
    }

    [TestMethod]
    public void ObserveOn_LongRunning_Simple()
    {
        var started = default(ManualResetEvent);
        var stopped = default(ManualResetEvent);

        var scheduler = new RxTestLongRunningScheduler(e => started = e, e => stopped = e);

        var s = new Subject<int>();

        var end = new ManualResetEvent(false);
        var lst = new List<int>();
        s.ObserveOn(scheduler).Subscribe(lst.Add, () => end.Set());

        s.OnNext(1);
        s.OnNext(2);
        s.OnNext(3);
        s.OnCompleted();

        end.WaitOne();

        Assert.IsTrue(lst.SequenceEqual([1, 2, 3]));
    }

    [TestMethod]
    public void ObserveOn_LongRunning_Error()
    {
        var started = default(ManualResetEvent);
        var stopped = default(ManualResetEvent);

        var scheduler = new RxTestLongRunningScheduler(e => started = e, e => stopped = e);

        var s = new Subject<int>();

        var end = new ManualResetEvent(false);
        var err = default(Exception);
        s.ObserveOn(scheduler).Subscribe(_ => { }, ex => { err = ex; end.Set(); });

        s.OnNext(1);
        s.OnNext(2);
        s.OnNext(3);
        var ex_ = new Exception();
        s.OnError(ex_);

        end.WaitOne();

        Assert.AreSame(ex_, err);
    }

    [TestMethod]
    public void ObserveOn_LongRunning_TimeVariance()
    {
        var started = default(ManualResetEvent);
        var stopped = default(ManualResetEvent);

        var scheduler = new RxTestLongRunningScheduler(e => started = e, e => stopped = e);

        var s = new Subject<int>();

        var end = new ManualResetEvent(false);
        s.ObserveOn(scheduler).Subscribe(_ => { }, () => end.Set());

        s.OnNext(1); // Ensure active
        started!.WaitOne();

        Thread.Sleep(100); // Try to enter the dispatcher event wait state

        for (var i = 0; i < 1000; i++)
        {
            if (i % 100 == 0)
            {
                Thread.Sleep(10);
            }

            s.OnNext(i);
        }

        s.OnCompleted();

        end.WaitOne();
    }

    [TestMethod]
    public void ObserveOn_LongRunning_HoldUpDuringDispatchAndFail()
    {
        var started = default(ManualResetEvent);
        var stopped = default(ManualResetEvent);

        var scheduler = new RxTestLongRunningScheduler(e => started = e, e => stopped = e);

        var s = new Subject<int>();

        var onNext = new ManualResetEvent(false);
        var resume = new ManualResetEvent(false);
        var lst = new List<int>();
        var err = default(Exception);
        var end = new ManualResetEvent(false);
        s.ObserveOn(scheduler).Subscribe(x => { lst.Add(x); onNext.Set(); resume.WaitOne(); }, ex_ => { err = ex_; end.Set(); });

        s.OnNext(1);
        onNext.WaitOne();

        s.OnNext(2);
        s.OnNext(3);
        var ex = new Exception();
        s.OnError(ex);

        resume.Set();

        end.WaitOne();

        Assert.IsTrue(lst.SequenceEqual([1, 2, 3]));
        Assert.AreSame(ex, err);
    }

    [TestMethod]
    public void ObserveOn_LongRunning_Cancel()
    {
        var started = default(ManualResetEvent);
        var stopped = default(ManualResetEvent);

        var scheduler = new RxTestLongRunningScheduler(e => started = e, e => stopped = e);

        var s = new Subject<int>();

        var lst = new List<int>();
        var end = new ManualResetEvent(false);
        var running = new ManualResetEvent(false);
        var d = s.ObserveOn(scheduler).Subscribe(x => { lst.Add(x); running.Set(); });

        s.OnNext(0);
        started!.WaitOne();

        s.OnNext(1);
        s.OnNext(2);
        s.OnNext(3);

        running.WaitOne();
        d.Dispose();
        stopped!.WaitOne();

        s.OnNext(4);

        Assert.IsTrue(lst.Count > 0 && !lst.Contains(4));
    }

    [TestMethod]
    public void ObserveOn_LongRunning_OnNextThrows()
    {
        var started = default(ManualResetEvent);
        var stopped = default(ManualResetEvent);
        var exception = default(Exception);

        var scheduler = new RxTestLongRunningScheduler(
            e => started = e, e => stopped = e, ex => exception = ex);

        var s = new Subject<int>();

        var lst = new List<int>();
        var end = new ManualResetEvent(false);
        var running = new ManualResetEvent(false);
        var d = s.ObserveOn(scheduler).Subscribe(x => { lst.Add(x); running.Set(); if (x == 3) { throw new Exception(); } });

        s.OnNext(0);
        started!.WaitOne();

        s.OnNext(1);
        s.OnNext(2);
        s.OnNext(3);

        running.WaitOne();
        s.OnNext(4);

        stopped!.WaitOne();

        Assert.IsNotNull(exception);
    }
}

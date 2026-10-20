// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information. 

using System.Reactive.Disposables;

namespace Tests.System.Reactive.Shared.Scenarios;

/// <summary>Shared <c>RunAsync</c> scenarios, from Rx.NET's <c>RunAsyncTest.cs</c>.</summary>
/// <remarks>
/// 6 of the file's 7 tests (<c>RunAsync_ArgumentChecking</c> is the code-generated stratum).
/// The terminal is called as <c>RunAsync(xs, token)</c> and returns the target's awaiter as a
/// <see cref="SeqAwaiter{T}"/>, on which the scenarios register their continuation and read the
/// result exactly as the originals do on the <c>AsyncSubject</c>. The connectable scenarios
/// build their source with <c>Seq.Create</c> over two scheduled notifications whose
/// cancellations the subscription's disposal runs (the <c>Action</c> form of <c>Create</c>,
/// over the original's <c>StableCompositeDisposable</c>). Each test clears the synchronization
/// context, static state, so the class is <c>[DoNotParallelize]</c>d test by test.
/// </remarks>
public abstract class RunAsyncTests : SharedReactiveTest
{
    [TestMethod]
    [DoNotParallelize]
    public void RunAsync_Simple()
    {
        SynchronizationContext.SetSynchronizationContext(null);
        var xs = Scheduler.CreateHotObservable(
            OnNext(220, 42),
            OnCompleted<int>(250)
        );

        var awaiter = default(SeqAwaiter<int>);
        var result = default(int);
        var t = long.MaxValue;

        Scheduler.ScheduleAbsolute(100, () => awaiter = RunAsync(xs, CancellationToken.None));
        Scheduler.ScheduleAbsolute(200, () => awaiter!.OnCompleted(() => { t = Scheduler.Clock; result = awaiter!.GetResult(); }));

        Scheduler.Start();

        Assert.AreEqual(250, t);
        Assert.AreEqual(42, result);

        xs.Subscriptions.AssertEqual(
            Subscribe(100)
        );
    }

    [TestMethod]
    [DoNotParallelize]
    public void RunAsync_Cancelled()
    {
        SynchronizationContext.SetSynchronizationContext(null);

        var cts = new CancellationTokenSource();
        cts.Cancel();
        var xs = Scheduler.CreateHotObservable(
            OnNext(220, 42),
            OnCompleted<int>(250)
        );

        var awaiter = default(SeqAwaiter<int>);
        var result = default(int);
        var t = long.MaxValue;

        Scheduler.ScheduleAbsolute(100, () => awaiter = RunAsync(xs, cts.Token));
        Scheduler.ScheduleAbsolute(200, () => awaiter!.OnCompleted(() =>
        {
            t = Scheduler.Clock;

            Assert.ThrowsExactly<OperationCanceledException>(() =>
            {
                result = awaiter!.GetResult();
            });
        }));

        Scheduler.Start();

        Assert.AreEqual(200, t);

        xs.Subscriptions.AssertEqual(
        );
    }

    [TestMethod]
    [DoNotParallelize]
    public void RunAsync_Cancel()
    {
        SynchronizationContext.SetSynchronizationContext(null);

        var cts = new CancellationTokenSource();
        var xs = Scheduler.CreateHotObservable(
            OnNext(220, 42),
            OnCompleted<int>(250)
        );

        var awaiter = default(SeqAwaiter<int>);
        var result = default(int);
        var t = long.MaxValue;

        Scheduler.ScheduleAbsolute(100, () => awaiter = RunAsync(xs, cts.Token));
        Scheduler.ScheduleAbsolute(200, () => awaiter!.OnCompleted(() =>
        {
            t = Scheduler.Clock;

            Assert.ThrowsExactly<OperationCanceledException>(() =>
            {
                result = awaiter!.GetResult();
            });
        }));
        Scheduler.ScheduleAbsolute(210, () => cts.Cancel());

        Scheduler.Start();

        Assert.AreEqual(210, t);

        xs.Subscriptions.AssertEqual(
            Subscribe(100, 210)
        );
    }

    [TestMethod]
    [DoNotParallelize]
    public void RunAsync_Connectable()
    {
        SynchronizationContext.SetSynchronizationContext(null);
        var s = default(long);

        var xs = Seq.Create<int>(observer =>
        {
            s = Scheduler.Clock;

            return ValueTask.FromResult<Action?>(StableCompositeDisposable.Create(
                Scheduler.ScheduleAbsolute(250, () => observer.OnNextAsync(42)),
                Scheduler.ScheduleAbsolute(260, () => observer.OnCompletedAsync())
            ).Dispose);
        });

        var ys = xs.Publish();

        var awaiter = default(SeqAwaiter<int>);
        var result = default(int);
        var t = long.MaxValue;

        Scheduler.ScheduleAbsolute(100, () => awaiter = RunAsync(ys, CancellationToken.None));
        Scheduler.ScheduleAbsolute(200, () => awaiter!.OnCompleted(() => { t = Scheduler.Clock; result = awaiter!.GetResult(); }));

        Scheduler.Start();

        Assert.AreEqual(100, s);
        Assert.AreEqual(260, t);
        Assert.AreEqual(42, result);
    }

    [TestMethod]
    [DoNotParallelize]
    public void RunAsync_Connectable_Cancelled()
    {
        SynchronizationContext.SetSynchronizationContext(null);

        var cts = new CancellationTokenSource();
        cts.Cancel();
        var s = default(long?);

        var xs = Seq.Create<int>(observer =>
        {
            s = Scheduler.Clock;

            return ValueTask.FromResult<Action?>(StableCompositeDisposable.Create(
                Scheduler.ScheduleAbsolute(250, () => observer.OnNextAsync(42)),
                Scheduler.ScheduleAbsolute(260, () => observer.OnCompletedAsync())
            ).Dispose);
        });

        var ys = xs.Publish();

        var awaiter = default(SeqAwaiter<int>);
        var result = default(int);
        var t = long.MaxValue;

        Scheduler.ScheduleAbsolute(100, () => awaiter = RunAsync(ys, cts.Token));
        Scheduler.ScheduleAbsolute(200, () => awaiter!.OnCompleted(() =>
        {
            t = Scheduler.Clock;

            Assert.ThrowsExactly<OperationCanceledException>(() =>
            {
                result = awaiter!.GetResult();
            });
        }));

        Scheduler.Start();

        Assert.IsFalse(s.HasValue);
        Assert.AreEqual(200, t);
    }

    [TestMethod]
    [DoNotParallelize]
    public void RunAsync_Connectable_Cancel()
    {
        SynchronizationContext.SetSynchronizationContext(null);

        var cts = new CancellationTokenSource();
        var s = default(long);
        var d = default(long);

        var xs = Seq.Create<int>(observer =>
        {
            s = Scheduler.Clock;

            return ValueTask.FromResult<Action?>(StableCompositeDisposable.Create(
                Scheduler.ScheduleAbsolute(250, () => observer.OnNextAsync(42)),
                Scheduler.ScheduleAbsolute(260, () => observer.OnCompletedAsync()),
                Disposable.Create(() => { d = Scheduler.Clock; })
            ).Dispose);
        });

        var ys = xs.Publish();

        var awaiter = default(SeqAwaiter<int>);
        var result = default(int);
        var t = long.MaxValue;

        Scheduler.ScheduleAbsolute(100, () => awaiter = RunAsync(ys, cts.Token));
        Scheduler.ScheduleAbsolute(200, () => awaiter!.OnCompleted(() =>
        {
            t = Scheduler.Clock;

            Assert.ThrowsExactly<OperationCanceledException>(() =>
            {
                result = awaiter!.GetResult();
            });
        }));
        Scheduler.ScheduleAbsolute(210, () => cts.Cancel());

        Scheduler.Start();

        Assert.AreEqual(100, s);
        Assert.AreEqual(210, d);
        Assert.AreEqual(210, t);
    }
}

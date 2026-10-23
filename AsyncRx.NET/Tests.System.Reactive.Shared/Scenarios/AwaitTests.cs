// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information. 

using System.Reactive.Disposables;

namespace Tests.System.Reactive.Shared.Scenarios;

/// <summary>Shared <c>GetAwaiter</c> scenarios, from Rx.NET's <c>AwaitTest.cs</c>.</summary>
/// <remarks>
/// 5 of the file's 6 tests (<c>Await_ArgumentChecking</c> is the code-generated stratum),
/// transcribed as the <c>RunAsync</c> scenarios are: <c>GetAwaiter(xs)</c> returns the target's
/// awaiter as a <see cref="SeqAwaiter{T}"/>, and the continuation, the result and the errors
/// it throws are read as the originals read them.
/// </remarks>
public abstract class AwaitTests : SharedReactiveTest
{
    [TestMethod]
    [DoNotParallelize]
    public void Await()
    {
        SynchronizationContext.SetSynchronizationContext(null);
        var xs = Scheduler.CreateHotObservable(
            OnNext(20, -1),
            OnNext(150, 0),
            OnNext(220, 1),
            OnNext(290, 2),
            OnNext(340, 3),
            OnCompleted<int>(410)
        );

        var awaiter = default(SeqAwaiter<int>);
        var result = default(int);
        var t = long.MaxValue;

        Scheduler.ScheduleAbsolute(100, () => awaiter = GetAwaiter(xs));
        Scheduler.ScheduleAbsolute(200, () => awaiter!.OnCompleted(() => { t = Scheduler.Clock; result = awaiter!.GetResult(); }));

        Scheduler.Start();

        Assert.AreEqual(410, t);
        Assert.AreEqual(3, result);

        xs.Subscriptions.AssertEqual(
            Subscribe(100)
        );
    }

    [TestMethod]
    [DoNotParallelize]
    public void Await_Connectable()
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

        Scheduler.ScheduleAbsolute(100, () => awaiter = GetAwaiter(ys));
        Scheduler.ScheduleAbsolute(200, () => awaiter!.OnCompleted(() => { t = Scheduler.Clock; result = awaiter!.GetResult(); }));

        Scheduler.Start();

        Assert.AreEqual(100, s);
        Assert.AreEqual(260, t);
        Assert.AreEqual(42, result);
    }

    [TestMethod]
    [DoNotParallelize]
    public void Await_Error()
    {
        SynchronizationContext.SetSynchronizationContext(null);
        var ex = new Exception();

        var xs = Scheduler.CreateHotObservable(
            OnNext(20, -1),
            OnNext(150, 0),
            OnNext(220, 1),
            OnNext(290, 2),
            OnNext(340, 3),
            OnError<int>(410, ex)
        );

        var awaiter = default(SeqAwaiter<int>);
        var t = long.MaxValue;

        Scheduler.ScheduleAbsolute(100, () => awaiter = GetAwaiter(xs));
        Scheduler.ScheduleAbsolute(200, () => awaiter!.OnCompleted(() => { t = Scheduler.Clock; Assert.AreSame(ex, Assert.ThrowsExactly<Exception>(() => awaiter!.GetResult())); }));

        Scheduler.Start();

        Assert.AreEqual(410, t);

        xs.Subscriptions.AssertEqual(
            Subscribe(100)
        );
    }

    [TestMethod]
    [DoNotParallelize]
    public void Await_Never()
    {
        SynchronizationContext.SetSynchronizationContext(null);
        var xs = Scheduler.CreateHotObservable(
            OnNext(20, -1),
            OnNext(150, 0),
            OnNext(220, 1),
            OnNext(290, 2),
            OnNext(340, 3)
        );

        var awaiter = default(SeqAwaiter<int>);
        var hasValue = default(bool);
        var t = long.MaxValue;

        Scheduler.ScheduleAbsolute(100, () => awaiter = GetAwaiter(xs));
        Scheduler.ScheduleAbsolute(200, () => awaiter!.OnCompleted(() => { t = Scheduler.Clock; awaiter!.GetResult(); hasValue = true; }));

        Scheduler.Start();

        Assert.AreEqual(long.MaxValue, t);
        Assert.IsFalse(hasValue);

        xs.Subscriptions.AssertEqual(
            Subscribe(100)
        );
    }

    [TestMethod]
    [DoNotParallelize]
    public void Await_Empty()
    {
        SynchronizationContext.SetSynchronizationContext(null);
        var xs = Scheduler.CreateHotObservable(
            OnCompleted<int>(300)
        );

        var awaiter = default(SeqAwaiter<int>);
        var t = long.MaxValue;

        Scheduler.ScheduleAbsolute(100, () => awaiter = GetAwaiter(xs));
        Scheduler.ScheduleAbsolute(200, () => awaiter!.OnCompleted(() => { t = Scheduler.Clock; Assert.ThrowsExactly<InvalidOperationException>(() => awaiter!.GetResult()); }));

        Scheduler.Start();

        Assert.AreEqual(300, t);

        xs.Subscriptions.AssertEqual(
            Subscribe(100)
        );
    }
}

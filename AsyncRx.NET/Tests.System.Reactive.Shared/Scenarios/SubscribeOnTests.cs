// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information. 

namespace Tests.System.Reactive.Shared.Scenarios;

/// <summary>Shared <c>SubscribeOn</c> scenarios, from Rx.NET's <c>SubscribeOnTest.cs</c>.</summary>
/// <remarks>
/// <para>
/// Five of the file's 12 tests, all virtual-time: over the test scheduler and over the
/// <see cref="MyCtx"/> context that posts to it. <c>SubscribeOn</c> schedules the subscription
/// and the disposal, and nothing else, so the source's subscription window moves by one tick on
/// Rx.NET's <c>TestScheduler</c> at each end (<c>Subscribe(201, 301)</c>) while the notifications
/// keep their ticks; the shared text writes <c>ScheduledAt</c> at both ends of the window and
/// leaves the messages as the original has them. <c>SubscribeOn_Scheduler_Sleep</c> reads the
/// clock from inside a <c>Create</c> subscribe and its disposal, the same two points.
/// </para>
/// <para>
/// Not here: the five Windows Forms and WPF tests, which Rx.NET's own runner compiles only
/// under <c>HAS_WINFORMS</c> and <c>HAS_WPF</c>, and the two <c>*_ArgumentChecking</c> tests,
/// the code-generated stratum.
/// </para>
/// </remarks>
public abstract class SubscribeOnTests : SharedReactiveTest
{
    [TestMethod]
    public void SubscribeOn_Scheduler_Sleep()
    {
        var scheduler = Scheduler;

        var s = 0L;
        var d = 0L;

        var xs = Seq.Create<long>(observer =>
        {
            s = scheduler.Clock;
            return ValueTask.FromResult<Action?>(() => d = scheduler.Clock);
        });

        var results = scheduler.Start(() =>
            xs.SubscribeOn(scheduler)
        );

        results.Messages.AssertEqual(
        );

        Assert.AreEqual(ScheduledAt(200), s);
        Assert.AreEqual(ScheduledAt(1000), d);
    }

    [TestMethod]
    public void SubscribeOn_Scheduler_Completed()
    {
        var scheduler = Scheduler;

        var xs = scheduler.CreateHotObservable(
            OnCompleted<long>(300)
        );

        var results = scheduler.Start(() =>
            xs.SubscribeOn(scheduler)
        );

        results.Messages.AssertEqual(
            OnCompleted<long>(300)
        );

        xs.Subscriptions.AssertEqual(
            Subscribe(ScheduledAt(200), ScheduledAt(300))
        );
    }

    [TestMethod]
    public void SubscribeOn_Scheduler_Error()
    {
        var scheduler = Scheduler;

        var ex = new Exception();

        var xs = scheduler.CreateHotObservable(
            OnError<int>(300, ex)
        );

        var results = scheduler.Start(() =>
            xs.SubscribeOn(scheduler)
        );

        results.Messages.AssertEqual(
            OnError<int>(300, ex)
        );

        xs.Subscriptions.AssertEqual(
            Subscribe(ScheduledAt(200), ScheduledAt(300))
        );
    }

    [TestMethod]
    public void SubscribeOn_Scheduler_Dispose()
    {
        var scheduler = Scheduler;

        var xs = scheduler.CreateHotObservable<int>(
        );

        var results = scheduler.Start(() =>
            xs.SubscribeOn(scheduler)
        );

        results.Messages.AssertEqual(
        );

        xs.Subscriptions.AssertEqual(
            Subscribe(ScheduledAt(200), ScheduledAt(1000))
        );
    }

    [TestMethod]
    public void SubscribeOn_SynchronizationContext_Simple()
    {
        var scheduler = Scheduler;

        var xs = scheduler.CreateHotObservable(
            OnNext(90, 1),
            OnNext(120, 2),
            OnNext(230, 3),
            OnNext(240, 4),
            OnNext(310, 5),
            OnNext(470, 6),
            OnCompleted<int>(530)
        );

        var results = scheduler.Start(() =>
            xs.SubscribeOn(new MyCtx(scheduler))
        );

        results.Messages.AssertEqual(
            OnNext(230, 3),
            OnNext(240, 4),
            OnNext(310, 5),
            OnNext(470, 6),
            OnCompleted<int>(530)
        );

        xs.Subscriptions.AssertEqual(
            Subscribe(ScheduledAt(200), ScheduledAt(530))
        );
    }
}

// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information. 

using System.Reactive;

namespace Tests.System.Reactive.Shared.Scenarios;

/// <summary>
/// Shared <c>TimeInterval</c> scenarios, from Rx.NET's <c>TimeIntervalTest.cs</c>.
/// </summary>
/// <remarks>
/// 9 of the file's 10 tests (the <c>TimeInterval_ArgumentChecking</c> test is the code-generated
/// stratum). Eight transcribe as they are: the virtual-time tests over the test
/// scheduler, once with its stopwatch (Rx.NET's optimisation, which AsyncRx.NET's clock stands
/// in for) and once with <c>DisableOptimizations()</c>, which on AsyncRx.NET is the scheduler
/// itself.
/// <c>TimeInterval_DefaultScheduler</c> counts the one element of
/// <c>Seq.Return(1).TimeInterval()</c> through
/// <see cref="SharedReactiveTest.ToListAsync{T}"/> where the original blocks on
/// <c>First()</c>.
/// </remarks>
public abstract class TimeIntervalTests : SharedReactiveTest
{
    [TestMethod]
    public void TimeInterval_Regular()
    {
        var scheduler = Scheduler;

        var xs = scheduler.CreateHotObservable(
            OnNext(150, 1),
            OnNext(210, 2),
            OnNext(230, 3),
            OnNext(260, 4),
            OnNext(300, 5),
            OnNext(350, 6),
            OnCompleted<int>(400)
        );

        var res = scheduler.Start(() =>
            xs.TimeInterval(scheduler.DisableOptimizations())
        );

        res.Messages.AssertEqual(
            OnNext(210, new TimeInterval<int>(2, TimeSpan.FromTicks(10))),
            OnNext(230, new TimeInterval<int>(3, TimeSpan.FromTicks(20))),
            OnNext(260, new TimeInterval<int>(4, TimeSpan.FromTicks(30))),
            OnNext(300, new TimeInterval<int>(5, TimeSpan.FromTicks(40))),
            OnNext(350, new TimeInterval<int>(6, TimeSpan.FromTicks(50))),
            OnCompleted<TimeInterval<int>>(400)
        );

        xs.Subscriptions.AssertEqual(
            Subscribe(200, 400)
        );
    }

    [TestMethod]
    public void TimeInterval_Empty()
    {
        var scheduler = Scheduler;

        var xs = scheduler.CreateHotObservable(
            OnNext(150, 1),
            OnCompleted<int>(300)
        );

        var res = scheduler.Start(() =>
            xs.TimeInterval(scheduler.DisableOptimizations())
        );

        res.Messages.AssertEqual(
            OnCompleted<TimeInterval<int>>(300)
        );

        xs.Subscriptions.AssertEqual(
            Subscribe(200, 300)
        );
    }

    [TestMethod]
    public void TimeInterval_Error()
    {
        var scheduler = Scheduler;

        var ex = new Exception();

        var xs = scheduler.CreateHotObservable(
            OnNext(150, 1),
            OnError<int>(300, ex)
        );

        var res = scheduler.Start(() =>
            xs.TimeInterval(scheduler.DisableOptimizations())
        );

        res.Messages.AssertEqual(
            OnError<TimeInterval<int>>(300, ex)
        );

        xs.Subscriptions.AssertEqual(
            Subscribe(200, 300)
        );
    }

    [TestMethod]
    public void TimeInterval_Never()
    {
        var scheduler = Scheduler;

        var xs = scheduler.CreateHotObservable(
            OnNext(150, 1)
        );

        var res = scheduler.Start(() =>
            xs.TimeInterval(scheduler.DisableOptimizations())
        );

        res.Messages.AssertEqual(
        );

        xs.Subscriptions.AssertEqual(
            Subscribe(200, 1000)
        );
    }

    [TestMethod]
    public async Task TimeInterval_DefaultScheduler()
    {
        Assert.IsTrue((await ToListAsync(Seq.Return(1).TimeInterval().Count())).Single() == 1);
    }

    [TestMethod]
    public void TimeInterval_WithStopwatch_Regular()
    {
        var scheduler = Scheduler;

        var xs = scheduler.CreateHotObservable(
            OnNext(150, 1),
            OnNext(210, 2),
            OnNext(230, 3),
            OnNext(260, 4),
            OnNext(300, 5),
            OnNext(350, 6),
            OnCompleted<int>(400)
        );

        var res = scheduler.Start(() =>
            xs.TimeInterval(scheduler)
        );

        res.Messages.AssertEqual(
            OnNext(210, new TimeInterval<int>(2, TimeSpan.FromTicks(10))),
            OnNext(230, new TimeInterval<int>(3, TimeSpan.FromTicks(20))),
            OnNext(260, new TimeInterval<int>(4, TimeSpan.FromTicks(30))),
            OnNext(300, new TimeInterval<int>(5, TimeSpan.FromTicks(40))),
            OnNext(350, new TimeInterval<int>(6, TimeSpan.FromTicks(50))),
            OnCompleted<TimeInterval<int>>(400)
        );

        xs.Subscriptions.AssertEqual(
            Subscribe(200, 400)
        );
    }

    [TestMethod]
    public void TimeInterval_WithStopwatch_Empty()
    {
        var scheduler = Scheduler;

        var xs = scheduler.CreateHotObservable(
            OnNext(150, 1),
            OnCompleted<int>(300)
        );

        var res = scheduler.Start(() =>
            xs.TimeInterval(scheduler)
        );

        res.Messages.AssertEqual(
            OnCompleted<TimeInterval<int>>(300)
        );

        xs.Subscriptions.AssertEqual(
            Subscribe(200, 300)
        );
    }

    [TestMethod]
    public void TimeInterval_WithStopwatch_Error()
    {
        var scheduler = Scheduler;

        var ex = new Exception();

        var xs = scheduler.CreateHotObservable(
            OnNext(150, 1),
            OnError<int>(300, ex)
        );

        var res = scheduler.Start(() =>
            xs.TimeInterval(scheduler)
        );

        res.Messages.AssertEqual(
            OnError<TimeInterval<int>>(300, ex)
        );

        xs.Subscriptions.AssertEqual(
            Subscribe(200, 300)
        );
    }

    [TestMethod]
    public void TimeInterval_WithStopwatch_Never()
    {
        var scheduler = Scheduler;

        var xs = scheduler.CreateHotObservable(
            OnNext(150, 1)
        );

        var res = scheduler.Start(() =>
            xs.TimeInterval(scheduler)
        );

        res.Messages.AssertEqual(
        );

        xs.Subscriptions.AssertEqual(
            Subscribe(200, 1000)
        );
    }
}

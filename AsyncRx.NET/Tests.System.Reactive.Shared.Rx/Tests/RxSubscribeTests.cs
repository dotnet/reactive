// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information. 

using System.Reactive;
using System.Reactive.Concurrency;
using System.Reactive.Linq;

using Microsoft.Reactive.Testing;

using Tests.System.Reactive.Shared.Scenarios;

// The originals write their lambdas with explicit parameter types.
#pragma warning disable IDE0350 // Use implicitly typed lambda

namespace Tests.System.Reactive.Shared.Rx.Tests;

/// <summary>
/// The shared <c>Subscribe</c> scenarios on Rx.NET, plus its <c>CancellationToken</c> tests.
/// </summary>
/// <remarks>
/// The eight <c>Subscribe_CT_*</c> tests are the originals: Rx.NET's
/// <c>Subscribe(..., CancellationToken)</c> overloads unsubscribe when the token is cancelled,
/// do nothing when it already is, and tolerate cancellation from inside a handler. AsyncRx.NET
/// has no such overloads (a deferred work item), so the tests run here against Rx.NET only, as
/// the originals but for MSTest's assertions and the immediate scheduler named in full, since
/// the shared base has a <c>Scheduler</c> property.
/// </remarks>
[TestClass]
public sealed class RxSubscribeTests : SubscribeTests
{
    protected override IRxTarget Target => RxTarget.Instance;

    [TestMethod]
    public void Subscribe_CT_None()
    {
        var scheduler = new TestScheduler();

        var xs = scheduler.CreateHotObservable(
            OnNext(210, 1),
            OnNext(220, 2),
            OnNext(230, 3),
            OnCompleted<int>(240)
        );

        var obs = scheduler.CreateObserver<int>();

        scheduler.ScheduleAbsolute(200, () => xs.Subscribe(obs, CancellationToken.None));

        scheduler.Start();

        obs.Messages.AssertEqual(
            OnNext(210, 1),
            OnNext(220, 2),
            OnNext(230, 3),
            OnCompleted<int>(240)
        );

        xs.Subscriptions.AssertEqual(
            Subscribe(200, Subscription.Infinite)
        );
    }

    [TestMethod]
    public void Subscribe_CT_CancelBeforeBegin()
    {
        var scheduler = new TestScheduler();

        var xs = scheduler.CreateHotObservable(
            OnNext(210, 1),
            OnNext(220, 2),
            OnNext(230, 3),
            OnCompleted<int>(240)
        );

        var cts = new CancellationTokenSource();

        var obs = scheduler.CreateObserver<int>();

        scheduler.ScheduleAbsolute(200, () => xs.Subscribe(obs, cts.Token));
        scheduler.ScheduleAbsolute(150, cts.Cancel);

        scheduler.Start();

        obs.Messages.AssertEqual(
        );

        xs.Subscriptions.AssertEqual(
        );
    }

    [TestMethod]
    public void Subscribe_CT_CancelMiddle()
    {
        var scheduler = new TestScheduler();

        var xs = scheduler.CreateHotObservable(
            OnNext(210, 1),
            OnNext(220, 2),
            OnNext(230, 3),
            OnCompleted<int>(240)
        );

        var cts = new CancellationTokenSource();

        var obs = scheduler.CreateObserver<int>();

        scheduler.ScheduleAbsolute(200, () => xs.Subscribe(obs, cts.Token));
        scheduler.ScheduleAbsolute(225, cts.Cancel);

        scheduler.Start();

        obs.Messages.AssertEqual(
            OnNext(210, 1),
            OnNext(220, 2)
        );

        xs.Subscriptions.AssertEqual(
            Subscribe(200, 225)
        );
    }

    [TestMethod]
    public void Subscribe_CT_CancelAfterEnd()
    {
        var scheduler = new TestScheduler();

        var ex = new Exception();

        var xs = scheduler.CreateHotObservable(
            OnNext(210, 1),
            OnNext(220, 2),
            OnNext(230, 3),
            OnError<int>(240, ex)
        );

        var cts = new CancellationTokenSource();

        var obs = scheduler.CreateObserver<int>();

        scheduler.ScheduleAbsolute(200, () => xs.Subscribe(obs, cts.Token));
        scheduler.ScheduleAbsolute(250, cts.Cancel);

        scheduler.Start();

        obs.Messages.AssertEqual(
            OnNext(210, 1),
            OnNext(220, 2),
            OnNext(230, 3),
            OnError<int>(240, ex)
        );

        xs.Subscriptions.AssertEqual(
            Subscribe(200, Subscription.Infinite)
        );
    }

    [TestMethod]
    public void Subscribe_CT_NeverCancel()
    {
        var scheduler = new TestScheduler();

        var xs = scheduler.CreateHotObservable(
            OnNext(210, 1),
            OnNext(220, 2),
            OnNext(230, 3),
            OnCompleted<int>(240)
        );

        var cts = new CancellationTokenSource();

        var obs = scheduler.CreateObserver<int>();

        scheduler.ScheduleAbsolute(200, () => xs.Subscribe(obs, cts.Token));

        scheduler.Start();

        obs.Messages.AssertEqual(
            OnNext(210, 1),
            OnNext(220, 2),
            OnNext(230, 3),
            OnCompleted<int>(240)
        );

        xs.Subscriptions.AssertEqual(
            Subscribe(200, Subscription.Infinite)
        );
    }

    [TestMethod]
    public void Subscribe_CT_Overloads_AlreadyCancelled()
    {
        var xs = Observable.Defer(() =>
        {
            Assert.Fail();
            return Observable.Return(42, global::System.Reactive.Concurrency.Scheduler.Immediate);
        });

        var cts = new CancellationTokenSource();
        cts.Cancel();

        xs.Subscribe(cts.Token);
        xs.Subscribe(_ => { }, cts.Token);
        xs.Subscribe(_ => { }, ex => { }, cts.Token);
        xs.Subscribe(_ => { }, () => { }, cts.Token);
        xs.Subscribe(_ => { }, ex => { }, () => { }, cts.Token);
        xs.Subscribe(Observer.Create<int>(_ => { }, ex => { }, () => { }), cts.Token);
    }

    [TestMethod]
    public void Subscribe_CT_Overloads_None()
    {
        var i = 0;
        var n = 0;
        var e = 0;
        var c = 0;

        var xs = Observable.Defer(() =>
        {
            i++;
            return Observable.Return(42, global::System.Reactive.Concurrency.Scheduler.Immediate);
        });

        xs.Subscribe(CancellationToken.None);
        xs.Subscribe(_ => { n++; }, CancellationToken.None);
        xs.Subscribe(_ => { n++; }, ex => { e++; }, CancellationToken.None);
        xs.Subscribe(_ => { n++; }, () => { c++; }, CancellationToken.None);
        xs.Subscribe(_ => { n++; }, ex => { e++; }, () => { c++; }, CancellationToken.None);
        xs.Subscribe(
            Observer.Create<int>(_ => { n++; }, ex => { e++; }, () => { c++; }),
            CancellationToken.None);

        Assert.AreEqual(6, i);
        Assert.AreEqual(5, n);
        Assert.AreEqual(0, e);
        Assert.AreEqual(3, c);
    }

    [TestMethod]
    public void Subscribe_CT_CancelDuringCallback()
    {
        var scheduler = new TestScheduler();

        var xs = scheduler.CreateHotObservable(
            OnNext(210, 1),
            OnNext(220, 2),
            OnNext(230, 3),
            OnCompleted<int>(240)
        );

        var cts = new CancellationTokenSource();

        var n = 0;

        scheduler.ScheduleAbsolute(200, () => xs.Subscribe(x =>
        {
            n++;

            if (x == 2)
            {
                cts.Cancel();
            }
        }, cts.Token));

        scheduler.Start();

        Assert.AreEqual(2, n);

        xs.Subscriptions.AssertEqual(
            Subscribe(200, 220)
        );
    }
}

// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information. 

using System.Reactive;
using System.Reactive.Concurrency;
using System.Reactive.Linq;

using Microsoft.Reactive.Testing.Async;

using Tests.System.Reactive.Shared;
using Tests.System.Reactive.Shared.Scenarios;

namespace Tests.System.Reactive.Async.Tests;

/// <summary>
/// Runs the kit's shared <c>Take</c> scenarios against AsyncRx.NET. The shared class needs
/// nothing from here — the witness supplies the operator — so this intermediate class exists
/// only to hold the target supplements below; the two sealed classes pin the execution shape.
/// </summary>
public abstract class AsyncRxTakeTests(ExecutionShape shape) : TakeTests
{
    protected override IRxTarget Target { get; } = new AsyncRxTarget(shape);

    // ---- AsyncRx.NET-specific scenarios: a consumer that prolongs completion ----
    //
    // Target-specific scenarios: ordinary [TestMethod]s next to the shared
    // ones, exercising what sync Rx cannot express — an OnNextAsync whose returned task
    // completes later than it started. They use the harness's extended expectation forms
    // directly, through the native scheduler ((TestAsyncScheduler)Scheduler.Native).

    private TestAsyncScheduler Pump => (TestAsyncScheduler)Scheduler.Native;

    /// <summary>A recording observer whose OnNext completions take <paramref name="ticks"/> virtual ticks each.</summary>
    private ITestableAsyncObserver<T> ProlongingObserver<T>(long ticks) =>
        Pump.CreateObserver<T>(n => n.Kind == NotificationKind.OnNext ? Pump.Delay(TimeSpan.FromTicks(ticks)) : default);

    [TestMethod]
    public void Take_ProlongedConsumer_CompletionWaitsForTheLastDelivery()
    {
        // Each delivery takes 50 ticks to complete. The hot source serializes its deliveries
        // behind the consumer, and Take's completion (issued once the second value has been
        // delivered) cannot overtake that delivery: it is issued when the delivery completes.
        var xs = Scheduler.CreateHotObservable(
            OnNext(210, 1),
            OnNext(220, 2),
            OnNext(230, 3),
            OnCompleted<int>(300)
        );

        var res = Pump.Start(() => ((IAsyncObservable<int>)xs.Native).Take(2), ProlongingObserver<int>(50));

        res.Messages.AssertEqual(
            OnNext((210, 260), 1),
            OnNext((260, 310), 2),
            OnCompleted<int>((310, 310))
        );

        xs.Subscriptions.AssertEqual(
            Subscribe(200, 310)
        );
    }

    [TestMethod]
    public void Take_ProlongedConsumer_DisposedDuringADelivery()
    {
        // Disposal at 230 arrives while the delivery that started at 210 is still in flight.
        // The in-flight delivery runs to completion (the consumer is still consuming); nothing
        // further is delivered; the source subscription is released at the disposal tick.
        var xs = Scheduler.CreateHotObservable(
            OnNext(210, 1),
            OnNext(220, 2),
            OnNext(230, 3),
            OnCompleted<int>(300)
        );

        var res = Pump.Start(() => ((IAsyncObservable<int>)xs.Native).Take(3), ProlongingObserver<int>(50), disposed: 230);

        res.Messages.AssertEqual(
            OnNext((210, 260), 1)
        );

        xs.Subscriptions.AssertEqual(
            Subscribe(200, 230)
        );
    }

    [TestMethod]
    public void Take_Timed_ProlongedConsumer_TimerFiresDuringADelivery()
    {
        // The 30-tick timer wants to complete the sequence at 230, while the delivery that
        // started at 210 is still in flight. Completion must not overlap a delivery (observer
        // grammar), so it is issued when that delivery completes, at 260 — the async analogue
        // of a sync consumer that has not yet returned from OnNext when the timer fires.
        var xs = Scheduler.CreateHotObservable(
            OnNext(210, 1),
            OnNext(220, 2),
            OnCompleted<int>(300)
        );

        var res = Pump.Start(() => ((IAsyncObservable<int>)xs.Native).Take(TimeSpan.FromTicks(30), Pump), ProlongingObserver<int>(50));

        res.Messages.AssertEqual(
            OnNext((210, 260), 1),
            OnCompleted<int>((260, 260))
        );

        xs.Subscriptions.AssertEqual(
            Subscribe(200, 260)
        );
    }
}

[TestClass]
public sealed class AsyncRxTakeTests_SynchronousCompletion() : AsyncRxTakeTests(ExecutionShape.SynchronousCompletion);

[TestClass]
public sealed class AsyncRxTakeTests_ForcedYield() : AsyncRxTakeTests(ExecutionShape.ForcedYield);

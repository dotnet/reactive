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

/// <summary>Runs the shared <c>RepeatWhen</c> scenarios against AsyncRx.NET.</summary>
/// <remarks>
/// The shared class needs nothing from here — the target supplies the operator — so this
/// intermediate class exists only to hold the target-specific scenarios below. The two sealed
/// classes pin the execution shape.
/// </remarks>
public abstract class AsyncRxRepeatWhenTests(ExecutionShape shape) : RepeatWhenTests
{
    protected override IRxTarget Target { get; } = new AsyncRxTarget(shape);

    // ---- AsyncRx.NET-specific scenarios: the handler terminates during a delivery ----
    //
    // What sync Rx cannot express: an OnNextAsync whose returned task completes later than it
    // started, with the handler's sequence terminating meanwhile. The terminal notification
    // must wait for the delivery to finish. These use the harness's extended expectation forms
    // directly, through the native scheduler (Scheduler.Native.Get<TestAsyncScheduler>()).

    private TestAsyncScheduler Pump => Scheduler.Native.Get<TestAsyncScheduler>();

    /// <summary>A recording observer whose OnNext completions take <paramref name="ticks"/> virtual ticks each.</summary>
    private ITestableAsyncObserver<T> ProlongingObserver<T>(long ticks) =>
        Pump.CreateObserver<T>(n => n.Kind == NotificationKind.OnNext ? Pump.Delay(TimeSpan.FromTicks(ticks)) : default);

    [TestMethod]
    public void RepeatWhen_ProlongedConsumer_HandlerCompletesDuringADelivery()
    {
        // The delivery that starts at 210 completes at 260; the handler completes at 230, while
        // it is in flight. The completion follows the delivery, and both subscriptions end with it.
        var xs = Scheduler.CreateHotObservable(
            OnNext(210, 1),
            OnNext(300, 2),
            OnCompleted<int>(400)
        );

        var ys = Scheduler.CreateHotObservable(
            OnCompleted<int>(230)
        );

        var res = Pump.Start(
            () => xs.Native.Get<IAsyncObservable<int>>().RepeatWhen(_ => ys.Native.Get<IAsyncObservable<int>>()),
            ProlongingObserver<int>(50));

        res.Messages.AssertEqual(
            OnNext((210, 260), 1),
            OnCompleted<int>((260, 260))
        );

        xs.Subscriptions.AssertEqual(
            Subscribe(200, 260)
        );
    }

    [TestMethod]
    public void RepeatWhen_ProlongedConsumer_HandlerFailsDuringADelivery()
    {
        // As above, with the handler failing at 230: the error follows the delivery.
        var ex = new InvalidOperationException();

        var xs = Scheduler.CreateHotObservable(
            OnNext(210, 1),
            OnNext(300, 2),
            OnCompleted<int>(400)
        );

        var ys = Scheduler.CreateHotObservable(
            OnError<int>(230, ex)
        );

        var res = Pump.Start(
            () => xs.Native.Get<IAsyncObservable<int>>().RepeatWhen(_ => ys.Native.Get<IAsyncObservable<int>>()),
            ProlongingObserver<int>(50));

        res.Messages.AssertEqual(
            OnNext((210, 260), 1),
            OnError<int>((260, 260), ex)
        );

        xs.Subscriptions.AssertEqual(
            Subscribe(200, 260)
        );
    }
}

[TestClass]
public sealed class AsyncRxRepeatWhenTests_SynchronousCompletion() : AsyncRxRepeatWhenTests(ExecutionShape.SynchronousCompletion);

[TestClass]
public sealed class AsyncRxRepeatWhenTests_ForcedYield() : AsyncRxRepeatWhenTests(ExecutionShape.ForcedYield);

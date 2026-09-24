// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information. 

using System.Reactive;
using System.Reactive.Concurrency;
using System.Reactive.Linq;

using Microsoft.Reactive.Testing.Async;

using Tests.System.Reactive.Shared;
using Tests.System.Reactive.Shared.Scenarios;

namespace Tests.System.Reactive.Async;

/// <summary>
/// Runs the kit's shared <c>Window</c> scenarios against AsyncRx.NET, under both execution
/// shapes, plus the platform supplement below.
/// </summary>
public abstract class AsyncRxWindowTests(ExecutionShape shape) : WindowTests
{
    protected override IPlatform Platform { get; } = new AsyncRxPlatform(shape);

    // ---- AsyncRx.NET-specific scenario: a consumer that prolongs completion ----

    private TestAsyncScheduler Pump => (TestAsyncScheduler)Scheduler.Native;

    [TestMethod]
    public void WindowWithCount_ProlongedConsumer_BackPressureReachesTheSource()
    {
        // The merged consumer takes 40 ticks per value. Everything upstream — Merge, the window
        // subjects, Window itself and the hot source — waits for each delivery to complete, so
        // the source's deliveries (scheduled every 10 ticks) back up behind the consumer, the
        // second window opens only once the first has drained, and the source's completion
        // (scheduled at 300) is delivered after the last value's delivery completes.
        var xs = Scheduler.CreateHotObservable(
            OnNext(210, 1),
            OnNext(220, 2),
            OnNext(230, 3),
            OnNext(240, 4),
            OnCompleted<int>(300)
        );

        var observer = Pump.CreateObserver<string>(n => n.Kind == NotificationKind.OnNext ? Pump.Delay(TimeSpan.FromTicks(40)) : default);

        var res = Pump.Start(
            () => ((IAsyncObservable<int>)xs.Native).Window(2).Select((w, i) => w.Select(x => i + " " + x)).Merge(),
            observer);

        res.Messages.AssertEqual(
            OnNext((210, 250), "0 1"),
            OnNext((250, 290), "0 2"),
            OnNext((290, 330), "1 3"),
            OnNext((330, 370), "1 4"),
            OnCompleted<string>((370, 370))
        );

        xs.Subscriptions.AssertEqual(
            Subscribe(200, 370)
        );
    }
}

[TestClass]
public sealed class AsyncRxWindowTests_SynchronousCompletion() : AsyncRxWindowTests(ExecutionShape.SynchronousCompletion);

[TestClass]
public sealed class AsyncRxWindowTests_ForcedYield() : AsyncRxWindowTests(ExecutionShape.ForcedYield);

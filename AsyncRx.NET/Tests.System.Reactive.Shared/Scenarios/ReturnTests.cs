// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information. 

namespace Tests.System.Reactive.Shared.Scenarios;

/// <summary>Shared <c>Return</c> scenarios, from Rx.NET's <c>ReturnTest.cs</c>.</summary>
/// <remarks>
/// 5 of the file's 6 tests (the <c>Return_ArgumentChecking</c> test is the code-generated
/// stratum). <c>Return_Basic</c> transcribes with <c>ScheduledAt(200)</c> for the original's
/// 201. <c>Return_Disposed</c> disposes inside the scheduled subscribe, right after it, where the
/// original disposes at the subscribe tick: on Rx.NET either is before the delivery at 201, on
/// the pump the delivery is due-now work that runs before any disposal scheduled for the same
/// tick, so only a disposal within the subscribe precedes it.
/// <c>Return_DisposedAfterNext</c> disposes the subscription from inside the element handler
/// and then pushes the element to a testable observer, as the original does, holding the
/// subscription in a local the scheduled subscribe assigns rather than a serial disposable.
/// <c>Return_ObserverThrows</c> takes the <c>Repeat</c> scenarios' shape (Step 31): one
/// scheduler per case, a scheduled subscribe, and the handler's exception expected from
/// <c>Start</c>. <c>Return_DefaultScheduler</c> compares the default-scheduler form with the
/// explicit one through <see cref="SharedReactiveTest.ToListAsync{T}"/>, where the original
/// compares with a blocking <c>AssertEqual</c>.
/// </remarks>
public abstract class ReturnTests : SharedReactiveTest
{
    [TestMethod]
    public void Return_Basic()
    {
        var scheduler = Scheduler;

        var res = scheduler.Start(() =>
            Seq.Return(42, scheduler)
        );

        res.Messages.AssertEqual(
            OnNext(ScheduledAt(200), 42),
            OnCompleted<int>(ScheduledAt(200))
        );
    }

    [TestMethod]
    public void Return_Disposed()
    {
        var scheduler = Scheduler;

        var xs = Seq.Return(42, scheduler);

        var res = scheduler.CreateObserver<int>();

        scheduler.ScheduleAbsolute(200, async () =>
        {
            var d = await xs.SubscribeAsync(res);
            await d.DisposeAsync();
        });

        scheduler.Start();

        res.Messages.AssertEqual(
        );
    }

    [TestMethod]
    public void Return_DisposedAfterNext()
    {
        var scheduler = Scheduler;

        IAsyncDisposable? d = null;

        var xs = Seq.Return(42, scheduler);

        var res = scheduler.CreateObserver<int>();

        scheduler.ScheduleAbsolute(100, async () =>
            d = await xs.SubscribeAsync(
                scheduler,
                async x =>
                {
                    await d!.DisposeAsync();
                    await res.OnNextAsync(x);
                },
                res.OnErrorAsync,
                res.OnCompletedAsync));

        scheduler.Start();

        res.Messages.AssertEqual(
            OnNext(ScheduledAt(100), 42)
        );
    }

    [TestMethod]
    public void Return_ObserverThrows()
    {
        var scheduler1 = new TestSchedulerRef(Target);

        var xs = Seq.Return(1, scheduler1);

        scheduler1.ScheduleAbsolute(100, async () =>
            await xs.SubscribeAsync(scheduler1, x => { throw new InvalidOperationException(); }));

        Assert.ThrowsExactly<InvalidOperationException>(() => scheduler1.Start());

        var scheduler2 = new TestSchedulerRef(Target);

        var ys = Seq.Return(1, scheduler2);

        scheduler2.ScheduleAbsolute(100, async () =>
            await ys.SubscribeAsync(
                scheduler2,
                x => { },
                ex => { },
                () => { throw new InvalidOperationException(); }));

        Assert.ThrowsExactly<InvalidOperationException>(() => scheduler2.Start());
    }

    [TestMethod]
    public async Task Return_DefaultScheduler()
    {
        Assert.IsTrue(
            (await ToListAsync(Seq.Return(42)))
                .SequenceEqual(await ToListAsync(Seq.Return(42, DefaultScheduler))));
    }
}

// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information. 

namespace Tests.System.Reactive.Shared.Scenarios;

/// <summary>Shared <c>Interval</c> scenarios, from Rx.NET's <c>IntervalTest.cs</c>.</summary>
/// <remarks>
/// 6 of the file's 7 tests (<c>Interval_TimeSpan_ArgumentChecking</c> is the code-generated
/// stratum). <c>Interval_TimeSpan_Basic</c> and <c>Interval_TimeSpan_Disposed</c> transcribe as
/// they are. <c>Interval_TimeSpan_Zero</c> disposes from the ninth element's handler where the
/// original disposes at tick 210: on the pump a zero-period timer never leaves its tick, since a
/// zero delay completes at once (as the untimed <c>Generate</c> loop in Step 55), so its elements
/// are at <c>ScheduledAt(200, n)</c>, the test scheduler's tick bump per element.
/// <c>Interval_TimeSpan_ObserverThrows</c> subscribes from a scheduled action in the
/// <c>Return</c> shape (Step 50). The two <c>*_DefaultScheduler</c> tests await through
/// <see cref="SharedReactiveTest.ToListAsync{T}"/> where the originals block on
/// <c>ToEnumerable</c> or wait on an event and count a test observer's messages.
/// </remarks>
public abstract class IntervalTests : SharedReactiveTest
{
    [TestMethod]
    public void Interval_TimeSpan_Basic()
    {
        var scheduler = Scheduler;

        var res = scheduler.Start(() =>
            Seq.Interval(TimeSpan.FromTicks(100), scheduler)
        );

        res.Messages.AssertEqual(
            OnNext(300, 0L),
            OnNext(400, 1L),
            OnNext(500, 2L),
            OnNext(600, 3L),
            OnNext(700, 4L),
            OnNext(800, 5L),
            OnNext(900, 6L)
        );
    }

    [TestMethod]
    public void Interval_TimeSpan_Zero()
    {
        var scheduler = Scheduler;

        IAsyncDisposable? d = null;

        var xs = Seq.Interval(TimeSpan.FromTicks(0), scheduler);

        var res = scheduler.CreateObserver<long>();

        scheduler.ScheduleAbsolute(200, async () =>
            d = await xs.SubscribeAsync(
                scheduler,
                async x =>
                {
                    await res.OnNextAsync(x);

                    if (x == 8)
                    {
                        await d!.DisposeAsync();
                    }
                },
                res.OnErrorAsync,
                res.OnCompletedAsync));

        scheduler.Start();

        res.Messages.AssertEqual(
            OnNext(ScheduledAt(200, 1), 0L),
            OnNext(ScheduledAt(200, 2), 1L),
            OnNext(ScheduledAt(200, 3), 2L),
            OnNext(ScheduledAt(200, 4), 3L),
            OnNext(ScheduledAt(200, 5), 4L),
            OnNext(ScheduledAt(200, 6), 5L),
            OnNext(ScheduledAt(200, 7), 6L),
            OnNext(ScheduledAt(200, 8), 7L),
            OnNext(ScheduledAt(200, 9), 8L)
        );
    }

    [TestMethod]
    public async Task Interval_TimeSpan_Zero_DefaultScheduler()
    {
        var lst = await ToListAsync(Seq.Interval(TimeSpan.Zero).TakeWhile(i => i < 10));

        Assert.HasCount(10, lst);
    }

    [TestMethod]
    public void Interval_TimeSpan_Disposed()
    {
        var scheduler = Scheduler;

        var res = scheduler.Start(
            () => Seq.Interval(TimeSpan.FromTicks(1000), scheduler)
        );

        res.Messages.AssertEqual(
        );
    }

    [TestMethod]
    public void Interval_TimeSpan_ObserverThrows()
    {
        var scheduler = new TestSchedulerRef(Target);

        var xs = Seq.Interval(TimeSpan.FromTicks(1), scheduler);

        scheduler.ScheduleAbsolute(100, async () =>
            await xs.SubscribeAsync(scheduler, x => { throw new InvalidOperationException(); }));

        Assert.ThrowsExactly<InvalidOperationException>(() => scheduler.Start());
    }

    [TestMethod]
    public async Task Interval_TimeSpan_DefaultScheduler()
    {
        Assert.IsTrue(
            (await ToListAsync(Seq.Interval(TimeSpan.FromMilliseconds(1)).Take(3)))
                .SequenceEqual([0L, 1L, 2L]));
    }
}

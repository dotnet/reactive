// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information. 

namespace Tests.System.Reactive.Shared.Scenarios;

/// <summary>Shared <c>Timer</c> scenarios, from Rx.NET's <c>TimerTest.cs</c>.</summary>
/// <remarks>
/// <para>
/// 17 of the file's 28 tests. Nine transcribe as they are, with <c>ScheduledAt(200)</c> where
/// the original expects tick 201 for a timer due at or before the subscribe tick (a zero,
/// negative, present or past due time): the one-shot timer with a relative and with an absolute
/// due time, its disposal before firing, and the periodic timer with each kind of due time over
/// the <c>Start(create, 0, 200, 750)</c> window. <c>OneShotTimer_TimeSpan_ObserverThrows</c>
/// subscribes from a scheduled action in the <c>Return</c> shape (Step 50). The four
/// <c>*_DefaultScheduler</c> tests await through <see cref="SharedReactiveTest.ToListAsync{T}"/>
/// where the originals block on <c>ToEnumerable</c>;
/// <c>OneShotTimer_TimeSpan_Zero_DefaultScheduler</c> and
/// <c>RepeatingTimer_TimeSpan_Zero_DefaultScheduler</c> count that list where the originals count
/// a test observer's messages once an event is set. <c>RepeatingTimer_Start_CatchUp</c> holds the
/// first element's handler for fifty periods in a <c>Do</c> and lets <c>TakeWhile</c> end the
/// sequence after the eleventh element, where the original holds it in the subscriber's handler
/// and disposes from there; the claim, that the timer goes on past a handler that outlasts many
/// periods, is the same.
/// </para>
/// <para>
/// Not here: <c>OneShotTimer_TimeSpan_ArgumentChecking</c> (the code-generated stratum);
/// <c>RepeatingTimer_Periodic1</c> and <c>RepeatingTimer_Periodic2</c>, which repeat their
/// twins' expectations over Rx.NET's <c>PeriodicTestScheduler</c> and add an assertion on the
/// periodic timer runs it records, a sync-scheduler feature with no async counterpart, as the
/// <c>Window</c>, <c>Buffer</c> and <c>Sample</c> periodic tests were left; and the seven
/// <c>RepeatingTimer_*_Slippage*</c> tests and <c>RepeatingTimer_Start_CatchUp_Throws</c>, which
/// pin how Rx.NET's periodic timer keeps time when a handler outlasts the period (the stopwatch
/// catch-up and the emulated periodic scheduling) through <c>scheduler.Sleep</c> inside a
/// handler, <c>DisableOptimizations</c> and a scheduler's <c>Catch</c>, none of which
/// AsyncRx.NET has (its periodic timer waits a period after each element, with no catch-up, a
/// deferred question); they run in <c>Tests.System.Reactive.Shared.Rx</c> as the originals.
/// </para>
/// </remarks>
public abstract class TimerTests : SharedReactiveTest
{
    [TestMethod]
    public void OneShotTimer_TimeSpan_Basic()
    {
        var scheduler = Scheduler;

        var res = scheduler.Start(() =>
            Seq.Timer(TimeSpan.FromTicks(300), scheduler)
        );

        res.Messages.AssertEqual(
            OnNext(500, 0L),
            OnCompleted<long>(500)
        );
    }

    [TestMethod]
    public void OneShotTimer_TimeSpan_Zero()
    {
        var scheduler = Scheduler;

        var res = scheduler.Start(() =>
            Seq.Timer(TimeSpan.FromTicks(0), scheduler)
        );

        res.Messages.AssertEqual(
            OnNext(ScheduledAt(200), 0L),
            OnCompleted<long>(ScheduledAt(200))
        );
    }

    [TestMethod]
    public async Task OneShotTimer_TimeSpan_Zero_DefaultScheduler()
    {
        var lst = await ToListAsync(Seq.Timer(TimeSpan.Zero));

        Assert.HasCount(1, lst);
    }

    [TestMethod]
    public void OneShotTimer_TimeSpan_Negative()
    {
        var scheduler = Scheduler;

        var res = scheduler.Start(() =>
            Seq.Timer(TimeSpan.FromTicks(-1), scheduler)
        );

        res.Messages.AssertEqual(
            OnNext(ScheduledAt(200), 0L),
            OnCompleted<long>(ScheduledAt(200))
        );
    }

    [TestMethod]
    public void OneShotTimer_TimeSpan_Disposed()
    {
        var scheduler = Scheduler;

        var res = scheduler.Start(() =>
            Seq.Timer(TimeSpan.FromTicks(1000), scheduler)
        );

        res.Messages.AssertEqual(
        );
    }

    [TestMethod]
    public void OneShotTimer_TimeSpan_ObserverThrows()
    {
        var scheduler1 = new TestSchedulerRef(Target);

        var xs = Seq.Timer(TimeSpan.FromTicks(1), scheduler1);

        scheduler1.ScheduleAbsolute(100, async () =>
            await xs.SubscribeAsync(scheduler1, x => { throw new InvalidOperationException(); }));

        Assert.ThrowsExactly<InvalidOperationException>(() => scheduler1.Start());

        var scheduler2 = new TestSchedulerRef(Target);

        var ys = Seq.Timer(TimeSpan.FromTicks(1), scheduler2);

        scheduler2.ScheduleAbsolute(100, async () =>
            await ys.SubscribeAsync(
                scheduler2,
                x => { },
                ex => { },
                () => { throw new InvalidOperationException(); }));

        Assert.ThrowsExactly<InvalidOperationException>(() => scheduler2.Start());
    }

    [TestMethod]
    public async Task OneShotTimer_TimeSpan_DefaultScheduler()
    {
        Assert.IsTrue(
            (await ToListAsync(Seq.Timer(TimeSpan.FromMilliseconds(1)))).SequenceEqual([0L]));
    }

    [TestMethod]
    public async Task OneShotTimer_DateTimeOffset_DefaultScheduler()
    {
        Assert.IsTrue(
            (await ToListAsync(Seq.Timer(DateTimeOffset.UtcNow + TimeSpan.FromSeconds(1))))
                .SequenceEqual([0L]));
    }

    [TestMethod]
    public async Task OneShotTimer_TimeSpan_TimeSpan_DefaultScheduler()
    {
        Assert.IsTrue(
            (await ToListAsync(
                Seq.Timer(TimeSpan.FromMilliseconds(1), TimeSpan.FromMilliseconds(1)).Take(2)))
                .SequenceEqual([0L, 1L]));
    }

    [TestMethod]
    public async Task OneShotTimer_DateTimeOffset_TimeSpan_DefaultScheduler()
    {
        Assert.IsTrue(
            (await ToListAsync(
                Seq.Timer(
                    DateTimeOffset.UtcNow + TimeSpan.FromSeconds(1),
                    TimeSpan.FromMilliseconds(1))
                    .Take(2)))
                .SequenceEqual([0L, 1L]));
    }

    [TestMethod]
    public void OneShotTimer_DateTimeOffset_Basic()
    {
        var scheduler = Scheduler;

        var res = scheduler.Start(() =>
            Seq.Timer(new DateTimeOffset(500, TimeSpan.Zero), scheduler)
        );

        res.Messages.AssertEqual(
            OnNext(500, 0L),
            OnCompleted<long>(500)
        );
    }

    [TestMethod]
    public void OneShotTimer_DateTimeOffset_Zero()
    {
        var scheduler = Scheduler;

        var res = scheduler.Start(() =>
            Seq.Timer(new DateTimeOffset(200, TimeSpan.Zero), scheduler)
        );

        res.Messages.AssertEqual(
            OnNext(ScheduledAt(200), 0L),
            OnCompleted<long>(ScheduledAt(200))
        );
    }

    [TestMethod]
    public void OneShotTimer_DateTimeOffset_Past()
    {
        var scheduler = Scheduler;

        var res = scheduler.Start(() =>
            Seq.Timer(new DateTimeOffset(0, TimeSpan.Zero), scheduler)
        );

        res.Messages.AssertEqual(
            OnNext(ScheduledAt(200), 0L),
            OnCompleted<long>(ScheduledAt(200))
        );
    }

    [TestMethod]
    public async Task RepeatingTimer_TimeSpan_Zero_DefaultScheduler()
    {
        var lst = await ToListAsync(
            Seq.Timer(TimeSpan.Zero, TimeSpan.Zero).TakeWhile(i => i < 10));

        Assert.HasCount(10, lst);
    }

    [TestMethod]
    public void RepeatingTimer_DateTimeOffset_TimeSpan_Simple()
    {
        var scheduler = Scheduler;

        var res = scheduler.Start(() =>
            Seq.Timer(new DateTimeOffset(300, TimeSpan.Zero), TimeSpan.FromTicks(100), scheduler),
            0, 200, 750
        );

        res.Messages.AssertEqual(
            OnNext(300, 0L),
            OnNext(400, 1L),
            OnNext(500, 2L),
            OnNext(600, 3L),
            OnNext(700, 4L)
        );
    }

    [TestMethod]
    public void RepeatingTimer_TimeSpan_TimeSpan_Simple()
    {
        var scheduler = Scheduler;

        var res = scheduler.Start(() =>
            Seq.Timer(TimeSpan.FromTicks(100), TimeSpan.FromTicks(100), scheduler),
            0, 200, 750
        );

        res.Messages.AssertEqual(
            OnNext(300, 0L),
            OnNext(400, 1L),
            OnNext(500, 2L),
            OnNext(600, 3L),
            OnNext(700, 4L)
        );
    }

    [TestMethod]
    public async Task RepeatingTimer_Start_CatchUp()
    {
        var xs = Seq.Timer(TimeSpan.Zero, TimeSpan.FromMilliseconds(10));

        var lst = await ToListAsync(
            xs.Do(x =>
                {
                    if (x == 0)
                    {
                        Thread.Sleep(500);
                    }
                })
                .TakeWhile(x => x <= 10));

        Assert.IsTrue(lst.SequenceEqual(Enumerable.Range(0, 11).Select(i => (long)i)));
    }
}

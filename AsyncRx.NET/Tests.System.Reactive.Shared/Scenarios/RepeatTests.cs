// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information. 

namespace Tests.System.Reactive.Shared.Scenarios;

/// <summary>Shared <c>Repeat</c> scenarios, from Rx.NET's <c>RepeatTest.cs</c>.</summary>
/// <remarks>
/// <para>
/// 19 of the file's 27 tests (the eight <c>*_ArgumentChecking</c> tests are the code-generated
/// stratum). The seven virtual-time tests over <c>xs.Repeat()</c> and <c>xs.Repeat(3)</c> with
/// cold sources transcribe as they are. The rest are over the value forms,
/// <c>Repeat(value[, repeatCount][, scheduler])</c>, whose scheduled loop is where the two test
/// schedulers differ: Rx.NET's <c>TestScheduler</c> moves one tick per scheduling, so the
/// original sees one value per tick from 201, and AsyncRx.NET's pump does not; the shared text
/// writes <c>ScheduledAt(200, n)</c> for the nth value, as the <c>SelectMany</c> scenarios do
/// for <c>Range</c>. Two are bounded where the original is not: <c>Repeat_Value_Count_Dispose</c>
/// and <c>Repeat_Value</c> take six values with <c>Take(6)</c> where the original disposes at
/// 207 and sees six values only because of that tick per scheduling; a pump that does not move
/// time runs the whole loop (or, for the infinite form, never leaves it) before a disposal due
/// at a later tick. What those tests verify, that disposing the subscription stops the loop
/// part-way, <c>Repeat_Count_LongRunning2</c> and <c>Repeat_Inf_LongRunning</c> verify in real
/// time on both targets.
/// </para>
/// <para>
/// The three <c>*_LongRunning</c> tests run over the shared
/// <see cref="TestLongRunningScheduler"/> and await its
/// <see cref="TestLongRunningScheduler.Started"/> and
/// <see cref="TestLongRunningScheduler.Stopped"/> where the original blocks on its events; the
/// two that wait for a hundredth value wait on an event the handler sets where the original
/// spins on the list's count, so they drop the original's <c>NoOptimization</c> attribute with
/// the spin, and its <c>Assert.True(true)</c> placeholder, which the MSTest analyzer flags: the
/// claim is that the scheduler's <c>Stopped</c> is reached. The two <c>*_Default</c> tests
/// compare the scheduler-less overload with its explicit <c>DefaultScheduler</c> twin by
/// collecting both through <c>ToListAsync</c>, as the <c>Replay</c> scenarios do. The two
/// <c>*_Throws</c> tests keep the original's one scheduler per case, as
/// <c>new TestSchedulerRef(Target)</c>, since a scheduler whose <c>Start</c> has thrown is
/// done. In the two cases whose handler throws under <c>Start</c>, the subscribe is a scheduled
/// action: a body-level subscribe runs the current tick's work on the pump, so the scheduled
/// <c>Return</c> would deliver, and the handler throw, there rather than under <c>Start</c>.
/// The case that repeats a scheduled <c>Return(1)</c> until a disposal at 210 repeats a cold
/// sequence with the same one-tick timing instead, which moves virtual time on both targets;
/// the throwing completion handler is never reached either way, which is what the case
/// verifies.
/// </para>
/// <para>
/// <c>Repeat</c>'s operator forms were plumbing before this step, nodes other scenarios compose
/// with; they keep that role from their own folder, and the value forms are new to the model.
/// </para>
/// </remarks>
public abstract class RepeatTests : SharedReactiveTest
{
    [TestMethod]
    public void Repeat_Value_Count_Zero()
    {
        var scheduler = Scheduler;

        var res = scheduler.Start(() =>
            Seq.Repeat(42, 0, scheduler)
        );

        res.Messages.AssertEqual(
            OnCompleted<int>(ScheduledAt(200))
        );
    }

    [TestMethod]
    public void Repeat_Value_Count_One()
    {
        var scheduler = Scheduler;

        var res = scheduler.Start(() =>
            Seq.Repeat(42, 1, scheduler)
        );

        res.Messages.AssertEqual(
            OnNext(ScheduledAt(200), 42),
            OnCompleted<int>(ScheduledAt(200))
        );
    }

    [TestMethod]
    public void Repeat_Value_Count_Ten()
    {
        var scheduler = Scheduler;

        var res = scheduler.Start(() =>
            Seq.Repeat(42, 10, scheduler)
        );

        res.Messages.AssertEqual(
            OnNext(ScheduledAt(200, 1), 42),
            OnNext(ScheduledAt(200, 2), 42),
            OnNext(ScheduledAt(200, 3), 42),
            OnNext(ScheduledAt(200, 4), 42),
            OnNext(ScheduledAt(200, 5), 42),
            OnNext(ScheduledAt(200, 6), 42),
            OnNext(ScheduledAt(200, 7), 42),
            OnNext(ScheduledAt(200, 8), 42),
            OnNext(ScheduledAt(200, 9), 42),
            OnNext(ScheduledAt(200, 10), 42),
            OnCompleted<int>(ScheduledAt(200, 10))
        );
    }

    [TestMethod]
    public void Repeat_Value_Count_Dispose()
    {
        var scheduler = Scheduler;

        var res = scheduler.Start(() =>
            Seq.Repeat(42, 10, scheduler).Take(6)
        );

        res.Messages.AssertEqual(
            OnNext(ScheduledAt(200, 1), 42),
            OnNext(ScheduledAt(200, 2), 42),
            OnNext(ScheduledAt(200, 3), 42),
            OnNext(ScheduledAt(200, 4), 42),
            OnNext(ScheduledAt(200, 5), 42),
            OnNext(ScheduledAt(200, 6), 42),
            OnCompleted<int>(ScheduledAt(200, 6))
        );
    }

    [TestMethod]
    public async Task Repeat_Value_Count_Default()
    {
        Assert.IsTrue(
            (await ToListAsync(Seq.Repeat(42, 10)))
                .SequenceEqual(await ToListAsync(Seq.Repeat(42, 10, DefaultScheduler))));
    }

    [TestMethod]
    public void Repeat_Value()
    {
        var scheduler = Scheduler;

        var res = scheduler.Start(() =>
            Seq.Repeat(42, scheduler).Take(6)
        );

        res.Messages.AssertEqual(
            OnNext(ScheduledAt(200, 1), 42),
            OnNext(ScheduledAt(200, 2), 42),
            OnNext(ScheduledAt(200, 3), 42),
            OnNext(ScheduledAt(200, 4), 42),
            OnNext(ScheduledAt(200, 5), 42),
            OnNext(ScheduledAt(200, 6), 42),
            OnCompleted<int>(ScheduledAt(200, 6))
        );
    }

    [TestMethod]
    public async Task Repeat_Value_Default()
    {
        Assert.IsTrue(
            (await ToListAsync(Seq.Repeat(42).Take(100)))
                .SequenceEqual(await ToListAsync(Seq.Repeat(42, DefaultScheduler).Take(100))));
    }

    [TestMethod]
    public async Task Repeat_Count_LongRunning1()
    {
        var s = new TestLongRunningScheduler(Target);

        var xs = Seq.Repeat(42, 100, s);

        var lst = new List<int>();
        var done = false;
        await xs.SubscribeAsync(Scheduler, x => { lst.Add(x); }, ex => { }, () => done = true);

        await s.Stopped;

        Assert.IsTrue(lst.SequenceEqual(Enumerable.Repeat(42, 100)));
        Assert.IsTrue(done);
    }

    [TestMethod]
    public async Task Repeat_Count_LongRunning2()
    {
        var s = new TestLongRunningScheduler(Target);

        var xs = Seq.Repeat(42, int.MaxValue, s);

        var lst = new List<int>();
        var hundred = new ManualResetEvent(false);
        var d = await xs.SubscribeAsync(
            Scheduler,
            x => { lst.Add(x); if (lst.Count == 100) { hundred.Set(); } });

        await s.Started;

        hundred.WaitOne();

        await d.DisposeAsync();
        await s.Stopped;
    }

    [TestMethod]
    public async Task Repeat_Inf_LongRunning()
    {
        var s = new TestLongRunningScheduler(Target);

        var xs = Seq.Repeat(42, s);

        var lst = new List<int>();
        var hundred = new ManualResetEvent(false);
        var d = await xs.SubscribeAsync(
            Scheduler,
            x => { lst.Add(x); if (lst.Count == 100) { hundred.Set(); } });

        await s.Started;

        hundred.WaitOne();

        await d.DisposeAsync();
        await s.Stopped;
    }

    [TestMethod]
    public void Repeat_Observable_Basic()
    {
        var scheduler = Scheduler;

        var xs = scheduler.CreateColdObservable(
            OnNext(100, 1),
            OnNext(150, 2),
            OnNext(200, 3),
            OnCompleted<int>(250)
        );

        var res = scheduler.Start(() =>
            xs.Repeat()
        );

        res.Messages.AssertEqual(
            OnNext(300, 1),
            OnNext(350, 2),
            OnNext(400, 3),
            OnNext(550, 1),
            OnNext(600, 2),
            OnNext(650, 3),
            OnNext(800, 1),
            OnNext(850, 2),
            OnNext(900, 3)
        );

        xs.Subscriptions.AssertEqual(
            Subscribe(200, 450),
            Subscribe(450, 700),
            Subscribe(700, 950),
            Subscribe(950, 1000)
        );
    }

    [TestMethod]
    public void Repeat_Observable_Infinite()
    {
        var scheduler = Scheduler;

        var xs = scheduler.CreateColdObservable(
            OnNext(100, 1),
            OnNext(150, 2),
            OnNext(200, 3)
        );

        var res = scheduler.Start(() =>
            xs.Repeat()
        );

        res.Messages.AssertEqual(
            OnNext(300, 1),
            OnNext(350, 2),
            OnNext(400, 3)
        );

        xs.Subscriptions.AssertEqual(
            Subscribe(200, 1000)
        );
    }

    [TestMethod]
    public void Repeat_Observable_Error()
    {
        var scheduler = Scheduler;

        var ex = new Exception();

        var xs = scheduler.CreateColdObservable(
            OnNext(100, 1),
            OnNext(150, 2),
            OnNext(200, 3),
            OnError<int>(250, ex)
        );

        var res = scheduler.Start(() =>
            xs.Repeat()
        );

        res.Messages.AssertEqual(
            OnNext(300, 1),
            OnNext(350, 2),
            OnNext(400, 3),
            OnError<int>(450, ex)
        );

        xs.Subscriptions.AssertEqual(
            Subscribe(200, 450)
        );
    }

    [TestMethod]
    public async Task Repeat_Observable_Throws()
    {
        var scheduler1 = new TestSchedulerRef(Target);

        var xs = Seq.Return(1, scheduler1).Repeat();

        scheduler1.ScheduleAbsolute(100, async () =>
            await xs.SubscribeAsync(scheduler1, x => { throw new InvalidOperationException(); }));

        Assert.ThrowsExactly<InvalidOperationException>(() => scheduler1.Start());

        var scheduler2 = new TestSchedulerRef(Target);

        var ys = Seq.Throw<int>(new Exception(), scheduler2).Repeat();

        scheduler2.ScheduleAbsolute(100, async () =>
            await ys.SubscribeAsync(
                scheduler2,
                x => { },
                ex => { throw new InvalidOperationException(); }));

        Assert.ThrowsExactly<InvalidOperationException>(() => scheduler2.Start());

        var scheduler3 = new TestSchedulerRef(Target);

        var zs = scheduler3.CreateColdObservable(OnNext(1, 1), OnCompleted<int>(1)).Repeat();

        var d = await zs.SubscribeAsync(
            scheduler3,
            x => { },
            ex => { },
            () => { throw new InvalidOperationException(); });

        scheduler3.ScheduleAbsolute(210, async () => await d.DisposeAsync());

        scheduler3.Start();

        var xss = Seq.Create(new Func<ObserverRef<int>, ValueTask<Action?>>(o =>
        {
            throw new InvalidOperationException();
        })).Repeat();

        await Assert.ThrowsExactlyAsync<InvalidOperationException>(async () =>
            await xss.SubscribeAsync(Scheduler, _ => { }));
    }

    [TestMethod]
    public void Repeat_Observable_RepeatCount_Basic()
    {
        var scheduler = Scheduler;

        var xs = scheduler.CreateColdObservable(
            OnNext(5, 1),
            OnNext(10, 2),
            OnNext(15, 3),
            OnCompleted<int>(20)
        );

        var res = scheduler.Start(() =>
            xs.Repeat(3)
        );

        res.Messages.AssertEqual(
            OnNext(205, 1),
            OnNext(210, 2),
            OnNext(215, 3),
            OnNext(225, 1),
            OnNext(230, 2),
            OnNext(235, 3),
            OnNext(245, 1),
            OnNext(250, 2),
            OnNext(255, 3),
            OnCompleted<int>(260)
        );

        xs.Subscriptions.AssertEqual(
            Subscribe(200, 220),
            Subscribe(220, 240),
            Subscribe(240, 260)
        );
    }

    [TestMethod]
    public void Repeat_Observable_RepeatCount_Dispose()
    {
        var scheduler = Scheduler;

        var xs = scheduler.CreateColdObservable(
            OnNext(5, 1),
            OnNext(10, 2),
            OnNext(15, 3),
            OnCompleted<int>(20)
        );

        var res = scheduler.Start(() =>
            xs.Repeat(3), 231
        );

        res.Messages.AssertEqual(
            OnNext(205, 1),
            OnNext(210, 2),
            OnNext(215, 3),
            OnNext(225, 1),
            OnNext(230, 2)
        );

        xs.Subscriptions.AssertEqual(
            Subscribe(200, 220),
            Subscribe(220, 231)
        );
    }

    [TestMethod]
    public void Repeat_Observable_RepeatCount_Infinite()
    {
        var scheduler = Scheduler;

        var xs = scheduler.CreateColdObservable(
            OnNext(100, 1),
            OnNext(150, 2),
            OnNext(200, 3)
        );

        var res = scheduler.Start(() =>
            xs.Repeat(3)
        );

        res.Messages.AssertEqual(
            OnNext(300, 1),
            OnNext(350, 2),
            OnNext(400, 3)
        );

        xs.Subscriptions.AssertEqual(
            Subscribe(200, 1000)
        );
    }

    [TestMethod]
    public void Repeat_Observable_RepeatCount_Error()
    {
        var scheduler = Scheduler;

        var ex = new Exception();

        var xs = scheduler.CreateColdObservable(
            OnNext(100, 1),
            OnNext(150, 2),
            OnNext(200, 3),
            OnError<int>(250, ex)
        );

        var res = scheduler.Start(() =>
            xs.Repeat(3)
        );

        res.Messages.AssertEqual(
            OnNext(300, 1),
            OnNext(350, 2),
            OnNext(400, 3),
            OnError<int>(450, ex)
        );

        xs.Subscriptions.AssertEqual(
            Subscribe(200, 450)
        );
    }

    [TestMethod]
    public async Task Repeat_Observable_RepeatCount_Throws()
    {
        var scheduler1 = new TestSchedulerRef(Target);

        var xs = Seq.Return(1, scheduler1).Repeat(3);

        scheduler1.ScheduleAbsolute(100, async () =>
            await xs.SubscribeAsync(scheduler1, x => { throw new InvalidOperationException(); }));

        Assert.ThrowsExactly<InvalidOperationException>(() => scheduler1.Start());

        var scheduler2 = new TestSchedulerRef(Target);

        var ys = Seq.Throw<int>(new Exception(), scheduler2).Repeat(3);

        scheduler2.ScheduleAbsolute(100, async () =>
            await ys.SubscribeAsync(
                scheduler2,
                x => { },
                ex => { throw new InvalidOperationException(); }));

        Assert.ThrowsExactly<InvalidOperationException>(() => scheduler2.Start());

        var scheduler3 = new TestSchedulerRef(Target);

        var zs = scheduler3.CreateColdObservable(OnNext(1, 1), OnCompleted<int>(1)).Repeat(100);

        var d = await zs.SubscribeAsync(
            scheduler3,
            x => { },
            ex => { },
            () => { throw new InvalidOperationException(); });

        scheduler3.ScheduleAbsolute(10, async () => await d.DisposeAsync());

        scheduler3.Start();

        var xss = Seq.Create(new Func<ObserverRef<int>, ValueTask<Action?>>(o =>
        {
            throw new InvalidOperationException();
        })).Repeat(3);

        await Assert.ThrowsExactlyAsync<InvalidOperationException>(async () =>
            await xss.SubscribeAsync(Scheduler, _ => { }));
    }
}

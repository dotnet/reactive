// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information. 

namespace Tests.System.Reactive.Shared.Scenarios;

/// <summary>Shared <c>Range</c> scenarios, from Rx.NET's <c>RangeTest.cs</c>.</summary>
/// <remarks>
/// <para>
/// 11 of the file's 13 tests (the two <c>*_ArgumentChecking</c> tests are the code-generated
/// stratum). Four transcribe with <c>ScheduledAt(200, n)</c> for the original's <c>200 + n</c>:
/// the virtual-time tests over the test scheduler, where each element and the completion is a
/// scheduling of its own, one tick each on Rx.NET and all within the tick on the pump.
/// <c>Range_Dispose</c> disposes from inside the handler of the third element where the
/// original disposes at tick 204, which on the pump is after the whole range, delivered within
/// tick 200, so the same three elements are what either target delivers before the
/// disposal; the delivery of the remaining two is what the disposal stops.
/// <c>Range_Default</c> compares the default-scheduler form with the explicit one through
/// <see cref="SharedReactiveTest.ToListAsync{T}"/>, a hundred times as the original does,
/// where the original compares with a blocking <c>AssertEqual</c>.
/// </para>
/// <para>
/// The five <c>Range_LongRunning*</c> tests run over the shared
/// <see cref="TestLongRunningScheduler"/> in the <c>Repeat</c> scenarios' shapes (Step 31):
/// awaiting <see cref="TestLongRunningScheduler.Started"/> and
/// <see cref="TestLongRunningScheduler.Stopped"/> where the original blocks on its events, an
/// event set by the hundredth element where the original spins on the list's count, and
/// <c>ToListAsync</c> where the original blocks on <c>ForEach</c>.
/// </para>
/// </remarks>
public abstract class RangeTests : SharedReactiveTest
{
    [TestMethod]
    public void Range_Zero()
    {
        var scheduler = Scheduler;

        var res = scheduler.Start(() =>
            Seq.Range(0, 0, scheduler)
        );

        res.Messages.AssertEqual(
            OnCompleted<int>(ScheduledAt(200, 1))
        );
    }

    [TestMethod]
    public void Range_One()
    {
        var scheduler = Scheduler;

        var res = scheduler.Start(() =>
            Seq.Range(0, 1, scheduler)
        );

        res.Messages.AssertEqual(
            OnNext(ScheduledAt(200, 1), 0),
            OnCompleted<int>(ScheduledAt(200, 2))
        );
    }

    [TestMethod]
    public void Range_Five()
    {
        var scheduler = Scheduler;

        var res = scheduler.Start(() =>
            Seq.Range(10, 5, scheduler)
        );

        res.Messages.AssertEqual(
            OnNext(ScheduledAt(200, 1), 10),
            OnNext(ScheduledAt(200, 2), 11),
            OnNext(ScheduledAt(200, 3), 12),
            OnNext(ScheduledAt(200, 4), 13),
            OnNext(ScheduledAt(200, 5), 14),
            OnCompleted<int>(ScheduledAt(200, 6))
        );
    }

    [TestMethod]
    public void Range_Boundaries()
    {
        var scheduler = Scheduler;

        var res = scheduler.Start(() =>
            Seq.Range(int.MaxValue, 1, scheduler)
        );

        res.Messages.AssertEqual(
            OnNext(ScheduledAt(200, 1), int.MaxValue),
            OnCompleted<int>(ScheduledAt(200, 2))
        );
    }

    [TestMethod]
    public void Range_Dispose()
    {
        var scheduler = Scheduler;

        IAsyncDisposable? d = null;

        var xs = Seq.Range(-10, 5, scheduler);

        var res = scheduler.CreateObserver<int>();

        scheduler.ScheduleAbsolute(200, async () =>
            d = await xs.SubscribeAsync(
                scheduler,
                async x =>
                {
                    await res.OnNextAsync(x);

                    if (x == -8)
                    {
                        await d!.DisposeAsync();
                    }
                },
                res.OnErrorAsync,
                res.OnCompletedAsync));

        scheduler.Start();

        res.Messages.AssertEqual(
            OnNext(ScheduledAt(200, 1), -10),
            OnNext(ScheduledAt(200, 2), -9),
            OnNext(ScheduledAt(200, 3), -8)
        );
    }

    [TestMethod]
    public async Task Range_Default()
    {
        for (var i = 0; i < 100; i++)
        {
            Assert.IsTrue(
                (await ToListAsync(Seq.Range(100, 100)))
                    .SequenceEqual(await ToListAsync(Seq.Range(100, 100, DefaultScheduler))));
        }
    }

    [TestMethod]
    public async Task Range_LongRunning1()
    {
        var s = new TestLongRunningScheduler(Target);

        var xs = Seq.Range(0, 100, s);

        var lst = new List<int>();
        var done = false;
        await xs.SubscribeAsync(Scheduler, x => { lst.Add(x); }, ex => { }, () => done = true);

        await s.Stopped;

        Assert.IsTrue(lst.SequenceEqual(Enumerable.Range(0, 100)));
        Assert.IsTrue(done);
    }

    [TestMethod]
    public async Task Range_LongRunning2()
    {
        var s = new TestLongRunningScheduler(Target);

        var xs = Seq.Range(0, int.MaxValue, s);

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
    public async Task Range_LongRunning_Empty()
    {
        var scheduler = new TestLongRunningScheduler(Target);

        var xs = Seq.Range(5, 0, scheduler);

        var lst = await ToListAsync(xs);

        Assert.IsTrue(lst.SequenceEqual(Enumerable.Range(5, 0)));
    }

    [TestMethod]
    public async Task Range_LongRunning_Regular()
    {
        var scheduler = new TestLongRunningScheduler(Target);

        var xs = Seq.Range(5, 17, scheduler);

        var lst = await ToListAsync(xs);

        Assert.IsTrue(lst.SequenceEqual(Enumerable.Range(5, 17)));
    }

    [TestMethod]
    public async Task Range_LongRunning_Boundaries()
    {
        var scheduler = new TestLongRunningScheduler(Target);

        var xs = Seq.Range(int.MaxValue, 1, scheduler);

        var lst = await ToListAsync(xs);

        Assert.IsTrue(lst.SequenceEqual(Enumerable.Range(int.MaxValue, 1)));
    }
}

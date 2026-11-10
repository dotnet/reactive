// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information. 

namespace Tests.System.Reactive.Shared.Scenarios;

/// <summary>Shared <c>Generate</c> scenarios, from Rx.NET's <c>GenerateTest.cs</c>.</summary>
/// <remarks>
/// <para>
/// 25 of the file's 31 tests (the six <c>*_ArgumentChecking</c> tests are the code-generated
/// stratum). Sixteen transcribe with <c>ScheduledAt</c> for the test scheduler's tick bump: the
/// three families (no time selector, a relative one, an absolute one) over a finite run, a
/// condition, result selector, iterate step or time selector that throws, and, for the timed
/// families, a disposal part-way. Without a time selector each element and the completion is a
/// scheduling of its own (<c>ScheduledAt(200, n)</c>); with one, each element comes after its
/// own delay (<c>ScheduledAt(200) + n</c>, the delays summed) and the completion with the last
/// element.
/// </para>
/// <para>
/// <c>Generate_Dispose</c> disposes from the second element's handler where the original disposes
/// at tick 203, which on the pump is after the whole loop, run within tick 200; the timed
/// <c>*_Dispose</c> tests keep their tick, since there each element waits its delay. The three
/// <c>*_DefaultScheduler</c> tests compare the default-scheduler form with the explicit one
/// through <see cref="SharedReactiveTest.ToListAsync{T}"/>; the three <c>*_LongRunning*</c> tests
/// run over the shared <see cref="TestLongRunningScheduler"/> in the <c>Range</c> shapes (Step
/// 50). The two <c>*_DisposeLater</c> tests are Rx.NET's regression tests for a crash on
/// disposing a long timed sequence; they take ten thousand elements of a zero-delay generator
/// through the originals' <c>WithLatestFrom</c> and let <c>Take</c> dispose it, where the
/// originals count to ten thousand in a handler and dispose by hand (Step 73 restored the
/// <c>WithLatestFrom</c>, which the model lacked when the tests were migrated).
/// </para>
/// </remarks>
public abstract class GenerateTests : SharedReactiveTest
{
    [TestMethod]
    public void Generate_Finite()
    {
        var scheduler = Scheduler;

        var res = scheduler.Start(() =>
            Seq.Generate(0, x => x <= 3, x => x + 1, x => x, scheduler)
        );

        res.Messages.AssertEqual(
            OnNext(ScheduledAt(200, 1), 0),
            OnNext(ScheduledAt(200, 2), 1),
            OnNext(ScheduledAt(200, 3), 2),
            OnNext(ScheduledAt(200, 4), 3),
            OnCompleted<int>(ScheduledAt(200, 5))
        );
    }

    [TestMethod]
    public void Generate_Throw_Condition()
    {
        var scheduler = Scheduler;

        var ex = new Exception();

        var res = scheduler.Start(() =>
            Seq.Generate(0, new Func<int, bool>(x => { throw ex; }), x => x + 1, x => x, scheduler)
        );

        res.Messages.AssertEqual(
            OnError<int>(ScheduledAt(200, 1), ex)
        );
    }

    [TestMethod]
    public void Generate_Throw_ResultSelector()
    {
        var scheduler = Scheduler;

        var ex = new Exception();

        var res = scheduler.Start(() =>
            Seq.Generate(
                0,
                x => true,
                x => x + 1,
                new Func<int,
                int>(x => { throw ex; }),
                scheduler)
        );

        res.Messages.AssertEqual(
            OnError<int>(ScheduledAt(200, 1), ex)
        );
    }

    [TestMethod]
    public void Generate_Throw_Iterate()
    {
        var scheduler = Scheduler;

        var ex = new Exception();

        var res = scheduler.Start(() =>
            Seq.Generate(0, x => true, new Func<int, int>(x => { throw ex; }), x => x, scheduler)
        );

        res.Messages.AssertEqual(
            OnNext(ScheduledAt(200, 1), 0),
            OnError<int>(ScheduledAt(200, 2), ex)
        );
    }

    [TestMethod]
    public void Generate_Dispose()
    {
        var scheduler = Scheduler;

        IAsyncDisposable? d = null;

        var xs = Seq.Generate(0, x => true, x => x + 1, x => x, scheduler);

        var res = scheduler.CreateObserver<int>();

        scheduler.ScheduleAbsolute(200, async () =>
            d = await xs.SubscribeAsync(
                scheduler,
                async x =>
                {
                    await res.OnNextAsync(x);

                    if (x == 1)
                    {
                        await d!.DisposeAsync();
                    }
                },
                res.OnErrorAsync,
                res.OnCompletedAsync));

        scheduler.Start();

        res.Messages.AssertEqual(
            OnNext(ScheduledAt(200, 1), 0),
            OnNext(ScheduledAt(200, 2), 1)
        );
    }

    [TestMethod]
    public async Task Generate_DefaultScheduler()
    {
        Assert.IsTrue(
            (await ToListAsync(Seq.Generate(0, x => x < 10, x => x + 1, x => x)))
                .SequenceEqual(await ToListAsync(
                    Seq.Generate(0, x => x < 10, x => x + 1, x => x, DefaultScheduler))));
    }

    [TestMethod]
    public async Task Generate_LongRunning1()
    {
        var s = new TestLongRunningScheduler(Target);

        var xs = Seq.Generate(0, x => x < 100, x => x + 1, x => x, s);

        var lst = new List<int>();
        var done = false;
        await xs.SubscribeAsync(Scheduler, x => { lst.Add(x); }, ex => { }, () => done = true);

        await s.Stopped;

        Assert.IsTrue(lst.SequenceEqual(Enumerable.Range(0, 100)));
        Assert.IsTrue(done);
    }

    [TestMethod]
    public async Task Generate_LongRunning2()
    {
        var s = new TestLongRunningScheduler(Target);

        var xs = Seq.Generate(0, _ => true, x => x + 1, x => x, s);

        var lst = new List<int>();
        var hundred = new ManualResetEvent(false);
        var d = await xs.SubscribeAsync(
            Scheduler,
            x => { lst.Add(x); if (lst.Count == 100) { hundred.Set(); } });

        await s.Started;

        hundred.WaitOne();

        await d.DisposeAsync();
        await s.Stopped;

        Assert.IsTrue(lst.Take(100).SequenceEqual(Enumerable.Range(0, 100)));
    }

    [TestMethod]
    public async Task Generate_LongRunning_Throw()
    {
        var s = new TestLongRunningScheduler(Target);

        var ex = new Exception();

        var xs = Seq.Generate(
            0,
            x => { if (x < 100) { return true; } throw ex; },
            x => x + 1,
            x => x,
            s);

        var lst = new List<int>();
        var e = default(Exception);
        var done = false;
        await xs.SubscribeAsync(Scheduler, x => { lst.Add(x); }, e_ => e = e_, () => done = true);

        await s.Stopped;

        Assert.IsTrue(lst.SequenceEqual(Enumerable.Range(0, 100)));
        Assert.AreSame(ex, e);
        Assert.IsFalse(done);
    }

    [TestMethod]
    public void Generate_TimeSpan_Finite()
    {
        var scheduler = Scheduler;

        var res = scheduler.Start(() =>
            Seq.Generate(
                0,
                x => x <= 3,
                x => x + 1,
                x => x,
                x => TimeSpan.FromTicks(x + 1),
                scheduler)
        );

        res.Messages.AssertEqual(
            OnNext(ScheduledAt(200) + 1, 0),
            OnNext(ScheduledAt(200) + 3, 1),
            OnNext(ScheduledAt(200) + 6, 2),
            OnNext(ScheduledAt(200) + 10, 3),
            OnCompleted<int>(ScheduledAt(200) + 10)
        );
    }

    [TestMethod]
    public void Generate_TimeSpan_Throw_Condition()
    {
        var scheduler = Scheduler;

        var ex = new Exception();

        var res = scheduler.Start(() =>
            Seq.Generate(
                0,
                new Func<int,
                bool>(x => { throw ex; }),
                x => x + 1,
                x => x,
                x => TimeSpan.FromTicks(x + 1),
                scheduler)
        );

        res.Messages.AssertEqual(
            OnError<int>(ScheduledAt(200), ex)
        );
    }

    [TestMethod]
    public void Generate_TimeSpan_Throw_ResultSelector()
    {
        var scheduler = Scheduler;

        var ex = new Exception();

        var res = scheduler.Start(() =>
            Seq.Generate(
                0,
                x => true,
                x => x + 1,
                new Func<int,
                int>(x => { throw ex; }),
                x => TimeSpan.FromTicks(x + 1),
                scheduler)
        );

        res.Messages.AssertEqual(
            OnError<int>(ScheduledAt(200), ex)
        );
    }

    [TestMethod]
    public void Generate_TimeSpan_Throw_Iterate()
    {
        var scheduler = Scheduler;

        var ex = new Exception();

        var res = scheduler.Start(() =>
            Seq.Generate(
                0,
                x => true,
                new Func<int,
                int>(x => { throw ex; }),
                x => x,
                x => TimeSpan.FromTicks(x + 1),
                scheduler)
        );

        res.Messages.AssertEqual(
            OnNext(ScheduledAt(200) + 1, 0),
            OnError<int>(ScheduledAt(200) + 1, ex)
        );
    }

    [TestMethod]
    public void Generate_TimeSpan_Throw_TimeSelector()
    {
        var scheduler = Scheduler;

        var ex = new Exception();

        var res = scheduler.Start(() =>
            Seq.Generate(
                0,
                x => true,
                x => x + 1,
                x => x,
                new Func<int,
                TimeSpan>(x => { throw ex; }),
                scheduler)
        );

        res.Messages.AssertEqual(
            OnError<int>(ScheduledAt(200), ex)
        );
    }

    [TestMethod]
    public void Generate_TimeSpan_Dispose()
    {
        var scheduler = Scheduler;

        var res = scheduler.Start(() =>
            Seq.Generate(
                0,
                x => true,
                x => x + 1,
                x => x,
                x => TimeSpan.FromTicks(x + 1),
                scheduler),
            210
        );

        res.Messages.AssertEqual(
            OnNext(ScheduledAt(200) + 1, 0),
            OnNext(ScheduledAt(200) + 3, 1),
            OnNext(ScheduledAt(200) + 6, 2)
        );
    }

    [TestMethod]
    public async Task Generate_TimeSpan_DefaultScheduler()
    {
        Assert.IsTrue(
            (await ToListAsync(
                Seq.Generate(
                    0,
                    x => x < 10,
                    x => x + 1,
                    x => x,
                    x => TimeSpan.FromMilliseconds(x))))
                .SequenceEqual(await ToListAsync(
                    Seq.Generate(
                        0,
                        x => x < 10,
                        x => x + 1,
                        x => x,
                        x => TimeSpan.FromMilliseconds(x),
                        DefaultScheduler))));
    }

    [TestMethod]
    public void Generate_DateTimeOffset_Finite()
    {
        var scheduler = Scheduler;

        var res = scheduler.Start(() =>
            Seq.Generate(
                0,
                x => x <= 3,
                x => x + 1,
                x => x,
                x => scheduler.Now.AddTicks(x + 1),
                scheduler)
        );

        res.Messages.AssertEqual(
            OnNext(ScheduledAt(200) + 1, 0),
            OnNext(ScheduledAt(200) + 3, 1),
            OnNext(ScheduledAt(200) + 6, 2),
            OnNext(ScheduledAt(200) + 10, 3),
            OnCompleted<int>(ScheduledAt(200) + 10)
        );
    }

    [TestMethod]
    public void Generate_DateTimeOffset_Throw_Condition()
    {
        var scheduler = Scheduler;

        var ex = new Exception();

        var res = scheduler.Start(() =>
            Seq.Generate(
                0,
                new Func<int,
                bool>(x => { throw ex; }),
                x => x + 1,
                x => x,
                x => scheduler.Now.AddTicks(x + 1),
                scheduler)
        );

        res.Messages.AssertEqual(
            OnError<int>(ScheduledAt(200), ex)
        );
    }

    [TestMethod]
    public void Generate_DateTimeOffset_Throw_ResultSelector()
    {
        var scheduler = Scheduler;

        var ex = new Exception();

        var res = scheduler.Start(() =>
            Seq.Generate(
                0,
                x => true,
                x => x + 1,
                new Func<int,
                int>(x => { throw ex; }),
                x => scheduler.Now.AddTicks(x + 1),
                scheduler)
        );

        res.Messages.AssertEqual(
            OnError<int>(ScheduledAt(200), ex)
        );
    }

    [TestMethod]
    public void Generate_DateTimeOffset_Throw_Iterate()
    {
        var scheduler = Scheduler;

        var ex = new Exception();

        var res = scheduler.Start(() =>
            Seq.Generate(
                0,
                x => true,
                new Func<int,
                int>(x => { throw ex; }),
                x => x,
                x => scheduler.Now.AddTicks(x + 1),
                scheduler)
        );

        res.Messages.AssertEqual(
            OnNext(ScheduledAt(200) + 1, 0),
            OnError<int>(ScheduledAt(200) + 1, ex)
        );
    }

    [TestMethod]
    public void Generate_DateTimeOffset_Throw_TimeSelector()
    {
        var scheduler = Scheduler;

        var ex = new Exception();

        var res = scheduler.Start(() =>
            Seq.Generate(
                0,
                x => true,
                x => x + 1,
                x => x,
                new Func<int,
                DateTimeOffset>(x => { throw ex; }),
                scheduler)
        );

        res.Messages.AssertEqual(
            OnError<int>(ScheduledAt(200), ex)
        );
    }

    [TestMethod]
    public void Generate_DateTimeOffset_Dispose()
    {
        var scheduler = Scheduler;

        var res = scheduler.Start(() =>
            Seq.Generate(
                0,
                x => true,
                x => x + 1,
                x => x,
                x => scheduler.Now.AddTicks(x + 1),
                scheduler),
            210
        );

        res.Messages.AssertEqual(
            OnNext(ScheduledAt(200) + 1, 0),
            OnNext(ScheduledAt(200) + 3, 1),
            OnNext(ScheduledAt(200) + 6, 2)
        );
    }

    [TestMethod]
    public async Task Generate_DateTimeOffset_DefaultScheduler()
    {
        Assert.IsTrue(
            (await ToListAsync(
                Seq.Generate(
                    0,
                    x => x < 10,
                    x => x + 1,
                    x => x,
                    x => DateTimeOffset.Now.AddMilliseconds(x))))
                .SequenceEqual(await ToListAsync(
                    Seq.Generate(
                        0,
                        x => x < 10,
                        x => x + 1,
                        x => x,
                        x => DateTimeOffset.Now.AddMilliseconds(x),
                        DefaultScheduler))));
    }

    [TestMethod]
    public async Task Generate_TimeSpan_DisposeLater()
    {
        var lst = await ToListAsync(
            Seq.Generate(0, i => true, i => i + 1, i => i, _ => TimeSpan.Zero)
                .WithLatestFrom(Seq.Return(1).Concat(Seq.Never<int>()), (a, b) => a)
                .Take(10000));

        Assert.HasCount(10000, lst);
    }

    [TestMethod]
    public async Task Generate_DateTimeOffset_DisposeLater()
    {
        var lst = await ToListAsync(
            Seq.Generate(0, i => true, i => i + 1, i => i, _ => DateTimeOffset.Now)
                .WithLatestFrom(Seq.Return(1).Concat(Seq.Never<int>()), (a, b) => a)
                .Take(10000));

        Assert.HasCount(10000, lst);
    }
}

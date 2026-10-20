// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information. 

namespace Tests.System.Reactive.Shared.Scenarios;

/// <summary>Shared <c>RepeatWhen</c> scenarios, from Rx.NET's <c>RepeatWhenTest.cs</c>.</summary>
/// <remarks>
/// <para>
/// 16 of the file's 17 tests (<c>RepeatWhen_ArgumentChecking</c> is the code-generated
/// stratum). The handler receives the completion signals as a <c>Seq&lt;object&gt;</c> and
/// returns a description, which the bridge adapts to the target's own delegate; the nine
/// virtual-time tests transcribe as they are. The three subject-driven tests make their
/// subjects through <c>CreateSubject</c>, await the subscribe and the drives, and read
/// <c>HasObservers</c> on the handles. The two <c>*_Throws</c> tests take the shapes the
/// <c>Retry</c> scenarios settled for handlers that throw under <c>Start</c>: one scheduler per
/// case as <c>new TestSchedulerRef(Target)</c>, the subscribe a scheduled action where the
/// handler must throw, and the case that repeats until a disposal repeating a cold sequence
/// with the original's one-tick timing in place of a scheduled <c>Return</c>, which moves
/// virtual time on both targets (on AsyncRx.NET's test scheduler a resubscribed
/// <c>Return</c> runs again in the same tick, and the repeats never end or, counted, all
/// happen before the disposal).
/// </para>
/// </remarks>
public abstract class RepeatWhenTests : SharedReactiveTest
{
    [TestMethod]
    public void RepeatWhen_Handler_Crash()
    {
        var xs = Scheduler.CreateColdObservable(
            OnCompleted<int>(10)
        );

        var ex = new InvalidOperationException();

        var res = Scheduler.Start(() =>
            xs.RepeatWhen<int, object>(v => { throw ex; })
        );

        res.Messages.AssertEqual(
            OnError<int>(200, ex)
        );

        xs.Subscriptions.AssertEqual(
        );
    }

    [TestMethod]
    public void RepeatWhen_Handler_Error()
    {
        var xs = Scheduler.CreateColdObservable(
            OnCompleted<int>(10)
        );

        var ex = new InvalidOperationException();

        var res = Scheduler.Start(() =>
            xs.RepeatWhen(v => v.Select<object, object>(w => throw ex))
        );

        res.Messages.AssertEqual(
            OnError<int>(210, ex)
        );

        xs.Subscriptions.AssertEqual(
            Subscribe(200, 210)
        );
    }

    [TestMethod]
    public void RepeatWhen_Handler_Completed()
    {
        var xs = Scheduler.CreateColdObservable(
            OnCompleted<int>(10)
        );

        var ex = new InvalidOperationException();

        var res = Scheduler.Start(() =>
            xs.RepeatWhen(v => v.Take(1).Skip(1))
        );

        res.Messages.AssertEqual(
            OnCompleted<int>(210)
        );

        xs.Subscriptions.AssertEqual(
            Subscribe(200, 210)
        );
    }

    [TestMethod]
    public async Task RepeatWhen_Disposed()
    {
        var main = CreateSubject<int>();
        var inner = CreateSubject<int>();

        var d = await main.RepeatWhen(v => inner).SubscribeAsync(Scheduler, x => { });

        Assert.IsTrue(main.HasObservers);
        Assert.IsTrue(inner.HasObservers);

        await d.DisposeAsync();

        Assert.IsFalse(main.HasObservers);
        Assert.IsFalse(inner.HasObservers);
    }

    [TestMethod]
    public async Task RepeatWhen_Handler_Completed_Disposes_Main()
    {
        var main = CreateSubject<int>();
        var inner = CreateSubject<int>();

        var end = 0;
        var items = 0;
        var errors = 0;

        await main.RepeatWhen(v => inner).SubscribeAsync(
            Scheduler,
            v => items++,
            e => errors++,
            () => end++);

        Assert.IsTrue(main.HasObservers);
        Assert.IsTrue(inner.HasObservers);

        await inner.OnCompletedAsync();

        Assert.IsFalse(main.HasObservers);
        Assert.IsFalse(inner.HasObservers);

        Assert.AreEqual(0, items);
        Assert.AreEqual(0, errors);
        Assert.AreEqual(1, end);
    }

    [TestMethod]
    public async Task RepeatWhen_Handler_Error_Disposes_Main()
    {
        var main = CreateSubject<int>();
        var inner = CreateSubject<int>();

        var end = 0;
        var items = 0;
        var errors = 0;

        await main.RepeatWhen(v => inner).SubscribeAsync(
            Scheduler,
            v => items++,
            e => errors++,
            () => end++);

        Assert.IsTrue(main.HasObservers);
        Assert.IsTrue(inner.HasObservers);

        await inner.OnErrorAsync(new InvalidOperationException());

        Assert.IsFalse(main.HasObservers);
        Assert.IsFalse(inner.HasObservers);

        Assert.AreEqual(0, items);
        Assert.AreEqual(1, errors);
        Assert.AreEqual(0, end);
    }

    [TestMethod]
    public void RepeatWhen_Basic()
    {
        var xs = Scheduler.CreateColdObservable(
            OnNext(100, 1),
            OnNext(150, 2),
            OnNext(200, 3),
            OnCompleted<int>(250)
        );

        var res = Scheduler.Start(() =>
            xs.RepeatWhen(v => v)
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
    public void RepeatWhen_Infinite()
    {
        var xs = Scheduler.CreateColdObservable(
            OnNext(100, 1),
            OnNext(150, 2),
            OnNext(200, 3)
        );

        var res = Scheduler.Start(() =>
            xs.RepeatWhen(v => v)
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
    public void RepeatWhen_Error()
    {
        var ex = new Exception();

        var xs = Scheduler.CreateColdObservable(
            OnNext(100, 1),
            OnNext(150, 2),
            OnNext(200, 3),
            OnError<int>(250, ex)
        );

        var res = Scheduler.Start(() =>
            xs.RepeatWhen(v => v)
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
    public async Task RepeatWhen_Throws()
    {
        var scheduler1 = new TestSchedulerRef(Target);

        var xs = Seq.Return(1, scheduler1).RepeatWhen(v => v);

        scheduler1.ScheduleAbsolute(100, async () =>
            await xs.SubscribeAsync(scheduler1, x => { throw new InvalidOperationException(); }));

        Assert.ThrowsExactly<InvalidOperationException>(() => scheduler1.Start());

        var scheduler2 = new TestSchedulerRef(Target);

        var ys = Seq.Throw<int>(new Exception(), scheduler2).RepeatWhen(v => v);

        scheduler2.ScheduleAbsolute(100, async () =>
            await ys.SubscribeAsync(
                scheduler2,
                x => { },
                ex => { throw new InvalidOperationException(); }));

        Assert.ThrowsExactly<InvalidOperationException>(() => scheduler2.Start());

        var scheduler3 = new TestSchedulerRef(Target);

        var zs = scheduler3.CreateColdObservable(OnNext(1, 1), OnCompleted<int>(1))
            .RepeatWhen(v => v);

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
        })).RepeatWhen(v => v);

        await Assert.ThrowsExactlyAsync<InvalidOperationException>(async () =>
            await xss.SubscribeAsync(Scheduler, _ => { }));
    }

    [TestMethod]
    public void RepeatWhen_RepeatCount_Basic()
    {
        var xs = Scheduler.CreateColdObservable(
            OnNext(5, 1),
            OnNext(10, 2),
            OnNext(15, 3),
            OnCompleted<int>(20)
        );

        var res = Scheduler.Start(() =>
            xs.RepeatWhen(v =>
            {
                var count = 0;
                return v.TakeWhile(w => ++count < 3);
            })
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
    public void RepeatWhen_RepeatCount_Dispose()
    {
        var xs = Scheduler.CreateColdObservable(
            OnNext(5, 1),
            OnNext(10, 2),
            OnNext(15, 3),
            OnCompleted<int>(20)
        );

        var res = Scheduler.Start(() =>
            xs.RepeatWhen(v =>
            {
                var count = 0;
                return v.TakeWhile(w => ++count < 3);
            }), 231
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
    public void RepeatWhen_RepeatCount_Infinite()
    {
        var xs = Scheduler.CreateColdObservable(
            OnNext(100, 1),
            OnNext(150, 2),
            OnNext(200, 3)
        );

        var res = Scheduler.Start(() =>
            xs.RepeatWhen(v =>
            {
                var count = 0;
                return v.TakeWhile(w => ++count < 3);
            })
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
    public void RepeatWhen_RepeatCount_Error()
    {
        var ex = new Exception();

        var xs = Scheduler.CreateColdObservable(
            OnNext(100, 1),
            OnNext(150, 2),
            OnNext(200, 3),
            OnError<int>(250, ex)
        );

        var res = Scheduler.Start(() =>
            xs.RepeatWhen(v =>
            {
                var count = 0;
                return v.TakeWhile(w => ++count < 3);
            })
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
    public async Task RepeatWhen_RepeatCount_Throws()
    {
        var scheduler1 = new TestSchedulerRef(Target);

        var xs = Seq.Return(1, scheduler1).RepeatWhen(v =>
        {
            var count = 0;
            return v.TakeWhile(w => ++count < 3);
        });

        scheduler1.ScheduleAbsolute(100, async () =>
            await xs.SubscribeAsync(scheduler1, x => { throw new InvalidOperationException(); }));

        Assert.ThrowsExactly<InvalidOperationException>(() => scheduler1.Start());

        var scheduler2 = new TestSchedulerRef(Target);

        var ys = Seq.Throw<int>(new Exception(), scheduler2).RepeatWhen(v =>
        {
            var count = 0;
            return v.TakeWhile(w => ++count < 3);
        });

        scheduler2.ScheduleAbsolute(100, async () =>
            await ys.SubscribeAsync(
                scheduler2,
                x => { },
                ex => { throw new InvalidOperationException(); }));

        Assert.ThrowsExactly<InvalidOperationException>(() => scheduler2.Start());

        var scheduler3 = new TestSchedulerRef(Target);

        var zs = scheduler3.CreateColdObservable(OnNext(1, 1), OnCompleted<int>(1))
            .RepeatWhen(v =>
        {
            var count = 0;
            return v.TakeWhile(w => ++count < 100);
        });

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
        })).RepeatWhen(v =>
        {
            var count = 0;
            return v.TakeWhile(w => ++count < 3);
        });

        await Assert.ThrowsExactlyAsync<InvalidOperationException>(async () =>
            await xss.SubscribeAsync(Scheduler, _ => { }));
    }

    [TestMethod]
    public void RepeatWhen_Observable_Repeat_Delayed()
    {
        var ex = new Exception();

        var xs = Scheduler.CreateColdObservable(
            OnNext(5, 1),
            OnNext(10, 2),
            OnNext(15, 3),
            OnCompleted<int>(20)
        );

        var res = Scheduler.Start(() =>
            xs.RepeatWhen(v =>
            {
                int[] count = [0];
                return v.SelectMany(w =>
                {
                    var c = ++count[0];
                    if (c == 3)
                    {
                        return Seq.Throw<int>(ex);
                    }
                    return Seq.Return(1).Delay(TimeSpan.FromTicks(c * 100), Scheduler);
                });
            })
        );

        res.Messages.AssertEqual(
            OnNext(205, 1),
            OnNext(210, 2),
            OnNext(215, 3),
            OnNext(325, 1),
            OnNext(330, 2),
            OnNext(335, 3),
            OnNext(545, 1),
            OnNext(550, 2),
            OnNext(555, 3),
            OnError<int>(560, ex)
        );

        xs.Subscriptions.AssertEqual(
            Subscribe(200, 220),
            Subscribe(320, 340),
            Subscribe(540, 560)
        );
    }
}

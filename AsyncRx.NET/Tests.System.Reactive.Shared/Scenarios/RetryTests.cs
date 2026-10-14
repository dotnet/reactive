// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information. 

namespace Tests.System.Reactive.Shared.Scenarios;

/// <summary>Shared <c>Retry</c> scenarios, from Rx.NET's <c>RetryTest.cs</c>.</summary>
/// <remarks>
/// <para>
/// 12 of the file's 16 live tests (the four <c>*_ArgumentChecking</c> tests are the
/// code-generated stratum; a seventeenth, <c>Retry_Observable_Throws4</c>, is commented out
/// upstream, with a note that a subscribe which throws has been retried without end since Rx
/// v2.0, and it does loop on the System.Reactive package this suite runs against). Seven
/// transcribe as they are: the virtual-time tests over <c>xs.Retry()</c> and <c>xs.Retry(3)</c>
/// with cold sources. The five that throw from handlers or compare the scheduler-less form
/// take the <c>Repeat</c> scenarios' shapes: where a handler must throw
/// under <c>Start</c>, the subscribe is a scheduled action, since a body-level subscribe runs
/// the current tick's work on the pump and the scheduled <c>Return</c> would deliver, and the
/// handler throw, there; the case that retries a scheduled <c>Throw</c> until a disposal
/// retries a cold sequence with the same one-tick timing instead, which moves virtual time on
/// both targets; a test with one scheduler per case makes each as
/// <c>new TestSchedulerRef(Target)</c>, since a scheduler whose <c>Start</c> has thrown is done;
/// and <c>Retry_Observable_RetryCount_Default</c> compares the two sides by collecting both
/// through <c>ToListAsync</c>.
/// </para>
/// </remarks>
public abstract class RetryTests : SharedReactiveTest
{
    [TestMethod]
    public void Retry_Observable_Basic()
    {
        var scheduler = Scheduler;

        var xs = scheduler.CreateColdObservable(
            OnNext(100, 1),
            OnNext(150, 2),
            OnNext(200, 3),
            OnCompleted<int>(250)
        );

        var res = scheduler.Start(() =>
            xs.Retry()
        );

        res.Messages.AssertEqual(
            OnNext(300, 1),
            OnNext(350, 2),
            OnNext(400, 3),
            OnCompleted<int>(450)
        );

        xs.Subscriptions.AssertEqual(
            Subscribe(200, 450)
        );
    }

    [TestMethod]
    public void Retry_Observable_Infinite()
    {
        var scheduler = Scheduler;

        var xs = scheduler.CreateColdObservable(
            OnNext(100, 1),
            OnNext(150, 2),
            OnNext(200, 3)
        );

        var res = scheduler.Start(() =>
            xs.Retry()
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
    public void Retry_Observable_Error()
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
            xs.Retry(), 1100
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
            OnNext(900, 3),
            OnNext(1050, 1)
        );

        xs.Subscriptions.AssertEqual(
            Subscribe(200, 450),
            Subscribe(450, 700),
            Subscribe(700, 950),
            Subscribe(950, 1100)
        );
    }

    [TestMethod]
    public async Task Retry_Observable_Throws1()
    {
        var xs = Seq.Return(1, Scheduler).Retry();

        Scheduler.ScheduleAbsolute(100, async () =>
            await xs.SubscribeAsync(Scheduler, x => { throw new InvalidOperationException(); }));

        Assert.ThrowsExactly<InvalidOperationException>(() => Scheduler.Start());
    }

    [TestMethod]
    public async Task Retry_Observable_Throws2()
    {
        var ys = Scheduler.CreateColdObservable(OnError<int>(1, new Exception())).Retry();

        var d = await ys.SubscribeAsync(
            Scheduler,
            x => { },
            ex => { throw new InvalidOperationException(); });

        Scheduler.ScheduleAbsolute(210, async () => await d.DisposeAsync());

        Scheduler.Start();
    }

    [TestMethod]
    public async Task Retry_Observable_Throws3()
    {
        var zs = Seq.Return(1, Scheduler).Retry();

        Scheduler.ScheduleAbsolute(100, async () =>
            await zs.SubscribeAsync(
                Scheduler,
                x => { },
                ex => { },
                () => { throw new InvalidOperationException(); }));

        Assert.ThrowsExactly<InvalidOperationException>(() => Scheduler.Start());
    }

    [TestMethod]
    public void Retry_Observable_RetryCount_Basic()
    {
        var scheduler = Scheduler;

        var ex = new Exception();

        var xs = scheduler.CreateColdObservable(
            OnNext(5, 1),
            OnNext(10, 2),
            OnNext(15, 3),
            OnError<int>(20, ex)
        );

        var res = scheduler.Start(() =>
            xs.Retry(3)
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
            OnError<int>(260, ex)
        );

        xs.Subscriptions.AssertEqual(
            Subscribe(200, 220),
            Subscribe(220, 240),
            Subscribe(240, 260)
        );
    }

    [TestMethod]
    public void Retry_Observable_RetryCount_Dispose()
    {
        var scheduler = Scheduler;

        var xs = scheduler.CreateColdObservable(
            OnNext(5, 1),
            OnNext(10, 2),
            OnNext(15, 3),
            OnError<int>(20, new Exception())
        );

        var res = scheduler.Start(() =>
            xs.Retry(3), 231
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
    public void Retry_Observable_RetryCount_Infinite()
    {
        var scheduler = Scheduler;

        var xs = scheduler.CreateColdObservable(
            OnNext(100, 1),
            OnNext(150, 2),
            OnNext(200, 3)
        );

        var res = scheduler.Start(() =>
            xs.Retry(3)
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
    public void Retry_Observable_RetryCount_Completed()
    {
        var scheduler = Scheduler;

        var xs = scheduler.CreateColdObservable(
            OnNext(100, 1),
            OnNext(150, 2),
            OnNext(200, 3),
            OnCompleted<int>(250)
        );

        var res = scheduler.Start(() =>
            xs.Retry(3)
        );

        res.Messages.AssertEqual(
            OnNext(300, 1),
            OnNext(350, 2),
            OnNext(400, 3),
            OnCompleted<int>(450)
        );

        xs.Subscriptions.AssertEqual(
            Subscribe(200, 450)
        );
    }

    [TestMethod]
    public async Task Retry_Observable_RetryCount_Throws()
    {
        var scheduler1 = new TestSchedulerRef(Target);

        var xs = Seq.Return(1, scheduler1).Retry(3);

        scheduler1.ScheduleAbsolute(100, async () =>
            await xs.SubscribeAsync(scheduler1, x => { throw new InvalidOperationException(); }));

        Assert.ThrowsExactly<InvalidOperationException>(() => scheduler1.Start());

        var scheduler2 = new TestSchedulerRef(Target);

        var ys = scheduler2.CreateColdObservable(OnError<int>(1, new Exception())).Retry(100);

        var d = await ys.SubscribeAsync(
            scheduler2,
            x => { },
            ex => { throw new InvalidOperationException(); });

        scheduler2.ScheduleAbsolute(10, async () => await d.DisposeAsync());

        scheduler2.Start();

        var scheduler3 = new TestSchedulerRef(Target);

        var zs = Seq.Return(1, scheduler3).Retry(100);

        scheduler3.ScheduleAbsolute(100, async () =>
            await zs.SubscribeAsync(
                scheduler3,
                x => { },
                ex => { },
                () => { throw new InvalidOperationException(); }));

        Assert.ThrowsExactly<InvalidOperationException>(() => scheduler3.Start());

        var xss = Seq.Create(new Func<ObserverRef<int>, ValueTask<Action?>>(o =>
        {
            throw new InvalidOperationException();
        })).Retry(3);

        await Assert.ThrowsExactlyAsync<InvalidOperationException>(async () =>
            await xss.SubscribeAsync(Scheduler, _ => { }));
    }

    [TestMethod]
    public async Task Retry_Observable_RetryCount_Default()
    {
        Assert.IsTrue(
            (await ToListAsync(Seq.Range(1, 3).Retry(3)))
                .SequenceEqual(await ToListAsync(Seq.Range(1, 3).Retry(3))));
    }
}

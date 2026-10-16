// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information. 

namespace Tests.System.Reactive.Shared.Scenarios;

/// <summary>Shared <c>TakeUntil</c> scenarios, from Rx.NET's <c>TakeUntilTest.cs</c>.</summary>
/// <remarks>
/// 28 of the file's 31 tests (the three <c>*_ArgumentChecking</c> tests are the code-generated
/// stratum). Twenty-six transcribe as they are: the other-sequence form, with the two that
/// compose <c>Do</c> onto a source to show it is heard from no more once preempted; the end-time
/// form, whose <c>TakeUntil_Zero</c> completes on a scheduler hop, written <c>ScheduledAt(200)</c>
/// for the original's 201; the stop-predicate form; and the cancellation-token form, whose token
/// is cancelled from a scheduled action. <c>TakeUntil_Default</c> runs the scheduler-less
/// end-time form on the real default scheduler and awaits the result through
/// <see cref="SharedReactiveTest.ToListAsync{T}"/> where the original blocks on an event.
/// </remarks>
public abstract class TakeUntilTests : SharedReactiveTest
{
    [TestMethod]
    public void TakeUntil_Preempt_SomeData_Next()
    {
        var scheduler = Scheduler;

        var l = scheduler.CreateHotObservable(
            OnNext(150, 1),
            OnNext(210, 2),
            OnNext(220, 3),
            OnNext(230, 4),
            OnNext(240, 5),
            OnCompleted<int>(250)
        );

        var r = scheduler.CreateHotObservable(
            OnNext(150, 1),
            OnNext(225, 99),
            OnCompleted<int>(230)
        );

        var res = scheduler.Start(() =>
            l.TakeUntil(r)
        );

        res.Messages.AssertEqual(
            OnNext(210, 2),
            OnNext(220, 3),
            OnCompleted<int>(225)
        );

        l.Subscriptions.AssertEqual(
            Subscribe(200, 225)
        );

        r.Subscriptions.AssertEqual(
            Subscribe(200, 225)
        );
    }

    [TestMethod]
    public void TakeUntil_Preempt_SomeData_Error()
    {
        var scheduler = Scheduler;

        var ex = new Exception();

        var l = scheduler.CreateHotObservable(
            OnNext(150, 1),
            OnNext(210, 2),
            OnNext(220, 3),
            OnNext(230, 4),
            OnNext(240, 5),
            OnCompleted<int>(250)
        );

        var r = scheduler.CreateHotObservable(
            OnNext(150, 1),
            OnError<int>(225, ex)
        );

        var res = scheduler.Start(() =>
            l.TakeUntil(r)
        );

        res.Messages.AssertEqual(
            OnNext(210, 2),
            OnNext(220, 3),
            OnError<int>(225, ex)
        );

        l.Subscriptions.AssertEqual(
            Subscribe(200, 225)
        );

        r.Subscriptions.AssertEqual(
            Subscribe(200, 225)
        );
    }

    [TestMethod]
    public void TakeUntil_NoPreempt_SomeData_Empty()
    {
        var scheduler = Scheduler;

        var l = scheduler.CreateHotObservable(
            OnNext(150, 1),
            OnNext(210, 2),
            OnNext(220, 3),
            OnNext(230, 4),
            OnNext(240, 5),
            OnCompleted<int>(250)
        );

        var r = scheduler.CreateHotObservable(
            OnNext(150, 1),
            OnCompleted<int>(225)
        );

        var res = scheduler.Start(() =>
            l.TakeUntil(r)
        );

        res.Messages.AssertEqual(
            OnNext(210, 2),
            OnNext(220, 3),
            OnNext(230, 4),
            OnNext(240, 5),
            OnCompleted<int>(250)
        );

        l.Subscriptions.AssertEqual(
            Subscribe(200, 250)
        );

        r.Subscriptions.AssertEqual(
            Subscribe(200, 225)
        );
    }

    [TestMethod]
    public void TakeUntil_NoPreempt_SomeData_Never()
    {
        var scheduler = Scheduler;

        var l = scheduler.CreateHotObservable(
            OnNext(150, 1),
            OnNext(210, 2),
            OnNext(220, 3),
            OnNext(230, 4),
            OnNext(240, 5),
            OnCompleted<int>(250)
        );

        var r = scheduler.CreateHotObservable(
            OnNext(150, 1)
        );

        var res = scheduler.Start(() =>
            l.TakeUntil(r)
        );

        res.Messages.AssertEqual(
            OnNext(210, 2),
            OnNext(220, 3),
            OnNext(230, 4),
            OnNext(240, 5),
            OnCompleted<int>(250)
        );

        l.Subscriptions.AssertEqual(
            Subscribe(200, 250)
        );

        r.Subscriptions.AssertEqual(
            Subscribe(200, 250)
        );
    }

    [TestMethod]
    public void TakeUntil_Preempt_Never_Next()
    {
        var scheduler = Scheduler;

        var l = scheduler.CreateHotObservable(
            OnNext(150, 1)
        );

        var r = scheduler.CreateHotObservable(
            OnNext(150, 1),
            OnNext(225, 2), //!
            OnCompleted<int>(250)
        );

        var res = scheduler.Start(() =>
            l.TakeUntil(r)
        );

        res.Messages.AssertEqual(
            OnCompleted<int>(225)
        );

        l.Subscriptions.AssertEqual(
            Subscribe(200, 225)
        );

        r.Subscriptions.AssertEqual(
            Subscribe(200, 225)
        );
    }

    [TestMethod]
    public void TakeUntil_Preempt_Never_Error()
    {
        var scheduler = Scheduler;

        var ex = new Exception();

        var l = scheduler.CreateHotObservable(
            OnNext(150, 1)
        );

        var r = scheduler.CreateHotObservable(
            OnNext(150, 1),
            OnError<int>(225, ex)
        );

        var res = scheduler.Start(() =>
            l.TakeUntil(r)
        );

        res.Messages.AssertEqual(
            OnError<int>(225, ex)
        );

        l.Subscriptions.AssertEqual(
            Subscribe(200, 225)
        );

        r.Subscriptions.AssertEqual(
            Subscribe(200, 225)
        );
    }

    [TestMethod]
    public void TakeUntil_NoPreempt_Never_Empty()
    {
        var scheduler = Scheduler;

        var l = scheduler.CreateHotObservable(
            OnNext(150, 1)
        );

        var r = scheduler.CreateHotObservable(
            OnNext(150, 1),
            OnCompleted<int>(225)
        );

        var res = scheduler.Start(() =>
            l.TakeUntil(r)
        );

        res.Messages.AssertEqual(
        );

        l.Subscriptions.AssertEqual(
            Subscribe(
                200,
                1000 /* can't dispose prematurely, could be in flight to dispatch OnError */)
        );

        r.Subscriptions.AssertEqual(
            Subscribe(200, 225)
        );
    }

    [TestMethod]
    public void TakeUntil_NoPreempt_Never_Never()
    {
        var scheduler = Scheduler;

        var l = scheduler.CreateHotObservable(
            OnNext(150, 1)
        );

        var r = scheduler.CreateHotObservable(
            OnNext(150, 1)
        );

        var res = scheduler.Start(() =>
            l.TakeUntil(r)
        );

        res.Messages.AssertEqual(
        );

        l.Subscriptions.AssertEqual(
            Subscribe(200, 1000)
        );

        r.Subscriptions.AssertEqual(
            Subscribe(200, 1000)
        );
    }

    [TestMethod]
    public void TakeUntil_Preempt_BeforeFirstProduced()
    {
        var scheduler = Scheduler;

        var l = scheduler.CreateHotObservable(
            OnNext(150, 1),
            OnNext(230, 2),
            OnCompleted<int>(240)
        );

        var r = scheduler.CreateHotObservable(
            OnNext(150, 1),
            OnNext(210, 2), //!
            OnCompleted<int>(220)
        );

        var res = scheduler.Start(() =>
            l.TakeUntil(r)
        );

        res.Messages.AssertEqual(
            OnCompleted<int>(210)
        );

        l.Subscriptions.AssertEqual(
            Subscribe(200, 210)
        );

        r.Subscriptions.AssertEqual(
            Subscribe(200, 210)
        );
    }

    [TestMethod]
    public void TakeUntil_Preempt_BeforeFirstProduced_RemainSilentAndProperDisposed()
    {
        var scheduler = Scheduler;

        var sourceNotDisposed = false;

        var l = scheduler.CreateHotObservable(
            OnNext(150, 1),
            OnError<int>(215, new Exception()), // should not come
            OnCompleted<int>(240)
        );

        var r = scheduler.CreateHotObservable(
            OnNext(150, 1),
            OnNext(210, 2), //!
            OnCompleted<int>(220)
        );

        var res = scheduler.Start(() =>
            l.Do(_ => sourceNotDisposed = true).TakeUntil(r)
        );

        res.Messages.AssertEqual(
            OnCompleted<int>(210)
        );

        Assert.IsFalse(sourceNotDisposed);
    }

    [TestMethod]
    public void TakeUntil_NoPreempt_AfterLastProduced_ProperDisposedSignal()
    {
        var scheduler = Scheduler;

        var signalNotDisposed = false;

        var l = scheduler.CreateHotObservable(
            OnNext(150, 1),
            OnNext(230, 2),
            OnCompleted<int>(240)
        );

        var r = scheduler.CreateHotObservable(
            OnNext(150, 1),
            OnNext(250, 2),
            OnCompleted<int>(260)
        );

        var res = scheduler.Start(() =>
            l.TakeUntil(r.Do(_ => signalNotDisposed = true))
        );

        res.Messages.AssertEqual(
            OnNext(230, 2),
            OnCompleted<int>(240)
        );

        Assert.IsFalse(signalNotDisposed);
    }

    [TestMethod]
    public void TakeUntil_Error_Some()
    {
        var scheduler = Scheduler;

        var ex = new Exception();

        var l = scheduler.CreateHotObservable(
            OnNext(150, 1),
            OnError<int>(225, ex)
        );

        var r = scheduler.CreateHotObservable(
            OnNext(150, 1),
            OnNext(240, 2)
        );

        var res = scheduler.Start(() =>
            l.TakeUntil(r)
        );

        res.Messages.AssertEqual(
            OnError<int>(225, ex)
        );

        l.Subscriptions.AssertEqual(
            Subscribe(200, 225)
        );

        r.Subscriptions.AssertEqual(
            Subscribe(200, 225)
        );
    }

    [TestMethod]
    public void TakeUntil_Immediate()
    {
        var scheduler = Scheduler;

        var xs = Seq.Return(1);
        var ys = Seq.Return("bar");

        var res = scheduler.Start(() =>
            xs.TakeUntil(ys)
        );

        res.Messages.AssertEqual(
            OnCompleted<int>(200)
        );
    }

    [TestMethod]
    public void TakeUntil_Zero()
    {
        var scheduler = Scheduler;

        var xs = scheduler.CreateHotObservable(
            OnNext(210, 1),
            OnNext(220, 2),
            OnCompleted<int>(230)
        );

        var res = scheduler.Start(() =>
            xs.TakeUntil(new DateTimeOffset(), scheduler)
        );

        res.Messages.AssertEqual(
            OnCompleted<int>(ScheduledAt(200))
        );

        xs.Subscriptions.AssertEqual(
            Subscribe(200, ScheduledAt(200))
        );
    }

    [TestMethod]
    public void TakeUntil_Some()
    {
        var scheduler = Scheduler;

        var xs = scheduler.CreateHotObservable(
            OnNext(210, 1),
            OnNext(220, 2),
            OnNext(230, 3),
            OnCompleted<int>(240)
        );

        var res = scheduler.Start(() =>
            xs.TakeUntil(new DateTimeOffset(225, TimeSpan.Zero), scheduler)
        );

        res.Messages.AssertEqual(
            OnNext(210, 1),
            OnNext(220, 2),
            OnCompleted<int>(225)
        );

        xs.Subscriptions.AssertEqual(
            Subscribe(200, 225)
        );
    }

    [TestMethod]
    public void TakeUntil_Late()
    {
        var scheduler = Scheduler;

        var xs = scheduler.CreateHotObservable(
            OnNext(210, 1),
            OnNext(220, 2),
            OnCompleted<int>(230)
        );

        var res = scheduler.Start(() =>
            xs.TakeUntil(new DateTimeOffset(250, TimeSpan.Zero), scheduler)
        );

        res.Messages.AssertEqual(
            OnNext(210, 1),
            OnNext(220, 2),
            OnCompleted<int>(230)
        );

        xs.Subscriptions.AssertEqual(
            Subscribe(200, 230)
        );
    }

    [TestMethod]
    public void TakeUntil_Error()
    {
        var scheduler = Scheduler;

        var ex = new Exception();

        var xs = scheduler.CreateHotObservable(
            OnError<int>(210, ex)
        );

        var res = scheduler.Start(() =>
            xs.TakeUntil(new DateTimeOffset(250, TimeSpan.Zero), scheduler)
        );

        res.Messages.AssertEqual(
            OnError<int>(210, ex)
        );

        xs.Subscriptions.AssertEqual(
            Subscribe(200, 210)
        );
    }

    [TestMethod]
    public void TakeUntil_Never()
    {
        var scheduler = Scheduler;

        var ex = new Exception();

        var xs = scheduler.CreateHotObservable<int>(
        );

        var res = scheduler.Start(() =>
            xs.TakeUntil(new DateTimeOffset(250, TimeSpan.Zero), scheduler)
        );

        res.Messages.AssertEqual(
            OnCompleted<int>(250)
        );

        xs.Subscriptions.AssertEqual(
            Subscribe(200, 250)
        );
    }

    [TestMethod]
    public void TakeUntil_Twice1()
    {
        var scheduler = Scheduler;

        var ex = new Exception();

        var xs = scheduler.CreateHotObservable(
            OnNext(210, 1),
            OnNext(220, 2),
            OnNext(230, 3),
            OnNext(240, 4),
            OnNext(250, 5),
            OnNext(260, 6),
            OnCompleted<int>(270)
        );

        var res = scheduler.Start(() =>
            xs.TakeUntil(new DateTimeOffset(255, TimeSpan.Zero), scheduler)
                .TakeUntil(new DateTimeOffset(235, TimeSpan.Zero), scheduler)
        );

        res.Messages.AssertEqual(
            OnNext(210, 1),
            OnNext(220, 2),
            OnNext(230, 3),
            OnCompleted<int>(235)
        );

        xs.Subscriptions.AssertEqual(
            Subscribe(200, 235)
        );
    }

    [TestMethod]
    public void TakeUntil_Twice2()
    {
        var scheduler = Scheduler;

        var ex = new Exception();

        var xs = scheduler.CreateHotObservable(
            OnNext(210, 1),
            OnNext(220, 2),
            OnNext(230, 3),
            OnNext(240, 4),
            OnNext(250, 5),
            OnNext(260, 6),
            OnCompleted<int>(270)
        );

        var res = scheduler.Start(() =>
            xs.TakeUntil(new DateTimeOffset(235, TimeSpan.Zero), scheduler)
                .TakeUntil(new DateTimeOffset(255, TimeSpan.Zero), scheduler)
        );

        res.Messages.AssertEqual(
            OnNext(210, 1),
            OnNext(220, 2),
            OnNext(230, 3),
            OnCompleted<int>(235)
        );

        xs.Subscriptions.AssertEqual(
            Subscribe(200, 235)
        );
    }

    [TestMethod]
    public async Task TakeUntil_Default()
    {
        var res = await ToListAsync(
            Seq.Range(0, 10, DefaultScheduler).TakeUntil(DateTimeOffset.Now.AddMinutes(1)));

        Assert.IsTrue(res.SequenceEqual(Enumerable.Range(0, 10)));
    }

    [TestMethod]
    public void TakeUntil_Predicate_Basic()
    {
        var scheduler = Scheduler;

        var source = scheduler.CreateColdObservable(
                OnNext(10, 1),
                OnNext(20, 2),
                OnNext(30, 3),
                OnNext(40, 4),
                OnNext(50, 5),
                OnNext(60, 6),
                OnNext(70, 7),
                OnNext(80, 8),
                OnNext(90, 9),
                OnCompleted<int>(100)
            );

        var result = scheduler.Start(() => source.TakeUntil(v => v == 5));

        result.Messages.AssertEqual(
            OnNext(210, 1),
            OnNext(220, 2),
            OnNext(230, 3),
            OnNext(240, 4),
            OnNext(250, 5),
            OnCompleted<int>(250)
        );

        source.Subscriptions.AssertEqual(
            Subscribe(200, 250)
        );
    }

    [TestMethod]
    public void TakeUntil_Predicate_True()
    {
        var scheduler = Scheduler;

        var source = scheduler.CreateColdObservable(
                OnNext(10, 1),
                OnNext(20, 2),
                OnNext(30, 3),
                OnNext(40, 4),
                OnNext(50, 5),
                OnNext(60, 6),
                OnNext(70, 7),
                OnNext(80, 8),
                OnNext(90, 9),
                OnCompleted<int>(100)
            );

        var result = scheduler.Start(() => source.TakeUntil(v => true));

        result.Messages.AssertEqual(
            OnNext(210, 1),
            OnCompleted<int>(210)
        );

        source.Subscriptions.AssertEqual(
            Subscribe(200, 210)
        );
    }

    [TestMethod]
    public void TakeUntil_Predicate_False()
    {
        var scheduler = Scheduler;

        var source = scheduler.CreateColdObservable(
                OnNext(10, 1),
                OnNext(20, 2),
                OnNext(30, 3),
                OnNext(40, 4),
                OnNext(50, 5),
                OnNext(60, 6),
                OnNext(70, 7),
                OnNext(80, 8),
                OnNext(90, 9),
                OnCompleted<int>(100)
            );

        var result = scheduler.Start(() => source.TakeUntil(v => false));

        result.Messages.AssertEqual(
            OnNext(210, 1),
            OnNext(220, 2),
            OnNext(230, 3),
            OnNext(240, 4),
            OnNext(250, 5),
            OnNext(260, 6),
            OnNext(270, 7),
            OnNext(280, 8),
            OnNext(290, 9),
            OnCompleted<int>(300)
        );

        source.Subscriptions.AssertEqual(
            Subscribe(200, 300)
        );
    }

    [TestMethod]
    public void TakeUntil_Predicate_Error()
    {
        var scheduler = Scheduler;

        var ex = new InvalidOperationException();

        var source = scheduler.CreateColdObservable(
                OnNext(10, 1),
                OnNext(20, 2),
                OnNext(30, 3),
                OnError<int>(40, ex)
            );

        var result = scheduler.Start(() => source.TakeUntil(v => false));

        result.Messages.AssertEqual(
            OnNext(210, 1),
            OnNext(220, 2),
            OnNext(230, 3),
            OnError<int>(240, ex)
        );

        source.Subscriptions.AssertEqual(
            Subscribe(200, 240)
        );
    }

    [TestMethod]
    public void TakeUntil_Predicate_Crash()
    {
        var scheduler = Scheduler;

        var ex = new InvalidOperationException();

        var source = scheduler.CreateColdObservable(
                OnNext(10, 1),
                OnNext(20, 2),
                OnNext(30, 3),
                OnNext(240, 4),
                OnNext(250, 5),
                OnCompleted<int>(260)
            );

        var result = scheduler.Start(() => source.TakeUntil(v =>
        {
            if (v == 3)
            {
                throw ex;
            }
            return false;
        }));

        result.Messages.AssertEqual(
            OnNext(210, 1),
            OnNext(220, 2),
            OnNext(230, 3),
            OnError<int>(230, ex)
        );

        source.Subscriptions.AssertEqual(
            Subscribe(200, 230)
        );
    }

    [TestMethod]
    public void TakeUntil_CancellationToken_BasicCancelation()
    {
        var scheduler = Scheduler;
        var tokenSource = new CancellationTokenSource();

        var source = scheduler.CreateColdObservable(
            OnNext(10, 1),
            OnNext(20, 2),
            OnNext(30, 3),
            OnNext(40, 4),
            OnNext(50, 5),
            OnCompleted<int>(260)
            );

        scheduler.ScheduleAbsolute(235, () => tokenSource.Cancel());

        var result = scheduler.Start(() => source.TakeUntil(tokenSource.Token));

        result.Messages.AssertEqual(
            OnNext(210, 1),
            OnNext(220, 2),
            OnNext(230, 3),
            OnCompleted<int>(235)
        );

        source.Subscriptions.AssertEqual(
            Subscribe(200, 235)
        );
    }

    [TestMethod]
    public void TakeUntil_CancellationToken_AlreadyCanceled()
    {
        var scheduler = Scheduler;
        var tokenSource = new CancellationTokenSource();
        tokenSource.Cancel();

        var source = scheduler.CreateColdObservable(
            OnNext(10, 1),
            OnNext(20, 2),
            OnNext(30, 3),
            OnNext(40, 4),
            OnNext(50, 5),
            OnCompleted<int>(260)
            );

        var result = scheduler.Start(() => source.TakeUntil(tokenSource.Token));

        result.Messages.AssertEqual(
            OnCompleted<int>(200)
        );

        source.Subscriptions.AssertEqual();
    }
}

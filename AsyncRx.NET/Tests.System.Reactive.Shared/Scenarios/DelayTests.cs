// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information. 

using System.Reactive;

using Microsoft.Reactive.Testing;

namespace Tests.System.Reactive.Shared.Scenarios;

/// <summary>
/// Shared <c>Delay</c> scenarios: every behavioural test from Rx.NET's <c>DelayTest.cs</c>, with the
/// overloads AsyncRx.NET gained to match Rx.NET's surface. The stopwatch axis is the sync text
/// (<c>useStopwatch ? Scheduler : Scheduler.DisableOptimizations()</c>): both are scheduler
/// references in a description, resolved by the platform.
/// </summary>
/// <remarks>
/// Transcribed mechanically from the sync file,
/// including its <c>*_Stopwatch</c> pairs, which run the same scenario with and without the
/// scheduler's optional capabilities — written as <c>Scheduler.DisableOptimizations()</c>, as in
/// the sync suite; on AsyncRx.NET the two are the same run. Two scheduler-free tests were
/// hand-ported onto the raw surface. Deliberately not here: the two <c>*_ArgumentChecking</c> tests, and the native
/// stratum — the eight <c>*_Real_*</c> tests (thread-pool scheduler and <c>Subject</c>s), the two
/// <c>*_DefaultScheduler</c> tests, <c>Delay_CrossingMessages</c>, <c>Delay_ErrorHandling1</c>
/// (a hand-written <c>IScheduler</c> with events), and the two <c>Delay_LongRunning_*</c> tests
/// (a hand-written <c>ISchedulerLongRunning</c>).
/// </remarks>
public abstract class DelayTests : SharedReactiveTest
{
    [TestMethod]
    public void Delay_TimeSpan_Simple1()
    {
        Delay_TimeSpan_Simple1_Impl(false);
    }

    [TestMethod]
    public void Delay_TimeSpan_Simple1_Stopwatch()
    {
        Delay_TimeSpan_Simple1_Impl(true);
    }

    private void Delay_TimeSpan_Simple1_Impl(bool useStopwatch)
    {

        var xs = Scheduler.CreateHotObservable(
            OnNext(150, 1),
            OnNext(250, 2),
            OnNext(350, 3),
            OnNext(450, 4),
            OnCompleted<int>(550)
        );

        var res = Scheduler.Start(() =>
            xs.Delay(TimeSpan.FromTicks(100), useStopwatch ? Scheduler : Scheduler.DisableOptimizations())
        );

        res.Messages.AssertEqual(
            OnNext(350, 2),
            OnNext(450, 3),
            OnNext(550, 4),
            OnCompleted<int>(650)
        );

        xs.Subscriptions.AssertEqual(
            Subscribe(200, 550)
        );
    }

    [TestMethod]
    public void Delay_DateTimeOffset_Simple1()
    {
        Delay_DateTimeOffset_Simple1_Impl(false);
    }

    [TestMethod]
    public void Delay_DateTimeOffset_Simple1_Stopwatch()
    {
        Delay_DateTimeOffset_Simple1_Impl(true);
    }

    private void Delay_DateTimeOffset_Simple1_Impl(bool useStopwatch)
    {

        var xs = Scheduler.CreateHotObservable(
            OnNext(150, 1),
            OnNext(250, 2),
            OnNext(350, 3),
            OnNext(450, 4),
            OnCompleted<int>(550)
        );

        var res = Scheduler.Start(() =>
            xs.Delay(new DateTimeOffset(300, TimeSpan.Zero), useStopwatch ? Scheduler : Scheduler.DisableOptimizations())
        );

        res.Messages.AssertEqual(
            OnNext(350, 2),
            OnNext(450, 3),
            OnNext(550, 4),
            OnCompleted<int>(650)
        );

        xs.Subscriptions.AssertEqual(
            Subscribe(200, 550)
        );
    }

    [TestMethod]
    public void Delay_TimeSpan_Simple2()
    {
        Delay_TimeSpan_Simple2_Impl(false);
    }

    [TestMethod]
    public void Delay_TimeSpan_Simple2_Stopwatch()
    {
        Delay_TimeSpan_Simple2_Impl(true);
    }

    private void Delay_TimeSpan_Simple2_Impl(bool useStopwatch)
    {

        var xs = Scheduler.CreateHotObservable(
            OnNext(150, 1),
            OnNext(250, 2),
            OnNext(350, 3),
            OnNext(450, 4),
            OnCompleted<int>(550)
        );

        var res = Scheduler.Start(() =>
            xs.Delay(TimeSpan.FromTicks(50), useStopwatch ? Scheduler : Scheduler.DisableOptimizations())
        );

        res.Messages.AssertEqual(
            OnNext(300, 2),
            OnNext(400, 3),
            OnNext(500, 4),
            OnCompleted<int>(600)
        );

        xs.Subscriptions.AssertEqual(
            Subscribe(200, 550)
        );
    }

    [TestMethod]
    public void Delay_DateTimeOffset_Simple2()
    {
        Delay_DateTimeOffset_Simple2_Impl(false);
    }

    [TestMethod]
    public void Delay_DateTimeOffset_Simple2_Stopwatch()
    {
        Delay_DateTimeOffset_Simple2_Impl(true);
    }

    private void Delay_DateTimeOffset_Simple2_Impl(bool useStopwatch)
    {

        var xs = Scheduler.CreateHotObservable(
            OnNext(150, 1),
            OnNext(250, 2),
            OnNext(350, 3),
            OnNext(450, 4),
            OnCompleted<int>(550)
        );

        var res = Scheduler.Start(() =>
            xs.Delay(new DateTimeOffset(250, TimeSpan.Zero), useStopwatch ? Scheduler : Scheduler.DisableOptimizations())
        );

        res.Messages.AssertEqual(
            OnNext(300, 2),
            OnNext(400, 3),
            OnNext(500, 4),
            OnCompleted<int>(600)
        );

        xs.Subscriptions.AssertEqual(
            Subscribe(200, 550)
        );
    }

    [TestMethod]
    public void Delay_TimeSpan_Simple3()
    {
        Delay_TimeSpan_Simple3_Impl(false);
    }

    [TestMethod]
    public void Delay_TimeSpan_Simple3_Stopwatch()
    {
        Delay_TimeSpan_Simple3_Impl(true);
    }

    private void Delay_TimeSpan_Simple3_Impl(bool useStopwatch)
    {

        var xs = Scheduler.CreateHotObservable(
            OnNext(150, 1),
            OnNext(250, 2),
            OnNext(350, 3),
            OnNext(450, 4),
            OnCompleted<int>(550)
        );

        var res = Scheduler.Start(() =>
            xs.Delay(TimeSpan.FromTicks(150), useStopwatch ? Scheduler : Scheduler.DisableOptimizations())
        );

        res.Messages.AssertEqual(
            OnNext(400, 2),
            OnNext(500, 3),
            OnNext(600, 4),
            OnCompleted<int>(700)
        );

        xs.Subscriptions.AssertEqual(
            Subscribe(200, 550)
        );
    }

    [TestMethod]
    public void Delay_DateTimeOffset_Simple3()
    {
        Delay_DateTimeOffset_Simple3_Impl(false);
    }

    [TestMethod]
    public void Delay_DateTimeOffset_Simple3_Stopwatch()
    {
        Delay_DateTimeOffset_Simple3_Impl(true);
    }

    private void Delay_DateTimeOffset_Simple3_Impl(bool useStopwatch)
    {

        var xs = Scheduler.CreateHotObservable(
            OnNext(150, 1),
            OnNext(250, 2),
            OnNext(350, 3),
            OnNext(450, 4),
            OnCompleted<int>(550)
        );

        var res = Scheduler.Start(() =>
            xs.Delay(new DateTimeOffset(350, TimeSpan.Zero), useStopwatch ? Scheduler : Scheduler.DisableOptimizations())
        );

        res.Messages.AssertEqual(
            OnNext(400, 2),
            OnNext(500, 3),
            OnNext(600, 4),
            OnCompleted<int>(700)
        );

        xs.Subscriptions.AssertEqual(
            Subscribe(200, 550)
        );
    }

    [TestMethod]
    public void Delay_TimeSpan_Error1()
    {
        Delay_TimeSpan_Error1_Impl(false);
    }

    [TestMethod]
    public void Delay_TimeSpan_Error1_Stopwatch()
    {
        Delay_TimeSpan_Error1_Impl(true);
    }

    private void Delay_TimeSpan_Error1_Impl(bool useStopwatch)
    {

        var ex = new Exception();

        var xs = Scheduler.CreateHotObservable(
            OnNext(150, 1),
            OnNext(250, 2),
            OnNext(350, 3),
            OnNext(450, 4),
            OnError<int>(550, ex)
        );

        var res = Scheduler.Start(() =>
            xs.Delay(TimeSpan.FromTicks(50), useStopwatch ? Scheduler : Scheduler.DisableOptimizations())
        );

        res.Messages.AssertEqual(
            OnNext(300, 2),
            OnNext(400, 3),
            OnNext(500, 4),
            OnError<int>(550, ex)
        );

        xs.Subscriptions.AssertEqual(
            Subscribe(200, 550)
        );
    }

    [TestMethod]
    public void Delay_DateTimeOffset_Error1()
    {
        Delay_DateTimeOffset_Error1_Impl(false);
    }

    [TestMethod]
    public void Delay_DateTimeOffset_Error1_Stopwatch()
    {
        Delay_DateTimeOffset_Error1_Impl(true);
    }

    private void Delay_DateTimeOffset_Error1_Impl(bool useStopwatch)
    {

        var ex = new Exception();

        var xs = Scheduler.CreateHotObservable(
            OnNext(150, 1),
            OnNext(250, 2),
            OnNext(350, 3),
            OnNext(450, 4),
            OnError<int>(550, ex)
        );

        var res = Scheduler.Start(() =>
            xs.Delay(new DateTimeOffset(250, TimeSpan.Zero), useStopwatch ? Scheduler : Scheduler.DisableOptimizations())
        );

        res.Messages.AssertEqual(
            OnNext(300, 2),
            OnNext(400, 3),
            OnNext(500, 4),
            OnError<int>(550, ex)
        );

        xs.Subscriptions.AssertEqual(
            Subscribe(200, 550)
        );
    }

    [TestMethod]
    public void Delay_TimeSpan_Error2()
    {
        Delay_TimeSpan_Error2_Impl(false);
    }

    [TestMethod]
    public void Delay_TimeSpan_Error2_Stopwatch()
    {
        Delay_TimeSpan_Error2_Impl(true);
    }

    private void Delay_TimeSpan_Error2_Impl(bool useStopwatch)
    {

        var ex = new Exception();

        var xs = Scheduler.CreateHotObservable(
            OnNext(150, 1),
            OnNext(250, 2),
            OnNext(350, 3),
            OnNext(450, 4),
            OnError<int>(550, ex)
        );

        var res = Scheduler.Start(() =>
            xs.Delay(TimeSpan.FromTicks(150), useStopwatch ? Scheduler : Scheduler.DisableOptimizations())
        );

        res.Messages.AssertEqual(
            OnNext(400, 2),
            OnNext(500, 3),
            OnError<int>(550, ex)
        );

        xs.Subscriptions.AssertEqual(
            Subscribe(200, 550)
        );
    }

    [TestMethod]
    public void Delay_DateTimeOffset_Error2()
    {
        Delay_DateTimeOffset_Error2_Impl(false);
    }

    [TestMethod]
    public void Delay_DateTimeOffset_Error2_Stopwatch()
    {
        Delay_DateTimeOffset_Error2_Impl(true);
    }

    private void Delay_DateTimeOffset_Error2_Impl(bool useStopwatch)
    {

        var ex = new Exception();

        var xs = Scheduler.CreateHotObservable(
            OnNext(150, 1),
            OnNext(250, 2),
            OnNext(350, 3),
            OnNext(450, 4),
            OnError<int>(550, ex)
        );

        var res = Scheduler.Start(() =>
            xs.Delay(new DateTimeOffset(350, TimeSpan.Zero), useStopwatch ? Scheduler : Scheduler.DisableOptimizations())
        );

        res.Messages.AssertEqual(
            OnNext(400, 2),
            OnNext(500, 3),
            OnError<int>(550, ex)
        );

        xs.Subscriptions.AssertEqual(
            Subscribe(200, 550)
        );
    }

    [TestMethod]
    public void Delay_TimeSpan_Positive()
    {

        var msgs = new[] {
            OnNext(150, 1),
            OnNext(250, 2),
            OnNext(350, 3),
            OnNext(450, 4),
            OnCompleted<int>(550)
        };

        var xs = Scheduler.CreateHotObservable(msgs);

        const ushort delay = 42;

        var res = Scheduler.Start(() =>
            xs.Delay(TimeSpan.FromTicks(delay), Scheduler)
        );

        var expected = from n in msgs
                       where n.Time > Subscribed
                       select new Recorded<Notification<int>>((ushort)(n.Time + delay), n.Value);

        res.Messages.AssertEqual(expected);
    }

    [TestMethod]
    public void Delay_Empty()
    {

        var xs = Scheduler.CreateHotObservable(
            OnNext(150, 1),
            OnCompleted<int>(550)
        );

        var res = Scheduler.Start(() =>
            xs.Delay(TimeSpan.FromTicks(10), Scheduler)
        );

        res.Messages.AssertEqual(
            OnCompleted<int>(560)
        );

        xs.Subscriptions.AssertEqual(
            Subscribe(200, 550)
        );
    }

    [TestMethod]
    public void Delay_Error()
    {

        var ex = new Exception();

        var xs = Scheduler.CreateHotObservable(
            OnNext(150, 1),
            OnError<int>(550, ex)
        );

        var res = Scheduler.Start(() =>
            xs.Delay(TimeSpan.FromTicks(10), Scheduler)
        );

        res.Messages.AssertEqual(
            OnError<int>(550, ex)
        );

        xs.Subscriptions.AssertEqual(
            Subscribe(200, 550)
        );
    }

    [TestMethod]
    public void Delay_Never()
    {

        var xs = Scheduler.CreateHotObservable(
            OnNext(150, 1)
        );

        var res = Scheduler.Start(() =>
            xs.Delay(TimeSpan.FromTicks(10), Scheduler)
        );

        res.Messages.AssertEqual(
        );

        xs.Subscriptions.AssertEqual(
            Subscribe(200, 1000)
        );
    }

    [TestMethod]
    public void Delay_Duration_Simple1()
    {

        var xs = Scheduler.CreateHotObservable(
            OnNext(150, 1),
            OnNext(210, 10),
            OnNext(220, 30),
            OnNext(230, 50),
            OnNext(240, 35),
            OnNext(250, 20),
            OnCompleted<int>(260)
        );

        var res = Scheduler.Start(() =>
            xs.Delay(x => Scheduler.CreateColdObservable([OnNext(x, "!")]))
        );

        res.Messages.AssertEqual(
            OnNext(210 + 10, 10),
            OnNext(220 + 30, 30),
            OnNext(250 + 20, 20),
            OnNext(240 + 35, 35),
            OnNext(230 + 50, 50),
            OnCompleted<int>(280)
        );

        xs.Subscriptions.AssertEqual(
            Subscribe(200, 260)
        );
    }

    [TestMethod]
    public void Delay_Duration_Simple2()
    {

        var xs = Scheduler.CreateHotObservable(
            OnNext(150, 1),
            OnNext(210, 2),
            OnNext(220, 3),
            OnNext(230, 4),
            OnNext(240, 5),
            OnNext(250, 6),
            OnCompleted<int>(300)
        );

        var ys = Scheduler.CreateColdObservable(
            OnNext(10, "!")
        );

        var res = Scheduler.Start(() =>
            xs.Delay(_ => ys)
        );

        res.Messages.AssertEqual(
            OnNext(210 + 10, 2),
            OnNext(220 + 10, 3),
            OnNext(230 + 10, 4),
            OnNext(240 + 10, 5),
            OnNext(250 + 10, 6),
            OnCompleted<int>(300)
        );

        xs.Subscriptions.AssertEqual(
            Subscribe(200, 300)
        );

        ys.Subscriptions.AssertEqual(
            Subscribe(210, 220),
            Subscribe(220, 230),
            Subscribe(230, 240),
            Subscribe(240, 250),
            Subscribe(250, 260)
        );
    }

    [TestMethod]
    public void Delay_Duration_Simple3()
    {

        var xs = Scheduler.CreateHotObservable(
            OnNext(150, 1),
            OnNext(210, 2),
            OnNext(220, 3),
            OnNext(230, 4),
            OnNext(240, 5),
            OnNext(250, 6),
            OnCompleted<int>(300)
        );

        var ys = Scheduler.CreateColdObservable(
            OnNext(100, "!")
        );

        var res = Scheduler.Start(() =>
            xs.Delay(_ => ys)
        );

        res.Messages.AssertEqual(
            OnNext(210 + 100, 2),
            OnNext(220 + 100, 3),
            OnNext(230 + 100, 4),
            OnNext(240 + 100, 5),
            OnNext(250 + 100, 6),
            OnCompleted<int>(350)
        );

        xs.Subscriptions.AssertEqual(
            Subscribe(200, 300)
        );

        ys.Subscriptions.AssertEqual(
            Subscribe(210, 310),
            Subscribe(220, 320),
            Subscribe(230, 330),
            Subscribe(240, 340),
            Subscribe(250, 350)
        );
    }

    [TestMethod]
    public void Delay_Duration_Simple4_InnerEmpty()
    {

        var xs = Scheduler.CreateHotObservable(
            OnNext(150, 1),
            OnNext(210, 2),
            OnNext(220, 3),
            OnNext(230, 4),
            OnNext(240, 5),
            OnNext(250, 6),
            OnCompleted<int>(300)
        );

        var ys = Scheduler.CreateColdObservable(
            OnCompleted<int>(100)
        );

        var res = Scheduler.Start(() =>
            xs.Delay(_ => ys)
        );

        res.Messages.AssertEqual(
            OnNext(210 + 100, 2),
            OnNext(220 + 100, 3),
            OnNext(230 + 100, 4),
            OnNext(240 + 100, 5),
            OnNext(250 + 100, 6),
            OnCompleted<int>(350)
        );

        xs.Subscriptions.AssertEqual(
            Subscribe(200, 300)
        );

        ys.Subscriptions.AssertEqual(
            Subscribe(210, 310),
            Subscribe(220, 320),
            Subscribe(230, 330),
            Subscribe(240, 340),
            Subscribe(250, 350)
        );
    }

    [TestMethod]
    public void Delay_Duration_Dispose1()
    {

        var xs = Scheduler.CreateHotObservable(
            OnNext(150, 1),
            OnNext(210, 2),
            OnNext(220, 3),
            OnNext(230, 4),
            OnNext(240, 5),
            OnNext(250, 6),
            OnCompleted<int>(300)
        );

        var ys = Scheduler.CreateColdObservable(
            OnNext(200, "!")
        );

        var res = Scheduler.Start(() =>
            xs.Delay(_ => ys),
            425
        );

        res.Messages.AssertEqual(
            OnNext(210 + 200, 2),
            OnNext(220 + 200, 3)
        );

        xs.Subscriptions.AssertEqual(
            Subscribe(200, 300)
        );

        ys.Subscriptions.AssertEqual(
            Subscribe(210, 410),
            Subscribe(220, 420),
            Subscribe(230, 425),
            Subscribe(240, 425),
            Subscribe(250, 425)
        );
    }

    [TestMethod]
    public void Delay_Duration_Dispose2()
    {

        var xs = Scheduler.CreateHotObservable(
            OnNext(150, 1),
            OnNext(210, 2),
            OnNext(400, 3),
            OnCompleted<int>(500)
        );

        var ys = Scheduler.CreateColdObservable(
            OnNext(50, "!")
        );

        var res = Scheduler.Start(() =>
            xs.Delay(_ => ys),
            300
        );

        res.Messages.AssertEqual(
            OnNext(210 + 50, 2)
        );

        xs.Subscriptions.AssertEqual(
            Subscribe(200, 300)
        );

        ys.Subscriptions.AssertEqual(
            Subscribe(210, 260)
        );
    }

    [TestMethod]
    public void Delay_Duration_OuterError1()
    {

        var ex = new Exception();

        var xs = Scheduler.CreateHotObservable(
            OnNext(150, 1),
            OnNext(210, 2),
            OnNext(220, 3),
            OnNext(230, 4),
            OnNext(240, 5),
            OnNext(250, 6),
            OnError<int>(300, ex)
        );

        var ys = Scheduler.CreateColdObservable(
            OnNext(100, "!")
        );

        var res = Scheduler.Start(() =>
            xs.Delay(_ => ys)
        );

        res.Messages.AssertEqual(
            OnError<int>(300, ex)
        );

        xs.Subscriptions.AssertEqual(
            Subscribe(200, 300)
        );

        ys.Subscriptions.AssertEqual(
            Subscribe(210, 300),
            Subscribe(220, 300),
            Subscribe(230, 300),
            Subscribe(240, 300),
            Subscribe(250, 300)
        );
    }

    [TestMethod]
    public void Delay_Duration_OuterError2()
    {

        var ex = new Exception();

        var xs = Scheduler.CreateHotObservable(
            OnNext(150, 1),
            OnNext(210, 2),
            OnNext(220, 3),
            OnNext(230, 4),
            OnNext(240, 5),
            OnNext(250, 6),
            OnError<int>(300, ex)
        );

        var ys = Scheduler.CreateColdObservable(
            OnNext(10, "!")
        );

        var res = Scheduler.Start(() =>
            xs.Delay(_ => ys)
        );

        res.Messages.AssertEqual(
            OnNext(210 + 10, 2),
            OnNext(220 + 10, 3),
            OnNext(230 + 10, 4),
            OnNext(240 + 10, 5),
            OnNext(250 + 10, 6),
            OnError<int>(300, ex)
        );

        xs.Subscriptions.AssertEqual(
            Subscribe(200, 300)
        );

        ys.Subscriptions.AssertEqual(
            Subscribe(210, 220),
            Subscribe(220, 230),
            Subscribe(230, 240),
            Subscribe(240, 250),
            Subscribe(250, 260)
        );
    }

    [TestMethod]
    public void Delay_Duration_InnerError1()
    {

        var ex = new Exception();

        var xs = Scheduler.CreateHotObservable(
            OnNext(150, 1),
            OnNext(210, 2),
            OnNext(220, 3),
            OnNext(230, 4),
            OnNext(240, 5),
            OnNext(250, 6),
            OnCompleted<int>(300)
        );

        var ys = Scheduler.CreateColdObservable(
            OnNext(30, "!")
        );

        var zs = Scheduler.CreateColdObservable(
            OnError<string>(25, ex)
        );

        var res = Scheduler.Start(() =>
            xs.Delay(x => x != 5 ? ys : zs)
        );

        res.Messages.AssertEqual(
            OnNext(210 + 30, 2),
            OnNext(220 + 30, 3),
            OnNext(230 + 30, 4),
            OnError<int>(240 + 25, ex)
        );

        xs.Subscriptions.AssertEqual(
            Subscribe(200, 265)
        );

        ys.Subscriptions.AssertEqual(
            Subscribe(210, 240),
            Subscribe(220, 250),
            Subscribe(230, 260),
            Subscribe(250, 265)
        );
    }

    [TestMethod]
    public void Delay_Duration_InnerError2()
    {

        var ex = new Exception();

        var xs = Scheduler.CreateHotObservable(
            OnNext(150, 1),
            OnNext(210, 2),
            OnNext(220, 3),
            OnNext(230, 4),
            OnNext(240, 5),
            OnNext(250, 6),
            OnCompleted<int>(300)
        );

        var ys = Scheduler.CreateColdObservable(
            OnError<string>(100, ex)
        );

        var res = Scheduler.Start(() =>
            xs.Delay(_ => ys)
        );

        res.Messages.AssertEqual(
            OnError<int>(210 + 100, ex)
        );

        xs.Subscriptions.AssertEqual(
            Subscribe(200, 300)
        );

        ys.Subscriptions.AssertEqual(
            Subscribe(210, 310),
            Subscribe(220, 310),
            Subscribe(230, 310),
            Subscribe(240, 310),
            Subscribe(250, 310)
        );
    }

    [TestMethod]
    public void Delay_Duration_SelectorThrows1()
    {

        var ex = new Exception();

        var xs = Scheduler.CreateHotObservable(
            OnNext(150, 1),
            OnNext(250, 2),
            OnNext(300, 3),
            OnNext(350, 4),
            OnNext(400, 5),
            OnNext(450, 6),
            OnCompleted<int>(500)
        );

        var ys = Scheduler.CreateColdObservable(
            OnNext(80, "")
        );

        var res = Scheduler.Start(() =>
            xs.Delay(x =>
            {
                if (x == 4)
                {
                    throw ex;
                }

                return ys;
            })
        );

        res.Messages.AssertEqual(
            OnNext(330, 2),
            OnError<int>(350, ex)
        );

        xs.Subscriptions.AssertEqual(
            Subscribe(200, 350)
        );

        ys.Subscriptions.AssertEqual(
            Subscribe(250, 330),
            Subscribe(300, 350)
        );
    }

    [TestMethod]
    public void Delay_Duration_Simple()
    {

        var xs = Scheduler.CreateHotObservable(
            OnNext(150, 1),
            OnNext(250, 2),
            OnNext(350, 3),
            OnNext(450, 4),
            OnCompleted<int>(550)
        );

        var res = Scheduler.Start(() =>
            xs.Delay(x =>
                Scheduler.CreateColdObservable(
                    OnNext(x * 10, "Ignore"),
                    OnNext(x * 10 + 5, "Aargh!")
                )
            )
        );

        res.Messages.AssertEqual(
            OnNext(250 + 2 * 10, 2),
            OnNext(350 + 3 * 10, 3),
            OnNext(450 + 4 * 10, 4),
            OnCompleted<int>(550)
        );

        xs.Subscriptions.AssertEqual(
            Subscribe(200, 550)
        );
    }

    [TestMethod]
    public void Delay_Duration_DeferOnCompleted()
    {

        var xs = Scheduler.CreateHotObservable(
            OnNext(150, 1),
            OnNext(250, 2),
            OnNext(350, 3),
            OnNext(450, 4),
            OnCompleted<int>(451)
        );

        var res = Scheduler.Start(() =>
            xs.Delay(x =>
                Scheduler.CreateColdObservable(
                    OnNext(x * 10, "Ignore"),
                    OnNext(x * 10 + 5, "Aargh!")
                )
            )
        );

        res.Messages.AssertEqual(
            OnNext(250 + 2 * 10, 2),
            OnNext(350 + 3 * 10, 3),
            OnNext(450 + 4 * 10, 4),
            OnCompleted<int>(450 + 4 * 10)
        );

        xs.Subscriptions.AssertEqual(
            Subscribe(200, 451)
        );
    }

    [TestMethod]
    public void Delay_Duration_InnerError()
    {

        var xs = Scheduler.CreateHotObservable(
            OnNext(150, 1),
            OnNext(250, 2),
            OnNext(350, 3),
            OnNext(450, 4),
            OnCompleted<int>(451)
        );

        var ex = new Exception();

        var res = Scheduler.Start(() =>
            xs.Delay(x =>
                x < 4 ? Scheduler.CreateColdObservable(
                            OnNext(x * 10, "Ignore"),
                            OnNext(x * 10 + 5, "Aargh!")
                        )
                      : Scheduler.CreateColdObservable(
                            OnError<string>(x * 10, ex)
                        )
            )
        );

        res.Messages.AssertEqual(
            OnNext(250 + 2 * 10, 2),
            OnNext(350 + 3 * 10, 3),
            OnError<int>(450 + 4 * 10, ex)
        );

        xs.Subscriptions.AssertEqual(
            Subscribe(200, 451)
        );
    }

    [TestMethod]
    public void Delay_Duration_OuterError()
    {

        var ex = new Exception();

        var xs = Scheduler.CreateHotObservable(
            OnNext(150, 1),
            OnNext(250, 2),
            OnNext(350, 3),
            OnNext(450, 4),
            OnError<int>(460, ex)
        );

        var res = Scheduler.Start(() =>
            xs.Delay(x =>
                Scheduler.CreateColdObservable(
                    OnNext(x * 10, "Ignore"),
                    OnNext(x * 10 + 5, "Aargh!")
                )
            )
        );

        res.Messages.AssertEqual(
            OnNext(250 + 2 * 10, 2),
            OnNext(350 + 3 * 10, 3),
            OnError<int>(460, ex)
        );

        xs.Subscriptions.AssertEqual(
            Subscribe(200, 460)
        );
    }

    [TestMethod]
    public void Delay_Duration_SelectorThrows2()
    {

        var xs = Scheduler.CreateHotObservable(
            OnNext(150, 1),
            OnNext(250, 2),
            OnNext(350, 3),
            OnNext(450, 4),
            OnCompleted<int>(550)
        );

        var ex = new Exception();

        var res = Scheduler.Start(() =>
            xs.Delay(x =>
            {
                if (x < 4)
                {
                    return Scheduler.CreateColdObservable(
                            OnNext(x * 10, "Ignore"),
                            OnNext(x * 10 + 5, "Aargh!")
                        );
                }
                else
                {
                    throw ex;
                }
            })
        );

        res.Messages.AssertEqual(
            OnNext(250 + 2 * 10, 2),
            OnNext(350 + 3 * 10, 3),
            OnError<int>(450, ex)
        );

        xs.Subscriptions.AssertEqual(
            Subscribe(200, 450)
        );
    }

    [TestMethod]
    public void Delay_Duration_InnerDone()
    {

        var xs = Scheduler.CreateHotObservable(
            OnNext(150, 1),
            OnNext(250, 2),
            OnNext(350, 3),
            OnNext(450, 4),
            OnCompleted<int>(550)
        );

        var ex = new Exception();

        var res = Scheduler.Start(() =>
            xs.Delay(x =>
                Scheduler.CreateColdObservable(
                    OnCompleted<string>(x * 10)
                )
            )
        );

        res.Messages.AssertEqual(
            OnNext(250 + 2 * 10, 2),
            OnNext(350 + 3 * 10, 3),
            OnNext(450 + 4 * 10, 4),
            OnCompleted<int>(550)
        );

        xs.Subscriptions.AssertEqual(
            Subscribe(200, 550)
        );
    }

    [TestMethod]
    public void Delay_Duration_InnerSubscriptionTimes()
    {

        var xs = Scheduler.CreateHotObservable(
            OnNext(150, -1),
            OnNext(250, 0),
            OnNext(350, 1),
            OnNext(450, 2),
            OnCompleted<int>(550)
        );

        var ys = new[] {
            Scheduler.CreateColdObservable(
                OnNext(20, 42),
                OnNext(25, 99)
            ),
            Scheduler.CreateColdObservable(
                OnNext(10, 43),
                OnNext(15, 99)
            ),
            Scheduler.CreateColdObservable(
                OnNext(30, 44),
                OnNext(35, 99)
            ),
        };

        var res = Scheduler.Start(() =>
            xs.Delay(x => ys[x])
        );

        res.Messages.AssertEqual(
            OnNext(250 + 20, 0),
            OnNext(350 + 10, 1),
            OnNext(450 + 30, 2),
            OnCompleted<int>(550)
        );

        xs.Subscriptions.AssertEqual(
            Subscribe(200, 550)
        );

        ys[0].Subscriptions.AssertEqual(Subscribe(250, 250 + 20));
        ys[1].Subscriptions.AssertEqual(Subscribe(350, 350 + 10));
        ys[2].Subscriptions.AssertEqual(Subscribe(450, 450 + 30));
    }

    [TestMethod]
    public void Delay_DurationAndSubscription_Simple1()
    {

        var xs = Scheduler.CreateHotObservable(
            OnNext(150, 1),
            OnNext(210, 10),
            OnNext(220, 30),
            OnNext(230, 50),
            OnNext(240, 35),
            OnNext(250, 20),
            OnCompleted<int>(260)
        );

        var ys = Scheduler.CreateColdObservable(
            OnNext(10, "!")
        );

        var res = Scheduler.Start(() =>
            xs.Delay(ys, x => Scheduler.CreateColdObservable([OnNext(x, "!")]))
        );

        res.Messages.AssertEqual(
            OnNext(220 + 30, 30),
            OnNext(250 + 20, 20),
            OnNext(240 + 35, 35),
            OnNext(230 + 50, 50),
            OnCompleted<int>(280)
        );

        xs.Subscriptions.AssertEqual(
            Subscribe(210, 260)
        );

        ys.Subscriptions.AssertEqual(
            Subscribe(200, 210)
        );
    }

    [TestMethod]
    public void Delay_DurationAndSubscription_Simple2()
    {

        var xs = Scheduler.CreateHotObservable(
            OnNext(150, 1),
            OnNext(210, 10),
            OnNext(220, 30),
            OnNext(230, 50),
            OnNext(240, 35),
            OnNext(250, 20),
            OnCompleted<int>(260)
        );

        var ys = Scheduler.CreateColdObservable(
            OnCompleted<string>(10)
        );

        var res = Scheduler.Start(() =>
            xs.Delay(ys, x => Scheduler.CreateColdObservable([OnNext(x, "!")]))
        );

        res.Messages.AssertEqual(
            OnNext(220 + 30, 30),
            OnNext(250 + 20, 20),
            OnNext(240 + 35, 35),
            OnNext(230 + 50, 50),
            OnCompleted<int>(280)
        );

        xs.Subscriptions.AssertEqual(
            Subscribe(210, 260)
        );

        ys.Subscriptions.AssertEqual(
            Subscribe(200, 210)
        );
    }

    [TestMethod]
    public void Delay_DurationAndSubscription_Dispose1()
    {

        var xs = Scheduler.CreateHotObservable(
            OnNext(150, 1),
            OnNext(210, 10),
            OnNext(220, 30),
            OnNext(230, 50),
            OnNext(240, 35),
            OnNext(250, 20),
            OnCompleted<int>(260)
        );

        var ys = Scheduler.CreateColdObservable(
            OnNext(10, "!")
        );

        var res = Scheduler.Start(() =>
            xs.Delay(ys, x => Scheduler.CreateColdObservable([OnNext(x, "!")])),
            255
        );

        res.Messages.AssertEqual(
            OnNext(220 + 30, 30)
        );

        xs.Subscriptions.AssertEqual(
            Subscribe(210, 255)
        );

        ys.Subscriptions.AssertEqual(
            Subscribe(200, 210)
        );
    }

    [TestMethod]
    public void Delay_DurationAndSubscription_Dispose2()
    {

        var xs = Scheduler.CreateHotObservable(
            OnNext(150, 1),
            OnNext(210, 10),
            OnNext(220, 30),
            OnNext(230, 50),
            OnNext(240, 35),
            OnNext(250, 20),
            OnCompleted<int>(260)
        );

        var ys = Scheduler.CreateColdObservable(
            OnNext(100, "!")
        );

        var res = Scheduler.Start(() =>
            xs.Delay(ys, x => Scheduler.CreateColdObservable([OnNext(x, "!")])),
            255
        );

        res.Messages.AssertEqual(
        );

        xs.Subscriptions.AssertEqual(
        );

        ys.Subscriptions.AssertEqual(
            Subscribe(200, 255)
        );
    }

    [TestMethod]
    public void Delay_DurationAndSubscription_Error()
    {

        var ex = new Exception();

        var xs = Scheduler.CreateHotObservable(
            OnNext(150, 1),
            OnNext(210, 10),
            OnNext(220, 30),
            OnNext(230, 50),
            OnNext(240, 35),
            OnNext(250, 20),
            OnCompleted<int>(260)
        );

        var ys = Scheduler.CreateColdObservable(
            OnError<string>(10, ex)
        );

        var res = Scheduler.Start(() =>
            xs.Delay(ys, x => Scheduler.CreateColdObservable([OnNext(x, "!")]))
        );

        res.Messages.AssertEqual(
            OnError<int>(200 + 10, ex)
        );

        xs.Subscriptions.AssertEqual(
        );

        ys.Subscriptions.AssertEqual(
            Subscribe(200, 210)
        );
    }

    [TestMethod]
    public void Delay_Duration_Selector_Immediately()
    {
        var list = new List<int>();

        // Scheduler-free in the sync suite; here driven through the raw surface so the async
        // platform can run it (subscription is awaited within the pump).
        Scheduler.ScheduleAbsolute(Created, async () =>
        {
            await Observable.Range(1, 5).Delay(_ => Observable.Return(1)).SubscribeAsync(Scheduler, list.Add);
        });

        Scheduler.Start();

        CollectionAssert.AreEqual(new[] { 1, 2, 3, 4, 5 }, list);
    }

    [TestMethod]
    public void Delay_Selector_Immediate()
    {
        var result = 0;

        Scheduler.ScheduleAbsolute(Created, async () =>
        {
            var source = Observable.Return(1);
            var delayed = source.Delay(_ => Observable.Return(2));
            await delayed.SubscribeAsync(Scheduler, v => result = v);
        });

        Scheduler.Start();

        Assert.AreEqual(1, result);
    }
}

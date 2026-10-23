// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information. 

using System.Reactive.Concurrency;
using System.Reactive.Linq;

using Microsoft.Reactive.Testing;

using Tests.System.Reactive.Shared.Scenarios;

namespace Tests.System.Reactive.Shared.Rx.Tests;

/// <summary>
/// The shared <c>Timer</c> scenarios on Rx.NET, plus its own periodic-timer timekeeping tests.
/// </summary>
/// <remarks>
/// The seven <c>RepeatingTimer_*_Slippage*</c> tests and <c>RepeatingTimer_Start_CatchUp_Throws</c>
/// are the originals, 8 of <c>TimerTest.cs</c>'s 28. They pin how Rx.NET's periodic timer keeps
/// time when a handler outlasts the period: with the scheduler's stopwatch it catches up in a
/// burst of back-to-back elements, without one (<c>DisableOptimizations</c>) the emulated
/// periodic scheduling gives best effort, and an error thrown while catching up reaches the
/// scheduler's <c>Catch</c>. The handlers advance the <c>TestScheduler</c> from inside a dispatch
/// with <c>scheduler.Sleep</c>, which the pump forbids, and AsyncRx.NET's periodic timer has no
/// catch-up to pin (it waits a period after each element; a deferred question), so the tests run
/// here against Rx.NET only, as the originals but for MSTest's assertions and the nullable
/// annotations.
/// </remarks>
[TestClass]
public sealed class RxTimerTests : TimerTests
{
    protected override IRxTarget Target => RxTarget.Instance;

    [TestMethod]
    public void RepeatingTimer_UsingStopwatch_Slippage1()
    {
        var scheduler = new TestScheduler();

        var xs = default(IObservable<long>);
        scheduler.ScheduleAbsolute(100, () =>
        {
            xs = Observable.Timer(TimeSpan.Zero, TimeSpan.FromTicks(100), scheduler);
        });

        var times = new List<long>();

        var onNext = new Action<long>(x =>
        {
            times.Add(scheduler.Clock);

            if (x == 0)
            {
                return;
            }

            if (x < 2)
            {
                scheduler.Sleep(50);
                return;
            }

            if (x < 4)
            {
                scheduler.Sleep(120);
                return;
            }

            if (x < 6)
            {
                scheduler.Sleep(50);
                return;
            }

            if (x < 8)
            {
                return;
            }
        });

        var d = default(IDisposable);
        scheduler.ScheduleAbsolute(200, () => { d = xs!.Subscribe(onNext); });

        scheduler.ScheduleAbsolute(1000, () => { d!.Dispose(); });

        scheduler.Start();

        times.AssertEqual(
            201, // 1 off because of initial scheduling jump (InvokeStart)
            301,
            401,
            522, // 2 off because of 401 + 120 + 1 scheduling tick
            643, // 3 off because of 522 + 120 + 1 scheduling tick
            701,
            801,
            901
        );
    }

    [TestMethod]
    public void RepeatingTimer_UsingStopwatch_Slippage2()
    {
        var scheduler = new TestScheduler();

        var xs = default(IObservable<long>);
        scheduler.ScheduleAbsolute(100, () =>
        {
            xs = Observable.Timer(TimeSpan.FromTicks(100), TimeSpan.FromTicks(100), scheduler);
        });

        var times = new List<long>();

        var onNext = new Action<long>(x =>
        {
            times.Add(scheduler.Clock);

            if (x == 0)
            {
                return;
            }

            if (x < 2)
            {
                scheduler.Sleep(50);
                return;
            }

            if (x < 4)
            {
                scheduler.Sleep(120);
                return;
            }

            if (x < 6)
            {
                scheduler.Sleep(50);
                return;
            }

            if (x < 8)
            {
                return;
            }
        });

        var d = default(IDisposable);
        scheduler.ScheduleAbsolute(200, () => { d = xs!.Subscribe(onNext); });

        scheduler.ScheduleAbsolute(1000, () => { d!.Dispose(); });

        scheduler.Start();

        times.AssertEqual(
            300,
            400,
            500,
            621, // 1 off because of recursive scheduling beyond the target time
            742, // 2 off because of 621 + 120 + 1 scheduling tick
            800,
            900
        );
    }

    [TestMethod]
    public void RepeatingTimer_UsingStopwatch_Slippage3_CatchUpFromLongInvokeStart()
    {
        var scheduler = new TestScheduler();

        var xs = default(IObservable<long>);
        scheduler.ScheduleAbsolute(100, () =>
        {
            xs = Observable.Timer(TimeSpan.Zero, TimeSpan.FromTicks(100), scheduler);
        });

        var times = new List<long>();

        var onNext = new Action<long>(x =>
        {
            times.Add(scheduler.Clock);

            if (x == 0)
            {
                scheduler.Sleep(350);
                return;
            }
        });

        var d = default(IDisposable);
        scheduler.ScheduleAbsolute(200, () => { d = xs!.Subscribe(onNext); });

        scheduler.ScheduleAbsolute(1000, () => { d!.Dispose(); });

        scheduler.Start();

        times.AssertEqual(
            201, // 1 off because of initial scheduling jump (InvokeStart)
            551, // catching up after excessive delay of 350 (target was 300)
            552, // catching up after excessive delay of 350 (target was 400)
            553, // catching up after excessive delay of 350 (target was 500)
            601, // back in sync
            701,
            801,
            901
        );
    }

    [TestMethod]
    public void RepeatingTimer_UsingStopwatch_Slippage3_CatchUpFromLongInvokeStart_ThrowsFirst()
    {
        var ex = new Exception();

        var scheduler = new TestScheduler();

        var xs = default(IObservable<long>);
        scheduler.ScheduleAbsolute(100, () =>
        {
            xs = Observable.Timer(TimeSpan.Zero, TimeSpan.FromTicks(100), scheduler);
        });

        var onNext = new Action<long>(x =>
        {
            if (x == 0)
            {
                throw ex;
            }
        });

        var d = default(IDisposable);
        scheduler.ScheduleAbsolute(200, () => { d = xs!.Subscribe(onNext); });

        scheduler.ScheduleAbsolute(1000, () => { d!.Dispose(); });

        try
        {
            scheduler.Start();
        }
        catch (Exception e)
        {
            Assert.AreEqual(201, scheduler.Clock);
            Assert.AreSame(ex, e);
        }
    }

    [TestMethod]
    public void RepeatingTimer_UsingStopwatch_Slippage3_CatchUpFromLongInvokeStart_ThrowsBeyondFirst()
    {
        var ex = new Exception();

        var scheduler = new TestScheduler();

        var xs = default(IObservable<long>);
        scheduler.ScheduleAbsolute(100, () =>
        {
            xs = Observable.Timer(TimeSpan.Zero, TimeSpan.FromTicks(100), scheduler);
        });

        var times = new List<long>();

        var onNext = new Action<long>(x =>
        {
            times.Add(scheduler.Clock);

            if (x == 0)
            {
                scheduler.Sleep(350);
                return;
            }

            if (x == 5)
            {
                throw ex;
            }
        });

        var d = default(IDisposable);
        scheduler.ScheduleAbsolute(200, () => { d = xs!.Subscribe(onNext); });

        scheduler.ScheduleAbsolute(1000, () => { d!.Dispose(); });

        try
        {
            scheduler.Start();
        }
        catch (Exception e)
        {
            Assert.AreEqual(701, scheduler.Clock);
            Assert.AreSame(ex, e);
        }

        times.AssertEqual(
            201, // 1 off because of initial scheduling jump (InvokeStart)
            551, // catching up after excessive delay of 350 (target was 300)
            552, // catching up after excessive delay of 350 (target was 400)
            553, // catching up after excessive delay of 350 (target was 500)
            601, // back in sync
            701
        );
    }

    [TestMethod]
    public void RepeatingTimer_NoStopwatch_Slippage1()
    {
        var scheduler = new TestScheduler();

        var xs = default(IObservable<long>);
        scheduler.ScheduleAbsolute(100, () =>
        {
            xs = Observable.Timer(
                TimeSpan.Zero,
                TimeSpan.FromTicks(100),
                scheduler.DisableOptimizations(typeof(IStopwatchProvider)));
        });

        var times = new List<long>();

        var onNext = new Action<long>(x =>
        {
            times.Add(scheduler.Clock);

            if (x == 0)
            {
                return;
            }

            if (x < 2)
            {
                scheduler.Sleep(50);
                return;
            }

            if (x < 4)
            {
                scheduler.Sleep(120);
                return;
            }

            if (x < 6)
            {
                scheduler.Sleep(50);
                return;
            }

            if (x < 8)
            {
                return;
            }
        });

        var d = default(IDisposable);
        scheduler.ScheduleAbsolute(200, () => { d = xs!.Subscribe(onNext); });

        scheduler.ScheduleAbsolute(1000, () => { d!.Dispose(); });

        scheduler.Start();

        times.AssertEqual(
            201, // 1 off because of initial scheduling jump (InvokeStart)
            301,
            401,
            // 3 off because of 401 + 120 + 2 scheduling ticks (one due to yield in
            // SchedulePeriodic emulation code)
            523,
            // 5 off because of 523 + 120 + 2 scheduling ticks (one due to yield in
            // SchedulePeriodic emulation code)
            645,
            // 43 off because this situation (no stopwatch or periodic scheduling interface)
            // only gets best effort treatment (see SchedulePeriodic emulation code)
            743,
            843,
            943
        );
    }

    [TestMethod]
    public void RepeatingTimer_NoStopwatch_Slippage2()
    {
        var scheduler = new TestScheduler();

        var xs = default(IObservable<long>);
        scheduler.ScheduleAbsolute(100, () =>
        {
            xs = Observable.Timer(
                TimeSpan.FromTicks(100),
                TimeSpan.FromTicks(100),
                scheduler.DisableOptimizations(typeof(IStopwatchProvider)));
        });

        var times = new List<long>();

        var onNext = new Action<long>(x =>
        {
            times.Add(scheduler.Clock);

            if (x == 0)
            {
                return;
            }

            if (x < 2)
            {
                scheduler.Sleep(50);
                return;
            }

            if (x < 4)
            {
                scheduler.Sleep(120);
                return;
            }

            if (x < 6)
            {
                scheduler.Sleep(50);
                return;
            }

            if (x < 8)
            {
                return;
            }
        });

        var d = default(IDisposable);
        scheduler.ScheduleAbsolute(200, () => { d = xs!.Subscribe(onNext); });

        scheduler.ScheduleAbsolute(1000, () => { d!.Dispose(); });

        scheduler.Start();

        times.AssertEqual(
            300,
            400,
            500,
            // 2 off because of 500 + 120 + 2 scheduling ticks (one due to yield in
            // SchedulePeriodic emulation code)
            622,
            // 4 off because of 622 + 120 + 2 scheduling ticks (one due to yield in
            // SchedulePeriodic emulation code)
            744,
            // 42 off because this situation (no stopwatch or periodic scheduling interface)
            // only gets best effort treatment (see SchedulePeriodic emulation code)
            842,
            942
        );
    }

    [TestMethod]
    public void RepeatingTimer_Start_CatchUp_Throws()
    {
        var end = new ManualResetEvent(false);

        var err = new Exception();
        Exception? ex = null;

        var s = global::System.Reactive.Concurrency.ThreadPoolScheduler.Instance
            .Catch<Exception>(e =>
        {
            Interlocked.Exchange(ref ex, e);
            end.Set();
            return true;
        });

        var xs = Observable.Timer(TimeSpan.Zero, TimeSpan.FromMilliseconds(10), s);

        xs.Subscribe(x =>
        {
            if (x == 0)
            {
                Thread.Sleep(500);
            }

            if (x == 5)
            {
                throw err;
            }
        });

        end.WaitOne();

        Assert.AreSame(err, ex);
    }
}

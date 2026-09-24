// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information. 

using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Tests.System.Reactive.Shared.Scenarios;

/// <summary>
/// Shared <c>Window</c> scenarios: every behavioural test from Rx.NET's <c>WindowTest.cs</c>.
/// <c>Window</c> returns a <see cref="Nested{T}"/> description; the flattening idiom's
/// <c>Select((w, i) =&gt; w.Select(...))</c> receives each window as a value and returns a
/// description materialized in place, so the pipeline the platform runs is the one written here,
/// with no wrapping <c>Select</c> around the windows.
/// </summary>
/// <remarks>
/// Transcribed mechanically from the sync file, except the three
/// <c>WindowWithCount_InnerTimings*</c> tests, which were ported by hand into the raw surface's
/// async shape. The two scenarios whose closing selector throws need explicit
/// type arguments in the sync suite too; here the platform is one of them
/// (<c>xs.Window&lt;int, int&gt;(...)</c>), since extension methods take all or none. Deliberately not here:
/// the four <c>*_ArgumentChecking</c> tests (the code-generated stratum); the three <c>*_Default</c>
/// tests (they block on <c>First()</c> over the real default scheduler) and the two
/// <c>Window_Time_Basic_Periodic*</c> tests (they assert on Rx.NET's <c>ISchedulerPeriodic</c>
/// usage via a <c>PeriodicTestScheduler</c>, a sync-scheduler feature with no async counterpart) —
/// all five belong to the per-platform native stratum.
/// </remarks>
public abstract class WindowTests : DescribedTest
{
    [TestMethod]
    public void Window_Closings_Basic()
    {

        var xs = Scheduler.CreateHotObservable(
            OnNext(90, 1),
            OnNext(180, 2),
            OnNext(250, 3),
            OnNext(260, 4),
            OnNext(310, 5),
            OnNext(340, 6),
            OnNext(410, 7),
            OnNext(420, 8),
            OnNext(470, 9),
            OnNext(550, 10),
            OnCompleted<int>(590)
        );

        var window = 1;

        var res = Scheduler.Start(() =>
            xs.Window(() => Observable.Timer(TimeSpan.FromTicks((window++) * 100), Scheduler)).Select((w, i) => w.Select(x => i.ToString() + " " + x.ToString())).Merge()
        );

        res.Messages.AssertEqual(
            OnNext(250, "0 3"),
            OnNext(260, "0 4"),
            OnNext(310, "1 5"),
            OnNext(340, "1 6"),
            OnNext(410, "1 7"),
            OnNext(420, "1 8"),
            OnNext(470, "1 9"),
            OnNext(550, "2 10"),
            OnCompleted<string>(590)
        );

        xs.Subscriptions.AssertEqual(
            Subscribe(200, 590)
        );
    }

    [TestMethod]
    public void Window_Closings_InnerSubscriptions()
    {

        var xs = Scheduler.CreateHotObservable(
            OnNext(90, 1),
            OnNext(180, 2),
            OnNext(250, 3),
            OnNext(260, 4),
            OnNext(310, 5),
            OnNext(340, 6),
            OnNext(410, 7),
            OnNext(420, 8),
            OnNext(470, 9),
            OnNext(550, 10),
            OnCompleted<int>(590)
        );

        var closings = new TestableSeq<bool>[] {
            Scheduler.CreateHotObservable(
                OnNext(300, true),
                OnNext(350, false),
                OnCompleted<bool>(380)
            ),
            Scheduler.CreateHotObservable(
                OnNext(400, true),
                OnNext(510, false),
                OnNext(620, false)
            ),
            Scheduler.CreateHotObservable(
                OnCompleted<bool>(500)
            ),
            Scheduler.CreateHotObservable(
                OnNext(600, true)
            )
        };

        var window = 0;

        var res = Scheduler.Start(() =>
            xs.Window(() => closings[window++]).Select((w, i) => w.Select(x => i.ToString() + " " + x.ToString())).Merge()
        );

        res.Messages.AssertEqual(
            OnNext(250, "0 3"),
            OnNext(260, "0 4"),
            OnNext(310, "1 5"),
            OnNext(340, "1 6"),
            OnNext(410, "2 7"),
            OnNext(420, "2 8"),
            OnNext(470, "2 9"),
            OnNext(550, "3 10"),
            OnCompleted<string>(590)
        );

        xs.Subscriptions.AssertEqual(
            Subscribe(200, 590)
        );

        closings[0].Subscriptions.AssertEqual(
            Subscribe(200, 300)
        );

        closings[1].Subscriptions.AssertEqual(
            Subscribe(300, 400)
        );

        closings[2].Subscriptions.AssertEqual(
            Subscribe(400, 500)
        );

        closings[3].Subscriptions.AssertEqual(
            Subscribe(500, 590)
        );
    }

    [TestMethod]
    public void Window_Closings_Empty()
    {

        var xs = Scheduler.CreateHotObservable(
            OnNext(90, 1),
            OnNext(180, 2),
            OnNext(250, 3),
            OnNext(260, 4),
            OnNext(310, 5),
            OnNext(340, 6),
            OnNext(410, 7),
            OnNext(420, 8),
            OnNext(470, 9),
            OnNext(550, 10),
            OnCompleted<int>(590)
        );

        var window = 1;

        var res = Scheduler.Start(() =>
            xs.Window(() => Observable.Empty<int>().Delay(TimeSpan.FromTicks((window++) * 100), Scheduler)).Select((w, i) => w.Select(x => i.ToString() + " " + x.ToString())).Merge()
        );

        res.Messages.AssertEqual(
            OnNext(250, "0 3"),
            OnNext(260, "0 4"),
            OnNext(310, "1 5"),
            OnNext(340, "1 6"),
            OnNext(410, "1 7"),
            OnNext(420, "1 8"),
            OnNext(470, "1 9"),
            OnNext(550, "2 10"),
            OnCompleted<string>(590)
        );

        xs.Subscriptions.AssertEqual(
            Subscribe(200, 590)
        );
    }

    [TestMethod]
    public void Window_Closings_Dispose()
    {

        var xs = Scheduler.CreateHotObservable(
            OnNext(90, 1),
            OnNext(180, 2),
            OnNext(250, 3),
            OnNext(260, 4),
            OnNext(310, 5),
            OnNext(340, 6),
            OnNext(410, 7),
            OnNext(420, 8),
            OnNext(470, 9),
            OnNext(550, 10),
            OnCompleted<int>(590)
        );

        var window = 1;

        var res = Scheduler.Start(() =>
            xs.Window(() => Observable.Timer(TimeSpan.FromTicks((window++) * 100), Scheduler)).Select((w, i) => w.Select(x => i.ToString() + " " + x.ToString())).Merge(),
            400
        );

        res.Messages.AssertEqual(
            OnNext(250, "0 3"),
            OnNext(260, "0 4"),
            OnNext(310, "1 5"),
            OnNext(340, "1 6")
        );

        xs.Subscriptions.AssertEqual(
            Subscribe(200, 400)
        );
    }

    [TestMethod]
    public void Window_Closings_Error()
    {

        var ex = new Exception();

        var xs = Scheduler.CreateHotObservable(
            OnNext(90, 1),
            OnNext(180, 2),
            OnNext(250, 3),
            OnNext(260, 4),
            OnNext(310, 5),
            OnNext(340, 6),
            OnNext(410, 7),
            OnNext(420, 8),
            OnNext(470, 9),
            OnNext(550, 10),
            OnError<int>(590, ex)
        );

        var window = 1;

        var res = Scheduler.Start(() =>
            xs.Window(() => Observable.Timer(TimeSpan.FromTicks((window++) * 100), Scheduler)).Select((w, i) => w.Select(x => i.ToString() + " " + x.ToString())).Merge()
        );

        res.Messages.AssertEqual(
            OnNext(250, "0 3"),
            OnNext(260, "0 4"),
            OnNext(310, "1 5"),
            OnNext(340, "1 6"),
            OnNext(410, "1 7"),
            OnNext(420, "1 8"),
            OnNext(470, "1 9"),
            OnNext(550, "2 10"),
            OnError<string>(590, ex)
        );

        xs.Subscriptions.AssertEqual(
            Subscribe(200, 590)
        );
    }

    [TestMethod]
    public void Window_Closings_Throw()
    {

        var ex = new Exception();

        var xs = Scheduler.CreateHotObservable(
            OnNext(90, 1),
            OnNext(180, 2),
            OnNext(250, 3),
            OnNext(260, 4),
            OnNext(310, 5),
            OnNext(340, 6),
            OnNext(410, 7),
            OnNext(420, 8),
            OnNext(470, 9),
            OnNext(550, 10),
            OnError<int>(590, new Exception())
        );

        var res = Scheduler.Start(() =>
            xs.Window<int, int>(() => { throw ex; }).Select((w, i) => w.Select(x => i.ToString() + " " + x.ToString())).Merge()
        );

        res.Messages.AssertEqual(
            OnError<string>(200, ex)
        );

        xs.Subscriptions.AssertEqual(
            Subscribe(200, 200)
        );
    }

    [TestMethod]
    public void Window_Closings_WindowClose_Error()
    {

        var ex = new Exception();

        var xs = Scheduler.CreateHotObservable(
            OnNext(90, 1),
            OnNext(180, 2),
            OnNext(250, 3),
            OnNext(260, 4),
            OnNext(310, 5),
            OnNext(340, 6),
            OnNext(410, 7),
            OnNext(420, 8),
            OnNext(470, 9),
            OnNext(550, 10),
            OnError<int>(590, new Exception())
        );

        var res = Scheduler.Start(() =>
            xs.Window(() => Observable.Throw<int>(ex, Scheduler)).Select((w, i) => w.Select(x => i.ToString() + " " + x.ToString())).Merge()
        );

        res.Messages.AssertEqual(
            OnError<string>(ScheduledAt(200), ex)
        );

        xs.Subscriptions.AssertEqual(
            Subscribe(200, ScheduledAt(200))
        );
    }

    [TestMethod]
    public void Window_Closings_Default()
    {

        var xs = Scheduler.CreateHotObservable(
            OnNext(90, 1),
            OnNext(180, 2),
            OnNext(250, 3),
            OnNext(260, 4),
            OnNext(310, 5),
            OnNext(340, 6),
            OnNext(410, 7),
            OnNext(420, 8),
            OnNext(470, 9),
            OnNext(550, 10),
            OnCompleted<int>(590)
        );

        var window = 1;

        var res = Scheduler.Start(() =>
            xs.Window(() => Observable.Timer(TimeSpan.FromTicks((window++) * 100), Scheduler)).Select((w, i) => w.Select(x => i.ToString() + " " + x.ToString())).Merge()
        );

        res.Messages.AssertEqual(
            OnNext(250, "0 3"),
            OnNext(260, "0 4"),
            OnNext(310, "1 5"),
            OnNext(340, "1 6"),
            OnNext(410, "1 7"),
            OnNext(420, "1 8"),
            OnNext(470, "1 9"),
            OnNext(550, "2 10"),
            OnCompleted<string>(590)
        );

        xs.Subscriptions.AssertEqual(
            Subscribe(200, 590)
        );
    }

    [TestMethod]
    public void Window_OpeningClosings_Basic()
    {

        var xs = Scheduler.CreateHotObservable(
            OnNext(90, 1),
            OnNext(180, 2),
            OnNext(250, 3),
            OnNext(260, 4),
            OnNext(310, 5),
            OnNext(340, 6),
            OnNext(410, 7),
            OnNext(420, 8),
            OnNext(470, 9),
            OnNext(550, 10),
            OnCompleted<int>(590)
        );

        var ys = Scheduler.CreateHotObservable(
            OnNext(255, 50),
            OnNext(330, 100),
            OnNext(350, 50),
            OnNext(400, 90),
            OnCompleted<int>(900)
        );

        var res = Scheduler.Start(() =>
            xs.Window(ys, x => Observable.Timer(TimeSpan.FromTicks(x), Scheduler)).Select((w, i) => w.Select(x => i.ToString() + " " + x.ToString())).Merge()
        );

        res.Messages.AssertEqual(
            OnNext(260, "0 4"),
            OnNext(340, "1 6"),
            OnNext(410, "1 7"),
            OnNext(410, "3 7"),
            OnNext(420, "1 8"),
            OnNext(420, "3 8"),
            OnNext(470, "3 9"),
            OnCompleted<string>(900)
        );

        xs.Subscriptions.AssertEqual(
            Subscribe(200, 590)
        );

        ys.Subscriptions.AssertEqual(
            Subscribe(200, 900)
        );
    }

    [TestMethod]
    public void Window_OpeningClosings_Throw()
    {

        var xs = Scheduler.CreateHotObservable(
            OnNext(90, 1),
            OnNext(180, 2),
            OnNext(250, 3),
            OnNext(260, 4),
            OnNext(310, 5),
            OnNext(340, 6),
            OnNext(410, 7),
            OnNext(420, 8),
            OnNext(470, 9),
            OnNext(550, 10),
            OnCompleted<int>(590)
        );

        var ys = Scheduler.CreateHotObservable(
            OnNext(255, 50),
            OnNext(330, 100),
            OnNext(350, 50),
            OnNext(400, 90),
            OnCompleted<int>(900)
        );

        var ex = new Exception();

        var res = Scheduler.Start(() =>
            xs.Window<int, int, int>(ys, x => { throw ex; }).Select((w, i) => w.Select(x => i.ToString() + " " + x.ToString())).Merge()
        );

        res.Messages.AssertEqual(
            OnError<string>(255, ex)
        );

        xs.Subscriptions.AssertEqual(
            Subscribe(200, 255)
        );

        ys.Subscriptions.AssertEqual(
            Subscribe(200, 255)
        );
    }

    [TestMethod]
    public void Window_OpeningClosings_Dispose()
    {

        var xs = Scheduler.CreateHotObservable(
            OnNext(90, 1),
            OnNext(180, 2),
            OnNext(250, 3),
            OnNext(260, 4),
            OnNext(310, 5),
            OnNext(340, 6),
            OnNext(410, 7),
            OnNext(420, 8),
            OnNext(470, 9),
            OnNext(550, 10),
            OnCompleted<int>(590)
        );

        var ys = Scheduler.CreateHotObservable(
            OnNext(255, 50),
            OnNext(330, 100),
            OnNext(350, 50),
            OnNext(400, 90),
            OnCompleted<int>(900)
        );

        var res = Scheduler.Start(() =>
            xs.Window(ys, x => Observable.Timer(TimeSpan.FromTicks(x), Scheduler)).Select((w, i) => w.Select(x => i.ToString() + " " + x.ToString())).Merge(),
            415
        );

        res.Messages.AssertEqual(
            OnNext(260, "0 4"),
            OnNext(340, "1 6"),
            OnNext(410, "1 7"),
            OnNext(410, "3 7")
        );

        xs.Subscriptions.AssertEqual(
            Subscribe(200, 415)
        );

        ys.Subscriptions.AssertEqual(
            Subscribe(200, 415)
        );
    }

    [TestMethod]
    public void Window_OpeningClosings_Data_Error()
    {

        var ex = new Exception();

        var xs = Scheduler.CreateHotObservable(
            OnNext(90, 1),
            OnNext(180, 2),
            OnNext(250, 3),
            OnNext(260, 4),
            OnNext(310, 5),
            OnNext(340, 6),
            OnNext(410, 7),
            OnError<int>(415, ex)
        );

        var ys = Scheduler.CreateHotObservable(
            OnNext(255, 50),
            OnNext(330, 100),
            OnNext(350, 50),
            OnNext(400, 90),
            OnCompleted<int>(900)
        );

        var res = Scheduler.Start(() =>
            xs.Window(ys, x => Observable.Timer(TimeSpan.FromTicks(x), Scheduler)).Select((w, i) => w.Select(x => i.ToString() + " " + x.ToString())).Merge()
        );

        res.Messages.AssertEqual(
            OnNext(260, "0 4"),
            OnNext(340, "1 6"),
            OnNext(410, "1 7"),
            OnNext(410, "3 7"),
            OnError<string>(415, ex)
        );

        xs.Subscriptions.AssertEqual(
            Subscribe(200, 415)
        );

        ys.Subscriptions.AssertEqual(
            Subscribe(200, 415)
        );
    }

    [TestMethod]
    public void Window_OpeningClosings_Window_Error()
    {

        var ex = new Exception();

        var xs = Scheduler.CreateHotObservable(
            OnNext(90, 1),
            OnNext(180, 2),
            OnNext(250, 3),
            OnNext(260, 4),
            OnNext(310, 5),
            OnNext(340, 6),
            OnNext(410, 7),
            OnNext(420, 8),
            OnNext(470, 9),
            OnNext(550, 10),
            OnCompleted<int>(590)
        );

        var ys = Scheduler.CreateHotObservable(
            OnNext(255, 50),
            OnNext(330, 100),
            OnNext(350, 50),
            OnNext(400, 90),
            OnError<int>(415, ex)
        );

        var res = Scheduler.Start(() =>
            xs.Window(ys, x => Observable.Timer(TimeSpan.FromTicks(x), Scheduler)).Select((w, i) => w.Select(x => i.ToString() + " " + x.ToString())).Merge()
        );

        res.Messages.AssertEqual(
            OnNext(260, "0 4"),
            OnNext(340, "1 6"),
            OnNext(410, "1 7"),
            OnNext(410, "3 7"),
            OnError<string>(415, ex)
        );

        xs.Subscriptions.AssertEqual(
            Subscribe(200, 415)
        );

        ys.Subscriptions.AssertEqual(
            Subscribe(200, 415)
        );
    }

    [TestMethod]
    public void Window_Boundaries_Simple()
    {

        var xs = Scheduler.CreateHotObservable(
            OnNext(90, 1),
            OnNext(180, 2),
            OnNext(250, 3),
            OnNext(260, 4),
            OnNext(310, 5),
            OnNext(340, 6),
            OnNext(410, 7),
            OnNext(420, 8),
            OnNext(470, 9),
            OnNext(550, 10),
            OnCompleted<int>(590)
        );

        var ys = Scheduler.CreateHotObservable(
            OnNext(255, true),
            OnNext(330, true),
            OnNext(350, true),
            OnNext(400, true),
            OnNext(500, true),
            OnCompleted<bool>(900)
        );

        var res = Scheduler.Start(() =>
            xs.Window(ys).Select((w, i) => w.Select(x => i.ToString() + " " + x.ToString())).Merge()
        );

        res.Messages.AssertEqual(
            OnNext(250, "0 3"),
            OnNext(260, "1 4"),
            OnNext(310, "1 5"),
            OnNext(340, "2 6"),
            OnNext(410, "4 7"),
            OnNext(420, "4 8"),
            OnNext(470, "4 9"),
            OnNext(550, "5 10"),
            OnCompleted<string>(590)
        );

        xs.Subscriptions.AssertEqual(
            Subscribe(200, 590)
        );

        ys.Subscriptions.AssertEqual(
            Subscribe(200, 590)
        );
    }

    [TestMethod]
    public void Window_Boundaries_OnCompletedBoundaries()
    {

        var xs = Scheduler.CreateHotObservable(
            OnNext(90, 1),
            OnNext(180, 2),
            OnNext(250, 3),
            OnNext(260, 4),
            OnNext(310, 5),
            OnNext(340, 6),
            OnNext(410, 7),
            OnNext(420, 8),
            OnNext(470, 9),
            OnNext(550, 10),
            OnCompleted<int>(590)
        );

        var ys = Scheduler.CreateHotObservable(
            OnNext(255, true),
            OnNext(330, true),
            OnNext(350, true),
            OnCompleted<bool>(400)
        );

        var res = Scheduler.Start(() =>
            xs.Window(ys).Select((w, i) => w.Select(x => i.ToString() + " " + x.ToString())).Merge()
        );

        res.Messages.AssertEqual(
            OnNext(250, "0 3"),
            OnNext(260, "1 4"),
            OnNext(310, "1 5"),
            OnNext(340, "2 6"),
            OnCompleted<string>(400)
        );

        xs.Subscriptions.AssertEqual(
            Subscribe(200, 400)
        );

        ys.Subscriptions.AssertEqual(
            Subscribe(200, 400)
        );
    }

    [TestMethod]
    public void Window_Boundaries_OnErrorSource()
    {
        var ex = new Exception();

        var xs = Scheduler.CreateHotObservable(
            OnNext(90, 1),
            OnNext(180, 2),
            OnNext(250, 3),
            OnNext(260, 4),
            OnNext(310, 5),
            OnNext(340, 6),
            OnNext(380, 7),
            OnError<int>(400, ex)
        );

        var ys = Scheduler.CreateHotObservable(
            OnNext(255, true),
            OnNext(330, true),
            OnNext(350, true),
            OnCompleted<bool>(500)
        );

        var res = Scheduler.Start(() =>
            xs.Window(ys).Select((w, i) => w.Select(x => i.ToString() + " " + x.ToString())).Merge()
        );

        res.Messages.AssertEqual(
            OnNext(250, "0 3"),
            OnNext(260, "1 4"),
            OnNext(310, "1 5"),
            OnNext(340, "2 6"),
            OnNext(380, "3 7"),
            OnError<string>(400, ex)
        );

        xs.Subscriptions.AssertEqual(
            Subscribe(200, 400)
        );

        ys.Subscriptions.AssertEqual(
            Subscribe(200, 400)
        );
    }

    [TestMethod]
    public void Window_Boundaries_OnErrorBoundaries()
    {
        var ex = new Exception();

        var xs = Scheduler.CreateHotObservable(
            OnNext(90, 1),
            OnNext(180, 2),
            OnNext(250, 3),
            OnNext(260, 4),
            OnNext(310, 5),
            OnNext(340, 6),
            OnNext(410, 7),
            OnNext(420, 8),
            OnNext(470, 9),
            OnNext(550, 10),
            OnCompleted<int>(590)
        );

        var ys = Scheduler.CreateHotObservable(
            OnNext(255, true),
            OnNext(330, true),
            OnNext(350, true),
            OnError<bool>(400, ex)
        );

        var res = Scheduler.Start(() =>
            xs.Window(ys).Select((w, i) => w.Select(x => i.ToString() + " " + x.ToString())).Merge()
        );

        res.Messages.AssertEqual(
            OnNext(250, "0 3"),
            OnNext(260, "1 4"),
            OnNext(310, "1 5"),
            OnNext(340, "2 6"),
            OnError<string>(400, ex)
        );

        xs.Subscriptions.AssertEqual(
            Subscribe(200, 400)
        );

        ys.Subscriptions.AssertEqual(
            Subscribe(200, 400)
        );
    }

    [TestMethod]
    public void WindowWithCount_Basic()
    {

        var xs = Scheduler.CreateHotObservable(
            OnNext(100, 1),
            OnNext(210, 2),
            OnNext(240, 3),
            OnNext(280, 4),
            OnNext(320, 5),
            OnNext(350, 6),
            OnNext(380, 7),
            OnNext(420, 8),
            OnNext(470, 9),
            OnCompleted<int>(600)
        );

        var res = Scheduler.Start(() =>
            xs.Window(3, 2).Select((w, i) => w.Select(x => i.ToString() + " " + x.ToString())).Merge()
        );

        res.Messages.AssertEqual(
            OnNext(210, "0 2"),
            OnNext(240, "0 3"),
            OnNext(280, "0 4"),
            OnNext(280, "1 4"),
            OnNext(320, "1 5"),
            OnNext(350, "1 6"),
            OnNext(350, "2 6"),
            OnNext(380, "2 7"),
            OnNext(420, "2 8"),
            OnNext(420, "3 8"),
            OnNext(470, "3 9"),
            OnCompleted<string>(600)
        );

        xs.Subscriptions.AssertEqual(
            Subscribe(200, 600)
        );
    }

    [TestMethod]
    public void WindowWithCount_Disposed()
    {

        var xs = Scheduler.CreateHotObservable(
            OnNext(100, 1),
            OnNext(210, 2),
            OnNext(240, 3),
            OnNext(280, 4),
            OnNext(320, 5),
            OnNext(350, 6),
            OnNext(380, 7),
            OnNext(420, 8),
            OnNext(470, 9),
            OnCompleted<int>(600)
        );

        var res = Scheduler.Start(() =>
            xs.Window(3, 2).Select((w, i) => w.Select(x => i.ToString() + " " + x.ToString())).Merge(), 370
        );

        res.Messages.AssertEqual(
            OnNext(210, "0 2"),
            OnNext(240, "0 3"),
            OnNext(280, "0 4"),
            OnNext(280, "1 4"),
            OnNext(320, "1 5"),
            OnNext(350, "1 6"),
            OnNext(350, "2 6")
        );

        xs.Subscriptions.AssertEqual(
            Subscribe(200, 370)
        );
    }

    [TestMethod]
    public void WindowWithCount_Error()
    {

        var ex = new Exception();

        var xs = Scheduler.CreateHotObservable(
            OnNext(100, 1),
            OnNext(210, 2),
            OnNext(240, 3),
            OnNext(280, 4),
            OnNext(320, 5),
            OnNext(350, 6),
            OnNext(380, 7),
            OnNext(420, 8),
            OnNext(470, 9),
            OnError<int>(600, ex)
        );

        var res = Scheduler.Start(() =>
            xs.Window(3, 2).Select((w, i) => w.Select(x => i.ToString() + " " + x.ToString())).Merge()
        );

        res.Messages.AssertEqual(
            OnNext(210, "0 2"),
            OnNext(240, "0 3"),
            OnNext(280, "0 4"),
            OnNext(280, "1 4"),
            OnNext(320, "1 5"),
            OnNext(350, "1 6"),
            OnNext(350, "2 6"),
            OnNext(380, "2 7"),
            OnNext(420, "2 8"),
            OnNext(420, "3 8"),
            OnNext(470, "3 9"),
            OnError<string>(600, ex)
        );

        xs.Subscriptions.AssertEqual(
            Subscribe(200, 600)
        );
    }

    [TestMethod]
    public void Window_Time_Basic()
    {

        var xs = Scheduler.CreateHotObservable(
            OnNext(150, 1),
            OnNext(210, 2),
            OnNext(240, 3),
            OnNext(270, 4),
            OnNext(320, 5),
            OnNext(360, 6),
            OnNext(390, 7),
            OnNext(410, 8),
            OnNext(460, 9),
            OnNext(470, 10),
            OnCompleted<int>(490)
        );

        var res = Scheduler.Start(() =>
            xs.Window(TimeSpan.FromTicks(100), Scheduler).Select((ys, i) => ys.Select(y => i + " " + y).Concat(Observable.Return(i + " end"))).Merge()
        );

        res.Messages.AssertEqual(
            OnNext(210, "0 2"),
            OnNext(240, "0 3"),
            OnNext(270, "0 4"),
            OnNext(300, "0 end"),
            OnNext(320, "1 5"),
            OnNext(360, "1 6"),
            OnNext(390, "1 7"),
            OnNext(400, "1 end"),
            OnNext(410, "2 8"),
            OnNext(460, "2 9"),
            OnNext(470, "2 10"),
            OnNext(490, "2 end"),
            OnCompleted<string>(490)
        );

        xs.Subscriptions.AssertEqual(
            Subscribe(200, 490)
        );
    }

    [TestMethod]
    public void Window_Time_Basic_Both()
    {

        var xs = Scheduler.CreateHotObservable(
            OnNext(150, 1),
            OnNext(210, 2),
            OnNext(240, 3),
            OnNext(270, 4),
            OnNext(320, 5),
            OnNext(360, 6),
            OnNext(390, 7),
            OnNext(410, 8),
            OnNext(460, 9),
            OnNext(470, 10),
            OnCompleted<int>(490)
        );

        var res = Scheduler.Start(() =>
            xs.Window(TimeSpan.FromTicks(100), TimeSpan.FromTicks(50), Scheduler).Select((ys, i) => ys.Select(y => i + " " + y).Concat(Observable.Return(i + " end"))).Merge()
        );

        res.Messages.AssertEqual(
            OnNext(210, "0 2"),
            OnNext(240, "0 3"),
            OnNext(270, "0 4"),
            OnNext(270, "1 4"),
            OnNext(300, "0 end"),
            OnNext(320, "1 5"),
            OnNext(320, "2 5"),
            OnNext(350, "1 end"),
            OnNext(360, "2 6"),
            OnNext(360, "3 6"),
            OnNext(390, "2 7"),
            OnNext(390, "3 7"),
            OnNext(400, "2 end"),
            OnNext(410, "3 8"),
            OnNext(410, "4 8"),
            OnNext(450, "3 end"),
            OnNext(460, "4 9"),
            OnNext(460, "5 9"),
            OnNext(470, "4 10"),
            OnNext(470, "5 10"),
            OnNext(490, "4 end"),
            OnNext(490, "5 end"),
            OnCompleted<string>(490)
        );

        xs.Subscriptions.AssertEqual(
            Subscribe(200, 490)
        );
    }

    [TestMethod]
    public void WindowWithTime_Basic1()
    {

        var xs = Scheduler.CreateHotObservable(
            OnNext(100, 1),
            OnNext(210, 2),
            OnNext(240, 3),
            OnNext(280, 4),
            OnNext(320, 5),
            OnNext(350, 6),
            OnNext(380, 7),
            OnNext(420, 8),
            OnNext(470, 9),
            OnCompleted<int>(600)
        );

        var res = Scheduler.Start(() =>
            xs.Window(TimeSpan.FromTicks(100), TimeSpan.FromTicks(70), Scheduler).Select((w, i) => w.Select(x => i.ToString() + " " + x.ToString())).Merge()
        );

        res.Messages.AssertEqual(
            OnNext(210, "0 2"),
            OnNext(240, "0 3"),
            OnNext(280, "0 4"),
            OnNext(280, "1 4"),
            OnNext(320, "1 5"),
            OnNext(350, "1 6"),
            OnNext(350, "2 6"),
            OnNext(380, "2 7"),
            OnNext(420, "2 8"),
            OnNext(420, "3 8"),
            OnNext(470, "3 9"),
            OnCompleted<string>(600)
        );

        xs.Subscriptions.AssertEqual(
            Subscribe(200, 600)
        );
    }

    [TestMethod]
    public void WindowWithTime_Basic2()
    {

        var xs = Scheduler.CreateHotObservable(
            OnNext(100, 1),
            OnNext(210, 2),
            OnNext(240, 3),
            OnNext(280, 4),
            OnNext(320, 5),
            OnNext(350, 6),
            OnNext(380, 7),
            OnNext(420, 8),
            OnNext(470, 9),
            OnCompleted<int>(600)
        );

        var res = Scheduler.Start(() =>
            xs.Window(TimeSpan.FromTicks(70), TimeSpan.FromTicks(100), Scheduler).Select((w, i) => w.Select(x => i.ToString() + " " + x.ToString())).Merge()
        );

        res.Messages.AssertEqual(
            OnNext(210, "0 2"),
            OnNext(240, "0 3"),
            OnNext(320, "1 5"),
            OnNext(350, "1 6"),
            OnNext(420, "2 8"),
            OnNext(470, "2 9"),
            OnCompleted<string>(600)
        );

        xs.Subscriptions.AssertEqual(
            Subscribe(200, 600)
        );
    }

    [TestMethod]
    public void WindowWithTime_Error()
    {

        var ex = new Exception();

        var xs = Scheduler.CreateHotObservable(
            OnNext(100, 1),
            OnNext(210, 2),
            OnNext(240, 3),
            OnNext(280, 4),
            OnNext(320, 5),
            OnNext(350, 6),
            OnNext(380, 7),
            OnNext(420, 8),
            OnNext(470, 9),
            OnError<int>(600, ex)
        );

        var res = Scheduler.Start(() =>
            xs.Window(TimeSpan.FromTicks(100), TimeSpan.FromTicks(70), Scheduler).Select((w, i) => w.Select(x => i.ToString() + " " + x.ToString())).Merge()
        );

        res.Messages.AssertEqual(
            OnNext(210, "0 2"),
            OnNext(240, "0 3"),
            OnNext(280, "0 4"),
            OnNext(280, "1 4"),
            OnNext(320, "1 5"),
            OnNext(350, "1 6"),
            OnNext(350, "2 6"),
            OnNext(380, "2 7"),
            OnNext(420, "2 8"),
            OnNext(420, "3 8"),
            OnNext(470, "3 9"),
            OnError<string>(600, ex)
        );

        xs.Subscriptions.AssertEqual(
            Subscribe(200, 600)
        );
    }

    [TestMethod]
    public void WindowWithTime_Disposed()
    {

        var xs = Scheduler.CreateHotObservable(
            OnNext(100, 1),
            OnNext(210, 2),
            OnNext(240, 3),
            OnNext(280, 4),
            OnNext(320, 5),
            OnNext(350, 6),
            OnNext(380, 7),
            OnNext(420, 8),
            OnNext(470, 9),
            OnCompleted<int>(600)
        );

        var res = Scheduler.Start(() =>
            xs.Window(TimeSpan.FromTicks(100), TimeSpan.FromTicks(70), Scheduler).Select((w, i) => w.Select(x => i.ToString() + " " + x.ToString())).Merge(),
            370
        );

        res.Messages.AssertEqual(
            OnNext(210, "0 2"),
            OnNext(240, "0 3"),
            OnNext(280, "0 4"),
            OnNext(280, "1 4"),
            OnNext(320, "1 5"),
            OnNext(350, "1 6"),
            OnNext(350, "2 6")
        );

        xs.Subscriptions.AssertEqual(
            Subscribe(200, 370)
        );
    }

    [TestMethod]
    public void WindowWithTime_Basic_Same()
    {

        var xs = Scheduler.CreateHotObservable(
            OnNext(100, 1),
            OnNext(210, 2),
            OnNext(240, 3),
            OnNext(280, 4),
            OnNext(320, 5),
            OnNext(350, 6),
            OnNext(380, 7),
            OnNext(420, 8),
            OnNext(470, 9),
            OnCompleted<int>(600)
        );

        var res = Scheduler.Start(() =>
            xs.Window(TimeSpan.FromTicks(100), Scheduler).Select((w, i) => w.Select(x => i.ToString() + " " + x.ToString())).Merge()
        );

        res.Messages.AssertEqual(
            OnNext(210, "0 2"),
            OnNext(240, "0 3"),
            OnNext(280, "0 4"),
            OnNext(320, "1 5"),
            OnNext(350, "1 6"),
            OnNext(380, "1 7"),
            OnNext(420, "2 8"),
            OnNext(470, "2 9"),
            OnCompleted<string>(600)
        );

        xs.Subscriptions.AssertEqual(
            Subscribe(200, 600)
        );
    }

    [TestMethod]
    public void WindowWithTimeOrCount_Basic()
    {

        var xs = Scheduler.CreateHotObservable(
            OnNext(205, 1),
            OnNext(210, 2),
            OnNext(240, 3),
            OnNext(280, 4),
            OnNext(320, 5),
            OnNext(350, 6),
            OnNext(370, 7),
            OnNext(420, 8),
            OnNext(470, 9),
            OnCompleted<int>(600)
        );

        var res = Scheduler.Start(() =>
            xs.Window(TimeSpan.FromTicks(70), 3, Scheduler).Select((w, i) => w.Select(x => i.ToString() + " " + x.ToString())).Merge()
        );

        res.Messages.AssertEqual(
            OnNext(205, "0 1"),
            OnNext(210, "0 2"),
            OnNext(240, "0 3"),
            OnNext(280, "1 4"),
            OnNext(320, "2 5"),
            OnNext(350, "2 6"),
            OnNext(370, "2 7"),
            OnNext(420, "3 8"),
            OnNext(470, "4 9"),
            OnCompleted<string>(600)
        );

        xs.Subscriptions.AssertEqual(
            Subscribe(200, 600)
        );
    }

    [TestMethod]
    public void WindowWithTimeOrCount_Error()
    {

        var ex = new Exception();

        var xs = Scheduler.CreateHotObservable(
            OnNext(205, 1),
            OnNext(210, 2),
            OnNext(240, 3),
            OnNext(280, 4),
            OnNext(320, 5),
            OnNext(350, 6),
            OnNext(370, 7),
            OnNext(420, 8),
            OnNext(470, 9),
            OnError<int>(600, ex)
        );

        var res = Scheduler.Start(() =>
            xs.Window(TimeSpan.FromTicks(70), 3, Scheduler).Select((w, i) => w.Select(x => i.ToString() + " " + x.ToString())).Merge()
        );

        res.Messages.AssertEqual(
            OnNext(205, "0 1"),
            OnNext(210, "0 2"),
            OnNext(240, "0 3"),
            OnNext(280, "1 4"),
            OnNext(320, "2 5"),
            OnNext(350, "2 6"),
            OnNext(370, "2 7"),
            OnNext(420, "3 8"),
            OnNext(470, "4 9"),
            OnError<string>(600, ex)
        );

        xs.Subscriptions.AssertEqual(
            Subscribe(200, 600)
        );
    }

    [TestMethod]
    public void WindowWithTimeOrCount_Disposed()
    {

        var xs = Scheduler.CreateHotObservable(
            OnNext(205, 1),
            OnNext(210, 2),
            OnNext(240, 3),
            OnNext(280, 4),
            OnNext(320, 5),
            OnNext(350, 6),
            OnNext(370, 7),
            OnNext(420, 8),
            OnNext(470, 9),
            OnCompleted<int>(600)
        );

        var res = Scheduler.Start(() =>
            xs.Window(TimeSpan.FromTicks(70), 3, Scheduler).Select((w, i) => w.Select(x => i.ToString() + " " + x.ToString())).Merge(),
            370
        );

        res.Messages.AssertEqual(
            OnNext(205, "0 1"),
            OnNext(210, "0 2"),
            OnNext(240, "0 3"),
            OnNext(280, "1 4"),
            OnNext(320, "2 5"),
            OnNext(350, "2 6"),
            OnNext(370, "2 7")
        );

        xs.Subscriptions.AssertEqual(
            Subscribe(200, 370)
        );
    }

    [TestMethod]
    public void WindowWithCount_InnerTimings()
    {
        var xs = Scheduler.CreateHotObservable(
            OnNext(100, 1),
            OnNext(210, 2),
            OnNext(240, 3),
            OnNext(280, 4),
            OnNext(320, 5),
            OnNext(350, 6),
            OnNext(380, 7),
            OnNext(420, 8),
            OnNext(470, 9),
            OnCompleted<int>(600)
        );

        var res = default(Nested<int>);
        var outerSubscription = default(IAsyncDisposable);
        var innerSubscriptions = new List<IAsyncDisposable>();
        var windows = new List<Seq<int>>();
        var observers = new List<TestableObserver<int>>();

        Scheduler.ScheduleAbsolute(Created, () => res = xs.Window(3, 2));

        Scheduler.ScheduleAbsolute(Subscribed, async () =>
        {
            outerSubscription = await res!.SubscribeAsync(Scheduler,
                window =>
                {
                    var result = Scheduler.CreateObserver<int>();
                    windows.Add(window);
                    observers.Add(result);
                    Scheduler.ScheduleAbsolute(0, async () => innerSubscriptions.Add(await window.SubscribeAsync(result)));
                }
            );
        });

        Scheduler.Start();

        Assert.AreEqual(5, observers.Count);

        observers[0].Messages.AssertEqual(
            OnNext(210, 2),
            OnNext(240, 3),
            OnNext(280, 4),
            OnCompleted<int>(280)
        );

        observers[1].Messages.AssertEqual(
            OnNext(280, 4),
            OnNext(320, 5),
            OnNext(350, 6),
            OnCompleted<int>(350)
        );

        observers[2].Messages.AssertEqual(
            OnNext(350, 6),
            OnNext(380, 7),
            OnNext(420, 8),
            OnCompleted<int>(420)
        );

        observers[3].Messages.AssertEqual(
            OnNext(420, 8),
            OnNext(470, 9),
            OnCompleted<int>(600)
        );

        observers[4].Messages.AssertEqual(
            OnCompleted<int>(600)
        );

        xs.Subscriptions.AssertEqual(
            Subscribe(200, 600)
        );
    }

    [TestMethod]
    public void WindowWithCount_InnerTimings_DisposeOuter()
    {
        var xs = Scheduler.CreateHotObservable(
            OnNext(100, 1),
            OnNext(210, 2),
            OnNext(240, 3),
            OnNext(280, 4),
            OnNext(320, 5),
            OnNext(350, 6),
            OnNext(380, 7),
            OnNext(420, 8),
            OnNext(470, 9),
            OnCompleted<int>(600)
        );

        var res = default(Nested<int>);
        var outerSubscription = default(IAsyncDisposable);
        var innerSubscriptions = new List<IAsyncDisposable>();
        var windows = new List<Seq<int>>();
        var observers = new List<TestableObserver<int>>();
        var windowCreationTimes = new List<long>();

        Scheduler.ScheduleAbsolute(Created, () => res = xs.Window(3, 2));

        Scheduler.ScheduleAbsolute(Subscribed, async () =>
        {
            outerSubscription = await res!.SubscribeAsync(Scheduler,
                window =>
                {
                    windowCreationTimes.Add(Scheduler.Clock);

                    var result = Scheduler.CreateObserver<int>();
                    windows.Add(window);
                    observers.Add(result);
                    Scheduler.ScheduleAbsolute(0, async () => innerSubscriptions.Add(await window.SubscribeAsync(result)));
                }
            );
        });

        Scheduler.ScheduleAbsolute(400, async () =>
        {
            await outerSubscription!.DisposeAsync();
        });

        Scheduler.Start();

        Assert.IsTrue(windowCreationTimes.Last() < 400);

        Assert.AreEqual(4, observers.Count);

        observers[0].Messages.AssertEqual(
            OnNext(210, 2),
            OnNext(240, 3),
            OnNext(280, 4),
            OnCompleted<int>(280)
        );

        observers[1].Messages.AssertEqual(
            OnNext(280, 4),
            OnNext(320, 5),
            OnNext(350, 6),
            OnCompleted<int>(350)
        );

        observers[2].Messages.AssertEqual(
            OnNext(350, 6),
            OnNext(380, 7),
            OnNext(420, 8),
            OnCompleted<int>(420)
        );

        observers[3].Messages.AssertEqual(
            OnNext(420, 8),
            OnNext(470, 9),
            OnCompleted<int>(600)
        );

        xs.Subscriptions.AssertEqual(
            Subscribe(200, 600)
        );
    }

    [TestMethod]
    public void WindowWithCount_InnerTimings_DisposeOuterAndInners()
    {
        var xs = Scheduler.CreateHotObservable(
            OnNext(100, 1),
            OnNext(210, 2),
            OnNext(240, 3),
            OnNext(280, 4),
            OnNext(320, 5),
            OnNext(350, 6),
            OnNext(380, 7),
            OnNext(420, 8),
            OnNext(470, 9),
            OnCompleted<int>(600)
        );

        var res = default(Nested<int>);
        var outerSubscription = default(IAsyncDisposable);
        var innerSubscriptions = new List<IAsyncDisposable>();
        var windows = new List<Seq<int>>();
        var observers = new List<TestableObserver<int>>();
        var windowCreationTimes = new List<long>();

        Scheduler.ScheduleAbsolute(Created, () => res = xs.Window(3, 2));

        Scheduler.ScheduleAbsolute(Subscribed, async () =>
        {
            outerSubscription = await res!.SubscribeAsync(Scheduler,
                window =>
                {
                    windowCreationTimes.Add(Scheduler.Clock);

                    var result = Scheduler.CreateObserver<int>();
                    windows.Add(window);
                    observers.Add(result);
                    Scheduler.ScheduleAbsolute(0, async () => innerSubscriptions.Add(await window.SubscribeAsync(result)));
                }
            );
        });

        Scheduler.ScheduleAbsolute(400, async () =>
        {
            await outerSubscription!.DisposeAsync();
            foreach (var d in innerSubscriptions)
            {
                await d.DisposeAsync();
            }
        });

        Scheduler.Start();

        Assert.IsTrue(windowCreationTimes.Last() < 400);

        Assert.AreEqual(4, observers.Count);

        observers[0].Messages.AssertEqual(
            OnNext(210, 2),
            OnNext(240, 3),
            OnNext(280, 4),
            OnCompleted<int>(280)
        );

        observers[1].Messages.AssertEqual(
            OnNext(280, 4),
            OnNext(320, 5),
            OnNext(350, 6),
            OnCompleted<int>(350)
        );

        observers[2].Messages.AssertEqual(
            OnNext(350, 6),
            OnNext(380, 7)
        );

        observers[3].Messages.AssertEqual(
        );

        xs.Subscriptions.AssertEqual(
            Subscribe(200, 400)
        );
    }
}

// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information. 

namespace Tests.System.Reactive.Shared.Scenarios;

/// <summary>Shared <c>Buffer</c> scenarios, from Rx.NET's <c>BufferTest.cs</c>.</summary>
/// <remarks>
/// <para>
/// 32 of the file's 41 tests. Twenty-nine transcribe as they are: the closing-selector,
/// openings, boundaries, count, time, time-shift and time-or-count forms, all virtual-time, with
/// the Rx v2 expectation where the original carries two under <c>NO_PERF</c>. The three
/// <c>*_Default</c> tests run the scheduler-less overloads on the real default scheduler, as the
/// <c>Window</c> scenarios do: the originals block on <c>First()</c> after a <c>Skip</c>; here
/// they await the whole output through <see cref="SharedReactiveTest.ToListAsync{T}"/> and
/// assert the buffer the original picked out and the count of buffers.
/// </para>
/// <para>
/// Not here: the six <c>*_ArgumentChecking</c> tests (the code-generated stratum); the two
/// <c>BufferWithTime_Basic_Same_Periodic*</c> tests, which assert on Rx.NET's
/// <c>ISchedulerPeriodic</c> usage through a <c>PeriodicTestScheduler</c>, a sync-scheduler
/// feature with no async counterpart, left out as their <c>Window</c> twins were; and
/// <c>BufferWithTime_TickWhileOnCompleted</c>, which advances the test scheduler from inside a
/// handler while a subscribe is delivering, a reentrancy the pump does not allow (nor does
/// Rx.NET's scheduler while it is running), so it is written against Rx.NET in its runner.
/// </para>
/// </remarks>
public abstract class BufferTests : SharedReactiveTest
{
    [TestMethod]
    public void Buffer_Closings_Basic()
    {
        var scheduler = Scheduler;

        var xs = scheduler.CreateHotObservable(
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

        var res = scheduler.Start(() =>
            xs.Buffer(() => Seq.Timer(TimeSpan.FromTicks((window++) * 100), scheduler))
        );

        res.Messages.AssertEqual(
            OnNext<IList<int>>(300, b => b.SequenceEqual([3, 4])),
            OnNext<IList<int>>(500, b => b.SequenceEqual([5, 6, 7, 8, 9])),
            OnNext<IList<int>>(590, b => b.SequenceEqual([10])),
            OnCompleted<IList<int>>(590)
        );

        xs.Subscriptions.AssertEqual(
            Subscribe(200, 590)
        );
    }

    [TestMethod]
    public void Buffer_Closings_InnerSubscriptions()
    {
        var scheduler = Scheduler;

        var xs = scheduler.CreateHotObservable(
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
            scheduler.CreateHotObservable(
                OnNext(300, true),
                OnNext(350, false),
                OnCompleted<bool>(380)
            ),
            scheduler.CreateHotObservable(
                OnNext(400, true),
                OnNext(510, false),
                OnNext(620, false)
            ),
            scheduler.CreateHotObservable(
                OnCompleted<bool>(500)
            ),
            scheduler.CreateHotObservable(
                OnNext(600, true)
            )
        };

        var window = 0;

        var res = scheduler.Start(() =>
            xs.Buffer(() => closings[window++])
        );

        res.Messages.AssertEqual(
            OnNext<IList<int>>(300, b => b.SequenceEqual([3, 4])),
            OnNext<IList<int>>(400, b => b.SequenceEqual([5, 6])),
            OnNext<IList<int>>(500, b => b.SequenceEqual([7, 8, 9])),
            OnNext<IList<int>>(590, b => b.SequenceEqual([10])),
            OnCompleted<IList<int>>(590)
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
    public void Buffer_Closings_Empty()
    {
        var scheduler = Scheduler;

        var xs = scheduler.CreateHotObservable(
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

        var res = scheduler.Start(() =>
            xs.Buffer(() => Seq.Empty<int>().Delay(TimeSpan.FromTicks((window++) * 100), scheduler))
        );

        res.Messages.AssertEqual(
            OnNext<IList<int>>(300, l => l.SequenceEqual([3, 4])),
            OnNext<IList<int>>(500, l => l.SequenceEqual([5, 6, 7, 8, 9])),
            OnNext<IList<int>>(590, l => l.SequenceEqual([10])),
            OnCompleted<IList<int>>(590)
        );

        xs.Subscriptions.AssertEqual(
            Subscribe(200, 590)
        );
    }

    [TestMethod]
    public void Buffer_Closings_Dispose()
    {
        var scheduler = Scheduler;

        var xs = scheduler.CreateHotObservable(
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

        var res = scheduler.Start(() =>
            xs.Buffer(() => Seq.Timer(TimeSpan.FromTicks((window++) * 100), scheduler)),
            400
        );

        res.Messages.AssertEqual(
            OnNext<IList<int>>(300, l => l.SequenceEqual([3, 4]))
        );

        xs.Subscriptions.AssertEqual(
            Subscribe(200, 400)
        );
    }

    [TestMethod]
    public void Buffer_Closings_Error()
    {
        var scheduler = Scheduler;

        var ex = new Exception();

        var xs = scheduler.CreateHotObservable(
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

        var res = scheduler.Start(() =>
            xs.Buffer(() => Seq.Timer(TimeSpan.FromTicks((window++) * 100), scheduler))
        );

        res.Messages.AssertEqual(
            OnNext<IList<int>>(300, l => l.SequenceEqual([3, 4])),
            OnNext<IList<int>>(500, l => l.SequenceEqual([5, 6, 7, 8, 9])),
            OnError<IList<int>>(590, ex)
        );

        xs.Subscriptions.AssertEqual(
            Subscribe(200, 590)
        );
    }

    [TestMethod]
    public void Buffer_Closings_Throw()
    {
        var scheduler = Scheduler;

        var ex = new Exception();

        var xs = scheduler.CreateHotObservable(
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

        var res = scheduler.Start(() =>
            xs.Buffer<int, int>(() => { throw ex; })
        );

        res.Messages.AssertEqual(
            OnError<IList<int>>(200, ex)
        );

        xs.Subscriptions.AssertEqual(
            Subscribe(200, 200)
        );
    }

    [TestMethod]
    public void Buffer_Closings_WindowClose_Error()
    {
        var scheduler = Scheduler;

        var ex = new Exception();

        var xs = scheduler.CreateHotObservable(
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

        var res = scheduler.Start(() =>
            xs.Buffer(() => Seq.Throw<int>(ex, scheduler))
        );

        res.Messages.AssertEqual(
            OnError<IList<int>>(ScheduledAt(200), ex)
        );

        xs.Subscriptions.AssertEqual(
            Subscribe(200, ScheduledAt(200))
        );
    }

    [TestMethod]
    public void Buffer_OpeningClosings_Basic()
    {
        var scheduler = Scheduler;

        var xs = scheduler.CreateHotObservable(
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

        var ys = scheduler.CreateHotObservable(
            OnNext(255, 50),
            OnNext(330, 100),
            OnNext(350, 50),
            OnNext(400, 90),
            OnCompleted<int>(900)
        );

        var res = scheduler.Start(() =>
            xs.Buffer(ys, x => Seq.Timer(TimeSpan.FromTicks(x), scheduler))
        );

        res.Messages.AssertEqual(
            OnNext<IList<int>>(305, b => b.SequenceEqual([4])),
            OnNext<IList<int>>(400, b => b.SequenceEqual([])),
            OnNext<IList<int>>(430, b => b.SequenceEqual([6, 7, 8])),
            OnNext<IList<int>>(490, b => b.SequenceEqual([7, 8, 9])),
            OnCompleted<IList<int>>(900)
        );

        xs.Subscriptions.AssertEqual(
            Subscribe(200, 590)
        );

        ys.Subscriptions.AssertEqual(
            Subscribe(200, 900)
        );
    }

    [TestMethod]
    public void Buffer_Boundaries_Simple()
    {
        var scheduler = Scheduler;

        var xs = scheduler.CreateHotObservable(
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

        var ys = scheduler.CreateHotObservable(
            OnNext(255, true),
            OnNext(330, true),
            OnNext(350, true),
            OnNext(400, true),
            OnNext(500, true),
            OnCompleted<bool>(900)
        );

        var res = scheduler.Start(() =>
            xs.Buffer(ys)
        );

        res.Messages.AssertEqual(
            OnNext<IList<int>>(255, b => b.SequenceEqual([3])),
            OnNext<IList<int>>(330, b => b.SequenceEqual([4, 5])),
            OnNext<IList<int>>(350, b => b.SequenceEqual([6])),
            OnNext<IList<int>>(400, b => b.SequenceEqual([])),
            OnNext<IList<int>>(500, b => b.SequenceEqual([7, 8, 9])),
            OnNext<IList<int>>(590, b => b.SequenceEqual([10])),
            OnCompleted<IList<int>>(590)
        );

        xs.Subscriptions.AssertEqual(
            Subscribe(200, 590)
        );

        ys.Subscriptions.AssertEqual(
            Subscribe(200, 590)
        );
    }

    [TestMethod]
    public void Buffer_Boundaries_OnCompletedBoundaries()
    {
        var scheduler = Scheduler;

        var xs = scheduler.CreateHotObservable(
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

        var ys = scheduler.CreateHotObservable(
            OnNext(255, true),
            OnNext(330, true),
            OnNext(350, true),
            OnCompleted<bool>(400)
        );

        var res = scheduler.Start(() =>
            xs.Buffer(ys)
        );

        res.Messages.AssertEqual(
            OnNext<IList<int>>(255, b => b.SequenceEqual([3])),
            OnNext<IList<int>>(330, b => b.SequenceEqual([4, 5])),
            OnNext<IList<int>>(350, b => b.SequenceEqual([6])),
            OnNext<IList<int>>(400, b => b.SequenceEqual([])),
            OnCompleted<IList<int>>(400)
        );

        xs.Subscriptions.AssertEqual(
            Subscribe(200, 400)
        );

        ys.Subscriptions.AssertEqual(
            Subscribe(200, 400)
        );
    }

    [TestMethod]
    public void Buffer_Boundaries_OnErrorSource()
    {
        var ex = new Exception();

        var scheduler = Scheduler;

        var xs = scheduler.CreateHotObservable(
            OnNext(90, 1),
            OnNext(180, 2),
            OnNext(250, 3),
            OnNext(260, 4),
            OnNext(310, 5),
            OnNext(340, 6),
            OnNext(380, 7),
            OnError<int>(400, ex)
        );

        var ys = scheduler.CreateHotObservable(
            OnNext(255, true),
            OnNext(330, true),
            OnNext(350, true),
            OnCompleted<bool>(500)
        );

        var res = scheduler.Start(() =>
            xs.Buffer(ys)
        );

        res.Messages.AssertEqual(
            OnNext<IList<int>>(255, b => b.SequenceEqual([3])),
            OnNext<IList<int>>(330, b => b.SequenceEqual([4, 5])),
            OnNext<IList<int>>(350, b => b.SequenceEqual([6])),
            OnError<IList<int>>(400, ex)
        );

        xs.Subscriptions.AssertEqual(
            Subscribe(200, 400)
        );

        ys.Subscriptions.AssertEqual(
            Subscribe(200, 400)
        );
    }

    [TestMethod]
    public void Buffer_Boundaries_OnErrorBoundaries()
    {
        var ex = new Exception();

        var scheduler = Scheduler;

        var xs = scheduler.CreateHotObservable(
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

        var ys = scheduler.CreateHotObservable(
            OnNext(255, true),
            OnNext(330, true),
            OnNext(350, true),
            OnError<bool>(400, ex)
        );

        var res = scheduler.Start(() =>
            xs.Buffer(ys)
        );

        res.Messages.AssertEqual(
            OnNext<IList<int>>(255, b => b.SequenceEqual([3])),
            OnNext<IList<int>>(330, b => b.SequenceEqual([4, 5])),
            OnNext<IList<int>>(350, b => b.SequenceEqual([6])),
            OnError<IList<int>>(400, ex)
        );

        xs.Subscriptions.AssertEqual(
            Subscribe(200, 400)
        );

        ys.Subscriptions.AssertEqual(
            Subscribe(200, 400)
        );
    }

    [TestMethod]
    public void Buffer_Count_PartialWindow()
    {
        var scheduler = Scheduler;

        var xs = scheduler.CreateHotObservable(
            OnNext(150, 1),
            OnNext(210, 2),
            OnNext(220, 3),
            OnNext(230, 4),
            OnNext(240, 5),
            OnCompleted<int>(250)
        );

        var res = scheduler.Start(() =>
            xs.Buffer(5)
        );

        res.Messages.AssertEqual(
            OnNext<IList<int>>(250, l => l.SequenceEqual([2, 3, 4, 5])),
            OnCompleted<IList<int>>(250)
        );

        xs.Subscriptions.AssertEqual(
            Subscribe(200, 250)
        );
    }

    [TestMethod]
    public void Buffer_Count_FullWindows()
    {
        var scheduler = Scheduler;

        var xs = scheduler.CreateHotObservable(
            OnNext(150, 1),
            OnNext(210, 2),
            OnNext(220, 3),
            OnNext(230, 4),
            OnNext(240, 5),
            OnCompleted<int>(250)
        );

        var res = scheduler.Start(() =>
            xs.Buffer(2)
        );

        res.Messages.AssertEqual(
            OnNext<IList<int>>(220, l => l.SequenceEqual([2, 3])),
            OnNext<IList<int>>(240, l => l.SequenceEqual([4, 5])),
            OnCompleted<IList<int>>(250)
        );

        xs.Subscriptions.AssertEqual(
            Subscribe(200, 250)
        );
    }

    [TestMethod]
    public void Buffer_Count_FullAndPartialWindows()
    {
        var scheduler = Scheduler;

        var xs = scheduler.CreateHotObservable(
            OnNext(150, 1),
            OnNext(210, 2),
            OnNext(220, 3),
            OnNext(230, 4),
            OnNext(240, 5),
            OnCompleted<int>(250)
        );

        var res = scheduler.Start(() =>
            xs.Buffer(3)
        );

        res.Messages.AssertEqual(
            OnNext<IList<int>>(230, l => l.SequenceEqual([2, 3, 4])),
            OnNext<IList<int>>(250, l => l.SequenceEqual([5])),
            OnCompleted<IList<int>>(250)
        );

        xs.Subscriptions.AssertEqual(
            Subscribe(200, 250)
        );
    }

    [TestMethod]
    public void Buffer_Count_Error()
    {
        var scheduler = Scheduler;

        var ex = new Exception();

        var xs = scheduler.CreateHotObservable(
            OnNext(150, 1),
            OnNext(210, 2),
            OnNext(220, 3),
            OnNext(230, 4),
            OnNext(240, 5),
            OnError<int>(250, ex)
        );

        var res = scheduler.Start(() =>
            xs.Buffer(5)
        );

        res.Messages.AssertEqual(
            OnError<IList<int>>(250, ex)
        );

        xs.Subscriptions.AssertEqual(
            Subscribe(200, 250)
        );
    }

    [TestMethod]
    public void Buffer_Count_Skip_Less()
    {
        var scheduler = Scheduler;

        var xs = scheduler.CreateHotObservable(
            OnNext(150, 1),
            OnNext(210, 2),
            OnNext(220, 3),
            OnNext(230, 4),
            OnNext(240, 5),
            OnCompleted<int>(250)
        );

        var res = scheduler.Start(() =>
            xs.Buffer(3, 1)
        );

        res.Messages.AssertEqual(
            OnNext<IList<int>>(230, l => l.SequenceEqual([2, 3, 4])),
            OnNext<IList<int>>(240, l => l.SequenceEqual([3, 4, 5])),
            OnNext<IList<int>>(250, l => l.SequenceEqual([4, 5])),
            OnNext<IList<int>>(250, l => l.SequenceEqual([5])),
            OnCompleted<IList<int>>(250)
        );

        xs.Subscriptions.AssertEqual(
            Subscribe(200, 250)
        );
    }

    [TestMethod]
    public void Buffer_Count_Skip_More()
    {
        var scheduler = Scheduler;

        var xs = scheduler.CreateHotObservable(
            OnNext(150, 1),
            OnNext(210, 2),
            OnNext(220, 3),
            OnNext(230, 4),
            OnNext(240, 5),
            OnCompleted<int>(250)
        );

        var res = scheduler.Start(() =>
            xs.Buffer(2, 3)
        );

        res.Messages.AssertEqual(
            OnNext<IList<int>>(220, l => l.SequenceEqual([2, 3])),
            OnNext<IList<int>>(250, l => l.SequenceEqual([5])),
            OnCompleted<IList<int>>(250)
        );

        xs.Subscriptions.AssertEqual(
            Subscribe(200, 250)
        );
    }

    [TestMethod]
    public void BufferWithCount_Basic()
    {
        var scheduler = Scheduler;

        var xs = scheduler.CreateHotObservable(
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

        var res = scheduler.Start(() =>
            xs.Buffer(3, 2).Select(x => string.Join(",", x.Select(xx => xx.ToString()).ToArray()))
        );

        res.Messages.AssertEqual(
            OnNext(280, "2,3,4"),
            OnNext(350, "4,5,6"),
            OnNext(420, "6,7,8"),
            OnNext(600, "8,9"),
            OnCompleted<string>(600)
        );

        xs.Subscriptions.AssertEqual(
            Subscribe(200, 600)
        );
    }

    [TestMethod]
    public void BufferWithCount_Disposed()
    {
        var scheduler = Scheduler;

        var xs = scheduler.CreateHotObservable(
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

        var res = scheduler.Start(() =>
            xs.Buffer(3, 2)
                .Select(x => string.Join(",", x.Select(xx => xx.ToString()).ToArray())), 370
        );

        res.Messages.AssertEqual(
            OnNext(280, "2,3,4"),
            OnNext(350, "4,5,6")
        );

        xs.Subscriptions.AssertEqual(
            Subscribe(200, 370)
        );
    }

    [TestMethod]
    public void BufferWithCount_Error()
    {
        var scheduler = Scheduler;

        var ex = new Exception();

        var xs = scheduler.CreateHotObservable(
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

        var res = scheduler.Start(() =>
            xs.Buffer(3, 2).Select(x => string.Join(",", x.Select(xx => xx.ToString()).ToArray()))
        );

        res.Messages.AssertEqual(
            OnNext(280, "2,3,4"),
            OnNext(350, "4,5,6"),
            OnNext(420, "6,7,8"),
            OnError<string>(600, ex)
        );

        xs.Subscriptions.AssertEqual(
            Subscribe(200, 600)
        );
    }

    [TestMethod]
    public async Task BufferWithCount_Default()
    {
        var res = await ToListAsync(Seq.Range(1, 10).Buffer(3));

        Assert.HasCount(4, res);
        Assert.IsTrue(res[1].SequenceEqual([4, 5, 6]));

        var res2 = await ToListAsync(Seq.Range(1, 10).Buffer(3, 2));

        Assert.HasCount(5, res2);
        Assert.IsTrue(res2[1].SequenceEqual([3, 4, 5]));
    }

    [TestMethod]
    public void BufferWithTime_Basic1()
    {
        var scheduler = Scheduler;

        var xs = scheduler.CreateHotObservable(
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

        var res = scheduler.Start(() =>
            xs.Buffer(TimeSpan.FromTicks(100), TimeSpan.FromTicks(70), scheduler)
                .Select(x => string.Join(",", x.Select(xx => xx.ToString()).ToArray()))
        );

        res.Messages.AssertEqual(
            OnNext(300, "2,3,4"),
            OnNext(370, "4,5,6"),
            OnNext(440, "6,7,8"),
            OnNext(510, "8,9"),
            OnNext(580, ""),
            OnNext(600, ""),
            OnCompleted<string>(600)
        );

        xs.Subscriptions.AssertEqual(
            Subscribe(200, 600)
        );
    }

    [TestMethod]
    public void BufferWithTime_Basic2()
    {
        var scheduler = Scheduler;

        var xs = scheduler.CreateHotObservable(
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

        var res = scheduler.Start(() =>
            xs.Buffer(TimeSpan.FromTicks(70), TimeSpan.FromTicks(100), scheduler)
                .Select(x => string.Join(",", x.Select(xx => xx.ToString()).ToArray()))
        );

        res.Messages.AssertEqual(
            OnNext(270, "2,3"),
            OnNext(370, "5,6"),
            OnNext(470, "8,9"),
            OnNext(570, ""),
            OnCompleted<string>(600)
        );

        xs.Subscriptions.AssertEqual(
            Subscribe(200, 600)
        );
    }

    [TestMethod]
    public void BufferWithTime_Error()
    {
        var scheduler = Scheduler;

        var ex = new Exception();

        var xs = scheduler.CreateHotObservable(
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

        var res = scheduler.Start(() =>
            xs.Buffer(TimeSpan.FromTicks(100), TimeSpan.FromTicks(70), scheduler)
                .Select(x => string.Join(",", x.Select(xx => xx.ToString()).ToArray()))
        );

        res.Messages.AssertEqual(
            OnNext(300, "2,3,4"),
            OnNext(370, "4,5,6"),
            OnNext(440, "6,7,8"),
            OnNext(510, "8,9"),
            OnNext(580, ""),
            OnError<string>(600, ex)
        );

        xs.Subscriptions.AssertEqual(
            Subscribe(200, 600)
        );
    }

    [TestMethod]
    public void BufferWithTime_Disposed()
    {
        var scheduler = Scheduler;

        var xs = scheduler.CreateHotObservable(
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

        var res = scheduler.Start(() =>
            xs.Buffer(TimeSpan.FromTicks(100), TimeSpan.FromTicks(70), scheduler)
                .Select(x => string.Join(",", x.Select(xx => xx.ToString()).ToArray())),
            370
        );

        res.Messages.AssertEqual(
            OnNext(300, "2,3,4")
        );

        xs.Subscriptions.AssertEqual(
            Subscribe(200, 370)
        );
    }

    [TestMethod]
    public void BufferWithTime_Basic_Same()
    {
        var scheduler = Scheduler;

        var xs = scheduler.CreateHotObservable(
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

        var res = scheduler.Start(() =>
            xs.Buffer(TimeSpan.FromTicks(100), scheduler)
                .Select(x => string.Join(",", x.Select(xx => xx.ToString()).ToArray()))
        );

        res.Messages.AssertEqual(
            OnNext(300, "2,3,4"),
            OnNext(400, "5,6,7"),
            OnNext(500, "8,9"),
            OnNext(600, ""),
            OnCompleted<string>(600)
        );

        xs.Subscriptions.AssertEqual(
            Subscribe(200, 600)
        );
    }

    [TestMethod]
    public async Task BufferWithTime_Default()
    {
        var res = await ToListAsync(
            Seq.Range(0, 10).Buffer(TimeSpan.FromDays(1), TimeSpan.FromDays(1)));

        Assert.HasCount(1, res);
        Assert.IsTrue(res[0].SequenceEqual(Enumerable.Range(0, 10)));

        var res2 = await ToListAsync(Seq.Range(0, 10).Buffer(TimeSpan.FromDays(1)));

        Assert.HasCount(1, res2);
        Assert.IsTrue(res2[0].SequenceEqual(Enumerable.Range(0, 10)));
    }

    [TestMethod]
    public void BufferWithTimeOrCount_Basic()
    {
        var scheduler = Scheduler;

        var xs = scheduler.CreateHotObservable(
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

        var res = scheduler.Start(() =>
            xs.Buffer(TimeSpan.FromTicks(70), 3, scheduler)
                .Select(x => string.Join(",", x.Select(xx => xx.ToString()).ToArray()))
        );

        res.Messages.AssertEqual(
            OnNext(240, "1,2,3"),
            OnNext(310, "4"),
            OnNext(370, "5,6,7"),
            OnNext(440, "8"),
            OnNext(510, "9"),
            OnNext(580, ""),
            OnNext(600, ""),
            OnCompleted<string>(600)
        );

        xs.Subscriptions.AssertEqual(
            Subscribe(200, 600)
        );
    }

    [TestMethod]
    public void BufferWithTimeOrCount_Error()
    {
        var scheduler = Scheduler;

        var ex = new Exception();

        var xs = scheduler.CreateHotObservable(
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

        var res = scheduler.Start(() =>
            xs.Buffer(TimeSpan.FromTicks(70), 3, scheduler)
                .Select(x => string.Join(",", x.Select(xx => xx.ToString()).ToArray()))
        );

        res.Messages.AssertEqual(
            OnNext(240, "1,2,3"),
            OnNext(310, "4"),
            OnNext(370, "5,6,7"),
            OnNext(440, "8"),
            OnNext(510, "9"),
            OnNext(580, ""),
            OnError<string>(600, ex)
        );

        xs.Subscriptions.AssertEqual(
            Subscribe(200, 600)
        );
    }

    [TestMethod]
    public void BufferWithTimeOrCount_Disposed()
    {
        var scheduler = Scheduler;

        var xs = scheduler.CreateHotObservable(
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

        var res = scheduler.Start(() =>
            xs.Buffer(TimeSpan.FromTicks(70), 3, scheduler)
                .Select(x => string.Join(",", x.Select(xx => xx.ToString()).ToArray())),
            370
        );

        res.Messages.AssertEqual(
            OnNext(240, "1,2,3"),
            OnNext(310, "4"),
            OnNext(370, "5,6,7")
        );

        xs.Subscriptions.AssertEqual(
            Subscribe(200, 370)
        );
    }

    [TestMethod]
    public async Task BufferWithTimeOrCount_Default()
    {
        var res = await ToListAsync(
            Seq.Range(1, 10, DefaultScheduler).Buffer(TimeSpan.FromDays(1), 3));

        Assert.HasCount(4, res);
        Assert.IsTrue(res[1].SequenceEqual([4, 5, 6]));
    }
}

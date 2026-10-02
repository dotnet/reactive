// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information. 

using System.Reactive;

using Microsoft.Reactive.Testing;

namespace Tests.System.Reactive.Shared.Scenarios;

/// <summary>Shared <c>Merge</c> scenarios, from Rx.NET's <c>MergeTest.cs</c>.</summary>
/// <remarks>
/// <para>
/// Every behavioural test from that file: 48 of its 50. Deliberately not here: the two
/// <c>*_ArgumentChecking</c> tests (the code-generated stratum). Where the original carries
/// pre-v2 expectations under <c>NO_PERF</c>, the v2 ones are used.
/// </para>
/// <para>
/// The overloads that subscribe to each source in its own scheduled step
/// (<c>Merge(scheduler, xs, ys)</c> and the enumerable forms) write their subscription ticks as
/// <c>ScheduledAt(200, n)</c> where the original has <c>201</c>, <c>202</c>, since only Rx.NET's
/// <c>TestScheduler</c> advances a tick per step.
/// </para>
/// <para>
/// The real-time tests (the <c>*_DefaultScheduler</c>, <c>MergeConcat_Enumerable*</c>,
/// <c>MergeConcat_Default</c> and <c>Merge_Task*</c> families) are <c>async Task</c> methods
/// that await <see cref="SharedReactiveTest.ToListAsync{T}"/> where the original blocks on a
/// <c>ManualResetEvent</c> or an enumerating conversion, and drive a
/// <see cref="SharedReactiveTest.CreateSubject{T}"/> where it has a <c>Subject&lt;T&gt;</c>. A
/// faulted result surfaces as the awaited task's exception, so the two <c>Faulted</c> and two
/// <c>Canceled</c> tests check the exception and not also that no value was produced.
/// <c>Merge_LotsMore</c> asserts its computed expectations through <c>AssertEqual</c> rather
/// than walking the recorded messages by index.
/// </para>
/// </remarks>
public abstract class MergeTests : SharedReactiveTest
{
    [TestMethod]
    public async Task Merge_DefaultScheduler()
    {
        var xs = Seq.Merge(Seq.Return(42), Seq.Return(43), Seq.Return(44));
        var res = await ToListAsync(xs);
        Assert.IsTrue(new[] { 42, 43, 44 }.SequenceEqual(res));
    }

    [TestMethod]
    public void Merge_Never2()
    {
        var n1 = Scheduler.CreateHotObservable(
            OnNext(150, 1)
        );

        var n2 = Scheduler.CreateHotObservable(
            OnNext(150, 1)
        );

        var res = Scheduler.Start(() =>
            Seq.Merge(Scheduler, n1, n2)
        );

        res.Messages.AssertEqual(
        );

        n1.Subscriptions.AssertEqual(
            Subscribe(ScheduledAt(200, 1), 1000)
        );

        n2.Subscriptions.AssertEqual(
            Subscribe(ScheduledAt(200, 2), 1000)
        );
    }

    [TestMethod]
    public void Merge_Never3()
    {
        var n1 = Scheduler.CreateHotObservable(
            OnNext(150, 1)
        );

        var n2 = Scheduler.CreateHotObservable(
            OnNext(150, 1)
        );

        var n3 = Scheduler.CreateHotObservable(
            OnNext(150, 1)
        );

        var res = Scheduler.Start(() =>
            Seq.Merge(Scheduler, n1, n2, n3)
        );

        res.Messages.AssertEqual(
        );

        n1.Subscriptions.AssertEqual(
            Subscribe(ScheduledAt(200, 1), 1000)
        );

        n2.Subscriptions.AssertEqual(
            Subscribe(ScheduledAt(200, 2), 1000)
        );

        n3.Subscriptions.AssertEqual(
            Subscribe(ScheduledAt(200, 3), 1000)
        );
    }

    [TestMethod]
    public void Merge_Empty2()
    {
        var e1 = Scheduler.CreateHotObservable(
            OnNext(150, 1),
            OnCompleted<int>(210)
        );

        var e2 = Scheduler.CreateHotObservable(
            OnNext(150, 1),
            OnCompleted<int>(230)
        );

        var res = Scheduler.Start(() =>
            Seq.Merge(Scheduler, e1, e2)
        );

        res.Messages.AssertEqual(
            OnCompleted<int>(230)
        );

        e1.Subscriptions.AssertEqual(
            Subscribe(ScheduledAt(200, 1), 210)
        );

        e2.Subscriptions.AssertEqual(
            Subscribe(ScheduledAt(200, 2), 230)
        );
    }

    [TestMethod]
    public void Merge_Empty3()
    {
        var e1 = Scheduler.CreateHotObservable(
            OnNext(150, 1),
            OnCompleted<int>(210)
        );

        var e2 = Scheduler.CreateHotObservable(
            OnNext(150, 1),
            OnCompleted<int>(230)
        );

        var e3 = Scheduler.CreateHotObservable(
            OnNext(150, 1),
            OnCompleted<int>(240)
        );

        var res = Scheduler.Start(() =>
            Seq.Merge(Scheduler, e1, e2, e3)
        );

        res.Messages.AssertEqual(
            OnCompleted<int>(240)
        );

        e1.Subscriptions.AssertEqual(
            Subscribe(ScheduledAt(200, 1), 210)
        );

        e2.Subscriptions.AssertEqual(
            Subscribe(ScheduledAt(200, 2), 230)
        );

        e3.Subscriptions.AssertEqual(
            Subscribe(ScheduledAt(200, 3), 240)
        );
    }

    [TestMethod]
    public void Merge_EmptyDelayed2_RightLast()
    {
        var e1 = Scheduler.CreateHotObservable(
            OnNext(150, 1),
            OnCompleted<int>(240)
        );

        var e2 = Scheduler.CreateHotObservable(
            OnNext(150, 1),
            OnCompleted<int>(250)
        );

        var res = Scheduler.Start(() =>
            Seq.Merge(Scheduler, e1, e2)
        );

        res.Messages.AssertEqual(
            OnCompleted<int>(250)
        );

        e1.Subscriptions.AssertEqual(
            Subscribe(ScheduledAt(200, 1), 240)
        );

        e2.Subscriptions.AssertEqual(
            Subscribe(ScheduledAt(200, 2), 250)
        );
    }

    [TestMethod]
    public void Merge_EmptyDelayed2_LeftLast()
    {
        var e1 = Scheduler.CreateHotObservable(
            OnNext(150, 1),
            OnCompleted<int>(250)
        );

        var e2 = Scheduler.CreateHotObservable(
            OnNext(150, 1),
            OnCompleted<int>(240)
        );

        var res = Scheduler.Start(() =>
            Seq.Merge(Scheduler, e1, e2)
        );

        res.Messages.AssertEqual(
            OnCompleted<int>(250)
        );

        e1.Subscriptions.AssertEqual(
            Subscribe(ScheduledAt(200, 1), 250)
        );

        e2.Subscriptions.AssertEqual(
            Subscribe(ScheduledAt(200, 2), 240)
        );
    }

    [TestMethod]
    public void Merge_EmptyDelayed3_MiddleLast()
    {
        var e1 = Scheduler.CreateHotObservable(
            OnNext(150, 1),
            OnCompleted<int>(245)
        );

        var e2 = Scheduler.CreateHotObservable(
            OnNext(150, 1),
            OnCompleted<int>(250)
        );

        var e3 = Scheduler.CreateHotObservable(
            OnNext(150, 1),
            OnCompleted<int>(240)
        );

        var res = Scheduler.Start(() =>
            Seq.Merge(Scheduler, e1, e2, e3)
        );

        res.Messages.AssertEqual(
            OnCompleted<int>(250)
        );

        e1.Subscriptions.AssertEqual(
            Subscribe(ScheduledAt(200, 1), 245)
        );

        e2.Subscriptions.AssertEqual(
            Subscribe(ScheduledAt(200, 2), 250)
        );

        e3.Subscriptions.AssertEqual(
            Subscribe(ScheduledAt(200, 3), 240)
        );
    }

    [TestMethod]
    public void Merge_EmptyNever()
    {
        var e1 = Scheduler.CreateHotObservable(
            OnNext(150, 1),
            OnCompleted<int>(245)
        );

        var n1 = Scheduler.CreateHotObservable(
            OnNext(150, 1)
        );

        var res = Scheduler.Start(() =>
            Seq.Merge(Scheduler, e1, n1)
        );

        res.Messages.AssertEqual(
        );

        e1.Subscriptions.AssertEqual(
            Subscribe(ScheduledAt(200, 1), 245)
        );

        n1.Subscriptions.AssertEqual(
            Subscribe(ScheduledAt(200, 2), 1000)
        );
    }

    [TestMethod]
    public void Merge_NeverEmpty()
    {
        var n1 = Scheduler.CreateHotObservable(
            OnNext(150, 1)
        );

        var e1 = Scheduler.CreateHotObservable(
            OnNext(150, 1),
            OnCompleted<int>(245)
        );

        var res = Scheduler.Start(() =>
            Seq.Merge(Scheduler, n1, e1)
        );

        res.Messages.AssertEqual(
        );

        n1.Subscriptions.AssertEqual(
            Subscribe(ScheduledAt(200, 1), 1000)
        );

        e1.Subscriptions.AssertEqual(
            Subscribe(ScheduledAt(200, 2), 245)
        );
    }

    [TestMethod]
    public void Merge_ReturnNever()
    {
        var r1 = Scheduler.CreateHotObservable(
            OnNext(150, 1),
            OnNext(210, 2),
            OnCompleted<int>(245)
        );

        var n1 = Scheduler.CreateHotObservable(
            OnNext(150, 1)
        );

        var res = Scheduler.Start(() =>
            Seq.Merge(Scheduler, r1, n1)
        );

        res.Messages.AssertEqual(
            OnNext(210, 2)
        );

        r1.Subscriptions.AssertEqual(
            Subscribe(ScheduledAt(200, 1), 245)
        );

        n1.Subscriptions.AssertEqual(
            Subscribe(ScheduledAt(200, 2), 1000)
        );
    }

    [TestMethod]
    public void Merge_NeverReturn()
    {
        var n1 = Scheduler.CreateHotObservable(
            OnNext(150, 1)
        );

        var r1 = Scheduler.CreateHotObservable(
            OnNext(150, 1),
            OnNext(210, 2),
            OnCompleted<int>(245)
        );

        var res = Scheduler.Start(() =>
            Seq.Merge(Scheduler, n1, r1)
        );

        res.Messages.AssertEqual(
            OnNext(210, 2)
        );

        n1.Subscriptions.AssertEqual(
            Subscribe(ScheduledAt(200, 1), 1000)
        );

        r1.Subscriptions.AssertEqual(
            Subscribe(ScheduledAt(200, 2), 245)
        );
    }

    [TestMethod]
    public void Merge_ErrorNever()
    {
        var ex = new Exception();

        var e1 = Scheduler.CreateHotObservable(
            OnNext(150, 1),
            OnNext(210, 2),
            OnError<int>(245, ex)
        );

        var n1 = Scheduler.CreateHotObservable(
            OnNext(150, 1)
        );

        var res = Scheduler.Start(() =>
            Seq.Merge(Scheduler, e1, n1)
        );

        res.Messages.AssertEqual(
            OnNext(210, 2),
            OnError<int>(245, ex)
        );

        e1.Subscriptions.AssertEqual(
            Subscribe(ScheduledAt(200, 1), 245)
        );

        n1.Subscriptions.AssertEqual(
            Subscribe(ScheduledAt(200, 2), 245)
        );
    }

    [TestMethod]
    public void Merge_NeverError()
    {
        var ex = new Exception();

        var n1 = Scheduler.CreateHotObservable(
            OnNext(150, 1)
        );

        var e1 = Scheduler.CreateHotObservable(
            OnNext(150, 1),
            OnNext(210, 2),
            OnError<int>(245, ex)
        );

        var res = Scheduler.Start(() =>
            Seq.Merge(Scheduler, n1, e1)
        );

        res.Messages.AssertEqual(
            OnNext(210, 2),
            OnError<int>(245, ex)
        );

        n1.Subscriptions.AssertEqual(
            Subscribe(ScheduledAt(200, 1), 245)
        );

        e1.Subscriptions.AssertEqual(
            Subscribe(ScheduledAt(200, 2), 245)
        );
    }

    [TestMethod]
    public void Merge_EmptyReturn()
    {
        var e1 = Scheduler.CreateHotObservable(
            OnNext(150, 1),
            OnCompleted<int>(245)
        );

        var r1 = Scheduler.CreateHotObservable(
            OnNext(150, 1),
            OnNext(210, 2),
            OnCompleted<int>(250)
        );

        var res = Scheduler.Start(() =>
            Seq.Merge(Scheduler, e1, r1)
        );

        res.Messages.AssertEqual(
            OnNext(210, 2),
            OnCompleted<int>(250)
        );

        e1.Subscriptions.AssertEqual(
            Subscribe(ScheduledAt(200, 1), 245)
        );

        r1.Subscriptions.AssertEqual(
            Subscribe(ScheduledAt(200, 2), 250)
        );
    }

    [TestMethod]
    public void Merge_ReturnEmpty()
    {
        var r1 = Scheduler.CreateHotObservable(
            OnNext(150, 1),
            OnNext(210, 2),
            OnCompleted<int>(250)
        );

        var e1 = Scheduler.CreateHotObservable(
            OnNext(150, 1),
            OnCompleted<int>(245)
        );

        var res = Scheduler.Start(() =>
            Seq.Merge(Scheduler, r1, e1)
        );

        res.Messages.AssertEqual(
            OnNext(210, 2),
            OnCompleted<int>(250)
        );

        r1.Subscriptions.AssertEqual(
            Subscribe(ScheduledAt(200, 1), 250)
        );

        e1.Subscriptions.AssertEqual(
            Subscribe(ScheduledAt(200, 2), 245)
        );
    }

    [TestMethod]
    public void Merge_Lots2()
    {
        var o1 = Scheduler.CreateHotObservable(
            OnNext(150, 1),
            OnNext(210, 2),
            OnNext(220, 4),
            OnNext(230, 6),
            OnNext(240, 8),
            OnCompleted<int>(245)
        );

        var o2 = Scheduler.CreateHotObservable(
            OnNext(150, 1),
            OnNext(215, 3),
            OnNext(225, 5),
            OnNext(235, 7),
            OnNext(245, 9),
            OnCompleted<int>(250)
        );

        var res = Scheduler.Start(() =>
            Seq.Merge(Scheduler, o1, o2)
        );

        res.Messages.AssertEqual(
            OnNext(210, 2),
            OnNext(215, 3),
            OnNext(220, 4),
            OnNext(225, 5),
            OnNext(230, 6),
            OnNext(235, 7),
            OnNext(240, 8),
            OnNext(245, 9),
            OnCompleted<int>(250)
        );

        o1.Subscriptions.AssertEqual(
            Subscribe(ScheduledAt(200, 1), 245)
        );

        o2.Subscriptions.AssertEqual(
            Subscribe(ScheduledAt(200, 2), 250)
        );
    }

    [TestMethod]
    public void Merge_Lots3()
    {
        var o1 = Scheduler.CreateHotObservable(
            OnNext(150, 1),
            OnNext(210, 2),
            OnNext(225, 5),
            OnNext(240, 8),
            OnCompleted<int>(245)
        );

        var o2 = Scheduler.CreateHotObservable(
            OnNext(150, 1),
            OnNext(215, 3),
            OnNext(230, 6),
            OnNext(245, 9),
            OnCompleted<int>(250)
        );

        var o3 = Scheduler.CreateHotObservable(
            OnNext(150, 1),
            OnNext(220, 4),
            OnNext(235, 7),
            OnCompleted<int>(240)
        );

        var res = Scheduler.Start(() =>
            new[] { o1, o2, o3 }.Merge(Scheduler)
        );

        res.Messages.AssertEqual(
            OnNext(210, 2),
            OnNext(215, 3),
            OnNext(220, 4),
            OnNext(225, 5),
            OnNext(230, 6),
            OnNext(235, 7),
            OnNext(240, 8),
            OnNext(245, 9),
            OnCompleted<int>(250)
        );

        o1.Subscriptions.AssertEqual(
            Subscribe(ScheduledAt(200, 1), 245)
        );

        o2.Subscriptions.AssertEqual(
            Subscribe(ScheduledAt(200, 2), 250)
        );

        o3.Subscriptions.AssertEqual(
            Subscribe(ScheduledAt(200, 3), 240)
        );
    }

    [TestMethod]
    public void Merge_LotsMore()
    {
        var inputs = new List<List<Recorded<Notification<int>>>>();

        const int N = 10;
        for (var i = 0; i < N; i++)
        {
            var lst = new List<Recorded<Notification<int>>> { OnNext(150, 1) };
            inputs.Add(lst);

            var start = (ushort)(301 + i);
            for (var j = 0; j < i; j++)
            {
                var onNext = OnNext(start += (ushort)(j * 5), j + i + 2);
                lst.Add(onNext);
            }

            lst.Add(OnCompleted<int>((ushort)(start + N - i)));
        }

        var inputsFlat = inputs.Aggregate((l, r) => l.Concat(r).ToList()).ToArray();

        var resOnNext = (from n in inputsFlat
                         where n.Time >= 200
                         where n.Value.Kind == NotificationKind.OnNext
                         orderby n.Time
                         select n).ToList();

        var lastCompleted = (from n in inputsFlat
                             where n.Time >= 200
                             where n.Value.Kind == NotificationKind.OnCompleted
                             orderby n.Time descending
                             select n).First();

        // Last ToArray: got to create the hot observables *now*
        var xss = inputs.Select(lst => (Seq<int>)Scheduler.CreateHotObservable(lst.ToArray())).ToArray();

        var res = Scheduler.Start(() =>
            xss.Merge(Scheduler)
        );

        // The original walks res.Messages by index to check the same thing.
        res.Messages.AssertEqual(resOnNext.Append(OnCompleted<int>(lastCompleted.Time)));
    }

    [TestMethod]
    public void Merge_ErrorLeft()
    {
        var ex = new Exception();

        var o1 = Scheduler.CreateHotObservable(
            OnNext(150, 1),
            OnNext(210, 2),
            OnError<int>(245, ex)
        );

        var o2 = Scheduler.CreateHotObservable(
            OnNext(150, 1),
            OnNext(215, 3),
            OnCompleted<int>(250)
        );

        var res = Scheduler.Start(() =>
            Seq.Merge(o1, o2, Scheduler)
        );

        res.Messages.AssertEqual(
            OnNext(210, 2),
            OnNext(215, 3),
            OnError<int>(245, ex)
        );

        o1.Subscriptions.AssertEqual(
            Subscribe(ScheduledAt(200, 1), 245)
        );

        o2.Subscriptions.AssertEqual(
            Subscribe(ScheduledAt(200, 2), 245)
        );
    }

    [TestMethod]
    public void Merge_ErrorCausesDisposal()
    {
        var ex = new Exception();

        var e1 = Scheduler.CreateHotObservable(
            OnNext(150, 1),
            OnError<int>(210, ex) //!
        );

        var o1 = Scheduler.CreateHotObservable(
            OnNext(150, 1),
            OnNext(220, 1), // should not come
            OnCompleted<int>(230)
        );

        var res = Scheduler.Start(() =>
            Seq.Merge(e1, o1, Scheduler)
        );

        res.Messages.AssertEqual(
            OnError<int>(210, ex) //!
        );

        e1.Subscriptions.AssertEqual(
            Subscribe(ScheduledAt(200, 1), 210)
        );

        o1.Subscriptions.AssertEqual(
            Subscribe(ScheduledAt(200, 2), 210)
        );
    }

    [TestMethod]
    public void Merge_ObservableOfObservable_Data()
    {
        var ys1 = Scheduler.CreateColdObservable(
            OnNext(10, 101),
            OnNext(20, 102),
            OnNext(110, 103),
            OnNext(120, 104),
            OnNext(210, 105),
            OnNext(220, 106),
            OnCompleted<int>(230)
        );

        var ys2 = Scheduler.CreateColdObservable(
            OnNext(10, 201),
            OnNext(20, 202),
            OnNext(30, 203),
            OnNext(40, 204),
            OnCompleted<int>(50)
        );

        var ys3 = Scheduler.CreateColdObservable(
            OnNext(10, 301),
            OnNext(20, 302),
            OnNext(30, 303),
            OnNext(40, 304),
            OnNext(120, 305),
            OnCompleted<int>(150)
        );

        var xs = Scheduler.CreateHotObservable(
            OnNext<Seq<int>>(300, ys1),
            OnNext<Seq<int>>(400, ys2),
            OnNext<Seq<int>>(500, ys3),
            OnCompleted<Seq<int>>(600)
        );

        var res = Scheduler.Start(() =>
            xs.Merge()
        );

        res.Messages.AssertEqual(
            OnNext(310, 101),
            OnNext(320, 102),
            OnNext(410, 103),
            OnNext(410, 201),
            OnNext(420, 104),
            OnNext(420, 202),
            OnNext(430, 203),
            OnNext(440, 204),
            OnNext(510, 105),
            OnNext(510, 301),
            OnNext(520, 106),
            OnNext(520, 302),
            OnNext(530, 303),
            OnNext(540, 304),
            OnNext(620, 305),
            OnCompleted<int>(650)
        );

        xs.Subscriptions.AssertEqual(
            Subscribe(200, 600)
        );

        ys1.Subscriptions.AssertEqual(
            Subscribe(300, 530)
        );

        ys2.Subscriptions.AssertEqual(
            Subscribe(400, 450)
        );

        ys3.Subscriptions.AssertEqual(
            Subscribe(500, 650)
        );
    }

    [TestMethod]
    public void Merge_ObservableOfObservable_Data_NonOverlapped()
    {
        var ys1 = Scheduler.CreateColdObservable(
            OnNext(10, 101),
            OnNext(20, 102),
            OnCompleted<int>(230)
        );

        var ys2 = Scheduler.CreateColdObservable(
            OnNext(10, 201),
            OnNext(20, 202),
            OnNext(30, 203),
            OnNext(40, 204),
            OnCompleted<int>(50)
        );

        var ys3 = Scheduler.CreateColdObservable(
            OnNext(10, 301),
            OnNext(20, 302),
            OnNext(30, 303),
            OnNext(40, 304),
            OnCompleted<int>(50)
        );

        var xs = Scheduler.CreateHotObservable(
            OnNext<Seq<int>>(300, ys1),
            OnNext<Seq<int>>(400, ys2),
            OnNext<Seq<int>>(500, ys3),
            OnCompleted<Seq<int>>(600)
        );

        var res = Scheduler.Start(() =>
            xs.Merge()
        );

        res.Messages.AssertEqual(
            OnNext(310, 101),
            OnNext(320, 102),
            OnNext(410, 201),
            OnNext(420, 202),
            OnNext(430, 203),
            OnNext(440, 204),
            OnNext(510, 301),
            OnNext(520, 302),
            OnNext(530, 303),
            OnNext(540, 304),
            OnCompleted<int>(600)
        );

        xs.Subscriptions.AssertEqual(
            Subscribe(200, 600)
        );

        ys1.Subscriptions.AssertEqual(
            Subscribe(300, 530)
        );

        ys2.Subscriptions.AssertEqual(
            Subscribe(400, 450)
        );

        ys3.Subscriptions.AssertEqual(
            Subscribe(500, 550)
        );
    }

    [TestMethod]
    public void Merge_ObservableOfObservable_InnerThrows()
    {
        var ex = new Exception();

        var ys1 = Scheduler.CreateColdObservable(
            OnNext(10, 101),
            OnNext(20, 102),
            OnNext(110, 103),
            OnNext(120, 104),
            OnNext(210, 105),
            OnNext(220, 106),
            OnCompleted<int>(230)
        );

        var ys2 = Scheduler.CreateColdObservable(
            OnNext(10, 201),
            OnNext(20, 202),
            OnNext(30, 203),
            OnNext(40, 204),
            OnError<int>(50, ex)
        );

        var ys3 = Scheduler.CreateColdObservable(
            OnNext(10, 301),
            OnNext(20, 302),
            OnNext(30, 303),
            OnNext(40, 304),
            OnCompleted<int>(150)
        );

        var xs = Scheduler.CreateHotObservable(
            OnNext<Seq<int>>(300, ys1),
            OnNext<Seq<int>>(400, ys2),
            OnNext<Seq<int>>(500, ys3),
            OnCompleted<Seq<int>>(600)
        );

        var res = Scheduler.Start(() =>
            xs.Merge()
        );

        res.Messages.AssertEqual(
            OnNext(310, 101),
            OnNext(320, 102),
            OnNext(410, 103),
            OnNext(410, 201),
            OnNext(420, 104),
            OnNext(420, 202),
            OnNext(430, 203),
            OnNext(440, 204),
            OnError<int>(450, ex)
        );

        xs.Subscriptions.AssertEqual(
            Subscribe(200, 450)
        );

        ys1.Subscriptions.AssertEqual(
            Subscribe(300, 450)
        );

        ys2.Subscriptions.AssertEqual(
            Subscribe(400, 450)
        );

        ys3.Subscriptions.AssertEqual(
        );
    }

    [TestMethod]
    public void Merge_ObservableOfObservable_OuterThrows()
    {
        var ex = new Exception();

        var ys1 = Scheduler.CreateColdObservable(
            OnNext(10, 101),
            OnNext(20, 102),
            OnNext(110, 103),
            OnNext(120, 104),
            OnNext(210, 105),
            OnNext(220, 106),
            OnCompleted<int>(230)
        );

        var ys2 = Scheduler.CreateColdObservable(
            OnNext(10, 201),
            OnNext(20, 202),
            OnNext(30, 203),
            OnNext(40, 204),
            OnCompleted<int>(50)
        );

        var xs = Scheduler.CreateHotObservable(
            OnNext<Seq<int>>(300, ys1),
            OnNext<Seq<int>>(400, ys2),
            OnError<Seq<int>>(500, ex)
        );

        var res = Scheduler.Start(() =>
            xs.Merge()
        );

        res.Messages.AssertEqual(
            OnNext(310, 101),
            OnNext(320, 102),
            OnNext(410, 103),
            OnNext(410, 201),
            OnNext(420, 104),
            OnNext(420, 202),
            OnNext(430, 203),
            OnNext(440, 204),
            OnError<int>(500, ex)
        );

        xs.Subscriptions.AssertEqual(
            Subscribe(200, 500)
        );

        ys1.Subscriptions.AssertEqual(
            Subscribe(300, 500)
        );

        ys2.Subscriptions.AssertEqual(
            Subscribe(400, 450)
        );
    }

    [TestMethod]
    public async Task Merge_Binary_DefaultScheduler()
    {
        var res = await ToListAsync(Seq.Return(1).Merge(Seq.Return(2)));
        Assert.IsTrue(res.OrderBy(x => x).SequenceEqual([1, 2]));
    }

    [TestMethod]
    public async Task Merge_Params_DefaultScheduler()
    {
        var res = await ToListAsync(Seq.Merge(Seq.Return(1), Seq.Return(2)));
        Assert.IsTrue(res.OrderBy(x => x).SequenceEqual([1, 2]));
    }

    [TestMethod]
    public async Task Merge_IEnumerableOfIObservable_DefaultScheduler()
    {
        var res = await ToListAsync(Seq.Merge((IEnumerable<Seq<int>>)[Seq.Return(1), Seq.Return(2)]));
        Assert.IsTrue(res.OrderBy(x => x).SequenceEqual([1, 2]));
    }

    [TestMethod]
    public async Task MergeConcat_Enumerable_Scheduler()
    {
        var res = await ToListAsync(Enumerable.Range(1, 3).Select(x => Seq.Range(x * 10, 3)).Merge(1));
        Assert.IsTrue(new[] { 10, 11, 12, 20, 21, 22, 30, 31, 32 }.SequenceEqual(res));
    }

    [TestMethod]
    public async Task MergeConcat_Enumerable()
    {
        var res = await ToListAsync(Enumerable.Range(1, 3).Select(x => Seq.Range(x * 10, 3)).Merge(1, DefaultScheduler));
        Assert.IsTrue(new[] { 10, 11, 12, 20, 21, 22, 30, 31, 32 }.SequenceEqual(res));
    }

    [TestMethod]
    public async Task MergeConcat_Default()
    {
        var res = await ToListAsync(Seq.Range(1, 3).Select(x => Seq.Range(x * 10, 3)).Concat());
        Assert.IsTrue(new[] { 10, 11, 12, 20, 21, 22, 30, 31, 32 }.SequenceEqual(res));
    }

    [TestMethod]
    public void MergeConcat_Basic()
    {
        var ys1 = Scheduler.CreateColdObservable(
            OnNext(50, 1),
            OnNext(100, 2),
            OnNext(120, 3),
            OnCompleted<int>(140)
        );

        var ys2 = Scheduler.CreateColdObservable(
            OnNext(20, 4),
            OnNext(70, 5),
            OnCompleted<int>(200)
        );

        var ys3 = Scheduler.CreateColdObservable(
            OnNext(10, 6),
            OnNext(90, 7),
            OnNext(110, 8),
            OnCompleted<int>(130)
        );

        var ys4 = Scheduler.CreateColdObservable(
            OnNext(210, 9),
            OnNext(240, 10),
            OnCompleted<int>(300)
        );

        var xs = Scheduler.CreateHotObservable(
            OnNext<Seq<int>>(210, ys1),
            OnNext<Seq<int>>(260, ys2),
            OnNext<Seq<int>>(270, ys3),
            OnNext<Seq<int>>(320, ys4),
            OnCompleted<Seq<int>>(400)
        );

        var res = Scheduler.Start(() =>
            xs.Merge(2)
        );

        res.Messages.AssertEqual(
            OnNext(260, 1),
            OnNext(280, 4),
            OnNext(310, 2),
            OnNext(330, 3),
            OnNext(330, 5),
            OnNext(360, 6),
            OnNext(440, 7),
            OnNext(460, 8),
            OnNext(670, 9),
            OnNext(700, 10),
            OnCompleted<int>(760)
        );

        xs.Subscriptions.AssertEqual(
            Subscribe(200, 400)
        );

        ys1.Subscriptions.AssertEqual(
            Subscribe(210, 350)
        );

        ys2.Subscriptions.AssertEqual(
            Subscribe(260, 460)
        );

        ys3.Subscriptions.AssertEqual(
            Subscribe(350, 480)
        );

        ys4.Subscriptions.AssertEqual(
            Subscribe(460, 760)
        );
    }

    [TestMethod]
    public void MergeConcat_Basic_Long()
    {
        var ys1 = Scheduler.CreateColdObservable(
            OnNext(50, 1),
            OnNext(100, 2),
            OnNext(120, 3),
            OnCompleted<int>(140)
        );

        var ys2 = Scheduler.CreateColdObservable(
            OnNext(20, 4),
            OnNext(70, 5),
            OnCompleted<int>(300)
        );

        var ys3 = Scheduler.CreateColdObservable(
            OnNext(10, 6),
            OnNext(90, 7),
            OnNext(110, 8),
            OnCompleted<int>(130)
        );

        var ys4 = Scheduler.CreateColdObservable(
            OnNext(210, 9),
            OnNext(240, 10),
            OnCompleted<int>(300)
        );

        var xs = Scheduler.CreateHotObservable(
            OnNext<Seq<int>>(210, ys1),
            OnNext<Seq<int>>(260, ys2),
            OnNext<Seq<int>>(270, ys3),
            OnNext<Seq<int>>(320, ys4),
            OnCompleted<Seq<int>>(400)
        );

        var res = Scheduler.Start(() =>
            xs.Merge(2)
        );

        res.Messages.AssertEqual(
            OnNext(260, 1),
            OnNext(280, 4),
            OnNext(310, 2),
            OnNext(330, 3),
            OnNext(330, 5),
            OnNext(360, 6),
            OnNext(440, 7),
            OnNext(460, 8),
            OnNext(690, 9),
            OnNext(720, 10),
            OnCompleted<int>(780)
        );

        xs.Subscriptions.AssertEqual(
            Subscribe(200, 400)
        );

        ys1.Subscriptions.AssertEqual(
            Subscribe(210, 350)
        );

        ys2.Subscriptions.AssertEqual(
            Subscribe(260, 560)
        );

        ys3.Subscriptions.AssertEqual(
            Subscribe(350, 480)
        );

        ys4.Subscriptions.AssertEqual(
            Subscribe(480, 780)
        );
    }

    [TestMethod]
    public void MergeConcat_Basic_Wide()
    {
        var ys1 = Scheduler.CreateColdObservable(
            OnNext(50, 1),
            OnNext(100, 2),
            OnNext(120, 3),
            OnCompleted<int>(140)
        );

        var ys2 = Scheduler.CreateColdObservable(
            OnNext(20, 4),
            OnNext(70, 5),
            OnCompleted<int>(300)
        );

        var ys3 = Scheduler.CreateColdObservable(
            OnNext(10, 6),
            OnNext(90, 7),
            OnNext(110, 8),
            OnCompleted<int>(130)
        );

        var ys4 = Scheduler.CreateColdObservable(
            OnNext(210, 9),
            OnNext(240, 10),
            OnCompleted<int>(300)
        );

        var xs = Scheduler.CreateHotObservable(
            OnNext<Seq<int>>(210, ys1),
            OnNext<Seq<int>>(260, ys2),
            OnNext<Seq<int>>(270, ys3),
            OnNext<Seq<int>>(420, ys4),
            OnCompleted<Seq<int>>(450)
        );

        var res = Scheduler.Start(() =>
            xs.Merge(3)
        );

        res.Messages.AssertEqual(
            OnNext(260, 1),
            OnNext(280, 4),
            OnNext(280, 6),
            OnNext(310, 2),
            OnNext(330, 3),
            OnNext(330, 5),
            OnNext(360, 7),
            OnNext(380, 8),
            OnNext(630, 9),
            OnNext(660, 10),
            OnCompleted<int>(720)
        );

        xs.Subscriptions.AssertEqual(
            Subscribe(200, 450)
        );

        ys1.Subscriptions.AssertEqual(
            Subscribe(210, 350)
        );

        ys2.Subscriptions.AssertEqual(
            Subscribe(260, 560)
        );

        ys3.Subscriptions.AssertEqual(
            Subscribe(270, 400)
        );

        ys4.Subscriptions.AssertEqual(
            Subscribe(420, 720)
        );
    }

    [TestMethod]
    public void MergeConcat_Basic_Late()
    {
        var ys1 = Scheduler.CreateColdObservable(
            OnNext(50, 1),
            OnNext(100, 2),
            OnNext(120, 3),
            OnCompleted<int>(140)
        );

        var ys2 = Scheduler.CreateColdObservable(
            OnNext(20, 4),
            OnNext(70, 5),
            OnCompleted<int>(300)
        );

        var ys3 = Scheduler.CreateColdObservable(
            OnNext(10, 6),
            OnNext(90, 7),
            OnNext(110, 8),
            OnCompleted<int>(130)
        );

        var ys4 = Scheduler.CreateColdObservable(
            OnNext(210, 9),
            OnNext(240, 10),
            OnCompleted<int>(300)
        );

        var xs = Scheduler.CreateHotObservable(
            OnNext<Seq<int>>(210, ys1),
            OnNext<Seq<int>>(260, ys2),
            OnNext<Seq<int>>(270, ys3),
            OnNext<Seq<int>>(420, ys4),
            OnCompleted<Seq<int>>(750)
        );

        var res = Scheduler.Start(() =>
            xs.Merge(3)
        );

        res.Messages.AssertEqual(
            OnNext(260, 1),
            OnNext(280, 4),
            OnNext(280, 6),
            OnNext(310, 2),
            OnNext(330, 3),
            OnNext(330, 5),
            OnNext(360, 7),
            OnNext(380, 8),
            OnNext(630, 9),
            OnNext(660, 10),
            OnCompleted<int>(750)
        );

        xs.Subscriptions.AssertEqual(
            Subscribe(200, 750)
        );

        ys1.Subscriptions.AssertEqual(
            Subscribe(210, 350)
        );

        ys2.Subscriptions.AssertEqual(
            Subscribe(260, 560)
        );

        ys3.Subscriptions.AssertEqual(
            Subscribe(270, 400)
        );

        ys4.Subscriptions.AssertEqual(
            Subscribe(420, 720)
        );
    }

    [TestMethod]
    public void MergeConcat_Disposed()
    {
        var ys1 = Scheduler.CreateColdObservable(
            OnNext(50, 1),
            OnNext(100, 2),
            OnNext(120, 3),
            OnCompleted<int>(140)
        );

        var ys2 = Scheduler.CreateColdObservable(
            OnNext(20, 4),
            OnNext(70, 5),
            OnCompleted<int>(200)
        );

        var ys3 = Scheduler.CreateColdObservable(
            OnNext(10, 6),
            OnNext(90, 7),
            OnNext(110, 8),
            OnCompleted<int>(130)
        );

        var ys4 = Scheduler.CreateColdObservable(
            OnNext(210, 9),
            OnNext(240, 10),
            OnCompleted<int>(300)
        );

        var xs = Scheduler.CreateHotObservable(
            OnNext<Seq<int>>(210, ys1),
            OnNext<Seq<int>>(260, ys2),
            OnNext<Seq<int>>(270, ys3),
            OnNext<Seq<int>>(320, ys4),
            OnCompleted<Seq<int>>(400)
        );

        var res = Scheduler.Start(() =>
            xs.Merge(2),
            450
        );

        res.Messages.AssertEqual(
            OnNext(260, 1),
            OnNext(280, 4),
            OnNext(310, 2),
            OnNext(330, 3),
            OnNext(330, 5),
            OnNext(360, 6),
            OnNext(440, 7)
        );

        xs.Subscriptions.AssertEqual(
            Subscribe(200, 400)
        );

        ys1.Subscriptions.AssertEqual(
            Subscribe(210, 350)
        );

        ys2.Subscriptions.AssertEqual(
            Subscribe(260, 450)
        );

        ys3.Subscriptions.AssertEqual(
            Subscribe(350, 450)
        );

        ys4.Subscriptions.AssertEqual(
        );
    }

    [TestMethod]
    public void MergeConcat_OuterError()
    {
        var ex = new Exception();

        var ys1 = Scheduler.CreateColdObservable(
            OnNext(50, 1),
            OnNext(100, 2),
            OnNext(120, 3),
            OnCompleted<int>(140)
        );

        var ys2 = Scheduler.CreateColdObservable(
            OnNext(20, 4),
            OnNext(70, 5),
            OnCompleted<int>(200)
        );

        var ys3 = Scheduler.CreateColdObservable(
            OnNext(10, 6),
            OnNext(90, 7),
            OnNext(110, 8),
            OnCompleted<int>(130)
        );

        var ys4 = Scheduler.CreateColdObservable(
            OnNext(210, 9),
            OnNext(240, 10),
            OnCompleted<int>(300)
        );

        var xs = Scheduler.CreateHotObservable(
            OnNext<Seq<int>>(210, ys1),
            OnNext<Seq<int>>(260, ys2),
            OnNext<Seq<int>>(270, ys3),
            OnNext<Seq<int>>(320, ys4),
            OnError<Seq<int>>(400, ex)
        );

        var res = Scheduler.Start(() =>
            xs.Merge(2)
        );

        res.Messages.AssertEqual(
            OnNext(260, 1),
            OnNext(280, 4),
            OnNext(310, 2),
            OnNext(330, 3),
            OnNext(330, 5),
            OnNext(360, 6),
            OnError<int>(400, ex)
        );

        xs.Subscriptions.AssertEqual(
            Subscribe(200, 400)
        );

        ys1.Subscriptions.AssertEqual(
            Subscribe(210, 350)
        );

        ys2.Subscriptions.AssertEqual(
            Subscribe(260, 400)
        );

        ys3.Subscriptions.AssertEqual(
            Subscribe(350, 400)
        );

        ys4.Subscriptions.AssertEqual(
        );
    }

    [TestMethod]
    public void MergeConcat_InnerError()
    {
        var ex = new Exception();

        var ys1 = Scheduler.CreateColdObservable(
            OnNext(50, 1),
            OnNext(100, 2),
            OnNext(120, 3),
            OnCompleted<int>(140)
        );

        var ys2 = Scheduler.CreateColdObservable(
            OnNext(20, 4),
            OnNext(70, 5),
            OnCompleted<int>(200)
        );

        var ys3 = Scheduler.CreateColdObservable(
            OnNext(10, 6),
            OnNext(90, 7),
            OnNext(110, 8),
            OnError<int>(140, ex)
        );

        var ys4 = Scheduler.CreateColdObservable(
            OnNext(210, 9),
            OnNext(240, 10),
            OnCompleted<int>(300)
        );

        var xs = Scheduler.CreateHotObservable(
            OnNext<Seq<int>>(210, ys1),
            OnNext<Seq<int>>(260, ys2),
            OnNext<Seq<int>>(270, ys3),
            OnNext<Seq<int>>(320, ys4),
            OnCompleted<Seq<int>>(400)
        );

        var res = Scheduler.Start(() =>
            xs.Merge(2)
        );

        res.Messages.AssertEqual(
            OnNext(260, 1),
            OnNext(280, 4),
            OnNext(310, 2),
            OnNext(330, 3),
            OnNext(330, 5),
            OnNext(360, 6),
            OnNext(440, 7),
            OnNext(460, 8),
            OnError<int>(490, ex)
        );

        xs.Subscriptions.AssertEqual(
            Subscribe(200, 400)
        );

        ys1.Subscriptions.AssertEqual(
            Subscribe(210, 350)
        );

        ys2.Subscriptions.AssertEqual(
            Subscribe(260, 460)
        );

        ys3.Subscriptions.AssertEqual(
            Subscribe(350, 490)
        );

        ys4.Subscriptions.AssertEqual(
            Subscribe(460, 490)
        );
    }

    [TestMethod]
    public async Task Merge_Task()
    {
        var tss = Seq.Merge(new[] { Task.Factory.StartNew(() => 1), Task.Factory.StartNew(() => 2), Task.Factory.StartNew(() => 3) }.ToSeq());

        var res = await ToListAsync(tss);

        Assert.IsTrue(res.OrderBy(x => x).SequenceEqual([1, 2, 3]));
    }

    [TestMethod]
    public async Task Merge_TaskWithCompletionSource_RanToCompletion_Async()
    {
        var tcss = new TaskCompletionSource<int>[2];
        tcss[0] = new TaskCompletionSource<int>();
        tcss[1] = new TaskCompletionSource<int>();

        var res = Seq.Merge(Seq.Range(0, 2).Select(x => tcss[x].Task));

        var run = ToListAsync(res);

        tcss[0].SetResult(42);
        tcss[1].SetResult(43);

        var lst = await run;

        lst.OrderBy(x => x).AssertEqual([42, 43]);
    }

    [TestMethod]
    public async Task Merge_TaskWithCompletionSource_RanToCompletion_Sync()
    {
        var tcss = new TaskCompletionSource<int>[2];
        tcss[0] = new TaskCompletionSource<int>();
        tcss[1] = new TaskCompletionSource<int>();

        tcss[0].SetResult(42);
        tcss[1].SetResult(43);

        var res = Seq.Merge(Seq.Range(0, 2).Select(x => tcss[x].Task));

        var lst = await ToListAsync(res);

        lst.OrderBy(x => x).AssertEqual([42, 43]);
    }

    [TestMethod]
    public async Task Merge_TaskWithCompletionSource_Faulted_Async()
    {
        var tcss = new TaskCompletionSource<int>[3];
        tcss[0] = new TaskCompletionSource<int>();
        tcss[1] = new TaskCompletionSource<int>();
        tcss[2] = new TaskCompletionSource<int>();

        var res = Seq.Merge(Seq.Range(0, 3).Select(x => tcss[x].Task));

        var run = ToListAsync(res);

        var ex = new Exception();
        tcss[1].SetException(ex);

        var err = await Assert.ThrowsExactlyAsync<Exception>(async () => await run);

        Assert.AreSame(ex, err);
    }

    [TestMethod]
    public async Task Merge_TaskWithCompletionSource_Faulted_Sync()
    {
        var tcss = new TaskCompletionSource<int>[3];
        tcss[0] = new TaskCompletionSource<int>();
        tcss[1] = new TaskCompletionSource<int>();
        tcss[2] = new TaskCompletionSource<int>();

        var ex = new Exception();
        tcss[1].SetException(ex);

        var res = Seq.Merge(Seq.Range(0, 3).Select(x => tcss[x].Task));

        var err = await Assert.ThrowsExactlyAsync<Exception>(async () => await ToListAsync(res));

        Assert.AreSame(ex, err);
    }

    [TestMethod]
    public async Task Merge_TaskWithCompletionSource_Canceled_Async()
    {
        var tcss = new TaskCompletionSource<int>[3];
        tcss[0] = new TaskCompletionSource<int>();
        tcss[1] = new TaskCompletionSource<int>();
        tcss[2] = new TaskCompletionSource<int>();

        var res = Seq.Merge(Seq.Range(0, 3).Select(x => tcss[x].Task));

        var run = ToListAsync(res);

        tcss[1].SetCanceled();

        var err = await Assert.ThrowsExactlyAsync<TaskCanceledException>(async () => await run);

        Assert.AreSame(tcss[1].Task, err.Task);
    }

    [TestMethod]
    public async Task Merge_TaskWithCompletionSource_Canceled_Sync()
    {
        var tcss = new TaskCompletionSource<int>[3];
        tcss[0] = new TaskCompletionSource<int>();
        tcss[1] = new TaskCompletionSource<int>();
        tcss[2] = new TaskCompletionSource<int>();

        tcss[1].SetCanceled();

        var res = Seq.Merge(Seq.Range(0, 3).Select(x => tcss[x].Task));

        var err = await Assert.ThrowsExactlyAsync<TaskCanceledException>(async () => await ToListAsync(res));

        Assert.AreSame(tcss[1].Task, err.Task);
    }

    [TestMethod]
    public async Task Merge_TaskWithCompletionSource_InnerCompleteBeforeOuter()
    {
        var xs = CreateSubject<int>();

        var tcss = new TaskCompletionSource<int>[3];
        tcss[0] = new TaskCompletionSource<int>();
        tcss[1] = new TaskCompletionSource<int>();
        tcss[2] = new TaskCompletionSource<int>();

        var res = Seq.Merge(xs.Select(x => tcss[x].Task));

        var run = ToListAsync(res);

        tcss[1].SetResult(42);

        await xs.OnNextAsync(0);
        await xs.OnNextAsync(1);
        await xs.OnNextAsync(2);

        tcss[0].SetResult(43);
        tcss[2].SetResult(44);

        await xs.OnCompletedAsync();

        var lst = await run;

        lst.OrderBy(x => x).AssertEqual([42, 43, 44]);
    }

    [TestMethod]
    public async Task Merge_TaskWithCompletionSource_OuterCompleteBeforeInner()
    {
        var xs = CreateSubject<int>();

        var tcss = new TaskCompletionSource<int>[3];
        tcss[0] = new TaskCompletionSource<int>();
        tcss[1] = new TaskCompletionSource<int>();
        tcss[2] = new TaskCompletionSource<int>();

        var res = Seq.Merge(xs.Select(x => tcss[x].Task));

        var run = ToListAsync(res);

        tcss[1].SetResult(42);

        await xs.OnNextAsync(0);
        await xs.OnNextAsync(1);
        await xs.OnNextAsync(2);

        await xs.OnCompletedAsync();

        tcss[0].SetResult(43);
        tcss[2].SetResult(44);

        var lst = await run;

        lst.OrderBy(x => x).AssertEqual([42, 43, 44]);
    }

    [TestMethod]
    public async Task Merge_Task_OnError()
    {
        var xs = CreateSubject<int>();

        var tcss = new TaskCompletionSource<int>[3];
        tcss[0] = new TaskCompletionSource<int>();
        tcss[1] = new TaskCompletionSource<int>();
        tcss[2] = new TaskCompletionSource<int>();

        var res = Seq.Merge(xs.Select(x => tcss[x].Task));

        var run = ToListAsync(res);

        tcss[1].SetResult(42);

        await xs.OnNextAsync(0);
        await xs.OnNextAsync(1);
        await xs.OnNextAsync(2);

        tcss[0].SetResult(43);
        tcss[2].SetResult(44);

        var ex = new Exception();
        await xs.OnErrorAsync(ex);

        var err = await Assert.ThrowsExactlyAsync<Exception>(async () => await run);

        Assert.AreSame(ex, err);
    }
}

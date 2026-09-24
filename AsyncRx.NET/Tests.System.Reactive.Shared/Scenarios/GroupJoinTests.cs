// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information. 

using System.Reactive;
using Microsoft.Reactive.Testing;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Tests.System.Reactive.Shared.Scenarios;

/// <summary>
/// Shared <c>GroupJoin</c> scenarios: every behavioural test from Rx.NET's <c>GroupJoinTest.cs</c>.
/// The result selector receives the group as a value and returns a description; the platform
/// materializes it in place, as for <c>Window</c>. <c>NewTimer</c> keeps its sync signature: the
/// scheduler creates cold sources eagerly, exactly as the sync <c>TestScheduler</c> does.
/// </summary>
/// <remarks>
/// Transcribed mechanically from the sync file; only the two helpers at the bottom are
/// hand-written.
/// </remarks>
public abstract class GroupJoinTests : DescribedTest
{
    [TestMethod]
    public void GroupJoinOp_Normal_I()
    {
        var xs = Scheduler.CreateHotObservable(
            OnNext(210, new TimeInterval<int>(0, TimeSpan.FromTicks(10))),
            OnNext(219, new TimeInterval<int>(1, TimeSpan.FromTicks(5))),
            OnNext(240, new TimeInterval<int>(2, TimeSpan.FromTicks(10))),
            OnNext(300, new TimeInterval<int>(3, TimeSpan.FromTicks(100))),
            OnNext(310, new TimeInterval<int>(4, TimeSpan.FromTicks(80))),
            OnNext(500, new TimeInterval<int>(5, TimeSpan.FromTicks(90))),
            OnNext(700, new TimeInterval<int>(6, TimeSpan.FromTicks(25))),
            OnNext(710, new TimeInterval<int>(7, TimeSpan.FromTicks(280))),
            OnNext(720, new TimeInterval<int>(8, TimeSpan.FromTicks(100))),
            OnNext(830, new TimeInterval<int>(9, TimeSpan.FromTicks(10))),
            OnCompleted<TimeInterval<int>>(900)
        );

        var ys = Scheduler.CreateHotObservable(
            OnNext(215, new TimeInterval<string>("hat", TimeSpan.FromTicks(20))),
            OnNext(217, new TimeInterval<string>("bat", TimeSpan.FromTicks(1))),
            OnNext(290, new TimeInterval<string>("wag", TimeSpan.FromTicks(200))),
            OnNext(300, new TimeInterval<string>("pig", TimeSpan.FromTicks(10))),
            OnNext(305, new TimeInterval<string>("cup", TimeSpan.FromTicks(50))),
            OnNext(600, new TimeInterval<string>("yak", TimeSpan.FromTicks(90))),
            OnNext(702, new TimeInterval<string>("tin", TimeSpan.FromTicks(20))),
            OnNext(712, new TimeInterval<string>("man", TimeSpan.FromTicks(10))),
            OnNext(722, new TimeInterval<string>("rat", TimeSpan.FromTicks(200))),
            OnNext(732, new TimeInterval<string>("wig", TimeSpan.FromTicks(5))),
            OnCompleted<TimeInterval<string>>(800)
        );

        var xsd = new List<TestableSeq<long>>();
        var ysd = new List<TestableSeq<long>>();

        var res = Scheduler.Start(() =>
            xs.GroupJoin(ys, x => NewTimer(xsd, x.Interval, Scheduler), y => NewTimer(ysd, y.Interval, Scheduler), (x, yy) => yy.Select(y => x.Value + y.Value)).Merge()
        );

        res.Messages.AssertEqual(
            OnNext(215, "0hat"),
            OnNext(217, "0bat"),
            OnNext(219, "1hat"),
            OnNext(300, "3wag"),
            OnNext(300, "3pig"),
            OnNext(305, "3cup"),
            OnNext(310, "4wag"),
            OnNext(310, "4pig"),
            OnNext(310, "4cup"),
            OnNext(702, "6tin"),
            OnNext(710, "7tin"),
            OnNext(712, "6man"),
            OnNext(712, "7man"),
            OnNext(720, "8tin"),
            OnNext(720, "8man"),
            OnNext(722, "6rat"),
            OnNext(722, "7rat"),
            OnNext(722, "8rat"),
            OnNext(732, "7wig"),
            OnNext(732, "8wig"),
            OnNext(830, "9rat"),
            OnCompleted<string>(990)
        );

        AssertDurations(xs, xsd, 990);
        AssertDurations(ys, ysd, 990);

        xs.Subscriptions.AssertEqual(
            Subscribe(200, 900)
        );

        ys.Subscriptions.AssertEqual(
            Subscribe(200, 800)
        );
    }

    [TestMethod]
    public void GroupJoinOp_Normal_II()
    {
        var xs = Scheduler.CreateHotObservable(
            OnNext(210, new TimeInterval<int>(0, TimeSpan.FromTicks(10))),
            OnNext(219, new TimeInterval<int>(1, TimeSpan.FromTicks(5))),
            OnNext(240, new TimeInterval<int>(2, TimeSpan.FromTicks(10))),
            OnNext(300, new TimeInterval<int>(3, TimeSpan.FromTicks(100))),
            OnNext(310, new TimeInterval<int>(4, TimeSpan.FromTicks(80))),
            OnNext(500, new TimeInterval<int>(5, TimeSpan.FromTicks(90))),
            OnNext(700, new TimeInterval<int>(6, TimeSpan.FromTicks(25))),
            OnNext(710, new TimeInterval<int>(7, TimeSpan.FromTicks(200))),
            OnNext(720, new TimeInterval<int>(8, TimeSpan.FromTicks(100))),
            OnCompleted<TimeInterval<int>>(721)
        );

        var ys = Scheduler.CreateHotObservable(
            OnNext(215, new TimeInterval<string>("hat", TimeSpan.FromTicks(20))),
            OnNext(217, new TimeInterval<string>("bat", TimeSpan.FromTicks(1))),
            OnNext(290, new TimeInterval<string>("wag", TimeSpan.FromTicks(200))),
            OnNext(300, new TimeInterval<string>("pig", TimeSpan.FromTicks(10))),
            OnNext(305, new TimeInterval<string>("cup", TimeSpan.FromTicks(50))),
            OnNext(600, new TimeInterval<string>("yak", TimeSpan.FromTicks(90))),
            OnNext(702, new TimeInterval<string>("tin", TimeSpan.FromTicks(20))),
            OnNext(712, new TimeInterval<string>("man", TimeSpan.FromTicks(10))),
            OnNext(722, new TimeInterval<string>("rat", TimeSpan.FromTicks(200))),
            OnNext(732, new TimeInterval<string>("wig", TimeSpan.FromTicks(5))),
            OnCompleted<TimeInterval<string>>(990)
        );

        var xsd = new List<TestableSeq<long>>();
        var ysd = new List<TestableSeq<long>>();

        var res = Scheduler.Start(() =>
            xs.GroupJoin(ys, x => NewTimer(xsd, x.Interval, Scheduler), y => NewTimer(ysd, y.Interval, Scheduler), (x, yy) => yy.Select(y => x.Value + y.Value)).Merge()
        );

        res.Messages.AssertEqual(
            OnNext(215, "0hat"),
            OnNext(217, "0bat"),
            OnNext(219, "1hat"),
            OnNext(300, "3wag"),
            OnNext(300, "3pig"),
            OnNext(305, "3cup"),
            OnNext(310, "4wag"),
            OnNext(310, "4pig"),
            OnNext(310, "4cup"),
            OnNext(702, "6tin"),
            OnNext(710, "7tin"),
            OnNext(712, "6man"),
            OnNext(712, "7man"),
            OnNext(720, "8tin"),
            OnNext(720, "8man"),
            OnNext(722, "6rat"),
            OnNext(722, "7rat"),
            OnNext(722, "8rat"),
            OnNext(732, "7wig"),
            OnNext(732, "8wig"),
            OnCompleted<string>(910)
        );

        AssertDurations(xs, xsd, 910);
        AssertDurations(ys, ysd, 910);

        xs.Subscriptions.AssertEqual(
            Subscribe(200, 721)
        );

        ys.Subscriptions.AssertEqual(
            Subscribe(200, 910)
        );
    }

    [TestMethod]
    public void GroupJoinOp_Normal_III()
    {
        var xs = Scheduler.CreateHotObservable(
            OnNext(210, new TimeInterval<int>(0, TimeSpan.FromTicks(10))),
            OnNext(219, new TimeInterval<int>(1, TimeSpan.FromTicks(5))),
            OnNext(240, new TimeInterval<int>(2, TimeSpan.FromTicks(10))),
            OnNext(300, new TimeInterval<int>(3, TimeSpan.FromTicks(100))),
            OnNext(310, new TimeInterval<int>(4, TimeSpan.FromTicks(80))),
            OnNext(500, new TimeInterval<int>(5, TimeSpan.FromTicks(90))),
            OnNext(700, new TimeInterval<int>(6, TimeSpan.FromTicks(25))),
            OnNext(710, new TimeInterval<int>(7, TimeSpan.FromTicks(280))),
            OnNext(720, new TimeInterval<int>(8, TimeSpan.FromTicks(100))),
            OnNext(830, new TimeInterval<int>(9, TimeSpan.FromTicks(10))),
            OnCompleted<TimeInterval<int>>(900)
        );

        var ys = Scheduler.CreateHotObservable(
            OnNext(215, new TimeInterval<string>("hat", TimeSpan.FromTicks(20))),
            OnNext(217, new TimeInterval<string>("bat", TimeSpan.FromTicks(1))),
            OnNext(290, new TimeInterval<string>("wag", TimeSpan.FromTicks(200))),
            OnNext(300, new TimeInterval<string>("pig", TimeSpan.FromTicks(10))),
            OnNext(305, new TimeInterval<string>("cup", TimeSpan.FromTicks(50))),
            OnNext(600, new TimeInterval<string>("yak", TimeSpan.FromTicks(90))),
            OnNext(702, new TimeInterval<string>("tin", TimeSpan.FromTicks(20))),
            OnNext(712, new TimeInterval<string>("man", TimeSpan.FromTicks(10))),
            OnNext(722, new TimeInterval<string>("rat", TimeSpan.FromTicks(200))),
            OnNext(732, new TimeInterval<string>("wig", TimeSpan.FromTicks(5))),
            OnCompleted<TimeInterval<string>>(800)
        );

        var xsd = new List<TestableSeq<long>>();
        var ysd = new List<TestableSeq<long>>();

        var res = Scheduler.Start(() =>
            xs.GroupJoin(ys, x => NewTimer(xsd, x.Interval, Scheduler).Where(_ => false), y => NewTimer(ysd, y.Interval, Scheduler).Where(_ => false), (x, yy) => yy.Select(y => x.Value + y.Value)).Merge()
        );

        res.Messages.AssertEqual(
            OnNext(215, "0hat"),
            OnNext(217, "0bat"),
            OnNext(219, "1hat"),
            OnNext(300, "3wag"),
            OnNext(300, "3pig"),
            OnNext(305, "3cup"),
            OnNext(310, "4wag"),
            OnNext(310, "4pig"),
            OnNext(310, "4cup"),
            OnNext(702, "6tin"),
            OnNext(710, "7tin"),
            OnNext(712, "6man"),
            OnNext(712, "7man"),
            OnNext(720, "8tin"),
            OnNext(720, "8man"),
            OnNext(722, "6rat"),
            OnNext(722, "7rat"),
            OnNext(722, "8rat"),
            OnNext(732, "7wig"),
            OnNext(732, "8wig"),
            OnNext(830, "9rat"),
            OnCompleted<string>(990)
        );

        AssertDurations(xs, xsd, 990);
        AssertDurations(ys, ysd, 990);

        xs.Subscriptions.AssertEqual(
            Subscribe(200, 900)
        );

        ys.Subscriptions.AssertEqual(
            Subscribe(200, 800)
        );
    }

    [TestMethod]
    public void GroupJoinOp_Normal_IV()
    {
        var xs = Scheduler.CreateHotObservable(
            OnNext(210, new TimeInterval<int>(0, TimeSpan.FromTicks(10))),
            OnNext(219, new TimeInterval<int>(1, TimeSpan.FromTicks(5))),
            OnNext(240, new TimeInterval<int>(2, TimeSpan.FromTicks(10))),
            OnNext(300, new TimeInterval<int>(3, TimeSpan.FromTicks(100))),
            OnNext(310, new TimeInterval<int>(4, TimeSpan.FromTicks(80))),
            OnNext(500, new TimeInterval<int>(5, TimeSpan.FromTicks(90))),
            OnNext(700, new TimeInterval<int>(6, TimeSpan.FromTicks(25))),
            OnNext(710, new TimeInterval<int>(7, TimeSpan.FromTicks(200))),
            OnNext(720, new TimeInterval<int>(8, TimeSpan.FromTicks(100))),
            OnCompleted<TimeInterval<int>>(990)
        );

        var ys = Scheduler.CreateHotObservable(
            OnNext(215, new TimeInterval<string>("hat", TimeSpan.FromTicks(20))),
            OnNext(217, new TimeInterval<string>("bat", TimeSpan.FromTicks(1))),
            OnNext(290, new TimeInterval<string>("wag", TimeSpan.FromTicks(200))),
            OnNext(300, new TimeInterval<string>("pig", TimeSpan.FromTicks(10))),
            OnNext(305, new TimeInterval<string>("cup", TimeSpan.FromTicks(50))),
            OnNext(600, new TimeInterval<string>("yak", TimeSpan.FromTicks(90))),
            OnNext(702, new TimeInterval<string>("tin", TimeSpan.FromTicks(20))),
            OnNext(712, new TimeInterval<string>("man", TimeSpan.FromTicks(10))),
            OnNext(722, new TimeInterval<string>("rat", TimeSpan.FromTicks(200))),
            OnNext(732, new TimeInterval<string>("wig", TimeSpan.FromTicks(5))),
            OnCompleted<TimeInterval<string>>(980)
        );

        var xsd = new List<TestableSeq<long>>();
        var ysd = new List<TestableSeq<long>>();

        var res = Scheduler.Start(() =>
            xs.GroupJoin(ys, x => NewTimer(xsd, x.Interval, Scheduler), y => NewTimer(ysd, y.Interval, Scheduler), (x, yy) => yy.Select(y => x.Value + y.Value)).Merge()
        );

        res.Messages.AssertEqual(
            OnNext(215, "0hat"),
            OnNext(217, "0bat"),
            OnNext(219, "1hat"),
            OnNext(300, "3wag"),
            OnNext(300, "3pig"),
            OnNext(305, "3cup"),
            OnNext(310, "4wag"),
            OnNext(310, "4pig"),
            OnNext(310, "4cup"),
            OnNext(702, "6tin"),
            OnNext(710, "7tin"),
            OnNext(712, "6man"),
            OnNext(712, "7man"),
            OnNext(720, "8tin"),
            OnNext(720, "8man"),
            OnNext(722, "6rat"),
            OnNext(722, "7rat"),
            OnNext(722, "8rat"),
            OnNext(732, "7wig"),
            OnNext(732, "8wig"),
            OnCompleted<string>(990)
        );

        AssertDurations(xs, xsd, 990);
        AssertDurations(ys, ysd, 990);

        xs.Subscriptions.AssertEqual(
            Subscribe(200, 990)
        );

        ys.Subscriptions.AssertEqual(
            Subscribe(200, 980)
        );
    }

    [TestMethod]
    public void GroupJoinOp_Normal_V()
    {
        var xs = Scheduler.CreateHotObservable(
            OnNext(210, new TimeInterval<int>(0, TimeSpan.FromTicks(10))),
            OnNext(219, new TimeInterval<int>(1, TimeSpan.FromTicks(5))),
            OnNext(240, new TimeInterval<int>(2, TimeSpan.FromTicks(10))),
            OnNext(300, new TimeInterval<int>(3, TimeSpan.FromTicks(100))),
            OnNext(310, new TimeInterval<int>(4, TimeSpan.FromTicks(80))),
            OnNext(500, new TimeInterval<int>(5, TimeSpan.FromTicks(90))),
            OnNext(700, new TimeInterval<int>(6, TimeSpan.FromTicks(25))),
            OnNext(710, new TimeInterval<int>(7, TimeSpan.FromTicks(200))),
            OnNext(720, new TimeInterval<int>(8, TimeSpan.FromTicks(100))),
            OnCompleted<TimeInterval<int>>(990)
        );

        var ys = Scheduler.CreateHotObservable(
            OnNext(215, new TimeInterval<string>("hat", TimeSpan.FromTicks(20))),
            OnNext(217, new TimeInterval<string>("bat", TimeSpan.FromTicks(1))),
            OnNext(290, new TimeInterval<string>("wag", TimeSpan.FromTicks(200))),
            OnNext(300, new TimeInterval<string>("pig", TimeSpan.FromTicks(10))),
            OnNext(305, new TimeInterval<string>("cup", TimeSpan.FromTicks(50))),
            OnNext(600, new TimeInterval<string>("yak", TimeSpan.FromTicks(90))),
            OnNext(702, new TimeInterval<string>("tin", TimeSpan.FromTicks(20))),
            OnNext(712, new TimeInterval<string>("man", TimeSpan.FromTicks(10))),
            OnNext(722, new TimeInterval<string>("rat", TimeSpan.FromTicks(200))),
            OnNext(732, new TimeInterval<string>("wig", TimeSpan.FromTicks(5))),
            OnCompleted<TimeInterval<string>>(900)
        );

        var xsd = new List<TestableSeq<long>>();
        var ysd = new List<TestableSeq<long>>();

        var res = Scheduler.Start(() =>
            xs.GroupJoin(ys, x => NewTimer(xsd, x.Interval, Scheduler), y => NewTimer(ysd, y.Interval, Scheduler), (x, yy) => yy.Select(y => x.Value + y.Value)).Merge()
        );

        res.Messages.AssertEqual(
            OnNext(215, "0hat"),
            OnNext(217, "0bat"),
            OnNext(219, "1hat"),
            OnNext(300, "3wag"),
            OnNext(300, "3pig"),
            OnNext(305, "3cup"),
            OnNext(310, "4wag"),
            OnNext(310, "4pig"),
            OnNext(310, "4cup"),
            OnNext(702, "6tin"),
            OnNext(710, "7tin"),
            OnNext(712, "6man"),
            OnNext(712, "7man"),
            OnNext(720, "8tin"),
            OnNext(720, "8man"),
            OnNext(722, "6rat"),
            OnNext(722, "7rat"),
            OnNext(722, "8rat"),
            OnNext(732, "7wig"),
            OnNext(732, "8wig"),
            OnCompleted<string>(990)
        );

        AssertDurations(xs, xsd, 990);
        AssertDurations(ys, ysd, 990);

        xs.Subscriptions.AssertEqual(
            Subscribe(200, 990)
        );

        ys.Subscriptions.AssertEqual(
            Subscribe(200, 900)
        );
    }

    [TestMethod]
    public void GroupJoinOp_Normal_VI()
    {
        var xs = Scheduler.CreateHotObservable(
            OnNext(210, new TimeInterval<int>(0, TimeSpan.FromTicks(10))),
            OnNext(219, new TimeInterval<int>(1, TimeSpan.FromTicks(5))),
            OnNext(240, new TimeInterval<int>(2, TimeSpan.FromTicks(10))),
            OnNext(300, new TimeInterval<int>(3, TimeSpan.FromTicks(100))),
            OnNext(310, new TimeInterval<int>(4, TimeSpan.FromTicks(80))),
            OnNext(500, new TimeInterval<int>(5, TimeSpan.FromTicks(90))),
            OnNext(700, new TimeInterval<int>(6, TimeSpan.FromTicks(25))),
            OnNext(710, new TimeInterval<int>(7, TimeSpan.FromTicks(30))),
            OnNext(720, new TimeInterval<int>(8, TimeSpan.FromTicks(200))),
            OnNext(830, new TimeInterval<int>(9, TimeSpan.FromTicks(10))),
            OnCompleted<TimeInterval<int>>(850)
        );

        var ys = Scheduler.CreateHotObservable(
            OnNext(215, new TimeInterval<string>("hat", TimeSpan.FromTicks(20))),
            OnNext(217, new TimeInterval<string>("bat", TimeSpan.FromTicks(1))),
            OnNext(290, new TimeInterval<string>("wag", TimeSpan.FromTicks(200))),
            OnNext(300, new TimeInterval<string>("pig", TimeSpan.FromTicks(10))),
            OnNext(305, new TimeInterval<string>("cup", TimeSpan.FromTicks(50))),
            OnNext(600, new TimeInterval<string>("yak", TimeSpan.FromTicks(90))),
            OnNext(702, new TimeInterval<string>("tin", TimeSpan.FromTicks(20))),
            OnNext(712, new TimeInterval<string>("man", TimeSpan.FromTicks(10))),
            OnNext(722, new TimeInterval<string>("rat", TimeSpan.FromTicks(20))),
            OnNext(732, new TimeInterval<string>("wig", TimeSpan.FromTicks(5))),
            OnCompleted<TimeInterval<string>>(900)
        );

        var xsd = new List<TestableSeq<long>>();
        var ysd = new List<TestableSeq<long>>();

        var res = Scheduler.Start(() =>
            xs.GroupJoin(ys, x => NewTimer(xsd, x.Interval, Scheduler), y => NewTimer(ysd, y.Interval, Scheduler), (x, yy) => yy.Select(y => x.Value + y.Value)).Merge()
        );

        res.Messages.AssertEqual(
            OnNext(215, "0hat"),
            OnNext(217, "0bat"),
            OnNext(219, "1hat"),
            OnNext(300, "3wag"),
            OnNext(300, "3pig"),
            OnNext(305, "3cup"),
            OnNext(310, "4wag"),
            OnNext(310, "4pig"),
            OnNext(310, "4cup"),
            OnNext(702, "6tin"),
            OnNext(710, "7tin"),
            OnNext(712, "6man"),
            OnNext(712, "7man"),
            OnNext(720, "8tin"),
            OnNext(720, "8man"),
            OnNext(722, "6rat"),
            OnNext(722, "7rat"),
            OnNext(722, "8rat"),
            OnNext(732, "7wig"),
            OnNext(732, "8wig"),
            OnCompleted<string>(920)
        );

        AssertDurations(xs, xsd, 920);
        AssertDurations(ys, ysd, 920);

        xs.Subscriptions.AssertEqual(
            Subscribe(200, 850)
        );

        ys.Subscriptions.AssertEqual(
            Subscribe(200, 900)
        );
    }

    [TestMethod]
    public void GroupJoinOp_Normal_VII()
    {
        var xs = Scheduler.CreateHotObservable(
            OnCompleted<TimeInterval<int>>(210)
        );

        var ys = Scheduler.CreateHotObservable(
            OnNext(215, new TimeInterval<string>("hat", TimeSpan.FromTicks(20))),
            OnNext(217, new TimeInterval<string>("bat", TimeSpan.FromTicks(1))),
            OnNext(290, new TimeInterval<string>("wag", TimeSpan.FromTicks(200))),
            OnNext(300, new TimeInterval<string>("pig", TimeSpan.FromTicks(10))),
            OnNext(305, new TimeInterval<string>("cup", TimeSpan.FromTicks(50))),
            OnNext(600, new TimeInterval<string>("yak", TimeSpan.FromTicks(90))),
            OnNext(702, new TimeInterval<string>("tin", TimeSpan.FromTicks(20))),
            OnNext(712, new TimeInterval<string>("man", TimeSpan.FromTicks(10))),
            OnNext(722, new TimeInterval<string>("rat", TimeSpan.FromTicks(20))),
            OnNext(732, new TimeInterval<string>("wig", TimeSpan.FromTicks(5))),
            OnCompleted<TimeInterval<string>>(900)
        );

        var xsd = new List<TestableSeq<long>>();
        var ysd = new List<TestableSeq<long>>();

        var res = Scheduler.Start(() =>
            xs.GroupJoin(ys, x => NewTimer(xsd, x.Interval, Scheduler), y => NewTimer(ysd, y.Interval, Scheduler), (x, yy) => yy.Select(y => x.Value + y.Value)).Merge()
        );

        res.Messages.AssertEqual(
            OnCompleted<string>(210)
        );

        AssertDurations(xs, xsd, 210);
        AssertDurations(ys, ysd, 210);

        xs.Subscriptions.AssertEqual(
            Subscribe(200, 210)
        );

        ys.Subscriptions.AssertEqual(
            Subscribe(200, 210)
        );
    }

    [TestMethod]
    public void GroupJoinOp_Normal_VIII()
    {
        var xs = Scheduler.CreateHotObservable(
            OnNext(210, new TimeInterval<int>(0, TimeSpan.FromTicks(200)))
        );

        var ys = Scheduler.CreateHotObservable(
            OnNext(220, new TimeInterval<string>("hat", TimeSpan.FromTicks(100))),
            OnCompleted<TimeInterval<string>>(230)
        );

        var xsd = new List<TestableSeq<long>>();
        var ysd = new List<TestableSeq<long>>();

        var res = Scheduler.Start(() =>
            xs.GroupJoin(ys, x => NewTimer(xsd, x.Interval, Scheduler), y => NewTimer(ysd, y.Interval, Scheduler), (x, yy) => yy.Select(y => x.Value + y.Value)).Merge()
        );

        res.Messages.AssertEqual(
            OnNext(220, "0hat")
        );

        AssertDurations(xs, xsd, 1000);
        AssertDurations(ys, ysd, 1000);

        xs.Subscriptions.AssertEqual(
            Subscribe(200, 1000)
        );

        ys.Subscriptions.AssertEqual(
            Subscribe(200, 230)
        );
    }

    [TestMethod]
    public void GroupJoinOp_Normal_IX()
    {
        var xs = Scheduler.CreateHotObservable(
            OnNext(210, new TimeInterval<int>(0, TimeSpan.FromTicks(10))),
            OnNext(219, new TimeInterval<int>(1, TimeSpan.FromTicks(5))),
            OnNext(240, new TimeInterval<int>(2, TimeSpan.FromTicks(10))),
            OnNext(300, new TimeInterval<int>(3, TimeSpan.FromTicks(100))),
            OnNext(310, new TimeInterval<int>(4, TimeSpan.FromTicks(80))),
            OnNext(500, new TimeInterval<int>(5, TimeSpan.FromTicks(90))),
            OnNext(700, new TimeInterval<int>(6, TimeSpan.FromTicks(25))),
            OnNext(710, new TimeInterval<int>(7, TimeSpan.FromTicks(300))),
            OnNext(720, new TimeInterval<int>(8, TimeSpan.FromTicks(100))),
            OnNext(830, new TimeInterval<int>(9, TimeSpan.FromTicks(10))),
            OnCompleted<TimeInterval<int>>(900)
        );

        var ys = Scheduler.CreateHotObservable(
            OnNext(215, new TimeInterval<string>("hat", TimeSpan.FromTicks(20))),
            OnNext(217, new TimeInterval<string>("bat", TimeSpan.FromTicks(1))),
            OnNext(290, new TimeInterval<string>("wag", TimeSpan.FromTicks(200))),
            OnNext(300, new TimeInterval<string>("pig", TimeSpan.FromTicks(10))),
            OnNext(305, new TimeInterval<string>("cup", TimeSpan.FromTicks(50))),
            OnNext(600, new TimeInterval<string>("yak", TimeSpan.FromTicks(90))),
            OnNext(702, new TimeInterval<string>("tin", TimeSpan.FromTicks(20))),
            OnNext(712, new TimeInterval<string>("man", TimeSpan.FromTicks(10))),
            OnNext(722, new TimeInterval<string>("rat", TimeSpan.FromTicks(200))),
            OnNext(732, new TimeInterval<string>("wig", TimeSpan.FromTicks(5))),
            OnCompleted<TimeInterval<string>>(800)
        );

        var xsd = new List<TestableSeq<long>>();
        var ysd = new List<TestableSeq<long>>();

        var res = Scheduler.Start(() =>
            xs.GroupJoin(ys, x => NewTimer(xsd, x.Interval, Scheduler), y => NewTimer(ysd, y.Interval, Scheduler), (x, yy) => yy.Select(y => x.Value + y.Value)).Merge(),
            713
        );

        res.Messages.AssertEqual(
            OnNext(215, "0hat"),
            OnNext(217, "0bat"),
            OnNext(219, "1hat"),
            OnNext(300, "3wag"),
            OnNext(300, "3pig"),
            OnNext(305, "3cup"),
            OnNext(310, "4wag"),
            OnNext(310, "4pig"),
            OnNext(310, "4cup"),
            OnNext(702, "6tin"),
            OnNext(710, "7tin"),
            OnNext(712, "6man"),
            OnNext(712, "7man")
        );

        AssertDurations(xs, xsd, 713);
        AssertDurations(ys, ysd, 713);

        xs.Subscriptions.AssertEqual(
            Subscribe(200, 713)
        );

        ys.Subscriptions.AssertEqual(
            Subscribe(200, 713)
        );
    }

    [TestMethod]
    public void GroupJoinOp_Error_I()
    {
        var ex = new Exception();

        var xs = Scheduler.CreateHotObservable(
            OnNext(210, new TimeInterval<int>(0, TimeSpan.FromTicks(10))),
            OnNext(219, new TimeInterval<int>(1, TimeSpan.FromTicks(5))),
            OnNext(240, new TimeInterval<int>(2, TimeSpan.FromTicks(10))),
            OnNext(300, new TimeInterval<int>(3, TimeSpan.FromTicks(100))),
            OnError<TimeInterval<int>>(310, ex)
        );

        var ys = Scheduler.CreateHotObservable(
            OnNext(215, new TimeInterval<string>("hat", TimeSpan.FromTicks(20))),
            OnNext(217, new TimeInterval<string>("bat", TimeSpan.FromTicks(1))),
            OnNext(290, new TimeInterval<string>("wag", TimeSpan.FromTicks(200))),
            OnNext(300, new TimeInterval<string>("pig", TimeSpan.FromTicks(10))),
            OnNext(305, new TimeInterval<string>("cup", TimeSpan.FromTicks(50))),
            OnNext(600, new TimeInterval<string>("yak", TimeSpan.FromTicks(90))),
            OnNext(702, new TimeInterval<string>("tin", TimeSpan.FromTicks(20))),
            OnNext(712, new TimeInterval<string>("man", TimeSpan.FromTicks(10))),
            OnNext(722, new TimeInterval<string>("rat", TimeSpan.FromTicks(200))),
            OnNext(732, new TimeInterval<string>("wig", TimeSpan.FromTicks(5))),
            OnCompleted<TimeInterval<string>>(800)
        );

        var xsd = new List<TestableSeq<long>>();
        var ysd = new List<TestableSeq<long>>();

        var res = Scheduler.Start(() =>
            xs.GroupJoin(ys, x => NewTimer(xsd, x.Interval, Scheduler), y => NewTimer(ysd, y.Interval, Scheduler), (x, yy) => yy.Select(y => x.Value + y.Value)).Merge()
        );

        res.Messages.AssertEqual(
            OnNext(215, "0hat"),
            OnNext(217, "0bat"),
            OnNext(219, "1hat"),
            OnNext(300, "3wag"),
            OnNext(300, "3pig"),
            OnNext(305, "3cup"),
            OnError<string>(310, ex)
        );

        AssertDurations(xs, xsd, 310);
        AssertDurations(ys, ysd, 310);

        xs.Subscriptions.AssertEqual(
            Subscribe(200, 310)
        );

        ys.Subscriptions.AssertEqual(
            Subscribe(200, 310)
        );
    }

    [TestMethod]
    public void GroupJoinOp_Error_II()
    {
        var ex = new Exception();

        var xs = Scheduler.CreateHotObservable(
            OnNext(210, new TimeInterval<int>(0, TimeSpan.FromTicks(10))),
            OnNext(219, new TimeInterval<int>(1, TimeSpan.FromTicks(5))),
            OnNext(240, new TimeInterval<int>(2, TimeSpan.FromTicks(10))),
            OnNext(300, new TimeInterval<int>(3, TimeSpan.FromTicks(100))),
            OnNext(310, new TimeInterval<int>(4, TimeSpan.FromTicks(80))),
            OnNext(500, new TimeInterval<int>(5, TimeSpan.FromTicks(90))),
            OnNext(700, new TimeInterval<int>(6, TimeSpan.FromTicks(25))),
            OnNext(710, new TimeInterval<int>(7, TimeSpan.FromTicks(300))),
            OnNext(720, new TimeInterval<int>(8, TimeSpan.FromTicks(100))),
            OnNext(830, new TimeInterval<int>(9, TimeSpan.FromTicks(10))),
            OnCompleted<TimeInterval<int>>(900)
        );

        var ys = Scheduler.CreateHotObservable(
            OnNext(215, new TimeInterval<string>("hat", TimeSpan.FromTicks(20))),
            OnNext(217, new TimeInterval<string>("bat", TimeSpan.FromTicks(1))),
            OnNext(290, new TimeInterval<string>("wag", TimeSpan.FromTicks(200))),
            OnNext(300, new TimeInterval<string>("pig", TimeSpan.FromTicks(10))),
            OnNext(305, new TimeInterval<string>("cup", TimeSpan.FromTicks(50))),
            OnNext(600, new TimeInterval<string>("yak", TimeSpan.FromTicks(90))),
            OnNext(702, new TimeInterval<string>("tin", TimeSpan.FromTicks(20))),
            OnNext(712, new TimeInterval<string>("man", TimeSpan.FromTicks(10))),
            OnError<TimeInterval<string>>(722, ex)
        );

        var xsd = new List<TestableSeq<long>>();
        var ysd = new List<TestableSeq<long>>();

        var res = Scheduler.Start(() =>
            xs.GroupJoin(ys, x => NewTimer(xsd, x.Interval, Scheduler), y => NewTimer(ysd, y.Interval, Scheduler), (x, yy) => yy.Select(y => x.Value + y.Value)).Merge()
        );

        res.Messages.AssertEqual(
            OnNext(215, "0hat"),
            OnNext(217, "0bat"),
            OnNext(219, "1hat"),
            OnNext(300, "3wag"),
            OnNext(300, "3pig"),
            OnNext(305, "3cup"),
            OnNext(310, "4wag"),
            OnNext(310, "4pig"),
            OnNext(310, "4cup"),
            OnNext(702, "6tin"),
            OnNext(710, "7tin"),
            OnNext(712, "6man"),
            OnNext(712, "7man"),
            OnNext(720, "8tin"),
            OnNext(720, "8man"),
            OnError<string>(722, ex)
        );

        AssertDurations(xs, xsd, 722);
        AssertDurations(ys, ysd, 722);

        xs.Subscriptions.AssertEqual(
            Subscribe(200, 722)
        );

        ys.Subscriptions.AssertEqual(
            Subscribe(200, 722)
        );
    }

    [TestMethod]
    public void GroupJoinOp_Error_III()
    {
        var xs = Scheduler.CreateHotObservable(
            OnNext(210, new TimeInterval<int>(0, TimeSpan.FromTicks(10))),
            OnNext(219, new TimeInterval<int>(1, TimeSpan.FromTicks(5))),
            OnNext(240, new TimeInterval<int>(2, TimeSpan.FromTicks(10))),
            OnNext(300, new TimeInterval<int>(3, TimeSpan.FromTicks(100))),
            OnNext(310, new TimeInterval<int>(4, TimeSpan.FromTicks(80))),
            OnNext(500, new TimeInterval<int>(5, TimeSpan.FromTicks(90))),
            OnNext(700, new TimeInterval<int>(6, TimeSpan.FromTicks(25))),
            OnNext(710, new TimeInterval<int>(7, TimeSpan.FromTicks(300))),
            OnNext(720, new TimeInterval<int>(8, TimeSpan.FromTicks(100))),
            OnNext(830, new TimeInterval<int>(9, TimeSpan.FromTicks(10))),
            OnCompleted<TimeInterval<int>>(900)
        );

        var ys = Scheduler.CreateHotObservable(
            OnNext(215, new TimeInterval<string>("hat", TimeSpan.FromTicks(20))),
            OnNext(217, new TimeInterval<string>("bat", TimeSpan.FromTicks(1))),
            OnNext(290, new TimeInterval<string>("wag", TimeSpan.FromTicks(200))),
            OnNext(300, new TimeInterval<string>("pig", TimeSpan.FromTicks(10))),
            OnNext(305, new TimeInterval<string>("cup", TimeSpan.FromTicks(50))),
            OnNext(600, new TimeInterval<string>("yak", TimeSpan.FromTicks(90))),
            OnNext(702, new TimeInterval<string>("tin", TimeSpan.FromTicks(20))),
            OnNext(712, new TimeInterval<string>("man", TimeSpan.FromTicks(10))),
            OnNext(722, new TimeInterval<string>("rat", TimeSpan.FromTicks(200))),
            OnNext(732, new TimeInterval<string>("wig", TimeSpan.FromTicks(5))),
            OnCompleted<TimeInterval<string>>(800)
        );

        var ex = new Exception();

        var xsd = new List<TestableSeq<long>>();
        var ysd = new List<TestableSeq<long>>();

        var res = Scheduler.Start(() =>
            xs.GroupJoin(ys, x => NewTimer(xsd, x.Interval, Scheduler).SelectMany(x.Value == 6 ? Observable.Throw<long>(ex) : Observable.Empty<long>()), y => NewTimer(ysd, y.Interval, Scheduler), (x, yy) => yy.Select(y => x.Value + y.Value)).Merge()
        );

        res.Messages.AssertEqual(
            OnNext(215, "0hat"),
            OnNext(217, "0bat"),
            OnNext(219, "1hat"),
            OnNext(300, "3wag"),
            OnNext(300, "3pig"),
            OnNext(305, "3cup"),
            OnNext(310, "4wag"),
            OnNext(310, "4pig"),
            OnNext(310, "4cup"),
            OnNext(702, "6tin"),
            OnNext(710, "7tin"),
            OnNext(712, "6man"),
            OnNext(712, "7man"),
            OnNext(720, "8tin"),
            OnNext(720, "8man"),
            OnNext(722, "6rat"),
            OnNext(722, "7rat"),
            OnNext(722, "8rat"),
            OnError<string>(725, ex)
        );

        AssertDurations(xs, xsd, 725);
        AssertDurations(ys, ysd, 725);

        xs.Subscriptions.AssertEqual(
            Subscribe(200, 725)
        );

        ys.Subscriptions.AssertEqual(
            Subscribe(200, 725)
        );
    }

    [TestMethod]
    public void GroupJoinOp_Error_IV()
    {
        var xs = Scheduler.CreateHotObservable(
            OnNext(210, new TimeInterval<int>(0, TimeSpan.FromTicks(10))),
            OnNext(219, new TimeInterval<int>(1, TimeSpan.FromTicks(5))),
            OnNext(240, new TimeInterval<int>(2, TimeSpan.FromTicks(10))),
            OnNext(300, new TimeInterval<int>(3, TimeSpan.FromTicks(100))),
            OnNext(310, new TimeInterval<int>(4, TimeSpan.FromTicks(80))),
            OnNext(500, new TimeInterval<int>(5, TimeSpan.FromTicks(90))),
            OnNext(700, new TimeInterval<int>(6, TimeSpan.FromTicks(25))),
            OnNext(710, new TimeInterval<int>(7, TimeSpan.FromTicks(300))),
            OnNext(720, new TimeInterval<int>(8, TimeSpan.FromTicks(100))),
            OnNext(830, new TimeInterval<int>(9, TimeSpan.FromTicks(10))),
            OnCompleted<TimeInterval<int>>(900)
        );

        var ys = Scheduler.CreateHotObservable(
            OnNext(215, new TimeInterval<string>("hat", TimeSpan.FromTicks(20))),
            OnNext(217, new TimeInterval<string>("bat", TimeSpan.FromTicks(1))),
            OnNext(290, new TimeInterval<string>("wag", TimeSpan.FromTicks(200))),
            OnNext(300, new TimeInterval<string>("pig", TimeSpan.FromTicks(10))),
            OnNext(305, new TimeInterval<string>("cup", TimeSpan.FromTicks(50))),
            OnNext(600, new TimeInterval<string>("yak", TimeSpan.FromTicks(90))),
            OnNext(702, new TimeInterval<string>("tin", TimeSpan.FromTicks(19))),
            OnNext(712, new TimeInterval<string>("man", TimeSpan.FromTicks(10))),
            OnNext(722, new TimeInterval<string>("rat", TimeSpan.FromTicks(200))),
            OnNext(732, new TimeInterval<string>("wig", TimeSpan.FromTicks(5))),
            OnCompleted<TimeInterval<string>>(800)
        );

        var ex = new Exception();

        var xsd = new List<TestableSeq<long>>();
        var ysd = new List<TestableSeq<long>>();

        var res = Scheduler.Start(() =>
            xs.GroupJoin(ys, x => NewTimer(xsd, x.Interval, Scheduler), y => NewTimer(ysd, y.Interval, Scheduler).SelectMany(y.Value == "tin" ? Observable.Throw<long>(ex) : Observable.Empty<long>()), (x, yy) => yy.Select(y => x.Value + y.Value)).Merge()
        );

        res.Messages.AssertEqual(
            OnNext(215, "0hat"),
            OnNext(217, "0bat"),
            OnNext(219, "1hat"),
            OnNext(300, "3wag"),
            OnNext(300, "3pig"),
            OnNext(305, "3cup"),
            OnNext(310, "4wag"),
            OnNext(310, "4pig"),
            OnNext(310, "4cup"),
            OnNext(702, "6tin"),
            OnNext(710, "7tin"),
            OnNext(712, "6man"),
            OnNext(712, "7man"),
            OnNext(720, "8tin"),
            OnNext(720, "8man"),
            OnError<string>(721, ex)
        );

        AssertDurations(xs, xsd, 721);
        AssertDurations(ys, ysd, 721);

        xs.Subscriptions.AssertEqual(
            Subscribe(200, 721)
        );

        ys.Subscriptions.AssertEqual(
            Subscribe(200, 721)
        );
    }

    [TestMethod]
    public void GroupJoinOp_Error_V()
    {
        var xs = Scheduler.CreateHotObservable(
            OnNext(210, new TimeInterval<int>(0, TimeSpan.FromTicks(10))),
            OnNext(219, new TimeInterval<int>(1, TimeSpan.FromTicks(5))),
            OnNext(240, new TimeInterval<int>(2, TimeSpan.FromTicks(10))),
            OnNext(300, new TimeInterval<int>(3, TimeSpan.FromTicks(100))),
            OnNext(310, new TimeInterval<int>(4, TimeSpan.FromTicks(80))),
            OnNext(500, new TimeInterval<int>(5, TimeSpan.FromTicks(90))),
            OnNext(700, new TimeInterval<int>(6, TimeSpan.FromTicks(25))),
            OnNext(710, new TimeInterval<int>(7, TimeSpan.FromTicks(300))),
            OnNext(720, new TimeInterval<int>(8, TimeSpan.FromTicks(100))),
            OnNext(830, new TimeInterval<int>(9, TimeSpan.FromTicks(10))),
            OnCompleted<TimeInterval<int>>(900)
        );

        var ys = Scheduler.CreateHotObservable(
            OnNext(215, new TimeInterval<string>("hat", TimeSpan.FromTicks(20))),
            OnNext(217, new TimeInterval<string>("bat", TimeSpan.FromTicks(1))),
            OnNext(290, new TimeInterval<string>("wag", TimeSpan.FromTicks(200))),
            OnNext(300, new TimeInterval<string>("pig", TimeSpan.FromTicks(10))),
            OnNext(305, new TimeInterval<string>("cup", TimeSpan.FromTicks(50))),
            OnNext(600, new TimeInterval<string>("yak", TimeSpan.FromTicks(90))),
            OnNext(702, new TimeInterval<string>("tin", TimeSpan.FromTicks(20))),
            OnNext(712, new TimeInterval<string>("man", TimeSpan.FromTicks(10))),
            OnNext(722, new TimeInterval<string>("rat", TimeSpan.FromTicks(200))),
            OnNext(732, new TimeInterval<string>("wig", TimeSpan.FromTicks(5))),
            OnCompleted<TimeInterval<string>>(800)
        );

        var ex = new Exception();

        var ysd = new List<TestableSeq<long>>();

        var res = Scheduler.Start(() =>
            xs.GroupJoin(ys, x => { if (x.Value >= 0) { throw ex; } return Observable.Empty<long>(); }, y => NewTimer(ysd, y.Interval, Scheduler), (x, yy) => yy.Select(y => x.Value + y.Value)).Merge()
        );

        res.Messages.AssertEqual(
            OnError<string>(210, ex)
        );

        AssertDurations(ys, ysd, 210);

        xs.Subscriptions.AssertEqual(
            Subscribe(200, 210)
        );

        ys.Subscriptions.AssertEqual(
            Subscribe(200, 210)
        );
    }

    [TestMethod]
    public void GroupJoinOp_Error_VI()
    {
        var xs = Scheduler.CreateHotObservable(
            OnNext(210, new TimeInterval<int>(0, TimeSpan.FromTicks(10))),
            OnNext(219, new TimeInterval<int>(1, TimeSpan.FromTicks(5))),
            OnNext(240, new TimeInterval<int>(2, TimeSpan.FromTicks(10))),
            OnNext(300, new TimeInterval<int>(3, TimeSpan.FromTicks(100))),
            OnNext(310, new TimeInterval<int>(4, TimeSpan.FromTicks(80))),
            OnNext(500, new TimeInterval<int>(5, TimeSpan.FromTicks(90))),
            OnNext(700, new TimeInterval<int>(6, TimeSpan.FromTicks(25))),
            OnNext(710, new TimeInterval<int>(7, TimeSpan.FromTicks(300))),
            OnNext(720, new TimeInterval<int>(8, TimeSpan.FromTicks(100))),
            OnNext(830, new TimeInterval<int>(9, TimeSpan.FromTicks(10))),
            OnCompleted<TimeInterval<int>>(900)
        );

        var ys = Scheduler.CreateHotObservable(
            OnNext(215, new TimeInterval<string>("hat", TimeSpan.FromTicks(20))),
            OnNext(217, new TimeInterval<string>("bat", TimeSpan.FromTicks(1))),
            OnNext(290, new TimeInterval<string>("wag", TimeSpan.FromTicks(200))),
            OnNext(300, new TimeInterval<string>("pig", TimeSpan.FromTicks(10))),
            OnNext(305, new TimeInterval<string>("cup", TimeSpan.FromTicks(50))),
            OnNext(600, new TimeInterval<string>("yak", TimeSpan.FromTicks(90))),
            OnNext(702, new TimeInterval<string>("tin", TimeSpan.FromTicks(20))),
            OnNext(712, new TimeInterval<string>("man", TimeSpan.FromTicks(10))),
            OnNext(722, new TimeInterval<string>("rat", TimeSpan.FromTicks(200))),
            OnNext(732, new TimeInterval<string>("wig", TimeSpan.FromTicks(5))),
            OnCompleted<TimeInterval<string>>(800)
        );

        var ex = new Exception();

        var xsd = new List<TestableSeq<long>>();

        var res = Scheduler.Start(() =>
            xs.GroupJoin(ys, x => NewTimer(xsd, x.Interval, Scheduler), y => { if (y.Value.Length >= 0) { throw ex; } return Observable.Empty<long>(); }, (x, yy) => yy.Select(y => x.Value + y.Value)).Merge()
        );

        res.Messages.AssertEqual(
            OnError<string>(215, ex)
        );

        AssertDurations(xs, xsd, 215);

        xs.Subscriptions.AssertEqual(
            Subscribe(200, 215)
        );

        ys.Subscriptions.AssertEqual(
            Subscribe(200, 215)
        );
    }

    [TestMethod]
    public void GroupJoinOp_Error_VII()
    {
        var xs = Scheduler.CreateHotObservable(
            OnNext(215, new TimeInterval<int>(0, TimeSpan.FromTicks(10))),
            OnNext(219, new TimeInterval<int>(1, TimeSpan.FromTicks(5))),
            OnNext(240, new TimeInterval<int>(2, TimeSpan.FromTicks(10))),
            OnNext(300, new TimeInterval<int>(3, TimeSpan.FromTicks(100))),
            OnNext(310, new TimeInterval<int>(4, TimeSpan.FromTicks(80))),
            OnNext(500, new TimeInterval<int>(5, TimeSpan.FromTicks(90))),
            OnNext(700, new TimeInterval<int>(6, TimeSpan.FromTicks(25))),
            OnNext(710, new TimeInterval<int>(7, TimeSpan.FromTicks(300))),
            OnNext(720, new TimeInterval<int>(8, TimeSpan.FromTicks(100))),
            OnNext(830, new TimeInterval<int>(9, TimeSpan.FromTicks(10))),
            OnCompleted<TimeInterval<int>>(900)
        );

        var ys = Scheduler.CreateHotObservable(
            OnNext(210, new TimeInterval<string>("hat", TimeSpan.FromTicks(20))),
            OnNext(217, new TimeInterval<string>("bat", TimeSpan.FromTicks(1))),
            OnNext(290, new TimeInterval<string>("wag", TimeSpan.FromTicks(200))),
            OnNext(300, new TimeInterval<string>("pig", TimeSpan.FromTicks(10))),
            OnNext(305, new TimeInterval<string>("cup", TimeSpan.FromTicks(50))),
            OnNext(600, new TimeInterval<string>("yak", TimeSpan.FromTicks(90))),
            OnNext(702, new TimeInterval<string>("tin", TimeSpan.FromTicks(20))),
            OnNext(712, new TimeInterval<string>("man", TimeSpan.FromTicks(10))),
            OnNext(722, new TimeInterval<string>("rat", TimeSpan.FromTicks(200))),
            OnNext(732, new TimeInterval<string>("wig", TimeSpan.FromTicks(5))),
            OnCompleted<TimeInterval<string>>(800)
        );

        var ex = new Exception();

        var xsd = new List<TestableSeq<long>>();
        var ysd = new List<TestableSeq<long>>();

        var res = Scheduler.Start(() =>
            xs.GroupJoin(ys, x => NewTimer(xsd, x.Interval, Scheduler), y => NewTimer(ysd, y.Interval, Scheduler), (x, yy) => { if (x.Value >= 0) { throw ex; } return yy.Select(y => x.Value + y.Value); }).Merge()
        );

        res.Messages.AssertEqual(
            OnError<string>(215, ex)
        );

        AssertDurations(xs, xsd, 215);
        AssertDurations(ys, ysd, 215);

        xs.Subscriptions.AssertEqual(
            Subscribe(200, 215)
        );

        ys.Subscriptions.AssertEqual(
            Subscribe(200, 215)
        );
    }

    [TestMethod]
    public void GroupJoinOp_Error_VIII()
    {
        var xs = Scheduler.CreateHotObservable(
            OnNext(210, new TimeInterval<int>(0, TimeSpan.FromTicks(10))),
            OnNext(219, new TimeInterval<int>(1, TimeSpan.FromTicks(5))),
            OnNext(240, new TimeInterval<int>(2, TimeSpan.FromTicks(10))),
            OnNext(300, new TimeInterval<int>(3, TimeSpan.FromTicks(100))),
            OnNext(310, new TimeInterval<int>(4, TimeSpan.FromTicks(80))),
            OnNext(500, new TimeInterval<int>(5, TimeSpan.FromTicks(90))),
            OnNext(700, new TimeInterval<int>(6, TimeSpan.FromTicks(25))),
            OnNext(710, new TimeInterval<int>(7, TimeSpan.FromTicks(300))),
            OnNext(720, new TimeInterval<int>(8, TimeSpan.FromTicks(100))),
            OnNext(830, new TimeInterval<int>(9, TimeSpan.FromTicks(10))),
            OnCompleted<TimeInterval<int>>(900)
        );

        var ys = Scheduler.CreateHotObservable(
            OnNext(215, new TimeInterval<string>("hat", TimeSpan.FromTicks(20))),
            OnNext(217, new TimeInterval<string>("bat", TimeSpan.FromTicks(1))),
            OnNext(290, new TimeInterval<string>("wag", TimeSpan.FromTicks(200))),
            OnNext(300, new TimeInterval<string>("pig", TimeSpan.FromTicks(10))),
            OnNext(305, new TimeInterval<string>("cup", TimeSpan.FromTicks(50))),
            OnNext(600, new TimeInterval<string>("yak", TimeSpan.FromTicks(90))),
            OnNext(702, new TimeInterval<string>("tin", TimeSpan.FromTicks(20))),
            OnNext(712, new TimeInterval<string>("man", TimeSpan.FromTicks(10))),
            OnNext(722, new TimeInterval<string>("rat", TimeSpan.FromTicks(200))),
            OnNext(732, new TimeInterval<string>("wig", TimeSpan.FromTicks(5))),
            OnCompleted<TimeInterval<string>>(800)
        );

        var ex = new Exception();

        var xsd = new List<TestableSeq<long>>();
        var ysd = new List<TestableSeq<long>>();

        var res = Scheduler.Start(() =>
            xs.GroupJoin(ys, x => NewTimer(xsd, x.Interval, Scheduler), y => NewTimer(ysd, y.Interval, Scheduler), (x, yy) => { if (x.Value >= 0) { throw ex; } return yy.Select(y => x.Value + y.Value); }).Merge()
        );

        res.Messages.AssertEqual(
            OnError<string>(210, ex)
        );

        AssertDurations(xs, xsd, 210);
        AssertDurations(ys, ysd, 210);

        xs.Subscriptions.AssertEqual(
            Subscribe(200, 210)
        );

        ys.Subscriptions.AssertEqual(
            Subscribe(200, 210)
        );
    }

    private static TestableSeq<long> NewTimer(List<TestableSeq<long>> l, TimeSpan t, TestScheduler scheduler)
    {
        var timer = scheduler.CreateColdObservable(OnNext(t.Ticks, 0L), OnCompleted<long>(t.Ticks));
        l.Add(timer);
        return timer;
    }

    private static void AssertDurations<T, U>(TestableSeq<TimeInterval<T>> xs, List<TestableSeq<U>> xsd, long lastEnd)
    {
        Assert.AreEqual(xs.Messages.Where(x => x.Value.Kind == NotificationKind.OnNext && x.Time <= lastEnd).Count(), xsd.Count);

        foreach (var pair in xs.Messages.Zip(xsd, (x, y) => new { Item1 = x, Item2 = y }))
        {
            var start = pair.Item1.Time;
            var end = Math.Min(start + pair.Item1.Value.Value.Interval.Ticks, lastEnd);
            pair.Item2.Subscriptions.AssertEqual(
                Subscribe(start, end)
            );
        }
    }
}

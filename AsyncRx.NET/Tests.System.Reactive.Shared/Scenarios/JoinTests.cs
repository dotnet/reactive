// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information. 

using System.Reactive;

namespace Tests.System.Reactive.Shared.Scenarios;

/// <summary>Shared <c>Join</c> scenarios, from Rx.NET's <c>JoinTest.cs</c>.</summary>
/// <remarks>
/// 15 of the file's 16 tests (<c>JoinOp_ArgumentChecking</c> is the code-generated stratum),
/// transcribed as the <c>GroupJoin</c> scenarios were: the <c>#if !NO_PERF</c> blocks resolved
/// to current behaviour (the more aggressive disposal of Rx.NET v2), the duration timers as
/// cold testable sources the <c>NewTimer</c> helper records, and <c>AssertDurations</c> checking
/// each timer's subscription window against its element's interval. The duration selectors
/// return descriptions, which the bridge adapts to return the target's own sequences.
/// </remarks>
public abstract class JoinTests : SharedReactiveTest
{
    [TestMethod]
    public void JoinOp_Normal_I()
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
            xs.Join(ys, x => NewTimer(xsd, x.Interval, Scheduler), y => NewTimer(ysd, y.Interval, Scheduler), (x, y) => x.Value + y.Value)
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
            OnCompleted<string>(900)
        );

        xs.Subscriptions.AssertEqual(
            Subscribe(200, 900)
        );

        ys.Subscriptions.AssertEqual(
            Subscribe(200, 800)
        );

        AssertDurations(xs, xsd, 900);
        AssertDurations(ys, ysd, 900);
    }

    [TestMethod]
    public void JoinOp_Normal_II()
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
            xs.Join(ys, x => NewTimer(xsd, x.Interval, Scheduler), y => NewTimer(ysd, y.Interval, Scheduler), (x, y) => x.Value + y.Value)
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

        xs.Subscriptions.AssertEqual(
            Subscribe(200, 721)
        );

        ys.Subscriptions.AssertEqual(
            Subscribe(200, 910)
        );

        AssertDurations(xs, xsd, 910);
        AssertDurations(ys, ysd, 910);
    }

    [TestMethod]
    public void JoinOp_Normal_III()
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
            xs.Join(ys, x => NewTimer(xsd, x.Interval, Scheduler).Where(_ => false), y => NewTimer(ysd, y.Interval, Scheduler).Where(_ => false), (x, y) => x.Value + y.Value)
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
            OnCompleted<string>(900)
        );

        xs.Subscriptions.AssertEqual(
            Subscribe(200, 900)
        );

        ys.Subscriptions.AssertEqual(
            Subscribe(200, 800)
        );

        AssertDurations(xs, xsd, 900);
        AssertDurations(ys, ysd, 900);
    }

    [TestMethod]
    public void JoinOp_Normal_IV()
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
            xs.Join(ys, x => NewTimer(xsd, x.Interval, Scheduler), y => NewTimer(ysd, y.Interval, Scheduler), (x, y) => x.Value + y.Value)
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
            OnCompleted<string>(980)
        );

        xs.Subscriptions.AssertEqual(
            Subscribe(200, 980)
        );

        ys.Subscriptions.AssertEqual(
            Subscribe(200, 980)
        );

        AssertDurations(xs, xsd, 980);
        AssertDurations(ys, ysd, 980);
    }

    [TestMethod]
    public void JoinOp_Normal_V()
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
            xs.Join(ys, x => NewTimer(xsd, x.Interval, Scheduler), y => NewTimer(ysd, y.Interval, Scheduler), (x, y) => x.Value + y.Value)
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
            OnCompleted<string>(922)
        );

        xs.Subscriptions.AssertEqual(
            Subscribe(200, 922)
        );

        ys.Subscriptions.AssertEqual(
            Subscribe(200, 900)
        );

        AssertDurations(xs, xsd, 922);
        AssertDurations(ys, ysd, 922);
    }

    [TestMethod]
    public void JoinOp_Normal_VI()
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
            xs.Join(ys, x => NewTimer(xsd, x.Interval, Scheduler), y => NewTimer(ysd, y.Interval, Scheduler), (x, y) => x.Value + y.Value)
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
            OnCompleted<string>(900)
        );

        xs.Subscriptions.AssertEqual(
            Subscribe(200, 850)
        );

        ys.Subscriptions.AssertEqual(
            Subscribe(200, 900)
        );

        AssertDurations(xs, xsd, 900);
        AssertDurations(ys, ysd, 900);
    }

    [TestMethod]
    public void JoinOp_Normal_VII()
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
            xs.Join(ys, x => NewTimer(xsd, x.Interval, Scheduler), y => NewTimer(ysd, y.Interval, Scheduler), (x, y) => x.Value + y.Value),
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

        xs.Subscriptions.AssertEqual(
            Subscribe(200, 713)
        );

        ys.Subscriptions.AssertEqual(
            Subscribe(200, 713)
        );

        AssertDurations(xs, xsd, 713);
        AssertDurations(ys, ysd, 713);
    }

    [TestMethod]
    public void JoinOp_Error_I()
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
            xs.Join(ys, x => NewTimer(xsd, x.Interval, Scheduler), y => NewTimer(ysd, y.Interval, Scheduler), (x, y) => x.Value + y.Value)
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

        xs.Subscriptions.AssertEqual(
            Subscribe(200, 310)
        );

        ys.Subscriptions.AssertEqual(
            Subscribe(200, 310)
        );

        AssertDurations(xs, xsd, 310);
        AssertDurations(ys, ysd, 310);
    }

    [TestMethod]
    public void JoinOp_Error_II()
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
            xs.Join(ys, x => NewTimer(xsd, x.Interval, Scheduler), y => NewTimer(ysd, y.Interval, Scheduler), (x, y) => x.Value + y.Value)
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

        xs.Subscriptions.AssertEqual(
            Subscribe(200, 722)
        );

        ys.Subscriptions.AssertEqual(
            Subscribe(200, 722)
        );

        AssertDurations(xs, xsd, 722);
        AssertDurations(ys, ysd, 722);
    }

    [TestMethod]
    public void JoinOp_Error_III()
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
            xs.Join(ys, x => NewTimer(xsd, x.Interval, Scheduler).SelectMany(x.Value == 6 ? Seq.Throw<long>(ex) : Seq.Empty<long>()), y => NewTimer(ysd, y.Interval, Scheduler), (x, y) => x.Value + y.Value)
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

        xs.Subscriptions.AssertEqual(
            Subscribe(200, 725)
        );

        ys.Subscriptions.AssertEqual(
            Subscribe(200, 725)
        );

        AssertDurations(xs, xsd, 725);
        AssertDurations(ys, ysd, 725);
    }

    [TestMethod]
    public void JoinOp_Error_IV()
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
            xs.Join(ys, x => NewTimer(xsd, x.Interval, Scheduler), y => NewTimer(ysd, y.Interval, Scheduler).SelectMany(y.Value == "tin" ? Seq.Throw<long>(ex) : Seq.Empty<long>()), (x, y) => x.Value + y.Value)
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

        xs.Subscriptions.AssertEqual(
            Subscribe(200, 721)
        );

        ys.Subscriptions.AssertEqual(
            Subscribe(200, 721)
        );

        AssertDurations(xs, xsd, 721);
        AssertDurations(ys, ysd, 721);
    }

    [TestMethod]
    public void JoinOp_Error_V()
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
            xs.Join(ys, x => { if (x.Value >= 0) { throw ex; } return Seq.Empty<long>(); }, y => NewTimer(ysd, y.Interval, Scheduler), (x, y) => x.Value + y.Value)
        );

        res.Messages.AssertEqual(
            OnError<string>(210, ex)
        );

        xs.Subscriptions.AssertEqual(
            Subscribe(200, 210)
        );

        ys.Subscriptions.AssertEqual(
            Subscribe(200, 210)
        );

        AssertDurations(ys, ysd, 210);
    }

    [TestMethod]
    public void JoinOp_Error_VI()
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
            xs.Join(ys, x => NewTimer(xsd, x.Interval, Scheduler), y => { if (y.Value.Length >= 0) { throw ex; } return Seq.Empty<long>(); }, (x, y) => x.Value + y.Value)
        );

        res.Messages.AssertEqual(
            OnError<string>(215, ex)
        );

        xs.Subscriptions.AssertEqual(
            Subscribe(200, 215)
        );

        ys.Subscriptions.AssertEqual(
            Subscribe(200, 215)
        );

        AssertDurations(xs, xsd, 215);
    }

    [TestMethod]
    public void JoinOp_Error_VII()
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
            xs.Join(ys, x => NewTimer(xsd, x.Interval, Scheduler), y => NewTimer(ysd, y.Interval, Scheduler), (x, y) => { if (x.Value >= 0) { throw ex; } return x.Value + y.Value; })
        );

        res.Messages.AssertEqual(
            OnError<string>(215, ex)
        );

        xs.Subscriptions.AssertEqual(
            Subscribe(200, 215)
        );

        ys.Subscriptions.AssertEqual(
            Subscribe(200, 215)
        );

        AssertDurations(xs, xsd, 215);
        AssertDurations(ys, ysd, 215);
    }

    [TestMethod]
    public void JoinOp_Error_VIII()
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
            xs.Join(ys, x => NewTimer(xsd, x.Interval, Scheduler), y => NewTimer(ysd, y.Interval, Scheduler), (x, y) => { if (x.Value >= 0) { throw ex; } return x.Value + y.Value; })
        );

        res.Messages.AssertEqual(
            OnError<string>(215, ex)
        );

        xs.Subscriptions.AssertEqual(
            Subscribe(200, 215)
        );

        ys.Subscriptions.AssertEqual(
            Subscribe(200, 215)
        );

        AssertDurations(xs, xsd, 215);
        AssertDurations(ys, ysd, 215);
    }

    private static TestableSeq<long> NewTimer(List<TestableSeq<long>> l, TimeSpan t, TestSchedulerRef scheduler)
    {
        var timer = scheduler.CreateColdObservable(OnNext(t.Ticks, 0L), OnCompleted<long>(t.Ticks));
        l.Add(timer);
        return timer;
    }

    private static void AssertDurations<T, U>(TestableSeq<TimeInterval<T>> xs, List<TestableSeq<U>> xsd, long lastEnd)
    {
        Assert.HasCount(xs.Messages.Where(x => x.Value.Kind == NotificationKind.OnNext && x.Time <= lastEnd).Count(), xsd);

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

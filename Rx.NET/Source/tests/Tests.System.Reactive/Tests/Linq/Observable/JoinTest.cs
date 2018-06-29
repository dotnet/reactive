// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the Apache 2.0 License.
// See the LICENSE file in the project root for more information. 

using System;
using System.Collections.Generic;
using System.Linq;
using System.Reactive;
using System.Reactive.Linq;
using Microsoft.Reactive.Testing;
using ReactiveTests.Dummies;
using Xunit;

namespace ReactiveTests.Tests
{
    public class JoinTest : ReactiveTest
    {

        [Fact]
        public void JoinOp_ArgumentChecking()
        {
            ReactiveAssert.Throws<ArgumentNullException>(() => Observable.Join(null, DummyObservable<int>.Instance, DummyFunc<int, IObservable<int>>.Instance, DummyFunc<int, IObservable<int>>.Instance, DummyFunc<int, int, int>.Instance));
            ReactiveAssert.Throws<ArgumentNullException>(() => Observable.Join(DummyObservable<int>.Instance, null, DummyFunc<int, IObservable<int>>.Instance, DummyFunc<int, IObservable<int>>.Instance, DummyFunc<int, int, int>.Instance));
            ReactiveAssert.Throws<ArgumentNullException>(() => Observable.Join(DummyObservable<int>.Instance, DummyObservable<int>.Instance, default(Func<int, IObservable<int>>), DummyFunc<int, IObservable<int>>.Instance, DummyFunc<int, int, int>.Instance));
            ReactiveAssert.Throws<ArgumentNullException>(() => Observable.Join(DummyObservable<int>.Instance, DummyObservable<int>.Instance, DummyFunc<int, IObservable<int>>.Instance, default(Func<int, IObservable<int>>), DummyFunc<int, int, int>.Instance));
            ReactiveAssert.Throws<ArgumentNullException>(() => Observable.Join(DummyObservable<int>.Instance, DummyObservable<int>.Instance, DummyFunc<int, IObservable<int>>.Instance, DummyFunc<int, IObservable<int>>.Instance, default(Func<int, int, int>)));
            ReactiveAssert.Throws<ArgumentNullException>(() => Observable.Join(DummyObservable<int>.Instance, DummyObservable<int>.Instance, DummyFunc<int, IObservable<int>>.Instance, DummyFunc<int, IObservable<int>>.Instance, DummyFunc<int, int, int>.Instance).Subscribe(null));
        }

        [Fact]
        public void JoinOp_Normal_I()
        {
            var scheduler = new TestScheduler();

            var xs = scheduler.CreateHotObservable(
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

            var ys = scheduler.CreateHotObservable(
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

            var xsd = new List<ITestableObservable<long>>();
            var ysd = new List<ITestableObservable<long>>();

            var res = scheduler.Start(() =>
                xs.Join(ys, x => NewTimer(xsd, x.Interval, scheduler), y => NewTimer(ysd, y.Interval, scheduler), (x, y) => x.Value + y.Value)
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

#if !NO_PERF // BREAKING CHANGE v2 > v1.x -> More aggressive disposal behavior
            ys.Subscriptions.AssertEqual(
                Subscribe(200, 800)
            );
#else
            ys.Subscriptions.AssertEqual(
                Subscribe(200, 900)
            );
#endif

            AssertDurations(xs, xsd, 900);
            AssertDurations(ys, ysd, 900);
        }

        [Fact]
        public void JoinOp_Normal_II()
        {
            var scheduler = new TestScheduler();

            var xs = scheduler.CreateHotObservable(
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

            var ys = scheduler.CreateHotObservable(
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

            var xsd = new List<ITestableObservable<long>>();
            var ysd = new List<ITestableObservable<long>>();

            var res = scheduler.Start(() =>
                xs.Join(ys, x => NewTimer(xsd, x.Interval, scheduler), y => NewTimer(ysd, y.Interval, scheduler), (x, y) => x.Value + y.Value)
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

#if !NO_PERF // BREAKING CHANGE v2 > v1.x -> More aggressive disposal behavior
            xs.Subscriptions.AssertEqual(
                Subscribe(200, 721)
            );
#else
            xs.Subscriptions.AssertEqual(
                Subscribe(200, 910)
            );
#endif

            ys.Subscriptions.AssertEqual(
                Subscribe(200, 910)
            );

            AssertDurations(xs, xsd, 910);
            AssertDurations(ys, ysd, 910);
        }

        [Fact]
        public void JoinOp_Normal_III()
        {
            var scheduler = new TestScheduler();

            var xs = scheduler.CreateHotObservable(
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

            var ys = scheduler.CreateHotObservable(
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

            var xsd = new List<ITestableObservable<long>>();
            var ysd = new List<ITestableObservable<long>>();

            var res = scheduler.Start(() =>
                xs.Join(ys, x => NewTimer(xsd, x.Interval, scheduler).Where(_ => false), y => NewTimer(ysd, y.Interval, scheduler).Where(_ => false), (x, y) => x.Value + y.Value)
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

#if !NO_PERF // BREAKING CHANGE v2 > v1.x -> More aggressive disposal behavior
            ys.Subscriptions.AssertEqual(
                Subscribe(200, 800)
            );
#else
            ys.Subscriptions.AssertEqual(
                Subscribe(200, 900)
            );
#endif

            AssertDurations(xs, xsd, 900);
            AssertDurations(ys, ysd, 900);
        }

        [Fact]
        public void JoinOp_Normal_IV()
        {
            var scheduler = new TestScheduler();

            var xs = scheduler.CreateHotObservable(
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

            var ys = scheduler.CreateHotObservable(
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

            var xsd = new List<ITestableObservable<long>>();
            var ysd = new List<ITestableObservable<long>>();

            var res = scheduler.Start(() =>
                xs.Join(ys, x => NewTimer(xsd, x.Interval, scheduler), y => NewTimer(ysd, y.Interval, scheduler), (x, y) => x.Value + y.Value)
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

        [Fact]
        public void JoinOp_Normal_V()
        {
            var scheduler = new TestScheduler();

            var xs = scheduler.CreateHotObservable(
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

            var ys = scheduler.CreateHotObservable(
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

            var xsd = new List<ITestableObservable<long>>();
            var ysd = new List<ITestableObservable<long>>();

            var res = scheduler.Start(() =>
                xs.Join(ys, x => NewTimer(xsd, x.Interval, scheduler), y => NewTimer(ysd, y.Interval, scheduler), (x, y) => x.Value + y.Value)
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

#if !NO_PERF // BREAKING CHANGE v2 > v1.x -> More aggressive disposal behavior
            ys.Subscriptions.AssertEqual(
                Subscribe(200, 900)
            );
#else
            ys.Subscriptions.AssertEqual(
                Subscribe(200, 922)
            );
#endif

            AssertDurations(xs, xsd, 922);
            AssertDurations(ys, ysd, 922);
        }

        [Fact]
        public void JoinOp_Normal_VI()
        {
            var scheduler = new TestScheduler();

            var xs = scheduler.CreateHotObservable(
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

            var ys = scheduler.CreateHotObservable(
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

            var xsd = new List<ITestableObservable<long>>();
            var ysd = new List<ITestableObservable<long>>();

            var res = scheduler.Start(() =>
                xs.Join(ys, x => NewTimer(xsd, x.Interval, scheduler), y => NewTimer(ysd, y.Interval, scheduler), (x, y) => x.Value + y.Value)
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

#if !NO_PERF // BREAKING CHANGE v2 > v1.x -> More aggressive disposal behavior
            xs.Subscriptions.AssertEqual(
                Subscribe(200, 850)
            );
#else
            xs.Subscriptions.AssertEqual(
                Subscribe(200, 900)
            );
#endif

            ys.Subscriptions.AssertEqual(
                Subscribe(200, 900)
            );

            AssertDurations(xs, xsd, 900);
            AssertDurations(ys, ysd, 900);
        }

        [Fact]
        public void JoinOp_Normal_VII()
        {
            var scheduler = new TestScheduler();

            var xs = scheduler.CreateHotObservable(
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

            var ys = scheduler.CreateHotObservable(
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

            var xsd = new List<ITestableObservable<long>>();
            var ysd = new List<ITestableObservable<long>>();

            var res = scheduler.Start(() =>
                xs.Join(ys, x => NewTimer(xsd, x.Interval, scheduler), y => NewTimer(ysd, y.Interval, scheduler), (x, y) => x.Value + y.Value),
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

        [Fact]
        public void JoinOp_Error_I()
        {
            var scheduler = new TestScheduler();

            var ex = new Exception();

            var xs = scheduler.CreateHotObservable(
                OnNext(210, new TimeInterval<int>(0, TimeSpan.FromTicks(10))),
                OnNext(219, new TimeInterval<int>(1, TimeSpan.FromTicks(5))),
                OnNext(240, new TimeInterval<int>(2, TimeSpan.FromTicks(10))),
                OnNext(300, new TimeInterval<int>(3, TimeSpan.FromTicks(100))),
                OnError<TimeInterval<int>>(310, ex)
            );

            var ys = scheduler.CreateHotObservable(
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

            var xsd = new List<ITestableObservable<long>>();
            var ysd = new List<ITestableObservable<long>>();

            var res = scheduler.Start(() =>
                xs.Join(ys, x => NewTimer(xsd, x.Interval, scheduler), y => NewTimer(ysd, y.Interval, scheduler), (x, y) => x.Value + y.Value)
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

        [Fact]
        public void JoinOp_Error_II()
        {
            var scheduler = new TestScheduler();

            var ex = new Exception();

            var xs = scheduler.CreateHotObservable(
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

            var ys = scheduler.CreateHotObservable(
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

            var xsd = new List<ITestableObservable<long>>();
            var ysd = new List<ITestableObservable<long>>();

            var res = scheduler.Start(() =>
                xs.Join(ys, x => NewTimer(xsd, x.Interval, scheduler), y => NewTimer(ysd, y.Interval, scheduler), (x, y) => x.Value + y.Value)
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

        [Fact]
        public void JoinOp_Error_III()
        {
            var scheduler = new TestScheduler();

            var xs = scheduler.CreateHotObservable(
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

            var ys = scheduler.CreateHotObservable(
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

            var xsd = new List<ITestableObservable<long>>();
            var ysd = new List<ITestableObservable<long>>();

            var res = scheduler.Start(() =>
                xs.Join(ys, x => NewTimer(xsd, x.Interval, scheduler).SelectMany(x.Value == 6 ? Observable.Throw<long>(ex) : Observable.Empty<long>()), y => NewTimer(ysd, y.Interval, scheduler), (x, y) => x.Value + y.Value)
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

        [Fact]
        public void JoinOp_Error_IV()
        {
            var scheduler = new TestScheduler();

            var xs = scheduler.CreateHotObservable(
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

            var ys = scheduler.CreateHotObservable(
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

            var xsd = new List<ITestableObservable<long>>();
            var ysd = new List<ITestableObservable<long>>();

            var res = scheduler.Start(() =>
                xs.Join(ys, x => NewTimer(xsd, x.Interval, scheduler), y => NewTimer(ysd, y.Interval, scheduler).SelectMany(y.Value == "tin" ? Observable.Throw<long>(ex) : Observable.Empty<long>()), (x, y) => x.Value + y.Value)
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

        [Fact]
        public void JoinOp_Error_V()
        {
            var scheduler = new TestScheduler();

            var xs = scheduler.CreateHotObservable(
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

            var ys = scheduler.CreateHotObservable(
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

            var ysd = new List<ITestableObservable<long>>();

            var res = scheduler.Start(() =>
                xs.Join(ys, x => { if (x.Value >= 0) { throw ex; } return Observable.Empty<long>(); }, y => NewTimer(ysd, y.Interval, scheduler), (x, y) => x.Value + y.Value)
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

        [Fact]
        public void JoinOp_Error_VI()
        {
            var scheduler = new TestScheduler();

            var xs = scheduler.CreateHotObservable(
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

            var ys = scheduler.CreateHotObservable(
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

            var xsd = new List<ITestableObservable<long>>();

            var res = scheduler.Start(() =>
                xs.Join(ys, x => NewTimer(xsd, x.Interval, scheduler), y => { if (y.Value.Length >= 0) { throw ex; } return Observable.Empty<long>(); }, (x, y) => x.Value + y.Value)
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

        [Fact]
        public void JoinOp_Error_VII()
        {
            var scheduler = new TestScheduler();

            var xs = scheduler.CreateHotObservable(
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

            var ys = scheduler.CreateHotObservable(
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

            var xsd = new List<ITestableObservable<long>>();
            var ysd = new List<ITestableObservable<long>>();

            var res = scheduler.Start(() =>
                xs.Join(ys, x => NewTimer(xsd, x.Interval, scheduler), y => NewTimer(ysd, y.Interval, scheduler), (x, y) => { if (x.Value >= 0) { throw ex; } return x.Value + y.Value; })
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

        [Fact]
        public void JoinOp_Error_VIII()
        {
            var scheduler = new TestScheduler();

            var xs = scheduler.CreateHotObservable(
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

            var ys = scheduler.CreateHotObservable(
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

            var xsd = new List<ITestableObservable<long>>();
            var ysd = new List<ITestableObservable<long>>();

            var res = scheduler.Start(() =>
                xs.Join(ys, x => NewTimer(xsd, x.Interval, scheduler), y => NewTimer(ysd, y.Interval, scheduler), (x, y) => { if (x.Value >= 0) { throw ex; } return x.Value + y.Value; })
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

        private ITestableObservable<long> NewTimer(List<ITestableObservable<long>> l, TimeSpan t, TestScheduler scheduler)
        {
            var timer = scheduler.CreateColdObservable(OnNext(t.Ticks, 0L), OnCompleted<long>(t.Ticks));
            l.Add(timer);
            return timer;
        }

        private void AssertDurations<T, U>(ITestableObservable<TimeInterval<T>> xs, List<ITestableObservable<U>> xsd, long lastEnd)
        {
            Assert.Equal(xs.Messages.Where(x => x.Value.Kind == NotificationKind.OnNext && x.Time <= lastEnd).Count(), xsd.Count);

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
}

// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the Apache 2.0 License.
// See the LICENSE file in the project root for more information. 

using System;
using System.Collections.Generic;
using System.Linq;
using System.Reactive.Linq;
using System.Reactive.Subjects;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Reactive.Testing;
using ReactiveTests.Dummies;
using Xunit;

namespace ReactiveTests.Tests
{
    public class SelectManyTest : ReactiveTest
    {

        [Fact]
        public void SelectMany_Then_ArgumentChecking()
        {
            ReactiveAssert.Throws<ArgumentNullException>(() => ((IObservable<int>)null).SelectMany(DummyObservable<string>.Instance));
            ReactiveAssert.Throws<ArgumentNullException>(() => DummyObservable<int>.Instance.SelectMany(((IObservable<string>)null)));
            ReactiveAssert.Throws<ArgumentNullException>(() => DummyObservable<int>.Instance.SelectMany(DummyObservable<string>.Instance).Subscribe(null));
        }

        [Fact]
        public void SelectMany_Then_Complete_Complete()
        {
            var scheduler = new TestScheduler();

            var xs = scheduler.CreateColdObservable(
                OnNext(100, 4),
                OnNext(200, 2),
                OnNext(300, 3),
                OnNext(400, 1),
                OnCompleted<int>(500)
            );

            var ys = scheduler.CreateColdObservable(
                OnNext(50, "foo"),
                OnNext(100, "bar"),
                OnNext(150, "baz"),
                OnNext(200, "qux"),
                OnCompleted<string>(250)
            );

            var res = scheduler.Start(() =>
                xs.SelectMany(ys)
            );

            res.Messages.AssertEqual(
                OnNext(350, "foo"),
                OnNext(400, "bar"),
                OnNext(450, "baz"),
                OnNext(450, "foo"),
                OnNext(500, "qux"),
                OnNext(500, "bar"),
                OnNext(550, "baz"),
                OnNext(550, "foo"),
                OnNext(600, "qux"),
                OnNext(600, "bar"),
                OnNext(650, "baz"),
                OnNext(650, "foo"),
                OnNext(700, "qux"),
                OnNext(700, "bar"),
                OnNext(750, "baz"),
                OnNext(800, "qux"),
                OnCompleted<string>(850)
            );

            xs.Subscriptions.AssertEqual(
                Subscribe(200, 700)
            );

            ys.Subscriptions.AssertEqual(
                Subscribe(300, 550),
                Subscribe(400, 650),
                Subscribe(500, 750),
                Subscribe(600, 850)
            );
        }

        [Fact]
        public void SelectMany_Then_Complete_Complete_2()
        {
            var scheduler = new TestScheduler();

            var xs = scheduler.CreateColdObservable(
                OnNext(100, 4),
                OnNext(200, 2),
                OnNext(300, 3),
                OnNext(400, 1),
                OnCompleted<int>(700)
            );

            var ys = scheduler.CreateColdObservable(
                OnNext(50, "foo"),
                OnNext(100, "bar"),
                OnNext(150, "baz"),
                OnNext(200, "qux"),
                OnCompleted<string>(250)
            );

            var res = scheduler.Start(() =>
                xs.SelectMany(ys)
            );

            res.Messages.AssertEqual(
                OnNext(350, "foo"),
                OnNext(400, "bar"),
                OnNext(450, "baz"),
                OnNext(450, "foo"),
                OnNext(500, "qux"),
                OnNext(500, "bar"),
                OnNext(550, "baz"),
                OnNext(550, "foo"),
                OnNext(600, "qux"),
                OnNext(600, "bar"),
                OnNext(650, "baz"),
                OnNext(650, "foo"),
                OnNext(700, "qux"),
                OnNext(700, "bar"),
                OnNext(750, "baz"),
                OnNext(800, "qux"),
                OnCompleted<string>(900)
            );

            xs.Subscriptions.AssertEqual(
                Subscribe(200, 900)
            );

            ys.Subscriptions.AssertEqual(
                Subscribe(300, 550),
                Subscribe(400, 650),
                Subscribe(500, 750),
                Subscribe(600, 850)
            );
        }

        [Fact]
        public void SelectMany_Then_Never_Complete()
        {
            var scheduler = new TestScheduler();

            var xs = scheduler.CreateColdObservable(
                OnNext(100, 4),
                OnNext(200, 2),
                OnNext(300, 3),
                OnNext(400, 1),
                OnNext(500, 5),
                OnNext(700, 0)
            );

            var ys = scheduler.CreateColdObservable(
                OnNext(50, "foo"),
                OnNext(100, "bar"),
                OnNext(150, "baz"),
                OnNext(200, "qux"),
                OnCompleted<string>(250)
            );

            var res = scheduler.Start(() =>
                xs.SelectMany(ys)
            );

            res.Messages.AssertEqual(
                OnNext(350, "foo"),
                OnNext(400, "bar"),
                OnNext(450, "baz"),
                OnNext(450, "foo"),
                OnNext(500, "qux"),
                OnNext(500, "bar"),
                OnNext(550, "baz"),
                OnNext(550, "foo"),
                OnNext(600, "qux"),
                OnNext(600, "bar"),
                OnNext(650, "baz"),
                OnNext(650, "foo"),
                OnNext(700, "qux"),
                OnNext(700, "bar"),
                OnNext(750, "baz"),
                OnNext(750, "foo"),
                OnNext(800, "qux"),
                OnNext(800, "bar"),
                OnNext(850, "baz"),
                OnNext(900, "qux"),
                OnNext(950, "foo")
            );

            xs.Subscriptions.AssertEqual(
                Subscribe(200, 1000)
            );

            ys.Subscriptions.AssertEqual(
                Subscribe(300, 550),
                Subscribe(400, 650),
                Subscribe(500, 750),
                Subscribe(600, 850),
                Subscribe(700, 950),
                Subscribe(900, 1000)
            );
        }

        [Fact]
        public void SelectMany_Then_Complete_Never()
        {
            var scheduler = new TestScheduler();

            var xs = scheduler.CreateColdObservable(
                OnNext(100, 4),
                OnNext(200, 2),
                OnNext(300, 3),
                OnNext(400, 1),
                OnCompleted<int>(500)
            );

            var ys = scheduler.CreateColdObservable(
                OnNext(50, "foo"),
                OnNext(100, "bar"),
                OnNext(150, "baz"),
                OnNext(200, "qux")
            );

            var res = scheduler.Start(() =>
                xs.SelectMany(ys)
            );

            res.Messages.AssertEqual(
                OnNext(350, "foo"),
                OnNext(400, "bar"),
                OnNext(450, "baz"),
                OnNext(450, "foo"),
                OnNext(500, "qux"),
                OnNext(500, "bar"),
                OnNext(550, "baz"),
                OnNext(550, "foo"),
                OnNext(600, "qux"),
                OnNext(600, "bar"),
                OnNext(650, "baz"),
                OnNext(650, "foo"),
                OnNext(700, "qux"),
                OnNext(700, "bar"),
                OnNext(750, "baz"),
                OnNext(800, "qux")
            );

            xs.Subscriptions.AssertEqual(
                Subscribe(200, 700)
            );

            ys.Subscriptions.AssertEqual(
                Subscribe(300, 1000),
                Subscribe(400, 1000),
                Subscribe(500, 1000),
                Subscribe(600, 1000)
            );
        }

        [Fact]
        public void SelectMany_Then_Complete_Error()
        {
            var scheduler = new TestScheduler();

            var ex = new Exception();

            var xs = scheduler.CreateColdObservable(
                OnNext(100, 4),
                OnNext(200, 2),
                OnNext(300, 3),
                OnNext(400, 1),
                OnCompleted<int>(500)
            );

            var ys = scheduler.CreateColdObservable(
                OnNext(50, "foo"),
                OnNext(100, "bar"),
                OnNext(150, "baz"),
                OnNext(200, "qux"),
                OnError<string>(300, ex)
            );

            var res = scheduler.Start(() =>
                xs.SelectMany(ys)
            );

            res.Messages.AssertEqual(
                OnNext(350, "foo"),
                OnNext(400, "bar"),
                OnNext(450, "baz"),
                OnNext(450, "foo"),
                OnNext(500, "qux"),
                OnNext(500, "bar"),
                OnNext(550, "baz"),
                OnNext(550, "foo"),
                OnError<string>(600, ex)
            );

            xs.Subscriptions.AssertEqual(
                Subscribe(200, 600)
            );

            ys.Subscriptions.AssertEqual(
                Subscribe(300, 600),
                Subscribe(400, 600),
                Subscribe(500, 600),
                Subscribe(600, 600)
            );
        }

        [Fact]
        public void SelectMany_Then_Error_Complete()
        {
            var scheduler = new TestScheduler();

            var ex = new Exception();

            var xs = scheduler.CreateColdObservable(
                OnNext(100, 4),
                OnNext(200, 2),
                OnNext(300, 3),
                OnNext(400, 1),
                OnError<int>(500, ex)
            );

            var ys = scheduler.CreateColdObservable(
                OnNext(50, "foo"),
                OnNext(100, "bar"),
                OnNext(150, "baz"),
                OnNext(200, "qux"),
                OnCompleted<string>(250)
            );

            var res = scheduler.Start(() =>
                xs.SelectMany(ys)
            );

            res.Messages.AssertEqual(
                OnNext(350, "foo"),
                OnNext(400, "bar"),
                OnNext(450, "baz"),
                OnNext(450, "foo"),
                OnNext(500, "qux"),
                OnNext(500, "bar"),
                OnNext(550, "baz"),
                OnNext(550, "foo"),
                OnNext(600, "qux"),
                OnNext(600, "bar"),
                OnNext(650, "baz"),
                OnNext(650, "foo"),
                OnError<string>(700, ex)
            );

            xs.Subscriptions.AssertEqual(
                Subscribe(200, 700)
            );

            ys.Subscriptions.AssertEqual(
                Subscribe(300, 550),
                Subscribe(400, 650),
                Subscribe(500, 700),
                Subscribe(600, 700)
            );
        }

        [Fact]
        public void SelectMany_Then_Error_Error()
        {
            var scheduler = new TestScheduler();

            var ex = new Exception();

            var xs = scheduler.CreateColdObservable(
                OnNext(100, 4),
                OnNext(200, 2),
                OnNext(300, 3),
                OnNext(400, 1),
                OnError<int>(500, new Exception())
            );

            var ys = scheduler.CreateColdObservable(
                OnNext(50, "foo"),
                OnNext(100, "bar"),
                OnNext(150, "baz"),
                OnNext(200, "qux"),
                OnError<string>(250, ex)
            );

            var res = scheduler.Start(() =>
                xs.SelectMany(ys)
            );

            res.Messages.AssertEqual(
                OnNext(350, "foo"),
                OnNext(400, "bar"),
                OnNext(450, "baz"),
                OnNext(450, "foo"),
                OnNext(500, "qux"),
                OnNext(500, "bar"),
                OnError<string>(550, ex)
            );

            xs.Subscriptions.AssertEqual(
                Subscribe(200, 550)
            );

            ys.Subscriptions.AssertEqual(
                Subscribe(300, 550),
                Subscribe(400, 550),
                Subscribe(500, 550)
            );
        }

        [Fact]
        public void SelectMany_ArgumentChecking()
        {
            ReactiveAssert.Throws<ArgumentNullException>(() => ((IObservable<int>)null).SelectMany(DummyFunc<int, IObservable<int>>.Instance));
            ReactiveAssert.Throws<ArgumentNullException>(() => DummyObservable<int>.Instance.SelectMany((Func<int, IObservable<int>>)null));
            ReactiveAssert.Throws<ArgumentNullException>(() => DummyObservable<int>.Instance.SelectMany(DummyFunc<int, IObservable<int>>.Instance).Subscribe(null));
        }

        [Fact]
        public void SelectMany_Complete()
        {
            var scheduler = new TestScheduler();

            var xs = scheduler.CreateHotObservable(
                    OnNext(5, scheduler.CreateColdObservable(
                        OnError<int>(1, new InvalidOperationException()))),
                    OnNext(105, scheduler.CreateColdObservable(
                        OnError<int>(1, new InvalidOperationException()))),
                    OnNext(300, scheduler.CreateColdObservable(
                        OnNext(10, 102),
                        OnNext(90, 103),
                        OnNext(110, 104),
                        OnNext(190, 105),
                        OnNext(440, 106),
                        OnCompleted<int>(460))),
                    OnNext(400, scheduler.CreateColdObservable(
                        OnNext(180, 202),
                        OnNext(190, 203),
                        OnCompleted<int>(205))),
                    OnNext(550, scheduler.CreateColdObservable(
                        OnNext(10, 301),
                        OnNext(50, 302),
                        OnNext(70, 303),
                        OnNext(260, 304),
                        OnNext(310, 305),
                        OnCompleted<int>(410))),
                    OnNext(750, scheduler.CreateColdObservable(
                        OnCompleted<int>(40))),
                    OnNext(850, scheduler.CreateColdObservable(
                        OnNext(80, 401),
                        OnNext(90, 402),
                        OnCompleted<int>(100))),
                    OnCompleted<ITestableObservable<int>>(900)
            );

            var res = scheduler.Start(() =>
                xs.SelectMany(x => x)
            );

            res.Messages.AssertEqual(
                OnNext(310, 102),
                OnNext(390, 103),
                OnNext(410, 104),
                OnNext(490, 105),
                OnNext(560, 301),
                OnNext(580, 202),
                OnNext(590, 203),
                OnNext(600, 302),
                OnNext(620, 303),
                OnNext(740, 106),
                OnNext(810, 304),
                OnNext(860, 305),
                OnNext(930, 401),
                OnNext(940, 402),
                OnCompleted<int>(960)
            );

            xs.Subscriptions.AssertEqual(
                Subscribe(200, 900));

            xs.Messages[2].Value.Value.Subscriptions.AssertEqual(
                Subscribe(300, 760));

            xs.Messages[3].Value.Value.Subscriptions.AssertEqual(
                Subscribe(400, 605));

            xs.Messages[4].Value.Value.Subscriptions.AssertEqual(
                Subscribe(550, 960));

            xs.Messages[5].Value.Value.Subscriptions.AssertEqual(
                Subscribe(750, 790));

            xs.Messages[6].Value.Value.Subscriptions.AssertEqual(
                Subscribe(850, 950));
        }

        [Fact]
        public void SelectMany_Complete_InnerNotComplete()
        {
            var scheduler = new TestScheduler();

            var xs = scheduler.CreateHotObservable(
                    OnNext(5, scheduler.CreateColdObservable(
                        OnError<int>(1, new InvalidOperationException()))),
                    OnNext(105, scheduler.CreateColdObservable(
                        OnError<int>(1, new InvalidOperationException()))),
                    OnNext(300, scheduler.CreateColdObservable(
                        OnNext(10, 102),
                        OnNext(90, 103),
                        OnNext(110, 104),
                        OnNext(190, 105),
                        OnNext(440, 106),
                        OnCompleted<int>(460))),
                    OnNext(400, scheduler.CreateColdObservable(
                        OnNext(180, 202),
                        OnNext(190, 203))),
                    OnNext(550, scheduler.CreateColdObservable(
                        OnNext(10, 301),
                        OnNext(50, 302),
                        OnNext(70, 303),
                        OnNext(260, 304),
                        OnNext(310, 305),
                        OnCompleted<int>(410))),
                    OnNext(750, scheduler.CreateColdObservable(
                        OnCompleted<int>(40))),
                    OnNext(850, scheduler.CreateColdObservable(
                        OnNext(80, 401),
                        OnNext(90, 402),
                        OnCompleted<int>(100))),
                    OnCompleted<ITestableObservable<int>>(900)
            );

            var res = scheduler.Start(() =>
                xs.SelectMany(x => x)
            );

            res.Messages.AssertEqual(
                OnNext(310, 102),
                OnNext(390, 103),
                OnNext(410, 104),
                OnNext(490, 105),
                OnNext(560, 301),
                OnNext(580, 202),
                OnNext(590, 203),
                OnNext(600, 302),
                OnNext(620, 303),
                OnNext(740, 106),
                OnNext(810, 304),
                OnNext(860, 305),
                OnNext(930, 401),
                OnNext(940, 402)
            );

            xs.Subscriptions.AssertEqual(
                Subscribe(200, 900));

            xs.Messages[2].Value.Value.Subscriptions.AssertEqual(
                Subscribe(300, 760));

            xs.Messages[3].Value.Value.Subscriptions.AssertEqual(
                Subscribe(400, 1000));

            xs.Messages[4].Value.Value.Subscriptions.AssertEqual(
                Subscribe(550, 960));

            xs.Messages[5].Value.Value.Subscriptions.AssertEqual(
                Subscribe(750, 790));

            xs.Messages[6].Value.Value.Subscriptions.AssertEqual(
                Subscribe(850, 950));
        }

        [Fact]
        public void SelectMany_Complete_OuterNotComplete()
        {
            var scheduler = new TestScheduler();

            var xs = scheduler.CreateHotObservable(
                    OnNext(5, scheduler.CreateColdObservable(
                        OnError<int>(1, new InvalidOperationException()))),
                    OnNext(105, scheduler.CreateColdObservable(
                        OnError<int>(1, new InvalidOperationException()))),
                    OnNext(300, scheduler.CreateColdObservable(
                        OnNext(10, 102),
                        OnNext(90, 103),
                        OnNext(110, 104),
                        OnNext(190, 105),
                        OnNext(440, 106),
                        OnCompleted<int>(460))),
                    OnNext(400, scheduler.CreateColdObservable(
                        OnNext(180, 202),
                        OnNext(190, 203),
                        OnCompleted<int>(205))),
                    OnNext(550, scheduler.CreateColdObservable(
                        OnNext(10, 301),
                        OnNext(50, 302),
                        OnNext(70, 303),
                        OnNext(260, 304),
                        OnNext(310, 305),
                        OnCompleted<int>(410))),
                    OnNext(750, scheduler.CreateColdObservable(
                        OnCompleted<int>(40))),
                    OnNext(850, scheduler.CreateColdObservable(
                        OnNext(80, 401),
                        OnNext(90, 402),
                        OnCompleted<int>(100)))
            );

            var res = scheduler.Start(() =>
                xs.SelectMany(x => x)
            );

            res.Messages.AssertEqual(
                OnNext(310, 102),
                OnNext(390, 103),
                OnNext(410, 104),
                OnNext(490, 105),
                OnNext(560, 301),
                OnNext(580, 202),
                OnNext(590, 203),
                OnNext(600, 302),
                OnNext(620, 303),
                OnNext(740, 106),
                OnNext(810, 304),
                OnNext(860, 305),
                OnNext(930, 401),
                OnNext(940, 402)
            );

            xs.Subscriptions.AssertEqual(
                Subscribe(200, 1000));

            xs.Messages[2].Value.Value.Subscriptions.AssertEqual(
                Subscribe(300, 760));

            xs.Messages[3].Value.Value.Subscriptions.AssertEqual(
                Subscribe(400, 605));

            xs.Messages[4].Value.Value.Subscriptions.AssertEqual(
                Subscribe(550, 960));

            xs.Messages[5].Value.Value.Subscriptions.AssertEqual(
                Subscribe(750, 790));

            xs.Messages[6].Value.Value.Subscriptions.AssertEqual(
                Subscribe(850, 950));
        }

        [Fact]
        public void SelectMany_Error_Outer()
        {
            var scheduler = new TestScheduler();

            var ex = new Exception();

            var xs = scheduler.CreateHotObservable(
                    OnNext(5, scheduler.CreateColdObservable(
                        OnError<int>(1, new InvalidOperationException()))),
                    OnNext(105, scheduler.CreateColdObservable(
                        OnError<int>(1, new InvalidOperationException()))),
                    OnNext(300, scheduler.CreateColdObservable(
                        OnNext(10, 102),
                        OnNext(90, 103),
                        OnNext(110, 104),
                        OnNext(190, 105),
                        OnNext(440, 106),
                        OnCompleted<int>(460))),
                    OnNext(400, scheduler.CreateColdObservable(
                        OnNext(180, 202),
                        OnNext(190, 203),
                        OnCompleted<int>(205))),
                    OnNext(550, scheduler.CreateColdObservable(
                        OnNext(10, 301),
                        OnNext(50, 302),
                        OnNext(70, 303),
                        OnNext(260, 304),
                        OnNext(310, 305),
                        OnCompleted<int>(410))),
                    OnNext(750, scheduler.CreateColdObservable(
                        OnCompleted<int>(40))),
                    OnNext(850, scheduler.CreateColdObservable(
                        OnNext(80, 401),
                        OnNext(90, 402),
                        OnCompleted<int>(100))),
                    OnError<ITestableObservable<int>>(900, ex)
            );

            var res = scheduler.Start(() =>
                xs.SelectMany(x => x)
            );

            res.Messages.AssertEqual(
                OnNext(310, 102),
                OnNext(390, 103),
                OnNext(410, 104),
                OnNext(490, 105),
                OnNext(560, 301),
                OnNext(580, 202),
                OnNext(590, 203),
                OnNext(600, 302),
                OnNext(620, 303),
                OnNext(740, 106),
                OnNext(810, 304),
                OnNext(860, 305),
                OnError<int>(900, ex)
            );

            xs.Subscriptions.AssertEqual(
                Subscribe(200, 900));

            xs.Messages[2].Value.Value.Subscriptions.AssertEqual(
                Subscribe(300, 760));

            xs.Messages[3].Value.Value.Subscriptions.AssertEqual(
                Subscribe(400, 605));

            xs.Messages[4].Value.Value.Subscriptions.AssertEqual(
                Subscribe(550, 900));

            xs.Messages[5].Value.Value.Subscriptions.AssertEqual(
                Subscribe(750, 790));

            xs.Messages[6].Value.Value.Subscriptions.AssertEqual(
                Subscribe(850, 900));
        }

        [Fact]
        public void SelectMany_Error_Inner()
        {
            var scheduler = new TestScheduler();

            var ex = new Exception();

            var xs = scheduler.CreateHotObservable(
                    OnNext(5, scheduler.CreateColdObservable(
                        OnError<int>(1, new InvalidOperationException()))),
                    OnNext(105, scheduler.CreateColdObservable(
                        OnError<int>(1, new InvalidOperationException()))),
                    OnNext(300, scheduler.CreateColdObservable(
                        OnNext(10, 102),
                        OnNext(90, 103),
                        OnNext(110, 104),
                        OnNext(190, 105),
                        OnNext(440, 106),
                        OnError<int>(460, ex))),
                    OnNext(400, scheduler.CreateColdObservable(
                        OnNext(180, 202),
                        OnNext(190, 203),
                        OnCompleted<int>(205))),
                    OnNext(550, scheduler.CreateColdObservable(
                        OnNext(10, 301),
                        OnNext(50, 302),
                        OnNext(70, 303),
                        OnNext(260, 304),
                        OnNext(310, 305),
                        OnCompleted<int>(410))),
                    OnNext(750, scheduler.CreateColdObservable(
                        OnCompleted<int>(40))),
                    OnNext(850, scheduler.CreateColdObservable(
                        OnNext(80, 401),
                        OnNext(90, 402),
                        OnCompleted<int>(100))),
                    OnCompleted<ITestableObservable<int>>(900)
            );

            var res = scheduler.Start(() =>
                xs.SelectMany(x => x)
            );

            res.Messages.AssertEqual(
                OnNext(310, 102),
                OnNext(390, 103),
                OnNext(410, 104),
                OnNext(490, 105),
                OnNext(560, 301),
                OnNext(580, 202),
                OnNext(590, 203),
                OnNext(600, 302),
                OnNext(620, 303),
                OnNext(740, 106),
                OnError<int>(760, ex)
            );

            xs.Subscriptions.AssertEqual(
                Subscribe(200, 760));

            xs.Messages[2].Value.Value.Subscriptions.AssertEqual(
                Subscribe(300, 760));

            xs.Messages[3].Value.Value.Subscriptions.AssertEqual(
                Subscribe(400, 605));

            xs.Messages[4].Value.Value.Subscriptions.AssertEqual(
                Subscribe(550, 760));

            xs.Messages[5].Value.Value.Subscriptions.AssertEqual(
                Subscribe(750, 760));

            xs.Messages[6].Value.Value.Subscriptions.AssertEqual(
            );
        }

        [Fact]
        public void SelectMany_Dispose()
        {
            var scheduler = new TestScheduler();

            var xs = scheduler.CreateHotObservable(
                    OnNext(5, scheduler.CreateColdObservable(
                        OnError<int>(1, new InvalidOperationException()))),
                    OnNext(105, scheduler.CreateColdObservable(
                        OnError<int>(1, new InvalidOperationException()))),
                    OnNext(300, scheduler.CreateColdObservable(
                        OnNext(10, 102),
                        OnNext(90, 103),
                        OnNext(110, 104),
                        OnNext(190, 105),
                        OnNext(440, 106),
                        OnCompleted<int>(460))),
                    OnNext(400, scheduler.CreateColdObservable(
                        OnNext(180, 202),
                        OnNext(190, 203),
                        OnCompleted<int>(205))),
                    OnNext(550, scheduler.CreateColdObservable(
                        OnNext(10, 301),
                        OnNext(50, 302),
                        OnNext(70, 303),
                        OnNext(260, 304),
                        OnNext(310, 305),
                        OnCompleted<int>(410))),
                    OnNext(750, scheduler.CreateColdObservable(
                        OnCompleted<int>(40))),
                    OnNext(850, scheduler.CreateColdObservable(
                        OnNext(80, 401),
                        OnNext(90, 402),
                        OnCompleted<int>(100))),
                    OnCompleted<ITestableObservable<int>>(900)
            );

            var res = scheduler.Start(() =>
                xs.SelectMany(x => x),
                700
            );

            res.Messages.AssertEqual(
                OnNext(310, 102),
                OnNext(390, 103),
                OnNext(410, 104),
                OnNext(490, 105),
                OnNext(560, 301),
                OnNext(580, 202),
                OnNext(590, 203),
                OnNext(600, 302),
                OnNext(620, 303)
            );

            xs.Subscriptions.AssertEqual(
                Subscribe(200, 700));

            xs.Messages[2].Value.Value.Subscriptions.AssertEqual(
                Subscribe(300, 700));

            xs.Messages[3].Value.Value.Subscriptions.AssertEqual(
                Subscribe(400, 605));

            xs.Messages[4].Value.Value.Subscriptions.AssertEqual(
                Subscribe(550, 700));

            xs.Messages[5].Value.Value.Subscriptions.AssertEqual(
            );

            xs.Messages[6].Value.Value.Subscriptions.AssertEqual(
            );
        }

        [Fact]
        public void SelectMany_Throw()
        {
            var scheduler = new TestScheduler();

            var xs = scheduler.CreateHotObservable(
                    OnNext(5, scheduler.CreateColdObservable(
                        OnError<int>(1, new InvalidOperationException()))),
                    OnNext(105, scheduler.CreateColdObservable(
                        OnError<int>(1, new InvalidOperationException()))),
                    OnNext(300, scheduler.CreateColdObservable(
                        OnNext(10, 102),
                        OnNext(90, 103),
                        OnNext(110, 104),
                        OnNext(190, 105),
                        OnNext(440, 106),
                        OnCompleted<int>(460))),
                    OnNext(400, scheduler.CreateColdObservable(
                        OnNext(180, 202),
                        OnNext(190, 203),
                        OnCompleted<int>(205))),
                    OnNext(550, scheduler.CreateColdObservable(
                        OnNext(10, 301),
                        OnNext(50, 302),
                        OnNext(70, 303),
                        OnNext(260, 304),
                        OnNext(310, 305),
                        OnCompleted<int>(410))),
                    OnNext(750, scheduler.CreateColdObservable(
                        OnCompleted<int>(40))),
                    OnNext(850, scheduler.CreateColdObservable(
                        OnNext(80, 401),
                        OnNext(90, 402),
                        OnCompleted<int>(100))),
                    OnCompleted<ITestableObservable<int>>(900)
            );

            var invoked = 0;

            var ex = new Exception();

            var res = scheduler.Start(() =>
                xs.SelectMany(x =>
                {
                    invoked++;
                    if (invoked == 3)
                    {
                        throw ex;
                    }

                    return x;
                })
            );

            res.Messages.AssertEqual(
                OnNext(310, 102),
                OnNext(390, 103),
                OnNext(410, 104),
                OnNext(490, 105),
                OnError<int>(550, ex)
            );

            xs.Subscriptions.AssertEqual(
                Subscribe(200, 550));

            xs.Messages[2].Value.Value.Subscriptions.AssertEqual(
                Subscribe(300, 550));

            xs.Messages[3].Value.Value.Subscriptions.AssertEqual(
                Subscribe(400, 550));

            xs.Messages[4].Value.Value.Subscriptions.AssertEqual(
            );

            xs.Messages[5].Value.Value.Subscriptions.AssertEqual(
            );

            xs.Messages[6].Value.Value.Subscriptions.AssertEqual(
            );

            Assert.Equal(3, invoked);
        }

        [Fact]
        public void SelectMany_UseFunction()
        {
            var scheduler = new TestScheduler();

            var xs = scheduler.CreateHotObservable(
                OnNext(210, 4),
                OnNext(220, 3),
                OnNext(250, 5),
                OnNext(270, 1),
                OnCompleted<int>(290)
            );

            var res = scheduler.Start(() =>
                xs.SelectMany(x => Observable.Interval(TimeSpan.FromTicks(10), scheduler).Select(_ => x).Take(x))
            );

            res.Messages.AssertEqual(
                OnNext(220, 4),
                OnNext(230, 3),
                OnNext(230, 4),
                OnNext(240, 3),
                OnNext(240, 4),
                OnNext(250, 3),
                OnNext(250, 4),
                OnNext(260, 5),
                OnNext(270, 5),
                OnNext(280, 1),
                OnNext(280, 5),
                OnNext(290, 5),
                OnNext(300, 5),
                OnCompleted<int>(300)
            );

            xs.Subscriptions.AssertEqual(
                Subscribe(200, 290)
            );
        }

        [Fact]
        public void SelectManyWithIndex_ArgumentChecking()
        {
            ReactiveAssert.Throws<ArgumentNullException>(() => ((IObservable<int>)null).SelectMany(DummyFunc<int, int, IObservable<int>>.Instance));
            ReactiveAssert.Throws<ArgumentNullException>(() => DummyObservable<int>.Instance.SelectMany((Func<int, int, IObservable<int>>)null));
            ReactiveAssert.Throws<ArgumentNullException>(() => DummyObservable<int>.Instance.SelectMany(DummyFunc<int, int, IObservable<int>>.Instance).Subscribe(null));
        }

        [Fact]
        public void SelectManyWithIndex_Index()
        {
            var scheduler = new TestScheduler();

            var xs = scheduler.CreateHotObservable(
                OnNext(210, 4),
                OnNext(220, 3),
                OnNext(250, 5),
                OnNext(270, 1),
                OnCompleted<int>(290)
            );

            var res = scheduler.Start(() =>
                xs.SelectMany((x, i) => Observable.Return(new { x, i }))
            );

            var witness = new { x = 0, i = 0 };

            res.Messages.AssertEqual(
                OnNext(210, new { x = 4, i = 0 }),
                OnNext(220, new { x = 3, i = 1 }),
                OnNext(250, new { x = 5, i = 2 }),
                OnNext(270, new { x = 1, i = 3 }),
                OnCompleted(290, witness)
            );

            xs.Subscriptions.AssertEqual(
                Subscribe(200, 290)
            );
        }

        [Fact]
        public void SelectManyWithIndex_Complete()
        {
            var scheduler = new TestScheduler();

            var xs = scheduler.CreateHotObservable(
                    OnNext(5, scheduler.CreateColdObservable(
                        OnError<int>(1, new InvalidOperationException()))),
                    OnNext(105, scheduler.CreateColdObservable(
                        OnError<int>(1, new InvalidOperationException()))),
                    OnNext(300, scheduler.CreateColdObservable(
                        OnNext(10, 102),
                        OnNext(90, 103),
                        OnNext(110, 104),
                        OnNext(190, 105),
                        OnNext(440, 106),
                        OnCompleted<int>(460))),
                    OnNext(400, scheduler.CreateColdObservable(
                        OnNext(180, 202),
                        OnNext(190, 203),
                        OnCompleted<int>(205))),
                    OnNext(550, scheduler.CreateColdObservable(
                        OnNext(10, 301),
                        OnNext(50, 302),
                        OnNext(70, 303),
                        OnNext(260, 304),
                        OnNext(310, 305),
                        OnCompleted<int>(410))),
                    OnNext(750, scheduler.CreateColdObservable(
                        OnCompleted<int>(40))),
                    OnNext(850, scheduler.CreateColdObservable(
                        OnNext(80, 401),
                        OnNext(90, 402),
                        OnCompleted<int>(100))),
                    OnCompleted<ITestableObservable<int>>(900)
            );

            var res = scheduler.Start(() =>
                xs.SelectMany((x, _) => x)
            );

            res.Messages.AssertEqual(
                OnNext(310, 102),
                OnNext(390, 103),
                OnNext(410, 104),
                OnNext(490, 105),
                OnNext(560, 301),
                OnNext(580, 202),
                OnNext(590, 203),
                OnNext(600, 302),
                OnNext(620, 303),
                OnNext(740, 106),
                OnNext(810, 304),
                OnNext(860, 305),
                OnNext(930, 401),
                OnNext(940, 402),
                OnCompleted<int>(960)
            );

            xs.Subscriptions.AssertEqual(
                Subscribe(200, 900));

            xs.Messages[2].Value.Value.Subscriptions.AssertEqual(
                Subscribe(300, 760));

            xs.Messages[3].Value.Value.Subscriptions.AssertEqual(
                Subscribe(400, 605));

            xs.Messages[4].Value.Value.Subscriptions.AssertEqual(
                Subscribe(550, 960));

            xs.Messages[5].Value.Value.Subscriptions.AssertEqual(
                Subscribe(750, 790));

            xs.Messages[6].Value.Value.Subscriptions.AssertEqual(
                Subscribe(850, 950));
        }

        [Fact]
        public void SelectManyWithIndex_Complete_InnerNotComplete()
        {
            var scheduler = new TestScheduler();

            var xs = scheduler.CreateHotObservable(
                    OnNext(5, scheduler.CreateColdObservable(
                        OnError<int>(1, new InvalidOperationException()))),
                    OnNext(105, scheduler.CreateColdObservable(
                        OnError<int>(1, new InvalidOperationException()))),
                    OnNext(300, scheduler.CreateColdObservable(
                        OnNext(10, 102),
                        OnNext(90, 103),
                        OnNext(110, 104),
                        OnNext(190, 105),
                        OnNext(440, 106),
                        OnCompleted<int>(460))),
                    OnNext(400, scheduler.CreateColdObservable(
                        OnNext(180, 202),
                        OnNext(190, 203))),
                    OnNext(550, scheduler.CreateColdObservable(
                        OnNext(10, 301),
                        OnNext(50, 302),
                        OnNext(70, 303),
                        OnNext(260, 304),
                        OnNext(310, 305),
                        OnCompleted<int>(410))),
                    OnNext(750, scheduler.CreateColdObservable(
                        OnCompleted<int>(40))),
                    OnNext(850, scheduler.CreateColdObservable(
                        OnNext(80, 401),
                        OnNext(90, 402),
                        OnCompleted<int>(100))),
                    OnCompleted<ITestableObservable<int>>(900)
            );

            var res = scheduler.Start(() =>
                xs.SelectMany((x, _) => x)
            );

            res.Messages.AssertEqual(
                OnNext(310, 102),
                OnNext(390, 103),
                OnNext(410, 104),
                OnNext(490, 105),
                OnNext(560, 301),
                OnNext(580, 202),
                OnNext(590, 203),
                OnNext(600, 302),
                OnNext(620, 303),
                OnNext(740, 106),
                OnNext(810, 304),
                OnNext(860, 305),
                OnNext(930, 401),
                OnNext(940, 402)
            );

            xs.Subscriptions.AssertEqual(
                Subscribe(200, 900));

            xs.Messages[2].Value.Value.Subscriptions.AssertEqual(
                Subscribe(300, 760));

            xs.Messages[3].Value.Value.Subscriptions.AssertEqual(
                Subscribe(400, 1000));

            xs.Messages[4].Value.Value.Subscriptions.AssertEqual(
                Subscribe(550, 960));

            xs.Messages[5].Value.Value.Subscriptions.AssertEqual(
                Subscribe(750, 790));

            xs.Messages[6].Value.Value.Subscriptions.AssertEqual(
                Subscribe(850, 950));
        }

        [Fact]
        public void SelectManyWithIndex_Complete_OuterNotComplete()
        {
            var scheduler = new TestScheduler();

            var xs = scheduler.CreateHotObservable(
                    OnNext(5, scheduler.CreateColdObservable(
                        OnError<int>(1, new InvalidOperationException()))),
                    OnNext(105, scheduler.CreateColdObservable(
                        OnError<int>(1, new InvalidOperationException()))),
                    OnNext(300, scheduler.CreateColdObservable(
                        OnNext(10, 102),
                        OnNext(90, 103),
                        OnNext(110, 104),
                        OnNext(190, 105),
                        OnNext(440, 106),
                        OnCompleted<int>(460))),
                    OnNext(400, scheduler.CreateColdObservable(
                        OnNext(180, 202),
                        OnNext(190, 203),
                        OnCompleted<int>(205))),
                    OnNext(550, scheduler.CreateColdObservable(
                        OnNext(10, 301),
                        OnNext(50, 302),
                        OnNext(70, 303),
                        OnNext(260, 304),
                        OnNext(310, 305),
                        OnCompleted<int>(410))),
                    OnNext(750, scheduler.CreateColdObservable(
                        OnCompleted<int>(40))),
                    OnNext(850, scheduler.CreateColdObservable(
                        OnNext(80, 401),
                        OnNext(90, 402),
                        OnCompleted<int>(100)))
            );

            var res = scheduler.Start(() =>
                xs.SelectMany((x, _) => x)
            );

            res.Messages.AssertEqual(
                OnNext(310, 102),
                OnNext(390, 103),
                OnNext(410, 104),
                OnNext(490, 105),
                OnNext(560, 301),
                OnNext(580, 202),
                OnNext(590, 203),
                OnNext(600, 302),
                OnNext(620, 303),
                OnNext(740, 106),
                OnNext(810, 304),
                OnNext(860, 305),
                OnNext(930, 401),
                OnNext(940, 402)
            );

            xs.Subscriptions.AssertEqual(
                Subscribe(200, 1000));

            xs.Messages[2].Value.Value.Subscriptions.AssertEqual(
                Subscribe(300, 760));

            xs.Messages[3].Value.Value.Subscriptions.AssertEqual(
                Subscribe(400, 605));

            xs.Messages[4].Value.Value.Subscriptions.AssertEqual(
                Subscribe(550, 960));

            xs.Messages[5].Value.Value.Subscriptions.AssertEqual(
                Subscribe(750, 790));

            xs.Messages[6].Value.Value.Subscriptions.AssertEqual(
                Subscribe(850, 950));
        }

        [Fact]
        public void SelectManyWithIndex_Error_Outer()
        {
            var scheduler = new TestScheduler();

            var ex = new Exception();

            var xs = scheduler.CreateHotObservable(
                    OnNext(5, scheduler.CreateColdObservable(
                        OnError<int>(1, new InvalidOperationException()))),
                    OnNext(105, scheduler.CreateColdObservable(
                        OnError<int>(1, new InvalidOperationException()))),
                    OnNext(300, scheduler.CreateColdObservable(
                        OnNext(10, 102),
                        OnNext(90, 103),
                        OnNext(110, 104),
                        OnNext(190, 105),
                        OnNext(440, 106),
                        OnCompleted<int>(460))),
                    OnNext(400, scheduler.CreateColdObservable(
                        OnNext(180, 202),
                        OnNext(190, 203),
                        OnCompleted<int>(205))),
                    OnNext(550, scheduler.CreateColdObservable(
                        OnNext(10, 301),
                        OnNext(50, 302),
                        OnNext(70, 303),
                        OnNext(260, 304),
                        OnNext(310, 305),
                        OnCompleted<int>(410))),
                    OnNext(750, scheduler.CreateColdObservable(
                        OnCompleted<int>(40))),
                    OnNext(850, scheduler.CreateColdObservable(
                        OnNext(80, 401),
                        OnNext(90, 402),
                        OnCompleted<int>(100))),
                    OnError<ITestableObservable<int>>(900, ex)
            );

            var res = scheduler.Start(() =>
                xs.SelectMany((x, _) => x)
            );

            res.Messages.AssertEqual(
                OnNext(310, 102),
                OnNext(390, 103),
                OnNext(410, 104),
                OnNext(490, 105),
                OnNext(560, 301),
                OnNext(580, 202),
                OnNext(590, 203),
                OnNext(600, 302),
                OnNext(620, 303),
                OnNext(740, 106),
                OnNext(810, 304),
                OnNext(860, 305),
                OnError<int>(900, ex)
            );

            xs.Subscriptions.AssertEqual(
                Subscribe(200, 900));

            xs.Messages[2].Value.Value.Subscriptions.AssertEqual(
                Subscribe(300, 760));

            xs.Messages[3].Value.Value.Subscriptions.AssertEqual(
                Subscribe(400, 605));

            xs.Messages[4].Value.Value.Subscriptions.AssertEqual(
                Subscribe(550, 900));

            xs.Messages[5].Value.Value.Subscriptions.AssertEqual(
                Subscribe(750, 790));

            xs.Messages[6].Value.Value.Subscriptions.AssertEqual(
                Subscribe(850, 900));
        }

        [Fact]
        public void SelectManyWithIndex_Error_Inner()
        {
            var scheduler = new TestScheduler();

            var ex = new Exception();

            var xs = scheduler.CreateHotObservable(
                    OnNext(5, scheduler.CreateColdObservable(
                        OnError<int>(1, new InvalidOperationException()))),
                    OnNext(105, scheduler.CreateColdObservable(
                        OnError<int>(1, new InvalidOperationException()))),
                    OnNext(300, scheduler.CreateColdObservable(
                        OnNext(10, 102),
                        OnNext(90, 103),
                        OnNext(110, 104),
                        OnNext(190, 105),
                        OnNext(440, 106),
                        OnError<int>(460, ex))),
                    OnNext(400, scheduler.CreateColdObservable(
                        OnNext(180, 202),
                        OnNext(190, 203),
                        OnCompleted<int>(205))),
                    OnNext(550, scheduler.CreateColdObservable(
                        OnNext(10, 301),
                        OnNext(50, 302),
                        OnNext(70, 303),
                        OnNext(260, 304),
                        OnNext(310, 305),
                        OnCompleted<int>(410))),
                    OnNext(750, scheduler.CreateColdObservable(
                        OnCompleted<int>(40))),
                    OnNext(850, scheduler.CreateColdObservable(
                        OnNext(80, 401),
                        OnNext(90, 402),
                        OnCompleted<int>(100))),
                    OnCompleted<ITestableObservable<int>>(900)
            );

            var res = scheduler.Start(() =>
                xs.SelectMany((x, _) => x)
            );

            res.Messages.AssertEqual(
                OnNext(310, 102),
                OnNext(390, 103),
                OnNext(410, 104),
                OnNext(490, 105),
                OnNext(560, 301),
                OnNext(580, 202),
                OnNext(590, 203),
                OnNext(600, 302),
                OnNext(620, 303),
                OnNext(740, 106),
                OnError<int>(760, ex)
            );

            xs.Subscriptions.AssertEqual(
                Subscribe(200, 760));

            xs.Messages[2].Value.Value.Subscriptions.AssertEqual(
                Subscribe(300, 760));

            xs.Messages[3].Value.Value.Subscriptions.AssertEqual(
                Subscribe(400, 605));

            xs.Messages[4].Value.Value.Subscriptions.AssertEqual(
                Subscribe(550, 760));

            xs.Messages[5].Value.Value.Subscriptions.AssertEqual(
                Subscribe(750, 760));

            xs.Messages[6].Value.Value.Subscriptions.AssertEqual(
            );
        }

        [Fact]
        public void SelectManyWithIndex_Dispose()
        {
            var scheduler = new TestScheduler();

            var xs = scheduler.CreateHotObservable(
                    OnNext(5, scheduler.CreateColdObservable(
                        OnError<int>(1, new InvalidOperationException()))),
                    OnNext(105, scheduler.CreateColdObservable(
                        OnError<int>(1, new InvalidOperationException()))),
                    OnNext(300, scheduler.CreateColdObservable(
                        OnNext(10, 102),
                        OnNext(90, 103),
                        OnNext(110, 104),
                        OnNext(190, 105),
                        OnNext(440, 106),
                        OnCompleted<int>(460))),
                    OnNext(400, scheduler.CreateColdObservable(
                        OnNext(180, 202),
                        OnNext(190, 203),
                        OnCompleted<int>(205))),
                    OnNext(550, scheduler.CreateColdObservable(
                        OnNext(10, 301),
                        OnNext(50, 302),
                        OnNext(70, 303),
                        OnNext(260, 304),
                        OnNext(310, 305),
                        OnCompleted<int>(410))),
                    OnNext(750, scheduler.CreateColdObservable(
                        OnCompleted<int>(40))),
                    OnNext(850, scheduler.CreateColdObservable(
                        OnNext(80, 401),
                        OnNext(90, 402),
                        OnCompleted<int>(100))),
                    OnCompleted<ITestableObservable<int>>(900)
            );

            var res = scheduler.Start(() =>
                xs.SelectMany((x, _) => x),
                700
            );

            res.Messages.AssertEqual(
                OnNext(310, 102),
                OnNext(390, 103),
                OnNext(410, 104),
                OnNext(490, 105),
                OnNext(560, 301),
                OnNext(580, 202),
                OnNext(590, 203),
                OnNext(600, 302),
                OnNext(620, 303)
            );

            xs.Subscriptions.AssertEqual(
                Subscribe(200, 700));

            xs.Messages[2].Value.Value.Subscriptions.AssertEqual(
                Subscribe(300, 700));

            xs.Messages[3].Value.Value.Subscriptions.AssertEqual(
                Subscribe(400, 605));

            xs.Messages[4].Value.Value.Subscriptions.AssertEqual(
                Subscribe(550, 700));

            xs.Messages[5].Value.Value.Subscriptions.AssertEqual(
            );

            xs.Messages[6].Value.Value.Subscriptions.AssertEqual(
            );
        }

        [Fact]
        public void SelectManyWithIndex_Throw()
        {
            var scheduler = new TestScheduler();

            var xs = scheduler.CreateHotObservable(
                    OnNext(5, scheduler.CreateColdObservable(
                        OnError<int>(1, new InvalidOperationException()))),
                    OnNext(105, scheduler.CreateColdObservable(
                        OnError<int>(1, new InvalidOperationException()))),
                    OnNext(300, scheduler.CreateColdObservable(
                        OnNext(10, 102),
                        OnNext(90, 103),
                        OnNext(110, 104),
                        OnNext(190, 105),
                        OnNext(440, 106),
                        OnCompleted<int>(460))),
                    OnNext(400, scheduler.CreateColdObservable(
                        OnNext(180, 202),
                        OnNext(190, 203),
                        OnCompleted<int>(205))),
                    OnNext(550, scheduler.CreateColdObservable(
                        OnNext(10, 301),
                        OnNext(50, 302),
                        OnNext(70, 303),
                        OnNext(260, 304),
                        OnNext(310, 305),
                        OnCompleted<int>(410))),
                    OnNext(750, scheduler.CreateColdObservable(
                        OnCompleted<int>(40))),
                    OnNext(850, scheduler.CreateColdObservable(
                        OnNext(80, 401),
                        OnNext(90, 402),
                        OnCompleted<int>(100))),
                    OnCompleted<ITestableObservable<int>>(900)
            );

            var invoked = 0;

            var ex = new Exception();

            var res = scheduler.Start(() =>
                xs.SelectMany((x, _) =>
                {
                    invoked++;
                    if (invoked == 3)
                    {
                        throw ex;
                    }

                    return x;
                })
            );

            res.Messages.AssertEqual(
                OnNext(310, 102),
                OnNext(390, 103),
                OnNext(410, 104),
                OnNext(490, 105),
                OnError<int>(550, ex)
            );

            xs.Subscriptions.AssertEqual(
                Subscribe(200, 550));

            xs.Messages[2].Value.Value.Subscriptions.AssertEqual(
                Subscribe(300, 550));

            xs.Messages[3].Value.Value.Subscriptions.AssertEqual(
                Subscribe(400, 550));

            xs.Messages[4].Value.Value.Subscriptions.AssertEqual(
            );

            xs.Messages[5].Value.Value.Subscriptions.AssertEqual(
            );

            xs.Messages[6].Value.Value.Subscriptions.AssertEqual(
            );

            Assert.Equal(3, invoked);
        }

        [Fact]
        public void SelectManyWithIndex_UseFunction()
        {
            var scheduler = new TestScheduler();

            var xs = scheduler.CreateHotObservable(
                OnNext(210, 4),
                OnNext(220, 3),
                OnNext(250, 5),
                OnNext(270, 1),
                OnCompleted<int>(290)
            );

            var res = scheduler.Start(() =>
                xs.SelectMany((x, _) => Observable.Interval(TimeSpan.FromTicks(10), scheduler).Select(__ => x).Take(x))
            );

            res.Messages.AssertEqual(
                OnNext(220, 4),
                OnNext(230, 3),
                OnNext(230, 4),
                OnNext(240, 3),
                OnNext(240, 4),
                OnNext(250, 3),
                OnNext(250, 4),
                OnNext(260, 5),
                OnNext(270, 5),
                OnNext(280, 1),
                OnNext(280, 5),
                OnNext(290, 5),
                OnNext(300, 5),
                OnCompleted<int>(300)
            );

            xs.Subscriptions.AssertEqual(
                Subscribe(200, 290)
            );
        }

        [Fact]
        public void SelectMany_Enumerable_ArgumentChecking()
        {
            ReactiveAssert.Throws<ArgumentNullException>(() => ((IObservable<int>)null).SelectMany(DummyFunc<int, IEnumerable<int>>.Instance));
            ReactiveAssert.Throws<ArgumentNullException>(() => DummyObservable<int>.Instance.SelectMany((Func<int, IEnumerable<int>>)null));
            ReactiveAssert.Throws<ArgumentNullException>(() => DummyObservable<int>.Instance.SelectMany(DummyFunc<int, IEnumerable<int>>.Instance).Subscribe(null));

            ReactiveAssert.Throws<ArgumentNullException>(() => ((IObservable<int>)null).SelectMany(DummyFunc<int, IEnumerable<int>>.Instance, DummyFunc<int, int, int>.Instance));
            ReactiveAssert.Throws<ArgumentNullException>(() => DummyObservable<int>.Instance.SelectMany((Func<int, IEnumerable<int>>)null, DummyFunc<int, int, int>.Instance));
            ReactiveAssert.Throws<ArgumentNullException>(() => DummyObservable<int>.Instance.SelectMany(DummyFunc<int, IEnumerable<int>>.Instance, (Func<int, int, int>)null));
        }

        [Fact]
        public void SelectMany_Enumerable_Complete()
        {
            var scheduler = new TestScheduler();

            var xs = scheduler.CreateHotObservable(
                OnNext(210, 2),
                OnNext(340, 4),
                OnNext(420, 3),
                OnNext(510, 2),
                OnCompleted<int>(600)
            );

            var inners = new List<MockEnumerable<int>>();

            var res = scheduler.Start(() =>
                xs.SelectMany(x =>
                {
                    var ys = new MockEnumerable<int>(scheduler, Enumerable.Repeat(x, x));
                    inners.Add(ys);
                    return ys;
                })
            );

            res.Messages.AssertEqual(
                OnNext(210, 2),
                OnNext(210, 2),
                OnNext(340, 4),
                OnNext(340, 4),
                OnNext(340, 4),
                OnNext(340, 4),
                OnNext(420, 3),
                OnNext(420, 3),
                OnNext(420, 3),
                OnNext(510, 2),
                OnNext(510, 2),
                OnCompleted<int>(600)
            );

            xs.Subscriptions.AssertEqual(
                Subscribe(200, 600)
            );

            Assert.Equal(4, inners.Count);

            inners[0].Subscriptions.AssertEqual(
                Subscribe(210, 210)
            );

            inners[1].Subscriptions.AssertEqual(
                Subscribe(340, 340)
            );

            inners[2].Subscriptions.AssertEqual(
                Subscribe(420, 420)
            );

            inners[3].Subscriptions.AssertEqual(
                Subscribe(510, 510)
            );
        }

        [Fact]
        public void SelectMany_Enumerable_Complete_ResultSelector()
        {
            var scheduler = new TestScheduler();

            var xs = scheduler.CreateHotObservable(
                OnNext(210, 2),
                OnNext(340, 4),
                OnNext(420, 3),
                OnNext(510, 2),
                OnCompleted<int>(600)
            );

            var res = scheduler.Start(() =>
                xs.SelectMany(x => Enumerable.Repeat(x, x), (x, y) => x + y)
            );

            res.Messages.AssertEqual(
                OnNext(210, 4),
                OnNext(210, 4),
                OnNext(340, 8),
                OnNext(340, 8),
                OnNext(340, 8),
                OnNext(340, 8),
                OnNext(420, 6),
                OnNext(420, 6),
                OnNext(420, 6),
                OnNext(510, 4),
                OnNext(510, 4),
                OnCompleted<int>(600)
            );

            xs.Subscriptions.AssertEqual(
                Subscribe(200, 600)
            );
        }

        [Fact]
        public void SelectMany_Enumerable_Error()
        {
            var scheduler = new TestScheduler();

            var ex = new Exception();

            var xs = scheduler.CreateHotObservable(
                OnNext(210, 2),
                OnNext(340, 4),
                OnNext(420, 3),
                OnNext(510, 2),
                OnError<int>(600, ex)
            );

            var res = scheduler.Start(() =>
                xs.SelectMany(x => Enumerable.Repeat(x, x))
            );

            res.Messages.AssertEqual(
                OnNext(210, 2),
                OnNext(210, 2),
                OnNext(340, 4),
                OnNext(340, 4),
                OnNext(340, 4),
                OnNext(340, 4),
                OnNext(420, 3),
                OnNext(420, 3),
                OnNext(420, 3),
                OnNext(510, 2),
                OnNext(510, 2),
                OnError<int>(600, ex)
            );

            xs.Subscriptions.AssertEqual(
                Subscribe(200, 600)
            );
        }

        [Fact]
        public void SelectMany_Enumerable_Error_ResultSelector()
        {
            var scheduler = new TestScheduler();

            var ex = new Exception();

            var xs = scheduler.CreateHotObservable(
                OnNext(210, 2),
                OnNext(340, 4),
                OnNext(420, 3),
                OnNext(510, 2),
                OnError<int>(600, ex)
            );

            var res = scheduler.Start(() =>
                xs.SelectMany(x => Enumerable.Repeat(x, x), (x, y) => x + y)
            );

            res.Messages.AssertEqual(
                OnNext(210, 4),
                OnNext(210, 4),
                OnNext(340, 8),
                OnNext(340, 8),
                OnNext(340, 8),
                OnNext(340, 8),
                OnNext(420, 6),
                OnNext(420, 6),
                OnNext(420, 6),
                OnNext(510, 4),
                OnNext(510, 4),
                OnError<int>(600, ex)
            );

            xs.Subscriptions.AssertEqual(
                Subscribe(200, 600)
            );
        }

        [Fact]
        public void SelectMany_Enumerable_Dispose()
        {
            var scheduler = new TestScheduler();

            var xs = scheduler.CreateHotObservable(
                OnNext(210, 2),
                OnNext(340, 4),
                OnNext(420, 3),
                OnNext(510, 2),
                OnCompleted<int>(600)
            );

            var res = scheduler.Start(() =>
                xs.SelectMany(x => Enumerable.Repeat(x, x)),
                350
            );

            res.Messages.AssertEqual(
                OnNext(210, 2),
                OnNext(210, 2),
                OnNext(340, 4),
                OnNext(340, 4),
                OnNext(340, 4),
                OnNext(340, 4)
            );

            xs.Subscriptions.AssertEqual(
                Subscribe(200, 350)
            );
        }

        [Fact]
        public void SelectMany_Enumerable_Dispose_ResultSelector()
        {
            var scheduler = new TestScheduler();

            var xs = scheduler.CreateHotObservable(
                OnNext(210, 2),
                OnNext(340, 4),
                OnNext(420, 3),
                OnNext(510, 2),
                OnCompleted<int>(600)
            );

            var res = scheduler.Start(() =>
                xs.SelectMany(x => Enumerable.Repeat(x, x), (x, y) => x + y),
                350
            );

            res.Messages.AssertEqual(
                OnNext(210, 4),
                OnNext(210, 4),
                OnNext(340, 8),
                OnNext(340, 8),
                OnNext(340, 8),
                OnNext(340, 8)
            );

            xs.Subscriptions.AssertEqual(
                Subscribe(200, 350)
            );
        }

        [Fact]
        public void SelectMany_Enumerable_SelectorThrows()
        {
            var scheduler = new TestScheduler();

            var xs = scheduler.CreateHotObservable(
                OnNext(210, 2),
                OnNext(340, 4),
                OnNext(420, 3),
                OnNext(510, 2),
                OnCompleted<int>(600)
            );

            var invoked = 0;
            var ex = new Exception();

            var res = scheduler.Start(() =>
                xs.SelectMany(x =>
                {
                    invoked++;
                    if (invoked == 3)
                    {
                        throw ex;
                    }

                    return Enumerable.Repeat(x, x);
                })
            );

            res.Messages.AssertEqual(
                OnNext(210, 2),
                OnNext(210, 2),
                OnNext(340, 4),
                OnNext(340, 4),
                OnNext(340, 4),
                OnNext(340, 4),
                OnError<int>(420, ex)
            );

            xs.Subscriptions.AssertEqual(
                Subscribe(200, 420)
            );

            Assert.Equal(3, invoked);
        }

        [Fact]
        public void SelectMany_Enumerable_ResultSelectorThrows()
        {
            var scheduler = new TestScheduler();

            var xs = scheduler.CreateHotObservable(
                OnNext(210, 2),
                OnNext(340, 4),
                OnNext(420, 3),
                OnNext(510, 2),
                OnCompleted<int>(600)
            );

            var ex = new Exception();

            var inners = new List<MockEnumerable<int>>();

            var res = scheduler.Start(() =>
                xs.SelectMany(
                    x =>
                    {
                        var ys = new MockEnumerable<int>(scheduler, Enumerable.Repeat(x, x));
                        inners.Add(ys);
                        return ys;
                    },
                    (x, y) =>
                    {
                        if (x == 3)
                        {
                            throw ex;
                        }

                        return x + y;
                    }
                )
            );

            res.Messages.AssertEqual(
                OnNext(210, 4),
                OnNext(210, 4),
                OnNext(340, 8),
                OnNext(340, 8),
                OnNext(340, 8),
                OnNext(340, 8),
                OnError<int>(420, ex)
            );

            xs.Subscriptions.AssertEqual(
                Subscribe(200, 420)
            );

            Assert.Equal(3, inners.Count);

            inners[0].Subscriptions.AssertEqual(
                Subscribe(210, 210)
            );

            inners[1].Subscriptions.AssertEqual(
                Subscribe(340, 340)
            );

            inners[2].Subscriptions.AssertEqual(
                Subscribe(420, 420)
            );
        }

        [Fact]
        public void SelectMany_Enumerable_ResultSelector_GetEnumeratorThrows()
        {
            var scheduler = new TestScheduler();

            var xs = scheduler.CreateHotObservable(
                OnNext(210, 2),
                OnNext(340, 4),
                OnNext(420, 3),
                OnNext(510, 2),
                OnCompleted<int>(600)
            );

            var ex = new Exception();

            var res = scheduler.Start(() =>
                xs.SelectMany(x => new RogueEnumerable<int>(ex), (x, y) => x + y)
            );

            res.Messages.AssertEqual(
                OnError<int>(210, ex)
            );

            xs.Subscriptions.AssertEqual(
                Subscribe(200, 210)
            );
        }

        [Fact]
        public void SelectMany_Enumerable_SelectorThrows_ResultSelector()
        {
            var scheduler = new TestScheduler();

            var xs = scheduler.CreateHotObservable(
                OnNext(210, 2),
                OnNext(340, 4),
                OnNext(420, 3),
                OnNext(510, 2),
                OnCompleted<int>(600)
            );

            var invoked = 0;
            var ex = new Exception();

            var res = scheduler.Start(() =>
                xs.SelectMany(
                    x =>
                    {
                        invoked++;
                        if (invoked == 3)
                        {
                            throw ex;
                        }

                        return Enumerable.Repeat(x, x);
                    },
                    (x, y) => x + y
                )
            );

            res.Messages.AssertEqual(
                OnNext(210, 4),
                OnNext(210, 4),
                OnNext(340, 8),
                OnNext(340, 8),
                OnNext(340, 8),
                OnNext(340, 8),
                OnError<int>(420, ex)
            );

            xs.Subscriptions.AssertEqual(
                Subscribe(200, 420)
            );

            Assert.Equal(3, invoked);
        }

        private class CurrentThrowsEnumerable<T> : IEnumerable<T>
        {
            private IEnumerable<T> _e;
            private readonly Exception _ex;

            public CurrentThrowsEnumerable(IEnumerable<T> e, Exception ex)
            {
                _e = e;
                _ex = ex;
            }

            public IEnumerator<T> GetEnumerator()
            {
                return new Enumerator(_e.GetEnumerator(), _ex);
            }

            System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
            {
                return GetEnumerator();
            }

            private class Enumerator : IEnumerator<T>
            {
                private IEnumerator<T> _e;
                private readonly Exception _ex;

                public Enumerator(IEnumerator<T> e, Exception ex)
                {
                    _e = e;
                    _ex = ex;
                }

                public T Current
                {
                    get { throw _ex; }
                }

                public void Dispose()
                {
                    _e.Dispose();
                }

                object System.Collections.IEnumerator.Current
                {
                    get { return Current; }
                }

                public bool MoveNext()
                {
                    return _e.MoveNext();
                }

                public void Reset()
                {
                    _e.Reset();
                }
            }
        }

        [Fact]
        public void SelectMany_Enumerable_CurrentThrows()
        {
            var scheduler = new TestScheduler();

            var xs = scheduler.CreateHotObservable(
                OnNext(210, 2),
                OnNext(340, 4),
                OnNext(420, 3),
                OnNext(510, 2),
                OnCompleted<int>(600)
            );

            var ex = new Exception();

            var res = scheduler.Start(() =>
                xs.SelectMany(x => new CurrentThrowsEnumerable<int>(Enumerable.Repeat(x, x), ex))
            );

            res.Messages.AssertEqual(
                OnError<int>(210, ex)
            );

            xs.Subscriptions.AssertEqual(
                Subscribe(200, 210)
            );
        }

        [Fact]
        public void SelectMany_Enumerable_CurrentThrows_ResultSelector()
        {
            var scheduler = new TestScheduler();

            var xs = scheduler.CreateHotObservable(
                OnNext(210, 2),
                OnNext(340, 4),
                OnNext(420, 3),
                OnNext(510, 2),
                OnCompleted<int>(600)
            );

            var ex = new Exception();

            var res = scheduler.Start(() =>
                xs.SelectMany(x => new CurrentThrowsEnumerable<int>(Enumerable.Repeat(x, x), ex), (x, y) => x + y)
            );

            res.Messages.AssertEqual(
                OnError<int>(210, ex)
            );

            xs.Subscriptions.AssertEqual(
                Subscribe(200, 210)
            );
        }

        private class MoveNextThrowsEnumerable<T> : IEnumerable<T>
        {
            private IEnumerable<T> _e;
            private readonly Exception _ex;

            public MoveNextThrowsEnumerable(IEnumerable<T> e, Exception ex)
            {
                _e = e;
                _ex = ex;
            }

            public IEnumerator<T> GetEnumerator()
            {
                return new Enumerator(_e.GetEnumerator(), _ex);
            }

            System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
            {
                return GetEnumerator();
            }

            private class Enumerator : IEnumerator<T>
            {
                private IEnumerator<T> _e;
                private readonly Exception _ex;

                public Enumerator(IEnumerator<T> e, Exception ex)
                {
                    _e = e;
                    _ex = ex;
                }

                public T Current
                {
                    get { return _e.Current; }
                }

                public void Dispose()
                {
                    _e.Dispose();
                }

                object System.Collections.IEnumerator.Current
                {
                    get { return Current; }
                }

                public bool MoveNext()
                {
                    throw _ex;
                }

                public void Reset()
                {
                    _e.Reset();
                }
            }
        }

        [Fact]
        public void SelectMany_Enumerable_GetEnumeratorThrows()
        {
            var scheduler = new TestScheduler();

            var xs = scheduler.CreateHotObservable(
                OnNext(210, 2),
                OnNext(340, 4),
                OnNext(420, 3),
                OnNext(510, 2),
                OnCompleted<int>(600)
            );

            var ex = new Exception();

            var res = scheduler.Start(() =>
                xs.SelectMany(x => new RogueEnumerable<int>(ex))
            );

            res.Messages.AssertEqual(
                OnError<int>(210, ex)
            );

            xs.Subscriptions.AssertEqual(
                Subscribe(200, 210)
            );
        }

        [Fact]
        public void SelectMany_Enumerable_MoveNextThrows()
        {
            var scheduler = new TestScheduler();

            var xs = scheduler.CreateHotObservable(
                OnNext(210, 2),
                OnNext(340, 4),
                OnNext(420, 3),
                OnNext(510, 2),
                OnCompleted<int>(600)
            );

            var ex = new Exception();

            var res = scheduler.Start(() =>
                xs.SelectMany(x => new MoveNextThrowsEnumerable<int>(Enumerable.Repeat(x, x), ex))
            );

            res.Messages.AssertEqual(
                OnError<int>(210, ex)
            );

            xs.Subscriptions.AssertEqual(
                Subscribe(200, 210)
            );
        }

        [Fact]
        public void SelectMany_Enumerable_MoveNextThrows_ResultSelector()
        {
            var scheduler = new TestScheduler();

            var xs = scheduler.CreateHotObservable(
                OnNext(210, 2),
                OnNext(340, 4),
                OnNext(420, 3),
                OnNext(510, 2),
                OnCompleted<int>(600)
            );

            var ex = new Exception();

            var res = scheduler.Start(() =>
                xs.SelectMany(x => new MoveNextThrowsEnumerable<int>(Enumerable.Repeat(x, x), ex), (x, y) => x + y)
            );

            res.Messages.AssertEqual(
                OnError<int>(210, ex)
            );

            xs.Subscriptions.AssertEqual(
                Subscribe(200, 210)
            );
        }

        [Fact]
        public void SelectManyWithIndex_Enumerable_ArgumentChecking()
        {
            ReactiveAssert.Throws<ArgumentNullException>(() => ((IObservable<int>)null).SelectMany(DummyFunc<int, int, IEnumerable<int>>.Instance));
            ReactiveAssert.Throws<ArgumentNullException>(() => DummyObservable<int>.Instance.SelectMany((Func<int, int, IEnumerable<int>>)null));
            ReactiveAssert.Throws<ArgumentNullException>(() => DummyObservable<int>.Instance.SelectMany(DummyFunc<int, int, IEnumerable<int>>.Instance).Subscribe(null));

            ReactiveAssert.Throws<ArgumentNullException>(() => ((IObservable<int>)null).SelectMany(DummyFunc<int, int, IEnumerable<int>>.Instance, DummyFunc<int, int, int, int, int>.Instance));
            ReactiveAssert.Throws<ArgumentNullException>(() => DummyObservable<int>.Instance.SelectMany((Func<int, int, IEnumerable<int>>)null, DummyFunc<int, int, int, int, int>.Instance));
            ReactiveAssert.Throws<ArgumentNullException>(() => DummyObservable<int>.Instance.SelectMany(DummyFunc<int, int, IEnumerable<int>>.Instance, (Func<int, int, int, int, int>)null));
        }

        [Fact]
        public void SelectManyWithIndex_Enumerable_Index()
        {
            var scheduler = new TestScheduler();

            var xs = scheduler.CreateHotObservable(
                OnNext(210, 4),
                OnNext(220, 3),
                OnNext(250, 5),
                OnNext(270, 1),
                OnCompleted<int>(290)
            );

            var res = scheduler.Start(() =>
                xs.SelectMany((x, i) => new[] { new { x, i } })
            );

            var witness = new { x = 0, i = 0 };

            res.Messages.AssertEqual(
                OnNext(210, new { x = 4, i = 0 }),
                OnNext(220, new { x = 3, i = 1 }),
                OnNext(250, new { x = 5, i = 2 }),
                OnNext(270, new { x = 1, i = 3 }),
                OnCompleted(290, witness)
            );

            xs.Subscriptions.AssertEqual(
                Subscribe(200, 290)
            );
        }

        [Fact]
        public void SelectManyWithIndex_Enumerable_ResultSelector_Index()
        {
            var scheduler = new TestScheduler();

            var xs = scheduler.CreateHotObservable(
                OnNext(210, 4),
                OnNext(220, 3),
                OnNext(250, 5),
                OnNext(270, 1),
                OnCompleted<int>(290)
            );

            var res = scheduler.Start(() =>
                xs.SelectMany((x, i) => Enumerable.Range(10, i + 1), (x, i, y, j) => new { x, i, y, j })
            );

            var witness = new { x = 0, i = 0, y = 0, j = 0 };

            res.Messages.AssertEqual(
                OnNext(210, new { x = 4, i = 0, y = 10, j = 0 }),
                OnNext(220, new { x = 3, i = 1, y = 10, j = 0 }),
                OnNext(220, new { x = 3, i = 1, y = 11, j = 1 }),
                OnNext(250, new { x = 5, i = 2, y = 10, j = 0 }),
                OnNext(250, new { x = 5, i = 2, y = 11, j = 1 }),
                OnNext(250, new { x = 5, i = 2, y = 12, j = 2 }),
                OnNext(270, new { x = 1, i = 3, y = 10, j = 0 }),
                OnNext(270, new { x = 1, i = 3, y = 11, j = 1 }),
                OnNext(270, new { x = 1, i = 3, y = 12, j = 2 }),
                OnNext(270, new { x = 1, i = 3, y = 13, j = 3 }),
                OnCompleted(290, witness)
            );

            xs.Subscriptions.AssertEqual(
                Subscribe(200, 290)
            );
        }

        [Fact]
        public void SelectManyWithIndex_Enumerable_Complete()
        {
            var scheduler = new TestScheduler();

            var xs = scheduler.CreateHotObservable(
                OnNext(210, 2),
                OnNext(340, 4),
                OnNext(420, 3),
                OnNext(510, 2),
                OnCompleted<int>(600)
            );

            var inners = new List<MockEnumerable<int>>();

            var res = scheduler.Start(() =>
                xs.SelectMany((x, _) =>
                {
                    var ys = new MockEnumerable<int>(scheduler, Enumerable.Repeat(x, x));
                    inners.Add(ys);
                    return ys;
                })
            );

            res.Messages.AssertEqual(
                OnNext(210, 2),
                OnNext(210, 2),
                OnNext(340, 4),
                OnNext(340, 4),
                OnNext(340, 4),
                OnNext(340, 4),
                OnNext(420, 3),
                OnNext(420, 3),
                OnNext(420, 3),
                OnNext(510, 2),
                OnNext(510, 2),
                OnCompleted<int>(600)
            );

            xs.Subscriptions.AssertEqual(
                Subscribe(200, 600)
            );

            Assert.Equal(4, inners.Count);

            inners[0].Subscriptions.AssertEqual(
                Subscribe(210, 210)
            );

            inners[1].Subscriptions.AssertEqual(
                Subscribe(340, 340)
            );

            inners[2].Subscriptions.AssertEqual(
                Subscribe(420, 420)
            );

            inners[3].Subscriptions.AssertEqual(
                Subscribe(510, 510)
            );
        }

        [Fact]
        public void SelectManyWithIndex_Enumerable_Complete_ResultSelector()
        {
            var scheduler = new TestScheduler();

            var xs = scheduler.CreateHotObservable(
                OnNext(210, 2),
                OnNext(340, 4),
                OnNext(420, 3),
                OnNext(510, 2),
                OnCompleted<int>(600)
            );

            var res = scheduler.Start(() =>
                xs.SelectMany((x, _) => Enumerable.Repeat(x, x), (x, _, y, __) => x + y)
            );

            res.Messages.AssertEqual(
                OnNext(210, 4),
                OnNext(210, 4),
                OnNext(340, 8),
                OnNext(340, 8),
                OnNext(340, 8),
                OnNext(340, 8),
                OnNext(420, 6),
                OnNext(420, 6),
                OnNext(420, 6),
                OnNext(510, 4),
                OnNext(510, 4),
                OnCompleted<int>(600)
            );

            xs.Subscriptions.AssertEqual(
                Subscribe(200, 600)
            );
        }

        [Fact]
        public void SelectManyWithIndex_Enumerable_Error()
        {
            var scheduler = new TestScheduler();

            var ex = new Exception();

            var xs = scheduler.CreateHotObservable(
                OnNext(210, 2),
                OnNext(340, 4),
                OnNext(420, 3),
                OnNext(510, 2),
                OnError<int>(600, ex)
            );

            var res = scheduler.Start(() =>
                xs.SelectMany((x, _) => Enumerable.Repeat(x, x))
            );

            res.Messages.AssertEqual(
                OnNext(210, 2),
                OnNext(210, 2),
                OnNext(340, 4),
                OnNext(340, 4),
                OnNext(340, 4),
                OnNext(340, 4),
                OnNext(420, 3),
                OnNext(420, 3),
                OnNext(420, 3),
                OnNext(510, 2),
                OnNext(510, 2),
                OnError<int>(600, ex)
            );

            xs.Subscriptions.AssertEqual(
                Subscribe(200, 600)
            );
        }

        [Fact]
        public void SelectManyWithIndex_Enumerable_Error_ResultSelector()
        {
            var scheduler = new TestScheduler();

            var ex = new Exception();

            var xs = scheduler.CreateHotObservable(
                OnNext(210, 2),
                OnNext(340, 4),
                OnNext(420, 3),
                OnNext(510, 2),
                OnError<int>(600, ex)
            );

            var res = scheduler.Start(() =>
                xs.SelectMany((x, _) => Enumerable.Repeat(x, x), (x, _, y, __) => x + y)
            );

            res.Messages.AssertEqual(
                OnNext(210, 4),
                OnNext(210, 4),
                OnNext(340, 8),
                OnNext(340, 8),
                OnNext(340, 8),
                OnNext(340, 8),
                OnNext(420, 6),
                OnNext(420, 6),
                OnNext(420, 6),
                OnNext(510, 4),
                OnNext(510, 4),
                OnError<int>(600, ex)
            );

            xs.Subscriptions.AssertEqual(
                Subscribe(200, 600)
            );
        }

        [Fact]
        public void SelectManyWithIndex_Enumerable_Dispose()
        {
            var scheduler = new TestScheduler();

            var xs = scheduler.CreateHotObservable(
                OnNext(210, 2),
                OnNext(340, 4),
                OnNext(420, 3),
                OnNext(510, 2),
                OnCompleted<int>(600)
            );

            var res = scheduler.Start(() =>
                xs.SelectMany((x, _) => Enumerable.Repeat(x, x)),
                350
            );

            res.Messages.AssertEqual(
                OnNext(210, 2),
                OnNext(210, 2),
                OnNext(340, 4),
                OnNext(340, 4),
                OnNext(340, 4),
                OnNext(340, 4)
            );

            xs.Subscriptions.AssertEqual(
                Subscribe(200, 350)
            );
        }

        [Fact]
        public void SelectManyWithIndex_Enumerable_Dispose_ResultSelector()
        {
            var scheduler = new TestScheduler();

            var xs = scheduler.CreateHotObservable(
                OnNext(210, 2),
                OnNext(340, 4),
                OnNext(420, 3),
                OnNext(510, 2),
                OnCompleted<int>(600)
            );

            var res = scheduler.Start(() =>
                xs.SelectMany((x, _) => Enumerable.Repeat(x, x), (x, _, y, __) => x + y),
                350
            );

            res.Messages.AssertEqual(
                OnNext(210, 4),
                OnNext(210, 4),
                OnNext(340, 8),
                OnNext(340, 8),
                OnNext(340, 8),
                OnNext(340, 8)
            );

            xs.Subscriptions.AssertEqual(
                Subscribe(200, 350)
            );
        }

        [Fact]
        public void SelectManyWithIndex_Enumerable_SelectorThrows()
        {
            var scheduler = new TestScheduler();

            var xs = scheduler.CreateHotObservable(
                OnNext(210, 2),
                OnNext(340, 4),
                OnNext(420, 3),
                OnNext(510, 2),
                OnCompleted<int>(600)
            );

            var invoked = 0;
            var ex = new Exception();

            var res = scheduler.Start(() =>
                xs.SelectMany((x, _) =>
                {
                    invoked++;
                    if (invoked == 3)
                    {
                        throw ex;
                    }

                    return Enumerable.Repeat(x, x);
                })
            );

            res.Messages.AssertEqual(
                OnNext(210, 2),
                OnNext(210, 2),
                OnNext(340, 4),
                OnNext(340, 4),
                OnNext(340, 4),
                OnNext(340, 4),
                OnError<int>(420, ex)
            );

            xs.Subscriptions.AssertEqual(
                Subscribe(200, 420)
            );

            Assert.Equal(3, invoked);
        }

        [Fact]
        public void SelectManyWithIndex_Enumerable_ResultSelectorThrows()
        {
            var scheduler = new TestScheduler();

            var xs = scheduler.CreateHotObservable(
                OnNext(210, 2),
                OnNext(340, 4),
                OnNext(420, 3),
                OnNext(510, 2),
                OnCompleted<int>(600)
            );

            var ex = new Exception();

            var inners = new List<MockEnumerable<int>>();

            var res = scheduler.Start(() =>
                xs.SelectMany(
                    (x, _) =>
                    {
                        var ys = new MockEnumerable<int>(scheduler, Enumerable.Repeat(x, x));
                        inners.Add(ys);
                        return ys;
                    },
                    (x, _, y, __) =>
                    {
                        if (x == 3)
                        {
                            throw ex;
                        }

                        return x + y;
                    }
                )
            );

            res.Messages.AssertEqual(
                OnNext(210, 4),
                OnNext(210, 4),
                OnNext(340, 8),
                OnNext(340, 8),
                OnNext(340, 8),
                OnNext(340, 8),
                OnError<int>(420, ex)
            );

            xs.Subscriptions.AssertEqual(
                Subscribe(200, 420)
            );

            Assert.Equal(3, inners.Count);

            inners[0].Subscriptions.AssertEqual(
                Subscribe(210, 210)
            );

            inners[1].Subscriptions.AssertEqual(
                Subscribe(340, 340)
            );

            inners[2].Subscriptions.AssertEqual(
                Subscribe(420, 420)
            );
        }

        [Fact]
        public void SelectManyWithIndex_Enumerable_ResultSelector_GetEnumeratorThrows()
        {
            var scheduler = new TestScheduler();

            var xs = scheduler.CreateHotObservable(
                OnNext(210, 2),
                OnNext(340, 4),
                OnNext(420, 3),
                OnNext(510, 2),
                OnCompleted<int>(600)
            );

            var ex = new Exception();

            var res = scheduler.Start(() =>
                xs.SelectMany((x, _) => new RogueEnumerable<int>(ex), (x, _, y, __) => x + y)
            );

            res.Messages.AssertEqual(
                OnError<int>(210, ex)
            );

            xs.Subscriptions.AssertEqual(
                Subscribe(200, 210)
            );
        }

        [Fact]
        public void SelectManyWithIndex_Enumerable_SelectorThrows_ResultSelector()
        {
            var scheduler = new TestScheduler();

            var xs = scheduler.CreateHotObservable(
                OnNext(210, 2),
                OnNext(340, 4),
                OnNext(420, 3),
                OnNext(510, 2),
                OnCompleted<int>(600)
            );

            var invoked = 0;
            var ex = new Exception();

            var res = scheduler.Start(() =>
                xs.SelectMany(
                    (x, _) =>
                    {
                        invoked++;
                        if (invoked == 3)
                        {
                            throw ex;
                        }

                        return Enumerable.Repeat(x, x);
                    },
                    (x, _, y, __) => x + y
                )
            );

            res.Messages.AssertEqual(
                OnNext(210, 4),
                OnNext(210, 4),
                OnNext(340, 8),
                OnNext(340, 8),
                OnNext(340, 8),
                OnNext(340, 8),
                OnError<int>(420, ex)
            );

            xs.Subscriptions.AssertEqual(
                Subscribe(200, 420)
            );

            Assert.Equal(3, invoked);
        }

        [Fact]
        public void SelectManyWithIndex_Enumerable_CurrentThrows()
        {
            var scheduler = new TestScheduler();

            var xs = scheduler.CreateHotObservable(
                OnNext(210, 2),
                OnNext(340, 4),
                OnNext(420, 3),
                OnNext(510, 2),
                OnCompleted<int>(600)
            );

            var ex = new Exception();

            var res = scheduler.Start(() =>
                xs.SelectMany((x, _) => new CurrentThrowsEnumerable<int>(Enumerable.Repeat(x, x), ex))
            );

            res.Messages.AssertEqual(
                OnError<int>(210, ex)
            );

            xs.Subscriptions.AssertEqual(
                Subscribe(200, 210)
            );
        }

        [Fact]
        public void SelectManyWithIndex_Enumerable_CurrentThrows_ResultSelector()
        {
            var scheduler = new TestScheduler();

            var xs = scheduler.CreateHotObservable(
                OnNext(210, 2),
                OnNext(340, 4),
                OnNext(420, 3),
                OnNext(510, 2),
                OnCompleted<int>(600)
            );

            var ex = new Exception();

            var res = scheduler.Start(() =>
                xs.SelectMany((x, _) => new CurrentThrowsEnumerable<int>(Enumerable.Repeat(x, x), ex), (x, _, y, __) => x + y)
            );

            res.Messages.AssertEqual(
                OnError<int>(210, ex)
            );

            xs.Subscriptions.AssertEqual(
                Subscribe(200, 210)
            );
        }

        [Fact]
        public void SelectManyWithIndex_Enumerable_GetEnumeratorThrows()
        {
            var scheduler = new TestScheduler();

            var xs = scheduler.CreateHotObservable(
                OnNext(210, 2),
                OnNext(340, 4),
                OnNext(420, 3),
                OnNext(510, 2),
                OnCompleted<int>(600)
            );

            var ex = new Exception();

            var res = scheduler.Start(() =>
                xs.SelectMany((x, _) => new RogueEnumerable<int>(ex))
            );

            res.Messages.AssertEqual(
                OnError<int>(210, ex)
            );

            xs.Subscriptions.AssertEqual(
                Subscribe(200, 210)
            );
        }

        [Fact]
        public void SelectManyWithIndex_Enumerable_MoveNextThrows()
        {
            var scheduler = new TestScheduler();

            var xs = scheduler.CreateHotObservable(
                OnNext(210, 2),
                OnNext(340, 4),
                OnNext(420, 3),
                OnNext(510, 2),
                OnCompleted<int>(600)
            );

            var ex = new Exception();

            var res = scheduler.Start(() =>
                xs.SelectMany((x, _) => new MoveNextThrowsEnumerable<int>(Enumerable.Repeat(x, x), ex))
            );

            res.Messages.AssertEqual(
                OnError<int>(210, ex)
            );

            xs.Subscriptions.AssertEqual(
                Subscribe(200, 210)
            );
        }

        [Fact]
        public void SelectManyWithIndex_Enumerable_MoveNextThrows_ResultSelector()
        {
            var scheduler = new TestScheduler();

            var xs = scheduler.CreateHotObservable(
                OnNext(210, 2),
                OnNext(340, 4),
                OnNext(420, 3),
                OnNext(510, 2),
                OnCompleted<int>(600)
            );

            var ex = new Exception();

            var res = scheduler.Start(() =>
                xs.SelectMany((x, _) => new MoveNextThrowsEnumerable<int>(Enumerable.Repeat(x, x), ex), (x, _, y, __) => x + y)
            );

            res.Messages.AssertEqual(
                OnError<int>(210, ex)
            );

            xs.Subscriptions.AssertEqual(
                Subscribe(200, 210)
            );
        }

        [Fact]
        public void SelectMany_QueryOperator_ArgumentChecking()
        {
            ReactiveAssert.Throws<ArgumentNullException>(() => ((IObservable<int>)null).SelectMany(DummyFunc<int, IObservable<int>>.Instance, DummyFunc<int, int, int>.Instance));
            ReactiveAssert.Throws<ArgumentNullException>(() => DummyObservable<int>.Instance.SelectMany((Func<int, IObservable<int>>)null, DummyFunc<int, int, int>.Instance));
            ReactiveAssert.Throws<ArgumentNullException>(() => DummyObservable<int>.Instance.SelectMany(DummyFunc<int, IObservable<int>>.Instance, ((Func<int, int, int>)null)));
            ReactiveAssert.Throws<ArgumentNullException>(() => DummyObservable<int>.Instance.SelectMany(DummyFunc<int, IObservable<int>>.Instance, DummyFunc<int, int, int>.Instance).Subscribe(null));

            ReactiveAssert.Throws<ArgumentNullException>(() => ((IObservable<int>)null).SelectMany(DummyFunc<int, Task<int>>.Instance, DummyFunc<int, int, int>.Instance));
            ReactiveAssert.Throws<ArgumentNullException>(() => DummyObservable<int>.Instance.SelectMany((Func<int, Task<int>>)null, DummyFunc<int, int, int>.Instance));
            ReactiveAssert.Throws<ArgumentNullException>(() => DummyObservable<int>.Instance.SelectMany(DummyFunc<int, Task<int>>.Instance, ((Func<int, int, int>)null)));

            ReactiveAssert.Throws<ArgumentNullException>(() => ((IObservable<int>)null).SelectMany(DummyFunc<int, CancellationToken, Task<int>>.Instance, DummyFunc<int, int, int>.Instance));
            ReactiveAssert.Throws<ArgumentNullException>(() => DummyObservable<int>.Instance.SelectMany((Func<int, CancellationToken, Task<int>>)null, DummyFunc<int, int, int>.Instance));
            ReactiveAssert.Throws<ArgumentNullException>(() => DummyObservable<int>.Instance.SelectMany(DummyFunc<int, CancellationToken, Task<int>>.Instance, ((Func<int, int, int>)null)));

            ReactiveAssert.Throws<ArgumentNullException>(() => ((IObservable<int>)null).SelectMany(DummyFunc<int, Task<int>>.Instance));
            ReactiveAssert.Throws<ArgumentNullException>(() => DummyObservable<int>.Instance.SelectMany((Func<int, Task<int>>)null));

            ReactiveAssert.Throws<ArgumentNullException>(() => ((IObservable<int>)null).SelectMany(DummyFunc<int, CancellationToken, Task<int>>.Instance));
            ReactiveAssert.Throws<ArgumentNullException>(() => DummyObservable<int>.Instance.SelectMany((Func<int, CancellationToken, Task<int>>)null));
        }

        [Fact]
        public void SelectMany_QueryOperator_CompleteOuterFirst()
        {
            var scheduler = new TestScheduler();

            var xs = scheduler.CreateHotObservable(
                OnNext(220, 4),
                OnNext(221, 3),
                OnNext(222, 2),
                OnNext(223, 5),
                OnCompleted<int>(224)
            );

            var res = scheduler.Start(() =>
                from x in xs
                from y in Observable.Interval(TimeSpan.FromTicks(1), scheduler).Take(x)
                select x * 10 + (int)y
            );

            res.Messages.AssertEqual(
                OnNext(221, 40),
                OnNext(222, 30),
                OnNext(222, 41),
                OnNext(223, 20),
                OnNext(223, 31),
                OnNext(223, 42),
                OnNext(224, 50),
                OnNext(224, 21),
                OnNext(224, 32),
                OnNext(224, 43),
                OnNext(225, 51),
                OnNext(226, 52),
                OnNext(227, 53),
                OnNext(228, 54),
                OnCompleted<int>(228)
            );

            xs.Subscriptions.AssertEqual(
                Subscribe(200, 224)
            );
        }

        [Fact]
        public void SelectMany_QueryOperator_CompleteInnerFirst()
        {
            var scheduler = new TestScheduler();

            var xs = scheduler.CreateHotObservable(
                OnNext(220, 4),
                OnNext(221, 3),
                OnNext(222, 2),
                OnNext(223, 5),
                OnCompleted<int>(300)
            );

            var res = scheduler.Start(() =>
                from x in xs
                from y in Observable.Interval(TimeSpan.FromTicks(1), scheduler).Take(x)
                select x * 10 + (int)y
            );

            res.Messages.AssertEqual(
                OnNext(221, 40),
                OnNext(222, 30),
                OnNext(222, 41),
                OnNext(223, 20),
                OnNext(223, 31),
                OnNext(223, 42),
                OnNext(224, 50),
                OnNext(224, 21),
                OnNext(224, 32),
                OnNext(224, 43),
                OnNext(225, 51),
                OnNext(226, 52),
                OnNext(227, 53),
                OnNext(228, 54),
                OnCompleted<int>(300)
            );

            xs.Subscriptions.AssertEqual(
                Subscribe(200, 300)
            );
        }

        [Fact]
        public void SelectMany_QueryOperator_ErrorOuter()
        {
            var scheduler = new TestScheduler();

            var ex = new Exception();

            var xs = scheduler.CreateHotObservable(
                OnNext(220, 4),
                OnNext(221, 3),
                OnNext(222, 2),
                OnNext(223, 5),
                OnError<int>(224, ex)
            );

            var res = scheduler.Start(() =>
                from x in xs
                from y in Observable.Interval(TimeSpan.FromTicks(1), scheduler).Take(x)
                select x * 10 + (int)y
            );

            res.Messages.AssertEqual(
                OnNext(221, 40),
                OnNext(222, 30),
                OnNext(222, 41),
                OnNext(223, 20),
                OnNext(223, 31),
                OnNext(223, 42),
                OnError<int>(224, ex)
            );

            xs.Subscriptions.AssertEqual(
                Subscribe(200, 224)
            );
        }

        [Fact]
        public void SelectMany_QueryOperator_ErrorInner()
        {
            var scheduler = new TestScheduler();

            var xs = scheduler.CreateHotObservable(
                OnNext(220, 4),
                OnNext(221, 3),
                OnNext(222, 2),
                OnNext(223, 5),
                OnCompleted<int>(224)
            );

            var ex = new Exception();

            var res = scheduler.Start(() =>
                from x in xs
                from y in x == 2 ? Observable.Throw<long>(ex, scheduler)
                                 : Observable.Interval(TimeSpan.FromTicks(1), scheduler).Take(x)
                select x * 10 + (int)y
            );

            res.Messages.AssertEqual(
                OnNext(221, 40),
                OnNext(222, 30),
                OnNext(222, 41),
                OnError<int>(223, ex)
            );

            xs.Subscriptions.AssertEqual(
                Subscribe(200, 223)
            );
        }

        [Fact]
        public void SelectMany_QueryOperator_Dispose()
        {
            var scheduler = new TestScheduler();

            var xs = scheduler.CreateHotObservable(
                OnNext(220, 4),
                OnNext(221, 3),
                OnNext(222, 2),
                OnNext(223, 5),
                OnCompleted<int>(224)
            );

            var res = scheduler.Start(() =>
                from x in xs
                from y in Observable.Interval(TimeSpan.FromTicks(1), scheduler).Take(x)
                select x * 10 + (int)y,
                223
            );

            res.Messages.AssertEqual(
                OnNext(221, 40),
                OnNext(222, 30),
                OnNext(222, 41)
            );

            xs.Subscriptions.AssertEqual(
                Subscribe(200, 223)
            );
        }

        private static T Throw<T>(Exception ex)
        {
            throw ex;
        }


        [Fact]
        public void SelectMany_QueryOperator_ThrowSelector()
        {
            var scheduler = new TestScheduler();

            var xs = scheduler.CreateHotObservable(
                OnNext(220, 4),
                OnNext(221, 3),
                OnNext(222, 2),
                OnNext(223, 5),
                OnCompleted<int>(224)
            );

            var ex = new Exception();

            var res = scheduler.Start(() =>
                from x in xs
                from y in Throw<IObservable<long>>(ex)
                select x * 10 + (int)y
            );

            res.Messages.AssertEqual(
                OnError<int>(220, ex)
            );

            xs.Subscriptions.AssertEqual(
                Subscribe(200, 220)
            );
        }

        [Fact]
        public void SelectMany_QueryOperator_ThrowResult()
        {
            var scheduler = new TestScheduler();

            var xs = scheduler.CreateHotObservable(
                OnNext(220, 4),
                OnNext(221, 3),
                OnNext(222, 2),
                OnNext(223, 5),
                OnCompleted<int>(224)
            );

            var ex = new Exception();

            var res = scheduler.Start(() =>
                from x in xs
                from y in Observable.Interval(TimeSpan.FromTicks(1), scheduler).Take(x)
                select Throw<int>(ex)
            );

            res.Messages.AssertEqual(
                OnError<int>(221, ex)
            );

            xs.Subscriptions.AssertEqual(
                Subscribe(200, 221)
            );
        }

        [Fact]
        public void SelectManyWithIndex_QueryOperator_ArgumentChecking()
        {
            ReactiveAssert.Throws<ArgumentNullException>(() => ((IObservable<int>)null).SelectMany(DummyFunc<int, int, IObservable<int>>.Instance, DummyFunc<int, int, int, int, int>.Instance));
            ReactiveAssert.Throws<ArgumentNullException>(() => DummyObservable<int>.Instance.SelectMany((Func<int, int, IObservable<int>>)null, DummyFunc<int, int, int, int, int>.Instance));
            ReactiveAssert.Throws<ArgumentNullException>(() => DummyObservable<int>.Instance.SelectMany(DummyFunc<int, int, IObservable<int>>.Instance, ((Func<int, int, int, int, int>)null)));
            ReactiveAssert.Throws<ArgumentNullException>(() => DummyObservable<int>.Instance.SelectMany(DummyFunc<int, int, IObservable<int>>.Instance, DummyFunc<int, int, int, int, int>.Instance).Subscribe(null));
        }

        [Fact]
        public void SelectManyWithIndex_QueryOperator_Index()
        {
            var scheduler = new TestScheduler();

            var xs = scheduler.CreateHotObservable(
                OnNext(210, 4),
                OnNext(220, 3),
                OnNext(250, 5),
                OnNext(270, 1),
                OnCompleted<int>(290)
            );

            var res = scheduler.Start(() =>
                xs.SelectMany((x, i) => Observable.Range(10, i + 1), (x, i, y, j) => new { x, i, y, j })
            );

            var witness = new { x = 0, i = 0, y = 0, j = 0 };

            res.Messages.AssertEqual(
                OnNext(210, new { x = 4, i = 0, y = 10, j = 0 }),
                OnNext(220, new { x = 3, i = 1, y = 10, j = 0 }),
                OnNext(220, new { x = 3, i = 1, y = 11, j = 1 }),
                OnNext(250, new { x = 5, i = 2, y = 10, j = 0 }),
                OnNext(250, new { x = 5, i = 2, y = 11, j = 1 }),
                OnNext(250, new { x = 5, i = 2, y = 12, j = 2 }),
                OnNext(270, new { x = 1, i = 3, y = 10, j = 0 }),
                OnNext(270, new { x = 1, i = 3, y = 11, j = 1 }),
                OnNext(270, new { x = 1, i = 3, y = 12, j = 2 }),
                OnNext(270, new { x = 1, i = 3, y = 13, j = 3 }),
                OnCompleted(290, witness)
            );

            xs.Subscriptions.AssertEqual(
                Subscribe(200, 290)
            );
        }

        [Fact]
        public void SelectManyWithIndex_QueryOperator_CompleteOuterFirst()
        {
            var scheduler = new TestScheduler();

            var xs = scheduler.CreateHotObservable(
                OnNext(220, 4),
                OnNext(221, 3),
                OnNext(222, 2),
                OnNext(223, 5),
                OnCompleted<int>(224)
            );

            var res = scheduler.Start(() =>
                xs.SelectMany((x, _) => Observable.Interval(TimeSpan.FromTicks(1), scheduler).Take(x), (x, _, y, __) => x * 10 + (int)y)
            );

            res.Messages.AssertEqual(
                OnNext(221, 40),
                OnNext(222, 30),
                OnNext(222, 41),
                OnNext(223, 20),
                OnNext(223, 31),
                OnNext(223, 42),
                OnNext(224, 50),
                OnNext(224, 21),
                OnNext(224, 32),
                OnNext(224, 43),
                OnNext(225, 51),
                OnNext(226, 52),
                OnNext(227, 53),
                OnNext(228, 54),
                OnCompleted<int>(228)
            );

            xs.Subscriptions.AssertEqual(
                Subscribe(200, 224)
            );
        }

        [Fact]
        public void SelectManyWithIndex_QueryOperator_CompleteInnerFirst()
        {
            var scheduler = new TestScheduler();

            var xs = scheduler.CreateHotObservable(
                OnNext(220, 4),
                OnNext(221, 3),
                OnNext(222, 2),
                OnNext(223, 5),
                OnCompleted<int>(300)
            );

            var res = scheduler.Start(() =>
                xs.SelectMany((x, _) => Observable.Interval(TimeSpan.FromTicks(1), scheduler).Take(x), (x, _, y, __) => x * 10 + (int)y)
            );

            res.Messages.AssertEqual(
                OnNext(221, 40),
                OnNext(222, 30),
                OnNext(222, 41),
                OnNext(223, 20),
                OnNext(223, 31),
                OnNext(223, 42),
                OnNext(224, 50),
                OnNext(224, 21),
                OnNext(224, 32),
                OnNext(224, 43),
                OnNext(225, 51),
                OnNext(226, 52),
                OnNext(227, 53),
                OnNext(228, 54),
                OnCompleted<int>(300)
            );

            xs.Subscriptions.AssertEqual(
                Subscribe(200, 300)
            );
        }

        [Fact]
        public void SelectManyWithIndex_QueryOperator_ErrorOuter()
        {
            var scheduler = new TestScheduler();

            var ex = new Exception();

            var xs = scheduler.CreateHotObservable(
                OnNext(220, 4),
                OnNext(221, 3),
                OnNext(222, 2),
                OnNext(223, 5),
                OnError<int>(224, ex)
            );

            var res = scheduler.Start(() =>
                xs.SelectMany((x, _) => Observable.Interval(TimeSpan.FromTicks(1), scheduler).Take(x), (x, _, y, __) => x * 10 + (int)y)
            );

            res.Messages.AssertEqual(
                OnNext(221, 40),
                OnNext(222, 30),
                OnNext(222, 41),
                OnNext(223, 20),
                OnNext(223, 31),
                OnNext(223, 42),
                OnError<int>(224, ex)
            );

            xs.Subscriptions.AssertEqual(
                Subscribe(200, 224)
            );
        }

        [Fact]
        public void SelectManyWithIndex_QueryOperator_ErrorInner()
        {
            var scheduler = new TestScheduler();

            var xs = scheduler.CreateHotObservable(
                OnNext(220, 4),
                OnNext(221, 3),
                OnNext(222, 2),
                OnNext(223, 5),
                OnCompleted<int>(224)
            );

            var ex = new Exception();

            var res = scheduler.Start(() =>
                xs.SelectMany(
                    (x, _) => x == 2
                            ? Observable.Throw<long>(ex, scheduler)
                            : Observable.Interval(TimeSpan.FromTicks(1), scheduler).Take(x),
                    (x, _, y, __) => x * 10 + (int)y)
            );

            res.Messages.AssertEqual(
                OnNext(221, 40),
                OnNext(222, 30),
                OnNext(222, 41),
                OnError<int>(223, ex)
            );

            xs.Subscriptions.AssertEqual(
                Subscribe(200, 223)
            );
        }

        [Fact]
        public void SelectManyWithIndex_QueryOperator_Dispose()
        {
            var scheduler = new TestScheduler();

            var xs = scheduler.CreateHotObservable(
                OnNext(220, 4),
                OnNext(221, 3),
                OnNext(222, 2),
                OnNext(223, 5),
                OnCompleted<int>(224)
            );

            var res = scheduler.Start(() =>
                xs.SelectMany((x, _) => Observable.Interval(TimeSpan.FromTicks(1), scheduler).Take(x), (x, _, y, __) => x * 10 + (int)y),
                223
            );

            res.Messages.AssertEqual(
                OnNext(221, 40),
                OnNext(222, 30),
                OnNext(222, 41)
            );

            xs.Subscriptions.AssertEqual(
                Subscribe(200, 223)
            );
        }

        [Fact]
        public void SelectManyWithIndex_QueryOperator_ThrowSelector()
        {
            var scheduler = new TestScheduler();

            var xs = scheduler.CreateHotObservable(
                OnNext(220, 4),
                OnNext(221, 3),
                OnNext(222, 2),
                OnNext(223, 5),
                OnCompleted<int>(224)
            );

            var ex = new Exception();

            var res = scheduler.Start(() =>
                xs.SelectMany((x, _) => Throw<IObservable<long>>(ex), (x, _, y, __) => x * 10 + (int)y)
            );

            res.Messages.AssertEqual(
                OnError<int>(220, ex)
            );

            xs.Subscriptions.AssertEqual(
                Subscribe(200, 220)
            );
        }

        [Fact]
        public void SelectManyWithIndex_QueryOperator_ThrowResult()
        {
            var scheduler = new TestScheduler();

            var xs = scheduler.CreateHotObservable(
                OnNext(220, 4),
                OnNext(221, 3),
                OnNext(222, 2),
                OnNext(223, 5),
                OnCompleted<int>(224)
            );

            var ex = new Exception();

            var res = scheduler.Start(() =>
                xs.SelectMany((x, _) => Observable.Interval(TimeSpan.FromTicks(1), scheduler).Take(x), (x, _, y, __) => Throw<int>(ex))
            );

            res.Messages.AssertEqual(
                OnError<int>(221, ex)
            );

            xs.Subscriptions.AssertEqual(
                Subscribe(200, 221)
            );
        }


        [Fact]
        public void SelectMany_Triple_ArgumentChecking()
        {
            ReactiveAssert.Throws<ArgumentNullException>(() => Observable.SelectMany(null, DummyFunc<int, IObservable<int>>.Instance, DummyFunc<Exception, IObservable<int>>.Instance, DummyFunc<IObservable<int>>.Instance));
            ReactiveAssert.Throws<ArgumentNullException>(() => Observable.SelectMany(DummyObservable<int>.Instance, (Func<int, IObservable<int>>)null, DummyFunc<Exception, IObservable<int>>.Instance, DummyFunc<IObservable<int>>.Instance));
            ReactiveAssert.Throws<ArgumentNullException>(() => Observable.SelectMany(DummyObservable<int>.Instance, DummyFunc<int, IObservable<int>>.Instance, null, DummyFunc<IObservable<int>>.Instance));
            ReactiveAssert.Throws<ArgumentNullException>(() => Observable.SelectMany(DummyObservable<int>.Instance, DummyFunc<int, IObservable<int>>.Instance, DummyFunc<Exception, IObservable<int>>.Instance, null));
            ReactiveAssert.Throws<ArgumentNullException>(() => Observable.SelectMany(DummyObservable<int>.Instance, DummyFunc<int, IObservable<int>>.Instance, DummyFunc<Exception, IObservable<int>>.Instance, DummyFunc<IObservable<int>>.Instance).Subscribe(null));
        }

        [Fact]
        public void SelectMany_Triple_Identity()
        {
            var scheduler = new TestScheduler();

            var xs = scheduler.CreateHotObservable(
                OnNext(300, 0),
                OnNext(301, 1),
                OnNext(302, 2),
                OnNext(303, 3),
                OnNext(304, 4),
                OnCompleted<int>(305)
            );

            var res = scheduler.Start(() =>
                xs.SelectMany(
                    x => Observable.Return(x, scheduler),
                    ex => Observable.Throw<int>(ex, scheduler),
                    () => Observable.Empty<int>(scheduler)
                )
            );

            res.Messages.AssertEqual(
                OnNext(301, 0),
                OnNext(302, 1),
                OnNext(303, 2),
                OnNext(304, 3),
                OnNext(305, 4),
                OnCompleted<int>(306)
            );

            xs.Subscriptions.AssertEqual(
                Subscribe(200, 305)
            );
        }

        [Fact]
        public void SelectMany_Triple_InnersWithTiming1()
        {
            var scheduler = new TestScheduler();

            var xs = scheduler.CreateHotObservable(
                OnNext(300, 0),
                OnNext(301, 1),
                OnNext(302, 2),
                OnNext(303, 3),
                OnNext(304, 4),
                OnCompleted<int>(305)
            );

            var ysn = scheduler.CreateColdObservable(
                OnNext(10, 10),
                OnNext(20, 11),
                OnNext(30, 12),
                OnCompleted<int>(40)
            );

            var yse = scheduler.CreateColdObservable(
                OnNext(0, 99),
                OnCompleted<int>(10)
            );

            var ysc = scheduler.CreateColdObservable(
                OnNext(10, 42),
                OnCompleted<int>(20)
            );

            var res = scheduler.Start(() =>
                xs.SelectMany(
                    x => ysn,
                    ex => yse,
                    () => ysc
                )
            );

            res.Messages.AssertEqual(
                OnNext(310, 10),
                OnNext(311, 10),
                OnNext(312, 10),
                OnNext(313, 10),
                OnNext(314, 10),
                OnNext(315, 42),
                OnNext(320, 11),
                OnNext(321, 11),
                OnNext(322, 11),
                OnNext(323, 11),
                OnNext(324, 11),
                OnNext(330, 12),
                OnNext(331, 12),
                OnNext(332, 12),
                OnNext(333, 12),
                OnNext(334, 12),
                OnCompleted<int>(344)
            );

            xs.Subscriptions.AssertEqual(
                Subscribe(200, 305)
            );

            ysn.Subscriptions.AssertEqual(
                Subscribe(300, 340),
                Subscribe(301, 341),
                Subscribe(302, 342),
                Subscribe(303, 343),
                Subscribe(304, 344)
            );

            yse.Subscriptions.AssertEqual(
            );

            ysc.Subscriptions.AssertEqual(
                Subscribe(305, 325)
            );
        }

        [Fact]
        public void SelectMany_Triple_InnersWithTiming2()
        {
            var scheduler = new TestScheduler();

            var xs = scheduler.CreateHotObservable(
                OnNext(300, 0),
                OnNext(301, 1),
                OnNext(302, 2),
                OnNext(303, 3),
                OnNext(304, 4),
                OnCompleted<int>(305)
            );

            var ysn = scheduler.CreateColdObservable(
                OnNext(10, 10),
                OnNext(20, 11),
                OnNext(30, 12),
                OnCompleted<int>(40)
            );

            var yse = scheduler.CreateColdObservable(
                OnNext(0, 99),
                OnCompleted<int>(10)
            );

            var ysc = scheduler.CreateColdObservable(
                OnNext(10, 42),
                OnCompleted<int>(50)
            );

            var res = scheduler.Start(() =>
                xs.SelectMany(
                    x => ysn,
                    ex => yse,
                    () => ysc
                )
            );

            res.Messages.AssertEqual(
                OnNext(310, 10),
                OnNext(311, 10),
                OnNext(312, 10),
                OnNext(313, 10),
                OnNext(314, 10),
                OnNext(315, 42),
                OnNext(320, 11),
                OnNext(321, 11),
                OnNext(322, 11),
                OnNext(323, 11),
                OnNext(324, 11),
                OnNext(330, 12),
                OnNext(331, 12),
                OnNext(332, 12),
                OnNext(333, 12),
                OnNext(334, 12),
                OnCompleted<int>(355)
            );

            xs.Subscriptions.AssertEqual(
                Subscribe(200, 305)
            );

            ysn.Subscriptions.AssertEqual(
                Subscribe(300, 340),
                Subscribe(301, 341),
                Subscribe(302, 342),
                Subscribe(303, 343),
                Subscribe(304, 344)
            );

            yse.Subscriptions.AssertEqual(
            );

            ysc.Subscriptions.AssertEqual(
                Subscribe(305, 355)
            );
        }

        [Fact]
        public void SelectMany_Triple_InnersWithTiming3()
        {
            var scheduler = new TestScheduler();

            var xs = scheduler.CreateHotObservable(
                OnNext(300, 0),
                OnNext(400, 1),
                OnNext(500, 2),
                OnNext(600, 3),
                OnNext(700, 4),
                OnCompleted<int>(800)
            );

            var ysn = scheduler.CreateColdObservable(
                OnNext(10, 10),
                OnNext(20, 11),
                OnNext(30, 12),
                OnCompleted<int>(40)
            );

            var yse = scheduler.CreateColdObservable(
                OnNext(0, 99),
                OnCompleted<int>(10)
            );

            var ysc = scheduler.CreateColdObservable(
                OnNext(10, 42),
                OnCompleted<int>(100)
            );

            var res = scheduler.Start(() =>
                xs.SelectMany(
                    x => ysn,
                    ex => yse,
                    () => ysc
                )
            );

            res.Messages.AssertEqual(
                OnNext(310, 10),
                OnNext(320, 11),
                OnNext(330, 12),
                OnNext(410, 10),
                OnNext(420, 11),
                OnNext(430, 12),
                OnNext(510, 10),
                OnNext(520, 11),
                OnNext(530, 12),
                OnNext(610, 10),
                OnNext(620, 11),
                OnNext(630, 12),
                OnNext(710, 10),
                OnNext(720, 11),
                OnNext(730, 12),
                OnNext(810, 42),
                OnCompleted<int>(900)
            );

            xs.Subscriptions.AssertEqual(
                Subscribe(200, 800)
            );

            ysn.Subscriptions.AssertEqual(
                Subscribe(300, 340),
                Subscribe(400, 440),
                Subscribe(500, 540),
                Subscribe(600, 640),
                Subscribe(700, 740)
            );

            yse.Subscriptions.AssertEqual(
            );

            ysc.Subscriptions.AssertEqual(
                Subscribe(800, 900)
            );
        }

        [Fact]
        public void SelectMany_Triple_Error_Identity()
        {
            var scheduler = new TestScheduler();

            var ex = new Exception();

            var xs = scheduler.CreateHotObservable(
                OnNext(300, 0),
                OnNext(301, 1),
                OnNext(302, 2),
                OnNext(303, 3),
                OnNext(304, 4),
                OnError<int>(305, ex)
            );

            var res = scheduler.Start(() =>
                xs.SelectMany(
                    x => Observable.Return(x, scheduler),
                    ex1 => Observable.Throw<int>(ex1, scheduler),
                    () => Observable.Empty<int>(scheduler)
                )
            );

            res.Messages.AssertEqual(
                OnNext(301, 0),
                OnNext(302, 1),
                OnNext(303, 2),
                OnNext(304, 3),
                OnNext(305, 4),
                OnError<int>(306, ex)
            );

            xs.Subscriptions.AssertEqual(
                Subscribe(200, 305)
            );
        }

        [Fact]
        public void SelectMany_Triple_SelectMany()
        {
            var scheduler = new TestScheduler();

            var xs = scheduler.CreateHotObservable(
                OnNext(300, 0),
                OnNext(301, 1),
                OnNext(302, 2),
                OnNext(303, 3),
                OnNext(304, 4),
                OnCompleted<int>(305)
            );

            var res = scheduler.Start(() =>
                xs.SelectMany(
                    x => Observable.Repeat(x, x, scheduler),
                    ex => Observable.Throw<int>(ex, scheduler),
                    () => Observable.Empty<int>(scheduler)
                )
            );

            res.Messages.AssertEqual(
                OnNext(302, 1),
                OnNext(303, 2),
                OnNext(304, 3),
                OnNext(304, 2),
                OnNext(305, 4),
                OnNext(305, 3),
                OnNext(306, 4),
                OnNext(306, 3),
                OnNext(307, 4),
                OnNext(308, 4),
                OnCompleted<int>(308)
            );

            xs.Subscriptions.AssertEqual(
                Subscribe(200, 305)
            );
        }


        [Fact]
        public void SelectMany_Triple_Concat()
        {
            var scheduler = new TestScheduler();

            var xs = scheduler.CreateHotObservable(
                OnNext(300, 0),
                OnNext(301, 1),
                OnNext(302, 2),
                OnNext(303, 3),
                OnNext(304, 4),
                OnCompleted<int>(305)
            );

            var res = scheduler.Start(() =>
                xs.SelectMany(
                    x => Observable.Return(x, scheduler),
                    ex => Observable.Throw<int>(ex, scheduler),
                    () => Observable.Range(1, 3, scheduler)
                )
            );

            res.Messages.AssertEqual(
                OnNext(301, 0),
                OnNext(302, 1),
                OnNext(303, 2),
                OnNext(304, 3),
                OnNext(305, 4),
                OnNext(306, 1),
                OnNext(307, 2),
                OnNext(308, 3),
                OnCompleted<int>(309)
            );

            xs.Subscriptions.AssertEqual(
                Subscribe(200, 305)
            );
        }

        [Fact]
        public void SelectMany_Triple_Catch()
        {
            var scheduler = new TestScheduler();

            var xs = scheduler.CreateHotObservable(
                OnNext(300, 0),
                OnNext(301, 1),
                OnNext(302, 2),
                OnNext(303, 3),
                OnNext(304, 4),
                OnCompleted<int>(305)
            );

            var res = scheduler.Start(() =>
                xs.SelectMany(
                    x => Observable.Return(x, scheduler),
                    ex => Observable.Range(1, 3, scheduler),
                    () => Observable.Empty<int>(scheduler)
                )
            );

            res.Messages.AssertEqual(
                OnNext(301, 0),
                OnNext(302, 1),
                OnNext(303, 2),
                OnNext(304, 3),
                OnNext(305, 4),
                OnCompleted<int>(306)
            );

            xs.Subscriptions.AssertEqual(
                Subscribe(200, 305)
            );
        }

        [Fact]
        public void SelectMany_Triple_Error_Catch()
        {
            var scheduler = new TestScheduler();

            var xs = scheduler.CreateHotObservable(
                OnNext(300, 0),
                OnNext(301, 1),
                OnNext(302, 2),
                OnNext(303, 3),
                OnNext(304, 4),
                OnError<int>(305, new Exception())
            );

            var res = scheduler.Start(() =>
                xs.SelectMany(
                    x => Observable.Return(x, scheduler),
                    ex => Observable.Range(1, 3, scheduler),
                    () => Observable.Empty<int>(scheduler)
                )
            );

            res.Messages.AssertEqual(
                OnNext(301, 0),
                OnNext(302, 1),
                OnNext(303, 2),
                OnNext(304, 3),
                OnNext(305, 4),
                OnNext(306, 1),
                OnNext(307, 2),
                OnNext(308, 3),
                OnCompleted<int>(309)
            );

            xs.Subscriptions.AssertEqual(
                Subscribe(200, 305)
            );
        }

        [Fact]
        public void SelectMany_Triple_All()
        {
            var scheduler = new TestScheduler();

            var xs = scheduler.CreateHotObservable(
                OnNext(300, 0),
                OnNext(301, 1),
                OnNext(302, 2),
                OnNext(303, 3),
                OnNext(304, 4),
                OnCompleted<int>(305)
            );

            var res = scheduler.Start(() =>
                xs.SelectMany(
                    x => Observable.Repeat(x, x, scheduler),
                    ex => Observable.Repeat(0, 2, scheduler),
                    () => Observable.Repeat(-1, 2, scheduler)
                )
            );

            res.Messages.AssertEqual(
                OnNext(302, 1),
                OnNext(303, 2),
                OnNext(304, 3),
                OnNext(304, 2),
                OnNext(305, 4),
                OnNext(305, 3),
                OnNext(306, -1),
                OnNext(306, 4),
                OnNext(306, 3),
                OnNext(307, -1),
                OnNext(307, 4),
                OnNext(308, 4),
                OnCompleted<int>(308)
            );

            xs.Subscriptions.AssertEqual(
                Subscribe(200, 305)
            );
        }

        [Fact]
        public void SelectMany_Triple_Error_All()
        {
            var scheduler = new TestScheduler();

            var xs = scheduler.CreateHotObservable(
                OnNext(300, 0),
                OnNext(301, 1),
                OnNext(302, 2),
                OnNext(303, 3),
                OnNext(304, 4),
                OnError<int>(305, new Exception())
            );

            var res = scheduler.Start(() =>
                xs.SelectMany(
                    x => Observable.Repeat(x, x, scheduler),
                    ex => Observable.Repeat(0, 2, scheduler),
                    () => Observable.Repeat(-1, 2, scheduler)
                )
            );

            res.Messages.AssertEqual(
                OnNext(302, 1),
                OnNext(303, 2),
                OnNext(304, 3),
                OnNext(304, 2),
                OnNext(305, 4),
                OnNext(305, 3),
                OnNext(306, 0),
                OnNext(306, 4),
                OnNext(306, 3),
                OnNext(307, 0),
                OnNext(307, 4),
                OnNext(308, 4),
                OnCompleted<int>(308)
            );

            xs.Subscriptions.AssertEqual(
                Subscribe(200, 305)
            );
        }

        [Fact]
        public void SelectMany_Triple_All_Dispose()
        {
            var scheduler = new TestScheduler();

            var xs = scheduler.CreateHotObservable(
                OnNext(300, 0),
                OnNext(301, 1),
                OnNext(302, 2),
                OnNext(303, 3),
                OnNext(304, 4),
                OnCompleted<int>(305)
            );

            var res = scheduler.Start(() =>
                xs.SelectMany(
                    x => Observable.Repeat(x, x, scheduler),
                    ex => Observable.Repeat(0, 2, scheduler),
                    () => Observable.Repeat(-1, 2, scheduler)
                ),
                307
            );

            res.Messages.AssertEqual(
                OnNext(302, 1),
                OnNext(303, 2),
                OnNext(304, 3),
                OnNext(304, 2),
                OnNext(305, 4),
                OnNext(305, 3),
                OnNext(306, -1),
                OnNext(306, 4),
                OnNext(306, 3)
            );

            xs.Subscriptions.AssertEqual(
                Subscribe(200, 305)
            );
        }

        [Fact]
        public void SelectMany_Triple_All_Dispose_Before_First()
        {
            var scheduler = new TestScheduler();

            var xs = scheduler.CreateHotObservable(
                OnNext(300, 0),
                OnNext(301, 1),
                OnNext(302, 2),
                OnNext(303, 3),
                OnNext(304, 4),
                OnCompleted<int>(305)
            );

            var res = scheduler.Start(() =>
                xs.SelectMany(
                    x => Observable.Repeat(x, x, scheduler),
                    ex => Observable.Repeat(0, 2, scheduler),
                    () => Observable.Repeat(-1, 2, scheduler)
                ),
                304
            );

            res.Messages.AssertEqual(
                OnNext(302, 1),
                OnNext(303, 2)
            );

            xs.Subscriptions.AssertEqual(
                Subscribe(200, 304)
            );
        }

        [Fact]
        public void SelectMany_Triple_OnNextThrow()
        {
            var scheduler = new TestScheduler();

            var xs = scheduler.CreateHotObservable(
                OnNext(300, 0),
                OnNext(301, 1),
                OnNext(302, 2),
                OnNext(303, 3),
                OnNext(304, 4),
                OnCompleted<int>(305)
            );

            var ex = new Exception();

            var res = scheduler.Start(() =>
                xs.SelectMany(
                    x => Throw<IObservable<int>>(ex),
                    ex1 => Observable.Repeat(0, 2, scheduler),
                    () => Observable.Repeat(-1, 2, scheduler)
                )
            );

            res.Messages.AssertEqual(
                OnError<int>(300, ex)
            );

            xs.Subscriptions.AssertEqual(
                Subscribe(200, 300)
            );
        }

        [Fact]
        public void SelectMany_Triple_OnErrorThrow()
        {
            var scheduler = new TestScheduler();

            var ex = new Exception();

            var xs = scheduler.CreateHotObservable(
                OnNext(300, 0),
                OnNext(301, 1),
                OnNext(302, 2),
                OnNext(303, 3),
                OnNext(304, 4),
                OnError<int>(305, new Exception())
            );

            var res = scheduler.Start(() =>
                xs.SelectMany(
                    x => Observable.Repeat(x, x, scheduler),
                    ex1 => Throw<IObservable<int>>(ex),
                    () => Observable.Repeat(-1, 2, scheduler)
                )
            );

            res.Messages.AssertEqual(
                OnNext(302, 1),
                OnNext(303, 2),
                OnNext(304, 3),
                OnNext(304, 2),
                OnError<int>(305, ex)
            );

            xs.Subscriptions.AssertEqual(
                Subscribe(200, 305)
            );
        }

        [Fact]
        public void SelectMany_Triple_OnCompletedThrow()
        {
            var scheduler = new TestScheduler();

            var xs = scheduler.CreateHotObservable(
                OnNext(300, 0),
                OnNext(301, 1),
                OnNext(302, 2),
                OnNext(303, 3),
                OnNext(304, 4),
                OnCompleted<int>(305)
            );

            var ex = new Exception();

            var res = scheduler.Start(() =>
                xs.SelectMany(
                    x => Observable.Repeat(x, x, scheduler),
                    ex1 => Observable.Repeat(0, 2, scheduler),
                    () => Throw<IObservable<int>>(ex)
                )
            );

            res.Messages.AssertEqual(
                OnNext(302, 1),
                OnNext(303, 2),
                OnNext(304, 3),
                OnNext(304, 2),
                OnError<int>(305, ex)
            );

            xs.Subscriptions.AssertEqual(
                Subscribe(200, 305)
            );
        }

        [Fact]
        public void SelectManyWithIndex_Triple_ArgumentChecking()
        {
            ReactiveAssert.Throws<ArgumentNullException>(() => Observable.SelectMany(null, DummyFunc<int, int, IObservable<int>>.Instance, DummyFunc<Exception, IObservable<int>>.Instance, DummyFunc<IObservable<int>>.Instance));
            ReactiveAssert.Throws<ArgumentNullException>(() => Observable.SelectMany(DummyObservable<int>.Instance, (Func<int, int, IObservable<int>>)null, DummyFunc<Exception, IObservable<int>>.Instance, DummyFunc<IObservable<int>>.Instance));
            ReactiveAssert.Throws<ArgumentNullException>(() => Observable.SelectMany(DummyObservable<int>.Instance, DummyFunc<int, int, IObservable<int>>.Instance, null, DummyFunc<IObservable<int>>.Instance));
            ReactiveAssert.Throws<ArgumentNullException>(() => Observable.SelectMany(DummyObservable<int>.Instance, DummyFunc<int, int, IObservable<int>>.Instance, DummyFunc<Exception, IObservable<int>>.Instance, null));
            ReactiveAssert.Throws<ArgumentNullException>(() => Observable.SelectMany(DummyObservable<int>.Instance, DummyFunc<int, int, IObservable<int>>.Instance, DummyFunc<Exception, IObservable<int>>.Instance, DummyFunc<IObservable<int>>.Instance).Subscribe(null));
        }

        [Fact]
        public void SelectManyWithIndex_Triple_Index()
        {
            var scheduler = new TestScheduler();

            var xs = scheduler.CreateHotObservable(
                OnNext(300, 0),
                OnNext(301, 1),
                OnNext(302, 2),
                OnNext(303, 3),
                OnNext(304, 4),
                OnCompleted<int>(305)
            );

            var witness = new { x = 0, i = 0 };

            var res = scheduler.Start(() =>
                xs.SelectMany(
                    (x, i) => Observable.Return(new { x, i }, scheduler),
                    ex => Observable.Throw(ex, scheduler, witness),
                    () => Observable.Empty(scheduler, witness)
                )
            );

            res.Messages.AssertEqual(
                OnNext(301, new { x = 0, i = 0 }),
                OnNext(302, new { x = 1, i = 1 }),
                OnNext(303, new { x = 2, i = 2 }),
                OnNext(304, new { x = 3, i = 3 }),
                OnNext(305, new { x = 4, i = 4 }),
                OnCompleted(306, witness)
            );

            xs.Subscriptions.AssertEqual(
                Subscribe(200, 305)
            );
        }

        [Fact]
        public void SelectManyWithIndex_Triple_Identity()
        {
            var scheduler = new TestScheduler();

            var xs = scheduler.CreateHotObservable(
                OnNext(300, 0),
                OnNext(301, 1),
                OnNext(302, 2),
                OnNext(303, 3),
                OnNext(304, 4),
                OnCompleted<int>(305)
            );

            var res = scheduler.Start(() =>
                xs.SelectMany(
                    (x, _) => Observable.Return(x, scheduler),
                    ex => Observable.Throw<int>(ex, scheduler),
                    () => Observable.Empty<int>(scheduler)
                )
            );

            res.Messages.AssertEqual(
                OnNext(301, 0),
                OnNext(302, 1),
                OnNext(303, 2),
                OnNext(304, 3),
                OnNext(305, 4),
                OnCompleted<int>(306)
            );

            xs.Subscriptions.AssertEqual(
                Subscribe(200, 305)
            );
        }

        [Fact]
        public void SelectManyWithIndex_Triple_InnersWithTiming1()
        {
            var scheduler = new TestScheduler();

            var xs = scheduler.CreateHotObservable(
                OnNext(300, 0),
                OnNext(301, 1),
                OnNext(302, 2),
                OnNext(303, 3),
                OnNext(304, 4),
                OnCompleted<int>(305)
            );

            var ysn = scheduler.CreateColdObservable(
                OnNext(10, 10),
                OnNext(20, 11),
                OnNext(30, 12),
                OnCompleted<int>(40)
            );

            var yse = scheduler.CreateColdObservable(
                OnNext(0, 99),
                OnCompleted<int>(10)
            );

            var ysc = scheduler.CreateColdObservable(
                OnNext(10, 42),
                OnCompleted<int>(20)
            );

            var res = scheduler.Start(() =>
                xs.SelectMany(
                    (x, _) => ysn,
                    ex => yse,
                    () => ysc
                )
            );

            res.Messages.AssertEqual(
                OnNext(310, 10),
                OnNext(311, 10),
                OnNext(312, 10),
                OnNext(313, 10),
                OnNext(314, 10),
                OnNext(315, 42),
                OnNext(320, 11),
                OnNext(321, 11),
                OnNext(322, 11),
                OnNext(323, 11),
                OnNext(324, 11),
                OnNext(330, 12),
                OnNext(331, 12),
                OnNext(332, 12),
                OnNext(333, 12),
                OnNext(334, 12),
                OnCompleted<int>(344)
            );

            xs.Subscriptions.AssertEqual(
                Subscribe(200, 305)
            );

            ysn.Subscriptions.AssertEqual(
                Subscribe(300, 340),
                Subscribe(301, 341),
                Subscribe(302, 342),
                Subscribe(303, 343),
                Subscribe(304, 344)
            );

            yse.Subscriptions.AssertEqual(
            );

            ysc.Subscriptions.AssertEqual(
                Subscribe(305, 325)
            );
        }

        [Fact]
        public void SelectManyWithIndex_Triple_InnersWithTiming2()
        {
            var scheduler = new TestScheduler();

            var xs = scheduler.CreateHotObservable(
                OnNext(300, 0),
                OnNext(301, 1),
                OnNext(302, 2),
                OnNext(303, 3),
                OnNext(304, 4),
                OnCompleted<int>(305)
            );

            var ysn = scheduler.CreateColdObservable(
                OnNext(10, 10),
                OnNext(20, 11),
                OnNext(30, 12),
                OnCompleted<int>(40)
            );

            var yse = scheduler.CreateColdObservable(
                OnNext(0, 99),
                OnCompleted<int>(10)
            );

            var ysc = scheduler.CreateColdObservable(
                OnNext(10, 42),
                OnCompleted<int>(50)
            );

            var res = scheduler.Start(() =>
                xs.SelectMany(
                    (x, _) => ysn,
                    ex => yse,
                    () => ysc
                )
            );

            res.Messages.AssertEqual(
                OnNext(310, 10),
                OnNext(311, 10),
                OnNext(312, 10),
                OnNext(313, 10),
                OnNext(314, 10),
                OnNext(315, 42),
                OnNext(320, 11),
                OnNext(321, 11),
                OnNext(322, 11),
                OnNext(323, 11),
                OnNext(324, 11),
                OnNext(330, 12),
                OnNext(331, 12),
                OnNext(332, 12),
                OnNext(333, 12),
                OnNext(334, 12),
                OnCompleted<int>(355)
            );

            xs.Subscriptions.AssertEqual(
                Subscribe(200, 305)
            );

            ysn.Subscriptions.AssertEqual(
                Subscribe(300, 340),
                Subscribe(301, 341),
                Subscribe(302, 342),
                Subscribe(303, 343),
                Subscribe(304, 344)
            );

            yse.Subscriptions.AssertEqual(
            );

            ysc.Subscriptions.AssertEqual(
                Subscribe(305, 355)
            );
        }

        [Fact]
        public void SelectManyWithIndex_Triple_InnersWithTiming3()
        {
            var scheduler = new TestScheduler();

            var xs = scheduler.CreateHotObservable(
                OnNext(300, 0),
                OnNext(400, 1),
                OnNext(500, 2),
                OnNext(600, 3),
                OnNext(700, 4),
                OnCompleted<int>(800)
            );

            var ysn = scheduler.CreateColdObservable(
                OnNext(10, 10),
                OnNext(20, 11),
                OnNext(30, 12),
                OnCompleted<int>(40)
            );

            var yse = scheduler.CreateColdObservable(
                OnNext(0, 99),
                OnCompleted<int>(10)
            );

            var ysc = scheduler.CreateColdObservable(
                OnNext(10, 42),
                OnCompleted<int>(100)
            );

            var res = scheduler.Start(() =>
                xs.SelectMany(
                    (x, _) => ysn,
                    ex => yse,
                    () => ysc
                )
            );

            res.Messages.AssertEqual(
                OnNext(310, 10),
                OnNext(320, 11),
                OnNext(330, 12),
                OnNext(410, 10),
                OnNext(420, 11),
                OnNext(430, 12),
                OnNext(510, 10),
                OnNext(520, 11),
                OnNext(530, 12),
                OnNext(610, 10),
                OnNext(620, 11),
                OnNext(630, 12),
                OnNext(710, 10),
                OnNext(720, 11),
                OnNext(730, 12),
                OnNext(810, 42),
                OnCompleted<int>(900)
            );

            xs.Subscriptions.AssertEqual(
                Subscribe(200, 800)
            );

            ysn.Subscriptions.AssertEqual(
                Subscribe(300, 340),
                Subscribe(400, 440),
                Subscribe(500, 540),
                Subscribe(600, 640),
                Subscribe(700, 740)
            );

            yse.Subscriptions.AssertEqual(
            );

            ysc.Subscriptions.AssertEqual(
                Subscribe(800, 900)
            );
        }

        [Fact]
        public void SelectManyWithIndex_Triple_Error_Identity()
        {
            var scheduler = new TestScheduler();

            var ex = new Exception();

            var xs = scheduler.CreateHotObservable(
                OnNext(300, 0),
                OnNext(301, 1),
                OnNext(302, 2),
                OnNext(303, 3),
                OnNext(304, 4),
                OnError<int>(305, ex)
            );

            var res = scheduler.Start(() =>
                xs.SelectMany(
                    (x, _) => Observable.Return(x, scheduler),
                    ex1 => Observable.Throw<int>(ex1, scheduler),
                    () => Observable.Empty<int>(scheduler)
                )
            );

            res.Messages.AssertEqual(
                OnNext(301, 0),
                OnNext(302, 1),
                OnNext(303, 2),
                OnNext(304, 3),
                OnNext(305, 4),
                OnError<int>(306, ex)
            );

            xs.Subscriptions.AssertEqual(
                Subscribe(200, 305)
            );
        }

        [Fact]
        public void SelectManyWithIndex_Triple_SelectMany()
        {
            var scheduler = new TestScheduler();

            var xs = scheduler.CreateHotObservable(
                OnNext(300, 0),
                OnNext(301, 1),
                OnNext(302, 2),
                OnNext(303, 3),
                OnNext(304, 4),
                OnCompleted<int>(305)
            );

            var res = scheduler.Start(() =>
                xs.SelectMany(
                    (x, _) => Observable.Repeat(x, x, scheduler),
                    ex => Observable.Throw<int>(ex, scheduler),
                    () => Observable.Empty<int>(scheduler)
                )
            );

            res.Messages.AssertEqual(
                OnNext(302, 1),
                OnNext(303, 2),
                OnNext(304, 3),
                OnNext(304, 2),
                OnNext(305, 4),
                OnNext(305, 3),
                OnNext(306, 4),
                OnNext(306, 3),
                OnNext(307, 4),
                OnNext(308, 4),
                OnCompleted<int>(308)
            );

            xs.Subscriptions.AssertEqual(
                Subscribe(200, 305)
            );
        }


        [Fact]
        public void SelectManyWithIndex_Triple_Concat()
        {
            var scheduler = new TestScheduler();

            var xs = scheduler.CreateHotObservable(
                OnNext(300, 0),
                OnNext(301, 1),
                OnNext(302, 2),
                OnNext(303, 3),
                OnNext(304, 4),
                OnCompleted<int>(305)
            );

            var res = scheduler.Start(() =>
                xs.SelectMany(
                    (x, _) => Observable.Return(x, scheduler),
                    ex => Observable.Throw<int>(ex, scheduler),
                    () => Observable.Range(1, 3, scheduler)
                )
            );

            res.Messages.AssertEqual(
                OnNext(301, 0),
                OnNext(302, 1),
                OnNext(303, 2),
                OnNext(304, 3),
                OnNext(305, 4),
                OnNext(306, 1),
                OnNext(307, 2),
                OnNext(308, 3),
                OnCompleted<int>(309)
            );

            xs.Subscriptions.AssertEqual(
                Subscribe(200, 305)
            );
        }

        [Fact]
        public void SelectManyWithIndex_Triple_Catch()
        {
            var scheduler = new TestScheduler();

            var xs = scheduler.CreateHotObservable(
                OnNext(300, 0),
                OnNext(301, 1),
                OnNext(302, 2),
                OnNext(303, 3),
                OnNext(304, 4),
                OnCompleted<int>(305)
            );

            var res = scheduler.Start(() =>
                xs.SelectMany(
                    (x, _) => Observable.Return(x, scheduler),
                    ex => Observable.Range(1, 3, scheduler),
                    () => Observable.Empty<int>(scheduler)
                )
            );

            res.Messages.AssertEqual(
                OnNext(301, 0),
                OnNext(302, 1),
                OnNext(303, 2),
                OnNext(304, 3),
                OnNext(305, 4),
                OnCompleted<int>(306)
            );

            xs.Subscriptions.AssertEqual(
                Subscribe(200, 305)
            );
        }

        [Fact]
        public void SelectManyWithIndex_Triple_Error_Catch()
        {
            var scheduler = new TestScheduler();

            var xs = scheduler.CreateHotObservable(
                OnNext(300, 0),
                OnNext(301, 1),
                OnNext(302, 2),
                OnNext(303, 3),
                OnNext(304, 4),
                OnError<int>(305, new Exception())
            );

            var res = scheduler.Start(() =>
                xs.SelectMany(
                    (x, _) => Observable.Return(x, scheduler),
                    ex => Observable.Range(1, 3, scheduler),
                    () => Observable.Empty<int>(scheduler)
                )
            );

            res.Messages.AssertEqual(
                OnNext(301, 0),
                OnNext(302, 1),
                OnNext(303, 2),
                OnNext(304, 3),
                OnNext(305, 4),
                OnNext(306, 1),
                OnNext(307, 2),
                OnNext(308, 3),
                OnCompleted<int>(309)
            );

            xs.Subscriptions.AssertEqual(
                Subscribe(200, 305)
            );
        }

        [Fact]
        public void SelectManyWithIndex_Triple_All()
        {
            var scheduler = new TestScheduler();

            var xs = scheduler.CreateHotObservable(
                OnNext(300, 0),
                OnNext(301, 1),
                OnNext(302, 2),
                OnNext(303, 3),
                OnNext(304, 4),
                OnCompleted<int>(305)
            );

            var res = scheduler.Start(() =>
                xs.SelectMany(
                    (x, _) => Observable.Repeat(x, x, scheduler),
                    ex => Observable.Repeat(0, 2, scheduler),
                    () => Observable.Repeat(-1, 2, scheduler)
                )
            );

            res.Messages.AssertEqual(
                OnNext(302, 1),
                OnNext(303, 2),
                OnNext(304, 3),
                OnNext(304, 2),
                OnNext(305, 4),
                OnNext(305, 3),
                OnNext(306, -1),
                OnNext(306, 4),
                OnNext(306, 3),
                OnNext(307, -1),
                OnNext(307, 4),
                OnNext(308, 4),
                OnCompleted<int>(308)
            );

            xs.Subscriptions.AssertEqual(
                Subscribe(200, 305)
            );
        }

        [Fact]
        public void SelectManyWithIndex_Triple_Error_All()
        {
            var scheduler = new TestScheduler();

            var xs = scheduler.CreateHotObservable(
                OnNext(300, 0),
                OnNext(301, 1),
                OnNext(302, 2),
                OnNext(303, 3),
                OnNext(304, 4),
                OnError<int>(305, new Exception())
            );

            var res = scheduler.Start(() =>
                xs.SelectMany(
                    (x, _) => Observable.Repeat(x, x, scheduler),
                    ex => Observable.Repeat(0, 2, scheduler),
                    () => Observable.Repeat(-1, 2, scheduler)
                )
            );

            res.Messages.AssertEqual(
                OnNext(302, 1),
                OnNext(303, 2),
                OnNext(304, 3),
                OnNext(304, 2),
                OnNext(305, 4),
                OnNext(305, 3),
                OnNext(306, 0),
                OnNext(306, 4),
                OnNext(306, 3),
                OnNext(307, 0),
                OnNext(307, 4),
                OnNext(308, 4),
                OnCompleted<int>(308)
            );

            xs.Subscriptions.AssertEqual(
                Subscribe(200, 305)
            );
        }

        [Fact]
        public void SelectManyWithIndex_Triple_All_Dispose()
        {
            var scheduler = new TestScheduler();

            var xs = scheduler.CreateHotObservable(
                OnNext(300, 0),
                OnNext(301, 1),
                OnNext(302, 2),
                OnNext(303, 3),
                OnNext(304, 4),
                OnCompleted<int>(305)
            );

            var res = scheduler.Start(() =>
                xs.SelectMany(
                    (x, _) => Observable.Repeat(x, x, scheduler),
                    ex => Observable.Repeat(0, 2, scheduler),
                    () => Observable.Repeat(-1, 2, scheduler)
                ),
                307
            );

            res.Messages.AssertEqual(
                OnNext(302, 1),
                OnNext(303, 2),
                OnNext(304, 3),
                OnNext(304, 2),
                OnNext(305, 4),
                OnNext(305, 3),
                OnNext(306, -1),
                OnNext(306, 4),
                OnNext(306, 3)
            );

            xs.Subscriptions.AssertEqual(
                Subscribe(200, 305)
            );
        }

        [Fact]
        public void SelectManyWithIndex_Triple_All_Dispose_Before_First()
        {
            var scheduler = new TestScheduler();

            var xs = scheduler.CreateHotObservable(
                OnNext(300, 0),
                OnNext(301, 1),
                OnNext(302, 2),
                OnNext(303, 3),
                OnNext(304, 4),
                OnCompleted<int>(305)
            );

            var res = scheduler.Start(() =>
                xs.SelectMany(
                    (x, _) => Observable.Repeat(x, x, scheduler),
                    ex => Observable.Repeat(0, 2, scheduler),
                    () => Observable.Repeat(-1, 2, scheduler)
                ),
                304
            );

            res.Messages.AssertEqual(
                OnNext(302, 1),
                OnNext(303, 2)
            );

            xs.Subscriptions.AssertEqual(
                Subscribe(200, 304)
            );
        }

        [Fact]
        public void SelectManyWithIndex_Triple_OnNextThrow()
        {
            var scheduler = new TestScheduler();

            var xs = scheduler.CreateHotObservable(
                OnNext(300, 0),
                OnNext(301, 1),
                OnNext(302, 2),
                OnNext(303, 3),
                OnNext(304, 4),
                OnCompleted<int>(305)
            );

            var ex = new Exception();

            var res = scheduler.Start(() =>
                xs.SelectMany(
                    (x, _) => Throw<IObservable<int>>(ex),
                    ex1 => Observable.Repeat(0, 2, scheduler),
                    () => Observable.Repeat(-1, 2, scheduler)
                )
            );

            res.Messages.AssertEqual(
                OnError<int>(300, ex)
            );

            xs.Subscriptions.AssertEqual(
                Subscribe(200, 300)
            );
        }

        [Fact]
        public void SelectManyWithIndex_Triple_OnErrorThrow()
        {
            var scheduler = new TestScheduler();

            var ex = new Exception();

            var xs = scheduler.CreateHotObservable(
                OnNext(300, 0),
                OnNext(301, 1),
                OnNext(302, 2),
                OnNext(303, 3),
                OnNext(304, 4),
                OnError<int>(305, new Exception())
            );

            var res = scheduler.Start(() =>
                xs.SelectMany(
                    (x, _) => Observable.Repeat(x, x, scheduler),
                    ex1 => Throw<IObservable<int>>(ex),
                    () => Observable.Repeat(-1, 2, scheduler)
                )
            );

            res.Messages.AssertEqual(
                OnNext(302, 1),
                OnNext(303, 2),
                OnNext(304, 3),
                OnNext(304, 2),
                OnError<int>(305, ex)
            );

            xs.Subscriptions.AssertEqual(
                Subscribe(200, 305)
            );
        }

        [Fact]
        public void SelectManyWithIndex_Triple_OnCompletedThrow()
        {
            var scheduler = new TestScheduler();

            var xs = scheduler.CreateHotObservable(
                OnNext(300, 0),
                OnNext(301, 1),
                OnNext(302, 2),
                OnNext(303, 3),
                OnNext(304, 4),
                OnCompleted<int>(305)
            );

            var ex = new Exception();

            var res = scheduler.Start(() =>
                xs.SelectMany(
                    (x, _) => Observable.Repeat(x, x, scheduler),
                    ex1 => Observable.Repeat(0, 2, scheduler),
                    () => Throw<IObservable<int>>(ex)
                )
            );

            res.Messages.AssertEqual(
                OnNext(302, 1),
                OnNext(303, 2),
                OnNext(304, 3),
                OnNext(304, 2),
                OnError<int>(305, ex)
            );

            xs.Subscriptions.AssertEqual(
                Subscribe(200, 305)
            );
        }

        [Fact]
        public void SelectMany_Task_ArgumentChecking()
        {
            var t = new Task<int>(() => 42);

            ReactiveAssert.Throws<ArgumentNullException>(() => Observable.SelectMany(default(IObservable<int>), x => t));
            ReactiveAssert.Throws<ArgumentNullException>(() => Observable.SelectMany(DummyObservable<int>.Instance, default(Func<int, Task<int>>)));

            ReactiveAssert.Throws<ArgumentNullException>(() => Observable.SelectMany(default, (int x, CancellationToken ct) => t));
            ReactiveAssert.Throws<ArgumentNullException>(() => Observable.SelectMany(DummyObservable<int>.Instance, default(Func<int, CancellationToken, Task<int>>)));

            ReactiveAssert.Throws<ArgumentNullException>(() => Observable.SelectMany(default(IObservable<int>), x => t, (x, y) => x));
            ReactiveAssert.Throws<ArgumentNullException>(() => Observable.SelectMany(DummyObservable<int>.Instance, default(Func<int, Task<int>>), (x, y) => x));
            ReactiveAssert.Throws<ArgumentNullException>(() => Observable.SelectMany(DummyObservable<int>.Instance, x => t, default(Func<int, int, int>)));

            ReactiveAssert.Throws<ArgumentNullException>(() => Observable.SelectMany(default(IObservable<int>), (x, ct) => t, (x, y) => x));
            ReactiveAssert.Throws<ArgumentNullException>(() => Observable.SelectMany(DummyObservable<int>.Instance, default(Func<int, CancellationToken, Task<int>>), (x, y) => x));
            ReactiveAssert.Throws<ArgumentNullException>(() => Observable.SelectMany(DummyObservable<int>.Instance, (x, ct) => t, default(Func<int, int, int>)));
        }

        [Fact]
        public void SelectMany_Task1()
        {
            var res = Observable.Range(0, 10).SelectMany(x => Task.Factory.StartNew(() => x + 1)).ToEnumerable();
            Assert.True(Enumerable.Range(0, 10).SelectMany(x => new[] { x + 1 }).SequenceEqual(res.OrderBy(x => x)));
        }

        [Fact]
        public void SelectMany_Task2()
        {
            var res = Observable.Range(0, 10).SelectMany((x, ct) => Task.Factory.StartNew(() => x + 1, ct)).ToEnumerable();
            Assert.True(Enumerable.Range(0, 10).SelectMany(x => new[] { x + 1 }).SequenceEqual(res.OrderBy(x => x)));
        }

        [Fact]
        public void SelectMany_Task_TaskThrows()
        {
            var ex = new Exception();

            var res = Observable.Range(0, 10).SelectMany(x => Task.Factory.StartNew(() =>
            {
                if (x > 5)
                {
                    throw ex;
                }

                return x + 1;
            })).ToEnumerable();

            ReactiveAssert.Throws(ex, () =>
            {
                foreach (var x in res)
                {
                    ;
                }
            });
        }

        [Fact]
        public void SelectMany_Task_SelectorThrows()
        {
            var ex = new Exception();

            var res = Observable.Range(0, 10).SelectMany(x =>
            {
                if (x > 5)
                {
                    throw ex;
                }

                return Task.Factory.StartNew(() => x + 1);
            }).ToEnumerable();

            ReactiveAssert.Throws(ex, () =>
            {
                foreach (var x in res)
                {
                    ;
                }
            });
        }

        [Fact]
        public void SelectMany_Task_ResultSelector1()
        {
            var res = Observable.Range(0, 10).SelectMany(x => Task.Factory.StartNew(() => x + 1), (x, y) => x + y).ToEnumerable();
            Assert.True(Enumerable.Range(0, 10).SelectMany(x => new[] { 2 * x + 1 }).SequenceEqual(res.OrderBy(x => x)));
        }

        [Fact]
        public void SelectMany_Task_ResultSelector2()
        {
            var res = Observable.Range(0, 10).SelectMany((x, ct) => Task.Factory.StartNew(() => x + 1, ct), (x, y) => x + y).ToEnumerable();
            Assert.True(Enumerable.Range(0, 10).SelectMany(x => new[] { 2 * x + 1 }).SequenceEqual(res.OrderBy(x => x)));
        }

        [Fact]
        public void SelectMany_Task_ResultSelectorThrows()
        {
            var ex = new Exception();

            var res = Observable.Range(0, 10).SelectMany(x => Task.Factory.StartNew(() => x + 1), (x, y) =>
            {
                if (x > 5)
                {
                    throw ex;
                }

                return x + y;
            }).ToEnumerable();

            ReactiveAssert.Throws(ex, () =>
            {
                foreach (var x in res)
                {
                    ;
                }
            });
        }

        [Fact]
        public void SelectMany_TaskWithCompletionSource_Simple_RanToCompletion_Async()
        {
            var tcss = new TaskCompletionSource<int>[2];
            tcss[0] = new TaskCompletionSource<int>();
            tcss[1] = new TaskCompletionSource<int>();

            var res = Observable.SelectMany(Observable.Range(0, 2), x => tcss[x].Task);

            var lst = new List<int>();

            var done = new ManualResetEvent(false);
            res.Subscribe(lst.Add, () => done.Set());

            tcss[0].SetResult(42);
            tcss[1].SetResult(43);

            done.WaitOne();

            lst.OrderBy(x => x).AssertEqual(new[] { 42, 43 });
        }

        [Fact]
        public void SelectMany_TaskWithCompletionSource_Simple_RanToCompletion_Sync()
        {
            var tcss = new TaskCompletionSource<int>[2];
            tcss[0] = new TaskCompletionSource<int>();
            tcss[1] = new TaskCompletionSource<int>();

            tcss[0].SetResult(42);
            tcss[1].SetResult(43);

            var res = Observable.SelectMany(Observable.Range(0, 2), x => tcss[x].Task);

            var lst = new List<int>();

            var done = new ManualResetEvent(false);
            res.Subscribe(lst.Add, () => done.Set());

            done.WaitOne();

            lst.OrderBy(x => x).AssertEqual(new[] { 42, 43 });
        }

        [Fact]
        public void SelectMany_TaskWithCompletionSource_Simple_Faulted_Async()
        {
            var tcss = new TaskCompletionSource<int>[3];
            tcss[0] = new TaskCompletionSource<int>();
            tcss[1] = new TaskCompletionSource<int>();
            tcss[2] = new TaskCompletionSource<int>();

            var res = Observable.SelectMany(Observable.Range(0, 3), x => tcss[x].Task);

            var lst = new List<int>();

            var err = default(Exception);
            var done = new ManualResetEvent(false);
            res.Subscribe(lst.Add, ex_ => { err = ex_; done.Set(); }, () => done.Set());

            var ex = new Exception();
            tcss[1].SetException(ex);

            done.WaitOne();

            lst.AssertEqual(new int[0]);
            Assert.Same(ex, err);
        }

        [Fact]
        public void SelectMany_TaskWithCompletionSource_Simple_Faulted_Sync()
        {
            var tcss = new TaskCompletionSource<int>[3];
            tcss[0] = new TaskCompletionSource<int>();
            tcss[1] = new TaskCompletionSource<int>();
            tcss[2] = new TaskCompletionSource<int>();

            var ex = new Exception();
            tcss[1].SetException(ex);

            var res = Observable.SelectMany(Observable.Range(0, 3), x => tcss[x].Task);

            var lst = new List<int>();

            var err = default(Exception);
            var done = new ManualResetEvent(false);
            res.Subscribe(lst.Add, ex_ => { err = ex_; done.Set(); }, () => done.Set());

            done.WaitOne();

            lst.AssertEqual(new int[0]);
            Assert.Same(ex, err);
        }

        [Fact]
        public void SelectMany_TaskWithCompletionSource_Simple_Canceled_Async()
        {
            var tcss = new TaskCompletionSource<int>[3];
            tcss[0] = new TaskCompletionSource<int>();
            tcss[1] = new TaskCompletionSource<int>();
            tcss[2] = new TaskCompletionSource<int>();

            var res = Observable.SelectMany(Observable.Range(0, 3), x => tcss[x].Task);

            var lst = new List<int>();

            var err = default(Exception);
            var done = new ManualResetEvent(false);
            res.Subscribe(lst.Add, ex_ => { err = ex_; done.Set(); }, () => done.Set());

            tcss[1].SetCanceled();

            done.WaitOne();

            lst.AssertEqual(new int[0]);
            Assert.True(err is TaskCanceledException && ((TaskCanceledException)err).Task == tcss[1].Task);
        }

        [Fact]
        public void SelectMany_TaskWithCompletionSource_Simple_Canceled_Sync()
        {
            var tcss = new TaskCompletionSource<int>[3];
            tcss[0] = new TaskCompletionSource<int>();
            tcss[1] = new TaskCompletionSource<int>();
            tcss[2] = new TaskCompletionSource<int>();

            tcss[1].SetCanceled();

            var res = Observable.SelectMany(Observable.Range(0, 3), x => tcss[x].Task);

            var lst = new List<int>();

            var err = default(Exception);
            var done = new ManualResetEvent(false);
            res.Subscribe(lst.Add, ex_ => { err = ex_; done.Set(); }, () => done.Set());

            done.WaitOne();

            lst.AssertEqual(new int[0]);
            Assert.True(err is TaskCanceledException && ((TaskCanceledException)err).Task == tcss[1].Task);
        }

        [Fact]
        public void SelectMany_TaskWithCompletionSource_Simple_InnerCompleteBeforeOuter()
        {
            var xs = new Subject<int>();

            var tcss = new TaskCompletionSource<int>[3];
            tcss[0] = new TaskCompletionSource<int>();
            tcss[1] = new TaskCompletionSource<int>();
            tcss[2] = new TaskCompletionSource<int>();

            var res = Observable.SelectMany(xs, x => tcss[x].Task);

            var lst = new List<int>();

            var done = new ManualResetEvent(false);
            res.Subscribe(lst.Add, () => done.Set());

            tcss[1].SetResult(42);

            xs.OnNext(0);
            xs.OnNext(1);
            xs.OnNext(2);

            tcss[0].SetResult(43);
            tcss[2].SetResult(44);

            xs.OnCompleted();

            done.WaitOne();

            lst.OrderBy(x => x).AssertEqual(new[] { 42, 43, 44 });
        }

        [Fact]
        public void SelectMany_TaskWithCompletionSource_Simple_OuterCompleteBeforeInner()
        {
            var xs = new Subject<int>();

            var tcss = new TaskCompletionSource<int>[3];
            tcss[0] = new TaskCompletionSource<int>();
            tcss[1] = new TaskCompletionSource<int>();
            tcss[2] = new TaskCompletionSource<int>();

            var res = Observable.SelectMany(xs, x => tcss[x].Task);

            var lst = new List<int>();

            var done = new ManualResetEvent(false);
            res.Subscribe(lst.Add, () => done.Set());

            tcss[1].SetResult(42);

            xs.OnNext(0);
            xs.OnNext(1);
            xs.OnNext(2);
            xs.OnCompleted();

            tcss[0].SetResult(43);
            tcss[2].SetResult(44);

            done.WaitOne();

            lst.OrderBy(x => x).AssertEqual(new[] { 42, 43, 44 });
        }

        [Fact]
        public void SelectMany_TaskWithCompletionSource_Simple_Cancellation_NeverInvoked()
        {
            var xs = new Subject<int>();

            var tcss = new TaskCompletionSource<int>[3];
            tcss[0] = new TaskCompletionSource<int>();
            tcss[1] = new TaskCompletionSource<int>();
            tcss[2] = new TaskCompletionSource<int>();

            var res = Observable.SelectMany(xs, (x, token) =>
            {
                var tcs = tcss[x];

                token.Register(() => tcs.SetCanceled());

                return tcs.Task;
            });

            var lst = new List<int>();

            var done = new ManualResetEvent(false);
            var d = res.Subscribe(lst.Add, () => done.Set());

            tcss[1].SetResult(42);

            xs.OnNext(0);
            xs.OnNext(1);
            xs.OnNext(2);
            xs.OnCompleted();

            tcss[0].SetResult(43);
            tcss[2].SetResult(44);

            done.WaitOne();

            lst.OrderBy(x => x).AssertEqual(new[] { 42, 43, 44 });
        }

        [Fact]
        public void SelectMany_TaskWithCompletionSource_Simple_Cancellation_Invoked()
        {
            var xs = new Subject<int>();

            var tcss = new TaskCompletionSource<int>[3];
            tcss[0] = new TaskCompletionSource<int>();
            tcss[1] = new TaskCompletionSource<int>();
            tcss[2] = new TaskCompletionSource<int>();

            var n = 0;
            var m = 0;

            var res = Observable.SelectMany(xs, (x, token) =>
            {
                var tcs = tcss[x];

                token.Register(() => { n++; m += tcs.TrySetCanceled() ? 1 : 0; });

                return tcs.Task;
            });

            var lst = new List<int>();

            var done = false;
            var d = res.Subscribe(lst.Add, () => done = true);

            tcss[1].SetResult(42);

            xs.OnNext(0);
            xs.OnNext(1);

            d.Dispose();

            xs.OnNext(2);
            xs.OnCompleted();

            Assert.False(tcss[0].TrySetResult(43));
            tcss[2].SetResult(44); // never observed because xs.OnNext(2) happened after dispose

            lst.AssertEqual(new[] { 42 });
            Assert.False(done);
            Assert.Equal(2, n);
            Assert.Equal(1, m); // tcss[1] was already finished
        }

        [Fact]
        public void SelectMany_TaskWithCompletionSource_Simple_Cancellation_AfterOuterError()
        {
            var xs = new Subject<int>();

            var tcss = new TaskCompletionSource<int>[3];
            tcss[0] = new TaskCompletionSource<int>();
            tcss[1] = new TaskCompletionSource<int>();
            tcss[2] = new TaskCompletionSource<int>();

            var n = 0;
            var m = 0;

            var res = Observable.SelectMany(xs, (x, token) =>
            {
                var tcs = tcss[x];

                token.Register(() => { n++; m += tcs.TrySetCanceled() ? 1 : 0; });

                return tcs.Task;
            });

            var lst = new List<int>();

            var done = false;
            var err = default(Exception);
            res.Subscribe(lst.Add, ex_ => err = ex_, () => done = true);

            tcss[1].SetResult(42);

            xs.OnNext(0);
            xs.OnNext(1);

            var ex = new Exception();
            xs.OnError(ex);

            Assert.False(tcss[0].TrySetResult(43));
            tcss[2].SetResult(44); // no-op

            lst.AssertEqual(new[] { 42 });
            Assert.Same(ex, err);
            Assert.False(done);
            Assert.Equal(2, n);
            Assert.Equal(1, m); // tcss[1] was already finished
        }

        [Fact]
        public void SelectMany_TaskWithCompletionSource_Simple_Cancellation_AfterSelectorThrows()
        {
            var xs = new Subject<int>();

            var tcss = new TaskCompletionSource<int>[4];
            tcss[0] = new TaskCompletionSource<int>();
            tcss[1] = new TaskCompletionSource<int>();
            tcss[2] = new TaskCompletionSource<int>();
            tcss[3] = new TaskCompletionSource<int>();

            var n = 0;
            var m = 0;

            var ex = new Exception();

            var res = Observable.SelectMany(xs, (x, token) =>
            {
                if (x == 2)
                {
                    throw ex;
                }

                var tcs = tcss[x];

                token.Register(() => { n++; m += tcs.TrySetCanceled() ? 1 : 0; });

                return tcs.Task;
            });

            var lst = new List<int>();

            var done = false;
            var evt = new ManualResetEvent(false);
            var err = default(Exception);
            res.Subscribe(lst.Add, ex_ => { err = ex_; evt.Set(); }, () => { done = true; evt.Set(); });

            tcss[1].SetResult(43);

            xs.OnNext(0);
            xs.OnNext(1);

            tcss[0].SetResult(42);

            xs.OnNext(2); // causes error
            xs.OnCompleted();

            evt.WaitOne();

            Assert.False(done);
            Assert.Same(ex, err);
            Assert.Equal(2, n);
            Assert.Equal(0, m);
        }

        [Fact]
        public void SelectMany_TaskWithCompletionSource_WithResultSelector_RanToCompletion_Async()
        {
            var tcss = new TaskCompletionSource<int>[2];
            tcss[0] = new TaskCompletionSource<int>();
            tcss[1] = new TaskCompletionSource<int>();

            var res = Observable.SelectMany(Observable.Range(0, 2), x => tcss[x].Task, (x, y) => x + y);

            var lst = new List<int>();

            var done = new ManualResetEvent(false);
            res.Subscribe(lst.Add, () => done.Set());

            tcss[0].SetResult(42);
            tcss[1].SetResult(43);

            done.WaitOne();

            lst.OrderBy(x => x).AssertEqual(new[] { 42 + 0, 43 + 1 });
        }

        [Fact]
        public void SelectMany_TaskWithCompletionSource_WithResultSelector_RanToCompletion_Sync()
        {
            var tcss = new TaskCompletionSource<int>[2];
            tcss[0] = new TaskCompletionSource<int>();
            tcss[1] = new TaskCompletionSource<int>();

            tcss[0].SetResult(42);
            tcss[1].SetResult(43);

            var res = Observable.SelectMany(Observable.Range(0, 2), x => tcss[x].Task, (x, y) => x + y);

            var lst = new List<int>();

            var done = new ManualResetEvent(false);
            res.Subscribe(lst.Add, () => done.Set());

            done.WaitOne();

            lst.OrderBy(x => x).AssertEqual(new[] { 42 + 0, 43 + 1 });
        }

        [Fact]
        public void SelectMany_TaskWithCompletionSource_WithResultSelector_Faulted_Async()
        {
            var tcss = new TaskCompletionSource<int>[3];
            tcss[0] = new TaskCompletionSource<int>();
            tcss[1] = new TaskCompletionSource<int>();
            tcss[2] = new TaskCompletionSource<int>();

            var res = Observable.SelectMany(Observable.Range(0, 3), x => tcss[x].Task, (x, y) => x + y);

            var lst = new List<int>();

            var err = default(Exception);
            var done = new ManualResetEvent(false);
            res.Subscribe(lst.Add, ex_ => { err = ex_; done.Set(); }, () => done.Set());

            var ex = new Exception();
            tcss[1].SetException(ex);

            done.WaitOne();

            lst.AssertEqual(new int[0]);
            Assert.Same(ex, err);
        }

        [Fact]
        public void SelectMany_TaskWithCompletionSource_WithResultSelector_Faulted_Sync()
        {
            var tcss = new TaskCompletionSource<int>[3];
            tcss[0] = new TaskCompletionSource<int>();
            tcss[1] = new TaskCompletionSource<int>();
            tcss[2] = new TaskCompletionSource<int>();

            var ex = new Exception();
            tcss[1].SetException(ex);

            var res = Observable.SelectMany(Observable.Range(0, 3), x => tcss[x].Task, (x, y) => x + y);

            var lst = new List<int>();

            var err = default(Exception);
            var done = new ManualResetEvent(false);
            res.Subscribe(lst.Add, ex_ => { err = ex_; done.Set(); }, () => done.Set());

            done.WaitOne();

            lst.AssertEqual(new int[0]);
            Assert.Same(ex, err);
        }

        [Fact]
        public void SelectMany_TaskWithCompletionSource_WithResultSelector_Canceled_Async()
        {
            var tcss = new TaskCompletionSource<int>[3];
            tcss[0] = new TaskCompletionSource<int>();
            tcss[1] = new TaskCompletionSource<int>();
            tcss[2] = new TaskCompletionSource<int>();

            var res = Observable.SelectMany(Observable.Range(0, 3), x => tcss[x].Task, (x, y) => x + y);

            var lst = new List<int>();

            var err = default(Exception);
            var done = new ManualResetEvent(false);
            res.Subscribe(lst.Add, ex_ => { err = ex_; done.Set(); }, () => done.Set());

            tcss[1].SetCanceled();

            done.WaitOne();

            lst.AssertEqual(new int[0]);
            Assert.True(err is TaskCanceledException && ((TaskCanceledException)err).Task == tcss[1].Task);
        }

        [Fact]
        public void SelectMany_TaskWithCompletionSource_WithResultSelector_Canceled_Sync()
        {
            var tcss = new TaskCompletionSource<int>[3];
            tcss[0] = new TaskCompletionSource<int>();
            tcss[1] = new TaskCompletionSource<int>();
            tcss[2] = new TaskCompletionSource<int>();

            tcss[1].SetCanceled();

            var res = Observable.SelectMany(Observable.Range(0, 3), x => tcss[x].Task, (x, y) => x + y);

            var lst = new List<int>();

            var err = default(Exception);
            var done = new ManualResetEvent(false);
            res.Subscribe(lst.Add, ex_ => { err = ex_; done.Set(); }, () => done.Set());

            done.WaitOne();

            lst.AssertEqual(new int[0]);
            Assert.True(err is TaskCanceledException && ((TaskCanceledException)err).Task == tcss[1].Task);
        }

        [Fact]
        public void SelectMany_TaskWithCompletionSource_WithResultSelector_InnerCompleteBeforeOuter()
        {
            var xs = new Subject<int>();

            var tcss = new TaskCompletionSource<int>[3];
            tcss[0] = new TaskCompletionSource<int>();
            tcss[1] = new TaskCompletionSource<int>();
            tcss[2] = new TaskCompletionSource<int>();

            var res = Observable.SelectMany(xs, x => tcss[x].Task, (x, y) => x + y);

            var lst = new List<int>();

            var done = new ManualResetEvent(false);
            res.Subscribe(lst.Add, () => done.Set());

            tcss[1].SetResult(42);

            xs.OnNext(0);
            xs.OnNext(1);
            xs.OnNext(2);

            tcss[0].SetResult(43);
            tcss[2].SetResult(44);

            xs.OnCompleted();

            done.WaitOne();

            lst.OrderBy(x => x).AssertEqual(new[] { 42 + 1, 43 + 0, 44 + 2 });
        }

        [Fact]
        public void SelectMany_TaskWithCompletionSource_WithResultSelector_OuterCompleteBeforeInner()
        {
            var xs = new Subject<int>();

            var tcss = new TaskCompletionSource<int>[3];
            tcss[0] = new TaskCompletionSource<int>();
            tcss[1] = new TaskCompletionSource<int>();
            tcss[2] = new TaskCompletionSource<int>();

            var res = Observable.SelectMany(xs, x => tcss[x].Task, (x, y) => x + y);

            var lst = new List<int>();

            var done = new ManualResetEvent(false);
            res.Subscribe(lst.Add, () => done.Set());

            tcss[1].SetResult(42);

            xs.OnNext(0);
            xs.OnNext(1);
            xs.OnNext(2);
            xs.OnCompleted();

            tcss[0].SetResult(43);
            tcss[2].SetResult(44);

            done.WaitOne();

            lst.OrderBy(x => x).AssertEqual(new[] { 42 + 1, 43 + 0, 44 + 2 });
        }

        [Fact]
        public void SelectMany_TaskWithCompletionSource_WithResultSelector_Cancellation_NeverInvoked()
        {
            var xs = new Subject<int>();

            var tcss = new TaskCompletionSource<int>[3];
            tcss[0] = new TaskCompletionSource<int>();
            tcss[1] = new TaskCompletionSource<int>();
            tcss[2] = new TaskCompletionSource<int>();

            var res = Observable.SelectMany(xs, (x, token) =>
            {
                var tcs = tcss[x];

                token.Register(() => tcs.SetCanceled());

                return tcs.Task;
            }, (x, y) => x + y);

            var lst = new List<int>();

            var done = new ManualResetEvent(false);
            var d = res.Subscribe(lst.Add, () => done.Set());

            tcss[1].SetResult(42);

            xs.OnNext(0);
            xs.OnNext(1);
            xs.OnNext(2);
            xs.OnCompleted();

            tcss[0].SetResult(43);
            tcss[2].SetResult(44);

            done.WaitOne();

            lst.OrderBy(x => x).AssertEqual(new[] { 42 + 1, 43 + 0, 44 + 2 });
        }

        [Fact]
        public void SelectMany_TaskWithCompletionSource_WithResultSelector_Cancellation_Invoked()
        {
            var xs = new Subject<int>();

            var tcss = new TaskCompletionSource<int>[3];
            tcss[0] = new TaskCompletionSource<int>();
            tcss[1] = new TaskCompletionSource<int>();
            tcss[2] = new TaskCompletionSource<int>();

            var n = 0;
            var m = 0;

            var res = Observable.SelectMany(xs, (x, token) =>
            {
                var tcs = tcss[x];

                token.Register(() => { n++; m += tcs.TrySetCanceled() ? 1 : 0; });

                return tcs.Task;
            }, (x, y) => x + y);

            var lst = new List<int>();

            var done = false;
            var d = res.Subscribe(lst.Add, () => done = true);

            tcss[1].SetResult(42);

            xs.OnNext(0);
            xs.OnNext(1);

            d.Dispose();

            xs.OnNext(2);
            xs.OnCompleted();

            Assert.False(tcss[0].TrySetResult(43));
            tcss[2].SetResult(44); // never observed because xs.OnNext(2) happened after dispose

            lst.AssertEqual(new[] { 42 + 1 });
            Assert.False(done);
            Assert.Equal(2, n);
            Assert.Equal(1, m); // tcss[1] was already finished
        }

        [Fact]
        public void SelectMany_TaskWithCompletionSource_WithResultSelector_Cancellation_AfterOuterError()
        {
            var xs = new Subject<int>();

            var tcss = new TaskCompletionSource<int>[3];
            tcss[0] = new TaskCompletionSource<int>();
            tcss[1] = new TaskCompletionSource<int>();
            tcss[2] = new TaskCompletionSource<int>();

            var n = 0;
            var m = 0;

            var res = Observable.SelectMany(xs, (x, token) =>
            {
                var tcs = tcss[x];

                token.Register(() => { n++; m += tcs.TrySetCanceled() ? 1 : 0; });

                return tcs.Task;
            }, (x, y) => x + y);

            var lst = new List<int>();

            var done = false;
            var err = default(Exception);
            res.Subscribe(lst.Add, ex_ => err = ex_, () => done = true);

            tcss[1].SetResult(42);

            xs.OnNext(0);
            xs.OnNext(1);

            var ex = new Exception();
            xs.OnError(ex);

            Assert.False(tcss[0].TrySetResult(43));
            tcss[2].SetResult(44); // no-op

            lst.AssertEqual(new[] { 42 + 1 });
            Assert.Same(ex, err);
            Assert.False(done);
            Assert.Equal(2, n);
            Assert.Equal(1, m); // tcss[1] was already finished
        }

        [Fact]
        public void SelectMany_TaskWithCompletionSource_WithResultSelector_Cancellation_AfterSelectorThrows()
        {
            var xs = new Subject<int>();

            var tcss = new TaskCompletionSource<int>[4];
            tcss[0] = new TaskCompletionSource<int>();
            tcss[1] = new TaskCompletionSource<int>();
            tcss[2] = new TaskCompletionSource<int>();
            tcss[3] = new TaskCompletionSource<int>();

            var n = 0;
            var m = 0;

            var ex = new Exception();

            var res = Observable.SelectMany(xs, (x, token) =>
            {
                if (x == 2)
                {
                    throw ex;
                }

                var tcs = tcss[x];

                token.Register(() => { n++; m += tcs.TrySetCanceled() ? 1 : 0; });

                return tcs.Task;
            }, (x, y) => x + y);

            var lst = new List<int>();

            var done = false;
            var evt = new ManualResetEvent(false);
            var err = default(Exception);
            res.Subscribe(lst.Add, ex_ => { err = ex_; evt.Set(); }, () => { done = true; evt.Set(); });

            tcss[1].SetResult(43);

            xs.OnNext(0);
            xs.OnNext(1);

            tcss[0].SetResult(42);

            xs.OnNext(2); // causes error
            xs.OnCompleted();

            evt.WaitOne();

            Assert.False(done);
            Assert.Same(ex, err);
            Assert.Equal(2, n);
            Assert.Equal(0, m);
        }

        [Fact]
        public void SelectManyWithIndex_Task_ArgumentChecking()
        {
            ReactiveAssert.Throws<ArgumentNullException>(() => ((IObservable<int>)null).SelectMany(DummyFunc<int, int, Task<int>>.Instance));
            ReactiveAssert.Throws<ArgumentNullException>(() => DummyObservable<int>.Instance.SelectMany((Func<int, int, Task<int>>)null));

            ReactiveAssert.Throws<ArgumentNullException>(() => ((IObservable<int>)null).SelectMany(DummyFunc<int, int, CancellationToken, Task<int>>.Instance));
            ReactiveAssert.Throws<ArgumentNullException>(() => DummyObservable<int>.Instance.SelectMany((Func<int, int, CancellationToken, Task<int>>)null));

            ReactiveAssert.Throws<ArgumentNullException>(() => ((IObservable<int>)null).SelectMany(DummyFunc<int, int, Task<int>>.Instance, DummyFunc<int, int, int, int>.Instance));
            ReactiveAssert.Throws<ArgumentNullException>(() => DummyObservable<int>.Instance.SelectMany((Func<int, int, Task<int>>)null, DummyFunc<int, int, int, int>.Instance));
            ReactiveAssert.Throws<ArgumentNullException>(() => DummyObservable<int>.Instance.SelectMany(DummyFunc<int, int, Task<int>>.Instance, ((Func<int, int, int, int>)null)));

            ReactiveAssert.Throws<ArgumentNullException>(() => ((IObservable<int>)null).SelectMany(DummyFunc<int, int, CancellationToken, Task<int>>.Instance, DummyFunc<int, int, int, int>.Instance));
            ReactiveAssert.Throws<ArgumentNullException>(() => DummyObservable<int>.Instance.SelectMany((Func<int, int, CancellationToken, Task<int>>)null, DummyFunc<int, int, int, int>.Instance));
            ReactiveAssert.Throws<ArgumentNullException>(() => DummyObservable<int>.Instance.SelectMany(DummyFunc<int, int, CancellationToken, Task<int>>.Instance, ((Func<int, int, int, int>)null)));
        }

        [Fact]
        public void SelectManyWithIndex_Task_Index()
        {
            var res = Observable.Range(0, 10).SelectMany((int x, int i) => Task.Factory.StartNew(() => new { x, i })).ToEnumerable();
            Assert.True(Enumerable.Range(0, 10).SelectMany((x, i) => new[] { new { x, i } }).SequenceEqual(res.OrderBy(v => v.i)));
        }

        [Fact]
        public void SelectManyWithIndex_Task_Cancellation_Index()
        {
            var res = Observable.Range(0, 10).SelectMany((x, i, ctx) => Task.Factory.StartNew(() => new { x, i }, ctx)).ToEnumerable();
            Assert.True(Enumerable.Range(0, 10).SelectMany((x, i) => new[] { new { x, i } }).SequenceEqual(res.OrderBy(v => v.i)));
        }

        [Fact]
        public void SelectManyWithIndex_Task_ResultSelector_Index()
        {
            var res = Observable.Range(0, 10).SelectMany((int x, int i) => Task.Factory.StartNew(() => new { x, i }), (x, i, r) => r).ToEnumerable();
            Assert.True(Enumerable.Range(0, 10).SelectMany((x, i) => new[] { new { x, i } }).SequenceEqual(res.OrderBy(v => v.i)));
        }

        [Fact]
        public void SelectManyWithIndex_Task_ResultSelector_Cancellation_Index()
        {
            var res = Observable.Range(0, 10).SelectMany((x, i, ctx) => Task.Factory.StartNew(() => new { x, i }, ctx), (x, i, r) => r).ToEnumerable();
            Assert.True(Enumerable.Range(0, 10).SelectMany((x, i) => new[] { new { x, i } }).SequenceEqual(res.OrderBy(v => v.i)));
        }

        [Fact]
        public void SelectManyWithIndex_Task1()
        {
            var res = Observable.Range(0, 10).SelectMany((int x, int _) => Task.Factory.StartNew(() => x + 1)).ToEnumerable();
            Assert.True(Enumerable.Range(0, 10).SelectMany(x => new[] { x + 1 }).SequenceEqual(res.OrderBy(x => x)));
        }

        [Fact]
        public void SelectManyWithIndex_Task2()
        {
            var res = Observable.Range(0, 10).SelectMany((x, _, ct) => Task.Factory.StartNew(() => x + 1, ct)).ToEnumerable();
            Assert.True(Enumerable.Range(0, 10).SelectMany(x => new[] { x + 1 }).SequenceEqual(res.OrderBy(x => x)));
        }

        [Fact]
        public void SelectManyWithIndex_Task_TaskThrows()
        {
            var ex = new Exception();

            var res = Observable.Range(0, 10).SelectMany((int x, int _) => Task.Factory.StartNew(() =>
            {
                if (x > 5)
                {
                    throw ex;
                }

                return x + 1;
            })).ToEnumerable();

            ReactiveAssert.Throws(ex, () =>
            {
                foreach (var x in res)
                {
                    ;
                }
            });
        }

        [Fact]
        public void SelectManyWithIndex_Task_SelectorThrows()
        {
            var ex = new Exception();

            var res = Observable.Range(0, 10).SelectMany((int x, int _) =>
            {
                if (x > 5)
                {
                    throw ex;
                }

                return Task.Factory.StartNew(() => x + 1);
            }).ToEnumerable();

            ReactiveAssert.Throws(ex, () =>
            {
                foreach (var x in res)
                {
                    ;
                }
            });
        }

        [Fact]
        public void SelectManyWithIndex_Task_ResultSelector1()
        {
            var res = Observable.Range(0, 10).SelectMany((x, _) => Task.Factory.StartNew(() => x + 1), (x, _, y) => x + y).ToEnumerable();
            Assert.True(Enumerable.Range(0, 10).SelectMany(x => new[] { 2 * x + 1 }).SequenceEqual(res.OrderBy(x => x)));
        }

        [Fact]
        public void SelectManyWithIndex_Task_ResultSelector2()
        {
            var res = Observable.Range(0, 10).SelectMany((x, _, ct) => Task.Factory.StartNew(() => x + 1, ct), (x, _, y) => x + y).ToEnumerable();
            Assert.True(Enumerable.Range(0, 10).SelectMany(x => new[] { 2 * x + 1 }).SequenceEqual(res.OrderBy(x => x)));
        }

        [Fact]
        public void SelectManyWithIndex_Task_ResultSelectorThrows()
        {
            var ex = new Exception();

            var res = Observable.Range(0, 10).SelectMany((x, _) => Task.Factory.StartNew(() => x + 1), (x, _, y) =>
            {
                if (x > 5)
                {
                    throw ex;
                }

                return x + y;
            }).ToEnumerable();

            ReactiveAssert.Throws(ex, () =>
            {
                foreach (var x in res)
                {
                    ;
                }
            });
        }

        [Fact]
        public void SelectManyWithIndex_TaskWithCompletionSource_Simple_RanToCompletion_Async()
        {
            var tcss = new TaskCompletionSource<int>[2];
            tcss[0] = new TaskCompletionSource<int>();
            tcss[1] = new TaskCompletionSource<int>();

            var res = Observable.SelectMany(Observable.Range(0, 2), (int x, int _) => tcss[x].Task);

            var lst = new List<int>();

            var done = new ManualResetEvent(false);
            res.Subscribe(lst.Add, () => done.Set());

            tcss[0].SetResult(42);
            tcss[1].SetResult(43);

            done.WaitOne();

            lst.OrderBy(x => x).AssertEqual(new[] { 42, 43 });
        }

        [Fact]
        public void SelectManyWithIndex_TaskWithCompletionSource_Simple_RanToCompletion_Sync()
        {
            var tcss = new TaskCompletionSource<int>[2];
            tcss[0] = new TaskCompletionSource<int>();
            tcss[1] = new TaskCompletionSource<int>();

            tcss[0].SetResult(42);
            tcss[1].SetResult(43);

            var res = Observable.SelectMany(Observable.Range(0, 2), (int x, int _) => tcss[x].Task);

            var lst = new List<int>();

            var done = new ManualResetEvent(false);
            res.Subscribe(lst.Add, () => done.Set());

            done.WaitOne();

            lst.OrderBy(x => x).AssertEqual(new[] { 42, 43 });
        }

        [Fact]
        public void SelectManyWithIndex_TaskWithCompletionSource_Simple_Faulted_Async()
        {
            var tcss = new TaskCompletionSource<int>[3];
            tcss[0] = new TaskCompletionSource<int>();
            tcss[1] = new TaskCompletionSource<int>();
            tcss[2] = new TaskCompletionSource<int>();

            var res = Observable.SelectMany(Observable.Range(0, 3), (int x, int _) => tcss[x].Task);

            var lst = new List<int>();

            var err = default(Exception);
            var done = new ManualResetEvent(false);
            res.Subscribe(lst.Add, ex_ => { err = ex_; done.Set(); }, () => done.Set());

            var ex = new Exception();
            tcss[1].SetException(ex);

            done.WaitOne();

            lst.AssertEqual(new int[0]);
            Assert.Same(ex, err);
        }

        [Fact]
        public void SelectManyWithIndex_TaskWithCompletionSource_Simple_Faulted_Sync()
        {
            var tcss = new TaskCompletionSource<int>[3];
            tcss[0] = new TaskCompletionSource<int>();
            tcss[1] = new TaskCompletionSource<int>();
            tcss[2] = new TaskCompletionSource<int>();

            var ex = new Exception();
            tcss[1].SetException(ex);

            var res = Observable.SelectMany(Observable.Range(0, 3), (int x, int _) => tcss[x].Task);

            var lst = new List<int>();

            var err = default(Exception);
            var done = new ManualResetEvent(false);
            res.Subscribe(lst.Add, ex_ => { err = ex_; done.Set(); }, () => done.Set());

            done.WaitOne();

            lst.AssertEqual(new int[0]);
            Assert.Same(ex, err);
        }

        [Fact]
        public void SelectManyWithIndex_TaskWithCompletionSource_Simple_Canceled_Async()
        {
            var tcss = new TaskCompletionSource<int>[3];
            tcss[0] = new TaskCompletionSource<int>();
            tcss[1] = new TaskCompletionSource<int>();
            tcss[2] = new TaskCompletionSource<int>();

            var res = Observable.SelectMany(Observable.Range(0, 3), (int x, int _) => tcss[x].Task);

            var lst = new List<int>();

            var err = default(Exception);
            var done = new ManualResetEvent(false);
            res.Subscribe(lst.Add, ex_ => { err = ex_; done.Set(); }, () => done.Set());

            tcss[1].SetCanceled();

            done.WaitOne();

            lst.AssertEqual(new int[0]);
            Assert.True(err is TaskCanceledException && ((TaskCanceledException)err).Task == tcss[1].Task);
        }

        [Fact]
        public void SelectManyWithIndex_TaskWithCompletionSource_Simple_Canceled_Sync()
        {
            var tcss = new TaskCompletionSource<int>[3];
            tcss[0] = new TaskCompletionSource<int>();
            tcss[1] = new TaskCompletionSource<int>();
            tcss[2] = new TaskCompletionSource<int>();

            tcss[1].SetCanceled();

            var res = Observable.SelectMany(Observable.Range(0, 3), (int x, int _) => tcss[x].Task);

            var lst = new List<int>();

            var err = default(Exception);
            var done = new ManualResetEvent(false);
            res.Subscribe(lst.Add, ex_ => { err = ex_; done.Set(); }, () => done.Set());

            done.WaitOne();

            lst.AssertEqual(new int[0]);
            Assert.True(err is TaskCanceledException && ((TaskCanceledException)err).Task == tcss[1].Task);
        }

        [Fact]
        public void SelectManyWithIndex_TaskWithCompletionSource_Simple_InnerCompleteBeforeOuter()
        {
            var xs = new Subject<int>();

            var tcss = new TaskCompletionSource<int>[3];
            tcss[0] = new TaskCompletionSource<int>();
            tcss[1] = new TaskCompletionSource<int>();
            tcss[2] = new TaskCompletionSource<int>();

            var res = Observable.SelectMany(xs, (int x, int _) => tcss[x].Task);

            var lst = new List<int>();

            var done = new ManualResetEvent(false);
            res.Subscribe(lst.Add, () => done.Set());

            tcss[1].SetResult(42);

            xs.OnNext(0);
            xs.OnNext(1);
            xs.OnNext(2);

            tcss[0].SetResult(43);
            tcss[2].SetResult(44);

            xs.OnCompleted();

            done.WaitOne();

            lst.OrderBy(x => x).AssertEqual(new[] { 42, 43, 44 });
        }

        [Fact]
        public void SelectManyWithIndex_TaskWithCompletionSource_Simple_OuterCompleteBeforeInner()
        {
            var xs = new Subject<int>();

            var tcss = new TaskCompletionSource<int>[3];
            tcss[0] = new TaskCompletionSource<int>();
            tcss[1] = new TaskCompletionSource<int>();
            tcss[2] = new TaskCompletionSource<int>();

            var res = Observable.SelectMany(xs, (int x, int _) => tcss[x].Task);

            var lst = new List<int>();

            var done = new ManualResetEvent(false);
            res.Subscribe(lst.Add, () => done.Set());

            tcss[1].SetResult(42);

            xs.OnNext(0);
            xs.OnNext(1);
            xs.OnNext(2);
            xs.OnCompleted();

            tcss[0].SetResult(43);
            tcss[2].SetResult(44);

            done.WaitOne();

            lst.OrderBy(x => x).AssertEqual(new[] { 42, 43, 44 });
        }

        [Fact]
        public void SelectManyWithIndex_TaskWithCompletionSource_Simple_Cancellation_NeverInvoked()
        {
            var xs = new Subject<int>();

            var tcss = new TaskCompletionSource<int>[3];
            tcss[0] = new TaskCompletionSource<int>();
            tcss[1] = new TaskCompletionSource<int>();
            tcss[2] = new TaskCompletionSource<int>();

            var res = Observable.SelectMany(xs, (x, _, token) =>
            {
                var tcs = tcss[x];

                token.Register(() => tcs.SetCanceled());

                return tcs.Task;
            });

            var lst = new List<int>();

            var done = new ManualResetEvent(false);
            var d = res.Subscribe(lst.Add, () => done.Set());

            tcss[1].SetResult(42);

            xs.OnNext(0);
            xs.OnNext(1);
            xs.OnNext(2);
            xs.OnCompleted();

            tcss[0].SetResult(43);
            tcss[2].SetResult(44);

            done.WaitOne();

            lst.OrderBy(x => x).AssertEqual(new[] { 42, 43, 44 });
        }

        [Fact]
        public void SelectManyWithIndex_TaskWithCompletionSource_Simple_Cancellation_Invoked()
        {
            var xs = new Subject<int>();

            var tcss = new TaskCompletionSource<int>[3];
            tcss[0] = new TaskCompletionSource<int>();
            tcss[1] = new TaskCompletionSource<int>();
            tcss[2] = new TaskCompletionSource<int>();

            var n = 0;
            var m = 0;

            var res = Observable.SelectMany(xs, (x, _, token) =>
            {
                var tcs = tcss[x];

                token.Register(() => { n++; m += tcs.TrySetCanceled() ? 1 : 0; });

                return tcs.Task;
            });

            var lst = new List<int>();

            var done = false;
            var d = res.Subscribe(lst.Add, () => done = true);

            tcss[1].SetResult(42);

            xs.OnNext(0);
            xs.OnNext(1);

            d.Dispose();

            xs.OnNext(2);
            xs.OnCompleted();

            Assert.False(tcss[0].TrySetResult(43));
            tcss[2].SetResult(44); // never observed because xs.OnNext(2) happened after dispose

            lst.AssertEqual(new[] { 42 });
            Assert.False(done);
            Assert.Equal(2, n);
            Assert.Equal(1, m); // tcss[1] was already finished
        }

        [Fact]
        public void SelectManyWithIndex_TaskWithCompletionSource_Simple_Cancellation_AfterOuterError()
        {
            var xs = new Subject<int>();

            var tcss = new TaskCompletionSource<int>[3];
            tcss[0] = new TaskCompletionSource<int>();
            tcss[1] = new TaskCompletionSource<int>();
            tcss[2] = new TaskCompletionSource<int>();

            var n = 0;
            var m = 0;

            var res = Observable.SelectMany(xs, (x, _, token) =>
            {
                var tcs = tcss[x];

                token.Register(() => { n++; m += tcs.TrySetCanceled() ? 1 : 0; });

                return tcs.Task;
            });

            var lst = new List<int>();

            var done = false;
            var err = default(Exception);
            res.Subscribe(lst.Add, ex_ => err = ex_, () => done = true);

            tcss[1].SetResult(42);

            xs.OnNext(0);
            xs.OnNext(1);

            var ex = new Exception();
            xs.OnError(ex);

            Assert.False(tcss[0].TrySetResult(43));
            tcss[2].SetResult(44); // no-op

            lst.AssertEqual(new[] { 42 });
            Assert.Same(ex, err);
            Assert.False(done);
            Assert.Equal(2, n);
            Assert.Equal(1, m); // tcss[1] was already finished
        }

        [Fact]
        public void SelectManyWithIndex_TaskWithCompletionSource_Simple_Cancellation_AfterSelectorThrows()
        {
            var xs = new Subject<int>();

            var tcss = new TaskCompletionSource<int>[4];
            tcss[0] = new TaskCompletionSource<int>();
            tcss[1] = new TaskCompletionSource<int>();
            tcss[2] = new TaskCompletionSource<int>();
            tcss[3] = new TaskCompletionSource<int>();

            var n = 0;
            var m = 0;

            var ex = new Exception();

            var res = Observable.SelectMany(xs, (x, _, token) =>
            {
                if (x == 2)
                {
                    throw ex;
                }

                var tcs = tcss[x];

                token.Register(() => { n++; m += tcs.TrySetCanceled() ? 1 : 0; });

                return tcs.Task;
            });

            var lst = new List<int>();

            var done = false;
            var evt = new ManualResetEvent(false);
            var err = default(Exception);
            res.Subscribe(lst.Add, ex_ => { err = ex_; evt.Set(); }, () => { done = true; evt.Set(); });

            tcss[1].SetResult(43);

            xs.OnNext(0);
            xs.OnNext(1);

            tcss[0].SetResult(42);

            xs.OnNext(2); // causes error
            xs.OnCompleted();

            evt.WaitOne();

            Assert.False(done);
            Assert.Same(ex, err);
            Assert.Equal(2, n);
            Assert.Equal(0, m);
        }

        [Fact]
        public void SelectManyWithIndex_TaskWithCompletionSource_WithResultSelector_RanToCompletion_Async()
        {
            var tcss = new TaskCompletionSource<int>[2];
            tcss[0] = new TaskCompletionSource<int>();
            tcss[1] = new TaskCompletionSource<int>();

            var res = Observable.SelectMany(Observable.Range(0, 2), (x, _) => tcss[x].Task, (x, _, y) => x + y);

            var lst = new List<int>();

            var done = new ManualResetEvent(false);
            res.Subscribe(lst.Add, () => done.Set());

            tcss[0].SetResult(42);
            tcss[1].SetResult(43);

            done.WaitOne();

            lst.OrderBy(x => x).AssertEqual(new[] { 42 + 0, 43 + 1 });
        }

        [Fact]
        public void SelectManyWithIndex_TaskWithCompletionSource_WithResultSelector_RanToCompletion_Sync()
        {
            var tcss = new TaskCompletionSource<int>[2];
            tcss[0] = new TaskCompletionSource<int>();
            tcss[1] = new TaskCompletionSource<int>();

            tcss[0].SetResult(42);
            tcss[1].SetResult(43);

            var res = Observable.SelectMany(Observable.Range(0, 2), (x, _) => tcss[x].Task, (x, _, y) => x + y);

            var lst = new List<int>();

            var done = new ManualResetEvent(false);
            res.Subscribe(lst.Add, () => done.Set());

            done.WaitOne();

            lst.OrderBy(x => x).AssertEqual(new[] { 42 + 0, 43 + 1 });
        }

        [Fact]
        public void SelectManyWithIndex_TaskWithCompletionSource_WithResultSelector_Faulted_Async()
        {
            var tcss = new TaskCompletionSource<int>[3];
            tcss[0] = new TaskCompletionSource<int>();
            tcss[1] = new TaskCompletionSource<int>();
            tcss[2] = new TaskCompletionSource<int>();

            var res = Observable.SelectMany(Observable.Range(0, 3), (x, _) => tcss[x].Task, (x, _, y) => x + y);

            var lst = new List<int>();

            var err = default(Exception);
            var done = new ManualResetEvent(false);
            res.Subscribe(lst.Add, ex_ => { err = ex_; done.Set(); }, () => done.Set());

            var ex = new Exception();
            tcss[1].SetException(ex);

            done.WaitOne();

            lst.AssertEqual(new int[0]);
            Assert.Same(ex, err);
        }

        [Fact]
        public void SelectManyWithIndex_TaskWithCompletionSource_WithResultSelector_Faulted_Sync()
        {
            var tcss = new TaskCompletionSource<int>[3];
            tcss[0] = new TaskCompletionSource<int>();
            tcss[1] = new TaskCompletionSource<int>();
            tcss[2] = new TaskCompletionSource<int>();

            var ex = new Exception();
            tcss[1].SetException(ex);

            var res = Observable.SelectMany(Observable.Range(0, 3), (x, _) => tcss[x].Task, (x, _, y) => x + y);

            var lst = new List<int>();

            var err = default(Exception);
            var done = new ManualResetEvent(false);
            res.Subscribe(lst.Add, ex_ => { err = ex_; done.Set(); }, () => done.Set());

            done.WaitOne();

            lst.AssertEqual(new int[0]);
            Assert.Same(ex, err);
        }

        [Fact]
        public void SelectManyWithIndex_TaskWithCompletionSource_WithResultSelector_Canceled_Async()
        {
            var tcss = new TaskCompletionSource<int>[3];
            tcss[0] = new TaskCompletionSource<int>();
            tcss[1] = new TaskCompletionSource<int>();
            tcss[2] = new TaskCompletionSource<int>();

            var res = Observable.SelectMany(Observable.Range(0, 3), (x, _) => tcss[x].Task, (x, _, y) => x + y);

            var lst = new List<int>();

            var err = default(Exception);
            var done = new ManualResetEvent(false);
            res.Subscribe(lst.Add, ex_ => { err = ex_; done.Set(); }, () => done.Set());

            tcss[1].SetCanceled();

            done.WaitOne();

            lst.AssertEqual(new int[0]);
            Assert.True(err is TaskCanceledException && ((TaskCanceledException)err).Task == tcss[1].Task);
        }

        [Fact]
        public void SelectManyWithIndex_TaskWithCompletionSource_WithResultSelector_Canceled_Sync()
        {
            var tcss = new TaskCompletionSource<int>[3];
            tcss[0] = new TaskCompletionSource<int>();
            tcss[1] = new TaskCompletionSource<int>();
            tcss[2] = new TaskCompletionSource<int>();

            tcss[1].SetCanceled();

            var res = Observable.SelectMany(Observable.Range(0, 3), (x, _) => tcss[x].Task, (x, _, y) => x + y);

            var lst = new List<int>();

            var err = default(Exception);
            var done = new ManualResetEvent(false);
            res.Subscribe(lst.Add, ex_ => { err = ex_; done.Set(); }, () => done.Set());

            done.WaitOne();

            lst.AssertEqual(new int[0]);
            Assert.True(err is TaskCanceledException && ((TaskCanceledException)err).Task == tcss[1].Task);
        }

        [Fact]
        public void SelectManyWithIndex_TaskWithCompletionSource_WithResultSelector_InnerCompleteBeforeOuter()
        {
            var xs = new Subject<int>();

            var tcss = new TaskCompletionSource<int>[3];
            tcss[0] = new TaskCompletionSource<int>();
            tcss[1] = new TaskCompletionSource<int>();
            tcss[2] = new TaskCompletionSource<int>();

            var res = Observable.SelectMany(xs, (x, _) => tcss[x].Task, (x, _, y) => x + y);

            var lst = new List<int>();

            var done = new ManualResetEvent(false);
            res.Subscribe(lst.Add, () => done.Set());

            tcss[1].SetResult(42);

            xs.OnNext(0);
            xs.OnNext(1);
            xs.OnNext(2);

            tcss[0].SetResult(43);
            tcss[2].SetResult(44);

            xs.OnCompleted();

            done.WaitOne();

            lst.OrderBy(x => x).AssertEqual(new[] { 42 + 1, 43 + 0, 44 + 2 });
        }

        [Fact]
        public void SelectManyWithIndex_TaskWithCompletionSource_WithResultSelector_OuterCompleteBeforeInner()
        {
            var xs = new Subject<int>();

            var tcss = new TaskCompletionSource<int>[3];
            tcss[0] = new TaskCompletionSource<int>();
            tcss[1] = new TaskCompletionSource<int>();
            tcss[2] = new TaskCompletionSource<int>();

            var res = Observable.SelectMany(xs, (x, _) => tcss[x].Task, (x, _, y) => x + y);

            var lst = new List<int>();

            var done = new ManualResetEvent(false);
            res.Subscribe(lst.Add, () => done.Set());

            tcss[1].SetResult(42);

            xs.OnNext(0);
            xs.OnNext(1);
            xs.OnNext(2);
            xs.OnCompleted();

            tcss[0].SetResult(43);
            tcss[2].SetResult(44);

            done.WaitOne();

            lst.OrderBy(x => x).AssertEqual(new[] { 42 + 1, 43 + 0, 44 + 2 });
        }

        [Fact]
        public void SelectManyWithIndex_TaskWithCompletionSource_WithResultSelector_Cancellation_NeverInvoked()
        {
            var xs = new Subject<int>();

            var tcss = new TaskCompletionSource<int>[3];
            tcss[0] = new TaskCompletionSource<int>();
            tcss[1] = new TaskCompletionSource<int>();
            tcss[2] = new TaskCompletionSource<int>();

            var res = Observable.SelectMany(xs, (x, _, token) =>
            {
                var tcs = tcss[x];

                token.Register(() => tcs.SetCanceled());

                return tcs.Task;
            }, (x, _, y) => x + y);

            var lst = new List<int>();

            var done = new ManualResetEvent(false);
            var d = res.Subscribe(lst.Add, () => done.Set());

            tcss[1].SetResult(42);

            xs.OnNext(0);
            xs.OnNext(1);
            xs.OnNext(2);
            xs.OnCompleted();

            tcss[0].SetResult(43);
            tcss[2].SetResult(44);

            done.WaitOne();

            lst.OrderBy(x => x).AssertEqual(new[] { 42 + 1, 43 + 0, 44 + 2 });
        }

        [Fact]
        public void SelectManyWithIndex_TaskWithCompletionSource_WithResultSelector_Cancellation_Invoked()
        {
            var xs = new Subject<int>();

            var tcss = new TaskCompletionSource<int>[3];
            tcss[0] = new TaskCompletionSource<int>();
            tcss[1] = new TaskCompletionSource<int>();
            tcss[2] = new TaskCompletionSource<int>();

            var n = 0;
            var m = 0;

            var res = Observable.SelectMany(xs, (x, _, token) =>
            {
                var tcs = tcss[x];

                token.Register(() => { n++; m += tcs.TrySetCanceled() ? 1 : 0; });

                return tcs.Task;
            }, (x, _, y) => x + y);

            var lst = new List<int>();

            var done = false;
            var d = res.Subscribe(lst.Add, () => done = true);

            tcss[1].SetResult(42);

            xs.OnNext(0);
            xs.OnNext(1);

            d.Dispose();

            xs.OnNext(2);
            xs.OnCompleted();

            Assert.False(tcss[0].TrySetResult(43));
            tcss[2].SetResult(44); // never observed because xs.OnNext(2) happened after dispose

            lst.AssertEqual(new[] { 42 + 1 });
            Assert.False(done);
            Assert.Equal(2, n);
            Assert.Equal(1, m); // tcss[1] was already finished
        }

        [Fact]
        public void SelectManyWithIndex_TaskWithCompletionSource_WithResultSelector_Cancellation_AfterOuterError()
        {
            var xs = new Subject<int>();

            var tcss = new TaskCompletionSource<int>[3];
            tcss[0] = new TaskCompletionSource<int>();
            tcss[1] = new TaskCompletionSource<int>();
            tcss[2] = new TaskCompletionSource<int>();

            var n = 0;
            var m = 0;

            var res = Observable.SelectMany(xs, (x, _, token) =>
            {
                var tcs = tcss[x];

                token.Register(() => { n++; m += tcs.TrySetCanceled() ? 1 : 0; });

                return tcs.Task;
            }, (x, _, y) => x + y);

            var lst = new List<int>();

            var done = false;
            var err = default(Exception);
            res.Subscribe(lst.Add, ex_ => err = ex_, () => done = true);

            tcss[1].SetResult(42);

            xs.OnNext(0);
            xs.OnNext(1);

            var ex = new Exception();
            xs.OnError(ex);

            Assert.False(tcss[0].TrySetResult(43));
            tcss[2].SetResult(44); // no-op

            lst.AssertEqual(new[] { 42 + 1 });
            Assert.Same(ex, err);
            Assert.False(done);
            Assert.Equal(2, n);
            Assert.Equal(1, m); // tcss[1] was already finished
        }

        [Fact]
        public void SelectManyWithIndex_TaskWithCompletionSource_WithResultSelector_Cancellation_AfterSelectorThrows()
        {
            var xs = new Subject<int>();

            var tcss = new TaskCompletionSource<int>[4];
            tcss[0] = new TaskCompletionSource<int>();
            tcss[1] = new TaskCompletionSource<int>();
            tcss[2] = new TaskCompletionSource<int>();
            tcss[3] = new TaskCompletionSource<int>();

            var n = 0;
            var m = 0;

            var ex = new Exception();

            var res = Observable.SelectMany(xs, (x, _, token) =>
            {
                if (x == 2)
                {
                    throw ex;
                }

                var tcs = tcss[x];

                token.Register(() => { n++; m += tcs.TrySetCanceled() ? 1 : 0; });

                return tcs.Task;
            }, (x, _, y) => x + y);

            var lst = new List<int>();

            var done = false;
            var evt = new ManualResetEvent(false);
            var err = default(Exception);
            res.Subscribe(lst.Add, ex_ => { err = ex_; evt.Set(); }, () => { done = true; evt.Set(); });

            tcss[1].SetResult(43);

            xs.OnNext(0);
            xs.OnNext(1);

            tcss[0].SetResult(42);

            xs.OnNext(2); // causes error
            xs.OnCompleted();

            evt.WaitOne();

            Assert.False(done);
            Assert.Same(ex, err);
            Assert.Equal(2, n);
            Assert.Equal(0, m);
        }

    }
}

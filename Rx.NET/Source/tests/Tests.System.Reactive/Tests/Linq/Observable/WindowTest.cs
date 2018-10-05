// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the Apache 2.0 License.
// See the LICENSE file in the project root for more information. 

using System;
using System.Collections.Generic;
using System.Linq;
using System.Reactive.Concurrency;
using System.Reactive.Linq;
using Microsoft.Reactive.Testing;
using ReactiveTests.Dummies;
using Xunit;

namespace ReactiveTests.Tests
{
    public class WindowTest : ReactiveTest
    {
        #region + Observable +

        [Fact]
        public void Window_ArgumentChecking()
        {
            ReactiveAssert.Throws<ArgumentNullException>(() => Observable.Window(default(IObservable<int>), DummyFunc<IObservable<int>>.Instance));
            ReactiveAssert.Throws<ArgumentNullException>(() => Observable.Window(DummyObservable<int>.Instance, default(Func<IObservable<int>>)));
            ReactiveAssert.Throws<ArgumentNullException>(() => Observable.Window(default(IObservable<int>), DummyObservable<int>.Instance, DummyFunc<int, IObservable<int>>.Instance));
            ReactiveAssert.Throws<ArgumentNullException>(() => Observable.Window(DummyObservable<int>.Instance, default, DummyFunc<int, IObservable<int>>.Instance));
            ReactiveAssert.Throws<ArgumentNullException>(() => Observable.Window(DummyObservable<int>.Instance, DummyObservable<int>.Instance, default(Func<int, IObservable<int>>)));
            ReactiveAssert.Throws<ArgumentNullException>(() => Observable.Window(default(IObservable<int>), DummyObservable<int>.Instance));
            ReactiveAssert.Throws<ArgumentNullException>(() => Observable.Window(DummyObservable<int>.Instance, default(IObservable<int>)));
        }

        [Fact]
        public void Window_Closings_Basic()
        {
            var scheduler = new TestScheduler();

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
                xs.Window(() => Observable.Timer(TimeSpan.FromTicks((window++) * 100), scheduler)).Select((w, i) => w.Select(x => i.ToString() + " " + x.ToString())).Merge()
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

        [Fact]
        public void Window_Closings_InnerSubscriptions()
        {
            var scheduler = new TestScheduler();

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

            var closings = new ITestableObservable<bool>[] {
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

        [Fact]
        public void Window_Closings_Empty()
        {
            var scheduler = new TestScheduler();

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
                xs.Window(() => Observable.Empty<int>().Delay(TimeSpan.FromTicks((window++) * 100), scheduler)).Select((w, i) => w.Select(x => i.ToString() + " " + x.ToString())).Merge()
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

        [Fact]
        public void Window_Closings_Dispose()
        {
            var scheduler = new TestScheduler();

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
                xs.Window(() => Observable.Timer(TimeSpan.FromTicks((window++) * 100), scheduler)).Select((w, i) => w.Select(x => i.ToString() + " " + x.ToString())).Merge(),
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

        [Fact]
        public void Window_Closings_Error()
        {
            var scheduler = new TestScheduler();

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
                xs.Window(() => Observable.Timer(TimeSpan.FromTicks((window++) * 100), scheduler)).Select((w, i) => w.Select(x => i.ToString() + " " + x.ToString())).Merge()
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

        [Fact]
        public void Window_Closings_Throw()
        {
            var scheduler = new TestScheduler();

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
                xs.Window<int, int>(() => { throw ex; }).Select((w, i) => w.Select(x => i.ToString() + " " + x.ToString())).Merge()
            );

            res.Messages.AssertEqual(
                OnError<string>(200, ex)
            );

            xs.Subscriptions.AssertEqual(
                Subscribe(200, 200)
            );
        }

        [Fact]
        public void Window_Closings_WindowClose_Error()
        {
            var scheduler = new TestScheduler();

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
                xs.Window(() => Observable.Throw<int>(ex, scheduler)).Select((w, i) => w.Select(x => i.ToString() + " " + x.ToString())).Merge()
            );

            res.Messages.AssertEqual(
                OnError<string>(201, ex)
            );

            xs.Subscriptions.AssertEqual(
                Subscribe(200, 201)
            );
        }

        [Fact]
        public void Window_Closings_Default()
        {
            var scheduler = new TestScheduler();

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
                xs.Window(() => Observable.Timer(TimeSpan.FromTicks((window++) * 100), scheduler)).Select((w, i) => w.Select(x => i.ToString() + " " + x.ToString())).Merge()
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

        [Fact]
        public void Window_OpeningClosings_Basic()
        {
            var scheduler = new TestScheduler();

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
                xs.Window(ys, x => Observable.Timer(TimeSpan.FromTicks(x), scheduler)).Select((w, i) => w.Select(x => i.ToString() + " " + x.ToString())).Merge()
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

#if !NO_PERF // BREAKING CHANGE v2 > v1.x -> More aggressive disposal behavior
            xs.Subscriptions.AssertEqual(
                Subscribe(200, 590)
            );
#else
            xs.Subscriptions.AssertEqual(
                Subscribe(200, 900)
            );
#endif

            ys.Subscriptions.AssertEqual(
                Subscribe(200, 900)
            );
        }

        [Fact]
        public void Window_OpeningClosings_Throw()
        {
            var scheduler = new TestScheduler();

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

            var ex = new Exception();

            var res = scheduler.Start(() =>
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

        [Fact]
        public void Window_OpeningClosings_Dispose()
        {
            var scheduler = new TestScheduler();

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
                xs.Window(ys, x => Observable.Timer(TimeSpan.FromTicks(x), scheduler)).Select((w, i) => w.Select(x => i.ToString() + " " + x.ToString())).Merge(),
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

        [Fact]
        public void Window_OpeningClosings_Data_Error()
        {
            var scheduler = new TestScheduler();

            var ex = new Exception();

            var xs = scheduler.CreateHotObservable(
                OnNext(90, 1),
                OnNext(180, 2),
                OnNext(250, 3),
                OnNext(260, 4),
                OnNext(310, 5),
                OnNext(340, 6),
                OnNext(410, 7),
                OnError<int>(415, ex)
            );

            var ys = scheduler.CreateHotObservable(
                OnNext(255, 50),
                OnNext(330, 100),
                OnNext(350, 50),
                OnNext(400, 90),
                OnCompleted<int>(900)
            );

            var res = scheduler.Start(() =>
                xs.Window(ys, x => Observable.Timer(TimeSpan.FromTicks(x), scheduler)).Select((w, i) => w.Select(x => i.ToString() + " " + x.ToString())).Merge()
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

        [Fact]
        public void Window_OpeningClosings_Window_Error()
        {
            var scheduler = new TestScheduler();

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
                OnCompleted<int>(590)
            );

            var ys = scheduler.CreateHotObservable(
                OnNext(255, 50),
                OnNext(330, 100),
                OnNext(350, 50),
                OnNext(400, 90),
                OnError<int>(415, ex)
            );

            var res = scheduler.Start(() =>
                xs.Window(ys, x => Observable.Timer(TimeSpan.FromTicks(x), scheduler)).Select((w, i) => w.Select(x => i.ToString() + " " + x.ToString())).Merge()
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

        [Fact]
        public void Window_Boundaries_Simple()
        {
            var scheduler = new TestScheduler();

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

        [Fact]
        public void Window_Boundaries_OnCompletedBoundaries()
        {
            var scheduler = new TestScheduler();

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

        [Fact]
        public void Window_Boundaries_OnErrorSource()
        {
            var ex = new Exception();

            var scheduler = new TestScheduler();

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

        [Fact]
        public void Window_Boundaries_OnErrorBoundaries()
        {
            var ex = new Exception();

            var scheduler = new TestScheduler();

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

        [Fact]
        public void WindowWithCount_ArgumentChecking()
        {
            ReactiveAssert.Throws<ArgumentNullException>(() => Observable.Window(default(IObservable<int>), 1, 1));
            ReactiveAssert.Throws<ArgumentOutOfRangeException>(() => Observable.Window(DummyObservable<int>.Instance, 0, 1));
            ReactiveAssert.Throws<ArgumentOutOfRangeException>(() => Observable.Window(DummyObservable<int>.Instance, 1, 0));
            ReactiveAssert.Throws<ArgumentNullException>(() => Observable.Window(default(IObservable<int>), 1));
            ReactiveAssert.Throws<ArgumentOutOfRangeException>(() => Observable.Window(DummyObservable<int>.Instance, 0));
        }

        [Fact]
        public void WindowWithCount_Basic()
        {
            var scheduler = new TestScheduler();

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

        [Fact]
        public void WindowWithCount_InnerTimings()
        {
            var scheduler = new TestScheduler();

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

            var res = default(IObservable<IObservable<int>>);
            var outerSubscription = default(IDisposable);
            var innerSubscriptions = new List<IDisposable>();
            var windows = new List<IObservable<int>>();
            var observers = new List<ITestableObserver<int>>();

            scheduler.ScheduleAbsolute(Created, () => res = xs.Window(3, 2));

            scheduler.ScheduleAbsolute(Subscribed, () =>
            {
                outerSubscription = res.Subscribe(
                    window =>
                    {
                        var result = scheduler.CreateObserver<int>();
                        windows.Add(window);
                        observers.Add(result);
                        scheduler.ScheduleAbsolute(0, () => innerSubscriptions.Add(window.Subscribe(result)));
                    }
                );
            });

            scheduler.Start();

            Assert.Equal(5, observers.Count);

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

        [Fact]
        public void WindowWithCount_InnerTimings_DisposeOuter()
        {
            var scheduler = new TestScheduler();

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

            var res = default(IObservable<IObservable<int>>);
            var outerSubscription = default(IDisposable);
            var innerSubscriptions = new List<IDisposable>();
            var windows = new List<IObservable<int>>();
            var observers = new List<ITestableObserver<int>>();
            var windowCreationTimes = new List<long>();

            scheduler.ScheduleAbsolute(Created, () => res = xs.Window(3, 2));

            scheduler.ScheduleAbsolute(Subscribed, () =>
            {
                outerSubscription = res.Subscribe(
                    window =>
                    {
                        windowCreationTimes.Add(scheduler.Clock);

                        var result = scheduler.CreateObserver<int>();
                        windows.Add(window);
                        observers.Add(result);
                        scheduler.ScheduleAbsolute(0, () => innerSubscriptions.Add(window.Subscribe(result)));
                    }
                );
            });

            scheduler.ScheduleAbsolute(400, () =>
            {
                outerSubscription.Dispose();
            });

            scheduler.Start();

            Assert.True(windowCreationTimes.Last() < 400);

            Assert.Equal(4, observers.Count);

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

        [Fact]
        public void WindowWithCount_InnerTimings_DisposeOuterAndInners()
        {
            var scheduler = new TestScheduler();

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

            var res = default(IObservable<IObservable<int>>);
            var outerSubscription = default(IDisposable);
            var innerSubscriptions = new List<IDisposable>();
            var windows = new List<IObservable<int>>();
            var observers = new List<ITestableObserver<int>>();
            var windowCreationTimes = new List<long>();

            scheduler.ScheduleAbsolute(Created, () => res = xs.Window(3, 2));

            scheduler.ScheduleAbsolute(Subscribed, () =>
            {
                outerSubscription = res.Subscribe(
                    window =>
                    {
                        windowCreationTimes.Add(scheduler.Clock);

                        var result = scheduler.CreateObserver<int>();
                        windows.Add(window);
                        observers.Add(result);
                        scheduler.ScheduleAbsolute(0, () => innerSubscriptions.Add(window.Subscribe(result)));
                    }
                );
            });

            scheduler.ScheduleAbsolute(400, () =>
            {
                outerSubscription.Dispose();
                foreach (var d in innerSubscriptions)
                {
                    d.Dispose();
                }
            });

            scheduler.Start();

            Assert.True(windowCreationTimes.Last() < 400);

            Assert.Equal(4, observers.Count);

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

        [Fact]
        public void WindowWithCount_Disposed()
        {
            var scheduler = new TestScheduler();

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

        [Fact]
        public void WindowWithCount_Error()
        {
            var scheduler = new TestScheduler();

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

        [Fact]
        public void WindowWithCount_Default()
        {
            Observable.Range(1, 10).Window(3).Skip(1).First().SequenceEqual(new[] { 4, 5, 6 }.ToObservable());
            Observable.Range(1, 10).Window(3).Skip(1).First().SequenceEqual(new[] { 4, 5, 6 }.ToObservable());
            Observable.Range(1, 10).Window(3, 2).Skip(1).First().SequenceEqual(new[] { 3, 4, 5 }.ToObservable());
        }

        #endregion

        #region + Timed +

        [Fact]
        public void Window_Time_Basic()
        {
            var scheduler = new TestScheduler();

            var xs = scheduler.CreateHotObservable(
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

            var res = scheduler.Start(() =>
                xs.Window(TimeSpan.FromTicks(100), scheduler).Select((ys, i) => ys.Select(y => i + " " + y).Concat(Observable.Return(i + " end"))).Merge()
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

        [Fact]
        public void Window_Time_Basic_Periodic()
        {
            var scheduler = new PeriodicTestScheduler();

            var xs = scheduler.CreateHotObservable(
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

            var res = scheduler.Start(() =>
                xs.Window(TimeSpan.FromTicks(100), scheduler).Select((ys, i) => ys.Select(y => i + " " + y).Concat(Observable.Return(i + " end"))).Merge()
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

#if !WINDOWS
            scheduler.Timers.AssertEqual(
                new TimerRun(200, 490) { 300, 400 }
            );
#endif
        }

        [Fact]
        public void Window_Time_Basic_Periodic_Error()
        {
            var ex = new Exception();

            var scheduler = new PeriodicTestScheduler();

            var xs = scheduler.CreateHotObservable(
                OnNext(150, 1),
                OnNext(210, 2),
                OnNext(240, 3),
                OnNext(270, 4),
                OnNext(320, 5),
                OnNext(360, 6),
                OnNext(390, 7),
                OnNext(410, 8),
                OnError<int>(460, ex)
            );

            var res = scheduler.Start(() =>
                xs.Window(TimeSpan.FromTicks(100), scheduler).Select((ys, i) => ys.Select(y => i + " " + y).Concat(Observable.Return(i + " end"))).Merge()
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
                OnError<string>(460, ex)
            );

            xs.Subscriptions.AssertEqual(
                Subscribe(200, 460)
            );

#if !WINDOWS
            scheduler.Timers.AssertEqual(
                new TimerRun(200, 460) { 300, 400 }
            );
#endif
        }

        [Fact]
        public void Window_Time_Basic_Both()
        {
            var scheduler = new TestScheduler();

            var xs = scheduler.CreateHotObservable(
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

            var res = scheduler.Start(() =>
                xs.Window(TimeSpan.FromTicks(100), TimeSpan.FromTicks(50), scheduler).Select((ys, i) => ys.Select(y => i + " " + y).Concat(Observable.Return(i + " end"))).Merge()
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

        [Fact]
        public void WindowWithTime_ArgumentChecking()
        {
            ReactiveAssert.Throws<ArgumentNullException>(() => Observable.Window(default(IObservable<int>), TimeSpan.FromTicks(1), TimeSpan.FromTicks(1), DummyScheduler.Instance));
            ReactiveAssert.Throws<ArgumentOutOfRangeException>(() => Observable.Window(DummyObservable<int>.Instance, TimeSpan.FromTicks(-1), TimeSpan.FromTicks(1), DummyScheduler.Instance));
            ReactiveAssert.Throws<ArgumentOutOfRangeException>(() => Observable.Window(DummyObservable<int>.Instance, TimeSpan.FromTicks(1), TimeSpan.FromTicks(-1), DummyScheduler.Instance));
            ReactiveAssert.Throws<ArgumentNullException>(() => Observable.Window(DummyObservable<int>.Instance, TimeSpan.FromTicks(1), TimeSpan.FromTicks(1), null));
            ReactiveAssert.Throws<ArgumentNullException>(() => Observable.Window(default(IObservable<int>), TimeSpan.FromTicks(1), TimeSpan.FromTicks(1)));
            ReactiveAssert.Throws<ArgumentOutOfRangeException>(() => Observable.Window(DummyObservable<int>.Instance, TimeSpan.FromTicks(-1), TimeSpan.FromTicks(1)));
            ReactiveAssert.Throws<ArgumentOutOfRangeException>(() => Observable.Window(DummyObservable<int>.Instance, TimeSpan.FromTicks(1), TimeSpan.FromTicks(-1)));
            ReactiveAssert.Throws<ArgumentNullException>(() => Observable.Window(default(IObservable<int>), TimeSpan.FromTicks(1), DummyScheduler.Instance));
            ReactiveAssert.Throws<ArgumentOutOfRangeException>(() => Observable.Window(DummyObservable<int>.Instance, TimeSpan.FromTicks(-1), DummyScheduler.Instance));
            ReactiveAssert.Throws<ArgumentNullException>(() => Observable.Window(DummyObservable<int>.Instance, TimeSpan.FromTicks(1), null));
            ReactiveAssert.Throws<ArgumentNullException>(() => Observable.Window(default(IObservable<int>), TimeSpan.FromTicks(1)));
            ReactiveAssert.Throws<ArgumentOutOfRangeException>(() => Observable.Window(DummyObservable<int>.Instance, TimeSpan.FromTicks(-1)));
        }

        [Fact]
        public void WindowWithTime_Basic1()
        {
            var scheduler = new TestScheduler();

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
                xs.Window(TimeSpan.FromTicks(100), TimeSpan.FromTicks(70), scheduler).Select((w, i) => w.Select(x => i.ToString() + " " + x.ToString())).Merge()
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

        [Fact]
        public void WindowWithTime_Basic2()
        {
            var scheduler = new TestScheduler();

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
                xs.Window(TimeSpan.FromTicks(70), TimeSpan.FromTicks(100), scheduler).Select((w, i) => w.Select(x => i.ToString() + " " + x.ToString())).Merge()
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

        [Fact]
        public void WindowWithTime_Error()
        {
            var scheduler = new TestScheduler();

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
                xs.Window(TimeSpan.FromTicks(100), TimeSpan.FromTicks(70), scheduler).Select((w, i) => w.Select(x => i.ToString() + " " + x.ToString())).Merge()
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

        [Fact]
        public void WindowWithTime_Disposed()
        {
            var scheduler = new TestScheduler();

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
                xs.Window(TimeSpan.FromTicks(100), TimeSpan.FromTicks(70), scheduler).Select((w, i) => w.Select(x => i.ToString() + " " + x.ToString())).Merge(),
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

        [Fact]
        public void WindowWithTime_Basic_Same()
        {
            var scheduler = new TestScheduler();

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
                xs.Window(TimeSpan.FromTicks(100), scheduler).Select((w, i) => w.Select(x => i.ToString() + " " + x.ToString())).Merge()
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

        [Fact]
        public void WindowWithTime_Default()
        {
            Observable.Range(0, 10).Window(TimeSpan.FromDays(1), TimeSpan.FromDays(1)).SelectMany(Observable.ToList).First().AssertEqual(Enumerable.Range(0, 10));
            Observable.Range(0, 10).Window(TimeSpan.FromDays(1)).SelectMany(Observable.ToList).First().AssertEqual(Enumerable.Range(0, 10));
        }

        [Fact]
        public void WindowWithTimeOrCount_ArgumentChecking()
        {
            ReactiveAssert.Throws<ArgumentNullException>(() => Observable.Window(default(IObservable<int>), TimeSpan.FromTicks(1), 1, DummyScheduler.Instance));
            ReactiveAssert.Throws<ArgumentOutOfRangeException>(() => Observable.Window(DummyObservable<int>.Instance, TimeSpan.FromTicks(-1), 1, DummyScheduler.Instance));
            ReactiveAssert.Throws<ArgumentOutOfRangeException>(() => Observable.Window(DummyObservable<int>.Instance, TimeSpan.FromTicks(1), 0, DummyScheduler.Instance));
            ReactiveAssert.Throws<ArgumentNullException>(() => Observable.Window(DummyObservable<int>.Instance, TimeSpan.FromTicks(1), 1, null));
            ReactiveAssert.Throws<ArgumentNullException>(() => Observable.Window(default(IObservable<int>), TimeSpan.FromTicks(1), 1));
            ReactiveAssert.Throws<ArgumentOutOfRangeException>(() => Observable.Window(DummyObservable<int>.Instance, TimeSpan.FromTicks(-1), 1));
            ReactiveAssert.Throws<ArgumentOutOfRangeException>(() => Observable.Window(DummyObservable<int>.Instance, TimeSpan.FromTicks(1), 0));
        }

        [Fact]
        public void WindowWithTimeOrCount_Basic()
        {
            var scheduler = new TestScheduler();

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
                xs.Window(TimeSpan.FromTicks(70), 3, scheduler).Select((w, i) => w.Select(x => i.ToString() + " " + x.ToString())).Merge()
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

        [Fact]
        public void WindowWithTimeOrCount_Error()
        {
            var scheduler = new TestScheduler();

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
                xs.Window(TimeSpan.FromTicks(70), 3, scheduler).Select((w, i) => w.Select(x => i.ToString() + " " + x.ToString())).Merge()
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

        [Fact]
        public void WindowWithTimeOrCount_Disposed()
        {
            var scheduler = new TestScheduler();

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
                xs.Window(TimeSpan.FromTicks(70), 3, scheduler).Select((w, i) => w.Select(x => i.ToString() + " " + x.ToString())).Merge(),
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

        [Fact]
        public void WindowWithTimeOrCount_Default()
        {
            Observable.Range(1, 10).Window(TimeSpan.FromDays(1), 3).Skip(1).First().SequenceEqual(Observable.Range(4, 3));
        }

        #endregion
    }
}

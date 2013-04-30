// Copyright (c) Microsoft Open Technologies, Inc. All rights reserved. See License.txt in the project root for license information.

using System;
using System.Collections.Generic;
using System.Linq;
using System.Reactive;
using System.Reactive.Concurrency;
using System.Reactive.Disposables;
using System.Reactive.Linq;
using System.Reactive.Subjects;
using System.Text;
using System.Threading;
using Microsoft.Reactive.Testing;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using ReactiveTests.Dummies;

namespace ReactiveTests.Tests
{
    [TestClass]
    public class ObservableTimeTest : ReactiveTest
    {
        #region + Buffer +

        [TestMethod]
        public void Buffer_Time_ArgumentChecking()
        {
            var scheduler = new TestScheduler();
            var someObservable = Observable.Empty<int>();

            ReactiveAssert.Throws<ArgumentNullException>(() => Observable.Buffer(default(IObservable<int>), TimeSpan.Zero));
            ReactiveAssert.Throws<ArgumentNullException>(() => Observable.Buffer(someObservable, TimeSpan.Zero, null));
            ReactiveAssert.Throws<ArgumentNullException>(() => Observable.Buffer(default(IObservable<int>), TimeSpan.Zero, scheduler));
            ReactiveAssert.Throws<ArgumentNullException>(() => Observable.Buffer(default(IObservable<int>), TimeSpan.Zero, TimeSpan.Zero));
            ReactiveAssert.Throws<ArgumentNullException>(() => Observable.Buffer(someObservable, TimeSpan.Zero, TimeSpan.Zero, null));
            ReactiveAssert.Throws<ArgumentNullException>(() => Observable.Buffer(default(IObservable<int>), TimeSpan.Zero, TimeSpan.Zero, scheduler));
        }

        [TestMethod]
        public void BufferWithTime_ArgumentChecking()
        {
            ReactiveAssert.Throws<ArgumentNullException>(() => Observable.Buffer(default(IObservable<int>), TimeSpan.FromTicks(1), TimeSpan.FromTicks(1), DummyScheduler.Instance));
            ReactiveAssert.Throws<ArgumentOutOfRangeException>(() => Observable.Buffer(DummyObservable<int>.Instance, TimeSpan.FromTicks(-1), TimeSpan.FromTicks(1), DummyScheduler.Instance));
            ReactiveAssert.Throws<ArgumentOutOfRangeException>(() => Observable.Buffer(DummyObservable<int>.Instance, TimeSpan.FromTicks(1), TimeSpan.FromTicks(-1), DummyScheduler.Instance));
            ReactiveAssert.Throws<ArgumentNullException>(() => Observable.Buffer(DummyObservable<int>.Instance, TimeSpan.FromTicks(1), TimeSpan.FromTicks(1), null));
            ReactiveAssert.Throws<ArgumentNullException>(() => Observable.Buffer(default(IObservable<int>), TimeSpan.FromTicks(1), TimeSpan.FromTicks(1)));
            ReactiveAssert.Throws<ArgumentOutOfRangeException>(() => Observable.Buffer(DummyObservable<int>.Instance, TimeSpan.FromTicks(-1), TimeSpan.FromTicks(1)));
            ReactiveAssert.Throws<ArgumentOutOfRangeException>(() => Observable.Buffer(DummyObservable<int>.Instance, TimeSpan.FromTicks(1), TimeSpan.FromTicks(-1)));
            ReactiveAssert.Throws<ArgumentNullException>(() => Observable.Buffer(default(IObservable<int>), TimeSpan.FromTicks(1), DummyScheduler.Instance));
            ReactiveAssert.Throws<ArgumentOutOfRangeException>(() => Observable.Buffer(DummyObservable<int>.Instance, TimeSpan.FromTicks(-1), DummyScheduler.Instance));
            ReactiveAssert.Throws<ArgumentNullException>(() => Observable.Buffer(DummyObservable<int>.Instance, TimeSpan.FromTicks(1), null));
            ReactiveAssert.Throws<ArgumentNullException>(() => Observable.Buffer(default(IObservable<int>), TimeSpan.FromTicks(1)));
            ReactiveAssert.Throws<ArgumentOutOfRangeException>(() => Observable.Buffer(DummyObservable<int>.Instance, TimeSpan.FromTicks(-1)));
        }

        [TestMethod]
        public void BufferWithTime_Basic1()
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
                xs.Buffer(TimeSpan.FromTicks(100), TimeSpan.FromTicks(70), scheduler).Select(x => string.Join(",", x.Select(xx => xx.ToString()).ToArray()))
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
                xs.Buffer(TimeSpan.FromTicks(70), TimeSpan.FromTicks(100), scheduler).Select(x => string.Join(",", x.Select(xx => xx.ToString()).ToArray()))
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
                xs.Buffer(TimeSpan.FromTicks(100), TimeSpan.FromTicks(70), scheduler).Select(x => string.Join(",", x.Select(xx => xx.ToString()).ToArray()))
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
                xs.Buffer(TimeSpan.FromTicks(100), TimeSpan.FromTicks(70), scheduler).Select(x => string.Join(",", x.Select(xx => xx.ToString()).ToArray())),
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
                xs.Buffer(TimeSpan.FromTicks(100), scheduler).Select(x => string.Join(",", x.Select(xx => xx.ToString()).ToArray()))
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
        public void BufferWithTime_Basic_Same_Periodic()
        {
            var scheduler = new PeriodicTestScheduler();

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
                xs.Buffer(TimeSpan.FromTicks(100), scheduler).Select(x => string.Join(",", x.Select(xx => xx.ToString()).ToArray()))
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

            scheduler.Timers.AssertEqual(
                new TimerRun(200, 600) { 300, 400, 500 }
            );
        }

        [TestMethod]
        public void BufferWithTime_Basic_Same_Periodic_Error()
        {
            var ex = new Exception();

            var scheduler = new PeriodicTestScheduler();

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
                OnError<int>(480, ex)
            );

            var res = scheduler.Start(() =>
                xs.Buffer(TimeSpan.FromTicks(100), scheduler).Select(x => string.Join(",", x.Select(xx => xx.ToString()).ToArray()))
            );

            res.Messages.AssertEqual(
                OnNext(300, "2,3,4"),
                OnNext(400, "5,6,7"),
                OnError<string>(480, ex)
            );

            xs.Subscriptions.AssertEqual(
                Subscribe(200, 480)
            );

            scheduler.Timers.AssertEqual(
                new TimerRun(200, 480) { 300, 400 }
            );
        }

        [TestMethod]
        public void BufferWithTime_Default()
        {
            Observable.Range(0, 10).Buffer(TimeSpan.FromDays(1), TimeSpan.FromDays(1)).First().AssertEqual(Enumerable.Range(0, 10));
            Observable.Range(0, 10).Buffer(TimeSpan.FromDays(1)).First().AssertEqual(Enumerable.Range(0, 10));
        }

        [TestMethod]
        public void BufferWithTimeOrCount_ArgumentChecking()
        {
            ReactiveAssert.Throws<ArgumentNullException>(() => Observable.Buffer(default(IObservable<int>), TimeSpan.FromTicks(1), 1, DummyScheduler.Instance));
            ReactiveAssert.Throws<ArgumentOutOfRangeException>(() => Observable.Buffer(DummyObservable<int>.Instance, TimeSpan.FromTicks(-1), 1, DummyScheduler.Instance));
            ReactiveAssert.Throws<ArgumentOutOfRangeException>(() => Observable.Buffer(DummyObservable<int>.Instance, TimeSpan.FromTicks(1), 0, DummyScheduler.Instance));
            ReactiveAssert.Throws<ArgumentNullException>(() => Observable.Buffer(DummyObservable<int>.Instance, TimeSpan.FromTicks(1), 1, null));
            ReactiveAssert.Throws<ArgumentNullException>(() => Observable.Buffer(default(IObservable<int>), TimeSpan.FromTicks(1), 1));
            ReactiveAssert.Throws<ArgumentOutOfRangeException>(() => Observable.Buffer(DummyObservable<int>.Instance, TimeSpan.FromTicks(-1), 1));
            ReactiveAssert.Throws<ArgumentOutOfRangeException>(() => Observable.Buffer(DummyObservable<int>.Instance, TimeSpan.FromTicks(1), 0));
        }

        [TestMethod]
        public void BufferWithTimeOrCount_Basic()
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
                xs.Buffer(TimeSpan.FromTicks(70), 3, scheduler).Select(x => string.Join(",", x.Select(xx => xx.ToString()).ToArray()))
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
                xs.Buffer(TimeSpan.FromTicks(70), 3, scheduler).Select(x => string.Join(",", x.Select(xx => xx.ToString()).ToArray()))
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
                xs.Buffer(TimeSpan.FromTicks(70), 3, scheduler).Select(x => string.Join(",", x.Select(xx => xx.ToString()).ToArray())),
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
        public void BufferWithTimeOrCount_Default()
        {
            Observable.Range(1, 10, DefaultScheduler.Instance).Buffer(TimeSpan.FromDays(1), 3).Skip(1).First().AssertEqual(4, 5, 6);
        }

        #endregion

        #region + Delay +

        [TestMethod]
        public void Delay_ArgumentChecking()
        {
            var scheduler = new TestScheduler();
            var someObservable = Observable.Empty<int>();

            ReactiveAssert.Throws<ArgumentNullException>(() => Observable.Delay(default(IObservable<int>), DateTimeOffset.Now));
            ReactiveAssert.Throws<ArgumentNullException>(() => Observable.Delay(default(IObservable<int>), TimeSpan.Zero));
            ReactiveAssert.Throws<ArgumentOutOfRangeException>(() => Observable.Delay(someObservable, TimeSpan.FromSeconds(-1)));

            ReactiveAssert.Throws<ArgumentNullException>(() => Observable.Delay(default(IObservable<int>), DateTimeOffset.Now, scheduler));
            ReactiveAssert.Throws<ArgumentNullException>(() => Observable.Delay(default(IObservable<int>), TimeSpan.Zero, scheduler));
            ReactiveAssert.Throws<ArgumentNullException>(() => Observable.Delay(someObservable, DateTimeOffset.Now, null));
            ReactiveAssert.Throws<ArgumentNullException>(() => Observable.Delay(someObservable, TimeSpan.Zero, null));
            ReactiveAssert.Throws<ArgumentOutOfRangeException>(() => Observable.Delay(someObservable, TimeSpan.FromSeconds(-1), scheduler));
        }

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
            var scheduler = new TestScheduler();

            var xs = scheduler.CreateHotObservable(
                OnNext(150, 1),
                OnNext(250, 2),
                OnNext(350, 3),
                OnNext(450, 4),
                OnCompleted<int>(550)
            );

            var res = scheduler.Start(() =>
                xs.Delay(TimeSpan.FromTicks(100), useStopwatch ? scheduler : scheduler.DisableOptimizations())
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
            var scheduler = new TestScheduler();

            var xs = scheduler.CreateHotObservable(
                OnNext(150, 1),
                OnNext(250, 2),
                OnNext(350, 3),
                OnNext(450, 4),
                OnCompleted<int>(550)
            );

            var res = scheduler.Start(() =>
                xs.Delay(new DateTimeOffset(300, TimeSpan.Zero), useStopwatch ? scheduler : scheduler.DisableOptimizations())
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
            var scheduler = new TestScheduler();

            var xs = scheduler.CreateHotObservable(
                OnNext(150, 1),
                OnNext(250, 2),
                OnNext(350, 3),
                OnNext(450, 4),
                OnCompleted<int>(550)
            );

            var res = scheduler.Start(() =>
                xs.Delay(TimeSpan.FromTicks(50), useStopwatch ? scheduler : scheduler.DisableOptimizations())
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
            var scheduler = new TestScheduler();

            var xs = scheduler.CreateHotObservable(
                OnNext(150, 1),
                OnNext(250, 2),
                OnNext(350, 3),
                OnNext(450, 4),
                OnCompleted<int>(550)
            );

            var res = scheduler.Start(() =>
                xs.Delay(new DateTimeOffset(250, TimeSpan.Zero), useStopwatch ? scheduler : scheduler.DisableOptimizations())
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
            var scheduler = new TestScheduler();

            var xs = scheduler.CreateHotObservable(
                OnNext(150, 1),
                OnNext(250, 2),
                OnNext(350, 3),
                OnNext(450, 4),
                OnCompleted<int>(550)
            );

            var res = scheduler.Start(() =>
                xs.Delay(TimeSpan.FromTicks(150), useStopwatch ? scheduler : scheduler.DisableOptimizations())
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
            var scheduler = new TestScheduler();

            var xs = scheduler.CreateHotObservable(
                OnNext(150, 1),
                OnNext(250, 2),
                OnNext(350, 3),
                OnNext(450, 4),
                OnCompleted<int>(550)
            );

            var res = scheduler.Start(() =>
                xs.Delay(new DateTimeOffset(350, TimeSpan.Zero), useStopwatch ? scheduler : scheduler.DisableOptimizations())
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
            var scheduler = new TestScheduler();

            var ex = new Exception();

            var xs = scheduler.CreateHotObservable(
                OnNext(150, 1),
                OnNext(250, 2),
                OnNext(350, 3),
                OnNext(450, 4),
                OnError<int>(550, ex)
            );

            var res = scheduler.Start(() =>
                xs.Delay(TimeSpan.FromTicks(50), useStopwatch ? scheduler : scheduler.DisableOptimizations())
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
            var scheduler = new TestScheduler();

            var ex = new Exception();

            var xs = scheduler.CreateHotObservable(
                OnNext(150, 1),
                OnNext(250, 2),
                OnNext(350, 3),
                OnNext(450, 4),
                OnError<int>(550, ex)
            );

            var res = scheduler.Start(() =>
                xs.Delay(new DateTimeOffset(250, TimeSpan.Zero), useStopwatch ? scheduler : scheduler.DisableOptimizations())
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
            var scheduler = new TestScheduler();

            var ex = new Exception();

            var xs = scheduler.CreateHotObservable(
                OnNext(150, 1),
                OnNext(250, 2),
                OnNext(350, 3),
                OnNext(450, 4),
                OnError<int>(550, ex)
            );

            var res = scheduler.Start(() =>
                xs.Delay(TimeSpan.FromTicks(150), useStopwatch ? scheduler : scheduler.DisableOptimizations())
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
            var scheduler = new TestScheduler();

            var ex = new Exception();

            var xs = scheduler.CreateHotObservable(
                OnNext(150, 1),
                OnNext(250, 2),
                OnNext(350, 3),
                OnNext(450, 4),
                OnError<int>(550, ex)
            );

            var res = scheduler.Start(() =>
                xs.Delay(new DateTimeOffset(350, TimeSpan.Zero), useStopwatch ? scheduler : scheduler.DisableOptimizations())
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
        public void Delay_TimeSpan_Real_Simple1()
        {
            Delay_TimeSpan_Real_Simple1_Impl(ThreadPoolScheduler.Instance.DisableOptimizations());
        }

        [TestMethod]
        public void Delay_TimeSpan_Real_Simple1_Stopwatch()
        {
            Delay_TimeSpan_Real_Simple1_Impl(ThreadPoolScheduler.Instance);
        }

        private void Delay_TimeSpan_Real_Simple1_Impl(IScheduler scheduler)
        {
            var s = new Subject<int>();

            var res = s.Delay(TimeSpan.FromMilliseconds(10), scheduler);

            var lst = new List<int>();
            var e = new ManualResetEvent(false);
            res.Subscribe(lst.Add, () => e.Set());

            new Thread(() =>
            {
                s.OnNext(1);
                s.OnNext(2);
                s.OnNext(3);
                s.OnCompleted();
            }).Start();

            e.WaitOne();
            Assert.IsTrue(new[] { 1, 2, 3 }.SequenceEqual(lst));
        }

        [TestMethod]
        public void Delay_TimeSpan_Real_Error1()
        {
            Delay_TimeSpan_Real_Error1_Impl(ThreadPoolScheduler.Instance.DisableOptimizations());
        }

        [TestMethod]
        public void Delay_TimeSpan_Real_Error1_Stopwatch()
        {
            Delay_TimeSpan_Real_Error1_Impl(ThreadPoolScheduler.Instance);
        }

        private void Delay_TimeSpan_Real_Error1_Impl(IScheduler scheduler)
        {
            var ex = new Exception();

            var s = new Subject<int>();

            var res = s.Delay(TimeSpan.FromMilliseconds(10), scheduler);

            var e = new ManualResetEvent(false);
            var err = default(Exception);
            res.Subscribe(_ => { }, ex_ => { err = ex_; e.Set(); });

            new Thread(() =>
            {
                s.OnNext(1);
                s.OnNext(2);
                s.OnNext(3);
                s.OnError(ex);
            }).Start();

            e.WaitOne();
            Assert.AreSame(ex, err);
        }

        [TestMethod]
        public void Delay_TimeSpan_Real_Error2()
        {
            Delay_TimeSpan_Real_Error2_Impl(ThreadPoolScheduler.Instance.DisableOptimizations());
        }

        [TestMethod]
        public void Delay_TimeSpan_Real_Error2_Stopwatch()
        {
            Delay_TimeSpan_Real_Error2_Impl(ThreadPoolScheduler.Instance);
        }

        private void Delay_TimeSpan_Real_Error2_Impl(IScheduler scheduler)
        {
            var ex = new Exception();

            var s = new Subject<int>();

            var res = s.Delay(TimeSpan.FromMilliseconds(10), scheduler);

            var next = new ManualResetEvent(false);
            var e = new ManualResetEvent(false);
            var err = default(Exception);
            res.Subscribe(_ => { next.Set(); }, ex_ => { err = ex_; e.Set(); });

            new Thread(() =>
            {
                s.OnNext(1);
                next.WaitOne();

                s.OnError(ex);
            }).Start();

            e.WaitOne();
            Assert.AreSame(ex, err);
        }

        [TestMethod]
        public void Delay_TimeSpan_Real_Error3()
        {
            Delay_TimeSpan_Real_Error3_Impl(ThreadPoolScheduler.Instance.DisableOptimizations());
        }

        [TestMethod]
        public void Delay_TimeSpan_Real_Error3_Stopwatch()
        {
            Delay_TimeSpan_Real_Error3_Impl(ThreadPoolScheduler.Instance);
        }

        private void Delay_TimeSpan_Real_Error3_Impl(IScheduler scheduler)
        {
            var ex = new Exception();

            var s = new Subject<int>();

            var res = s.Delay(TimeSpan.FromMilliseconds(10), scheduler);

            var next = new ManualResetEvent(false);
            var ack = new ManualResetEvent(false);

            var e = new ManualResetEvent(false);
            var err = default(Exception);
            res.Subscribe(_ => { next.Set(); ack.WaitOne(); }, ex_ => { err = ex_; e.Set(); });

            new Thread(() =>
            {
                s.OnNext(1);
                next.WaitOne();

                s.OnError(ex);
                ack.Set();
            }).Start();

            e.WaitOne();
            Assert.AreSame(ex, err);
        }

        [TestMethod]
        public void Delay_TimeSpan_Positive()
        {
            var scheduler = new TestScheduler();

            var msgs = new[] {
                OnNext(150, 1),
                OnNext(250, 2),
                OnNext(350, 3),
                OnNext(450, 4),
                OnCompleted<int>(550)
            };

            var xs = scheduler.CreateHotObservable(msgs);

            const ushort delay = 42;

            var res = scheduler.Start(() =>
                xs.Delay(TimeSpan.FromTicks(delay), scheduler)
            );

            var expected = from n in msgs
                           where n.Time > ObservableTest.Subscribed
                           select new Recorded<Notification<int>>((ushort)(n.Time + delay), n.Value);

            res.Messages.AssertEqual(expected);
        }

        [TestMethod]
        public void Delay_Empty()
        {
            var scheduler = new TestScheduler();

            var xs = scheduler.CreateHotObservable(
                OnNext(150, 1),
                OnCompleted<int>(550)
            );

            var res = scheduler.Start(() =>
                xs.Delay(TimeSpan.FromTicks(10), scheduler)
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
            var scheduler = new TestScheduler();

            var ex = new Exception();

            var xs = scheduler.CreateHotObservable(
                OnNext(150, 1),
                OnError<int>(550, ex)
            );

            var res = scheduler.Start(() =>
                xs.Delay(TimeSpan.FromTicks(10), scheduler)
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
            var scheduler = new TestScheduler();

            var xs = scheduler.CreateHotObservable(
                OnNext(150, 1)
            );

            var res = scheduler.Start(() =>
                xs.Delay(TimeSpan.FromTicks(10), scheduler)
            );

            res.Messages.AssertEqual(
            );

            xs.Subscriptions.AssertEqual(
                Subscribe(200, 1000)
            );
        }

        [TestMethod]
        public void Delay_TimeSpan_DefaultScheduler()
        {
            Assert.IsTrue(Observable.Return(1).Delay(TimeSpan.FromMilliseconds(1)).ToEnumerable().SequenceEqual(new[] { 1 }));
        }

        [TestMethod]
        public void Delay_DateTimeOffset_DefaultScheduler()
        {
            Assert.IsTrue(Observable.Return(1).Delay(DateTimeOffset.UtcNow + TimeSpan.FromMilliseconds(1)).ToEnumerable().SequenceEqual(new[] { 1 }));
        }

        [TestMethod]
        public void Delay_CrossingMessages()
        {
            var lst = new List<int>();

            var evt = new ManualResetEvent(false);

            var s = new Subject<int>();
            s.Delay(TimeSpan.FromSeconds(0.01)).Subscribe(x =>
            {
                lst.Add(x);
                if (x < 9)
                    s.OnNext(x + 1);
                else
                    s.OnCompleted();
            }, () =>
            {
                evt.Set();
            });
            s.OnNext(0);

            evt.WaitOne();

            Assert.IsTrue(Enumerable.Range(0, 10).SequenceEqual(lst));
        }

        [TestMethod]
        public void Delay_Duration_ArgumentChecking()
        {
            var someObservable = DummyObservable<int>.Instance;

            ReactiveAssert.Throws<ArgumentNullException>(() => Observable.Delay(default(IObservable<int>), x => someObservable));
            ReactiveAssert.Throws<ArgumentNullException>(() => Observable.Delay(someObservable, default(Func<int, IObservable<int>>)));
            ReactiveAssert.Throws<ArgumentNullException>(() => Observable.Delay(default(IObservable<int>), someObservable, x => someObservable));
            ReactiveAssert.Throws<ArgumentNullException>(() => Observable.Delay(someObservable, default(IObservable<int>), x => someObservable));
            ReactiveAssert.Throws<ArgumentNullException>(() => Observable.Delay(someObservable, someObservable, default(Func<int, IObservable<int>>)));
        }

        [TestMethod]
        public void Delay_Duration_Simple1()
        {
            var scheduler = new TestScheduler();

            var xs = scheduler.CreateHotObservable(
                OnNext(150, 1),
                OnNext(210, 10),
                OnNext(220, 30),
                OnNext(230, 50),
                OnNext(240, 35),
                OnNext(250, 20),
                OnCompleted<int>(260)
            );

            var res = scheduler.Start(() =>
                xs.Delay(x => scheduler.CreateColdObservable(new[] { OnNext(x, "!") }))
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
            var scheduler = new TestScheduler();

            var xs = scheduler.CreateHotObservable(
                OnNext(150, 1),
                OnNext(210, 2),
                OnNext(220, 3),
                OnNext(230, 4),
                OnNext(240, 5),
                OnNext(250, 6),
                OnCompleted<int>(300)
            );

            var ys = scheduler.CreateColdObservable(
                OnNext(10, "!")
            );

            var res = scheduler.Start(() =>
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
            var scheduler = new TestScheduler();

            var xs = scheduler.CreateHotObservable(
                OnNext(150, 1),
                OnNext(210, 2),
                OnNext(220, 3),
                OnNext(230, 4),
                OnNext(240, 5),
                OnNext(250, 6),
                OnCompleted<int>(300)
            );

            var ys = scheduler.CreateColdObservable(
                OnNext(100, "!")
            );

            var res = scheduler.Start(() =>
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
            var scheduler = new TestScheduler();

            var xs = scheduler.CreateHotObservable(
                OnNext(150, 1),
                OnNext(210, 2),
                OnNext(220, 3),
                OnNext(230, 4),
                OnNext(240, 5),
                OnNext(250, 6),
                OnCompleted<int>(300)
            );

            var ys = scheduler.CreateColdObservable(
                OnCompleted<int>(100)
            );

            var res = scheduler.Start(() =>
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
            var scheduler = new TestScheduler();

            var xs = scheduler.CreateHotObservable(
                OnNext(150, 1),
                OnNext(210, 2),
                OnNext(220, 3),
                OnNext(230, 4),
                OnNext(240, 5),
                OnNext(250, 6),
                OnCompleted<int>(300)
            );

            var ys = scheduler.CreateColdObservable(
                OnNext(200, "!")
            );

            var res = scheduler.Start(() =>
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
            var scheduler = new TestScheduler();

            var xs = scheduler.CreateHotObservable(
                OnNext(150, 1),
                OnNext(210, 2),
                OnNext(400, 3),
                OnCompleted<int>(500)
            );

            var ys = scheduler.CreateColdObservable(
                OnNext(50, "!")
            );

            var res = scheduler.Start(() =>
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
            var scheduler = new TestScheduler();

            var ex = new Exception();

            var xs = scheduler.CreateHotObservable(
                OnNext(150, 1),
                OnNext(210, 2),
                OnNext(220, 3),
                OnNext(230, 4),
                OnNext(240, 5),
                OnNext(250, 6),
                OnError<int>(300, ex)
            );

            var ys = scheduler.CreateColdObservable(
                OnNext(100, "!")
            );

            var res = scheduler.Start(() =>
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
            var scheduler = new TestScheduler();

            var ex = new Exception();

            var xs = scheduler.CreateHotObservable(
                OnNext(150, 1),
                OnNext(210, 2),
                OnNext(220, 3),
                OnNext(230, 4),
                OnNext(240, 5),
                OnNext(250, 6),
                OnError<int>(300, ex)
            );

            var ys = scheduler.CreateColdObservable(
                OnNext(10, "!")
            );

            var res = scheduler.Start(() =>
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
            var scheduler = new TestScheduler();

            var ex = new Exception();

            var xs = scheduler.CreateHotObservable(
                OnNext(150, 1),
                OnNext(210, 2),
                OnNext(220, 3),
                OnNext(230, 4),
                OnNext(240, 5),
                OnNext(250, 6),
                OnCompleted<int>(300)
            );

            var ys = scheduler.CreateColdObservable(
                OnNext(30, "!")
            );

            var zs = scheduler.CreateColdObservable(
                OnError<string>(25, ex)
            );

            var res = scheduler.Start(() =>
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
            var scheduler = new TestScheduler();

            var ex = new Exception();

            var xs = scheduler.CreateHotObservable(
                OnNext(150, 1),
                OnNext(210, 2),
                OnNext(220, 3),
                OnNext(230, 4),
                OnNext(240, 5),
                OnNext(250, 6),
                OnCompleted<int>(300)
            );

            var ys = scheduler.CreateColdObservable(
                OnError<string>(100, ex)
            );

            var res = scheduler.Start(() =>
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
            var scheduler = new TestScheduler();

            var ex = new Exception();

            var xs = scheduler.CreateHotObservable(
                OnNext(150, 1),
                OnNext(250, 2),
                OnNext(300, 3),
                OnNext(350, 4),
                OnNext(400, 5),
                OnNext(450, 6),
                OnCompleted<int>(500)
            );

            var ys = scheduler.CreateColdObservable(
                OnNext<string>(80, "")
            );

            var res = scheduler.Start(() =>
                xs.Delay(x =>
                {
                    if (x == 4)
                        throw ex;

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
            var scheduler = new TestScheduler();

            var xs = scheduler.CreateHotObservable(
                OnNext(150, 1),
                OnNext(250, 2),
                OnNext(350, 3),
                OnNext(450, 4),
                OnCompleted<int>(550)
            );

            var res = scheduler.Start(() =>
                xs.Delay(x =>
                    scheduler.CreateColdObservable(
                        OnNext(x * 10, "Ignore"),
                        OnNext(x * 10 + 5, "Aargh!")
                    )
                )
            );

            res.Messages.AssertEqual(
                OnNext<int>(250 + 2 * 10, 2),
                OnNext<int>(350 + 3 * 10, 3),
                OnNext<int>(450 + 4 * 10, 4),
                OnCompleted<int>(550)
            );

            xs.Subscriptions.AssertEqual(
                Subscribe(200, 550)
            );
        }

        [TestMethod]
        public void Delay_Duration_DeferOnCompleted()
        {
            var scheduler = new TestScheduler();

            var xs = scheduler.CreateHotObservable(
                OnNext(150, 1),
                OnNext(250, 2),
                OnNext(350, 3),
                OnNext(450, 4),
                OnCompleted<int>(451)
            );

            var res = scheduler.Start(() =>
                xs.Delay(x =>
                    scheduler.CreateColdObservable(
                        OnNext(x * 10, "Ignore"),
                        OnNext(x * 10 + 5, "Aargh!")
                    )
                )
            );

            res.Messages.AssertEqual(
                OnNext<int>(250 + 2 * 10, 2),
                OnNext<int>(350 + 3 * 10, 3),
                OnNext<int>(450 + 4 * 10, 4),
                OnCompleted<int>(450 + 4 * 10)
            );

            xs.Subscriptions.AssertEqual(
                Subscribe(200, 451)
            );
        }

        [TestMethod]
        public void Delay_Duration_InnerError()
        {
            var scheduler = new TestScheduler();

            var xs = scheduler.CreateHotObservable(
                OnNext(150, 1),
                OnNext(250, 2),
                OnNext(350, 3),
                OnNext(450, 4),
                OnCompleted<int>(451)
            );

            var ex = new Exception();

            var res = scheduler.Start(() =>
                xs.Delay(x =>
                    x < 4 ? scheduler.CreateColdObservable(
                                OnNext(x * 10, "Ignore"),
                                OnNext(x * 10 + 5, "Aargh!")
                            )
                          : scheduler.CreateColdObservable(
                                OnError<string>(x * 10, ex)
                            )
                )
            );

            res.Messages.AssertEqual(
                OnNext<int>(250 + 2 * 10, 2),
                OnNext<int>(350 + 3 * 10, 3),
                OnError<int>(450 + 4 * 10, ex)
            );

            xs.Subscriptions.AssertEqual(
                Subscribe(200, 451)
            );
        }

        [TestMethod]
        public void Delay_Duration_OuterError()
        {
            var scheduler = new TestScheduler();

            var ex = new Exception();

            var xs = scheduler.CreateHotObservable(
                OnNext(150, 1),
                OnNext(250, 2),
                OnNext(350, 3),
                OnNext(450, 4),
                OnError<int>(460, ex)
            );

            var res = scheduler.Start(() =>
                xs.Delay(x =>
                    scheduler.CreateColdObservable(
                        OnNext(x * 10, "Ignore"),
                        OnNext(x * 10 + 5, "Aargh!")
                    )
                )
            );

            res.Messages.AssertEqual(
                OnNext<int>(250 + 2 * 10, 2),
                OnNext<int>(350 + 3 * 10, 3),
                OnError<int>(460, ex)
            );

            xs.Subscriptions.AssertEqual(
                Subscribe(200, 460)
            );
        }

        [TestMethod]
        public void Delay_Duration_SelectorThrows2()
        {
            var scheduler = new TestScheduler();

            var xs = scheduler.CreateHotObservable(
                OnNext(150, 1),
                OnNext(250, 2),
                OnNext(350, 3),
                OnNext(450, 4),
                OnCompleted<int>(550)
            );

            var ex = new Exception();

            var res = scheduler.Start(() =>
                xs.Delay(x =>
                {
                    if (x < 4)
                    {
                        return scheduler.CreateColdObservable(
                                OnNext(x * 10, "Ignore"),
                                OnNext(x * 10 + 5, "Aargh!")
                            );
                    }
                    else
                        throw ex;
                })
            );

            res.Messages.AssertEqual(
                OnNext<int>(250 + 2 * 10, 2),
                OnNext<int>(350 + 3 * 10, 3),
                OnError<int>(450, ex)
            );

            xs.Subscriptions.AssertEqual(
                Subscribe(200, 450)
            );
        }

        [TestMethod]
        public void Delay_Duration_InnerDone()
        {
            var scheduler = new TestScheduler();

            var xs = scheduler.CreateHotObservable(
                OnNext(150, 1),
                OnNext(250, 2),
                OnNext(350, 3),
                OnNext(450, 4),
                OnCompleted<int>(550)
            );

            var ex = new Exception();

            var res = scheduler.Start(() =>
                xs.Delay(x =>
                    scheduler.CreateColdObservable(
                        OnCompleted<string>(x * 10)
                    )
                )
            );

            res.Messages.AssertEqual(
                OnNext<int>(250 + 2 * 10, 2),
                OnNext<int>(350 + 3 * 10, 3),
                OnNext<int>(450 + 4 * 10, 4),
                OnCompleted<int>(550)
            );

            xs.Subscriptions.AssertEqual(
                Subscribe(200, 550)
            );
        }

        [TestMethod]
        public void Delay_Duration_InnerSubscriptionTimes()
        {
            var scheduler = new TestScheduler();

            var xs = scheduler.CreateHotObservable(
                OnNext(150, -1),
                OnNext(250, 0),
                OnNext(350, 1),
                OnNext(450, 2),
                OnCompleted<int>(550)
            );

            var ys = new[] {
                scheduler.CreateColdObservable(
                    OnNext(20, 42),
                    OnNext(25, 99)
                ),
                scheduler.CreateColdObservable(
                    OnNext(10, 43),
                    OnNext(15, 99)
                ),
                scheduler.CreateColdObservable(
                    OnNext(30, 44),
                    OnNext(35, 99)
                ),
            };

            var res = scheduler.Start(() =>
                xs.Delay(x => ys[x])
            );

            res.Messages.AssertEqual(
                OnNext<int>(250 + 20, 0),
                OnNext<int>(350 + 10, 1),
                OnNext<int>(450 + 30, 2),
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
            var scheduler = new TestScheduler();

            var xs = scheduler.CreateHotObservable(
                OnNext(150, 1),
                OnNext(210, 10),
                OnNext(220, 30),
                OnNext(230, 50),
                OnNext(240, 35),
                OnNext(250, 20),
                OnCompleted<int>(260)
            );

            var ys = scheduler.CreateColdObservable(
                OnNext(10, "!")
            );

            var res = scheduler.Start(() =>
                xs.Delay(ys, x => scheduler.CreateColdObservable(new[] { OnNext(x, "!") }))
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
            var scheduler = new TestScheduler();

            var xs = scheduler.CreateHotObservable(
                OnNext(150, 1),
                OnNext(210, 10),
                OnNext(220, 30),
                OnNext(230, 50),
                OnNext(240, 35),
                OnNext(250, 20),
                OnCompleted<int>(260)
            );

            var ys = scheduler.CreateColdObservable(
                OnCompleted<string>(10)
            );

            var res = scheduler.Start(() =>
                xs.Delay(ys, x => scheduler.CreateColdObservable(new[] { OnNext(x, "!") }))
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
            var scheduler = new TestScheduler();

            var xs = scheduler.CreateHotObservable(
                OnNext(150, 1),
                OnNext(210, 10),
                OnNext(220, 30),
                OnNext(230, 50),
                OnNext(240, 35),
                OnNext(250, 20),
                OnCompleted<int>(260)
            );

            var ys = scheduler.CreateColdObservable(
                OnNext<string>(10, "!")
            );

            var res = scheduler.Start(() =>
                xs.Delay(ys, x => scheduler.CreateColdObservable(new[] { OnNext(x, "!") })),
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
            var scheduler = new TestScheduler();

            var xs = scheduler.CreateHotObservable(
                OnNext(150, 1),
                OnNext(210, 10),
                OnNext(220, 30),
                OnNext(230, 50),
                OnNext(240, 35),
                OnNext(250, 20),
                OnCompleted<int>(260)
            );

            var ys = scheduler.CreateColdObservable(
                OnNext<string>(100, "!")
            );

            var res = scheduler.Start(() =>
                xs.Delay(ys, x => scheduler.CreateColdObservable(new[] { OnNext(x, "!") })),
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
            var scheduler = new TestScheduler();

            var ex = new Exception();

            var xs = scheduler.CreateHotObservable(
                OnNext(150, 1),
                OnNext(210, 10),
                OnNext(220, 30),
                OnNext(230, 50),
                OnNext(240, 35),
                OnNext(250, 20),
                OnCompleted<int>(260)
            );

            var ys = scheduler.CreateColdObservable(
                OnError<string>(10, ex)
            );

            var res = scheduler.Start(() =>
                xs.Delay(ys, x => scheduler.CreateColdObservable(new[] { OnNext(x, "!") }))
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
        public void Delay_ErrorHandling1()
        {
            //
            // Checks for race condition between OnNext and OnError where the latter has a chance to
            // send out the OnError message before the former gets a chance to run in the delayed
            // queue. In that case, the OnNext message should not come out. 
            //
            // See DrainQueue's first _hasFailed check.
            //

            var xs = Observable.Create<int>(observer =>
            {
                observer.OnNext(42);
                observer.OnError(new Exception());
                return () => { };
            });

            var s = new ImpulseScheduler();

            var called = false;
            var failed = new ManualResetEvent(false);
            xs.Delay(TimeSpan.FromDays(1), s).Subscribe(_ => { called = true; }, ex => { failed.Set(); });

            failed.WaitOne();
            s.Event.Set();
            s.Done.WaitOne();

            Assert.IsFalse(called);
        }

        class ImpulseScheduler : IScheduler
        {
            public DateTimeOffset Now
            {
                get { return DateTimeOffset.UtcNow; }
            }

            public IDisposable Schedule<TState>(TState state, Func<IScheduler, TState, IDisposable> action)
            {
                throw new NotImplementedException();
            }

            private ManualResetEvent _event = new ManualResetEvent(false);
            private ManualResetEvent _done = new ManualResetEvent(false);

            public ManualResetEvent Event { get { return _event; } }
            public ManualResetEvent Done { get { return _done; } }

            public IDisposable Schedule<TState>(TState state, TimeSpan dueTime, Func<IScheduler, TState, IDisposable> action)
            {
                Scheduler.Default.Schedule(() =>
                {
                    _event.WaitOne();
                    action(this, state);
                    _done.Set();
                });

                return Disposable.Empty;
            }

            public IDisposable Schedule<TState>(TState state, DateTimeOffset dueTime, Func<IScheduler, TState, IDisposable> action)
            {
                throw new NotImplementedException();
            }
        }

        [TestMethod]
        public void Delay_LongRunning_CancelEarly()
        {
            var xs = Observable.Create<int>(observer =>
            {
                return Scheduler.Default.Schedule(TimeSpan.FromHours(1), () =>
                {
                    observer.OnNext(42);
                });
            });

            var s = new ManualResetEvent(false);
            var e = new ManualResetEvent(false);

            var ys = xs.Delay(TimeSpan.Zero, new MyLongRunning1(s, e));

            var d = ys.Subscribe(_ => { });

            s.WaitOne();
            d.Dispose();
            e.WaitOne();
        }

        class MyLongRunning1 : LocalScheduler, ISchedulerLongRunning
        {
            private ManualResetEvent _start;
            private ManualResetEvent _stop;

            public MyLongRunning1(ManualResetEvent start, ManualResetEvent stop)
            {
                _start = start;
                _stop = stop;
            }

            public IDisposable ScheduleLongRunning<TState>(TState state, Action<TState, ICancelable> action)
            {
                var b = new BooleanDisposable();
                new Thread(() =>
                {
                    _start.Set();
                    action(state, b);
                    _stop.Set();
                }).Start();
                return b;
            }

            public override IDisposable Schedule<TState>(TState state, TimeSpan dueTime, Func<IScheduler, TState, IDisposable> action)
            {
                throw new NotImplementedException();
            }
        }

        [TestMethod]
        public void Delay_LongRunning_CancelLate()
        {
            var xs = Observable.Return(42);

            var s = new ManualResetEvent(false);
            var e = new ManualResetEvent(false);

            var ys = xs.Delay(TimeSpan.FromHours(1), new MyLongRunning2(s, e));

            var d = ys.Subscribe(_ => { });

            s.WaitOne();
            d.Dispose();
            e.WaitOne();
        }

        class MyLongRunning2 : LocalScheduler, ISchedulerLongRunning
        {
            private ManualResetEvent _start;
            private ManualResetEvent _stop;

            public MyLongRunning2(ManualResetEvent start, ManualResetEvent stop)
            {
                _start = start;
                _stop = stop;
            }

            public IDisposable ScheduleLongRunning<TState>(TState state, Action<TState, ICancelable> action)
            {
                var b = new BooleanDisposable();
                new Thread(() =>
                {
                    action(state, b);
                    _stop.Set();
                }).Start();
                return b;
            }

            public override IDisposable Schedule<TState>(TState state, TimeSpan dueTime, Func<IScheduler, TState, IDisposable> action)
            {
                _start.Set();
                return Disposable.Empty;
            }
        }

        #endregion

        #region + DelaySubscription +

        [TestMethod]
        public void DelaySubscription_ArgumentChecking()
        {
            ReactiveAssert.Throws<ArgumentNullException>(() => Observable.DelaySubscription(default(IObservable<int>), DateTimeOffset.Now));
            ReactiveAssert.Throws<ArgumentNullException>(() => Observable.DelaySubscription(default(IObservable<int>), DateTimeOffset.Now, Scheduler.Immediate));
            ReactiveAssert.Throws<ArgumentNullException>(() => Observable.DelaySubscription(DummyObservable<int>.Instance, DateTimeOffset.Now, default(IScheduler)));

            ReactiveAssert.Throws<ArgumentNullException>(() => Observable.DelaySubscription(default(IObservable<int>), TimeSpan.Zero));
            ReactiveAssert.Throws<ArgumentNullException>(() => Observable.DelaySubscription(default(IObservable<int>), TimeSpan.Zero, Scheduler.Immediate));
            ReactiveAssert.Throws<ArgumentNullException>(() => Observable.DelaySubscription(DummyObservable<int>.Instance, TimeSpan.Zero, default(IScheduler)));

            ReactiveAssert.Throws<ArgumentOutOfRangeException>(() => Observable.DelaySubscription(DummyObservable<int>.Instance, TimeSpan.FromSeconds(-1)));
            ReactiveAssert.Throws<ArgumentOutOfRangeException>(() => Observable.DelaySubscription(DummyObservable<int>.Instance, TimeSpan.FromSeconds(-1), Scheduler.Immediate));
        }

        [TestMethod]
        public void DelaySubscription_TimeSpan_Default()
        {
            var lst = new List<int>();
            Observable.Range(0, 10).DelaySubscription(TimeSpan.FromMilliseconds(1)).ForEach(lst.Add);
            Assert.IsTrue(Enumerable.Range(0, 10).SequenceEqual(lst));
        }

        [TestMethod]
        public void DelaySubscription_TimeSpan_Simple()
        {
            var scheduler = new TestScheduler();

            var xs = scheduler.CreateColdObservable(
                OnNext(50, 42),
                OnNext(60, 43),
                OnCompleted<int>(70)
            );

            var res = scheduler.Start(() =>
                xs.DelaySubscription(TimeSpan.FromTicks(30), scheduler)
            );

            res.Messages.AssertEqual(
                OnNext(280, 42),
                OnNext(290, 43),
                OnCompleted<int>(300)
            );

            xs.Subscriptions.AssertEqual(
                Subscribe(230, 300)
            );
        }

        [TestMethod]
        public void DelaySubscription_TimeSpan_Error()
        {
            var ex = new Exception();

            var scheduler = new TestScheduler();

            var xs = scheduler.CreateColdObservable(
                OnNext(50, 42),
                OnNext(60, 43),
                OnError<int>(70, ex)
            );

            var res = scheduler.Start(() =>
                xs.DelaySubscription(TimeSpan.FromTicks(30), scheduler)
            );

            res.Messages.AssertEqual(
                OnNext(280, 42),
                OnNext(290, 43),
                OnError<int>(300, ex)
            );

            xs.Subscriptions.AssertEqual(
                Subscribe(230, 300)
            );
        }

        [TestMethod]
        public void DelaySubscription_DateTimeOffset_Default()
        {
            var lst = new List<int>();
            Observable.Range(0, 10).DelaySubscription(DateTimeOffset.UtcNow.AddMilliseconds(1)).ForEach(lst.Add);
            Assert.IsTrue(Enumerable.Range(0, 10).SequenceEqual(lst));
        }

        [TestMethod]
        public void DelaySubscription_DateTimeOffset_Simple()
        {
            var scheduler = new TestScheduler();

            var xs = scheduler.CreateColdObservable(
                OnNext(50, 42),
                OnNext(60, 43),
                OnCompleted<int>(70)
            );

            var res = scheduler.Start(() =>
                xs.DelaySubscription(new DateTimeOffset(230, TimeSpan.Zero), scheduler)
            );

            res.Messages.AssertEqual(
                OnNext(280, 42),
                OnNext(290, 43),
                OnCompleted<int>(300)
            );

            xs.Subscriptions.AssertEqual(
                Subscribe(230, 300)
            );
        }

        [TestMethod]
        public void DelaySubscription_DateTimeOffset_Error()
        {
            var ex = new Exception();

            var scheduler = new TestScheduler();

            var xs = scheduler.CreateColdObservable(
                OnNext(50, 42),
                OnNext(60, 43),
                OnError<int>(70, ex)
            );

            var res = scheduler.Start(() =>
                xs.DelaySubscription(new DateTimeOffset(230, TimeSpan.Zero), scheduler)
            );

            res.Messages.AssertEqual(
                OnNext(280, 42),
                OnNext(290, 43),
                OnError<int>(300, ex)
            );

            xs.Subscriptions.AssertEqual(
                Subscribe(230, 300)
            );
        }

        #endregion

        #region + Generate +

        [TestMethod]
        public void Generate_TimeSpan_ArgumentChecking()
        {
            ReactiveAssert.Throws<ArgumentNullException>(() => Observable.Generate(0, DummyFunc<int, bool>.Instance, DummyFunc<int, int>.Instance, DummyFunc<int, int>.Instance, DummyFunc<int, TimeSpan>.Instance, (IScheduler)null));
            ReactiveAssert.Throws<ArgumentNullException>(() => Observable.Generate(0, (Func<int, bool>)null, DummyFunc<int, int>.Instance, DummyFunc<int, int>.Instance, DummyFunc<int, TimeSpan>.Instance, DummyScheduler.Instance));
            ReactiveAssert.Throws<ArgumentNullException>(() => Observable.Generate(0, DummyFunc<int, bool>.Instance, DummyFunc<int, int>.Instance, (Func<int, int>)null, DummyFunc<int, TimeSpan>.Instance, DummyScheduler.Instance));
            ReactiveAssert.Throws<ArgumentNullException>(() => Observable.Generate(0, DummyFunc<int, bool>.Instance, (Func<int, int>)null, DummyFunc<int, int>.Instance, DummyFunc<int, TimeSpan>.Instance, DummyScheduler.Instance));
            ReactiveAssert.Throws<ArgumentNullException>(() => Observable.Generate(0, DummyFunc<int, bool>.Instance, DummyFunc<int, int>.Instance, DummyFunc<int, int>.Instance, (Func<int, TimeSpan>)null, DummyScheduler.Instance));
            ReactiveAssert.Throws<ArgumentNullException>(() => Observable.Generate(0, DummyFunc<int, bool>.Instance, DummyFunc<int, int>.Instance, DummyFunc<int, int>.Instance, DummyFunc<int, TimeSpan>.Instance, DummyScheduler.Instance).Subscribe(null));
        }

        [TestMethod]
        public void Generate_TimeSpan_Finite()
        {
            var scheduler = new TestScheduler();

            var res = scheduler.Start(() =>
                Observable.Generate(0, x => x <= 3, x => x + 1, x => x, x => TimeSpan.FromTicks(x + 1), scheduler)
            );

            res.Messages.AssertEqual(
                OnNext(202, 0),
                OnNext(204, 1),
                OnNext(207, 2),
                OnNext(211, 3),
                OnCompleted<int>(211)
            );
        }

        [TestMethod]
        public void Generate_TimeSpan_Throw_Condition()
        {
            var scheduler = new TestScheduler();

            var ex = new Exception();

            var res = scheduler.Start(() =>
                Observable.Generate(0, new Func<int, bool>(x => { throw ex; }), x => x + 1, x => x, x => TimeSpan.FromTicks(x + 1), scheduler)
            );

            res.Messages.AssertEqual(
                OnError<int>(201, ex)
            );
        }

        [TestMethod]
        public void Generate_TimeSpan_Throw_ResultSelector()
        {
            var scheduler = new TestScheduler();

            var ex = new Exception();

            var res = scheduler.Start(() =>
                Observable.Generate(0, x => true, x => x + 1, new Func<int, int>(x => { throw ex; }), x => TimeSpan.FromTicks(x + 1), scheduler)
            );

            res.Messages.AssertEqual(
                OnError<int>(201, ex)
            );
        }

        [TestMethod]
        public void Generate_TimeSpan_Throw_Iterate()
        {
            var scheduler = new TestScheduler();

            var ex = new Exception();

            var res = scheduler.Start(() =>
                Observable.Generate(0, x => true, new Func<int, int>(x => { throw ex; }), x => x, x => TimeSpan.FromTicks(x + 1), scheduler)
            );

            res.Messages.AssertEqual(
                OnNext(202, 0),
                OnError<int>(202, ex)
            );
        }

        [TestMethod]
        public void Generate_TimeSpan_Throw_TimeSelector()
        {
            var scheduler = new TestScheduler();

            var ex = new Exception();

            var res = scheduler.Start(() =>
                Observable.Generate(0, x => true, x => x + 1, x => x, new Func<int, TimeSpan>(x => { throw ex; }), scheduler)
            );

            res.Messages.AssertEqual(
                OnError<int>(201, ex)
            );
        }

        [TestMethod]
        public void Generate_TimeSpan_Dispose()
        {
            var scheduler = new TestScheduler();

            var res = scheduler.Start(() =>
                Observable.Generate(0, x => true, x => x + 1, x => x, x => TimeSpan.FromTicks(x + 1), scheduler),
                210
            );

            res.Messages.AssertEqual(
                OnNext(202, 0),
                OnNext(204, 1),
                OnNext(207, 2)
            );
        }

        [TestMethod]
        public void Generate_TimeSpan_DefaultScheduler_ArgumentChecking()
        {
            ReactiveAssert.Throws<ArgumentNullException>(() => Observable.Generate(0, (Func<int, bool>)null, DummyFunc<int, int>.Instance, DummyFunc<int, int>.Instance, DummyFunc<int, TimeSpan>.Instance));
            ReactiveAssert.Throws<ArgumentNullException>(() => Observable.Generate(0, DummyFunc<int, bool>.Instance, DummyFunc<int, int>.Instance, (Func<int, int>)null, DummyFunc<int, TimeSpan>.Instance));
            ReactiveAssert.Throws<ArgumentNullException>(() => Observable.Generate(0, DummyFunc<int, bool>.Instance, (Func<int, int>)null, DummyFunc<int, int>.Instance, DummyFunc<int, TimeSpan>.Instance));
            ReactiveAssert.Throws<ArgumentNullException>(() => Observable.Generate(0, DummyFunc<int, bool>.Instance, DummyFunc<int, int>.Instance, DummyFunc<int, int>.Instance, (Func<int, TimeSpan>)null));
            ReactiveAssert.Throws<ArgumentNullException>(() => Observable.Generate(0, DummyFunc<int, bool>.Instance, DummyFunc<int, int>.Instance, DummyFunc<int, int>.Instance, DummyFunc<int, TimeSpan>.Instance).Subscribe(null));
        }

        [TestMethod]
        public void Generate_TimeSpan_DefaultScheduler()
        {
            Observable.Generate(0, x => x < 10, x => x + 1, x => x, x => TimeSpan.FromMilliseconds(x)).AssertEqual(Observable.Generate(0, x => x < 10, x => x + 1, x => x, x => TimeSpan.FromMilliseconds(x), DefaultScheduler.Instance));
        }

        [TestMethod]
        public void Generate_DateTimeOffset_ArgumentChecking()
        {
            ReactiveAssert.Throws<ArgumentNullException>(() => Observable.Generate(0, DummyFunc<int, bool>.Instance, DummyFunc<int, int>.Instance, DummyFunc<int, int>.Instance, DummyFunc<int, DateTimeOffset>.Instance, (IScheduler)null));
            ReactiveAssert.Throws<ArgumentNullException>(() => Observable.Generate(0, (Func<int, bool>)null, DummyFunc<int, int>.Instance, DummyFunc<int, int>.Instance, DummyFunc<int, DateTimeOffset>.Instance, DummyScheduler.Instance));
            ReactiveAssert.Throws<ArgumentNullException>(() => Observable.Generate(0, DummyFunc<int, bool>.Instance, DummyFunc<int, int>.Instance, (Func<int, int>)null, DummyFunc<int, DateTimeOffset>.Instance, DummyScheduler.Instance));
            ReactiveAssert.Throws<ArgumentNullException>(() => Observable.Generate(0, DummyFunc<int, bool>.Instance, (Func<int, int>)null, DummyFunc<int, int>.Instance, DummyFunc<int, DateTimeOffset>.Instance, DummyScheduler.Instance));
            ReactiveAssert.Throws<ArgumentNullException>(() => Observable.Generate(0, DummyFunc<int, bool>.Instance, DummyFunc<int, int>.Instance, DummyFunc<int, int>.Instance, (Func<int, DateTimeOffset>)null, DummyScheduler.Instance));
            ReactiveAssert.Throws<ArgumentNullException>(() => Observable.Generate(0, DummyFunc<int, bool>.Instance, DummyFunc<int, int>.Instance, DummyFunc<int, int>.Instance, DummyFunc<int, DateTimeOffset>.Instance, DummyScheduler.Instance).Subscribe(null));
        }

        [TestMethod]
        public void Generate_DateTimeOffset_Finite()
        {
            var scheduler = new TestScheduler();

            var res = scheduler.Start(() =>
                Observable.Generate(0, x => x <= 3, x => x + 1, x => x, x => scheduler.Now.AddTicks(x + 1), scheduler)
            );

            res.Messages.AssertEqual(
                OnNext(202, 0),
                OnNext(204, 1),
                OnNext(207, 2),
                OnNext(211, 3),
                OnCompleted<int>(211)
            );
        }

        [TestMethod]
        public void Generate_DateTimeOffset_Throw_Condition()
        {
            var scheduler = new TestScheduler();

            var ex = new Exception();

            var res = scheduler.Start(() =>
                Observable.Generate(0, new Func<int, bool>(x => { throw ex; }), x => x + 1, x => x, x => scheduler.Now.AddTicks(x + 1), scheduler)
            );

            res.Messages.AssertEqual(
                OnError<int>(201, ex)
            );
        }

        [TestMethod]
        public void Generate_DateTimeOffset_Throw_ResultSelector()
        {
            var scheduler = new TestScheduler();

            var ex = new Exception();

            var res = scheduler.Start(() =>
                Observable.Generate(0, x => true, x => x + 1, new Func<int, int>(x => { throw ex; }), x => scheduler.Now.AddTicks(x + 1), scheduler)
            );

            res.Messages.AssertEqual(
                OnError<int>(201, ex)
            );
        }

        [TestMethod]
        public void Generate_DateTimeOffset_Throw_Iterate()
        {
            var scheduler = new TestScheduler();

            var ex = new Exception();

            var res = scheduler.Start(() =>
                Observable.Generate(0, x => true, new Func<int, int>(x => { throw ex; }), x => x, x => scheduler.Now.AddTicks(x + 1), scheduler)
            );

            res.Messages.AssertEqual(
                OnNext(202, 0),
                OnError<int>(202, ex)
            );
        }

        [TestMethod]
        public void Generate_DateTimeOffset_Throw_TimeSelector()
        {
            var scheduler = new TestScheduler();

            var ex = new Exception();

            var res = scheduler.Start(() =>
                Observable.Generate(0, x => true, x => x + 1, x => x, new Func<int, DateTimeOffset>(x => { throw ex; }), scheduler)
            );

            res.Messages.AssertEqual(
                OnError<int>(201, ex)
            );
        }

        [TestMethod]
        public void Generate_DateTimeOffset_Dispose()
        {
            var scheduler = new TestScheduler();

            var res = scheduler.Start(() =>
                Observable.Generate(0, x => true, x => x + 1, x => x, x => scheduler.Now.AddTicks(x + 1), scheduler),
                210
            );

            res.Messages.AssertEqual(
                OnNext(202, 0),
                OnNext(204, 1),
                OnNext(207, 2)
            );
        }

        [TestMethod]
        public void Generate_DateTimeOffset_DefaultScheduler_ArgumentChecking()
        {
            ReactiveAssert.Throws<ArgumentNullException>(() => Observable.Generate(0, (Func<int, bool>)null, DummyFunc<int, int>.Instance, DummyFunc<int, int>.Instance, DummyFunc<int, DateTimeOffset>.Instance));
            ReactiveAssert.Throws<ArgumentNullException>(() => Observable.Generate(0, DummyFunc<int, bool>.Instance, DummyFunc<int, int>.Instance, (Func<int, int>)null, DummyFunc<int, DateTimeOffset>.Instance));
            ReactiveAssert.Throws<ArgumentNullException>(() => Observable.Generate(0, DummyFunc<int, bool>.Instance, (Func<int, int>)null, DummyFunc<int, int>.Instance, DummyFunc<int, DateTimeOffset>.Instance));
            ReactiveAssert.Throws<ArgumentNullException>(() => Observable.Generate(0, DummyFunc<int, bool>.Instance, DummyFunc<int, int>.Instance, DummyFunc<int, int>.Instance, (Func<int, DateTimeOffset>)null));
            ReactiveAssert.Throws<ArgumentNullException>(() => Observable.Generate(0, DummyFunc<int, bool>.Instance, DummyFunc<int, int>.Instance, DummyFunc<int, int>.Instance, DummyFunc<int, DateTimeOffset>.Instance).Subscribe(null));
        }

        [TestMethod]
        public void Generate_DateTimeOffset_DefaultScheduler()
        {
            Observable.Generate(0, x => x < 10, x => x + 1, x => x, x => DateTimeOffset.Now.AddMilliseconds(x)).AssertEqual(Observable.Generate(0, x => x < 10, x => x + 1, x => x, x => DateTimeOffset.Now.AddMilliseconds(x), DefaultScheduler.Instance));
        }

        #endregion

        #region + Interval +

        [TestMethod]
        public void Interval_TimeSpan_ArgumentChecking()
        {
            ReactiveAssert.Throws<ArgumentNullException>(() => Observable.Interval(TimeSpan.Zero, null));
            ReactiveAssert.Throws<ArgumentNullException>(() => Observable.Interval(TimeSpan.Zero, DummyScheduler.Instance).Subscribe(null));
            ReactiveAssert.Throws<ArgumentOutOfRangeException>(() => Observable.Interval(TimeSpan.FromSeconds(-1)));
            ReactiveAssert.Throws<ArgumentOutOfRangeException>(() => Observable.Interval(TimeSpan.FromSeconds(-1), DummyScheduler.Instance));
        }

        [TestMethod]
        public void Interval_TimeSpan_Basic()
        {
            var scheduler = new TestScheduler();

            var res = scheduler.Start(() =>
                Observable.Interval(TimeSpan.FromTicks(100), scheduler)
            );

            res.Messages.AssertEqual(
                OnNext(300, 0L),
                OnNext(400, 1L),
                OnNext(500, 2L),
                OnNext(600, 3L),
                OnNext(700, 4L),
                OnNext(800, 5L),
                OnNext(900, 6L)
            );
        }

        [TestMethod]
        public void Interval_TimeSpan_Zero()
        {
            var scheduler = new TestScheduler();

            var res = scheduler.Start(() =>
                Observable.Interval(TimeSpan.FromTicks(0), scheduler),
                210
            );

            res.Messages.AssertEqual(
                OnNext(201, 0L),
                OnNext(202, 1L),
                OnNext(203, 2L),
                OnNext(204, 3L),
                OnNext(205, 4L),
                OnNext(206, 5L),
                OnNext(207, 6L),
                OnNext(208, 7L),
                OnNext(209, 8L)
            );
        }

        [TestMethod]
        public void Interval_TimeSpan_Zero_DefaultScheduler()
        {
            var scheduler = new TestScheduler();
            var observer = scheduler.CreateObserver<long>();
            var completed = new ManualResetEventSlim();

            Observable.Interval(TimeSpan.Zero).TakeWhile(i => i < 10).Subscribe(observer.OnNext, completed.Set);

            completed.Wait();
            
            Assert.AreEqual(10, observer.Messages.Count);
        }

        [TestMethod]
        public void Interval_TimeSpan_Disposed()
        {
            var scheduler = new TestScheduler();

            var res = scheduler.Start(
                () => Observable.Interval(TimeSpan.FromTicks(1000), scheduler)
            );

            res.Messages.AssertEqual(
            );
        }

        [TestMethod]
        public void Interval_TimeSpan_ObserverThrows()
        {
            var scheduler = new TestScheduler();

            var xs = Observable.Interval(TimeSpan.FromTicks(1), scheduler);

            xs.Subscribe(x => { throw new InvalidOperationException(); });

            ReactiveAssert.Throws<InvalidOperationException>(() => scheduler.Start());
        }

        [TestMethod]
        public void Interval_TimeSpan_DefaultScheduler()
        {
            Assert.IsTrue(Observable.Interval(TimeSpan.FromMilliseconds(1)).ToEnumerable().Take(3).SequenceEqual(new[] { 0L, 1L, 2L }));
        }

        #endregion

        #region + Sample +

        [TestMethod]
        public void Sample_ArgumentChecking()
        {
            var scheduler = new TestScheduler();
            var someObservable = Observable.Empty<int>();

            ReactiveAssert.Throws<ArgumentNullException>(() => Observable.Sample(default(IObservable<int>), TimeSpan.Zero));
            ReactiveAssert.Throws<ArgumentNullException>(() => Observable.Sample(default(IObservable<int>), TimeSpan.Zero, scheduler));
            ReactiveAssert.Throws<ArgumentNullException>(() => Observable.Sample(someObservable, TimeSpan.Zero, null));

            ReactiveAssert.Throws<ArgumentOutOfRangeException>(() => Observable.Sample(someObservable, TimeSpan.FromSeconds(-1)));
            ReactiveAssert.Throws<ArgumentOutOfRangeException>(() => Observable.Sample(someObservable, TimeSpan.FromSeconds(-1), scheduler));

            ReactiveAssert.Throws<ArgumentNullException>(() => Observable.Sample(default(IObservable<int>), someObservable));
            ReactiveAssert.Throws<ArgumentNullException>(() => Observable.Sample(someObservable, default(IObservable<int>)));
        }

        [TestMethod]
        public void Sample_Regular()
        {
            var scheduler = new TestScheduler();

            var xs = scheduler.CreateHotObservable(
                OnNext(150, 1),
                OnNext(210, 2),
                OnNext(230, 3),
                OnNext(260, 4),
                OnNext(300, 5),
                OnNext(350, 6),
                OnNext(380, 7),
                OnCompleted<int>(390)
            );

            var res = scheduler.Start(() =>
                xs.Sample(TimeSpan.FromTicks(50), scheduler)
            );

            res.Messages.AssertEqual(
                OnNext(250, 3),
                OnNext(300, 5), /* CHECK: boundary of sampling */
                OnNext(350, 6),
                OnNext(400, 7), /* Sample in last bucket */
                OnCompleted<int>(400)
            );

            xs.Subscriptions.AssertEqual(
                Subscribe(200, 390)
            );
        }

        [TestMethod]
        public void Sample_Periodic_Regular()
        {
            var scheduler = new PeriodicTestScheduler();

            var xs = scheduler.CreateHotObservable(
                OnNext(150, 1),
                OnNext(210, 2),
                OnNext(230, 3),
                OnNext(260, 4),
                OnNext(300, 5),
                OnNext(350, 6),
                OnNext(380, 7),
                OnCompleted<int>(390)
            );

            var res = scheduler.Start(() =>
                xs.Sample(TimeSpan.FromTicks(50), scheduler)
            );

            res.Messages.AssertEqual(
                OnNext(250, 3),
                OnNext(300, 5), /* CHECK: boundary of sampling */
                OnNext(350, 6),
                OnNext(400, 7), /* Sample in last bucket */
                OnCompleted<int>(400)
            );

            xs.Subscriptions.AssertEqual(
                Subscribe(200, 390)
            );

            scheduler.Timers.AssertEqual(
                new TimerRun(200, 400) { 250, 300, 350, 400 }
            );
        }

        [TestMethod]
        public void Sample_ErrorInFlight()
        {
            var scheduler = new TestScheduler();

            var ex = new Exception();

            var xs = scheduler.CreateHotObservable(
                OnNext(150, 1),
                OnNext(210, 2),
                OnNext(230, 3),
                OnNext(260, 4),
                OnNext(300, 5),
                OnNext(310, 6),
                OnError<int>(330, ex)
            );

            var res = scheduler.Start(() =>
                xs.Sample(TimeSpan.FromTicks(50), scheduler)
            );

            res.Messages.AssertEqual(
                OnNext(250, 3),
                OnNext(300, 5), /* CHECK: boundary of sampling */
                OnError<int>(330, ex)
            );

            xs.Subscriptions.AssertEqual(
                Subscribe(200, 330)
            );
        }

        [TestMethod]
        public void Sample_Periodic_ErrorInFlight()
        {
            var scheduler = new PeriodicTestScheduler();

            var ex = new Exception();

            var xs = scheduler.CreateHotObservable(
                OnNext(150, 1),
                OnNext(210, 2),
                OnNext(230, 3),
                OnNext(260, 4),
                OnNext(300, 5),
                OnNext(310, 6),
                OnError<int>(330, ex)
            );

            var res = scheduler.Start(() =>
                xs.Sample(TimeSpan.FromTicks(50), scheduler)
            );

            res.Messages.AssertEqual(
                OnNext(250, 3),
                OnNext(300, 5), /* CHECK: boundary of sampling */
                OnError<int>(330, ex)
            );

            xs.Subscriptions.AssertEqual(
                Subscribe(200, 330)
            );

            scheduler.Timers.AssertEqual(
                new TimerRun(200, 330) { 250, 300 }
            );
        }

        [TestMethod]
        public void Sample_Empty()
        {
            var scheduler = new TestScheduler();

            var xs = scheduler.CreateHotObservable(
                OnNext(150, 1),
                OnCompleted<int>(300)
            );

            var res = scheduler.Start(() =>
                xs.Sample(TimeSpan.FromTicks(50), scheduler)
            );

            res.Messages.AssertEqual(
                OnCompleted<int>(300)
            );

            xs.Subscriptions.AssertEqual(
                Subscribe(200, 300)
            );
        }

        [TestMethod]
        public void Sample_Periodic_Empty()
        {
            var scheduler = new PeriodicTestScheduler();

            var xs = scheduler.CreateHotObservable(
                OnNext(150, 1),
                OnCompleted<int>(300)
            );

            var res = scheduler.Start(() =>
                xs.Sample(TimeSpan.FromTicks(50), scheduler)
            );

            res.Messages.AssertEqual(
                OnCompleted<int>(300)
            );

            xs.Subscriptions.AssertEqual(
                Subscribe(200, 300)
            );

            scheduler.Timers.AssertEqual(
                new TimerRun(200, 300) { 250, 300 }
            );
        }

        [TestMethod]
        public void Sample_Error()
        {
            var scheduler = new TestScheduler();

            var ex = new Exception();

            var xs = scheduler.CreateHotObservable(
                OnNext(150, 1),
                OnError<int>(300, ex)
            );

            var res = scheduler.Start(() =>
                xs.Sample(TimeSpan.FromTicks(50), scheduler)
            );

            res.Messages.AssertEqual(
                OnError<int>(300, ex)
            );

            xs.Subscriptions.AssertEqual(
                Subscribe(200, 300)
            );
        }

        [TestMethod]
        public void Sample_Periodic_Error()
        {
            var scheduler = new PeriodicTestScheduler();

            var ex = new Exception();

            var xs = scheduler.CreateHotObservable(
                OnNext(150, 1),
                OnError<int>(300, ex)
            );

            var res = scheduler.Start(() =>
                xs.Sample(TimeSpan.FromTicks(50), scheduler)
            );

            res.Messages.AssertEqual(
                OnError<int>(300, ex)
            );

            xs.Subscriptions.AssertEqual(
                Subscribe(200, 300)
            );

            scheduler.Timers.AssertEqual(
                new TimerRun(200, 300) { 250 }
            );
        }

        [TestMethod]
        public void Sample_Never()
        {
            var scheduler = new TestScheduler();

            var xs = scheduler.CreateHotObservable(
                OnNext(150, 1)
            );

            var res = scheduler.Start(() =>
                xs.Sample(TimeSpan.FromTicks(50), scheduler)
            );

            res.Messages.AssertEqual(
            );

            xs.Subscriptions.AssertEqual(
                Subscribe(200, 1000)
            );
        }

        [TestMethod]
        public void Sample_Periodic_Never()
        {
            var scheduler = new PeriodicTestScheduler();

            var xs = scheduler.CreateHotObservable(
                OnNext(150, 1)
            );

            var res = scheduler.Start(() =>
                xs.Sample(TimeSpan.FromTicks(50), scheduler)
            );

            res.Messages.AssertEqual(
            );

            xs.Subscriptions.AssertEqual(
                Subscribe(200, 1000)
            );

            scheduler.Timers.AssertEqual(
                new TimerRun(200, 1000) { 250, 300, 350, 400, 450, 500, 550, 600, 650, 700, 750, 800, 850, 900, 950 }
            );
        }

        [TestMethod]
        public void Sample_DefaultScheduler_Periodic()
        {
            var res = Observable.Return(42).Sample(TimeSpan.FromMilliseconds(1)).ToEnumerable().Single();
            Assert.AreEqual(42, res);
        }

        [TestMethod]
        public void Sample_DefaultScheduler_PeriodicDisabled()
        {
            var res = Observable.Return(42).Sample(TimeSpan.FromMilliseconds(1), Scheduler.Default.DisableOptimizations()).ToEnumerable().Single();
            Assert.AreEqual(42, res);
        }

        [TestMethod]
        public void Sample_Sampler_Simple1()
        {
            var scheduler = new TestScheduler();

            var xs = scheduler.CreateHotObservable(
                OnNext(150, 1),
                OnNext(220, 2),
                OnNext(240, 3),
                OnNext(290, 4),
                OnNext(300, 5),
                OnNext(310, 6),
                OnCompleted<int>(400)
            );

            var ys = scheduler.CreateHotObservable(
                OnNext(150, ""),
                OnNext(210, "bar"),
                OnNext(250, "foo"),
                OnNext(260, "qux"),
                OnNext(320, "baz"),
                OnCompleted<string>(500)
            );

            var res = scheduler.Start(() =>
                xs.Sample(ys)
            );

            res.Messages.AssertEqual(
                OnNext(250, 3),
                OnNext(320, 6),
                OnCompleted<int>(500 /* on sampling boundaries only */)
            );

            xs.Subscriptions.AssertEqual(
                Subscribe(200, 400)
            );

            ys.Subscriptions.AssertEqual(
                Subscribe(200, 500)
            );
        }

        [TestMethod]
        public void Sample_Sampler_Simple2()
        {
            var scheduler = new TestScheduler();

            var xs = scheduler.CreateHotObservable(
                OnNext(150, 1),
                OnNext(220, 2),
                OnNext(240, 3),
                OnNext(290, 4),
                OnNext(300, 5),
                OnNext(310, 6),
                OnNext(360, 7),
                OnCompleted<int>(400)
            );

            var ys = scheduler.CreateHotObservable(
                OnNext(150, ""),
                OnNext(210, "bar"),
                OnNext(250, "foo"),
                OnNext(260, "qux"),
                OnNext(320, "baz"),
                OnCompleted<string>(500)
            );

            var res = scheduler.Start(() =>
                xs.Sample(ys)
            );

            res.Messages.AssertEqual(
                OnNext(250, 3),
                OnNext(320, 6),
                OnNext(500, 7),
                OnCompleted<int>(500 /* on sampling boundaries only */)
            );

            xs.Subscriptions.AssertEqual(
                Subscribe(200, 400)
            );

            ys.Subscriptions.AssertEqual(
                Subscribe(200, 500)
            );
        }

        [TestMethod]
        public void Sample_Sampler_Simple3()
        {
            var scheduler = new TestScheduler();

            var xs = scheduler.CreateHotObservable(
                OnNext(150, 1),
                OnNext(220, 2),
                OnNext(240, 3),
                OnNext(290, 4),
                OnCompleted<int>(300)
            );

            var ys = scheduler.CreateHotObservable(
                OnNext(150, ""),
                OnNext(210, "bar"),
                OnNext(250, "foo"),
                OnNext(260, "qux"),
                OnNext(320, "baz"),
                OnCompleted<string>(500)
            );

            var res = scheduler.Start(() =>
                xs.Sample(ys)
            );

            res.Messages.AssertEqual(
                OnNext(250, 3),
                OnNext(320, 4),
                OnCompleted<int>(320 /* on sampling boundaries only */)
            );

            xs.Subscriptions.AssertEqual(
                Subscribe(200, 300)
            );

            ys.Subscriptions.AssertEqual(
                Subscribe(200, 320)
            );
        }

        [TestMethod]
        public void Sample_Sampler_SourceThrows()
        {
            var ex = new Exception();

            var scheduler = new TestScheduler();

            var xs = scheduler.CreateHotObservable(
                OnNext(150, 1),
                OnNext(220, 2),
                OnNext(240, 3),
                OnNext(290, 4),
                OnNext(300, 5),
                OnNext(310, 6),
                OnError<int>(320, ex)
            );

            var ys = scheduler.CreateHotObservable(
                OnNext(150, ""),
                OnNext(210, "bar"),
                OnNext(250, "foo"),
                OnNext(260, "qux"),
                OnNext(330, "baz"),
                OnCompleted<string>(400)
            );

            var res = scheduler.Start(() =>
                xs.Sample(ys)
            );

            res.Messages.AssertEqual(
                OnNext(250, 3),
                OnError<int>(320, ex)
            );

            xs.Subscriptions.AssertEqual(
                Subscribe(200, 320)
            );

            ys.Subscriptions.AssertEqual(
                Subscribe(200, 320)
            );
        }

#if !NO_PERF // BREAKING CHANGE v2 > v1.x - behavior when sampler throws
        [TestMethod]
        public void Sample_Sampler_SamplerThrows()
        {
            var ex = new Exception();

            var scheduler = new TestScheduler();

            var xs = scheduler.CreateHotObservable(
                OnNext(150, 1),
                OnNext(220, 2),
                OnNext(240, 3),
                OnNext(290, 4),
                OnNext(300, 5),
                OnNext(310, 6),
                OnCompleted<int>(400)
            );

            var ys = scheduler.CreateHotObservable(
                OnNext(150, ""),
                OnNext(210, "bar"),
                OnNext(250, "foo"),
                OnNext(260, "qux"),
                OnError<string>(320, ex)
            );

            var res = scheduler.Start(() =>
                xs.Sample(ys)
            );

            res.Messages.AssertEqual(
                OnNext(250, 3),
                OnError<int>(320, ex)
            );

            xs.Subscriptions.AssertEqual(
                Subscribe(200, 320)
            );

            ys.Subscriptions.AssertEqual(
                Subscribe(200, 320)
            );
        }
#endif

        #endregion

        #region + Skip +

        [TestMethod]
        public void Skip_ArgumentChecking()
        {
            var xs = Observable.Return(42);

            ReactiveAssert.Throws<ArgumentNullException>(() => Observable.Skip(default(IObservable<int>), TimeSpan.FromSeconds(1)));
            ReactiveAssert.Throws<ArgumentOutOfRangeException>(() => Observable.Skip(xs, TimeSpan.FromSeconds(-1)));

            ReactiveAssert.Throws<ArgumentNullException>(() => Observable.Skip(default(IObservable<int>), TimeSpan.FromSeconds(1), Scheduler.Default));
            ReactiveAssert.Throws<ArgumentNullException>(() => Observable.Skip(xs, TimeSpan.FromSeconds(1), default(IScheduler)));
            ReactiveAssert.Throws<ArgumentOutOfRangeException>(() => Observable.Skip(xs, TimeSpan.FromSeconds(-1), Scheduler.Default));
        }

        [TestMethod]
        public void Skip_Zero()
        {
            var scheduler = new TestScheduler();

            var xs = scheduler.CreateHotObservable<int>(
                OnNext(210, 1),
                OnNext(220, 2),
                OnCompleted<int>(230)
            );

            var res = scheduler.Start(() =>
                xs.Skip(TimeSpan.Zero, scheduler)
            );

            res.Messages.AssertEqual(
                OnNext(210, 1),
                OnNext(220, 2),
                OnCompleted<int>(230)
            );

            xs.Subscriptions.AssertEqual(
                Subscribe(200, 230)
            );
        }

        [TestMethod]
        public void Skip_Some()
        {
            var scheduler = new TestScheduler();

            var xs = scheduler.CreateHotObservable<int>(
                OnNext(210, 1),
                OnNext(220, 2),
                OnCompleted<int>(230)
            );

            var res = scheduler.Start(() =>
                xs.Skip(TimeSpan.FromTicks(15), scheduler)
            );

            res.Messages.AssertEqual(
                OnNext(220, 2),
                OnCompleted<int>(230)
            );

            xs.Subscriptions.AssertEqual(
                Subscribe(200, 230)
            );
        }

        [TestMethod]
        public void Skip_Late()
        {
            var scheduler = new TestScheduler();

            var xs = scheduler.CreateHotObservable<int>(
                OnNext(210, 1),
                OnNext(220, 2),
                OnCompleted<int>(230)
            );

            var res = scheduler.Start(() =>
                xs.Skip(TimeSpan.FromTicks(50), scheduler)
            );

            res.Messages.AssertEqual(
                OnCompleted<int>(230)
            );

            xs.Subscriptions.AssertEqual(
                Subscribe(200, 230)
            );
        }

        [TestMethod]
        public void Skip_Error()
        {
            var scheduler = new TestScheduler();

            var ex = new Exception();

            var xs = scheduler.CreateHotObservable<int>(
                OnError<int>(210, ex)
            );

            var res = scheduler.Start(() =>
                xs.Skip(TimeSpan.FromTicks(50), scheduler)
            );

            res.Messages.AssertEqual(
                OnError<int>(210, ex)
            );

            xs.Subscriptions.AssertEqual(
                Subscribe(200, 210)
            );
        }

        [TestMethod]
        public void Skip_Never()
        {
            var scheduler = new TestScheduler();

            var ex = new Exception();

            var xs = scheduler.CreateHotObservable<int>(
            );

            var res = scheduler.Start(() =>
                xs.Skip(TimeSpan.FromTicks(50), scheduler)
            );

            res.Messages.AssertEqual(
            );

            xs.Subscriptions.AssertEqual(
                Subscribe(200, 1000)
            );
        }

        [TestMethod]
        public void Skip_Twice1()
        {
            var scheduler = new TestScheduler();

            var ex = new Exception();

            var xs = scheduler.CreateHotObservable<int>(
                OnNext(210, 1),
                OnNext(220, 2),
                OnNext(230, 3),
                OnNext(240, 4),
                OnNext(250, 5),
                OnNext(260, 6),
                OnCompleted<int>(270)
            );

            var res = scheduler.Start(() =>
                xs.Skip(TimeSpan.FromTicks(15), scheduler).Skip(TimeSpan.FromTicks(30), scheduler)
            );

            res.Messages.AssertEqual(
                OnNext(240, 4),
                OnNext(250, 5),
                OnNext(260, 6),
                OnCompleted<int>(270)
            );

            xs.Subscriptions.AssertEqual(
                Subscribe(200, 270)
            );
        }

        [TestMethod]
        public void Skip_Twice2()
        {
            var scheduler = new TestScheduler();

            var ex = new Exception();

            var xs = scheduler.CreateHotObservable<int>(
                OnNext(210, 1),
                OnNext(220, 2),
                OnNext(230, 3),
                OnNext(240, 4),
                OnNext(250, 5),
                OnNext(260, 6),
                OnCompleted<int>(270)
            );

            var res = scheduler.Start(() =>
                xs.Skip(TimeSpan.FromTicks(30), scheduler).Skip(TimeSpan.FromTicks(15), scheduler)
            );

            res.Messages.AssertEqual(
                OnNext(240, 4),
                OnNext(250, 5),
                OnNext(260, 6),
                OnCompleted<int>(270)
            );

            xs.Subscriptions.AssertEqual(
                Subscribe(200, 270)
            );
        }

        [TestMethod]
        public void Skip_Default()
        {
            var xs = Observable.Range(0, 10, Scheduler.Default);

            var res = xs.Skip(TimeSpan.FromSeconds(60));

            var e = new ManualResetEvent(false);

            var lst = new List<int>();
            res.Subscribe(
                lst.Add,
                () => e.Set()
            );

            e.WaitOne();

            Assert.IsTrue(lst.Count == 0);
        }

        #endregion

        #region + SkipLast +

        [TestMethod]
        public void SkipLast_ArgumentChecking()
        {
            var xs = Observable.Return(42);

            ReactiveAssert.Throws<ArgumentNullException>(() => Observable.SkipLast(default(IObservable<int>), TimeSpan.FromSeconds(1)));
            ReactiveAssert.Throws<ArgumentOutOfRangeException>(() => Observable.SkipLast(xs, TimeSpan.FromSeconds(-1)));

            ReactiveAssert.Throws<ArgumentNullException>(() => Observable.SkipLast(default(IObservable<int>), TimeSpan.FromSeconds(1), Scheduler.Default));
            ReactiveAssert.Throws<ArgumentNullException>(() => Observable.SkipLast(xs, TimeSpan.FromSeconds(1), default(IScheduler)));
            ReactiveAssert.Throws<ArgumentOutOfRangeException>(() => Observable.SkipLast(xs, TimeSpan.FromSeconds(-1), Scheduler.Default));
        }

        [TestMethod]
        public void SkipLast_Zero1()
        {
            var scheduler = new TestScheduler();

            var xs = scheduler.CreateHotObservable<int>(
                OnNext(210, 1),
                OnNext(220, 2),
                OnCompleted<int>(230)
            );

            var res = scheduler.Start(() =>
                xs.SkipLast(TimeSpan.Zero, scheduler)
            );

            res.Messages.AssertEqual(
                OnNext(210, 1),
                OnNext(220, 2),
                OnCompleted<int>(230)
            );

            xs.Subscriptions.AssertEqual(
                Subscribe(200, 230)
            );
        }

        [TestMethod]
        public void SkipLast_Zero2()
        {
            var scheduler = new TestScheduler();

            var xs = scheduler.CreateHotObservable<int>(
                OnNext(210, 1),
                OnNext(220, 2),
                OnNext(230, 3),
                OnCompleted<int>(230)
            );

            var res = scheduler.Start(() =>
                xs.SkipLast(TimeSpan.Zero, scheduler)
            );

            res.Messages.AssertEqual(
                OnNext(210, 1),
                OnNext(220, 2),
                OnNext(230, 3),
                OnCompleted<int>(230)
            );

            xs.Subscriptions.AssertEqual(
                Subscribe(200, 230)
            );
        }

        [TestMethod]
        public void SkipLast_Some1()
        {
            var scheduler = new TestScheduler();

            var xs = scheduler.CreateHotObservable<int>(
                OnNext(210, 1),
                OnNext(220, 2),
                OnCompleted<int>(230)
            );

            var res = scheduler.Start(() =>
                xs.SkipLast(TimeSpan.FromTicks(15), scheduler)
            );

            res.Messages.AssertEqual(
                OnNext(230, 1),
                OnCompleted<int>(230)
            );

            xs.Subscriptions.AssertEqual(
                Subscribe(200, 230)
            );
        }

        [TestMethod]
        public void SkipLast_Some2()
        {
            var scheduler = new TestScheduler();

            var xs = scheduler.CreateHotObservable<int>(
                OnNext(210, 1),
                OnNext(220, 2),
                OnNext(230, 3),
                OnNext(240, 4),
                OnNext(250, 5),
                OnNext(260, 6),
                OnNext(270, 7),
                OnNext(280, 8),
                OnNext(290, 9),
                OnCompleted<int>(300)
            );

            var res = scheduler.Start(() =>
                xs.SkipLast(TimeSpan.FromTicks(45), scheduler)
            );

            res.Messages.AssertEqual(
                OnNext(260, 1),
                OnNext(270, 2),
                OnNext(280, 3),
                OnNext(290, 4),
                OnNext(300, 5),
                OnCompleted<int>(300)
            );

            xs.Subscriptions.AssertEqual(
                Subscribe(200, 300)
            );
        }

        [TestMethod]
        public void SkipLast_All()
        {
            var scheduler = new TestScheduler();

            var xs = scheduler.CreateHotObservable<int>(
                OnNext(210, 1),
                OnNext(220, 2),
                OnCompleted<int>(230)
            );

            var res = scheduler.Start(() =>
                xs.SkipLast(TimeSpan.FromTicks(50), scheduler)
            );

            res.Messages.AssertEqual(
                OnCompleted<int>(230)
            );

            xs.Subscriptions.AssertEqual(
                Subscribe(200, 230)
            );
        }

        [TestMethod]
        public void SkipLast_Error()
        {
            var scheduler = new TestScheduler();

            var ex = new Exception();

            var xs = scheduler.CreateHotObservable<int>(
                OnError<int>(210, ex)
            );

            var res = scheduler.Start(() =>
                xs.SkipLast(TimeSpan.FromTicks(50), scheduler)
            );

            res.Messages.AssertEqual(
                OnError<int>(210, ex)
            );

            xs.Subscriptions.AssertEqual(
                Subscribe(200, 210)
            );
        }

        [TestMethod]
        public void SkipLast_Never()
        {
            var scheduler = new TestScheduler();

            var ex = new Exception();

            var xs = scheduler.CreateHotObservable<int>(
            );

            var res = scheduler.Start(() =>
                xs.SkipLast(TimeSpan.FromTicks(50), scheduler)
            );

            res.Messages.AssertEqual(
            );

            xs.Subscriptions.AssertEqual(
                Subscribe(200, 1000)
            );
        }

        [TestMethod]
        public void SkipLast_Default1()
        {
            var xs = Observable.Range(0, 10, Scheduler.Default);

            var res = xs.SkipLast(TimeSpan.FromSeconds(60));

            var e = new ManualResetEvent(false);

            var lst = new List<int>();
            res.Subscribe(
                lst.Add,
                () => e.Set()
            );

            e.WaitOne();

            Assert.IsTrue(lst.Count == 0);
        }

        [TestMethod]
        public void SkipLast_Default2()
        {
            var xs = Observable.Range(0, 10, Scheduler.Default);

            var res = xs.SkipLast(TimeSpan.FromSeconds(60), Scheduler.Default.DisableOptimizations());

            var e = new ManualResetEvent(false);

            var lst = new List<int>();
            res.Subscribe(
                lst.Add,
                () => e.Set()
            );

            e.WaitOne();

            Assert.IsTrue(lst.Count == 0);
        }

        [TestMethod]
        public void SkipLast_Default3()
        {
            var xs = Observable.Range(0, 10, Scheduler.Default);

            var res = xs.SkipLast(TimeSpan.Zero);

            var e = new ManualResetEvent(false);

            var lst = new List<int>();
            res.Subscribe(
                lst.Add,
                () => e.Set()
            );

            e.WaitOne();

            Assert.IsTrue(lst.SequenceEqual(Enumerable.Range(0, 10)));
        }

        [TestMethod]
        public void SkipLast_Default4()
        {
            var xs = Observable.Range(0, 10, Scheduler.Default);

            var res = xs.SkipLast(TimeSpan.Zero, Scheduler.Default.DisableOptimizations());

            var e = new ManualResetEvent(false);

            var lst = new List<int>();
            res.Subscribe(
                lst.Add,
                () => e.Set()
            );

            e.WaitOne();

            Assert.IsTrue(lst.SequenceEqual(Enumerable.Range(0, 10)));
        }

        #endregion

        #region + SkipUntil +

        [TestMethod]
        public void SkipUntil_ArgumentChecking()
        {
            var xs = Observable.Return(42);

            ReactiveAssert.Throws<ArgumentNullException>(() => Observable.SkipUntil(default(IObservable<int>), DateTimeOffset.Now));

            ReactiveAssert.Throws<ArgumentNullException>(() => Observable.SkipUntil(default(IObservable<int>), DateTimeOffset.Now, Scheduler.Default));
            ReactiveAssert.Throws<ArgumentNullException>(() => Observable.SkipUntil(xs, DateTimeOffset.Now, default(IScheduler)));
        }

        [TestMethod]
        public void SkipUntil_Zero()
        {
            var scheduler = new TestScheduler();

            var xs = scheduler.CreateHotObservable<int>(
                OnNext(210, 1),
                OnNext(220, 2),
                OnCompleted<int>(230)
            );

            var res = scheduler.Start(() =>
                xs.SkipUntil(new DateTimeOffset(), scheduler)
            );

            res.Messages.AssertEqual(
                OnNext(210, 1),
                OnNext(220, 2),
                OnCompleted<int>(230)
            );

            xs.Subscriptions.AssertEqual(
                Subscribe(200, 230)
            );
        }

        [TestMethod]
        public void SkipUntil_Some()
        {
            var scheduler = new TestScheduler();

            var xs = scheduler.CreateHotObservable<int>(
                OnNext(210, 1),
                OnNext(220, 2),
                OnCompleted<int>(230)
            );

            var res = scheduler.Start(() =>
                xs.SkipUntil(new DateTimeOffset(215, TimeSpan.Zero), scheduler)
            );

            res.Messages.AssertEqual(
                OnNext(220, 2),
                OnCompleted<int>(230)
            );

            xs.Subscriptions.AssertEqual(
                Subscribe(200, 230)
            );
        }

        [TestMethod]
        public void SkipUntil_Late()
        {
            var scheduler = new TestScheduler();

            var xs = scheduler.CreateHotObservable<int>(
                OnNext(210, 1),
                OnNext(220, 2),
                OnCompleted<int>(230)
            );

            var res = scheduler.Start(() =>
                xs.SkipUntil(new DateTimeOffset(250, TimeSpan.Zero), scheduler)
            );

            res.Messages.AssertEqual(
                OnCompleted<int>(230)
            );

            xs.Subscriptions.AssertEqual(
                Subscribe(200, 230)
            );
        }

        [TestMethod]
        public void SkipUntil_Error()
        {
            var scheduler = new TestScheduler();

            var ex = new Exception();

            var xs = scheduler.CreateHotObservable<int>(
                OnError<int>(210, ex)
            );

            var res = scheduler.Start(() =>
                xs.SkipUntil(new DateTimeOffset(250, TimeSpan.Zero), scheduler)
            );

            res.Messages.AssertEqual(
                OnError<int>(210, ex)
            );

            xs.Subscriptions.AssertEqual(
                Subscribe(200, 210)
            );
        }

        [TestMethod]
        public void SkipUntil_Never()
        {
            var scheduler = new TestScheduler();

            var ex = new Exception();

            var xs = scheduler.CreateHotObservable<int>(
            );

            var res = scheduler.Start(() =>
                xs.SkipUntil(new DateTimeOffset(250, TimeSpan.Zero), scheduler)
            );

            res.Messages.AssertEqual(
            );

            xs.Subscriptions.AssertEqual(
                Subscribe(200, 1000)
            );
        }

        [TestMethod]
        public void SkipUntil_Twice1()
        {
            var scheduler = new TestScheduler();

            var ex = new Exception();

            var xs = scheduler.CreateHotObservable<int>(
                OnNext(210, 1),
                OnNext(220, 2),
                OnNext(230, 3),
                OnNext(240, 4),
                OnNext(250, 5),
                OnNext(260, 6),
                OnCompleted<int>(270)
            );

            var res = scheduler.Start(() =>
                xs.SkipUntil(new DateTimeOffset(215, TimeSpan.Zero), scheduler).SkipUntil(new DateTimeOffset(230, TimeSpan.Zero), scheduler)
            );

            res.Messages.AssertEqual(
                OnNext(240, 4),
                OnNext(250, 5),
                OnNext(260, 6),
                OnCompleted<int>(270)
            );

            xs.Subscriptions.AssertEqual(
                Subscribe(200, 270)
            );
        }

        [TestMethod]
        public void SkipUntil_Twice2()
        {
            var scheduler = new TestScheduler();

            var ex = new Exception();

            var xs = scheduler.CreateHotObservable<int>(
                OnNext(210, 1),
                OnNext(220, 2),
                OnNext(230, 3),
                OnNext(240, 4),
                OnNext(250, 5),
                OnNext(260, 6),
                OnCompleted<int>(270)
            );

            var res = scheduler.Start(() =>
                xs.SkipUntil(new DateTimeOffset(230, TimeSpan.Zero), scheduler).SkipUntil(new DateTimeOffset(215, TimeSpan.Zero), scheduler)
            );

            res.Messages.AssertEqual(
                OnNext(240, 4),
                OnNext(250, 5),
                OnNext(260, 6),
                OnCompleted<int>(270)
            );

            xs.Subscriptions.AssertEqual(
                Subscribe(200, 270)
            );
        }

        [TestMethod]
        public void SkipUntil_Default()
        {
            var xs = Observable.Range(0, 10, Scheduler.Default);

            var res = xs.SkipUntil(DateTimeOffset.UtcNow.AddMinutes(1));

            var e = new ManualResetEvent(false);

            var lst = new List<int>();
            res.Subscribe(
                lst.Add,
                () => e.Set()
            );

            e.WaitOne();

            Assert.IsTrue(lst.Count == 0);
        }

        #endregion

        #region + Take +

        [TestMethod]
        public void Take_ArgumentChecking()
        {
            var xs = Observable.Return(42);

            ReactiveAssert.Throws<ArgumentNullException>(() => Observable.Take(default(IObservable<int>), TimeSpan.FromSeconds(1)));
            ReactiveAssert.Throws<ArgumentOutOfRangeException>(() => Observable.Take(xs, TimeSpan.FromSeconds(-1)));

            ReactiveAssert.Throws<ArgumentNullException>(() => Observable.Take(default(IObservable<int>), TimeSpan.FromSeconds(1), Scheduler.Default));
            ReactiveAssert.Throws<ArgumentNullException>(() => Observable.Take(xs, TimeSpan.FromSeconds(1), default(IScheduler)));
            ReactiveAssert.Throws<ArgumentOutOfRangeException>(() => Observable.Take(xs, TimeSpan.FromSeconds(-1), Scheduler.Default));
        }

        [TestMethod]
        public void Take_Zero()
        {
            var scheduler = new TestScheduler();

            var xs = scheduler.CreateHotObservable<int>(
                OnNext(210, 1),
                OnNext(220, 2),
                OnCompleted<int>(230)
            );

            var res = scheduler.Start(() =>
                xs.Take(TimeSpan.Zero, scheduler)
            );

            res.Messages.AssertEqual(
                OnCompleted<int>(201)
            );

            xs.Subscriptions.AssertEqual(
                Subscribe(200, 201)
            );
        }

        [TestMethod]
        public void Take_Some()
        {
            var scheduler = new TestScheduler();

            var xs = scheduler.CreateHotObservable<int>(
                OnNext(210, 1),
                OnNext(220, 2),
                OnNext(230, 3),
                OnCompleted<int>(240)
            );

            var res = scheduler.Start(() =>
                xs.Take(TimeSpan.FromTicks(25), scheduler)
            );

            res.Messages.AssertEqual(
                OnNext(210, 1),
                OnNext(220, 2),
                OnCompleted<int>(225)
            );

            xs.Subscriptions.AssertEqual(
                Subscribe(200, 225)
            );
        }

        [TestMethod]
        public void Take_Late()
        {
            var scheduler = new TestScheduler();

            var xs = scheduler.CreateHotObservable<int>(
                OnNext(210, 1),
                OnNext(220, 2),
                OnCompleted<int>(230)
            );

            var res = scheduler.Start(() =>
                xs.Take(TimeSpan.FromTicks(50), scheduler)
            );

            res.Messages.AssertEqual(
                OnNext(210, 1),
                OnNext(220, 2),
                OnCompleted<int>(230)
            );

            xs.Subscriptions.AssertEqual(
                Subscribe(200, 230)
            );
        }

        [TestMethod]
        public void Take_Error()
        {
            var scheduler = new TestScheduler();

            var ex = new Exception();

            var xs = scheduler.CreateHotObservable<int>(
                OnError<int>(210, ex)
            );

            var res = scheduler.Start(() =>
                xs.Take(TimeSpan.FromTicks(50), scheduler)
            );

            res.Messages.AssertEqual(
                OnError<int>(210, ex)
            );

            xs.Subscriptions.AssertEqual(
                Subscribe(200, 210)
            );
        }

        [TestMethod]
        public void Take_Never()
        {
            var scheduler = new TestScheduler();

            var ex = new Exception();

            var xs = scheduler.CreateHotObservable<int>(
            );

            var res = scheduler.Start(() =>
                xs.Take(TimeSpan.FromTicks(50), scheduler)
            );

            res.Messages.AssertEqual(
                OnCompleted<int>(250)
            );

            xs.Subscriptions.AssertEqual(
                Subscribe(200, 250)
            );
        }

        [TestMethod]
        public void Take_Twice1()
        {
            var scheduler = new TestScheduler();

            var ex = new Exception();

            var xs = scheduler.CreateHotObservable<int>(
                OnNext(210, 1),
                OnNext(220, 2),
                OnNext(230, 3),
                OnNext(240, 4),
                OnNext(250, 5),
                OnNext(260, 6),
                OnCompleted<int>(270)
            );

            var res = scheduler.Start(() =>
                xs.Take(TimeSpan.FromTicks(55), scheduler).Take(TimeSpan.FromTicks(35), scheduler)
            );

            res.Messages.AssertEqual(
                OnNext(210, 1),
                OnNext(220, 2),
                OnNext(230, 3),
                OnCompleted<int>(235)
            );

            xs.Subscriptions.AssertEqual(
                Subscribe(200, 235)
            );
        }

        [TestMethod]
        public void Take_Twice2()
        {
            var scheduler = new TestScheduler();

            var ex = new Exception();

            var xs = scheduler.CreateHotObservable<int>(
                OnNext(210, 1),
                OnNext(220, 2),
                OnNext(230, 3),
                OnNext(240, 4),
                OnNext(250, 5),
                OnNext(260, 6),
                OnCompleted<int>(270)
            );

            var res = scheduler.Start(() =>
                xs.Take(TimeSpan.FromTicks(35), scheduler).Take(TimeSpan.FromTicks(55), scheduler)
            );

            res.Messages.AssertEqual(
                OnNext(210, 1),
                OnNext(220, 2),
                OnNext(230, 3),
                OnCompleted<int>(235)
            );

            xs.Subscriptions.AssertEqual(
                Subscribe(200, 235)
            );
        }

        [TestMethod]
        public void Take_Default()
        {
            var xs = Observable.Range(0, 10, Scheduler.Default);

            var res = xs.Take(TimeSpan.FromSeconds(60));

            var e = new ManualResetEvent(false);

            var lst = new List<int>();
            res.Subscribe(
                lst.Add,
                () => e.Set()
            );

            e.WaitOne();

            Assert.IsTrue(lst.SequenceEqual(Enumerable.Range(0, 10)));
        }

        #endregion

        #region + TakeLast +

        [TestMethod]
        public void TakeLast_ArgumentChecking()
        {
            var xs = Observable.Return(42);

            ReactiveAssert.Throws<ArgumentNullException>(() => Observable.TakeLast(default(IObservable<int>), TimeSpan.FromSeconds(1)));
            ReactiveAssert.Throws<ArgumentOutOfRangeException>(() => Observable.TakeLast(xs, TimeSpan.FromSeconds(-1)));

            ReactiveAssert.Throws<ArgumentNullException>(() => Observable.TakeLast(default(IObservable<int>), TimeSpan.FromSeconds(1), Scheduler.Default));
            ReactiveAssert.Throws<ArgumentNullException>(() => Observable.TakeLast(xs, TimeSpan.FromSeconds(1), default(IScheduler)));
            ReactiveAssert.Throws<ArgumentOutOfRangeException>(() => Observable.TakeLast(xs, TimeSpan.FromSeconds(-1), Scheduler.Default));

            ReactiveAssert.Throws<ArgumentNullException>(() => Observable.TakeLast(default(IObservable<int>), TimeSpan.FromSeconds(1), Scheduler.Default, Scheduler.Default));
            ReactiveAssert.Throws<ArgumentNullException>(() => Observable.TakeLast(xs, TimeSpan.FromSeconds(1), default(IScheduler), Scheduler.Default));
            ReactiveAssert.Throws<ArgumentNullException>(() => Observable.TakeLast(xs, TimeSpan.FromSeconds(1), Scheduler.Default, default(IScheduler)));
            ReactiveAssert.Throws<ArgumentOutOfRangeException>(() => Observable.TakeLast(xs, TimeSpan.FromSeconds(-1), Scheduler.Default, Scheduler.Default));
        }

        [TestMethod]
        public void TakeLast_Zero1()
        {
            var scheduler = new TestScheduler();

            var xs = scheduler.CreateHotObservable<int>(
                OnNext(210, 1),
                OnNext(220, 2),
                OnCompleted<int>(230)
            );

            var res = scheduler.Start(() =>
                xs.TakeLast(TimeSpan.Zero, scheduler)
            );

            res.Messages.AssertEqual(
                OnCompleted<int>(230)
            );

            xs.Subscriptions.AssertEqual(
                Subscribe(200, 230)
            );
        }

        [TestMethod]
        public void TakeLast_Zero1_WithLoopScheduler()
        {
            var scheduler = new TestScheduler();

            var xs = scheduler.CreateHotObservable<int>(
                OnNext(210, 1),
                OnNext(220, 2),
                OnCompleted<int>(230)
            );

            var res = scheduler.Start(() =>
                xs.TakeLast(TimeSpan.Zero, scheduler, scheduler)
            );

            res.Messages.AssertEqual(
                OnCompleted<int>(231)
            );

            xs.Subscriptions.AssertEqual(
                Subscribe(200, 230)
            );
        }

        [TestMethod]
        public void TakeLast_Zero2()
        {
            var scheduler = new TestScheduler();

            var xs = scheduler.CreateHotObservable<int>(
                OnNext(210, 1),
                OnNext(220, 2),
                OnNext(230, 3),
                OnCompleted<int>(230)
            );

            var res = scheduler.Start(() =>
                xs.TakeLast(TimeSpan.Zero, scheduler)
            );

            res.Messages.AssertEqual(
                OnCompleted<int>(230)
            );

            xs.Subscriptions.AssertEqual(
                Subscribe(200, 230)
            );
        }

        [TestMethod]
        public void TakeLast_Zero2_WithLoopScheduler()
        {
            var scheduler = new TestScheduler();

            var xs = scheduler.CreateHotObservable<int>(
                OnNext(210, 1),
                OnNext(220, 2),
                OnNext(230, 3),
                OnCompleted<int>(230)
            );

            var res = scheduler.Start(() =>
                xs.TakeLast(TimeSpan.Zero, scheduler, scheduler)
            );

            res.Messages.AssertEqual(
                OnCompleted<int>(231)
            );

            xs.Subscriptions.AssertEqual(
                Subscribe(200, 230)
            );
        }

        [TestMethod]
        public void TakeLast_Some1()
        {
            var scheduler = new TestScheduler();

            var xs = scheduler.CreateHotObservable<int>(
                OnNext(210, 1),
                OnNext(220, 2),
                OnNext(230, 3),
                OnCompleted<int>(240)
            );

            var res = scheduler.Start(() =>
                xs.TakeLast(TimeSpan.FromTicks(25), scheduler)
            );

            res.Messages.AssertEqual(
                OnNext(240, 2),
                OnNext(240, 3),
                OnCompleted<int>(240)
            );

            xs.Subscriptions.AssertEqual(
                Subscribe(200, 240)
            );
        }

        [TestMethod]
        public void TakeLast_Some1_WithLoopScheduler()
        {
            var scheduler = new TestScheduler();

            var xs = scheduler.CreateHotObservable<int>(
                OnNext(210, 1),
                OnNext(220, 2),
                OnNext(230, 3),
                OnCompleted<int>(240)
            );

            var res = scheduler.Start(() =>
                xs.TakeLast(TimeSpan.FromTicks(25), scheduler, scheduler)
            );

            res.Messages.AssertEqual(
                OnNext(241, 2),
                OnNext(242, 3),
                OnCompleted<int>(243)
            );

            xs.Subscriptions.AssertEqual(
                Subscribe(200, 240)
            );
        }

        [TestMethod]
        public void TakeLast_Some2()
        {
            var scheduler = new TestScheduler();

            var xs = scheduler.CreateHotObservable<int>(
                OnNext(210, 1),
                OnNext(220, 2),
                OnNext(230, 3),
                OnCompleted<int>(300)
            );

            var res = scheduler.Start(() =>
                xs.TakeLast(TimeSpan.FromTicks(25), scheduler)
            );

            res.Messages.AssertEqual(
                OnCompleted<int>(300)
            );

            xs.Subscriptions.AssertEqual(
                Subscribe(200, 300)
            );
        }

        [TestMethod]
        public void TakeLast_Some2_WithLoopScheduler()
        {
            var scheduler = new TestScheduler();

            var xs = scheduler.CreateHotObservable<int>(
                OnNext(210, 1),
                OnNext(220, 2),
                OnNext(230, 3),
                OnCompleted<int>(300)
            );

            var res = scheduler.Start(() =>
                xs.TakeLast(TimeSpan.FromTicks(25), scheduler, scheduler)
            );

            res.Messages.AssertEqual(
                OnCompleted<int>(301)
            );

            xs.Subscriptions.AssertEqual(
                Subscribe(200, 300)
            );
        }

        [TestMethod]
        public void TakeLast_Some3()
        {
            var scheduler = new TestScheduler();

            var xs = scheduler.CreateHotObservable<int>(
                OnNext(210, 1),
                OnNext(220, 2),
                OnNext(230, 3),
                OnNext(240, 4),
                OnNext(250, 5),
                OnNext(260, 6),
                OnNext(270, 7),
                OnNext(280, 8),
                OnNext(290, 9),
                OnCompleted<int>(300)
            );

            var res = scheduler.Start(() =>
                xs.TakeLast(TimeSpan.FromTicks(45), scheduler)
            );

            res.Messages.AssertEqual(
                OnNext(300, 6),
                OnNext(300, 7),
                OnNext(300, 8),
                OnNext(300, 9),
                OnCompleted<int>(300)
            );

            xs.Subscriptions.AssertEqual(
                Subscribe(200, 300)
            );
        }

        [TestMethod]
        public void TakeLast_Some3_WithLoopScheduler()
        {
            var scheduler = new TestScheduler();

            var xs = scheduler.CreateHotObservable<int>(
                OnNext(210, 1),
                OnNext(220, 2),
                OnNext(230, 3),
                OnNext(240, 4),
                OnNext(250, 5),
                OnNext(260, 6),
                OnNext(270, 7),
                OnNext(280, 8),
                OnNext(290, 9),
                OnCompleted<int>(300)
            );

            var res = scheduler.Start(() =>
                xs.TakeLast(TimeSpan.FromTicks(45), scheduler, scheduler)
            );

            res.Messages.AssertEqual(
                OnNext(301, 6),
                OnNext(302, 7),
                OnNext(303, 8),
                OnNext(304, 9),
                OnCompleted<int>(305)
            );

            xs.Subscriptions.AssertEqual(
                Subscribe(200, 300)
            );
        }

        [TestMethod]
        public void TakeLast_Some4()
        {
            var scheduler = new TestScheduler();

            var xs = scheduler.CreateHotObservable<int>(
                OnNext(210, 1),
                OnNext(240, 2),
                OnNext(250, 3),
                OnNext(280, 4),
                OnNext(290, 5),
                OnNext(300, 6),
                OnCompleted<int>(350)
            );

            var res = scheduler.Start(() =>
                xs.TakeLast(TimeSpan.FromTicks(25), scheduler)
            );

            res.Messages.AssertEqual(
                OnCompleted<int>(350)
            );

            xs.Subscriptions.AssertEqual(
                Subscribe(200, 350)
            );
        }

        [TestMethod]
        public void TakeLast_Some4_WithLoopScheduler()
        {
            var scheduler = new TestScheduler();

            var xs = scheduler.CreateHotObservable<int>(
                OnNext(210, 1),
                OnNext(240, 2),
                OnNext(250, 3),
                OnNext(280, 4),
                OnNext(290, 5),
                OnNext(300, 6),
                OnCompleted<int>(350)
            );

            var res = scheduler.Start(() =>
                xs.TakeLast(TimeSpan.FromTicks(25), scheduler, scheduler)
            );

            res.Messages.AssertEqual(
                OnCompleted<int>(351)
            );

            xs.Subscriptions.AssertEqual(
                Subscribe(200, 350)
            );
        }

        [TestMethod]
        public void TakeLast_All()
        {
            var scheduler = new TestScheduler();

            var xs = scheduler.CreateHotObservable<int>(
                OnNext(210, 1),
                OnNext(220, 2),
                OnCompleted<int>(230)
            );

            var res = scheduler.Start(() =>
                xs.TakeLast(TimeSpan.FromTicks(50), scheduler)
            );

            res.Messages.AssertEqual(
                OnNext(230, 1),
                OnNext(230, 2),
                OnCompleted<int>(230)
            );

            xs.Subscriptions.AssertEqual(
                Subscribe(200, 230)
            );
        }

        [TestMethod]
        public void TakeLast_All_WithLoopScheduler()
        {
            var scheduler = new TestScheduler();

            var xs = scheduler.CreateHotObservable<int>(
                OnNext(210, 1),
                OnNext(220, 2),
                OnCompleted<int>(230)
            );

            var res = scheduler.Start(() =>
                xs.TakeLast(TimeSpan.FromTicks(50), scheduler, scheduler)
            );

            res.Messages.AssertEqual(
                OnNext(231, 1),
                OnNext(232, 2),
                OnCompleted<int>(233)
            );

            xs.Subscriptions.AssertEqual(
                Subscribe(200, 230)
            );
        }

        [TestMethod]
        public void TakeLast_Error()
        {
            var scheduler = new TestScheduler();

            var ex = new Exception();

            var xs = scheduler.CreateHotObservable<int>(
                OnError<int>(210, ex)
            );

            var res = scheduler.Start(() =>
                xs.TakeLast(TimeSpan.FromTicks(50), scheduler)
            );

            res.Messages.AssertEqual(
                OnError<int>(210, ex)
            );

            xs.Subscriptions.AssertEqual(
                Subscribe(200, 210)
            );
        }

        [TestMethod]
        public void TakeLast_Error_WithLoopScheduler()
        {
            var scheduler = new TestScheduler();

            var ex = new Exception();

            var xs = scheduler.CreateHotObservable<int>(
                OnError<int>(210, ex)
            );

            var res = scheduler.Start(() =>
                xs.TakeLast(TimeSpan.FromTicks(50), scheduler, scheduler)
            );

            res.Messages.AssertEqual(
                OnError<int>(210, ex)
            );

            xs.Subscriptions.AssertEqual(
                Subscribe(200, 210)
            );
        }

        [TestMethod]
        public void TakeLast_Never()
        {
            var scheduler = new TestScheduler();

            var ex = new Exception();

            var xs = scheduler.CreateHotObservable<int>(
            );

            var res = scheduler.Start(() =>
                xs.TakeLast(TimeSpan.FromTicks(50), scheduler)
            );

            res.Messages.AssertEqual(
            );

            xs.Subscriptions.AssertEqual(
                Subscribe(200, 1000)
            );
        }

        [TestMethod]
        public void TakeLast_Never_WithLoopScheduler()
        {
            var scheduler = new TestScheduler();

            var ex = new Exception();

            var xs = scheduler.CreateHotObservable<int>(
            );

            var res = scheduler.Start(() =>
                xs.TakeLast(TimeSpan.FromTicks(50), scheduler, scheduler)
            );

            res.Messages.AssertEqual(
            );

            xs.Subscriptions.AssertEqual(
                Subscribe(200, 1000)
            );
        }

        [TestMethod]
        public void TakeLast_Default1()
        {
            var xs = Observable.Range(0, 10, Scheduler.Default);

            var res = xs.TakeLast(TimeSpan.FromSeconds(60));

            var e = new ManualResetEvent(false);

            var lst = new List<int>();
            res.Subscribe(
                lst.Add,
                () => e.Set()
            );

            e.WaitOne();

            Assert.IsTrue(lst.SequenceEqual(Enumerable.Range(0, 10)));
        }

        [TestMethod]
        public void TakeLast_Default2()
        {
            var xs = Observable.Range(0, 10, Scheduler.Default);

            var res = xs.TakeLast(TimeSpan.FromSeconds(60), Scheduler.Default.DisableOptimizations());

            var e = new ManualResetEvent(false);

            var lst = new List<int>();
            res.Subscribe(
                lst.Add,
                () => e.Set()
            );

            e.WaitOne();

            Assert.IsTrue(lst.SequenceEqual(Enumerable.Range(0, 10)));
        }

        [TestMethod]
        public void TakeLast_Default3()
        {
            var xs = Observable.Range(0, 10, Scheduler.Default);

            var res = xs.TakeLast(TimeSpan.Zero);

            var e = new ManualResetEvent(false);

            var lst = new List<int>();
            res.Subscribe(
                lst.Add,
                () => e.Set()
            );

            e.WaitOne();

            Assert.IsTrue(lst.Count == 0);
        }

        [TestMethod]
        public void TakeLast_Default4()
        {
            var xs = Observable.Range(0, 10, Scheduler.Default);

            var res = xs.TakeLast(TimeSpan.Zero, Scheduler.Default.DisableOptimizations());

            var e = new ManualResetEvent(false);

            var lst = new List<int>();
            res.Subscribe(
                lst.Add,
                () => e.Set()
            );

            e.WaitOne();

            Assert.IsTrue(lst.Count == 0);
        }

        [TestMethod]
        public void TakeLast_LongRunning_Regular()
        {
            var res = Observable.Range(0, 10, Scheduler.Default).TakeLast(TimeSpan.FromSeconds(60), Scheduler.Default, NewThreadScheduler.Default);

            var lst = new List<int>();
            res.ForEach(lst.Add);

            Assert.IsTrue(Enumerable.Range(0, 10).SequenceEqual(lst));
        }

        #endregion

        #region + TakeLastBuffer +

        [TestMethod]
        public void TakeLastBuffer_ArgumentChecking()
        {
            var xs = Observable.Return(42);

            ReactiveAssert.Throws<ArgumentNullException>(() => Observable.TakeLastBuffer(default(IObservable<int>), TimeSpan.FromSeconds(1)));
            ReactiveAssert.Throws<ArgumentOutOfRangeException>(() => Observable.TakeLastBuffer(xs, TimeSpan.FromSeconds(-1)));

            ReactiveAssert.Throws<ArgumentNullException>(() => Observable.TakeLastBuffer(default(IObservable<int>), TimeSpan.FromSeconds(1), Scheduler.Default));
            ReactiveAssert.Throws<ArgumentNullException>(() => Observable.TakeLastBuffer(xs, TimeSpan.FromSeconds(1), default(IScheduler)));
            ReactiveAssert.Throws<ArgumentOutOfRangeException>(() => Observable.TakeLastBuffer(xs, TimeSpan.FromSeconds(-1), Scheduler.Default));
        }

        [TestMethod]
        public void TakeLastBuffer_Zero1()
        {
            var scheduler = new TestScheduler();

            var xs = scheduler.CreateHotObservable<int>(
                OnNext(210, 1),
                OnNext(220, 2),
                OnCompleted<int>(230)
            );

            var res = scheduler.Start(() =>
                xs.TakeLastBuffer(TimeSpan.Zero, scheduler)
            );

            res.Messages.AssertEqual(
                OnNext<IList<int>>(230, lst => lst.Count == 0),
                OnCompleted<IList<int>>(230)
            );

            xs.Subscriptions.AssertEqual(
                Subscribe(200, 230)
            );
        }

        [TestMethod]
        public void TakeLastBuffer_Zero2()
        {
            var scheduler = new TestScheduler();

            var xs = scheduler.CreateHotObservable<int>(
                OnNext(210, 1),
                OnNext(220, 2),
                OnNext(230, 3),
                OnCompleted<int>(230)
            );

            var res = scheduler.Start(() =>
                xs.TakeLastBuffer(TimeSpan.Zero, scheduler)
            );

            res.Messages.AssertEqual(
                OnNext<IList<int>>(230, lst => lst.Count == 0),
                OnCompleted<IList<int>>(230)
            );

            xs.Subscriptions.AssertEqual(
                Subscribe(200, 230)
            );
        }

        [TestMethod]
        public void TakeLastBuffer_Some1()
        {
            var scheduler = new TestScheduler();

            var xs = scheduler.CreateHotObservable<int>(
                OnNext(210, 1),
                OnNext(220, 2),
                OnNext(230, 3),
                OnCompleted<int>(240)
            );

            var res = scheduler.Start(() =>
                xs.TakeLastBuffer(TimeSpan.FromTicks(25), scheduler)
            );

            res.Messages.AssertEqual(
                OnNext<IList<int>>(240, lst => lst.SequenceEqual(new[] { 2, 3 })),
                OnCompleted<IList<int>>(240)
            );

            xs.Subscriptions.AssertEqual(
                Subscribe(200, 240)
            );
        }

        [TestMethod]
        public void TakeLastBuffer_Some2()
        {
            var scheduler = new TestScheduler();

            var xs = scheduler.CreateHotObservable<int>(
                OnNext(210, 1),
                OnNext(220, 2),
                OnNext(230, 3),
                OnCompleted<int>(300)
            );

            var res = scheduler.Start(() =>
                xs.TakeLastBuffer(TimeSpan.FromTicks(25), scheduler)
            );

            res.Messages.AssertEqual(
                OnNext<IList<int>>(300, lst => lst.Count == 0),
                OnCompleted<IList<int>>(300)
            );

            xs.Subscriptions.AssertEqual(
                Subscribe(200, 300)
            );
        }

        [TestMethod]
        public void TakeLastBuffer_Some3()
        {
            var scheduler = new TestScheduler();

            var xs = scheduler.CreateHotObservable<int>(
                OnNext(210, 1),
                OnNext(220, 2),
                OnNext(230, 3),
                OnNext(240, 4),
                OnNext(250, 5),
                OnNext(260, 6),
                OnNext(270, 7),
                OnNext(280, 8),
                OnNext(290, 9),
                OnCompleted<int>(300)
            );

            var res = scheduler.Start(() =>
                xs.TakeLastBuffer(TimeSpan.FromTicks(45), scheduler)
            );

            res.Messages.AssertEqual(
                OnNext<IList<int>>(300, lst => lst.SequenceEqual(new[] { 6, 7, 8, 9 })),
                OnCompleted<IList<int>>(300)
            );

            xs.Subscriptions.AssertEqual(
                Subscribe(200, 300)
            );
        }

        [TestMethod]
        public void TakeLastBuffer_Some4()
        {
            var scheduler = new TestScheduler();

            var xs = scheduler.CreateHotObservable<int>(
                OnNext(210, 1),
                OnNext(240, 2),
                OnNext(250, 3),
                OnNext(280, 4),
                OnNext(290, 5),
                OnNext(300, 6),
                OnCompleted<int>(350)
            );

            var res = scheduler.Start(() =>
                xs.TakeLastBuffer(TimeSpan.FromTicks(25), scheduler)
            );

            res.Messages.AssertEqual(
                OnNext<IList<int>>(350, lst => lst.Count == 0),
                OnCompleted<IList<int>>(350)
            );

            xs.Subscriptions.AssertEqual(
                Subscribe(200, 350)
            );
        }

        [TestMethod]
        public void TakeLastBuffer_All()
        {
            var scheduler = new TestScheduler();

            var xs = scheduler.CreateHotObservable<int>(
                OnNext(210, 1),
                OnNext(220, 2),
                OnCompleted<int>(230)
            );

            var res = scheduler.Start(() =>
                xs.TakeLastBuffer(TimeSpan.FromTicks(50), scheduler)
            );

            res.Messages.AssertEqual(
                OnNext<IList<int>>(230, lst => lst.SequenceEqual(new[] { 1, 2 })),
                OnCompleted<IList<int>>(230)
            );

            xs.Subscriptions.AssertEqual(
                Subscribe(200, 230)
            );
        }

        [TestMethod]
        public void TakeLastBuffer_Error()
        {
            var scheduler = new TestScheduler();

            var ex = new Exception();

            var xs = scheduler.CreateHotObservable<int>(
                OnError<int>(210, ex)
            );

            var res = scheduler.Start(() =>
                xs.TakeLastBuffer(TimeSpan.FromTicks(50), scheduler)
            );

            res.Messages.AssertEqual(
                OnError<IList<int>>(210, ex)
            );

            xs.Subscriptions.AssertEqual(
                Subscribe(200, 210)
            );
        }

        [TestMethod]
        public void TakeLastBuffer_Never()
        {
            var scheduler = new TestScheduler();

            var ex = new Exception();

            var xs = scheduler.CreateHotObservable<int>(
            );

            var res = scheduler.Start(() =>
                xs.TakeLastBuffer(TimeSpan.FromTicks(50), scheduler)
            );

            res.Messages.AssertEqual(
            );

            xs.Subscriptions.AssertEqual(
                Subscribe(200, 1000)
            );
        }

        [TestMethod]
        public void TakeLastBuffer_Default1()
        {
            var xs = Observable.Range(0, 10, Scheduler.Default);

            var res = xs.TakeLastBuffer(TimeSpan.FromSeconds(60)).SingleAsync();

            var e = new ManualResetEvent(false);

            var lst = default (IList<int>);
            res.Subscribe(
                x => lst = x,
                () => e.Set()
            );

            e.WaitOne();

            Assert.IsTrue(lst.SequenceEqual(Enumerable.Range(0, 10)));
        }

        [TestMethod]
        public void TakeLastBuffer_Default2()
        {
            var xs = Observable.Range(0, 10, Scheduler.Default);

            var res = xs.TakeLastBuffer(TimeSpan.FromSeconds(60), Scheduler.Default.DisableOptimizations()).SingleAsync();

            var e = new ManualResetEvent(false);

            var lst = default(IList<int>);
            res.Subscribe(
                x => lst = x,
                () => e.Set()
            );

            e.WaitOne();

            Assert.IsTrue(lst.SequenceEqual(Enumerable.Range(0, 10)));
        }

        [TestMethod]
        public void TakeLastBuffer_Default3()
        {
            var xs = Observable.Range(0, 10, Scheduler.Default);

            var res = xs.TakeLastBuffer(TimeSpan.Zero).SingleAsync();

            var e = new ManualResetEvent(false);

            var lst = default(IList<int>);
            res.Subscribe(
                x => lst = x,
                () => e.Set()
            );

            e.WaitOne();

            Assert.IsTrue(lst.Count == 0);
        }

        [TestMethod]
        public void TakeLastBuffer_Default4()
        {
            var xs = Observable.Range(0, 10, Scheduler.Default);

            var res = xs.TakeLastBuffer(TimeSpan.Zero, Scheduler.Default.DisableOptimizations()).SingleAsync();

            var e = new ManualResetEvent(false);

            var lst = default(IList<int>);
            res.Subscribe(
                x => lst = x,
                () => e.Set()
            );

            e.WaitOne();

            Assert.IsTrue(lst.Count == 0);
        }

        #endregion

        #region + TakeUntil +

        [TestMethod]
        public void TakeUntil_ArgumentChecking()
        {
            var xs = Observable.Return(42);

            ReactiveAssert.Throws<ArgumentNullException>(() => Observable.TakeUntil(default(IObservable<int>), DateTimeOffset.Now));

            ReactiveAssert.Throws<ArgumentNullException>(() => Observable.TakeUntil(default(IObservable<int>), DateTimeOffset.Now, Scheduler.Default));
            ReactiveAssert.Throws<ArgumentNullException>(() => Observable.TakeUntil(xs, DateTimeOffset.Now, default(IScheduler)));
        }

        [TestMethod]
        public void TakeUntil_Zero()
        {
            var scheduler = new TestScheduler();

            var xs = scheduler.CreateHotObservable<int>(
                OnNext(210, 1),
                OnNext(220, 2),
                OnCompleted<int>(230)
            );

            var res = scheduler.Start(() =>
                xs.TakeUntil(new DateTimeOffset(), scheduler)
            );

            res.Messages.AssertEqual(
                OnCompleted<int>(201)
            );

            xs.Subscriptions.AssertEqual(
                Subscribe(200, 201)
            );
        }

        [TestMethod]
        public void TakeUntil_Some()
        {
            var scheduler = new TestScheduler();

            var xs = scheduler.CreateHotObservable<int>(
                OnNext(210, 1),
                OnNext(220, 2),
                OnNext(230, 3),
                OnCompleted<int>(240)
            );

            var res = scheduler.Start(() =>
                xs.TakeUntil(new DateTimeOffset(225, TimeSpan.Zero), scheduler)
            );

            res.Messages.AssertEqual(
                OnNext(210, 1),
                OnNext(220, 2),
                OnCompleted<int>(225)
            );

            xs.Subscriptions.AssertEqual(
                Subscribe(200, 225)
            );
        }

        [TestMethod]
        public void TakeUntil_Late()
        {
            var scheduler = new TestScheduler();

            var xs = scheduler.CreateHotObservable<int>(
                OnNext(210, 1),
                OnNext(220, 2),
                OnCompleted<int>(230)
            );

            var res = scheduler.Start(() =>
                xs.TakeUntil(new DateTimeOffset(250, TimeSpan.Zero), scheduler)
            );

            res.Messages.AssertEqual(
                OnNext(210, 1),
                OnNext(220, 2),
                OnCompleted<int>(230)
            );

            xs.Subscriptions.AssertEqual(
                Subscribe(200, 230)
            );
        }

        [TestMethod]
        public void TakeUntil_Error()
        {
            var scheduler = new TestScheduler();

            var ex = new Exception();

            var xs = scheduler.CreateHotObservable<int>(
                OnError<int>(210, ex)
            );

            var res = scheduler.Start(() =>
                xs.TakeUntil(new DateTimeOffset(250, TimeSpan.Zero), scheduler)
            );

            res.Messages.AssertEqual(
                OnError<int>(210, ex)
            );

            xs.Subscriptions.AssertEqual(
                Subscribe(200, 210)
            );
        }

        [TestMethod]
        public void TakeUntil_Never()
        {
            var scheduler = new TestScheduler();

            var ex = new Exception();

            var xs = scheduler.CreateHotObservable<int>(
            );

            var res = scheduler.Start(() =>
                xs.TakeUntil(new DateTimeOffset(250, TimeSpan.Zero), scheduler)
            );

            res.Messages.AssertEqual(
                OnCompleted<int>(250)
            );

            xs.Subscriptions.AssertEqual(
                Subscribe(200, 250)
            );
        }

        [TestMethod]
        public void TakeUntil_Twice1()
        {
            var scheduler = new TestScheduler();

            var ex = new Exception();

            var xs = scheduler.CreateHotObservable<int>(
                OnNext(210, 1),
                OnNext(220, 2),
                OnNext(230, 3),
                OnNext(240, 4),
                OnNext(250, 5),
                OnNext(260, 6),
                OnCompleted<int>(270)
            );

            var res = scheduler.Start(() =>
                xs.TakeUntil(new DateTimeOffset(255, TimeSpan.Zero), scheduler).TakeUntil(new DateTimeOffset(235, TimeSpan.Zero), scheduler)
            );

            res.Messages.AssertEqual(
                OnNext(210, 1),
                OnNext(220, 2),
                OnNext(230, 3),
                OnCompleted<int>(235)
            );

            xs.Subscriptions.AssertEqual(
                Subscribe(200, 235)
            );
        }

        [TestMethod]
        public void TakeUntil_Twice2()
        {
            var scheduler = new TestScheduler();

            var ex = new Exception();

            var xs = scheduler.CreateHotObservable<int>(
                OnNext(210, 1),
                OnNext(220, 2),
                OnNext(230, 3),
                OnNext(240, 4),
                OnNext(250, 5),
                OnNext(260, 6),
                OnCompleted<int>(270)
            );

            var res = scheduler.Start(() =>
                xs.TakeUntil(new DateTimeOffset(235, TimeSpan.Zero), scheduler).TakeUntil(new DateTimeOffset(255, TimeSpan.Zero), scheduler)
            );

            res.Messages.AssertEqual(
                OnNext(210, 1),
                OnNext(220, 2),
                OnNext(230, 3),
                OnCompleted<int>(235)
            );

            xs.Subscriptions.AssertEqual(
                Subscribe(200, 235)
            );
        }

        [TestMethod]
        public void TakeUntil_Default()
        {
            var xs = Observable.Range(0, 10, Scheduler.Default);

            var res = xs.TakeUntil(DateTimeOffset.Now.AddMinutes(1));

            var e = new ManualResetEvent(false);

            var lst = new List<int>();
            res.Subscribe(
                lst.Add,
                () => e.Set()
            );

            e.WaitOne();

            Assert.IsTrue(lst.SequenceEqual(Enumerable.Range(0, 10)));
        }

        #endregion

        #region + Throttle +

        [TestMethod]
        public void Throttle_ArgumentChecking()
        {
            var scheduler = new TestScheduler();
            var someObservable = Observable.Empty<int>();

            ReactiveAssert.Throws<ArgumentNullException>(() => Observable.Throttle(default(IObservable<int>), TimeSpan.Zero));
            ReactiveAssert.Throws<ArgumentNullException>(() => Observable.Throttle(someObservable, TimeSpan.Zero, null));
            ReactiveAssert.Throws<ArgumentNullException>(() => Observable.Throttle(default(IObservable<int>), TimeSpan.Zero, scheduler));

            ReactiveAssert.Throws<ArgumentOutOfRangeException>(() => Observable.Throttle(someObservable, TimeSpan.FromSeconds(-1)));
            ReactiveAssert.Throws<ArgumentOutOfRangeException>(() => Observable.Throttle(someObservable, TimeSpan.FromSeconds(-1), scheduler));
        }

        private IEnumerable<Recorded<Notification<T>>> Generate<T, S>(S seed, Func<S, bool> condition, Func<S, S> iterate, Func<S, Recorded<Notification<T>>> selector, Func<S, Recorded<Notification<T>>> final)
        {
            S s;
            for (s = seed; condition(s); s = iterate(s))
                yield return selector(s);

            yield return final(s);
        }

        [TestMethod]
        public void Throttle_TimeSpan_AllPass()
        {
            var scheduler = new TestScheduler();

            var xs = scheduler.CreateHotObservable(
                OnNext(150, 0),
                OnNext(210, 1),
                OnNext(240, 2),
                OnNext(270, 3),
                OnNext(300, 4),
                OnCompleted<int>(400)
            );

            var res = scheduler.Start(() =>
                xs.Throttle(TimeSpan.FromTicks(20), scheduler)
            );

            res.Messages.AssertEqual(
                OnNext(230, 1),
                OnNext(260, 2),
                OnNext(290, 3),
                OnNext(320, 4),
                OnCompleted<int>(400)
            );

            xs.Subscriptions.AssertEqual(
                Subscribe(200, 400)
            );
        }

        [TestMethod]
        public void Throttle_TimeSpan_AllPass_ErrorEnd()
        {
            var ex = new Exception();

            var scheduler = new TestScheduler();

            var xs = scheduler.CreateHotObservable(
                OnNext(150, 0),
                OnNext(210, 1),
                OnNext(240, 2),
                OnNext(270, 3),
                OnNext(300, 4),
                OnError<int>(400, ex)
            );

            var res = scheduler.Start(() =>
                xs.Throttle(TimeSpan.FromTicks(20), scheduler)
            );

            res.Messages.AssertEqual(
                OnNext(230, 1),
                OnNext(260, 2),
                OnNext(290, 3),
                OnNext(320, 4),
                OnError<int>(400, ex)
            );

            xs.Subscriptions.AssertEqual(
                Subscribe(200, 400)
            );
        }

        [TestMethod]
        public void Throttle_TimeSpan_AllDrop()
        {
            var scheduler = new TestScheduler();

            var xs = scheduler.CreateHotObservable(
                OnNext(150, 0),
                OnNext(210, 1),
                OnNext(240, 2),
                OnNext(270, 3),
                OnNext(300, 4),
                OnNext(330, 5),
                OnNext(360, 6),
                OnNext(390, 7),
                OnCompleted<int>(400)
            );

            var res = scheduler.Start(() =>
                xs.Throttle(TimeSpan.FromTicks(40), scheduler)
            );

            res.Messages.AssertEqual(
                OnNext(400, 7),
                OnCompleted<int>(400)
            );

            xs.Subscriptions.AssertEqual(
                Subscribe(200, 400)
            );
        }

        [TestMethod]
        public void Throttle_TimeSpan_AllDrop_ErrorEnd()
        {
            var ex = new Exception();

            var scheduler = new TestScheduler();

            var xs = scheduler.CreateHotObservable(
                OnNext(150, 0),
                OnNext(210, 1),
                OnNext(240, 2),
                OnNext(270, 3),
                OnNext(300, 4),
                OnNext(330, 5),
                OnNext(360, 6),
                OnNext(390, 7),
                OnError<int>(400, ex)
            );

            var res = scheduler.Start(() =>
                xs.Throttle(TimeSpan.FromTicks(40), scheduler)
            );

            res.Messages.AssertEqual(
                OnError<int>(400, ex)
            );

            xs.Subscriptions.AssertEqual(
                Subscribe(200, 400)
            );
        }

        [TestMethod]
        public void Throttle_Empty()
        {
            var scheduler = new TestScheduler();

            var xs = scheduler.CreateHotObservable(
                OnNext(150, 0),
                OnCompleted<int>(300)
            );

            var res = scheduler.Start(() =>
                xs.Throttle(TimeSpan.FromTicks(10), scheduler)
            );

            res.Messages.AssertEqual(
                OnCompleted<int>(300)
            );

            xs.Subscriptions.AssertEqual(
                Subscribe(200, 300)
            );
        }

        [TestMethod]
        public void Throttle_Error()
        {
            var scheduler = new TestScheduler();

            var ex = new Exception();

            var xs = scheduler.CreateHotObservable(
                OnNext(150, 0),
                OnError<int>(300, ex)
            );

            var res = scheduler.Start(() =>
                xs.Throttle(TimeSpan.FromTicks(10), scheduler)
            );

            res.Messages.AssertEqual(
                OnError<int>(300, ex)
            );

            xs.Subscriptions.AssertEqual(
                Subscribe(200, 300)
            );
        }

        [TestMethod]
        public void Throttle_Never()
        {
            var scheduler = new TestScheduler();

            var xs = scheduler.CreateHotObservable(
                OnNext(150, 0)
            );

            var res = scheduler.Start(() =>
                xs.Throttle(TimeSpan.FromTicks(10), scheduler)
            );

            res.Messages.AssertEqual(
            );

            xs.Subscriptions.AssertEqual(
                Subscribe(200, 1000)
            );
        }

        [TestMethod]
        public void Throttle_Simple()
        {
            var scheduler = new TestScheduler();

            var xs = scheduler.CreateHotObservable(
                OnNext(150, 0),
                OnNext(210, 1),
                OnNext(240, 2),
                OnNext(250, 3),
                OnNext(280, 4),
                OnCompleted<int>(300)
            );

            var res = scheduler.Start(() =>
                xs.Throttle(TimeSpan.FromTicks(20), scheduler)
            );

            res.Messages.AssertEqual(
                OnNext(230, 1),
                OnNext(270, 3),
                OnNext(300, 4),
                OnCompleted<int>(300)
            );

            xs.Subscriptions.AssertEqual(
                Subscribe(200, 300)
            );
        }

        [TestMethod]
        public void Throttle_DefaultScheduler()
        {
            Assert.IsTrue(Observable.Return(1).Throttle(TimeSpan.FromMilliseconds(1)).ToEnumerable().SequenceEqual(new[] { 1 }));
        }

        [TestMethod]
        public void Throttle_Duration_ArgumentChecking()
        {
            var someObservable = DummyObservable<int>.Instance;

            ReactiveAssert.Throws<ArgumentNullException>(() => Observable.Throttle(default(IObservable<int>), x => someObservable));
            ReactiveAssert.Throws<ArgumentNullException>(() => Observable.Throttle(someObservable, default(Func<int, IObservable<string>>)));
        }

        [TestMethod]
        public void Throttle_Duration_DelayBehavior()
        {
            var scheduler = new TestScheduler();

            var xs = scheduler.CreateHotObservable(
                OnNext(150, -1),
                OnNext(250, 0),
                OnNext(280, 1),
                OnNext(310, 2),
                OnNext(350, 3),
                OnNext(400, 4),
                OnCompleted<int>(550)
            );

            var ys = new[] {
                scheduler.CreateColdObservable(
                    OnNext(20, 42),
                    OnNext(25, 99)
                ),
                scheduler.CreateColdObservable(
                    OnNext(20, 42),
                    OnNext(25, 99)
                ),
                scheduler.CreateColdObservable(
                    OnNext(20, 42),
                    OnNext(25, 99)
                ),
                scheduler.CreateColdObservable(
                    OnNext(20, 42),
                    OnNext(25, 99)
                ),
                scheduler.CreateColdObservable(
                    OnNext(20, 42),
                    OnNext(25, 99)
                ),
            };

            var res = scheduler.Start(() =>
                xs.Throttle(x => ys[x])
            );

            res.Messages.AssertEqual(
                OnNext<int>(250 + 20, 0),
                OnNext<int>(280 + 20, 1),
                OnNext<int>(310 + 20, 2),
                OnNext<int>(350 + 20, 3),
                OnNext<int>(400 + 20, 4),
                OnCompleted<int>(550)
            );

            xs.Subscriptions.AssertEqual(
                Subscribe(200, 550)
            );

            ys[0].Subscriptions.AssertEqual(Subscribe(250, 250 + 20));
            ys[1].Subscriptions.AssertEqual(Subscribe(280, 280 + 20));
            ys[2].Subscriptions.AssertEqual(Subscribe(310, 310 + 20));
            ys[3].Subscriptions.AssertEqual(Subscribe(350, 350 + 20));
            ys[4].Subscriptions.AssertEqual(Subscribe(400, 400 + 20));
        }

        [TestMethod]
        public void Throttle_Duration_ThrottleBehavior()
        {
            var scheduler = new TestScheduler();

            var xs = scheduler.CreateHotObservable(
                OnNext(150, -1),
                OnNext(250, 0),
                OnNext(280, 1),
                OnNext(310, 2),
                OnNext(350, 3),
                OnNext(400, 4),
                OnCompleted<int>(550)
            );

            var ys = new[] {
                scheduler.CreateColdObservable(
                    OnNext(20, 42),
                    OnNext(25, 99)
                ),
                scheduler.CreateColdObservable(
                    OnNext(40, 42),
                    OnNext(45, 99)
                ),
                scheduler.CreateColdObservable(
                    OnNext(20, 42),
                    OnNext(25, 99)
                ),
                scheduler.CreateColdObservable(
                    OnNext(60, 42),
                    OnNext(65, 99)
                ),
                scheduler.CreateColdObservable(
                    OnNext(20, 42),
                    OnNext(25, 99)
                ),
            };

            var res = scheduler.Start(() =>
                xs.Throttle(x => ys[x])
            );

            res.Messages.AssertEqual(
                OnNext<int>(250 + 20, 0),
                OnNext<int>(310 + 20, 2),
                OnNext<int>(400 + 20, 4),
                OnCompleted<int>(550)
            );

            xs.Subscriptions.AssertEqual(
                Subscribe(200, 550)
            );

            ys[0].Subscriptions.AssertEqual(Subscribe(250, 250 + 20));
            ys[1].Subscriptions.AssertEqual(Subscribe(280, 310));
            ys[2].Subscriptions.AssertEqual(Subscribe(310, 310 + 20));
            ys[3].Subscriptions.AssertEqual(Subscribe(350, 400));
            ys[4].Subscriptions.AssertEqual(Subscribe(400, 400 + 20));
        }

        [TestMethod]
        public void Throttle_Duration_EarlyCompletion()
        {
            var scheduler = new TestScheduler();

            var xs = scheduler.CreateHotObservable(
                OnNext(150, -1),
                OnNext(250, 0),
                OnNext(280, 1),
                OnNext(310, 2),
                OnNext(350, 3),
                OnNext(400, 4),
                OnCompleted<int>(410)
            );

            var ys = new[] {
                scheduler.CreateColdObservable(
                    OnNext(20, 42),
                    OnNext(25, 99)
                ),
                scheduler.CreateColdObservable(
                    OnNext(40, 42),
                    OnNext(45, 99)
                ),
                scheduler.CreateColdObservable(
                    OnNext(20, 42),
                    OnNext(25, 99)
                ),
                scheduler.CreateColdObservable(
                    OnNext(60, 42),
                    OnNext(65, 99)
                ),
                scheduler.CreateColdObservable(
                    OnNext(20, 42),
                    OnNext(25, 99)
                ),
            };

            var res = scheduler.Start(() =>
                xs.Throttle(x => ys[x])
            );

            res.Messages.AssertEqual(
                OnNext<int>(250 + 20, 0),
                OnNext<int>(310 + 20, 2),
                OnNext<int>(410, 4),
                OnCompleted<int>(410)
            );

            xs.Subscriptions.AssertEqual(
                Subscribe(200, 410)
            );

            ys[0].Subscriptions.AssertEqual(Subscribe(250, 250 + 20));
            ys[1].Subscriptions.AssertEqual(Subscribe(280, 310));
            ys[2].Subscriptions.AssertEqual(Subscribe(310, 310 + 20));
            ys[3].Subscriptions.AssertEqual(Subscribe(350, 400));
            ys[4].Subscriptions.AssertEqual(Subscribe(400, 410));
        }

        [TestMethod]
        public void Throttle_Duration_InnerError()
        {
            var scheduler = new TestScheduler();

            var xs = scheduler.CreateHotObservable(
                OnNext(150, 1),
                OnNext(250, 2),
                OnNext(350, 3),
                OnNext(450, 4),
                OnCompleted<int>(550)
            );

            var ex = new Exception();

            var res = scheduler.Start(() =>
                xs.Throttle(x =>
                    x < 4 ? scheduler.CreateColdObservable(
                                OnNext(x * 10, "Ignore"),
                                OnNext(x * 10 + 5, "Aargh!")
                            )
                          : scheduler.CreateColdObservable(
                                OnError<string>(x * 10, ex)
                            )
                )
            );

            res.Messages.AssertEqual(
                OnNext<int>(250 + 2 * 10, 2),
                OnNext<int>(350 + 3 * 10, 3),
                OnError<int>(450 + 4 * 10, ex)
            );

            xs.Subscriptions.AssertEqual(
                Subscribe(200, 490)
            );
        }

        [TestMethod]
        public void Throttle_Duration_OuterError()
        {
            var scheduler = new TestScheduler();

            var ex = new Exception();

            var xs = scheduler.CreateHotObservable(
                OnNext(150, 1),
                OnNext(250, 2),
                OnNext(350, 3),
                OnNext(450, 4),
                OnError<int>(460, ex)
            );

            var res = scheduler.Start(() =>
                xs.Throttle(x =>
                    scheduler.CreateColdObservable(
                        OnNext(x * 10, "Ignore"),
                        OnNext(x * 10 + 5, "Aargh!")
                    )
                )
            );

            res.Messages.AssertEqual(
                OnNext<int>(250 + 2 * 10, 2),
                OnNext<int>(350 + 3 * 10, 3),
                OnError<int>(460, ex)
            );

            xs.Subscriptions.AssertEqual(
                Subscribe(200, 460)
            );
        }

        [TestMethod]
        public void Throttle_Duration_SelectorThrows()
        {
            var scheduler = new TestScheduler();

            var xs = scheduler.CreateHotObservable(
                OnNext(150, 1),
                OnNext(250, 2),
                OnNext(350, 3),
                OnNext(450, 4),
                OnCompleted<int>(550)
            );

            var ex = new Exception();

            var res = scheduler.Start(() =>
                xs.Throttle(x =>
                {
                    if (x < 4)
                    {
                        return scheduler.CreateColdObservable(
                                OnNext(x * 10, "Ignore"),
                                OnNext(x * 10 + 5, "Aargh!")
                            );
                    }
                    else
                        throw ex;
                })
            );

            res.Messages.AssertEqual(
                OnNext<int>(250 + 2 * 10, 2),
                OnNext<int>(350 + 3 * 10, 3),
                OnError<int>(450, ex)
            );

            xs.Subscriptions.AssertEqual(
                Subscribe(200, 450)
            );
        }

        [TestMethod]
        public void Throttle_Duration_InnerDone_DelayBehavior()
        {
            var scheduler = new TestScheduler();

            var xs = scheduler.CreateHotObservable(
                OnNext(150, 1),
                OnNext(250, 2),
                OnNext(350, 3),
                OnNext(450, 4),
                OnCompleted<int>(550)
            );

            var ex = new Exception();

            var res = scheduler.Start(() =>
                xs.Throttle(x =>
                    scheduler.CreateColdObservable(
                        OnCompleted<string>(x * 10)
                    )
                )
            );

            res.Messages.AssertEqual(
                OnNext<int>(250 + 2 * 10, 2),
                OnNext<int>(350 + 3 * 10, 3),
                OnNext<int>(450 + 4 * 10, 4),
                OnCompleted<int>(550)
            );

            xs.Subscriptions.AssertEqual(
                Subscribe(200, 550)
            );
        }

        [TestMethod]
        public void Throttle_Duration_InnerDone_ThrottleBehavior()
        {
            var scheduler = new TestScheduler();

            var xs = scheduler.CreateHotObservable(
                OnNext(150, 1),
                OnNext(250, 2),
                OnNext(280, 3),
                OnNext(300, 4),
                OnNext(400, 5),
                OnNext(410, 6),
                OnCompleted<int>(550)
            );

            var ex = new Exception();

            var res = scheduler.Start(() =>
                xs.Throttle(x =>
                    scheduler.CreateColdObservable(
                        OnCompleted<string>(x * 10)
                    )
                )
            );

            res.Messages.AssertEqual(
                OnNext<int>(250 + 2 * 10, 2),
                OnNext<int>(300 + 4 * 10, 4),
                OnNext<int>(410 + 6 * 10, 6),
                OnCompleted<int>(550)
            );

            xs.Subscriptions.AssertEqual(
                Subscribe(200, 550)
            );
        }

        #endregion

        #region + TimeInterval +

        [TestMethod]
        public void TimeInterval_ArgumentChecking()
        {
            var scheduler = new TestScheduler();
            var someObservable = Observable.Empty<int>();

            ReactiveAssert.Throws<ArgumentNullException>(() => Observable.TimeInterval(default(IObservable<int>)));
            ReactiveAssert.Throws<ArgumentNullException>(() => Observable.TimeInterval(default(IObservable<int>), scheduler));
            ReactiveAssert.Throws<ArgumentNullException>(() => Observable.TimeInterval(someObservable, null));
        }

        [TestMethod]
        public void TimeInterval_Regular()
        {
            var scheduler = new TestScheduler();

            var xs = scheduler.CreateHotObservable(
                OnNext(150, 1),
                OnNext(210, 2),
                OnNext(230, 3),
                OnNext(260, 4),
                OnNext(300, 5),
                OnNext(350, 6),
                OnCompleted<int>(400)
            );

            var res = scheduler.Start(() =>
                xs.TimeInterval(scheduler.DisableOptimizations())
            );

            res.Messages.AssertEqual(
                OnNext(210, new TimeInterval<int>(2, TimeSpan.FromTicks(10))),
                OnNext(230, new TimeInterval<int>(3, TimeSpan.FromTicks(20))),
                OnNext(260, new TimeInterval<int>(4, TimeSpan.FromTicks(30))),
                OnNext(300, new TimeInterval<int>(5, TimeSpan.FromTicks(40))),
                OnNext(350, new TimeInterval<int>(6, TimeSpan.FromTicks(50))),
                OnCompleted<TimeInterval<int>>(400)
            );

            xs.Subscriptions.AssertEqual(
                Subscribe(200, 400)
            );
        }

        [TestMethod]
        public void TimeInterval_Empty()
        {
            var scheduler = new TestScheduler();

            var xs = scheduler.CreateHotObservable(
                OnNext(150, 1),
                OnCompleted<int>(300)
            );

            var res = scheduler.Start(() =>
                xs.TimeInterval(scheduler.DisableOptimizations())
            );

            res.Messages.AssertEqual(
                OnCompleted<TimeInterval<int>>(300)
            );

            xs.Subscriptions.AssertEqual(
                Subscribe(200, 300)
            );
        }

        [TestMethod]
        public void TimeInterval_Error()
        {
            var scheduler = new TestScheduler();

            var ex = new Exception();

            var xs = scheduler.CreateHotObservable(
                OnNext(150, 1),
                OnError<int>(300, ex)
            );

            var res = scheduler.Start(() =>
                xs.TimeInterval(scheduler.DisableOptimizations())
            );

            res.Messages.AssertEqual(
                OnError<TimeInterval<int>>(300, ex)
            );

            xs.Subscriptions.AssertEqual(
                Subscribe(200, 300)
            );
        }

        [TestMethod]
        public void TimeInterval_Never()
        {
            var scheduler = new TestScheduler();

            var xs = scheduler.CreateHotObservable(
                OnNext(150, 1)
            );

            var res = scheduler.Start(() =>
                xs.TimeInterval(scheduler.DisableOptimizations())
            );

            res.Messages.AssertEqual(
            );

            xs.Subscriptions.AssertEqual(
                Subscribe(200, 1000)
            );
        }

        [TestMethod]
        public void TimeInterval_DefaultScheduler()
        {
            Assert.IsTrue(Observable.Return(1).TimeInterval().Count().First() == 1);
        }

        [TestMethod]
        public void TimeInterval_WithStopwatch_Regular()
        {
            var scheduler = new TestScheduler();

            var xs = scheduler.CreateHotObservable(
                OnNext(150, 1),
                OnNext(210, 2),
                OnNext(230, 3),
                OnNext(260, 4),
                OnNext(300, 5),
                OnNext(350, 6),
                OnCompleted<int>(400)
            );

            var res = scheduler.Start(() =>
                xs.TimeInterval(scheduler)
            );

            res.Messages.AssertEqual(
                OnNext(210, new TimeInterval<int>(2, TimeSpan.FromTicks(10))),
                OnNext(230, new TimeInterval<int>(3, TimeSpan.FromTicks(20))),
                OnNext(260, new TimeInterval<int>(4, TimeSpan.FromTicks(30))),
                OnNext(300, new TimeInterval<int>(5, TimeSpan.FromTicks(40))),
                OnNext(350, new TimeInterval<int>(6, TimeSpan.FromTicks(50))),
                OnCompleted<TimeInterval<int>>(400)
            );

            xs.Subscriptions.AssertEqual(
                Subscribe(200, 400)
            );
        }

        [TestMethod]
        public void TimeInterval_WithStopwatch_Empty()
        {
            var scheduler = new TestScheduler();

            var xs = scheduler.CreateHotObservable(
                OnNext(150, 1),
                OnCompleted<int>(300)
            );

            var res = scheduler.Start(() =>
                xs.TimeInterval(scheduler)
            );

            res.Messages.AssertEqual(
                OnCompleted<TimeInterval<int>>(300)
            );

            xs.Subscriptions.AssertEqual(
                Subscribe(200, 300)
            );
        }

        [TestMethod]
        public void TimeInterval_WithStopwatch_Error()
        {
            var scheduler = new TestScheduler();

            var ex = new Exception();

            var xs = scheduler.CreateHotObservable(
                OnNext(150, 1),
                OnError<int>(300, ex)
            );

            var res = scheduler.Start(() =>
                xs.TimeInterval(scheduler)
            );

            res.Messages.AssertEqual(
                OnError<TimeInterval<int>>(300, ex)
            );

            xs.Subscriptions.AssertEqual(
                Subscribe(200, 300)
            );
        }

        [TestMethod]
        public void TimeInterval_WithStopwatch_Never()
        {
            var scheduler = new TestScheduler();

            var xs = scheduler.CreateHotObservable(
                OnNext(150, 1)
            );

            var res = scheduler.Start(() =>
                xs.TimeInterval(scheduler)
            );

            res.Messages.AssertEqual(
            );

            xs.Subscriptions.AssertEqual(
                Subscribe(200, 1000)
            );
        }

        #endregion

        #region + Timeout +

        [TestMethod]
        public void Timeout_ArgumentChecking()
        {
            var scheduler = new TestScheduler();
            var someObservable = Observable.Empty<int>();

            ReactiveAssert.Throws<ArgumentNullException>(() => Observable.Timeout(default(IObservable<int>), TimeSpan.Zero));
            ReactiveAssert.Throws<ArgumentNullException>(() => Observable.Timeout(default(IObservable<int>), TimeSpan.Zero, someObservable));
            ReactiveAssert.Throws<ArgumentNullException>(() => Observable.Timeout(someObservable, TimeSpan.Zero, default(IObservable<int>)));
            ReactiveAssert.Throws<ArgumentNullException>(() => Observable.Timeout(default(IObservable<int>), new DateTimeOffset()));
            ReactiveAssert.Throws<ArgumentNullException>(() => Observable.Timeout(default(IObservable<int>), new DateTimeOffset(), someObservable));
            ReactiveAssert.Throws<ArgumentNullException>(() => Observable.Timeout(someObservable, new DateTimeOffset(), default(IObservable<int>)));
            ReactiveAssert.Throws<ArgumentNullException>(() => Observable.Timeout(default(IObservable<int>), TimeSpan.Zero, scheduler));
            ReactiveAssert.Throws<ArgumentNullException>(() => Observable.Timeout(someObservable, TimeSpan.Zero, default(IScheduler)));
            ReactiveAssert.Throws<ArgumentNullException>(() => Observable.Timeout(default(IObservable<int>), TimeSpan.Zero, someObservable, scheduler));
            ReactiveAssert.Throws<ArgumentNullException>(() => Observable.Timeout(someObservable, TimeSpan.Zero, someObservable, null));
            ReactiveAssert.Throws<ArgumentNullException>(() => Observable.Timeout(someObservable, TimeSpan.Zero, default(IObservable<int>), scheduler));
            ReactiveAssert.Throws<ArgumentNullException>(() => Observable.Timeout(default(IObservable<int>), new DateTimeOffset(), scheduler));
            ReactiveAssert.Throws<ArgumentNullException>(() => Observable.Timeout(someObservable, new DateTimeOffset(), default(IScheduler)));
            ReactiveAssert.Throws<ArgumentNullException>(() => Observable.Timeout(default(IObservable<int>), new DateTimeOffset(), someObservable, scheduler));
            ReactiveAssert.Throws<ArgumentNullException>(() => Observable.Timeout(someObservable, new DateTimeOffset(), someObservable, null));
            ReactiveAssert.Throws<ArgumentNullException>(() => Observable.Timeout(someObservable, new DateTimeOffset(), default(IObservable<int>), scheduler));

            ReactiveAssert.Throws<ArgumentOutOfRangeException>(() => Observable.Timeout(someObservable, TimeSpan.FromSeconds(-1)));
            ReactiveAssert.Throws<ArgumentOutOfRangeException>(() => Observable.Timeout(someObservable, TimeSpan.FromSeconds(-1), scheduler));
            ReactiveAssert.Throws<ArgumentOutOfRangeException>(() => Observable.Timeout(someObservable, TimeSpan.FromSeconds(-1), someObservable));
            ReactiveAssert.Throws<ArgumentOutOfRangeException>(() => Observable.Timeout(someObservable, TimeSpan.FromSeconds(-1), someObservable, scheduler));
        }

        [TestMethod]
        public void Timeout_InTime()
        {
            var scheduler = new TestScheduler();

            var xs = scheduler.CreateHotObservable(
                OnNext(150, 1),
                OnNext(210, 2),
                OnNext(230, 3),
                OnNext(260, 4),
                OnNext(300, 5),
                OnNext(350, 6),
                OnCompleted<int>(400)
            );

            var res = scheduler.Start(() =>
                xs.Timeout(TimeSpan.FromTicks(500), scheduler)
            );

            res.Messages.AssertEqual(
                OnNext(210, 2),
                OnNext(230, 3),
                OnNext(260, 4),
                OnNext(300, 5),
                OnNext(350, 6),
                OnCompleted<int>(400)
            );

            xs.Subscriptions.AssertEqual(
                Subscribe(200, 400)
            );
        }

        [TestMethod]
        public void Timeout_DateTimeOffset_TimeoutOccurs_WithDefaultException()
        {
            var scheduler = new TestScheduler();

            var xs = scheduler.CreateHotObservable(
                OnNext(410, 1)
            );

            var res = scheduler.Start(() =>
                xs.Timeout(new DateTimeOffset(new DateTime(400), TimeSpan.Zero), scheduler)
            );

            res.Messages.AssertEqual(
                OnError<int>(400, ex => ex is TimeoutException)
            );

            xs.Subscriptions.AssertEqual(
                Subscribe(200, 400)
            );
        }

        [TestMethod]
        public void Timeout_TimeSpan_TimeoutOccurs_WithDefaultException()
        {
            var scheduler = new TestScheduler();

            var xs = scheduler.CreateHotObservable(
                OnNext(410, 1)
            );

            var res = scheduler.Start(() =>
                xs.Timeout(TimeSpan.FromTicks(200), scheduler)
            );

            res.Messages.AssertEqual(
                OnError<int>(400, ex => ex is TimeoutException)
            );

            xs.Subscriptions.AssertEqual(
                Subscribe(200, 400)
            );
        }

        [TestMethod]
        public void Timeout_TimeSpan_DefaultScheduler()
        {
            Assert.IsTrue(Observable.Return(1).Timeout(TimeSpan.FromSeconds(10)).ToEnumerable().Single() == 1);
        }

        [TestMethod]
        public void Timeout_TimeSpan_Observable_DefaultScheduler()
        {
            Assert.IsTrue(Observable.Return(1).Timeout(TimeSpan.FromSeconds(10), Observable.Return(2)).ToEnumerable().Single() == 1);
        }

        [TestMethod]
        public void Timeout_DateTimeOffset_DefaultScheduler()
        {
            Assert.IsTrue(Observable.Return(1).Timeout(DateTimeOffset.UtcNow + TimeSpan.FromSeconds(10)).ToEnumerable().Single() == 1);
        }

        [TestMethod]
        public void Timeout_DateTimeOffset_Observable_DefaultScheduler()
        {
            Assert.IsTrue(Observable.Return(1).Timeout(DateTimeOffset.UtcNow + TimeSpan.FromSeconds(10), Observable.Return(2)).ToEnumerable().Single() == 1);
        }

        [TestMethod]
        public void Timeout_TimeoutOccurs_1()
        {
            var scheduler = new TestScheduler();

            var xs = scheduler.CreateHotObservable(
                OnNext(70, 1),
                OnNext(130, 2),
                OnNext(310, 3),
                OnNext(400, 4),
                OnCompleted<int>(500)
            );

            var ys = scheduler.CreateColdObservable(
                OnNext(50, -1),
                OnNext(200, -2),
                OnNext(310, -3),
                OnCompleted<int>(320)
            );

            var res = scheduler.Start(() =>
                xs.Timeout(TimeSpan.FromTicks(100), ys, scheduler)
            );

            res.Messages.AssertEqual(
                OnNext(350, -1),
                OnNext(500, -2),
                OnNext(610, -3),
                OnCompleted<int>(620)
            );

            xs.Subscriptions.AssertEqual(
                Subscribe(200, 300)
            );

            ys.Subscriptions.AssertEqual(
                Subscribe(300, 620)
            );
        }

        [TestMethod]
        public void Timeout_TimeoutOccurs_2()
        {
            var scheduler = new TestScheduler();

            var xs = scheduler.CreateHotObservable(
                OnNext(70, 1),
                OnNext(130, 2),
                OnNext(240, 3),
                OnNext(310, 4),
                OnNext(430, 5),
                OnCompleted<int>(500)
            );

            var ys = scheduler.CreateColdObservable(
                OnNext(50, -1),
                OnNext(200, -2),
                OnNext(310, -3),
                OnCompleted<int>(320)
            );

            var res = scheduler.Start(() =>
                xs.Timeout(TimeSpan.FromTicks(100), ys, scheduler)
            );

            res.Messages.AssertEqual(
                OnNext(240, 3),
                OnNext(310, 4),
                OnNext(460, -1),
                OnNext(610, -2),
                OnNext(720, -3),
                OnCompleted<int>(730)
            );

            xs.Subscriptions.AssertEqual(
                Subscribe(200, 410)
            );

            ys.Subscriptions.AssertEqual(
                Subscribe(410, 730)
            );
        }

        [TestMethod]
        public void Timeout_TimeoutOccurs_Never()
        {
            var scheduler = new TestScheduler();

            var xs = scheduler.CreateHotObservable(
                OnNext(70, 1),
                OnNext(130, 2),
                OnNext(240, 3),
                OnNext(310, 4),
                OnNext(430, 5),
                OnCompleted<int>(500)
            );

            var ys = scheduler.CreateColdObservable<int>(
            );

            var res = scheduler.Start(() =>
                xs.Timeout(TimeSpan.FromTicks(100), ys, scheduler)
            );

            res.Messages.AssertEqual(
                OnNext(240, 3),
                OnNext(310, 4)
            );

            xs.Subscriptions.AssertEqual(
                Subscribe(200, 410)
            );

            ys.Subscriptions.AssertEqual(
                Subscribe(410, 1000)
            );
        }

        [TestMethod]
        public void Timeout_TimeoutOccurs_Completed()
        {
            var scheduler = new TestScheduler();

            var xs = scheduler.CreateHotObservable(
                OnCompleted<int>(500)
            );

            var ys = scheduler.CreateColdObservable(
                OnNext(100, -1)
            );

            var res = scheduler.Start(() =>
                xs.Timeout(TimeSpan.FromTicks(100), ys, scheduler)
            );

            res.Messages.AssertEqual(
                OnNext(400, -1)
            );

            xs.Subscriptions.AssertEqual(
                Subscribe(200, 300)
            );

            ys.Subscriptions.AssertEqual(
                Subscribe(300, 1000)
            );
        }

        [TestMethod]
        public void Timeout_TimeoutOccurs_Error()
        {
            var scheduler = new TestScheduler();

            var xs = scheduler.CreateHotObservable(
                OnError<int>(500, new Exception())
            );

            var ys = scheduler.CreateColdObservable(
                OnNext(100, -1)
            );

            var res = scheduler.Start(() =>
                xs.Timeout(TimeSpan.FromTicks(100), ys, scheduler)
            );

            res.Messages.AssertEqual(
                OnNext(400, -1)
            );

            xs.Subscriptions.AssertEqual(
                Subscribe(200, 300)
            );

            ys.Subscriptions.AssertEqual(
                Subscribe(300, 1000)
            );
        }

        [TestMethod]
        public void Timeout_TimeoutOccurs_NextIsError()
        {
            var ex = new Exception();

            var scheduler = new TestScheduler();

            var xs = scheduler.CreateHotObservable(
                OnNext<int>(500, 42)
            );

            var ys = scheduler.CreateColdObservable(
                OnError<int>(100, ex)
            );

            var res = scheduler.Start(() =>
                xs.Timeout(TimeSpan.FromTicks(100), ys, scheduler)
            );

            res.Messages.AssertEqual(
                OnError<int>(400, ex)
            );

            xs.Subscriptions.AssertEqual(
                Subscribe(200, 300)
            );

            ys.Subscriptions.AssertEqual(
                Subscribe(300, 400)
            );
        }

        [TestMethod]
        public void Timeout_TimeoutNotOccurs_Completed()
        {
            var scheduler = new TestScheduler();

            var xs = scheduler.CreateHotObservable(
                OnCompleted<int>(250)
            );

            var ys = scheduler.CreateColdObservable(
                OnNext(100, -1)
            );

            var res = scheduler.Start(() =>
                xs.Timeout(TimeSpan.FromTicks(100), ys, scheduler)
            );

            res.Messages.AssertEqual(
                OnCompleted<int>(250)
            );

            xs.Subscriptions.AssertEqual(
                Subscribe(200, 250)
            );

            ys.Subscriptions.AssertEqual(
            );
        }

        [TestMethod]
        public void Timeout_TimeoutNotOccurs_Error()
        {
            var scheduler = new TestScheduler();

            var ex = new Exception();

            var xs = scheduler.CreateHotObservable(
                OnError<int>(250, ex)
            );

            var ys = scheduler.CreateColdObservable(
                OnNext(100, -1)
            );

            var res = scheduler.Start(() =>
                xs.Timeout(TimeSpan.FromTicks(100), ys, scheduler)
            );

            res.Messages.AssertEqual(
                OnError<int>(250, ex)
            );

            xs.Subscriptions.AssertEqual(
                Subscribe(200, 250)
            );

            ys.Subscriptions.AssertEqual(
            );
        }

        [TestMethod]
        public void Timeout_TimeoutDoesNotOccur()
        {
            var scheduler = new TestScheduler();

            var xs = scheduler.CreateHotObservable(
                OnNext(70, 1),
                OnNext(130, 2),
                OnNext(240, 3),
                OnNext(320, 4),
                OnNext(410, 5),
                OnCompleted<int>(500)
            );

            var ys = scheduler.CreateColdObservable(
                OnNext(50, -1),
                OnNext(200, -2),
                OnNext(310, -3),
                OnCompleted<int>(320)
            );

            var res = scheduler.Start(() =>
                xs.Timeout(TimeSpan.FromTicks(100), ys, scheduler)
            );

            res.Messages.AssertEqual(
                OnNext(240, 3),
                OnNext(320, 4),
                OnNext(410, 5),
                OnCompleted<int>(500)
            );

            xs.Subscriptions.AssertEqual(
                Subscribe(200, 500)
            );

            ys.Subscriptions.AssertEqual(
            );
        }

        [TestMethod]
        public void Timeout_DateTimeOffset_TimeoutOccurs()
        {
            var scheduler = new TestScheduler();

            var xs = scheduler.CreateHotObservable(
                OnNext(410, 1)
            );

            var ys = scheduler.CreateColdObservable(
                OnNext(100, -1)
            );

            var res = scheduler.Start(() =>
                xs.Timeout(new DateTimeOffset(new DateTime(400), TimeSpan.Zero), ys, scheduler)
            );

            res.Messages.AssertEqual(
                OnNext(500, -1)
            );

            xs.Subscriptions.AssertEqual(
                Subscribe(200, 400)
            );

            ys.Subscriptions.AssertEqual(
                Subscribe(400, 1000)
            );
        }

        [TestMethod]
        public void Timeout_DateTimeOffset_TimeoutDoesNotOccur_Completed()
        {
            var scheduler = new TestScheduler();

            var xs = scheduler.CreateHotObservable(
                OnNext(310, 1),
                OnCompleted<int>(390)
            );

            var ys = scheduler.CreateColdObservable(
                OnNext(100, -1)
            );

            var res = scheduler.Start(() =>
                xs.Timeout(new DateTimeOffset(new DateTime(400), TimeSpan.Zero), ys, scheduler)
            );

            res.Messages.AssertEqual(
                OnNext(310, 1),
                OnCompleted<int>(390)
            );

            xs.Subscriptions.AssertEqual(
                Subscribe(200, 390)
            );

            ys.Subscriptions.AssertEqual(
            );
        }

        [TestMethod]
        public void Timeout_DateTimeOffset_TimeoutDoesNotOccur_Error()
        {
            var scheduler = new TestScheduler();

            var ex = new Exception();

            var xs = scheduler.CreateHotObservable(
                OnNext(310, 1),
                OnError<int>(390, ex)
            );

            var ys = scheduler.CreateColdObservable(
                OnNext(100, -1)
            );

            var res = scheduler.Start(() =>
                xs.Timeout(new DateTimeOffset(new DateTime(400), TimeSpan.Zero), ys, scheduler)
            );

            res.Messages.AssertEqual(
                OnNext(310, 1),
                OnError<int>(390, ex)
            );

            xs.Subscriptions.AssertEqual(
                Subscribe(200, 390)
            );

            ys.Subscriptions.AssertEqual(
            );
        }

        [TestMethod]
        public void Timeout_DateTimeOffset_TimeoutOccur_2()
        {
            var scheduler = new TestScheduler();

            var xs = scheduler.CreateHotObservable(
                OnNext(310, 1),
                OnNext(350, 2),
                OnNext(420, 3),
                OnCompleted<int>(450)
            );

            var ys = scheduler.CreateColdObservable(
                OnNext(100, -1)
            );

            var res = scheduler.Start(() =>
                xs.Timeout(new DateTimeOffset(new DateTime(400), TimeSpan.Zero), ys, scheduler)
            );

            res.Messages.AssertEqual(
                OnNext(310, 1),
                OnNext(350, 2),
                OnNext(500, -1)
            );

            xs.Subscriptions.AssertEqual(
                Subscribe(200, 400)
            );

            ys.Subscriptions.AssertEqual(
                Subscribe(400, 1000)
            );
        }

        [TestMethod]
        public void Timeout_DateTimeOffset_TimeoutOccur_3()
        {
            var scheduler = new TestScheduler();

            var xs = scheduler.CreateHotObservable(
                OnNext(310, 1),
                OnNext(350, 2),
                OnNext(420, 3),
                OnCompleted<int>(450)
            );

            var ys = scheduler.CreateColdObservable<int>(
            );

            var res = scheduler.Start(() =>
                xs.Timeout(new DateTimeOffset(new DateTime(400), TimeSpan.Zero), ys, scheduler)
            );

            res.Messages.AssertEqual(
                OnNext(310, 1),
                OnNext(350, 2)
            );

            xs.Subscriptions.AssertEqual(
                Subscribe(200, 400)
            );

            ys.Subscriptions.AssertEqual(
                Subscribe(400, 1000)
            );
        }

        [TestMethod]
        public void Timeout_Duration_ArgumentChecking()
        {
            var someObservable = Observable.Empty<int>();

            ReactiveAssert.Throws<ArgumentNullException>(() => Observable.Timeout(default(IObservable<int>), someObservable, x => someObservable, someObservable));
            ReactiveAssert.Throws<ArgumentNullException>(() => Observable.Timeout(someObservable, default(IObservable<int>), x => someObservable, someObservable));
            ReactiveAssert.Throws<ArgumentNullException>(() => Observable.Timeout(someObservable, someObservable, default(Func<int, IObservable<int>>), someObservable));
            ReactiveAssert.Throws<ArgumentNullException>(() => Observable.Timeout(someObservable, someObservable, x => someObservable, default(IObservable<int>)));

            ReactiveAssert.Throws<ArgumentNullException>(() => Observable.Timeout(default(IObservable<int>), someObservable, x => someObservable));
            ReactiveAssert.Throws<ArgumentNullException>(() => Observable.Timeout(someObservable, default(IObservable<int>), x => someObservable));
            ReactiveAssert.Throws<ArgumentNullException>(() => Observable.Timeout(someObservable, someObservable, default(Func<int, IObservable<int>>)));

            ReactiveAssert.Throws<ArgumentNullException>(() => Observable.Timeout(default(IObservable<int>), x => someObservable, someObservable));
            ReactiveAssert.Throws<ArgumentNullException>(() => Observable.Timeout(someObservable, default(Func<int, IObservable<int>>), someObservable));
            ReactiveAssert.Throws<ArgumentNullException>(() => Observable.Timeout(someObservable, x => someObservable, default(IObservable<int>)));

            ReactiveAssert.Throws<ArgumentNullException>(() => Observable.Timeout(default(IObservable<int>), x => someObservable));
            ReactiveAssert.Throws<ArgumentNullException>(() => Observable.Timeout(someObservable, default(Func<int, IObservable<int>>)));
        }

        [TestMethod]
        public void Timeout_Duration_Simple_Never()
        {
            var scheduler = new TestScheduler();

            var xs = scheduler.CreateHotObservable(
                OnNext(310, 1),
                OnNext(350, 2),
                OnNext(420, 3),
                OnCompleted<int>(450)
            );

            var ys = scheduler.CreateColdObservable<int>(
            );

            var res = scheduler.Start(() =>
                xs.Timeout(ys, _ => ys)
            );

            res.Messages.AssertEqual(
                OnNext(310, 1),
                OnNext(350, 2),
                OnNext(420, 3),
                OnCompleted<int>(450)
            );

            xs.Subscriptions.AssertEqual(
                Subscribe(200, 450)
            );

            ys.Subscriptions.AssertEqual(
                Subscribe(200, 310),
                Subscribe(310, 350),
                Subscribe(350, 420),
                Subscribe(420, 450)
            );
        }

        [TestMethod]
        public void Timeout_Duration_Simple_TimeoutFirst()
        {
            var scheduler = new TestScheduler();

            var xs = scheduler.CreateHotObservable(
                OnNext(310, 1),
                OnNext(350, 2),
                OnNext(420, 3),
                OnCompleted<int>(450)
            );

            var ys = scheduler.CreateColdObservable<string>(
                OnNext(100, "Boo!")
            );

            var zs = scheduler.CreateColdObservable<string>(
            );

            var res = scheduler.Start(() =>
                xs.Timeout(ys, _ => zs)
            );

            res.Messages.AssertEqual(
                OnError<int>(300, ex => ex is TimeoutException)
            );

            xs.Subscriptions.AssertEqual(
                Subscribe(200, 300)
            );

            ys.Subscriptions.AssertEqual(
                Subscribe(200, 300)
            );

            zs.Subscriptions.AssertEqual(
            );
        }

        [TestMethod]
        public void Timeout_Duration_Simple_TimeoutFirst_Other()
        {
            var scheduler = new TestScheduler();

            var xs = scheduler.CreateHotObservable(
                OnNext(310, 1),
                OnNext(350, 2),
                OnNext(420, 3),
                OnCompleted<int>(450)
            );

            var ys = scheduler.CreateColdObservable<string>(
                OnNext(100, "Boo!")
            );

            var zs = scheduler.CreateColdObservable<string>(
            );

            var ts = scheduler.CreateColdObservable<int>(
                OnNext(50, 42),
                OnCompleted<int>(70)
            );

            var res = scheduler.Start(() =>
                xs.Timeout(ys, _ => zs, ts)
            );

            res.Messages.AssertEqual(
                OnNext(350, 42),
                OnCompleted<int>(370)
            );

            xs.Subscriptions.AssertEqual(
                Subscribe(200, 300)
            );

            ys.Subscriptions.AssertEqual(
                Subscribe(200, 300)
            );

            zs.Subscriptions.AssertEqual(
            );

            ts.Subscriptions.AssertEqual(
                Subscribe(300, 370)
            );
        }

        [TestMethod]
        public void Timeout_Duration_Simple_TimeoutLater()
        {
            var scheduler = new TestScheduler();

            var xs = scheduler.CreateHotObservable(
                OnNext(310, 1),
                OnNext(350, 2),
                OnNext(420, 3),
                OnCompleted<int>(450)
            );

            var ys = scheduler.CreateColdObservable<string>(
            );

            var zs = scheduler.CreateColdObservable<string>(
                OnNext(50, "Boo!")
            );

            var res = scheduler.Start(() =>
                xs.Timeout(ys, _ => zs)
            );

            res.Messages.AssertEqual(
                OnNext<int>(310, 1),
                OnNext<int>(350, 2),
                OnError<int>(400, ex => ex is TimeoutException)
            );

            xs.Subscriptions.AssertEqual(
                Subscribe(200, 400)
            );

            ys.Subscriptions.AssertEqual(
                Subscribe(200, 310)
            );

            zs.Subscriptions.AssertEqual(
                Subscribe(310, 350),
                Subscribe(350, 400)
            );
        }

        [TestMethod]
        public void Timeout_Duration_Simple_TimeoutLater_Other()
        {
            var scheduler = new TestScheduler();

            var xs = scheduler.CreateHotObservable(
                OnNext(310, 1),
                OnNext(350, 2),
                OnNext(420, 3),
                OnCompleted<int>(450)
            );

            var ys = scheduler.CreateColdObservable<string>(
            );

            var zs = scheduler.CreateColdObservable<string>(
                OnNext(50, "Boo!")
            );

            var ts = scheduler.CreateColdObservable<int>(
                OnNext(50, 42),
                OnCompleted<int>(70)
            );

            var res = scheduler.Start(() =>
                xs.Timeout(ys, _ => zs, ts)
            );

            res.Messages.AssertEqual(
                OnNext(310, 1),
                OnNext(350, 2),
                OnNext(450, 42),
                OnCompleted<int>(470)
            );

            xs.Subscriptions.AssertEqual(
                Subscribe(200, 400)
            );

            ys.Subscriptions.AssertEqual(
                Subscribe(200, 310)
            );

            zs.Subscriptions.AssertEqual(
                Subscribe(310, 350),
                Subscribe(350, 400)
            );

            ts.Subscriptions.AssertEqual(
                Subscribe(400, 470)
            );
        }

        [TestMethod]
        public void Timeout_Duration_Simple_TimeoutLater_NoFirst()
        {
            var scheduler = new TestScheduler();

            var xs = scheduler.CreateHotObservable(
                OnNext(310, 1),
                OnNext(350, 2),
                OnNext(420, 3),
                OnCompleted<int>(450)
            );

            var zs = scheduler.CreateColdObservable<string>(
                OnNext(50, "Boo!")
            );

            var res = scheduler.Start(() =>
                xs.Timeout(_ => zs)
            );

            res.Messages.AssertEqual(
                OnNext<int>(310, 1),
                OnNext<int>(350, 2),
                OnError<int>(400, ex => ex is TimeoutException)
            );

            xs.Subscriptions.AssertEqual(
                Subscribe(200, 400)
            );

            zs.Subscriptions.AssertEqual(
                Subscribe(310, 350),
                Subscribe(350, 400)
            );
        }

        [TestMethod]
        public void Timeout_Duration_Simple_TimeoutLater_Other_NoFirst()
        {
            var scheduler = new TestScheduler();

            var xs = scheduler.CreateHotObservable(
                OnNext(310, 1),
                OnNext(350, 2),
                OnNext(420, 3),
                OnCompleted<int>(450)
            );

            var zs = scheduler.CreateColdObservable<string>(
                OnNext(50, "Boo!")
            );

            var ts = scheduler.CreateColdObservable<int>(
                OnNext(50, 42),
                OnCompleted<int>(70)
            );

            var res = scheduler.Start(() =>
                xs.Timeout(_ => zs, ts)
            );

            res.Messages.AssertEqual(
                OnNext(310, 1),
                OnNext(350, 2),
                OnNext(450, 42),
                OnCompleted<int>(470)
            );

            xs.Subscriptions.AssertEqual(
                Subscribe(200, 400)
            );

            zs.Subscriptions.AssertEqual(
                Subscribe(310, 350),
                Subscribe(350, 400)
            );

            ts.Subscriptions.AssertEqual(
                Subscribe(400, 470)
            );
        }

        [TestMethod]
        public void Timeout_Duration_Simple_TimeoutByCompletion()
        {
            var scheduler = new TestScheduler();

            var xs = scheduler.CreateHotObservable(
                OnNext(310, 1),
                OnNext(350, 2),
                OnNext(420, 3),
                OnCompleted<int>(450)
            );

            var ys = scheduler.CreateColdObservable<string>(
            );

            var zs = scheduler.CreateColdObservable<string>(
                OnCompleted<string>(50)
            );

            var res = scheduler.Start(() =>
                xs.Timeout(ys, _ => zs)
            );

            res.Messages.AssertEqual(
                OnNext<int>(310, 1),
                OnNext<int>(350, 2),
                OnError<int>(400, ex => ex is TimeoutException)
            );

            xs.Subscriptions.AssertEqual(
                Subscribe(200, 400)
            );

            ys.Subscriptions.AssertEqual(
                Subscribe(200, 310)
            );

            zs.Subscriptions.AssertEqual(
                Subscribe(310, 350),
                Subscribe(350, 400)
            );
        }

        [TestMethod]
        public void Timeout_Duration_Simple_SelectorThrows()
        {
            var scheduler = new TestScheduler();

            var xs = scheduler.CreateHotObservable(
                OnNext(310, 1),
                OnNext(350, 2),
                OnNext(420, 3),
                OnCompleted<int>(450)
            );

            var ys = scheduler.CreateColdObservable<string>(
            );

            var zs = scheduler.CreateColdObservable<string>(
            );

            var ex = new Exception();

            var res = scheduler.Start(() =>
                xs.Timeout(ys, x =>
                {
                    if (x < 3)
                        return zs;
                    else
                        throw ex;
                })
            );

            res.Messages.AssertEqual(
                OnNext(310, 1),
                OnNext(350, 2),
                OnNext(420, 3),
                OnError<int>(420, ex)
            );

            xs.Subscriptions.AssertEqual(
                Subscribe(200, 420)
            );

            ys.Subscriptions.AssertEqual(
                Subscribe(200, 310)
            );

            zs.Subscriptions.AssertEqual(
                Subscribe(310, 350),
                Subscribe(350, 420)
            );
        }

        [TestMethod]
        public void Timeout_Duration_Simple_InnerThrows()
        {
            var ex = new Exception();

            var scheduler = new TestScheduler();

            var xs = scheduler.CreateHotObservable(
                OnNext(310, 1),
                OnNext(350, 2),
                OnNext(420, 3),
                OnCompleted<int>(450)
            );

            var ys = scheduler.CreateColdObservable<string>(
            );

            var zs = scheduler.CreateColdObservable<string>(
                OnError<string>(50, ex)
            );

            var res = scheduler.Start(() =>
                xs.Timeout(ys, x => zs)
            );

            res.Messages.AssertEqual(
                OnNext(310, 1),
                OnNext(350, 2),
                OnError<int>(400, ex)
            );

            xs.Subscriptions.AssertEqual(
                Subscribe(200, 400)
            );

            ys.Subscriptions.AssertEqual(
                Subscribe(200, 310)
            );

            zs.Subscriptions.AssertEqual(
                Subscribe(310, 350),
                Subscribe(350, 400)
            );
        }

        [TestMethod]
        public void Timeout_Duration_Simple_FirstThrows()
        {
            var ex = new Exception();

            var scheduler = new TestScheduler();

            var xs = scheduler.CreateHotObservable(
                OnNext(310, 1),
                OnNext(350, 2),
                OnNext(420, 3),
                OnCompleted<int>(450)
            );

            var ys = scheduler.CreateColdObservable<string>(
                OnError<string>(50, ex)
            );

            var zs = scheduler.CreateColdObservable<string>(
            );

            var res = scheduler.Start(() =>
                xs.Timeout(ys, x => zs)
            );

            res.Messages.AssertEqual(
                OnError<int>(250, ex)
            );

            xs.Subscriptions.AssertEqual(
                Subscribe(200, 250)
            );

            ys.Subscriptions.AssertEqual(
                Subscribe(200, 250)
            );

            zs.Subscriptions.AssertEqual(
            );
        }

        [TestMethod]
        public void Timeout_Duration_Simple_SourceThrows()
        {
            var ex = new Exception();

            var scheduler = new TestScheduler();

            var xs = scheduler.CreateHotObservable(
                OnNext(310, 1),
                OnNext(350, 2),
                OnNext(420, 3),
                OnError<int>(450, ex)
            );

            var ys = scheduler.CreateColdObservable<string>(
            );

            var zs = scheduler.CreateColdObservable<string>(
            );

            var res = scheduler.Start(() =>
                xs.Timeout(ys, x => zs)
            );

            res.Messages.AssertEqual(
                OnNext(310, 1),
                OnNext(350, 2),
                OnNext(420, 3),
                OnError<int>(450, ex)
            );

            xs.Subscriptions.AssertEqual(
                Subscribe(200, 450)
            );

            ys.Subscriptions.AssertEqual(
                Subscribe(200, 310)
            );

            zs.Subscriptions.AssertEqual(
                Subscribe(310, 350),
                Subscribe(350, 420),
                Subscribe(420, 450)
            );
        }

        #endregion

        #region + Timer +

        [TestMethod]
        public void OneShotTimer_TimeSpan_ArgumentChecking()
        {
            ReactiveAssert.Throws<ArgumentNullException>(() => Observable.Timer(TimeSpan.Zero, null));
            ReactiveAssert.Throws<ArgumentNullException>(() => Observable.Timer(TimeSpan.Zero, DummyScheduler.Instance).Subscribe(null));
            ReactiveAssert.Throws<ArgumentNullException>(() => Observable.Timer(DateTimeOffset.Now, null));
            ReactiveAssert.Throws<ArgumentNullException>(() => Observable.Timer(TimeSpan.Zero, TimeSpan.Zero, null));
            ReactiveAssert.Throws<ArgumentNullException>(() => Observable.Timer(DateTimeOffset.Now, TimeSpan.Zero, null));

            ReactiveAssert.Throws<ArgumentOutOfRangeException>(() => Observable.Timer(TimeSpan.Zero, TimeSpan.FromSeconds(-1)));
            ReactiveAssert.Throws<ArgumentOutOfRangeException>(() => Observable.Timer(TimeSpan.Zero, TimeSpan.FromSeconds(-1), DummyScheduler.Instance));
            ReactiveAssert.Throws<ArgumentOutOfRangeException>(() => Observable.Timer(DateTimeOffset.Now, TimeSpan.FromSeconds(-1)));
            ReactiveAssert.Throws<ArgumentOutOfRangeException>(() => Observable.Timer(DateTimeOffset.Now, TimeSpan.FromSeconds(-1), DummyScheduler.Instance));
        }

        [TestMethod]
        public void OneShotTimer_TimeSpan_Basic()
        {
            var scheduler = new TestScheduler();

            var res = scheduler.Start(() =>
                Observable.Timer(TimeSpan.FromTicks(300), scheduler)
            );

            res.Messages.AssertEqual(
                OnNext(500, 0L),
                OnCompleted<long>(500)
            );
        }

        [TestMethod]
        public void OneShotTimer_TimeSpan_Zero()
        {
            var scheduler = new TestScheduler();

            var res = scheduler.Start(() =>
                Observable.Timer(TimeSpan.FromTicks(0), scheduler)
            );

            res.Messages.AssertEqual(
                OnNext(201, 0L),
                OnCompleted<long>(201)
            );
        }

        [TestMethod]
        public void OneShotTimer_TimeSpan_Zero_DefaultScheduler()
        {
            var scheduler = new TestScheduler();
            var observer = scheduler.CreateObserver<long>();
            var completed = new ManualResetEventSlim();

            Observable.Timer(TimeSpan.Zero).Subscribe(observer.OnNext, completed.Set);

            completed.Wait();
            
            Assert.AreEqual(1, observer.Messages.Count);
        }

        [TestMethod]
        public void OneShotTimer_TimeSpan_Negative()
        {
            var scheduler = new TestScheduler();

            var res = scheduler.Start(() =>
                Observable.Timer(TimeSpan.FromTicks(-1), scheduler)
            );

            res.Messages.AssertEqual(
                OnNext(201, 0L),
                OnCompleted<long>(201)
            );
        }

        [TestMethod]
        public void OneShotTimer_TimeSpan_Disposed()
        {
            var scheduler = new TestScheduler();

            var res = scheduler.Start(() =>
                Observable.Timer(TimeSpan.FromTicks(1000), scheduler)
            );

            res.Messages.AssertEqual(
            );
        }

        [TestMethod]
        public void OneShotTimer_TimeSpan_ObserverThrows()
        {
            var scheduler1 = new TestScheduler();

            var xs = Observable.Timer(TimeSpan.FromTicks(1), scheduler1);

            xs.Subscribe(x => { throw new InvalidOperationException(); });

            ReactiveAssert.Throws<InvalidOperationException>(() => scheduler1.Start());

            var scheduler2 = new TestScheduler();

            var ys = Observable.Timer(TimeSpan.FromTicks(1), scheduler2);

            ys.Subscribe(x => { }, ex => { }, () => { throw new InvalidOperationException(); });

            ReactiveAssert.Throws<InvalidOperationException>(() => scheduler2.Start());
        }

        [TestMethod]
        public void OneShotTimer_TimeSpan_DefaultScheduler()
        {
            Assert.IsTrue(Observable.Timer(TimeSpan.FromMilliseconds(1)).ToEnumerable().SequenceEqual(new[] { 0L }));
        }

        [TestMethod]
        public void OneShotTimer_DateTimeOffset_DefaultScheduler()
        {
            Assert.IsTrue(Observable.Timer(DateTimeOffset.UtcNow + TimeSpan.FromSeconds(1)).ToEnumerable().SequenceEqual(new[] { 0L }));
        }

        [TestMethod]
        public void OneShotTimer_TimeSpan_TimeSpan_DefaultScheduler()
        {
            Assert.IsTrue(Observable.Timer(TimeSpan.FromMilliseconds(1), TimeSpan.FromMilliseconds(1)).ToEnumerable().Take(2).SequenceEqual(new[] { 0L, 1L }));
        }

        [TestMethod]
        public void OneShotTimer_DateTimeOffset_TimeSpan_DefaultScheduler()
        {
            Assert.IsTrue(Observable.Timer(DateTimeOffset.UtcNow + TimeSpan.FromSeconds(1), TimeSpan.FromMilliseconds(1)).ToEnumerable().Take(2).SequenceEqual(new[] { 0L, 1L }));
        }

        [TestMethod]
        public void OneShotTimer_DateTimeOffset_Basic()
        {
            var scheduler = new TestScheduler();

            var res = scheduler.Start(() =>
                Observable.Timer(new DateTimeOffset(500, TimeSpan.Zero), scheduler)
            );

            res.Messages.AssertEqual(
                OnNext(500, 0L),
                OnCompleted<long>(500)
            );
        }

        [TestMethod]
        public void OneShotTimer_DateTimeOffset_Zero()
        {
            var scheduler = new TestScheduler();

            var res = scheduler.Start(() =>
                Observable.Timer(new DateTimeOffset(200, TimeSpan.Zero), scheduler)
            );

            res.Messages.AssertEqual(
                OnNext(201, 0L),
                OnCompleted<long>(201)
            );
        }

        [TestMethod]
        public void OneShotTimer_DateTimeOffset_Past()
        {
            var scheduler = new TestScheduler();

            var res = scheduler.Start(() =>
                Observable.Timer(new DateTimeOffset(0, TimeSpan.Zero), scheduler)
            );

            res.Messages.AssertEqual(
                OnNext(201, 0L),
                OnCompleted<long>(201)
            );
        }

        [TestMethod]
        public void RepeatingTimer_TimeSpan_Zero_DefaultScheduler()
        {
            var scheduler = new TestScheduler();
            var observer = scheduler.CreateObserver<long>();
            var completed = new ManualResetEventSlim();

            Observable.Timer(TimeSpan.Zero, TimeSpan.Zero).TakeWhile(i => i < 10).Subscribe(observer.OnNext, completed.Set);

            completed.Wait();

            Assert.AreEqual(10, observer.Messages.Count);
        }

        [TestMethod]
        public void RepeatingTimer_DateTimeOffset_TimeSpan_Simple()
        {
            var scheduler = new TestScheduler();

            var res = scheduler.Start(() =>
                Observable.Timer(new DateTimeOffset(300, TimeSpan.Zero), TimeSpan.FromTicks(100), scheduler),
                0, 200, 750
            );

            res.Messages.AssertEqual(
                OnNext(300, 0L),
                OnNext(400, 1L),
                OnNext(500, 2L),
                OnNext(600, 3L),
                OnNext(700, 4L)
            );
        }

        [TestMethod]
        public void RepeatingTimer_TimeSpan_TimeSpan_Simple()
        {
            var scheduler = new TestScheduler();

            var res = scheduler.Start(() =>
                Observable.Timer(TimeSpan.FromTicks(100), TimeSpan.FromTicks(100), scheduler),
                0, 200, 750
            );

            res.Messages.AssertEqual(
                OnNext(300, 0L),
                OnNext(400, 1L),
                OnNext(500, 2L),
                OnNext(600, 3L),
                OnNext(700, 4L)
            );
        }

        [TestMethod]
        public void RepeatingTimer_Periodic1()
        {
            var scheduler = new PeriodicTestScheduler();

            var res = scheduler.Start(() =>
                Observable.Timer(TimeSpan.FromTicks(50), TimeSpan.FromTicks(100), scheduler),
                0, 200, 700
            );

            res.Messages.AssertEqual(
                OnNext(250, 0L),
                OnNext(350, 1L),
                OnNext(450, 2L),
                OnNext(550, 3L),
                OnNext(650, 4L)
            );

            scheduler.Timers.AssertEqual(
                new TimerRun(250, 700) { 350, 450, 550, 650 }
            );
        }

        [TestMethod]
        public void RepeatingTimer_Periodic2()
        {
            var scheduler = new PeriodicTestScheduler();

            var res = scheduler.Start(() =>
                Observable.Timer(TimeSpan.FromTicks(100), TimeSpan.FromTicks(100), scheduler),
                0, 200, 750
            );

            res.Messages.AssertEqual(
                OnNext(300, 0L),
                OnNext(400, 1L),
                OnNext(500, 2L),
                OnNext(600, 3L),
                OnNext(700, 4L)
            );

            scheduler.Timers.AssertEqual(
                new TimerRun(200, 750) { 300, 400, 500, 600, 700 }
            );
        }

        [TestMethod]
        public void RepeatingTimer_UsingStopwatch_Slippage1()
        {
            var scheduler = new TestScheduler();

            var xs = default(IObservable<long>);
            scheduler.ScheduleAbsolute(100, () => { xs = Observable.Timer(TimeSpan.Zero, TimeSpan.FromTicks(100), scheduler); });

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
            scheduler.ScheduleAbsolute(200, () => { d = xs.Subscribe(onNext); });

            scheduler.ScheduleAbsolute(1000, () => { d.Dispose(); });

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
            scheduler.ScheduleAbsolute(100, () => { xs = Observable.Timer(TimeSpan.FromTicks(100), TimeSpan.FromTicks(100), scheduler); });

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
            scheduler.ScheduleAbsolute(200, () => { d = xs.Subscribe(onNext); });

            scheduler.ScheduleAbsolute(1000, () => { d.Dispose(); });

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
            scheduler.ScheduleAbsolute(100, () => { xs = Observable.Timer(TimeSpan.Zero, TimeSpan.FromTicks(100), scheduler); });

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
            scheduler.ScheduleAbsolute(200, () => { d = xs.Subscribe(onNext); });

            scheduler.ScheduleAbsolute(1000, () => { d.Dispose(); });

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
            scheduler.ScheduleAbsolute(100, () => { xs = Observable.Timer(TimeSpan.Zero, TimeSpan.FromTicks(100), scheduler); });

            var onNext = new Action<long>(x =>
            {
                if (x == 0)
                    throw ex;
            });

            var d = default(IDisposable);
            scheduler.ScheduleAbsolute(200, () => { d = xs.Subscribe(onNext); });

            scheduler.ScheduleAbsolute(1000, () => { d.Dispose(); });

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
            scheduler.ScheduleAbsolute(100, () => { xs = Observable.Timer(TimeSpan.Zero, TimeSpan.FromTicks(100), scheduler); });

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
                    throw ex;
            });

            var d = default(IDisposable);
            scheduler.ScheduleAbsolute(200, () => { d = xs.Subscribe(onNext); });

            scheduler.ScheduleAbsolute(1000, () => { d.Dispose(); });

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
            scheduler.ScheduleAbsolute(100, () => { xs = Observable.Timer(TimeSpan.Zero, TimeSpan.FromTicks(100), scheduler.DisableOptimizations(typeof(IStopwatchProvider))); });

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
            scheduler.ScheduleAbsolute(200, () => { d = xs.Subscribe(onNext); });

            scheduler.ScheduleAbsolute(1000, () => { d.Dispose(); });

            scheduler.Start();

            times.AssertEqual(
                201, // 1 off because of initial scheduling jump (InvokeStart)
                301,
                401,
                523, // 3 off because of 401 + 120 + 2 scheduling ticks (one due to yield in SchedulePeriodic emulation code)
                645, // 5 off because of 523 + 120 + 2 scheduling ticks (one due to yield in SchedulePeriodic emulation code)
                743, // \
                843, //  +--> 43 off because this situation (no stopwatch or periodic scheduling interface) only gets best effort treatment (see SchedulePeriodic emulation code)
                943  // /
            );
        }

        [TestMethod]
        public void RepeatingTimer_NoStopwatch_Slippage2()
        {
            var scheduler = new TestScheduler();

            var xs = default(IObservable<long>);
            scheduler.ScheduleAbsolute(100, () => { xs = Observable.Timer(TimeSpan.FromTicks(100), TimeSpan.FromTicks(100), scheduler.DisableOptimizations(typeof(IStopwatchProvider))); });

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
            scheduler.ScheduleAbsolute(200, () => { d = xs.Subscribe(onNext); });

            scheduler.ScheduleAbsolute(1000, () => { d.Dispose(); });

            scheduler.Start();

            times.AssertEqual(
                300,
                400,
                500,
                622, // 2 off because of 500 + 120 + 2 scheduling ticks (one due to yield in SchedulePeriodic emulation code)
                744, // 4 off because of 622 + 120 + 2 scheduling ticks (one due to yield in SchedulePeriodic emulation code)
                842, // |
                942  // +--> 42 off because this situation (no stopwatch or periodic scheduling interface) only gets best effort treatment (see SchedulePeriodic emulation code)
            );
        }

        [TestMethod]
        public void RepeatingTimer_Start_CatchUp()
        {
            var e = new ManualResetEvent(false);

            var xs = Observable.Timer(TimeSpan.Zero, TimeSpan.FromMilliseconds(10));

            var d = new SingleAssignmentDisposable();
            d.Disposable = xs.Subscribe(x =>
            {
                if (x == 0)
                    Thread.Sleep(500);

                if (x > 10)
                {
                    e.Set();
                    d.Dispose();
                }
            });

            e.WaitOne();
        }

        [TestMethod]
        public void RepeatingTimer_Start_CatchUp_Throws()
        {
            var end = new ManualResetEvent(false);

            var err = new Exception();
            var ex = default(Exception);

            var s = ThreadPoolScheduler.Instance.Catch<Exception>(e =>
            {
                Interlocked.Exchange(ref ex, e);
                end.Set();
                return true;
            });

            var xs = Observable.Timer(TimeSpan.Zero, TimeSpan.FromMilliseconds(10), s);

            xs.Subscribe(x =>
            {
                if (x == 0)
                    Thread.Sleep(500);

                if (x == 5)
                    throw err;
            });

            end.WaitOne();

            Assert.AreSame(err, ex);
        }

        class SchedulerWithCatch : IServiceProvider, IScheduler
        {
            private readonly IScheduler _scheduler;
            private readonly Action<Exception> _setException;

            public SchedulerWithCatch(IScheduler scheduler, Action<Exception> setException)
            {
                _scheduler = scheduler;
                _setException = setException;
            }

            public object GetService(Type serviceType)
            {
                return ((IServiceProvider)_scheduler).GetService(serviceType);
            }

            public DateTimeOffset Now
            {
                get { return _scheduler.Now; }
            }

            public IDisposable Schedule<TState>(TState state, Func<IScheduler, TState, IDisposable> action)
            {
                return _scheduler.Schedule<TState>(state, GetCatch(action));
            }

            public IDisposable Schedule<TState>(TState state, TimeSpan dueTime, Func<IScheduler, TState, IDisposable> action)
            {
                return _scheduler.Schedule<TState>(state, dueTime, GetCatch(action));
            }

            public IDisposable Schedule<TState>(TState state, DateTimeOffset dueTime, Func<IScheduler, TState, IDisposable> action)
            {
                return _scheduler.Schedule<TState>(state, dueTime, GetCatch(action));
            }

            private Func<IScheduler, TState, IDisposable> GetCatch<TState>(Func<IScheduler, TState, IDisposable> action)
            {
                return (self, s) =>
                {
                    try
                    {
                        return action(new SchedulerWithCatch(self, _setException), s);
                    }
                    catch (Exception ex)
                    {
                        _setException(ex);
                        return Disposable.Empty;
                    }
                };
            }
        }

        class PeriodicTestScheduler : TestScheduler, ISchedulerPeriodic, IServiceProvider
        {
            private readonly List<TimerRun> _timers;

            public PeriodicTestScheduler()
            {
                _timers = new List<TimerRun>();
            }

            public IDisposable SchedulePeriodic<TState>(TState state, TimeSpan period, Func<TState, TState> action)
            {
                var run = new TimerRun(this.Clock);
                _timers.Add(run);

                var x = state;

                var d = this.Schedule(period, self =>
                {
                    run.Add(this.Clock);

                    x = action(x);
                    self(period);
                });

                return new CompositeDisposable(
                    Disposable.Create(() => { run.Stop(this.Clock); }),
                    d
                );
            }

            public List<TimerRun> Timers
            {
                get { return _timers; }
            }

            protected override object GetService(Type serviceType)
            {
                if (serviceType == typeof(ISchedulerPeriodic))
                    return this as ISchedulerPeriodic;

                return null;
            }
        }

        class TimerRun : IEnumerable<long>
        {
            private readonly long _started;
            private long _stopped;
            private bool _hasStopped;
            private readonly List<long> _ticks;

            public TimerRun(long started)
            {
                _started = started;
                _ticks = new List<long>();
            }

            public TimerRun(long started, long stopped)
            {
                _started = started;
                _stopped = stopped;
                _hasStopped = true;
                _ticks = new List<long>();
            }

            public override int GetHashCode()
            {
                return 0;
            }

            public override bool Equals(object obj)
            {
                var other = obj as TimerRun;
                if (other == null)
                    return false;

                return _started == other._started && _stopped == other._stopped && _ticks.SequenceEqual(other._ticks);
            }

            public long Started
            {
                get { return _started; }
            }

            public IEnumerable<long> Ticks
            {
                get { return _ticks; }
            }

            public long Stopped
            {
                get { return _stopped; }
            }

            internal void Stop(long clock)
            {
                _stopped = clock;
                _hasStopped = true;
            }

            public override string ToString()
            {
                var sb = new StringBuilder();

                sb.Append("Start(" + _started + ") ");
                sb.Append("Ticks(" + string.Join(", ", _ticks.Select(t => t.ToString()).ToArray()) + ") ");
                if (_hasStopped)
                    sb.Append("Stop(" + _stopped + ")");

                return sb.ToString();
            }

            public IEnumerator<long> GetEnumerator()
            {
                return _ticks.GetEnumerator();
            }

            System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
            {
                return _ticks.GetEnumerator();
            }

            public void Add(long clock)
            {
                _ticks.Add(clock);
            }
        }

        #endregion

        #region + Timestamp +

        [TestMethod]
        public void Timestamp_ArgumentChecking()
        {
            var scheduler = new TestScheduler();
            var someObservable = Observable.Empty<int>();

            ReactiveAssert.Throws<ArgumentNullException>(() => Observable.Timestamp(default(IObservable<int>)));
            ReactiveAssert.Throws<ArgumentNullException>(() => Observable.Timestamp(default(IObservable<int>), scheduler));
            ReactiveAssert.Throws<ArgumentNullException>(() => Observable.Timestamp(someObservable, null));
        }

        [TestMethod]
        public void Timestamp_Regular()
        {
            var scheduler = new TestScheduler();

            var xs = scheduler.CreateHotObservable(
                OnNext(150, 1),
                OnNext(210, 2),
                OnNext(230, 3),
                OnNext(260, 4),
                OnNext(300, 5),
                OnNext(350, 6),
                OnCompleted<int>(400)
            );

            var res = scheduler.Start(() =>
                xs.Timestamp(scheduler)
            );

            res.Messages.AssertEqual(
                OnNext(210, new Timestamped<int>(2, new DateTimeOffset(210, TimeSpan.Zero))),
                OnNext(230, new Timestamped<int>(3, new DateTimeOffset(230, TimeSpan.Zero))),
                OnNext(260, new Timestamped<int>(4, new DateTimeOffset(260, TimeSpan.Zero))),
                OnNext(300, new Timestamped<int>(5, new DateTimeOffset(300, TimeSpan.Zero))),
                OnNext(350, new Timestamped<int>(6, new DateTimeOffset(350, TimeSpan.Zero))),
                OnCompleted<Timestamped<int>>(400)
            );

            xs.Subscriptions.AssertEqual(
                Subscribe(200, 400)
            );
        }

        [TestMethod]
        public void Timestamp_Empty()
        {
            var scheduler = new TestScheduler();

            var xs = scheduler.CreateHotObservable<int>(
                OnNext(150, 1),
                OnCompleted<int>(400)
            );

            var res = scheduler.Start(() =>
                xs.Timestamp(scheduler)
            );

            res.Messages.AssertEqual(
                OnCompleted<Timestamped<int>>(400)
            );

            xs.Subscriptions.AssertEqual(
                Subscribe(200, 400)
            );
        }

        [TestMethod]
        public void Timestamp_Error()
        {
            var scheduler = new TestScheduler();

            var ex = new Exception();

            var xs = scheduler.CreateHotObservable<int>(
                OnNext(150, 1),
                OnError<int>(400, ex)
            );

            var res = scheduler.Start(() =>
                xs.Timestamp(scheduler)
            );

            res.Messages.AssertEqual(
                OnError<Timestamped<int>>(400, ex)
            );

            xs.Subscriptions.AssertEqual(
                Subscribe(200, 400)
            );
        }

        [TestMethod]
        public void Timestamp_Never()
        {
            var scheduler = new TestScheduler();

            var xs = scheduler.CreateHotObservable<int>(
                OnNext(150, 1)
            );

            var res = scheduler.Start(() =>
                xs.Timestamp(scheduler)
            );

            res.Messages.AssertEqual(
            );

            xs.Subscriptions.AssertEqual(
                Subscribe(200, 1000)
            );
        }

        [TestMethod]
        public void Timestamp_DefaultScheduler()
        {
            Assert.IsTrue(Observable.Return(1).Timestamp().Count().First() == 1);
        }

        #endregion

        #region + Window +

        [TestMethod]
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

        [TestMethod]
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

            scheduler.Timers.AssertEqual(
                new TimerRun(200, 490) { 300, 400 }
            );
        }

        [TestMethod]
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

            scheduler.Timers.AssertEqual(
                new TimerRun(200, 460) { 300, 400 }
            );
        }

        [TestMethod]
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

        [TestMethod]
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

        [TestMethod]
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

        [TestMethod]
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

        [TestMethod]
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

        [TestMethod]
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

        [TestMethod]
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

        [TestMethod]
        public void WindowWithTime_Default()
        {
            Observable.Range(0, 10).Window(TimeSpan.FromDays(1), TimeSpan.FromDays(1)).SelectMany(Observable.ToList).First().AssertEqual(Enumerable.Range(0, 10));
            Observable.Range(0, 10).Window(TimeSpan.FromDays(1)).SelectMany(Observable.ToList).First().AssertEqual(Enumerable.Range(0, 10));
        }

        [TestMethod]
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

        [TestMethod]
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

        [TestMethod]
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

        [TestMethod]
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

        [TestMethod]
        public void WindowWithTimeOrCount_Default()
        {
            Observable.Range(1, 10).Window(TimeSpan.FromDays(1), 3).Skip(1).First().SequenceEqual(Observable.Range(4, 3));
        }

        #endregion
    }
}

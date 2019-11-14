// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the Apache 2.0 License.
// See the LICENSE file in the project root for more information. 

using System;
using System.Collections.Generic;
using System.Linq;
using System.Reactive;
using System.Reactive.Concurrency;
using System.Reactive.Disposables;
using System.Reactive.Linq;
using System.Reactive.Subjects;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Reactive.Testing;
using ReactiveTests.Dummies;
using Xunit;

namespace ReactiveTests.Tests
{
    public class DelayTest : ReactiveTest
    {

        [Fact]
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

        [Fact]
        public void Delay_TimeSpan_Simple1()
        {
            Delay_TimeSpan_Simple1_Impl(false);
        }

        [Fact]
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

        [Fact]
        public void Delay_DateTimeOffset_Simple1()
        {
            Delay_DateTimeOffset_Simple1_Impl(false);
        }

        [Fact]
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

        [Fact]
        public void Delay_TimeSpan_Simple2()
        {
            Delay_TimeSpan_Simple2_Impl(false);
        }

        [Fact]
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

        [Fact]
        public void Delay_DateTimeOffset_Simple2()
        {
            Delay_DateTimeOffset_Simple2_Impl(false);
        }

        [Fact]
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

        [Fact]
        public void Delay_TimeSpan_Simple3()
        {
            Delay_TimeSpan_Simple3_Impl(false);
        }

        [Fact]
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

        [Fact]
        public void Delay_DateTimeOffset_Simple3()
        {
            Delay_DateTimeOffset_Simple3_Impl(false);
        }

        [Fact]
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

        [Fact]
        public void Delay_TimeSpan_Error1()
        {
            Delay_TimeSpan_Error1_Impl(false);
        }

        [Fact]
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

        [Fact]
        public void Delay_DateTimeOffset_Error1()
        {
            Delay_DateTimeOffset_Error1_Impl(false);
        }

        [Fact]
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

        [Fact]
        public void Delay_TimeSpan_Error2()
        {
            Delay_TimeSpan_Error2_Impl(false);
        }

        [Fact]
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

        [Fact]
        public void Delay_DateTimeOffset_Error2()
        {
            Delay_DateTimeOffset_Error2_Impl(false);
        }

        [Fact]
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

#if !NO_THREAD
        [Fact]
        public void Delay_TimeSpan_Real_Simple1()
        {
            Delay_TimeSpan_Real_Simple1_Impl(ThreadPoolScheduler.Instance.DisableOptimizations());
        }

        [Fact]
        public void Delay_TimeSpan_Real_Simple1_Stopwatch()
        {
            Delay_TimeSpan_Real_Simple1_Impl(ThreadPoolScheduler.Instance);
        }
#endif

        private void Delay_TimeSpan_Real_Simple1_Impl(IScheduler scheduler)
        {
            var s = new Subject<int>();

            var res = s.Delay(TimeSpan.FromMilliseconds(10), scheduler);

            var lst = new List<int>();
            var e = new ManualResetEvent(false);
            res.Subscribe(lst.Add, () => e.Set());

            Task.Run(() =>
            {
                s.OnNext(1);
                s.OnNext(2);
                s.OnNext(3);
                s.OnCompleted();
            });

            e.WaitOne();
            Assert.True(new[] { 1, 2, 3 }.SequenceEqual(lst));
        }

#if !NO_THREAD
        [Fact]
        public void Delay_TimeSpan_Real_Error1()
        {
            Delay_TimeSpan_Real_Error1_Impl(ThreadPoolScheduler.Instance.DisableOptimizations());
        }

        [Fact]
        public void Delay_TimeSpan_Real_Error1_Stopwatch()
        {
            Delay_TimeSpan_Real_Error1_Impl(ThreadPoolScheduler.Instance);
        }
#endif

        private void Delay_TimeSpan_Real_Error1_Impl(IScheduler scheduler)
        {
            var ex = new Exception();

            var s = new Subject<int>();

            var res = s.Delay(TimeSpan.FromMilliseconds(10), scheduler);

            var e = new ManualResetEvent(false);
            var err = default(Exception);
            res.Subscribe(_ => { }, ex_ => { err = ex_; e.Set(); });

            Task.Run(() =>
            {
                s.OnNext(1);
                s.OnNext(2);
                s.OnNext(3);
                s.OnError(ex);
            });

            e.WaitOne();
            Assert.Same(ex, err);
        }

#if !NO_THREAD
        [Fact]
        public void Delay_TimeSpan_Real_Error2()
        {
            Delay_TimeSpan_Real_Error2_Impl(ThreadPoolScheduler.Instance.DisableOptimizations());
        }

        [Fact]
        public void Delay_TimeSpan_Real_Error2_Stopwatch()
        {
            Delay_TimeSpan_Real_Error2_Impl(ThreadPoolScheduler.Instance);
        }
#endif

        private void Delay_TimeSpan_Real_Error2_Impl(IScheduler scheduler)
        {
            var ex = new Exception();

            var s = new Subject<int>();

            var res = s.Delay(TimeSpan.FromMilliseconds(10), scheduler);

            var next = new ManualResetEvent(false);
            var e = new ManualResetEvent(false);
            var err = default(Exception);
            res.Subscribe(_ => { next.Set(); }, ex_ => { err = ex_; e.Set(); });

            Task.Run(() =>
            {
                s.OnNext(1);
                next.WaitOne();

                s.OnError(ex);
            });

            e.WaitOne();
            Assert.Same(ex, err);
        }

#if !NO_THREAD
        [Fact]
        public void Delay_TimeSpan_Real_Error3()
        {
            Delay_TimeSpan_Real_Error3_Impl(ThreadPoolScheduler.Instance.DisableOptimizations());
        }

        [Fact]
        public void Delay_TimeSpan_Real_Error3_Stopwatch()
        {
            Delay_TimeSpan_Real_Error3_Impl(ThreadPoolScheduler.Instance);
        }
#endif

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

            Task.Run(() =>
            {
                s.OnNext(1);
                next.WaitOne();

                s.OnError(ex);
                ack.Set();
            });

            e.WaitOne();
            Assert.Same(ex, err);
        }

        [Fact]
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
                           where n.Time > Subscribed
                           select new Recorded<Notification<int>>((ushort)(n.Time + delay), n.Value);

            res.Messages.AssertEqual(expected);
        }

        [Fact]
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

        [Fact]
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

        [Fact]
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

        [Fact]
        public void Delay_TimeSpan_DefaultScheduler()
        {
            Assert.True(Observable.Return(1).Delay(TimeSpan.FromMilliseconds(1)).ToEnumerable().SequenceEqual(new[] { 1 }));
        }

        [Fact]
        public void Delay_DateTimeOffset_DefaultScheduler()
        {
            Assert.True(Observable.Return(1).Delay(DateTimeOffset.UtcNow + TimeSpan.FromMilliseconds(1)).ToEnumerable().SequenceEqual(new[] { 1 }));
        }

        [Fact]
        public void Delay_CrossingMessages()
        {
            var lst = new List<int>();

            var evt = new ManualResetEvent(false);

            var s = new Subject<int>();
            s.Delay(TimeSpan.FromSeconds(0.01)).Subscribe(x =>
            {
                lst.Add(x);
                if (x < 9)
                {
                    s.OnNext(x + 1);
                }
                else
                {
                    s.OnCompleted();
                }
            }, () =>
            {
                evt.Set();
            });
            s.OnNext(0);

            evt.WaitOne();

            Assert.True(Enumerable.Range(0, 10).SequenceEqual(lst));
        }

        [Fact]
        public void Delay_Duration_ArgumentChecking()
        {
            var someObservable = DummyObservable<int>.Instance;

            ReactiveAssert.Throws<ArgumentNullException>(() => Observable.Delay(default(IObservable<int>), x => someObservable));
            ReactiveAssert.Throws<ArgumentNullException>(() => Observable.Delay(someObservable, default(Func<int, IObservable<int>>)));
            ReactiveAssert.Throws<ArgumentNullException>(() => Observable.Delay(default(IObservable<int>), someObservable, x => someObservable));
            ReactiveAssert.Throws<ArgumentNullException>(() => Observable.Delay(someObservable, default, x => someObservable));
            ReactiveAssert.Throws<ArgumentNullException>(() => Observable.Delay(someObservable, someObservable, default));
        }

        [Fact]
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

        [Fact]
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

        [Fact]
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

        [Fact]
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

        [Fact]
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

        [Fact]
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

        [Fact]
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

        [Fact]
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

        [Fact]
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

        [Fact]
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

        [Fact]
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
                OnNext(80, "")
            );

            var res = scheduler.Start(() =>
                xs.Delay(x =>
                {
                    if (x == 4)
                    {
                        throw ex;
                    }

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

        [Fact]
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
                OnNext(250 + 2 * 10, 2),
                OnNext(350 + 3 * 10, 3),
                OnNext(450 + 4 * 10, 4),
                OnCompleted<int>(550)
            );

            xs.Subscriptions.AssertEqual(
                Subscribe(200, 550)
            );
        }

        [Fact]
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
                OnNext(250 + 2 * 10, 2),
                OnNext(350 + 3 * 10, 3),
                OnNext(450 + 4 * 10, 4),
                OnCompleted<int>(450 + 4 * 10)
            );

            xs.Subscriptions.AssertEqual(
                Subscribe(200, 451)
            );
        }

        [Fact]
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
                OnNext(250 + 2 * 10, 2),
                OnNext(350 + 3 * 10, 3),
                OnError<int>(450 + 4 * 10, ex)
            );

            xs.Subscriptions.AssertEqual(
                Subscribe(200, 451)
            );
        }

        [Fact]
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
                OnNext(250 + 2 * 10, 2),
                OnNext(350 + 3 * 10, 3),
                OnError<int>(460, ex)
            );

            xs.Subscriptions.AssertEqual(
                Subscribe(200, 460)
            );
        }

        [Fact]
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
                    {
                        throw ex;
                    }
                })
            );

            res.Messages.AssertEqual(
                OnNext(250 + 2 * 10, 2),
                OnNext(350 + 3 * 10, 3),
                OnError<int>(450, ex)
            );

            xs.Subscriptions.AssertEqual(
                Subscribe(200, 450)
            );
        }

        [Fact]
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
                OnNext(250 + 2 * 10, 2),
                OnNext(350 + 3 * 10, 3),
                OnNext(450 + 4 * 10, 4),
                OnCompleted<int>(550)
            );

            xs.Subscriptions.AssertEqual(
                Subscribe(200, 550)
            );
        }

        [Fact]
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
                OnNext(250 + 20, 0),
                OnNext(350 + 10, 1),
                OnNext(450 + 30, 2),
                OnCompleted<int>(550)
            );

            xs.Subscriptions.AssertEqual(
                Subscribe(200, 550)
            );

            ys[0].Subscriptions.AssertEqual(Subscribe(250, 250 + 20));
            ys[1].Subscriptions.AssertEqual(Subscribe(350, 350 + 10));
            ys[2].Subscriptions.AssertEqual(Subscribe(450, 450 + 30));
        }

        [Fact]
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

        [Fact]
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

        [Fact]
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
                OnNext(10, "!")
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

        [Fact]
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
                OnNext(100, "!")
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

        [Fact]
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

        [Fact]
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

            Assert.False(called);
        }

        private class ImpulseScheduler : IScheduler
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

        [Fact]
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

        private class MyLongRunning1 : LocalScheduler, ISchedulerLongRunning
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
                Task.Run(() =>
                {
                    _start.Set();
                    action(state, b);
                    _stop.Set();
                });
                return b;
            }

            public override IDisposable Schedule<TState>(TState state, TimeSpan dueTime, Func<IScheduler, TState, IDisposable> action)
            {
                throw new NotImplementedException();
            }
        }

        [Fact]
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

        [Fact]
        public void Delay_Selector_Immediate()
        {
            var result = 0;
            var source = Observable.Return(1);
            var delayed = source.Delay(_ => Observable.Return(2));
            delayed.Subscribe(v => result = v);

            Assert.Equal(1, result);
        }

        private class MyLongRunning2 : LocalScheduler, ISchedulerLongRunning
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
                Task.Run(() =>
                {
                    action(state, b);
                    _stop.Set();
                });
                return b;
            }

            public override IDisposable Schedule<TState>(TState state, TimeSpan dueTime, Func<IScheduler, TState, IDisposable> action)
            {
                _start.Set();
                return Disposable.Empty;
            }
        }

    }
}

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
    public class BufferTest : ReactiveTest
    {
        #region + Boundary +

        [Fact]
        public void Buffer_ArgumentChecking()
        {
            ReactiveAssert.Throws<ArgumentNullException>(() => Observable.Buffer(default(IObservable<int>), DummyFunc<IObservable<int>>.Instance));
            ReactiveAssert.Throws<ArgumentNullException>(() => Observable.Buffer(DummyObservable<int>.Instance, default(Func<IObservable<int>>)));
            ReactiveAssert.Throws<ArgumentNullException>(() => Observable.Buffer(default(IObservable<int>), DummyObservable<int>.Instance, DummyFunc<int, IObservable<int>>.Instance));
            ReactiveAssert.Throws<ArgumentNullException>(() => Observable.Buffer(DummyObservable<int>.Instance, default, DummyFunc<int, IObservable<int>>.Instance));
            ReactiveAssert.Throws<ArgumentNullException>(() => Observable.Buffer(DummyObservable<int>.Instance, DummyObservable<int>.Instance, default(Func<int, IObservable<int>>)));
            ReactiveAssert.Throws<ArgumentNullException>(() => Observable.Buffer(default(IObservable<int>), DummyObservable<int>.Instance));
            ReactiveAssert.Throws<ArgumentNullException>(() => Observable.Buffer(DummyObservable<int>.Instance, default(IObservable<int>)));
        }

        [Fact]
        public void Buffer_Closings_Basic()
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
                xs.Buffer(() => Observable.Timer(TimeSpan.FromTicks((window++) * 100), scheduler))
            );

            res.Messages.AssertEqual(
                OnNext<IList<int>>(300, b => b.SequenceEqual(new int[] { 3, 4 })),
                OnNext<IList<int>>(500, b => b.SequenceEqual(new int[] { 5, 6, 7, 8, 9 })),
                OnNext<IList<int>>(590, b => b.SequenceEqual(new int[] { 10 })),
                OnCompleted<IList<int>>(590)
            );

            xs.Subscriptions.AssertEqual(
                Subscribe(200, 590)
            );
        }

        [Fact]
        public void Buffer_Closings_InnerSubscriptions()
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
                xs.Buffer(() => closings[window++])
            );

            res.Messages.AssertEqual(
                OnNext<IList<int>>(300, b => b.SequenceEqual(new int[] { 3, 4 })),
                OnNext<IList<int>>(400, b => b.SequenceEqual(new int[] { 5, 6 })),
                OnNext<IList<int>>(500, b => b.SequenceEqual(new int[] { 7, 8, 9 })),
                OnNext<IList<int>>(590, b => b.SequenceEqual(new int[] { 10 })),
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

        [Fact]
        public void Buffer_Closings_Empty()
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
                xs.Buffer(() => Observable.Empty<int>().Delay(TimeSpan.FromTicks((window++) * 100), scheduler))
            );

            res.Messages.AssertEqual(
                OnNext<IList<int>>(300, l => l.SequenceEqual(new int[] { 3, 4 })),
                OnNext<IList<int>>(500, l => l.SequenceEqual(new int[] { 5, 6, 7, 8, 9 })),
                OnNext<IList<int>>(590, l => l.SequenceEqual(new int[] { 10 })),
                OnCompleted<IList<int>>(590)
            );

            xs.Subscriptions.AssertEqual(
                Subscribe(200, 590)
            );
        }


        [Fact]
        public void Buffer_Closings_Dispose()
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
                xs.Buffer(() => Observable.Timer(TimeSpan.FromTicks((window++) * 100), scheduler)),
                400
            );

            res.Messages.AssertEqual(
                OnNext<IList<int>>(300, l => l.SequenceEqual(new int[] { 3, 4 }))
            );

            xs.Subscriptions.AssertEqual(
                Subscribe(200, 400)
            );
        }

        [Fact]
        public void Buffer_Closings_Error()
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
                xs.Buffer(() => Observable.Timer(TimeSpan.FromTicks((window++) * 100), scheduler))
            );

            res.Messages.AssertEqual(
                OnNext<IList<int>>(300, l => l.SequenceEqual(new int[] { 3, 4 })),
                OnNext<IList<int>>(500, l => l.SequenceEqual(new int[] { 5, 6, 7, 8, 9 })),
                OnError<IList<int>>(590, ex)
            );

            xs.Subscriptions.AssertEqual(
                Subscribe(200, 590)
            );
        }

        [Fact]
        public void Buffer_Closings_Throw()
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
                xs.Buffer<int, int>(() => { throw ex; })
            );

            res.Messages.AssertEqual(
                OnError<IList<int>>(200, ex)
            );

            xs.Subscriptions.AssertEqual(
                Subscribe(200, 200)
            );
        }

        [Fact]
        public void Buffer_Closings_WindowClose_Error()
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
                xs.Buffer(() => Observable.Throw<int>(ex, scheduler))
            );

            res.Messages.AssertEqual(
                OnError<IList<int>>(201, ex)
            );

            xs.Subscriptions.AssertEqual(
                Subscribe(200, 201)
            );
        }

        [Fact]
        public void Buffer_OpeningClosings_Basic()
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
                xs.Buffer(ys, x => Observable.Timer(TimeSpan.FromTicks(x), scheduler))
            );

            res.Messages.AssertEqual(
                OnNext<IList<int>>(305, b => b.SequenceEqual(new int[] { 4 })),
                OnNext<IList<int>>(400, b => b.SequenceEqual(new int[] { })),
                OnNext<IList<int>>(430, b => b.SequenceEqual(new int[] { 6, 7, 8 })),
                OnNext<IList<int>>(490, b => b.SequenceEqual(new int[] { 7, 8, 9 })),
                OnCompleted<IList<int>>(900)
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
        public void Buffer_Boundaries_Simple()
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
                xs.Buffer(ys)
            );

            res.Messages.AssertEqual(
                OnNext<IList<int>>(255, b => b.SequenceEqual(new int[] { 3 })),
                OnNext<IList<int>>(330, b => b.SequenceEqual(new int[] { 4, 5 })),
                OnNext<IList<int>>(350, b => b.SequenceEqual(new int[] { 6 })),
                OnNext<IList<int>>(400, b => b.SequenceEqual(new int[] { })),
                OnNext<IList<int>>(500, b => b.SequenceEqual(new int[] { 7, 8, 9 })),
                OnNext<IList<int>>(590, b => b.SequenceEqual(new int[] { 10 })),
                OnCompleted<IList<int>>(590)
            );

            xs.Subscriptions.AssertEqual(
                Subscribe(200, 590)
            );

            ys.Subscriptions.AssertEqual(
                Subscribe(200, 590)
            );
        }

        [Fact]
        public void Buffer_Boundaries_OnCompletedBoundaries()
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
                xs.Buffer(ys)
            );

            res.Messages.AssertEqual(
                OnNext<IList<int>>(255, b => b.SequenceEqual(new int[] { 3 })),
                OnNext<IList<int>>(330, b => b.SequenceEqual(new int[] { 4, 5 })),
                OnNext<IList<int>>(350, b => b.SequenceEqual(new int[] { 6 })),
                OnNext<IList<int>>(400, b => b.SequenceEqual(new int[] { })),
                OnCompleted<IList<int>>(400)
            );

            xs.Subscriptions.AssertEqual(
                Subscribe(200, 400)
            );

            ys.Subscriptions.AssertEqual(
                Subscribe(200, 400)
            );
        }

        [Fact]
        public void Buffer_Boundaries_OnErrorSource()
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
                xs.Buffer(ys)
            );

            res.Messages.AssertEqual(
                OnNext<IList<int>>(255, b => b.SequenceEqual(new int[] { 3 })),
                OnNext<IList<int>>(330, b => b.SequenceEqual(new int[] { 4, 5 })),
                OnNext<IList<int>>(350, b => b.SequenceEqual(new int[] { 6 })),
                OnError<IList<int>>(400, ex)
            );

            xs.Subscriptions.AssertEqual(
                Subscribe(200, 400)
            );

            ys.Subscriptions.AssertEqual(
                Subscribe(200, 400)
            );
        }

        [Fact]
        public void Buffer_Boundaries_OnErrorBoundaries()
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
                xs.Buffer(ys)
            );

            res.Messages.AssertEqual(
                OnNext<IList<int>>(255, b => b.SequenceEqual(new int[] { 3 })),
                OnNext<IList<int>>(330, b => b.SequenceEqual(new int[] { 4, 5 })),
                OnNext<IList<int>>(350, b => b.SequenceEqual(new int[] { 6 })),
                OnError<IList<int>>(400, ex)
            );

            xs.Subscriptions.AssertEqual(
                Subscribe(200, 400)
            );

            ys.Subscriptions.AssertEqual(
                Subscribe(200, 400)
            );
        }

        #endregion

        #region + Count +

        [Fact]
        public void Buffer_Single_ArgumentChecking()
        {
            var someObservable = Observable.Empty<int>();

            ReactiveAssert.Throws<ArgumentNullException>(() => Observable.Buffer(default(IObservable<int>), 1));
            ReactiveAssert.Throws<ArgumentOutOfRangeException>(() => Observable.Buffer(someObservable, 0));
            ReactiveAssert.Throws<ArgumentOutOfRangeException>(() => Observable.Buffer(someObservable, -1));
            ReactiveAssert.Throws<ArgumentNullException>(() => Observable.Buffer(default(IObservable<int>), 1, 1));
            ReactiveAssert.Throws<ArgumentOutOfRangeException>(() => Observable.Buffer(someObservable, 1, 0));
            ReactiveAssert.Throws<ArgumentOutOfRangeException>(() => Observable.Buffer(someObservable, 0, 1));
        }

        [Fact]
        public void Buffer_Count_PartialWindow()
        {
            var scheduler = new TestScheduler();

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
                OnNext<IList<int>>(250, l => l.SequenceEqual(new[] { 2, 3, 4, 5 })),
                OnCompleted<IList<int>>(250)
            );

            xs.Subscriptions.AssertEqual(
                Subscribe(200, 250)
            );
        }

        [Fact]
        public void Buffer_Count_FullWindows()
        {
            var scheduler = new TestScheduler();

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
                OnNext<IList<int>>(220, l => l.SequenceEqual(new[] { 2, 3 })),
                OnNext<IList<int>>(240, l => l.SequenceEqual(new[] { 4, 5 })),
                OnCompleted<IList<int>>(250)
            );

            xs.Subscriptions.AssertEqual(
                Subscribe(200, 250)
            );
        }

        [Fact]
        public void Buffer_Count_FullAndPartialWindows()
        {
            var scheduler = new TestScheduler();

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
                OnNext<IList<int>>(230, l => l.SequenceEqual(new int[] { 2, 3, 4 })),
                OnNext<IList<int>>(250, l => l.SequenceEqual(new int[] { 5 })),
                OnCompleted<IList<int>>(250)
            );

            xs.Subscriptions.AssertEqual(
                Subscribe(200, 250)
            );
        }

        [Fact]
        public void Buffer_Count_Error()
        {
            var scheduler = new TestScheduler();

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

        [Fact]
        public void Buffer_Count_Skip_Less()
        {
            var scheduler = new TestScheduler();

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
                OnNext<IList<int>>(230, l => l.SequenceEqual(new int[] { 2, 3, 4 })),
                OnNext<IList<int>>(240, l => l.SequenceEqual(new int[] { 3, 4, 5 })),
                OnNext<IList<int>>(250, l => l.SequenceEqual(new int[] { 4, 5 })),
                OnNext<IList<int>>(250, l => l.SequenceEqual(new int[] { 5 })),
                OnCompleted<IList<int>>(250)
            );

            xs.Subscriptions.AssertEqual(
                Subscribe(200, 250)
            );
        }

        [Fact]
        public void Buffer_Count_Skip_More()
        {
            var scheduler = new TestScheduler();

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
                OnNext<IList<int>>(220, l => l.SequenceEqual(new int[] { 2, 3 })),
                OnNext<IList<int>>(250, l => l.SequenceEqual(new int[] { 5 })),
                OnCompleted<IList<int>>(250)
            );

            xs.Subscriptions.AssertEqual(
                Subscribe(200, 250)
            );
        }

        [Fact]
        public void BufferWithCount_ArgumentChecking()
        {
            ReactiveAssert.Throws<ArgumentNullException>(() => Observable.Buffer(default(IObservable<int>), 1, 1));
            ReactiveAssert.Throws<ArgumentOutOfRangeException>(() => Observable.Buffer(DummyObservable<int>.Instance, 0, 1));
            ReactiveAssert.Throws<ArgumentOutOfRangeException>(() => Observable.Buffer(DummyObservable<int>.Instance, 1, 0));
            ReactiveAssert.Throws<ArgumentNullException>(() => Observable.Buffer(default(IObservable<int>), 1));
            ReactiveAssert.Throws<ArgumentOutOfRangeException>(() => Observable.Buffer(DummyObservable<int>.Instance, 0));
        }

        [Fact]
        public void BufferWithCount_Basic()
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

        [Fact]
        public void BufferWithCount_Disposed()
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
                xs.Buffer(3, 2).Select(x => string.Join(",", x.Select(xx => xx.ToString()).ToArray())), 370
            );

            res.Messages.AssertEqual(
                OnNext(280, "2,3,4"),
                OnNext(350, "4,5,6")
            );

            xs.Subscriptions.AssertEqual(
                Subscribe(200, 370)
            );
        }

        [Fact]
        public void BufferWithCount_Error()
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

        [Fact]
        public void BufferWithCount_Default()
        {
            Observable.Range(1, 10).Buffer(3).Skip(1).First().AssertEqual(4, 5, 6);
            Observable.Range(1, 10).Buffer(3, 2).Skip(1).First().AssertEqual(3, 4, 5);
        }

        #endregion

        #region + Time +

        [Fact]
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

        [Fact]
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

        [Fact]
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

        [Fact]
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

        [Fact]
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

        [Fact]
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

        [Fact]
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

        [Fact]
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

#if !WINDOWS
            scheduler.Timers.AssertEqual(
                new TimerRun(200, 600) { 300, 400, 500 }
            );
#endif
        }

        [Fact]
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

#if !WINDOWS
            scheduler.Timers.AssertEqual(
                new TimerRun(200, 480) { 300, 400 }
            );
#endif
        }

        [Fact]
        public void BufferWithTime_Default()
        {
            Observable.Range(0, 10).Buffer(TimeSpan.FromDays(1), TimeSpan.FromDays(1)).First().AssertEqual(Enumerable.Range(0, 10));
            Observable.Range(0, 10).Buffer(TimeSpan.FromDays(1)).First().AssertEqual(Enumerable.Range(0, 10));
        }

        [Fact]
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

        [Fact]
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

        [Fact]
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

        [Fact]
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

        [Fact]
        public void BufferWithTimeOrCount_Default()
        {
            Observable.Range(1, 10, DefaultScheduler.Instance).Buffer(TimeSpan.FromDays(1), 3).Skip(1).First().AssertEqual(4, 5, 6);
        }

        [Fact]
        public void BufferWithTime_TickWhileOnCompleted()
        {
            var scheduler = new TestScheduler();

            Observable.Return(1)
                .Buffer(TimeSpan.FromMilliseconds(1), TimeSpan.FromMilliseconds(2), scheduler)
                .Subscribe(v =>
                {
                    scheduler.AdvanceBy(TimeSpan.FromMilliseconds(1).Ticks);
                });
        }

        #endregion

    }
}

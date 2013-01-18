// Copyright (c) Microsoft Open Technologies, Inc. All rights reserved. See License.txt in the project root for license information.

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reactive;
using System.Reactive.Concurrency;
using System.Reactive.Linq;
using System.Threading;
using System.Reactive.Subjects;
using Microsoft.Reactive.Testing;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using ReactiveTests.Dummies;

#if !NO_TPL
using System.Threading.Tasks;
#endif

namespace ReactiveTests.Tests
{
    [TestClass]
    public class ObservableMultipleTest : ReactiveTest
    {
        #region + Amb +

        [TestMethod]
        public void Amb_ArgumentChecking()
        {
            ReactiveAssert.Throws<ArgumentNullException>(() => Observable.Amb((IObservable<int>[])null));
            ReactiveAssert.Throws<ArgumentNullException>(() => Observable.Amb((IEnumerable<IObservable<int>>)null));
            ReactiveAssert.Throws<ArgumentNullException>(() => Observable.Amb(null, DummyObservable<int>.Instance));
            ReactiveAssert.Throws<ArgumentNullException>(() => Observable.Amb(DummyObservable<int>.Instance, null));
        }

        [TestMethod]
        public void Amb_Never2()
        {
            var scheduler = new TestScheduler();

            var l = scheduler.CreateHotObservable(
                OnNext(150, 1)
            );

            var r = scheduler.CreateHotObservable(
                OnNext(150, 1)
            );

            var res = scheduler.Start(() =>
                l.Amb(r)
            );

            res.Messages.AssertEqual(
            );

            l.Subscriptions.AssertEqual(
                Subscribe(200, 1000)
            );

            r.Subscriptions.AssertEqual(
                Subscribe(200, 1000)
            );
        }

        [TestMethod]
        public void Amb_Never3()
        {
            var scheduler = new TestScheduler();

            var n1 = scheduler.CreateHotObservable(
                OnNext(150, 1)
            );

            var n2 = scheduler.CreateHotObservable(
                OnNext(150, 1)
            );

            var n3 = scheduler.CreateHotObservable(
                OnNext(150, 1)
            );

            var res = scheduler.Start(() =>
                new[] { n1, n2, n3 }.Amb()
            );

            res.Messages.AssertEqual(
            );

            n1.Subscriptions.AssertEqual(
                Subscribe(200, 1000)
            );

            n2.Subscriptions.AssertEqual(
                Subscribe(200, 1000)
            );

            n3.Subscriptions.AssertEqual(
                Subscribe(200, 1000)
            );
        }

        [TestMethod]
        public void Amb_Never3_Params()
        {
            var scheduler = new TestScheduler();

            var n1 = scheduler.CreateHotObservable(
                OnNext(150, 1)
            );

            var n2 = scheduler.CreateHotObservable(
                OnNext(150, 1)
            );

            var n3 = scheduler.CreateHotObservable(
                OnNext(150, 1)
            );

            var res = scheduler.Start(() =>
                Observable.Amb(n1, n2, n3)
            );

            res.Messages.AssertEqual(
            );

            n1.Subscriptions.AssertEqual(
                Subscribe(200, 1000)
            );

            n2.Subscriptions.AssertEqual(
                Subscribe(200, 1000)
            );

            n3.Subscriptions.AssertEqual(
                Subscribe(200, 1000)
            );
        }

        [TestMethod]
        public void Amb_NeverEmpty()
        {
            var scheduler = new TestScheduler();

            var n = scheduler.CreateHotObservable(
                OnNext(150, 1)
            );

            var e = scheduler.CreateHotObservable(
                OnNext(150, 1),
                OnCompleted<int>(225)
            );

            var res = scheduler.Start(() =>
                n.Amb(e)
            );

            res.Messages.AssertEqual(
                OnCompleted<int>(225)
            );

            n.Subscriptions.AssertEqual(
                Subscribe(200, 225)
            );

            e.Subscriptions.AssertEqual(
                Subscribe(200, 225)
            );
        }

        [TestMethod]
        public void Amb_EmptyNever()
        {
            var scheduler = new TestScheduler();

            var n = scheduler.CreateHotObservable(
                OnNext(150, 1)
            );

            var e = scheduler.CreateHotObservable(
                OnNext(150, 1),
                OnCompleted<int>(225)
            );

            var res = scheduler.Start(() =>
                e.Amb(n)
            );

            res.Messages.AssertEqual(
                OnCompleted<int>(225)
            );

            n.Subscriptions.AssertEqual(
                Subscribe(200, 225)
            );

            e.Subscriptions.AssertEqual(
                Subscribe(200, 225)
            );
        }

        [TestMethod]
        public void Amb_RegularShouldDisposeLoser()
        {
            var scheduler = new TestScheduler();

            var o1 = scheduler.CreateHotObservable(
                OnNext(150, 1),
                OnNext(210, 2),
                OnCompleted<int>(240)
            );

            var o2 = scheduler.CreateHotObservable(
                OnNext(150, 1),
                OnNext(220, 3),
                OnCompleted<int>(250)
            );

            var res = scheduler.Start(() =>
                o1.Amb(o2)
            );

            res.Messages.AssertEqual(
                OnNext(210, 2),
                OnCompleted<int>(240)
            );

            o1.Subscriptions.AssertEqual(
                Subscribe(200, 240)
            );

            o2.Subscriptions.AssertEqual(
                Subscribe(200, 210)
            );
        }

        [TestMethod]
        public void Amb_WinnerThrows()
        {
            var scheduler = new TestScheduler();

            var ex = new Exception();

            var o1 = scheduler.CreateHotObservable(
                OnNext(150, 1),
                OnNext(210, 2),
                OnError<int>(220, ex)
            );

            var o2 = scheduler.CreateHotObservable(
                OnNext(150, 1),
                OnNext(220, 3),
                OnCompleted<int>(250)
            );

            var res = scheduler.Start(() =>
                o1.Amb(o2)
            );

            res.Messages.AssertEqual(
                OnNext(210, 2),
                OnError<int>(220, ex)
            );

            o1.Subscriptions.AssertEqual(
                Subscribe(200, 220)
            );

            o2.Subscriptions.AssertEqual(
                Subscribe(200, 210)
            );
        }

        [TestMethod]
        public void Amb_LoserThrows()
        {
            var scheduler = new TestScheduler();

            var ex = new Exception();

            var o1 = scheduler.CreateHotObservable(
                OnNext(150, 1),
                OnNext(220, 2),
                OnError<int>(230, ex)
            );

            var o2 = scheduler.CreateHotObservable(
                OnNext(150, 1),
                OnNext(210, 3),
                OnCompleted<int>(250)
            );

            var res = scheduler.Start(() =>
                o1.Amb(o2)
            );

            res.Messages.AssertEqual(
                OnNext(210, 3),
                OnCompleted<int>(250)
            );

            o1.Subscriptions.AssertEqual(
                Subscribe(200, 210)
            );

            o2.Subscriptions.AssertEqual(
                Subscribe(200, 250)
            );
        }

        [TestMethod]
        public void Amb_ThrowsBeforeElectionLeft()
        {
            var scheduler = new TestScheduler();

            var ex = new Exception();

            var o1 = scheduler.CreateHotObservable(
                OnNext(150, 1),
                OnError<int>(210, ex)
            );

            var o2 = scheduler.CreateHotObservable(
                OnNext(150, 1),
                OnNext(220, 3),
                OnCompleted<int>(250)
            );

            var res = scheduler.Start(() =>
                o1.Amb(o2)
            );

            res.Messages.AssertEqual(
                OnError<int>(210, ex)
            );

            o1.Subscriptions.AssertEqual(
                Subscribe(200, 210)
            );

            o2.Subscriptions.AssertEqual(
                Subscribe(200, 210)
            );
        }

        [TestMethod]
        public void Amb_ThrowsBeforeElectionRight()
        {
            var scheduler = new TestScheduler();

            var ex = new Exception();

            var o1 = scheduler.CreateHotObservable(
                OnNext(150, 1),
                OnNext(220, 3),
                OnCompleted<int>(250)
            );

            var o2 = scheduler.CreateHotObservable(
                OnNext(150, 1),
                OnError<int>(210, ex)
            );

            var res = scheduler.Start(() =>
                o1.Amb(o2)
            );

            res.Messages.AssertEqual(
                OnError<int>(210, ex)
            );

            o1.Subscriptions.AssertEqual(
                Subscribe(200, 210)
            );

            o2.Subscriptions.AssertEqual(
                Subscribe(200, 210)
            );
        }

        #endregion

        #region + Buffer +

        [TestMethod]
        public void Buffer_ArgumentChecking()
        {
            ReactiveAssert.Throws<ArgumentNullException>(() => Observable.Buffer(default(IObservable<int>), DummyFunc<IObservable<int>>.Instance));
            ReactiveAssert.Throws<ArgumentNullException>(() => Observable.Buffer(DummyObservable<int>.Instance, default(Func<IObservable<int>>)));
            ReactiveAssert.Throws<ArgumentNullException>(() => Observable.Buffer(default(IObservable<int>), DummyObservable<int>.Instance, DummyFunc<int, IObservable<int>>.Instance));
            ReactiveAssert.Throws<ArgumentNullException>(() => Observable.Buffer(DummyObservable<int>.Instance, default(IObservable<int>), DummyFunc<int, IObservable<int>>.Instance));
            ReactiveAssert.Throws<ArgumentNullException>(() => Observable.Buffer(DummyObservable<int>.Instance, DummyObservable<int>.Instance, default(Func<int, IObservable<int>>)));
            ReactiveAssert.Throws<ArgumentNullException>(() => Observable.Buffer(default(IObservable<int>), DummyObservable<int>.Instance));
            ReactiveAssert.Throws<ArgumentNullException>(() => Observable.Buffer(DummyObservable<int>.Instance, default(IObservable<int>)));
        }

        [TestMethod]
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

        [TestMethod]
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

        [TestMethod]
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


        [TestMethod]
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

        [TestMethod]
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

        [TestMethod]
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

        [TestMethod]
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

        [TestMethod]
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

        [TestMethod]
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

        [TestMethod]
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

        [TestMethod]
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

        [TestMethod]
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

        #region + Catch +

        [TestMethod]
        public void Catch_ArgumentChecking()
        {
            ReactiveAssert.Throws<ArgumentNullException>(() => Observable.Catch<int>((IObservable<int>[])null));
            ReactiveAssert.Throws<ArgumentNullException>(() => Observable.Catch<int>((IEnumerable<IObservable<int>>)null));
            ReactiveAssert.Throws<ArgumentNullException>(() => Observable.Catch<int>(DummyObservable<int>.Instance, null));
            ReactiveAssert.Throws<ArgumentNullException>(() => Observable.Catch<int>((IObservable<int>)null, DummyObservable<int>.Instance));
            ReactiveAssert.Throws<ArgumentNullException>(() => Observable.Catch<int, Exception>(null, _ => DummyObservable<int>.Instance));
            ReactiveAssert.Throws<ArgumentNullException>(() => Observable.Catch<int, Exception>(DummyObservable<int>.Instance, null));
        }

        [TestMethod]
        public void Catch_IEofIO_GetEnumeratorThrows()
        {
            var ex = new Exception();

            var scheduler = new TestScheduler();

            var xss = new RogueEnumerable<IObservable<int>>(ex);

            var res = scheduler.Start(() =>
                Observable.Catch(xss)
            );

            res.Messages.AssertEqual(
                OnError<int>(200, ex)
            );
        }

        [TestMethod]
        public void Catch_IEofIO()
        {
            var scheduler = new TestScheduler();

            var xs1 = scheduler.CreateColdObservable<int>(
                OnNext(10, 1),
                OnNext(20, 2),
                OnNext(30, 3),
                OnError<int>(40, new Exception())
            );

            var xs2 = scheduler.CreateColdObservable<int>(
                OnNext(10, 4),
                OnNext(20, 5),
                OnError<int>(30, new Exception())
            );

            var xs3 = scheduler.CreateColdObservable<int>(
                OnNext(10, 6),
                OnNext(20, 7),
                OnNext(30, 8),
                OnNext(40, 9),
                OnCompleted<int>(50)
            );

            var res = scheduler.Start(() =>
                Observable.Catch(new[] { xs1, xs2, xs3 })
            );

            res.Messages.AssertEqual(
                OnNext(210, 1),
                OnNext(220, 2),
                OnNext(230, 3),
                OnNext(250, 4),
                OnNext(260, 5),
                OnNext(280, 6),
                OnNext(290, 7),
                OnNext(300, 8),
                OnNext(310, 9),
                OnCompleted<int>(320)
            );

            xs1.Subscriptions.AssertEqual(
                Subscribe(200, 240)
            );

            xs2.Subscriptions.AssertEqual(
                Subscribe(240, 270)
            );

            xs3.Subscriptions.AssertEqual(
                Subscribe(270, 320)
            );
        }

        [TestMethod]
        public void Catch_NoErrors()
        {
            var scheduler = new TestScheduler();

            var o1 = scheduler.CreateHotObservable(
                OnNext(150, 1),
                OnNext(210, 2),
                OnNext(220, 3),
                OnCompleted<int>(230)
            );

            var o2 = scheduler.CreateHotObservable(
                OnNext(240, 4),
                OnCompleted<int>(250)
            );

            var res = scheduler.Start(() =>
                o1.Catch(o2)
            );

            res.Messages.AssertEqual(
                OnNext(210, 2),
                OnNext(220, 3),
                OnCompleted<int>(230)
            );

            o1.Subscriptions.AssertEqual(
                Subscribe(200, 230)
            );

            o2.Subscriptions.AssertEqual(
            );
        }

        [TestMethod]
        public void Catch_Never()
        {
            var scheduler = new TestScheduler();

            var o1 = scheduler.CreateHotObservable(
                OnNext(150, 1)
            );

            var o2 = scheduler.CreateHotObservable(
                OnNext(240, 4),
                OnCompleted<int>(250)
            );

            var res = scheduler.Start(() =>
                o1.Catch(o2)
            );

            res.Messages.AssertEqual(
            );

            o1.Subscriptions.AssertEqual(
                Subscribe(200, 1000)
            );

            o2.Subscriptions.AssertEqual(
            );
        }

        [TestMethod]
        public void Catch_Empty()
        {
            var scheduler = new TestScheduler();

            var o1 = scheduler.CreateHotObservable(
                OnNext(150, 1),
                OnCompleted<int>(230)
            );

            var o2 = scheduler.CreateHotObservable(
                OnNext(240, 4),
                OnCompleted<int>(250)
            );

            var res = scheduler.Start(() =>
                o1.Catch(o2)
            );

            res.Messages.AssertEqual(
                OnCompleted<int>(230)
            );

            o1.Subscriptions.AssertEqual(
                Subscribe(200, 230)
            );

            o2.Subscriptions.AssertEqual(
            );
        }

        [TestMethod]
        public void Catch_Return()
        {
            var scheduler = new TestScheduler();

            var o1 = scheduler.CreateHotObservable(
                OnNext(150, 1),
                OnNext(210, 2),
                OnCompleted<int>(230)
            );

            var o2 = scheduler.CreateHotObservable(
                OnNext(240, 4),
                OnCompleted<int>(250)
            );

            var res = scheduler.Start(() =>
                o1.Catch(o2)
            );

            res.Messages.AssertEqual(
                OnNext(210, 2),
                OnCompleted<int>(230)
            );

            o1.Subscriptions.AssertEqual(
                Subscribe(200, 230)
            );

            o2.Subscriptions.AssertEqual(
            );
        }

        [TestMethod]
        public void Catch_Error()
        {
            var scheduler = new TestScheduler();

            var ex = new Exception();

            var o1 = scheduler.CreateHotObservable(
                OnNext(150, 1),
                OnNext(210, 2),
                OnNext(220, 3),
                OnError<int>(230, ex)
            );

            var o2 = scheduler.CreateHotObservable(
                OnNext(240, 4),
                OnCompleted<int>(250)
            );

            var res = scheduler.Start(() =>
                o1.Catch(o2)
            );

            res.Messages.AssertEqual(
                OnNext(210, 2),
                OnNext(220, 3),
                OnNext(240, 4),
                OnCompleted<int>(250)
            );

            o1.Subscriptions.AssertEqual(
                Subscribe(200, 230)
            );

            o2.Subscriptions.AssertEqual(
                Subscribe(230, 250)
            );
        }

        [TestMethod]
        public void Catch_Error_Never()
        {
            var scheduler = new TestScheduler();

            var ex = new Exception();

            var o1 = scheduler.CreateHotObservable(
                OnNext(150, 1),
                OnNext(210, 2),
                OnNext(220, 3),
                OnError<int>(230, ex)
            );

            var o2 = scheduler.CreateHotObservable(
                OnNext(150, 1)
            );

            var res = scheduler.Start(() =>
                o1.Catch(o2)
            );

            res.Messages.AssertEqual(
                OnNext(210, 2),
                OnNext(220, 3)
            );

            o1.Subscriptions.AssertEqual(
                Subscribe(200, 230)
            );

            o2.Subscriptions.AssertEqual(
                Subscribe(230, 1000)
            );
        }

        [TestMethod]
        public void Catch_Error_Error()
        {
            var scheduler = new TestScheduler();

            var ex = new Exception();

            var o1 = scheduler.CreateHotObservable(
                OnNext(150, 1),
                OnNext(210, 2),
                OnNext(220, 3),
                OnError<int>(230, new Exception())
            );

            var o2 = scheduler.CreateHotObservable(
                OnNext(240, 4),
                OnError<int>(250, ex)
            );

            var res = scheduler.Start(() =>
                o1.Catch(o2)
            );

            res.Messages.AssertEqual(
                OnNext(210, 2),
                OnNext(220, 3),
                OnNext(240, 4),
                OnError<int>(250, ex)
            );

            o1.Subscriptions.AssertEqual(
                Subscribe(200, 230)
            );

            o2.Subscriptions.AssertEqual(
                Subscribe(230, 250)
            );
        }

        [TestMethod]
        public void Catch_Multiple()
        {
            var scheduler = new TestScheduler();

            var ex = new Exception();

            var o1 = scheduler.CreateHotObservable(
                OnNext(150, 1),
                OnNext(210, 2),
                OnError<int>(215, ex)
            );

            var o2 = scheduler.CreateHotObservable(
                OnNext(220, 3),
                OnError<int>(225, ex)
            );

            var o3 = scheduler.CreateHotObservable(
                OnNext(230, 4),
                OnCompleted<int>(235)
            );

            var res = scheduler.Start(() =>
                Observable.Catch(o1, o2, o3)
            );

            res.Messages.AssertEqual(
                OnNext(210, 2),
                OnNext(220, 3),
                OnNext(230, 4),
                OnCompleted<int>(235)
            );

            o1.Subscriptions.AssertEqual(
                Subscribe(200, 215)
            );

            o2.Subscriptions.AssertEqual(
                Subscribe(215, 225)
            );

            o3.Subscriptions.AssertEqual(
                Subscribe(225, 235)
            );
        }

        [TestMethod]
        public void Catch_ErrorSpecific_Caught()
        {
            var scheduler = new TestScheduler();

            var ex = new ArgumentException("x");

            var o1 = scheduler.CreateHotObservable(
                OnNext(150, 1),
                OnNext(210, 2),
                OnNext(220, 3),
                OnError<int>(230, ex)
            );

            var o2 = scheduler.CreateHotObservable(
                OnNext(240, 4),
                OnCompleted<int>(250)
            );

            var handlerCalled = default(long?);

            var res = scheduler.Start(() =>
                o1.Catch((ArgumentException ex_) => { handlerCalled = scheduler.Clock; return o2; })
            );

            Assert.AreEqual(230, handlerCalled);

            res.Messages.AssertEqual(
                OnNext(210, 2),
                OnNext(220, 3),
                OnNext(240, 4),
                OnCompleted<int>(250)
            );

            o1.Subscriptions.AssertEqual(
                Subscribe(200, 230)
            );

            o2.Subscriptions.AssertEqual(
                Subscribe(230, 250)
            );
        }

        [TestMethod]
        public void Catch_ErrorSpecific_Uncaught()
        {
            var scheduler = new TestScheduler();

            var ex = new InvalidOperationException("x");

            var o1 = scheduler.CreateHotObservable(
                OnNext(150, 1),
                OnNext(210, 2),
                OnNext(220, 3),
                OnError<int>(230, ex)
            );

            var o2 = scheduler.CreateHotObservable(
                OnNext(240, 4),
                OnCompleted<int>(250)
            );

            var handlerCalled = default(long?);

            var res = scheduler.Start(() =>
                o1.Catch((ArgumentException ex_) => { handlerCalled = scheduler.Clock; return o2; })
            );

            Assert.AreEqual(null, handlerCalled);

            res.Messages.AssertEqual(
                OnNext(210, 2),
                OnNext(220, 3),
                OnError<int>(230, ex)
            );

            o1.Subscriptions.AssertEqual(
                Subscribe(200, 230)
            );

            o2.Subscriptions.AssertEqual(
            );
        }

        [TestMethod]
        public void Catch_HandlerThrows()
        {
            var scheduler = new TestScheduler();

            var ex1 = new ArgumentException("x");
            var ex2 = new Exception();

            var o1 = scheduler.CreateHotObservable(
                OnNext(150, 1),
                OnNext(210, 2),
                OnNext(220, 3),
                OnError<int>(230, ex1)
            );

            var handlerCalled = default(long?);

            var res = scheduler.Start(() =>
                o1.Catch((ArgumentException ex_) => { handlerCalled = scheduler.Clock; throw ex2; })
            );

            Assert.AreEqual(230, handlerCalled);

            res.Messages.AssertEqual(
                OnNext(210, 2),
                OnNext(220, 3),
                OnError<int>(230, ex2)
            );

            o1.Subscriptions.AssertEqual(
                Subscribe(200, 230)
            );
        }

        [TestMethod]
        public void Catch_Nested_OuterCatches()
        {
            var scheduler = new TestScheduler();

            var ex = new ArgumentException("x");

            var o1 = scheduler.CreateHotObservable(
                OnNext(150, 1),
                OnNext(210, 2),
                OnError<int>(215, ex)
            );

            var o2 = scheduler.CreateHotObservable(
                OnNext(220, 3),
                OnCompleted<int>(225)
            );

            var o3 = scheduler.CreateHotObservable(
                OnNext(220, 4), //!
                OnCompleted<int>(225)
            );

            var firstHandlerCalled = default(long?);
            var secondHandlerCalled = default(long?);

            var res = scheduler.Start(() =>
                o1
                .Catch((InvalidOperationException ex_) => { firstHandlerCalled = scheduler.Clock; return o2; })
                .Catch((ArgumentException ex_) => { secondHandlerCalled = scheduler.Clock; return o3; })
            );

            Assert.AreEqual(null, firstHandlerCalled);
            Assert.AreEqual(215, secondHandlerCalled);

            res.Messages.AssertEqual(
                OnNext(210, 2),
                OnNext(220, 4),
                OnCompleted<int>(225)
            );

            o1.Subscriptions.AssertEqual(
                Subscribe(200, 215)
            );

            o2.Subscriptions.AssertEqual(
            );

            o3.Subscriptions.AssertEqual(
                Subscribe(215, 225)
            );
        }

        [TestMethod]
        public void Catch_Nested_InnerCatches()
        {
            var scheduler = new TestScheduler();

            var ex = new ArgumentException("x");

            var o1 = scheduler.CreateHotObservable(
                OnNext(150, 1),
                OnNext(210, 2),
                OnError<int>(215, ex)
            );

            var o2 = scheduler.CreateHotObservable(
                OnNext(220, 3), //!
                OnCompleted<int>(225)
            );

            var o3 = scheduler.CreateHotObservable(
                OnNext(220, 4),
                OnCompleted<int>(225)
            );

            var firstHandlerCalled = default(long?);
            var secondHandlerCalled = default(long?);

            var res = scheduler.Start(() =>
                o1
                .Catch((ArgumentException ex_) => { firstHandlerCalled = scheduler.Clock; return o2; })
                .Catch((InvalidOperationException ex_) => { secondHandlerCalled = scheduler.Clock; return o3; })
            );

            Assert.AreEqual(215, firstHandlerCalled);
            Assert.AreEqual(null, secondHandlerCalled);

            res.Messages.AssertEqual(
                OnNext(210, 2),
                OnNext(220, 3),
                OnCompleted<int>(225)
            );

            o1.Subscriptions.AssertEqual(
                Subscribe(200, 215)
            );

            o2.Subscriptions.AssertEqual(
                Subscribe(215, 225)
            );

            o3.Subscriptions.AssertEqual(
            );
        }

        [TestMethod]
        public void Catch_ThrowFromNestedCatch()
        {
            var scheduler = new TestScheduler();

            var ex1 = new ArgumentException("x1");
            var ex2 = new ArgumentException("x2");

            var o1 = scheduler.CreateHotObservable(
                OnNext(150, 1),
                OnNext(210, 2),
                OnError<int>(215, ex1)
            );

            var o2 = scheduler.CreateHotObservable(
                OnNext(220, 3), //!
                OnError<int>(225, ex2)
            );

            var o3 = scheduler.CreateHotObservable(
                OnNext(230, 4),
                OnCompleted<int>(235)
            );

            var firstHandlerCalled = default(long?);
            var secondHandlerCalled = default(long?);

            var res = scheduler.Start(() =>
                o1
                .Catch((ArgumentException ex_) => { firstHandlerCalled = scheduler.Clock; Assert.IsTrue(ex1 == ex_, "Expected ex1"); return o2; })
                .Catch((ArgumentException ex_) => { secondHandlerCalled = scheduler.Clock; Assert.IsTrue(ex2 == ex_, "Expected ex2"); return o3; })
            );

            Assert.AreEqual(215, firstHandlerCalled);
            Assert.AreEqual(225, secondHandlerCalled);

            res.Messages.AssertEqual(
                OnNext(210, 2),
                OnNext(220, 3),
                OnNext(230, 4),
                OnCompleted<int>(235)
            );

            o1.Subscriptions.AssertEqual(
                Subscribe(200, 215)
            );

            o2.Subscriptions.AssertEqual(
                Subscribe(215, 225)
            );

            o3.Subscriptions.AssertEqual(
                Subscribe(225, 235)
            );
        }

        [TestMethod]
        public void Catch_DefaultScheduler_Binary()
        {
            var evt = new ManualResetEvent(false);

            int res = 0;
            Observable.Return(1).Catch(Observable.Return(2)).Subscribe(x =>
            {
                res = x;
                evt.Set();
            });

            evt.WaitOne();
            Assert.AreEqual(1, res);
        }

        [TestMethod]
        public void Catch_DefaultScheduler_Nary()
        {
            var evt = new ManualResetEvent(false);

            int res = 0;
            Observable.Catch(Observable.Return(1), Observable.Return(2), Observable.Return(3)).Subscribe(x =>
            {
                res = x;
                evt.Set();
            });

            evt.WaitOne();
            Assert.AreEqual(1, res);
        }

        [TestMethod]
        public void Catch_DefaultScheduler_NaryEnumerable()
        {
            var evt = new ManualResetEvent(false);

            IEnumerable<IObservable<int>> sources = new[] { Observable.Return(1), Observable.Return(2), Observable.Return(3) };

            int res = 0;
            Observable.Catch(sources).Subscribe(x =>
            {
                res = x;
                evt.Set();
            });

            evt.WaitOne();
            Assert.AreEqual(1, res);
        }

        [TestMethod]
        public void Catch_EmptyIterator()
        {
            var scheduler = new TestScheduler();

            var res = scheduler.Start(() =>
                Observable.Catch<int>((IEnumerable<IObservable<int>>)new IObservable<int>[0])
            );

            res.Messages.AssertEqual(
                OnCompleted<int>(200)
            );
        }

        [TestMethod]
        public void Catch_IteratorThrows()
        {
            var scheduler = new TestScheduler();
            var ex = new Exception();

            var res = scheduler.Start(() =>
                Observable.Catch<int>(Catch_IteratorThrows_Source(ex, true))
            );

            res.Messages.AssertEqual(
                OnError<int>(200, ex)
            );
        }

        private IEnumerable<IObservable<int>> Catch_IteratorThrows_Source(Exception ex, bool b)
        {
            if (b)
                throw ex;
            else
                yield break;
        }

        [TestMethod]
        public void Catch_EnumerableThrows()
        {
            var scheduler = new TestScheduler();

            var o = scheduler.CreateHotObservable(
                OnNext(150, 1),
                OnNext(210, 2),
                OnNext(220, 3),
                OnCompleted<int>(225)
            );

            var ex = new Exception();
            var xss = new MockEnumerable<IObservable<int>>(scheduler, GetObservablesForCatchThrow(o, ex));

            var res = scheduler.Start(() =>
                xss.Catch()
            );

            res.Messages.AssertEqual(
                OnNext(210, 2),
                OnNext(220, 3),
                OnCompleted<int>(225)
            );

            o.Subscriptions.AssertEqual(
                Subscribe(200, 225)
            );

            xss.Subscriptions.AssertEqual(
                Subscribe(200, 225)
            );
        }

        private IEnumerable<IObservable<int>> GetObservablesForCatchThrow(IObservable<int> first, Exception ex)
        {
            yield return first;
            throw ex;
        }

        [TestMethod]
        public void Catch_EnumerableTiming()
        {
            var scheduler = new TestScheduler();

            var o1 = scheduler.CreateHotObservable(
                OnNext(150, 1),
                OnNext(210, 2), // !
                OnNext(220, 3), // !
                OnError<int>(230, new Exception())
            );

            var o2 = scheduler.CreateColdObservable(
                OnNext(50, 4),  // !
                OnNext(60, 5),  // !
                OnNext(70, 6),  // !
                OnError<int>(80, new Exception())
            );

            var o3 = scheduler.CreateHotObservable(
                OnNext(150, 1),
                OnNext(200, 2),
                OnNext(210, 3),
                OnNext(220, 4),
                OnNext(230, 5),
                OnNext(270, 6),
                OnNext(320, 7), // !
                OnNext(330, 8), // !
                OnCompleted<int>(340)
            );

            var xss = new MockEnumerable<ITestableObservable<int>>(scheduler, new[] { o1, o2, o3, o2 });

            var res = scheduler.Start(() =>
                xss.Select(xs => (IObservable<int>)xs).Catch()
            );

            res.Messages.AssertEqual(
                OnNext(210, 2),
                OnNext(220, 3),
                OnNext(280, 4),
                OnNext(290, 5),
                OnNext(300, 6),
                OnNext(320, 7),
                OnNext(330, 8),
                OnCompleted<int>(340)
            );

            o1.Subscriptions.AssertEqual(
                Subscribe(200, 230)
            );

            o2.Subscriptions.AssertEqual(
                Subscribe(230, 310)
            );

            o3.Subscriptions.AssertEqual(
                Subscribe(310, 340)
            );

            xss.Subscriptions.AssertEqual(
                Subscribe(200, 340)
            );
        }

        [TestMethod]
        public void Catch_Enumerable_Dispose()
        {
            var scheduler = new TestScheduler();

            var o1 = scheduler.CreateHotObservable(
                OnNext(150, 1),
                OnNext(210, 2),
                OnNext(220, 3),
                OnError<int>(230, new Exception())
            );

            var o2 = scheduler.CreateHotObservable(
                OnNext(150, 1),
                OnNext(200, 2),
                OnNext(210, 3),
                OnNext(240, 4),
                OnNext(270, 5),
                OnNext(320, 6),
                OnNext(330, 7),
                OnCompleted<int>(340)
            );

            var xss = new MockEnumerable<ITestableObservable<int>>(scheduler, new[] { o1, o2 });

            var res = scheduler.Start(() =>
                xss.Select(xs => (IObservable<int>)xs).Catch(),
                300
            );

            res.Messages.AssertEqual(
                OnNext(210, 2),
                OnNext(220, 3),
                OnNext(240, 4),
                OnNext(270, 5)
            );

            o1.Subscriptions.AssertEqual(
                Subscribe(200, 230)
            );

            o2.Subscriptions.AssertEqual(
                Subscribe(230, 300)
            );

            xss.Subscriptions.AssertEqual(
                Subscribe(200, 300)
            );
        }

#if !NO_PERF
        [TestMethod]
        public void Catch_TailRecursive1()
        {
            var create = 0L;
            var start = 200L;
            var end = 1000L;

            var scheduler = new TestScheduler();

            var o = scheduler.CreateColdObservable<int>(
                OnNext(10, 1),
                OnNext(20, 2),
                OnNext(30, 3),
                OnError<int>(40, new Exception())
            );

            var f = default(Func<IObservable<int>>);
            f = () => Observable.Defer(() => o.Catch(f()));

            var res = scheduler.Start(() => f(), create, start, end);

            var expected = new List<Recorded<Notification<int>>>();

            var t = start;
            while (t <= end)
            {
                var n = (t - start) / 10;
                if (n % 4 != 0)
                {
                    expected.Add(OnNext(t, (int)(n % 4)));
                }

                t += 10;
            }

            res.Messages.AssertEqual(expected);
        }

        [TestMethod]
        public void Catch_TailRecursive2()
        {
            var f = default(Func<int, IObservable<int>>);
            f = x => Observable.Defer(() => Observable.Throw<int>(new Exception(), ThreadPoolScheduler.Instance).StartWith(x).Catch(f(x + 1)));

            var lst = new List<int>();
            f(0).Select(x => new StackTrace().FrameCount).Take(10).ForEach(lst.Add);

            Assert.IsTrue(lst.Last() - lst.First() < 10);
        }
#endif

        #endregion

        #region + CombineLatest +

        #region ArgumentChecking

        [TestMethod]
        public void CombineLatest_ArgumentChecking()
        {
            ReactiveAssert.Throws<ArgumentNullException>(() => Observable.CombineLatest<int, int, int>(DummyObservable<int>.Instance, DummyObservable<int>.Instance, null));
            ReactiveAssert.Throws<ArgumentNullException>(() => Observable.CombineLatest<int, int, int>(null, DummyObservable<int>.Instance, (_, __) => 0));
            ReactiveAssert.Throws<ArgumentNullException>(() => Observable.CombineLatest<int, int, int>(DummyObservable<int>.Instance, null, (_, __) => 0));
        }

        [TestMethod]
        public void CombineLatest_ArgumentCheckingHighArity()
        {
            var xs = DummyObservable<int>.Instance;

            ReactiveAssert.Throws<ArgumentNullException>(() => Observable.CombineLatest(default(IObservable<int>), xs, (_0, _1) => _0 + _1));
            ReactiveAssert.Throws<ArgumentNullException>(() => Observable.CombineLatest(xs, default(IObservable<int>), (_0, _1) => _0 + _1));
            ReactiveAssert.Throws<ArgumentNullException>(() => Observable.CombineLatest(xs, xs, default(Func<int, int, int>)));
            ReactiveAssert.Throws<ArgumentNullException>(() => Observable.CombineLatest(default(IObservable<int>), xs, xs, (_0, _1, _2) => _0 + _1 + _2));
            ReactiveAssert.Throws<ArgumentNullException>(() => Observable.CombineLatest(xs, default(IObservable<int>), xs, (_0, _1, _2) => _0 + _1 + _2));
            ReactiveAssert.Throws<ArgumentNullException>(() => Observable.CombineLatest(xs, xs, default(IObservable<int>), (_0, _1, _2) => _0 + _1 + _2));
            ReactiveAssert.Throws<ArgumentNullException>(() => Observable.CombineLatest(xs, xs, xs, default(Func<int, int, int, int>)));
            ReactiveAssert.Throws<ArgumentNullException>(() => Observable.CombineLatest(default(IObservable<int>), xs, xs, xs, (_0, _1, _2, _3) => _0 + _1 + _2 + _3));
            ReactiveAssert.Throws<ArgumentNullException>(() => Observable.CombineLatest(xs, default(IObservable<int>), xs, xs, (_0, _1, _2, _3) => _0 + _1 + _2 + _3));
            ReactiveAssert.Throws<ArgumentNullException>(() => Observable.CombineLatest(xs, xs, default(IObservable<int>), xs, (_0, _1, _2, _3) => _0 + _1 + _2 + _3));
            ReactiveAssert.Throws<ArgumentNullException>(() => Observable.CombineLatest(xs, xs, xs, default(IObservable<int>), (_0, _1, _2, _3) => _0 + _1 + _2 + _3));
            ReactiveAssert.Throws<ArgumentNullException>(() => Observable.CombineLatest(xs, xs, xs, xs, default(Func<int, int, int, int, int>)));
#if !NO_LARGEARITY
            ReactiveAssert.Throws<ArgumentNullException>(() => Observable.CombineLatest(default(IObservable<int>), xs, xs, xs, xs, (_0, _1, _2, _3, _4) => _0 + _1 + _2 + _3 + _4));
            ReactiveAssert.Throws<ArgumentNullException>(() => Observable.CombineLatest(xs, default(IObservable<int>), xs, xs, xs, (_0, _1, _2, _3, _4) => _0 + _1 + _2 + _3 + _4));
            ReactiveAssert.Throws<ArgumentNullException>(() => Observable.CombineLatest(xs, xs, default(IObservable<int>), xs, xs, (_0, _1, _2, _3, _4) => _0 + _1 + _2 + _3 + _4));
            ReactiveAssert.Throws<ArgumentNullException>(() => Observable.CombineLatest(xs, xs, xs, default(IObservable<int>), xs, (_0, _1, _2, _3, _4) => _0 + _1 + _2 + _3 + _4));
            ReactiveAssert.Throws<ArgumentNullException>(() => Observable.CombineLatest(xs, xs, xs, xs, default(IObservable<int>), (_0, _1, _2, _3, _4) => _0 + _1 + _2 + _3 + _4));
            ReactiveAssert.Throws<ArgumentNullException>(() => Observable.CombineLatest(xs, xs, xs, xs, xs, default(Func<int, int, int, int, int, int>)));
            ReactiveAssert.Throws<ArgumentNullException>(() => Observable.CombineLatest(default(IObservable<int>), xs, xs, xs, xs, xs, (_0, _1, _2, _3, _4, _5) => _0 + _1 + _2 + _3 + _4 + _5));
            ReactiveAssert.Throws<ArgumentNullException>(() => Observable.CombineLatest(xs, default(IObservable<int>), xs, xs, xs, xs, (_0, _1, _2, _3, _4, _5) => _0 + _1 + _2 + _3 + _4 + _5));
            ReactiveAssert.Throws<ArgumentNullException>(() => Observable.CombineLatest(xs, xs, default(IObservable<int>), xs, xs, xs, (_0, _1, _2, _3, _4, _5) => _0 + _1 + _2 + _3 + _4 + _5));
            ReactiveAssert.Throws<ArgumentNullException>(() => Observable.CombineLatest(xs, xs, xs, default(IObservable<int>), xs, xs, (_0, _1, _2, _3, _4, _5) => _0 + _1 + _2 + _3 + _4 + _5));
            ReactiveAssert.Throws<ArgumentNullException>(() => Observable.CombineLatest(xs, xs, xs, xs, default(IObservable<int>), xs, (_0, _1, _2, _3, _4, _5) => _0 + _1 + _2 + _3 + _4 + _5));
            ReactiveAssert.Throws<ArgumentNullException>(() => Observable.CombineLatest(xs, xs, xs, xs, xs, default(IObservable<int>), (_0, _1, _2, _3, _4, _5) => _0 + _1 + _2 + _3 + _4 + _5));
            ReactiveAssert.Throws<ArgumentNullException>(() => Observable.CombineLatest(xs, xs, xs, xs, xs, xs, default(Func<int, int, int, int, int, int, int>)));
            ReactiveAssert.Throws<ArgumentNullException>(() => Observable.CombineLatest(default(IObservable<int>), xs, xs, xs, xs, xs, xs, (_0, _1, _2, _3, _4, _5, _6) => _0 + _1 + _2 + _3 + _4 + _5 + _6));
            ReactiveAssert.Throws<ArgumentNullException>(() => Observable.CombineLatest(xs, default(IObservable<int>), xs, xs, xs, xs, xs, (_0, _1, _2, _3, _4, _5, _6) => _0 + _1 + _2 + _3 + _4 + _5 + _6));
            ReactiveAssert.Throws<ArgumentNullException>(() => Observable.CombineLatest(xs, xs, default(IObservable<int>), xs, xs, xs, xs, (_0, _1, _2, _3, _4, _5, _6) => _0 + _1 + _2 + _3 + _4 + _5 + _6));
            ReactiveAssert.Throws<ArgumentNullException>(() => Observable.CombineLatest(xs, xs, xs, default(IObservable<int>), xs, xs, xs, (_0, _1, _2, _3, _4, _5, _6) => _0 + _1 + _2 + _3 + _4 + _5 + _6));
            ReactiveAssert.Throws<ArgumentNullException>(() => Observable.CombineLatest(xs, xs, xs, xs, default(IObservable<int>), xs, xs, (_0, _1, _2, _3, _4, _5, _6) => _0 + _1 + _2 + _3 + _4 + _5 + _6));
            ReactiveAssert.Throws<ArgumentNullException>(() => Observable.CombineLatest(xs, xs, xs, xs, xs, default(IObservable<int>), xs, (_0, _1, _2, _3, _4, _5, _6) => _0 + _1 + _2 + _3 + _4 + _5 + _6));
            ReactiveAssert.Throws<ArgumentNullException>(() => Observable.CombineLatest(xs, xs, xs, xs, xs, xs, default(IObservable<int>), (_0, _1, _2, _3, _4, _5, _6) => _0 + _1 + _2 + _3 + _4 + _5 + _6));
            ReactiveAssert.Throws<ArgumentNullException>(() => Observable.CombineLatest(xs, xs, xs, xs, xs, xs, xs, default(Func<int, int, int, int, int, int, int, int>)));
            ReactiveAssert.Throws<ArgumentNullException>(() => Observable.CombineLatest(default(IObservable<int>), xs, xs, xs, xs, xs, xs, xs, (_0, _1, _2, _3, _4, _5, _6, _7) => _0 + _1 + _2 + _3 + _4 + _5 + _6 + _7));
            ReactiveAssert.Throws<ArgumentNullException>(() => Observable.CombineLatest(xs, default(IObservable<int>), xs, xs, xs, xs, xs, xs, (_0, _1, _2, _3, _4, _5, _6, _7) => _0 + _1 + _2 + _3 + _4 + _5 + _6 + _7));
            ReactiveAssert.Throws<ArgumentNullException>(() => Observable.CombineLatest(xs, xs, default(IObservable<int>), xs, xs, xs, xs, xs, (_0, _1, _2, _3, _4, _5, _6, _7) => _0 + _1 + _2 + _3 + _4 + _5 + _6 + _7));
            ReactiveAssert.Throws<ArgumentNullException>(() => Observable.CombineLatest(xs, xs, xs, default(IObservable<int>), xs, xs, xs, xs, (_0, _1, _2, _3, _4, _5, _6, _7) => _0 + _1 + _2 + _3 + _4 + _5 + _6 + _7));
            ReactiveAssert.Throws<ArgumentNullException>(() => Observable.CombineLatest(xs, xs, xs, xs, default(IObservable<int>), xs, xs, xs, (_0, _1, _2, _3, _4, _5, _6, _7) => _0 + _1 + _2 + _3 + _4 + _5 + _6 + _7));
            ReactiveAssert.Throws<ArgumentNullException>(() => Observable.CombineLatest(xs, xs, xs, xs, xs, default(IObservable<int>), xs, xs, (_0, _1, _2, _3, _4, _5, _6, _7) => _0 + _1 + _2 + _3 + _4 + _5 + _6 + _7));
            ReactiveAssert.Throws<ArgumentNullException>(() => Observable.CombineLatest(xs, xs, xs, xs, xs, xs, default(IObservable<int>), xs, (_0, _1, _2, _3, _4, _5, _6, _7) => _0 + _1 + _2 + _3 + _4 + _5 + _6 + _7));
            ReactiveAssert.Throws<ArgumentNullException>(() => Observable.CombineLatest(xs, xs, xs, xs, xs, xs, xs, default(IObservable<int>), (_0, _1, _2, _3, _4, _5, _6, _7) => _0 + _1 + _2 + _3 + _4 + _5 + _6 + _7));
            ReactiveAssert.Throws<ArgumentNullException>(() => Observable.CombineLatest(xs, xs, xs, xs, xs, xs, xs, xs, default(Func<int, int, int, int, int, int, int, int, int>)));
            ReactiveAssert.Throws<ArgumentNullException>(() => Observable.CombineLatest(default(IObservable<int>), xs, xs, xs, xs, xs, xs, xs, xs, (_0, _1, _2, _3, _4, _5, _6, _7, _8) => _0 + _1 + _2 + _3 + _4 + _5 + _6 + _7 + _8));
            ReactiveAssert.Throws<ArgumentNullException>(() => Observable.CombineLatest(xs, default(IObservable<int>), xs, xs, xs, xs, xs, xs, xs, (_0, _1, _2, _3, _4, _5, _6, _7, _8) => _0 + _1 + _2 + _3 + _4 + _5 + _6 + _7 + _8));
            ReactiveAssert.Throws<ArgumentNullException>(() => Observable.CombineLatest(xs, xs, default(IObservable<int>), xs, xs, xs, xs, xs, xs, (_0, _1, _2, _3, _4, _5, _6, _7, _8) => _0 + _1 + _2 + _3 + _4 + _5 + _6 + _7 + _8));
            ReactiveAssert.Throws<ArgumentNullException>(() => Observable.CombineLatest(xs, xs, xs, default(IObservable<int>), xs, xs, xs, xs, xs, (_0, _1, _2, _3, _4, _5, _6, _7, _8) => _0 + _1 + _2 + _3 + _4 + _5 + _6 + _7 + _8));
            ReactiveAssert.Throws<ArgumentNullException>(() => Observable.CombineLatest(xs, xs, xs, xs, default(IObservable<int>), xs, xs, xs, xs, (_0, _1, _2, _3, _4, _5, _6, _7, _8) => _0 + _1 + _2 + _3 + _4 + _5 + _6 + _7 + _8));
            ReactiveAssert.Throws<ArgumentNullException>(() => Observable.CombineLatest(xs, xs, xs, xs, xs, default(IObservable<int>), xs, xs, xs, (_0, _1, _2, _3, _4, _5, _6, _7, _8) => _0 + _1 + _2 + _3 + _4 + _5 + _6 + _7 + _8));
            ReactiveAssert.Throws<ArgumentNullException>(() => Observable.CombineLatest(xs, xs, xs, xs, xs, xs, default(IObservable<int>), xs, xs, (_0, _1, _2, _3, _4, _5, _6, _7, _8) => _0 + _1 + _2 + _3 + _4 + _5 + _6 + _7 + _8));
            ReactiveAssert.Throws<ArgumentNullException>(() => Observable.CombineLatest(xs, xs, xs, xs, xs, xs, xs, default(IObservable<int>), xs, (_0, _1, _2, _3, _4, _5, _6, _7, _8) => _0 + _1 + _2 + _3 + _4 + _5 + _6 + _7 + _8));
            ReactiveAssert.Throws<ArgumentNullException>(() => Observable.CombineLatest(xs, xs, xs, xs, xs, xs, xs, xs, default(IObservable<int>), (_0, _1, _2, _3, _4, _5, _6, _7, _8) => _0 + _1 + _2 + _3 + _4 + _5 + _6 + _7 + _8));
            ReactiveAssert.Throws<ArgumentNullException>(() => Observable.CombineLatest(xs, xs, xs, xs, xs, xs, xs, xs, xs, default(Func<int, int, int, int, int, int, int, int, int, int>)));
            ReactiveAssert.Throws<ArgumentNullException>(() => Observable.CombineLatest(default(IObservable<int>), xs, xs, xs, xs, xs, xs, xs, xs, xs, (_0, _1, _2, _3, _4, _5, _6, _7, _8, _9) => _0 + _1 + _2 + _3 + _4 + _5 + _6 + _7 + _8 + _9));
            ReactiveAssert.Throws<ArgumentNullException>(() => Observable.CombineLatest(xs, default(IObservable<int>), xs, xs, xs, xs, xs, xs, xs, xs, (_0, _1, _2, _3, _4, _5, _6, _7, _8, _9) => _0 + _1 + _2 + _3 + _4 + _5 + _6 + _7 + _8 + _9));
            ReactiveAssert.Throws<ArgumentNullException>(() => Observable.CombineLatest(xs, xs, default(IObservable<int>), xs, xs, xs, xs, xs, xs, xs, (_0, _1, _2, _3, _4, _5, _6, _7, _8, _9) => _0 + _1 + _2 + _3 + _4 + _5 + _6 + _7 + _8 + _9));
            ReactiveAssert.Throws<ArgumentNullException>(() => Observable.CombineLatest(xs, xs, xs, default(IObservable<int>), xs, xs, xs, xs, xs, xs, (_0, _1, _2, _3, _4, _5, _6, _7, _8, _9) => _0 + _1 + _2 + _3 + _4 + _5 + _6 + _7 + _8 + _9));
            ReactiveAssert.Throws<ArgumentNullException>(() => Observable.CombineLatest(xs, xs, xs, xs, default(IObservable<int>), xs, xs, xs, xs, xs, (_0, _1, _2, _3, _4, _5, _6, _7, _8, _9) => _0 + _1 + _2 + _3 + _4 + _5 + _6 + _7 + _8 + _9));
            ReactiveAssert.Throws<ArgumentNullException>(() => Observable.CombineLatest(xs, xs, xs, xs, xs, default(IObservable<int>), xs, xs, xs, xs, (_0, _1, _2, _3, _4, _5, _6, _7, _8, _9) => _0 + _1 + _2 + _3 + _4 + _5 + _6 + _7 + _8 + _9));
            ReactiveAssert.Throws<ArgumentNullException>(() => Observable.CombineLatest(xs, xs, xs, xs, xs, xs, default(IObservable<int>), xs, xs, xs, (_0, _1, _2, _3, _4, _5, _6, _7, _8, _9) => _0 + _1 + _2 + _3 + _4 + _5 + _6 + _7 + _8 + _9));
            ReactiveAssert.Throws<ArgumentNullException>(() => Observable.CombineLatest(xs, xs, xs, xs, xs, xs, xs, default(IObservable<int>), xs, xs, (_0, _1, _2, _3, _4, _5, _6, _7, _8, _9) => _0 + _1 + _2 + _3 + _4 + _5 + _6 + _7 + _8 + _9));
            ReactiveAssert.Throws<ArgumentNullException>(() => Observable.CombineLatest(xs, xs, xs, xs, xs, xs, xs, xs, default(IObservable<int>), xs, (_0, _1, _2, _3, _4, _5, _6, _7, _8, _9) => _0 + _1 + _2 + _3 + _4 + _5 + _6 + _7 + _8 + _9));
            ReactiveAssert.Throws<ArgumentNullException>(() => Observable.CombineLatest(xs, xs, xs, xs, xs, xs, xs, xs, xs, default(IObservable<int>), (_0, _1, _2, _3, _4, _5, _6, _7, _8, _9) => _0 + _1 + _2 + _3 + _4 + _5 + _6 + _7 + _8 + _9));
            ReactiveAssert.Throws<ArgumentNullException>(() => Observable.CombineLatest(xs, xs, xs, xs, xs, xs, xs, xs, xs, xs, default(Func<int, int, int, int, int, int, int, int, int, int, int>)));
            ReactiveAssert.Throws<ArgumentNullException>(() => Observable.CombineLatest(default(IObservable<int>), xs, xs, xs, xs, xs, xs, xs, xs, xs, xs, (_0, _1, _2, _3, _4, _5, _6, _7, _8, _9, _10) => _0 + _1 + _2 + _3 + _4 + _5 + _6 + _7 + _8 + _9 + _10));
            ReactiveAssert.Throws<ArgumentNullException>(() => Observable.CombineLatest(xs, default(IObservable<int>), xs, xs, xs, xs, xs, xs, xs, xs, xs, (_0, _1, _2, _3, _4, _5, _6, _7, _8, _9, _10) => _0 + _1 + _2 + _3 + _4 + _5 + _6 + _7 + _8 + _9 + _10));
            ReactiveAssert.Throws<ArgumentNullException>(() => Observable.CombineLatest(xs, xs, default(IObservable<int>), xs, xs, xs, xs, xs, xs, xs, xs, (_0, _1, _2, _3, _4, _5, _6, _7, _8, _9, _10) => _0 + _1 + _2 + _3 + _4 + _5 + _6 + _7 + _8 + _9 + _10));
            ReactiveAssert.Throws<ArgumentNullException>(() => Observable.CombineLatest(xs, xs, xs, default(IObservable<int>), xs, xs, xs, xs, xs, xs, xs, (_0, _1, _2, _3, _4, _5, _6, _7, _8, _9, _10) => _0 + _1 + _2 + _3 + _4 + _5 + _6 + _7 + _8 + _9 + _10));
            ReactiveAssert.Throws<ArgumentNullException>(() => Observable.CombineLatest(xs, xs, xs, xs, default(IObservable<int>), xs, xs, xs, xs, xs, xs, (_0, _1, _2, _3, _4, _5, _6, _7, _8, _9, _10) => _0 + _1 + _2 + _3 + _4 + _5 + _6 + _7 + _8 + _9 + _10));
            ReactiveAssert.Throws<ArgumentNullException>(() => Observable.CombineLatest(xs, xs, xs, xs, xs, default(IObservable<int>), xs, xs, xs, xs, xs, (_0, _1, _2, _3, _4, _5, _6, _7, _8, _9, _10) => _0 + _1 + _2 + _3 + _4 + _5 + _6 + _7 + _8 + _9 + _10));
            ReactiveAssert.Throws<ArgumentNullException>(() => Observable.CombineLatest(xs, xs, xs, xs, xs, xs, default(IObservable<int>), xs, xs, xs, xs, (_0, _1, _2, _3, _4, _5, _6, _7, _8, _9, _10) => _0 + _1 + _2 + _3 + _4 + _5 + _6 + _7 + _8 + _9 + _10));
            ReactiveAssert.Throws<ArgumentNullException>(() => Observable.CombineLatest(xs, xs, xs, xs, xs, xs, xs, default(IObservable<int>), xs, xs, xs, (_0, _1, _2, _3, _4, _5, _6, _7, _8, _9, _10) => _0 + _1 + _2 + _3 + _4 + _5 + _6 + _7 + _8 + _9 + _10));
            ReactiveAssert.Throws<ArgumentNullException>(() => Observable.CombineLatest(xs, xs, xs, xs, xs, xs, xs, xs, default(IObservable<int>), xs, xs, (_0, _1, _2, _3, _4, _5, _6, _7, _8, _9, _10) => _0 + _1 + _2 + _3 + _4 + _5 + _6 + _7 + _8 + _9 + _10));
            ReactiveAssert.Throws<ArgumentNullException>(() => Observable.CombineLatest(xs, xs, xs, xs, xs, xs, xs, xs, xs, default(IObservable<int>), xs, (_0, _1, _2, _3, _4, _5, _6, _7, _8, _9, _10) => _0 + _1 + _2 + _3 + _4 + _5 + _6 + _7 + _8 + _9 + _10));
            ReactiveAssert.Throws<ArgumentNullException>(() => Observable.CombineLatest(xs, xs, xs, xs, xs, xs, xs, xs, xs, xs, default(IObservable<int>), (_0, _1, _2, _3, _4, _5, _6, _7, _8, _9, _10) => _0 + _1 + _2 + _3 + _4 + _5 + _6 + _7 + _8 + _9 + _10));
            ReactiveAssert.Throws<ArgumentNullException>(() => Observable.CombineLatest(xs, xs, xs, xs, xs, xs, xs, xs, xs, xs, xs, default(Func<int, int, int, int, int, int, int, int, int, int, int, int>)));
            ReactiveAssert.Throws<ArgumentNullException>(() => Observable.CombineLatest(default(IObservable<int>), xs, xs, xs, xs, xs, xs, xs, xs, xs, xs, xs, (_0, _1, _2, _3, _4, _5, _6, _7, _8, _9, _10, _11) => _0 + _1 + _2 + _3 + _4 + _5 + _6 + _7 + _8 + _9 + _10 + _11));
            ReactiveAssert.Throws<ArgumentNullException>(() => Observable.CombineLatest(xs, default(IObservable<int>), xs, xs, xs, xs, xs, xs, xs, xs, xs, xs, (_0, _1, _2, _3, _4, _5, _6, _7, _8, _9, _10, _11) => _0 + _1 + _2 + _3 + _4 + _5 + _6 + _7 + _8 + _9 + _10 + _11));
            ReactiveAssert.Throws<ArgumentNullException>(() => Observable.CombineLatest(xs, xs, default(IObservable<int>), xs, xs, xs, xs, xs, xs, xs, xs, xs, (_0, _1, _2, _3, _4, _5, _6, _7, _8, _9, _10, _11) => _0 + _1 + _2 + _3 + _4 + _5 + _6 + _7 + _8 + _9 + _10 + _11));
            ReactiveAssert.Throws<ArgumentNullException>(() => Observable.CombineLatest(xs, xs, xs, default(IObservable<int>), xs, xs, xs, xs, xs, xs, xs, xs, (_0, _1, _2, _3, _4, _5, _6, _7, _8, _9, _10, _11) => _0 + _1 + _2 + _3 + _4 + _5 + _6 + _7 + _8 + _9 + _10 + _11));
            ReactiveAssert.Throws<ArgumentNullException>(() => Observable.CombineLatest(xs, xs, xs, xs, default(IObservable<int>), xs, xs, xs, xs, xs, xs, xs, (_0, _1, _2, _3, _4, _5, _6, _7, _8, _9, _10, _11) => _0 + _1 + _2 + _3 + _4 + _5 + _6 + _7 + _8 + _9 + _10 + _11));
            ReactiveAssert.Throws<ArgumentNullException>(() => Observable.CombineLatest(xs, xs, xs, xs, xs, default(IObservable<int>), xs, xs, xs, xs, xs, xs, (_0, _1, _2, _3, _4, _5, _6, _7, _8, _9, _10, _11) => _0 + _1 + _2 + _3 + _4 + _5 + _6 + _7 + _8 + _9 + _10 + _11));
            ReactiveAssert.Throws<ArgumentNullException>(() => Observable.CombineLatest(xs, xs, xs, xs, xs, xs, default(IObservable<int>), xs, xs, xs, xs, xs, (_0, _1, _2, _3, _4, _5, _6, _7, _8, _9, _10, _11) => _0 + _1 + _2 + _3 + _4 + _5 + _6 + _7 + _8 + _9 + _10 + _11));
            ReactiveAssert.Throws<ArgumentNullException>(() => Observable.CombineLatest(xs, xs, xs, xs, xs, xs, xs, default(IObservable<int>), xs, xs, xs, xs, (_0, _1, _2, _3, _4, _5, _6, _7, _8, _9, _10, _11) => _0 + _1 + _2 + _3 + _4 + _5 + _6 + _7 + _8 + _9 + _10 + _11));
            ReactiveAssert.Throws<ArgumentNullException>(() => Observable.CombineLatest(xs, xs, xs, xs, xs, xs, xs, xs, default(IObservable<int>), xs, xs, xs, (_0, _1, _2, _3, _4, _5, _6, _7, _8, _9, _10, _11) => _0 + _1 + _2 + _3 + _4 + _5 + _6 + _7 + _8 + _9 + _10 + _11));
            ReactiveAssert.Throws<ArgumentNullException>(() => Observable.CombineLatest(xs, xs, xs, xs, xs, xs, xs, xs, xs, default(IObservable<int>), xs, xs, (_0, _1, _2, _3, _4, _5, _6, _7, _8, _9, _10, _11) => _0 + _1 + _2 + _3 + _4 + _5 + _6 + _7 + _8 + _9 + _10 + _11));
            ReactiveAssert.Throws<ArgumentNullException>(() => Observable.CombineLatest(xs, xs, xs, xs, xs, xs, xs, xs, xs, xs, default(IObservable<int>), xs, (_0, _1, _2, _3, _4, _5, _6, _7, _8, _9, _10, _11) => _0 + _1 + _2 + _3 + _4 + _5 + _6 + _7 + _8 + _9 + _10 + _11));
            ReactiveAssert.Throws<ArgumentNullException>(() => Observable.CombineLatest(xs, xs, xs, xs, xs, xs, xs, xs, xs, xs, xs, default(IObservable<int>), (_0, _1, _2, _3, _4, _5, _6, _7, _8, _9, _10, _11) => _0 + _1 + _2 + _3 + _4 + _5 + _6 + _7 + _8 + _9 + _10 + _11));
            ReactiveAssert.Throws<ArgumentNullException>(() => Observable.CombineLatest(xs, xs, xs, xs, xs, xs, xs, xs, xs, xs, xs, xs, default(Func<int, int, int, int, int, int, int, int, int, int, int, int, int>)));
            ReactiveAssert.Throws<ArgumentNullException>(() => Observable.CombineLatest(default(IObservable<int>), xs, xs, xs, xs, xs, xs, xs, xs, xs, xs, xs, xs, (_0, _1, _2, _3, _4, _5, _6, _7, _8, _9, _10, _11, _12) => _0 + _1 + _2 + _3 + _4 + _5 + _6 + _7 + _8 + _9 + _10 + _11 + _12));
            ReactiveAssert.Throws<ArgumentNullException>(() => Observable.CombineLatest(xs, default(IObservable<int>), xs, xs, xs, xs, xs, xs, xs, xs, xs, xs, xs, (_0, _1, _2, _3, _4, _5, _6, _7, _8, _9, _10, _11, _12) => _0 + _1 + _2 + _3 + _4 + _5 + _6 + _7 + _8 + _9 + _10 + _11 + _12));
            ReactiveAssert.Throws<ArgumentNullException>(() => Observable.CombineLatest(xs, xs, default(IObservable<int>), xs, xs, xs, xs, xs, xs, xs, xs, xs, xs, (_0, _1, _2, _3, _4, _5, _6, _7, _8, _9, _10, _11, _12) => _0 + _1 + _2 + _3 + _4 + _5 + _6 + _7 + _8 + _9 + _10 + _11 + _12));
            ReactiveAssert.Throws<ArgumentNullException>(() => Observable.CombineLatest(xs, xs, xs, default(IObservable<int>), xs, xs, xs, xs, xs, xs, xs, xs, xs, (_0, _1, _2, _3, _4, _5, _6, _7, _8, _9, _10, _11, _12) => _0 + _1 + _2 + _3 + _4 + _5 + _6 + _7 + _8 + _9 + _10 + _11 + _12));
            ReactiveAssert.Throws<ArgumentNullException>(() => Observable.CombineLatest(xs, xs, xs, xs, default(IObservable<int>), xs, xs, xs, xs, xs, xs, xs, xs, (_0, _1, _2, _3, _4, _5, _6, _7, _8, _9, _10, _11, _12) => _0 + _1 + _2 + _3 + _4 + _5 + _6 + _7 + _8 + _9 + _10 + _11 + _12));
            ReactiveAssert.Throws<ArgumentNullException>(() => Observable.CombineLatest(xs, xs, xs, xs, xs, default(IObservable<int>), xs, xs, xs, xs, xs, xs, xs, (_0, _1, _2, _3, _4, _5, _6, _7, _8, _9, _10, _11, _12) => _0 + _1 + _2 + _3 + _4 + _5 + _6 + _7 + _8 + _9 + _10 + _11 + _12));
            ReactiveAssert.Throws<ArgumentNullException>(() => Observable.CombineLatest(xs, xs, xs, xs, xs, xs, default(IObservable<int>), xs, xs, xs, xs, xs, xs, (_0, _1, _2, _3, _4, _5, _6, _7, _8, _9, _10, _11, _12) => _0 + _1 + _2 + _3 + _4 + _5 + _6 + _7 + _8 + _9 + _10 + _11 + _12));
            ReactiveAssert.Throws<ArgumentNullException>(() => Observable.CombineLatest(xs, xs, xs, xs, xs, xs, xs, default(IObservable<int>), xs, xs, xs, xs, xs, (_0, _1, _2, _3, _4, _5, _6, _7, _8, _9, _10, _11, _12) => _0 + _1 + _2 + _3 + _4 + _5 + _6 + _7 + _8 + _9 + _10 + _11 + _12));
            ReactiveAssert.Throws<ArgumentNullException>(() => Observable.CombineLatest(xs, xs, xs, xs, xs, xs, xs, xs, default(IObservable<int>), xs, xs, xs, xs, (_0, _1, _2, _3, _4, _5, _6, _7, _8, _9, _10, _11, _12) => _0 + _1 + _2 + _3 + _4 + _5 + _6 + _7 + _8 + _9 + _10 + _11 + _12));
            ReactiveAssert.Throws<ArgumentNullException>(() => Observable.CombineLatest(xs, xs, xs, xs, xs, xs, xs, xs, xs, default(IObservable<int>), xs, xs, xs, (_0, _1, _2, _3, _4, _5, _6, _7, _8, _9, _10, _11, _12) => _0 + _1 + _2 + _3 + _4 + _5 + _6 + _7 + _8 + _9 + _10 + _11 + _12));
            ReactiveAssert.Throws<ArgumentNullException>(() => Observable.CombineLatest(xs, xs, xs, xs, xs, xs, xs, xs, xs, xs, default(IObservable<int>), xs, xs, (_0, _1, _2, _3, _4, _5, _6, _7, _8, _9, _10, _11, _12) => _0 + _1 + _2 + _3 + _4 + _5 + _6 + _7 + _8 + _9 + _10 + _11 + _12));
            ReactiveAssert.Throws<ArgumentNullException>(() => Observable.CombineLatest(xs, xs, xs, xs, xs, xs, xs, xs, xs, xs, xs, default(IObservable<int>), xs, (_0, _1, _2, _3, _4, _5, _6, _7, _8, _9, _10, _11, _12) => _0 + _1 + _2 + _3 + _4 + _5 + _6 + _7 + _8 + _9 + _10 + _11 + _12));
            ReactiveAssert.Throws<ArgumentNullException>(() => Observable.CombineLatest(xs, xs, xs, xs, xs, xs, xs, xs, xs, xs, xs, xs, default(IObservable<int>), (_0, _1, _2, _3, _4, _5, _6, _7, _8, _9, _10, _11, _12) => _0 + _1 + _2 + _3 + _4 + _5 + _6 + _7 + _8 + _9 + _10 + _11 + _12));
            ReactiveAssert.Throws<ArgumentNullException>(() => Observable.CombineLatest(xs, xs, xs, xs, xs, xs, xs, xs, xs, xs, xs, xs, xs, default(Func<int, int, int, int, int, int, int, int, int, int, int, int, int, int>)));
            ReactiveAssert.Throws<ArgumentNullException>(() => Observable.CombineLatest(default(IObservable<int>), xs, xs, xs, xs, xs, xs, xs, xs, xs, xs, xs, xs, xs, (_0, _1, _2, _3, _4, _5, _6, _7, _8, _9, _10, _11, _12, _13) => _0 + _1 + _2 + _3 + _4 + _5 + _6 + _7 + _8 + _9 + _10 + _11 + _12 + _13));
            ReactiveAssert.Throws<ArgumentNullException>(() => Observable.CombineLatest(xs, default(IObservable<int>), xs, xs, xs, xs, xs, xs, xs, xs, xs, xs, xs, xs, (_0, _1, _2, _3, _4, _5, _6, _7, _8, _9, _10, _11, _12, _13) => _0 + _1 + _2 + _3 + _4 + _5 + _6 + _7 + _8 + _9 + _10 + _11 + _12 + _13));
            ReactiveAssert.Throws<ArgumentNullException>(() => Observable.CombineLatest(xs, xs, default(IObservable<int>), xs, xs, xs, xs, xs, xs, xs, xs, xs, xs, xs, (_0, _1, _2, _3, _4, _5, _6, _7, _8, _9, _10, _11, _12, _13) => _0 + _1 + _2 + _3 + _4 + _5 + _6 + _7 + _8 + _9 + _10 + _11 + _12 + _13));
            ReactiveAssert.Throws<ArgumentNullException>(() => Observable.CombineLatest(xs, xs, xs, default(IObservable<int>), xs, xs, xs, xs, xs, xs, xs, xs, xs, xs, (_0, _1, _2, _3, _4, _5, _6, _7, _8, _9, _10, _11, _12, _13) => _0 + _1 + _2 + _3 + _4 + _5 + _6 + _7 + _8 + _9 + _10 + _11 + _12 + _13));
            ReactiveAssert.Throws<ArgumentNullException>(() => Observable.CombineLatest(xs, xs, xs, xs, default(IObservable<int>), xs, xs, xs, xs, xs, xs, xs, xs, xs, (_0, _1, _2, _3, _4, _5, _6, _7, _8, _9, _10, _11, _12, _13) => _0 + _1 + _2 + _3 + _4 + _5 + _6 + _7 + _8 + _9 + _10 + _11 + _12 + _13));
            ReactiveAssert.Throws<ArgumentNullException>(() => Observable.CombineLatest(xs, xs, xs, xs, xs, default(IObservable<int>), xs, xs, xs, xs, xs, xs, xs, xs, (_0, _1, _2, _3, _4, _5, _6, _7, _8, _9, _10, _11, _12, _13) => _0 + _1 + _2 + _3 + _4 + _5 + _6 + _7 + _8 + _9 + _10 + _11 + _12 + _13));
            ReactiveAssert.Throws<ArgumentNullException>(() => Observable.CombineLatest(xs, xs, xs, xs, xs, xs, default(IObservable<int>), xs, xs, xs, xs, xs, xs, xs, (_0, _1, _2, _3, _4, _5, _6, _7, _8, _9, _10, _11, _12, _13) => _0 + _1 + _2 + _3 + _4 + _5 + _6 + _7 + _8 + _9 + _10 + _11 + _12 + _13));
            ReactiveAssert.Throws<ArgumentNullException>(() => Observable.CombineLatest(xs, xs, xs, xs, xs, xs, xs, default(IObservable<int>), xs, xs, xs, xs, xs, xs, (_0, _1, _2, _3, _4, _5, _6, _7, _8, _9, _10, _11, _12, _13) => _0 + _1 + _2 + _3 + _4 + _5 + _6 + _7 + _8 + _9 + _10 + _11 + _12 + _13));
            ReactiveAssert.Throws<ArgumentNullException>(() => Observable.CombineLatest(xs, xs, xs, xs, xs, xs, xs, xs, default(IObservable<int>), xs, xs, xs, xs, xs, (_0, _1, _2, _3, _4, _5, _6, _7, _8, _9, _10, _11, _12, _13) => _0 + _1 + _2 + _3 + _4 + _5 + _6 + _7 + _8 + _9 + _10 + _11 + _12 + _13));
            ReactiveAssert.Throws<ArgumentNullException>(() => Observable.CombineLatest(xs, xs, xs, xs, xs, xs, xs, xs, xs, default(IObservable<int>), xs, xs, xs, xs, (_0, _1, _2, _3, _4, _5, _6, _7, _8, _9, _10, _11, _12, _13) => _0 + _1 + _2 + _3 + _4 + _5 + _6 + _7 + _8 + _9 + _10 + _11 + _12 + _13));
            ReactiveAssert.Throws<ArgumentNullException>(() => Observable.CombineLatest(xs, xs, xs, xs, xs, xs, xs, xs, xs, xs, default(IObservable<int>), xs, xs, xs, (_0, _1, _2, _3, _4, _5, _6, _7, _8, _9, _10, _11, _12, _13) => _0 + _1 + _2 + _3 + _4 + _5 + _6 + _7 + _8 + _9 + _10 + _11 + _12 + _13));
            ReactiveAssert.Throws<ArgumentNullException>(() => Observable.CombineLatest(xs, xs, xs, xs, xs, xs, xs, xs, xs, xs, xs, default(IObservable<int>), xs, xs, (_0, _1, _2, _3, _4, _5, _6, _7, _8, _9, _10, _11, _12, _13) => _0 + _1 + _2 + _3 + _4 + _5 + _6 + _7 + _8 + _9 + _10 + _11 + _12 + _13));
            ReactiveAssert.Throws<ArgumentNullException>(() => Observable.CombineLatest(xs, xs, xs, xs, xs, xs, xs, xs, xs, xs, xs, xs, default(IObservable<int>), xs, (_0, _1, _2, _3, _4, _5, _6, _7, _8, _9, _10, _11, _12, _13) => _0 + _1 + _2 + _3 + _4 + _5 + _6 + _7 + _8 + _9 + _10 + _11 + _12 + _13));
            ReactiveAssert.Throws<ArgumentNullException>(() => Observable.CombineLatest(xs, xs, xs, xs, xs, xs, xs, xs, xs, xs, xs, xs, xs, default(IObservable<int>), (_0, _1, _2, _3, _4, _5, _6, _7, _8, _9, _10, _11, _12, _13) => _0 + _1 + _2 + _3 + _4 + _5 + _6 + _7 + _8 + _9 + _10 + _11 + _12 + _13));
            ReactiveAssert.Throws<ArgumentNullException>(() => Observable.CombineLatest(xs, xs, xs, xs, xs, xs, xs, xs, xs, xs, xs, xs, xs, xs, default(Func<int, int, int, int, int, int, int, int, int, int, int, int, int, int, int>)));
            ReactiveAssert.Throws<ArgumentNullException>(() => Observable.CombineLatest(default(IObservable<int>), xs, xs, xs, xs, xs, xs, xs, xs, xs, xs, xs, xs, xs, xs, (_0, _1, _2, _3, _4, _5, _6, _7, _8, _9, _10, _11, _12, _13, _14) => _0 + _1 + _2 + _3 + _4 + _5 + _6 + _7 + _8 + _9 + _10 + _11 + _12 + _13 + _14));
            ReactiveAssert.Throws<ArgumentNullException>(() => Observable.CombineLatest(xs, default(IObservable<int>), xs, xs, xs, xs, xs, xs, xs, xs, xs, xs, xs, xs, xs, (_0, _1, _2, _3, _4, _5, _6, _7, _8, _9, _10, _11, _12, _13, _14) => _0 + _1 + _2 + _3 + _4 + _5 + _6 + _7 + _8 + _9 + _10 + _11 + _12 + _13 + _14));
            ReactiveAssert.Throws<ArgumentNullException>(() => Observable.CombineLatest(xs, xs, default(IObservable<int>), xs, xs, xs, xs, xs, xs, xs, xs, xs, xs, xs, xs, (_0, _1, _2, _3, _4, _5, _6, _7, _8, _9, _10, _11, _12, _13, _14) => _0 + _1 + _2 + _3 + _4 + _5 + _6 + _7 + _8 + _9 + _10 + _11 + _12 + _13 + _14));
            ReactiveAssert.Throws<ArgumentNullException>(() => Observable.CombineLatest(xs, xs, xs, default(IObservable<int>), xs, xs, xs, xs, xs, xs, xs, xs, xs, xs, xs, (_0, _1, _2, _3, _4, _5, _6, _7, _8, _9, _10, _11, _12, _13, _14) => _0 + _1 + _2 + _3 + _4 + _5 + _6 + _7 + _8 + _9 + _10 + _11 + _12 + _13 + _14));
            ReactiveAssert.Throws<ArgumentNullException>(() => Observable.CombineLatest(xs, xs, xs, xs, default(IObservable<int>), xs, xs, xs, xs, xs, xs, xs, xs, xs, xs, (_0, _1, _2, _3, _4, _5, _6, _7, _8, _9, _10, _11, _12, _13, _14) => _0 + _1 + _2 + _3 + _4 + _5 + _6 + _7 + _8 + _9 + _10 + _11 + _12 + _13 + _14));
            ReactiveAssert.Throws<ArgumentNullException>(() => Observable.CombineLatest(xs, xs, xs, xs, xs, default(IObservable<int>), xs, xs, xs, xs, xs, xs, xs, xs, xs, (_0, _1, _2, _3, _4, _5, _6, _7, _8, _9, _10, _11, _12, _13, _14) => _0 + _1 + _2 + _3 + _4 + _5 + _6 + _7 + _8 + _9 + _10 + _11 + _12 + _13 + _14));
            ReactiveAssert.Throws<ArgumentNullException>(() => Observable.CombineLatest(xs, xs, xs, xs, xs, xs, default(IObservable<int>), xs, xs, xs, xs, xs, xs, xs, xs, (_0, _1, _2, _3, _4, _5, _6, _7, _8, _9, _10, _11, _12, _13, _14) => _0 + _1 + _2 + _3 + _4 + _5 + _6 + _7 + _8 + _9 + _10 + _11 + _12 + _13 + _14));
            ReactiveAssert.Throws<ArgumentNullException>(() => Observable.CombineLatest(xs, xs, xs, xs, xs, xs, xs, default(IObservable<int>), xs, xs, xs, xs, xs, xs, xs, (_0, _1, _2, _3, _4, _5, _6, _7, _8, _9, _10, _11, _12, _13, _14) => _0 + _1 + _2 + _3 + _4 + _5 + _6 + _7 + _8 + _9 + _10 + _11 + _12 + _13 + _14));
            ReactiveAssert.Throws<ArgumentNullException>(() => Observable.CombineLatest(xs, xs, xs, xs, xs, xs, xs, xs, default(IObservable<int>), xs, xs, xs, xs, xs, xs, (_0, _1, _2, _3, _4, _5, _6, _7, _8, _9, _10, _11, _12, _13, _14) => _0 + _1 + _2 + _3 + _4 + _5 + _6 + _7 + _8 + _9 + _10 + _11 + _12 + _13 + _14));
            ReactiveAssert.Throws<ArgumentNullException>(() => Observable.CombineLatest(xs, xs, xs, xs, xs, xs, xs, xs, xs, default(IObservable<int>), xs, xs, xs, xs, xs, (_0, _1, _2, _3, _4, _5, _6, _7, _8, _9, _10, _11, _12, _13, _14) => _0 + _1 + _2 + _3 + _4 + _5 + _6 + _7 + _8 + _9 + _10 + _11 + _12 + _13 + _14));
            ReactiveAssert.Throws<ArgumentNullException>(() => Observable.CombineLatest(xs, xs, xs, xs, xs, xs, xs, xs, xs, xs, default(IObservable<int>), xs, xs, xs, xs, (_0, _1, _2, _3, _4, _5, _6, _7, _8, _9, _10, _11, _12, _13, _14) => _0 + _1 + _2 + _3 + _4 + _5 + _6 + _7 + _8 + _9 + _10 + _11 + _12 + _13 + _14));
            ReactiveAssert.Throws<ArgumentNullException>(() => Observable.CombineLatest(xs, xs, xs, xs, xs, xs, xs, xs, xs, xs, xs, default(IObservable<int>), xs, xs, xs, (_0, _1, _2, _3, _4, _5, _6, _7, _8, _9, _10, _11, _12, _13, _14) => _0 + _1 + _2 + _3 + _4 + _5 + _6 + _7 + _8 + _9 + _10 + _11 + _12 + _13 + _14));
            ReactiveAssert.Throws<ArgumentNullException>(() => Observable.CombineLatest(xs, xs, xs, xs, xs, xs, xs, xs, xs, xs, xs, xs, default(IObservable<int>), xs, xs, (_0, _1, _2, _3, _4, _5, _6, _7, _8, _9, _10, _11, _12, _13, _14) => _0 + _1 + _2 + _3 + _4 + _5 + _6 + _7 + _8 + _9 + _10 + _11 + _12 + _13 + _14));
            ReactiveAssert.Throws<ArgumentNullException>(() => Observable.CombineLatest(xs, xs, xs, xs, xs, xs, xs, xs, xs, xs, xs, xs, xs, default(IObservable<int>), xs, (_0, _1, _2, _3, _4, _5, _6, _7, _8, _9, _10, _11, _12, _13, _14) => _0 + _1 + _2 + _3 + _4 + _5 + _6 + _7 + _8 + _9 + _10 + _11 + _12 + _13 + _14));
            ReactiveAssert.Throws<ArgumentNullException>(() => Observable.CombineLatest(xs, xs, xs, xs, xs, xs, xs, xs, xs, xs, xs, xs, xs, xs, default(IObservable<int>), (_0, _1, _2, _3, _4, _5, _6, _7, _8, _9, _10, _11, _12, _13, _14) => _0 + _1 + _2 + _3 + _4 + _5 + _6 + _7 + _8 + _9 + _10 + _11 + _12 + _13 + _14));
            ReactiveAssert.Throws<ArgumentNullException>(() => Observable.CombineLatest(xs, xs, xs, xs, xs, xs, xs, xs, xs, xs, xs, xs, xs, xs, xs, default(Func<int, int, int, int, int, int, int, int, int, int, int, int, int, int, int, int>)));
            ReactiveAssert.Throws<ArgumentNullException>(() => Observable.CombineLatest(default(IObservable<int>), xs, xs, xs, xs, xs, xs, xs, xs, xs, xs, xs, xs, xs, xs, xs, (_0, _1, _2, _3, _4, _5, _6, _7, _8, _9, _10, _11, _12, _13, _14, _15) => _0 + _1 + _2 + _3 + _4 + _5 + _6 + _7 + _8 + _9 + _10 + _11 + _12 + _13 + _14 + _15));
            ReactiveAssert.Throws<ArgumentNullException>(() => Observable.CombineLatest(xs, default(IObservable<int>), xs, xs, xs, xs, xs, xs, xs, xs, xs, xs, xs, xs, xs, xs, (_0, _1, _2, _3, _4, _5, _6, _7, _8, _9, _10, _11, _12, _13, _14, _15) => _0 + _1 + _2 + _3 + _4 + _5 + _6 + _7 + _8 + _9 + _10 + _11 + _12 + _13 + _14 + _15));
            ReactiveAssert.Throws<ArgumentNullException>(() => Observable.CombineLatest(xs, xs, default(IObservable<int>), xs, xs, xs, xs, xs, xs, xs, xs, xs, xs, xs, xs, xs, (_0, _1, _2, _3, _4, _5, _6, _7, _8, _9, _10, _11, _12, _13, _14, _15) => _0 + _1 + _2 + _3 + _4 + _5 + _6 + _7 + _8 + _9 + _10 + _11 + _12 + _13 + _14 + _15));
            ReactiveAssert.Throws<ArgumentNullException>(() => Observable.CombineLatest(xs, xs, xs, default(IObservable<int>), xs, xs, xs, xs, xs, xs, xs, xs, xs, xs, xs, xs, (_0, _1, _2, _3, _4, _5, _6, _7, _8, _9, _10, _11, _12, _13, _14, _15) => _0 + _1 + _2 + _3 + _4 + _5 + _6 + _7 + _8 + _9 + _10 + _11 + _12 + _13 + _14 + _15));
            ReactiveAssert.Throws<ArgumentNullException>(() => Observable.CombineLatest(xs, xs, xs, xs, default(IObservable<int>), xs, xs, xs, xs, xs, xs, xs, xs, xs, xs, xs, (_0, _1, _2, _3, _4, _5, _6, _7, _8, _9, _10, _11, _12, _13, _14, _15) => _0 + _1 + _2 + _3 + _4 + _5 + _6 + _7 + _8 + _9 + _10 + _11 + _12 + _13 + _14 + _15));
            ReactiveAssert.Throws<ArgumentNullException>(() => Observable.CombineLatest(xs, xs, xs, xs, xs, default(IObservable<int>), xs, xs, xs, xs, xs, xs, xs, xs, xs, xs, (_0, _1, _2, _3, _4, _5, _6, _7, _8, _9, _10, _11, _12, _13, _14, _15) => _0 + _1 + _2 + _3 + _4 + _5 + _6 + _7 + _8 + _9 + _10 + _11 + _12 + _13 + _14 + _15));
            ReactiveAssert.Throws<ArgumentNullException>(() => Observable.CombineLatest(xs, xs, xs, xs, xs, xs, default(IObservable<int>), xs, xs, xs, xs, xs, xs, xs, xs, xs, (_0, _1, _2, _3, _4, _5, _6, _7, _8, _9, _10, _11, _12, _13, _14, _15) => _0 + _1 + _2 + _3 + _4 + _5 + _6 + _7 + _8 + _9 + _10 + _11 + _12 + _13 + _14 + _15));
            ReactiveAssert.Throws<ArgumentNullException>(() => Observable.CombineLatest(xs, xs, xs, xs, xs, xs, xs, default(IObservable<int>), xs, xs, xs, xs, xs, xs, xs, xs, (_0, _1, _2, _3, _4, _5, _6, _7, _8, _9, _10, _11, _12, _13, _14, _15) => _0 + _1 + _2 + _3 + _4 + _5 + _6 + _7 + _8 + _9 + _10 + _11 + _12 + _13 + _14 + _15));
            ReactiveAssert.Throws<ArgumentNullException>(() => Observable.CombineLatest(xs, xs, xs, xs, xs, xs, xs, xs, default(IObservable<int>), xs, xs, xs, xs, xs, xs, xs, (_0, _1, _2, _3, _4, _5, _6, _7, _8, _9, _10, _11, _12, _13, _14, _15) => _0 + _1 + _2 + _3 + _4 + _5 + _6 + _7 + _8 + _9 + _10 + _11 + _12 + _13 + _14 + _15));
            ReactiveAssert.Throws<ArgumentNullException>(() => Observable.CombineLatest(xs, xs, xs, xs, xs, xs, xs, xs, xs, default(IObservable<int>), xs, xs, xs, xs, xs, xs, (_0, _1, _2, _3, _4, _5, _6, _7, _8, _9, _10, _11, _12, _13, _14, _15) => _0 + _1 + _2 + _3 + _4 + _5 + _6 + _7 + _8 + _9 + _10 + _11 + _12 + _13 + _14 + _15));
            ReactiveAssert.Throws<ArgumentNullException>(() => Observable.CombineLatest(xs, xs, xs, xs, xs, xs, xs, xs, xs, xs, default(IObservable<int>), xs, xs, xs, xs, xs, (_0, _1, _2, _3, _4, _5, _6, _7, _8, _9, _10, _11, _12, _13, _14, _15) => _0 + _1 + _2 + _3 + _4 + _5 + _6 + _7 + _8 + _9 + _10 + _11 + _12 + _13 + _14 + _15));
            ReactiveAssert.Throws<ArgumentNullException>(() => Observable.CombineLatest(xs, xs, xs, xs, xs, xs, xs, xs, xs, xs, xs, default(IObservable<int>), xs, xs, xs, xs, (_0, _1, _2, _3, _4, _5, _6, _7, _8, _9, _10, _11, _12, _13, _14, _15) => _0 + _1 + _2 + _3 + _4 + _5 + _6 + _7 + _8 + _9 + _10 + _11 + _12 + _13 + _14 + _15));
            ReactiveAssert.Throws<ArgumentNullException>(() => Observable.CombineLatest(xs, xs, xs, xs, xs, xs, xs, xs, xs, xs, xs, xs, default(IObservable<int>), xs, xs, xs, (_0, _1, _2, _3, _4, _5, _6, _7, _8, _9, _10, _11, _12, _13, _14, _15) => _0 + _1 + _2 + _3 + _4 + _5 + _6 + _7 + _8 + _9 + _10 + _11 + _12 + _13 + _14 + _15));
            ReactiveAssert.Throws<ArgumentNullException>(() => Observable.CombineLatest(xs, xs, xs, xs, xs, xs, xs, xs, xs, xs, xs, xs, xs, default(IObservable<int>), xs, xs, (_0, _1, _2, _3, _4, _5, _6, _7, _8, _9, _10, _11, _12, _13, _14, _15) => _0 + _1 + _2 + _3 + _4 + _5 + _6 + _7 + _8 + _9 + _10 + _11 + _12 + _13 + _14 + _15));
            ReactiveAssert.Throws<ArgumentNullException>(() => Observable.CombineLatest(xs, xs, xs, xs, xs, xs, xs, xs, xs, xs, xs, xs, xs, xs, default(IObservable<int>), xs, (_0, _1, _2, _3, _4, _5, _6, _7, _8, _9, _10, _11, _12, _13, _14, _15) => _0 + _1 + _2 + _3 + _4 + _5 + _6 + _7 + _8 + _9 + _10 + _11 + _12 + _13 + _14 + _15));
            ReactiveAssert.Throws<ArgumentNullException>(() => Observable.CombineLatest(xs, xs, xs, xs, xs, xs, xs, xs, xs, xs, xs, xs, xs, xs, xs, default(IObservable<int>), (_0, _1, _2, _3, _4, _5, _6, _7, _8, _9, _10, _11, _12, _13, _14, _15) => _0 + _1 + _2 + _3 + _4 + _5 + _6 + _7 + _8 + _9 + _10 + _11 + _12 + _13 + _14 + _15));
            ReactiveAssert.Throws<ArgumentNullException>(() => Observable.CombineLatest(xs, xs, xs, xs, xs, xs, xs, xs, xs, xs, xs, xs, xs, xs, xs, xs, default(Func<int, int, int, int, int, int, int, int, int, int, int, int, int, int, int, int, int>)));
#endif
        }

        #endregion

        #region Never

        [TestMethod]
        public void CombineLatest_NeverN()
        {
            var scheduler = new TestScheduler();

            var e0 = scheduler.CreateHotObservable(new[] { OnNext(150, 1) });
            var e1 = scheduler.CreateHotObservable(new[] { OnNext(150, 1) });
            var e2 = scheduler.CreateHotObservable(new[] { OnNext(150, 1) });

            var res = scheduler.Start(() =>
                Observable.CombineLatest(new[] { e0, e1, e2 }, xs => xs.Sum())
            );
            
            res.Messages.AssertEqual(
            );

            foreach (var e in new[] { e0, e1, e2 })
                e.Subscriptions.AssertEqual(Subscribe(200, 1000));
        }

        [TestMethod]
        public void CombineLatest_Never2()
        {
            var scheduler = new TestScheduler();
            
            var e0 = scheduler.CreateHotObservable(new[] { OnNext(150, 1) });
            var e1 = scheduler.CreateHotObservable(new[] { OnNext(150, 1) });

            var res = scheduler.Start(() =>
                Observable.CombineLatest(e0, e1, (_0, _1) => 42)
            );

            res.Messages.AssertEqual(
            );

            foreach (var e in new[] { e0, e1 })
                e.Subscriptions.AssertEqual(Subscribe(200, 1000));
        }

        [TestMethod]
        public void CombineLatest_Never3()
        {
            var scheduler = new TestScheduler();

            var e0 = scheduler.CreateHotObservable(new[] { OnNext(150, 1) });
            var e1 = scheduler.CreateHotObservable(new[] { OnNext(150, 1) });
            var e2 = scheduler.CreateHotObservable(new[] { OnNext(150, 1) });

            var res = scheduler.Start(() =>
                Observable.CombineLatest(e0, e1, e2, (_0, _1, _2) => 42)
            );

            res.Messages.AssertEqual(
            );

            foreach (var e in new[] { e0, e1, e2 })
                e.Subscriptions.AssertEqual(Subscribe(200, 1000));
        }

        [TestMethod]
        public void CombineLatest_Never4()
        {
            var scheduler = new TestScheduler();

            var e0 = scheduler.CreateHotObservable(new[] { OnNext(150, 1) });
            var e1 = scheduler.CreateHotObservable(new[] { OnNext(150, 1) });
            var e2 = scheduler.CreateHotObservable(new[] { OnNext(150, 1) });
            var e3 = scheduler.CreateHotObservable(new[] { OnNext(150, 1) });

            var res = scheduler.Start(() =>
                Observable.CombineLatest(e0, e1, e2, e3, (_0, _1, _2, _3) => 42)
            );

            res.Messages.AssertEqual(
            );

            foreach (var e in new[] { e0, e1, e2, e3 })
                e.Subscriptions.AssertEqual(Subscribe(200, 1000));
        }

#if !NO_LARGEARITY
        [TestMethod]
        public void CombineLatest_Never5()
        {
            var scheduler = new TestScheduler();

            var e0 = scheduler.CreateHotObservable(new[] { OnNext(150, 1) });
            var e1 = scheduler.CreateHotObservable(new[] { OnNext(150, 1) });
            var e2 = scheduler.CreateHotObservable(new[] { OnNext(150, 1) });
            var e3 = scheduler.CreateHotObservable(new[] { OnNext(150, 1) });
            var e4 = scheduler.CreateHotObservable(new[] { OnNext(150, 1) });

            var res = scheduler.Start(() =>
                Observable.CombineLatest(e0, e1, e2, e3, e4, (_0, _1, _2, _3, _4) => 42)
            );

            res.Messages.AssertEqual(
            );

            foreach (var e in new[] { e0, e1, e2, e3, e4 })
                e.Subscriptions.AssertEqual(Subscribe(200, 1000));
        }

        [TestMethod]
        public void CombineLatest_Never6()
        {
            var scheduler = new TestScheduler();

            var e0 = scheduler.CreateHotObservable(new[] { OnNext(150, 1) });
            var e1 = scheduler.CreateHotObservable(new[] { OnNext(150, 1) });
            var e2 = scheduler.CreateHotObservable(new[] { OnNext(150, 1) });
            var e3 = scheduler.CreateHotObservable(new[] { OnNext(150, 1) });
            var e4 = scheduler.CreateHotObservable(new[] { OnNext(150, 1) });
            var e5 = scheduler.CreateHotObservable(new[] { OnNext(150, 1) });

            var res = scheduler.Start(() =>
                Observable.CombineLatest(e0, e1, e2, e3, e4, e5, (_0, _1, _2, _3, _4, _5) => 42)
            );

            res.Messages.AssertEqual(
            );

            foreach (var e in new[] { e0, e1, e2, e3, e4, e5 })
                e.Subscriptions.AssertEqual(Subscribe(200, 1000));
        }

        [TestMethod]
        public void CombineLatest_Never7()
        {
            var scheduler = new TestScheduler();

            var e0 = scheduler.CreateHotObservable(new[] { OnNext(150, 1) });
            var e1 = scheduler.CreateHotObservable(new[] { OnNext(150, 1) });
            var e2 = scheduler.CreateHotObservable(new[] { OnNext(150, 1) });
            var e3 = scheduler.CreateHotObservable(new[] { OnNext(150, 1) });
            var e4 = scheduler.CreateHotObservable(new[] { OnNext(150, 1) });
            var e5 = scheduler.CreateHotObservable(new[] { OnNext(150, 1) });
            var e6 = scheduler.CreateHotObservable(new[] { OnNext(150, 1) });

            var res = scheduler.Start(() =>
                Observable.CombineLatest(e0, e1, e2, e3, e4, e5, e6, (_0, _1, _2, _3, _4, _5, _6) => 42)
            );

            res.Messages.AssertEqual(
            );

            foreach (var e in new[] { e0, e1, e2, e3, e4, e5, e6 })
                e.Subscriptions.AssertEqual(Subscribe(200, 1000));
        }

        [TestMethod]
        public void CombineLatest_Never8()
        {
            var scheduler = new TestScheduler();

            var e0 = scheduler.CreateHotObservable(new[] { OnNext(150, 1) });
            var e1 = scheduler.CreateHotObservable(new[] { OnNext(150, 1) });
            var e2 = scheduler.CreateHotObservable(new[] { OnNext(150, 1) });
            var e3 = scheduler.CreateHotObservable(new[] { OnNext(150, 1) });
            var e4 = scheduler.CreateHotObservable(new[] { OnNext(150, 1) });
            var e5 = scheduler.CreateHotObservable(new[] { OnNext(150, 1) });
            var e6 = scheduler.CreateHotObservable(new[] { OnNext(150, 1) });
            var e7 = scheduler.CreateHotObservable(new[] { OnNext(150, 1) });

            var res = scheduler.Start(() =>
                Observable.CombineLatest(e0, e1, e2, e3, e4, e5, e6, e7, (_0, _1, _2, _3, _4, _5, _6, _7) => 42)
            );

            res.Messages.AssertEqual(
            );

            foreach (var e in new[] { e0, e1, e2, e3, e4, e5, e6, e7 })
                e.Subscriptions.AssertEqual(Subscribe(200, 1000));
        }

        [TestMethod]
        public void CombineLatest_Never9()
        {
            var scheduler = new TestScheduler();

            var e0 = scheduler.CreateHotObservable(new[] { OnNext(150, 1) });
            var e1 = scheduler.CreateHotObservable(new[] { OnNext(150, 1) });
            var e2 = scheduler.CreateHotObservable(new[] { OnNext(150, 1) });
            var e3 = scheduler.CreateHotObservable(new[] { OnNext(150, 1) });
            var e4 = scheduler.CreateHotObservable(new[] { OnNext(150, 1) });
            var e5 = scheduler.CreateHotObservable(new[] { OnNext(150, 1) });
            var e6 = scheduler.CreateHotObservable(new[] { OnNext(150, 1) });
            var e7 = scheduler.CreateHotObservable(new[] { OnNext(150, 1) });
            var e8 = scheduler.CreateHotObservable(new[] { OnNext(150, 1) });

            var res = scheduler.Start(() =>
                Observable.CombineLatest(e0, e1, e2, e3, e4, e5, e6, e7, e8, (_0, _1, _2, _3, _4, _5, _6, _7, _8) => 42)
            );

            res.Messages.AssertEqual(
            );

            foreach (var e in new[] { e0, e1, e2, e3, e4, e5, e6, e7, e8 })
                e.Subscriptions.AssertEqual(Subscribe(200, 1000));
        }

        [TestMethod]
        public void CombineLatest_Never10()
        {
            var scheduler = new TestScheduler();

            var e0 = scheduler.CreateHotObservable(new[] { OnNext(150, 1) });
            var e1 = scheduler.CreateHotObservable(new[] { OnNext(150, 1) });
            var e2 = scheduler.CreateHotObservable(new[] { OnNext(150, 1) });
            var e3 = scheduler.CreateHotObservable(new[] { OnNext(150, 1) });
            var e4 = scheduler.CreateHotObservable(new[] { OnNext(150, 1) });
            var e5 = scheduler.CreateHotObservable(new[] { OnNext(150, 1) });
            var e6 = scheduler.CreateHotObservable(new[] { OnNext(150, 1) });
            var e7 = scheduler.CreateHotObservable(new[] { OnNext(150, 1) });
            var e8 = scheduler.CreateHotObservable(new[] { OnNext(150, 1) });
            var e9 = scheduler.CreateHotObservable(new[] { OnNext(150, 1) });

            var res = scheduler.Start(() =>
                Observable.CombineLatest(e0, e1, e2, e3, e4, e5, e6, e7, e8, e9, (_0, _1, _2, _3, _4, _5, _6, _7, _8, _9) => 42)
            );

            res.Messages.AssertEqual(
            );

            foreach (var e in new[] { e0, e1, e2, e3, e4, e5, e6, e7, e8, e9 })
                e.Subscriptions.AssertEqual(Subscribe(200, 1000));
        }

        [TestMethod]
        public void CombineLatest_Never11()
        {
            var scheduler = new TestScheduler();

            var e0 = scheduler.CreateHotObservable(new[] { OnNext(150, 1) });
            var e1 = scheduler.CreateHotObservable(new[] { OnNext(150, 1) });
            var e2 = scheduler.CreateHotObservable(new[] { OnNext(150, 1) });
            var e3 = scheduler.CreateHotObservable(new[] { OnNext(150, 1) });
            var e4 = scheduler.CreateHotObservable(new[] { OnNext(150, 1) });
            var e5 = scheduler.CreateHotObservable(new[] { OnNext(150, 1) });
            var e6 = scheduler.CreateHotObservable(new[] { OnNext(150, 1) });
            var e7 = scheduler.CreateHotObservable(new[] { OnNext(150, 1) });
            var e8 = scheduler.CreateHotObservable(new[] { OnNext(150, 1) });
            var e9 = scheduler.CreateHotObservable(new[] { OnNext(150, 1) });
            var e10 = scheduler.CreateHotObservable(new[] { OnNext(150, 1) });

            var res = scheduler.Start(() =>
                Observable.CombineLatest(e0, e1, e2, e3, e4, e5, e6, e7, e8, e9, e10, (_0, _1, _2, _3, _4, _5, _6, _7, _8, _9, _10) => 42)
            );

            res.Messages.AssertEqual(
            );

            foreach (var e in new[] { e0, e1, e2, e3, e4, e5, e6, e7, e8, e9, e10 })
                e.Subscriptions.AssertEqual(Subscribe(200, 1000));
        }

        [TestMethod]
        public void CombineLatest_Never12()
        {
            var scheduler = new TestScheduler();

            var e0 = scheduler.CreateHotObservable(new[] { OnNext(150, 1) });
            var e1 = scheduler.CreateHotObservable(new[] { OnNext(150, 1) });
            var e2 = scheduler.CreateHotObservable(new[] { OnNext(150, 1) });
            var e3 = scheduler.CreateHotObservable(new[] { OnNext(150, 1) });
            var e4 = scheduler.CreateHotObservable(new[] { OnNext(150, 1) });
            var e5 = scheduler.CreateHotObservable(new[] { OnNext(150, 1) });
            var e6 = scheduler.CreateHotObservable(new[] { OnNext(150, 1) });
            var e7 = scheduler.CreateHotObservable(new[] { OnNext(150, 1) });
            var e8 = scheduler.CreateHotObservable(new[] { OnNext(150, 1) });
            var e9 = scheduler.CreateHotObservable(new[] { OnNext(150, 1) });
            var e10 = scheduler.CreateHotObservable(new[] { OnNext(150, 1) });
            var e11 = scheduler.CreateHotObservable(new[] { OnNext(150, 1) });

            var res = scheduler.Start(() =>
                Observable.CombineLatest(e0, e1, e2, e3, e4, e5, e6, e7, e8, e9, e10, e11, (_0, _1, _2, _3, _4, _5, _6, _7, _8, _9, _10, _11) => 42)
            );

            res.Messages.AssertEqual(
            );

            foreach (var e in new[] { e0, e1, e2, e3, e4, e5, e6, e7, e8, e9, e10, e11 })
                e.Subscriptions.AssertEqual(Subscribe(200, 1000));
        }

        [TestMethod]
        public void CombineLatest_Never13()
        {
            var scheduler = new TestScheduler();

            var e0 = scheduler.CreateHotObservable(new[] { OnNext(150, 1) });
            var e1 = scheduler.CreateHotObservable(new[] { OnNext(150, 1) });
            var e2 = scheduler.CreateHotObservable(new[] { OnNext(150, 1) });
            var e3 = scheduler.CreateHotObservable(new[] { OnNext(150, 1) });
            var e4 = scheduler.CreateHotObservable(new[] { OnNext(150, 1) });
            var e5 = scheduler.CreateHotObservable(new[] { OnNext(150, 1) });
            var e6 = scheduler.CreateHotObservable(new[] { OnNext(150, 1) });
            var e7 = scheduler.CreateHotObservable(new[] { OnNext(150, 1) });
            var e8 = scheduler.CreateHotObservable(new[] { OnNext(150, 1) });
            var e9 = scheduler.CreateHotObservable(new[] { OnNext(150, 1) });
            var e10 = scheduler.CreateHotObservable(new[] { OnNext(150, 1) });
            var e11 = scheduler.CreateHotObservable(new[] { OnNext(150, 1) });
            var e12 = scheduler.CreateHotObservable(new[] { OnNext(150, 1) });

            var res = scheduler.Start(() =>
                Observable.CombineLatest(e0, e1, e2, e3, e4, e5, e6, e7, e8, e9, e10, e11, e12, (_0, _1, _2, _3, _4, _5, _6, _7, _8, _9, _10, _11, _12) => 42)
            );

            res.Messages.AssertEqual(
            );

            foreach (var e in new[] { e0, e1, e2, e3, e4, e5, e6, e7, e8, e9, e10, e11, e12 })
                e.Subscriptions.AssertEqual(Subscribe(200, 1000));
        }

        [TestMethod]
        public void CombineLatest_Never14()
        {
            var scheduler = new TestScheduler();

            var e0 = scheduler.CreateHotObservable(new[] { OnNext(150, 1) });
            var e1 = scheduler.CreateHotObservable(new[] { OnNext(150, 1) });
            var e2 = scheduler.CreateHotObservable(new[] { OnNext(150, 1) });
            var e3 = scheduler.CreateHotObservable(new[] { OnNext(150, 1) });
            var e4 = scheduler.CreateHotObservable(new[] { OnNext(150, 1) });
            var e5 = scheduler.CreateHotObservable(new[] { OnNext(150, 1) });
            var e6 = scheduler.CreateHotObservable(new[] { OnNext(150, 1) });
            var e7 = scheduler.CreateHotObservable(new[] { OnNext(150, 1) });
            var e8 = scheduler.CreateHotObservable(new[] { OnNext(150, 1) });
            var e9 = scheduler.CreateHotObservable(new[] { OnNext(150, 1) });
            var e10 = scheduler.CreateHotObservable(new[] { OnNext(150, 1) });
            var e11 = scheduler.CreateHotObservable(new[] { OnNext(150, 1) });
            var e12 = scheduler.CreateHotObservable(new[] { OnNext(150, 1) });
            var e13 = scheduler.CreateHotObservable(new[] { OnNext(150, 1) });

            var res = scheduler.Start(() =>
                Observable.CombineLatest(e0, e1, e2, e3, e4, e5, e6, e7, e8, e9, e10, e11, e12, e13, (_0, _1, _2, _3, _4, _5, _6, _7, _8, _9, _10, _11, _12, _13) => 42)
            );

            res.Messages.AssertEqual(
            );

            foreach (var e in new[] { e0, e1, e2, e3, e4, e5, e6, e7, e8, e9, e10, e11, e12, e13 })
                e.Subscriptions.AssertEqual(Subscribe(200, 1000));
        }

        [TestMethod]
        public void CombineLatest_Never15()
        {
            var scheduler = new TestScheduler();

            var e0 = scheduler.CreateHotObservable(new[] { OnNext(150, 1) });
            var e1 = scheduler.CreateHotObservable(new[] { OnNext(150, 1) });
            var e2 = scheduler.CreateHotObservable(new[] { OnNext(150, 1) });
            var e3 = scheduler.CreateHotObservable(new[] { OnNext(150, 1) });
            var e4 = scheduler.CreateHotObservable(new[] { OnNext(150, 1) });
            var e5 = scheduler.CreateHotObservable(new[] { OnNext(150, 1) });
            var e6 = scheduler.CreateHotObservable(new[] { OnNext(150, 1) });
            var e7 = scheduler.CreateHotObservable(new[] { OnNext(150, 1) });
            var e8 = scheduler.CreateHotObservable(new[] { OnNext(150, 1) });
            var e9 = scheduler.CreateHotObservable(new[] { OnNext(150, 1) });
            var e10 = scheduler.CreateHotObservable(new[] { OnNext(150, 1) });
            var e11 = scheduler.CreateHotObservable(new[] { OnNext(150, 1) });
            var e12 = scheduler.CreateHotObservable(new[] { OnNext(150, 1) });
            var e13 = scheduler.CreateHotObservable(new[] { OnNext(150, 1) });
            var e14 = scheduler.CreateHotObservable(new[] { OnNext(150, 1) });

            var res = scheduler.Start(() =>
                Observable.CombineLatest(e0, e1, e2, e3, e4, e5, e6, e7, e8, e9, e10, e11, e12, e13, e14, (_0, _1, _2, _3, _4, _5, _6, _7, _8, _9, _10, _11, _12, _13, _14) => 42)
            );

            res.Messages.AssertEqual(
            );

            foreach (var e in new[] { e0, e1, e2, e3, e4, e5, e6, e7, e8, e9, e10, e11, e12, e13, e14 })
                e.Subscriptions.AssertEqual(Subscribe(200, 1000));
        }

        [TestMethod]
        public void CombineLatest_Never16()
        {
            var scheduler = new TestScheduler();

            var e0 = scheduler.CreateHotObservable(new[] { OnNext(150, 1) });
            var e1 = scheduler.CreateHotObservable(new[] { OnNext(150, 1) });
            var e2 = scheduler.CreateHotObservable(new[] { OnNext(150, 1) });
            var e3 = scheduler.CreateHotObservable(new[] { OnNext(150, 1) });
            var e4 = scheduler.CreateHotObservable(new[] { OnNext(150, 1) });
            var e5 = scheduler.CreateHotObservable(new[] { OnNext(150, 1) });
            var e6 = scheduler.CreateHotObservable(new[] { OnNext(150, 1) });
            var e7 = scheduler.CreateHotObservable(new[] { OnNext(150, 1) });
            var e8 = scheduler.CreateHotObservable(new[] { OnNext(150, 1) });
            var e9 = scheduler.CreateHotObservable(new[] { OnNext(150, 1) });
            var e10 = scheduler.CreateHotObservable(new[] { OnNext(150, 1) });
            var e11 = scheduler.CreateHotObservable(new[] { OnNext(150, 1) });
            var e12 = scheduler.CreateHotObservable(new[] { OnNext(150, 1) });
            var e13 = scheduler.CreateHotObservable(new[] { OnNext(150, 1) });
            var e14 = scheduler.CreateHotObservable(new[] { OnNext(150, 1) });
            var e15 = scheduler.CreateHotObservable(new[] { OnNext(150, 1) });

            var res = scheduler.Start(() =>
                Observable.CombineLatest(e0, e1, e2, e3, e4, e5, e6, e7, e8, e9, e10, e11, e12, e13, e14, e15, (_0, _1, _2, _3, _4, _5, _6, _7, _8, _9, _10, _11, _12, _13, _14, _15) => 42)
            );

            res.Messages.AssertEqual(
            );

            foreach (var e in new[] { e0, e1, e2, e3, e4, e5, e6, e7, e8, e9, e10, e11, e12, e13, e14, e15 })
                e.Subscriptions.AssertEqual(Subscribe(200, 1000));
        }
#endif

        #endregion

        #region Never/Empty

        [TestMethod]
        public void CombineLatest_NeverEmpty()
        {
            var scheduler = new TestScheduler();

            var n = scheduler.CreateHotObservable(
                OnNext(150, 1)
            );

            var e = scheduler.CreateHotObservable(
                OnNext(150, 1),
                OnCompleted<int>(210)
            );

            var res = scheduler.Start(() =>
                n.CombineLatest(e, (x, y) => x + y)
            );

            res.Messages.AssertEqual(
            );

            n.Subscriptions.AssertEqual(
                Subscribe(200, 1000)
            );

            e.Subscriptions.AssertEqual(
                Subscribe(200, 210)
            );
        }

        [TestMethod]
        public void CombineLatest_EmptyNever()
        {
            var scheduler = new TestScheduler();

            var e = scheduler.CreateHotObservable(
                OnNext(150, 1),
                OnCompleted<int>(210)
            );

            var n = scheduler.CreateHotObservable(
                OnNext(150, 1)
            );

            var res = scheduler.Start(() =>
                e.CombineLatest(n, (x, y) => x + y)
            );

            res.Messages.AssertEqual(
            );

            n.Subscriptions.AssertEqual(
                Subscribe(200, 1000)
            );

            e.Subscriptions.AssertEqual(
                Subscribe(200, 210)
            );
        }

        #endregion

        #region Empty

        [TestMethod]
        public void CombineLatest_EmptyN()
        {
            var scheduler = new TestScheduler();

            var e0 = scheduler.CreateHotObservable(new[] { OnNext(150, 1), OnCompleted<int>(210) });
            var e1 = scheduler.CreateHotObservable(new[] { OnNext(150, 1), OnCompleted<int>(220) });
            var e2 = scheduler.CreateHotObservable(new[] { OnNext(150, 1), OnCompleted<int>(230) });

            var res = scheduler.Start(() =>
                Observable.CombineLatest(new[] { e0, e1, e2 }, xs => xs.Sum())
            );
            
            res.Messages.AssertEqual(
                OnCompleted<int>(230)
            );

            var i = 0;
            foreach (var e in new[] { e0, e1, e2 })
                e.Subscriptions.AssertEqual(Subscribe(200, 200 + (++i * 10)));
        }

        [TestMethod]
        public void CombineLatest_Empty2()
        {
            var scheduler = new TestScheduler();

            var e0 = scheduler.CreateHotObservable(new[] { OnNext(150, 1), OnCompleted<int>(210) });
            var e1 = scheduler.CreateHotObservable(new[] { OnNext(150, 1), OnCompleted<int>(220) });

            var res = scheduler.Start(() =>
                Observable.CombineLatest(e0, e1, (_0, _1) => 42)
            );
            
            res.Messages.AssertEqual(
                OnCompleted<int>(220)
            );

            var i = 0;
            foreach (var e in new[] { e0, e1 })
                e.Subscriptions.AssertEqual(Subscribe(200, 200 + (++i * 10)));
        }

        [TestMethod]
        public void CombineLatest_Empty3()
        {
            var scheduler = new TestScheduler();

            var e0 = scheduler.CreateHotObservable(new[] { OnNext(150, 1), OnCompleted<int>(210) });
            var e1 = scheduler.CreateHotObservable(new[] { OnNext(150, 1), OnCompleted<int>(220) });
            var e2 = scheduler.CreateHotObservable(new[] { OnNext(150, 1), OnCompleted<int>(230) });

            var res = scheduler.Start(() =>
                Observable.CombineLatest(e0, e1, e2, (_0, _1, _2) => 42)
            );
            
            res.Messages.AssertEqual(
                OnCompleted<int>(230)
            );

            var i = 0;
            foreach (var e in new[] { e0, e1, e2 })
                e.Subscriptions.AssertEqual(Subscribe(200, 200 + (++i * 10)));
        }

        [TestMethod]
        public void CombineLatest_Empty4()
        {
            var scheduler = new TestScheduler();

            var e0 = scheduler.CreateHotObservable(new[] { OnNext(150, 1), OnCompleted<int>(210) });
            var e1 = scheduler.CreateHotObservable(new[] { OnNext(150, 1), OnCompleted<int>(220) });
            var e2 = scheduler.CreateHotObservable(new[] { OnNext(150, 1), OnCompleted<int>(230) });
            var e3 = scheduler.CreateHotObservable(new[] { OnNext(150, 1), OnCompleted<int>(240) });

            var res = scheduler.Start(() =>
                Observable.CombineLatest(e0, e1, e2, e3, (_0, _1, _2, _3) => 42)
            );

            res.Messages.AssertEqual(
                OnCompleted<int>(240)
            );

            var i = 0;
            foreach (var e in new[] { e0, e1, e2, e3 })
                e.Subscriptions.AssertEqual(Subscribe(200, 200 + (++i * 10)));
        }

#if !NO_LARGEARITY
        [TestMethod]
        public void CombineLatest_Empty5()
        {
            var scheduler = new TestScheduler();

            var e0 = scheduler.CreateHotObservable(new[] { OnNext(150, 1), OnCompleted<int>(210) });
            var e1 = scheduler.CreateHotObservable(new[] { OnNext(150, 1), OnCompleted<int>(220) });
            var e2 = scheduler.CreateHotObservable(new[] { OnNext(150, 1), OnCompleted<int>(230) });
            var e3 = scheduler.CreateHotObservable(new[] { OnNext(150, 1), OnCompleted<int>(240) });
            var e4 = scheduler.CreateHotObservable(new[] { OnNext(150, 1), OnCompleted<int>(250) });

            var res = scheduler.Start(() =>
                Observable.CombineLatest(e0, e1, e2, e3, e4, (_0, _1, _2, _3, _4) => 42)
            );
            
            res.Messages.AssertEqual(
                OnCompleted<int>(250)
            );

            var i = 0;
            foreach (var e in new[] { e0, e1, e2, e3, e4 })
                e.Subscriptions.AssertEqual(Subscribe(200, 200 + (++i * 10)));
        }

        [TestMethod]
        public void CombineLatest_Empty6()
        {
            var scheduler = new TestScheduler();

            var e0 = scheduler.CreateHotObservable(new[] { OnNext(150, 1), OnCompleted<int>(210) });
            var e1 = scheduler.CreateHotObservable(new[] { OnNext(150, 1), OnCompleted<int>(220) });
            var e2 = scheduler.CreateHotObservable(new[] { OnNext(150, 1), OnCompleted<int>(230) });
            var e3 = scheduler.CreateHotObservable(new[] { OnNext(150, 1), OnCompleted<int>(240) });
            var e4 = scheduler.CreateHotObservable(new[] { OnNext(150, 1), OnCompleted<int>(250) });
            var e5 = scheduler.CreateHotObservable(new[] { OnNext(150, 1), OnCompleted<int>(260) });

            var res = scheduler.Start(() =>
                Observable.CombineLatest(e0, e1, e2, e3, e4, e5, (_0, _1, _2, _3, _4, _5) => 42)
            );
            
            res.Messages.AssertEqual(
                OnCompleted<int>(260)
            );

            var i = 0;
            foreach (var e in new[] { e0, e1, e2, e3, e4, e5 })
                e.Subscriptions.AssertEqual(Subscribe(200, 200 + (++i * 10)));
        }

        [TestMethod]
        public void CombineLatest_Empty7()
        {
            var scheduler = new TestScheduler();

            var e0 = scheduler.CreateHotObservable(new[] { OnNext(150, 1), OnCompleted<int>(210) });
            var e1 = scheduler.CreateHotObservable(new[] { OnNext(150, 1), OnCompleted<int>(220) });
            var e2 = scheduler.CreateHotObservable(new[] { OnNext(150, 1), OnCompleted<int>(230) });
            var e3 = scheduler.CreateHotObservable(new[] { OnNext(150, 1), OnCompleted<int>(240) });
            var e4 = scheduler.CreateHotObservable(new[] { OnNext(150, 1), OnCompleted<int>(250) });
            var e5 = scheduler.CreateHotObservable(new[] { OnNext(150, 1), OnCompleted<int>(260) });
            var e6 = scheduler.CreateHotObservable(new[] { OnNext(150, 1), OnCompleted<int>(270) });

            var res = scheduler.Start(() =>
                Observable.CombineLatest(e0, e1, e2, e3, e4, e5, e6, (_0, _1, _2, _3, _4, _5, _6) => 42)
            );
            
            res.Messages.AssertEqual(
                OnCompleted<int>(270)
            );

            var i = 0;
            foreach (var e in new[] { e0, e1, e2, e3, e4, e5, e6 })
                e.Subscriptions.AssertEqual(Subscribe(200, 200 + (++i * 10)));
        }

        [TestMethod]
        public void CombineLatest_Empty8()
        {
            var scheduler = new TestScheduler();

            var e0 = scheduler.CreateHotObservable(new[] { OnNext(150, 1), OnCompleted<int>(210) });
            var e1 = scheduler.CreateHotObservable(new[] { OnNext(150, 1), OnCompleted<int>(220) });
            var e2 = scheduler.CreateHotObservable(new[] { OnNext(150, 1), OnCompleted<int>(230) });
            var e3 = scheduler.CreateHotObservable(new[] { OnNext(150, 1), OnCompleted<int>(240) });
            var e4 = scheduler.CreateHotObservable(new[] { OnNext(150, 1), OnCompleted<int>(250) });
            var e5 = scheduler.CreateHotObservable(new[] { OnNext(150, 1), OnCompleted<int>(260) });
            var e6 = scheduler.CreateHotObservable(new[] { OnNext(150, 1), OnCompleted<int>(270) });
            var e7 = scheduler.CreateHotObservable(new[] { OnNext(150, 1), OnCompleted<int>(280) });

            var res = scheduler.Start(() =>
                Observable.CombineLatest(e0, e1, e2, e3, e4, e5, e6, e7, (_0, _1, _2, _3, _4, _5, _6, _7) => 42)
            );
            
            res.Messages.AssertEqual(
                OnCompleted<int>(280)
            );

            var i = 0;
            foreach (var e in new[] { e0, e1, e2, e3, e4, e5, e6, e7 })
                e.Subscriptions.AssertEqual(Subscribe(200, 200 + (++i * 10)));
        }

        [TestMethod]
        public void CombineLatest_Empty9()
        {
            var scheduler = new TestScheduler();

            var e0 = scheduler.CreateHotObservable(new[] { OnNext(150, 1), OnCompleted<int>(210) });
            var e1 = scheduler.CreateHotObservable(new[] { OnNext(150, 1), OnCompleted<int>(220) });
            var e2 = scheduler.CreateHotObservable(new[] { OnNext(150, 1), OnCompleted<int>(230) });
            var e3 = scheduler.CreateHotObservable(new[] { OnNext(150, 1), OnCompleted<int>(240) });
            var e4 = scheduler.CreateHotObservable(new[] { OnNext(150, 1), OnCompleted<int>(250) });
            var e5 = scheduler.CreateHotObservable(new[] { OnNext(150, 1), OnCompleted<int>(260) });
            var e6 = scheduler.CreateHotObservable(new[] { OnNext(150, 1), OnCompleted<int>(270) });
            var e7 = scheduler.CreateHotObservable(new[] { OnNext(150, 1), OnCompleted<int>(280) });
            var e8 = scheduler.CreateHotObservable(new[] { OnNext(150, 1), OnCompleted<int>(290) });

            var res = scheduler.Start(() =>
                Observable.CombineLatest(e0, e1, e2, e3, e4, e5, e6, e7, e8, (_0, _1, _2, _3, _4, _5, _6, _7, _8) => 42)
            );
            
            res.Messages.AssertEqual(
                OnCompleted<int>(290)
            );

            var i = 0;
            foreach (var e in new[] { e0, e1, e2, e3, e4, e5, e6, e7, e8 })
                e.Subscriptions.AssertEqual(Subscribe(200, 200 + (++i * 10)));
        }

        [TestMethod]
        public void CombineLatest_Empty10()
        {
            var scheduler = new TestScheduler();

            var e0 = scheduler.CreateHotObservable(new[] { OnNext(150, 1), OnCompleted<int>(210) });
            var e1 = scheduler.CreateHotObservable(new[] { OnNext(150, 1), OnCompleted<int>(220) });
            var e2 = scheduler.CreateHotObservable(new[] { OnNext(150, 1), OnCompleted<int>(230) });
            var e3 = scheduler.CreateHotObservable(new[] { OnNext(150, 1), OnCompleted<int>(240) });
            var e4 = scheduler.CreateHotObservable(new[] { OnNext(150, 1), OnCompleted<int>(250) });
            var e5 = scheduler.CreateHotObservable(new[] { OnNext(150, 1), OnCompleted<int>(260) });
            var e6 = scheduler.CreateHotObservable(new[] { OnNext(150, 1), OnCompleted<int>(270) });
            var e7 = scheduler.CreateHotObservable(new[] { OnNext(150, 1), OnCompleted<int>(280) });
            var e8 = scheduler.CreateHotObservable(new[] { OnNext(150, 1), OnCompleted<int>(290) });
            var e9 = scheduler.CreateHotObservable(new[] { OnNext(150, 1), OnCompleted<int>(300) });

            var res = scheduler.Start(() =>
                Observable.CombineLatest(e0, e1, e2, e3, e4, e5, e6, e7, e8, e9, (_0, _1, _2, _3, _4, _5, _6, _7, _8, _9) => 42)
            );

            res.Messages.AssertEqual(
                OnCompleted<int>(300)
            );

            var i = 0;
            foreach (var e in new[] { e0, e1, e2, e3, e4, e5, e6, e7, e8, e9 })
                e.Subscriptions.AssertEqual(Subscribe(200, 200 + (++i * 10)));
        }

        [TestMethod]
        public void CombineLatest_Empty11()
        {
            var scheduler = new TestScheduler();

            var e0 = scheduler.CreateHotObservable(new[] { OnNext(150, 1), OnCompleted<int>(210) });
            var e1 = scheduler.CreateHotObservable(new[] { OnNext(150, 1), OnCompleted<int>(220) });
            var e2 = scheduler.CreateHotObservable(new[] { OnNext(150, 1), OnCompleted<int>(230) });
            var e3 = scheduler.CreateHotObservable(new[] { OnNext(150, 1), OnCompleted<int>(240) });
            var e4 = scheduler.CreateHotObservable(new[] { OnNext(150, 1), OnCompleted<int>(250) });
            var e5 = scheduler.CreateHotObservable(new[] { OnNext(150, 1), OnCompleted<int>(260) });
            var e6 = scheduler.CreateHotObservable(new[] { OnNext(150, 1), OnCompleted<int>(270) });
            var e7 = scheduler.CreateHotObservable(new[] { OnNext(150, 1), OnCompleted<int>(280) });
            var e8 = scheduler.CreateHotObservable(new[] { OnNext(150, 1), OnCompleted<int>(290) });
            var e9 = scheduler.CreateHotObservable(new[] { OnNext(150, 1), OnCompleted<int>(300) });
            var e10 = scheduler.CreateHotObservable(new[] { OnNext(150, 1), OnCompleted<int>(310) });

            var res = scheduler.Start(() =>
                Observable.CombineLatest(e0, e1, e2, e3, e4, e5, e6, e7, e8, e9, e10, (_0, _1, _2, _3, _4, _5, _6, _7, _8, _9, _10) => 42)
            );
            
            res.Messages.AssertEqual(
                OnCompleted<int>(310)
            );

            var i = 0;
            foreach (var e in new[] { e0, e1, e2, e3, e4, e5, e6, e7, e8, e9, e10 })
                e.Subscriptions.AssertEqual(Subscribe(200, 200 + (++i * 10)));
        }

        [TestMethod]
        public void CombineLatest_Empty12()
        {
            var scheduler = new TestScheduler();

            var e0 = scheduler.CreateHotObservable(new[] { OnNext(150, 1), OnCompleted<int>(210) });
            var e1 = scheduler.CreateHotObservable(new[] { OnNext(150, 1), OnCompleted<int>(220) });
            var e2 = scheduler.CreateHotObservable(new[] { OnNext(150, 1), OnCompleted<int>(230) });
            var e3 = scheduler.CreateHotObservable(new[] { OnNext(150, 1), OnCompleted<int>(240) });
            var e4 = scheduler.CreateHotObservable(new[] { OnNext(150, 1), OnCompleted<int>(250) });
            var e5 = scheduler.CreateHotObservable(new[] { OnNext(150, 1), OnCompleted<int>(260) });
            var e6 = scheduler.CreateHotObservable(new[] { OnNext(150, 1), OnCompleted<int>(270) });
            var e7 = scheduler.CreateHotObservable(new[] { OnNext(150, 1), OnCompleted<int>(280) });
            var e8 = scheduler.CreateHotObservable(new[] { OnNext(150, 1), OnCompleted<int>(290) });
            var e9 = scheduler.CreateHotObservable(new[] { OnNext(150, 1), OnCompleted<int>(300) });
            var e10 = scheduler.CreateHotObservable(new[] { OnNext(150, 1), OnCompleted<int>(310) });
            var e11 = scheduler.CreateHotObservable(new[] { OnNext(150, 1), OnCompleted<int>(320) });

            var res = scheduler.Start(() =>
                Observable.CombineLatest(e0, e1, e2, e3, e4, e5, e6, e7, e8, e9, e10, e11, (_0, _1, _2, _3, _4, _5, _6, _7, _8, _9, _10, _11) => 42)
            );

            res.Messages.AssertEqual(
                OnCompleted<int>(320)
            );

            var i = 0;
            foreach (var e in new[] { e0, e1, e2, e3, e4, e5, e6, e7, e8, e9, e10, e11 })
                e.Subscriptions.AssertEqual(Subscribe(200, 200 + (++i * 10)));
        }

        [TestMethod]
        public void CombineLatest_Empty13()
        {
            var scheduler = new TestScheduler();

            var e0 = scheduler.CreateHotObservable(new[] { OnNext(150, 1), OnCompleted<int>(210) });
            var e1 = scheduler.CreateHotObservable(new[] { OnNext(150, 1), OnCompleted<int>(220) });
            var e2 = scheduler.CreateHotObservable(new[] { OnNext(150, 1), OnCompleted<int>(230) });
            var e3 = scheduler.CreateHotObservable(new[] { OnNext(150, 1), OnCompleted<int>(240) });
            var e4 = scheduler.CreateHotObservable(new[] { OnNext(150, 1), OnCompleted<int>(250) });
            var e5 = scheduler.CreateHotObservable(new[] { OnNext(150, 1), OnCompleted<int>(260) });
            var e6 = scheduler.CreateHotObservable(new[] { OnNext(150, 1), OnCompleted<int>(270) });
            var e7 = scheduler.CreateHotObservable(new[] { OnNext(150, 1), OnCompleted<int>(280) });
            var e8 = scheduler.CreateHotObservable(new[] { OnNext(150, 1), OnCompleted<int>(290) });
            var e9 = scheduler.CreateHotObservable(new[] { OnNext(150, 1), OnCompleted<int>(300) });
            var e10 = scheduler.CreateHotObservable(new[] { OnNext(150, 1), OnCompleted<int>(310) });
            var e11 = scheduler.CreateHotObservable(new[] { OnNext(150, 1), OnCompleted<int>(320) });
            var e12 = scheduler.CreateHotObservable(new[] { OnNext(150, 1), OnCompleted<int>(330) });

            var res = scheduler.Start(() =>
                Observable.CombineLatest(e0, e1, e2, e3, e4, e5, e6, e7, e8, e9, e10, e11, e12, (_0, _1, _2, _3, _4, _5, _6, _7, _8, _9, _10, _11, _12) => 42)
            );

            res.Messages.AssertEqual(
                OnCompleted<int>(330)
            );

            var i = 0;
            foreach (var e in new[] { e0, e1, e2, e3, e4, e5, e6, e7, e8, e9, e10, e11, e12 })
                e.Subscriptions.AssertEqual(Subscribe(200, 200 + (++i * 10)));
        }

        [TestMethod]
        public void CombineLatest_Empty14()
        {
            var scheduler = new TestScheduler();

            var e0 = scheduler.CreateHotObservable(new[] { OnNext(150, 1), OnCompleted<int>(210) });
            var e1 = scheduler.CreateHotObservable(new[] { OnNext(150, 1), OnCompleted<int>(220) });
            var e2 = scheduler.CreateHotObservable(new[] { OnNext(150, 1), OnCompleted<int>(230) });
            var e3 = scheduler.CreateHotObservable(new[] { OnNext(150, 1), OnCompleted<int>(240) });
            var e4 = scheduler.CreateHotObservable(new[] { OnNext(150, 1), OnCompleted<int>(250) });
            var e5 = scheduler.CreateHotObservable(new[] { OnNext(150, 1), OnCompleted<int>(260) });
            var e6 = scheduler.CreateHotObservable(new[] { OnNext(150, 1), OnCompleted<int>(270) });
            var e7 = scheduler.CreateHotObservable(new[] { OnNext(150, 1), OnCompleted<int>(280) });
            var e8 = scheduler.CreateHotObservable(new[] { OnNext(150, 1), OnCompleted<int>(290) });
            var e9 = scheduler.CreateHotObservable(new[] { OnNext(150, 1), OnCompleted<int>(300) });
            var e10 = scheduler.CreateHotObservable(new[] { OnNext(150, 1), OnCompleted<int>(310) });
            var e11 = scheduler.CreateHotObservable(new[] { OnNext(150, 1), OnCompleted<int>(320) });
            var e12 = scheduler.CreateHotObservable(new[] { OnNext(150, 1), OnCompleted<int>(330) });
            var e13 = scheduler.CreateHotObservable(new[] { OnNext(150, 1), OnCompleted<int>(340) });

            var res = scheduler.Start(() =>
                Observable.CombineLatest(e0, e1, e2, e3, e4, e5, e6, e7, e8, e9, e10, e11, e12, e13, (_0, _1, _2, _3, _4, _5, _6, _7, _8, _9, _10, _11, _12, _13) => 42)
            );

            res.Messages.AssertEqual(
                OnCompleted<int>(340)
            );

            var i = 0;
            foreach (var e in new[] { e0, e1, e2, e3, e4, e5, e6, e7, e8, e9, e10, e11, e12, e13 })
                e.Subscriptions.AssertEqual(Subscribe(200, 200 + (++i * 10)));
        }

        [TestMethod]
        public void CombineLatest_Empty15()
        {
            var scheduler = new TestScheduler();

            var e0 = scheduler.CreateHotObservable(new[] { OnNext(150, 1), OnCompleted<int>(210) });
            var e1 = scheduler.CreateHotObservable(new[] { OnNext(150, 1), OnCompleted<int>(220) });
            var e2 = scheduler.CreateHotObservable(new[] { OnNext(150, 1), OnCompleted<int>(230) });
            var e3 = scheduler.CreateHotObservable(new[] { OnNext(150, 1), OnCompleted<int>(240) });
            var e4 = scheduler.CreateHotObservable(new[] { OnNext(150, 1), OnCompleted<int>(250) });
            var e5 = scheduler.CreateHotObservable(new[] { OnNext(150, 1), OnCompleted<int>(260) });
            var e6 = scheduler.CreateHotObservable(new[] { OnNext(150, 1), OnCompleted<int>(270) });
            var e7 = scheduler.CreateHotObservable(new[] { OnNext(150, 1), OnCompleted<int>(280) });
            var e8 = scheduler.CreateHotObservable(new[] { OnNext(150, 1), OnCompleted<int>(290) });
            var e9 = scheduler.CreateHotObservable(new[] { OnNext(150, 1), OnCompleted<int>(300) });
            var e10 = scheduler.CreateHotObservable(new[] { OnNext(150, 1), OnCompleted<int>(310) });
            var e11 = scheduler.CreateHotObservable(new[] { OnNext(150, 1), OnCompleted<int>(320) });
            var e12 = scheduler.CreateHotObservable(new[] { OnNext(150, 1), OnCompleted<int>(330) });
            var e13 = scheduler.CreateHotObservable(new[] { OnNext(150, 1), OnCompleted<int>(340) });
            var e14 = scheduler.CreateHotObservable(new[] { OnNext(150, 1), OnCompleted<int>(350) });

            var res = scheduler.Start(() =>
                Observable.CombineLatest(e0, e1, e2, e3, e4, e5, e6, e7, e8, e9, e10, e11, e12, e13, e14, (_0, _1, _2, _3, _4, _5, _6, _7, _8, _9, _10, _11, _12, _13, _14) => 42)
            );

            res.Messages.AssertEqual(
                OnCompleted<int>(350)
            );

            var i = 0;
            foreach (var e in new[] { e0, e1, e2, e3, e4, e5, e6, e7, e8, e9, e10, e11, e12, e13, e14 })
                e.Subscriptions.AssertEqual(Subscribe(200, 200 + (++i * 10)));
        }

        [TestMethod]
        public void CombineLatest_Empty16()
        {
            var scheduler = new TestScheduler();

            var e0 = scheduler.CreateHotObservable(new[] { OnNext(150, 1), OnCompleted<int>(210) });
            var e1 = scheduler.CreateHotObservable(new[] { OnNext(150, 1), OnCompleted<int>(220) });
            var e2 = scheduler.CreateHotObservable(new[] { OnNext(150, 1), OnCompleted<int>(230) });
            var e3 = scheduler.CreateHotObservable(new[] { OnNext(150, 1), OnCompleted<int>(240) });
            var e4 = scheduler.CreateHotObservable(new[] { OnNext(150, 1), OnCompleted<int>(250) });
            var e5 = scheduler.CreateHotObservable(new[] { OnNext(150, 1), OnCompleted<int>(260) });
            var e6 = scheduler.CreateHotObservable(new[] { OnNext(150, 1), OnCompleted<int>(270) });
            var e7 = scheduler.CreateHotObservable(new[] { OnNext(150, 1), OnCompleted<int>(280) });
            var e8 = scheduler.CreateHotObservable(new[] { OnNext(150, 1), OnCompleted<int>(290) });
            var e9 = scheduler.CreateHotObservable(new[] { OnNext(150, 1), OnCompleted<int>(300) });
            var e10 = scheduler.CreateHotObservable(new[] { OnNext(150, 1), OnCompleted<int>(310) });
            var e11 = scheduler.CreateHotObservable(new[] { OnNext(150, 1), OnCompleted<int>(320) });
            var e12 = scheduler.CreateHotObservable(new[] { OnNext(150, 1), OnCompleted<int>(330) });
            var e13 = scheduler.CreateHotObservable(new[] { OnNext(150, 1), OnCompleted<int>(340) });
            var e14 = scheduler.CreateHotObservable(new[] { OnNext(150, 1), OnCompleted<int>(350) });
            var e15 = scheduler.CreateHotObservable(new[] { OnNext(150, 1), OnCompleted<int>(360) });

            var res = scheduler.Start(() =>
                Observable.CombineLatest(e0, e1, e2, e3, e4, e5, e6, e7, e8, e9, e10, e11, e12, e13, e14, e15, (_0, _1, _2, _3, _4, _5, _6, _7, _8, _9, _10, _11, _12, _13, _14, _15) => 42)
            );

            res.Messages.AssertEqual(
                OnCompleted<int>(360)
            );

            var i = 0;
            foreach (var e in new[] { e0, e1, e2, e3, e4, e5, e6, e7, e8, e9, e10, e11, e12, e13, e14, e15 })
                e.Subscriptions.AssertEqual(Subscribe(200, 200 + (++i * 10)));
        }
#endif

        #endregion

        #region Empty/Return

        [TestMethod]
        public void CombineLatest_EmptyReturn()
        {
            var scheduler = new TestScheduler();

            var e = scheduler.CreateHotObservable(
                OnNext(150, 1),
                OnCompleted<int>(210)
            );

            var o = scheduler.CreateHotObservable(
                OnNext(150, 1),
                OnNext(215, 2),
                OnCompleted<int>(220)
            );

            var res = scheduler.Start(() =>
                e.CombineLatest(o, (x, y) => x + y)
            );

            res.Messages.AssertEqual(
                OnCompleted<int>(215)
            );

            e.Subscriptions.AssertEqual(
                Subscribe(200, 210)
            );

            o.Subscriptions.AssertEqual(
                Subscribe(200, 215)
            );
        }

        [TestMethod]
        public void CombineLatest_ReturnEmpty()
        {
            var scheduler = new TestScheduler();

            var e = scheduler.CreateHotObservable(
                OnNext(150, 1),
                OnCompleted<int>(210)
            );

            var o = scheduler.CreateHotObservable(
                OnNext(150, 1),
                OnNext(215, 2),
                OnCompleted<int>(220)
            );

            var res = scheduler.Start(() =>
                o.CombineLatest(e, (x, y) => x + y)
            );

            res.Messages.AssertEqual(
                OnCompleted<int>(215)
            );

            e.Subscriptions.AssertEqual(
                Subscribe(200, 210)
            );

            o.Subscriptions.AssertEqual(
                Subscribe(200, 215)
            );
        }

        #endregion

        #region Never/Return

        [TestMethod]
        public void CombineLatest_NeverReturn()
        {
            var scheduler = new TestScheduler();

            var o = scheduler.CreateHotObservable(
                OnNext(150, 1),
                OnNext(215, 2),
                OnCompleted<int>(220)
            );

            var n = scheduler.CreateHotObservable(
                OnNext(150, 1)
            );

            var res = scheduler.Start(() =>
                n.CombineLatest(o, (x, y) => x + y)
            );
            
            res.Messages.AssertEqual(
            );

            o.Subscriptions.AssertEqual(
                Subscribe(200, 220)
            );

            n.Subscriptions.AssertEqual(
                Subscribe(200, 1000)
            );
        }

        [TestMethod]
        public void CombineLatest_ReturnNever()
        {
            var scheduler = new TestScheduler();

            var o = scheduler.CreateHotObservable(
                OnNext(150, 1),
                OnNext(215, 2),
                OnCompleted<int>(220)
            );

            var n = scheduler.CreateHotObservable(
                OnNext(150, 1)
            );

            var res = scheduler.Start(() =>
                o.CombineLatest(n, (x, y) => x + y)
            );

            res.Messages.AssertEqual(
            );

            o.Subscriptions.AssertEqual(
                Subscribe(200, 220)
            );

            n.Subscriptions.AssertEqual(
                Subscribe(200, 1000)
            );
        }

        #endregion

        #region Return/Return

        [TestMethod]
        public void CombineLatest_ReturnReturn()
        {
            var scheduler = new TestScheduler();

            var o1 = scheduler.CreateHotObservable(
                OnNext(150, 1),
                OnNext(215, 2),
                OnCompleted<int>(230)
            );

            var o2 = scheduler.CreateHotObservable(
                OnNext(150, 1),
                OnNext(220, 3),
                OnCompleted<int>(240)
            );

            var res = scheduler.Start(() =>
                o1.CombineLatest(o2, (x, y) => x + y)
            );

            res.Messages.AssertEqual(
                OnNext(220, 2 + 3),
                OnCompleted<int>(240)
            );

            o1.Subscriptions.AssertEqual(
                Subscribe(200, 230)
            );

            o2.Subscriptions.AssertEqual(
                Subscribe(200, 240)
            );
        }

        #endregion

        #region Empty/Error

        [TestMethod]
        public void CombineLatest_EmptyError()
        {
            var scheduler = new TestScheduler();

            var ex = new Exception();

            var o1 = scheduler.CreateHotObservable(
                OnNext(150, 1),
                OnCompleted<int>(230)
            );

            var o2 = scheduler.CreateHotObservable(
                OnNext(150, 1),
                OnError<int>(220, ex)
            );

            var res = scheduler.Start(() =>
                o1.CombineLatest(o2, (x, y) => x + y)
            );

            res.Messages.AssertEqual(
                OnError<int>(220, ex)
            );

            o1.Subscriptions.AssertEqual(
                Subscribe(200, 220)
            );

            o2.Subscriptions.AssertEqual(
                Subscribe(200, 220)
            );
        }

        [TestMethod]
        public void CombineLatest_ErrorEmpty()
        {
            var scheduler = new TestScheduler();

            var ex = new Exception();

            var e = scheduler.CreateHotObservable(
                OnNext(150, 1),
                OnCompleted<int>(230)
            );

            var f = scheduler.CreateHotObservable(
                OnNext(150, 1),
                OnError<int>(220, ex)
            );

            var res = scheduler.Start(() =>
                f.CombineLatest(e, (x, y) => x + y)
            );

            res.Messages.AssertEqual(
                OnError<int>(220, ex)
            );

            e.Subscriptions.AssertEqual(
                Subscribe(200, 220)
            );

            f.Subscriptions.AssertEqual(
                Subscribe(200, 220)
            );
        }

        #endregion

        #region Return/Throw

        [TestMethod]
        public void CombineLatest_ReturnThrow()
        {
            var scheduler = new TestScheduler();

            var ex = new Exception();

            var o1 = scheduler.CreateHotObservable(
                OnNext(150, 1),
                OnNext(210, 2),
                OnCompleted<int>(230)
            );

            var o2 = scheduler.CreateHotObservable(
                OnNext(150, 1),
                OnError<int>(220, ex)
            );

            var res = scheduler.Start(() =>
                o1.CombineLatest(o2, (x, y) => x + y)
            );

            res.Messages.AssertEqual(
                OnError<int>(220, ex)
            );

            o1.Subscriptions.AssertEqual(
                Subscribe(200, 220)
            );

            o2.Subscriptions.AssertEqual(
                Subscribe(200, 220)
            );
        }

        [TestMethod]
        public void CombineLatest_ThrowReturn()
        {
            var scheduler = new TestScheduler();

            var ex = new Exception();

            var o1 = scheduler.CreateHotObservable(
                OnNext(150, 1),
                OnNext(210, 2),
                OnCompleted<int>(230)
            );

            var o2 = scheduler.CreateHotObservable(
                OnNext(150, 1),
                OnError<int>(220, ex)
            );

            var res = scheduler.Start(() =>
                o2.CombineLatest(o1, (x, y) => x + y)
            );

            res.Messages.AssertEqual(
                OnError<int>(220, ex)
            );

            o1.Subscriptions.AssertEqual(
                Subscribe(200, 220)
            );

            o2.Subscriptions.AssertEqual(
                Subscribe(200, 220)
            );
        }

        #endregion

        #region Throw/Throw

        [TestMethod]
        public void CombineLatest_ThrowThrow()
        {
            var scheduler = new TestScheduler();

            var ex1 = new Exception();
            var ex2 = new Exception();

            var o1 = scheduler.CreateHotObservable(
                OnNext(150, 1),
                OnError<int>(220, ex1)
            );

            var o2 = scheduler.CreateHotObservable(
                OnNext(150, 1),
                OnError<int>(230, ex2)
            );

            var res = scheduler.Start(() =>
                o2.CombineLatest(o1, (x, y) => x + y)
            );

            res.Messages.AssertEqual(
                OnError<int>(220, ex1)
            );

            o1.Subscriptions.AssertEqual(
                Subscribe(200, 220)
            );

            o2.Subscriptions.AssertEqual(
                Subscribe(200, 220)
            );
        }

        [TestMethod]
        public void CombineLatest_ErrorThrow()
        {
            var scheduler = new TestScheduler();

            var ex1 = new Exception();
            var ex2 = new Exception();

            var o1 = scheduler.CreateHotObservable(
                OnNext(150, 1),
                OnNext(210, 2),
                OnError<int>(220, ex1)
            );

            var o2 = scheduler.CreateHotObservable(
                OnNext(150, 1),
                OnError<int>(230, ex2)
            );

            var res = scheduler.Start(() =>
                o2.CombineLatest(o1, (x, y) => x + y)
            );

            res.Messages.AssertEqual(
                OnError<int>(220, ex1)
            );

            o1.Subscriptions.AssertEqual(
                Subscribe(200, 220)
            );

            o2.Subscriptions.AssertEqual(
                Subscribe(200, 220)
            );
        }

        [TestMethod]
        public void CombineLatest_ThrowError()
        {
            var scheduler = new TestScheduler();

            var ex1 = new Exception();
            var ex2 = new Exception();

            var o1 = scheduler.CreateHotObservable(
                OnNext(150, 1),
                OnNext(210, 2),
                OnError<int>(220, ex1)
            );

            var o2 = scheduler.CreateHotObservable(
                OnNext(150, 1),
                OnError<int>(230, ex2)
            );

            var res = scheduler.Start(() =>
                o1.CombineLatest(o2, (x, y) => x + y)
            );

            res.Messages.AssertEqual(
                OnError<int>(220, ex1)
            );

            o1.Subscriptions.AssertEqual(
                Subscribe(200, 220)
            );

            o2.Subscriptions.AssertEqual(
                Subscribe(200, 220)
            );
        }

        #endregion

        #region Never/Throw

        [TestMethod]
        public void CombineLatest_NeverThrow()
        {
            var scheduler = new TestScheduler();

            var ex = new Exception();

            var o1 = scheduler.CreateHotObservable(
                OnNext(150, 1)
            );

            var o2 = scheduler.CreateHotObservable(
                OnNext(150, 1),
                OnError<int>(220, ex)
            );

            var res = scheduler.Start(() =>
                o1.CombineLatest(o2, (x, y) => x + y)
            );

            res.Messages.AssertEqual(
                OnError<int>(220, ex)
            );

            o1.Subscriptions.AssertEqual(
                Subscribe(200, 220)
            );

            o2.Subscriptions.AssertEqual(
                Subscribe(200, 220)
            );
        }

        [TestMethod]
        public void CombineLatest_ThrowNever()
        {
            var scheduler = new TestScheduler();

            var ex = new Exception();

            var o1 = scheduler.CreateHotObservable(
                OnNext(150, 1)
            );

            var o2 = scheduler.CreateHotObservable(
                OnNext(150, 1),
                OnError<int>(220, ex)
            );

            var res = scheduler.Start(() =>
                o2.CombineLatest(o1, (x, y) => x + y)
            );

            res.Messages.AssertEqual(
                OnError<int>(220, ex)
            );

            o1.Subscriptions.AssertEqual(
                Subscribe(200, 220)
            );

            o2.Subscriptions.AssertEqual(
                Subscribe(200, 220)
            );
        }

        #endregion

        #region Some/Throw

        [TestMethod]
        public void CombineLatest_SomeThrow()
        {
            var scheduler = new TestScheduler();

            var ex = new Exception();

            var o1 = scheduler.CreateHotObservable(
                OnNext(150, 1),
                OnNext(215, 2),
                OnCompleted<int>(230)
            );

            var o2 = scheduler.CreateHotObservable(
                OnNext(150, 1),
                OnError<int>(220, ex)
            );

            var res = scheduler.Start(() =>
                o1.CombineLatest(o2, (x, y) => x + y)
            );

            res.Messages.AssertEqual(
                OnError<int>(220, ex)
            );

            o1.Subscriptions.AssertEqual(
                Subscribe(200, 220)
            );

            o2.Subscriptions.AssertEqual(
                Subscribe(200, 220)
            );
        }

        [TestMethod]
        public void CombineLatest_ThrowSome()
        {
            var scheduler = new TestScheduler();

            var ex = new Exception();

            var o1 = scheduler.CreateHotObservable(
                OnNext(150, 1),
                OnNext(215, 2),
                OnCompleted<int>(230)
            );

            var o2 = scheduler.CreateHotObservable(
                OnNext(150, 1),
                OnError<int>(220, ex)
            );

            var res = scheduler.Start(() =>
                o2.CombineLatest(o1, (x, y) => x + y)
            );

            res.Messages.AssertEqual(
                OnError<int>(220, ex)
            );

            o1.Subscriptions.AssertEqual(
                Subscribe(200, 220)
            );

            o2.Subscriptions.AssertEqual(
                Subscribe(200, 220)
            );
        }

        #endregion

        #region ThrowAfterCompleted

        [TestMethod]
        public void CombineLatest_ThrowAfterCompleteLeft()
        {
            var scheduler = new TestScheduler();

            var ex = new Exception();

            var o1 = scheduler.CreateHotObservable(
                OnNext(150, 1),
                OnNext(215, 2),
                OnCompleted<int>(220)
            );

            var o2 = scheduler.CreateHotObservable(
                OnNext(150, 1),
                OnError<int>(230, ex)
            );

            var res = scheduler.Start(() =>
                o2.CombineLatest(o1, (x, y) => x + y)
            );
            
            res.Messages.AssertEqual(
                OnError<int>(230, ex)
            );

            o1.Subscriptions.AssertEqual(
                Subscribe(200, 220)
            );

            o2.Subscriptions.AssertEqual(
                Subscribe(200, 230)
            );
        }

        [TestMethod]
        public void CombineLatest_ThrowAfterCompleteRight()
        {
            var scheduler = new TestScheduler();

            var ex = new Exception();

            var o1 = scheduler.CreateHotObservable(
                OnNext(150, 1),
                OnNext(215, 2),
                OnCompleted<int>(220)
            );

            var o2 = scheduler.CreateHotObservable(
                OnNext(150, 1),
                OnError<int>(230, ex)
            );

            var res = scheduler.Start(() =>
                o1.CombineLatest(o2, (x, y) => x + y)
            );

            res.Messages.AssertEqual(
                OnError<int>(230, ex)
            );

            o1.Subscriptions.AssertEqual(
                Subscribe(200, 220)
            );

            o2.Subscriptions.AssertEqual(
                Subscribe(200, 230)
            );
        }

        #endregion

        #region Basics

        [TestMethod]
        public void CombineLatest_InterleavedWithTail()
        {
            var scheduler = new TestScheduler();

            var o1 = scheduler.CreateHotObservable(
                OnNext(150, 1),
                OnNext(215, 2),
                OnNext(225, 4),
                OnCompleted<int>(230)
            );

            var o2 = scheduler.CreateHotObservable(
                OnNext(150, 1),
                OnNext(220, 3),
                OnNext(230, 5),
                OnNext(235, 6),
                OnNext(240, 7),
                OnCompleted<int>(250)
            );

            var res = scheduler.Start(() =>
                o2.CombineLatest(o1, (x, y) => x + y)
            );
            
            res.Messages.AssertEqual(
                OnNext(220, 2 + 3),
                OnNext(225, 3 + 4),
                OnNext(230, 4 + 5),
                OnNext(235, 4 + 6),
                OnNext(240, 4 + 7),
                OnCompleted<int>(250)
            );

            o1.Subscriptions.AssertEqual(
                Subscribe(200, 230)
            );

            o2.Subscriptions.AssertEqual(
                Subscribe(200, 250)
            );
        }

        [TestMethod]
        public void CombineLatest_Consecutive()
        {
            var scheduler = new TestScheduler();

            var o1 = scheduler.CreateHotObservable(
                OnNext(150, 1),
                OnNext(215, 2),
                OnNext(225, 4),
                OnCompleted<int>(230)
            );

            var o2 = scheduler.CreateHotObservable(
                OnNext(150, 1),
                OnNext(235, 6),
                OnNext(240, 7),
                OnCompleted<int>(250)
            );

            var res = scheduler.Start(() =>
                o2.CombineLatest(o1, (x, y) => x + y)
            );

            res.Messages.AssertEqual(
                OnNext(235, 4 + 6),
                OnNext(240, 4 + 7),
                OnCompleted<int>(250)
            );

            o1.Subscriptions.AssertEqual(
                Subscribe(200, 230)
            );

            o2.Subscriptions.AssertEqual(
                Subscribe(200, 250)
            );
        }

        [TestMethod]
        public void CombineLatest_ConsecutiveEndWithErrorLeft()
        {
            var scheduler = new TestScheduler();

            var ex = new Exception();

            var o1 = scheduler.CreateHotObservable(
                OnNext(150, 1),
                OnNext(215, 2),
                OnNext(225, 4),
                OnError<int>(230, ex)
            );

            var o2 = scheduler.CreateHotObservable(
                OnNext(150, 1),
                OnNext(235, 6),
                OnNext(240, 7),
                OnCompleted<int>(250)
            );

            var res = scheduler.Start(() =>
                o2.CombineLatest(o1, (x, y) => x + y)
            );

            res.Messages.AssertEqual(
                OnError<int>(230, ex)
            );

            o1.Subscriptions.AssertEqual(
                Subscribe(200, 230)
            );

            o2.Subscriptions.AssertEqual(
                Subscribe(200, 230)
            );
        }

        [TestMethod]
        public void CombineLatest_ConsecutiveEndWithErrorRight()
        {
            var scheduler = new TestScheduler();

            var ex = new Exception();

            var o1 = scheduler.CreateHotObservable(
                OnNext(150, 1),
                OnNext(215, 2),
                OnNext(225, 4),
                OnCompleted<int>(250)
            );

            var o2 = scheduler.CreateHotObservable(
                OnNext(150, 1),
                OnNext(235, 6),
                OnNext(240, 7),
                OnError<int>(245, ex)
            );

            var res = scheduler.Start(() =>
                o2.CombineLatest(o1, (x, y) => x + y)
            );

            res.Messages.AssertEqual(
                OnNext(235, 4 + 6),
                OnNext(240, 4 + 7),
                OnError<int>(245, ex)
            );

            o1.Subscriptions.AssertEqual(
                Subscribe(200, 245)
            );

            o2.Subscriptions.AssertEqual(
                Subscribe(200, 245)
            );
        }

        #endregion

        #region SelectorThrows

        [TestMethod]
        public void CombineLatest_SelectorThrows()
        {
            var scheduler = new TestScheduler();

            var o1 = scheduler.CreateHotObservable(
                OnNext(150, 1),
                OnNext(215, 2),
                OnCompleted<int>(230)
            );

            var o2 = scheduler.CreateHotObservable(
                OnNext(150, 1),
                OnNext(220, 3),
                OnCompleted<int>(240)
            );

            var ex = new Exception();

            var res = scheduler.Start(() =>
                o2.CombineLatest<int, int, int>(o1, (x, y) => { throw ex; })
            );

            res.Messages.AssertEqual(
                OnError<int>(220, ex)
            );

            o1.Subscriptions.AssertEqual(
                Subscribe(200, 220)
            );

            o2.Subscriptions.AssertEqual(
                Subscribe(200, 220)
            );
        }

        [TestMethod]
        public void CombineLatest_SelectorThrowsN()
        {
            var scheduler = new TestScheduler();
            
            var e0 = scheduler.CreateHotObservable(new[] { OnNext(150, 1), OnNext(210, 1), OnCompleted<int>(400) });
            var e1 = scheduler.CreateHotObservable(new[] { OnNext(150, 1), OnNext(220, 2), OnCompleted<int>(400) });
            var e2 = scheduler.CreateHotObservable(new[] { OnNext(150, 1), OnNext(230, 3), OnCompleted<int>(400) });
            
            var ex = new Exception();
            Func<IList<int>, int> f = xs => { throw ex; };
            
            var res = scheduler.Start(() =>
                Observable.CombineLatest(new[] { e0, e1, e2 }, f)
            );

            res.Messages.AssertEqual(
                OnError<int>(230, ex)
            );

            var es = new[] { e0, e1, e2 };
            foreach (var e in es)
                e.Subscriptions.AssertEqual(Subscribe(200, 200 + 10 * es.Length));
        }

        [TestMethod]
        public void CombineLatest_SelectorThrows2()
        {
            var scheduler = new TestScheduler();
            
            var e0 = scheduler.CreateHotObservable(new[] { OnNext(150, 1), OnNext(210, 1), OnCompleted<int>(400) });
            var e1 = scheduler.CreateHotObservable(new[] { OnNext(150, 1), OnNext(220, 2), OnCompleted<int>(400) });
            
            var ex = new Exception();
            Func<int> f = () => { throw ex; };
            
            var res = scheduler.Start(() =>
                Observable.CombineLatest(e0, e1, (_0, _1) => f())
            );
            
            res.Messages.AssertEqual(
                OnError<int>(220, ex)
            );

            var es = new[] { e0, e1 };
            foreach (var e in es)
                e.Subscriptions.AssertEqual(Subscribe(200, 200 + 10 * es.Length));
        }

        [TestMethod]
        public void CombineLatest_SelectorThrows3()
        {
            var scheduler = new TestScheduler();
            
            var e0 = scheduler.CreateHotObservable(new[] { OnNext(150, 1), OnNext(210, 1), OnCompleted<int>(400) });
            var e1 = scheduler.CreateHotObservable(new[] { OnNext(150, 1), OnNext(220, 2), OnCompleted<int>(400) });
            var e2 = scheduler.CreateHotObservable(new[] { OnNext(150, 1), OnNext(230, 3), OnCompleted<int>(400) });
            
            var ex = new Exception();
            Func<int> f = () => { throw ex; };
            
            var res = scheduler.Start(() =>
                Observable.CombineLatest(e0, e1, e2, (_0, _1, _2) => f())
            );

            res.Messages.AssertEqual(
                OnError<int>(230, ex)
            );

            var es = new[] { e0, e1, e2 };
            foreach (var e in es)
                e.Subscriptions.AssertEqual(Subscribe(200, 200 + 10 * es.Length));
        }

        [TestMethod]
        public void CombineLatest_SelectorThrows4()
        {
            var scheduler = new TestScheduler();
            
            var e0 = scheduler.CreateHotObservable(new[] { OnNext(150, 1), OnNext(210, 1), OnCompleted<int>(400) });
            var e1 = scheduler.CreateHotObservable(new[] { OnNext(150, 1), OnNext(220, 2), OnCompleted<int>(400) });
            var e2 = scheduler.CreateHotObservable(new[] { OnNext(150, 1), OnNext(230, 3), OnCompleted<int>(400) });
            var e3 = scheduler.CreateHotObservable(new[] { OnNext(150, 1), OnNext(240, 4), OnCompleted<int>(400) });
            
            var ex = new Exception();
            Func<int> f = () => { throw ex; };
            
            var res = scheduler.Start(() =>
                Observable.CombineLatest(e0, e1, e2, e3, (_0, _1, _2, _3) => f())
            );
            
            res.Messages.AssertEqual(
                OnError<int>(240, ex)
            );

            var es = new[] { e0, e1, e2, e3 };
            foreach (var e in es)
                e.Subscriptions.AssertEqual(Subscribe(200, 200 + 10 * es.Length));
        }

#if !NO_LARGEARITY
        [TestMethod]
        public void CombineLatest_SelectorThrows5()
        {
            var scheduler = new TestScheduler();
            
            var e0 = scheduler.CreateHotObservable(new[] { OnNext(150, 1), OnNext(210, 1), OnCompleted<int>(400) });
            var e1 = scheduler.CreateHotObservable(new[] { OnNext(150, 1), OnNext(220, 2), OnCompleted<int>(400) });
            var e2 = scheduler.CreateHotObservable(new[] { OnNext(150, 1), OnNext(230, 3), OnCompleted<int>(400) });
            var e3 = scheduler.CreateHotObservable(new[] { OnNext(150, 1), OnNext(240, 4), OnCompleted<int>(400) });
            var e4 = scheduler.CreateHotObservable(new[] { OnNext(150, 1), OnNext(250, 5), OnCompleted<int>(400) });
            
            var ex = new Exception();
            Func<int> f = () => { throw ex; };
            
            var res = scheduler.Start(() =>
                Observable.CombineLatest(e0, e1, e2, e3, e4, (_0, _1, _2, _3, _4) => f())
            );
            
            res.Messages.AssertEqual(
                OnError<int>(250, ex)
            );

            var es = new[] { e0, e1, e2, e3, e4 };
            foreach (var e in es)
                e.Subscriptions.AssertEqual(Subscribe(200, 200 + 10 * es.Length));
        }

        [TestMethod]
        public void CombineLatest_SelectorThrows6()
        {
            var scheduler = new TestScheduler();
            
            var e0 = scheduler.CreateHotObservable(new[] { OnNext(150, 1), OnNext(210, 1), OnCompleted<int>(400) });
            var e1 = scheduler.CreateHotObservable(new[] { OnNext(150, 1), OnNext(220, 2), OnCompleted<int>(400) });
            var e2 = scheduler.CreateHotObservable(new[] { OnNext(150, 1), OnNext(230, 3), OnCompleted<int>(400) });
            var e3 = scheduler.CreateHotObservable(new[] { OnNext(150, 1), OnNext(240, 4), OnCompleted<int>(400) });
            var e4 = scheduler.CreateHotObservable(new[] { OnNext(150, 1), OnNext(250, 5), OnCompleted<int>(400) });
            var e5 = scheduler.CreateHotObservable(new[] { OnNext(150, 1), OnNext(260, 6), OnCompleted<int>(400) });
            
            var ex = new Exception();
            Func<int> f = () => { throw ex; };
            
            var res = scheduler.Start(() =>
                Observable.CombineLatest(e0, e1, e2, e3, e4, e5, (_0, _1, _2, _3, _4, _5) => f())
            );
            
            res.Messages.AssertEqual(
                OnError<int>(260, ex)
            );

            var es = new[] { e0, e1, e2, e3, e4, e5 };
            foreach (var e in es)
                e.Subscriptions.AssertEqual(Subscribe(200, 200 + 10 * es.Length));
        }

        [TestMethod]
        public void CombineLatest_SelectorThrows7()
        {
            var scheduler = new TestScheduler();
            
            var e0 = scheduler.CreateHotObservable(new[] { OnNext(150, 1), OnNext(210, 1), OnCompleted<int>(400) });
            var e1 = scheduler.CreateHotObservable(new[] { OnNext(150, 1), OnNext(220, 2), OnCompleted<int>(400) });
            var e2 = scheduler.CreateHotObservable(new[] { OnNext(150, 1), OnNext(230, 3), OnCompleted<int>(400) });
            var e3 = scheduler.CreateHotObservable(new[] { OnNext(150, 1), OnNext(240, 4), OnCompleted<int>(400) });
            var e4 = scheduler.CreateHotObservable(new[] { OnNext(150, 1), OnNext(250, 5), OnCompleted<int>(400) });
            var e5 = scheduler.CreateHotObservable(new[] { OnNext(150, 1), OnNext(260, 6), OnCompleted<int>(400) });
            var e6 = scheduler.CreateHotObservable(new[] { OnNext(150, 1), OnNext(270, 7), OnCompleted<int>(400) });
            
            var ex = new Exception();
            Func<int> f = () => { throw ex; };
            
            var res = scheduler.Start(() =>
                Observable.CombineLatest(e0, e1, e2, e3, e4, e5, e6, (_0, _1, _2, _3, _4, _5, _6) => f())
            );
            
            res.Messages.AssertEqual(
                OnError<int>(270, ex)
            );

            var es = new[] { e0, e1, e2, e3, e4, e5, e6 };
            foreach (var e in es)
                e.Subscriptions.AssertEqual(Subscribe(200, 200 + 10 * es.Length));
        }

        [TestMethod]
        public void CombineLatest_SelectorThrows8()
        {
            var scheduler = new TestScheduler();
            
            var e0 = scheduler.CreateHotObservable(new[] { OnNext(150, 1), OnNext(210, 1), OnCompleted<int>(400) });
            var e1 = scheduler.CreateHotObservable(new[] { OnNext(150, 1), OnNext(220, 2), OnCompleted<int>(400) });
            var e2 = scheduler.CreateHotObservable(new[] { OnNext(150, 1), OnNext(230, 3), OnCompleted<int>(400) });
            var e3 = scheduler.CreateHotObservable(new[] { OnNext(150, 1), OnNext(240, 4), OnCompleted<int>(400) });
            var e4 = scheduler.CreateHotObservable(new[] { OnNext(150, 1), OnNext(250, 5), OnCompleted<int>(400) });
            var e5 = scheduler.CreateHotObservable(new[] { OnNext(150, 1), OnNext(260, 6), OnCompleted<int>(400) });
            var e6 = scheduler.CreateHotObservable(new[] { OnNext(150, 1), OnNext(270, 7), OnCompleted<int>(400) });
            var e7 = scheduler.CreateHotObservable(new[] { OnNext(150, 1), OnNext(280, 8), OnCompleted<int>(400) });
            
            var ex = new Exception();
            Func<int> f = () => { throw ex; };
            
            var res = scheduler.Start(() =>
                Observable.CombineLatest(e0, e1, e2, e3, e4, e5, e6, e7, (_0, _1, _2, _3, _4, _5, _6, _7) => f())
            );
            
            res.Messages.AssertEqual(
                OnError<int>(280, ex)
            );

            var es = new[] { e0, e1, e2, e3, e4, e5, e6, e7 };
            foreach (var e in es)
                e.Subscriptions.AssertEqual(Subscribe(200, 200 + 10 * es.Length));
        }

        [TestMethod]
        public void CombineLatest_SelectorThrows9()
        {
            var scheduler = new TestScheduler();
            
            var e0 = scheduler.CreateHotObservable(new[] { OnNext(150, 1), OnNext(210, 1), OnCompleted<int>(400) });
            var e1 = scheduler.CreateHotObservable(new[] { OnNext(150, 1), OnNext(220, 2), OnCompleted<int>(400) });
            var e2 = scheduler.CreateHotObservable(new[] { OnNext(150, 1), OnNext(230, 3), OnCompleted<int>(400) });
            var e3 = scheduler.CreateHotObservable(new[] { OnNext(150, 1), OnNext(240, 4), OnCompleted<int>(400) });
            var e4 = scheduler.CreateHotObservable(new[] { OnNext(150, 1), OnNext(250, 5), OnCompleted<int>(400) });
            var e5 = scheduler.CreateHotObservable(new[] { OnNext(150, 1), OnNext(260, 6), OnCompleted<int>(400) });
            var e6 = scheduler.CreateHotObservable(new[] { OnNext(150, 1), OnNext(270, 7), OnCompleted<int>(400) });
            var e7 = scheduler.CreateHotObservable(new[] { OnNext(150, 1), OnNext(280, 8), OnCompleted<int>(400) });
            var e8 = scheduler.CreateHotObservable(new[] { OnNext(150, 1), OnNext(290, 9), OnCompleted<int>(400) });
            
            var ex = new Exception();
            Func<int> f = () => { throw ex; };
            
            var res = scheduler.Start(() =>
                Observable.CombineLatest(e0, e1, e2, e3, e4, e5, e6, e7, e8, (_0, _1, _2, _3, _4, _5, _6, _7, _8) => f())
            );
            
            res.Messages.AssertEqual(
                OnError<int>(290, ex)
            );

            var es = new[] { e0, e1, e2, e3, e4, e5, e6, e7, e8 };
            foreach (var e in es)
                e.Subscriptions.AssertEqual(Subscribe(200, 200 + 10 * es.Length));
        }

        [TestMethod]
        public void CombineLatest_SelectorThrows10()
        {
            var scheduler = new TestScheduler();
            
            var e0 = scheduler.CreateHotObservable(new[] { OnNext(150, 1), OnNext(210, 1), OnCompleted<int>(400) });
            var e1 = scheduler.CreateHotObservable(new[] { OnNext(150, 1), OnNext(220, 2), OnCompleted<int>(400) });
            var e2 = scheduler.CreateHotObservable(new[] { OnNext(150, 1), OnNext(230, 3), OnCompleted<int>(400) });
            var e3 = scheduler.CreateHotObservable(new[] { OnNext(150, 1), OnNext(240, 4), OnCompleted<int>(400) });
            var e4 = scheduler.CreateHotObservable(new[] { OnNext(150, 1), OnNext(250, 5), OnCompleted<int>(400) });
            var e5 = scheduler.CreateHotObservable(new[] { OnNext(150, 1), OnNext(260, 6), OnCompleted<int>(400) });
            var e6 = scheduler.CreateHotObservable(new[] { OnNext(150, 1), OnNext(270, 7), OnCompleted<int>(400) });
            var e7 = scheduler.CreateHotObservable(new[] { OnNext(150, 1), OnNext(280, 8), OnCompleted<int>(400) });
            var e8 = scheduler.CreateHotObservable(new[] { OnNext(150, 1), OnNext(290, 9), OnCompleted<int>(400) });
            var e9 = scheduler.CreateHotObservable(new[] { OnNext(150, 1), OnNext(300, 10), OnCompleted<int>(400) });
            
            var ex = new Exception();
            Func<int> f = () => { throw ex; };
            
            var res = scheduler.Start(() =>
                Observable.CombineLatest(e0, e1, e2, e3, e4, e5, e6, e7, e8, e9, (_0, _1, _2, _3, _4, _5, _6, _7, _8, _9) => f())
            );
            
            res.Messages.AssertEqual(
                OnError<int>(300, ex)
            );

            var es = new[] { e0, e1, e2, e3, e4, e5, e6, e7, e8, e9 };
            foreach (var e in es)
                e.Subscriptions.AssertEqual(Subscribe(200, 200 + 10 * es.Length));
        }

        [TestMethod]
        public void CombineLatest_SelectorThrows11()
        {
            var scheduler = new TestScheduler();
            
            var e0 = scheduler.CreateHotObservable(new[] { OnNext(150, 1), OnNext(210, 1), OnCompleted<int>(400) });
            var e1 = scheduler.CreateHotObservable(new[] { OnNext(150, 1), OnNext(220, 2), OnCompleted<int>(400) });
            var e2 = scheduler.CreateHotObservable(new[] { OnNext(150, 1), OnNext(230, 3), OnCompleted<int>(400) });
            var e3 = scheduler.CreateHotObservable(new[] { OnNext(150, 1), OnNext(240, 4), OnCompleted<int>(400) });
            var e4 = scheduler.CreateHotObservable(new[] { OnNext(150, 1), OnNext(250, 5), OnCompleted<int>(400) });
            var e5 = scheduler.CreateHotObservable(new[] { OnNext(150, 1), OnNext(260, 6), OnCompleted<int>(400) });
            var e6 = scheduler.CreateHotObservable(new[] { OnNext(150, 1), OnNext(270, 7), OnCompleted<int>(400) });
            var e7 = scheduler.CreateHotObservable(new[] { OnNext(150, 1), OnNext(280, 8), OnCompleted<int>(400) });
            var e8 = scheduler.CreateHotObservable(new[] { OnNext(150, 1), OnNext(290, 9), OnCompleted<int>(400) });
            var e9 = scheduler.CreateHotObservable(new[] { OnNext(150, 1), OnNext(300, 10), OnCompleted<int>(400) });
            var e10 = scheduler.CreateHotObservable(new[] { OnNext(150, 1), OnNext(310, 11), OnCompleted<int>(400) });
            
            var ex = new Exception();
            Func<int> f = () => { throw ex; };
            
            var res = scheduler.Start(() =>
                Observable.CombineLatest(e0, e1, e2, e3, e4, e5, e6, e7, e8, e9, e10, (_0, _1, _2, _3, _4, _5, _6, _7, _8, _9, _10) => f())
            );
            
            res.Messages.AssertEqual(
                OnError<int>(310, ex)
            );

            var es = new[] { e0, e1, e2, e3, e4, e5, e6, e7, e8, e9, e10 };
            foreach (var e in es)
                e.Subscriptions.AssertEqual(Subscribe(200, 200 + 10 * es.Length));
        }

        [TestMethod]
        public void CombineLatest_SelectorThrows12()
        {
            var scheduler = new TestScheduler();
            
            var e0 = scheduler.CreateHotObservable(new[] { OnNext(150, 1), OnNext(210, 1), OnCompleted<int>(400) });
            var e1 = scheduler.CreateHotObservable(new[] { OnNext(150, 1), OnNext(220, 2), OnCompleted<int>(400) });
            var e2 = scheduler.CreateHotObservable(new[] { OnNext(150, 1), OnNext(230, 3), OnCompleted<int>(400) });
            var e3 = scheduler.CreateHotObservable(new[] { OnNext(150, 1), OnNext(240, 4), OnCompleted<int>(400) });
            var e4 = scheduler.CreateHotObservable(new[] { OnNext(150, 1), OnNext(250, 5), OnCompleted<int>(400) });
            var e5 = scheduler.CreateHotObservable(new[] { OnNext(150, 1), OnNext(260, 6), OnCompleted<int>(400) });
            var e6 = scheduler.CreateHotObservable(new[] { OnNext(150, 1), OnNext(270, 7), OnCompleted<int>(400) });
            var e7 = scheduler.CreateHotObservable(new[] { OnNext(150, 1), OnNext(280, 8), OnCompleted<int>(400) });
            var e8 = scheduler.CreateHotObservable(new[] { OnNext(150, 1), OnNext(290, 9), OnCompleted<int>(400) });
            var e9 = scheduler.CreateHotObservable(new[] { OnNext(150, 1), OnNext(300, 10), OnCompleted<int>(400) });
            var e10 = scheduler.CreateHotObservable(new[] { OnNext(150, 1), OnNext(310, 11), OnCompleted<int>(400) });
            var e11 = scheduler.CreateHotObservable(new[] { OnNext(150, 1), OnNext(320, 12), OnCompleted<int>(400) });
            
            var ex = new Exception();
            Func<int> f = () => { throw ex; };
            
            var res = scheduler.Start(() =>
                Observable.CombineLatest(e0, e1, e2, e3, e4, e5, e6, e7, e8, e9, e10, e11, (_0, _1, _2, _3, _4, _5, _6, _7, _8, _9, _10, _11) => f())
            );
            
            res.Messages.AssertEqual(
                OnError<int>(320, ex)
            );

            var es = new[] { e0, e1, e2, e3, e4, e5, e6, e7, e8, e9, e10, e11 };
            foreach (var e in es)
                e.Subscriptions.AssertEqual(Subscribe(200, 200 + 10 * es.Length));
        }

        [TestMethod]
        public void CombineLatest_SelectorThrows13()
        {
            var scheduler = new TestScheduler();
            
            var e0 = scheduler.CreateHotObservable(new[] { OnNext(150, 1), OnNext(210, 1), OnCompleted<int>(400) });
            var e1 = scheduler.CreateHotObservable(new[] { OnNext(150, 1), OnNext(220, 2), OnCompleted<int>(400) });
            var e2 = scheduler.CreateHotObservable(new[] { OnNext(150, 1), OnNext(230, 3), OnCompleted<int>(400) });
            var e3 = scheduler.CreateHotObservable(new[] { OnNext(150, 1), OnNext(240, 4), OnCompleted<int>(400) });
            var e4 = scheduler.CreateHotObservable(new[] { OnNext(150, 1), OnNext(250, 5), OnCompleted<int>(400) });
            var e5 = scheduler.CreateHotObservable(new[] { OnNext(150, 1), OnNext(260, 6), OnCompleted<int>(400) });
            var e6 = scheduler.CreateHotObservable(new[] { OnNext(150, 1), OnNext(270, 7), OnCompleted<int>(400) });
            var e7 = scheduler.CreateHotObservable(new[] { OnNext(150, 1), OnNext(280, 8), OnCompleted<int>(400) });
            var e8 = scheduler.CreateHotObservable(new[] { OnNext(150, 1), OnNext(290, 9), OnCompleted<int>(400) });
            var e9 = scheduler.CreateHotObservable(new[] { OnNext(150, 1), OnNext(300, 10), OnCompleted<int>(400) });
            var e10 = scheduler.CreateHotObservable(new[] { OnNext(150, 1), OnNext(310, 11), OnCompleted<int>(400) });
            var e11 = scheduler.CreateHotObservable(new[] { OnNext(150, 1), OnNext(320, 12), OnCompleted<int>(400) });
            var e12 = scheduler.CreateHotObservable(new[] { OnNext(150, 1), OnNext(330, 13), OnCompleted<int>(400) });
            
            var ex = new Exception();
            Func<int> f = () => { throw ex; };
            
            var res = scheduler.Start(() =>
                Observable.CombineLatest(e0, e1, e2, e3, e4, e5, e6, e7, e8, e9, e10, e11, e12, (_0, _1, _2, _3, _4, _5, _6, _7, _8, _9, _10, _11, _12) => f())
            );
            
            res.Messages.AssertEqual(
                OnError<int>(330, ex)
            );

            var es = new[] { e0, e1, e2, e3, e4, e5, e6, e7, e8, e9, e10, e11, e12 };
            foreach (var e in es)
                e.Subscriptions.AssertEqual(Subscribe(200, 200 + 10 * es.Length));
        }

        [TestMethod]
        public void CombineLatest_SelectorThrows14()
        {
            var scheduler = new TestScheduler();
            
            var e0 = scheduler.CreateHotObservable(new[] { OnNext(150, 1), OnNext(210, 1), OnCompleted<int>(400) });
            var e1 = scheduler.CreateHotObservable(new[] { OnNext(150, 1), OnNext(220, 2), OnCompleted<int>(400) });
            var e2 = scheduler.CreateHotObservable(new[] { OnNext(150, 1), OnNext(230, 3), OnCompleted<int>(400) });
            var e3 = scheduler.CreateHotObservable(new[] { OnNext(150, 1), OnNext(240, 4), OnCompleted<int>(400) });
            var e4 = scheduler.CreateHotObservable(new[] { OnNext(150, 1), OnNext(250, 5), OnCompleted<int>(400) });
            var e5 = scheduler.CreateHotObservable(new[] { OnNext(150, 1), OnNext(260, 6), OnCompleted<int>(400) });
            var e6 = scheduler.CreateHotObservable(new[] { OnNext(150, 1), OnNext(270, 7), OnCompleted<int>(400) });
            var e7 = scheduler.CreateHotObservable(new[] { OnNext(150, 1), OnNext(280, 8), OnCompleted<int>(400) });
            var e8 = scheduler.CreateHotObservable(new[] { OnNext(150, 1), OnNext(290, 9), OnCompleted<int>(400) });
            var e9 = scheduler.CreateHotObservable(new[] { OnNext(150, 1), OnNext(300, 10), OnCompleted<int>(400) });
            var e10 = scheduler.CreateHotObservable(new[] { OnNext(150, 1), OnNext(310, 11), OnCompleted<int>(400) });
            var e11 = scheduler.CreateHotObservable(new[] { OnNext(150, 1), OnNext(320, 12), OnCompleted<int>(400) });
            var e12 = scheduler.CreateHotObservable(new[] { OnNext(150, 1), OnNext(330, 13), OnCompleted<int>(400) });
            var e13 = scheduler.CreateHotObservable(new[] { OnNext(150, 1), OnNext(340, 14), OnCompleted<int>(400) });
            
            var ex = new Exception();
            Func<int> f = () => { throw ex; };
            
            var res = scheduler.Start(() =>
                Observable.CombineLatest(e0, e1, e2, e3, e4, e5, e6, e7, e8, e9, e10, e11, e12, e13, (_0, _1, _2, _3, _4, _5, _6, _7, _8, _9, _10, _11, _12, _13) => f())
            );
            
            res.Messages.AssertEqual(
                OnError<int>(340, ex)
            );

            var es = new[] { e0, e1, e2, e3, e4, e5, e6, e7, e8, e9, e10, e11, e12, e13 };
            foreach (var e in es)
                e.Subscriptions.AssertEqual(Subscribe(200, 200 + 10 * es.Length));
        }

        [TestMethod]
        public void CombineLatest_SelectorThrows15()
        {
            var scheduler = new TestScheduler();
            
            var e0 = scheduler.CreateHotObservable(new[] { OnNext(150, 1), OnNext(210, 1), OnCompleted<int>(400) });
            var e1 = scheduler.CreateHotObservable(new[] { OnNext(150, 1), OnNext(220, 2), OnCompleted<int>(400) });
            var e2 = scheduler.CreateHotObservable(new[] { OnNext(150, 1), OnNext(230, 3), OnCompleted<int>(400) });
            var e3 = scheduler.CreateHotObservable(new[] { OnNext(150, 1), OnNext(240, 4), OnCompleted<int>(400) });
            var e4 = scheduler.CreateHotObservable(new[] { OnNext(150, 1), OnNext(250, 5), OnCompleted<int>(400) });
            var e5 = scheduler.CreateHotObservable(new[] { OnNext(150, 1), OnNext(260, 6), OnCompleted<int>(400) });
            var e6 = scheduler.CreateHotObservable(new[] { OnNext(150, 1), OnNext(270, 7), OnCompleted<int>(400) });
            var e7 = scheduler.CreateHotObservable(new[] { OnNext(150, 1), OnNext(280, 8), OnCompleted<int>(400) });
            var e8 = scheduler.CreateHotObservable(new[] { OnNext(150, 1), OnNext(290, 9), OnCompleted<int>(400) });
            var e9 = scheduler.CreateHotObservable(new[] { OnNext(150, 1), OnNext(300, 10), OnCompleted<int>(400) });
            var e10 = scheduler.CreateHotObservable(new[] { OnNext(150, 1), OnNext(310, 11), OnCompleted<int>(400) });
            var e11 = scheduler.CreateHotObservable(new[] { OnNext(150, 1), OnNext(320, 12), OnCompleted<int>(400) });
            var e12 = scheduler.CreateHotObservable(new[] { OnNext(150, 1), OnNext(330, 13), OnCompleted<int>(400) });
            var e13 = scheduler.CreateHotObservable(new[] { OnNext(150, 1), OnNext(340, 14), OnCompleted<int>(400) });
            var e14 = scheduler.CreateHotObservable(new[] { OnNext(150, 1), OnNext(350, 15), OnCompleted<int>(400) });
            
            var ex = new Exception();
            Func<int> f = () => { throw ex; };
            
            var res = scheduler.Start(() =>
                Observable.CombineLatest(e0, e1, e2, e3, e4, e5, e6, e7, e8, e9, e10, e11, e12, e13, e14, (_0, _1, _2, _3, _4, _5, _6, _7, _8, _9, _10, _11, _12, _13, _14) => f())
            );
            
            res.Messages.AssertEqual(
                OnError<int>(350, ex)
            );

            var es = new[] { e0, e1, e2, e3, e4, e5, e6, e7, e8, e9, e10, e11, e12, e13, e14 };
            foreach (var e in es)
                e.Subscriptions.AssertEqual(Subscribe(200, 200 + 10 * es.Length));
        }

        [TestMethod]
        public void CombineLatest_SelectorThrows16()
        {
            var scheduler = new TestScheduler();
            
            var e0 = scheduler.CreateHotObservable(new[] { OnNext(150, 1), OnNext(210, 1), OnCompleted<int>(400) });
            var e1 = scheduler.CreateHotObservable(new[] { OnNext(150, 1), OnNext(220, 2), OnCompleted<int>(400) });
            var e2 = scheduler.CreateHotObservable(new[] { OnNext(150, 1), OnNext(230, 3), OnCompleted<int>(400) });
            var e3 = scheduler.CreateHotObservable(new[] { OnNext(150, 1), OnNext(240, 4), OnCompleted<int>(400) });
            var e4 = scheduler.CreateHotObservable(new[] { OnNext(150, 1), OnNext(250, 5), OnCompleted<int>(400) });
            var e5 = scheduler.CreateHotObservable(new[] { OnNext(150, 1), OnNext(260, 6), OnCompleted<int>(400) });
            var e6 = scheduler.CreateHotObservable(new[] { OnNext(150, 1), OnNext(270, 7), OnCompleted<int>(400) });
            var e7 = scheduler.CreateHotObservable(new[] { OnNext(150, 1), OnNext(280, 8), OnCompleted<int>(400) });
            var e8 = scheduler.CreateHotObservable(new[] { OnNext(150, 1), OnNext(290, 9), OnCompleted<int>(400) });
            var e9 = scheduler.CreateHotObservable(new[] { OnNext(150, 1), OnNext(300, 10), OnCompleted<int>(400) });
            var e10 = scheduler.CreateHotObservable(new[] { OnNext(150, 1), OnNext(310, 11), OnCompleted<int>(400) });
            var e11 = scheduler.CreateHotObservable(new[] { OnNext(150, 1), OnNext(320, 12), OnCompleted<int>(400) });
            var e12 = scheduler.CreateHotObservable(new[] { OnNext(150, 1), OnNext(330, 13), OnCompleted<int>(400) });
            var e13 = scheduler.CreateHotObservable(new[] { OnNext(150, 1), OnNext(340, 14), OnCompleted<int>(400) });
            var e14 = scheduler.CreateHotObservable(new[] { OnNext(150, 1), OnNext(350, 15), OnCompleted<int>(400) });
            var e15 = scheduler.CreateHotObservable(new[] { OnNext(150, 1), OnNext(360, 16), OnCompleted<int>(400) });
            
            var ex = new Exception();
            Func<int> f = () => { throw ex; };
            
            var res = scheduler.Start(() =>
                Observable.CombineLatest(e0, e1, e2, e3, e4, e5, e6, e7, e8, e9, e10, e11, e12, e13, e14, e15, (_0, _1, _2, _3, _4, _5, _6, _7, _8, _9, _10, _11, _12, _13, _14, _15) => f())
            );
            
            res.Messages.AssertEqual(
                OnError<int>(360, ex)
            );

            var es = new[] { e0, e1, e2, e3, e4, e5, e6, e7, e8, e9, e10, e11, e12, e13, e14, e15 };
            foreach (var e in es)
                e.Subscriptions.AssertEqual(Subscribe(200, 200 + 10 * es.Length));
        }
#endif

        #endregion

        #region AllEmptyButOne

        [TestMethod]
        public void CombineLatest_WillNeverBeAbleToCombineN()
        {
            var scheduler = new TestScheduler();

            var e0 = scheduler.CreateHotObservable(new[] { OnNext(150, 1), OnCompleted<int>(250) });
            var e1 = scheduler.CreateHotObservable(new[] { OnNext(150, 1), OnCompleted<int>(260) });
            var e2 = scheduler.CreateHotObservable(new[] { OnNext(150, 1), OnNext(500, 2), OnCompleted<int>(800) });

            var res = scheduler.Start(() =>
                Observable.CombineLatest(new[] { e0, e1, e2 }, xs => 42)
            );
            
            res.Messages.AssertEqual(
                OnCompleted<int>(500)
            );

            var i = 0;
            var es = new[] { e0, e1, e2 };
            foreach (var e in es.Take(es.Length - 1))
                e.Subscriptions.AssertEqual(Subscribe(200, 200 + 50 + (i++ * 10)));

            es.Last().Subscriptions.AssertEqual(Subscribe(200, 500));
        }

        [TestMethod]
        public void CombineLatest_WillNeverBeAbleToCombine2()
        {
            var scheduler = new TestScheduler();

            var e0 = scheduler.CreateHotObservable(new[] { OnNext(150, 1), OnCompleted<int>(250) });
            var e1 = scheduler.CreateHotObservable(new[] { OnNext(150, 1), OnNext(500, 2), OnCompleted<int>(800) });

            var res = scheduler.Start(() =>
                Observable.CombineLatest(e0, e1, (_0, _1) => 42)
            );
            
            res.Messages.AssertEqual(
                OnCompleted<int>(500)
            );

            var i = 0;
            var es = new[] { e0, e1 };
            foreach (var e in es.Take(es.Length - 1))
                e.Subscriptions.AssertEqual(Subscribe(200, 200 + 50 + (i++ * 10)));

            es.Last().Subscriptions.AssertEqual(Subscribe(200, 500));
        }

        [TestMethod]
        public void CombineLatest_WillNeverBeAbleToCombine3()
        {
            var scheduler = new TestScheduler();

            var e0 = scheduler.CreateHotObservable(new[] { OnNext(150, 1), OnCompleted<int>(250) });
            var e1 = scheduler.CreateHotObservable(new[] { OnNext(150, 1), OnCompleted<int>(260) });
            var e2 = scheduler.CreateHotObservable(new[] { OnNext(150, 1), OnNext(500, 2), OnCompleted<int>(800) });

            var res = scheduler.Start(() =>
                Observable.CombineLatest(e0, e1, e2, (_0, _1, _2) => 42)
            );
            
            res.Messages.AssertEqual(
                OnCompleted<int>(500)
            );

            var i = 0;
            var es = new[] { e0, e1, e2 };
            foreach (var e in es.Take(es.Length - 1))
                e.Subscriptions.AssertEqual(Subscribe(200, 200 + 50 + (i++ * 10)));

            es.Last().Subscriptions.AssertEqual(Subscribe(200, 500));
        }

        [TestMethod]
        public void CombineLatest_WillNeverBeAbleToCombine4()
        {
            var scheduler = new TestScheduler();

            var e0 = scheduler.CreateHotObservable(new[] { OnNext(150, 1), OnCompleted<int>(250) });
            var e1 = scheduler.CreateHotObservable(new[] { OnNext(150, 1), OnCompleted<int>(260) });
            var e2 = scheduler.CreateHotObservable(new[] { OnNext(150, 1), OnCompleted<int>(270) });
            var e3 = scheduler.CreateHotObservable(new[] { OnNext(150, 1), OnNext(500, 2), OnCompleted<int>(800) });

            var res = scheduler.Start(() =>
                Observable.CombineLatest(e0, e1, e2, e3, (_0, _1, _2, _3) => 42)
            );
            
            res.Messages.AssertEqual(
                OnCompleted<int>(500)
            );

            var i = 0;
            var es = new[] { e0, e1, e2, e3 };
            foreach (var e in es.Take(es.Length - 1))
                e.Subscriptions.AssertEqual(Subscribe(200, 200 + 50 + (i++ * 10)));

            es.Last().Subscriptions.AssertEqual(Subscribe(200, 500));
        }

#if !NO_LARGEARITY
        [TestMethod]
        public void CombineLatest_WillNeverBeAbleToCombine5()
        {
            var scheduler = new TestScheduler();

            var e0 = scheduler.CreateHotObservable(new[] { OnNext(150, 1), OnCompleted<int>(250) });
            var e1 = scheduler.CreateHotObservable(new[] { OnNext(150, 1), OnCompleted<int>(260) });
            var e2 = scheduler.CreateHotObservable(new[] { OnNext(150, 1), OnCompleted<int>(270) });
            var e3 = scheduler.CreateHotObservable(new[] { OnNext(150, 1), OnCompleted<int>(280) });
            var e4 = scheduler.CreateHotObservable(new[] { OnNext(150, 1), OnNext(500, 2), OnCompleted<int>(800) });

            var res = scheduler.Start(() =>
                Observable.CombineLatest(e0, e1, e2, e3, e4, (_0, _1, _2, _3, _4) => 42)
            );
            
            res.Messages.AssertEqual(
                OnCompleted<int>(500)
            );

            var i = 0;
            var es = new[] { e0, e1, e2, e3, e4 };
            foreach (var e in es.Take(es.Length - 1))
                e.Subscriptions.AssertEqual(Subscribe(200, 200 + 50 + (i++ * 10)));

            es.Last().Subscriptions.AssertEqual(Subscribe(200, 500));
        }

        [TestMethod]
        public void CombineLatest_WillNeverBeAbleToCombine6()
        {
            var scheduler = new TestScheduler();

            var e0 = scheduler.CreateHotObservable(new[] { OnNext(150, 1), OnCompleted<int>(250) });
            var e1 = scheduler.CreateHotObservable(new[] { OnNext(150, 1), OnCompleted<int>(260) });
            var e2 = scheduler.CreateHotObservable(new[] { OnNext(150, 1), OnCompleted<int>(270) });
            var e3 = scheduler.CreateHotObservable(new[] { OnNext(150, 1), OnCompleted<int>(280) });
            var e4 = scheduler.CreateHotObservable(new[] { OnNext(150, 1), OnCompleted<int>(290) });
            var e5 = scheduler.CreateHotObservable(new[] { OnNext(150, 1), OnNext(500, 2), OnCompleted<int>(800) });

            var res = scheduler.Start(() =>
                Observable.CombineLatest(e0, e1, e2, e3, e4, e5, (_0, _1, _2, _3, _4, _5) => 42)
            );
            
            res.Messages.AssertEqual(
                OnCompleted<int>(500)
            );

            var i = 0;
            var es = new[] { e0, e1, e2, e3, e4, e5 };
            foreach (var e in es.Take(es.Length - 1))
                e.Subscriptions.AssertEqual(Subscribe(200, 200 + 50 + (i++ * 10)));

            es.Last().Subscriptions.AssertEqual(Subscribe(200, 500));
        }

        [TestMethod]
        public void CombineLatest_WillNeverBeAbleToCombine7()
        {
            var scheduler = new TestScheduler();

            var e0 = scheduler.CreateHotObservable(new[] { OnNext(150, 1), OnCompleted<int>(250) });
            var e1 = scheduler.CreateHotObservable(new[] { OnNext(150, 1), OnCompleted<int>(260) });
            var e2 = scheduler.CreateHotObservable(new[] { OnNext(150, 1), OnCompleted<int>(270) });
            var e3 = scheduler.CreateHotObservable(new[] { OnNext(150, 1), OnCompleted<int>(280) });
            var e4 = scheduler.CreateHotObservable(new[] { OnNext(150, 1), OnCompleted<int>(290) });
            var e5 = scheduler.CreateHotObservable(new[] { OnNext(150, 1), OnCompleted<int>(300) });
            var e6 = scheduler.CreateHotObservable(new[] { OnNext(150, 1), OnNext(500, 2), OnCompleted<int>(800) });

            var res = scheduler.Start(() =>
                Observable.CombineLatest(e0, e1, e2, e3, e4, e5, e6, (_0, _1, _2, _3, _4, _5, _6) => 42)
            );
            
            res.Messages.AssertEqual(
                OnCompleted<int>(500)
            );

            var i = 0;
            var es = new[] { e0, e1, e2, e3, e4, e5, e6 };
            foreach (var e in es.Take(es.Length - 1))
                e.Subscriptions.AssertEqual(Subscribe(200, 200 + 50 + (i++ * 10)));

            es.Last().Subscriptions.AssertEqual(Subscribe(200, 500));
        }

        [TestMethod]
        public void CombineLatest_WillNeverBeAbleToCombine8()
        {
            var scheduler = new TestScheduler();

            var e0 = scheduler.CreateHotObservable(new[] { OnNext(150, 1), OnCompleted<int>(250) });
            var e1 = scheduler.CreateHotObservable(new[] { OnNext(150, 1), OnCompleted<int>(260) });
            var e2 = scheduler.CreateHotObservable(new[] { OnNext(150, 1), OnCompleted<int>(270) });
            var e3 = scheduler.CreateHotObservable(new[] { OnNext(150, 1), OnCompleted<int>(280) });
            var e4 = scheduler.CreateHotObservable(new[] { OnNext(150, 1), OnCompleted<int>(290) });
            var e5 = scheduler.CreateHotObservable(new[] { OnNext(150, 1), OnCompleted<int>(300) });
            var e6 = scheduler.CreateHotObservable(new[] { OnNext(150, 1), OnCompleted<int>(310) });
            var e7 = scheduler.CreateHotObservable(new[] { OnNext(150, 1), OnNext(500, 2), OnCompleted<int>(800) });

            var res = scheduler.Start(() =>
                Observable.CombineLatest(e0, e1, e2, e3, e4, e5, e6, e7, (_0, _1, _2, _3, _4, _5, _6, _7) => 42)
            );
            
            res.Messages.AssertEqual(
                OnCompleted<int>(500)
            );

            var i = 0;
            var es = new[] { e0, e1, e2, e3, e4, e5, e6, e7 };
            foreach (var e in es.Take(es.Length - 1))
                e.Subscriptions.AssertEqual(Subscribe(200, 200 + 50 + (i++ * 10)));

            es.Last().Subscriptions.AssertEqual(Subscribe(200, 500));
        }

        [TestMethod]
        public void CombineLatest_WillNeverBeAbleToCombine9()
        {
            var scheduler = new TestScheduler();

            var e0 = scheduler.CreateHotObservable(new[] { OnNext(150, 1), OnCompleted<int>(250) });
            var e1 = scheduler.CreateHotObservable(new[] { OnNext(150, 1), OnCompleted<int>(260) });
            var e2 = scheduler.CreateHotObservable(new[] { OnNext(150, 1), OnCompleted<int>(270) });
            var e3 = scheduler.CreateHotObservable(new[] { OnNext(150, 1), OnCompleted<int>(280) });
            var e4 = scheduler.CreateHotObservable(new[] { OnNext(150, 1), OnCompleted<int>(290) });
            var e5 = scheduler.CreateHotObservable(new[] { OnNext(150, 1), OnCompleted<int>(300) });
            var e6 = scheduler.CreateHotObservable(new[] { OnNext(150, 1), OnCompleted<int>(310) });
            var e7 = scheduler.CreateHotObservable(new[] { OnNext(150, 1), OnCompleted<int>(320) });
            var e8 = scheduler.CreateHotObservable(new[] { OnNext(150, 1), OnNext(500, 2), OnCompleted<int>(800) });

            var res = scheduler.Start(() =>
                Observable.CombineLatest(e0, e1, e2, e3, e4, e5, e6, e7, e8, (_0, _1, _2, _3, _4, _5, _6, _7, _8) => 42)
            );
            
            res.Messages.AssertEqual(
                OnCompleted<int>(500)
            );

            var i = 0;
            var es = new[] { e0, e1, e2, e3, e4, e5, e6, e7, e8 };
            foreach (var e in es.Take(es.Length - 1))
                e.Subscriptions.AssertEqual(Subscribe(200, 200 + 50 + (i++ * 10)));

            es.Last().Subscriptions.AssertEqual(Subscribe(200, 500));
        }

        [TestMethod]
        public void CombineLatest_WillNeverBeAbleToCombine10()
        {
            var scheduler = new TestScheduler();

            var e0 = scheduler.CreateHotObservable(new[] { OnNext(150, 1), OnCompleted<int>(250) });
            var e1 = scheduler.CreateHotObservable(new[] { OnNext(150, 1), OnCompleted<int>(260) });
            var e2 = scheduler.CreateHotObservable(new[] { OnNext(150, 1), OnCompleted<int>(270) });
            var e3 = scheduler.CreateHotObservable(new[] { OnNext(150, 1), OnCompleted<int>(280) });
            var e4 = scheduler.CreateHotObservable(new[] { OnNext(150, 1), OnCompleted<int>(290) });
            var e5 = scheduler.CreateHotObservable(new[] { OnNext(150, 1), OnCompleted<int>(300) });
            var e6 = scheduler.CreateHotObservable(new[] { OnNext(150, 1), OnCompleted<int>(310) });
            var e7 = scheduler.CreateHotObservable(new[] { OnNext(150, 1), OnCompleted<int>(320) });
            var e8 = scheduler.CreateHotObservable(new[] { OnNext(150, 1), OnCompleted<int>(330) });
            var e9 = scheduler.CreateHotObservable(new[] { OnNext(150, 1), OnNext(500, 2), OnCompleted<int>(800) });

            var res = scheduler.Start(() =>
                Observable.CombineLatest(e0, e1, e2, e3, e4, e5, e6, e7, e8, e9, (_0, _1, _2, _3, _4, _5, _6, _7, _8, _9) => 42)
            );
            
            res.Messages.AssertEqual(
                OnCompleted<int>(500)
            );

            var i = 0;
            var es = new[] { e0, e1, e2, e3, e4, e5, e6, e7, e8, e9 };
            foreach (var e in es.Take(es.Length - 1))
                e.Subscriptions.AssertEqual(Subscribe(200, 200 + 50 + (i++ * 10)));

            es.Last().Subscriptions.AssertEqual(Subscribe(200, 500));
        }

        [TestMethod]
        public void CombineLatest_WillNeverBeAbleToCombine11()
        {
            var scheduler = new TestScheduler();

            var e0 = scheduler.CreateHotObservable(new[] { OnNext(150, 1), OnCompleted<int>(250) });
            var e1 = scheduler.CreateHotObservable(new[] { OnNext(150, 1), OnCompleted<int>(260) });
            var e2 = scheduler.CreateHotObservable(new[] { OnNext(150, 1), OnCompleted<int>(270) });
            var e3 = scheduler.CreateHotObservable(new[] { OnNext(150, 1), OnCompleted<int>(280) });
            var e4 = scheduler.CreateHotObservable(new[] { OnNext(150, 1), OnCompleted<int>(290) });
            var e5 = scheduler.CreateHotObservable(new[] { OnNext(150, 1), OnCompleted<int>(300) });
            var e6 = scheduler.CreateHotObservable(new[] { OnNext(150, 1), OnCompleted<int>(310) });
            var e7 = scheduler.CreateHotObservable(new[] { OnNext(150, 1), OnCompleted<int>(320) });
            var e8 = scheduler.CreateHotObservable(new[] { OnNext(150, 1), OnCompleted<int>(330) });
            var e9 = scheduler.CreateHotObservable(new[] { OnNext(150, 1), OnCompleted<int>(340) });
            var e10 = scheduler.CreateHotObservable(new[] { OnNext(150, 1), OnNext(500, 2), OnCompleted<int>(800) });

            var res = scheduler.Start(() =>
                Observable.CombineLatest(e0, e1, e2, e3, e4, e5, e6, e7, e8, e9, e10, (_0, _1, _2, _3, _4, _5, _6, _7, _8, _9, _10) => 42)
            );
            
            res.Messages.AssertEqual(
                OnCompleted<int>(500)
            );

            var i = 0;
            var es = new[] { e0, e1, e2, e3, e4, e5, e6, e7, e8, e9, e10 };
            foreach (var e in es.Take(es.Length - 1))
                e.Subscriptions.AssertEqual(Subscribe(200, 200 + 50 + (i++ * 10)));

            es.Last().Subscriptions.AssertEqual(Subscribe(200, 500));
        }

        [TestMethod]
        public void CombineLatest_WillNeverBeAbleToCombine12()
        {
            var scheduler = new TestScheduler();

            var e0 = scheduler.CreateHotObservable(new[] { OnNext(150, 1), OnCompleted<int>(250) });
            var e1 = scheduler.CreateHotObservable(new[] { OnNext(150, 1), OnCompleted<int>(260) });
            var e2 = scheduler.CreateHotObservable(new[] { OnNext(150, 1), OnCompleted<int>(270) });
            var e3 = scheduler.CreateHotObservable(new[] { OnNext(150, 1), OnCompleted<int>(280) });
            var e4 = scheduler.CreateHotObservable(new[] { OnNext(150, 1), OnCompleted<int>(290) });
            var e5 = scheduler.CreateHotObservable(new[] { OnNext(150, 1), OnCompleted<int>(300) });
            var e6 = scheduler.CreateHotObservable(new[] { OnNext(150, 1), OnCompleted<int>(310) });
            var e7 = scheduler.CreateHotObservable(new[] { OnNext(150, 1), OnCompleted<int>(320) });
            var e8 = scheduler.CreateHotObservable(new[] { OnNext(150, 1), OnCompleted<int>(330) });
            var e9 = scheduler.CreateHotObservable(new[] { OnNext(150, 1), OnCompleted<int>(340) });
            var e10 = scheduler.CreateHotObservable(new[] { OnNext(150, 1), OnCompleted<int>(350) });
            var e11 = scheduler.CreateHotObservable(new[] { OnNext(150, 1), OnNext(500, 2), OnCompleted<int>(800) });

            var res = scheduler.Start(() =>
                Observable.CombineLatest(e0, e1, e2, e3, e4, e5, e6, e7, e8, e9, e10, e11, (_0, _1, _2, _3, _4, _5, _6, _7, _8, _9, _10, _11) => 42)
            );
            
            res.Messages.AssertEqual(
                OnCompleted<int>(500)
            );

            var i = 0;
            var es = new[] { e0, e1, e2, e3, e4, e5, e6, e7, e8, e9, e10, e11 };
            foreach (var e in es.Take(es.Length - 1))
                e.Subscriptions.AssertEqual(Subscribe(200, 200 + 50 + (i++ * 10)));

            es.Last().Subscriptions.AssertEqual(Subscribe(200, 500));
        }

        [TestMethod]
        public void CombineLatest_WillNeverBeAbleToCombine13()
        {
            var scheduler = new TestScheduler();

            var e0 = scheduler.CreateHotObservable(new[] { OnNext(150, 1), OnCompleted<int>(250) });
            var e1 = scheduler.CreateHotObservable(new[] { OnNext(150, 1), OnCompleted<int>(260) });
            var e2 = scheduler.CreateHotObservable(new[] { OnNext(150, 1), OnCompleted<int>(270) });
            var e3 = scheduler.CreateHotObservable(new[] { OnNext(150, 1), OnCompleted<int>(280) });
            var e4 = scheduler.CreateHotObservable(new[] { OnNext(150, 1), OnCompleted<int>(290) });
            var e5 = scheduler.CreateHotObservable(new[] { OnNext(150, 1), OnCompleted<int>(300) });
            var e6 = scheduler.CreateHotObservable(new[] { OnNext(150, 1), OnCompleted<int>(310) });
            var e7 = scheduler.CreateHotObservable(new[] { OnNext(150, 1), OnCompleted<int>(320) });
            var e8 = scheduler.CreateHotObservable(new[] { OnNext(150, 1), OnCompleted<int>(330) });
            var e9 = scheduler.CreateHotObservable(new[] { OnNext(150, 1), OnCompleted<int>(340) });
            var e10 = scheduler.CreateHotObservable(new[] { OnNext(150, 1), OnCompleted<int>(350) });
            var e11 = scheduler.CreateHotObservable(new[] { OnNext(150, 1), OnCompleted<int>(360) });
            var e12 = scheduler.CreateHotObservable(new[] { OnNext(150, 1), OnNext(500, 2), OnCompleted<int>(800) });

            var res = scheduler.Start(() =>
                Observable.CombineLatest(e0, e1, e2, e3, e4, e5, e6, e7, e8, e9, e10, e11, e12, (_0, _1, _2, _3, _4, _5, _6, _7, _8, _9, _10, _11, _12) => 42)
            );
            
            res.Messages.AssertEqual(
                OnCompleted<int>(500)
            );

            var i = 0;
            var es = new[] { e0, e1, e2, e3, e4, e5, e6, e7, e8, e9, e10, e11, e12 };
            foreach (var e in es.Take(es.Length - 1))
                e.Subscriptions.AssertEqual(Subscribe(200, 200 + 50 + (i++ * 10)));

            es.Last().Subscriptions.AssertEqual(Subscribe(200, 500));
        }

        [TestMethod]
        public void CombineLatest_WillNeverBeAbleToCombine14()
        {
            var scheduler = new TestScheduler();

            var e0 = scheduler.CreateHotObservable(new[] { OnNext(150, 1), OnCompleted<int>(250) });
            var e1 = scheduler.CreateHotObservable(new[] { OnNext(150, 1), OnCompleted<int>(260) });
            var e2 = scheduler.CreateHotObservable(new[] { OnNext(150, 1), OnCompleted<int>(270) });
            var e3 = scheduler.CreateHotObservable(new[] { OnNext(150, 1), OnCompleted<int>(280) });
            var e4 = scheduler.CreateHotObservable(new[] { OnNext(150, 1), OnCompleted<int>(290) });
            var e5 = scheduler.CreateHotObservable(new[] { OnNext(150, 1), OnCompleted<int>(300) });
            var e6 = scheduler.CreateHotObservable(new[] { OnNext(150, 1), OnCompleted<int>(310) });
            var e7 = scheduler.CreateHotObservable(new[] { OnNext(150, 1), OnCompleted<int>(320) });
            var e8 = scheduler.CreateHotObservable(new[] { OnNext(150, 1), OnCompleted<int>(330) });
            var e9 = scheduler.CreateHotObservable(new[] { OnNext(150, 1), OnCompleted<int>(340) });
            var e10 = scheduler.CreateHotObservable(new[] { OnNext(150, 1), OnCompleted<int>(350) });
            var e11 = scheduler.CreateHotObservable(new[] { OnNext(150, 1), OnCompleted<int>(360) });
            var e12 = scheduler.CreateHotObservable(new[] { OnNext(150, 1), OnCompleted<int>(370) });
            var e13 = scheduler.CreateHotObservable(new[] { OnNext(150, 1), OnNext(500, 2), OnCompleted<int>(800) });

            var res = scheduler.Start(() =>
                Observable.CombineLatest(e0, e1, e2, e3, e4, e5, e6, e7, e8, e9, e10, e11, e12, e13, (_0, _1, _2, _3, _4, _5, _6, _7, _8, _9, _10, _11, _12, _13) => 42)
            );
            
            res.Messages.AssertEqual(
                OnCompleted<int>(500)
            );

            var i = 0;
            var es = new[] { e0, e1, e2, e3, e4, e5, e6, e7, e8, e9, e10, e11, e12, e13 };
            foreach (var e in es.Take(es.Length - 1))
                e.Subscriptions.AssertEqual(Subscribe(200, 200 + 50 + (i++ * 10)));

            es.Last().Subscriptions.AssertEqual(Subscribe(200, 500));
        }

        [TestMethod]
        public void CombineLatest_WillNeverBeAbleToCombine15()
        {
            var scheduler = new TestScheduler();

            var e0 = scheduler.CreateHotObservable(new[] { OnNext(150, 1), OnCompleted<int>(250) });
            var e1 = scheduler.CreateHotObservable(new[] { OnNext(150, 1), OnCompleted<int>(260) });
            var e2 = scheduler.CreateHotObservable(new[] { OnNext(150, 1), OnCompleted<int>(270) });
            var e3 = scheduler.CreateHotObservable(new[] { OnNext(150, 1), OnCompleted<int>(280) });
            var e4 = scheduler.CreateHotObservable(new[] { OnNext(150, 1), OnCompleted<int>(290) });
            var e5 = scheduler.CreateHotObservable(new[] { OnNext(150, 1), OnCompleted<int>(300) });
            var e6 = scheduler.CreateHotObservable(new[] { OnNext(150, 1), OnCompleted<int>(310) });
            var e7 = scheduler.CreateHotObservable(new[] { OnNext(150, 1), OnCompleted<int>(320) });
            var e8 = scheduler.CreateHotObservable(new[] { OnNext(150, 1), OnCompleted<int>(330) });
            var e9 = scheduler.CreateHotObservable(new[] { OnNext(150, 1), OnCompleted<int>(340) });
            var e10 = scheduler.CreateHotObservable(new[] { OnNext(150, 1), OnCompleted<int>(350) });
            var e11 = scheduler.CreateHotObservable(new[] { OnNext(150, 1), OnCompleted<int>(360) });
            var e12 = scheduler.CreateHotObservable(new[] { OnNext(150, 1), OnCompleted<int>(370) });
            var e13 = scheduler.CreateHotObservable(new[] { OnNext(150, 1), OnCompleted<int>(380) });
            var e14 = scheduler.CreateHotObservable(new[] { OnNext(150, 1), OnNext(500, 2), OnCompleted<int>(800) });

            var res = scheduler.Start(() =>
                Observable.CombineLatest(e0, e1, e2, e3, e4, e5, e6, e7, e8, e9, e10, e11, e12, e13, e14, (_0, _1, _2, _3, _4, _5, _6, _7, _8, _9, _10, _11, _12, _13, _14) => 42)
            );
            
            res.Messages.AssertEqual(
                OnCompleted<int>(500)
            );

            var i = 0;
            var es = new[] { e0, e1, e2, e3, e4, e5, e6, e7, e8, e9, e10, e11, e12, e13, e14 };
            foreach (var e in es.Take(es.Length - 1))
                e.Subscriptions.AssertEqual(Subscribe(200, 200 + 50 + (i++ * 10)));

            es.Last().Subscriptions.AssertEqual(Subscribe(200, 500));
        }

        [TestMethod]
        public void CombineLatest_WillNeverBeAbleToCombine16()
        {
            var scheduler = new TestScheduler();

            var e0 = scheduler.CreateHotObservable(new[] { OnNext(150, 1), OnCompleted<int>(250) });
            var e1 = scheduler.CreateHotObservable(new[] { OnNext(150, 1), OnCompleted<int>(260) });
            var e2 = scheduler.CreateHotObservable(new[] { OnNext(150, 1), OnCompleted<int>(270) });
            var e3 = scheduler.CreateHotObservable(new[] { OnNext(150, 1), OnCompleted<int>(280) });
            var e4 = scheduler.CreateHotObservable(new[] { OnNext(150, 1), OnCompleted<int>(290) });
            var e5 = scheduler.CreateHotObservable(new[] { OnNext(150, 1), OnCompleted<int>(300) });
            var e6 = scheduler.CreateHotObservable(new[] { OnNext(150, 1), OnCompleted<int>(310) });
            var e7 = scheduler.CreateHotObservable(new[] { OnNext(150, 1), OnCompleted<int>(320) });
            var e8 = scheduler.CreateHotObservable(new[] { OnNext(150, 1), OnCompleted<int>(330) });
            var e9 = scheduler.CreateHotObservable(new[] { OnNext(150, 1), OnCompleted<int>(340) });
            var e10 = scheduler.CreateHotObservable(new[] { OnNext(150, 1), OnCompleted<int>(350) });
            var e11 = scheduler.CreateHotObservable(new[] { OnNext(150, 1), OnCompleted<int>(360) });
            var e12 = scheduler.CreateHotObservable(new[] { OnNext(150, 1), OnCompleted<int>(370) });
            var e13 = scheduler.CreateHotObservable(new[] { OnNext(150, 1), OnCompleted<int>(380) });
            var e14 = scheduler.CreateHotObservable(new[] { OnNext(150, 1), OnCompleted<int>(390) });
            var e15 = scheduler.CreateHotObservable(new[] { OnNext(150, 1), OnNext(500, 2), OnCompleted<int>(800) });

            var res = scheduler.Start(() =>
                Observable.CombineLatest(e0, e1, e2, e3, e4, e5, e6, e7, e8, e9, e10, e11, e12, e13, e14, e15, (_0, _1, _2, _3, _4, _5, _6, _7, _8, _9, _10, _11, _12, _13, _14, _15) => 42)
            );
            
            res.Messages.AssertEqual(
                OnCompleted<int>(500)
            );

            var i = 0;
            var es = new[] { e0, e1, e2, e3, e4, e5, e6, e7, e8, e9, e10, e11, e12, e13, e14, e15 };
            foreach (var e in es.Take(es.Length - 1))
                e.Subscriptions.AssertEqual(Subscribe(200, 200 + 50 + (i++ * 10)));

            es.Last().Subscriptions.AssertEqual(Subscribe(200, 500));
        }
#endif

        #endregion

        #region Typical

        [TestMethod]
        public void CombineLatest_TypicalN()
        {
            var scheduler = new TestScheduler();

            var e0 = scheduler.CreateHotObservable(new[] { OnNext(150, 1), OnNext(210, 1), OnNext(410, 4), OnCompleted<int>(800) });
            var e1 = scheduler.CreateHotObservable(new[] { OnNext(150, 1), OnNext(220, 2), OnNext(420, 5), OnCompleted<int>(800) });
            var e2 = scheduler.CreateHotObservable(new[] { OnNext(150, 1), OnNext(230, 3), OnNext(430, 6), OnCompleted<int>(800) });

            var res = scheduler.Start(() =>
                Observable.CombineLatest(new[] { e0, e1, e2 }, xs => xs.Sum())
            );
            
            res.Messages.AssertEqual(
                OnNext(230, 6),
                OnNext(410, 9),
                OnNext(420, 12),
                OnNext(430, 15),
                OnCompleted<int>(800)
            );

            foreach (var e in new[] { e0, e1, e2 })
                e.Subscriptions.AssertEqual(Subscribe(200, 800));
        }

        [TestMethod]
        public void CombineLatest_Typical2()
        {
            var scheduler = new TestScheduler();

            var e0 = scheduler.CreateHotObservable(new[] { OnNext(150, 1), OnNext(210, 1), OnNext(410, 3), OnCompleted<int>(800) });
            var e1 = scheduler.CreateHotObservable(new[] { OnNext(150, 1), OnNext(220, 2), OnNext(420, 4), OnCompleted<int>(800) });

            var res = scheduler.Start(() =>
                Observable.CombineLatest(e0, e1, (_0, _1) => _0 + _1)
            );
            
            res.Messages.AssertEqual(
                OnNext(220, 3),
                OnNext(410, 5),
                OnNext(420, 7),
                OnCompleted<int>(800)
            );

            foreach (var e in new[] { e0, e1 })
                e.Subscriptions.AssertEqual(Subscribe(200, 800));
        }

        [TestMethod]
        public void CombineLatest_Typical3()
        {
            var scheduler = new TestScheduler();

            var e0 = scheduler.CreateHotObservable(new[] { OnNext(150, 1), OnNext(210, 1), OnNext(410, 4), OnCompleted<int>(800) });
            var e1 = scheduler.CreateHotObservable(new[] { OnNext(150, 1), OnNext(220, 2), OnNext(420, 5), OnCompleted<int>(800) });
            var e2 = scheduler.CreateHotObservable(new[] { OnNext(150, 1), OnNext(230, 3), OnNext(430, 6), OnCompleted<int>(800) });

            var res = scheduler.Start(() =>
                Observable.CombineLatest(e0, e1, e2, (_0, _1, _2) => _0 + _1 + _2)
            );
            
            res.Messages.AssertEqual(
                OnNext(230, 6),
                OnNext(410, 9),
                OnNext(420, 12),
                OnNext(430, 15),
                OnCompleted<int>(800)
            );

            foreach (var e in new[] { e0, e1, e2 })
                e.Subscriptions.AssertEqual(Subscribe(200, 800));
        }

        [TestMethod]
        public void CombineLatest_Typical4()
        {
            var scheduler = new TestScheduler();

            var e0 = scheduler.CreateHotObservable(new[] { OnNext(150, 1), OnNext(210, 1), OnNext(410, 5), OnCompleted<int>(800) });
            var e1 = scheduler.CreateHotObservable(new[] { OnNext(150, 1), OnNext(220, 2), OnNext(420, 6), OnCompleted<int>(800) });
            var e2 = scheduler.CreateHotObservable(new[] { OnNext(150, 1), OnNext(230, 3), OnNext(430, 7), OnCompleted<int>(800) });
            var e3 = scheduler.CreateHotObservable(new[] { OnNext(150, 1), OnNext(240, 4), OnNext(440, 8), OnCompleted<int>(800) });

            var res = scheduler.Start(() =>
                Observable.CombineLatest(e0, e1, e2, e3, (_0, _1, _2, _3) => _0 + _1 + _2 + _3)
            );
            
            res.Messages.AssertEqual(
                OnNext(240, 10),
                OnNext(410, 14),
                OnNext(420, 18),
                OnNext(430, 22),
                OnNext(440, 26),
                OnCompleted<int>(800)
            );

            foreach (var e in new[] { e0, e1, e2, e3 })
                e.Subscriptions.AssertEqual(Subscribe(200, 800));
        }

#if !NO_LARGEARITY
        [TestMethod]
        public void CombineLatest_Typical5()
        {
            var scheduler = new TestScheduler();

            var e0 = scheduler.CreateHotObservable(new[] { OnNext(150, 1), OnNext(210, 1), OnNext(410, 6), OnCompleted<int>(800) });
            var e1 = scheduler.CreateHotObservable(new[] { OnNext(150, 1), OnNext(220, 2), OnNext(420, 7), OnCompleted<int>(800) });
            var e2 = scheduler.CreateHotObservable(new[] { OnNext(150, 1), OnNext(230, 3), OnNext(430, 8), OnCompleted<int>(800) });
            var e3 = scheduler.CreateHotObservable(new[] { OnNext(150, 1), OnNext(240, 4), OnNext(440, 9), OnCompleted<int>(800) });
            var e4 = scheduler.CreateHotObservable(new[] { OnNext(150, 1), OnNext(250, 5), OnNext(450, 10), OnCompleted<int>(800) });

            var res = scheduler.Start(() =>
                Observable.CombineLatest(e0, e1, e2, e3, e4, (_0, _1, _2, _3, _4) => _0 + _1 + _2 + _3 + _4)
            );
            
            res.Messages.AssertEqual(
                OnNext(250, 15),
                OnNext(410, 20),
                OnNext(420, 25),
                OnNext(430, 30),
                OnNext(440, 35),
                OnNext(450, 40),
                OnCompleted<int>(800)
            );

            foreach (var e in new[] { e0, e1, e2, e3, e4 })
                e.Subscriptions.AssertEqual(Subscribe(200, 800));
        }

        [TestMethod]
        public void CombineLatest_Typical6()
        {
            var scheduler = new TestScheduler();

            var e0 = scheduler.CreateHotObservable(new[] { OnNext(150, 1), OnNext(210, 1), OnNext(410, 7), OnCompleted<int>(800) });
            var e1 = scheduler.CreateHotObservable(new[] { OnNext(150, 1), OnNext(220, 2), OnNext(420, 8), OnCompleted<int>(800) });
            var e2 = scheduler.CreateHotObservable(new[] { OnNext(150, 1), OnNext(230, 3), OnNext(430, 9), OnCompleted<int>(800) });
            var e3 = scheduler.CreateHotObservable(new[] { OnNext(150, 1), OnNext(240, 4), OnNext(440, 10), OnCompleted<int>(800) });
            var e4 = scheduler.CreateHotObservable(new[] { OnNext(150, 1), OnNext(250, 5), OnNext(450, 11), OnCompleted<int>(800) });
            var e5 = scheduler.CreateHotObservable(new[] { OnNext(150, 1), OnNext(260, 6), OnNext(460, 12), OnCompleted<int>(800) });

            var res = scheduler.Start(() =>
                Observable.CombineLatest(e0, e1, e2, e3, e4, e5, (_0, _1, _2, _3, _4, _5) => _0 + _1 + _2 + _3 + _4 + _5)
            );
            
            res.Messages.AssertEqual(
                OnNext(260, 21),
                OnNext(410, 27),
                OnNext(420, 33),
                OnNext(430, 39),
                OnNext(440, 45),
                OnNext(450, 51),
                OnNext(460, 57),
                OnCompleted<int>(800)
            );

            foreach (var e in new[] { e0, e1, e2, e3, e4, e5 })
                e.Subscriptions.AssertEqual(Subscribe(200, 800));
        }

        [TestMethod]
        public void CombineLatest_Typical7()
        {
            var scheduler = new TestScheduler();

            var e0 = scheduler.CreateHotObservable(new[] { OnNext(150, 1), OnNext(210, 1), OnNext(410, 8), OnCompleted<int>(800) });
            var e1 = scheduler.CreateHotObservable(new[] { OnNext(150, 1), OnNext(220, 2), OnNext(420, 9), OnCompleted<int>(800) });
            var e2 = scheduler.CreateHotObservable(new[] { OnNext(150, 1), OnNext(230, 3), OnNext(430, 10), OnCompleted<int>(800) });
            var e3 = scheduler.CreateHotObservable(new[] { OnNext(150, 1), OnNext(240, 4), OnNext(440, 11), OnCompleted<int>(800) });
            var e4 = scheduler.CreateHotObservable(new[] { OnNext(150, 1), OnNext(250, 5), OnNext(450, 12), OnCompleted<int>(800) });
            var e5 = scheduler.CreateHotObservable(new[] { OnNext(150, 1), OnNext(260, 6), OnNext(460, 13), OnCompleted<int>(800) });
            var e6 = scheduler.CreateHotObservable(new[] { OnNext(150, 1), OnNext(270, 7), OnNext(470, 14), OnCompleted<int>(800) });

            var res = scheduler.Start(() =>
                Observable.CombineLatest(e0, e1, e2, e3, e4, e5, e6, (_0, _1, _2, _3, _4, _5, _6) => _0 + _1 + _2 + _3 + _4 + _5 + _6)
            );
            
            res.Messages.AssertEqual(
                OnNext(270, 28),
                OnNext(410, 35),
                OnNext(420, 42),
                OnNext(430, 49),
                OnNext(440, 56),
                OnNext(450, 63),
                OnNext(460, 70),
                OnNext(470, 77),
                OnCompleted<int>(800)
            );

            foreach (var e in new[] { e0, e1, e2, e3, e4, e5, e6 })
                e.Subscriptions.AssertEqual(Subscribe(200, 800));
        }

        [TestMethod]
        public void CombineLatest_Typical8()
        {
            var scheduler = new TestScheduler();

            var e0 = scheduler.CreateHotObservable(new[] { OnNext(150, 1), OnNext(210, 1), OnNext(410, 9), OnCompleted<int>(800) });
            var e1 = scheduler.CreateHotObservable(new[] { OnNext(150, 1), OnNext(220, 2), OnNext(420, 10), OnCompleted<int>(800) });
            var e2 = scheduler.CreateHotObservable(new[] { OnNext(150, 1), OnNext(230, 3), OnNext(430, 11), OnCompleted<int>(800) });
            var e3 = scheduler.CreateHotObservable(new[] { OnNext(150, 1), OnNext(240, 4), OnNext(440, 12), OnCompleted<int>(800) });
            var e4 = scheduler.CreateHotObservable(new[] { OnNext(150, 1), OnNext(250, 5), OnNext(450, 13), OnCompleted<int>(800) });
            var e5 = scheduler.CreateHotObservable(new[] { OnNext(150, 1), OnNext(260, 6), OnNext(460, 14), OnCompleted<int>(800) });
            var e6 = scheduler.CreateHotObservable(new[] { OnNext(150, 1), OnNext(270, 7), OnNext(470, 15), OnCompleted<int>(800) });
            var e7 = scheduler.CreateHotObservable(new[] { OnNext(150, 1), OnNext(280, 8), OnNext(480, 16), OnCompleted<int>(800) });

            var res = scheduler.Start(() =>
                Observable.CombineLatest(e0, e1, e2, e3, e4, e5, e6, e7, (_0, _1, _2, _3, _4, _5, _6, _7) => _0 + _1 + _2 + _3 + _4 + _5 + _6 + _7)
            );
            
            res.Messages.AssertEqual(
                OnNext(280, 36),
                OnNext(410, 44),
                OnNext(420, 52),
                OnNext(430, 60),
                OnNext(440, 68),
                OnNext(450, 76),
                OnNext(460, 84),
                OnNext(470, 92),
                OnNext(480, 100),
                OnCompleted<int>(800)
            );

            foreach (var e in new[] { e0, e1, e2, e3, e4, e5, e6, e7 })
                e.Subscriptions.AssertEqual(Subscribe(200, 800));
        }

        [TestMethod]
        public void CombineLatest_Typical9()
        {
            var scheduler = new TestScheduler();

            var e0 = scheduler.CreateHotObservable(new[] { OnNext(150, 1), OnNext(210, 1), OnNext(410, 10), OnCompleted<int>(800) });
            var e1 = scheduler.CreateHotObservable(new[] { OnNext(150, 1), OnNext(220, 2), OnNext(420, 11), OnCompleted<int>(800) });
            var e2 = scheduler.CreateHotObservable(new[] { OnNext(150, 1), OnNext(230, 3), OnNext(430, 12), OnCompleted<int>(800) });
            var e3 = scheduler.CreateHotObservable(new[] { OnNext(150, 1), OnNext(240, 4), OnNext(440, 13), OnCompleted<int>(800) });
            var e4 = scheduler.CreateHotObservable(new[] { OnNext(150, 1), OnNext(250, 5), OnNext(450, 14), OnCompleted<int>(800) });
            var e5 = scheduler.CreateHotObservable(new[] { OnNext(150, 1), OnNext(260, 6), OnNext(460, 15), OnCompleted<int>(800) });
            var e6 = scheduler.CreateHotObservable(new[] { OnNext(150, 1), OnNext(270, 7), OnNext(470, 16), OnCompleted<int>(800) });
            var e7 = scheduler.CreateHotObservable(new[] { OnNext(150, 1), OnNext(280, 8), OnNext(480, 17), OnCompleted<int>(800) });
            var e8 = scheduler.CreateHotObservable(new[] { OnNext(150, 1), OnNext(290, 9), OnNext(490, 18), OnCompleted<int>(800) });

            var res = scheduler.Start(() =>
                Observable.CombineLatest(e0, e1, e2, e3, e4, e5, e6, e7, e8, (_0, _1, _2, _3, _4, _5, _6, _7, _8) => _0 + _1 + _2 + _3 + _4 + _5 + _6 + _7 + _8)
            );
            
            res.Messages.AssertEqual(
                OnNext(290, 45),
                OnNext(410, 54),
                OnNext(420, 63),
                OnNext(430, 72),
                OnNext(440, 81),
                OnNext(450, 90),
                OnNext(460, 99),
                OnNext(470, 108),
                OnNext(480, 117),
                OnNext(490, 126),
                OnCompleted<int>(800)
            );

            foreach (var e in new[] { e0, e1, e2, e3, e4, e5, e6, e7, e8 })
                e.Subscriptions.AssertEqual(Subscribe(200, 800));
        }

        [TestMethod]
        public void CombineLatest_Typical10()
        {
            var scheduler = new TestScheduler();

            var e0 = scheduler.CreateHotObservable(new[] { OnNext(150, 1), OnNext(210, 1), OnNext(410, 11), OnCompleted<int>(800) });
            var e1 = scheduler.CreateHotObservable(new[] { OnNext(150, 1), OnNext(220, 2), OnNext(420, 12), OnCompleted<int>(800) });
            var e2 = scheduler.CreateHotObservable(new[] { OnNext(150, 1), OnNext(230, 3), OnNext(430, 13), OnCompleted<int>(800) });
            var e3 = scheduler.CreateHotObservable(new[] { OnNext(150, 1), OnNext(240, 4), OnNext(440, 14), OnCompleted<int>(800) });
            var e4 = scheduler.CreateHotObservable(new[] { OnNext(150, 1), OnNext(250, 5), OnNext(450, 15), OnCompleted<int>(800) });
            var e5 = scheduler.CreateHotObservable(new[] { OnNext(150, 1), OnNext(260, 6), OnNext(460, 16), OnCompleted<int>(800) });
            var e6 = scheduler.CreateHotObservable(new[] { OnNext(150, 1), OnNext(270, 7), OnNext(470, 17), OnCompleted<int>(800) });
            var e7 = scheduler.CreateHotObservable(new[] { OnNext(150, 1), OnNext(280, 8), OnNext(480, 18), OnCompleted<int>(800) });
            var e8 = scheduler.CreateHotObservable(new[] { OnNext(150, 1), OnNext(290, 9), OnNext(490, 19), OnCompleted<int>(800) });
            var e9 = scheduler.CreateHotObservable(new[] { OnNext(150, 1), OnNext(300, 10), OnNext(500, 20), OnCompleted<int>(800) });

            var res = scheduler.Start(() =>
                Observable.CombineLatest(e0, e1, e2, e3, e4, e5, e6, e7, e8, e9, (_0, _1, _2, _3, _4, _5, _6, _7, _8, _9) => _0 + _1 + _2 + _3 + _4 + _5 + _6 + _7 + _8 + _9)
            );
            
            res.Messages.AssertEqual(
                OnNext(300, 55),
                OnNext(410, 65),
                OnNext(420, 75),
                OnNext(430, 85),
                OnNext(440, 95),
                OnNext(450, 105),
                OnNext(460, 115),
                OnNext(470, 125),
                OnNext(480, 135),
                OnNext(490, 145),
                OnNext(500, 155),
                OnCompleted<int>(800)
            );

            foreach (var e in new[] { e0, e1, e2, e3, e4, e5, e6, e7, e8, e9 })
                e.Subscriptions.AssertEqual(Subscribe(200, 800));
        }

        [TestMethod]
        public void CombineLatest_Typical11()
        {
            var scheduler = new TestScheduler();

            var e0 = scheduler.CreateHotObservable(new[] { OnNext(150, 1), OnNext(210, 1), OnNext(410, 12), OnCompleted<int>(800) });
            var e1 = scheduler.CreateHotObservable(new[] { OnNext(150, 1), OnNext(220, 2), OnNext(420, 13), OnCompleted<int>(800) });
            var e2 = scheduler.CreateHotObservable(new[] { OnNext(150, 1), OnNext(230, 3), OnNext(430, 14), OnCompleted<int>(800) });
            var e3 = scheduler.CreateHotObservable(new[] { OnNext(150, 1), OnNext(240, 4), OnNext(440, 15), OnCompleted<int>(800) });
            var e4 = scheduler.CreateHotObservable(new[] { OnNext(150, 1), OnNext(250, 5), OnNext(450, 16), OnCompleted<int>(800) });
            var e5 = scheduler.CreateHotObservable(new[] { OnNext(150, 1), OnNext(260, 6), OnNext(460, 17), OnCompleted<int>(800) });
            var e6 = scheduler.CreateHotObservable(new[] { OnNext(150, 1), OnNext(270, 7), OnNext(470, 18), OnCompleted<int>(800) });
            var e7 = scheduler.CreateHotObservable(new[] { OnNext(150, 1), OnNext(280, 8), OnNext(480, 19), OnCompleted<int>(800) });
            var e8 = scheduler.CreateHotObservable(new[] { OnNext(150, 1), OnNext(290, 9), OnNext(490, 20), OnCompleted<int>(800) });
            var e9 = scheduler.CreateHotObservable(new[] { OnNext(150, 1), OnNext(300, 10), OnNext(500, 21), OnCompleted<int>(800) });
            var e10 = scheduler.CreateHotObservable(new[] { OnNext(150, 1), OnNext(310, 11), OnNext(510, 22), OnCompleted<int>(800) });

            var res = scheduler.Start(() =>
                Observable.CombineLatest(e0, e1, e2, e3, e4, e5, e6, e7, e8, e9, e10, (_0, _1, _2, _3, _4, _5, _6, _7, _8, _9, _10) => _0 + _1 + _2 + _3 + _4 + _5 + _6 + _7 + _8 + _9 + _10)
            );
            
            res.Messages.AssertEqual(
                OnNext(310, 66),
                OnNext(410, 77),
                OnNext(420, 88),
                OnNext(430, 99),
                OnNext(440, 110),
                OnNext(450, 121),
                OnNext(460, 132),
                OnNext(470, 143),
                OnNext(480, 154),
                OnNext(490, 165),
                OnNext(500, 176),
                OnNext(510, 187),
                OnCompleted<int>(800)
            );

            foreach (var e in new[] { e0, e1, e2, e3, e4, e5, e6, e7, e8, e9, e10 })
                e.Subscriptions.AssertEqual(Subscribe(200, 800));
        }

        [TestMethod]
        public void CombineLatest_Typical12()
        {
            var scheduler = new TestScheduler();

            var e0 = scheduler.CreateHotObservable(new[] { OnNext(150, 1), OnNext(210, 1), OnNext(410, 13), OnCompleted<int>(800) });
            var e1 = scheduler.CreateHotObservable(new[] { OnNext(150, 1), OnNext(220, 2), OnNext(420, 14), OnCompleted<int>(800) });
            var e2 = scheduler.CreateHotObservable(new[] { OnNext(150, 1), OnNext(230, 3), OnNext(430, 15), OnCompleted<int>(800) });
            var e3 = scheduler.CreateHotObservable(new[] { OnNext(150, 1), OnNext(240, 4), OnNext(440, 16), OnCompleted<int>(800) });
            var e4 = scheduler.CreateHotObservable(new[] { OnNext(150, 1), OnNext(250, 5), OnNext(450, 17), OnCompleted<int>(800) });
            var e5 = scheduler.CreateHotObservable(new[] { OnNext(150, 1), OnNext(260, 6), OnNext(460, 18), OnCompleted<int>(800) });
            var e6 = scheduler.CreateHotObservable(new[] { OnNext(150, 1), OnNext(270, 7), OnNext(470, 19), OnCompleted<int>(800) });
            var e7 = scheduler.CreateHotObservable(new[] { OnNext(150, 1), OnNext(280, 8), OnNext(480, 20), OnCompleted<int>(800) });
            var e8 = scheduler.CreateHotObservable(new[] { OnNext(150, 1), OnNext(290, 9), OnNext(490, 21), OnCompleted<int>(800) });
            var e9 = scheduler.CreateHotObservable(new[] { OnNext(150, 1), OnNext(300, 10), OnNext(500, 22), OnCompleted<int>(800) });
            var e10 = scheduler.CreateHotObservable(new[] { OnNext(150, 1), OnNext(310, 11), OnNext(510, 23), OnCompleted<int>(800) });
            var e11 = scheduler.CreateHotObservable(new[] { OnNext(150, 1), OnNext(320, 12), OnNext(520, 24), OnCompleted<int>(800) });

            var res = scheduler.Start(() =>
                Observable.CombineLatest(e0, e1, e2, e3, e4, e5, e6, e7, e8, e9, e10, e11, (_0, _1, _2, _3, _4, _5, _6, _7, _8, _9, _10, _11) => _0 + _1 + _2 + _3 + _4 + _5 + _6 + _7 + _8 + _9 + _10 + _11)
            );
            
            res.Messages.AssertEqual(
                OnNext(320, 78),
                OnNext(410, 90),
                OnNext(420, 102),
                OnNext(430, 114),
                OnNext(440, 126),
                OnNext(450, 138),
                OnNext(460, 150),
                OnNext(470, 162),
                OnNext(480, 174),
                OnNext(490, 186),
                OnNext(500, 198),
                OnNext(510, 210),
                OnNext(520, 222),
                OnCompleted<int>(800)
            );

            foreach (var e in new[] { e0, e1, e2, e3, e4, e5, e6, e7, e8, e9, e10, e11 })
                e.Subscriptions.AssertEqual(Subscribe(200, 800));
        }

        [TestMethod]
        public void CombineLatest_Typical13()
        {
            var scheduler = new TestScheduler();

            var e0 = scheduler.CreateHotObservable(new[] { OnNext(150, 1), OnNext(210, 1), OnNext(410, 14), OnCompleted<int>(800) });
            var e1 = scheduler.CreateHotObservable(new[] { OnNext(150, 1), OnNext(220, 2), OnNext(420, 15), OnCompleted<int>(800) });
            var e2 = scheduler.CreateHotObservable(new[] { OnNext(150, 1), OnNext(230, 3), OnNext(430, 16), OnCompleted<int>(800) });
            var e3 = scheduler.CreateHotObservable(new[] { OnNext(150, 1), OnNext(240, 4), OnNext(440, 17), OnCompleted<int>(800) });
            var e4 = scheduler.CreateHotObservable(new[] { OnNext(150, 1), OnNext(250, 5), OnNext(450, 18), OnCompleted<int>(800) });
            var e5 = scheduler.CreateHotObservable(new[] { OnNext(150, 1), OnNext(260, 6), OnNext(460, 19), OnCompleted<int>(800) });
            var e6 = scheduler.CreateHotObservable(new[] { OnNext(150, 1), OnNext(270, 7), OnNext(470, 20), OnCompleted<int>(800) });
            var e7 = scheduler.CreateHotObservable(new[] { OnNext(150, 1), OnNext(280, 8), OnNext(480, 21), OnCompleted<int>(800) });
            var e8 = scheduler.CreateHotObservable(new[] { OnNext(150, 1), OnNext(290, 9), OnNext(490, 22), OnCompleted<int>(800) });
            var e9 = scheduler.CreateHotObservable(new[] { OnNext(150, 1), OnNext(300, 10), OnNext(500, 23), OnCompleted<int>(800) });
            var e10 = scheduler.CreateHotObservable(new[] { OnNext(150, 1), OnNext(310, 11), OnNext(510, 24), OnCompleted<int>(800) });
            var e11 = scheduler.CreateHotObservable(new[] { OnNext(150, 1), OnNext(320, 12), OnNext(520, 25), OnCompleted<int>(800) });
            var e12 = scheduler.CreateHotObservable(new[] { OnNext(150, 1), OnNext(330, 13), OnNext(530, 26), OnCompleted<int>(800) });

            var res = scheduler.Start(() =>
                Observable.CombineLatest(e0, e1, e2, e3, e4, e5, e6, e7, e8, e9, e10, e11, e12, (_0, _1, _2, _3, _4, _5, _6, _7, _8, _9, _10, _11, _12) => _0 + _1 + _2 + _3 + _4 + _5 + _6 + _7 + _8 + _9 + _10 + _11 + _12)
            );

            res.Messages.AssertEqual(
                OnNext(330, 91),
                OnNext(410, 104),
                OnNext(420, 117),
                OnNext(430, 130),
                OnNext(440, 143),
                OnNext(450, 156),
                OnNext(460, 169),
                OnNext(470, 182),
                OnNext(480, 195),
                OnNext(490, 208),
                OnNext(500, 221),
                OnNext(510, 234),
                OnNext(520, 247),
                OnNext(530, 260),
                OnCompleted<int>(800)
            );

            foreach (var e in new[] { e0, e1, e2, e3, e4, e5, e6, e7, e8, e9, e10, e11, e12 })
                e.Subscriptions.AssertEqual(Subscribe(200, 800));
        }

        [TestMethod]
        public void CombineLatest_Typical14()
        {
            var scheduler = new TestScheduler();

            var e0 = scheduler.CreateHotObservable(new[] { OnNext(150, 1), OnNext(210, 1), OnNext(410, 15), OnCompleted<int>(800) });
            var e1 = scheduler.CreateHotObservable(new[] { OnNext(150, 1), OnNext(220, 2), OnNext(420, 16), OnCompleted<int>(800) });
            var e2 = scheduler.CreateHotObservable(new[] { OnNext(150, 1), OnNext(230, 3), OnNext(430, 17), OnCompleted<int>(800) });
            var e3 = scheduler.CreateHotObservable(new[] { OnNext(150, 1), OnNext(240, 4), OnNext(440, 18), OnCompleted<int>(800) });
            var e4 = scheduler.CreateHotObservable(new[] { OnNext(150, 1), OnNext(250, 5), OnNext(450, 19), OnCompleted<int>(800) });
            var e5 = scheduler.CreateHotObservable(new[] { OnNext(150, 1), OnNext(260, 6), OnNext(460, 20), OnCompleted<int>(800) });
            var e6 = scheduler.CreateHotObservable(new[] { OnNext(150, 1), OnNext(270, 7), OnNext(470, 21), OnCompleted<int>(800) });
            var e7 = scheduler.CreateHotObservable(new[] { OnNext(150, 1), OnNext(280, 8), OnNext(480, 22), OnCompleted<int>(800) });
            var e8 = scheduler.CreateHotObservable(new[] { OnNext(150, 1), OnNext(290, 9), OnNext(490, 23), OnCompleted<int>(800) });
            var e9 = scheduler.CreateHotObservable(new[] { OnNext(150, 1), OnNext(300, 10), OnNext(500, 24), OnCompleted<int>(800) });
            var e10 = scheduler.CreateHotObservable(new[] { OnNext(150, 1), OnNext(310, 11), OnNext(510, 25), OnCompleted<int>(800) });
            var e11 = scheduler.CreateHotObservable(new[] { OnNext(150, 1), OnNext(320, 12), OnNext(520, 26), OnCompleted<int>(800) });
            var e12 = scheduler.CreateHotObservable(new[] { OnNext(150, 1), OnNext(330, 13), OnNext(530, 27), OnCompleted<int>(800) });
            var e13 = scheduler.CreateHotObservable(new[] { OnNext(150, 1), OnNext(340, 14), OnNext(540, 28), OnCompleted<int>(800) });

            var res = scheduler.Start(() =>
                Observable.CombineLatest(e0, e1, e2, e3, e4, e5, e6, e7, e8, e9, e10, e11, e12, e13, (_0, _1, _2, _3, _4, _5, _6, _7, _8, _9, _10, _11, _12, _13) => _0 + _1 + _2 + _3 + _4 + _5 + _6 + _7 + _8 + _9 + _10 + _11 + _12 + _13)
            );
            
            res.Messages.AssertEqual(
                OnNext(340, 105),
                OnNext(410, 119),
                OnNext(420, 133),
                OnNext(430, 147),
                OnNext(440, 161),
                OnNext(450, 175),
                OnNext(460, 189),
                OnNext(470, 203),
                OnNext(480, 217),
                OnNext(490, 231),
                OnNext(500, 245),
                OnNext(510, 259),
                OnNext(520, 273),
                OnNext(530, 287),
                OnNext(540, 301),
                OnCompleted<int>(800)
            );

            foreach (var e in new[] { e0, e1, e2, e3, e4, e5, e6, e7, e8, e9, e10, e11, e12, e13 })
                e.Subscriptions.AssertEqual(Subscribe(200, 800));
        }

        [TestMethod]
        public void CombineLatest_Typical15()
        {
            var scheduler = new TestScheduler();

            var e0 = scheduler.CreateHotObservable(new[] { OnNext(150, 1), OnNext(210, 1), OnNext(410, 16), OnCompleted<int>(800) });
            var e1 = scheduler.CreateHotObservable(new[] { OnNext(150, 1), OnNext(220, 2), OnNext(420, 17), OnCompleted<int>(800) });
            var e2 = scheduler.CreateHotObservable(new[] { OnNext(150, 1), OnNext(230, 3), OnNext(430, 18), OnCompleted<int>(800) });
            var e3 = scheduler.CreateHotObservable(new[] { OnNext(150, 1), OnNext(240, 4), OnNext(440, 19), OnCompleted<int>(800) });
            var e4 = scheduler.CreateHotObservable(new[] { OnNext(150, 1), OnNext(250, 5), OnNext(450, 20), OnCompleted<int>(800) });
            var e5 = scheduler.CreateHotObservable(new[] { OnNext(150, 1), OnNext(260, 6), OnNext(460, 21), OnCompleted<int>(800) });
            var e6 = scheduler.CreateHotObservable(new[] { OnNext(150, 1), OnNext(270, 7), OnNext(470, 22), OnCompleted<int>(800) });
            var e7 = scheduler.CreateHotObservable(new[] { OnNext(150, 1), OnNext(280, 8), OnNext(480, 23), OnCompleted<int>(800) });
            var e8 = scheduler.CreateHotObservable(new[] { OnNext(150, 1), OnNext(290, 9), OnNext(490, 24), OnCompleted<int>(800) });
            var e9 = scheduler.CreateHotObservable(new[] { OnNext(150, 1), OnNext(300, 10), OnNext(500, 25), OnCompleted<int>(800) });
            var e10 = scheduler.CreateHotObservable(new[] { OnNext(150, 1), OnNext(310, 11), OnNext(510, 26), OnCompleted<int>(800) });
            var e11 = scheduler.CreateHotObservable(new[] { OnNext(150, 1), OnNext(320, 12), OnNext(520, 27), OnCompleted<int>(800) });
            var e12 = scheduler.CreateHotObservable(new[] { OnNext(150, 1), OnNext(330, 13), OnNext(530, 28), OnCompleted<int>(800) });
            var e13 = scheduler.CreateHotObservable(new[] { OnNext(150, 1), OnNext(340, 14), OnNext(540, 29), OnCompleted<int>(800) });
            var e14 = scheduler.CreateHotObservable(new[] { OnNext(150, 1), OnNext(350, 15), OnNext(550, 30), OnCompleted<int>(800) });

            var res = scheduler.Start(() =>
                Observable.CombineLatest(e0, e1, e2, e3, e4, e5, e6, e7, e8, e9, e10, e11, e12, e13, e14, (_0, _1, _2, _3, _4, _5, _6, _7, _8, _9, _10, _11, _12, _13, _14) => _0 + _1 + _2 + _3 + _4 + _5 + _6 + _7 + _8 + _9 + _10 + _11 + _12 + _13 + _14)
            );
            
            res.Messages.AssertEqual(
                OnNext(350, 120),
                OnNext(410, 135),
                OnNext(420, 150),
                OnNext(430, 165),
                OnNext(440, 180),
                OnNext(450, 195),
                OnNext(460, 210),
                OnNext(470, 225),
                OnNext(480, 240),
                OnNext(490, 255),
                OnNext(500, 270),
                OnNext(510, 285),
                OnNext(520, 300),
                OnNext(530, 315),
                OnNext(540, 330),
                OnNext(550, 345),
                OnCompleted<int>(800)
            );

            foreach (var e in new[] { e0, e1, e2, e3, e4, e5, e6, e7, e8, e9, e10, e11, e12, e13, e14 })
                e.Subscriptions.AssertEqual(Subscribe(200, 800));
        }

        [TestMethod]
        public void CombineLatest_Typical16()
        {
            var scheduler = new TestScheduler();

            var e0 = scheduler.CreateHotObservable(new[] { OnNext(150, 1), OnNext(210, 1), OnNext(410, 17), OnCompleted<int>(800) });
            var e1 = scheduler.CreateHotObservable(new[] { OnNext(150, 1), OnNext(220, 2), OnNext(420, 18), OnCompleted<int>(800) });
            var e2 = scheduler.CreateHotObservable(new[] { OnNext(150, 1), OnNext(230, 3), OnNext(430, 19), OnCompleted<int>(800) });
            var e3 = scheduler.CreateHotObservable(new[] { OnNext(150, 1), OnNext(240, 4), OnNext(440, 20), OnCompleted<int>(800) });
            var e4 = scheduler.CreateHotObservable(new[] { OnNext(150, 1), OnNext(250, 5), OnNext(450, 21), OnCompleted<int>(800) });
            var e5 = scheduler.CreateHotObservable(new[] { OnNext(150, 1), OnNext(260, 6), OnNext(460, 22), OnCompleted<int>(800) });
            var e6 = scheduler.CreateHotObservable(new[] { OnNext(150, 1), OnNext(270, 7), OnNext(470, 23), OnCompleted<int>(800) });
            var e7 = scheduler.CreateHotObservable(new[] { OnNext(150, 1), OnNext(280, 8), OnNext(480, 24), OnCompleted<int>(800) });
            var e8 = scheduler.CreateHotObservable(new[] { OnNext(150, 1), OnNext(290, 9), OnNext(490, 25), OnCompleted<int>(800) });
            var e9 = scheduler.CreateHotObservable(new[] { OnNext(150, 1), OnNext(300, 10), OnNext(500, 26), OnCompleted<int>(800) });
            var e10 = scheduler.CreateHotObservable(new[] { OnNext(150, 1), OnNext(310, 11), OnNext(510, 27), OnCompleted<int>(800) });
            var e11 = scheduler.CreateHotObservable(new[] { OnNext(150, 1), OnNext(320, 12), OnNext(520, 28), OnCompleted<int>(800) });
            var e12 = scheduler.CreateHotObservable(new[] { OnNext(150, 1), OnNext(330, 13), OnNext(530, 29), OnCompleted<int>(800) });
            var e13 = scheduler.CreateHotObservable(new[] { OnNext(150, 1), OnNext(340, 14), OnNext(540, 30), OnCompleted<int>(800) });
            var e14 = scheduler.CreateHotObservable(new[] { OnNext(150, 1), OnNext(350, 15), OnNext(550, 31), OnCompleted<int>(800) });
            var e15 = scheduler.CreateHotObservable(new[] { OnNext(150, 1), OnNext(360, 16), OnNext(560, 32), OnCompleted<int>(800) });

            var res = scheduler.Start(() =>
                Observable.CombineLatest(e0, e1, e2, e3, e4, e5, e6, e7, e8, e9, e10, e11, e12, e13, e14, e15, (_0, _1, _2, _3, _4, _5, _6, _7, _8, _9, _10, _11, _12, _13, _14, _15) => _0 + _1 + _2 + _3 + _4 + _5 + _6 + _7 + _8 + _9 + _10 + _11 + _12 + _13 + _14 + _15)
            );
            
            res.Messages.AssertEqual(
                OnNext(360, 136),
                OnNext(410, 152),
                OnNext(420, 168),
                OnNext(430, 184),
                OnNext(440, 200),
                OnNext(450, 216),
                OnNext(460, 232),
                OnNext(470, 248),
                OnNext(480, 264),
                OnNext(490, 280),
                OnNext(500, 296),
                OnNext(510, 312),
                OnNext(520, 328),
                OnNext(530, 344),
                OnNext(540, 360),
                OnNext(550, 376),
                OnNext(560, 392),
                OnCompleted<int>(800)
            );

            foreach (var e in new[] { e0, e1, e2, e3, e4, e5, e6, e7, e8, e9, e10, e11, e12, e13, e14, e15 })
                e.Subscriptions.AssertEqual(Subscribe(200, 800));
        }
#endif

        #endregion

        #region NAry

        [TestMethod]
        public void CombineLatest_List_Regular()
        {
            var scheduler = new TestScheduler();
            
            var e0 = scheduler.CreateHotObservable(new[] { OnNext(150, 1), OnNext(210, 1), OnNext(240, 4), OnCompleted<int>(270) });
            var e1 = scheduler.CreateHotObservable(new[] { OnNext(150, 1), OnNext(220, 2), OnNext(250, 5), OnCompleted<int>(280) });
            var e2 = scheduler.CreateHotObservable(new[] { OnNext(150, 1), OnNext(230, 3), OnNext(260, 6), OnCompleted<int>(290) });
            
            var res = scheduler.Start(() =>
                Observable.CombineLatest<int>(new IObservable<int>[] { e0, e1, e2 }.AsEnumerable())
            );

            res.Messages.AssertEqual(
                OnNext<IList<int>>(230, l => l.SequenceEqual(new[] { 1, 2, 3 })),
                OnNext<IList<int>>(240, l => l.SequenceEqual(new[] { 4, 2, 3 })),
                OnNext<IList<int>>(250, l => l.SequenceEqual(new[] { 4, 5, 3 })),
                OnNext<IList<int>>(260, l => l.SequenceEqual(new[] { 4, 5, 6 })),
                OnCompleted<IList<int>>(290)
            );
        }

        [TestMethod]
        public void CombineLatest_NAry_ArgumentChecking()
        {
            ReactiveAssert.Throws<ArgumentNullException>(() => Observable.CombineLatest(default(IEnumerable<IObservable<int>>)));
            ReactiveAssert.Throws<ArgumentNullException>(() => Observable.CombineLatest(default(IEnumerable<IObservable<int>>), _ => 42));
            ReactiveAssert.Throws<ArgumentNullException>(() => Observable.CombineLatest(new[] { Observable.Return(42) }, default(Func<IList<int>, string>)));
            ReactiveAssert.Throws<ArgumentNullException>(() => Observable.CombineLatest(default(IObservable<int>[])));
        }

        [TestMethod]
        public void CombineLatest_NAry_Symmetric()
        {
            var scheduler = new TestScheduler();

            var e0 = scheduler.CreateHotObservable(new[] { OnNext(150, 1), OnNext(210, 1), OnNext(250, 4), OnCompleted<int>(420) });
            var e1 = scheduler.CreateHotObservable(new[] { OnNext(150, 1), OnNext(220, 2), OnNext(240, 5), OnCompleted<int>(410) });
            var e2 = scheduler.CreateHotObservable(new[] { OnNext(150, 1), OnNext(230, 3), OnNext(260, 6), OnCompleted<int>(400) });

            var res = scheduler.Start(() =>
                Observable.CombineLatest(e0, e1, e2)
            );

            res.Messages.AssertEqual(
                OnNext<IList<int>>(230, l => l.SequenceEqual(new[] { 1, 2, 3 })),
                OnNext<IList<int>>(240, l => l.SequenceEqual(new[] { 1, 5, 3 })),
                OnNext<IList<int>>(250, l => l.SequenceEqual(new[] { 4, 5, 3 })),
                OnNext<IList<int>>(260, l => l.SequenceEqual(new[] { 4, 5, 6 })),
                OnCompleted<IList<int>>(420)
            );

            e0.Subscriptions.AssertEqual(
                Subscribe(200, 420)
            );

            e1.Subscriptions.AssertEqual(
                Subscribe(200, 410)
            );

            e2.Subscriptions.AssertEqual(
                Subscribe(200, 400)
            );
        }

        [TestMethod]
        public void CombineLatest_NAry_Symmetric_Selector()
        {
            var scheduler = new TestScheduler();

            var e0 = scheduler.CreateHotObservable(new[] { OnNext(150, 1), OnNext(210, 1), OnNext(250, 4), OnCompleted<int>(420) });
            var e1 = scheduler.CreateHotObservable(new[] { OnNext(150, 1), OnNext(220, 2), OnNext(240, 5), OnCompleted<int>(410) });
            var e2 = scheduler.CreateHotObservable(new[] { OnNext(150, 1), OnNext(230, 3), OnNext(260, 6), OnCompleted<int>(400) });

            var res = scheduler.Start(() =>
                Observable.CombineLatest(new[] { e0, e1, e2 }, xs => xs.Sum())
            );

            res.Messages.AssertEqual(
                OnNext<int>(230, new[] { 1, 2, 3 }.Sum()),
                OnNext<int>(240, new[] { 1, 5, 3 }.Sum()),
                OnNext<int>(250, new[] { 4, 5, 3 }.Sum()),
                OnNext<int>(260, new[] { 4, 5, 6 }.Sum()),
                OnCompleted<int>(420)
            );

            e0.Subscriptions.AssertEqual(
                Subscribe(200, 420)
            );

            e1.Subscriptions.AssertEqual(
                Subscribe(200, 410)
            );

            e2.Subscriptions.AssertEqual(
                Subscribe(200, 400)
            );
        }

        [TestMethod]
        public void CombineLatest_NAry_Asymmetric()
        {
            var scheduler = new TestScheduler();

            var e0 = scheduler.CreateHotObservable(new[] { OnNext(150, 1), OnNext(210, 1), OnNext(250, 4), OnCompleted<int>(270) });
            var e1 = scheduler.CreateHotObservable(new[] { OnNext(150, 1), OnNext(220, 2), OnNext(240, 5), OnNext(290, 7), OnNext(310, 9), OnCompleted<int>(410) });
            var e2 = scheduler.CreateHotObservable(new[] { OnNext(150, 1), OnNext(230, 3), OnNext(260, 6), OnNext(280, 8), OnCompleted<int>(300) });

            var res = scheduler.Start(() =>
                Observable.CombineLatest(e0, e1, e2)
            );

            res.Messages.AssertEqual(
                OnNext<IList<int>>(230, l => l.SequenceEqual(new[] { 1, 2, 3 })),
                OnNext<IList<int>>(240, l => l.SequenceEqual(new[] { 1, 5, 3 })),
                OnNext<IList<int>>(250, l => l.SequenceEqual(new[] { 4, 5, 3 })),
                OnNext<IList<int>>(260, l => l.SequenceEqual(new[] { 4, 5, 6 })),
                OnNext<IList<int>>(280, l => l.SequenceEqual(new[] { 4, 5, 8 })),
                OnNext<IList<int>>(290, l => l.SequenceEqual(new[] { 4, 7, 8 })),
                OnNext<IList<int>>(310, l => l.SequenceEqual(new[] { 4, 9, 8 })),
                OnCompleted<IList<int>>(410)
            );

            e0.Subscriptions.AssertEqual(
                Subscribe(200, 270)
            );

            e1.Subscriptions.AssertEqual(
                Subscribe(200, 410)
            );

            e2.Subscriptions.AssertEqual(
                Subscribe(200, 300)
            );
        }

        [TestMethod]
        public void CombineLatest_NAry_Asymmetric_Selector()
        {
            var scheduler = new TestScheduler();

            var e0 = scheduler.CreateHotObservable(new[] { OnNext(150, 1), OnNext(210, 1), OnNext(250, 4), OnCompleted<int>(270) });
            var e1 = scheduler.CreateHotObservable(new[] { OnNext(150, 1), OnNext(220, 2), OnNext(240, 5), OnNext(290, 7), OnNext(310, 9), OnCompleted<int>(410) });
            var e2 = scheduler.CreateHotObservable(new[] { OnNext(150, 1), OnNext(230, 3), OnNext(260, 6), OnNext(280, 8), OnCompleted<int>(300) });

            var res = scheduler.Start(() =>
                Observable.CombineLatest(new[] { e0, e1, e2 }, xs => xs.Sum())
            );

            res.Messages.AssertEqual(
                OnNext<int>(230, new[] { 1, 2, 3 }.Sum()),
                OnNext<int>(240, new[] { 1, 5, 3 }.Sum()),
                OnNext<int>(250, new[] { 4, 5, 3 }.Sum()),
                OnNext<int>(260, new[] { 4, 5, 6 }.Sum()),
                OnNext<int>(280, new[] { 4, 5, 8 }.Sum()),
                OnNext<int>(290, new[] { 4, 7, 8 }.Sum()),
                OnNext<int>(310, new[] { 4, 9, 8 }.Sum()),
                OnCompleted<int>(410)
            );

            e0.Subscriptions.AssertEqual(
                Subscribe(200, 270)
            );

            e1.Subscriptions.AssertEqual(
                Subscribe(200, 410)
            );

            e2.Subscriptions.AssertEqual(
                Subscribe(200, 300)
            );
        }

        [TestMethod]
        public void CombineLatest_NAry_Error()
        {
            var ex = new Exception();

            var scheduler = new TestScheduler();

            var e0 = scheduler.CreateHotObservable(new[] { OnNext(150, 1), OnNext(210, 1), OnError<int>(250, ex) });
            var e1 = scheduler.CreateHotObservable(new[] { OnNext(150, 1), OnNext(220, 2), OnNext(240, 5), OnCompleted<int>(410) });
            var e2 = scheduler.CreateHotObservable(new[] { OnNext(150, 1), OnNext(230, 3), OnNext(260, 6), OnCompleted<int>(400) });

            var res = scheduler.Start(() =>
                Observable.CombineLatest(e0, e1, e2)
            );

            res.Messages.AssertEqual(
                OnNext<IList<int>>(230, l => l.SequenceEqual(new[] { 1, 2, 3 })),
                OnNext<IList<int>>(240, l => l.SequenceEqual(new[] { 1, 5, 3 })),
                OnError<IList<int>>(250, ex)
            );

            e0.Subscriptions.AssertEqual(
                Subscribe(200, 250)
            );

            e1.Subscriptions.AssertEqual(
                Subscribe(200, 250)
            );

            e2.Subscriptions.AssertEqual(
                Subscribe(200, 250)
            );
        }

        [TestMethod]
        public void CombineLatest_NAry_Error_Selector()
        {
            var ex = new Exception();

            var scheduler = new TestScheduler();

            var e0 = scheduler.CreateHotObservable(new[] { OnNext(150, 1), OnNext(210, 1), OnError<int>(250, ex) });
            var e1 = scheduler.CreateHotObservable(new[] { OnNext(150, 1), OnNext(220, 2), OnNext(240, 5), OnCompleted<int>(410) });
            var e2 = scheduler.CreateHotObservable(new[] { OnNext(150, 1), OnNext(230, 3), OnNext(260, 6), OnCompleted<int>(400) });

            var res = scheduler.Start(() =>
                Observable.CombineLatest(new[] { e0, e1, e2 }, xs => xs.Sum())
            );

            res.Messages.AssertEqual(
                OnNext<int>(230, new[] { 1, 2, 3 }.Sum()),
                OnNext<int>(240, new[] { 1, 5, 3 }.Sum()),
                OnError<int>(250, ex)
            );

            e0.Subscriptions.AssertEqual(
                Subscribe(200, 250)
            );

            e1.Subscriptions.AssertEqual(
                Subscribe(200, 250)
            );

            e2.Subscriptions.AssertEqual(
                Subscribe(200, 250)
            );
        }

        #endregion

        #region AtLeastOneThrows

        [TestMethod]
        public void CombineLatest_AtLeastOneThrows4()
        {
            var ex = new Exception();

            var scheduler = new TestScheduler();
            var e0 = scheduler.CreateHotObservable(new[] { OnNext(150, 1), OnNext(210, 1), OnCompleted<int>(400) });
            var e1 = scheduler.CreateHotObservable(new[] { OnNext(150, 1), OnNext(220, 2), OnCompleted<int>(400) });
            var e2 = scheduler.CreateHotObservable(new[] { OnNext(150, 1), OnError<int>(230, ex) });
            var e3 = scheduler.CreateHotObservable(new[] { OnNext(150, 1), OnNext(240, 4), OnCompleted<int>(400) });

            var res = scheduler.Start(() =>
                Observable.CombineLatest(e0, e1, e2, e3, (_0, _1, _2, _3) => 42)
            );
            
            res.Messages.AssertEqual(
                OnError<int>(230, ex)
            );

            e0.Subscriptions.AssertEqual(Subscribe(200, 230));
            e1.Subscriptions.AssertEqual(Subscribe(200, 230));
            e2.Subscriptions.AssertEqual(Subscribe(200, 230));
            e3.Subscriptions.AssertEqual(Subscribe(200, 230));
        }

        #endregion

        #endregion

        #region + Concat +

        [TestMethod]
        public void Concat_ArgumentChecking()
        {
            var xs = DummyObservable<int>.Instance;

            ReactiveAssert.Throws<ArgumentNullException>(() => Observable.Concat(xs, (IObservable<int>)null));
            ReactiveAssert.Throws<ArgumentNullException>(() => Observable.Concat((IObservable<int>)null, xs));
            ReactiveAssert.Throws<ArgumentNullException>(() => Observable.Concat((IObservable<int>[])null));
            ReactiveAssert.Throws<ArgumentNullException>(() => Observable.Concat((IEnumerable<IObservable<int>>)null));
            ReactiveAssert.Throws<ArgumentNullException>(() => Observable.Concat((IObservable<int>[])null));
            ReactiveAssert.Throws<ArgumentNullException>(() => Observable.Concat((IEnumerable<IObservable<int>>)null));
            ReactiveAssert.Throws<ArgumentNullException>(() => Observable.Concat(xs, null));
            ReactiveAssert.Throws<ArgumentNullException>(() => Observable.Concat(null, xs));
        }

        [TestMethod]
        public void Concat_DefaultScheduler()
        {
            var evt = new ManualResetEvent(false);

            int sum = 0;
            Observable.Concat(Observable.Return(1), Observable.Return(2), Observable.Return(3)).Subscribe(n =>
            {
                sum += n;
            }, () => evt.Set());

            evt.WaitOne();

            Assert.AreEqual(6, sum);
        }

        [TestMethod]
        public void Concat_IEofIO_DefaultScheduler()
        {
            var evt = new ManualResetEvent(false);

            IEnumerable<IObservable<int>> sources = new[] { Observable.Return(1), Observable.Return(2), Observable.Return(3) };

            int sum = 0;
            Observable.Concat(sources).Subscribe(n =>
            {
                sum += n;
            }, () => evt.Set());

            evt.WaitOne();

            Assert.AreEqual(6, sum);
        }

        [TestMethod]
        public void Concat_IEofIO_GetEnumeratorThrows()
        {
            var ex = new Exception();

            var scheduler = new TestScheduler();

            var xss = new RogueEnumerable<IObservable<int>>(ex);

            var res = scheduler.Start(() =>
                Observable.Concat(xss)
            );

            res.Messages.AssertEqual(
                OnError<int>(200, ex)
            );
        }

        [TestMethod]
        public void Concat_IEofIO()
        {
            var scheduler = new TestScheduler();

            var xs1 = scheduler.CreateColdObservable<int>(
                OnNext(10, 1),
                OnNext(20, 2),
                OnNext(30, 3),
                OnCompleted<int>(40)
            );

            var xs2 = scheduler.CreateColdObservable<int>(
                OnNext(10, 4),
                OnNext(20, 5),
                OnCompleted<int>(30)
            );

            var xs3 = scheduler.CreateColdObservable<int>(
                OnNext(10, 6),
                OnNext(20, 7),
                OnNext(30, 8),
                OnNext(40, 9),
                OnCompleted<int>(50)
            );

            var res = scheduler.Start(() =>
                Observable.Concat(new[] { xs1, xs2, xs3 })
            );

            res.Messages.AssertEqual(
                OnNext(210, 1),
                OnNext(220, 2),
                OnNext(230, 3),
                OnNext(250, 4),
                OnNext(260, 5),
                OnNext(280, 6),
                OnNext(290, 7),
                OnNext(300, 8),
                OnNext(310, 9),
                OnCompleted<int>(320)
            );

            xs1.Subscriptions.AssertEqual(
                Subscribe(200, 240)
            );

            xs2.Subscriptions.AssertEqual(
                Subscribe(240, 270)
            );

            xs3.Subscriptions.AssertEqual(
                Subscribe(270, 320)
            );
        }

        [TestMethod]
        public void Concat_EmptyEmpty()
        {
            var scheduler = new TestScheduler();

            var o1 = scheduler.CreateHotObservable(
                OnNext(150, 1),
                OnCompleted<int>(230)
            );

            var o2 = scheduler.CreateHotObservable(
                OnNext(150, 1),
                OnCompleted<int>(250)
            );

            var res = scheduler.Start(() =>
                o1.Concat(o2)
            );

            res.Messages.AssertEqual(
                OnCompleted<int>(250)
            );

            o1.Subscriptions.AssertEqual(
                Subscribe(200, 230)
            );

            o2.Subscriptions.AssertEqual(
                Subscribe(230, 250)
            );
        }

        [TestMethod]
        public void Concat_EmptyNever()
        {
            var scheduler = new TestScheduler();

            var o1 = scheduler.CreateHotObservable(
                OnNext(150, 1),
                OnCompleted<int>(230)
            );

            var o2 = scheduler.CreateHotObservable(
                OnNext(150, 1)
            );

            var res = scheduler.Start(() =>
                o1.Concat(o2)
            );

            res.Messages.AssertEqual(
            );

            o1.Subscriptions.AssertEqual(
                Subscribe(200, 230)
            );

            o2.Subscriptions.AssertEqual(
                Subscribe(230, 1000)
            );
        }

        [TestMethod]
        public void Concat_NeverEmpty()
        {
            var scheduler = new TestScheduler();

            var o1 = scheduler.CreateHotObservable(
                OnNext(150, 1)
            );

            var o2 = scheduler.CreateHotObservable(
                OnNext(150, 1),
                OnCompleted<int>(230)
            );

            var res = scheduler.Start(() =>
                o1.Concat(o2)
            );

            res.Messages.AssertEqual(
            );

            o1.Subscriptions.AssertEqual(
                Subscribe(200, 1000)
            );

            o2.Subscriptions.AssertEqual(
            );
        }

        [TestMethod]
        public void Concat_NeverNever()
        {
            var scheduler = new TestScheduler();

            var o1 = scheduler.CreateHotObservable(
                OnNext(150, 1)
            );

            var o2 = scheduler.CreateHotObservable(
                OnNext(150, 1)
            );

            var res = scheduler.Start(() =>
                o1.Concat(o2)
            );

            res.Messages.AssertEqual(
            );

            o1.Subscriptions.AssertEqual(
                Subscribe(200, 1000)
            );

            o2.Subscriptions.AssertEqual(
            );
        }

        [TestMethod]
        public void Concat_EmptyThrow()
        {
            var scheduler = new TestScheduler();

            var ex = new Exception();

            var o1 = scheduler.CreateHotObservable(
                OnNext(150, 1),
                OnCompleted<int>(230)
            );

            var o2 = scheduler.CreateHotObservable(
                OnNext(150, 1),
                OnError<int>(250, ex)
            );

            var res = scheduler.Start(() =>
                o1.Concat(o2)
            );

            res.Messages.AssertEqual(
                OnError<int>(250, ex)
            );

            o1.Subscriptions.AssertEqual(
                Subscribe(200, 230)
            );

            o2.Subscriptions.AssertEqual(
                Subscribe(230, 250)
            );
        }

        [TestMethod]
        public void Concat_ThrowEmpty()
        {
            var scheduler = new TestScheduler();

            var ex = new Exception();

            var o1 = scheduler.CreateHotObservable(
                OnNext(150, 1),
                OnError<int>(230, ex)
            );

            var o2 = scheduler.CreateHotObservable(
                OnNext(150, 1),
                OnCompleted<int>(250)
            );

            var res = scheduler.Start(() =>
                o1.Concat(o2)
            );

            res.Messages.AssertEqual(
                OnError<int>(230, ex)
            );

            o1.Subscriptions.AssertEqual(
                Subscribe(200, 230)
            );

            o2.Subscriptions.AssertEqual(
            );
        }

        [TestMethod]
        public void Concat_ThrowThrow()
        {
            var scheduler = new TestScheduler();

            var ex = new Exception();

            var o1 = scheduler.CreateHotObservable(
                OnNext(150, 1),
                OnError<int>(230, ex)
            );

            var o2 = scheduler.CreateHotObservable(
                OnNext(150, 1),
                OnError<int>(250, new Exception())
            );

            var res = scheduler.Start(() =>
                o1.Concat(o2)
            );

            res.Messages.AssertEqual(
                OnError<int>(230, ex)
            );

            o1.Subscriptions.AssertEqual(
                Subscribe(200, 230)
            );

            o2.Subscriptions.AssertEqual(
            );
        }

        [TestMethod]
        public void Concat_ReturnEmpty()
        {
            var scheduler = new TestScheduler();

            var o1 = scheduler.CreateHotObservable(
                OnNext(150, 1),
                OnNext(210, 2),
                OnCompleted<int>(230)
            );

            var o2 = scheduler.CreateHotObservable(
                OnNext(150, 1),
                OnCompleted<int>(250)
            );

            var res = scheduler.Start(() =>
                o1.Concat(o2)
            );

            res.Messages.AssertEqual(
                OnNext(210, 2),
                OnCompleted<int>(250)
            );

            o1.Subscriptions.AssertEqual(
                Subscribe(200, 230)
            );

            o2.Subscriptions.AssertEqual(
                Subscribe(230, 250)
            );
        }

        [TestMethod]
        public void Concat_EmptyReturn()
        {
            var scheduler = new TestScheduler();

            var o1 = scheduler.CreateHotObservable(
                OnNext(150, 1),
                OnCompleted<int>(230)
            );

            var o2 = scheduler.CreateHotObservable(
                OnNext(150, 1),
                OnNext(240, 2),
                OnCompleted<int>(250)
            );

            var res = scheduler.Start(() =>
                o1.Concat(o2)
            );

            res.Messages.AssertEqual(
                OnNext(240, 2),
                OnCompleted<int>(250)
            );

            o1.Subscriptions.AssertEqual(
                Subscribe(200, 230)
            );

            o2.Subscriptions.AssertEqual(
                Subscribe(230, 250)
            );
        }

        [TestMethod]
        public void Concat_ReturnNever()
        {
            var scheduler = new TestScheduler();

            var o1 = scheduler.CreateHotObservable(
                OnNext(150, 1),
                OnNext(210, 2),
                OnCompleted<int>(230)
            );

            var o2 = scheduler.CreateHotObservable(
                OnNext(150, 1)
            );

            var res = scheduler.Start(() =>
                o1.Concat(o2)
            );

            res.Messages.AssertEqual(
                OnNext(210, 2)
            );

            o1.Subscriptions.AssertEqual(
                Subscribe(200, 230)
            );

            o2.Subscriptions.AssertEqual(
                Subscribe(230, 1000)
            );
        }

        [TestMethod]
        public void Concat_NeverReturn()
        {
            var scheduler = new TestScheduler();

            var o1 = scheduler.CreateHotObservable(
                OnNext(150, 1)
            );

            var o2 = scheduler.CreateHotObservable(
                OnNext(150, 1),
                OnNext(210, 2),
                OnCompleted<int>(230)
            );

            var res = scheduler.Start(() =>
                o1.Concat(o2)
            );

            res.Messages.AssertEqual(
            );

            o1.Subscriptions.AssertEqual(
                Subscribe(200, 1000)
            );

            o2.Subscriptions.AssertEqual(
            );
        }

        [TestMethod]
        public void Concat_ReturnReturn()
        {
            var scheduler = new TestScheduler();

            var ex = new Exception();

            var o1 = scheduler.CreateHotObservable(
                OnNext(150, 1),
                OnNext(220, 2),
                OnCompleted<int>(230)
            );

            var o2 = scheduler.CreateHotObservable(
                OnNext(150, 1),
                OnNext(240, 3),
                OnCompleted<int>(250)
            );

            var res = scheduler.Start(() =>
                o1.Concat(o2)
            );

            res.Messages.AssertEqual(
                OnNext(220, 2),
                OnNext(240, 3),
                OnCompleted<int>(250)
            );

            o1.Subscriptions.AssertEqual(
                Subscribe(200, 230)
            );

            o2.Subscriptions.AssertEqual(
                Subscribe(230, 250)
            );
        }

        [TestMethod]
        public void Concat_ThrowReturn()
        {
            var scheduler = new TestScheduler();

            var ex = new Exception();

            var o1 = scheduler.CreateHotObservable(
                OnNext(150, 1),
                OnError<int>(230, ex)
            );

            var o2 = scheduler.CreateHotObservable(
                OnNext(150, 1),
                OnNext(240, 2),
                OnCompleted<int>(250)
            );

            var res = scheduler.Start(() =>
                o1.Concat(o2)
            );

            res.Messages.AssertEqual(
                OnError<int>(230, ex)
            );

            o1.Subscriptions.AssertEqual(
                Subscribe(200, 230)
            );

            o2.Subscriptions.AssertEqual(
            );
        }

        [TestMethod]
        public void Concat_ReturnThrow()
        {
            var scheduler = new TestScheduler();

            var ex = new Exception();

            var o1 = scheduler.CreateHotObservable(
                OnNext(150, 1),
                OnNext(220, 2),
                OnCompleted<int>(230)
            );

            var o2 = scheduler.CreateHotObservable(
                OnNext(150, 1),
                OnError<int>(250, ex)
            );

            var res = scheduler.Start(() =>
                o1.Concat(o2)
            );

            res.Messages.AssertEqual(
                OnNext(220, 2),
                OnError<int>(250, ex)
            );

            o1.Subscriptions.AssertEqual(
                Subscribe(200, 230)
            );

            o2.Subscriptions.AssertEqual(
                Subscribe(230, 250)
            );
        }

        [TestMethod]
        public void Concat_SomeDataSomeData()
        {
            var scheduler = new TestScheduler();

            var ex = new Exception();

            var o1 = scheduler.CreateHotObservable(
                OnNext(150, 1),
                OnNext(210, 2),
                OnNext(220, 3),
                OnCompleted<int>(225)
            );

            var o2 = scheduler.CreateHotObservable(
                OnNext(150, 1),
                OnNext(230, 4),
                OnNext(240, 5),
                OnCompleted<int>(250)
            );

            var res = scheduler.Start(() =>
                o1.Concat(o2)
            );

            res.Messages.AssertEqual(
                OnNext(210, 2),
                OnNext(220, 3),
                OnNext(230, 4),
                OnNext(240, 5),
                OnCompleted<int>(250)
            );

            o1.Subscriptions.AssertEqual(
                Subscribe(200, 225)
            );

            o2.Subscriptions.AssertEqual(
                Subscribe(225, 250)
            );
        }

        [TestMethod]
        public void Concat_EnumerableThrows()
        {
            var scheduler = new TestScheduler();

            var o = scheduler.CreateHotObservable(
                OnNext(150, 1),
                OnNext(210, 2),
                OnNext(220, 3),
                OnCompleted<int>(225)
            );

            var ex = new Exception();
            var xss = new MockEnumerable<IObservable<int>>(scheduler, GetObservablesForConcatThrow(o, ex));

            var res = scheduler.Start(() =>
                xss.Concat()
            );

            res.Messages.AssertEqual(
                OnNext(210, 2),
                OnNext(220, 3),
                OnError<int>(225, ex)
            );

            o.Subscriptions.AssertEqual(
                Subscribe(200, 225)
            );

            xss.Subscriptions.AssertEqual(
                Subscribe(200, 225)
            );
        }

        private IEnumerable<IObservable<int>> GetObservablesForConcatThrow(IObservable<int> first, Exception ex)
        {
            yield return first;
            throw ex;
        }

        [TestMethod]
        public void Concat_EnumerableTiming()
        {
            var scheduler = new TestScheduler();

            var o1 = scheduler.CreateHotObservable(
                OnNext(150, 1),
                OnNext(210, 2), // !
                OnNext(220, 3), // !
                OnCompleted<int>(230)
            );

            var o2 = scheduler.CreateColdObservable(
                OnNext(50, 4),  // !
                OnNext(60, 5),  // !
                OnNext(70, 6),  // !
                OnCompleted<int>(80)
            );

            var o3 = scheduler.CreateHotObservable(
                OnNext(150, 1),
                OnNext(200, 2),
                OnNext(210, 3),
                OnNext(220, 4),
                OnNext(230, 5),
                OnNext(270, 6),
                OnNext(320, 7), // !
                OnNext(330, 8), // !
                OnCompleted<int>(340)
            );

            var xss = new MockEnumerable<ITestableObservable<int>>(scheduler, new[] { o1, o2, o3, o2 });

            var res = scheduler.Start(() =>
                xss.Select(xs => (IObservable<int>)xs).Concat()
            );

            res.Messages.AssertEqual(
                OnNext(210, 2),
                OnNext(220, 3),
                OnNext(280, 4),
                OnNext(290, 5),
                OnNext(300, 6),
                OnNext(320, 7),
                OnNext(330, 8),
                OnNext(390, 4),
                OnNext(400, 5),
                OnNext(410, 6),
                OnCompleted<int>(420)
            );

            o1.Subscriptions.AssertEqual(
                Subscribe(200, 230)
            );

            o2.Subscriptions.AssertEqual(
                Subscribe(230, 310),
                Subscribe(340, 420)
            );

            o3.Subscriptions.AssertEqual(
                Subscribe(310, 340)
            );

            xss.Subscriptions.AssertEqual(
                Subscribe(200, 420)
            );
        }

        [TestMethod]
        public void Concat_Enumerable_Dispose()
        {
            var scheduler = new TestScheduler();

            var o1 = scheduler.CreateHotObservable(
                OnNext(150, 1),
                OnNext(210, 2),
                OnNext(220, 3),
                OnCompleted<int>(230)
            );

            var o2 = scheduler.CreateHotObservable(
                OnNext(150, 1),
                OnNext(200, 2),
                OnNext(210, 3),
                OnNext(240, 4),
                OnNext(270, 5),
                OnNext(320, 6),
                OnNext(330, 7),
                OnCompleted<int>(340)
            );

            var xss = new MockEnumerable<ITestableObservable<int>>(scheduler, new[] { o1, o2 });

            var res = scheduler.Start(() =>
                xss.Select(xs => (IObservable<int>)xs).Concat(),
                300
            );

            res.Messages.AssertEqual(
                OnNext(210, 2),
                OnNext(220, 3),
                OnNext(240, 4),
                OnNext(270, 5)
            );

            o1.Subscriptions.AssertEqual(
                Subscribe(200, 230)
            );

            o2.Subscriptions.AssertEqual(
                Subscribe(230, 300)
            );

            xss.Subscriptions.AssertEqual(
                Subscribe(200, 300)
            );
        }

        [TestMethod]
        public void Concat_Optimization_DeferEvalTiming()
        {
            var scheduler = new TestScheduler();

            var o1 = scheduler.CreateHotObservable(
                OnNext(150, 1),
                OnNext(210, 2),
                OnNext(220, 3),
                OnCompleted<int>(230)
            );

            var o2 = scheduler.CreateColdObservable(
                OnNext(10, 4),
                OnNext(20, 5),
                OnNext(30, 6),
                OnCompleted<int>(40)
            );

            var invoked = default(long);

            var xs = o1;
            var ys = Observable.Defer(() => { invoked = scheduler.Clock; return o2; });

            var res = scheduler.Start(() =>
                xs.Concat(ys)
            );

            res.Messages.AssertEqual(
                OnNext(210, 2),
                OnNext(220, 3),
                OnNext(240, 4),
                OnNext(250, 5),
                OnNext(260, 6),
                OnCompleted<int>(270)
            );

            o1.Subscriptions.AssertEqual(
                Subscribe(200, 230)
            );

            o2.Subscriptions.AssertEqual(
                Subscribe(230, 270)
            );

            Assert.AreEqual(230, invoked);
        }

        [TestMethod]
        public void Concat_Optimization_DeferExceptionPropagation()
        {
            var scheduler = new TestScheduler();

            var o1 = scheduler.CreateHotObservable(
                OnNext(150, 1),
                OnNext(210, 2),
                OnCompleted<int>(220)
            );

            var ex = new Exception();
            var invoked = default(long);

            var xs = o1;
            var ys = Observable.Defer<int>(new Func<IObservable<int>>(() => { invoked = scheduler.Clock; throw ex; }));

            var res = scheduler.Start(() =>
                xs.Concat(ys)
            );

            res.Messages.AssertEqual(
                OnNext(210, 2),
                OnError<int>(220, ex)
            );

            o1.Subscriptions.AssertEqual(
                Subscribe(200, 220)
            );

            Assert.AreEqual(220, invoked);
        }

#if !NO_PERF
        [TestMethod]
        public void Concat_TailRecursive1()
        {
            var create = 0L;
            var start = 200L;
            var end = 1000L;

            var scheduler = new TestScheduler();

            var o = scheduler.CreateColdObservable<int>(
                OnNext(10, 1),
                OnNext(20, 2),
                OnNext(30, 3),
                OnCompleted<int>(40)
            );

            var f = default(Func<IObservable<int>>);
            f = () => Observable.Defer(() => o.Concat(f()));

            var res = scheduler.Start(() => f(), create, start, end);

            var expected = new List<Recorded<Notification<int>>>();

            var t = start;
            while (t <= end)
            {
                var n = (t - start) / 10;
                if (n % 4 != 0)
                {
                    expected.Add(OnNext(t, (int)(n % 4)));
                }

                t += 10;
            }

            res.Messages.AssertEqual(expected);
        }

        [TestMethod]
        public void Concat_TailRecursive2()
        {
            var f = default(Func<int, IObservable<int>>);
            f = x => Observable.Defer(() => Observable.Return(x, ThreadPoolScheduler.Instance).Concat(f(x + 1)));

            var lst = new List<int>();
            f(0).Select(x => new StackTrace().FrameCount).Take(10).ForEach(lst.Add);

            Assert.IsTrue(lst.Last() - lst.First() < 10);
        }
#endif

#if !NO_TPL
        [TestMethod]
        public void Concat_Task()
        {
            var tss = Observable.Concat(new[] { Task.Factory.StartNew(() => 1), Task.Factory.StartNew(() => 2), Task.Factory.StartNew(() => 3) }.ToObservable());

            var res = tss.ToArray().Single();

            Assert.IsTrue(res.SequenceEqual(new[] { 1, 2, 3 }));
        }
#endif

        #endregion

        #region + Merge +

        [TestMethod]
        public void Merge_ArgumentChecking()
        {
            var xs = DummyObservable<int>.Instance;

            ReactiveAssert.Throws<ArgumentNullException>(() => Observable.Merge(default(IScheduler), xs, xs));
            ReactiveAssert.Throws<ArgumentNullException>(() => Observable.Merge(xs, xs, default(IScheduler)));
            ReactiveAssert.Throws<ArgumentNullException>(() => Observable.Merge(xs, null));
            ReactiveAssert.Throws<ArgumentNullException>(() => Observable.Merge(default(IObservable<int>), xs));
            ReactiveAssert.Throws<ArgumentNullException>(() => Observable.Merge((IObservable<int>[])null));
            ReactiveAssert.Throws<ArgumentNullException>(() => Observable.Merge((IEnumerable<IObservable<int>>)null));
            ReactiveAssert.Throws<ArgumentNullException>(() => ((IObservable<int>)null).Merge(xs, DummyScheduler.Instance));
            ReactiveAssert.Throws<ArgumentNullException>(() => xs.Merge(default(IObservable<int>), DummyScheduler.Instance));
            ReactiveAssert.Throws<ArgumentNullException>(() => Observable.Merge((IEnumerable<IObservable<int>>)null, DummyScheduler.Instance));
            ReactiveAssert.Throws<ArgumentNullException>(() => Observable.Merge(new IObservable<int>[0], default(IScheduler)));
            ReactiveAssert.Throws<ArgumentNullException>(() => Observable.Merge((IObservable<IObservable<int>>)null));
            ReactiveAssert.Throws<ArgumentNullException>(() => Observable.Merge(DummyScheduler.Instance, (IObservable<int>[])null));

#if !NO_TPL
            ReactiveAssert.Throws<ArgumentNullException>(() => Observable.Merge((IObservable<System.Threading.Tasks.Task<int>>)null));
#endif
        }

        [TestMethod]
        public void Merge_DefaultScheduler()
        {
            var xs = Observable.Merge<int>(Observable.Return(42), Observable.Return(43), Observable.Return(44));
            var res = xs.ToList().Single();
            Assert.IsTrue(new[] { 42, 43, 44 }.SequenceEqual(res));
        }

        [TestMethod]
        public void Merge_Never2()
        {
            var scheduler = new TestScheduler();

            var n1 = scheduler.CreateHotObservable(
                OnNext(150, 1)
            );

            var n2 = scheduler.CreateHotObservable(
                OnNext(150, 1)
            );

            var res = scheduler.Start(() =>
                Observable.Merge(scheduler, n1, n2)
            );

            res.Messages.AssertEqual(
            );

            n1.Subscriptions.AssertEqual(
                Subscribe(201, 1000)
            );

            n2.Subscriptions.AssertEqual(
                Subscribe(202, 1000)
            );
        }

        [TestMethod]
        public void Merge_Never3()
        {
            var scheduler = new TestScheduler();

            var n1 = scheduler.CreateHotObservable(
                OnNext(150, 1)
            );

            var n2 = scheduler.CreateHotObservable(
                OnNext(150, 1)
            );

            var n3 = scheduler.CreateHotObservable(
                OnNext(150, 1)
            );

            var res = scheduler.Start(() =>
                Observable.Merge(scheduler, n1, n2, n3)
            );

            res.Messages.AssertEqual(
            );

            n1.Subscriptions.AssertEqual(
                Subscribe(201, 1000)
            );

            n2.Subscriptions.AssertEqual(
                Subscribe(202, 1000)
            );

            n3.Subscriptions.AssertEqual(
                Subscribe(203, 1000)
            );
        }

        [TestMethod]
        public void Merge_Empty2()
        {
            var scheduler = new TestScheduler();

            var e1 = scheduler.CreateHotObservable(
                OnNext(150, 1),
                OnCompleted<int>(210)
            );

            var e2 = scheduler.CreateHotObservable(
                OnNext(150, 1),
                OnCompleted<int>(230)
            );

            var res = scheduler.Start(() =>
                Observable.Merge(scheduler, e1, e2)
            );

            res.Messages.AssertEqual(
                OnCompleted<int>(230)
            );

            e1.Subscriptions.AssertEqual(
                Subscribe(201, 210)
            );

            e2.Subscriptions.AssertEqual(
                Subscribe(202, 230)
            );
        }

        [TestMethod]
        public void Merge_Empty3()
        {
            var scheduler = new TestScheduler();

            var e1 = scheduler.CreateHotObservable(
                OnNext(150, 1),
                OnCompleted<int>(210)
            );

            var e2 = scheduler.CreateHotObservable(
                OnNext(150, 1),
                OnCompleted<int>(230)
            );

            var e3 = scheduler.CreateHotObservable(
                OnNext(150, 1),
                OnCompleted<int>(240)
            );

            var res = scheduler.Start(() =>
                Observable.Merge(scheduler, e1, e2, e3)
            );

            res.Messages.AssertEqual(
                OnCompleted<int>(240)
            );

            e1.Subscriptions.AssertEqual(
                Subscribe(201, 210)
            );

            e2.Subscriptions.AssertEqual(
                Subscribe(202, 230)
            );

            e3.Subscriptions.AssertEqual(
                Subscribe(203, 240)
            );
        }

        [TestMethod]
        public void Merge_EmptyDelayed2_RightLast()
        {
            var scheduler = new TestScheduler();

            var e1 = scheduler.CreateHotObservable(
                OnNext(150, 1),
                OnCompleted<int>(240)
            );

            var e2 = scheduler.CreateHotObservable(
                OnNext(150, 1),
                OnCompleted<int>(250)
            );

            var res = scheduler.Start(() =>
                Observable.Merge(scheduler, e1, e2)
            );

            res.Messages.AssertEqual(
                OnCompleted<int>(250)
            );

            e1.Subscriptions.AssertEqual(
                Subscribe(201, 240)
            );

            e2.Subscriptions.AssertEqual(
                Subscribe(202, 250)
            );
        }

        [TestMethod]
        public void Merge_EmptyDelayed2_LeftLast()
        {
            var scheduler = new TestScheduler();

            var e1 = scheduler.CreateHotObservable(
                OnNext(150, 1),
                OnCompleted<int>(250)
            );

            var e2 = scheduler.CreateHotObservable(
                OnNext(150, 1),
                OnCompleted<int>(240)
            );

            var res = scheduler.Start(() =>
                Observable.Merge(scheduler, e1, e2)
            );

            res.Messages.AssertEqual(
                OnCompleted<int>(250)
            );

            e1.Subscriptions.AssertEqual(
                Subscribe(201, 250)
            );

            e2.Subscriptions.AssertEqual(
                Subscribe(202, 240)
            );
        }

        [TestMethod]
        public void Merge_EmptyDelayed3_MiddleLast()
        {
            var scheduler = new TestScheduler();

            var e1 = scheduler.CreateHotObservable(
                OnNext(150, 1),
                OnCompleted<int>(245)
            );

            var e2 = scheduler.CreateHotObservable(
                OnNext(150, 1),
                OnCompleted<int>(250)
            );

            var e3 = scheduler.CreateHotObservable(
                OnNext(150, 1),
                OnCompleted<int>(240)
            );

            var res = scheduler.Start(() =>
                Observable.Merge(scheduler, e1, e2, e3)
            );

            res.Messages.AssertEqual(
                OnCompleted<int>(250)
            );

            e1.Subscriptions.AssertEqual(
                Subscribe(201, 245)
            );

            e2.Subscriptions.AssertEqual(
                Subscribe(202, 250)
            );

            e3.Subscriptions.AssertEqual(
                Subscribe(203, 240)
            );
        }

        [TestMethod]
        public void Merge_EmptyNever()
        {
            var scheduler = new TestScheduler();

            var e1 = scheduler.CreateHotObservable(
                OnNext(150, 1),
                OnCompleted<int>(245)
            );

            var n1 = scheduler.CreateHotObservable(
                OnNext(150, 1)
            );

            var res = scheduler.Start(() =>
                Observable.Merge(scheduler, e1, n1)
            );

            res.Messages.AssertEqual(
            );

            e1.Subscriptions.AssertEqual(
                Subscribe(201, 245)
            );

            n1.Subscriptions.AssertEqual(
                Subscribe(202, 1000)
            );
        }

        [TestMethod]
        public void Merge_NeverEmpty()
        {
            var scheduler = new TestScheduler();

            var n1 = scheduler.CreateHotObservable(
                OnNext(150, 1)
            );

            var e1 = scheduler.CreateHotObservable(
                OnNext(150, 1),
                OnCompleted<int>(245)
            );

            var res = scheduler.Start(() =>
                Observable.Merge(scheduler, n1, e1)
            );

            res.Messages.AssertEqual(
            );

            n1.Subscriptions.AssertEqual(
                Subscribe(201, 1000)
            );

            e1.Subscriptions.AssertEqual(
                Subscribe(202, 245)
            );
        }

        [TestMethod]
        public void Merge_ReturnNever()
        {
            var scheduler = new TestScheduler();

            var r1 = scheduler.CreateHotObservable(
                OnNext(150, 1),
                OnNext(210, 2),
                OnCompleted<int>(245)
            );

            var n1 = scheduler.CreateHotObservable(
                OnNext(150, 1)
            );

            var res = scheduler.Start(() =>
                Observable.Merge(scheduler, r1, n1)
            );

            res.Messages.AssertEqual(
                OnNext(210, 2)
            );

            r1.Subscriptions.AssertEqual(
                Subscribe(201, 245)
            );

            n1.Subscriptions.AssertEqual(
                Subscribe(202, 1000)
            );
        }

        [TestMethod]
        public void Merge_NeverReturn()
        {
            var scheduler = new TestScheduler();

            var n1 = scheduler.CreateHotObservable(
                OnNext(150, 1)
            );

            var r1 = scheduler.CreateHotObservable(
                OnNext(150, 1),
                OnNext(210, 2),
                OnCompleted<int>(245)
            );

            var res = scheduler.Start(() =>
                Observable.Merge(scheduler, n1, r1)
            );

            res.Messages.AssertEqual(
                OnNext(210, 2)
            );

            n1.Subscriptions.AssertEqual(
                Subscribe(201, 1000)
            );

            r1.Subscriptions.AssertEqual(
                Subscribe(202, 245)
            );
        }

        [TestMethod]
        public void Merge_ErrorNever()
        {
            var scheduler = new TestScheduler();

            var ex = new Exception();

            var e1 = scheduler.CreateHotObservable(
                OnNext(150, 1),
                OnNext(210, 2),
                OnError<int>(245, ex)
            );

            var n1 = scheduler.CreateHotObservable(
                OnNext(150, 1)
            );

            var res = scheduler.Start(() =>
                Observable.Merge(scheduler, e1, n1)
            );

            res.Messages.AssertEqual(
                OnNext(210, 2),
                OnError<int>(245, ex)
            );

            e1.Subscriptions.AssertEqual(
                Subscribe(201, 245)
            );

            n1.Subscriptions.AssertEqual(
                Subscribe(202, 245)
            );
        }

        [TestMethod]
        public void Merge_NeverError()
        {
            var scheduler = new TestScheduler();

            var ex = new Exception();

            var n1 = scheduler.CreateHotObservable(
                OnNext(150, 1)
            );

            var e1 = scheduler.CreateHotObservable(
                OnNext(150, 1),
                OnNext(210, 2),
                OnError<int>(245, ex)
            );

            var res = scheduler.Start(() =>
                Observable.Merge(scheduler, n1, e1)
            );

            res.Messages.AssertEqual(
                OnNext(210, 2),
                OnError<int>(245, ex)
            );

            n1.Subscriptions.AssertEqual(
                Subscribe(201, 245)
            );

            e1.Subscriptions.AssertEqual(
                Subscribe(202, 245)
            );
        }

        [TestMethod]
        public void Merge_EmptyReturn()
        {
            var scheduler = new TestScheduler();

            var e1 = scheduler.CreateHotObservable(
                OnNext(150, 1),
                OnCompleted<int>(245)
            );

            var r1 = scheduler.CreateHotObservable(
                OnNext(150, 1),
                OnNext(210, 2),
                OnCompleted<int>(250)
            );

            var res = scheduler.Start(() =>
                Observable.Merge(scheduler, e1, r1)
            );

            res.Messages.AssertEqual(
                OnNext(210, 2),
                OnCompleted<int>(250)
            );

            e1.Subscriptions.AssertEqual(
                Subscribe(201, 245)
            );

            r1.Subscriptions.AssertEqual(
                Subscribe(202, 250)
            );
        }

        [TestMethod]
        public void Merge_ReturnEmpty()
        {
            var scheduler = new TestScheduler();

            var r1 = scheduler.CreateHotObservable(
                OnNext(150, 1),
                OnNext(210, 2),
                OnCompleted<int>(250)
            );

            var e1 = scheduler.CreateHotObservable(
                OnNext(150, 1),
                OnCompleted<int>(245)
            );

            var res = scheduler.Start(() =>
                Observable.Merge(scheduler, r1, e1)
            );

            res.Messages.AssertEqual(
                OnNext(210, 2),
                OnCompleted<int>(250)
            );

            r1.Subscriptions.AssertEqual(
                Subscribe(201, 250)
            );

            e1.Subscriptions.AssertEqual(
                Subscribe(202, 245)
            );
        }

        [TestMethod]
        public void Merge_Lots2()
        {
            var scheduler = new TestScheduler();

            var o1 = scheduler.CreateHotObservable(
                OnNext(150, 1),
                OnNext(210, 2),
                OnNext(220, 4),
                OnNext(230, 6),
                OnNext(240, 8),
                OnCompleted<int>(245)
            );

            var o2 = scheduler.CreateHotObservable(
                OnNext(150, 1),
                OnNext(215, 3),
                OnNext(225, 5),
                OnNext(235, 7),
                OnNext(245, 9),
                OnCompleted<int>(250)
            );

            var res = scheduler.Start(() =>
                Observable.Merge(scheduler, o1, o2)
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
                Subscribe(201, 245)
            );

            o2.Subscriptions.AssertEqual(
                Subscribe(202, 250)
            );
        }

        [TestMethod]
        public void Merge_Lots3()
        {
            var scheduler = new TestScheduler();

            var o1 = scheduler.CreateHotObservable(
                OnNext(150, 1),
                OnNext(210, 2),
                OnNext(225, 5),
                OnNext(240, 8),
                OnCompleted<int>(245)
            );

            var o2 = scheduler.CreateHotObservable(
                OnNext(150, 1),
                OnNext(215, 3),
                OnNext(230, 6),
                OnNext(245, 9),
                OnCompleted<int>(250)
            );

            var o3 = scheduler.CreateHotObservable(
                OnNext(150, 1),
                OnNext(220, 4),
                OnNext(235, 7),
                OnCompleted<int>(240)
            );

            var res = scheduler.Start(() =>
                new[] { o1, o2, o3 }.Merge(scheduler)
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
                Subscribe(201, 245)
            );

            o2.Subscriptions.AssertEqual(
                Subscribe(202, 250)
            );

            o3.Subscriptions.AssertEqual(
                Subscribe(203, 240)
            );
        }

        [TestMethod]
        public void Merge_LotsMore()
        {
            var inputs = new List<List<Recorded<Notification<int>>>>();

            const int N = 10;
            for (int i = 0; i < N; i++)
            {
                var lst = new List<Recorded<Notification<int>>> { OnNext(150, 1) };
                inputs.Add(lst);

                ushort start = (ushort)(301 + i);
                for (int j = 0; j < i; j++)
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

            var scheduler = new TestScheduler();

            // Last ToArray: got to create the hot observables *now*
            var xss = inputs.Select(lst => (IObservable<int>)scheduler.CreateHotObservable(lst.ToArray())).ToArray();

            var res = scheduler.Start(() =>
                xss.Merge(scheduler)
            );

            Assert.AreEqual(resOnNext.Count + 1, res.Messages.Count, "length");
            for (int i = 0; i < resOnNext.Count; i++)
            {
                var msg = res.Messages[i];
                Assert.IsTrue(msg.Time == resOnNext[i].Time);
                Assert.IsTrue(msg.Value.Kind == NotificationKind.OnNext);
                Assert.IsTrue(msg.Value.Value == resOnNext[i].Value.Value);
            }
            Assert.IsTrue(res.Messages[resOnNext.Count].Value.Kind == NotificationKind.OnCompleted && res.Messages[resOnNext.Count].Time == lastCompleted.Time, "complete");
        }

        [TestMethod]
        public void Merge_ErrorLeft()
        {
            var scheduler = new TestScheduler();

            var ex = new Exception();

            var o1 = scheduler.CreateHotObservable(
                OnNext(150, 1),
                OnNext(210, 2),
                OnError<int>(245, ex)
            );

            var o2 = scheduler.CreateHotObservable(
                OnNext(150, 1),
                OnNext(215, 3),
                OnCompleted<int>(250)
            );

            var res = scheduler.Start(() =>
                Observable.Merge(o1, o2, scheduler)
            );

            res.Messages.AssertEqual(
                OnNext(210, 2),
                OnNext(215, 3),
                OnError<int>(245, ex)
            );

            o1.Subscriptions.AssertEqual(
                Subscribe(201, 245)
            );

            o2.Subscriptions.AssertEqual(
                Subscribe(202, 245)
            );
        }

        [TestMethod]
        public void Merge_ErrorCausesDisposal()
        {
            var scheduler = new TestScheduler();

            var ex = new Exception();

            var e1 = scheduler.CreateHotObservable(
                OnNext(150, 1),
                OnError<int>(210, ex) //!
            );

            var o1 = scheduler.CreateHotObservable(
                OnNext(150, 1),
                OnNext(220, 1), // should not come
                OnCompleted<int>(230)
            );

            var res = scheduler.Start(() =>
                Observable.Merge(e1, o1, scheduler)
            );

            res.Messages.AssertEqual(
                OnError<int>(210, ex) //!
            );

            e1.Subscriptions.AssertEqual(
                Subscribe(201, 210)
            );

            o1.Subscriptions.AssertEqual(
                Subscribe(202, 210)
            );
        }

        [TestMethod]
        public void Merge_ObservableOfObservable_Data()
        {
            var scheduler = new TestScheduler();

            var ys1 = scheduler.CreateColdObservable(
                OnNext(10, 101),
                OnNext(20, 102),
                OnNext(110, 103),
                OnNext(120, 104),
                OnNext(210, 105),
                OnNext(220, 106),
                OnCompleted<int>(230)
            );

            var ys2 = scheduler.CreateColdObservable(
                OnNext(10, 201),
                OnNext(20, 202),
                OnNext(30, 203),
                OnNext(40, 204),
                OnCompleted<int>(50)
            );

            var ys3 = scheduler.CreateColdObservable(
                OnNext(10, 301),
                OnNext(20, 302),
                OnNext(30, 303),
                OnNext(40, 304),
                OnNext(120, 305),
                OnCompleted<int>(150)
            );

            var xs = scheduler.CreateHotObservable(
                OnNext<IObservable<int>>(300, ys1),
                OnNext<IObservable<int>>(400, ys2),
                OnNext<IObservable<int>>(500, ys3),
                OnCompleted<IObservable<int>>(600)
            );

            var res = scheduler.Start(() =>
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

#if !NO_PERF
            // BREAKING CHANGE v2 > v1.x -> More aggressive disposal behavior
            xs.Subscriptions.AssertEqual(
                Subscribe(200, 600)
            );
#else
            xs.Subscriptions.AssertEqual(
                Subscribe(200, 650)
            );
#endif

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
            var scheduler = new TestScheduler();

            var ys1 = scheduler.CreateColdObservable(
                OnNext(10, 101),
                OnNext(20, 102),
                OnCompleted<int>(230)
            );

            var ys2 = scheduler.CreateColdObservable(
                OnNext(10, 201),
                OnNext(20, 202),
                OnNext(30, 203),
                OnNext(40, 204),
                OnCompleted<int>(50)
            );

            var ys3 = scheduler.CreateColdObservable(
                OnNext(10, 301),
                OnNext(20, 302),
                OnNext(30, 303),
                OnNext(40, 304),
                OnCompleted<int>(50)
            );

            var xs = scheduler.CreateHotObservable(
                OnNext<IObservable<int>>(300, ys1),
                OnNext<IObservable<int>>(400, ys2),
                OnNext<IObservable<int>>(500, ys3),
                OnCompleted<IObservable<int>>(600)
            );

            var res = scheduler.Start(() =>
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
            var scheduler = new TestScheduler();

            var ex = new Exception();

            var ys1 = scheduler.CreateColdObservable(
                OnNext(10, 101),
                OnNext(20, 102),
                OnNext(110, 103),
                OnNext(120, 104),
                OnNext(210, 105),
                OnNext(220, 106),
                OnCompleted<int>(230)
            );

            var ys2 = scheduler.CreateColdObservable(
                OnNext(10, 201),
                OnNext(20, 202),
                OnNext(30, 203),
                OnNext(40, 204),
                OnError<int>(50, ex)
            );

            var ys3 = scheduler.CreateColdObservable(
                OnNext(10, 301),
                OnNext(20, 302),
                OnNext(30, 303),
                OnNext(40, 304),
                OnCompleted<int>(150)
            );

            var xs = scheduler.CreateHotObservable(
                OnNext<IObservable<int>>(300, ys1),
                OnNext<IObservable<int>>(400, ys2),
                OnNext<IObservable<int>>(500, ys3),
                OnCompleted<IObservable<int>>(600)
            );

            var res = scheduler.Start(() =>
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
            var scheduler = new TestScheduler();

            var ex = new Exception();

            var ys1 = scheduler.CreateColdObservable(
                OnNext(10, 101),
                OnNext(20, 102),
                OnNext(110, 103),
                OnNext(120, 104),
                OnNext(210, 105),
                OnNext(220, 106),
                OnCompleted<int>(230)
            );

            var ys2 = scheduler.CreateColdObservable(
                OnNext(10, 201),
                OnNext(20, 202),
                OnNext(30, 203),
                OnNext(40, 204),
                OnCompleted<int>(50)
            );

            var xs = scheduler.CreateHotObservable(
                OnNext<IObservable<int>>(300, ys1),
                OnNext<IObservable<int>>(400, ys2),
                OnError<IObservable<int>>(500, ex)
            );

            var res = scheduler.Start(() =>
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
        public void Merge_Binary_DefaultScheduler()
        {
            Assert.IsTrue(Observable.Return(1).Merge(Observable.Return(2)).ToEnumerable().OrderBy(x => x).SequenceEqual(new[] { 1, 2 }));
        }

        [TestMethod]
        public void Merge_Params_DefaultScheduler()
        {
            Assert.IsTrue(Observable.Merge(Observable.Return(1), Observable.Return(2)).ToEnumerable().OrderBy(x => x).SequenceEqual(new[] { 1, 2 }));
        }

        [TestMethod]
        public void Merge_IEnumerableOfIObservable_DefaultScheduler()
        {
            Assert.IsTrue(Observable.Merge((IEnumerable<IObservable<int>>)new[] { Observable.Return(1), Observable.Return(2) }).ToEnumerable().OrderBy(x => x).SequenceEqual(new[] { 1, 2 }));
        }

        [TestMethod]
        public void MergeConcat_ArgumentChecking()
        {
            ReactiveAssert.Throws<ArgumentNullException>(() => Observable.Merge(default(IEnumerable<IObservable<int>>), 1, DummyScheduler.Instance));
            ReactiveAssert.Throws<ArgumentOutOfRangeException>(() => Observable.Merge(DummyEnumerable<IObservable<int>>.Instance, 0, DummyScheduler.Instance));
            ReactiveAssert.Throws<ArgumentNullException>(() => Observable.Merge(DummyEnumerable<IObservable<int>>.Instance, 1, null));
            ReactiveAssert.Throws<ArgumentNullException>(() => Observable.Merge(default(IEnumerable<IObservable<int>>), 1));
            ReactiveAssert.Throws<ArgumentOutOfRangeException>(() => Observable.Merge(DummyEnumerable<IObservable<int>>.Instance, 0));
            ReactiveAssert.Throws<ArgumentNullException>(() => Observable.Merge(default(IObservable<IObservable<int>>), 1));
            ReactiveAssert.Throws<ArgumentOutOfRangeException>(() => Observable.Merge(DummyObservable<IObservable<int>>.Instance, 0));
            ReactiveAssert.Throws<ArgumentNullException>(() => Observable.Concat(default(IObservable<IObservable<int>>)));

#if !NO_TPL
            ReactiveAssert.Throws<ArgumentNullException>(() => Observable.Concat(default(IObservable<System.Threading.Tasks.Task<int>>)));
#endif
        }

        [TestMethod]
        public void MergeConcat_Enumerable_Scheduler()
        {
            var b = Enumerable.Range(1, 3).Select(x => Observable.Range(x * 10, 3)).Merge(1)
                    .SequenceEqual(new[] { 10, 11, 12, 20, 21, 22, 30, 31, 32 }.ToObservable())
                    .First();
            Assert.IsTrue(b);
        }

        [TestMethod]
        public void MergeConcat_Enumerable()
        {
            var b = Enumerable.Range(1, 3).Select(x => Observable.Range(x * 10, 3)).Merge(1, DefaultScheduler.Instance)
                    .SequenceEqual(new[] { 10, 11, 12, 20, 21, 22, 30, 31, 32 }.ToObservable())
                    .First();
            Assert.IsTrue(b);
        }

        [TestMethod]
        public void MergeConcat_Default()
        {
            var b = Observable.Range(1, 3).Select(x => Observable.Range(x * 10, 3)).Concat()
                    .SequenceEqual(new[] { 10, 11, 12, 20, 21, 22, 30, 31, 32 }.ToObservable())
                    .First();
            Assert.IsTrue(b);
        }

        [TestMethod]
        public void MergeConcat_Basic()
        {
            var scheduler = new TestScheduler();

            var ys1 = scheduler.CreateColdObservable(
                OnNext(50, 1),
                OnNext(100, 2),
                OnNext(120, 3),
                OnCompleted<int>(140)
            );

            var ys2 = scheduler.CreateColdObservable(
                OnNext(20, 4),
                OnNext(70, 5),
                OnCompleted<int>(200)
            );

            var ys3 = scheduler.CreateColdObservable(
                OnNext(10, 6),
                OnNext(90, 7),
                OnNext(110, 8),
                OnCompleted<int>(130)
            );

            var ys4 = scheduler.CreateColdObservable(
                OnNext(210, 9),
                OnNext(240, 10),
                OnCompleted<int>(300)
            );

            var xs = scheduler.CreateHotObservable<IObservable<int>>(
                OnNext<IObservable<int>>(210, ys1),
                OnNext<IObservable<int>>(260, ys2),
                OnNext<IObservable<int>>(270, ys3),
                OnNext<IObservable<int>>(320, ys4),
                OnCompleted<IObservable<int>>(400)
            );

            var res = scheduler.Start(() =>
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

#if !NO_PERF
            // BREAKING CHANGE v2 > v1.x -> More aggressive disposal behavior
            xs.Subscriptions.AssertEqual(
                Subscribe(200, 400)
            );
#else
            xs.Subscriptions.AssertEqual(
                Subscribe(200, 760)
            );
#endif

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
            var scheduler = new TestScheduler();

            var ys1 = scheduler.CreateColdObservable(
                OnNext(50, 1),
                OnNext(100, 2),
                OnNext(120, 3),
                OnCompleted<int>(140)
            );

            var ys2 = scheduler.CreateColdObservable(
                OnNext(20, 4),
                OnNext(70, 5),
                OnCompleted<int>(300)
            );

            var ys3 = scheduler.CreateColdObservable(
                OnNext(10, 6),
                OnNext(90, 7),
                OnNext(110, 8),
                OnCompleted<int>(130)
            );

            var ys4 = scheduler.CreateColdObservable(
                OnNext(210, 9),
                OnNext(240, 10),
                OnCompleted<int>(300)
            );

            var xs = scheduler.CreateHotObservable(
                OnNext<IObservable<int>>(210, ys1),
                OnNext<IObservable<int>>(260, ys2),
                OnNext<IObservable<int>>(270, ys3),
                OnNext<IObservable<int>>(320, ys4),
                OnCompleted<IObservable<int>>(400)
            );

            var res = scheduler.Start(() =>
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

#if !NO_PERF
            // BREAKING CHANGE v2 > v1.x -> More aggressive disposal behavior
            xs.Subscriptions.AssertEqual(
                Subscribe(200, 400)
            );
#else
            xs.Subscriptions.AssertEqual(
                Subscribe(200, 780)
            );
#endif

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
            var scheduler = new TestScheduler();

            var ys1 = scheduler.CreateColdObservable(
                OnNext(50, 1),
                OnNext(100, 2),
                OnNext(120, 3),
                OnCompleted<int>(140)
            );

            var ys2 = scheduler.CreateColdObservable(
                OnNext(20, 4),
                OnNext(70, 5),
                OnCompleted<int>(300)
            );

            var ys3 = scheduler.CreateColdObservable(
                OnNext(10, 6),
                OnNext(90, 7),
                OnNext(110, 8),
                OnCompleted<int>(130)
            );

            var ys4 = scheduler.CreateColdObservable(
                OnNext(210, 9),
                OnNext(240, 10),
                OnCompleted<int>(300)
            );

            var xs = scheduler.CreateHotObservable(
                OnNext<IObservable<int>>(210, ys1),
                OnNext<IObservable<int>>(260, ys2),
                OnNext<IObservable<int>>(270, ys3),
                OnNext<IObservable<int>>(420, ys4),
                OnCompleted<IObservable<int>>(450)
            );

            var res = scheduler.Start(() =>
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

#if !NO_PERF
            // BREAKING CHANGE v2 > v1.x -> More aggressive disposal behavior
            xs.Subscriptions.AssertEqual(
                Subscribe(200, 450)
            );
#else
            xs.Subscriptions.AssertEqual(
                Subscribe(200, 720)
            );
#endif

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
            var scheduler = new TestScheduler();

            var ys1 = scheduler.CreateColdObservable(
                OnNext(50, 1),
                OnNext(100, 2),
                OnNext(120, 3),
                OnCompleted<int>(140)
            );

            var ys2 = scheduler.CreateColdObservable(
                OnNext(20, 4),
                OnNext(70, 5),
                OnCompleted<int>(300)
            );

            var ys3 = scheduler.CreateColdObservable(
                OnNext(10, 6),
                OnNext(90, 7),
                OnNext(110, 8),
                OnCompleted<int>(130)
            );

            var ys4 = scheduler.CreateColdObservable(
                OnNext(210, 9),
                OnNext(240, 10),
                OnCompleted<int>(300)
            );

            var xs = scheduler.CreateHotObservable(
                OnNext<IObservable<int>>(210, ys1),
                OnNext<IObservable<int>>(260, ys2),
                OnNext<IObservable<int>>(270, ys3),
                OnNext<IObservable<int>>(420, ys4),
                OnCompleted<IObservable<int>>(750)
            );

            var res = scheduler.Start(() =>
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
            var scheduler = new TestScheduler();

            var ys1 = scheduler.CreateColdObservable(
                OnNext(50, 1),
                OnNext(100, 2),
                OnNext(120, 3),
                OnCompleted<int>(140)
            );

            var ys2 = scheduler.CreateColdObservable(
                OnNext(20, 4),
                OnNext(70, 5),
                OnCompleted<int>(200)
            );

            var ys3 = scheduler.CreateColdObservable(
                OnNext(10, 6),
                OnNext(90, 7),
                OnNext(110, 8),
                OnCompleted<int>(130)
            );

            var ys4 = scheduler.CreateColdObservable(
                OnNext(210, 9),
                OnNext(240, 10),
                OnCompleted<int>(300)
            );

            var xs = scheduler.CreateHotObservable(
                OnNext<IObservable<int>>(210, ys1),
                OnNext<IObservable<int>>(260, ys2),
                OnNext<IObservable<int>>(270, ys3),
                OnNext<IObservable<int>>(320, ys4),
                OnCompleted<IObservable<int>>(400)
            );

            var res = scheduler.Start(() =>
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

#if !NO_PERF
            // BREAKING CHANGE v2 > v1.x -> More aggressive disposal behavior
            xs.Subscriptions.AssertEqual(
                Subscribe(200, 400)
            );
#else
            xs.Subscriptions.AssertEqual(
                Subscribe(200, 450)
            );
#endif

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
            var scheduler = new TestScheduler();

            var ex = new Exception();

            var ys1 = scheduler.CreateColdObservable(
                OnNext(50, 1),
                OnNext(100, 2),
                OnNext(120, 3),
                OnCompleted<int>(140)
            );

            var ys2 = scheduler.CreateColdObservable(
                OnNext(20, 4),
                OnNext(70, 5),
                OnCompleted<int>(200)
            );

            var ys3 = scheduler.CreateColdObservable(
                OnNext(10, 6),
                OnNext(90, 7),
                OnNext(110, 8),
                OnCompleted<int>(130)
            );

            var ys4 = scheduler.CreateColdObservable(
                OnNext(210, 9),
                OnNext(240, 10),
                OnCompleted<int>(300)
            );

            var xs = scheduler.CreateHotObservable(
                OnNext<IObservable<int>>(210, ys1),
                OnNext<IObservable<int>>(260, ys2),
                OnNext<IObservable<int>>(270, ys3),
                OnNext<IObservable<int>>(320, ys4),
                OnError<IObservable<int>>(400, ex)
            );

            var res = scheduler.Start(() =>
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
            var scheduler = new TestScheduler();

            var ex = new Exception();

            var ys1 = scheduler.CreateColdObservable(
                OnNext(50, 1),
                OnNext(100, 2),
                OnNext(120, 3),
                OnCompleted<int>(140)
            );

            var ys2 = scheduler.CreateColdObservable(
                OnNext(20, 4),
                OnNext(70, 5),
                OnCompleted<int>(200)
            );

            var ys3 = scheduler.CreateColdObservable(
                OnNext(10, 6),
                OnNext(90, 7),
                OnNext(110, 8),
                OnError<int>(140, ex)
            );

            var ys4 = scheduler.CreateColdObservable(
                OnNext(210, 9),
                OnNext(240, 10),
                OnCompleted<int>(300)
            );

            var xs = scheduler.CreateHotObservable(
                OnNext<IObservable<int>>(210, ys1),
                OnNext<IObservable<int>>(260, ys2),
                OnNext<IObservable<int>>(270, ys3),
                OnNext<IObservable<int>>(320, ys4),
                OnCompleted<IObservable<int>>(400)
            );

            var res = scheduler.Start(() =>
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

#if !NO_PERF
            // BREAKING CHANGE v2 > v1.x -> More aggressive disposal behavior
            xs.Subscriptions.AssertEqual(
                Subscribe(200, 400)
            );
#else
            xs.Subscriptions.AssertEqual(
                Subscribe(200, 490)
            );
#endif

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

#if !NO_TPL
        [TestMethod]
        public void Merge_Task()
        {
            var tss = Observable.Merge(new[] { Task.Factory.StartNew(() => 1), Task.Factory.StartNew(() => 2), Task.Factory.StartNew(() => 3) }.ToObservable());

            var res = tss.ToArray().Single();

            Assert.IsTrue(res.OrderBy(x => x).SequenceEqual(new[] { 1, 2, 3 }));
        }

        [TestMethod]
        public void Merge_TaskWithCompletionSource_RanToCompletion_Async()
        {
            var tcss = new TaskCompletionSource<int>[2];
            tcss[0] = new TaskCompletionSource<int>();
            tcss[1] = new TaskCompletionSource<int>();

            var res = Observable.Merge(Observable.Range(0, 2).Select(x => tcss[x].Task));

            var lst = new List<int>();

            var done = new ManualResetEvent(false);
            res.Subscribe(lst.Add, () => done.Set());

            tcss[0].SetResult(42);
            tcss[1].SetResult(43);

            done.WaitOne();

            lst.OrderBy(x => x).AssertEqual(new[] { 42, 43 });
        }

        [TestMethod]
        public void Merge_TaskWithCompletionSource_RanToCompletion_Sync()
        {
            var tcss = new TaskCompletionSource<int>[2];
            tcss[0] = new TaskCompletionSource<int>();
            tcss[1] = new TaskCompletionSource<int>();

            tcss[0].SetResult(42);
            tcss[1].SetResult(43);

            var res = Observable.Merge(Observable.Range(0, 2).Select(x => tcss[x].Task));

            var lst = new List<int>();

            var done = new ManualResetEvent(false);
            res.Subscribe(lst.Add, () => done.Set());

            done.WaitOne();

            lst.OrderBy(x => x).AssertEqual(new[] { 42, 43 });
        }

        [TestMethod]
        public void Merge_TaskWithCompletionSource_Faulted_Async()
        {
            var tcss = new TaskCompletionSource<int>[3];
            tcss[0] = new TaskCompletionSource<int>();
            tcss[1] = new TaskCompletionSource<int>();
            tcss[2] = new TaskCompletionSource<int>();

            var res = Observable.Merge(Observable.Range(0, 3).Select(x => tcss[x].Task));

            var lst = new List<int>();

            var err = default(Exception);
            var done = new ManualResetEvent(false);
            res.Subscribe(lst.Add, ex_ => { err = ex_; done.Set(); }, () => done.Set());

            var ex = new Exception();
            tcss[1].SetException(ex);

            done.WaitOne();

            lst.AssertEqual(new int[0]);
            Assert.AreSame(ex, err);
        }

        [TestMethod]
        public void Merge_TaskWithCompletionSource_Faulted_Sync()
        {
            var tcss = new TaskCompletionSource<int>[3];
            tcss[0] = new TaskCompletionSource<int>();
            tcss[1] = new TaskCompletionSource<int>();
            tcss[2] = new TaskCompletionSource<int>();

            var ex = new Exception();
            tcss[1].SetException(ex);

            var res = Observable.Merge(Observable.Range(0, 3).Select(x => tcss[x].Task));

            var lst = new List<int>();

            var err = default(Exception);
            var done = new ManualResetEvent(false);
            res.Subscribe(lst.Add, ex_ => { err = ex_; done.Set(); }, () => done.Set());

            done.WaitOne();

            lst.AssertEqual(new int[0]);
            Assert.AreSame(ex, err);
        }

        [TestMethod]
        public void Merge_TaskWithCompletionSource_Canceled_Async()
        {
            var tcss = new TaskCompletionSource<int>[3];
            tcss[0] = new TaskCompletionSource<int>();
            tcss[1] = new TaskCompletionSource<int>();
            tcss[2] = new TaskCompletionSource<int>();

            var res = Observable.Merge(Observable.Range(0, 3).Select(x => tcss[x].Task));

            var lst = new List<int>();

            var err = default(Exception);
            var done = new ManualResetEvent(false);
            res.Subscribe(lst.Add, ex_ => { err = ex_; done.Set(); }, () => done.Set());

            tcss[1].SetCanceled();

            done.WaitOne();

            lst.AssertEqual(new int[0]);
            Assert.IsTrue(err is TaskCanceledException && ((TaskCanceledException)err).Task == tcss[1].Task);
        }

        [TestMethod]
        public void Merge_TaskWithCompletionSource_Canceled_Sync()
        {
            var tcss = new TaskCompletionSource<int>[3];
            tcss[0] = new TaskCompletionSource<int>();
            tcss[1] = new TaskCompletionSource<int>();
            tcss[2] = new TaskCompletionSource<int>();

            tcss[1].SetCanceled();

            var res = Observable.Merge(Observable.Range(0, 3).Select(x => tcss[x].Task));

            var lst = new List<int>();

            var err = default(Exception);
            var done = new ManualResetEvent(false);
            res.Subscribe(lst.Add, ex_ => { err = ex_; done.Set(); }, () => done.Set());

            done.WaitOne();

            lst.AssertEqual(new int[0]);
            Assert.IsTrue(err is TaskCanceledException && ((TaskCanceledException)err).Task == tcss[1].Task);
        }

        [TestMethod]
        public void Merge_TaskWithCompletionSource_InnerCompleteBeforeOuter()
        {
            var xs = new Subject<int>();

            var tcss = new TaskCompletionSource<int>[3];
            tcss[0] = new TaskCompletionSource<int>();
            tcss[1] = new TaskCompletionSource<int>();
            tcss[2] = new TaskCompletionSource<int>();

            var res = Observable.Merge(xs.Select(x => tcss[x].Task));

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

        [TestMethod]
        public void Merge_TaskWithCompletionSource_OuterCompleteBeforeInner()
        {
            var xs = new Subject<int>();

            var tcss = new TaskCompletionSource<int>[3];
            tcss[0] = new TaskCompletionSource<int>();
            tcss[1] = new TaskCompletionSource<int>();
            tcss[2] = new TaskCompletionSource<int>();

            var res = Observable.Merge(xs.Select(x => tcss[x].Task));

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

        [TestMethod]
        public void Merge_Task_OnError()
        {
            var xs = new Subject<int>();

            var tcss = new TaskCompletionSource<int>[3];
            tcss[0] = new TaskCompletionSource<int>();
            tcss[1] = new TaskCompletionSource<int>();
            tcss[2] = new TaskCompletionSource<int>();

            var res = Observable.Merge(xs.Select(x => tcss[x].Task));

            var lst = new List<int>();

            var err = default(Exception);
            var done = new ManualResetEvent(false);
            res.Subscribe(lst.Add, ex_ => { err = ex_; done.Set(); }, () => done.Set());

            tcss[1].SetResult(42);

            xs.OnNext(0);
            xs.OnNext(1);
            xs.OnNext(2);

            tcss[0].SetResult(43);
            tcss[2].SetResult(44);

            var ex = new Exception();
            xs.OnError(ex);

            done.WaitOne();

            Assert.AreSame(ex, err);
        }
#endif

        #endregion

        #region + OnErrorResumeNext +

        [TestMethod]
        public void OnErrorResumeNext_ArgumentChecking()
        {
            var xs = DummyObservable<int>.Instance;

            ReactiveAssert.Throws<ArgumentNullException>(() => Observable.OnErrorResumeNext<int>((IObservable<int>[])null));
            ReactiveAssert.Throws<ArgumentNullException>(() => Observable.OnErrorResumeNext<int>((IEnumerable<IObservable<int>>)null));
            ReactiveAssert.Throws<ArgumentNullException>(() => Observable.OnErrorResumeNext<int>((IObservable<int>)null, xs));
            ReactiveAssert.Throws<ArgumentNullException>(() => Observable.OnErrorResumeNext<int>(xs, (IObservable<int>)null));
            ReactiveAssert.Throws<ArgumentNullException>(() => Observable.OnErrorResumeNext<int>(null, xs));
            ReactiveAssert.Throws<ArgumentNullException>(() => Observable.OnErrorResumeNext<int>(xs, null));
        }

        [TestMethod]
        public void OnErrorResumeNext_IEofIO_GetEnumeratorThrows()
        {
            var ex = new Exception();

            var scheduler = new TestScheduler();

            var xss = new RogueEnumerable<IObservable<int>>(ex);

            var res = scheduler.Start(() =>
                Observable.OnErrorResumeNext(xss)
            );

            res.Messages.AssertEqual(
                OnError<int>(200, ex)
            );
        }

        [TestMethod]
        public void OnErrorResumeNext_IEofIO()
        {
            var scheduler = new TestScheduler();

            var xs1 = scheduler.CreateColdObservable<int>(
                OnNext(10, 1),
                OnNext(20, 2),
                OnNext(30, 3),
                OnCompleted<int>(40)
            );

            var xs2 = scheduler.CreateColdObservable<int>(
                OnNext(10, 4),
                OnNext(20, 5),
                OnError<int>(30, new Exception())
            );

            var xs3 = scheduler.CreateColdObservable<int>(
                OnNext(10, 6),
                OnNext(20, 7),
                OnNext(30, 8),
                OnNext(40, 9),
                OnCompleted<int>(50)
            );

            var res = scheduler.Start(() =>
                Observable.OnErrorResumeNext(new[] { xs1, xs2, xs3 })
            );

            res.Messages.AssertEqual(
                OnNext(210, 1),
                OnNext(220, 2),
                OnNext(230, 3),
                OnNext(250, 4),
                OnNext(260, 5),
                OnNext(280, 6),
                OnNext(290, 7),
                OnNext(300, 8),
                OnNext(310, 9),
                OnCompleted<int>(320)
            );

            xs1.Subscriptions.AssertEqual(
                Subscribe(200, 240)
            );

            xs2.Subscriptions.AssertEqual(
                Subscribe(240, 270)
            );

            xs3.Subscriptions.AssertEqual(
                Subscribe(270, 320)
            );
        }

        [TestMethod]
        public void OnErrorResumeNext_NoErrors()
        {
            var scheduler = new TestScheduler();

            var o1 = scheduler.CreateHotObservable(
                OnNext(150, 1),
                OnNext(210, 2),
                OnNext(220, 3),
                OnCompleted<int>(230)
            );

            var o2 = scheduler.CreateHotObservable(
                OnNext(240, 4),
                OnCompleted<int>(250)
            );

            var res = scheduler.Start(() =>
                o1.OnErrorResumeNext(o2)
            );

            res.Messages.AssertEqual(
                OnNext(210, 2),
                OnNext(220, 3),
                OnNext(240, 4),
                OnCompleted<int>(250)
            );

            o1.Subscriptions.AssertEqual(
                Subscribe(200, 230)
            );

            o2.Subscriptions.AssertEqual(
                Subscribe(230, 250)
            );
        }

        [TestMethod]
        public void OnErrorResumeNext_Error()
        {
            var scheduler = new TestScheduler();

            var o1 = scheduler.CreateHotObservable(
                OnNext(150, 1),
                OnNext(210, 2),
                OnNext(220, 3),
                OnError<int>(230, new Exception())
            );

            var o2 = scheduler.CreateHotObservable(
                OnNext(240, 4),
                OnCompleted<int>(250)
            );

            var res = scheduler.Start(() =>
                o1.OnErrorResumeNext(o2)
            );

            res.Messages.AssertEqual(
                OnNext(210, 2),
                OnNext(220, 3),
                OnNext(240, 4),
                OnCompleted<int>(250)
            );

            o1.Subscriptions.AssertEqual(
                Subscribe(200, 230)
            );

            o2.Subscriptions.AssertEqual(
                Subscribe(230, 250)
            );
        }

        [TestMethod]
        public void OnErrorResumeNext_ErrorMultiple()
        {
            var scheduler = new TestScheduler();

            var o1 = scheduler.CreateHotObservable(
                OnNext(150, 1),
                OnNext(210, 2),
                OnError<int>(220, new Exception())
            );

            var o2 = scheduler.CreateHotObservable(
                OnNext(230, 3),
                OnError<int>(240, new Exception())
            );

            var o3 = scheduler.CreateHotObservable(
                OnCompleted<int>(250)
            );

            var res = scheduler.Start(() =>
                Observable.OnErrorResumeNext(o1, o2, o3)
            );

            res.Messages.AssertEqual(
                OnNext(210, 2),
                OnNext(230, 3),
                OnCompleted<int>(250)
            );

            o1.Subscriptions.AssertEqual(
                Subscribe(200, 220)
            );

            o2.Subscriptions.AssertEqual(
                Subscribe(220, 240)
            );

            o3.Subscriptions.AssertEqual(
                Subscribe(240, 250)
            );
        }

        [TestMethod]
        public void OnErrorResumeNext_EmptyReturnThrowAndMore()
        {
            var scheduler = new TestScheduler();

            var o1 = scheduler.CreateHotObservable(
                OnNext(150, 1),
                OnCompleted<int>(205)
            );

            var o2 = scheduler.CreateHotObservable(
                OnNext(215, 2),
                OnCompleted<int>(220)
            );

            var o3 = scheduler.CreateHotObservable(
                OnNext(225, 3),
                OnNext(230, 4),
                OnCompleted<int>(235)
            );

            var o4 = scheduler.CreateHotObservable(
                OnError<int>(240, new Exception())
            );

            var o5 = scheduler.CreateHotObservable(
                OnNext<int>(245, 5),
                OnCompleted<int>(250)
            );

            var res = scheduler.Start(() =>
                new[] { o1, o2, o3, o4, o5 }.OnErrorResumeNext()
            );

            res.Messages.AssertEqual(
                OnNext(215, 2),
                OnNext(225, 3),
                OnNext(230, 4),
                OnNext(245, 5),
                OnCompleted<int>(250)
            );

            o1.Subscriptions.AssertEqual(
                Subscribe(200, 205)
            );

            o2.Subscriptions.AssertEqual(
                Subscribe(205, 220)
            );

            o3.Subscriptions.AssertEqual(
                Subscribe(220, 235)
            );

            o4.Subscriptions.AssertEqual(
                Subscribe(235, 240)
            );

            o5.Subscriptions.AssertEqual(
                Subscribe(240, 250)
            );
        }

        [TestMethod]
        public void OnErrorResumeNext_LastIsntSpecial()
        {
            var scheduler = new TestScheduler();

            var ex = new Exception();

            var o1 = scheduler.CreateHotObservable(
                OnNext(150, 1),
                OnNext(210, 2),
                OnCompleted<int>(220)
            );

            var o2 = scheduler.CreateHotObservable(
                OnError<int>(230, ex)
            );

            var res = scheduler.Start(() =>
                o1.OnErrorResumeNext(o2)
            );

            res.Messages.AssertEqual(
                OnNext(210, 2),
                OnCompleted<int>(230)
            );

            o1.Subscriptions.AssertEqual(
                Subscribe(200, 220)
            );

            o2.Subscriptions.AssertEqual(
                Subscribe(220, 230)
            );
        }

        [TestMethod]
        public void OnErrorResumeNext_SingleSourceDoesntThrow()
        {
            var scheduler = new TestScheduler();

            var ex = new Exception();

            var o1 = scheduler.CreateHotObservable(
                OnError<int>(230, ex)
            );

            var res = scheduler.Start(() =>
                Observable.OnErrorResumeNext(o1)
            );

            res.Messages.AssertEqual(
                OnCompleted<int>(230)
            );

            o1.Subscriptions.AssertEqual(
                Subscribe(200, 230)
            );
        }

        [TestMethod]
        public void OnErrorResumeNext_EndWithNever()
        {
            var scheduler = new TestScheduler();

            var ex = new Exception();

            var o1 = scheduler.CreateHotObservable(
                OnNext(150, 1),
                OnNext(210, 2),
                OnCompleted<int>(220)
            );

            var o2 = scheduler.CreateHotObservable(
                OnNext(150, 1)
            );

            var res = scheduler.Start(() =>
                Observable.OnErrorResumeNext(o1, o2)
            );

            res.Messages.AssertEqual(
                OnNext(210, 2)
            );

            o1.Subscriptions.AssertEqual(
                Subscribe(200, 220)
            );

            o2.Subscriptions.AssertEqual(
                Subscribe(220, 1000)
            );
        }

        [TestMethod]
        public void OnErrorResumeNext_StartWithNever()
        {
            var scheduler = new TestScheduler();

            var ex = new Exception();

            var o1 = scheduler.CreateHotObservable(
                OnNext(150, 1)
            );

            var o2 = scheduler.CreateHotObservable(
                OnNext(150, 1),
                OnNext(210, 2),
                OnCompleted<int>(220)
            );

            var res = scheduler.Start(() =>
                Observable.OnErrorResumeNext(o1, o2)
            );

            res.Messages.AssertEqual(
            );

            o1.Subscriptions.AssertEqual(
                Subscribe(200, 1000)
            );

            o2.Subscriptions.AssertEqual(
            );
        }

        [TestMethod]
        public void OnErrorResumeNext_DefaultScheduler_Binary()
        {
            var evt = new ManualResetEvent(false);

            int sum = 0;
            Observable.Return(1).OnErrorResumeNext(Observable.Return(2)).Subscribe(x =>
            {
                sum += x;
            }, () => evt.Set());

            evt.WaitOne();
            Assert.AreEqual(3, sum);
        }

        [TestMethod]
        public void OnErrorResumeNext_DefaultScheduler_Nary()
        {
            var evt = new ManualResetEvent(false);

            int sum = 0;
            Observable.OnErrorResumeNext(Observable.Return(1), Observable.Return(2), Observable.Return(3)).Subscribe(x =>
            {
                sum += x;
            }, () => evt.Set());

            evt.WaitOne();
            Assert.AreEqual(6, sum);
        }

        [TestMethod]
        public void OnErrorResumeNext_DefaultScheduler_NaryEnumerable()
        {
            var evt = new ManualResetEvent(false);

            IEnumerable<IObservable<int>> sources = new[] { Observable.Return(1), Observable.Return(2), Observable.Return(3) };

            int sum = 0;
            Observable.OnErrorResumeNext(sources).Subscribe(x =>
            {
                sum += x;
            }, () => evt.Set());

            evt.WaitOne();
            Assert.AreEqual(6, sum);
        }

        [TestMethod]
        public void OnErrorResumeNext_IteratorThrows()
        {
            var scheduler = new TestScheduler();
            var ex = new Exception();

            var res = scheduler.Start(() =>
                Observable.OnErrorResumeNext<int>(Catch_IteratorThrows_Source(ex, true))
            );

            res.Messages.AssertEqual(
                OnError<int>(200, ex)
            );
        }

        [TestMethod]
        public void OnErrorResumeNext_EnumerableThrows()
        {
            var scheduler = new TestScheduler();

            var o = scheduler.CreateHotObservable(
                OnNext(150, 1),
                OnNext(210, 2),
                OnNext(220, 3),
                OnError<int>(225, new Exception())
            );

            var ex = new Exception();
            var xss = new MockEnumerable<IObservable<int>>(scheduler, GetObservablesForOnErrorResumeNextThrow(o, ex));

            var res = scheduler.Start(() =>
                xss.OnErrorResumeNext()
            );

            res.Messages.AssertEqual(
                OnNext(210, 2),
                OnNext(220, 3),
                OnError<int>(225, ex)
            );

            o.Subscriptions.AssertEqual(
                Subscribe(200, 225)
            );

            xss.Subscriptions.AssertEqual(
                Subscribe(200, 225)
            );
        }

        private IEnumerable<IObservable<int>> GetObservablesForOnErrorResumeNextThrow(IObservable<int> first, Exception ex)
        {
            yield return first;
            throw ex;
        }

        [TestMethod]
        public void OnErrorResumeNext_EnumerableTiming()
        {
            var scheduler = new TestScheduler();

            var o1 = scheduler.CreateHotObservable(
                OnNext(150, 1),
                OnNext(210, 2), // !
                OnNext(220, 3), // !
                OnCompleted<int>(230)
            );

            var o2 = scheduler.CreateColdObservable(
                OnNext(50, 4),  // !
                OnNext(60, 5),  // !
                OnNext(70, 6),  // !
                OnError<int>(80, new Exception())
            );

            var o3 = scheduler.CreateHotObservable(
                OnNext(150, 1),
                OnNext(200, 2),
                OnNext(210, 3),
                OnNext(220, 4),
                OnNext(230, 5),
                OnNext(270, 6),
                OnNext(320, 7), // !
                OnNext(330, 8), // !
                OnCompleted<int>(340)
            );

            var xss = new MockEnumerable<ITestableObservable<int>>(scheduler, new[] { o1, o2, o3, o2 });

            var res = scheduler.Start(() =>
                xss.Select(xs => (IObservable<int>)xs).OnErrorResumeNext()
            );

            res.Messages.AssertEqual(
                OnNext(210, 2),
                OnNext(220, 3),
                OnNext(280, 4),
                OnNext(290, 5),
                OnNext(300, 6),
                OnNext(320, 7),
                OnNext(330, 8),
                OnNext(390, 4),
                OnNext(400, 5),
                OnNext(410, 6),
                OnCompleted<int>(420)
            );

            o1.Subscriptions.AssertEqual(
                Subscribe(200, 230)
            );

            o2.Subscriptions.AssertEqual(
                Subscribe(230, 310),
                Subscribe(340, 420)
            );

            o3.Subscriptions.AssertEqual(
                Subscribe(310, 340)
            );

            xss.Subscriptions.AssertEqual(
                Subscribe(200, 420)
            );
        }

        [TestMethod]
        public void OnErrorResumeNext_Enumerable_Dispose()
        {
            var scheduler = new TestScheduler();

            var o1 = scheduler.CreateHotObservable(
                OnNext(150, 1),
                OnNext(210, 2),
                OnNext(220, 3),
                OnError<int>(230, new Exception())
            );

            var o2 = scheduler.CreateHotObservable(
                OnNext(150, 1),
                OnNext(200, 2),
                OnNext(210, 3),
                OnNext(240, 4),
                OnNext(270, 5),
                OnNext(320, 6),
                OnNext(330, 7),
                OnCompleted<int>(340)
            );

            var xss = new MockEnumerable<ITestableObservable<int>>(scheduler, new[] { o1, o2 });

            var res = scheduler.Start(() =>
                xss.Select(xs => (IObservable<int>)xs).OnErrorResumeNext(),
                300
            );

            res.Messages.AssertEqual(
                OnNext(210, 2),
                OnNext(220, 3),
                OnNext(240, 4),
                OnNext(270, 5)
            );

            o1.Subscriptions.AssertEqual(
                Subscribe(200, 230)
            );

            o2.Subscriptions.AssertEqual(
                Subscribe(230, 300)
            );

            xss.Subscriptions.AssertEqual(
                Subscribe(200, 300)
            );
        }

#if !NO_PERF
        [TestMethod]
        public void OnErrorResumeNext_TailRecursive1()
        {
            var create = 0L;
            var start = 200L;
            var end = 1000L;

            var scheduler = new TestScheduler();

            var o = scheduler.CreateColdObservable<int>(
                OnNext(10, 1),
                OnNext(20, 2),
                OnNext(30, 3),
                OnError<int>(40, new Exception())
            );

            var f = default(Func<IObservable<int>>);
            f = () => Observable.Defer(() => o.OnErrorResumeNext(f()));

            var res = scheduler.Start(() => f(), create, start, end);

            var expected = new List<Recorded<Notification<int>>>();

            var t = start;
            while (t <= end)
            {
                var n = (t - start) / 10;
                if (n % 4 != 0)
                {
                    expected.Add(OnNext(t, (int)(n % 4)));
                }

                t += 10;
            }

            res.Messages.AssertEqual(expected);
        }

        [TestMethod]
        public void OnErrorResumeNext_TailRecursive2()
        {
            var f = default(Func<int, IObservable<int>>);
            f = x => Observable.Defer(() => Observable.Throw<int>(new Exception(), ThreadPoolScheduler.Instance).StartWith(x).OnErrorResumeNext(f(x + 1)));

            var lst = new List<int>();
            f(0).Select(x => new StackTrace().FrameCount).Take(10).ForEach(lst.Add);

            Assert.IsTrue(lst.Last() - lst.First() < 10);
        }
#endif

        #endregion

        #region + SkipUntil +

        [TestMethod]
        public void SkipUntil_ArgumentChecking()
        {
            ReactiveAssert.Throws<ArgumentNullException>(() => Observable.SkipUntil<int, int>(null, DummyObservable<int>.Instance));
            ReactiveAssert.Throws<ArgumentNullException>(() => Observable.SkipUntil<int, int>(DummyObservable<int>.Instance, null));
        }

        [TestMethod]
        public void SkipUntil_SomeData_Next()
        {
            var scheduler = new TestScheduler();

            var l = scheduler.CreateHotObservable(
                OnNext(150, 1),
                OnNext(210, 2),
                OnNext(220, 3),
                OnNext(230, 4), //!
                OnNext(240, 5), //!
                OnCompleted<int>(250)
            );

            var r = scheduler.CreateHotObservable(
                OnNext(150, 1),
                OnNext(225, 99),
                OnCompleted<int>(230)
            );

            var res = scheduler.Start(() =>
                l.SkipUntil(r)
            );

            res.Messages.AssertEqual(
                OnNext(230, 4),
                OnNext(240, 5),
                OnCompleted<int>(250)
            );

            l.Subscriptions.AssertEqual(
                Subscribe(200, 250)
            );

            r.Subscriptions.AssertEqual(
                Subscribe(200, 225)
            );
        }

        [TestMethod]
        public void SkipUntil_SomeData_Error()
        {
            var scheduler = new TestScheduler();

            var ex = new Exception();

            var l = scheduler.CreateHotObservable(
                OnNext(150, 1),
                OnNext(210, 2),
                OnNext(220, 3),
                OnNext(230, 4),
                OnNext(240, 5),
                OnCompleted<int>(250)
            );

            var r = scheduler.CreateHotObservable(
                OnNext(150, 1),
                OnError<int>(225, ex)
            );

            var res = scheduler.Start(() =>
                l.SkipUntil(r)
            );

            res.Messages.AssertEqual(
                OnError<int>(225, ex)
            );

            l.Subscriptions.AssertEqual(
                Subscribe(200, 225)
            );

            r.Subscriptions.AssertEqual(
                Subscribe(200, 225)
            );
        }

        [TestMethod]
        public void SkipUntil_Error_SomeData()
        {
            var scheduler = new TestScheduler();

            var ex = new Exception();

            var l = scheduler.CreateHotObservable(
                OnNext(150, 1),
                OnNext(210, 2),
                OnError<int>(220, ex)
            );

            var r = scheduler.CreateHotObservable(
                OnNext(150, 1),
                OnNext(230, 2),
                OnCompleted<int>(250)
            );

            var res = scheduler.Start(() =>
                l.SkipUntil(r)
            );

            res.Messages.AssertEqual(
                OnError<int>(220, ex)
            );

            l.Subscriptions.AssertEqual(
                Subscribe(200, 220)
            );

            r.Subscriptions.AssertEqual(
                Subscribe(200, 220)
            );
        }

        [TestMethod]
        public void SkipUntil_SomeData_Empty()
        {
            var scheduler = new TestScheduler();

            var l = scheduler.CreateHotObservable(
                OnNext(150, 1),
                OnNext(210, 2),
                OnNext(220, 3),
                OnNext(230, 4),
                OnNext(240, 5),
                OnCompleted<int>(250)
            );

            var r = scheduler.CreateHotObservable(
                OnNext(150, 1),
                OnCompleted<int>(225)
            );

            var res = scheduler.Start(() =>
                l.SkipUntil(r)
            );

            res.Messages.AssertEqual(
            );

            l.Subscriptions.AssertEqual(
                Subscribe(200, 250)
            );

            r.Subscriptions.AssertEqual(
                Subscribe(200, 225)
            );
        }

        [TestMethod]
        public void SkipUntil_Never_Next()
        {
            var scheduler = new TestScheduler();

            var l = scheduler.CreateHotObservable(
                OnNext(150, 1)
            );

            var r = scheduler.CreateHotObservable(
                OnNext(150, 1),
                OnNext(225, 2), //!
                OnCompleted<int>(250)
            );

            var res = scheduler.Start(() =>
                l.SkipUntil(r)
            );

            res.Messages.AssertEqual(
            );

            l.Subscriptions.AssertEqual(
                Subscribe(200, 1000)
            );

            r.Subscriptions.AssertEqual(
                Subscribe(200, 225)
            );
        }

        [TestMethod]
        public void SkipUntil_Never_Error1()
        {
            var scheduler = new TestScheduler();

            var ex = new Exception();

            var l = scheduler.CreateHotObservable(
                OnNext(150, 1)
            );

            var r = scheduler.CreateHotObservable(
                OnNext(150, 1),
                OnError<int>(225, ex)
            );

            var res = scheduler.Start(() =>
                l.SkipUntil(r)
            );

            res.Messages.AssertEqual(
                OnError<int>(225, ex)
            );

            l.Subscriptions.AssertEqual(
                Subscribe(200, 225)
            );

            r.Subscriptions.AssertEqual(
                Subscribe(200, 225)
            );
        }

        [TestMethod]
        public void SkipUntil_SomeData_Error2()
        {
            var scheduler = new TestScheduler();

            var ex = new Exception();

            var l = scheduler.CreateHotObservable(
                OnNext(150, 1),
                OnNext(210, 2),
                OnNext(220, 3),
                OnNext(230, 4),
                OnNext(240, 5),
                OnCompleted<int>(250)
            );

            var r = scheduler.CreateHotObservable(
                OnNext(150, 1),
                OnError<int>(300, ex)
            );

            var res = scheduler.Start(() =>
                l.SkipUntil(r)
            );

            res.Messages.AssertEqual(
                OnError<int>(300, ex)
            );

            l.Subscriptions.AssertEqual(
                Subscribe(200, 250)
            );

            r.Subscriptions.AssertEqual(
                Subscribe(200, 300)
            );
        }

        [TestMethod]
        public void SkipUntil_SomeData_Never()
        {
            var scheduler = new TestScheduler();

            var l = scheduler.CreateHotObservable(
                OnNext(150, 1),
                OnNext(210, 2),
                OnNext(220, 3),
                OnNext(230, 4),
                OnNext(240, 5),
                OnCompleted<int>(250)
            );

            var r = scheduler.CreateHotObservable(
                OnNext(150, 1)
            );

            var res = scheduler.Start(() =>
                l.SkipUntil(r)
            );

            res.Messages.AssertEqual(
            );

            l.Subscriptions.AssertEqual(
                Subscribe(200, 250)
            );

            r.Subscriptions.AssertEqual(
                Subscribe(200, 1000 /* can't dispose prematurely, could be in flight to dispatch OnError */)
            );
        }

        [TestMethod]
        public void SkipUntil_Never_Empty()
        {
            var scheduler = new TestScheduler();

            var l = scheduler.CreateHotObservable(
                OnNext(150, 1)
            );

            var r = scheduler.CreateHotObservable(
                OnNext(150, 1),
                OnCompleted<int>(225)
            );

            var res = scheduler.Start(() =>
                l.SkipUntil(r)
            );

            res.Messages.AssertEqual(
            );

            l.Subscriptions.AssertEqual(
                Subscribe(200, 1000)
            );

            r.Subscriptions.AssertEqual(
                Subscribe(200, 225)
            );
        }

        [TestMethod]
        public void SkipUntil_Never_Never()
        {
            var scheduler = new TestScheduler();

            var l = scheduler.CreateHotObservable(
                OnNext(150, 1)
            );

            var r = scheduler.CreateHotObservable(
                OnNext(150, 1)
            );

            var res = scheduler.Start(() =>
                l.SkipUntil(r)
            );

            res.Messages.AssertEqual(
            );

            l.Subscriptions.AssertEqual(
                Subscribe(200, 1000)
            );

            r.Subscriptions.AssertEqual(
                Subscribe(200, 1000)
            );
        }

        [TestMethod]
        public void SkipUntil_HasCompletedCausesDisposal()
        {
            var scheduler = new TestScheduler();

            bool disposed = false;

            var l = scheduler.CreateHotObservable(
                OnNext(150, 1),
                OnNext(210, 2),
                OnNext(220, 3),
                OnNext(230, 4),
                OnNext(240, 5),
                OnCompleted<int>(250)
            );

            var r = Observable.Create<int>(obs => () => { disposed = true; });

            var res = scheduler.Start(() =>
                l.SkipUntil(r)
            );
            
            res.Messages.AssertEqual(
            );

            Assert.IsTrue(disposed, "disposed");
        }

        #endregion

        #region + Switch +

        [TestMethod]
        public void Switch_ArgumentChecking()
        {
            ReactiveAssert.Throws<ArgumentNullException>(() => Observable.Switch((IObservable<IObservable<int>>)null));

#if !NO_TPL
            ReactiveAssert.Throws<ArgumentNullException>(() => Observable.Switch((IObservable<System.Threading.Tasks.Task<int>>)null));
#endif
        }

        [TestMethod]
        public void Switch_Data()
        {
            var scheduler = new TestScheduler();

            var ys1 = scheduler.CreateColdObservable(
                OnNext(10, 101),
                OnNext(20, 102),
                OnNext(110, 103),
                OnNext(120, 104),
                OnNext(210, 105),
                OnNext(220, 106),
                OnCompleted<int>(230)
            );

            var ys2 = scheduler.CreateColdObservable(
                OnNext(10, 201),
                OnNext(20, 202),
                OnNext(30, 203),
                OnNext(40, 204),
                OnCompleted<int>(50)
            );

            var ys3 = scheduler.CreateColdObservable(
                OnNext(10, 301),
                OnNext(20, 302),
                OnNext(30, 303),
                OnNext(40, 304),
                OnCompleted<int>(150)
            );

            var xs = scheduler.CreateHotObservable(
                OnNext<IObservable<int>>(300, ys1),
                OnNext<IObservable<int>>(400, ys2),
                OnNext<IObservable<int>>(500, ys3),
                OnCompleted<IObservable<int>>(600)
            );

            var res = scheduler.Start(() =>
                xs.Switch()
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
                OnCompleted<int>(650)
            );

#if !NO_PERF
            // BREAKING CHANGE v2 > v1.x -> More aggressive disposal behavior
            xs.Subscriptions.AssertEqual(
                Subscribe(200, 600)
            );
#else
            xs.Subscriptions.AssertEqual(
                Subscribe(200, 650)
            );
#endif

            ys1.Subscriptions.AssertEqual(
                Subscribe(300, 400)
            );

#if !NO_PERF
            // BREAKING CHANGE v2 > v1.x -> More aggressive disposal behavior
            ys2.Subscriptions.AssertEqual(
                Subscribe(400, 450)
            );
#else
            ys2.Subscriptions.AssertEqual(
                Subscribe(400, 500)
            );
#endif

            ys3.Subscriptions.AssertEqual(
                Subscribe(500, 650)
            );
        }

        [TestMethod]
        public void Switch_InnerThrows()
        {
            var scheduler = new TestScheduler();

            var ex = new Exception();

            var ys1 = scheduler.CreateColdObservable(
                OnNext(10, 101),
                OnNext(20, 102),
                OnNext(110, 103),
                OnNext(120, 104),
                OnNext(210, 105),
                OnNext(220, 106),
                OnCompleted<int>(230)
            );

            var ys2 = scheduler.CreateColdObservable(
                OnNext(10, 201),
                OnNext(20, 202),
                OnNext(30, 203),
                OnNext(40, 204),
                OnError<int>(50, ex)
            );

            var ys3 = scheduler.CreateColdObservable(
                OnNext(10, 301),
                OnNext(20, 302),
                OnNext(30, 303),
                OnNext(40, 304),
                OnCompleted<int>(150)
            );

            var xs = scheduler.CreateHotObservable(
                OnNext<IObservable<int>>(300, ys1),
                OnNext<IObservable<int>>(400, ys2),
                OnNext<IObservable<int>>(500, ys3),
                OnCompleted<IObservable<int>>(600)
            );

            var res = scheduler.Start(() =>
                xs.Switch()
            );

            res.Messages.AssertEqual(
                OnNext(310, 101),
                OnNext(320, 102),
                OnNext(410, 201),
                OnNext(420, 202),
                OnNext(430, 203),
                OnNext(440, 204),
                OnError<int>(450, ex)
            );

            xs.Subscriptions.AssertEqual(
                Subscribe(200, 450)
            );

            ys1.Subscriptions.AssertEqual(
                Subscribe(300, 400)
            );

            ys2.Subscriptions.AssertEqual(
                Subscribe(400, 450)
            );

            ys3.Subscriptions.AssertEqual(
            );
        }

        [TestMethod]
        public void Switch_OuterThrows()
        {
            var scheduler = new TestScheduler();

            var ex = new Exception();

            var ys1 = scheduler.CreateColdObservable(
                OnNext(10, 101),
                OnNext(20, 102),
                OnNext(110, 103),
                OnNext(120, 104),
                OnNext(210, 105),
                OnNext(220, 106),
                OnCompleted<int>(230)
            );

            var ys2 = scheduler.CreateColdObservable(
                OnNext(10, 201),
                OnNext(20, 202),
                OnNext(30, 203),
                OnNext(40, 204),
                OnCompleted<int>(50)
            );

            var xs = scheduler.CreateHotObservable(
                OnNext<IObservable<int>>(300, ys1),
                OnNext<IObservable<int>>(400, ys2),
                OnError<IObservable<int>>(500, ex)
            );

            var res = scheduler.Start(() =>
                xs.Switch()
            );

            res.Messages.AssertEqual(
                OnNext(310, 101),
                OnNext(320, 102),
                OnNext(410, 201),
                OnNext(420, 202),
                OnNext(430, 203),
                OnNext(440, 204),
                OnError<int>(500, ex)
            );

            xs.Subscriptions.AssertEqual(
                Subscribe(200, 500)
            );

            ys1.Subscriptions.AssertEqual(
                Subscribe(300, 400)
            );

#if !NO_PERF
            // BREAKING CHANGE v2 > v1.x -> More aggressive disposal behavior
            ys2.Subscriptions.AssertEqual(
                Subscribe(400, 450)
            );
#else
            ys2.Subscriptions.AssertEqual(
                Subscribe(400, 500)
            );
#endif
        }

        [TestMethod]
        public void Switch_NoInner()
        {
            var scheduler = new TestScheduler();

            var xs = scheduler.CreateHotObservable(
                OnCompleted<IObservable<int>>(500)
            );

            var res = scheduler.Start(() =>
                xs.Switch()
            );

            res.Messages.AssertEqual(
                OnCompleted<int>(500)
            );

            xs.Subscriptions.AssertEqual(
                Subscribe(200, 500)
            );
        }

        [TestMethod]
        public void Switch_InnerCompletes()
        {
            var scheduler = new TestScheduler();

            var ys1 = scheduler.CreateColdObservable(
                OnNext(10, 101),
                OnNext(20, 102),
                OnNext(110, 103),
                OnNext(120, 104),
                OnNext(210, 105),
                OnNext(220, 106),
                OnCompleted<int>(230)
            );

            var xs = scheduler.CreateHotObservable(
                OnNext<IObservable<int>>(300, ys1),
                OnCompleted<IObservable<int>>(540)
            );

            var res = scheduler.Start(() =>
                xs.Switch()
            );

            res.Messages.AssertEqual(
                OnNext(310, 101),
                OnNext(320, 102),
                OnNext(410, 103),
                OnNext(420, 104),
                OnNext(510, 105),
                OnNext(520, 106),
                OnCompleted<int>(540)
            );

            xs.Subscriptions.AssertEqual(
                Subscribe(200, 540)
            );

#if !NO_PERF
            // BREAKING CHANGE v2 > v1.x -> More aggressive disposal behavior
            ys1.Subscriptions.AssertEqual(
                Subscribe(300, 530)
            );
#else
            ys1.Subscriptions.AssertEqual(
                Subscribe(300, 540)
            );
#endif
        }

#if !NO_TPL
        [TestMethod]
        public void Switch_Task()
        {
            var tss = Observable.Switch(new[] { Task.Factory.StartNew(() => 1), Task.Factory.StartNew(() => 2), Task.Factory.StartNew(() => 3) }.ToObservable());

            var res = tss.ToArray().Single();

            Assert.IsTrue(res.Zip(res.Skip(1), (l, r) => r > l).All(b => b));
        }
#endif

        #endregion

        #region + TakeUntil +

        [TestMethod]
        public void TakeUntil_ArgumentChecking()
        {
            ReactiveAssert.Throws<ArgumentNullException>(() => Observable.TakeUntil<int, int>(null, DummyObservable<int>.Instance));
            ReactiveAssert.Throws<ArgumentNullException>(() => Observable.TakeUntil<int, int>(DummyObservable<int>.Instance, null));
        }

        [TestMethod]
        public void TakeUntil_Preempt_SomeData_Next()
        {
            var scheduler = new TestScheduler();

            var l = scheduler.CreateHotObservable(
                OnNext(150, 1),
                OnNext(210, 2),
                OnNext(220, 3),
                OnNext(230, 4),
                OnNext(240, 5),
                OnCompleted<int>(250)
            );

            var r = scheduler.CreateHotObservable(
                OnNext(150, 1),
                OnNext(225, 99),
                OnCompleted<int>(230)
            );

            var res = scheduler.Start(() =>
                l.TakeUntil(r)
            );

            res.Messages.AssertEqual(
                OnNext(210, 2),
                OnNext(220, 3),
                OnCompleted<int>(225)
            );

            l.Subscriptions.AssertEqual(
                Subscribe(200, 225)
            );

            r.Subscriptions.AssertEqual(
                Subscribe(200, 225)
            );
        }

        [TestMethod]
        public void TakeUntil_Preempt_SomeData_Error()
        {
            var scheduler = new TestScheduler();

            var ex = new Exception();

            var l = scheduler.CreateHotObservable(
                OnNext(150, 1),
                OnNext(210, 2),
                OnNext(220, 3),
                OnNext(230, 4),
                OnNext(240, 5),
                OnCompleted<int>(250)
            );

            var r = scheduler.CreateHotObservable(
                OnNext(150, 1),
                OnError<int>(225, ex)
            );

            var res = scheduler.Start(() =>
                l.TakeUntil(r)
            );

            res.Messages.AssertEqual(
                OnNext(210, 2),
                OnNext(220, 3),
                OnError<int>(225, ex)
            );

            l.Subscriptions.AssertEqual(
                Subscribe(200, 225)
            );

            r.Subscriptions.AssertEqual(
                Subscribe(200, 225)
            );
        }

        [TestMethod]
        public void TakeUntil_NoPreempt_SomeData_Empty()
        {
            var scheduler = new TestScheduler();

            var l = scheduler.CreateHotObservable(
                OnNext(150, 1),
                OnNext(210, 2),
                OnNext(220, 3),
                OnNext(230, 4),
                OnNext(240, 5),
                OnCompleted<int>(250)
            );

            var r = scheduler.CreateHotObservable(
                OnNext(150, 1),
                OnCompleted<int>(225)
            );

            var res = scheduler.Start(() =>
                l.TakeUntil(r)
            );

            res.Messages.AssertEqual(
                OnNext(210, 2),
                OnNext(220, 3),
                OnNext(230, 4),
                OnNext(240, 5),
                OnCompleted<int>(250)
            );

            l.Subscriptions.AssertEqual(
                Subscribe(200, 250)
            );

            r.Subscriptions.AssertEqual(
                Subscribe(200, 225)
            );
        }

        [TestMethod]
        public void TakeUntil_NoPreempt_SomeData_Never()
        {
            var scheduler = new TestScheduler();

            var l = scheduler.CreateHotObservable(
                OnNext(150, 1),
                OnNext(210, 2),
                OnNext(220, 3),
                OnNext(230, 4),
                OnNext(240, 5),
                OnCompleted<int>(250)
            );

            var r = scheduler.CreateHotObservable(
                OnNext(150, 1)
            );

            var res = scheduler.Start(() =>
                l.TakeUntil(r)
            );

            res.Messages.AssertEqual(
                OnNext(210, 2),
                OnNext(220, 3),
                OnNext(230, 4),
                OnNext(240, 5),
                OnCompleted<int>(250)
            );

            l.Subscriptions.AssertEqual(
                Subscribe(200, 250)
            );

            r.Subscriptions.AssertEqual(
                Subscribe(200, 250)
            );
        }

        [TestMethod]
        public void TakeUntil_Preempt_Never_Next()
        {
            var scheduler = new TestScheduler();

            var l = scheduler.CreateHotObservable(
                OnNext(150, 1)
            );

            var r = scheduler.CreateHotObservable(
                OnNext(150, 1),
                OnNext(225, 2), //!
                OnCompleted<int>(250)
            );

            var res = scheduler.Start(() =>
                l.TakeUntil(r)
            );

            res.Messages.AssertEqual(
                OnCompleted<int>(225)
            );

            l.Subscriptions.AssertEqual(
                Subscribe(200, 225)
            );

            r.Subscriptions.AssertEqual(
                Subscribe(200, 225)
            );
        }

        [TestMethod]
        public void TakeUntil_Preempt_Never_Error()
        {
            var scheduler = new TestScheduler();

            var ex = new Exception();

            var l = scheduler.CreateHotObservable(
                OnNext(150, 1)
            );

            var r = scheduler.CreateHotObservable(
                OnNext(150, 1),
                OnError<int>(225, ex)
            );

            var res = scheduler.Start(() =>
                l.TakeUntil(r)
            );

            res.Messages.AssertEqual(
                OnError<int>(225, ex)
            );

            l.Subscriptions.AssertEqual(
                Subscribe(200, 225)
            );

            r.Subscriptions.AssertEqual(
                Subscribe(200, 225)
            );
        }

        [TestMethod]
        public void TakeUntil_NoPreempt_Never_Empty()
        {
            var scheduler = new TestScheduler();

            var l = scheduler.CreateHotObservable(
                OnNext(150, 1)
            );

            var r = scheduler.CreateHotObservable(
                OnNext(150, 1),
                OnCompleted<int>(225)
            );

            var res = scheduler.Start(() =>
                l.TakeUntil(r)
            );

            res.Messages.AssertEqual(
            );

            l.Subscriptions.AssertEqual(
                Subscribe(200, 1000 /* can't dispose prematurely, could be in flight to dispatch OnError */)
            );

            r.Subscriptions.AssertEqual(
                Subscribe(200, 225)
            );
        }

        [TestMethod]
        public void TakeUntil_NoPreempt_Never_Never()
        {
            var scheduler = new TestScheduler();

            var l = scheduler.CreateHotObservable(
                OnNext(150, 1)
            );

            var r = scheduler.CreateHotObservable(
                OnNext(150, 1)
            );

            var res = scheduler.Start(() =>
                l.TakeUntil(r)
            );

            res.Messages.AssertEqual(
            );

            l.Subscriptions.AssertEqual(
                Subscribe(200, 1000)
            );

            r.Subscriptions.AssertEqual(
                Subscribe(200, 1000)
            );
        }

        [TestMethod]
        public void TakeUntil_Preempt_BeforeFirstProduced()
        {
            var scheduler = new TestScheduler();

            var l = scheduler.CreateHotObservable(
                OnNext(150, 1),
                OnNext(230, 2),
                OnCompleted<int>(240)
            );

            var r = scheduler.CreateHotObservable(
                OnNext(150, 1),
                OnNext(210, 2), //!
                OnCompleted<int>(220)
            );

            var res = scheduler.Start(() =>
                l.TakeUntil(r)
            );

            res.Messages.AssertEqual(
                OnCompleted<int>(210)
            );

            l.Subscriptions.AssertEqual(
                Subscribe(200, 210)
            );

            r.Subscriptions.AssertEqual(
                Subscribe(200, 210)
            );
        }

        [TestMethod]
        public void TakeUntil_Preempt_BeforeFirstProduced_RemainSilentAndProperDisposed()
        {
            var scheduler = new TestScheduler();

            bool sourceNotDisposed = false;

            var l = scheduler.CreateHotObservable(
                OnNext(150, 1),
                OnError<int>(215, new Exception()), // should not come
                OnCompleted<int>(240)
            );

            var r = scheduler.CreateHotObservable(
                OnNext(150, 1),
                OnNext(210, 2), //!
                OnCompleted<int>(220)
            );

            var res = scheduler.Start(() =>
                l.Do(_ => sourceNotDisposed = true).TakeUntil(r)
            );

            res.Messages.AssertEqual(
                OnCompleted<int>(210)
            );

            Assert.IsFalse(sourceNotDisposed);
        }

        [TestMethod]
        public void TakeUntil_NoPreempt_AfterLastProduced_ProperDisposedSignal()
        {
            var scheduler = new TestScheduler();

            bool signalNotDisposed = false;

            var l = scheduler.CreateHotObservable(
                OnNext(150, 1),
                OnNext(230, 2),
                OnCompleted<int>(240)
            );

            var r = scheduler.CreateHotObservable(
                OnNext(150, 1),
                OnNext(250, 2),
                OnCompleted<int>(260)
            );

            var res = scheduler.Start(() =>
                l.TakeUntil(r.Do(_ => signalNotDisposed = true))
            );

            res.Messages.AssertEqual(
                OnNext(230, 2),
                OnCompleted<int>(240)
            );

            Assert.IsFalse(signalNotDisposed);
        }

        [TestMethod]
        public void TakeUntil_Error_Some()
        {
            var scheduler = new TestScheduler();

            var ex = new Exception();

            var l = scheduler.CreateHotObservable(
                OnNext(150, 1),
                OnError<int>(225, ex)
            );

            var r = scheduler.CreateHotObservable(
                OnNext(150, 1),
                OnNext<int>(240, 2)
            );

            var res = scheduler.Start(() =>
                l.TakeUntil(r)
            );

            res.Messages.AssertEqual(
                OnError<int>(225, ex)
            );

            l.Subscriptions.AssertEqual(
                Subscribe(200, 225)
            );

            r.Subscriptions.AssertEqual(
                Subscribe(200, 225)
            );
        }

        #endregion

        #region + Window +

        [TestMethod]
        public void Window_ArgumentChecking()
        {
            ReactiveAssert.Throws<ArgumentNullException>(() => Observable.Window(default(IObservable<int>), DummyFunc<IObservable<int>>.Instance));
            ReactiveAssert.Throws<ArgumentNullException>(() => Observable.Window(DummyObservable<int>.Instance, default(Func<IObservable<int>>)));
            ReactiveAssert.Throws<ArgumentNullException>(() => Observable.Window(default(IObservable<int>), DummyObservable<int>.Instance, DummyFunc<int, IObservable<int>>.Instance));
            ReactiveAssert.Throws<ArgumentNullException>(() => Observable.Window(DummyObservable<int>.Instance, default(IObservable<int>), DummyFunc<int, IObservable<int>>.Instance));
            ReactiveAssert.Throws<ArgumentNullException>(() => Observable.Window(DummyObservable<int>.Instance, DummyObservable<int>.Instance, default(Func<int, IObservable<int>>)));
            ReactiveAssert.Throws<ArgumentNullException>(() => Observable.Window(default(IObservable<int>), DummyObservable<int>.Instance));
            ReactiveAssert.Throws<ArgumentNullException>(() => Observable.Window(DummyObservable<int>.Instance, default(IObservable<int>)));
        }

        [TestMethod]
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

        [TestMethod]
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

        [TestMethod]
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

        [TestMethod]
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

        [TestMethod]
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

        [TestMethod]
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

        [TestMethod]
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

        [TestMethod]
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

        [TestMethod]
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

        [TestMethod]
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

        [TestMethod]
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

        [TestMethod]
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

        [TestMethod]
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

        [TestMethod]
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

        [TestMethod]
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

        [TestMethod]
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

        [TestMethod]
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

        #endregion

        #region + Zip +

        #region ArgumentChecking

        [TestMethod]
        public void Zip_ArgumentChecking()
        {
            var someObservable = DummyObservable<int>.Instance;
            var someEnumerable = DummyEnumerable<int>.Instance;

            ReactiveAssert.Throws<ArgumentNullException>(() => Observable.Zip<int, int, int>(someObservable, someObservable, null));
            ReactiveAssert.Throws<ArgumentNullException>(() => Observable.Zip<int, int, int>(null, someObservable, (_, __) => 0));
            ReactiveAssert.Throws<ArgumentNullException>(() => Observable.Zip<int, int, int>(someObservable, default(IObservable<int>), (_, __) => 0));
            ReactiveAssert.Throws<ArgumentNullException>(() => Observable.Zip<int, int, int>(someObservable, someEnumerable, null));
            ReactiveAssert.Throws<ArgumentNullException>(() => Observable.Zip<int, int, int>(null, someEnumerable, (_, __) => 0));
            ReactiveAssert.Throws<ArgumentNullException>(() => Observable.Zip<int, int, int>(someObservable, default(IEnumerable<int>), (_, __) => 0));
        }

        [TestMethod]
        public void Zip_ArgumentCheckingHighArity()
        {
            var xs = DummyObservable<int>.Instance;

            ReactiveAssert.Throws<ArgumentNullException>(() => Observable.Zip(default(IObservable<int>), xs, (_0, _1) => _0 + _1));
            ReactiveAssert.Throws<ArgumentNullException>(() => Observable.Zip(xs, default(IObservable<int>), (_0, _1) => _0 + _1));
            ReactiveAssert.Throws<ArgumentNullException>(() => Observable.Zip(xs, xs, default(Func<int, int, int>)));
            ReactiveAssert.Throws<ArgumentNullException>(() => Observable.Zip(default(IObservable<int>), xs, xs, (_0, _1, _2) => _0 + _1 + _2));
            ReactiveAssert.Throws<ArgumentNullException>(() => Observable.Zip(xs, default(IObservable<int>), xs, (_0, _1, _2) => _0 + _1 + _2));
            ReactiveAssert.Throws<ArgumentNullException>(() => Observable.Zip(xs, xs, default(IObservable<int>), (_0, _1, _2) => _0 + _1 + _2));
            ReactiveAssert.Throws<ArgumentNullException>(() => Observable.Zip(xs, xs, xs, default(Func<int, int, int, int>)));
            ReactiveAssert.Throws<ArgumentNullException>(() => Observable.Zip(default(IObservable<int>), xs, xs, xs, (_0, _1, _2, _3) => _0 + _1 + _2 + _3));
            ReactiveAssert.Throws<ArgumentNullException>(() => Observable.Zip(xs, default(IObservable<int>), xs, xs, (_0, _1, _2, _3) => _0 + _1 + _2 + _3));
            ReactiveAssert.Throws<ArgumentNullException>(() => Observable.Zip(xs, xs, default(IObservable<int>), xs, (_0, _1, _2, _3) => _0 + _1 + _2 + _3));
            ReactiveAssert.Throws<ArgumentNullException>(() => Observable.Zip(xs, xs, xs, default(IObservable<int>), (_0, _1, _2, _3) => _0 + _1 + _2 + _3));
            ReactiveAssert.Throws<ArgumentNullException>(() => Observable.Zip(xs, xs, xs, xs, default(Func<int, int, int, int, int>)));
#if !NO_LARGEARITY
            ReactiveAssert.Throws<ArgumentNullException>(() => Observable.Zip(default(IObservable<int>), xs, xs, xs, xs, (_0, _1, _2, _3, _4) => _0 + _1 + _2 + _3 + _4));
            ReactiveAssert.Throws<ArgumentNullException>(() => Observable.Zip(xs, default(IObservable<int>), xs, xs, xs, (_0, _1, _2, _3, _4) => _0 + _1 + _2 + _3 + _4));
            ReactiveAssert.Throws<ArgumentNullException>(() => Observable.Zip(xs, xs, default(IObservable<int>), xs, xs, (_0, _1, _2, _3, _4) => _0 + _1 + _2 + _3 + _4));
            ReactiveAssert.Throws<ArgumentNullException>(() => Observable.Zip(xs, xs, xs, default(IObservable<int>), xs, (_0, _1, _2, _3, _4) => _0 + _1 + _2 + _3 + _4));
            ReactiveAssert.Throws<ArgumentNullException>(() => Observable.Zip(xs, xs, xs, xs, default(IObservable<int>), (_0, _1, _2, _3, _4) => _0 + _1 + _2 + _3 + _4));
            ReactiveAssert.Throws<ArgumentNullException>(() => Observable.Zip(xs, xs, xs, xs, xs, default(Func<int, int, int, int, int, int>)));
            ReactiveAssert.Throws<ArgumentNullException>(() => Observable.Zip(default(IObservable<int>), xs, xs, xs, xs, xs, (_0, _1, _2, _3, _4, _5) => _0 + _1 + _2 + _3 + _4 + _5));
            ReactiveAssert.Throws<ArgumentNullException>(() => Observable.Zip(xs, default(IObservable<int>), xs, xs, xs, xs, (_0, _1, _2, _3, _4, _5) => _0 + _1 + _2 + _3 + _4 + _5));
            ReactiveAssert.Throws<ArgumentNullException>(() => Observable.Zip(xs, xs, default(IObservable<int>), xs, xs, xs, (_0, _1, _2, _3, _4, _5) => _0 + _1 + _2 + _3 + _4 + _5));
            ReactiveAssert.Throws<ArgumentNullException>(() => Observable.Zip(xs, xs, xs, default(IObservable<int>), xs, xs, (_0, _1, _2, _3, _4, _5) => _0 + _1 + _2 + _3 + _4 + _5));
            ReactiveAssert.Throws<ArgumentNullException>(() => Observable.Zip(xs, xs, xs, xs, default(IObservable<int>), xs, (_0, _1, _2, _3, _4, _5) => _0 + _1 + _2 + _3 + _4 + _5));
            ReactiveAssert.Throws<ArgumentNullException>(() => Observable.Zip(xs, xs, xs, xs, xs, default(IObservable<int>), (_0, _1, _2, _3, _4, _5) => _0 + _1 + _2 + _3 + _4 + _5));
            ReactiveAssert.Throws<ArgumentNullException>(() => Observable.Zip(xs, xs, xs, xs, xs, xs, default(Func<int, int, int, int, int, int, int>)));
            ReactiveAssert.Throws<ArgumentNullException>(() => Observable.Zip(default(IObservable<int>), xs, xs, xs, xs, xs, xs, (_0, _1, _2, _3, _4, _5, _6) => _0 + _1 + _2 + _3 + _4 + _5 + _6));
            ReactiveAssert.Throws<ArgumentNullException>(() => Observable.Zip(xs, default(IObservable<int>), xs, xs, xs, xs, xs, (_0, _1, _2, _3, _4, _5, _6) => _0 + _1 + _2 + _3 + _4 + _5 + _6));
            ReactiveAssert.Throws<ArgumentNullException>(() => Observable.Zip(xs, xs, default(IObservable<int>), xs, xs, xs, xs, (_0, _1, _2, _3, _4, _5, _6) => _0 + _1 + _2 + _3 + _4 + _5 + _6));
            ReactiveAssert.Throws<ArgumentNullException>(() => Observable.Zip(xs, xs, xs, default(IObservable<int>), xs, xs, xs, (_0, _1, _2, _3, _4, _5, _6) => _0 + _1 + _2 + _3 + _4 + _5 + _6));
            ReactiveAssert.Throws<ArgumentNullException>(() => Observable.Zip(xs, xs, xs, xs, default(IObservable<int>), xs, xs, (_0, _1, _2, _3, _4, _5, _6) => _0 + _1 + _2 + _3 + _4 + _5 + _6));
            ReactiveAssert.Throws<ArgumentNullException>(() => Observable.Zip(xs, xs, xs, xs, xs, default(IObservable<int>), xs, (_0, _1, _2, _3, _4, _5, _6) => _0 + _1 + _2 + _3 + _4 + _5 + _6));
            ReactiveAssert.Throws<ArgumentNullException>(() => Observable.Zip(xs, xs, xs, xs, xs, xs, default(IObservable<int>), (_0, _1, _2, _3, _4, _5, _6) => _0 + _1 + _2 + _3 + _4 + _5 + _6));
            ReactiveAssert.Throws<ArgumentNullException>(() => Observable.Zip(xs, xs, xs, xs, xs, xs, xs, default(Func<int, int, int, int, int, int, int, int>)));
            ReactiveAssert.Throws<ArgumentNullException>(() => Observable.Zip(default(IObservable<int>), xs, xs, xs, xs, xs, xs, xs, (_0, _1, _2, _3, _4, _5, _6, _7) => _0 + _1 + _2 + _3 + _4 + _5 + _6 + _7));
            ReactiveAssert.Throws<ArgumentNullException>(() => Observable.Zip(xs, default(IObservable<int>), xs, xs, xs, xs, xs, xs, (_0, _1, _2, _3, _4, _5, _6, _7) => _0 + _1 + _2 + _3 + _4 + _5 + _6 + _7));
            ReactiveAssert.Throws<ArgumentNullException>(() => Observable.Zip(xs, xs, default(IObservable<int>), xs, xs, xs, xs, xs, (_0, _1, _2, _3, _4, _5, _6, _7) => _0 + _1 + _2 + _3 + _4 + _5 + _6 + _7));
            ReactiveAssert.Throws<ArgumentNullException>(() => Observable.Zip(xs, xs, xs, default(IObservable<int>), xs, xs, xs, xs, (_0, _1, _2, _3, _4, _5, _6, _7) => _0 + _1 + _2 + _3 + _4 + _5 + _6 + _7));
            ReactiveAssert.Throws<ArgumentNullException>(() => Observable.Zip(xs, xs, xs, xs, default(IObservable<int>), xs, xs, xs, (_0, _1, _2, _3, _4, _5, _6, _7) => _0 + _1 + _2 + _3 + _4 + _5 + _6 + _7));
            ReactiveAssert.Throws<ArgumentNullException>(() => Observable.Zip(xs, xs, xs, xs, xs, default(IObservable<int>), xs, xs, (_0, _1, _2, _3, _4, _5, _6, _7) => _0 + _1 + _2 + _3 + _4 + _5 + _6 + _7));
            ReactiveAssert.Throws<ArgumentNullException>(() => Observable.Zip(xs, xs, xs, xs, xs, xs, default(IObservable<int>), xs, (_0, _1, _2, _3, _4, _5, _6, _7) => _0 + _1 + _2 + _3 + _4 + _5 + _6 + _7));
            ReactiveAssert.Throws<ArgumentNullException>(() => Observable.Zip(xs, xs, xs, xs, xs, xs, xs, default(IObservable<int>), (_0, _1, _2, _3, _4, _5, _6, _7) => _0 + _1 + _2 + _3 + _4 + _5 + _6 + _7));
            ReactiveAssert.Throws<ArgumentNullException>(() => Observable.Zip(xs, xs, xs, xs, xs, xs, xs, xs, default(Func<int, int, int, int, int, int, int, int, int>)));
            ReactiveAssert.Throws<ArgumentNullException>(() => Observable.Zip(default(IObservable<int>), xs, xs, xs, xs, xs, xs, xs, xs, (_0, _1, _2, _3, _4, _5, _6, _7, _8) => _0 + _1 + _2 + _3 + _4 + _5 + _6 + _7 + _8));
            ReactiveAssert.Throws<ArgumentNullException>(() => Observable.Zip(xs, default(IObservable<int>), xs, xs, xs, xs, xs, xs, xs, (_0, _1, _2, _3, _4, _5, _6, _7, _8) => _0 + _1 + _2 + _3 + _4 + _5 + _6 + _7 + _8));
            ReactiveAssert.Throws<ArgumentNullException>(() => Observable.Zip(xs, xs, default(IObservable<int>), xs, xs, xs, xs, xs, xs, (_0, _1, _2, _3, _4, _5, _6, _7, _8) => _0 + _1 + _2 + _3 + _4 + _5 + _6 + _7 + _8));
            ReactiveAssert.Throws<ArgumentNullException>(() => Observable.Zip(xs, xs, xs, default(IObservable<int>), xs, xs, xs, xs, xs, (_0, _1, _2, _3, _4, _5, _6, _7, _8) => _0 + _1 + _2 + _3 + _4 + _5 + _6 + _7 + _8));
            ReactiveAssert.Throws<ArgumentNullException>(() => Observable.Zip(xs, xs, xs, xs, default(IObservable<int>), xs, xs, xs, xs, (_0, _1, _2, _3, _4, _5, _6, _7, _8) => _0 + _1 + _2 + _3 + _4 + _5 + _6 + _7 + _8));
            ReactiveAssert.Throws<ArgumentNullException>(() => Observable.Zip(xs, xs, xs, xs, xs, default(IObservable<int>), xs, xs, xs, (_0, _1, _2, _3, _4, _5, _6, _7, _8) => _0 + _1 + _2 + _3 + _4 + _5 + _6 + _7 + _8));
            ReactiveAssert.Throws<ArgumentNullException>(() => Observable.Zip(xs, xs, xs, xs, xs, xs, default(IObservable<int>), xs, xs, (_0, _1, _2, _3, _4, _5, _6, _7, _8) => _0 + _1 + _2 + _3 + _4 + _5 + _6 + _7 + _8));
            ReactiveAssert.Throws<ArgumentNullException>(() => Observable.Zip(xs, xs, xs, xs, xs, xs, xs, default(IObservable<int>), xs, (_0, _1, _2, _3, _4, _5, _6, _7, _8) => _0 + _1 + _2 + _3 + _4 + _5 + _6 + _7 + _8));
            ReactiveAssert.Throws<ArgumentNullException>(() => Observable.Zip(xs, xs, xs, xs, xs, xs, xs, xs, default(IObservable<int>), (_0, _1, _2, _3, _4, _5, _6, _7, _8) => _0 + _1 + _2 + _3 + _4 + _5 + _6 + _7 + _8));
            ReactiveAssert.Throws<ArgumentNullException>(() => Observable.Zip(xs, xs, xs, xs, xs, xs, xs, xs, xs, default(Func<int, int, int, int, int, int, int, int, int, int>)));
            ReactiveAssert.Throws<ArgumentNullException>(() => Observable.Zip(default(IObservable<int>), xs, xs, xs, xs, xs, xs, xs, xs, xs, (_0, _1, _2, _3, _4, _5, _6, _7, _8, _9) => _0 + _1 + _2 + _3 + _4 + _5 + _6 + _7 + _8 + _9));
            ReactiveAssert.Throws<ArgumentNullException>(() => Observable.Zip(xs, default(IObservable<int>), xs, xs, xs, xs, xs, xs, xs, xs, (_0, _1, _2, _3, _4, _5, _6, _7, _8, _9) => _0 + _1 + _2 + _3 + _4 + _5 + _6 + _7 + _8 + _9));
            ReactiveAssert.Throws<ArgumentNullException>(() => Observable.Zip(xs, xs, default(IObservable<int>), xs, xs, xs, xs, xs, xs, xs, (_0, _1, _2, _3, _4, _5, _6, _7, _8, _9) => _0 + _1 + _2 + _3 + _4 + _5 + _6 + _7 + _8 + _9));
            ReactiveAssert.Throws<ArgumentNullException>(() => Observable.Zip(xs, xs, xs, default(IObservable<int>), xs, xs, xs, xs, xs, xs, (_0, _1, _2, _3, _4, _5, _6, _7, _8, _9) => _0 + _1 + _2 + _3 + _4 + _5 + _6 + _7 + _8 + _9));
            ReactiveAssert.Throws<ArgumentNullException>(() => Observable.Zip(xs, xs, xs, xs, default(IObservable<int>), xs, xs, xs, xs, xs, (_0, _1, _2, _3, _4, _5, _6, _7, _8, _9) => _0 + _1 + _2 + _3 + _4 + _5 + _6 + _7 + _8 + _9));
            ReactiveAssert.Throws<ArgumentNullException>(() => Observable.Zip(xs, xs, xs, xs, xs, default(IObservable<int>), xs, xs, xs, xs, (_0, _1, _2, _3, _4, _5, _6, _7, _8, _9) => _0 + _1 + _2 + _3 + _4 + _5 + _6 + _7 + _8 + _9));
            ReactiveAssert.Throws<ArgumentNullException>(() => Observable.Zip(xs, xs, xs, xs, xs, xs, default(IObservable<int>), xs, xs, xs, (_0, _1, _2, _3, _4, _5, _6, _7, _8, _9) => _0 + _1 + _2 + _3 + _4 + _5 + _6 + _7 + _8 + _9));
            ReactiveAssert.Throws<ArgumentNullException>(() => Observable.Zip(xs, xs, xs, xs, xs, xs, xs, default(IObservable<int>), xs, xs, (_0, _1, _2, _3, _4, _5, _6, _7, _8, _9) => _0 + _1 + _2 + _3 + _4 + _5 + _6 + _7 + _8 + _9));
            ReactiveAssert.Throws<ArgumentNullException>(() => Observable.Zip(xs, xs, xs, xs, xs, xs, xs, xs, default(IObservable<int>), xs, (_0, _1, _2, _3, _4, _5, _6, _7, _8, _9) => _0 + _1 + _2 + _3 + _4 + _5 + _6 + _7 + _8 + _9));
            ReactiveAssert.Throws<ArgumentNullException>(() => Observable.Zip(xs, xs, xs, xs, xs, xs, xs, xs, xs, default(IObservable<int>), (_0, _1, _2, _3, _4, _5, _6, _7, _8, _9) => _0 + _1 + _2 + _3 + _4 + _5 + _6 + _7 + _8 + _9));
            ReactiveAssert.Throws<ArgumentNullException>(() => Observable.Zip(xs, xs, xs, xs, xs, xs, xs, xs, xs, xs, default(Func<int, int, int, int, int, int, int, int, int, int, int>)));
            ReactiveAssert.Throws<ArgumentNullException>(() => Observable.Zip(default(IObservable<int>), xs, xs, xs, xs, xs, xs, xs, xs, xs, xs, (_0, _1, _2, _3, _4, _5, _6, _7, _8, _9, _10) => _0 + _1 + _2 + _3 + _4 + _5 + _6 + _7 + _8 + _9 + _10));
            ReactiveAssert.Throws<ArgumentNullException>(() => Observable.Zip(xs, default(IObservable<int>), xs, xs, xs, xs, xs, xs, xs, xs, xs, (_0, _1, _2, _3, _4, _5, _6, _7, _8, _9, _10) => _0 + _1 + _2 + _3 + _4 + _5 + _6 + _7 + _8 + _9 + _10));
            ReactiveAssert.Throws<ArgumentNullException>(() => Observable.Zip(xs, xs, default(IObservable<int>), xs, xs, xs, xs, xs, xs, xs, xs, (_0, _1, _2, _3, _4, _5, _6, _7, _8, _9, _10) => _0 + _1 + _2 + _3 + _4 + _5 + _6 + _7 + _8 + _9 + _10));
            ReactiveAssert.Throws<ArgumentNullException>(() => Observable.Zip(xs, xs, xs, default(IObservable<int>), xs, xs, xs, xs, xs, xs, xs, (_0, _1, _2, _3, _4, _5, _6, _7, _8, _9, _10) => _0 + _1 + _2 + _3 + _4 + _5 + _6 + _7 + _8 + _9 + _10));
            ReactiveAssert.Throws<ArgumentNullException>(() => Observable.Zip(xs, xs, xs, xs, default(IObservable<int>), xs, xs, xs, xs, xs, xs, (_0, _1, _2, _3, _4, _5, _6, _7, _8, _9, _10) => _0 + _1 + _2 + _3 + _4 + _5 + _6 + _7 + _8 + _9 + _10));
            ReactiveAssert.Throws<ArgumentNullException>(() => Observable.Zip(xs, xs, xs, xs, xs, default(IObservable<int>), xs, xs, xs, xs, xs, (_0, _1, _2, _3, _4, _5, _6, _7, _8, _9, _10) => _0 + _1 + _2 + _3 + _4 + _5 + _6 + _7 + _8 + _9 + _10));
            ReactiveAssert.Throws<ArgumentNullException>(() => Observable.Zip(xs, xs, xs, xs, xs, xs, default(IObservable<int>), xs, xs, xs, xs, (_0, _1, _2, _3, _4, _5, _6, _7, _8, _9, _10) => _0 + _1 + _2 + _3 + _4 + _5 + _6 + _7 + _8 + _9 + _10));
            ReactiveAssert.Throws<ArgumentNullException>(() => Observable.Zip(xs, xs, xs, xs, xs, xs, xs, default(IObservable<int>), xs, xs, xs, (_0, _1, _2, _3, _4, _5, _6, _7, _8, _9, _10) => _0 + _1 + _2 + _3 + _4 + _5 + _6 + _7 + _8 + _9 + _10));
            ReactiveAssert.Throws<ArgumentNullException>(() => Observable.Zip(xs, xs, xs, xs, xs, xs, xs, xs, default(IObservable<int>), xs, xs, (_0, _1, _2, _3, _4, _5, _6, _7, _8, _9, _10) => _0 + _1 + _2 + _3 + _4 + _5 + _6 + _7 + _8 + _9 + _10));
            ReactiveAssert.Throws<ArgumentNullException>(() => Observable.Zip(xs, xs, xs, xs, xs, xs, xs, xs, xs, default(IObservable<int>), xs, (_0, _1, _2, _3, _4, _5, _6, _7, _8, _9, _10) => _0 + _1 + _2 + _3 + _4 + _5 + _6 + _7 + _8 + _9 + _10));
            ReactiveAssert.Throws<ArgumentNullException>(() => Observable.Zip(xs, xs, xs, xs, xs, xs, xs, xs, xs, xs, default(IObservable<int>), (_0, _1, _2, _3, _4, _5, _6, _7, _8, _9, _10) => _0 + _1 + _2 + _3 + _4 + _5 + _6 + _7 + _8 + _9 + _10));
            ReactiveAssert.Throws<ArgumentNullException>(() => Observable.Zip(xs, xs, xs, xs, xs, xs, xs, xs, xs, xs, xs, default(Func<int, int, int, int, int, int, int, int, int, int, int, int>)));
            ReactiveAssert.Throws<ArgumentNullException>(() => Observable.Zip(default(IObservable<int>), xs, xs, xs, xs, xs, xs, xs, xs, xs, xs, xs, (_0, _1, _2, _3, _4, _5, _6, _7, _8, _9, _10, _11) => _0 + _1 + _2 + _3 + _4 + _5 + _6 + _7 + _8 + _9 + _10 + _11));
            ReactiveAssert.Throws<ArgumentNullException>(() => Observable.Zip(xs, default(IObservable<int>), xs, xs, xs, xs, xs, xs, xs, xs, xs, xs, (_0, _1, _2, _3, _4, _5, _6, _7, _8, _9, _10, _11) => _0 + _1 + _2 + _3 + _4 + _5 + _6 + _7 + _8 + _9 + _10 + _11));
            ReactiveAssert.Throws<ArgumentNullException>(() => Observable.Zip(xs, xs, default(IObservable<int>), xs, xs, xs, xs, xs, xs, xs, xs, xs, (_0, _1, _2, _3, _4, _5, _6, _7, _8, _9, _10, _11) => _0 + _1 + _2 + _3 + _4 + _5 + _6 + _7 + _8 + _9 + _10 + _11));
            ReactiveAssert.Throws<ArgumentNullException>(() => Observable.Zip(xs, xs, xs, default(IObservable<int>), xs, xs, xs, xs, xs, xs, xs, xs, (_0, _1, _2, _3, _4, _5, _6, _7, _8, _9, _10, _11) => _0 + _1 + _2 + _3 + _4 + _5 + _6 + _7 + _8 + _9 + _10 + _11));
            ReactiveAssert.Throws<ArgumentNullException>(() => Observable.Zip(xs, xs, xs, xs, default(IObservable<int>), xs, xs, xs, xs, xs, xs, xs, (_0, _1, _2, _3, _4, _5, _6, _7, _8, _9, _10, _11) => _0 + _1 + _2 + _3 + _4 + _5 + _6 + _7 + _8 + _9 + _10 + _11));
            ReactiveAssert.Throws<ArgumentNullException>(() => Observable.Zip(xs, xs, xs, xs, xs, default(IObservable<int>), xs, xs, xs, xs, xs, xs, (_0, _1, _2, _3, _4, _5, _6, _7, _8, _9, _10, _11) => _0 + _1 + _2 + _3 + _4 + _5 + _6 + _7 + _8 + _9 + _10 + _11));
            ReactiveAssert.Throws<ArgumentNullException>(() => Observable.Zip(xs, xs, xs, xs, xs, xs, default(IObservable<int>), xs, xs, xs, xs, xs, (_0, _1, _2, _3, _4, _5, _6, _7, _8, _9, _10, _11) => _0 + _1 + _2 + _3 + _4 + _5 + _6 + _7 + _8 + _9 + _10 + _11));
            ReactiveAssert.Throws<ArgumentNullException>(() => Observable.Zip(xs, xs, xs, xs, xs, xs, xs, default(IObservable<int>), xs, xs, xs, xs, (_0, _1, _2, _3, _4, _5, _6, _7, _8, _9, _10, _11) => _0 + _1 + _2 + _3 + _4 + _5 + _6 + _7 + _8 + _9 + _10 + _11));
            ReactiveAssert.Throws<ArgumentNullException>(() => Observable.Zip(xs, xs, xs, xs, xs, xs, xs, xs, default(IObservable<int>), xs, xs, xs, (_0, _1, _2, _3, _4, _5, _6, _7, _8, _9, _10, _11) => _0 + _1 + _2 + _3 + _4 + _5 + _6 + _7 + _8 + _9 + _10 + _11));
            ReactiveAssert.Throws<ArgumentNullException>(() => Observable.Zip(xs, xs, xs, xs, xs, xs, xs, xs, xs, default(IObservable<int>), xs, xs, (_0, _1, _2, _3, _4, _5, _6, _7, _8, _9, _10, _11) => _0 + _1 + _2 + _3 + _4 + _5 + _6 + _7 + _8 + _9 + _10 + _11));
            ReactiveAssert.Throws<ArgumentNullException>(() => Observable.Zip(xs, xs, xs, xs, xs, xs, xs, xs, xs, xs, default(IObservable<int>), xs, (_0, _1, _2, _3, _4, _5, _6, _7, _8, _9, _10, _11) => _0 + _1 + _2 + _3 + _4 + _5 + _6 + _7 + _8 + _9 + _10 + _11));
            ReactiveAssert.Throws<ArgumentNullException>(() => Observable.Zip(xs, xs, xs, xs, xs, xs, xs, xs, xs, xs, xs, default(IObservable<int>), (_0, _1, _2, _3, _4, _5, _6, _7, _8, _9, _10, _11) => _0 + _1 + _2 + _3 + _4 + _5 + _6 + _7 + _8 + _9 + _10 + _11));
            ReactiveAssert.Throws<ArgumentNullException>(() => Observable.Zip(xs, xs, xs, xs, xs, xs, xs, xs, xs, xs, xs, xs, default(Func<int, int, int, int, int, int, int, int, int, int, int, int, int>)));
            ReactiveAssert.Throws<ArgumentNullException>(() => Observable.Zip(default(IObservable<int>), xs, xs, xs, xs, xs, xs, xs, xs, xs, xs, xs, xs, (_0, _1, _2, _3, _4, _5, _6, _7, _8, _9, _10, _11, _12) => _0 + _1 + _2 + _3 + _4 + _5 + _6 + _7 + _8 + _9 + _10 + _11 + _12));
            ReactiveAssert.Throws<ArgumentNullException>(() => Observable.Zip(xs, default(IObservable<int>), xs, xs, xs, xs, xs, xs, xs, xs, xs, xs, xs, (_0, _1, _2, _3, _4, _5, _6, _7, _8, _9, _10, _11, _12) => _0 + _1 + _2 + _3 + _4 + _5 + _6 + _7 + _8 + _9 + _10 + _11 + _12));
            ReactiveAssert.Throws<ArgumentNullException>(() => Observable.Zip(xs, xs, default(IObservable<int>), xs, xs, xs, xs, xs, xs, xs, xs, xs, xs, (_0, _1, _2, _3, _4, _5, _6, _7, _8, _9, _10, _11, _12) => _0 + _1 + _2 + _3 + _4 + _5 + _6 + _7 + _8 + _9 + _10 + _11 + _12));
            ReactiveAssert.Throws<ArgumentNullException>(() => Observable.Zip(xs, xs, xs, default(IObservable<int>), xs, xs, xs, xs, xs, xs, xs, xs, xs, (_0, _1, _2, _3, _4, _5, _6, _7, _8, _9, _10, _11, _12) => _0 + _1 + _2 + _3 + _4 + _5 + _6 + _7 + _8 + _9 + _10 + _11 + _12));
            ReactiveAssert.Throws<ArgumentNullException>(() => Observable.Zip(xs, xs, xs, xs, default(IObservable<int>), xs, xs, xs, xs, xs, xs, xs, xs, (_0, _1, _2, _3, _4, _5, _6, _7, _8, _9, _10, _11, _12) => _0 + _1 + _2 + _3 + _4 + _5 + _6 + _7 + _8 + _9 + _10 + _11 + _12));
            ReactiveAssert.Throws<ArgumentNullException>(() => Observable.Zip(xs, xs, xs, xs, xs, default(IObservable<int>), xs, xs, xs, xs, xs, xs, xs, (_0, _1, _2, _3, _4, _5, _6, _7, _8, _9, _10, _11, _12) => _0 + _1 + _2 + _3 + _4 + _5 + _6 + _7 + _8 + _9 + _10 + _11 + _12));
            ReactiveAssert.Throws<ArgumentNullException>(() => Observable.Zip(xs, xs, xs, xs, xs, xs, default(IObservable<int>), xs, xs, xs, xs, xs, xs, (_0, _1, _2, _3, _4, _5, _6, _7, _8, _9, _10, _11, _12) => _0 + _1 + _2 + _3 + _4 + _5 + _6 + _7 + _8 + _9 + _10 + _11 + _12));
            ReactiveAssert.Throws<ArgumentNullException>(() => Observable.Zip(xs, xs, xs, xs, xs, xs, xs, default(IObservable<int>), xs, xs, xs, xs, xs, (_0, _1, _2, _3, _4, _5, _6, _7, _8, _9, _10, _11, _12) => _0 + _1 + _2 + _3 + _4 + _5 + _6 + _7 + _8 + _9 + _10 + _11 + _12));
            ReactiveAssert.Throws<ArgumentNullException>(() => Observable.Zip(xs, xs, xs, xs, xs, xs, xs, xs, default(IObservable<int>), xs, xs, xs, xs, (_0, _1, _2, _3, _4, _5, _6, _7, _8, _9, _10, _11, _12) => _0 + _1 + _2 + _3 + _4 + _5 + _6 + _7 + _8 + _9 + _10 + _11 + _12));
            ReactiveAssert.Throws<ArgumentNullException>(() => Observable.Zip(xs, xs, xs, xs, xs, xs, xs, xs, xs, default(IObservable<int>), xs, xs, xs, (_0, _1, _2, _3, _4, _5, _6, _7, _8, _9, _10, _11, _12) => _0 + _1 + _2 + _3 + _4 + _5 + _6 + _7 + _8 + _9 + _10 + _11 + _12));
            ReactiveAssert.Throws<ArgumentNullException>(() => Observable.Zip(xs, xs, xs, xs, xs, xs, xs, xs, xs, xs, default(IObservable<int>), xs, xs, (_0, _1, _2, _3, _4, _5, _6, _7, _8, _9, _10, _11, _12) => _0 + _1 + _2 + _3 + _4 + _5 + _6 + _7 + _8 + _9 + _10 + _11 + _12));
            ReactiveAssert.Throws<ArgumentNullException>(() => Observable.Zip(xs, xs, xs, xs, xs, xs, xs, xs, xs, xs, xs, default(IObservable<int>), xs, (_0, _1, _2, _3, _4, _5, _6, _7, _8, _9, _10, _11, _12) => _0 + _1 + _2 + _3 + _4 + _5 + _6 + _7 + _8 + _9 + _10 + _11 + _12));
            ReactiveAssert.Throws<ArgumentNullException>(() => Observable.Zip(xs, xs, xs, xs, xs, xs, xs, xs, xs, xs, xs, xs, default(IObservable<int>), (_0, _1, _2, _3, _4, _5, _6, _7, _8, _9, _10, _11, _12) => _0 + _1 + _2 + _3 + _4 + _5 + _6 + _7 + _8 + _9 + _10 + _11 + _12));
            ReactiveAssert.Throws<ArgumentNullException>(() => Observable.Zip(xs, xs, xs, xs, xs, xs, xs, xs, xs, xs, xs, xs, xs, default(Func<int, int, int, int, int, int, int, int, int, int, int, int, int, int>)));
            ReactiveAssert.Throws<ArgumentNullException>(() => Observable.Zip(default(IObservable<int>), xs, xs, xs, xs, xs, xs, xs, xs, xs, xs, xs, xs, xs, (_0, _1, _2, _3, _4, _5, _6, _7, _8, _9, _10, _11, _12, _13) => _0 + _1 + _2 + _3 + _4 + _5 + _6 + _7 + _8 + _9 + _10 + _11 + _12 + _13));
            ReactiveAssert.Throws<ArgumentNullException>(() => Observable.Zip(xs, default(IObservable<int>), xs, xs, xs, xs, xs, xs, xs, xs, xs, xs, xs, xs, (_0, _1, _2, _3, _4, _5, _6, _7, _8, _9, _10, _11, _12, _13) => _0 + _1 + _2 + _3 + _4 + _5 + _6 + _7 + _8 + _9 + _10 + _11 + _12 + _13));
            ReactiveAssert.Throws<ArgumentNullException>(() => Observable.Zip(xs, xs, default(IObservable<int>), xs, xs, xs, xs, xs, xs, xs, xs, xs, xs, xs, (_0, _1, _2, _3, _4, _5, _6, _7, _8, _9, _10, _11, _12, _13) => _0 + _1 + _2 + _3 + _4 + _5 + _6 + _7 + _8 + _9 + _10 + _11 + _12 + _13));
            ReactiveAssert.Throws<ArgumentNullException>(() => Observable.Zip(xs, xs, xs, default(IObservable<int>), xs, xs, xs, xs, xs, xs, xs, xs, xs, xs, (_0, _1, _2, _3, _4, _5, _6, _7, _8, _9, _10, _11, _12, _13) => _0 + _1 + _2 + _3 + _4 + _5 + _6 + _7 + _8 + _9 + _10 + _11 + _12 + _13));
            ReactiveAssert.Throws<ArgumentNullException>(() => Observable.Zip(xs, xs, xs, xs, default(IObservable<int>), xs, xs, xs, xs, xs, xs, xs, xs, xs, (_0, _1, _2, _3, _4, _5, _6, _7, _8, _9, _10, _11, _12, _13) => _0 + _1 + _2 + _3 + _4 + _5 + _6 + _7 + _8 + _9 + _10 + _11 + _12 + _13));
            ReactiveAssert.Throws<ArgumentNullException>(() => Observable.Zip(xs, xs, xs, xs, xs, default(IObservable<int>), xs, xs, xs, xs, xs, xs, xs, xs, (_0, _1, _2, _3, _4, _5, _6, _7, _8, _9, _10, _11, _12, _13) => _0 + _1 + _2 + _3 + _4 + _5 + _6 + _7 + _8 + _9 + _10 + _11 + _12 + _13));
            ReactiveAssert.Throws<ArgumentNullException>(() => Observable.Zip(xs, xs, xs, xs, xs, xs, default(IObservable<int>), xs, xs, xs, xs, xs, xs, xs, (_0, _1, _2, _3, _4, _5, _6, _7, _8, _9, _10, _11, _12, _13) => _0 + _1 + _2 + _3 + _4 + _5 + _6 + _7 + _8 + _9 + _10 + _11 + _12 + _13));
            ReactiveAssert.Throws<ArgumentNullException>(() => Observable.Zip(xs, xs, xs, xs, xs, xs, xs, default(IObservable<int>), xs, xs, xs, xs, xs, xs, (_0, _1, _2, _3, _4, _5, _6, _7, _8, _9, _10, _11, _12, _13) => _0 + _1 + _2 + _3 + _4 + _5 + _6 + _7 + _8 + _9 + _10 + _11 + _12 + _13));
            ReactiveAssert.Throws<ArgumentNullException>(() => Observable.Zip(xs, xs, xs, xs, xs, xs, xs, xs, default(IObservable<int>), xs, xs, xs, xs, xs, (_0, _1, _2, _3, _4, _5, _6, _7, _8, _9, _10, _11, _12, _13) => _0 + _1 + _2 + _3 + _4 + _5 + _6 + _7 + _8 + _9 + _10 + _11 + _12 + _13));
            ReactiveAssert.Throws<ArgumentNullException>(() => Observable.Zip(xs, xs, xs, xs, xs, xs, xs, xs, xs, default(IObservable<int>), xs, xs, xs, xs, (_0, _1, _2, _3, _4, _5, _6, _7, _8, _9, _10, _11, _12, _13) => _0 + _1 + _2 + _3 + _4 + _5 + _6 + _7 + _8 + _9 + _10 + _11 + _12 + _13));
            ReactiveAssert.Throws<ArgumentNullException>(() => Observable.Zip(xs, xs, xs, xs, xs, xs, xs, xs, xs, xs, default(IObservable<int>), xs, xs, xs, (_0, _1, _2, _3, _4, _5, _6, _7, _8, _9, _10, _11, _12, _13) => _0 + _1 + _2 + _3 + _4 + _5 + _6 + _7 + _8 + _9 + _10 + _11 + _12 + _13));
            ReactiveAssert.Throws<ArgumentNullException>(() => Observable.Zip(xs, xs, xs, xs, xs, xs, xs, xs, xs, xs, xs, default(IObservable<int>), xs, xs, (_0, _1, _2, _3, _4, _5, _6, _7, _8, _9, _10, _11, _12, _13) => _0 + _1 + _2 + _3 + _4 + _5 + _6 + _7 + _8 + _9 + _10 + _11 + _12 + _13));
            ReactiveAssert.Throws<ArgumentNullException>(() => Observable.Zip(xs, xs, xs, xs, xs, xs, xs, xs, xs, xs, xs, xs, default(IObservable<int>), xs, (_0, _1, _2, _3, _4, _5, _6, _7, _8, _9, _10, _11, _12, _13) => _0 + _1 + _2 + _3 + _4 + _5 + _6 + _7 + _8 + _9 + _10 + _11 + _12 + _13));
            ReactiveAssert.Throws<ArgumentNullException>(() => Observable.Zip(xs, xs, xs, xs, xs, xs, xs, xs, xs, xs, xs, xs, xs, default(IObservable<int>), (_0, _1, _2, _3, _4, _5, _6, _7, _8, _9, _10, _11, _12, _13) => _0 + _1 + _2 + _3 + _4 + _5 + _6 + _7 + _8 + _9 + _10 + _11 + _12 + _13));
            ReactiveAssert.Throws<ArgumentNullException>(() => Observable.Zip(xs, xs, xs, xs, xs, xs, xs, xs, xs, xs, xs, xs, xs, xs, default(Func<int, int, int, int, int, int, int, int, int, int, int, int, int, int, int>)));
            ReactiveAssert.Throws<ArgumentNullException>(() => Observable.Zip(default(IObservable<int>), xs, xs, xs, xs, xs, xs, xs, xs, xs, xs, xs, xs, xs, xs, (_0, _1, _2, _3, _4, _5, _6, _7, _8, _9, _10, _11, _12, _13, _14) => _0 + _1 + _2 + _3 + _4 + _5 + _6 + _7 + _8 + _9 + _10 + _11 + _12 + _13 + _14));
            ReactiveAssert.Throws<ArgumentNullException>(() => Observable.Zip(xs, default(IObservable<int>), xs, xs, xs, xs, xs, xs, xs, xs, xs, xs, xs, xs, xs, (_0, _1, _2, _3, _4, _5, _6, _7, _8, _9, _10, _11, _12, _13, _14) => _0 + _1 + _2 + _3 + _4 + _5 + _6 + _7 + _8 + _9 + _10 + _11 + _12 + _13 + _14));
            ReactiveAssert.Throws<ArgumentNullException>(() => Observable.Zip(xs, xs, default(IObservable<int>), xs, xs, xs, xs, xs, xs, xs, xs, xs, xs, xs, xs, (_0, _1, _2, _3, _4, _5, _6, _7, _8, _9, _10, _11, _12, _13, _14) => _0 + _1 + _2 + _3 + _4 + _5 + _6 + _7 + _8 + _9 + _10 + _11 + _12 + _13 + _14));
            ReactiveAssert.Throws<ArgumentNullException>(() => Observable.Zip(xs, xs, xs, default(IObservable<int>), xs, xs, xs, xs, xs, xs, xs, xs, xs, xs, xs, (_0, _1, _2, _3, _4, _5, _6, _7, _8, _9, _10, _11, _12, _13, _14) => _0 + _1 + _2 + _3 + _4 + _5 + _6 + _7 + _8 + _9 + _10 + _11 + _12 + _13 + _14));
            ReactiveAssert.Throws<ArgumentNullException>(() => Observable.Zip(xs, xs, xs, xs, default(IObservable<int>), xs, xs, xs, xs, xs, xs, xs, xs, xs, xs, (_0, _1, _2, _3, _4, _5, _6, _7, _8, _9, _10, _11, _12, _13, _14) => _0 + _1 + _2 + _3 + _4 + _5 + _6 + _7 + _8 + _9 + _10 + _11 + _12 + _13 + _14));
            ReactiveAssert.Throws<ArgumentNullException>(() => Observable.Zip(xs, xs, xs, xs, xs, default(IObservable<int>), xs, xs, xs, xs, xs, xs, xs, xs, xs, (_0, _1, _2, _3, _4, _5, _6, _7, _8, _9, _10, _11, _12, _13, _14) => _0 + _1 + _2 + _3 + _4 + _5 + _6 + _7 + _8 + _9 + _10 + _11 + _12 + _13 + _14));
            ReactiveAssert.Throws<ArgumentNullException>(() => Observable.Zip(xs, xs, xs, xs, xs, xs, default(IObservable<int>), xs, xs, xs, xs, xs, xs, xs, xs, (_0, _1, _2, _3, _4, _5, _6, _7, _8, _9, _10, _11, _12, _13, _14) => _0 + _1 + _2 + _3 + _4 + _5 + _6 + _7 + _8 + _9 + _10 + _11 + _12 + _13 + _14));
            ReactiveAssert.Throws<ArgumentNullException>(() => Observable.Zip(xs, xs, xs, xs, xs, xs, xs, default(IObservable<int>), xs, xs, xs, xs, xs, xs, xs, (_0, _1, _2, _3, _4, _5, _6, _7, _8, _9, _10, _11, _12, _13, _14) => _0 + _1 + _2 + _3 + _4 + _5 + _6 + _7 + _8 + _9 + _10 + _11 + _12 + _13 + _14));
            ReactiveAssert.Throws<ArgumentNullException>(() => Observable.Zip(xs, xs, xs, xs, xs, xs, xs, xs, default(IObservable<int>), xs, xs, xs, xs, xs, xs, (_0, _1, _2, _3, _4, _5, _6, _7, _8, _9, _10, _11, _12, _13, _14) => _0 + _1 + _2 + _3 + _4 + _5 + _6 + _7 + _8 + _9 + _10 + _11 + _12 + _13 + _14));
            ReactiveAssert.Throws<ArgumentNullException>(() => Observable.Zip(xs, xs, xs, xs, xs, xs, xs, xs, xs, default(IObservable<int>), xs, xs, xs, xs, xs, (_0, _1, _2, _3, _4, _5, _6, _7, _8, _9, _10, _11, _12, _13, _14) => _0 + _1 + _2 + _3 + _4 + _5 + _6 + _7 + _8 + _9 + _10 + _11 + _12 + _13 + _14));
            ReactiveAssert.Throws<ArgumentNullException>(() => Observable.Zip(xs, xs, xs, xs, xs, xs, xs, xs, xs, xs, default(IObservable<int>), xs, xs, xs, xs, (_0, _1, _2, _3, _4, _5, _6, _7, _8, _9, _10, _11, _12, _13, _14) => _0 + _1 + _2 + _3 + _4 + _5 + _6 + _7 + _8 + _9 + _10 + _11 + _12 + _13 + _14));
            ReactiveAssert.Throws<ArgumentNullException>(() => Observable.Zip(xs, xs, xs, xs, xs, xs, xs, xs, xs, xs, xs, default(IObservable<int>), xs, xs, xs, (_0, _1, _2, _3, _4, _5, _6, _7, _8, _9, _10, _11, _12, _13, _14) => _0 + _1 + _2 + _3 + _4 + _5 + _6 + _7 + _8 + _9 + _10 + _11 + _12 + _13 + _14));
            ReactiveAssert.Throws<ArgumentNullException>(() => Observable.Zip(xs, xs, xs, xs, xs, xs, xs, xs, xs, xs, xs, xs, default(IObservable<int>), xs, xs, (_0, _1, _2, _3, _4, _5, _6, _7, _8, _9, _10, _11, _12, _13, _14) => _0 + _1 + _2 + _3 + _4 + _5 + _6 + _7 + _8 + _9 + _10 + _11 + _12 + _13 + _14));
            ReactiveAssert.Throws<ArgumentNullException>(() => Observable.Zip(xs, xs, xs, xs, xs, xs, xs, xs, xs, xs, xs, xs, xs, default(IObservable<int>), xs, (_0, _1, _2, _3, _4, _5, _6, _7, _8, _9, _10, _11, _12, _13, _14) => _0 + _1 + _2 + _3 + _4 + _5 + _6 + _7 + _8 + _9 + _10 + _11 + _12 + _13 + _14));
            ReactiveAssert.Throws<ArgumentNullException>(() => Observable.Zip(xs, xs, xs, xs, xs, xs, xs, xs, xs, xs, xs, xs, xs, xs, default(IObservable<int>), (_0, _1, _2, _3, _4, _5, _6, _7, _8, _9, _10, _11, _12, _13, _14) => _0 + _1 + _2 + _3 + _4 + _5 + _6 + _7 + _8 + _9 + _10 + _11 + _12 + _13 + _14));
            ReactiveAssert.Throws<ArgumentNullException>(() => Observable.Zip(xs, xs, xs, xs, xs, xs, xs, xs, xs, xs, xs, xs, xs, xs, xs, default(Func<int, int, int, int, int, int, int, int, int, int, int, int, int, int, int, int>)));
            ReactiveAssert.Throws<ArgumentNullException>(() => Observable.Zip(default(IObservable<int>), xs, xs, xs, xs, xs, xs, xs, xs, xs, xs, xs, xs, xs, xs, xs, (_0, _1, _2, _3, _4, _5, _6, _7, _8, _9, _10, _11, _12, _13, _14, _15) => _0 + _1 + _2 + _3 + _4 + _5 + _6 + _7 + _8 + _9 + _10 + _11 + _12 + _13 + _14 + _15));
            ReactiveAssert.Throws<ArgumentNullException>(() => Observable.Zip(xs, default(IObservable<int>), xs, xs, xs, xs, xs, xs, xs, xs, xs, xs, xs, xs, xs, xs, (_0, _1, _2, _3, _4, _5, _6, _7, _8, _9, _10, _11, _12, _13, _14, _15) => _0 + _1 + _2 + _3 + _4 + _5 + _6 + _7 + _8 + _9 + _10 + _11 + _12 + _13 + _14 + _15));
            ReactiveAssert.Throws<ArgumentNullException>(() => Observable.Zip(xs, xs, default(IObservable<int>), xs, xs, xs, xs, xs, xs, xs, xs, xs, xs, xs, xs, xs, (_0, _1, _2, _3, _4, _5, _6, _7, _8, _9, _10, _11, _12, _13, _14, _15) => _0 + _1 + _2 + _3 + _4 + _5 + _6 + _7 + _8 + _9 + _10 + _11 + _12 + _13 + _14 + _15));
            ReactiveAssert.Throws<ArgumentNullException>(() => Observable.Zip(xs, xs, xs, default(IObservable<int>), xs, xs, xs, xs, xs, xs, xs, xs, xs, xs, xs, xs, (_0, _1, _2, _3, _4, _5, _6, _7, _8, _9, _10, _11, _12, _13, _14, _15) => _0 + _1 + _2 + _3 + _4 + _5 + _6 + _7 + _8 + _9 + _10 + _11 + _12 + _13 + _14 + _15));
            ReactiveAssert.Throws<ArgumentNullException>(() => Observable.Zip(xs, xs, xs, xs, default(IObservable<int>), xs, xs, xs, xs, xs, xs, xs, xs, xs, xs, xs, (_0, _1, _2, _3, _4, _5, _6, _7, _8, _9, _10, _11, _12, _13, _14, _15) => _0 + _1 + _2 + _3 + _4 + _5 + _6 + _7 + _8 + _9 + _10 + _11 + _12 + _13 + _14 + _15));
            ReactiveAssert.Throws<ArgumentNullException>(() => Observable.Zip(xs, xs, xs, xs, xs, default(IObservable<int>), xs, xs, xs, xs, xs, xs, xs, xs, xs, xs, (_0, _1, _2, _3, _4, _5, _6, _7, _8, _9, _10, _11, _12, _13, _14, _15) => _0 + _1 + _2 + _3 + _4 + _5 + _6 + _7 + _8 + _9 + _10 + _11 + _12 + _13 + _14 + _15));
            ReactiveAssert.Throws<ArgumentNullException>(() => Observable.Zip(xs, xs, xs, xs, xs, xs, default(IObservable<int>), xs, xs, xs, xs, xs, xs, xs, xs, xs, (_0, _1, _2, _3, _4, _5, _6, _7, _8, _9, _10, _11, _12, _13, _14, _15) => _0 + _1 + _2 + _3 + _4 + _5 + _6 + _7 + _8 + _9 + _10 + _11 + _12 + _13 + _14 + _15));
            ReactiveAssert.Throws<ArgumentNullException>(() => Observable.Zip(xs, xs, xs, xs, xs, xs, xs, default(IObservable<int>), xs, xs, xs, xs, xs, xs, xs, xs, (_0, _1, _2, _3, _4, _5, _6, _7, _8, _9, _10, _11, _12, _13, _14, _15) => _0 + _1 + _2 + _3 + _4 + _5 + _6 + _7 + _8 + _9 + _10 + _11 + _12 + _13 + _14 + _15));
            ReactiveAssert.Throws<ArgumentNullException>(() => Observable.Zip(xs, xs, xs, xs, xs, xs, xs, xs, default(IObservable<int>), xs, xs, xs, xs, xs, xs, xs, (_0, _1, _2, _3, _4, _5, _6, _7, _8, _9, _10, _11, _12, _13, _14, _15) => _0 + _1 + _2 + _3 + _4 + _5 + _6 + _7 + _8 + _9 + _10 + _11 + _12 + _13 + _14 + _15));
            ReactiveAssert.Throws<ArgumentNullException>(() => Observable.Zip(xs, xs, xs, xs, xs, xs, xs, xs, xs, default(IObservable<int>), xs, xs, xs, xs, xs, xs, (_0, _1, _2, _3, _4, _5, _6, _7, _8, _9, _10, _11, _12, _13, _14, _15) => _0 + _1 + _2 + _3 + _4 + _5 + _6 + _7 + _8 + _9 + _10 + _11 + _12 + _13 + _14 + _15));
            ReactiveAssert.Throws<ArgumentNullException>(() => Observable.Zip(xs, xs, xs, xs, xs, xs, xs, xs, xs, xs, default(IObservable<int>), xs, xs, xs, xs, xs, (_0, _1, _2, _3, _4, _5, _6, _7, _8, _9, _10, _11, _12, _13, _14, _15) => _0 + _1 + _2 + _3 + _4 + _5 + _6 + _7 + _8 + _9 + _10 + _11 + _12 + _13 + _14 + _15));
            ReactiveAssert.Throws<ArgumentNullException>(() => Observable.Zip(xs, xs, xs, xs, xs, xs, xs, xs, xs, xs, xs, default(IObservable<int>), xs, xs, xs, xs, (_0, _1, _2, _3, _4, _5, _6, _7, _8, _9, _10, _11, _12, _13, _14, _15) => _0 + _1 + _2 + _3 + _4 + _5 + _6 + _7 + _8 + _9 + _10 + _11 + _12 + _13 + _14 + _15));
            ReactiveAssert.Throws<ArgumentNullException>(() => Observable.Zip(xs, xs, xs, xs, xs, xs, xs, xs, xs, xs, xs, xs, default(IObservable<int>), xs, xs, xs, (_0, _1, _2, _3, _4, _5, _6, _7, _8, _9, _10, _11, _12, _13, _14, _15) => _0 + _1 + _2 + _3 + _4 + _5 + _6 + _7 + _8 + _9 + _10 + _11 + _12 + _13 + _14 + _15));
            ReactiveAssert.Throws<ArgumentNullException>(() => Observable.Zip(xs, xs, xs, xs, xs, xs, xs, xs, xs, xs, xs, xs, xs, default(IObservable<int>), xs, xs, (_0, _1, _2, _3, _4, _5, _6, _7, _8, _9, _10, _11, _12, _13, _14, _15) => _0 + _1 + _2 + _3 + _4 + _5 + _6 + _7 + _8 + _9 + _10 + _11 + _12 + _13 + _14 + _15));
            ReactiveAssert.Throws<ArgumentNullException>(() => Observable.Zip(xs, xs, xs, xs, xs, xs, xs, xs, xs, xs, xs, xs, xs, xs, default(IObservable<int>), xs, (_0, _1, _2, _3, _4, _5, _6, _7, _8, _9, _10, _11, _12, _13, _14, _15) => _0 + _1 + _2 + _3 + _4 + _5 + _6 + _7 + _8 + _9 + _10 + _11 + _12 + _13 + _14 + _15));
            ReactiveAssert.Throws<ArgumentNullException>(() => Observable.Zip(xs, xs, xs, xs, xs, xs, xs, xs, xs, xs, xs, xs, xs, xs, xs, default(IObservable<int>), (_0, _1, _2, _3, _4, _5, _6, _7, _8, _9, _10, _11, _12, _13, _14, _15) => _0 + _1 + _2 + _3 + _4 + _5 + _6 + _7 + _8 + _9 + _10 + _11 + _12 + _13 + _14 + _15));
            ReactiveAssert.Throws<ArgumentNullException>(() => Observable.Zip(xs, xs, xs, xs, xs, xs, xs, xs, xs, xs, xs, xs, xs, xs, xs, xs, default(Func<int, int, int, int, int, int, int, int, int, int, int, int, int, int, int, int, int>)));
#endif
        }

        #endregion

        #region Never/Never

        [TestMethod]
        public void Zip_Never2()
        {
            var scheduler = new TestScheduler();

            var e0 = scheduler.CreateHotObservable(new[] { OnNext(150, 1) });
            var e1 = scheduler.CreateHotObservable(new[] { OnNext(150, 1) });

            var res = scheduler.Start(() =>
                Observable.Zip(e0, e1, (_0, _1) => 42)
            );
            
            res.Messages.AssertEqual(
            );

            foreach (var e in new[] { e0, e1 })
                e.Subscriptions.AssertEqual(Subscribe(200, 1000));
        }

        [TestMethod]
        public void Zip_Never3()
        {
            var scheduler = new TestScheduler();

            var e0 = scheduler.CreateHotObservable(new[] { OnNext(150, 1) });
            var e1 = scheduler.CreateHotObservable(new[] { OnNext(150, 1) });
            var e2 = scheduler.CreateHotObservable(new[] { OnNext(150, 1) });

            var res = scheduler.Start(() =>
                Observable.Zip(e0, e1, e2, (_0, _1, _2) => 42)
            );
            
            res.Messages.AssertEqual(
            );

            foreach (var e in new[] { e0, e1, e2 })
                e.Subscriptions.AssertEqual(Subscribe(200, 1000));
        }

        [TestMethod]
        public void Zip_Never4()
        {
            var scheduler = new TestScheduler();

            var e0 = scheduler.CreateHotObservable(new[] { OnNext(150, 1) });
            var e1 = scheduler.CreateHotObservable(new[] { OnNext(150, 1) });
            var e2 = scheduler.CreateHotObservable(new[] { OnNext(150, 1) });
            var e3 = scheduler.CreateHotObservable(new[] { OnNext(150, 1) });

            var res = scheduler.Start(() =>
                Observable.Zip(e0, e1, e2, e3, (_0, _1, _2, _3) => 42)
            );
            
            res.Messages.AssertEqual(
            );

            foreach (var e in new[] { e0, e1, e2, e3 })
                e.Subscriptions.AssertEqual(Subscribe(200, 1000));
        }

#if !NO_LARGEARITY
        [TestMethod]
        public void Zip_Never5()
        {
            var scheduler = new TestScheduler();

            var e0 = scheduler.CreateHotObservable(new[] { OnNext(150, 1) });
            var e1 = scheduler.CreateHotObservable(new[] { OnNext(150, 1) });
            var e2 = scheduler.CreateHotObservable(new[] { OnNext(150, 1) });
            var e3 = scheduler.CreateHotObservable(new[] { OnNext(150, 1) });
            var e4 = scheduler.CreateHotObservable(new[] { OnNext(150, 1) });

            var res = scheduler.Start(() =>
                Observable.Zip(e0, e1, e2, e3, e4, (_0, _1, _2, _3, _4) => 42)
            );
            
            res.Messages.AssertEqual(
            );

            foreach (var e in new[] { e0, e1, e2, e3, e4 })
                e.Subscriptions.AssertEqual(Subscribe(200, 1000));
        }

        [TestMethod]
        public void Zip_Never6()
        {
            var scheduler = new TestScheduler();

            var e0 = scheduler.CreateHotObservable(new[] { OnNext(150, 1) });
            var e1 = scheduler.CreateHotObservable(new[] { OnNext(150, 1) });
            var e2 = scheduler.CreateHotObservable(new[] { OnNext(150, 1) });
            var e3 = scheduler.CreateHotObservable(new[] { OnNext(150, 1) });
            var e4 = scheduler.CreateHotObservable(new[] { OnNext(150, 1) });
            var e5 = scheduler.CreateHotObservable(new[] { OnNext(150, 1) });

            var res = scheduler.Start(() =>
                Observable.Zip(e0, e1, e2, e3, e4, e5, (_0, _1, _2, _3, _4, _5) => 42)
            );
            
            res.Messages.AssertEqual(
            );

            foreach (var e in new[] { e0, e1, e2, e3, e4, e5 })
                e.Subscriptions.AssertEqual(Subscribe(200, 1000));
        }

        [TestMethod]
        public void Zip_Never7()
        {
            var scheduler = new TestScheduler();

            var e0 = scheduler.CreateHotObservable(new[] { OnNext(150, 1) });
            var e1 = scheduler.CreateHotObservable(new[] { OnNext(150, 1) });
            var e2 = scheduler.CreateHotObservable(new[] { OnNext(150, 1) });
            var e3 = scheduler.CreateHotObservable(new[] { OnNext(150, 1) });
            var e4 = scheduler.CreateHotObservable(new[] { OnNext(150, 1) });
            var e5 = scheduler.CreateHotObservable(new[] { OnNext(150, 1) });
            var e6 = scheduler.CreateHotObservable(new[] { OnNext(150, 1) });

            var res = scheduler.Start(() =>
                Observable.Zip(e0, e1, e2, e3, e4, e5, e6, (_0, _1, _2, _3, _4, _5, _6) => 42)
            );
            
            res.Messages.AssertEqual(
            );

            foreach (var e in new[] { e0, e1, e2, e3, e4, e5, e6 })
                e.Subscriptions.AssertEqual(Subscribe(200, 1000));
        }

        [TestMethod]
        public void Zip_Never8()
        {
            var scheduler = new TestScheduler();

            var e0 = scheduler.CreateHotObservable(new[] { OnNext(150, 1) });
            var e1 = scheduler.CreateHotObservable(new[] { OnNext(150, 1) });
            var e2 = scheduler.CreateHotObservable(new[] { OnNext(150, 1) });
            var e3 = scheduler.CreateHotObservable(new[] { OnNext(150, 1) });
            var e4 = scheduler.CreateHotObservable(new[] { OnNext(150, 1) });
            var e5 = scheduler.CreateHotObservable(new[] { OnNext(150, 1) });
            var e6 = scheduler.CreateHotObservable(new[] { OnNext(150, 1) });
            var e7 = scheduler.CreateHotObservable(new[] { OnNext(150, 1) });

            var res = scheduler.Start(() =>
                Observable.Zip(e0, e1, e2, e3, e4, e5, e6, e7, (_0, _1, _2, _3, _4, _5, _6, _7) => 42)
            );
            
            res.Messages.AssertEqual(
            );

            foreach (var e in new[] { e0, e1, e2, e3, e4, e5, e6, e7 })
                e.Subscriptions.AssertEqual(Subscribe(200, 1000));
        }

        [TestMethod]
        public void Zip_Never9()
        {
            var scheduler = new TestScheduler();

            var e0 = scheduler.CreateHotObservable(new[] { OnNext(150, 1) });
            var e1 = scheduler.CreateHotObservable(new[] { OnNext(150, 1) });
            var e2 = scheduler.CreateHotObservable(new[] { OnNext(150, 1) });
            var e3 = scheduler.CreateHotObservable(new[] { OnNext(150, 1) });
            var e4 = scheduler.CreateHotObservable(new[] { OnNext(150, 1) });
            var e5 = scheduler.CreateHotObservable(new[] { OnNext(150, 1) });
            var e6 = scheduler.CreateHotObservable(new[] { OnNext(150, 1) });
            var e7 = scheduler.CreateHotObservable(new[] { OnNext(150, 1) });
            var e8 = scheduler.CreateHotObservable(new[] { OnNext(150, 1) });

            var res = scheduler.Start(() =>
                Observable.Zip(e0, e1, e2, e3, e4, e5, e6, e7, e8, (_0, _1, _2, _3, _4, _5, _6, _7, _8) => 42)
            );
            
            res.Messages.AssertEqual(
            );

            foreach (var e in new[] { e0, e1, e2, e3, e4, e5, e6, e7, e8 })
                e.Subscriptions.AssertEqual(Subscribe(200, 1000));
        }

        [TestMethod]
        public void Zip_Never10()
        {
            var scheduler = new TestScheduler();

            var e0 = scheduler.CreateHotObservable(new[] { OnNext(150, 1) });
            var e1 = scheduler.CreateHotObservable(new[] { OnNext(150, 1) });
            var e2 = scheduler.CreateHotObservable(new[] { OnNext(150, 1) });
            var e3 = scheduler.CreateHotObservable(new[] { OnNext(150, 1) });
            var e4 = scheduler.CreateHotObservable(new[] { OnNext(150, 1) });
            var e5 = scheduler.CreateHotObservable(new[] { OnNext(150, 1) });
            var e6 = scheduler.CreateHotObservable(new[] { OnNext(150, 1) });
            var e7 = scheduler.CreateHotObservable(new[] { OnNext(150, 1) });
            var e8 = scheduler.CreateHotObservable(new[] { OnNext(150, 1) });
            var e9 = scheduler.CreateHotObservable(new[] { OnNext(150, 1) });

            var res = scheduler.Start(() =>
                Observable.Zip(e0, e1, e2, e3, e4, e5, e6, e7, e8, e9, (_0, _1, _2, _3, _4, _5, _6, _7, _8, _9) => 42)
            );
            
            res.Messages.AssertEqual(
            );

            foreach (var e in new[] { e0, e1, e2, e3, e4, e5, e6, e7, e8, e9 })
                e.Subscriptions.AssertEqual(Subscribe(200, 1000));
        }

        [TestMethod]
        public void Zip_Never11()
        {
            var scheduler = new TestScheduler();

            var e0 = scheduler.CreateHotObservable(new[] { OnNext(150, 1) });
            var e1 = scheduler.CreateHotObservable(new[] { OnNext(150, 1) });
            var e2 = scheduler.CreateHotObservable(new[] { OnNext(150, 1) });
            var e3 = scheduler.CreateHotObservable(new[] { OnNext(150, 1) });
            var e4 = scheduler.CreateHotObservable(new[] { OnNext(150, 1) });
            var e5 = scheduler.CreateHotObservable(new[] { OnNext(150, 1) });
            var e6 = scheduler.CreateHotObservable(new[] { OnNext(150, 1) });
            var e7 = scheduler.CreateHotObservable(new[] { OnNext(150, 1) });
            var e8 = scheduler.CreateHotObservable(new[] { OnNext(150, 1) });
            var e9 = scheduler.CreateHotObservable(new[] { OnNext(150, 1) });
            var e10 = scheduler.CreateHotObservable(new[] { OnNext(150, 1) });

            var res = scheduler.Start(() =>
                Observable.Zip(e0, e1, e2, e3, e4, e5, e6, e7, e8, e9, e10, (_0, _1, _2, _3, _4, _5, _6, _7, _8, _9, _10) => 42)
            );
            
            res.Messages.AssertEqual(
            );

            foreach (var e in new[] { e0, e1, e2, e3, e4, e5, e6, e7, e8, e9, e10 })
                e.Subscriptions.AssertEqual(Subscribe(200, 1000));
        }

        [TestMethod]
        public void Zip_Never12()
        {
            var scheduler = new TestScheduler();

            var e0 = scheduler.CreateHotObservable(new[] { OnNext(150, 1) });
            var e1 = scheduler.CreateHotObservable(new[] { OnNext(150, 1) });
            var e2 = scheduler.CreateHotObservable(new[] { OnNext(150, 1) });
            var e3 = scheduler.CreateHotObservable(new[] { OnNext(150, 1) });
            var e4 = scheduler.CreateHotObservable(new[] { OnNext(150, 1) });
            var e5 = scheduler.CreateHotObservable(new[] { OnNext(150, 1) });
            var e6 = scheduler.CreateHotObservable(new[] { OnNext(150, 1) });
            var e7 = scheduler.CreateHotObservable(new[] { OnNext(150, 1) });
            var e8 = scheduler.CreateHotObservable(new[] { OnNext(150, 1) });
            var e9 = scheduler.CreateHotObservable(new[] { OnNext(150, 1) });
            var e10 = scheduler.CreateHotObservable(new[] { OnNext(150, 1) });
            var e11 = scheduler.CreateHotObservable(new[] { OnNext(150, 1) });

            var res = scheduler.Start(() =>
                Observable.Zip(e0, e1, e2, e3, e4, e5, e6, e7, e8, e9, e10, e11, (_0, _1, _2, _3, _4, _5, _6, _7, _8, _9, _10, _11) => 42)
            );
            
            res.Messages.AssertEqual(
            );

            foreach (var e in new[] { e0, e1, e2, e3, e4, e5, e6, e7, e8, e9, e10, e11 })
                e.Subscriptions.AssertEqual(Subscribe(200, 1000));
        }

        [TestMethod]
        public void Zip_Never13()
        {
            var scheduler = new TestScheduler();

            var e0 = scheduler.CreateHotObservable(new[] { OnNext(150, 1) });
            var e1 = scheduler.CreateHotObservable(new[] { OnNext(150, 1) });
            var e2 = scheduler.CreateHotObservable(new[] { OnNext(150, 1) });
            var e3 = scheduler.CreateHotObservable(new[] { OnNext(150, 1) });
            var e4 = scheduler.CreateHotObservable(new[] { OnNext(150, 1) });
            var e5 = scheduler.CreateHotObservable(new[] { OnNext(150, 1) });
            var e6 = scheduler.CreateHotObservable(new[] { OnNext(150, 1) });
            var e7 = scheduler.CreateHotObservable(new[] { OnNext(150, 1) });
            var e8 = scheduler.CreateHotObservable(new[] { OnNext(150, 1) });
            var e9 = scheduler.CreateHotObservable(new[] { OnNext(150, 1) });
            var e10 = scheduler.CreateHotObservable(new[] { OnNext(150, 1) });
            var e11 = scheduler.CreateHotObservable(new[] { OnNext(150, 1) });
            var e12 = scheduler.CreateHotObservable(new[] { OnNext(150, 1) });

            var res = scheduler.Start(() =>
                Observable.Zip(e0, e1, e2, e3, e4, e5, e6, e7, e8, e9, e10, e11, e12, (_0, _1, _2, _3, _4, _5, _6, _7, _8, _9, _10, _11, _12) => 42)
            );
            
            res.Messages.AssertEqual(
            );

            foreach (var e in new[] { e0, e1, e2, e3, e4, e5, e6, e7, e8, e9, e10, e11, e12 })
                e.Subscriptions.AssertEqual(Subscribe(200, 1000));
        }

        [TestMethod]
        public void Zip_Never14()
        {
            var scheduler = new TestScheduler();

            var e0 = scheduler.CreateHotObservable(new[] { OnNext(150, 1) });
            var e1 = scheduler.CreateHotObservable(new[] { OnNext(150, 1) });
            var e2 = scheduler.CreateHotObservable(new[] { OnNext(150, 1) });
            var e3 = scheduler.CreateHotObservable(new[] { OnNext(150, 1) });
            var e4 = scheduler.CreateHotObservable(new[] { OnNext(150, 1) });
            var e5 = scheduler.CreateHotObservable(new[] { OnNext(150, 1) });
            var e6 = scheduler.CreateHotObservable(new[] { OnNext(150, 1) });
            var e7 = scheduler.CreateHotObservable(new[] { OnNext(150, 1) });
            var e8 = scheduler.CreateHotObservable(new[] { OnNext(150, 1) });
            var e9 = scheduler.CreateHotObservable(new[] { OnNext(150, 1) });
            var e10 = scheduler.CreateHotObservable(new[] { OnNext(150, 1) });
            var e11 = scheduler.CreateHotObservable(new[] { OnNext(150, 1) });
            var e12 = scheduler.CreateHotObservable(new[] { OnNext(150, 1) });
            var e13 = scheduler.CreateHotObservable(new[] { OnNext(150, 1) });

            var res = scheduler.Start(
                () => Observable.Zip(e0, e1, e2, e3, e4, e5, e6, e7, e8, e9, e10, e11, e12, e13, (_0, _1, _2, _3, _4, _5, _6, _7, _8, _9, _10, _11, _12, _13) => 42)
            );
            
            res.Messages.AssertEqual(
            );

            foreach (var e in new[] { e0, e1, e2, e3, e4, e5, e6, e7, e8, e9, e10, e11, e12, e13 })
                e.Subscriptions.AssertEqual(Subscribe(200, 1000));
        }

        [TestMethod]
        public void Zip_Never15()
        {
            var scheduler = new TestScheduler();

            var e0 = scheduler.CreateHotObservable(new[] { OnNext(150, 1) });
            var e1 = scheduler.CreateHotObservable(new[] { OnNext(150, 1) });
            var e2 = scheduler.CreateHotObservable(new[] { OnNext(150, 1) });
            var e3 = scheduler.CreateHotObservable(new[] { OnNext(150, 1) });
            var e4 = scheduler.CreateHotObservable(new[] { OnNext(150, 1) });
            var e5 = scheduler.CreateHotObservable(new[] { OnNext(150, 1) });
            var e6 = scheduler.CreateHotObservable(new[] { OnNext(150, 1) });
            var e7 = scheduler.CreateHotObservable(new[] { OnNext(150, 1) });
            var e8 = scheduler.CreateHotObservable(new[] { OnNext(150, 1) });
            var e9 = scheduler.CreateHotObservable(new[] { OnNext(150, 1) });
            var e10 = scheduler.CreateHotObservable(new[] { OnNext(150, 1) });
            var e11 = scheduler.CreateHotObservable(new[] { OnNext(150, 1) });
            var e12 = scheduler.CreateHotObservable(new[] { OnNext(150, 1) });
            var e13 = scheduler.CreateHotObservable(new[] { OnNext(150, 1) });
            var e14 = scheduler.CreateHotObservable(new[] { OnNext(150, 1) });

            var res = scheduler.Start(() =>
                Observable.Zip(e0, e1, e2, e3, e4, e5, e6, e7, e8, e9, e10, e11, e12, e13, e14, (_0, _1, _2, _3, _4, _5, _6, _7, _8, _9, _10, _11, _12, _13, _14) => 42)
            );
            
            res.Messages.AssertEqual(
            );

            foreach (var e in new[] { e0, e1, e2, e3, e4, e5, e6, e7, e8, e9, e10, e11, e12, e13, e14 })
                e.Subscriptions.AssertEqual(Subscribe(200, 1000));
        }

        [TestMethod]
        public void Zip_Never16()
        {
            var scheduler = new TestScheduler();

            var e0 = scheduler.CreateHotObservable(new[] { OnNext(150, 1) });
            var e1 = scheduler.CreateHotObservable(new[] { OnNext(150, 1) });
            var e2 = scheduler.CreateHotObservable(new[] { OnNext(150, 1) });
            var e3 = scheduler.CreateHotObservable(new[] { OnNext(150, 1) });
            var e4 = scheduler.CreateHotObservable(new[] { OnNext(150, 1) });
            var e5 = scheduler.CreateHotObservable(new[] { OnNext(150, 1) });
            var e6 = scheduler.CreateHotObservable(new[] { OnNext(150, 1) });
            var e7 = scheduler.CreateHotObservable(new[] { OnNext(150, 1) });
            var e8 = scheduler.CreateHotObservable(new[] { OnNext(150, 1) });
            var e9 = scheduler.CreateHotObservable(new[] { OnNext(150, 1) });
            var e10 = scheduler.CreateHotObservable(new[] { OnNext(150, 1) });
            var e11 = scheduler.CreateHotObservable(new[] { OnNext(150, 1) });
            var e12 = scheduler.CreateHotObservable(new[] { OnNext(150, 1) });
            var e13 = scheduler.CreateHotObservable(new[] { OnNext(150, 1) });
            var e14 = scheduler.CreateHotObservable(new[] { OnNext(150, 1) });
            var e15 = scheduler.CreateHotObservable(new[] { OnNext(150, 1) });

            var res = scheduler.Start(() =>
                Observable.Zip(e0, e1, e2, e3, e4, e5, e6, e7, e8, e9, e10, e11, e12, e13, e14, e15, (_0, _1, _2, _3, _4, _5, _6, _7, _8, _9, _10, _11, _12, _13, _14, _15) => 42)
            );
            
            res.Messages.AssertEqual(
            );

            foreach (var e in new[] { e0, e1, e2, e3, e4, e5, e6, e7, e8, e9, e10, e11, e12, e13, e14, e15 })
                e.Subscriptions.AssertEqual(Subscribe(200, 1000));
        }
#endif

        #endregion

        #region Never/Empty

        [TestMethod]
        public void Zip_NeverEmpty()
        {
            var scheduler = new TestScheduler();

            var n = scheduler.CreateHotObservable(
                OnNext(150, 1)
            );

            var e = scheduler.CreateHotObservable(
                OnNext(150, 1),
                OnCompleted<int>(210)
            );

            var res = scheduler.Start(() =>
                n.Zip(e, (x, y) => x + y)
            );

            res.Messages.AssertEqual(
            );

            n.Subscriptions.AssertEqual(
                Subscribe(200, 1000)
            );

            e.Subscriptions.AssertEqual(
                Subscribe(200, 210)
            );
        }

        [TestMethod]
        public void Zip_EmptyNever()
        {
            var scheduler = new TestScheduler();

            var n = scheduler.CreateHotObservable(
                OnNext(150, 1)
            );

            var e = scheduler.CreateHotObservable(
                OnNext(150, 1),
                OnCompleted<int>(210)
            );

            var res = scheduler.Start(() =>
                e.Zip(n, (x, y) => x + y)
            );

            res.Messages.AssertEqual(
            );

            n.Subscriptions.AssertEqual(
                Subscribe(200, 1000)
            );

            e.Subscriptions.AssertEqual(
                Subscribe(200, 210)
            );
        }

        #endregion

        #region Empty/Empty

        [TestMethod]
        public void Zip_EmptyEmpty()
        {
            var scheduler = new TestScheduler();

            var e1 = scheduler.CreateHotObservable(
                OnNext(150, 1),
                OnCompleted<int>(210)
            );

            var e2 = scheduler.CreateHotObservable(
                OnNext(150, 1),
                OnCompleted<int>(210)
            );

            var res = scheduler.Start(() =>
                e1.Zip(e2, (x, y) => x + y)
            );

            res.Messages.AssertEqual(
                OnCompleted<int>(210)
            );

            e1.Subscriptions.AssertEqual(
                Subscribe(200, 210)
            );

            e2.Subscriptions.AssertEqual(
                Subscribe(200, 210)
            );
        }

        [TestMethod]
        public void Zip_Empty2()
        {
            var scheduler = new TestScheduler();

            var e0 = scheduler.CreateHotObservable(new[] { OnNext(150, 1), OnCompleted<int>(210) });
            var e1 = scheduler.CreateHotObservable(new[] { OnNext(150, 1), OnCompleted<int>(220) });

            var res = scheduler.Start(() =>
                Observable.Zip(e0, e1, (_0, _1) => 42)
            );
            
            res.Messages.AssertEqual(
                OnCompleted<int>(220)
            );

            var i = 0;
            foreach (var e in new[] { e0, e1 })
                e.Subscriptions.AssertEqual(Subscribe(200, 200 + (++i * 10)));
        }

        [TestMethod]
        public void Zip_Empty3()
        {
            var scheduler = new TestScheduler();

            var e0 = scheduler.CreateHotObservable(new[] { OnNext(150, 1), OnCompleted<int>(210) });
            var e1 = scheduler.CreateHotObservable(new[] { OnNext(150, 1), OnCompleted<int>(220) });
            var e2 = scheduler.CreateHotObservable(new[] { OnNext(150, 1), OnCompleted<int>(230) });

            var res = scheduler.Start(() =>
                Observable.Zip(e0, e1, e2, (_0, _1, _2) => 42)
            );
            
            res.Messages.AssertEqual(
                OnCompleted<int>(230)
            );

            var i = 0;
            foreach (var e in new[] { e0, e1, e2 })
                e.Subscriptions.AssertEqual(Subscribe(200, 200 + (++i * 10)));
        }

        [TestMethod]
        public void Zip_Empty4()
        {
            var scheduler = new TestScheduler();

            var e0 = scheduler.CreateHotObservable(new[] { OnNext(150, 1), OnCompleted<int>(210) });
            var e1 = scheduler.CreateHotObservable(new[] { OnNext(150, 1), OnCompleted<int>(220) });
            var e2 = scheduler.CreateHotObservable(new[] { OnNext(150, 1), OnCompleted<int>(230) });
            var e3 = scheduler.CreateHotObservable(new[] { OnNext(150, 1), OnCompleted<int>(240) });

            var res = scheduler.Start(() =>
                Observable.Zip(e0, e1, e2, e3, (_0, _1, _2, _3) => 42)
            );
            
            res.Messages.AssertEqual(
                OnCompleted<int>(240)
            );

            var i = 0;
            foreach (var e in new[] { e0, e1, e2, e3 })
                e.Subscriptions.AssertEqual(Subscribe(200, 200 + (++i * 10)));
        }

#if !NO_LARGEARITY
        [TestMethod]
        public void Zip_Empty5()
        {
            var scheduler = new TestScheduler();

            var e0 = scheduler.CreateHotObservable(new[] { OnNext(150, 1), OnCompleted<int>(210) });
            var e1 = scheduler.CreateHotObservable(new[] { OnNext(150, 1), OnCompleted<int>(220) });
            var e2 = scheduler.CreateHotObservable(new[] { OnNext(150, 1), OnCompleted<int>(230) });
            var e3 = scheduler.CreateHotObservable(new[] { OnNext(150, 1), OnCompleted<int>(240) });
            var e4 = scheduler.CreateHotObservable(new[] { OnNext(150, 1), OnCompleted<int>(250) });

            var res = scheduler.Start(() =>
                Observable.Zip(e0, e1, e2, e3, e4, (_0, _1, _2, _3, _4) => 42)
            );
            
            res.Messages.AssertEqual(
                OnCompleted<int>(250)
            );

            var i = 0;
            foreach (var e in new[] { e0, e1, e2, e3, e4 })
                e.Subscriptions.AssertEqual(Subscribe(200, 200 + (++i * 10)));
        }

        [TestMethod]
        public void Zip_Empty6()
        {
            var scheduler = new TestScheduler();

            var e0 = scheduler.CreateHotObservable(new[] { OnNext(150, 1), OnCompleted<int>(210) });
            var e1 = scheduler.CreateHotObservable(new[] { OnNext(150, 1), OnCompleted<int>(220) });
            var e2 = scheduler.CreateHotObservable(new[] { OnNext(150, 1), OnCompleted<int>(230) });
            var e3 = scheduler.CreateHotObservable(new[] { OnNext(150, 1), OnCompleted<int>(240) });
            var e4 = scheduler.CreateHotObservable(new[] { OnNext(150, 1), OnCompleted<int>(250) });
            var e5 = scheduler.CreateHotObservable(new[] { OnNext(150, 1), OnCompleted<int>(260) });

            var res = scheduler.Start(() =>
                Observable.Zip(e0, e1, e2, e3, e4, e5, (_0, _1, _2, _3, _4, _5) => 42)
            );
            
            res.Messages.AssertEqual(
                OnCompleted<int>(260)
            );

            var i = 0;
            foreach (var e in new[] { e0, e1, e2, e3, e4, e5 })
                e.Subscriptions.AssertEqual(Subscribe(200, 200 + (++i * 10)));
        }

        [TestMethod]
        public void Zip_Empty7()
        {
            var scheduler = new TestScheduler();

            var e0 = scheduler.CreateHotObservable(new[] { OnNext(150, 1), OnCompleted<int>(210) });
            var e1 = scheduler.CreateHotObservable(new[] { OnNext(150, 1), OnCompleted<int>(220) });
            var e2 = scheduler.CreateHotObservable(new[] { OnNext(150, 1), OnCompleted<int>(230) });
            var e3 = scheduler.CreateHotObservable(new[] { OnNext(150, 1), OnCompleted<int>(240) });
            var e4 = scheduler.CreateHotObservable(new[] { OnNext(150, 1), OnCompleted<int>(250) });
            var e5 = scheduler.CreateHotObservable(new[] { OnNext(150, 1), OnCompleted<int>(260) });
            var e6 = scheduler.CreateHotObservable(new[] { OnNext(150, 1), OnCompleted<int>(270) });

            var res = scheduler.Start(() =>
                Observable.Zip(e0, e1, e2, e3, e4, e5, e6, (_0, _1, _2, _3, _4, _5, _6) => 42)
            );
            
            res.Messages.AssertEqual(
                OnCompleted<int>(270)
            );

            var i = 0;
            foreach (var e in new[] { e0, e1, e2, e3, e4, e5, e6 })
                e.Subscriptions.AssertEqual(Subscribe(200, 200 + (++i * 10)));
        }

        [TestMethod]
        public void Zip_Empty8()
        {
            var scheduler = new TestScheduler();

            var e0 = scheduler.CreateHotObservable(new[] { OnNext(150, 1), OnCompleted<int>(210) });
            var e1 = scheduler.CreateHotObservable(new[] { OnNext(150, 1), OnCompleted<int>(220) });
            var e2 = scheduler.CreateHotObservable(new[] { OnNext(150, 1), OnCompleted<int>(230) });
            var e3 = scheduler.CreateHotObservable(new[] { OnNext(150, 1), OnCompleted<int>(240) });
            var e4 = scheduler.CreateHotObservable(new[] { OnNext(150, 1), OnCompleted<int>(250) });
            var e5 = scheduler.CreateHotObservable(new[] { OnNext(150, 1), OnCompleted<int>(260) });
            var e6 = scheduler.CreateHotObservable(new[] { OnNext(150, 1), OnCompleted<int>(270) });
            var e7 = scheduler.CreateHotObservable(new[] { OnNext(150, 1), OnCompleted<int>(280) });

            var res = scheduler.Start(() =>
                Observable.Zip(e0, e1, e2, e3, e4, e5, e6, e7, (_0, _1, _2, _3, _4, _5, _6, _7) => 42)
            );
            
            res.Messages.AssertEqual(
                OnCompleted<int>(280)
            );

            var i = 0;
            foreach (var e in new[] { e0, e1, e2, e3, e4, e5, e6, e7 })
                e.Subscriptions.AssertEqual(Subscribe(200, 200 + (++i * 10)));
        }

        [TestMethod]
        public void Zip_Empty9()
        {
            var scheduler = new TestScheduler();

            var e0 = scheduler.CreateHotObservable(new[] { OnNext(150, 1), OnCompleted<int>(210) });
            var e1 = scheduler.CreateHotObservable(new[] { OnNext(150, 1), OnCompleted<int>(220) });
            var e2 = scheduler.CreateHotObservable(new[] { OnNext(150, 1), OnCompleted<int>(230) });
            var e3 = scheduler.CreateHotObservable(new[] { OnNext(150, 1), OnCompleted<int>(240) });
            var e4 = scheduler.CreateHotObservable(new[] { OnNext(150, 1), OnCompleted<int>(250) });
            var e5 = scheduler.CreateHotObservable(new[] { OnNext(150, 1), OnCompleted<int>(260) });
            var e6 = scheduler.CreateHotObservable(new[] { OnNext(150, 1), OnCompleted<int>(270) });
            var e7 = scheduler.CreateHotObservable(new[] { OnNext(150, 1), OnCompleted<int>(280) });
            var e8 = scheduler.CreateHotObservable(new[] { OnNext(150, 1), OnCompleted<int>(290) });

            var res = scheduler.Start(() =>
                Observable.Zip(e0, e1, e2, e3, e4, e5, e6, e7, e8, (_0, _1, _2, _3, _4, _5, _6, _7, _8) => 42)
            );
            
            res.Messages.AssertEqual(
                OnCompleted<int>(290)
            );

            var i = 0;
            foreach (var e in new[] { e0, e1, e2, e3, e4, e5, e6, e7, e8 })
                e.Subscriptions.AssertEqual(Subscribe(200, 200 + (++i * 10)));
        }

        [TestMethod]
        public void Zip_Empty10()
        {
            var scheduler = new TestScheduler();

            var e0 = scheduler.CreateHotObservable(new[] { OnNext(150, 1), OnCompleted<int>(210) });
            var e1 = scheduler.CreateHotObservable(new[] { OnNext(150, 1), OnCompleted<int>(220) });
            var e2 = scheduler.CreateHotObservable(new[] { OnNext(150, 1), OnCompleted<int>(230) });
            var e3 = scheduler.CreateHotObservable(new[] { OnNext(150, 1), OnCompleted<int>(240) });
            var e4 = scheduler.CreateHotObservable(new[] { OnNext(150, 1), OnCompleted<int>(250) });
            var e5 = scheduler.CreateHotObservable(new[] { OnNext(150, 1), OnCompleted<int>(260) });
            var e6 = scheduler.CreateHotObservable(new[] { OnNext(150, 1), OnCompleted<int>(270) });
            var e7 = scheduler.CreateHotObservable(new[] { OnNext(150, 1), OnCompleted<int>(280) });
            var e8 = scheduler.CreateHotObservable(new[] { OnNext(150, 1), OnCompleted<int>(290) });
            var e9 = scheduler.CreateHotObservable(new[] { OnNext(150, 1), OnCompleted<int>(300) });

            var res = scheduler.Start(() =>
                Observable.Zip(e0, e1, e2, e3, e4, e5, e6, e7, e8, e9, (_0, _1, _2, _3, _4, _5, _6, _7, _8, _9) => 42)
            );
            
            res.Messages.AssertEqual(
                OnCompleted<int>(300)
            );

            var i = 0;
            foreach (var e in new[] { e0, e1, e2, e3, e4, e5, e6, e7, e8, e9 })
                e.Subscriptions.AssertEqual(Subscribe(200, 200 + (++i * 10)));
        }

        [TestMethod]
        public void Zip_Empty11()
        {
            var scheduler = new TestScheduler();

            var e0 = scheduler.CreateHotObservable(new[] { OnNext(150, 1), OnCompleted<int>(210) });
            var e1 = scheduler.CreateHotObservable(new[] { OnNext(150, 1), OnCompleted<int>(220) });
            var e2 = scheduler.CreateHotObservable(new[] { OnNext(150, 1), OnCompleted<int>(230) });
            var e3 = scheduler.CreateHotObservable(new[] { OnNext(150, 1), OnCompleted<int>(240) });
            var e4 = scheduler.CreateHotObservable(new[] { OnNext(150, 1), OnCompleted<int>(250) });
            var e5 = scheduler.CreateHotObservable(new[] { OnNext(150, 1), OnCompleted<int>(260) });
            var e6 = scheduler.CreateHotObservable(new[] { OnNext(150, 1), OnCompleted<int>(270) });
            var e7 = scheduler.CreateHotObservable(new[] { OnNext(150, 1), OnCompleted<int>(280) });
            var e8 = scheduler.CreateHotObservable(new[] { OnNext(150, 1), OnCompleted<int>(290) });
            var e9 = scheduler.CreateHotObservable(new[] { OnNext(150, 1), OnCompleted<int>(300) });
            var e10 = scheduler.CreateHotObservable(new[] { OnNext(150, 1), OnCompleted<int>(310) });

            var res = scheduler.Start(() =>
                Observable.Zip(e0, e1, e2, e3, e4, e5, e6, e7, e8, e9, e10, (_0, _1, _2, _3, _4, _5, _6, _7, _8, _9, _10) => 42)
            );
            
            res.Messages.AssertEqual(
                OnCompleted<int>(310)
            );

            var i = 0;
            foreach (var e in new[] { e0, e1, e2, e3, e4, e5, e6, e7, e8, e9, e10 })
                e.Subscriptions.AssertEqual(Subscribe(200, 200 + (++i * 10)));
        }

        [TestMethod]
        public void Zip_Empty12()
        {
            var scheduler = new TestScheduler();

            var e0 = scheduler.CreateHotObservable(new[] { OnNext(150, 1), OnCompleted<int>(210) });
            var e1 = scheduler.CreateHotObservable(new[] { OnNext(150, 1), OnCompleted<int>(220) });
            var e2 = scheduler.CreateHotObservable(new[] { OnNext(150, 1), OnCompleted<int>(230) });
            var e3 = scheduler.CreateHotObservable(new[] { OnNext(150, 1), OnCompleted<int>(240) });
            var e4 = scheduler.CreateHotObservable(new[] { OnNext(150, 1), OnCompleted<int>(250) });
            var e5 = scheduler.CreateHotObservable(new[] { OnNext(150, 1), OnCompleted<int>(260) });
            var e6 = scheduler.CreateHotObservable(new[] { OnNext(150, 1), OnCompleted<int>(270) });
            var e7 = scheduler.CreateHotObservable(new[] { OnNext(150, 1), OnCompleted<int>(280) });
            var e8 = scheduler.CreateHotObservable(new[] { OnNext(150, 1), OnCompleted<int>(290) });
            var e9 = scheduler.CreateHotObservable(new[] { OnNext(150, 1), OnCompleted<int>(300) });
            var e10 = scheduler.CreateHotObservable(new[] { OnNext(150, 1), OnCompleted<int>(310) });
            var e11 = scheduler.CreateHotObservable(new[] { OnNext(150, 1), OnCompleted<int>(320) });

            var res = scheduler.Start(() =>
                Observable.Zip(e0, e1, e2, e3, e4, e5, e6, e7, e8, e9, e10, e11, (_0, _1, _2, _3, _4, _5, _6, _7, _8, _9, _10, _11) => 42)
            );
            
            res.Messages.AssertEqual(
                OnCompleted<int>(320)
            );

            var i = 0;
            foreach (var e in new[] { e0, e1, e2, e3, e4, e5, e6, e7, e8, e9, e10, e11 })
                e.Subscriptions.AssertEqual(Subscribe(200, 200 + (++i * 10)));
        }

        [TestMethod]
        public void Zip_Empty13()
        {
            var scheduler = new TestScheduler();

            var e0 = scheduler.CreateHotObservable(new[] { OnNext(150, 1), OnCompleted<int>(210) });
            var e1 = scheduler.CreateHotObservable(new[] { OnNext(150, 1), OnCompleted<int>(220) });
            var e2 = scheduler.CreateHotObservable(new[] { OnNext(150, 1), OnCompleted<int>(230) });
            var e3 = scheduler.CreateHotObservable(new[] { OnNext(150, 1), OnCompleted<int>(240) });
            var e4 = scheduler.CreateHotObservable(new[] { OnNext(150, 1), OnCompleted<int>(250) });
            var e5 = scheduler.CreateHotObservable(new[] { OnNext(150, 1), OnCompleted<int>(260) });
            var e6 = scheduler.CreateHotObservable(new[] { OnNext(150, 1), OnCompleted<int>(270) });
            var e7 = scheduler.CreateHotObservable(new[] { OnNext(150, 1), OnCompleted<int>(280) });
            var e8 = scheduler.CreateHotObservable(new[] { OnNext(150, 1), OnCompleted<int>(290) });
            var e9 = scheduler.CreateHotObservable(new[] { OnNext(150, 1), OnCompleted<int>(300) });
            var e10 = scheduler.CreateHotObservable(new[] { OnNext(150, 1), OnCompleted<int>(310) });
            var e11 = scheduler.CreateHotObservable(new[] { OnNext(150, 1), OnCompleted<int>(320) });
            var e12 = scheduler.CreateHotObservable(new[] { OnNext(150, 1), OnCompleted<int>(330) });

            var res = scheduler.Start(() =>
                Observable.Zip(e0, e1, e2, e3, e4, e5, e6, e7, e8, e9, e10, e11, e12, (_0, _1, _2, _3, _4, _5, _6, _7, _8, _9, _10, _11, _12) => 42)
            );
            
            res.Messages.AssertEqual(
                OnCompleted<int>(330)
            );

            var i = 0;
            foreach (var e in new[] { e0, e1, e2, e3, e4, e5, e6, e7, e8, e9, e10, e11, e12 })
                e.Subscriptions.AssertEqual(Subscribe(200, 200 + (++i * 10)));
        }

        [TestMethod]
        public void Zip_Empty14()
        {
            var scheduler = new TestScheduler();

            var e0 = scheduler.CreateHotObservable(new[] { OnNext(150, 1), OnCompleted<int>(210) });
            var e1 = scheduler.CreateHotObservable(new[] { OnNext(150, 1), OnCompleted<int>(220) });
            var e2 = scheduler.CreateHotObservable(new[] { OnNext(150, 1), OnCompleted<int>(230) });
            var e3 = scheduler.CreateHotObservable(new[] { OnNext(150, 1), OnCompleted<int>(240) });
            var e4 = scheduler.CreateHotObservable(new[] { OnNext(150, 1), OnCompleted<int>(250) });
            var e5 = scheduler.CreateHotObservable(new[] { OnNext(150, 1), OnCompleted<int>(260) });
            var e6 = scheduler.CreateHotObservable(new[] { OnNext(150, 1), OnCompleted<int>(270) });
            var e7 = scheduler.CreateHotObservable(new[] { OnNext(150, 1), OnCompleted<int>(280) });
            var e8 = scheduler.CreateHotObservable(new[] { OnNext(150, 1), OnCompleted<int>(290) });
            var e9 = scheduler.CreateHotObservable(new[] { OnNext(150, 1), OnCompleted<int>(300) });
            var e10 = scheduler.CreateHotObservable(new[] { OnNext(150, 1), OnCompleted<int>(310) });
            var e11 = scheduler.CreateHotObservable(new[] { OnNext(150, 1), OnCompleted<int>(320) });
            var e12 = scheduler.CreateHotObservable(new[] { OnNext(150, 1), OnCompleted<int>(330) });
            var e13 = scheduler.CreateHotObservable(new[] { OnNext(150, 1), OnCompleted<int>(340) });

            var res = scheduler.Start(() =>
                Observable.Zip(e0, e1, e2, e3, e4, e5, e6, e7, e8, e9, e10, e11, e12, e13, (_0, _1, _2, _3, _4, _5, _6, _7, _8, _9, _10, _11, _12, _13) => 42)
            );
            
            res.Messages.AssertEqual(
                OnCompleted<int>(340)
            );

            var i = 0;
            foreach (var e in new[] { e0, e1, e2, e3, e4, e5, e6, e7, e8, e9, e10, e11, e12, e13 })
                e.Subscriptions.AssertEqual(Subscribe(200, 200 + (++i * 10)));
        }

        [TestMethod]
        public void Zip_Empty15()
        {
            var scheduler = new TestScheduler();

            var e0 = scheduler.CreateHotObservable(new[] { OnNext(150, 1), OnCompleted<int>(210) });
            var e1 = scheduler.CreateHotObservable(new[] { OnNext(150, 1), OnCompleted<int>(220) });
            var e2 = scheduler.CreateHotObservable(new[] { OnNext(150, 1), OnCompleted<int>(230) });
            var e3 = scheduler.CreateHotObservable(new[] { OnNext(150, 1), OnCompleted<int>(240) });
            var e4 = scheduler.CreateHotObservable(new[] { OnNext(150, 1), OnCompleted<int>(250) });
            var e5 = scheduler.CreateHotObservable(new[] { OnNext(150, 1), OnCompleted<int>(260) });
            var e6 = scheduler.CreateHotObservable(new[] { OnNext(150, 1), OnCompleted<int>(270) });
            var e7 = scheduler.CreateHotObservable(new[] { OnNext(150, 1), OnCompleted<int>(280) });
            var e8 = scheduler.CreateHotObservable(new[] { OnNext(150, 1), OnCompleted<int>(290) });
            var e9 = scheduler.CreateHotObservable(new[] { OnNext(150, 1), OnCompleted<int>(300) });
            var e10 = scheduler.CreateHotObservable(new[] { OnNext(150, 1), OnCompleted<int>(310) });
            var e11 = scheduler.CreateHotObservable(new[] { OnNext(150, 1), OnCompleted<int>(320) });
            var e12 = scheduler.CreateHotObservable(new[] { OnNext(150, 1), OnCompleted<int>(330) });
            var e13 = scheduler.CreateHotObservable(new[] { OnNext(150, 1), OnCompleted<int>(340) });
            var e14 = scheduler.CreateHotObservable(new[] { OnNext(150, 1), OnCompleted<int>(350) });

            var res = scheduler.Start(() =>
                Observable.Zip(e0, e1, e2, e3, e4, e5, e6, e7, e8, e9, e10, e11, e12, e13, e14, (_0, _1, _2, _3, _4, _5, _6, _7, _8, _9, _10, _11, _12, _13, _14) => 42)
            );
            
            res.Messages.AssertEqual(
                OnCompleted<int>(350)
            );

            var i = 0;
            foreach (var e in new[] { e0, e1, e2, e3, e4, e5, e6, e7, e8, e9, e10, e11, e12, e13, e14 })
                e.Subscriptions.AssertEqual(Subscribe(200, 200 + (++i * 10)));
        }

        [TestMethod]
        public void Zip_Empty16()
        {
            var scheduler = new TestScheduler();

            var e0 = scheduler.CreateHotObservable(new[] { OnNext(150, 1), OnCompleted<int>(210) });
            var e1 = scheduler.CreateHotObservable(new[] { OnNext(150, 1), OnCompleted<int>(220) });
            var e2 = scheduler.CreateHotObservable(new[] { OnNext(150, 1), OnCompleted<int>(230) });
            var e3 = scheduler.CreateHotObservable(new[] { OnNext(150, 1), OnCompleted<int>(240) });
            var e4 = scheduler.CreateHotObservable(new[] { OnNext(150, 1), OnCompleted<int>(250) });
            var e5 = scheduler.CreateHotObservable(new[] { OnNext(150, 1), OnCompleted<int>(260) });
            var e6 = scheduler.CreateHotObservable(new[] { OnNext(150, 1), OnCompleted<int>(270) });
            var e7 = scheduler.CreateHotObservable(new[] { OnNext(150, 1), OnCompleted<int>(280) });
            var e8 = scheduler.CreateHotObservable(new[] { OnNext(150, 1), OnCompleted<int>(290) });
            var e9 = scheduler.CreateHotObservable(new[] { OnNext(150, 1), OnCompleted<int>(300) });
            var e10 = scheduler.CreateHotObservable(new[] { OnNext(150, 1), OnCompleted<int>(310) });
            var e11 = scheduler.CreateHotObservable(new[] { OnNext(150, 1), OnCompleted<int>(320) });
            var e12 = scheduler.CreateHotObservable(new[] { OnNext(150, 1), OnCompleted<int>(330) });
            var e13 = scheduler.CreateHotObservable(new[] { OnNext(150, 1), OnCompleted<int>(340) });
            var e14 = scheduler.CreateHotObservable(new[] { OnNext(150, 1), OnCompleted<int>(350) });
            var e15 = scheduler.CreateHotObservable(new[] { OnNext(150, 1), OnCompleted<int>(360) });

            var res = scheduler.Start(() =>
                Observable.Zip(e0, e1, e2, e3, e4, e5, e6, e7, e8, e9, e10, e11, e12, e13, e14, e15, (_0, _1, _2, _3, _4, _5, _6, _7, _8, _9, _10, _11, _12, _13, _14, _15) => 42)
            );
            
            res.Messages.AssertEqual(
                OnCompleted<int>(360)
            );

            var i = 0;
            foreach (var e in new[] { e0, e1, e2, e3, e4, e5, e6, e7, e8, e9, e10, e11, e12, e13, e14, e15 })
                e.Subscriptions.AssertEqual(Subscribe(200, 200 + (++i * 10)));
        }
#endif

        #endregion

        #region Empty/Some

        [TestMethod]
        public void Zip_EmptyNonEmpty()
        {
            var scheduler = new TestScheduler();

            var e = scheduler.CreateHotObservable(
                OnNext(150, 1),
                OnCompleted<int>(210)
            );

            var o = scheduler.CreateHotObservable(
                OnNext(150, 1),
                OnNext(215, 2), // Intended behavior - will only know here there was no error and we can complete gracefully
                OnCompleted<int>(220)
            );

            var res = scheduler.Start(() =>
                e.Zip(o, (x, y) => x + y)
            );

            res.Messages.AssertEqual(
                OnCompleted<int>(215)
            );

            e.Subscriptions.AssertEqual(
                Subscribe(200, 210)
            );

            o.Subscriptions.AssertEqual(
                Subscribe(200, 215)
            );
        }

        [TestMethod]
        public void Zip_NonEmptyEmpty()
        {
            var scheduler = new TestScheduler();

            var e = scheduler.CreateHotObservable(
                OnNext(150, 1),
                OnCompleted<int>(210)
            );

            var o = scheduler.CreateHotObservable(
                OnNext(150, 1),
                OnNext(215, 2),
                OnCompleted<int>(220)
            );

            var res = scheduler.Start(() =>
                o.Zip(e, (x, y) => x + y)
            );

            res.Messages.AssertEqual(
                OnCompleted<int>(215)
            );

            e.Subscriptions.AssertEqual(
                Subscribe(200, 210)
            );

            o.Subscriptions.AssertEqual(
                Subscribe(200, 215)
            );
        }

        #endregion

        #region Never/Some

        [TestMethod]
        public void Zip_NeverNonEmpty()
        {
            var scheduler = new TestScheduler();

            var o = scheduler.CreateHotObservable(
                OnNext(150, 1),
                OnNext(215, 2),
                OnCompleted<int>(220)
            );

            var n = scheduler.CreateHotObservable(
                OnNext(150, 1)
            );

            var res = scheduler.Start(() =>
                n.Zip(o, (x, y) => x + y)
            );

            res.Messages.AssertEqual(
            );

            o.Subscriptions.AssertEqual(
                Subscribe(200, 220)
            );

            n.Subscriptions.AssertEqual(
                Subscribe(200, 1000)
            );
        }

        [TestMethod]
        public void Zip_NonEmptyNever()
        {
            var scheduler = new TestScheduler();

            var o = scheduler.CreateHotObservable(
                OnNext(150, 1),
                OnNext(215, 2),
                OnCompleted<int>(220)
            );

            var n = scheduler.CreateHotObservable(
                OnNext(150, 1)
            );

            var res = scheduler.Start(() =>
                o.Zip(n, (x, y) => x + y)
            );

            res.Messages.AssertEqual(
            );

            o.Subscriptions.AssertEqual(
                Subscribe(200, 220)
            );

            n.Subscriptions.AssertEqual(
                Subscribe(200, 1000)
            );
        }

        #endregion

        #region Some/Some

        [TestMethod]
        public void Zip_NonEmptyNonEmpty()
        {
            var scheduler = new TestScheduler();

            var o1 = scheduler.CreateHotObservable(
                OnNext(150, 1),
                OnNext(215, 2),
                OnCompleted<int>(230)
            );

            var o2 = scheduler.CreateHotObservable(
                OnNext(150, 1),
                OnNext(220, 3),
                OnCompleted<int>(240) // Intended behavior - will only know here there was no error and we can complete gracefully
            );

            var res = scheduler.Start(() =>
                o1.Zip(o2, (x, y) => x + y)
            );

            res.Messages.AssertEqual(
                OnNext(220, 2 + 3),
                OnCompleted<int>(240)
            );

            o1.Subscriptions.AssertEqual(
                Subscribe(200, 230)
            );

            o2.Subscriptions.AssertEqual(
                Subscribe(200, 240)
            );
        }

        #endregion

        #region Empty/Error

        [TestMethod]
        public void Zip_EmptyError()
        {
            var scheduler = new TestScheduler();

            var ex = new Exception();

            var e = scheduler.CreateHotObservable(
                OnNext(150, 1),
                OnCompleted<int>(230)
            );

            var f = scheduler.CreateHotObservable(
                OnNext(150, 1),
                OnError<int>(220, ex)
            );

            var res = scheduler.Start(() =>
                e.Zip(f, (x, y) => x + y)
            );

            res.Messages.AssertEqual(
                OnError<int>(220, ex)
            );

            e.Subscriptions.AssertEqual(
                Subscribe(200, 220)
            );

            f.Subscriptions.AssertEqual(
                Subscribe(200, 220)
            );
        }

        [TestMethod]
        public void Zip_ErrorEmpty()
        {
            var scheduler = new TestScheduler();

            var ex = new Exception();

            var e = scheduler.CreateHotObservable(
                OnNext(150, 1),
                OnCompleted<int>(230)
            );

            var f = scheduler.CreateHotObservable(
                OnNext(150, 1),
                OnError<int>(220, ex)
            );

            var res = scheduler.Start(() =>
                f.Zip(e, (x, y) => x + y)
            );

            res.Messages.AssertEqual(
                OnError<int>(220, ex)
            );

            e.Subscriptions.AssertEqual(
                Subscribe(200, 220)
            );

            f.Subscriptions.AssertEqual(
                Subscribe(200, 220)
            );
        }

        #endregion

        #region Never/Error

        [TestMethod]
        public void Zip_NeverError()
        {
            var scheduler = new TestScheduler();

            var ex = new Exception();

            var n = scheduler.CreateHotObservable(
                OnNext(150, 1)
            );

            var f = scheduler.CreateHotObservable(
                OnNext(150, 1),
                OnError<int>(220, ex)
            );

            var res = scheduler.Start(() =>
                n.Zip(f, (x, y) => x + y)
            );

            res.Messages.AssertEqual(
                OnError<int>(220, ex)
            );

            n.Subscriptions.AssertEqual(
                Subscribe(200, 220)
            );

            f.Subscriptions.AssertEqual(
                Subscribe(200, 220)
            );
        }

        [TestMethod]
        public void Zip_ErrorNever()
        {
            var scheduler = new TestScheduler();

            var ex = new Exception();

            var n = scheduler.CreateHotObservable(
                OnNext(150, 1)
            );

            var f = scheduler.CreateHotObservable(
                OnNext(150, 1),
                OnError<int>(220, ex)
            );

            var res = scheduler.Start(() =>
                f.Zip(n, (x, y) => x + y)
            );

            res.Messages.AssertEqual(
                OnError<int>(220, ex)
            );

            n.Subscriptions.AssertEqual(
                Subscribe(200, 220)
            );

            f.Subscriptions.AssertEqual(
                Subscribe(200, 220)
            );
        }

        #endregion

        #region Error/Error

        [TestMethod]
        public void Zip_ErrorError()
        {
            var scheduler = new TestScheduler();

            var ex1 = new Exception();
            var ex2 = new Exception();

            var f1 = scheduler.CreateHotObservable(
                OnNext(150, 1),
                OnError<int>(230, ex1)
            );

            var f2 = scheduler.CreateHotObservable(
                OnNext(150, 1),
                OnError<int>(220, ex2)
            );

            var res = scheduler.Start(() =>
                f1.Zip(f2, (x, y) => x + y)
            );

            res.Messages.AssertEqual(
                OnError<int>(220, ex2)
            );

            f1.Subscriptions.AssertEqual(
                Subscribe(200, 220)
            );

            f2.Subscriptions.AssertEqual(
                Subscribe(200, 220)
            );
        }

        #endregion

        #region Some/Error

        [TestMethod]
        public void Zip_SomeError()
        {
            var scheduler = new TestScheduler();

            var ex = new Exception();

            var o = scheduler.CreateHotObservable(
                OnNext(150, 1),
                OnNext(215, 2),
                OnCompleted<int>(230)
            );

            var e = scheduler.CreateHotObservable(
                OnNext(150, 1),
                OnError<int>(220, ex)
            );

            var res = scheduler.Start(() =>
                o.Zip(e, (x, y) => x + y)
            );

            res.Messages.AssertEqual(
                OnError<int>(220, ex)
            );

            o.Subscriptions.AssertEqual(
                Subscribe(200, 220)
            );

            e.Subscriptions.AssertEqual(
                Subscribe(200, 220)
            );
        }

        [TestMethod]
        public void Zip_ErrorSome()
        {
            var scheduler = new TestScheduler();

            var ex = new Exception();

            var o = scheduler.CreateHotObservable(
                OnNext(150, 1),
                OnNext(215, 2),
                OnCompleted<int>(230)
            );

            var e = scheduler.CreateHotObservable(
                OnNext(150, 1),
                OnError<int>(220, ex)
            );

            var res = scheduler.Start(() =>
                e.Zip(o, (x, y) => x + y)
            );

            res.Messages.AssertEqual(
                OnError<int>(220, ex)
            );

            o.Subscriptions.AssertEqual(
                Subscribe(200, 220)
            );

            e.Subscriptions.AssertEqual(
                Subscribe(200, 220)
            );
        }

        #endregion

        #region Simple

        [TestMethod]
        public void Zip_LeftCompletesFirst()
        {
            var scheduler = new TestScheduler();

            var o = scheduler.CreateHotObservable(
                OnNext(150, 1),
                OnNext(210, 2),
                OnCompleted<int>(220)
            );

            var e = scheduler.CreateHotObservable(
                OnNext(150, 1),
                OnNext(215, 4),
                OnCompleted<int>(225)
            );

            var res = scheduler.Start(() =>
                o.Zip(e, (x, y) => x + y)
            );

            res.Messages.AssertEqual(
                OnNext(215, 6),
                OnCompleted<int>(225)
            );

            o.Subscriptions.AssertEqual(
                Subscribe(200, 220)
            );

            e.Subscriptions.AssertEqual(
                Subscribe(200, 225)
            );
        }

        [TestMethod]
        public void Zip_RightCompletesFirst()
        {
            var scheduler = new TestScheduler();

            var o = scheduler.CreateHotObservable(
                OnNext(150, 1),
                OnNext(215, 4),
                OnCompleted<int>(225)
            );

            var e = scheduler.CreateHotObservable(
                OnNext(150, 1),
                OnNext(210, 2),
                OnCompleted<int>(220)
            );

            var res = scheduler.Start(() =>
                o.Zip(e, (x, y) => x + y)
            );

            res.Messages.AssertEqual(
                OnNext(215, 6),
                OnCompleted<int>(225)
            );

            o.Subscriptions.AssertEqual(
                Subscribe(200, 225)
            );

            e.Subscriptions.AssertEqual(
                Subscribe(200, 220)
            );
        }

        [TestMethod]
        public void Zip_LeftTriggersSelectorError()
        {
            var scheduler = new TestScheduler();

            var ex = new Exception();

            var o = scheduler.CreateHotObservable(
                OnNext(150, 1),
                OnNext(220, 2)
            );

            var e = scheduler.CreateHotObservable(
                OnNext(150, 1),
                OnNext(210, 4)
            );

            var res = scheduler.Start(() =>
                o.Zip(e, (x, y) => { if (x == y) return 42; throw ex; })
            );

            res.Messages.AssertEqual(
                OnError<int>(220, ex)
            );

            o.Subscriptions.AssertEqual(
                Subscribe(200, 220)
            );

            e.Subscriptions.AssertEqual(
                Subscribe(200, 220)
            );
        }

        [TestMethod]
        public void Zip_RightTriggersSelectorError()
        {
            var scheduler = new TestScheduler();

            var ex = new Exception();

            var o = scheduler.CreateHotObservable(
                OnNext(150, 1),
                OnNext(210, 2)
            );

            var e = scheduler.CreateHotObservable(
                OnNext(150, 1),
                OnNext(220, 4)
            );

            var res = scheduler.Start(() =>
                o.Zip(e, (x, y) => { if (x == y) return 42; throw ex; })
            );

            res.Messages.AssertEqual(
                OnError<int>(220, ex)
            );

            o.Subscriptions.AssertEqual(
                Subscribe(200, 220)
            );

            e.Subscriptions.AssertEqual(
                Subscribe(200, 220)
            );
        }

        #endregion

        #region SymmetricReturn

        [TestMethod]
        public void Zip_SymmetricReturn2()
        {
            var scheduler = new TestScheduler();

            var e0 = scheduler.CreateHotObservable(new[] { OnNext(150, 1), OnNext(210, 1), OnCompleted<int>(400) });
            var e1 = scheduler.CreateHotObservable(new[] { OnNext(150, 1), OnNext(220, 2), OnCompleted<int>(400) });

            var res = scheduler.Start(() =>
                Observable.Zip(e0, e1, (_0, _1) => _0 + _1)
            );
            
            res.Messages.AssertEqual(
                OnNext(220, 3),
                OnCompleted<int>(400)
            );

            foreach (var e in new[] { e0, e1 })
                e.Subscriptions.AssertEqual(Subscribe(200, 400));
        }

        [TestMethod]
        public void Zip_SymmetricReturn3()
        {
            var scheduler = new TestScheduler();

            var e0 = scheduler.CreateHotObservable(new[] { OnNext(150, 1), OnNext(210, 1), OnCompleted<int>(400) });
            var e1 = scheduler.CreateHotObservable(new[] { OnNext(150, 1), OnNext(220, 2), OnCompleted<int>(400) });
            var e2 = scheduler.CreateHotObservable(new[] { OnNext(150, 1), OnNext(230, 3), OnCompleted<int>(400) });

            var res = scheduler.Start(() =>
                Observable.Zip(e0, e1, e2, (_0, _1, _2) => _0 + _1 + _2)
            );
            
            res.Messages.AssertEqual(
                OnNext(230, 6),
                OnCompleted<int>(400)
            );

            foreach (var e in new[] { e0, e1, e2 })
                e.Subscriptions.AssertEqual(Subscribe(200, 400));
        }

        [TestMethod]
        public void Zip_SymmetricReturn4()
        {
            var scheduler = new TestScheduler();

            var e0 = scheduler.CreateHotObservable(new[] { OnNext(150, 1), OnNext(210, 1), OnCompleted<int>(400) });
            var e1 = scheduler.CreateHotObservable(new[] { OnNext(150, 1), OnNext(220, 2), OnCompleted<int>(400) });
            var e2 = scheduler.CreateHotObservable(new[] { OnNext(150, 1), OnNext(230, 3), OnCompleted<int>(400) });
            var e3 = scheduler.CreateHotObservable(new[] { OnNext(150, 1), OnNext(240, 4), OnCompleted<int>(400) });

            var res = scheduler.Start(() =>
                Observable.Zip(e0, e1, e2, e3, (_0, _1, _2, _3) => _0 + _1 + _2 + _3)
            );
            
            res.Messages.AssertEqual(
                OnNext(240, 10),
                OnCompleted<int>(400)
            );

            foreach (var e in new[] { e0, e1, e2, e3 })
                e.Subscriptions.AssertEqual(Subscribe(200, 400));
        }

#if !NO_LARGEARITY
        [TestMethod]
        public void Zip_SymmetricReturn5()
        {
            var scheduler = new TestScheduler();

            var e0 = scheduler.CreateHotObservable(new[] { OnNext(150, 1), OnNext(210, 1), OnCompleted<int>(400) });
            var e1 = scheduler.CreateHotObservable(new[] { OnNext(150, 1), OnNext(220, 2), OnCompleted<int>(400) });
            var e2 = scheduler.CreateHotObservable(new[] { OnNext(150, 1), OnNext(230, 3), OnCompleted<int>(400) });
            var e3 = scheduler.CreateHotObservable(new[] { OnNext(150, 1), OnNext(240, 4), OnCompleted<int>(400) });
            var e4 = scheduler.CreateHotObservable(new[] { OnNext(150, 1), OnNext(250, 5), OnCompleted<int>(400) });

            var res = scheduler.Start(() =>
                Observable.Zip(e0, e1, e2, e3, e4, (_0, _1, _2, _3, _4) => _0 + _1 + _2 + _3 + _4)
            );
            
            res.Messages.AssertEqual(
                OnNext(250, 15),
                OnCompleted<int>(400)
            );

            foreach (var e in new[] { e0, e1, e2, e3, e4 })
                e.Subscriptions.AssertEqual(Subscribe(200, 400));
        }

        [TestMethod]
        public void Zip_SymmetricReturn6()
        {
            var scheduler = new TestScheduler();

            var e0 = scheduler.CreateHotObservable(new[] { OnNext(150, 1), OnNext(210, 1), OnCompleted<int>(400) });
            var e1 = scheduler.CreateHotObservable(new[] { OnNext(150, 1), OnNext(220, 2), OnCompleted<int>(400) });
            var e2 = scheduler.CreateHotObservable(new[] { OnNext(150, 1), OnNext(230, 3), OnCompleted<int>(400) });
            var e3 = scheduler.CreateHotObservable(new[] { OnNext(150, 1), OnNext(240, 4), OnCompleted<int>(400) });
            var e4 = scheduler.CreateHotObservable(new[] { OnNext(150, 1), OnNext(250, 5), OnCompleted<int>(400) });
            var e5 = scheduler.CreateHotObservable(new[] { OnNext(150, 1), OnNext(260, 6), OnCompleted<int>(400) });

            var res = scheduler.Start(() =>
                Observable.Zip(e0, e1, e2, e3, e4, e5, (_0, _1, _2, _3, _4, _5) => _0 + _1 + _2 + _3 + _4 + _5)
            );
            
            res.Messages.AssertEqual(
                OnNext(260, 21),
                OnCompleted<int>(400)
            );

            foreach (var e in new[] { e0, e1, e2, e3, e4, e5 })
                e.Subscriptions.AssertEqual(Subscribe(200, 400));
        }

        [TestMethod]
        public void Zip_SymmetricReturn7()
        {
            var scheduler = new TestScheduler();

            var e0 = scheduler.CreateHotObservable(new[] { OnNext(150, 1), OnNext(210, 1), OnCompleted<int>(400) });
            var e1 = scheduler.CreateHotObservable(new[] { OnNext(150, 1), OnNext(220, 2), OnCompleted<int>(400) });
            var e2 = scheduler.CreateHotObservable(new[] { OnNext(150, 1), OnNext(230, 3), OnCompleted<int>(400) });
            var e3 = scheduler.CreateHotObservable(new[] { OnNext(150, 1), OnNext(240, 4), OnCompleted<int>(400) });
            var e4 = scheduler.CreateHotObservable(new[] { OnNext(150, 1), OnNext(250, 5), OnCompleted<int>(400) });
            var e5 = scheduler.CreateHotObservable(new[] { OnNext(150, 1), OnNext(260, 6), OnCompleted<int>(400) });
            var e6 = scheduler.CreateHotObservable(new[] { OnNext(150, 1), OnNext(270, 7), OnCompleted<int>(400) });

            var res = scheduler.Start(() =>
                Observable.Zip(e0, e1, e2, e3, e4, e5, e6, (_0, _1, _2, _3, _4, _5, _6) => _0 + _1 + _2 + _3 + _4 + _5 + _6)
            );
            
            res.Messages.AssertEqual(
                OnNext(270, 28),
                OnCompleted<int>(400)
            );

            foreach (var e in new[] { e0, e1, e2, e3, e4, e5, e6 })
                e.Subscriptions.AssertEqual(Subscribe(200, 400));
        }

        [TestMethod]
        public void Zip_SymmetricReturn8()
        {
            var scheduler = new TestScheduler();

            var e0 = scheduler.CreateHotObservable(new[] { OnNext(150, 1), OnNext(210, 1), OnCompleted<int>(400) });
            var e1 = scheduler.CreateHotObservable(new[] { OnNext(150, 1), OnNext(220, 2), OnCompleted<int>(400) });
            var e2 = scheduler.CreateHotObservable(new[] { OnNext(150, 1), OnNext(230, 3), OnCompleted<int>(400) });
            var e3 = scheduler.CreateHotObservable(new[] { OnNext(150, 1), OnNext(240, 4), OnCompleted<int>(400) });
            var e4 = scheduler.CreateHotObservable(new[] { OnNext(150, 1), OnNext(250, 5), OnCompleted<int>(400) });
            var e5 = scheduler.CreateHotObservable(new[] { OnNext(150, 1), OnNext(260, 6), OnCompleted<int>(400) });
            var e6 = scheduler.CreateHotObservable(new[] { OnNext(150, 1), OnNext(270, 7), OnCompleted<int>(400) });
            var e7 = scheduler.CreateHotObservable(new[] { OnNext(150, 1), OnNext(280, 8), OnCompleted<int>(400) });

            var res = scheduler.Start(() =>
                Observable.Zip(e0, e1, e2, e3, e4, e5, e6, e7, (_0, _1, _2, _3, _4, _5, _6, _7) => _0 + _1 + _2 + _3 + _4 + _5 + _6 + _7)
            );
            
            res.Messages.AssertEqual(
                OnNext(280, 36),
                OnCompleted<int>(400)
            );

            foreach (var e in new[] { e0, e1, e2, e3, e4, e5, e6, e7 })
                e.Subscriptions.AssertEqual(Subscribe(200, 400));
        }

        [TestMethod]
        public void Zip_SymmetricReturn9()
        {
            var scheduler = new TestScheduler();

            var e0 = scheduler.CreateHotObservable(new[] { OnNext(150, 1), OnNext(210, 1), OnCompleted<int>(400) });
            var e1 = scheduler.CreateHotObservable(new[] { OnNext(150, 1), OnNext(220, 2), OnCompleted<int>(400) });
            var e2 = scheduler.CreateHotObservable(new[] { OnNext(150, 1), OnNext(230, 3), OnCompleted<int>(400) });
            var e3 = scheduler.CreateHotObservable(new[] { OnNext(150, 1), OnNext(240, 4), OnCompleted<int>(400) });
            var e4 = scheduler.CreateHotObservable(new[] { OnNext(150, 1), OnNext(250, 5), OnCompleted<int>(400) });
            var e5 = scheduler.CreateHotObservable(new[] { OnNext(150, 1), OnNext(260, 6), OnCompleted<int>(400) });
            var e6 = scheduler.CreateHotObservable(new[] { OnNext(150, 1), OnNext(270, 7), OnCompleted<int>(400) });
            var e7 = scheduler.CreateHotObservable(new[] { OnNext(150, 1), OnNext(280, 8), OnCompleted<int>(400) });
            var e8 = scheduler.CreateHotObservable(new[] { OnNext(150, 1), OnNext(290, 9), OnCompleted<int>(400) });

            var res = scheduler.Start(() =>
                Observable.Zip(e0, e1, e2, e3, e4, e5, e6, e7, e8, (_0, _1, _2, _3, _4, _5, _6, _7, _8) => _0 + _1 + _2 + _3 + _4 + _5 + _6 + _7 + _8)
            );
            
            res.Messages.AssertEqual(
                OnNext(290, 45),
                OnCompleted<int>(400)
            );

            foreach (var e in new[] { e0, e1, e2, e3, e4, e5, e6, e7, e8 })
                e.Subscriptions.AssertEqual(Subscribe(200, 400));
        }

        [TestMethod]
        public void Zip_SymmetricReturn10()
        {
            var scheduler = new TestScheduler();

            var e0 = scheduler.CreateHotObservable(new[] { OnNext(150, 1), OnNext(210, 1), OnCompleted<int>(400) });
            var e1 = scheduler.CreateHotObservable(new[] { OnNext(150, 1), OnNext(220, 2), OnCompleted<int>(400) });
            var e2 = scheduler.CreateHotObservable(new[] { OnNext(150, 1), OnNext(230, 3), OnCompleted<int>(400) });
            var e3 = scheduler.CreateHotObservable(new[] { OnNext(150, 1), OnNext(240, 4), OnCompleted<int>(400) });
            var e4 = scheduler.CreateHotObservable(new[] { OnNext(150, 1), OnNext(250, 5), OnCompleted<int>(400) });
            var e5 = scheduler.CreateHotObservable(new[] { OnNext(150, 1), OnNext(260, 6), OnCompleted<int>(400) });
            var e6 = scheduler.CreateHotObservable(new[] { OnNext(150, 1), OnNext(270, 7), OnCompleted<int>(400) });
            var e7 = scheduler.CreateHotObservable(new[] { OnNext(150, 1), OnNext(280, 8), OnCompleted<int>(400) });
            var e8 = scheduler.CreateHotObservable(new[] { OnNext(150, 1), OnNext(290, 9), OnCompleted<int>(400) });
            var e9 = scheduler.CreateHotObservable(new[] { OnNext(150, 1), OnNext(300, 10), OnCompleted<int>(400) });

            var res = scheduler.Start(() =>
                Observable.Zip(e0, e1, e2, e3, e4, e5, e6, e7, e8, e9, (_0, _1, _2, _3, _4, _5, _6, _7, _8, _9) => _0 + _1 + _2 + _3 + _4 + _5 + _6 + _7 + _8 + _9)
            );
            
            res.Messages.AssertEqual(
                OnNext(300, 55),
                OnCompleted<int>(400)
            );

            foreach (var e in new[] { e0, e1, e2, e3, e4, e5, e6, e7, e8, e9 })
                e.Subscriptions.AssertEqual(Subscribe(200, 400));
        }

        [TestMethod]
        public void Zip_SymmetricReturn11()
        {
            var scheduler = new TestScheduler();

            var e0 = scheduler.CreateHotObservable(new[] { OnNext(150, 1), OnNext(210, 1), OnCompleted<int>(400) });
            var e1 = scheduler.CreateHotObservable(new[] { OnNext(150, 1), OnNext(220, 2), OnCompleted<int>(400) });
            var e2 = scheduler.CreateHotObservable(new[] { OnNext(150, 1), OnNext(230, 3), OnCompleted<int>(400) });
            var e3 = scheduler.CreateHotObservable(new[] { OnNext(150, 1), OnNext(240, 4), OnCompleted<int>(400) });
            var e4 = scheduler.CreateHotObservable(new[] { OnNext(150, 1), OnNext(250, 5), OnCompleted<int>(400) });
            var e5 = scheduler.CreateHotObservable(new[] { OnNext(150, 1), OnNext(260, 6), OnCompleted<int>(400) });
            var e6 = scheduler.CreateHotObservable(new[] { OnNext(150, 1), OnNext(270, 7), OnCompleted<int>(400) });
            var e7 = scheduler.CreateHotObservable(new[] { OnNext(150, 1), OnNext(280, 8), OnCompleted<int>(400) });
            var e8 = scheduler.CreateHotObservable(new[] { OnNext(150, 1), OnNext(290, 9), OnCompleted<int>(400) });
            var e9 = scheduler.CreateHotObservable(new[] { OnNext(150, 1), OnNext(300, 10), OnCompleted<int>(400) });
            var e10 = scheduler.CreateHotObservable(new[] { OnNext(150, 1), OnNext(310, 11), OnCompleted<int>(400) });

            var res = scheduler.Start(() =>
                Observable.Zip(e0, e1, e2, e3, e4, e5, e6, e7, e8, e9, e10, (_0, _1, _2, _3, _4, _5, _6, _7, _8, _9, _10) => _0 + _1 + _2 + _3 + _4 + _5 + _6 + _7 + _8 + _9 + _10)
            );
            
            res.Messages.AssertEqual(
                OnNext(310, 66),
                OnCompleted<int>(400)
            );

            foreach (var e in new[] { e0, e1, e2, e3, e4, e5, e6, e7, e8, e9, e10 })
                e.Subscriptions.AssertEqual(Subscribe(200, 400));
        }

        [TestMethod]
        public void Zip_SymmetricReturn12()
        {
            var scheduler = new TestScheduler();

            var e0 = scheduler.CreateHotObservable(new[] { OnNext(150, 1), OnNext(210, 1), OnCompleted<int>(400) });
            var e1 = scheduler.CreateHotObservable(new[] { OnNext(150, 1), OnNext(220, 2), OnCompleted<int>(400) });
            var e2 = scheduler.CreateHotObservable(new[] { OnNext(150, 1), OnNext(230, 3), OnCompleted<int>(400) });
            var e3 = scheduler.CreateHotObservable(new[] { OnNext(150, 1), OnNext(240, 4), OnCompleted<int>(400) });
            var e4 = scheduler.CreateHotObservable(new[] { OnNext(150, 1), OnNext(250, 5), OnCompleted<int>(400) });
            var e5 = scheduler.CreateHotObservable(new[] { OnNext(150, 1), OnNext(260, 6), OnCompleted<int>(400) });
            var e6 = scheduler.CreateHotObservable(new[] { OnNext(150, 1), OnNext(270, 7), OnCompleted<int>(400) });
            var e7 = scheduler.CreateHotObservable(new[] { OnNext(150, 1), OnNext(280, 8), OnCompleted<int>(400) });
            var e8 = scheduler.CreateHotObservable(new[] { OnNext(150, 1), OnNext(290, 9), OnCompleted<int>(400) });
            var e9 = scheduler.CreateHotObservable(new[] { OnNext(150, 1), OnNext(300, 10), OnCompleted<int>(400) });
            var e10 = scheduler.CreateHotObservable(new[] { OnNext(150, 1), OnNext(310, 11), OnCompleted<int>(400) });
            var e11 = scheduler.CreateHotObservable(new[] { OnNext(150, 1), OnNext(320, 12), OnCompleted<int>(400) });

            var res = scheduler.Start(() =>
                Observable.Zip(e0, e1, e2, e3, e4, e5, e6, e7, e8, e9, e10, e11, (_0, _1, _2, _3, _4, _5, _6, _7, _8, _9, _10, _11) => _0 + _1 + _2 + _3 + _4 + _5 + _6 + _7 + _8 + _9 + _10 + _11)
            );
            
            res.Messages.AssertEqual(
                OnNext(320, 78),
                OnCompleted<int>(400)
            );

            foreach (var e in new[] { e0, e1, e2, e3, e4, e5, e6, e7, e8, e9, e10, e11 })
                e.Subscriptions.AssertEqual(Subscribe(200, 400));
        }

        [TestMethod]
        public void Zip_SymmetricReturn13()
        {
            var scheduler = new TestScheduler();

            var e0 = scheduler.CreateHotObservable(new[] { OnNext(150, 1), OnNext(210, 1), OnCompleted<int>(400) });
            var e1 = scheduler.CreateHotObservable(new[] { OnNext(150, 1), OnNext(220, 2), OnCompleted<int>(400) });
            var e2 = scheduler.CreateHotObservable(new[] { OnNext(150, 1), OnNext(230, 3), OnCompleted<int>(400) });
            var e3 = scheduler.CreateHotObservable(new[] { OnNext(150, 1), OnNext(240, 4), OnCompleted<int>(400) });
            var e4 = scheduler.CreateHotObservable(new[] { OnNext(150, 1), OnNext(250, 5), OnCompleted<int>(400) });
            var e5 = scheduler.CreateHotObservable(new[] { OnNext(150, 1), OnNext(260, 6), OnCompleted<int>(400) });
            var e6 = scheduler.CreateHotObservable(new[] { OnNext(150, 1), OnNext(270, 7), OnCompleted<int>(400) });
            var e7 = scheduler.CreateHotObservable(new[] { OnNext(150, 1), OnNext(280, 8), OnCompleted<int>(400) });
            var e8 = scheduler.CreateHotObservable(new[] { OnNext(150, 1), OnNext(290, 9), OnCompleted<int>(400) });
            var e9 = scheduler.CreateHotObservable(new[] { OnNext(150, 1), OnNext(300, 10), OnCompleted<int>(400) });
            var e10 = scheduler.CreateHotObservable(new[] { OnNext(150, 1), OnNext(310, 11), OnCompleted<int>(400) });
            var e11 = scheduler.CreateHotObservable(new[] { OnNext(150, 1), OnNext(320, 12), OnCompleted<int>(400) });
            var e12 = scheduler.CreateHotObservable(new[] { OnNext(150, 1), OnNext(330, 13), OnCompleted<int>(400) });

            var res = scheduler.Start(() =>
                Observable.Zip(e0, e1, e2, e3, e4, e5, e6, e7, e8, e9, e10, e11, e12, (_0, _1, _2, _3, _4, _5, _6, _7, _8, _9, _10, _11, _12) => _0 + _1 + _2 + _3 + _4 + _5 + _6 + _7 + _8 + _9 + _10 + _11 + _12)
            );
            
            res.Messages.AssertEqual(
                OnNext(330, 91),
                OnCompleted<int>(400)
            );

            foreach (var e in new[] { e0, e1, e2, e3, e4, e5, e6, e7, e8, e9, e10, e11, e12 })
                e.Subscriptions.AssertEqual(Subscribe(200, 400));
        }

        [TestMethod]
        public void Zip_SymmetricReturn14()
        {
            var scheduler = new TestScheduler();

            var e0 = scheduler.CreateHotObservable(new[] { OnNext(150, 1), OnNext(210, 1), OnCompleted<int>(400) });
            var e1 = scheduler.CreateHotObservable(new[] { OnNext(150, 1), OnNext(220, 2), OnCompleted<int>(400) });
            var e2 = scheduler.CreateHotObservable(new[] { OnNext(150, 1), OnNext(230, 3), OnCompleted<int>(400) });
            var e3 = scheduler.CreateHotObservable(new[] { OnNext(150, 1), OnNext(240, 4), OnCompleted<int>(400) });
            var e4 = scheduler.CreateHotObservable(new[] { OnNext(150, 1), OnNext(250, 5), OnCompleted<int>(400) });
            var e5 = scheduler.CreateHotObservable(new[] { OnNext(150, 1), OnNext(260, 6), OnCompleted<int>(400) });
            var e6 = scheduler.CreateHotObservable(new[] { OnNext(150, 1), OnNext(270, 7), OnCompleted<int>(400) });
            var e7 = scheduler.CreateHotObservable(new[] { OnNext(150, 1), OnNext(280, 8), OnCompleted<int>(400) });
            var e8 = scheduler.CreateHotObservable(new[] { OnNext(150, 1), OnNext(290, 9), OnCompleted<int>(400) });
            var e9 = scheduler.CreateHotObservable(new[] { OnNext(150, 1), OnNext(300, 10), OnCompleted<int>(400) });
            var e10 = scheduler.CreateHotObservable(new[] { OnNext(150, 1), OnNext(310, 11), OnCompleted<int>(400) });
            var e11 = scheduler.CreateHotObservable(new[] { OnNext(150, 1), OnNext(320, 12), OnCompleted<int>(400) });
            var e12 = scheduler.CreateHotObservable(new[] { OnNext(150, 1), OnNext(330, 13), OnCompleted<int>(400) });
            var e13 = scheduler.CreateHotObservable(new[] { OnNext(150, 1), OnNext(340, 14), OnCompleted<int>(400) });

            var res = scheduler.Start(() =>
                Observable.Zip(e0, e1, e2, e3, e4, e5, e6, e7, e8, e9, e10, e11, e12, e13, (_0, _1, _2, _3, _4, _5, _6, _7, _8, _9, _10, _11, _12, _13) => _0 + _1 + _2 + _3 + _4 + _5 + _6 + _7 + _8 + _9 + _10 + _11 + _12 + _13)
            );
            
            res.Messages.AssertEqual(
                OnNext(340, 105),
                OnCompleted<int>(400)
            );

            foreach (var e in new[] { e0, e1, e2, e3, e4, e5, e6, e7, e8, e9, e10, e11, e12, e13 })
                e.Subscriptions.AssertEqual(Subscribe(200, 400));
        }

        [TestMethod]
        public void Zip_SymmetricReturn15()
        {
            var scheduler = new TestScheduler();

            var e0 = scheduler.CreateHotObservable(new[] { OnNext(150, 1), OnNext(210, 1), OnCompleted<int>(400) });
            var e1 = scheduler.CreateHotObservable(new[] { OnNext(150, 1), OnNext(220, 2), OnCompleted<int>(400) });
            var e2 = scheduler.CreateHotObservable(new[] { OnNext(150, 1), OnNext(230, 3), OnCompleted<int>(400) });
            var e3 = scheduler.CreateHotObservable(new[] { OnNext(150, 1), OnNext(240, 4), OnCompleted<int>(400) });
            var e4 = scheduler.CreateHotObservable(new[] { OnNext(150, 1), OnNext(250, 5), OnCompleted<int>(400) });
            var e5 = scheduler.CreateHotObservable(new[] { OnNext(150, 1), OnNext(260, 6), OnCompleted<int>(400) });
            var e6 = scheduler.CreateHotObservable(new[] { OnNext(150, 1), OnNext(270, 7), OnCompleted<int>(400) });
            var e7 = scheduler.CreateHotObservable(new[] { OnNext(150, 1), OnNext(280, 8), OnCompleted<int>(400) });
            var e8 = scheduler.CreateHotObservable(new[] { OnNext(150, 1), OnNext(290, 9), OnCompleted<int>(400) });
            var e9 = scheduler.CreateHotObservable(new[] { OnNext(150, 1), OnNext(300, 10), OnCompleted<int>(400) });
            var e10 = scheduler.CreateHotObservable(new[] { OnNext(150, 1), OnNext(310, 11), OnCompleted<int>(400) });
            var e11 = scheduler.CreateHotObservable(new[] { OnNext(150, 1), OnNext(320, 12), OnCompleted<int>(400) });
            var e12 = scheduler.CreateHotObservable(new[] { OnNext(150, 1), OnNext(330, 13), OnCompleted<int>(400) });
            var e13 = scheduler.CreateHotObservable(new[] { OnNext(150, 1), OnNext(340, 14), OnCompleted<int>(400) });
            var e14 = scheduler.CreateHotObservable(new[] { OnNext(150, 1), OnNext(350, 15), OnCompleted<int>(400) });

            var res = scheduler.Start(() =>
                Observable.Zip(e0, e1, e2, e3, e4, e5, e6, e7, e8, e9, e10, e11, e12, e13, e14, (_0, _1, _2, _3, _4, _5, _6, _7, _8, _9, _10, _11, _12, _13, _14) => _0 + _1 + _2 + _3 + _4 + _5 + _6 + _7 + _8 + _9 + _10 + _11 + _12 + _13 + _14)
            );
            
            res.Messages.AssertEqual(
                OnNext(350, 120),
                OnCompleted<int>(400)
            );

            foreach (var e in new[] { e0, e1, e2, e3, e4, e5, e6, e7, e8, e9, e10, e11, e12, e13, e14 })
                e.Subscriptions.AssertEqual(Subscribe(200, 400));
        }

        [TestMethod]
        public void Zip_SymmetricReturn16()
        {
            var scheduler = new TestScheduler();

            var e0 = scheduler.CreateHotObservable(new[] { OnNext(150, 1), OnNext(210, 1), OnCompleted<int>(400) });
            var e1 = scheduler.CreateHotObservable(new[] { OnNext(150, 1), OnNext(220, 2), OnCompleted<int>(400) });
            var e2 = scheduler.CreateHotObservable(new[] { OnNext(150, 1), OnNext(230, 3), OnCompleted<int>(400) });
            var e3 = scheduler.CreateHotObservable(new[] { OnNext(150, 1), OnNext(240, 4), OnCompleted<int>(400) });
            var e4 = scheduler.CreateHotObservable(new[] { OnNext(150, 1), OnNext(250, 5), OnCompleted<int>(400) });
            var e5 = scheduler.CreateHotObservable(new[] { OnNext(150, 1), OnNext(260, 6), OnCompleted<int>(400) });
            var e6 = scheduler.CreateHotObservable(new[] { OnNext(150, 1), OnNext(270, 7), OnCompleted<int>(400) });
            var e7 = scheduler.CreateHotObservable(new[] { OnNext(150, 1), OnNext(280, 8), OnCompleted<int>(400) });
            var e8 = scheduler.CreateHotObservable(new[] { OnNext(150, 1), OnNext(290, 9), OnCompleted<int>(400) });
            var e9 = scheduler.CreateHotObservable(new[] { OnNext(150, 1), OnNext(300, 10), OnCompleted<int>(400) });
            var e10 = scheduler.CreateHotObservable(new[] { OnNext(150, 1), OnNext(310, 11), OnCompleted<int>(400) });
            var e11 = scheduler.CreateHotObservable(new[] { OnNext(150, 1), OnNext(320, 12), OnCompleted<int>(400) });
            var e12 = scheduler.CreateHotObservable(new[] { OnNext(150, 1), OnNext(330, 13), OnCompleted<int>(400) });
            var e13 = scheduler.CreateHotObservable(new[] { OnNext(150, 1), OnNext(340, 14), OnCompleted<int>(400) });
            var e14 = scheduler.CreateHotObservable(new[] { OnNext(150, 1), OnNext(350, 15), OnCompleted<int>(400) });
            var e15 = scheduler.CreateHotObservable(new[] { OnNext(150, 1), OnNext(360, 16), OnCompleted<int>(400) });

            var res = scheduler.Start(() =>
                Observable.Zip(e0, e1, e2, e3, e4, e5, e6, e7, e8, e9, e10, e11, e12, e13, e14, e15, (_0, _1, _2, _3, _4, _5, _6, _7, _8, _9, _10, _11, _12, _13, _14, _15) => _0 + _1 + _2 + _3 + _4 + _5 + _6 + _7 + _8 + _9 + _10 + _11 + _12 + _13 + _14 + _15)
            );
            
            res.Messages.AssertEqual(
                OnNext(360, 136),
                OnCompleted<int>(400)
            );

            foreach (var e in new[] { e0, e1, e2, e3, e4, e5, e6, e7, e8, e9, e10, e11, e12, e13, e14, e15 })
                e.Subscriptions.AssertEqual(Subscribe(200, 400));
        }
#endif

        #endregion

        #region Various

        [TestMethod]
        public void Zip_SomeDataAsymmetric1()
        {
            var scheduler = new TestScheduler();

            var msgs1 = Enumerable.Range(0, 5).Select((x, i) => OnNext((ushort)(205 + i * 5), x)).ToArray();
            var msgs2 = Enumerable.Range(0, 10).Select((x, i) => OnNext((ushort)(202 + i * 8), x)).ToArray();

            int len = Math.Min(msgs1.Length, msgs2.Length);

            var o1 = scheduler.CreateHotObservable(msgs1);
            var o2 = scheduler.CreateHotObservable(msgs2);

            var res = scheduler.Start(() =>
                o1.Zip(o2, (x, y) => x + y)
            );

            Assert.AreEqual(len, res.Messages.Count, "length");
            for (int i = 0; i < len; i++)
            {
                var sum = msgs1[i].Value.Value + msgs2[i].Value.Value;
                var time = Math.Max(msgs1[i].Time, msgs2[i].Time);

                Assert.IsTrue(res.Messages[i].Time == time);
                Assert.IsTrue(res.Messages[i].Value.Kind == NotificationKind.OnNext);
                Assert.IsTrue(res.Messages[i].Value.Value == sum, i.ToString());
            }
        }

        [TestMethod]
        public void Zip_SomeDataAsymmetric2()
        {
            var scheduler = new TestScheduler();

            var msgs1 = Enumerable.Range(0, 10).Select((x, i) => OnNext((ushort)(205 + i * 5), x)).ToArray();
            var msgs2 = Enumerable.Range(0, 5).Select((x, i) => OnNext((ushort)(202 + i * 8), x)).ToArray();

            int len = Math.Min(msgs1.Length, msgs2.Length);

            var o1 = scheduler.CreateHotObservable(msgs1);
            var o2 = scheduler.CreateHotObservable(msgs2);

            var res = scheduler.Start(() =>
                o1.Zip(o2, (x, y) => x + y)
            );

            Assert.AreEqual(len, res.Messages.Count, "length");
            for (int i = 0; i < len; i++)
            {
                var sum = msgs1[i].Value.Value + msgs2[i].Value.Value;
                var time = Math.Max(msgs1[i].Time, msgs2[i].Time);

                Assert.IsTrue(res.Messages[i].Time == time);
                Assert.IsTrue(res.Messages[i].Value.Kind == NotificationKind.OnNext);
                Assert.IsTrue(res.Messages[i].Value.Value == sum, i.ToString());
            }
        }

        [TestMethod]
        public void Zip_SomeDataSymmetric()
        {
            var scheduler = new TestScheduler();

            var msgs1 = Enumerable.Range(0, 10).Select((x, i) => OnNext((ushort)(205 + i * 5), x)).ToArray();
            var msgs2 = Enumerable.Range(0, 10).Select((x, i) => OnNext((ushort)(202 + i * 8), x)).ToArray();

            int len = Math.Min(msgs1.Length, msgs2.Length);

            var o1 = scheduler.CreateHotObservable(msgs1);
            var o2 = scheduler.CreateHotObservable(msgs2);

            var res = scheduler.Start(() =>
                o1.Zip(o2, (x, y) => x + y)
            );

            Assert.AreEqual(len, res.Messages.Count, "length");
            for (int i = 0; i < len; i++)
            {
                var sum = msgs1[i].Value.Value + msgs2[i].Value.Value;
                var time = Math.Max(msgs1[i].Time, msgs2[i].Time);

                Assert.IsTrue(res.Messages[i].Time == time);
                Assert.IsTrue(res.Messages[i].Value.Kind == NotificationKind.OnNext);
                Assert.IsTrue(res.Messages[i].Value.Value == sum, i.ToString());
            }
        }

        #endregion

        #region SelectorThrows

        [TestMethod]
        public void Zip_SelectorThrows()
        {
            var scheduler = new TestScheduler();

            var o1 = scheduler.CreateHotObservable(
                OnNext(150, 1),
                OnNext(215, 2),
                OnNext(225, 4),
                OnCompleted<int>(240)
            );

            var o2 = scheduler.CreateHotObservable(
                OnNext(150, 1),
                OnNext(220, 3),
                OnNext(230, 5), //!
                OnCompleted<int>(250)
            );

            var ex = new Exception();

            var res = scheduler.Start(() =>
                o1.Zip(o2, (x, y) =>
                {
                    if (y == 5)
                        throw ex;
                    return x + y;
                })
            );

            res.Messages.AssertEqual(
                OnNext(220, 2 + 3),
                OnError<int>(230, ex)
            );
        }

        [TestMethod]
        public void Zip_SelectorThrows2()
        {
            var scheduler = new TestScheduler();

            var e0 = scheduler.CreateHotObservable(new[] { OnNext(150, 1), OnNext(210, 1), OnCompleted<int>(400) });
            var e1 = scheduler.CreateHotObservable(new[] { OnNext(150, 1), OnNext(220, 2), OnCompleted<int>(400) });

            var ex = new Exception();
            Func<int> f = () => { throw ex; };

            var res = scheduler.Start(() =>
                Observable.Zip(e0, e1, (_0, _1) => f())
            );
            
            res.Messages.AssertEqual(
                OnError<int>(220, ex)
            );

            var es = new[] { e0, e1 };
            foreach (var e in es)
                e.Subscriptions.AssertEqual(Subscribe(200, 200 + es.Length * 10));
        }

        [TestMethod]
        public void Zip_SelectorThrows3()
        {
            var scheduler = new TestScheduler();
            
            var e0 = scheduler.CreateHotObservable(new[] { OnNext(150, 1), OnNext(210, 1), OnCompleted<int>(400) });
            var e1 = scheduler.CreateHotObservable(new[] { OnNext(150, 1), OnNext(220, 2), OnCompleted<int>(400) });
            var e2 = scheduler.CreateHotObservable(new[] { OnNext(150, 1), OnNext(230, 3), OnCompleted<int>(400) });
            
            var ex = new Exception();
            Func<int> f = () => { throw ex; };

            var res = scheduler.Start(() =>
                Observable.Zip(e0, e1, e2, (_0, _1, _2) => f())
            );
            
            res.Messages.AssertEqual(
                OnError<int>(230, ex)
            );

            var es = new[] { e0, e1, e2 };
            foreach (var e in es)
                e.Subscriptions.AssertEqual(Subscribe(200, 200 + es.Length * 10));
        }

        [TestMethod]
        public void Zip_SelectorThrows4()
        {
            var scheduler = new TestScheduler();
            
            var e0 = scheduler.CreateHotObservable(new[] { OnNext(150, 1), OnNext(210, 1), OnCompleted<int>(400) });
            var e1 = scheduler.CreateHotObservable(new[] { OnNext(150, 1), OnNext(220, 2), OnCompleted<int>(400) });
            var e2 = scheduler.CreateHotObservable(new[] { OnNext(150, 1), OnNext(230, 3), OnCompleted<int>(400) });
            var e3 = scheduler.CreateHotObservable(new[] { OnNext(150, 1), OnNext(240, 4), OnCompleted<int>(400) });
            
            var ex = new Exception();
            Func<int> f = () => { throw ex; };

            var res = scheduler.Start(() =>
                Observable.Zip(e0, e1, e2, e3, (_0, _1, _2, _3) => f())
            );
            
            res.Messages.AssertEqual(
                OnError<int>(240, ex)
            );

            var es = new[] { e0, e1, e2, e3 };
            foreach (var e in es)
                e.Subscriptions.AssertEqual(Subscribe(200, 200 + es.Length * 10));
        }

#if !NO_LARGEARITY
        [TestMethod]
        public void Zip_SelectorThrows5()
        {
            var scheduler = new TestScheduler();
            
            var e0 = scheduler.CreateHotObservable(new[] { OnNext(150, 1), OnNext(210, 1), OnCompleted<int>(400) });
            var e1 = scheduler.CreateHotObservable(new[] { OnNext(150, 1), OnNext(220, 2), OnCompleted<int>(400) });
            var e2 = scheduler.CreateHotObservable(new[] { OnNext(150, 1), OnNext(230, 3), OnCompleted<int>(400) });
            var e3 = scheduler.CreateHotObservable(new[] { OnNext(150, 1), OnNext(240, 4), OnCompleted<int>(400) });
            var e4 = scheduler.CreateHotObservable(new[] { OnNext(150, 1), OnNext(250, 5), OnCompleted<int>(400) });
            
            var ex = new Exception();
            Func<int> f = () => { throw ex; };

            var res = scheduler.Start(() =>
                Observable.Zip(e0, e1, e2, e3, e4, (_0, _1, _2, _3, _4) => f())
            );
            
            res.Messages.AssertEqual(
                OnError<int>(250, ex)
            );

            var es = new[] { e0, e1, e2, e3, e4 };
            foreach (var e in es)
                e.Subscriptions.AssertEqual(Subscribe(200, 200 + es.Length * 10));
        }

        [TestMethod]
        public void Zip_SelectorThrows6()
        {
            var scheduler = new TestScheduler();
            
            var e0 = scheduler.CreateHotObservable(new[] { OnNext(150, 1), OnNext(210, 1), OnCompleted<int>(400) });
            var e1 = scheduler.CreateHotObservable(new[] { OnNext(150, 1), OnNext(220, 2), OnCompleted<int>(400) });
            var e2 = scheduler.CreateHotObservable(new[] { OnNext(150, 1), OnNext(230, 3), OnCompleted<int>(400) });
            var e3 = scheduler.CreateHotObservable(new[] { OnNext(150, 1), OnNext(240, 4), OnCompleted<int>(400) });
            var e4 = scheduler.CreateHotObservable(new[] { OnNext(150, 1), OnNext(250, 5), OnCompleted<int>(400) });
            var e5 = scheduler.CreateHotObservable(new[] { OnNext(150, 1), OnNext(260, 6), OnCompleted<int>(400) });
            
            var ex = new Exception();
            Func<int> f = () => { throw ex; };

            var res = scheduler.Start(() =>
                Observable.Zip(e0, e1, e2, e3, e4, e5, (_0, _1, _2, _3, _4, _5) => f())
            );
            
            res.Messages.AssertEqual(
                OnError<int>(260, ex)
            );

            var es = new[] { e0, e1, e2, e3, e4, e5 };
            foreach (var e in es)
                e.Subscriptions.AssertEqual(Subscribe(200, 200 + es.Length * 10));
        }

        [TestMethod]
        public void Zip_SelectorThrows7()
        {
            var scheduler = new TestScheduler();
            
            var e0 = scheduler.CreateHotObservable(new[] { OnNext(150, 1), OnNext(210, 1), OnCompleted<int>(400) });
            var e1 = scheduler.CreateHotObservable(new[] { OnNext(150, 1), OnNext(220, 2), OnCompleted<int>(400) });
            var e2 = scheduler.CreateHotObservable(new[] { OnNext(150, 1), OnNext(230, 3), OnCompleted<int>(400) });
            var e3 = scheduler.CreateHotObservable(new[] { OnNext(150, 1), OnNext(240, 4), OnCompleted<int>(400) });
            var e4 = scheduler.CreateHotObservable(new[] { OnNext(150, 1), OnNext(250, 5), OnCompleted<int>(400) });
            var e5 = scheduler.CreateHotObservable(new[] { OnNext(150, 1), OnNext(260, 6), OnCompleted<int>(400) });
            var e6 = scheduler.CreateHotObservable(new[] { OnNext(150, 1), OnNext(270, 7), OnCompleted<int>(400) });
            
            var ex = new Exception();
            Func<int> f = () => { throw ex; };

            var res = scheduler.Start(() =>
                Observable.Zip(e0, e1, e2, e3, e4, e5, e6, (_0, _1, _2, _3, _4, _5, _6) => f())
            );
            
            res.Messages.AssertEqual(
                OnError<int>(270, ex)
            );

            var es = new[] { e0, e1, e2, e3, e4, e5, e6 };
            foreach (var e in es)
                e.Subscriptions.AssertEqual(Subscribe(200, 200 + es.Length * 10));
        }

        [TestMethod]
        public void Zip_SelectorThrows8()
        {
            var scheduler = new TestScheduler();
            
            var e0 = scheduler.CreateHotObservable(new[] { OnNext(150, 1), OnNext(210, 1), OnCompleted<int>(400) });
            var e1 = scheduler.CreateHotObservable(new[] { OnNext(150, 1), OnNext(220, 2), OnCompleted<int>(400) });
            var e2 = scheduler.CreateHotObservable(new[] { OnNext(150, 1), OnNext(230, 3), OnCompleted<int>(400) });
            var e3 = scheduler.CreateHotObservable(new[] { OnNext(150, 1), OnNext(240, 4), OnCompleted<int>(400) });
            var e4 = scheduler.CreateHotObservable(new[] { OnNext(150, 1), OnNext(250, 5), OnCompleted<int>(400) });
            var e5 = scheduler.CreateHotObservable(new[] { OnNext(150, 1), OnNext(260, 6), OnCompleted<int>(400) });
            var e6 = scheduler.CreateHotObservable(new[] { OnNext(150, 1), OnNext(270, 7), OnCompleted<int>(400) });
            var e7 = scheduler.CreateHotObservable(new[] { OnNext(150, 1), OnNext(280, 8), OnCompleted<int>(400) });
            
            var ex = new Exception();
            Func<int> f = () => { throw ex; };

            var res = scheduler.Start(() =>
                Observable.Zip(e0, e1, e2, e3, e4, e5, e6, e7, (_0, _1, _2, _3, _4, _5, _6, _7) => f())
            );
            
            res.Messages.AssertEqual(
                OnError<int>(280, ex)
            );

            var es = new[] { e0, e1, e2, e3, e4, e5, e6, e7 };
            foreach (var e in es)
                e.Subscriptions.AssertEqual(Subscribe(200, 200 + es.Length * 10));
        }

        [TestMethod]
        public void Zip_SelectorThrows9()
        {
            var scheduler = new TestScheduler();
            
            var e0 = scheduler.CreateHotObservable(new[] { OnNext(150, 1), OnNext(210, 1), OnCompleted<int>(400) });
            var e1 = scheduler.CreateHotObservable(new[] { OnNext(150, 1), OnNext(220, 2), OnCompleted<int>(400) });
            var e2 = scheduler.CreateHotObservable(new[] { OnNext(150, 1), OnNext(230, 3), OnCompleted<int>(400) });
            var e3 = scheduler.CreateHotObservable(new[] { OnNext(150, 1), OnNext(240, 4), OnCompleted<int>(400) });
            var e4 = scheduler.CreateHotObservable(new[] { OnNext(150, 1), OnNext(250, 5), OnCompleted<int>(400) });
            var e5 = scheduler.CreateHotObservable(new[] { OnNext(150, 1), OnNext(260, 6), OnCompleted<int>(400) });
            var e6 = scheduler.CreateHotObservable(new[] { OnNext(150, 1), OnNext(270, 7), OnCompleted<int>(400) });
            var e7 = scheduler.CreateHotObservable(new[] { OnNext(150, 1), OnNext(280, 8), OnCompleted<int>(400) });
            var e8 = scheduler.CreateHotObservable(new[] { OnNext(150, 1), OnNext(290, 9), OnCompleted<int>(400) });
            
            var ex = new Exception();
            Func<int> f = () => { throw ex; };

            var res = scheduler.Start(() =>
                Observable.Zip(e0, e1, e2, e3, e4, e5, e6, e7, e8, (_0, _1, _2, _3, _4, _5, _6, _7, _8) => f())
            );
            
            res.Messages.AssertEqual(
                OnError<int>(290, ex)
            );

            var es = new[] { e0, e1, e2, e3, e4, e5, e6, e7, e8 };
            foreach (var e in es)
                e.Subscriptions.AssertEqual(Subscribe(200, 200 + es.Length * 10));
        }

        [TestMethod]
        public void Zip_SelectorThrows10()
        {
            var scheduler = new TestScheduler();
            
            var e0 = scheduler.CreateHotObservable(new[] { OnNext(150, 1), OnNext(210, 1), OnCompleted<int>(400) });
            var e1 = scheduler.CreateHotObservable(new[] { OnNext(150, 1), OnNext(220, 2), OnCompleted<int>(400) });
            var e2 = scheduler.CreateHotObservable(new[] { OnNext(150, 1), OnNext(230, 3), OnCompleted<int>(400) });
            var e3 = scheduler.CreateHotObservable(new[] { OnNext(150, 1), OnNext(240, 4), OnCompleted<int>(400) });
            var e4 = scheduler.CreateHotObservable(new[] { OnNext(150, 1), OnNext(250, 5), OnCompleted<int>(400) });
            var e5 = scheduler.CreateHotObservable(new[] { OnNext(150, 1), OnNext(260, 6), OnCompleted<int>(400) });
            var e6 = scheduler.CreateHotObservable(new[] { OnNext(150, 1), OnNext(270, 7), OnCompleted<int>(400) });
            var e7 = scheduler.CreateHotObservable(new[] { OnNext(150, 1), OnNext(280, 8), OnCompleted<int>(400) });
            var e8 = scheduler.CreateHotObservable(new[] { OnNext(150, 1), OnNext(290, 9), OnCompleted<int>(400) });
            var e9 = scheduler.CreateHotObservable(new[] { OnNext(150, 1), OnNext(300, 10), OnCompleted<int>(400) });
            
            var ex = new Exception();
            Func<int> f = () => { throw ex; };

            var res = scheduler.Start(() =>
                Observable.Zip(e0, e1, e2, e3, e4, e5, e6, e7, e8, e9, (_0, _1, _2, _3, _4, _5, _6, _7, _8, _9) => f())
            );
            
            res.Messages.AssertEqual(
                OnError<int>(300, ex)
            );

            var es = new[] { e0, e1, e2, e3, e4, e5, e6, e7, e8, e9 };
            foreach (var e in es)
                e.Subscriptions.AssertEqual(Subscribe(200, 200 + es.Length * 10));
        }

        [TestMethod]
        public void Zip_SelectorThrows11()
        {
            var scheduler = new TestScheduler();
            
            var e0 = scheduler.CreateHotObservable(new[] { OnNext(150, 1), OnNext(210, 1), OnCompleted<int>(400) });
            var e1 = scheduler.CreateHotObservable(new[] { OnNext(150, 1), OnNext(220, 2), OnCompleted<int>(400) });
            var e2 = scheduler.CreateHotObservable(new[] { OnNext(150, 1), OnNext(230, 3), OnCompleted<int>(400) });
            var e3 = scheduler.CreateHotObservable(new[] { OnNext(150, 1), OnNext(240, 4), OnCompleted<int>(400) });
            var e4 = scheduler.CreateHotObservable(new[] { OnNext(150, 1), OnNext(250, 5), OnCompleted<int>(400) });
            var e5 = scheduler.CreateHotObservable(new[] { OnNext(150, 1), OnNext(260, 6), OnCompleted<int>(400) });
            var e6 = scheduler.CreateHotObservable(new[] { OnNext(150, 1), OnNext(270, 7), OnCompleted<int>(400) });
            var e7 = scheduler.CreateHotObservable(new[] { OnNext(150, 1), OnNext(280, 8), OnCompleted<int>(400) });
            var e8 = scheduler.CreateHotObservable(new[] { OnNext(150, 1), OnNext(290, 9), OnCompleted<int>(400) });
            var e9 = scheduler.CreateHotObservable(new[] { OnNext(150, 1), OnNext(300, 10), OnCompleted<int>(400) });
            var e10 = scheduler.CreateHotObservable(new[] { OnNext(150, 1), OnNext(310, 11), OnCompleted<int>(400) });
            
            var ex = new Exception();
            Func<int> f = () => { throw ex; };

            var res = scheduler.Start(() =>
                Observable.Zip(e0, e1, e2, e3, e4, e5, e6, e7, e8, e9, e10, (_0, _1, _2, _3, _4, _5, _6, _7, _8, _9, _10) => f())
            );
            
            res.Messages.AssertEqual(
                OnError<int>(310, ex)
            );

            var es = new[] { e0, e1, e2, e3, e4, e5, e6, e7, e8, e9, e10 };
            foreach (var e in es)
                e.Subscriptions.AssertEqual(Subscribe(200, 200 + es.Length * 10));
        }

        [TestMethod]
        public void Zip_SelectorThrows12()
        {
            var scheduler = new TestScheduler();
            
            var e0 = scheduler.CreateHotObservable(new[] { OnNext(150, 1), OnNext(210, 1), OnCompleted<int>(400) });
            var e1 = scheduler.CreateHotObservable(new[] { OnNext(150, 1), OnNext(220, 2), OnCompleted<int>(400) });
            var e2 = scheduler.CreateHotObservable(new[] { OnNext(150, 1), OnNext(230, 3), OnCompleted<int>(400) });
            var e3 = scheduler.CreateHotObservable(new[] { OnNext(150, 1), OnNext(240, 4), OnCompleted<int>(400) });
            var e4 = scheduler.CreateHotObservable(new[] { OnNext(150, 1), OnNext(250, 5), OnCompleted<int>(400) });
            var e5 = scheduler.CreateHotObservable(new[] { OnNext(150, 1), OnNext(260, 6), OnCompleted<int>(400) });
            var e6 = scheduler.CreateHotObservable(new[] { OnNext(150, 1), OnNext(270, 7), OnCompleted<int>(400) });
            var e7 = scheduler.CreateHotObservable(new[] { OnNext(150, 1), OnNext(280, 8), OnCompleted<int>(400) });
            var e8 = scheduler.CreateHotObservable(new[] { OnNext(150, 1), OnNext(290, 9), OnCompleted<int>(400) });
            var e9 = scheduler.CreateHotObservable(new[] { OnNext(150, 1), OnNext(300, 10), OnCompleted<int>(400) });
            var e10 = scheduler.CreateHotObservable(new[] { OnNext(150, 1), OnNext(310, 11), OnCompleted<int>(400) });
            var e11 = scheduler.CreateHotObservable(new[] { OnNext(150, 1), OnNext(320, 12), OnCompleted<int>(400) });
            
            var ex = new Exception();
            Func<int> f = () => { throw ex; };

            var res = scheduler.Start(() =>
                Observable.Zip(e0, e1, e2, e3, e4, e5, e6, e7, e8, e9, e10, e11, (_0, _1, _2, _3, _4, _5, _6, _7, _8, _9, _10, _11) => f())
            );
            
            res.Messages.AssertEqual(
                OnError<int>(320, ex)
            );

            var es = new[] { e0, e1, e2, e3, e4, e5, e6, e7, e8, e9, e10, e11 };
            foreach (var e in es)
                e.Subscriptions.AssertEqual(Subscribe(200, 200 + es.Length * 10));
        }

        [TestMethod]
        public void Zip_SelectorThrows13()
        {
            var scheduler = new TestScheduler();
            
            var e0 = scheduler.CreateHotObservable(new[] { OnNext(150, 1), OnNext(210, 1), OnCompleted<int>(400) });
            var e1 = scheduler.CreateHotObservable(new[] { OnNext(150, 1), OnNext(220, 2), OnCompleted<int>(400) });
            var e2 = scheduler.CreateHotObservable(new[] { OnNext(150, 1), OnNext(230, 3), OnCompleted<int>(400) });
            var e3 = scheduler.CreateHotObservable(new[] { OnNext(150, 1), OnNext(240, 4), OnCompleted<int>(400) });
            var e4 = scheduler.CreateHotObservable(new[] { OnNext(150, 1), OnNext(250, 5), OnCompleted<int>(400) });
            var e5 = scheduler.CreateHotObservable(new[] { OnNext(150, 1), OnNext(260, 6), OnCompleted<int>(400) });
            var e6 = scheduler.CreateHotObservable(new[] { OnNext(150, 1), OnNext(270, 7), OnCompleted<int>(400) });
            var e7 = scheduler.CreateHotObservable(new[] { OnNext(150, 1), OnNext(280, 8), OnCompleted<int>(400) });
            var e8 = scheduler.CreateHotObservable(new[] { OnNext(150, 1), OnNext(290, 9), OnCompleted<int>(400) });
            var e9 = scheduler.CreateHotObservable(new[] { OnNext(150, 1), OnNext(300, 10), OnCompleted<int>(400) });
            var e10 = scheduler.CreateHotObservable(new[] { OnNext(150, 1), OnNext(310, 11), OnCompleted<int>(400) });
            var e11 = scheduler.CreateHotObservable(new[] { OnNext(150, 1), OnNext(320, 12), OnCompleted<int>(400) });
            var e12 = scheduler.CreateHotObservable(new[] { OnNext(150, 1), OnNext(330, 13), OnCompleted<int>(400) });
            
            var ex = new Exception();
            Func<int> f = () => { throw ex; };

            var res = scheduler.Start(() =>
                Observable.Zip(e0, e1, e2, e3, e4, e5, e6, e7, e8, e9, e10, e11, e12, (_0, _1, _2, _3, _4, _5, _6, _7, _8, _9, _10, _11, _12) => f())
            );
            
            res.Messages.AssertEqual(
                OnError<int>(330, ex)
            );

            var es = new[] { e0, e1, e2, e3, e4, e5, e6, e7, e8, e9, e10, e11, e12 };
            foreach (var e in es)
                e.Subscriptions.AssertEqual(Subscribe(200, 200 + es.Length * 10));
        }

        [TestMethod]
        public void Zip_SelectorThrows14()
        {
            var scheduler = new TestScheduler();
            
            var e0 = scheduler.CreateHotObservable(new[] { OnNext(150, 1), OnNext(210, 1), OnCompleted<int>(400) });
            var e1 = scheduler.CreateHotObservable(new[] { OnNext(150, 1), OnNext(220, 2), OnCompleted<int>(400) });
            var e2 = scheduler.CreateHotObservable(new[] { OnNext(150, 1), OnNext(230, 3), OnCompleted<int>(400) });
            var e3 = scheduler.CreateHotObservable(new[] { OnNext(150, 1), OnNext(240, 4), OnCompleted<int>(400) });
            var e4 = scheduler.CreateHotObservable(new[] { OnNext(150, 1), OnNext(250, 5), OnCompleted<int>(400) });
            var e5 = scheduler.CreateHotObservable(new[] { OnNext(150, 1), OnNext(260, 6), OnCompleted<int>(400) });
            var e6 = scheduler.CreateHotObservable(new[] { OnNext(150, 1), OnNext(270, 7), OnCompleted<int>(400) });
            var e7 = scheduler.CreateHotObservable(new[] { OnNext(150, 1), OnNext(280, 8), OnCompleted<int>(400) });
            var e8 = scheduler.CreateHotObservable(new[] { OnNext(150, 1), OnNext(290, 9), OnCompleted<int>(400) });
            var e9 = scheduler.CreateHotObservable(new[] { OnNext(150, 1), OnNext(300, 10), OnCompleted<int>(400) });
            var e10 = scheduler.CreateHotObservable(new[] { OnNext(150, 1), OnNext(310, 11), OnCompleted<int>(400) });
            var e11 = scheduler.CreateHotObservable(new[] { OnNext(150, 1), OnNext(320, 12), OnCompleted<int>(400) });
            var e12 = scheduler.CreateHotObservable(new[] { OnNext(150, 1), OnNext(330, 13), OnCompleted<int>(400) });
            var e13 = scheduler.CreateHotObservable(new[] { OnNext(150, 1), OnNext(340, 14), OnCompleted<int>(400) });
            
            var ex = new Exception();
            Func<int> f = () => { throw ex; };

            var res = scheduler.Start(() =>
                Observable.Zip(e0, e1, e2, e3, e4, e5, e6, e7, e8, e9, e10, e11, e12, e13, (_0, _1, _2, _3, _4, _5, _6, _7, _8, _9, _10, _11, _12, _13) => f())
            );
            
            res.Messages.AssertEqual(
                OnError<int>(340, ex)
            );

            var es = new[] { e0, e1, e2, e3, e4, e5, e6, e7, e8, e9, e10, e11, e12, e13 };
            foreach (var e in es)
                e.Subscriptions.AssertEqual(Subscribe(200, 200 + es.Length * 10));
        }

        [TestMethod]
        public void Zip_SelectorThrows15()
        {
            var scheduler = new TestScheduler();
            
            var e0 = scheduler.CreateHotObservable(new[] { OnNext(150, 1), OnNext(210, 1), OnCompleted<int>(400) });
            var e1 = scheduler.CreateHotObservable(new[] { OnNext(150, 1), OnNext(220, 2), OnCompleted<int>(400) });
            var e2 = scheduler.CreateHotObservable(new[] { OnNext(150, 1), OnNext(230, 3), OnCompleted<int>(400) });
            var e3 = scheduler.CreateHotObservable(new[] { OnNext(150, 1), OnNext(240, 4), OnCompleted<int>(400) });
            var e4 = scheduler.CreateHotObservable(new[] { OnNext(150, 1), OnNext(250, 5), OnCompleted<int>(400) });
            var e5 = scheduler.CreateHotObservable(new[] { OnNext(150, 1), OnNext(260, 6), OnCompleted<int>(400) });
            var e6 = scheduler.CreateHotObservable(new[] { OnNext(150, 1), OnNext(270, 7), OnCompleted<int>(400) });
            var e7 = scheduler.CreateHotObservable(new[] { OnNext(150, 1), OnNext(280, 8), OnCompleted<int>(400) });
            var e8 = scheduler.CreateHotObservable(new[] { OnNext(150, 1), OnNext(290, 9), OnCompleted<int>(400) });
            var e9 = scheduler.CreateHotObservable(new[] { OnNext(150, 1), OnNext(300, 10), OnCompleted<int>(400) });
            var e10 = scheduler.CreateHotObservable(new[] { OnNext(150, 1), OnNext(310, 11), OnCompleted<int>(400) });
            var e11 = scheduler.CreateHotObservable(new[] { OnNext(150, 1), OnNext(320, 12), OnCompleted<int>(400) });
            var e12 = scheduler.CreateHotObservable(new[] { OnNext(150, 1), OnNext(330, 13), OnCompleted<int>(400) });
            var e13 = scheduler.CreateHotObservable(new[] { OnNext(150, 1), OnNext(340, 14), OnCompleted<int>(400) });
            var e14 = scheduler.CreateHotObservable(new[] { OnNext(150, 1), OnNext(350, 15), OnCompleted<int>(400) });
            
            var ex = new Exception();
            Func<int> f = () => { throw ex; };

            var res = scheduler.Start(() =>
                Observable.Zip(e0, e1, e2, e3, e4, e5, e6, e7, e8, e9, e10, e11, e12, e13, e14, (_0, _1, _2, _3, _4, _5, _6, _7, _8, _9, _10, _11, _12, _13, _14) => f())
            );
            
            res.Messages.AssertEqual(
                OnError<int>(350, ex)
            );

            var es = new[] { e0, e1, e2, e3, e4, e5, e6, e7, e8, e9, e10, e11, e12, e13, e14 };
            foreach (var e in es)
                e.Subscriptions.AssertEqual(Subscribe(200, 200 + es.Length * 10));
        }

        [TestMethod]
        public void Zip_SelectorThrows16()
        {
            var scheduler = new TestScheduler();
            
            var e0 = scheduler.CreateHotObservable(new[] { OnNext(150, 1), OnNext(210, 1), OnCompleted<int>(400) });
            var e1 = scheduler.CreateHotObservable(new[] { OnNext(150, 1), OnNext(220, 2), OnCompleted<int>(400) });
            var e2 = scheduler.CreateHotObservable(new[] { OnNext(150, 1), OnNext(230, 3), OnCompleted<int>(400) });
            var e3 = scheduler.CreateHotObservable(new[] { OnNext(150, 1), OnNext(240, 4), OnCompleted<int>(400) });
            var e4 = scheduler.CreateHotObservable(new[] { OnNext(150, 1), OnNext(250, 5), OnCompleted<int>(400) });
            var e5 = scheduler.CreateHotObservable(new[] { OnNext(150, 1), OnNext(260, 6), OnCompleted<int>(400) });
            var e6 = scheduler.CreateHotObservable(new[] { OnNext(150, 1), OnNext(270, 7), OnCompleted<int>(400) });
            var e7 = scheduler.CreateHotObservable(new[] { OnNext(150, 1), OnNext(280, 8), OnCompleted<int>(400) });
            var e8 = scheduler.CreateHotObservable(new[] { OnNext(150, 1), OnNext(290, 9), OnCompleted<int>(400) });
            var e9 = scheduler.CreateHotObservable(new[] { OnNext(150, 1), OnNext(300, 10), OnCompleted<int>(400) });
            var e10 = scheduler.CreateHotObservable(new[] { OnNext(150, 1), OnNext(310, 11), OnCompleted<int>(400) });
            var e11 = scheduler.CreateHotObservable(new[] { OnNext(150, 1), OnNext(320, 12), OnCompleted<int>(400) });
            var e12 = scheduler.CreateHotObservable(new[] { OnNext(150, 1), OnNext(330, 13), OnCompleted<int>(400) });
            var e13 = scheduler.CreateHotObservable(new[] { OnNext(150, 1), OnNext(340, 14), OnCompleted<int>(400) });
            var e14 = scheduler.CreateHotObservable(new[] { OnNext(150, 1), OnNext(350, 15), OnCompleted<int>(400) });
            var e15 = scheduler.CreateHotObservable(new[] { OnNext(150, 1), OnNext(360, 16), OnCompleted<int>(400) });
            
            var ex = new Exception();
            Func<int> f = () => { throw ex; };

            var res = scheduler.Start(() =>
                Observable.Zip(e0, e1, e2, e3, e4, e5, e6, e7, e8, e9, e10, e11, e12, e13, e14, e15, (_0, _1, _2, _3, _4, _5, _6, _7, _8, _9, _10, _11, _12, _13, _14, _15) => f())
            );
            
            res.Messages.AssertEqual(
                OnError<int>(360, ex)
            );

            var es = new[] { e0, e1, e2, e3, e4, e5, e6, e7, e8, e9, e10, e11, e12, e13, e14, e15 };
            foreach (var e in es)
                e.Subscriptions.AssertEqual(Subscribe(200, 200 + es.Length * 10));
        }
#endif

        #endregion

        #region GetEnumeratorThrows

        [TestMethod]
        public void Zip_GetEnumeratorThrows()
        {
            var ex = new Exception();

            var scheduler = new TestScheduler();

            var xs = scheduler.CreateHotObservable<int>(
                OnNext(210, 42),
                OnNext(220, 43),
                OnCompleted<int>(230)
            );

            var ys = new RogueEnumerable<int>(ex);

            var res = scheduler.Start(() =>
                xs.Zip(ys, (x, y) => x + y)
            );

            res.Messages.AssertEqual(
                OnError<int>(200, ex)
            );

            xs.Subscriptions.AssertEqual(
            );
        }

        #endregion

        #region AllCompleted

        [TestMethod]
        public void Zip_AllCompleted2()
        {
            var scheduler = new TestScheduler();
            
            var e0 = scheduler.CreateHotObservable(new[] { OnNext(150, 1), OnNext(210, 5), OnCompleted<int>(220) });
            var e1 = scheduler.CreateHotObservable(new[] { OnNext(150, 1), OnNext(210, 5), OnNext(220, 6), OnCompleted<int>(230) });

            var res = scheduler.Start(() =>
                Observable.Zip(e0, e1, (_0, _1) => _0 + _1)
            );
            
            res.Messages.AssertEqual(
                OnNext<int>(210, 10),
                OnCompleted<int>(220)
            );

            var es = new[] { e0, e1 };

            var i = 0;
            foreach (var e in es.Take(es.Length - 1))
                e.Subscriptions.AssertEqual(Subscribe(200, 220 + (i++ * 10)));

            es.Last().Subscriptions.AssertEqual(
                Subscribe(200, 220 + (i - 1) * 10)
            );
        }

        [TestMethod]
        public void Zip_AllCompleted3()
        {
            var scheduler = new TestScheduler();
            
            var e0 = scheduler.CreateHotObservable(new[] { OnNext(150, 1), OnNext(210, 5), OnCompleted<int>(220) });
            var e1 = scheduler.CreateHotObservable(new[] { OnNext(150, 1), OnNext(210, 5), OnNext(220, 6), OnCompleted<int>(230) });
            var e2 = scheduler.CreateHotObservable(new[] { OnNext(150, 1), OnNext(210, 5), OnNext(220, 6), OnNext(230, 7), OnCompleted<int>(240) });

            var res = scheduler.Start(() =>
                Observable.Zip(e0, e1, e2, (_0, _1, _2) => _0 + _1 + _2)
            );
            
            res.Messages.AssertEqual(
                OnNext<int>(210, 15),
                OnCompleted<int>(230)
            );

            var es = new[] { e0, e1, e2 };

            var i = 0;
            foreach (var e in es.Take(es.Length - 1))
                e.Subscriptions.AssertEqual(Subscribe(200, 220 + (i++ * 10)));

            es.Last().Subscriptions.AssertEqual(
                Subscribe(200, 220 + (i - 1) * 10)
            );
        }

        [TestMethod]
        public void Zip_AllCompleted4()
        {
            var scheduler = new TestScheduler();
            
            var e0 = scheduler.CreateHotObservable(new[] { OnNext(150, 1), OnNext(210, 5), OnCompleted<int>(220) });
            var e1 = scheduler.CreateHotObservable(new[] { OnNext(150, 1), OnNext(210, 5), OnNext(220, 6), OnCompleted<int>(230) });
            var e2 = scheduler.CreateHotObservable(new[] { OnNext(150, 1), OnNext(210, 5), OnNext(220, 6), OnNext(230, 7), OnCompleted<int>(240) });
            var e3 = scheduler.CreateHotObservable(new[] { OnNext(150, 1), OnNext(210, 5), OnNext(220, 6), OnNext(230, 7), OnNext(240, 8), OnCompleted<int>(250) });

            var res = scheduler.Start(() =>
                Observable.Zip(e0, e1, e2, e3, (_0, _1, _2, _3) => _0 + _1 + _2 + _3)
            );
            
            res.Messages.AssertEqual(
                OnNext<int>(210, 20),
                OnCompleted<int>(240)
            );

            var es = new[] { e0, e1, e2, e3 };

            var i = 0;
            foreach (var e in es.Take(es.Length - 1))
                e.Subscriptions.AssertEqual(Subscribe(200, 220 + (i++ * 10)));

            es.Last().Subscriptions.AssertEqual(
                Subscribe(200, 220 + (i - 1) * 10)
            );
        }

#if !NO_LARGEARITY
        [TestMethod]
        public void Zip_AllCompleted5()
        {
            var scheduler = new TestScheduler();
            
            var e0 = scheduler.CreateHotObservable(new[] { OnNext(150, 1), OnNext(210, 5), OnCompleted<int>(220) });
            var e1 = scheduler.CreateHotObservable(new[] { OnNext(150, 1), OnNext(210, 5), OnNext(220, 6), OnCompleted<int>(230) });
            var e2 = scheduler.CreateHotObservable(new[] { OnNext(150, 1), OnNext(210, 5), OnNext(220, 6), OnNext(230, 7), OnCompleted<int>(240) });
            var e3 = scheduler.CreateHotObservable(new[] { OnNext(150, 1), OnNext(210, 5), OnNext(220, 6), OnNext(230, 7), OnNext(240, 8), OnCompleted<int>(250) });
            var e4 = scheduler.CreateHotObservable(new[] { OnNext(150, 1), OnNext(210, 5), OnNext(220, 6), OnNext(230, 7), OnNext(240, 8), OnNext(250, 9), OnCompleted<int>(260) });

            var res = scheduler.Start(() =>
                Observable.Zip(e0, e1, e2, e3, e4, (_0, _1, _2, _3, _4) => _0 + _1 + _2 + _3 + _4)
            );
            
            res.Messages.AssertEqual(
                OnNext<int>(210, 25),
                OnCompleted<int>(250)
            );

            var es = new[] { e0, e1, e2, e3, e4 };

            var i = 0;
            foreach (var e in es.Take(es.Length - 1))
                e.Subscriptions.AssertEqual(Subscribe(200, 220 + (i++ * 10)));

            es.Last().Subscriptions.AssertEqual(
                Subscribe(200, 220 + (i - 1) * 10)
            );
        }

        [TestMethod]
        public void Zip_AllCompleted6()
        {
            var scheduler = new TestScheduler();
            
            var e0 = scheduler.CreateHotObservable(new[] { OnNext(150, 1), OnNext(210, 5), OnCompleted<int>(220) });
            var e1 = scheduler.CreateHotObservable(new[] { OnNext(150, 1), OnNext(210, 5), OnNext(220, 6), OnCompleted<int>(230) });
            var e2 = scheduler.CreateHotObservable(new[] { OnNext(150, 1), OnNext(210, 5), OnNext(220, 6), OnNext(230, 7), OnCompleted<int>(240) });
            var e3 = scheduler.CreateHotObservable(new[] { OnNext(150, 1), OnNext(210, 5), OnNext(220, 6), OnNext(230, 7), OnNext(240, 8), OnCompleted<int>(250) });
            var e4 = scheduler.CreateHotObservable(new[] { OnNext(150, 1), OnNext(210, 5), OnNext(220, 6), OnNext(230, 7), OnNext(240, 8), OnNext(250, 9), OnCompleted<int>(260) });
            var e5 = scheduler.CreateHotObservable(new[] { OnNext(150, 1), OnNext(210, 5), OnNext(220, 6), OnNext(230, 7), OnNext(240, 8), OnNext(250, 9), OnNext(260, 10), OnCompleted<int>(270) });

            var res = scheduler.Start(() =>
                Observable.Zip(e0, e1, e2, e3, e4, e5, (_0, _1, _2, _3, _4, _5) => _0 + _1 + _2 + _3 + _4 + _5)
            );
            
            res.Messages.AssertEqual(
                OnNext<int>(210, 30),
                OnCompleted<int>(260)
            );

            var es = new[] { e0, e1, e2, e3, e4, e5 };

            var i = 0;
            foreach (var e in es.Take(es.Length - 1))
                e.Subscriptions.AssertEqual(Subscribe(200, 220 + (i++ * 10)));

            es.Last().Subscriptions.AssertEqual(
                Subscribe(200, 220 + (i - 1) * 10)
            );
        }

        [TestMethod]
        public void Zip_AllCompleted7()
        {
            var scheduler = new TestScheduler();
            
            var e0 = scheduler.CreateHotObservable(new[] { OnNext(150, 1), OnNext(210, 5), OnCompleted<int>(220) });
            var e1 = scheduler.CreateHotObservable(new[] { OnNext(150, 1), OnNext(210, 5), OnNext(220, 6), OnCompleted<int>(230) });
            var e2 = scheduler.CreateHotObservable(new[] { OnNext(150, 1), OnNext(210, 5), OnNext(220, 6), OnNext(230, 7), OnCompleted<int>(240) });
            var e3 = scheduler.CreateHotObservable(new[] { OnNext(150, 1), OnNext(210, 5), OnNext(220, 6), OnNext(230, 7), OnNext(240, 8), OnCompleted<int>(250) });
            var e4 = scheduler.CreateHotObservable(new[] { OnNext(150, 1), OnNext(210, 5), OnNext(220, 6), OnNext(230, 7), OnNext(240, 8), OnNext(250, 9), OnCompleted<int>(260) });
            var e5 = scheduler.CreateHotObservable(new[] { OnNext(150, 1), OnNext(210, 5), OnNext(220, 6), OnNext(230, 7), OnNext(240, 8), OnNext(250, 9), OnNext(260, 10), OnCompleted<int>(270) });
            var e6 = scheduler.CreateHotObservable(new[] { OnNext(150, 1), OnNext(210, 5), OnNext(220, 6), OnNext(230, 7), OnNext(240, 8), OnNext(250, 9), OnNext(260, 10), OnNext(270, 11), OnCompleted<int>(280) });

            var res = scheduler.Start(() =>
                Observable.Zip(e0, e1, e2, e3, e4, e5, e6, (_0, _1, _2, _3, _4, _5, _6) => _0 + _1 + _2 + _3 + _4 + _5 + _6)
            );
            
            res.Messages.AssertEqual(
                OnNext<int>(210, 35),
                OnCompleted<int>(270)
            );

            var es = new[] { e0, e1, e2, e3, e4, e5, e6 };

            var i = 0;
            foreach (var e in es.Take(es.Length - 1))
                e.Subscriptions.AssertEqual(Subscribe(200, 220 + (i++ * 10)));

            es.Last().Subscriptions.AssertEqual(
                Subscribe(200, 220 + (i - 1) * 10)
            );
        }

        [TestMethod]
        public void Zip_AllCompleted8()
        {
            var scheduler = new TestScheduler();
            
            var e0 = scheduler.CreateHotObservable(new[] { OnNext(150, 1), OnNext(210, 5), OnCompleted<int>(220) });
            var e1 = scheduler.CreateHotObservable(new[] { OnNext(150, 1), OnNext(210, 5), OnNext(220, 6), OnCompleted<int>(230) });
            var e2 = scheduler.CreateHotObservable(new[] { OnNext(150, 1), OnNext(210, 5), OnNext(220, 6), OnNext(230, 7), OnCompleted<int>(240) });
            var e3 = scheduler.CreateHotObservable(new[] { OnNext(150, 1), OnNext(210, 5), OnNext(220, 6), OnNext(230, 7), OnNext(240, 8), OnCompleted<int>(250) });
            var e4 = scheduler.CreateHotObservable(new[] { OnNext(150, 1), OnNext(210, 5), OnNext(220, 6), OnNext(230, 7), OnNext(240, 8), OnNext(250, 9), OnCompleted<int>(260) });
            var e5 = scheduler.CreateHotObservable(new[] { OnNext(150, 1), OnNext(210, 5), OnNext(220, 6), OnNext(230, 7), OnNext(240, 8), OnNext(250, 9), OnNext(260, 10), OnCompleted<int>(270) });
            var e6 = scheduler.CreateHotObservable(new[] { OnNext(150, 1), OnNext(210, 5), OnNext(220, 6), OnNext(230, 7), OnNext(240, 8), OnNext(250, 9), OnNext(260, 10), OnNext(270, 11), OnCompleted<int>(280) });
            var e7 = scheduler.CreateHotObservable(new[] { OnNext(150, 1), OnNext(210, 5), OnNext(220, 6), OnNext(230, 7), OnNext(240, 8), OnNext(250, 9), OnNext(260, 10), OnNext(270, 11), OnNext(280, 12), OnCompleted<int>(290) });

            var res = scheduler.Start(() =>
                Observable.Zip(e0, e1, e2, e3, e4, e5, e6, e7, (_0, _1, _2, _3, _4, _5, _6, _7) => _0 + _1 + _2 + _3 + _4 + _5 + _6 + _7)
            );
            
            res.Messages.AssertEqual(
                OnNext<int>(210, 40),
                OnCompleted<int>(280)
            );

            var es = new[] { e0, e1, e2, e3, e4, e5, e6, e7 };

            var i = 0;
            foreach (var e in es.Take(es.Length - 1))
                e.Subscriptions.AssertEqual(Subscribe(200, 220 + (i++ * 10)));

            es.Last().Subscriptions.AssertEqual(
                Subscribe(200, 220 + (i - 1) * 10)
            );
        }

        [TestMethod]
        public void Zip_AllCompleted9()
        {
            var scheduler = new TestScheduler();
            
            var e0 = scheduler.CreateHotObservable(new[] { OnNext(150, 1), OnNext(210, 5), OnCompleted<int>(220) });
            var e1 = scheduler.CreateHotObservable(new[] { OnNext(150, 1), OnNext(210, 5), OnNext(220, 6), OnCompleted<int>(230) });
            var e2 = scheduler.CreateHotObservable(new[] { OnNext(150, 1), OnNext(210, 5), OnNext(220, 6), OnNext(230, 7), OnCompleted<int>(240) });
            var e3 = scheduler.CreateHotObservable(new[] { OnNext(150, 1), OnNext(210, 5), OnNext(220, 6), OnNext(230, 7), OnNext(240, 8), OnCompleted<int>(250) });
            var e4 = scheduler.CreateHotObservable(new[] { OnNext(150, 1), OnNext(210, 5), OnNext(220, 6), OnNext(230, 7), OnNext(240, 8), OnNext(250, 9), OnCompleted<int>(260) });
            var e5 = scheduler.CreateHotObservable(new[] { OnNext(150, 1), OnNext(210, 5), OnNext(220, 6), OnNext(230, 7), OnNext(240, 8), OnNext(250, 9), OnNext(260, 10), OnCompleted<int>(270) });
            var e6 = scheduler.CreateHotObservable(new[] { OnNext(150, 1), OnNext(210, 5), OnNext(220, 6), OnNext(230, 7), OnNext(240, 8), OnNext(250, 9), OnNext(260, 10), OnNext(270, 11), OnCompleted<int>(280) });
            var e7 = scheduler.CreateHotObservable(new[] { OnNext(150, 1), OnNext(210, 5), OnNext(220, 6), OnNext(230, 7), OnNext(240, 8), OnNext(250, 9), OnNext(260, 10), OnNext(270, 11), OnNext(280, 12), OnCompleted<int>(290) });
            var e8 = scheduler.CreateHotObservable(new[] { OnNext(150, 1), OnNext(210, 5), OnNext(220, 6), OnNext(230, 7), OnNext(240, 8), OnNext(250, 9), OnNext(260, 10), OnNext(270, 11), OnNext(280, 12), OnNext(290, 13), OnCompleted<int>(300) });

            var res = scheduler.Start(() =>
                Observable.Zip(e0, e1, e2, e3, e4, e5, e6, e7, e8, (_0, _1, _2, _3, _4, _5, _6, _7, _8) => _0 + _1 + _2 + _3 + _4 + _5 + _6 + _7 + _8)
            );
            
            res.Messages.AssertEqual(
                OnNext<int>(210, 45),
                OnCompleted<int>(290)
            );

            var es = new[] { e0, e1, e2, e3, e4, e5, e6, e7, e8 };

            var i = 0;
            foreach (var e in es.Take(es.Length - 1))
                e.Subscriptions.AssertEqual(Subscribe(200, 220 + (i++ * 10)));

            es.Last().Subscriptions.AssertEqual(
                Subscribe(200, 220 + (i - 1) * 10)
            );
        }

        [TestMethod]
        public void Zip_AllCompleted10()
        {
            var scheduler = new TestScheduler();
            
            var e0 = scheduler.CreateHotObservable(new[] { OnNext(150, 1), OnNext(210, 5), OnCompleted<int>(220) });
            var e1 = scheduler.CreateHotObservable(new[] { OnNext(150, 1), OnNext(210, 5), OnNext(220, 6), OnCompleted<int>(230) });
            var e2 = scheduler.CreateHotObservable(new[] { OnNext(150, 1), OnNext(210, 5), OnNext(220, 6), OnNext(230, 7), OnCompleted<int>(240) });
            var e3 = scheduler.CreateHotObservable(new[] { OnNext(150, 1), OnNext(210, 5), OnNext(220, 6), OnNext(230, 7), OnNext(240, 8), OnCompleted<int>(250) });
            var e4 = scheduler.CreateHotObservable(new[] { OnNext(150, 1), OnNext(210, 5), OnNext(220, 6), OnNext(230, 7), OnNext(240, 8), OnNext(250, 9), OnCompleted<int>(260) });
            var e5 = scheduler.CreateHotObservable(new[] { OnNext(150, 1), OnNext(210, 5), OnNext(220, 6), OnNext(230, 7), OnNext(240, 8), OnNext(250, 9), OnNext(260, 10), OnCompleted<int>(270) });
            var e6 = scheduler.CreateHotObservable(new[] { OnNext(150, 1), OnNext(210, 5), OnNext(220, 6), OnNext(230, 7), OnNext(240, 8), OnNext(250, 9), OnNext(260, 10), OnNext(270, 11), OnCompleted<int>(280) });
            var e7 = scheduler.CreateHotObservable(new[] { OnNext(150, 1), OnNext(210, 5), OnNext(220, 6), OnNext(230, 7), OnNext(240, 8), OnNext(250, 9), OnNext(260, 10), OnNext(270, 11), OnNext(280, 12), OnCompleted<int>(290) });
            var e8 = scheduler.CreateHotObservable(new[] { OnNext(150, 1), OnNext(210, 5), OnNext(220, 6), OnNext(230, 7), OnNext(240, 8), OnNext(250, 9), OnNext(260, 10), OnNext(270, 11), OnNext(280, 12), OnNext(290, 13), OnCompleted<int>(300) });
            var e9 = scheduler.CreateHotObservable(new[] { OnNext(150, 1), OnNext(210, 5), OnNext(220, 6), OnNext(230, 7), OnNext(240, 8), OnNext(250, 9), OnNext(260, 10), OnNext(270, 11), OnNext(280, 12), OnNext(290, 13), OnNext(300, 14), OnCompleted<int>(310) });

            var res = scheduler.Start(() =>
                Observable.Zip(e0, e1, e2, e3, e4, e5, e6, e7, e8, e9, (_0, _1, _2, _3, _4, _5, _6, _7, _8, _9) => _0 + _1 + _2 + _3 + _4 + _5 + _6 + _7 + _8 + _9)
            );
            
            res.Messages.AssertEqual(
                OnNext<int>(210, 50),
                OnCompleted<int>(300)
            );

            var es = new[] { e0, e1, e2, e3, e4, e5, e6, e7, e8, e9 };

            var i = 0;
            foreach (var e in es.Take(es.Length - 1))
                e.Subscriptions.AssertEqual(Subscribe(200, 220 + (i++ * 10)));

            es.Last().Subscriptions.AssertEqual(
                Subscribe(200, 220 + (i - 1) * 10)
            );
        }

        [TestMethod]
        public void Zip_AllCompleted11()
        {
            var scheduler = new TestScheduler();
            
            var e0 = scheduler.CreateHotObservable(new[] { OnNext(150, 1), OnNext(210, 5), OnCompleted<int>(220) });
            var e1 = scheduler.CreateHotObservable(new[] { OnNext(150, 1), OnNext(210, 5), OnNext(220, 6), OnCompleted<int>(230) });
            var e2 = scheduler.CreateHotObservable(new[] { OnNext(150, 1), OnNext(210, 5), OnNext(220, 6), OnNext(230, 7), OnCompleted<int>(240) });
            var e3 = scheduler.CreateHotObservable(new[] { OnNext(150, 1), OnNext(210, 5), OnNext(220, 6), OnNext(230, 7), OnNext(240, 8), OnCompleted<int>(250) });
            var e4 = scheduler.CreateHotObservable(new[] { OnNext(150, 1), OnNext(210, 5), OnNext(220, 6), OnNext(230, 7), OnNext(240, 8), OnNext(250, 9), OnCompleted<int>(260) });
            var e5 = scheduler.CreateHotObservable(new[] { OnNext(150, 1), OnNext(210, 5), OnNext(220, 6), OnNext(230, 7), OnNext(240, 8), OnNext(250, 9), OnNext(260, 10), OnCompleted<int>(270) });
            var e6 = scheduler.CreateHotObservable(new[] { OnNext(150, 1), OnNext(210, 5), OnNext(220, 6), OnNext(230, 7), OnNext(240, 8), OnNext(250, 9), OnNext(260, 10), OnNext(270, 11), OnCompleted<int>(280) });
            var e7 = scheduler.CreateHotObservable(new[] { OnNext(150, 1), OnNext(210, 5), OnNext(220, 6), OnNext(230, 7), OnNext(240, 8), OnNext(250, 9), OnNext(260, 10), OnNext(270, 11), OnNext(280, 12), OnCompleted<int>(290) });
            var e8 = scheduler.CreateHotObservable(new[] { OnNext(150, 1), OnNext(210, 5), OnNext(220, 6), OnNext(230, 7), OnNext(240, 8), OnNext(250, 9), OnNext(260, 10), OnNext(270, 11), OnNext(280, 12), OnNext(290, 13), OnCompleted<int>(300) });
            var e9 = scheduler.CreateHotObservable(new[] { OnNext(150, 1), OnNext(210, 5), OnNext(220, 6), OnNext(230, 7), OnNext(240, 8), OnNext(250, 9), OnNext(260, 10), OnNext(270, 11), OnNext(280, 12), OnNext(290, 13), OnNext(300, 14), OnCompleted<int>(310) });
            var e10 = scheduler.CreateHotObservable(new[] { OnNext(150, 1), OnNext(210, 5), OnNext(220, 6), OnNext(230, 7), OnNext(240, 8), OnNext(250, 9), OnNext(260, 10), OnNext(270, 11), OnNext(280, 12), OnNext(290, 13), OnNext(300, 14), OnNext(310, 15), OnCompleted<int>(320) });

            var res = scheduler.Start(() =>
                Observable.Zip(e0, e1, e2, e3, e4, e5, e6, e7, e8, e9, e10, (_0, _1, _2, _3, _4, _5, _6, _7, _8, _9, _10) => _0 + _1 + _2 + _3 + _4 + _5 + _6 + _7 + _8 + _9 + _10)
            );
            
            res.Messages.AssertEqual(
                OnNext<int>(210, 55),
                OnCompleted<int>(310)
            );

            var es = new[] { e0, e1, e2, e3, e4, e5, e6, e7, e8, e9, e10 };

            var i = 0;
            foreach (var e in es.Take(es.Length - 1))
                e.Subscriptions.AssertEqual(Subscribe(200, 220 + (i++ * 10)));

            es.Last().Subscriptions.AssertEqual(
                Subscribe(200, 220 + (i - 1) * 10)
            );
        }

        [TestMethod]
        public void Zip_AllCompleted12()
        {
            var scheduler = new TestScheduler();
            
            var e0 = scheduler.CreateHotObservable(new[] { OnNext(150, 1), OnNext(210, 5), OnCompleted<int>(220) });
            var e1 = scheduler.CreateHotObservable(new[] { OnNext(150, 1), OnNext(210, 5), OnNext(220, 6), OnCompleted<int>(230) });
            var e2 = scheduler.CreateHotObservable(new[] { OnNext(150, 1), OnNext(210, 5), OnNext(220, 6), OnNext(230, 7), OnCompleted<int>(240) });
            var e3 = scheduler.CreateHotObservable(new[] { OnNext(150, 1), OnNext(210, 5), OnNext(220, 6), OnNext(230, 7), OnNext(240, 8), OnCompleted<int>(250) });
            var e4 = scheduler.CreateHotObservable(new[] { OnNext(150, 1), OnNext(210, 5), OnNext(220, 6), OnNext(230, 7), OnNext(240, 8), OnNext(250, 9), OnCompleted<int>(260) });
            var e5 = scheduler.CreateHotObservable(new[] { OnNext(150, 1), OnNext(210, 5), OnNext(220, 6), OnNext(230, 7), OnNext(240, 8), OnNext(250, 9), OnNext(260, 10), OnCompleted<int>(270) });
            var e6 = scheduler.CreateHotObservable(new[] { OnNext(150, 1), OnNext(210, 5), OnNext(220, 6), OnNext(230, 7), OnNext(240, 8), OnNext(250, 9), OnNext(260, 10), OnNext(270, 11), OnCompleted<int>(280) });
            var e7 = scheduler.CreateHotObservable(new[] { OnNext(150, 1), OnNext(210, 5), OnNext(220, 6), OnNext(230, 7), OnNext(240, 8), OnNext(250, 9), OnNext(260, 10), OnNext(270, 11), OnNext(280, 12), OnCompleted<int>(290) });
            var e8 = scheduler.CreateHotObservable(new[] { OnNext(150, 1), OnNext(210, 5), OnNext(220, 6), OnNext(230, 7), OnNext(240, 8), OnNext(250, 9), OnNext(260, 10), OnNext(270, 11), OnNext(280, 12), OnNext(290, 13), OnCompleted<int>(300) });
            var e9 = scheduler.CreateHotObservable(new[] { OnNext(150, 1), OnNext(210, 5), OnNext(220, 6), OnNext(230, 7), OnNext(240, 8), OnNext(250, 9), OnNext(260, 10), OnNext(270, 11), OnNext(280, 12), OnNext(290, 13), OnNext(300, 14), OnCompleted<int>(310) });
            var e10 = scheduler.CreateHotObservable(new[] { OnNext(150, 1), OnNext(210, 5), OnNext(220, 6), OnNext(230, 7), OnNext(240, 8), OnNext(250, 9), OnNext(260, 10), OnNext(270, 11), OnNext(280, 12), OnNext(290, 13), OnNext(300, 14), OnNext(310, 15), OnCompleted<int>(320) });
            var e11 = scheduler.CreateHotObservable(new[] { OnNext(150, 1), OnNext(210, 5), OnNext(220, 6), OnNext(230, 7), OnNext(240, 8), OnNext(250, 9), OnNext(260, 10), OnNext(270, 11), OnNext(280, 12), OnNext(290, 13), OnNext(300, 14), OnNext(310, 15), OnNext(320, 16), OnCompleted<int>(330) });

            var res = scheduler.Start(() =>
                Observable.Zip(e0, e1, e2, e3, e4, e5, e6, e7, e8, e9, e10, e11, (_0, _1, _2, _3, _4, _5, _6, _7, _8, _9, _10, _11) => _0 + _1 + _2 + _3 + _4 + _5 + _6 + _7 + _8 + _9 + _10 + _11)
            );
            
            res.Messages.AssertEqual(
                OnNext<int>(210, 60),
                OnCompleted<int>(320)
            );

            var es = new[] { e0, e1, e2, e3, e4, e5, e6, e7, e8, e9, e10, e11 };

            var i = 0;
            foreach (var e in es.Take(es.Length - 1))
                e.Subscriptions.AssertEqual(Subscribe(200, 220 + (i++ * 10)));

            es.Last().Subscriptions.AssertEqual(
                Subscribe(200, 220 + (i - 1) * 10)
            );
        }

        [TestMethod]
        public void Zip_AllCompleted13()
        {
            var scheduler = new TestScheduler();
            
            var e0 = scheduler.CreateHotObservable(new[] { OnNext(150, 1), OnNext(210, 5), OnCompleted<int>(220) });
            var e1 = scheduler.CreateHotObservable(new[] { OnNext(150, 1), OnNext(210, 5), OnNext(220, 6), OnCompleted<int>(230) });
            var e2 = scheduler.CreateHotObservable(new[] { OnNext(150, 1), OnNext(210, 5), OnNext(220, 6), OnNext(230, 7), OnCompleted<int>(240) });
            var e3 = scheduler.CreateHotObservable(new[] { OnNext(150, 1), OnNext(210, 5), OnNext(220, 6), OnNext(230, 7), OnNext(240, 8), OnCompleted<int>(250) });
            var e4 = scheduler.CreateHotObservable(new[] { OnNext(150, 1), OnNext(210, 5), OnNext(220, 6), OnNext(230, 7), OnNext(240, 8), OnNext(250, 9), OnCompleted<int>(260) });
            var e5 = scheduler.CreateHotObservable(new[] { OnNext(150, 1), OnNext(210, 5), OnNext(220, 6), OnNext(230, 7), OnNext(240, 8), OnNext(250, 9), OnNext(260, 10), OnCompleted<int>(270) });
            var e6 = scheduler.CreateHotObservable(new[] { OnNext(150, 1), OnNext(210, 5), OnNext(220, 6), OnNext(230, 7), OnNext(240, 8), OnNext(250, 9), OnNext(260, 10), OnNext(270, 11), OnCompleted<int>(280) });
            var e7 = scheduler.CreateHotObservable(new[] { OnNext(150, 1), OnNext(210, 5), OnNext(220, 6), OnNext(230, 7), OnNext(240, 8), OnNext(250, 9), OnNext(260, 10), OnNext(270, 11), OnNext(280, 12), OnCompleted<int>(290) });
            var e8 = scheduler.CreateHotObservable(new[] { OnNext(150, 1), OnNext(210, 5), OnNext(220, 6), OnNext(230, 7), OnNext(240, 8), OnNext(250, 9), OnNext(260, 10), OnNext(270, 11), OnNext(280, 12), OnNext(290, 13), OnCompleted<int>(300) });
            var e9 = scheduler.CreateHotObservable(new[] { OnNext(150, 1), OnNext(210, 5), OnNext(220, 6), OnNext(230, 7), OnNext(240, 8), OnNext(250, 9), OnNext(260, 10), OnNext(270, 11), OnNext(280, 12), OnNext(290, 13), OnNext(300, 14), OnCompleted<int>(310) });
            var e10 = scheduler.CreateHotObservable(new[] { OnNext(150, 1), OnNext(210, 5), OnNext(220, 6), OnNext(230, 7), OnNext(240, 8), OnNext(250, 9), OnNext(260, 10), OnNext(270, 11), OnNext(280, 12), OnNext(290, 13), OnNext(300, 14), OnNext(310, 15), OnCompleted<int>(320) });
            var e11 = scheduler.CreateHotObservable(new[] { OnNext(150, 1), OnNext(210, 5), OnNext(220, 6), OnNext(230, 7), OnNext(240, 8), OnNext(250, 9), OnNext(260, 10), OnNext(270, 11), OnNext(280, 12), OnNext(290, 13), OnNext(300, 14), OnNext(310, 15), OnNext(320, 16), OnCompleted<int>(330) });
            var e12 = scheduler.CreateHotObservable(new[] { OnNext(150, 1), OnNext(210, 5), OnNext(220, 6), OnNext(230, 7), OnNext(240, 8), OnNext(250, 9), OnNext(260, 10), OnNext(270, 11), OnNext(280, 12), OnNext(290, 13), OnNext(300, 14), OnNext(310, 15), OnNext(320, 16), OnNext(330, 17), OnCompleted<int>(340) });

            var res = scheduler.Start(() =>
                Observable.Zip(e0, e1, e2, e3, e4, e5, e6, e7, e8, e9, e10, e11, e12, (_0, _1, _2, _3, _4, _5, _6, _7, _8, _9, _10, _11, _12) => _0 + _1 + _2 + _3 + _4 + _5 + _6 + _7 + _8 + _9 + _10 + _11 + _12)
            );
            
            res.Messages.AssertEqual(
                OnNext<int>(210, 65),
                OnCompleted<int>(330)
            );

            var es = new[] { e0, e1, e2, e3, e4, e5, e6, e7, e8, e9, e10, e11, e12 };

            var i = 0;
            foreach (var e in es.Take(es.Length - 1))
                e.Subscriptions.AssertEqual(Subscribe(200, 220 + (i++ * 10)));

            es.Last().Subscriptions.AssertEqual(
                Subscribe(200, 220 + (i - 1) * 10)
            );
        }

        [TestMethod]
        public void Zip_AllCompleted14()
        {
            var scheduler = new TestScheduler();
            
            var e0 = scheduler.CreateHotObservable(new[] { OnNext(150, 1), OnNext(210, 5), OnCompleted<int>(220) });
            var e1 = scheduler.CreateHotObservable(new[] { OnNext(150, 1), OnNext(210, 5), OnNext(220, 6), OnCompleted<int>(230) });
            var e2 = scheduler.CreateHotObservable(new[] { OnNext(150, 1), OnNext(210, 5), OnNext(220, 6), OnNext(230, 7), OnCompleted<int>(240) });
            var e3 = scheduler.CreateHotObservable(new[] { OnNext(150, 1), OnNext(210, 5), OnNext(220, 6), OnNext(230, 7), OnNext(240, 8), OnCompleted<int>(250) });
            var e4 = scheduler.CreateHotObservable(new[] { OnNext(150, 1), OnNext(210, 5), OnNext(220, 6), OnNext(230, 7), OnNext(240, 8), OnNext(250, 9), OnCompleted<int>(260) });
            var e5 = scheduler.CreateHotObservable(new[] { OnNext(150, 1), OnNext(210, 5), OnNext(220, 6), OnNext(230, 7), OnNext(240, 8), OnNext(250, 9), OnNext(260, 10), OnCompleted<int>(270) });
            var e6 = scheduler.CreateHotObservable(new[] { OnNext(150, 1), OnNext(210, 5), OnNext(220, 6), OnNext(230, 7), OnNext(240, 8), OnNext(250, 9), OnNext(260, 10), OnNext(270, 11), OnCompleted<int>(280) });
            var e7 = scheduler.CreateHotObservable(new[] { OnNext(150, 1), OnNext(210, 5), OnNext(220, 6), OnNext(230, 7), OnNext(240, 8), OnNext(250, 9), OnNext(260, 10), OnNext(270, 11), OnNext(280, 12), OnCompleted<int>(290) });
            var e8 = scheduler.CreateHotObservable(new[] { OnNext(150, 1), OnNext(210, 5), OnNext(220, 6), OnNext(230, 7), OnNext(240, 8), OnNext(250, 9), OnNext(260, 10), OnNext(270, 11), OnNext(280, 12), OnNext(290, 13), OnCompleted<int>(300) });
            var e9 = scheduler.CreateHotObservable(new[] { OnNext(150, 1), OnNext(210, 5), OnNext(220, 6), OnNext(230, 7), OnNext(240, 8), OnNext(250, 9), OnNext(260, 10), OnNext(270, 11), OnNext(280, 12), OnNext(290, 13), OnNext(300, 14), OnCompleted<int>(310) });
            var e10 = scheduler.CreateHotObservable(new[] { OnNext(150, 1), OnNext(210, 5), OnNext(220, 6), OnNext(230, 7), OnNext(240, 8), OnNext(250, 9), OnNext(260, 10), OnNext(270, 11), OnNext(280, 12), OnNext(290, 13), OnNext(300, 14), OnNext(310, 15), OnCompleted<int>(320) });
            var e11 = scheduler.CreateHotObservable(new[] { OnNext(150, 1), OnNext(210, 5), OnNext(220, 6), OnNext(230, 7), OnNext(240, 8), OnNext(250, 9), OnNext(260, 10), OnNext(270, 11), OnNext(280, 12), OnNext(290, 13), OnNext(300, 14), OnNext(310, 15), OnNext(320, 16), OnCompleted<int>(330) });
            var e12 = scheduler.CreateHotObservable(new[] { OnNext(150, 1), OnNext(210, 5), OnNext(220, 6), OnNext(230, 7), OnNext(240, 8), OnNext(250, 9), OnNext(260, 10), OnNext(270, 11), OnNext(280, 12), OnNext(290, 13), OnNext(300, 14), OnNext(310, 15), OnNext(320, 16), OnNext(330, 17), OnCompleted<int>(340) });
            var e13 = scheduler.CreateHotObservable(new[] { OnNext(150, 1), OnNext(210, 5), OnNext(220, 6), OnNext(230, 7), OnNext(240, 8), OnNext(250, 9), OnNext(260, 10), OnNext(270, 11), OnNext(280, 12), OnNext(290, 13), OnNext(300, 14), OnNext(310, 15), OnNext(320, 16), OnNext(330, 17), OnNext(340, 18), OnCompleted<int>(350) });

            var res = scheduler.Start(() =>
                Observable.Zip(e0, e1, e2, e3, e4, e5, e6, e7, e8, e9, e10, e11, e12, e13, (_0, _1, _2, _3, _4, _5, _6, _7, _8, _9, _10, _11, _12, _13) => _0 + _1 + _2 + _3 + _4 + _5 + _6 + _7 + _8 + _9 + _10 + _11 + _12 + _13)
            );
            
            res.Messages.AssertEqual(
                OnNext<int>(210, 70),
                OnCompleted<int>(340)
            );

            var es = new[] { e0, e1, e2, e3, e4, e5, e6, e7, e8, e9, e10, e11, e12, e13 };

            var i = 0;
            foreach (var e in es.Take(es.Length - 1))
                e.Subscriptions.AssertEqual(Subscribe(200, 220 + (i++ * 10)));

            es.Last().Subscriptions.AssertEqual(
                Subscribe(200, 220 + (i - 1) * 10)
            );
        }

        [TestMethod]
        public void Zip_AllCompleted15()
        {
            var scheduler = new TestScheduler();

            var e0 = scheduler.CreateHotObservable(new[] { OnNext(150, 1), OnNext(210, 5), OnCompleted<int>(220) });
            var e1 = scheduler.CreateHotObservable(new[] { OnNext(150, 1), OnNext(210, 5), OnNext(220, 6), OnCompleted<int>(230) });
            var e2 = scheduler.CreateHotObservable(new[] { OnNext(150, 1), OnNext(210, 5), OnNext(220, 6), OnNext(230, 7), OnCompleted<int>(240) });
            var e3 = scheduler.CreateHotObservable(new[] { OnNext(150, 1), OnNext(210, 5), OnNext(220, 6), OnNext(230, 7), OnNext(240, 8), OnCompleted<int>(250) });
            var e4 = scheduler.CreateHotObservable(new[] { OnNext(150, 1), OnNext(210, 5), OnNext(220, 6), OnNext(230, 7), OnNext(240, 8), OnNext(250, 9), OnCompleted<int>(260) });
            var e5 = scheduler.CreateHotObservable(new[] { OnNext(150, 1), OnNext(210, 5), OnNext(220, 6), OnNext(230, 7), OnNext(240, 8), OnNext(250, 9), OnNext(260, 10), OnCompleted<int>(270) });
            var e6 = scheduler.CreateHotObservable(new[] { OnNext(150, 1), OnNext(210, 5), OnNext(220, 6), OnNext(230, 7), OnNext(240, 8), OnNext(250, 9), OnNext(260, 10), OnNext(270, 11), OnCompleted<int>(280) });
            var e7 = scheduler.CreateHotObservable(new[] { OnNext(150, 1), OnNext(210, 5), OnNext(220, 6), OnNext(230, 7), OnNext(240, 8), OnNext(250, 9), OnNext(260, 10), OnNext(270, 11), OnNext(280, 12), OnCompleted<int>(290) });
            var e8 = scheduler.CreateHotObservable(new[] { OnNext(150, 1), OnNext(210, 5), OnNext(220, 6), OnNext(230, 7), OnNext(240, 8), OnNext(250, 9), OnNext(260, 10), OnNext(270, 11), OnNext(280, 12), OnNext(290, 13), OnCompleted<int>(300) });
            var e9 = scheduler.CreateHotObservable(new[] { OnNext(150, 1), OnNext(210, 5), OnNext(220, 6), OnNext(230, 7), OnNext(240, 8), OnNext(250, 9), OnNext(260, 10), OnNext(270, 11), OnNext(280, 12), OnNext(290, 13), OnNext(300, 14), OnCompleted<int>(310) });
            var e10 = scheduler.CreateHotObservable(new[] { OnNext(150, 1), OnNext(210, 5), OnNext(220, 6), OnNext(230, 7), OnNext(240, 8), OnNext(250, 9), OnNext(260, 10), OnNext(270, 11), OnNext(280, 12), OnNext(290, 13), OnNext(300, 14), OnNext(310, 15), OnCompleted<int>(320) });
            var e11 = scheduler.CreateHotObservable(new[] { OnNext(150, 1), OnNext(210, 5), OnNext(220, 6), OnNext(230, 7), OnNext(240, 8), OnNext(250, 9), OnNext(260, 10), OnNext(270, 11), OnNext(280, 12), OnNext(290, 13), OnNext(300, 14), OnNext(310, 15), OnNext(320, 16), OnCompleted<int>(330) });
            var e12 = scheduler.CreateHotObservable(new[] { OnNext(150, 1), OnNext(210, 5), OnNext(220, 6), OnNext(230, 7), OnNext(240, 8), OnNext(250, 9), OnNext(260, 10), OnNext(270, 11), OnNext(280, 12), OnNext(290, 13), OnNext(300, 14), OnNext(310, 15), OnNext(320, 16), OnNext(330, 17), OnCompleted<int>(340) });
            var e13 = scheduler.CreateHotObservable(new[] { OnNext(150, 1), OnNext(210, 5), OnNext(220, 6), OnNext(230, 7), OnNext(240, 8), OnNext(250, 9), OnNext(260, 10), OnNext(270, 11), OnNext(280, 12), OnNext(290, 13), OnNext(300, 14), OnNext(310, 15), OnNext(320, 16), OnNext(330, 17), OnNext(340, 18), OnCompleted<int>(350) });
            var e14 = scheduler.CreateHotObservable(new[] { OnNext(150, 1), OnNext(210, 5), OnNext(220, 6), OnNext(230, 7), OnNext(240, 8), OnNext(250, 9), OnNext(260, 10), OnNext(270, 11), OnNext(280, 12), OnNext(290, 13), OnNext(300, 14), OnNext(310, 15), OnNext(320, 16), OnNext(330, 17), OnNext(340, 18), OnNext(350, 19), OnCompleted<int>(360) });

            var res = scheduler.Start(() =>
                Observable.Zip(e0, e1, e2, e3, e4, e5, e6, e7, e8, e9, e10, e11, e12, e13, e14, (_0, _1, _2, _3, _4, _5, _6, _7, _8, _9, _10, _11, _12, _13, _14) => _0 + _1 + _2 + _3 + _4 + _5 + _6 + _7 + _8 + _9 + _10 + _11 + _12 + _13 + _14)
            );
            
            res.Messages.AssertEqual(
                OnNext<int>(210, 75),
                OnCompleted<int>(350)
            );

            var es = new[] { e0, e1, e2, e3, e4, e5, e6, e7, e8, e9, e10, e11, e12, e13, e14 };

            var i = 0;
            foreach (var e in es.Take(es.Length - 1))
                e.Subscriptions.AssertEqual(Subscribe(200, 220 + (i++ * 10)));

            es.Last().Subscriptions.AssertEqual(
                Subscribe(200, 220 + (i - 1) * 10)
            );
        }

        [TestMethod]
        public void Zip_AllCompleted16()
        {
            var scheduler = new TestScheduler();

            var e0 = scheduler.CreateHotObservable(new[] { OnNext(150, 1), OnNext(210, 5), OnCompleted<int>(220) });
            var e1 = scheduler.CreateHotObservable(new[] { OnNext(150, 1), OnNext(210, 5), OnNext(220, 6), OnCompleted<int>(230) });
            var e2 = scheduler.CreateHotObservable(new[] { OnNext(150, 1), OnNext(210, 5), OnNext(220, 6), OnNext(230, 7), OnCompleted<int>(240) });
            var e3 = scheduler.CreateHotObservable(new[] { OnNext(150, 1), OnNext(210, 5), OnNext(220, 6), OnNext(230, 7), OnNext(240, 8), OnCompleted<int>(250) });
            var e4 = scheduler.CreateHotObservable(new[] { OnNext(150, 1), OnNext(210, 5), OnNext(220, 6), OnNext(230, 7), OnNext(240, 8), OnNext(250, 9), OnCompleted<int>(260) });
            var e5 = scheduler.CreateHotObservable(new[] { OnNext(150, 1), OnNext(210, 5), OnNext(220, 6), OnNext(230, 7), OnNext(240, 8), OnNext(250, 9), OnNext(260, 10), OnCompleted<int>(270) });
            var e6 = scheduler.CreateHotObservable(new[] { OnNext(150, 1), OnNext(210, 5), OnNext(220, 6), OnNext(230, 7), OnNext(240, 8), OnNext(250, 9), OnNext(260, 10), OnNext(270, 11), OnCompleted<int>(280) });
            var e7 = scheduler.CreateHotObservable(new[] { OnNext(150, 1), OnNext(210, 5), OnNext(220, 6), OnNext(230, 7), OnNext(240, 8), OnNext(250, 9), OnNext(260, 10), OnNext(270, 11), OnNext(280, 12), OnCompleted<int>(290) });
            var e8 = scheduler.CreateHotObservable(new[] { OnNext(150, 1), OnNext(210, 5), OnNext(220, 6), OnNext(230, 7), OnNext(240, 8), OnNext(250, 9), OnNext(260, 10), OnNext(270, 11), OnNext(280, 12), OnNext(290, 13), OnCompleted<int>(300) });
            var e9 = scheduler.CreateHotObservable(new[] { OnNext(150, 1), OnNext(210, 5), OnNext(220, 6), OnNext(230, 7), OnNext(240, 8), OnNext(250, 9), OnNext(260, 10), OnNext(270, 11), OnNext(280, 12), OnNext(290, 13), OnNext(300, 14), OnCompleted<int>(310) });
            var e10 = scheduler.CreateHotObservable(new[] { OnNext(150, 1), OnNext(210, 5), OnNext(220, 6), OnNext(230, 7), OnNext(240, 8), OnNext(250, 9), OnNext(260, 10), OnNext(270, 11), OnNext(280, 12), OnNext(290, 13), OnNext(300, 14), OnNext(310, 15), OnCompleted<int>(320) });
            var e11 = scheduler.CreateHotObservable(new[] { OnNext(150, 1), OnNext(210, 5), OnNext(220, 6), OnNext(230, 7), OnNext(240, 8), OnNext(250, 9), OnNext(260, 10), OnNext(270, 11), OnNext(280, 12), OnNext(290, 13), OnNext(300, 14), OnNext(310, 15), OnNext(320, 16), OnCompleted<int>(330) });
            var e12 = scheduler.CreateHotObservable(new[] { OnNext(150, 1), OnNext(210, 5), OnNext(220, 6), OnNext(230, 7), OnNext(240, 8), OnNext(250, 9), OnNext(260, 10), OnNext(270, 11), OnNext(280, 12), OnNext(290, 13), OnNext(300, 14), OnNext(310, 15), OnNext(320, 16), OnNext(330, 17), OnCompleted<int>(340) });
            var e13 = scheduler.CreateHotObservable(new[] { OnNext(150, 1), OnNext(210, 5), OnNext(220, 6), OnNext(230, 7), OnNext(240, 8), OnNext(250, 9), OnNext(260, 10), OnNext(270, 11), OnNext(280, 12), OnNext(290, 13), OnNext(300, 14), OnNext(310, 15), OnNext(320, 16), OnNext(330, 17), OnNext(340, 18), OnCompleted<int>(350) });
            var e14 = scheduler.CreateHotObservable(new[] { OnNext(150, 1), OnNext(210, 5), OnNext(220, 6), OnNext(230, 7), OnNext(240, 8), OnNext(250, 9), OnNext(260, 10), OnNext(270, 11), OnNext(280, 12), OnNext(290, 13), OnNext(300, 14), OnNext(310, 15), OnNext(320, 16), OnNext(330, 17), OnNext(340, 18), OnNext(350, 19), OnCompleted<int>(360) });
            var e15 = scheduler.CreateHotObservable(new[] { OnNext(150, 1), OnNext(210, 5), OnNext(220, 6), OnNext(230, 7), OnNext(240, 8), OnNext(250, 9), OnNext(260, 10), OnNext(270, 11), OnNext(280, 12), OnNext(290, 13), OnNext(300, 14), OnNext(310, 15), OnNext(320, 16), OnNext(330, 17), OnNext(340, 18), OnNext(350, 19), OnNext(360, 20), OnCompleted<int>(370) });

            var res = scheduler.Start(() =>
                Observable.Zip(e0, e1, e2, e3, e4, e5, e6, e7, e8, e9, e10, e11, e12, e13, e14, e15, (_0, _1, _2, _3, _4, _5, _6, _7, _8, _9, _10, _11, _12, _13, _14, _15) => _0 + _1 + _2 + _3 + _4 + _5 + _6 + _7 + _8 + _9 + _10 + _11 + _12 + _13 + _14 + _15)
            );
            
            res.Messages.AssertEqual(
                OnNext<int>(210, 80),
                OnCompleted<int>(360)
            );

            var es = new[] { e0, e1, e2, e3, e4, e5, e6, e7, e8, e9, e10, e11, e12, e13, e14, e15 };

            var i = 0;
            foreach (var e in es.Take(es.Length - 1))
                e.Subscriptions.AssertEqual(Subscribe(200, 220 + (i++ * 10)));

            es.Last().Subscriptions.AssertEqual(
                Subscribe(200, 220 + (i - 1) * 10)
            );
        }
#endif

#endregion

        #region ZipWithEnumerable

        [TestMethod]
        public void ZipWithEnumerable_NeverNever()
        {
            var evt = new ManualResetEvent(false);
            var scheduler = new TestScheduler();

            var n1 = scheduler.CreateHotObservable(
                OnNext(150, 1)
            );

            var n2 = new MockEnumerable<int>(scheduler,
                EnumerableNever(evt)
            );

            var res = scheduler.Start(() =>
                n1.Zip(n2, (x, y) => x + y)
            );

            res.Messages.AssertEqual(
            );

            n1.Subscriptions.AssertEqual(
                Subscribe(200, 1000)
            );

            n2.Subscriptions.AssertEqual(
                Subscribe(200, 1000)
            );

            evt.Set();
        }

        [TestMethod]
        public void ZipWithEnumerable_NeverEmpty()
        {
            var scheduler = new TestScheduler();

            var n = scheduler.CreateHotObservable(
                OnNext(150, 1)
            );

            var e = new MockEnumerable<int>(scheduler,
                Enumerable.Empty<int>()
            );

            var res = scheduler.Start(() =>
                n.Zip(e, (x, y) => x + y)
            );

            res.Messages.AssertEqual(
            );

            n.Subscriptions.AssertEqual(
                Subscribe(200, 1000)
            );

            e.Subscriptions.AssertEqual(
                Subscribe(200, 1000)
            );
        }

        [TestMethod]
        public void ZipWithEnumerable_EmptyNever()
        {
            var evt = new ManualResetEvent(false);

            var scheduler = new TestScheduler();

            var e = scheduler.CreateHotObservable(
                OnNext(150, 1),
                OnCompleted<int>(210)
            );

            var n = new MockEnumerable<int>(scheduler,
                EnumerableNever(evt)
            );

            var res = scheduler.Start(() =>
                e.Zip(n, (x, y) => x + y)
            );

            res.Messages.AssertEqual(
                OnCompleted<int>(210)
            );

            e.Subscriptions.AssertEqual(
                Subscribe(200, 210)
            );

            n.Subscriptions.AssertEqual(
                Subscribe(200, 210)
            );

            evt.Set();
        }

        [TestMethod]
        public void ZipWithEnumerable_EmptyEmpty()
        {
            var scheduler = new TestScheduler();

            var e1 = scheduler.CreateHotObservable(
                OnNext(150, 1),
                OnCompleted<int>(210)
            );

            var e2 = new MockEnumerable<int>(scheduler,
                Enumerable.Empty<int>()
            );

            var res = scheduler.Start(() =>
                e1.Zip(e2, (x, y) => x + y)
            );

            res.Messages.AssertEqual(
                OnCompleted<int>(210)
            );

            e1.Subscriptions.AssertEqual(
                Subscribe(200, 210)
            );

            e2.Subscriptions.AssertEqual(
                Subscribe(200, 210)
            );
        }

        [TestMethod]
        public void ZipWithEnumerable_EmptyNonEmpty()
        {
            var scheduler = new TestScheduler();

            var e = scheduler.CreateHotObservable(
                OnNext(150, 1),
                OnCompleted<int>(210)
            );

            var o = new MockEnumerable<int>(scheduler,
                new[] { 2 }
            );

            var res = scheduler.Start(() =>
                e.Zip(o, (x, y) => x + y)
            );

            res.Messages.AssertEqual(
                OnCompleted<int>(210)
            );

            e.Subscriptions.AssertEqual(
                Subscribe(200, 210)
            );

            o.Subscriptions.AssertEqual(
                Subscribe(200, 210)
            );
        }

        [TestMethod]
        public void ZipWithEnumerable_NonEmptyEmpty()
        {
            var scheduler = new TestScheduler();

            var e = new MockEnumerable<int>(scheduler,
                Enumerable.Empty<int>()
            );

            var o = scheduler.CreateHotObservable(
                OnNext(150, 1),
                OnNext(215, 2),
                OnCompleted<int>(220)
            );

            var res = scheduler.Start(() =>
                o.Zip(e, (x, y) => x + y)
            );

            res.Messages.AssertEqual(
                OnCompleted<int>(215)
            );

            e.Subscriptions.AssertEqual(
                Subscribe(200, 215)
            );

            o.Subscriptions.AssertEqual(
                Subscribe(200, 215)
            );
        }

        [TestMethod]
        public void ZipWithEnumerable_NeverNonEmpty()
        {
            var scheduler = new TestScheduler();

            var o = new MockEnumerable<int>(scheduler,
                new[] { 2 }
            );

            var n = scheduler.CreateHotObservable(
                OnNext(150, 1)
            );

            var res = scheduler.Start(() =>
                n.Zip(o, (x, y) => x + y)
            );

            res.Messages.AssertEqual(
            );

            o.Subscriptions.AssertEqual(
                Subscribe(200, 1000)
            );

            n.Subscriptions.AssertEqual(
                Subscribe(200, 1000)
            );
        }

        [TestMethod]
        public void ZipWithEnumerable_NonEmptyNonEmpty()
        {
            var scheduler = new TestScheduler();

            var o1 = scheduler.CreateHotObservable(
                OnNext(150, 1),
                OnNext(215, 2),
                OnCompleted<int>(230)
            );

            var o2 = new MockEnumerable<int>(scheduler,
                new[] { 3 }
            );

            var res = scheduler.Start(() =>
                o1.Zip(o2, (x, y) => x + y)
            );

            res.Messages.AssertEqual(
                OnNext(215, 2 + 3),
                OnCompleted<int>(230)
            );

            o1.Subscriptions.AssertEqual(
                Subscribe(200, 230)
            );

            o2.Subscriptions.AssertEqual(
                Subscribe(200, 230)
            );
        }

        [TestMethod]
        public void ZipWithEnumerable_EmptyError()
        {
            var scheduler = new TestScheduler();

            var ex = new Exception();

            var e = scheduler.CreateHotObservable(
                OnNext(150, 1),
                OnCompleted<int>(230)
            );

            var f = new MockEnumerable<int>(scheduler,
                ThrowEnumerable(false, ex)
            );

            var res = scheduler.Start(() =>
                e.Zip(f, (x, y) => x + y)
            );

            res.Messages.AssertEqual(
                OnCompleted<int>(230)
            );

            e.Subscriptions.AssertEqual(
                Subscribe(200, 230)
            );

            f.Subscriptions.AssertEqual(
                Subscribe(200, 230)
            );
        }

        [TestMethod]
        public void ZipWithEnumerable_ErrorEmpty()
        {
            var scheduler = new TestScheduler();

            var ex = new Exception();

            var e = new MockEnumerable<int>(scheduler,
                Enumerable.Empty<int>()
            );

            var f = scheduler.CreateHotObservable(
                OnNext(150, 1),
                OnError<int>(220, ex)
            );

            var res = scheduler.Start(() =>
                f.Zip(e, (x, y) => x + y)
            );

            res.Messages.AssertEqual(
                OnError<int>(220, ex)
            );

            e.Subscriptions.AssertEqual(
                Subscribe(200, 220)
            );

            f.Subscriptions.AssertEqual(
                Subscribe(200, 220)
            );
        }

        [TestMethod]
        public void ZipWithEnumerable_NeverError()
        {
            var scheduler = new TestScheduler();

            var ex = new Exception();

            var n = scheduler.CreateHotObservable(
                OnNext(150, 1)
            );

            var f = new MockEnumerable<int>(scheduler,
                ThrowEnumerable(false, ex)
            );

            var res = scheduler.Start(() =>
                n.Zip(f, (x, y) => x + y)
            );

            res.Messages.AssertEqual(
            );

            n.Subscriptions.AssertEqual(
                Subscribe(200, 1000)
            );

            f.Subscriptions.AssertEqual(
                Subscribe(200, 1000)
            );
        }

        [TestMethod]
        public void ZipWithEnumerable_ErrorNever()
        {
            var evt = new ManualResetEvent(false);

            var scheduler = new TestScheduler();

            var ex = new Exception();

            var n = new MockEnumerable<int>(scheduler,
                EnumerableNever(evt)
            );

            var f = scheduler.CreateHotObservable(
                OnNext(150, 1),
                OnError<int>(220, ex)
            );

            var res = scheduler.Start(() =>
                f.Zip(n, (x, y) => x + y)
            );

            res.Messages.AssertEqual(
                OnError<int>(220, ex)
            );

            n.Subscriptions.AssertEqual(
                Subscribe(200, 220)
            );

            f.Subscriptions.AssertEqual(
                Subscribe(200, 220)
            );

            evt.Set();
        }

        [TestMethod]
        public void ZipWithEnumerable_ErrorError()
        {
            var scheduler = new TestScheduler();

            var ex1 = new Exception();
            var ex2 = new Exception();

            var f1 = scheduler.CreateHotObservable(
                OnNext(150, 1),
                OnError<int>(230, ex1)
            );

            var f2 = new MockEnumerable<int>(scheduler,
                ThrowEnumerable(false, ex2)
            );

            var res = scheduler.Start(() =>
                f1.Zip(f2, (x, y) => x + y)
            );

            res.Messages.AssertEqual(
                OnError<int>(230, ex1)
            );

            f1.Subscriptions.AssertEqual(
                Subscribe(200, 230)
            );

            f2.Subscriptions.AssertEqual(
                Subscribe(200, 230)
            );
        }

        [TestMethod]
        public void ZipWithEnumerable_SomeError()
        {
            var scheduler = new TestScheduler();

            var ex = new Exception();

            var o = scheduler.CreateHotObservable(
                OnNext(150, 1),
                OnNext(215, 2),
                OnCompleted<int>(230)
            );

            var e = new MockEnumerable<int>(scheduler,
                ThrowEnumerable(false, ex)
            );

            var res = scheduler.Start(() =>
                o.Zip(e, (x, y) => x + y)
            );

            res.Messages.AssertEqual(
                OnError<int>(215, ex)
            );

            o.Subscriptions.AssertEqual(
                Subscribe(200, 215)
            );

            e.Subscriptions.AssertEqual(
                Subscribe(200, 215)
            );
        }

        [TestMethod]
        public void ZipWithEnumerable_ErrorSome()
        {
            var scheduler = new TestScheduler();

            var ex = new Exception();

            var o = new MockEnumerable<int>(scheduler,
                new[] { 2 }
            );

            var e = scheduler.CreateHotObservable(
                OnNext(150, 1),
                OnError<int>(220, ex)
            );

            var res = scheduler.Start(() =>
                e.Zip(o, (x, y) => x + y)
            );

            res.Messages.AssertEqual(
                OnError<int>(220, ex)
            );

            o.Subscriptions.AssertEqual(
                Subscribe(200, 220)
            );

            e.Subscriptions.AssertEqual(
                Subscribe(200, 220)
            );
        }

        [TestMethod]
        public void ZipWithEnumerable_SomeDataBothSides()
        {
            var scheduler = new TestScheduler();

            var ex = new Exception();

            var o = new MockEnumerable<int>(scheduler,
                new[] { 5, 4, 3, 2 }
            );

            var e = scheduler.CreateHotObservable(
                OnNext(150, 1),
                OnNext(210, 2),
                OnNext(220, 3),
                OnNext(230, 4),
                OnNext(240, 5)
            );

            var res = scheduler.Start(() =>
                e.Zip(o, (x, y) => x + y)
            );

            res.Messages.AssertEqual(
                OnNext(210, 7),
                OnNext(220, 7),
                OnNext(230, 7),
                OnNext(240, 7)
            );

            o.Subscriptions.AssertEqual(
                Subscribe(200, 1000)
            );

            e.Subscriptions.AssertEqual(
                Subscribe(200, 1000)
            );
        }

        [TestMethod]
        public void ZipWithEnumerable_EnumeratorThrowsMoveNext()
        {
            var ex = new Exception();

            var scheduler = new TestScheduler();

            var o1 = scheduler.CreateHotObservable(
                OnNext(150, 1),
                OnNext(215, 2),
                OnNext(225, 4),
                OnCompleted<int>(240)
            );

            var o2 = new MockEnumerable<int>(scheduler,
                new MyEnumerable(false, ex)
            );

            var res = scheduler.Start(() =>
                o1.Zip(o2, (x, y) => x + y)
            );

            res.Messages.AssertEqual(
                OnError<int>(215, ex)
            );

            o1.Subscriptions.AssertEqual(
                Subscribe(200, 215)
            );

            o2.Subscriptions.AssertEqual(
                Subscribe(200, 215)
            );
        }

        [TestMethod]
        public void ZipWithEnumerable_EnumeratorThrowsCurrent()
        {
            var ex = new Exception();

            var scheduler = new TestScheduler();

            var o1 = scheduler.CreateHotObservable(
                OnNext(150, 1),
                OnNext(215, 2),
                OnNext(225, 4),
                OnCompleted<int>(240)
            );

            var o2 = new MockEnumerable<int>(scheduler,
                new MyEnumerable(true, ex)
            );

            var res = scheduler.Start(() =>
                o1.Zip(o2, (x, y) => x + y)
            );

            res.Messages.AssertEqual(
                OnError<int>(215, ex)
            );

            o1.Subscriptions.AssertEqual(
                Subscribe(200, 215)
            );

            o2.Subscriptions.AssertEqual(
                Subscribe(200, 215)
            );
        }

        [TestMethod]
        public void ZipWithEnumerable_SelectorThrows()
        {
            var scheduler = new TestScheduler();

            var o1 = scheduler.CreateHotObservable(
                OnNext(150, 1),
                OnNext(215, 2),
                OnNext(225, 4),
                OnCompleted<int>(240)
            );

            var o2 = new MockEnumerable<int>(scheduler,
                new[] { 3, 5 }
            );

            var ex = new Exception();

            var res = scheduler.Start(() =>
                o1.Zip(o2, (x, y) =>
                {
                    if (y == 5)
                        throw ex;
                    return x + y;
                })
            );

            res.Messages.AssertEqual(
                OnNext(215, 2 + 3),
                OnError<int>(225, ex)
            );

            o1.Subscriptions.AssertEqual(
                Subscribe(200, 225)
            );

            o2.Subscriptions.AssertEqual(
                Subscribe(200, 225)
            );
        }

        private IEnumerable<int> EnumerableNever(ManualResetEvent evt)
        {
            evt.WaitOne();
            yield break;
        }

        private IEnumerable<int> ThrowEnumerable(bool b, Exception ex)
        {
            if (!b)
                throw ex;
            yield break;
        }

        class MyEnumerable : IEnumerable<int>
        {
            private bool _throwInCurrent;
            private Exception _ex;

            public MyEnumerable(bool throwInCurrent, Exception ex)
            {
                _throwInCurrent = throwInCurrent;
                _ex = ex;
            }

            public IEnumerator<int> GetEnumerator()
            {
                return new MyEnumerator(_throwInCurrent, _ex);
            }

            System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
            {
                return GetEnumerator();
            }

            class MyEnumerator : IEnumerator<int>
            {
                private bool _throwInCurrent;
                private Exception _ex;

                public MyEnumerator(bool throwInCurrent, Exception ex)
                {
                    _throwInCurrent = throwInCurrent;
                    _ex = ex;
                }

                public int Current
                {
                    get
                    {
                        if (_throwInCurrent)
                            throw _ex;
                        else
                            return 1;
                    }
                }

                public void Dispose()
                {
                }

                object System.Collections.IEnumerator.Current
                {
                    get { return Current; }
                }

                public bool MoveNext()
                {
                    if (!_throwInCurrent)
                        throw _ex;
                    return true;
                }

                public void Reset()
                {
                }
            }
        }

        #endregion

        #region NAry

        [TestMethod]
        public void Zip_NAry_ArgumentChecking()
        {
            ReactiveAssert.Throws<ArgumentNullException>(() => Observable.Zip(default(IEnumerable<IObservable<int>>)));
            ReactiveAssert.Throws<ArgumentNullException>(() => Observable.Zip(default(IEnumerable<IObservable<int>>), _ => 42));
            ReactiveAssert.Throws<ArgumentNullException>(() => Observable.Zip(new[] { Observable.Return(42) }, default(Func<IList<int>, string>)));
            ReactiveAssert.Throws<ArgumentNullException>(() => Observable.Zip(default(IObservable<int>[])));
        }

        [TestMethod]
        public void Zip_NAry_Symmetric()
        {
            var scheduler = new TestScheduler();

            var e0 = scheduler.CreateHotObservable(new[] { OnNext(150, 1), OnNext(210, 1), OnNext(250, 4), OnCompleted<int>(420) });
            var e1 = scheduler.CreateHotObservable(new[] { OnNext(150, 1), OnNext(220, 2), OnNext(240, 5), OnCompleted<int>(410) });
            var e2 = scheduler.CreateHotObservable(new[] { OnNext(150, 1), OnNext(230, 3), OnNext(260, 6), OnCompleted<int>(400) });

            var res = scheduler.Start(() =>
                Observable.Zip(e0, e1, e2)
            );

            res.Messages.AssertEqual(
                OnNext<IList<int>>(230, l => l.SequenceEqual(new[] { 1, 2, 3 })),
                OnNext<IList<int>>(260, l => l.SequenceEqual(new[] { 4, 5, 6 })),
                OnCompleted<IList<int>>(420)
            );

            e0.Subscriptions.AssertEqual(
                Subscribe(200, 420)
            );

            e1.Subscriptions.AssertEqual(
                Subscribe(200, 410)
            );

            e2.Subscriptions.AssertEqual(
                Subscribe(200, 400)
            );
        }

        [TestMethod]
        public void Zip_NAry_Symmetric_Selector()
        {
            var scheduler = new TestScheduler();

            var e0 = scheduler.CreateHotObservable(new[] { OnNext(150, 1), OnNext(210, 1), OnNext(250, 4), OnCompleted<int>(420) });
            var e1 = scheduler.CreateHotObservable(new[] { OnNext(150, 1), OnNext(220, 2), OnNext(240, 5), OnCompleted<int>(410) });
            var e2 = scheduler.CreateHotObservable(new[] { OnNext(150, 1), OnNext(230, 3), OnNext(260, 6), OnCompleted<int>(400) });

            var res = scheduler.Start(() =>
                Observable.Zip(new[] { e0, e1, e2 }, xs => xs.Sum())
            );

            res.Messages.AssertEqual(
                OnNext<int>(230, new[] { 1, 2, 3 }.Sum()),
                OnNext<int>(260, new[] { 4, 5, 6 }.Sum()),
                OnCompleted<int>(420)
            );

            e0.Subscriptions.AssertEqual(
                Subscribe(200, 420)
            );

            e1.Subscriptions.AssertEqual(
                Subscribe(200, 410)
            );

            e2.Subscriptions.AssertEqual(
                Subscribe(200, 400)
            );
        }

        [TestMethod]
        public void Zip_NAry_Asymmetric()
        {
            var scheduler = new TestScheduler();

            var e0 = scheduler.CreateHotObservable(new[] { OnNext(150, 1), OnNext(210, 1), OnNext(250, 4), OnCompleted<int>(270) });
            var e1 = scheduler.CreateHotObservable(new[] { OnNext(150, 1), OnNext(220, 2), OnNext(240, 5), OnNext(290, 7), OnNext(310, 9), OnCompleted<int>(410) });
            var e2 = scheduler.CreateHotObservable(new[] { OnNext(150, 1), OnNext(230, 3), OnNext(260, 6), OnNext(280, 8), OnCompleted<int>(300) });

            var res = scheduler.Start(() =>
                Observable.Zip(e0, e1, e2)
            );

            res.Messages.AssertEqual(
                OnNext<IList<int>>(230, l => l.SequenceEqual(new[] { 1, 2, 3 })),
                OnNext<IList<int>>(260, l => l.SequenceEqual(new[] { 4, 5, 6 })),
                OnCompleted<IList<int>>(310)
            );

            e0.Subscriptions.AssertEqual(
                Subscribe(200, 270)
            );

            e1.Subscriptions.AssertEqual(
                Subscribe(200, 310)
            );

            e2.Subscriptions.AssertEqual(
                Subscribe(200, 300)
            );
        }

        [TestMethod]
        public void Zip_NAry_Asymmetric_Selector()
        {
            var scheduler = new TestScheduler();

            var e0 = scheduler.CreateHotObservable(new[] { OnNext(150, 1), OnNext(210, 1), OnNext(250, 4), OnCompleted<int>(270) });
            var e1 = scheduler.CreateHotObservable(new[] { OnNext(150, 1), OnNext(220, 2), OnNext(240, 5), OnNext(290, 7), OnNext(310, 9), OnCompleted<int>(410) });
            var e2 = scheduler.CreateHotObservable(new[] { OnNext(150, 1), OnNext(230, 3), OnNext(260, 6), OnNext(280, 8), OnCompleted<int>(300) });

            var res = scheduler.Start(() =>
                Observable.Zip(new[] { e0, e1, e2 }, xs => xs.Sum())
            );

            res.Messages.AssertEqual(
                OnNext<int>(230, new[] { 1, 2, 3 }.Sum()),
                OnNext<int>(260, new[] { 4, 5, 6 }.Sum()),
                OnCompleted<int>(310)
            );

            e0.Subscriptions.AssertEqual(
                Subscribe(200, 270)
            );

            e1.Subscriptions.AssertEqual(
                Subscribe(200, 310)
            );

            e2.Subscriptions.AssertEqual(
                Subscribe(200, 300)
            );
        }

        [TestMethod]
        public void Zip_NAry_Error()
        {
            var ex = new Exception();

            var scheduler = new TestScheduler();

            var e0 = scheduler.CreateHotObservable(new[] { OnNext(150, 1), OnNext(210, 1), OnError<int>(250, ex) });
            var e1 = scheduler.CreateHotObservable(new[] { OnNext(150, 1), OnNext(220, 2), OnNext(240, 5), OnCompleted<int>(410) });
            var e2 = scheduler.CreateHotObservable(new[] { OnNext(150, 1), OnNext(230, 3), OnNext(260, 6), OnCompleted<int>(400) });

            var res = scheduler.Start(() =>
                Observable.Zip(e0, e1, e2)
            );

            res.Messages.AssertEqual(
                OnNext<IList<int>>(230, l => l.SequenceEqual(new[] { 1, 2, 3 })),
                OnError<IList<int>>(250, ex)
            );

            e0.Subscriptions.AssertEqual(
                Subscribe(200, 250)
            );

            e1.Subscriptions.AssertEqual(
                Subscribe(200, 250)
            );

            e2.Subscriptions.AssertEqual(
                Subscribe(200, 250)
            );
        }

        [TestMethod]
        public void Zip_NAry_Error_Selector()
        {
            var ex = new Exception();

            var scheduler = new TestScheduler();

            var e0 = scheduler.CreateHotObservable(new[] { OnNext(150, 1), OnNext(210, 1), OnError<int>(250, ex) });
            var e1 = scheduler.CreateHotObservable(new[] { OnNext(150, 1), OnNext(220, 2), OnNext(240, 5), OnCompleted<int>(410) });
            var e2 = scheduler.CreateHotObservable(new[] { OnNext(150, 1), OnNext(230, 3), OnNext(260, 6), OnCompleted<int>(400) });

            var res = scheduler.Start(() =>
                Observable.Zip(new[] { e0, e1, e2 }, xs => xs.Sum())
            );

            res.Messages.AssertEqual(
                OnNext<int>(230, new[] { 1, 2, 3 }.Sum()),
                OnError<int>(250, ex)
            );

            e0.Subscriptions.AssertEqual(
                Subscribe(200, 250)
            );

            e1.Subscriptions.AssertEqual(
                Subscribe(200, 250)
            );

            e2.Subscriptions.AssertEqual(
                Subscribe(200, 250)
            );
        }

        [TestMethod]
        public void Zip_NAry_Enumerable_Simple()
        {
            var scheduler = new TestScheduler();

            var e0 = scheduler.CreateHotObservable(new[] { OnNext(150, 1), OnNext(210, 1), OnNext(250, 4), OnCompleted<int>(420) });
            var e1 = scheduler.CreateHotObservable(new[] { OnNext(150, 1), OnNext(220, 2), OnNext(240, 5), OnCompleted<int>(410) });
            var e2 = scheduler.CreateHotObservable(new[] { OnNext(150, 1), OnNext(230, 3), OnNext(260, 6), OnCompleted<int>(400) });

            var started = default(long);
            var xss = GetSources(() => started = scheduler.Clock, e0, e1, e2).Select(xs => (IObservable<int>)xs);

            var res = scheduler.Start(() =>
                Observable.Zip(xss)
            );

            Assert.AreEqual(200, started);

            res.Messages.AssertEqual(
                OnNext<IList<int>>(230, l => l.SequenceEqual(new[] { 1, 2, 3 })),
                OnNext<IList<int>>(260, l => l.SequenceEqual(new[] { 4, 5, 6 })),
                OnCompleted<IList<int>>(420)
            );

            e0.Subscriptions.AssertEqual(
                Subscribe(200, 420)
            );

            e1.Subscriptions.AssertEqual(
                Subscribe(200, 410)
            );

            e2.Subscriptions.AssertEqual(
                Subscribe(200, 400)
            );
        }

        [TestMethod]
        public void Zip_NAry_Enumerable_Throws()
        {
            var ex = new Exception();
            var xss = GetSources(ex, Observable.Return(42));
            var res = Observable.Zip(xss);

            ReactiveAssert.Throws(ex, () => res.Subscribe(_ => { }));
        }

        private IEnumerable<ITestableObservable<int>> GetSources(Action start, params ITestableObservable<int>[] sources)
        {
            start();

            foreach (var xs in sources)
                yield return xs;
        }

        private IEnumerable<IObservable<T>> GetSources<T>(Exception ex, params IObservable<T>[] sources)
        {
            foreach (var xs in sources)
                yield return xs;

            throw ex;
        }

        #endregion

        #region AtLeastOneThrows

        [TestMethod]
        public void Zip_AtLeastOneThrows4()
        {
            var ex = new Exception();

            var scheduler = new TestScheduler();

            var e0 = scheduler.CreateHotObservable(new[] { OnNext(150, 1), OnNext(210, 1), OnCompleted<int>(400) });
            var e1 = scheduler.CreateHotObservable(new[] { OnNext(150, 1), OnNext(220, 2), OnCompleted<int>(400) });
            var e2 = scheduler.CreateHotObservable(new[] { OnNext(150, 1), OnError<int>(230, ex) });
            var e3 = scheduler.CreateHotObservable(new[] { OnNext(150, 1), OnNext(240, 4), OnCompleted<int>(400) });

            var res = scheduler.Start(() =>
                Observable.Zip(e0, e1, e2, e3, (_0, _1, _2, _3) => 42)
            );
            
            res.Messages.AssertEqual(
                OnError<int>(230, ex)
            );

            e0.Subscriptions.AssertEqual(Subscribe(200, 230));
            e1.Subscriptions.AssertEqual(Subscribe(200, 230));
            e2.Subscriptions.AssertEqual(Subscribe(200, 230));
            e3.Subscriptions.AssertEqual(Subscribe(200, 230));
        }

        #endregion

        #endregion
    }
}

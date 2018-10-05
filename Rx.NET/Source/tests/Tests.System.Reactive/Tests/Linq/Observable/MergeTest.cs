// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the Apache 2.0 License.
// See the LICENSE file in the project root for more information. 

using System;
using System.Collections.Generic;
using System.Linq;
using System.Reactive;
using System.Reactive.Concurrency;
using System.Reactive.Linq;
using System.Reactive.Subjects;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Reactive.Testing;
using ReactiveTests.Dummies;
using Xunit;

namespace ReactiveTests.Tests
{
    public class MergeTest : ReactiveTest
    {

        [Fact]
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
            ReactiveAssert.Throws<ArgumentNullException>(() => xs.Merge(default, DummyScheduler.Instance));
            ReactiveAssert.Throws<ArgumentNullException>(() => Observable.Merge((IEnumerable<IObservable<int>>)null, DummyScheduler.Instance));
            ReactiveAssert.Throws<ArgumentNullException>(() => Observable.Merge(new IObservable<int>[0], default(IScheduler)));
            ReactiveAssert.Throws<ArgumentNullException>(() => Observable.Merge((IObservable<IObservable<int>>)null));
            ReactiveAssert.Throws<ArgumentNullException>(() => Observable.Merge(DummyScheduler.Instance, (IObservable<int>[])null));
            ReactiveAssert.Throws<ArgumentNullException>(() => Observable.Merge((IObservable<Task<int>>)null));
        }

        [Fact]
        public void Merge_DefaultScheduler()
        {
            var xs = Observable.Merge(Observable.Return(42), Observable.Return(43), Observable.Return(44));
            var res = xs.ToList().Single();
            Assert.True(new[] { 42, 43, 44 }.SequenceEqual(res));
        }

        [Fact]
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

        [Fact]
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

        [Fact]
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

        [Fact]
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

        [Fact]
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

        [Fact]
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

        [Fact]
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

        [Fact]
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

        [Fact]
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

        [Fact]
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

        [Fact]
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

        [Fact]
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

        [Fact]
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

        [Fact]
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

        [Fact]
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

        [Fact]
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

        [Fact]
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

        [Fact]
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

            var scheduler = new TestScheduler();

            // Last ToArray: got to create the hot observables *now*
            var xss = inputs.Select(lst => (IObservable<int>)scheduler.CreateHotObservable(lst.ToArray())).ToArray();

            var res = scheduler.Start(() =>
                xss.Merge(scheduler)
            );

            Assert.True(resOnNext.Count + 1 == res.Messages.Count, "length");
            for (var i = 0; i < resOnNext.Count; i++)
            {
                var msg = res.Messages[i];
                Assert.True(msg.Time == resOnNext[i].Time);
                Assert.True(msg.Value.Kind == NotificationKind.OnNext);
                Assert.True(msg.Value.Value == resOnNext[i].Value.Value);
            }
            Assert.True(res.Messages[resOnNext.Count].Value.Kind == NotificationKind.OnCompleted && res.Messages[resOnNext.Count].Time == lastCompleted.Time, "complete");
        }

        [Fact]
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

        [Fact]
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

        [Fact]
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

        [Fact]
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

        [Fact]
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

        [Fact]
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

        [Fact]
        public void Merge_Binary_DefaultScheduler()
        {
            Assert.True(Observable.Return(1).Merge(Observable.Return(2)).ToEnumerable().OrderBy(x => x).SequenceEqual(new[] { 1, 2 }));
        }

        [Fact]
        public void Merge_Params_DefaultScheduler()
        {
            Assert.True(Observable.Merge(Observable.Return(1), Observable.Return(2)).ToEnumerable().OrderBy(x => x).SequenceEqual(new[] { 1, 2 }));
        }

        [Fact]
        public void Merge_IEnumerableOfIObservable_DefaultScheduler()
        {
            Assert.True(Observable.Merge((IEnumerable<IObservable<int>>)new[] { Observable.Return(1), Observable.Return(2) }).ToEnumerable().OrderBy(x => x).SequenceEqual(new[] { 1, 2 }));
        }

        [Fact]
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
            ReactiveAssert.Throws<ArgumentNullException>(() => Observable.Concat(default(IObservable<Task<int>>)));
        }

        [Fact]
        public void MergeConcat_Enumerable_Scheduler()
        {
            var b = Enumerable.Range(1, 3).Select(x => Observable.Range(x * 10, 3)).Merge(1)
                    .SequenceEqual(new[] { 10, 11, 12, 20, 21, 22, 30, 31, 32 }.ToObservable())
                    .First();
            Assert.True(b);
        }

        [Fact]
        public void MergeConcat_Enumerable()
        {
            var b = Enumerable.Range(1, 3).Select(x => Observable.Range(x * 10, 3)).Merge(1, DefaultScheduler.Instance)
                    .SequenceEqual(new[] { 10, 11, 12, 20, 21, 22, 30, 31, 32 }.ToObservable())
                    .First();
            Assert.True(b);
        }

        [Fact]
        public void MergeConcat_Default()
        {
            var b = Observable.Range(1, 3).Select(x => Observable.Range(x * 10, 3)).Concat()
                    .SequenceEqual(new[] { 10, 11, 12, 20, 21, 22, 30, 31, 32 }.ToObservable())
                    .First();
            Assert.True(b);
        }

        [Fact]
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

        [Fact]
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

        [Fact]
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

        [Fact]
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

        [Fact]
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

        [Fact]
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

        [Fact]
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

        [Fact]
        public void Merge_Task()
        {
            var tss = Observable.Merge(new[] { Task.Factory.StartNew(() => 1), Task.Factory.StartNew(() => 2), Task.Factory.StartNew(() => 3) }.ToObservable());

            var res = tss.ToArray().Single();

            Assert.True(res.OrderBy(x => x).SequenceEqual(new[] { 1, 2, 3 }));
        }

        [Fact]
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

        [Fact]
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

        [Fact]
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
            Assert.Same(ex, err);
        }

        [Fact]
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
            Assert.Same(ex, err);
        }

        [Fact]
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
            Assert.True(err is TaskCanceledException && ((TaskCanceledException)err).Task == tcss[1].Task);
        }

        [Fact]
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
            Assert.True(err is TaskCanceledException && ((TaskCanceledException)err).Task == tcss[1].Task);
        }

        [Fact]
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

        [Fact]
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

        [Fact]
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

            Assert.Same(ex, err);
        }

    }
}

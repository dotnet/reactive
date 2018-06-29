// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the Apache 2.0 License.
// See the LICENSE file in the project root for more information. 

using System;
using System.Collections.Generic;
using System.Linq;
using System.Reactive.Linq;
using Microsoft.Reactive.Testing;
using ReactiveTests.Dummies;
using Xunit;

namespace ReactiveTests.Tests
{
    public class SequenceEqualTest : ReactiveTest
    {

        [Fact]
        public void SequenceEqual_ArgumentChecking()
        {
            ReactiveAssert.Throws<ArgumentNullException>(() => Observable.SequenceEqual(default, DummyObservable<int>.Instance));
            ReactiveAssert.Throws<ArgumentNullException>(() => Observable.SequenceEqual(DummyObservable<int>.Instance, default(IObservable<int>)));
            ReactiveAssert.Throws<ArgumentNullException>(() => Observable.SequenceEqual(default, DummyObservable<int>.Instance, EqualityComparer<int>.Default));
            ReactiveAssert.Throws<ArgumentNullException>(() => Observable.SequenceEqual(DummyObservable<int>.Instance, default(IObservable<int>), EqualityComparer<int>.Default));
            ReactiveAssert.Throws<ArgumentNullException>(() => Observable.SequenceEqual(DummyObservable<int>.Instance, DummyObservable<int>.Instance, default));

            ReactiveAssert.Throws<ArgumentNullException>(() => Observable.SequenceEqual(default, new[] { 42 }));
            ReactiveAssert.Throws<ArgumentNullException>(() => Observable.SequenceEqual(DummyObservable<int>.Instance, default(IEnumerable<int>)));
            ReactiveAssert.Throws<ArgumentNullException>(() => Observable.SequenceEqual(default, new[] { 42 }, EqualityComparer<int>.Default));
            ReactiveAssert.Throws<ArgumentNullException>(() => Observable.SequenceEqual(DummyObservable<int>.Instance, default(IEnumerable<int>), EqualityComparer<int>.Default));
            ReactiveAssert.Throws<ArgumentNullException>(() => Observable.SequenceEqual(DummyObservable<int>.Instance, new[] { 42 }, default));
        }

        [Fact]
        public void SequenceEqual_Observable_Equal()
        {
            var scheduler = new TestScheduler();

            var xs = scheduler.CreateHotObservable(
                OnNext(110, 1),
                OnNext(190, 2),
                OnNext(240, 3),
                OnNext(290, 4),
                OnNext(310, 5),
                OnNext(340, 6),
                OnNext(450, 7),
                OnCompleted<int>(510)
            );

            var ys = scheduler.CreateHotObservable(
                OnNext(90, 1),
                OnNext(270, 3),
                OnNext(280, 4),
                OnNext(300, 5),
                OnNext(330, 6),
                OnNext(340, 7),
                OnCompleted<int>(720)
            );

            var res = scheduler.Start(() =>
                xs.SequenceEqual(ys)
            );

            res.Messages.AssertEqual(
                OnNext(720, true),
                OnCompleted<bool>(720)
            );

            xs.Subscriptions.AssertEqual(
                Subscribe(200, 720)
            );

            ys.Subscriptions.AssertEqual(
                Subscribe(200, 720)
            );
        }

        [Fact]
        public void SequenceEqual_Observable_Equal_Sym()
        {
            var scheduler = new TestScheduler();

            var xs = scheduler.CreateHotObservable(
                OnNext(110, 1),
                OnNext(190, 2),
                OnNext(240, 3),
                OnNext(290, 4),
                OnNext(310, 5),
                OnNext(340, 6),
                OnNext(450, 7),
                OnCompleted<int>(510)
            );

            var ys = scheduler.CreateHotObservable(
                OnNext(90, 1),
                OnNext(270, 3),
                OnNext(280, 4),
                OnNext(300, 5),
                OnNext(330, 6),
                OnNext(340, 7),
                OnCompleted<int>(720)
            );

            var res = scheduler.Start(() =>
                ys.SequenceEqual(xs)
            );

            res.Messages.AssertEqual(
                OnNext(720, true),
                OnCompleted<bool>(720)
            );

            xs.Subscriptions.AssertEqual(
                Subscribe(200, 720)
            );

            ys.Subscriptions.AssertEqual(
                Subscribe(200, 720)
            );
        }

        [Fact]
        public void SequenceEqual_Observable_NotEqual_Left()
        {
            var scheduler = new TestScheduler();

            var xs = scheduler.CreateHotObservable(
                OnNext(110, 1),
                OnNext(190, 2),
                OnNext(240, 3),
                OnNext(290, 4),
                OnNext(310, 0),
                OnNext(340, 6),
                OnNext(450, 7),
                OnCompleted<int>(510)
            );

            var ys = scheduler.CreateHotObservable(
                OnNext(90, 1),
                OnNext(270, 3),
                OnNext(280, 4),
                OnNext(300, 5),
                OnNext(330, 6),
                OnNext(340, 7),
                OnCompleted<int>(720)
            );

            var res = scheduler.Start(() =>
                xs.SequenceEqual(ys)
            );

            res.Messages.AssertEqual(
                OnNext(310, false),
                OnCompleted<bool>(310)
            );

            xs.Subscriptions.AssertEqual(
                Subscribe(200, 310)
            );

            ys.Subscriptions.AssertEqual(
                Subscribe(200, 310)
            );
        }

        [Fact]
        public void SequenceEqual_Observable_NotEqual_Left_Sym()
        {
            var scheduler = new TestScheduler();

            var xs = scheduler.CreateHotObservable(
                OnNext(110, 1),
                OnNext(190, 2),
                OnNext(240, 3),
                OnNext(290, 4),
                OnNext(310, 0),
                OnNext(340, 6),
                OnNext(450, 7),
                OnCompleted<int>(510)
            );

            var ys = scheduler.CreateHotObservable(
                OnNext(90, 1),
                OnNext(270, 3),
                OnNext(280, 4),
                OnNext(300, 5),
                OnNext(330, 6),
                OnNext(340, 7),
                OnCompleted<int>(720)
            );

            var res = scheduler.Start(() =>
                ys.SequenceEqual(xs)
            );

            res.Messages.AssertEqual(
                OnNext(310, false),
                OnCompleted<bool>(310)
            );

            xs.Subscriptions.AssertEqual(
                Subscribe(200, 310)
            );

            ys.Subscriptions.AssertEqual(
                Subscribe(200, 310)
            );
        }

        [Fact]
        public void SequenceEqual_Observable_NotEqual_Right()
        {
            var scheduler = new TestScheduler();

            var xs = scheduler.CreateHotObservable(
                OnNext(110, 1),
                OnNext(190, 2),
                OnNext(240, 3),
                OnNext(290, 4),
                OnNext(310, 5),
                OnNext(340, 6),
                OnNext(450, 7),
                OnCompleted<int>(510)
            );

            var ys = scheduler.CreateHotObservable(
                OnNext(90, 1),
                OnNext(270, 3),
                OnNext(280, 4),
                OnNext(300, 5),
                OnNext(330, 6),
                OnNext(340, 7),
                OnNext(350, 8)
            );

            var res = scheduler.Start(() =>
                xs.SequenceEqual(ys)
            );

            res.Messages.AssertEqual(
                OnNext(510, false),
                OnCompleted<bool>(510)
            );

            xs.Subscriptions.AssertEqual(
                Subscribe(200, 510)
            );

            ys.Subscriptions.AssertEqual(
                Subscribe(200, 510)
            );
        }

        [Fact]
        public void SequenceEqual_Observable_NotEqual_Right_Sym()
        {
            var scheduler = new TestScheduler();

            var xs = scheduler.CreateHotObservable(
                OnNext(110, 1),
                OnNext(190, 2),
                OnNext(240, 3),
                OnNext(290, 4),
                OnNext(310, 5),
                OnNext(340, 6),
                OnNext(450, 7),
                OnCompleted<int>(510)
            );

            var ys = scheduler.CreateHotObservable(
                OnNext(90, 1),
                OnNext(270, 3),
                OnNext(280, 4),
                OnNext(300, 5),
                OnNext(330, 6),
                OnNext(340, 7),
                OnNext(350, 8)
            );

            var res = scheduler.Start(() =>
                ys.SequenceEqual(xs)
            );

            res.Messages.AssertEqual(
                OnNext(510, false),
                OnCompleted<bool>(510)
            );

            xs.Subscriptions.AssertEqual(
                Subscribe(200, 510)
            );

            ys.Subscriptions.AssertEqual(
                Subscribe(200, 510)
            );
        }

        [Fact]
        public void SequenceEqual_Observable_NotEqual_2()
        {
            var scheduler = new TestScheduler();

            var xs = scheduler.CreateHotObservable(
                OnNext(110, 1),
                OnNext(190, 2),
                OnNext(240, 3),
                OnNext(290, 4),
                OnNext(310, 5),
                OnNext(340, 6),
                OnNext(450, 7),
                OnNext(490, 8),
                OnNext(520, 9),
                OnNext(580, 10),
                OnNext(600, 11)
            );

            var ys = scheduler.CreateHotObservable(
                OnNext(90, 1),
                OnNext(270, 3),
                OnNext(280, 4),
                OnNext(300, 5),
                OnNext(330, 6),
                OnNext(340, 7),
                OnNext(350, 9),
                OnNext(400, 9),
                OnNext(410, 10),
                OnNext(490, 11),
                OnNext(550, 12),
                OnNext(560, 13)
            );

            var res = scheduler.Start(() =>
                xs.SequenceEqual(ys)
            );

            res.Messages.AssertEqual(
                OnNext(490, false),
                OnCompleted<bool>(490)
            );

            xs.Subscriptions.AssertEqual(
                Subscribe(200, 490)
            );

            ys.Subscriptions.AssertEqual(
                Subscribe(200, 490)
            );
        }

        [Fact]
        public void SequenceEqual_Observable_NotEqual_2_Sym()
        {
            var scheduler = new TestScheduler();

            var xs = scheduler.CreateHotObservable(
                OnNext(110, 1),
                OnNext(190, 2),
                OnNext(240, 3),
                OnNext(290, 4),
                OnNext(310, 5),
                OnNext(340, 6),
                OnNext(450, 7),
                OnNext(490, 8),
                OnNext(520, 9),
                OnNext(580, 10),
                OnNext(600, 11)
            );

            var ys = scheduler.CreateHotObservable(
                OnNext(90, 1),
                OnNext(270, 3),
                OnNext(280, 4),
                OnNext(300, 5),
                OnNext(330, 6),
                OnNext(340, 7),
                OnNext(350, 9),
                OnNext(400, 9),
                OnNext(410, 10),
                OnNext(490, 11),
                OnNext(550, 12),
                OnNext(560, 13)
            );

            var res = scheduler.Start(() =>
                ys.SequenceEqual(xs)
            );

            res.Messages.AssertEqual(
                OnNext(490, false),
                OnCompleted<bool>(490)
            );

            xs.Subscriptions.AssertEqual(
                Subscribe(200, 490)
            );

            ys.Subscriptions.AssertEqual(
                Subscribe(200, 490)
            );
        }

        [Fact]
        public void SequenceEqual_Observable_NotEqual_3()
        {
            var scheduler = new TestScheduler();

            var xs = scheduler.CreateHotObservable(
                OnNext(110, 1),
                OnNext(190, 2),
                OnNext(240, 3),
                OnNext(290, 4),
                OnNext(310, 5),
                OnCompleted<int>(330)
            );

            var ys = scheduler.CreateHotObservable(
                OnNext(90, 1),
                OnNext(270, 3),
                OnNext(400, 4),
                OnCompleted<int>(420)
            );

            var res = scheduler.Start(() =>
                xs.SequenceEqual(ys)
            );

            res.Messages.AssertEqual(
                OnNext(420, false),
                OnCompleted<bool>(420)
            );

            xs.Subscriptions.AssertEqual(
                Subscribe(200, 420)
            );

            ys.Subscriptions.AssertEqual(
                Subscribe(200, 420)
            );
        }

        [Fact]
        public void SequenceEqual_Observable_NotEqual_3_Sym()
        {
            var scheduler = new TestScheduler();

            var xs = scheduler.CreateHotObservable(
                OnNext(110, 1),
                OnNext(190, 2),
                OnNext(240, 3),
                OnNext(290, 4),
                OnNext(310, 5),
                OnCompleted<int>(330)
            );

            var ys = scheduler.CreateHotObservable(
                OnNext(90, 1),
                OnNext(270, 3),
                OnNext(400, 4),
                OnCompleted<int>(420)
            );

            var res = scheduler.Start(() =>
                ys.SequenceEqual(xs)
            );

            res.Messages.AssertEqual(
                OnNext(420, false),
                OnCompleted<bool>(420)
            );

            xs.Subscriptions.AssertEqual(
                Subscribe(200, 420)
            );

            ys.Subscriptions.AssertEqual(
                Subscribe(200, 420)
            );
        }

        [Fact]
        public void SequenceEqual_Observable_ComparerThrows()
        {
            var scheduler = new TestScheduler();

            var xs = scheduler.CreateHotObservable(
                OnNext(110, 1),
                OnNext(190, 2),
                OnNext(240, 3),
                OnNext(290, 4),
                OnNext(310, 5),
                OnCompleted<int>(330)
            );

            var ys = scheduler.CreateHotObservable(
                OnNext(90, 1),
                OnNext(270, 3),
                OnNext(400, 4),
                OnCompleted<int>(420)
            );

            var ex = new Exception();

            var res = scheduler.Start(() =>
                xs.SequenceEqual(ys, new ThrowComparer(ex))
            );

            res.Messages.AssertEqual(
                OnError<bool>(270, ex)
            );

            xs.Subscriptions.AssertEqual(
                Subscribe(200, 270)
            );

            ys.Subscriptions.AssertEqual(
                Subscribe(200, 270)
            );
        }

        [Fact]
        public void SequenceEqual_Observable_ComparerThrows_Sym()
        {
            var scheduler = new TestScheduler();

            var xs = scheduler.CreateHotObservable(
                OnNext(110, 1),
                OnNext(190, 2),
                OnNext(240, 3),
                OnNext(290, 4),
                OnNext(310, 5),
                OnCompleted<int>(330)
            );

            var ys = scheduler.CreateHotObservable(
                OnNext(90, 1),
                OnNext(270, 3),
                OnNext(400, 4),
                OnCompleted<int>(420)
            );

            var ex = new Exception();

            var res = scheduler.Start(() =>
                ys.SequenceEqual(xs, new ThrowComparer(ex))
            );

            res.Messages.AssertEqual(
                OnError<bool>(270, ex)
            );

            xs.Subscriptions.AssertEqual(
                Subscribe(200, 270)
            );

            ys.Subscriptions.AssertEqual(
                Subscribe(200, 270)
            );
        }

        private class ThrowComparer : IEqualityComparer<int>
        {
            private readonly Exception _ex;

            public ThrowComparer(Exception ex)
            {
                _ex = ex;
            }

            public bool Equals(int x, int y)
            {
                throw _ex;
            }

            public int GetHashCode(int obj)
            {
                throw new NotImplementedException();
            }
        }

        [Fact]
        public void SequenceEqual_Observable_NotEqual_4()
        {
            var scheduler = new TestScheduler();

            var xs = scheduler.CreateHotObservable(
                OnNext(250, 1),
                OnCompleted<int>(300)
            );

            var ys = scheduler.CreateHotObservable(
                OnNext(290, 1),
                OnNext(310, 2),
                OnCompleted<int>(350)
            );

            var res = scheduler.Start(() =>
                xs.SequenceEqual(ys)
            );

            res.Messages.AssertEqual(
                OnNext(310, false),
                OnCompleted<bool>(310)
            );

            xs.Subscriptions.AssertEqual(
                Subscribe(200, 310)
            );

            ys.Subscriptions.AssertEqual(
                Subscribe(200, 310)
            );
        }

        [Fact]
        public void SequenceEqual_Observable_NotEqual_4_Sym()
        {
            var scheduler = new TestScheduler();

            var xs = scheduler.CreateHotObservable(
                OnNext(250, 1),
                OnCompleted<int>(300)
            );

            var ys = scheduler.CreateHotObservable(
                OnNext(290, 1),
                OnNext(310, 2),
                OnCompleted<int>(350)
            );

            var res = scheduler.Start(() =>
                ys.SequenceEqual(xs)
            );

            res.Messages.AssertEqual(
                OnNext(310, false),
                OnCompleted<bool>(310)
            );

            xs.Subscriptions.AssertEqual(
                Subscribe(200, 310)
            );

            ys.Subscriptions.AssertEqual(
                Subscribe(200, 310)
            );
        }

        [Fact]
        public void SequenceEqual_Observable_Left_Throw()
        {
            var scheduler = new TestScheduler();

            var ex = new Exception();

            var xs = scheduler.CreateHotObservable(
                OnNext(250, 1),
                OnError<int>(300, ex)
            );

            var ys = scheduler.CreateHotObservable(
                OnNext(290, 1),
                OnNext(310, 2),
                OnCompleted<int>(350)
            );

            var res = scheduler.Start(() =>
                ys.SequenceEqual(xs)
            );

            res.Messages.AssertEqual(
                OnError<bool>(300, ex)
            );

            xs.Subscriptions.AssertEqual(
                Subscribe(200, 300)
            );

            ys.Subscriptions.AssertEqual(
                Subscribe(200, 300)
            );
        }

        [Fact]
        public void SequenceEqual_Observable_Right_Throw()
        {
            var scheduler = new TestScheduler();

            var ex = new Exception();

            var xs = scheduler.CreateHotObservable(
                OnNext(290, 1),
                OnNext(310, 2),
                OnCompleted<int>(350)
            );

            var ys = scheduler.CreateHotObservable(
                OnNext(250, 1),
                OnError<int>(300, ex)
            );

            var res = scheduler.Start(() =>
                ys.SequenceEqual(xs)
            );

            res.Messages.AssertEqual(
                OnError<bool>(300, ex)
            );

            xs.Subscriptions.AssertEqual(
                Subscribe(200, 300)
            );

            ys.Subscriptions.AssertEqual(
                Subscribe(200, 300)
            );
        }

        [Fact]
        public void SequenceEqual_Enumerable_Equal()
        {
            var scheduler = new TestScheduler();

            var xs = scheduler.CreateHotObservable(
                OnNext(110, 1),
                OnNext(190, 2),
                OnNext(240, 3),
                OnNext(290, 4),
                OnNext(310, 5),
                OnNext(340, 6),
                OnNext(450, 7),
                OnCompleted<int>(510)
            );

            var res = scheduler.Start(() =>
                xs.SequenceEqual(new[] { 3, 4, 5, 6, 7 })
            );

            res.Messages.AssertEqual(
                OnNext(510, true),
                OnCompleted<bool>(510)
            );

            xs.Subscriptions.AssertEqual(
                Subscribe(200, 510)
            );
        }

        [Fact]
        public void SequenceEqual_Enumerable_NotEqual_Elements()
        {
            var scheduler = new TestScheduler();

            var xs = scheduler.CreateHotObservable(
                OnNext(110, 1),
                OnNext(190, 2),
                OnNext(240, 3),
                OnNext(290, 4),
                OnNext(310, 5),
                OnNext(340, 6),
                OnNext(450, 7),
                OnCompleted<int>(510)
            );

            var res = scheduler.Start(() =>
                xs.SequenceEqual(new[] { 3, 4, 9, 6, 7 })
            );

            res.Messages.AssertEqual(
                OnNext(310, false),
                OnCompleted<bool>(310)
            );

            xs.Subscriptions.AssertEqual(
                Subscribe(200, 310)
            );
        }

        [Fact]
        public void SequenceEqual_Enumerable_Comparer_Equal()
        {
            var scheduler = new TestScheduler();

            var xs = scheduler.CreateHotObservable(
                OnNext(110, 1),
                OnNext(190, 2),
                OnNext(240, 3),
                OnNext(290, 4),
                OnNext(310, 5),
                OnNext(340, 6),
                OnNext(450, 7),
                OnCompleted<int>(510)
            );

            var res = scheduler.Start(() =>
                xs.SequenceEqual(new[] { 3 - 2, 4, 5, 6 + 42, 7 - 6 }, new OddEvenComparer())
            );

            res.Messages.AssertEqual(
                OnNext(510, true),
                OnCompleted<bool>(510)
            );

            xs.Subscriptions.AssertEqual(
                Subscribe(200, 510)
            );
        }

        [Fact]
        public void SequenceEqual_Enumerable_Comparer_NotEqual()
        {
            var scheduler = new TestScheduler();

            var xs = scheduler.CreateHotObservable(
                OnNext(110, 1),
                OnNext(190, 2),
                OnNext(240, 3),
                OnNext(290, 4),
                OnNext(310, 5),
                OnNext(340, 6),
                OnNext(450, 7),
                OnCompleted<int>(510)
            );

            var res = scheduler.Start(() =>
                xs.SequenceEqual(new[] { 3 - 2, 4, 5 + 9, 6 + 42, 7 - 6 }, new OddEvenComparer())
            );

            res.Messages.AssertEqual(
                OnNext(310, false),
                OnCompleted<bool>(310)
            );

            xs.Subscriptions.AssertEqual(
                Subscribe(200, 310)
            );
        }

        private class OddEvenComparer : IEqualityComparer<int>
        {
            public bool Equals(int x, int y)
            {
                return x % 2 == y % 2;
            }

            public int GetHashCode(int obj)
            {
                return (obj % 2).GetHashCode();
            }
        }

        [Fact]
        public void SequenceEqual_Enumerable_Comparer_Throws()
        {
            var scheduler = new TestScheduler();

            var xs = scheduler.CreateHotObservable(
                OnNext(110, 1),
                OnNext(190, 2),
                OnNext(240, 3),
                OnNext(290, 4),
                OnNext(310, 5),
                OnNext(340, 6),
                OnNext(450, 7),
                OnCompleted<int>(510)
            );

            var ex = new Exception();

            var res = scheduler.Start(() =>
                xs.SequenceEqual(new[] { 3, 4, 5, 6, 7 }, new ThrowingComparer(5, ex))
            );

            res.Messages.AssertEqual(
                OnError<bool>(310, ex)
            );

            xs.Subscriptions.AssertEqual(
                Subscribe(200, 310)
            );
        }

        private class ThrowingComparer : IEqualityComparer<int>
        {
            private readonly int _x;
            private readonly Exception _ex;

            public ThrowingComparer(int x, Exception ex)
            {
                _x = x;
                _ex = ex;
            }

            public bool Equals(int x, int y)
            {
                if (x == _x)
                {
                    throw _ex;
                }

                return x == y;
            }

            public int GetHashCode(int obj)
            {
                return obj.GetHashCode();
            }
        }

        [Fact]
        public void SequenceEqual_Enumerable_NotEqual_TooLong()
        {
            var scheduler = new TestScheduler();

            var xs = scheduler.CreateHotObservable(
                OnNext(110, 1),
                OnNext(190, 2),
                OnNext(240, 3),
                OnNext(290, 4),
                OnNext(310, 5),
                OnNext(340, 6),
                OnNext(450, 7),
                OnCompleted<int>(510)
            );

            var res = scheduler.Start(() =>
                xs.SequenceEqual(new[] { 3, 4, 5, 6, 7, 8 })
            );

            res.Messages.AssertEqual(
                OnNext(510, false),
                OnCompleted<bool>(510)
            );

            xs.Subscriptions.AssertEqual(
                Subscribe(200, 510)
            );
        }

        [Fact]
        public void SequenceEqual_Enumerable_NotEqual_TooShort()
        {
            var scheduler = new TestScheduler();

            var xs = scheduler.CreateHotObservable(
                OnNext(110, 1),
                OnNext(190, 2),
                OnNext(240, 3),
                OnNext(290, 4),
                OnNext(310, 5),
                OnNext(340, 6),
                OnNext(450, 7),
                OnCompleted<int>(510)
            );

            var res = scheduler.Start(() =>
                xs.SequenceEqual(new[] { 3, 4, 5, 6 })
            );

            res.Messages.AssertEqual(
                OnNext(450, false),
                OnCompleted<bool>(450)
            );

            xs.Subscriptions.AssertEqual(
                Subscribe(200, 450)
            );
        }

        [Fact]
        public void SequenceEqual_Enumerable_OnError()
        {
            var ex = new Exception();

            var scheduler = new TestScheduler();

            var xs = scheduler.CreateHotObservable(
                OnNext(110, 1),
                OnNext(190, 2),
                OnNext(240, 3),
                OnNext(290, 4),
                OnError<int>(310, ex)
            );

            var res = scheduler.Start(() =>
                xs.SequenceEqual(new[] { 3, 4 })
            );

            res.Messages.AssertEqual(
                OnError<bool>(310, ex)
            );

            xs.Subscriptions.AssertEqual(
                Subscribe(200, 310)
            );
        }

        [Fact]
        public void SequenceEqual_Enumerable_IteratorThrows1()
        {
            var ex = new Exception();

            var scheduler = new TestScheduler();

            var xs = scheduler.CreateHotObservable(
                OnNext(110, 1),
                OnNext(190, 2),
                OnNext(240, 3),
                OnNext(290, 4),
                OnCompleted<int>(310)
            );

            var res = scheduler.Start(() =>
                xs.SequenceEqual(Throw(ex))
            );

            res.Messages.AssertEqual(
                OnError<bool>(290, ex)
            );

            xs.Subscriptions.AssertEqual(
                Subscribe(200, 290)
            );
        }

        [Fact]
        public void SequenceEqual_Enumerable_IteratorThrows2()
        {
            var ex = new Exception();

            var scheduler = new TestScheduler();

            var xs = scheduler.CreateHotObservable(
                OnNext(110, 1),
                OnNext(190, 2),
                OnNext(240, 3),
                OnCompleted<int>(310)
            );

            var res = scheduler.Start(() =>
                xs.SequenceEqual(Throw(ex))
            );

            res.Messages.AssertEqual(
                OnError<bool>(310, ex)
            );

            xs.Subscriptions.AssertEqual(
                Subscribe(200, 310)
            );
        }

        private IEnumerable<int> Throw(Exception ex)
        {
            yield return 3;
            throw ex;
        }

        [Fact]
        public void SequenceEqual_Enumerable_GetEnumeratorThrows()
        {
            var ex = new Exception();

            var scheduler = new TestScheduler();

            var xs = scheduler.CreateHotObservable(
                OnNext(110, 1),
                OnNext(190, 2),
                OnNext(240, 3),
                OnCompleted<int>(310)
            );

            var res = scheduler.Start(() =>
                xs.SequenceEqual(new RogueEnumerable<int>(ex))
            );

            res.Messages.AssertEqual(
                OnError<bool>(200, ex)
            );

            xs.Subscriptions.AssertEqual(
            );
        }

    }
}

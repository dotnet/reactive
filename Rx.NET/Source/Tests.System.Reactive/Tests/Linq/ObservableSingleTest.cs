// Copyright (c) Microsoft Open Technologies, Inc. All rights reserved. See License.txt in the project root for license information.

using System;
using System.Collections.Generic;
using System.Linq;
using System.Reactive;
using System.Reactive.Concurrency;
using System.Reactive.Linq;
using Microsoft.Reactive.Testing;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using ReactiveTests.Dummies;

namespace ReactiveTests.Tests
{
    [TestClass]
    public class ObservableSingleTest : ReactiveTest
    {
        #region + AsObservable +

        [TestMethod]
        public void AsObservable_ArgumentChecking()
        {
            ReactiveAssert.Throws<ArgumentNullException>(() => Observable.AsObservable<int>(null));
        }

        [TestMethod]
        public void AsObservable_AsObservable()
        {
            var scheduler = new TestScheduler();

            var xs = scheduler.CreateHotObservable(
                OnNext(150, 1),
                OnNext(220, 2),
                OnCompleted<int>(250)
            );

            var ys = xs.AsObservable().AsObservable();

            Assert.AreNotSame(xs, ys);

            var res = scheduler.Start(() =>
                ys
            );

            res.Messages.AssertEqual(
                OnNext(220, 2),
                OnCompleted<int>(250)
            );

            xs.Subscriptions.AssertEqual(
                Subscribe(200, 250)
            );
        }

        [TestMethod]
        public void AsObservable_Hides()
        {
            var xs = Observable.Empty<int>();

            var res = xs.AsObservable();

            Assert.AreNotSame(xs, res);
        }

        [TestMethod]
        public void AsObservable_Never()
        {
            var scheduler = new TestScheduler();

            var xs = Observable.Never<int>();

            var res = scheduler.Start(() =>
                xs.AsObservable()
            );

            res.Messages.AssertEqual(
            );
        }

        [TestMethod]
        public void AsObservable_Empty()
        {
            var scheduler = new TestScheduler();

            var xs = scheduler.CreateHotObservable(
                OnNext(150, 1),
                OnCompleted<int>(250)
            );

            var res = scheduler.Start(() =>
                xs.AsObservable()
            );

            res.Messages.AssertEqual(
                OnCompleted<int>(250)
            );

            xs.Subscriptions.AssertEqual(
                Subscribe(200, 250)
            );
        }

        [TestMethod]
        public void AsObservable_Throw()
        {
            var scheduler = new TestScheduler();

            var ex = new Exception();
            var xs = scheduler.CreateHotObservable(
                OnNext(150, 1),
                OnError<int>(250, ex)
            );

            var res = scheduler.Start(() =>
                xs.AsObservable()
            );

            res.Messages.AssertEqual(
                OnError<int>(250, ex)
            );

            xs.Subscriptions.AssertEqual(
                Subscribe(200, 250)
            );
        }

        [TestMethod]
        public void AsObservable_Return()
        {
            var scheduler = new TestScheduler();

            var xs = scheduler.CreateHotObservable(
                OnNext(150, 1),
                OnNext(220, 2),
                OnCompleted<int>(250)
            );

            var res = scheduler.Start(() =>
                xs.AsObservable()
            );

            res.Messages.AssertEqual(
                OnNext(220, 2),
                OnCompleted<int>(250)
            );

            xs.Subscriptions.AssertEqual(
                Subscribe(200, 250)
            );
        }

        [TestMethod]
        public void AsObservable_IsNotEager()
        {
            var scheduler = new TestScheduler();

            bool subscribed = false;
            var xs = Observable.Create<int>(obs =>
            {
                subscribed = true;

                var disp = scheduler.CreateHotObservable(
                    OnNext(150, 1),
                    OnNext(220, 2),
                    OnCompleted<int>(250)
                ).Subscribe(obs);

                return disp.Dispose;
            });

            xs.AsObservable();
            Assert.IsFalse(subscribed);

            var res = scheduler.Start(() =>
                xs.AsObservable()
            );
            Assert.IsTrue(subscribed);
        }

        #endregion

        #region + Buffer +

        [TestMethod]
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

        [TestMethod]
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

        [TestMethod]
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

        [TestMethod]
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

        [TestMethod]
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

        [TestMethod]
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

        [TestMethod]
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

        [TestMethod]
        public void BufferWithCount_ArgumentChecking()
        {
            ReactiveAssert.Throws<ArgumentNullException>(() => Observable.Buffer(default(IObservable<int>), 1, 1));
            ReactiveAssert.Throws<ArgumentOutOfRangeException>(() => Observable.Buffer(DummyObservable<int>.Instance, 0, 1));
            ReactiveAssert.Throws<ArgumentOutOfRangeException>(() => Observable.Buffer(DummyObservable<int>.Instance, 1, 0));
            ReactiveAssert.Throws<ArgumentNullException>(() => Observable.Buffer(default(IObservable<int>), 1));
            ReactiveAssert.Throws<ArgumentOutOfRangeException>(() => Observable.Buffer(DummyObservable<int>.Instance, 0));
        }

        [TestMethod]
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

        [TestMethod]
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

        [TestMethod]
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

        [TestMethod]
        public void BufferWithCount_Default()
        {
            Observable.Range(1, 10).Buffer(3).Skip(1).First().AssertEqual(4, 5, 6);
            Observable.Range(1, 10).Buffer(3, 2).Skip(1).First().AssertEqual(3, 4, 5);
        }

        #endregion

        #region + Dematerialize +

        [TestMethod]
        public void Dematerialize_ArgumentChecking()
        {
            ReactiveAssert.Throws<ArgumentNullException>(() => Observable.Dematerialize<int>(null));
        }

        [TestMethod]
        public void Dematerialize_Range1()
        {
            var scheduler = new TestScheduler();

            var xs = scheduler.CreateHotObservable(
                OnNext(150, Notification.CreateOnNext(41)),
                OnNext(210, Notification.CreateOnNext(42)),
                OnNext(220, Notification.CreateOnNext(43)),
                OnCompleted<Notification<int>>(250)
            );
            var res = scheduler.Start(() =>
                xs.Dematerialize()
            );

            res.Messages.AssertEqual(
                OnNext(210, 42),
                OnNext(220, 43),
                OnCompleted<int>(250)
            );

            xs.Subscriptions.AssertEqual(
                Subscribe(200, 250)
            );
        }

        [TestMethod]
        public void Dematerialize_Range2()
        {
            var scheduler = new TestScheduler();

            var xs = scheduler.CreateHotObservable(
                OnNext(150, Notification.CreateOnNext(41)),
                OnNext(210, Notification.CreateOnNext(42)),
                OnNext(220, Notification.CreateOnNext(43)),
                OnNext(230, Notification.CreateOnCompleted<int>())
            );
            var res = scheduler.Start(() =>
                xs.Dematerialize()
            );

            res.Messages.AssertEqual(
                OnNext(210, 42),
                OnNext(220, 43),
                OnCompleted<int>(230)
            );

            xs.Subscriptions.AssertEqual(
                Subscribe(200, 230)
            );
        }

        [TestMethod]
        public void Dematerialize_Error1()
        {
            var scheduler = new TestScheduler();

            var ex = new Exception();

            var xs = scheduler.CreateHotObservable(
                OnNext(150, Notification.CreateOnNext(41)),
                OnNext(210, Notification.CreateOnNext(42)),
                OnNext(220, Notification.CreateOnNext(43)),
                OnError<Notification<int>>(230, ex)
            );
            var res = scheduler.Start(() =>
                xs.Dematerialize()
            );

            res.Messages.AssertEqual(
                OnNext(210, 42),
                OnNext(220, 43),
                OnError<int>(230, ex)
            );

            xs.Subscriptions.AssertEqual(
                Subscribe(200, 230)
            );
        }

        [TestMethod]
        public void Dematerialize_Error2()
        {
            var scheduler = new TestScheduler();

            var ex = new Exception();

            var xs = scheduler.CreateHotObservable(
                OnNext(150, Notification.CreateOnNext(41)),
                OnNext(210, Notification.CreateOnNext(42)),
                OnNext(220, Notification.CreateOnNext(43)),
                OnNext(230, Notification.CreateOnError<int>(ex))
            );
            var res = scheduler.Start(() =>
                xs.Dematerialize()
            );

            res.Messages.AssertEqual(
                OnNext(210, 42),
                OnNext(220, 43),
                OnError<int>(230, ex)
            );

            xs.Subscriptions.AssertEqual(
                Subscribe(200, 230)
            );
        }

        [TestMethod]
        public void Materialize_Dematerialize_Never()
        {
            var scheduler = new TestScheduler();

            var xs = Observable.Never<int>();

            var res = scheduler.Start(() =>
                xs.Materialize().Dematerialize()
            );

            res.Messages.AssertEqual(
            );
        }

        [TestMethod]
        public void Materialize_Dematerialize_Empty()
        {
            var scheduler = new TestScheduler();

            var xs = scheduler.CreateHotObservable(
                OnNext(150, 1),
                OnCompleted<int>(250)
            );

            var res = scheduler.Start(() =>
                xs.Materialize().Dematerialize()
            );

            res.Messages.AssertEqual(
                OnCompleted<int>(250)
            );

            xs.Subscriptions.AssertEqual(
                Subscribe(200, 250)
            );
        }

        [TestMethod]
        public void Materialize_Dematerialize_Return()
        {
            var scheduler = new TestScheduler();

            var xs = scheduler.CreateHotObservable(
                OnNext(150, 1),
                OnNext(210, 2),
                OnCompleted<int>(250)
            );

            var res = scheduler.Start(() =>
                xs.Materialize().Dematerialize()
            );

            res.Messages.AssertEqual(
                OnNext(210, 2),
                OnCompleted<int>(250)
            );

            xs.Subscriptions.AssertEqual(
                Subscribe(200, 250)
            );
        }

        [TestMethod]
        public void Materialize_Dematerialize_Throw()
        {
            var scheduler = new TestScheduler();

            var ex = new Exception();

            var xs = scheduler.CreateHotObservable(
                OnNext(150, 1),
                OnError<int>(250, ex)
            );

            var res = scheduler.Start(() =>
                xs.Materialize().Dematerialize()
            );

            res.Messages.AssertEqual(
                OnError<int>(250, ex)
            );

            xs.Subscriptions.AssertEqual(
                Subscribe(200, 250)
            );
        }

        #endregion

        #region + DistinctUntilChanged +

        [TestMethod]
        public void DistinctUntilChanged_ArgumentChecking()
        {
            var someObservable = Observable.Empty<int>();

            ReactiveAssert.Throws<ArgumentNullException>(() => Observable.DistinctUntilChanged<int>(null));
            ReactiveAssert.Throws<ArgumentNullException>(() => Observable.DistinctUntilChanged<int>(null, EqualityComparer<int>.Default));
            ReactiveAssert.Throws<ArgumentNullException>(() => Observable.DistinctUntilChanged<int>(someObservable, null));
            ReactiveAssert.Throws<ArgumentNullException>(() => Observable.DistinctUntilChanged<int, int>(null, _ => _));
            ReactiveAssert.Throws<ArgumentNullException>(() => Observable.DistinctUntilChanged<int, int>(someObservable, null));
            ReactiveAssert.Throws<ArgumentNullException>(() => Observable.DistinctUntilChanged<int, int>(someObservable, _ => _, null));
            ReactiveAssert.Throws<ArgumentNullException>(() => Observable.DistinctUntilChanged<int, int>(null, _ => _, EqualityComparer<int>.Default));
            ReactiveAssert.Throws<ArgumentNullException>(() => Observable.DistinctUntilChanged<int, int>(someObservable, null, EqualityComparer<int>.Default));
        }

        [TestMethod]
        public void DistinctUntilChanged_Never()
        {
            var scheduler = new TestScheduler();

            var xs = Observable.Never<int>();

            var res = scheduler.Start(() =>
                xs.DistinctUntilChanged()
            );

            res.Messages.AssertEqual(
            );
        }

        [TestMethod]
        public void DistinctUntilChanged_Empty()
        {
            var scheduler = new TestScheduler();

            var xs = scheduler.CreateHotObservable(
                OnNext(150, 1),
                OnCompleted<int>(250)
            );

            var res = scheduler.Start(() =>
                xs.DistinctUntilChanged()
            );

            res.Messages.AssertEqual(
                OnCompleted<int>(250)
            );

            xs.Subscriptions.AssertEqual(
                Subscribe(200, 250)
            );
        }

        [TestMethod]
        public void DistinctUntilChanged_Return()
        {
            var scheduler = new TestScheduler();

            var xs = scheduler.CreateHotObservable(
                OnNext(150, 1),
                OnNext(220, 2),
                OnCompleted<int>(250)
            );

            var res = scheduler.Start(() =>
                xs.DistinctUntilChanged()
            );

            res.Messages.AssertEqual(
                OnNext(220, 2),
                OnCompleted<int>(250)
            );

            xs.Subscriptions.AssertEqual(
                Subscribe(200, 250)
            );
        }

        [TestMethod]
        public void DistinctUntilChanged_Throw()
        {
            var scheduler = new TestScheduler();

            var ex = new Exception();
            var xs = scheduler.CreateHotObservable(
                OnNext(150, 1),
                OnError<int>(250, ex)
            );

            var res = scheduler.Start(() =>
                xs.DistinctUntilChanged()
            );

            res.Messages.AssertEqual(
                OnError<int>(250, ex)
            );

            xs.Subscriptions.AssertEqual(
                Subscribe(200, 250)
            );
        }

        [TestMethod]
        public void DistinctUntilChanged_AllChanges()
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
                xs.DistinctUntilChanged()
            );

            res.Messages.AssertEqual(
                OnNext(210, 2),
                OnNext(220, 3),
                OnNext(230, 4),
                OnNext(240, 5),
                OnCompleted<int>(250)
            );

            xs.Subscriptions.AssertEqual(
                Subscribe(200, 250)
            );
        }

        [TestMethod]
        public void DistinctUntilChanged_AllSame()
        {
            var scheduler = new TestScheduler();

            var xs = scheduler.CreateHotObservable(
                OnNext(150, 1),
                OnNext(210, 2),
                OnNext(220, 2),
                OnNext(230, 2),
                OnNext(240, 2),
                OnCompleted<int>(250)
            );

            var res = scheduler.Start(() =>
                xs.DistinctUntilChanged()
            );

            res.Messages.AssertEqual(
                OnNext(210, 2),
                OnCompleted<int>(250)
            );

            xs.Subscriptions.AssertEqual(
                Subscribe(200, 250)
            );
        }

        [TestMethod]
        public void DistinctUntilChanged_SomeChanges()
        {
            var scheduler = new TestScheduler();

            var xs = scheduler.CreateHotObservable(
                OnNext(150, 1),
                OnNext(210, 2), //*
                OnNext(215, 3), //*
                OnNext(220, 3),
                OnNext(225, 2), //*
                OnNext(230, 2),
                OnNext(230, 1), //*
                OnNext(240, 2), //*
                OnCompleted<int>(250)
            );

            var res = scheduler.Start(() =>
                xs.DistinctUntilChanged()
            );

            res.Messages.AssertEqual(
                OnNext(210, 2),
                OnNext(215, 3),
                OnNext(225, 2),
                OnNext(230, 1),
                OnNext(240, 2),
                OnCompleted<int>(250)
            );

            xs.Subscriptions.AssertEqual(
                Subscribe(200, 250)
            );
        }

        [TestMethod]
        public void DistinctUntilChanged_Comparer_AllEqual()
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
                xs.DistinctUntilChanged(new FuncComparer<int>((x, y) => true))
            );

            res.Messages.AssertEqual(
                OnNext(210, 2),
                OnCompleted<int>(250)
            );

            xs.Subscriptions.AssertEqual(
                Subscribe(200, 250)
            );
        }

        [TestMethod]
        public void DistinctUntilChanged_Comparer_AllDifferent()
        {
            var scheduler = new TestScheduler();

            var xs = scheduler.CreateHotObservable(
                OnNext(150, 1),
                OnNext(210, 2),
                OnNext(220, 2),
                OnNext(230, 2),
                OnNext(240, 2),
                OnCompleted<int>(250)
            );

            var res = scheduler.Start(() =>
                xs.DistinctUntilChanged(new FuncComparer<int>((x, y) => false))
            );

            res.Messages.AssertEqual(
                OnNext(210, 2),
                OnNext(220, 2),
                OnNext(230, 2),
                OnNext(240, 2),
                OnCompleted<int>(250)
            );

            xs.Subscriptions.AssertEqual(
                Subscribe(200, 250)
            );
        }

        [TestMethod]
        public void DistinctUntilChanged_KeySelector_Div2()
        {
            var scheduler = new TestScheduler();

            var xs = scheduler.CreateHotObservable(
                OnNext(150, 1),
                OnNext(210, 2), //*
                OnNext(220, 4),
                OnNext(230, 3), //*
                OnNext(240, 5),
                OnCompleted<int>(250)
            );

            var res = scheduler.Start(() =>
                xs.DistinctUntilChanged(x => x % 2)
            );

            res.Messages.AssertEqual(
                OnNext(210, 2),
                OnNext(230, 3),
                OnCompleted<int>(250)
            );

            xs.Subscriptions.AssertEqual(
                Subscribe(200, 250)
            );
        }

        class FuncComparer<T> : IEqualityComparer<T>
        {
            private Func<T, T, bool> _equals;

            public FuncComparer(Func<T, T, bool> equals)
            {
                _equals = equals;
            }

            public bool Equals(T x, T y)
            {
                return _equals(x, y);
            }

            public int GetHashCode(T obj)
            {
                return 0;
            }
        }

        [TestMethod]
        public void DistinctUntilChanged_KeySelectorThrows()
        {
            var ex = new Exception();

            var scheduler = new TestScheduler();

            var xs = scheduler.CreateHotObservable(
                OnNext(150, 1),
                OnNext(210, 2),
                OnCompleted<int>(250)
            );

            var res = scheduler.Start(() =>
                xs.DistinctUntilChanged(new Func<int, int>(x => { throw ex; }))
            );

            res.Messages.AssertEqual(
                OnError<int>(210, ex)
            );
        }

        [TestMethod]
        public void DistinctUntilChanged_ComparerThrows()
        {
            var ex = new Exception();

            var scheduler = new TestScheduler();

            var xs = scheduler.CreateHotObservable(
                OnNext(150, 1),
                OnNext(210, 2),
                OnNext(220, 3),
                OnCompleted<int>(250)
            );

            var res = scheduler.Start(() =>
                xs.DistinctUntilChanged(new ThrowComparer<int>(ex))
            );

            res.Messages.AssertEqual(
                OnNext(210, 2),
                OnError<int>(220, ex)
            );
        }

        class ThrowComparer<T> : IEqualityComparer<T>
        {
            private Exception _ex;

            public ThrowComparer(Exception ex)
            {
                _ex = ex;
            }

            public bool Equals(T x, T y)
            {
                throw _ex;
            }

            public int GetHashCode(T obj)
            {
                return 0;
            }
        }

        [TestMethod]
        public void DistinctUntilChanged_KeySelector_Comparer()
        {
            var scheduler = new TestScheduler();

            var xs = scheduler.CreateHotObservable(
                OnNext(150, 1),
                OnNext(210, 2), // * key = 1    % 3 = 1
                OnNext(220, 8), //   key = 4    % 3 = 1   same
                OnNext(230, 2), //   key = 1    % 3 = 1   same
                OnNext(240, 5), // * key = 2    % 3 = 2
                OnCompleted<int>(250)
            );

            var res = scheduler.Start(() =>
                xs.DistinctUntilChanged(x => x / 2, new FuncComparer<int>((x, y) => x % 3 == y % 3))
            );

            res.Messages.AssertEqual(
                OnNext(210, 2),
                OnNext(240, 5),
                OnCompleted<int>(250)
            );

            xs.Subscriptions.AssertEqual(
                Subscribe(200, 250)
            );
        }

        #endregion

        #region + Do +

        [TestMethod]
        public void Do_ArgumentChecking()
        {
            var someObservable = Observable.Empty<int>();

            ReactiveAssert.Throws<ArgumentNullException>(() => Observable.Do<int>(someObservable, (Action<int>)null));
            ReactiveAssert.Throws<ArgumentNullException>(() => Observable.Do<int>(null, _ => { }));
            ReactiveAssert.Throws<ArgumentNullException>(() => Observable.Do<int>(someObservable, x => { }, (Action)null));
            ReactiveAssert.Throws<ArgumentNullException>(() => Observable.Do<int>(someObservable, (Action<int>)null, () => { }));
            ReactiveAssert.Throws<ArgumentNullException>(() => Observable.Do<int>(null, x => { }, () => { }));
            ReactiveAssert.Throws<ArgumentNullException>(() => Observable.Do<int>(someObservable, x => { }, (Action<Exception>)null));
            ReactiveAssert.Throws<ArgumentNullException>(() => Observable.Do<int>(someObservable, (Action<int>)null, (Exception _) => { }));
            ReactiveAssert.Throws<ArgumentNullException>(() => Observable.Do<int>(null, x => { }, (Exception _) => { }));
            ReactiveAssert.Throws<ArgumentNullException>(() => Observable.Do<int>(someObservable, x => { }, (Exception _) => { }, null));
            ReactiveAssert.Throws<ArgumentNullException>(() => Observable.Do<int>(someObservable, x => { }, (Action<Exception>)null, () => { }));
            ReactiveAssert.Throws<ArgumentNullException>(() => Observable.Do<int>(someObservable, (Action<int>)null, (Exception _) => { }, () => { }));
            ReactiveAssert.Throws<ArgumentNullException>(() => Observable.Do<int>(null, x => { }, (Exception _) => { }, () => { }));
            ReactiveAssert.Throws<ArgumentNullException>(() => Observable.Do<int>(null, Observer.Create<int>(i => { })));
            ReactiveAssert.Throws<ArgumentNullException>(() => Observable.Do<int>(someObservable, default(IObserver<int>)));
        }

        [TestMethod]
        public void Do_ShouldSeeAllValues()
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

            int i = 0;
            int sum = 2 + 3 + 4 + 5;
            var res = scheduler.Start(() =>
                xs.Do(x => { i++; sum -= x; })
            );

            Assert.AreEqual(4, i);
            Assert.AreEqual(0, sum);

            res.Messages.AssertEqual(
                OnNext(210, 2),
                OnNext(220, 3),
                OnNext(230, 4),
                OnNext(240, 5),
                OnCompleted<int>(250)
            );

            xs.Subscriptions.AssertEqual(
                Subscribe(200, 250)
            );
        }

        [TestMethod]
        public void Do_PlainAction()
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

            int i = 0;
            var res = scheduler.Start(() =>
                xs.Do(_ => { i++; })
            );

            Assert.AreEqual(4, i);

            res.Messages.AssertEqual(
                OnNext(210, 2),
                OnNext(220, 3),
                OnNext(230, 4),
                OnNext(240, 5),
                OnCompleted<int>(250)
            );

            xs.Subscriptions.AssertEqual(
                Subscribe(200, 250)
            );
        }

        [TestMethod]
        public void Do_NextCompleted()
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

            int i = 0;
            int sum = 2 + 3 + 4 + 5;
            bool completed = false;
            var res = scheduler.Start(() =>
                xs.Do(x => { i++; sum -= x; }, () => { completed = true; })
            );

            Assert.AreEqual(4, i);
            Assert.AreEqual(0, sum);
            Assert.IsTrue(completed);

            res.Messages.AssertEqual(
                OnNext(210, 2),
                OnNext(220, 3),
                OnNext(230, 4),
                OnNext(240, 5),
                OnCompleted<int>(250)
            );

            xs.Subscriptions.AssertEqual(
                Subscribe(200, 250)
            );
        }

        [TestMethod]
        public void Do_NextCompleted_Never()
        {
            var scheduler = new TestScheduler();

            var xs = scheduler.CreateHotObservable<int>();

            int i = 0;
            bool completed = false;
            var res = scheduler.Start(() =>
                xs.Do(x => { i++; }, () => { completed = true; })
            );

            Assert.AreEqual(0, i);
            Assert.IsFalse(completed);

            res.Messages.AssertEqual(
            );

            xs.Subscriptions.AssertEqual(
                Subscribe(200, 1000)
            );
        }

        [TestMethod]
        public void Do_NextError()
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

            int i = 0;
            int sum = 2 + 3 + 4 + 5;
            bool sawError = false;
            var res = scheduler.Start(() =>
                xs.Do(x => { i++; sum -= x; }, e => { sawError = e == ex; })
            );

            Assert.AreEqual(4, i);
            Assert.AreEqual(0, sum);
            Assert.IsTrue(sawError);

            res.Messages.AssertEqual(
                OnNext(210, 2),
                OnNext(220, 3),
                OnNext(230, 4),
                OnNext(240, 5),
                OnError<int>(250, ex)
            );

            xs.Subscriptions.AssertEqual(
                Subscribe(200, 250)
            );
        }

        [TestMethod]
        public void Do_NextErrorNot()
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

            int i = 0;
            int sum = 2 + 3 + 4 + 5;
            bool sawError = false;
            var res = scheduler.Start(() =>
                xs.Do(x => { i++; sum -= x; }, _ => { sawError = true; })
            );

            Assert.AreEqual(4, i);
            Assert.AreEqual(0, sum);
            Assert.IsFalse(sawError);

            res.Messages.AssertEqual(
                OnNext(210, 2),
                OnNext(220, 3),
                OnNext(230, 4),
                OnNext(240, 5),
                OnCompleted<int>(250)
            );

            xs.Subscriptions.AssertEqual(
                Subscribe(200, 250)
            );
        }

        [TestMethod]
        public void Do_NextErrorCompleted()
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

            int i = 0;
            int sum = 2 + 3 + 4 + 5;
            bool sawError = false;
            bool hasCompleted = false;
            var res = scheduler.Start(() =>
                xs.Do(x => { i++; sum -= x; }, e => { sawError = true; }, () => { hasCompleted = true; })
            );

            Assert.AreEqual(4, i);
            Assert.AreEqual(0, sum);
            Assert.IsFalse(sawError);
            Assert.IsTrue(hasCompleted);

            res.Messages.AssertEqual(
                OnNext(210, 2),
                OnNext(220, 3),
                OnNext(230, 4),
                OnNext(240, 5),
                OnCompleted<int>(250)
            );

            xs.Subscriptions.AssertEqual(
                Subscribe(200, 250)
            );
        }

        [TestMethod]
        public void Do_NextErrorCompletedError()
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

            int i = 0;
            int sum = 2 + 3 + 4 + 5;
            bool sawError = false;
            bool hasCompleted = false;
            var res = scheduler.Start(() =>
                xs.Do(x => { i++; sum -= x; }, e => { sawError = e == ex; }, () => { hasCompleted = true; })
            );

            Assert.AreEqual(4, i);
            Assert.AreEqual(0, sum);
            Assert.IsTrue(sawError);
            Assert.IsFalse(hasCompleted);

            res.Messages.AssertEqual(
                OnNext(210, 2),
                OnNext(220, 3),
                OnNext(230, 4),
                OnNext(240, 5),
                OnError<int>(250, ex)
            );

            xs.Subscriptions.AssertEqual(
                Subscribe(200, 250)
            );
        }

        [TestMethod]
        public void Do_NextErrorCompletedNever()
        {
            var scheduler = new TestScheduler();

            var xs = scheduler.CreateHotObservable<int>();

            int i = 0;
            bool sawError = false;
            bool hasCompleted = false;
            var res = scheduler.Start(() =>
                xs.Do(x => { i++; }, e => { sawError = true; }, () => { hasCompleted = true; })
            );

            Assert.AreEqual(0, i);
            Assert.IsFalse(sawError);
            Assert.IsFalse(hasCompleted);

            res.Messages.AssertEqual(
            );

            xs.Subscriptions.AssertEqual(
                Subscribe(200, 1000)
            );
        }

        [TestMethod]
        public void Do_Observer_SomeDataWithError()
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

            int i = 0;
            int sum = 2 + 3 + 4 + 5;
            bool sawError = false;
            bool hasCompleted = false;
            var res = scheduler.Start(() =>
                xs.Do(Observer.Create<int>(x => { i++; sum -= x; }, e => { sawError = e == ex; }, () => { hasCompleted = true; }))
            );

            Assert.AreEqual(4, i);
            Assert.AreEqual(0, sum);
            Assert.IsTrue(sawError);
            Assert.IsFalse(hasCompleted);

            res.Messages.AssertEqual(
                OnNext(210, 2),
                OnNext(220, 3),
                OnNext(230, 4),
                OnNext(240, 5),
                OnError<int>(250, ex)
            );

            xs.Subscriptions.AssertEqual(
                Subscribe(200, 250)
            );
        }

        [TestMethod]
        public void Do_Observer_SomeDataWithoutError()
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

            int i = 0;
            int sum = 2 + 3 + 4 + 5;
            bool sawError = false;
            bool hasCompleted = false;
            var res = scheduler.Start(() =>
                xs.Do(Observer.Create<int>(x => { i++; sum -= x; }, e => { sawError = true; }, () => { hasCompleted = true; }))
            );

            Assert.AreEqual(4, i);
            Assert.AreEqual(0, sum);
            Assert.IsFalse(sawError);
            Assert.IsTrue(hasCompleted);

            res.Messages.AssertEqual(
                OnNext(210, 2),
                OnNext(220, 3),
                OnNext(230, 4),
                OnNext(240, 5),
                OnCompleted<int>(250)
            );

            xs.Subscriptions.AssertEqual(
                Subscribe(200, 250)
            );
        }

        [TestMethod]
        public void Do1422_Next_NextThrows()
        {
            var scheduler = new TestScheduler();

            var ex = new Exception();

            var xs = scheduler.CreateHotObservable(
                OnNext(150, 1),
                OnNext(210, 2),
                OnCompleted<int>(250)
            );

            var res = scheduler.Start(() =>
                xs.Do(x => { throw ex; })
            );

            res.Messages.AssertEqual(
                OnError<int>(210, ex)
            );

            xs.Subscriptions.AssertEqual(
                Subscribe(200, 210)
            );
        }

        [TestMethod]
        public void Do1422_NextCompleted_NextThrows()
        {
            var scheduler = new TestScheduler();

            var ex = new Exception();

            var xs = scheduler.CreateHotObservable(
                OnNext(150, 1),
                OnNext(210, 2),
                OnCompleted<int>(250)
            );

            var res = scheduler.Start(() =>
                xs.Do(x => { throw ex; }, () => { })
            );

            res.Messages.AssertEqual(
                OnError<int>(210, ex)
            );

            xs.Subscriptions.AssertEqual(
                Subscribe(200, 210)
            );
        }

        [TestMethod]
        public void Do1422_NextCompleted_CompletedThrows()
        {
            var scheduler = new TestScheduler();

            var ex = new Exception();

            var xs = scheduler.CreateHotObservable(
                OnNext(150, 1),
                OnNext(210, 2),
                OnCompleted<int>(250)
            );

            var res = scheduler.Start(() =>
                xs.Do(x => { }, () => { throw ex; })
            );

            res.Messages.AssertEqual(
                OnNext(210, 2),
                OnError<int>(250, ex)
            );

            xs.Subscriptions.AssertEqual(
                Subscribe(200, 250)
            );
        }

        [TestMethod]
        public void Do1422_NextError_NextThrows()
        {
            var scheduler = new TestScheduler();

            var ex = new Exception();

            var xs = scheduler.CreateHotObservable(
                OnNext(150, 1),
                OnNext(210, 2),
                OnCompleted<int>(250)
            );

            var res = scheduler.Start(() =>
                xs.Do(x => { throw ex; }, _ => { })
            );

            res.Messages.AssertEqual(
                OnError<int>(210, ex)
            );

            xs.Subscriptions.AssertEqual(
                Subscribe(200, 210)
            );
        }

        [TestMethod]
        public void Do1422_NextError_ErrorThrows()
        {
            var scheduler = new TestScheduler();

            var ex1 = new Exception();
            var ex2 = new Exception();

            var xs = scheduler.CreateHotObservable(
                OnNext(150, 1),
                OnError<int>(210, ex1)
            );

            var res = scheduler.Start(() =>
                xs.Do(x => { }, _ => { throw ex2; })
            );

            res.Messages.AssertEqual(
                OnError<int>(210, ex2)
            );

            xs.Subscriptions.AssertEqual(
                Subscribe(200, 210)
            );
        }

        [TestMethod]
        public void Do1422_NextErrorCompleted_NextThrows()
        {
            var scheduler = new TestScheduler();

            var ex = new Exception();

            var xs = scheduler.CreateHotObservable(
                OnNext(150, 1),
                OnNext(210, 2),
                OnCompleted<int>(250)
            );

            var res = scheduler.Start(() =>
                xs.Do(x => { throw ex; }, _ => { }, () => { })
            );

            res.Messages.AssertEqual(
                OnError<int>(210, ex)
            );

            xs.Subscriptions.AssertEqual(
                Subscribe(200, 210)
            );
        }

        [TestMethod]
        public void Do1422_NextErrorCompleted_ErrorThrows()
        {
            var scheduler = new TestScheduler();

            var ex1 = new Exception();
            var ex2 = new Exception();

            var xs = scheduler.CreateHotObservable(
                OnNext(150, 1),
                OnError<int>(210, ex1)
            );

            var res = scheduler.Start(() =>
                xs.Do(x => { }, _ => { throw ex2; }, () => { })
            );

            res.Messages.AssertEqual(
                OnError<int>(210, ex2)
            );

            xs.Subscriptions.AssertEqual(
                Subscribe(200, 210)
            );
        }

        [TestMethod]
        public void Do1422_NextErrorCompleted_CompletedThrows()
        {
            var scheduler = new TestScheduler();

            var ex = new Exception();

            var xs = scheduler.CreateHotObservable(
                OnNext(150, 1),
                OnNext(210, 2),
                OnCompleted<int>(250)
            );

            var res = scheduler.Start(() =>
                xs.Do(x => { }, _ => { }, () => { throw ex; })
            );

            res.Messages.AssertEqual(
                OnNext(210, 2),
                OnError<int>(250, ex)
            );

            xs.Subscriptions.AssertEqual(
                Subscribe(200, 250)
            );
        }

        [TestMethod]
        public void Do1422_Observer_NextThrows()
        {
            var scheduler = new TestScheduler();

            var ex = new Exception();

            var xs = scheduler.CreateHotObservable(
                OnNext(150, 1),
                OnNext(210, 2),
                OnCompleted<int>(250)
            );

            var res = scheduler.Start(() =>
                xs.Do(Observer.Create<int>(x => { throw ex; }, _ => { }, () => { }))
            );

            res.Messages.AssertEqual(
                OnError<int>(210, ex)
            );

            xs.Subscriptions.AssertEqual(
                Subscribe(200, 210)
            );
        }

        [TestMethod]
        public void Do1422_Observer_ErrorThrows()
        {
            var scheduler = new TestScheduler();

            var ex1 = new Exception();
            var ex2 = new Exception();

            var xs = scheduler.CreateHotObservable(
                OnNext(150, 1),
                OnError<int>(210, ex1)
            );

            var res = scheduler.Start(() =>
                xs.Do(Observer.Create<int>(x => { }, _ => { throw ex2; }, () => { }))
            );

            res.Messages.AssertEqual(
                OnError<int>(210, ex2)
            );

            xs.Subscriptions.AssertEqual(
                Subscribe(200, 210)
            );
        }

        [TestMethod]
        public void Do1422_Observer_CompletedThrows()
        {
            var scheduler = new TestScheduler();

            var ex = new Exception();

            var xs = scheduler.CreateHotObservable(
                OnNext(150, 1),
                OnNext(210, 2),
                OnCompleted<int>(250)
            );

            var res = scheduler.Start(() =>
                xs.Do(Observer.Create<int>(x => { }, _ => { }, () => { throw ex; }))
            );

            res.Messages.AssertEqual(
                OnNext(210, 2),
                OnError<int>(250, ex)
            );

            xs.Subscriptions.AssertEqual(
                Subscribe(200, 250)
            );
        }

        #endregion

        #region + Finally +

        [TestMethod]
        public void Finally_ArgumentChecking()
        {
            var someObservable = Observable.Empty<int>();

            ReactiveAssert.Throws<ArgumentNullException>(() => Observable.Finally<int>(null, () => { }));
            ReactiveAssert.Throws<ArgumentNullException>(() => Observable.Finally<int>(someObservable, null));
        }

        [TestMethod]
        public void Finally_Never()
        {
            var scheduler = new TestScheduler();

            bool invoked = false;
            var res = scheduler.Start(() =>
                Observable.Never<int>().Finally(() => { invoked = true; })
            );

            res.Messages.AssertEqual(
            );

            Assert.IsTrue(invoked); // due to unsubscribe; see 1356
        }

        [TestMethod]
        public void Finally_OnlyCalledOnce_Never()
        {
            int invokeCount = 0;
            var someObservable = Observable.Never<int>().Finally(() => { invokeCount++; });
            var d = someObservable.Subscribe();
            d.Dispose();
            d.Dispose();

            Assert.AreEqual(1, invokeCount);
        }

        [TestMethod]
        public void Finally_OnlyCalledOnce_Empty()
        {
            var invokeCount = 0;
            var someObservable = Observable.Empty<int>().Finally(() => { invokeCount++; });
            var d = someObservable.Subscribe();
            d.Dispose();
            d.Dispose();

            Assert.AreEqual(1, invokeCount);
        }

        [TestMethod]
        public void Finally_Empty()
        {
            var scheduler = new TestScheduler();

            var xs = scheduler.CreateHotObservable(
                OnNext(150, 1),
                OnCompleted<int>(250)
            );

            var invoked = false;
            var res = scheduler.Start(() =>
                xs.Finally(() => { invoked = true; })
            );

            Assert.IsTrue(invoked);

            res.Messages.AssertEqual(
                OnCompleted<int>(250)
            );

            xs.Subscriptions.AssertEqual(
                Subscribe(200, 250)
            );
        }

        [TestMethod]
        public void Finally_Return()
        {
            var scheduler = new TestScheduler();

            var xs = scheduler.CreateHotObservable(
                OnNext(150, 1),
                OnNext(210, 2),
                OnCompleted<int>(250)
            );

            var invoked = false;
            var res = scheduler.Start(() =>
                xs.Finally(() => { invoked = true; })
            );

            Assert.IsTrue(invoked);

            res.Messages.AssertEqual(
                OnNext(210, 2),
                OnCompleted<int>(250)
            );

            xs.Subscriptions.AssertEqual(
                Subscribe(200, 250)
            );
        }

        [TestMethod]
        public void Finally_Throw()
        {
            var scheduler = new TestScheduler();

            var ex = new Exception();
            var xs = scheduler.CreateHotObservable(
                OnNext(150, 1),
                OnError<int>(250, ex)
            );

            var invoked = false;
            var res = scheduler.Start(() =>
                xs.Finally(() => { invoked = true; })
            );

            Assert.IsTrue(invoked);

            res.Messages.AssertEqual(
                OnError<int>(250, ex)
            );

            xs.Subscriptions.AssertEqual(
                Subscribe(200, 250)
            );
        }

        #endregion

        #region + IgnoreElements +

        [TestMethod]
        public void IgnoreElements_ArgumentChecking()
        {
            ReactiveAssert.Throws<ArgumentNullException>(() => Observable.IgnoreElements<int>(null));
        }

        [TestMethod]
        public void IgnoreElements_IgnoreElements()
        {
            var scheduler = new TestScheduler();

            var xs = scheduler.CreateHotObservable(
                OnNext(180, 1),
                OnNext(210, 2),
                OnNext(250, 3),
                OnNext(270, 4),
                OnCompleted<int>(300)
            );

            var res = scheduler.Start(() =>
                xs.IgnoreElements().IgnoreElements()
            );

            res.Messages.AssertEqual(
                OnCompleted<int>(300)
            );

            xs.Subscriptions.AssertEqual(
                Subscribe(200, 300)
            );
        }

        [TestMethod]
        public void IgnoreElements_Basic()
        {
            var scheduler = new TestScheduler();

            var xs = scheduler.CreateHotObservable(
                OnNext(180, 1),
                OnNext(210, 2),
                OnNext(250, 3),
                OnNext(270, 4),
                OnNext(310, 5),
                OnNext(360, 6),
                OnNext(380, 7),
                OnNext(410, 8),
                OnNext(590, 9)
            );

            var res = scheduler.Start(() =>
                xs.IgnoreElements()
            );

            res.Messages.AssertEqual(
            );

            xs.Subscriptions.AssertEqual(
                Subscribe(200, 1000)
            );
        }

        [TestMethod]
        public void IgnoreElements_Completed()
        {
            var scheduler = new TestScheduler();

            var xs = scheduler.CreateHotObservable(
                OnNext(180, 1),
                OnNext(210, 2),
                OnNext(250, 3),
                OnNext(270, 4),
                OnNext(310, 5),
                OnNext(360, 6),
                OnNext(380, 7),
                OnNext(410, 8),
                OnNext(590, 9),
                OnCompleted<int>(610)
            );

            var res = scheduler.Start(() =>
                xs.IgnoreElements()
            );

            res.Messages.AssertEqual(
                OnCompleted<int>(610)
            );

            xs.Subscriptions.AssertEqual(
                Subscribe(200, 610)
            );
        }

        [TestMethod]
        public void IgnoreElements_Error()
        {
            var scheduler = new TestScheduler();

            var ex = new Exception();

            var xs = scheduler.CreateHotObservable(
                OnNext(180, 1),
                OnNext(210, 2),
                OnNext(250, 3),
                OnNext(270, 4),
                OnNext(310, 5),
                OnNext(360, 6),
                OnNext(380, 7),
                OnNext(410, 8),
                OnNext(590, 9),
                OnError<int>(610, ex)
            );

            var res = scheduler.Start(() =>
                xs.IgnoreElements()
            );

            res.Messages.AssertEqual(
                OnError<int>(610, ex)
            );

            xs.Subscriptions.AssertEqual(
                Subscribe(200, 610)
            );
        }

        #endregion

        #region + Materialize +

        [TestMethod]
        public void Materialize_ArgumentChecking()
        {
            ReactiveAssert.Throws<ArgumentNullException>(() => Observable.Materialize<int>(null));
        }

        [TestMethod]
        public void Materialize_Never()
        {
            var scheduler = new TestScheduler();
            var res = scheduler.Start(() =>
                Observable.Never<int>().Materialize()
            );

            res.Messages.AssertEqual(
            );
        }

        [TestMethod]
        public void Materialize_Empty()
        {
            var scheduler = new TestScheduler();

            var xs = scheduler.CreateHotObservable(
                OnNext(150, 1),
                OnCompleted<int>(250)
            );

            var res = scheduler.Start(() =>
                xs.Materialize()
            );

            res.Messages.AssertEqual(
                OnNext(250, Notification.CreateOnCompleted<int>()),
                OnCompleted<Notification<int>>(250)
            );

            xs.Subscriptions.AssertEqual(
                Subscribe(200, 250)
            );
        }

        [TestMethod]
        public void Materialize_Return()
        {
            var scheduler = new TestScheduler();

            var xs = scheduler.CreateHotObservable(
                OnNext(150, 1),
                OnNext(210, 2),
                OnCompleted<int>(250)
            );

            var res = scheduler.Start(() =>
                xs.Materialize()
            );

            res.Messages.AssertEqual(
                OnNext(210, Notification.CreateOnNext(2)),
                OnNext(250, Notification.CreateOnCompleted<int>()),
                OnCompleted<Notification<int>>(250)
            );

            xs.Subscriptions.AssertEqual(
                Subscribe(200, 250)
            );
        }

        [TestMethod]
        public void Materialize_Throw()
        {
            var scheduler = new TestScheduler();

            var ex = new Exception();

            var xs = scheduler.CreateHotObservable(
                OnNext(150, 1),
                OnError<int>(250, ex)
            );

            var res = scheduler.Start(() =>
                xs.Materialize()
            );

            res.Messages.AssertEqual(
                OnNext(250, Notification.CreateOnError<int>(ex)),
                OnCompleted<Notification<int>>(250)
            );

            xs.Subscriptions.AssertEqual(
                Subscribe(200, 250)
            );
        }

        #endregion

        #region - Repeat -

        [TestMethod]
        public void Repeat_Observable_ArgumentChecking()
        {
            ReactiveAssert.Throws<ArgumentNullException>(() => Observable.Repeat<int>(null));
            ReactiveAssert.Throws<ArgumentNullException>(() => DummyObservable<int>.Instance.Repeat().Subscribe(null));
        }

        [TestMethod]
        public void Repeat_Observable_Basic()
        {
            var scheduler = new TestScheduler();

            var xs = scheduler.CreateColdObservable(
                OnNext(100, 1),
                OnNext(150, 2),
                OnNext(200, 3),
                OnCompleted<int>(250)
            );

            var res = scheduler.Start(() =>
                xs.Repeat()
            );

            res.Messages.AssertEqual(
                OnNext(300, 1),
                OnNext(350, 2),
                OnNext(400, 3),
                OnNext(550, 1),
                OnNext(600, 2),
                OnNext(650, 3),
                OnNext(800, 1),
                OnNext(850, 2),
                OnNext(900, 3)
            );

            xs.Subscriptions.AssertEqual(
                Subscribe(200, 450),
                Subscribe(450, 700),
                Subscribe(700, 950),
                Subscribe(950, 1000)
            );
        }

        [TestMethod]
        public void Repeat_Observable_Infinite()
        {
            var scheduler = new TestScheduler();

            var xs = scheduler.CreateColdObservable(
                OnNext(100, 1),
                OnNext(150, 2),
                OnNext(200, 3)
            );

            var res = scheduler.Start(() =>
                xs.Repeat()
            );

            res.Messages.AssertEqual(
                OnNext(300, 1),
                OnNext(350, 2),
                OnNext(400, 3)
            );

            xs.Subscriptions.AssertEqual(
                Subscribe(200, 1000)
            );
        }

        [TestMethod]
        public void Repeat_Observable_Error()
        {
            var scheduler = new TestScheduler();

            var ex = new Exception();

            var xs = scheduler.CreateColdObservable(
                OnNext(100, 1),
                OnNext(150, 2),
                OnNext(200, 3),
                OnError<int>(250, ex)
            );

            var res = scheduler.Start(() =>
                xs.Repeat()
            );

            res.Messages.AssertEqual(
                OnNext(300, 1),
                OnNext(350, 2),
                OnNext(400, 3),
                OnError<int>(450, ex)
            );

            xs.Subscriptions.AssertEqual(
                Subscribe(200, 450)
            );
        }

        [TestMethod]
        public void Repeat_Observable_Throws()
        {
            var scheduler1 = new TestScheduler();

            var xs = Observable.Return(1, scheduler1).Repeat();

            xs.Subscribe(x => { throw new InvalidOperationException(); });

            ReactiveAssert.Throws<InvalidOperationException>(() => scheduler1.Start());

            var scheduler2 = new TestScheduler();

            var ys = Observable.Throw<int>(new Exception(), scheduler2).Repeat();

            ys.Subscribe(x => { }, ex => { throw new InvalidOperationException(); });

            ReactiveAssert.Throws<InvalidOperationException>(() => scheduler2.Start());

            var scheduler3 = new TestScheduler();

            var zs = Observable.Return(1, scheduler3).Repeat();

            var d = zs.Subscribe(x => { }, ex => { }, () => { throw new InvalidOperationException(); });

            scheduler3.ScheduleAbsolute(210, () => d.Dispose());

            scheduler3.Start();

            var xss = Observable.Create<int>(new Func<IObserver<int>, Action>(o => { throw new InvalidOperationException(); })).Repeat();

            ReactiveAssert.Throws<InvalidOperationException>(() => xss.Subscribe());
        }

        [TestMethod]
        public void Repeat_Observable_Default_ArgumentChecking()
        {
            ReactiveAssert.Throws<ArgumentNullException>(() => Observable.Repeat<int>((IObservable<int>)null));
            ReactiveAssert.Throws<ArgumentNullException>(() => DummyObservable<int>.Instance.Repeat().Subscribe(null));
        }

        [TestMethod]
        public void Repeat_Observable_RepeatCount_ArgumentChecking()
        {
            ReactiveAssert.Throws<ArgumentNullException>(() => Observable.Repeat<int>(null, 0));
            ReactiveAssert.Throws<ArgumentOutOfRangeException>(() => DummyObservable<int>.Instance.Repeat(-1));
            ReactiveAssert.Throws<ArgumentNullException>(() => DummyObservable<int>.Instance.Repeat(0).Subscribe(null));
        }

        [TestMethod]
        public void Repeat_Observable_RepeatCount_Basic()
        {
            var scheduler = new TestScheduler();

            var xs = scheduler.CreateColdObservable(
                OnNext(5, 1),
                OnNext(10, 2),
                OnNext(15, 3),
                OnCompleted<int>(20)
            );

            var res = scheduler.Start(() =>
                xs.Repeat(3)
            );

            res.Messages.AssertEqual(
                OnNext(205, 1),
                OnNext(210, 2),
                OnNext(215, 3),
                OnNext(225, 1),
                OnNext(230, 2),
                OnNext(235, 3),
                OnNext(245, 1),
                OnNext(250, 2),
                OnNext(255, 3),
                OnCompleted<int>(260)
            );

            xs.Subscriptions.AssertEqual(
                Subscribe(200, 220),
                Subscribe(220, 240),
                Subscribe(240, 260)
            );
        }

        [TestMethod]
        public void Repeat_Observable_RepeatCount_Dispose()
        {
            var scheduler = new TestScheduler();

            var xs = scheduler.CreateColdObservable(
                OnNext(5, 1),
                OnNext(10, 2),
                OnNext(15, 3),
                OnCompleted<int>(20)
            );

            var res = scheduler.Start(() =>
                xs.Repeat(3), 231
            );

            res.Messages.AssertEqual(
                OnNext(205, 1),
                OnNext(210, 2),
                OnNext(215, 3),
                OnNext(225, 1),
                OnNext(230, 2)
            );

            xs.Subscriptions.AssertEqual(
                Subscribe(200, 220),
                Subscribe(220, 231)
            );
        }

        [TestMethod]
        public void Repeat_Observable_RepeatCount_Infinite()
        {
            var scheduler = new TestScheduler();

            var xs = scheduler.CreateColdObservable(
                OnNext(100, 1),
                OnNext(150, 2),
                OnNext(200, 3)
            );

            var res = scheduler.Start(() =>
                xs.Repeat(3)
            );

            res.Messages.AssertEqual(
                OnNext(300, 1),
                OnNext(350, 2),
                OnNext(400, 3)
            );

            xs.Subscriptions.AssertEqual(
                Subscribe(200, 1000)
            );
        }

        [TestMethod]
        public void Repeat_Observable_RepeatCount_Error()
        {
            var scheduler = new TestScheduler();

            var ex = new Exception();

            var xs = scheduler.CreateColdObservable(
                OnNext(100, 1),
                OnNext(150, 2),
                OnNext(200, 3),
                OnError<int>(250, ex)
            );

            var res = scheduler.Start(() =>
                xs.Repeat(3)
            );

            res.Messages.AssertEqual(
                OnNext(300, 1),
                OnNext(350, 2),
                OnNext(400, 3),
                OnError<int>(450, ex)
            );

            xs.Subscriptions.AssertEqual(
                Subscribe(200, 450)
            );
        }

        [TestMethod]
        public void Repeat_Observable_RepeatCount_Throws()
        {
            var scheduler1 = new TestScheduler();

            var xs = Observable.Return(1, scheduler1).Repeat(3);

            xs.Subscribe(x => { throw new InvalidOperationException(); });

            ReactiveAssert.Throws<InvalidOperationException>(() => scheduler1.Start());

            var scheduler2 = new TestScheduler();

            var ys = Observable.Throw<int>(new Exception(), scheduler2).Repeat(3);

            ys.Subscribe(x => { }, ex => { throw new InvalidOperationException(); });

            ReactiveAssert.Throws<InvalidOperationException>(() => scheduler2.Start());

            var scheduler3 = new TestScheduler();

            var zs = Observable.Return(1, scheduler3).Repeat(100);

            var d = zs.Subscribe(x => { }, ex => { }, () => { throw new InvalidOperationException(); });

            scheduler3.ScheduleAbsolute(10, () => d.Dispose());

            scheduler3.Start();

            var xss = Observable.Create<int>(new Func<IObserver<int>, Action>(o => { throw new InvalidOperationException(); })).Repeat(3);

            ReactiveAssert.Throws<InvalidOperationException>(() => xss.Subscribe());
        }

        [TestMethod]
        public void Repeat_Observable_RepeatCount_Default_ArgumentChecking()
        {
            ReactiveAssert.Throws<ArgumentNullException>(() => Observable.Repeat<int>(default(IObservable<int>), 0));
            ReactiveAssert.Throws<ArgumentOutOfRangeException>(() => DummyObservable<int>.Instance.Repeat(-1));
            ReactiveAssert.Throws<ArgumentNullException>(() => DummyObservable<int>.Instance.Repeat(0).Subscribe(null));
        }

        #endregion

        #region - Retry -

        [TestMethod]
        public void Retry_Observable_ArgumentChecking()
        {
            ReactiveAssert.Throws<ArgumentNullException>(() => Observable.Retry<int>(null));
            ReactiveAssert.Throws<ArgumentNullException>(() => DummyObservable<int>.Instance.Retry().Subscribe(null));
        }

        [TestMethod]
        public void Retry_Observable_Basic()
        {
            var scheduler = new TestScheduler();

            var xs = scheduler.CreateColdObservable(
                OnNext(100, 1),
                OnNext(150, 2),
                OnNext(200, 3),
                OnCompleted<int>(250)
            );

            var res = scheduler.Start(() =>
                xs.Retry()
            );

            res.Messages.AssertEqual(
                OnNext(300, 1),
                OnNext(350, 2),
                OnNext(400, 3),
                OnCompleted<int>(450)
            );

            xs.Subscriptions.AssertEqual(
                Subscribe(200, 450)
            );
        }

        [TestMethod]
        public void Retry_Observable_Infinite()
        {
            var scheduler = new TestScheduler();

            var xs = scheduler.CreateColdObservable(
                OnNext(100, 1),
                OnNext(150, 2),
                OnNext(200, 3)
            );

            var res = scheduler.Start(() =>
                xs.Retry()
            );

            res.Messages.AssertEqual(
                OnNext(300, 1),
                OnNext(350, 2),
                OnNext(400, 3)
            );

            xs.Subscriptions.AssertEqual(
                Subscribe(200, 1000)
            );
        }

        [TestMethod]
        public void Retry_Observable_Error()
        {
            var scheduler = new TestScheduler();

            var ex = new Exception();

            var xs = scheduler.CreateColdObservable(
                OnNext(100, 1),
                OnNext(150, 2),
                OnNext(200, 3),
                OnError<int>(250, ex)
            );

            var res = scheduler.Start(() =>
                xs.Retry(), 1100
            );

            res.Messages.AssertEqual(
                OnNext(300, 1),
                OnNext(350, 2),
                OnNext(400, 3),
                OnNext(550, 1),
                OnNext(600, 2),
                OnNext(650, 3),
                OnNext(800, 1),
                OnNext(850, 2),
                OnNext(900, 3),
                OnNext(1050, 1)
            );

            xs.Subscriptions.AssertEqual(
                Subscribe(200, 450),
                Subscribe(450, 700),
                Subscribe(700, 950),
                Subscribe(950, 1100)
            );
        }

        [TestMethod]
        public void Retry_Observable_Throws1()
        {
            var scheduler1 = new TestScheduler();

            var xs = Observable.Return(1, scheduler1).Retry();

            xs.Subscribe(x => { throw new InvalidOperationException(); });

            ReactiveAssert.Throws<InvalidOperationException>(() => scheduler1.Start());
        }

        [TestMethod]
        public void Retry_Observable_Throws2()
        {
            var scheduler2 = new TestScheduler();

            var ys = Observable.Throw<int>(new Exception(), scheduler2).Retry();

            var d = ys.Subscribe(x => { }, ex => { throw new InvalidOperationException(); });

            scheduler2.ScheduleAbsolute(210, () => d.Dispose());

            scheduler2.Start();
        }

        [TestMethod]
        public void Retry_Observable_Throws3()
        {
            var scheduler3 = new TestScheduler();

            var zs = Observable.Return(1, scheduler3).Retry();

            zs.Subscribe(x => { }, ex => { }, () => { throw new InvalidOperationException(); });

            ReactiveAssert.Throws<InvalidOperationException>(() => scheduler3.Start());
        }

        /*
         * BREAKING CHANGE v2.0 > v1.x - The code below will loop endlessly, trying to repeat the failing subscription,
         *                               whose exception is propagated through OnError starting from v2.0.
         * 
        [TestMethod]
        public void Retry_Observable_Throws4()
        {
            var xss = Observable.Create<int>(new Func<IObserver<int>, Action>(o => { throw new InvalidOperationException(); })).Retry();

            ReactiveAssert.Throws<InvalidOperationException>(() => xss.Subscribe());
        }
         */

        [TestMethod]
        public void Retry_Observable_Default_ArgumentChecking()
        {
            ReactiveAssert.Throws<ArgumentNullException>(() => Observable.Retry<int>((IObservable<int>)null));
            ReactiveAssert.Throws<ArgumentNullException>(() => DummyObservable<int>.Instance.Retry().Subscribe(null));
        }

        [TestMethod]
        public void Retry_Observable_RetryCount_ArgumentChecking()
        {
            ReactiveAssert.Throws<ArgumentNullException>(() => Observable.Retry<int>(null, 0));
            ReactiveAssert.Throws<ArgumentOutOfRangeException>(() => DummyObservable<int>.Instance.Retry(-1));
            ReactiveAssert.Throws<ArgumentNullException>(() => DummyObservable<int>.Instance.Retry(0).Subscribe(null));
        }

        [TestMethod]
        public void Retry_Observable_RetryCount_Basic()
        {
            var scheduler = new TestScheduler();

            var ex = new Exception();

            var xs = scheduler.CreateColdObservable(
                OnNext(5, 1),
                OnNext(10, 2),
                OnNext(15, 3),
                OnError<int>(20, ex)
            );

            var res = scheduler.Start(() =>
                xs.Retry(3)
            );

            res.Messages.AssertEqual(
                OnNext(205, 1),
                OnNext(210, 2),
                OnNext(215, 3),
                OnNext(225, 1),
                OnNext(230, 2),
                OnNext(235, 3),
                OnNext(245, 1),
                OnNext(250, 2),
                OnNext(255, 3),
                OnError<int>(260, ex)
            );

            xs.Subscriptions.AssertEqual(
                Subscribe(200, 220),
                Subscribe(220, 240),
                Subscribe(240, 260)
            );
        }

        [TestMethod]
        public void Retry_Observable_RetryCount_Dispose()
        {
            var scheduler = new TestScheduler();

            var xs = scheduler.CreateColdObservable(
                OnNext(5, 1),
                OnNext(10, 2),
                OnNext(15, 3),
                OnError<int>(20, new Exception())
            );

            var res = scheduler.Start(() =>
                xs.Retry(3), 231
            );

            res.Messages.AssertEqual(
                OnNext(205, 1),
                OnNext(210, 2),
                OnNext(215, 3),
                OnNext(225, 1),
                OnNext(230, 2)
            );

            xs.Subscriptions.AssertEqual(
                Subscribe(200, 220),
                Subscribe(220, 231)
            );
        }

        [TestMethod]
        public void Retry_Observable_RetryCount_Infinite()
        {
            var scheduler = new TestScheduler();

            var xs = scheduler.CreateColdObservable(
                OnNext(100, 1),
                OnNext(150, 2),
                OnNext(200, 3)
            );

            var res = scheduler.Start(() =>
                xs.Retry(3)
            );

            res.Messages.AssertEqual(
                OnNext(300, 1),
                OnNext(350, 2),
                OnNext(400, 3)
            );

            xs.Subscriptions.AssertEqual(
                Subscribe(200, 1000)
            );
        }

        [TestMethod]
        public void Retry_Observable_RetryCount_Completed()
        {
            var scheduler = new TestScheduler();

            var xs = scheduler.CreateColdObservable(
                OnNext(100, 1),
                OnNext(150, 2),
                OnNext(200, 3),
                OnCompleted<int>(250)
            );

            var res = scheduler.Start(() =>
                xs.Retry(3)
            );

            res.Messages.AssertEqual(
                OnNext(300, 1),
                OnNext(350, 2),
                OnNext(400, 3),
                OnCompleted<int>(450)
            );

            xs.Subscriptions.AssertEqual(
                Subscribe(200, 450)
            );
        }

        [TestMethod]
        public void Retry_Observable_RetryCount_Throws()
        {
            var scheduler1 = new TestScheduler();

            var xs = Observable.Return(1, scheduler1).Retry(3);

            xs.Subscribe(x => { throw new InvalidOperationException(); });

            ReactiveAssert.Throws<InvalidOperationException>(() => scheduler1.Start());

            var scheduler2 = new TestScheduler();

            var ys = Observable.Throw<int>(new Exception(), scheduler2).Retry(100);

            var d = ys.Subscribe(x => { }, ex => { throw new InvalidOperationException(); });

            scheduler2.ScheduleAbsolute(10, () => d.Dispose());

            scheduler2.Start();

            var scheduler3 = new TestScheduler();

            var zs = Observable.Return(1, scheduler3).Retry(100);

            zs.Subscribe(x => { }, ex => { }, () => { throw new InvalidOperationException(); });

            ReactiveAssert.Throws<InvalidOperationException>(() => scheduler3.Start());

            var xss = Observable.Create<int>(new Func<IObserver<int>, Action>(o => { throw new InvalidOperationException(); })).Retry(3);

            ReactiveAssert.Throws<InvalidOperationException>(() => xss.Subscribe());
        }

        [TestMethod]
        public void Retry_Observable_RetryCount_Default_ArgumentChecking()
        {
            ReactiveAssert.Throws<ArgumentNullException>(() => Observable.Retry<int>(default(IObservable<int>), 0));
            ReactiveAssert.Throws<ArgumentOutOfRangeException>(() => DummyObservable<int>.Instance.Retry(-1));
            ReactiveAssert.Throws<ArgumentNullException>(() => DummyObservable<int>.Instance.Retry(0).Subscribe(null));
        }

        [TestMethod]
        public void Retry_Observable_RetryCount_Default()
        {
            Observable.Range(1, 3).Retry(3).AssertEqual(Observable.Range(1, 3).Retry(3));
        }

        #endregion

        #region + Scan +

        [TestMethod]
        public void Scan_ArgumentChecking()
        {
            var someObservable = Observable.Empty<int>();

            ReactiveAssert.Throws<ArgumentNullException>(() => Observable.Scan<int>(null, (_, __) => 0));
            ReactiveAssert.Throws<ArgumentNullException>(() => Observable.Scan<int>(someObservable, null));
            ReactiveAssert.Throws<ArgumentNullException>(() => Observable.Scan<int, int>(null, 0, (_, __) => 0));
            ReactiveAssert.Throws<ArgumentNullException>(() => Observable.Scan<int, int>(someObservable, 0, null));
        }

        [TestMethod]
        public void Scan_Seed_Never()
        {
            var scheduler = new TestScheduler();

            var xs = scheduler.CreateHotObservable<int>();

            var seed = 42;
            var res = scheduler.Start(() =>
                xs.Scan(seed, (acc, x) => acc + x)
            );

            res.Messages.AssertEqual(
            );

            xs.Subscriptions.AssertEqual(
                Subscribe(200, 1000)
            );
        }

        [TestMethod]
        public void Scan_Seed_Empty()
        {
            var scheduler = new TestScheduler();

            var xs = scheduler.CreateHotObservable(
                OnNext(150, 1),
                OnCompleted<int>(250)
            );

            var seed = 42;
            var res = scheduler.Start(() =>
                xs.Scan(seed, (acc, x) => acc + x)
            );

            res.Messages.AssertEqual(
                OnCompleted<int>(250)
            );

            xs.Subscriptions.AssertEqual(
                Subscribe(200, 250)
            );
        }

        [TestMethod]
        public void Scan_Seed_Return()
        {
            var scheduler = new TestScheduler();

            var xs = scheduler.CreateHotObservable(
                OnNext(150, 1),
                OnNext(220, 2),
                OnCompleted<int>(250)
            );

            var seed = 42;
            var res = scheduler.Start(() =>
                xs.Scan(seed, (acc, x) => acc + x)
            );

            res.Messages.AssertEqual(
                OnNext(220, seed + 2),
                OnCompleted<int>(250)
            );

            xs.Subscriptions.AssertEqual(
                Subscribe(200, 250)
            );
        }

        [TestMethod]
        public void Scan_Seed_Throw()
        {
            var scheduler = new TestScheduler();

            var ex = new Exception();
            var xs = scheduler.CreateHotObservable(
                OnNext(150, 1),
                OnError<int>(250, ex)
            );

            var seed = 42;
            var res = scheduler.Start(() =>
                xs.Scan(seed, (acc, x) => acc + x)
            );

            res.Messages.AssertEqual(
                OnError<int>(250, ex)
            );

            xs.Subscriptions.AssertEqual(
                Subscribe(200, 250)
            );
        }

        [TestMethod]
        public void Scan_Seed_SomeData()
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

            var seed = 1;
            var res = scheduler.Start(() =>
                xs.Scan(seed, (acc, x) => acc + x)
            );

            res.Messages.AssertEqual(
                OnNext(210, seed + 2),
                OnNext(220, seed + 2 + 3),
                OnNext(230, seed + 2 + 3 + 4),
                OnNext(240, seed + 2 + 3 + 4 + 5),
                OnCompleted<int>(250)
            );

            xs.Subscriptions.AssertEqual(
                Subscribe(200, 250)
            );
        }

        [TestMethod]
        public void Scan_Seed_AccumulatorThrows()
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

            var ex = new Exception();
            var seed = 1;
            var res = scheduler.Start(() =>
                xs.Scan(seed, (acc, x) => { if (x == 4) throw ex; return acc + x; })
            );

            res.Messages.AssertEqual(
                OnNext(210, seed + 2),
                OnNext(220, seed + 2 + 3),
                OnError<int>(230, ex)
            );

            xs.Subscriptions.AssertEqual(
                Subscribe(200, 230)
            );
        }

        [TestMethod]
        public void Scan_NoSeed_Never()
        {
            var scheduler = new TestScheduler();

            var xs = scheduler.CreateHotObservable<int>();

            var res = scheduler.Start(() =>
                xs.Scan((acc, x) => acc + x)
            );

            res.Messages.AssertEqual(
            );

            xs.Subscriptions.AssertEqual(
                Subscribe(200, 1000)
            );
        }

        [TestMethod]
        public void Scan_NoSeed_Empty()
        {
            var scheduler = new TestScheduler();

            var xs = scheduler.CreateHotObservable(
                OnNext(150, 1),
                OnCompleted<int>(250)
            );

            var res = scheduler.Start(() =>
                xs.Scan((acc, x) => acc + x)
            );

            res.Messages.AssertEqual(
                OnCompleted<int>(250)
            );

            xs.Subscriptions.AssertEqual(
                Subscribe(200, 250)
            );
        }

        [TestMethod]
        public void Scan_NoSeed_Return()
        {
            var scheduler = new TestScheduler();

            var xs = scheduler.CreateHotObservable(
                OnNext(150, 1),
                OnNext(220, 2),
                OnCompleted<int>(250)
            );

            var res = scheduler.Start(() =>
                xs.Scan((acc, x) => acc + x)
            );

            res.Messages.AssertEqual(
                OnNext(220, 2),
                OnCompleted<int>(250)
            );

            xs.Subscriptions.AssertEqual(
                Subscribe(200, 250)
            );
        }

        [TestMethod]
        public void Scan_NoSeed_Throw()
        {
            var scheduler = new TestScheduler();

            var ex = new Exception();
            var xs = scheduler.CreateHotObservable(
                OnNext(150, 1),
                OnError<int>(250, ex)
            );

            var res = scheduler.Start(() =>
                xs.Scan((acc, x) => acc + x)
            );

            res.Messages.AssertEqual(
                OnError<int>(250, ex)
            );

            xs.Subscriptions.AssertEqual(
                Subscribe(200, 250)
            );
        }

        [TestMethod]
        public void Scan_NoSeed_SomeData()
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
                xs.Scan((acc, x) => acc + x)
            );

            res.Messages.AssertEqual(
                OnNext(210, 2),
                OnNext(220, 2 + 3),
                OnNext(230, 2 + 3 + 4),
                OnNext(240, 2 + 3 + 4 + 5),
                OnCompleted<int>(250)
            );

            xs.Subscriptions.AssertEqual(
                Subscribe(200, 250)
            );
        }

        [TestMethod]
        public void Scan_NoSeed_AccumulatorThrows()
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

            var ex = new Exception();
            var res = scheduler.Start(() =>
                xs.Scan((acc, x) => { if (x == 4) throw ex; return acc + x; })
            );

            res.Messages.AssertEqual(
                OnNext(210, 2),
                OnNext(220, 2 + 3),
                OnError<int>(230, ex)
            );

            xs.Subscriptions.AssertEqual(
                Subscribe(200, 230)
            );
        }

        #endregion

        #region + SkipLast +

        [TestMethod]
        public void SkipLast_ArgumentChecking()
        {
            ReactiveAssert.Throws<ArgumentNullException>(() => Observable.SkipLast<int>(null, 0));
            ReactiveAssert.Throws<ArgumentOutOfRangeException>(() => Observable.SkipLast(DummyObservable<int>.Instance, -1));
        }

        [TestMethod]
        public void SkipLast_Zero_Completed()
        {
            var scheduler = new TestScheduler();

            var xs = scheduler.CreateHotObservable(
                OnNext(180, 1),
                OnNext(210, 2),
                OnNext(250, 3),
                OnNext(270, 4),
                OnNext(310, 5),
                OnNext(360, 6),
                OnNext(380, 7),
                OnNext(410, 8),
                OnNext(590, 9),
                OnCompleted<int>(650)
            );

            var res = scheduler.Start(() =>
                xs.SkipLast(0)
            );

            res.Messages.AssertEqual(
                OnNext(210, 2),
                OnNext(250, 3),
                OnNext(270, 4),
                OnNext(310, 5),
                OnNext(360, 6),
                OnNext(380, 7),
                OnNext(410, 8),
                OnNext(590, 9),
                OnCompleted<int>(650)
            );

            xs.Subscriptions.AssertEqual(
                Subscribe(200, 650)
            );
        }

        [TestMethod]
        public void SkipLast_Zero_Error()
        {
            var scheduler = new TestScheduler();

            var ex = new Exception();

            var xs = scheduler.CreateHotObservable(
                OnNext(180, 1),
                OnNext(210, 2),
                OnNext(250, 3),
                OnNext(270, 4),
                OnNext(310, 5),
                OnNext(360, 6),
                OnNext(380, 7),
                OnNext(410, 8),
                OnNext(590, 9),
                OnError<int>(650, ex)
            );

            var res = scheduler.Start(() =>
                xs.SkipLast(0)
            );

            res.Messages.AssertEqual(
                OnNext(210, 2),
                OnNext(250, 3),
                OnNext(270, 4),
                OnNext(310, 5),
                OnNext(360, 6),
                OnNext(380, 7),
                OnNext(410, 8),
                OnNext(590, 9),
                OnError<int>(650, ex)
            );

            xs.Subscriptions.AssertEqual(
                Subscribe(200, 650)
            );
        }

        [TestMethod]
        public void SkipLast_Zero_Disposed()
        {
            var scheduler = new TestScheduler();

            var xs = scheduler.CreateHotObservable(
                OnNext(180, 1),
                OnNext(210, 2),
                OnNext(250, 3),
                OnNext(270, 4),
                OnNext(310, 5),
                OnNext(360, 6),
                OnNext(380, 7),
                OnNext(410, 8),
                OnNext(590, 9)
            );

            var res = scheduler.Start(() =>
                xs.SkipLast(0)
            );

            res.Messages.AssertEqual(
                OnNext(210, 2),
                OnNext(250, 3),
                OnNext(270, 4),
                OnNext(310, 5),
                OnNext(360, 6),
                OnNext(380, 7),
                OnNext(410, 8),
                OnNext(590, 9)
            );

            xs.Subscriptions.AssertEqual(
                Subscribe(200, 1000)
            );
        }

        [TestMethod]
        public void SkipLast_One_Completed()
        {
            var scheduler = new TestScheduler();

            var xs = scheduler.CreateHotObservable(
                OnNext(180, 1),
                OnNext(210, 2),
                OnNext(250, 3),
                OnNext(270, 4),
                OnNext(310, 5),
                OnNext(360, 6),
                OnNext(380, 7),
                OnNext(410, 8),
                OnNext(590, 9),
                OnCompleted<int>(650)
            );

            var res = scheduler.Start(() =>
                xs.SkipLast(1)
            );

            res.Messages.AssertEqual(
                OnNext(250, 2),
                OnNext(270, 3),
                OnNext(310, 4),
                OnNext(360, 5),
                OnNext(380, 6),
                OnNext(410, 7),
                OnNext(590, 8),
                OnCompleted<int>(650)
            );

            xs.Subscriptions.AssertEqual(
                Subscribe(200, 650)
            );
        }

        [TestMethod]
        public void SkipLast_One_Error()
        {
            var scheduler = new TestScheduler();

            var ex = new Exception();

            var xs = scheduler.CreateHotObservable(
                OnNext(180, 1),
                OnNext(210, 2),
                OnNext(250, 3),
                OnNext(270, 4),
                OnNext(310, 5),
                OnNext(360, 6),
                OnNext(380, 7),
                OnNext(410, 8),
                OnNext(590, 9),
                OnError<int>(650, ex)
            );

            var res = scheduler.Start(() =>
                xs.SkipLast(1)
            );

            res.Messages.AssertEqual(
                OnNext(250, 2),
                OnNext(270, 3),
                OnNext(310, 4),
                OnNext(360, 5),
                OnNext(380, 6),
                OnNext(410, 7),
                OnNext(590, 8),
                OnError<int>(650, ex)
            );

            xs.Subscriptions.AssertEqual(
                Subscribe(200, 650)
            );
        }

        [TestMethod]
        public void SkipLast_One_Disposed()
        {
            var scheduler = new TestScheduler();

            var xs = scheduler.CreateHotObservable(
                OnNext(180, 1),
                OnNext(210, 2),
                OnNext(250, 3),
                OnNext(270, 4),
                OnNext(310, 5),
                OnNext(360, 6),
                OnNext(380, 7),
                OnNext(410, 8),
                OnNext(590, 9)
            );

            var res = scheduler.Start(() =>
                xs.SkipLast(1)
            );

            res.Messages.AssertEqual(
                OnNext(250, 2),
                OnNext(270, 3),
                OnNext(310, 4),
                OnNext(360, 5),
                OnNext(380, 6),
                OnNext(410, 7),
                OnNext(590, 8)
            );

            xs.Subscriptions.AssertEqual(
                Subscribe(200, 1000)
            );
        }

        [TestMethod]
        public void SkipLast_Three_Completed()
        {
            var scheduler = new TestScheduler();

            var xs = scheduler.CreateHotObservable(
                OnNext(180, 1),
                OnNext(210, 2),
                OnNext(250, 3),
                OnNext(270, 4),
                OnNext(310, 5),
                OnNext(360, 6),
                OnNext(380, 7),
                OnNext(410, 8),
                OnNext(590, 9),
                OnCompleted<int>(650)
            );

            var res = scheduler.Start(() =>
                xs.SkipLast(3)
            );

            res.Messages.AssertEqual(
                OnNext(310, 2),
                OnNext(360, 3),
                OnNext(380, 4),
                OnNext(410, 5),
                OnNext(590, 6),
                OnCompleted<int>(650)
            );

            xs.Subscriptions.AssertEqual(
                Subscribe(200, 650)
            );
        }

        [TestMethod]
        public void SkipLast_Three_Error()
        {
            var scheduler = new TestScheduler();

            var ex = new Exception();

            var xs = scheduler.CreateHotObservable(
                OnNext(180, 1),
                OnNext(210, 2),
                OnNext(250, 3),
                OnNext(270, 4),
                OnNext(310, 5),
                OnNext(360, 6),
                OnNext(380, 7),
                OnNext(410, 8),
                OnNext(590, 9),
                OnError<int>(650, ex)
            );

            var res = scheduler.Start(() =>
                xs.SkipLast(3)
            );

            res.Messages.AssertEqual(
                OnNext(310, 2),
                OnNext(360, 3),
                OnNext(380, 4),
                OnNext(410, 5),
                OnNext(590, 6),
                OnError<int>(650, ex)
            );

            xs.Subscriptions.AssertEqual(
                Subscribe(200, 650)
            );
        }

        [TestMethod]
        public void SkipLast_Three_Disposed()
        {
            var scheduler = new TestScheduler();

            var xs = scheduler.CreateHotObservable(
                OnNext(180, 1),
                OnNext(210, 2),
                OnNext(250, 3),
                OnNext(270, 4),
                OnNext(310, 5),
                OnNext(360, 6),
                OnNext(380, 7),
                OnNext(410, 8),
                OnNext(590, 9)
            );

            var res = scheduler.Start(() =>
                xs.SkipLast(3)
            );

            res.Messages.AssertEqual(
                OnNext(310, 2),
                OnNext(360, 3),
                OnNext(380, 4),
                OnNext(410, 5),
                OnNext(590, 6)
            );

            xs.Subscriptions.AssertEqual(
                Subscribe(200, 1000)
            );
        }

        #endregion

        #region StartWith

        [TestMethod]
        public void StartWith_ArgumentChecking()
        {
            var scheduler = new TestScheduler();
            var someObservable = Observable.Empty<int>();

            ReactiveAssert.Throws<ArgumentNullException>(() => Observable.StartWith(default(IObservable<int>), 1));
            ReactiveAssert.Throws<ArgumentNullException>(() => Observable.StartWith(someObservable, default(int[])));

            ReactiveAssert.Throws<ArgumentNullException>(() => Observable.StartWith(default(IObservable<int>), scheduler, 1));
            ReactiveAssert.Throws<ArgumentNullException>(() => Observable.StartWith(someObservable, default(IScheduler), 1));
            ReactiveAssert.Throws<ArgumentNullException>(() => Observable.StartWith(someObservable, scheduler, default(int[])));
        }

        [TestMethod]
        public void StartWith()
        {
            var scheduler = new TestScheduler();

            var xs = scheduler.CreateHotObservable(
                OnNext(150, 1),
                OnNext(220, 2),
                OnCompleted<int>(250)
            );

            var res = scheduler.Start(() =>
                xs.StartWith(1)
            );

            res.Messages.AssertEqual(
                OnNext(200, 1),
                OnNext(220, 2),
                OnCompleted<int>(250)
            );
        }

        [TestMethod]
        public void StartWith_Scheduler()
        {
            var scheduler = new TestScheduler();

            var xs = scheduler.CreateHotObservable(
                OnNext(150, 1),
                OnNext(220, 4),
                OnCompleted<int>(250)
            );

            var res = scheduler.Start(() =>
                xs.StartWith(scheduler, 1, 2, 3)
            );

            res.Messages.AssertEqual(
                OnNext(201, 1),
                OnNext(202, 2),
                OnNext(203, 3),
                OnNext(220, 4),
                OnCompleted<int>(250)
            );
        }

        [TestMethod]
        public void StartWith_Enumerable()
        {
            var scheduler = new TestScheduler();

            var xs = scheduler.CreateHotObservable(
                OnNext(150, 1),
                OnNext(220, 4),
                OnCompleted<int>(250)
            );

            List<int> data = new List<int>(new[] { 1, 2, 3 });
            var res = scheduler.Start(() =>
                xs.StartWith(data)
            );

            res.Messages.AssertEqual(
                OnNext(200, 1),
                OnNext(200, 2),
                OnNext(200, 3),
                OnNext(220, 4),
                OnCompleted<int>(250)
            );
        }

        [TestMethod]
        public void StartWith_Enumerable_Scheduler()
        {
            var scheduler = new TestScheduler();

            var xs = scheduler.CreateHotObservable(
                OnNext(150, 1),
                OnNext(220, 4),
                OnCompleted<int>(250)
            );

            List<int> data = new List<int>(new[] { 1, 2, 3 });
            var res = scheduler.Start(() =>
                xs.StartWith(scheduler, data)
            );

            res.Messages.AssertEqual(
                OnNext(201, 1),
                OnNext(202, 2),
                OnNext(203, 3),
                OnNext(220, 4),
                OnCompleted<int>(250)
            );
        }

        #endregion

        #region + TakeLast +

        [TestMethod]
        public void TakeLast_ArgumentChecking()
        {
            ReactiveAssert.Throws<ArgumentNullException>(() => Observable.TakeLast<int>(null, 0));
            ReactiveAssert.Throws<ArgumentOutOfRangeException>(() => Observable.TakeLast(DummyObservable<int>.Instance, -1));
            ReactiveAssert.Throws<ArgumentNullException>(() => Observable.TakeLast<int>(null, 0, Scheduler.Default));
            ReactiveAssert.Throws<ArgumentOutOfRangeException>(() => Observable.TakeLast(DummyObservable<int>.Instance, -1, Scheduler.Default));
            ReactiveAssert.Throws<ArgumentNullException>(() => Observable.TakeLast(DummyObservable<int>.Instance, 0, default(IScheduler)));
        }

        [TestMethod]
        public void TakeLast_Zero_Completed()
        {
            var scheduler = new TestScheduler();

            var xs = scheduler.CreateHotObservable(
                OnNext(180, 1),
                OnNext(210, 2),
                OnNext(250, 3),
                OnNext(270, 4),
                OnNext(310, 5),
                OnNext(360, 6),
                OnNext(380, 7),
                OnNext(410, 8),
                OnNext(590, 9),
                OnCompleted<int>(650)
            );

            var res = scheduler.Start(() =>
                xs.TakeLast(0)
            );

            res.Messages.AssertEqual(
                OnCompleted<int>(650)
            );

            xs.Subscriptions.AssertEqual(
                Subscribe(200, 650)
            );
        }

        [TestMethod]
        public void TakeLast_Zero_Error()
        {
            var scheduler = new TestScheduler();

            var ex = new Exception();

            var xs = scheduler.CreateHotObservable(
                OnNext(180, 1),
                OnNext(210, 2),
                OnNext(250, 3),
                OnNext(270, 4),
                OnNext(310, 5),
                OnNext(360, 6),
                OnNext(380, 7),
                OnNext(410, 8),
                OnNext(590, 9),
                OnError<int>(650, ex)
            );

            var res = scheduler.Start(() =>
                xs.TakeLast(0)
            );

            res.Messages.AssertEqual(
                OnError<int>(650, ex)
            );

            xs.Subscriptions.AssertEqual(
                Subscribe(200, 650)
            );
        }

        [TestMethod]
        public void TakeLast_Zero_Disposed()
        {
            var scheduler = new TestScheduler();

            var xs = scheduler.CreateHotObservable(
                OnNext(180, 1),
                OnNext(210, 2),
                OnNext(250, 3),
                OnNext(270, 4),
                OnNext(310, 5),
                OnNext(360, 6),
                OnNext(380, 7),
                OnNext(410, 8),
                OnNext(590, 9)
            );

            var res = scheduler.Start(() =>
                xs.TakeLast(0)
            );

            res.Messages.AssertEqual(
            );

            xs.Subscriptions.AssertEqual(
                Subscribe(200, 1000)
            );
        }

        [TestMethod]
        public void TakeLast_One_Completed()
        {
            var scheduler = new TestScheduler();

            var xs = scheduler.CreateHotObservable(
                OnNext(180, 1),
                OnNext(210, 2),
                OnNext(250, 3),
                OnNext(270, 4),
                OnNext(310, 5),
                OnNext(360, 6),
                OnNext(380, 7),
                OnNext(410, 8),
                OnNext(590, 9),
                OnCompleted<int>(650)
            );

            var res = scheduler.Start(() =>
                xs.TakeLast(1)
            );

            res.Messages.AssertEqual(
                OnNext(650, 9),
                OnCompleted<int>(650)
            );

            xs.Subscriptions.AssertEqual(
                Subscribe(200, 650)
            );
        }

        [TestMethod]
        public void TakeLast_One_Error()
        {
            var scheduler = new TestScheduler();

            var ex = new Exception();

            var xs = scheduler.CreateHotObservable(
                OnNext(180, 1),
                OnNext(210, 2),
                OnNext(250, 3),
                OnNext(270, 4),
                OnNext(310, 5),
                OnNext(360, 6),
                OnNext(380, 7),
                OnNext(410, 8),
                OnNext(590, 9),
                OnError<int>(650, ex)
            );

            var res = scheduler.Start(() =>
                xs.TakeLast(1)
            );

            res.Messages.AssertEqual(
                OnError<int>(650, ex)
            );

            xs.Subscriptions.AssertEqual(
                Subscribe(200, 650)
            );
        }

        [TestMethod]
        public void TakeLast_One_Disposed()
        {
            var scheduler = new TestScheduler();

            var xs = scheduler.CreateHotObservable(
                OnNext(180, 1),
                OnNext(210, 2),
                OnNext(250, 3),
                OnNext(270, 4),
                OnNext(310, 5),
                OnNext(360, 6),
                OnNext(380, 7),
                OnNext(410, 8),
                OnNext(590, 9)
            );

            var res = scheduler.Start(() =>
                xs.TakeLast(1)
            );

            res.Messages.AssertEqual(
            );

            xs.Subscriptions.AssertEqual(
                Subscribe(200, 1000)
            );
        }

        [TestMethod]
        public void TakeLast_Three_Completed()
        {
            var scheduler = new TestScheduler();

            var xs = scheduler.CreateHotObservable(
                OnNext(180, 1),
                OnNext(210, 2),
                OnNext(250, 3),
                OnNext(270, 4),
                OnNext(310, 5),
                OnNext(360, 6),
                OnNext(380, 7),
                OnNext(410, 8),
                OnNext(590, 9),
                OnCompleted<int>(650)
            );

            var res = scheduler.Start(() =>
                xs.TakeLast(3)
            );

            res.Messages.AssertEqual(
                OnNext(650, 7),
                OnNext(650, 8),
                OnNext(650, 9),
                OnCompleted<int>(650)
            );

            xs.Subscriptions.AssertEqual(
                Subscribe(200, 650)
            );
        }

        [TestMethod]
        public void TakeLast_Three_Error()
        {
            var scheduler = new TestScheduler();

            var ex = new Exception();

            var xs = scheduler.CreateHotObservable(
                OnNext(180, 1),
                OnNext(210, 2),
                OnNext(250, 3),
                OnNext(270, 4),
                OnNext(310, 5),
                OnNext(360, 6),
                OnNext(380, 7),
                OnNext(410, 8),
                OnNext(590, 9),
                OnError<int>(650, ex)
            );

            var res = scheduler.Start(() =>
                xs.TakeLast(3)
            );

            res.Messages.AssertEqual(
                OnError<int>(650, ex)
            );

            xs.Subscriptions.AssertEqual(
                Subscribe(200, 650)
            );
        }

        [TestMethod]
        public void TakeLast_Three_Disposed()
        {
            var scheduler = new TestScheduler();

            var xs = scheduler.CreateHotObservable(
                OnNext(180, 1),
                OnNext(210, 2),
                OnNext(250, 3),
                OnNext(270, 4),
                OnNext(310, 5),
                OnNext(360, 6),
                OnNext(380, 7),
                OnNext(410, 8),
                OnNext(590, 9)
            );

            var res = scheduler.Start(() =>
                xs.TakeLast(3)
            );

            res.Messages.AssertEqual(
            );

            xs.Subscriptions.AssertEqual(
                Subscribe(200, 1000)
            );
        }

        [TestMethod]
        public void TakeLast_LongRunning_Regular()
        {
            var res = Observable.Range(0, 100, Scheduler.Default).TakeLast(10, NewThreadScheduler.Default);

            var lst = new List<int>();
            res.ForEach(lst.Add);

            Assert.IsTrue(Enumerable.Range(90, 10).SequenceEqual(lst));
        }

        #endregion

        #region + TakeLastBuffer +

        [TestMethod]
        public void TakeLastBuffer_ArgumentChecking()
        {
            ReactiveAssert.Throws<ArgumentNullException>(() => Observable.TakeLastBuffer<int>(null, 0));
            ReactiveAssert.Throws<ArgumentOutOfRangeException>(() => Observable.TakeLastBuffer(DummyObservable<int>.Instance, -1));
        }

        [TestMethod]
        public void TakeLastBuffer_Zero_Completed()
        {
            var scheduler = new TestScheduler();

            var xs = scheduler.CreateHotObservable(
                OnNext(180, 1),
                OnNext(210, 2),
                OnNext(250, 3),
                OnNext(270, 4),
                OnNext(310, 5),
                OnNext(360, 6),
                OnNext(380, 7),
                OnNext(410, 8),
                OnNext(590, 9),
                OnCompleted<int>(650)
            );

            var res = scheduler.Start(() =>
                xs.TakeLastBuffer(0)
            );

            res.Messages.AssertEqual(
                OnNext<IList<int>>(650, lst => lst.Count == 0),
                OnCompleted<IList<int>>(650)
            );

            xs.Subscriptions.AssertEqual(
                Subscribe(200, 650)
            );
        }

        [TestMethod]
        public void TakeLastBuffer_Zero_Error()
        {
            var scheduler = new TestScheduler();

            var ex = new Exception();

            var xs = scheduler.CreateHotObservable(
                OnNext(180, 1),
                OnNext(210, 2),
                OnNext(250, 3),
                OnNext(270, 4),
                OnNext(310, 5),
                OnNext(360, 6),
                OnNext(380, 7),
                OnNext(410, 8),
                OnNext(590, 9),
                OnError<int>(650, ex)
            );

            var res = scheduler.Start(() =>
                xs.TakeLastBuffer(0)
            );

            res.Messages.AssertEqual(
                OnError<IList<int>>(650, ex)
            );

            xs.Subscriptions.AssertEqual(
                Subscribe(200, 650)
            );
        }

        [TestMethod]
        public void TakeLastBuffer_Zero_Disposed()
        {
            var scheduler = new TestScheduler();

            var xs = scheduler.CreateHotObservable(
                OnNext(180, 1),
                OnNext(210, 2),
                OnNext(250, 3),
                OnNext(270, 4),
                OnNext(310, 5),
                OnNext(360, 6),
                OnNext(380, 7),
                OnNext(410, 8),
                OnNext(590, 9)
            );

            var res = scheduler.Start(() =>
                xs.TakeLastBuffer(0)
            );

            res.Messages.AssertEqual(
            );

            xs.Subscriptions.AssertEqual(
                Subscribe(200, 1000)
            );
        }

        [TestMethod]
        public void TakeLastBuffer_One_Completed()
        {
            var scheduler = new TestScheduler();

            var xs = scheduler.CreateHotObservable(
                OnNext(180, 1),
                OnNext(210, 2),
                OnNext(250, 3),
                OnNext(270, 4),
                OnNext(310, 5),
                OnNext(360, 6),
                OnNext(380, 7),
                OnNext(410, 8),
                OnNext(590, 9),
                OnCompleted<int>(650)
            );

            var res = scheduler.Start(() =>
                xs.TakeLastBuffer(1)
            );

            res.Messages.AssertEqual(
                OnNext<IList<int>>(650, lst => lst.SequenceEqual(new[] { 9 })),
                OnCompleted<IList<int>>(650)
            );

            xs.Subscriptions.AssertEqual(
                Subscribe(200, 650)
            );
        }

        [TestMethod]
        public void TakeLastBuffer_One_Error()
        {
            var scheduler = new TestScheduler();

            var ex = new Exception();

            var xs = scheduler.CreateHotObservable(
                OnNext(180, 1),
                OnNext(210, 2),
                OnNext(250, 3),
                OnNext(270, 4),
                OnNext(310, 5),
                OnNext(360, 6),
                OnNext(380, 7),
                OnNext(410, 8),
                OnNext(590, 9),
                OnError<int>(650, ex)
            );

            var res = scheduler.Start(() =>
                xs.TakeLastBuffer(1)
            );

            res.Messages.AssertEqual(
                OnError<IList<int>>(650, ex)
            );

            xs.Subscriptions.AssertEqual(
                Subscribe(200, 650)
            );
        }

        [TestMethod]
        public void TakeLastBuffer_One_Disposed()
        {
            var scheduler = new TestScheduler();

            var xs = scheduler.CreateHotObservable(
                OnNext(180, 1),
                OnNext(210, 2),
                OnNext(250, 3),
                OnNext(270, 4),
                OnNext(310, 5),
                OnNext(360, 6),
                OnNext(380, 7),
                OnNext(410, 8),
                OnNext(590, 9)
            );

            var res = scheduler.Start(() =>
                xs.TakeLastBuffer(1)
            );

            res.Messages.AssertEqual(
            );

            xs.Subscriptions.AssertEqual(
                Subscribe(200, 1000)
            );
        }

        [TestMethod]
        public void TakeLastBuffer_Three_Completed()
        {
            var scheduler = new TestScheduler();

            var xs = scheduler.CreateHotObservable(
                OnNext(180, 1),
                OnNext(210, 2),
                OnNext(250, 3),
                OnNext(270, 4),
                OnNext(310, 5),
                OnNext(360, 6),
                OnNext(380, 7),
                OnNext(410, 8),
                OnNext(590, 9),
                OnCompleted<int>(650)
            );

            var res = scheduler.Start(() =>
                xs.TakeLastBuffer(3)
            );

            res.Messages.AssertEqual(
                OnNext<IList<int>>(650, lst => lst.SequenceEqual(new[] { 7, 8, 9 })),
                OnCompleted<IList<int>>(650)
            );

            xs.Subscriptions.AssertEqual(
                Subscribe(200, 650)
            );
        }

        [TestMethod]
        public void TakeLastBuffer_Three_Error()
        {
            var scheduler = new TestScheduler();

            var ex = new Exception();

            var xs = scheduler.CreateHotObservable(
                OnNext(180, 1),
                OnNext(210, 2),
                OnNext(250, 3),
                OnNext(270, 4),
                OnNext(310, 5),
                OnNext(360, 6),
                OnNext(380, 7),
                OnNext(410, 8),
                OnNext(590, 9),
                OnError<int>(650, ex)
            );

            var res = scheduler.Start(() =>
                xs.TakeLastBuffer(3)
            );

            res.Messages.AssertEqual(
                OnError<IList<int>>(650, ex)
            );

            xs.Subscriptions.AssertEqual(
                Subscribe(200, 650)
            );
        }

        [TestMethod]
        public void TakeLastBuffer_Three_Disposed()
        {
            var scheduler = new TestScheduler();

            var xs = scheduler.CreateHotObservable(
                OnNext(180, 1),
                OnNext(210, 2),
                OnNext(250, 3),
                OnNext(270, 4),
                OnNext(310, 5),
                OnNext(360, 6),
                OnNext(380, 7),
                OnNext(410, 8),
                OnNext(590, 9)
            );

            var res = scheduler.Start(() =>
                xs.TakeLastBuffer(3)
            );

            res.Messages.AssertEqual(
            );

            xs.Subscriptions.AssertEqual(
                Subscribe(200, 1000)
            );
        }

        #endregion
        
        #region + Window +

        [TestMethod]
        public void WindowWithCount_ArgumentChecking()
        {
            ReactiveAssert.Throws<ArgumentNullException>(() => Observable.Window(default(IObservable<int>), 1, 1));
            ReactiveAssert.Throws<ArgumentOutOfRangeException>(() => Observable.Window(DummyObservable<int>.Instance, 0, 1));
            ReactiveAssert.Throws<ArgumentOutOfRangeException>(() => Observable.Window(DummyObservable<int>.Instance, 1, 0));
            ReactiveAssert.Throws<ArgumentNullException>(() => Observable.Window(default(IObservable<int>), 1));
            ReactiveAssert.Throws<ArgumentOutOfRangeException>(() => Observable.Window(DummyObservable<int>.Instance, 0));
        }

        [TestMethod]
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

        [TestMethod]
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

            Assert.AreEqual(5, observers.Count);

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

        [TestMethod]
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

            Assert.IsTrue(windowCreationTimes.Last() < 400);

            Assert.AreEqual(4, observers.Count);

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

        [TestMethod]
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
                    d.Dispose();
            });

            scheduler.Start();

            Assert.IsTrue(windowCreationTimes.Last() < 400);

            Assert.AreEqual(4, observers.Count);

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

        [TestMethod]
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

        [TestMethod]
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

        [TestMethod]
        public void WindowWithCount_Default()
        {
            Observable.Range(1, 10).Window(3).Skip(1).First().SequenceEqual(new[] { 4, 5, 6 }.ToObservable());
            Observable.Range(1, 10).Window(3).Skip(1).First().SequenceEqual(new[] { 4, 5, 6 }.ToObservable());
            Observable.Range(1, 10).Window(3, 2).Skip(1).First().SequenceEqual(new[] { 3, 4, 5 }.ToObservable());
        }

        #endregion
    }
}

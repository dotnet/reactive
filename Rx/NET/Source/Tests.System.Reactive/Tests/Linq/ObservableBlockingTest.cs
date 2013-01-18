// Copyright (c) Microsoft Open Technologies, Inc. All rights reserved. See License.txt in the project root for license information.

using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reactive.Concurrency;
using System.Reactive.Disposables;
using System.Reactive.Linq;
using System.Threading;
using Microsoft.Reactive.Testing;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using ReactiveTests.Dummies;

#if !NO_TPL
using System.Threading.Tasks;
#endif

namespace ReactiveTests.Tests
{
    [TestClass]
    public partial class ObservableBlockingTest : ReactiveTest
    {
        #region Chunkify

        [TestMethod]
        public void Chunkify_ArgumentChecking()
        {
            ReactiveAssert.Throws<ArgumentNullException>(() => Observable.Chunkify(default(IObservable<int>)));
        }

        [TestMethod]
        public void Chunkify_Regular1()
        {
            var scheduler = new TestScheduler();

            var xs = scheduler.CreateHotObservable(
                OnNext(100, 1),
                OnNext(200, 2),
                OnNext(300, 3),
                OnNext(400, 4),
                OnNext(500, 5),
                OnNext(600, 6),
                OnNext(700, 7),
                OnNext(800, 8),
                OnCompleted<int>(900)
            );

            var ys = xs.Chunkify();
            var e = default(IEnumerator<IList<int>>);

            var res = new List<IList<int>>();

            var log = new Action(() =>
            {
                Assert.IsTrue(e.MoveNext());
                res.Add(e.Current);
            });

            scheduler.ScheduleAbsolute(250, () => { e = ys.GetEnumerator(); });
            scheduler.ScheduleAbsolute(270, log);
            scheduler.ScheduleAbsolute(310, log);
            scheduler.ScheduleAbsolute(450, log);
            scheduler.ScheduleAbsolute(470, log);
            scheduler.ScheduleAbsolute(750, log);
            scheduler.ScheduleAbsolute(850, log);
            scheduler.ScheduleAbsolute(950, log);
            scheduler.ScheduleAbsolute(980, () => Assert.IsFalse(e.MoveNext()));

            scheduler.Start();

            xs.Subscriptions.AssertEqual(
                Subscribe(250, 900)
            );

            Assert.AreEqual(7, res.Count);
            Assert.IsTrue(res[0].SequenceEqual(new int[] { }));
            Assert.IsTrue(res[1].SequenceEqual(new int[] { 3 }));
            Assert.IsTrue(res[2].SequenceEqual(new int[] { 4 }));
            Assert.IsTrue(res[3].SequenceEqual(new int[] { }));
            Assert.IsTrue(res[4].SequenceEqual(new int[] { 5, 6, 7 }));
            Assert.IsTrue(res[5].SequenceEqual(new int[] { 8 }));
            Assert.IsTrue(res[6].SequenceEqual(new int[] { }));
        }

        [TestMethod]
        public void Chunkify_Regular2()
        {
            var scheduler = new TestScheduler();

            var xs = scheduler.CreateHotObservable(
                OnNext(100, 1),
                OnNext(200, 2),
                OnNext(300, 3),
                OnNext(400, 4),
                OnNext(500, 5),
                OnNext(600, 6),
                OnNext(700, 7),
                OnNext(800, 8),
                OnCompleted<int>(900)
            );

            var ys = xs.Chunkify();
            var e = default(IEnumerator<IList<int>>);

            var res = new List<IList<int>>();

            var log = new Action(() =>
            {
                Assert.IsTrue(e.MoveNext());
                res.Add(e.Current);
            });

            scheduler.ScheduleAbsolute(250, () => { e = ys.GetEnumerator(); });
            scheduler.ScheduleAbsolute(550, log);
            scheduler.ScheduleAbsolute(950, log);
            scheduler.ScheduleAbsolute(980, () => Assert.IsFalse(e.MoveNext()));

            scheduler.Start();

            xs.Subscriptions.AssertEqual(
                Subscribe(250, 900)
            );

            Assert.AreEqual(2, res.Count);
            Assert.IsTrue(res[0].SequenceEqual(new int[] { 3, 4, 5 }));
            Assert.IsTrue(res[1].SequenceEqual(new int[] { 6, 7, 8 }));
        }

        [TestMethod]
        public void Chunkify_Error()
        {
            var ex = new Exception();

            var scheduler = new TestScheduler();

            var xs = scheduler.CreateHotObservable(
                OnNext(100, 1),
                OnNext(200, 2),
                OnNext(300, 3),
                OnNext(400, 4),
                OnNext(500, 5),
                OnNext(600, 6),
                OnError<int>(700, ex)
            );

            var ys = xs.Chunkify();
            var e = default(IEnumerator<IList<int>>);

            var res = new List<IList<int>>();

            var log = new Action(() =>
            {
                Assert.IsTrue(e.MoveNext());
                res.Add(e.Current);
            });

            scheduler.ScheduleAbsolute(250, () => { e = ys.GetEnumerator(); });
            scheduler.ScheduleAbsolute(270, log);
            scheduler.ScheduleAbsolute(310, log);
            scheduler.ScheduleAbsolute(450, log);
            scheduler.ScheduleAbsolute(470, log);
            scheduler.ScheduleAbsolute(750, () =>
            {
                try
                {
                    e.MoveNext();
                    Assert.Fail();
                }
                catch (Exception error)
                {
                    Assert.AreSame(ex, error);
                }
            });

            scheduler.Start();

            xs.Subscriptions.AssertEqual(
                Subscribe(250, 700)
            );

            Assert.AreEqual(4, res.Count);
            Assert.IsTrue(res[0].SequenceEqual(new int[] { }));
            Assert.IsTrue(res[1].SequenceEqual(new int[] { 3 }));
            Assert.IsTrue(res[2].SequenceEqual(new int[] { 4 }));
            Assert.IsTrue(res[3].SequenceEqual(new int[] { }));
        }

        #endregion

        #region Collect

        [TestMethod]
        public void Collect_ArgumentChecking()
        {
            var someObservable = Observable.Empty<int>();

            ReactiveAssert.Throws<ArgumentNullException>(() => Observable.Collect(default(IObservable<int>), () => 0, (x, y) => x));
            ReactiveAssert.Throws<ArgumentNullException>(() => Observable.Collect(someObservable, default(Func<int>), (x, y) => x));
            ReactiveAssert.Throws<ArgumentNullException>(() => Observable.Collect(someObservable, () => 0, default(Func<int, int, int>)));

            ReactiveAssert.Throws<ArgumentNullException>(() => Observable.Collect(default(IObservable<int>), () => 0, (x, y) => x, x => x));
            ReactiveAssert.Throws<ArgumentNullException>(() => Observable.Collect(someObservable, default(Func<int>), (x, y) => x, x => x));
            ReactiveAssert.Throws<ArgumentNullException>(() => Observable.Collect(someObservable, () => 0, default(Func<int, int, int>), x => x));
            ReactiveAssert.Throws<ArgumentNullException>(() => Observable.Collect(someObservable, () => 0, (x, y) => x, default(Func<int, int>)));
        }

        [TestMethod]
        public void Collect_Regular1()
        {
            var scheduler = new TestScheduler();

            var xs = scheduler.CreateHotObservable(
                OnNext(100, 1),
                OnNext(200, 2),
                OnNext(300, 3),
                OnNext(400, 4),
                OnNext(500, 5),
                OnNext(600, 6),
                OnNext(700, 7),
                OnNext(800, 8),
                OnCompleted<int>(900)
            );

            var ys = xs.Collect(() => 0, (x, y) => x + y);
            var e = default(IEnumerator<int>);

            var res = new List<int>();

            var log = new Action(() =>
            {
                Assert.IsTrue(e.MoveNext());
                res.Add(e.Current);
            });

            scheduler.ScheduleAbsolute(250, () => { e = ys.GetEnumerator(); });
            scheduler.ScheduleAbsolute(270, log);
            scheduler.ScheduleAbsolute(310, log);
            scheduler.ScheduleAbsolute(450, log);
            scheduler.ScheduleAbsolute(470, log);
            scheduler.ScheduleAbsolute(750, log);
            scheduler.ScheduleAbsolute(850, log);
            scheduler.ScheduleAbsolute(950, log);
            scheduler.ScheduleAbsolute(980, () => Assert.IsFalse(e.MoveNext()));

            scheduler.Start();

            xs.Subscriptions.AssertEqual(
                Subscribe(250, 900)
            );

            Assert.AreEqual(7, res.Count);
            Assert.AreEqual(res[0], new int[] { }.Sum());
            Assert.AreEqual(res[1], new int[] { 3 }.Sum());
            Assert.AreEqual(res[2], new int[] { 4 }.Sum());
            Assert.AreEqual(res[3], new int[] { }.Sum());
            Assert.AreEqual(res[4], new int[] { 5, 6, 7 }.Sum());
            Assert.AreEqual(res[5], new int[] { 8 }.Sum());
            Assert.AreEqual(res[6], new int[] { }.Sum());
        }

        [TestMethod]
        public void Collect_Regular2()
        {
            var scheduler = new TestScheduler();

            var xs = scheduler.CreateHotObservable(
                OnNext(100, 1),
                OnNext(200, 2),
                OnNext(300, 3),
                OnNext(400, 4),
                OnNext(500, 5),
                OnNext(600, 6),
                OnNext(700, 7),
                OnNext(800, 8),
                OnCompleted<int>(900)
            );

            var ys = xs.Collect(() => 0, (x, y) => x + y);
            var e = default(IEnumerator<int>);

            var res = new List<int>();

            var log = new Action(() =>
            {
                Assert.IsTrue(e.MoveNext());
                res.Add(e.Current);
            });

            scheduler.ScheduleAbsolute(250, () => { e = ys.GetEnumerator(); });
            scheduler.ScheduleAbsolute(550, log);
            scheduler.ScheduleAbsolute(950, log);
            scheduler.ScheduleAbsolute(980, () => Assert.IsFalse(e.MoveNext()));

            scheduler.Start();

            xs.Subscriptions.AssertEqual(
                Subscribe(250, 900)
            );

            Assert.AreEqual(2, res.Count);
            Assert.AreEqual(res[0], new int[] { 3, 4, 5 }.Sum());
            Assert.AreEqual(res[1], new int[] { 6, 7, 8 }.Sum());
        }

        [TestMethod]
        public void Collect_InitialCollectorThrows()
        {
            var scheduler = new TestScheduler();

            var xs = scheduler.CreateHotObservable(
                OnNext(100, 1),
                OnNext(200, 2),
                OnNext(300, 3),
                OnNext(400, 4),
                OnCompleted<int>(500)
            );

            var ex = new Exception();
            var ys = xs.Collect<int, int>(() => { throw ex; }, (x, y) => x + y);

            var ex_ = default(Exception);

            scheduler.ScheduleAbsolute(250, () =>
            {
                try
                {
                    ys.GetEnumerator();
                }
                catch (Exception err)
                {
                    ex_ = err;
                }
            });

            scheduler.Start();

            xs.Subscriptions.AssertEqual(
            );

            Assert.AreSame(ex_, ex);
        }

        [TestMethod]
        public void Collect_SecondCollectorThrows()
        {
            var scheduler = new TestScheduler();

            var xs = scheduler.CreateHotObservable(
                OnNext(100, 1),
                OnNext(200, 2),
                OnNext(300, 3),
                OnNext(400, 4),
                OnCompleted<int>(500)
            );

            var ex = new Exception();
            var n = 0;
            var ys = xs.Collect<int, int>(() => { if (n++ == 0) return 0; else throw ex; }, (x, y) => x + y);
            var e = default(IEnumerator<int>);

            var ex_ = default(Exception);

            scheduler.ScheduleAbsolute(250, () => e = ys.GetEnumerator());
            scheduler.ScheduleAbsolute(350, () =>
            {
                try
                {
                    e.MoveNext();
                }
                catch (Exception err)
                {
                    ex_ = err;
                }
            });

            scheduler.Start();

            xs.Subscriptions.AssertEqual(
                Subscribe(250, 350)
            );

            Assert.AreSame(ex_, ex);
        }

        [TestMethod]
        public void Collect_NewCollectorThrows()
        {
            var scheduler = new TestScheduler();

            var xs = scheduler.CreateHotObservable(
                OnNext(100, 1),
                OnNext(200, 2),
                OnNext(300, 3),
                OnNext(400, 4),
                OnCompleted<int>(500)
            );

            var ex = new Exception();
            var ys = xs.Collect<int, int>(() => 0, (x, y) => x + y, x => { throw ex; });
            var e = default(IEnumerator<int>);

            var ex_ = default(Exception);

            scheduler.ScheduleAbsolute(250, () => e = ys.GetEnumerator());
            scheduler.ScheduleAbsolute(350, () =>
            {
                try
                {
                    e.MoveNext();
                }
                catch (Exception err)
                {
                    ex_ = err;
                }
            });

            scheduler.Start();

            xs.Subscriptions.AssertEqual(
                Subscribe(250, 350)
            );

            Assert.AreSame(ex_, ex);
        }

        [TestMethod]
        public void Collect_MergeThrows()
        {
            var scheduler = new TestScheduler();

            var xs = scheduler.CreateHotObservable(
                OnNext(100, 1),
                OnNext(200, 2),
                OnNext(300, 3),
                OnNext(400, 4),
                OnCompleted<int>(500)
            );

            var ex = new Exception();
            var ys = xs.Collect<int, int>(() => 0, (x, y) => { throw ex; });
            var e = default(IEnumerator<int>);

            var ex_ = default(Exception);

            scheduler.ScheduleAbsolute(250, () => { e = ys.GetEnumerator(); });
            scheduler.ScheduleAbsolute(350, () =>
            {
                try
                {
                    e.MoveNext();
                }
                catch (Exception err)
                {
                    ex_ = err;
                }
            });

            scheduler.Start();

            xs.Subscriptions.AssertEqual(
                Subscribe(250, 300)
            );

            Assert.AreSame(ex_, ex);
        }

        #endregion

        #region First

        [TestMethod]
        public void First_ArgumentChecking()
        {
            ReactiveAssert.Throws<ArgumentNullException>(() => Observable.First(default(IObservable<int>)));
            ReactiveAssert.Throws<ArgumentNullException>(() => Observable.First(default(IObservable<int>), _ => true));
            ReactiveAssert.Throws<ArgumentNullException>(() => Observable.First(DummyObservable<int>.Instance, default(Func<int, bool>)));
        }

        [TestMethod]
        public void First_Empty()
        {
            ReactiveAssert.Throws<InvalidOperationException>(() => Observable.Empty<int>().First());
        }

        [TestMethod]
        public void FirstPredicate_Empty()
        {
            ReactiveAssert.Throws<InvalidOperationException>(() => Observable.Empty<int>().First(_ => true));
        }

        [TestMethod]
        public void First_Return()
        {
            var value = 42;
            Assert.AreEqual(value, Observable.Return<int>(value).First());
        }

        [TestMethod]
        public void FirstPredicate_Return()
        {
            var value = 42;
            Assert.AreEqual(value, Observable.Return<int>(value).First(i => i % 2 == 0));
        }

        [TestMethod]
        public void FirstPredicate_Return_NoMatch()
        {
            var value = 42;
            ReactiveAssert.Throws<InvalidOperationException>(() => Observable.Return<int>(value).First(i => i % 2 != 0));
        }

        [TestMethod]
        public void First_Throw()
        {
            var ex = new Exception();

            var xs = Observable.Throw<int>(ex);

            ReactiveAssert.Throws(ex, () => xs.First());
        }

        [TestMethod]
        public void FirstPredicate_Throw()
        {
            var ex = new Exception();

            var xs = Observable.Throw<int>(ex);

            ReactiveAssert.Throws(ex, () => xs.First(_ => true));
        }

        [TestMethod]
        public void First_Range()
        {
            var value = 42;
            Assert.AreEqual(value, Observable.Range(value, 10).First());
        }

        [TestMethod]
        public void FirstPredicate_Range()
        {
            var value = 42;
            Assert.AreEqual(46, Observable.Range(value, 10).First(i => i > 45));
        }

        #endregion

        #region FirstOrDefault

        [TestMethod]
        public void FirstOrDefault_ArgumentChecking()
        {
            ReactiveAssert.Throws<ArgumentNullException>(() => Observable.FirstOrDefault(default(IObservable<int>)));
            ReactiveAssert.Throws<ArgumentNullException>(() => Observable.FirstOrDefault(default(IObservable<int>), _ => true));
            ReactiveAssert.Throws<ArgumentNullException>(() => Observable.FirstOrDefault(DummyObservable<int>.Instance, default(Func<int, bool>)));
        }

        [TestMethod]
        public void FirstOrDefault_Empty()
        {
            Assert.AreEqual(default(int), Observable.Empty<int>().FirstOrDefault());
        }

        [TestMethod]
        public void FirstOrDefaultPredicate_Empty()
        {
            Assert.AreEqual(default(int), Observable.Empty<int>().FirstOrDefault(_ => true));
        }

        [TestMethod]
        public void FirstOrDefault_Return()
        {
            var value = 42;
            Assert.AreEqual(value, Observable.Return<int>(value).FirstOrDefault());
        }

        [TestMethod]
        public void FirstOrDefault_Throw()
        {
            var ex = new Exception();

            var xs = Observable.Throw<int>(ex);

            ReactiveAssert.Throws(ex, () => xs.FirstOrDefault());
        }

        [TestMethod]
        public void FirstOrDefault_Range()
        {
            var value = 42;
            Assert.AreEqual(value, Observable.Range(value, 10).FirstOrDefault());
        }

        #endregion

        #region + ForEach +

        [TestMethod]
        public void ForEach_ArgumentChecking()
        {
            var someObservable = Observable.Empty<int>();

            ReactiveAssert.Throws<ArgumentNullException>(() => Observable.ForEach(default(IObservable<int>), x => { }));
            ReactiveAssert.Throws<ArgumentNullException>(() => Observable.ForEach(someObservable, default(Action<int>)));

            ReactiveAssert.Throws<ArgumentNullException>(() => Observable.ForEach(default(IObservable<int>), (x, i) => { }));
            ReactiveAssert.Throws<ArgumentNullException>(() => Observable.ForEach(someObservable, default(Action<int, int>)));
        }

        [TestMethod]
        public void ForEach_Empty()
        {
            var lst = new List<int>();
            Observable.Empty<int>().ForEach(x => lst.Add(x));
            Assert.IsTrue(lst.SequenceEqual(Enumerable.Empty<int>()));
        }

        [TestMethod]
        public void ForEach_Index_Empty()
        {
            var lstX = new List<int>();
            Observable.Empty<int>().ForEach((x, i) => lstX.Add(x));
            Assert.IsTrue(lstX.SequenceEqual(Enumerable.Empty<int>()));
        }

        [TestMethod]
        public void ForEach_Return()
        {
            var lst = new List<int>();
            Observable.Return(42).ForEach(x => lst.Add(x));
            Assert.IsTrue(lst.SequenceEqual(new[] { 42 }));
        }

        [TestMethod]
        public void ForEach_Index_Return()
        {
            var lstX = new List<int>();
            var lstI = new List<int>();
            Observable.Return(42).ForEach((x, i) => { lstX.Add(x); lstI.Add(i); });
            Assert.IsTrue(lstX.SequenceEqual(new[] { 42 }));
            Assert.IsTrue(lstI.SequenceEqual(new[] { 0 }));
        }

        [TestMethod]
        public void ForEach_Throw()
        {
            var ex = new Exception();

            var xs = Observable.Throw<int>(ex);

            ReactiveAssert.Throws(ex, () => xs.ForEach(x => { Assert.Fail(); }));
        }

        [TestMethod]
        public void ForEach_Index_Throw()
        {
            var ex = new Exception();

            var xs = Observable.Throw<int>(ex);

            ReactiveAssert.Throws(ex, () => xs.ForEach((x, i) => { Assert.Fail(); }));
        }

        [TestMethod]
        public void ForEach_SomeData()
        {
            var lstX = new List<int>();
            Observable.Range(10, 10).ForEach(x => lstX.Add(x));
            Assert.IsTrue(lstX.SequenceEqual(Enumerable.Range(10, 10)));
        }

        [TestMethod]
        public void ForEach_Index_SomeData()
        {
            var lstX = new List<int>();
            var lstI = new List<int>();
            Observable.Range(10, 10).ForEach((x, i) => { lstX.Add(x); lstI.Add(i); });
            Assert.IsTrue(lstX.SequenceEqual(Enumerable.Range(10, 10)));
            Assert.IsTrue(lstI.SequenceEqual(Enumerable.Range(0, 10)));
        }

        [TestMethod]
        public void ForEach_OnNextThrows()
        {
            var ex = new Exception();

            var xs = Observable.Range(0, 10);

            ReactiveAssert.Throws(ex, () => xs.ForEach(x => { throw ex; }));
        }

        [TestMethod]
        public void ForEach_Index_OnNextThrows()
        {
            var ex = new Exception();

            var xs = Observable.Range(0, 10);

            ReactiveAssert.Throws(ex, () => xs.ForEach((x, i) => { throw ex; }));
        }

        #endregion

        #region + GetEnumerator +

        [TestMethod]
        public void GetEnumerator_ArgumentChecking()
        {
            ReactiveAssert.Throws<ArgumentNullException>(() => Observable.GetEnumerator(default(IObservable<int>)));
        }

        [TestMethod]
        public void GetEnumerator_Regular1()
        {
            var scheduler = new TestScheduler();

            var xs = scheduler.CreateColdObservable<int>(
                OnNext(10, 2),
                OnNext(20, 3),
                OnNext(30, 5),
                OnNext(40, 7),
                OnCompleted<int>(50)
            );

            var res = default(IEnumerator<int>);

            scheduler.ScheduleAbsolute(default(object), 100, (self, _) => { res = xs.GetEnumerator(); return Disposable.Empty; });

            var hasNext = new List<bool>();
            var vals = new List<Tuple<long, int>>();
            for (long i = 200; i <= 250; i += 10)
            {
                var t = i;
                scheduler.ScheduleAbsolute(default(object), t, (self, _) =>
                {
                    var b = res.MoveNext();
                    hasNext.Add(b);
                    if (b)
                        vals.Add(new Tuple<long, int>(scheduler.Clock, res.Current));
                    return Disposable.Empty;
                });
            }

            scheduler.Start();

            xs.Subscriptions.AssertEqual(
                Subscribe(100, 150)
            );

            Assert.AreEqual(6, hasNext.Count);
            Assert.IsTrue(hasNext.Take(4).All(_ => _));
            Assert.IsTrue(hasNext.Skip(4).All(_ => !_));

            Assert.AreEqual(4, vals.Count);
            Assert.IsTrue(vals[0].Item1 == 200 && vals[0].Item2 == 2);
            Assert.IsTrue(vals[1].Item1 == 210 && vals[1].Item2 == 3);
            Assert.IsTrue(vals[2].Item1 == 220 && vals[2].Item2 == 5);
            Assert.IsTrue(vals[3].Item1 == 230 && vals[3].Item2 == 7);
        }

        [TestMethod]
        public void GetEnumerator_Regular2()
        {
            var scheduler = new TestScheduler();

            var xs = scheduler.CreateColdObservable<int>(
                OnNext(10, 2),
                OnNext(30, 3),
                OnNext(50, 5),
                OnNext(70, 7),
                OnCompleted<int>(90)
            );

            var res = default(IEnumerator<int>);

            scheduler.ScheduleAbsolute(default(object), 100, (self, _) => { res = xs.GetEnumerator(); return Disposable.Empty; });

            var hasNext = new List<bool>();
            var vals = new List<Tuple<long, int>>();
            for (long i = 120; i <= 220; i += 20)
            {
                var t = i;
                scheduler.ScheduleAbsolute(default(object), t, (self, _) =>
                {
                    var b = res.MoveNext();
                    hasNext.Add(b);
                    if (b)
                        vals.Add(new Tuple<long, int>(scheduler.Clock, res.Current));
                    return Disposable.Empty;
                });
            }

            scheduler.Start();

            xs.Subscriptions.AssertEqual(
                Subscribe(100, 190)
            );

            Assert.AreEqual(6, hasNext.Count);
            Assert.IsTrue(hasNext.Take(4).All(_ => _));
            Assert.IsTrue(hasNext.Skip(4).All(_ => !_));

            Assert.AreEqual(4, vals.Count);
            Assert.IsTrue(vals[0].Item1 == 120 && vals[0].Item2 == 2);
            Assert.IsTrue(vals[1].Item1 == 140 && vals[1].Item2 == 3);
            Assert.IsTrue(vals[2].Item1 == 160 && vals[2].Item2 == 5);
            Assert.IsTrue(vals[3].Item1 == 180 && vals[3].Item2 == 7);
        }

        [TestMethod]
        public void GetEnumerator_Dispose()
        {
            var scheduler = new TestScheduler();

            var xs = scheduler.CreateColdObservable<int>(
                OnNext(10, 2),
                OnNext(30, 3),
                OnNext(50, 5),
                OnNext(70, 7),
                OnCompleted<int>(90)
            );

            var res = default(IEnumerator<int>);

            scheduler.ScheduleAbsolute(default(object), 100, (self, _) => { res = xs.GetEnumerator(); return Disposable.Empty; });

            scheduler.ScheduleAbsolute(default(object), 140, (self, _) =>
            {
                Assert.IsTrue(res.MoveNext());
                Assert.AreEqual(2, res.Current);

                Assert.IsTrue(res.MoveNext());
                Assert.AreEqual(3, res.Current);

                res.Dispose();

                return Disposable.Empty;
            });

            scheduler.ScheduleAbsolute(default(object), 160, (self, _) =>
            {
                ReactiveAssert.Throws<ObjectDisposedException>(() => res.MoveNext());
                return Disposable.Empty;
            });

            scheduler.Start();

            xs.Subscriptions.AssertEqual(
                Subscribe(100, 140)
            );
        }

#if DESKTOPCLR20 || SILVERLIGHTM7
        class Tuple<T1, T2>
        {
            public Tuple(T1 item1, T2 item2)
            {
                Item1 = item1;
                Item2 = item2;
            }

            public T1 Item1 { get; private set; }
            public T2 Item2 { get; private set; }
        }
#endif

        #endregion

        #region Last

        [TestMethod]
        public void Last_ArgumentChecking()
        {
            ReactiveAssert.Throws<ArgumentNullException>(() => Observable.Last(default(IObservable<int>)));
            ReactiveAssert.Throws<ArgumentNullException>(() => Observable.Last(default(IObservable<int>), _ => true));
            ReactiveAssert.Throws<ArgumentNullException>(() => Observable.Last(DummyObservable<int>.Instance, default(Func<int, bool>)));
        }

        [TestMethod]
        public void Last_Empty()
        {
            ReactiveAssert.Throws<InvalidOperationException>(() => Observable.Empty<int>().Last());
        }

        [TestMethod]
        public void LastPredicate_Empty()
        {
            ReactiveAssert.Throws<InvalidOperationException>(() => Observable.Empty<int>().Last(_ => true));
        }

        [TestMethod]
        public void Last_Return()
        {
            var value = 42;
            Assert.AreEqual(value, Observable.Return<int>(value).Last());
        }

        [TestMethod]
        public void Last_Throw()
        {
            var ex = new Exception();

            var xs = Observable.Throw<int>(ex);

            ReactiveAssert.Throws(ex, () => xs.Last());
        }

        [TestMethod]
        public void Last_Range()
        {
            var value = 42;
            Assert.AreEqual(value, Observable.Range(value - 9, 10).Last());
        }

        [TestMethod]
        public void LastPredicate_Range()
        {
            var value = 42;
            Assert.AreEqual(50, Observable.Range(value, 10).Last(i => i % 2 == 0));
        }

        #endregion

        #region LastOrDefault

        [TestMethod]
        public void LastOrDefault_ArgumentChecking()
        {
            ReactiveAssert.Throws<ArgumentNullException>(() => Observable.LastOrDefault(default(IObservable<int>)));
            ReactiveAssert.Throws<ArgumentNullException>(() => Observable.LastOrDefault(default(IObservable<int>), _ => true));
            ReactiveAssert.Throws<ArgumentNullException>(() => Observable.LastOrDefault(DummyObservable<int>.Instance, default(Func<int, bool>)));
        }

        [TestMethod]
        public void LastOrDefault_Empty()
        {
            Assert.AreEqual(default(int), Observable.Empty<int>().LastOrDefault());
        }

        [TestMethod]
        public void LastOrDefaultPredicate_Empty()
        {
            Assert.AreEqual(default(int), Observable.Empty<int>().LastOrDefault(_ => true));
        }

        [TestMethod]
        public void LastOrDefault_Return()
        {
            var value = 42;
            Assert.AreEqual(value, Observable.Return<int>(value).LastOrDefault());
        }

        [TestMethod]
        public void LastOrDefault_Throw()
        {
            var ex = new Exception();

            var xs = Observable.Throw<int>(ex);

            ReactiveAssert.Throws(ex, () => xs.LastOrDefault());
        }

        [TestMethod]
        public void LastOrDefault_Range()
        {
            var value = 42;
            Assert.AreEqual(value, Observable.Range(value - 9, 10).LastOrDefault());
        }

        [TestMethod]
        public void LastOrDefaultPredicate_Range()
        {
            var value = 42;
            Assert.AreEqual(50, Observable.Range(value, 10).LastOrDefault(i => i % 2 == 0));
        }

        #endregion

        #region + Latest +

        [TestMethod]
        public void Latest_ArgumentChecking()
        {
            ReactiveAssert.Throws<ArgumentNullException>(() => Observable.Latest(default(IObservable<int>)));
        }

        [TestMethod]
        public void Latest1()
        {
            var disposed = false;
            var evt = new AutoResetEvent(false);
            var src = Observable.Create<int>(obs =>
            {
                new Thread(() =>
                {
                    evt.WaitOne();
                    obs.OnNext(1);
                    evt.WaitOne();
                    obs.OnNext(2);
                    evt.WaitOne();
                    obs.OnCompleted();
                }).Start();

                return () => { disposed = true; };
            });

            var res = src.Latest().GetEnumerator();

            new Thread(() =>
            {
                Thread.Sleep(250);
                evt.Set();
            }).Start();

            Assert.IsTrue(res.MoveNext());
            Assert.AreEqual(1, res.Current);

            evt.Set();
            Assert.IsTrue(((IEnumerator)res).MoveNext());
            Assert.AreEqual(2, ((IEnumerator)res).Current);

            evt.Set();
            Assert.IsFalse(res.MoveNext());

            ReactiveAssert.Throws<NotSupportedException>(() => res.Reset());

            res.Dispose();
            //ReactiveAssert.Throws<ObjectDisposedException>(() => res.MoveNext());
            Assert.IsTrue(disposed);
        }

        [TestMethod]
        public void Latest2()
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

            var res = xs.Latest();

            var e1 = default(IEnumerator<int>);
            scheduler.ScheduleAbsolute(205, () =>
            {
                e1 = res.GetEnumerator();
            });

            var o1 = new List<int>();
            scheduler.ScheduleAbsolute(235, () =>
            {
                Assert.IsTrue(e1.MoveNext());
                o1.Add(e1.Current);
            });
            scheduler.ScheduleAbsolute(265, () =>
            {
                Assert.IsTrue(e1.MoveNext());
                o1.Add(e1.Current);
            });

            scheduler.ScheduleAbsolute(285, () => e1.Dispose());

            var e2 = default(IEnumerator);
            scheduler.ScheduleAbsolute(255, () =>
            {
                e2 = ((IEnumerable)res).GetEnumerator();
            });

            var o2 = new List<int>();
            scheduler.ScheduleAbsolute(265, () =>
            {
                Assert.IsTrue(e2.MoveNext());
                o2.Add((int)e2.Current);
            });
            scheduler.ScheduleAbsolute(275, () =>
            {
                Assert.IsTrue(e2.MoveNext());
                o2.Add((int)e2.Current);
            });

            scheduler.Start();

            xs.Subscriptions.AssertEqual(
                Subscribe(205, 285),
                Subscribe(255, 300)
            );

            o1.AssertEqual(3, 6);
            o2.AssertEqual(6, 7);
        }

        [TestMethod]
        public void Latest_Error()
        {
            var ex = new Exception();

            var evt = new AutoResetEvent(false);
            var src = Observable.Create<int>(obs =>
            {
                new Thread(() =>
                {
                    evt.WaitOne();
                    obs.OnNext(1);
                    evt.WaitOne();
                    obs.OnError(ex);
                }).Start();

                return () => { };
            });

            var res = src.Latest().GetEnumerator();

            new Thread(() =>
            {
                Thread.Sleep(250);
                evt.Set();
            }).Start();

            Assert.IsTrue(res.MoveNext());
            Assert.AreEqual(1, res.Current);

            evt.Set();

            ReactiveAssert.Throws(ex, () => res.MoveNext());
        }

        #endregion

        #region + MostRecent +

        [TestMethod]
        public void MostRecent_ArgumentChecking()
        {
            ReactiveAssert.Throws<ArgumentNullException>(() => Observable.MostRecent(default(IObservable<int>), 1));
        }

        [TestMethod]
        public void MostRecent1()
        {
            var evt = new AutoResetEvent(false);
            var nxt = new AutoResetEvent(false);
            var src = Observable.Create<int>(obs =>
            {
                new Thread(() =>
                {
                    evt.WaitOne();
                    obs.OnNext(1);
                    nxt.Set();
                    evt.WaitOne();
                    obs.OnNext(2);
                    nxt.Set();
                    evt.WaitOne();
                    obs.OnCompleted();
                    nxt.Set();
                }).Start();

                return () => { };
            });

            var res = src.MostRecent(42).GetEnumerator();

            Assert.IsTrue(res.MoveNext());
            Assert.AreEqual(42, res.Current);
            Assert.IsTrue(res.MoveNext());
            Assert.AreEqual(42, res.Current);

            for (int i = 1; i <= 2; i++)
            {
                evt.Set();
                nxt.WaitOne();
                Assert.IsTrue(res.MoveNext());
                Assert.AreEqual(i, res.Current);
                Assert.IsTrue(res.MoveNext());
                Assert.AreEqual(i, res.Current);
            }

            evt.Set();
            nxt.WaitOne();
            Assert.IsFalse(res.MoveNext());
        }

        [TestMethod]
        public void MostRecent2()
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

            var res = xs.MostRecent(0);

            var e1 = default(IEnumerator<int>);
            scheduler.ScheduleAbsolute(200, () =>
            {
                e1 = res.GetEnumerator();
            });

            var o1 = new List<int>();
            scheduler.ScheduleAbsolute(205, () =>
            {
                Assert.IsTrue(e1.MoveNext());
                o1.Add(e1.Current);
            });
            scheduler.ScheduleAbsolute(232, () =>
            {
                Assert.IsTrue(e1.MoveNext());
                o1.Add(e1.Current);
            });
            scheduler.ScheduleAbsolute(234, () =>
            {
                Assert.IsTrue(e1.MoveNext());
                o1.Add(e1.Current);
            });
            scheduler.ScheduleAbsolute(265, () =>
            {
                Assert.IsTrue(e1.MoveNext());
                o1.Add(e1.Current);
            });

            scheduler.ScheduleAbsolute(285, () => e1.Dispose());

            var e2 = default(IEnumerator);
            scheduler.ScheduleAbsolute(255, () =>
            {
                e2 = ((IEnumerable)res).GetEnumerator();
            });

            var o2 = new List<int>();
            scheduler.ScheduleAbsolute(258, () =>
            {
                Assert.IsTrue(e2.MoveNext());
                o2.Add((int)e2.Current);
            });
            scheduler.ScheduleAbsolute(262, () =>
            {
                Assert.IsTrue(e2.MoveNext());
                o2.Add((int)e2.Current);
            });
            scheduler.ScheduleAbsolute(264, () =>
            {
                Assert.IsTrue(e2.MoveNext());
                o2.Add((int)e2.Current);
            });
            scheduler.ScheduleAbsolute(275, () =>
            {
                Assert.IsTrue(e2.MoveNext());
                o2.Add((int)e2.Current);
            });

            scheduler.Start();

            xs.Subscriptions.AssertEqual(
                Subscribe(200, 285),
                Subscribe(255, 300)
            );

            o1.AssertEqual(0, 3, 3, 6);
            o2.AssertEqual(0, 6, 6, 7);
        }

        [TestMethod]
        public void MostRecent_Error()
        {
            var ex = new Exception();

            var evt = new AutoResetEvent(false);
            var nxt = new AutoResetEvent(false);
            var src = Observable.Create<int>(obs =>
            {
                new Thread(() =>
                {
                    evt.WaitOne();
                    obs.OnNext(1);
                    nxt.Set();
                    evt.WaitOne();
                    obs.OnError(ex);
                    nxt.Set();
                }).Start();

                return () => { };
            });

            var res = src.MostRecent(42).GetEnumerator();

            Assert.IsTrue(res.MoveNext());
            Assert.AreEqual(42, res.Current);
            Assert.IsTrue(res.MoveNext());
            Assert.AreEqual(42, res.Current);

            evt.Set();
            nxt.WaitOne();
            Assert.IsTrue(res.MoveNext());
            Assert.AreEqual(1, res.Current);
            Assert.IsTrue(res.MoveNext());
            Assert.AreEqual(1, res.Current);

            evt.Set();
            nxt.WaitOne();

            ReactiveAssert.Throws(ex, () => res.MoveNext());
        }

        #endregion

        #region + Next +

        [TestMethod]
        public void Next_ArgumentChecking()
        {
            ReactiveAssert.Throws<ArgumentNullException>(() => Observable.Next(default(IObservable<int>)));
        }

        [TestMethod]
        public void Next1()
        {
            var evt = new AutoResetEvent(false);
            var src = Observable.Create<int>(obs =>
            {
                new Thread(() =>
                {
                    evt.WaitOne();
                    obs.OnNext(1);
                    evt.WaitOne();
                    obs.OnNext(2);
                    evt.WaitOne();
                    obs.OnCompleted();
                }).Start();

                return () => { };
            });

            var res = src.Next().GetEnumerator();

            Action release = () => new Thread(() =>
            {
                Thread.Sleep(250);
                evt.Set();
            }).Start();

            release();
            Assert.IsTrue(res.MoveNext());
            Assert.AreEqual(1, res.Current);

            release();
            Assert.IsTrue(res.MoveNext());
            Assert.AreEqual(2, res.Current);

            release();
            Assert.IsFalse(res.MoveNext());
        }

        [TestMethod]
        public void Next2()
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

            var res = xs.Next();

            var e1 = default(IEnumerator<int>);
            scheduler.ScheduleAbsolute(200, () =>
            {
                e1 = res.GetEnumerator();
            });

            scheduler.ScheduleAbsolute(285, () => e1.Dispose());

            var e2 = default(IEnumerator);
            scheduler.ScheduleAbsolute(255, () =>
            {
                e2 = ((IEnumerable)res).GetEnumerator();
            });

            scheduler.Start();

            xs.Subscriptions.AssertEqual(
                Subscribe(200, 285),
                Subscribe(255, 300)
            );
        }

        [TestMethod]
        public void Next_DoesNotBlock()
        {
            var evt = new ManualResetEvent(false);

            var xs = Observable.Empty<int>().Do(_ => { }, () => evt.Set());

            var e = xs.Next().GetEnumerator();

            evt.WaitOne();

            Assert.IsFalse(e.MoveNext());
        }

        [TestMethod]
        public void Next_SomeResults()
        {
            var xs = Observable.Range(0, 100, Scheduler.Default);

            var res = xs.Next().ToList();

            Assert.IsTrue(res.All(x => x < 100));
            Assert.IsTrue(res.Count == res.Distinct().Count());
        }

        [TestMethod]
        public void Next_Error()
        {
            var ex = new Exception();

            var evt = new AutoResetEvent(false);
            var src = Observable.Create<int>(obs =>
            {
                new Thread(() =>
                {
                    evt.WaitOne();
                    obs.OnNext(1);
                    evt.WaitOne();
                    obs.OnError(ex);
                }).Start();

                return () => { };
            });

            var res = src.Next().GetEnumerator();

            Action release = () => new Thread(() =>
            {
                Thread.Sleep(250);
                evt.Set();
            }).Start();

            release();
            Assert.IsTrue(res.MoveNext());
            Assert.AreEqual(1, res.Current);

            release();

            ReactiveAssert.Throws(ex, () => res.MoveNext());
        }

        #endregion

        #region Single

        [TestMethod]
        public void Single_ArgumentChecking()
        {
            ReactiveAssert.Throws<ArgumentNullException>(() => Observable.Single(default(IObservable<int>)));
            ReactiveAssert.Throws<ArgumentNullException>(() => Observable.Single(default(IObservable<int>), _ => true));
            ReactiveAssert.Throws<ArgumentNullException>(() => Observable.Single(DummyObservable<int>.Instance, default(Func<int, bool>)));
        }

        [TestMethod]
        public void Single_Empty()
        {
            ReactiveAssert.Throws<InvalidOperationException>(() => Observable.Empty<int>().Single());
        }

        [TestMethod]
        public void SinglePredicate_Empty()
        {
            ReactiveAssert.Throws<InvalidOperationException>(() => Observable.Empty<int>().Single(_ => true));
        }

        [TestMethod]
        public void Single_Return()
        {
            var value = 42;
            Assert.AreEqual(value, Observable.Return<int>(value).Single());
        }

        [TestMethod]
        public void Single_Throw()
        {
            var ex = new Exception();

            var xs = Observable.Throw<int>(ex);

            ReactiveAssert.Throws(ex, () => xs.Single());
        }

        [TestMethod]
        public void Single_Range()
        {
            var value = 42;
            ReactiveAssert.Throws<InvalidOperationException>(() => Observable.Range(value, 10).Single());
        }

        [TestMethod]
        public void SinglePredicate_Range()
        {
            var value = 42;
            ReactiveAssert.Throws<InvalidOperationException>(() => Observable.Range(value, 10).Single(i => i % 2 == 0));
        }

        [TestMethod]
        public void SinglePredicate_Range_ReducesToSingle()
        {
            var value = 42;
            Assert.AreEqual(45, Observable.Range(value, 10).Single(i => i == 45));
        }

        #endregion

        #region SingleOrDefault

        [TestMethod]
        public void SingleOrDefault_ArgumentChecking()
        {
            ReactiveAssert.Throws<ArgumentNullException>(() => Observable.SingleOrDefault(default(IObservable<int>)));
            ReactiveAssert.Throws<ArgumentNullException>(() => Observable.SingleOrDefault(default(IObservable<int>), _ => true));
            ReactiveAssert.Throws<ArgumentNullException>(() => Observable.SingleOrDefault(DummyObservable<int>.Instance, default(Func<int, bool>)));
        }

        [TestMethod]
        public void SingleOrDefault_Empty()
        {
            Assert.AreEqual(default(int), Observable.Empty<int>().SingleOrDefault());
        }

        [TestMethod]
        public void SingleOrDefaultPredicate_Empty()
        {
            Assert.AreEqual(default(int), Observable.Empty<int>().SingleOrDefault(_ => true));
        }

        [TestMethod]
        public void SingleOrDefault_Return()
        {
            var value = 42;
            Assert.AreEqual(value, Observable.Return<int>(value).SingleOrDefault());
        }

        [TestMethod]
        public void SingleOrDefault_Throw()
        {
            var ex = new Exception();

            var xs = Observable.Throw<int>(ex);

            ReactiveAssert.Throws(ex, () => xs.SingleOrDefault());
        }

        [TestMethod]
        public void SingleOrDefault_Range()
        {
            var value = 42;
            ReactiveAssert.Throws<InvalidOperationException>(() => Observable.Range(value, 10).SingleOrDefault());
        }

        [TestMethod]
        public void SingleOrDefaultPredicate_Range()
        {
            var value = 42;
            ReactiveAssert.Throws<InvalidOperationException>(() => Observable.Range(value, 10).SingleOrDefault(i => i % 2 == 0));
        }

        [TestMethod]
        public void SingleOrDefault_Range_ReducesToSingle()
        {
            var value = 42;
            Assert.AreEqual(45, Observable.Range(value, 10).SingleOrDefault(i => i == 45));
        }

        [TestMethod]
        public void SingleOrDefault_Range_ReducesToNone()
        {
            var value = 42;
            Assert.AreEqual(0, Observable.Range(value, 10).SingleOrDefault(i => i > 100));
        }

        #endregion

        #region Wait

        [TestMethod]
        public void Wait_ArgumentChecking()
        {
            ReactiveAssert.Throws<ArgumentNullException>(() => Observable.Wait(default(IObservable<int>)));
        }

        [TestMethod]
        public void Wait_Return()
        {
            var x = 42;
            var xs = Observable.Return(x, ThreadPoolScheduler.Instance);
            var res = xs.Wait();
            Assert.AreEqual(x, res);
        }

        [TestMethod]
        public void Wait_Empty()
        {
            ReactiveAssert.Throws<InvalidOperationException>(() => Observable.Empty<int>().Wait());
        }

        [TestMethod]
        public void Wait_Throw()
        {
            var ex = new Exception();

            var xs = Observable.Throw<int>(ex);

            ReactiveAssert.Throws(ex, () => xs.Wait());
        }

        [TestMethod]
        public void Wait_Range()
        {
            var n = 42;
            var xs = Observable.Range(1, n, ThreadPoolScheduler.Instance);
            var res = xs.Wait();
            Assert.AreEqual(n, res);
        }

        #endregion
    }
}

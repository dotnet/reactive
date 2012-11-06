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
    public class ObservableExTest : ReactiveTest
    {
        #region Create

        [TestMethod]
        public void Iterate_ArgumentChecking()
        {
            ReactiveAssert.Throws<ArgumentNullException>(() => ObservableEx.Create<int>(default(Func<IObserver<int>, IEnumerable<IObservable<Object>>>)));
            ReactiveAssert.Throws<ArgumentNullException>(() => ObservableEx.Create(DummyFunc<IObserver<int>, IEnumerable<IObservable<Object>>>.Instance).Subscribe(null));
        }

        IEnumerable<IObservable<Object>> ToIterate_Complete(IObservable<int> xs, IObservable<int> ys, IObservable<int> zs, IObserver<int> observer)
        {
            observer.OnNext(1);
            yield return xs.Select(x => new Object());

            observer.OnNext(2);
            yield return ys.Select(x => new Object());

            observer.OnNext(3);
            observer.OnCompleted();
            yield return zs.Select(x => new Object());

            observer.OnNext(4);
        }

        [TestMethod]
        public void Iterate_Complete()
        {
            var scheduler = new TestScheduler();

            var xs = scheduler.CreateColdObservable(
                OnNext(10, 1),
                OnNext(20, 2),
                OnNext(30, 3),
                OnNext(40, 4),
                OnCompleted<int>(50)
            );

            var ys = scheduler.CreateColdObservable(
                OnNext(10, 1),
                OnNext(20, 2),
                OnCompleted<int>(30)
            );

            var zs = scheduler.CreateColdObservable(
                OnNext(10, 1),
                OnNext(20, 2),
                OnNext(30, 3),
                OnNext(40, 4),
                OnNext(50, 5),
                OnCompleted<int>(60)
            );

            var res = scheduler.Start(() => ObservableEx.Create<int>(observer => ToIterate_Complete(xs, ys, zs, observer)));

            res.Messages.AssertEqual(
                OnNext(200, 1),
                OnNext(250, 2),
                OnNext(280, 3),
                OnCompleted<int>(280)
            );

            xs.Subscriptions.AssertEqual(
                Subscribe(200, 250)
            );

            ys.Subscriptions.AssertEqual(
                Subscribe(250, 280)
            );

            zs.Subscriptions.AssertEqual(
                Subscribe(280, 280)
            );
        }

        IEnumerable<IObservable<Object>> ToIterate_Complete_Implicit(IObservable<int> xs, IObservable<int> ys, IObservable<int> zs, IObserver<int> observer)
        {
            observer.OnNext(1);
            yield return xs.Select(x => new Object());

            observer.OnNext(2);
            yield return ys.Select(x => new Object());

            observer.OnNext(3);
            yield return zs.Select(x => new Object());

            observer.OnNext(4);
        }

        [TestMethod]
        public void Iterate_Complete_Implicit()
        {
            var scheduler = new TestScheduler();

            var xs = scheduler.CreateColdObservable(
                OnNext(10, 1),
                OnNext(20, 2),
                OnNext(30, 3),
                OnNext(40, 4),
                OnCompleted<int>(50)
            );

            var ys = scheduler.CreateColdObservable(
                OnNext(10, 1),
                OnNext(20, 2),
                OnCompleted<int>(30)
            );

            var zs = scheduler.CreateColdObservable(
                OnNext(10, 1),
                OnNext(20, 2),
                OnNext(30, 3),
                OnNext(40, 4),
                OnNext(50, 5),
                OnCompleted<int>(60)
            );

            var res = scheduler.Start(() => ObservableEx.Create<int>(observer => ToIterate_Complete_Implicit(xs, ys, zs, observer)));

            res.Messages.AssertEqual(
                OnNext(200, 1),
                OnNext(250, 2),
                OnNext(280, 3),
                OnNext(340, 4),
                OnCompleted<int>(340)
            );

            xs.Subscriptions.AssertEqual(
                Subscribe(200, 250)
            );

            ys.Subscriptions.AssertEqual(
                Subscribe(250, 280)
            );

            zs.Subscriptions.AssertEqual(
                Subscribe(280, 340)
            );
        }

        IEnumerable<IObservable<Object>> ToIterate_Throw(IObservable<int> xs, IObservable<int> ys, IObservable<int> zs, IObserver<int> observer, Exception ex)
        {
            observer.OnNext(1);
            yield return xs.Select(x => new Object());

            observer.OnNext(2);
            yield return ys.Select(x => new Object());

            observer.OnNext(3);

            if (xs != null)
                throw ex;

            yield return zs.Select(x => new Object());

            observer.OnNext(4);
            observer.OnCompleted();
        }

        [TestMethod]
        public void Iterate_Iterator_Throw()
        {
            var scheduler = new TestScheduler();

            var xs = scheduler.CreateColdObservable(
                OnNext(10, 1),
                OnNext(20, 2),
                OnNext(30, 3),
                OnNext(40, 4),
                OnCompleted<int>(50)
            );

            var ys = scheduler.CreateColdObservable(
                OnNext(10, 1),
                OnNext(20, 2),
                OnCompleted<int>(30)
            );

            var zs = scheduler.CreateColdObservable(
                OnNext(10, 1),
                OnNext(20, 2),
                OnNext(30, 3),
                OnNext(40, 4),
                OnNext(50, 5),
                OnCompleted<int>(60)
            );

            var ex = new Exception();

            var res = scheduler.Start(() => ObservableEx.Create<int>(observer => ToIterate_Throw(xs, ys, zs, observer, ex)));

            res.Messages.AssertEqual(
                OnNext(200, 1),
                OnNext(250, 2),
                OnNext(280, 3),
                OnError<int>(280, ex)
            );

            xs.Subscriptions.AssertEqual(
                Subscribe(200, 250)
            );

            ys.Subscriptions.AssertEqual(
                Subscribe(250, 280)
            );

            zs.Subscriptions.AssertEqual(
            );
        }

        IEnumerable<IObservable<Object>> ToIterate_Error(IObservable<int> xs, IObservable<int> ys, IObservable<int> zs, IObserver<int> observer, Exception ex)
        {
            observer.OnNext(1);
            yield return xs.Select(x => new Object());

            observer.OnNext(2);
            observer.OnError(ex);

            yield return ys.Select(x => new Object());

            observer.OnNext(3);

            yield return zs.Select(x => new Object());

            observer.OnNext(4);
            observer.OnCompleted();
        }

        [TestMethod]
        public void Iterate_Iterator_Error()
        {
            var scheduler = new TestScheduler();

            var ex = new Exception();

            var xs = scheduler.CreateColdObservable(
                OnNext(10, 1),
                OnNext(20, 2),
                OnNext(30, 3),
                OnNext(40, 4),
                OnCompleted<int>(50)
            );

            var ys = scheduler.CreateColdObservable(
                OnNext(10, 1),
                OnNext(20, 2),
                OnCompleted<int>(30)
            );

            var zs = scheduler.CreateColdObservable(
                OnNext(10, 1),
                OnNext(20, 2),
                OnNext(30, 3),
                OnNext(40, 4),
                OnNext(50, 5),
                OnCompleted<int>(60)
            );

            var res = scheduler.Start(() => ObservableEx.Create<int>(observer => ToIterate_Error(xs, ys, zs, observer, ex)));

            res.Messages.AssertEqual(
                OnNext(200, 1),
                OnNext(250, 2),
                OnError<int>(250, ex)
            );

            xs.Subscriptions.AssertEqual(
                Subscribe(200, 250)
            );

            ys.Subscriptions.AssertEqual(
                Subscribe(250, 250)
            );

            zs.Subscriptions.AssertEqual(
            );
        }

        IEnumerable<IObservable<Object>> ToIterate_Complete_Dispose(IObservable<int> xs, IObservable<int> ys, IObservable<int> zs, IObserver<int> observer)
        {
            observer.OnNext(1);
            yield return xs.Select(x => new Object());

            observer.OnNext(2);
            yield return ys.Select(x => new Object());

            observer.OnNext(3);
            yield return zs.Select(x => new Object());

            observer.OnNext(4);
        }

        [TestMethod]
        public void Iterate_Complete_Dispose()
        {
            var scheduler = new TestScheduler();

            var xs = scheduler.CreateColdObservable(
                OnNext(10, 1),
                OnNext(20, 2),
                OnNext(30, 3),
                OnNext(40, 4),
                OnCompleted<int>(50)
            );

            var ys = scheduler.CreateColdObservable(
                OnNext(10, 1),
                OnNext(20, 2),
                OnCompleted<int>(30)
            );

            var zs = scheduler.CreateColdObservable(
                OnNext(100, 1),
                OnNext(200, 2),
                OnNext(300, 3),
                OnNext(400, 4),
                OnNext(500, 5),
                OnNext(600, 6),
                OnNext(700, 7),
                OnNext(800, 8),
                OnNext(900, 9),
                OnNext(1000, 10)
            );

            var res = scheduler.Start(() => ObservableEx.Create<int>(observer => ToIterate_Complete_Dispose(xs, ys, zs, observer)));

            res.Messages.AssertEqual(
                OnNext(200, 1),
                OnNext(250, 2),
                OnNext(280, 3)
            );

            xs.Subscriptions.AssertEqual(
                Subscribe(200, 250)
            );

            ys.Subscriptions.AssertEqual(
                Subscribe(250, 280)
            );

            zs.Subscriptions.AssertEqual(
                Subscribe(280, 1000)
            );
        }

        [TestMethod]
        public void IteratorScenario()
        {
            var xs = ObservableEx.Create<int>(o => _IteratorScenario(100, 1000, o));

            xs.AssertEqual(new[] { 100, 1000 }.ToObservable());
        }

        static IEnumerable<IObservable<Object>> _IteratorScenario(int x, int y, IObserver<int> results)
        {
            var xs = Observable.Range(1, x).ToListObservable();
            yield return xs;

            results.OnNext(xs.Value);

            var ys = Observable.Range(1, y).ToListObservable();
            yield return ys;

            results.OnNext(ys.Value);
        }

        [TestMethod]
        public void Iterate_Void_ArgumentChecking()
        {
            ReactiveAssert.Throws<ArgumentNullException>(() => ObservableEx.Create(default(Func<IEnumerable<IObservable<object>>>)));
            ReactiveAssert.Throws<ArgumentNullException>(() => ObservableEx.Create(DummyFunc<IEnumerable<IObservable<Object>>>.Instance).Subscribe(null));
        }

        IEnumerable<IObservable<Object>> ToIterate_Void_Complete(IObservable<int> xs, IObservable<int> ys, IObservable<int> zs)
        {
            yield return xs.Select(x => new Object());

            yield return ys.Select(x => new Object());

            yield return zs.Select(x => new Object());
        }

        [TestMethod]
        public void Iterate_Void_Complete()
        {
            var scheduler = new TestScheduler();

            var xs = scheduler.CreateColdObservable(
                OnNext(10, 1),
                OnNext(20, 2),
                OnNext(30, 3),
                OnNext(40, 4),
                OnCompleted<int>(50)
            );

            var ys = scheduler.CreateColdObservable(
                OnNext(10, 1),
                OnNext(20, 2),
                OnCompleted<int>(30)
            );

            var zs = scheduler.CreateColdObservable(
                OnNext(10, 1),
                OnNext(20, 2),
                OnNext(30, 3),
                OnNext(40, 4),
                OnNext(50, 5),
                OnCompleted<int>(60)
            );

            var res = scheduler.Start(() => ObservableEx.Create(() => ToIterate_Void_Complete(xs, ys, zs)));

            res.Messages.AssertEqual(
                OnCompleted<Unit>(340)
            );

            xs.Subscriptions.AssertEqual(
                Subscribe(200, 250)
            );

            ys.Subscriptions.AssertEqual(
                Subscribe(250, 280)
            );

            zs.Subscriptions.AssertEqual(
                Subscribe(280, 340)
            );
        }

        IEnumerable<IObservable<Object>> ToIterate_Void_Complete_Implicit(IObservable<int> xs, IObservable<int> ys, IObservable<int> zs)
        {
            yield return xs.Select(x => new Object());

            yield return ys.Select(x => new Object());

            yield return zs.Select(x => new Object());
        }

        [TestMethod]
        public void Iterate_Void_Complete_Implicit()
        {
            var scheduler = new TestScheduler();

            var xs = scheduler.CreateColdObservable(
                OnNext(10, 1),
                OnNext(20, 2),
                OnNext(30, 3),
                OnNext(40, 4),
                OnCompleted<int>(50)
            );

            var ys = scheduler.CreateColdObservable(
                OnNext(10, 1),
                OnNext(20, 2),
                OnCompleted<int>(30)
            );

            var zs = scheduler.CreateColdObservable(
                OnNext(10, 1),
                OnNext(20, 2),
                OnNext(30, 3),
                OnNext(40, 4),
                OnNext(50, 5),
                OnCompleted<int>(60)
            );

            var res = scheduler.Start(() => ObservableEx.Create(() => ToIterate_Void_Complete_Implicit(xs, ys, zs)));

            res.Messages.AssertEqual(
                OnCompleted<Unit>(340)
            );

            xs.Subscriptions.AssertEqual(
                Subscribe(200, 250)
            );

            ys.Subscriptions.AssertEqual(
                Subscribe(250, 280)
            );

            zs.Subscriptions.AssertEqual(
                Subscribe(280, 340)
            );
        }

        IEnumerable<IObservable<Object>> ToIterate_Void_Throw(IObservable<int> xs, IObservable<int> ys, IObservable<int> zs, Exception ex)
        {
            yield return xs.Select(x => new Object());

            yield return ys.Select(x => new Object());

            if (xs != null)
                throw ex;

            yield return zs.Select(x => new Object());
        }

        [TestMethod]
        public void Iterate_Void_Iterator_Throw()
        {
            var scheduler = new TestScheduler();

            var xs = scheduler.CreateColdObservable(
                OnNext(10, 1),
                OnNext(20, 2),
                OnNext(30, 3),
                OnNext(40, 4),
                OnCompleted<int>(50)
            );

            var ys = scheduler.CreateColdObservable(
                OnNext(10, 1),
                OnNext(20, 2),
                OnCompleted<int>(30)
            );

            var zs = scheduler.CreateColdObservable(
                OnNext(10, 1),
                OnNext(20, 2),
                OnNext(30, 3),
                OnNext(40, 4),
                OnNext(50, 5),
                OnCompleted<int>(60)
            );

            var ex = new Exception();

            var res = scheduler.Start(() => ObservableEx.Create(() => ToIterate_Void_Throw(xs, ys, zs, ex)));

            res.Messages.AssertEqual(
                OnError<Unit>(280, ex)
            );

            xs.Subscriptions.AssertEqual(
                Subscribe(200, 250)
            );

            ys.Subscriptions.AssertEqual(
                Subscribe(250, 280)
            );

            zs.Subscriptions.AssertEqual(
            );
        }

        IEnumerable<IObservable<Object>> ToIterate_Void_Complete_Dispose(IObservable<int> xs, IObservable<int> ys, IObservable<int> zs)
        {
            yield return xs.Select(x => new Object());

            yield return ys.Select(x => new Object());

            yield return zs.Select(x => new Object());
        }

        [TestMethod]
        public void Iterate_Void_Complete_Dispose()
        {
            var scheduler = new TestScheduler();

            var xs = scheduler.CreateColdObservable(
                OnNext(10, 1),
                OnNext(20, 2),
                OnNext(30, 3),
                OnNext(40, 4),
                OnCompleted<int>(50)
            );

            var ys = scheduler.CreateColdObservable(
                OnNext(10, 1),
                OnNext(20, 2),
                OnCompleted<int>(30)
            );

            var zs = scheduler.CreateColdObservable(
                OnNext(100, 1),
                OnNext(200, 2),
                OnNext(300, 3),
                OnNext(400, 4),
                OnNext(500, 5),
                OnNext(600, 6),
                OnNext(700, 7),
                OnNext(800, 8),
                OnNext(900, 9),
                OnNext(1000, 10)
            );

            var res = scheduler.Start(() => ObservableEx.Create(() => ToIterate_Void_Complete_Dispose(xs, ys, zs)));

            res.Messages.AssertEqual(
            );

            xs.Subscriptions.AssertEqual(
                Subscribe(200, 250)
            );

            ys.Subscriptions.AssertEqual(
                Subscribe(250, 280)
            );

            zs.Subscriptions.AssertEqual(
                Subscribe(280, 1000)
            );
        }

        [TestMethod]
        [Ignore]
        public void Iterate_Void_Func_Throw()
        {
            var scheduler = new TestScheduler();

            ReactiveAssert.Throws<InvalidOperationException>(() => scheduler.Start(() => ObservableEx.Create(() => { throw new InvalidOperationException(); })));
        }

        static IEnumerable<IObservable<Object>> _IteratorScenario_Void(int x, int y)
        {
            var xs = Observable.Range(1, x).ToListObservable();
            yield return xs;

            var ys = Observable.Range(1, y).ToListObservable();
            yield return ys;
        }

        [TestMethod]
        public void IteratorScenario_Void()
        {
            var xs = ObservableEx.Create(() => _IteratorScenario_Void(100, 1000));

            xs.AssertEqual(new Unit[] { }.ToObservable());
        }

        #endregion

        #region Expand

        [TestMethod]
        public void Expand_ArgumentChecking()
        {
            ReactiveAssert.Throws<ArgumentNullException>(() => ObservableEx.Expand(null, DummyFunc<int, IObservable<int>>.Instance, DummyScheduler.Instance));
            ReactiveAssert.Throws<ArgumentNullException>(() => ObservableEx.Expand(DummyObservable<int>.Instance, null, DummyScheduler.Instance));
            ReactiveAssert.Throws<ArgumentNullException>(() => ObservableEx.Expand(DummyObservable<int>.Instance, DummyFunc<int, IObservable<int>>.Instance, null));
            ReactiveAssert.Throws<ArgumentNullException>(() => ObservableEx.Expand(null, DummyFunc<int, IObservable<int>>.Instance));
            ReactiveAssert.Throws<ArgumentNullException>(() => ObservableEx.Expand(DummyObservable<int>.Instance, null));
        }

        [TestMethod]
        public void Expand_Default()
        {
            var b = Observable.Return(1).Expand(x => x < 10 ? Observable.Return(x + 1) : Observable.Empty<int>())
                        .SequenceEqual(Observable.Range(1, 10)).First();

            Assert.IsTrue(b);
        }

        [TestMethod]
        public void Expand_Empty()
        {
            var scheduler = new TestScheduler();

            var xs = scheduler.CreateHotObservable(
                OnCompleted<int>(300)
            );

            var res = scheduler.Start(() =>
                xs.Expand(x => scheduler.CreateColdObservable(
                    OnNext(100, 1),
                    OnNext(200, 2),
                    OnCompleted<int>(300)
                ), scheduler)
            );

            res.Messages.AssertEqual(
                OnCompleted<int>(300)
            );

            xs.Subscriptions.AssertEqual(
                Subscribe(201, 300)
            );
        }

        [TestMethod]
        public void Expand_Error()
        {
            var scheduler = new TestScheduler();

            var ex = new Exception();

            var xs = scheduler.CreateHotObservable(
                OnError<int>(300, ex)
            );

            var res = scheduler.Start(() =>
                xs.Expand(x => scheduler.CreateColdObservable<int>(
                    OnNext(100 + x, 2 * x),
                    OnNext(200 + x, 3 * x),
                    OnCompleted<int>(300 + x)
                ), scheduler)
            );

            res.Messages.AssertEqual(
                OnError<int>(300, ex)
            );

            xs.Subscriptions.AssertEqual(
                Subscribe(201, 300)
            );
        }

        [TestMethod]
        public void Expand_Never()
        {
            var scheduler = new TestScheduler();

            var xs = scheduler.CreateHotObservable<int>(
            );

            var res = scheduler.Start(() =>
                xs.Expand(x => scheduler.CreateColdObservable<int>(
                    OnNext(100 + x, 2 * x),
                    OnNext(200 + x, 3 * x),
                    OnCompleted<int>(300 + x)
                ), scheduler)
            );

            res.Messages.AssertEqual(
            );

            xs.Subscriptions.AssertEqual(
                Subscribe(201, 1000)
            );
        }

        [TestMethod]
        public void Expand_Basic()
        {
            var scheduler = new TestScheduler();

            var xs = scheduler.CreateHotObservable(
                OnNext(550, 1),
                OnNext(850, 2),
                OnCompleted<int>(950)
            );

            var res = scheduler.Start(() =>
                xs.Expand(x => scheduler.CreateColdObservable(
                    OnNext(100, 2 * x),
                    OnNext(200, 3 * x),
                    OnCompleted<int>(300)
                ), scheduler)
            );

            res.Messages.AssertEqual(
                OnNext(550, 1),
                OnNext(651, 2),
                OnNext(751, 3),
                OnNext(752, 4),
                OnNext(850, 2),
                OnNext(852, 6),
                OnNext(852, 6),
                OnNext(853, 8),
                OnNext(951, 4),
                OnNext(952, 9),
                OnNext(952, 12),
                OnNext(953, 12),
                OnNext(953, 12),
                OnNext(954, 16)
            );

            xs.Subscriptions.AssertEqual(
                Subscribe(201, 950)
            );
        }

        [TestMethod]
        public void Expand_Throw()
        {
            var scheduler = new TestScheduler();

            var xs = scheduler.CreateHotObservable(
                OnNext(550, 1),
                OnNext(850, 2),
                OnCompleted<int>(950)
            );

            var ex = new Exception();

            var res = scheduler.Start(() =>
                xs.Expand(x => { throw ex; }, scheduler)
            );

            res.Messages.AssertEqual(
                OnNext(550, 1),
                OnError<int>(550, ex)
            );

            xs.Subscriptions.AssertEqual(
                Subscribe(201, 550)
            );
        }

        #endregion

        #region ForkJoin

        [TestMethod]
        public void ForkJoin_ArgumentChecking()
        {
            var someObservable = DummyObservable<int>.Instance;

            ReactiveAssert.Throws<ArgumentNullException>(() => ObservableEx.ForkJoin(someObservable, someObservable, (Func<int, int, int>)null));
            ReactiveAssert.Throws<ArgumentNullException>(() => ObservableEx.ForkJoin(someObservable, (IObservable<int>)null, (_, __) => _ + __));
            ReactiveAssert.Throws<ArgumentNullException>(() => ObservableEx.ForkJoin((IObservable<int>)null, someObservable, (_, __) => _ + __));
            ReactiveAssert.Throws<ArgumentNullException>(() => ObservableEx.ForkJoin((IObservable<int>[])null));
            ReactiveAssert.Throws<ArgumentNullException>(() => ObservableEx.ForkJoin((IEnumerable<IObservable<int>>)null));
        }

        [TestMethod]
        public void ForkJoin_EmptyEmpty()
        {
            var scheduler = new TestScheduler();

            var msgs1 = new[] {
                OnNext(150, 1),
                OnCompleted<int>(230)
            };

            var msgs2 = new[] {
                OnNext(150, 1),
                OnCompleted<int>(250)
            };

            var o = scheduler.CreateHotObservable(msgs1);
            var e = scheduler.CreateHotObservable(msgs2);

            var res = scheduler.Start(() => e.ForkJoin(o, (x, y) => x + y));
            res.Messages.AssertEqual(
                OnCompleted<int>(250)
            );
        }

        [TestMethod]
        public void ForkJoin_None()
        {
            var scheduler = new TestScheduler();

            var res = scheduler.Start(() => ObservableEx.ForkJoin<int>());
            res.Messages.AssertEqual(
                OnCompleted<int[]>(200)
            );
        }

        [TestMethod]
        public void ForkJoin_EmptyReturn()
        {
            var scheduler = new TestScheduler();

            var msgs1 = new[] {
                OnNext(150, 1),
                OnCompleted<int>(230)
            };

            var msgs2 = new[] {
                OnNext(150, 1),
                OnNext(210, 2),
                OnCompleted<int>(250)
            };

            var o = scheduler.CreateHotObservable(msgs1);
            var e = scheduler.CreateHotObservable(msgs2);

            var res = scheduler.Start(() => e.ForkJoin(o, (x, y) => x + y));
            res.Messages.AssertEqual(
                OnCompleted<int>(250)
            );
        }

        [TestMethod]
        public void ForkJoin_ReturnEmpty()
        {
            var scheduler = new TestScheduler();

            var msgs1 = new[] {
                OnNext(150, 1),
                OnNext(210, 2),
                OnCompleted<int>(230)
            };

            var msgs2 = new[] {
                OnNext(150, 1),
                OnCompleted<int>(250)
            };

            var o = scheduler.CreateHotObservable(msgs1);
            var e = scheduler.CreateHotObservable(msgs2);

            var res = scheduler.Start(() => e.ForkJoin(o, (x, y) => x + y));
            res.Messages.AssertEqual(
                OnCompleted<int>(250)
            );
        }

        [TestMethod]
        public void ForkJoin_ReturnReturn()
        {
            var scheduler = new TestScheduler();

            var msgs1 = new[] {
                OnNext(150, 1),
                OnNext(210, 2),
                OnCompleted<int>(230)
            };

            var msgs2 = new[] {
                OnNext(150, 1),
                OnNext(220, 3),
                OnCompleted<int>(250)
            };

            var o = scheduler.CreateHotObservable(msgs1);
            var e = scheduler.CreateHotObservable(msgs2);

            var res = scheduler.Start(() => e.ForkJoin(o, (x, y) => x + y));
            res.Messages.AssertEqual(
                OnNext(250, 2 + 3),
                OnCompleted<int>(250)
            );
        }

        [TestMethod]
        public void ForkJoin_EmptyThrow()
        {
            var ex = new Exception();

            var scheduler = new TestScheduler();

            var msgs1 = new[] {
                OnNext(150, 1),
                OnCompleted<int>(230)
            };

            var msgs2 = new[] {
                OnNext(150, 1),
                OnError<int>(210, ex),
                OnCompleted<int>(250)
            };

            var o = scheduler.CreateHotObservable(msgs1);
            var e = scheduler.CreateHotObservable(msgs2);

            var res = scheduler.Start(() => e.ForkJoin(o, (x, y) => x + y));
            res.Messages.AssertEqual(
                OnError<int>(210, ex)
            );
        }

        [TestMethod]
        public void ForkJoin_ThrowEmpty()
        {
            var ex = new Exception();

            var scheduler = new TestScheduler();

            var msgs1 = new[] {
                OnNext(150, 1),
                OnError<int>(210, ex),
                OnCompleted<int>(230)
            };

            var msgs2 = new[] {
                OnNext(150, 1),
                OnCompleted<int>(250)
            };

            var o = scheduler.CreateHotObservable(msgs1);
            var e = scheduler.CreateHotObservable(msgs2);

            var res = scheduler.Start(() => e.ForkJoin(o, (x, y) => x + y));
            res.Messages.AssertEqual(
                OnError<int>(210, ex)
            );
        }

        [TestMethod]
        public void ForkJoin_ReturnThrow()
        {
            var ex = new Exception();

            var scheduler = new TestScheduler();

            var msgs1 = new[] {
                OnNext(150, 1),
                OnNext(210, 2),
                OnCompleted<int>(230)
            };

            var msgs2 = new[] {
                OnNext(150, 1),
                OnError<int>(220, ex),
                OnCompleted<int>(250)
            };

            var o = scheduler.CreateHotObservable(msgs1);
            var e = scheduler.CreateHotObservable(msgs2);

            var res = scheduler.Start(() => e.ForkJoin(o, (x, y) => x + y));
            res.Messages.AssertEqual(
                OnError<int>(220, ex)
            );
        }

        [TestMethod]
        public void ForkJoin_ThrowReturn()
        {
            var ex = new Exception();

            var scheduler = new TestScheduler();

            var msgs1 = new[] {
                OnNext(150, 1),
                OnError<int>(220, ex),
                OnCompleted<int>(230)
            };

            var msgs2 = new[] {
                OnNext(150, 1),
                OnNext(210, 2),
                OnCompleted<int>(250)
            };

            var o = scheduler.CreateHotObservable(msgs1);
            var e = scheduler.CreateHotObservable(msgs2);

            var res = scheduler.Start(() => e.ForkJoin(o, (x, y) => x + y));
            res.Messages.AssertEqual(
                OnError<int>(220, ex)
            );
        }

        [TestMethod]
        public void ForkJoin_Binary()
        {
            var scheduler = new TestScheduler();

            var msgs1 = new[] {
                OnNext(150, 1),
                OnNext(215, 2),
                OnNext(225, 4),
                OnCompleted<int>(230)
            };

            var msgs2 = new[] {
                OnNext(150, 1),
                OnNext(235, 6),
                OnNext(240, 7),
                OnCompleted<int>(250)
            };

            var o = scheduler.CreateHotObservable(msgs1);
            var e = scheduler.CreateHotObservable(msgs2);

            var res = scheduler.Start(() => e.ForkJoin(o, (x, y) => x + y));
            res.Messages.AssertEqual(
                OnNext(250, 4 + 7),   // TODO: fix ForkJoin behavior
                OnCompleted<int>(250)
            );
        }

        [TestMethod]
        public void ForkJoin_NaryParams()
        {
            var scheduler = new TestScheduler();

            var msgs1 = new[] {
                OnNext(150, 1),
                OnNext(215, 2),
                OnNext(225, 4),
                OnCompleted<int>(230)
            };

            var msgs2 = new[] {
                OnNext(150, 1),
                OnNext(235, 6),
                OnNext(240, 7),
                OnCompleted<int>(250)
            };

            var msgs3 = new[] {
                OnNext(150, 1),
                OnNext(230, 3),
                OnNext(245, 5),
                OnCompleted<int>(270)
            };

            var o1 = scheduler.CreateHotObservable(msgs1);
            var o2 = scheduler.CreateHotObservable(msgs2);
            var o3 = scheduler.CreateHotObservable(msgs3);

            var res = scheduler.Start(() => ObservableEx.ForkJoin(o1, o2, o3));

            res.Messages.AssertEqual(
                OnNext<int[]>(270, l => l.SequenceEqual(new[] { 4, 7, 5 })), // TODO: fix ForkJoin behavior
                OnCompleted<int[]>(270)
            );
        }

        [TestMethod]
        public void ForkJoin_NaryParamsEmpty()
        {
            var scheduler = new TestScheduler();

            var msgs1 = new[] {
                OnNext(150, 1),
                OnNext(215, 2),
                OnNext(225, 4),
                OnCompleted<int>(230)
            };

            var msgs2 = new[] {
                OnNext(150, 1),
                OnNext(235, 6),
                OnNext(240, 7),
                OnCompleted<int>(250)
            };

            var msgs3 = new[] {
                OnCompleted<int>(270)
            };

            var o1 = scheduler.CreateHotObservable(msgs1);
            var o2 = scheduler.CreateHotObservable(msgs2);
            var o3 = scheduler.CreateHotObservable(msgs3);

            var res = scheduler.Start(() => ObservableEx.ForkJoin(o1, o2, o3));

            res.Messages.AssertEqual(
                OnCompleted<int[]>(270)
            );
        }

        [TestMethod]
        public void ForkJoin_NaryParamsEmptyBeforeEnd()
        {
            var scheduler = new TestScheduler();

            var msgs1 = new[] {
                OnNext(150, 1),
                OnNext(215, 2),
                OnNext(225, 4),
                OnCompleted<int>(230)
            };

            var msgs2 = new[] {
                OnCompleted<int>(235)
            };

            var msgs3 = new[] {
                OnNext(150, 1),
                OnNext(230, 3),
                OnNext(245, 5),
                OnCompleted<int>(270)
            };

            var o1 = scheduler.CreateHotObservable(msgs1);
            var o2 = scheduler.CreateHotObservable(msgs2);
            var o3 = scheduler.CreateHotObservable(msgs3);

            var res = scheduler.Start(() => ObservableEx.ForkJoin(o1, o2, o3));

            res.Messages.AssertEqual(
                OnCompleted<int[]>(235)
            );
        }

        [TestMethod]
        public void ForkJoin_Nary_Immediate()
        {
            ObservableEx.ForkJoin(Observable.Return(1), Observable.Return(2)).First().SequenceEqual(new[] { 1, 2 });
        }

        [TestMethod]
        public void ForkJoin_Nary_Virtual_And_Immediate()
        {
            var scheduler = new TestScheduler();

            var msgs1 = new[] {
                OnNext(150, 1),
                OnNext(215, 2),
                OnNext(225, 4),
                OnCompleted<int>(230)
            };

            var msgs2 = new[] {
                OnNext(150, 1),
                OnNext(235, 6),
                OnNext(240, 7),
                OnCompleted<int>(250)
            };

            var msgs3 = new[] {
                OnNext(150, 1),
                OnNext(230, 3),
                OnNext(245, 5),
                OnCompleted<int>(270)
            };

            var o1 = scheduler.CreateHotObservable(msgs1);
            var o2 = scheduler.CreateHotObservable(msgs2);
            var o3 = scheduler.CreateHotObservable(msgs3);

            var res = scheduler.Start(() => ObservableEx.ForkJoin(new List<IObservable<int>> { o1, o2, o3, Observable.Return(20) }));

            res.Messages.AssertEqual(
                OnNext<int[]>(270, l => l.SequenceEqual(new[] { 4, 7, 5, 20 })),
                OnCompleted<int[]>(270)
            );
        }

        [TestMethod]
        public void ForkJoin_Nary_Immediate_And_Virtual()
        {
            var scheduler = new TestScheduler();

            var msgs1 = new[] {
                OnNext(150, 1),
                OnNext(215, 2),
                OnNext(225, 4),
                OnCompleted<int>(230)
            };

            var msgs2 = new[] {
                OnNext(150, 1),
                OnNext(235, 6),
                OnNext(240, 7),
                OnCompleted<int>(250)
            };

            var msgs3 = new[] {
                OnNext(150, 1),
                OnNext(230, 3),
                OnNext(245, 5),
                OnCompleted<int>(270)
            };

            var o1 = scheduler.CreateHotObservable(msgs1);
            var o2 = scheduler.CreateHotObservable(msgs2);
            var o3 = scheduler.CreateHotObservable(msgs3);

            var res = scheduler.Start(() => ObservableEx.ForkJoin(new List<IObservable<int>> { Observable.Return(20), o1, o2, o3 }));

            res.Messages.AssertEqual(
                OnNext<int[]>(270, l => l.SequenceEqual(new[] { 20, 4, 7, 5 })),
                OnCompleted<int[]>(270)
            );
        }

        [TestMethod]
        public void ForkJoin_Nary()
        {
            var scheduler = new TestScheduler();

            var msgs1 = new[] {
                OnNext(150, 1),
                OnNext(215, 2),
                OnNext(225, 4),
                OnCompleted<int>(230)
            };

            var msgs2 = new[] {
                OnNext(150, 1),
                OnNext(235, 6),
                OnNext(240, 7),
                OnCompleted<int>(250)
            };

            var msgs3 = new[] {
                OnNext(150, 1),
                OnNext(230, 3),
                OnNext(245, 5),
                OnCompleted<int>(270)
            };

            var o1 = scheduler.CreateHotObservable(msgs1);
            var o2 = scheduler.CreateHotObservable(msgs2);
            var o3 = scheduler.CreateHotObservable(msgs3);

            var res = scheduler.Start(() => ObservableEx.ForkJoin(new List<IObservable<int>> { o1, o2, o3 }));

            res.Messages.AssertEqual(
                OnNext<int[]>(270, l => l.SequenceEqual(new[] { 4, 7, 5 })), // TODO: fix ForkJoin behavior
                OnCompleted<int[]>(270)
            );
        }

        [TestMethod]
        public void Bug_1302_SelectorThrows_LeftLast()
        {
            var scheduler = new TestScheduler();

            var xs = scheduler.CreateHotObservable(
                OnNext(210, 1),
                OnCompleted<int>(220)
            );

            var ys = scheduler.CreateHotObservable(
                OnNext(215, 2),
                OnCompleted<int>(217)
            );

            var ex = new Exception();

            var results = scheduler.Start(() => xs.ForkJoin<int, int, int>(ys, (x, y) => { throw ex; }));

            results.Messages.AssertEqual(
                OnError<int>(220, ex)
            );

            xs.Subscriptions.AssertEqual(
                Subscribe(200, 220)
            );

            ys.Subscriptions.AssertEqual(
                Subscribe(200, 217)
            );
        }

        [TestMethod]
        public void Bug_1302_SelectorThrows_RightLast()
        {
            var scheduler = new TestScheduler();

            var xs = scheduler.CreateHotObservable(
                OnNext(210, 1),
                OnCompleted<int>(217)
            );

            var ys = scheduler.CreateHotObservable(
                OnNext(215, 2),
                OnCompleted<int>(220)
            );

            var ex = new Exception();

            var results = scheduler.Start(() => xs.ForkJoin<int, int, int>(ys, (x, y) => { throw ex; }));

            results.Messages.AssertEqual(
                OnError<int>(220, ex)
            );

            xs.Subscriptions.AssertEqual(
                Subscribe(200, 217)
            );

            ys.Subscriptions.AssertEqual(
                Subscribe(200, 220)
            );
        }

        [TestMethod]
        public void Bug_1302_RightLast_NoLeft()
        {
            var scheduler = new TestScheduler();

            var xs = scheduler.CreateHotObservable(
                OnCompleted<int>(217)
            );

            var ys = scheduler.CreateHotObservable(
                OnNext(215, 2),
                OnCompleted<int>(220)
            );

            var results = scheduler.Start(() => xs.ForkJoin<int, int, int>(ys, (x, y) => x + y));

            results.Messages.AssertEqual(
                OnCompleted<int>(220)
            );

            xs.Subscriptions.AssertEqual(
                Subscribe(200, 217)
            );

            ys.Subscriptions.AssertEqual(
                Subscribe(200, 220)
            );
        }

        [TestMethod]
        public void Bug_1302_RightLast_NoRight()
        {
            var scheduler = new TestScheduler();

            var xs = scheduler.CreateHotObservable(
                OnNext(215, 2),
                OnCompleted<int>(217)
            );

            var ys = scheduler.CreateHotObservable(
                OnCompleted<int>(220)
            );

            var results = scheduler.Start(() => xs.ForkJoin<int, int, int>(ys, (x, y) => x + y));

            results.Messages.AssertEqual(
                OnCompleted<int>(220)
            );

            xs.Subscriptions.AssertEqual(
                Subscribe(200, 217)
            );

            ys.Subscriptions.AssertEqual(
                Subscribe(200, 220)
            );
        }

        #endregion

        #region Let

        [TestMethod]
        public void Let_ArgumentChecking()
        {
            var someObservable = Observable.Empty<int>();

            ReactiveAssert.Throws<ArgumentNullException>(() => ObservableEx.Let(default(IObservable<int>), x => x));
            ReactiveAssert.Throws<ArgumentNullException>(() => ObservableEx.Let<int, int>(someObservable, null));
        }

        [TestMethod]
        public void Let_CallsFunctionImmediately()
        {
            bool called = false;
            Observable.Empty<int>().Let(x => { called = true; return x; });
            Assert.IsTrue(called);
        }

        #endregion

        #region ManySelect

        [TestMethod]
        public void ManySelect_ArgumentChecking()
        {
            ReactiveAssert.Throws<ArgumentNullException>(() => ObservableEx.ManySelect<int, int>(null, DummyFunc<IObservable<int>, int>.Instance, DummyScheduler.Instance));
            ReactiveAssert.Throws<ArgumentNullException>(() => ObservableEx.ManySelect<int, int>(DummyObservable<int>.Instance, null, DummyScheduler.Instance));
            ReactiveAssert.Throws<ArgumentNullException>(() => ObservableEx.ManySelect<int, int>(DummyObservable<int>.Instance, DummyFunc<IObservable<int>, int>.Instance, null));
            ReactiveAssert.Throws<ArgumentNullException>(() => ObservableEx.ManySelect<int, int>(null, DummyFunc<IObservable<int>, int>.Instance));
            ReactiveAssert.Throws<ArgumentNullException>(() => ObservableEx.ManySelect<int, int>(DummyObservable<int>.Instance, null));
        }

        [TestMethod]
        public void ManySelect_Law_1()
        {
            var xs = Observable.Range(1, 0);

            var left = xs.ManySelect(Observable.First);
            var right = xs;

            Assert.IsTrue(left.SequenceEqual(right).First());
        }

        [TestMethod]
        public void ManySelect_Law_2()
        {
            var xs = Observable.Range(1, 10);
            Func<IObservable<int>, int> f = ys => ys.Count().First();

            var left = xs.ManySelect(f).First();
            var right = f(xs);

            Assert.AreEqual(left, right);
        }

        [TestMethod]
        public void ManySelect_Law_3()
        {
            var xs = Observable.Range(1, 10);
            Func<IObservable<int>, int> f = ys => ys.Count().First();
            Func<IObservable<int>, int> g = ys => ys.Last();

            var left = xs.ManySelect(f).ManySelect(g);
            var right = xs.ManySelect(ys => g(ys.ManySelect(f)));

            Assert.IsTrue(left.SequenceEqual(right).First());
        }

        [TestMethod]
        public void ManySelect_Basic()
        {
            var scheduler = new TestScheduler();

            var xs = scheduler.CreateHotObservable(
                OnNext(100, 1),
                OnNext(220, 2),
                OnNext(270, 3),
                OnNext(410, 4),
                OnCompleted<int>(500)
            );

            var res = scheduler.Start(() =>
                xs.ManySelect(ys => ys.First(), scheduler)
            );

            res.Messages.AssertEqual(
                OnNext(221, 2),
                OnNext(271, 3),
                OnNext(411, 4),
                OnCompleted<int>(501)
            );

            xs.Subscriptions.AssertEqual(
                Subscribe(200, 500)
            );
        }

        [TestMethod]
        public void ManySelect_Error()
        {
            var scheduler = new TestScheduler();

            var ex = new Exception();

            var xs = scheduler.CreateHotObservable(
                OnNext(100, 1),
                OnNext(220, 2),
                OnNext(270, 3),
                OnNext(410, 4),
                OnError<int>(500, ex)
            );

            var res = scheduler.Start(() =>
                xs.ManySelect(ys => ys.First(), scheduler)
            );

            res.Messages.AssertEqual(
                OnNext(221, 2),
                OnNext(271, 3),
                OnNext(411, 4),
                OnError<int>(501, ex)
            );

            xs.Subscriptions.AssertEqual(
                Subscribe(200, 500)
            );
        }

        #endregion

        #region ToListObservable

        [TestMethod]
        public void ToListObservable_ArgumentChecking()
        {
            ReactiveAssert.Throws<ArgumentNullException>(() => ((IObservable<int>)null).ToListObservable());
            ReactiveAssert.Throws<ArgumentNullException>(() => Observable.Never<int>().ToListObservable().Subscribe(null));
        }

        [TestMethod]
        public void ToListObservable_OnNext()
        {
            var scheduler = new TestScheduler();

            var xs = scheduler.CreateHotObservable(
                OnNext(300, 1),
                OnNext(400, 2),
                OnNext(500, 3),
                OnNext(600, 4)
            );

            var res = scheduler.Start(() =>
                xs.ToListObservable()
            );

            res.Messages.AssertEqual(
            );
        }

        [TestMethod]
        public void ToListObservable_OnError()
        {
            var scheduler = new TestScheduler();

            var ex = new InvalidOperationException();

            var xs = scheduler.CreateHotObservable(
                OnNext(300, 1),
                OnNext(400, 2),
                OnNext(500, 3),
                OnError<int>(600, ex)
            );

            var s = default(ListObservable<int>);
            var subscription = default(IDisposable);
            var res = scheduler.CreateObserver<object>();

            scheduler.ScheduleAbsolute(Created, () => s = xs.ToListObservable());
            scheduler.ScheduleAbsolute(Subscribed, () => subscription = s.Subscribe(res));
            scheduler.ScheduleAbsolute(Disposed, () => subscription.Dispose());

            scheduler.Start();

            ReactiveAssert.Throws<InvalidOperationException>(() => { var t = s.Value; });

            res.Messages.AssertEqual(
                OnError<Object>(600, ex)
            );
        }

        [TestMethod]
        public void ToListObservable_OnCompleted()
        {
            var scheduler = new TestScheduler();

            var xs = scheduler.CreateHotObservable(
                OnNext(300, 1),
                OnNext(400, 2),
                OnNext(500, 3),
                OnCompleted<int>(600)
            );

            var s = default(ListObservable<int>);

            var res = scheduler.Start(() =>
                s = xs.ToListObservable()
            );

            s.AssertEqual(1, 2, 3);

            res.Messages.AssertEqual(
                OnCompleted<Object>(600)
            );

            Assert.AreEqual(3, s.Value);
        }

        [TestMethod]
        public void ToListObservable_Disposed()
        {
            var scheduler = new TestScheduler();

            var xs = scheduler.CreateHotObservable(
                OnNext(300, 1),
                OnNext(400, 2),
                OnNext(500, 3),
                OnNext(1050, 4),
                OnCompleted<int>(1100)
            );

            var s = default(ListObservable<int>);

            var res = scheduler.Start(() =>
                s = xs.ToListObservable()
            );

            res.Messages.AssertEqual(
            );
        }

        [TestMethod]
        public void ToListObservable_Never()
        {
            var scheduler = new TestScheduler();

            var xs = scheduler.CreateHotObservable<int>(
            );

            var res = scheduler.Start(() =>
                xs.ToListObservable()
            );

            res.Messages.AssertEqual(
            );
        }

        #endregion
    }
}

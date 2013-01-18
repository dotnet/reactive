// Copyright (c) Microsoft Open Technologies, Inc. All rights reserved. See License.txt in the project root for license information.

using System;
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
    public partial class ObservableImperativeTest : ReactiveTest
    {
        #region ForEachAsync

#if !NO_TPL
        [TestMethod]
        public void ForEachAsync_ArgumentChecking()
        {
            ReactiveAssert.Throws<ArgumentNullException>(() => Observable.ForEachAsync(default(IObservable<int>), x => { }));
            ReactiveAssert.Throws<ArgumentNullException>(() => Observable.ForEachAsync(Observable.Never<int>(), default(Action<int>)));
            ReactiveAssert.Throws<ArgumentNullException>(() => Observable.ForEachAsync(default(IObservable<int>), x => { }, CancellationToken.None));
            ReactiveAssert.Throws<ArgumentNullException>(() => Observable.ForEachAsync(Observable.Never<int>(), default(Action<int>), CancellationToken.None));

            ReactiveAssert.Throws<ArgumentNullException>(() => Observable.ForEachAsync(default(IObservable<int>), (x, i) => { }));
            ReactiveAssert.Throws<ArgumentNullException>(() => Observable.ForEachAsync(Observable.Never<int>(), default(Action<int, int>)));
            ReactiveAssert.Throws<ArgumentNullException>(() => Observable.ForEachAsync(default(IObservable<int>), (x, i) => { }, CancellationToken.None));
            ReactiveAssert.Throws<ArgumentNullException>(() => Observable.ForEachAsync(Observable.Never<int>(), default(Action<int, int>), CancellationToken.None));
        }

        [TestMethod]
        public void ForEachAsync_Never()
        {
            var scheduler = new TestScheduler();

            var xs = scheduler.CreateHotObservable(
                OnNext(100, 1),
                OnNext(200, 2),
                OnNext(300, 3),
                OnNext(400, 4),
                OnNext(500, 5)
            );

            var task = default(Task);
            var cts = new CancellationTokenSource();
            var list = new List<Recorded<int>>();

            scheduler.ScheduleAbsolute(150, () => task = xs.ForEachAsync(x => list.Add(new Recorded<int>(scheduler.Clock, x)), cts.Token));

            scheduler.Start();

            xs.Subscriptions.AssertEqual(
                Subscribe(150)
            );

            list.AssertEqual(
                new Recorded<int>(200, 2),
                new Recorded<int>(300, 3),
                new Recorded<int>(400, 4),
                new Recorded<int>(500, 5)
            );

            Assert.AreEqual(TaskStatus.WaitingForActivation, task.Status);
        }

        [TestMethod]
        public void ForEachAsync_Completed()
        {
            var scheduler = new TestScheduler();

            var xs = scheduler.CreateHotObservable(
                OnNext(100, 1),
                OnNext(200, 2),
                OnNext(300, 3),
                OnNext(400, 4),
                OnNext(500, 5),
                OnCompleted<int>(600)
            );

            var task = default(Task);
            var cts = new CancellationTokenSource();
            var list = new List<Recorded<int>>();

            scheduler.ScheduleAbsolute(150, () => task = xs.ForEachAsync(x => list.Add(new Recorded<int>(scheduler.Clock, x)), cts.Token));

            scheduler.Start();

            xs.Subscriptions.AssertEqual(
                Subscribe(150, 600)
            );

            list.AssertEqual(
                new Recorded<int>(200, 2),
                new Recorded<int>(300, 3),
                new Recorded<int>(400, 4),
                new Recorded<int>(500, 5)
            );

            Assert.AreEqual(TaskStatus.RanToCompletion, task.Status);
        }

        [TestMethod]
        public void ForEachAsync_Error()
        {
            var scheduler = new TestScheduler();

            var exception = new Exception();

            var xs = scheduler.CreateHotObservable(
                OnNext(100, 1),
                OnNext(200, 2),
                OnNext(300, 3),
                OnNext(400, 4),
                OnNext(500, 5),
                OnError<int>(600, exception)
            );

            var task = default(Task);
            var cts = new CancellationTokenSource();
            var list = new List<Recorded<int>>();

            scheduler.ScheduleAbsolute(150, () => task = xs.ForEachAsync(x => list.Add(new Recorded<int>(scheduler.Clock, x)), cts.Token));

            scheduler.Start();

            xs.Subscriptions.AssertEqual(
                Subscribe(150, 600)
            );

            list.AssertEqual(
                new Recorded<int>(200, 2),
                new Recorded<int>(300, 3),
                new Recorded<int>(400, 4),
                new Recorded<int>(500, 5)
            );

            Assert.AreEqual(TaskStatus.Faulted, task.Status);
            Assert.AreSame(exception, task.Exception.InnerException);
        }

        [TestMethod]
        public void ForEachAsync_Throw()
        {
            var scheduler = new TestScheduler();

            var exception = new Exception();

            var xs = scheduler.CreateHotObservable(
                OnNext(100, 1),
                OnNext(200, 2),
                OnNext(300, 3),
                OnNext(400, 4),
                OnNext(500, 5),
                OnCompleted<int>(600)
            );

            var task = default(Task);
            var cts = new CancellationTokenSource();
            var list = new List<Recorded<int>>();

            scheduler.ScheduleAbsolute(150, () => task = xs.ForEachAsync(x =>
            {
                if (scheduler.Clock > 400)
                    throw exception;
                list.Add(new Recorded<int>(scheduler.Clock, x));
            }, cts.Token));

            scheduler.Start();

            xs.Subscriptions.AssertEqual(
                Subscribe(150, 500)
            );

            list.AssertEqual(
                new Recorded<int>(200, 2),
                new Recorded<int>(300, 3),
                new Recorded<int>(400, 4)
            );

            Assert.AreEqual(TaskStatus.Faulted, task.Status);
            Assert.AreSame(exception, task.Exception.InnerException);
        }

        [TestMethod]
        public void ForEachAsync_CancelDuring()
        {
            var scheduler = new TestScheduler();

            var xs = scheduler.CreateHotObservable(
                OnNext(100, 1),
                OnNext(200, 2),
                OnNext(300, 3),
                OnNext(400, 4),
                OnNext(500, 5),
                OnCompleted<int>(600)
            );

            var task = default(Task);
            var cts = new CancellationTokenSource();
            var list = new List<Recorded<int>>();

            scheduler.ScheduleAbsolute(150, () => task = xs.ForEachAsync(x => list.Add(new Recorded<int>(scheduler.Clock, x)), cts.Token));
            scheduler.ScheduleAbsolute(350, () => cts.Cancel());

            scheduler.Start();

            xs.Subscriptions.AssertEqual(
                Subscribe(150, 350)
            );

            list.AssertEqual(
                new Recorded<int>(200, 2),
                new Recorded<int>(300, 3)
            );

            Assert.AreEqual(TaskStatus.Canceled, task.Status);
        }

        [TestMethod]
        public void ForEachAsync_CancelBefore()
        {
            var scheduler = new TestScheduler();

            var xs = scheduler.CreateHotObservable(
                OnNext(100, 1),
                OnNext(200, 2),
                OnNext(300, 3),
                OnNext(400, 4),
                OnNext(500, 5),
                OnCompleted<int>(600)
            );

            var task = default(Task);
            var cts = new CancellationTokenSource();
            var list = new List<Recorded<int>>();

            cts.Cancel();

            scheduler.ScheduleAbsolute(150, () => task = xs.ForEachAsync(x => list.Add(new Recorded<int>(scheduler.Clock, x)), cts.Token));

            scheduler.Start();

            xs.Subscriptions.AssertEqual(
            );

            list.AssertEqual(
            );

            Assert.AreEqual(TaskStatus.Canceled, task.Status);
        }

        [TestMethod]
        public void ForEachAsync_CancelAfter()
        {
            var scheduler = new TestScheduler();

            var xs = scheduler.CreateHotObservable(
                OnNext(100, 1),
                OnNext(200, 2),
                OnNext(300, 3),
                OnNext(400, 4),
                OnNext(500, 5),
                OnCompleted<int>(600)
            );

            var task = default(Task);
            var cts = new CancellationTokenSource();
            var list = new List<Recorded<int>>();

            scheduler.ScheduleAbsolute(150, () => task = xs.ForEachAsync(x => list.Add(new Recorded<int>(scheduler.Clock, x)), cts.Token));
            scheduler.ScheduleAbsolute(700, () => cts.Cancel());

            scheduler.Start();

            xs.Subscriptions.AssertEqual(
                Subscribe(150, 600)
            );

            list.AssertEqual(
                new Recorded<int>(200, 2),
                new Recorded<int>(300, 3),
                new Recorded<int>(400, 4),
                new Recorded<int>(500, 5)
            );

            Assert.AreEqual(TaskStatus.RanToCompletion, task.Status);
        }

        [TestMethod]
        public void ForEachAsync_Default()
        {
            var list = new List<int>();
            Observable.Range(1, 10).ForEachAsync(list.Add).Wait();
            list.AssertEqual(1, 2, 3, 4, 5, 6, 7, 8, 9, 10);
        }

        [TestMethod]
        public void ForEachAsync_Index()
        {
            var list = new List<int>();
            Observable.Range(3, 5).ForEachAsync((x, i) => list.Add(x * i)).Wait();
            list.AssertEqual(3 * 0, 4 * 1, 5 * 2, 6 * 3, 7 * 4);
        }

        [TestMethod]
        public void ForEachAsync_Default_Cancel()
        {
            var N = 10;

            for (int n = 0; n < N; n++)
            {
                var cts = new CancellationTokenSource();
                var done = false;

                var xs = Observable.Create<int>(observer =>
                {
                    return new CompositeDisposable(
                        Observable.Repeat(42, Scheduler.Default).Subscribe(observer),
                        Disposable.Create(() => done = true)
                    );
                });

                var lst = new List<int>();

                var t = xs.ForEachAsync(
                    x =>
                    {
                        lock (lst)
                            lst.Add(x);
                    },
                    cts.Token
                );

                while (true)
                {
                    lock (lst)
                        if (lst.Count >= 10)
                            break;
                }

                cts.Cancel();

                while (!t.IsCompleted)
                    ;

                for (int i = 0; i < 10; i++)
                    Assert.AreEqual(42, lst[i]);

                Assert.IsTrue(done);
                Assert.IsTrue(t.IsCanceled);
            }
        }

        [TestMethod]
        public void ForEachAsync_Index_Cancel()
        {
            var N = 10;

            for (int n = 0; n < N; n++)
            {
                var cts = new CancellationTokenSource();
                var done = false;

                var xs = Observable.Create<int>(observer =>
                {
                    return new CompositeDisposable(
                        Observable.Repeat(42, Scheduler.Default).Subscribe(observer),
                        Disposable.Create(() => done = true)
                    );
                });

                var lst = new List<int>();

                var t = xs.ForEachAsync(
                    (x, i) =>
                    {
                        lock (lst)
                            lst.Add(x * i);
                    },
                    cts.Token
                );

                while (true)
                {
                    lock (lst)
                        if (lst.Count >= 10)
                            break;
                }

                cts.Cancel();

                while (!t.IsCompleted)
                    ;

                for (int i = 0; i < 10; i++)
                    Assert.AreEqual(i * 42, lst[i]);

                Assert.IsTrue(done);
                Assert.IsTrue(t.IsCanceled);
            }
        }

        [TestMethod]
        [Ignore]
        public void ForEachAsync_DisposeThrows()
        {
            var cts = new CancellationTokenSource();
            var ex = new Exception();

            var xs = Observable.Create<int>(observer =>
            {
                return new CompositeDisposable(
                    Observable.Range(0, 10, Scheduler.Default).Subscribe(observer),
                    Disposable.Create(() => { throw ex; })
                );
            });

            var lst = new List<int>();
            var t = xs.ForEachAsync(lst.Add, cts.Token);

            try
            {
                t.Wait();
                Assert.Fail();
            }
            catch (AggregateException err)
            {
                Assert.AreEqual(1, err.InnerExceptions.Count);
                Assert.AreSame(ex, err.InnerExceptions[0]);
            }
        }

        [TestMethod]
        public void ForEachAsync_SubscribeThrows()
        {
            var ex = new Exception();

            var x = 42;
            var xs = Observable.Create<int>(observer =>
            {
                if (x == 42)
                    throw ex;
                return Disposable.Empty;
            });

            var t = xs.ForEachAsync(_ => { });

            try
            {
                t.Wait();
                Assert.Fail();
            }
            catch (AggregateException err)
            {
                Assert.AreEqual(1, err.InnerExceptions.Count);
                Assert.AreSame(ex, err.InnerExceptions[0]);
            }
        }
#endif

        #endregion

        #region + Case +

        [TestMethod]
        public void Case_ArgumentChecking()
        {
            ReactiveAssert.Throws<ArgumentNullException>(() => Observable.Case(null, new Dictionary<int, IObservable<int>>(), DummyObservable<int>.Instance));
            ReactiveAssert.Throws<ArgumentNullException>(() => Observable.Case(DummyFunc<int>.Instance, null, DummyObservable<int>.Instance));
            ReactiveAssert.Throws<ArgumentNullException>(() => Observable.Case(DummyFunc<int>.Instance, new Dictionary<int, IObservable<int>>(), default(IObservable<int>)));

            ReactiveAssert.Throws<ArgumentNullException>(() => Observable.Case(null, new Dictionary<int, IObservable<int>>(), DummyScheduler.Instance));
            ReactiveAssert.Throws<ArgumentNullException>(() => Observable.Case<int, int>(DummyFunc<int>.Instance, null, DummyScheduler.Instance));
            ReactiveAssert.Throws<ArgumentNullException>(() => Observable.Case(DummyFunc<int>.Instance, new Dictionary<int, IObservable<int>>(), default(IScheduler)));

            ReactiveAssert.Throws<ArgumentNullException>(() => Observable.Case(null, new Dictionary<int, IObservable<int>>()));
            ReactiveAssert.Throws<ArgumentNullException>(() => Observable.Case<int, int>(DummyFunc<int>.Instance, null));
        }

        [TestMethod]
        public void Case_One()
        {
            var scheduler = new TestScheduler();

            var xs = scheduler.CreateHotObservable(
                OnNext(210, 1),
                OnNext(240, 2),
                OnNext(270, 3),
                OnCompleted<int>(300)
            );

            var ys = scheduler.CreateHotObservable(
                OnNext(220, 11),
                OnNext(250, 12),
                OnNext(280, 13),
                OnCompleted<int>(310)
            );

            var zs = scheduler.CreateHotObservable(
                OnNext(230, 21),
                OnNext(240, 22),
                OnNext(290, 23),
                OnCompleted<int>(320)
            );

            var map = new Dictionary<int, IObservable<int>>
            {
                { 1, xs },
                { 2, ys }
            };

            var results = scheduler.Start(() => Observable.Case(() => 1, map, zs));

            results.Messages.AssertEqual(
                OnNext(210, 1),
                OnNext(240, 2),
                OnNext(270, 3),
                OnCompleted<int>(300)
            );

            xs.Subscriptions.AssertEqual(
                Subscribe(200, 300)
            );

            ys.Subscriptions.AssertEqual(
            );

            zs.Subscriptions.AssertEqual(
            );
        }

        [TestMethod]
        public void Case_Two()
        {
            var scheduler = new TestScheduler();

            var xs = scheduler.CreateHotObservable(
                OnNext(210, 1),
                OnNext(240, 2),
                OnNext(270, 3),
                OnCompleted<int>(300)
            );

            var ys = scheduler.CreateHotObservable(
                OnNext(220, 11),
                OnNext(250, 12),
                OnNext(280, 13),
                OnCompleted<int>(310)
            );

            var zs = scheduler.CreateHotObservable(
                OnNext(230, 21),
                OnNext(240, 22),
                OnNext(290, 23),
                OnCompleted<int>(320)
            );

            var map = new Dictionary<int, IObservable<int>>
            {
                { 1, xs },
                { 2, ys }
            };

            var results = scheduler.Start(() => Observable.Case(() => 2, map, zs));

            results.Messages.AssertEqual(
                OnNext(220, 11),
                OnNext(250, 12),
                OnNext(280, 13),
                OnCompleted<int>(310)
            );

            xs.Subscriptions.AssertEqual(
            );

            ys.Subscriptions.AssertEqual(
                Subscribe(200, 310)
            );

            zs.Subscriptions.AssertEqual(
            );
        }

        [TestMethod]
        public void Case_Three()
        {
            var scheduler = new TestScheduler();

            var xs = scheduler.CreateHotObservable(
                OnNext(210, 1),
                OnNext(240, 2),
                OnNext(270, 3),
                OnCompleted<int>(300)
            );

            var ys = scheduler.CreateHotObservable(
                OnNext(220, 11),
                OnNext(250, 12),
                OnNext(280, 13),
                OnCompleted<int>(310)
            );

            var zs = scheduler.CreateHotObservable(
                OnNext(230, 21),
                OnNext(240, 22),
                OnNext(290, 23),
                OnCompleted<int>(320)
            );

            var map = new Dictionary<int, IObservable<int>>
            {
                { 1, xs },
                { 2, ys }
            };

            var results = scheduler.Start(() => Observable.Case(() => 3, map, zs));

            results.Messages.AssertEqual(
                OnNext(230, 21),
                OnNext(240, 22),
                OnNext(290, 23),
                OnCompleted<int>(320)
            );

            xs.Subscriptions.AssertEqual(
            );

            ys.Subscriptions.AssertEqual(
            );

            zs.Subscriptions.AssertEqual(
                Subscribe(200, 320)
            );
        }

        [TestMethod]
        public void Case_Throw()
        {
            var scheduler = new TestScheduler();

            var xs = scheduler.CreateHotObservable(
                OnNext(210, 1),
                OnNext(240, 2),
                OnNext(270, 3),
                OnCompleted<int>(300)
            );

            var ys = scheduler.CreateHotObservable(
                OnNext(220, 11),
                OnNext(250, 12),
                OnNext(280, 13),
                OnCompleted<int>(310)
            );

            var zs = scheduler.CreateHotObservable(
                OnNext(230, 21),
                OnNext(240, 22),
                OnNext(290, 23),
                OnCompleted<int>(320)
            );

            var map = new Dictionary<int, IObservable<int>>
            {
                { 1, xs },
                { 2, ys }
            };

            var ex = new Exception();

            var results = scheduler.Start(() => Observable.Case(() => Throw<int>(ex), map, zs));

            results.Messages.AssertEqual(
                OnError<int>(200, ex)
            );

            xs.Subscriptions.AssertEqual(
            );

            ys.Subscriptions.AssertEqual(
            );

            zs.Subscriptions.AssertEqual(
            );
        }

        [TestMethod]
        public void CaseWithDefault_One()
        {
            var scheduler = new TestScheduler();

            var xs = scheduler.CreateHotObservable(
                OnNext(210, 1),
                OnNext(240, 2),
                OnNext(270, 3),
                OnCompleted<int>(300)
            );

            var ys = scheduler.CreateHotObservable(
                OnNext(220, 11),
                OnNext(250, 12),
                OnNext(280, 13),
                OnCompleted<int>(310)
            );

            var map = new Dictionary<int, IObservable<int>>
            {
                { 1, xs },
                { 2, ys }
            };

            var results = scheduler.Start(() => Observable.Case(() => 1, map, scheduler));

            results.Messages.AssertEqual(
                OnNext(210, 1),
                OnNext(240, 2),
                OnNext(270, 3),
                OnCompleted<int>(300)
            );

            xs.Subscriptions.AssertEqual(
                Subscribe(200, 300)
            );

            ys.Subscriptions.AssertEqual(
            );
        }

        [TestMethod]
        public void CaseWithDefault_Two()
        {
            var scheduler = new TestScheduler();

            var xs = scheduler.CreateHotObservable(
                OnNext(210, 1),
                OnNext(240, 2),
                OnNext(270, 3),
                OnCompleted<int>(300)
            );

            var ys = scheduler.CreateHotObservable(
                OnNext(220, 11),
                OnNext(250, 12),
                OnNext(280, 13),
                OnCompleted<int>(310)
            );

            var map = new Dictionary<int, IObservable<int>>
            {
                { 1, xs },
                { 2, ys }
            };

            var results = scheduler.Start(() => Observable.Case(() => 2, map, scheduler));

            results.Messages.AssertEqual(
                OnNext(220, 11),
                OnNext(250, 12),
                OnNext(280, 13),
                OnCompleted<int>(310)
            );

            xs.Subscriptions.AssertEqual(
            );

            ys.Subscriptions.AssertEqual(
                Subscribe(200, 310)
            );
        }

        [TestMethod]
        public void CaseWithDefault_Three()
        {
            var scheduler = new TestScheduler();

            var xs = scheduler.CreateHotObservable(
                OnNext(210, 1),
                OnNext(240, 2),
                OnNext(270, 3),
                OnCompleted<int>(300)
            );

            var ys = scheduler.CreateHotObservable(
                OnNext(220, 11),
                OnNext(250, 12),
                OnNext(280, 13),
                OnCompleted<int>(310)
            );

            var map = new Dictionary<int, IObservable<int>>
            {
                { 1, xs },
                { 2, ys }
            };

            var results = scheduler.Start(() => Observable.Case(() => 3, map, scheduler));

            results.Messages.AssertEqual(
                OnCompleted<int>(201)
            );

            xs.Subscriptions.AssertEqual(
            );

            ys.Subscriptions.AssertEqual(
            );
        }

        [TestMethod]
        public void CaseWithDefault_Throw()
        {
            var scheduler = new TestScheduler();

            var xs = scheduler.CreateHotObservable(
                OnNext(210, 1),
                OnNext(240, 2),
                OnNext(270, 3),
                OnCompleted<int>(300)
            );

            var ys = scheduler.CreateHotObservable(
                OnNext(220, 11),
                OnNext(250, 12),
                OnNext(280, 13),
                OnCompleted<int>(310)
            );

            var map = new Dictionary<int, IObservable<int>>
            {
                { 1, xs },
                { 2, ys }
            };

            var ex = new Exception();

            var results = scheduler.Start(() => Observable.Case(() => Throw<int>(ex), map, scheduler));

            results.Messages.AssertEqual(
                OnError<int>(200, ex)
            );

            xs.Subscriptions.AssertEqual(
            );

            ys.Subscriptions.AssertEqual(
            );
        }

        [TestMethod]
        public void CaseWithDefault_CheckDefault()
        {
            Observable.Case(() => 1, new Dictionary<int, IObservable<int>>(), DefaultScheduler.Instance)
                .AssertEqual(Observable.Case(() => 1, new Dictionary<int, IObservable<int>>()));
        }

        [TestMethod]
        public void Case_Error()
        {
            var scheduler = new TestScheduler();

            var ex = new Exception();

            var xs = scheduler.CreateHotObservable(
                OnNext(210, 1),
                OnNext(240, 2),
                OnNext(270, 3),
                OnError<int>(300, ex)
            );

            var ys = scheduler.CreateHotObservable(
                OnNext(220, 11),
                OnNext(250, 12),
                OnNext(280, 13),
                OnCompleted<int>(310)
            );

            var map = new Dictionary<int, IObservable<int>>
            {
                { 1, xs },
                { 2, ys }
            };

            var results = scheduler.Start(() => Observable.Case(() => 1, map, scheduler));

            results.Messages.AssertEqual(
                OnNext(210, 1),
                OnNext(240, 2),
                OnNext(270, 3),
                OnError<int>(300, ex)
            );

            xs.Subscriptions.AssertEqual(
                Subscribe(200, 300)
            );

            ys.Subscriptions.AssertEqual(
            );
        }

        #endregion

        #region + DoWhile +

        [TestMethod]
        public void DoWhile_ArgumentChecking()
        {
            ReactiveAssert.Throws<ArgumentNullException>(() => Observable.DoWhile(DummyObservable<int>.Instance, null));
            ReactiveAssert.Throws<ArgumentNullException>(() => Observable.DoWhile(default(IObservable<int>), DummyFunc<bool>.Instance));
        }

        [TestMethod]
        public void DoWhile_AlwaysFalse()
        {
            var scheduler = new TestScheduler();

            var xs = scheduler.CreateColdObservable(
                OnNext(50, 1),
                OnNext(100, 2),
                OnNext(150, 3),
                OnNext(200, 4),
                OnCompleted<int>(250)
            );

            var results = scheduler.Start(() => Observable.DoWhile(xs, () => false));

            results.Messages.AssertEqual(
                OnNext(250, 1),
                OnNext(300, 2),
                OnNext(350, 3),
                OnNext(400, 4),
                OnCompleted<int>(450)
            );

            xs.Subscriptions.AssertEqual(
                Subscribe(200, 450)
            );
        }

        [TestMethod]
        public void DoWhile_AlwaysTrue()
        {
            var scheduler = new TestScheduler();

            var xs = scheduler.CreateColdObservable(
                OnNext(50, 1),
                OnNext(100, 2),
                OnNext(150, 3),
                OnNext(200, 4),
                OnCompleted<int>(250)
            );

            var results = scheduler.Start(() => Observable.DoWhile(xs, () => true));

            results.Messages.AssertEqual(
                OnNext(250, 1),
                OnNext(300, 2),
                OnNext(350, 3),
                OnNext(400, 4),
                OnNext(500, 1),
                OnNext(550, 2),
                OnNext(600, 3),
                OnNext(650, 4),
                OnNext(750, 1),
                OnNext(800, 2),
                OnNext(850, 3),
                OnNext(900, 4)
            );

            xs.Subscriptions.AssertEqual(
                Subscribe(200, 450),
                Subscribe(450, 700),
                Subscribe(700, 950),
                Subscribe(950, 1000)
            );
        }

        [TestMethod]
        public void DoWhile_AlwaysTrue_Throw()
        {
            var scheduler = new TestScheduler();

            var ex = new Exception();

            var xs = scheduler.CreateColdObservable(
                OnError<int>(50, ex)
            );

            var results = scheduler.Start(() => Observable.DoWhile(xs, () => true));

            results.Messages.AssertEqual(
                OnError<int>(250, ex)
            );

            xs.Subscriptions.AssertEqual(
                Subscribe(200, 250)
            );
        }

        [TestMethod]
        public void DoWhile_AlwaysTrue_Infinite()
        {
            var scheduler = new TestScheduler();

            var xs = scheduler.CreateColdObservable(
                OnNext(50, 1)
            );

            var results = scheduler.Start(() => Observable.DoWhile(xs, () => true));

            results.Messages.AssertEqual(
                OnNext(250, 1)
            );

            xs.Subscriptions.AssertEqual(
                Subscribe(200, 1000)
            );
        }

        [TestMethod]
        public void DoWhile_SometimesTrue()
        {
            var scheduler = new TestScheduler();

            var xs = scheduler.CreateColdObservable(
                OnNext(50, 1),
                OnNext(100, 2),
                OnNext(150, 3),
                OnNext(200, 4),
                OnCompleted<int>(250)
            );

            int n = 0;

            var results = scheduler.Start(() => Observable.DoWhile(xs, () => ++n < 3));

            results.Messages.AssertEqual(
                OnNext(250, 1),
                OnNext(300, 2),
                OnNext(350, 3),
                OnNext(400, 4),
                OnNext(500, 1),
                OnNext(550, 2),
                OnNext(600, 3),
                OnNext(650, 4),
                OnNext(750, 1),
                OnNext(800, 2),
                OnNext(850, 3),
                OnNext(900, 4),
                OnCompleted<int>(950)
            );

            xs.Subscriptions.AssertEqual(
                Subscribe(200, 450),
                Subscribe(450, 700),
                Subscribe(700, 950)
            );
        }

        [TestMethod]
        public void DoWhile_SometimesThrows()
        {
            var scheduler = new TestScheduler();

            var xs = scheduler.CreateColdObservable(
                OnNext(50, 1),
                OnNext(100, 2),
                OnNext(150, 3),
                OnNext(200, 4),
                OnCompleted<int>(250)
            );

            int n = 0;

            var ex = new Exception();

            var results = scheduler.Start(() => Observable.DoWhile(xs, () => ++n < 3 ? true : Throw<bool>(ex)));

            results.Messages.AssertEqual(
                OnNext(250, 1),
                OnNext(300, 2),
                OnNext(350, 3),
                OnNext(400, 4),
                OnNext(500, 1),
                OnNext(550, 2),
                OnNext(600, 3),
                OnNext(650, 4),
                OnNext(750, 1),
                OnNext(800, 2),
                OnNext(850, 3),
                OnNext(900, 4),
                OnError<int>(950, ex)
            );

            xs.Subscriptions.AssertEqual(
                Subscribe(200, 450),
                Subscribe(450, 700),
                Subscribe(700, 950)
            );
        }

        #endregion

        #region + For +

        [TestMethod]
        public void For_ArgumentChecking()
        {
            ReactiveAssert.Throws<ArgumentNullException>(() => Observable.For(DummyEnumerable<int>.Instance, default(Func<int, IObservable<int>>)));
            ReactiveAssert.Throws<ArgumentNullException>(() => Observable.For(null, DummyFunc<int, IObservable<int>>.Instance));
        }

        [TestMethod]
        public void For_Basic()
        {
            var scheduler = new TestScheduler();

            var results = scheduler.Start(() => Observable.For(new[] { 1, 2, 3 }, x => scheduler.CreateColdObservable(
                OnNext<int>((ushort)(x * 100 + 10), x * 10 + 1),
                OnNext<int>((ushort)(x * 100 + 20), x * 10 + 2),
                OnNext<int>((ushort)(x * 100 + 30), x * 10 + 3),
                OnCompleted<int>((ushort)(x * 100 + 40)))));

            results.Messages.AssertEqual(
                OnNext(310, 11),
                OnNext(320, 12),
                OnNext(330, 13),
                OnNext(550, 21),
                OnNext(560, 22),
                OnNext(570, 23),
                OnNext(890, 31),
                OnNext(900, 32),
                OnNext(910, 33),
                OnCompleted<int>(920)
            );
        }

        IEnumerable<int> For_Error_Core(Exception ex)
        {
            yield return 1;
            yield return 2;
            yield return 3;
            throw ex;
        }

        [TestMethod]
        public void For_Error_Iterator()
        {
            var scheduler = new TestScheduler();

            var ex = new Exception();

            var results = scheduler.Start(() => Observable.For(For_Error_Core(ex), x => scheduler.CreateColdObservable(
                OnNext<int>((ushort)(x * 100 + 10), x * 10 + 1),
                OnNext<int>((ushort)(x * 100 + 20), x * 10 + 2),
                OnNext<int>((ushort)(x * 100 + 30), x * 10 + 3),
                OnCompleted<int>((ushort)(x * 100 + 40)))));

            results.Messages.AssertEqual(
                OnNext(310, 11),
                OnNext(320, 12),
                OnNext(330, 13),
                OnNext(550, 21),
                OnNext(560, 22),
                OnNext(570, 23),
                OnNext(890, 31),
                OnNext(900, 32),
                OnNext(910, 33),
                OnError<int>(920, ex)
            );
        }

        [TestMethod]
        public void For_Error_Source()
        {
            var scheduler = new TestScheduler();

            var ex = new Exception();

            var results = scheduler.Start(() => Observable.For(new[] { 1, 2, 3 }, x => Observable.Throw<int>(ex)));

            results.Messages.AssertEqual(
                OnError<int>(200, ex)
            );
        }

        [TestMethod]
        public void For_SelectorThrows()
        {
            var scheduler = new TestScheduler();

            var ex = new Exception();

            var results = scheduler.Start(() => Observable.For(new[] { 1, 2, 3 }, x => Throw<IObservable<int>>(ex)));

            results.Messages.AssertEqual(
                OnError<int>(200, ex)
            );
        }

        #endregion

        #region + If +

        [TestMethod]
        public void If_ArgumentChecking()
        {
            ReactiveAssert.Throws<ArgumentNullException>(() => Observable.If(null, DummyObservable<int>.Instance, DummyObservable<int>.Instance));
            ReactiveAssert.Throws<ArgumentNullException>(() => Observable.If(DummyFunc<bool>.Instance, null, DummyObservable<int>.Instance));
            ReactiveAssert.Throws<ArgumentNullException>(() => Observable.If(DummyFunc<bool>.Instance, DummyObservable<int>.Instance, default(IObservable<int>)));
            ReactiveAssert.Throws<ArgumentNullException>(() => Observable.If(null, DummyObservable<int>.Instance, Scheduler.Default));
            ReactiveAssert.Throws<ArgumentNullException>(() => Observable.If(DummyFunc<bool>.Instance, default(IObservable<int>), Scheduler.Default));
            ReactiveAssert.Throws<ArgumentNullException>(() => Observable.If(DummyFunc<bool>.Instance, DummyObservable<int>.Instance, default(IScheduler)));
        }

        [TestMethod]
        public void If_True()
        {
            var scheduler = new TestScheduler();

            var xs = scheduler.CreateHotObservable(
                OnNext(210, 1),
                OnNext(250, 2),
                OnCompleted<int>(300)
            );

            var ys = scheduler.CreateHotObservable(
                OnNext(310, 3),
                OnNext(350, 4),
                OnCompleted<int>(400)
            );

            var results = scheduler.Start(() => Observable.If(() => true, xs, ys));

            results.Messages.AssertEqual(
                OnNext(210, 1),
                OnNext(250, 2),
                OnCompleted<int>(300)
            );

            xs.Subscriptions.AssertEqual(
                Subscribe(200, 300)
            );

            ys.Subscriptions.AssertEqual(
            );
        }

        [TestMethod]
        public void If_False()
        {
            var scheduler = new TestScheduler();

            var xs = scheduler.CreateHotObservable(
                OnNext(210, 1),
                OnNext(250, 2),
                OnCompleted<int>(300)
            );

            var ys = scheduler.CreateHotObservable(
                OnNext(310, 3),
                OnNext(350, 4),
                OnCompleted<int>(400)
            );

            var results = scheduler.Start(() => Observable.If(() => false, xs, ys));

            results.Messages.AssertEqual(
                OnNext(310, 3),
                OnNext(350, 4),
                OnCompleted<int>(400)
            );

            xs.Subscriptions.AssertEqual(
            );

            ys.Subscriptions.AssertEqual(
                Subscribe(200, 400)
            );
        }

        [TestMethod]
        public void If_Throw()
        {
            var scheduler = new TestScheduler();

            var xs = scheduler.CreateHotObservable(
                OnNext(210, 1),
                OnNext(250, 2),
                OnCompleted<int>(300)
            );

            var ys = scheduler.CreateHotObservable(
                OnNext(310, 3),
                OnNext(350, 4),
                OnCompleted<int>(400)
            );

            var ex = new Exception();

            var results = scheduler.Start(() => Observable.If(() => Throw<bool>(ex), xs, ys));

            results.Messages.AssertEqual(
                OnError<int>(200, ex)
            );

            xs.Subscriptions.AssertEqual(
            );

            ys.Subscriptions.AssertEqual(
            );
        }

        [TestMethod]
        public void If_Dispose()
        {
            var scheduler = new TestScheduler();

            var xs = scheduler.CreateHotObservable(
                OnNext(210, 1),
                OnNext(250, 2)
            );

            var ys = scheduler.CreateHotObservable(
                OnNext(310, 3),
                OnNext(350, 4),
                OnCompleted<int>(400)
            );

            var results = scheduler.Start(() => Observable.If(() => true, xs, ys));

            results.Messages.AssertEqual(
                OnNext(210, 1),
                OnNext(250, 2)
            );

            xs.Subscriptions.AssertEqual(
                Subscribe(200, 1000)
            );

            ys.Subscriptions.AssertEqual(
            );
        }

        [TestMethod]
        public void If_Default_ArgumentChecking()
        {
            ReactiveAssert.Throws<ArgumentNullException>(() => Observable.If<int>(null, DummyObservable<int>.Instance));
            ReactiveAssert.Throws<ArgumentNullException>(() => Observable.If<int>(DummyFunc<bool>.Instance, null));
        }

        [TestMethod]
        public void If_Default_Completed()
        {
            var scheduler = new TestScheduler();

            var xs = scheduler.CreateHotObservable(
                OnNext(110, 1),
                OnNext(220, 2),
                OnNext(330, 3),
                OnCompleted<int>(440)
            );

            var b = false;

            scheduler.ScheduleAbsolute(150, () => b = true);

            var results = scheduler.Start(() => Observable.If(() => b, xs));

            results.Messages.AssertEqual(
                OnNext(220, 2),
                OnNext(330, 3),
                OnCompleted<int>(440)
            );

            xs.Subscriptions.AssertEqual(
                Subscribe(200, 440)
            );
        }

        [TestMethod]
        public void If_Default_Error()
        {
            var scheduler = new TestScheduler();

            var ex = new Exception();

            var xs = scheduler.CreateHotObservable(
                OnNext(110, 1),
                OnNext(220, 2),
                OnNext(330, 3),
                OnError<int>(440, ex)
            );

            var b = false;

            scheduler.ScheduleAbsolute(150, () => b = true);

            var results = scheduler.Start(() => Observable.If(() => b, xs));

            results.Messages.AssertEqual(
                OnNext(220, 2),
                OnNext(330, 3),
                OnError<int>(440, ex)
            );

            xs.Subscriptions.AssertEqual(
                Subscribe(200, 440)
            );
        }

        [TestMethod]
        public void If_Default_Never()
        {
            var scheduler = new TestScheduler();

            var xs = scheduler.CreateHotObservable(
                OnNext(110, 1),
                OnNext(220, 2),
                OnNext(330, 3)
            );

            var b = false;

            scheduler.ScheduleAbsolute(150, () => b = true);

            var results = scheduler.Start(() => Observable.If(() => b, xs));

            results.Messages.AssertEqual(
                OnNext(220, 2),
                OnNext(330, 3)
            );

            xs.Subscriptions.AssertEqual(
                Subscribe(200, 1000)
            );
        }

        [TestMethod]
        public void If_Default_Other()
        {
            var scheduler = new TestScheduler();

            var xs = scheduler.CreateHotObservable(
                OnNext(110, 1),
                OnNext(220, 2),
                OnNext(330, 3),
                OnError<int>(440, new Exception())
            );

            var b = true;

            scheduler.ScheduleAbsolute(150, () => b = false);

            var results = scheduler.Start(() => Observable.If(() => b, xs));

            results.Messages.AssertEqual(
                OnCompleted<int>(200)
            );

            xs.Subscriptions.AssertEqual(
            );
        }

        [TestMethod]
        public void If_Default_Scheduler()
        {
            var scheduler = new TestScheduler();

            var xs = scheduler.CreateHotObservable(
                OnNext(110, 1),
                OnNext(220, 2),
                OnNext(330, 3),
                OnError<int>(440, new Exception())
            );

            var results = scheduler.Start(() => Observable.If(() => false, xs, scheduler));

            results.Messages.AssertEqual(
                OnCompleted<int>(201)
            );

            xs.Subscriptions.AssertEqual(
            );
        }

        #endregion

        #region + While +

        [TestMethod]
        public void While_ArgumentChecking()
        {
            ReactiveAssert.Throws<ArgumentNullException>(() => Observable.While(default(Func<bool>), DummyObservable<int>.Instance));
            ReactiveAssert.Throws<ArgumentNullException>(() => Observable.While(DummyFunc<bool>.Instance, default(IObservable<int>)));
        }

        [TestMethod]
        public void While_AlwaysFalse()
        {
            var scheduler = new TestScheduler();

            var xs = scheduler.CreateColdObservable(
                OnNext(50, 1),
                OnNext(100, 2),
                OnNext(150, 3),
                OnNext(200, 4),
                OnCompleted<int>(250)
            );

            var results = scheduler.Start(() => Observable.While(() => false, xs));

            results.Messages.AssertEqual(
                OnCompleted<int>(200)
            );

            xs.Subscriptions.AssertEqual(
            );
        }

        [TestMethod]
        public void While_AlwaysTrue()
        {
            var scheduler = new TestScheduler();

            var xs = scheduler.CreateColdObservable(
                OnNext(50, 1),
                OnNext(100, 2),
                OnNext(150, 3),
                OnNext(200, 4),
                OnCompleted<int>(250)
            );

            var results = scheduler.Start(() => Observable.While(() => true, xs));

            results.Messages.AssertEqual(
                OnNext(250, 1),
                OnNext(300, 2),
                OnNext(350, 3),
                OnNext(400, 4),
                OnNext(500, 1),
                OnNext(550, 2),
                OnNext(600, 3),
                OnNext(650, 4),
                OnNext(750, 1),
                OnNext(800, 2),
                OnNext(850, 3),
                OnNext(900, 4)
            );

            xs.Subscriptions.AssertEqual(
                Subscribe(200, 450),
                Subscribe(450, 700),
                Subscribe(700, 950),
                Subscribe(950, 1000)
            );
        }

        [TestMethod]
        public void While_AlwaysTrue_Throw()
        {
            var scheduler = new TestScheduler();

            var ex = new Exception();

            var xs = scheduler.CreateColdObservable(
                OnError<int>(50, ex)
            );

            var results = scheduler.Start(() => Observable.While(() => true, xs));

            results.Messages.AssertEqual(
                OnError<int>(250, ex)
            );

            xs.Subscriptions.AssertEqual(
                Subscribe(200, 250)
            );
        }

        [TestMethod]
        public void While_AlwaysTrue_Infinite()
        {
            var scheduler = new TestScheduler();

            var xs = scheduler.CreateColdObservable(
                OnNext(50, 1)
            );

            var results = scheduler.Start(() => Observable.While(() => true, xs));

            results.Messages.AssertEqual(
                OnNext(250, 1)
            );

            xs.Subscriptions.AssertEqual(
                Subscribe(200, 1000)
            );
        }

        [TestMethod]
        public void While_SometimesTrue()
        {
            var scheduler = new TestScheduler();

            var xs = scheduler.CreateColdObservable(
                OnNext(50, 1),
                OnNext(100, 2),
                OnNext(150, 3),
                OnNext(200, 4),
                OnCompleted<int>(250)
            );

            int n = 0;

            var results = scheduler.Start(() => Observable.While(() => ++n < 3, xs));

            results.Messages.AssertEqual(
                OnNext(250, 1),
                OnNext(300, 2),
                OnNext(350, 3),
                OnNext(400, 4),
                OnNext(500, 1),
                OnNext(550, 2),
                OnNext(600, 3),
                OnNext(650, 4),
                OnCompleted<int>(700)
            );

            xs.Subscriptions.AssertEqual(
                Subscribe(200, 450),
                Subscribe(450, 700)
            );
        }

        static T Throw<T>(Exception ex)
        {
            throw ex;
        }

        [TestMethod]
        public void While_SometimesThrows()
        {
            var scheduler = new TestScheduler();

            var xs = scheduler.CreateColdObservable(
                OnNext(50, 1),
                OnNext(100, 2),
                OnNext(150, 3),
                OnNext(200, 4),
                OnCompleted<int>(250)
            );

            int n = 0;

            var ex = new Exception();

            var results = scheduler.Start(() => Observable.While(() => ++n < 3 ? true : Throw<bool>(ex), xs));

            results.Messages.AssertEqual(
                OnNext(250, 1),
                OnNext(300, 2),
                OnNext(350, 3),
                OnNext(400, 4),
                OnNext(500, 1),
                OnNext(550, 2),
                OnNext(600, 3),
                OnNext(650, 4),
                OnError<int>(700, ex)
            );

            xs.Subscriptions.AssertEqual(
                Subscribe(200, 450),
                Subscribe(450, 700)
            );
        }

        #endregion

        #region General tests for loops

        [TestMethod]
        public void LoopTest1()
        {
            var loop = Observable.Defer(() =>
            {
                var n = 0;
                return Observable.While(
                    () => n++ < 5,
                    Observable.Defer(() =>
                    {
                        return Observable.For(
                            Enumerable.Range(0, n),
                            x => Observable.Return(x)
                        );
                    })
                );
            });

            var res = new List<int>();
            var std = new List<int>();
            loop.ForEach(x =>
            {
                res.Add(x);
                std.Add(new System.Diagnostics.StackTrace().FrameCount);
            });

            Assert.IsTrue(res.SequenceEqual(new[] { 0, 0, 1, 0, 1, 2, 0, 1, 2, 3, 0, 1, 2, 3, 4 }));
            Assert.IsTrue(std.Distinct().Count() == 1);
        }

        [TestMethod]
        public void LoopTest2()
        {
            var n = 0;

            var loop = default(IObservable<int>);
            loop = Observable.While(
                () => n++ < 10,
                Observable.Concat(
                    Observable.Return(42),
                    Observable.Defer(() => loop)
                )
            );

            var res = new List<int>();
            var std = new List<int>();
            loop.ForEach(x =>
            {
                res.Add(x);
                std.Add(new System.Diagnostics.StackTrace().FrameCount);
            });

            Assert.IsTrue(res.SequenceEqual(Enumerable.Repeat(42, 10)));
            Assert.IsTrue(std.Distinct().Count() == 1);
        }

        #endregion
    }
}

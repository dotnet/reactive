// Copyright (c) Microsoft Open Technologies, Inc. All rights reserved. See License.txt in the project root for license information.

using System;
using System.Collections.Generic;
using System.Linq;
using System.Reactive;
using System.Reactive.Concurrency;
using System.Reactive.Linq;
using System.Reactive.Subjects;
using System.Threading;
using Microsoft.Reactive.Testing;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace ReactiveTests.Tests
{
    [TestClass]
    public class RegressionTest : ReactiveTest
    {
#if DESKTOPCLR40 || DESKTOPCLR45
        [TestMethod]
        public void Bug_ConcurrentMerge()
        {
            const int reps = 1000;
            var source = Enumerable.Range(0, reps).ToObservable();

            var resultQueue = new System.Collections.Concurrent.ConcurrentQueue<int>();
            var r = new Random();

            source.Select(i => Observable.Create<Unit>(o =>
            {
                resultQueue.Enqueue(i);
                System.Threading.Tasks.Task.Factory.StartNew(
                    () =>
                    {
                        Thread.Sleep(r.Next(10));
                        o.OnCompleted();
                    });
                return () => { };
            })).Merge(3).ForEach(_ => { });

            Assert.IsTrue(Enumerable.Range(0, reps).ToList().SequenceEqual(resultQueue.ToList()));
        }
#endif

        [TestMethod]
        public void Bug_1283()
        {
            var scheduler = new TestScheduler();

            var xs = scheduler.CreateHotObservable(
                OnNext(100, 1),
                OnNext(220, 2),
                OnNext(240, 3),
                OnNext(300, 4),
                OnNext(310, 5),
                OnCompleted<int>(350)
            );

            var results = scheduler.Start(() =>
                xs.Window(TimeSpan.FromTicks(100), scheduler).Select((x, i) => x.Select(y => i.ToString() + " " + y.ToString()).Concat(Observable.Return(i.ToString() + " end", scheduler))).Merge()
            );

            results.Messages.AssertEqual(
                OnNext(220, "0 2"),
                OnNext(240, "0 3"),
                OnNext(300, "0 4"),
                OnNext(301, "0 end"),
                OnNext(310, "1 5"),
                OnNext(351, "1 end"),
                OnCompleted<string>(351)
            );
        }

        [TestMethod]
        public void Bug_1261()
        {
            var scheduler = new TestScheduler();

            var xs = scheduler.CreateHotObservable(
                OnNext(205, 1),
                OnNext(210, 2),
                OnNext(215, 3),
                OnNext(220, 4),
                OnNext(225, 5),
                OnNext(230, 6),
                OnCompleted<int>(230));

            var results = scheduler.Start(() =>
                xs.Window(TimeSpan.FromTicks(10), scheduler).Select((x, i) => x.Select(y => i.ToString() + " " + y.ToString()).Concat(Observable.Return(i.ToString() + " end", scheduler))).Merge()
            );

            results.Messages.AssertEqual(
                OnNext(205, "0 1"),
                OnNext(210, "0 2"),
                OnNext(211, "0 end"),
                OnNext(215, "1 3"),
                OnNext(220, "1 4"),
                OnNext(221, "1 end"),
                OnNext(225, "2 5"),
                OnNext(230, "2 6"),
                OnNext(231, "2 end"),
                OnCompleted<string>(231)
            );
        }

        [TestMethod]
        public void Bug_1130()
        {
            var xs = Observable.Start(() => 5);
            Assert.IsNull(xs as ISubject<int, int>);
        }

        [TestMethod]
        public void Bug_1286()
        {
            var infinite = Observable.Return(new { Name = "test", Value = 0d }, DefaultScheduler.Instance).Repeat();
            var grouped = infinite.GroupBy(x => x.Name, x => x.Value);
            var disp = grouped.Subscribe(_ => { });
            Thread.Sleep(1);
            //most of the time, this deadlocks
            disp.Dispose();
            disp = grouped.Subscribe(_ => { });
            Thread.Sleep(1);
            //if the first doesn't this one always
            disp.Dispose();
        }

        [TestMethod]
        public void Bug_1287()
        {
            var flag = false;
            var x = Observable.Return(1, Scheduler.CurrentThread).Concat(Observable.Never<int>()).Finally(() => flag = true).First();
            Assert.AreEqual(1, x);
            Assert.IsTrue(flag);
        }

#if !SILVERLIGHTM7
        static IEnumerable<int> Bug_1333_Enumerable(AsyncSubject<IDisposable> s, Semaphore sema)
        {
            var d = s.First();
            var t = new Thread(() => { d.Dispose(); sema.Release(); });
            t.Start();
            t.Join();
            yield return 1;
        }

        [TestMethod]
        [Timeout(1000)]
        public void Bug_1333()
        {
            var sema = new Semaphore(0, 1);
            var d = new AsyncSubject<IDisposable>();
            var e = Bug_1333_Enumerable(d, sema).ToObservable(DefaultScheduler.Instance).Subscribe();
            d.OnNext(e);
            d.OnCompleted();
            sema.WaitOne();
        }
#endif
        [TestMethod]
        public void Bug_1295_Completed()
        {
            var scheduler = new TestScheduler();

            var xs = scheduler.CreateHotObservable(
                OnNext(300, 1),
                OnNext(350, 2),
                OnNext(500, 3),
                OnCompleted<int>(550)
            );

            var results = scheduler.Start(() =>
                xs.Throttle(TimeSpan.FromTicks(100), scheduler)
            );

            results.Messages.AssertEqual(
                OnNext(450, 2),
                OnNext(550, 3),
                OnCompleted<int>(550)
            );

            xs.Subscriptions.AssertEqual(
                Subscribe(200, 550)
            );
        }

        [TestMethod]
        public void Bug_1295_Error()
        {
            var scheduler = new TestScheduler();

            var ex = new Exception();

            var xs = scheduler.CreateHotObservable(
                OnNext(300, 1),
                OnNext(350, 2),
                OnNext(500, 3),
                OnError<int>(550, ex)
            );

            var results = scheduler.Start(() =>
                xs.Throttle(TimeSpan.FromTicks(100), scheduler)
            );

            results.Messages.AssertEqual(
                OnNext(450, 2),
                OnError<int>(550, ex)
            );

            xs.Subscriptions.AssertEqual(
                Subscribe(200, 550)
            );
        }

        [TestMethod]
        public void Bug_1297_Catch_None()
        {
            var scheduler = new TestScheduler();

            var results = scheduler.Start(() =>
                Observable.Catch<int>()
            );

            results.Messages.AssertEqual(
                OnCompleted<int>(200)
            );
        }

        [TestMethod]
        public void Bug_1297_OnErrorResumeNext_None()
        {
            var scheduler = new TestScheduler();

            var results = scheduler.Start(() =>
                Observable.OnErrorResumeNext<int>()
            );

            results.Messages.AssertEqual(
                OnCompleted<int>(200)
            );
        }

        [TestMethod]
        public void Bug_1297_Catch_Single()
        {
            var scheduler = new TestScheduler();

            var ex = new Exception();

            var xs = Observable.Throw<int>(ex, scheduler);

            var results = scheduler.Start(() =>
                Observable.Catch(xs)
            );

            results.Messages.AssertEqual(
                OnError<int>(201, ex)
            );
        }

        [TestMethod]
        public void Bug_1297_OnErrorResumeNext_Single()
        {
            var scheduler = new TestScheduler();

            var xs = Observable.Throw<int>(new Exception(), scheduler);

            var results = scheduler.Start(() =>
                Observable.OnErrorResumeNext(xs)
            );

            results.Messages.AssertEqual(
                OnCompleted<int>(201)
            );
        }

        [TestMethod]
        public void Bug_1297_Catch_Multi()
        {
            var scheduler = new TestScheduler();

            var ex1 = new Exception();
            var ex2 = new Exception();
            var ex3 = new Exception();

            var xs = Observable.Throw<int>(ex1, scheduler);
            var ys = Observable.Throw<int>(ex2, scheduler);
            var zs = Observable.Throw<int>(ex3, scheduler);

            var results = scheduler.Start(() =>
                Observable.Catch(xs, ys, zs)
            );

            results.Messages.AssertEqual(
                OnError<int>(203, ex3)
            );
        }

        [TestMethod]
        public void Bug_1297_OnErrorResumeNext_Multi()
        {
            var scheduler = new TestScheduler();

            var ex1 = new Exception();
            var ex2 = new Exception();
            var ex3 = new Exception();

            var xs = Observable.Throw<int>(ex1, scheduler);
            var ys = Observable.Throw<int>(ex2, scheduler);
            var zs = Observable.Throw<int>(ex3, scheduler);

            var results = scheduler.Start(() =>
                Observable.OnErrorResumeNext(xs, ys, zs)
            );

            results.Messages.AssertEqual(
                OnCompleted<int>(203)
            );
        }

        [TestMethod]
        public void Bug_1380()
        {
            var scheduler = new TestScheduler();

            var ex = new Exception();

            var xs = scheduler.CreateHotObservable(
                OnNext(220, 1),
                OnNext(250, 2),
                OnNext(270, 3),
                OnNext(290, 4),
                OnNext(310, 5),
                OnNext(340, 6),
                OnNext(360, 7),
                OnError<int>(380, ex)
            );

            var results = scheduler.Start(() =>
                xs.Delay(TimeSpan.FromTicks(100), scheduler)
            );

            results.Messages.AssertEqual(
                OnNext(320, 1),
                OnNext(350, 2),
                OnNext(370, 3),
                OnError<int>(380, ex)
            );

            xs.Subscriptions.AssertEqual(
                Subscribe(200, 380)
            );
        }


        [TestMethod]
        public void Bug_1356()
        {
            var run = false;
            Observable.Range(0, 10).Finally(() => run = true).Take(5).ForEach(_ => { });
            Assert.IsTrue(run);
        }

        [TestMethod]
        public void Bug_1381()
        {
            var scheduler = new TestScheduler();

            var xs = scheduler.CreateHotObservable(
                OnNext( 90, 1),
                OnNext(110, 2),
                OnNext(250, 3),
                OnNext(270, 4),
                OnNext(280, 5),
                OnNext(301, 6),
                OnNext(302, 7),
                OnNext(400, 8),
                OnNext(401, 9),
                OnNext(510, 10)
            );

            var results = scheduler.CreateObserver<int>();
            var ys = default(IConnectableObservable<int>);
            var connection = default(IDisposable);
            var subscription = default(IDisposable);

            scheduler.ScheduleAbsolute(100, () => ys = xs.Multicast(new ReplaySubject<int>(scheduler)));
            scheduler.ScheduleAbsolute(200, () => connection = ys.Connect());
            scheduler.ScheduleAbsolute(300, () => subscription = ys.Subscribe(results));
            scheduler.ScheduleAbsolute(500, () => subscription.Dispose());
            scheduler.ScheduleAbsolute(600, () => connection.Dispose());

            scheduler.Start();

            results.Messages.AssertEqual(
                OnNext(301, 3),
                OnNext(302, 4),
                OnNext(303, 5),
                OnNext(304, 6),
                OnNext(305, 7),
                OnNext(401, 8),
                OnNext(402, 9)
            );

            xs.Subscriptions.AssertEqual(
                Subscribe(200, 600)
            );
        }

        [TestMethod]
        public void Reentrant_Subject()
        {
            var s = Subject.Synchronize(new Subject<int>(), Scheduler.Immediate);
            var list = new List<int>();

            s.Subscribe(
                x =>
                {
                    list.Add(x);
                    if (x < 3)
                        s.OnNext(x + 1);
                    list.Add(-x);
                });

            s.OnNext(1);

            list.AssertEqual(1, -1, 2, -2, 3, -3);
        }

        [TestMethod]
        public void Merge_Trampoline1()
        {
            var ys = new[] { 1, 2, 3 }.ToObservable().Publish(xs => xs.Merge(xs));

            var list = new List<int>();
            ys.Subscribe(list.Add);

            list.AssertEqual(1, 1, 2, 2, 3, 3);
        }

        [TestMethod]
        public void Merge_Trampoline2()
        {
            var ys = new[] { 1, 2, 3 }.ToObservable().Publish(xs => Observable.Merge(xs, xs, xs, xs));

            var list = new List<int>();
            ys.Subscribe(list.Add);

            list.AssertEqual(1, 1, 1, 1, 2, 2, 2, 2, 3, 3, 3, 3);
        }
    }
}

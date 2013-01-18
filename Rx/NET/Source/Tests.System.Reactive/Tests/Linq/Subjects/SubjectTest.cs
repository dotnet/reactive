// Copyright (c) Microsoft Open Technologies, Inc. All rights reserved. See License.txt in the project root for license information.

using System;
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
    public partial class SubjectTest : ReactiveTest
    {
        [TestMethod]
        public void Subscribe_ArgumentChecking()
        {
            ReactiveAssert.Throws<ArgumentNullException>(() => new Subject<int>().Subscribe(null));
        }

        [TestMethod]
        public void OnError_ArgumentChecking()
        {
            ReactiveAssert.Throws<ArgumentNullException>(() => new Subject<int>().OnError(null));
        }

        [TestMethod]
        public void Infinite()
        {
            var scheduler = new TestScheduler();

            var xs = scheduler.CreateHotObservable(
                OnNext(70, 1),
                OnNext(110, 2),
                OnNext(220, 3),
                OnNext(270, 4),
                OnNext(340, 5),
                OnNext(410, 6),
                OnNext(520, 7),
                OnNext(630, 8),
                OnNext(710, 9),
                OnNext(870, 10),
                OnNext(940, 11),
                OnNext(1020, 12)
            );

            var s = default(Subject<int>);
            var subscription = default(IDisposable);

            var results1 = scheduler.CreateObserver<int>();
            var subscription1 = default(IDisposable);

            var results2 = scheduler.CreateObserver<int>();
            var subscription2 = default(IDisposable);

            var results3 = scheduler.CreateObserver<int>();
            var subscription3 = default(IDisposable);

            scheduler.ScheduleAbsolute(100, () => s = new Subject<int>());
            scheduler.ScheduleAbsolute(200, () => subscription = xs.Subscribe(s));
            scheduler.ScheduleAbsolute(1000, () => subscription.Dispose());

            scheduler.ScheduleAbsolute(300, () => subscription1 = s.Subscribe(results1));
            scheduler.ScheduleAbsolute(400, () => subscription2 = s.Subscribe(results2));
            scheduler.ScheduleAbsolute(900, () => subscription3 = s.Subscribe(results3));

            scheduler.ScheduleAbsolute(600, () => subscription1.Dispose());
            scheduler.ScheduleAbsolute(700, () => subscription2.Dispose());
            scheduler.ScheduleAbsolute(800, () => subscription1.Dispose());
            scheduler.ScheduleAbsolute(950, () => subscription3.Dispose());

            scheduler.Start();

            results1.Messages.AssertEqual(
                OnNext(340, 5),
                OnNext(410, 6),
                OnNext(520, 7)
            );

            results2.Messages.AssertEqual(
                OnNext(410, 6),
                OnNext(520, 7),
                OnNext(630, 8)
            );

            results3.Messages.AssertEqual(
                OnNext(940, 11)
            );
        }

        [TestMethod]
        public void Finite()
        {
            var scheduler = new TestScheduler();

            var xs = scheduler.CreateHotObservable(
                OnNext(70, 1),
                OnNext(110, 2),
                OnNext(220, 3),
                OnNext(270, 4),
                OnNext(340, 5),
                OnNext(410, 6),
                OnNext(520, 7),
                OnCompleted<int>(630),
                OnNext(640, 9),
                OnCompleted<int>(650),
                OnError<int>(660, new Exception())
            );

            var s = default(Subject<int>);
            var subscription = default(IDisposable);

            var results1 = scheduler.CreateObserver<int>();
            var subscription1 = default(IDisposable);

            var results2 = scheduler.CreateObserver<int>();
            var subscription2 = default(IDisposable);

            var results3 = scheduler.CreateObserver<int>();
            var subscription3 = default(IDisposable);

            scheduler.ScheduleAbsolute(100, () => s = new Subject<int>());
            scheduler.ScheduleAbsolute(200, () => subscription = xs.Subscribe(s));
            scheduler.ScheduleAbsolute(1000, () => subscription.Dispose());

            scheduler.ScheduleAbsolute(300, () => subscription1 = s.Subscribe(results1));
            scheduler.ScheduleAbsolute(400, () => subscription2 = s.Subscribe(results2));
            scheduler.ScheduleAbsolute(900, () => subscription3 = s.Subscribe(results3));

            scheduler.ScheduleAbsolute(600, () => subscription1.Dispose());
            scheduler.ScheduleAbsolute(700, () => subscription2.Dispose());
            scheduler.ScheduleAbsolute(800, () => subscription1.Dispose());
            scheduler.ScheduleAbsolute(950, () => subscription3.Dispose());

            scheduler.Start();

            results1.Messages.AssertEqual(
                OnNext(340, 5),
                OnNext(410, 6),
                OnNext(520, 7)
            );

            results2.Messages.AssertEqual(
                OnNext(410, 6),
                OnNext(520, 7),
                OnCompleted<int>(630)
            );

            results3.Messages.AssertEqual(
                OnCompleted<int>(900)
            );
        }

        [TestMethod]
        public void Error()
        {
            var scheduler = new TestScheduler();

            var ex = new Exception();

            var xs = scheduler.CreateHotObservable(
                OnNext(70, 1),
                OnNext(110, 2),
                OnNext(220, 3),
                OnNext(270, 4),
                OnNext(340, 5),
                OnNext(410, 6),
                OnNext(520, 7),
                OnError<int>(630, ex),
                OnNext(640, 9),
                OnCompleted<int>(650),
                OnError<int>(660, new Exception())
            );

            var s = default(Subject<int>);
            var subscription = default(IDisposable);

            var results1 = scheduler.CreateObserver<int>();
            var subscription1 = default(IDisposable);

            var results2 = scheduler.CreateObserver<int>();
            var subscription2 = default(IDisposable);

            var results3 = scheduler.CreateObserver<int>();
            var subscription3 = default(IDisposable);

            scheduler.ScheduleAbsolute(100, () => s = new Subject<int>());
            scheduler.ScheduleAbsolute(200, () => subscription = xs.Subscribe(s));
            scheduler.ScheduleAbsolute(1000, () => subscription.Dispose());

            scheduler.ScheduleAbsolute(300, () => subscription1 = s.Subscribe(results1));
            scheduler.ScheduleAbsolute(400, () => subscription2 = s.Subscribe(results2));
            scheduler.ScheduleAbsolute(900, () => subscription3 = s.Subscribe(results3));

            scheduler.ScheduleAbsolute(600, () => subscription1.Dispose());
            scheduler.ScheduleAbsolute(700, () => subscription2.Dispose());
            scheduler.ScheduleAbsolute(800, () => subscription1.Dispose());
            scheduler.ScheduleAbsolute(950, () => subscription3.Dispose());

            scheduler.Start();

            results1.Messages.AssertEqual(
                OnNext(340, 5),
                OnNext(410, 6),
                OnNext(520, 7)
            );

            results2.Messages.AssertEqual(
                OnNext(410, 6),
                OnNext(520, 7),
                OnError<int>(630, ex)
            );

            results3.Messages.AssertEqual(
                OnError<int>(900, ex)
            );
        }

        [TestMethod]
        public void Canceled()
        {
            var scheduler = new TestScheduler();

            var xs = scheduler.CreateHotObservable(
                OnCompleted<int>(630),
                OnNext(640, 9),
                OnCompleted<int>(650),
                OnError<int>(660, new Exception())
            );

            var s = default(Subject<int>);
            var subscription = default(IDisposable);

            var results1 = scheduler.CreateObserver<int>();
            var subscription1 = default(IDisposable);

            var results2 = scheduler.CreateObserver<int>();
            var subscription2 = default(IDisposable);

            var results3 = scheduler.CreateObserver<int>();
            var subscription3 = default(IDisposable);

            scheduler.ScheduleAbsolute(100, () => s = new Subject<int>());
            scheduler.ScheduleAbsolute(200, () => subscription = xs.Subscribe(s));
            scheduler.ScheduleAbsolute(1000, () => subscription.Dispose());

            scheduler.ScheduleAbsolute(300, () => subscription1 = s.Subscribe(results1));
            scheduler.ScheduleAbsolute(400, () => subscription2 = s.Subscribe(results2));
            scheduler.ScheduleAbsolute(900, () => subscription3 = s.Subscribe(results3));

            scheduler.ScheduleAbsolute(600, () => subscription1.Dispose());
            scheduler.ScheduleAbsolute(700, () => subscription2.Dispose());
            scheduler.ScheduleAbsolute(800, () => subscription1.Dispose());
            scheduler.ScheduleAbsolute(950, () => subscription3.Dispose());

            scheduler.Start();

            results1.Messages.AssertEqual(
            );

            results2.Messages.AssertEqual(
                OnCompleted<int>(630)
            );

            results3.Messages.AssertEqual(
                OnCompleted<int>(900)
            );
        }

        [TestMethod]
        public void Dispose()
        {
            var scheduler = new TestScheduler();

            var s = new Subject<int>();

            scheduler.ScheduleAbsolute(300, () => s.OnNext(1));
            scheduler.ScheduleAbsolute(998, () => s.OnNext(2));
            scheduler.ScheduleAbsolute(999, () => s.OnNext(3));
            scheduler.ScheduleAbsolute(1001, () => s.OnNext(3));

            var results = scheduler.Start(() => s);

            results.Messages.AssertEqual(
                OnNext(300, 1),
                OnNext(998, 2),
                OnNext(999, 3)
            );
        }

        [TestMethod]
        public void PreComplete()
        {
            var scheduler = new TestScheduler();

            var s = new Subject<int>();

            scheduler.ScheduleAbsolute(90, () => s.OnCompleted());

            var results = scheduler.Start(() => s);

            results.Messages.AssertEqual(
                OnCompleted<int>(200)
            );
        }

        [TestMethod]
        public void SubjectDisposed()
        {
            var scheduler = new TestScheduler();

            var subject = default(Subject<int>);

            var results1 = scheduler.CreateObserver<int>();
            var subscription1 = default(IDisposable);

            var results2 = scheduler.CreateObserver<int>();
            var subscription2 = default(IDisposable);

            var results3 = scheduler.CreateObserver<int>();
            var subscription3 = default(IDisposable);

            scheduler.ScheduleAbsolute(100, () => subject = new Subject<int>());
            scheduler.ScheduleAbsolute(200, () => subscription1 = subject.Subscribe(results1));
            scheduler.ScheduleAbsolute(300, () => subscription2 = subject.Subscribe(results2));
            scheduler.ScheduleAbsolute(400, () => subscription3 = subject.Subscribe(results3));
            scheduler.ScheduleAbsolute(500, () => subscription1.Dispose());
            scheduler.ScheduleAbsolute(600, () => subject.Dispose());
            scheduler.ScheduleAbsolute(700, () => subscription2.Dispose());
            scheduler.ScheduleAbsolute(800, () => subscription3.Dispose());

            scheduler.ScheduleAbsolute(150, () => subject.OnNext(1));
            scheduler.ScheduleAbsolute(250, () => subject.OnNext(2));
            scheduler.ScheduleAbsolute(350, () => subject.OnNext(3));
            scheduler.ScheduleAbsolute(450, () => subject.OnNext(4));
            scheduler.ScheduleAbsolute(550, () => subject.OnNext(5));
            scheduler.ScheduleAbsolute(650, () => ReactiveAssert.Throws<ObjectDisposedException>(() => subject.OnNext(6)));
            scheduler.ScheduleAbsolute(750, () => ReactiveAssert.Throws<ObjectDisposedException>(() => subject.OnCompleted()));
            scheduler.ScheduleAbsolute(850, () => ReactiveAssert.Throws<ObjectDisposedException>(() => subject.OnError(new Exception())));
            scheduler.ScheduleAbsolute(950, () => ReactiveAssert.Throws<ObjectDisposedException>(() => subject.Subscribe()));

            scheduler.Start();

            results1.Messages.AssertEqual(
                OnNext(250, 2),
                OnNext(350, 3),
                OnNext(450, 4)
            );

            results2.Messages.AssertEqual(
                OnNext(350, 3),
                OnNext(450, 4),
                OnNext(550, 5)
            );

            results3.Messages.AssertEqual(
                OnNext(450, 4),
                OnNext(550, 5)
            );
        }

        [TestMethod]
        public void Subject_Create_ArgumentChecking()
        {
            ReactiveAssert.Throws<ArgumentNullException>(() => Subject.Create<int, int>(null, Observable.Return(42)));
            ReactiveAssert.Throws<ArgumentNullException>(() => Subject.Create<int, int>(Observer.Create<int>(x => {}), null));
        }

        [TestMethod]
        public void Subject_Create()
        {
            var _x = default(int);
            var _ex = default(Exception);
            var done = false;

            var v = Observer.Create<int>(x => _x = x, ex => _ex = ex, () => done = true);
            var o = Observable.Return(42);

            var s = Subject.Create<int, int>(v, o);

            ReactiveAssert.Throws<ArgumentNullException>(() => s.Subscribe(null));
            s.Subscribe(x => _x = x);
            Assert.AreEqual(42, _x);

            s.OnNext(21);
            Assert.AreEqual(21, _x);

            ReactiveAssert.Throws<ArgumentNullException>(() => s.OnError(null));
            var e = new Exception();
            s.OnError(e);
            Assert.AreSame(e, _ex);

            s.OnCompleted();
            Assert.IsFalse(done); // already cut off
        }

        [TestMethod]
        public void Subject_Synchronize_ArgumentChecking()
        {
            var s = new Subject<int>();

            ReactiveAssert.Throws<ArgumentNullException>(() => Subject.Synchronize(default(ISubject<int, int>)));
            ReactiveAssert.Throws<ArgumentNullException>(() => Subject.Synchronize(default(ISubject<int, int>), Scheduler.Immediate));
            ReactiveAssert.Throws<ArgumentNullException>(() => Subject.Synchronize(s, null));
        }

        [TestMethod]
        public void Subject_Synchronize()
        {
            var N = 10;

            var y = 0;
            var o = Observer.Create<int>(x => y += x);
            var s = Subject.Synchronize(Subject.Create(o, Observable.Empty<string>()));

            var e = new ManualResetEvent(false);
            var t = Enumerable.Range(0, N).Select(x => new Thread(() => { e.WaitOne(); s.OnNext(x); })).ToArray();

            foreach (var u in t)
                u.Start();

            e.Set();

            foreach (var u in t)
                u.Join();

            Assert.AreEqual(Enumerable.Range(0, N).Sum(), y);
        }

        [TestMethod]
        public void HasObservers()
        {
            var s = new Subject<int>();
            Assert.IsFalse(s.HasObservers);

            var d1 = s.Subscribe(_ => { });
            Assert.IsTrue(s.HasObservers);

            d1.Dispose();
            Assert.IsFalse(s.HasObservers);

            var d2 = s.Subscribe(_ => { });
            Assert.IsTrue(s.HasObservers);

            var d3 = s.Subscribe(_ => { });
            Assert.IsTrue(s.HasObservers);

            d2.Dispose();
            Assert.IsTrue(s.HasObservers);

            d3.Dispose();
            Assert.IsFalse(s.HasObservers);
        }

        [TestMethod]
        public void HasObservers_Dispose1()
        {
            var s = new Subject<int>();
            Assert.IsFalse(s.HasObservers);

            var d = s.Subscribe(_ => { });
            Assert.IsTrue(s.HasObservers);

            s.Dispose();
            Assert.IsFalse(s.HasObservers);

            d.Dispose();
            Assert.IsFalse(s.HasObservers);
        }

        [TestMethod]
        public void HasObservers_Dispose2()
        {
            var s = new Subject<int>();
            Assert.IsFalse(s.HasObservers);

            var d = s.Subscribe(_ => { });
            Assert.IsTrue(s.HasObservers);

            d.Dispose();
            Assert.IsFalse(s.HasObservers);

            s.Dispose();
            Assert.IsFalse(s.HasObservers);
        }

        [TestMethod]
        public void HasObservers_Dispose3()
        {
            var s = new Subject<int>();
            Assert.IsFalse(s.HasObservers);

            s.Dispose();
            Assert.IsFalse(s.HasObservers);
        }

        [TestMethod]
        public void HasObservers_OnCompleted()
        {
            var s = new Subject<int>();
            Assert.IsFalse(s.HasObservers);

            var d = s.Subscribe(_ => { });
            Assert.IsTrue(s.HasObservers);

            s.OnNext(42);
            Assert.IsTrue(s.HasObservers);

            s.OnCompleted();
            Assert.IsFalse(s.HasObservers);
        }

        [TestMethod]
        public void HasObservers_OnError()
        {
            var s = new Subject<int>();
            Assert.IsFalse(s.HasObservers);

            var d = s.Subscribe(_ => { }, ex => { });
            Assert.IsTrue(s.HasObservers);

            s.OnNext(42);
            Assert.IsTrue(s.HasObservers);

            s.OnError(new Exception());
            Assert.IsFalse(s.HasObservers);
        }
    }
}

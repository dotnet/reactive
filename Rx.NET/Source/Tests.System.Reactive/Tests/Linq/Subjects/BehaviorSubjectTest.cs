// Copyright (c) Microsoft Open Technologies, Inc. All rights reserved. See License.txt in the project root for license information.

using System;
using System.Reactive.Concurrency;
using System.Reactive.Subjects;
using Microsoft.Reactive.Testing;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace ReactiveTests.Tests
{
    [TestClass]
    public partial class BehaviorSubjectTest : ReactiveTest
    {
        [TestMethod]
        public void Subscribe_ArgumentChecking()
        {
            ReactiveAssert.Throws<ArgumentNullException>(() => new BehaviorSubject<int>(1).Subscribe(null));
        }

        [TestMethod]
        public void OnError_ArgumentChecking()
        {
            ReactiveAssert.Throws<ArgumentNullException>(() => new BehaviorSubject<int>(1).OnError(null));
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

            var subject = default(BehaviorSubject<int>);
            var subscription = default(IDisposable);

            var results1 = scheduler.CreateObserver<int>();
            var subscription1 = default(IDisposable);

            var results2 = scheduler.CreateObserver<int>();
            var subscription2 = default(IDisposable);

            var results3 = scheduler.CreateObserver<int>();
            var subscription3 = default(IDisposable);

            scheduler.ScheduleAbsolute(100, () => subject = new BehaviorSubject<int>(100));
            scheduler.ScheduleAbsolute(200, () => subscription = xs.Subscribe(subject));
            scheduler.ScheduleAbsolute(1000, () => subscription.Dispose());

            scheduler.ScheduleAbsolute(300, () => subscription1 = subject.Subscribe(results1));
            scheduler.ScheduleAbsolute(400, () => subscription2 = subject.Subscribe(results2));
            scheduler.ScheduleAbsolute(900, () => subscription3 = subject.Subscribe(results3));

            scheduler.ScheduleAbsolute(600, () => subscription1.Dispose());
            scheduler.ScheduleAbsolute(700, () => subscription2.Dispose());
            scheduler.ScheduleAbsolute(800, () => subscription1.Dispose());
            scheduler.ScheduleAbsolute(950, () => subscription3.Dispose());

            scheduler.Start();

            results1.Messages.AssertEqual(
                OnNext(300, 4),
                OnNext(340, 5),
                OnNext(410, 6),
                OnNext(520, 7)
            );

            results2.Messages.AssertEqual(
                OnNext(400, 5),
                OnNext(410, 6),
                OnNext(520, 7),
                OnNext(630, 8)
            );

            results3.Messages.AssertEqual(
                OnNext(900, 10),
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

            var subject = default(BehaviorSubject<int>);
            var subscription = default(IDisposable);

            var results1 = scheduler.CreateObserver<int>();
            var subscription1 = default(IDisposable);

            var results2 = scheduler.CreateObserver<int>();
            var subscription2 = default(IDisposable);

            var results3 = scheduler.CreateObserver<int>();
            var subscription3 = default(IDisposable);

            scheduler.ScheduleAbsolute(100, () => subject = new BehaviorSubject<int>(100));
            scheduler.ScheduleAbsolute(200, () => subscription = xs.Subscribe(subject));
            scheduler.ScheduleAbsolute(1000, () => subscription.Dispose());

            scheduler.ScheduleAbsolute(300, () => subscription1 = subject.Subscribe(results1));
            scheduler.ScheduleAbsolute(400, () => subscription2 = subject.Subscribe(results2));
            scheduler.ScheduleAbsolute(900, () => subscription3 = subject.Subscribe(results3));

            scheduler.ScheduleAbsolute(600, () => subscription1.Dispose());
            scheduler.ScheduleAbsolute(700, () => subscription2.Dispose());
            scheduler.ScheduleAbsolute(800, () => subscription1.Dispose());
            scheduler.ScheduleAbsolute(950, () => subscription3.Dispose());

            scheduler.Start();

            results1.Messages.AssertEqual(
                OnNext(300, 4),
                OnNext(340, 5),
                OnNext(410, 6),
                OnNext(520, 7)
            );

            results2.Messages.AssertEqual(
                OnNext(400, 5),
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

            var subject = default(BehaviorSubject<int>);
            var subscription = default(IDisposable);

            var results1 = scheduler.CreateObserver<int>();
            var subscription1 = default(IDisposable);

            var results2 = scheduler.CreateObserver<int>();
            var subscription2 = default(IDisposable);

            var results3 = scheduler.CreateObserver<int>();
            var subscription3 = default(IDisposable);

            scheduler.ScheduleAbsolute(100, () => subject = new BehaviorSubject<int>(100));
            scheduler.ScheduleAbsolute(200, () => subscription = xs.Subscribe(subject));
            scheduler.ScheduleAbsolute(1000, () => subscription.Dispose());

            scheduler.ScheduleAbsolute(300, () => subscription1 = subject.Subscribe(results1));
            scheduler.ScheduleAbsolute(400, () => subscription2 = subject.Subscribe(results2));
            scheduler.ScheduleAbsolute(900, () => subscription3 = subject.Subscribe(results3));

            scheduler.ScheduleAbsolute(600, () => subscription1.Dispose());
            scheduler.ScheduleAbsolute(700, () => subscription2.Dispose());
            scheduler.ScheduleAbsolute(800, () => subscription1.Dispose());
            scheduler.ScheduleAbsolute(950, () => subscription3.Dispose());

            scheduler.Start();

            results1.Messages.AssertEqual(
                OnNext(300, 4),
                OnNext(340, 5),
                OnNext(410, 6),
                OnNext(520, 7)
            );

            results2.Messages.AssertEqual(
                OnNext(400, 5),
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

            var subject = default(BehaviorSubject<int>);
            var subscription = default(IDisposable);

            var results1 = scheduler.CreateObserver<int>();
            var subscription1 = default(IDisposable);

            var results2 = scheduler.CreateObserver<int>();
            var subscription2 = default(IDisposable);

            var results3 = scheduler.CreateObserver<int>();
            var subscription3 = default(IDisposable);

            scheduler.ScheduleAbsolute(100, () => subject = new BehaviorSubject<int>(100));
            scheduler.ScheduleAbsolute(200, () => subscription = xs.Subscribe(subject));
            scheduler.ScheduleAbsolute(1000, () => subscription.Dispose());

            scheduler.ScheduleAbsolute(300, () => subscription1 = subject.Subscribe(results1));
            scheduler.ScheduleAbsolute(400, () => subscription2 = subject.Subscribe(results2));
            scheduler.ScheduleAbsolute(900, () => subscription3 = subject.Subscribe(results3));

            scheduler.ScheduleAbsolute(600, () => subscription1.Dispose());
            scheduler.ScheduleAbsolute(700, () => subscription2.Dispose());
            scheduler.ScheduleAbsolute(800, () => subscription1.Dispose());
            scheduler.ScheduleAbsolute(950, () => subscription3.Dispose());

            scheduler.Start();

            results1.Messages.AssertEqual(
                OnNext(300, 100)
            );

            results2.Messages.AssertEqual(
                OnNext(400, 100),
                OnCompleted<int>(630)
            );

            results3.Messages.AssertEqual(
                OnCompleted<int>(900)
            );
        }

        [TestMethod]
        public void SubjectDisposed()
        {
            var scheduler = new TestScheduler();

            var subject = default(BehaviorSubject<int>);

            var results1 = scheduler.CreateObserver<int>();
            var subscription1 = default(IDisposable);

            var results2 = scheduler.CreateObserver<int>();
            var subscription2 = default(IDisposable);

            var results3 = scheduler.CreateObserver<int>();
            var subscription3 = default(IDisposable);

            scheduler.ScheduleAbsolute(100, () => subject = new BehaviorSubject<int>(0));
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
                OnNext(200, 1),
                OnNext(250, 2),
                OnNext(350, 3),
                OnNext(450, 4)
            );

            results2.Messages.AssertEqual(
                OnNext(300, 2),
                OnNext(350, 3),
                OnNext(450, 4),
                OnNext(550, 5)
            );

            results3.Messages.AssertEqual(
                OnNext(400, 3),
                OnNext(450, 4),
                OnNext(550, 5)
            );
        }

        [TestMethod]
        public void HasObservers()
        {
            var s = new BehaviorSubject<int>(42);
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
            var s = new BehaviorSubject<int>(42);
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
            var s = new BehaviorSubject<int>(42);
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
            var s = new BehaviorSubject<int>(42);
            Assert.IsFalse(s.HasObservers);

            s.Dispose();
            Assert.IsFalse(s.HasObservers);
        }

        [TestMethod]
        public void HasObservers_OnCompleted()
        {
            var s = new BehaviorSubject<int>(42);
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
            var s = new BehaviorSubject<int>(42);
            Assert.IsFalse(s.HasObservers);

            var d = s.Subscribe(_ => { }, ex => { });
            Assert.IsTrue(s.HasObservers);

            s.OnNext(42);
            Assert.IsTrue(s.HasObservers);

            s.OnError(new Exception());
            Assert.IsFalse(s.HasObservers);
        }

        [TestMethod]
        public void Value_Initial()
        {
            var s = new BehaviorSubject<int>(42);
            Assert.AreEqual(42, s.Value);
        }

        [TestMethod]
        public void Value_First()
        {
            var s = new BehaviorSubject<int>(42);
            Assert.AreEqual(42, s.Value);

            s.OnNext(43);
            Assert.AreEqual(43, s.Value);
        }

        [TestMethod]
        public void Value_Second()
        {
            var s = new BehaviorSubject<int>(42);
            Assert.AreEqual(42, s.Value);

            s.OnNext(43);
            Assert.AreEqual(43, s.Value);

            s.OnNext(44);
            Assert.AreEqual(44, s.Value);
        }

        [TestMethod]
        public void Value_FrozenAfterOnCompleted()
        {
            var s = new BehaviorSubject<int>(42);
            Assert.AreEqual(42, s.Value);

            s.OnNext(43);
            Assert.AreEqual(43, s.Value);

            s.OnNext(44);
            Assert.AreEqual(44, s.Value);

            s.OnCompleted();
            Assert.AreEqual(44, s.Value);

            s.OnNext(1234);
            Assert.AreEqual(44, s.Value);
        }

        [TestMethod, ExpectedException(typeof(InvalidOperationException))]
        public void Value_ThrowsAfterOnError()
        {
            var s = new BehaviorSubject<int>(42);
            Assert.AreEqual(42, s.Value);

            s.OnError(new InvalidOperationException());
            
            Assert.Fail("Should not be able to read Value: {0}", s.Value);
        }

        [TestMethod, ExpectedException(typeof(ObjectDisposedException))]
        public void Value_ThrowsOnDispose()
        {
            var s = new BehaviorSubject<int>(42);
            Assert.AreEqual(42, s.Value);

            s.Dispose();

            Assert.Fail("Should not be able to read Value: {0}", s.Value);
        }
    }
}

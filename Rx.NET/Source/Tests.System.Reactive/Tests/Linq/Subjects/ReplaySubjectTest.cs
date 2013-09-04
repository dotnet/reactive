// Copyright (c) Microsoft Open Technologies, Inc. All rights reserved. See License.txt in the project root for license information.

using System;
using System.Reactive.Concurrency;
using System.Reactive.Subjects;
using Microsoft.Reactive.Testing;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using ReactiveTests.Dummies;

namespace ReactiveTests.Tests
{
    [TestClass]
    public partial class ReplaySubjectTest : ReactiveTest
    {
        [TestMethod]
        public void Subscribe_ArgumentChecking()
        {
            ReactiveAssert.Throws<ArgumentNullException>(() => new ReplaySubject<int>().Subscribe(null));
        }

        [TestMethod]
        public void OnError_ArgumentChecking()
        {
            ReactiveAssert.Throws<ArgumentNullException>(() => new ReplaySubject<int>(DummyScheduler.Instance).OnError(null));
        }

        [TestMethod]
        public void Constructor_ArgumentChecking()
        {
            ReactiveAssert.Throws<ArgumentOutOfRangeException>(() => new ReplaySubject<int>(-1));
            ReactiveAssert.Throws<ArgumentOutOfRangeException>(() => new ReplaySubject<int>(-1, DummyScheduler.Instance));
            ReactiveAssert.Throws<ArgumentOutOfRangeException>(() => new ReplaySubject<int>(-1, TimeSpan.Zero));
            ReactiveAssert.Throws<ArgumentOutOfRangeException>(() => new ReplaySubject<int>(-1, TimeSpan.Zero, DummyScheduler.Instance));

            ReactiveAssert.Throws<ArgumentOutOfRangeException>(() => new ReplaySubject<int>(TimeSpan.FromTicks(-1)));
            ReactiveAssert.Throws<ArgumentOutOfRangeException>(() => new ReplaySubject<int>(TimeSpan.FromTicks(-1), DummyScheduler.Instance));
            ReactiveAssert.Throws<ArgumentOutOfRangeException>(() => new ReplaySubject<int>(0, TimeSpan.FromTicks(-1)));
            ReactiveAssert.Throws<ArgumentOutOfRangeException>(() => new ReplaySubject<int>(0, TimeSpan.FromTicks(-1), DummyScheduler.Instance));

            ReactiveAssert.Throws<ArgumentNullException>(() => new ReplaySubject<int>(null));
            ReactiveAssert.Throws<ArgumentNullException>(() => new ReplaySubject<int>(0, null));
            ReactiveAssert.Throws<ArgumentNullException>(() => new ReplaySubject<int>(TimeSpan.Zero, null));
            ReactiveAssert.Throws<ArgumentNullException>(() => new ReplaySubject<int>(0, TimeSpan.Zero, null));

            // zero allowed
            new ReplaySubject<int>(0);
            new ReplaySubject<int>(TimeSpan.Zero);
            new ReplaySubject<int>(0, TimeSpan.Zero);
            new ReplaySubject<int>(0, DummyScheduler.Instance);
            new ReplaySubject<int>(TimeSpan.Zero, DummyScheduler.Instance);
            new ReplaySubject<int>(0, TimeSpan.Zero, DummyScheduler.Instance);
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

            var subject = default(ReplaySubject<int>);
            var subscription = default(IDisposable);

            var results1 = scheduler.CreateObserver<int>();
            var subscription1 = default(IDisposable);

            var results2 = scheduler.CreateObserver<int>();
            var subscription2 = default(IDisposable);

            var results3 = scheduler.CreateObserver<int>();
            var subscription3 = default(IDisposable);

            scheduler.ScheduleAbsolute(100, () => subject = new ReplaySubject<int>(3, TimeSpan.FromTicks(100), scheduler));
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
                OnNext(301, 3),
                OnNext(302, 4),
                OnNext(341, 5),
                OnNext(411, 6),
                OnNext(521, 7)
            );

            results2.Messages.AssertEqual(
                OnNext(401, 5),
                OnNext(411, 6),
                OnNext(521, 7),
                OnNext(631, 8)
            );

            results3.Messages.AssertEqual(
                OnNext(901, 10),
                OnNext(941, 11)
            );
        }

        [TestMethod]
        public void Infinite2()
        {
            var scheduler = new TestScheduler();

            var xs = scheduler.CreateHotObservable(
                OnNext(70, 1),
                OnNext(110, 2),
                OnNext(220, 3),
                OnNext(270, 4),
                OnNext(280, -1),
                OnNext(290, -2),
                OnNext(340, 5),
                OnNext(410, 6),
                OnNext(520, 7),
                OnNext(630, 8),
                OnNext(710, 9),
                OnNext(870, 10),
                OnNext(940, 11),
                OnNext(1020, 12)
            );

            var subject = default(ReplaySubject<int>);
            var subscription = default(IDisposable);

            var results1 = scheduler.CreateObserver<int>();
            var subscription1 = default(IDisposable);

            var results2 = scheduler.CreateObserver<int>();
            var subscription2 = default(IDisposable);

            var results3 = scheduler.CreateObserver<int>();
            var subscription3 = default(IDisposable);

            scheduler.ScheduleAbsolute(100, () => subject = new ReplaySubject<int>(3, TimeSpan.FromTicks(100), scheduler));
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
                OnNext(301, 4),
                OnNext(302, -1),
                OnNext(303, -2),
                OnNext(341, 5),
                OnNext(411, 6),
                OnNext(521, 7)
            );

            results2.Messages.AssertEqual(
                OnNext(401, 5),
                OnNext(411, 6),
                OnNext(521, 7),
                OnNext(631, 8)
            );

            results3.Messages.AssertEqual(
                OnNext(901, 10),
                OnNext(941, 11)
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

            var subject = default(ReplaySubject<int>);
            var subscription = default(IDisposable);

            var results1 = scheduler.CreateObserver<int>();
            var subscription1 = default(IDisposable);

            var results2 = scheduler.CreateObserver<int>();
            var subscription2 = default(IDisposable);

            var results3 = scheduler.CreateObserver<int>();
            var subscription3 = default(IDisposable);

            scheduler.ScheduleAbsolute(100, () => subject = new ReplaySubject<int>(3, TimeSpan.FromTicks(100), scheduler));
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
                OnNext(301, 3),
                OnNext(302, 4),
                OnNext(341, 5),
                OnNext(411, 6),
                OnNext(521, 7)
            );

            results2.Messages.AssertEqual(
                OnNext(401, 5),
                OnNext(411, 6),
                OnNext(521, 7),
                OnCompleted<int>(631)
            );

            results3.Messages.AssertEqual(
                OnCompleted<int>(901)
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

            var subject = default(ReplaySubject<int>);
            var subscription = default(IDisposable);

            var results1 = scheduler.CreateObserver<int>();
            var subscription1 = default(IDisposable);

            var results2 = scheduler.CreateObserver<int>();
            var subscription2 = default(IDisposable);

            var results3 = scheduler.CreateObserver<int>();
            var subscription3 = default(IDisposable);

            scheduler.ScheduleAbsolute(100, () => subject = new ReplaySubject<int>(3, TimeSpan.FromTicks(100), scheduler));
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
                OnNext(301, 3),
                OnNext(302, 4),
                OnNext(341, 5),
                OnNext(411, 6),
                OnNext(521, 7)
            );

            results2.Messages.AssertEqual(
                OnNext(401, 5),
                OnNext(411, 6),
                OnNext(521, 7),
                OnError<int>(631, ex)
            );

            results3.Messages.AssertEqual(
                OnError<int>(901, ex)
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

            var subject = default(ReplaySubject<int>);
            var subscription = default(IDisposable);

            var results1 = scheduler.CreateObserver<int>();
            var subscription1 = default(IDisposable);

            var results2 = scheduler.CreateObserver<int>();
            var subscription2 = default(IDisposable);

            var results3 = scheduler.CreateObserver<int>();
            var subscription3 = default(IDisposable);

            scheduler.ScheduleAbsolute(100, () => subject = new ReplaySubject<int>(3, TimeSpan.FromTicks(100), scheduler));
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
            );

            results2.Messages.AssertEqual(
                OnCompleted<int>(631)
            );

            results3.Messages.AssertEqual(
                OnCompleted<int>(901)
            );
        }

        [TestMethod]
        public void SubjectDisposed()
        {
            var scheduler = new TestScheduler();

            var subject = default(ReplaySubject<int>);

            var results1 = scheduler.CreateObserver<int>();
            var subscription1 = default(IDisposable);

            var results2 = scheduler.CreateObserver<int>();
            var subscription2 = default(IDisposable);

            var results3 = scheduler.CreateObserver<int>();
            var subscription3 = default(IDisposable);

            scheduler.ScheduleAbsolute(100, () => subject = new ReplaySubject<int>(scheduler));
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
                OnNext(201, 1),
                OnNext(251, 2),
                OnNext(351, 3),
                OnNext(451, 4)
            );

            results2.Messages.AssertEqual(
                OnNext(301, 1),
                OnNext(302, 2),
                OnNext(351, 3),
                OnNext(451, 4),
                OnNext(551, 5)
            );

            results3.Messages.AssertEqual(
                OnNext(401, 1),
                OnNext(402, 2),
                OnNext(403, 3),
                OnNext(451, 4),
                OnNext(551, 5)
            );
        }

        [TestMethod]
        public void ReplaySubjectDiesOut()
        {
            //
            // Tests v1.x behavior as documented in ReplaySubject.cs (Subscribe method).
            //

            var scheduler = new TestScheduler();

            var xs = scheduler.CreateHotObservable(
                OnNext(70, 1),
                OnNext(110, 2),
                OnNext(220, 3),
                OnNext(270, 4),
                OnNext(340, 5),
                OnNext(410, 6),
                OnNext(520, 7),
                OnCompleted<int>(580)
            );

            var subject = default(ReplaySubject<int>);

            var results1 = scheduler.CreateObserver<int>();
            var results2 = scheduler.CreateObserver<int>();
            var results3 = scheduler.CreateObserver<int>();
            var results4 = scheduler.CreateObserver<int>();

            scheduler.ScheduleAbsolute(100, () => subject = new ReplaySubject<int>(int.MaxValue, TimeSpan.FromTicks(100), scheduler));
            scheduler.ScheduleAbsolute(200, () => xs.Subscribe(subject));

            scheduler.ScheduleAbsolute(300, () => subject.Subscribe(results1));
            scheduler.ScheduleAbsolute(400, () => subject.Subscribe(results2));
            scheduler.ScheduleAbsolute(600, () => subject.Subscribe(results3));
            scheduler.ScheduleAbsolute(900, () => subject.Subscribe(results4));

            scheduler.Start();

            results1.Messages.AssertEqual(
                OnNext(301, 3),
                OnNext(302, 4),
                OnNext(341, 5),
                OnNext(411, 6),
                OnNext(521, 7),
                OnCompleted<int>(581)
            );

            results2.Messages.AssertEqual(
                OnNext(401, 5),
                OnNext(411, 6),
                OnNext(521, 7),
                OnCompleted<int>(581)
            );

            results3.Messages.AssertEqual(
                OnNext(601, 7),
                OnCompleted<int>(602)
            );

            results4.Messages.AssertEqual(
                OnCompleted<int>(901)
            );
        }

        [TestMethod]
        public void HasObservers()
        {
            var s = new ReplaySubject<int>();
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
            var s = new ReplaySubject<int>();
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
            var s = new ReplaySubject<int>();
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
            var s = new ReplaySubject<int>();
            Assert.IsFalse(s.HasObservers);

            s.Dispose();
            Assert.IsFalse(s.HasObservers);
        }

        [TestMethod]
        public void HasObservers_OnCompleted()
        {
            var s = new ReplaySubject<int>();
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
            var s = new ReplaySubject<int>();
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

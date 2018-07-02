// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the Apache 2.0 License.
// See the LICENSE file in the project root for more information. 

using System;
using System.Collections.Generic;
using System.Linq;
using System.Reactive;
using System.Reactive.Concurrency;
using System.Reactive.Subjects;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Reactive.Testing;
using ReactiveTests.Dummies;
using Xunit;

namespace ReactiveTests.Tests
{

    public partial class ReplaySubjectTest : ReactiveTest
    {
        [Fact]
        public void Subscribe_ArgumentChecking()
        {
            ReactiveAssert.Throws<ArgumentNullException>(() => new ReplaySubject<int>().Subscribe(null));
            ReactiveAssert.Throws<ArgumentNullException>(() => new ReplaySubject<int>(1).Subscribe(null));
            ReactiveAssert.Throws<ArgumentNullException>(() => new ReplaySubject<int>(2).Subscribe(null));
            ReactiveAssert.Throws<ArgumentNullException>(() => new ReplaySubject<int>(DummyScheduler.Instance).Subscribe(null));
        }

        [Fact]
        public void OnError_ArgumentChecking()
        {
            ReactiveAssert.Throws<ArgumentNullException>(() => new ReplaySubject<int>().OnError(null));
            ReactiveAssert.Throws<ArgumentNullException>(() => new ReplaySubject<int>(1).OnError(null));
            ReactiveAssert.Throws<ArgumentNullException>(() => new ReplaySubject<int>(2).OnError(null));
            ReactiveAssert.Throws<ArgumentNullException>(() => new ReplaySubject<int>(DummyScheduler.Instance).OnError(null));
        }

        [Fact]
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

        [Fact]
        public void Infinite_ReplayByTime()
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

        [Fact]
        public void Infinite_ReplayOne()
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

            var results4 = scheduler.CreateObserver<int>();
            var subscription4 = default(IDisposable);

            scheduler.ScheduleAbsolute(100, () => subject = new ReplaySubject<int>(1));
            scheduler.ScheduleAbsolute(200, () => subscription = xs.Subscribe(subject));
            scheduler.ScheduleAbsolute(1200, () => subscription.Dispose());

            scheduler.ScheduleAbsolute(300, () => subscription1 = subject.Subscribe(results1));
            scheduler.ScheduleAbsolute(400, () => subscription2 = subject.Subscribe(results2));
            scheduler.ScheduleAbsolute(900, () => subscription3 = subject.Subscribe(results3));
            scheduler.ScheduleAbsolute(1100, () => subscription4 = subject.Subscribe(results4));

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

            results4.Messages.AssertEqual(
                OnNext(1100, 12)
            );
        }

        [Fact]
        public void Infinite_ReplayMany()
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

            scheduler.ScheduleAbsolute(100, () => subject = new ReplaySubject<int>(3));
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
                OnNext(300, 3),
                OnNext(300, 4),
                OnNext(340, 5),
                OnNext(410, 6),
                OnNext(520, 7)
            );

            results2.Messages.AssertEqual(
                OnNext(400, 3),
                OnNext(400, 4),
                OnNext(400, 5),
                OnNext(410, 6),
                OnNext(520, 7),
                OnNext(630, 8)
            );

            results3.Messages.AssertEqual(
                OnNext(900, 8),
                OnNext(900, 9),
                OnNext(900, 10),
                OnNext(940, 11)
            );
        }

        [Fact]
        public void Infinite_ReplayAll()
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

            scheduler.ScheduleAbsolute(100, () => subject = new ReplaySubject<int>());
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
                OnNext(300, 3),
                OnNext(300, 4),
                OnNext(340, 5),
                OnNext(410, 6),
                OnNext(520, 7)
            );

            results2.Messages.AssertEqual(
                OnNext(400, 3),
                OnNext(400, 4),
                OnNext(400, 5),
                OnNext(410, 6),
                OnNext(520, 7),
                OnNext(630, 8)
            );

            results3.Messages.AssertEqual(
                OnNext(900, 3),
                OnNext(900, 4),
                OnNext(900, 5),
                OnNext(900, 6),
                OnNext(900, 7),
                OnNext(900, 8),
                OnNext(900, 9),
                OnNext(900, 10),
                OnNext(940, 11)
            );
        }

        [Fact]
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

        [Fact]
        public void Finite_ReplayByTime()
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

        [Fact]
        public void Finite_ReplayOne()
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

            scheduler.ScheduleAbsolute(100, () => subject = new ReplaySubject<int>(1));
            scheduler.ScheduleAbsolute(200, () => subscription = xs.Subscribe(subject));
            scheduler.ScheduleAbsolute(1000, () => subscription.Dispose());

            scheduler.ScheduleAbsolute(300, () => subscription1 = subject.Subscribe(results1));
            scheduler.ScheduleAbsolute(400, () => subscription2 = subject.Subscribe(results2));
            scheduler.ScheduleAbsolute(900, () => subscription3 = subject.Subscribe(results3));

            scheduler.ScheduleAbsolute(600, () => subscription1.Dispose());
            scheduler.ScheduleAbsolute(700, () => subscription2.Dispose());
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
                OnNext(900, 7),
                OnCompleted<int>(900)
            );
        }

        [Fact]
        public void Finite_ReplayMany()
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

            scheduler.ScheduleAbsolute(100, () => subject = new ReplaySubject<int>(3));
            scheduler.ScheduleAbsolute(200, () => subscription = xs.Subscribe(subject));
            scheduler.ScheduleAbsolute(1000, () => subscription.Dispose());

            scheduler.ScheduleAbsolute(300, () => subscription1 = subject.Subscribe(results1));
            scheduler.ScheduleAbsolute(400, () => subscription2 = subject.Subscribe(results2));
            scheduler.ScheduleAbsolute(900, () => subscription3 = subject.Subscribe(results3));

            scheduler.ScheduleAbsolute(600, () => subscription1.Dispose());
            scheduler.ScheduleAbsolute(700, () => subscription2.Dispose());
            scheduler.ScheduleAbsolute(950, () => subscription3.Dispose());

            scheduler.Start();

            results1.Messages.AssertEqual(
                OnNext(300, 3),
                OnNext(300, 4),
                OnNext(340, 5),
                OnNext(410, 6),
                OnNext(520, 7)
            );

            results2.Messages.AssertEqual(
                OnNext(400, 3),
                OnNext(400, 4),
                OnNext(400, 5),
                OnNext(410, 6),
                OnNext(520, 7),
                OnCompleted<int>(630)
            );

            results3.Messages.AssertEqual(
                OnNext(900, 5),
                OnNext(900, 6),
                OnNext(900, 7),
                OnCompleted<int>(900)
            );
        }

        [Fact]
        public void Finite_ReplayAll()
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

            scheduler.ScheduleAbsolute(100, () => subject = new ReplaySubject<int>());
            scheduler.ScheduleAbsolute(200, () => subscription = xs.Subscribe(subject));
            scheduler.ScheduleAbsolute(1000, () => subscription.Dispose());

            scheduler.ScheduleAbsolute(300, () => subscription1 = subject.Subscribe(results1));
            scheduler.ScheduleAbsolute(400, () => subscription2 = subject.Subscribe(results2));
            scheduler.ScheduleAbsolute(900, () => subscription3 = subject.Subscribe(results3));

            scheduler.ScheduleAbsolute(600, () => subscription1.Dispose());
            scheduler.ScheduleAbsolute(700, () => subscription2.Dispose());
            scheduler.ScheduleAbsolute(950, () => subscription3.Dispose());

            scheduler.Start();

            results1.Messages.AssertEqual(
                OnNext(300, 3),
                OnNext(300, 4),
                OnNext(340, 5),
                OnNext(410, 6),
                OnNext(520, 7)
            );

            results2.Messages.AssertEqual(
                OnNext(400, 3),
                OnNext(400, 4),
                OnNext(400, 5),
                OnNext(410, 6),
                OnNext(520, 7),
                OnCompleted<int>(630)
            );

            results3.Messages.AssertEqual(
                OnNext(900, 3),
                OnNext(900, 4),
                OnNext(900, 5),
                OnNext(900, 6),
                OnNext(900, 7),
                OnCompleted<int>(900)
            );
        }

        [Fact]
        public void Error_ReplayByTime()
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

        [Fact]
        public void Error_ReplayOne()
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

            scheduler.ScheduleAbsolute(100, () => subject = new ReplaySubject<int>(1));
            scheduler.ScheduleAbsolute(200, () => subscription = xs.Subscribe(subject));
            scheduler.ScheduleAbsolute(1000, () => subscription.Dispose());

            scheduler.ScheduleAbsolute(300, () => subscription1 = subject.Subscribe(results1));
            scheduler.ScheduleAbsolute(400, () => subscription2 = subject.Subscribe(results2));
            scheduler.ScheduleAbsolute(900, () => subscription3 = subject.Subscribe(results3));

            scheduler.ScheduleAbsolute(600, () => subscription1.Dispose());
            scheduler.ScheduleAbsolute(700, () => subscription2.Dispose());
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
                OnNext(900, 7),
                OnError<int>(900, ex)
            );
        }

        [Fact]
        public void Error_ReplayMany()
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

            scheduler.ScheduleAbsolute(100, () => subject = new ReplaySubject<int>(3));
            scheduler.ScheduleAbsolute(200, () => subscription = xs.Subscribe(subject));
            scheduler.ScheduleAbsolute(1000, () => subscription.Dispose());

            scheduler.ScheduleAbsolute(300, () => subscription1 = subject.Subscribe(results1));
            scheduler.ScheduleAbsolute(400, () => subscription2 = subject.Subscribe(results2));
            scheduler.ScheduleAbsolute(900, () => subscription3 = subject.Subscribe(results3));

            scheduler.ScheduleAbsolute(600, () => subscription1.Dispose());
            scheduler.ScheduleAbsolute(700, () => subscription2.Dispose());
            scheduler.ScheduleAbsolute(950, () => subscription3.Dispose());

            scheduler.Start();

            results1.Messages.AssertEqual(
                OnNext(300, 3),
                OnNext(300, 4),
                OnNext(340, 5),
                OnNext(410, 6),
                OnNext(520, 7)
            );

            results2.Messages.AssertEqual(
                OnNext(400, 3),
                OnNext(400, 4),
                OnNext(400, 5),
                OnNext(410, 6),
                OnNext(520, 7),
                OnError<int>(630, ex)
            );

            results3.Messages.AssertEqual(
                OnNext(900, 5),
                OnNext(900, 6),
                OnNext(900, 7),
                OnError<int>(900, ex)
            );
        }

        [Fact]
        public void Error_ReplayAll()
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

            scheduler.ScheduleAbsolute(100, () => subject = new ReplaySubject<int>());
            scheduler.ScheduleAbsolute(200, () => subscription = xs.Subscribe(subject));
            scheduler.ScheduleAbsolute(1000, () => subscription.Dispose());

            scheduler.ScheduleAbsolute(300, () => subscription1 = subject.Subscribe(results1));
            scheduler.ScheduleAbsolute(400, () => subscription2 = subject.Subscribe(results2));
            scheduler.ScheduleAbsolute(900, () => subscription3 = subject.Subscribe(results3));

            scheduler.ScheduleAbsolute(600, () => subscription1.Dispose());
            scheduler.ScheduleAbsolute(700, () => subscription2.Dispose());
            scheduler.ScheduleAbsolute(950, () => subscription3.Dispose());

            scheduler.Start();

            results1.Messages.AssertEqual(
                OnNext(300, 3),
                OnNext(300, 4),
                OnNext(340, 5),
                OnNext(410, 6),
                OnNext(520, 7)
            );

            results2.Messages.AssertEqual(
                OnNext(400, 3),
                OnNext(400, 4),
                OnNext(400, 5),
                OnNext(410, 6),
                OnNext(520, 7),
                OnError<int>(630, ex)
            );

            results3.Messages.AssertEqual(
                OnNext(900, 3),
                OnNext(900, 4),
                OnNext(900, 5),
                OnNext(900, 6),
                OnNext(900, 7),
                OnError<int>(900, ex)
            );
        }

        [Fact]
        public void Canceled_ReplayByTime()
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

        [Fact]
        public void Canceled_ReplayOne()
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

            scheduler.ScheduleAbsolute(100, () => subject = new ReplaySubject<int>(1));
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
                OnCompleted<int>(630)
            );

            results3.Messages.AssertEqual(
                OnCompleted<int>(900)
            );
        }

        [Fact]
        public void Canceled_ReplayMany()
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

            scheduler.ScheduleAbsolute(100, () => subject = new ReplaySubject<int>(3));
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
                OnCompleted<int>(630)
            );

            results3.Messages.AssertEqual(
                OnCompleted<int>(900)
            );
        }

        [Fact]
        public void Canceled_ReplayAll()
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

            scheduler.ScheduleAbsolute(100, () => subject = new ReplaySubject<int>());
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
                OnCompleted<int>(630)
            );

            results3.Messages.AssertEqual(
                OnCompleted<int>(900)
            );
        }

        [Fact]
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

        [Fact]
        public void SubjectDisposed_ReplayOne()
        {
            var scheduler = new TestScheduler();

            var subject = default(ReplaySubject<int>);

            var results1 = scheduler.CreateObserver<int>();
            var subscription1 = default(IDisposable);

            var results2 = scheduler.CreateObserver<int>();
            var subscription2 = default(IDisposable);

            var results3 = scheduler.CreateObserver<int>();
            var subscription3 = default(IDisposable);

            scheduler.ScheduleAbsolute(100, () => subject = new ReplaySubject<int>(1));
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

        [Fact]
        public void SubjectDisposed_ReplayMany()
        {
            var scheduler = new TestScheduler();

            var subject = default(ReplaySubject<int>);

            var results1 = scheduler.CreateObserver<int>();
            var subscription1 = default(IDisposable);

            var results2 = scheduler.CreateObserver<int>();
            var subscription2 = default(IDisposable);

            var results3 = scheduler.CreateObserver<int>();
            var subscription3 = default(IDisposable);

            scheduler.ScheduleAbsolute(100, () => subject = new ReplaySubject<int>(3));
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
                OnNext(300, 1),
                OnNext(300, 2),
                OnNext(350, 3),
                OnNext(450, 4),
                OnNext(550, 5)
            );

            results3.Messages.AssertEqual(
                OnNext(400, 1),
                OnNext(400, 2),
                OnNext(400, 3),
                OnNext(450, 4),
                OnNext(550, 5)
            );
        }

        [Fact]
        public void SubjectDisposed_ReplayAll()
        {
            var scheduler = new TestScheduler();

            var subject = default(ReplaySubject<int>);

            var results1 = scheduler.CreateObserver<int>();
            var subscription1 = default(IDisposable);

            var results2 = scheduler.CreateObserver<int>();
            var subscription2 = default(IDisposable);

            var results3 = scheduler.CreateObserver<int>();
            var subscription3 = default(IDisposable);

            scheduler.ScheduleAbsolute(100, () => subject = new ReplaySubject<int>());
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
                OnNext(300, 1),
                OnNext(300, 2),
                OnNext(350, 3),
                OnNext(450, 4),
                OnNext(550, 5)
            );

            results3.Messages.AssertEqual(
                OnNext(400, 1),
                OnNext(400, 2),
                OnNext(400, 3),
                OnNext(450, 4),
                OnNext(550, 5)
            );
        }

        //
        // TODO: Create a failing test for this for the other implementations (ReplayOne/Many/All).
        // I don't understand the behavior. 
        // I think it may have to do with calling Trim() on Subscription (as well as in the OnNext calls). -LC
        //

        [Fact]
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

        [Fact]
        public void HasObservers()
        {
            HasObserversImpl(new ReplaySubject<int>());
            HasObserversImpl(new ReplaySubject<int>(1));
            HasObserversImpl(new ReplaySubject<int>(3));
            HasObserversImpl(new ReplaySubject<int>(TimeSpan.FromSeconds(1)));
        }

        private static void HasObserversImpl(ReplaySubject<int> s)
        {
            Assert.False(s.HasObservers);

            var d1 = s.Subscribe(_ => { });
            Assert.True(s.HasObservers);

            d1.Dispose();
            Assert.False(s.HasObservers);

            var d2 = s.Subscribe(_ => { });
            Assert.True(s.HasObservers);

            var d3 = s.Subscribe(_ => { });
            Assert.True(s.HasObservers);

            d2.Dispose();
            Assert.True(s.HasObservers);

            d3.Dispose();
            Assert.False(s.HasObservers);
        }

        [Fact]
        public void HasObservers_Dispose1()
        {
            HasObservers_Dispose1Impl(new ReplaySubject<int>());
            HasObservers_Dispose1Impl(new ReplaySubject<int>(1));
            HasObservers_Dispose1Impl(new ReplaySubject<int>(3));
            HasObservers_Dispose1Impl(new ReplaySubject<int>(TimeSpan.FromSeconds(1)));
        }

        private static void HasObservers_Dispose1Impl(ReplaySubject<int> s)
        {
            Assert.False(s.HasObservers);
            Assert.False(s.IsDisposed);

            var d = s.Subscribe(_ => { });
            Assert.True(s.HasObservers);
            Assert.False(s.IsDisposed);

            s.Dispose();
            Assert.False(s.HasObservers);
            Assert.True(s.IsDisposed);

            d.Dispose();
            Assert.False(s.HasObservers);
            Assert.True(s.IsDisposed);
        }

        [Fact]
        public void HasObservers_Dispose2()
        {
            HasObservers_Dispose2Impl(new ReplaySubject<int>());
            HasObservers_Dispose2Impl(new ReplaySubject<int>(1));
            HasObservers_Dispose2Impl(new ReplaySubject<int>(3));
            HasObservers_Dispose2Impl(new ReplaySubject<int>(TimeSpan.FromSeconds(1)));
        }

        private static void HasObservers_Dispose2Impl(ReplaySubject<int> s)
        {
            Assert.False(s.HasObservers);
            Assert.False(s.IsDisposed);

            var d = s.Subscribe(_ => { });
            Assert.True(s.HasObservers);
            Assert.False(s.IsDisposed);

            d.Dispose();
            Assert.False(s.HasObservers);
            Assert.False(s.IsDisposed);

            s.Dispose();
            Assert.False(s.HasObservers);
            Assert.True(s.IsDisposed);
        }

        [Fact]
        public void HasObservers_Dispose3()
        {
            HasObservers_Dispose3Impl(new ReplaySubject<int>());
            HasObservers_Dispose3Impl(new ReplaySubject<int>(1));
            HasObservers_Dispose3Impl(new ReplaySubject<int>(3));
            HasObservers_Dispose3Impl(new ReplaySubject<int>(TimeSpan.FromSeconds(1)));
        }

        private static void HasObservers_Dispose3Impl(ReplaySubject<int> s)
        {
            Assert.False(s.HasObservers);
            Assert.False(s.IsDisposed);

            s.Dispose();
            Assert.False(s.HasObservers);
            Assert.True(s.IsDisposed);
        }

        [Fact]
        public void HasObservers_OnCompleted()
        {
            HasObservers_OnCompletedImpl(new ReplaySubject<int>());
            HasObservers_OnCompletedImpl(new ReplaySubject<int>(1));
            HasObservers_OnCompletedImpl(new ReplaySubject<int>(3));
            HasObservers_OnCompletedImpl(new ReplaySubject<int>(TimeSpan.FromSeconds(1)));
        }

        private static void HasObservers_OnCompletedImpl(ReplaySubject<int> s)
        {
            Assert.False(s.HasObservers);

            var d = s.Subscribe(_ => { });
            Assert.True(s.HasObservers);

            s.OnNext(42);
            Assert.True(s.HasObservers);

            s.OnCompleted();
            Assert.False(s.HasObservers);
        }

        [Fact]
        public void HasObservers_OnError()
        {
            HasObservers_OnErrorImpl(new ReplaySubject<int>());
            HasObservers_OnErrorImpl(new ReplaySubject<int>(1));
            HasObservers_OnErrorImpl(new ReplaySubject<int>(3));
            HasObservers_OnErrorImpl(new ReplaySubject<int>(TimeSpan.FromSeconds(1)));
        }

        private static void HasObservers_OnErrorImpl(ReplaySubject<int> s)
        {
            Assert.False(s.HasObservers);

            var d = s.Subscribe(_ => { }, ex => { });
            Assert.True(s.HasObservers);

            s.OnNext(42);
            Assert.True(s.HasObservers);

            s.OnError(new Exception());
            Assert.False(s.HasObservers);
        }

        [Fact]
        public void Completed_to_late_subscriber_ReplayAll()
        {
            var s = new ReplaySubject<int>();
            s.OnNext(1);
            s.OnNext(2);
            s.OnCompleted();

            var scheduler = new TestScheduler();
            var observer = scheduler.CreateObserver<int>();
            s.Subscribe(observer);

            Assert.Equal(3, observer.Messages.Count);

            Assert.Equal(1, observer.Messages[0].Value.Value);
            Assert.Equal(2, observer.Messages[1].Value.Value);
            Assert.Equal(NotificationKind.OnCompleted, observer.Messages[2].Value.Kind);
        }

        [Fact]
        public void Completed_to_late_subscriber_ReplayOne()
        {
            var s = new ReplaySubject<int>(1);
            s.OnNext(1);
            s.OnNext(2);
            s.OnCompleted();

            var scheduler = new TestScheduler();
            var observer = scheduler.CreateObserver<int>();
            s.Subscribe(observer);

            Assert.Equal(2, observer.Messages.Count);

            Assert.Equal(2, observer.Messages[0].Value.Value);
            Assert.Equal(NotificationKind.OnCompleted, observer.Messages[1].Value.Kind);
        }

        [Fact]
        public void Completed_to_late_subscriber_ReplayMany()
        {
            var s = new ReplaySubject<int>(2);
            s.OnNext(1);
            s.OnNext(2);
            s.OnNext(3);
            s.OnCompleted();

            var scheduler = new TestScheduler();
            var observer = scheduler.CreateObserver<int>();
            s.Subscribe(observer);

            Assert.Equal(3, observer.Messages.Count);

            Assert.Equal(2, observer.Messages[0].Value.Value);
            Assert.Equal(3, observer.Messages[1].Value.Value);
            Assert.Equal(NotificationKind.OnCompleted, observer.Messages[2].Value.Kind);
        }

        [Fact]
        public void Completed_to_late_subscriber_ReplayByTime()
        {
            var s = new ReplaySubject<int>(TimeSpan.FromMinutes(1));
            s.OnNext(1);
            s.OnNext(2);
            s.OnNext(3);
            s.OnCompleted();

            var scheduler = new TestScheduler();
            var observer = scheduler.CreateObserver<int>();
            s.Subscribe(observer);

            Assert.Equal(4, observer.Messages.Count);

            Assert.Equal(1, observer.Messages[0].Value.Value);
            Assert.Equal(2, observer.Messages[1].Value.Value);
            Assert.Equal(3, observer.Messages[2].Value.Value);
            Assert.Equal(NotificationKind.OnCompleted, observer.Messages[3].Value.Kind);
        }

        [Fact]
        public void Errored_to_late_subscriber_ReplayAll()
        {
            var expectedException = new Exception("Test");
            var s = new ReplaySubject<int>();
            s.OnNext(1);
            s.OnNext(2);
            s.OnError(expectedException);

            var scheduler = new TestScheduler();
            var observer = scheduler.CreateObserver<int>();
            s.Subscribe(observer);

            Assert.Equal(3, observer.Messages.Count);

            Assert.Equal(1, observer.Messages[0].Value.Value);
            Assert.Equal(2, observer.Messages[1].Value.Value);
            Assert.Equal(NotificationKind.OnError, observer.Messages[2].Value.Kind);
            Assert.Equal(expectedException, observer.Messages[2].Value.Exception);
        }

        [Fact]
        public void Errored_to_late_subscriber_ReplayOne()
        {
            var expectedException = new Exception("Test");
            var s = new ReplaySubject<int>(1);
            s.OnNext(1);
            s.OnNext(2);
            s.OnError(expectedException);

            var scheduler = new TestScheduler();
            var observer = scheduler.CreateObserver<int>();
            s.Subscribe(observer);

            Assert.Equal(2, observer.Messages.Count);

            Assert.Equal(2, observer.Messages[0].Value.Value);
            Assert.Equal(NotificationKind.OnError, observer.Messages[1].Value.Kind);
            Assert.Equal(expectedException, observer.Messages[1].Value.Exception);
        }

        [Fact]
        public void Errored_to_late_subscriber_ReplayMany()
        {
            var expectedException = new Exception("Test");
            var s = new ReplaySubject<int>(2);
            s.OnNext(1);
            s.OnNext(2);
            s.OnNext(3);
            s.OnError(expectedException);

            var scheduler = new TestScheduler();
            var observer = scheduler.CreateObserver<int>();
            s.Subscribe(observer);

            Assert.Equal(3, observer.Messages.Count);

            Assert.Equal(2, observer.Messages[0].Value.Value);
            Assert.Equal(3, observer.Messages[1].Value.Value);
            Assert.Equal(NotificationKind.OnError, observer.Messages[2].Value.Kind);
            Assert.Equal(expectedException, observer.Messages[2].Value.Exception);
        }

        [Fact]
        public void Errored_to_late_subscriber_ReplayByTime()
        {
            var expectedException = new Exception("Test");
            var s = new ReplaySubject<int>(TimeSpan.FromMinutes(1));
            s.OnNext(1);
            s.OnNext(2);
            s.OnNext(3);
            s.OnError(expectedException);

            var scheduler = new TestScheduler();
            var observer = scheduler.CreateObserver<int>();
            s.Subscribe(observer);

            Assert.Equal(4, observer.Messages.Count);

            Assert.Equal(1, observer.Messages[0].Value.Value);
            Assert.Equal(2, observer.Messages[1].Value.Value);
            Assert.Equal(3, observer.Messages[2].Value.Value);
            Assert.Equal(NotificationKind.OnError, observer.Messages[3].Value.Kind);
            Assert.Equal(expectedException, observer.Messages[3].Value.Exception);
        }

        [Fact]
        public void ReplaySubject_Reentrant()
        {
            var r = new ReplaySubject<int>(4);

            r.OnNext(0);
            r.OnNext(1);
            r.OnNext(2);
            r.OnNext(3);
            r.OnNext(4);

            var xs = new List<int>();

            var i = 0;
            r.Subscribe(x =>
            {
                xs.Add(x);

                if (++i <= 10)
                {
                    r.OnNext(x);
                }
            });

            r.OnNext(5);

            Assert.True(xs.SequenceEqual(new[]
            {
                1, 2, 3, 4, // original
                1, 2, 3, 4, // reentrant (+ fed back)
                1, 2, 3, 4, // reentrant (+ first two fed back)
                1, 2,       // reentrant
                5           // tune in
            }));
        }

#if !NO_INTERNALSTEST
        [Fact]
        public void FastImmediateObserver_Simple1()
        {
            var res = FastImmediateObserverTest(fio =>
            {
                fio.OnNext(1);
                fio.OnNext(2);
                fio.OnNext(3);
                fio.OnCompleted();

                fio.EnsureActive(4);
            });

            res.AssertEqual(
                OnNext(0, 1),
                OnNext(1, 2),
                OnNext(2, 3),
                OnCompleted<int>(3)
            );
        }

        [Fact]
        public void FastImmediateObserver_Simple2()
        {
            var ex = new Exception();

            var res = FastImmediateObserverTest(fio =>
            {
                fio.OnNext(1);
                fio.OnNext(2);
                fio.OnNext(3);
                fio.OnError(ex);

                fio.EnsureActive(4);
            });

            res.AssertEqual(
                OnNext(0, 1),
                OnNext(1, 2),
                OnNext(2, 3),
                OnError<int>(3, ex)
            );
        }

        [Fact]
        public void FastImmediateObserver_Simple3()
        {
            var res = FastImmediateObserverTest(fio =>
            {
                fio.OnNext(1);
                fio.EnsureActive();

                fio.OnNext(2);
                fio.EnsureActive();

                fio.OnNext(3);
                fio.EnsureActive();

                fio.OnCompleted();
                fio.EnsureActive();
            });

            res.AssertEqual(
                OnNext(0, 1),
                OnNext(1, 2),
                OnNext(2, 3),
                OnCompleted<int>(3)
            );
        }

        [Fact]
        public void FastImmediateObserver_Fault()
        {
            var xs = new List<int>();

            var o = Observer.Create<int>(
                x => { xs.Add(x); if (x == 2) { throw new Exception(); } },
                ex => { },
                () => { }
            );

            var fio = new FastImmediateObserver<int>(o);

            fio.OnNext(1);
            fio.OnNext(2);
            fio.OnNext(3);

            ReactiveAssert.Throws<Exception>(() => fio.EnsureActive());

            fio.OnNext(4);
            fio.EnsureActive();

            fio.OnNext(2);
            fio.EnsureActive();

            Assert.True(xs.Count == 2);
        }

        [Fact]
        public void FastImmediateObserver_Ownership1()
        {
            var xs = new List<int>();

            var o = Observer.Create<int>(
                xs.Add,
                ex => { },
                () => { }
            );

            var fio = new FastImmediateObserver<int>(o);

            var ts = new Task[16];
            var N = 100;

            for (var i = 0; i < ts.Length; i++)
            {
                var j = i;

                ts[i] = Task.Factory.StartNew(() =>
                {
                    for (var k = 0; k < N; k++)
                    {
                        fio.OnNext(j * N + k);
                    }

                    fio.EnsureActive(N);
                });
            }

            Task.WaitAll(ts);

            Assert.True(xs.Count == ts.Length * N);
        }

        [Fact]
        public void FastImmediateObserver_Ownership2()
        {
            var cd = new CountdownEvent(3);

            var w = new ManualResetEvent(false);
            var e = new ManualResetEvent(false);

            var xs = new List<int>();

            var o = Observer.Create<int>(
                x => { xs.Add(x); w.Set(); e.WaitOne(); cd.Signal(); },
                ex => { },
                () => { }
            );

            var fio = new FastImmediateObserver<int>(o);

            fio.OnNext(1);

            var t = Task.Factory.StartNew(() =>
            {
                fio.EnsureActive();
            });

            w.WaitOne();

            fio.OnNext(2);
            fio.OnNext(3);

            fio.EnsureActive(2);

            e.Set();

            cd.Wait();

            Assert.True(xs.Count == 3);
        }

        private IEnumerable<Recorded<Notification<int>>> FastImmediateObserverTest(Action<IScheduledObserver<int>> f)
        {
            var ns = new List<Recorded<Notification<int>>>();

            var l = 0L;

            var o = Observer.Create<int>(
                x => { ns.Add(OnNext(l++, x)); },
                ex => { ns.Add(OnError<int>(l++, ex)); },
                () => { ns.Add(OnCompleted<int>(l++)); }
            );

            var fio = new FastImmediateObserver<int>(o);

            f(fio);

            return ns;
        }
#endif
    }
}

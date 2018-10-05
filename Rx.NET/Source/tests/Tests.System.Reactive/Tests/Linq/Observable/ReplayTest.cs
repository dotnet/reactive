// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the Apache 2.0 License.
// See the LICENSE file in the project root for more information. 

using System;
using System.Linq;
using System.Reactive.Concurrency;
using System.Reactive.Linq;
using System.Reactive.Subjects;
using Microsoft.Reactive.Testing;
using ReactiveTests.Dummies;
using Xunit;

namespace ReactiveTests.Tests
{
    public class ReplayTest : ReactiveTest
    {

        [Fact]
        public void Replay_ArgumentChecking()
        {
            var someObservable = Observable.Empty<int>();
            var scheduler = new TestScheduler();

            ReactiveAssert.Throws<ArgumentNullException>(() => Observable.Replay(default(IObservable<int>)));
            ReactiveAssert.Throws<ArgumentNullException>(() => Observable.Replay(default(IObservable<int>), x => x));
            ReactiveAssert.Throws<ArgumentNullException>(() => Observable.Replay<int, int>(someObservable, null));
            ReactiveAssert.Throws<ArgumentNullException>(() => Observable.Replay<int>(null, DummyScheduler.Instance));
            ReactiveAssert.Throws<ArgumentNullException>(() => Observable.Replay(DummyObservable<int>.Instance, null));
            ReactiveAssert.Throws<ArgumentNullException>(() => Observable.Replay(null, DummyFunc<IObservable<int>, IObservable<int>>.Instance, DummyScheduler.Instance));
            ReactiveAssert.Throws<ArgumentNullException>(() => Observable.Replay<int, int>(DummyObservable<int>.Instance, null, DummyScheduler.Instance));
            ReactiveAssert.Throws<ArgumentNullException>(() => Observable.Replay(DummyObservable<int>.Instance, DummyFunc<IObservable<int>, IObservable<int>>.Instance, null));
            ReactiveAssert.Throws<ArgumentNullException>(() => Observable.Replay(default(IObservable<int>), TimeSpan.FromSeconds(1)));
            ReactiveAssert.Throws<ArgumentOutOfRangeException>(() => Observable.Replay(someObservable, TimeSpan.FromSeconds(-1)));
            ReactiveAssert.Throws<ArgumentNullException>(() => Observable.Replay(default(IObservable<int>), x => x, TimeSpan.FromSeconds(1)));
            ReactiveAssert.Throws<ArgumentNullException>(() => Observable.Replay<int, int>(someObservable, null, TimeSpan.FromSeconds(1)));
            ReactiveAssert.Throws<ArgumentOutOfRangeException>(() => Observable.Replay(someObservable, x => x, TimeSpan.FromSeconds(-1)));
            ReactiveAssert.Throws<ArgumentNullException>(() => Observable.Replay(default(IObservable<int>), TimeSpan.FromSeconds(1), scheduler));
            ReactiveAssert.Throws<ArgumentOutOfRangeException>(() => Observable.Replay(someObservable, TimeSpan.FromSeconds(-1), scheduler));
            ReactiveAssert.Throws<ArgumentNullException>(() => Observable.Replay(someObservable, TimeSpan.FromSeconds(1), default));
            ReactiveAssert.Throws<ArgumentNullException>(() => Observable.Replay(default(IObservable<int>), x => x, TimeSpan.FromSeconds(1), scheduler));
            ReactiveAssert.Throws<ArgumentNullException>(() => Observable.Replay<int, int>(someObservable, null, TimeSpan.FromSeconds(1), scheduler));
            ReactiveAssert.Throws<ArgumentOutOfRangeException>(() => Observable.Replay(someObservable, x => x, TimeSpan.FromSeconds(-1), scheduler));
            ReactiveAssert.Throws<ArgumentNullException>(() => Observable.Replay(someObservable, x => x, TimeSpan.FromSeconds(1), default));
            ReactiveAssert.Throws<ArgumentNullException>(() => Observable.Replay(default(IObservable<int>), 1, scheduler));
            ReactiveAssert.Throws<ArgumentOutOfRangeException>(() => Observable.Replay(someObservable, -2, scheduler));
            ReactiveAssert.Throws<ArgumentNullException>(() => Observable.Replay(someObservable, 1, default(IScheduler)));
            ReactiveAssert.Throws<ArgumentNullException>(() => Observable.Replay(default(IObservable<int>), x => x, 1, scheduler));
            ReactiveAssert.Throws<ArgumentNullException>(() => Observable.Replay<int, int>(someObservable, null, -2, scheduler));
            ReactiveAssert.Throws<ArgumentOutOfRangeException>(() => Observable.Replay(someObservable, x => x, -2, scheduler));
            ReactiveAssert.Throws<ArgumentNullException>(() => Observable.Replay(someObservable, x => x, 1, default(IScheduler)));
            ReactiveAssert.Throws<ArgumentNullException>(() => Observable.Replay(default(IObservable<int>), 1));
            ReactiveAssert.Throws<ArgumentOutOfRangeException>(() => Observable.Replay(someObservable, -2));
            ReactiveAssert.Throws<ArgumentNullException>(() => Observable.Replay(default(IObservable<int>), x => x, 1));
            ReactiveAssert.Throws<ArgumentNullException>(() => Observable.Replay<int, int>(someObservable, null, 1));
            ReactiveAssert.Throws<ArgumentOutOfRangeException>(() => Observable.Replay(someObservable, x => x, -2));
            ReactiveAssert.Throws<ArgumentNullException>(() => Observable.Replay(default(IObservable<int>), 1, TimeSpan.FromSeconds(1)));
            ReactiveAssert.Throws<ArgumentOutOfRangeException>(() => Observable.Replay(someObservable, -2, TimeSpan.FromSeconds(1)));
            ReactiveAssert.Throws<ArgumentOutOfRangeException>(() => Observable.Replay(someObservable, 1, TimeSpan.FromSeconds(-1)));
            ReactiveAssert.Throws<ArgumentNullException>(() => Observable.Replay(default(IObservable<int>), x => x, 1, TimeSpan.FromSeconds(1)));
            ReactiveAssert.Throws<ArgumentNullException>(() => Observable.Replay<int, int>(someObservable, null, 1, TimeSpan.FromSeconds(1)));
            ReactiveAssert.Throws<ArgumentOutOfRangeException>(() => Observable.Replay(someObservable, x => x, -2, TimeSpan.FromSeconds(1)));
            ReactiveAssert.Throws<ArgumentOutOfRangeException>(() => Observable.Replay(someObservable, x => x, 1, TimeSpan.FromSeconds(-1)));
            ReactiveAssert.Throws<ArgumentNullException>(() => Observable.Replay(default(IObservable<int>), 1, TimeSpan.FromSeconds(1), scheduler));
            ReactiveAssert.Throws<ArgumentOutOfRangeException>(() => Observable.Replay(someObservable, -2, TimeSpan.FromSeconds(1), scheduler));
            ReactiveAssert.Throws<ArgumentOutOfRangeException>(() => Observable.Replay(someObservable, 1, TimeSpan.FromSeconds(-1), scheduler));
            ReactiveAssert.Throws<ArgumentNullException>(() => Observable.Replay(someObservable, 1, TimeSpan.FromSeconds(1), null));
            ReactiveAssert.Throws<ArgumentNullException>(() => Observable.Replay(default(IObservable<int>), x => x, 1, TimeSpan.FromSeconds(1), scheduler));
            ReactiveAssert.Throws<ArgumentNullException>(() => Observable.Replay<int, int>(someObservable, null, 1, TimeSpan.FromSeconds(1), scheduler));
            ReactiveAssert.Throws<ArgumentOutOfRangeException>(() => Observable.Replay(someObservable, x => x, -2, TimeSpan.FromSeconds(1), scheduler));
            ReactiveAssert.Throws<ArgumentOutOfRangeException>(() => Observable.Replay(someObservable, x => x, 1, TimeSpan.FromSeconds(-1), scheduler));
            ReactiveAssert.Throws<ArgumentNullException>(() => Observable.Replay(someObservable, x => x, 1, TimeSpan.FromSeconds(1), null));
        }

        [Fact]
        public void ReplayCount_Basic()
        {
            var scheduler = new TestScheduler();

            var xs = scheduler.CreateHotObservable(
                OnNext(110, 7),
                OnNext(220, 3),
                OnNext(280, 4),
                OnNext(290, 1),
                OnNext(340, 8),
                OnNext(360, 5),
                OnNext(370, 6),
                OnNext(390, 7),
                OnNext(410, 13),
                OnNext(430, 2),
                OnNext(450, 9),
                OnNext(520, 11),
                OnNext(560, 20),
                OnCompleted<int>(600)
            );

            var ys = default(IConnectableObservable<int>);
            var subscription = default(IDisposable);
            var connection = default(IDisposable);
            var res = scheduler.CreateObserver<int>();

            scheduler.ScheduleAbsolute(Created, () => ys = xs.Replay(3, scheduler));
            scheduler.ScheduleAbsolute(450, () => subscription = ys.Subscribe(res));
            scheduler.ScheduleAbsolute(Disposed, () => subscription.Dispose());

            scheduler.ScheduleAbsolute(300, () => connection = ys.Connect());
            scheduler.ScheduleAbsolute(400, () => connection.Dispose());

            scheduler.ScheduleAbsolute(500, () => connection = ys.Connect());
            scheduler.ScheduleAbsolute(550, () => connection.Dispose());

            scheduler.ScheduleAbsolute(650, () => connection = ys.Connect());
            scheduler.ScheduleAbsolute(800, () => connection.Dispose());

            scheduler.Start();

            res.Messages.AssertEqual(
                OnNext(451, 5),
                OnNext(452, 6),
                OnNext(453, 7),
                OnNext(521, 11)
            );

            xs.Subscriptions.AssertEqual(
                Subscribe(300, 400),
                Subscribe(500, 550),
                Subscribe(650, 800)
            );
        }

        [Fact]
        public void ReplayCount_Error()
        {
            var scheduler = new TestScheduler();

            var ex = new Exception();

            var xs = scheduler.CreateHotObservable(
                OnNext(110, 7),
                OnNext(220, 3),
                OnNext(280, 4),
                OnNext(290, 1),
                OnNext(340, 8),
                OnNext(360, 5),
                OnNext(370, 6),
                OnNext(390, 7),
                OnNext(410, 13),
                OnNext(430, 2),
                OnNext(450, 9),
                OnNext(520, 11),
                OnNext(560, 20),
                OnError<int>(600, ex)
            );

            var ys = default(IConnectableObservable<int>);
            var subscription = default(IDisposable);
            var connection = default(IDisposable);
            var res = scheduler.CreateObserver<int>();

            scheduler.ScheduleAbsolute(Created, () => ys = xs.Replay(3, scheduler));
            scheduler.ScheduleAbsolute(450, () => subscription = ys.Subscribe(res));
            scheduler.ScheduleAbsolute(Disposed, () => subscription.Dispose());

            scheduler.ScheduleAbsolute(300, () => connection = ys.Connect());
            scheduler.ScheduleAbsolute(400, () => connection.Dispose());

            scheduler.ScheduleAbsolute(500, () => connection = ys.Connect());
            scheduler.ScheduleAbsolute(800, () => connection.Dispose());

            scheduler.Start();

            res.Messages.AssertEqual(
                OnNext(451, 5),
                OnNext(452, 6),
                OnNext(453, 7),
                OnNext(521, 11),
                OnNext(561, 20),
                OnError<int>(601, ex)
            );

            xs.Subscriptions.AssertEqual(
                Subscribe(300, 400),
                Subscribe(500, 600)
            );
        }

        [Fact]
        public void ReplayCount_Complete()
        {
            var scheduler = new TestScheduler();

            var xs = scheduler.CreateHotObservable(
                OnNext(110, 7),
                OnNext(220, 3),
                OnNext(280, 4),
                OnNext(290, 1),
                OnNext(340, 8),
                OnNext(360, 5),
                OnNext(370, 6),
                OnNext(390, 7),
                OnNext(410, 13),
                OnNext(430, 2),
                OnNext(450, 9),
                OnNext(520, 11),
                OnNext(560, 20),
                OnCompleted<int>(600)
            );

            var ys = default(IConnectableObservable<int>);
            var subscription = default(IDisposable);
            var connection = default(IDisposable);
            var res = scheduler.CreateObserver<int>();

            scheduler.ScheduleAbsolute(Created, () => ys = xs.Replay(3, scheduler));
            scheduler.ScheduleAbsolute(450, () => subscription = ys.Subscribe(res));
            scheduler.ScheduleAbsolute(Disposed, () => subscription.Dispose());

            scheduler.ScheduleAbsolute(300, () => connection = ys.Connect());
            scheduler.ScheduleAbsolute(400, () => connection.Dispose());

            scheduler.ScheduleAbsolute(500, () => connection = ys.Connect());
            scheduler.ScheduleAbsolute(800, () => connection.Dispose());

            scheduler.Start();

            res.Messages.AssertEqual(
                OnNext(451, 5),
                OnNext(452, 6),
                OnNext(453, 7),
                OnNext(521, 11),
                OnNext(561, 20),
                OnCompleted<int>(601)
            );

            xs.Subscriptions.AssertEqual(
                Subscribe(300, 400),
                Subscribe(500, 600)
            );
        }

        [Fact]
        public void ReplayCount_Dispose()
        {
            var scheduler = new TestScheduler();

            var xs = scheduler.CreateHotObservable(
                OnNext(110, 7),
                OnNext(220, 3),
                OnNext(280, 4),
                OnNext(290, 1),
                OnNext(340, 8),
                OnNext(360, 5),
                OnNext(370, 6),
                OnNext(390, 7),
                OnNext(410, 13),
                OnNext(430, 2),
                OnNext(450, 9),
                OnNext(520, 11),
                OnNext(560, 20),
                OnCompleted<int>(600)
            );

            var ys = default(IConnectableObservable<int>);
            var subscription = default(IDisposable);
            var connection = default(IDisposable);
            var res = scheduler.CreateObserver<int>();

            scheduler.ScheduleAbsolute(Created, () => ys = xs.Replay(3, scheduler));
            scheduler.ScheduleAbsolute(450, () => subscription = ys.Subscribe(res));
            scheduler.ScheduleAbsolute(475, () => subscription.Dispose());

            scheduler.ScheduleAbsolute(300, () => connection = ys.Connect());
            scheduler.ScheduleAbsolute(400, () => connection.Dispose());

            scheduler.ScheduleAbsolute(500, () => connection = ys.Connect());
            scheduler.ScheduleAbsolute(550, () => connection.Dispose());

            scheduler.ScheduleAbsolute(650, () => connection = ys.Connect());
            scheduler.ScheduleAbsolute(800, () => connection.Dispose());

            scheduler.Start();

            res.Messages.AssertEqual(
                OnNext(451, 5),
                OnNext(452, 6),
                OnNext(453, 7)
            );

            xs.Subscriptions.AssertEqual(
                Subscribe(300, 400),
                Subscribe(500, 550),
                Subscribe(650, 800)
            );
        }

        [Fact]
        public void ReplayCount_MultipleConnections()
        {
            var xs = Observable.Never<int>();
            var ys = xs.Replay(3, new TestScheduler());

            var connection1 = ys.Connect();
            var connection2 = ys.Connect();

            Assert.Same(connection1, connection2);

            connection1.Dispose();
            connection2.Dispose();

            var connection3 = ys.Connect();

            Assert.NotSame(connection1, connection3);

            connection3.Dispose();
        }

        [Fact]
        public void ReplayCountLambda_Zip_Complete()
        {
            var scheduler = new TestScheduler();

            var xs = scheduler.CreateHotObservable(
                OnNext(110, 7),
                OnNext(220, 3),
                OnNext(280, 4),
                OnNext(290, 1),
                OnNext(340, 8),
                OnNext(360, 5),
                OnNext(370, 6),
                OnNext(390, 7),
                OnNext(410, 13),
                OnNext(430, 2),
                OnNext(450, 9),
                OnNext(520, 11),
                OnNext(560, 20),
                OnCompleted<int>(600)
            );

            var res = scheduler.Start(() =>
                xs.Replay(_xs => _xs.Take(6).Repeat(), 3, scheduler),
                610
            );

            res.Messages.AssertEqual(
                OnNext(221, 3),
                OnNext(281, 4),
                OnNext(291, 1),
                OnNext(341, 8),
                OnNext(361, 5),
                OnNext(371, 6),
                OnNext(372, 8),
                OnNext(373, 5),
                OnNext(374, 6),
                OnNext(391, 7),
                OnNext(411, 13),
                OnNext(431, 2),
                OnNext(432, 7),
                OnNext(433, 13),
                OnNext(434, 2),
                OnNext(451, 9),
                OnNext(521, 11),
                OnNext(561, 20),
                OnNext(562, 9),
                OnNext(563, 11),
                OnNext(564, 20),
                OnNext(602, 9),
                OnNext(603, 11),
                OnNext(604, 20),
                OnNext(606, 9),
                OnNext(607, 11),
                OnNext(608, 20)
            );

            xs.Subscriptions.AssertEqual(
                Subscribe(200, 600)
            );
        }

        [Fact]
        public void ReplayCountLambda_Zip_Error()
        {
            var scheduler = new TestScheduler();

            var ex = new Exception();

            var xs = scheduler.CreateHotObservable(
                OnNext(110, 7),
                OnNext(220, 3),
                OnNext(280, 4),
                OnNext(290, 1),
                OnNext(340, 8),
                OnNext(360, 5),
                OnNext(370, 6),
                OnNext(390, 7),
                OnNext(410, 13),
                OnNext(430, 2),
                OnNext(450, 9),
                OnNext(520, 11),
                OnNext(560, 20),
                OnError<int>(600, ex)
            );

            var res = scheduler.Start(() =>
                xs.Replay(_xs => _xs.Take(6).Repeat(), 3, scheduler)
            );

            res.Messages.AssertEqual(
                OnNext(221, 3),
                OnNext(281, 4),
                OnNext(291, 1),
                OnNext(341, 8),
                OnNext(361, 5),
                OnNext(371, 6),
                OnNext(372, 8),
                OnNext(373, 5),
                OnNext(374, 6),
                OnNext(391, 7),
                OnNext(411, 13),
                OnNext(431, 2),
                OnNext(432, 7),
                OnNext(433, 13),
                OnNext(434, 2),
                OnNext(451, 9),
                OnNext(521, 11),
                OnNext(561, 20),
                OnNext(562, 9),
                OnNext(563, 11),
                OnNext(564, 20),
                OnError<int>(601, ex)
            );

            xs.Subscriptions.AssertEqual(
                Subscribe(200, 600)
            );
        }

        [Fact]
        public void ReplayCountLambda_Zip_Dispose()
        {
            var scheduler = new TestScheduler();

            var xs = scheduler.CreateHotObservable(
                OnNext(110, 7),
                OnNext(220, 3),
                OnNext(280, 4),
                OnNext(290, 1),
                OnNext(340, 8),
                OnNext(360, 5),
                OnNext(370, 6),
                OnNext(390, 7),
                OnNext(410, 13),
                OnNext(430, 2),
                OnNext(450, 9),
                OnNext(520, 11),
                OnNext(560, 20),
                OnCompleted<int>(600)
            );

            var res = scheduler.Start(() =>
                xs.Replay(_xs => _xs.Take(6).Repeat(), 3, scheduler),
                470
            );

            res.Messages.AssertEqual(
                OnNext(221, 3),
                OnNext(281, 4),
                OnNext(291, 1),
                OnNext(341, 8),
                OnNext(361, 5),
                OnNext(371, 6),
                OnNext(372, 8),
                OnNext(373, 5),
                OnNext(374, 6),
                OnNext(391, 7),
                OnNext(411, 13),
                OnNext(431, 2),
                OnNext(432, 7),
                OnNext(433, 13),
                OnNext(434, 2),
                OnNext(451, 9)
            );

            xs.Subscriptions.AssertEqual(
                Subscribe(200, 470)
            );
        }

        [Fact]
        public void ReplayTime_Basic()
        {
            var scheduler = new TestScheduler();

            var xs = scheduler.CreateHotObservable(
                OnNext(110, 7),
                OnNext(220, 3),
                OnNext(280, 4),
                OnNext(290, 1),
                OnNext(340, 8),
                OnNext(360, 5),
                OnNext(370, 6),
                OnNext(390, 7),
                OnNext(410, 13),
                OnNext(430, 2),
                OnNext(450, 9),
                OnNext(520, 11),
                OnNext(560, 20),
                OnCompleted<int>(600)
            );

            var ys = default(IConnectableObservable<int>);
            var subscription = default(IDisposable);
            var connection = default(IDisposable);
            var res = scheduler.CreateObserver<int>();

            scheduler.ScheduleAbsolute(Created, () => ys = xs.Replay(TimeSpan.FromTicks(150), scheduler));
            scheduler.ScheduleAbsolute(450, () => subscription = ys.Subscribe(res));
            scheduler.ScheduleAbsolute(Disposed, () => subscription.Dispose());

            scheduler.ScheduleAbsolute(300, () => connection = ys.Connect());
            scheduler.ScheduleAbsolute(400, () => connection.Dispose());

            scheduler.ScheduleAbsolute(500, () => connection = ys.Connect());
            scheduler.ScheduleAbsolute(550, () => connection.Dispose());

            scheduler.ScheduleAbsolute(650, () => connection = ys.Connect());
            scheduler.ScheduleAbsolute(800, () => connection.Dispose());

            scheduler.Start();

            res.Messages.AssertEqual(
                OnNext(451, 8),
                OnNext(452, 5),
                OnNext(453, 6),
                OnNext(454, 7),
                OnNext(521, 11)
            );

            xs.Subscriptions.AssertEqual(
                Subscribe(300, 400),
                Subscribe(500, 550),
                Subscribe(650, 800)
            );
        }

        [Fact]
        public void ReplayTime_Error()
        {
            var scheduler = new TestScheduler();

            var ex = new Exception();

            var xs = scheduler.CreateHotObservable(
                OnNext(110, 7),
                OnNext(220, 3),
                OnNext(280, 4),
                OnNext(290, 1),
                OnNext(340, 8),
                OnNext(360, 5),
                OnNext(370, 6),
                OnNext(390, 7),
                OnNext(410, 13),
                OnNext(430, 2),
                OnNext(450, 9),
                OnNext(520, 11),
                OnNext(560, 20),
                OnError<int>(600, ex)
            );

            var ys = default(IConnectableObservable<int>);
            var subscription = default(IDisposable);
            var connection = default(IDisposable);
            var res = scheduler.CreateObserver<int>();

            scheduler.ScheduleAbsolute(Created, () => ys = xs.Replay(TimeSpan.FromTicks(75), scheduler));
            scheduler.ScheduleAbsolute(450, () => subscription = ys.Subscribe(res));
            scheduler.ScheduleAbsolute(Disposed, () => subscription.Dispose());

            scheduler.ScheduleAbsolute(300, () => connection = ys.Connect());
            scheduler.ScheduleAbsolute(400, () => connection.Dispose());

            scheduler.ScheduleAbsolute(500, () => connection = ys.Connect());
            scheduler.ScheduleAbsolute(800, () => connection.Dispose());

            scheduler.Start();

            res.Messages.AssertEqual(
                OnNext(451, 7),
                OnNext(521, 11),
                OnNext(561, 20),
                OnError<int>(601, ex)
            );

            xs.Subscriptions.AssertEqual(
                Subscribe(300, 400),
                Subscribe(500, 600)
            );
        }

        [Fact]
        public void ReplayTime_Complete()
        {
            var scheduler = new TestScheduler();

            var xs = scheduler.CreateHotObservable(
                OnNext(110, 7),
                OnNext(220, 3),
                OnNext(280, 4),
                OnNext(290, 1),
                OnNext(340, 8),
                OnNext(360, 5),
                OnNext(370, 6),
                OnNext(390, 7),
                OnNext(410, 13),
                OnNext(430, 2),
                OnNext(450, 9),
                OnNext(520, 11),
                OnNext(560, 20),
                OnCompleted<int>(600)
            );

            var ys = default(IConnectableObservable<int>);
            var subscription = default(IDisposable);
            var connection = default(IDisposable);
            var res = scheduler.CreateObserver<int>();

            scheduler.ScheduleAbsolute(Created, () => ys = xs.Replay(TimeSpan.FromTicks(85), scheduler));
            scheduler.ScheduleAbsolute(450, () => subscription = ys.Subscribe(res));
            scheduler.ScheduleAbsolute(Disposed, () => subscription.Dispose());

            scheduler.ScheduleAbsolute(300, () => connection = ys.Connect());
            scheduler.ScheduleAbsolute(400, () => connection.Dispose());

            scheduler.ScheduleAbsolute(500, () => connection = ys.Connect());
            scheduler.ScheduleAbsolute(800, () => connection.Dispose());

            scheduler.Start();

            res.Messages.AssertEqual(
                OnNext(451, 6),
                OnNext(452, 7),
                OnNext(521, 11),
                OnNext(561, 20),
                OnCompleted<int>(601)
            );

            xs.Subscriptions.AssertEqual(
                Subscribe(300, 400),
                Subscribe(500, 600)
            );
        }

        [Fact]
        public void ReplayTime_Dispose()
        {
            var scheduler = new TestScheduler();

            var xs = scheduler.CreateHotObservable(
                OnNext(110, 7),
                OnNext(220, 3),
                OnNext(280, 4),
                OnNext(290, 1),
                OnNext(340, 8),
                OnNext(360, 5),
                OnNext(370, 6),
                OnNext(390, 7),
                OnNext(410, 13),
                OnNext(430, 2),
                OnNext(450, 9),
                OnNext(520, 11),
                OnNext(560, 20),
                OnCompleted<int>(600)
            );

            var ys = default(IConnectableObservable<int>);
            var subscription = default(IDisposable);
            var connection = default(IDisposable);
            var res = scheduler.CreateObserver<int>();

            scheduler.ScheduleAbsolute(Created, () => ys = xs.Replay(TimeSpan.FromTicks(100), scheduler));
            scheduler.ScheduleAbsolute(450, () => subscription = ys.Subscribe(res));
            scheduler.ScheduleAbsolute(475, () => subscription.Dispose());

            scheduler.ScheduleAbsolute(300, () => connection = ys.Connect());
            scheduler.ScheduleAbsolute(400, () => connection.Dispose());

            scheduler.ScheduleAbsolute(500, () => connection = ys.Connect());
            scheduler.ScheduleAbsolute(550, () => connection.Dispose());

            scheduler.ScheduleAbsolute(650, () => connection = ys.Connect());
            scheduler.ScheduleAbsolute(800, () => connection.Dispose());

            scheduler.Start();

            res.Messages.AssertEqual(
                OnNext(451, 5),
                OnNext(452, 6),
                OnNext(453, 7)
            );

            xs.Subscriptions.AssertEqual(
                Subscribe(300, 400),
                Subscribe(500, 550),
                Subscribe(650, 800)
            );
        }

        [Fact]
        public void ReplayTime_MultipleConnections()
        {
            var xs = Observable.Never<int>();
            var ys = xs.Replay(TimeSpan.FromTicks(100), new TestScheduler());

            var connection1 = ys.Connect();
            var connection2 = ys.Connect();

            Assert.Same(connection1, connection2);

            connection1.Dispose();
            connection2.Dispose();

            var connection3 = ys.Connect();

            Assert.NotSame(connection1, connection3);

            connection3.Dispose();
        }

        [Fact]
        public void ReplayTimeLambda_Zip_Complete()
        {
            var scheduler = new TestScheduler();

            var xs = scheduler.CreateHotObservable(
                OnNext(110, 7),
                OnNext(220, 3),
                OnNext(280, 4),
                OnNext(290, 1),
                OnNext(340, 8),
                OnNext(360, 5),
                OnNext(370, 6),
                OnNext(390, 7),
                OnNext(410, 13),
                OnNext(430, 2),
                OnNext(450, 9),
                OnNext(520, 11),
                OnNext(560, 20),
                OnCompleted<int>(600)
            );

            var res = scheduler.Start(() =>
                xs.Replay(_xs => _xs.Take(6).Repeat(), TimeSpan.FromTicks(50), scheduler),
                610
            );

            res.Messages.AssertEqual(
                OnNext(221, 3),
                OnNext(281, 4),
                OnNext(291, 1),
                OnNext(341, 8),
                OnNext(361, 5),
                OnNext(371, 6),
                OnNext(372, 8),
                OnNext(373, 5),
                OnNext(374, 6),
                OnNext(391, 7),
                OnNext(411, 13),
                OnNext(431, 2),
                OnNext(432, 7),
                OnNext(433, 13),
                OnNext(434, 2),
                OnNext(451, 9),
                OnNext(521, 11),
                OnNext(561, 20),
                OnNext(562, 11),
                OnNext(563, 20),
                OnNext(602, 20),
                OnNext(604, 20),
                OnNext(606, 20),
                OnNext(608, 20)
            );

            xs.Subscriptions.AssertEqual(
                Subscribe(200, 600)
            );
        }

        [Fact]
        public void ReplayTimeLambda_Zip_Error()
        {
            var scheduler = new TestScheduler();

            var ex = new Exception();

            var xs = scheduler.CreateHotObservable(
                OnNext(110, 7),
                OnNext(220, 3),
                OnNext(280, 4),
                OnNext(290, 1),
                OnNext(340, 8),
                OnNext(360, 5),
                OnNext(370, 6),
                OnNext(390, 7),
                OnNext(410, 13),
                OnNext(430, 2),
                OnNext(450, 9),
                OnNext(520, 11),
                OnNext(560, 20),
                OnError<int>(600, ex)
            );

            var res = scheduler.Start(() =>
                xs.Replay(_xs => _xs.Take(6).Repeat(), TimeSpan.FromTicks(50), scheduler)
            );

            res.Messages.AssertEqual(
                OnNext(221, 3),
                OnNext(281, 4),
                OnNext(291, 1),
                OnNext(341, 8),
                OnNext(361, 5),
                OnNext(371, 6),
                OnNext(372, 8),
                OnNext(373, 5),
                OnNext(374, 6),
                OnNext(391, 7),
                OnNext(411, 13),
                OnNext(431, 2),
                OnNext(432, 7),
                OnNext(433, 13),
                OnNext(434, 2),
                OnNext(451, 9),
                OnNext(521, 11),
                OnNext(561, 20),
                OnNext(562, 11),
                OnNext(563, 20),
                OnError<int>(601, ex)
            );

            xs.Subscriptions.AssertEqual(
                Subscribe(200, 600)
            );
        }

        [Fact]
        public void ReplayTimeLambda_Zip_Dispose()
        {
            var scheduler = new TestScheduler();

            var xs = scheduler.CreateHotObservable(
                OnNext(110, 7),
                OnNext(220, 3),
                OnNext(280, 4),
                OnNext(290, 1),
                OnNext(340, 8),
                OnNext(360, 5),
                OnNext(370, 6),
                OnNext(390, 7),
                OnNext(410, 13),
                OnNext(430, 2),
                OnNext(450, 9),
                OnNext(520, 11),
                OnNext(560, 20),
                OnCompleted<int>(600)
            );

            var res = scheduler.Start(() =>
                xs.Replay(_xs => _xs.Take(6).Repeat(), TimeSpan.FromTicks(50), scheduler),
                470
            );

            res.Messages.AssertEqual(
                OnNext(221, 3),
                OnNext(281, 4),
                OnNext(291, 1),
                OnNext(341, 8),
                OnNext(361, 5),
                OnNext(371, 6),
                OnNext(372, 8),
                OnNext(373, 5),
                OnNext(374, 6),
                OnNext(391, 7),
                OnNext(411, 13),
                OnNext(431, 2),
                OnNext(432, 7),
                OnNext(433, 13),
                OnNext(434, 2),
                OnNext(451, 9)
            );

            xs.Subscriptions.AssertEqual(
                Subscribe(200, 470)
            );
        }

        [Fact]
        public void Replay_Default1()
        {
            var s = new Subject<int>();
            var xs = s.Replay(100, DefaultScheduler.Instance);
            var ys = s.Replay(100);

            xs.Connect();
            ys.Connect();

            s.OnNext(1);
            s.OnNext(2);
            s.OnCompleted();

            xs.AssertEqual(ys);
        }

        [Fact]
        public void Replay_Default2()
        {
            var s = new Subject<int>();
            var xs = s.Replay(TimeSpan.FromHours(1), DefaultScheduler.Instance);
            var ys = s.Replay(TimeSpan.FromHours(1));

            xs.Connect();
            ys.Connect();

            s.OnNext(1);
            s.OnNext(2);
            s.OnCompleted();

            xs.AssertEqual(ys);
        }

        [Fact]
        public void Replay_Default3()
        {
            var s = new Subject<int>();
            var xs = s.Replay(100, TimeSpan.FromHours(1), DefaultScheduler.Instance);
            var ys = s.Replay(100, TimeSpan.FromHours(1));

            xs.Connect();
            ys.Connect();

            s.OnNext(1);
            s.OnNext(2);
            s.OnCompleted();

            xs.AssertEqual(ys);
        }

        [Fact]
        public void Replay_Default4()
        {
            var s = new Subject<int>();
            var xs = s.Replay(DefaultScheduler.Instance);
            var ys = s.Replay();

            xs.Connect();
            ys.Connect();

            s.OnNext(1);
            s.OnNext(2);
            s.OnCompleted();

            xs.AssertEqual(ys);
        }

        [Fact]
        public void ReplayLambda_Default1()
        {
            var xs = Observable.Range(1, 10).Replay(_xs => _xs, 100, DefaultScheduler.Instance);
            var ys = Observable.Range(1, 10).Replay(_xs => _xs, 100);

            xs.AssertEqual(ys);
        }

        [Fact]
        public void ReplayLambda_Default2()
        {
            var xs = Observable.Range(1, 10).Replay(_xs => _xs, TimeSpan.FromHours(1), DefaultScheduler.Instance);
            var ys = Observable.Range(1, 10).Replay(_xs => _xs, TimeSpan.FromHours(1));

            xs.AssertEqual(ys);
        }

        [Fact]
        public void ReplayLambda_Default3()
        {
            var xs = Observable.Range(1, 10).Replay(_xs => _xs, 100, TimeSpan.FromHours(1), DefaultScheduler.Instance);
            var ys = Observable.Range(1, 10).Replay(_xs => _xs, 100, TimeSpan.FromHours(1));

            xs.AssertEqual(ys);
        }

        [Fact]
        public void ReplayLambda_Default4()
        {
            var xs = Observable.Range(1, 10).Replay(_xs => _xs, DefaultScheduler.Instance);
            var ys = Observable.Range(1, 10).Replay(_xs => _xs);

            xs.AssertEqual(ys);
        }

    }
}

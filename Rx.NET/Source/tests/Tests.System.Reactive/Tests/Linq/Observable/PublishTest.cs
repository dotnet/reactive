// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the Apache 2.0 License.
// See the LICENSE file in the project root for more information. 

using System;
using System.Linq;
using System.Reactive.Concurrency;
using System.Reactive.Linq;
using System.Reactive.Subjects;
using Microsoft.Reactive.Testing;
using Xunit;

namespace ReactiveTests.Tests
{
    public class PublishTest : ReactiveTest
    {
        [Fact]
        public void Publish_Cold_Zip()
        {
            var scheduler = new TestScheduler();

            var xs = scheduler.CreateHotObservable(
                OnNext(40, 0),
                OnNext(90, 1),
                OnNext(150, 2),
                OnNext(210, 3),
                OnNext(240, 4),
                OnNext(270, 5),
                OnNext(330, 6),
                OnNext(340, 7),
                OnCompleted<int>(390)
            );

            var res = scheduler.Start(() =>
                xs.Publish(ys => ys.Zip(ys, (a, b) => a + b))
            );

            res.Messages.AssertEqual(
                OnNext(210, 6),
                OnNext(240, 8),
                OnNext(270, 10),
                OnNext(330, 12),
                OnNext(340, 14),
                OnCompleted<int>(390)
            );

            xs.Subscriptions.AssertEqual(
                Subscribe(200, 390)
            );
        }

        [Fact]
        public void Publish_ArgumentChecking()
        {
            var someObservable = Observable.Empty<int>();

            ReactiveAssert.Throws<ArgumentNullException>(() => Observable.Publish(default(IObservable<int>)));
            ReactiveAssert.Throws<ArgumentNullException>(() => Observable.Publish(default(IObservable<int>), x => x));
            ReactiveAssert.Throws<ArgumentNullException>(() => Observable.Publish<int, int>(someObservable, null));
        }

        [Fact]
        public void Publish_Basic()
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

            scheduler.ScheduleAbsolute(Created, () => ys = xs.Publish());
            scheduler.ScheduleAbsolute(Subscribed, () => subscription = ys.Subscribe(res));
            scheduler.ScheduleAbsolute(Disposed, () => subscription.Dispose());

            scheduler.ScheduleAbsolute(300, () => connection = ys.Connect());
            scheduler.ScheduleAbsolute(400, () => connection.Dispose());

            scheduler.ScheduleAbsolute(500, () => connection = ys.Connect());
            scheduler.ScheduleAbsolute(550, () => connection.Dispose());

            scheduler.ScheduleAbsolute(650, () => connection = ys.Connect());
            scheduler.ScheduleAbsolute(800, () => connection.Dispose());

            scheduler.Start();

            res.Messages.AssertEqual(
                OnNext(340, 8),
                OnNext(360, 5),
                OnNext(370, 6),
                OnNext(390, 7),
                OnNext(520, 11)
            );

            xs.Subscriptions.AssertEqual(
                Subscribe(300, 400),
                Subscribe(500, 550),
                Subscribe(650, 800)
            );
        }

        [Fact]
        public void Publish_Error()
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

            scheduler.ScheduleAbsolute(Created, () => ys = xs.Publish());
            scheduler.ScheduleAbsolute(Subscribed, () => subscription = ys.Subscribe(res));
            scheduler.ScheduleAbsolute(Disposed, () => subscription.Dispose());

            scheduler.ScheduleAbsolute(300, () => connection = ys.Connect());
            scheduler.ScheduleAbsolute(400, () => connection.Dispose());

            scheduler.ScheduleAbsolute(500, () => connection = ys.Connect());
            scheduler.ScheduleAbsolute(800, () => connection.Dispose());

            scheduler.Start();

            res.Messages.AssertEqual(
                OnNext(340, 8),
                OnNext(360, 5),
                OnNext(370, 6),
                OnNext(390, 7),
                OnNext(520, 11),
                OnNext(560, 20),
                OnError<int>(600, ex)
            );

            xs.Subscriptions.AssertEqual(
                Subscribe(300, 400),
                Subscribe(500, 600)
            );
        }

        [Fact]
        public void Publish_Complete()
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

            scheduler.ScheduleAbsolute(Created, () => ys = xs.Publish());
            scheduler.ScheduleAbsolute(Subscribed, () => subscription = ys.Subscribe(res));
            scheduler.ScheduleAbsolute(Disposed, () => subscription.Dispose());

            scheduler.ScheduleAbsolute(300, () => connection = ys.Connect());
            scheduler.ScheduleAbsolute(400, () => connection.Dispose());

            scheduler.ScheduleAbsolute(500, () => connection = ys.Connect());
            scheduler.ScheduleAbsolute(800, () => connection.Dispose());

            scheduler.Start();

            res.Messages.AssertEqual(
                OnNext(340, 8),
                OnNext(360, 5),
                OnNext(370, 6),
                OnNext(390, 7),
                OnNext(520, 11),
                OnNext(560, 20),
                OnCompleted<int>(600)
            );

            xs.Subscriptions.AssertEqual(
                Subscribe(300, 400),
                Subscribe(500, 600)
            );
        }

        [Fact]
        public void Publish_Dispose()
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

            scheduler.ScheduleAbsolute(Created, () => ys = xs.Publish());
            scheduler.ScheduleAbsolute(Subscribed, () => subscription = ys.Subscribe(res));
            scheduler.ScheduleAbsolute(350, () => subscription.Dispose());

            scheduler.ScheduleAbsolute(300, () => connection = ys.Connect());
            scheduler.ScheduleAbsolute(400, () => connection.Dispose());

            scheduler.ScheduleAbsolute(500, () => connection = ys.Connect());
            scheduler.ScheduleAbsolute(550, () => connection.Dispose());

            scheduler.ScheduleAbsolute(650, () => connection = ys.Connect());
            scheduler.ScheduleAbsolute(800, () => connection.Dispose());

            scheduler.Start();

            res.Messages.AssertEqual(
                OnNext(340, 8)
            );

            xs.Subscriptions.AssertEqual(
                Subscribe(300, 400),
                Subscribe(500, 550),
                Subscribe(650, 800)
            );
        }

        [Fact]
        public void Publish_MultipleConnections()
        {
            var xs = Observable.Never<int>();
            var ys = xs.Publish();

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
        public void PublishLambda_Zip_Complete()
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
                xs.Publish(_xs => _xs.Zip(_xs.Skip(1), (prev, cur) => cur + prev))
            );

            res.Messages.AssertEqual(
                OnNext(280, 7),
                OnNext(290, 5),
                OnNext(340, 9),
                OnNext(360, 13),
                OnNext(370, 11),
                OnNext(390, 13),
                OnNext(410, 20),
                OnNext(430, 15),
                OnNext(450, 11),
                OnNext(520, 20),
                OnNext(560, 31),
                OnCompleted<int>(600)
            );

            xs.Subscriptions.AssertEqual(
                Subscribe(200, 600)
            );
        }

        [Fact]
        public void PublishLambda_Zip_Error()
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
                xs.Publish(_xs => _xs.Zip(_xs.Skip(1), (prev, cur) => cur + prev))
            );

            res.Messages.AssertEqual(
                OnNext(280, 7),
                OnNext(290, 5),
                OnNext(340, 9),
                OnNext(360, 13),
                OnNext(370, 11),
                OnNext(390, 13),
                OnNext(410, 20),
                OnNext(430, 15),
                OnNext(450, 11),
                OnNext(520, 20),
                OnNext(560, 31),
                OnError<int>(600, ex)
            );

            xs.Subscriptions.AssertEqual(
                Subscribe(200, 600)
            );
        }

        [Fact]
        public void PublishLambda_Zip_Dispose()
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
                xs.Publish(_xs => _xs.Zip(_xs.Skip(1), (prev, cur) => cur + prev)),
                470
            );

            res.Messages.AssertEqual(
                OnNext(280, 7),
                OnNext(290, 5),
                OnNext(340, 9),
                OnNext(360, 13),
                OnNext(370, 11),
                OnNext(390, 13),
                OnNext(410, 20),
                OnNext(430, 15),
                OnNext(450, 11)
            );

            xs.Subscriptions.AssertEqual(
                Subscribe(200, 470)
            );
        }

        [Fact]
        public void PublishWithInitialValue_ArgumentChecking()
        {
            var someObservable = Observable.Empty<int>();

            ReactiveAssert.Throws<ArgumentNullException>(() => Observable.Publish(default, 1));
            ReactiveAssert.Throws<ArgumentNullException>(() => Observable.Publish(default, x => x, 1));
            ReactiveAssert.Throws<ArgumentNullException>(() => Observable.Publish(someObservable, default(Func<IObservable<int>, IObservable<int>>), 1));
        }

        [Fact]
        public void PublishWithInitialValue_SanityCheck()
        {
            var someObservable = Observable.Empty<int>();

            Observable.Publish(Observable.Range(1, 10), x => x, 0).AssertEqual(Observable.Range(0, 11));
        }

        [Fact]
        public void PublishWithInitialValue_Basic()
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

            scheduler.ScheduleAbsolute(Created, () => ys = xs.Publish(1979));
            scheduler.ScheduleAbsolute(Subscribed, () => subscription = ys.Subscribe(res));
            scheduler.ScheduleAbsolute(Disposed, () => subscription.Dispose());

            scheduler.ScheduleAbsolute(300, () => connection = ys.Connect());
            scheduler.ScheduleAbsolute(400, () => connection.Dispose());

            scheduler.ScheduleAbsolute(500, () => connection = ys.Connect());
            scheduler.ScheduleAbsolute(550, () => connection.Dispose());

            scheduler.ScheduleAbsolute(650, () => connection = ys.Connect());
            scheduler.ScheduleAbsolute(800, () => connection.Dispose());

            scheduler.Start();

            res.Messages.AssertEqual(
                OnNext(200, 1979),
                OnNext(340, 8),
                OnNext(360, 5),
                OnNext(370, 6),
                OnNext(390, 7),
                OnNext(520, 11)
            );

            xs.Subscriptions.AssertEqual(
                Subscribe(300, 400),
                Subscribe(500, 550),
                Subscribe(650, 800)
            );
        }

        [Fact]
        public void PublishWithInitialValue_Error()
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

            scheduler.ScheduleAbsolute(Created, () => ys = xs.Publish(1979));
            scheduler.ScheduleAbsolute(Subscribed, () => subscription = ys.Subscribe(res));
            scheduler.ScheduleAbsolute(Disposed, () => subscription.Dispose());

            scheduler.ScheduleAbsolute(300, () => connection = ys.Connect());
            scheduler.ScheduleAbsolute(400, () => connection.Dispose());

            scheduler.ScheduleAbsolute(500, () => connection = ys.Connect());
            scheduler.ScheduleAbsolute(800, () => connection.Dispose());

            scheduler.Start();

            res.Messages.AssertEqual(
                OnNext(200, 1979),
                OnNext(340, 8),
                OnNext(360, 5),
                OnNext(370, 6),
                OnNext(390, 7),
                OnNext(520, 11),
                OnNext(560, 20),
                OnError<int>(600, ex)
            );

            xs.Subscriptions.AssertEqual(
                Subscribe(300, 400),
                Subscribe(500, 600)
            );
        }

        [Fact]
        public void PublishWithInitialValue_Complete()
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

            scheduler.ScheduleAbsolute(Created, () => ys = xs.Publish(1979));
            scheduler.ScheduleAbsolute(Subscribed, () => subscription = ys.Subscribe(res));
            scheduler.ScheduleAbsolute(Disposed, () => subscription.Dispose());

            scheduler.ScheduleAbsolute(300, () => connection = ys.Connect());
            scheduler.ScheduleAbsolute(400, () => connection.Dispose());

            scheduler.ScheduleAbsolute(500, () => connection = ys.Connect());
            scheduler.ScheduleAbsolute(800, () => connection.Dispose());

            scheduler.Start();

            res.Messages.AssertEqual(
                OnNext(200, 1979),
                OnNext(340, 8),
                OnNext(360, 5),
                OnNext(370, 6),
                OnNext(390, 7),
                OnNext(520, 11),
                OnNext(560, 20),
                OnCompleted<int>(600)
            );

            xs.Subscriptions.AssertEqual(
                Subscribe(300, 400),
                Subscribe(500, 600)
            );
        }

        [Fact]
        public void PublishWithInitialValue_Dispose()
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

            scheduler.ScheduleAbsolute(Created, () => ys = xs.Publish(1979));
            scheduler.ScheduleAbsolute(Subscribed, () => subscription = ys.Subscribe(res));
            scheduler.ScheduleAbsolute(350, () => subscription.Dispose());

            scheduler.ScheduleAbsolute(300, () => connection = ys.Connect());
            scheduler.ScheduleAbsolute(400, () => connection.Dispose());

            scheduler.ScheduleAbsolute(500, () => connection = ys.Connect());
            scheduler.ScheduleAbsolute(550, () => connection.Dispose());

            scheduler.ScheduleAbsolute(650, () => connection = ys.Connect());
            scheduler.ScheduleAbsolute(800, () => connection.Dispose());

            scheduler.Start();

            res.Messages.AssertEqual(
                OnNext(200, 1979),
                OnNext(340, 8)
            );

            xs.Subscriptions.AssertEqual(
                Subscribe(300, 400),
                Subscribe(500, 550),
                Subscribe(650, 800)
            );
        }

        [Fact]
        public void PublishWithInitialValue_MultipleConnections()
        {
            var xs = Observable.Never<int>();
            var ys = xs.Publish(1979);

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
        public void PublishWithInitialValueLambda_Zip_Complete()
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
                xs.Publish(_xs => _xs.Zip(_xs.Skip(1), (prev, cur) => cur + prev), 1979)
            );

            res.Messages.AssertEqual(
                OnNext(220, 1982),
                OnNext(280, 7),
                OnNext(290, 5),
                OnNext(340, 9),
                OnNext(360, 13),
                OnNext(370, 11),
                OnNext(390, 13),
                OnNext(410, 20),
                OnNext(430, 15),
                OnNext(450, 11),
                OnNext(520, 20),
                OnNext(560, 31),
                OnCompleted<int>(600)
            );

            xs.Subscriptions.AssertEqual(
                Subscribe(200, 600)
            );
        }

        [Fact]
        public void PublishWithInitialValueLambda_Zip_Error()
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
                xs.Publish(_xs => _xs.Zip(_xs.Skip(1), (prev, cur) => cur + prev), 1979)
            );

            res.Messages.AssertEqual(
                OnNext(220, 1982),
                OnNext(280, 7),
                OnNext(290, 5),
                OnNext(340, 9),
                OnNext(360, 13),
                OnNext(370, 11),
                OnNext(390, 13),
                OnNext(410, 20),
                OnNext(430, 15),
                OnNext(450, 11),
                OnNext(520, 20),
                OnNext(560, 31),
                OnError<int>(600, ex)
            );

            xs.Subscriptions.AssertEqual(
                Subscribe(200, 600)
            );
        }

        [Fact]
        public void PublishWithInitialValueLambda_Zip_Dispose()
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
                xs.Publish(_xs => _xs.Zip(_xs.Skip(1), (prev, cur) => cur + prev), 1979),
                470
            );

            res.Messages.AssertEqual(
                OnNext(220, 1982),
                OnNext(280, 7),
                OnNext(290, 5),
                OnNext(340, 9),
                OnNext(360, 13),
                OnNext(370, 11),
                OnNext(390, 13),
                OnNext(410, 20),
                OnNext(430, 15),
                OnNext(450, 11)
            );

            xs.Subscriptions.AssertEqual(
                Subscribe(200, 470)
            );
        }

    }
}

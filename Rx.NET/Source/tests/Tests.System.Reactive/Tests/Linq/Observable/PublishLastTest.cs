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
    public class PublishLastTest : ReactiveTest
    {

        [Fact]
        public void PublishLast_ArgumentChecking()
        {
            var someObservable = Observable.Empty<int>();
            var scheduler = new TestScheduler();

            ReactiveAssert.Throws<ArgumentNullException>(() => Observable.PublishLast(default(IObservable<int>)));
            ReactiveAssert.Throws<ArgumentNullException>(() => Observable.PublishLast(default(IObservable<int>), x => x));
            ReactiveAssert.Throws<ArgumentNullException>(() => Observable.PublishLast<int, int>(someObservable, null));
        }

        [Fact]
        public void PublishLast_Basic()
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

            scheduler.ScheduleAbsolute(Created, () => ys = xs.PublishLast());
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
            );

            xs.Subscriptions.AssertEqual(
                Subscribe(300, 400),
                Subscribe(500, 550),
                Subscribe(650, 800)
            );
        }

        [Fact]
        public void PublishLast_Error()
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

            scheduler.ScheduleAbsolute(Created, () => ys = xs.PublishLast());
            scheduler.ScheduleAbsolute(Subscribed, () => subscription = ys.Subscribe(res));
            scheduler.ScheduleAbsolute(Disposed, () => subscription.Dispose());

            scheduler.ScheduleAbsolute(300, () => connection = ys.Connect());
            scheduler.ScheduleAbsolute(400, () => connection.Dispose());

            scheduler.ScheduleAbsolute(500, () => connection = ys.Connect());
            scheduler.ScheduleAbsolute(800, () => connection.Dispose());

            scheduler.Start();

            res.Messages.AssertEqual(
                OnError<int>(600, ex)
            );

            xs.Subscriptions.AssertEqual(
                Subscribe(300, 400),
                Subscribe(500, 600)
            );
        }

        [Fact]
        public void PublishLast_Complete()
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

            scheduler.ScheduleAbsolute(Created, () => ys = xs.PublishLast());
            scheduler.ScheduleAbsolute(Subscribed, () => subscription = ys.Subscribe(res));
            scheduler.ScheduleAbsolute(Disposed, () => subscription.Dispose());

            scheduler.ScheduleAbsolute(300, () => connection = ys.Connect());
            scheduler.ScheduleAbsolute(400, () => connection.Dispose());

            scheduler.ScheduleAbsolute(500, () => connection = ys.Connect());
            scheduler.ScheduleAbsolute(800, () => connection.Dispose());

            scheduler.Start();

            res.Messages.AssertEqual(
                OnNext(600, 20),
                OnCompleted<int>(600)
            );

            xs.Subscriptions.AssertEqual(
                Subscribe(300, 400),
                Subscribe(500, 600)
            );
        }

        [Fact]
        public void PublishLast_Dispose()
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

            scheduler.ScheduleAbsolute(Created, () => ys = xs.PublishLast());
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
            );

            xs.Subscriptions.AssertEqual(
                Subscribe(300, 400),
                Subscribe(500, 550),
                Subscribe(650, 800)
            );
        }

        [Fact]
        public void PublishLast_MultipleConnections()
        {
            var xs = Observable.Never<int>();
            var ys = xs.PublishLast();

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
        public void PublishLastLambda_Zip_Complete()
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
                xs.PublishLast(_xs => _xs.Zip(_xs, (x, y) => x + y))
            );

            res.Messages.AssertEqual(
                OnNext(600, 40),
                OnCompleted<int>(600)
            );

            xs.Subscriptions.AssertEqual(
                Subscribe(200, 600)
            );
        }

        [Fact]
        public void PublishLastLambda_Zip_Error()
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
                xs.PublishLast(_xs => _xs.Zip(_xs, (x, y) => x + y))
            );

            res.Messages.AssertEqual(
                OnError<int>(600, ex)
            );

            xs.Subscriptions.AssertEqual(
                Subscribe(200, 600)
            );
        }

        [Fact]
        public void PublishLastLambda_Zip_Dispose()
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
                xs.PublishLast(_xs => _xs.Zip(_xs, (x, y) => x + y)),
                470
            );

            res.Messages.AssertEqual(
            );

            xs.Subscriptions.AssertEqual(
                Subscribe(200, 470)
            );
        }

    }
}

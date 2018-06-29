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
    public class MulticastTest : ReactiveTest
    {

        [Fact]
        public void Multicast_ArgumentChecking()
        {
            ReactiveAssert.Throws<ArgumentNullException>(() => Observable.Multicast(null, new Subject<int>()));
            ReactiveAssert.Throws<ArgumentNullException>(() => Observable.Multicast<int, int>(DummyObservable<int>.Instance, null));
            ReactiveAssert.Throws<ArgumentNullException>(() => Observable.Multicast(null, () => new Subject<int>(), xs => xs));
            ReactiveAssert.Throws<ArgumentNullException>(() => Observable.Multicast<int, int, int>(DummyObservable<int>.Instance, null, xs => xs));
            ReactiveAssert.Throws<ArgumentNullException>(() => Observable.Multicast<int, int, int>(DummyObservable<int>.Instance, () => new Subject<int>(), null));
        }

        [Fact]
        public void Multicast_Hot_1()
        {
            var scheduler = new TestScheduler();

            var s = new Subject<int>();

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

            var c = default(IConnectableObservable<int>);
            var o = scheduler.CreateObserver<int>();
            var d1 = default(IDisposable);
            var d2 = default(IDisposable);

            scheduler.ScheduleAbsolute(50, () => c = xs.Multicast(s));
            scheduler.ScheduleAbsolute(100, () => d1 = c.Subscribe(o));
            scheduler.ScheduleAbsolute(200, () => d2 = c.Connect());
            scheduler.ScheduleAbsolute(300, () => d1.Dispose());

            scheduler.Start();

            o.Messages.AssertEqual(
                OnNext(210, 3),
                OnNext(240, 4),
                OnNext(270, 5)
            );

            xs.Subscriptions.AssertEqual(
                Subscribe(200, 390)
            );
        }

        [Fact]
        public void Multicast_Hot_2()
        {
            var scheduler = new TestScheduler();

            var s = new Subject<int>();

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

            var c = default(IConnectableObservable<int>);
            var o = scheduler.CreateObserver<int>();
            var d1 = default(IDisposable);
            var d2 = default(IDisposable);

            scheduler.ScheduleAbsolute(50, () => c = xs.Multicast(s));
            scheduler.ScheduleAbsolute(100, () => d2 = c.Connect());
            scheduler.ScheduleAbsolute(200, () => d1 = c.Subscribe(o));
            scheduler.ScheduleAbsolute(300, () => d1.Dispose());

            scheduler.Start();

            o.Messages.AssertEqual(
                OnNext(210, 3),
                OnNext(240, 4),
                OnNext(270, 5)
            );

            xs.Subscriptions.AssertEqual(
                Subscribe(100, 390)
            );
        }

        [Fact]
        public void Multicast_Hot_3()
        {
            var scheduler = new TestScheduler();

            var s = new Subject<int>();

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

            var c = default(IConnectableObservable<int>);
            var o = scheduler.CreateObserver<int>();
            var d1 = default(IDisposable);
            var d2 = default(IDisposable);

            scheduler.ScheduleAbsolute(50, () => c = xs.Multicast(s));
            scheduler.ScheduleAbsolute(100, () => d2 = c.Connect());
            scheduler.ScheduleAbsolute(200, () => d1 = c.Subscribe(o));
            scheduler.ScheduleAbsolute(300, () => d2.Dispose());
            scheduler.ScheduleAbsolute(335, () => d2 = c.Connect());

            scheduler.Start();

            o.Messages.AssertEqual(
                OnNext(210, 3),
                OnNext(240, 4),
                OnNext(270, 5),
                OnNext(340, 7),
                OnCompleted<int>(390)
            );

            xs.Subscriptions.AssertEqual(
                Subscribe(100, 300),
                Subscribe(335, 390)
            );
        }

        [Fact]
        public void Multicast_Hot_4()
        {
            var scheduler = new TestScheduler();

            var s = new Subject<int>();

            var ex = new Exception();

            var xs = scheduler.CreateHotObservable(
                OnNext(40, 0),
                OnNext(90, 1),
                OnNext(150, 2),
                OnNext(210, 3),
                OnNext(240, 4),
                OnNext(270, 5),
                OnNext(330, 6),
                OnNext(340, 7),
                OnError<int>(390, ex)
            );

            var c = default(IConnectableObservable<int>);
            var o = scheduler.CreateObserver<int>();
            var d1 = default(IDisposable);
            var d2 = default(IDisposable);

            scheduler.ScheduleAbsolute(50, () => c = xs.Multicast(s));
            scheduler.ScheduleAbsolute(100, () => d2 = c.Connect());
            scheduler.ScheduleAbsolute(200, () => d1 = c.Subscribe(o));
            scheduler.ScheduleAbsolute(300, () => d2.Dispose());
            scheduler.ScheduleAbsolute(335, () => d2 = c.Connect());

            scheduler.Start();

            o.Messages.AssertEqual(
                OnNext(210, 3),
                OnNext(240, 4),
                OnNext(270, 5),
                OnNext(340, 7),
                OnError<int>(390, ex)
            );

            xs.Subscriptions.AssertEqual(
                Subscribe(100, 300),
                Subscribe(335, 390)
            );
        }

        [Fact]
        public void Multicast_Hot_5()
        {
            var scheduler = new TestScheduler();

            var s = new Subject<int>();

            var ex = new Exception();

            var xs = scheduler.CreateHotObservable(
                OnNext(40, 0),
                OnNext(90, 1),
                OnNext(150, 2),
                OnNext(210, 3),
                OnNext(240, 4),
                OnNext(270, 5),
                OnNext(330, 6),
                OnNext(340, 7),
                OnError<int>(390, ex)
            );

            var c = default(IConnectableObservable<int>);
            var o = scheduler.CreateObserver<int>();
            var d1 = default(IDisposable);
            var d2 = default(IDisposable);

            scheduler.ScheduleAbsolute(50, () => c = xs.Multicast(s));
            scheduler.ScheduleAbsolute(100, () => d2 = c.Connect());
            scheduler.ScheduleAbsolute(400, () => d1 = c.Subscribe(o));

            scheduler.Start();

            o.Messages.AssertEqual(
                OnError<int>(400, ex)
            );

            xs.Subscriptions.AssertEqual(
                Subscribe(100, 390)
            );
        }

        [Fact]
        public void Multicast_Hot_6()
        {
            var scheduler = new TestScheduler();

            var s = new Subject<int>();

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

            var c = default(IConnectableObservable<int>);
            var o = scheduler.CreateObserver<int>();
            var d1 = default(IDisposable);
            var d2 = default(IDisposable);

            scheduler.ScheduleAbsolute(50, () => c = xs.Multicast(s));
            scheduler.ScheduleAbsolute(100, () => d2 = c.Connect());
            scheduler.ScheduleAbsolute(400, () => d1 = c.Subscribe(o));

            scheduler.Start();

            o.Messages.AssertEqual(
                OnCompleted<int>(400)
            );

            xs.Subscriptions.AssertEqual(
                Subscribe(100, 390)
            );
        }

        [Fact]
        public void Multicast_Cold_Completed()
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
                xs.Multicast(() => new Subject<int>(), ys => ys)
            );

            res.Messages.AssertEqual(
                OnNext(210, 3),
                OnNext(240, 4),
                OnNext(270, 5),
                OnNext(330, 6),
                OnNext(340, 7),
                OnCompleted<int>(390)
            );

            xs.Subscriptions.AssertEqual(
                Subscribe(200, 390)
            );
        }

        [Fact]
        public void Multicast_Cold_Error()
        {
            var scheduler = new TestScheduler();

            var ex = new Exception();

            var xs = scheduler.CreateHotObservable(
                OnNext(40, 0),
                OnNext(90, 1),
                OnNext(150, 2),
                OnNext(210, 3),
                OnNext(240, 4),
                OnNext(270, 5),
                OnNext(330, 6),
                OnNext(340, 7),
                OnError<int>(390, ex)
            );

            var res = scheduler.Start(() =>
                xs.Multicast(() => new Subject<int>(), ys => ys)
            );

            res.Messages.AssertEqual(
                OnNext(210, 3),
                OnNext(240, 4),
                OnNext(270, 5),
                OnNext(330, 6),
                OnNext(340, 7),
                OnError<int>(390, ex)
            );

            xs.Subscriptions.AssertEqual(
                Subscribe(200, 390)
            );
        }

        [Fact]
        public void Multicast_Cold_Dispose()
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
                OnNext(340, 7)
            );

            var res = scheduler.Start(() =>
                xs.Multicast(() => new Subject<int>(), ys => ys)
            );

            res.Messages.AssertEqual(
                OnNext(210, 3),
                OnNext(240, 4),
                OnNext(270, 5),
                OnNext(330, 6),
                OnNext(340, 7)
            );

            xs.Subscriptions.AssertEqual(
                Subscribe(200, 1000)
            );
        }

        [Fact]
        public void Multicast_Cold_Zip()
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
                xs.Multicast(() => new Subject<int>(), ys => ys.Zip(ys, (a, b) => a + b))
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
        public void Multicast_SubjectSelectorThrows()
        {
            var ex = new Exception();

            var scheduler = new TestScheduler();

            var xs = scheduler.CreateHotObservable(
                OnNext(210, 1),
                OnNext(240, 2),
                OnCompleted<int>(300)
            );

            var res = scheduler.Start(() =>
                xs.Multicast<int, int, int>(() => { throw ex; }, _ => _)
            );

            res.Messages.AssertEqual(
                OnError<int>(200, ex)
            );

            xs.Subscriptions.AssertEqual(
            );
        }

        [Fact]
        public void Multicast_SelectorThrows()
        {
            var ex = new Exception();

            var scheduler = new TestScheduler();

            var xs = scheduler.CreateHotObservable(
                OnNext(210, 1),
                OnNext(240, 2),
                OnCompleted<int>(300)
            );

            var res = scheduler.Start(() =>
                xs.Multicast<int, int, int>(() => new Subject<int>(), _ => { throw ex; })
            );

            res.Messages.AssertEqual(
                OnError<int>(200, ex)
            );

            xs.Subscriptions.AssertEqual(
            );
        }

    }
}

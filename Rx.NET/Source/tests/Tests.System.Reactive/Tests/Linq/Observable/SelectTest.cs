// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the Apache 2.0 License.
// See the LICENSE file in the project root for more information. 

using System;
using System.Linq;
using System.Reactive.Concurrency;
using System.Reactive.Disposables;
using System.Reactive.Linq;
using Microsoft.Reactive.Testing;
using ReactiveTests.Dummies;
using Xunit;

namespace ReactiveTests.Tests
{
    public class SelectTest : ReactiveTest
    {

        [Fact]
        public void Select_ArgumentChecking()
        {
            ReactiveAssert.Throws<ArgumentNullException>(() => ((IObservable<int>)null).Select(DummyFunc<int, int>.Instance));
            ReactiveAssert.Throws<ArgumentNullException>(() => DummyObservable<int>.Instance.Select((Func<int, int>)null));
            ReactiveAssert.Throws<ArgumentNullException>(() => DummyObservable<int>.Instance.Select(DummyFunc<int, int>.Instance).Subscribe(null));
        }

        [Fact]
        public void Select_Throws()
        {
            ReactiveAssert.Throws<InvalidOperationException>(() =>
                Observable.Return(1).Select(x => x).Subscribe(
                 x =>
                 {
                     throw new InvalidOperationException();
                 }));
            ReactiveAssert.Throws<InvalidOperationException>(() =>
                Observable.Throw<int>(new Exception()).Select(x => x).Subscribe(
                 x => { },
                 exception =>
                 {
                     throw new InvalidOperationException();
                 }));
            ReactiveAssert.Throws<InvalidOperationException>(() =>
                 Observable.Empty<int>().Select(x => x).Subscribe(
                 x => { },
                 exception => { },
                 () =>
                 {
                     throw new InvalidOperationException();
                 }));
            ReactiveAssert.Throws<InvalidOperationException>(() => Observable.Create(new Func<IObserver<int>, Action>(o => { throw new InvalidOperationException(); })).Select(x => x).Subscribe());
        }

        [Fact]
        public void Select_DisposeInsideSelector()
        {
            var scheduler = new TestScheduler();

            var xs = scheduler.CreateHotObservable(
                OnNext(100, 1),
                OnNext(200, 2),
                OnNext(500, 3),
                OnNext(600, 4)
            );

            var invoked = 0;

            var res = scheduler.CreateObserver<int>();

            var d = new SerialDisposable();
            d.Disposable = xs.Select(x =>
            {
                invoked++;
                if (scheduler.Clock > 400)
                {
                    d.Dispose();
                }

                return x;
            }).Subscribe(res);

            scheduler.ScheduleAbsolute(Disposed, d.Dispose);

            scheduler.Start();

            res.Messages.AssertEqual(
                OnNext(100, 1),
                OnNext(200, 2)
            );

            xs.Subscriptions.AssertEqual(
                Subscribe(0, 500)
            );

            Assert.Equal(3, invoked);
        }

        [Fact]
        public void Select_Completed()
        {
            var scheduler = new TestScheduler();

            var invoked = 0;

            var xs = scheduler.CreateHotObservable(
                OnNext(180, 1),
                OnNext(210, 2),
                OnNext(240, 3),
                OnNext(290, 4),
                OnNext(350, 5),
                OnCompleted<int>(400),
                OnNext(410, -1),
                OnCompleted<int>(420),
                OnError<int>(430, new Exception())
            );

            var res = scheduler.Start(() =>
                xs.Select(x =>
                {
                    invoked++;
                    return x + 1;
                })
            );

            res.Messages.AssertEqual(
                OnNext(210, 3),
                OnNext(240, 4),
                OnNext(290, 5),
                OnNext(350, 6),
                OnCompleted<int>(400)
            );

            xs.Subscriptions.AssertEqual(
                Subscribe(200, 400)
            );

            Assert.Equal(4, invoked);
        }

        [Fact]
        public void Select_NotCompleted()
        {
            var scheduler = new TestScheduler();

            var invoked = 0;

            var xs = scheduler.CreateHotObservable(
                OnNext(180, 1),
                OnNext(210, 2),
                OnNext(240, 3),
                OnNext(290, 4),
                OnNext(350, 5)
            );

            var res = scheduler.Start(() =>
                xs.Select(x =>
                {
                    invoked++;
                    return x + 1;
                })
            );

            res.Messages.AssertEqual(
                OnNext(210, 3),
                OnNext(240, 4),
                OnNext(290, 5),
                OnNext(350, 6)
            );

            xs.Subscriptions.AssertEqual(
                Subscribe(200, 1000)
            );

            Assert.Equal(4, invoked);
        }

        [Fact]
        public void Select_Error()
        {
            var scheduler = new TestScheduler();

            var invoked = 0;

            var ex = new Exception();

            var xs = scheduler.CreateHotObservable(
                OnNext(180, 1),
                OnNext(210, 2),
                OnNext(240, 3),
                OnNext(290, 4),
                OnNext(350, 5),
                OnError<int>(400, ex),
                OnNext(410, -1),
                OnCompleted<int>(420),
                OnError<int>(430, new Exception())
            );

            var res = scheduler.Start(() =>
                xs.Select(x =>
                {
                    invoked++;
                    return x + 1;
                })
            );

            res.Messages.AssertEqual(
                OnNext(210, 3),
                OnNext(240, 4),
                OnNext(290, 5),
                OnNext(350, 6),
                OnError<int>(400, ex)
            );

            xs.Subscriptions.AssertEqual(
                Subscribe(200, 400)
            );

            Assert.Equal(4, invoked);
        }

        [Fact]
        public void Select_SelectorThrows()
        {
            var scheduler = new TestScheduler();

            var invoked = 0;

            var xs = scheduler.CreateHotObservable(
                OnNext(180, 1),
                OnNext(210, 2),
                OnNext(240, 3),
                OnNext(290, 4),
                OnNext(350, 5),
                OnCompleted<int>(400),
                OnNext(410, -1),
                OnCompleted<int>(420),
                OnError<int>(430, new Exception())
            );

            var ex = new Exception();

            var res = scheduler.Start(() =>
                xs.Select(x =>
                {
                    invoked++;
                    if (invoked == 3)
                    {
                        throw ex;
                    }

                    return x + 1;
                })
            );

            res.Messages.AssertEqual(
                OnNext(210, 3),
                OnNext(240, 4),
                OnError<int>(290, ex)
            );

            xs.Subscriptions.AssertEqual(
                Subscribe(200, 290)
            );

            Assert.Equal(3, invoked);
        }

        [Fact]
        public void SelectWithIndex_ArgumentChecking()
        {
            ReactiveAssert.Throws<ArgumentNullException>(() => ((IObservable<int>)null).Select(DummyFunc<int, int, int>.Instance));
            ReactiveAssert.Throws<ArgumentNullException>(() => DummyObservable<int>.Instance.Select((Func<int, int, int>)null));
            ReactiveAssert.Throws<ArgumentNullException>(() => DummyObservable<int>.Instance.Select(DummyFunc<int, int, int>.Instance).Subscribe(null));
        }

        [Fact]
        public void SelectWithIndex_Throws()
        {
            ReactiveAssert.Throws<InvalidOperationException>(() =>
                Observable.Return(1).Select((x, index) => x).Subscribe(
                 x =>
                 {
                     throw new InvalidOperationException();
                 }));
            ReactiveAssert.Throws<InvalidOperationException>(() =>
                Observable.Throw<int>(new Exception()).Select((x, index) => x).Subscribe(
                 x => { },
                 exception =>
                 {
                     throw new InvalidOperationException();
                 }));
            ReactiveAssert.Throws<InvalidOperationException>(() =>
                 Observable.Empty<int>().Select((x, index) => x).Subscribe(
                 x => { },
                 exception => { },
                 () =>
                 {
                     throw new InvalidOperationException();
                 }));
            ReactiveAssert.Throws<InvalidOperationException>(() => Observable.Create(new Func<IObserver<int>, Action>(o => { throw new InvalidOperationException(); })).Select((x, index) => x).Subscribe());
        }

        [Fact]
        public void SelectWithIndex_DisposeInsideSelector()
        {
            var scheduler = new TestScheduler();

            var xs = scheduler.CreateHotObservable(
                OnNext(100, 4),
                OnNext(200, 3),
                OnNext(500, 2),
                OnNext(600, 1)
            );

            var invoked = 0;

            var res = scheduler.CreateObserver<int>();

            var d = new SerialDisposable();
            d.Disposable = xs.Select((x, index) =>
            {
                invoked++;
                if (scheduler.Clock > 400)
                {
                    d.Dispose();
                }

                return x + index * 10;
            }).Subscribe(res);

            scheduler.ScheduleAbsolute(Disposed, d.Dispose);

            scheduler.Start();

            res.Messages.AssertEqual(
                OnNext(100, 4),
                OnNext(200, 13)
            );

            xs.Subscriptions.AssertEqual(
                Subscribe(0, 500)
            );

            Assert.Equal(3, invoked);
        }

        [Fact]
        public void SelectWithIndex_Completed()
        {
            var scheduler = new TestScheduler();

            var invoked = 0;

            var xs = scheduler.CreateHotObservable(
                OnNext(180, 5),
                OnNext(210, 4),
                OnNext(240, 3),
                OnNext(290, 2),
                OnNext(350, 1),
                OnCompleted<int>(400),
                OnNext(410, -1),
                OnCompleted<int>(420),
                OnError<int>(430, new Exception())
            );

            var res = scheduler.Start(() =>
                xs.Select((x, index) =>
                {
                    invoked++;
                    return (x + 1) + (index * 10);
                })
            );

            res.Messages.AssertEqual(
                OnNext(210, 5),
                OnNext(240, 14),
                OnNext(290, 23),
                OnNext(350, 32),
                OnCompleted<int>(400)
            );

            xs.Subscriptions.AssertEqual(
                Subscribe(200, 400)
            );

            Assert.Equal(4, invoked);
        }

        [Fact]
        public void SelectWithIndex_NotCompleted()
        {
            var scheduler = new TestScheduler();

            var invoked = 0;

            var xs = scheduler.CreateHotObservable(
                OnNext(180, 5),
                OnNext(210, 4),
                OnNext(240, 3),
                OnNext(290, 2),
                OnNext(350, 1)
            );

            var res = scheduler.Start(() =>
                xs.Select((x, index) =>
                {
                    invoked++;
                    return (x + 1) + (index * 10);
                })
            );

            res.Messages.AssertEqual(
                OnNext(210, 5),
                OnNext(240, 14),
                OnNext(290, 23),
                OnNext(350, 32)
            );

            xs.Subscriptions.AssertEqual(
                Subscribe(200, 1000)
            );

            Assert.Equal(4, invoked);
        }

        [Fact]
        public void SelectWithIndex_Error()
        {
            var scheduler = new TestScheduler();

            var invoked = 0;
            var ex = new Exception();

            var xs = scheduler.CreateHotObservable(
                OnNext(180, 5),
                OnNext(210, 4),
                OnNext(240, 3),
                OnNext(290, 2),
                OnNext(350, 1),
                OnError<int>(400, ex),
                OnNext(410, -1),
                OnCompleted<int>(420),
                OnError<int>(430, new Exception())
            );

            var res = scheduler.Start(() =>
                xs.Select((x, index) =>
                {
                    invoked++;
                    return (x + 1) + (index * 10);
                })
            );

            res.Messages.AssertEqual(
                OnNext(210, 5),
                OnNext(240, 14),
                OnNext(290, 23),
                OnNext(350, 32),
                OnError<int>(400, ex)
            );

            xs.Subscriptions.AssertEqual(
                Subscribe(200, 400)
            );

            Assert.Equal(4, invoked);
        }

        [Fact]
        public void SelectWithIndex_SelectorThrows()
        {
            var scheduler = new TestScheduler();

            var invoked = 0;

            var xs = scheduler.CreateHotObservable(
                OnNext(180, 5),
                OnNext(210, 4),
                OnNext(240, 3),
                OnNext(290, 2),
                OnNext(350, 1),
                OnCompleted<int>(400),
                OnNext(410, -1),
                OnCompleted<int>(420),
                OnError<int>(430, new Exception())
            );

            var ex = new Exception();

            var res = scheduler.Start(() =>
                xs.Select((x, index) =>
                {
                    invoked++;
                    if (invoked == 3)
                    {
                        throw ex;
                    }

                    return (x + 1) + (index * 10);
                })
            );

            res.Messages.AssertEqual(
                OnNext(210, 5),
                OnNext(240, 14),
                OnError<int>(290, ex)
            );

            xs.Subscriptions.AssertEqual(
                Subscribe(200, 290)
            );

            Assert.Equal(3, invoked);
        }

        [Fact]
        public void Select_Select1()
        {
            var scheduler = new TestScheduler();

            var xs = scheduler.CreateHotObservable(
                OnNext(180, 5),
                OnNext(210, 4),
                OnNext(240, 3),
                OnNext(290, 2),
                OnNext(350, 1),
                OnCompleted<int>(400)
            );

            var res = scheduler.Start(() =>
                xs.Select(x => x + 1).Select(x => x - 2)
            );

            res.Messages.AssertEqual(
                OnNext(210, 4 + 1 - 2),
                OnNext(240, 3 + 1 - 2),
                OnNext(290, 2 + 1 - 2),
                OnNext(350, 1 + 1 - 2),
                OnCompleted<int>(400)
            );

            xs.Subscriptions.AssertEqual(
                Subscribe(200, 400)
            );
        }

        [Fact]
        public void Select_Select2()
        {
            var scheduler = new TestScheduler();

            var xs = scheduler.CreateHotObservable(
                OnNext(180, 5),
                OnNext(210, 4),
                OnNext(240, 3),
                OnNext(290, 2),
                OnNext(350, 1),
                OnCompleted<int>(400)
            );

            var res = scheduler.Start(() =>
                xs.Select((x, i) => x + i).Select(x => x - 2)
            );

            res.Messages.AssertEqual(
                OnNext(210, 4 + 0 - 2),
                OnNext(240, 3 + 1 - 2),
                OnNext(290, 2 + 2 - 2),
                OnNext(350, 1 + 3 - 2),
                OnCompleted<int>(400)
            );

            xs.Subscriptions.AssertEqual(
                Subscribe(200, 400)
            );
        }

        [Fact]
        public void Select_Select3()
        {
            var scheduler = new TestScheduler();

            var xs = scheduler.CreateHotObservable(
                OnNext(180, 5),
                OnNext(210, 4),
                OnNext(240, 3),
                OnNext(290, 2),
                OnNext(350, 1),
                OnCompleted<int>(400)
            );

            var res = scheduler.Start(() =>
                xs.Select(x => x + 1).Select((x, i) => x - i)
            );

            res.Messages.AssertEqual(
                OnNext(210, 4 + 1 - 0),
                OnNext(240, 3 + 1 - 1),
                OnNext(290, 2 + 1 - 2),
                OnNext(350, 1 + 1 - 3),
                OnCompleted<int>(400)
            );

            xs.Subscriptions.AssertEqual(
                Subscribe(200, 400)
            );
        }

        [Fact]
        public void Select_Select4()
        {
            var scheduler = new TestScheduler();

            var xs = scheduler.CreateHotObservable(
                OnNext(180, 5),
                OnNext(210, 4),
                OnNext(240, 3),
                OnNext(290, 2),
                OnNext(350, 1),
                OnCompleted<int>(400)
            );

            var res = scheduler.Start(() =>
                xs.Select((x, i) => x + i).Select((x, i) => x - i)
            );

            res.Messages.AssertEqual(
                OnNext(210, 4),
                OnNext(240, 3),
                OnNext(290, 2),
                OnNext(350, 1),
                OnCompleted<int>(400)
            );

            xs.Subscriptions.AssertEqual(
                Subscribe(200, 400)
            );
        }

    }
}

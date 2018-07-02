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
    public class WhereTest : ReactiveTest
    {

        [Fact]
        public void Where_ArgumentChecking()
        {
            ReactiveAssert.Throws<ArgumentNullException>(() => ((IObservable<int>)null).Where(DummyFunc<int, bool>.Instance));
            ReactiveAssert.Throws<ArgumentNullException>(() => DummyObservable<int>.Instance.Where((Func<int, bool>)null));
            ReactiveAssert.Throws<ArgumentNullException>(() => DummyObservable<int>.Instance.Where(DummyFunc<int, bool>.Instance).Subscribe(null));
        }

        private static bool IsPrime(int i)
        {
            if (i <= 1)
            {
                return false;
            }

            var max = (int)Math.Sqrt(i);
            for (var j = 2; j <= max; ++j)
            {
                if (i % j == 0)
                {
                    return false;
                }
            }

            return true;
        }

        [Fact]
        public void Where_Complete()
        {
            var scheduler = new TestScheduler();

            var invoked = 0;

            var xs = scheduler.CreateHotObservable(
                OnNext(110, 1),
                OnNext(180, 2),
                OnNext(230, 3),
                OnNext(270, 4),
                OnNext(340, 5),
                OnNext(380, 6),
                OnNext(390, 7),
                OnNext(450, 8),
                OnNext(470, 9),
                OnNext(560, 10),
                OnNext(580, 11),
                OnCompleted<int>(600),
                OnNext(610, 12),
                OnError<int>(620, new Exception()),
                OnCompleted<int>(630)
            );

            var res = scheduler.Start(() =>
                xs.Where(x =>
                {
                    invoked++;
                    return IsPrime(x);
                })
            );

            res.Messages.AssertEqual(
                OnNext(230, 3),
                OnNext(340, 5),
                OnNext(390, 7),
                OnNext(580, 11),
                OnCompleted<int>(600)
            );

            xs.Subscriptions.AssertEqual(
                Subscribe(200, 600)
            );

            Assert.Equal(9, invoked);
        }

        [Fact]
        public void Where_True()
        {
            var scheduler = new TestScheduler();

            var invoked = 0;

            var xs = scheduler.CreateHotObservable(
                OnNext(110, 1),
                OnNext(180, 2),
                OnNext(230, 3),
                OnNext(270, 4),
                OnNext(340, 5),
                OnNext(380, 6),
                OnNext(390, 7),
                OnNext(450, 8),
                OnNext(470, 9),
                OnNext(560, 10),
                OnNext(580, 11),
                OnCompleted<int>(600)
            );

            var res = scheduler.Start(() =>
                xs.Where(x =>
                {
                    invoked++;
                    return true;
                })
            );

            res.Messages.AssertEqual(
                OnNext(230, 3),
                OnNext(270, 4),
                OnNext(340, 5),
                OnNext(380, 6),
                OnNext(390, 7),
                OnNext(450, 8),
                OnNext(470, 9),
                OnNext(560, 10),
                OnNext(580, 11),
                OnCompleted<int>(600)
            );

            xs.Subscriptions.AssertEqual(
                Subscribe(200, 600)
            );

            Assert.Equal(9, invoked);
        }

        [Fact]
        public void Where_False()
        {
            var scheduler = new TestScheduler();

            var invoked = 0;

            var xs = scheduler.CreateHotObservable(
                OnNext(110, 1),
                OnNext(180, 2),
                OnNext(230, 3),
                OnNext(270, 4),
                OnNext(340, 5),
                OnNext(380, 6),
                OnNext(390, 7),
                OnNext(450, 8),
                OnNext(470, 9),
                OnNext(560, 10),
                OnNext(580, 11),
                OnCompleted<int>(600)
            );

            var res = scheduler.Start(() =>
                xs.Where(x =>
                {
                    invoked++;
                    return false;
                })
            );

            res.Messages.AssertEqual(
                OnCompleted<int>(600)
            );

            xs.Subscriptions.AssertEqual(
                Subscribe(200, 600)
            );

            Assert.Equal(9, invoked);
        }

        [Fact]
        public void Where_Dispose()
        {
            var scheduler = new TestScheduler();

            var invoked = 0;

            var xs = scheduler.CreateHotObservable(
                OnNext(110, 1),
                OnNext(180, 2),
                OnNext(230, 3),
                OnNext(270, 4),
                OnNext(340, 5),
                OnNext(380, 6),
                OnNext(390, 7),
                OnNext(450, 8),
                OnNext(470, 9),
                OnNext(560, 10),
                OnNext(580, 11),
                OnCompleted<int>(600)
            );

            var res = scheduler.Start(() =>
                xs.Where(x =>
                {
                    invoked++;
                    return IsPrime(x);
                }),
                400
            );

            res.Messages.AssertEqual(
                OnNext(230, 3),
                OnNext(340, 5),
                OnNext(390, 7)
            );

            xs.Subscriptions.AssertEqual(
                Subscribe(200, 400)
            );

            Assert.Equal(5, invoked);
        }

        [Fact]
        public void Where_Error()
        {
            var scheduler = new TestScheduler();

            var invoked = 0;

            var ex = new Exception();

            var xs = scheduler.CreateHotObservable(
                OnNext(110, 1),
                OnNext(180, 2),
                OnNext(230, 3),
                OnNext(270, 4),
                OnNext(340, 5),
                OnNext(380, 6),
                OnNext(390, 7),
                OnNext(450, 8),
                OnNext(470, 9),
                OnNext(560, 10),
                OnNext(580, 11),
                OnError<int>(600, ex),
                OnNext(610, 12),
                OnError<int>(620, new Exception()),
                OnCompleted<int>(630)
            );

            var res = scheduler.Start(() =>
                xs.Where(x =>
                {
                    invoked++;
                    return IsPrime(x);
                })
            );

            res.Messages.AssertEqual(
                OnNext(230, 3),
                OnNext(340, 5),
                OnNext(390, 7),
                OnNext(580, 11),
                OnError<int>(600, ex)
            );

            xs.Subscriptions.AssertEqual(
                Subscribe(200, 600)
            );

            Assert.Equal(9, invoked);
        }

        [Fact]
        public void Where_Throw()
        {
            var scheduler = new TestScheduler();

            var invoked = 0;
            var ex = new Exception();

            var xs = scheduler.CreateHotObservable(
                OnNext(110, 1),
                OnNext(180, 2),
                OnNext(230, 3),
                OnNext(270, 4),
                OnNext(340, 5),
                OnNext(380, 6),
                OnNext(390, 7),
                OnNext(450, 8),
                OnNext(470, 9),
                OnNext(560, 10),
                OnNext(580, 11),
                OnCompleted<int>(600),
                OnNext(610, 12),
                OnError<int>(620, new Exception()),
                OnCompleted<int>(630)
            );

            var res = scheduler.Start(() =>
                xs.Where(x =>
                {
                    invoked++;
                    if (x > 5)
                    {
                        throw ex;
                    }

                    return IsPrime(x);
                })
            );

            res.Messages.AssertEqual(
                OnNext(230, 3),
                OnNext(340, 5),
                OnError<int>(380, ex)
            );

            xs.Subscriptions.AssertEqual(
                Subscribe(200, 380)
            );

            Assert.Equal(4, invoked);
        }

        [Fact]
        public void Where_DisposeInPredicate()
        {
            var scheduler = new TestScheduler();

            var invoked = 0;

            var xs = scheduler.CreateHotObservable(
                OnNext(110, 1),
                OnNext(180, 2),
                OnNext(230, 3),
                OnNext(270, 4),
                OnNext(340, 5),
                OnNext(380, 6),
                OnNext(390, 7),
                OnNext(450, 8),
                OnNext(470, 9),
                OnNext(560, 10),
                OnNext(580, 11),
                OnCompleted<int>(600),
                OnNext(610, 12),
                OnError<int>(620, new Exception()),
                OnCompleted<int>(630)
            );

            var res = scheduler.CreateObserver<int>();

            var d = new SerialDisposable();
            var ys = default(IObservable<int>);

            scheduler.ScheduleAbsolute(Created, () => ys = xs.Where(x =>
            {
                invoked++;
                if (x == 8)
                {
                    d.Dispose();
                }

                return IsPrime(x);
            }));

            scheduler.ScheduleAbsolute(Subscribed, () => d.Disposable = ys.Subscribe(res));

            scheduler.ScheduleAbsolute(Disposed, () => d.Dispose());

            scheduler.Start();

            res.Messages.AssertEqual(
                OnNext(230, 3),
                OnNext(340, 5),
                OnNext(390, 7)
            );

            xs.Subscriptions.AssertEqual(
                Subscribe(200, 450)
            );

            Assert.Equal(6, invoked);
        }

        [Fact]
        public void WhereWhereOptimization_Regular()
        {
            var scheduler = new TestScheduler();

            var xs = scheduler.CreateHotObservable(
                OnNext(110, 1),
                OnNext(180, 2),
                OnNext(230, 3),
                OnNext(270, 4),
                OnNext(340, 5),
                OnNext(380, 6),
                OnNext(390, 7),
                OnNext(450, 8),
                OnNext(470, 9),
                OnNext(560, 10),
                OnNext(580, 11),
                OnCompleted<int>(600)
            );

            var res = scheduler.Start(() =>
                xs.Where(x => x > 3).Where(x => x % 2 == 0)
            );

            res.Messages.AssertEqual(
                OnNext(270, 4),
                OnNext(380, 6),
                OnNext(450, 8),
                OnNext(560, 10),
                OnCompleted<int>(600)
            );
        }

        [Fact]
        public void WhereWhereOptimization_SecondPredicateThrows()
        {
            var scheduler = new TestScheduler();

            var xs = scheduler.CreateHotObservable(
                OnNext(110, 1),
                OnNext(180, 2),
                OnNext(230, 3),
                OnNext(270, 4),
                OnNext(340, 5),
                OnNext(380, 6),
                OnNext(390, 7),
                OnNext(450, 8),
                OnNext(470, 9),
                OnNext(560, 10),
                OnNext(580, 11),
                OnCompleted<int>(600)
            );

            var res = scheduler.Start(() =>
                xs.Where(x => x > 3).Where(x =>
                {
                    if (x <= 3)
                    {
                        throw new Exception();
                    }

                    return x % 2 == 0;
                })
            );

            res.Messages.AssertEqual(
                OnNext(270, 4),
                OnNext(380, 6),
                OnNext(450, 8),
                OnNext(560, 10),
                OnCompleted<int>(600)
            );
        }

        [Fact]
        public void WhereIndex_ArgumentChecking()
        {
            ReactiveAssert.Throws<ArgumentNullException>(() => ((IObservable<int>)null).Where(DummyFunc<int, int, bool>.Instance));
            ReactiveAssert.Throws<ArgumentNullException>(() => DummyObservable<int>.Instance.Where((Func<int, int, bool>)null));
            ReactiveAssert.Throws<ArgumentNullException>(() => DummyObservable<int>.Instance.Where(DummyFunc<int, int, bool>.Instance).Subscribe(null));
        }

        [Fact]
        public void WhereIndex_Complete()
        {
            var scheduler = new TestScheduler();

            var invoked = 0;

            var xs = scheduler.CreateHotObservable(
                OnNext(110, 1),
                OnNext(180, 2),
                OnNext(230, 3),
                OnNext(270, 4),
                OnNext(340, 5),
                OnNext(380, 6),
                OnNext(390, 7),
                OnNext(450, 8),
                OnNext(470, 9),
                OnNext(560, 10),
                OnNext(580, 11),
                OnCompleted<int>(600),
                OnNext(610, 12),
                OnError<int>(620, new Exception()),
                OnCompleted<int>(630)
            );

            var res = scheduler.Start(() =>
                xs.Where((x, i) =>
                {
                    invoked++;
                    return IsPrime(x + i * 10);
                })
            );

            res.Messages.AssertEqual(
                OnNext(230, 3),
                OnNext(390, 7),
                OnCompleted<int>(600)
            );

            xs.Subscriptions.AssertEqual(
                Subscribe(200, 600)
            );

            Assert.Equal(9, invoked);
        }

        [Fact]
        public void WhereIndex_True()
        {
            var scheduler = new TestScheduler();

            var invoked = 0;

            var xs = scheduler.CreateHotObservable(
                OnNext(110, 1),
                OnNext(180, 2),
                OnNext(230, 3),
                OnNext(270, 4),
                OnNext(340, 5),
                OnNext(380, 6),
                OnNext(390, 7),
                OnNext(450, 8),
                OnNext(470, 9),
                OnNext(560, 10),
                OnNext(580, 11),
                OnCompleted<int>(600)
            );

            var res = scheduler.Start(() =>
                xs.Where((x, i) =>
                {
                    invoked++;
                    return true;
                })
            );

            res.Messages.AssertEqual(
                OnNext(230, 3),
                OnNext(270, 4),
                OnNext(340, 5),
                OnNext(380, 6),
                OnNext(390, 7),
                OnNext(450, 8),
                OnNext(470, 9),
                OnNext(560, 10),
                OnNext(580, 11),
                OnCompleted<int>(600)
            );

            xs.Subscriptions.AssertEqual(
                Subscribe(200, 600)
            );

            Assert.Equal(9, invoked);
        }

        [Fact]
        public void WhereIndex_False()
        {
            var scheduler = new TestScheduler();

            var invoked = 0;

            var xs = scheduler.CreateHotObservable(
                OnNext(110, 1),
                OnNext(180, 2),
                OnNext(230, 3),
                OnNext(270, 4),
                OnNext(340, 5),
                OnNext(380, 6),
                OnNext(390, 7),
                OnNext(450, 8),
                OnNext(470, 9),
                OnNext(560, 10),
                OnNext(580, 11),
                OnCompleted<int>(600)
            );

            var res = scheduler.Start(() =>
                xs.Where((x, i) =>
                {
                    invoked++;
                    return false;
                })
            );

            res.Messages.AssertEqual(
                OnCompleted<int>(600)
            );

            xs.Subscriptions.AssertEqual(
                Subscribe(200, 600)
            );

            Assert.Equal(9, invoked);
        }

        [Fact]
        public void WhereIndex_Dispose()
        {
            var scheduler = new TestScheduler();

            var invoked = 0;

            var xs = scheduler.CreateHotObservable(
                OnNext(110, 1),
                OnNext(180, 2),
                OnNext(230, 3),
                OnNext(270, 4),
                OnNext(340, 5),
                OnNext(380, 6),
                OnNext(390, 7),
                OnNext(450, 8),
                OnNext(470, 9),
                OnNext(560, 10),
                OnNext(580, 11),
                OnCompleted<int>(600)
            );

            var res = scheduler.Start(() =>
                xs.Where((x, i) =>
                {
                    invoked++;
                    return IsPrime(x + i * 10);
                }),
                400
            );

            res.Messages.AssertEqual(
                OnNext(230, 3),
                OnNext(390, 7)
            );

            xs.Subscriptions.AssertEqual(
                Subscribe(200, 400)
            );

            Assert.Equal(5, invoked);
        }

        [Fact]
        public void WhereIndex_Error()
        {
            var scheduler = new TestScheduler();

            var invoked = 0;
            var ex = new Exception();

            var xs = scheduler.CreateHotObservable(
                OnNext(110, 1),
                OnNext(180, 2),
                OnNext(230, 3),
                OnNext(270, 4),
                OnNext(340, 5),
                OnNext(380, 6),
                OnNext(390, 7),
                OnNext(450, 8),
                OnNext(470, 9),
                OnNext(560, 10),
                OnNext(580, 11),
                OnError<int>(600, ex),
                OnNext(610, 12),
                OnError<int>(620, new Exception()),
                OnCompleted<int>(630)
            );

            var res = scheduler.Start(() =>
                xs.Where((x, i) =>
                {
                    invoked++;
                    return IsPrime(x + i * 10);
                })
            );

            res.Messages.AssertEqual(
                OnNext(230, 3),
                OnNext(390, 7),
                OnError<int>(600, ex)
            );

            xs.Subscriptions.AssertEqual(
                Subscribe(200, 600)
            );

            Assert.Equal(9, invoked);
        }

        [Fact]
        public void WhereIndex_Throw()
        {
            var scheduler = new TestScheduler();

            var invoked = 0;
            var ex = new Exception();

            var xs = scheduler.CreateHotObservable(
                OnNext(110, 1),
                OnNext(180, 2),
                OnNext(230, 3),
                OnNext(270, 4),
                OnNext(340, 5),
                OnNext(380, 6),
                OnNext(390, 7),
                OnNext(450, 8),
                OnNext(470, 9),
                OnNext(560, 10),
                OnNext(580, 11),
                OnCompleted<int>(600),
                OnNext(610, 12),
                OnError<int>(620, new Exception()),
                OnCompleted<int>(630)
            );

            var res = scheduler.Start(() =>
                xs.Where((x, i) =>
                {
                    invoked++;
                    if (x > 5)
                    {
                        throw ex;
                    }

                    return IsPrime(x + i * 10);
                })
            );

            res.Messages.AssertEqual(
                OnNext(230, 3),
                OnError<int>(380, ex)
            );

            xs.Subscriptions.AssertEqual(
                Subscribe(200, 380)
            );

            Assert.Equal(4, invoked);
        }

        [Fact]
        public void WhereIndex_DisposeInPredicate()
        {
            var scheduler = new TestScheduler();

            var invoked = 0;

            var xs = scheduler.CreateHotObservable(
                OnNext(110, 1),
                OnNext(180, 2),
                OnNext(230, 3),
                OnNext(270, 4),
                OnNext(340, 5),
                OnNext(380, 6),
                OnNext(390, 7),
                OnNext(450, 8),
                OnNext(470, 9),
                OnNext(560, 10),
                OnNext(580, 11),
                OnCompleted<int>(600),
                OnNext(610, 12),
                OnError<int>(620, new Exception()),
                OnCompleted<int>(630)
            );

            var res = scheduler.CreateObserver<int>();

            var d = new SerialDisposable();
            var ys = default(IObservable<int>);


            scheduler.ScheduleAbsolute(Created, () => ys = xs.Where((x, i) =>
            {
                invoked++;
                if (x == 8)
                {
                    d.Dispose();
                }

                return IsPrime(x + i * 10);
            }));

            scheduler.ScheduleAbsolute(Subscribed, () => d.Disposable = ys.Subscribe(res));

            scheduler.ScheduleAbsolute(Disposed, () => d.Dispose());

            scheduler.Start();

            res.Messages.AssertEqual(
                OnNext(230, 3),
                OnNext(390, 7)
            );

            xs.Subscriptions.AssertEqual(
                Subscribe(200, 450)
            );

            Assert.Equal(6, invoked);
        }

        [Fact]
        public void Where_Where1()
        {
            var scheduler = new TestScheduler();

            var xs = scheduler.CreateHotObservable(
                OnNext(110, 1),
                OnNext(180, 2),
                OnNext(230, 3),
                OnNext(270, 4),
                OnNext(340, 5),
                OnNext(380, 6),
                OnNext(390, 7),
                OnCompleted<int>(400)
            );

            var res = scheduler.Start(() =>
                xs.Where(x => x > 3).Where(x => x < 6)
            );

            res.Messages.AssertEqual(
                OnNext(270, 4),
                OnNext(340, 5),
                OnCompleted<int>(400)
            );

            xs.Subscriptions.AssertEqual(
                Subscribe(200, 400)
            );
        }

        [Fact]
        public void Where_Where2()
        {
            var scheduler = new TestScheduler();

            var xs = scheduler.CreateHotObservable(
                OnNext(110, 1),
                OnNext(180, 2),
                OnNext(230, 3),
                OnNext(270, 4),
                OnNext(340, 5),
                OnNext(380, 6),
                OnNext(390, 7),
                OnCompleted<int>(400)
            );

            var res = scheduler.Start(() =>
                xs.Where((x, i) => i >= 1).Where(x => x < 6)
            );

            res.Messages.AssertEqual(
                OnNext(270, 4),
                OnNext(340, 5),
                OnCompleted<int>(400)
            );

            xs.Subscriptions.AssertEqual(
                Subscribe(200, 400)
            );
        }

        [Fact]
        public void Where_Where3()
        {
            var scheduler = new TestScheduler();

            var xs = scheduler.CreateHotObservable(
                OnNext(110, 1),
                OnNext(180, 2),
                OnNext(230, 3),
                OnNext(270, 4),
                OnNext(340, 5),
                OnNext(380, 6),
                OnNext(390, 7),
                OnCompleted<int>(400)
            );

            var res = scheduler.Start(() =>
                xs.Where(x => x > 3).Where((x, i) => i < 2)
            );

            res.Messages.AssertEqual(
                OnNext(270, 4),
                OnNext(340, 5),
                OnCompleted<int>(400)
            );

            xs.Subscriptions.AssertEqual(
                Subscribe(200, 400)
            );
        }

        [Fact]
        public void Where_Where4()
        {
            var scheduler = new TestScheduler();

            var xs = scheduler.CreateHotObservable(
                OnNext(110, 1),
                OnNext(180, 2),
                OnNext(230, 3),
                OnNext(270, 4),
                OnNext(340, 5),
                OnNext(380, 6),
                OnNext(390, 7),
                OnCompleted<int>(400)
            );

            var res = scheduler.Start(() =>
                xs.Where((x, i) => i >= 1).Where((x, i) => i < 2)
            );

            res.Messages.AssertEqual(
                OnNext(270, 4),
                OnNext(340, 5),
                OnCompleted<int>(400)
            );

            xs.Subscriptions.AssertEqual(
                Subscribe(200, 400)
            );
        }

    }
}

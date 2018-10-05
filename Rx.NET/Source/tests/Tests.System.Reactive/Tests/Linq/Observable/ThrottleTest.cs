// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the Apache 2.0 License.
// See the LICENSE file in the project root for more information. 

using System;
using System.Collections.Generic;
using System.Linq;
using System.Reactive;
using System.Reactive.Linq;
using Microsoft.Reactive.Testing;
using ReactiveTests.Dummies;
using Xunit;

namespace ReactiveTests.Tests
{
    public class ThrottleTest : ReactiveTest
    {

        [Fact]
        public void Throttle_ArgumentChecking()
        {
            var scheduler = new TestScheduler();
            var someObservable = Observable.Empty<int>();

            ReactiveAssert.Throws<ArgumentNullException>(() => Observable.Throttle(default(IObservable<int>), TimeSpan.Zero));
            ReactiveAssert.Throws<ArgumentNullException>(() => Observable.Throttle(someObservable, TimeSpan.Zero, null));
            ReactiveAssert.Throws<ArgumentNullException>(() => Observable.Throttle(default(IObservable<int>), TimeSpan.Zero, scheduler));

            ReactiveAssert.Throws<ArgumentOutOfRangeException>(() => Observable.Throttle(someObservable, TimeSpan.FromSeconds(-1)));
            ReactiveAssert.Throws<ArgumentOutOfRangeException>(() => Observable.Throttle(someObservable, TimeSpan.FromSeconds(-1), scheduler));
        }

        private IEnumerable<Recorded<Notification<T>>> Generate<T, S>(S seed, Func<S, bool> condition, Func<S, S> iterate, Func<S, Recorded<Notification<T>>> selector, Func<S, Recorded<Notification<T>>> final)
        {
            S s;
            for (s = seed; condition(s); s = iterate(s))
            {
                yield return selector(s);
            }

            yield return final(s);
        }

        [Fact]
        public void Throttle_TimeSpan_AllPass()
        {
            var scheduler = new TestScheduler();

            var xs = scheduler.CreateHotObservable(
                OnNext(150, 0),
                OnNext(210, 1),
                OnNext(240, 2),
                OnNext(270, 3),
                OnNext(300, 4),
                OnCompleted<int>(400)
            );

            var res = scheduler.Start(() =>
                xs.Throttle(TimeSpan.FromTicks(20), scheduler)
            );

            res.Messages.AssertEqual(
                OnNext(230, 1),
                OnNext(260, 2),
                OnNext(290, 3),
                OnNext(320, 4),
                OnCompleted<int>(400)
            );

            xs.Subscriptions.AssertEqual(
                Subscribe(200, 400)
            );
        }

        [Fact]
        public void Throttle_TimeSpan_AllPass_ErrorEnd()
        {
            var ex = new Exception();

            var scheduler = new TestScheduler();

            var xs = scheduler.CreateHotObservable(
                OnNext(150, 0),
                OnNext(210, 1),
                OnNext(240, 2),
                OnNext(270, 3),
                OnNext(300, 4),
                OnError<int>(400, ex)
            );

            var res = scheduler.Start(() =>
                xs.Throttle(TimeSpan.FromTicks(20), scheduler)
            );

            res.Messages.AssertEqual(
                OnNext(230, 1),
                OnNext(260, 2),
                OnNext(290, 3),
                OnNext(320, 4),
                OnError<int>(400, ex)
            );

            xs.Subscriptions.AssertEqual(
                Subscribe(200, 400)
            );
        }

        [Fact]
        public void Throttle_TimeSpan_AllDrop()
        {
            var scheduler = new TestScheduler();

            var xs = scheduler.CreateHotObservable(
                OnNext(150, 0),
                OnNext(210, 1),
                OnNext(240, 2),
                OnNext(270, 3),
                OnNext(300, 4),
                OnNext(330, 5),
                OnNext(360, 6),
                OnNext(390, 7),
                OnCompleted<int>(400)
            );

            var res = scheduler.Start(() =>
                xs.Throttle(TimeSpan.FromTicks(40), scheduler)
            );

            res.Messages.AssertEqual(
                OnNext(400, 7),
                OnCompleted<int>(400)
            );

            xs.Subscriptions.AssertEqual(
                Subscribe(200, 400)
            );
        }

        [Fact]
        public void Throttle_TimeSpan_AllDrop_ErrorEnd()
        {
            var ex = new Exception();

            var scheduler = new TestScheduler();

            var xs = scheduler.CreateHotObservable(
                OnNext(150, 0),
                OnNext(210, 1),
                OnNext(240, 2),
                OnNext(270, 3),
                OnNext(300, 4),
                OnNext(330, 5),
                OnNext(360, 6),
                OnNext(390, 7),
                OnError<int>(400, ex)
            );

            var res = scheduler.Start(() =>
                xs.Throttle(TimeSpan.FromTicks(40), scheduler)
            );

            res.Messages.AssertEqual(
                OnError<int>(400, ex)
            );

            xs.Subscriptions.AssertEqual(
                Subscribe(200, 400)
            );
        }

        [Fact]
        public void Throttle_Empty()
        {
            var scheduler = new TestScheduler();

            var xs = scheduler.CreateHotObservable(
                OnNext(150, 0),
                OnCompleted<int>(300)
            );

            var res = scheduler.Start(() =>
                xs.Throttle(TimeSpan.FromTicks(10), scheduler)
            );

            res.Messages.AssertEqual(
                OnCompleted<int>(300)
            );

            xs.Subscriptions.AssertEqual(
                Subscribe(200, 300)
            );
        }

        [Fact]
        public void Throttle_Error()
        {
            var scheduler = new TestScheduler();

            var ex = new Exception();

            var xs = scheduler.CreateHotObservable(
                OnNext(150, 0),
                OnError<int>(300, ex)
            );

            var res = scheduler.Start(() =>
                xs.Throttle(TimeSpan.FromTicks(10), scheduler)
            );

            res.Messages.AssertEqual(
                OnError<int>(300, ex)
            );

            xs.Subscriptions.AssertEqual(
                Subscribe(200, 300)
            );
        }

        [Fact]
        public void Throttle_Never()
        {
            var scheduler = new TestScheduler();

            var xs = scheduler.CreateHotObservable(
                OnNext(150, 0)
            );

            var res = scheduler.Start(() =>
                xs.Throttle(TimeSpan.FromTicks(10), scheduler)
            );

            res.Messages.AssertEqual(
            );

            xs.Subscriptions.AssertEqual(
                Subscribe(200, 1000)
            );
        }

        [Fact]
        public void Throttle_Simple()
        {
            var scheduler = new TestScheduler();

            var xs = scheduler.CreateHotObservable(
                OnNext(150, 0),
                OnNext(210, 1),
                OnNext(240, 2),
                OnNext(250, 3),
                OnNext(280, 4),
                OnCompleted<int>(300)
            );

            var res = scheduler.Start(() =>
                xs.Throttle(TimeSpan.FromTicks(20), scheduler)
            );

            res.Messages.AssertEqual(
                OnNext(230, 1),
                OnNext(270, 3),
                OnNext(300, 4),
                OnCompleted<int>(300)
            );

            xs.Subscriptions.AssertEqual(
                Subscribe(200, 300)
            );
        }

        [Fact]
        public void Throttle_DefaultScheduler()
        {
            Assert.True(Observable.Return(1).Throttle(TimeSpan.FromMilliseconds(1)).ToEnumerable().SequenceEqual(new[] { 1 }));
        }

        [Fact]
        public void Throttle_Duration_ArgumentChecking()
        {
            var someObservable = DummyObservable<int>.Instance;

            ReactiveAssert.Throws<ArgumentNullException>(() => Observable.Throttle(default(IObservable<int>), x => someObservable));
            ReactiveAssert.Throws<ArgumentNullException>(() => Observable.Throttle(someObservable, default(Func<int, IObservable<string>>)));
        }

        [Fact]
        public void Throttle_Duration_DelayBehavior()
        {
            var scheduler = new TestScheduler();

            var xs = scheduler.CreateHotObservable(
                OnNext(150, -1),
                OnNext(250, 0),
                OnNext(280, 1),
                OnNext(310, 2),
                OnNext(350, 3),
                OnNext(400, 4),
                OnCompleted<int>(550)
            );

            var ys = new[] {
                scheduler.CreateColdObservable(
                    OnNext(20, 42),
                    OnNext(25, 99)
                ),
                scheduler.CreateColdObservable(
                    OnNext(20, 42),
                    OnNext(25, 99)
                ),
                scheduler.CreateColdObservable(
                    OnNext(20, 42),
                    OnNext(25, 99)
                ),
                scheduler.CreateColdObservable(
                    OnNext(20, 42),
                    OnNext(25, 99)
                ),
                scheduler.CreateColdObservable(
                    OnNext(20, 42),
                    OnNext(25, 99)
                ),
            };

            var res = scheduler.Start(() =>
                xs.Throttle(x => ys[x])
            );

            res.Messages.AssertEqual(
                OnNext(250 + 20, 0),
                OnNext(280 + 20, 1),
                OnNext(310 + 20, 2),
                OnNext(350 + 20, 3),
                OnNext(400 + 20, 4),
                OnCompleted<int>(550)
            );

            xs.Subscriptions.AssertEqual(
                Subscribe(200, 550)
            );

            ys[0].Subscriptions.AssertEqual(Subscribe(250, 250 + 20));
            ys[1].Subscriptions.AssertEqual(Subscribe(280, 280 + 20));
            ys[2].Subscriptions.AssertEqual(Subscribe(310, 310 + 20));
            ys[3].Subscriptions.AssertEqual(Subscribe(350, 350 + 20));
            ys[4].Subscriptions.AssertEqual(Subscribe(400, 400 + 20));
        }

        [Fact]
        public void Throttle_Duration_ThrottleBehavior()
        {
            var scheduler = new TestScheduler();

            var xs = scheduler.CreateHotObservable(
                OnNext(150, -1),
                OnNext(250, 0),
                OnNext(280, 1),
                OnNext(310, 2),
                OnNext(350, 3),
                OnNext(400, 4),
                OnCompleted<int>(550)
            );

            var ys = new[] {
                scheduler.CreateColdObservable(
                    OnNext(20, 42),
                    OnNext(25, 99)
                ),
                scheduler.CreateColdObservable(
                    OnNext(40, 42),
                    OnNext(45, 99)
                ),
                scheduler.CreateColdObservable(
                    OnNext(20, 42),
                    OnNext(25, 99)
                ),
                scheduler.CreateColdObservable(
                    OnNext(60, 42),
                    OnNext(65, 99)
                ),
                scheduler.CreateColdObservable(
                    OnNext(20, 42),
                    OnNext(25, 99)
                ),
            };

            var res = scheduler.Start(() =>
                xs.Throttle(x => ys[x])
            );

            res.Messages.AssertEqual(
                OnNext(250 + 20, 0),
                OnNext(310 + 20, 2),
                OnNext(400 + 20, 4),
                OnCompleted<int>(550)
            );

            xs.Subscriptions.AssertEqual(
                Subscribe(200, 550)
            );

            ys[0].Subscriptions.AssertEqual(Subscribe(250, 250 + 20));
            ys[1].Subscriptions.AssertEqual(Subscribe(280, 310));
            ys[2].Subscriptions.AssertEqual(Subscribe(310, 310 + 20));
            ys[3].Subscriptions.AssertEqual(Subscribe(350, 400));
            ys[4].Subscriptions.AssertEqual(Subscribe(400, 400 + 20));
        }

        [Fact]
        public void Throttle_Duration_EarlyCompletion()
        {
            var scheduler = new TestScheduler();

            var xs = scheduler.CreateHotObservable(
                OnNext(150, -1),
                OnNext(250, 0),
                OnNext(280, 1),
                OnNext(310, 2),
                OnNext(350, 3),
                OnNext(400, 4),
                OnCompleted<int>(410)
            );

            var ys = new[] {
                scheduler.CreateColdObservable(
                    OnNext(20, 42),
                    OnNext(25, 99)
                ),
                scheduler.CreateColdObservable(
                    OnNext(40, 42),
                    OnNext(45, 99)
                ),
                scheduler.CreateColdObservable(
                    OnNext(20, 42),
                    OnNext(25, 99)
                ),
                scheduler.CreateColdObservable(
                    OnNext(60, 42),
                    OnNext(65, 99)
                ),
                scheduler.CreateColdObservable(
                    OnNext(20, 42),
                    OnNext(25, 99)
                ),
            };

            var res = scheduler.Start(() =>
                xs.Throttle(x => ys[x])
            );

            res.Messages.AssertEqual(
                OnNext(250 + 20, 0),
                OnNext(310 + 20, 2),
                OnNext(410, 4),
                OnCompleted<int>(410)
            );

            xs.Subscriptions.AssertEqual(
                Subscribe(200, 410)
            );

            ys[0].Subscriptions.AssertEqual(Subscribe(250, 250 + 20));
            ys[1].Subscriptions.AssertEqual(Subscribe(280, 310));
            ys[2].Subscriptions.AssertEqual(Subscribe(310, 310 + 20));
            ys[3].Subscriptions.AssertEqual(Subscribe(350, 400));
            ys[4].Subscriptions.AssertEqual(Subscribe(400, 410));
        }

        [Fact]
        public void Throttle_Duration_InnerError()
        {
            var scheduler = new TestScheduler();

            var xs = scheduler.CreateHotObservable(
                OnNext(150, 1),
                OnNext(250, 2),
                OnNext(350, 3),
                OnNext(450, 4),
                OnCompleted<int>(550)
            );

            var ex = new Exception();

            var res = scheduler.Start(() =>
                xs.Throttle(x =>
                    x < 4 ? scheduler.CreateColdObservable(
                                OnNext(x * 10, "Ignore"),
                                OnNext(x * 10 + 5, "Aargh!")
                            )
                          : scheduler.CreateColdObservable(
                                OnError<string>(x * 10, ex)
                            )
                )
            );

            res.Messages.AssertEqual(
                OnNext(250 + 2 * 10, 2),
                OnNext(350 + 3 * 10, 3),
                OnError<int>(450 + 4 * 10, ex)
            );

            xs.Subscriptions.AssertEqual(
                Subscribe(200, 490)
            );
        }

        [Fact]
        public void Throttle_Duration_OuterError()
        {
            var scheduler = new TestScheduler();

            var ex = new Exception();

            var xs = scheduler.CreateHotObservable(
                OnNext(150, 1),
                OnNext(250, 2),
                OnNext(350, 3),
                OnNext(450, 4),
                OnError<int>(460, ex)
            );

            var res = scheduler.Start(() =>
                xs.Throttle(x =>
                    scheduler.CreateColdObservable(
                        OnNext(x * 10, "Ignore"),
                        OnNext(x * 10 + 5, "Aargh!")
                    )
                )
            );

            res.Messages.AssertEqual(
                OnNext(250 + 2 * 10, 2),
                OnNext(350 + 3 * 10, 3),
                OnError<int>(460, ex)
            );

            xs.Subscriptions.AssertEqual(
                Subscribe(200, 460)
            );
        }

        [Fact]
        public void Throttle_Duration_SelectorThrows()
        {
            var scheduler = new TestScheduler();

            var xs = scheduler.CreateHotObservable(
                OnNext(150, 1),
                OnNext(250, 2),
                OnNext(350, 3),
                OnNext(450, 4),
                OnCompleted<int>(550)
            );

            var ex = new Exception();

            var res = scheduler.Start(() =>
                xs.Throttle(x =>
                {
                    if (x < 4)
                    {
                        return scheduler.CreateColdObservable(
                                OnNext(x * 10, "Ignore"),
                                OnNext(x * 10 + 5, "Aargh!")
                            );
                    }
                    else
                    {
                        throw ex;
                    }
                })
            );

            res.Messages.AssertEqual(
                OnNext(250 + 2 * 10, 2),
                OnNext(350 + 3 * 10, 3),
                OnError<int>(450, ex)
            );

            xs.Subscriptions.AssertEqual(
                Subscribe(200, 450)
            );
        }

        [Fact]
        public void Throttle_Duration_InnerDone_DelayBehavior()
        {
            var scheduler = new TestScheduler();

            var xs = scheduler.CreateHotObservable(
                OnNext(150, 1),
                OnNext(250, 2),
                OnNext(350, 3),
                OnNext(450, 4),
                OnCompleted<int>(550)
            );

            var ex = new Exception();

            var res = scheduler.Start(() =>
                xs.Throttle(x =>
                    scheduler.CreateColdObservable(
                        OnCompleted<string>(x * 10)
                    )
                )
            );

            res.Messages.AssertEqual(
                OnNext(250 + 2 * 10, 2),
                OnNext(350 + 3 * 10, 3),
                OnNext(450 + 4 * 10, 4),
                OnCompleted<int>(550)
            );

            xs.Subscriptions.AssertEqual(
                Subscribe(200, 550)
            );
        }

        [Fact]
        public void Throttle_Duration_InnerDone_ThrottleBehavior()
        {
            var scheduler = new TestScheduler();

            var xs = scheduler.CreateHotObservable(
                OnNext(150, 1),
                OnNext(250, 2),
                OnNext(280, 3),
                OnNext(300, 4),
                OnNext(400, 5),
                OnNext(410, 6),
                OnCompleted<int>(550)
            );

            var ex = new Exception();

            var res = scheduler.Start(() =>
                xs.Throttle(x =>
                    scheduler.CreateColdObservable(
                        OnCompleted<string>(x * 10)
                    )
                )
            );

            res.Messages.AssertEqual(
                OnNext(250 + 2 * 10, 2),
                OnNext(300 + 4 * 10, 4),
                OnNext(410 + 6 * 10, 6),
                OnCompleted<int>(550)
            );

            xs.Subscriptions.AssertEqual(
                Subscribe(200, 550)
            );
        }

    }
}

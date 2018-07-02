// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the Apache 2.0 License.
// See the LICENSE file in the project root for more information. 

using System;
using System.Collections.Generic;
using System.Reactive;
using System.Reactive.Concurrency;
using System.Reactive.Linq;
using System.Threading;
using Microsoft.Reactive.Testing;
using ReactiveTests.Dummies;
using Xunit;

namespace ReactiveTests.Tests
{
    public class SubscribeTest : ReactiveTest
    {

        [Fact]
        public void SubscribeToEnumerable_ArgumentChecking()
        {
            ReactiveAssert.Throws<ArgumentNullException>(() => Observable.Subscribe(null, DummyObserver<int>.Instance));
            ReactiveAssert.Throws<ArgumentNullException>(() => Observable.Subscribe(DummyEnumerable<int>.Instance, null));

            ReactiveAssert.Throws<ArgumentNullException>(() => Observable.Subscribe(null, DummyObserver<int>.Instance, DummyScheduler.Instance));
            ReactiveAssert.Throws<ArgumentNullException>(() => Observable.Subscribe(DummyEnumerable<int>.Instance, DummyObserver<int>.Instance, null));
            ReactiveAssert.Throws<ArgumentNullException>(() => Observable.Subscribe(DummyEnumerable<int>.Instance, null, DummyScheduler.Instance));
            ReactiveAssert.Throws<NullReferenceException>(() => NullEnumeratorEnumerable<int>.Instance.Subscribe(Observer.Create<int>(x => { }), Scheduler.CurrentThread));
        }

        [Fact]
        public void SubscribeToEnumerable_Finite()
        {
            var scheduler = new TestScheduler();

            var results = scheduler.CreateObserver<int>();
            var d = default(IDisposable);
            var xs = default(MockEnumerable<int>);

            scheduler.ScheduleAbsolute(Created, () => xs = new MockEnumerable<int>(scheduler, Enumerable_Finite()));
            scheduler.ScheduleAbsolute(Subscribed, () => d = xs.Subscribe(results, scheduler));
            scheduler.ScheduleAbsolute(Disposed, () => d.Dispose());

            scheduler.Start();

            results.Messages.AssertEqual(
                OnNext(201, 1),
                OnNext(202, 2),
                OnNext(203, 3),
                OnNext(204, 4),
                OnNext(205, 5),
                OnCompleted<int>(206)
            );

            xs.Subscriptions.AssertEqual(
                Subscribe(200, 206)
            );
        }

        [Fact]
        public void SubscribeToEnumerable_Infinite()
        {
            var scheduler = new TestScheduler();

            var results = scheduler.CreateObserver<int>();
            var d = default(IDisposable);
            var xs = default(MockEnumerable<int>);

            scheduler.ScheduleAbsolute(Created, () => xs = new MockEnumerable<int>(scheduler, Enumerable_Infinite()));
            scheduler.ScheduleAbsolute(Subscribed, () => d = xs.Subscribe(results, scheduler));
            scheduler.ScheduleAbsolute(210, () => d.Dispose());

            scheduler.Start();

            results.Messages.AssertEqual(
                OnNext(201, 1),
                OnNext(202, 1),
                OnNext(203, 1),
                OnNext(204, 1),
                OnNext(205, 1),
                OnNext(206, 1),
                OnNext(207, 1),
                OnNext(208, 1),
                OnNext(209, 1)
            );

            xs.Subscriptions.AssertEqual(
                Subscribe(200, 210)
            );
        }

        [Fact]
        public void SubscribeToEnumerable_Error()
        {
            var scheduler = new TestScheduler();

            var results = scheduler.CreateObserver<int>();
            var d = default(IDisposable);
            var xs = default(MockEnumerable<int>);
            var ex = new Exception();

            scheduler.ScheduleAbsolute(Created, () => xs = new MockEnumerable<int>(scheduler, Enumerable_Error(ex)));
            scheduler.ScheduleAbsolute(Subscribed, () => d = xs.Subscribe(results, scheduler));
            scheduler.ScheduleAbsolute(Disposed, () => d.Dispose());

            scheduler.Start();

            results.Messages.AssertEqual(
                OnNext(201, 1),
                OnNext(202, 2),
                OnNext(203, 3),
                OnError<int>(204, ex)
            );

            xs.Subscriptions.AssertEqual(
                Subscribe(200, 204)
            );
        }

        [Fact]
        public void SubscribeToEnumerable_DefaultScheduler()
        {
            for (var i = 0; i < 100; i++)
            {
                var scheduler = new TestScheduler();

                var results1 = new List<int>();
                var results2 = new List<int>();

                var s1 = new Semaphore(0, 1);
                var s2 = new Semaphore(0, 1);

                Observable.Subscribe(Enumerable_Finite(),
                    Observer.Create<int>(x => results1.Add(x), ex => { throw ex; }, () => s1.Release()));
                Observable.Subscribe(Enumerable_Finite(),
                    Observer.Create<int>(x => results2.Add(x), ex => { throw ex; }, () => s2.Release()),
                    DefaultScheduler.Instance);

                s1.WaitOne();
                s2.WaitOne();

                results1.AssertEqual(results2);
            }
        }

        private IEnumerable<int> Enumerable_Finite()
        {
            yield return 1;
            yield return 2;
            yield return 3;
            yield return 4;
            yield return 5;
            yield break;
        }

        private IEnumerable<int> Enumerable_Infinite()
        {
            while (true)
            {
                yield return 1;
            }
        }

        private IEnumerable<int> Enumerable_Error(Exception exception)
        {
            yield return 1;
            yield return 2;
            yield return 3;
            throw exception;
        }

        #region Subscribe

        [Fact]
        public void Subscribe_ArgumentChecking()
        {
            var someObservable = Observable.Empty<int>();

            ReactiveAssert.Throws<ArgumentNullException>(() => ObservableExtensions.Subscribe<int>(default));

            ReactiveAssert.Throws<ArgumentNullException>(() => ObservableExtensions.Subscribe<int>(default, _ => { }));
            ReactiveAssert.Throws<ArgumentNullException>(() => ObservableExtensions.Subscribe(someObservable, default(Action<int>)));

            ReactiveAssert.Throws<ArgumentNullException>(() => ObservableExtensions.Subscribe<int>(default, _ => { }, () => { }));
            ReactiveAssert.Throws<ArgumentNullException>(() => ObservableExtensions.Subscribe(someObservable, default, () => { }));
            ReactiveAssert.Throws<ArgumentNullException>(() => ObservableExtensions.Subscribe(someObservable, _ => { }, default(Action)));

            ReactiveAssert.Throws<ArgumentNullException>(() => ObservableExtensions.Subscribe<int>(default, _ => { }, (Exception _) => { }));
            ReactiveAssert.Throws<ArgumentNullException>(() => ObservableExtensions.Subscribe(someObservable, default, (Exception _) => { }));
            ReactiveAssert.Throws<ArgumentNullException>(() => ObservableExtensions.Subscribe(someObservable, _ => { }, default(Action<Exception>)));

            ReactiveAssert.Throws<ArgumentNullException>(() => ObservableExtensions.Subscribe<int>(default, _ => { }, (Exception _) => { }, () => { }));
            ReactiveAssert.Throws<ArgumentNullException>(() => ObservableExtensions.Subscribe(someObservable, default, (Exception _) => { }, () => { }));
            ReactiveAssert.Throws<ArgumentNullException>(() => ObservableExtensions.Subscribe(someObservable, _ => { }, default, () => { }));
            ReactiveAssert.Throws<ArgumentNullException>(() => ObservableExtensions.Subscribe(someObservable, _ => { }, (Exception _) => { }, default(Action)));
        }

        [Fact]
        public void Subscribe_None_Return()
        {
            Observable.Return(1, Scheduler.Immediate).Subscribe();
        }

        [Fact]
        public void Subscribe_None_Throw()
        {
            var ex = new Exception();

            var xs = Observable.Throw<int>(ex, Scheduler.Immediate);

            ReactiveAssert.Throws(ex, () => xs.Subscribe());
        }

        [Fact]
        public void Subscribe_None_Empty()
        {
            Observable.Empty<int>(Scheduler.Immediate).Subscribe((int _) => { Assert.True(false); });
        }

        [Fact]
        public void Subscribe_OnNext_Return()
        {
            var _x = -1;
            Observable.Return(42, Scheduler.Immediate).Subscribe((int x) => { _x = x; });
            Assert.Equal(42, _x);
        }

        [Fact]
        public void Subscribe_OnNext_Throw()
        {
            var ex = new Exception();

            var xs = Observable.Throw<int>(ex, Scheduler.Immediate);

            ReactiveAssert.Throws(ex, () => xs.Subscribe(_ => { Assert.True(false); }));
        }

        [Fact]
        public void Subscribe_OnNext_Empty()
        {
            Observable.Empty<int>(Scheduler.Immediate).Subscribe((int _) => { Assert.True(false); });
        }

        [Fact]
        public void Subscribe_OnNextOnCompleted_Return()
        {
            var finished = false;
            var _x = -1;
            Observable.Return(42, Scheduler.Immediate).Subscribe((int x) => { _x = x; }, () => { finished = true; });
            Assert.Equal(42, _x);
            Assert.True(finished);
        }

        [Fact]
        public void Subscribe_OnNextOnCompleted_Throw()
        {
            var ex = new Exception();

            var xs = Observable.Throw<int>(ex, Scheduler.Immediate);

            ReactiveAssert.Throws(ex, () => xs.Subscribe(_ => { Assert.True(false); }, () => { Assert.True(false); }));
        }

        [Fact]
        public void Subscribe_OnNextOnCompleted_Empty()
        {
            var finished = false;
            Observable.Empty<int>(Scheduler.Immediate).Subscribe((int _) => { Assert.True(false); }, () => { finished = true; });
            Assert.True(finished);
        }

        #endregion

        #region Subscribe with CancellationToken

        [Fact]
        public void Subscribe_CT_ArgumentChecking()
        {
            var someObservable = Observable.Empty<int>();
            var someObserver = Observer.Create<int>(_ => { });
            var ct = CancellationToken.None;

            ReactiveAssert.Throws<ArgumentNullException>(() => ObservableExtensions.Subscribe(default, someObserver, ct));
            ReactiveAssert.Throws<ArgumentNullException>(() => ObservableExtensions.Subscribe(someObservable, default(IObserver<int>), ct));

            ReactiveAssert.Throws<ArgumentNullException>(() => ObservableExtensions.Subscribe<int>(default, ct));

            ReactiveAssert.Throws<ArgumentNullException>(() => ObservableExtensions.Subscribe<int>(default, _ => { }, ct));
            ReactiveAssert.Throws<ArgumentNullException>(() => ObservableExtensions.Subscribe(someObservable, default(Action<int>), ct));

            ReactiveAssert.Throws<ArgumentNullException>(() => ObservableExtensions.Subscribe<int>(default, _ => { }, () => { }, ct));
            ReactiveAssert.Throws<ArgumentNullException>(() => ObservableExtensions.Subscribe(someObservable, default, () => { }, ct));
            ReactiveAssert.Throws<ArgumentNullException>(() => ObservableExtensions.Subscribe(someObservable, _ => { }, default(Action), ct));

            ReactiveAssert.Throws<ArgumentNullException>(() => ObservableExtensions.Subscribe<int>(default, _ => { }, (Exception _) => { }, ct));
            ReactiveAssert.Throws<ArgumentNullException>(() => ObservableExtensions.Subscribe(someObservable, default, (Exception _) => { }, ct));
            ReactiveAssert.Throws<ArgumentNullException>(() => ObservableExtensions.Subscribe(someObservable, _ => { }, default(Action<Exception>), ct));

            ReactiveAssert.Throws<ArgumentNullException>(() => ObservableExtensions.Subscribe<int>(default, _ => { }, (Exception _) => { }, () => { }, ct));
            ReactiveAssert.Throws<ArgumentNullException>(() => ObservableExtensions.Subscribe(someObservable, default, (Exception _) => { }, () => { }, ct));
            ReactiveAssert.Throws<ArgumentNullException>(() => ObservableExtensions.Subscribe(someObservable, _ => { }, default, () => { }, ct));
            ReactiveAssert.Throws<ArgumentNullException>(() => ObservableExtensions.Subscribe(someObservable, _ => { }, (Exception _) => { }, default, ct));
        }

        [Fact]
        public void Subscribe_CT_None()
        {
            var scheduler = new TestScheduler();

            var xs = scheduler.CreateHotObservable(
                OnNext(210, 1),
                OnNext(220, 2),
                OnNext(230, 3),
                OnCompleted<int>(240)
            );

            var obs = scheduler.CreateObserver<int>();

            scheduler.ScheduleAbsolute(200, () => xs.Subscribe(obs, CancellationToken.None));

            scheduler.Start();

            obs.Messages.AssertEqual(
                OnNext(210, 1),
                OnNext(220, 2),
                OnNext(230, 3),
                OnCompleted<int>(240)
            );

            xs.Subscriptions.AssertEqual(
                Subscribe(200, Subscription.Infinite /* no auto-dispose when using CreateHotObservable */)
            );
        }

        [Fact]
        public void Subscribe_CT_CancelBeforeBegin()
        {
            var scheduler = new TestScheduler();

            var xs = scheduler.CreateHotObservable(
                OnNext(210, 1),
                OnNext(220, 2),
                OnNext(230, 3),
                OnCompleted<int>(240)
            );

            var cts = new CancellationTokenSource();

            var obs = scheduler.CreateObserver<int>();

            scheduler.ScheduleAbsolute(200, () => xs.Subscribe(obs, cts.Token));
            scheduler.ScheduleAbsolute(150, cts.Cancel);

            scheduler.Start();

            obs.Messages.AssertEqual(
            );

            xs.Subscriptions.AssertEqual(
            );
        }

        [Fact]
        public void Subscribe_CT_CancelMiddle()
        {
            var scheduler = new TestScheduler();

            var xs = scheduler.CreateHotObservable(
                OnNext(210, 1),
                OnNext(220, 2),
                OnNext(230, 3),
                OnCompleted<int>(240)
            );

            var cts = new CancellationTokenSource();

            var obs = scheduler.CreateObserver<int>();

            scheduler.ScheduleAbsolute(200, () => xs.Subscribe(obs, cts.Token));
            scheduler.ScheduleAbsolute(225, cts.Cancel);

            scheduler.Start();

            obs.Messages.AssertEqual(
                OnNext(210, 1),
                OnNext(220, 2)
            );

            xs.Subscriptions.AssertEqual(
                Subscribe(200, 225)
            );
        }

        [Fact]
        public void Subscribe_CT_CancelAfterEnd()
        {
            var scheduler = new TestScheduler();

            var ex = new Exception();

            var xs = scheduler.CreateHotObservable(
                OnNext(210, 1),
                OnNext(220, 2),
                OnNext(230, 3),
                OnError<int>(240, ex)
            );

            var cts = new CancellationTokenSource();

            var obs = scheduler.CreateObserver<int>();

            scheduler.ScheduleAbsolute(200, () => xs.Subscribe(obs, cts.Token));
            scheduler.ScheduleAbsolute(250, cts.Cancel);

            scheduler.Start();

            obs.Messages.AssertEqual(
                OnNext(210, 1),
                OnNext(220, 2),
                OnNext(230, 3),
                OnError<int>(240, ex)
            );

            xs.Subscriptions.AssertEqual(
                Subscribe(200, Subscription.Infinite /* no auto-dispose when using CreateHotObservable */)
            );
        }

        [Fact]
        public void Subscribe_CT_NeverCancel()
        {
            var scheduler = new TestScheduler();

            var xs = scheduler.CreateHotObservable(
                OnNext(210, 1),
                OnNext(220, 2),
                OnNext(230, 3),
                OnCompleted<int>(240)
            );

            var cts = new CancellationTokenSource();

            var obs = scheduler.CreateObserver<int>();

            scheduler.ScheduleAbsolute(200, () => xs.Subscribe(obs, cts.Token));

            scheduler.Start();

            obs.Messages.AssertEqual(
                OnNext(210, 1),
                OnNext(220, 2),
                OnNext(230, 3),
                OnCompleted<int>(240)
            );

            xs.Subscriptions.AssertEqual(
                Subscribe(200, Subscription.Infinite /* no auto-dispose when using CreateHotObservable */)
            );
        }

        [Fact]
        public void Subscribe_CT_Overloads_AlreadyCancelled()
        {
            var xs = Observable.Defer(() =>
            {
                Assert.True(false);
                return Observable.Return(42, Scheduler.Immediate);
            });

            var cts = new CancellationTokenSource();
            cts.Cancel();

            xs.Subscribe(cts.Token);
            xs.Subscribe(_ => { }, cts.Token);
            xs.Subscribe(_ => { }, ex => { }, cts.Token);
            xs.Subscribe(_ => { }, () => { }, cts.Token);
            xs.Subscribe(_ => { }, ex => { }, () => { }, cts.Token);
            xs.Subscribe(Observer.Create<int>(_ => { }, ex => { }, () => { }), cts.Token);
        }

        [Fact]
        public void Subscribe_CT_Overloads_None()
        {
            var i = 0;
            var n = 0;
            var e = 0;
            var c = 0;

            var xs = Observable.Defer(() =>
            {
                i++;
                return Observable.Return(42, Scheduler.Immediate);
            });

            xs.Subscribe(CancellationToken.None);
            xs.Subscribe(_ => { n++; }, CancellationToken.None);
            xs.Subscribe(_ => { n++; }, ex => { e++; }, CancellationToken.None);
            xs.Subscribe(_ => { n++; }, () => { c++; }, CancellationToken.None);
            xs.Subscribe(_ => { n++; }, ex => { e++; }, () => { c++; }, CancellationToken.None);
            xs.Subscribe(Observer.Create<int>(_ => { n++; }, ex => { e++; }, () => { c++; }), CancellationToken.None);

            Assert.Equal(6, i);
            Assert.Equal(5, n);
            Assert.Equal(0, e);
            Assert.Equal(3, c);
        }

        [Fact]
        public void Subscribe_CT_CancelDuringCallback()
        {
            var scheduler = new TestScheduler();

            var xs = scheduler.CreateHotObservable(
                OnNext(210, 1),
                OnNext(220, 2),
                OnNext(230, 3),
                OnCompleted<int>(240)
            );

            var cts = new CancellationTokenSource();

            var n = 0;

            scheduler.ScheduleAbsolute(200, () => xs.Subscribe(x =>
            {
                n++;

                if (x == 2)
                {
                    cts.Cancel();
                }
            }, cts.Token));

            scheduler.Start();

            Assert.Equal(2, n);

            xs.Subscriptions.AssertEqual(
                Subscribe(200, 220)
            );
        }

        #endregion

    }
}

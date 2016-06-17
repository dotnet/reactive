// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the Apache 2.0 License.
// See the LICENSE file in the project root for more information. 

#if HAS_AWAIT

using System;
using System.Collections.Generic;
using System.Reactive;
using System.Reactive.Concurrency;
using System.Reactive.Linq;
using System.Reactive.Subjects;
using System.Threading;
using Microsoft.Reactive.Testing;
using Xunit;
using ReactiveTests.Dummies;
using System.Reactive.Disposables;

namespace ReactiveTests.Tests
{
    
    public class ObservableAwaiterTest : ReactiveTest
    {
        [Fact]
        public void Await_ArgumentChecking()
        {
            ReactiveAssert.Throws<ArgumentNullException>(() => Observable.GetAwaiter<int>(default(IObservable<int>)));
            ReactiveAssert.Throws<ArgumentNullException>(() => Observable.GetAwaiter<int>(default(IConnectableObservable<int>)));

            ReactiveAssert.Throws<ArgumentNullException>(() => Observable.GetAwaiter(Observable.Empty<int>()).OnCompleted(null));
        }

        [Fact]
        public void Await()
        {
            SynchronizationContext.SetSynchronizationContext(null);

            var scheduler = new TestScheduler();

            var xs = scheduler.CreateHotObservable(
                OnNext(20, -1),
                OnNext(150, 0),
                OnNext(220, 1),
                OnNext(290, 2),
                OnNext(340, 3),
                OnCompleted<int>(410)
            );

            var awaiter = default(AsyncSubject<int>);
            var result = default(int);
            var t = long.MaxValue;

            scheduler.ScheduleAbsolute(100, () => awaiter = xs.GetAwaiter());
            scheduler.ScheduleAbsolute(200, () => awaiter.OnCompleted(() => { t = scheduler.Clock; result = awaiter.GetResult(); }));

            scheduler.Start();

            Assert.Equal(410, t);
            Assert.Equal(3, result);

            xs.Subscriptions.AssertEqual(
                Subscribe(100)
            );
        }

        [Fact]
        public void Await_Connectable()
        {
            SynchronizationContext.SetSynchronizationContext(null);

            var scheduler = new TestScheduler();

            var s = default(long);

            var xs = Observable.Create<int>(observer =>
            {
                s = scheduler.Clock;

                return StableCompositeDisposable.Create(
                    scheduler.ScheduleAbsolute(250, () => { observer.OnNext(42); }),
                    scheduler.ScheduleAbsolute(260, () => { observer.OnCompleted(); })
                );
            });

            var ys = xs.Publish();

            var awaiter = default(AsyncSubject<int>);
            var result = default(int);
            var t = long.MaxValue;

            scheduler.ScheduleAbsolute(100, () => awaiter = ys.GetAwaiter());
            scheduler.ScheduleAbsolute(200, () => awaiter.OnCompleted(() => { t = scheduler.Clock; result = awaiter.GetResult(); }));

            scheduler.Start();

            Assert.Equal(100, s);
            Assert.Equal(260, t);
            Assert.Equal(42, result);
        }

        [Fact]
        public void Await_Error()
        {
            SynchronizationContext.SetSynchronizationContext(null);

            var scheduler = new TestScheduler();

            var ex = new Exception();

            var xs = scheduler.CreateHotObservable(
                OnNext(20, -1),
                OnNext(150, 0),
                OnNext(220, 1),
                OnNext(290, 2),
                OnNext(340, 3),
                OnError<int>(410, ex)
            );

            var awaiter = default(AsyncSubject<int>);
            var t = long.MaxValue;

            scheduler.ScheduleAbsolute(100, () => awaiter = xs.GetAwaiter());
            scheduler.ScheduleAbsolute(200, () => awaiter.OnCompleted(() => { t = scheduler.Clock; ReactiveAssert.Throws(ex, () => awaiter.GetResult()); }));

            scheduler.Start();

            Assert.Equal(410, t);

            xs.Subscriptions.AssertEqual(
                Subscribe(100)
            );
        }

        [Fact]
        public void Await_Never()
        {
            SynchronizationContext.SetSynchronizationContext(null);

            var scheduler = new TestScheduler();

            var xs = scheduler.CreateHotObservable(
                OnNext(20, -1),
                OnNext(150, 0),
                OnNext(220, 1),
                OnNext(290, 2),
                OnNext(340, 3)
            );

            var awaiter = default(AsyncSubject<int>);
            var hasValue = default(bool);
            var t = long.MaxValue;

            scheduler.ScheduleAbsolute(100, () => awaiter = xs.GetAwaiter());
            scheduler.ScheduleAbsolute(200, () => awaiter.OnCompleted(() => { t = scheduler.Clock; awaiter.GetResult(); hasValue = true; }));

            scheduler.Start();

            Assert.Equal(long.MaxValue, t);
            Assert.False(hasValue);

            xs.Subscriptions.AssertEqual(
                Subscribe(100)
            );
        }

        [Fact]
        public void Await_Empty()
        {
            SynchronizationContext.SetSynchronizationContext(null);

            var scheduler = new TestScheduler();

            var xs = scheduler.CreateHotObservable(
                OnCompleted<int>(300)
            );

            var awaiter = default(AsyncSubject<int>);
            var t = long.MaxValue;

            scheduler.ScheduleAbsolute(100, () => awaiter = xs.GetAwaiter());
            scheduler.ScheduleAbsolute(200, () => awaiter.OnCompleted(() => { t = scheduler.Clock; ReactiveAssert.Throws<InvalidOperationException>(() => awaiter.GetResult()); }));

            scheduler.Start();

            Assert.Equal(300, t);

            xs.Subscriptions.AssertEqual(
                Subscribe(100)
            );
        }

        [Fact]
        public void RunAsync_ArgumentChecking()
        {
            var ct = CancellationToken.None;

            ReactiveAssert.Throws<ArgumentNullException>(() => Observable.RunAsync<int>(default(IObservable<int>), ct));
            ReactiveAssert.Throws<ArgumentNullException>(() => Observable.RunAsync<int>(default(IConnectableObservable<int>), ct));
        }

        [Fact]
        public void RunAsync_Simple()
        {
            SynchronizationContext.SetSynchronizationContext(null);

            var scheduler = new TestScheduler();

            var xs = scheduler.CreateHotObservable(
                OnNext(220, 42),
                OnCompleted<int>(250)
            );

            var awaiter = default(AsyncSubject<int>);
            var result = default(int);
            var t = long.MaxValue;

            scheduler.ScheduleAbsolute(100, () => awaiter = xs.RunAsync(CancellationToken.None));
            scheduler.ScheduleAbsolute(200, () => awaiter.OnCompleted(() => { t = scheduler.Clock; result = awaiter.GetResult(); }));

            scheduler.Start();

            Assert.Equal(250, t);
            Assert.Equal(42, result);

            xs.Subscriptions.AssertEqual(
                Subscribe(100)
            );
        }

        [Fact]
        public void RunAsync_Cancelled()
        {
            SynchronizationContext.SetSynchronizationContext(null);

            var cts = new CancellationTokenSource();
            cts.Cancel();

            var scheduler = new TestScheduler();

            var xs = scheduler.CreateHotObservable(
                OnNext(220, 42),
                OnCompleted<int>(250)
            );

            var awaiter = default(AsyncSubject<int>);
            var result = default(int);
            var t = long.MaxValue;

            scheduler.ScheduleAbsolute(100, () => awaiter = xs.RunAsync(cts.Token));
            scheduler.ScheduleAbsolute(200, () => awaiter.OnCompleted(() =>
            {
                t = scheduler.Clock;

                ReactiveAssert.Throws<OperationCanceledException>(() =>
                {
                    result = awaiter.GetResult();
                });
            }));

            scheduler.Start();

            Assert.Equal(200, t);

            xs.Subscriptions.AssertEqual(
            );
        }

        [Fact]
        public void RunAsync_Cancel()
        {
            SynchronizationContext.SetSynchronizationContext(null);

            var cts = new CancellationTokenSource();

            var scheduler = new TestScheduler();

            var xs = scheduler.CreateHotObservable(
                OnNext(220, 42),
                OnCompleted<int>(250)
            );

            var awaiter = default(AsyncSubject<int>);
            var result = default(int);
            var t = long.MaxValue;

            scheduler.ScheduleAbsolute(100, () => awaiter = xs.RunAsync(cts.Token));
            scheduler.ScheduleAbsolute(200, () => awaiter.OnCompleted(() =>
            {
                t = scheduler.Clock;

                ReactiveAssert.Throws<OperationCanceledException>(() =>
                {
                    result = awaiter.GetResult();
                });
            }));
            scheduler.ScheduleAbsolute(210, () => cts.Cancel());

            scheduler.Start();

            Assert.Equal(210, t);

            xs.Subscriptions.AssertEqual(
                Subscribe(100, 210)
            );
        }

        [Fact]
        public void RunAsync_Connectable()
        {
            SynchronizationContext.SetSynchronizationContext(null);

            var scheduler = new TestScheduler();

            var s = default(long);

            var xs = Observable.Create<int>(observer =>
            {
                s = scheduler.Clock;

                return StableCompositeDisposable.Create(
                    scheduler.ScheduleAbsolute(250, () => { observer.OnNext(42); }),
                    scheduler.ScheduleAbsolute(260, () => { observer.OnCompleted(); })
                );
            });

            var ys = xs.Publish();

            var awaiter = default(AsyncSubject<int>);
            var result = default(int);
            var t = long.MaxValue;

            scheduler.ScheduleAbsolute(100, () => awaiter = ys.RunAsync(CancellationToken.None));
            scheduler.ScheduleAbsolute(200, () => awaiter.OnCompleted(() => { t = scheduler.Clock; result = awaiter.GetResult(); }));

            scheduler.Start();

            Assert.Equal(100, s);
            Assert.Equal(260, t);
            Assert.Equal(42, result);
        }

        [Fact]
        public void RunAsync_Connectable_Cancelled()
        {
            SynchronizationContext.SetSynchronizationContext(null);

            var cts = new CancellationTokenSource();
            cts.Cancel();

            var scheduler = new TestScheduler();

            var s = default(long?);

            var xs = Observable.Create<int>(observer =>
            {
                s = scheduler.Clock;

                return StableCompositeDisposable.Create(
                    scheduler.ScheduleAbsolute(250, () => { observer.OnNext(42); }),
                    scheduler.ScheduleAbsolute(260, () => { observer.OnCompleted(); })
                );
            });

            var ys = xs.Publish();

            var awaiter = default(AsyncSubject<int>);
            var result = default(int);
            var t = long.MaxValue;

            scheduler.ScheduleAbsolute(100, () => awaiter = ys.RunAsync(cts.Token));
            scheduler.ScheduleAbsolute(200, () => awaiter.OnCompleted(() =>
            {
                t = scheduler.Clock;

                ReactiveAssert.Throws<OperationCanceledException>(() =>
                {
                    result = awaiter.GetResult();
                });
            }));

            scheduler.Start();

            Assert.False(s.HasValue);
            Assert.Equal(200, t);
        }

        [Fact]
        public void RunAsync_Connectable_Cancel()
        {
            SynchronizationContext.SetSynchronizationContext(null);

            var cts = new CancellationTokenSource();

            var scheduler = new TestScheduler();

            var s = default(long);
            var d = default(long);

            var xs = Observable.Create<int>(observer =>
            {
                s = scheduler.Clock;

                return StableCompositeDisposable.Create(
                    scheduler.ScheduleAbsolute(250, () => { observer.OnNext(42); }),
                    scheduler.ScheduleAbsolute(260, () => { observer.OnCompleted(); }),
                    Disposable.Create(() => { d = scheduler.Clock; })
                );
            });

            var ys = xs.Publish();

            var awaiter = default(AsyncSubject<int>);
            var result = default(int);
            var t = long.MaxValue;

            scheduler.ScheduleAbsolute(100, () => awaiter = ys.RunAsync(cts.Token));
            scheduler.ScheduleAbsolute(200, () => awaiter.OnCompleted(() =>
            {
                t = scheduler.Clock;

                ReactiveAssert.Throws<OperationCanceledException>(() =>
                {
                    result = awaiter.GetResult();
                });
            }));
            scheduler.ScheduleAbsolute(210, () => cts.Cancel());

            scheduler.Start();

            Assert.Equal(100, s);
            Assert.Equal(210, d);
            Assert.Equal(210, t);
        }
    }
}

#endif

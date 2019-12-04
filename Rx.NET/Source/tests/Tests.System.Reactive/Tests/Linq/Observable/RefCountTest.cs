// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the Apache 2.0 License.
// See the LICENSE file in the project root for more information. 

using System;
using System.Collections.Generic;
using System.Reactive;
using System.Reactive.Concurrency;
using System.Reactive.Disposables;
using System.Reactive.Linq;
using System.Reactive.Subjects;
using Microsoft.Reactive.Testing;
using Xunit;

namespace ReactiveTests.Tests
{
    public class RefCountTest : ReactiveTest
    {
        private sealed class DematerializingConnectableObservable<T> : IConnectableObservable<T>
        {
            private readonly IConnectableObservable<Notification<T>> _subject;

            public DematerializingConnectableObservable(IConnectableObservable<Notification<T>> subject)
            {
                _subject = subject;
            }

            public IDisposable Subscribe(IObserver<T> observer)
            {
                return _subject.Dematerialize().Subscribe(observer);
            }

            public IDisposable Connect()
            {
                return _subject.Connect();
            }
        }

        [Fact]
        public void RefCount_ArgumentChecking()
        {
            ReactiveAssert.Throws<ArgumentNullException>(() => Observable.RefCount<int>(null));
            ReactiveAssert.Throws<ArgumentNullException>(() => Observable.RefCount<int>(null, 2));
            ReactiveAssert.Throws<ArgumentOutOfRangeException>(() => Observable.RefCount(Observable.Never<int>().Publish(), 0));
            ReactiveAssert.Throws<ArgumentOutOfRangeException>(() => Observable.RefCount(Observable.Never<int>().Publish(), -1));
            ReactiveAssert.Throws<ArgumentOutOfRangeException>(() => Observable.RefCount(Observable.Never<int>().Publish(), -2));
        }

        [Fact]
        public void RefCount_ConnectsOnFirst()
        {
            var scheduler = new TestScheduler();

            var xs = scheduler.CreateHotObservable(
                OnNext(210, 1),
                OnNext(220, 2),
                OnNext(230, 3),
                OnNext(240, 4),
                OnCompleted<int>(250)
            );

            var subject = new MySubject();

            var conn = new ConnectableObservable<int>(xs, subject);

            var res = scheduler.Start(() =>
                conn.RefCount()
            );

            res.Messages.AssertEqual(
                OnNext(210, 1),
                OnNext(220, 2),
                OnNext(230, 3),
                OnNext(240, 4),
                OnCompleted<int>(250)
            );

            Assert.True(subject.Disposed);
        }

        [Fact]
        public void RefCount_NotConnected()
        {
            var disconnected = false;
            var count = 0;
            var xs = Observable.Defer(() =>
            {
                count++;
                return Observable.Create<int>(obs =>
                {
                    return () => { disconnected = true; };
                });
            });

            var subject = new MySubject();

            var conn = new ConnectableObservable<int>(xs, subject);
            var refd = conn.RefCount();

            var dis1 = refd.Subscribe();
            Assert.Equal(1, count);
            Assert.Equal(1, subject.SubscribeCount);
            Assert.False(disconnected);

            var dis2 = refd.Subscribe();
            Assert.Equal(1, count);
            Assert.Equal(2, subject.SubscribeCount);
            Assert.False(disconnected);

            dis1.Dispose();
            Assert.False(disconnected);
            dis2.Dispose();
            Assert.True(disconnected);
            disconnected = false;

            var dis3 = refd.Subscribe();
            Assert.Equal(2, count);
            Assert.Equal(3, subject.SubscribeCount);
            Assert.False(disconnected);

            dis3.Dispose();
            Assert.True(disconnected);
        }

        [Fact]
        public void RefCount_OnError()
        {
            var ex = new Exception();
            var xs = Observable.Throw<int>(ex, Scheduler.Immediate);

            var res = xs.Publish().RefCount();

            res.Subscribe(_ => { Assert.True(false); }, ex_ => { Assert.Same(ex, ex_); }, () => { Assert.True(false); });
            res.Subscribe(_ => { Assert.True(false); }, ex_ => { Assert.Same(ex, ex_); }, () => { Assert.True(false); });
        }

        [Fact]
        public void RefCount_Publish()
        {
            var scheduler = new TestScheduler();

            var xs = scheduler.CreateHotObservable(
                OnNext(210, 1),
                OnNext(220, 2),
                OnNext(230, 3),
                OnNext(240, 4),
                OnNext(250, 5),
                OnNext(260, 6),
                OnNext(270, 7),
                OnNext(280, 8),
                OnNext(290, 9),
                OnCompleted<int>(300)
            );

            var res = xs.Publish().RefCount();

            var d1 = default(IDisposable);
            var o1 = scheduler.CreateObserver<int>();
            scheduler.ScheduleAbsolute(215, () => { d1 = res.Subscribe(o1); });
            scheduler.ScheduleAbsolute(235, () => { d1.Dispose(); });

            var d2 = default(IDisposable);
            var o2 = scheduler.CreateObserver<int>();
            scheduler.ScheduleAbsolute(225, () => { d2 = res.Subscribe(o2); });
            scheduler.ScheduleAbsolute(275, () => { d2.Dispose(); });

            var d3 = default(IDisposable);
            var o3 = scheduler.CreateObserver<int>();
            scheduler.ScheduleAbsolute(255, () => { d3 = res.Subscribe(o3); });
            scheduler.ScheduleAbsolute(265, () => { d3.Dispose(); });

            var d4 = default(IDisposable);
            var o4 = scheduler.CreateObserver<int>();
            scheduler.ScheduleAbsolute(285, () => { d4 = res.Subscribe(o4); });
            scheduler.ScheduleAbsolute(320, () => { d4.Dispose(); });

            scheduler.Start();

            o1.Messages.AssertEqual(
                OnNext(220, 2),
                OnNext(230, 3)
            );

            o2.Messages.AssertEqual(
                OnNext(230, 3),
                OnNext(240, 4),
                OnNext(250, 5),
                OnNext(260, 6),
                OnNext(270, 7)
            );

            o3.Messages.AssertEqual(
                OnNext(260, 6)
            );

            o4.Messages.AssertEqual(
                OnNext(290, 9),
                OnCompleted<int>(300)
            );

            xs.Subscriptions.AssertEqual(
                Subscribe(215, 275),
                Subscribe(285, 300)
            );
        }

        [Fact]
        public void RefCount_can_connect_again_if_previous_subscription_terminated_synchronously()
        {
            var seen = 0;
            var terminated = false;

            var subject = new ReplaySubject<Notification<int>>(1);
            var connectable = new DematerializingConnectableObservable<int>(subject.Publish());
            var refCount = connectable.RefCount();

            subject.OnNext(Notification.CreateOnNext(36));

            using (refCount.Subscribe(value => seen = value, () => terminated = true))
            {
                Assert.Equal(36, seen);
            }

            seen = 0;
            terminated = false;
            subject.OnNext(Notification.CreateOnCompleted<int>());

            using (refCount.Subscribe(value => seen = value, () => terminated = true))
            {
                Assert.Equal(0, seen);
                Assert.True(terminated);
            }

            seen = 0;
            terminated = false;
            subject.OnNext(Notification.CreateOnNext(36));

            using (refCount.Subscribe(value => seen = value, () => terminated = true))
            {
                Assert.Equal(36, seen);
                Assert.False(terminated);
            }
        }
        
        [Fact]
        public void LazyRefCount_ArgumentChecking()
        {
            ReactiveAssert.Throws<ArgumentNullException>(() => Observable.RefCount<int>(null, TimeSpan.FromSeconds(2)));
            ReactiveAssert.Throws<ArgumentNullException>(() => Observable.RefCount<int>(null, TimeSpan.FromSeconds(2), Scheduler.Default));
            ReactiveAssert.Throws<ArgumentNullException>(() => Observable.RefCount<int>(null, 2, TimeSpan.FromSeconds(2)));
            ReactiveAssert.Throws<ArgumentNullException>(() => Observable.RefCount<int>(null, 2, TimeSpan.FromSeconds(2)));
            ReactiveAssert.Throws<ArgumentNullException>(() => Observable.RefCount(Observable.Never<int>().Publish(), TimeSpan.FromSeconds(2), null));
            ReactiveAssert.Throws<ArgumentOutOfRangeException>(() => Observable.RefCount(Observable.Never<int>().Publish(), 0, TimeSpan.FromSeconds(2)));
            ReactiveAssert.Throws<ArgumentOutOfRangeException>(() => Observable.RefCount(Observable.Never<int>().Publish(), -1, TimeSpan.FromSeconds(2)));
            ReactiveAssert.Throws<ArgumentNullException>(() => Observable.RefCount(Observable.Never<int>().Publish(), 2, TimeSpan.FromSeconds(2), null));
            ReactiveAssert.Throws<ArgumentOutOfRangeException>(() => Observable.RefCount(Observable.Never<int>().Publish(), 0, TimeSpan.FromSeconds(2), Scheduler.Default));
            ReactiveAssert.Throws<ArgumentOutOfRangeException>(() => Observable.RefCount(Observable.Never<int>().Publish(), -1, TimeSpan.FromSeconds(2), Scheduler.Default));
        }

        [Fact]
        public void LazyRefCount_ConnectsOnFirst()
        {
            var scheduler = new TestScheduler();

            var xs = scheduler.CreateHotObservable(
                OnNext(210, 1),
                OnNext(220, 2),
                OnNext(230, 3),
                OnNext(240, 4),
                OnCompleted<int>(250)
            );

            var subject = new MySubject();

            var conn = new ConnectableObservable<int>(xs, subject);

            var res = scheduler.Start(() =>
                conn.RefCount(TimeSpan.FromSeconds(2))
            );

            res.Messages.AssertEqual(
                OnNext(210, 1),
                OnNext(220, 2),
                OnNext(230, 3),
                OnNext(240, 4),
                OnCompleted<int>(250)
            );

            Assert.True(subject.Disposed);
        }

        [Fact]
        public void LazyRefCount_NotConnected()
        {
            var scheduler = new TestScheduler();
            var disconnected = false;
            var count = 0;
            var xs = Observable.Defer(() =>
            {
                count++;
                return Observable.Create<int>(obs =>
                {
                    return () => { disconnected = true; };
                });
            });

            var subject = new MySubject();

            var conn = new ConnectableObservable<int>(xs, subject);
            var refd = conn.RefCount(TimeSpan.FromTicks(20), scheduler);

            var dis1 = refd.Subscribe();
            Assert.Equal(1, count);
            Assert.Equal(1, subject.SubscribeCount);
            Assert.False(disconnected);

            var dis2 = refd.Subscribe();
            Assert.Equal(1, count);
            Assert.Equal(2, subject.SubscribeCount);
            Assert.False(disconnected);

            dis1.Dispose();
            Assert.False(disconnected);
            dis2.Dispose();
            Assert.False(disconnected);

            scheduler.AdvanceBy(19);
            Assert.False(disconnected);

            scheduler.AdvanceBy(1);
            Assert.True(disconnected);
            disconnected = false;

            var dis3 = refd.Subscribe();
            Assert.Equal(2, count);
            Assert.Equal(3, subject.SubscribeCount);
            Assert.False(disconnected);

            dis3.Dispose();
            scheduler.AdvanceBy(20);
            Assert.True(disconnected);
        }

        [Fact]
        public void LazyRefCount_OnError()
        {
            var ex = new Exception();
            var xs = Observable.Throw<int>(ex, Scheduler.Immediate);

            var res = xs.Publish().RefCount(TimeSpan.FromSeconds(2));

            res.Subscribe(_ => throw new Exception(), ex_ => { Assert.Same(ex, ex_); }, () => throw new Exception());
            res.Subscribe(_ => throw new Exception(), ex_ => { Assert.Same(ex, ex_); }, () => throw new Exception());
        }

        [Fact]
        public void LazyRefCount_Publish()
        {
            var scheduler = new TestScheduler();

            var xs = scheduler.CreateHotObservable(
                OnNext(210, 1),
                OnNext(220, 2),
                OnNext(230, 3),
                OnNext(240, 4),
                OnNext(250, 5),
                OnNext(260, 6),
                OnNext(270, 7),
                OnNext(280, 8),
                OnNext(290, 9),
                OnCompleted<int>(300)
            );

            var res = xs.Publish().RefCount(TimeSpan.FromTicks(9), scheduler);

            var d1 = default(IDisposable);
            var o1 = scheduler.CreateObserver<int>();
            scheduler.ScheduleAbsolute(215, () => { d1 = res.Subscribe(o1); });
            scheduler.ScheduleAbsolute(235, () => { d1.Dispose(); });

            var d2 = default(IDisposable);
            var o2 = scheduler.CreateObserver<int>();
            scheduler.ScheduleAbsolute(225, () => { d2 = res.Subscribe(o2); });
            scheduler.ScheduleAbsolute(275, () =>
            {
                d2.Dispose();
            });

            var d3 = default(IDisposable);
            var o3 = scheduler.CreateObserver<int>();
            scheduler.ScheduleAbsolute(255, () => { d3 = res.Subscribe(o3); });
            scheduler.ScheduleAbsolute(265, () => { d3.Dispose(); });

            var d4 = default(IDisposable);
            var o4 = scheduler.CreateObserver<int>();
            scheduler.ScheduleAbsolute(285, () => { d4 = res.Subscribe(o4); });
            scheduler.ScheduleAbsolute(320, () => { d4.Dispose(); });

            scheduler.Start();

            o1.Messages.AssertEqual(
                OnNext(220, 2),
                OnNext(230, 3)
            );

            o2.Messages.AssertEqual(
                OnNext(230, 3),
                OnNext(240, 4),
                OnNext(250, 5),
                OnNext(260, 6),
                OnNext(270, 7)
            );

            o3.Messages.AssertEqual(
                OnNext(260, 6)
            );

            o4.Messages.AssertEqual(
                OnNext(290, 9),
                OnCompleted<int>(300)
            );

            xs.Subscriptions.AssertEqual(
                Subscribe(215, 284),
                Subscribe(285, 300)
            );
        }

        [Fact]
        public void RefCount_source_already_completed_synchronously()
        {
            var subscribed = 0;
            var unsubscribed = 0;
            
            var o1 = Observable.Create<string>(observer =>
            {
                subscribed++;
                observer.OnCompleted();

                return Disposable.Create(() => unsubscribed++);
            });

            var o2 = o1.Publish().RefCount();

            var s1 = o2.Subscribe();
            Assert.Equal(1, subscribed);
            Assert.Equal(1, unsubscribed);

            var s2 = o2.Subscribe();
            Assert.Equal(1, subscribed);
            Assert.Equal(1, unsubscribed);
        }

        [Fact]
        public void RefCount_minObservers_not_connected_Eager()
        {
            int connected = 0;
            var source = Observable.Defer(() =>
            {
                connected++;
                return Observable.Never<int>();
            })
            .Publish()
            .RefCount(2);

            Assert.Equal(0, connected);

            source.Subscribe();

            Assert.Equal(0, connected);
        }

        [Fact]
        public void RefCount_minObservers_connected_Eager()
        {
            var connected = 0;
            var source = Observable.Defer(() =>
            {
                connected++;
                return Observable.Range(1, 5);
            })
            .Publish()
            .RefCount(2);

            Assert.Equal(0, connected);

            var list1 = new List<int>();
            source.Subscribe(list1.Add);

            Assert.Equal(0, connected);
            Assert.Empty(list1);

            var list2 = new List<int>();
            source.Subscribe(list2.Add);

            Assert.Equal(1, connected);

            var expected = new List<int>(new[] { 1, 2, 3, 4, 5 });

            Assert.Equal(expected, list1);
            Assert.Equal(expected, list2);
        }

        [Fact]
        public void RefCount_minObservers_not_connected_Lazy()
        {
            int connected = 0;
            var source = Observable.Defer(() =>
            {
                connected++;
                return Observable.Never<int>();
            })
            .Publish()
            .RefCount(2, TimeSpan.FromMinutes(1));

            Assert.Equal(0, connected);

            source.Subscribe();

            Assert.Equal(0, connected);
        }

        [Fact]
        public void RefCount_minObservers_connected_Lazy()
        {
            var connected = 0;
            var source = Observable.Defer(() =>
            {
                connected++;
                return Observable.Range(1, 5);
            })
            .Publish()
            .RefCount(2, TimeSpan.FromMinutes(1));

            Assert.Equal(0, connected);

            var list1 = new List<int>();
            source.Subscribe(list1.Add);

            Assert.Equal(0, connected);
            Assert.Empty(list1);

            var list2 = new List<int>();
            source.Subscribe(list2.Add);

            Assert.Equal(1, connected);

            var expected = new List<int>(new[] { 1, 2, 3, 4, 5 });

            Assert.Equal(expected, list1);
            Assert.Equal(expected, list2);
        }
    }
}

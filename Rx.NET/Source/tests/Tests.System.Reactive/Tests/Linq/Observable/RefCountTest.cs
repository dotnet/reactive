// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information. 

using System;
using System.Collections.Generic;
using System.Reactive;
using System.Reactive.Concurrency;
using System.Reactive.Disposables;
using System.Reactive.Linq;
using System.Reactive.Subjects;
using Microsoft.Reactive.Testing;
using Microsoft.VisualStudio.TestTools.UnitTesting;

using Assert = Xunit.Assert;

namespace ReactiveTests.Tests
{
    [TestClass]
    public class RefCountTest : ReactiveTest
    {
        /// <summary>
        /// A connectable observable that provides an individual notification upon connection, where
        /// the notification can be different from one connection to the next.
        /// </summary>
        /// <typeparam name="T">Element type.</typeparam>
        /// <remarks>
        /// <para>
        /// The most important capability this provides is to be able to provide values after
        /// having completed. Obviously it won't do that for any single subscription because that
        /// would break the basic Rx contract, but this can deliver completion to some subscribers,
        /// and then go on to deliver values to subsequent subscribers. (The connectable
        /// observables returned by <c>Publish</c> can't do this: once their subject has delivered
        /// a completion notification it can't deliver anything else, not even to new subscribers.
        /// That's why we need a specialized type.)
        /// </para>
        /// </remarks>
        private sealed class SerialSingleNotificationConnectable<T> : IConnectableObservable<T>
        {
            private readonly object _gate = new();
            private Notification<T> _notificationAtNextConnect;
            private Subject<T> _sourceForNextConnect = new();
            private Connection _nextConnectionInProgress;

            public SerialSingleNotificationConnectable(Notification<T> initialNotificationAtNextConnect)
            {
                _notificationAtNextConnect = initialNotificationAtNextConnect;
                _nextConnectionInProgress = new(_sourceForNextConnect);
            }

            public List<Connection> Connections { get; } = new();

            private Connection ActiveConnection => (Connections.Count > 0 &&
                Connections[Connections.Count - 1] is Connection { Disposed: false } activeConnection)
                ? activeConnection : null;

            private Connection CurrentConnection => ActiveConnection ?? _nextConnectionInProgress;

            public void SetNotificationForNextConnect(Notification<T> notification)
            {
                _notificationAtNextConnect = notification;
            }

            public void DeliverNotificationForActiveConnection(Notification<T> notification)
            {
                if (ActiveConnection is not Connection activeConnection)
                {
                    throw new InvalidOperationException("No connection is currently active");
                }

                if (activeConnection.Source is not Subject<T> source)
                {
                    throw new InvalidOperationException("Active connection's source has been replaced and is no longer a Subject<T>, so it is not possible to deliver further notifications to current subscribers");
                }

                notification.Accept(source);
            }

            public IDisposable Connect()
            {
                Connection connecting;
                Notification<T> notification;
                Subject<T> source;
                lock (_gate)
                {
                    connecting = _nextConnectionInProgress;
                    notification = _notificationAtNextConnect;
                    source = _sourceForNextConnect;

                    _sourceForNextConnect = new Subject<T>();
                    _nextConnectionInProgress = new(_sourceForNextConnect);
                    Connections.Add(connecting);
                }

                notification.Accept(source);

                return connecting;
            }

            public IDisposable Subscribe(IObserver<T> observer)
            {
                Connection connection;
                lock (_gate)
                {
                    connection = CurrentConnection;
                }

                return connection.Source.Subscribe(observer);
            }

            public sealed class Connection(IObservable<T> source) : IDisposable
            {
                /// <summary>
                /// Gets a value indicating whether this connection has been disposed.
                /// </summary>
                public bool Disposed { get; private set; }

                public IObservable<T> Source { get; private set; } = source;

                /// <summary>
                /// In scenarios where <see cref="Source"/> has entered a completed state, this
                /// replaces it with a new source so if further subscribers to the same connection
                /// come along, tests can deliver notifications to those.
                /// </summary>
                /// <remarks>
                /// Without this method, <see cref="SerialSingleNotificationConnectable{T}"/> will
                /// deliver events only when <see cref="Connect"/> is called, meaning that only
                /// observers that subscribed before that call will receive any notifications
                /// (unless the notification was <c>OnComplete</c>, in which case the subject
                /// enters a completed state, and completes all further subscribers). This enables
                /// tests to create scenarios where subscriptions made after <c>Connect</c> (and
                /// before that connection is disposed) can receive further notifications.
                /// </remarks>
                public void ReplaceSource(IObservable<T> source)
                {
                    Source = source;
                }
                public void Dispose()
                {
                    Disposed = true;
                }
            }
        }

        /// <summary>
        /// A connectable observable that logs calls to <see cref="Connect"/> but otherwise ignores
        /// them, forwarding <see cref="Subscribe"/> calls to the current underlying source (which
        /// can be changed over time).
        /// </summary>
        /// <typeparam name="T">Element type.</typeparam>
        /// <remarks>
        /// <para>
        /// This is similar to <see cref="SerialSingleNotificationConnectable{T}"/>, in that the
        /// underlying source can be changed over time, making it possible for this to complete
        /// observers, but then revert to a state where subsequent observers will not be completed.
        /// But this also enables simulation of unusual (but not strictly disallowed) behaviour,
        /// in which subscribers will receive notifications before calling <see cref="Connect"/>.
        /// It's useful to be able to do this because it can happen in more normal setups when
        /// sources completed synchronously, and it's easy to handle this incorrectly.
        /// </para>
        /// </remarks>
        private sealed class SerialConnectableIgnoringConnect<T> : IConnectableObservable<T>
        {
            private IObservable<T> _source;

            public SerialConnectableIgnoringConnect(IObservable<T> initialSource)
            {
                _source = initialSource;
            }

            public void SetSource(IObservable<T> source)
            {
                _source = source;
            }

            public List<Connection> Connections { get; } = new();

            public IDisposable Connect()
            {
                var connection = new Connection();
                Connections.Add(connection);
                return connection;
            }

            public IDisposable Subscribe(IObserver<T> observer)
            {
                return _source.Subscribe(observer);
            }

            public sealed class Connection() : IDisposable
            {
                /// <summary>
                /// Gets a value indicating whether this connection has been disposed.
                /// </summary>
                public bool Disposed { get; private set; }

                public void Dispose()
                {
                    Disposed = true;
                }
            }
        }

        #region Immediate Disconnect

        [TestMethod]
        public void RefCount_NoDelay_ArgumentChecking()
        {
            ReactiveAssert.Throws<ArgumentNullException>(() => Observable.RefCount<int>(null));
            ReactiveAssert.Throws<ArgumentNullException>(() => Observable.RefCount<int>(null, 2));
            ReactiveAssert.Throws<ArgumentOutOfRangeException>(() => Observable.RefCount(Observable.Never<int>().Publish(), 0));
            ReactiveAssert.Throws<ArgumentOutOfRangeException>(() => Observable.RefCount(Observable.Never<int>().Publish(), -1));
            ReactiveAssert.Throws<ArgumentOutOfRangeException>(() => Observable.RefCount(Observable.Never<int>().Publish(), -2));
        }

        [TestMethod]
        public void RefCount_NoDelay_ConnectsOnFirst()
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

        [TestMethod]
        public void RefCount_NoDelay_minObservers_ConnectsOnObserverThresholdReached()
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
            var res = conn.RefCount(2);

            var d1 = default(IDisposable);
            var o1 = scheduler.CreateObserver<int>();
            scheduler.ScheduleAbsolute(210, () => { d1 = res.Subscribe(o1); });

            var d2 = default(IDisposable);
            var o2 = scheduler.CreateObserver<int>();
            scheduler.ScheduleAbsolute(225, () => { d2 = res.Subscribe(o2); });

            scheduler.Start();

            o1.Messages.AssertEqual(
                OnNext(230, 3),
                OnNext(240, 4),
                OnCompleted<int>(250)
            );

            Assert.True(subject.Disposed);
        }

        [TestMethod]
        public void RefCount_NoDelay_SourceProducesValuesAndCompletesInSubscribe()
        {
            var connected = 0;
            var source = Observable.Defer(() =>
            {
                connected++;
                return Observable.Range(1, 5);
            })
            .Publish()
            .RefCount();

            Assert.Equal(0, connected);

            var list1 = new List<int>();
            source.Subscribe(list1.Add);

            Assert.Equal(1, connected);

            List<int> expected1 = [1, 2, 3, 4, 5];

            Assert.Equal(expected1, list1);

            var list2 = new List<int>();
            source.Subscribe(list2.Add);

            Assert.Equal(1, connected);
            Assert.Empty(list2);
        }

        [TestMethod]
        public void RefCount_NoDelay_minObservers_SourceProducesValuesAndCompletesInSubscribe()
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

            List<int> expected = [1, 2, 3, 4, 5];

            Assert.Equal(expected, list1);
            Assert.Equal(expected, list2);
        }

        [TestMethod]
        public void RefCount_NoDelay_SourceCompletesWithNoValuesInSubscribe()
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

        [TestMethod]
        public void RefCount_NoDelay_minObservers_SourceCompletesWithNoValuesInSubscribe()
        {
            var subscribed = 0;
            var unsubscribed = 0;

            var o1 = Observable.Create<string>(observer =>
            {
                subscribed++;
                observer.OnCompleted();

                return Disposable.Create(() => unsubscribed++);
            });

            var o2 = o1.Publish().RefCount(2);

            var s1 = o2.Subscribe();
            Assert.Equal(0, subscribed);
            Assert.Equal(0, unsubscribed);

            var s2 = o2.Subscribe();
            Assert.Equal(1, subscribed);
            Assert.Equal(1, unsubscribed);

            s1.Dispose();
            s2.Dispose();

            // At this point, the RefCount has 0 subscribers, and will have disconnected from
            // its source. When we add a new subscriber, the count will be at 0, which is below
            // minObservers, so we don't expect a new connection. RefCount _will_ call Subscribe
            // on its source, but that source is the Subject created by Publish(). And since
            // o1 already delivered an OnComplete, that Subject is now in a completed state, so
            // it will immediately complete any further subscriptions. RefCount sees this, so
            // although the connection count briefly goes up to 1, it will then go back down to
            // 0 before this call to Subscribe returns.
            // Basically, because this test uses o1.Publish(), once our connectable source source
            // completes is it incapable of restarting. That's why we have other tests that use
            // SerialSingleNotificationConnectable - that enables us to build a source that resets
            var s3 = o2.Subscribe();
            Assert.Equal(1, subscribed);
            Assert.Equal(1, unsubscribed);

            // While it might look like adding a second subscriber should tip us back over the threshold
            // and trigger a reconnect, for the reasons described above o2 immdiately completed in the
            // last call to subscribe, so the RefCount is zero at this point. This is a limitation of
            // Publish(). It doesn't really matter for this test, but it's why some tests use
            // SerialSingleNotificationConnectable.
            var s4 = o2.Subscribe();
            Assert.Equal(1, subscribed);
            Assert.Equal(1, unsubscribed);
        }

        [TestMethod]
        public void RefCount_NoDelay_NotConnected()
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

        [TestMethod]
        public void RefCount_NoDelay_minObservers_NotConnected()
        {
            var connected = 0;
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

        [TestMethod]
        public void RefCount_NoDelay_OnError()
        {
            var ex = new Exception();
            var xs = Observable.Throw<int>(ex, Scheduler.Immediate);

            var res = xs.Publish().RefCount();

            res.Subscribe(_ => { Assert.True(false); }, ex_ => { Assert.Same(ex, ex_); }, () => { Assert.True(false); });
            res.Subscribe(_ => { Assert.True(false); }, ex_ => { Assert.Same(ex, ex_); }, () => { Assert.True(false); });
        }

        [TestMethod]
        public void RefCount_NoDelay_minObservers_OnError()
        {
            var ex = new Exception();
            var xs = Observable.Throw<int>(ex, Scheduler.Immediate);

            var res = xs.Publish().RefCount(2);

            var exceptionsReceived = new List<Exception>();
            void AddSubscriber()
            {
                res.Subscribe(
                    _ => { Assert.Fail("OnNext unexpected"); },
                    ex_ => { exceptionsReceived.Add(ex); },
                    () => { Assert.Fail("OnComplete unexpected"); });
            }

            AddSubscriber();
            Assert.Equal(0, exceptionsReceived.Count);

            AddSubscriber();
            Assert.Equal(2, exceptionsReceived.Count);
            Assert.Same(ex, exceptionsReceived[0]);
            Assert.Same(ex, exceptionsReceived[1]);
        }

        [TestMethod]
        public void RefCount_NoDelay_HotSourceMultipleSubscribers()
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

        [TestMethod]
        public void RefCount_NoDelay_minObservers_HotSourceMultipleSubscribers()
        {
            var scheduler = new TestScheduler();

            var xs = scheduler.CreateHotObservable(
                OnNext(210, 1), // 0 subscribers
                OnNext(220, 2), // 1 subscriber
                OnNext(230, 3), // 2 subscribers
                OnNext(240, 4), // 1 subscriber
                OnNext(250, 5), // 1 subscriber
                OnNext(260, 6), // 2 subscribers
                OnNext(270, 7), // 1 subscribers
                OnNext(280, 8), // 0 subscribers
                OnNext(290, 9), // 1 subscribers
                OnNext(300, 10), // 2 subscribers
                OnCompleted<int>(310)
            );

            var res = xs.Publish().RefCount(2);

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

            var d5 = default(IDisposable);
            var o5 = scheduler.CreateObserver<int>();
            scheduler.ScheduleAbsolute(295, () => { d5 = res.Subscribe(o5); });
            scheduler.ScheduleAbsolute(320, () => { d5.Dispose(); });

            scheduler.Start();

            o1.Messages.AssertEqual(
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
                OnNext(300, 10),
                OnCompleted<int>(310)
            );

            o5.Messages.AssertEqual(
                OnNext(300, 10),
                OnCompleted<int>(310)
            );

            xs.Subscriptions.AssertEqual(
                Subscribe(225, 275),
                Subscribe(295, 310)
            );
        }

        [TestMethod]
        public void RefCount_NoDelay_minObservers_SubscriptionsDropBelowThresholdButNotToZero()
        {
            var subject = new ReplaySubject<int>(5);
            var connected = 0;
            var source = Observable.Defer(() =>
                {
                    connected++;
                    return subject;
                })
                .Publish().RefCount(2);

            subject.OnNext(1);

            Assert.Equal(0, connected);

            var list1 = new List<int>();
            var sub1 = source.Subscribe(list1.Add);
            Assert.Equal(0, connected);
            Assert.Empty(list1);

            subject.OnNext(2);

            var list2 = new List<int>();
            var sub2 = source.Subscribe(list2.Add);

            // Since connection only occurred with the 2nd subscriber, we expect both to get everything
            // the ReplaySubject has stored.
            List<int> expectedSub1 = [1, 2];
            var expectedSub2 = expectedSub1;

            Assert.Equal(expectedSub1, list1);
            Assert.Equal(expectedSub1, list2);
            Assert.Equal(1, connected);

            subject.OnNext(3);

            // Both subscribers should have received the new item.
            expectedSub1 = expectedSub2 = [1, 2, 3];
            Assert.Equal(expectedSub1, list1);
            Assert.Equal(expectedSub2, list2);
            Assert.Equal(1, connected);

            var list3 = new List<int>();
            source.Subscribe(list3.Add);

            // Since we were already connected, the 3rd subscriber just gets added to the observers of
            // the Publish multicast output, and no new connection should occur to the underlying ReplaySubject.
            // So for this 3rd subscription, no new items should be received by any of the subscribers
            List<int> expectedSub3 = [];
            Assert.Equal(expectedSub1, list1);
            Assert.Equal(expectedSub2, list2);
            Assert.Equal(expectedSub3, list3);
            Assert.Equal(1, connected);

            subject.OnNext(4);

            // All the current subscribers should have received that latest item.
            expectedSub1 = expectedSub2 = [1, 2, 3, 4];
            expectedSub3 = [4];
            Assert.Equal(expectedSub1, list1);
            Assert.Equal(expectedSub2, list2);
            Assert.Equal(expectedSub3, list3);
            Assert.Equal(1, connected);

            sub1.Dispose();

            subject.OnNext(5);

            // The two remaining subscribers should have received that new item, but the one that just
            // unsubscribed should not.
            expectedSub1 = [1, 2, 3, 4];
            expectedSub2 = [1, 2, 3, 4, 5];
            expectedSub3 = [4, 5];
            Assert.Equal(expectedSub1, list1);
            Assert.Equal(expectedSub2, list2);
            Assert.Equal(expectedSub3, list3);
            Assert.Equal(1, connected);

            sub2.Dispose();

            subject.OnNext(6);

            // We are now below the minObservers threshold of 2, but that threshold only governs when we move
            // from a disconnected state to a connected state. We should remain connected as long as there is
            // at least one subscriber, so we expect the remaining subscriber to receive that last item.
            expectedSub1 = [1, 2, 3, 4];
            expectedSub2 = [1, 2, 3, 4, 5];
            expectedSub3 = [4, 5, 6];
            Assert.Equal(expectedSub1, list1);
            Assert.Equal(expectedSub2, list2);
            Assert.Equal(expectedSub3, list3);
            Assert.Equal(1, connected);
        }

        [TestMethod]
        public void RefCount_NoDelay_SubscriptionsDropBelowThresholdAndThenBackAbove()
        {
            var sourceAfterInitial = new Subject<int>();
            var connected = 0;
            var source = Observable.Defer(() =>
            {
                connected++;
                return Observable.Range(1, 5).Concat(sourceAfterInitial);
            })
            .Publish()
            .RefCount(2);


            Assert.Equal(0, connected);

            var list1 = new List<int>();
            var sub1 = source.Subscribe(list1.Add);

            Assert.Equal(0, connected);
            Assert.Empty(list1);

            var list2 = new List<int>();
            var sub2 = source.Subscribe(list2.Add);

            Assert.Equal(1, connected);

            sourceAfterInitial.OnNext(6);

            sub1.Dispose();
            sourceAfterInitial.OnNext(7);

            Assert.Equal(1, connected);

            var list3 = new List<int>();
            var sub3 = source.Subscribe(list3.Add);

            // This is the distinguishing feature of this test. With that last subscription, we went from 1
            // subscriber (below minObservers) but still connected (because we already hit minObservers once
            // and never dropped to zero), and now we're passing through minObservers again. We used to have
            // a bug where we would erroneously attempt to reconnect at this point.
            Assert.Equal(1, connected);
            sourceAfterInitial.OnNext(8);

            var expectedSub1 = new List<int>([1, 2, 3, 4, 5, 6]);
            var expectedSub2 = new List<int>([1, 2, 3, 4, 5, 6, 7, 8]);
            var expectedSub3 = new List<int>([8]);

            Assert.Equal(expectedSub1, list1);
            Assert.Equal(expectedSub2, list2);
            Assert.Equal(expectedSub3, list3);
        }

        [TestMethod]
        public void RefCount_NoDelay_ValuesDuringAndAfterSubscribe()
        {
            var subject = new ReplaySubject<int>(5);
            var source = subject.Publish().RefCount();

            subject.OnNext(1);

            // Although the source is a ReplaySubject, the use of Publish means there will only be
            // a single subscription to the ReplaySubject, so it will only replay one. (It will replay
            // that first value on the initial connect.) So we expect each subscriber to see fewer and
            // fewer values.
            // all subscribers will see all the values
            List<int> expected1 = [1];

            var list1 = new List<int>();
            source.Subscribe(list1.Add);
            Assert.Equal(expected1, list1);

            subject.OnNext(2);

            var list2 = new List<int>();
            source.Subscribe(list2.Add);

            expected1 = [1, 2];
            List<int> expected2 = [];

            Assert.Equal(expected1, list1);
            Assert.Equal(expected2, list2);

            subject.OnNext(3);

            var list3 = new List<int>();
            source.Subscribe(list3.Add);

            expected1 = [1, 2, 3];
            expected2 = [3];
            List<int> expected3 = [];

            Assert.Equal(expected1, list1);
            Assert.Equal(expected2, list2);
            Assert.Equal(expected3, list3);

            subject.OnNext(4);
            expected1 = [1, 2, 3, 4];
            expected2 = [3, 4];
            expected3 = [4];
            Assert.Equal(expected1, list1);
            Assert.Equal(expected2, list2);
            Assert.Equal(expected3, list3);
        }

        [TestMethod]
        public void RefCount_NoDelay_minObservers_ValuesDuringAndAfterSubscribe()
        {
            var subject = new ReplaySubject<int>(5);
            var source = subject.Publish().RefCount(2);

            subject.OnNext(1);

            var list1 = new List<int>();
            source.Subscribe(list1.Add);
            Assert.Empty(list1);

            subject.OnNext(2);

            List<int> expected1and2 = [1, 2];

            var list2 = new List<int>();
            source.Subscribe(list2.Add);

            Assert.Equal(expected1and2, list1);
            Assert.Equal(expected1and2, list2);

            subject.OnNext(3);
            expected1and2 = [1, 2, 3];
            Assert.Equal(expected1and2, list1);
            Assert.Equal(expected1and2, list2);

            var list3 = new List<int>();
            source.Subscribe(list3.Add);

            List<int> expected3 = [];
            Assert.Equal(expected1and2, list1);
            Assert.Equal(expected1and2, list2);
            Assert.Equal(expected3, list3);

            subject.OnNext(4);
            expected1and2 = [1, 2, 3, 4];
            expected3 = [4];
            Assert.Equal(expected1and2, list1);
            Assert.Equal(expected1and2, list2);
            Assert.Equal(expected3, list3);
        }

        [TestMethod]
        public void RefCount_NoDelay_CanConnectAgainIfPreviousSubscriptionTerminatedFromSubscribeByCompletion()
        {
            var seen = 0;
            var terminated = false;

            // On initial subscription, the source will produce one value and will not complete.
            var connectable = new SerialSingleNotificationConnectable<int>(Notification.CreateOnNext(36));
            var refCount = connectable.RefCount();

            using (refCount.Subscribe(value => seen = value, () => terminated = true))
            {
                Assert.Equal(36, seen);
            }

            seen = 0;
            terminated = false;

            // This time around, the source will complete when subscribed to.
            connectable.SetNotificationForNextConnect(Notification.CreateOnCompleted<int>());

            using (refCount.Subscribe(value => seen = value, () => terminated = true))
            {
                Assert.Equal(0, seen);
                Assert.True(terminated);
            }

            seen = 0;
            terminated = false;

            // Now we go back to the initial behaviour in which the source produces one value and does not complete.
            connectable.SetNotificationForNextConnect(Notification.CreateOnNext(42));

            using (refCount.Subscribe(value => seen = value, () => terminated = true))
            {
                Assert.Equal(42, seen);
                Assert.False(terminated);
            }
        }

        [TestMethod]
        public void RefCount_NoDelay_minObservers_CanConnectAgainIfPreviousSubscriptionTerminatedFromSubscribeByCompletion()
        {
            var seen1 = 0;
            var seen2 = 0;
            var terminated1 = false;
            var terminated2 = false;

            // On initial subscription, the source will produce one value and will not complete.
            var connectable = new SerialSingleNotificationConnectable<int>(Notification.CreateOnNext(36));
            var refCount = connectable.RefCount(2);

            using (refCount.Subscribe(value => seen1 = value, () => terminated1 = true))
            {
                Assert.Equal(0, seen1);
                using (refCount.Subscribe(value => seen2 = value, () => terminated2 = true))
                {
                    Assert.Equal(36, seen1);
                    Assert.Equal(36, seen2);
                }
            }

            seen1 = seen2 = 0;
            terminated1 = terminated2 = false;

            // This time around, the source will complete when subscribed to.
            connectable.SetNotificationForNextConnect(Notification.CreateOnCompleted<int>());

            using (refCount.Subscribe(value => seen1 = value, () => terminated1 = true))
            {
                Assert.False(terminated1);
                Assert.False(terminated2);
                using (refCount.Subscribe(value => seen2 = value, () => terminated2 = true))
                {
                    Assert.Equal(0, seen1);
                    Assert.Equal(0, seen2);
                    Assert.True(terminated1);
                    Assert.True(terminated2);
                }
            }

            seen1 = seen2 = 0;
            terminated1 = terminated2 = false;

            // Now we go back to the initial behaviour in which the source produces one value and does not complete.
            connectable.SetNotificationForNextConnect(Notification.CreateOnNext(42));

            using (refCount.Subscribe(value => seen1 = value, () => terminated1 = true))
            {
                Assert.Equal(0, seen1);
                using (refCount.Subscribe(value => seen2 = value, () => terminated2 = true))
                {
                    Assert.Equal(42, seen1);
                    Assert.Equal(42, seen2);
                    Assert.False(terminated1);
                    Assert.False(terminated2);
                }
            }
        }

        #endregion

        #region Delayed Disconnect

        [TestMethod]
        public void RefCount_DelayedDisconnect_ArgumentChecking()
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

        [TestMethod]
        public void RefCount_DelayedDisconnect_ConnectsOnFirst()
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

        [TestMethod]
        public void RefCount_DelayedDisconnect_minObservers_ConnectsOnObserverThresholdReached()
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
            var res = conn.RefCount(2, TimeSpan.FromTicks(300));

            var d1 = default(IDisposable);
            var o1 = scheduler.CreateObserver<int>();
            scheduler.ScheduleAbsolute(210, () => { d1 = res.Subscribe(o1); });

            var d2 = default(IDisposable);
            var o2 = scheduler.CreateObserver<int>();
            scheduler.ScheduleAbsolute(225, () => { d2 = res.Subscribe(o2); });

            scheduler.Start();

            o1.Messages.AssertEqual(
                OnNext(230, 3),
                OnNext(240, 4),
                OnCompleted<int>(250)
            );

            Assert.True(subject.Disposed);
        }

        [TestMethod]
        public void RefCount_DelayedDisconnect_minObservers_SourceProducesValuesAndCompletesInSubscribe()
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

            var expected = new List<int>([1, 2, 3, 4, 5]);

            Assert.Equal(expected, list1);
            Assert.Equal(expected, list2);
        }

        [TestMethod]
        public void RefCount_DelayedDisconnect_SourceCompletesWithNoValuesInSubscribe()
        {
            var subscribed = 0;
            var unsubscribed = 0;

            var o1 = Observable.Create<string>(observer =>
            {
                subscribed++;
                observer.OnCompleted();

                return Disposable.Create(() => unsubscribed++);
            });

            var o2 = o1.Publish().RefCount(TimeSpan.FromSeconds(20));

            var s1 = o2.Subscribe();
            Assert.Equal(1, subscribed);
            Assert.Equal(1, unsubscribed);

            var s2 = o2.Subscribe();
            Assert.Equal(1, subscribed);
            Assert.Equal(1, unsubscribed);
        }

        [TestMethod]
        public void RefCount_DelayedDisconnect_minObservers_SourceCompletesWithNoValuesInSubscribe()
        {
            var scheduler = new TestScheduler();
            var subscribed = 0;
            var unsubscribed = 0;

            var o1 = Observable.Create<string>(observer =>
            {
                subscribed++;
                observer.OnCompleted();

                return Disposable.Create(() => unsubscribed++);
            });

            var o2 = o1.Publish().RefCount(2, TimeSpan.FromTicks(10), scheduler);

            var s1 = o2.Subscribe();
            Assert.Equal(0, subscribed);
            Assert.Equal(0, unsubscribed);

            // Note that although we've got a delayed disconnect, we don't need to call AdvanceBy
            // here because the source itself completes. The disconnect is triggered by the source,
            // not the RefCount in this test.
            var s2 = o2.Subscribe();
            Assert.Equal(1, subscribed);
            Assert.Equal(1, unsubscribed);

            s1.Dispose();
            s2.Dispose();

            // At this point, the RefCount has 0 subscribers, and will have disconnected from
            // its source. When we add a new subscriber, the count will be at 0, which is below
            // minObservers, so we don't expect a new connection. RefCount _will_ call Subscribe
            // on its source, but that source is the Subject created by Publish(). And since
            // o1 already delivered an OnComplete, that Subject is now in a completed state, so
            // it will immediately complete any further subscriptions. RefCount sees this, so
            // although the connection count briefly goes up to 1, it will then go back down to
            // 0 before this call to Subscribe returns.
            // Basically, because this test uses o1.Publish(), once our connectable source source
            // completes is it incapable of restarting. That's why we have other tests that use
            // SerialSingleNotificationConnectable - that enables us to build a source that resets
            var s3 = o2.Subscribe();
            Assert.Equal(1, subscribed);
            Assert.Equal(1, unsubscribed);

            // While it might look like adding a second subscriber should tip us back over the threshold
            // and trigger a reconnect, for the reasons described above o2 immediately completed in the
            // last call to subscribe, so the RefCount is zero at this point. This is a limitation of
            // Publish(). It doesn't really matter for this test, but it's why some tests use
            // SerialSingleNotificationConnectable.
            var s4 = o2.Subscribe();
            Assert.Equal(1, subscribed);
            Assert.Equal(1, unsubscribed);
        }

        [TestMethod]
        public void RefCount_DelayedDisconnect_NotConnected()
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

        [TestMethod]
        public void RefCount_DelayedDisconnect_minObservers_NotConnected()
        {
            var connected = 0;
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

        [TestMethod]
        public void RefCount_DelayedDisconnect_OnError()
        {
            var ex = new Exception();
            var xs = Observable.Throw<int>(ex, Scheduler.Immediate);

            var res = xs.Publish().RefCount(TimeSpan.FromSeconds(2));

            res.Subscribe(_ => throw new Exception(), ex_ => { Assert.Same(ex, ex_); }, () => throw new Exception());
            res.Subscribe(_ => throw new Exception(), ex_ => { Assert.Same(ex, ex_); }, () => throw new Exception());
        }

        [TestMethod]
        public void RefCount_DelayedDisconnect_minObservers_OnError()
        {
            var ex = new Exception();
            var xs = Observable.Throw<int>(ex, Scheduler.Immediate);

            var res = xs.Publish().RefCount(2, TimeSpan.FromSeconds(200));

            var exceptionsReceived = new List<Exception>();
            void AddSubscriber()
            {
                res.Subscribe(
                    _ => { Assert.Fail("OnNext unexpected"); },
                    ex_ => { exceptionsReceived.Add(ex); },
                    () => { Assert.Fail("OnComplete unexpected"); });
            }

            AddSubscriber();
            Assert.Equal(0, exceptionsReceived.Count);

            AddSubscriber();
            Assert.Equal(2, exceptionsReceived.Count);
            Assert.Same(ex, exceptionsReceived[0]);
            Assert.Same(ex, exceptionsReceived[1]);
        }

        [TestMethod]
        public void RefCount_DelayedDisconnect_HotSourceMultipleSubscribers()
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

        [TestMethod]
        public void RefCount_DelayedDisconnect_minObservers_HotSourceMultipleSubscribers()
        {
            var scheduler = new TestScheduler();

            var xs = scheduler.CreateHotObservable(
                OnNext(210, 1), // 0 subscribers
                OnNext(220, 2), // 1 subscriber
                OnNext(230, 3), // 2 subscribers
                OnNext(240, 4), // 1 subscriber
                OnNext(250, 5), // 1 subscriber
                OnNext(260, 6), // 2 subscribers
                OnNext(270, 7), // 1 subscribers
                OnNext(280, 8), // 0 subscribers
                OnNext(290, 9), // 1 subscribers
                OnNext(300, 10), // 2 subscribers
                OnCompleted<int>(310)
            );

            var res = xs.Publish().RefCount(2, TimeSpan.FromTicks(9), scheduler);

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

            var d5 = default(IDisposable);
            var o5 = scheduler.CreateObserver<int>();
            scheduler.ScheduleAbsolute(295, () => { d5 = res.Subscribe(o5); });
            scheduler.ScheduleAbsolute(320, () => { d5.Dispose(); });

            scheduler.Start();

            o1.Messages.AssertEqual(
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
                OnNext(300, 10),
                OnCompleted<int>(310)
            );

            o5.Messages.AssertEqual(
                OnNext(300, 10),
                OnCompleted<int>(310)
            );

            xs.Subscriptions.AssertEqual(
                Subscribe(225, 284),
                Subscribe(295, 310)
            );
        }

        [TestMethod]
        public void RefCount_DelayedDisconnect_minObservers_SubscriptionsDropBelowThresholdButNotToZero()
        {
            var subject = new ReplaySubject<int>(5);
            var connected = 0;
            var source = Observable.Defer(() =>
            {
                connected++;
                return subject;
            })
            .Publish()
            .RefCount(2, TimeSpan.FromMinutes(1));

            subject.OnNext(1);

            Assert.Equal(0, connected);

            var list1 = new List<int>();
            var sub1 = source.Subscribe(list1.Add);
            Assert.Equal(0, connected);
            Assert.Empty(list1);

            subject.OnNext(2);

            var list2 = new List<int>();
            var sub2 = source.Subscribe(list2.Add);

            // Since connection only occurred with the 2nd subscriber, we expect both to get everything
            // the ReplaySubject has stored.
            List<int> expectedSub1 = [1, 2];
            var expectedSub2 = expectedSub1;

            Assert.Equal(expectedSub1, list1);
            Assert.Equal(expectedSub1, list2);
            Assert.Equal(1, connected);

            subject.OnNext(3);

            // Both subscribers should have received the new item.
            expectedSub1 = expectedSub2 = [1, 2, 3];
            Assert.Equal(expectedSub1, list1);
            Assert.Equal(expectedSub2, list2);
            Assert.Equal(1, connected);

            var list3 = new List<int>();
            source.Subscribe(list3.Add);

            // Since we were already connected, the 3rd subscriber just gets added to the observers of
            // the Publish multicast output, and no new connection should occur to the underlying ReplaySubject.
            // So for this 3rd subscription, no new items should be received by any of the subscribers
            List<int> expectedSub3 = [];
            Assert.Equal(expectedSub1, list1);
            Assert.Equal(expectedSub2, list2);
            Assert.Equal(expectedSub3, list3);
            Assert.Equal(1, connected);

            subject.OnNext(4);

            // All the current subscribers should have received that latest item.
            expectedSub1 = expectedSub2 = [1, 2, 3, 4];
            expectedSub3 = [4];
            Assert.Equal(expectedSub1, list1);
            Assert.Equal(expectedSub2, list2);
            Assert.Equal(expectedSub3, list3);
            Assert.Equal(1, connected);

            sub1.Dispose();

            subject.OnNext(5);

            // The two remaining subscribers should have received that new item, but the one that just
            // unsubscribed should not.
            expectedSub1 = [1, 2, 3, 4];
            expectedSub2 = [1, 2, 3, 4, 5];
            expectedSub3 = [4, 5];
            Assert.Equal(expectedSub1, list1);
            Assert.Equal(expectedSub2, list2);
            Assert.Equal(expectedSub3, list3);
            Assert.Equal(1, connected);

            sub2.Dispose();

            subject.OnNext(6);

            // We are now below the minObservers threshold of 2, but that threshold only governs when we move
            // from a disconnected state to a connected state. We should remain connected as long as there is
            // at least one subscriber, so we expect the remaining subscriber to receive that last item.
            expectedSub1 = [1, 2, 3, 4];
            expectedSub2 = [1, 2, 3, 4, 5];
            expectedSub3 = [4, 5, 6];
            Assert.Equal(expectedSub1, list1);
            Assert.Equal(expectedSub2, list2);
            Assert.Equal(expectedSub3, list3);
            Assert.Equal(1, connected);
        }

        [TestMethod]
        public void RefCount_DelayedDisconnect_SubscriptionsDropBelowThresholdAndThenBackAbove()
        {
            var scheduler = new TestScheduler();

            var sourceAfterInitial = new Subject<int>();
            var connected = 0;
            var source = Observable.Defer(() =>
            {
                connected++;
                return Observable.Range(1, 5).Concat(sourceAfterInitial);
            })
            .Publish()
            .RefCount(2, TimeSpan.FromTicks(10), scheduler);


            Assert.Equal(0, connected);

            var list1 = new List<int>();
            var sub1 = source.Subscribe(list1.Add); // 1 subscriber

            Assert.Equal(0, connected);
            Assert.Empty(list1);

            var list2 = new List<int>();
            var sub2 = source.Subscribe(list2.Add); // 2 subscribers

            Assert.Equal(1, connected);

            sourceAfterInitial.OnNext(6);

            sub1.Dispose(); // 1 subscriber

            // We don't expect a disconnect, but provide enough time for one to occur, should that bug ever creep in
            scheduler.AdvanceBy(10);
            Assert.Equal(1, connected);

            sourceAfterInitial.OnNext(7);

            Assert.Equal(1, connected);

            var list3 = new List<int>();
            var sub3 = source.Subscribe(list3.Add);

            // This is the distinguishing feature of this test. With that last subscription, we went from 1
            // subscriber (below minObservers) but still connected (because we already hit minObservers once
            // and never dropped to zero), and now we're passing through minObservers again. We used to have
            // a bug where we would erroneously attempt to reconnect at this point.
            Assert.Equal(1, connected);
            sourceAfterInitial.OnNext(8);

            var expectedSub1 = new List<int>([1, 2, 3, 4, 5, 6]);
            var expectedSub2 = new List<int>([1, 2, 3, 4, 5, 6, 7, 8]);
            var expectedSub3 = new List<int>([8]);

            Assert.Equal(expectedSub1, list1);
            Assert.Equal(expectedSub2, list2);
            Assert.Equal(expectedSub3, list3);
        }

        [TestMethod]
        public void RefCount_DelayedDisconnect_SubscriptionsDropToZeroThenNewSubscriptionArrivesBeforeDisconnectDelay()
        {
            var scheduler = new TestScheduler();
            var source = new SerialSingleNotificationConnectable<int>(Notification.CreateOnNext(1));
            var rco = source.RefCount(TimeSpan.FromTicks(10), scheduler);

            var s1 = rco.Subscribe();
            s1.Dispose();

            // There are now 0 subscribers, but the time for the disconnect has not yet come.
            Assert.Equal(1, source.Connections.Count);
            Assert.False(source.Connections[0].Disposed);

            scheduler.AdvanceBy(9);

            // The time has still not come,
            Assert.Equal(1, source.Connections.Count);
            Assert.False(source.Connections[0].Disposed);

            // Since we were still connected, this should move the connection from a 'waiting to
            // shut down' state into an active state.
            var seen = 0;
            var terminated = false;
            var s2 = rco.Subscribe(x => seen = x, () => terminated = true);
            source.DeliverNotificationForActiveConnection(Notification.CreateOnNext(2));
            Assert.Equal(2, seen);
            Assert.False(terminated);
            Assert.False(source.Connections[0].Disposed);

            // This moves us past the time when `RefCount` would have shut down the connection if no new
            // subscriptions had turned up.
            scheduler.AdvanceBy(2);

            Assert.False(terminated);
            Assert.False(source.Connections[0].Disposed);

            // We should be able to advance well beyond the disconnect delay because we have an active
            // subscriber.
            scheduler.AdvanceBy(20);

            Assert.False(terminated);
            Assert.False(source.Connections[0].Disposed);
        }

        [TestMethod]
        public void RefCount_DelayedDisconnect_minObservers_SubscriptionsDropToZeroThenNewSubscriptionArrivesBeforeDisconnectDelay()
        {
            var scheduler = new TestScheduler();
            var source = new SerialSingleNotificationConnectable<int>(Notification.CreateOnNext(1));
            var rco = source.RefCount(2, TimeSpan.FromTicks(10), scheduler);

            var s1 = rco.Subscribe();
            var s2 = rco.Subscribe();
            s1.Dispose();
            s2.Dispose();

            // There are now 0 subscribers, but the time for the disconnect has not yet come.
            Assert.Equal(1, source.Connections.Count);
            Assert.False(source.Connections[0].Disposed);

            scheduler.AdvanceBy(9);

            // The time has still not come,
            Assert.Equal(1, source.Connections.Count);
            Assert.False(source.Connections[0].Disposed);

            // Since we were still connected, this should move the connection from a 'waiting to
            // shut down' state into an active state. (We're below the minObservers threshold, but
            // that just determines when Connect is called. RefCount has historically always waited
            // for the subscription count to reach 0 before disconnecting, so if that count goes
            // above 0 while we were waiting for the disconnect delay, it should return to an
            // active state.)
            var seen = 0;
            var terminated = false;
            var s3 = rco.Subscribe(x => seen = x, () => terminated = true);
            source.DeliverNotificationForActiveConnection(Notification.CreateOnNext(2));
            Assert.Equal(2, seen);
            Assert.False(terminated);
            Assert.False(source.Connections[0].Disposed);

            // This moves us past the time when `RefCount` would have shut down the connection if
            // no new subscriptions had turned up. The arrival of a new subscriber should ensure
            // that we remain connected.
            scheduler.AdvanceBy(2);

            Assert.False(terminated);
            Assert.False(source.Connections[0].Disposed);

            // We should be able to advance well beyond the disconnect delay because we have an active
            // subscriber.
            scheduler.AdvanceBy(20);

            Assert.False(terminated);
            Assert.False(source.Connections[0].Disposed);
        }

        [TestMethod]
        public void RefCount_DelayedDisconnect_ValuesDuringAndAfterSubscribe()
        {
            var subject = new ReplaySubject<int>(5);
            var source = subject.Publish().RefCount(TimeSpan.FromSeconds(20));

            subject.OnNext(1);

            // Although the source is a ReplaySubject, the use of Publish means there will only be
            // a single subscription to the ReplaySubject, so it will only replay one. (It will replay
            // that first value on the initial connect.) So we expect each subscriber to see fewer and
            // fewer values.
            // all subscribers will see all the values
            List<int> expected1 = [1];

            var list1 = new List<int>();
            source.Subscribe(list1.Add);
            Assert.Equal(expected1, list1);

            subject.OnNext(2);

            var list2 = new List<int>();
            source.Subscribe(list2.Add);

            expected1 = [1, 2];
            List<int> expected2 = [];

            Assert.Equal(expected1, list1);
            Assert.Equal(expected2, list2);

            subject.OnNext(3);

            var list3 = new List<int>();
            source.Subscribe(list3.Add);

            expected1 = [1, 2, 3];
            expected2 = [3];
            List<int> expected3 = [];

            Assert.Equal(expected1, list1);
            Assert.Equal(expected2, list2);
            Assert.Equal(expected3, list3);

            subject.OnNext(4);
            expected1 = [1, 2, 3, 4];
            expected2 = [3, 4];
            expected3 = [4];
            Assert.Equal(expected1, list1);
            Assert.Equal(expected2, list2);
            Assert.Equal(expected3, list3);
        }

        [TestMethod]
        public void RefCount_DelayedDisconnect_minObservers_ValuesDuringAndAfterSubscribe()
        {
            var subject = new ReplaySubject<int>(5);
            var source = subject.Publish().RefCount(2, TimeSpan.FromSeconds(20));

            subject.OnNext(1);

            var list1 = new List<int>();
            source.Subscribe(list1.Add);
            Assert.Empty(list1);

            subject.OnNext(2);

            List<int> expected1and2 = [1, 2];

            var list2 = new List<int>();
            source.Subscribe(list2.Add);

            Assert.Equal(expected1and2, list1);
            Assert.Equal(expected1and2, list2);

            subject.OnNext(3);
            expected1and2 = [1, 2, 3];
            Assert.Equal(expected1and2, list1);
            Assert.Equal(expected1and2, list2);

            var list3 = new List<int>();
            source.Subscribe(list3.Add);

            List<int> expected3 = [];
            Assert.Equal(expected1and2, list1);
            Assert.Equal(expected1and2, list2);
            Assert.Equal(expected3, list3);

            subject.OnNext(4);
            expected1and2 = [1, 2, 3, 4];
            expected3 = [4];
            Assert.Equal(expected1and2, list1);
            Assert.Equal(expected1and2, list2);
            Assert.Equal(expected3, list3);
        }

        [TestMethod]
        [DataRow(true)]
        [DataRow(false)]
        public void RefCount_DelayedDisconnect_CanConnectAgainIfPreviousSubscriptionTerminatedFromSubscribeByCompletion(
            bool reSubscribeBeforeDelayedDisconnect)
        {
            var scheduler = new TestScheduler();
            var seen = 0;
            var terminated = false;

            // On initial subscription, the source will produce one value and will not complete.
            var connectable = new SerialSingleNotificationConnectable<int>(Notification.CreateOnNext(36));
            var refCount = connectable.RefCount(TimeSpan.FromTicks(10), scheduler);

            using (refCount.Subscribe(value => seen = value, () => terminated = true))
            {
                Assert.Equal(36, seen);
                Assert.Equal(1, connectable.Connections.Count);
                Assert.False(connectable.Connections[0].Disposed);
            }

            Assert.False(connectable.Connections[0].Disposed);

            // For these initial subscriptions, we allow enough time for the delayed disconnect to occur even if
            // reSubscribeBeforeDelayedDisconnect is false, because it's the resubscription after a source-induced
            // completion that this test is interested in.
            scheduler.AdvanceBy(11);

            Assert.Equal(1, connectable.Connections.Count);
            Assert.True(connectable.Connections[0].Disposed);

            seen = 0;
            terminated = false;

            // This time around, when Connect is called, all subscriptions after the preceding Connect will be
            // completed.
            connectable.SetNotificationForNextConnect(Notification.CreateOnCompleted<int>());

            using (refCount.Subscribe(value => seen = value, () => terminated = true))
            {
                Assert.Equal(0, seen);
                Assert.True(terminated);
                Assert.Equal(2, connectable.Connections.Count);
                Assert.False(connectable.Connections[1].Disposed);
            }

            Assert.Equal(2, connectable.Connections.Count);
            Assert.False(connectable.Connections[1].Disposed);

            scheduler.AdvanceBy(reSubscribeBeforeDelayedDisconnect ? 1 : 11);

            Assert.Equal(2, connectable.Connections.Count);
            Assert.Equal(!reSubscribeBeforeDelayedDisconnect, connectable.Connections[1].Disposed);

            seen = 0;
            terminated = false;

            // Now we go back to the initial behaviour in which the source produces one value and does not complete.
            connectable.SetNotificationForNextConnect(Notification.CreateOnNext(42));

            using (refCount.Subscribe(value => seen = value, () => terminated = true))
            {
                Assert.Equal(reSubscribeBeforeDelayedDisconnect ? 0 : 42, seen);
                Assert.Equal(reSubscribeBeforeDelayedDisconnect, terminated);
                Assert.Equal(reSubscribeBeforeDelayedDisconnect ? 2 : 3, connectable.Connections.Count);
                Assert.False(connectable.Connections[reSubscribeBeforeDelayedDisconnect ? 1 : 2].Disposed);
            }
        }

        [TestMethod]
        public void RefCount_DelayedDisconnect_minObservers_CanConnectAgainIfPreviousSubscriptionTerminatedFromSubscribeByCompletionAndEnoughTimeForDisconnectHasPassed()
        {
            var scheduler = new TestScheduler();
            var seen1 = 0;
            var seen2 = 0;
            var terminated1 = false;
            var terminated2 = false;

            // On initial subscription, the source will produce one value and will not complete.
            var connectable = new SerialSingleNotificationConnectable<int>(Notification.CreateOnNext(36));
            var refCount = connectable.RefCount(2, TimeSpan.FromTicks(10), scheduler);

            using (refCount.Subscribe(value => seen1 = value, () => terminated1 = true))
            {
                Assert.Equal(0, seen1);
                Assert.Empty(connectable.Connections);
                using (refCount.Subscribe(value => seen2 = value, () => terminated2 = true))
                {
                    Assert.Equal(36, seen1);
                    Assert.Equal(36, seen2);
                    Assert.Equal(1, connectable.Connections.Count);
                    Assert.False(connectable.Connections[0].Disposed);
                }
            }

            Assert.Equal(1, connectable.Connections.Count);
            Assert.False(connectable.Connections[0].Disposed);

            scheduler.AdvanceBy(11);

            Assert.Equal(1, connectable.Connections.Count);
            Assert.True(connectable.Connections[0].Disposed);

            seen1 = seen2 = 0;
            terminated1 = terminated2 = false;

            // This time around, when Connect is called, all subscriptions after the preceding Connect will be
            // completed.
            connectable.SetNotificationForNextConnect(Notification.CreateOnCompleted<int>());

            using (refCount.Subscribe(value => seen1 = value, () => terminated1 = true))
            {
                Assert.Equal(1, connectable.Connections.Count);
                Assert.False(terminated1);
                Assert.False(terminated2);
                using (refCount.Subscribe(value => seen2 = value, () => terminated2 = true))
                {
                    Assert.Equal(0, seen1);
                    Assert.Equal(0, seen2);
                    Assert.True(terminated1);
                    Assert.True(terminated2);
                    Assert.Equal(2, connectable.Connections.Count);
                    Assert.False(connectable.Connections[1].Disposed);
                }
            }

            Assert.Equal(2, connectable.Connections.Count);
            Assert.False(connectable.Connections[1].Disposed);

            scheduler.AdvanceBy(11);

            Assert.Equal(2, connectable.Connections.Count);
            Assert.True(connectable.Connections[1].Disposed);

            seen1 = seen2 = 0;
            terminated1 = terminated2 = false;

            // Now we go back to the initial behaviour in which the source produces one value and does not complete.
            connectable.SetNotificationForNextConnect(Notification.CreateOnNext(42));

            using (refCount.Subscribe(value => seen1 = value, () => terminated1 = true))
            {
                Assert.False(terminated1);
                Assert.Equal(0, seen1);
                Assert.False(terminated2);
                Assert.Equal(2, connectable.Connections.Count);
                using (refCount.Subscribe(value => seen2 = value, () => terminated2 = true))
                {
                    Assert.Equal(42, seen1);
                    Assert.Equal(42, seen2);
                    Assert.False(terminated1);
                    Assert.False(terminated2);
                    Assert.Equal(3, connectable.Connections.Count);
                    Assert.False(connectable.Connections[2].Disposed);
                }
            }
        }

        [TestMethod]
        public void RefCount_DelayedDisconnect_minObservers_CanConnectAgainIfPreviousSubscriptionTerminatedFromSubscribeByCompletionAndEnoughTimeForDisconnectHasPassed_WithPreConnectNotifications()
        {
            var scheduler = new TestScheduler();
            var seen1 = 0;
            var seen2 = 0;
            var terminated1 = false;
            var terminated2 = false;

            // On initial subscription, the source will produce one value and will not complete.
            var connectable = new SerialConnectableIgnoringConnect<int>(new BehaviorSubject<int>(36));
            var refCount = connectable.RefCount(2, TimeSpan.FromTicks(10), scheduler);

            using (refCount.Subscribe(value => seen1 = value, () => terminated1 = true))
            {
                // The SerialConnectableConnectIgnoringObservable is unusual in that it can produce values before the
                // call to Connect. So we expect to see the value from the source, but not yet to
                // have seen a Connect call.
                Assert.Equal(36, seen1);
                Assert.Empty(connectable.Connections);
                using (refCount.Subscribe(value => seen2 = value, () => terminated2 = true))
                {
                    Assert.Equal(36, seen1);
                    Assert.Equal(36, seen2);
                    Assert.Equal(1, connectable.Connections.Count);
                    Assert.False(connectable.Connections[0].Disposed);
                }
            }

            Assert.Equal(1, connectable.Connections.Count);
            Assert.False(connectable.Connections[0].Disposed);

            scheduler.AdvanceBy(11);

            Assert.Equal(1, connectable.Connections.Count);
            Assert.True(connectable.Connections[0].Disposed);

            seen1 = seen2 = 0;
            terminated1 = terminated2 = false;

            // This time around, the source will complete when subscribed to.
            connectable.SetSource(Observable.Empty<int>());

            using (refCount.Subscribe(value => seen1 = value, () => terminated1 = true))
            {
                // Again, the SerialConnectableConnectIgnoringObservable's unsual behaviour of
                // delivering notifications immediately from subscription without waiting for the
                // Connect means we see the initial termination immediately (and no connection yet).
                Assert.True(terminated1);
                Assert.False(terminated2);
                Assert.Equal(1, connectable.Connections.Count);
                using (refCount.Subscribe(value => seen2 = value, () => terminated2 = true))
                {
                    Assert.Equal(0, seen1);
                    Assert.Equal(0, seen2);
                    Assert.True(terminated1);
                    Assert.True(terminated2);

                    // Since the initial subscription completed immediately, the observer count
                    // never got above 1, so we do not expect a second connection
                    Assert.Equal(1, connectable.Connections.Count);
                    Assert.True(connectable.Connections[0].Disposed);
                }
            }

            Assert.Equal(1, connectable.Connections.Count);

            scheduler.AdvanceBy(11);

            Assert.Equal(1, connectable.Connections.Count);

            seen1 = seen2 = 0;
            terminated1 = terminated2 = false;

            // Now we go back to the initial behaviour in which the source produces one value and does not complete.
            connectable.SetSource(new BehaviorSubject<int>(42));

            using (refCount.Subscribe(value => seen1 = value, () => terminated1 = true))
            {
                Assert.False(terminated1);
                Assert.Equal(42, seen1);
                Assert.False(terminated2);
                Assert.Equal(1, connectable.Connections.Count);
                using (refCount.Subscribe(value => seen2 = value, () => terminated2 = true))
                {
                    Assert.Equal(42, seen1);
                    Assert.Equal(42, seen2);
                    Assert.False(terminated1);
                    Assert.False(terminated2);
                    Assert.Equal(2, connectable.Connections.Count);
                    Assert.False(connectable.Connections[1].Disposed);
                }
            }
        }

        [TestMethod]
        public void RefCount_DelayedDisconnect_minObservers_DoesNotConnectAgainIfPreviousSubscriptionTerminatedFromSubscribeByCompletionButNotEnoughTimeForDelayedDisconnectHasPassed()
        {
            var scheduler = new TestScheduler();
            var seen1 = 0;
            var seen2 = 0;
            var terminated1 = false;
            var terminated2 = false;

            // On initial subscription, the source will produce one value and will not complete.
            var connectable = new SerialSingleNotificationConnectable<int>(Notification.CreateOnNext(36));
            var refCount = connectable.RefCount(2, TimeSpan.FromTicks(10), scheduler);

            using (refCount.Subscribe(value => seen1 = value, () => terminated1 = true))
            {
                Assert.Equal(0, seen1);
                Assert.Empty(connectable.Connections);
                using (refCount.Subscribe(value => seen2 = value, () => terminated2 = true))
                {
                    Assert.Equal(36, seen1);
                    Assert.Equal(36, seen2);
                    Assert.Equal(1, connectable.Connections.Count);
                    Assert.False(connectable.Connections[0].Disposed);
                }
            }

            Assert.Equal(1, connectable.Connections.Count);
            Assert.False(connectable.Connections[0].Disposed);

            // For these initial subscriptions, we allow enough time for the delayed disconnect to occur, because
            // it's the resubscription after a source-induced completion that this test is interested in.
            scheduler.AdvanceBy(11);

            Assert.Equal(1, connectable.Connections.Count);
            Assert.True(connectable.Connections[0].Disposed);

            seen1 = seen2 = 0;
            terminated1 = terminated2 = false;

            // Any further subscriptions will be completed on the next Connect.
            connectable.SetNotificationForNextConnect(Notification.CreateOnCompleted<int>());

            using (refCount.Subscribe(value => seen1 = value, () => terminated1 = true))
            {
                Assert.Equal(1, connectable.Connections.Count);
                Assert.False(terminated1);
                using (refCount.Subscribe(value => seen2 = value, () => terminated2 = true))
                {
                    Assert.Equal(0, seen1);
                    Assert.Equal(0, seen2);
                    Assert.True(terminated1);
                    Assert.True(terminated2);

                    Assert.Equal(2, connectable.Connections.Count);
                    Assert.False(connectable.Connections[1].Disposed);
                }
            }

            Assert.Equal(2, connectable.Connections.Count);
            Assert.False(connectable.Connections[1].Disposed);

            scheduler.AdvanceBy(5);

            Assert.Equal(2, connectable.Connections.Count);
            Assert.False(connectable.Connections[1].Disposed);

            seen1 = seen2 = 0;
            terminated1 = terminated2 = false;

            // To verify that individual subscriptions continue to be forwarded to the underlying source even
            // when no reconnect occurs, we arrange for subsequent subscriptions to get receive a single value.
            // (This is a slightly odd thing to do, but it's not RefCount's place to have opinions on how the
            // source should behave.)
            connectable.Connections[1].ReplaceSource(new BehaviorSubject<int>(42));

            // The connection set up in the preceding section won't be torn down until the
            // specified disconnect delay has elapsed, so the expected behaviour if we try to establish
            // new subscriptions in that time is that their Subscribe will be passed through to the source,
            // and that we won't see any further connections. But now that the further subscriptions to the
            // source will result in a value (even though earlier subscriptions to the same source have been
            // completed) we expect these new subscriptions each to see the value.
            using (refCount.Subscribe(value => seen1 = value, () => terminated1 = true))
            {
                Assert.False(terminated1);
                Assert.Equal(42, seen1);
                Assert.False(terminated2);
                Assert.Equal(2, connectable.Connections.Count);
                Assert.False(connectable.Connections[1].Disposed);
                using (refCount.Subscribe(value => seen2 = value, () => terminated2 = true))
                {
                    Assert.Equal(42, seen1);
                    Assert.Equal(42, seen2);
                    Assert.False(terminated1);
                    Assert.False(terminated2);
                    Assert.Equal(2, connectable.Connections.Count);
                    Assert.False(connectable.Connections[1].Disposed);
                }
            }

            connectable.SetNotificationForNextConnect(Notification.CreateOnNext(99));

            // If we advanced by enough for the deferred disconnect to occur, it should be able to create a fresh
            // connection to the underlying source, at which point we'll see the value again.
            // We were at 5, so this takes us to 11 since the initial connection, but we don't expect that to be
            // enough, because the deferred disconnection should be relative to the most recent subscription.
            scheduler.AdvanceBy(6);

            Assert.Equal(2, connectable.Connections.Count);
            Assert.False(connectable.Connections[1].Disposed);

            // Since the last subscription occurred at 5, advancing to 16 should trigger disconnection. And
            // since we're already up to 11, this should do it:
            scheduler.AdvanceBy(5);

            Assert.Equal(2, connectable.Connections.Count);
            Assert.True(connectable.Connections[1].Disposed);

            seen1 = seen2 = 0;
            terminated1 = terminated2 = false;

            using (refCount.Subscribe(value => seen1 = value, () => terminated1 = true))
            {
                Assert.Equal(0, seen1);
                Assert.Equal(2, connectable.Connections.Count);
                Assert.True(connectable.Connections[1].Disposed);
                using (refCount.Subscribe(value => seen2 = value, () => terminated2 = true))
                {
                    Assert.Equal(99, seen1);
                    Assert.Equal(99, seen2);
                    Assert.False(terminated1);
                    Assert.False(terminated2);
                    Assert.Equal(3, connectable.Connections.Count);
                    Assert.False(connectable.Connections[2].Disposed);
                }
            }
        }

        #endregion
    }
}

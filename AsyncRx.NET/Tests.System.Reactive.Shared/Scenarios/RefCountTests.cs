// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information. 

using System.Reactive;

namespace Tests.System.Reactive.Shared.Scenarios;

/// <summary>Shared <c>RefCount</c> scenarios, from Rx.NET's <c>RefCountTest.cs</c>.</summary>
/// <remarks>
/// <para>
/// Both halves of the file: 35 of its 41 tests (one is a <c>DataRow</c> pair in the original).
/// Deliberately not here: the two <c>*_ArgumentChecking</c> tests (the code-generated stratum)
/// and the six tests that build a
/// connectable by hand from a <c>MySubject</c> and
/// <c>ConnectableObservable&lt;int&gt;(xs, subject)</c>, which belong with <c>Multicast</c>.
/// The <c>DelayedDisconnect</c> tests step the clock by hand with
/// <c>Scheduler.AdvanceBy</c> between subscriptions and assertions, and one of them is a
/// <c>DataRow</c> pair in the original, written here as two tests.
/// </para>
/// <para>
/// This is the first operator whose input is a connectable. Two things shape the text. Most
/// tests are real-time: they subscribe to the <c>RefCount</c> result through the raw surface with
/// handlers or a <c>List&lt;int&gt;.Add</c>, push into subjects by hand, and assert on captured
/// locals, so they are <c>async Task</c> methods that await each subscription and disposal.
/// And many drive two test doubles, <see cref="SerialSingleNotificationConnectable{T}"/> and
/// <see cref="SerialConnectableIgnoringConnect{T}"/>, which
/// are declared here, next to the tests that use them, as Rx.NET's originals are private nested
/// types of its <c>RefCountTest</c>; they are not general facilities. Each one's behaviour is
/// defined once, in its nested type's documentation; what differs per target, the connectable
/// object built over the target's own subjects, comes from
/// <see cref="IRxTarget.CreateRefCountTestConnectable{T}"/> and its sibling.
/// </para>
/// </remarks>
public abstract class RefCountTests : SharedReactiveTest
{
    /// <summary>A connectable that delivers one chosen notification per connection.</summary>
    /// <remarks>
    /// <para>
    /// Rx.NET's <c>SerialSingleNotificationConnectable&lt;T&gt;</c>. Its point is to deliver
    /// completion to one connection and then values to the next, which <c>Publish</c> cannot do
    /// (its subject stays completed), so that a scenario can check that <c>RefCount</c>
    /// reconnects after a source completed inside <c>Connect</c>.
    /// </para>
    /// <para>
    /// Each <c>Connect()</c> opens a connection with a fresh subject, delivers the current
    /// notification into it, and records the connection in <see cref="Connections"/>.
    /// Subscribing attaches to the active connection's subject, or to the one the next
    /// connection will use. Each target builds the object that behaves this way over its own
    /// subjects, recording into the <see cref="State"/> this leaf exposes.
    /// </para>
    /// </remarks>
    public sealed class SerialSingleNotificationConnectable<T>(Realized<Seq<T>> native, SerialSingleNotificationConnectable<T>.State state)
        : ConnectableSeq<T>
    {
        /// <summary>The connections opened so far, oldest first.</summary>
        public IReadOnlyList<Connection> Connections => state.Connections;

        /// <summary>Changes the notification the next connection delivers.</summary>
        /// <param name="notification">The notification.</param>
        public void SetNotificationForNextConnect(Notification<T> notification) => state.Next = notification;

        /// <summary>Delivers a notification into the active connection's subscribers.</summary>
        /// <param name="notification">The notification.</param>
        /// <remarks>
        /// Fails if no connection is active, or if the active connection's source has been
        /// replaced through <see cref="Connection.ReplaceSource"/> and is no longer the subject
        /// this double can push into.
        /// </remarks>
        public ValueTask DeliverNotificationForActiveConnectionAsync(Notification<T> notification) =>
            state.DeliverToActive(notification);

        /// <inheritdoc/>
        protected override Realized<Seq<T>> AcceptCore(ISeqVisitor visitor) => native;

        /// <inheritdoc/>
        public override string ToString() => "SerialSingleNotificationConnectable";

        /// <summary>What the double remembers across connections.</summary>
        /// <param name="initial">The notification the first connection delivers.</param>
        public sealed class State(Notification<T> initial)
        {
            /// <summary>The notification the next connection delivers.</summary>
            public Notification<T> Next { get; set; } = initial;

            /// <summary>The connections opened so far.</summary>
            public List<Connection> Connections { get; } = [];

            /// <summary>
            /// Set by the target: pushes a notification into the active connection.
            /// </summary>
            /// <remarks>
            /// The target's object owns the subjects, so only it can push; it installs this when
            /// built, and the double calls it for
            /// <see cref="SerialSingleNotificationConnectable{T}.DeliverNotificationForActiveConnectionAsync"/>.
            /// </remarks>
            public Func<Notification<T>, ValueTask> DeliverToActive { get; set; } = _ =>
                throw new InvalidOperationException("The target did not install a delivery function.");
        }

        /// <summary>One connection, open until its disposable is disposed.</summary>
        public sealed class Connection
        {
            /// <summary>Whether the connection's disposable has been disposed.</summary>
            public bool Disposed { get; set; }

            /// <summary>
            /// The source this connection's later subscribers attach to, once replaced; null
            /// while it is still the connection's own subject.
            /// </summary>
            public Seq<T>? ReplacedSource { get; private set; }

            /// <summary>
            /// Replaces this connection's source for subscribers that arrive later.
            /// </summary>
            /// <param name="source">The new source.</param>
            /// <remarks>
            /// Once a connection's subject has completed, it completes every later subscriber;
            /// replacing the source lets a scenario deliver notifications to subscribers that
            /// arrive after that, while the connection stays open.
            /// </remarks>
            public void ReplaceSource(Seq<T> source) => ReplacedSource = source;
        }
    }

    /// <summary>Creates the test double on the target under test.</summary>
    /// <typeparam name="T">The type of the elements the connectable carries.</typeparam>
    /// <param name="initial">The notification the first connection delivers.</param>
    /// <remarks>
    /// A scenario calls this where the Rx.NET test writes
    /// <c>new SerialSingleNotificationConnectable&lt;T&gt;(n)</c>.
    /// </remarks>
    private SerialSingleNotificationConnectable<T> CreateSerialSingleNotificationConnectable<T>(Notification<T> initial)
    {
        var state = new SerialSingleNotificationConnectable<T>.State(initial);
        return new(Target.CreateRefCountTestConnectable(state), state);
    }

    /// <summary>A connectable that logs <c>Connect()</c> but otherwise ignores it.</summary>
    /// <remarks>
    /// <para>
    /// Rx.NET's <c>SerialConnectableIgnoringConnect&lt;T&gt;</c>. It forwards every subscription
    /// to its current source, which a scenario can replace over time, and records each
    /// <c>Connect()</c> as a connection without acting on it. That simulates a source that
    /// delivers notifications before <c>Connect</c> is called (as sources that complete
    /// synchronously do), which <c>RefCount</c> must handle, and a source that completes some
    /// subscribers and then reverts to a state where later ones are not completed.
    /// </para>
    /// </remarks>
    public sealed class SerialConnectableIgnoringConnect<T>(Realized<Seq<T>> native, SerialConnectableIgnoringConnect<T>.State state)
        : ConnectableSeq<T>
    {
        /// <summary>The connections logged so far, oldest first.</summary>
        public IReadOnlyList<SerialSingleNotificationConnectable<T>.Connection> Connections => state.Connections;

        /// <summary>Replaces the source later subscribers attach to.</summary>
        /// <param name="source">The new source.</param>
        public void SetSource(Seq<T> source) => state.Source = source;

        /// <inheritdoc/>
        protected override Realized<Seq<T>> AcceptCore(ISeqVisitor visitor) => native;

        /// <inheritdoc/>
        public override string ToString() => "SerialConnectableIgnoringConnect";

        /// <summary>What the double remembers.</summary>
        /// <param name="initialSource">
        /// The source subscribers attach to until it is replaced.
        /// </param>
        public sealed class State(Seq<T> initialSource)
        {
            /// <summary>The source subscribers attach to.</summary>
            public Seq<T> Source { get; set; } = initialSource;

            /// <summary>The connections logged so far.</summary>
            public List<SerialSingleNotificationConnectable<T>.Connection> Connections { get; } = [];
        }
    }

    /// <summary>Creates the connect-ignoring double on the target under test.</summary>
    /// <typeparam name="T">The type of the elements the connectable carries.</typeparam>
    /// <param name="initialSource">The source subscribers attach to until it is replaced.</param>
    /// <remarks>
    /// A scenario calls this where the Rx.NET test writes
    /// <c>new SerialConnectableIgnoringConnect&lt;T&gt;(source)</c>.
    /// </remarks>
    private SerialConnectableIgnoringConnect<T> CreateSerialConnectableIgnoringConnect<T>(Seq<T> initialSource)
    {
        var state = new SerialConnectableIgnoringConnect<T>.State(initialSource);
        return new(Target.CreateRefCountTestConnectableIgnoringConnect(state), state);
    }

    [TestMethod]
    public async Task RefCount_NoDelay_SourceProducesValuesAndCompletesInConnect()
    {
        var connected = 0;
        var source = Seq.Defer(() =>
        {
            connected++;
            return Seq.Range(1, 5);
        })
        .Publish()
        .RefCount();

        Assert.AreEqual(0, connected);

        var list1 = new List<int>();
        await source.SubscribeAsync(Scheduler, list1.Add);
        Assert.AreEqual(1, connected);
        List<int> expected1 = [1, 2, 3, 4, 5];
        CollectionAssert.AreEqual(expected1, list1);

        var list2 = new List<int>();
        await source.SubscribeAsync(Scheduler, list2.Add);
        Assert.AreEqual(1, connected);
        Assert.IsEmpty(list2);
    }

    [TestMethod]
    public async Task RefCount_NoDelay_minObservers_SourceProducesValuesAndCompletesInConnect()
    {
        var connected = 0;
        var source = Seq.Defer(() =>
        {
            connected++;
            return Seq.Range(1, 5);
        })
        .Publish()
        .RefCount(2);

        Assert.AreEqual(0, connected);

        var list1 = new List<int>();
        await source.SubscribeAsync(Scheduler, list1.Add);
        Assert.AreEqual(0, connected);
        Assert.IsEmpty(list1);

        var list2 = new List<int>();
        await source.SubscribeAsync(Scheduler, list2.Add);
        Assert.AreEqual(1, connected);
        List<int> expected = [1, 2, 3, 4, 5];
        CollectionAssert.AreEqual(expected, list1);
        CollectionAssert.AreEqual(expected, list2);
    }

    [TestMethod]
    public async Task RefCount_NoDelay_SourceCompletesWithNoValuesInConnect()
    {
        var connectable = CreateSerialSingleNotificationConnectable(Notification.CreateOnCompleted<int>());
        var refCount = connectable.RefCount();

        await using var s1 = await refCount.SubscribeAsync(Scheduler, _ => { });
        Assert.HasCount(1, connectable.Connections);

        // Since the source immediately completed, the RefCount goes back to zero subscribers
        // inside the call to Connect, so we expect to be disconnected.
        Assert.IsTrue(connectable.Connections[0].Disposed);

        await using var s2 = await refCount.SubscribeAsync(Scheduler, _ => { });
        Assert.HasCount(2, connectable.Connections);
        Assert.IsTrue(connectable.Connections[1].Disposed);
    }

    [TestMethod]
    public async Task RefCount_NoDelay_minObservers_SourceCompletesWithNoValuesInConnect()
    {
        var connectable = CreateSerialSingleNotificationConnectable(Notification.CreateOnCompleted<int>());
        var refCount = connectable.RefCount(2);

        var s1 = await refCount.SubscribeAsync(Scheduler, _ => { });
        Assert.IsEmpty(connectable.Connections);

        var s2 = await refCount.SubscribeAsync(Scheduler, _ => { });
        Assert.HasCount(1, connectable.Connections);

        // Since the source completes immediately, we will have no active subscribers, so
        // we expect to be disconnected.
        Assert.IsTrue(connectable.Connections[0].Disposed);

        await s1.DisposeAsync();
        await s2.DisposeAsync();

        // Disposing subscriptions should change nothing because they self-completed.
        Assert.HasCount(1, connectable.Connections);

        // We're now back in the initial disconnected state, so nothing more should
        // happen until we get up to minObservers.
        await using var s3 = await refCount.SubscribeAsync(Scheduler, _ => { });
        Assert.HasCount(1, connectable.Connections);

        await using var s4 = await refCount.SubscribeAsync(Scheduler, _ => { });
        Assert.HasCount(2, connectable.Connections);
        Assert.IsTrue(connectable.Connections[1].Disposed);
    }

    [TestMethod]
    public async Task RefCount_NoDelay_minObservers_NotConnected()
    {
        var connected = 0;
        var source = Seq.Defer(() =>
        {
            connected++;
            return Seq.Never<int>();
        })
        .Publish()
        .RefCount(2);

        Assert.AreEqual(0, connected);

        await source.SubscribeAsync(Scheduler, _ => { });
        Assert.AreEqual(0, connected);
    }

    [TestMethod]
    public async Task RefCount_NoDelay_OnError()
    {
        var ex = new Exception();
        var xs = Seq.Throw<int>(ex, ImmediateScheduler);

        var res = xs.Publish().RefCount();

        await res.SubscribeAsync(Scheduler, _ => Assert.Fail("OnNext unexpected"), ex_ => Assert.AreSame(ex, ex_), () => Assert.Fail("OnCompleted unexpected"));
        await res.SubscribeAsync(Scheduler, _ => Assert.Fail("OnNext unexpected"), ex_ => Assert.AreSame(ex, ex_), () => Assert.Fail("OnCompleted unexpected"));
    }

    [TestMethod]
    public async Task RefCount_NoDelay_minObservers_OnError()
    {
        var ex = new Exception();
        var xs = Seq.Throw<int>(ex, ImmediateScheduler);

        var res = xs.Publish().RefCount(2);

        var exceptionsReceived = new List<Exception>();

        ValueTask<IAsyncDisposable> AddSubscriber() =>
            res.SubscribeAsync(
                Scheduler,
                _ => Assert.Fail("OnNext unexpected"),
                ex_ => exceptionsReceived.Add(ex),
                () => Assert.Fail("OnComplete unexpected"));

        await AddSubscriber();
        Assert.IsEmpty(exceptionsReceived);

        await AddSubscriber();
        Assert.HasCount(2, exceptionsReceived);
        Assert.AreSame(ex, exceptionsReceived[0]);
        Assert.AreSame(ex, exceptionsReceived[1]);
    }

    [TestMethod]
    public void RefCount_NoDelay_HotSourceMultipleSubscribers()
    {
        var xs = Scheduler.CreateHotObservable(
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

        var d1 = default(IAsyncDisposable);
        var o1 = Scheduler.CreateObserver<int>();
        Scheduler.ScheduleAbsolute(215, async () => { d1 = await res.SubscribeAsync(o1); });
        Scheduler.ScheduleAbsolute(235, async () => { await d1!.DisposeAsync(); });

        var d2 = default(IAsyncDisposable);
        var o2 = Scheduler.CreateObserver<int>();
        Scheduler.ScheduleAbsolute(225, async () => { d2 = await res.SubscribeAsync(o2); });
        Scheduler.ScheduleAbsolute(275, async () => { await d2!.DisposeAsync(); });

        var d3 = default(IAsyncDisposable);
        var o3 = Scheduler.CreateObserver<int>();
        Scheduler.ScheduleAbsolute(255, async () => { d3 = await res.SubscribeAsync(o3); });
        Scheduler.ScheduleAbsolute(265, async () => { await d3!.DisposeAsync(); });

        var d4 = default(IAsyncDisposable);
        var o4 = Scheduler.CreateObserver<int>();
        Scheduler.ScheduleAbsolute(285, async () => { d4 = await res.SubscribeAsync(o4); });
        Scheduler.ScheduleAbsolute(320, async () => { await d4!.DisposeAsync(); });

        Scheduler.Start();

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
        var xs = Scheduler.CreateHotObservable(
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

        var d1 = default(IAsyncDisposable);
        var o1 = Scheduler.CreateObserver<int>();
        Scheduler.ScheduleAbsolute(215, async () => { d1 = await res.SubscribeAsync(o1); });
        Scheduler.ScheduleAbsolute(235, async () => { await d1!.DisposeAsync(); });

        var d2 = default(IAsyncDisposable);
        var o2 = Scheduler.CreateObserver<int>();
        Scheduler.ScheduleAbsolute(225, async () => { d2 = await res.SubscribeAsync(o2); });
        Scheduler.ScheduleAbsolute(275, async () => { await d2!.DisposeAsync(); });

        var d3 = default(IAsyncDisposable);
        var o3 = Scheduler.CreateObserver<int>();
        Scheduler.ScheduleAbsolute(255, async () => { d3 = await res.SubscribeAsync(o3); });
        Scheduler.ScheduleAbsolute(265, async () => { await d3!.DisposeAsync(); });

        var d4 = default(IAsyncDisposable);
        var o4 = Scheduler.CreateObserver<int>();
        Scheduler.ScheduleAbsolute(285, async () => { d4 = await res.SubscribeAsync(o4); });
        Scheduler.ScheduleAbsolute(320, async () => { await d4!.DisposeAsync(); });

        var d5 = default(IAsyncDisposable);
        var o5 = Scheduler.CreateObserver<int>();
        Scheduler.ScheduleAbsolute(295, async () => { d5 = await res.SubscribeAsync(o5); });
        Scheduler.ScheduleAbsolute(320, async () => { await d5!.DisposeAsync(); });

        Scheduler.Start();

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
    public async Task RefCount_NoDelay_minObservers_SubscriptionsDropBelowThresholdButNotToZero()
    {
        var subject = CreateReplaySubject<int>(5);
        var connected = 0;
        var source = Seq.Defer(() =>
            {
                connected++;
                return subject;
            })
            .Publish().RefCount(2);

        await subject.OnNextAsync(1);
        Assert.AreEqual(0, connected);

        var list1 = new List<int>();
        var sub1 = await source.SubscribeAsync(Scheduler, list1.Add);
        Assert.AreEqual(0, connected);
        Assert.IsEmpty(list1);

        await subject.OnNextAsync(2);

        var list2 = new List<int>();
        var sub2 = await source.SubscribeAsync(Scheduler, list2.Add);

        // Since connection only occurred with the 2nd subscriber, we expect both to get everything
        // the ReplaySubject has stored.
        List<int> expectedSub1 = [1, 2];
        var expectedSub2 = expectedSub1;
        CollectionAssert.AreEqual(expectedSub1, list1);
        CollectionAssert.AreEqual(expectedSub1, list2);
        Assert.AreEqual(1, connected);

        await subject.OnNextAsync(3);

        // Both subscribers should have received the new item.
        expectedSub1 = expectedSub2 = [1, 2, 3];
        CollectionAssert.AreEqual(expectedSub1, list1);
        CollectionAssert.AreEqual(expectedSub2, list2);
        Assert.AreEqual(1, connected);

        var list3 = new List<int>();
        await source.SubscribeAsync(Scheduler, list3.Add);

        // Since we were already connected, the 3rd subscriber just gets added to the observers of
        // the Publish multicast output, and no new connection should occur to the underlying ReplaySubject.
        // So for this 3rd subscription, no new items should be received by any of the subscribers
        List<int> expectedSub3 = [];
        CollectionAssert.AreEqual(expectedSub1, list1);
        CollectionAssert.AreEqual(expectedSub2, list2);
        CollectionAssert.AreEqual(expectedSub3, list3);
        Assert.AreEqual(1, connected);

        await subject.OnNextAsync(4);

        // All the current subscribers should have received that latest item.
        expectedSub1 = expectedSub2 = [1, 2, 3, 4];
        expectedSub3 = [4];
        CollectionAssert.AreEqual(expectedSub1, list1);
        CollectionAssert.AreEqual(expectedSub2, list2);
        CollectionAssert.AreEqual(expectedSub3, list3);
        Assert.AreEqual(1, connected);

        await sub1.DisposeAsync();
        await subject.OnNextAsync(5);

        // The two remaining subscribers should have received that new item, but the one that just
        // unsubscribed should not.
        expectedSub1 = [1, 2, 3, 4];
        expectedSub2 = [1, 2, 3, 4, 5];
        expectedSub3 = [4, 5];
        CollectionAssert.AreEqual(expectedSub1, list1);
        CollectionAssert.AreEqual(expectedSub2, list2);
        CollectionAssert.AreEqual(expectedSub3, list3);
        Assert.AreEqual(1, connected);

        await sub2.DisposeAsync();
        await subject.OnNextAsync(6);

        // We are now below the minObservers threshold of 2, but that threshold only governs when we move
        // from a disconnected state to a connected state. We should remain connected as long as there is
        // at least one subscriber, so we expect the remaining subscriber to receive that last item.
        expectedSub1 = [1, 2, 3, 4];
        expectedSub2 = [1, 2, 3, 4, 5];
        expectedSub3 = [4, 5, 6];
        CollectionAssert.AreEqual(expectedSub1, list1);
        CollectionAssert.AreEqual(expectedSub2, list2);
        CollectionAssert.AreEqual(expectedSub3, list3);
        Assert.AreEqual(1, connected);
    }

    [TestMethod]
    public async Task RefCount_NoDelay_SubscriptionsDropBelowThresholdAndThenBackAbove()
    {
        var sourceAfterInitial = CreateSubject<int>();
        var connected = 0;
        var source = Seq.Defer(() =>
        {
            connected++;
            return Seq.Range(1, 5).Concat(sourceAfterInitial);
        })
        .Publish()
        .RefCount(2);

        Assert.AreEqual(0, connected);

        var list1 = new List<int>();
        var sub1 = await source.SubscribeAsync(Scheduler, list1.Add);
        Assert.AreEqual(0, connected);
        Assert.IsEmpty(list1);

        var list2 = new List<int>();
        var sub2 = await source.SubscribeAsync(Scheduler, list2.Add);
        Assert.AreEqual(1, connected);

        await sourceAfterInitial.OnNextAsync(6);
        await sub1.DisposeAsync();
        await sourceAfterInitial.OnNextAsync(7);
        Assert.AreEqual(1, connected);

        var list3 = new List<int>();
        var sub3 = await source.SubscribeAsync(Scheduler, list3.Add);

        // This is the distinguishing feature of this test. With that last subscription, we went from 1
        // subscriber (below minObservers) but still connected (because we already hit minObservers once
        // and never dropped to zero), and now we're passing through minObservers again. We used to have
        // a bug where we would erroneously attempt to reconnect at this point.
        Assert.AreEqual(1, connected);

        await sourceAfterInitial.OnNextAsync(8);

        var expectedSub1 = new List<int>([1, 2, 3, 4, 5, 6]);
        var expectedSub2 = new List<int>([1, 2, 3, 4, 5, 6, 7, 8]);
        var expectedSub3 = new List<int>([8]);
        CollectionAssert.AreEqual(expectedSub1, list1);
        CollectionAssert.AreEqual(expectedSub2, list2);
        CollectionAssert.AreEqual(expectedSub3, list3);
    }

    [TestMethod]
    public async Task RefCount_NoDelay_ValuesDuringAndAfterSubscribe()
    {
        var subject = CreateReplaySubject<int>(5);
        var source = subject.Publish().RefCount();

        await subject.OnNextAsync(1);

        // Although the source is a ReplaySubject, the use of Publish means there will only be
        // a single subscription to the ReplaySubject, so it will only replay one. (It will replay
        // that first value on the initial connect.) So we expect each subscriber to see fewer and
        // fewer values.

        // all subscribers will see all the values
        List<int> expected1 = [1];
        var list1 = new List<int>();
        await source.SubscribeAsync(Scheduler, list1.Add);
        CollectionAssert.AreEqual(expected1, list1);

        await subject.OnNextAsync(2);
        var list2 = new List<int>();
        await source.SubscribeAsync(Scheduler, list2.Add);

        expected1 = [1, 2];
        List<int> expected2 = [];
        CollectionAssert.AreEqual(expected1, list1);
        CollectionAssert.AreEqual(expected2, list2);

        await subject.OnNextAsync(3);
        var list3 = new List<int>();
        await source.SubscribeAsync(Scheduler, list3.Add);

        expected1 = [1, 2, 3];
        expected2 = [3];
        List<int> expected3 = [];
        CollectionAssert.AreEqual(expected1, list1);
        CollectionAssert.AreEqual(expected2, list2);
        CollectionAssert.AreEqual(expected3, list3);

        await subject.OnNextAsync(4);

        expected1 = [1, 2, 3, 4];
        expected2 = [3, 4];
        expected3 = [4];
        CollectionAssert.AreEqual(expected1, list1);
        CollectionAssert.AreEqual(expected2, list2);
        CollectionAssert.AreEqual(expected3, list3);
    }

    [TestMethod]
    public async Task RefCount_NoDelay_minObservers_ValuesDuringAndAfterSubscribe()
    {
        var subject = CreateReplaySubject<int>(5);
        var source = subject.Publish().RefCount(2);

        await subject.OnNextAsync(1);

        var list1 = new List<int>();
        await source.SubscribeAsync(Scheduler, list1.Add);
        Assert.IsEmpty(list1);

        await subject.OnNextAsync(2);
        List<int> expected1and2 = [1, 2];
        var list2 = new List<int>();
        await source.SubscribeAsync(Scheduler, list2.Add);

        CollectionAssert.AreEqual(expected1and2, list1);
        CollectionAssert.AreEqual(expected1and2, list2);

        await subject.OnNextAsync(3);

        expected1and2 = [1, 2, 3];
        CollectionAssert.AreEqual(expected1and2, list1);
        CollectionAssert.AreEqual(expected1and2, list2);

        var list3 = new List<int>();
        await source.SubscribeAsync(Scheduler, list3.Add);

        List<int> expected3 = [];
        CollectionAssert.AreEqual(expected1and2, list1);
        CollectionAssert.AreEqual(expected1and2, list2);
        CollectionAssert.AreEqual(expected3, list3);

        await subject.OnNextAsync(4);

        expected1and2 = [1, 2, 3, 4];
        expected3 = [4];
        CollectionAssert.AreEqual(expected1and2, list1);
        CollectionAssert.AreEqual(expected1and2, list2);
        CollectionAssert.AreEqual(expected3, list3);
    }

    [TestMethod]
    public async Task RefCount_NoDelay_CanConnectAgainIfPreviousSubscriptionTerminatedFromSubscribeByCompletion()
    {
        var seen = 0;
        var terminated = false;

        // On initial subscription, the source will produce one value and will not complete.
        var connectable = CreateSerialSingleNotificationConnectable(Notification.CreateOnNext(36));
        var refCount = connectable.RefCount();

        await using (await refCount.SubscribeAsync(Scheduler, value => seen = value, _ => { }, () => terminated = true))
        {
            Assert.AreEqual(36, seen);
        }

        seen = 0;
        terminated = false;

        // This time around, the source will complete when subscribed to.
        connectable.SetNotificationForNextConnect(Notification.CreateOnCompleted<int>());
        await using (await refCount.SubscribeAsync(Scheduler, value => seen = value, _ => { }, () => terminated = true))
        {
            Assert.AreEqual(0, seen);
            Assert.IsTrue(terminated);
        }

        seen = 0;
        terminated = false;

        // Now we go back to the initial behaviour in which the source produces one value and does not complete.
        connectable.SetNotificationForNextConnect(Notification.CreateOnNext(42));
        await using (await refCount.SubscribeAsync(Scheduler, value => seen = value, _ => { }, () => terminated = true))
        {
            Assert.AreEqual(42, seen);
            Assert.IsFalse(terminated);
        }
    }

    [TestMethod]
    public async Task RefCount_NoDelay_minObservers_CanConnectAgainIfPreviousSubscriptionTerminatedFromSubscribeByCompletion()
    {
        var seen1 = 0;
        var seen2 = 0;
        var terminated1 = false;
        var terminated2 = false;

        // On initial subscription, the source will produce one value and will not complete.
        var connectable = CreateSerialSingleNotificationConnectable(Notification.CreateOnNext(36));
        var refCount = connectable.RefCount(2);

        await using (await refCount.SubscribeAsync(Scheduler, value => seen1 = value, _ => { }, () => terminated1 = true))
        {
            Assert.AreEqual(0, seen1);
            await using (await refCount.SubscribeAsync(Scheduler, value => seen2 = value, _ => { }, () => terminated2 = true))
            {
                Assert.AreEqual(36, seen1);
                Assert.AreEqual(36, seen2);
            }
        }

        seen1 = seen2 = 0;
        terminated1 = terminated2 = false;

        // This time around, the source will complete when subscribed to.
        connectable.SetNotificationForNextConnect(Notification.CreateOnCompleted<int>());
        await using (await refCount.SubscribeAsync(Scheduler, value => seen1 = value, _ => { }, () => terminated1 = true))
        {
            Assert.IsFalse(terminated1);
            Assert.IsFalse(terminated2);
            await using (await refCount.SubscribeAsync(Scheduler, value => seen2 = value, _ => { }, () => terminated2 = true))
            {
                Assert.AreEqual(0, seen1);
                Assert.AreEqual(0, seen2);
                Assert.IsTrue(terminated1);
                Assert.IsTrue(terminated2);
            }
        }

        seen1 = seen2 = 0;
        terminated1 = terminated2 = false;

        // Now we go back to the initial behaviour in which the source produces one value and does not complete.
        connectable.SetNotificationForNextConnect(Notification.CreateOnNext(42));
        await using (await refCount.SubscribeAsync(Scheduler, value => seen1 = value, _ => { }, () => terminated1 = true))
        {
            Assert.AreEqual(0, seen1);
            await using (await refCount.SubscribeAsync(Scheduler, value => seen2 = value, _ => { }, () => terminated2 = true))
            {
                Assert.AreEqual(42, seen1);
                Assert.AreEqual(42, seen2);
                Assert.IsFalse(terminated1);
                Assert.IsFalse(terminated2);
            }
        }
    }

    [TestMethod]
    public async Task RefCount_DelayedDisconnect_minObservers_SourceProducesValuesAndCompletesInSubscribe()
    {
        var connected = 0;
        var source = Seq.Defer(() =>
        {
            connected++;
            return Seq.Range(1, 5);
        })
        .Publish()
        .RefCount(2, TimeSpan.FromMinutes(1));

        Assert.AreEqual(0, connected);

        var list1 = new List<int>();
        await source.SubscribeAsync(Scheduler, list1.Add);
        Assert.AreEqual(0, connected);
        Assert.IsEmpty(list1);

        var list2 = new List<int>();
        await source.SubscribeAsync(Scheduler, list2.Add);
        Assert.AreEqual(1, connected);
        var expected = new List<int>([1, 2, 3, 4, 5]);
        CollectionAssert.AreEqual(expected, list1);
        CollectionAssert.AreEqual(expected, list2);
    }

    [TestMethod]
    public async Task RefCount_DelayedDisconnect_SourceCompletesWithNoValuesInSubscribe()
    {
        var subscribed = 0;
        var unsubscribed = 0;
        var o1 = Seq.Create<string>(async observer =>
        {
            subscribed++;
            await observer.OnCompletedAsync();
            return () => unsubscribed++;
        });

        var o2 = o1.Publish().RefCount(TimeSpan.FromSeconds(20));

        var s1 = await o2.SubscribeAsync(Scheduler, _ => { });
        Assert.AreEqual(1, subscribed);
        Assert.AreEqual(1, unsubscribed);

        var s2 = await o2.SubscribeAsync(Scheduler, _ => { });
        Assert.AreEqual(1, subscribed);
        Assert.AreEqual(1, unsubscribed);
    }

    [TestMethod]
    public async Task RefCount_DelayedDisconnect_minObservers_SourceCompletesWithNoValuesInSubscribe()
    {
        var subscribed = 0;
        var unsubscribed = 0;
        var o1 = Seq.Create<string>(async observer =>
        {
            subscribed++;
            await observer.OnCompletedAsync();
            return () => unsubscribed++;
        });

        var o2 = o1.Publish().RefCount(2, TimeSpan.FromTicks(10), Scheduler);

        var s1 = await o2.SubscribeAsync(Scheduler, _ => { });
        Assert.AreEqual(0, subscribed);
        Assert.AreEqual(0, unsubscribed);

        // Note that although we've got a delayed disconnect, we don't need to call AdvanceBy
        // here because the source itself completes. The disconnect is triggered by the source,
        // not the RefCount in this test.
        var s2 = await o2.SubscribeAsync(Scheduler, _ => { });
        Assert.AreEqual(1, subscribed);
        Assert.AreEqual(1, unsubscribed);

        await s1.DisposeAsync();
        await s2.DisposeAsync();

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
        var s3 = await o2.SubscribeAsync(Scheduler, _ => { });
        Assert.AreEqual(1, subscribed);
        Assert.AreEqual(1, unsubscribed);

        // While it might look like adding a second subscriber should tip us back over the threshold
        // and trigger a reconnect, for the reasons described above o2 immediately completed in the
        // last call to subscribe, so the RefCount is zero at this point. This is a limitation of
        // Publish(). It doesn't really matter for this test, but it's why some tests use
        // SerialSingleNotificationConnectable.
        var s4 = await o2.SubscribeAsync(Scheduler, _ => { });
        Assert.AreEqual(1, subscribed);
        Assert.AreEqual(1, unsubscribed);
    }

    [TestMethod]
    public async Task RefCount_DelayedDisconnect_minObservers_NotConnected()
    {
        var connected = 0;
        var source = Seq.Defer(() =>
        {
            connected++;
            return Seq.Never<int>();
        })
        .Publish()
        .RefCount(2, TimeSpan.FromMinutes(1));

        Assert.AreEqual(0, connected);

        await source.SubscribeAsync(Scheduler, _ => { });
        Assert.AreEqual(0, connected);
    }

    [TestMethod]
    public async Task RefCount_DelayedDisconnect_OnError()
    {
        var ex = new Exception();
        var xs = Seq.Throw<int>(ex, ImmediateScheduler);

        var res = xs.Publish().RefCount(TimeSpan.FromSeconds(2));

        await res.SubscribeAsync(Scheduler, _ => throw new Exception(), ex_ => Assert.AreSame(ex, ex_), () => throw new Exception());
        await res.SubscribeAsync(Scheduler, _ => throw new Exception(), ex_ => Assert.AreSame(ex, ex_), () => throw new Exception());
    }

    [TestMethod]
    public async Task RefCount_DelayedDisconnect_minObservers_OnError()
    {
        var ex = new Exception();
        var xs = Seq.Throw<int>(ex, ImmediateScheduler);

        var res = xs.Publish().RefCount(2, TimeSpan.FromSeconds(200));

        var exceptionsReceived = new List<Exception>();

        ValueTask<IAsyncDisposable> AddSubscriber() =>
            res.SubscribeAsync(
                Scheduler,
                _ => Assert.Fail("OnNext unexpected"),
                ex_ => exceptionsReceived.Add(ex),
                () => Assert.Fail("OnComplete unexpected"));

        await AddSubscriber();
        Assert.IsEmpty(exceptionsReceived);

        await AddSubscriber();
        Assert.HasCount(2, exceptionsReceived);
        Assert.AreSame(ex, exceptionsReceived[0]);
        Assert.AreSame(ex, exceptionsReceived[1]);
    }

    [TestMethod]
    public void RefCount_DelayedDisconnect_HotSourceMultipleSubscribers()
    {
        var xs = Scheduler.CreateHotObservable(
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

        var res = xs.Publish().RefCount(TimeSpan.FromTicks(9), Scheduler);

        var d1 = default(IAsyncDisposable);
        var o1 = Scheduler.CreateObserver<int>();
        Scheduler.ScheduleAbsolute(215, async () => { d1 = await res.SubscribeAsync(o1); });
        Scheduler.ScheduleAbsolute(235, async () => { await d1!.DisposeAsync(); });

        var d2 = default(IAsyncDisposable);
        var o2 = Scheduler.CreateObserver<int>();
        Scheduler.ScheduleAbsolute(225, async () => { d2 = await res.SubscribeAsync(o2); });
        Scheduler.ScheduleAbsolute(275, async () =>
        {
            await d2!.DisposeAsync();
        });

        var d3 = default(IAsyncDisposable);
        var o3 = Scheduler.CreateObserver<int>();
        Scheduler.ScheduleAbsolute(255, async () => { d3 = await res.SubscribeAsync(o3); });
        Scheduler.ScheduleAbsolute(265, async () => { await d3!.DisposeAsync(); });

        var d4 = default(IAsyncDisposable);
        var o4 = Scheduler.CreateObserver<int>();
        Scheduler.ScheduleAbsolute(285, async () => { d4 = await res.SubscribeAsync(o4); });
        Scheduler.ScheduleAbsolute(320, async () => { await d4!.DisposeAsync(); });

        Scheduler.Start();

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
        var xs = Scheduler.CreateHotObservable(
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

        var res = xs.Publish().RefCount(2, TimeSpan.FromTicks(9), Scheduler);

        var d1 = default(IAsyncDisposable);
        var o1 = Scheduler.CreateObserver<int>();
        Scheduler.ScheduleAbsolute(215, async () => { d1 = await res.SubscribeAsync(o1); });
        Scheduler.ScheduleAbsolute(235, async () => { await d1!.DisposeAsync(); });

        var d2 = default(IAsyncDisposable);
        var o2 = Scheduler.CreateObserver<int>();
        Scheduler.ScheduleAbsolute(225, async () => { d2 = await res.SubscribeAsync(o2); });
        Scheduler.ScheduleAbsolute(275, async () =>
        {
            await d2!.DisposeAsync();
        });

        var d3 = default(IAsyncDisposable);
        var o3 = Scheduler.CreateObserver<int>();
        Scheduler.ScheduleAbsolute(255, async () => { d3 = await res.SubscribeAsync(o3); });
        Scheduler.ScheduleAbsolute(265, async () => { await d3!.DisposeAsync(); });

        var d4 = default(IAsyncDisposable);
        var o4 = Scheduler.CreateObserver<int>();
        Scheduler.ScheduleAbsolute(285, async () => { d4 = await res.SubscribeAsync(o4); });
        Scheduler.ScheduleAbsolute(320, async () => { await d4!.DisposeAsync(); });

        var d5 = default(IAsyncDisposable);
        var o5 = Scheduler.CreateObserver<int>();
        Scheduler.ScheduleAbsolute(295, async () => { d5 = await res.SubscribeAsync(o5); });
        Scheduler.ScheduleAbsolute(320, async () => { await d5!.DisposeAsync(); });

        Scheduler.Start();

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
    public async Task RefCount_DelayedDisconnect_minObservers_SubscriptionsDropBelowThresholdButNotToZero()
    {
        var subject = CreateReplaySubject<int>(5);
        var connected = 0;
        var source = Seq.Defer(() =>
        {
            connected++;
            return subject;
        })
        .Publish()
        .RefCount(2, TimeSpan.FromMinutes(1));

        await subject.OnNextAsync(1);
        Assert.AreEqual(0, connected);

        var list1 = new List<int>();
        var sub1 = await source.SubscribeAsync(Scheduler, list1.Add);
        Assert.AreEqual(0, connected);
        Assert.IsEmpty(list1);

        await subject.OnNextAsync(2);

        var list2 = new List<int>();
        var sub2 = await source.SubscribeAsync(Scheduler, list2.Add);

        List<int> expectedSub1 = [1, 2];
        var expectedSub2 = expectedSub1;
        CollectionAssert.AreEqual(expectedSub1, list1);
        CollectionAssert.AreEqual(expectedSub1, list2);
        Assert.AreEqual(1, connected);

        await subject.OnNextAsync(3);

        expectedSub1 = expectedSub2 = [1, 2, 3];
        CollectionAssert.AreEqual(expectedSub1, list1);
        CollectionAssert.AreEqual(expectedSub2, list2);
        Assert.AreEqual(1, connected);

        var list3 = new List<int>();
        await source.SubscribeAsync(Scheduler, list3.Add);

        List<int> expectedSub3 = [];
        CollectionAssert.AreEqual(expectedSub1, list1);
        CollectionAssert.AreEqual(expectedSub2, list2);
        CollectionAssert.AreEqual(expectedSub3, list3);
        Assert.AreEqual(1, connected);

        await subject.OnNextAsync(4);

        expectedSub1 = expectedSub2 = [1, 2, 3, 4];
        expectedSub3 = [4];
        CollectionAssert.AreEqual(expectedSub1, list1);
        CollectionAssert.AreEqual(expectedSub2, list2);
        CollectionAssert.AreEqual(expectedSub3, list3);
        Assert.AreEqual(1, connected);

        await sub1.DisposeAsync();
        await subject.OnNextAsync(5);

        expectedSub1 = [1, 2, 3, 4];
        expectedSub2 = [1, 2, 3, 4, 5];
        expectedSub3 = [4, 5];
        CollectionAssert.AreEqual(expectedSub1, list1);
        CollectionAssert.AreEqual(expectedSub2, list2);
        CollectionAssert.AreEqual(expectedSub3, list3);
        Assert.AreEqual(1, connected);

        await sub2.DisposeAsync();
        await subject.OnNextAsync(6);

        expectedSub1 = [1, 2, 3, 4];
        expectedSub2 = [1, 2, 3, 4, 5];
        expectedSub3 = [4, 5, 6];
        CollectionAssert.AreEqual(expectedSub1, list1);
        CollectionAssert.AreEqual(expectedSub2, list2);
        CollectionAssert.AreEqual(expectedSub3, list3);
        Assert.AreEqual(1, connected);
    }

    [TestMethod]
    public async Task RefCount_DelayedDisconnect_SubscriptionsDropBelowThresholdAndThenBackAbove()
    {
        var sourceAfterInitial = CreateSubject<int>();
        var connected = 0;
        var source = Seq.Defer(() =>
        {
            connected++;
            return Seq.Range(1, 5).Concat(sourceAfterInitial);
        })
        .Publish()
        .RefCount(2, TimeSpan.FromTicks(10), Scheduler);

        Assert.AreEqual(0, connected);

        var list1 = new List<int>();
        var sub1 = await source.SubscribeAsync(Scheduler, list1.Add); // 1 subscriber
        Assert.AreEqual(0, connected);
        Assert.IsEmpty(list1);

        var list2 = new List<int>();
        var sub2 = await source.SubscribeAsync(Scheduler, list2.Add); // 2 subscribers
        Assert.AreEqual(1, connected);

        await sourceAfterInitial.OnNextAsync(6);
        await sub1.DisposeAsync(); // 1 subscriber

        Scheduler.AdvanceBy(10);
        Assert.AreEqual(1, connected);

        await sourceAfterInitial.OnNextAsync(7);
        Assert.AreEqual(1, connected);

        var list3 = new List<int>();
        var sub3 = await source.SubscribeAsync(Scheduler, list3.Add);
        Assert.AreEqual(1, connected);

        await sourceAfterInitial.OnNextAsync(8);

        var expectedSub1 = new List<int>([1, 2, 3, 4, 5, 6]);
        var expectedSub2 = new List<int>([1, 2, 3, 4, 5, 6, 7, 8]);
        var expectedSub3 = new List<int>([8]);
        CollectionAssert.AreEqual(expectedSub1, list1);
        CollectionAssert.AreEqual(expectedSub2, list2);
        CollectionAssert.AreEqual(expectedSub3, list3);
    }

    [TestMethod]
    public async Task RefCount_DelayedDisconnect_SubscriptionsDropToZeroThenNewSubscriptionArrivesBeforeDisconnectDelay()
    {
        var source = CreateSerialSingleNotificationConnectable(Notification.CreateOnNext(1));
        var rco = source.RefCount(TimeSpan.FromTicks(10), Scheduler);

        var s1 = await rco.SubscribeAsync(Scheduler, _ => { });
        await s1.DisposeAsync();

        // There are now 0 subscribers, but the time for the disconnect has not yet come.
        Assert.HasCount(1, source.Connections);
        Assert.IsFalse(source.Connections[0].Disposed);

        Scheduler.AdvanceBy(9);

        // The time has still not come,
        Assert.HasCount(1, source.Connections);
        Assert.IsFalse(source.Connections[0].Disposed);

        // Since we were still connected, this should move the connection from a 'waiting to
        // shut down' state into an active state.
        var seen = 0;
        var terminated = false;
        var s2 = await rco.SubscribeAsync(Scheduler, x => seen = x, _ => { }, () => terminated = true);
        await source.DeliverNotificationForActiveConnectionAsync(Notification.CreateOnNext(2));
        Assert.AreEqual(2, seen);
        Assert.IsFalse(terminated);
        Assert.IsFalse(source.Connections[0].Disposed);

        // This moves us past the time when `RefCount` would have shut down the connection if no new
        // subscriptions had turned up.
        Scheduler.AdvanceBy(2);
        Assert.IsFalse(terminated);
        Assert.IsFalse(source.Connections[0].Disposed);

        // We should be able to advance well beyond the disconnect delay because we have an active
        // subscriber.
        Scheduler.AdvanceBy(20);
        Assert.IsFalse(terminated);
        Assert.IsFalse(source.Connections[0].Disposed);
    }

    [TestMethod]
    public async Task RefCount_DelayedDisconnect_minObservers_SubscriptionsDropToZeroThenNewSubscriptionArrivesBeforeDisconnectDelay()
    {
        var source = CreateSerialSingleNotificationConnectable(Notification.CreateOnNext(1));
        var rco = source.RefCount(2, TimeSpan.FromTicks(10), Scheduler);

        var s1 = await rco.SubscribeAsync(Scheduler, _ => { });
        var s2 = await rco.SubscribeAsync(Scheduler, _ => { });
        await s1.DisposeAsync();
        await s2.DisposeAsync();

        Assert.HasCount(1, source.Connections);
        Assert.IsFalse(source.Connections[0].Disposed);

        Scheduler.AdvanceBy(9);

        Assert.HasCount(1, source.Connections);
        Assert.IsFalse(source.Connections[0].Disposed);

        var seen = 0;
        var terminated = false;
        var s3 = await rco.SubscribeAsync(Scheduler, x => seen = x, _ => { }, () => terminated = true);
        await source.DeliverNotificationForActiveConnectionAsync(Notification.CreateOnNext(2));
        Assert.AreEqual(2, seen);
        Assert.IsFalse(terminated);
        Assert.IsFalse(source.Connections[0].Disposed);

        Scheduler.AdvanceBy(2);
        Assert.IsFalse(terminated);
        Assert.IsFalse(source.Connections[0].Disposed);

        Scheduler.AdvanceBy(20);
        Assert.IsFalse(terminated);
        Assert.IsFalse(source.Connections[0].Disposed);
    }

    [TestMethod]
    public async Task RefCount_DelayedDisconnect_ValuesDuringAndAfterSubscribe()
    {
        var subject = CreateReplaySubject<int>(5);
        var source = subject.Publish().RefCount(TimeSpan.FromSeconds(20));

        await subject.OnNextAsync(1);

        List<int> expected1 = [1];
        var list1 = new List<int>();
        await source.SubscribeAsync(Scheduler, list1.Add);
        CollectionAssert.AreEqual(expected1, list1);

        await subject.OnNextAsync(2);
        var list2 = new List<int>();
        await source.SubscribeAsync(Scheduler, list2.Add);

        expected1 = [1, 2];
        List<int> expected2 = [];
        CollectionAssert.AreEqual(expected1, list1);
        CollectionAssert.AreEqual(expected2, list2);

        await subject.OnNextAsync(3);
        var list3 = new List<int>();
        await source.SubscribeAsync(Scheduler, list3.Add);

        expected1 = [1, 2, 3];
        expected2 = [3];
        List<int> expected3 = [];
        CollectionAssert.AreEqual(expected1, list1);
        CollectionAssert.AreEqual(expected2, list2);
        CollectionAssert.AreEqual(expected3, list3);

        await subject.OnNextAsync(4);

        expected1 = [1, 2, 3, 4];
        expected2 = [3, 4];
        expected3 = [4];
        CollectionAssert.AreEqual(expected1, list1);
        CollectionAssert.AreEqual(expected2, list2);
        CollectionAssert.AreEqual(expected3, list3);
    }

    [TestMethod]
    public async Task RefCount_DelayedDisconnect_minObservers_ValuesDuringAndAfterSubscribe()
    {
        var subject = CreateReplaySubject<int>(5);
        var source = subject.Publish().RefCount(2, TimeSpan.FromSeconds(20));

        await subject.OnNextAsync(1);

        var list1 = new List<int>();
        await source.SubscribeAsync(Scheduler, list1.Add);
        Assert.IsEmpty(list1);

        await subject.OnNextAsync(2);
        List<int> expected1and2 = [1, 2];
        var list2 = new List<int>();
        await source.SubscribeAsync(Scheduler, list2.Add);

        CollectionAssert.AreEqual(expected1and2, list1);
        CollectionAssert.AreEqual(expected1and2, list2);

        await subject.OnNextAsync(3);

        expected1and2 = [1, 2, 3];
        CollectionAssert.AreEqual(expected1and2, list1);
        CollectionAssert.AreEqual(expected1and2, list2);

        var list3 = new List<int>();
        await source.SubscribeAsync(Scheduler, list3.Add);

        List<int> expected3 = [];
        CollectionAssert.AreEqual(expected1and2, list1);
        CollectionAssert.AreEqual(expected1and2, list2);
        CollectionAssert.AreEqual(expected3, list3);

        await subject.OnNextAsync(4);

        expected1and2 = [1, 2, 3, 4];
        expected3 = [4];
        CollectionAssert.AreEqual(expected1and2, list1);
        CollectionAssert.AreEqual(expected1and2, list2);
        CollectionAssert.AreEqual(expected3, list3);
    }

    [TestMethod]
    public Task RefCount_DelayedDisconnect_CanConnectAgainIfPreviousSubscriptionTerminatedFromSubscribeByCompletion_ReSubscribeBefore() =>
        RefCount_DelayedDisconnect_CanConnectAgainIfPreviousSubscriptionTerminatedFromSubscribeByCompletion(true);

    [TestMethod]
    public Task RefCount_DelayedDisconnect_CanConnectAgainIfPreviousSubscriptionTerminatedFromSubscribeByCompletion_ReSubscribeAfter() =>
        RefCount_DelayedDisconnect_CanConnectAgainIfPreviousSubscriptionTerminatedFromSubscribeByCompletion(false);

    // A [DataRow] pair in the original; the suite keeps to plain [TestMethod]s.
    private async Task RefCount_DelayedDisconnect_CanConnectAgainIfPreviousSubscriptionTerminatedFromSubscribeByCompletion(
        bool reSubscribeBeforeDelayedDisconnect)
    {
        var seen = 0;
        var terminated = false;

        // On initial subscription, the source will produce one value and will not complete.
        var connectable = CreateSerialSingleNotificationConnectable(Notification.CreateOnNext(36));
        var refCount = connectable.RefCount(TimeSpan.FromTicks(10), Scheduler);

        await using (await refCount.SubscribeAsync(Scheduler, value => seen = value, _ => { }, () => terminated = true))
        {
            Assert.AreEqual(36, seen);
            Assert.HasCount(1, connectable.Connections);
            Assert.IsFalse(connectable.Connections[0].Disposed);
        }

        // Disposing the subscription doesn't disconnect immediately because of the delay.
        Assert.IsFalse(connectable.Connections[0].Disposed);
        Scheduler.AdvanceBy(11);
        Assert.HasCount(1, connectable.Connections);
        Assert.IsTrue(connectable.Connections[0].Disposed);

        seen = 0;
        terminated = false;

        // This time around, the source will complete when subscribed to.
        connectable.SetNotificationForNextConnect(Notification.CreateOnCompleted<int>());
        await using (await refCount.SubscribeAsync(Scheduler, value => seen = value, _ => { }, () => terminated = true))
        {
            Assert.AreEqual(0, seen);
            Assert.IsTrue(terminated);
            Assert.HasCount(2, connectable.Connections);
            Assert.IsFalse(connectable.Connections[1].Disposed);
        }

        Assert.HasCount(2, connectable.Connections);
        Assert.IsFalse(connectable.Connections[1].Disposed);
        Scheduler.AdvanceBy(reSubscribeBeforeDelayedDisconnect ? 1 : 11);
        Assert.HasCount(2, connectable.Connections);
        Assert.AreEqual(!reSubscribeBeforeDelayedDisconnect, connectable.Connections[1].Disposed);

        seen = 0;
        terminated = false;

        // Now we go back to the initial behaviour in which the source produces one value and does not complete.
        connectable.SetNotificationForNextConnect(Notification.CreateOnNext(42));
        await using (await refCount.SubscribeAsync(Scheduler, value => seen = value, _ => { }, () => terminated = true))
        {
            Assert.AreEqual(reSubscribeBeforeDelayedDisconnect ? 0 : 42, seen);
            Assert.AreEqual(reSubscribeBeforeDelayedDisconnect, terminated);
            Assert.HasCount(reSubscribeBeforeDelayedDisconnect ? 2 : 3, connectable.Connections);
            Assert.IsFalse(connectable.Connections[reSubscribeBeforeDelayedDisconnect ? 1 : 2].Disposed);
        }
    }

    [TestMethod]
    public async Task RefCount_DelayedDisconnect_minObservers_CanConnectAgainIfPreviousSubscriptionTerminatedFromSubscribeByCompletionAndEnoughTimeForDisconnectHasPassed()
    {
        var seen1 = 0;
        var seen2 = 0;
        var terminated1 = false;
        var terminated2 = false;

        // On initial subscription, the source will produce one value and will not complete.
        var connectable = CreateSerialSingleNotificationConnectable(Notification.CreateOnNext(36));
        var refCount = connectable.RefCount(2, TimeSpan.FromTicks(10), Scheduler);

        await using (await refCount.SubscribeAsync(Scheduler, value => seen1 = value, _ => { }, () => terminated1 = true))
        {
            Assert.AreEqual(0, seen1);
            Assert.IsEmpty(connectable.Connections);
            await using (await refCount.SubscribeAsync(Scheduler, value => seen2 = value, _ => { }, () => terminated2 = true))
            {
                Assert.AreEqual(36, seen1);
                Assert.AreEqual(36, seen2);
                Assert.HasCount(1, connectable.Connections);
                Assert.IsFalse(connectable.Connections[0].Disposed);
            }
        }

        Assert.HasCount(1, connectable.Connections);
        Assert.IsFalse(connectable.Connections[0].Disposed);
        Scheduler.AdvanceBy(11);
        Assert.HasCount(1, connectable.Connections);
        Assert.IsTrue(connectable.Connections[0].Disposed);

        seen1 = seen2 = 0;
        terminated1 = terminated2 = false;

        // This time around, the source will complete when subscribed to.
        connectable.SetNotificationForNextConnect(Notification.CreateOnCompleted<int>());
        await using (await refCount.SubscribeAsync(Scheduler, value => seen1 = value, _ => { }, () => terminated1 = true))
        {
            Assert.HasCount(1, connectable.Connections);
            Assert.IsFalse(terminated1);
            Assert.IsFalse(terminated2);
            await using (await refCount.SubscribeAsync(Scheduler, value => seen2 = value, _ => { }, () => terminated2 = true))
            {
                Assert.AreEqual(0, seen1);
                Assert.AreEqual(0, seen2);
                Assert.IsTrue(terminated1);
                Assert.IsTrue(terminated2);
                Assert.HasCount(2, connectable.Connections);
                Assert.IsFalse(connectable.Connections[1].Disposed);
            }
        }

        Assert.HasCount(2, connectable.Connections);
        Assert.IsFalse(connectable.Connections[1].Disposed);
        Scheduler.AdvanceBy(11);
        Assert.HasCount(2, connectable.Connections);
        Assert.IsTrue(connectable.Connections[1].Disposed);

        seen1 = seen2 = 0;
        terminated1 = terminated2 = false;

        // Now we go back to the initial behaviour in which the source produces one value and does not complete.
        connectable.SetNotificationForNextConnect(Notification.CreateOnNext(42));
        await using (await refCount.SubscribeAsync(Scheduler, value => seen1 = value, _ => { }, () => terminated1 = true))
        {
            Assert.IsFalse(terminated1);
            Assert.AreEqual(0, seen1);
            Assert.IsFalse(terminated2);
            Assert.HasCount(2, connectable.Connections);
            await using (await refCount.SubscribeAsync(Scheduler, value => seen2 = value, _ => { }, () => terminated2 = true))
            {
                Assert.AreEqual(42, seen1);
                Assert.AreEqual(42, seen2);
                Assert.IsFalse(terminated1);
                Assert.IsFalse(terminated2);
                Assert.HasCount(3, connectable.Connections);
                Assert.IsFalse(connectable.Connections[2].Disposed);
            }
        }
    }

    [TestMethod]
    public async Task RefCount_DelayedDisconnect_minObservers_CanConnectAgainIfPreviousSubscriptionTerminatedFromSubscribeByCompletionAndEnoughTimeForDisconnectHasPassed_WithPreConnectNotifications()
    {
        var seen1 = 0;
        var seen2 = 0;
        var terminated1 = false;
        var terminated2 = false;

        var connectable = CreateSerialConnectableIgnoringConnect(CreateBehaviorSubject(36));
        var refCount = connectable.RefCount(2, TimeSpan.FromTicks(10), Scheduler);

        await using (await refCount.SubscribeAsync(Scheduler, value => seen1 = value, _ => { }, () => terminated1 = true))
        {
            Assert.AreEqual(36, seen1);
            Assert.IsEmpty(connectable.Connections);
            await using (await refCount.SubscribeAsync(Scheduler, value => seen2 = value, _ => { }, () => terminated2 = true))
            {
                Assert.AreEqual(36, seen1);
                Assert.AreEqual(36, seen2);
                Assert.HasCount(1, connectable.Connections);
                Assert.IsFalse(connectable.Connections[0].Disposed);
            }
        }

        Assert.HasCount(1, connectable.Connections);
        Assert.IsFalse(connectable.Connections[0].Disposed);
        Scheduler.AdvanceBy(11);
        Assert.HasCount(1, connectable.Connections);
        Assert.IsTrue(connectable.Connections[0].Disposed);

        seen1 = seen2 = 0;
        terminated1 = terminated2 = false;

        connectable.SetSource(Seq.Empty<int>());
        await using (await refCount.SubscribeAsync(Scheduler, value => seen1 = value, _ => { }, () => terminated1 = true))
        {
            Assert.IsTrue(terminated1);
            Assert.IsFalse(terminated2);
            Assert.HasCount(1, connectable.Connections);
            await using (await refCount.SubscribeAsync(Scheduler, value => seen2 = value, _ => { }, () => terminated2 = true))
            {
                Assert.AreEqual(0, seen1);
                Assert.AreEqual(0, seen2);
                Assert.IsTrue(terminated1);
                Assert.IsTrue(terminated2);
                Assert.HasCount(1, connectable.Connections);
                Assert.IsTrue(connectable.Connections[0].Disposed);
            }
        }

        Assert.HasCount(1, connectable.Connections);
        Scheduler.AdvanceBy(11);
        Assert.HasCount(1, connectable.Connections);

        seen1 = seen2 = 0;
        terminated1 = terminated2 = false;

        connectable.SetSource(CreateBehaviorSubject(42));
        await using (await refCount.SubscribeAsync(Scheduler, value => seen1 = value, _ => { }, () => terminated1 = true))
        {
            Assert.IsFalse(terminated1);
            Assert.AreEqual(42, seen1);
            Assert.IsFalse(terminated2);
            Assert.HasCount(1, connectable.Connections);
            await using (await refCount.SubscribeAsync(Scheduler, value => seen2 = value, _ => { }, () => terminated2 = true))
            {
                Assert.AreEqual(42, seen1);
                Assert.AreEqual(42, seen2);
                Assert.IsFalse(terminated1);
                Assert.IsFalse(terminated2);
                Assert.HasCount(2, connectable.Connections);
                Assert.IsFalse(connectable.Connections[1].Disposed);
            }
        }
    }

    [TestMethod]
    public async Task RefCount_DelayedDisconnect_minObservers_DoesNotConnectAgainIfPreviousSubscriptionTerminatedFromSubscribeByCompletionButNotEnoughTimeForDelayedDisconnectHasPassed()
    {
        var seen1 = 0;
        var seen2 = 0;
        var terminated1 = false;
        var terminated2 = false;

        // On initial subscription, the source will produce one value and will not complete.
        var connectable = CreateSerialSingleNotificationConnectable(Notification.CreateOnNext(36));
        var refCount = connectable.RefCount(2, TimeSpan.FromTicks(10), Scheduler);

        await using (await refCount.SubscribeAsync(Scheduler, value => seen1 = value, _ => { }, () => terminated1 = true))
        {
            Assert.AreEqual(0, seen1);
            Assert.IsEmpty(connectable.Connections);
            await using (await refCount.SubscribeAsync(Scheduler, value => seen2 = value, _ => { }, () => terminated2 = true))
            {
                Assert.AreEqual(36, seen1);
                Assert.AreEqual(36, seen2);
                Assert.HasCount(1, connectable.Connections);
                Assert.IsFalse(connectable.Connections[0].Disposed);
            }
        }

        Assert.HasCount(1, connectable.Connections);
        Assert.IsFalse(connectable.Connections[0].Disposed);
        Scheduler.AdvanceBy(11);
        Assert.HasCount(1, connectable.Connections);
        Assert.IsTrue(connectable.Connections[0].Disposed);

        seen1 = seen2 = 0;
        terminated1 = terminated2 = false;

        // This time around, the source will complete when subscribed to.
        connectable.SetNotificationForNextConnect(Notification.CreateOnCompleted<int>());
        await using (await refCount.SubscribeAsync(Scheduler, value => seen1 = value, _ => { }, () => terminated1 = true))
        {
            Assert.HasCount(1, connectable.Connections);
            Assert.IsFalse(terminated1);
            await using (await refCount.SubscribeAsync(Scheduler, value => seen2 = value, _ => { }, () => terminated2 = true))
            {
                Assert.AreEqual(0, seen1);
                Assert.AreEqual(0, seen2);
                Assert.IsTrue(terminated1);
                Assert.IsTrue(terminated2);
                Assert.HasCount(2, connectable.Connections);
                Assert.IsFalse(connectable.Connections[1].Disposed);
            }
        }

        Assert.HasCount(2, connectable.Connections);
        Assert.IsFalse(connectable.Connections[1].Disposed);
        Scheduler.AdvanceBy(5);
        Assert.HasCount(2, connectable.Connections);
        Assert.IsFalse(connectable.Connections[1].Disposed);

        seen1 = seen2 = 0;
        terminated1 = terminated2 = false;

        // Our source has completed, but RefCount hasn't yet disconnected from it (because not
        // enough time has passed). So the connection is live, but completed. The double's
        // subject has completed, so later subscribers would just be completed; replace the
        // connection's source so that they receive a value instead.
        connectable.Connections[1].ReplaceSource(CreateBehaviorSubject(42));
        await using (await refCount.SubscribeAsync(Scheduler, value => seen1 = value, _ => { }, () => terminated1 = true))
        {
            Assert.IsFalse(terminated1);
            Assert.AreEqual(42, seen1);
            Assert.IsFalse(terminated2);
            Assert.HasCount(2, connectable.Connections);
            Assert.IsFalse(connectable.Connections[1].Disposed);
            await using (await refCount.SubscribeAsync(Scheduler, value => seen2 = value, _ => { }, () => terminated2 = true))
            {
                Assert.AreEqual(42, seen1);
                Assert.AreEqual(42, seen2);
                Assert.IsFalse(terminated1);
                Assert.IsFalse(terminated2);
                Assert.HasCount(2, connectable.Connections);
                Assert.IsFalse(connectable.Connections[1].Disposed);
            }
        }

        connectable.SetNotificationForNextConnect(Notification.CreateOnNext(99));

        Scheduler.AdvanceBy(6);
        Assert.HasCount(2, connectable.Connections);
        Assert.IsFalse(connectable.Connections[1].Disposed);
        Scheduler.AdvanceBy(5);
        Assert.HasCount(2, connectable.Connections);
        Assert.IsTrue(connectable.Connections[1].Disposed);

        seen1 = seen2 = 0;
        terminated1 = terminated2 = false;

        await using (await refCount.SubscribeAsync(Scheduler, value => seen1 = value, _ => { }, () => terminated1 = true))
        {
            Assert.AreEqual(0, seen1);
            Assert.HasCount(2, connectable.Connections);
            Assert.IsTrue(connectable.Connections[1].Disposed);
            await using (await refCount.SubscribeAsync(Scheduler, value => seen2 = value, _ => { }, () => terminated2 = true))
            {
                Assert.AreEqual(99, seen1);
                Assert.AreEqual(99, seen2);
                Assert.IsFalse(terminated1);
                Assert.IsFalse(terminated2);
                Assert.HasCount(3, connectable.Connections);
                Assert.IsFalse(connectable.Connections[2].Disposed);
            }
        }
    }

}

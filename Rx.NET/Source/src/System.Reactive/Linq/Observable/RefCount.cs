// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information. 

using System.Diagnostics;
using System.Reactive.Concurrency;
using System.Reactive.Disposables;
using System.Reactive.Subjects;
using System.Threading;

namespace System.Reactive.Linq.ObservableImpl
{
    internal static class RefCount<TSource>
    {
        internal sealed class Eager : Producer<TSource, Eager._>
        {
            private readonly IConnectableObservable<TSource> _source;

            private readonly object _gate = new();

            /// <summary>
            /// Contains the current active connection's state or null
            /// if no connection is active at the moment.
            /// Should be manipulated while holding the <see cref="_gate"/> lock.
            /// </summary>
            private RefConnection? _connection;

            private readonly int _minObservers;

            public Eager(IConnectableObservable<TSource> source, int minObservers)
            {
                _source = source;
                _minObservers = minObservers;
            }

            protected override _ CreateSink(IObserver<TSource> observer) => new(observer, this);

            protected override void Run(_ sink) => sink.Run();

            internal sealed class _ : IdentitySink<TSource>
            {
                private readonly Eager _parent;

                /// <summary>
                /// Contains the connection reference the downstream observer
                /// has subscribed to. Its purpose is to
                /// avoid subscribing, connecting and disconnecting
                /// while holding a lock.
                /// </summary>
                private RefConnection? _targetConnection;

                public _(IObserver<TSource> observer, Eager parent)
                    : base(observer)
                {
                    _parent = parent;
                }

                public void Run()
                {
                    bool doConnect;
                    RefConnection? conn;

                    lock (_parent._gate)
                    {
                        // get the active connection state
                        conn = _parent._connection;

                        // if null, a new connection should be established
                        if (conn == null)
                        {
                            conn = new RefConnection();
                            // make it the active one
                            _parent._connection = conn;
                        }

                        // if this is the first time the observer count has reached the minimum
                        // observer count since we last had no observers, then connect
                        doConnect = ++conn._count == _parent._minObservers && conn._disposable.Disposable is null;

                        // save the current connection for this observer
                        _targetConnection = conn;
                    }

                    // subscribe to the source first
                    Run(_parent._source);

                    // then connect the source if necessary
                    if (doConnect && !conn._disposable.IsDisposed)
                    {
                        // this makes sure if the connection ends synchronously
                        // only the currently known connection is affected
                        // and a connection from a concurrent reconnection won't
                        // interfere
                        conn._disposable.Disposable = _parent._source.Connect();
                    }
                }

                protected override void Dispose(bool disposing)
                {
                    base.Dispose(disposing);

                    if (disposing)
                    {
                        // get and forget the saved connection
                        var targetConnection = _targetConnection!; // NB: Always set by Run prior to calling Dispose, and base class hardens protects against double-dispose.
                        _targetConnection = null;

                        lock (_parent._gate)
                        {
                            // if the current connection is no longer the saved connection
                            // or the counter hasn't reached zero yet
                            if (targetConnection != _parent._connection
                                || --targetConnection._count != 0)
                            {
                                // nothing to do.
                                return;
                            }

                            // forget the current connection
                            _parent._connection = null;
                        }

                        // disconnect
                        targetConnection._disposable.Dispose();
                    }
                }
            }

            /// <summary>
            /// Holds an individual connection state: the observer count and
            /// the connection's IDisposable.
            /// </summary>
            private sealed class RefConnection
            {
                internal int _count;
                internal SingleAssignmentDisposableValue _disposable;
            }
        }

        internal sealed class Lazy : Producer<TSource, Lazy._>
        {
            // This operator's state transitions are easily misunderstood, as bugs #2214 and #2215
            // testify. In particular, there are tricky cases around:
            //
            //  * a transition to 0 subscribers followed by the arrival of a new subscriber before
            //      the disconnect delay elapses
            //  * sources that complete before returning from Connect
            //  * sources that produce notifications without waiting for Connect (especially if
            //      they call OnComplete before returning from Subscribe)
            //
            // This is further complicated by the need to handle multithreading. Although Rx
            // requires notifications to an individual observer to be sequential, there are two
            // reasons concurrency may occur here:
            //
            //  * each subscription to RefCount causes a subscription to the underlying source.
            //      (RefCount only aggregates the calls to Connect. In the common usage of the
            //      form source.Publish().RefCount(), these subscriptions all go to a single
            //      subject, so there won't be concurrent source notifications in practice. But
            //      RefCount works with any IConnectableObservable<T>, and in general, connectable
            //      observables are not required to synchronize notifications across all
            //      subscribers for a particular connection. Since RefCount needs to detect when
            //      sources complete by themselves, it needs to be able to handle concurrent
            //      completions.)
            // * new Subscribe calls to RefCount could happen concurrently with notifications
            //      emerging for existing subscriptions, or concurrently with other calls to
            //      Subscribe. (Strictly speaking, we've never promised that the latter will work.
            //      The documentation does not tell you that it's OK for multiple threads to make
            //      concurrent calls to Subscribe on a single RefCount. However, historically we
            //      have always guarded against such calls, so for backwards compatibility we must
            //      continue to do so.)
            //
            // Each call to RefCount(disconnectDelay) creates a single instance of this Lazy class.
            // Then we get one instance of the nested Lazy._ sink for each subscriber. The outer
            // Lazy class will be in one of the states described in the State enumeration.
            //
            // State transitions are tricky. Although we can use a lock to protect against
            // concurrency, re-entrancy causes problems: when we call Subscribe or Connect, those
            // can complete subscribers (either ones already set up, or the one we're trying to set
            // up when calling Subscribe or Connect). So even though we may hold a lock to protect
            // the operator's state, calling these methods can cause completion to occur, and the
            // completion logic may try to acquire the same lock. It will succeed (because
            // re-entrant lock acquisition is supported, since the alternative is deadlock or
            // failure) and so we end up with a block of code owning a lock and modifying data
            // protected by that lock right in the middle of the execution of another block of code
            // that also owns that same lock. That is exactly the situation we normally expect lock
            // to prevent, but it can't help us when re-entrancy occurs. So we typically want to
            // avoid any calls that could trigger such re-entrancy while updating shared state, but
            // that's not always possible, so in cases where we need to call out to user code while
            // holding the _gate lock, we need to remember that state might have changed during
            // that call.

            private readonly object _gate;
            private readonly IScheduler _scheduler;
            private readonly TimeSpan _disconnectTime;
            private readonly IConnectableObservable<TSource> _source;
            private readonly int _minObservers;

            private State _state;
            private IDisposable? _serial;
            private int _count;
            private IDisposable? _connectableSubscription;

            /// <summary>
            /// Represents the <see cref="Lazy"/> instances state (shared across all subscriptions
            /// to that instance).
            /// </summary>
            private enum State
            {
                /// <summary>
                /// Disconnected with 0 subscribers. This is the initial state, and also the state
                /// we return to after the subscriber count drops to zero, and the disconnect delay
                /// elapses without further subscriptions being added).
                /// </summary>
                DisconnectedNoSubscribers,

                /// <summary>
                /// Disconnected with 0 &lt; subscribers &lt; minObservers. This is the state we
                /// enter when we get our first subscriber and minObservers is >= 2).
                /// </summary>
                DisconnectedWithSubscribers,

                /// <summary>
                /// Connected with at least one subscriber. We enter this state when the number of
                /// subscribers first reaches minObservers (or when it reached it again after
                /// disconnecting). If minObservers = 1, we enter this state from
                /// <see cref="DisconnectedNoSubscribers"/> as soon as we get a subscriber.
                /// </summary>
                ConnectedWithSubscribers,

                /// <summary>
                /// Connected with 0 subscribers, and waiting for the disconnect delay to elapse,
                /// or for new subscriptions come in before that delay completes.
                /// </summary>
                ConnectedWithNoSubscribers
            }

            public Lazy(IConnectableObservable<TSource> source, TimeSpan disconnectTime, IScheduler scheduler, int minObservers)
            {
                _source = source;
                _gate = new object();
                _disconnectTime = disconnectTime;
                _scheduler = scheduler;
                _minObservers = minObservers;
                _state = State.DisconnectedNoSubscribers;
            }

            protected override _ CreateSink(IObserver<TSource> observer) => new(observer);

            protected override void Run(_ sink) => sink.Run(this);

            internal sealed class _ : IdentitySink<TSource>
            {
                public _(IObserver<TSource> observer)
                    : base(observer)
                {
                }

                public void Run(Lazy parent)
                {
                    // The source might complete synchronously, so it's possible that we might be
                    // in a Disposed state for the remainder of the method. This is expected, but
                    // anyone planning to modify this code needs to bear that in mind.
                    var subscription = parent._source.SubscribeSafe(this);

                    lock (parent._gate)
                    {
                        var observerCount = ++parent._count;
                        var shouldConnect = false;
                        var shouldCancelDelayedDisconnect = false;
                        switch (parent._state)
                        {
                            case State.DisconnectedNoSubscribers:
                            case State.DisconnectedWithSubscribers:
                                Debug.Assert(observerCount <= parent._minObservers, "RefCount should never exceed minObservers without already having connected");
                                shouldConnect = observerCount == parent._minObservers;
                                parent._state = shouldConnect ? State.ConnectedWithSubscribers : State.DisconnectedWithSubscribers;
                                break;

                            // If we're ConnectedWithSubscribers, we have no further work to do.

                            case State.ConnectedWithNoSubscribers:
                                shouldCancelDelayedDisconnect = true;
                                parent._state = State.ConnectedWithSubscribers;
                                break;
                        }

                        if (shouldConnect)
                        {
                            Debug.Assert(parent._connectableSubscription is null, "RefCount already connected when it should not be");
                            parent._connectableSubscription = parent._source.Connect();

                            // That call to Connect can cause the underlying source to complete. If
                            // there were already subscribers (e.g., we have minObservers > 1, and
                            // this latest subscription is the one that hit the minObserver
                            // threshold), those would be completed inside this call, meaning that
                            // the Disposable.Create callbacks they set as their upstreams will
                            // run. That callback (see later in this method) acquires the same
                            // _gate lock that we currently hold, so we need to be aware that
                            // our fields could change during this call even though we own the lock.
                            //
                            // That said, it shouldn't change _state: those callbacks will only
                            // disconnect if the observer count drops to zero, and since that count
                            // includes the subscription currently being established, for which
                            // we've not yet registered the upstream disposable, there should still
                            // be at least one subscriber right now.
                            Debug.Assert(parent._state == State.ConnectedWithSubscribers, "Unexpected state change in Connect");
                        }

                        if (shouldConnect || shouldCancelDelayedDisconnect)
                        {
                            // If a delayed disconnect work item has been scheduled, it will
                            // already be in _serial, so this will cancel it. In any case, this
                            // ensures that an unused SingleAssignmentDisposable is available for
                            // the upstream disposal callback to use when it needs to set up the
                            // delayed disconnect work item.
                            Disposable.TrySetSerial(ref parent._serial, new SingleAssignmentDisposable());
                        }
                    }

                    SetUpstream(Disposable.Create(
                        (parent, subscription),
                        static tuple =>
                        {
                            var (closureParent, closureSubscription) = tuple;

                            closureSubscription.Dispose();

                            lock (closureParent._gate)
                            {
                                if (--closureParent._count == 0)
                                {
                                    // It's possible for the count to reach 0 without ever having
                                    // gone above the minObservers threshold, in which case we
                                    // won't ever have called Connect. More subtly, when sources
                                    // call OnComplete inside Subscribe, it's possible for this
                                    // Disposable callback to run *inside* the call to Connect
                                    // above.
                                    // So we only want to schedule the disconnection work item if
                                    // we have already connected.
                                    if (closureParent._state == State.ConnectedWithSubscribers)
                                    {
                                        closureParent._state = State.ConnectedWithNoSubscribers;

                                        // NB: _serial is guaranteed to be set by TrySetSerial earlier on.
                                        var cancelable = (SingleAssignmentDisposable)Volatile.Read(ref closureParent._serial)!;

                                        cancelable.Disposable = closureParent._scheduler.ScheduleAction((cancelable, closureParent), closureParent._disconnectTime, static tuple2 =>
                                        {
                                            lock (tuple2.closureParent._gate)
                                            {
                                                if (ReferenceEquals(Volatile.Read(ref tuple2.closureParent._serial), tuple2.cancelable))
                                                {
                                                    tuple2.closureParent._state = State.DisconnectedNoSubscribers;

                                                    // NB: _connectableSubscription is guaranteed to be set above, and Disposable.Create protects against double-dispose.
                                                    var connectableSubscription = tuple2.closureParent._connectableSubscription!;
                                                    tuple2.closureParent._connectableSubscription = null;
                                                    connectableSubscription.Dispose();
                                                }
                                            }
                                        });
                                    }
                                    else // closureParent._state == State.ConnectedWithSubscribers
                                    {
                                        // This callback should only run when we have at least one subscriber,
                                        // so if we weren't in ConnectedWithSubscribers, we'd should be in
                                        // DisconnectedWithSubscribers.
                                        Debug.Assert(closureParent._state == State.DisconnectedWithSubscribers);

                                        closureParent._state = State.DisconnectedNoSubscribers;
                                    }
                                }
                            }
                        }));
                }
            }
        }
    }
}

// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the Apache 2.0 License.
// See the LICENSE file in the project root for more information. 

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

            private readonly object _gate;
            /// <summary>
            /// Contains the current active connection's state or null
            /// if no connection is active at the moment.
            /// Should be manipulated while holding the <see cref="_gate"/> lock.
            /// </summary>
            private RefConnection _connection;

            private readonly int _minObservers;

            public Eager(IConnectableObservable<TSource> source, int minObservers)
            {
                _source = source;
                _gate = new object();
                _minObservers = minObservers;
            }

            protected override _ CreateSink(IObserver<TSource> observer) => new _(observer, this);

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
                private RefConnection _targetConnection;

                public _(IObserver<TSource> observer, Eager parent)
                    : base(observer)
                {
                    _parent = parent;
                }

                public void Run()
                {
                    var doConnect = false;
                    var conn = default(RefConnection);

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

                        // this is the first observer, then connect
                        doConnect = ++conn._count == _parent._minObservers;
                        // save the current connection for this observer
                        _targetConnection = conn;
                    }

                    // subscribe to the source first
                    Run(_parent._source);
                    // then connect the source if necessary
                    if (doConnect && !Disposable.GetIsDisposed(ref conn._disposable))
                    {
                        // this makes sure if the connection ends synchronously
                        // only the currently known connection is affected
                        // and a connection from a concurrent reconnection won't
                        // interfere
                        Disposable.SetSingle(ref conn._disposable, _parent._source.Connect());
                    }
                }

                protected override void Dispose(bool disposing)
                {
                    base.Dispose(disposing);
                    if (disposing)
                    {
                        // get and forget the saved connection
                        var targetConnection = _targetConnection;
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
                        Disposable.TryDispose(ref targetConnection._disposable);
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
                internal IDisposable _disposable;
            }
        }

        internal sealed class Lazy : Producer<TSource, Lazy._>
        {
            private readonly object _gate;
            private readonly IScheduler _scheduler;
            private readonly TimeSpan _disconnectTime;
            private readonly IConnectableObservable<TSource> _source;
            private readonly int _minObservers;

            private IDisposable _serial;
            private int _count;
            private IDisposable _connectableSubscription;

            public Lazy(IConnectableObservable<TSource> source, TimeSpan disconnectTime, IScheduler scheduler, int minObservers)
            {
                _source = source;
                _gate = new object();
                _disconnectTime = disconnectTime;
                _scheduler = scheduler;
                _minObservers = minObservers;
            }

            protected override _ CreateSink(IObserver<TSource> observer) => new _(observer);

            protected override void Run(_ sink) => sink.Run(this);

            internal sealed class _ : IdentitySink<TSource>
            {
                public _(IObserver<TSource> observer)
                    : base(observer)
                {
                }

                public void Run(Lazy parent)
                {
                    var subscription = parent._source.SubscribeSafe(this);

                    lock (parent._gate)
                    {
                        if (++parent._count == parent._minObservers)
                        {
                            if (parent._connectableSubscription == null)
                            {
                                parent._connectableSubscription = parent._source.Connect();
                            }

                            Disposable.TrySetSerial(ref parent._serial, new SingleAssignmentDisposable());
                        }
                    }

                    SetUpstream(Disposable.Create(
                        (parent, subscription),
                        tuple =>
                        {
                            var (closureParent, closureSubscription) = tuple;

                            closureSubscription.Dispose();

                            lock (closureParent._gate)
                            {
                                if (--closureParent._count == 0)
                                {
                                    var cancelable = (SingleAssignmentDisposable)Volatile.Read(ref closureParent._serial);

                                    cancelable.Disposable = closureParent._scheduler.ScheduleAction((cancelable, closureParent), closureParent._disconnectTime, tuple2 =>
                                    {
                                        lock (tuple2.closureParent._gate)
                                        {
                                            if (ReferenceEquals(Volatile.Read(ref tuple2.closureParent._serial), tuple2.cancelable))
                                            {
                                                tuple2.closureParent._connectableSubscription.Dispose();
                                                tuple2.closureParent._connectableSubscription = null;
                                            }
                                        }
                                    });
                                }
                            }
                        }));
                }
            }
        }
    }
}

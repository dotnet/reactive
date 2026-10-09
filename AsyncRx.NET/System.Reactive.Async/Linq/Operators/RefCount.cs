// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information. 

using System.Reactive.Concurrency;
using System.Reactive.Disposables;
using System.Reactive.Subjects;
using System.Threading;

namespace System.Reactive.Linq
{
    public partial class AsyncObservable
    {
        public static IAsyncObservable<TSource> RefCount<TSource>(this IConnectableAsyncObservable<TSource> source)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));

            return RefCount(source, 1);
        }

        public static IAsyncObservable<TSource> RefCount<TSource>(this IConnectableAsyncObservable<TSource> source, int minObservers)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));
            if (minObservers <= 0)
                throw new ArgumentOutOfRangeException(nameof(minObservers));

            // A port of Rx.NET's eager RefCount: one connection state at a time, counting the
            // observers subscribed through it. The count reaching minObservers for the first time
            // connects; the count reaching zero disconnects and forgets the state, so the next
            // subscriber starts a fresh one. A subscription records which state it belongs to, so
            // that a subscriber of an earlier, already disconnected state cannot disturb a later one.
            var gate = new AsyncGate();
            var connection = default(RefConnection);

            return Create<TSource>(async observer =>
            {
                RefConnection target;
                bool connect;

                using (await gate.LockAsync().ConfigureAwait(false))
                {
                    target = connection ??= new RefConnection();
                    // Connect if this subscriber takes the count to the threshold for the first
                    // time in this state. The count can cross the threshold again later (drop to
                    // one, then back to two) while still connected; that must not reconnect.
                    connect = ++target.Count == minObservers && !target.Connected;
                }

                // Subscribe to the source first, through an observer that notices termination.
                // The source may terminate this subscriber inside SubscribeAsync (a completed
                // Publish subject completes new subscribers at once); then the count must fall
                // before we decide whether to connect, as Rx.NET's eager RefCount does, whose
                // sink is the observer and disposes itself on termination.
                var terminated = false;
                var subscription = await source.SubscribeSafeAsync(AsyncObserver.Create<TSource>(
                    observer.OnNextAsync,
                    async ex =>
                    {
                        terminated = true;
                        await observer.OnErrorAsync(ex).ConfigureAwait(false);
                    },
                    async () =>
                    {
                        terminated = true;
                        await observer.OnCompletedAsync().ConfigureAwait(false);
                    })).ConfigureAwait(false);

                var disposed = 0;
                var disposal = AsyncDisposable.Create(async () =>
                {
                    if (Interlocked.Exchange(ref disposed, 1) != 0)
                    {
                        return;
                    }

                    await subscription.DisposeAsync().ConfigureAwait(false);

                    IAsyncDisposable toDisconnect = null;

                    using (await gate.LockAsync().ConfigureAwait(false))
                    {
                        if (ReferenceEquals(target, connection) && --target.Count == 0)
                        {
                            connection = null;
                            target.Retired = true;
                            toDisconnect = target.Disposable;
                            target.Disposable = null;
                        }
                    }

                    if (toDisconnect != null)
                    {
                        await toDisconnect.DisposeAsync().ConfigureAwait(false);
                    }
                });

                if (terminated)
                {
                    await disposal.DisposeAsync().ConfigureAwait(false);
                }

                if (connect)
                {
                    bool stillCurrent;
                    using (await gate.LockAsync().ConfigureAwait(false))
                    {
                        stillCurrent = !target.Retired;
                        if (stillCurrent)
                        {
                            target.Connected = true;
                        }
                    }

                    if (stillCurrent)
                    {
                        var connected = await source.ConnectAsync().ConfigureAwait(false);

                        bool disposeNow;
                        using (await gate.LockAsync().ConfigureAwait(false))
                        {
                            // Connecting may have completed every subscriber synchronously and
                            // retired the state; the connection must then be released now.
                            disposeNow = target.Retired;
                            if (!disposeNow)
                            {
                                target.Disposable = connected;
                            }
                        }

                        if (disposeNow)
                        {
                            await connected.DisposeAsync().ConfigureAwait(false);
                        }
                    }
                }

                return disposal;
            });
        }

        public static IAsyncObservable<TSource> RefCount<TSource>(this IConnectableAsyncObservable<TSource> source, TimeSpan disconnectDelay)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));

            return RefCount(source, 1, disconnectDelay, TaskPoolAsyncScheduler.Default);
        }

        public static IAsyncObservable<TSource> RefCount<TSource>(this IConnectableAsyncObservable<TSource> source, TimeSpan disconnectDelay, IAsyncScheduler scheduler)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));
            if (scheduler == null)
                throw new ArgumentNullException(nameof(scheduler));

            return RefCount(source, 1, disconnectDelay, scheduler);
        }

        public static IAsyncObservable<TSource> RefCount<TSource>(this IConnectableAsyncObservable<TSource> source, int minObservers, TimeSpan disconnectDelay)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));
            if (minObservers <= 0)
                throw new ArgumentOutOfRangeException(nameof(minObservers));

            return RefCount(source, minObservers, disconnectDelay, TaskPoolAsyncScheduler.Default);
        }

        public static IAsyncObservable<TSource> RefCount<TSource>(this IConnectableAsyncObservable<TSource> source, int minObservers, TimeSpan disconnectDelay, IAsyncScheduler scheduler)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));
            if (minObservers <= 0)
                throw new ArgumentOutOfRangeException(nameof(minObservers));
            if (scheduler == null)
                throw new ArgumentNullException(nameof(scheduler));

            // A port of Rx.NET's delayed-disconnect RefCount, whose state machine is shared across
            // all subscriptions to one instance (see the State enum). Connecting may complete
            // subscribers synchronously, which runs their disposal re-entrantly under the same
            // gate; that is why the state is re-read after Connect rather than assumed.
            var gate = new AsyncGate();
            var state = LazyState.DisconnectedNoSubscribers;
            var count = 0;
            var connection = default(IAsyncDisposable);
            var pendingDisconnect = default(IAsyncDisposable);
            var pendingDisconnectId = 0L;

            return Create<TSource>(async observer =>
            {
                var terminated = false;
                var subscription = await source.SubscribeSafeAsync(AsyncObserver.Create<TSource>(
                    observer.OnNextAsync,
                    async ex =>
                    {
                        terminated = true;
                        await observer.OnErrorAsync(ex).ConfigureAwait(false);
                    },
                    async () =>
                    {
                        terminated = true;
                        await observer.OnCompletedAsync().ConfigureAwait(false);
                    })).ConfigureAwait(false);

                var disposed = 0;
                var disposal = AsyncDisposable.Create(async () =>
                {
                    if (Interlocked.Exchange(ref disposed, 1) != 0)
                    {
                        return;
                    }

                    await subscription.DisposeAsync().ConfigureAwait(false);

                    using (await gate.LockAsync().ConfigureAwait(false))
                    {
                        if (--count != 0)
                        {
                            return;
                        }

                        if (state == LazyState.ConnectedWithSubscribers)
                        {
                            // Nothing more may arrive before the delay: keep the connection, and
                            // schedule the disconnection; a new subscriber cancels it by
                            // bumping the id, so a stale work item finds its id superseded.
                            state = LazyState.ConnectedWithNoSubscribers;
                            var id = ++pendingDisconnectId;
                            pendingDisconnect = await scheduler.ScheduleAsync(id, disconnectDelay, async (myId, ct) =>
                            {
                                IAsyncDisposable toDispose = null;
                                using (await gate.LockAsync().RendezVous(scheduler, ct))
                                {
                                    if (pendingDisconnectId == myId && state == LazyState.ConnectedWithNoSubscribers)
                                    {
                                        state = LazyState.DisconnectedNoSubscribers;
                                        toDispose = connection;
                                        connection = null;
                                    }
                                }

                                if (toDispose != null)
                                {
                                    await toDispose.DisposeAsync().RendezVous(scheduler, ct);
                                }
                            }).ConfigureAwait(false);
                        }
                        else
                        {
                            // Below the threshold and never connected in this cycle.
                            state = LazyState.DisconnectedNoSubscribers;
                        }
                    }
                });

                if (terminated)
                {
                    // The source completed this subscriber inside SubscribeAsync, so the count
                    // it will contribute has already come and gone; account for it before we
                    // decide whether its arrival connects. The count is incremented below.
                }

                bool shouldConnect;
                using (await gate.LockAsync().ConfigureAwait(false))
                {
                    count++;
                    shouldConnect = false;
                    switch (state)
                    {
                        case LazyState.DisconnectedNoSubscribers:
                        case LazyState.DisconnectedWithSubscribers:
                            shouldConnect = count == minObservers;
                            state = shouldConnect ? LazyState.ConnectedWithSubscribers : LazyState.DisconnectedWithSubscribers;
                            break;
                        case LazyState.ConnectedWithNoSubscribers:
                            // A subscriber arrived before the delayed disconnect: cancel it.
                            state = LazyState.ConnectedWithSubscribers;
                            pendingDisconnectId++;
                            break;
                    }
                }

                if (shouldConnect)
                {
                    var connected = await source.ConnectAsync().ConfigureAwait(false);

                    IAsyncDisposable disposeNow = null;
                    using (await gate.LockAsync().ConfigureAwait(false))
                    {
                        // Connect may have completed every subscriber synchronously; the
                        // disposals then ran under the gate and moved the state on. Only hold
                        // the connection if we are still connected with subscribers, or waiting
                        // out the delay.
                        if (state == LazyState.ConnectedWithSubscribers || state == LazyState.ConnectedWithNoSubscribers)
                        {
                            connection = connected;
                        }
                        else
                        {
                            disposeNow = connected;
                        }
                    }

                    if (disposeNow != null)
                    {
                        await disposeNow.DisposeAsync().ConfigureAwait(false);
                    }
                }

                if (terminated)
                {
                    await disposal.DisposeAsync().ConfigureAwait(false);
                }

                return disposal;
            });
        }

        private enum LazyState
        {
            DisconnectedNoSubscribers,
            DisconnectedWithSubscribers,
            ConnectedWithSubscribers,
            ConnectedWithNoSubscribers,
        }

        private sealed class RefConnection
        {
            public int Count;
            public bool Connected;
            public bool Retired;
            public IAsyncDisposable Disposable;
        }
    }
}

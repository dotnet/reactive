// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information. 

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

        private sealed class RefConnection
        {
            public int Count;
            public bool Connected;
            public bool Retired;
            public IAsyncDisposable Disposable;
        }
    }
}

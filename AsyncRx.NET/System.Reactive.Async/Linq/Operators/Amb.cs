// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information. 

using System.Collections.Generic;
using System.Linq;
using System.Reactive.Disposables;
using System.Threading;
using System.Threading.Tasks;

namespace System.Reactive.Linq
{
    public partial class AsyncObservable
    {
        public static IAsyncObservable<TSource> Amb<TSource>(this IAsyncObservable<TSource> first, IAsyncObservable<TSource> second)
        {
            if (first == null)
                throw new ArgumentNullException(nameof(first));
            if (second == null)
                throw new ArgumentNullException(nameof(second));

            return CreateAsyncObservable<TSource>.From(
                first,
                second,
                static async (first, second, observer) =>
                {
                    var firstSubscription = new SingleAssignmentAsyncDisposable();
                    var secondSubscription = new SingleAssignmentAsyncDisposable();

                    var (firstObserver, secondObserver) = AsyncObserver.Amb(observer, firstSubscription, secondSubscription);

                    // Subscribed one after the other, each subscription assigned as it is made,
                    // as Rx.NET's Amb subscribes; a thread-pool continuation per subscription,
                    // as before, ran the assignment off the subscribing thread.
                    await firstSubscription.AssignAsync(await first.SubscribeSafeAsync(firstObserver).ConfigureAwait(false)).ConfigureAwait(false);
                    await secondSubscription.AssignAsync(await second.SubscribeSafeAsync(secondObserver).ConfigureAwait(false)).ConfigureAwait(false);

                    return StableCompositeAsyncDisposable.Create(firstSubscription, secondSubscription);
                });
        }

        public static IAsyncObservable<TSource> Amb<TSource>(this IEnumerable<IAsyncObservable<TSource>> sources)
        {
            if (sources == null)
                throw new ArgumentNullException(nameof(sources));

            return Amb(sources.ToArray());
        }

        public static IAsyncObservable<TSource> Amb<TSource>(params IAsyncObservable<TSource>[] sources)
        {
            if (sources == null)
                throw new ArgumentNullException(nameof(sources));

            // No sources: nothing can win, and the result is empty, as Rx.NET's is.
            if (sources.Length == 0)
            {
                return Empty<TSource>();
            }

            return CreateAsyncObservable<TSource>.From(async observer =>
            {
                var count = sources.Length;

                var subscriptions = new SingleAssignmentAsyncDisposable[count];

                for (var i = 0; i < count; i++)
                {
                    subscriptions[i] = new SingleAssignmentAsyncDisposable();
                }

                var observers = AsyncObserver.Amb(observer, subscriptions);

                for (var i = 0; i < count; i++)
                {
                    await subscriptions[i].AssignAsync(await sources[i].SubscribeSafeAsync(observers[i]).ConfigureAwait(false)).ConfigureAwait(false);
                }

                return StableCompositeAsyncDisposable.Create(subscriptions);
            });
        }
    }

    public partial class AsyncObserver
    {
        public static (IAsyncObserver<TSource>, IAsyncObserver<TSource>) Amb<TSource>(IAsyncObserver<TSource> observer, IAsyncDisposable first, IAsyncDisposable second)
        {
            if (observer == null)
                throw new ArgumentNullException(nameof(observer));
            if (first == null)
                throw new ArgumentNullException(nameof(first));
            if (second == null)
                throw new ArgumentNullException(nameof(second));

            var gate = new AsyncGate();

            var state = AmbState.None;

            return
                (
                    Create<TSource>(
                        async x =>
                        {
                            using (await gate.LockAsync().ConfigureAwait(false))
                            {
                                if (state == AmbState.None)
                                {
                                    state = AmbState.First;
                                    await second.DisposeAsync().ConfigureAwait(false);
                                }

                                if (state == AmbState.First)
                                {
                                    await observer.OnNextAsync(x).ConfigureAwait(false);
                                }
                            }
                        },
                        async ex =>
                        {
                            using (await gate.LockAsync().ConfigureAwait(false))
                            {
                                if (state == AmbState.None)
                                {
                                    state = AmbState.First;
                                    await second.DisposeAsync().ConfigureAwait(false);
                                }

                                if (state == AmbState.First)
                                {
                                    await observer.OnErrorAsync(ex).ConfigureAwait(false);
                                }
                            }
                        },
                        async () =>
                        {
                            using (await gate.LockAsync().ConfigureAwait(false))
                            {
                                if (state == AmbState.None)
                                {
                                    state = AmbState.First;
                                    await second.DisposeAsync().ConfigureAwait(false);
                                }

                                if (state == AmbState.First)
                                {
                                    await observer.OnCompletedAsync().ConfigureAwait(false);
                                }
                            }
                        }
                    ),
                    Create<TSource>(
                        async x =>
                        {
                            using (await gate.LockAsync().ConfigureAwait(false))
                            {
                                if (state == AmbState.None)
                                {
                                    state = AmbState.Second;
                                    await first.DisposeAsync().ConfigureAwait(false);
                                }

                                if (state == AmbState.Second)
                                {
                                    await observer.OnNextAsync(x).ConfigureAwait(false);
                                }
                            }
                        },
                        async ex =>
                        {
                            using (await gate.LockAsync().ConfigureAwait(false))
                            {
                                if (state == AmbState.None)
                                {
                                    state = AmbState.Second;
                                    await first.DisposeAsync().ConfigureAwait(false);
                                }

                                if (state == AmbState.Second)
                                {
                                    await observer.OnErrorAsync(ex).ConfigureAwait(false);
                                }
                            }
                        },
                        async () =>
                        {
                            using (await gate.LockAsync().ConfigureAwait(false))
                            {
                                if (state == AmbState.None)
                                {
                                    state = AmbState.Second;
                                    await first.DisposeAsync().ConfigureAwait(false);
                                }

                                if (state == AmbState.Second)
                                {
                                    await observer.OnCompletedAsync().ConfigureAwait(false);
                                }
                            }
                        }
                    )
                );
        }

        public static IAsyncObserver<TSource>[] Amb<TSource>(IAsyncObserver<TSource> observer, IAsyncDisposable[] subscriptions)
        {
            if (observer == null)
                throw new ArgumentNullException(nameof(observer));
            if (subscriptions == null)
                throw new ArgumentNullException(nameof(subscriptions));

            var gate = new AsyncGate();

            var winner = default(int?);

            var count = subscriptions.Length;

            async Task ElectWinnerAsync(int index)
            {
                winner = index;

                for (var i = 0; i < count; i++)
                {
                    if (i != index)
                    {
                        await subscriptions[i].DisposeAsync().ConfigureAwait(false);
                    }
                }
            }

            IAsyncObserver<TSource> CreateObserver(int index) =>
                Create<TSource>(
                    async x =>
                    {
                        using (await gate.LockAsync().ConfigureAwait(false))
                        {
                            if (winner == null)
                            {
                                await ElectWinnerAsync(index).ConfigureAwait(false);
                            }

                            if (winner == index)
                            {
                                await observer.OnNextAsync(x).ConfigureAwait(false);
                            }
                        }
                    },
                    async ex =>
                    {
                        using (await gate.LockAsync().ConfigureAwait(false))
                        {
                            if (winner == null)
                            {
                                await ElectWinnerAsync(index).ConfigureAwait(false);
                            }

                            if (winner == index)
                            {
                                await observer.OnErrorAsync(ex).ConfigureAwait(false);
                            }
                        }
                    },
                    async () =>
                    {
                        using (await gate.LockAsync().ConfigureAwait(false))
                        {
                            if (winner == null)
                            {
                                await ElectWinnerAsync(index).ConfigureAwait(false);
                            }

                            if (winner == index)
                            {
                                await observer.OnCompletedAsync().ConfigureAwait(false);
                            }
                        }
                    }
                );

            var res = new IAsyncObserver<TSource>[count];

            for (var i = 0; i < count; i++)
            {
                res[i] = CreateObserver(i);
            }

            return res;
        }

        private enum AmbState
        {
            None,
            First,
            Second,
        }
    }
}

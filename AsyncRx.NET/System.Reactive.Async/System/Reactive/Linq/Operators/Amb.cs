// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the Apache 2.0 License.
// See the LICENSE file in the project root for more information. 

using System.Collections.Generic;
using System.Linq;
using System.Reactive.Disposables;
using System.Threading;
using System.Threading.Tasks;

namespace System.Reactive.Linq
{
    partial class AsyncObservable
    {
        public static IAsyncObservable<TSource> Amb<TSource>(this IAsyncObservable<TSource> first, IAsyncObservable<TSource> second)
        {
            if (first == null)
                throw new ArgumentNullException(nameof(first));
            if (second == null)
                throw new ArgumentNullException(nameof(second));

            return Create<TSource>(async observer =>
            {
                var firstSubscription = new SingleAssignmentAsyncDisposable();
                var secondSubscription = new SingleAssignmentAsyncDisposable();

                var (firstObserver, secondObserver) = AsyncObserver.Amb(observer, firstSubscription, secondSubscription);

                var firstTask = first.SubscribeSafeAsync(firstObserver).ContinueWith(d => firstSubscription.AssignAsync(d.Result)).Unwrap();
                var secondTask = second.SubscribeSafeAsync(secondObserver).ContinueWith(d => secondSubscription.AssignAsync(d.Result)).Unwrap();

                await Task.WhenAll(firstTask, secondTask).ConfigureAwait(false);

                return StableCompositeAsyncDisposable.Create(firstSubscription, secondSubscription);
            });
        }

        public static IAsyncObservable<TSource> Amb<TSource>(this IEnumerable<IAsyncObservable<TSource>> sources) => Amb(sources.ToArray());

        public static IAsyncObservable<TSource> Amb<TSource>(params IAsyncObservable<TSource>[] sources)
        {
            if (sources == null)
                throw new ArgumentNullException(nameof(sources));

            return Create<TSource>(async observer =>
            {
                var count = sources.Length;

                var subscriptions = new SingleAssignmentAsyncDisposable[count];

                for (var i = 0; i < count; i++)
                {
                    subscriptions[i] = new SingleAssignmentAsyncDisposable();
                }

                var observers = AsyncObserver.Amb(observer, subscriptions);

                var tasks = new Task[count];

                for (var i = 0; i < count; i++)
                {
                    tasks[i] = sources[i].SubscribeSafeAsync(observers[i]).ContinueWith(d => subscriptions[i].AssignAsync(d.Result)).Unwrap();
                }

                await Task.WhenAll(tasks).ConfigureAwait(false);

                return StableCompositeAsyncDisposable.Create(subscriptions);
            });
        }
    }

    partial class AsyncObserver
    {
        public static (IAsyncObserver<TSource>, IAsyncObserver<TSource>) Amb<TSource>(IAsyncObserver<TSource> observer, IAsyncDisposable first, IAsyncDisposable second)
        {
            if (observer == null)
                throw new ArgumentNullException(nameof(observer));
            if (first == null)
                throw new ArgumentNullException(nameof(first));
            if (second == null)
                throw new ArgumentNullException(nameof(second));

            var gate = new AsyncLock();

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

            var gate = new AsyncLock();

            var winner = default(int?);

            var count = subscriptions.Length;

            async Task ElectWinnerAsync(int index)
            {
                winner = index;

                var dispose = new List<Task>(count - 1);

                for (var i = 0; i < count; i++)
                {
                    if (i != index)
                    {
                        dispose.Add(subscriptions[i].DisposeAsync());
                    }
                }

                await Task.WhenAll(dispose).ConfigureAwait(false);
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

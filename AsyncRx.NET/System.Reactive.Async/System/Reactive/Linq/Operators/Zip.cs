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
        // TODO: Add Zip<T>(IAsyncObservable<T>, IAsyncEnumerable<T>) overload when we have reference to IAsyncEnumerable<T>.

        public static IAsyncObservable<IList<TSource>> Zip<TSource>(IEnumerable<IAsyncObservable<TSource>> sources) => Zip(sources.ToArray());

        public static IAsyncObservable<IList<TSource>> Zip<TSource>(params IAsyncObservable<TSource>[] sources)
        {
            if (sources == null)
                throw new ArgumentNullException(nameof(sources));

            return Create<IList<TSource>>(async observer =>
            {
                var count = sources.Length;

                var observers = AsyncObserver.Zip(observer, count);

                var tasks = new Task<IAsyncDisposable>[count];

                for (var i = 0; i < count; i++)
                {
                    tasks[i] = sources[i].SubscribeSafeAsync(observers[i]);
                }

                await Task.WhenAll(tasks).ConfigureAwait(false);

                return StableCompositeAsyncDisposable.Create(tasks.Select(t => t.Result));
            });
        }
    }

    partial class AsyncObserver
    {
        public static IAsyncObserver<TSource>[] Zip<TSource>(IAsyncObserver<IList<TSource>> observer, int count)
        {
            if (observer == null)
                throw new ArgumentNullException(nameof(observer));
            if (count < 0)
                throw new ArgumentOutOfRangeException(nameof(count));

            var gate = new AsyncLock();

            var queues = new Queue<TSource>[count];
            var isDone = new bool[count];
            var res = new IAsyncObserver<TSource>[count];

            IAsyncObserver<TSource> CreateObserver(int index) =>
                Create<TSource>(
                    async x =>
                    {
                        using (await gate.LockAsync().ConfigureAwait(false))
                        {
                            queues[index].Enqueue(x);

                            if (queues.All(queue => queue.Count > 0))
                            {
                                var list = new TSource[count];

                                for (var i = 0; i < count; i++)
                                {
                                    list[i] = queues[i].Dequeue();
                                }

                                await observer.OnNextAsync(list).ConfigureAwait(false);
                            }
                            else
                            {
                                var allDone = true;

                                for (var i = 0; i < count; i++)
                                {
                                    if (i != index && !isDone[i])
                                    {
                                        allDone = false;
                                        break;
                                    }
                                }

                                if (allDone)
                                {
                                    await observer.OnCompletedAsync().ConfigureAwait(false);
                                }
                            }
                        }
                    },
                    async ex =>
                    {
                        using (await gate.LockAsync().ConfigureAwait(false))
                        {
                            await observer.OnErrorAsync(ex).ConfigureAwait(false);
                        }
                    },
                    async () =>
                    {
                        using (await gate.LockAsync().ConfigureAwait(false))
                        {
                            isDone[index] = true;

                            var allDone = true;

                            for (var i = 0; i < count; i++)
                            {
                                if (!isDone[i])
                                {
                                    allDone = false;
                                    break;
                                }
                            }

                            if (allDone)
                            {
                                await observer.OnCompletedAsync().ConfigureAwait(false);
                            }
                        }
                    }
                );

            for (var i = 0; i < count; i++)
            {
                queues[i] = new Queue<TSource>();
                res[i] = CreateObserver(i);
            }

            return res;
        }
    }
}

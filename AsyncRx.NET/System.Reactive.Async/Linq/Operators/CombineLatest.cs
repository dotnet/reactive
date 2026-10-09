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
        public static IAsyncObservable<IList<TSource>> CombineLatest<TSource>(IEnumerable<IAsyncObservable<TSource>> sources)
        {
            if (sources == null)
                throw new ArgumentNullException(nameof(sources));

            return CombineLatest<TSource, IList<TSource>>(sources, static xs => new ValueTask<IList<TSource>>(xs));
        }

        public static IAsyncObservable<IList<TSource>> CombineLatest<TSource>(params IAsyncObservable<TSource>[] sources)
        {
            if (sources == null)
                throw new ArgumentNullException(nameof(sources));

            return CombineLatest<TSource, IList<TSource>>(sources, static xs => new ValueTask<IList<TSource>>(xs));
        }

        public static IAsyncObservable<TResult> CombineLatest<TSource, TResult>(IEnumerable<IAsyncObservable<TSource>> sources, Func<IList<TSource>, TResult> resultSelector)
        {
            if (sources == null)
                throw new ArgumentNullException(nameof(sources));
            if (resultSelector == null)
                throw new ArgumentNullException(nameof(resultSelector));

            return CombineLatest<TSource, TResult>(sources, xs => new ValueTask<TResult>(resultSelector(xs)));
        }

        public static IAsyncObservable<TResult> CombineLatest<TSource, TResult>(IEnumerable<IAsyncObservable<TSource>> sources, Func<IList<TSource>, ValueTask<TResult>> resultSelector)
        {
            if (sources == null)
                throw new ArgumentNullException(nameof(sources));
            if (resultSelector == null)
                throw new ArgumentNullException(nameof(resultSelector));

            return Create<TResult>(async observer =>
            {
                var array = sources.ToArray();
                var count = array.Length;

                var d = new CompositeAsyncDisposable();

                var observers = AsyncObserver.CombineLatest(observer, count, resultSelector);

                var subscriptions = new ValueTask<IAsyncDisposable>[count];

                for (var i = 0; i < count; i++)
                {
                    subscriptions[i] = SubscribeReleasingOnCompletedAsync(array[i], observers[i]);
                }

                await CollectAsync(d, subscriptions).ConfigureAwait(false);

                return d;
            });
        }
    }

    public partial class AsyncObserver
    {
        public static IAsyncObserver<TSource>[] CombineLatest<TSource>(IAsyncObserver<IList<TSource>> observer, int count)
        {
            if (observer == null)
                throw new ArgumentNullException(nameof(observer));

            return CombineLatest<TSource, IList<TSource>>(observer, count, static xs => new ValueTask<IList<TSource>>(xs));
        }

        public static IAsyncObserver<TSource>[] CombineLatest<TSource, TResult>(IAsyncObserver<TResult> observer, int count, Func<IList<TSource>, TResult> resultSelector)
        {
            if (observer == null)
                throw new ArgumentNullException(nameof(observer));
            if (resultSelector == null)
                throw new ArgumentNullException(nameof(resultSelector));

            return CombineLatest<TSource, TResult>(observer, count, xs => new ValueTask<TResult>(resultSelector(xs)));
        }

        // The N-ary form of the generated sinks: the latest value of every source, combined
        // whenever one of them produces an element once all have; completed when every source
        // has completed, or when one produces an element while all the others are done. The
        // selector receives a fresh list each time, so that a result holding on to it (the list
        // form hands it straight to the observer) sees the values it was given.
        public static IAsyncObserver<TSource>[] CombineLatest<TSource, TResult>(IAsyncObserver<TResult> observer, int count, Func<IList<TSource>, ValueTask<TResult>> resultSelector)
        {
            if (observer == null)
                throw new ArgumentNullException(nameof(observer));
            if (count < 0)
                throw new ArgumentOutOfRangeException(nameof(count));
            if (resultSelector == null)
                throw new ArgumentNullException(nameof(resultSelector));

            var gate = new AsyncGate();

            var hasValue = new bool[count];
            var isDone = new bool[count];
            var latest = new TSource[count];
            var allHasValue = false;

            var res = new IAsyncObserver<TSource>[count];

            IAsyncObserver<TSource> CreateObserver(int index) =>
                Create<TSource>(
                    async x =>
                    {
                        using (await gate.LockAsync().ConfigureAwait(false))
                        {
                            if (!hasValue[index])
                            {
                                hasValue[index] = true;
                                allHasValue = Array.TrueForAll(hasValue, static v => v);
                            }

                            latest[index] = x;

                            if (allHasValue)
                            {
                                TResult result;

                                try
                                {
                                    result = await resultSelector(new List<TSource>(latest)).ConfigureAwait(false);
                                }
                                catch (Exception ex)
                                {
                                    await observer.OnErrorAsync(ex).ConfigureAwait(false);
                                    return;
                                }

                                await observer.OnNextAsync(result).ConfigureAwait(false);
                            }
                            else
                            {
                                var othersDone = true;

                                for (var i = 0; i < count; i++)
                                {
                                    if (i != index && !isDone[i])
                                    {
                                        othersDone = false;
                                        break;
                                    }
                                }

                                if (othersDone)
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

                            if (Array.TrueForAll(isDone, static d => d))
                            {
                                await observer.OnCompletedAsync().ConfigureAwait(false);
                            }
                        }
                    }
                );

            for (var i = 0; i < count; i++)
            {
                res[i] = CreateObserver(i);
            }

            return res;
        }
    }
}

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
        // TODO: Add Zip<T>(IAsyncObservable<T>, IAsyncEnumerable<T>) overload when we have reference to IAsyncEnumerable<T>.

        public static IAsyncObservable<IList<TSource>> Zip<TSource>(IEnumerable<IAsyncObservable<TSource>> sources)
        {
            if (sources == null)
                throw new ArgumentNullException(nameof(sources));

            return Zip<TSource, IList<TSource>>(sources, static xs => new ValueTask<IList<TSource>>(xs));
        }

        public static IAsyncObservable<IList<TSource>> Zip<TSource>(params IAsyncObservable<TSource>[] sources)
        {
            if (sources == null)
                throw new ArgumentNullException(nameof(sources));

            return Zip<TSource, IList<TSource>>(sources, static xs => new ValueTask<IList<TSource>>(xs));
        }

        public static IAsyncObservable<TResult> Zip<TSource, TResult>(IEnumerable<IAsyncObservable<TSource>> sources, Func<IList<TSource>, TResult> resultSelector)
        {
            if (sources == null)
                throw new ArgumentNullException(nameof(sources));
            if (resultSelector == null)
                throw new ArgumentNullException(nameof(resultSelector));

            return Zip<TSource, TResult>(sources, xs => new ValueTask<TResult>(resultSelector(xs)));
        }

        public static IAsyncObservable<TResult> Zip<TSource, TResult>(IEnumerable<IAsyncObservable<TSource>> sources, Func<IList<TSource>, ValueTask<TResult>> resultSelector)
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

                var observers = AsyncObserver.Zip(observer, count, resultSelector);

                var subscriptions = new ValueTask<IAsyncDisposable>[count];

                for (var i = 0; i < count; i++)
                {
                    subscriptions[i] = SubscribeReleasingOnCompletedAsync(array[i], observers[i]);
                }

                await CollectAsync(d, subscriptions).ConfigureAwait(false);

                return d;
            });
        }

        public static IAsyncObservable<(T1, T2)> Zip<T1, T2>(this IAsyncObservable<T1> first, IEnumerable<T2> second)
        {
            if (first == null)
                throw new ArgumentNullException(nameof(first));
            if (second == null)
                throw new ArgumentNullException(nameof(second));

            return Zip<T1, T2, (T1, T2)>(first, second, static (x, y) => new ValueTask<(T1, T2)>((x, y)));
        }

        public static IAsyncObservable<TResult> Zip<T1, T2, TResult>(this IAsyncObservable<T1> first, IEnumerable<T2> second, Func<T1, T2, TResult> selector)
        {
            if (first == null)
                throw new ArgumentNullException(nameof(first));
            if (second == null)
                throw new ArgumentNullException(nameof(second));
            if (selector == null)
                throw new ArgumentNullException(nameof(selector));

            return Zip<T1, T2, TResult>(first, second, (x, y) => new ValueTask<TResult>(selector(x, y)));
        }

        // The enumerator is obtained before the observable is subscribed, as Rx.NET does, so that
        // it is in place by the time the first element can arrive; an enumerable that throws on
        // enumeration ends the result with that exception without subscribing at all.
        public static IAsyncObservable<TResult> Zip<T1, T2, TResult>(this IAsyncObservable<T1> first, IEnumerable<T2> second, Func<T1, T2, ValueTask<TResult>> selector)
        {
            if (first == null)
                throw new ArgumentNullException(nameof(first));
            if (second == null)
                throw new ArgumentNullException(nameof(second));
            if (selector == null)
                throw new ArgumentNullException(nameof(selector));

            return Create<TResult>(async observer =>
            {
                IEnumerator<T2> enumerator;

                try
                {
                    enumerator = second.GetEnumerator();
                }
                catch (Exception ex)
                {
                    await observer.OnErrorAsync(ex).ConfigureAwait(false);
                    return AsyncDisposable.Nop;
                }

                var (sink, disposeEnumerator) = AsyncObserver.Zip(observer, enumerator, selector);

                var subscription = await first.SubscribeSafeAsync(sink).ConfigureAwait(false);

                return StableCompositeAsyncDisposable.Create(subscription, disposeEnumerator);
            });
        }
    }

    public partial class AsyncObserver
    {
        public static IAsyncObserver<TSource>[] Zip<TSource>(IAsyncObserver<IList<TSource>> observer, int count)
        {
            if (observer == null)
                throw new ArgumentNullException(nameof(observer));

            return Zip<TSource, IList<TSource>>(observer, count, static xs => new ValueTask<IList<TSource>>(xs));
        }

        public static IAsyncObserver<TSource>[] Zip<TSource, TResult>(IAsyncObserver<TResult> observer, int count, Func<IList<TSource>, TResult> resultSelector)
        {
            if (observer == null)
                throw new ArgumentNullException(nameof(observer));
            if (resultSelector == null)
                throw new ArgumentNullException(nameof(resultSelector));

            return Zip<TSource, TResult>(observer, count, xs => new ValueTask<TResult>(resultSelector(xs)));
        }

        public static IAsyncObserver<TSource>[] Zip<TSource, TResult>(IAsyncObserver<TResult> observer, int count, Func<IList<TSource>, ValueTask<TResult>> resultSelector)
        {
            if (observer == null)
                throw new ArgumentNullException(nameof(observer));
            if (count < 0)
                throw new ArgumentOutOfRangeException(nameof(count));
            if (resultSelector == null)
                throw new ArgumentNullException(nameof(resultSelector));

            var gate = new AsyncGate();

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

                                TResult result;

                                try
                                {
                                    result = await resultSelector(list).ConfigureAwait(false);
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

        // The observable-with-enumerable sink, a port of Rx.NET's: each element from the observable
        // takes the enumerator one step, and the pair goes through the selector; the enumerator
        // running dry completes the result. Disposal from inside MoveNext or Current (an observer
        // that unsubscribes on the first element) must not dispose the enumerator while it is in
        // use, so disposal is counted against enumeration in progress and deferred to the end of
        // that step, as Rx.NET's sink does.
        public static (IAsyncObserver<T1>, IAsyncDisposable) Zip<T1, T2, TResult>(IAsyncObserver<TResult> observer, IEnumerator<T2> enumerator, Func<T1, T2, ValueTask<TResult>> selector)
        {
            if (observer == null)
                throw new ArgumentNullException(nameof(observer));
            if (enumerator == null)
                throw new ArgumentNullException(nameof(enumerator));
            if (selector == null)
                throw new ArgumentNullException(nameof(selector));

            var current = enumerator;
            var enumerationInProgress = 0;

            void DisposeEnumerator()
            {
                Interlocked.Exchange(ref current, null)?.Dispose();
            }

            var sink = Create<T1>(
                async x =>
                {
                    var e = Volatile.Read(ref current);

                    if (e == null)
                    {
                        return;
                    }

                    if (Interlocked.Increment(ref enumerationInProgress) != 1)
                    {
                        return;
                    }

                    bool hasNext;
                    var right = default(T2);
                    var wasDisposed = false;

                    try
                    {
                        try
                        {
                            hasNext = e.MoveNext();

                            if (hasNext)
                            {
                                right = e.Current;
                            }
                        }
                        finally
                        {
                            if (Interlocked.Decrement(ref enumerationInProgress) != 0)
                            {
                                DisposeEnumerator();
                                wasDisposed = true;
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        await observer.OnErrorAsync(ex).ConfigureAwait(false);
                        return;
                    }

                    if (wasDisposed)
                    {
                        return;
                    }

                    if (hasNext)
                    {
                        TResult result;

                        try
                        {
                            result = await selector(x, right).ConfigureAwait(false);
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
                        await observer.OnCompletedAsync().ConfigureAwait(false);
                    }
                },
                observer.OnErrorAsync,
                observer.OnCompletedAsync);

            var dispose = AsyncDisposable.Create(() =>
            {
                if (Interlocked.Increment(ref enumerationInProgress) == 1)
                {
                    DisposeEnumerator();
                }

                return default;
            });

            return (sink, dispose);
        }
    }
}

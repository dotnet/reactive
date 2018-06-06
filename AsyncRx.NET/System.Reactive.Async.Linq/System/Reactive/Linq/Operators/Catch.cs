// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the Apache 2.0 License.
// See the LICENSE file in the project root for more information. 

using System.Collections.Generic;
using System.Reactive.Disposables;
using System.Threading.Tasks;

namespace System.Reactive.Linq
{
    // TODO: Implement tail call behavior to flatten Catch chains.

    partial class AsyncObservable
    {
        public static IAsyncObservable<TSource> Catch<TSource, TException>(this IAsyncObservable<TSource> source, Func<TException, IAsyncObservable<TSource>> handler)
            where TException : Exception
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));
            if (handler == null)
                throw new ArgumentNullException(nameof(handler));

            return Create<TSource>(async observer =>
            {
                var (sink, inner) = AsyncObserver.Catch(observer, handler);

                var subscription = await source.SubscribeSafeAsync(sink).ConfigureAwait(false);

                return StableCompositeAsyncDisposable.Create(subscription, inner);
            });
        }

        public static IAsyncObservable<TSource> Catch<TSource, TException>(this IAsyncObservable<TSource> source, Func<TException, Task<IAsyncObservable<TSource>>> handler)
            where TException : Exception
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));
            if (handler == null)
                throw new ArgumentNullException(nameof(handler));

            return Create<TSource>(async observer =>
            {
                var (sink, inner) = AsyncObserver.Catch(observer, handler);

                var subscription = await source.SubscribeSafeAsync(sink).ConfigureAwait(false);

                return StableCompositeAsyncDisposable.Create(subscription, inner);
            });
        }

        public static IAsyncObservable<TSource> Catch<TSource>(this IAsyncObservable<TSource> first, IAsyncObservable<TSource> second)
        {
            if (first == null)
                throw new ArgumentNullException(nameof(first));
            if (second == null)
                throw new ArgumentNullException(nameof(second));

            return Create<TSource>(async observer =>
            {
                var (sink, inner) = AsyncObserver.Catch(observer, second);

                var subscription = await first.SubscribeSafeAsync(sink).ConfigureAwait(false);

                return StableCompositeAsyncDisposable.Create(subscription, inner);
            });
        }

        public static IAsyncObservable<TSource> Catch<TSource>(params IAsyncObservable<TSource>[] sources) => Catch((IEnumerable<IAsyncObservable<TSource>>)sources);

        public static IAsyncObservable<TSource> Catch<TSource>(this IEnumerable<IAsyncObservable<TSource>> sources)
        {
            if (sources == null)
                throw new ArgumentNullException(nameof(sources));

            return Create<TSource>(async observer =>
            {
                var enumerator = sources.GetEnumerator();

                if (!enumerator.MoveNext())
                {
                    return AsyncDisposable.Nop; // REVIEW: Is Never behavior right here?
                }

                var source = enumerator.Current;

                var (sink, inner) = AsyncObserver.Catch(observer, enumerator);

                var subscription = await source.SubscribeSafeAsync(sink).ConfigureAwait(false);

                return StableCompositeAsyncDisposable.Create(subscription, inner);
            });
        }
    }

    partial class AsyncObserver
    {
        public static (IAsyncObserver<TSource>, IAsyncDisposable) Catch<TSource, TException>(IAsyncObserver<TSource> observer, Func<TException, IAsyncObservable<TSource>> handler)
            where TException : Exception
        {
            if (observer == null)
                throw new ArgumentNullException(nameof(observer));
            if (handler == null)
                throw new ArgumentNullException(nameof(handler));

            return Catch<TSource, TException>(observer, ex => Task.FromResult(handler(ex)));
        }

        public static (IAsyncObserver<TSource>, IAsyncDisposable) Catch<TSource, TException>(IAsyncObserver<TSource> observer, Func<TException, Task<IAsyncObservable<TSource>>> handler)
            where TException : Exception
        {
            if (observer == null)
                throw new ArgumentNullException(nameof(observer));
            if (handler == null)
                throw new ArgumentNullException(nameof(handler));

            var subscription = new SingleAssignmentAsyncDisposable();

            var sink = Create<TSource>(
                observer.OnNextAsync,
                async ex =>
                {
                    if (ex is TException error)
                    {
                        IAsyncObservable<TSource> handlerObservable;

                        try
                        {
                            handlerObservable = await handler(error).ConfigureAwait(false);
                        }
                        catch (Exception err)
                        {
                            await observer.OnErrorAsync(err).ConfigureAwait(false);
                            return;
                        }

                        var handlerSubscription = await handlerObservable.SubscribeSafeAsync(observer).ConfigureAwait(false);

                        await subscription.AssignAsync(handlerSubscription).ConfigureAwait(false);
                    }
                    else
                    {
                        await observer.OnErrorAsync(ex).ConfigureAwait(false);
                    }
                },
                observer.OnCompletedAsync
            );

            return (sink, subscription);
        }

        public static (IAsyncObserver<TSource>, IAsyncDisposable) Catch<TSource>(IAsyncObserver<TSource> observer, IAsyncObservable<TSource> second)
        {
            if (observer == null)
                throw new ArgumentNullException(nameof(observer));
            if (second == null)
                throw new ArgumentNullException(nameof(second));

            var subscription = new SingleAssignmentAsyncDisposable();

            var sink = Create<TSource>(
                observer.OnNextAsync,
                async ex =>
                {
                    var secondSubscription = await second.SubscribeSafeAsync(observer).ConfigureAwait(false);

                    await subscription.AssignAsync(secondSubscription).ConfigureAwait(false);
                },
                observer.OnCompletedAsync
            );

            return (sink, subscription);
        }

        public static (IAsyncObserver<TSource>, IAsyncDisposable) Catch<TSource>(IAsyncObserver<TSource> observer, IEnumerator<IAsyncObservable<TSource>> handlers)
        {
            if (observer == null)
                throw new ArgumentNullException(nameof(observer));
            if (handlers == null)
                throw new ArgumentNullException(nameof(handlers));

            var innerSubscription = new SerialAsyncDisposable();

            IAsyncObserver<TSource> GetSink() =>
                Create<TSource>(
                    observer.OnNextAsync,
                    async ex =>
                    {
                        var handler = default(IAsyncObservable<TSource>);

                        try
                        {
                            if (handlers.MoveNext())
                            {
                                handler = handlers.Current;
                            }
                        }
                        catch (Exception err)
                        {
                            await observer.OnErrorAsync(err).ConfigureAwait(false);
                            return;
                        }

                        if (handler == null)
                        {
                            await observer.OnErrorAsync(ex).ConfigureAwait(false); // REVIEW: Is Throw behavior right here?
                            return;
                        }

                        var handlerSubscription = await handler.SubscribeSafeAsync(GetSink()).ConfigureAwait(false);

                        await innerSubscription.AssignAsync(handlerSubscription).ConfigureAwait(false);
                    },
                    observer.OnCompletedAsync
                );

            var disposeEnumerator = AsyncDisposable.Create(() =>
            {
                handlers.Dispose();
                return Task.CompletedTask;
            });

            var subscription = StableCompositeAsyncDisposable.Create(innerSubscription, disposeEnumerator);

            return (GetSink(), subscription);
        }
    }
}

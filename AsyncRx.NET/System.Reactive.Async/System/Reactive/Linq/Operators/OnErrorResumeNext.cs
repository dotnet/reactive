// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the Apache 2.0 License.
// See the LICENSE file in the project root for more information. 

using System.Collections.Generic;
using System.Reactive.Disposables;
using System.Threading.Tasks;

namespace System.Reactive.Linq
{
    // TODO: Implement tail call behavior to flatten OnErrorResumeNext chains.

    partial class AsyncObservable
    {
        public static IAsyncObservable<TSource> OnErrorResumeNext<TSource>(this IAsyncObservable<TSource> first, IAsyncObservable<TSource> second)
        {
            if (first == null)
                throw new ArgumentNullException(nameof(first));
            if (second == null)
                throw new ArgumentNullException(nameof(second));

            return Create<TSource>(async observer =>
            {
                var (sink, inner) = AsyncObserver.OnErrorResumeNext(observer, second);

                var subscription = await first.SubscribeSafeAsync(sink).ConfigureAwait(false);

                return StableCompositeAsyncDisposable.Create(subscription, inner);
            });
        }

        public static IAsyncObservable<TSource> OnErrorResumeNext<TSource>(params IAsyncObservable<TSource>[] sources) => OnErrorResumeNext((IEnumerable<IAsyncObservable<TSource>>)sources);

        public static IAsyncObservable<TSource> OnErrorResumeNext<TSource>(this IEnumerable<IAsyncObservable<TSource>> sources)
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

                var (sink, inner) = AsyncObserver.OnErrorResumeNext(observer, enumerator);

                var subscription = await source.SubscribeSafeAsync(sink).ConfigureAwait(false);

                return StableCompositeAsyncDisposable.Create(subscription, inner);
            });
        }
    }

    partial class AsyncObserver
    {
        public static (IAsyncObserver<TSource>, IAsyncDisposable) OnErrorResumeNext<TSource>(IAsyncObserver<TSource> observer, IAsyncObservable<TSource> second)
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
                async () =>
                {
                    var secondSubscription = await second.SubscribeSafeAsync(observer).ConfigureAwait(false);

                    await subscription.AssignAsync(secondSubscription).ConfigureAwait(false);
                }
            );

            return (sink, subscription);
        }

        public static (IAsyncObserver<TSource>, IAsyncDisposable) OnErrorResumeNext<TSource>(IAsyncObserver<TSource> observer, IEnumerator<IAsyncObservable<TSource>> handlers)
        {
            if (observer == null)
                throw new ArgumentNullException(nameof(observer));
            if (handlers == null)
                throw new ArgumentNullException(nameof(handlers));

            var innerSubscription = new SerialAsyncDisposable();

            async Task NextAsync()
            {
                var next = default(IAsyncObservable<TSource>);

                try
                {
                    if (handlers.MoveNext())
                    {
                        next = handlers.Current;
                    }
                }
                catch (Exception err)
                {
                    await observer.OnErrorAsync(err).ConfigureAwait(false);
                    return;
                }

                if (next == null)
                {
                    await observer.OnCompletedAsync().ConfigureAwait(false); // REVIEW: Is Empty behavior right here?
                    return;
                }

                var nextSubscription = await next.SubscribeSafeAsync(GetSink()).ConfigureAwait(false);

                await innerSubscription.AssignAsync(nextSubscription).ConfigureAwait(false);
            }

            IAsyncObserver<TSource> GetSink() =>
                Create<TSource>(
                    observer.OnNextAsync,
                    async ex => await NextAsync().ConfigureAwait(false),
                    NextAsync
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

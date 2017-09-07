// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the Apache 2.0 License.
// See the LICENSE file in the project root for more information. 

using System.Reactive.Concurrency;
using System.Threading.Tasks;

namespace System.Reactive.Linq
{
    partial class AsyncObservable
    {
        public static IAsyncObservable<TSource> Prepend<TSource>(this IAsyncObservable<TSource> source, TSource element)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));

            return Create<TSource>(async observer => await source.SubscribeAsync(await AsyncObserver.Prepend(observer, element).ConfigureAwait(false)).ConfigureAwait(false));
        }

        public static IAsyncObservable<TSource> Prepend<TSource>(this IAsyncObservable<TSource> source, TSource element, IAsyncScheduler scheduler)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));
            if (scheduler == null)
                throw new ArgumentNullException(nameof(scheduler));

            return Create<TSource>(async observer => await source.SubscribeAsync(await AsyncObserver.Prepend(observer, element, scheduler).ConfigureAwait(false)).ConfigureAwait(false));
        }
    }

    partial class AsyncObserver
    {
        public static Task<IAsyncObserver<TSource>> Prepend<TSource>(IAsyncObserver<TSource> observer, TSource element)
        {
            if (observer == null)
                throw new ArgumentNullException(nameof(observer));

            return Core();

            async Task<IAsyncObserver<TSource>> Core()
            {
                await observer.OnNextAsync(element);

                return Create<TSource>(
                    observer.OnNextAsync,
                    observer.OnErrorAsync,
                    async () =>
                    {
                        await observer.OnNextAsync(element).ConfigureAwait(false);
                        await observer.OnCompletedAsync().ConfigureAwait(false);
                    }
                );
            }
        }

        public static Task<IAsyncObserver<TSource>> Prepend<TSource>(IAsyncObserver<TSource> observer, TSource element, IAsyncScheduler scheduler)
        {
            if (observer == null)
                throw new ArgumentNullException(nameof(observer));
            if (scheduler == null)
                throw new ArgumentNullException(nameof(scheduler));

            throw new NotImplementedException();
        }
    }
}

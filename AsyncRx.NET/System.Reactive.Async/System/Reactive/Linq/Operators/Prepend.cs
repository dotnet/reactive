// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the Apache 2.0 License.
// See the LICENSE file in the project root for more information. 

using System.Collections.Generic;
using System.Reactive.Concurrency;
using System.Threading.Tasks;

namespace System.Reactive.Linq
{
    partial class AsyncObservable
    {
        public static IAsyncObservable<TSource> StartWith<TSource>(this IAsyncObservable<TSource> source, params TSource[] values) => Prepend(source, values);
        public static IAsyncObservable<TSource> StartWith<TSource>(this IAsyncObservable<TSource> source, IEnumerable<TSource> values) => Prepend(source, values);
        public static IAsyncObservable<TSource> StartWith<TSource>(this IAsyncObservable<TSource> source, IAsyncScheduler scheduler, params TSource[] values) => Prepend(source, scheduler, values);
        public static IAsyncObservable<TSource> StartWith<TSource>(this IAsyncObservable<TSource> source, IAsyncScheduler scheduler, IEnumerable<TSource> values) => Prepend(source, scheduler, values);

        public static IAsyncObservable<TSource> Prepend<TSource>(this IAsyncObservable<TSource> source, TSource value)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));

            return Create<TSource>(async observer => await source.SubscribeAsync(await AsyncObserver.Prepend(observer, value).ConfigureAwait(false)).ConfigureAwait(false));
        }

        public static IAsyncObservable<TSource> Prepend<TSource>(this IAsyncObservable<TSource> source, TSource value, IAsyncScheduler scheduler)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));
            if (scheduler == null)
                throw new ArgumentNullException(nameof(scheduler));

            return Create<TSource>(async observer => await source.SubscribeAsync(await AsyncObserver.Prepend(observer, value, scheduler).ConfigureAwait(false)).ConfigureAwait(false));
        }

        public static IAsyncObservable<TSource> Prepend<TSource>(this IAsyncObservable<TSource> source, params TSource[] values)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));
            if (values == null)
                throw new ArgumentNullException(nameof(values));

            return Create<TSource>(async observer => await source.SubscribeAsync(await AsyncObserver.Prepend(observer, values).ConfigureAwait(false)).ConfigureAwait(false));
        }

        public static IAsyncObservable<TSource> Prepend<TSource>(this IAsyncObservable<TSource> source, IAsyncScheduler scheduler, params TSource[] values)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));
            if (scheduler == null)
                throw new ArgumentNullException(nameof(scheduler));
            if (values == null)
                throw new ArgumentNullException(nameof(values));

            return Create<TSource>(async observer => await source.SubscribeAsync(await AsyncObserver.Prepend(observer, scheduler, values).ConfigureAwait(false)).ConfigureAwait(false));
        }

        public static IAsyncObservable<TSource> Prepend<TSource>(this IAsyncObservable<TSource> source, IEnumerable<TSource> values)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));
            if (values == null)
                throw new ArgumentNullException(nameof(values));

            return Create<TSource>(async observer => await source.SubscribeAsync(await AsyncObserver.Prepend(observer, values).ConfigureAwait(false)).ConfigureAwait(false));
        }

        public static IAsyncObservable<TSource> Prepend<TSource>(this IAsyncObservable<TSource> source, IAsyncScheduler scheduler, IEnumerable<TSource> values)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));
            if (scheduler == null)
                throw new ArgumentNullException(nameof(scheduler));
            if (values == null)
                throw new ArgumentNullException(nameof(values));

            return Create<TSource>(async observer => await source.SubscribeAsync(await AsyncObserver.Prepend(observer, scheduler, values).ConfigureAwait(false)).ConfigureAwait(false));
        }
    }

    partial class AsyncObserver
    {
        public static Task<IAsyncObserver<TSource>> Prepend<TSource>(IAsyncObserver<TSource> observer, TSource value)
        {
            if (observer == null)
                throw new ArgumentNullException(nameof(observer));

            return Core();

            async Task<IAsyncObserver<TSource>> Core()
            {
                await observer.OnNextAsync(value);

                return observer;
            }
        }

        public static Task<IAsyncObserver<TSource>> Prepend<TSource>(IAsyncObserver<TSource> observer, TSource value, IAsyncScheduler scheduler)
        {
            if (observer == null)
                throw new ArgumentNullException(nameof(observer));
            if (scheduler == null)
                throw new ArgumentNullException(nameof(scheduler));

            throw new NotImplementedException();
        }

        public static Task<IAsyncObserver<TSource>> Prepend<TSource>(IAsyncObserver<TSource> observer, params TSource[] values)
        {
            if (observer == null)
                throw new ArgumentNullException(nameof(observer));
            if (values == null)
                throw new ArgumentNullException(nameof(values));

            return Core();

            async Task<IAsyncObserver<TSource>> Core()
            {
                foreach (var value in values)
                {
                    await observer.OnNextAsync(value);
                }

                return observer;
            }
        }

        public static Task<IAsyncObserver<TSource>> Prepend<TSource>(IAsyncObserver<TSource> observer, IAsyncScheduler scheduler, params TSource[] values)
        {
            if (observer == null)
                throw new ArgumentNullException(nameof(observer));
            if (scheduler == null)
                throw new ArgumentNullException(nameof(scheduler));
            if (values == null)
                throw new ArgumentNullException(nameof(values));

            throw new NotImplementedException();
        }

        public static Task<IAsyncObserver<TSource>> Prepend<TSource>(IAsyncObserver<TSource> observer, IEnumerable<TSource> values)
        {
            if (observer == null)
                throw new ArgumentNullException(nameof(observer));
            if (values == null)
                throw new ArgumentNullException(nameof(values));

            return Core();

            async Task<IAsyncObserver<TSource>> Core()
            {
                foreach (var value in values)
                {
                    await observer.OnNextAsync(value);
                }

                return observer;
            }
        }

        public static Task<IAsyncObserver<TSource>> Prepend<TSource>(IAsyncObserver<TSource> observer, IAsyncScheduler scheduler, IEnumerable<TSource> values)
        {
            if (observer == null)
                throw new ArgumentNullException(nameof(observer));
            if (scheduler == null)
                throw new ArgumentNullException(nameof(scheduler));
            if (values == null)
                throw new ArgumentNullException(nameof(values));

            throw new NotImplementedException();
        }
    }
}

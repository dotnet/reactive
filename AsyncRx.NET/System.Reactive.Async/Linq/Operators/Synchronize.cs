// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information. 

using System.Threading;

namespace System.Reactive.Linq
{
    public partial class AsyncObservable
    {
        public static IAsyncObservable<TSource> Synchronize<TSource>(this IAsyncObservable<TSource> source)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));

            return Create(source, static (source, observer) => source.SubscribeSafeAsync(AsyncObserver.Synchronize(observer)));
        }

        public static IAsyncObservable<TSource> Synchronize<TSource>(this IAsyncObservable<TSource> source, AsyncGate gate)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));
            if (gate == null)
                throw new ArgumentNullException(nameof(gate));

            return CreateAsyncObservable<TSource>.From(
                source,
                gate,
                static (source, gate, observer) => source.SubscribeSafeAsync(AsyncObserver.Synchronize(observer, gate)));
        }
    }

    public partial class AsyncObserver
    {
        public static IAsyncObserver<TSource> Synchronize<TSource>(IAsyncObserver<TSource> observer)
        {
            if (observer == null)
                throw new ArgumentNullException(nameof(observer));

            return Synchronize(observer, new AsyncGate());
        }

        public static IAsyncObserver<TSource> Synchronize<TSource>(IAsyncObserver<TSource> observer, AsyncGate gate)
        {
            if (observer == null)
                throw new ArgumentNullException(nameof(observer));
            if (gate == null)
                throw new ArgumentNullException(nameof(gate));

            return Create<TSource>(
                async x =>
                {
                    using (await gate.LockAsync().ConfigureAwait(false))
                    {
                        await observer.OnNextAsync(x).ConfigureAwait(false);
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
                        await observer.OnCompletedAsync().ConfigureAwait(false);
                    }
                }
            );
        }
    }
}

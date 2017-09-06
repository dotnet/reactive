// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the Apache 2.0 License.
// See the LICENSE file in the project root for more information. 

namespace System.Reactive.Linq
{
    partial class AsyncObservable
    {
        public static IAsyncObservable<T> Take<T>(this IAsyncObservable<T> source, int count)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));
            if (count < 0)
                throw new ArgumentOutOfRangeException(nameof(count));

            if (count == 0)
            {
                return Empty<T>();
            }

            return Create<T>(observer => source.SubscribeAsync(observer.Take(count)));
        }
    }

    partial class AsyncObserver
    {
        public static IAsyncObserver<T> Take<T>(this IAsyncObserver<T> observer, int count)
        {
            if (observer == null)
                throw new ArgumentNullException(nameof(observer));
            if (count <= 0)
                throw new ArgumentOutOfRangeException(nameof(count));

            return Create<T>(
                async x =>
                {
                    var remaining = --count;

                    await observer.OnNextAsync(x).ConfigureAwait(false);

                    if (remaining == 0)
                    {
                        await observer.OnCompletedAsync().ConfigureAwait(false);
                    }
                },
                observer.OnErrorAsync,
                observer.OnCompletedAsync
            );
        }
    }
}

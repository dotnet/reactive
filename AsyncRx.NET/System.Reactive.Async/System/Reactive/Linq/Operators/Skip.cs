// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the Apache 2.0 License.
// See the LICENSE file in the project root for more information. 

namespace System.Reactive.Linq
{
    partial class AsyncObservable
    {
        public static IAsyncObservable<T> Skip<T>(this IAsyncObservable<T> source, int count)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));
            if (count < 0)
                throw new ArgumentOutOfRangeException(nameof(count));

            if (count == 0)
            {
                return source;
            }

            return Create<T>(observer => source.SubscribeAsync(AsyncObserver.Skip(observer, count)));
        }
    }

    partial class AsyncObserver
    {
        public static IAsyncObserver<T> Skip<T>(IAsyncObserver<T> observer, int count)
        {
            if (observer == null)
                throw new ArgumentNullException(nameof(observer));
            if (count <= 0)
                throw new ArgumentOutOfRangeException(nameof(count));

            return Create<T>(
                async x =>
                {
                    if (count == 0)
                    {
                        await observer.OnNextAsync(x).ConfigureAwait(false);
                    }
                    else
                    {
                        --count;
                    }
                },
                observer.OnErrorAsync,
                observer.OnCompletedAsync
            );
        }
    }
}

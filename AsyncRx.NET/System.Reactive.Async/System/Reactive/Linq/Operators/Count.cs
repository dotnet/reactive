// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the Apache 2.0 License.
// See the LICENSE file in the project root for more information. 

namespace System.Reactive.Linq
{
    partial class AsyncObservable
    {
        public static IAsyncObservable<int> Count<TSource>(this IAsyncObservable<TSource> source)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));

            return Create<int>(observer => source.SubscribeAsync(AsyncObserver.Count<TSource>(observer)));
        }
    }

    partial class AsyncObserver
    {
        public static IAsyncObserver<TSource> Count<TSource>(IAsyncObserver<int> observer)
        {
            if (observer == null)
                throw new ArgumentNullException(nameof(observer));

            var count = 0;

            return Create<TSource>(
                async x =>
                {
                    try
                    {
                        checked
                        {
                            count++;
                        }
                    }
                    catch (Exception ex)
                    {
                        await observer.OnErrorAsync(ex).ConfigureAwait(false);
                        return;
                    }
                },
                observer.OnErrorAsync,
                async () =>
                {
                    await observer.OnNextAsync(count).ConfigureAwait(false);
                    await observer.OnCompletedAsync().ConfigureAwait(false);
                }
            );
        }
    }
}

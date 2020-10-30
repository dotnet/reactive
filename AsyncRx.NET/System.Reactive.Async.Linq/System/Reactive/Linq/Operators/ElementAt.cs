// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information. 

using System.Threading.Tasks;

namespace System.Reactive.Linq
{
    public partial class AsyncObservable
    {
        public static IAsyncObservable<TSource> ElementAt<TSource>(this IAsyncObservable<TSource> source, int index)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));
            if (index < 0)
                throw new ArgumentOutOfRangeException(nameof(index));

            return Create(
                source,
                index,
                static (source, index, observer) => source.SubscribeSafeAsync(AsyncObserver.ElementAt(observer, index)));
        }
    }

    public partial class AsyncObserver
    {
        public static IAsyncObserver<TSource> ElementAt<TSource>(IAsyncObserver<TSource> observer, int index)
        {
            if (observer == null)
                throw new ArgumentNullException(nameof(observer));
            if (index < 0)
                throw new ArgumentOutOfRangeException(nameof(index));

            return Create<TSource>(
                async x =>
                {
                    if (index-- == 0)
                    {
                        await observer.OnNextAsync(x).ConfigureAwait(false);
                        await observer.OnCompletedAsync().ConfigureAwait(false);
                    }
                },
                observer.OnErrorAsync,
                async () =>
                {
                    await observer.OnErrorAsync(new ArgumentOutOfRangeException("The element at the specified index was not found.")).ConfigureAwait(false);
                }
            );
        }
    }
}

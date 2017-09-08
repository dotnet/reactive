// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the Apache 2.0 License.
// See the LICENSE file in the project root for more information. 

using System.Reactive.Disposables;
using System.Threading.Tasks;

namespace System.Reactive.Linq
{
    partial class AsyncObservable
    {
        public static IAsyncObservable<TSource> Finally<TSource>(this IAsyncObservable<TSource> source, Action finallyAction)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));
            if (finallyAction == null)
                throw new ArgumentNullException(nameof(finallyAction));

            return Create<TSource>(async observer =>
            {
                var subscription = await source.SubscribeSafeAsync(observer).ConfigureAwait(false);

                return AsyncDisposable.Create(async () =>
                {
                    try
                    {
                        await subscription.DisposeAsync().ConfigureAwait(false);
                    }
                    finally
                    {
                        finallyAction();
                    }
                });
            });
        }

        public static IAsyncObservable<TSource> Finally<TSource>(this IAsyncObservable<TSource> source, Func<Task> finallyAction)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));
            if (finallyAction == null)
                throw new ArgumentNullException(nameof(finallyAction));

            return Create<TSource>(async observer =>
            {
                var subscription = await source.SubscribeSafeAsync(observer).ConfigureAwait(false);

                return AsyncDisposable.Create(async () =>
                {
                    try
                    {
                        await subscription.DisposeAsync().ConfigureAwait(false);
                    }
                    finally
                    {
                        await finallyAction().ConfigureAwait(false);
                    }
                });
            });
        }
    }
}

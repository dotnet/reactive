// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the Apache 2.0 License.
// See the LICENSE file in the project root for more information. 

using System.Reactive.Disposables;
using System.Reactive.Subjects;
using System.Threading;

namespace System.Reactive.Linq
{
    partial class AsyncObservable
    {
        public static IAsyncObservable<TSource> RefCount<TSource>(this IConnectableAsyncObservable<TSource> source)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));

            var gate = new AsyncLock();
            var count = 0;
            var connectable = default(IAsyncDisposable);

            return Create<TSource>(async observer =>
            {
                var subscription = await source.SubscribeSafeAsync(observer).ConfigureAwait(false);

                using (await gate.LockAsync().ConfigureAwait(false))
                {
                    if (++count == 1)
                    {
                        connectable = await source.ConnectAsync().ConfigureAwait(false);
                    }
                }

                return AsyncDisposable.Create(async () =>
                {
                    await subscription.DisposeAsync().ConfigureAwait(false);

                    using (await gate.LockAsync().ConfigureAwait(false))
                    {
                        if (--count == 0)
                        {
                            await connectable.DisposeAsync().ConfigureAwait(false);
                        }
                    }
                });
            });
        }
    }
}

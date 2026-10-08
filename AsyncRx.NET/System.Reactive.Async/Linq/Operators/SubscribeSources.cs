// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information. 

using System.Reactive.Disposables;
using System.Threading.Tasks;

namespace System.Reactive.Linq
{
    // The subscription plumbing that the operators over several sources (Zip, CombineLatest)
    // share: subscribing to every source and holding the subscriptions together, and letting
    // each source go as soon as it completes. Here rather than in either operator's file, so
    // that neither hosts the other's plumbing.
    public partial class AsyncObservable
    {
        // Collects subscriptions that were all started before any was awaited, so that the
        // sources are subscribed concurrently. If any of them faults, the rest are still
        // collected and everything collected so far is disposed, so that no subscription is
        // left running with no one holding it.
        internal static async ValueTask CollectAsync(CompositeAsyncDisposable composite, params ValueTask<IAsyncDisposable>[] subscriptions)
        {
            Exception error = null;

            foreach (var subscription in subscriptions)
            {
                try
                {
                    await composite.AddAsync(await subscription.ConfigureAwait(false)).ConfigureAwait(false);
                }
                catch (Exception ex)
                {
                    error ??= ex;
                }
            }

            if (error != null)
            {
                await composite.DisposeAsync().ConfigureAwait(false);
                System.Runtime.ExceptionServices.ExceptionDispatchInfo.Capture(error).Throw();
            }
        }

        // Subscribes one of a combining operator's sources (Zip, CombineLatest) so that its
        // subscription is released as soon as that source completes, while the others carry on,
        // as Rx.NET's sinks do; the holder returned is what the operator's composite disposable
        // keeps. A source that completes during the subscribe call disposes the holder before it
        // is assigned, which disposes the subscription the moment it is.
        internal static async ValueTask<IAsyncDisposable> SubscribeReleasingOnCompletedAsync<TSource>(IAsyncObservable<TSource> source, IAsyncObserver<TSource> observer)
        {
            var subscription = new SingleAssignmentAsyncDisposable();

            var releasing = AsyncObserver.Create<TSource>(
                observer.OnNextAsync,
                observer.OnErrorAsync,
                async () =>
                {
                    await subscription.DisposeAsync().ConfigureAwait(false);
                    await observer.OnCompletedAsync().ConfigureAwait(false);
                });

            await subscription.AssignAsync(await source.SubscribeSafeAsync(releasing).ConfigureAwait(false)).ConfigureAwait(false);

            return subscription;
        }
    }
}

// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the Apache 2.0 License.
// See the LICENSE file in the project root for more information. 

using System.Collections.Generic;
using System.Reactive.Disposables;
using System.Reactive.Joins;
using System.Threading;

namespace System.Reactive.Linq
{
    partial class AsyncObservable
    {
        public static IAsyncObservable<TResult> When<TResult>(IEnumerable<AsyncPlan<TResult>> plans)
        {
            if (plans == null)
                throw new ArgumentNullException(nameof(plans));

            return Create<TResult>(async observer =>
            {
                var externalSubscriptions = new Dictionary<object, IAsyncJoinObserver>();
                var gate = new AsyncLock();
                var activePlans = new List<ActiveAsyncPlan>();

                var outputObserver = AsyncObserver.Create<TResult>(
                    observer.OnNextAsync,
                    async ex =>
                    {
                        foreach (var subscription in externalSubscriptions.Values)
                        {
                            await subscription.DisposeAsync().ConfigureAwait(false);
                        }

                        await observer.OnErrorAsync(ex).ConfigureAwait(false);
                    },
                    observer.OnCompletedAsync
                );

                try
                {
                    foreach (var plan in plans)
                    {
                        var activatedPlan = plan.Activate(externalSubscriptions, outputObserver, async activePlan =>
                        {
                            activePlans.Remove(activePlan);

                            if (activePlans.Count == 0)
                            {
                                await outputObserver.OnCompletedAsync().ConfigureAwait(false);
                            }
                        });

                        activePlans.Add(activatedPlan);
                    }
                }
                catch (Exception ex)
                {
                    return await Throw<TResult>(ex).SubscribeAsync(observer).ConfigureAwait(false);
                }

                var d = new CompositeAsyncDisposable();

                foreach (var joinObserver in externalSubscriptions.Values)
                {
                    await joinObserver.SubscribeAsync(gate).ConfigureAwait(false);
                    await d.AddAsync(joinObserver).ConfigureAwait(false);
                }

                return d;
            });
        }

        public static IAsyncObservable<TResult> When<TResult>(params AsyncPlan<TResult>[] plans) => When((IEnumerable<AsyncPlan<TResult>>)plans);
    }
}

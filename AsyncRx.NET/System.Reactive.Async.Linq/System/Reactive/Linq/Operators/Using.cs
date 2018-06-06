// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the Apache 2.0 License.
// See the LICENSE file in the project root for more information. 

using System.Reactive.Disposables;
using System.Threading.Tasks;

namespace System.Reactive.Linq
{
    partial class AsyncObservable
    {
        public static IAsyncObservable<TResult> Using<TResult, TResource>(Func<TResource> resourceFactory, Func<TResource, IAsyncObservable<TResult>> observableFactory)
            where TResource : IDisposable
        {
            if (resourceFactory == null)
                throw new ArgumentNullException(nameof(resourceFactory));
            if (observableFactory == null)
                throw new ArgumentNullException(nameof(observableFactory));

            return Create<TResult>(async observer =>
            {
                TResource resource;
                try
                {
                    resource = resourceFactory();
                }
                catch (Exception ex)
                {
                    await observer.OnErrorAsync(ex).ConfigureAwait(false);
                    return AsyncDisposable.Nop;
                }

                IAsyncObservable<TResult> observable;
                try
                {
                    observable = observableFactory(resource);
                }
                catch (Exception ex)
                {
                    using (resource)
                    {
                        await observer.OnErrorAsync(ex).ConfigureAwait(false);
                        return AsyncDisposable.Nop;
                    }
                }

                var subscription = await observable.SubscribeSafeAsync(observer).ConfigureAwait(false);

                return AsyncDisposable.Create(async () =>
                {
                    using (resource)
                    {
                        await subscription.DisposeAsync().ConfigureAwait(false);
                    }
                });
            });
        }

        public static IAsyncObservable<TResult> UsingAsync<TResult, TResource>(Func<Task<TResource>> resourceFactory, Func<TResource, Task<IAsyncObservable<TResult>>> observableFactory)
            where TResource : IDisposable
        {
            if (resourceFactory == null)
                throw new ArgumentNullException(nameof(resourceFactory));
            if (observableFactory == null)
                throw new ArgumentNullException(nameof(observableFactory));

            return Create<TResult>(async observer =>
            {
                TResource resource;
                try
                {
                    resource = await resourceFactory().ConfigureAwait(false);
                }
                catch (Exception ex)
                {
                    await observer.OnErrorAsync(ex).ConfigureAwait(false);
                    return AsyncDisposable.Nop;
                }

                IAsyncObservable<TResult> observable;
                try
                {
                    observable = await observableFactory(resource).ConfigureAwait(false);
                }
                catch (Exception ex)
                {
                    using (resource)
                    {
                        await observer.OnErrorAsync(ex).ConfigureAwait(false);
                        return AsyncDisposable.Nop;
                    }
                }

                var subscription = await observable.SubscribeSafeAsync(observer).ConfigureAwait(false);

                return AsyncDisposable.Create(async () =>
                {
                    using (resource)
                    {
                        await subscription.DisposeAsync().ConfigureAwait(false);
                    }
                });
            });
        }

        public static IAsyncObservable<TResult> UsingAwait<TResult, TResource>(Func<TResource> resourceFactory, Func<TResource, IAsyncObservable<TResult>> observableFactory)
            where TResource : IAsyncDisposable
        {
            if (resourceFactory == null)
                throw new ArgumentNullException(nameof(resourceFactory));
            if (observableFactory == null)
                throw new ArgumentNullException(nameof(observableFactory));

            return Create<TResult>(async observer =>
            {
                TResource resource;
                try
                {
                    resource = resourceFactory();
                }
                catch (Exception ex)
                {
                    await observer.OnErrorAsync(ex).ConfigureAwait(false);
                    return AsyncDisposable.Nop;
                }

                IAsyncObservable<TResult> observable;
                try
                {
                    observable = observableFactory(resource);
                }
                catch (Exception ex)
                {
                    try
                    {
                        await observer.OnErrorAsync(ex).ConfigureAwait(false);
                        return AsyncDisposable.Nop;
                    }
                    finally
                    {
                        await resource.DisposeAsync().ConfigureAwait(false);
                    }
                }

                var subscription = await observable.SubscribeSafeAsync(observer).ConfigureAwait(false);

                return AsyncDisposable.Create(async () =>
                {
                    try
                    {
                        await subscription.DisposeAsync().ConfigureAwait(false);
                    }
                    finally
                    {
                        await resource.DisposeAsync().ConfigureAwait(false);
                    }
                });
            });
        }

        public static IAsyncObservable<TResult> UsingAwaitAsync<TResult, TResource>(Func<Task<TResource>> resourceFactory, Func<TResource, Task<IAsyncObservable<TResult>>> observableFactory)
            where TResource : IAsyncDisposable
        {
            if (resourceFactory == null)
                throw new ArgumentNullException(nameof(resourceFactory));
            if (observableFactory == null)
                throw new ArgumentNullException(nameof(observableFactory));

            return Create<TResult>(async observer =>
            {
                TResource resource;
                try
                {
                    resource = await resourceFactory().ConfigureAwait(false);
                }
                catch (Exception ex)
                {
                    await observer.OnErrorAsync(ex).ConfigureAwait(false);
                    return AsyncDisposable.Nop;
                }

                IAsyncObservable<TResult> observable;
                try
                {
                    observable = await observableFactory(resource).ConfigureAwait(false);
                }
                catch (Exception ex)
                {
                    try
                    {
                        await observer.OnErrorAsync(ex).ConfigureAwait(false);
                        return AsyncDisposable.Nop;
                    }
                    finally
                    {
                        await resource.DisposeAsync().ConfigureAwait(false);
                    }
                }

                var subscription = await observable.SubscribeSafeAsync(observer).ConfigureAwait(false);

                return AsyncDisposable.Create(async () =>
                {
                    try
                    {
                        await subscription.DisposeAsync().ConfigureAwait(false);
                    }
                    finally
                    {
                        await resource.DisposeAsync().ConfigureAwait(false);
                    }
                });
            });
        }
    }
}

// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the Apache 2.0 License.
// See the LICENSE file in the project root for more information. 

using System.Threading.Tasks;

namespace System.Reactive.Linq
{
    public static class AsyncObservable
    {
        public static IAsyncObservable<T> Create<T>(Func<IAsyncObserver<T>, Task<IAsyncDisposable>> subscribeAsync)
        {
            if (subscribeAsync == null)
                throw new ArgumentNullException(nameof(subscribeAsync));

            return new AnonymousAsyncObservable<T>(subscribeAsync);
        }

        private sealed class AnonymousAsyncObservable<T> : IAsyncObservable<T>
        {
            private readonly Func<IAsyncObserver<T>, Task<IAsyncDisposable>> _subscribeAsync;

            public AnonymousAsyncObservable(Func<IAsyncObserver<T>, Task<IAsyncDisposable>> subscribeAsync)
            {
                _subscribeAsync = subscribeAsync;
            }

            public async Task<IAsyncDisposable> SubscribeAsync(IAsyncObserver<T> observer)
            {
                if (observer == null)
                    throw new ArgumentNullException(nameof(observer));

                var autoDetach = new AutoDetachAsyncObserver(observer);

                var subscription = await _subscribeAsync(autoDetach).ConfigureAwait(false);

                await autoDetach.AssignAsync(subscription);

                return autoDetach;
            }

            private sealed class AutoDetachAsyncObserver : AsyncObserverBase<T>, IAsyncDisposable
            {
                private readonly IAsyncObserver<T> _observer;

                public AutoDetachAsyncObserver(IAsyncObserver<T> observer)
                {
                    _observer = observer;
                }

                public Task AssignAsync(IAsyncDisposable subscription)
                {
                    throw new NotImplementedException();
                }

                public Task DisposeAsync()
                {
                    throw new NotImplementedException();
                }

                protected override Task OnCompletedAsyncCore()
                {
                    throw new NotImplementedException();
                }

                protected override Task OnErrorAsyncCore(Exception error)
                {
                    throw new NotImplementedException();
                }

                protected override Task OnNextAsyncCore(T value) => _observer.OnNextAsync(value);
            }
        }
    }
}

// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the Apache 2.0 License.
// See the LICENSE file in the project root for more information. 

using System.Threading.Tasks;

namespace System.Reactive.Linq
{
    public static partial class AsyncObservable
    {
        public static IAsyncObservable<T> Create<T>(Func<IAsyncObserver<T>, Task<IAsyncDisposable>> subscribeAsync)
        {
            if (subscribeAsync == null)
                throw new ArgumentNullException(nameof(subscribeAsync));

            return new AnonymousAsyncObservable<T>(subscribeAsync);
        }

        private sealed class AnonymousAsyncObservable<T> : AsyncObservableBase<T>
        {
            private readonly Func<IAsyncObserver<T>, Task<IAsyncDisposable>> _subscribeAsync;

            public AnonymousAsyncObservable(Func<IAsyncObserver<T>, Task<IAsyncDisposable>> subscribeAsync)
            {
                _subscribeAsync = subscribeAsync;
            }

            protected override Task<IAsyncDisposable> SubscribeAsyncCore(IAsyncObserver<T> observer) => _subscribeAsync(observer);
        }
    }
}

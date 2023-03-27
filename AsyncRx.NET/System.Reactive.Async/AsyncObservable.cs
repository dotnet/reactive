// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information. 

using System.Threading.Tasks;

namespace System.Reactive
{
    public class AsyncObservable<T> : AsyncObservableBase<T>
    {
        private readonly Func<IAsyncObserver<T>, ValueTask<IAsyncDisposable>> _subscribeAsync;

        public AsyncObservable(Func<IAsyncObserver<T>, ValueTask<IAsyncDisposable>> subscribeAsync)
        {
            _subscribeAsync = subscribeAsync ?? throw new ArgumentNullException(nameof(subscribeAsync));
        }

        protected override ValueTask<IAsyncDisposable> SubscribeAsyncCore(IAsyncObserver<T> observer)
        {
            if (observer == null)
                throw new ArgumentNullException(nameof(observer));

            return _subscribeAsync(observer);
        }
    }
}

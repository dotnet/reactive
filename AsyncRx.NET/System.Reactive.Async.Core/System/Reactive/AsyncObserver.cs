// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the Apache 2.0 License.
// See the LICENSE file in the project root for more information. 

using System.Threading.Tasks;

namespace System.Reactive
{
    public class AsyncObserver<T> : AsyncObserverBase<T>
    {
        private readonly Func<T, Task> _onNextAsync;
        private readonly Func<Exception, Task> _onErrorAsync;
        private readonly Func<Task> _onCompletedAsync;

        public AsyncObserver(Func<T, Task> onNextAsync, Func<Exception, Task> onErrorAsync, Func<Task> onCompletedAsync)
        {
            if (onNextAsync == null)
                throw new ArgumentNullException(nameof(onNextAsync));
            if (onErrorAsync == null)
                throw new ArgumentNullException(nameof(onErrorAsync));
            if (onCompletedAsync == null)
                throw new ArgumentNullException(nameof(onCompletedAsync));

            _onNextAsync = onNextAsync;
            _onErrorAsync = onErrorAsync;
            _onCompletedAsync = onCompletedAsync;
        }

        protected override Task OnCompletedAsyncCore() => _onCompletedAsync();

        protected override Task OnErrorAsyncCore(Exception error) => _onErrorAsync(error ?? throw new ArgumentNullException(nameof(error)));

        protected override Task OnNextAsyncCore(T value) => _onNextAsync(value);
    }
}

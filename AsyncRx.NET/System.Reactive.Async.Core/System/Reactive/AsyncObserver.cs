// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information. 

using System.Threading.Tasks;

namespace System.Reactive
{
    public class AsyncObserver<T> : AsyncObserverBase<T>
    {
        private readonly Func<T, ValueTask> _onNextAsync;
        private readonly Func<Exception, ValueTask> _onErrorAsync;
        private readonly Func<ValueTask> _onCompletedAsync;

        public AsyncObserver(Func<T, ValueTask> onNextAsync, Func<Exception, ValueTask> onErrorAsync, Func<ValueTask> onCompletedAsync)
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

        protected override ValueTask OnCompletedAsyncCore() => _onCompletedAsync();

        protected override ValueTask OnErrorAsyncCore(Exception error) => _onErrorAsync(error ?? throw new ArgumentNullException(nameof(error)));

        protected override ValueTask OnNextAsyncCore(T value) => _onNextAsync(value);
    }
}

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
            _onNextAsync = onNextAsync ?? throw new ArgumentNullException(nameof(onNextAsync));
            _onErrorAsync = onErrorAsync ?? throw new ArgumentNullException(nameof(onErrorAsync));
            _onCompletedAsync = onCompletedAsync ?? throw new ArgumentNullException(nameof(onCompletedAsync));
        }

        protected override ValueTask OnCompletedAsyncCore() => _onCompletedAsync();

        protected override ValueTask OnErrorAsyncCore(Exception error) => _onErrorAsync(error ?? throw new ArgumentNullException(nameof(error)));

        protected override ValueTask OnNextAsyncCore(T value) => _onNextAsync(value);
    }
}

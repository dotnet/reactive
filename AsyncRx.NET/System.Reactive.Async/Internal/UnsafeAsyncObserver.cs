// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information. 

using System.Threading.Tasks;

namespace System.Reactive
{
    public class UnsafeAsyncObserver<T> : IAsyncObserver<T>
    {
        private readonly Func<T, ValueTask> _onNextAsync;
        private readonly Func<Exception, ValueTask> _onErrorAsync;
        private readonly Func<ValueTask> _onCompletedAsync;

        public UnsafeAsyncObserver(Func<T, ValueTask> onNextAsync, Func<Exception, ValueTask> onErrorAsync, Func<ValueTask> onCompletedAsync)
        {
            _onNextAsync = onNextAsync ?? throw new ArgumentNullException(nameof(onNextAsync));
            _onErrorAsync = onErrorAsync ?? throw new ArgumentNullException(nameof(onErrorAsync));
            _onCompletedAsync = onCompletedAsync ?? throw new ArgumentNullException(nameof(onCompletedAsync));
        }

        public ValueTask OnCompletedAsync() => _onCompletedAsync();

        public ValueTask OnErrorAsync(Exception error) => _onErrorAsync(error ?? throw new ArgumentNullException(nameof(error)));

        public ValueTask OnNextAsync(T value) => _onNextAsync(value);
    }
}

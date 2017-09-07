// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the Apache 2.0 License.
// See the LICENSE file in the project root for more information. 

using System.Runtime.ExceptionServices;
using System.Threading.Tasks;

namespace System.Reactive.Linq
{
    public static partial class AsyncObserver
    {
        public static IAsyncObserver<T> Create<T>(Func<T, Task> onNextAsync)
        {
            if (onNextAsync == null)
                throw new ArgumentNullException(nameof(onNextAsync));

            return new AnonymousAsyncObserver<T>(
                onNextAsync,
                ex =>
                {
                    ExceptionDispatchInfo.Capture(ex).Throw();
                    return Task.CompletedTask;
                },
                () => Task.CompletedTask
            );
        }

        public static IAsyncObserver<T> Create<T>(Func<T, Task> onNextAsync, Func<Exception, Task> onErrorAsync, Func<Task> onCompletedAsync)
        {
            if (onNextAsync == null)
                throw new ArgumentNullException(nameof(onNextAsync));
            if (onErrorAsync == null)
                throw new ArgumentNullException(nameof(onErrorAsync));
            if (onCompletedAsync == null)
                throw new ArgumentNullException(nameof(onCompletedAsync));

            return new AnonymousAsyncObserver<T>(onNextAsync, onErrorAsync, onCompletedAsync);
        }

        private sealed class AnonymousAsyncObserver<T> : AsyncObserverBase<T>
        {
            private readonly Func<T, Task> _onNextAsync;
            private readonly Func<Exception, Task> _onErrorAsync;
            private readonly Func<Task> _onCompletedAsync;

            public AnonymousAsyncObserver(Func<T, Task> onNextAsync, Func<Exception, Task> onErrorAsync, Func<Task> onCompletedAsync)
            {
                _onNextAsync = onNextAsync;
                _onErrorAsync = onErrorAsync;
                _onCompletedAsync = onCompletedAsync;
            }

            protected override Task OnCompletedAsyncCore() => _onCompletedAsync();

            protected override Task OnErrorAsyncCore(Exception error) => _onErrorAsync(error);

            protected override Task OnNextAsyncCore(T value) => _onNextAsync(value);
        }
    }
}

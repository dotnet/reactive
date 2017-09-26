// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the Apache 2.0 License.
// See the LICENSE file in the project root for more information. 

using System.Threading.Tasks;

namespace System.Reactive.Linq
{
    public static partial class AsyncObserver
    {
        public static IAsyncObserver<T> Create<T>(Func<T, Task> onNextAsync)
        {
            if (onNextAsync == null)
                throw new ArgumentNullException(nameof(onNextAsync));

            return new AsyncObserver<T>(
                onNextAsync,
                ex => Task.FromException(ex),
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

            return new AsyncObserver<T>(onNextAsync, onErrorAsync, onCompletedAsync);
        }

        internal static IAsyncObserver<T> CreateUnsafe<T>(Func<T, Task> onNextAsync, Func<Exception, Task> onErrorAsync, Func<Task> onCompletedAsync)
        {
            if (onNextAsync == null)
                throw new ArgumentNullException(nameof(onNextAsync));
            if (onErrorAsync == null)
                throw new ArgumentNullException(nameof(onErrorAsync));
            if (onCompletedAsync == null)
                throw new ArgumentNullException(nameof(onCompletedAsync));

            return new UnsafeAsyncObserver<T>(onNextAsync, onErrorAsync, onCompletedAsync);
        }
    }
}

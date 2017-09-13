// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the Apache 2.0 License.
// See the LICENSE file in the project root for more information. 

using System.Reactive;
using System.Threading.Tasks;

namespace System
{
    public static class AsyncObservableExtensions
    {
        public static Task<IAsyncDisposable> SubscribeAsync<T>(this IAsyncObservable<T> source, Func<T, Task> onNextAsync, Func<Exception, Task> onErrorAsync, Func<Task> onCompletedAsync)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));
            if (onNextAsync == null)
                throw new ArgumentNullException(nameof(onNextAsync));
            if (onErrorAsync == null)
                throw new ArgumentNullException(nameof(onErrorAsync));
            if (onCompletedAsync == null)
                throw new ArgumentNullException(nameof(onCompletedAsync));

            return source.SubscribeAsync(new AsyncObserver<T>(onNextAsync, onErrorAsync, onCompletedAsync));
        }
    }
}

// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information. 

using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace System.Reactive.Subjects
{
    public sealed class ConcurrentAsyncAsyncSubject<T> : AsyncAsyncSubject<T>
    {
        protected override ValueTask OnCompletedAsyncCore(IEnumerable<IAsyncObserver<T>> observers) => new(Task.WhenAll(observers.Select(observer => observer.OnCompletedAsync().AsTask())));

        protected override ValueTask OnErrorAsyncCore(IEnumerable<IAsyncObserver<T>> observers, Exception error) => new(Task.WhenAll(observers.Select(observer => observer.OnErrorAsync(error).AsTask())));

        protected override ValueTask OnNextAsyncCore(IEnumerable<IAsyncObserver<T>> observers, T value) => new(Task.WhenAll(observers.Select(observer => observer.OnNextAsync(value).AsTask())));
    }
}

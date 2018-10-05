// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the Apache 2.0 License.
// See the LICENSE file in the project root for more information. 

using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace System.Reactive.Subjects
{
    public sealed class ConcurrentAsyncAsyncSubject<T> : AsyncAsyncSubject<T>
    {
        protected override Task OnCompletedAsyncCore(IEnumerable<IAsyncObserver<T>> observers) => Task.WhenAll(observers.Select(observer => observer.OnCompletedAsync()));

        protected override Task OnErrorAsyncCore(IEnumerable<IAsyncObserver<T>> observers, Exception error) => Task.WhenAll(observers.Select(observer => observer.OnErrorAsync(error)));

        protected override Task OnNextAsyncCore(IEnumerable<IAsyncObserver<T>> observers, T value) => Task.WhenAll(observers.Select(observer => observer.OnNextAsync(value)));
    }
}

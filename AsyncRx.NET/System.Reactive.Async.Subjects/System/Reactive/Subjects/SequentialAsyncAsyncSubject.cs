// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the Apache 2.0 License.
// See the LICENSE file in the project root for more information. 

using System.Collections.Generic;
using System.Threading.Tasks;

namespace System.Reactive.Subjects
{
    public sealed class SequentialAsyncAsyncSubject<T> : AsyncAsyncSubject<T>
    {
        protected override async Task OnCompletedAsyncCore(IEnumerable<IAsyncObserver<T>> observers)
        {
            foreach (var observer in observers)
            {
                await observer.OnCompletedAsync().ConfigureAwait(false);
            }
        }

        protected override async Task OnErrorAsyncCore(IEnumerable<IAsyncObserver<T>> observers, Exception error)
        {
            foreach (var observer in observers)
            {
                await observer.OnErrorAsync(error).ConfigureAwait(false);
            }
        }

        protected override async Task OnNextAsyncCore(IEnumerable<IAsyncObserver<T>> observers, T value)
        {
            foreach (var observer in observers)
            {
                await observer.OnNextAsync(value).ConfigureAwait(false);
            }
        }
    }
}

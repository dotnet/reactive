// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information. 

using System.Collections.Generic;
using System.Threading.Tasks;

namespace System.Reactive.Subjects
{
    public sealed class SequentialAsyncAsyncSubject<T> : AsyncAsyncSubject<T>
    {
        protected override async ValueTask OnCompletedAsyncCore(IEnumerable<IAsyncObserver<T>> observers)
        {
            foreach (var observer in observers)
            {
                await observer.OnCompletedAsync().ConfigureAwait(false);
            }
        }

        protected override async ValueTask OnErrorAsyncCore(IEnumerable<IAsyncObserver<T>> observers, Exception error)
        {
            foreach (var observer in observers)
            {
                await observer.OnErrorAsync(error).ConfigureAwait(false);
            }
        }

        protected override async ValueTask OnNextAsyncCore(IEnumerable<IAsyncObserver<T>> observers, T value)
        {
            foreach (var observer in observers)
            {
                await observer.OnNextAsync(value).ConfigureAwait(false);
            }
        }
    }
}

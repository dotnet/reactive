// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the Apache 2.0 License.
// See the LICENSE file in the project root for more information. 

using System.Reactive.Disposables;

namespace System.Reactive.Linq
{
    partial class AsyncObservable
    {
        public static IAsyncObservable<TSource> Return<TSource>(TSource value)
        {
            return Create<TSource>(async observer =>
            {
                await observer.OnNextAsync(value).ConfigureAwait(false);
                await observer.OnCompletedAsync().ConfigureAwait(false);
                return AsyncDisposable.Nop;
            });
        }
    }
}

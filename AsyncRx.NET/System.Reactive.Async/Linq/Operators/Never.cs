// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information. 

using System.Reactive.Disposables;
using System.Threading.Tasks;

namespace System.Reactive.Linq
{
    public partial class AsyncObservable
    {
        public static IAsyncObservable<TSource> Never<TSource>()
        {
            return Create<TSource>(observer => new ValueTask<IAsyncDisposable>(AsyncDisposable.Nop));
        }
    }
}

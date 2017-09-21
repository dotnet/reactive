// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the Apache 2.0 License.
// See the LICENSE file in the project root for more information. 

using System.Threading.Tasks;

namespace System.Reactive
{
    internal interface IScheduledAsyncObserver<T> : IAsyncObserver<T>, IAsyncDisposable
    {
        Task EnsureActive();

        Task EnsureActive(int count);
    }
}

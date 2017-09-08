// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the Apache 2.0 License.
// See the LICENSE file in the project root for more information. 

using System.Threading.Tasks;

namespace System.Reactive.Subjects
{
    public interface IConnectableAsyncObservable<out T> : IAsyncObservable<T>
    {
        Task<IAsyncDisposable> ConnectAsync();
    }
}

// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the Apache 2.0 License.
// See the LICENSE file in the project root for more information. 

namespace System.Reactive.Subjects
{
    public interface IAsyncSubject<in TInput, out TOutput> : IAsyncObservable<TOutput>, IAsyncObserver<TInput>
    {
    }

    public interface IAsyncSubject<T> : IAsyncSubject<T, T>
    {
    }
}

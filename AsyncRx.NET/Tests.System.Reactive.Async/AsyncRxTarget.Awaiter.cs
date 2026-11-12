// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information. 

using System.Reactive.Linq;
using System.Reactive.Subjects;

using Tests.System.Reactive.Shared;

namespace Tests.System.Reactive.Async;

public sealed partial class AsyncRxTarget
{
    public SeqAwaiter<T> GetAwaiter<T>(Seq<T> source) =>
        new AsyncRxSeqAwaiter<T>(_bridge.Call<AsyncAsyncSubject<T>>(GetAwaiterImpl<T>, source));

    public SeqAwaiter<T> GetAwaiter<T>(ConnectableSeq<T> source) =>
        new AsyncRxSeqAwaiter<T>(
            _bridge.Call<AsyncAsyncSubject<T>>(GetAwaiterConnectableImpl<T>, source));

    public SeqAwaiter<T> RunAsync<T>(Seq<T> source, CancellationToken token) =>
        new AsyncRxSeqAwaiter<T>(
            _bridge.Call<AsyncAsyncSubject<T>>(RunAsyncImpl<T>, source, token));

    public SeqAwaiter<T> RunAsync<T>(ConnectableSeq<T> source, CancellationToken token) =>
        new AsyncRxSeqAwaiter<T>(
            _bridge.Call<AsyncAsyncSubject<T>>(RunAsyncConnectableImpl<T>, source, token));

    private static AsyncAsyncSubject<T> GetAwaiterImpl<T>(IAsyncObservable<T> source) =>
        source.GetAwaiter();

    private static AsyncAsyncSubject<T> GetAwaiterConnectableImpl<T>(
        IConnectableAsyncObservable<T> source) =>
        source.GetAwaiter();

    private static AsyncAsyncSubject<T> RunAsyncImpl<T>(
        IAsyncObservable<T> source,
        CancellationToken token) =>
        source.RunAsync(token);

    private static AsyncAsyncSubject<T> RunAsyncConnectableImpl<T>(
        IConnectableAsyncObservable<T> source,
        CancellationToken token) =>
        source.RunAsync(token);
}

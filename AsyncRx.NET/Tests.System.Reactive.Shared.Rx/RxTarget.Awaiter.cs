// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information. 

using System.Reactive.Linq;
using System.Reactive.Subjects;

namespace Tests.System.Reactive.Shared.Rx;

public sealed partial class RxTarget
{
    public SeqAwaiter<T> GetAwaiter<T>(Seq<T> source) =>
        new RxSeqAwaiter<T>(_bridge.Call<AsyncSubject<T>>(GetAwaiterImpl<T>, source));

    public SeqAwaiter<T> GetAwaiter<T>(ConnectableSeq<T> source) =>
        new RxSeqAwaiter<T>(_bridge.Call<AsyncSubject<T>>(GetAwaiterConnectableImpl<T>, source));

    public SeqAwaiter<T> RunAsync<T>(Seq<T> source, CancellationToken token) =>
        new RxSeqAwaiter<T>(_bridge.Call<AsyncSubject<T>>(RunAsyncImpl<T>, source, token));

    public SeqAwaiter<T> RunAsync<T>(ConnectableSeq<T> source, CancellationToken token) =>
        new RxSeqAwaiter<T>(
            _bridge.Call<AsyncSubject<T>>(RunAsyncConnectableImpl<T>, source, token));

    private static AsyncSubject<T> GetAwaiterImpl<T>(IObservable<T> source) => source.GetAwaiter();

    private static AsyncSubject<T> GetAwaiterConnectableImpl<T>(IConnectableObservable<T> source) =>
        source.GetAwaiter();

    private static AsyncSubject<T> RunAsyncImpl<T>(IObservable<T> source, CancellationToken token) =>
        source.RunAsync(token);

    private static AsyncSubject<T> RunAsyncConnectableImpl<T>(
        IConnectableObservable<T> source,
        CancellationToken token) =>
        source.RunAsync(token);
}

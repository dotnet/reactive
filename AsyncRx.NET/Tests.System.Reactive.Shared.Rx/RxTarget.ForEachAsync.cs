// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information. 

using System.Reactive.Linq;

namespace Tests.System.Reactive.Shared.Rx;

public sealed partial class RxTarget
{
    public Task ForEachAsync<T>(Seq<T> source, Action<T> onNext, CancellationToken token) =>
        _bridge.Call<Task>(ForEachAsyncImpl<T>, source, onNext, token);

    public Task ForEachAsync<T>(Seq<T> source, Action<T, int> onNext, CancellationToken token) =>
        _bridge.Call<Task>(ForEachAsyncIndexImpl<T>, source, onNext, token);

    private static Task ForEachAsyncImpl<T>(
        IObservable<T> source,
        Action<T> onNext,
        CancellationToken token) =>
        source.ForEachAsync(onNext, token);

    private static Task ForEachAsyncIndexImpl<T>(
        IObservable<T> source,
        Action<T, int> onNext,
        CancellationToken token) =>
        source.ForEachAsync(onNext, token);
}

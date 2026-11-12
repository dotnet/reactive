// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information. 

using System.Reactive.Linq;

namespace Tests.System.Reactive.Shared.Rx;

public sealed partial class RxTarget
{
    Realized<Seq<T>> ISeqVisitor.Using<T, TResource>(UsingSeq<T, TResource> seq) =>
        _bridge.Run<Seq<T>>(UsingImpl<T, TResource>, seq.ResourceFactory, seq.ObservableFactory);

    Realized<Seq<T>> ISeqVisitor.UsingAsync<T, TResource>(UsingAsyncSeq<T, TResource> seq) =>
        _bridge.Run<Seq<T>>(
            UsingAsyncImpl<T, TResource>, seq.ResourceFactory, seq.ObservableFactory);

    // The factories' ValueTasks handed over as Tasks, as Create's are.
    [AdaptsLibraryCall]
    private static IObservable<T> UsingAsyncImpl<T, TResource>(
        Func<CancellationToken, ValueTask<TResource>> resourceFactory,
        Func<TResource, CancellationToken, ValueTask<IObservable<T>>> observableFactory)
        where TResource : IDisposable =>
        Observable.Using(
            token => resourceFactory(token).AsTask(),
            (resource, token) => observableFactory(resource, token).AsTask());

    private static IObservable<T> UsingImpl<T, TResource>(
        Func<TResource> resourceFactory,
        Func<TResource, IObservable<T>> observableFactory)
        where TResource : IDisposable =>
        Observable.Using(resourceFactory, observableFactory);
}

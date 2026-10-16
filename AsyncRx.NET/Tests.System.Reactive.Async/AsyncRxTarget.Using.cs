// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information. 

using System.Reactive.Linq;

using Tests.System.Reactive.Shared;

namespace Tests.System.Reactive.Async;

public sealed partial class AsyncRxTarget
{
    Realized<Seq<T>> ISeqVisitor.Using<T, TResource>(UsingSeq<T, TResource> seq) =>
        _bridge.Run<Seq<T>>(UsingImpl<T, TResource>, seq.ResourceFactory, seq.ObservableFactory);

    private static IAsyncObservable<T> UsingImpl<T, TResource>(
        Func<TResource> resourceFactory,
        Func<TResource, IAsyncObservable<T>> observableFactory)
        where TResource : IDisposable =>
        AsyncObservable.Using(resourceFactory, observableFactory);
}

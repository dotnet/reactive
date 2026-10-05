// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information. 

using System.Reactive.Linq;
using System.Reactive.Subjects;

using Tests.System.Reactive.Shared;

namespace Tests.System.Reactive.Async;

public sealed partial class AsyncRxTarget
{
    Realized<Seq<T>> ISeqVisitor.RefCount<T>(RefCountSeq<T> seq) =>
        _bridge.Run<Seq<T>>(RefCountImpl<T>, seq.Source);

    Realized<Seq<T>> ISeqVisitor.RefCountMinObservers<T>(RefCountMinObserversSeq<T> seq) =>
        _bridge.Run<Seq<T>>(RefCountMinObserversImpl<T>, seq.Source, seq.MinObservers);

    private static IAsyncObservable<T> RefCountImpl<T>(IConnectableAsyncObservable<T> source) =>
        source.RefCount();

    private static IAsyncObservable<T> RefCountMinObserversImpl<T>(
        IConnectableAsyncObservable<T> source,
        int minObservers) =>
        source.RefCount(minObservers);
}

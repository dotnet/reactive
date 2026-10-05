// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information. 

using System.Reactive.Linq;
using System.Reactive.Subjects;

namespace Tests.System.Reactive.Shared.Rx;

public sealed partial class RxTarget
{
    Realized<Seq<T>> ISeqVisitor.RefCount<T>(RefCountSeq<T> seq) =>
        _bridge.Run<Seq<T>>(RefCountImpl<T>, seq.Source);

    Realized<Seq<T>> ISeqVisitor.RefCountMinObservers<T>(RefCountMinObserversSeq<T> seq) =>
        _bridge.Run<Seq<T>>(RefCountMinObserversImpl<T>, seq.Source, seq.MinObservers);

    private static IObservable<T> RefCountImpl<T>(IConnectableObservable<T> source) => source.RefCount();

    private static IObservable<T> RefCountMinObserversImpl<T>(IConnectableObservable<T> source, int minObservers) =>
        source.RefCount(minObservers);
}

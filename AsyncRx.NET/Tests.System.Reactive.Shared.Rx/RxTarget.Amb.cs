// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information. 

using System.Reactive.Linq;

namespace Tests.System.Reactive.Shared.Rx;

public sealed partial class RxTarget
{
    Realized<Seq<T>> ISeqVisitor.Amb<T>(AmbSeq<T> seq) =>
        _bridge.Run<Seq<T>>(AmbImpl<T>, seq.First, seq.Second);

    Realized<Seq<T>> ISeqVisitor.AmbParams<T>(AmbParamsSeq<T> seq) =>
        _bridge.Run<Seq<T>>(AmbParamsImpl<T>, [seq.Sources]);

    Realized<Seq<T>> ISeqVisitor.AmbEnumerable<T>(AmbEnumerableSeq<T> seq) =>
        _bridge.Run<Seq<T>>(AmbEnumerableImpl<T>, seq.Sources);

    private static IObservable<T> AmbImpl<T>(IObservable<T> first, IObservable<T> second) =>
        first.Amb(second);

    private static IObservable<T> AmbParamsImpl<T>(IObservable<T>[] sources) =>
        Observable.Amb(sources);

    private static IObservable<T> AmbEnumerableImpl<T>(IEnumerable<IObservable<T>> sources) =>
        sources.Amb();
}

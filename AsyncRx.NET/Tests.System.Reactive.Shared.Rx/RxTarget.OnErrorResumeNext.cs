// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information. 

using System.Reactive.Linq;

namespace Tests.System.Reactive.Shared.Rx;

public sealed partial class RxTarget
{
    Realized<Seq<T>> ISeqVisitor.OnErrorResumeNext<T>(OnErrorResumeNextSeq<T> seq) =>
        _bridge.Run<Seq<T>>(OnErrorResumeNextImpl<T>, seq.First, seq.Second);

    Realized<Seq<T>> ISeqVisitor.OnErrorResumeNextParams<T>(OnErrorResumeNextParamsSeq<T> seq) =>
        _bridge.Run<Seq<T>>(OnErrorResumeNextParamsImpl<T>, [seq.Sources]);

    Realized<Seq<T>> ISeqVisitor.OnErrorResumeNextEnumerable<T>(OnErrorResumeNextEnumerableSeq<T> seq) =>
        _bridge.Run<Seq<T>>(OnErrorResumeNextEnumerableImpl<T>, seq.Sources);

    private static IObservable<T> OnErrorResumeNextImpl<T>(IObservable<T> first, IObservable<T> second) =>
        first.OnErrorResumeNext(second);

    private static IObservable<T> OnErrorResumeNextParamsImpl<T>(IObservable<T>[] sources) =>
        Observable.OnErrorResumeNext(sources);

    private static IObservable<T> OnErrorResumeNextEnumerableImpl<T>(IEnumerable<IObservable<T>> sources) =>
        sources.OnErrorResumeNext();
}

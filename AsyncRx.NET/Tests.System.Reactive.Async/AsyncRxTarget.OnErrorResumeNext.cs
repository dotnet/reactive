// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information. 

using System.Reactive.Linq;

using Tests.System.Reactive.Shared;

namespace Tests.System.Reactive.Async;

public sealed partial class AsyncRxTarget
{
    Realized<Seq<T>> ISeqVisitor.OnErrorResumeNext<T>(OnErrorResumeNextSeq<T> seq) =>
        _bridge.Run<Seq<T>>(OnErrorResumeNextImpl<T>, seq.First, seq.Second);

    Realized<Seq<T>> ISeqVisitor.OnErrorResumeNextParams<T>(OnErrorResumeNextParamsSeq<T> seq) =>
        _bridge.Run<Seq<T>>(OnErrorResumeNextParamsImpl<T>, [seq.Sources]);

    Realized<Seq<T>> ISeqVisitor.OnErrorResumeNextEnumerable<T>(OnErrorResumeNextEnumerableSeq<T> seq) =>
        _bridge.Run<Seq<T>>(OnErrorResumeNextEnumerableImpl<T>, seq.Sources);

    private static IAsyncObservable<T> OnErrorResumeNextImpl<T>(IAsyncObservable<T> first, IAsyncObservable<T> second) =>
        first.OnErrorResumeNext(second);

    private static IAsyncObservable<T> OnErrorResumeNextParamsImpl<T>(IAsyncObservable<T>[] sources) =>
        AsyncObservable.OnErrorResumeNext(sources);

    private static IAsyncObservable<T> OnErrorResumeNextEnumerableImpl<T>(IEnumerable<IAsyncObservable<T>> sources) =>
        sources.OnErrorResumeNext();
}

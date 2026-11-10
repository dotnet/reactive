// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information. 

using System.Reactive.Linq;

namespace Tests.System.Reactive.Shared.Rx;

public sealed partial class RxTarget
{
    Realized<Seq<T>> ISeqVisitor.SingleAsync<T>(SingleAsyncSeq<T> seq) =>
        _bridge.Run<Seq<T>>(SingleAsyncImpl<T>, seq.Source);

    Realized<Seq<T>> ISeqVisitor.SingleAsyncPredicate<T>(
        SingleAsyncPredicateSeq<T> seq) =>
        _bridge.Run<Seq<T>>(SingleAsyncPredicateImpl<T>, seq.Source, seq.Predicate);

    private static IObservable<T> SingleAsyncImpl<T>(IObservable<T> source) =>
        source.SingleAsync();

    private static IObservable<T> SingleAsyncPredicateImpl<T>(
        IObservable<T> source,
        Func<T, bool> predicate) =>
        source.SingleAsync(predicate);
}

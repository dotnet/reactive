// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information. 

using System.Reactive.Linq;

namespace Tests.System.Reactive.Shared.Rx;

public sealed partial class RxTarget
{
    Realized<Seq<T>> ISeqVisitor.FirstAsync<T>(FirstAsyncSeq<T> seq) =>
        _bridge.Run<Seq<T>>(FirstAsyncImpl<T>, seq.Source);

    Realized<Seq<T>> ISeqVisitor.FirstAsyncPredicate<T>(
        FirstAsyncPredicateSeq<T> seq) =>
        _bridge.Run<Seq<T>>(FirstAsyncPredicateImpl<T>, seq.Source, seq.Predicate);

    private static IObservable<T> FirstAsyncImpl<T>(IObservable<T> source) =>
        source.FirstAsync();

    private static IObservable<T> FirstAsyncPredicateImpl<T>(
        IObservable<T> source,
        Func<T, bool> predicate) =>
        source.FirstAsync(predicate);
}

// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information. 

using System.Reactive.Linq;

namespace Tests.System.Reactive.Shared.Rx;

public sealed partial class RxTarget
{
    Realized<Seq<long>> ISeqVisitor.LongCount<T>(LongCountSeq<T> seq) =>
        _bridge.Run<Seq<long>>(LongCountImpl<T>, seq.Source);

    Realized<Seq<long>> ISeqVisitor.LongCountPredicate<T>(LongCountPredicateSeq<T> seq) =>
        _bridge.Run<Seq<long>>(LongCountPredicateImpl<T>, seq.Source, seq.Predicate);

    private static IObservable<long> LongCountImpl<T>(IObservable<T> source) =>
        source.LongCount();

    private static IObservable<long> LongCountPredicateImpl<T>(
        IObservable<T> source,
        Func<T, bool> predicate) =>
        source.LongCount(predicate);
}

// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information. 

using System.Reactive.Linq;

namespace Tests.System.Reactive.Shared.Rx;

public sealed partial class RxTarget
{
    Realized<Seq<int>> ISeqVisitor.Count<T>(CountSeq<T> seq) =>
        _bridge.Run<Seq<int>>(CountImpl<T>, seq.Source);

    Realized<Seq<int>> ISeqVisitor.CountPredicate<T>(CountPredicateSeq<T> seq) =>
        _bridge.Run<Seq<int>>(CountPredicateImpl<T>, seq.Source, seq.Predicate);

    private static IObservable<int> CountImpl<T>(IObservable<T> source) =>
        source.Count();

    private static IObservable<int> CountPredicateImpl<T>(
        IObservable<T> source,
        Func<T, bool> predicate) =>
        source.Count(predicate);
}

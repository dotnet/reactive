// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information. 

using System.Reactive.Linq;

using Tests.System.Reactive.Shared;

namespace Tests.System.Reactive.Async;

public sealed partial class AsyncRxTarget
{
    Realized<Seq<int>> ISeqVisitor.Count<T>(CountSeq<T> seq) =>
        _bridge.Run<Seq<int>>(CountImpl<T>, seq.Source);

    Realized<Seq<int>> ISeqVisitor.CountPredicate<T>(CountPredicateSeq<T> seq) =>
        _bridge.Run<Seq<int>>(CountPredicateImpl<T>, seq.Source, seq.Predicate);

    private static IAsyncObservable<int> CountImpl<T>(IAsyncObservable<T> source) =>
        source.Count();

    private static IAsyncObservable<int> CountPredicateImpl<T>(
        IAsyncObservable<T> source,
        Func<T, bool> predicate) =>
        source.Count(predicate);
}

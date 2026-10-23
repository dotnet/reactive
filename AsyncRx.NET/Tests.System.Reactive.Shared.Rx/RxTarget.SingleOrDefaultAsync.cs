// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information. 

using System.Reactive.Linq;

namespace Tests.System.Reactive.Shared.Rx;

public sealed partial class RxTarget
{
    Realized<Seq<T>> ISeqVisitor.SingleOrDefaultAsync<T>(SingleOrDefaultAsyncSeq<T> seq) =>
        _bridge.Run<Seq<T>>(SingleOrDefaultAsyncImpl<T>, seq.Source);

    Realized<Seq<T>> ISeqVisitor.SingleOrDefaultAsyncPredicate<T>(
        SingleOrDefaultAsyncPredicateSeq<T> seq) =>
        _bridge.Run<Seq<T>>(SingleOrDefaultAsyncPredicateImpl<T>, seq.Source, seq.Predicate);

    private static IObservable<T?> SingleOrDefaultAsyncImpl<T>(IObservable<T> source) =>
        source.SingleOrDefaultAsync();

    private static IObservable<T?> SingleOrDefaultAsyncPredicateImpl<T>(
        IObservable<T> source,
        Func<T, bool> predicate) =>
        source.SingleOrDefaultAsync(predicate);
}

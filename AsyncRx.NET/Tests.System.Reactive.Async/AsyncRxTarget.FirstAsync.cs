// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information. 

using System.Reactive.Linq;

using Tests.System.Reactive.Shared;

namespace Tests.System.Reactive.Async;

public sealed partial class AsyncRxTarget
{
    Realized<Seq<T>> ISeqVisitor.FirstAsync<T>(FirstAsyncSeq<T> seq) =>
        _bridge.Run<Seq<T>>(FirstAsyncImpl<T>, seq.Source);

    Realized<Seq<T>> ISeqVisitor.FirstAsyncPredicate<T>(
        FirstAsyncPredicateSeq<T> seq) =>
        _bridge.Run<Seq<T>>(FirstAsyncPredicateImpl<T>, seq.Source, seq.Predicate);

    private static IAsyncObservable<T> FirstAsyncImpl<T>(IAsyncObservable<T> source) =>
        source.FirstAsync();

    private static IAsyncObservable<T> FirstAsyncPredicateImpl<T>(
        IAsyncObservable<T> source,
        Func<T, bool> predicate) =>
        source.FirstAsync(predicate);
}

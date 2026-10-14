// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information. 

using System.Reactive.Linq;

using Tests.System.Reactive.Shared;

namespace Tests.System.Reactive.Async;

public sealed partial class AsyncRxTarget
{
    Realized<Seq<T>> ISeqVisitor.LastAsync<T>(LastAsyncSeq<T> seq) =>
        _bridge.Run<Seq<T>>(LastAsyncImpl<T>, seq.Source);

    Realized<Seq<T>> ISeqVisitor.LastAsyncPredicate<T>(
        LastAsyncPredicateSeq<T> seq) =>
        _bridge.Run<Seq<T>>(LastAsyncPredicateImpl<T>, seq.Source, seq.Predicate);

    private static IAsyncObservable<T> LastAsyncImpl<T>(IAsyncObservable<T> source) =>
        source.LastAsync();

    private static IAsyncObservable<T> LastAsyncPredicateImpl<T>(
        IAsyncObservable<T> source,
        Func<T, bool> predicate) =>
        source.LastAsync(predicate);
}

// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information. 

using System.Reactive.Linq;

using Tests.System.Reactive.Shared;

namespace Tests.System.Reactive.Async;

public sealed partial class AsyncRxTarget
{
    Realized<Seq<T>> ISeqVisitor.LastOrDefaultAsync<T>(LastOrDefaultAsyncSeq<T> seq) =>
        _bridge.Run<Seq<T>>(LastOrDefaultAsyncImpl<T>, seq.Source);

    Realized<Seq<T>> ISeqVisitor.LastOrDefaultAsyncPredicate<T>(
        LastOrDefaultAsyncPredicateSeq<T> seq) =>
        _bridge.Run<Seq<T>>(LastOrDefaultAsyncPredicateImpl<T>, seq.Source, seq.Predicate);

    private static IAsyncObservable<T> LastOrDefaultAsyncImpl<T>(IAsyncObservable<T> source) =>
        source.LastOrDefaultAsync();

    private static IAsyncObservable<T> LastOrDefaultAsyncPredicateImpl<T>(
        IAsyncObservable<T> source,
        Func<T, bool> predicate) =>
        source.LastOrDefaultAsync(predicate);
}

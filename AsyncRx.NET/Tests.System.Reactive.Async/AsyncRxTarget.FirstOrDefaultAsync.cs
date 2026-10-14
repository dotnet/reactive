// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information. 

using System.Reactive.Linq;

using Tests.System.Reactive.Shared;

namespace Tests.System.Reactive.Async;

public sealed partial class AsyncRxTarget
{
    Realized<Seq<T>> ISeqVisitor.FirstOrDefaultAsync<T>(FirstOrDefaultAsyncSeq<T> seq) =>
        _bridge.Run<Seq<T>>(FirstOrDefaultAsyncImpl<T>, seq.Source);

    Realized<Seq<T>> ISeqVisitor.FirstOrDefaultAsyncPredicate<T>(
        FirstOrDefaultAsyncPredicateSeq<T> seq) =>
        _bridge.Run<Seq<T>>(FirstOrDefaultAsyncPredicateImpl<T>, seq.Source, seq.Predicate);

    private static IAsyncObservable<T> FirstOrDefaultAsyncImpl<T>(IAsyncObservable<T> source) =>
        source.FirstOrDefaultAsync();

    private static IAsyncObservable<T> FirstOrDefaultAsyncPredicateImpl<T>(
        IAsyncObservable<T> source,
        Func<T, bool> predicate) =>
        source.FirstOrDefaultAsync(predicate);
}

// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information. 

using System.Reactive.Linq;

using Tests.System.Reactive.Shared;

namespace Tests.System.Reactive.Async;

public sealed partial class AsyncRxTarget
{
    Realized<Seq<T>> ISeqVisitor.TakeWhile<T>(TakeWhileSeq<T> seq) =>
        RunByForm<Seq<T>>(TakeWhileImpl<T>, TakeWhileAsyncImpl<T>, seq.Source, seq.Predicate);

    Realized<Seq<T>> ISeqVisitor.TakeWhileIndexed<T>(TakeWhileIndexedSeq<T> seq) =>
        RunByForm<Seq<T>>(
            TakeWhileIndexedImpl<T>, TakeWhileIndexedAsyncImpl<T>, seq.Source, seq.Predicate);

    private static IAsyncObservable<T> TakeWhileImpl<T>(
        IAsyncObservable<T> source,
        Func<T, bool> predicate) =>
        source.TakeWhile(predicate);

    private static IAsyncObservable<T> TakeWhileAsyncImpl<T>(
        IAsyncObservable<T> source,
        Func<T, ValueTask<bool>> predicate) =>
        source.TakeWhile(predicate);

    private static IAsyncObservable<T> TakeWhileIndexedImpl<T>(
        IAsyncObservable<T> source,
        Func<T, int, bool> predicate) =>
        source.TakeWhile(predicate);

    private static IAsyncObservable<T> TakeWhileIndexedAsyncImpl<T>(
        IAsyncObservable<T> source,
        Func<T, int, ValueTask<bool>> predicate) =>
        source.TakeWhile(predicate);
}

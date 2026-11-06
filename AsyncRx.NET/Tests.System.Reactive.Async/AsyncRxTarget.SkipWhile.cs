// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information. 

using System.Reactive.Linq;

using Tests.System.Reactive.Shared;

namespace Tests.System.Reactive.Async;

public sealed partial class AsyncRxTarget
{
    Realized<Seq<T>> ISeqVisitor.SkipWhile<T>(SkipWhileSeq<T> seq) =>
        RunByForm<Seq<T>>(SkipWhileImpl<T>, SkipWhileAsyncImpl<T>, seq.Source, seq.Predicate);

    Realized<Seq<T>> ISeqVisitor.SkipWhileIndexed<T>(SkipWhileIndexedSeq<T> seq) =>
        RunByForm<Seq<T>>(
            SkipWhileIndexedImpl<T>, SkipWhileIndexedAsyncImpl<T>, seq.Source, seq.Predicate);

    private static IAsyncObservable<T> SkipWhileImpl<T>(
        IAsyncObservable<T> source,
        Func<T, bool> predicate) =>
        source.SkipWhile(predicate);

    private static IAsyncObservable<T> SkipWhileAsyncImpl<T>(
        IAsyncObservable<T> source,
        Func<T, ValueTask<bool>> predicate) =>
        source.SkipWhile(predicate);

    private static IAsyncObservable<T> SkipWhileIndexedImpl<T>(
        IAsyncObservable<T> source,
        Func<T, int, bool> predicate) =>
        source.SkipWhile(predicate);

    private static IAsyncObservable<T> SkipWhileIndexedAsyncImpl<T>(
        IAsyncObservable<T> source,
        Func<T, int, ValueTask<bool>> predicate) =>
        source.SkipWhile(predicate);
}

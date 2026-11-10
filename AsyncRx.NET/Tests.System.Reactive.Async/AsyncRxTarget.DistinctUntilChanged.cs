// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information. 

using System.Reactive.Linq;

using Tests.System.Reactive.Shared;

namespace Tests.System.Reactive.Async;

public sealed partial class AsyncRxTarget
{
    Realized<Seq<T>> ISeqVisitor.DistinctUntilChanged<T>(DistinctUntilChangedSeq<T> seq) =>
        _bridge.Run<Seq<T>>(DistinctUntilChangedImpl<T>, seq.Source);

    Realized<Seq<T>> ISeqVisitor.DistinctUntilChangedComparer<T>(
        DistinctUntilChangedComparerSeq<T> seq) =>
        _bridge.Run<Seq<T>>(DistinctUntilChangedComparerImpl<T>, seq.Source, seq.Comparer);

    Realized<Seq<T>> ISeqVisitor.DistinctUntilChangedKey<T, TKey>(
        DistinctUntilChangedKeySeq<T, TKey> seq) =>
        RunByForm<Seq<T>>(
            DistinctUntilChangedKeyImpl<T, TKey>, DistinctUntilChangedKeyAsyncImpl<T, TKey>,
            seq.Source, seq.KeySelector);

    Realized<Seq<T>> ISeqVisitor.DistinctUntilChangedKeyComparer<T, TKey>(
        DistinctUntilChangedKeyComparerSeq<T, TKey> seq) =>
        RunByForm<Seq<T>>(
            DistinctUntilChangedKeyComparerImpl<T, TKey>,
            DistinctUntilChangedKeyComparerAsyncImpl<T, TKey>, seq.Source, seq.KeySelector,
            seq.Comparer);

    private static IAsyncObservable<T> DistinctUntilChangedImpl<T>(IAsyncObservable<T> source) =>
        source.DistinctUntilChanged();

    private static IAsyncObservable<T> DistinctUntilChangedComparerImpl<T>(
        IAsyncObservable<T> source,
        IEqualityComparer<T> comparer) =>
        source.DistinctUntilChanged(comparer);

    private static IAsyncObservable<T> DistinctUntilChangedKeyImpl<T, TKey>(
        IAsyncObservable<T> source,
        Func<T, TKey> keySelector) =>
        source.DistinctUntilChanged(keySelector);

    private static IAsyncObservable<T> DistinctUntilChangedKeyAsyncImpl<T, TKey>(
        IAsyncObservable<T> source,
        Func<T, ValueTask<TKey>> keySelector) =>
        source.DistinctUntilChanged(keySelector);

    private static IAsyncObservable<T> DistinctUntilChangedKeyComparerImpl<T, TKey>(
        IAsyncObservable<T> source,
        Func<T, TKey> keySelector,
        IEqualityComparer<TKey> comparer) =>
        source.DistinctUntilChanged(keySelector, comparer);

    private static IAsyncObservable<T> DistinctUntilChangedKeyComparerAsyncImpl<T, TKey>(
        IAsyncObservable<T> source,
        Func<T, ValueTask<TKey>> keySelector,
        IEqualityComparer<TKey> comparer) =>
        source.DistinctUntilChanged(keySelector, comparer);
}

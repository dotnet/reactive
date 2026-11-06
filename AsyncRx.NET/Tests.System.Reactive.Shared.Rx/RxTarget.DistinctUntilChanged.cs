// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information. 

using System.Reactive.Linq;

namespace Tests.System.Reactive.Shared.Rx;

public sealed partial class RxTarget
{
    Realized<Seq<T>> ISeqVisitor.DistinctUntilChanged<T>(DistinctUntilChangedSeq<T> seq) =>
        _bridge.Run<Seq<T>>(DistinctUntilChangedImpl<T>, seq.Source);

    Realized<Seq<T>> ISeqVisitor.DistinctUntilChangedComparer<T>(
        DistinctUntilChangedComparerSeq<T> seq) =>
        _bridge.Run<Seq<T>>(DistinctUntilChangedComparerImpl<T>, seq.Source, seq.Comparer);

    Realized<Seq<T>> ISeqVisitor.DistinctUntilChangedKey<T, TKey>(
        DistinctUntilChangedKeySeq<T, TKey> seq) =>
        _bridge.Run<Seq<T>>(DistinctUntilChangedKeyImpl<T, TKey>, seq.Source, seq.KeySelector);

    Realized<Seq<T>> ISeqVisitor.DistinctUntilChangedKeyComparer<T, TKey>(
        DistinctUntilChangedKeyComparerSeq<T, TKey> seq) =>
        _bridge.Run<Seq<T>>(
            DistinctUntilChangedKeyComparerImpl<T, TKey>,
            seq.Source,
            seq.KeySelector,
            seq.Comparer);

    private static IObservable<T> DistinctUntilChangedImpl<T>(IObservable<T> source) =>
        source.DistinctUntilChanged();

    private static IObservable<T> DistinctUntilChangedComparerImpl<T>(
        IObservable<T> source,
        IEqualityComparer<T> comparer) =>
        source.DistinctUntilChanged(comparer);

    private static IObservable<T> DistinctUntilChangedKeyImpl<T, TKey>(
        IObservable<T> source,
        Func<T, TKey> keySelector) =>
        source.DistinctUntilChanged(keySelector);

    private static IObservable<T> DistinctUntilChangedKeyComparerImpl<T, TKey>(
        IObservable<T> source,
        Func<T, TKey> keySelector,
        IEqualityComparer<TKey> comparer) =>
        source.DistinctUntilChanged(keySelector, comparer);
}

// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information. 

using System.Reactive.Linq;

namespace Tests.System.Reactive.Shared.Rx;

public sealed partial class RxTarget
{
    Realized<Seq<T>> ISeqVisitor.Distinct<T>(DistinctSeq<T> seq) =>
        _bridge.Run<Seq<T>>(DistinctImpl<T>, seq.Source);

    Realized<Seq<T>> ISeqVisitor.DistinctComparer<T>(
        DistinctComparerSeq<T> seq) =>
        _bridge.Run<Seq<T>>(DistinctComparerImpl<T>, seq.Source, seq.Comparer);

    Realized<Seq<T>> ISeqVisitor.DistinctKey<T, TKey>(
        DistinctKeySeq<T, TKey> seq) =>
        _bridge.Run<Seq<T>>(DistinctKeyImpl<T, TKey>, seq.Source, seq.KeySelector);

    Realized<Seq<T>> ISeqVisitor.DistinctKeyComparer<T, TKey>(
        DistinctKeyComparerSeq<T, TKey> seq) =>
        _bridge.Run<Seq<T>>(
            DistinctKeyComparerImpl<T, TKey>,
            seq.Source,
            seq.KeySelector,
            seq.Comparer);

    private static IObservable<T> DistinctImpl<T>(IObservable<T> source) =>
        source.Distinct();

    private static IObservable<T> DistinctComparerImpl<T>(
        IObservable<T> source,
        IEqualityComparer<T> comparer) =>
        source.Distinct(comparer);

    private static IObservable<T> DistinctKeyImpl<T, TKey>(
        IObservable<T> source,
        Func<T, TKey> keySelector) =>
        source.Distinct(keySelector);

    private static IObservable<T> DistinctKeyComparerImpl<T, TKey>(
        IObservable<T> source,
        Func<T, TKey> keySelector,
        IEqualityComparer<TKey> comparer) =>
        source.Distinct(keySelector, comparer);
}

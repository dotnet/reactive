// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information. 

using System.Reactive.Linq;

using Tests.System.Reactive.Shared;

namespace Tests.System.Reactive.Async;

public sealed partial class AsyncRxTarget
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

    private static IAsyncObservable<T> DistinctImpl<T>(IAsyncObservable<T> source) =>
        source.Distinct();

    private static IAsyncObservable<T> DistinctComparerImpl<T>(
        IAsyncObservable<T> source,
        IEqualityComparer<T> comparer) =>
        source.Distinct(comparer);

    private static IAsyncObservable<T> DistinctKeyImpl<T, TKey>(
        IAsyncObservable<T> source,
        Func<T, TKey> keySelector) =>
        source.Distinct(keySelector);

    private static IAsyncObservable<T> DistinctKeyComparerImpl<T, TKey>(
        IAsyncObservable<T> source,
        Func<T, TKey> keySelector,
        IEqualityComparer<TKey> comparer) =>
        source.Distinct(keySelector, comparer);
}

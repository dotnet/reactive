// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information. 

using System.Reactive.Linq;

namespace Tests.System.Reactive.Shared.Rx;

public sealed partial class RxTarget
{
    Realized<Seq<Group<TKey, T>>> ISeqVisitor.GroupByKey<T, TKey>(GroupByKeySeq<T, TKey> seq) =>
        _bridge.Run<Seq<Group<TKey, T>>>(GroupByKeyImpl<T, TKey>, seq.Source, seq.KeySelector);

    Realized<Seq<Group<TKey, T>>> ISeqVisitor.GroupByKeyComparer<T, TKey>(GroupByKeyComparerSeq<T, TKey> seq) =>
        _bridge.Run<Seq<Group<TKey, T>>>(GroupByKeyComparerImpl<T, TKey>, seq.Source, seq.KeySelector, seq.Comparer);

    Realized<Seq<Group<TKey, TElement>>> ISeqVisitor.GroupByKeyElement<T, TKey, TElement>(GroupByKeyElementSeq<T, TKey, TElement> seq) =>
        _bridge.Run<Seq<Group<TKey, TElement>>>(GroupByKeyElementImpl<T, TKey, TElement>, seq.Source, seq.KeySelector, seq.ElementSelector);

    Realized<Seq<Group<TKey, TElement>>> ISeqVisitor.GroupByKeyElementComparer<T, TKey, TElement>(GroupByKeyElementComparerSeq<T, TKey, TElement> seq) =>
        _bridge.Run<Seq<Group<TKey, TElement>>>(GroupByKeyElementComparerImpl<T, TKey, TElement>, seq.Source, seq.KeySelector, seq.ElementSelector, seq.Comparer);

    Realized<Seq<Group<TKey, T>>> ISeqVisitor.GroupByKeyCapacity<T, TKey>(GroupByKeyCapacitySeq<T, TKey> seq) =>
        _bridge.Run<Seq<Group<TKey, T>>>(GroupByKeyCapacityImpl<T, TKey>, seq.Source, seq.KeySelector, seq.Capacity);

    Realized<Seq<Group<TKey, T>>> ISeqVisitor.GroupByKeyCapacityComparer<T, TKey>(GroupByKeyCapacityComparerSeq<T, TKey> seq) =>
        _bridge.Run<Seq<Group<TKey, T>>>(GroupByKeyCapacityComparerImpl<T, TKey>, seq.Source, seq.KeySelector, seq.Capacity, seq.Comparer);

    Realized<Seq<Group<TKey, TElement>>> ISeqVisitor.GroupByKeyElementCapacity<T, TKey, TElement>(GroupByKeyElementCapacitySeq<T, TKey, TElement> seq) =>
        _bridge.Run<Seq<Group<TKey, TElement>>>(GroupByKeyElementCapacityImpl<T, TKey, TElement>, seq.Source, seq.KeySelector, seq.ElementSelector, seq.Capacity);

    Realized<Seq<Group<TKey, TElement>>> ISeqVisitor.GroupByKeyElementCapacityComparer<T, TKey, TElement>(GroupByKeyElementCapacityComparerSeq<T, TKey, TElement> seq) =>
        _bridge.Run<Seq<Group<TKey, TElement>>>(GroupByKeyElementCapacityComparerImpl<T, TKey, TElement>, seq.Source, seq.KeySelector, seq.ElementSelector, seq.Capacity, seq.Comparer);

    private static IObservable<IGroupedObservable<TKey, T>> GroupByKeyImpl<T, TKey>(
        IObservable<T> source,
        Func<T, TKey> keySelector) =>
        source.GroupBy(keySelector);

    private static IObservable<IGroupedObservable<TKey, T>> GroupByKeyComparerImpl<T, TKey>(
        IObservable<T> source,
        Func<T, TKey> keySelector,
        IEqualityComparer<TKey> comparer) =>
        source.GroupBy(keySelector, comparer);

    private static IObservable<IGroupedObservable<TKey, TElement>> GroupByKeyElementImpl<T, TKey, TElement>(
        IObservable<T> source,
        Func<T, TKey> keySelector,
        Func<T, TElement> elementSelector) =>
        source.GroupBy(keySelector, elementSelector);

    private static IObservable<IGroupedObservable<TKey, TElement>> GroupByKeyElementComparerImpl<T, TKey, TElement>(
        IObservable<T> source,
        Func<T, TKey> keySelector,
        Func<T, TElement> elementSelector,
        IEqualityComparer<TKey> comparer) =>
        source.GroupBy(keySelector, elementSelector, comparer);

    private static IObservable<IGroupedObservable<TKey, T>> GroupByKeyCapacityImpl<T, TKey>(
        IObservable<T> source,
        Func<T, TKey> keySelector,
        int capacity) =>
        source.GroupBy(keySelector, capacity);

    private static IObservable<IGroupedObservable<TKey, T>> GroupByKeyCapacityComparerImpl<T, TKey>(
        IObservable<T> source,
        Func<T, TKey> keySelector,
        int capacity,
        IEqualityComparer<TKey> comparer) =>
        source.GroupBy(keySelector, capacity, comparer);

    private static IObservable<IGroupedObservable<TKey, TElement>> GroupByKeyElementCapacityImpl<T, TKey, TElement>(
        IObservable<T> source,
        Func<T, TKey> keySelector,
        Func<T, TElement> elementSelector,
        int capacity) =>
        source.GroupBy(keySelector, elementSelector, capacity);

    private static IObservable<IGroupedObservable<TKey, TElement>> GroupByKeyElementCapacityComparerImpl<T, TKey, TElement>(
        IObservable<T> source,
        Func<T, TKey> keySelector,
        Func<T, TElement> elementSelector,
        int capacity,
        IEqualityComparer<TKey> comparer) =>
        source.GroupBy(keySelector, elementSelector, capacity, comparer);
}

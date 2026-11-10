// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information. 

using System.Reactive.Linq;

namespace Tests.System.Reactive.Shared.Rx;

public sealed partial class RxTarget
{
    Realized<Seq<Group<TKey, T>>> ISeqVisitor.GroupByUntilKey<T, TKey, TDuration>(GroupByUntilKeySeq<T, TKey, TDuration> seq) =>
        _bridge.Run<Seq<Group<TKey, T>>>(
            GroupByUntilKeyImpl<T, TKey, TDuration>,
            seq.Source, seq.KeySelector, seq.DurationSelector);

    Realized<Seq<Group<TKey, T>>> ISeqVisitor.GroupByUntilKeyComparer<T, TKey, TDuration>(GroupByUntilKeyComparerSeq<T, TKey, TDuration> seq) =>
        _bridge.Run<Seq<Group<TKey, T>>>(
            GroupByUntilKeyComparerImpl<T, TKey, TDuration>,
            seq.Source, seq.KeySelector, seq.DurationSelector, seq.Comparer);

    Realized<Seq<Group<TKey, T>>> ISeqVisitor.GroupByUntilKeyCapacity<T, TKey, TDuration>(GroupByUntilKeyCapacitySeq<T, TKey, TDuration> seq) =>
        _bridge.Run<Seq<Group<TKey, T>>>(
            GroupByUntilKeyCapacityImpl<T, TKey, TDuration>,
            seq.Source, seq.KeySelector, seq.DurationSelector, seq.Capacity);

    Realized<Seq<Group<TKey, T>>> ISeqVisitor.GroupByUntilKeyCapacityComparer<T, TKey, TDuration>(GroupByUntilKeyCapacityComparerSeq<T, TKey, TDuration> seq) =>
        _bridge.Run<Seq<Group<TKey, T>>>(
            GroupByUntilKeyCapacityComparerImpl<T, TKey, TDuration>,
            seq.Source, seq.KeySelector, seq.DurationSelector, seq.Capacity, seq.Comparer);

    Realized<Seq<Group<TKey, TElement>>> ISeqVisitor.GroupByUntilKeyElement<T, TKey, TElement, TDuration>(GroupByUntilKeyElementSeq<T, TKey, TElement, TDuration> seq) =>
        _bridge.Run<Seq<Group<TKey, TElement>>>(
            GroupByUntilKeyElementImpl<T, TKey, TElement, TDuration>,
            seq.Source, seq.KeySelector, seq.ElementSelector, seq.DurationSelector);

    Realized<Seq<Group<TKey, TElement>>> ISeqVisitor.GroupByUntilKeyElementComparer<T, TKey, TElement, TDuration>(GroupByUntilKeyElementComparerSeq<T, TKey, TElement, TDuration> seq) =>
        _bridge.Run<Seq<Group<TKey, TElement>>>(
            GroupByUntilKeyElementComparerImpl<T, TKey, TElement, TDuration>,
            seq.Source, seq.KeySelector, seq.ElementSelector, seq.DurationSelector, seq.Comparer);

    Realized<Seq<Group<TKey, TElement>>> ISeqVisitor.GroupByUntilKeyElementCapacity<T, TKey, TElement, TDuration>(GroupByUntilKeyElementCapacitySeq<T, TKey, TElement, TDuration> seq) =>
        _bridge.Run<Seq<Group<TKey, TElement>>>(
            GroupByUntilKeyElementCapacityImpl<T, TKey, TElement, TDuration>,
            seq.Source, seq.KeySelector, seq.ElementSelector, seq.DurationSelector, seq.Capacity);

    Realized<Seq<Group<TKey, TElement>>> ISeqVisitor.GroupByUntilKeyElementCapacityComparer<T, TKey, TElement, TDuration>(GroupByUntilKeyElementCapacityComparerSeq<T, TKey, TElement, TDuration> seq) =>
        _bridge.Run<Seq<Group<TKey, TElement>>>(
            GroupByUntilKeyElementCapacityComparerImpl<T, TKey, TElement, TDuration>,
            seq.Source, seq.KeySelector, seq.ElementSelector, seq.DurationSelector, seq.Capacity, seq.Comparer);

    private static IObservable<IGroupedObservable<TKey, T>> GroupByUntilKeyImpl<T, TKey, TDuration>(
        IObservable<T> source,
        Func<T, TKey> keySelector,
        Func<IGroupedObservable<TKey, T>, IObservable<TDuration>> durationSelector) =>
        source.GroupByUntil(keySelector, durationSelector);

    private static IObservable<IGroupedObservable<TKey, T>> GroupByUntilKeyComparerImpl<T, TKey, TDuration>(
        IObservable<T> source,
        Func<T, TKey> keySelector,
        Func<IGroupedObservable<TKey, T>, IObservable<TDuration>> durationSelector,
        IEqualityComparer<TKey> comparer) =>
        source.GroupByUntil(keySelector, durationSelector, comparer);

    private static IObservable<IGroupedObservable<TKey, T>> GroupByUntilKeyCapacityImpl<T, TKey, TDuration>(
        IObservable<T> source,
        Func<T, TKey> keySelector,
        Func<IGroupedObservable<TKey, T>, IObservable<TDuration>> durationSelector,
        int capacity) =>
        source.GroupByUntil(keySelector, durationSelector, capacity);

    private static IObservable<IGroupedObservable<TKey, T>> GroupByUntilKeyCapacityComparerImpl<T, TKey, TDuration>(
        IObservable<T> source,
        Func<T, TKey> keySelector,
        Func<IGroupedObservable<TKey, T>, IObservable<TDuration>> durationSelector,
        int capacity,
        IEqualityComparer<TKey> comparer) =>
        source.GroupByUntil(keySelector, durationSelector, capacity, comparer);

    private static IObservable<IGroupedObservable<TKey, TElement>> GroupByUntilKeyElementImpl<T, TKey, TElement, TDuration>(
        IObservable<T> source,
        Func<T, TKey> keySelector,
        Func<T, TElement> elementSelector,
        Func<IGroupedObservable<TKey, TElement>, IObservable<TDuration>> durationSelector) =>
        source.GroupByUntil(keySelector, elementSelector, durationSelector);

    private static IObservable<IGroupedObservable<TKey, TElement>> GroupByUntilKeyElementComparerImpl<T, TKey, TElement, TDuration>(
        IObservable<T> source,
        Func<T, TKey> keySelector,
        Func<T, TElement> elementSelector,
        Func<IGroupedObservable<TKey, TElement>, IObservable<TDuration>> durationSelector,
        IEqualityComparer<TKey> comparer) =>
        source.GroupByUntil(keySelector, elementSelector, durationSelector, comparer);

    private static IObservable<IGroupedObservable<TKey, TElement>> GroupByUntilKeyElementCapacityImpl<T, TKey, TElement, TDuration>(
        IObservable<T> source,
        Func<T, TKey> keySelector,
        Func<T, TElement> elementSelector,
        Func<IGroupedObservable<TKey, TElement>, IObservable<TDuration>> durationSelector,
        int capacity) =>
        source.GroupByUntil(keySelector, elementSelector, durationSelector, capacity);

    private static IObservable<IGroupedObservable<TKey, TElement>> GroupByUntilKeyElementCapacityComparerImpl<T, TKey, TElement, TDuration>(
        IObservable<T> source,
        Func<T, TKey> keySelector,
        Func<T, TElement> elementSelector,
        Func<IGroupedObservable<TKey, TElement>, IObservable<TDuration>> durationSelector,
        int capacity,
        IEqualityComparer<TKey> comparer) =>
        source.GroupByUntil(keySelector, elementSelector, durationSelector, capacity, comparer);
}

// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information. 

namespace Tests.System.Reactive.Shared;

public partial interface ISeqVisitor
{
    /// <summary>
    /// Materializes a <see cref="GroupByUntilKeySeq{T, TKey, TDuration}"/> as the target's own
    /// <c>source.GroupByUntil(keySelector, durationSelector)</c>.
    /// </summary>
    /// <remarks>
    /// Built by the <c>GroupByUntil</c> overload of <see cref="GroupByUntilExtensions"/> with
    /// these parameters.
    /// </remarks>
    Realized<Seq<Group<TKey, T>>> GroupByUntilKey<T, TKey, TDuration>(GroupByUntilKeySeq<T, TKey, TDuration> seq);

    /// <summary>
    /// Materializes a <see cref="GroupByUntilKeyComparerSeq{T, TKey, TDuration}"/> as the target's own
    /// <c>source.GroupByUntil(keySelector, durationSelector, comparer)</c>.
    /// </summary>
    /// <remarks>
    /// Built by the <c>GroupByUntil</c> overload of <see cref="GroupByUntilExtensions"/> with
    /// these parameters.
    /// </remarks>
    Realized<Seq<Group<TKey, T>>> GroupByUntilKeyComparer<T, TKey, TDuration>(GroupByUntilKeyComparerSeq<T, TKey, TDuration> seq);

    /// <summary>
    /// Materializes a <see cref="GroupByUntilKeyCapacitySeq{T, TKey, TDuration}"/> as the target's own
    /// <c>source.GroupByUntil(keySelector, durationSelector, capacity)</c>.
    /// </summary>
    /// <remarks>
    /// Built by the <c>GroupByUntil</c> overload of <see cref="GroupByUntilExtensions"/> with
    /// these parameters.
    /// </remarks>
    Realized<Seq<Group<TKey, T>>> GroupByUntilKeyCapacity<T, TKey, TDuration>(GroupByUntilKeyCapacitySeq<T, TKey, TDuration> seq);

    /// <summary>
    /// Materializes a <see cref="GroupByUntilKeyCapacityComparerSeq{T, TKey, TDuration}"/> as the target's own
    /// <c>source.GroupByUntil(keySelector, durationSelector, capacity, comparer)</c>.
    /// </summary>
    /// <remarks>
    /// Built by the <c>GroupByUntil</c> overload of <see cref="GroupByUntilExtensions"/> with
    /// these parameters.
    /// </remarks>
    Realized<Seq<Group<TKey, T>>> GroupByUntilKeyCapacityComparer<T, TKey, TDuration>(GroupByUntilKeyCapacityComparerSeq<T, TKey, TDuration> seq);

    /// <summary>
    /// Materializes a <see cref="GroupByUntilKeyElementSeq{T, TKey, TElement, TDuration}"/> as the target's own
    /// <c>source.GroupByUntil(keySelector, elementSelector, durationSelector)</c>.
    /// </summary>
    /// <remarks>
    /// Built by the <c>GroupByUntil</c> overload of <see cref="GroupByUntilExtensions"/> with
    /// these parameters.
    /// </remarks>
    Realized<Seq<Group<TKey, TElement>>> GroupByUntilKeyElement<T, TKey, TElement, TDuration>(GroupByUntilKeyElementSeq<T, TKey, TElement, TDuration> seq);

    /// <summary>
    /// Materializes a <see cref="GroupByUntilKeyElementComparerSeq{T, TKey, TElement, TDuration}"/> as the target's own
    /// <c>source.GroupByUntil(keySelector, elementSelector, durationSelector, comparer)</c>.
    /// </summary>
    /// <remarks>
    /// Built by the <c>GroupByUntil</c> overload of <see cref="GroupByUntilExtensions"/> with
    /// these parameters.
    /// </remarks>
    Realized<Seq<Group<TKey, TElement>>> GroupByUntilKeyElementComparer<T, TKey, TElement, TDuration>(GroupByUntilKeyElementComparerSeq<T, TKey, TElement, TDuration> seq);

    /// <summary>
    /// Materializes a <see cref="GroupByUntilKeyElementCapacitySeq{T, TKey, TElement, TDuration}"/> as the target's own
    /// <c>source.GroupByUntil(keySelector, elementSelector, durationSelector, capacity)</c>.
    /// </summary>
    /// <remarks>
    /// Built by the <c>GroupByUntil</c> overload of <see cref="GroupByUntilExtensions"/> with
    /// these parameters.
    /// </remarks>
    Realized<Seq<Group<TKey, TElement>>> GroupByUntilKeyElementCapacity<T, TKey, TElement, TDuration>(GroupByUntilKeyElementCapacitySeq<T, TKey, TElement, TDuration> seq);

    /// <summary>
    /// Materializes a <see cref="GroupByUntilKeyElementCapacityComparerSeq{T, TKey, TElement, TDuration}"/> as the target's own
    /// <c>source.GroupByUntil(keySelector, elementSelector, durationSelector, capacity, comparer)</c>.
    /// </summary>
    /// <remarks>
    /// Built by the <c>GroupByUntil</c> overload of <see cref="GroupByUntilExtensions"/> with
    /// these parameters.
    /// </remarks>
    Realized<Seq<Group<TKey, TElement>>> GroupByUntilKeyElementCapacityComparer<T, TKey, TElement, TDuration>(GroupByUntilKeyElementCapacityComparerSeq<T, TKey, TElement, TDuration> seq);
}

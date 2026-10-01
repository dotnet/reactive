// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information. 

namespace Tests.System.Reactive.Shared;

public partial interface ISeqVisitor
{
    /// <summary>
    /// Materializes a <see cref="GroupByKeySeq{T, TKey}"/> as the target's own
    /// <c>source.GroupBy(keySelector)</c>.
    /// </summary>
    /// <remarks>
    /// Built by <see cref="GroupByExtensions.GroupBy{T, TKey}(Seq{T}, Func{T, TKey}, string)"/>.
    /// </remarks>
    Realized<Seq<Group<TKey, T>>> GroupByKey<T, TKey>(GroupByKeySeq<T, TKey> seq);

    /// <summary>
    /// Materializes a <see cref="GroupByKeyComparerSeq{T, TKey}"/> as the target's own
    /// <c>source.GroupBy(keySelector, comparer)</c>.
    /// </summary>
    /// <remarks>
    /// Built by
    /// <see cref="GroupByExtensions.GroupBy{T, TKey}(Seq{T}, Func{T, TKey}, IEqualityComparer{TKey}, string)"/>.
    /// </remarks>
    Realized<Seq<Group<TKey, T>>> GroupByKeyComparer<T, TKey>(GroupByKeyComparerSeq<T, TKey> seq);

    /// <summary>
    /// Materializes a <see cref="GroupByKeyElementSeq{T, TKey, TElement}"/> as the target's own
    /// <c>source.GroupBy(keySelector, elementSelector)</c>.
    /// </summary>
    /// <remarks>
    /// Built by
    /// <see cref="GroupByExtensions.GroupBy{T, TKey, TElement}(Seq{T}, Func{T, TKey}, Func{T, TElement}, string)"/>.
    /// </remarks>
    Realized<Seq<Group<TKey, TElement>>> GroupByKeyElement<T, TKey, TElement>(GroupByKeyElementSeq<T, TKey, TElement> seq);

    /// <summary>
    /// Materializes a <see cref="GroupByKeyElementComparerSeq{T, TKey, TElement}"/> as the target's
    /// own <c>source.GroupBy(keySelector, elementSelector, comparer)</c>.
    /// </summary>
    /// <remarks>
    /// Built by
    /// <see cref="GroupByExtensions.GroupBy{T, TKey, TElement}(Seq{T}, Func{T, TKey}, Func{T, TElement}, IEqualityComparer{TKey}, string)"/>.
    /// </remarks>
    Realized<Seq<Group<TKey, TElement>>> GroupByKeyElementComparer<T, TKey, TElement>(GroupByKeyElementComparerSeq<T, TKey, TElement> seq);

    /// <summary>
    /// Materializes a <see cref="GroupByKeyCapacitySeq{T, TKey}"/> as the target's own
    /// <c>source.GroupBy(keySelector, capacity)</c>.
    /// </summary>
    /// <remarks>
    /// Built by
    /// <see cref="GroupByExtensions.GroupBy{T, TKey}(Seq{T}, Func{T, TKey}, int, string)"/>.
    /// </remarks>
    Realized<Seq<Group<TKey, T>>> GroupByKeyCapacity<T, TKey>(GroupByKeyCapacitySeq<T, TKey> seq);

    /// <summary>
    /// Materializes a <see cref="GroupByKeyCapacityComparerSeq{T, TKey}"/> as the target's own
    /// <c>source.GroupBy(keySelector, capacity, comparer)</c>.
    /// </summary>
    /// <remarks>
    /// Built by
    /// <see cref="GroupByExtensions.GroupBy{T, TKey}(Seq{T}, Func{T, TKey}, int, IEqualityComparer{TKey}, string)"/>.
    /// </remarks>
    Realized<Seq<Group<TKey, T>>> GroupByKeyCapacityComparer<T, TKey>(GroupByKeyCapacityComparerSeq<T, TKey> seq);

    /// <summary>
    /// Materializes a <see cref="GroupByKeyElementCapacitySeq{T, TKey, TElement}"/> as the target's
    /// own <c>source.GroupBy(keySelector, elementSelector, capacity)</c>.
    /// </summary>
    /// <remarks>
    /// Built by
    /// <see cref="GroupByExtensions.GroupBy{T, TKey, TElement}(Seq{T}, Func{T, TKey}, Func{T, TElement}, int, string)"/>.
    /// </remarks>
    Realized<Seq<Group<TKey, TElement>>> GroupByKeyElementCapacity<T, TKey, TElement>(GroupByKeyElementCapacitySeq<T, TKey, TElement> seq);

    /// <summary>
    /// Materializes a <see cref="GroupByKeyElementCapacityComparerSeq{T, TKey, TElement}"/> as the
    /// target's own <c>source.GroupBy(keySelector, elementSelector, capacity, comparer)</c>.
    /// </summary>
    /// <remarks>
    /// Built by
    /// <see cref="GroupByExtensions.GroupBy{T, TKey, TElement}(Seq{T}, Func{T, TKey}, Func{T, TElement}, int, IEqualityComparer{TKey}, string)"/>.
    /// </remarks>
    Realized<Seq<Group<TKey, TElement>>> GroupByKeyElementCapacityComparer<T, TKey, TElement>(GroupByKeyElementCapacityComparerSeq<T, TKey, TElement> seq);
}

// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information. 

namespace Tests.System.Reactive.Shared;

public partial interface ISeqVisitor
{
    /// <summary>
    /// Materializes a <see cref="DistinctSeq{T}"/> as the target's own <c>Distinct()</c>.
    /// </summary>
    /// <remarks>Built by <see cref="DistinctExtensions.Distinct{T}(Seq{T})"/>.</remarks>
    Realized<Seq<T>> Distinct<T>(DistinctSeq<T> seq);

    /// <summary>
    /// Materializes a <see cref="DistinctComparerSeq{T}"/> as the target's own
    /// <c>Distinct(comparer)</c>.
    /// </summary>
    /// <remarks>
    /// Built by <see cref="DistinctExtensions.Distinct{T}(Seq{T}, IEqualityComparer{T})"/>.
    /// </remarks>
    Realized<Seq<T>> DistinctComparer<T>(DistinctComparerSeq<T> seq);

    /// <summary>
    /// Materializes a <see cref="DistinctKeySeq{T, TKey}"/> as the target's own
    /// <c>Distinct(keySelector)</c>.
    /// </summary>
    /// <remarks>
    /// Built by
    /// <see cref="DistinctExtensions.Distinct{T, TKey}(Seq{T}, Func{T, TKey}, string)"/>.
    /// </remarks>
    Realized<Seq<T>> DistinctKey<T, TKey>(DistinctKeySeq<T, TKey> seq);

    /// <summary>
    /// Materializes a <see cref="DistinctKeyComparerSeq{T, TKey}"/> as the target's own
    /// <c>Distinct(keySelector, comparer)</c>.
    /// </summary>
    /// <remarks>
    /// Built by
    /// <see cref="DistinctExtensions.Distinct{T, TKey}(Seq{T}, Func{T, TKey}, IEqualityComparer{TKey}, string)"/>.
    /// </remarks>
    Realized<Seq<T>> DistinctKeyComparer<T, TKey>(
        DistinctKeyComparerSeq<T, TKey> seq);
}

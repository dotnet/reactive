// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information. 

namespace Tests.System.Reactive.Shared;

public partial interface ISeqVisitor
{
    /// <summary>
    /// Materializes a <see cref="DistinctUntilChangedSeq{T}"/> as the target's own <c>DistinctUntilChanged()</c>.
    /// </summary>
    /// <remarks>Built by <see cref="DistinctUntilChangedExtensions.DistinctUntilChanged{T}(Seq{T})"/>.</remarks>
    Realized<Seq<T>> DistinctUntilChanged<T>(DistinctUntilChangedSeq<T> seq);

    /// <summary>
    /// Materializes a <see cref="DistinctUntilChangedComparerSeq{T}"/> as the target's own
    /// <c>DistinctUntilChanged(comparer)</c>.
    /// </summary>
    /// <remarks>
    /// Built by <see cref="DistinctUntilChangedExtensions.DistinctUntilChanged{T}(Seq{T}, IEqualityComparer{T})"/>.
    /// </remarks>
    Realized<Seq<T>> DistinctUntilChangedComparer<T>(DistinctUntilChangedComparerSeq<T> seq);

    /// <summary>
    /// Materializes a <see cref="DistinctUntilChangedKeySeq{T, TKey}"/> as the target's own
    /// <c>DistinctUntilChanged(keySelector)</c>.
    /// </summary>
    /// <remarks>
    /// Built by
    /// <see cref="DistinctUntilChangedExtensions.DistinctUntilChanged{T, TKey}(Seq{T}, Func{T, TKey}, string)"/>.
    /// </remarks>
    Realized<Seq<T>> DistinctUntilChangedKey<T, TKey>(DistinctUntilChangedKeySeq<T, TKey> seq);

    /// <summary>
    /// Materializes a <see cref="DistinctUntilChangedKeyComparerSeq{T, TKey}"/> as the target's own
    /// <c>DistinctUntilChanged(keySelector, comparer)</c>.
    /// </summary>
    /// <remarks>
    /// Built by
    /// <see cref="DistinctUntilChangedExtensions.DistinctUntilChanged{T, TKey}(Seq{T}, Func{T, TKey}, IEqualityComparer{TKey}, string)"/>.
    /// </remarks>
    Realized<Seq<T>> DistinctUntilChangedKeyComparer<T, TKey>(
        DistinctUntilChangedKeyComparerSeq<T, TKey> seq);
}

// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information. 

namespace Tests.System.Reactive.Shared;

public partial interface ISeqVisitor
{
    /// <summary>
    /// Materializes a <see cref="SequenceEqualSeq{T}"/> as the target's own
    /// <c>SequenceEqual(second)</c>.
    /// </summary>
    /// <remarks>
    /// Built by <see cref="SequenceEqualExtensions.SequenceEqual{T}(Seq{T}, Seq{T})"/>.
    /// </remarks>
    Realized<Seq<bool>> SequenceEqual<T>(SequenceEqualSeq<T> seq);

    /// <summary>
    /// Materializes a <see cref="SequenceEqualComparerSeq{T}"/> as the target's own
    /// <c>SequenceEqual(second, comparer)</c>.
    /// </summary>
    /// <remarks>
    /// Built by
    /// <see cref="SequenceEqualExtensions.SequenceEqual{T}(Seq{T}, Seq{T}, IEqualityComparer{T})"/>.
    /// </remarks>
    Realized<Seq<bool>> SequenceEqualComparer<T>(SequenceEqualComparerSeq<T> seq);

    /// <summary>
    /// Materializes a <see cref="SequenceEqualEnumerableSeq{T}"/> as the target's own
    /// <c>SequenceEqual(IEnumerable)</c>.
    /// </summary>
    /// <remarks>
    /// Built by <see cref="SequenceEqualExtensions.SequenceEqual{T}(Seq{T}, IEnumerable{T})"/>.
    /// </remarks>
    Realized<Seq<bool>> SequenceEqualEnumerable<T>(SequenceEqualEnumerableSeq<T> seq);

    /// <summary>
    /// Materializes a <see cref="SequenceEqualEnumerableComparerSeq{T}"/> as the target's own
    /// <c>SequenceEqual(IEnumerable, comparer)</c>.
    /// </summary>
    /// <remarks>
    /// Built by
    /// <see cref="SequenceEqualExtensions.SequenceEqual{T}(Seq{T}, IEnumerable{T}, IEqualityComparer{T})"/>.
    /// </remarks>
    Realized<Seq<bool>> SequenceEqualEnumerableComparer<T>(
        SequenceEqualEnumerableComparerSeq<T> seq);
}

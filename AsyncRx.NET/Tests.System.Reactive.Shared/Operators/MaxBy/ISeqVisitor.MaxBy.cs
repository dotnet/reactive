// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information. 

namespace Tests.System.Reactive.Shared;

public partial interface ISeqVisitor
{
    /// <summary>
    /// Materializes a <see cref="MaxBySeq{TSource, TKey}"/> as the target's own
    /// <c>MaxBy(keySelector)</c>.
    /// </summary>
    /// <remarks>
    /// Built by
    /// <see cref="Seq.MaxBy{TSource, TKey}(Seq{TSource}, Func{TSource, TKey}, string)"/>.
    /// </remarks>
    Realized<Seq<IList<TSource>>> MaxBy<TSource, TKey>(MaxBySeq<TSource, TKey> seq);

    /// <summary>
    /// Materializes a <see cref="MaxByComparerSeq{TSource, TKey}"/> as the target's own
    /// <c>MaxBy(keySelector, comparer)</c>.
    /// </summary>
    /// <remarks>
    /// Built by
    /// <see cref="Seq.MaxBy{TSource, TKey}(Seq{TSource}, Func{TSource, TKey}, IComparer{TKey}, string)"/>.
    /// </remarks>
    Realized<Seq<IList<TSource>>> MaxByComparer<TSource, TKey>(
        MaxByComparerSeq<TSource, TKey> seq);
}

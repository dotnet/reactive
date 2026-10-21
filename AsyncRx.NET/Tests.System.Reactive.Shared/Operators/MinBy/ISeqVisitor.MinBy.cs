// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information. 

namespace Tests.System.Reactive.Shared;

public partial interface ISeqVisitor
{
    /// <summary>
    /// Materializes a <see cref="MinBySeq{TSource, TKey}"/> as the target's own
    /// <c>MinBy(keySelector)</c>.
    /// </summary>
    /// <remarks>
    /// Built by
    /// <see cref="Seq.MinBy{TSource, TKey}(Seq{TSource}, Func{TSource, TKey}, string)"/>.
    /// </remarks>
    Realized<Seq<IList<TSource>>> MinBy<TSource, TKey>(MinBySeq<TSource, TKey> seq);

    /// <summary>
    /// Materializes a <see cref="MinByComparerSeq{TSource, TKey}"/> as the target's own
    /// <c>MinBy(keySelector, comparer)</c>.
    /// </summary>
    /// <remarks>
    /// Built by
    /// <see cref="Seq.MinBy{TSource, TKey}(Seq{TSource}, Func{TSource, TKey}, IComparer{TKey}, string)"/>.
    /// </remarks>
    Realized<Seq<IList<TSource>>> MinByComparer<TSource, TKey>(
        MinByComparerSeq<TSource, TKey> seq);
}

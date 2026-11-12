// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information. 

namespace Tests.System.Reactive.Shared;

public partial interface ISeqVisitor
{
    /// <summary>
    /// Materializes a <see cref="ToDictionarySeq{TSource, TKey}"/> as the target's own
    /// <c>ToDictionary(keySelector)</c>.
    /// </summary>
    /// <remarks>
    /// Built by
    /// <see cref="Seq.ToDictionary{TSource, TKey}(Seq{TSource}, Func{TSource, TKey}, string)"/>.
    /// </remarks>
    Realized<Seq<IDictionary<TKey, TSource>>> ToDictionary<TSource, TKey>(
        ToDictionarySeq<TSource, TKey> seq) where TKey : notnull;

    /// <summary>
    /// Materializes a <see cref="ToDictionaryComparerSeq{TSource, TKey}"/> as the target's own
    /// <c>ToDictionary(keySelector, comparer)</c>.
    /// </summary>
    /// <remarks>
    /// Built by
    /// <see cref="Seq.ToDictionary{TSource, TKey}(Seq{TSource}, Func{TSource, TKey}, IEqualityComparer{TKey}, string)"/>.
    /// </remarks>
    Realized<Seq<IDictionary<TKey, TSource>>> ToDictionaryComparer<TSource, TKey>(
        ToDictionaryComparerSeq<TSource, TKey> seq) where TKey : notnull;

    /// <summary>
    /// Materializes a <see cref="ToDictionaryElementSeq{TSource, TKey, TElement}"/> as the target's own
    /// <c>ToDictionary(keySelector, elementSelector)</c>.
    /// </summary>
    /// <remarks>
    /// Built by
    /// <see cref="Seq.ToDictionary{TSource, TKey, TElement}(Seq{TSource}, Func{TSource, TKey}, Func{TSource, TElement}, string, string)"/>.
    /// </remarks>
    Realized<Seq<IDictionary<TKey, TElement>>> ToDictionaryElement<TSource, TKey, TElement>(
        ToDictionaryElementSeq<TSource, TKey, TElement> seq) where TKey : notnull;

    /// <summary>
    /// Materializes a <see cref="ToDictionaryElementComparerSeq{TSource, TKey, TElement}"/> as the target's own
    /// <c>ToDictionary(keySelector, elementSelector, comparer)</c>.
    /// </summary>
    /// <remarks>
    /// Built by
    /// <see cref="Seq.ToDictionary{TSource, TKey, TElement}(Seq{TSource}, Func{TSource, TKey}, Func{TSource, TElement}, IEqualityComparer{TKey}, string, string)"/>.
    /// </remarks>
    Realized<Seq<IDictionary<TKey, TElement>>> ToDictionaryElementComparer<TSource, TKey, TElement>(
        ToDictionaryElementComparerSeq<TSource, TKey, TElement> seq) where TKey : notnull;
}

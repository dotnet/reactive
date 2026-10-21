// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information. 

namespace Tests.System.Reactive.Shared;

public partial interface ISeqVisitor
{
    /// <summary>
    /// Materializes a <see cref="ToLookupSeq{TSource, TKey}"/> as the target's own
    /// <c>ToLookup(keySelector)</c>.
    /// </summary>
    /// <remarks>
    /// Built by
    /// <see cref="Seq.ToLookup{TSource, TKey}(Seq{TSource}, Func{TSource, TKey}, string)"/>.
    /// </remarks>
    Realized<Seq<ILookup<TKey, TSource>>> ToLookup<TSource, TKey>(
        ToLookupSeq<TSource, TKey> seq);

    /// <summary>
    /// Materializes a <see cref="ToLookupComparerSeq{TSource, TKey}"/> as the target's own
    /// <c>ToLookup(keySelector, comparer)</c>.
    /// </summary>
    /// <remarks>
    /// Built by
    /// <see cref="Seq.ToLookup{TSource, TKey}(Seq{TSource}, Func{TSource, TKey}, IEqualityComparer{TKey}, string)"/>.
    /// </remarks>
    Realized<Seq<ILookup<TKey, TSource>>> ToLookupComparer<TSource, TKey>(
        ToLookupComparerSeq<TSource, TKey> seq);

    /// <summary>
    /// Materializes a <see cref="ToLookupElementSeq{TSource, TKey, TElement}"/> as the target's own
    /// <c>ToLookup(keySelector, elementSelector)</c>.
    /// </summary>
    /// <remarks>
    /// Built by
    /// <see cref="Seq.ToLookup{TSource, TKey, TElement}(Seq{TSource}, Func{TSource, TKey}, Func{TSource, TElement}, string, string)"/>.
    /// </remarks>
    Realized<Seq<ILookup<TKey, TElement>>> ToLookupElement<TSource, TKey, TElement>(
        ToLookupElementSeq<TSource, TKey, TElement> seq);

    /// <summary>
    /// Materializes a <see cref="ToLookupElementComparerSeq{TSource, TKey, TElement}"/> as the target's own
    /// <c>ToLookup(keySelector, elementSelector, comparer)</c>.
    /// </summary>
    /// <remarks>
    /// Built by
    /// <see cref="Seq.ToLookup{TSource, TKey, TElement}(Seq{TSource}, Func{TSource, TKey}, Func{TSource, TElement}, IEqualityComparer{TKey}, string, string)"/>.
    /// </remarks>
    Realized<Seq<ILookup<TKey, TElement>>> ToLookupElementComparer<TSource, TKey, TElement>(
        ToLookupElementComparerSeq<TSource, TKey, TElement> seq);
}

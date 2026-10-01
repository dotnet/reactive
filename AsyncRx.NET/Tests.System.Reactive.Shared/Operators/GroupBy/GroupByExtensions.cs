// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information. 

using System.Runtime.CompilerServices;

namespace Tests.System.Reactive.Shared;

/// <summary>The <c>GroupBy</c> overload with key and element selectors and a comparer.</summary>
public static class GroupByExtensions
{
    /// <summary>Describes <c>source.GroupBy(keySelector, elementSelector, comparer)</c>.</summary>
    /// <typeparam name="T">The type of the elements in the source sequence.</typeparam>
    /// <typeparam name="TKey">The type of the grouping key computed for each element.</typeparam>
    /// <typeparam name="TElement">The type of the elements within the groups.</typeparam>
    /// <param name="source">An observable sequence whose elements to group.</param>
    /// <param name="keySelector">A function to extract the key for each element.</param>
    /// <param name="elementSelector">
    /// A function to map each source element to an element in a group.
    /// </param>
    /// <param name="comparer">An equality comparer to compare keys with.</param>
    /// <param name="text">
    /// Supplied by the compiler (the source text of the key selector), for printing the query in
    /// diagnostics; do not pass it.
    /// </param>
    public static Seq<Group<TKey, TElement>> GroupBy<T, TKey, TElement>(
        this Seq<T> source,
        Func<T, TKey> keySelector,
        Func<T, TElement> elementSelector,
        IEqualityComparer<TKey> comparer,
        [CallerArgumentExpression(nameof(keySelector))] string text = "") =>
        new GroupBySeq<T, TKey, TElement>(source, keySelector, elementSelector, comparer, text);
}

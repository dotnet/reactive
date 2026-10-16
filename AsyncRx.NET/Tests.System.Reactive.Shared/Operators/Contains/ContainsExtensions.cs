// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information. 

namespace Tests.System.Reactive.Shared;

/// <summary>The <c>Contains</c> overloads, as extension methods on <see cref="Seq{T}"/>.</summary>
/// <remarks>
/// These exist so that a shared scenario writes <c>xs.Contains(42)</c> or
/// <c>xs.Contains(42, comparer)</c> exactly as the Rx.NET test it was migrated from does. Each
/// method builds one node of the query description (one node type per overload, in this
/// folder), which each target turns into its own <c>Contains</c> call through the matching
/// <see cref="ISeqVisitor"/> member in <c>ISeqVisitor.Contains.cs</c>.
/// </remarks>
public static class ContainsExtensions
{
    /// <summary>Describes <c>source.Contains(value)</c>.</summary>
    /// <typeparam name="T">The type of the elements in the source sequence.</typeparam>
    /// <param name="source">An observable sequence in which to locate a value.</param>
    /// <param name="value">The value to locate in the source sequence.</param>
    /// <remarks>
    /// Builds a <see cref="ContainsSeq{T}"/>, which each target materializes through
    /// <see cref="ISeqVisitor.Contains{T}(ContainsSeq{T})"/>.
    /// </remarks>
    public static Seq<bool> Contains<T>(this Seq<T> source, T value) =>
        new ContainsSeq<T>(source, value);

    /// <summary>Describes <c>source.Contains(value, comparer)</c>.</summary>
    /// <typeparam name="T">The type of the elements in the source sequence.</typeparam>
    /// <param name="source">An observable sequence in which to locate a value.</param>
    /// <param name="value">The value to locate in the source sequence.</param>
    /// <param name="comparer">An equality comparer to compare elements.</param>
    /// <remarks>
    /// Builds a <see cref="ContainsComparerSeq{T}"/>, which each target materializes through
    /// <see cref="ISeqVisitor.ContainsComparer{T}(ContainsComparerSeq{T})"/>.
    /// </remarks>
    public static Seq<bool> Contains<T>(
        this Seq<T> source,
        T value,
        IEqualityComparer<T> comparer) =>
        new ContainsComparerSeq<T>(source, value, comparer);
}

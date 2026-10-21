// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information. 

namespace Tests.System.Reactive.Shared;

/// <summary>The <c>SequenceEqual</c> overloads.</summary>
/// <remarks>
/// Rx.NET's four: the second operand a sequence or an enumerable, with or without a comparer.
/// Each builds one node of the query description, which each target turns into its own
/// <c>SequenceEqual</c> call through the matching <see cref="ISeqVisitor"/> member in
/// <c>ISeqVisitor.SequenceEqual.cs</c>.
/// </remarks>
public static class SequenceEqualExtensions
{
    /// <summary>
    /// Describes <c>first.SequenceEqual(second)</c>: whether the two sequences produce the
    /// same elements in the same order.
    /// </summary>
    /// <typeparam name="T">The type of the elements in the sequences.</typeparam>
    /// <param name="first">First observable sequence to compare.</param>
    /// <param name="second">Second observable sequence to compare.</param>
    /// <remarks>
    /// Builds a <see cref="SequenceEqualSeq{T}"/>, which each target materializes through
    /// <see cref="ISeqVisitor.SequenceEqual{T}(SequenceEqualSeq{T})"/>.
    /// </remarks>
    public static Seq<bool> SequenceEqual<T>(this Seq<T> first, Seq<T> second) =>
        new SequenceEqualSeq<T>(first, second);

    /// <summary>
    /// Describes <c>first.SequenceEqual(second, comparer)</c>: whether the two sequences
    /// produce equal elements, by the comparer, in the same order.
    /// </summary>
    /// <typeparam name="T">The type of the elements in the sequences.</typeparam>
    /// <param name="first">First observable sequence to compare.</param>
    /// <param name="second">Second observable sequence to compare.</param>
    /// <param name="comparer">Comparer used to compare elements of both sequences.</param>
    /// <remarks>
    /// Builds a <see cref="SequenceEqualComparerSeq{T}"/>, which each target materializes
    /// through <see cref="ISeqVisitor.SequenceEqualComparer{T}(SequenceEqualComparerSeq{T})"/>.
    /// </remarks>
    public static Seq<bool> SequenceEqual<T>(
        this Seq<T> first,
        Seq<T> second,
        IEqualityComparer<T> comparer) =>
        new SequenceEqualComparerSeq<T>(first, second, comparer);

    /// <summary>
    /// Describes <c>first.SequenceEqual(second)</c> against an enumerable: whether the
    /// sequence produces the enumerable's elements in its order.
    /// </summary>
    /// <typeparam name="T">The type of the elements in the sequences.</typeparam>
    /// <param name="first">First observable sequence to compare.</param>
    /// <param name="second">Second enumerable sequence to compare.</param>
    /// <remarks>
    /// Builds a <see cref="SequenceEqualEnumerableSeq{T}"/>, which each target materializes
    /// through
    /// <see cref="ISeqVisitor.SequenceEqualEnumerable{T}(SequenceEqualEnumerableSeq{T})"/>.
    /// </remarks>
    public static Seq<bool> SequenceEqual<T>(this Seq<T> first, IEnumerable<T> second) =>
        new SequenceEqualEnumerableSeq<T>(first, second);

    /// <summary>
    /// Describes <c>first.SequenceEqual(second, comparer)</c> against an enumerable: whether
    /// the sequence produces elements equal, by the comparer, to the enumerable's in its order.
    /// </summary>
    /// <typeparam name="T">The type of the elements in the sequences.</typeparam>
    /// <param name="first">First observable sequence to compare.</param>
    /// <param name="second">Second enumerable sequence to compare.</param>
    /// <param name="comparer">Comparer used to compare elements of both sequences.</param>
    /// <remarks>
    /// Builds a <see cref="SequenceEqualEnumerableComparerSeq{T}"/>, which each target
    /// materializes through
    /// <see cref="ISeqVisitor.SequenceEqualEnumerableComparer{T}(SequenceEqualEnumerableComparerSeq{T})"/>.
    /// </remarks>
    public static Seq<bool> SequenceEqual<T>(
        this Seq<T> first,
        IEnumerable<T> second,
        IEqualityComparer<T> comparer) =>
        new SequenceEqualEnumerableComparerSeq<T>(first, second, comparer);
}

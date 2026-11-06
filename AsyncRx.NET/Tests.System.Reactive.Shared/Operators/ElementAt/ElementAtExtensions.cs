// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information. 

namespace Tests.System.Reactive.Shared;

/// <summary>The <c>ElementAt</c> overload, as an extension method on <see cref="Seq{T}"/>.</summary>
/// <remarks>
/// This exists so that a shared scenario writes <c>xs.ElementAt(n)</c> exactly as the
/// Rx.NET test it was migrated from does. It builds one node of the query description, which
/// each target turns into its own <c>ElementAt</c> call through the matching
/// <see cref="ISeqVisitor"/> member in <c>ISeqVisitor.ElementAt.cs</c>.
/// </remarks>
public static class ElementAtExtensions
{
    /// <summary>Describes <c>source.ElementAt(index)</c>.</summary>
    /// <typeparam name="T">The type of the elements in the source sequence.</typeparam>
    /// <param name="source">Observable sequence to return the element from.</param>
    /// <param name="index">The zero-based index of the element to retrieve.</param>
    /// <remarks>
    /// Builds an <see cref="ElementAtSeq{T}"/>, which each target materializes
    /// through <see cref="ISeqVisitor.ElementAt{T}(ElementAtSeq{T})"/>.
    /// </remarks>
    public static Seq<T> ElementAt<T>(this Seq<T> source, int index) =>
        new ElementAtSeq<T>(source, index);
}

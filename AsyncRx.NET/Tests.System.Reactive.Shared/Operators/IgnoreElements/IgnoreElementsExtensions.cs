// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information. 

namespace Tests.System.Reactive.Shared;

/// <summary>The <c>IgnoreElements</c> operator: a sequence's termination without its elements.</summary>
public static class IgnoreElementsExtensions
{
    /// <summary>Describes <c>source.IgnoreElements()</c>.</summary>
    /// <typeparam name="T">The type of the elements in the source sequence.</typeparam>
    /// <param name="source">Source sequence.</param>
    /// <remarks>
    /// Builds an <see cref="IgnoreElementsSeq{T}"/>, which each target materializes through
    /// <see cref="ISeqVisitor.IgnoreElements{T}(IgnoreElementsSeq{T})"/>.
    /// </remarks>
    public static Seq<T> IgnoreElements<T>(this Seq<T> source) => new IgnoreElementsSeq<T>(source);
}

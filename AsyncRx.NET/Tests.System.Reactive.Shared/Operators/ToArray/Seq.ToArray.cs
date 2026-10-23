// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information. 

namespace Tests.System.Reactive.Shared;

/// <summary>The <c>ToArray</c> operator, as an extension method on <see cref="Seq"/>.</summary>
/// <remarks>
/// Where an Rx.NET test writes <c>xs.ToArray()</c>, a shared scenario writes the same over a
/// <see cref="Seq{T}"/>. The one node, in this folder, is materialized by a target through
/// <see cref="ISeqVisitor.ToArray{T}(ToArraySeq{T})"/>. The result is a sequence of one
/// array holding every element of the source, as Rx.NET's <c>ToArray</c> yields it.
/// </remarks>
public static partial class Seq
{
    /// <summary>Describes <c>source.ToArray()</c>.</summary>
    /// <typeparam name="T">The type of the elements in the source sequence.</typeparam>
    /// <param name="source">The source observable sequence to get an array of elements for.</param>
    /// <remarks>
    /// Builds a <see cref="ToArraySeq{T}"/>, which each target materializes through
    /// <see cref="ISeqVisitor.ToArray{T}(ToArraySeq{T})"/>.
    /// </remarks>
    public static Seq<T[]> ToArray<T>(this Seq<T> source) => new ToArraySeq<T>(source);
}

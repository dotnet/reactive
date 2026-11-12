// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information. 

namespace Tests.System.Reactive.Shared;

/// <summary>The <c>ToList</c> operator, as an extension method on <see cref="Seq"/>.</summary>
/// <remarks>
/// Where an Rx.NET test writes <c>xs.ToList()</c>, a shared scenario writes the same over a
/// <see cref="Seq{T}"/>. The one node, in this folder, is materialized by a target through
/// <see cref="ISeqVisitor.ToList{T}(ToListSeq{T})"/>. The result is a sequence of one
/// list holding every element of the source, as Rx.NET's <c>ToList</c> yields it.
/// </remarks>
public static partial class Seq
{
    /// <summary>Describes <c>source.ToList()</c>.</summary>
    /// <typeparam name="T">The type of the elements in the source sequence.</typeparam>
    /// <param name="source">The source observable sequence to get a list of elements for.</param>
    /// <remarks>
    /// Builds a <see cref="ToListSeq{T}"/>, which each target materializes through
    /// <see cref="ISeqVisitor.ToList{T}(ToListSeq{T})"/>.
    /// </remarks>
    public static Seq<IList<T>> ToList<T>(this Seq<T> source) => new ToListSeq<T>(source);
}

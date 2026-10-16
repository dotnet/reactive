// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information. 

namespace Tests.System.Reactive.Shared;

/// <summary>The <c>StartWith</c> overloads, as extension methods on <see cref="Seq{T}"/>.</summary>
/// <remarks>
/// Only the <c>params</c> form so far: the <c>Catch</c> scenarios compose with it. The other forms
/// join when <c>StartWith</c> comes under test. Each method builds one node of the query
/// description, which each target turns into its own <c>StartWith</c> call through the matching
/// <see cref="ISeqVisitor"/> member in <c>ISeqVisitor.StartWith.cs</c>.
/// </remarks>
public static class StartWithExtensions
{
    /// <summary>Describes <c>source.StartWith(values)</c>.</summary>
    /// <typeparam name="T">The type of the elements in the source sequence.</typeparam>
    /// <param name="source">Source sequence to prepend values to.</param>
    /// <param name="values">Values to prepend to the specified sequence.</param>
    /// <remarks>
    /// Builds a <see cref="StartWithSeq{T}"/>, which each target materializes through
    /// <see cref="ISeqVisitor.StartWith{T}(StartWithSeq{T})"/>.
    /// </remarks>
    public static Seq<T> StartWith<T>(this Seq<T> source, params T[] values) =>
        new StartWithSeq<T>(source, values);
}

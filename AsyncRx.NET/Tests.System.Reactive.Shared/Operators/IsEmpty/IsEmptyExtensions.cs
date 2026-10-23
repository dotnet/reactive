// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information. 

namespace Tests.System.Reactive.Shared;

/// <summary>The <c>IsEmpty</c> overload, as an extension method on <see cref="Seq{T}"/>.</summary>
/// <remarks>
/// This exists so that a shared scenario writes <c>xs.IsEmpty()</c> exactly as the Rx.NET test
/// it was migrated from does. It builds one node of the query description, which each target
/// turns into its own <c>IsEmpty</c> call through the matching <see cref="ISeqVisitor"/>
/// member in <c>ISeqVisitor.IsEmpty.cs</c>.
/// </remarks>
public static class IsEmptyExtensions
{
    /// <summary>Describes <c>source.IsEmpty()</c>.</summary>
    /// <typeparam name="T">The type of the elements in the source sequence.</typeparam>
    /// <param name="source">An observable sequence to check for emptiness.</param>
    /// <remarks>
    /// Builds an <see cref="IsEmptySeq{T}"/>, which each target materializes through
    /// <see cref="ISeqVisitor.IsEmpty{T}(IsEmptySeq{T})"/>.
    /// </remarks>
    public static Seq<bool> IsEmpty<T>(this Seq<T> source) => new IsEmptySeq<T>(source);
}

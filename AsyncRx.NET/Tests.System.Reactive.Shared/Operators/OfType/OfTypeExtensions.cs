// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information. 

namespace Tests.System.Reactive.Shared;

/// <summary>The <c>OfType</c> overload, as an extension method on <see cref="Seq{T}"/>.</summary>
/// <remarks>
/// This exists so that a shared scenario writes <c>xs.OfType&lt;B&gt;()</c> exactly as the
/// Rx.NET test it was migrated from does, over a <c>Seq&lt;object?&gt;</c> as Rx.NET's is over an
/// <c>IObservable&lt;object?&gt;</c>. It builds one node of the query description, which each
/// target turns into its own <c>OfType</c> call through the matching <see cref="ISeqVisitor"/>
/// member in <c>ISeqVisitor.OfType.cs</c>.
/// </remarks>
public static class OfTypeExtensions
{
    /// <summary>Describes <c>source.OfType&lt;TResult&gt;()</c>.</summary>
    /// <typeparam name="TResult">
    /// The type to filter the elements in the source sequence on.
    /// </typeparam>
    /// <param name="source">
    /// The observable sequence that contains the elements to be filtered.
    /// </param>
    /// <remarks>
    /// Builds an <see cref="OfTypeSeq{TResult}"/>, which each target materializes through
    /// <see cref="ISeqVisitor.OfType{TResult}(OfTypeSeq{TResult})"/>.
    /// </remarks>
    public static Seq<TResult> OfType<TResult>(this Seq<object?> source) =>
        new OfTypeSeq<TResult>(source);
}

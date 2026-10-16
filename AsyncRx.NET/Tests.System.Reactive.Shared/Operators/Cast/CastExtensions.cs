// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information. 

namespace Tests.System.Reactive.Shared;

/// <summary>The <c>Cast</c> overload, as an extension method on <see cref="Seq{T}"/>.</summary>
/// <remarks>
/// This exists so that a shared scenario writes <c>xs.Cast&lt;B&gt;()</c> exactly as the Rx.NET
/// test it was migrated from does, over a <c>Seq&lt;object&gt;</c> as Rx.NET's is over an
/// <c>IObservable&lt;object&gt;</c>. It builds one node of the query description, which each
/// target turns into its own <c>Cast</c> call through the matching <see cref="ISeqVisitor"/>
/// member in <c>ISeqVisitor.Cast.cs</c>.
/// </remarks>
public static class CastExtensions
{
    /// <summary>Describes <c>source.Cast&lt;TResult&gt;()</c>.</summary>
    /// <typeparam name="TResult">The target type to convert the elements to.</typeparam>
    /// <param name="source">
    /// The observable sequence that contains the elements to be converted.
    /// </param>
    /// <remarks>
    /// Builds a <see cref="CastSeq{TResult}"/>, which each target materializes through
    /// <see cref="ISeqVisitor.Cast{TResult}(CastSeq{TResult})"/>.
    /// </remarks>
    public static Seq<TResult> Cast<TResult>(this Seq<object> source) =>
        new CastSeq<TResult>(source);
}

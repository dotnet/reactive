// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information. 

using System.Runtime.CompilerServices;

namespace Tests.System.Reactive.Shared;

/// <summary>The <c>Finally</c> overload, as an extension method on <see cref="Seq{T}"/>.</summary>
/// <remarks>
/// This exists so that a shared scenario writes <c>xs.Finally(() =&gt; ...)</c> exactly as the
/// Rx.NET test it was migrated from does. It builds one node of the query description, which
/// each target turns into its own <c>Finally</c> call through the matching
/// <see cref="ISeqVisitor"/> member in <c>ISeqVisitor.Finally.cs</c>.
/// </remarks>
public static class FinallyExtensions
{
    /// <summary>Describes <c>source.Finally(finallyAction)</c>.</summary>
    /// <typeparam name="T">The type of the elements in the source sequence.</typeparam>
    /// <param name="source">Source sequence.</param>
    /// <param name="finallyAction">
    /// Action to invoke after the source observable sequence terminates.
    /// </param>
    /// <param name="text">
    /// Supplied by the compiler (the source text of the action), for printing the query in
    /// diagnostics; do not pass it.
    /// </param>
    /// <remarks>
    /// Builds a <see cref="FinallySeq{T}"/>, which each target materializes through
    /// <see cref="ISeqVisitor.Finally{T}(FinallySeq{T})"/>.
    /// </remarks>
    public static Seq<T> Finally<T>(
        this Seq<T> source,
        Action finallyAction,
        [CallerArgumentExpression(nameof(finallyAction))] string text = "") =>
        new FinallySeq<T>(source, finallyAction, text);
}

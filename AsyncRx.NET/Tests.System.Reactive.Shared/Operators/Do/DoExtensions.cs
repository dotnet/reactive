// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information. 

using System.Runtime.CompilerServices;

namespace Tests.System.Reactive.Shared;

/// <summary>The <c>Do</c> overloads, as extension methods on <see cref="Seq{T}"/>.</summary>
/// <remarks>
/// Only the <c>onNext</c> form so far: the <c>TakeUntil</c> scenarios compose with it to show
/// that a preempted source is heard from no more. The other forms join when <c>Do</c> comes
/// under test. Each method builds one node of the query description, which each target turns
/// into its own <c>Do</c> call through the matching <see cref="ISeqVisitor"/> member in
/// <c>ISeqVisitor.Do.cs</c>.
/// </remarks>
public static class DoExtensions
{
    /// <summary>Describes <c>source.Do(onNext)</c>.</summary>
    /// <typeparam name="T">The type of the elements in the source sequence.</typeparam>
    /// <param name="source">Source sequence.</param>
    /// <param name="onNext">Action to invoke for each element in the observable sequence.</param>
    /// <param name="text">
    /// Supplied by the compiler (the source text of the action), for printing the query in
    /// diagnostics; do not pass it.
    /// </param>
    /// <remarks>
    /// Builds a <see cref="DoSeq{T}"/>, which each target materializes through
    /// <see cref="ISeqVisitor.Do{T}(DoSeq{T})"/>.
    /// </remarks>
    public static Seq<T> Do<T>(
        this Seq<T> source,
        Action<T> onNext,
        [CallerArgumentExpression(nameof(onNext))] string text = "") =>
        new DoSeq<T>(source, onNext, text);
}

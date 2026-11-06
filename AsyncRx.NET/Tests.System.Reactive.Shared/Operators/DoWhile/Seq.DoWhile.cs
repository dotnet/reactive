// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information. 

using System.Runtime.CompilerServices;

namespace Tests.System.Reactive.Shared;

public static partial class Seq
{
    /// <summary>Describes <c>Seq.DoWhile(source, condition)</c>.</summary>
    /// <typeparam name="T">The type of the elements in the source sequence.</typeparam>
    /// <param name="source">
    /// Source to repeat as long as the condition function evaluates to true.
    /// </param>
    /// <param name="condition">
    /// Condition evaluated after each repetition of the source sequence.
    /// </param>
    /// <param name="text">
    /// Supplied by the compiler (the source text of <paramref name="condition"/>), for printing
    /// the query in diagnostics; do not pass it.
    /// </param>
    /// <remarks>
    /// An extension method, as Rx.NET's is, so a scenario may write it either way. Builds a
    /// <see cref="DoWhileSeq{T}"/>, which each target materializes through
    /// <see cref="ISeqVisitor.DoWhile{T}(DoWhileSeq{T})"/>.
    /// </remarks>
    public static Seq<T> DoWhile<T>(
        this Seq<T> source,
        Func<bool> condition,
        [CallerArgumentExpression(nameof(condition))] string text = "") =>
        new DoWhileSeq<T>(source, condition, text);
}

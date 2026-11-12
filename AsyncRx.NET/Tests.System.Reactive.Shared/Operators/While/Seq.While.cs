// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information. 

using System.Runtime.CompilerServices;

namespace Tests.System.Reactive.Shared;

public static partial class Seq
{
    /// <summary>Describes <c>Seq.While(condition, source)</c>.</summary>
    /// <typeparam name="T">The type of the elements in the source sequence.</typeparam>
    /// <param name="condition">
    /// Condition evaluated upon subscription to the resulting sequence and after each repetition
    /// of the source sequence.
    /// </param>
    /// <param name="source">
    /// Source to repeat as long as the condition function evaluates to true.
    /// </param>
    /// <param name="text">
    /// Supplied by the compiler (the source text of <paramref name="condition"/>), for printing
    /// the query in diagnostics; do not pass it.
    /// </param>
    /// <remarks>
    /// Builds a <see cref="WhileSeq{T}"/>, which each target materializes through
    /// <see cref="ISeqVisitor.While{T}(WhileSeq{T})"/>.
    /// </remarks>
    public static Seq<T> While<T>(
        Func<bool> condition,
        Seq<T> source,
        [CallerArgumentExpression(nameof(condition))] string text = "") =>
        new WhileSeq<T>(condition, source, text);
}

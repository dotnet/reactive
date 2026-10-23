// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information. 

using System.Runtime.CompilerServices;

namespace Tests.System.Reactive.Shared;

/// <summary>The <c>RepeatWhen</c> overload.</summary>
public static class RepeatWhenExtensions
{
    /// <summary>
    /// Describes <c>source.RepeatWhen(handler)</c>: repeats <paramref name="source"/> for each
    /// element of the sequence the handler returns for its completion signals.
    /// </summary>
    /// <typeparam name="T">The type of the elements in the source sequence.</typeparam>
    /// <typeparam name="TSignal">The type of the elements of the handler's sequence.</typeparam>
    /// <param name="source">The sequence to repeat.</param>
    /// <param name="handler">
    /// The handler: receives the sequence of completion signals and returns the sequence whose
    /// elements repeat the source, and whose completion or error ends the result.
    /// </param>
    /// <param name="text">
    /// Supplied by the compiler (the source text of the handler), for printing the query in
    /// diagnostics; do not pass it.
    /// </param>
    /// <remarks>
    /// Builds a <see cref="RepeatWhenSeq{T, TSignal}"/>, which each target materializes through
    /// <see cref="ISeqVisitor.RepeatWhen{T, TSignal}(RepeatWhenSeq{T, TSignal})"/>.
    /// </remarks>
    public static Seq<T> RepeatWhen<T, TSignal>(
        this Seq<T> source,
        Func<Seq<object>, Seq<TSignal>> handler,
        [CallerArgumentExpression(nameof(handler))] string text = "") =>
        new RepeatWhenSeq<T, TSignal>(source, handler, text);
}

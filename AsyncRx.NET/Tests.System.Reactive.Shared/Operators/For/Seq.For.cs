// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information. 

using System.Runtime.CompilerServices;

namespace Tests.System.Reactive.Shared;

public static partial class Seq
{
    /// <summary>Describes <c>Seq.For(source, resultSelector)</c>.</summary>
    /// <typeparam name="TSource">The type of the elements in the enumerable source.</typeparam>
    /// <typeparam name="TResult">The type of the elements in the result sequence.</typeparam>
    /// <param name="source">
    /// Enumerable sequence for which each element will be mapped to an observable sequence.
    /// </param>
    /// <param name="resultSelector">
    /// Function to select an observable sequence for each element of the enumerable.
    /// </param>
    /// <param name="text">
    /// Supplied by the compiler (the source text of <paramref name="resultSelector"/>), for
    /// printing the query in diagnostics; do not pass it.
    /// </param>
    /// <remarks>
    /// Builds a <see cref="ForSeq{TSource, TResult}"/>, which each target materializes through
    /// <see cref="ISeqVisitor.For{TSource, TResult}(ForSeq{TSource, TResult})"/>.
    /// </remarks>
    public static Seq<TResult> For<TSource, TResult>(
        IEnumerable<TSource> source,
        Func<TSource, Seq<TResult>> resultSelector,
        [CallerArgumentExpression(nameof(resultSelector))] string text = "") =>
        new ForSeq<TSource, TResult>(source, resultSelector, text);
}

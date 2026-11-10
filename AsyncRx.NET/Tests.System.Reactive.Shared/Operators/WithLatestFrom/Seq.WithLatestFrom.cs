// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information. 

using System.Runtime.CompilerServices;

namespace Tests.System.Reactive.Shared;

/// <summary>
/// The <c>WithLatestFrom</c> operator, as an extension method on <see cref="Seq"/>.
/// </summary>
/// <remarks>
/// Where an Rx.NET test writes <c>xs.WithLatestFrom(ys, (x, y) =&gt; x + y)</c>, a shared
/// scenario writes the same over two <see cref="Seq{T}"/>s. Rx.NET has the one overload, with
/// a result selector; the one node, in this folder, is materialized by a target through
/// <see cref="ISeqVisitor.WithLatestFrom{TFirst, TSecond, TResult}(WithLatestFromSeq{TFirst, TSecond, TResult})"/>.
/// AsyncRx.NET's tuple-returning form, which Rx.NET does not have, stays outside the model.
/// </remarks>
public static partial class Seq
{
    /// <summary>Describes <c>first.WithLatestFrom(second, resultSelector)</c>.</summary>
    /// <typeparam name="TFirst">The type of the elements in the first source sequence.</typeparam>
    /// <typeparam name="TSecond">The type of the elements in the second source sequence.</typeparam>
    /// <typeparam name="TResult">
    /// The type of the elements in the result sequence, returned by the selector function.
    /// </typeparam>
    /// <param name="first">First observable source.</param>
    /// <param name="second">Second observable source.</param>
    /// <param name="resultSelector">
    /// Function to invoke for each element from the first source combined with the latest
    /// element from the second source, if any.
    /// </param>
    /// <param name="text">
    /// Supplied by the compiler (the source text of the result selector), for printing the
    /// query in diagnostics; do not pass it.
    /// </param>
    /// <remarks>
    /// Builds a <see cref="WithLatestFromSeq{TFirst, TSecond, TResult}"/>, which each target
    /// materializes through
    /// <see cref="ISeqVisitor.WithLatestFrom{TFirst, TSecond, TResult}(WithLatestFromSeq{TFirst, TSecond, TResult})"/>.
    /// </remarks>
    public static Seq<TResult> WithLatestFrom<TFirst, TSecond, TResult>(
        this Seq<TFirst> first,
        Seq<TSecond> second,
        Func<TFirst, TSecond, TResult> resultSelector,
        [CallerArgumentExpression(nameof(resultSelector))] string text = "") =>
        new WithLatestFromSeq<TFirst, TSecond, TResult>(first, second, resultSelector, text);
}

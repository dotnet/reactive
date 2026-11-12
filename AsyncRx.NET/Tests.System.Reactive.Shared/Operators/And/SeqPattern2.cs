// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information. 

using System.Runtime.CompilerServices;

namespace Tests.System.Reactive.Shared;

/// <summary>Describes <c>left.And(right)</c>, a pattern over two sequences.</summary>
/// <typeparam name="T1">The type of the elements in the first observable sequence.</typeparam>
/// <typeparam name="T2">The type of the elements in the second observable sequence.</typeparam>
/// <remarks>
/// Built by <see cref="AndExtensions.And{T1, T2}(Seq{T1}, Seq{T2})"/>; materialized by each target through
/// <see cref="ISeqVisitor.And2{T1, T2}(SeqPattern2{T1, T2})"/>. A pattern is not a sequence: it is
/// what <c>Then</c> turns into a plan for <c>Seq.When</c>.
/// </remarks>
public sealed class SeqPattern2<T1, T2>(Seq<T1> left, Seq<T2> right)
    : Description<SeqPattern2<T1, T2>>
{
    /// <summary>Observable sequence to match with the right sequence.</summary>
    public Seq<T1> Left => left;

    /// <summary>Observable sequence to match with the left sequence.</summary>
    public Seq<T2> Right => right;

    /// <summary>Describes <c>pattern.And(other)</c>, a pattern over three sequences.</summary>
    /// <typeparam name="T3">The type of the elements in the third observable sequence.</typeparam>
    /// <param name="other">Observable sequence to match with the two previous sequences.</param>
    /// <remarks>
    /// Builds a <see cref="SeqPattern3{T1, T2, T3}"/>, which each target materializes through
    /// <see cref="ISeqVisitor.And3{T1, T2, T3}(SeqPattern3{T1, T2, T3})"/>.
    /// </remarks>
    public SeqPattern3<T1, T2, T3> And<T3>(Seq<T3> other) => new(this, other);

    /// <summary>Describes <c>pattern.Then(selector)</c>.</summary>
    /// <typeparam name="TResult">
    /// The type of the elements in the result sequence, returned by the selector function.
    /// </typeparam>
    /// <param name="selector">
    /// Selector that will be invoked for elements in the source sequences.
    /// </param>
    /// <param name="text">
    /// Supplied by the compiler (the source text of <paramref name="selector"/>), for printing
    /// the query in diagnostics; do not pass it.
    /// </param>
    /// <remarks>
    /// Builds a <see cref="SeqPlan2{T1, T2, TResult}"/>, which each target materializes through
    /// <see cref="ISeqVisitor.Then2{T1, T2, TResult}(SeqPlan2{T1, T2, TResult})"/>.
    /// </remarks>
    public SeqPlan<TResult> Then<TResult>(
        Func<T1, T2, TResult> selector,
        [CallerArgumentExpression(nameof(selector))] string text = "") =>
        new SeqPlan2<T1, T2, TResult>(this, selector, text);

    /// <inheritdoc/>
    protected override Realized<SeqPattern2<T1, T2>> AcceptCore(ISeqVisitor visitor) =>
        visitor.And2(this);

    /// <inheritdoc/>
    public override string ToString() => $"{left}.And({right})";
}

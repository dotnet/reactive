// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information. 

using System.Runtime.CompilerServices;

namespace Tests.System.Reactive.Shared;

/// <summary>Describes <c>pattern.And(other)</c>, a pattern over five sequences.</summary>
/// <typeparam name="T1">The type of the elements in the first observable sequence.</typeparam>
/// <typeparam name="T2">The type of the elements in the second observable sequence.</typeparam>
/// <typeparam name="T3">The type of the elements in the third observable sequence.</typeparam>
/// <typeparam name="T4">The type of the elements in the fourth observable sequence.</typeparam>
/// <typeparam name="T5">The type of the elements in the fifth observable sequence.</typeparam>
/// <remarks>
/// Built by <see cref="SeqPattern4{T1, T2, T3, T4}.And{T5}(Seq{T5})"/>; materialized by each target through
/// <see cref="ISeqVisitor.And5{T1, T2, T3, T4, T5}(SeqPattern5{T1, T2, T3, T4, T5})"/>. A pattern is not a sequence: it is
/// what <c>Then</c> turns into a plan for <c>Seq.When</c>.
/// </remarks>
public sealed class SeqPattern5<T1, T2, T3, T4, T5>(SeqPattern4<T1, T2, T3, T4> pattern, Seq<
        T5> other)
    : Description<SeqPattern5<T1, T2, T3, T4, T5>>
{
    /// <summary>The pattern over the four previous sequences.</summary>
    public SeqPattern4<T1, T2, T3, T4> Pattern => pattern;

    /// <summary>Observable sequence to match with the four previous sequences.</summary>
    public Seq<T5> Other => other;

    /// <summary>Describes <c>pattern.And(other)</c>, a pattern over six sequences.</summary>
    /// <typeparam name="T6">The type of the elements in the sixth observable sequence.</typeparam>
    /// <param name="other">Observable sequence to match with the five previous sequences.</param>
    /// <remarks>
    /// Builds a <see cref="SeqPattern6{T1, T2, T3, T4, T5, T6}"/>, which each target materializes through
    /// <see cref="ISeqVisitor.And6{T1, T2, T3, T4, T5, T6}(SeqPattern6{T1, T2, T3, T4, T5, T6})"/>.
    /// </remarks>
    public SeqPattern6<T1, T2, T3, T4, T5, T6> And<T6>(Seq<T6> other) => new(this, other);

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
    /// Builds a <see cref="SeqPlan5{T1, T2, T3, T4, T5, TResult}"/>, which each target materializes through
    /// <see cref="ISeqVisitor.Then5{T1, T2, T3, T4, T5, TResult}(SeqPlan5{T1, T2, T3, T4, T5, TResult})"/>.
    /// </remarks>
    public SeqPlan<TResult> Then<TResult>(
        Func<T1, T2, T3, T4, T5, TResult> selector,
        [CallerArgumentExpression(nameof(selector))] string text = "") =>
        new SeqPlan5<T1, T2, T3, T4, T5, TResult>(this, selector, text);

    /// <inheritdoc/>
    protected override Realized<SeqPattern5<T1, T2, T3, T4, T5>> AcceptCore(ISeqVisitor visitor) =>
        visitor.And5(this);

    /// <inheritdoc/>
    public override string ToString() => $"{pattern}.And({other})";
}

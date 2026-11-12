// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information. 

using System.Runtime.CompilerServices;

namespace Tests.System.Reactive.Shared;

/// <summary>Describes <c>pattern.And(other)</c>, a pattern over four sequences.</summary>
/// <typeparam name="T1">The type of the elements in the first observable sequence.</typeparam>
/// <typeparam name="T2">The type of the elements in the second observable sequence.</typeparam>
/// <typeparam name="T3">The type of the elements in the third observable sequence.</typeparam>
/// <typeparam name="T4">The type of the elements in the fourth observable sequence.</typeparam>
/// <remarks>
/// Built by <see cref="SeqPattern3{T1, T2, T3}.And{T4}(Seq{T4})"/>; materialized by each target through
/// <see cref="ISeqVisitor.And4{T1, T2, T3, T4}(SeqPattern4{T1, T2, T3, T4})"/>. A pattern is not a sequence: it is
/// what <c>Then</c> turns into a plan for <c>Seq.When</c>.
/// </remarks>
public sealed class SeqPattern4<T1, T2, T3, T4>(SeqPattern3<T1, T2, T3> pattern, Seq<T4> other)
    : Description<SeqPattern4<T1, T2, T3, T4>>
{
    /// <summary>The pattern over the three previous sequences.</summary>
    public SeqPattern3<T1, T2, T3> Pattern => pattern;

    /// <summary>Observable sequence to match with the three previous sequences.</summary>
    public Seq<T4> Other => other;

    /// <summary>Describes <c>pattern.And(other)</c>, a pattern over five sequences.</summary>
    /// <typeparam name="T5">The type of the elements in the fifth observable sequence.</typeparam>
    /// <param name="other">Observable sequence to match with the four previous sequences.</param>
    /// <remarks>
    /// Builds a <see cref="SeqPattern5{T1, T2, T3, T4, T5}"/>, which each target materializes through
    /// <see cref="ISeqVisitor.And5{T1, T2, T3, T4, T5}(SeqPattern5{T1, T2, T3, T4, T5})"/>.
    /// </remarks>
    public SeqPattern5<T1, T2, T3, T4, T5> And<T5>(Seq<T5> other) => new(this, other);

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
    /// Builds a <see cref="SeqPlan4{T1, T2, T3, T4, TResult}"/>, which each target materializes through
    /// <see cref="ISeqVisitor.Then4{T1, T2, T3, T4, TResult}(SeqPlan4{T1, T2, T3, T4, TResult})"/>.
    /// </remarks>
    public SeqPlan<TResult> Then<TResult>(
        Func<T1, T2, T3, T4, TResult> selector,
        [CallerArgumentExpression(nameof(selector))] string text = "") =>
        new SeqPlan4<T1, T2, T3, T4, TResult>(this, selector, text);

    /// <inheritdoc/>
    protected override Realized<SeqPattern4<T1, T2, T3, T4>> AcceptCore(ISeqVisitor visitor) =>
        visitor.And4(this);

    /// <inheritdoc/>
    public override string ToString() => $"{pattern}.And({other})";
}

// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information. 

using System.Runtime.CompilerServices;

namespace Tests.System.Reactive.Shared;

/// <summary>Describes <c>pattern.And(other)</c>, a pattern over three sequences.</summary>
/// <typeparam name="T1">The type of the elements in the first observable sequence.</typeparam>
/// <typeparam name="T2">The type of the elements in the second observable sequence.</typeparam>
/// <typeparam name="T3">The type of the elements in the third observable sequence.</typeparam>
/// <remarks>
/// Built by <see cref="SeqPattern2{T1, T2}.And{T3}(Seq{T3})"/>; materialized by each target through
/// <see cref="ISeqVisitor.And3{T1, T2, T3}(SeqPattern3{T1, T2, T3})"/>. A pattern is not a sequence: it is
/// what <c>Then</c> turns into a plan for <c>Seq.When</c>.
/// </remarks>
public sealed class SeqPattern3<T1, T2, T3>(SeqPattern2<T1, T2> pattern, Seq<T3> other)
    : Description<SeqPattern3<T1, T2, T3>>
{
    /// <summary>The pattern over the two previous sequences.</summary>
    public SeqPattern2<T1, T2> Pattern => pattern;

    /// <summary>Observable sequence to match with the two previous sequences.</summary>
    public Seq<T3> Other => other;

    /// <summary>Describes <c>pattern.And(other)</c>, a pattern over four sequences.</summary>
    /// <typeparam name="T4">The type of the elements in the fourth observable sequence.</typeparam>
    /// <param name="other">Observable sequence to match with the three previous sequences.</param>
    /// <remarks>
    /// Builds a <see cref="SeqPattern4{T1, T2, T3, T4}"/>, which each target materializes through
    /// <see cref="ISeqVisitor.And4{T1, T2, T3, T4}(SeqPattern4{T1, T2, T3, T4})"/>.
    /// </remarks>
    public SeqPattern4<T1, T2, T3, T4> And<T4>(Seq<T4> other) => new(this, other);

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
    /// Builds a <see cref="SeqPlan3{T1, T2, T3, TResult}"/>, which each target materializes through
    /// <see cref="ISeqVisitor.Then3{T1, T2, T3, TResult}(SeqPlan3{T1, T2, T3, TResult})"/>.
    /// </remarks>
    public SeqPlan<TResult> Then<TResult>(
        Func<T1, T2, T3, TResult> selector,
        [CallerArgumentExpression(nameof(selector))] string text = "") =>
        new SeqPlan3<T1, T2, T3, TResult>(this, selector, text);

    /// <inheritdoc/>
    protected override Realized<SeqPattern3<T1, T2, T3>> AcceptCore(ISeqVisitor visitor) =>
        visitor.And3(this);

    /// <inheritdoc/>
    public override string ToString() => $"{pattern}.And({other})";
}

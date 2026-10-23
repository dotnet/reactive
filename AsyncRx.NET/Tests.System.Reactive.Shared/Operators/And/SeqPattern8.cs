// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information. 

using System.Runtime.CompilerServices;

namespace Tests.System.Reactive.Shared;

/// <summary>Describes <c>pattern.And(other)</c>, a pattern over eight sequences.</summary>
/// <typeparam name="T1">The type of the elements in the first observable sequence.</typeparam>
/// <typeparam name="T2">The type of the elements in the second observable sequence.</typeparam>
/// <typeparam name="T3">The type of the elements in the third observable sequence.</typeparam>
/// <typeparam name="T4">The type of the elements in the fourth observable sequence.</typeparam>
/// <typeparam name="T5">The type of the elements in the fifth observable sequence.</typeparam>
/// <typeparam name="T6">The type of the elements in the sixth observable sequence.</typeparam>
/// <typeparam name="T7">The type of the elements in the seventh observable sequence.</typeparam>
/// <typeparam name="T8">The type of the elements in the eighth observable sequence.</typeparam>
/// <remarks>
/// Built by <see cref="SeqPattern7{T1, T2, T3, T4, T5, T6, T7}.And{T8}(Seq{T8})"/>; materialized by each target through
/// <see cref="ISeqVisitor.And8{T1, T2, T3, T4, T5, T6, T7, T8}(SeqPattern8{T1, T2, T3, T4, T5, T6, T7, T8})"/>. A pattern is not a sequence: it is
/// what <c>Then</c> turns into a plan for <c>Seq.When</c>.
/// </remarks>
public sealed class SeqPattern8<T1, T2, T3, T4, T5, T6, T7, T8>(SeqPattern7<T1, T2, T3, T4, T5, T6,
        T7> pattern, Seq<T8> other)
    : Description<SeqPattern8<T1, T2, T3, T4, T5, T6, T7, T8>>
{
    /// <summary>The pattern over the seven previous sequences.</summary>
    public SeqPattern7<T1, T2, T3, T4, T5, T6, T7> Pattern => pattern;

    /// <summary>Observable sequence to match with the seven previous sequences.</summary>
    public Seq<T8> Other => other;

    /// <summary>Describes <c>pattern.And(other)</c>, a pattern over nine sequences.</summary>
    /// <typeparam name="T9">The type of the elements in the ninth observable sequence.</typeparam>
    /// <param name="other">Observable sequence to match with the eight previous sequences.</param>
    /// <remarks>
    /// Builds a <see cref="SeqPattern9{T1, T2, T3, T4, T5, T6, T7, T8, T9}"/>, which each target materializes through
    /// <see cref="ISeqVisitor.And9{T1, T2, T3, T4, T5, T6, T7, T8, T9}(SeqPattern9{T1, T2, T3, T4, T5, T6, T7, T8, T9})"/>.
    /// </remarks>
    public SeqPattern9<T1, T2, T3, T4, T5, T6, T7, T8, T9> And<T9>(Seq<T9> other) => new(this,
            other);

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
    /// Builds a <see cref="SeqPlan8{T1, T2, T3, T4, T5, T6, T7, T8, TResult}"/>, which each target materializes through
    /// <see cref="ISeqVisitor.Then8{T1, T2, T3, T4, T5, T6, T7, T8, TResult}(SeqPlan8{T1, T2, T3, T4, T5, T6, T7, T8, TResult})"/>.
    /// </remarks>
    public SeqPlan<TResult> Then<TResult>(
        Func<T1, T2, T3, T4, T5, T6, T7, T8, TResult> selector,
        [CallerArgumentExpression(nameof(selector))] string text = "") =>
        new SeqPlan8<T1, T2, T3, T4, T5, T6, T7, T8, TResult>(this, selector, text);

    /// <inheritdoc/>
    protected override Realized<SeqPattern8<T1, T2, T3, T4, T5, T6, T7, T8>> AcceptCore(
            ISeqVisitor visitor) =>
        visitor.And8(this);

    /// <inheritdoc/>
    public override string ToString() => $"{pattern}.And({other})";
}

// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information. 

using System.Runtime.CompilerServices;

namespace Tests.System.Reactive.Shared;

/// <summary>Describes <c>pattern.And(other)</c>, a pattern over eleven sequences.</summary>
/// <typeparam name="T1">The type of the elements in the first observable sequence.</typeparam>
/// <typeparam name="T2">The type of the elements in the second observable sequence.</typeparam>
/// <typeparam name="T3">The type of the elements in the third observable sequence.</typeparam>
/// <typeparam name="T4">The type of the elements in the fourth observable sequence.</typeparam>
/// <typeparam name="T5">The type of the elements in the fifth observable sequence.</typeparam>
/// <typeparam name="T6">The type of the elements in the sixth observable sequence.</typeparam>
/// <typeparam name="T7">The type of the elements in the seventh observable sequence.</typeparam>
/// <typeparam name="T8">The type of the elements in the eighth observable sequence.</typeparam>
/// <typeparam name="T9">The type of the elements in the ninth observable sequence.</typeparam>
/// <typeparam name="T10">The type of the elements in the tenth observable sequence.</typeparam>
/// <typeparam name="T11">The type of the elements in the eleventh observable sequence.</typeparam>
/// <remarks>
/// Built by <see cref="SeqPattern10{T1, T2, T3, T4, T5, T6, T7, T8, T9, T10}.And{T11}(Seq{T11})"/>; materialized by each target through
/// <see cref="ISeqVisitor.And11{T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11}(SeqPattern11{T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11})"/>. A pattern is not a sequence: it is
/// what <c>Then</c> turns into a plan for <c>Seq.When</c>.
/// </remarks>
public sealed class SeqPattern11<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11>(SeqPattern10<T1, T2,
        T3, T4, T5, T6, T7, T8, T9, T10> pattern, Seq<T11> other)
    : Description<SeqPattern11<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11>>
{
    /// <summary>The pattern over the ten previous sequences.</summary>
    public SeqPattern10<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10> Pattern => pattern;

    /// <summary>Observable sequence to match with the ten previous sequences.</summary>
    public Seq<T11> Other => other;

    /// <summary>Describes <c>pattern.And(other)</c>, a pattern over twelve sequences.</summary>
    /// <typeparam name="T12">
    /// The type of the elements in the twelfth observable sequence.
    /// </typeparam>
    /// <param name="other">Observable sequence to match with the eleven previous sequences.</param>
    /// <remarks>
    /// Builds a <see cref="SeqPattern12{T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12}"/>, which each target materializes through
    /// <see cref="ISeqVisitor.And12{T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12}(SeqPattern12{T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12})"/>.
    /// </remarks>
    public SeqPattern12<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12> And<T12>(Seq<
            T12> other) => new(this, other);

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
    /// Builds a <see cref="SeqPlan11{T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, TResult}"/>, which each target materializes through
    /// <see cref="ISeqVisitor.Then11{T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, TResult}(SeqPlan11{T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, TResult})"/>.
    /// </remarks>
    public SeqPlan<TResult> Then<TResult>(
        Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, TResult> selector,
        [CallerArgumentExpression(nameof(selector))] string text = "") =>
        new SeqPlan11<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, TResult>(this, selector, text);

    /// <inheritdoc/>
    protected override Realized<SeqPattern11<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10,
            T11>> AcceptCore(ISeqVisitor visitor) =>
        visitor.And11(this);

    /// <inheritdoc/>
    public override string ToString() => $"{pattern}.And({other})";
}

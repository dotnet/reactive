// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information. 

using System.Runtime.CompilerServices;

namespace Tests.System.Reactive.Shared;

/// <summary>Describes <c>pattern.And(other)</c>, a pattern over fourteen sequences.</summary>
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
/// <typeparam name="T12">The type of the elements in the twelfth observable sequence.</typeparam>
/// <typeparam name="T13">
/// The type of the elements in the thirteenth observable sequence.
/// </typeparam>
/// <typeparam name="T14">
/// The type of the elements in the fourteenth observable sequence.
/// </typeparam>
/// <remarks>
/// Built by <see cref="SeqPattern13{T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13}.And{T14}(Seq{T14})"/>; materialized by each target through
/// <see cref="ISeqVisitor.And14{T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14}(SeqPattern14{T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14})"/>. A pattern is not a sequence: it is
/// what <c>Then</c> turns into a plan for <c>Seq.When</c>.
/// </remarks>
public sealed class SeqPattern14<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14>(
        SeqPattern13<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13> pattern, Seq<
        T14> other)
    : Description<SeqPattern14<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14>>
{
    /// <summary>The pattern over the thirteen previous sequences.</summary>
    public SeqPattern13<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13> Pattern => pattern;

    /// <summary>Observable sequence to match with the thirteen previous sequences.</summary>
    public Seq<T14> Other => other;

    /// <summary>Describes <c>pattern.And(other)</c>, a pattern over fifteen sequences.</summary>
    /// <typeparam name="T15">
    /// The type of the elements in the fifteenth observable sequence.
    /// </typeparam>
    /// <param name="other">
    /// Observable sequence to match with the fourteen previous sequences.
    /// </param>
    /// <remarks>
    /// Builds a <see cref="SeqPattern15{T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15}"/>, which each target materializes through
    /// <see cref="ISeqVisitor.And15{T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15}(SeqPattern15{T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15})"/>.
    /// </remarks>
    public SeqPattern15<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15> And<T15>(
            Seq<T15> other) => new(this, other);

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
    /// Builds a <see cref="SeqPlan14{T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, TResult}"/>, which each target materializes through
    /// <see cref="ISeqVisitor.Then14{T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, TResult}(SeqPlan14{T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, TResult})"/>.
    /// </remarks>
    public SeqPlan<TResult> Then<TResult>(
        Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, TResult> selector,
        [CallerArgumentExpression(nameof(selector))] string text = "") =>
        new SeqPlan14<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, TResult>(this,
                selector, text);

    /// <inheritdoc/>
    protected override Realized<SeqPattern14<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13,
            T14>> AcceptCore(ISeqVisitor visitor) =>
        visitor.And14(this);

    /// <inheritdoc/>
    public override string ToString() => $"{pattern}.And({other})";
}

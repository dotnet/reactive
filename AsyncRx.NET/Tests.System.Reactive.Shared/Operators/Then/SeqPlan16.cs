// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information. 

namespace Tests.System.Reactive.Shared;

/// <summary>Describes <c>pattern.Then(selector)</c>, a plan over sixteen sequences.</summary>
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
/// <typeparam name="T15">The type of the elements in the fifteenth observable sequence.</typeparam>
/// <typeparam name="T16">The type of the elements in the sixteenth observable sequence.</typeparam>
/// <typeparam name="TResult">
/// The type of the elements in the result sequence, returned by the selector function.
/// </typeparam>
/// <remarks>
/// Built by <see cref="SeqPattern16{T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16}.Then{TResult}(Func{T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16, TResult}, string)"/>; materialized by each target through
/// <see cref="ISeqVisitor.Then16{T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16, TResult}(SeqPlan16{T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16, TResult})"/>.
/// </remarks>
public sealed class SeqPlan16<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16,
        TResult>(SeqPattern16<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15,
        T16> pattern, Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16,
        TResult> selector, string text)
    : SeqPlan<TResult>
{
    /// <summary>The pattern over the sixteen sequences.</summary>
    public SeqPattern16<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15,
            T16> Pattern => pattern;

    /// <summary>Selector that will be invoked for elements in the source sequences.</summary>
    public Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16,
            TResult> Selector => selector;

    /// <inheritdoc/>
    protected override Realized<SeqPlan<TResult>> AcceptCore(ISeqVisitor visitor) =>
        visitor.Then16(this);

    /// <inheritdoc/>
    public override string ToString() => $"{pattern}.Then({text})";
}

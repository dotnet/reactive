// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information. 

namespace Tests.System.Reactive.Shared;

/// <summary>Describes <c>pattern.Then(selector)</c>, a plan over eleven sequences.</summary>
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
/// <typeparam name="TResult">
/// The type of the elements in the result sequence, returned by the selector function.
/// </typeparam>
/// <remarks>
/// Built by <see cref="SeqPattern11{T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11}.Then{TResult}(Func{T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, TResult}, string)"/>; materialized by each target through
/// <see cref="ISeqVisitor.Then11{T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, TResult}(SeqPlan11{T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, TResult})"/>.
/// </remarks>
public sealed class SeqPlan11<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, TResult>(SeqPattern11<
        T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11> pattern, Func<T1, T2, T3, T4, T5, T6, T7, T8,
        T9, T10, T11, TResult> selector, string text)
    : SeqPlan<TResult>
{
    /// <summary>The pattern over the eleven sequences.</summary>
    public SeqPattern11<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11> Pattern => pattern;

    /// <summary>Selector that will be invoked for elements in the source sequences.</summary>
    public Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, TResult> Selector => selector;

    /// <inheritdoc/>
    protected override Realized<SeqPlan<TResult>> AcceptCore(ISeqVisitor visitor) =>
        visitor.Then11(this);

    /// <inheritdoc/>
    public override string ToString() => $"{pattern}.Then({text})";
}

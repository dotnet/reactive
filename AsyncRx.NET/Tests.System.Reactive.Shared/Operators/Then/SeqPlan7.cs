// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information. 

namespace Tests.System.Reactive.Shared;

/// <summary>Describes <c>pattern.Then(selector)</c>, a plan over seven sequences.</summary>
/// <typeparam name="T1">The type of the elements in the first observable sequence.</typeparam>
/// <typeparam name="T2">The type of the elements in the second observable sequence.</typeparam>
/// <typeparam name="T3">The type of the elements in the third observable sequence.</typeparam>
/// <typeparam name="T4">The type of the elements in the fourth observable sequence.</typeparam>
/// <typeparam name="T5">The type of the elements in the fifth observable sequence.</typeparam>
/// <typeparam name="T6">The type of the elements in the sixth observable sequence.</typeparam>
/// <typeparam name="T7">The type of the elements in the seventh observable sequence.</typeparam>
/// <typeparam name="TResult">
/// The type of the elements in the result sequence, returned by the selector function.
/// </typeparam>
/// <remarks>
/// Built by <see cref="SeqPattern7{T1, T2, T3, T4, T5, T6, T7}.Then{TResult}(Func{T1, T2, T3, T4, T5, T6, T7, TResult}, string)"/>; materialized by each target through
/// <see cref="ISeqVisitor.Then7{T1, T2, T3, T4, T5, T6, T7, TResult}(SeqPlan7{T1, T2, T3, T4, T5, T6, T7, TResult})"/>.
/// </remarks>
public sealed class SeqPlan7<T1, T2, T3, T4, T5, T6, T7, TResult>(SeqPattern7<T1, T2, T3, T4, T5,
        T6, T7> pattern, Func<T1, T2, T3, T4, T5, T6, T7, TResult> selector, string text)
    : SeqPlan<TResult>
{
    /// <summary>The pattern over the seven sequences.</summary>
    public SeqPattern7<T1, T2, T3, T4, T5, T6, T7> Pattern => pattern;

    /// <summary>Selector that will be invoked for elements in the source sequences.</summary>
    public Func<T1, T2, T3, T4, T5, T6, T7, TResult> Selector => selector;

    /// <inheritdoc/>
    protected override Realized<SeqPlan<TResult>> AcceptCore(ISeqVisitor visitor) =>
        visitor.Then7(this);

    /// <inheritdoc/>
    public override string ToString() => $"{pattern}.Then({text})";
}

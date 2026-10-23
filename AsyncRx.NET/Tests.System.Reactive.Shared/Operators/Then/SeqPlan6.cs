// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information. 

namespace Tests.System.Reactive.Shared;

/// <summary>Describes <c>pattern.Then(selector)</c>, a plan over six sequences.</summary>
/// <typeparam name="T1">The type of the elements in the first observable sequence.</typeparam>
/// <typeparam name="T2">The type of the elements in the second observable sequence.</typeparam>
/// <typeparam name="T3">The type of the elements in the third observable sequence.</typeparam>
/// <typeparam name="T4">The type of the elements in the fourth observable sequence.</typeparam>
/// <typeparam name="T5">The type of the elements in the fifth observable sequence.</typeparam>
/// <typeparam name="T6">The type of the elements in the sixth observable sequence.</typeparam>
/// <typeparam name="TResult">
/// The type of the elements in the result sequence, returned by the selector function.
/// </typeparam>
/// <remarks>
/// Built by <see cref="SeqPattern6{T1, T2, T3, T4, T5, T6}.Then{TResult}(Func{T1, T2, T3, T4, T5, T6, TResult}, string)"/>; materialized by each target through
/// <see cref="ISeqVisitor.Then6{T1, T2, T3, T4, T5, T6, TResult}(SeqPlan6{T1, T2, T3, T4, T5, T6, TResult})"/>.
/// </remarks>
public sealed class SeqPlan6<T1, T2, T3, T4, T5, T6, TResult>(SeqPattern6<T1, T2, T3, T4, T5,
        T6> pattern, Func<T1, T2, T3, T4, T5, T6, TResult> selector, string text)
    : SeqPlan<TResult>
{
    /// <summary>The pattern over the six sequences.</summary>
    public SeqPattern6<T1, T2, T3, T4, T5, T6> Pattern => pattern;

    /// <summary>Selector that will be invoked for elements in the source sequences.</summary>
    public Func<T1, T2, T3, T4, T5, T6, TResult> Selector => selector;

    /// <inheritdoc/>
    protected override Realized<SeqPlan<TResult>> AcceptCore(ISeqVisitor visitor) =>
        visitor.Then6(this);

    /// <inheritdoc/>
    public override string ToString() => $"{pattern}.Then({text})";
}

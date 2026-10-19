// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information. 

namespace Tests.System.Reactive.Shared;

/// <summary>Describes <c>pattern.Then(selector)</c>, a plan over four sequences.</summary>
/// <typeparam name="T1">The type of the elements in the first observable sequence.</typeparam>
/// <typeparam name="T2">The type of the elements in the second observable sequence.</typeparam>
/// <typeparam name="T3">The type of the elements in the third observable sequence.</typeparam>
/// <typeparam name="T4">The type of the elements in the fourth observable sequence.</typeparam>
/// <typeparam name="TResult">
/// The type of the elements in the result sequence, returned by the selector function.
/// </typeparam>
/// <remarks>
/// Built by <see cref="SeqPattern4{T1, T2, T3, T4}.Then{TResult}(Func{T1, T2, T3, T4, TResult}, string)"/>; materialized by each target through
/// <see cref="ISeqVisitor.Then4{T1, T2, T3, T4, TResult}(SeqPlan4{T1, T2, T3, T4, TResult})"/>.
/// </remarks>
public sealed class SeqPlan4<T1, T2, T3, T4, TResult>(SeqPattern4<T1, T2, T3, T4> pattern, Func<T1,
        T2, T3, T4, TResult> selector, string text)
    : SeqPlan<TResult>
{
    /// <summary>The pattern over the four sequences.</summary>
    public SeqPattern4<T1, T2, T3, T4> Pattern => pattern;

    /// <summary>Selector that will be invoked for elements in the source sequences.</summary>
    public Func<T1, T2, T3, T4, TResult> Selector => selector;

    /// <inheritdoc/>
    protected override Realized<SeqPlan<TResult>> AcceptCore(ISeqVisitor visitor) =>
        visitor.Then4(this);

    /// <inheritdoc/>
    public override string ToString() => $"{pattern}.Then({text})";
}

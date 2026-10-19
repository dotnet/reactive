// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information. 

namespace Tests.System.Reactive.Shared;

/// <summary>Describes <c>pattern.Then(selector)</c>, a plan over two sequences.</summary>
/// <typeparam name="T1">The type of the elements in the first observable sequence.</typeparam>
/// <typeparam name="T2">The type of the elements in the second observable sequence.</typeparam>
/// <typeparam name="TResult">
/// The type of the elements in the result sequence, returned by the selector function.
/// </typeparam>
/// <remarks>
/// Built by <see cref="SeqPattern2{T1, T2}.Then{TResult}(Func{T1, T2, TResult}, string)"/>; materialized by each target through
/// <see cref="ISeqVisitor.Then2{T1, T2, TResult}(SeqPlan2{T1, T2, TResult})"/>.
/// </remarks>
public sealed class SeqPlan2<T1, T2, TResult>(SeqPattern2<T1, T2> pattern, Func<T1, T2,
        TResult> selector, string text)
    : SeqPlan<TResult>
{
    /// <summary>The pattern over the two sequences.</summary>
    public SeqPattern2<T1, T2> Pattern => pattern;

    /// <summary>Selector that will be invoked for elements in the source sequences.</summary>
    public Func<T1, T2, TResult> Selector => selector;

    /// <inheritdoc/>
    protected override Realized<SeqPlan<TResult>> AcceptCore(ISeqVisitor visitor) =>
        visitor.Then2(this);

    /// <inheritdoc/>
    public override string ToString() => $"{pattern}.Then({text})";
}

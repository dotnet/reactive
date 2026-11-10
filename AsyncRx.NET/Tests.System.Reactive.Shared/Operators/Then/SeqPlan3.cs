// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information. 

namespace Tests.System.Reactive.Shared;

/// <summary>Describes <c>pattern.Then(selector)</c>, a plan over three sequences.</summary>
/// <typeparam name="T1">The type of the elements in the first observable sequence.</typeparam>
/// <typeparam name="T2">The type of the elements in the second observable sequence.</typeparam>
/// <typeparam name="T3">The type of the elements in the third observable sequence.</typeparam>
/// <typeparam name="TResult">
/// The type of the elements in the result sequence, returned by the selector function.
/// </typeparam>
/// <remarks>
/// Built by <see cref="SeqPattern3{T1, T2, T3}.Then{TResult}(Func{T1, T2, T3, TResult}, string)"/>; materialized by each target through
/// <see cref="ISeqVisitor.Then3{T1, T2, T3, TResult}(SeqPlan3{T1, T2, T3, TResult})"/>.
/// </remarks>
public sealed class SeqPlan3<T1, T2, T3, TResult>(SeqPattern3<T1, T2, T3> pattern, Func<T1, T2, T3,
        TResult> selector, string text)
    : SeqPlan<TResult>
{
    /// <summary>The pattern over the three sequences.</summary>
    public SeqPattern3<T1, T2, T3> Pattern => pattern;

    /// <summary>Selector that will be invoked for elements in the source sequences.</summary>
    public Func<T1, T2, T3, TResult> Selector => selector;

    /// <inheritdoc/>
    protected override Realized<SeqPlan<TResult>> AcceptCore(ISeqVisitor visitor) =>
        visitor.Then3(this);

    /// <inheritdoc/>
    public override string ToString() => $"{pattern}.Then({text})";
}

// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information. 

namespace Tests.System.Reactive.Shared;

/// <summary>Describes <c>source.Then(selector)</c>, a plan over one sequence.</summary>
/// <typeparam name="T1">The type of the elements in the first observable sequence.</typeparam>
/// <typeparam name="TResult">
/// The type of the elements in the result sequence, returned by the selector function.
/// </typeparam>
/// <remarks>
/// Built by <see cref="ThenExtensions.Then{TSource, TResult}(Seq{TSource}, Func{TSource, TResult}, string)"/>; materialized by each target through
/// <see cref="ISeqVisitor.Then1{T1, TResult}(SeqPlan1{T1, TResult})"/>.
/// </remarks>
public sealed class SeqPlan1<T1, TResult>(Seq<T1> source, Func<T1, TResult> selector, string text)
    : SeqPlan<TResult>
{
    /// <summary>Observable sequence to apply the selector on.</summary>
    public Seq<T1> Source => source;

    /// <summary>Selector that will be invoked for elements in the source sequence.</summary>
    public Func<T1, TResult> Selector => selector;

    /// <inheritdoc/>
    protected override Realized<SeqPlan<TResult>> AcceptCore(ISeqVisitor visitor) =>
        visitor.Then1(this);

    /// <inheritdoc/>
    public override string ToString() => $"{source}.Then({text})";
}

// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information. 

namespace Tests.System.Reactive.Shared;

/// <summary>Describes <c>source.SingleAsync(predicate)</c>.</summary>
/// <remarks>
/// Built by <see cref="SingleAsyncExtensions.SingleAsync{T}(Seq{T}, Func{T, bool}, string)"/>;
/// materialized by each target through
/// <see cref="ISeqVisitor.SingleAsyncPredicate{T}(SingleAsyncPredicateSeq{T})"/>.
/// </remarks>
public sealed class SingleAsyncPredicateSeq<T>(
    Seq<T> source,
    Func<T, bool> predicate,
    string text)
    : Seq<T>
{
    /// <summary>Source observable sequence.</summary>
    public Seq<T> Source => source;

    /// <summary>A predicate function to evaluate for elements in the source sequence.</summary>
    public Func<T, bool> Predicate => predicate;

    /// <inheritdoc/>
    protected override Realized<Seq<T>> AcceptCore(ISeqVisitor visitor) =>
        visitor.SingleAsyncPredicate(this);

    /// <inheritdoc/>
    public override string ToString() => $"{source}.SingleAsync({text})";
}

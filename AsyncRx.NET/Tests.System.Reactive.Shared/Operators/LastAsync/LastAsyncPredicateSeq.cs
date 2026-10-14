// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information. 

namespace Tests.System.Reactive.Shared;

/// <summary>Describes <c>source.LastAsync(predicate)</c>.</summary>
/// <remarks>
/// Built by <see cref="LastAsyncExtensions.LastAsync{T}(Seq{T}, Func{T, bool}, string)"/>;
/// materialized by each target through
/// <see cref="ISeqVisitor.LastAsyncPredicate{T}(LastAsyncPredicateSeq{T})"/>.
/// </remarks>
public sealed class LastAsyncPredicateSeq<T>(
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
        visitor.LastAsyncPredicate(this);

    /// <inheritdoc/>
    public override string ToString() => $"{source}.LastAsync({text})";
}

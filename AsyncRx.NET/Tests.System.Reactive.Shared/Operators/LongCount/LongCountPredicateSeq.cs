// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information. 

namespace Tests.System.Reactive.Shared;

/// <summary>Describes <c>source.LongCount(predicate)</c>.</summary>
/// <remarks>
/// Built by <see cref="LongCountExtensions.LongCount{T}(Seq{T}, Func{T, bool}, string)"/>;
/// materialized by each target through
/// <see cref="ISeqVisitor.LongCountPredicate{T}(LongCountPredicateSeq{T})"/>.
/// </remarks>
public sealed class LongCountPredicateSeq<T>(Seq<T> source, Func<T, bool> predicate, string text)
    : Seq<long>
{
    /// <summary>An observable sequence that contains elements to be counted.</summary>
    public Seq<T> Source => source;

    /// <summary>A function to test each element for a condition.</summary>
    public Func<T, bool> Predicate => predicate;

    /// <inheritdoc/>
    protected override Realized<Seq<long>> AcceptCore(ISeqVisitor visitor) =>
        visitor.LongCountPredicate(this);

    /// <inheritdoc/>
    public override string ToString() => $"{source}.LongCount({text})";
}

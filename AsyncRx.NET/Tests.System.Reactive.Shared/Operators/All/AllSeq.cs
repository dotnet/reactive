// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information. 

namespace Tests.System.Reactive.Shared;

/// <summary>Describes <c>source.All(predicate)</c>.</summary>
/// <remarks>
/// Built by <see cref="AllExtensions.All{T}(Seq{T}, Func{T, bool}, string)"/>;
/// materialized by each target through <see cref="ISeqVisitor.All{T}(AllSeq{T})"/>.
/// </remarks>
public sealed class AllSeq<T>(
    Seq<T> source,
    Func<T, bool> predicate,
    string text)
    : Seq<bool>
{
    /// <summary>An observable sequence whose elements to apply the predicate to.</summary>
    public Seq<T> Source => source;

    /// <summary>A function to test each element for a condition.</summary>
    public Func<T, bool> Predicate => predicate;

    /// <inheritdoc/>
    protected override Realized<Seq<bool>> AcceptCore(ISeqVisitor visitor) =>
        visitor.All(this);

    /// <inheritdoc/>
    public override string ToString() => $"{source}.All({text})";
}

// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information. 

namespace Tests.System.Reactive.Shared;

/// <summary>Describes <c>source.Distinct()</c>.</summary>
/// <remarks>
/// Built by <see cref="DistinctExtensions.Distinct{T}(Seq{T})"/>;
/// materialized by each target through
/// <see cref="ISeqVisitor.Distinct{T}(DistinctSeq{T})"/>.
/// </remarks>
public sealed class DistinctSeq<T>(Seq<T> source)
    : Seq<T>
{
    /// <summary>An observable sequence to retain distinct elements for.</summary>
    public Seq<T> Source => source;

    /// <inheritdoc/>
    protected override Realized<Seq<T>> AcceptCore(ISeqVisitor visitor) =>
        visitor.Distinct(this);

    /// <inheritdoc/>
    public override string ToString() => $"{source}.Distinct()";
}

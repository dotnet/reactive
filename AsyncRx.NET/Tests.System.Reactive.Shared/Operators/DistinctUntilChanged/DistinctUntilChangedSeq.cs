// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information. 

namespace Tests.System.Reactive.Shared;

/// <summary>Describes <c>source.DistinctUntilChanged()</c>.</summary>
/// <remarks>
/// Built by <see cref="DistinctUntilChangedExtensions.DistinctUntilChanged{T}(Seq{T})"/>;
/// materialized by each target through
/// <see cref="ISeqVisitor.DistinctUntilChanged{T}(DistinctUntilChangedSeq{T})"/>.
/// </remarks>
public sealed class DistinctUntilChangedSeq<T>(Seq<T> source)
    : Seq<T>
{
    /// <summary>An observable sequence to retain distinct contiguous elements for.</summary>
    public Seq<T> Source => source;

    /// <inheritdoc/>
    protected override Realized<Seq<T>> AcceptCore(ISeqVisitor visitor) =>
        visitor.DistinctUntilChanged(this);

    /// <inheritdoc/>
    public override string ToString() => $"{source}.DistinctUntilChanged()";
}

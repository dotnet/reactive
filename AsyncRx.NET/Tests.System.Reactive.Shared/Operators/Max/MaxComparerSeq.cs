// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information. 

namespace Tests.System.Reactive.Shared;

/// <summary>Describes <c>source.Max(comparer)</c>.</summary>
/// <remarks>
/// Built by <see cref="Seq.Max{T}(Seq{T}, IComparer{T})"/>;
/// materialized by each target through
/// <see cref="ISeqVisitor.MaxComparer{T}(MaxComparerSeq{T})"/>.
/// </remarks>
public sealed class MaxComparerSeq<T>(Seq<T> source, IComparer<T> comparer) : Seq<T>
{
    /// <summary>An observable sequence to determine the maximum element of.</summary>
    public Seq<T> Source => source;

    /// <summary>Comparer used to compare elements.</summary>
    public IComparer<T> Comparer => comparer;

    /// <inheritdoc/>
    protected override Realized<Seq<T>> AcceptCore(ISeqVisitor visitor) =>
        visitor.MaxComparer(this);

    /// <inheritdoc/>
    public override string ToString() => $"{source}.Max({comparer})";
}

// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information. 

namespace Tests.System.Reactive.Shared;

/// <summary>Describes <c>first.Merge(second)</c>.</summary>
/// <remarks>
/// Built by <see cref="Seq.Merge{T}(Seq{T}, Seq{T})"/>; materialized by each target through
/// <see cref="ISeqVisitor.MergeBinary{T}(MergeBinarySeq{T})"/>.
/// </remarks>
public sealed class MergeBinarySeq<T>(Seq<T> first, Seq<T> second) : Seq<T>
{
    /// <summary>First observable sequence.</summary>
    public Seq<T> First => first;

    /// <summary>Second observable sequence.</summary>
    public Seq<T> Second => second;

    /// <inheritdoc/>
    protected override Realized<Seq<T>> AcceptCore(ISeqVisitor visitor) => visitor.MergeBinary(this);

    /// <inheritdoc/>
    public override string ToString() => $"{first}.Merge({second})";
}

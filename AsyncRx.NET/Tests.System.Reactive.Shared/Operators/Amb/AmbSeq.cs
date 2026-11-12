// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information. 

namespace Tests.System.Reactive.Shared;

/// <summary>Describes <c>first.Amb(second)</c>.</summary>
/// <remarks>
/// Built by <see cref="Seq.Amb{T}(Seq{T}, Seq{T})"/>; materialized by each target through
/// <see cref="ISeqVisitor.Amb{T}(AmbSeq{T})"/>.
/// </remarks>
public sealed class AmbSeq<T>(Seq<T> first, Seq<T> second) : Seq<T>
{
    /// <summary>The first sequence.</summary>
    public Seq<T> First => first;

    /// <summary>The second sequence.</summary>
    public Seq<T> Second => second;

    /// <inheritdoc/>
    protected override Realized<Seq<T>> AcceptCore(ISeqVisitor visitor) => visitor.Amb(this);

    /// <inheritdoc/>
    public override string ToString() => $"{first}.Amb({second})";
}

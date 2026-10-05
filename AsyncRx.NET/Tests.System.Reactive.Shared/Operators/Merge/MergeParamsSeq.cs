// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information. 

namespace Tests.System.Reactive.Shared;

/// <summary>Describes <c>Seq.Merge(sources)</c> over an array of sequences.</summary>
/// <remarks>
/// Built by <see cref="Seq.Merge{T}(Seq{T}[])"/>; materialized by each target through
/// <see cref="ISeqVisitor.MergeParams{T}(MergeParamsSeq{T})"/>.
/// </remarks>
public sealed class MergeParamsSeq<T>(Seq<T>[] sources) : Seq<T>
{
    /// <summary>Observable sequences.</summary>
    public Seq<T>[] Sources => sources;

    /// <inheritdoc/>
    protected override Realized<Seq<T>> AcceptCore(ISeqVisitor visitor) => visitor.MergeParams(this);

    /// <inheritdoc/>
    public override string ToString() => $"Seq.Merge({string.Join(", ", sources.AsEnumerable())})";
}

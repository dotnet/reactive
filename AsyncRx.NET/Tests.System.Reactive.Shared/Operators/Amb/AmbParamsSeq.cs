// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information. 

namespace Tests.System.Reactive.Shared;

/// <summary>Describes <c>Seq.Amb(source1, source2, ...)</c>.</summary>
/// <remarks>
/// Built by <see cref="Seq.Amb{T}(Seq{T}[])"/>; materialized by each target through
/// <see cref="ISeqVisitor.AmbParams{T}(AmbParamsSeq{T})"/>.
/// </remarks>
public sealed class AmbParamsSeq<T>(Seq<T>[] sources) : Seq<T>
{
    /// <summary>The sources, in order.</summary>
    public Seq<T>[] Sources => sources;

    /// <inheritdoc/>
    protected override Realized<Seq<T>> AcceptCore(ISeqVisitor visitor) =>
        visitor.AmbParams(this);

    /// <inheritdoc/>
    public override string ToString() =>
        $"Seq.Amb({string.Join(", ", sources.Select(s => s.ToString()))})";
}

// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information. 

namespace Tests.System.Reactive.Shared;

/// <summary>
/// Describes <c>Seq.Concat(source1, source2, ...)</c> over any number of sources.
/// </summary>
/// <remarks>
/// Built by <see cref="Seq.Concat{T}(Seq{T}[])"/>, the <c>params</c> form; materialized by each
/// target through <see cref="ISeqVisitor.ConcatParams{T}(ConcatParamsSeq{T})"/>.
/// </remarks>
public sealed class ConcatParamsSeq<T>(Seq<T>[] sources) : Seq<T>
{
    /// <summary>Observable sequences to concatenate.</summary>
    public Seq<T>[] Sources => sources;

    /// <inheritdoc/>
    protected override Realized<Seq<T>> AcceptCore(ISeqVisitor visitor) =>
        visitor.ConcatParams(this);

    /// <inheritdoc/>
    public override string ToString() =>
        $"Seq.Concat({string.Join(", ", sources.Select(s => s.ToString()))})";
}

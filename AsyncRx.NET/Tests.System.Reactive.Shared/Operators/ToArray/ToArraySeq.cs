// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information. 

namespace Tests.System.Reactive.Shared;

/// <summary>Describes <c>source.ToArray()</c>.</summary>
/// <remarks>
/// Built by <see cref="Seq.ToArray{T}(Seq{T})"/>; materialized by each target through
/// <see cref="ISeqVisitor.ToArray{T}(ToArraySeq{T})"/>.
/// </remarks>
public sealed class ToArraySeq<T>(Seq<T> source) : Seq<T[]>
{
    /// <summary>The source observable sequence to get an array of elements for.</summary>
    public Seq<T> Source => source;

    /// <inheritdoc/>
    protected override Realized<Seq<T[]>> AcceptCore(ISeqVisitor visitor) =>
        visitor.ToArray(this);

    /// <inheritdoc/>
    public override string ToString() => $"{source}.ToArray()";
}

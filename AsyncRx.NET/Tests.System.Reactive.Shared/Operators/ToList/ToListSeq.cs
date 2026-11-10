// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information. 

namespace Tests.System.Reactive.Shared;

/// <summary>Describes <c>source.ToList()</c>.</summary>
/// <remarks>
/// Built by <see cref="Seq.ToList{T}(Seq{T})"/>; materialized by each target through
/// <see cref="ISeqVisitor.ToList{T}(ToListSeq{T})"/>.
/// </remarks>
public sealed class ToListSeq<T>(Seq<T> source) : Seq<IList<T>>
{
    /// <summary>The source observable sequence to get a list of elements for.</summary>
    public Seq<T> Source => source;

    /// <inheritdoc/>
    protected override Realized<Seq<IList<T>>> AcceptCore(ISeqVisitor visitor) =>
        visitor.ToList(this);

    /// <inheritdoc/>
    public override string ToString() => $"{source}.ToList()";
}

// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information. 

namespace Tests.System.Reactive.Shared;

/// <summary>Describes <c>source.Prepend(value)</c>.</summary>
/// <remarks>
/// Built by <see cref="Seq.Prepend{T}(Seq{T}, T)"/>; materialized by each target through
/// <see cref="ISeqVisitor.Prepend{T}(PrependSeq{T})"/>.
/// </remarks>
public sealed class PrependSeq<T>(Seq<T> source, T value) : Seq<T>
{
    /// <summary>Source sequence to prepend the value to.</summary>
    public Seq<T> Source => source;

    /// <summary>Value to prepend to the specified sequence.</summary>
    public T Value => value;

    /// <inheritdoc/>
    protected override Realized<Seq<T>> AcceptCore(ISeqVisitor visitor) => visitor.Prepend(this);

    /// <inheritdoc/>
    public override string ToString() => $"{source}.Prepend({value})";
}

// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information. 

namespace Tests.System.Reactive.Shared;

/// <summary>Describes <c>source.Repeat(repeatCount)</c>.</summary>
/// <remarks>
/// Built by <see cref="SeqExtensions.Repeat{T}(Seq{T}, int)"/>;
/// materialized by each target through
/// <see cref="ISeqVisitor.RepeatCount{T}(RepeatCountSeq{T})"/>.
/// </remarks>
public sealed class RepeatCountSeq<T>(Seq<T> source, int repeatCount) : Seq<T>
{
    /// <summary>Observable sequence to repeat.</summary>
    public Seq<T> Source => source;

    /// <summary>Number of times to repeat the sequence.</summary>
    public int RepeatCount => repeatCount;

    /// <inheritdoc/>
    protected override Realized<Seq<T>> AcceptCore(ISeqVisitor visitor) =>
        visitor.RepeatCount(this);

    /// <inheritdoc/>
    public override string ToString() => $"{source}.Repeat({repeatCount})";
}

// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information. 

namespace Tests.System.Reactive.Shared;

/// <summary>Describes <c>source.Repeat()</c>.</summary>
/// <remarks>
/// Built by <see cref="SeqExtensions.Repeat{T}(Seq{T})"/>;
/// materialized by each target through <see cref="ISeqVisitor.Repeat{T}(RepeatSeq{T})"/>.
/// </remarks>
public sealed class RepeatSeq<T>(Seq<T> source) : Seq<T>
{
    /// <summary>Observable sequence to repeat.</summary>
    public Seq<T> Source => source;

    /// <inheritdoc/>
    protected override Realized<Seq<T>> AcceptCore(ISeqVisitor visitor) => visitor.Repeat(this);

    /// <inheritdoc/>
    public override string ToString() => $"{source}.Repeat()";
}

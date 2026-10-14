// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information. 

namespace Tests.System.Reactive.Shared;

/// <summary>Describes <c>source.LongCount()</c>.</summary>
/// <remarks>
/// Built by <see cref="LongCountExtensions.LongCount{T}(Seq{T})"/>;
/// materialized by each target through <see cref="ISeqVisitor.LongCount{T}(LongCountSeq{T})"/>.
/// </remarks>
public sealed class LongCountSeq<T>(Seq<T> source) : Seq<long>
{
    /// <summary>An observable sequence that contains elements to be counted.</summary>
    public Seq<T> Source => source;

    /// <inheritdoc/>
    protected override Realized<Seq<long>> AcceptCore(ISeqVisitor visitor) =>
        visitor.LongCount(this);

    /// <inheritdoc/>
    public override string ToString() => $"{source}.LongCount()";
}

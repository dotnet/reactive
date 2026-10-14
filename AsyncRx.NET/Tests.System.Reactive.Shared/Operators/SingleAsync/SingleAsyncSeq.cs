// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information. 

namespace Tests.System.Reactive.Shared;

/// <summary>Describes <c>source.SingleAsync()</c>.</summary>
/// <remarks>
/// Built by <see cref="SingleAsyncExtensions.SingleAsync{T}(Seq{T})"/>;
/// materialized by each target through <see cref="ISeqVisitor.SingleAsync{T}(SingleAsyncSeq{T})"/>.
/// </remarks>
public sealed class SingleAsyncSeq<T>(Seq<T> source) : Seq<T>
{
    /// <summary>Source observable sequence.</summary>
    public Seq<T> Source => source;

    /// <inheritdoc/>
    protected override Realized<Seq<T>> AcceptCore(ISeqVisitor visitor) =>
        visitor.SingleAsync(this);

    /// <inheritdoc/>
    public override string ToString() => $"{source}.SingleAsync()";
}

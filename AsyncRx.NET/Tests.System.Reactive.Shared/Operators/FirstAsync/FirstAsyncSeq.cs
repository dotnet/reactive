// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information. 

namespace Tests.System.Reactive.Shared;

/// <summary>Describes <c>source.FirstAsync()</c>.</summary>
/// <remarks>
/// Built by <see cref="FirstAsyncExtensions.FirstAsync{T}(Seq{T})"/>;
/// materialized by each target through <see cref="ISeqVisitor.FirstAsync{T}(FirstAsyncSeq{T})"/>.
/// </remarks>
public sealed class FirstAsyncSeq<T>(Seq<T> source) : Seq<T>
{
    /// <summary>Source observable sequence.</summary>
    public Seq<T> Source => source;

    /// <inheritdoc/>
    protected override Realized<Seq<T>> AcceptCore(ISeqVisitor visitor) =>
        visitor.FirstAsync(this);

    /// <inheritdoc/>
    public override string ToString() => $"{source}.FirstAsync()";
}

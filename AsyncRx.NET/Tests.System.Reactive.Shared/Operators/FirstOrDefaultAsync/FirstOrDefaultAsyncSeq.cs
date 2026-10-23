// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information. 

namespace Tests.System.Reactive.Shared;

/// <summary>Describes <c>source.FirstOrDefaultAsync()</c>.</summary>
/// <remarks>
/// Built by <see cref="FirstOrDefaultAsyncExtensions.FirstOrDefaultAsync{T}(Seq{T})"/>;
/// materialized by each target through <see cref="ISeqVisitor.FirstOrDefaultAsync{T}(FirstOrDefaultAsyncSeq{T})"/>.
/// </remarks>
public sealed class FirstOrDefaultAsyncSeq<T>(Seq<T> source) : Seq<T>
{
    /// <summary>Source observable sequence.</summary>
    public Seq<T> Source => source;

    /// <inheritdoc/>
    protected override Realized<Seq<T>> AcceptCore(ISeqVisitor visitor) =>
        visitor.FirstOrDefaultAsync(this);

    /// <inheritdoc/>
    public override string ToString() => $"{source}.FirstOrDefaultAsync()";
}

// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information. 

namespace Tests.System.Reactive.Shared;

/// <summary>Describes <c>source.IsEmpty()</c>.</summary>
/// <remarks>
/// Built by <see cref="IsEmptyExtensions.IsEmpty{T}(Seq{T})"/>;
/// materialized by each target through <see cref="ISeqVisitor.IsEmpty{T}(IsEmptySeq{T})"/>.
/// </remarks>
public sealed class IsEmptySeq<T>(Seq<T> source)
    : Seq<bool>
{
    /// <summary>An observable sequence to check for emptiness.</summary>
    public Seq<T> Source => source;

    /// <inheritdoc/>
    protected override Realized<Seq<bool>> AcceptCore(ISeqVisitor visitor) =>
        visitor.IsEmpty(this);

    /// <inheritdoc/>
    public override string ToString() => $"{source}.IsEmpty()";
}

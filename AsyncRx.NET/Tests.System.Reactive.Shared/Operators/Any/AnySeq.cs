// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information. 

namespace Tests.System.Reactive.Shared;

/// <summary>Describes <c>source.Any()</c>.</summary>
/// <remarks>
/// Built by <see cref="AnyExtensions.Any{T}(Seq{T})"/>;
/// materialized by each target through <see cref="ISeqVisitor.Any{T}(AnySeq{T})"/>.
/// </remarks>
public sealed class AnySeq<T>(Seq<T> source)
    : Seq<bool>
{
    /// <summary>An observable sequence to check for non-emptiness.</summary>
    public Seq<T> Source => source;

    /// <inheritdoc/>
    protected override Realized<Seq<bool>> AcceptCore(ISeqVisitor visitor) =>
        visitor.Any(this);

    /// <inheritdoc/>
    public override string ToString() => $"{source}.Any()";
}

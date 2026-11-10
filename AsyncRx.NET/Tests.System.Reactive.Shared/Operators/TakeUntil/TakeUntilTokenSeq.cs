// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information. 

namespace Tests.System.Reactive.Shared;

/// <summary>Describes <c>source.TakeUntil(cancellationToken)</c>.</summary>
/// <remarks>
/// Built by <see cref="TakeUntilExtensions.TakeUntil{T}(Seq{T}, CancellationToken)"/>;
/// materialized by each target through
/// <see cref="ISeqVisitor.TakeUntilToken{T}(TakeUntilTokenSeq{T})"/>.
/// </remarks>
public sealed class TakeUntilTokenSeq<T>(
    Seq<T> source,
    CancellationToken cancellationToken)
    : Seq<T>
{
    /// <summary>The source sequence to relay elements of.</summary>
    public Seq<T> Source => source;

    /// <summary>The cancellation token to complete the target observable sequence on.</summary>
    public CancellationToken CancellationToken => cancellationToken;

    /// <inheritdoc/>
    protected override Realized<Seq<T>> AcceptCore(ISeqVisitor visitor) =>
        visitor.TakeUntilToken(this);

    /// <inheritdoc/>
    public override string ToString() => $"{source}.TakeUntil(cancellationToken)";
}

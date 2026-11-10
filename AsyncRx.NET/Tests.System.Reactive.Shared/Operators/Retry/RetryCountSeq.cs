// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information. 

namespace Tests.System.Reactive.Shared;

/// <summary>Describes <c>source.Retry(retryCount)</c>.</summary>
/// <remarks>
/// Built by <see cref="RetryExtensions.Retry{T}(Seq{T}, int)"/>;
/// materialized by each target through <see cref="ISeqVisitor.RetryCount{T}(RetryCountSeq{T})"/>.
/// </remarks>
public sealed class RetryCountSeq<T>(Seq<T> source, int retryCount) : Seq<T>
{
    /// <summary>Observable sequence to repeat until it successfully terminates.</summary>
    public Seq<T> Source => source;

    /// <summary>Number of times to repeat the sequence.</summary>
    public int RetryCount => retryCount;

    /// <inheritdoc/>
    protected override Realized<Seq<T>> AcceptCore(ISeqVisitor visitor) => visitor.RetryCount(this);

    /// <inheritdoc/>
    public override string ToString() => $"{source}.Retry({retryCount})";
}

// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information. 

namespace Tests.System.Reactive.Shared;

/// <summary>Describes <c>source.Retry()</c>.</summary>
/// <remarks>
/// Built by <see cref="RetryExtensions.Retry{T}(Seq{T})"/>;
/// materialized by each target through <see cref="ISeqVisitor.Retry{T}(RetrySeq{T})"/>.
/// </remarks>
public sealed class RetrySeq<T>(Seq<T> source) : Seq<T>
{
    /// <summary>Observable sequence to repeat until it successfully terminates.</summary>
    public Seq<T> Source => source;

    /// <inheritdoc/>
    protected override Realized<Seq<T>> AcceptCore(ISeqVisitor visitor) => visitor.Retry(this);

    /// <inheritdoc/>
    public override string ToString() => $"{source}.Retry()";
}

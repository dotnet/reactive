// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information. 

namespace Tests.System.Reactive.Shared;

/// <summary>Describes <c>source.PublishLast()</c>.</summary>
/// <remarks>
/// Built by <see cref="PublishLastExtensions.PublishLast{T}(Seq{T})"/>;
/// materialized by each target through
/// <see cref="ISeqVisitor.PublishLast{T}(PublishLastSeq{T})"/>.
/// </remarks>
public sealed class PublishLastSeq<T>(Seq<T> source) : ConnectableSeq<T>
{
    /// <summary>
    /// Source sequence whose elements will be multicasted through a single shared subscription.
    /// </summary>
    public Seq<T> Source => source;

    /// <inheritdoc/>
    protected override Realized<Seq<T>> AcceptCore(ISeqVisitor visitor) =>
        visitor.PublishLast(this);

    /// <inheritdoc/>
    public override string ToString() => $"{source}.PublishLast()";
}

// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information. 

namespace Tests.System.Reactive.Shared;

/// <summary>Describes <c>source.Publish(initialValue)</c>.</summary>
/// <remarks>
/// Built by <see cref="Seq.Publish{T}(Seq{T}, T)"/>;
/// materialized by each target through
/// <see cref="ISeqVisitor.PublishInitial{T}(PublishInitialSeq{T})"/>.
/// </remarks>
public sealed class PublishInitialSeq<T>(Seq<T> source, T initialValue) : ConnectableSeq<T>
{
    /// <summary>
    /// Source sequence whose elements will be multicasted through a single shared subscription.
    /// </summary>
    public Seq<T> Source => source;

    /// <summary>Initial value received by observers upon subscription.</summary>
    public T InitialValue => initialValue;

    /// <inheritdoc/>
    protected override Realized<Seq<T>> AcceptCore(ISeqVisitor visitor) => visitor.PublishInitial(this);

    /// <inheritdoc/>
    public override string ToString() => $"{source}.Publish({initialValue})";
}

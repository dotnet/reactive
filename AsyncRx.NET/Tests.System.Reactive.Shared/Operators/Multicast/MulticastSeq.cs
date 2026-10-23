// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information. 

namespace Tests.System.Reactive.Shared;

/// <summary>Describes <c>source.Multicast(subject)</c>.</summary>
/// <remarks>
/// Built by <see cref="MulticastExtensions.Multicast{T}(Seq{T}, SubjectSeq{T})"/>; materialized by
/// each target through <see cref="ISeqVisitor.Multicast{T}(MulticastSeq{T})"/>. The result is a
/// connectable: subscribing attaches to the subject, and connecting subscribes the subject to
/// the source.
/// </remarks>
public sealed class MulticastSeq<T>(Seq<T> source, SubjectSeq<T> subject) : ConnectableSeq<T>
{
    /// <summary>
    /// Source sequence whose elements will be pushed into the specified subject.
    /// </summary>
    public Seq<T> Source => source;

    /// <summary>Subject to push source elements into.</summary>
    public SubjectSeq<T> Subject => subject;

    /// <inheritdoc/>
    protected override Realized<Seq<T>> AcceptCore(ISeqVisitor visitor) => visitor.Multicast(this);

    /// <inheritdoc/>
    public override string ToString() => $"{source}.Multicast({subject})";
}

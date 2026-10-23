// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information. 

namespace Tests.System.Reactive.Shared;

/// <summary>Describes <c>source.RefCount()</c>.</summary>
/// <remarks>
/// Built by <see cref="RefCountExtensions.RefCount{T}(ConnectableSeq{T})"/>; materialized by
/// each target through <see cref="ISeqVisitor.RefCount{T}(RefCountSeq{T})"/>.
/// </remarks>
public sealed class RefCountSeq<T>(ConnectableSeq<T> source) : Seq<T>
{
    /// <summary>Connectable observable sequence.</summary>
    public ConnectableSeq<T> Source => source;

    /// <inheritdoc/>
    protected override Realized<Seq<T>> AcceptCore(ISeqVisitor visitor) => visitor.RefCount(this);

    /// <inheritdoc/>
    public override string ToString() => $"{source}.RefCount()";
}

// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information. 

namespace Tests.System.Reactive.Shared;

/// <summary>Describes <c>source.RefCount(minObservers)</c>.</summary>
/// <remarks>
/// Built by <see cref="RefCountExtensions.RefCount{T}(ConnectableSeq{T}, int)"/>; materialized by
/// each target through
/// <see cref="ISeqVisitor.RefCountMinObservers{T}(RefCountMinObserversSeq{T})"/>.
/// </remarks>
public sealed class RefCountMinObserversSeq<T>(ConnectableSeq<T> source, int minObservers) : Seq<T>
{
    /// <summary>Connectable observable sequence.</summary>
    public ConnectableSeq<T> Source => source;

    /// <summary>The minimum number of observers required to connect.</summary>
    public int MinObservers => minObservers;

    /// <inheritdoc/>
    protected override Realized<Seq<T>> AcceptCore(ISeqVisitor visitor) =>
        visitor.RefCountMinObservers(this);

    /// <inheritdoc/>
    public override string ToString() => $"{source}.RefCount({minObservers})";
}

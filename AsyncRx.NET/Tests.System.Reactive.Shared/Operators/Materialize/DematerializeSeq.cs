// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information. 

using System.Reactive;

namespace Tests.System.Reactive.Shared;

/// <summary>Describes <c>source.Dematerialize()</c>.</summary>
/// <remarks>
/// Built by <see cref="MaterializeExtensions.Dematerialize{T}(Seq{Notification{T}})"/>;
/// materialized by each target through
/// <see cref="ISeqVisitor.Dematerialize{T}(DematerializeSeq{T})"/>.
/// </remarks>
public sealed class DematerializeSeq<T>(Seq<Notification<T>> source) : Seq<T>
{
    /// <summary>
    /// An observable sequence containing explicit notification values which have to be turned
    /// into implicit notifications.
    /// </summary>
    public Seq<Notification<T>> Source => source;

    /// <inheritdoc/>
    protected override Realized<Seq<T>> AcceptCore(ISeqVisitor visitor) =>
        visitor.Dematerialize(this);

    /// <inheritdoc/>
    public override string ToString() => $"{source}.Dematerialize()";
}

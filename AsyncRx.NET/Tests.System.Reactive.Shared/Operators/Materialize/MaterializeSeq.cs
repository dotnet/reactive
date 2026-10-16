// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information. 

using System.Reactive;

namespace Tests.System.Reactive.Shared;

/// <summary>Describes <c>source.Materialize()</c>.</summary>
/// <remarks>
/// Built by <see cref="MaterializeExtensions.Materialize{T}(Seq{T})"/>; materialized by each
/// target through <see cref="ISeqVisitor.Materialize{T}(MaterializeSeq{T})"/>. The elements are
/// Rx.NET's <see cref="Notification{T}"/>, which AsyncRx.NET uses too, so both targets' results
/// compare as they are.
/// </remarks>
public sealed class MaterializeSeq<T>(Seq<T> source) : Seq<Notification<T>>
{
    /// <summary>An observable sequence to get notification values for.</summary>
    public Seq<T> Source => source;

    /// <inheritdoc/>
    protected override Realized<Seq<Notification<T>>> AcceptCore(ISeqVisitor visitor) =>
        visitor.Materialize(this);

    /// <inheritdoc/>
    public override string ToString() => $"{source}.Materialize()";
}

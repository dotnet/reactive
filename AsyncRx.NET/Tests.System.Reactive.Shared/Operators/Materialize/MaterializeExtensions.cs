// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information. 

using System.Reactive;

namespace Tests.System.Reactive.Shared;

/// <summary>
/// <c>Materialize</c> and <c>Dematerialize</c>, as extension methods on <see cref="Seq{T}"/>.
/// </summary>
/// <remarks>
/// These exist so that a shared scenario writes <c>xs.Materialize()</c> or
/// <c>xs.Dematerialize()</c> exactly as the Rx.NET test it was migrated from does. The two are
/// one folder because they are each other's inverse and share their tests. Each method builds
/// one node of the query description, which each target turns into its own call through the
/// matching <see cref="ISeqVisitor"/> member in <c>ISeqVisitor.Materialize.cs</c>.
/// </remarks>
public static class MaterializeExtensions
{
    /// <summary>Describes <c>source.Materialize()</c>.</summary>
    /// <typeparam name="T">The type of the elements in the source sequence.</typeparam>
    /// <param name="source">An observable sequence to get notification values for.</param>
    /// <remarks>
    /// Builds a <see cref="MaterializeSeq{T}"/>, which each target materializes through
    /// <see cref="ISeqVisitor.Materialize{T}(MaterializeSeq{T})"/>.
    /// </remarks>
    public static Seq<Notification<T>> Materialize<T>(this Seq<T> source) =>
        new MaterializeSeq<T>(source);

    /// <summary>Describes <c>source.Dematerialize()</c>.</summary>
    /// <typeparam name="T">
    /// The type of the elements materialized in the source sequence notification objects.
    /// </typeparam>
    /// <param name="source">
    /// An observable sequence containing explicit notification values which have to be turned
    /// into implicit notifications.
    /// </param>
    /// <remarks>
    /// Builds a <see cref="DematerializeSeq{T}"/>, which each target materializes through
    /// <see cref="ISeqVisitor.Dematerialize{T}(DematerializeSeq{T})"/>.
    /// </remarks>
    public static Seq<T> Dematerialize<T>(this Seq<Notification<T>> source) =>
        new DematerializeSeq<T>(source);
}

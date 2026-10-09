// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information. 

namespace Tests.System.Reactive.Shared;

/// <summary>The <c>ObserveOn</c> overloads, as extension methods on <see cref="Seq{T}"/>.</summary>
/// <remarks>
/// Over a scheduler or a <see cref="SynchronizationContext"/>. Each method builds one node of the
/// query description (one node type per overload, in this folder), and each target turns that
/// node into its own <c>ObserveOn</c> call through the matching <see cref="ISeqVisitor"/> member
/// in <c>ISeqVisitor.ObserveOn.cs</c>.
/// </remarks>
public static class ObserveOnExtensions
{
    /// <summary>Describes <c>source.ObserveOn(scheduler)</c>.</summary>
    /// <typeparam name="T">The type of the elements in the source sequence.</typeparam>
    /// <param name="source">Source sequence.</param>
    /// <param name="scheduler">Scheduler to notify observers on.</param>
    /// <remarks>
    /// Builds an <see cref="ObserveOnSeq{T}"/>, which each target materializes through
    /// <see cref="ISeqVisitor.ObserveOn{T}(ObserveOnSeq{T})"/>.
    /// </remarks>
    public static Seq<T> ObserveOn<T>(this Seq<T> source, SchedulerRef scheduler) =>
        new ObserveOnSeq<T>(source, scheduler);

    /// <summary>Describes <c>source.ObserveOn(context)</c>.</summary>
    /// <typeparam name="T">The type of the elements in the source sequence.</typeparam>
    /// <param name="source">Source sequence.</param>
    /// <param name="context">Synchronization context to notify observers on.</param>
    /// <remarks>
    /// Builds an <see cref="ObserveOnContextSeq{T}"/>, which each target materializes through
    /// <see cref="ISeqVisitor.ObserveOnContext{T}(ObserveOnContextSeq{T})"/>.
    /// </remarks>
    public static Seq<T> ObserveOn<T>(this Seq<T> source, SynchronizationContext context) =>
        new ObserveOnContextSeq<T>(source, context);
}

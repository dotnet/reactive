// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information. 

namespace Tests.System.Reactive.Shared;

/// <summary>
/// The <c>SubscribeOn</c> overloads, as extension methods on <see cref="Seq{T}"/>.
/// </summary>
/// <remarks>
/// Over a scheduler or a <see cref="SynchronizationContext"/>. Each method builds one node of the
/// query description (one node type per overload, in this folder), and each target turns that
/// node into its own <c>SubscribeOn</c> call through the matching <see cref="ISeqVisitor"/>
/// member in <c>ISeqVisitor.SubscribeOn.cs</c>.
/// </remarks>
public static class SubscribeOnExtensions
{
    /// <summary>Describes <c>source.SubscribeOn(scheduler)</c>.</summary>
    /// <typeparam name="T">The type of the elements in the source sequence.</typeparam>
    /// <param name="source">Source sequence.</param>
    /// <param name="scheduler">
    /// Scheduler to perform subscription and unsubscription actions on.
    /// </param>
    /// <remarks>
    /// Builds a <see cref="SubscribeOnSeq{T}"/>, which each target materializes through
    /// <see cref="ISeqVisitor.SubscribeOn{T}(SubscribeOnSeq{T})"/>.
    /// </remarks>
    public static Seq<T> SubscribeOn<T>(this Seq<T> source, SchedulerRef scheduler) =>
        new SubscribeOnSeq<T>(source, scheduler);

    /// <summary>Describes <c>source.SubscribeOn(context)</c>.</summary>
    /// <typeparam name="T">The type of the elements in the source sequence.</typeparam>
    /// <param name="source">Source sequence.</param>
    /// <param name="context">
    /// Synchronization context to perform subscription and unsubscription actions on.
    /// </param>
    /// <remarks>
    /// Builds a <see cref="SubscribeOnContextSeq{T}"/>, which each target materializes through
    /// <see cref="ISeqVisitor.SubscribeOnContext{T}(SubscribeOnContextSeq{T})"/>.
    /// </remarks>
    public static Seq<T> SubscribeOn<T>(this Seq<T> source, SynchronizationContext context) =>
        new SubscribeOnContextSeq<T>(source, context);
}

// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information. 

namespace Tests.System.Reactive.Shared;

/// <summary>
/// The <c>RefCount</c> overloads, as extension methods on <see cref="ConnectableSeq{T}"/>.
/// </summary>
/// <remarks>
/// The first operator whose input is a connectable: the bridge maps a
/// <see cref="ConnectableSeq{T}"/> argument to the target's own connectable type, as it maps a
/// <see cref="Seq{T}"/> to its observable type. Each method builds one node of the query
/// description (one node type per overload, in this folder), and each target turns that node
/// into its own <c>RefCount</c> call through the matching <see cref="ISeqVisitor"/> member in
/// <c>ISeqVisitor.RefCount.cs</c>.
/// </remarks>
public static class RefCountExtensions
{
    /// <summary>Describes <c>source.RefCount()</c>.</summary>
    /// <typeparam name="T">The type of the elements in the source sequence.</typeparam>
    /// <param name="source">Connectable observable sequence.</param>
    /// <remarks>
    /// Builds a <see cref="RefCountSeq{T}"/>, which each target materializes through
    /// <see cref="ISeqVisitor.RefCount{T}(RefCountSeq{T})"/>.
    /// </remarks>
    public static Seq<T> RefCount<T>(this ConnectableSeq<T> source) => new RefCountSeq<T>(source);

    /// <summary>Describes <c>source.RefCount(minObservers)</c>.</summary>
    /// <typeparam name="T">The type of the elements in the source sequence.</typeparam>
    /// <param name="source">Connectable observable sequence.</param>
    /// <param name="minObservers">The minimum number of observers required to connect.</param>
    /// <remarks>
    /// Builds a <see cref="RefCountMinObserversSeq{T}"/>, which each target materializes through
    /// <see cref="ISeqVisitor.RefCountMinObservers{T}(RefCountMinObserversSeq{T})"/>.
    /// </remarks>
    public static Seq<T> RefCount<T>(this ConnectableSeq<T> source, int minObservers) =>
        new RefCountMinObserversSeq<T>(source, minObservers);

    /// <summary>Describes <c>source.RefCount(disconnectDelay)</c>.</summary>
    /// <typeparam name="T">The type of the elements in the source sequence.</typeparam>
    /// <param name="source">Connectable observable sequence.</param>
    /// <param name="disconnectDelay">
    /// The time to wait before disconnecting after all observers have unsubscribed.
    /// </param>
    /// <remarks>
    /// Builds a <see cref="RefCountDelaySeq{T}"/>, which each target materializes through
    /// <see cref="ISeqVisitor.RefCountDelay{T}(RefCountDelaySeq{T})"/>.
    /// </remarks>
    public static Seq<T> RefCount<T>(this ConnectableSeq<T> source, TimeSpan disconnectDelay) =>
        new RefCountDelaySeq<T>(source, disconnectDelay);

    /// <summary>Describes <c>source.RefCount(disconnectDelay, scheduler)</c>.</summary>
    /// <typeparam name="T">The type of the elements in the source sequence.</typeparam>
    /// <param name="source">Connectable observable sequence.</param>
    /// <param name="disconnectDelay">
    /// The time to wait before disconnecting after all observers have unsubscribed.
    /// </param>
    /// <param name="scheduler">The scheduler to use for delayed unsubscription.</param>
    /// <remarks>
    /// Builds a <see cref="RefCountDelayScheduledSeq{T}"/>, which each target materializes
    /// through <see cref="ISeqVisitor.RefCountDelayScheduled{T}(RefCountDelayScheduledSeq{T})"/>.
    /// </remarks>
    public static Seq<T> RefCount<T>(
        this ConnectableSeq<T> source,
        TimeSpan disconnectDelay,
        SchedulerRef scheduler) =>
        new RefCountDelayScheduledSeq<T>(source, disconnectDelay, scheduler);

    /// <summary>Describes <c>source.RefCount(minObservers, disconnectDelay)</c>.</summary>
    /// <typeparam name="T">The type of the elements in the source sequence.</typeparam>
    /// <param name="source">Connectable observable sequence.</param>
    /// <param name="minObservers">The minimum number of observers required to connect.</param>
    /// <param name="disconnectDelay">
    /// The time to wait before disconnecting after all observers have unsubscribed.
    /// </param>
    /// <remarks>
    /// Builds a <see cref="RefCountMinObserversDelaySeq{T}"/>, which each target materializes
    /// through
    /// <see cref="ISeqVisitor.RefCountMinObserversDelay{T}(RefCountMinObserversDelaySeq{T})"/>.
    /// </remarks>
    public static Seq<T> RefCount<T>(
        this ConnectableSeq<T> source,
        int minObservers,
        TimeSpan disconnectDelay) =>
        new RefCountMinObserversDelaySeq<T>(source, minObservers, disconnectDelay);

    /// <summary>
    /// Describes <c>source.RefCount(minObservers, disconnectDelay, scheduler)</c>.
    /// </summary>
    /// <typeparam name="T">The type of the elements in the source sequence.</typeparam>
    /// <param name="source">Connectable observable sequence.</param>
    /// <param name="minObservers">The minimum number of observers required to connect.</param>
    /// <param name="disconnectDelay">
    /// The time to wait before disconnecting after all observers have unsubscribed.
    /// </param>
    /// <param name="scheduler">The scheduler to use for delayed unsubscription.</param>
    /// <remarks>
    /// Builds a <see cref="RefCountMinObserversDelayScheduledSeq{T}"/>, which each target
    /// materializes through
    /// <see cref="ISeqVisitor.RefCountMinObserversDelayScheduled{T}(RefCountMinObserversDelayScheduledSeq{T})"/>.
    /// </remarks>
    public static Seq<T> RefCount<T>(
        this ConnectableSeq<T> source,
        int minObservers,
        TimeSpan disconnectDelay,
        SchedulerRef scheduler) =>
        new RefCountMinObserversDelayScheduledSeq<T>(source, minObservers, disconnectDelay, scheduler);
}

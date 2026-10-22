// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information. 

namespace Tests.System.Reactive.Shared;

/// <summary>The <c>DelaySubscription</c> overloads, as extension methods on <see cref="Seq"/>.</summary>
/// <remarks>
/// Where an Rx.NET test writes <c>xs.DelaySubscription(TimeSpan.FromTicks(30), scheduler)</c>,
/// a shared scenario writes the same over a <see cref="Seq{T}"/>; the overload set is Rx.NET's
/// (a relative or an absolute due time, each with or without a scheduler), so the same call
/// resolves to the same overload on both sides. One node per due-time kind, in this folder,
/// carrying the scheduler or null for the target's default, each materialized by a target
/// through the matching <see cref="ISeqVisitor"/> member in
/// <c>ISeqVisitor.DelaySubscription.cs</c>. The <c>Delay</c> overload that takes a
/// subscription-delay sequence is a different operator, described by
/// <see cref="DelaySubscriptionDelaySelectorSeq{T, TDelay}"/> in the <c>Delay</c> folder.
/// </remarks>
public static partial class Seq
{
    /// <summary>
    /// Describes <c>source.DelaySubscription(dueTime, scheduler)</c> with a relative due time.
    /// </summary>
    /// <typeparam name="T">The type of the elements in the source sequence.</typeparam>
    /// <param name="source">Source sequence to delay subscription for.</param>
    /// <param name="dueTime">Relative time shift of the subscribe operation.</param>
    /// <param name="scheduler">Scheduler to run the subscription delay timer on.</param>
    /// <remarks>
    /// Builds a <see cref="DelaySubscriptionTimeSeq{T}"/>, which each target materializes through
    /// <see cref="ISeqVisitor.DelaySubscriptionTime{T}(DelaySubscriptionTimeSeq{T})"/>.
    /// </remarks>
    public static Seq<T> DelaySubscription<T>(
        this Seq<T> source,
        TimeSpan dueTime,
        SchedulerRef scheduler) =>
        new DelaySubscriptionTimeSeq<T>(source, dueTime, scheduler);

    /// <summary>
    /// Describes <c>source.DelaySubscription(dueTime)</c> with a relative due time, on the
    /// target's default scheduler.
    /// </summary>
    /// <typeparam name="T">The type of the elements in the source sequence.</typeparam>
    /// <param name="source">Source sequence to delay subscription for.</param>
    /// <param name="dueTime">Relative time shift of the subscribe operation.</param>
    /// <remarks>
    /// Builds a <see cref="DelaySubscriptionTimeSeq{T}"/> with no scheduler, which each target
    /// materializes through
    /// <see cref="ISeqVisitor.DelaySubscriptionTime{T}(DelaySubscriptionTimeSeq{T})"/>.
    /// </remarks>
    public static Seq<T> DelaySubscription<T>(this Seq<T> source, TimeSpan dueTime) =>
        new DelaySubscriptionTimeSeq<T>(source, dueTime, null);

    /// <summary>
    /// Describes <c>source.DelaySubscription(dueTime, scheduler)</c> with an absolute due time.
    /// </summary>
    /// <typeparam name="T">The type of the elements in the source sequence.</typeparam>
    /// <param name="source">Source sequence to delay subscription for.</param>
    /// <param name="dueTime">Absolute time to perform the subscription at.</param>
    /// <param name="scheduler">Scheduler to run the subscription delay timer on.</param>
    /// <remarks>
    /// Builds a <see cref="DelaySubscriptionAbsoluteSeq{T}"/>, which each target materializes through
    /// <see cref="ISeqVisitor.DelaySubscriptionAbsolute{T}(DelaySubscriptionAbsoluteSeq{T})"/>.
    /// </remarks>
    public static Seq<T> DelaySubscription<T>(
        this Seq<T> source,
        DateTimeOffset dueTime,
        SchedulerRef scheduler) =>
        new DelaySubscriptionAbsoluteSeq<T>(source, dueTime, scheduler);

    /// <summary>
    /// Describes <c>source.DelaySubscription(dueTime)</c> with an absolute due time, on the
    /// target's default scheduler.
    /// </summary>
    /// <typeparam name="T">The type of the elements in the source sequence.</typeparam>
    /// <param name="source">Source sequence to delay subscription for.</param>
    /// <param name="dueTime">Absolute time to perform the subscription at.</param>
    /// <remarks>
    /// Builds a <see cref="DelaySubscriptionAbsoluteSeq{T}"/> with no scheduler, which each target
    /// materializes through
    /// <see cref="ISeqVisitor.DelaySubscriptionAbsolute{T}(DelaySubscriptionAbsoluteSeq{T})"/>.
    /// </remarks>
    public static Seq<T> DelaySubscription<T>(this Seq<T> source, DateTimeOffset dueTime) =>
        new DelaySubscriptionAbsoluteSeq<T>(source, dueTime, null);
}

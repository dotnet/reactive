// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information. 

using System.Runtime.CompilerServices;

namespace Tests.System.Reactive.Shared;

/// <summary>The <c>Throttle</c> overloads, as extension methods on <see cref="Seq{T}"/>.</summary>
/// <remarks>
/// These exist so that a shared scenario writes <c>xs.Throttle(...)</c> exactly as the Rx.NET
/// test it was migrated from does: the time-based form, with and without the scheduler, and the
/// selector form. Each method builds one node of the query description (one node type per
/// overload, in this folder), and each target turns that node into its own <c>Throttle</c>
/// call through the matching <see cref="ISeqVisitor"/> member in
/// <c>ISeqVisitor.Throttle.cs</c>.
/// </remarks>
public static class ThrottleExtensions
{
    /// <summary>Describes <c>source.Throttle(dueTime, scheduler)</c>.</summary>
    /// <typeparam name="T">The type of the elements in the source sequence.</typeparam>
    /// <param name="source">Source sequence to throttle.</param>
    /// <param name="dueTime">Throttling duration for each element.</param>
    /// <param name="scheduler">Scheduler to run the throttle timers on.</param>
    /// <remarks>
    /// Builds a <see cref="ThrottleTimeSeq{T}"/>, which each target materializes through
    /// <see cref="ISeqVisitor.ThrottleTime{T}(ThrottleTimeSeq{T})"/>.
    /// </remarks>
    public static Seq<T> Throttle<T>(
        this Seq<T> source,
        TimeSpan dueTime,
        SchedulerRef scheduler) =>
        new ThrottleTimeSeq<T>(source, dueTime, scheduler);

    /// <summary>
    /// Describes <c>source.Throttle(dueTime)</c>, on the target's default scheduler.
    /// </summary>
    /// <typeparam name="T">The type of the elements in the source sequence.</typeparam>
    /// <param name="source">Source sequence to throttle.</param>
    /// <param name="dueTime">Throttling duration for each element.</param>
    /// <remarks>
    /// Builds a <see cref="ThrottleTimeSeq{T}"/> with no scheduler, which each target
    /// materializes through <see cref="ISeqVisitor.ThrottleTime{T}(ThrottleTimeSeq{T})"/>.
    /// </remarks>
    public static Seq<T> Throttle<T>(this Seq<T> source, TimeSpan dueTime) =>
        new ThrottleTimeSeq<T>(source, dueTime, null);

    /// <summary>Describes <c>source.Throttle(throttleDurationSelector)</c>.</summary>
    /// <typeparam name="T">The type of the elements in the source sequence.</typeparam>
    /// <typeparam name="TThrottle">
    /// The type of the elements in the throttle sequences selected for each element in the
    /// source sequence.
    /// </typeparam>
    /// <param name="source">Source sequence to throttle.</param>
    /// <param name="throttleDurationSelector">
    /// Selector function to retrieve a sequence indicating the throttle duration for each given
    /// element.
    /// </param>
    /// <param name="text">
    /// Supplied by the compiler (the source text of the selector), for printing the query in
    /// diagnostics; do not pass it.
    /// </param>
    /// <remarks>
    /// Builds a <see cref="ThrottleSelectorSeq{T, TThrottle}"/>, which each target materializes
    /// through
    /// <see cref="ISeqVisitor.ThrottleSelector{T, TThrottle}(ThrottleSelectorSeq{T, TThrottle})"/>.
    /// </remarks>
    public static Seq<T> Throttle<T, TThrottle>(
        this Seq<T> source,
        Func<T, Seq<TThrottle>> throttleDurationSelector,
        [CallerArgumentExpression(nameof(throttleDurationSelector))] string text = "") =>
        new ThrottleSelectorSeq<T, TThrottle>(source, throttleDurationSelector, text);
}

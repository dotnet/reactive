// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information. 

using System.Runtime.CompilerServices;

namespace Tests.System.Reactive.Shared;

/// <summary>The <c>Timeout</c> overloads, as extension methods on <see cref="Seq{T}"/>.</summary>
/// <remarks>
/// These exist so that a shared scenario writes <c>xs.Timeout(...)</c> exactly as the Rx.NET test
/// it was migrated from does: the relative and absolute time-based forms, each with and without a
/// fallback sequence and with and without the scheduler, and the four duration-selector forms.
/// The scheduler-less time-based forms build the same node as their scheduled twins with no
/// scheduler, as <c>Delay</c>'s do; otherwise one node type per overload, in this folder, each
/// turned by a target into its own <c>Timeout</c> call through the matching
/// <see cref="ISeqVisitor"/> member in <c>ISeqVisitor.Timeout.cs</c>.
/// </remarks>
public static class TimeoutExtensions
{
    /// <summary>
    /// Describes <c>source.Timeout(dueTime, scheduler)</c> with a relative due time.
    /// </summary>
    /// <typeparam name="T">The type of the elements in the source sequence.</typeparam>
    /// <param name="source">Source sequence to perform a timeout for.</param>
    /// <param name="dueTime">Maximum duration between values before a timeout occurs.</param>
    /// <param name="scheduler">Scheduler to run the timeout timers on.</param>
    /// <remarks>
    /// Builds a <see cref="TimeoutTimeSeq{T}"/>, which each target materializes through
    /// <see cref="ISeqVisitor.TimeoutTime{T}(TimeoutTimeSeq{T})"/>.
    /// </remarks>
    public static Seq<T> Timeout<T>(this Seq<T> source, TimeSpan dueTime, SchedulerRef scheduler) =>
        new TimeoutTimeSeq<T>(source, dueTime, scheduler);

    /// <summary>
    /// Describes <c>source.Timeout(dueTime)</c> with a relative due time, on the target's default
    /// scheduler.
    /// </summary>
    /// <typeparam name="T">The type of the elements in the source sequence.</typeparam>
    /// <param name="source">Source sequence to perform a timeout for.</param>
    /// <param name="dueTime">Maximum duration between values before a timeout occurs.</param>
    /// <remarks>
    /// Builds a <see cref="TimeoutTimeSeq{T}"/> with no scheduler, which each target materializes
    /// through <see cref="ISeqVisitor.TimeoutTime{T}(TimeoutTimeSeq{T})"/>.
    /// </remarks>
    public static Seq<T> Timeout<T>(this Seq<T> source, TimeSpan dueTime) =>
        new TimeoutTimeSeq<T>(source, dueTime, null);

    /// <summary>
    /// Describes <c>source.Timeout(dueTime, other, scheduler)</c> with a relative due time.
    /// </summary>
    /// <typeparam name="T">The type of the elements in the source sequence.</typeparam>
    /// <param name="source">Source sequence to perform a timeout for.</param>
    /// <param name="dueTime">Maximum duration between values before a timeout occurs.</param>
    /// <param name="other">Sequence to return in case of a timeout.</param>
    /// <param name="scheduler">Scheduler to run the timeout timers on.</param>
    /// <remarks>
    /// Builds a <see cref="TimeoutTimeOtherSeq{T}"/>, which each target materializes through
    /// <see cref="ISeqVisitor.TimeoutTimeOther{T}(TimeoutTimeOtherSeq{T})"/>.
    /// </remarks>
    public static Seq<T> Timeout<T>(
        this Seq<T> source,
        TimeSpan dueTime,
        Seq<T> other,
        SchedulerRef scheduler) =>
        new TimeoutTimeOtherSeq<T>(source, dueTime, other, scheduler);

    /// <summary>
    /// Describes <c>source.Timeout(dueTime, other)</c> with a relative due time, on the target's
    /// default scheduler.
    /// </summary>
    /// <typeparam name="T">The type of the elements in the source sequence.</typeparam>
    /// <param name="source">Source sequence to perform a timeout for.</param>
    /// <param name="dueTime">Maximum duration between values before a timeout occurs.</param>
    /// <param name="other">Sequence to return in case of a timeout.</param>
    /// <remarks>
    /// Builds a <see cref="TimeoutTimeOtherSeq{T}"/> with no scheduler, which each target
    /// materializes through <see cref="ISeqVisitor.TimeoutTimeOther{T}(TimeoutTimeOtherSeq{T})"/>.
    /// </remarks>
    public static Seq<T> Timeout<T>(this Seq<T> source, TimeSpan dueTime, Seq<T> other) =>
        new TimeoutTimeOtherSeq<T>(source, dueTime, other, null);

    /// <summary>
    /// Describes <c>source.Timeout(dueTime, scheduler)</c> with an absolute due time.
    /// </summary>
    /// <typeparam name="T">The type of the elements in the source sequence.</typeparam>
    /// <param name="source">Source sequence to perform a timeout for.</param>
    /// <param name="dueTime">
    /// Time when a timeout occurs. If this value is less than or equal to
    /// DateTimeOffset.UtcNow, the timeout occurs immediately.
    /// </param>
    /// <param name="scheduler">Scheduler to run the timeout timers on.</param>
    /// <remarks>
    /// Builds a <see cref="TimeoutAbsoluteSeq{T}"/>, which each target materializes through
    /// <see cref="ISeqVisitor.TimeoutAbsolute{T}(TimeoutAbsoluteSeq{T})"/>.
    /// </remarks>
    public static Seq<T> Timeout<T>(
        this Seq<T> source,
        DateTimeOffset dueTime,
        SchedulerRef scheduler) =>
        new TimeoutAbsoluteSeq<T>(source, dueTime, scheduler);

    /// <summary>
    /// Describes <c>source.Timeout(dueTime)</c> with an absolute due time, on the target's default
    /// scheduler.
    /// </summary>
    /// <typeparam name="T">The type of the elements in the source sequence.</typeparam>
    /// <param name="source">Source sequence to perform a timeout for.</param>
    /// <param name="dueTime">
    /// Time when a timeout occurs. If this value is less than or equal to
    /// DateTimeOffset.UtcNow, the timeout occurs immediately.
    /// </param>
    /// <remarks>
    /// Builds a <see cref="TimeoutAbsoluteSeq{T}"/> with no scheduler, which each target
    /// materializes through <see cref="ISeqVisitor.TimeoutAbsolute{T}(TimeoutAbsoluteSeq{T})"/>.
    /// </remarks>
    public static Seq<T> Timeout<T>(this Seq<T> source, DateTimeOffset dueTime) =>
        new TimeoutAbsoluteSeq<T>(source, dueTime, null);

    /// <summary>
    /// Describes <c>source.Timeout(dueTime, other, scheduler)</c> with an absolute due time.
    /// </summary>
    /// <typeparam name="T">The type of the elements in the source sequence.</typeparam>
    /// <param name="source">Source sequence to perform a timeout for.</param>
    /// <param name="dueTime">
    /// Time when a timeout occurs. If this value is less than or equal to
    /// DateTimeOffset.UtcNow, the timeout occurs immediately.
    /// </param>
    /// <param name="other">Sequence to return in case of a timeout.</param>
    /// <param name="scheduler">Scheduler to run the timeout timers on.</param>
    /// <remarks>
    /// Builds a <see cref="TimeoutAbsoluteOtherSeq{T}"/>, which each target materializes through
    /// <see cref="ISeqVisitor.TimeoutAbsoluteOther{T}(TimeoutAbsoluteOtherSeq{T})"/>.
    /// </remarks>
    public static Seq<T> Timeout<T>(
        this Seq<T> source,
        DateTimeOffset dueTime,
        Seq<T> other,
        SchedulerRef scheduler) =>
        new TimeoutAbsoluteOtherSeq<T>(source, dueTime, other, scheduler);

    /// <summary>
    /// Describes <c>source.Timeout(dueTime, other)</c> with an absolute due time, on the target's
    /// default scheduler.
    /// </summary>
    /// <typeparam name="T">The type of the elements in the source sequence.</typeparam>
    /// <param name="source">Source sequence to perform a timeout for.</param>
    /// <param name="dueTime">
    /// Time when a timeout occurs. If this value is less than or equal to
    /// DateTimeOffset.UtcNow, the timeout occurs immediately.
    /// </param>
    /// <param name="other">Sequence to return in case of a timeout.</param>
    /// <remarks>
    /// Builds a <see cref="TimeoutAbsoluteOtherSeq{T}"/> with no scheduler, which each target
    /// materializes through
    /// <see cref="ISeqVisitor.TimeoutAbsoluteOther{T}(TimeoutAbsoluteOtherSeq{T})"/>.
    /// </remarks>
    public static Seq<T> Timeout<T>(this Seq<T> source, DateTimeOffset dueTime, Seq<T> other) =>
        new TimeoutAbsoluteOtherSeq<T>(source, dueTime, other, null);

    /// <summary>
    /// Describes <c>source.Timeout(timeoutDurationSelector)</c>.
    /// </summary>
    /// <typeparam name="T">The type of the elements in the source sequence.</typeparam>
    /// <typeparam name="TTimeout">
    /// The type of the elements in the timeout sequences used to indicate the timeout duration for
    /// each element in the source sequence.
    /// </typeparam>
    /// <param name="source">Source sequence to perform a timeout for.</param>
    /// <param name="timeoutDurationSelector">
    /// Selector to retrieve an observable sequence that represents the timeout between the current
    /// element and the next element.
    /// </param>
    /// <param name="text">
    /// Supplied by the compiler (the source text of the selector), for printing the query in
    /// diagnostics; do not pass it.
    /// </param>
    /// <remarks>
    /// Builds a <see cref="TimeoutDurationSeq{T, TTimeout}"/>, which each target materializes
    /// through <see cref="ISeqVisitor.TimeoutDuration{T, TTimeout}(TimeoutDurationSeq{T, TTimeout})"/>.
    /// </remarks>
    public static Seq<T> Timeout<T, TTimeout>(
        this Seq<T> source,
        Func<T, Seq<TTimeout>> timeoutDurationSelector,
        [CallerArgumentExpression(nameof(timeoutDurationSelector))] string text = "") =>
        new TimeoutDurationSeq<T, TTimeout>(source, timeoutDurationSelector, text);

    /// <summary>
    /// Describes <c>source.Timeout(timeoutDurationSelector, other)</c>.
    /// </summary>
    /// <typeparam name="T">The type of the elements in the source sequence.</typeparam>
    /// <typeparam name="TTimeout">
    /// The type of the elements in the timeout sequences used to indicate the timeout duration for
    /// each element in the source sequence.
    /// </typeparam>
    /// <param name="source">Source sequence to perform a timeout for.</param>
    /// <param name="timeoutDurationSelector">
    /// Selector to retrieve an observable sequence that represents the timeout between the current
    /// element and the next element.
    /// </param>
    /// <param name="other">Sequence to return in case of a timeout.</param>
    /// <param name="text">
    /// Supplied by the compiler (the source text of the selector), for printing the query in
    /// diagnostics; do not pass it.
    /// </param>
    /// <remarks>
    /// Builds a <see cref="TimeoutDurationOtherSeq{T, TTimeout}"/>, which each target
    /// materializes through
    /// <see cref="ISeqVisitor.TimeoutDurationOther{T, TTimeout}(TimeoutDurationOtherSeq{T, TTimeout})"/>.
    /// </remarks>
    public static Seq<T> Timeout<T, TTimeout>(
        this Seq<T> source,
        Func<T, Seq<TTimeout>> timeoutDurationSelector,
        Seq<T> other,
        [CallerArgumentExpression(nameof(timeoutDurationSelector))] string text = "") =>
        new TimeoutDurationOtherSeq<T, TTimeout>(source, timeoutDurationSelector, other, text);

    /// <summary>
    /// Describes <c>source.Timeout(firstTimeout, timeoutDurationSelector)</c>.
    /// </summary>
    /// <typeparam name="T">The type of the elements in the source sequence.</typeparam>
    /// <typeparam name="TTimeout">
    /// The type of the elements in the timeout sequences used to indicate the timeout duration for
    /// each element in the source sequence.
    /// </typeparam>
    /// <param name="source">Source sequence to perform a timeout for.</param>
    /// <param name="firstTimeout">
    /// Observable sequence that represents the timeout for the first element.
    /// </param>
    /// <param name="timeoutDurationSelector">
    /// Selector to retrieve an observable sequence that represents the timeout between the current
    /// element and the next element.
    /// </param>
    /// <param name="text">
    /// Supplied by the compiler (the source text of the selector), for printing the query in
    /// diagnostics; do not pass it.
    /// </param>
    /// <remarks>
    /// Builds a <see cref="TimeoutFirstDurationSeq{T, TTimeout}"/>, which each target
    /// materializes through
    /// <see cref="ISeqVisitor.TimeoutFirstDuration{T, TTimeout}(TimeoutFirstDurationSeq{T, TTimeout})"/>.
    /// </remarks>
    public static Seq<T> Timeout<T, TTimeout>(
        this Seq<T> source,
        Seq<TTimeout> firstTimeout,
        Func<T, Seq<TTimeout>> timeoutDurationSelector,
        [CallerArgumentExpression(nameof(timeoutDurationSelector))] string text = "") =>
        new TimeoutFirstDurationSeq<T, TTimeout>(
            source,
            firstTimeout,
            timeoutDurationSelector,
            text);

    /// <summary>
    /// Describes <c>source.Timeout(firstTimeout, timeoutDurationSelector, other)</c>.
    /// </summary>
    /// <typeparam name="T">The type of the elements in the source sequence.</typeparam>
    /// <typeparam name="TTimeout">
    /// The type of the elements in the timeout sequences used to indicate the timeout duration for
    /// each element in the source sequence.
    /// </typeparam>
    /// <param name="source">Source sequence to perform a timeout for.</param>
    /// <param name="firstTimeout">
    /// Observable sequence that represents the timeout for the first element.
    /// </param>
    /// <param name="timeoutDurationSelector">
    /// Selector to retrieve an observable sequence that represents the timeout between the current
    /// element and the next element.
    /// </param>
    /// <param name="other">Sequence to return in case of a timeout.</param>
    /// <param name="text">
    /// Supplied by the compiler (the source text of the selector), for printing the query in
    /// diagnostics; do not pass it.
    /// </param>
    /// <remarks>
    /// Builds a <see cref="TimeoutFirstDurationOtherSeq{T, TTimeout}"/>, which each target
    /// materializes through
    /// <see cref="ISeqVisitor.TimeoutFirstDurationOther{T, TTimeout}(TimeoutFirstDurationOtherSeq{T, TTimeout})"/>.
    /// </remarks>
    public static Seq<T> Timeout<T, TTimeout>(
        this Seq<T> source,
        Seq<TTimeout> firstTimeout,
        Func<T, Seq<TTimeout>> timeoutDurationSelector,
        Seq<T> other,
        [CallerArgumentExpression(nameof(timeoutDurationSelector))] string text = "") =>
        new TimeoutFirstDurationOtherSeq<T, TTimeout>(
            source,
            firstTimeout,
            timeoutDurationSelector,
            other,
            text);
}

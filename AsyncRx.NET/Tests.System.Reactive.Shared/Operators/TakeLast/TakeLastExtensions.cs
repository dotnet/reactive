// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information. 

namespace Tests.System.Reactive.Shared;

/// <summary>The <c>TakeLast</c> overloads, as extension methods on <see cref="Seq{T}"/>.</summary>
/// <remarks>
/// These exist so that a shared scenario writes <c>xs.TakeLast(...)</c> exactly as the Rx.NET
/// test it was migrated from does: the count form and the duration form, each with and without
/// a scheduler (one node each, the scheduler optional, as <c>Delay</c>'s), and the duration form
/// with a timer scheduler and a loop scheduler, which AsyncRx.NET takes as a clock and a
/// scheduler. Each method builds one node of the query description, which each target turns
/// into its own <c>TakeLast</c> call through the matching <see cref="ISeqVisitor"/> member in
/// <c>ISeqVisitor.TakeLast.cs</c>.
/// </remarks>
public static class TakeLastExtensions
{
    /// <summary>Describes <c>source.TakeLast(count)</c>.</summary>
    /// <typeparam name="T">The type of the elements in the source sequence.</typeparam>
    /// <param name="source">Source sequence.</param>
    /// <param name="count">Number of elements to take from the end of the source sequence.</param>
    /// <remarks>
    /// Builds a <see cref="TakeLastCountSeq{T}"/> with no scheduler, which each target
    /// materializes through <see cref="ISeqVisitor.TakeLastCount{T}(TakeLastCountSeq{T})"/>.
    /// </remarks>
    public static Seq<T> TakeLast<T>(this Seq<T> source, int count) =>
        new TakeLastCountSeq<T>(source, count, null);

    /// <summary>Describes <c>source.TakeLast(count, scheduler)</c>.</summary>
    /// <typeparam name="T">The type of the elements in the source sequence.</typeparam>
    /// <param name="source">Source sequence.</param>
    /// <param name="count">Number of elements to take from the end of the source sequence.</param>
    /// <param name="scheduler">
    /// Scheduler used to drain the queue upon completion of the source sequence.
    /// </param>
    /// <remarks>
    /// Builds a <see cref="TakeLastCountSeq{T}"/>, which each target materializes through
    /// <see cref="ISeqVisitor.TakeLastCount{T}(TakeLastCountSeq{T})"/>.
    /// </remarks>
    public static Seq<T> TakeLast<T>(this Seq<T> source, int count, SchedulerRef scheduler) =>
        new TakeLastCountSeq<T>(source, count, scheduler);

    /// <summary>Describes <c>source.TakeLast(duration)</c>.</summary>
    /// <typeparam name="T">The type of the elements in the source sequence.</typeparam>
    /// <param name="source">Source sequence to take elements from.</param>
    /// <param name="duration">Duration for taking elements from the end of the sequence.</param>
    /// <remarks>
    /// Builds a <see cref="TakeLastTimeSeq{T}"/> with no scheduler, which each target
    /// materializes through <see cref="ISeqVisitor.TakeLastTime{T}(TakeLastTimeSeq{T})"/>.
    /// </remarks>
    public static Seq<T> TakeLast<T>(this Seq<T> source, TimeSpan duration) =>
        new TakeLastTimeSeq<T>(source, duration, null);

    /// <summary>Describes <c>source.TakeLast(duration, scheduler)</c>.</summary>
    /// <typeparam name="T">The type of the elements in the source sequence.</typeparam>
    /// <param name="source">Source sequence to take elements from.</param>
    /// <param name="duration">Duration for taking elements from the end of the sequence.</param>
    /// <param name="scheduler">Scheduler to run the timer on.</param>
    /// <remarks>
    /// Builds a <see cref="TakeLastTimeSeq{T}"/>, which each target materializes through
    /// <see cref="ISeqVisitor.TakeLastTime{T}(TakeLastTimeSeq{T})"/>.
    /// </remarks>
    public static Seq<T> TakeLast<T>(
        this Seq<T> source,
        TimeSpan duration,
        SchedulerRef scheduler) =>
        new TakeLastTimeSeq<T>(source, duration, scheduler);

    /// <summary>
    /// Describes <c>source.TakeLast(duration, timerScheduler, loopScheduler)</c>.
    /// </summary>
    /// <typeparam name="T">The type of the elements in the source sequence.</typeparam>
    /// <param name="source">Source sequence to take elements from.</param>
    /// <param name="duration">Duration for taking elements from the end of the sequence.</param>
    /// <param name="timerScheduler">Scheduler to run the timer on.</param>
    /// <param name="loopScheduler">Scheduler to drain the collected elements.</param>
    /// <remarks>
    /// Builds a <see cref="TakeLastTimeLoopSeq{T}"/>, which each target materializes through
    /// <see cref="ISeqVisitor.TakeLastTimeLoop{T}(TakeLastTimeLoopSeq{T})"/>.
    /// </remarks>
    public static Seq<T> TakeLast<T>(
        this Seq<T> source,
        TimeSpan duration,
        SchedulerRef timerScheduler,
        SchedulerRef loopScheduler) =>
        new TakeLastTimeLoopSeq<T>(source, duration, timerScheduler, loopScheduler);
}

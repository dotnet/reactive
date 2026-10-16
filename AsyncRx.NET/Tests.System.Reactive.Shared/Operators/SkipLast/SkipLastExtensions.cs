// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information. 

namespace Tests.System.Reactive.Shared;

/// <summary>The <c>SkipLast</c> overloads, as extension methods on <see cref="Seq{T}"/>.</summary>
/// <remarks>
/// These exist so that a shared scenario writes <c>xs.SkipLast(...)</c> exactly as the Rx.NET
/// test it was migrated from does: the count form and the duration form with and without a
/// scheduler (one node, the scheduler optional, as <c>Delay</c>'s), which AsyncRx.NET takes as a
/// clock. Each method builds one node of the query description, which each target turns into
/// its own <c>SkipLast</c> call through the matching <see cref="ISeqVisitor"/> member in
/// <c>ISeqVisitor.SkipLast.cs</c>.
/// </remarks>
public static class SkipLastExtensions
{
    /// <summary>Describes <c>source.SkipLast(count)</c>.</summary>
    /// <typeparam name="T">The type of the elements in the source sequence.</typeparam>
    /// <param name="source">Source sequence.</param>
    /// <param name="count">Number of elements to bypass at the end of the source sequence.</param>
    /// <remarks>
    /// Builds a <see cref="SkipLastCountSeq{T}"/>, which each target materializes through
    /// <see cref="ISeqVisitor.SkipLastCount{T}(SkipLastCountSeq{T})"/>.
    /// </remarks>
    public static Seq<T> SkipLast<T>(this Seq<T> source, int count) =>
        new SkipLastCountSeq<T>(source, count);

    /// <summary>Describes <c>source.SkipLast(duration)</c>.</summary>
    /// <typeparam name="T">The type of the elements in the source sequence.</typeparam>
    /// <param name="source">Source sequence to skip elements for.</param>
    /// <param name="duration">Duration for skipping elements from the end of the sequence.</param>
    /// <remarks>
    /// Builds a <see cref="SkipLastTimeSeq{T}"/> with no scheduler, which each target
    /// materializes through <see cref="ISeqVisitor.SkipLastTime{T}(SkipLastTimeSeq{T})"/>.
    /// </remarks>
    public static Seq<T> SkipLast<T>(this Seq<T> source, TimeSpan duration) =>
        new SkipLastTimeSeq<T>(source, duration, null);

    /// <summary>Describes <c>source.SkipLast(duration, scheduler)</c>.</summary>
    /// <typeparam name="T">The type of the elements in the source sequence.</typeparam>
    /// <param name="source">Source sequence to skip elements for.</param>
    /// <param name="duration">Duration for skipping elements from the end of the sequence.</param>
    /// <param name="scheduler">Scheduler to run the timer on.</param>
    /// <remarks>
    /// Builds a <see cref="SkipLastTimeSeq{T}"/>, which each target materializes through
    /// <see cref="ISeqVisitor.SkipLastTime{T}(SkipLastTimeSeq{T})"/>.
    /// </remarks>
    public static Seq<T> SkipLast<T>(
        this Seq<T> source,
        TimeSpan duration,
        SchedulerRef scheduler) =>
        new SkipLastTimeSeq<T>(source, duration, scheduler);
}

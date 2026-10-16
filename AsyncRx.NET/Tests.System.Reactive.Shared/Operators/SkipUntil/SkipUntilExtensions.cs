// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information. 

namespace Tests.System.Reactive.Shared;

/// <summary>The <c>SkipUntil</c> overloads, as extension methods on <see cref="Seq{T}"/>.</summary>
/// <remarks>
/// These exist so that a shared scenario writes <c>xs.SkipUntil(...)</c> exactly as the Rx.NET
/// test it was migrated from does: the other-sequence form and the start-time form with and
/// without the scheduler (one node, the scheduler optional, as <c>Delay</c>'s). Each method
/// builds one node of the query description, which each target turns into its own
/// <c>SkipUntil</c> call through the matching <see cref="ISeqVisitor"/> member in
/// <c>ISeqVisitor.SkipUntil.cs</c>.
/// </remarks>
public static class SkipUntilExtensions
{
    /// <summary>Describes <c>source.SkipUntil(other)</c>.</summary>
    /// <typeparam name="T">The type of the elements in the source sequence.</typeparam>
    /// <typeparam name="TOther">
    /// The type of the elements in the other sequence that indicates the end of skip behavior.
    /// </typeparam>
    /// <param name="source">Source sequence to propagate elements for.</param>
    /// <param name="other">
    /// Observable sequence that triggers propagation of elements of the source sequence.
    /// </param>
    /// <remarks>
    /// Builds a <see cref="SkipUntilSeq{T, TOther}"/>, which each target materializes through
    /// <see cref="ISeqVisitor.SkipUntil{T, TOther}(SkipUntilSeq{T, TOther})"/>.
    /// </remarks>
    public static Seq<T> SkipUntil<T, TOther>(this Seq<T> source, Seq<TOther> other) =>
        new SkipUntilSeq<T, TOther>(source, other);

    /// <summary>Describes <c>source.SkipUntil(startTime, scheduler)</c>.</summary>
    /// <typeparam name="T">The type of the elements in the source sequence.</typeparam>
    /// <param name="source">Source sequence to skip elements for.</param>
    /// <param name="startTime">
    /// Time to start taking elements from the source sequence. If this value is less than or
    /// equal to DateTimeOffset.UtcNow, no elements will be skipped.
    /// </param>
    /// <param name="scheduler">Scheduler to run the timer on.</param>
    /// <remarks>
    /// Builds a <see cref="SkipUntilTimeSeq{T}"/>, which each target materializes through
    /// <see cref="ISeqVisitor.SkipUntilTime{T}(SkipUntilTimeSeq{T})"/>.
    /// </remarks>
    public static Seq<T> SkipUntil<T>(
        this Seq<T> source,
        DateTimeOffset startTime,
        SchedulerRef scheduler) =>
        new SkipUntilTimeSeq<T>(source, startTime, scheduler);

    /// <summary>
    /// Describes <c>source.SkipUntil(startTime)</c>, on the target's default scheduler.
    /// </summary>
    /// <typeparam name="T">The type of the elements in the source sequence.</typeparam>
    /// <param name="source">Source sequence to skip elements for.</param>
    /// <param name="startTime">
    /// Time to start taking elements from the source sequence. If this value is less than or
    /// equal to DateTimeOffset.UtcNow, no elements will be skipped.
    /// </param>
    /// <remarks>
    /// Builds a <see cref="SkipUntilTimeSeq{T}"/> with no scheduler, which each target
    /// materializes through <see cref="ISeqVisitor.SkipUntilTime{T}(SkipUntilTimeSeq{T})"/>.
    /// </remarks>
    public static Seq<T> SkipUntil<T>(this Seq<T> source, DateTimeOffset startTime) =>
        new SkipUntilTimeSeq<T>(source, startTime, null);
}

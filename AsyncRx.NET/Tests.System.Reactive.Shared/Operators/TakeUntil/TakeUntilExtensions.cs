// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information. 

using System.Runtime.CompilerServices;

namespace Tests.System.Reactive.Shared;

/// <summary>The <c>TakeUntil</c> overloads, as extension methods on <see cref="Seq{T}"/>.</summary>
/// <remarks>
/// These exist so that a shared scenario writes <c>xs.TakeUntil(...)</c> exactly as the Rx.NET
/// test it was migrated from does: the other-sequence form, the end-time form with and without
/// the scheduler (one node, the scheduler optional, as <c>Delay</c>'s), the stop-predicate form
/// and the cancellation-token form. Each method builds one node of the query description, which
/// each target turns into its own <c>TakeUntil</c> call through the matching
/// <see cref="ISeqVisitor"/> member in <c>ISeqVisitor.TakeUntil.cs</c>.
/// </remarks>
public static class TakeUntilExtensions
{
    /// <summary>Describes <c>source.TakeUntil(other)</c>.</summary>
    /// <typeparam name="T">The type of the elements in the source sequence.</typeparam>
    /// <typeparam name="TOther">
    /// The type of the elements in the other sequence that indicates the end of take behavior.
    /// </typeparam>
    /// <param name="source">Source sequence to propagate elements for.</param>
    /// <param name="other">
    /// Observable sequence that terminates propagation of elements of the source sequence.
    /// </param>
    /// <remarks>
    /// Builds a <see cref="TakeUntilSeq{T, TOther}"/>, which each target materializes through
    /// <see cref="ISeqVisitor.TakeUntil{T, TOther}(TakeUntilSeq{T, TOther})"/>.
    /// </remarks>
    public static Seq<T> TakeUntil<T, TOther>(this Seq<T> source, Seq<TOther> other) =>
        new TakeUntilSeq<T, TOther>(source, other);

    /// <summary>Describes <c>source.TakeUntil(endTime, scheduler)</c>.</summary>
    /// <typeparam name="T">The type of the elements in the source sequence.</typeparam>
    /// <param name="source">Source sequence to take elements from.</param>
    /// <param name="endTime">
    /// Time to stop taking elements from the source sequence. If this value is less than or equal
    /// to DateTimeOffset.UtcNow, the result stream will complete immediately.
    /// </param>
    /// <param name="scheduler">Scheduler to run the timer on.</param>
    /// <remarks>
    /// Builds a <see cref="TakeUntilTimeSeq{T}"/>, which each target materializes through
    /// <see cref="ISeqVisitor.TakeUntilTime{T}(TakeUntilTimeSeq{T})"/>.
    /// </remarks>
    public static Seq<T> TakeUntil<T>(
        this Seq<T> source,
        DateTimeOffset endTime,
        SchedulerRef scheduler) =>
        new TakeUntilTimeSeq<T>(source, endTime, scheduler);

    /// <summary>
    /// Describes <c>source.TakeUntil(endTime)</c>, on the target's default scheduler.
    /// </summary>
    /// <typeparam name="T">The type of the elements in the source sequence.</typeparam>
    /// <param name="source">Source sequence to take elements from.</param>
    /// <param name="endTime">
    /// Time to stop taking elements from the source sequence. If this value is less than or equal
    /// to DateTimeOffset.UtcNow, the result stream will complete immediately.
    /// </param>
    /// <remarks>
    /// Builds a <see cref="TakeUntilTimeSeq{T}"/> with no scheduler, which each target materializes
    /// through <see cref="ISeqVisitor.TakeUntilTime{T}(TakeUntilTimeSeq{T})"/>.
    /// </remarks>
    public static Seq<T> TakeUntil<T>(this Seq<T> source, DateTimeOffset endTime) =>
        new TakeUntilTimeSeq<T>(source, endTime, null);

    /// <summary>Describes <c>source.TakeUntil(stopPredicate)</c>.</summary>
    /// <typeparam name="T">The type of the elements in the source and result sequences.</typeparam>
    /// <param name="source">The source sequence to relay elements of.</param>
    /// <param name="stopPredicate">
    /// Called after each upstream item has been emitted with that upstream item and should return
    /// <c>true</c> to indicate the sequence should complete.
    /// </param>
    /// <param name="text">
    /// Supplied by the compiler (the source text of the predicate), for printing the query in
    /// diagnostics; do not pass it.
    /// </param>
    /// <remarks>
    /// Builds a <see cref="TakeUntilPredicateSeq{T}"/>, which each target materializes through
    /// <see cref="ISeqVisitor.TakeUntilPredicate{T}(TakeUntilPredicateSeq{T})"/>.
    /// </remarks>
    public static Seq<T> TakeUntil<T>(
        this Seq<T> source,
        Func<T, bool> stopPredicate,
        [CallerArgumentExpression(nameof(stopPredicate))] string text = "") =>
        new TakeUntilPredicateSeq<T>(source, stopPredicate, text);

    /// <summary>Describes <c>source.TakeUntil(cancellationToken)</c>.</summary>
    /// <typeparam name="T">The type of the elements in the source and result sequences.</typeparam>
    /// <param name="source">The source sequence to relay elements of.</param>
    /// <param name="cancellationToken">
    /// The cancellation token to complete the target observable sequence on.
    /// </param>
    /// <remarks>
    /// Builds a <see cref="TakeUntilTokenSeq{T}"/>, which each target materializes through
    /// <see cref="ISeqVisitor.TakeUntilToken{T}(TakeUntilTokenSeq{T})"/>.
    /// </remarks>
    public static Seq<T> TakeUntil<T>(this Seq<T> source, CancellationToken cancellationToken) =>
        new TakeUntilTokenSeq<T>(source, cancellationToken);
}

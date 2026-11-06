// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information. 

namespace Tests.System.Reactive.Shared;

/// <summary>
/// The <c>TakeLastBuffer</c> overloads, as extension methods on <see cref="Seq{T}"/>.
/// </summary>
/// <remarks>
/// These exist so that a shared scenario writes <c>xs.TakeLastBuffer(...)</c> exactly as the
/// Rx.NET test it was migrated from does: the count form and the duration form with and without
/// a scheduler (one node, the scheduler optional, as <c>Delay</c>'s), which AsyncRx.NET takes as a
/// clock. Every form returns a <c>Seq&lt;IList&lt;T&gt;&gt;</c>, one list, as the library's do.
/// Each method builds one node of the query description, which each target turns into its own
/// <c>TakeLastBuffer</c> call through the matching <see cref="ISeqVisitor"/> member in
/// <c>ISeqVisitor.TakeLastBuffer.cs</c>.
/// </remarks>
public static class TakeLastBufferExtensions
{
    /// <summary>Describes <c>source.TakeLastBuffer(count)</c>.</summary>
    /// <typeparam name="T">The type of the elements in the source sequence.</typeparam>
    /// <param name="source">Source sequence.</param>
    /// <param name="count">Number of elements to take from the end of the source sequence.</param>
    /// <remarks>
    /// Builds a <see cref="TakeLastBufferCountSeq{T}"/>, which each target materializes through
    /// <see cref="ISeqVisitor.TakeLastBufferCount{T}(TakeLastBufferCountSeq{T})"/>.
    /// </remarks>
    public static Seq<IList<T>> TakeLastBuffer<T>(this Seq<T> source, int count) =>
        new TakeLastBufferCountSeq<T>(source, count);

    /// <summary>Describes <c>source.TakeLastBuffer(duration)</c>.</summary>
    /// <typeparam name="T">The type of the elements in the source sequence.</typeparam>
    /// <param name="source">Source sequence to take elements from.</param>
    /// <param name="duration">Duration for taking elements from the end of the sequence.</param>
    /// <remarks>
    /// Builds a <see cref="TakeLastBufferTimeSeq{T}"/> with no scheduler, which each target
    /// materializes through
    /// <see cref="ISeqVisitor.TakeLastBufferTime{T}(TakeLastBufferTimeSeq{T})"/>.
    /// </remarks>
    public static Seq<IList<T>> TakeLastBuffer<T>(this Seq<T> source, TimeSpan duration) =>
        new TakeLastBufferTimeSeq<T>(source, duration, null);

    /// <summary>Describes <c>source.TakeLastBuffer(duration, scheduler)</c>.</summary>
    /// <typeparam name="T">The type of the elements in the source sequence.</typeparam>
    /// <param name="source">Source sequence to take elements from.</param>
    /// <param name="duration">Duration for taking elements from the end of the sequence.</param>
    /// <param name="scheduler">Scheduler to run the timer on.</param>
    /// <remarks>
    /// Builds a <see cref="TakeLastBufferTimeSeq{T}"/>, which each target materializes through
    /// <see cref="ISeqVisitor.TakeLastBufferTime{T}(TakeLastBufferTimeSeq{T})"/>.
    /// </remarks>
    public static Seq<IList<T>> TakeLastBuffer<T>(
        this Seq<T> source,
        TimeSpan duration,
        SchedulerRef scheduler) =>
        new TakeLastBufferTimeSeq<T>(source, duration, scheduler);
}

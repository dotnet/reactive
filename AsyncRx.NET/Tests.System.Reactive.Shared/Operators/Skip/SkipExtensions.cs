// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information. 

namespace Tests.System.Reactive.Shared;

/// <summary>The <c>Skip</c> overloads, as extension methods on <see cref="Seq{T}"/>.</summary>
/// <remarks>
/// These exist so that a shared scenario writes <c>xs.Skip(3)</c> exactly as the Rx.NET test it
/// was migrated from does. Each method builds one node of the query description (one node type
/// per overload, in this folder), and each target turns that node into its own <c>Skip</c> call
/// through the matching <see cref="ISeqVisitor"/> member in <c>ISeqVisitor.Skip.cs</c>.
/// </remarks>
public static class SkipExtensions
{
    /// <summary>Describes <c>source.Skip(count)</c>.</summary>
    /// <typeparam name="T">The type of the elements in the source sequence.</typeparam>
    /// <param name="source">The sequence to skip elements from.</param>
    /// <param name="count">
    /// The number of elements to skip before returning the remaining elements.
    /// </param>
    /// <remarks>
    /// Builds a <see cref="SkipSeq{T}"/>, which each target materializes through
    /// <see cref="ISeqVisitor.Skip{T}(SkipSeq{T})"/>.
    /// </remarks>
    public static Seq<T> Skip<T>(this Seq<T> source, int count) => new SkipSeq<T>(source, count);

    /// <summary>Describes <c>source.Skip(duration, scheduler)</c>.</summary>
    /// <typeparam name="T">The type of the elements in the source sequence.</typeparam>
    /// <param name="source">Source sequence to skip elements for.</param>
    /// <param name="duration">
    /// Duration for skipping elements from the start of the sequence.
    /// </param>
    /// <param name="scheduler">Scheduler to run the timer on.</param>
    /// <remarks>
    /// Builds a <see cref="SkipTimeSeq{T}"/>, which each target materializes through
    /// <see cref="ISeqVisitor.SkipTime{T}(SkipTimeSeq{T})"/>.
    /// </remarks>
    public static Seq<T> Skip<T>(this Seq<T> source, TimeSpan duration, SchedulerRef scheduler) =>
        new SkipTimeSeq<T>(source, duration, scheduler);

    /// <summary>Describes <c>source.Skip(duration)</c> on the default scheduler.</summary>
    /// <typeparam name="T">The type of the elements in the source sequence.</typeparam>
    /// <param name="source">Source sequence to skip elements for.</param>
    /// <param name="duration">
    /// Duration for skipping elements from the start of the sequence.
    /// </param>
    /// <remarks>
    /// Builds a <see cref="SkipDurationSeq{T}"/>, which each target materializes through
    /// <see cref="ISeqVisitor.SkipDuration{T}(SkipDurationSeq{T})"/>. Only a real-time scenario can
    /// use this form, since a virtual-time one must name the scheduler the timer runs on.
    /// </remarks>
    public static Seq<T> Skip<T>(this Seq<T> source, TimeSpan duration) =>
        new SkipDurationSeq<T>(source, duration);
}

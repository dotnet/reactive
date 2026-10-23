// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information. 

using System.Reactive;

namespace Tests.System.Reactive.Shared;

/// <summary>The <c>Timestamp</c> overloads, as extension methods on <see cref="Seq{T}"/>.</summary>
/// <remarks>
/// These exist so that a shared scenario writes <c>xs.Timestamp()</c> or
/// <c>xs.Timestamp(scheduler)</c> exactly as the Rx.NET test it was migrated from does. Both
/// build the one node of the query description, which each target turns into its own
/// <c>Timestamp</c> call through the matching <see cref="ISeqVisitor"/> member in
/// <c>ISeqVisitor.Timestamp.cs</c>. Rx.NET's scheduler argument is a clock on AsyncRx.NET
/// (<c>IClock</c>), which every scheduler there is.
/// </remarks>
public static class TimestampExtensions
{
    /// <summary>
    /// Describes <c>source.Timestamp()</c>, on the target's default scheduler.
    /// </summary>
    /// <typeparam name="T">The type of the elements in the source sequence.</typeparam>
    /// <param name="source">Source sequence to retrieve timestamps for.</param>
    /// <remarks>
    /// Builds a <see cref="TimestampSeq{T}"/> with no scheduler, which each target materializes
    /// through <see cref="ISeqVisitor.Timestamp{T}(TimestampSeq{T})"/>.
    /// </remarks>
    public static Seq<Timestamped<T>> Timestamp<T>(this Seq<T> source) =>
        new TimestampSeq<T>(source, null);

    /// <summary>Describes <c>source.Timestamp(scheduler)</c>.</summary>
    /// <typeparam name="T">The type of the elements in the source sequence.</typeparam>
    /// <param name="source">Source sequence to retrieve timestamps for.</param>
    /// <param name="scheduler">Scheduler used to compute timestamps.</param>
    /// <remarks>
    /// Builds a <see cref="TimestampSeq{T}"/>, which each target materializes through
    /// <see cref="ISeqVisitor.Timestamp{T}(TimestampSeq{T})"/>.
    /// </remarks>
    public static Seq<Timestamped<T>> Timestamp<T>(
        this Seq<T> source,
        SchedulerRef scheduler) =>
        new TimestampSeq<T>(source, scheduler);
}

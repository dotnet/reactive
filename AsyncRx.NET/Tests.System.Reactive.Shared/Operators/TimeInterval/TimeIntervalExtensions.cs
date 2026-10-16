// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information. 

using System.Reactive;

namespace Tests.System.Reactive.Shared;

/// <summary>The <c>TimeInterval</c> overloads, as extension methods on <see cref="Seq{T}"/>.</summary>
/// <remarks>
/// These exist so that a shared scenario writes <c>xs.TimeInterval()</c> or
/// <c>xs.TimeInterval(scheduler)</c> exactly as the Rx.NET test it was migrated from does. Both
/// build the one node of the query description, which each target turns into its own
/// <c>TimeInterval</c> call through the matching <see cref="ISeqVisitor"/> member in
/// <c>ISeqVisitor.TimeInterval.cs</c>. Rx.NET's scheduler argument is a clock on AsyncRx.NET
/// (<c>IClock</c>), which every scheduler there is.
/// </remarks>
public static class TimeIntervalExtensions
{
    /// <summary>
    /// Describes <c>source.TimeInterval()</c>, on the target's default scheduler.
    /// </summary>
    /// <typeparam name="T">The type of the elements in the source sequence.</typeparam>
    /// <param name="source">Source sequence to retrieve time intervals for.</param>
    /// <remarks>
    /// Builds a <see cref="TimeIntervalSeq{T}"/> with no scheduler, which each target materializes
    /// through <see cref="ISeqVisitor.TimeInterval{T}(TimeIntervalSeq{T})"/>.
    /// </remarks>
    public static Seq<TimeInterval<T>> TimeInterval<T>(this Seq<T> source) =>
        new TimeIntervalSeq<T>(source, null);

    /// <summary>Describes <c>source.TimeInterval(scheduler)</c>.</summary>
    /// <typeparam name="T">The type of the elements in the source sequence.</typeparam>
    /// <param name="source">Source sequence to retrieve time intervals for.</param>
    /// <param name="scheduler">Scheduler used to compute time intervals.</param>
    /// <remarks>
    /// Builds a <see cref="TimeIntervalSeq{T}"/>, which each target materializes through
    /// <see cref="ISeqVisitor.TimeInterval{T}(TimeIntervalSeq{T})"/>.
    /// </remarks>
    public static Seq<TimeInterval<T>> TimeInterval<T>(
        this Seq<T> source,
        SchedulerRef scheduler) =>
        new TimeIntervalSeq<T>(source, scheduler);
}

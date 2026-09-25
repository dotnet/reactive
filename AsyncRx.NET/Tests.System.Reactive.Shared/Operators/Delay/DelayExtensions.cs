// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information. 

using System.Runtime.CompilerServices;

namespace Tests.System.Reactive.Shared;

/// <summary>
/// The <c>Delay</c> overloads as extension methods on <see cref="Seq{T}"/>, so that a shared
/// scenario writes <c>xs.Delay(...)</c> exactly as the Rx.NET test it was migrated from does:
/// the two time-based forms (relative and absolute, both taking the scheduler) and the two
/// selector forms.
/// </summary>
/// <remarks>
/// Each method builds one node of the query description (one node type per overload, in this
/// folder), and each target turns that node into its own <c>Delay</c> call through the matching
/// <see cref="ISeqVisitor"/> member in <c>ISeqVisitor.Delay.cs</c>. <c>Delay</c> is both an
/// operator under test and plumbing other scenarios use (<c>Window_Closings_Empty</c> closes its
/// windows with <c>Empty().Delay(n, Scheduler)</c>); the description does not distinguish the two.
/// </remarks>
public static class DelayExtensions
{
    /// <summary>Describes <c>source.Delay(dueTime, scheduler)</c> with a relative due time.</summary>
    /// <typeparam name="T">The type of the elements in the source sequence.</typeparam>
    /// <param name="source">Source sequence to delay values for.</param>
    /// <param name="dueTime">Relative time by which to shift the observable sequence. If this value is equal to TimeSpan.Zero, the scheduler will dispatch observer callbacks as soon as possible.</param>
    /// <param name="scheduler">Scheduler to run the delay timers on.</param>
    /// <remarks>Builds a <see cref="DelayTimeSeq{T}"/>, which each target materializes through <see cref="ISeqVisitor.DelayTime{T}(DelayTimeSeq{T})"/>.</remarks>
    public static Seq<T> Delay<T>(this Seq<T> source, TimeSpan dueTime, SchedulerRef scheduler) => new DelayTimeSeq<T>(source, dueTime, scheduler);

    /// <summary>Describes <c>source.Delay(dueTime, scheduler)</c> with an absolute due time.</summary>
    /// <typeparam name="T">The type of the elements in the source sequence.</typeparam>
    /// <param name="source">Source sequence to delay values for.</param>
    /// <param name="dueTime">Absolute time used to shift the observable sequence; the relative time shift gets computed upon subscription. If this value is less than or equal to DateTimeOffset.UtcNow, the scheduler will dispatch observer callbacks as soon as possible.</param>
    /// <param name="scheduler">Scheduler to run the delay timers on.</param>
    /// <remarks>Builds a <see cref="DelayAbsoluteSeq{T}"/>, which each target materializes through <see cref="ISeqVisitor.DelayAbsolute{T}(DelayAbsoluteSeq{T})"/>.</remarks>
    public static Seq<T> Delay<T>(this Seq<T> source, DateTimeOffset dueTime, SchedulerRef scheduler) => new DelayAbsoluteSeq<T>(source, dueTime, scheduler);

    /// <summary>Describes <c>source.Delay(delayDurationSelector)</c>.</summary>
    /// <typeparam name="T">The type of the elements in the source sequence.</typeparam>
    /// <typeparam name="TDelay">The type of the elements in the delay sequences used to denote the delay duration of each element in the source sequence.</typeparam>
    /// <param name="source">Source sequence to delay values for.</param>
    /// <param name="delayDurationSelector">Selector function to retrieve a sequence indicating the delay for each given element.</param>
    /// <param name="text">Supplied by the compiler (the source text of the selector), for printing the query in diagnostics; do not pass it.</param>
    /// <remarks>Builds a <see cref="DelaySelectorSeq{T, TDelay}"/>, which each target materializes through <see cref="ISeqVisitor.DelaySelector{T, TDelay}(DelaySelectorSeq{T, TDelay})"/>.</remarks>
    public static Seq<T> Delay<T, TDelay>(this Seq<T> source, Func<T, Seq<TDelay>> delayDurationSelector, [CallerArgumentExpression(nameof(delayDurationSelector))] string text = "") =>
        new DelaySelectorSeq<T, TDelay>(source, delayDurationSelector, text);

    /// <summary>Describes <c>source.Delay(subscriptionDelay, delayDurationSelector)</c>.</summary>
    /// <typeparam name="T">The type of the elements in the source sequence.</typeparam>
    /// <typeparam name="TDelay">The type of the elements in the delay sequences used to denote the delay duration of each element in the source sequence.</typeparam>
    /// <param name="source">Source sequence to delay values for.</param>
    /// <param name="subscriptionDelay">Sequence indicating the delay for the subscription to the source.</param>
    /// <param name="delayDurationSelector">Selector function to retrieve a sequence indicating the delay for each given element.</param>
    /// <param name="text">Supplied by the compiler (the source text of the selector), for printing the query in diagnostics; do not pass it.</param>
    /// <remarks>Builds a <see cref="DelaySubscriptionSeq{T, TDelay}"/>, which each target materializes through <see cref="ISeqVisitor.DelaySubscription{T, TDelay}(DelaySubscriptionSeq{T, TDelay})"/>.</remarks>
    public static Seq<T> Delay<T, TDelay>(this Seq<T> source, Seq<TDelay> subscriptionDelay, Func<T, Seq<TDelay>> delayDurationSelector, [CallerArgumentExpression(nameof(delayDurationSelector))] string text = "") =>
        new DelaySubscriptionSeq<T, TDelay>(source, subscriptionDelay, delayDurationSelector, text);
}

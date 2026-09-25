// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information. 

namespace Tests.System.Reactive.Shared;

/// <summary>
/// The <c>Take</c> overloads as extension methods on <see cref="Seq{T}"/>, so that a shared
/// scenario writes <c>xs.Take(3)</c> exactly as the Rx.NET test it was migrated from does.
/// </summary>
/// <remarks>
/// Nothing here runs an operator. Each method builds one node of the query description (one
/// node type per overload, in this folder), and when the scenario's query is materialized, each
/// target turns that node into its own <c>Take</c> call through the matching <see cref="ISeqVisitor"/>
/// member, declared in this folder's <c>ISeqVisitor.Take.cs</c> and implemented by <c>RxTarget</c>
/// and <c>AsyncRxTarget</c>. Each method's own comment names its node and visitor member.
/// </remarks>
public static class TakeExtensions
{
    /// <summary>Describes <c>source.Take(count)</c>.</summary>
    /// <typeparam name="T">The type of the elements in the source sequence.</typeparam>
    /// <param name="source">The sequence to take elements from.</param>
    /// <param name="count">The number of elements to return.</param>
    /// <remarks>Builds a <see cref="TakeSeq{T}"/>, which each target materializes through <see cref="ISeqVisitor.Take{T}(TakeSeq{T})"/>.</remarks>
    public static Seq<T> Take<T>(this Seq<T> source, int count) => new TakeSeq<T>(source, count);

    /// <summary>Describes <c>source.Take(count, scheduler)</c>.</summary>
    /// <typeparam name="T">The type of the elements in the source sequence.</typeparam>
    /// <param name="source">The sequence to take elements from.</param>
    /// <param name="count">The number of elements to return.</param>
    /// <param name="scheduler">Scheduler used to produce an OnCompleted message in case <paramref name="count">count</paramref> is set to 0.</param>
    /// <remarks>Builds a <see cref="TakeScheduledSeq{T}"/>, which each target materializes through <see cref="ISeqVisitor.TakeScheduled{T}(TakeScheduledSeq{T})"/>.</remarks>
    public static Seq<T> Take<T>(this Seq<T> source, int count, SchedulerRef scheduler) => new TakeScheduledSeq<T>(source, count, scheduler);

    /// <summary>Describes <c>source.Take(duration, scheduler)</c>.</summary>
    /// <typeparam name="T">The type of the elements in the source sequence.</typeparam>
    /// <param name="source">Source sequence to take elements from.</param>
    /// <param name="duration">Duration for taking elements from the start of the sequence.</param>
    /// <param name="scheduler">Scheduler to run the timer on.</param>
    /// <remarks>Builds a <see cref="TakeTimeSeq{T}"/>, which each target materializes through <see cref="ISeqVisitor.TakeTime{T}(TakeTimeSeq{T})"/>.</remarks>
    public static Seq<T> Take<T>(this Seq<T> source, TimeSpan duration, SchedulerRef scheduler) => new TakeTimeSeq<T>(source, duration, scheduler);
}

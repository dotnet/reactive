// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information. 

namespace Tests.System.Reactive.Shared;

/// <summary>The <c>Merge</c> overloads.</summary>
/// <remarks>
/// These are on <see cref="Seq"/> rather than in a <c>MergeExtensions</c> class because the
/// Rx.NET tests call them both as extension methods (<c>xs.Merge(ys)</c>, <c>xss.Merge(2)</c>)
/// and statically (<c>Observable.Merge(scheduler, xs, ys)</c>), and a shared scenario writes
/// <c>Seq.Merge(Scheduler, xs, ys)</c> for the latter. Each method builds one node of the query
/// description (one node type per overload, in this folder), and each target turns that node
/// into its own <c>Merge</c> call through the matching <see cref="ISeqVisitor"/> member in
/// <c>ISeqVisitor.Merge.cs</c>.
/// </remarks>
public static partial class Seq
{
    /// <summary>Describes <c>sources.Merge()</c> over a nested sequence.</summary>
    /// <typeparam name="T">The type of the elements in the source sequences.</typeparam>
    /// <param name="sources">Observable sequence of inner observable sequences.</param>
    /// <remarks>
    /// Builds a <see cref="MergeSeq{T}"/>, which each target materializes through
    /// <see cref="ISeqVisitor.Merge{T}(MergeSeq{T})"/>.
    /// </remarks>
    public static Seq<T> Merge<T>(this Seq<Seq<T>> sources) => new MergeSeq<T>(sources);

    /// <summary>Describes <c>sources.Merge(maxConcurrent)</c> over a nested sequence.</summary>
    /// <typeparam name="T">The type of the elements in the source sequences.</typeparam>
    /// <param name="sources">Observable sequence of inner observable sequences.</param>
    /// <param name="maxConcurrent">
    /// Maximum number of inner observable sequences being subscribed to concurrently.
    /// </param>
    /// <remarks>
    /// Builds a <see cref="MergeMaxConcurrentSeq{T}"/>, which each target materializes through
    /// <see cref="ISeqVisitor.MergeMaxConcurrent{T}(MergeMaxConcurrentSeq{T})"/>.
    /// </remarks>
    public static Seq<T> Merge<T>(this Seq<Seq<T>> sources, int maxConcurrent) =>
        new MergeMaxConcurrentSeq<T>(sources, maxConcurrent);

    /// <summary>Describes <c>sources.Merge()</c> over a sequence of tasks.</summary>
    /// <typeparam name="T">The type of the results produced by the tasks.</typeparam>
    /// <param name="sources">Observable sequence of tasks.</param>
    /// <remarks>
    /// Builds a <see cref="MergeTasksSeq{T}"/>, which each target materializes through
    /// <see cref="ISeqVisitor.MergeTasks{T}(MergeTasksSeq{T})"/>.
    /// </remarks>
    public static Seq<T> Merge<T>(this Seq<Task<T>> sources) => new MergeTasksSeq<T>(sources);

    /// <summary>Describes <c>first.Merge(second)</c>.</summary>
    /// <typeparam name="T">The type of the elements in the source sequences.</typeparam>
    /// <param name="first">First observable sequence.</param>
    /// <param name="second">Second observable sequence.</param>
    /// <remarks>
    /// Builds a <see cref="MergeBinarySeq{T}"/>, which each target materializes through
    /// <see cref="ISeqVisitor.MergeBinary{T}(MergeBinarySeq{T})"/>.
    /// </remarks>
    public static Seq<T> Merge<T>(this Seq<T> first, Seq<T> second) =>
        new MergeBinarySeq<T>(first, second);

    /// <summary>Describes <c>first.Merge(second, scheduler)</c>.</summary>
    /// <typeparam name="T">The type of the elements in the source sequences.</typeparam>
    /// <param name="first">First observable sequence.</param>
    /// <param name="second">Second observable sequence.</param>
    /// <param name="scheduler">
    /// Scheduler used to introduce concurrency for making subscriptions to the given sequences.
    /// </param>
    /// <remarks>
    /// Builds a <see cref="MergeBinaryScheduledSeq{T}"/>, which each target materializes through
    /// <see cref="ISeqVisitor.MergeBinaryScheduled{T}(MergeBinaryScheduledSeq{T})"/>.
    /// </remarks>
    public static Seq<T> Merge<T>(this Seq<T> first, Seq<T> second, SchedulerRef scheduler) =>
        new MergeBinaryScheduledSeq<T>(first, second, scheduler);

    /// <summary>Describes <c>Seq.Merge(sources)</c> over an array of sequences.</summary>
    /// <typeparam name="T">The type of the elements in the source sequences.</typeparam>
    /// <param name="sources">Observable sequences.</param>
    /// <remarks>
    /// Builds a <see cref="MergeParamsSeq{T}"/>, which each target materializes through
    /// <see cref="ISeqVisitor.MergeParams{T}(MergeParamsSeq{T})"/>.
    /// </remarks>
    public static Seq<T> Merge<T>(params Seq<T>[] sources) => new MergeParamsSeq<T>(sources);

    /// <summary>
    /// Describes <c> Seq.Merge(scheduler, sources)</c> over an array of sequences.
    /// </summary>
    /// <typeparam name="T">The type of the elements in the source sequences.</typeparam>
    /// <param name="scheduler">
    /// Scheduler used to introduce concurrency for making subscriptions to the given sequences.
    /// </param>
    /// <param name="sources">Observable sequences.</param>
    /// <remarks>
    /// Builds a <see cref="MergeParamsScheduledSeq{T}"/>, which each target materializes through
    /// <see cref="ISeqVisitor.MergeParamsScheduled{T}(MergeParamsScheduledSeq{T})"/>.
    /// </remarks>
    public static Seq<T> Merge<T>(SchedulerRef scheduler, params Seq<T>[] sources) =>
        new MergeParamsScheduledSeq<T>(scheduler, sources);

    /// <summary>Describes <c>sources.Merge()</c> over an enumerable of sequences.</summary>
    /// <typeparam name="T">The type of the elements in the source sequences.</typeparam>
    /// <param name="sources">Enumerable sequence of observable sequences.</param>
    /// <remarks>
    /// Builds a <see cref="MergeEnumerableSeq{T}"/>, which each target materializes through
    /// <see cref="ISeqVisitor.MergeEnumerable{T}(MergeEnumerableSeq{T})"/>.
    /// </remarks>
    public static Seq<T> Merge<T>(this IEnumerable<Seq<T>> sources) =>
        new MergeEnumerableSeq<T>(sources);

    /// <summary>
    /// Describes <c> sources.Merge(scheduler)</c> over an enumerable of sequences.
    /// </summary>
    /// <typeparam name="T">The type of the elements in the source sequences.</typeparam>
    /// <param name="sources">Enumerable sequence of observable sequences.</param>
    /// <param name="scheduler">
    /// Scheduler to run the enumeration of the sequence of sources on.
    /// </param>
    /// <remarks>
    /// Builds a <see cref="MergeEnumerableScheduledSeq{T}"/>, which each target
    /// materializes through
    /// <see cref="ISeqVisitor.MergeEnumerableScheduled{T}(MergeEnumerableScheduledSeq{T})"/>.
    /// </remarks>
    public static Seq<T> Merge<T>(this IEnumerable<Seq<T>> sources, SchedulerRef scheduler) =>
        new MergeEnumerableScheduledSeq<T>(sources, scheduler);

    /// <summary>
    /// Describes <c>sources.Merge(maxConcurrent)</c> over an enumerable of sequences.
    /// </summary>
    /// <typeparam name="T">The type of the elements in the source sequences.</typeparam>
    /// <param name="sources">Enumerable sequence of observable sequences.</param>
    /// <param name="maxConcurrent">
    /// Maximum number of observable sequences being subscribed to concurrently.
    /// </param>
    /// <remarks>
    /// Builds a <see cref="MergeEnumerableMaxConcurrentSeq{T}"/>, which each target
    /// materializes through
    /// <see cref="ISeqVisitor.MergeEnumerableMaxConcurrent{T}(MergeEnumerableMaxConcurrentSeq{T})"/>.
    /// </remarks>
    public static Seq<T> Merge<T>(this IEnumerable<Seq<T>> sources, int maxConcurrent) =>
        new MergeEnumerableMaxConcurrentSeq<T>(sources, maxConcurrent);

    /// <summary>
    /// Describes <c>sources.Merge(maxConcurrent, scheduler)</c> over an enumerable of sequences.
    /// </summary>
    /// <typeparam name="T">The type of the elements in the source sequences.</typeparam>
    /// <param name="sources">Enumerable sequence of observable sequences.</param>
    /// <param name="maxConcurrent">
    /// Maximum number of observable sequences being subscribed to concurrently.
    /// </param>
    /// <param name="scheduler">
    /// Scheduler to run the enumeration of the sequence of sources on.
    /// </param>
    /// <remarks>
    /// Builds a <see cref="MergeEnumerableMaxConcurrentScheduledSeq{T}"/>, which each
    /// target materializes through
    /// <see cref="ISeqVisitor.MergeEnumerableMaxConcurrentScheduled{T}(MergeEnumerableMaxConcurrentScheduledSeq{T})"/>.
    /// </remarks>
    public static Seq<T> Merge<T>(
        this IEnumerable<Seq<T>> sources,
        int maxConcurrent,
        SchedulerRef scheduler) =>
        new MergeEnumerableMaxConcurrentScheduledSeq<T>(sources, maxConcurrent, scheduler);
}

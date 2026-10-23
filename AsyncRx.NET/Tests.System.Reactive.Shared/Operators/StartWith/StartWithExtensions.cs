// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information. 

namespace Tests.System.Reactive.Shared;

/// <summary>The <c>StartWith</c> overloads, as extension methods on <see cref="Seq{T}"/>.</summary>
/// <remarks>
/// Rx.NET's four forms: the values as a <c>params</c> array or an enumerable, each with or
/// without a scheduler to emit them on. The <c>params</c> form came first, for the <c>Catch</c>
/// scenarios to compose with; the other three joined when <c>StartWith</c> came under test.
/// Each method builds one node of the query description, which each target turns into its own
/// <c>StartWith</c> call through the matching <see cref="ISeqVisitor"/> member in
/// <c>ISeqVisitor.StartWith.cs</c>.
/// </remarks>
public static class StartWithExtensions
{
    /// <summary>Describes <c>source.StartWith(values)</c>.</summary>
    /// <typeparam name="T">The type of the elements in the source sequence.</typeparam>
    /// <param name="source">Source sequence to prepend values to.</param>
    /// <param name="values">Values to prepend to the specified sequence.</param>
    /// <remarks>
    /// Builds a <see cref="StartWithSeq{T}"/>, which each target materializes through
    /// <see cref="ISeqVisitor.StartWith{T}(StartWithSeq{T})"/>.
    /// </remarks>
    public static Seq<T> StartWith<T>(this Seq<T> source, params T[] values) =>
        new StartWithSeq<T>(source, values);

    /// <summary>Describes <c>source.StartWith(values)</c> over an enumerable of values.</summary>
    /// <typeparam name="T">The type of the elements in the source sequence.</typeparam>
    /// <param name="source">Source sequence to prepend values to.</param>
    /// <param name="values">Values to prepend to the specified sequence.</param>
    /// <remarks>
    /// Builds a <see cref="StartWithEnumerableSeq{T}"/>, which each target materializes through
    /// <see cref="ISeqVisitor.StartWithEnumerable{T}(StartWithEnumerableSeq{T})"/>.
    /// </remarks>
    public static Seq<T> StartWith<T>(this Seq<T> source, IEnumerable<T> values) =>
        new StartWithEnumerableSeq<T>(source, values);

    /// <summary>Describes <c>source.StartWith(scheduler, values)</c>.</summary>
    /// <typeparam name="T">The type of the elements in the source sequence.</typeparam>
    /// <param name="source">Source sequence to prepend values to.</param>
    /// <param name="scheduler">Scheduler to emit the prepended values on.</param>
    /// <param name="values">Values to prepend to the specified sequence.</param>
    /// <remarks>
    /// Builds a <see cref="StartWithSchedulerSeq{T}"/>, which each target materializes through
    /// <see cref="ISeqVisitor.StartWithScheduler{T}(StartWithSchedulerSeq{T})"/>.
    /// </remarks>
    public static Seq<T> StartWith<T>(
        this Seq<T> source,
        SchedulerRef scheduler,
        params T[] values) =>
        new StartWithSchedulerSeq<T>(source, scheduler, values);

    /// <summary>
    /// Describes <c>source.StartWith(scheduler, values)</c> over an enumerable of values.
    /// </summary>
    /// <typeparam name="T">The type of the elements in the source sequence.</typeparam>
    /// <param name="source">Source sequence to prepend values to.</param>
    /// <param name="scheduler">Scheduler to emit the prepended values on.</param>
    /// <param name="values">Values to prepend to the specified sequence.</param>
    /// <remarks>
    /// Builds a <see cref="StartWithSchedulerEnumerableSeq{T}"/>, which each target
    /// materializes through
    /// <see cref="ISeqVisitor.StartWithSchedulerEnumerable{T}(StartWithSchedulerEnumerableSeq{T})"/>.
    /// </remarks>
    public static Seq<T> StartWith<T>(
        this Seq<T> source,
        SchedulerRef scheduler,
        IEnumerable<T> values) =>
        new StartWithSchedulerEnumerableSeq<T>(source, scheduler, values);
}

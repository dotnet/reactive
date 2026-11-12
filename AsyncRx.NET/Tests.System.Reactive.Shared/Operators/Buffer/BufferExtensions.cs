// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information. 

using System.Runtime.CompilerServices;

namespace Tests.System.Reactive.Shared;

/// <summary>The <c>Buffer</c> overloads, as extension methods on <see cref="Seq{T}"/>.</summary>
/// <remarks>
/// These exist so that a shared scenario writes <c>xs.Buffer(...)</c> exactly as the Rx.NET test
/// it was migrated from does. Each method builds one node of the query description (one node type
/// per overload, in this folder), and each target turns that node into its own <c>Buffer</c> call
/// through the matching <see cref="ISeqVisitor"/> member in <c>ISeqVisitor.Buffer.cs</c>. Every
/// form returns a <c>Seq&lt;IList&lt;T&gt;&gt;</c>, a list per buffer, as the library's do; the
/// overload set is <c>Window</c>'s, one node per form, and the callbacks return descriptions
/// (<c>Seq&lt;TBufferClosing&gt;</c>), which the target materializes at the moment it invokes the
/// callback — so a closing selector that throws, throws inside the target's own call, as in the
/// original test.
/// </remarks>
public static class BufferExtensions
{
    /// <summary>Describes <c>source.Buffer(bufferClosingSelector)</c>.</summary>
    /// <typeparam name="T">
    /// The type of the elements in the source sequence, and in the lists in the result sequence.
    /// </typeparam>
    /// <typeparam name="TBufferClosing">
    /// The type of the elements in the sequences indicating buffer closing events.
    /// </typeparam>
    /// <param name="source">Source sequence to produce buffers over.</param>
    /// <param name="bufferClosingSelector">
    /// A function invoked to define the boundaries of the produced buffers. A new buffer is
    /// started when the previous one is closed.
    /// </param>
    /// <param name="text">
    /// Supplied by the compiler (the source text of the selector), for printing the query in
    /// diagnostics; do not pass it.
    /// </param>
    /// <remarks>
    /// Builds a <see cref="BufferClosingsSeq{T, TBufferClosing}"/>, which each target materializes
    /// through
    /// <see cref="ISeqVisitor.BufferClosings{T, TBufferClosing}(BufferClosingsSeq{T, TBufferClosing})"/>.
    /// </remarks>
    public static Seq<IList<T>> Buffer<T, TBufferClosing>(
        this Seq<T> source,
        Func<Seq<TBufferClosing>> bufferClosingSelector,
        [CallerArgumentExpression(nameof(bufferClosingSelector))] string text = "") =>
        new BufferClosingsSeq<T, TBufferClosing>(source, bufferClosingSelector, text);

    /// <summary>Describes <c>source.Buffer(bufferOpenings, bufferClosingSelector)</c>.</summary>
    /// <typeparam name="T">
    /// The type of the elements in the source sequence, and in the lists in the result sequence.
    /// </typeparam>
    /// <typeparam name="TBufferOpening">
    /// The type of the elements in the sequence indicating buffer opening events, also passed to
    /// the closing selector to obtain a sequence of buffer closing events.
    /// </typeparam>
    /// <typeparam name="TBufferClosing">
    /// The type of the elements in the sequences indicating buffer closing events.
    /// </typeparam>
    /// <param name="source">Source sequence to produce buffers over.</param>
    /// <param name="bufferOpenings">
    /// Observable sequence whose elements denote the creation of new buffers.
    /// </param>
    /// <param name="bufferClosingSelector">
    /// A function invoked to define the closing of each produced buffer.
    /// </param>
    /// <param name="text">
    /// Supplied by the compiler (the source text of the selector), for printing the query in
    /// diagnostics; do not pass it.
    /// </param>
    /// <remarks>
    /// Builds a <see cref="BufferOpeningsSeq{T, TBufferOpening, TBufferClosing}"/>, which each
    /// target materializes through
    /// <see cref="ISeqVisitor.BufferOpenings{T, TBufferOpening, TBufferClosing}(BufferOpeningsSeq{T, TBufferOpening, TBufferClosing})"/>.
    /// </remarks>
    public static Seq<IList<T>> Buffer<T, TBufferOpening, TBufferClosing>(
        this Seq<T> source,
        Seq<TBufferOpening> bufferOpenings,
        Func<TBufferOpening, Seq<TBufferClosing>> bufferClosingSelector,
        [CallerArgumentExpression(nameof(bufferClosingSelector))] string text = "") =>
        new BufferOpeningsSeq<T, TBufferOpening, TBufferClosing>(
            source,
            bufferOpenings,
            bufferClosingSelector,
            text);

    /// <summary>Describes <c>source.Buffer(bufferBoundaries)</c>.</summary>
    /// <typeparam name="T">
    /// The type of the elements in the source sequence, and in the lists in the result sequence.
    /// </typeparam>
    /// <typeparam name="TBufferBoundary">
    /// The type of the elements in the sequences indicating buffer boundary events.
    /// </typeparam>
    /// <param name="source">Source sequence to produce buffers over.</param>
    /// <param name="bufferBoundaries">
    /// Sequence of buffer boundary markers. The current buffer is closed and a new buffer is
    /// opened upon receiving a boundary marker.
    /// </param>
    /// <remarks>
    /// Builds a <see cref="BufferBoundariesSeq{T, TBufferBoundary}"/>, which each target
    /// materializes through
    /// <see cref="ISeqVisitor.BufferBoundaries{T, TBufferBoundary}(BufferBoundariesSeq{T, TBufferBoundary})"/>.
    /// </remarks>
    public static Seq<IList<T>> Buffer<T, TBufferBoundary>(
        this Seq<T> source,
        Seq<TBufferBoundary> bufferBoundaries) =>
        new BufferBoundariesSeq<T, TBufferBoundary>(source, bufferBoundaries);

    /// <summary>Describes <c>source.Buffer(count, skip)</c>.</summary>
    /// <typeparam name="T">
    /// The type of the elements in the source sequence, and in the lists in the result sequence.
    /// </typeparam>
    /// <param name="source">Source sequence to produce buffers over.</param>
    /// <param name="count">Length of each buffer.</param>
    /// <param name="skip">
    /// Number of elements to skip between creation of consecutive buffers.
    /// </param>
    /// <remarks>
    /// Builds a <see cref="BufferCountSeq{T}"/>, which each target materializes through
    /// <see cref="ISeqVisitor.BufferCount{T}(BufferCountSeq{T})"/>.
    /// </remarks>
    public static Seq<IList<T>> Buffer<T>(this Seq<T> source, int count, int skip) =>
        new BufferCountSeq<T>(source, count, skip);

    /// <summary>Describes <c>source.Buffer(timeSpan, scheduler)</c>.</summary>
    /// <typeparam name="T">
    /// The type of the elements in the source sequence, and in the lists in the result sequence.
    /// </typeparam>
    /// <param name="source">Source sequence to produce buffers over.</param>
    /// <param name="timeSpan">Length of each buffer.</param>
    /// <param name="scheduler">Scheduler to run buffering timers on.</param>
    /// <remarks>
    /// Builds a <see cref="BufferTimeSeq{T}"/>, which each target materializes through
    /// <see cref="ISeqVisitor.BufferTime{T}(BufferTimeSeq{T})"/>.
    /// </remarks>
    public static Seq<IList<T>> Buffer<T>(
        this Seq<T> source,
        TimeSpan timeSpan,
        SchedulerRef scheduler) =>
        new BufferTimeSeq<T>(source, timeSpan, scheduler);

    /// <summary>Describes <c>source.Buffer(timeSpan, timeShift, scheduler)</c>.</summary>
    /// <typeparam name="T">
    /// The type of the elements in the source sequence, and in the lists in the result sequence.
    /// </typeparam>
    /// <param name="source">Source sequence to produce buffers over.</param>
    /// <param name="timeSpan">Length of each buffer.</param>
    /// <param name="timeShift">Interval between creation of consecutive buffers.</param>
    /// <param name="scheduler">Scheduler to run buffering timers on.</param>
    /// <remarks>
    /// Builds a <see cref="BufferTimeShiftSeq{T}"/>, which each target materializes through
    /// <see cref="ISeqVisitor.BufferTimeShift{T}(BufferTimeShiftSeq{T})"/>.
    /// </remarks>
    public static Seq<IList<T>> Buffer<T>(
        this Seq<T> source,
        TimeSpan timeSpan,
        TimeSpan timeShift,
        SchedulerRef scheduler) =>
        new BufferTimeShiftSeq<T>(source, timeSpan, timeShift, scheduler);

    /// <summary>Describes <c>source.Buffer(timeSpan, count, scheduler)</c>.</summary>
    /// <typeparam name="T">
    /// The type of the elements in the source sequence, and in the lists in the result sequence.
    /// </typeparam>
    /// <param name="source">Source sequence to produce buffers over.</param>
    /// <param name="timeSpan">Maximum time length of a buffer.</param>
    /// <param name="count">Maximum element count of a buffer.</param>
    /// <param name="scheduler">Scheduler to run buffering timers on.</param>
    /// <remarks>
    /// Builds a <see cref="BufferTimeOrCountSeq{T}"/>, which each target materializes through
    /// <see cref="ISeqVisitor.BufferTimeOrCount{T}(BufferTimeOrCountSeq{T})"/>.
    /// </remarks>
    public static Seq<IList<T>> Buffer<T>(
        this Seq<T> source,
        TimeSpan timeSpan,
        int count,
        SchedulerRef scheduler) =>
        new BufferTimeOrCountSeq<T>(source, timeSpan, count, scheduler);

    /// <summary>Describes <c>source.Buffer(count)</c>.</summary>
    /// <typeparam name="T">
    /// The type of the elements in the source sequence, and in the lists in the result sequence.
    /// </typeparam>
    /// <param name="source">Source sequence to produce buffers over.</param>
    /// <param name="count">Length of each buffer.</param>
    /// <remarks>
    /// Builds a <see cref="BufferCountOnlySeq{T}"/>, which each target materializes through
    /// <see cref="ISeqVisitor.BufferCountOnly{T}(BufferCountOnlySeq{T})"/>.
    /// </remarks>
    public static Seq<IList<T>> Buffer<T>(this Seq<T> source, int count) =>
        new BufferCountOnlySeq<T>(source, count);

    /// <summary>Describes <c>source.Buffer(timeSpan)</c> on the default scheduler.</summary>
    /// <typeparam name="T">
    /// The type of the elements in the source sequence, and in the lists in the result sequence.
    /// </typeparam>
    /// <param name="source">Source sequence to produce buffers over.</param>
    /// <param name="timeSpan">Length of each buffer.</param>
    /// <remarks>
    /// Builds a <see cref="BufferTimeDefaultSeq{T}"/>, which each target materializes through
    /// <see cref="ISeqVisitor.BufferTimeDefault{T}(BufferTimeDefaultSeq{T})"/>. Only a real-time
    /// scenario can use this form, since a virtual-time one must name the scheduler.
    /// </remarks>
    public static Seq<IList<T>> Buffer<T>(this Seq<T> source, TimeSpan timeSpan) =>
        new BufferTimeDefaultSeq<T>(source, timeSpan);

    /// <summary>
    /// Describes <c>source.Buffer(timeSpan, timeShift)</c> on the default scheduler.
    /// </summary>
    /// <typeparam name="T">
    /// The type of the elements in the source sequence, and in the lists in the result sequence.
    /// </typeparam>
    /// <param name="source">Source sequence to produce buffers over.</param>
    /// <param name="timeSpan">Length of each buffer.</param>
    /// <param name="timeShift">Interval between creation of consecutive buffers.</param>
    /// <remarks>
    /// Builds a <see cref="BufferTimeShiftDefaultSeq{T}"/>, which each target materializes through
    /// <see cref="ISeqVisitor.BufferTimeShiftDefault{T}(BufferTimeShiftDefaultSeq{T})"/>. Only a
    /// real-time scenario can use this form, since a virtual-time one must name the scheduler.
    /// </remarks>
    public static Seq<IList<T>> Buffer<T>(
        this Seq<T> source,
        TimeSpan timeSpan,
        TimeSpan timeShift) =>
        new BufferTimeShiftDefaultSeq<T>(source, timeSpan, timeShift);

    /// <summary>
    /// Describes <c>source.Buffer(timeSpan, count)</c> on the default scheduler.
    /// </summary>
    /// <typeparam name="T">
    /// The type of the elements in the source sequence, and in the lists in the result sequence.
    /// </typeparam>
    /// <param name="source">Source sequence to produce buffers over.</param>
    /// <param name="timeSpan">Maximum time length of a buffer.</param>
    /// <param name="count">Maximum element count of a buffer.</param>
    /// <remarks>
    /// Builds a <see cref="BufferTimeOrCountDefaultSeq{T}"/>, which each target materializes
    /// through
    /// <see cref="ISeqVisitor.BufferTimeOrCountDefault{T}(BufferTimeOrCountDefaultSeq{T})"/>.
    /// Only a real-time scenario can use this form, since a virtual-time one must name the
    /// scheduler.
    /// </remarks>
    public static Seq<IList<T>> Buffer<T>(this Seq<T> source, TimeSpan timeSpan, int count) =>
        new BufferTimeOrCountDefaultSeq<T>(source, timeSpan, count);
}

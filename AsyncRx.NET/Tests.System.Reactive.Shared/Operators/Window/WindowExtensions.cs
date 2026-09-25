// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information. 

using System.Runtime.CompilerServices;

namespace Tests.System.Reactive.Shared;

/// <summary>
/// The <c>Window</c> overloads as extension methods on <see cref="Seq{T}"/>, so that a shared
/// scenario writes <c>xs.Window(...)</c> exactly as the Rx.NET test it was migrated from does.
/// </summary>
/// <remarks>
/// Each method builds one node of the query description (one node type per overload, in this
/// folder), and each target turns that node into its own <c>Window</c> call through the matching
/// <see cref="ISeqVisitor"/> member in <c>ISeqVisitor.Window.cs</c>. Every form returns a
/// <see cref="Nested{T}"/>. The callbacks return descriptions (<c>Seq&lt;TWindowClosing&gt;</c>),
/// which the target materializes at the moment it invokes the callback — so a closing selector
/// that throws, throws inside the target's own call, as in the original test.
/// </remarks>
public static class WindowExtensions
{
    /// <summary>Describes <c>source.Window(windowClosingSelector)</c>.</summary>
    /// <typeparam name="T">The type of the elements in the source sequence, and in the windows in the result sequence.</typeparam>
    /// <typeparam name="TWindowClosing">The type of the elements in the sequences indicating window closing events.</typeparam>
    /// <param name="source">Source sequence to produce windows over.</param>
    /// <param name="windowClosingSelector">A function invoked to define the boundaries of the produced windows. A new window is started when the previous one is closed.</param>
    /// <param name="text">Supplied by the compiler (the source text of the selector), for printing the query in diagnostics; do not pass it.</param>
    /// <remarks>Builds a <see cref="WindowClosingsSeq{T, TWindowClosing}"/>, which each target materializes through <see cref="ISeqVisitor.WindowClosings{T, TWindowClosing}(WindowClosingsSeq{T, TWindowClosing})"/>.</remarks>
    public static Nested<T> Window<T, TWindowClosing>(this Seq<T> source, Func<Seq<TWindowClosing>> windowClosingSelector, [CallerArgumentExpression(nameof(windowClosingSelector))] string text = "") =>
        new WindowClosingsSeq<T, TWindowClosing>(source, windowClosingSelector, text);

    /// <summary>Describes <c>source.Window(windowOpenings, windowClosingSelector)</c>.</summary>
    /// <typeparam name="T">The type of the elements in the source sequence, and in the windows in the result sequence.</typeparam>
    /// <typeparam name="TWindowOpening">The type of the elements in the sequence indicating window opening events, also passed to the closing selector to obtain a sequence of window closing events.</typeparam>
    /// <typeparam name="TWindowClosing">The type of the elements in the sequences indicating window closing events.</typeparam>
    /// <param name="source">Source sequence to produce windows over.</param>
    /// <param name="windowOpenings">Observable sequence whose elements denote the creation of new windows.</param>
    /// <param name="windowClosingSelector">A function invoked to define the closing of each produced window.</param>
    /// <param name="text">Supplied by the compiler (the source text of the selector), for printing the query in diagnostics; do not pass it.</param>
    /// <remarks>Builds a <see cref="WindowOpeningsSeq{T, TWindowOpening, TWindowClosing}"/>, which each target materializes through <see cref="ISeqVisitor.WindowOpenings{T, TWindowOpening, TWindowClosing}(WindowOpeningsSeq{T, TWindowOpening, TWindowClosing})"/>.</remarks>
    public static Nested<T> Window<T, TWindowOpening, TWindowClosing>(this Seq<T> source, Seq<TWindowOpening> windowOpenings, Func<TWindowOpening, Seq<TWindowClosing>> windowClosingSelector, [CallerArgumentExpression(nameof(windowClosingSelector))] string text = "") =>
        new WindowOpeningsSeq<T, TWindowOpening, TWindowClosing>(source, windowOpenings, windowClosingSelector, text);

    /// <summary>Describes <c>source.Window(windowBoundaries)</c>.</summary>
    /// <typeparam name="T">The type of the elements in the source sequence, and in the windows in the result sequence.</typeparam>
    /// <typeparam name="TWindowBoundary">The type of the elements in the sequences indicating window boundary events.</typeparam>
    /// <param name="source">Source sequence to produce windows over.</param>
    /// <param name="windowBoundaries">Sequence of window boundary markers. The current window is closed and a new window is opened upon receiving a boundary marker.</param>
    /// <remarks>Builds a <see cref="WindowBoundariesSeq{T, TWindowBoundary}"/>, which each target materializes through <see cref="ISeqVisitor.WindowBoundaries{T, TWindowBoundary}(WindowBoundariesSeq{T, TWindowBoundary})"/>.</remarks>
    public static Nested<T> Window<T, TWindowBoundary>(this Seq<T> source, Seq<TWindowBoundary> windowBoundaries) =>
        new WindowBoundariesSeq<T, TWindowBoundary>(source, windowBoundaries);

    /// <summary>Describes <c>source.Window(count, skip)</c>.</summary>
    /// <typeparam name="T">The type of the elements in the source sequence, and in the windows in the result sequence.</typeparam>
    /// <param name="source">Source sequence to produce windows over.</param>
    /// <param name="count">Length of each window.</param>
    /// <param name="skip">Number of elements to skip between creation of consecutive windows.</param>
    /// <remarks>Builds a <see cref="WindowCountSeq{T}"/>, which each target materializes through <see cref="ISeqVisitor.WindowCount{T}(WindowCountSeq{T})"/>.</remarks>
    public static Nested<T> Window<T>(this Seq<T> source, int count, int skip) => new WindowCountSeq<T>(source, count, skip);

    /// <summary>Describes <c>source.Window(timeSpan, scheduler)</c>.</summary>
    /// <typeparam name="T">The type of the elements in the source sequence, and in the windows in the result sequence.</typeparam>
    /// <param name="source">Source sequence to produce windows over.</param>
    /// <param name="timeSpan">Length of each window.</param>
    /// <param name="scheduler">Scheduler to run windowing timers on.</param>
    /// <remarks>Builds a <see cref="WindowTimeSeq{T}"/>, which each target materializes through <see cref="ISeqVisitor.WindowTime{T}(WindowTimeSeq{T})"/>.</remarks>
    public static Nested<T> Window<T>(this Seq<T> source, TimeSpan timeSpan, SchedulerRef scheduler) => new WindowTimeSeq<T>(source, timeSpan, scheduler);

    /// <summary>Describes <c>source.Window(timeSpan, timeShift, scheduler)</c>.</summary>
    /// <typeparam name="T">The type of the elements in the source sequence, and in the windows in the result sequence.</typeparam>
    /// <param name="source">Source sequence to produce windows over.</param>
    /// <param name="timeSpan">Length of each window.</param>
    /// <param name="timeShift">Interval between creation of consecutive windows.</param>
    /// <param name="scheduler">Scheduler to run windowing timers on.</param>
    /// <remarks>Builds a <see cref="WindowTimeShiftSeq{T}"/>, which each target materializes through <see cref="ISeqVisitor.WindowTimeShift{T}(WindowTimeShiftSeq{T})"/>.</remarks>
    public static Nested<T> Window<T>(this Seq<T> source, TimeSpan timeSpan, TimeSpan timeShift, SchedulerRef scheduler) => new WindowTimeShiftSeq<T>(source, timeSpan, timeShift, scheduler);

    /// <summary>Describes <c>source.Window(timeSpan, count, scheduler)</c>.</summary>
    /// <typeparam name="T">The type of the elements in the source sequence, and in the windows in the result sequence.</typeparam>
    /// <param name="source">Source sequence to produce windows over.</param>
    /// <param name="timeSpan">Maximum time length of a window.</param>
    /// <param name="count">Maximum element count of a window.</param>
    /// <param name="scheduler">Scheduler to run windowing timers on.</param>
    /// <remarks>Builds a <see cref="WindowTimeOrCountSeq{T}"/>, which each target materializes through <see cref="ISeqVisitor.WindowTimeOrCount{T}(WindowTimeOrCountSeq{T})"/>.</remarks>
    public static Nested<T> Window<T>(this Seq<T> source, TimeSpan timeSpan, int count, SchedulerRef scheduler) => new WindowTimeOrCountSeq<T>(source, timeSpan, count, scheduler);
}

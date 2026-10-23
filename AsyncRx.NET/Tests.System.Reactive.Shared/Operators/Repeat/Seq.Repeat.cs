// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information. 

namespace Tests.System.Reactive.Shared;

/// <summary>
/// The <c>Repeat</c> overloads, as static and extension methods on <see cref="Seq"/>.
/// </summary>
/// <remarks>
/// Where an Rx.NET test writes <c>Observable.Repeat(42, 10, scheduler)</c> or <c>xs.Repeat(3)</c>,
/// a shared scenario writes the same with <see cref="Seq"/> in place of <c>Observable</c>. The
/// creation forms repeat a value, with and without a count and with and without a scheduler:
/// two nodes with an optional scheduler, as <see cref="ReturnSeq{T}"/> is. The operator forms
/// repeat a sequence, without end or a given number of times: one node each. The operator
/// forms were plumbing before they came under test, nodes other scenarios compose with; they
/// keep that role from here. Each target turns a node into its own <c>Repeat</c> call through
/// the matching <see cref="ISeqVisitor"/> member in <c>ISeqVisitor.Repeat.cs</c>.
/// </remarks>
public static partial class Seq
{
    /// <summary>Describes <c>Seq.Repeat(value)</c>.</summary>
    /// <typeparam name="T">
    /// The type of the element that will be repeated in the produced sequence.
    /// </typeparam>
    /// <param name="value">Element to repeat.</param>
    /// <remarks>
    /// Builds a <see cref="RepeatValueSeq{T}"/>, which each target materializes through
    /// <see cref="ISeqVisitor.RepeatValue{T}(RepeatValueSeq{T})"/>.
    /// </remarks>
    public static Seq<T> Repeat<T>(T value) => new RepeatValueSeq<T>(value);

    /// <summary>Describes <c>Seq.Repeat(value, scheduler)</c>.</summary>
    /// <typeparam name="T">
    /// The type of the element that will be repeated in the produced sequence.
    /// </typeparam>
    /// <param name="value">Element to repeat.</param>
    /// <param name="scheduler">Scheduler to run the producer loop on.</param>
    /// <remarks>
    /// Builds a <see cref="RepeatValueSeq{T}"/>, which each target materializes through
    /// <see cref="ISeqVisitor.RepeatValue{T}(RepeatValueSeq{T})"/>.
    /// </remarks>
    public static Seq<T> Repeat<T>(T value, SchedulerRef scheduler) =>
        new RepeatValueSeq<T>(value, scheduler);

    /// <summary>Describes <c>Seq.Repeat(value, repeatCount)</c>.</summary>
    /// <typeparam name="T">
    /// The type of the element that will be repeated in the produced sequence.
    /// </typeparam>
    /// <param name="value">Element to repeat.</param>
    /// <param name="repeatCount">Number of times to repeat the element.</param>
    /// <remarks>
    /// Builds a <see cref="RepeatValueCountSeq{T}"/>, which each target materializes through
    /// <see cref="ISeqVisitor.RepeatValueCount{T}(RepeatValueCountSeq{T})"/>.
    /// </remarks>
    public static Seq<T> Repeat<T>(T value, int repeatCount) =>
        new RepeatValueCountSeq<T>(value, repeatCount);

    /// <summary>Describes <c>Seq.Repeat(value, repeatCount, scheduler)</c>.</summary>
    /// <typeparam name="T">
    /// The type of the element that will be repeated in the produced sequence.
    /// </typeparam>
    /// <param name="value">Element to repeat.</param>
    /// <param name="repeatCount">Number of times to repeat the element.</param>
    /// <param name="scheduler">Scheduler to run the producer loop on.</param>
    /// <remarks>
    /// Builds a <see cref="RepeatValueCountSeq{T}"/>, which each target materializes through
    /// <see cref="ISeqVisitor.RepeatValueCount{T}(RepeatValueCountSeq{T})"/>.
    /// </remarks>
    public static Seq<T> Repeat<T>(T value, int repeatCount, SchedulerRef scheduler) =>
        new RepeatValueCountSeq<T>(value, repeatCount, scheduler);

    /// <summary>Describes <c>source.Repeat()</c>.</summary>
    /// <typeparam name="T">The type of the elements in the source sequence.</typeparam>
    /// <param name="source">Observable sequence to repeat.</param>
    /// <remarks>
    /// Builds a <see cref="RepeatSeq{T}"/>, which each target materializes through
    /// <see cref="ISeqVisitor.Repeat{T}(RepeatSeq{T})"/>.
    /// </remarks>
    public static Seq<T> Repeat<T>(this Seq<T> source) => new RepeatSeq<T>(source);

    /// <summary>Describes <c>source.Repeat(repeatCount)</c>.</summary>
    /// <typeparam name="T">The type of the elements in the source sequence.</typeparam>
    /// <param name="source">Observable sequence to repeat.</param>
    /// <param name="repeatCount">Number of times to repeat the sequence.</param>
    /// <remarks>
    /// Builds a <see cref="RepeatCountSeq{T}"/>, which each target materializes through
    /// <see cref="ISeqVisitor.RepeatCount{T}(RepeatCountSeq{T})"/>.
    /// </remarks>
    public static Seq<T> Repeat<T>(this Seq<T> source, int repeatCount) =>
        new RepeatCountSeq<T>(source, repeatCount);
}

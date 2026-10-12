// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information. 

namespace Tests.System.Reactive.Shared;

/// <summary>
/// The plumbing operators the shared scenarios compose around the operator under test, as
/// extension methods on <see cref="Seq{T}"/>.
/// </summary>
/// <remarks>
/// <c>Repeat</c>; <c>Select</c>, <c>Where</c>, <c>Concat</c>, <c>Zip</c> and <c>Merge</c> have
/// left for their own folders as they came under test. These exist so that
/// a scenario writes them
/// exactly as the Rx.NET test it was migrated from does. Each method builds one node of the query
/// description (one node type per overload, in this folder), and each target turns that node into
/// its own operator call through the matching member of <see cref="ISeqVisitor"/>.
/// </remarks>
public static class SeqExtensions
{
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

// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information. 

namespace Tests.System.Reactive.Shared;

/// <summary>The <c>Concat</c> overloads, as static and extension methods on <see cref="Seq"/>.</summary>
/// <remarks>
/// Where an Rx.NET test writes <c>Observable.Concat(xs, ys, zs)</c>, <c>sources.Concat()</c> or
/// <c>o1.Concat(o2)</c>, a shared scenario writes the same with <see cref="Seq"/> in place of
/// <c>Observable</c>. One node type per overload, in this folder: two sources, any number of
/// sources, an enumerable of sources, a sequence of sequences, and a sequence of tasks.
/// <c>Concat</c> was plumbing before it came under test (the first two forms, which other
/// scenarios compose with); it keeps that role from here.
/// </remarks>
public static partial class Seq
{
    /// <summary>Describes <c>first.Concat(second)</c>.</summary>
    /// <typeparam name="T">The type of the elements in the source sequences.</typeparam>
    /// <param name="first">First observable sequence.</param>
    /// <param name="second">Second observable sequence.</param>
    /// <remarks>
    /// Builds a <see cref="ConcatSeq{T}"/>, which each target materializes through
    /// <see cref="ISeqVisitor.Concat{T}(ConcatSeq{T})"/>.
    /// </remarks>
    public static Seq<T> Concat<T>(this Seq<T> first, Seq<T> second) =>
        new ConcatSeq<T>(first, second);

    /// <summary>Describes <c>sources.Concat()</c> over a nested sequence.</summary>
    /// <typeparam name="T">The type of the elements in the source sequences.</typeparam>
    /// <param name="sources">Observable sequence of inner observable sequences.</param>
    /// <remarks>
    /// Builds a <see cref="ConcatNestedSeq{T}"/>, which each target materializes through
    /// <see cref="ISeqVisitor.ConcatNested{T}(ConcatNestedSeq{T})"/>.
    /// </remarks>
    public static Seq<T> Concat<T>(this Seq<Seq<T>> sources) => new ConcatNestedSeq<T>(sources);

    /// <summary>
    /// Describes <c>Seq.Concat(source1, source2, ...)</c> over any number of sources.
    /// </summary>
    /// <typeparam name="T">The type of the elements in the source sequences.</typeparam>
    /// <param name="sources">Observable sequences to concatenate.</param>
    /// <remarks>
    /// Builds a <see cref="ConcatParamsSeq{T}"/>, which each target materializes through
    /// <see cref="ISeqVisitor.ConcatParams{T}(ConcatParamsSeq{T})"/>.
    /// </remarks>
    public static Seq<T> Concat<T>(params Seq<T>[] sources) => new ConcatParamsSeq<T>(sources);

    /// <summary>Describes <c>sources.Concat()</c> over an enumerable of sources.</summary>
    /// <typeparam name="T">The type of the elements in the source sequences.</typeparam>
    /// <param name="sources">Observable sequences to concatenate.</param>
    /// <remarks>
    /// Builds a <see cref="ConcatEnumerableSeq{T}"/>, which each target materializes through
    /// <see cref="ISeqVisitor.ConcatEnumerable{T}(ConcatEnumerableSeq{T})"/>.
    /// </remarks>
    public static Seq<T> Concat<T>(this IEnumerable<Seq<T>> sources) =>
        new ConcatEnumerableSeq<T>(sources);

    /// <summary>Describes <c>sources.Concat()</c> over a sequence of tasks.</summary>
    /// <typeparam name="T">The type of the results produced by the tasks.</typeparam>
    /// <param name="sources">Observable sequence of tasks.</param>
    /// <remarks>
    /// Builds a <see cref="ConcatTasksSeq{T}"/>, which each target materializes through
    /// <see cref="ISeqVisitor.ConcatTasks{T}(ConcatTasksSeq{T})"/>.
    /// </remarks>
    public static Seq<T> Concat<T>(this Seq<Task<T>> sources) => new ConcatTasksSeq<T>(sources);
}

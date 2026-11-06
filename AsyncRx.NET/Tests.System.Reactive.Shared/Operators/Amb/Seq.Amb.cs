// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information. 

namespace Tests.System.Reactive.Shared;

/// <summary>The <c>Amb</c> overloads.</summary>
/// <remarks>
/// The <c>params</c> form is on <see cref="Seq"/> because the Rx.NET tests call it statically,
/// as <c>Observable.Amb(o1, o2, o3)</c>; the two-source and enumerable forms are
/// extensions, as Rx.NET's are. Each builds one node of the query description, which each
/// target turns into its own <c>Amb</c> call through the matching <see cref="ISeqVisitor"/>
/// member in <c>ISeqVisitor.Amb.cs</c>.
/// </remarks>
public static partial class Seq
{
    /// <summary>
    /// Describes <c>first.Amb(second)</c>: whichever reacts first is the result and the other
    /// is unsubscribed.
    /// </summary>
    /// <typeparam name="T">The type of the elements in the source sequences.</typeparam>
    /// <param name="first">The first sequence.</param>
    /// <param name="second">The second sequence.</param>
    /// <remarks>
    /// Builds a <see cref="AmbSeq{T}"/>, which each target materializes through
    /// <see cref="ISeqVisitor.Amb{T}(AmbSeq{T})"/>.
    /// </remarks>
    public static Seq<T> Amb<T>(this Seq<T> first, Seq<T> second) =>
        new AmbSeq<T>(first, second);

    /// <summary>
    /// Describes <c>Seq.Amb(source1, source2, ...)</c>: whichever reacts first is the result and
    /// the others are unsubscribed.
    /// </summary>
    /// <typeparam name="T">The type of the elements in the source sequences.</typeparam>
    /// <param name="sources">The sources.</param>
    /// <remarks>
    /// Builds a <see cref="AmbParamsSeq{T}"/>, which each target materializes through
    /// <see cref="ISeqVisitor.AmbParams{T}(AmbParamsSeq{T})"/>.
    /// </remarks>
    public static Seq<T> Amb<T>(params Seq<T>[] sources) => new AmbParamsSeq<T>(sources);

    /// <summary>Describes <c>sources.Amb()</c> over an enumerable of sources.</summary>
    /// <typeparam name="T">The type of the elements in the source sequences.</typeparam>
    /// <param name="sources">The sources.</param>
    /// <remarks>
    /// Builds a <see cref="AmbEnumerableSeq{T}"/>, which each target materializes through
    /// <see cref="ISeqVisitor.AmbEnumerable{T}(AmbEnumerableSeq{T})"/>.
    /// </remarks>
    public static Seq<T> Amb<T>(this IEnumerable<Seq<T>> sources) =>
        new AmbEnumerableSeq<T>(sources);
}

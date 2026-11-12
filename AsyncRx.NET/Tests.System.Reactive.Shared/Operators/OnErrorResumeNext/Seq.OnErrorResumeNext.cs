// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information. 

namespace Tests.System.Reactive.Shared;

/// <summary>The <c>OnErrorResumeNext</c> overloads.</summary>
/// <remarks>
/// The <c>params</c> form is on <see cref="Seq"/> because the Rx.NET tests call it statically,
/// as <c>Observable.OnErrorResumeNext(o1, o2, o3)</c>; the two-source and enumerable forms are
/// extensions, as Rx.NET's are. Each builds one node of the query description, which each
/// target turns into its own <c>OnErrorResumeNext</c> call through the matching <see cref="ISeqVisitor"/>
/// member in <c>ISeqVisitor.OnErrorResumeNext.cs</c>.
/// </remarks>
public static partial class Seq
{
    /// <summary>
    /// Describes <c>first.OnErrorResumeNext(second)</c>: the second sequence follows the first
    /// whether it completes or fails.
    /// </summary>
    /// <typeparam name="T">The type of the elements in the source sequences.</typeparam>
    /// <param name="first">The first sequence.</param>
    /// <param name="second">The second sequence.</param>
    /// <remarks>
    /// Builds a <see cref="OnErrorResumeNextSeq{T}"/>, which each target materializes through
    /// <see cref="ISeqVisitor.OnErrorResumeNext{T}(OnErrorResumeNextSeq{T})"/>.
    /// </remarks>
    public static Seq<T> OnErrorResumeNext<T>(this Seq<T> first, Seq<T> second) =>
        new OnErrorResumeNextSeq<T>(first, second);

    /// <summary>
    /// Describes <c>Seq.OnErrorResumeNext(source1, source2, ...)</c>: each sequence follows the
    /// previous whether it completes or fails.
    /// </summary>
    /// <typeparam name="T">The type of the elements in the source sequences.</typeparam>
    /// <param name="sources">The sources.</param>
    /// <remarks>
    /// Builds a <see cref="OnErrorResumeNextParamsSeq{T}"/>, which each target materializes through
    /// <see cref="ISeqVisitor.OnErrorResumeNextParams{T}(OnErrorResumeNextParamsSeq{T})"/>.
    /// </remarks>
    public static Seq<T> OnErrorResumeNext<T>(params Seq<T>[] sources) =>
        new OnErrorResumeNextParamsSeq<T>(sources);

    /// <summary>
    /// Describes <c>sources.OnErrorResumeNext()</c> over an enumerable of sources.
    /// </summary>
    /// <typeparam name="T">The type of the elements in the source sequences.</typeparam>
    /// <param name="sources">The sources.</param>
    /// <remarks>
    /// Builds a <see cref="OnErrorResumeNextEnumerableSeq{T}"/>, which each target materializes through
    /// <see cref="ISeqVisitor.OnErrorResumeNextEnumerable{T}(OnErrorResumeNextEnumerableSeq{T})"/>.
    /// </remarks>
    public static Seq<T> OnErrorResumeNext<T>(this IEnumerable<Seq<T>> sources) =>
        new OnErrorResumeNextEnumerableSeq<T>(sources);
}

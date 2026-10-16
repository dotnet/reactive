// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information. 

using System.Runtime.CompilerServices;

namespace Tests.System.Reactive.Shared;

/// <summary>
/// The <c>Multicast</c> overloads, as extension methods on <see cref="Seq{T}"/>.
/// </summary>
/// <remarks>
/// The first operator whose input is a subject: a <see cref="SubjectSeq{T}"/> argument maps to the
/// target's subject type through the bridge, as a <see cref="Seq{T}"/> maps to its observable
/// type, and a subject factory's result does likewise. Each method builds one node of the query
/// description (one node type per overload, in this folder), and each target turns that node
/// into its own <c>Multicast</c> call through the matching <see cref="ISeqVisitor"/> member in
/// <c>ISeqVisitor.Multicast.cs</c>.
/// </remarks>
public static class MulticastExtensions
{
    /// <summary>Describes <c>source.Multicast(subject)</c>.</summary>
    /// <typeparam name="T">The type of the elements in the source sequence.</typeparam>
    /// <param name="source">
    /// Source sequence whose elements will be pushed into the specified subject.
    /// </param>
    /// <param name="subject">Subject to push source elements into.</param>
    /// <remarks>
    /// Builds a <see cref="MulticastSeq{T}"/>, which each target materializes through
    /// <see cref="ISeqVisitor.Multicast{T}(MulticastSeq{T})"/>.
    /// </remarks>
    public static ConnectableSeq<T> Multicast<T>(this Seq<T> source, SubjectSeq<T> subject) =>
        new MulticastSeq<T>(source, subject);

    /// <summary>Describes <c>source.Multicast(subjectSelector, selector)</c>.</summary>
    /// <typeparam name="T">The type of the elements in the source sequence.</typeparam>
    /// <typeparam name="TResult">The type of the elements in the result sequence.</typeparam>
    /// <param name="source">
    /// Source sequence which will be multicasted in the specified selector function.
    /// </param>
    /// <param name="subjectSelector">
    /// Factory function to create an intermediate subject through which the source sequence's
    /// elements will be multicast to the selector function.
    /// </param>
    /// <param name="selector">
    /// Selector function which can use the multicasted source sequence subject to the policies
    /// enforced by the created subject.
    /// </param>
    /// <param name="text">
    /// Supplied by the compiler (the source text of the selector), for printing the query in
    /// diagnostics; do not pass it.
    /// </param>
    /// <remarks>
    /// Builds a <see cref="MulticastSelectorSeq{T, TResult}"/>, which each target materializes
    /// through
    /// <see cref="ISeqVisitor.MulticastSelector{T, TResult}(MulticastSelectorSeq{T, TResult})"/>.
    /// </remarks>
    public static Seq<TResult> Multicast<T, TResult>(
        this Seq<T> source,
        Func<SubjectSeq<T>> subjectSelector,
        Func<Seq<T>, Seq<TResult>> selector,
        [CallerArgumentExpression(nameof(selector))] string text = "") =>
        new MulticastSelectorSeq<T, TResult>(source, subjectSelector, selector, text);
}

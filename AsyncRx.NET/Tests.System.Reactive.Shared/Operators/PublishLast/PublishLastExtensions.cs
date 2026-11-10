// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information. 

using System.Runtime.CompilerServices;

namespace Tests.System.Reactive.Shared;

/// <summary>
/// The <c>PublishLast</c> overloads, as extension methods on <see cref="Seq{T}"/>.
/// </summary>
/// <remarks>
/// As <c>Publish</c>, over an async subject: only the source's last value reaches subscribers,
/// on completion. The form without a selector returns a <see cref="ConnectableSeq{T}"/>, which a
/// scenario connects through <see cref="ConnectableSeq{T}.ConnectAsync"/>; the form with a
/// selector hands the selector the published sequence as a leaf and returns an ordinary
/// sequence. Each method builds one node of the query description (one node type per overload,
/// in this folder), and each target turns that node into its own <c>PublishLast</c> call
/// through the matching <see cref="ISeqVisitor"/> member in <c>ISeqVisitor.PublishLast.cs</c>.
/// </remarks>
public static class PublishLastExtensions
{
    /// <summary>Describes <c>source.PublishLast()</c>.</summary>
    /// <typeparam name="T">The type of the elements in the source sequence.</typeparam>
    /// <param name="source">
    /// Source sequence whose elements will be multicasted through a single shared subscription.
    /// </param>
    /// <remarks>
    /// Builds a <see cref="PublishLastSeq{T}"/>, which each target materializes through
    /// <see cref="ISeqVisitor.PublishLast{T}(PublishLastSeq{T})"/>.
    /// </remarks>
    public static ConnectableSeq<T> PublishLast<T>(this Seq<T> source) =>
        new PublishLastSeq<T>(source);

    /// <summary>Describes <c>source.PublishLast(selector)</c>.</summary>
    /// <typeparam name="T">The type of the elements in the source sequence.</typeparam>
    /// <typeparam name="TResult">The type of the elements in the result sequence.</typeparam>
    /// <param name="source">
    /// Source sequence whose elements will be multicasted through a single shared subscription.
    /// </param>
    /// <param name="selector">
    /// Selector function which can use the multicasted source sequence as many times as needed,
    /// without causing multiple subscriptions to the source sequence. Subscribers to the given
    /// source will only receive the last notification of the source.
    /// </param>
    /// <param name="text">
    /// Supplied by the compiler (the source text of the selector), for printing the query in
    /// diagnostics; do not pass it.
    /// </param>
    /// <remarks>
    /// Builds a <see cref="PublishLastSelectorSeq{T, TResult}"/>, which each target materializes
    /// through
    /// <see cref="ISeqVisitor.PublishLastSelector{T, TResult}(PublishLastSelectorSeq{T, TResult})"/>.
    /// </remarks>
    public static Seq<TResult> PublishLast<T, TResult>(
        this Seq<T> source,
        Func<Seq<T>, Seq<TResult>> selector,
        [CallerArgumentExpression(nameof(selector))] string text = "") =>
        new PublishLastSelectorSeq<T, TResult>(source, selector, text);
}

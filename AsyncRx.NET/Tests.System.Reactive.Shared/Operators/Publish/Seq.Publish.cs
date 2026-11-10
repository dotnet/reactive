// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information. 

using System.Runtime.CompilerServices;

namespace Tests.System.Reactive.Shared;

/// <summary>The <c>Publish</c> overloads.</summary>
/// <remarks>
/// On <see cref="Seq"/> because the Rx.NET tests call them both as extension methods and
/// statically (<c>Observable.Publish(source, selector, initialValue)</c>). The two forms without
/// a selector return a <see cref="ConnectableSeq{T}"/>, which a scenario connects through
/// <see cref="ConnectableSeq{T}.ConnectAsync"/>; the two with a selector hand the selector the
/// published sequence as a leaf and return an ordinary sequence. Each method builds one node of
/// the query description (one node type per overload, in this folder), and each target turns
/// that node into its own <c>Publish</c> call through the matching <see cref="ISeqVisitor"/>
/// member in <c>ISeqVisitor.Publish.cs</c>.
/// </remarks>
public static partial class Seq
{
    /// <summary>Describes <c>source.Publish()</c>.</summary>
    /// <typeparam name="T">The type of the elements in the source sequence.</typeparam>
    /// <param name="source">
    /// Source sequence whose elements will be multicasted through a single shared subscription.
    /// </param>
    /// <remarks>
    /// Builds a <see cref="PublishSeq{T}"/>, which each target materializes through
    /// <see cref="ISeqVisitor.Publish{T}(PublishSeq{T})"/>.
    /// </remarks>
    public static ConnectableSeq<T> Publish<T>(this Seq<T> source) => new PublishSeq<T>(source);

    /// <summary>Describes <c>source.Publish(initialValue)</c>.</summary>
    /// <typeparam name="T">The type of the elements in the source sequence.</typeparam>
    /// <param name="source">
    /// Source sequence whose elements will be multicasted through a single shared subscription.
    /// </param>
    /// <param name="initialValue">Initial value received by observers upon subscription.</param>
    /// <remarks>
    /// Builds a <see cref="PublishInitialSeq{T}"/>, which each target materializes through
    /// <see cref="ISeqVisitor.PublishInitial{T}(PublishInitialSeq{T})"/>.
    /// </remarks>
    public static ConnectableSeq<T> Publish<T>(this Seq<T> source, T initialValue) =>
        new PublishInitialSeq<T>(source, initialValue);

    /// <summary>Describes <c>source.Publish(selector)</c>.</summary>
    /// <typeparam name="T">The type of the elements in the source sequence.</typeparam>
    /// <typeparam name="TResult">The type of the elements in the result sequence.</typeparam>
    /// <param name="source">
    /// Source sequence whose elements will be multicasted through a single shared subscription.
    /// </param>
    /// <param name="selector">
    /// Selector function which can use the multicasted source sequence as many times as needed,
    /// without causing multiple subscriptions to the source sequence.
    /// </param>
    /// <param name="text">
    /// Supplied by the compiler (the source text of the selector), for printing the query in
    /// diagnostics; do not pass it.
    /// </param>
    /// <remarks>
    /// Builds a <see cref="PublishSelectorSeq{T, TResult}"/>, which each target materializes
    /// through
    /// <see cref="ISeqVisitor.PublishSelector{T, TResult}(PublishSelectorSeq{T, TResult})"/>.
    /// </remarks>
    public static Seq<TResult> Publish<T, TResult>(
        this Seq<T> source,
        Func<Seq<T>, Seq<TResult>> selector,
        [CallerArgumentExpression(nameof(selector))] string text = "") =>
        new PublishSelectorSeq<T, TResult>(source, selector, text);

    /// <summary>Describes <c>source.Publish(selector, initialValue)</c>.</summary>
    /// <typeparam name="T">The type of the elements in the source sequence.</typeparam>
    /// <typeparam name="TResult">The type of the elements in the result sequence.</typeparam>
    /// <param name="source">
    /// Source sequence whose elements will be multicasted through a single shared subscription.
    /// </param>
    /// <param name="selector">
    /// Selector function which can use the multicasted source sequence as many times as needed,
    /// without causing multiple subscriptions to the source sequence.
    /// </param>
    /// <param name="initialValue">Initial value received by observers upon subscription.</param>
    /// <param name="text">
    /// Supplied by the compiler (the source text of the selector), for printing the query in
    /// diagnostics; do not pass it.
    /// </param>
    /// <remarks>
    /// Builds a <see cref="PublishSelectorInitialSeq{T, TResult}"/>, which each target
    /// materializes through
    /// <see cref="ISeqVisitor.PublishSelectorInitial{T, TResult}(PublishSelectorInitialSeq{T, TResult})"/>.
    /// </remarks>
    public static Seq<TResult> Publish<T, TResult>(
        this Seq<T> source,
        Func<Seq<T>, Seq<TResult>> selector,
        T initialValue,
        [CallerArgumentExpression(nameof(selector))] string text = "") =>
        new PublishSelectorInitialSeq<T, TResult>(source, selector, initialValue, text);
}

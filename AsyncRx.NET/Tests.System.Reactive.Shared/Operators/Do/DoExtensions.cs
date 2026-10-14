// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information. 

using System.Runtime.CompilerServices;

namespace Tests.System.Reactive.Shared;

/// <summary>The <c>Do</c> overloads, as extension methods on <see cref="Seq{T}"/>.</summary>
/// <remarks>
/// These exist so that a shared scenario writes <c>xs.Do(x =&gt; ...)</c>,
/// <c>xs.Do(onNext, onCompleted)</c> or <c>xs.Do(ObserverRef.Create(...))</c> exactly as the
/// Rx.NET test it was migrated from does. One node type per overload, in this folder: the
/// handler forms carry each handler's source text, captured by the compiler, so that a
/// diagnostic can print the query; the observer form takes the model's
/// <see cref="ObserverRef{T}"/>, which the bridge turns into the target's own observer. Each
/// target turns a node into its own <c>Do</c> call through the matching
/// <see cref="ISeqVisitor"/> member in <c>ISeqVisitor.Do.cs</c>.
/// </remarks>
public static class DoExtensions
{
    /// <summary>Describes <c>source.Do(onNext)</c>.</summary>
    /// <typeparam name="T">The type of the elements in the source sequence.</typeparam>
    /// <param name="source">Source sequence.</param>
    /// <param name="onNext">Action to invoke for each element in the observable sequence.</param>
    /// <param name="text">
    /// Supplied by the compiler (the source text of <paramref name="onNext"/>), for
    /// printing the query in diagnostics; do not pass it.
    /// </param>
    /// <remarks>
    /// Builds a <see cref="DoSeq{T}"/>, which each target materializes through
    /// <see cref="ISeqVisitor.Do{T}(DoSeq{T})"/>.
    /// </remarks>
    public static Seq<T> Do<T>(
        this Seq<T> source,
        Action<T> onNext,
        [CallerArgumentExpression(nameof(onNext))] string text = "") =>
        new DoSeq<T>(source, onNext, text);

    /// <summary>Describes <c>source.Do(onNext, onCompleted)</c>.</summary>
    /// <typeparam name="T">The type of the elements in the source sequence.</typeparam>
    /// <param name="source">Source sequence.</param>
    /// <param name="onNext">Action to invoke for each element in the observable sequence.</param>
    /// <param name="onCompleted">
    /// Action to invoke upon graceful termination of the observable sequence.
    /// </param>
    /// <param name="nextText">
    /// Supplied by the compiler (the source text of <paramref name="onNext"/>), for
    /// printing the query in diagnostics; do not pass it.
    /// </param>
    /// <param name="completedText">
    /// Supplied by the compiler (the source text of <paramref name="onCompleted"/>), for
    /// printing the query in diagnostics; do not pass it.
    /// </param>
    /// <remarks>
    /// Builds a <see cref="DoNextCompletedSeq{T}"/>, which each target materializes through
    /// <see cref="ISeqVisitor.DoNextCompleted{T}(DoNextCompletedSeq{T})"/>.
    /// </remarks>
    public static Seq<T> Do<T>(
        this Seq<T> source,
        Action<T> onNext,
        Action onCompleted,
        [CallerArgumentExpression(nameof(onNext))] string nextText = "",
        [CallerArgumentExpression(nameof(onCompleted))] string completedText = "") =>
        new DoNextCompletedSeq<T>(source, onNext, onCompleted, nextText, completedText);

    /// <summary>Describes <c>source.Do(onNext, onError)</c>.</summary>
    /// <typeparam name="T">The type of the elements in the source sequence.</typeparam>
    /// <param name="source">Source sequence.</param>
    /// <param name="onNext">Action to invoke for each element in the observable sequence.</param>
    /// <param name="onError">
    /// Action to invoke upon exceptional termination of the observable sequence.
    /// </param>
    /// <param name="nextText">
    /// Supplied by the compiler (the source text of <paramref name="onNext"/>), for
    /// printing the query in diagnostics; do not pass it.
    /// </param>
    /// <param name="errorText">
    /// Supplied by the compiler (the source text of <paramref name="onError"/>), for
    /// printing the query in diagnostics; do not pass it.
    /// </param>
    /// <remarks>
    /// Builds a <see cref="DoNextErrorSeq{T}"/>, which each target materializes through
    /// <see cref="ISeqVisitor.DoNextError{T}(DoNextErrorSeq{T})"/>.
    /// </remarks>
    public static Seq<T> Do<T>(
        this Seq<T> source,
        Action<T> onNext,
        Action<Exception> onError,
        [CallerArgumentExpression(nameof(onNext))] string nextText = "",
        [CallerArgumentExpression(nameof(onError))] string errorText = "") =>
        new DoNextErrorSeq<T>(source, onNext, onError, nextText, errorText);

    /// <summary>Describes <c>source.Do(onNext, onError, onCompleted)</c>.</summary>
    /// <typeparam name="T">The type of the elements in the source sequence.</typeparam>
    /// <param name="source">Source sequence.</param>
    /// <param name="onNext">Action to invoke for each element in the observable sequence.</param>
    /// <param name="onError">
    /// Action to invoke upon exceptional termination of the observable sequence.
    /// </param>
    /// <param name="onCompleted">
    /// Action to invoke upon graceful termination of the observable sequence.
    /// </param>
    /// <param name="nextText">
    /// Supplied by the compiler (the source text of <paramref name="onNext"/>), for
    /// printing the query in diagnostics; do not pass it.
    /// </param>
    /// <param name="errorText">
    /// Supplied by the compiler (the source text of <paramref name="onError"/>), for
    /// printing the query in diagnostics; do not pass it.
    /// </param>
    /// <param name="completedText">
    /// Supplied by the compiler (the source text of <paramref name="onCompleted"/>), for
    /// printing the query in diagnostics; do not pass it.
    /// </param>
    /// <remarks>
    /// Builds a <see cref="DoNextErrorCompletedSeq{T}"/>, which each target materializes
    /// through <see cref="ISeqVisitor.DoNextErrorCompleted{T}(DoNextErrorCompletedSeq{T})"/>.
    /// </remarks>
    public static Seq<T> Do<T>(
        this Seq<T> source,
        Action<T> onNext,
        Action<Exception> onError,
        Action onCompleted,
        [CallerArgumentExpression(nameof(onNext))] string nextText = "",
        [CallerArgumentExpression(nameof(onError))] string errorText = "",
        [CallerArgumentExpression(nameof(onCompleted))] string completedText = "") =>
        new DoNextErrorCompletedSeq<T>(
            source, onNext, onError, onCompleted, nextText, errorText, completedText);

    /// <summary>Describes <c>source.Do(observer)</c>.</summary>
    /// <typeparam name="T">The type of the elements in the source sequence.</typeparam>
    /// <param name="source">Source sequence.</param>
    /// <param name="observer">
    /// Observer whose methods to invoke as part of the source sequence's observation.
    /// </param>
    /// <param name="text">
    /// Supplied by the compiler (the source text of <paramref name="observer"/>), for
    /// printing the query in diagnostics; do not pass it.
    /// </param>
    /// <remarks>
    /// Builds a <see cref="DoObserverSeq{T}"/>, which each target materializes through
    /// <see cref="ISeqVisitor.DoObserver{T}(DoObserverSeq{T})"/>.
    /// </remarks>
    public static Seq<T> Do<T>(
        this Seq<T> source,
        ObserverRef<T> observer,
        [CallerArgumentExpression(nameof(observer))] string text = "") =>
        new DoObserverSeq<T>(source, observer, text);
}

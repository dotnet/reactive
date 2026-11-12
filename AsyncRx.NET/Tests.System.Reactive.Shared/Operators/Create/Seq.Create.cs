// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information. 

using System.Runtime.CompilerServices;

namespace Tests.System.Reactive.Shared;

/// <summary>The <c>Create</c> overloads.</summary>
/// <remarks>
/// <para>
/// On <see cref="Seq"/> because the Rx.NET tests call them statically, as
/// <c>Observable.Create&lt;int&gt;(o =&gt; ...)</c>. The callback is the one place where a shared
/// scenario drives an observer itself, so it is async-shaped: it receives an
/// <see cref="ObserverRef{T}"/>, awaits its <c>OnNextAsync</c> and the rest, and returns its
/// action or disposable through a <see cref="ValueTask{TResult}"/>, or only a task, whose
/// completion then completes the sequence. The six overloads are Rx.NET's six task-based
/// <c>Create</c> overloads, and both targets realize them with those semantics: the subscribe
/// returns when the callback returns, what the callback's task produces becomes the
/// subscription's resource when it arrives, a failure of the callback is the sequence's error,
/// and the token, where the callback takes one, is cancelled when the subscriber unsubscribes.
/// Rx.NET's target calls those overloads; AsyncRx.NET's calls its own, which all take the
/// token, and ignores it for the forms that do not.
/// </para>
/// <para>
/// Each method builds one node of the query description (one node type per overload, in this
/// folder), and each target turns that node into its own <c>Create</c> call through the matching
/// <see cref="ISeqVisitor"/> member in <c>ISeqVisitor.Create.cs</c>.
/// </para>
/// </remarks>
public static partial class Seq
{
    /// <summary>Describes <c>Seq.Create(subscribe)</c> where the callback returns an action.</summary>
    /// <typeparam name="T">The type of the elements in the produced sequence.</typeparam>
    /// <param name="subscribe">
    /// Implementation of the resulting observable sequence's subscribe method, returning an
    /// action to run on disposal, or null for none.
    /// </param>
    /// <param name="text">
    /// Supplied by the compiler (the source text of the callback), for printing the query in
    /// diagnostics; do not pass it.
    /// </param>
    /// <remarks>
    /// Builds a <see cref="CreateSeq{T}"/>, which each target materializes through
    /// <see cref="ISeqVisitor.Create{T}(CreateSeq{T})"/>.
    /// </remarks>
    public static Seq<T> Create<T>(
        Func<ObserverRef<T>, ValueTask<Action?>> subscribe,
        [CallerArgumentExpression(nameof(subscribe))] string text = "") =>
        new CreateSeq<T>(subscribe, text);

    /// <summary>
    /// Describes <c>Seq.Create(subscribe)</c> for a callback that returns a disposable.
    /// </summary>
    /// <typeparam name="T">The type of the elements in the produced sequence.</typeparam>
    /// <param name="subscribe">
    /// Implementation of the resulting observable sequence's subscribe method, returning the
    /// subscription's disposable, or null for none.
    /// </param>
    /// <param name="text">
    /// Supplied by the compiler (the source text of the callback), for printing the query in
    /// diagnostics; do not pass it.
    /// </param>
    /// <remarks>
    /// Builds a <see cref="CreateDisposableSeq{T}"/>, which each target materializes through
    /// <see cref="ISeqVisitor.CreateDisposable{T}(CreateDisposableSeq{T})"/>.
    /// </remarks>
    public static Seq<T> Create<T>(
        Func<ObserverRef<T>, ValueTask<IAsyncDisposable?>> subscribe,
        [CallerArgumentExpression(nameof(subscribe))] string text = "") =>
        new CreateDisposableSeq<T>(subscribe, text);

    /// <summary>
    /// Describes <c>Seq.Create(subscribe)</c> for a callback that takes a token and returns an
    /// action.
    /// </summary>
    /// <typeparam name="T">The type of the elements in the produced sequence.</typeparam>
    /// <param name="subscribe">
    /// Implementation of the resulting observable sequence's subscribe method, handed a token
    /// that is cancelled on unsubscription, and returning an action to run on disposal, or null
    /// for none.
    /// </param>
    /// <param name="text">
    /// Supplied by the compiler (the source text of the callback), for printing the query in
    /// diagnostics; do not pass it.
    /// </param>
    /// <remarks>
    /// Builds a <see cref="CreateWithTokenSeq{T}"/>, which each target materializes through
    /// <see cref="ISeqVisitor.CreateWithToken{T}(CreateWithTokenSeq{T})"/>.
    /// </remarks>
    public static Seq<T> Create<T>(
        Func<ObserverRef<T>, CancellationToken, ValueTask<Action?>> subscribe,
        [CallerArgumentExpression(nameof(subscribe))] string text = "") =>
        new CreateWithTokenSeq<T>(subscribe, text);

    /// <summary>
    /// Describes <c>Seq.Create(subscribe)</c> for a callback that takes a token and returns a
    /// disposable.
    /// </summary>
    /// <typeparam name="T">The type of the elements in the produced sequence.</typeparam>
    /// <param name="subscribe">
    /// Implementation of the resulting observable sequence's subscribe method, handed a token
    /// that is cancelled on unsubscription, and returning the subscription's disposable, or
    /// null for none.
    /// </param>
    /// <param name="text">
    /// Supplied by the compiler (the source text of the callback), for printing the query in
    /// diagnostics; do not pass it.
    /// </param>
    /// <remarks>
    /// Builds a <see cref="CreateDisposableWithTokenSeq{T}"/>, which each target materializes
    /// through
    /// <see cref="ISeqVisitor.CreateDisposableWithToken{T}(CreateDisposableWithTokenSeq{T})"/>.
    /// </remarks>
    public static Seq<T> Create<T>(
        Func<ObserverRef<T>, CancellationToken, ValueTask<IAsyncDisposable?>> subscribe,
        [CallerArgumentExpression(nameof(subscribe))] string text = "") =>
        new CreateDisposableWithTokenSeq<T>(subscribe, text);

    /// <summary>
    /// Describes <c>Seq.Create(subscribe)</c> for a callback whose completion completes the
    /// sequence.
    /// </summary>
    /// <typeparam name="T">The type of the elements in the produced sequence.</typeparam>
    /// <param name="subscribe">
    /// Implementation of the resulting observable sequence's subscribe method, which also
    /// produces the elements: when its task completes the sequence completes, and if it faults
    /// the sequence fails.
    /// </param>
    /// <param name="text">
    /// Supplied by the compiler (the source text of the callback), for printing the query in
    /// diagnostics; do not pass it.
    /// </param>
    /// <remarks>
    /// Builds a <see cref="CreateAutoCompletingSeq{T}"/>, which each target materializes through
    /// <see cref="ISeqVisitor.CreateAutoCompleting{T}(CreateAutoCompletingSeq{T})"/>.
    /// </remarks>
    public static Seq<T> Create<T>(
        Func<ObserverRef<T>, ValueTask> subscribe,
        [CallerArgumentExpression(nameof(subscribe))] string text = "") =>
        new CreateAutoCompletingSeq<T>(subscribe, text);

    /// <summary>
    /// Describes <c>Seq.Create(subscribe)</c> for a callback that takes a token and whose
    /// completion completes the sequence.
    /// </summary>
    /// <typeparam name="T">The type of the elements in the produced sequence.</typeparam>
    /// <param name="subscribe">
    /// Implementation of the resulting observable sequence's subscribe method, handed a token
    /// that is cancelled on unsubscription, which also produces the elements: when its task
    /// completes the sequence completes, and if it faults the sequence fails.
    /// </param>
    /// <param name="text">
    /// Supplied by the compiler (the source text of the callback), for printing the query in
    /// diagnostics; do not pass it.
    /// </param>
    /// <remarks>
    /// Builds a <see cref="CreateAutoCompletingWithTokenSeq{T}"/>, which each target
    /// materializes through
    /// <see cref="ISeqVisitor.CreateAutoCompletingWithToken{T}(CreateAutoCompletingWithTokenSeq{T})"/>.
    /// </remarks>
    public static Seq<T> Create<T>(
        Func<ObserverRef<T>, CancellationToken, ValueTask> subscribe,
        [CallerArgumentExpression(nameof(subscribe))] string text = "") =>
        new CreateAutoCompletingWithTokenSeq<T>(subscribe, text);
}

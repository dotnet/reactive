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
/// action or disposable through a <see cref="ValueTask{TResult}"/>. On Rx.NET every await in it
/// completes synchronously and the target runs it as a synchronous subscribe; on AsyncRx.NET it
/// is the subscribe function itself.
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
}

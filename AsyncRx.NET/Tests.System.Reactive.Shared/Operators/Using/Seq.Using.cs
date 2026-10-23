// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information. 

using System.Runtime.CompilerServices;

namespace Tests.System.Reactive.Shared;

/// <summary>The <c>Using</c> overload, as a static method on <see cref="Seq"/>.</summary>
/// <remarks>
/// On <see cref="Seq"/> because the Rx.NET tests call it statically, as
/// <c>Observable.Using(() =&gt; ..., d =&gt; ...)</c>; a shared scenario writes the same with
/// <see cref="Seq"/> in place of <c>Observable</c>. It builds one node of the query
/// description, which each target turns into its own <c>Using</c> call through the matching
/// <see cref="ISeqVisitor"/> member in <c>ISeqVisitor.Using.cs</c>.
/// </remarks>
public static partial class Seq
{
    /// <summary>Describes <c>Seq.Using(resourceFactory, observableFactory)</c>.</summary>
    /// <typeparam name="T">The type of the elements in the produced sequence.</typeparam>
    /// <typeparam name="TResource">
    /// The type of the resource used during the generation of the resulting sequence. Needs to
    /// implement <see cref="IDisposable"/>.
    /// </typeparam>
    /// <param name="resourceFactory">Factory function to obtain a resource object.</param>
    /// <param name="observableFactory">
    /// Factory function to obtain an observable sequence that depends on the obtained resource.
    /// </param>
    /// <param name="resourceText">
    /// Supplied by the compiler (the source text of <paramref name="resourceFactory"/>), for
    /// printing the query in diagnostics; do not pass it.
    /// </param>
    /// <param name="observableText">
    /// Supplied by the compiler (the source text of <paramref name="observableFactory"/>), for
    /// printing the query in diagnostics; do not pass it.
    /// </param>
    /// <remarks>
    /// Builds a <see cref="UsingSeq{T, TResource}"/>, which each target materializes through
    /// <see cref="ISeqVisitor.Using{T, TResource}(UsingSeq{T, TResource})"/>.
    /// </remarks>
    public static Seq<T> Using<T, TResource>(
        Func<TResource> resourceFactory,
        Func<TResource, Seq<T>> observableFactory,
        [CallerArgumentExpression(nameof(resourceFactory))] string resourceText = "",
        [CallerArgumentExpression(nameof(observableFactory))] string observableText = "")
        where TResource : IDisposable =>
        new UsingSeq<T, TResource>(
            resourceFactory, observableFactory, resourceText, observableText);

    /// <summary>
    /// Describes <c>Seq.Using(resourceFactory, observableFactory)</c> with asynchronous
    /// factories that take a token.
    /// </summary>
    /// <typeparam name="T">The type of the elements in the produced sequence.</typeparam>
    /// <typeparam name="TResource">
    /// The type of the resource used during the generation of the resulting sequence. Needs to
    /// implement <see cref="IDisposable"/>.
    /// </typeparam>
    /// <param name="resourceFactory">
    /// Asynchronous factory function to obtain a resource object, handed a token that is
    /// cancelled if the subscriber unsubscribes while it runs.
    /// </param>
    /// <param name="observableFactory">
    /// Asynchronous factory function to obtain an observable sequence that depends on the
    /// obtained resource, handed a token that is cancelled the same way.
    /// </param>
    /// <param name="resourceText">
    /// Supplied by the compiler (the source text of <paramref name="resourceFactory"/>), for
    /// printing the query in diagnostics; do not pass it.
    /// </param>
    /// <param name="observableText">
    /// Supplied by the compiler (the source text of <paramref name="observableFactory"/>), for
    /// printing the query in diagnostics; do not pass it.
    /// </param>
    /// <remarks>
    /// Rx.NET's asynchronous <c>Using</c> overload, AsyncRx.NET's <c>UsingAsync</c>. Builds a
    /// <see cref="UsingAsyncSeq{T, TResource}"/>, which each target materializes through
    /// <see cref="ISeqVisitor.UsingAsync{T, TResource}(UsingAsyncSeq{T, TResource})"/>.
    /// </remarks>
    public static Seq<T> Using<T, TResource>(
        Func<CancellationToken, ValueTask<TResource>> resourceFactory,
        Func<TResource, CancellationToken, ValueTask<Seq<T>>> observableFactory,
        [CallerArgumentExpression(nameof(resourceFactory))] string resourceText = "",
        [CallerArgumentExpression(nameof(observableFactory))] string observableText = "")
        where TResource : IDisposable =>
        new UsingAsyncSeq<T, TResource>(
            resourceFactory, observableFactory, resourceText, observableText);
}

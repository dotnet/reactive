// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information. 

namespace Tests.System.Reactive.Shared;

/// <summary>
/// Describes <c>Seq.Using(resourceFactory, observableFactory)</c> with asynchronous factories
/// that take a token.
/// </summary>
/// <remarks>
/// Built by the asynchronous <c>Using</c> overload of <see cref="Seq"/>; materialized by each
/// target through <see cref="ISeqVisitor.UsingAsync{T, TResource}(UsingAsyncSeq{T, TResource})"/>.
/// The token each factory receives is cancelled if the subscriber unsubscribes while that factory
/// is still running.
/// </remarks>
public sealed class UsingAsyncSeq<T, TResource>(
    Func<CancellationToken, ValueTask<TResource>> resourceFactory,
    Func<TResource, CancellationToken, ValueTask<Seq<T>>> observableFactory,
    string resourceText,
    string observableText)
    : Seq<T>
    where TResource : IDisposable
{
    /// <summary>Asynchronous factory function to obtain a resource object.</summary>
    public Func<CancellationToken, ValueTask<TResource>> ResourceFactory => resourceFactory;

    /// <summary>
    /// Asynchronous factory function to obtain an observable sequence that depends on the
    /// obtained resource.
    /// </summary>
    public Func<TResource, CancellationToken, ValueTask<Seq<T>>> ObservableFactory =>
        observableFactory;

    /// <inheritdoc/>
    protected override Realized<Seq<T>> AcceptCore(ISeqVisitor visitor) =>
        visitor.UsingAsync(this);

    /// <inheritdoc/>
    public override string ToString() => $"Seq.Using({resourceText}, {observableText})";
}

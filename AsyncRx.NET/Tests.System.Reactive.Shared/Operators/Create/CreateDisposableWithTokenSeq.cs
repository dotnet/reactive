// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information. 

namespace Tests.System.Reactive.Shared;

/// <summary>Describes <c>Seq.Create(subscribe)</c> where the callback takes a token and returns a disposable.</summary>
/// <remarks>
/// Built by <see cref="Seq.Create{T}(Func{ObserverRef{T}, CancellationToken, ValueTask{IAsyncDisposable}}, string)"/>; materialized by each target through <see cref="ISeqVisitor.CreateDisposableWithToken{T}(CreateDisposableWithTokenSeq{T})"/>. The callback
/// receives an <see cref="ObserverRef{T}"/> and the token cancelled on
/// unsubscription, and returns the subscription's disposable, or null for none.
/// </remarks>
public sealed class CreateDisposableWithTokenSeq<T>(
    Func<ObserverRef<T>, CancellationToken, ValueTask<IAsyncDisposable?>> subscribe,
    string text) : Seq<T>
{
    /// <summary>Implementation of the resulting observable sequence's subscribe method.</summary>
    public Func<ObserverRef<T>, CancellationToken, ValueTask<IAsyncDisposable?>> Subscribe =>
        subscribe;

    /// <inheritdoc/>
    protected override Realized<Seq<T>> AcceptCore(ISeqVisitor visitor) =>
        visitor.CreateDisposableWithToken(this);

    /// <inheritdoc/>
    public override string ToString() => $"Seq.Create({text})";
}

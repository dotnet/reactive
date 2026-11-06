// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information. 

namespace Tests.System.Reactive.Shared;

/// <summary>Describes <c>Seq.Create(subscribe)</c> where the callback takes a token and returns an action.</summary>
/// <remarks>
/// Built by <see cref="Seq.Create{T}(Func{ObserverRef{T}, CancellationToken, ValueTask{Action}}, string)"/>; materialized by each target through <see cref="ISeqVisitor.CreateWithToken{T}(CreateWithTokenSeq{T})"/>. The callback
/// receives an <see cref="ObserverRef{T}"/> and the token cancelled on
/// unsubscription, and returns the action to run on disposal, or null for none.
/// </remarks>
public sealed class CreateWithTokenSeq<T>(
    Func<ObserverRef<T>, CancellationToken, ValueTask<Action?>> subscribe,
    string text) : Seq<T>
{
    /// <summary>Implementation of the resulting observable sequence's subscribe method.</summary>
    public Func<ObserverRef<T>, CancellationToken, ValueTask<Action?>> Subscribe => subscribe;

    /// <inheritdoc/>
    protected override Realized<Seq<T>> AcceptCore(ISeqVisitor visitor) =>
        visitor.CreateWithToken(this);

    /// <inheritdoc/>
    public override string ToString() => $"Seq.Create({text})";
}

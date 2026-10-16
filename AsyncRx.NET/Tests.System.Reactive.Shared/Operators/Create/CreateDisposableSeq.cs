// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information. 

namespace Tests.System.Reactive.Shared;

/// <summary>Describes <c>Seq.Create(subscribe)</c> with a disposable-returning callback.</summary>
/// <remarks>
/// Built by
/// <see cref="Seq.Create{T}(Func{ObserverRef{T}, ValueTask{IAsyncDisposable}}, string)"/>;
/// materialized by each target through
/// <see cref="ISeqVisitor.CreateDisposable{T}(CreateDisposableSeq{T})"/>. The callback receives
/// an <see cref="ObserverRef{T}"/> and returns the subscription's disposable, or null for none.
/// </remarks>
public sealed class CreateDisposableSeq<T>(
    Func<ObserverRef<T>, ValueTask<IAsyncDisposable?>> subscribe,
    string text) : Seq<T>
{
    /// <summary>
    /// Implementation of the resulting observable sequence's subscribe method, returning the
    /// subscription's disposable.
    /// </summary>
    public Func<ObserverRef<T>, ValueTask<IAsyncDisposable?>> Subscribe => subscribe;

    /// <inheritdoc/>
    protected override Realized<Seq<T>> AcceptCore(ISeqVisitor visitor) => visitor.CreateDisposable(this);

    /// <inheritdoc/>
    public override string ToString() => $"Seq.Create({text})";
}

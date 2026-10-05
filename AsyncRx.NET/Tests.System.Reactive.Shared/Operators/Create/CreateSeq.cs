// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information. 

namespace Tests.System.Reactive.Shared;

/// <summary>Describes <c>Seq.Create(subscribe)</c> where the callback returns an action.</summary>
/// <remarks>
/// Built by
/// <see cref="Seq.Create{T}(Func{ObserverRef{T}, ValueTask{Action}}, string)"/>; materialized
/// by each target through <see cref="ISeqVisitor.Create{T}(CreateSeq{T})"/>. The callback
/// receives an <see cref="ObserverRef{T}"/> and returns the action to run on disposal, or null
/// for none.
/// </remarks>
public sealed class CreateSeq<T>(Func<ObserverRef<T>, ValueTask<Action?>> subscribe, string text) : Seq<T>
{
    /// <summary>
    /// Implementation of the resulting observable sequence's subscribe method, returning an
    /// action to run on disposal.
    /// </summary>
    public Func<ObserverRef<T>, ValueTask<Action?>> Subscribe => subscribe;

    /// <inheritdoc/>
    protected override Realized<Seq<T>> AcceptCore(ISeqVisitor visitor) => visitor.Create(this);

    /// <inheritdoc/>
    public override string ToString() => $"Seq.Create({text})";
}

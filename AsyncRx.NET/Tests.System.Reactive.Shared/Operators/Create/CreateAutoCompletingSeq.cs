// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information. 

namespace Tests.System.Reactive.Shared;

/// <summary>Describes <c>Seq.Create(subscribe)</c> where the callback's completion completes the sequence.</summary>
/// <remarks>
/// Built by <see cref="Seq.Create{T}(Func{ObserverRef{T}, ValueTask}, string)"/>; materialized by each target through <see cref="ISeqVisitor.CreateAutoCompleting{T}(CreateAutoCompletingSeq{T})"/>. The callback
/// receives an <see cref="ObserverRef{T}"/> and returns only a task: the
/// sequence completes when it does, and fails if it faults.
/// </remarks>
public sealed class CreateAutoCompletingSeq<T>(
    Func<ObserverRef<T>, ValueTask> subscribe,
    string text) : Seq<T>
{
    /// <summary>Implementation of the resulting observable sequence's subscribe method.</summary>
    public Func<ObserverRef<T>, ValueTask> Subscribe => subscribe;

    /// <inheritdoc/>
    protected override Realized<Seq<T>> AcceptCore(ISeqVisitor visitor) =>
        visitor.CreateAutoCompleting(this);

    /// <inheritdoc/>
    public override string ToString() => $"Seq.Create({text})";
}

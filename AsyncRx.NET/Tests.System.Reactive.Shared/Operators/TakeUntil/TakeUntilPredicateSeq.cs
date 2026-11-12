// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information. 

namespace Tests.System.Reactive.Shared;

/// <summary>Describes <c>source.TakeUntil(stopPredicate)</c>.</summary>
/// <remarks>
/// Built by
/// <see cref="TakeUntilExtensions.TakeUntil{T}(Seq{T}, Func{T, bool}, string)"/>; materialized by
/// each target through <see cref="ISeqVisitor.TakeUntilPredicate{T}(TakeUntilPredicateSeq{T})"/>.
/// </remarks>
public sealed class TakeUntilPredicateSeq<T>(
    Seq<T> source,
    Func<T, bool> stopPredicate,
    string text)
    : Seq<T>
{
    /// <summary>The source sequence to relay elements of.</summary>
    public Seq<T> Source => source;

    /// <summary>
    /// Called after each upstream item has been emitted with that upstream item and should return
    /// <c>true</c> to indicate the sequence should complete.
    /// </summary>
    public Func<T, bool> StopPredicate => stopPredicate;

    /// <inheritdoc/>
    protected override Realized<Seq<T>> AcceptCore(ISeqVisitor visitor) =>
        visitor.TakeUntilPredicate(this);

    /// <inheritdoc/>
    public override string ToString() => $"{source}.TakeUntil({text})";
}

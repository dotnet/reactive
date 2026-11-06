// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information. 

namespace Tests.System.Reactive.Shared;

/// <summary>Describes <c>Seq.While(condition, source)</c>.</summary>
/// <typeparam name="T">The type of the elements in the source sequence.</typeparam>
/// <remarks>
/// Built by <see cref="Seq.While{T}(Func{bool}, Seq{T}, string)"/>; materialized by each target
/// through <see cref="ISeqVisitor.While{T}(WhileSeq{T})"/>.
/// </remarks>
public sealed class WhileSeq<T>(Func<bool> condition, Seq<T> source, string text) : Seq<T>
{
    /// <summary>Condition evaluated upon subscription and after each repetition.</summary>
    public Func<bool> Condition => condition;

    /// <summary>Source to repeat as long as the condition function evaluates to true.</summary>
    public Seq<T> Source => source;

    /// <inheritdoc/>
    protected override Realized<Seq<T>> AcceptCore(ISeqVisitor visitor) => visitor.While(this);

    /// <inheritdoc/>
    public override string ToString() => $"Seq.While({text}, {source})";
}

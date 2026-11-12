// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information. 

namespace Tests.System.Reactive.Shared;

/// <summary>Describes <c>Seq.DoWhile(source, condition)</c>.</summary>
/// <typeparam name="T">The type of the elements in the source sequence.</typeparam>
/// <remarks>
/// Built by <see cref="Seq.DoWhile{T}(Seq{T}, Func{bool}, string)"/>; materialized by each
/// target through <see cref="ISeqVisitor.DoWhile{T}(DoWhileSeq{T})"/>.
/// </remarks>
public sealed class DoWhileSeq<T>(Seq<T> source, Func<bool> condition, string text) : Seq<T>
{
    /// <summary>Source to repeat as long as the condition function evaluates to true.</summary>
    public Seq<T> Source => source;

    /// <summary>Condition evaluated after each repetition of the source.</summary>
    public Func<bool> Condition => condition;

    /// <inheritdoc/>
    protected override Realized<Seq<T>> AcceptCore(ISeqVisitor visitor) => visitor.DoWhile(this);

    /// <inheritdoc/>
    public override string ToString() => $"Seq.DoWhile({source}, {text})";
}

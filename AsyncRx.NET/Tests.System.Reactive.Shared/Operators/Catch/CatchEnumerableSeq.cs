// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information. 

namespace Tests.System.Reactive.Shared;

/// <summary>Describes <c>sources.Catch()</c> over an enumerable of sources.</summary>
/// <remarks>
/// Built by <see cref="Seq.Catch{T}(IEnumerable{Seq{T}})"/>; materialized by each target
/// through <see cref="ISeqVisitor.CatchEnumerable{T}(CatchEnumerableSeq{T})"/>. The enumerable is
/// enumerated by the operator, as it moves from a failed source to the next, and by nothing
/// else: a scenario may be counting its enumerations (<c>MockEnumerable</c>) or expecting the
/// operator to be the one that trips over it (<c>RogueEnumerable</c>), so the description's
/// text names only a materialized collection's elements.
/// </remarks>
public sealed class CatchEnumerableSeq<T>(IEnumerable<Seq<T>> sources) : Seq<T>
{
    /// <summary>Observable sequences to catch exceptions for.</summary>
    public IEnumerable<Seq<T>> Sources => sources;

    /// <inheritdoc/>
    protected override Realized<Seq<T>> AcceptCore(ISeqVisitor visitor) =>
        visitor.CatchEnumerable(this);

    /// <inheritdoc/>
    public override string ToString() =>
        sources is ICollection<Seq<T>>
            ? $"Seq.Catch([{string.Join(", ", sources)}])"
            : $"Seq.Catch({sources.GetType().Name})";
}

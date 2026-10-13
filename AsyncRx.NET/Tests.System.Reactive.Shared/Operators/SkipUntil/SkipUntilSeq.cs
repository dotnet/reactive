// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information. 

namespace Tests.System.Reactive.Shared;

/// <summary>Describes <c>source.SkipUntil(other)</c>.</summary>
/// <remarks>
/// Built by <see cref="SkipUntilExtensions.SkipUntil{T, TOther}(Seq{T}, Seq{TOther})"/>;
/// materialized by each target through
/// <see cref="ISeqVisitor.SkipUntil{T, TOther}(SkipUntilSeq{T, TOther})"/>.
/// </remarks>
public sealed class SkipUntilSeq<T, TOther>(Seq<T> source, Seq<TOther> other) : Seq<T>
{
    /// <summary>Source sequence to propagate elements for.</summary>
    public Seq<T> Source => source;

    /// <summary>
    /// Observable sequence that triggers propagation of elements of the source sequence.
    /// </summary>
    public Seq<TOther> Other => other;

    /// <inheritdoc/>
    protected override Realized<Seq<T>> AcceptCore(ISeqVisitor visitor) => visitor.SkipUntil(this);

    /// <inheritdoc/>
    public override string ToString() => $"{source}.SkipUntil({other})";
}

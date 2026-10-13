// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information. 

namespace Tests.System.Reactive.Shared;

/// <summary>Describes <c>source.Min()</c> over elements of any type.</summary>
/// <remarks>
/// Built by <see cref="Seq.Min{T}(Seq{T})"/>;
/// materialized by each target through <see cref="ISeqVisitor.Min{T}(MinSeq{T})"/>.
/// </remarks>
public sealed class MinSeq<T>(Seq<T> source) : Seq<T>
{
    /// <summary>An observable sequence to determine the minimum element of.</summary>
    public Seq<T> Source => source;

    /// <inheritdoc/>
    protected override Realized<Seq<T>> AcceptCore(ISeqVisitor visitor) => visitor.Min(this);

    /// <inheritdoc/>
    public override string ToString() => $"{source}.Min()";
}

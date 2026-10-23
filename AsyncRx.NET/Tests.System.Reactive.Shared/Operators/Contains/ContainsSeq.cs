// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information. 

namespace Tests.System.Reactive.Shared;

/// <summary>Describes <c>source.Contains(value)</c>.</summary>
/// <remarks>
/// Built by <see cref="ContainsExtensions.Contains{T}(Seq{T}, T)"/>;
/// materialized by each target through <see cref="ISeqVisitor.Contains{T}(ContainsSeq{T})"/>.
/// </remarks>
public sealed class ContainsSeq<T>(Seq<T> source, T value)
    : Seq<bool>
{
    /// <summary>An observable sequence in which to locate a value.</summary>
    public Seq<T> Source => source;

    /// <summary>The value to locate in the source sequence.</summary>
    public T Value => value;

    /// <inheritdoc/>
    protected override Realized<Seq<bool>> AcceptCore(ISeqVisitor visitor) =>
        visitor.Contains(this);

    /// <inheritdoc/>
    public override string ToString() => $"{source}.Contains({value})";
}

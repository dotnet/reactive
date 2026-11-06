// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information. 

namespace Tests.System.Reactive.Shared;

/// <summary>Describes <c>source.StartWith(values)</c>.</summary>
/// <remarks>
/// Built by <see cref="StartWithExtensions.StartWith{T}(Seq{T}, T[])"/>; materialized by each
/// target through <see cref="ISeqVisitor.StartWith{T}(StartWithSeq{T})"/>.
/// </remarks>
public sealed class StartWithSeq<T>(Seq<T> source, T[] values) : Seq<T>
{
    /// <summary>Source sequence to prepend values to.</summary>
    public Seq<T> Source => source;

    /// <summary>Values to prepend to the specified sequence.</summary>
    public T[] Values => values;

    /// <inheritdoc/>
    protected override Realized<Seq<T>> AcceptCore(ISeqVisitor visitor) => visitor.StartWith(this);

    /// <inheritdoc/>
    public override string ToString() => $"{source}.StartWith({string.Join(", ", values)})";
}

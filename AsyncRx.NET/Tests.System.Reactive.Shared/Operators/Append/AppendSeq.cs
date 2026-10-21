// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information. 

namespace Tests.System.Reactive.Shared;

/// <summary>Describes <c>source.Append(value)</c>.</summary>
/// <remarks>
/// Built by <see cref="Seq.Append{T}(Seq{T}, T)"/>; materialized by each target through
/// <see cref="ISeqVisitor.Append{T}(AppendSeq{T})"/>.
/// </remarks>
public sealed class AppendSeq<T>(Seq<T> source, T value) : Seq<T>
{
    /// <summary>Source sequence to append the value to.</summary>
    public Seq<T> Source => source;

    /// <summary>Value to append to the specified sequence.</summary>
    public T Value => value;

    /// <inheritdoc/>
    protected override Realized<Seq<T>> AcceptCore(ISeqVisitor visitor) => visitor.Append(this);

    /// <inheritdoc/>
    public override string ToString() => $"{source}.Append({value})";
}

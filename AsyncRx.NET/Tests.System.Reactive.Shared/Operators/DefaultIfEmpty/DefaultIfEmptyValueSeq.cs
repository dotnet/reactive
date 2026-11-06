// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information. 

namespace Tests.System.Reactive.Shared;

/// <summary>Describes <c>source.DefaultIfEmpty(defaultValue)</c>.</summary>
/// <remarks>
/// Built by <see cref="DefaultIfEmptyExtensions.DefaultIfEmpty{T}(Seq{T}, T)"/>; materialized
/// by each target through
/// <see cref="ISeqVisitor.DefaultIfEmptyValue{T}(DefaultIfEmptyValueSeq{T})"/>.
/// </remarks>
public sealed class DefaultIfEmptyValueSeq<T>(Seq<T> source, T defaultValue) : Seq<T>
{
    /// <summary>The sequence to return a default value for if it is empty.</summary>
    public Seq<T> Source => source;

    /// <summary>The value to return if the sequence is empty.</summary>
    public T DefaultValue => defaultValue;

    /// <inheritdoc/>
    protected override Realized<Seq<T>> AcceptCore(ISeqVisitor visitor) =>
        visitor.DefaultIfEmptyValue(this);

    /// <inheritdoc/>
    public override string ToString() => $"{source}.DefaultIfEmpty({defaultValue})";
}

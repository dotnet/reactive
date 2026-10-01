// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information. 

namespace Tests.System.Reactive.Shared;

/// <summary>Describes <c>source.Skip(count)</c>.</summary>
public sealed class SkipSeq<T>(Seq<T> source, int count) : Seq<T>
{
    /// <summary>The sequence to skip elements from.</summary>
    public Seq<T> Source => source;

    /// <summary>The number of elements to skip before returning the remaining elements.</summary>
    public int Count => count;

    /// <inheritdoc/>
    public override Realized<Seq<T>> Accept(ISeqVisitor visitor) => visitor.Skip(this);

    /// <inheritdoc/>
    public override string ToString() => $"{source}.Skip({count})";
}

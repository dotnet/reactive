// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information. 

namespace Tests.System.Reactive.Shared;

/// <summary>Describes <c>source.IgnoreElements()</c>.</summary>
/// <typeparam name="T">The type of the elements in the source sequence.</typeparam>
/// <remarks>
/// Built by <see cref="IgnoreElementsExtensions.IgnoreElements{T}(Seq{T})"/>; materialized by
/// each target through <see cref="ISeqVisitor.IgnoreElements{T}(IgnoreElementsSeq{T})"/>.
/// </remarks>
public sealed class IgnoreElementsSeq<T>(Seq<T> source) : Seq<T>
{
    /// <summary>Source sequence.</summary>
    public Seq<T> Source => source;

    /// <inheritdoc/>
    protected override Realized<Seq<T>> AcceptCore(ISeqVisitor visitor) =>
        visitor.IgnoreElements(this);

    /// <inheritdoc/>
    public override string ToString() => $"{source}.IgnoreElements()";
}

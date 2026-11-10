// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information. 

namespace Tests.System.Reactive.Shared;

/// <summary>Describes <c>source.AsObservable()</c>.</summary>
/// <typeparam name="T">The type of the elements in the source sequence.</typeparam>
/// <remarks>
/// Built by <see cref="AsObservableExtensions.AsObservable{T}(Seq{T})"/>; materialized by each
/// target through <see cref="ISeqVisitor.AsObservable{T}(AsObservableSeq{T})"/>.
/// </remarks>
public sealed class AsObservableSeq<T>(Seq<T> source) : Seq<T>
{
    /// <summary>An observable sequence to hide identity of.</summary>
    public Seq<T> Source => source;

    /// <inheritdoc/>
    protected override Realized<Seq<T>> AcceptCore(ISeqVisitor visitor) =>
        visitor.AsObservable(this);

    /// <inheritdoc/>
    public override string ToString() => $"{source}.AsObservable()";
}

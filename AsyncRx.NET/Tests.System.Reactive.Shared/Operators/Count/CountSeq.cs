// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information. 

namespace Tests.System.Reactive.Shared;

/// <summary>Describes <c>source.Count()</c>.</summary>
/// <remarks>
/// Built by <see cref="CountExtensions.Count{T}(Seq{T})"/>;
/// materialized by each target through <see cref="ISeqVisitor.Count{T}(CountSeq{T})"/>.
/// </remarks>
public sealed class CountSeq<T>(Seq<T> source) : Seq<int>
{
    /// <summary>An observable sequence that contains elements to be counted.</summary>
    public Seq<T> Source => source;

    /// <inheritdoc/>
    protected override Realized<Seq<int>> AcceptCore(ISeqVisitor visitor) =>
        visitor.Count(this);

    /// <inheritdoc/>
    public override string ToString() => $"{source}.Count()";
}

// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information. 

namespace Tests.System.Reactive.Shared;

/// <summary>Describes <c>Seq.Empty&lt;T&gt;()</c>.</summary>
/// <remarks>
/// Built by <see cref="Seq.Empty{T}"/>;
/// materialized by each target through <see cref="ISeqVisitor.Empty{T}(EmptySeq{T})"/>.
/// </remarks>
public sealed class EmptySeq<T> : Seq<T>
{
    /// <inheritdoc/>
    protected override Realized<Seq<T>> AcceptCore(ISeqVisitor visitor) => visitor.Empty(this);

    /// <inheritdoc/>
    public override string ToString() => $"Seq.Empty<{typeof(T).Name}>()";
}

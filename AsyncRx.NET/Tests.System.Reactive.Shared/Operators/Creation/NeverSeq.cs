// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information. 

namespace Tests.System.Reactive.Shared;

/// <summary>Describes <c>Seq.Never()</c>.</summary>
/// <remarks>
/// Built by <see cref="Seq.Never{T}"/>; materialized by each target through
/// <see cref="ISeqVisitor.Never{T}(NeverSeq{T})"/>.
/// </remarks>
public sealed class NeverSeq<T> : Seq<T>
{
    /// <inheritdoc/>
    protected override Realized<Seq<T>> AcceptCore(ISeqVisitor visitor) => visitor.Never(this);

    /// <inheritdoc/>
    public override string ToString() => "Seq.Never()";
}

// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information. 

namespace Tests.System.Reactive.Shared;

/// <summary>Describes <c>source.Finally(finallyAction)</c>.</summary>
/// <remarks>
/// Built by <see cref="FinallyExtensions.Finally{T}(Seq{T}, Action, string)"/>;
/// materialized by each target through <see cref="ISeqVisitor.Finally{T}(FinallySeq{T})"/>.
/// </remarks>
public sealed class FinallySeq<T>(Seq<T> source, Action finallyAction, string text) : Seq<T>
{
    /// <summary>Source sequence.</summary>
    public Seq<T> Source => source;

    /// <summary>Action to invoke after the source observable sequence terminates.</summary>
    public Action FinallyAction => finallyAction;

    /// <inheritdoc/>
    protected override Realized<Seq<T>> AcceptCore(ISeqVisitor visitor) =>
        visitor.Finally(this);

    /// <inheritdoc/>
    public override string ToString() => $"{source}.Finally({text})";
}

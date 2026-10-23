// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information. 

namespace Tests.System.Reactive.Shared;

/// <summary>Describes <c>first.Catch(second)</c>.</summary>
/// <remarks>
/// Built by <see cref="Seq.Catch{T}(Seq{T}, Seq{T})"/>;
/// materialized by each target through <see cref="ISeqVisitor.Catch{T}(CatchSeq{T})"/>.
/// </remarks>
public sealed class CatchSeq<T>(Seq<T> first, Seq<T> second) : Seq<T>
{
    /// <summary>First observable sequence whose exception (if any) is caught.</summary>
    public Seq<T> First => first;

    /// <summary>
    /// Second observable sequence used to produce results when an error occurred in the first
    /// sequence.
    /// </summary>
    public Seq<T> Second => second;

    /// <inheritdoc/>
    protected override Realized<Seq<T>> AcceptCore(ISeqVisitor visitor) => visitor.Catch(this);

    /// <inheritdoc/>
    public override string ToString() => $"{first}.Catch({second})";
}

// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information. 

namespace Tests.System.Reactive.Shared;

/// <summary>Describes <c>first.OnErrorResumeNext(second)</c>.</summary>
/// <remarks>
/// Built by <see cref="Seq.OnErrorResumeNext{T}(Seq{T}, Seq{T})"/>; materialized by each target through
/// <see cref="ISeqVisitor.OnErrorResumeNext{T}(OnErrorResumeNextSeq{T})"/>.
/// </remarks>
public sealed class OnErrorResumeNextSeq<T>(Seq<T> first, Seq<T> second) : Seq<T>
{
    /// <summary>The first sequence.</summary>
    public Seq<T> First => first;

    /// <summary>The second sequence.</summary>
    public Seq<T> Second => second;

    /// <inheritdoc/>
    protected override Realized<Seq<T>> AcceptCore(ISeqVisitor visitor) =>
        visitor.OnErrorResumeNext(this);

    /// <inheritdoc/>
    public override string ToString() => $"{first}.OnErrorResumeNext({second})";
}

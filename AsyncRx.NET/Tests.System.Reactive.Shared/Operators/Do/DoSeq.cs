// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information. 

namespace Tests.System.Reactive.Shared;

/// <summary>Describes <c>source.Do(onNext)</c>.</summary>
/// <remarks>
/// Built by <see cref="DoExtensions.Do{T}(Seq{T}, Action{T}, string)"/>;
/// materialized by each target through <see cref="ISeqVisitor.Do{T}(DoSeq{T})"/>.
/// </remarks>
public sealed class DoSeq<T>(
    Seq<T> source,
    Action<T> onNext,
    string text)
    : Seq<T>
{
    /// <summary>Source sequence.</summary>
    public Seq<T> Source => source;

    /// <summary>Action to invoke for each element in the observable sequence.</summary>
    public Action<T> OnNext => onNext;

    /// <inheritdoc/>
    protected override Realized<Seq<T>> AcceptCore(ISeqVisitor visitor) =>
        visitor.Do(this);

    /// <inheritdoc/>
    public override string ToString() => $"{source}.Do({text})";
}

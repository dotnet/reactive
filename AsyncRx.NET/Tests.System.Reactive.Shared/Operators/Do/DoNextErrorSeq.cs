// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information. 

namespace Tests.System.Reactive.Shared;

/// <summary>Describes <c>source.Do(onNext, onError)</c>.</summary>
/// <remarks>
/// Built by <see cref="DoExtensions.Do{T}(Seq{T}, Action{T}, Action{Exception}, string, string)"/>;
/// materialized by each target through <see cref="ISeqVisitor.DoNextError{T}(DoNextErrorSeq{T})"/>.
/// </remarks>
public sealed class DoNextErrorSeq<T>(
    Seq<T> source,
    Action<T> onNext,
    Action<Exception> onError,
    string nextText,
    string errorText)
    : Seq<T>
{
    /// <summary>Source sequence.</summary>
    public Seq<T> Source => source;

    /// <summary>Action to invoke for each element in the observable sequence.</summary>
    public Action<T> OnNext => onNext;

    /// <summary>
    /// Action to invoke upon exceptional termination of the observable sequence.
    /// </summary>
    public Action<Exception> OnError => onError;

    /// <inheritdoc/>
    protected override Realized<Seq<T>> AcceptCore(ISeqVisitor visitor) =>
        visitor.DoNextError(this);

    /// <inheritdoc/>
    public override string ToString() => $"{source}.Do({nextText}, {errorText})";
}

// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information. 

namespace Tests.System.Reactive.Shared;

public partial interface ISeqVisitor
{
    /// <summary>Materializes a <see cref="DoSeq{T}"/> as the target's own <c>Do(onNext)</c>.</summary>
    /// <remarks>Built by <see cref="DoExtensions.Do{T}(Seq{T}, Action{T}, string)"/>.</remarks>
    Realized<Seq<T>> Do<T>(DoSeq<T> seq);

    /// <summary>
    /// Materializes a <see cref="DoNextCompletedSeq{T}"/> as the target's own
    /// <c>Do(onNext, onCompleted)</c>.
    /// </summary>
    /// <remarks>
    /// Built by <see cref="DoExtensions.Do{T}(Seq{T}, Action{T}, Action, string, string)"/>.
    /// </remarks>
    Realized<Seq<T>> DoNextCompleted<T>(DoNextCompletedSeq<T> seq);

    /// <summary>
    /// Materializes a <see cref="DoNextErrorSeq{T}"/> as the target's own
    /// <c>Do(onNext, onError)</c>.
    /// </summary>
    /// <remarks>
    /// Built by
    /// <see cref="DoExtensions.Do{T}(Seq{T}, Action{T}, Action{Exception}, string, string)"/>.
    /// </remarks>
    Realized<Seq<T>> DoNextError<T>(DoNextErrorSeq<T> seq);

    /// <summary>
    /// Materializes a <see cref="DoNextErrorCompletedSeq{T}"/> as the target's own
    /// <c>Do(onNext, onError, onCompleted)</c>.
    /// </summary>
    /// <remarks>
    /// Built by
    /// <see cref="DoExtensions.Do{T}(Seq{T}, Action{T}, Action{Exception}, Action, string, string, string)"/>.
    /// </remarks>
    Realized<Seq<T>> DoNextErrorCompleted<T>(DoNextErrorCompletedSeq<T> seq);

    /// <summary>
    /// Materializes a <see cref="DoObserverSeq{T}"/> as the target's own <c>Do(observer)</c>.
    /// </summary>
    /// <remarks>
    /// Built by <see cref="DoExtensions.Do{T}(Seq{T}, ObserverRef{T}, string)"/>.
    /// </remarks>
    Realized<Seq<T>> DoObserver<T>(DoObserverSeq<T> seq);
}

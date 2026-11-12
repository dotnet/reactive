// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information. 

namespace Tests.System.Reactive.Shared;

public partial interface ISeqVisitor
{
    /// <summary>
    /// Materializes a <see cref="CreateSeq{T}"/> as the target's own <c>Create</c> over a
    /// callback that returns an action.
    /// </summary>
    /// <remarks>
    /// Built by <see cref="Seq.Create{T}(Func{ObserverRef{T}, ValueTask{Action}}, string)"/>.
    /// </remarks>
    Realized<Seq<T>> Create<T>(CreateSeq<T> seq);

    /// <summary>
    /// Materializes a <see cref="CreateDisposableSeq{T}"/> as the target's own <c>Create</c>
    /// over a callback that returns a disposable.
    /// </summary>
    /// <remarks>
    /// Built by
    /// <see cref="Seq.Create{T}(Func{ObserverRef{T}, ValueTask{IAsyncDisposable}}, string)"/>.
    /// </remarks>
    Realized<Seq<T>> CreateDisposable<T>(CreateDisposableSeq<T> seq);

    /// <summary>
    /// Materializes a <see cref="CreateWithTokenSeq{T}"/> as the target's own <c>Create</c> over
    /// a callback that takes a token and returns an action.
    /// </summary>
    /// <remarks>
    /// Built by
    /// <see cref="Seq.Create{T}(Func{ObserverRef{T}, CancellationToken, ValueTask{Action}}, string)"/>.
    /// </remarks>
    Realized<Seq<T>> CreateWithToken<T>(CreateWithTokenSeq<T> seq);

    /// <summary>
    /// Materializes a <see cref="CreateDisposableWithTokenSeq{T}"/> as the target's own
    /// <c>Create</c> over a callback that takes a token and returns a disposable.
    /// </summary>
    /// <remarks>
    /// Built by
    /// <see cref="Seq.Create{T}(Func{ObserverRef{T}, CancellationToken, ValueTask{IAsyncDisposable}}, string)"/>.
    /// </remarks>
    Realized<Seq<T>> CreateDisposableWithToken<T>(CreateDisposableWithTokenSeq<T> seq);

    /// <summary>
    /// Materializes a <see cref="CreateAutoCompletingSeq{T}"/> as the target's own <c>Create</c>
    /// over a callback whose completion completes the sequence.
    /// </summary>
    /// <remarks>
    /// Built by <see cref="Seq.Create{T}(Func{ObserverRef{T}, ValueTask}, string)"/>.
    /// </remarks>
    Realized<Seq<T>> CreateAutoCompleting<T>(CreateAutoCompletingSeq<T> seq);

    /// <summary>
    /// Materializes a <see cref="CreateAutoCompletingWithTokenSeq{T}"/> as the target's own
    /// <c>Create</c> over a callback that takes a token and whose completion completes the
    /// sequence.
    /// </summary>
    /// <remarks>
    /// Built by
    /// <see cref="Seq.Create{T}(Func{ObserverRef{T}, CancellationToken, ValueTask}, string)"/>.
    /// </remarks>
    Realized<Seq<T>> CreateAutoCompletingWithToken<T>(CreateAutoCompletingWithTokenSeq<T> seq);
}

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
}

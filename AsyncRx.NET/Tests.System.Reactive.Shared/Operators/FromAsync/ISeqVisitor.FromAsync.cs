// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information. 

using System.Reactive;

namespace Tests.System.Reactive.Shared;

public partial interface ISeqVisitor
{
    /// <summary>
    /// Materializes a <see cref="FromAsyncSeq{TResult}"/> as the target's own <c>FromAsync(functionAsync[, scheduler])</c>
    /// over an asynchronous function.
    /// </summary>
    /// <remarks>Built by the <c>Seq.FromAsync</c> overloads in <c>Seq.FromAsync.cs</c>.</remarks>
    Realized<Seq<TResult>> FromAsync<TResult>(FromAsyncSeq<TResult> seq);

    /// <summary>
    /// Materializes a <see cref="FromAsyncCancelSeq{TResult}"/> as the target's own <c>FromAsync(functionAsync[, scheduler])</c>
    /// over an asynchronous function that takes a cancellation token.
    /// </summary>
    /// <remarks>Built by the <c>Seq.FromAsync</c> overloads in <c>Seq.FromAsync.cs</c>.</remarks>
    Realized<Seq<TResult>> FromAsyncCancel<TResult>(FromAsyncCancelSeq<TResult> seq);

    /// <summary>
    /// Materializes a <see cref="FromAsyncActionSeq"/> as the target's own <c>FromAsync(actionAsync[, scheduler])</c>
    /// over an asynchronous action.
    /// </summary>
    /// <remarks>Built by the <c>Seq.FromAsync</c> overloads in <c>Seq.FromAsync.cs</c>.</remarks>
    Realized<Seq<Unit>> FromAsyncAction(FromAsyncActionSeq seq);

    /// <summary>
    /// Materializes a <see cref="FromAsyncActionCancelSeq"/> as the target's own <c>FromAsync(actionAsync[, scheduler])</c>
    /// over an asynchronous action that takes a cancellation token.
    /// </summary>
    /// <remarks>Built by the <c>Seq.FromAsync</c> overloads in <c>Seq.FromAsync.cs</c>.</remarks>
    Realized<Seq<Unit>> FromAsyncActionCancel(FromAsyncActionCancelSeq seq);
}

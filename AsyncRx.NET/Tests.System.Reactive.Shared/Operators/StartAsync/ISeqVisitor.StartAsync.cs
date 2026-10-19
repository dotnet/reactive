// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information. 

using System.Reactive;

namespace Tests.System.Reactive.Shared;

public partial interface ISeqVisitor
{
    /// <summary>
    /// Materializes a <see cref="StartAsyncSeq{TResult}"/> as the target's own <c>StartAsync(functionAsync[, scheduler])</c>
    /// over an asynchronous function, run at the call.
    /// </summary>
    /// <remarks>Built by the <c>Seq.StartAsync</c> overloads in <c>Seq.StartAsync.cs</c>.</remarks>
    Realized<Seq<TResult>> StartAsync<TResult>(StartAsyncSeq<TResult> seq);

    /// <summary>
    /// Materializes a <see cref="StartAsyncCancelSeq{TResult}"/> as the target's own <c>StartAsync(functionAsync[, scheduler])</c>
    /// over an asynchronous function that takes a cancellation token, run at the call.
    /// </summary>
    /// <remarks>Built by the <c>Seq.StartAsync</c> overloads in <c>Seq.StartAsync.cs</c>.</remarks>
    Realized<Seq<TResult>> StartAsyncCancel<TResult>(StartAsyncCancelSeq<TResult> seq);

    /// <summary>
    /// Materializes a <see cref="StartAsyncActionSeq"/> as the target's own <c>StartAsync(actionAsync[, scheduler])</c>
    /// over an asynchronous action, run at the call.
    /// </summary>
    /// <remarks>Built by the <c>Seq.StartAsync</c> overloads in <c>Seq.StartAsync.cs</c>.</remarks>
    Realized<Seq<Unit>> StartAsyncAction(StartAsyncActionSeq seq);

    /// <summary>
    /// Materializes a <see cref="StartAsyncActionCancelSeq"/> as the target's own <c>StartAsync(actionAsync[, scheduler])</c>
    /// over an asynchronous action that takes a cancellation token, run at the call.
    /// </summary>
    /// <remarks>Built by the <c>Seq.StartAsync</c> overloads in <c>Seq.StartAsync.cs</c>.</remarks>
    Realized<Seq<Unit>> StartAsyncActionCancel(StartAsyncActionCancelSeq seq);
}

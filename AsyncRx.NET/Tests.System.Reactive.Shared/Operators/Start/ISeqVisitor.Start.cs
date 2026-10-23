// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information. 

using System.Reactive;

namespace Tests.System.Reactive.Shared;

public partial interface ISeqVisitor
{
    /// <summary>
    /// Materializes a <see cref="StartSeq{TResult}"/> as the target's own <c>Start(function[, scheduler])</c>
    /// over a function, run at the call.
    /// </summary>
    /// <remarks>Built by the <c>Seq.Start</c> overloads in <c>Seq.Start.cs</c>.</remarks>
    Realized<Seq<TResult>> Start<TResult>(StartSeq<TResult> seq);

    /// <summary>
    /// Materializes a <see cref="StartActionSeq"/> as the target's own <c>Start(action[, scheduler])</c>
    /// over an action, run at the call.
    /// </summary>
    /// <remarks>Built by the <c>Seq.Start</c> overloads in <c>Seq.Start.cs</c>.</remarks>
    Realized<Seq<Unit>> StartAction(StartActionSeq seq);
}

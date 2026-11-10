// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information. 

using System.Reactive;

namespace Tests.System.Reactive.Shared;

public partial interface ISeqVisitor
{
    /// <summary>
    /// Materializes a <see cref="TaskSeq{TResult}"/> as the target's own
    /// <c>ToObservable([scheduler])</c> over a <c>Task&lt;TResult&gt;</c>.
    /// </summary>
    /// <remarks>Built by the <c>ToSeq</c> overloads in <c>TaskExtensions.cs</c>.</remarks>
    Realized<Seq<TResult>> FromTask<TResult>(TaskSeq<TResult> seq);

    /// <summary>
    /// Materializes a <see cref="VoidTaskSeq"/> as the target's own
    /// <c>ToObservable([scheduler])</c> over a plain <c>Task</c>.
    /// </summary>
    /// <remarks>Built by the <c>ToSeq</c> overloads in <c>TaskExtensions.cs</c>.</remarks>
    Realized<Seq<Unit>> FromVoidTask(VoidTaskSeq seq);
}

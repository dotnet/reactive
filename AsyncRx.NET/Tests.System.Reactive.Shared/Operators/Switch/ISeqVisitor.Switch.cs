// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT License.
// See the LICENSE file in the project root for more information. 

namespace Tests.System.Reactive.Shared;

public partial interface ISeqVisitor
{
    /// <summary>Materializes a <see cref="SwitchSeq{T}"/> as the target's <c>Switch</c>.</summary>
    Realized<Seq<T>> Switch<T>(SwitchSeq<T> seq);

    /// <summary>
    /// Materializes a <see cref="SwitchTasksSeq{T}"/> as the target's own <c>Switch()</c> over a
    /// sequence of tasks.
    /// </summary>
    /// <remarks>Built by <see cref="SwitchExtensions.Switch{T}(Seq{Task{T}})"/>.</remarks>
    Realized<Seq<T>> SwitchTasks<T>(SwitchTasksSeq<T> seq);
}
